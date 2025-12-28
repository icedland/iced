// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_INSTRUCTION_INFO_HPP
#define ICED_X86_INSTRUCTION_INFO_HPP

#ifndef ICED_X86_NO_INSTR_INFO

#include "instruction.hpp"
#include "register.hpp"
#include "op_access.hpp"
#include "memory_size.hpp"
#include "code_size.hpp"
#include "code.hpp"
#include "rflags_bits.hpp"
#include "cpuid_feature.hpp"
#include "flow_control.hpp"
#include "encoding_kind.hpp"
#include "condition_code.hpp"
#include <cstdint>
#include <array>
#include <vector>
#include <span>

namespace iced_x86 {

/// @brief A register used by an instruction.
struct UsedRegister {
	Register register_ = Register::NONE;  ///< The register
	OpAccess access = OpAccess::NONE;     ///< How the register is accessed
	
	constexpr UsedRegister() noexcept = default;
	constexpr UsedRegister( Register reg, OpAccess acc ) noexcept 
		: register_( reg ), access( acc ) {}
};

/// @brief A memory location used by an instruction.
struct UsedMemory {
	Register segment = Register::NONE;         ///< Effective segment register or NONE
	Register base = Register::NONE;            ///< Base register or NONE
	Register index = Register::NONE;           ///< Index register or NONE
	uint32_t scale = 1;                        ///< Index scale (1, 2, 4, or 8)
	uint64_t displacement = 0;                 ///< Memory displacement
	MemorySize memory_size = MemorySize::UNKNOWN; ///< Size of memory access
	OpAccess access = OpAccess::NONE;          ///< How memory is accessed
	CodeSize address_size = CodeSize::UNKNOWN; ///< Address size
	uint32_t vsib_size = 0;                    ///< VSIB size (0, 4, or 8)
	
	constexpr UsedMemory() noexcept = default;
};

/// @brief Options for InstructionInfoFactory.
enum class InstructionInfoOptions : uint32_t {
	/// @brief No options
	NONE = 0,
	/// @brief Don't include memory usage information
	NO_MEMORY_USAGE = 1,
	/// @brief Don't include register usage information
	NO_REGISTER_USAGE = 2
};

inline InstructionInfoOptions operator|( InstructionInfoOptions a, InstructionInfoOptions b ) noexcept {
	return static_cast<InstructionInfoOptions>( static_cast<uint32_t>( a ) | static_cast<uint32_t>( b ) );
}

inline InstructionInfoOptions operator&( InstructionInfoOptions a, InstructionInfoOptions b ) noexcept {
	return static_cast<InstructionInfoOptions>( static_cast<uint32_t>( a ) & static_cast<uint32_t>( b ) );
}

/// @brief Contains information about an instruction, such as register/memory access.
class InstructionInfo {
public:
	/// @brief Gets operand 0's access type.
	[[nodiscard]] OpAccess op0_access() const noexcept { return op_accesses_[0]; }
	
	/// @brief Gets operand 1's access type.
	[[nodiscard]] OpAccess op1_access() const noexcept { return op_accesses_[1]; }
	
	/// @brief Gets operand 2's access type.
	[[nodiscard]] OpAccess op2_access() const noexcept { return op_accesses_[2]; }
	
	/// @brief Gets operand 3's access type.
	[[nodiscard]] OpAccess op3_access() const noexcept { return op_accesses_[3]; }
	
	/// @brief Gets operand 4's access type.
	[[nodiscard]] OpAccess op4_access() const noexcept { return op_accesses_[4]; }
	
	/// @brief Gets an operand's access type.
	/// @param operand Operand index (0-4)
	[[nodiscard]] OpAccess op_access( uint32_t operand ) const noexcept {
		return operand < 5 ? op_accesses_[operand] : OpAccess::NONE;
	}
	
	/// @brief Gets all registers used by the instruction.
	[[nodiscard]] std::span<const UsedRegister> used_registers() const noexcept {
		return used_registers_;
	}
	
