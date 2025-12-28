// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_ENCODER_HPP
#define ICED_X86_ENCODER_HPP

#ifndef ICED_X86_NO_ENCODER

#include "iced_x86/instruction.hpp"
#include "iced_x86/code_size.hpp"
#include "iced_x86/internal/encoder_flags.hpp"
#include "iced_x86/internal/encoder_displ_size.hpp"
#include "iced_x86/internal/encoder_imm_size.hpp"
#include "iced_x86/internal/encoder_EncFlags1.hpp"
#include "iced_x86/internal/encoder_EncFlags2.hpp"
#include "iced_x86/internal/encoder_EncFlags3.hpp"
#include "iced_x86/internal/encoder_handler.hpp"

#include <cstdint>
#include <cstddef>
#include <vector>
#include <expected>
#include <string>
#include <string_view>

namespace iced_x86 {

// Forward declarations
namespace internal {
struct OpCodeHandler;
}

/// @brief Error information returned when encoding fails.
struct EncodeError {
	std::string message;
	
	EncodeError() = default;
	explicit EncodeError(std::string msg) : message(std::move(msg)) {}
	explicit EncodeError(std::string_view msg) : message(msg) {}
};

/// @brief Constant offsets in an encoded instruction.
struct ConstantOffsets {
	uint8_t displacement_offset = 0;
	uint8_t displacement_size = 0;
	uint8_t immediate_offset = 0;
	uint8_t immediate_size = 0;
	uint8_t immediate_offset2 = 0;
	uint8_t immediate_size2 = 0;
	
	/// @brief Checks if there's a displacement.
	[[nodiscard]] constexpr bool has_displacement() const noexcept { return displacement_size != 0; }
	
	/// @brief Checks if there's an immediate.
	[[nodiscard]] constexpr bool has_immediate() const noexcept { return immediate_size != 0; }
	
	/// @brief Checks if there's a second immediate.
	[[nodiscard]] constexpr bool has_immediate2() const noexcept { return immediate_size2 != 0; }
};

/// @brief Encodes instructions decoded by the decoder or instructions created by other code.
///
/// @example
/// ```cpp
/// // xchg ah,[rdx+rsi+16h]
/// std::vector<uint8_t> bytes = {0x86, 0x64, 0x32, 0x16};
/// Decoder decoder(64, bytes, 0x12345678);
/// auto instr = decoder.decode().value();
///
/// Encoder encoder(64);
/// auto result = encoder.encode(instr, 0x55555555);
/// if (result) {
///     assert(*result == 4);
/// }
/// auto buffer = encoder.take_buffer();
/// assert(buffer == std::vector<uint8_t>{0x86, 0x64, 0x32, 0x16});
/// ```
class Encoder {
public:
	/// @brief Creates an encoder.
	/// @param bitness 16, 32, or 64
	/// @throws std::invalid_argument if bitness is not 16, 32, or 64
	explicit Encoder(uint32_t bitness);
	
	/// @brief Creates an encoder with an initial buffer capacity.
	/// @param bitness 16, 32, or 64
	/// @param capacity Initial capacity of the output buffer
	/// @throws std::invalid_argument if bitness is not 16, 32, or 64
	Encoder(uint32_t bitness, std::size_t capacity);
	
	/// @brief Encodes an instruction and returns the size of the encoded instruction.
	/// @param instruction Instruction to encode
	/// @param rip RIP of the encoded instruction
	/// @return Size of encoded instruction in bytes, or error
	[[nodiscard]] std::expected<std::size_t, EncodeError> encode(const Instruction& instruction, uint64_t rip) noexcept;
	
	/// @brief Writes a byte to the output buffer.
	/// @param value Byte to write
	void write_u8(uint8_t value) noexcept;
	
	/// @brief Returns the buffer and initializes the internal buffer to an empty vector.
	/// @return The encoded bytes
	[[nodiscard]] std::vector<uint8_t> take_buffer() noexcept;
	
	/// @brief Overwrites the buffer with a new vector.
	/// @param buffer New buffer
	void set_buffer(std::vector<uint8_t> buffer) noexcept;
	
	/// @brief Gets the offsets of constants in the encoded instruction.
	/// @return Constant offsets
	[[nodiscard]] ConstantOffsets get_constant_offsets() const noexcept;
	
	/// @brief Gets the bitness.
	[[nodiscard]] uint32_t bitness() const noexcept { return bitness_; }
	
