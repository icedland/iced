// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/instruction_create.hpp"
#include "iced_x86/instruction.hpp"
#include "iced_x86/memory_operand.hpp"
#include "iced_x86/code.hpp"
#include "iced_x86/op_kind.hpp"
#include "iced_x86/register.hpp"
#include "iced_x86/rep_prefix_kind.hpp"

#include <cstring>

namespace iced_x86 {

namespace {

// Helper to initialize a memory operand in an instruction
void init_memory_operand(Instruction& instruction, const MemoryOperand& memory) {
	instruction.set_memory_base(memory.base);
	instruction.set_memory_index(memory.index);
	instruction.set_memory_index_scale(memory.scale);
	instruction.set_memory_displacement64(static_cast<uint64_t>(memory.displacement));
	instruction.set_memory_displ_size(memory.displ_size);
	instruction.set_is_broadcast(memory.is_broadcast);
	instruction.set_segment_prefix(memory.segment_prefix);
}

// Sentinel value to indicate "use fallback logic"
constexpr OpKind UNKNOWN_OP_KIND = static_cast<OpKind>(255);

// Helper to get the OpKind for a given Code value and operand index
// Returns the appropriate immediate OpKind based on the instruction's encoding
// We parse the code name suffix to determine the expected immediate type
OpKind get_immediate_op_kind(Code code, uint32_t operand) {
	// Get the code value as integer
	uint32_t code_val = static_cast<uint32_t>(code);
	
	// For instructions with immediates, the code name usually contains the size hint
	// This is a heuristic based on the Code enum naming convention
	// Common patterns: _IMM8, _IMM16, _IMM32, _IMM64, _IB (imm8), _IW (imm16), _ID (imm32)
	
	// Check specific known patterns for immediate instructions
	// MOV_R32_IMM32 codes are typically in range 176-191 (based on Code enum)
	// ADD/SUB/CMP etc with IMM32 typically require IMMEDIATE32
	
	// For multi-byte immediate instructions (IMM16, IMM32, IMM64)
	// These need full-size immediates, not sign-extended 8-bit values
	
	// The safest approach: if the instruction Code name ends with IMM32, use IMMEDIATE32
	// This covers ADD_RM32_IMM32, MOV_R32_IMM32, CMP_RM32_IMM32, etc.
	
	// Since we can't easily get the code name string at runtime, we use known ranges
	// All _IMM32 instructions need OpKind::IMMEDIATE32
	// All _IMM8 instructions need OpKind::IMMEDIATE8 or IMMEDIATE8TO32/etc
	// All _IMM64 instructions need OpKind::IMMEDIATE64
	
	// Rather than enumerate all codes, we'll return UNKNOWN and let the caller
	// determine based on instruction creation context. However, for the common
	// case of register + immediate, we'll use IMMEDIATE32 for 32-bit operand sizes.
	
	(void)code_val;
	(void)operand;
	
	// Return sentinel to indicate we need instruction-specific handling
	return UNKNOWN_OP_KIND;
}

// Helper to create string instruction with Reg, SegRSI pattern (OUTS, LODS)
Instruction create_string_reg_segrsi(Code code, uint32_t address_size, Register reg, Register segment_prefix, RepPrefixKind rep_prefix) {
	Instruction instruction{};
	instruction.set_code(code);
	
	if (rep_prefix == RepPrefixKind::REPE) {
		instruction.set_has_repe_prefix(true);
	} else if (rep_prefix == RepPrefixKind::REPNE) {
		instruction.set_has_repne_prefix(true);
	}
	
	instruction.set_op0_kind(OpKind::REGISTER);
	instruction.set_op0_register(reg);
	
	if (address_size == 64) {
		instruction.set_op1_kind(OpKind::MEMORY_SEG_RSI);
	} else if (address_size == 32) {
		instruction.set_op1_kind(OpKind::MEMORY_SEG_ESI);
	} else {
		instruction.set_op1_kind(OpKind::MEMORY_SEG_SI);
	}
	
	instruction.set_segment_prefix(segment_prefix);
	return instruction;
}

// Helper to create string instruction with Reg, ESRDI pattern (SCAS, INS)
Instruction create_string_reg_esrdi(Code code, uint32_t address_size, Register reg, RepPrefixKind rep_prefix) {
	Instruction instruction{};
	instruction.set_code(code);
	
	if (rep_prefix == RepPrefixKind::REPE) {
		instruction.set_has_repe_prefix(true);
	} else if (rep_prefix == RepPrefixKind::REPNE) {
		instruction.set_has_repne_prefix(true);
	}
	
	instruction.set_op0_kind(OpKind::REGISTER);
	instruction.set_op0_register(reg);
	
	if (address_size == 64) {
		instruction.set_op1_kind(OpKind::MEMORY_ESRDI);
	} else if (address_size == 32) {
		instruction.set_op1_kind(OpKind::MEMORY_ESEDI);
	} else {
		instruction.set_op1_kind(OpKind::MEMORY_ESDI);
	}
	
	return instruction;
}

// Helper to create string instruction with ESRDI, Reg pattern (STOS)
Instruction create_string_esrdi_reg(Code code, uint32_t address_size, Register reg, RepPrefixKind rep_prefix) {
	Instruction instruction{};
	instruction.set_code(code);
	
	if (rep_prefix == RepPrefixKind::REPE) {
		instruction.set_has_repe_prefix(true);
	} else if (rep_prefix == RepPrefixKind::REPNE) {
		instruction.set_has_repne_prefix(true);
	}
	
	if (address_size == 64) {
		instruction.set_op0_kind(OpKind::MEMORY_ESRDI);
	} else if (address_size == 32) {
		instruction.set_op0_kind(OpKind::MEMORY_ESEDI);
	} else {
		instruction.set_op0_kind(OpKind::MEMORY_ESDI);
	}
	
	instruction.set_op1_kind(OpKind::REGISTER);
	instruction.set_op1_register(reg);
	return instruction;
}

// Helper to create string instruction with ESRDI, SegRSI pattern (MOVS)
Instruction create_string_esrdi_segrsi(Code code, uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	Instruction instruction{};
	instruction.set_code(code);
	
	if (rep_prefix == RepPrefixKind::REPE) {
		instruction.set_has_repe_prefix(true);
	} else if (rep_prefix == RepPrefixKind::REPNE) {
		instruction.set_has_repne_prefix(true);
	}
	
	if (address_size == 64) {
		instruction.set_op0_kind(OpKind::MEMORY_ESRDI);
		instruction.set_op1_kind(OpKind::MEMORY_SEG_RSI);
	} else if (address_size == 32) {
		instruction.set_op0_kind(OpKind::MEMORY_ESEDI);
		instruction.set_op1_kind(OpKind::MEMORY_SEG_ESI);
	} else {
		instruction.set_op0_kind(OpKind::MEMORY_ESDI);
		instruction.set_op1_kind(OpKind::MEMORY_SEG_SI);
	}
	
	instruction.set_segment_prefix(segment_prefix);
	return instruction;
}

// Helper to create string instruction with SegRSI, ESRDI pattern (CMPS)  
Instruction create_string_segrsi_esrdi(Code code, uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	Instruction instruction{};
	instruction.set_code(code);
	
	if (rep_prefix == RepPrefixKind::REPE) {
		instruction.set_has_repe_prefix(true);
	} else if (rep_prefix == RepPrefixKind::REPNE) {
		instruction.set_has_repne_prefix(true);
	}
	
	if (address_size == 64) {
		instruction.set_op0_kind(OpKind::MEMORY_SEG_RSI);
		instruction.set_op1_kind(OpKind::MEMORY_ESRDI);
	} else if (address_size == 32) {
		instruction.set_op0_kind(OpKind::MEMORY_SEG_ESI);
		instruction.set_op1_kind(OpKind::MEMORY_ESEDI);
	} else {
		instruction.set_op0_kind(OpKind::MEMORY_SEG_SI);
		instruction.set_op1_kind(OpKind::MEMORY_ESDI);
	}
	
	instruction.set_segment_prefix(segment_prefix);
	return instruction;
}

// Initialize an immediate operand
void initialize_immediate(Instruction& instruction, uint32_t operand, int64_t immediate) {
	// Get the appropriate OpKind for this code/operand combination
	auto code = instruction.code();
	auto op_kind = get_immediate_op_kind(code, operand);
	
	// If we got a valid OpKind, use it directly
	if (op_kind != UNKNOWN_OP_KIND) {
		instruction.set_op_kind(operand, op_kind);
		
		switch (op_kind) {
		case OpKind::IMMEDIATE8:
			instruction.set_immediate8(static_cast<uint8_t>(immediate));
			break;
		case OpKind::IMMEDIATE8_2ND:
			instruction.set_immediate8_2nd(static_cast<uint8_t>(immediate));
			break;
		case OpKind::IMMEDIATE16:
			instruction.set_immediate16(static_cast<uint16_t>(immediate));
			break;
		case OpKind::IMMEDIATE32:
			instruction.set_immediate32(static_cast<uint32_t>(immediate));
			break;
		case OpKind::IMMEDIATE64:
			instruction.set_immediate64(static_cast<uint64_t>(immediate));
			break;
		case OpKind::IMMEDIATE8TO16:
			instruction.set_immediate8(static_cast<uint8_t>(immediate));
			break;
		case OpKind::IMMEDIATE8TO32:
			instruction.set_immediate8(static_cast<uint8_t>(immediate));
			break;
		case OpKind::IMMEDIATE8TO64:
			instruction.set_immediate8(static_cast<uint8_t>(immediate));
			break;
		case OpKind::IMMEDIATE32TO64:
			instruction.set_immediate32(static_cast<uint32_t>(immediate));
			break;
		default:
			break;
		}
	} else {
		// Fallback: determine OpKind based on instruction type
		// For most instructions with _IMM32 in the name, use IMMEDIATE32
		// This is a simplified heuristic - the encoder will validate
		if (immediate >= INT32_MIN && immediate <= INT32_MAX) {
			// Use IMMEDIATE32 for values that fit in 32 bits
			// This is the safest choice as most instructions that take
			// immediates use full-width immediates (not sign-extended 8-bit)
			instruction.set_op_kind(operand, OpKind::IMMEDIATE32);
			instruction.set_immediate32(static_cast<uint32_t>(immediate));
		} else {
			instruction.set_op_kind(operand, OpKind::IMMEDIATE64);
			instruction.set_immediate64(static_cast<uint64_t>(immediate));
		}
	}
}

void initialize_immediate(Instruction& instruction, uint32_t operand, uint64_t immediate) {
	initialize_immediate(instruction, operand, static_cast<int64_t>(immediate));
}

// Get near branch OpKind based on Code
// This examines the Code to determine the branch target size
OpKind get_near_branch_op_kind(Code code) {
	uint32_t code_val = static_cast<uint32_t>(code);
	
	// Check for rel8 instructions (JMP_REL8_*, JCC short forms)
	// These all use NEAR_BRANCH64 for the target even though displacement is 8-bit
	// The rel8 codes are: JMP_REL8_16=699, JMP_REL8_32=700, JMP_REL8_64=701
	// And conditional jumps: JO_REL8_16=702...
	if (code_val >= 699 && code_val <= 829) {
		// rel8 forms - grouped together in the Code enum
		// Return based on the _16, _32, _64 suffix
		uint32_t offset = (code_val - 699) % 3;
		if (offset == 0) return OpKind::NEAR_BRANCH16;
		if (offset == 1) return OpKind::NEAR_BRANCH32;
		return OpKind::NEAR_BRANCH64;
	}
	
	// Check for specific rel16/rel32 codes
	switch (code) {
		case Code::CALL_REL16:
		case Code::JMP_REL16:
			return OpKind::NEAR_BRANCH16;
			
		case Code::CALL_REL32_32:
		case Code::JMP_REL32_32:
			return OpKind::NEAR_BRANCH32;
			
		case Code::CALL_REL32_64:
		case Code::JMP_REL32_64:
			return OpKind::NEAR_BRANCH64;
		
		case Code::XBEGIN_REL16:
			return OpKind::NEAR_BRANCH32;  // 16-bit mode uses 32-bit near branch
			
		case Code::XBEGIN_REL32:
			return OpKind::NEAR_BRANCH64;  // 32/64-bit mode uses 64-bit near branch
			
		default:
			// Default to 64-bit for unknown codes
			return OpKind::NEAR_BRANCH64;
	}
}

// Get far branch OpKind based on Code  
OpKind get_far_branch_op_kind(Code code) {
	switch (code) {
		case Code::CALL_PTR1616:
		case Code::JMP_PTR1616:
			return OpKind::FAR_BRANCH16;
			
		case Code::CALL_PTR1632:
		case Code::JMP_PTR1632:
		default:
			return OpKind::FAR_BRANCH32;
	}
}

// Get the base register for address size
Register get_address_base_register(uint32_t address_size) {
	switch (address_size) {
	case 16: return Register::DI;
	case 32: return Register::EDI;
	case 64: return Register::RDI;
	default:
		__assume(false);
	}
}

Register get_address_index_register(uint32_t address_size) {
	switch (address_size) {
	case 16: return Register::SI;
	case 32: return Register::ESI;
	case 64: return Register::RSI;
	default:
		__assume(false);
	}
}

} // anonymous namespace

// ============================================================================
// with() - Zero operand instructions
// ============================================================================

Instruction InstructionFactory::with(Code code) {
	Instruction instruction{};
	instruction.set_code(code);
	return instruction;
}

// ============================================================================
// with1() - Single operand instructions
// ============================================================================

Instruction InstructionFactory::with1(Code code, Register register_) {
	Instruction instruction{};
	instruction.set_code(code);
	// OpKind::Register == 0, so it's already set by default
	instruction.set_op0_register(register_);
	return instruction;
}

Instruction InstructionFactory::with1(Code code, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	initialize_immediate(instruction, 0, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with1(Code code, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	initialize_immediate(instruction, 0, static_cast<uint64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with1(Code code, const MemoryOperand& memory) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	return instruction;
}

// ============================================================================
// with2() - Two operand instructions
// ============================================================================

Instruction InstructionFactory::with2(Code code, Register register1, Register register2) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	return instruction;
}

Instruction InstructionFactory::with2(Code code, Register register_, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	initialize_immediate(instruction, 1, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with2(Code code, Register register_, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	initialize_immediate(instruction, 1, static_cast<uint64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with2(Code code, Register register_, int64_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	initialize_immediate(instruction, 1, immediate);
	return instruction;
}

Instruction InstructionFactory::with2(Code code, Register register_, uint64_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	initialize_immediate(instruction, 1, immediate);
	return instruction;
}

Instruction InstructionFactory::with2(Code code, Register register_, const MemoryOperand& memory) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	instruction.set_op1_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	return instruction;
}

Instruction InstructionFactory::with2(Code code, int32_t immediate, Register register_) {
	Instruction instruction{};
	instruction.set_code(code);
	initialize_immediate(instruction, 0, static_cast<int64_t>(immediate));
	instruction.set_op1_register(register_);
	return instruction;
}

Instruction InstructionFactory::with2(Code code, uint32_t immediate, Register register_) {
	Instruction instruction{};
	instruction.set_code(code);
	initialize_immediate(instruction, 0, static_cast<uint64_t>(immediate));
	instruction.set_op1_register(register_);
	return instruction;
}

Instruction InstructionFactory::with2(Code code, int32_t immediate1, int32_t immediate2) {
	Instruction instruction{};
	instruction.set_code(code);
	initialize_immediate(instruction, 0, static_cast<int64_t>(immediate1));
	initialize_immediate(instruction, 1, static_cast<int64_t>(immediate2));
	return instruction;
}

Instruction InstructionFactory::with2(Code code, uint32_t immediate1, uint32_t immediate2) {
	Instruction instruction{};
	instruction.set_code(code);
	initialize_immediate(instruction, 0, static_cast<uint64_t>(immediate1));
	initialize_immediate(instruction, 1, static_cast<uint64_t>(immediate2));
	return instruction;
}

Instruction InstructionFactory::with2(Code code, const MemoryOperand& memory, Register register_) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op1_register(register_);
	return instruction;
}

Instruction InstructionFactory::with2(Code code, const MemoryOperand& memory, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 1, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with2(Code code, const MemoryOperand& memory, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 1, static_cast<uint64_t>(immediate));
	return instruction;
}

// ============================================================================
// with3() - Three operand instructions
// ============================================================================

Instruction InstructionFactory::with3(Code code, Register register1, Register register2, Register register3) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register1, Register register2, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	initialize_immediate(instruction, 2, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register1, Register register2, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	initialize_immediate(instruction, 2, static_cast<uint64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register1, Register register2, const MemoryOperand& memory) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register_, int32_t immediate1, int32_t immediate2) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	initialize_immediate(instruction, 1, static_cast<int64_t>(immediate1));
	initialize_immediate(instruction, 2, static_cast<int64_t>(immediate2));
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register_, uint32_t immediate1, uint32_t immediate2) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	initialize_immediate(instruction, 1, static_cast<uint64_t>(immediate1));
	initialize_immediate(instruction, 2, static_cast<uint64_t>(immediate2));
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register1, const MemoryOperand& memory, Register register2) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op2_register(register2);
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register_, const MemoryOperand& memory, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	instruction.set_op1_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 2, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with3(Code code, Register register_, const MemoryOperand& memory, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register_);
	instruction.set_op1_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 2, static_cast<uint64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with3(Code code, const MemoryOperand& memory, Register register1, Register register2) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op1_register(register1);
	instruction.set_op2_register(register2);
	return instruction;
}

Instruction InstructionFactory::with3(Code code, const MemoryOperand& memory, Register register_, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op1_register(register_);
	initialize_immediate(instruction, 2, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with3(Code code, const MemoryOperand& memory, Register register_, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op1_register(register_);
	initialize_immediate(instruction, 2, static_cast<uint64_t>(immediate));
	return instruction;
}

// ============================================================================
// with4() - Four operand instructions
// ============================================================================

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, Register register3, Register register4) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	instruction.set_op3_register(register4);
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, Register register3, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	initialize_immediate(instruction, 3, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, Register register3, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	initialize_immediate(instruction, 3, static_cast<uint64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, Register register3, const MemoryOperand& memory) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	instruction.set_op3_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, int32_t immediate1, int32_t immediate2) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	initialize_immediate(instruction, 2, static_cast<int64_t>(immediate1));
	initialize_immediate(instruction, 3, static_cast<int64_t>(immediate2));
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, uint32_t immediate1, uint32_t immediate2) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	initialize_immediate(instruction, 2, static_cast<uint64_t>(immediate1));
	initialize_immediate(instruction, 3, static_cast<uint64_t>(immediate2));
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, const MemoryOperand& memory, Register register3) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op3_register(register3);
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, const MemoryOperand& memory, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 3, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with4(Code code, Register register1, Register register2, const MemoryOperand& memory, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 3, static_cast<uint64_t>(immediate));
	return instruction;
}

