// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_OP_CODE_INFO_HPP
#define ICED_X86_OP_CODE_INFO_HPP

#ifndef ICED_X86_NO_OP_CODE_INFO

#include "iced_x86/code.hpp"
#include "iced_x86/mnemonic.hpp"
#include "iced_x86/encoding_kind.hpp"
#include "iced_x86/mandatory_prefix.hpp"
#include "iced_x86/op_code_table_kind.hpp"
#include "iced_x86/op_code_operand_kind.hpp"
#include "iced_x86/tuple_type.hpp"
#include "iced_x86/memory_size.hpp"
#include "iced_x86/decoder_options.hpp"

#include <cstdint>
#include <string>

namespace iced_x86 {

/// @brief Opcode info for an instruction.
/// @details This class provides detailed information about an instruction's
/// encoding, operands, and properties. Use Code::op_code() or 
/// Instruction::op_code() to get an OpCodeInfo.
class OpCodeInfo {
public:
	/// @brief Gets the OpCodeInfo for a Code value.
	/// @param code The instruction code
	/// @return OpCodeInfo for the specified code
	[[nodiscard]] static const OpCodeInfo& get( Code code ) noexcept;

	// === Basic Properties ===

	/// @brief Gets the instruction code.
	[[nodiscard]] constexpr Code code() const noexcept { return code_; }

	/// @brief Gets the mnemonic.
	[[nodiscard]] Mnemonic mnemonic() const noexcept;

	/// @brief Gets the encoding (Legacy, VEX, EVEX, XOP, 3DNow!, MVEX).
	[[nodiscard]] constexpr EncodingKind encoding() const noexcept { return encoding_; }

	/// @brief Returns true if it's an instruction, false if it's eg. INVALID, db, dw, dd, dq, zero_bytes.
	[[nodiscard]] bool is_instruction() const noexcept;

	// === Mode Support ===

	/// @brief Returns true if the instruction is available in 16-bit mode.
	[[nodiscard]] constexpr bool mode16() const noexcept { return ( enc_flags3_ & 0x00010000U ) != 0; }

	/// @brief Returns true if the instruction is available in 32-bit mode.
	[[nodiscard]] constexpr bool mode32() const noexcept { return ( enc_flags3_ & 0x00010000U ) != 0; }

	/// @brief Returns true if the instruction is available in 64-bit mode.
	[[nodiscard]] constexpr bool mode64() const noexcept { return ( enc_flags3_ & 0x00020000U ) != 0; }

	// === Opcode Information ===

	/// @brief Gets the opcode byte(s). The low byte(s) of this value is the opcode.
	[[nodiscard]] constexpr uint16_t op_code() const noexcept { return op_code_; }

	/// @brief Gets the opcode table (Normal, 0F, 0F38, 0F3A, etc.).
	[[nodiscard]] constexpr OpCodeTableKind table() const noexcept { return table_; }

	/// @brief Gets the mandatory prefix (None, 66, F3, F2).
	[[nodiscard]] constexpr MandatoryPrefix mandatory_prefix() const noexcept { return mandatory_prefix_; }

	/// @brief Gets the group index (0-7) or -1 if not a group instruction.
	[[nodiscard]] constexpr int8_t group_index() const noexcept { return group_index_; }

	/// @brief Gets the RM group index (0-7) or -1 if not an RM group instruction.
	[[nodiscard]] constexpr int8_t rm_group_index() const noexcept { return rm_group_index_; }

	// === Size Properties ===

	/// @brief (Legacy encoding) Gets the required operand size (16,32,64) or 0.
	[[nodiscard]] constexpr uint32_t operand_size() const noexcept { return operand_size_; }

	/// @brief (Legacy encoding) Gets the required address size (16,32,64) or 0.
	[[nodiscard]] constexpr uint32_t address_size() const noexcept { return address_size_; }

	/// @brief (VEX/XOP/EVEX) Gets the L / L'L value or default value if is_lig() is true.
	[[nodiscard]] constexpr uint32_t l() const noexcept { return l_; }

	/// @brief (VEX/XOP/EVEX/MVEX) Gets the W value or default if is_wig() or is_wig32() is true.
	[[nodiscard]] constexpr uint32_t w() const noexcept { return ( flags_ & FLAG_W ) != 0 ? 1U : 0U; }

	/// @brief (EVEX/MVEX) Gets the tuple type.
	[[nodiscard]] constexpr TupleType tuple_type() const noexcept { return tuple_type_; }

	// === VEX/EVEX Properties ===

