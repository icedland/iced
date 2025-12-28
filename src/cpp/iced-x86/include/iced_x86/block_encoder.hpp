// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_BLOCK_ENCODER_HPP
#define ICED_X86_BLOCK_ENCODER_HPP

#ifndef ICED_X86_NO_BLOCK_ENCODER

#include "instruction.hpp"
#include "encoder.hpp"
#include <cstdint>
#include <vector>
#include <span>
#include <string>
#include <expected>

namespace iced_x86 {

/// @brief Block encoder options
namespace BlockEncoderOptions {
	using Value = uint32_t;
	
	/// @brief No options set
	constexpr Value NONE = 0;
	
	/// @brief Disable automatic branch target fixup. By default, the encoder will convert
	/// SHORT branches to NEAR if the target is too far away. This option disables that.
	constexpr Value DONT_FIX_BRANCHES = 0x00000001;
	
	/// @brief Return relocation information for 64-bit addresses
	constexpr Value RETURN_RELOC_INFOS = 0x00000002;
	
	/// @brief Return new instruction offsets
	constexpr Value RETURN_NEW_INSTRUCTION_OFFSETS = 0x00000004;
	
	/// @brief Return constant offsets for each instruction
	constexpr Value RETURN_CONSTANT_OFFSETS = 0x00000008;
}

/// @brief Relocation kind
enum class RelocKind : uint32_t {
	/// @brief 64-bit offset
	OFFSET64 = 0
};

/// @brief Relocation information
struct RelocInfo {
	uint64_t address = 0;      ///< Address of the relocation
	RelocKind kind = RelocKind::OFFSET64;  ///< Relocation kind
	
	RelocInfo() = default;
	RelocInfo( RelocKind k, uint64_t addr ) : address( addr ), kind( k ) {}
};

/// @brief Result from block encoding
struct BlockEncoderResult {
	uint64_t rip = 0;                            ///< Base RIP of encoded instructions
	std::vector<uint8_t> code_buffer;            ///< Encoded bytes
	std::vector<RelocInfo> reloc_infos;          ///< Relocation info (if requested)
	std::vector<uint32_t> new_instruction_offsets; ///< New offsets (if requested), UINT32_MAX if rewritten
	std::vector<ConstantOffsets> constant_offsets; ///< Constant offsets (if requested)
};

/// @brief An instruction block to be encoded
struct InstructionBlock {
	std::span<const Instruction> instructions;  ///< Instructions to encode
	uint64_t rip = 0;                           ///< Base RIP for the block
	
	InstructionBlock() = default;
	InstructionBlock( std::span<const Instruction> instrs, uint64_t base_rip )
		: instructions( instrs ), rip( base_rip ) {}
};

/// @brief Encodes multiple instructions, fixing branch targets as needed.
///
/// The BlockEncoder can encode a block of instructions and automatically
/// fix up branch targets that are too far away (e.g., converting SHORT
/// branches to NEAR branches).
///
/// @example
/// @code
/// std::vector<Instruction> instructions;
/// // ... populate instructions ...
/// 
/// auto result = BlockEncoder::encode( 64, instructions, 0x1000 );
/// if ( result ) {
///     // result->code_buffer contains the encoded bytes
/// }
/// @endcode
class BlockEncoder {
public:
	/// @brief Encodes a block of instructions.
	/// @param bitness 16, 32, or 64
	/// @param instructions Instructions to encode
	/// @param rip Base RIP for the encoded instructions
	/// @param options Encoding options
	/// @return Encoded result or error message
	[[nodiscard]] static std::expected<BlockEncoderResult, std::string> encode(
		uint32_t bitness,
		std::span<const Instruction> instructions,
		uint64_t rip,
		BlockEncoderOptions::Value options = BlockEncoderOptions::NONE
	) noexcept;
	
	/// @brief Encodes a block of instructions.
	/// @param bitness 16, 32, or 64
	/// @param block Instruction block
	/// @param options Encoding options
	/// @return Encoded result or error message
	[[nodiscard]] static std::expected<BlockEncoderResult, std::string> encode(
		uint32_t bitness,
		const InstructionBlock& block,
		BlockEncoderOptions::Value options = BlockEncoderOptions::NONE
	) noexcept;
	
	/// @brief Encodes multiple instruction blocks.
	/// @param bitness 16, 32, or 64
	/// @param blocks Instruction blocks
	/// @param options Encoding options
	/// @return Vector of encoded results or error message
	[[nodiscard]] static std::expected<std::vector<BlockEncoderResult>, std::string> encode(
		uint32_t bitness,
		std::span<const InstructionBlock> blocks,
		BlockEncoderOptions::Value options = BlockEncoderOptions::NONE
	) noexcept;

private:
	BlockEncoder( uint32_t bitness, BlockEncoderOptions::Value options ) noexcept;
	
	std::expected<BlockEncoderResult, std::string> encode_block( 
		std::span<const Instruction> instructions, 
		uint64_t rip 
	) noexcept;
	
	bool try_encode_instruction( const Instruction& instr, uint64_t ip ) noexcept;
	
	uint32_t bitness_;
	BlockEncoderOptions::Value options_;
	Encoder encoder_;
	std::string error_message_;
};

} // namespace iced_x86

#endif // !ICED_X86_NO_BLOCK_ENCODER

#endif // ICED_X86_BLOCK_ENCODER_HPP