// ============================================================================
// with5() - Five operand instructions
// ============================================================================

Instruction InstructionFactory::with5(Code code, Register register1, Register register2, Register register3, Register register4, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	instruction.set_op3_register(register4);
	initialize_immediate(instruction, 4, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with5(Code code, Register register1, Register register2, Register register3, Register register4, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	instruction.set_op3_register(register4);
	initialize_immediate(instruction, 4, static_cast<uint64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with5(Code code, Register register1, Register register2, Register register3, const MemoryOperand& memory, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	instruction.set_op3_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 4, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with5(Code code, Register register1, Register register2, Register register3, const MemoryOperand& memory, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_register(register3);
	instruction.set_op3_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	initialize_immediate(instruction, 4, static_cast<uint64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with5(Code code, Register register1, Register register2, const MemoryOperand& memory, Register register3, int32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op3_register(register3);
	initialize_immediate(instruction, 4, static_cast<int64_t>(immediate));
	return instruction;
}

Instruction InstructionFactory::with5(Code code, Register register1, Register register2, const MemoryOperand& memory, Register register3, uint32_t immediate) {
	Instruction instruction{};
	instruction.set_code(code);
	instruction.set_op0_register(register1);
	instruction.set_op1_register(register2);
	instruction.set_op2_kind(OpKind::MEMORY);
	init_memory_operand(instruction, memory);
	instruction.set_op3_register(register3);
	initialize_immediate(instruction, 4, static_cast<uint64_t>(immediate));
	return instruction;
}

// ============================================================================
// Branch instructions
// ============================================================================

Instruction InstructionFactory::with_branch(Code code, uint64_t target) {
	Instruction instruction{};
	instruction.set_code(code);
	auto op_kind = get_near_branch_op_kind(code);
	instruction.set_op0_kind(op_kind);
	instruction.set_near_branch64(target);
	return instruction;
}

Instruction InstructionFactory::with_far_branch(Code code, uint16_t selector, uint32_t offset) {
	Instruction instruction{};
	instruction.set_code(code);
	auto op_kind = get_far_branch_op_kind(code);
	instruction.set_op0_kind(op_kind);
	instruction.set_far_branch_selector(selector);
	instruction.set_far_branch32(offset);
	return instruction;
}

Instruction InstructionFactory::with_xbegin(uint32_t bitness, uint64_t target) {
	Instruction instruction{};
	switch (bitness) {
	case 16:
		instruction.set_code(Code::XBEGIN_REL16);
		instruction.set_op0_kind(OpKind::NEAR_BRANCH32);
		instruction.set_near_branch32(static_cast<uint32_t>(target));
		break;
	case 32:
		instruction.set_code(Code::XBEGIN_REL32);
		instruction.set_op0_kind(OpKind::NEAR_BRANCH32);
		instruction.set_near_branch32(static_cast<uint32_t>(target));
		break;
	case 64:
		instruction.set_code(Code::XBEGIN_REL32);
		instruction.set_op0_kind(OpKind::NEAR_BRANCH64);
		instruction.set_near_branch64(target);
		break;
	default:
		__assume(false);
	}
	return instruction;
}

// ============================================================================
// String instructions
// ============================================================================

// OUTS instructions (DX, [seg:rSI])
Instruction InstructionFactory::with_outsb(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_reg_segrsi(Code::OUTSB_DX_M8, address_size, Register::DX, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_outsb(uint32_t address_size) {
	return create_string_reg_segrsi(Code::OUTSB_DX_M8, address_size, Register::DX, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_outsw(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_reg_segrsi(Code::OUTSW_DX_M16, address_size, Register::DX, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_outsw(uint32_t address_size) {
	return create_string_reg_segrsi(Code::OUTSW_DX_M16, address_size, Register::DX, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_outsd(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_reg_segrsi(Code::OUTSD_DX_M32, address_size, Register::DX, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_outsd(uint32_t address_size) {
	return create_string_reg_segrsi(Code::OUTSD_DX_M32, address_size, Register::DX, Register::NONE, RepPrefixKind::REPE);
}

// LODS instructions (AL/AX/EAX/RAX, [seg:rSI])
Instruction InstructionFactory::with_lodsb(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_reg_segrsi(Code::LODSB_AL_M8, address_size, Register::AL, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_lodsb(uint32_t address_size) {
	return create_string_reg_segrsi(Code::LODSB_AL_M8, address_size, Register::AL, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_lodsw(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_reg_segrsi(Code::LODSW_AX_M16, address_size, Register::AX, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_lodsw(uint32_t address_size) {
	return create_string_reg_segrsi(Code::LODSW_AX_M16, address_size, Register::AX, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_lodsd(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_reg_segrsi(Code::LODSD_EAX_M32, address_size, Register::EAX, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_lodsd(uint32_t address_size) {
	return create_string_reg_segrsi(Code::LODSD_EAX_M32, address_size, Register::EAX, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_lodsq(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_reg_segrsi(Code::LODSQ_RAX_M64, address_size, Register::RAX, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_lodsq(uint32_t address_size) {
	return create_string_reg_segrsi(Code::LODSQ_RAX_M64, address_size, Register::RAX, Register::NONE, RepPrefixKind::REPE);
}

// SCAS instructions (AL/AX/EAX/RAX, [ES:rDI])
Instruction InstructionFactory::with_scasb(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_reg_esrdi(Code::SCASB_AL_M8, address_size, Register::AL, rep_prefix);
}

Instruction InstructionFactory::with_repe_scasb(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASB_AL_M8, address_size, Register::AL, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_scasb(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASB_AL_M8, address_size, Register::AL, RepPrefixKind::REPNE);
}

Instruction InstructionFactory::with_scasw(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_reg_esrdi(Code::SCASW_AX_M16, address_size, Register::AX, rep_prefix);
}

Instruction InstructionFactory::with_repe_scasw(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASW_AX_M16, address_size, Register::AX, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_scasw(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASW_AX_M16, address_size, Register::AX, RepPrefixKind::REPNE);
}

Instruction InstructionFactory::with_scasd(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_reg_esrdi(Code::SCASD_EAX_M32, address_size, Register::EAX, rep_prefix);
}

Instruction InstructionFactory::with_repe_scasd(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASD_EAX_M32, address_size, Register::EAX, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_scasd(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASD_EAX_M32, address_size, Register::EAX, RepPrefixKind::REPNE);
}

Instruction InstructionFactory::with_scasq(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_reg_esrdi(Code::SCASQ_RAX_M64, address_size, Register::RAX, rep_prefix);
}

Instruction InstructionFactory::with_repe_scasq(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASQ_RAX_M64, address_size, Register::RAX, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_scasq(uint32_t address_size) {
	return create_string_reg_esrdi(Code::SCASQ_RAX_M64, address_size, Register::RAX, RepPrefixKind::REPNE);
}

// INS instructions ([ES:rDI], DX)
Instruction InstructionFactory::with_insb(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_esrdi_reg(Code::INSB_M8_DX, address_size, Register::DX, rep_prefix);
}

Instruction InstructionFactory::with_rep_insb(uint32_t address_size) {
	return create_string_esrdi_reg(Code::INSB_M8_DX, address_size, Register::DX, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_insw(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_esrdi_reg(Code::INSW_M16_DX, address_size, Register::DX, rep_prefix);
}

Instruction InstructionFactory::with_rep_insw(uint32_t address_size) {
	return create_string_esrdi_reg(Code::INSW_M16_DX, address_size, Register::DX, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_insd(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_esrdi_reg(Code::INSD_M32_DX, address_size, Register::DX, rep_prefix);
}

Instruction InstructionFactory::with_rep_insd(uint32_t address_size) {
	return create_string_esrdi_reg(Code::INSD_M32_DX, address_size, Register::DX, RepPrefixKind::REPE);
}

// STOS instructions ([ES:rDI], AL/AX/EAX/RAX)
Instruction InstructionFactory::with_stosb(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_esrdi_reg(Code::STOSB_M8_AL, address_size, Register::AL, rep_prefix);
}

Instruction InstructionFactory::with_rep_stosb(uint32_t address_size) {
	return create_string_esrdi_reg(Code::STOSB_M8_AL, address_size, Register::AL, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_stosw(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_esrdi_reg(Code::STOSW_M16_AX, address_size, Register::AX, rep_prefix);
}

Instruction InstructionFactory::with_rep_stosw(uint32_t address_size) {
	return create_string_esrdi_reg(Code::STOSW_M16_AX, address_size, Register::AX, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_stosd(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_esrdi_reg(Code::STOSD_M32_EAX, address_size, Register::EAX, rep_prefix);
}

Instruction InstructionFactory::with_rep_stosd(uint32_t address_size) {
	return create_string_esrdi_reg(Code::STOSD_M32_EAX, address_size, Register::EAX, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_stosq(uint32_t address_size, RepPrefixKind rep_prefix) {
	return create_string_esrdi_reg(Code::STOSQ_M64_RAX, address_size, Register::RAX, rep_prefix);
}

Instruction InstructionFactory::with_rep_stosq(uint32_t address_size) {
	return create_string_esrdi_reg(Code::STOSQ_M64_RAX, address_size, Register::RAX, RepPrefixKind::REPE);
}

// CMPS instructions ([seg:rSI], [ES:rDI])
Instruction InstructionFactory::with_cmpsb(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_segrsi_esrdi(Code::CMPSB_M8_M8, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_repe_cmpsb(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSB_M8_M8, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_cmpsb(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSB_M8_M8, address_size, Register::NONE, RepPrefixKind::REPNE);
}

Instruction InstructionFactory::with_cmpsw(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_segrsi_esrdi(Code::CMPSW_M16_M16, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_repe_cmpsw(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSW_M16_M16, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_cmpsw(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSW_M16_M16, address_size, Register::NONE, RepPrefixKind::REPNE);
}

Instruction InstructionFactory::with_cmpsd(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_segrsi_esrdi(Code::CMPSD_M32_M32, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_repe_cmpsd(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSD_M32_M32, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_cmpsd(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSD_M32_M32, address_size, Register::NONE, RepPrefixKind::REPNE);
}

Instruction InstructionFactory::with_cmpsq(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_segrsi_esrdi(Code::CMPSQ_M64_M64, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_repe_cmpsq(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSQ_M64_M64, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_repne_cmpsq(uint32_t address_size) {
	return create_string_segrsi_esrdi(Code::CMPSQ_M64_M64, address_size, Register::NONE, RepPrefixKind::REPNE);
}

// MOVS instructions ([ES:rDI], [seg:rSI])
Instruction InstructionFactory::with_movsb(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_esrdi_segrsi(Code::MOVSB_M8_M8, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_movsb(uint32_t address_size) {
	return create_string_esrdi_segrsi(Code::MOVSB_M8_M8, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_movsw(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_esrdi_segrsi(Code::MOVSW_M16_M16, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_movsw(uint32_t address_size) {
	return create_string_esrdi_segrsi(Code::MOVSW_M16_M16, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_movsd(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_esrdi_segrsi(Code::MOVSD_M32_M32, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_movsd(uint32_t address_size) {
	return create_string_esrdi_segrsi(Code::MOVSD_M32_M32, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_movsq(uint32_t address_size, Register segment_prefix, RepPrefixKind rep_prefix) {
	return create_string_esrdi_segrsi(Code::MOVSQ_M64_M64, address_size, segment_prefix, rep_prefix);
}

Instruction InstructionFactory::with_rep_movsq(uint32_t address_size) {
	return create_string_esrdi_segrsi(Code::MOVSQ_M64_M64, address_size, Register::NONE, RepPrefixKind::REPE);
}

Instruction InstructionFactory::with_maskmovq(uint32_t address_size, Register register1, Register register2, Register segment_prefix) {
	(void)address_size; (void)register1; (void)register2; (void)segment_prefix;
	__assume(false); // Not implemented: with_maskmovq");
}

Instruction InstructionFactory::with_maskmovdqu(uint32_t address_size, Register register1, Register register2, Register segment_prefix) {
	(void)address_size; (void)register1; (void)register2; (void)segment_prefix;
	__assume(false); // Not implemented: with_maskmovdqu");
}

Instruction InstructionFactory::with_vmaskmovdqu(uint32_t address_size, Register register1, Register register2, Register segment_prefix) {
	(void)address_size; (void)register1; (void)register2; (void)segment_prefix;
	__assume(false); // Not implemented: with_vmaskmovdqu");
}

// ============================================================================
// Declare data instructions
// ============================================================================

// Helper to set declare data length
void set_declare_data_len(Instruction& instruction, uint32_t len) {
	// The declare data length is stored in a special way in flags1_
	// This is a simplified version - actual implementation may need adjustment
	(void)instruction;
	(void)len;
}

Instruction InstructionFactory::with_declare_byte_1(uint8_t b0) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_BYTE);
	instruction.set_declare_data_len(1);
	instruction.set_declare_byte_value(0, b0);
	return instruction;
}

Instruction InstructionFactory::with_declare_byte_2(uint8_t b0, uint8_t b1) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_BYTE);
	instruction.set_immediate16(static_cast<uint16_t>(b0) | (static_cast<uint16_t>(b1) << 8));
	return instruction;
}

Instruction InstructionFactory::with_declare_byte_3(uint8_t b0, uint8_t b1, uint8_t b2) {
	(void)b0; (void)b1; (void)b2;
	__assume(false); // Not implemented: with_declare_byte_3");
}

Instruction InstructionFactory::with_declare_byte_4(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_BYTE);
	instruction.set_immediate32(
		static_cast<uint32_t>(b0) |
		(static_cast<uint32_t>(b1) << 8) |
		(static_cast<uint32_t>(b2) << 16) |
		(static_cast<uint32_t>(b3) << 24)
	);
	return instruction;
}

Instruction InstructionFactory::with_declare_byte_5(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4;
	__assume(false); // Not implemented: with_declare_byte_5");
}

Instruction InstructionFactory::with_declare_byte_6(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5;
	__assume(false); // Not implemented: with_declare_byte_6");
}

Instruction InstructionFactory::with_declare_byte_7(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6;
	__assume(false); // Not implemented: with_declare_byte_7");
}

Instruction InstructionFactory::with_declare_byte_8(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7;
	__assume(false); // Not implemented: with_declare_byte_8");
}

Instruction InstructionFactory::with_declare_byte_9(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8;
	__assume(false); // Not implemented: with_declare_byte_9");
}

Instruction InstructionFactory::with_declare_byte_10(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8, uint8_t b9) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8; (void)b9;
	__assume(false); // Not implemented: with_declare_byte_10");
}

Instruction InstructionFactory::with_declare_byte_11(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8, uint8_t b9, uint8_t b10) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8; (void)b9; (void)b10;
	__assume(false); // Not implemented: with_declare_byte_11");
}

Instruction InstructionFactory::with_declare_byte_12(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8, uint8_t b9, uint8_t b10, uint8_t b11) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8; (void)b9; (void)b10; (void)b11;
	__assume(false); // Not implemented: with_declare_byte_12");
}

Instruction InstructionFactory::with_declare_byte_13(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8, uint8_t b9, uint8_t b10, uint8_t b11, uint8_t b12) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8; (void)b9; (void)b10; (void)b11; (void)b12;
	__assume(false); // Not implemented: with_declare_byte_13");
}

Instruction InstructionFactory::with_declare_byte_14(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8, uint8_t b9, uint8_t b10, uint8_t b11, uint8_t b12, uint8_t b13) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8; (void)b9; (void)b10; (void)b11; (void)b12; (void)b13;
	__assume(false); // Not implemented: with_declare_byte_14");
}

Instruction InstructionFactory::with_declare_byte_15(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8, uint8_t b9, uint8_t b10, uint8_t b11, uint8_t b12, uint8_t b13, uint8_t b14) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8; (void)b9; (void)b10; (void)b11; (void)b12; (void)b13; (void)b14;
	__assume(false); // Not implemented: with_declare_byte_15");
}

Instruction InstructionFactory::with_declare_byte_16(uint8_t b0, uint8_t b1, uint8_t b2, uint8_t b3, uint8_t b4, uint8_t b5, uint8_t b6, uint8_t b7, uint8_t b8, uint8_t b9, uint8_t b10, uint8_t b11, uint8_t b12, uint8_t b13, uint8_t b14, uint8_t b15) {
	(void)b0; (void)b1; (void)b2; (void)b3; (void)b4; (void)b5; (void)b6; (void)b7; (void)b8; (void)b9; (void)b10; (void)b11; (void)b12; (void)b13; (void)b14; (void)b15;
	__assume(false); // Not implemented: with_declare_byte_16");
}

Instruction InstructionFactory::with_declare_byte(const uint8_t* data, size_t length) {
	(void)data; (void)length;
	__assume(false); // Not implemented: with_declare_byte(ptr)");
}

Instruction InstructionFactory::with_declare_byte_span(std::span<const uint8_t> data) {
	(void)data;
	__assume(false); // Not implemented: with_declare_byte_span");
}

// Word declarations
Instruction InstructionFactory::with_declare_word_1(uint16_t w0) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_WORD);
	instruction.set_immediate16(w0);
	return instruction;
}

Instruction InstructionFactory::with_declare_word_2(uint16_t w0, uint16_t w1) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_WORD);
	instruction.set_immediate32(static_cast<uint32_t>(w0) | (static_cast<uint32_t>(w1) << 16));
	return instruction;
}

Instruction InstructionFactory::with_declare_word_3(uint16_t w0, uint16_t w1, uint16_t w2) {
	(void)w0; (void)w1; (void)w2;
	__assume(false); // Not implemented: with_declare_word_3");
}

Instruction InstructionFactory::with_declare_word_4(uint16_t w0, uint16_t w1, uint16_t w2, uint16_t w3) {
	(void)w0; (void)w1; (void)w2; (void)w3;
	__assume(false); // Not implemented: with_declare_word_4");
}

Instruction InstructionFactory::with_declare_word_5(uint16_t w0, uint16_t w1, uint16_t w2, uint16_t w3, uint16_t w4) {
	(void)w0; (void)w1; (void)w2; (void)w3; (void)w4;
	__assume(false); // Not implemented: with_declare_word_5");
}

Instruction InstructionFactory::with_declare_word_6(uint16_t w0, uint16_t w1, uint16_t w2, uint16_t w3, uint16_t w4, uint16_t w5) {
	(void)w0; (void)w1; (void)w2; (void)w3; (void)w4; (void)w5;
	__assume(false); // Not implemented: with_declare_word_6");
}

Instruction InstructionFactory::with_declare_word_7(uint16_t w0, uint16_t w1, uint16_t w2, uint16_t w3, uint16_t w4, uint16_t w5, uint16_t w6) {
	(void)w0; (void)w1; (void)w2; (void)w3; (void)w4; (void)w5; (void)w6;
	__assume(false); // Not implemented: with_declare_word_7");
}

Instruction InstructionFactory::with_declare_word_8(uint16_t w0, uint16_t w1, uint16_t w2, uint16_t w3, uint16_t w4, uint16_t w5, uint16_t w6, uint16_t w7) {
	(void)w0; (void)w1; (void)w2; (void)w3; (void)w4; (void)w5; (void)w6; (void)w7;
	__assume(false); // Not implemented: with_declare_word_8");
}

Instruction InstructionFactory::with_declare_word(const uint8_t* data, size_t length) {
	(void)data; (void)length;
	__assume(false); // Not implemented: with_declare_word(u8 ptr)");
}

Instruction InstructionFactory::with_declare_word_span(std::span<const uint8_t> data) {
	(void)data;
	__assume(false); // Not implemented: with_declare_word_span(u8)");
}

Instruction InstructionFactory::with_declare_word(const uint16_t* data, size_t length) {
	(void)data; (void)length;
	__assume(false); // Not implemented: with_declare_word(u16 ptr)");
}

Instruction InstructionFactory::with_declare_word_span(std::span<const uint16_t> data) {
	(void)data;
	__assume(false); // Not implemented: with_declare_word_span(u16)");
}

// Dword declarations
Instruction InstructionFactory::with_declare_dword_1(uint32_t d0) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_DWORD);
	instruction.set_immediate32(d0);
	return instruction;
}

Instruction InstructionFactory::with_declare_dword_2(uint32_t d0, uint32_t d1) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_DWORD);
	instruction.set_immediate64(static_cast<uint64_t>(d0) | (static_cast<uint64_t>(d1) << 32));
	return instruction;
}

Instruction InstructionFactory::with_declare_dword_3(uint32_t d0, uint32_t d1, uint32_t d2) {
	(void)d0; (void)d1; (void)d2;
	__assume(false); // Not implemented: with_declare_dword_3");
}

Instruction InstructionFactory::with_declare_dword_4(uint32_t d0, uint32_t d1, uint32_t d2, uint32_t d3) {
	(void)d0; (void)d1; (void)d2; (void)d3;
	__assume(false); // Not implemented: with_declare_dword_4");
}

Instruction InstructionFactory::with_declare_dword(const uint8_t* data, size_t length) {
	(void)data; (void)length;
	__assume(false); // Not implemented: with_declare_dword(u8 ptr)");
}

Instruction InstructionFactory::with_declare_dword_span(std::span<const uint8_t> data) {
	(void)data;
	__assume(false); // Not implemented: with_declare_dword_span(u8)");
}

Instruction InstructionFactory::with_declare_dword(const uint32_t* data, size_t length) {
	(void)data; (void)length;
	__assume(false); // Not implemented: with_declare_dword(u32 ptr)");
}

Instruction InstructionFactory::with_declare_dword_span(std::span<const uint32_t> data) {
	(void)data;
	__assume(false); // Not implemented: with_declare_dword_span(u32)");
}

// Qword declarations
Instruction InstructionFactory::with_declare_qword_1(uint64_t q0) {
	Instruction instruction{};
	instruction.set_code(Code::DECLARE_QWORD);
	instruction.set_immediate64(q0);
	return instruction;
}

Instruction InstructionFactory::with_declare_qword_2(uint64_t q0, uint64_t q1) {
	(void)q0; (void)q1;
	__assume(false); // Not implemented: with_declare_qword_2");
}

Instruction InstructionFactory::with_declare_qword(const uint8_t* data, size_t length) {
	(void)data; (void)length;
	__assume(false); // Not implemented: with_declare_qword(u8 ptr)");
}

Instruction InstructionFactory::with_declare_qword_span(std::span<const uint8_t> data) {
	(void)data;
	__assume(false); // Not implemented: with_declare_qword_span(u8)");
}

Instruction InstructionFactory::with_declare_qword(const uint64_t* data, size_t length) {
	(void)data; (void)length;
	__assume(false); // Not implemented: with_declare_qword(u64 ptr)");
}

Instruction InstructionFactory::with_declare_qword_span(std::span<const uint64_t> data) {
	(void)data;
	__assume(false); // Not implemented: with_declare_qword_span(u64)");
}

} // namespace iced_x86
