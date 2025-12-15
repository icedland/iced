// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/op_code_info.hpp"
#include "iced_x86/register.hpp"
#include "iced_x86/memory_size.hpp"
#include "iced_x86/memory_size_info.hpp"
#include "iced_x86/internal/encoder_data.hpp"
#include "iced_x86/internal/encoder_EncFlags1.hpp"
#include "iced_x86/internal/encoder_EncFlags2.hpp"
#include "iced_x86/internal/encoder_EncFlags3.hpp"
#include "iced_x86/internal/op_code_info_flags.hpp"
#include "iced_x86/internal/tables.hpp"
#include "iced_x86/internal/encoder_op_kind_tables.hpp"
#include "iced_x86/internal/formatter_mnemonics.hpp"
#include "iced_x86/iced_constants.hpp"
#include "iced_x86/decoder_options.hpp"

#include <array>
#include <format>
#include <string>

namespace iced_x86 {

// OPC_FLAGS1 and OPC_FLAGS2 are now in encoder_data.hpp (generated)

namespace {

// Decoder options lookup table (maps DecOptionValue to DecoderOptions)
constexpr std::array<DecoderOptions::Value, 18> g_to_decoder_options = {{
	DecoderOptions::NONE,
	DecoderOptions::ALTINST,
	DecoderOptions::CL1INVMB,
	DecoderOptions::CMPXCHG486A,
	DecoderOptions::CYRIX,
	DecoderOptions::CYRIX_DMI,
	DecoderOptions::CYRIX_SMINT_0F7E,
	DecoderOptions::JMPE,
	DecoderOptions::LOADALL286,
	DecoderOptions::LOADALL386,
	DecoderOptions::MOV_TR,
	DecoderOptions::MPX,
	DecoderOptions::OLD_FPU,
	DecoderOptions::PCOMMIT,
	DecoderOptions::UMOV,
	DecoderOptions::XBTS,
	DecoderOptions::UDBG,
	DecoderOptions::KNC
}};

} // anonymous namespace

// Static storage for all OpCodeInfo instances (lazy initialized)
// Named OpCodeInfoFactory to match the friend declaration in op_code_info.hpp
class OpCodeInfoFactory {
public:
	static const OpCodeInfo& get( Code code ) noexcept {
		static OpCodeInfoFactory storage;
		return storage.infos_[static_cast<std::size_t>( code )];
	}

private:
	OpCodeInfoFactory() {
		for ( std::size_t i = 0; i < IcedConstants::CODE_ENUM_COUNT; ++i ) {
			init_op_code_info( infos_[i], static_cast<Code>( i ) );
		}
	}

	static void init_op_code_info( OpCodeInfo& info, Code code ) noexcept;

