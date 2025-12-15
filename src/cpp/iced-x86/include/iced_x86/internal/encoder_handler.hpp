// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_ENCODER_HANDLER_HPP
#define ICED_X86_ENCODER_HANDLER_HPP

#include "iced_x86/code_size.hpp"
#include "iced_x86/internal/encoder_ops.hpp"
#include "iced_x86/internal/encoder_EncFlags2.hpp"
#include "iced_x86/internal/encoder_EncFlags3.hpp"

#include <array>
#include <cstdint>
#include <span>

namespace iced_x86 {

// Forward declarations
class Encoder;
struct Instruction;

namespace internal {

// Forward declaration needed for function pointer types
struct EncoderOpCodeHandler;

/// @brief Function pointer type for encoding
using EncodeFn = void (*)(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);

/// @brief Function pointer type for disp8n conversion (EVEX compressed displacement)
using TryConvertToDisp8NFn = bool (*)(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction, int32_t displ, int8_t& result);

/// @brief Opcode handler base for all encodings
struct EncoderOpCodeHandler {
	EncodeFn encode = nullptr;
	TryConvertToDisp8NFn try_convert_to_disp8n = nullptr;
	std::span<const Op* const> operands;
	uint32_t op_code = 0;
	int32_t group_index = -1;
	int32_t rm_group_index = -1;
	uint32_t enc_flags3 = 0;
	CodeSize op_size = CodeSize::UNKNOWN;
	CodeSize addr_size = CodeSize::UNKNOWN;
	bool is_2byte_opcode = false;
	bool is_special_instr = false;
};

/// @brief Invalid instruction handler
struct InvalidHandler {
	EncoderOpCodeHandler base;
	
	InvalidHandler();
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
	static constexpr const char* ERROR_MESSAGE = "Can't encode an invalid instruction";
};

/// @brief Declare data handler (db, dw, dd, dq)
struct DeclareDataHandler {
	EncoderOpCodeHandler base;
	uint32_t elem_size;
	
	explicit DeclareDataHandler(Code code);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
};

/// @brief Zero bytes handler (placeholder instructions)
struct ZeroBytesHandler {
	EncoderOpCodeHandler base;
	
	explicit ZeroBytesHandler(Code code);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
};

/// @brief Legacy instruction handler
struct LegacyHandler {
	EncoderOpCodeHandler base;
	std::array<const Op*, 4> ops;  ///< Operand encoders from LEGACY_TABLE
	uint32_t table_byte1;
	uint32_t table_byte2;
	uint32_t mandatory_prefix;
	
	LegacyHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
};

/// @brief VEX instruction handler
struct VexHandler {
	EncoderOpCodeHandler base;
	std::array<const Op*, 4> ops;  ///< Operand encoders from VEX_TABLE
	uint32_t table;
	uint32_t last_byte;
	uint32_t mask_w_l;
	uint32_t mask_l;
	uint32_t w1;
	
	VexHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
};

/// @brief XOP instruction handler
struct XopHandler {
	EncoderOpCodeHandler base;
	std::array<const Op*, 4> ops;  ///< Operand encoders from XOP_TABLE
	uint32_t table;
	uint32_t last_byte;
	uint32_t mask_w_l;
	uint32_t mask_l;
	uint32_t w1;
	
	XopHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
};

/// @brief EVEX instruction handler
struct EvexHandler {
	EncoderOpCodeHandler base;
	std::array<const Op*, 4> ops;  ///< Operand encoders from EVEX_TABLE
	uint32_t table;
	uint32_t p1_bits;
	uint32_t ll_bits;
	uint32_t mask_w;
	uint32_t mask_ll;
	uint32_t tuple_type;
	uint32_t w1;
	
	EvexHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
	static bool try_convert_to_disp8n(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction, int32_t displ, int8_t& result);
};

/// @brief D3NOW instruction handler
struct D3nowHandler {
	EncoderOpCodeHandler base;
	std::array<const Op*, 4> ops;  ///< Operand encoders from LEGACY_TABLE (D3NOW uses legacy operands)
	uint32_t immediate;
	
	D3nowHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
};

/// @brief MVEX instruction handler (Intel Knights Corner / Xeon Phi)
struct MvexHandler {
	EncoderOpCodeHandler base;
	std::array<const Op*, 4> ops;  ///< Operand encoders from MVEX_TABLE
	uint32_t table;       ///< Opcode table (1=0F, 2=0F38, 3=0F3A)
	uint32_t p1_bits;     ///< Pre-computed: pp | W bit
	uint32_t mask_w;      ///< 0x80 if WIG, else 0
	uint32_t wbit;        ///< WBit enum value
	
	MvexHandler(uint32_t enc_flags1, uint32_t enc_flags2, uint32_t enc_flags3);
	static void encode(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction);
	static bool try_convert_to_disp8n(const EncoderOpCodeHandler* handler, Encoder& encoder, const Instruction& instruction, int32_t displ, int8_t& result);
};

/// @brief Get opcode from enc_flags2
inline constexpr uint32_t get_op_code(uint32_t enc_flags2) noexcept {
	return (enc_flags2 >> EncFlags2::OP_CODE_SHIFT) & 0xFFFF;
}

} // namespace internal
} // namespace iced_x86

#endif // ICED_X86_ENCODER_HANDLER_HPP