	/// @brief Gets current buffer position.
	[[nodiscard]] std::size_t position() const noexcept { return buffer_.size(); }
	
	/// @brief Gets a reference to the buffer (without taking ownership).
	[[nodiscard]] const std::vector<uint8_t>& buffer() const noexcept { return buffer_; }

	// Internal methods used by Op handlers
	void set_error_message(std::string_view message) noexcept;
	[[nodiscard]] bool verify_op_kind(uint32_t operand, OpKind expected, OpKind actual) noexcept;
	[[nodiscard]] bool verify_register(uint32_t operand, Register expected, Register actual) noexcept;
	[[nodiscard]] bool verify_register_range(uint32_t operand, Register reg, Register reg_lo, Register reg_hi) noexcept;
	
	void add_branch(OpKind op_kind, uint32_t imm_size, const Instruction& instruction, uint32_t operand) noexcept;
	void add_branch_x(uint32_t imm_size, const Instruction& instruction, uint32_t operand) noexcept;
	void add_branch_disp(uint32_t displ_size, const Instruction& instruction, uint32_t operand) noexcept;
	void add_far_branch(const Instruction& instruction, uint32_t operand, uint32_t size) noexcept;
	void set_addr_size(uint32_t reg_size) noexcept;
	void add_abs_mem(const Instruction& instruction, uint32_t operand) noexcept;
	void add_mod_rm_register(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi) noexcept;
	void add_reg(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi) noexcept;
	void add_reg_or_mem(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi, bool allow_mem_op, bool allow_reg_op) noexcept;
	void add_reg_or_mem_full(const Instruction& instruction, uint32_t operand, Register reg_lo, Register reg_hi, 
		Register vsib_index_reg_lo, Register vsib_index_reg_hi, bool allow_mem_op, bool allow_reg_op) noexcept;
	
	void write_byte_internal(uint32_t value) noexcept;
	
	// Internal state accessors for Op handlers
	void set_immediate(uint32_t value) noexcept { immediate_ = value; }
	void set_immediate_hi(uint32_t value) noexcept { immediate_hi_ = value; }
	void set_imm_size(internal::ImmSize size) noexcept { imm_size_ = size; }
	void or_encoder_flags(uint32_t flags) noexcept { encoder_flags_ |= flags; }
	void or_mod_rm(uint8_t value) noexcept { mod_rm_ |= value; }
	void or_op_code(uint32_t value) noexcept { op_code_ |= value; }
	
	[[nodiscard]] uint32_t encoder_flags() const noexcept { return encoder_flags_; }
	[[nodiscard]] uint32_t op_code() const noexcept { return op_code_; }
	[[nodiscard]] uint64_t current_rip() const noexcept { return current_rip_; }
	[[nodiscard]] uint32_t opsize16_flags() const noexcept { return opsize16_flags_; }
	[[nodiscard]] uint32_t opsize32_flags() const noexcept { return opsize32_flags_; }
	[[nodiscard]] uint32_t internal_mvex_wig() const noexcept { return internal_mvex_wig_; }
	[[nodiscard]] uint32_t internal_prevent_vex2() const noexcept { return prevent_vex2_; }
	[[nodiscard]] uint32_t internal_vex_wig_lig() const noexcept { return internal_vex_wig_lig_; }
	[[nodiscard]] uint32_t internal_evex_wig() const noexcept { return internal_evex_wig_; }
	[[nodiscard]] uint32_t internal_evex_lig() const noexcept { return internal_evex_lig_; }
	
	/// @brief If true, don't use 2-byte VEX encoding even if possible
	[[nodiscard]] bool prevent_vex2() const noexcept { return prevent_vex2_ != 0; }
	/// @brief If true, don't use 2-byte VEX encoding even if possible
	void set_prevent_vex2(bool value) noexcept { prevent_vex2_ = value ? UINT32_MAX : 0; }
	
	/// @brief Value of VEX.W bit to use if the instruction has WIG (W-ignore)
	[[nodiscard]] uint32_t vex_wig() const noexcept { return internal_vex_wig_lig_ >> 7; }
	/// @brief Value of VEX.W bit to use if the instruction has WIG (W-ignore)
	void set_vex_wig(uint32_t value) noexcept { internal_vex_wig_lig_ = (internal_vex_wig_lig_ & 0x7F) | ((value & 1) << 7); }
	