	std::array<OpCodeInfo, IcedConstants::CODE_ENUM_COUNT> infos_;
};

void OpCodeInfoFactory::init_op_code_info( OpCodeInfo& info, Code code ) noexcept {
	using namespace internal;

	const std::size_t index = static_cast<std::size_t>( code );
	const uint32_t enc_flags1 = ENC_FLAGS1[index];
	const uint32_t enc_flags2 = ENC_FLAGS2[index];
	const uint32_t enc_flags3 = ENC_FLAGS3[index];
	const uint32_t opc_flags1 = OPC_FLAGS1[index];
	const uint32_t opc_flags2 = OPC_FLAGS2[index];

	info.code_ = code;
	info.enc_flags3_ = enc_flags3;
	info.opc_flags1_ = opc_flags1;
	info.opc_flags2_ = opc_flags2;

	// Extract opcode
	info.op_code_ = static_cast<uint16_t>( enc_flags2 & 0xFFFFU );

	// Extract encoding kind
	info.encoding_ = static_cast<iced_x86::EncodingKind>( ( enc_flags3 >> EncFlags3::ENCODING_SHIFT ) & EncFlags3::ENCODING_MASK );

	// Extract mandatory prefix
	const auto mpb = static_cast<MandatoryPrefixByte>( ( enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT ) & EncFlags2::MANDATORY_PREFIX_MASK );
	switch ( mpb ) {
	case MandatoryPrefixByte::NONE:
		info.mandatory_prefix_ = ( enc_flags2 & EncFlags2::HAS_MANDATORY_PREFIX ) != 0 ? MandatoryPrefix::PNP : MandatoryPrefix::NONE;
		break;
	case MandatoryPrefixByte::P66:
		info.mandatory_prefix_ = MandatoryPrefix::P66;
		break;
	case MandatoryPrefixByte::PF3:
		info.mandatory_prefix_ = MandatoryPrefix::PF3;
		break;
	case MandatoryPrefixByte::PF2:
		info.mandatory_prefix_ = MandatoryPrefix::PF2;
		break;
	}

	// Extract operand size
	const auto op_size_code = static_cast<uint8_t>( ( enc_flags3 >> EncFlags3::OPERAND_SIZE_SHIFT ) & EncFlags3::OPERAND_SIZE_MASK );
	switch ( op_size_code ) {
	case 0: info.operand_size_ = 0; break;
	case 1: info.operand_size_ = 16; break;
	case 2: info.operand_size_ = 32; break;
	case 3: info.operand_size_ = 64; break;
	}

	// Extract address size
	const auto addr_size_code = static_cast<uint8_t>( ( enc_flags3 >> EncFlags3::ADDRESS_SIZE_SHIFT ) & EncFlags3::ADDRESS_SIZE_MASK );
	switch ( addr_size_code ) {
	case 0: info.address_size_ = 0; break;
	case 1: info.address_size_ = 16; break;
	case 2: info.address_size_ = 32; break;
	case 3: info.address_size_ = 64; break;
	}

	// Extract group index
	info.group_index_ = ( enc_flags2 & EncFlags2::HAS_GROUP_INDEX ) != 0 
		? static_cast<int8_t>( ( enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT ) & 7 ) 
		: -1;

	// Extract RM group index
	info.rm_group_index_ = ( enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX ) != 0
		? static_cast<int8_t>( ( enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT ) & 7 )
		: -1;

	// Extract tuple type
	info.tuple_type_ = static_cast<TupleType>( ( enc_flags3 >> EncFlags3::TUPLE_TYPE_SHIFT ) & EncFlags3::TUPLE_TYPE_MASK );

	// Extract L bit
	const auto lbit = static_cast<LBit>( ( enc_flags2 >> EncFlags2::LBIT_SHIFT ) & EncFlags2::LBIT_MASK );
	switch ( lbit ) {
	case LBit::LZ:
	case LBit::L0:
		info.l_ = 0;
		break;
	case LBit::L1:
		info.l_ = 1;
		break;
	case LBit::L128:
		info.l_ = 0;
		break;
	case LBit::L256:
		info.l_ = 1;
		break;
	case LBit::L512:
		info.l_ = 2;
		break;
	case LBit::LIG:
		info.l_ = 0;
		info.flags_ |= OpCodeInfo::FLAG_LIG;
		break;
	}

	// Extract W bit
	const auto wbit = static_cast<WBit>( ( enc_flags2 >> EncFlags2::WBIT_SHIFT ) & EncFlags2::WBIT_MASK );
	switch ( wbit ) {
	case WBit::W0:
		break;
	case WBit::W1:
		info.flags_ |= OpCodeInfo::FLAG_W;
		break;
	case WBit::WIG:
		info.flags_ |= OpCodeInfo::FLAG_WIG;
		break;
	}

	// Extract CPL flags
	const uint32_t cpl_flags = opc_flags1 & ( OpCodeInfoFlags1::CPL0_ONLY | OpCodeInfoFlags1::CPL3_ONLY );
	if ( cpl_flags == OpCodeInfoFlags1::CPL0_ONLY ) {
		info.flags_ |= OpCodeInfo::FLAG_CPL0;
	} else if ( cpl_flags == OpCodeInfoFlags1::CPL3_ONLY ) {
		info.flags_ |= OpCodeInfo::FLAG_CPL3;
	} else {
		info.flags_ |= OpCodeInfo::FLAG_CPL0 | OpCodeInfo::FLAG_CPL1 | OpCodeInfo::FLAG_CPL2 | OpCodeInfo::FLAG_CPL3;
	}

	// Extract ignores rounding control flag
	if ( ( enc_flags1 & EncFlags1::IGNORES_ROUNDING_CONTROL ) != 0 ) {
		info.flags_ |= OpCodeInfo::FLAG_IGNORES_ROUNDING_CONTROL;
	}

	// Extract AMD lock reg bit flag
	if ( ( enc_flags1 & EncFlags1::AMD_LOCK_REG_BIT ) != 0 ) {
		info.flags_ |= OpCodeInfo::FLAG_AMD_LOCK_REG_BIT;
	}

	// Extract table and operand kinds based on encoding
	switch ( info.encoding_ ) {
	case iced_x86::EncodingKind::LEGACY: {
		info.op_kinds_[0] = LEGACY_OP_KINDS[( enc_flags1 >> EncFlags1::LEGACY_OP0_SHIFT ) & EncFlags1::LEGACY_OP_MASK];
		info.op_kinds_[1] = LEGACY_OP_KINDS[( enc_flags1 >> EncFlags1::LEGACY_OP1_SHIFT ) & EncFlags1::LEGACY_OP_MASK];
		info.op_kinds_[2] = LEGACY_OP_KINDS[( enc_flags1 >> EncFlags1::LEGACY_OP2_SHIFT ) & EncFlags1::LEGACY_OP_MASK];
		info.op_kinds_[3] = LEGACY_OP_KINDS[( enc_flags1 >> EncFlags1::LEGACY_OP3_SHIFT ) & EncFlags1::LEGACY_OP_MASK];
		info.op_kinds_[4] = OpCodeOperandKind::NONE;
		
		const auto table = static_cast<LegacyOpCodeTable>( ( enc_flags2 >> EncFlags2::TABLE_SHIFT ) & EncFlags2::TABLE_MASK );
		switch ( table ) {
		case LegacyOpCodeTable::MAP0: info.table_ = OpCodeTableKind::NORMAL; break;
		case LegacyOpCodeTable::MAP0F: info.table_ = OpCodeTableKind::T0_F; break;
		case LegacyOpCodeTable::MAP0F38: info.table_ = OpCodeTableKind::T0_F38; break;
		case LegacyOpCodeTable::MAP0F3A: info.table_ = OpCodeTableKind::T0_F3_A; break;
		}
		break;
	}
	case iced_x86::EncodingKind::VEX: {
		info.op_kinds_[0] = VEX_OP_KINDS[( enc_flags1 >> EncFlags1::VEX_OP0_SHIFT ) & EncFlags1::VEX_OP_MASK];
		info.op_kinds_[1] = VEX_OP_KINDS[( enc_flags1 >> EncFlags1::VEX_OP1_SHIFT ) & EncFlags1::VEX_OP_MASK];
		info.op_kinds_[2] = VEX_OP_KINDS[( enc_flags1 >> EncFlags1::VEX_OP2_SHIFT ) & EncFlags1::VEX_OP_MASK];
		info.op_kinds_[3] = VEX_OP_KINDS[( enc_flags1 >> EncFlags1::VEX_OP3_SHIFT ) & EncFlags1::VEX_OP_MASK];
		info.op_kinds_[4] = VEX_OP_KINDS[( enc_flags1 >> EncFlags1::VEX_OP4_SHIFT ) & EncFlags1::VEX_OP_MASK];
		
		const auto table = static_cast<VexOpCodeTable>( ( enc_flags2 >> EncFlags2::TABLE_SHIFT ) & EncFlags2::TABLE_MASK );
		switch ( table ) {
		case VexOpCodeTable::MAP0: info.table_ = OpCodeTableKind::NORMAL; break;
		case VexOpCodeTable::MAP0F: info.table_ = OpCodeTableKind::T0_F; break;
		case VexOpCodeTable::MAP0F38: info.table_ = OpCodeTableKind::T0_F38; break;
		case VexOpCodeTable::MAP0F3A: info.table_ = OpCodeTableKind::T0_F3_A; break;
		}
		break;
	}
	case iced_x86::EncodingKind::EVEX: {
		info.op_kinds_[0] = EVEX_OP_KINDS[( enc_flags1 >> EncFlags1::EVEX_OP0_SHIFT ) & EncFlags1::EVEX_OP_MASK];
		info.op_kinds_[1] = EVEX_OP_KINDS[( enc_flags1 >> EncFlags1::EVEX_OP1_SHIFT ) & EncFlags1::EVEX_OP_MASK];
		info.op_kinds_[2] = EVEX_OP_KINDS[( enc_flags1 >> EncFlags1::EVEX_OP2_SHIFT ) & EncFlags1::EVEX_OP_MASK];
		info.op_kinds_[3] = EVEX_OP_KINDS[( enc_flags1 >> EncFlags1::EVEX_OP3_SHIFT ) & EncFlags1::EVEX_OP_MASK];
		info.op_kinds_[4] = OpCodeOperandKind::NONE;
		
		const auto table = static_cast<EvexOpCodeTable>( ( enc_flags2 >> EncFlags2::TABLE_SHIFT ) & EncFlags2::TABLE_MASK );
		switch ( table ) {
		case EvexOpCodeTable::MAP0F: info.table_ = OpCodeTableKind::T0_F; break;
		case EvexOpCodeTable::MAP0F38: info.table_ = OpCodeTableKind::T0_F38; break;
		case EvexOpCodeTable::MAP0F3A: info.table_ = OpCodeTableKind::T0_F3_A; break;
		case EvexOpCodeTable::MAP5: info.table_ = OpCodeTableKind::MAP5; break;
		case EvexOpCodeTable::MAP6: info.table_ = OpCodeTableKind::MAP6; break;
		}
		break;
	}
	case iced_x86::EncodingKind::XOP: {
		info.op_kinds_[0] = XOP_OP_KINDS[( enc_flags1 >> EncFlags1::XOP_OP0_SHIFT ) & EncFlags1::XOP_OP_MASK];
		info.op_kinds_[1] = XOP_OP_KINDS[( enc_flags1 >> EncFlags1::XOP_OP1_SHIFT ) & EncFlags1::XOP_OP_MASK];
		info.op_kinds_[2] = XOP_OP_KINDS[( enc_flags1 >> EncFlags1::XOP_OP2_SHIFT ) & EncFlags1::XOP_OP_MASK];
		info.op_kinds_[3] = XOP_OP_KINDS[( enc_flags1 >> EncFlags1::XOP_OP3_SHIFT ) & EncFlags1::XOP_OP_MASK];
		info.op_kinds_[4] = OpCodeOperandKind::NONE;
		
		const auto table = static_cast<XopOpCodeTable>( ( enc_flags2 >> EncFlags2::TABLE_SHIFT ) & EncFlags2::TABLE_MASK );
		switch ( table ) {
		case XopOpCodeTable::MAP8: info.table_ = OpCodeTableKind::MAP8; break;
		case XopOpCodeTable::MAP9: info.table_ = OpCodeTableKind::MAP9; break;
		case XopOpCodeTable::MAP10: info.table_ = OpCodeTableKind::MAP10; break;
		}
		break;
	}
	case iced_x86::EncodingKind::D3NOW:
		info.op_kinds_[0] = OpCodeOperandKind::MM_REG;
		info.op_kinds_[1] = OpCodeOperandKind::MM_OR_MEM;
		info.op_kinds_[2] = OpCodeOperandKind::NONE;
		info.op_kinds_[3] = OpCodeOperandKind::NONE;
		info.op_kinds_[4] = OpCodeOperandKind::NONE;
		info.table_ = OpCodeTableKind::T0_F;
		break;
	case iced_x86::EncodingKind::MVEX: {
		info.op_kinds_[0] = MVEX_OP_KINDS[( enc_flags1 >> EncFlags1::MVEX_OP0_SHIFT ) & EncFlags1::MVEX_OP_MASK];
		info.op_kinds_[1] = MVEX_OP_KINDS[( enc_flags1 >> EncFlags1::MVEX_OP1_SHIFT ) & EncFlags1::MVEX_OP_MASK];
		info.op_kinds_[2] = MVEX_OP_KINDS[( enc_flags1 >> EncFlags1::MVEX_OP2_SHIFT ) & EncFlags1::MVEX_OP_MASK];
		info.op_kinds_[3] = MVEX_OP_KINDS[( enc_flags1 >> EncFlags1::MVEX_OP3_SHIFT ) & EncFlags1::MVEX_OP_MASK];
		info.op_kinds_[4] = OpCodeOperandKind::NONE;
		
		// Note: MVEX table is similar to EVEX in some ways
		info.table_ = OpCodeTableKind::T0_F; // Default, needs refinement
		break;
	}
	}
}

const OpCodeInfo& OpCodeInfo::get( Code code ) noexcept {
	return OpCodeInfoFactory::get( code );
}

Mnemonic OpCodeInfo::mnemonic() const noexcept {
	return internal::g_code_to_mnemonic[static_cast<std::size_t>( code_ )];
}

bool OpCodeInfo::is_instruction() const noexcept {
	return !( code_ <= Code::DECLARE_QWORD || code_ == Code::ZERO_BYTES );
}

uint32_t OpCodeInfo::op_count() const noexcept {
	return internal::g_instruction_op_counts[static_cast<std::size_t>( code_ )];
}

OpCodeOperandKind OpCodeInfo::op_kind( uint32_t operand ) const noexcept {
	if ( operand < 5 ) {
		return op_kinds_[operand];
	}
	return OpCodeOperandKind::NONE;
}

MemorySize OpCodeInfo::memory_size() const noexcept {
	return internal::g_instruction_memory_sizes[static_cast<std::size_t>( code_ )];
}

MemorySize OpCodeInfo::broadcast_memory_size() const noexcept {
	// If the instruction doesn't support broadcast, return UNKNOWN
	if ( !can_broadcast() ) {
		return MemorySize::UNKNOWN;
	}

	// Get the non-broadcast memory size and extract its element type
	// The broadcast element size is the element type of the packed memory operand
	MemorySize mem_size = memory_size();
	if ( mem_size == MemorySize::UNKNOWN ) {
		return MemorySize::UNKNOWN;
	}

	// Use the memory size info to get the element type
	const auto& info = memory_size_ext::get_info( mem_size );
	return info.element_type;
}

bool OpCodeInfo::force_op_size64() const noexcept {
	return ( opc_flags1_ & internal::OpCodeInfoFlags1::FORCE_OP_SIZE64 ) != 0;
}

bool OpCodeInfo::must_be_cpl0() const noexcept {
	constexpr uint16_t all_cpl = FLAG_CPL0 | FLAG_CPL1 | FLAG_CPL2 | FLAG_CPL3;
	return ( flags_ & all_cpl ) == FLAG_CPL0;
}

// OpcFlags1 properties
bool OpCodeInfo::is_input_output() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::INPUT_OUTPUT ) != 0; }
bool OpCodeInfo::is_nop() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::NOP ) != 0; }
bool OpCodeInfo::is_reserved_nop() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::RESERVED_NOP ) != 0; }
bool OpCodeInfo::is_serializing_intel() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::SERIALIZING_INTEL ) != 0; }
bool OpCodeInfo::is_serializing_amd() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::SERIALIZING_AMD ) != 0; }
bool OpCodeInfo::may_require_cpl0() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::MAY_REQUIRE_CPL0 ) != 0; }
bool OpCodeInfo::is_cet_tracked() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::CET_TRACKED ) != 0; }
bool OpCodeInfo::is_non_temporal() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::NON_TEMPORAL ) != 0; }
bool OpCodeInfo::is_fpu_no_wait() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::FPU_NO_WAIT ) != 0; }
bool OpCodeInfo::ignores_mod_bits() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::IGNORES_MOD_BITS ) != 0; }
bool OpCodeInfo::no66() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::NO66 ) != 0; }
bool OpCodeInfo::nfx() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::NFX ) != 0; }
bool OpCodeInfo::requires_unique_reg_nums() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::REQUIRES_UNIQUE_REG_NUMS ) != 0; }
bool OpCodeInfo::requires_unique_dest_reg_num() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::REQUIRES_UNIQUE_DEST_REG_NUM ) != 0; }
bool OpCodeInfo::is_privileged() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::PRIVILEGED ) != 0; }
bool OpCodeInfo::is_save_restore() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::SAVE_RESTORE ) != 0; }
bool OpCodeInfo::is_stack_instruction() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::STACK_INSTRUCTION ) != 0; }
bool OpCodeInfo::ignores_segment() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::IGNORES_SEGMENT ) != 0; }
bool OpCodeInfo::is_op_mask_read_write() const noexcept { return ( opc_flags1_ & internal::OpCodeInfoFlags1::OP_MASK_READ_WRITE ) != 0; }

