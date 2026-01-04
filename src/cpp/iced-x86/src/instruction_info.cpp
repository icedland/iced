// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/instruction_info.hpp"
#include "iced_x86/register_info.hpp"
#include "iced_x86/code.hpp"
#include "iced_x86/op_kind.hpp"

namespace iced_x86 {

// ============================================================================
// InstructionInfoFactory Implementation
// ============================================================================

const InstructionInfo& InstructionInfoFactory::info( const Instruction& instruction ) noexcept {
	analyze( instruction, InstructionInfoOptions::NONE );
	return info_;
}

const InstructionInfo& InstructionInfoFactory::info( const Instruction& instruction, 
                                                     InstructionInfoOptions options ) noexcept {
	analyze( instruction, options );
	return info_;
}

void InstructionInfoFactory::analyze( const Instruction& instruction, 
                                      InstructionInfoOptions options ) noexcept {
	// Clear previous state
	info_.used_registers_.clear();
	info_.used_memory_.clear();
	for ( auto& acc : info_.op_accesses_ ) {
		acc = OpAccess::NONE;
	}
	
	bool include_regs = ( static_cast<uint32_t>( options ) & 
	                      static_cast<uint32_t>( InstructionInfoOptions::NO_REGISTER_USAGE ) ) == 0;
	bool include_mem = ( static_cast<uint32_t>( options ) & 
	                     static_cast<uint32_t>( InstructionInfoOptions::NO_MEMORY_USAGE ) ) == 0;
	
	// Analyze each operand
	uint32_t op_count = instruction.op_count();
	for ( uint32_t i = 0; i < op_count; ++i ) {
		OpKind kind = instruction.op_kind( i );
		
		// Determine access type based on operand position
		// This is a simplified heuristic - real implementation would use instruction-specific data
		OpAccess access = OpAccess::READ;
		if ( i == 0 ) {
			// First operand is typically the destination
			access = OpAccess::WRITE;
			// Many instructions read-modify-write the first operand
			Code code = instruction.code();
			// ADD, SUB, AND, OR, XOR, etc. read and write first operand
			// This is a simplified check - real implementation uses tables
			if ( code >= Code::ADD_RM8_R8 && code <= Code::ADD_RAX_IMM32 ) {
				access = OpAccess::READ_WRITE;
			}
		}
		
		info_.op_accesses_[i] = access;
		
		switch ( kind ) {
			case OpKind::REGISTER:
				if ( include_regs ) {
					add_register( instruction.op_register( i ), access );
				}
				break;
				
			case OpKind::MEMORY:
				if ( include_mem ) {
					add_memory( instruction, access );
				}
				if ( include_regs ) {
					// Add base and index registers as read
					Register base = instruction.memory_base();
					Register index = instruction.memory_index();
					if ( base != Register::NONE ) {
						add_register( base, OpAccess::READ );
					}
					if ( index != Register::NONE ) {
						add_register( index, OpAccess::READ );
					}
				}
				break;
				
			default:
				// Immediate operands don't use registers or memory
				break;
		}
	}
	
	// Add implicit registers based on instruction
	// This is simplified - real implementation uses instruction-specific tables
	if ( include_regs ) {
		Code code = instruction.code();
		
		// PUSH/POP use RSP/ESP/SP
		if ( code >= Code::PUSH_R16 && code <= Code::PUSH_RM64 ) {
			add_register( Register::RSP, OpAccess::READ_WRITE );
		}
		if ( code >= Code::POP_R16 && code <= Code::POP_RM64 ) {
			add_register( Register::RSP, OpAccess::READ_WRITE );
		}
		
		// CALL/RET use RSP/ESP/SP
		if ( code >= Code::CALL_REL16 && code <= Code::CALL_M1664 ) {
			add_register( Register::RSP, OpAccess::READ_WRITE );
		}
		if ( code >= Code::RETNW && code <= Code::RETFQ ) {
			add_register( Register::RSP, OpAccess::READ_WRITE );
		}
	}
}

void InstructionInfoFactory::add_register( Register reg, OpAccess access ) noexcept {
	if ( reg == Register::NONE ) return;
	
	// Check if register already added
	for ( auto& ur : info_.used_registers_ ) {
		if ( ur.register_ == reg ) {
			// Merge access: if already READ and now WRITE, becomes READ_WRITE
			if ( ( ur.access == OpAccess::READ && access == OpAccess::WRITE ) ||
			     ( ur.access == OpAccess::WRITE && access == OpAccess::READ ) ) {
				ur.access = OpAccess::READ_WRITE;
			}
			return;
		}
	}
	
	info_.used_registers_.emplace_back( reg, access );
}

void InstructionInfoFactory::add_memory( const Instruction& instruction, OpAccess access ) noexcept {
	UsedMemory mem;
	mem.segment = instruction.memory_segment();
	mem.base = instruction.memory_base();
	mem.index = instruction.memory_index();
	mem.scale = instruction.memory_index_scale();
	mem.displacement = instruction.memory_displacement64();
	mem.memory_size = instruction.memory_size();
	mem.access = access;
	mem.address_size = instruction.code_size();
	
	info_.used_memory_.push_back( mem );
}

// ============================================================================
// InstructionExtensions Implementation
// ============================================================================

namespace InstructionExtensions {

EncodingKind encoding( const Instruction& instruction ) noexcept {
	// Determine encoding from instruction code
	// This is simplified - real implementation uses lookup tables
	Code code = instruction.code();
	(void)code;
	
	// Check for VEX-encoded instructions
	// VEX instructions typically have specific code ranges
	// For now, return Legacy as default
	return EncodingKind::LEGACY;
}

std::span<const CpuidFeature> cpuid_features( const Instruction& instruction ) noexcept {
	// This would need a large lookup table mapping Code -> CpuidFeature[]
	// For now, return empty span
	(void)instruction;
	return {};
}

FlowControl flow_control( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	
	// JMP SHORT (rel8)
	if ( code == Code::JMP_REL8_16 || code == Code::JMP_REL8_32 || code == Code::JMP_REL8_64 ) {
		return FlowControl::UNCONDITIONAL_BRANCH;
	}
	
	// JMP NEAR (rel16/rel32)
	if ( code == Code::JMP_REL16 || code == Code::JMP_REL32_32 || code == Code::JMP_REL32_64 ) {
		return FlowControl::UNCONDITIONAL_BRANCH;
	}
	
	// JMP FAR
	if ( code == Code::JMP_PTR1616 || code == Code::JMP_PTR1632 ) {
		return FlowControl::UNCONDITIONAL_BRANCH;
	}
	
	// JMP indirect
	if ( code == Code::JMP_RM16 || code == Code::JMP_RM32 || code == Code::JMP_RM64 ||
	     code == Code::JMP_M1616 || code == Code::JMP_M1632 || code == Code::JMP_M1664 ) {
		return FlowControl::INDIRECT_BRANCH;
	}
	
	// Jcc SHORT (rel8) - codes 159-206
	if ( code >= Code::JO_REL8_16 && code <= Code::JG_REL8_64 ) {
		return FlowControl::CONDITIONAL_BRANCH;
	}
	
	// Jcc NEAR (rel16/rel32) - codes 1854-1901
	if ( code >= Code::JO_REL16 && code <= Code::JG_REL32_64 ) {
		return FlowControl::CONDITIONAL_BRANCH;
	}
	
	// CALL NEAR
	if ( code == Code::CALL_REL16 || code == Code::CALL_REL32_32 || code == Code::CALL_REL32_64 ) {
		return FlowControl::CALL;
	}
	
	// CALL FAR
	if ( code == Code::CALL_PTR1616 || code == Code::CALL_PTR1632 ) {
		return FlowControl::CALL;
	}
	
	// CALL indirect
	if ( code == Code::CALL_RM16 || code == Code::CALL_RM32 || code == Code::CALL_RM64 ||
	     code == Code::CALL_M1616 || code == Code::CALL_M1632 || code == Code::CALL_M1664 ) {
		return FlowControl::INDIRECT_CALL;
	}
	
	// RET near/far
	if ( code == Code::RETNW || code == Code::RETND || code == Code::RETNQ ||
	     code == Code::RETNW_IMM16 || code == Code::RETND_IMM16 || code == Code::RETNQ_IMM16 ||
	     code == Code::RETFW || code == Code::RETFD || code == Code::RETFQ ||
	     code == Code::RETFW_IMM16 || code == Code::RETFD_IMM16 || code == Code::RETFQ_IMM16 ) {
		return FlowControl::RETURN;
	}
	
	// INT
	if ( code == Code::INT3 || code == Code::INT_IMM8 || code == Code::INTO ) {
		return FlowControl::INTERRUPT;
	}
	
	return FlowControl::NEXT;
}

bool is_privileged( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	
	// CLI, STI, HLT, LGDT, LIDT, etc. are privileged
	if ( code == Code::CLI || code == Code::STI || code == Code::HLT ) {
		return true;
	}
	
	// This is simplified - real implementation uses lookup tables
	return false;
}

bool is_stack_instruction( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	
	// PUSH
	if ( code == Code::PUSH_R16 || code == Code::PUSH_R32 || code == Code::PUSH_R64 ||
	     code == Code::PUSH_IMM16 ||
	     code == Code::PUSH_RM16 || code == Code::PUSH_RM32 || code == Code::PUSH_RM64 ) {
		return true;
	}
	
	// POP
	if ( code == Code::POP_R16 || code == Code::POP_R32 || code == Code::POP_R64 ||
	     code == Code::POP_RM16 || code == Code::POP_RM32 || code == Code::POP_RM64 ) {
		return true;
	}
	
	// CALL
	if ( code == Code::CALL_REL16 || code == Code::CALL_REL32_32 || code == Code::CALL_REL32_64 ||
	     code == Code::CALL_PTR1616 || code == Code::CALL_PTR1632 ||
	     code == Code::CALL_RM16 || code == Code::CALL_RM32 || code == Code::CALL_RM64 ||
	     code == Code::CALL_M1616 || code == Code::CALL_M1632 || code == Code::CALL_M1664 ) {
		return true;
	}
	
	// RET
	if ( code == Code::RETNW || code == Code::RETND || code == Code::RETNQ ||
	     code == Code::RETNW_IMM16 || code == Code::RETND_IMM16 || code == Code::RETNQ_IMM16 ||
	     code == Code::RETFW || code == Code::RETFD || code == Code::RETFQ ||
	     code == Code::RETFW_IMM16 || code == Code::RETFD_IMM16 || code == Code::RETFQ_IMM16 ) {
		return true;
	}
	
	// ENTER/LEAVE
	if ( code == Code::ENTERW_IMM16_IMM8 || code == Code::ENTERD_IMM16_IMM8 || code == Code::ENTERQ_IMM16_IMM8 ||
	     code == Code::LEAVEW || code == Code::LEAVED || code == Code::LEAVEQ ) {
		return true;
	}
	
	return false;
}

bool is_save_restore_instruction( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	
	// PUSHA, POPA, PUSHF, POPF
	if ( code == Code::PUSHAW || code == Code::PUSHAD || 
	     code == Code::POPAW || code == Code::POPAD ) {
		return true;
	}
	
	return false;
}

int32_t stack_pointer_increment( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	
	// PUSH decrements, POP increments
	if ( code >= Code::PUSH_R16 && code <= Code::PUSH_RM64 ) {
		// Size depends on operand size
		return -8;  // Simplified - assume 64-bit
	}
	if ( code >= Code::POP_R16 && code <= Code::POP_RM64 ) {
		return 8;   // Simplified - assume 64-bit
	}
	if ( code >= Code::CALL_REL16 && code <= Code::CALL_M1664 ) {
		return -8;  // CALL pushes return address
	}
	if ( code >= Code::RETNW && code <= Code::RETFQ ) {
		return 8;   // RET pops return address
	}
	
	return 0;
}

ConditionCode condition_code( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	
	// Map Jcc instructions to condition codes
	// Check SHORT versions (rel8)
	if ( code == Code::JO_REL8_16 || code == Code::JO_REL8_32 || code == Code::JO_REL8_64 ||
	     code == Code::JO_REL16 || code == Code::JO_REL32_32 || code == Code::JO_REL32_64 ) {
		return ConditionCode::O;
	}
	if ( code == Code::JNO_REL8_16 || code == Code::JNO_REL8_32 || code == Code::JNO_REL8_64 ||
	     code == Code::JNO_REL16 || code == Code::JNO_REL32_32 || code == Code::JNO_REL32_64 ) {
		return ConditionCode::NO;
	}
	if ( code == Code::JB_REL8_16 || code == Code::JB_REL8_32 || code == Code::JB_REL8_64 ||
	     code == Code::JB_REL16 || code == Code::JB_REL32_32 || code == Code::JB_REL32_64 ) {
		return ConditionCode::B;
	}
	if ( code == Code::JAE_REL8_16 || code == Code::JAE_REL8_32 || code == Code::JAE_REL8_64 ||
	     code == Code::JAE_REL16 || code == Code::JAE_REL32_32 || code == Code::JAE_REL32_64 ) {
		return ConditionCode::AE;
	}
	if ( code == Code::JE_REL8_16 || code == Code::JE_REL8_32 || code == Code::JE_REL8_64 ||
	     code == Code::JE_REL16 || code == Code::JE_REL32_32 || code == Code::JE_REL32_64 ) {
		return ConditionCode::E;
	}
	if ( code == Code::JNE_REL8_16 || code == Code::JNE_REL8_32 || code == Code::JNE_REL8_64 ||
	     code == Code::JNE_REL16 || code == Code::JNE_REL32_32 || code == Code::JNE_REL32_64 ) {
		return ConditionCode::NE;
	}
	if ( code == Code::JBE_REL8_16 || code == Code::JBE_REL8_32 || code == Code::JBE_REL8_64 ||
	     code == Code::JBE_REL16 || code == Code::JBE_REL32_32 || code == Code::JBE_REL32_64 ) {
		return ConditionCode::BE;
	}
	if ( code == Code::JA_REL8_16 || code == Code::JA_REL8_32 || code == Code::JA_REL8_64 ||
	     code == Code::JA_REL16 || code == Code::JA_REL32_32 || code == Code::JA_REL32_64 ) {
		return ConditionCode::A;
	}
	
	return ConditionCode::NONE;
}

bool is_string_instruction( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	
	// MOVS, CMPS, STOS, LODS, SCAS, INS, OUTS
	if ( code >= Code::MOVSB_M8_M8 && code <= Code::MOVSQ_M64_M64 ) return true;
	if ( code >= Code::CMPSB_M8_M8 && code <= Code::CMPSQ_M64_M64 ) return true;
	if ( code >= Code::STOSB_M8_AL && code <= Code::STOSQ_M64_RAX ) return true;
	if ( code >= Code::LODSB_AL_M8 && code <= Code::LODSQ_RAX_M64 ) return true;
	if ( code >= Code::SCASB_AL_M8 && code <= Code::SCASQ_RAX_M64 ) return true;
	if ( code >= Code::INSB_M8_DX && code <= Code::INSD_M32_DX ) return true;
	if ( code >= Code::OUTSB_DX_M8 && code <= Code::OUTSD_DX_M32 ) return true;
	
	return false;
}

// === RFLAGS Analysis ===
// These would need large lookup tables in a complete implementation

RflagsBits::Value rflags_read( const Instruction& instruction ) noexcept {
	// Simplified - Jcc instructions read flags
	Code code = instruction.code();
	if ( code >= Code::JO_REL8_16 && code <= Code::JG_REL32_64 ) {
		// Each Jcc reads specific flags
		return RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::CF | RflagsBits::PF;
	}
	return RflagsBits::NONE;
}

RflagsBits::Value rflags_written( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	// ADD, SUB, etc. write flags
	if ( code >= Code::ADD_RM8_R8 && code <= Code::ADD_RAX_IMM32 ) {
		return RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF;
	}
	return RflagsBits::NONE;
}

RflagsBits::Value rflags_cleared( const Instruction& instruction ) noexcept {
	(void)instruction;
	return RflagsBits::NONE;
}

RflagsBits::Value rflags_set( const Instruction& instruction ) noexcept {
	(void)instruction;
	return RflagsBits::NONE;
}

RflagsBits::Value rflags_undefined( const Instruction& instruction ) noexcept {
	(void)instruction;
	return RflagsBits::NONE;
}

RflagsBits::Value rflags_modified( const Instruction& instruction ) noexcept {
	return rflags_written( instruction ) | rflags_cleared( instruction ) | 
	       rflags_set( instruction ) | rflags_undefined( instruction );
}

// === Branch Type Checks ===

bool is_jcc_short_or_near( const Instruction& instruction ) noexcept {
	return is_jcc_short( instruction ) || is_jcc_near( instruction );
}

bool is_jcc_near( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	// Jcc NEAR (rel16/rel32)
	return ( code >= Code::JO_REL16 && code <= Code::JG_REL32_64 );
}

bool is_jcc_short( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	// Jcc SHORT (rel8)
	return ( code >= Code::JO_REL8_16 && code <= Code::JG_REL8_64 );
}

bool is_jmp_short( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code == Code::JMP_REL8_16 || code == Code::JMP_REL8_32 || code == Code::JMP_REL8_64;
}

bool is_jmp_near( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code == Code::JMP_REL16 || code == Code::JMP_REL32_32 || code == Code::JMP_REL32_64;
}

bool is_jmp_short_or_near( const Instruction& instruction ) noexcept {
	return is_jmp_short( instruction ) || is_jmp_near( instruction );
}

bool is_jmp_far( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code == Code::JMP_PTR1616 || code == Code::JMP_PTR1632;
}

bool is_call_near( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code == Code::CALL_REL16 || code == Code::CALL_REL32_32 || code == Code::CALL_REL32_64;
}

bool is_call_far( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code == Code::CALL_PTR1616 || code == Code::CALL_PTR1632;
}

bool is_jmp_near_indirect( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code >= Code::JMP_RM16 && code <= Code::JMP_RM64;
}

bool is_jmp_far_indirect( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code >= Code::JMP_M1616 && code <= Code::JMP_M1664;
}

bool is_call_near_indirect( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code >= Code::CALL_RM16 && code <= Code::CALL_RM64;
}

bool is_call_far_indirect( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code >= Code::CALL_M1616 && code <= Code::CALL_M1664;
}

bool is_jcx_short( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code == Code::JCXZ_REL8_16 || code == Code::JCXZ_REL8_32 || 
	       code == Code::JECXZ_REL8_16 || code == Code::JECXZ_REL8_32 || code == Code::JECXZ_REL8_64 ||
	       code == Code::JRCXZ_REL8_64;
}

bool is_loopcc( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code >= Code::LOOPNE_REL8_16_CX && code <= Code::LOOPE_REL8_64_RCX;
}

bool is_loop( const Instruction& instruction ) noexcept {
	Code code = instruction.code();
	return code >= Code::LOOP_REL8_16_CX && code <= Code::LOOP_REL8_64_RCX;
}

// === Branch Manipulation ===

void negate_condition_code( Instruction& instruction ) noexcept {
	Code new_code = negate_condition_code( instruction.code() );
	instruction.set_code( new_code );
}

bool to_short_branch( Instruction& instruction ) noexcept {
	Code new_code = to_short_branch( instruction.code() );
	if ( new_code != instruction.code() ) {
		instruction.set_code( new_code );
		return true;
	}
	return false;
}

bool to_near_branch( Instruction& instruction ) noexcept {
	Code new_code = to_near_branch( instruction.code() );
	if ( new_code != instruction.code() ) {
		instruction.set_code( new_code );
		return true;
	}
	return false;
}

Code negate_condition_code( Code code ) noexcept {
	// JO <-> JNO, JB <-> JAE, JE <-> JNE, etc.
	// This is a simplified implementation - real one uses lookup tables
	switch ( code ) {
		case Code::JO_REL8_16: return Code::JNO_REL8_16;
		case Code::JO_REL8_32: return Code::JNO_REL8_32;
		case Code::JO_REL8_64: return Code::JNO_REL8_64;
		case Code::JNO_REL8_16: return Code::JO_REL8_16;
		case Code::JNO_REL8_32: return Code::JO_REL8_32;
		case Code::JNO_REL8_64: return Code::JO_REL8_64;
		
		case Code::JE_REL8_16: return Code::JNE_REL8_16;
		case Code::JE_REL8_32: return Code::JNE_REL8_32;
		case Code::JE_REL8_64: return Code::JNE_REL8_64;
		case Code::JNE_REL8_16: return Code::JE_REL8_16;
		case Code::JNE_REL8_32: return Code::JE_REL8_32;
		case Code::JNE_REL8_64: return Code::JE_REL8_64;
		
		// Add more mappings as needed...
		default: return code;
	}
}

Code to_short_branch( Code code ) noexcept {
	// Convert NEAR to SHORT
	switch ( code ) {
		case Code::JO_REL16: return Code::JO_REL8_16;
		case Code::JO_REL32_32: return Code::JO_REL8_32;
		case Code::JO_REL32_64: return Code::JO_REL8_64;
		
		case Code::JNO_REL16: return Code::JNO_REL8_16;
		case Code::JNO_REL32_32: return Code::JNO_REL8_32;
		case Code::JNO_REL32_64: return Code::JNO_REL8_64;
		
		case Code::JE_REL16: return Code::JE_REL8_16;
		case Code::JE_REL32_32: return Code::JE_REL8_32;
		case Code::JE_REL32_64: return Code::JE_REL8_64;
		
		case Code::JNE_REL16: return Code::JNE_REL8_16;
		case Code::JNE_REL32_32: return Code::JNE_REL8_32;
		case Code::JNE_REL32_64: return Code::JNE_REL8_64;
		
		case Code::JMP_REL16: return Code::JMP_REL8_16;
		case Code::JMP_REL32_32: return Code::JMP_REL8_32;
		case Code::JMP_REL32_64: return Code::JMP_REL8_64;
		
		// Add more mappings as needed...
		default: return code;
	}
}

Code to_near_branch( Code code ) noexcept {
	// Convert SHORT to NEAR
	switch ( code ) {
		case Code::JO_REL8_16: return Code::JO_REL16;
		case Code::JO_REL8_32: return Code::JO_REL32_32;
		case Code::JO_REL8_64: return Code::JO_REL32_64;
		
		case Code::JNO_REL8_16: return Code::JNO_REL16;
		case Code::JNO_REL8_32: return Code::JNO_REL32_32;
		case Code::JNO_REL8_64: return Code::JNO_REL32_64;
		
		case Code::JE_REL8_16: return Code::JE_REL16;
		case Code::JE_REL8_32: return Code::JE_REL32_32;
		case Code::JE_REL8_64: return Code::JE_REL32_64;
		
		case Code::JNE_REL8_16: return Code::JNE_REL16;
		case Code::JNE_REL8_32: return Code::JNE_REL32_32;
		case Code::JNE_REL8_64: return Code::JNE_REL32_64;
		
		case Code::JMP_REL8_16: return Code::JMP_REL16;
		case Code::JMP_REL8_32: return Code::JMP_REL32_32;
		case Code::JMP_REL8_64: return Code::JMP_REL32_64;
		
		// Add more mappings as needed...
		default: return code;
	}
}

} // namespace InstructionExtensions

} // namespace iced_x86
