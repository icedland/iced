// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/encoder.hpp"
#include "iced_x86/instruction.hpp"
#include "iced_x86/internal/encoder_data.hpp"
#include "iced_x86/internal/encoder_ops_tables.hpp"
#include "iced_x86/internal/encoder_imm_sizes.hpp"
#include "iced_x86/internal/encoder_flags.hpp"
#include "iced_x86/internal/encoder_handler.hpp"
#include "iced_x86/iced_constants.hpp"

#include <stdexcept>
#include <format>

namespace iced_x86 {

using namespace internal;

// Get handlers table - defined in encoder_handlers.cpp
namespace internal {
const EncoderOpCodeHandler* const* get_handlers_table();
}

Encoder::Encoder(uint32_t bitness) : Encoder(bitness, 0) {}

Encoder::Encoder(uint32_t bitness, std::size_t capacity)
	: bitness_(bitness)
{
	if (bitness != 16 && bitness != 32 && bitness != 64) {
		throw std::invalid_argument("Invalid bitness: must be 16, 32, or 64");
	}
	
	if (capacity > 0) {
		buffer_.reserve(capacity);
	}
	
	opsize16_flags_ = (bitness != 16) ? EncoderFlags::P66 : 0;
	opsize32_flags_ = (bitness == 16) ? EncoderFlags::P66 : 0;
	adrsize16_flags_ = (bitness != 16) ? EncoderFlags::P67 : 0;
	adrsize32_flags_ = (bitness != 32) ? EncoderFlags::P67 : 0;
}

std::expected<std::size_t, EncodeError> Encoder::encode(const Instruction& instruction, uint64_t rip) noexcept {
	current_rip_ = rip;
	eip_ = static_cast<uint32_t>(rip);
	
	// Reset state
	encoder_flags_ = EncoderFlags::NONE;
	displ_size_ = DisplSize::NONE;
	imm_size_ = ImmSize::NONE;
	mod_rm_ = 0;
	sib_ = 0;
	error_message_.clear();
	
	auto code = instruction.code();
	handler_ = get_handlers_table()[static_cast<std::size_t>(code)];
	
	op_code_ = handler_->op_code;
	
	auto group_index = handler_->group_index;
	if (group_index >= 0) {
		encoder_flags_ |= EncoderFlags::MOD_RM;
		mod_rm_ = static_cast<uint8_t>(group_index) << 3;
	}
	
	auto rm_group_index = handler_->rm_group_index;
	if (rm_group_index >= 0) {
		encoder_flags_ |= EncoderFlags::MOD_RM;
		mod_rm_ |= static_cast<uint8_t>(rm_group_index) | 0xC0;
	}
	
	// Check bitness compatibility
	auto enc_flags3 = handler_->enc_flags3;
	if ((enc_flags3 & (EncFlags3::BIT16OR32 | EncFlags3::BIT64)) == EncFlags3::BIT16OR32) {
		if (bitness_ == 64) {
			set_error_message(ERROR_ONLY_1632_BIT_MODE);
		}
	} else if ((enc_flags3 & (EncFlags3::BIT16OR32 | EncFlags3::BIT64)) == EncFlags3::BIT64) {
		if (bitness_ != 64) {
			set_error_message(ERROR_ONLY_64_BIT_MODE);
		}
	}
	
	// Set operand size prefix
	switch (handler_->op_size) {
		case CodeSize::UNKNOWN:
			break;
		case CodeSize::CODE16:
			encoder_flags_ |= opsize16_flags_;
			break;
		case CodeSize::CODE32:
			encoder_flags_ |= opsize32_flags_;
			break;
		case CodeSize::CODE64:
			if ((enc_flags3 & EncFlags3::DEFAULT_OP_SIZE64) == 0) {
				encoder_flags_ |= EncoderFlags::W;
			}
			break;
	}
	
	// Set address size prefix
	switch (handler_->addr_size) {
		case CodeSize::UNKNOWN:
		case CodeSize::CODE64:
			break;
		case CodeSize::CODE16:
			encoder_flags_ |= adrsize16_flags_;
			break;
		case CodeSize::CODE32:
			encoder_flags_ |= adrsize32_flags_;
			break;
	}
	
	if (!handler_->is_special_instr) {
		// Encode operands
		auto operands = handler_->operands;
		for (std::size_t i = 0; i < operands.size(); ++i) {
			operands[i]->encode(*this, instruction, static_cast<uint32_t>(i));
		}
		
		// Write FWAIT if needed
		if ((enc_flags3 & EncFlags3::FWAIT) != 0) {
			write_byte_internal(0x9B);
		}
		
		// Call handler-specific encode
		handler_->encode(handler_, *this, instruction);
		
		// Write opcode
		if (!handler_->is_2byte_opcode) {
			write_byte_internal(op_code_);
		} else {
			write_byte_internal(op_code_ >> 8);
			write_byte_internal(op_code_);
		}
		
		// Write ModR/M + SIB + displacement
		if ((encoder_flags_ & (EncoderFlags::MOD_RM | EncoderFlags::DISPL)) != 0) {
			write_mod_rm();
		}
		
		// Write immediate
		if (imm_size_ != ImmSize::NONE) {
			write_immediate();
		}
	} else {
		// Special instruction (e.g., declare data)
		handler_->encode(handler_, *this, instruction);
	}
	
	auto instr_len = static_cast<std::size_t>(current_rip_ - rip);
	if (instr_len > IcedConstants::MAX_INSTRUCTION_LENGTH && !handler_->is_special_instr) {
		set_error_message(std::format("Instruction length > {} bytes", IcedConstants::MAX_INSTRUCTION_LENGTH));
	}
	
	if (!error_message_.empty()) {
		return std::unexpected(EncodeError(std::move(error_message_)));
	}
	
	return instr_len;
}

void Encoder::write_u8(uint8_t value) noexcept {
	write_byte_internal(value);
}

std::vector<uint8_t> Encoder::take_buffer() noexcept {
	return std::move(buffer_);
}

void Encoder::set_buffer(std::vector<uint8_t> buffer) noexcept {
	buffer_ = std::move(buffer);
}

void Encoder::set_error_message(std::string_view message) noexcept {
	if (error_message_.empty()) {
		error_message_ = message;
	}
}

bool Encoder::verify_op_kind(uint32_t operand, OpKind expected, OpKind actual) noexcept {
	if (expected == actual) {
		return true;
	}
	set_error_message(std::format("Operand {}: Expected OpKind {}, actual OpKind {}", 
		operand, static_cast<uint32_t>(expected), static_cast<uint32_t>(actual)));
	return false;
}

bool Encoder::verify_register(uint32_t operand, Register expected, Register actual) noexcept {
	if (expected == actual) {
		return true;
	}
	set_error_message(std::format("Operand {}: Expected Register {}, actual Register {}", 
		operand, static_cast<uint32_t>(expected), static_cast<uint32_t>(actual)));
	return false;
}

bool Encoder::verify_register_range(uint32_t operand, Register reg, Register reg_lo, Register reg_hi) noexcept {
	// In 16/32-bit mode, only the low 8 regs are used
	if (bitness_ != 64 && static_cast<uint32_t>(reg_hi) > static_cast<uint32_t>(reg_lo) + 7) {
		reg_hi = static_cast<Register>(static_cast<uint32_t>(reg_lo) + 7);
	}
	
	if (reg_lo <= reg && reg <= reg_hi) {
		return true;
	}
	
	set_error_message(std::format("Operand {}: Register {} is not between {} and {} (inclusive)", 
		operand, static_cast<uint32_t>(reg), static_cast<uint32_t>(reg_lo), static_cast<uint32_t>(reg_hi)));
	return false;
}

void Encoder::write_byte_internal(uint32_t value) noexcept {
	buffer_.push_back(static_cast<uint8_t>(value));
	++current_rip_;
}

void Encoder::write_prefixes(const Instruction& instruction, bool can_write_f3) noexcept {
	// Segment prefix
	auto seg = instruction.segment_prefix();
	if (seg != Register::NONE) {
		static constexpr uint8_t SEGMENT_OVERRIDES[] = {0x26, 0x2E, 0x36, 0x3E, 0x64, 0x65};
		auto seg_idx = static_cast<uint32_t>(seg) - static_cast<uint32_t>(Register::ES);
		if (seg_idx < 6) {
			write_byte_internal(SEGMENT_OVERRIDES[seg_idx]);
		}
	}
	
	// Lock prefix
	if ((encoder_flags_ & EncoderFlags::PF0) != 0 || instruction.has_lock_prefix()) {
		write_byte_internal(0xF0);
	}
	
	// Operand size prefix (66)
	if ((encoder_flags_ & EncoderFlags::P66) != 0) {
		write_byte_internal(0x66);
	}
	
	// Address size prefix (67)
	if ((encoder_flags_ & EncoderFlags::P67) != 0) {
		write_byte_internal(0x67);
	}
	
	// REPE prefix (F3)
	if (can_write_f3 && instruction.has_repe_prefix()) {
		write_byte_internal(0xF3);
	}
	
	// REPNE prefix (F2)
	if (instruction.has_repne_prefix()) {
		write_byte_internal(0xF2);
	}
}

void Encoder::write_mod_rm() noexcept {
	// Write ModR/M byte
	if ((encoder_flags_ & EncoderFlags::MOD_RM) != 0) {
		write_byte_internal(mod_rm_);
		
		// Write SIB byte if needed
		if ((encoder_flags_ & EncoderFlags::SIB) != 0) {
			write_byte_internal(sib_);
		}
	}
	
	// Write displacement
	displ_addr_ = static_cast<uint32_t>(current_rip_);
	uint32_t diff4;
	
	switch (displ_size_) {
		case DisplSize::NONE:
			break;
			
		case DisplSize::SIZE1:
			write_byte_internal(displ_);
			break;
			
		case DisplSize::SIZE2:
			diff4 = displ_;
			write_byte_internal(diff4);
			write_byte_internal(diff4 >> 8);
			break;
			
		case DisplSize::SIZE4:
			diff4 = displ_;
			write_byte_internal(diff4);
			write_byte_internal(diff4 >> 8);
			write_byte_internal(diff4 >> 16);
			write_byte_internal(diff4 >> 24);
			break;
			
		case DisplSize::SIZE8:
			diff4 = displ_;
			write_byte_internal(diff4);
			write_byte_internal(diff4 >> 8);
			write_byte_internal(diff4 >> 16);
			write_byte_internal(diff4 >> 24);
			diff4 = displ_hi_;
			write_byte_internal(diff4);
			write_byte_internal(diff4 >> 8);
			write_byte_internal(diff4 >> 16);
			write_byte_internal(diff4 >> 24);
			break;
			
		case DisplSize::RIP_REL_SIZE4_TARGET32: {
			auto eip = static_cast<uint32_t>(current_rip_) + 4 + IMM_SIZES[static_cast<std::size_t>(imm_size_)];
			diff4 = displ_ - eip;
			write_byte_internal(diff4);
			write_byte_internal(diff4 >> 8);
			write_byte_internal(diff4 >> 16);
			write_byte_internal(diff4 >> 24);
			break;
		}
			
		case DisplSize::RIP_REL_SIZE4_TARGET64: {
			auto rip_next = current_rip_ + 4 + IMM_SIZES[static_cast<std::size_t>(imm_size_)];
			auto target = (static_cast<uint64_t>(displ_hi_) << 32) | displ_;
			auto diff8 = static_cast<int64_t>(target - rip_next);
			if (diff8 < INT32_MIN || diff8 > INT32_MAX) {
				set_error_message(std::format(
					"RIP relative distance is too far away: next_ip: 0x{:016X} target: 0x{:016X}, diff = {}, diff must fit in an i32",
					rip_next, target, diff8));
			}
			diff4 = static_cast<uint32_t>(diff8);
			write_byte_internal(diff4);
			write_byte_internal(diff4 >> 8);
			write_byte_internal(diff4 >> 16);
			write_byte_internal(diff4 >> 24);
			break;
		}
	}
}

void Encoder::write_immediate() noexcept {
	imm_addr_ = static_cast<uint32_t>(current_rip_);
	uint32_t value;
	
	switch (imm_size_) {
		case ImmSize::NONE:
			break;
			
		case ImmSize::SIZE1:
		case ImmSize::SIZE_IB_REG:
		case ImmSize::SIZE1_OP_CODE:
			write_byte_internal(immediate_);
			break;
			
		case ImmSize::SIZE2:
			value = immediate_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			break;
			
		case ImmSize::SIZE4:
			value = immediate_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			write_byte_internal(value >> 16);
			write_byte_internal(value >> 24);
			break;
			
		case ImmSize::SIZE8:
			value = immediate_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			write_byte_internal(value >> 16);
			write_byte_internal(value >> 24);
			value = immediate_hi_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			write_byte_internal(value >> 16);
			write_byte_internal(value >> 24);
			break;
			
		case ImmSize::SIZE2_1:  // ENTER xxxx,yy
			value = immediate_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			write_byte_internal(immediate_hi_);
			break;
			
		case ImmSize::SIZE1_1:  // EXTRQ/INSERTQ xx,yy
			write_byte_internal(immediate_);
			write_byte_internal(immediate_hi_);
			break;
			
		case ImmSize::SIZE2_2:  // CALL16 FAR x:y
			value = immediate_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			value = immediate_hi_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			break;
			
		case ImmSize::SIZE4_2:  // CALL32 FAR x:y
			value = immediate_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			write_byte_internal(value >> 16);
			write_byte_internal(value >> 24);
			value = immediate_hi_;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			break;
			
		case ImmSize::RIP_REL_SIZE1_TARGET16: {
			auto ip = static_cast<uint16_t>(static_cast<uint32_t>(current_rip_) + 1);
			auto diff2 = static_cast<int16_t>(static_cast<int16_t>(immediate_) - ip);
			if (diff2 < INT8_MIN || diff2 > INT8_MAX) {
				set_error_message(std::format(
					"Branch distance is too far away: next_ip: 0x{:04X} target: 0x{:04X}, diff = {}, diff must fit in an i8",
					ip, static_cast<uint16_t>(immediate_), diff2));
			}
			write_byte_internal(static_cast<uint32_t>(diff2));
			break;
		}
			
		case ImmSize::RIP_REL_SIZE1_TARGET32: {
			auto eip = static_cast<uint32_t>(current_rip_) + 1;
			auto diff4 = static_cast<int32_t>(immediate_ - eip);
			if (diff4 < INT8_MIN || diff4 > INT8_MAX) {
				set_error_message(std::format(
					"Branch distance is too far away: next_ip: 0x{:08X} target: 0x{:08X}, diff = {}, diff must fit in an i8",
					eip, immediate_, diff4));
			}
			write_byte_internal(static_cast<uint32_t>(diff4));
			break;
		}
			
		case ImmSize::RIP_REL_SIZE1_TARGET64: {
			auto rip = current_rip_ + 1;
			auto target = (static_cast<uint64_t>(immediate_hi_) << 32) | immediate_;
			auto diff8 = static_cast<int64_t>(target - rip);
			if (diff8 < INT8_MIN || diff8 > INT8_MAX) {
				set_error_message(std::format(
					"Branch distance is too far away: next_ip: 0x{:016X} target: 0x{:016X}, diff = {}, diff must fit in an i8",
					rip, target, diff8));
			}
			write_byte_internal(static_cast<uint32_t>(diff8));
			break;
		}
			
		case ImmSize::RIP_REL_SIZE2_TARGET16: {
			auto eip = static_cast<uint32_t>(current_rip_) + 2;
			value = immediate_ - eip;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			break;
		}
			
		case ImmSize::RIP_REL_SIZE2_TARGET32: {
			auto eip = static_cast<uint32_t>(current_rip_) + 2;
			auto diff4 = static_cast<int32_t>(immediate_ - eip);
			if (diff4 < INT16_MIN || diff4 > INT16_MAX) {
				set_error_message(std::format(
					"Branch distance is too far away: next_ip: 0x{:08X} target: 0x{:08X}, diff = {}, diff must fit in an i16",
					eip, immediate_, diff4));
			}
			value = static_cast<uint32_t>(diff4);
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			break;
		}
			
		case ImmSize::RIP_REL_SIZE2_TARGET64: {
			auto rip = current_rip_ + 2;
			auto target = (static_cast<uint64_t>(immediate_hi_) << 32) | immediate_;
			auto diff8 = static_cast<int64_t>(target - rip);
			if (diff8 < INT16_MIN || diff8 > INT16_MAX) {
				set_error_message(std::format(
					"Branch distance is too far away: next_ip: 0x{:016X} target: 0x{:016X}, diff = {}, diff must fit in an i16",
					rip, target, diff8));
			}
			value = static_cast<uint32_t>(diff8);
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			break;
		}
			
		case ImmSize::RIP_REL_SIZE4_TARGET32: {
			auto eip = static_cast<uint32_t>(current_rip_) + 4;
			value = immediate_ - eip;
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			write_byte_internal(value >> 16);
			write_byte_internal(value >> 24);
			break;
		}
			
		case ImmSize::RIP_REL_SIZE4_TARGET64: {
			auto rip = current_rip_ + 4;
			auto target = (static_cast<uint64_t>(immediate_hi_) << 32) | immediate_;
			auto diff8 = static_cast<int64_t>(target - rip);
			if (diff8 < INT32_MIN || diff8 > INT32_MAX) {
				set_error_message(std::format(
					"Branch distance is too far away: next_ip: 0x{:016X} target: 0x{:016X}, diff = {}, diff must fit in an i32",
					rip, target, diff8));
			}
			value = static_cast<uint32_t>(diff8);
			write_byte_internal(value);
			write_byte_internal(value >> 8);
			write_byte_internal(value >> 16);
			write_byte_internal(value >> 24);
			break;
		}
	}
}

ConstantOffsets Encoder::get_constant_offsets() const noexcept {
	ConstantOffsets co;
	
	switch (displ_size_) {
		case DisplSize::NONE:
			break;
		case DisplSize::SIZE1:
			co.displacement_size = 1;
			co.displacement_offset = static_cast<uint8_t>(displ_addr_ - eip_);
			break;
		case DisplSize::SIZE2:
			co.displacement_size = 2;
			co.displacement_offset = static_cast<uint8_t>(displ_addr_ - eip_);
			break;
		case DisplSize::SIZE4:
		case DisplSize::RIP_REL_SIZE4_TARGET32:
		case DisplSize::RIP_REL_SIZE4_TARGET64:
			co.displacement_size = 4;
			co.displacement_offset = static_cast<uint8_t>(displ_addr_ - eip_);
			break;
		case DisplSize::SIZE8:
			co.displacement_size = 8;
			co.displacement_offset = static_cast<uint8_t>(displ_addr_ - eip_);
			break;
	}
	
	// Immediate offsets
	switch (imm_size_) {
		case ImmSize::NONE:
			break;
		case ImmSize::SIZE1:
		case ImmSize::SIZE_IB_REG:
		case ImmSize::SIZE1_OP_CODE:
		case ImmSize::RIP_REL_SIZE1_TARGET16:
		case ImmSize::RIP_REL_SIZE1_TARGET32:
		case ImmSize::RIP_REL_SIZE1_TARGET64:
			co.immediate_size = 1;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			break;
		case ImmSize::SIZE2:
		case ImmSize::RIP_REL_SIZE2_TARGET16:
		case ImmSize::RIP_REL_SIZE2_TARGET32:
		case ImmSize::RIP_REL_SIZE2_TARGET64:
			co.immediate_size = 2;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			break;
		case ImmSize::SIZE4:
		case ImmSize::RIP_REL_SIZE4_TARGET32:
		case ImmSize::RIP_REL_SIZE4_TARGET64:
			co.immediate_size = 4;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			break;
		case ImmSize::SIZE8:
			co.immediate_size = 8;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			break;
		case ImmSize::SIZE2_1:  // ENTER
			co.immediate_size = 2;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			co.immediate_size2 = 1;
			co.immediate_offset2 = static_cast<uint8_t>(imm_addr_ + 2 - eip_);
			break;
		case ImmSize::SIZE1_1:  // EXTRQ/INSERTQ
			co.immediate_size = 1;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			co.immediate_size2 = 1;
			co.immediate_offset2 = static_cast<uint8_t>(imm_addr_ + 1 - eip_);
			break;
		case ImmSize::SIZE2_2:  // CALL16 FAR
			co.immediate_size = 2;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			co.immediate_size2 = 2;
			co.immediate_offset2 = static_cast<uint8_t>(imm_addr_ + 2 - eip_);
			break;
		case ImmSize::SIZE4_2:  // CALL32 FAR
			co.immediate_size = 4;
			co.immediate_offset = static_cast<uint8_t>(imm_addr_ - eip_);
			co.immediate_size2 = 2;
			co.immediate_offset2 = static_cast<uint8_t>(imm_addr_ + 4 - eip_);
			break;
	}
	
	return co;
}

} // namespace iced_x86