// OpcFlags2 properties
bool OpCodeInfo::real_mode() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::REAL_MODE ) != 0; }
bool OpCodeInfo::protected_mode() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::PROTECTED_MODE ) != 0; }
bool OpCodeInfo::virtual8086_mode() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::VIRTUAL8086_MODE ) != 0; }
bool OpCodeInfo::compatibility_mode() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::COMPATIBILITY_MODE ) != 0; }
bool OpCodeInfo::use_outside_smm() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_OUTSIDE_SMM ) != 0; }
bool OpCodeInfo::use_in_smm() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_IN_SMM ) != 0; }
bool OpCodeInfo::use_outside_enclave_sgx() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_OUTSIDE_ENCLAVE_SGX ) != 0; }
bool OpCodeInfo::use_in_enclave_sgx1() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_IN_ENCLAVE_SGX1 ) != 0; }
bool OpCodeInfo::use_in_enclave_sgx2() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_IN_ENCLAVE_SGX2 ) != 0; }
bool OpCodeInfo::use_outside_vmx_op() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_OUTSIDE_VMX_OP ) != 0; }
bool OpCodeInfo::use_in_vmx_root_op() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_IN_VMX_ROOT_OP ) != 0; }
bool OpCodeInfo::use_in_vmx_non_root_op() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_IN_VMX_NON_ROOT_OP ) != 0; }
bool OpCodeInfo::use_outside_seam() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_OUTSIDE_SEAM ) != 0; }
bool OpCodeInfo::use_in_seam() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::USE_IN_SEAM ) != 0; }
bool OpCodeInfo::tdx_non_root_gen_ud() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::TDX_NON_ROOT_GEN_UD ) != 0; }
bool OpCodeInfo::tdx_non_root_gen_ve() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::TDX_NON_ROOT_GEN_VE ) != 0; }
bool OpCodeInfo::tdx_non_root_may_gen_ex() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::TDX_NON_ROOT_MAY_GEN_EX ) != 0; }
bool OpCodeInfo::intel_vm_exit() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::INTEL_VM_EXIT ) != 0; }
bool OpCodeInfo::intel_may_vm_exit() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::INTEL_MAY_VM_EXIT ) != 0; }
bool OpCodeInfo::intel_smm_vm_exit() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::INTEL_SMM_VM_EXIT ) != 0; }
bool OpCodeInfo::amd_vm_exit() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::AMD_VM_EXIT ) != 0; }
bool OpCodeInfo::amd_may_vm_exit() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::AMD_MAY_VM_EXIT ) != 0; }
bool OpCodeInfo::tsx_abort() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::TSX_ABORT ) != 0; }
bool OpCodeInfo::tsx_impl_abort() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::TSX_IMPL_ABORT ) != 0; }
bool OpCodeInfo::tsx_may_abort() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::TSX_MAY_ABORT ) != 0; }
bool OpCodeInfo::intel_decoder16() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::INTEL_DECODER16OR32 ) != 0; }
bool OpCodeInfo::intel_decoder32() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::INTEL_DECODER16OR32 ) != 0; }
bool OpCodeInfo::intel_decoder64() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::INTEL_DECODER64 ) != 0; }
bool OpCodeInfo::amd_decoder16() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::AMD_DECODER16OR32 ) != 0; }
bool OpCodeInfo::amd_decoder32() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::AMD_DECODER16OR32 ) != 0; }
bool OpCodeInfo::amd_decoder64() const noexcept { return ( opc_flags2_ & internal::OpCodeInfoFlags2::AMD_DECODER64 ) != 0; }

