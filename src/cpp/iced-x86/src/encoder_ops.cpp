// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/encoder.hpp"
#include "iced_x86/instruction.hpp"
#include "iced_x86/internal/encoder_ops.hpp"
#include "iced_x86/internal/encoder_flags.hpp"
#include "iced_x86/code.hpp"

#include <format>

namespace iced_x86::internal {

// InvalidOpHandler
void InvalidOpHandler::encode(Encoder& /*encoder*/, const Instruction& /*instruction*/, uint32_t /*operand*/) const {
	// Should never be called
}

// OpA - Absolute far address
void OpA::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_far_branch(instruction, operand, size);
}

// OpHx - VEX/EVEX vvvv register
void OpHx::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::REGISTER, instruction.op_kind(operand))) {
		return;
	}
	auto reg = instruction.op_register(operand);
	if (!encoder.verify_register_range(operand, reg, reg_lo, reg_hi)) {
		return;
	}
	auto reg_num = static_cast<uint32_t>(reg) - static_cast<uint32_t>(reg_lo);
	encoder.or_encoder_flags((reg_num & 0x1F) << EncoderFlags::VVVVV_SHIFT);
}

// OpI4 - 4-bit immediate (high nibble)
void OpI4::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::IMMEDIATE8, instruction.op_kind(operand))) {
		return;
	}
	auto imm = instruction.immediate8();
	if (imm > 0xF) {
		encoder.set_error_message(std::format("Operand {}: Immediate value must be 0-15, but value is 0x{:02X}", operand, imm));
		return;
	}
	encoder.set_imm_size(ImmSize::SIZE1);
	encoder.set_immediate(encoder.op_code() | imm);  // Was OR'ed into immediate, which was initialized with the Is4/Is5 register
}

// OpIb - 8-bit immediate
void OpIb::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	auto current_imm_size = encoder.encoder_flags();  // Check if we already have an immediate
	// This is a simplified version - full implementation would handle SIZE1_1 and SIZE2_1 cases
	if (!encoder.verify_op_kind(operand, op_kind, instruction.op_kind(operand))) {
		return;
	}
	encoder.set_imm_size(ImmSize::SIZE1);
	encoder.set_immediate(instruction.immediate8());
}

// OpId - 32-bit immediate
void OpId::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, op_kind, instruction.op_kind(operand))) {
		return;
	}
	encoder.set_imm_size(ImmSize::SIZE4);
	encoder.set_immediate(instruction.immediate32());
}

// OpImm - Fixed immediate value
void OpImm::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::IMMEDIATE8, instruction.op_kind(operand))) {
		return;
	}
	if (instruction.immediate8() != value) {
		encoder.set_error_message(std::format("Operand {}: Expected immediate {}, actual {}", operand, value, instruction.immediate8()));
	}
}

// OpIq - 64-bit immediate
void OpIq::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::IMMEDIATE64, instruction.op_kind(operand))) {
		return;
	}
	encoder.set_imm_size(ImmSize::SIZE8);
	auto imm = instruction.immediate64();
	encoder.set_immediate(static_cast<uint32_t>(imm));
	encoder.set_immediate_hi(static_cast<uint32_t>(imm >> 32));
}

// OpIsX - Is4/Is5 register encoding in immediate
void OpIsX::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::REGISTER, instruction.op_kind(operand))) {
		return;
	}
	auto reg = instruction.op_register(operand);
	if (!encoder.verify_register_range(operand, reg, reg_lo, reg_hi)) {
		return;
	}
	auto reg_num = static_cast<uint32_t>(reg) - static_cast<uint32_t>(reg_lo);
	encoder.set_imm_size(ImmSize::SIZE_IB_REG);
	encoder.set_immediate((reg_num & 0xF) << 4);  // Store in high nibble, low nibble will be OR'ed by OpI4
}

// OpIw - 16-bit immediate
void OpIw::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::IMMEDIATE16, instruction.op_kind(operand))) {
		return;
	}
	encoder.set_imm_size(ImmSize::SIZE2);
	encoder.set_immediate(instruction.immediate16());
}

// OpJ - Relative branch
void OpJ::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_branch(op_kind, imm_size, instruction, operand);
}

// OpJdisp - Direct displacement branch
void OpJdisp::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_branch_disp(displ_size, instruction, operand);
}

