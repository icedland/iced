// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/encoder.hpp"
#include "iced_x86/instruction.hpp"
#include "iced_x86/internal/encoder_flags.hpp"
#include "iced_x86/internal/encoder_handler.hpp"

#include <format>

namespace iced_x86 {

using namespace internal;

void Encoder::add_branch(OpKind op_kind, uint32_t imm_size, const Instruction& instruction, uint32_t operand) noexcept {
	if (!verify_op_kind(operand, op_kind, instruction.op_kind(operand))) {
		return;
	}
	
	uint64_t target;
	switch (imm_size) {
		case 1:
			switch (op_kind) {
				case OpKind::NEAR_BRANCH16:
					encoder_flags_ |= opsize16_flags_;
					imm_size_ = ImmSize::RIP_REL_SIZE1_TARGET16;
					immediate_ = instruction.near_branch16();
					break;
				case OpKind::NEAR_BRANCH32:
					encoder_flags_ |= opsize32_flags_;
					imm_size_ = ImmSize::RIP_REL_SIZE1_TARGET32;
					immediate_ = instruction.near_branch32();
					break;
				case OpKind::NEAR_BRANCH64:
					imm_size_ = ImmSize::RIP_REL_SIZE1_TARGET64;
					target = instruction.near_branch64();
					immediate_ = static_cast<uint32_t>(target);
					immediate_hi_ = static_cast<uint32_t>(target >> 32);
					break;
				default:
					break;
			}
			break;
			
		case 2:
			switch (op_kind) {
				case OpKind::NEAR_BRANCH16:
					encoder_flags_ |= opsize16_flags_;
					imm_size_ = ImmSize::RIP_REL_SIZE2_TARGET16;
					immediate_ = instruction.near_branch16();
					break;
				default:
					break;
			}
			break;
			
		case 4:
			switch (op_kind) {
				case OpKind::NEAR_BRANCH32:
					encoder_flags_ |= opsize32_flags_;
					imm_size_ = ImmSize::RIP_REL_SIZE4_TARGET32;
					immediate_ = instruction.near_branch32();
					break;
				case OpKind::NEAR_BRANCH64:
					imm_size_ = ImmSize::RIP_REL_SIZE4_TARGET64;
					target = instruction.near_branch64();
					immediate_ = static_cast<uint32_t>(target);
					immediate_hi_ = static_cast<uint32_t>(target >> 32);
					break;
				default:
					break;
			}
			break;
			
		default:
			break;
	}
}

void Encoder::add_branch_x(uint32_t imm_size, const Instruction& instruction, uint32_t operand) noexcept {
	if (bitness_ == 64) {
		if (!verify_op_kind(operand, OpKind::NEAR_BRANCH64, instruction.op_kind(operand))) {
			return;
		}
		auto target = instruction.near_branch64();
		switch (imm_size) {
			case 2:
				encoder_flags_ |= EncoderFlags::P66;
				imm_size_ = ImmSize::RIP_REL_SIZE2_TARGET64;
				immediate_ = static_cast<uint32_t>(target);
				immediate_hi_ = static_cast<uint32_t>(target >> 32);
				break;
			case 4:
				imm_size_ = ImmSize::RIP_REL_SIZE4_TARGET64;
				immediate_ = static_cast<uint32_t>(target);
				immediate_hi_ = static_cast<uint32_t>(target >> 32);
				break;
			default:
				break;
		}
	} else {
		if (!verify_op_kind(operand, OpKind::NEAR_BRANCH32, instruction.op_kind(operand))) {
			return;
		}
		switch (imm_size) {
			case 2:
				// P66 if bitness == 32
				encoder_flags_ |= (bitness_ & 0x20) << 2;
				imm_size_ = ImmSize::RIP_REL_SIZE2_TARGET32;
				immediate_ = instruction.near_branch32();
				break;
			case 4:
				// P66 if bitness == 16
				encoder_flags_ |= (bitness_ & 0x10) << 3;
				imm_size_ = ImmSize::RIP_REL_SIZE4_TARGET32;
				immediate_ = instruction.near_branch32();
				break;
			default:
				break;
		}
	}
}

void Encoder::add_branch_disp(uint32_t displ_size, const Instruction& instruction, uint32_t operand) noexcept {
	OpKind op_kind;
	switch (displ_size) {
		case 2:
			op_kind = OpKind::NEAR_BRANCH16;
			imm_size_ = ImmSize::SIZE2;
			immediate_ = instruction.near_branch16();
			break;
		case 4:
			op_kind = OpKind::NEAR_BRANCH32;
			imm_size_ = ImmSize::SIZE4;
			immediate_ = instruction.near_branch32();
			break;
		default:
			return;
	}
	verify_op_kind(operand, op_kind, instruction.op_kind(operand));
}

void Encoder::add_far_branch(const Instruction& instruction, uint32_t operand, uint32_t size) noexcept {
	if (size == 2) {
		if (!verify_op_kind(operand, OpKind::FAR_BRANCH16, instruction.op_kind(operand))) {
			return;
		}
		imm_size_ = ImmSize::SIZE2_2;
		immediate_ = instruction.far_branch16();
		immediate_hi_ = instruction.far_branch_selector();
	} else {
		if (!verify_op_kind(operand, OpKind::FAR_BRANCH32, instruction.op_kind(operand))) {
			return;
		}
		imm_size_ = ImmSize::SIZE4_2;
		immediate_ = instruction.far_branch32();
		immediate_hi_ = instruction.far_branch_selector();
	}
	if (bitness_ != size * 8) {
		encoder_flags_ |= EncoderFlags::P66;
	}
}

void Encoder::set_addr_size(uint32_t reg_size) noexcept {
	if (bitness_ == 64) {
		if (reg_size == 2) {
			set_error_message(std::format("Invalid register size: {}, must be 32-bit or 64-bit", reg_size * 8));
		} else if (reg_size == 4) {
			encoder_flags_ |= EncoderFlags::P67;
		}
	} else {
		if (reg_size == 8) {
			set_error_message(std::format("Invalid register size: {}, must be 16-bit or 32-bit", reg_size * 8));
		} else if (bitness_ == 16) {
			if (reg_size == 4) {
				encoder_flags_ |= EncoderFlags::P67;
			}
		} else {
			// bitness == 32
			if (reg_size == 2) {
				encoder_flags_ |= EncoderFlags::P67;
			}
		}
	}
}

void Encoder::add_abs_mem(const Instruction& instruction, uint32_t operand) noexcept {
	encoder_flags_ |= EncoderFlags::DISPL;
	auto op_kind = instruction.op_kind(operand);
	
	if (op_kind == OpKind::MEMORY) {
		if (instruction.memory_base() != Register::NONE || instruction.memory_index() != Register::NONE) {
			set_error_message(std::format("Operand {}: Absolute addresses can't have base and/or index regs", operand));
			return;
		}
		if (instruction.memory_index_scale() != 1) {
			set_error_message(std::format("Operand {}: Absolute addresses must have scale == *1", operand));
			return;
		}
		
		switch (instruction.memory_displ_size()) {
			case 2:
				if (bitness_ == 64) {
					set_error_message(std::format("Operand {}: 16-bit abs addresses can't be used in 64-bit mode", operand));
					return;
				}
				if (bitness_ == 32) {
					encoder_flags_ |= EncoderFlags::P67;
				}
				displ_size_ = DisplSize::SIZE2;
				if (instruction.memory_displacement64() > UINT16_MAX) {
					set_error_message(std::format("Operand {}: Displacement must fit in a u16", operand));
					return;
				}
				displ_ = instruction.memory_displacement32();
				break;
				
			case 4:
				encoder_flags_ |= adrsize32_flags_;
				displ_size_ = DisplSize::SIZE4;
				if (instruction.memory_displacement64() > UINT32_MAX) {
					set_error_message(std::format("Operand {}: Displacement must fit in a u32", operand));
					return;
				}
				displ_ = instruction.memory_displacement32();
				break;
				
			case 8:
				if (bitness_ != 64) {
					set_error_message(std::format("Operand {}: 64-bit abs address is only available in 64-bit mode", operand));
					return;
				}
				displ_size_ = DisplSize::SIZE8;
				{
					auto addr = instruction.memory_displacement64();
					displ_ = static_cast<uint32_t>(addr);
					displ_hi_ = static_cast<uint32_t>(addr >> 32);
				}
				break;
				
			default:
				set_error_message(std::format(
					"Operand {}: memory_displ_size() must be initialized to 2 (16-bit), 4 (32-bit) or 8 (64-bit)", operand));
				break;
		}
	} else {
		set_error_message(std::format("Operand {}: Expected OpKind::MEMORY, actual: {}", operand, static_cast<uint32_t>(op_kind)));
	}
}

void Encoder::add_mod_rm_register(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi) noexcept {
	if (!verify_op_kind(operand, OpKind::REGISTER, instruction.op_kind(operand))) {
		return;
	}
	auto reg = instruction.op_register(operand);
	if (!verify_register_range(operand, reg, reg_lo, reg_hi)) {
		return;
	}
	
	auto reg_num = static_cast<uint32_t>(reg) - static_cast<uint32_t>(reg_lo);
	
	// Handle high 8-bit legacy registers
	if (reg_lo == Register::AL) {
		if (reg >= Register::SPL) {
			reg_num -= 4;
			encoder_flags_ |= EncoderFlags::REX;
		} else if (reg >= Register::AH) {
			encoder_flags_ |= EncoderFlags::HIGH_LEGACY_8_BIT_REGS;
		}
	}
	
	mod_rm_ |= static_cast<uint8_t>((reg_num & 7) << 3);
	encoder_flags_ |= EncoderFlags::MOD_RM;
	// R bit
	encoder_flags_ |= (reg_num & 8) >> 1;
	// R2 bit (EVEX.R')
	encoder_flags_ |= (reg_num & 0x10) << (9 - 4);
}

void Encoder::add_reg(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi) noexcept {
	if (!verify_op_kind(operand, OpKind::REGISTER, instruction.op_kind(operand))) {
		return;
	}
	auto reg = instruction.op_register(operand);
	if (!verify_register_range(operand, reg, reg_lo, reg_hi)) {
		return;
	}
	
	auto reg_num = static_cast<uint32_t>(reg) - static_cast<uint32_t>(reg_lo);
	
	// Handle high 8-bit legacy registers
	if (reg_lo == Register::AL) {
		if (reg >= Register::SPL) {
			reg_num -= 4;
			encoder_flags_ |= EncoderFlags::REX;
		} else if (reg >= Register::AH) {
			encoder_flags_ |= EncoderFlags::HIGH_LEGACY_8_BIT_REGS;
		}
	}
	
	op_code_ |= reg_num & 7;
	// B bit
	encoder_flags_ |= reg_num >> 3;
}

void Encoder::add_reg_or_mem(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi, 
                              bool allow_mem_op, bool allow_reg_op) noexcept {
	add_reg_or_mem_full(instruction, operand, reg_lo, reg_hi, Register::NONE, Register::NONE, allow_mem_op, allow_reg_op);
}

void Encoder::add_reg_or_mem_full(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi,
                                   Register vsib_index_reg_lo, Register vsib_index_reg_hi, 
                                   bool allow_mem_op, bool allow_reg_op) noexcept {
	auto op_kind = instruction.op_kind(operand);
	encoder_flags_ |= EncoderFlags::MOD_RM;
	
	if (op_kind == OpKind::REGISTER) {
		if (!allow_reg_op) {
			set_error_message(std::format("Operand {}: register operand is not allowed", operand));
			return;
		}
		auto reg = instruction.op_register(operand);
		if (!verify_register_range(operand, reg, reg_lo, reg_hi)) {
			return;
		}
		
		auto reg_num = static_cast<uint32_t>(reg) - static_cast<uint32_t>(reg_lo);
		
		// Handle high 8-bit legacy registers
		if (reg_lo == Register::AL) {
			if (reg >= Register::R8_L) {
				reg_num -= 4;
			} else if (reg >= Register::SPL) {
				reg_num -= 4;
				encoder_flags_ |= EncoderFlags::REX;
			} else if (reg >= Register::AH) {
				encoder_flags_ |= EncoderFlags::HIGH_LEGACY_8_BIT_REGS;
			}
		}
		
		mod_rm_ |= static_cast<uint8_t>(reg_num & 7);
		mod_rm_ |= 0xC0;  // mod = 11 (register)
		// B and X bits
		encoder_flags_ |= (reg_num >> 3) & 3;
		
	} else if (op_kind == OpKind::MEMORY) {
		if (!allow_mem_op) {
			set_error_message(std::format("Operand {}: memory operand is not allowed", operand));
			return;
		}
		
		if (instruction.is_broadcast()) {
			encoder_flags_ |= EncoderFlags::BROADCAST;
		}
		
		auto code_size = instruction.code_size();
		if (code_size == CodeSize::UNKNOWN) {
			if (bitness_ == 64) {
				code_size = CodeSize::CODE64;
			} else if (bitness_ == 32) {
				code_size = CodeSize::CODE32;
			} else {
				code_size = CodeSize::CODE16;
			}
		}
		
		auto addr_size = get_address_size_in_bytes(instruction, code_size) * 8;
		if (addr_size != bitness_) {
			encoder_flags_ |= EncoderFlags::P67;
		}
		
		if ((encoder_flags_ & EncoderFlags::REG_IS_MEMORY) != 0) {
			auto reg_size = get_register_op_size(instruction);
			if (reg_size != addr_size) {
				set_error_message(std::format("Operand {}: Register operand size must equal memory addressing mode (16/32/64)", operand));
				return;
			}
		}
		
		if (addr_size == 16) {
			if (vsib_index_reg_lo != Register::NONE) {
				set_error_message(std::format(
					"Operand {}: VSIB operands can't use 16-bit addressing. It must be 32-bit or 64-bit addressing", operand));
				return;
			}
			add_mem_op16(instruction, operand);
		} else {
			add_mem_op(instruction, operand, addr_size, vsib_index_reg_lo, vsib_index_reg_hi);
		}
	} else {
		set_error_message(std::format("Operand {}: Expected a register or memory operand, but op_kind is {}", 
			operand, static_cast<uint32_t>(op_kind)));
	}
}

uint32_t Encoder::get_register_op_size(const Instruction& instruction) noexcept {
	if (instruction.op0_kind() == OpKind::REGISTER) {
		auto reg = instruction.op0_register();
		if (static_cast<uint32_t>(reg) >= static_cast<uint32_t>(Register::RAX) && 
		    static_cast<uint32_t>(reg) <= static_cast<uint32_t>(Register::R15)) {
			return 64;
		}
		if (static_cast<uint32_t>(reg) >= static_cast<uint32_t>(Register::EAX) && 
		    static_cast<uint32_t>(reg) <= static_cast<uint32_t>(Register::R15_D)) {
			return 32;
		}
		if (static_cast<uint32_t>(reg) >= static_cast<uint32_t>(Register::AX) && 
		    static_cast<uint32_t>(reg) <= static_cast<uint32_t>(Register::R15_W)) {
			return 16;
		}
	}
	return 0;
}

uint32_t Encoder::get_address_size_in_bytes(const Instruction& instruction, CodeSize code_size) noexcept {
	auto base = instruction.memory_base();
	auto index = instruction.memory_index();
	
	// If we have a base register, use its size
	if (base != Register::NONE) {
		// RIP-relative: 64-bit addressing
		if (base == Register::RIP) {
			return 8;
		}
		// EIP-relative: 32-bit addressing  
		if (base == Register::EIP) {
			return 4;
		}
		if (static_cast<uint32_t>(base) >= static_cast<uint32_t>(Register::RAX) && 
		    static_cast<uint32_t>(base) <= static_cast<uint32_t>(Register::R15)) {
			return 8;
		}
		if (static_cast<uint32_t>(base) >= static_cast<uint32_t>(Register::EAX) && 
		    static_cast<uint32_t>(base) <= static_cast<uint32_t>(Register::R15_D)) {
			return 4;
		}
		if (static_cast<uint32_t>(base) >= static_cast<uint32_t>(Register::AX) && 
		    static_cast<uint32_t>(base) <= static_cast<uint32_t>(Register::R15_W)) {
			return 2;
		}
		// 16-bit addressing: BX, BP, SI, DI
		if (base == Register::BX || base == Register::BP || base == Register::SI || base == Register::DI) {
			return 2;
		}
	}
	
	// If we have an index register, use its size
	if (index != Register::NONE) {
		if (static_cast<uint32_t>(index) >= static_cast<uint32_t>(Register::RAX) && 
		    static_cast<uint32_t>(index) <= static_cast<uint32_t>(Register::R15)) {
			return 8;
		}
		if (static_cast<uint32_t>(index) >= static_cast<uint32_t>(Register::EAX) && 
		    static_cast<uint32_t>(index) <= static_cast<uint32_t>(Register::R15_D)) {
			return 4;
		}
		// XMM/YMM/ZMM for VSIB - use code size to determine address size
		if (static_cast<uint32_t>(index) >= static_cast<uint32_t>(Register::XMM0)) {
			return code_size == CodeSize::CODE64 ? 8 : 4;
		}
	}
	
	// Use displacement size hint
	auto displ_size = instruction.memory_displ_size();
	if (displ_size == 2) return 2;
	if (displ_size == 4) return 4;
	if (displ_size == 8) return 8;
	
	// Default based on code size
	switch (code_size) {
		case CodeSize::CODE16: return 2;
		case CodeSize::CODE32: return 4;
		case CodeSize::CODE64: return 8;
		default: return 4;
	}
}

void Encoder::add_mem_op16(const Instruction& instruction, uint32_t operand) noexcept {
	if (bitness_ == 64) {
		set_error_message(std::format("Operand {}: 16-bit addressing can't be used by 64-bit code", operand));
		return;
	}
	
	auto base = instruction.memory_base();
	auto index = instruction.memory_index();
	auto displ_size = instruction.memory_displ_size();
	
	// Determine r/m field based on base+index combination
	if (base == Register::BX && index == Register::SI) {
		// [BX+SI]
	} else if (base == Register::BX && index == Register::DI) {
		mod_rm_ |= 1;  // [BX+DI]
	} else if (base == Register::BP && index == Register::SI) {
		mod_rm_ |= 2;  // [BP+SI]
	} else if (base == Register::BP && index == Register::DI) {
		mod_rm_ |= 3;  // [BP+DI]
	} else if (base == Register::SI && index == Register::NONE) {
		mod_rm_ |= 4;  // [SI]
	} else if (base == Register::DI && index == Register::NONE) {
		mod_rm_ |= 5;  // [DI]
	} else if (base == Register::BP && index == Register::NONE) {
		mod_rm_ |= 6;  // [BP]
	} else if (base == Register::BX && index == Register::NONE) {
		mod_rm_ |= 7;  // [BX]
	} else if (base == Register::NONE && index == Register::NONE) {
		// Direct address
		mod_rm_ |= 6;
		displ_size_ = DisplSize::SIZE2;
		if (instruction.memory_displacement64() > UINT16_MAX) {
			set_error_message(std::format("Operand {}: Displacement must fit in a u16", operand));
			return;
		}
		displ_ = instruction.memory_displacement32();
		return;
	} else {
		set_error_message(std::format("Operand {}: Invalid 16-bit base + index registers: base={}, index={}",
			operand, static_cast<uint32_t>(base), static_cast<uint32_t>(index)));
		return;
	}
	
	// Has base or index register - handle displacement
	auto displ64 = instruction.memory_displacement64();
	if (static_cast<int64_t>(displ64) < INT16_MIN || static_cast<int64_t>(displ64) > UINT16_MAX) {
		set_error_message(std::format("Operand {}: Displacement must fit in an i16 or a u16", operand));
		return;
	}
	displ_ = instruction.memory_displacement32();
	
	// [BP] => [BP+00]
	if (displ_size == 0 && base == Register::BP && index == Register::NONE) {
		displ_size = 1;
		if (displ_ != 0) {
			set_error_message(std::format("Operand {}: Displacement must be 0 if displ_size == 0", operand));
			return;
		}
	}
	
	// Try compressed displacement (EVEX) for displ_size == 1
	if (displ_size == 1) {
		int8_t disp8n = 0;
		if (handler_ && handler_->try_convert_to_disp8n &&
		    handler_->try_convert_to_disp8n(handler_, *this, instruction, static_cast<int16_t>(displ_) , disp8n)) {
			displ_ = static_cast<uint32_t>(static_cast<uint8_t>(disp8n));
		} else {
			displ_size = 2;
		}
	}
	
	if (displ_size == 0) {
		if (displ_ != 0) {
			set_error_message(std::format("Operand {}: Displacement must be 0 if displ_size == 0", operand));
			return;
		}
		// No displacement needed
	} else if (displ_size == 1) {
		auto displ_i32 = static_cast<int32_t>(displ_);
		if (displ_i32 < INT8_MIN || displ_i32 > INT8_MAX) {
			set_error_message(std::format("Operand {}: Displacement must fit in an i8", operand));
			return;
		}
		mod_rm_ |= 0x40;  // mod = 01
		displ_size_ = DisplSize::SIZE1;
	} else if (displ_size == 2) {
		mod_rm_ |= 0x80;  // mod = 10
		displ_size_ = DisplSize::SIZE2;
	} else {
		set_error_message(std::format("Operand {}: Invalid displacement size: {}, must be 0, 1, or 2", operand, displ_size));
	}
}

void Encoder::add_mem_op(const Instruction& instruction, uint32_t operand, uint32_t addr_size,
                          Register vsib_index_reg_lo, Register vsib_index_reg_hi) noexcept {
	auto base = instruction.memory_base();
	auto index = instruction.memory_index();
	auto displ = static_cast<int32_t>(instruction.memory_displacement32());
	auto displ_size = instruction.memory_displ_size();
	auto scale = instruction.memory_index_scale();
	
	// Determine valid register ranges based on address size
	Register base_lo, base_hi, index_lo, index_hi;
	if (addr_size == 64) {
		base_lo = Register::RAX;
		base_hi = Register::R15;
	} else {
		base_lo = Register::EAX;
		base_hi = Register::R15_D;
	}
	if (vsib_index_reg_lo != Register::NONE) {
		index_lo = vsib_index_reg_lo;
		index_hi = vsib_index_reg_hi;
	} else {
		index_lo = base_lo;
		index_hi = base_hi;
	}
	
	// Validate base register range
	if (base != Register::NONE && base != Register::RIP && base != Register::EIP) {
		if (!verify_register_range(operand, base, base_lo, base_hi)) {
			return;
		}
	}
	
	// Validate index register range
	if (index != Register::NONE && !verify_register_range(operand, index, index_lo, index_hi)) {
		return;
	}
	
	// Get base register number (-1 if none)
	int32_t base_num = -1;
	if (base != Register::NONE) {
		if (addr_size == 64) {
			base_num = static_cast<int32_t>(base) - static_cast<int32_t>(Register::RAX);
		} else {
			base_num = static_cast<int32_t>(base) - static_cast<int32_t>(Register::EAX);
		}
		if (base == Register::RIP || base == Register::EIP) {
			base_num = -1;  // RIP-relative
		}
	}
	
	// Get index register number (-1 if none)
	int32_t index_num = -1;
	if (index != Register::NONE) {
		if (vsib_index_reg_lo != Register::NONE) {
			// VSIB - already validated above
			index_num = static_cast<int32_t>(index) - static_cast<int32_t>(vsib_index_reg_lo);
		} else if (addr_size == 64) {
			index_num = static_cast<int32_t>(index) - static_cast<int32_t>(Register::RAX);
		} else {
			index_num = static_cast<int32_t>(index) - static_cast<int32_t>(Register::EAX);
		}
	}
	
	// RIP-relative addressing
	if (base == Register::RIP || base == Register::EIP) {
		if (index != Register::NONE) {
			set_error_message(std::format("Operand {}: RIP-relative addressing can't have an index register", operand));
			return;
		}
		mod_rm_ |= 5;  // r/m = 101, mod = 00 (RIP+disp32)
		if (bitness_ == 64) {
			displ_size_ = DisplSize::RIP_REL_SIZE4_TARGET64;
			auto addr = instruction.memory_displacement64();
			displ_ = static_cast<uint32_t>(addr);
			displ_hi_ = static_cast<uint32_t>(addr >> 32);
		} else {
			displ_size_ = DisplSize::RIP_REL_SIZE4_TARGET32;
			displ_ = instruction.memory_displacement32();
		}
		return;
	}
	
	// Convert scale to encoding (1->0, 2->1, 4->2, 8->3)
	uint32_t scale_enc = 0;
	switch (scale) {
		case 1: scale_enc = 0; break;
		case 2: scale_enc = 1; break;
		case 4: scale_enc = 2; break;
		case 8: scale_enc = 3; break;
	}
	
	// Determine if SIB byte is needed
	bool need_sib = index_num >= 0 
		|| (base_num >= 0 && (base_num & 7) == 4)  // ESP/RSP or R12
		|| base_num < 0
		|| (encoder_flags_ & EncoderFlags::MUST_USE_SIB) != 0;
	
	// Store displacement in member variable
	displ_ = instruction.memory_displacement32();
	
	// [EBP]/[EBP+index*scale] => [EBP+00]/[EBP+index*scale+00]
	// Also applies to RBP and R13
	if (displ_size == 0 && base_num >= 0 && (base_num & 7) == 5) {
		displ_size = 1;
		if (displ_ != 0) {
			set_error_message(std::format("Operand {}: Displacement must be 0 if displ_size == 0", operand));
			return;
		}
	}
	
	// Try compressed displacement (EVEX) for displ_size == 1
	if (displ_size == 1) {
		int8_t disp8n = 0;
		if (handler_ && handler_->try_convert_to_disp8n &&
		    handler_->try_convert_to_disp8n(handler_, *this, instruction, displ, disp8n)) {
			displ_ = static_cast<uint32_t>(static_cast<uint8_t>(disp8n));
		} else if (static_cast<int32_t>(displ_) >= INT8_MIN && static_cast<int32_t>(displ_) <= INT8_MAX) {
			// Displacement already fits in i8, keep it
		} else {
			displ_size = addr_size / 8;  // Upgrade to 4 or 8 bytes
		}
	}
	
	// Determine mod field
	uint8_t mod;
	if (base_num < 0) {
		// No base - need disp32
		displ_size_ = DisplSize::SIZE4;
		mod = 0;  // mod=00 with SIB base=101 means disp32 without base
	} else if (displ_size == 1) {
		auto displ_i32 = static_cast<int32_t>(displ_);
		if (displ_i32 < INT8_MIN || displ_i32 > INT8_MAX) {
			set_error_message(std::format("Operand {}: Displacement must fit in an i8", operand));
			return;
		}
		mod = 1;  // [reg + disp8]
		displ_size_ = DisplSize::SIZE1;
	} else if (displ_size == static_cast<uint32_t>(addr_size / 8)) {
		mod = 2;  // [reg + disp32]
		displ_size_ = DisplSize::SIZE4;
	} else if (displ_size == 0) {
		if (displ_ != 0) {
			set_error_message(std::format("Operand {}: Displacement must be 0 if displ_size == 0", operand));
			return;
		}
		mod = 0;  // [reg]
	} else {
		set_error_message("Invalid memory_displ_size() value");
		mod = 0;
	}
	
	mod_rm_ |= mod << 6;
	
	if (need_sib) {
		encoder_flags_ |= EncoderFlags::SIB;
		sib_ = static_cast<uint8_t>(scale_enc << 6);
		mod_rm_ |= 4;  // r/m = 100 (SIB follows)
		
		if (index == Register::RSP || index == Register::ESP) {
			set_error_message(std::format("Operand {}: ESP/RSP can't be used as an index register", operand));
			return;
		}
		
		if (base_num < 0) {
			sib_ |= 5;  // base = 101 (no base)
		} else {
			sib_ |= static_cast<uint8_t>(base_num & 7);
		}
		
		if (index_num < 0) {
			sib_ |= 0x20;  // index = 100 (no index)
		} else {
			sib_ |= static_cast<uint8_t>((index_num & 7) << 3);
		}
	} else {
		mod_rm_ |= static_cast<uint8_t>(base_num & 7);
	}
	
	// Set REX.B and REX.X bits
	if (base_num >= 0) {
		encoder_flags_ |= static_cast<uint32_t>(base_num) >> 3;  // B bit
	}
	if (index_num >= 0) {
		encoder_flags_ |= (static_cast<uint32_t>(index_num) >> 2) & 2;  // X bit
		encoder_flags_ |= (static_cast<uint32_t>(index_num) & 0x10) << EncoderFlags::VVVVV_SHIFT;  // V' for VSIB
	}
}

} // namespace iced_x86