	/// @brief Gets all memory locations used by the instruction.
	[[nodiscard]] std::span<const UsedMemory> used_memory() const noexcept {
		return used_memory_;
	}

private:
	friend class InstructionInfoFactory;
	
	std::array<OpAccess, 5> op_accesses_ = {};
	std::vector<UsedRegister> used_registers_;
	std::vector<UsedMemory> used_memory_;
};

/// @brief Creates InstructionInfo objects. This class can be reused to reduce allocations.
class InstructionInfoFactory {
public:
	/// @brief Creates a new factory.
	InstructionInfoFactory() = default;
	
	/// @brief Gets instruction info for the specified instruction.
	/// @param instruction Instruction to analyze
	/// @return Instruction information
	[[nodiscard]] const InstructionInfo& info( const Instruction& instruction ) noexcept;
	
	/// @brief Gets instruction info with the specified options.
	/// @param instruction Instruction to analyze
	/// @param options Analysis options
	/// @return Instruction information
	[[nodiscard]] const InstructionInfo& info( const Instruction& instruction, 
	                                           InstructionInfoOptions options ) noexcept;

private:
	void analyze( const Instruction& instruction, InstructionInfoOptions options ) noexcept;
	void add_register( Register reg, OpAccess access ) noexcept;
	void add_memory( const Instruction& instruction, OpAccess access ) noexcept;
	
