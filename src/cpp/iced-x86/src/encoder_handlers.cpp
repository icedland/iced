// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/encoder.hpp"
#include "iced_x86/instruction.hpp"
#include "iced_x86/internal/encoder_handler.hpp"
#include "iced_x86/internal/encoder_data.hpp"
#include "iced_x86/internal/encoder_ops_tables.hpp"
#include "iced_x86/internal/encoder_flags.hpp"
#include "iced_x86/internal/encoder_EncFlags1.hpp"
#include "iced_x86/internal/encoder_EncFlags2.hpp"
#include "iced_x86/internal/encoder_EncFlags3.hpp"
#include "iced_x86/internal/mvex_info.hpp"
#include "iced_x86/iced_constants.hpp"
#include "iced_x86/code.hpp"

#include <array>
#include <format>
#include <vector>
#include <memory>

namespace iced_x86::internal {

// InvalidHandler implementation
InvalidHandler::InvalidHandler() {
	base.encode = &InvalidHandler::encode;
	base.try_convert_to_disp8n = nullptr;
	base.operands = {};
	base.op_code = 0;
	base.group_index = -1;
	base.rm_group_index = -1;
	base.enc_flags3 = EncFlags3::NONE;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = false;
	base.is_special_instr = false;
}

void InvalidHandler::encode(const EncoderOpCodeHandler* /*handler*/, Encoder& encoder, const Instruction& /*instruction*/) {
	encoder.set_error_message(ERROR_MESSAGE);
}

// DeclareDataHandler implementation
DeclareDataHandler::DeclareDataHandler(Code code) {
	base.encode = &DeclareDataHandler::encode;
	base.try_convert_to_disp8n = nullptr;
	base.operands = {};
	base.op_code = 0;
	base.group_index = -1;
	base.rm_group_index = -1;
	base.enc_flags3 = EncFlags3::NONE;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = false;
	base.is_special_instr = true;
	
	switch (code) {
		case Code::DECLARE_BYTE:
			elem_size = 1;
			break;
		case Code::DECLARE_WORD:
			elem_size = 2;
			break;
		case Code::DECLARE_DWORD:
			elem_size = 4;
			break;
		case Code::DECLARE_QWORD:
			elem_size = 8;
			break;
		default:
			elem_size = 1;
			break;
	}
}

void DeclareDataHandler::encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction) {
	auto* self = reinterpret_cast<const DeclareDataHandler*>(handler);
	auto length = instruction.declare_data_len() * self->elem_size;
	for (std::size_t i = 0; i < length; ++i) {
		auto byte = instruction.get_declare_byte_value(static_cast<uint32_t>(i));
		encoder.write_byte_internal(byte);
	}
}

// ZeroBytesHandler implementation  
ZeroBytesHandler::ZeroBytesHandler(Code /*code*/) {
	base.encode = &ZeroBytesHandler::encode;
	base.try_convert_to_disp8n = nullptr;
	base.operands = {};
	base.op_code = 0;
	base.group_index = -1;
	base.rm_group_index = -1;
	base.enc_flags3 = EncFlags3::NONE;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = false;
	base.is_special_instr = true;
}

void ZeroBytesHandler::encode(const EncoderOpCodeHandler* /*handler*/, Encoder& /*encoder*/, const Instruction& /*instruction*/) {
	// Does nothing - zero byte instruction
}

// Storage for operand tables (these are used by handlers at runtime)
namespace {

// Helper to create a span from static operand arrays
template<std::size_t N>
constexpr std::span<const Op* const> make_op_span(const std::array<const Op*, N>& arr) {
	return std::span<const Op* const>(arr.data(), N);
}

// Empty operand span
inline constexpr std::span<const Op* const> EMPTY_OPS{};

} // anonymous namespace