// OpJx - Relative branch with variable size (xbegin)
void OpJx::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_branch_x(imm_size, instruction, operand);
}

// OpModRM_reg - ModR/M reg field register
void OpModRM_reg::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_mod_rm_register(instruction, operand, reg_lo, reg_hi);
}

// OpModRM_reg_mem - ModR/M reg field that encodes memory
void OpModRM_reg_mem::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_mod_rm_register(instruction, operand, reg_lo, reg_hi);
	encoder.or_encoder_flags(EncoderFlags::REG_IS_MEMORY);
}

// OpModRM_regF0 - ModR/M reg with F0 prefix for high CR regs
void OpModRM_regF0::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (encoder.bitness() != 64 
		&& instruction.op_kind(operand) == OpKind::REGISTER
		&& static_cast<uint32_t>(instruction.op_register(operand)) >= static_cast<uint32_t>(reg_lo) + 8
		&& static_cast<uint32_t>(instruction.op_register(operand)) <= static_cast<uint32_t>(reg_lo) + 15) {
		encoder.or_encoder_flags(EncoderFlags::PF0);
		auto adjusted_lo = static_cast<Register>(static_cast<uint32_t>(reg_lo) + 8);
		auto adjusted_hi = static_cast<Register>(static_cast<uint32_t>(reg_lo) + 15);
		encoder.add_mod_rm_register(instruction, operand, adjusted_lo, adjusted_hi);
	} else {
		encoder.add_mod_rm_register(instruction, operand, reg_lo, reg_hi);
	}
}

// OpModRM_rm - ModR/M r/m field (register or memory)
void OpModRM_rm::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_reg_or_mem(instruction, operand, reg_lo, reg_hi, true, true);
}

// OpModRM_rm_mem_only - ModR/M r/m field (memory only)
void OpModRM_rm_mem_only::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (must_use_sib) {
		encoder.or_encoder_flags(EncoderFlags::MUST_USE_SIB);
	}
	encoder.add_reg_or_mem(instruction, operand, Register::NONE, Register::NONE, true, false);
}

// OpModRM_rm_reg_only - ModR/M r/m field (register only)
void OpModRM_rm_reg_only::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_reg_or_mem(instruction, operand, reg_lo, reg_hi, false, true);
}

// OpMRBX - Memory [rBX + AL] for XLAT
void OpMRBX::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::MEMORY, instruction.op_kind(operand))) {
		return;
	}
	auto base = instruction.memory_base();
	if (instruction.memory_displ_size() != 0
		|| instruction.memory_displacement64() != 0
		|| instruction.memory_index_scale() != 1
		|| instruction.memory_index() != Register::AL
		|| (base != Register::BX && base != Register::EBX && base != Register::RBX)) {
		encoder.set_error_message(std::format("Operand {}: Operand must be [bx+al], [ebx+al], or [rbx+al]", operand));
		return;
	}
	uint32_t reg_size;
	if (base == Register::RBX) {
		reg_size = 8;
	} else if (base == Register::EBX) {
		reg_size = 4;
	} else {
		reg_size = 2;
	}
	encoder.set_addr_size(reg_size);
}

// OpO - Offset-only memory operand (moffs)
void OpO::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_abs_mem(instruction, operand);
}

// OprDI - Memory [rDI] for string instructions
void OprDI::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	auto op_kind = instruction.op_kind(operand);
	uint32_t reg_size = 0;
	
	switch (op_kind) {
		case OpKind::MEMORY_SEG_RDI:
			reg_size = 8;
			break;
		case OpKind::MEMORY_SEG_EDI:
			reg_size = 4;
			break;
		case OpKind::MEMORY_SEG_DI:
			reg_size = 2;
			break;
		default:
			break;
	}
	
	if (reg_size == 0) {
		encoder.set_error_message(std::format(
			"Operand {}: expected OpKind = MEMORY_SEG_DI, MEMORY_SEG_EDI or MEMORY_SEG_RDI", operand));
		return;
	}
	encoder.set_addr_size(reg_size);
}

// OpReg - Fixed register operand
void OpReg::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.verify_op_kind(operand, OpKind::REGISTER, instruction.op_kind(operand));
	encoder.verify_register(operand, register_, instruction.op_register(operand));
}

// OpRegEmbed8 - Register embedded in low 3 bits of opcode
void OpRegEmbed8::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_reg(instruction, operand, reg_lo, reg_hi);
}