	InstructionInfo info_;
};

// ============================================================================
// Instruction Extension Methods for Info Properties
// ============================================================================

namespace InstructionExtensions {

/// @brief Gets the encoding kind (Legacy, VEX, EVEX, XOP, 3DNow!, MVEX).
[[nodiscard]] EncodingKind encoding( const Instruction& instruction ) noexcept;

/// @brief Gets all CPUID features required by this instruction.
[[nodiscard]] std::span<const CpuidFeature> cpuid_features( const Instruction& instruction ) noexcept;

/// @brief Gets the control flow info.
[[nodiscard]] FlowControl flow_control( const Instruction& instruction ) noexcept;

/// @brief Returns true if this is a privileged instruction.
[[nodiscard]] bool is_privileged( const Instruction& instruction ) noexcept;

/// @brief Returns true if this is a stack instruction (PUSH, POP, CALL, RET, etc.).
[[nodiscard]] bool is_stack_instruction( const Instruction& instruction ) noexcept;

/// @brief Returns true if this instruction saves/restores many registers (PUSHA, POPA, etc.).
[[nodiscard]] bool is_save_restore_instruction( const Instruction& instruction ) noexcept;

/// @brief Gets the number of bytes added to SP/ESP/RSP or 0 if unchanged.
[[nodiscard]] int32_t stack_pointer_increment( const Instruction& instruction ) noexcept;

/// @brief Gets the condition code for Jcc, SETcc, CMOVcc, LOOPcc instructions.
[[nodiscard]] ConditionCode condition_code( const Instruction& instruction ) noexcept;

/// @brief Returns true if this is a string instruction (MOVS, CMPS, STOS, LODS, SCAS, INS, OUTS).
[[nodiscard]] bool is_string_instruction( const Instruction& instruction ) noexcept;

// === RFLAGS Analysis ===

/// @brief Gets all flags read by the instruction.
[[nodiscard]] RflagsBits::Value rflags_read( const Instruction& instruction ) noexcept;

/// @brief Gets all flags written by the instruction (excluding undefined/cleared/set).
[[nodiscard]] RflagsBits::Value rflags_written( const Instruction& instruction ) noexcept;

/// @brief Gets all flags cleared by the instruction.
[[nodiscard]] RflagsBits::Value rflags_cleared( const Instruction& instruction ) noexcept;

/// @brief Gets all flags set by the instruction.
[[nodiscard]] RflagsBits::Value rflags_set( const Instruction& instruction ) noexcept;

/// @brief Gets all flags undefined after the instruction.
[[nodiscard]] RflagsBits::Value rflags_undefined( const Instruction& instruction ) noexcept;

/// @brief Gets all flags modified by the instruction.
[[nodiscard]] RflagsBits::Value rflags_modified( const Instruction& instruction ) noexcept;

// === Branch Type Checks ===

/// @brief Returns true if Jcc SHORT or Jcc NEAR.
[[nodiscard]] bool is_jcc_short_or_near( const Instruction& instruction ) noexcept;

/// @brief Returns true if Jcc NEAR.
[[nodiscard]] bool is_jcc_near( const Instruction& instruction ) noexcept;

/// @brief Returns true if Jcc SHORT.
[[nodiscard]] bool is_jcc_short( const Instruction& instruction ) noexcept;

/// @brief Returns true if JMP SHORT.
[[nodiscard]] bool is_jmp_short( const Instruction& instruction ) noexcept;

/// @brief Returns true if JMP NEAR.
[[nodiscard]] bool is_jmp_near( const Instruction& instruction ) noexcept;

/// @brief Returns true if JMP SHORT or JMP NEAR.
[[nodiscard]] bool is_jmp_short_or_near( const Instruction& instruction ) noexcept;

/// @brief Returns true if JMP FAR.
[[nodiscard]] bool is_jmp_far( const Instruction& instruction ) noexcept;

/// @brief Returns true if CALL NEAR.
[[nodiscard]] bool is_call_near( const Instruction& instruction ) noexcept;

/// @brief Returns true if CALL FAR.
[[nodiscard]] bool is_call_far( const Instruction& instruction ) noexcept;

/// @brief Returns true if JMP NEAR reg/[mem].
[[nodiscard]] bool is_jmp_near_indirect( const Instruction& instruction ) noexcept;

/// @brief Returns true if JMP FAR [mem].
[[nodiscard]] bool is_jmp_far_indirect( const Instruction& instruction ) noexcept;

/// @brief Returns true if CALL NEAR reg/[mem].
[[nodiscard]] bool is_call_near_indirect( const Instruction& instruction ) noexcept;

/// @brief Returns true if CALL FAR [mem].
[[nodiscard]] bool is_call_far_indirect( const Instruction& instruction ) noexcept;

/// @brief Returns true if JCXZ/JECXZ/JRCXZ SHORT.
[[nodiscard]] bool is_jcx_short( const Instruction& instruction ) noexcept;

/// @brief Returns true if LOOPcc SHORT.
[[nodiscard]] bool is_loopcc( const Instruction& instruction ) noexcept;

/// @brief Returns true if LOOP SHORT.
[[nodiscard]] bool is_loop( const Instruction& instruction ) noexcept;

// === Branch Manipulation ===

/// @brief Negates the condition code (JE -> JNE, etc.).
/// @param instruction Instruction to modify (in place)
void negate_condition_code( Instruction& instruction ) noexcept;

/// @brief Converts a Jcc/JMP NEAR to SHORT.
/// @param instruction Instruction to modify (in place)  
/// @return true if successful, false if not a Jcc/JMP NEAR
[[nodiscard]] bool to_short_branch( Instruction& instruction ) noexcept;

/// @brief Converts a Jcc/JMP SHORT to NEAR.
/// @param instruction Instruction to modify (in place)
/// @return true if successful, false if not a Jcc/JMP SHORT
[[nodiscard]] bool to_near_branch( Instruction& instruction ) noexcept;

/// @brief Gets the negated condition code version of the Code.
/// @param code Code to negate
/// @return Negated Code, or same Code if not a conditional instruction
[[nodiscard]] Code negate_condition_code( Code code ) noexcept;

/// @brief Converts a Jcc/JMP NEAR Code to SHORT.
/// @param code Code to convert
/// @return SHORT version, or same Code if not NEAR
[[nodiscard]] Code to_short_branch( Code code ) noexcept;

/// @brief Converts a Jcc/JMP SHORT Code to NEAR.
/// @param code Code to convert
/// @return NEAR version, or same Code if not SHORT
[[nodiscard]] Code to_near_branch( Code code ) noexcept;

} // namespace InstructionExtensions

} // namespace iced_x86

#endif // !ICED_X86_NO_INSTR_INFO

#endif // ICED_X86_INSTRUCTION_INFO_HPP