DecoderOptions::Value OpCodeInfo::decoder_option() const noexcept {
	const uint32_t dec_option_value = ( opc_flags1_ >> internal::OpCodeInfoFlags1::DEC_OPTION_VALUE_SHIFT ) & internal::OpCodeInfoFlags1::DEC_OPTION_VALUE_MASK;
	if ( dec_option_value < g_to_decoder_options.size() ) {
		return g_to_decoder_options[dec_option_value];
	}
	return DecoderOptions::NONE;
}

std::string OpCodeInfo::to_op_code_string() const {
	std::string result;

	// Helper lambda for mandatory prefix
	auto get_mandatory_prefix = []( MandatoryPrefix p ) -> const char* {
		switch ( p ) {
		case MandatoryPrefix::P66: return "66.";
		case MandatoryPrefix::PF3: return "F3.";
		case MandatoryPrefix::PF2: return "F2.";
		default: return "";
		}
	};

	// Helper lambda for table
	auto get_table = []( OpCodeTableKind t ) -> const char* {
		switch ( t ) {
		case OpCodeTableKind::T0_F: return "0F.";
		case OpCodeTableKind::T0_F38: return "0F38.";
		case OpCodeTableKind::T0_F3_A: return "0F3A.";
		case OpCodeTableKind::MAP5: return "MAP5.";
		case OpCodeTableKind::MAP6: return "MAP6.";
		case OpCodeTableKind::MAP8: return "MAP8.";
		case OpCodeTableKind::MAP9: return "MAP9.";
		case OpCodeTableKind::MAP10: return "MAP10.";
		default: return "";
		}
	};

	// Helper for vector length
	auto get_vex_l = [this]() -> const char* {
		if ( l_ == 0 ) return "128.";
		if ( l_ == 1 ) return "256.";
		return "";
	};

	auto get_evex_l = [this]() -> const char* {
		if ( l_ == 0 ) return "128.";
		if ( l_ == 1 ) return "256.";
		if ( l_ == 2 ) return "512.";
		return "";
	};

	const char* w_str = ( flags_ & FLAG_W ) != 0 ? "W1 " : "W0 ";

	switch ( encoding_ ) {
	case EncodingKind::LEGACY: {
		// Legacy encoding: mandatory prefix, table, opcode
		switch ( mandatory_prefix_ ) {
		case MandatoryPrefix::P66: result += "66 "; break;
		case MandatoryPrefix::PF3: result += "F3 "; break;
		case MandatoryPrefix::PF2: result += "F2 "; break;
		default: break;
		}
		switch ( table_ ) {
		case OpCodeTableKind::T0_F: result += "0F "; break;
		case OpCodeTableKind::T0_F38: result += "0F 38 "; break;
		case OpCodeTableKind::T0_F3_A: result += "0F 3A "; break;
		default: break;
		}
		break;
	}

	case EncodingKind::VEX:
		result += "VEX.";
		result += get_vex_l();
		if ( l_ > 1 ) result += std::format( "L{}.", static_cast<int>( l_ ) );
		result += get_mandatory_prefix( mandatory_prefix_ );
		result += get_table( table_ );
		result += w_str;
		break;

	case EncodingKind::EVEX:
		result += "EVEX.";
		result += get_evex_l();
		if ( l_ > 2 ) result += std::format( "L{}.", static_cast<int>( l_ ) );
		result += get_mandatory_prefix( mandatory_prefix_ );
		result += get_table( table_ );
		result += w_str;
		break;

	case EncodingKind::XOP:
		result += "XOP.";
		result += get_vex_l();
		result += get_table( table_ );
		result += w_str;
		break;

	case EncodingKind::D3NOW:
		result += "0F 0F ";
		break;

	case EncodingKind::MVEX:
		result += "MVEX.";
		result += get_evex_l();
		result += get_mandatory_prefix( mandatory_prefix_ );
		result += get_table( table_ );
		result += w_str;
		break;
	}

	// Opcode byte(s)
	result += std::format( "{:02X}", op_code_ );

	// ModR/M.reg extension (group index)
	if ( group_index_ >= 0 ) {
		result += std::format( " /{}", static_cast<int>( group_index_ ) );
	}

	// 3DNow! has opcode at end
	if ( encoding_ == EncodingKind::D3NOW ) {
		result += std::format( " {:02X}", op_code_ );
	}

	return result;
}