	/// @brief (VEX/XOP/EVEX) Returns true if the L / L'L fields are ignored.
	[[nodiscard]] constexpr bool is_lig() const noexcept { return ( flags_ & FLAG_LIG ) != 0; }

	/// @brief (VEX/XOP/EVEX/MVEX) Returns true if the W field is ignored in 16/32/64-bit modes.
	[[nodiscard]] constexpr bool is_wig() const noexcept { return ( flags_ & FLAG_WIG ) != 0; }

	/// @brief (VEX/XOP/EVEX/MVEX) Returns true if W is ignored in 16/32-bit modes (but not 64-bit).
	[[nodiscard]] constexpr bool is_wig32() const noexcept { return ( flags_ & FLAG_WIG32 ) != 0; }

	// === Operand Information ===

	/// @brief Gets the number of operands.
	[[nodiscard]] uint32_t op_count() const noexcept;

	/// @brief Gets the operand kind for operand 0.
	[[nodiscard]] constexpr OpCodeOperandKind op0_kind() const noexcept { return op_kinds_[0]; }

	/// @brief Gets the operand kind for operand 1.
	[[nodiscard]] constexpr OpCodeOperandKind op1_kind() const noexcept { return op_kinds_[1]; }

	/// @brief Gets the operand kind for operand 2.
	[[nodiscard]] constexpr OpCodeOperandKind op2_kind() const noexcept { return op_kinds_[2]; }

	/// @brief Gets the operand kind for operand 3.
	[[nodiscard]] constexpr OpCodeOperandKind op3_kind() const noexcept { return op_kinds_[3]; }

	/// @brief Gets the operand kind for operand 4.
	[[nodiscard]] constexpr OpCodeOperandKind op4_kind() const noexcept { return op_kinds_[4]; }

	/// @brief Gets the operand kind for the specified operand.
	/// @param operand Operand index (0-4)
	[[nodiscard]] OpCodeOperandKind op_kind( uint32_t operand ) const noexcept;

	// === Memory Size ===

	/// @brief Gets the non-broadcast memory size if it has a memory operand.
	[[nodiscard]] MemorySize memory_size() const noexcept;

	/// @brief Gets the broadcast memory size if it has a memory operand.
	[[nodiscard]] MemorySize broadcast_memory_size() const noexcept;

	// === Prefix Support ===

	/// @brief Returns true if FWAIT (9B) is added before the instruction.
	[[nodiscard]] constexpr bool fwait() const noexcept { return ( enc_flags3_ & 0x00008000U ) != 0; }

	/// @brief Returns true if the LOCK (F0) prefix can be used.
	[[nodiscard]] constexpr bool can_use_lock_prefix() const noexcept { return ( enc_flags3_ & 0x00040000U ) != 0; }

	/// @brief Returns true if the XACQUIRE (F2) prefix can be used.
	[[nodiscard]] constexpr bool can_use_xacquire_prefix() const noexcept { return ( enc_flags3_ & 0x00080000U ) != 0; }

	/// @brief Returns true if the XRELEASE (F3) prefix can be used.
	[[nodiscard]] constexpr bool can_use_xrelease_prefix() const noexcept { return ( enc_flags3_ & 0x00100000U ) != 0; }

	/// @brief Returns true if the REP / REPE (F3) prefixes can be used.
	[[nodiscard]] constexpr bool can_use_rep_prefix() const noexcept { return ( enc_flags3_ & 0x00200000U ) != 0; }

	/// @brief Returns true if the REPNE (F2) prefix can be used.
	[[nodiscard]] constexpr bool can_use_repne_prefix() const noexcept { return ( enc_flags3_ & 0x00400000U ) != 0; }

	/// @brief Returns true if the BND (F2) prefix can be used.
	[[nodiscard]] constexpr bool can_use_bnd_prefix() const noexcept { return ( enc_flags3_ & 0x00800000U ) != 0; }

	/// @brief Returns true if the HINT-TAKEN (3E) and HINT-NOT-TAKEN (2E) prefixes can be used.
	[[nodiscard]] constexpr bool can_use_hint_taken_prefix() const noexcept { return ( enc_flags3_ & 0x01000000U ) != 0; }

	/// @brief Returns true if the NOTRACK (3E) prefix can be used.
	[[nodiscard]] constexpr bool can_use_notrack_prefix() const noexcept { return ( enc_flags3_ & 0x02000000U ) != 0; }

	// === EVEX/MVEX Properties ===

	/// @brief (EVEX) Returns true if the instruction supports broadcasting (EVEX.b bit).
	[[nodiscard]] constexpr bool can_broadcast() const noexcept { return ( enc_flags3_ & 0x04000000U ) != 0; }