	/// @brief Value of VEX.L bit to use if the instruction has LIG (L-ignore)
	[[nodiscard]] uint32_t vex_lig() const noexcept { return (internal_vex_wig_lig_ >> 2) & 1; }
	/// @brief Value of VEX.L bit to use if the instruction has LIG (L-ignore)
	void set_vex_lig(uint32_t value) noexcept { internal_vex_wig_lig_ = (internal_vex_wig_lig_ & ~4) | ((value & 1) << 2); }
	
	/// @brief Value of EVEX.W bit to use if the instruction has WIG (W-ignore)
	[[nodiscard]] uint32_t evex_wig() const noexcept { return internal_evex_wig_ >> 7; }
	/// @brief Value of EVEX.W bit to use if the instruction has WIG (W-ignore)
	void set_evex_wig(uint32_t value) noexcept { internal_evex_wig_ = (value & 1) << 7; }
	
	/// @brief Value of EVEX.L'L bits to use if the instruction has LIG (L-ignore)
	[[nodiscard]] uint32_t evex_lig() const noexcept { return internal_evex_lig_ >> 5; }
	/// @brief Value of EVEX.L'L bits to use if the instruction has LIG (L-ignore)
	void set_evex_lig(uint32_t value) noexcept { internal_evex_lig_ = (value & 3) << 5; }
	
	/// @brief Value of MVEX.W bit to use if the instruction has WIG (W-ignore)
	[[nodiscard]] uint32_t mvex_wig() const noexcept { return internal_mvex_wig_ >> 7; }
	/// @brief Value of MVEX.W bit to use if the instruction has WIG (W-ignore)
	void set_mvex_wig(uint32_t value) noexcept { internal_mvex_wig_ = (value & 1) << 7; }

	static constexpr const char* ERROR_ONLY_1632_BIT_MODE = "The instruction can only be used in 16/32-bit mode";
	static constexpr const char* ERROR_ONLY_64_BIT_MODE = "The instruction can only be used in 64-bit mode";

private:
	// Handler encode functions need access to write_prefixes
	friend struct internal::LegacyHandler;
	friend struct internal::VexHandler;
	friend struct internal::XopHandler;
	friend struct internal::EvexHandler;
	friend struct internal::D3nowHandler;
	friend struct internal::MvexHandler;
	
	void write_prefixes(const Instruction& instruction, bool can_write_f3) noexcept;
	void write_mod_rm() noexcept;
	void write_immediate() noexcept;
	void add_mem_op16(const Instruction& instruction, uint32_t operand) noexcept;
	void add_mem_op(const Instruction& instruction, uint32_t operand, uint32_t addr_size, 
		Register vsib_index_reg_lo, Register vsib_index_reg_hi) noexcept;
	[[nodiscard]] static uint32_t get_register_op_size(const Instruction& instruction) noexcept;
	[[nodiscard]] static uint32_t get_address_size_in_bytes(const Instruction& instruction, CodeSize code_size) noexcept;

	uint64_t current_rip_ = 0;
	std::vector<uint8_t> buffer_;
	const internal::EncoderOpCodeHandler* handler_ = nullptr;
	std::string error_message_;
	uint32_t bitness_ = 0;
	uint32_t eip_ = 0;
	uint32_t displ_addr_ = 0;
	uint32_t imm_addr_ = 0;
	uint32_t immediate_ = 0;
	uint32_t immediate_hi_ = 0;
	uint32_t displ_ = 0;
	uint32_t displ_hi_ = 0;
	uint32_t op_code_ = 0;
	uint32_t internal_vex_wig_lig_ = 0;
	uint32_t internal_vex_lig_ = 0;
	uint32_t internal_evex_wig_ = 0;
	uint32_t internal_evex_lig_ = 0;
	uint32_t internal_mvex_wig_ = 0;
	uint32_t prevent_vex2_ = 0;
	uint32_t opsize16_flags_ = 0;
	uint32_t opsize32_flags_ = 0;
	uint32_t adrsize16_flags_ = 0;
	uint32_t adrsize32_flags_ = 0;
	uint32_t encoder_flags_ = 0;
	internal::DisplSize displ_size_ = internal::DisplSize::NONE;
	internal::ImmSize imm_size_ = internal::ImmSize::NONE;
	uint8_t mod_rm_ = 0;
	uint8_t sib_ = 0;
};

} // namespace iced_x86

#endif // !ICED_X86_NO_ENCODER

#endif // ICED_X86_ENCODER_HPP