// LegacyHandler implementation
LegacyHandler::LegacyHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3) {
	base.encode = &LegacyHandler::encode;
	base.try_convert_to_disp8n = nullptr;
	base.op_code = get_op_code(enc_flags2);
	base.group_index = (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.rm_group_index = (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.enc_flags3 = enc_flags3;
	base.op_size = static_cast<CodeSize>((enc_flags3 >> EncFlags3::OPERAND_SIZE_SHIFT) & EncFlags3::OPERAND_SIZE_MASK);
	base.addr_size = static_cast<CodeSize>((enc_flags3 >> EncFlags3::ADDRESS_SIZE_SHIFT) & EncFlags3::ADDRESS_SIZE_MASK);
	base.is_2byte_opcode = (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0;
	base.is_special_instr = false;
	
	// Determine table bytes
	auto table = static_cast<LegacyOpCodeTable>((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK);
	switch (table) {
		case LegacyOpCodeTable::MAP0:
			table_byte1 = 0;
			table_byte2 = 0;
			break;
		case LegacyOpCodeTable::MAP0F:
			table_byte1 = 0x0F;
			table_byte2 = 0;
			break;
		case LegacyOpCodeTable::MAP0F38:
			table_byte1 = 0x0F;
			table_byte2 = 0x38;
			break;
		case LegacyOpCodeTable::MAP0F3A:
			table_byte1 = 0x0F;
			table_byte2 = 0x3A;
			break;
	}
	
	// Determine mandatory prefix
	auto mpb = static_cast<MandatoryPrefixByte>((enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK);
	switch (mpb) {
		case MandatoryPrefixByte::NONE:
			mandatory_prefix = 0;
			break;
		case MandatoryPrefixByte::P66:
			mandatory_prefix = 0x66;
			break;
		case MandatoryPrefixByte::PF3:
			mandatory_prefix = 0xF3;
			break;
		case MandatoryPrefixByte::PF2:
			mandatory_prefix = 0xF2;
			break;
	}
	
	// Set operands from LEGACY_TABLE
	auto op0 = (enc_flags1 >> EncFlags1::LEGACY_OP0_SHIFT) & EncFlags1::LEGACY_OP_MASK;
	auto op1 = (enc_flags1 >> EncFlags1::LEGACY_OP1_SHIFT) & EncFlags1::LEGACY_OP_MASK;
	auto op2 = (enc_flags1 >> EncFlags1::LEGACY_OP2_SHIFT) & EncFlags1::LEGACY_OP_MASK;
	auto op3 = (enc_flags1 >> EncFlags1::LEGACY_OP3_SHIFT) & EncFlags1::LEGACY_OP_MASK;
	
	// Populate operand array from LEGACY_TABLE
	ops[0] = LEGACY_TABLE[op0];
	ops[1] = LEGACY_TABLE[op1];
	ops[2] = LEGACY_TABLE[op2];
	ops[3] = LEGACY_TABLE[op3];
	
	// Count non-none operands for span
	std::size_t op_count = 0;
	if (op0 != 0) op_count = 1;
	if (op1 != 0) op_count = 2;
	if (op2 != 0) op_count = 3;
	if (op3 != 0) op_count = 4;
	
	base.operands = std::span<const Op* const>(ops.data(), op_count);
}

void LegacyHandler::encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction) {
	auto* self = reinterpret_cast<const LegacyHandler*>(handler);
	
	// Write mandatory prefix (if any)
	auto b = self->mandatory_prefix;
	encoder.write_prefixes(instruction, b != 0xF3);
	if (b != 0) {
		encoder.write_byte_internal(b);
	}
	
	// Write REX prefix if needed
	b = encoder.encoder_flags();
	b &= 0x4F;  // B, X, R, W, REX flags
	if (b != 0) {
		if ((encoder.encoder_flags() & EncoderFlags::HIGH_LEGACY_8_BIT_REGS) != 0) {
			encoder.set_error_message(
				"Registers AH, CH, DH, BH can't be used if there's a REX prefix. Use AL, CL, DL, BL, SPL, BPL, SIL, DIL, R8L-R15L instead.");
		}
		b |= 0x40;  // REX prefix
		encoder.write_byte_internal(b);
	}
	
	// Write escape bytes
	b = self->table_byte1;
	if (b != 0) {
		encoder.write_byte_internal(b);
		b = self->table_byte2;
		if (b != 0) {
			encoder.write_byte_internal(b);
		}
	}
}

// VexHandler implementation
VexHandler::VexHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3) {
	base.encode = &VexHandler::encode;
	base.try_convert_to_disp8n = nullptr;
	base.op_code = get_op_code(enc_flags2);
	base.group_index = (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.rm_group_index = (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.enc_flags3 = enc_flags3;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0;
	base.is_special_instr = false;
	
	table = (enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK;
	
	auto wbit = static_cast<WBit>((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK);
	w1 = (wbit == WBit::W1) ? UINT32_MAX : 0;
	
	auto lbit = static_cast<LBit>((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK);
	last_byte = (lbit == LBit::L1 || lbit == LBit::L256) ? 4 : 0;
	if (w1 != 0) {
		last_byte |= 0x80;
	}
	last_byte |= (enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK;
	
	mask_w_l = (wbit == WBit::WIG) ? 0x80 : 0;
	mask_l = (lbit == LBit::LIG) ? 4 : 0;
	if (lbit == LBit::LIG) {
		mask_w_l |= 4;
	}
	
	// Set operands from VEX_TABLE
	auto op0 = (enc_flags1 >> EncFlags1::VEX_OP0_SHIFT) & EncFlags1::VEX_OP_MASK;
	auto op1 = (enc_flags1 >> EncFlags1::VEX_OP1_SHIFT) & EncFlags1::VEX_OP_MASK;
	auto op2 = (enc_flags1 >> EncFlags1::VEX_OP2_SHIFT) & EncFlags1::VEX_OP_MASK;
	auto op3 = (enc_flags1 >> EncFlags1::VEX_OP3_SHIFT) & EncFlags1::VEX_OP_MASK;
	
	ops[0] = VEX_TABLE[op0];
	ops[1] = VEX_TABLE[op1];
	ops[2] = VEX_TABLE[op2];
	ops[3] = VEX_TABLE[op3];
	
	std::size_t op_count = 0;
	if (op0 != 0) op_count = 1;
	if (op1 != 0) op_count = 2;
	if (op2 != 0) op_count = 3;
	if (op3 != 0) op_count = 4;
	
	base.operands = std::span<const Op* const>(ops.data(), op_count);
}

void VexHandler::encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction) {
	auto* self = reinterpret_cast<const VexHandler*>(handler);
	
	// Write segment prefix if needed
	auto seg = instruction.segment_prefix();
	if (seg != Register::NONE) {
		static constexpr uint8_t SEGMENT_OVERRIDES[] = {0x26, 0x2E, 0x36, 0x3E, 0x64, 0x65};
		auto seg_idx = static_cast<uint32_t>(seg) - static_cast<uint32_t>(Register::ES);
		if (seg_idx < 6) {
			encoder.write_byte_internal(SEGMENT_OVERRIDES[seg_idx]);
		}
	}
	
	// Write address size prefix if needed
	if ((encoder.encoder_flags() & EncoderFlags::P67) != 0) {
		encoder.write_byte_internal(0x67);
	}
	
	uint32_t b;
	auto encoder_flags = encoder.encoder_flags();
	
	// Determine if we can use 2-byte VEX or need 3-byte
	// Use 3-byte if:
	// - prevent_vex2 is set (user prefers 3-byte)
	// - W bit is needed
	// - X or B bits are needed (extended registers)
	// - Table is not 0F (need to encode table in 3-byte form)
	bool use_vex3 = (encoder.internal_prevent_vex2() | self->w1 | 
		(self->table - static_cast<uint32_t>(VexOpCodeTable::MAP0F)) |
		(encoder_flags & (EncoderFlags::X | EncoderFlags::B | EncoderFlags::W))) != 0;
	
	if (use_vex3) {
		// 3-byte VEX: C4 RXBmmmmm WvvvvLpp
		encoder.write_byte_internal(0xC4);
		
		b = self->table;
		b |= (~encoder_flags & 7) << 5;  // ~R, ~X, ~B in bits 7, 6, 5
		encoder.write_byte_internal(b);
		
		b = self->last_byte;
		b |= ((~encoder_flags >> EncoderFlags::VVVVV_SHIFT) & 0xF) << 3;  // ~vvvv in bits 6:3 (4 bits only)
		// Apply WIG/LIG from encoder settings (OR with mask_w_l bits that user wants to set)
		b |= self->mask_w_l & encoder.internal_vex_wig_lig();
		encoder.write_byte_internal(b);
	} else {
		// 2-byte VEX: C5 RvvvvLpp
		encoder.write_byte_internal(0xC5);
		
		b = self->last_byte;
		b |= ((~encoder_flags >> EncoderFlags::VVVVV_SHIFT) & 0xF) << 3;  // ~vvvv in bits 6:3 (4 bits only)
		b |= (~encoder_flags & EncoderFlags::R) << 5;  // ~R in bit 7 (R is bit 2, shift by 5 to reach bit 7)
		// Apply LIG from encoder settings
		b |= self->mask_l & encoder.internal_vex_wig_lig();
		encoder.write_byte_internal(b);
	}
}

// XopHandler implementation
XopHandler::XopHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3) {
	base.encode = &XopHandler::encode;
	base.try_convert_to_disp8n = nullptr;
	base.op_code = get_op_code(enc_flags2);
	base.group_index = (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.rm_group_index = (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.enc_flags3 = enc_flags3;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0;
	base.is_special_instr = false;
	
	table = 8 + ((enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK);
	
	auto wbit = static_cast<WBit>((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK);
	w1 = (wbit == WBit::W1) ? UINT32_MAX : 0;
	
	auto lbit = static_cast<LBit>((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK);
	last_byte = (lbit == LBit::L1 || lbit == LBit::L256) ? 4 : 0;
	if (w1 != 0) {
		last_byte |= 0x80;
	}
	last_byte |= (enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK;
	
	mask_w_l = (wbit == WBit::WIG) ? 0x80 : 0;
	mask_l = (lbit == LBit::LIG) ? 4 : 0;
	if (lbit == LBit::LIG) {
		mask_w_l |= 4;
	}
	
	// Set operands from XOP_TABLE
	auto op0 = (enc_flags1 >> EncFlags1::XOP_OP0_SHIFT) & EncFlags1::XOP_OP_MASK;
	auto op1 = (enc_flags1 >> EncFlags1::XOP_OP1_SHIFT) & EncFlags1::XOP_OP_MASK;
	auto op2 = (enc_flags1 >> EncFlags1::XOP_OP2_SHIFT) & EncFlags1::XOP_OP_MASK;
	auto op3 = (enc_flags1 >> EncFlags1::XOP_OP3_SHIFT) & EncFlags1::XOP_OP_MASK;
	
	ops[0] = XOP_TABLE[op0];
	ops[1] = XOP_TABLE[op1];
	ops[2] = XOP_TABLE[op2];
	ops[3] = XOP_TABLE[op3];
	
	std::size_t op_count = 0;
	if (op0 != 0) op_count = 1;
	if (op1 != 0) op_count = 2;
	if (op2 != 0) op_count = 3;
	if (op3 != 0) op_count = 4;
	
	base.operands = std::span<const Op* const>(ops.data(), op_count);
}

void XopHandler::encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction) {
	auto* self = reinterpret_cast<const XopHandler*>(handler);
	
	// Write segment prefix if needed
	auto seg = instruction.segment_prefix();
	if (seg != Register::NONE) {
		static constexpr uint8_t SEGMENT_OVERRIDES[] = {0x26, 0x2E, 0x36, 0x3E, 0x64, 0x65};
		auto seg_idx = static_cast<uint32_t>(seg) - static_cast<uint32_t>(Register::ES);
		if (seg_idx < 6) {
			encoder.write_byte_internal(SEGMENT_OVERRIDES[seg_idx]);
		}
	}
	
	// Write address size prefix if needed
	if ((encoder.encoder_flags() & EncoderFlags::P67) != 0) {
		encoder.write_byte_internal(0x67);
	}
	
	// XOP always uses 3-byte prefix: 8F RXBmmmmm WvvvvLpp
	encoder.write_byte_internal(0x8F);
	
	auto encoder_flags = encoder.encoder_flags();
	uint32_t b = self->table;
	b |= (~encoder_flags & 7) << 5;
	encoder.write_byte_internal(b);
	
	b = self->last_byte;
	b |= (~encoder_flags >> EncoderFlags::VVVVV_SHIFT) << 3;
	// Apply WIG/LIG from encoder settings (XOP uses same VEX WIG/LIG settings)
	b |= self->mask_w_l & encoder.internal_vex_wig_lig();
	encoder.write_byte_internal(b);
}

// EvexHandler implementation
EvexHandler::EvexHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3) {
	base.encode = &EvexHandler::encode;
	base.try_convert_to_disp8n = &EvexHandler::try_convert_to_disp8n;
	base.op_code = get_op_code(enc_flags2);
	base.group_index = (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.rm_group_index = (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.enc_flags3 = enc_flags3;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0;
	base.is_special_instr = false;
	
	table = (enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK;
	
	p1_bits = 4 | ((enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK);
	
	auto wbit = static_cast<WBit>((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK);
	w1 = (wbit == WBit::W1) ? UINT32_MAX : 0;
	mask_w = (wbit == WBit::WIG) ? 0x80 : 0;
	if (w1 != 0) {
		p1_bits |= 0x80;
	}
	
	auto lbit = static_cast<LBit>((enc_flags2 >> EncFlags2::LBIT_SHIFT) & EncFlags2::LBIT_MASK);
	switch (lbit) {
		case LBit::L0:
		case LBit::LZ:
		case LBit::L128:
			ll_bits = 0 << 5;  // 128-bit / scalar (pre-shifted)
			break;
		case LBit::L1:
		case LBit::L256:
			ll_bits = 1 << 5;  // 256-bit (pre-shifted)
			break;
		case LBit::L512:
			ll_bits = 2 << 5;  // 512-bit (pre-shifted)
			break;
		case LBit::LIG:
		default:
			ll_bits = 0 << 5;  // (pre-shifted)
			break;
	}
	mask_ll = (lbit == LBit::LIG) ? (3 << 5) : 0;  // Pre-shifted mask
	
	tuple_type = (enc_flags3 >> EncFlags3::TUPLE_TYPE_SHIFT) & EncFlags3::TUPLE_TYPE_MASK;
	
	// Set operands from EVEX_TABLE
	auto op0 = (enc_flags1 >> EncFlags1::EVEX_OP0_SHIFT) & EncFlags1::EVEX_OP_MASK;
	auto op1 = (enc_flags1 >> EncFlags1::EVEX_OP1_SHIFT) & EncFlags1::EVEX_OP_MASK;
	auto op2 = (enc_flags1 >> EncFlags1::EVEX_OP2_SHIFT) & EncFlags1::EVEX_OP_MASK;
	auto op3 = (enc_flags1 >> EncFlags1::EVEX_OP3_SHIFT) & EncFlags1::EVEX_OP_MASK;
	
	ops[0] = EVEX_TABLE[op0];
	ops[1] = EVEX_TABLE[op1];
	ops[2] = EVEX_TABLE[op2];
	ops[3] = EVEX_TABLE[op3];
	
	std::size_t op_count = 0;
	if (op0 != 0) op_count = 1;
	if (op1 != 0) op_count = 2;
	if (op2 != 0) op_count = 3;
	if (op3 != 0) op_count = 4;
	
	base.operands = std::span<const Op* const>(ops.data(), op_count);
}

void EvexHandler::encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction) {
	auto* self = reinterpret_cast<const EvexHandler*>(handler);
	
	// Write segment prefix if needed
	auto seg = instruction.segment_prefix();
	if (seg != Register::NONE) {
		static constexpr uint8_t SEGMENT_OVERRIDES[] = {0x26, 0x2E, 0x36, 0x3E, 0x64, 0x65};
		auto seg_idx = static_cast<uint32_t>(seg) - static_cast<uint32_t>(Register::ES);
		if (seg_idx < 6) {
			encoder.write_byte_internal(SEGMENT_OVERRIDES[seg_idx]);
		}
	}
	
	// Write address size prefix if needed
	if ((encoder.encoder_flags() & EncoderFlags::P67) != 0) {
		encoder.write_byte_internal(0x67);
	}
	
	// EVEX: 62 P0 P1 P2
	encoder.write_byte_internal(0x62);
	
	auto encoder_flags = encoder.encoder_flags();
	
	// P0: R X B R' 0 0 m m
	uint32_t b = self->table;
	b |= (~encoder_flags & 7) << 5;  // ~R ~X ~B
	b |= (~encoder_flags & EncoderFlags::R2) >> 5;  // ~R' (shift bit 9 to bit 4)
	encoder.write_byte_internal(b);
	
	// P1: W v v v v 1 p p
	b = self->p1_bits;
	b |= ((~encoder_flags >> EncoderFlags::VVVVV_SHIFT) & 0xF) << 3;  // ~vvvv (4 bits)
	// Apply WIG from encoder settings
	b |= self->mask_w & encoder.internal_evex_wig();
	encoder.write_byte_internal(b);
	
	// P2: z L' L b V' a a a
	
	// Validate and add mask register (aaa) - K0=0, K1=1, etc.
	auto mask_reg = instruction.op_mask();
	uint32_t aaa = 0;
	if (mask_reg != Register::NONE) {
		aaa = (static_cast<uint32_t>(mask_reg) - static_cast<uint32_t>(Register::K0)) & 7;
		if ((self->base.enc_flags3 & EncFlags3::OP_MASK_REGISTER) == 0) {
			encoder.set_error_message("The instruction doesn't support opmask registers");
		}
	} else {
		if ((self->base.enc_flags3 & EncFlags3::REQUIRE_OP_MASK_REGISTER) != 0) {
			encoder.set_error_message("The instruction must use an opmask register");
		}
	}
	
	b = aaa;
	b |= (encoder_flags >> (EncoderFlags::VVVVV_SHIFT + 4 - 3)) & 8;  // V' (not inverted yet, XOR 8 below does it)
	
	// Handle SAE (Suppress All Exceptions)
	if (instruction.suppress_all_exceptions()) {
		if ((self->base.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) == 0) {
			encoder.set_error_message("The instruction doesn't support suppress-all-exceptions");
		}
		b |= 0x10;
	}
	
	// Handle Rounding Control
	auto rc = instruction.rounding_control();
	if (rc != RoundingControl::NONE) {
		if ((self->base.enc_flags3 & EncFlags3::ROUNDING_CONTROL) == 0) {
			encoder.set_error_message("The instruction doesn't support rounding control");
		}
		b |= 0x10;
		// RC values: RoundToNearest=1, RoundDown=2, RoundUp=3, RoundTowardZero=4
		// We need (rc-1) << 5 to get L'L bits
		b |= (static_cast<uint32_t>(rc) - static_cast<uint32_t>(RoundingControl::ROUND_TO_NEAREST)) << 5;
	} else if ((self->base.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) == 0 || !instruction.suppress_all_exceptions()) {
		// Apply L'L bits only if not using SAE/RC (ll_bits is already pre-shifted)
		b |= self->ll_bits;
	}
	
	// Handle Broadcast
	if ((encoder_flags & EncoderFlags::BROADCAST) != 0) {
		b |= 0x10;  // broadcast bit
	} else if (instruction.is_broadcast()) {
		encoder.set_error_message("The instruction doesn't support broadcasting");
	}
	
	// Handle Zeroing masking
	if (instruction.zeroing_masking()) {
		if ((self->base.enc_flags3 & EncFlags3::ZEROING_MASKING) == 0) {
			encoder.set_error_message("The instruction doesn't support zeroing masking");
		}
		b |= 0x80;
	}
	
	// XOR V' bit (it's inverted in EVEX encoding)
	b ^= 8;
	// Apply LIG mask after inversion (mask_ll is pre-shifted)
	b |= self->mask_ll & encoder.internal_evex_lig();
	
	encoder.write_byte_internal(b);
}

// Helper function to get disp8n scale factor for EVEX instructions
// tuple_type is the TupleType enum value
// is_broadcast indicates if broadcast is enabled (b bit in EVEX P2)
static uint32_t get_evex_disp8n(uint32_t tuple_type, bool is_broadcast) noexcept {
	// TupleType enum values and their scale factors:
	// N1=0, N2=1, N4=2, N8=3, N16=4, N32=5, N64=6 -> scale = 1 << tuple_type
	// For broadcast types (N*B*), use the broadcast scale if b=1, else non-broadcast scale
	switch (static_cast<TupleType>(tuple_type)) {
		case TupleType::N1: return 1;
		case TupleType::N2: return 2;
		case TupleType::N4: return 4;
		case TupleType::N8: return 8;
		case TupleType::N16: return 16;
		case TupleType::N32: return 32;
		case TupleType::N64: return 64;
		case TupleType::N8B4: return is_broadcast ? 4 : 8;
		case TupleType::N16B4: return is_broadcast ? 4 : 16;
		case TupleType::N32B4: return is_broadcast ? 4 : 32;
		case TupleType::N64B4: return is_broadcast ? 4 : 64;
		case TupleType::N16B8: return is_broadcast ? 8 : 16;
		case TupleType::N32B8: return is_broadcast ? 8 : 32;
		case TupleType::N64B8: return is_broadcast ? 8 : 64;
		case TupleType::N4B2: return is_broadcast ? 2 : 4;
		case TupleType::N8B2: return is_broadcast ? 2 : 8;
		case TupleType::N16B2: return is_broadcast ? 2 : 16;
		case TupleType::N32B2: return is_broadcast ? 2 : 32;
		case TupleType::N64B2: return is_broadcast ? 2 : 64;
		default: return 1;
	}
}

bool EvexHandler::try_convert_to_disp8n(const EncoderOpCodeHandler* handler, Encoder& encoder, 
                                         const Instruction& /*instruction*/, int32_t displ, int8_t& result) {
	// Don't use compressed displacement for zero displacement - use mod=00 instead
	if (displ == 0) {
		return false;
	}
	auto* self = reinterpret_cast<const EvexHandler*>(handler);
	bool is_broadcast = (encoder.encoder_flags() & EncoderFlags::BROADCAST) != 0;
	auto n = static_cast<int32_t>(get_evex_disp8n(self->tuple_type, is_broadcast));
	int32_t res = displ / n;
	if (res * n == displ && res >= INT8_MIN && res <= INT8_MAX) {
		result = static_cast<int8_t>(res);
		return true;
	}
	return false;
}

// D3nowHandler implementation
D3nowHandler::D3nowHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3) {
	base.encode = &D3nowHandler::encode;
	base.try_convert_to_disp8n = nullptr;
	base.op_code = 0x0F0F;  // 3DNow! always uses 0F 0F
	base.group_index = -1;
	base.rm_group_index = -1;
	base.enc_flags3 = enc_flags3;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = true;
	base.is_special_instr = false;
	
	immediate = get_op_code(enc_flags2);  // The 3DNow! opcode byte
	
	// D3Now uses legacy operands (MMX registers)
	auto op0 = (enc_flags1 >> EncFlags1::LEGACY_OP0_SHIFT) & EncFlags1::LEGACY_OP_MASK;
	auto op1 = (enc_flags1 >> EncFlags1::LEGACY_OP1_SHIFT) & EncFlags1::LEGACY_OP_MASK;
	
	ops[0] = LEGACY_TABLE[op0];
	ops[1] = LEGACY_TABLE[op1];
	ops[2] = &g_none;
	ops[3] = &g_none;
	
	std::size_t op_count = 0;
	if (op0 != 0) op_count = 1;
	if (op1 != 0) op_count = 2;
	
	base.operands = std::span<const Op* const>(ops.data(), op_count);
}

void D3nowHandler::encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction) {
	auto* self = reinterpret_cast<const D3nowHandler*>(handler);
	
	encoder.write_prefixes(instruction, true);
	
	// Write REX if needed
	auto b = encoder.encoder_flags();
	b &= 0x4F;
	if (b != 0) {
		if ((encoder.encoder_flags() & EncoderFlags::HIGH_LEGACY_8_BIT_REGS) != 0) {
			encoder.set_error_message(
				"Registers AH, CH, DH, BH can't be used if there's a REX prefix.");
		}
		b |= 0x40;
		encoder.write_byte_internal(b);
	}
	
	// The opcode is written by the main encode() function
	// The immediate byte comes after ModRM/SIB/disp
	encoder.set_imm_size(ImmSize::SIZE1_OP_CODE);
	encoder.set_immediate(self->immediate);
}

// MvexHandler implementation (Intel Knights Corner / Xeon Phi)
MvexHandler::MvexHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3) {
	base.encode = &MvexHandler::encode;
	base.try_convert_to_disp8n = &MvexHandler::try_convert_to_disp8n;
	base.op_code = get_op_code(enc_flags2);
	base.group_index = (enc_flags2 & EncFlags2::HAS_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.rm_group_index = (enc_flags3 & EncFlags3::HAS_RM_GROUP_INDEX) == 0 
		? -1 
		: static_cast<int32_t>((enc_flags2 >> EncFlags2::GROUP_INDEX_SHIFT) & 7);
	base.enc_flags3 = enc_flags3;
	base.op_size = CodeSize::UNKNOWN;
	base.addr_size = CodeSize::UNKNOWN;
	base.is_2byte_opcode = (enc_flags2 & EncFlags2::OP_CODE_IS2_BYTES) != 0;
	base.is_special_instr = false;
	
	// MVEX table (1=0F, 2=0F38, 3=0F3A)
	table = (enc_flags2 >> EncFlags2::TABLE_SHIFT) & EncFlags2::TABLE_MASK;
	
	// Build p1_bits: pp (mandatory prefix) and W bit
	// MandatoryPrefixByte::NONE=0, P66=1, PF3=2, PF2=3
	p1_bits = (enc_flags2 >> EncFlags2::MANDATORY_PREFIX_SHIFT) & EncFlags2::MANDATORY_PREFIX_MASK;
	
	auto wbit_val = static_cast<WBit>((enc_flags2 >> EncFlags2::WBIT_SHIFT) & EncFlags2::WBIT_MASK);
	wbit = static_cast<uint32_t>(wbit_val);
	if (wbit_val == WBit::W1) {
		p1_bits |= 0x80;
	}
	mask_w = (wbit_val == WBit::WIG) ? 0x80 : 0;
	
	// Set operands from MVEX_TABLE
	auto op0 = (enc_flags1 >> EncFlags1::MVEX_OP0_SHIFT) & EncFlags1::MVEX_OP_MASK;
	auto op1 = (enc_flags1 >> EncFlags1::MVEX_OP1_SHIFT) & EncFlags1::MVEX_OP_MASK;
	auto op2 = (enc_flags1 >> EncFlags1::MVEX_OP2_SHIFT) & EncFlags1::MVEX_OP_MASK;
	auto op3 = (enc_flags1 >> EncFlags1::MVEX_OP3_SHIFT) & EncFlags1::MVEX_OP_MASK;
	
	ops[0] = MVEX_TABLE[op0];
	ops[1] = MVEX_TABLE[op1];
	ops[2] = MVEX_TABLE[op2];
	ops[3] = MVEX_TABLE[op3];
	
	std::size_t op_count = 0;
	if (op0 != 0) op_count = 1;
	if (op1 != 0) op_count = 2;
	if (op2 != 0) op_count = 3;
	if (op3 != 0) op_count = 4;
	
	base.operands = std::span<const Op* const>(ops.data(), op_count);
}

void MvexHandler::encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction) {
	auto* self = reinterpret_cast<const MvexHandler*>(handler);
	
	// Write segment prefix if needed
	auto seg = instruction.segment_prefix();
	if (seg != Register::NONE) {
		static constexpr uint8_t SEGMENT_OVERRIDES[] = {0x26, 0x2E, 0x36, 0x3E, 0x64, 0x65};
		auto seg_idx = static_cast<uint32_t>(seg) - static_cast<uint32_t>(Register::ES);
		if (seg_idx < 6) {
			encoder.write_byte_internal(SEGMENT_OVERRIDES[seg_idx]);
		}
	}
	
	// Write address size prefix if needed
	if ((encoder.encoder_flags() & EncoderFlags::P67) != 0) {
		encoder.write_byte_internal(0x67);
	}
	
	// MVEX: 62 P0 P1 P2 (4-byte prefix)
	encoder.write_byte_internal(0x62);
	
	auto encoder_flags = encoder.encoder_flags();
	
	// P0: ~R ~X ~B R' mmmm
	// mmmm: 1=0F, 2=0F38, 3=0F3A
	// R, X, B are inverted; R' is also inverted
	uint32_t b = self->table;
	// ~R, ~X, ~B in bits 7, 6, 5
	b |= (encoder_flags & 7) << 5;
	// R' (bit 4 of R2) in bit 4 - inverted
	b |= (encoder_flags >> (9 - 4)) & 0x10;  // EncoderFlags::R2 = 0x200
	b ^= ~0x0FU;  // Invert R, X, B, R' (but not mmmm)
	encoder.write_byte_internal(b);
	
	// P1: W vvvv pp
	// W bit, inverted vvvvv in bits 6:3, mandatory prefix pp in bits 1:0
	b = self->p1_bits;
	b |= (~encoder_flags >> (EncoderFlags::VVVVV_SHIFT - 3)) & 0x78;  // ~vvvv in bits 6:3
	// Apply WIG mask if applicable
	b |= self->mask_w & encoder.internal_mvex_wig();
	encoder.write_byte_internal(b);
	
	// P2: E V' aaa SSS
	// E = eviction hint (bit 7)
	// V' = vvvv[4] inverted (bit 3)
	// aaa = opmask register (bits 2:0)
	// SSS = swizzle/conversion (bits 6:4)
	
	// Start with opmask register
	auto mask_reg = instruction.op_mask();
	if (mask_reg != Register::NONE) {
		b = (static_cast<uint32_t>(mask_reg) - static_cast<uint32_t>(Register::K0)) & 7;
		if ((self->base.enc_flags3 & EncFlags3::OP_MASK_REGISTER) == 0) {
			encoder.set_error_message("The instruction doesn't support opmask registers");
		}
	} else {
		b = 0;
		if ((self->base.enc_flags3 & EncFlags3::REQUIRE_OP_MASK_REGISTER) != 0) {
			encoder.set_error_message("The instruction must use an opmask register");
		}
	}
	
	// V' (bit 3) - vvvv[4] inverted
	b |= (encoder_flags >> (EncoderFlags::VVVVV_SHIFT + 4 - 3)) & 8;
	
	// Check if any operand is memory
	bool has_memory = (instruction.op0_kind() == OpKind::MEMORY || 
	                   instruction.op1_kind() == OpKind::MEMORY || 
	                   instruction.op2_kind() == OpKind::MEMORY);
	
	auto conv = instruction.mvex_reg_mem_conv();
	const auto& mvex = get_mvex_info(instruction.code());
	
	if (has_memory) {
		// Memory operands: SSS = memory conversion, EH = eviction hint
		// MvexRegMemConv::MEM_CONV_NONE = 9, so SSS = (conv - 9) & 7
		if (conv >= MvexRegMemConv::MEM_CONV_NONE && conv <= MvexRegMemConv::MEM_CONV_SINT16) {
			b |= ((static_cast<uint32_t>(conv) - static_cast<uint32_t>(MvexRegMemConv::MEM_CONV_NONE)) & 7) << 4;
		} else if (conv != MvexRegMemConv::NONE) {
			encoder.set_error_message("Memory operands must use a valid MvexRegMemConv variant, eg. MvexRegMemConv::MEM_CONV_NONE");
		}
		// Eviction hint in EH bit (bit 7)
		if (instruction.is_mvex_eviction_hint()) {
			if (!mvex.can_use_eviction_hint()) {
				encoder.set_error_message("This instruction doesn't support eviction hint (`{eh}`)");
			}
			b |= 0x80;
		}
	} else {
		// Register operands
		if (instruction.is_mvex_eviction_hint()) {
			encoder.set_error_message("Only memory operands can enable eviction hint (`{eh}`)");
		}
		
		if (conv == MvexRegMemConv::NONE) {
			// No conversion - set EH bit (bit 7) = 1
			b |= 0x80;
			
			// Handle suppress all exceptions
			if (instruction.suppress_all_exceptions()) {
				b |= 0x40;  // SAE uses SSS[2] = 1
				if ((self->base.enc_flags3 & EncFlags3::SUPPRESS_ALL_EXCEPTIONS) == 0) {
					encoder.set_error_message("The instruction doesn't support suppress-all-exceptions");
				}
			}
			
			// Handle rounding control
			auto rc = instruction.rounding_control();
			if (rc != RoundingControl::NONE) {
				if ((self->base.enc_flags3 & EncFlags3::ROUNDING_CONTROL) == 0) {
					encoder.set_error_message("The instruction doesn't support rounding control");
				} else {
					// RoundingControl: NONE=0, RoundToNearest=1, RoundDown=2, RoundUp=3, RoundTowardZero=4
					b |= ((static_cast<uint32_t>(rc) - 1) & 3) << 4;
				}
			}
		} else if (conv >= MvexRegMemConv::REG_SWIZZLE_NONE && conv <= MvexRegMemConv::REG_SWIZZLE_DDDD) {
			// Register swizzle: SSS = (conv - REG_SWIZZLE_NONE) & 7
			if (instruction.suppress_all_exceptions()) {
				encoder.set_error_message("Can't use {sae} with register swizzles");
			} else if (instruction.rounding_control() != RoundingControl::NONE) {
				encoder.set_error_message("Can't use rounding control with register swizzles");
			}
			b |= ((static_cast<uint32_t>(conv) - static_cast<uint32_t>(MvexRegMemConv::REG_SWIZZLE_NONE)) & 7) << 4;
		} else {
			encoder.set_error_message("Register operands can't use memory up/down conversions");
		}
	}
	
	// Handle instructions that require EH=1 (like Vmovnrngoapd)
	if (mvex.eh_bit == MvexEHBit::EH1) {
		b |= 0x80;
	}
	
	// Invert V' bit (bit 3)
	b ^= 8;
	
	encoder.write_byte_internal(b);
}

bool MvexHandler::try_convert_to_disp8n(const EncoderOpCodeHandler* /*handler*/, Encoder& /*encoder*/, 
                                         const Instruction& instruction, int32_t displ, int8_t& result) {
	// Get MVEX info for this instruction
	const auto& mvex = get_mvex_info(instruction.code());
	
	// Get the SSS bits from the memory conversion
	// MvexRegMemConv::MEM_CONV_NONE = 9, so SSS = (conv - 9) & 7
	auto conv = instruction.mvex_reg_mem_conv();
	uint32_t sss;
	if (conv >= MvexRegMemConv::MEM_CONV_NONE && conv <= MvexRegMemConv::MEM_CONV_SINT16) {
		sss = (static_cast<uint32_t>(conv) - static_cast<uint32_t>(MvexRegMemConv::MEM_CONV_NONE)) & 7;
	} else {
		sss = 0;  // Default to no conversion
	}
	
	// Look up the tuple type from the LUT
	auto lut_idx = static_cast<std::size_t>(mvex.tuple_type_lut_kind) * 8 + sss;
	auto tuple_type = MVEX_TUPLE_TYPE_LUT[lut_idx];
	
	// Get the displacement scale factor
	auto n = static_cast<int32_t>(get_disp8n(tuple_type));
	
	int32_t res = displ / n;
	if (res * n == displ && res >= INT8_MIN && res <= INT8_MAX) {
		result = static_cast<int8_t>(res);
		return true;
	}
	return false;
}

// HANDLERS_TABLE
// Handlers are created dynamically at startup based on ENC_FLAGS tables

namespace {

// Storage for handler instances
struct HandlerStorage {
	std::vector<std::unique_ptr<InvalidHandler>> invalid_handlers;
	std::vector<std::unique_ptr<DeclareDataHandler>> declare_data_handlers;
	std::vector<std::unique_ptr<ZeroBytesHandler>> zero_bytes_handlers;
	std::vector<std::unique_ptr<LegacyHandler>> legacy_handlers;
	std::vector<std::unique_ptr<VexHandler>> vex_handlers;
	std::vector<std::unique_ptr<XopHandler>> xop_handlers;
	std::vector<std::unique_ptr<EvexHandler>> evex_handlers;
	std::vector<std::unique_ptr<D3nowHandler>> d3now_handlers;
	std::vector<std::unique_ptr<MvexHandler>> mvex_handlers;
	std::array<const EncoderOpCodeHandler*, IcedConstants::CODE_ENUM_COUNT> table{};
	
	HandlerStorage() {
		// Reserve space to avoid reallocations
		invalid_handlers.reserve(10);
		declare_data_handlers.reserve(5);
		zero_bytes_handlers.reserve(5);
		legacy_handlers.reserve(3000);
		vex_handlers.reserve(1500);
		xop_handlers.reserve(200);
		evex_handlers.reserve(1500);
		d3now_handlers.reserve(50);
		mvex_handlers.reserve(200);
		
		// Create a single invalid handler for reuse
		invalid_handlers.push_back(std::make_unique<InvalidHandler>());
		auto* invalid = &invalid_handlers[0]->base;
		
		// Populate the handlers table
		for (std::size_t i = 0; i < IcedConstants::CODE_ENUM_COUNT; ++i) {
			auto code = static_cast<Code>(i);
			auto enc_flags1 = ENC_FLAGS1[i];
			auto enc_flags2 = ENC_FLAGS2[i];
			auto enc_flags3 = ENC_FLAGS3[i];
			
			auto encoding = static_cast<EncodingKind>((enc_flags3 >> EncFlags3::ENCODING_SHIFT) & EncFlags3::ENCODING_MASK);
			
			const EncoderOpCodeHandler* handler = nullptr;
			
			switch (encoding) {
				case EncodingKind::LEGACY:
					if (code == Code::INVALID) {
						handler = invalid;
					} else if (code == Code::DECLARE_BYTE || code == Code::DECLARE_WORD || 
					           code == Code::DECLARE_DWORD || code == Code::DECLARE_QWORD) {
						declare_data_handlers.push_back(std::make_unique<DeclareDataHandler>(code));
						handler = &declare_data_handlers.back()->base;
					} else if (code == Code::ZERO_BYTES) {
						zero_bytes_handlers.push_back(std::make_unique<ZeroBytesHandler>(code));
						handler = &zero_bytes_handlers.back()->base;
					} else {
						legacy_handlers.push_back(std::make_unique<LegacyHandler>(enc_flags1, enc_flags2, enc_flags3));
						handler = &legacy_handlers.back()->base;
					}
					break;
					
				case EncodingKind::VEX:
					vex_handlers.push_back(std::make_unique<VexHandler>(enc_flags1, enc_flags2, enc_flags3));
					handler = &vex_handlers.back()->base;
					break;
					
				case EncodingKind::EVEX:
					evex_handlers.push_back(std::make_unique<EvexHandler>(enc_flags1, enc_flags2, enc_flags3));
					handler = &evex_handlers.back()->base;
					break;
					
				case EncodingKind::XOP:
					xop_handlers.push_back(std::make_unique<XopHandler>(enc_flags1, enc_flags2, enc_flags3));
					handler = &xop_handlers.back()->base;
					break;
					
				case EncodingKind::D3NOW:
					d3now_handlers.push_back(std::make_unique<D3nowHandler>(enc_flags1, enc_flags2, enc_flags3));
					handler = &d3now_handlers.back()->base;
					break;
					
			case EncodingKind::MVEX:
				mvex_handlers.push_back(std::make_unique<MvexHandler>(enc_flags1, enc_flags2, enc_flags3));
				handler = &mvex_handlers.back()->base;
				break;
					
				default:
					handler = invalid;
					break;
			}
			
			table[i] = handler;
		}
	}
};

// Global handler storage (initialized on first use)
HandlerStorage& get_handler_storage() {
	static HandlerStorage storage;
	return storage;
}

} // anonymous namespace

// External declaration of HANDLERS_TABLE
const EncoderOpCodeHandler* const* get_handlers_table() {
	return get_handler_storage().table.data();
}

} // namespace iced_x86::internal

namespace iced_x86 {
// Define the external HANDLERS_TABLE reference used by Encoder
const internal::OpCodeHandler* const HANDLERS_TABLE[] = { nullptr };  // Placeholder, use get_handlers_table() instead
} // namespace iced_x86