	/// @brief (EVEX/MVEX) Returns true if the instruction supports rounding control.
	[[nodiscard]] constexpr bool can_use_rounding_control() const noexcept { return ( enc_flags3_ & 0x08000000U ) != 0; }

	/// @brief (EVEX/MVEX) Returns true if the instruction supports suppress all exceptions.
	[[nodiscard]] constexpr bool can_suppress_all_exceptions() const noexcept { return ( enc_flags3_ & 0x10000000U ) != 0; }

	/// @brief (EVEX/MVEX) Returns true if an opmask register can be used.
	[[nodiscard]] constexpr bool can_use_op_mask_register() const noexcept { return ( enc_flags3_ & 0x20000000U ) != 0; }

	/// @brief (EVEX/MVEX) Returns true if a non-zero opmask register must be used.
	[[nodiscard]] constexpr bool require_op_mask_register() const noexcept { return ( enc_flags3_ & 0x80000000U ) != 0; }

	/// @brief (EVEX) Returns true if the instruction supports zeroing masking.
	[[nodiscard]] constexpr bool can_use_zeroing_masking() const noexcept { return ( enc_flags3_ & 0x40000000U ) != 0; }

	// === Rounding/Exception Properties ===

	/// @brief Returns true if rounding control is ignored (#UD is not generated).
	[[nodiscard]] constexpr bool ignores_rounding_control() const noexcept { return ( flags_ & FLAG_IGNORES_ROUNDING_CONTROL ) != 0; }

	/// @brief (AMD) Returns true if LOCK can be used as extra register bit (bit 3).
	[[nodiscard]] constexpr bool amd_lock_reg_bit() const noexcept { return ( flags_ & FLAG_AMD_LOCK_REG_BIT ) != 0; }

	// === Operand Size Properties ===

	/// @brief Returns true if default operand size is 64 in 64-bit mode. 66h switches to 16-bit.
	[[nodiscard]] constexpr bool default_op_size64() const noexcept { return ( enc_flags3_ & 0x00001000U ) != 0; }

	/// @brief Returns true if operand size is always 64 in 64-bit mode. 66h is ignored.
	[[nodiscard]] bool force_op_size64() const noexcept;

	/// @brief Returns true if Intel decoder forces 64-bit operand size. 66h is ignored.
	[[nodiscard]] constexpr bool intel_force_op_size64() const noexcept { return ( enc_flags3_ & 0x00004000U ) != 0; }

	// === CPL Properties ===

	/// @brief Returns true if can only be executed when CPL=0.
	[[nodiscard]] bool must_be_cpl0() const noexcept;

	/// @brief Returns true if can be executed when CPL=0.
	[[nodiscard]] constexpr bool cpl0() const noexcept { return ( flags_ & FLAG_CPL0 ) != 0; }

	/// @brief Returns true if can be executed when CPL=1.
	[[nodiscard]] constexpr bool cpl1() const noexcept { return ( flags_ & FLAG_CPL1 ) != 0; }

	/// @brief Returns true if can be executed when CPL=2.
	[[nodiscard]] constexpr bool cpl2() const noexcept { return ( flags_ & FLAG_CPL2 ) != 0; }

	/// @brief Returns true if can be executed when CPL=3.
	[[nodiscard]] constexpr bool cpl3() const noexcept { return ( flags_ & FLAG_CPL3 ) != 0; }

	// === Instruction Properties (from OpcFlags1) ===

	/// @brief Returns true if the instruction accesses I/O address space (IN, OUT, INS, OUTS).
	[[nodiscard]] bool is_input_output() const noexcept;

	/// @brief Returns true if it's a NOP instruction (not FPU NOPs like FNOP).
	[[nodiscard]] bool is_nop() const noexcept;

	/// @brief Returns true if it's a reserved NOP instruction (0F0D, 0F18-0F1F).
	[[nodiscard]] bool is_reserved_nop() const noexcept;

	/// @brief Returns true if it's a serializing instruction (Intel CPUs).
	[[nodiscard]] bool is_serializing_intel() const noexcept;

	/// @brief Returns true if it's a serializing instruction (AMD CPUs).
	[[nodiscard]] bool is_serializing_amd() const noexcept;

	/// @brief Returns true if may require CPL=0 depending on CPU option (CR4.TSD, CR4.PCE, etc.).
	[[nodiscard]] bool may_require_cpl0() const noexcept;

	/// @brief Returns true if it's a tracked JMP/CALL indirect instruction (CET).
	[[nodiscard]] bool is_cet_tracked() const noexcept;