// OpRegSTi - FPU ST(i) register
void OpRegSTi::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	if (!encoder.verify_op_kind(operand, OpKind::REGISTER, instruction.op_kind(operand))) {
		return;
	}
	auto reg = instruction.op_register(operand);
	if (!encoder.verify_register_range(operand, reg, Register::ST0, Register::ST7)) {
		return;
	}
	encoder.or_op_code(static_cast<uint32_t>(reg) - static_cast<uint32_t>(Register::ST0));
}

// OpVsib - VSIB memory operand for gather/scatter
void OpVsib::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	encoder.add_reg_or_mem_full(instruction, operand, Register::NONE, Register::NONE,
		vsib_index_reg_lo, vsib_index_reg_hi, true, false);
}

// OpX - Memory [rSI] for string instructions
void OpX::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	auto op_kind = instruction.op_kind(operand);
	uint32_t reg_size = 0;
	
	switch (op_kind) {
		case OpKind::MEMORY_SEG_RSI:
			reg_size = 8;
			break;
		case OpKind::MEMORY_SEG_ESI:
			reg_size = 4;
			break;
		case OpKind::MEMORY_SEG_SI:
			reg_size = 2;
			break;
		default:
			break;
	}
	
	if (reg_size == 0) {
		encoder.set_error_message(std::format(
			"Operand {}: expected OpKind = MEMORY_SEG_SI, MEMORY_SEG_ESI or MEMORY_SEG_RSI", operand));
		return;
	}
	
	// For MOVS, check that source and destination sizes match
	auto code = instruction.code();
	if (code == Code::MOVSB_M8_M8 || code == Code::MOVSW_M16_M16 
		|| code == Code::MOVSD_M32_M32 || code == Code::MOVSQ_M64_M64) {
		uint32_t regy_size = 0;
		auto op0_kind = instruction.op0_kind();
		switch (op0_kind) {
		case OpKind::MEMORY_ESRDI:
			regy_size = 8;
			break;
		case OpKind::MEMORY_ESEDI:
			regy_size = 4;
			break;
		case OpKind::MEMORY_ESDI:
			regy_size = 2;
			break;
			default:
				break;
		}
		if (reg_size != regy_size) {
			encoder.set_error_message(std::format(
				"Same sized register must be used: reg #1 size = {}, reg #2 size = {}",
				regy_size * 8, reg_size * 8));
			return;
		}
	}
	
	encoder.set_addr_size(reg_size);
}

// OpY - Memory [rDI] for string instructions
void OpY::encode(Encoder& encoder, const Instruction& instruction, uint32_t operand) const {
	auto op_kind = instruction.op_kind(operand);
	uint32_t reg_size = 0;
	
	switch (op_kind) {
		case OpKind::MEMORY_ESRDI:
			reg_size = 8;
			break;
		case OpKind::MEMORY_ESEDI:
			reg_size = 4;
			break;
		case OpKind::MEMORY_ESDI:
			reg_size = 2;
			break;
		default:
			break;
	}
	
	if (reg_size == 0) {
		encoder.set_error_message(std::format(
			"Operand {}: expected OpKind = MEMORY_ESDI, MEMORY_ESEDI or MEMORY_ESRDI", operand));
		return;
	}
	
	// For CMPS, check that source and destination sizes match
	auto code = instruction.code();
	if (code == Code::CMPSB_M8_M8 || code == Code::CMPSW_M16_M16 
		|| code == Code::CMPSD_M32_M32 || code == Code::CMPSQ_M64_M64) {
		uint32_t regx_size = 0;
		auto op0_kind = instruction.op0_kind();
		switch (op0_kind) {
			case OpKind::MEMORY_SEG_RSI:
				regx_size = 8;
				break;
			case OpKind::MEMORY_SEG_ESI:
				regx_size = 4;
				break;
			case OpKind::MEMORY_SEG_SI:
				regx_size = 2;
				break;
			default:
				break;
		}
		if (regx_size != reg_size) {
			encoder.set_error_message(std::format(
				"Same sized register must be used: reg #1 size = {}, reg #2 size = {}",
				regx_size * 8, reg_size * 8));
			return;
		}
	}
	
	encoder.set_addr_size(reg_size);
}

} // namespace iced_x86::internal