namespace {
// Helper to get string representation of OpCodeOperandKind
const char* op_kind_to_string( OpCodeOperandKind kind ) {
	switch ( kind ) {
	case OpCodeOperandKind::NONE: return nullptr;
	case OpCodeOperandKind::FARBR2_2: return "ptr16:16";
	case OpCodeOperandKind::FARBR4_2: return "ptr16:32";
	case OpCodeOperandKind::MEM_OFFS: return "moffs";
	case OpCodeOperandKind::MEM: return "m";
	case OpCodeOperandKind::MEM_MPX: return "m";
	case OpCodeOperandKind::MEM_MIB: return "mib";
	case OpCodeOperandKind::MEM_VSIB32X: return "vm32x";
	case OpCodeOperandKind::MEM_VSIB64X: return "vm64x";
	case OpCodeOperandKind::MEM_VSIB32Y: return "vm32y";
	case OpCodeOperandKind::MEM_VSIB64Y: return "vm64y";
	case OpCodeOperandKind::MEM_VSIB32Z: return "vm32z";
	case OpCodeOperandKind::MEM_VSIB64Z: return "vm64z";
	case OpCodeOperandKind::R8_OR_MEM: return "r/m8";
	case OpCodeOperandKind::R16_OR_MEM: return "r/m16";
	case OpCodeOperandKind::R32_OR_MEM: return "r/m32";
	case OpCodeOperandKind::R32_OR_MEM_MPX: return "r/m32";
	case OpCodeOperandKind::R64_OR_MEM: return "r/m64";
	case OpCodeOperandKind::R64_OR_MEM_MPX: return "r/m64";
	case OpCodeOperandKind::MM_OR_MEM: return "mm/m64";
	case OpCodeOperandKind::XMM_OR_MEM: return "xmm/m128";
	case OpCodeOperandKind::YMM_OR_MEM: return "ymm/m256";
	case OpCodeOperandKind::ZMM_OR_MEM: return "zmm/m512";
	case OpCodeOperandKind::BND_OR_MEM_MPX: return "bnd/m";
	case OpCodeOperandKind::K_OR_MEM: return "k/m";
	case OpCodeOperandKind::R8_REG: return "r8";
	case OpCodeOperandKind::R8_OPCODE: return "r8";
	case OpCodeOperandKind::R16_REG: return "r16";
	case OpCodeOperandKind::R16_REG_MEM: return "r16";
	case OpCodeOperandKind::R16_RM: return "r16";
	case OpCodeOperandKind::R16_OPCODE: return "r16";
	case OpCodeOperandKind::R32_REG: return "r32";
	case OpCodeOperandKind::R32_REG_MEM: return "r32";
	case OpCodeOperandKind::R32_RM: return "r32";
	case OpCodeOperandKind::R32_OPCODE: return "r32";
	case OpCodeOperandKind::R32_VVVV: return "r32";
	case OpCodeOperandKind::R64_REG: return "r64";
	case OpCodeOperandKind::R64_REG_MEM: return "r64";
	case OpCodeOperandKind::R64_RM: return "r64";
	case OpCodeOperandKind::R64_OPCODE: return "r64";
	case OpCodeOperandKind::R64_VVVV: return "r64";
	case OpCodeOperandKind::SEG_REG: return "Sreg";
	case OpCodeOperandKind::K_REG: return "k";
	case OpCodeOperandKind::KP1_REG: return "k+1";
	case OpCodeOperandKind::K_RM: return "k";
	case OpCodeOperandKind::K_VVVV: return "k";
	case OpCodeOperandKind::MM_REG: return "mm";
	case OpCodeOperandKind::MM_RM: return "mm";
	case OpCodeOperandKind::XMM_REG: return "xmm";
	case OpCodeOperandKind::XMM_RM: return "xmm";
	case OpCodeOperandKind::XMM_VVVV: return "xmm";
	case OpCodeOperandKind::XMMP3_VVVV: return "xmm+3";
	case OpCodeOperandKind::XMM_IS4: return "xmm";
	case OpCodeOperandKind::XMM_IS5: return "xmm";
	case OpCodeOperandKind::YMM_REG: return "ymm";
	case OpCodeOperandKind::YMM_RM: return "ymm";
	case OpCodeOperandKind::YMM_VVVV: return "ymm";
	case OpCodeOperandKind::YMM_IS4: return "ymm";
	case OpCodeOperandKind::YMM_IS5: return "ymm";
	case OpCodeOperandKind::ZMM_REG: return "zmm";
	case OpCodeOperandKind::ZMM_RM: return "zmm";
	case OpCodeOperandKind::ZMM_VVVV: return "zmm";
	case OpCodeOperandKind::ZMMP3_VVVV: return "zmm+3";
	case OpCodeOperandKind::CR_REG: return "cr";
	case OpCodeOperandKind::DR_REG: return "dr";
	case OpCodeOperandKind::TR_REG: return "tr";
	case OpCodeOperandKind::BND_REG: return "bnd";
	case OpCodeOperandKind::ES: return "es";
	case OpCodeOperandKind::CS: return "cs";
	case OpCodeOperandKind::SS: return "ss";
	case OpCodeOperandKind::DS: return "ds";
	case OpCodeOperandKind::FS: return "fs";
	case OpCodeOperandKind::GS: return "gs";
	case OpCodeOperandKind::AL: return "al";
	case OpCodeOperandKind::CL: return "cl";
	case OpCodeOperandKind::AX: return "ax";
	case OpCodeOperandKind::DX: return "dx";
	case OpCodeOperandKind::EAX: return "eax";
	case OpCodeOperandKind::RAX: return "rax";
	case OpCodeOperandKind::ST0: return "st(0)";
	case OpCodeOperandKind::STI_OPCODE: return "st(i)";
	case OpCodeOperandKind::IMM4_M2Z: return "imm4";
	case OpCodeOperandKind::IMM8: return "imm8";
	case OpCodeOperandKind::IMM8_CONST_1: return "1";
	case OpCodeOperandKind::IMM8SEX16: return "imm8";
	case OpCodeOperandKind::IMM8SEX32: return "imm8";
	case OpCodeOperandKind::IMM8SEX64: return "imm8";
	case OpCodeOperandKind::IMM16: return "imm16";
	case OpCodeOperandKind::IMM32: return "imm32";
	case OpCodeOperandKind::IMM32SEX64: return "imm32";
	case OpCodeOperandKind::IMM64: return "imm64";
	case OpCodeOperandKind::SEG_R_SI: return "[rSI]";
	case OpCodeOperandKind::ES_R_DI: return "es:[rDI]";
	case OpCodeOperandKind::SEG_R_DI: return "[rDI]";
	case OpCodeOperandKind::SEG_R_BX_AL: return "[rBX+AL]";
	case OpCodeOperandKind::BR16_1: return "rel8";
	case OpCodeOperandKind::BR32_1: return "rel8";
	case OpCodeOperandKind::BR64_1: return "rel8";
	case OpCodeOperandKind::BR16_2: return "rel16";
	case OpCodeOperandKind::BR32_4: return "rel32";
	case OpCodeOperandKind::BR64_4: return "rel32";
	case OpCodeOperandKind::XBEGIN_2: return "rel16";
	case OpCodeOperandKind::XBEGIN_4: return "rel32";
	case OpCodeOperandKind::BRDISP_2: return "disp16";
	case OpCodeOperandKind::BRDISP_4: return "disp32";
	case OpCodeOperandKind::SIBMEM: return "sibmem";
	case OpCodeOperandKind::TMM_REG: return "tmm";
	case OpCodeOperandKind::TMM_RM: return "tmm";
	case OpCodeOperandKind::TMM_VVVV: return "tmm";
	default: return "?";
	}
}
} // anonymous namespace

std::string OpCodeInfo::to_instruction_string() const {
	std::string result{ internal::get_mnemonic_string( mnemonic(), false ) };

	// Add operands
	bool first = true;
	for ( int i = 0; i < 5; ++i ) {
		OpCodeOperandKind kind = op_kinds_[i];
		if ( kind == OpCodeOperandKind::NONE ) {
			break;
		}
		const char* op_str = op_kind_to_string( kind );
		if ( op_str ) {
			if ( first ) {
				result += ' ';
				first = false;
			} else {
				result += ", ";
			}
			result += op_str;
		}
	}

	return result;
}

} // namespace iced_x86