	/// @brief Returns true if it's a non-temporal hint memory access (MOVNTDQ, etc.).
	[[nodiscard]] bool is_non_temporal() const noexcept;

	/// @brief Returns true if it's a no-wait FPU instruction (FNINIT, etc.).
	[[nodiscard]] bool is_fpu_no_wait() const noexcept;

	/// @brief Returns true if mod bits are ignored and assumed modrm[7:6] == 11b.
	[[nodiscard]] bool ignores_mod_bits() const noexcept;

	/// @brief Returns true if the 66h prefix is not allowed (causes #UD).
	[[nodiscard]] bool no66() const noexcept;

	/// @brief Returns true if F2/F3 prefixes aren't allowed.
	[[nodiscard]] bool nfx() const noexcept;

	/// @brief Returns true if register operands must have unique reg-nums.
	[[nodiscard]] bool requires_unique_reg_nums() const noexcept;

	/// @brief Returns true if destination register's reg-num must be unique.
	[[nodiscard]] bool requires_unique_dest_reg_num() const noexcept;

	/// @brief Returns true if it's a privileged instruction.
	[[nodiscard]] bool is_privileged() const noexcept;

	/// @brief Returns true if it reads/writes too many registers (PUSHA, POPA, etc.).
	[[nodiscard]] bool is_save_restore() const noexcept;

	/// @brief Returns true if it's an instruction that uses the stack (CALL, POP, etc.).
	[[nodiscard]] bool is_stack_instruction() const noexcept;

	/// @brief Returns true if the instruction ignores the segment register for memory ops.
	[[nodiscard]] bool ignores_segment() const noexcept;

	/// @brief Returns true if the opmask register is read and written.
	[[nodiscard]] bool is_op_mask_read_write() const noexcept;

	// === Mode Properties (from OpcFlags2) ===

	/// @brief Returns true if can be executed in real mode.
	[[nodiscard]] bool real_mode() const noexcept;

	/// @brief Returns true if can be executed in protected mode.
	[[nodiscard]] bool protected_mode() const noexcept;

	/// @brief Returns true if can be executed in virtual 8086 mode.
	[[nodiscard]] bool virtual8086_mode() const noexcept;

	/// @brief Returns true if can be executed in compatibility mode.
	[[nodiscard]] bool compatibility_mode() const noexcept;

	/// @brief Returns true if can be executed in 64-bit mode (long mode).
	[[nodiscard]] constexpr bool long_mode() const noexcept { return ( enc_flags3_ & 0x00020000U ) != 0; }

	/// @brief Returns true if can be used outside SMM.
	[[nodiscard]] bool use_outside_smm() const noexcept;

	/// @brief Returns true if can be used in SMM.
	[[nodiscard]] bool use_in_smm() const noexcept;

	/// @brief Returns true if can be used outside an SGX enclave.
	[[nodiscard]] bool use_outside_enclave_sgx() const noexcept;

	/// @brief Returns true if can be used inside an SGX1 enclave.
	[[nodiscard]] bool use_in_enclave_sgx1() const noexcept;

	/// @brief Returns true if can be used inside an SGX2 enclave.
	[[nodiscard]] bool use_in_enclave_sgx2() const noexcept;

	/// @brief Returns true if can be used outside VMX operation.
	[[nodiscard]] bool use_outside_vmx_op() const noexcept;

	/// @brief Returns true if can be used in VMX root operation.
	[[nodiscard]] bool use_in_vmx_root_op() const noexcept;

	/// @brief Returns true if can be used in VMX non-root operation.
	[[nodiscard]] bool use_in_vmx_non_root_op() const noexcept;

	/// @brief Returns true if can be used outside SEAM.
	[[nodiscard]] bool use_outside_seam() const noexcept;

	/// @brief Returns true if can be used in SEAM.
	[[nodiscard]] bool use_in_seam() const noexcept;

	/// @brief Returns true if TDX non-root generates #UD.
	[[nodiscard]] bool tdx_non_root_gen_ud() const noexcept;

	/// @brief Returns true if TDX non-root generates #VE.
	[[nodiscard]] bool tdx_non_root_gen_ve() const noexcept;

	/// @brief Returns true if TDX non-root may generate exception.
	[[nodiscard]] bool tdx_non_root_may_gen_ex() const noexcept;

	/// @brief Returns true if instruction causes Intel VM exit.
	[[nodiscard]] bool intel_vm_exit() const noexcept;

	/// @brief Returns true if instruction may cause Intel VM exit.
	[[nodiscard]] bool intel_may_vm_exit() const noexcept;

	/// @brief Returns true if instruction causes Intel SMM VM exit.
	[[nodiscard]] bool intel_smm_vm_exit() const noexcept;

	/// @brief Returns true if instruction causes AMD VM exit.
	[[nodiscard]] bool amd_vm_exit() const noexcept;

	/// @brief Returns true if instruction may cause AMD VM exit.
	[[nodiscard]] bool amd_may_vm_exit() const noexcept;

	/// @brief Returns true if instruction aborts TSX transaction.
	[[nodiscard]] bool tsx_abort() const noexcept;

	/// @brief Returns true if instruction may implicitly abort TSX transaction.
	[[nodiscard]] bool tsx_impl_abort() const noexcept;

	/// @brief Returns true if instruction may abort TSX transaction.
	[[nodiscard]] bool tsx_may_abort() const noexcept;

	/// @brief Returns true if Intel 16/32-bit decoder can decode this instruction.
	[[nodiscard]] bool intel_decoder16() const noexcept;

	/// @brief Returns true if Intel 16/32-bit decoder can decode this instruction.
	[[nodiscard]] bool intel_decoder32() const noexcept;

	/// @brief Returns true if Intel 64-bit decoder can decode this instruction.
	[[nodiscard]] bool intel_decoder64() const noexcept;

	/// @brief Returns true if AMD 16/32-bit decoder can decode this instruction.
	[[nodiscard]] bool amd_decoder16() const noexcept;

	/// @brief Returns true if AMD 16/32-bit decoder can decode this instruction.
	[[nodiscard]] bool amd_decoder32() const noexcept;

	/// @brief Returns true if AMD 64-bit decoder can decode this instruction.
	[[nodiscard]] bool amd_decoder64() const noexcept;

	/// @brief Gets the decoder option needed to decode this instruction, or NONE.
	[[nodiscard]] DecoderOptions::Value decoder_option() const noexcept;

	// === String Representations ===

	/// @brief Gets the opcode string (e.g., "VEX.128.66.0F38.W0 00 /r").
	[[nodiscard]] std::string to_op_code_string() const;

	/// @brief Gets the instruction string (e.g., "VPSHUFB xmm1, xmm2, xmm3/m128").
	[[nodiscard]] std::string to_instruction_string() const;

private:
	friend class OpCodeInfoFactory;

	// Internal flags
	static constexpr uint16_t FLAG_IGNORES_ROUNDING_CONTROL = 0x0001;
	static constexpr uint16_t FLAG_AMD_LOCK_REG_BIT = 0x0002;
	static constexpr uint16_t FLAG_LIG = 0x0004;
	static constexpr uint16_t FLAG_W = 0x0008;
	static constexpr uint16_t FLAG_WIG = 0x0010;
	static constexpr uint16_t FLAG_WIG32 = 0x0020;
	static constexpr uint16_t FLAG_CPL0 = 0x0040;
	static constexpr uint16_t FLAG_CPL1 = 0x0080;
	static constexpr uint16_t FLAG_CPL2 = 0x0100;
	static constexpr uint16_t FLAG_CPL3 = 0x0200;

	Code code_ = Code::INVALID;
	EncodingKind encoding_ = EncodingKind::LEGACY;
	MandatoryPrefix mandatory_prefix_ = MandatoryPrefix::NONE;
	OpCodeTableKind table_ = OpCodeTableKind::NORMAL;
	TupleType tuple_type_ = TupleType::N1;
	uint16_t op_code_ = 0;
	uint16_t flags_ = 0;
	uint8_t operand_size_ = 0;
	uint8_t address_size_ = 0;
	uint8_t l_ = 0;
	int8_t group_index_ = -1;
	int8_t rm_group_index_ = -1;
	OpCodeOperandKind op_kinds_[5] = {};

	// Raw flags from tables (for properties not decoded in constructor)
	uint32_t enc_flags3_ = 0;
	uint32_t opc_flags1_ = 0;
	uint32_t opc_flags2_ = 0;
};

/// @brief Extension methods for Code to get OpCodeInfo.
namespace CodeExtensions {
	/// @brief Gets the OpCodeInfo for this Code.
	/// @param code The instruction code
	/// @return Reference to the OpCodeInfo
	[[nodiscard]] inline const OpCodeInfo& op_code( Code code ) noexcept {
		return OpCodeInfo::get( code );
	}
}

} // namespace iced_x86

#endif // !ICED_X86_NO_OP_CODE_INFO

#endif // ICED_X86_OP_CODE_INFO_HPP
