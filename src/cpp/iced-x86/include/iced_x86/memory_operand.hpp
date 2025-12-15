// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_MEMORY_OPERAND_HPP
#define ICED_X86_MEMORY_OPERAND_HPP

#include "register.hpp"
#include <cstdint>

namespace iced_x86 {

/// @brief Memory operand passed to Instruction's with_*() factory methods
struct MemoryOperand {
	/// @brief Segment override or Register::NONE
	Register segment_prefix = Register::NONE;

	/// @brief Base register or Register::NONE
	Register base = Register::NONE;

	/// @brief Index register or Register::NONE
	Register index = Register::NONE;

	/// @brief Index register scale (1, 2, 4, or 8)
	uint32_t scale = 1;

	/// @brief Memory displacement
	int64_t displacement = 0;

	/// @brief 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in an i8), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	uint32_t displ_size = 0;

	/// @brief true if it's broadcast memory (EVEX instructions)
	bool is_broadcast = false;

	/// @brief Default constructor
	constexpr MemoryOperand() noexcept = default;

	/// @brief Full constructor
	/// @param base Base register or Register::NONE
	/// @param index Index register or Register::NONE
	/// @param scale Index register scale (1, 2, 4, or 8)
	/// @param displacement Memory displacement
	/// @param displ_size 0 (no displ), 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	/// @param is_broadcast true if it's broadcast memory (EVEX instructions)
	/// @param segment_prefix Segment override or Register::NONE
	constexpr MemoryOperand(
		Register base,
		Register index,
		uint32_t scale,
		int64_t displacement,
		uint32_t displ_size,
		bool is_broadcast,
		Register segment_prefix
	) noexcept
		: segment_prefix(segment_prefix)
		, base(base)
		, index(index)
		, scale(scale)
		, displacement(displacement)
		, displ_size(displ_size)
		, is_broadcast(is_broadcast)
	{}

	/// @brief Create memory operand with base and index registers and scale
	/// @param base Base register or Register::NONE
	/// @param index Index register or Register::NONE
	/// @param scale Index register scale (1, 2, 4, or 8)
	[[nodiscard]] static constexpr MemoryOperand with_base_index_scale(Register base, Register index, uint32_t scale) noexcept {
		return MemoryOperand(base, index, scale, 0, 0, false, Register::NONE);
	}

	/// @brief Create memory operand with base and index registers
	/// @param base Base register or Register::NONE
	/// @param index Index register or Register::NONE
	[[nodiscard]] static constexpr MemoryOperand with_base_index(Register base, Register index) noexcept {
		return MemoryOperand(base, index, 1, 0, 0, false, Register::NONE);
	}

	/// @brief Create memory operand with base register and displacement
	/// @param base Base register or Register::NONE
	/// @param displacement Memory displacement
	/// @param displ_size 0 (no displ), 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	[[nodiscard]] static constexpr MemoryOperand with_base_displ_size(Register base, int64_t displacement, uint32_t displ_size) noexcept {
		return MemoryOperand(base, Register::NONE, 1, displacement, displ_size, false, Register::NONE);
	}

	/// @brief Create memory operand with base register and displacement (auto-detect displacement size)
	/// @param base Base register or Register::NONE
	/// @param displacement Memory displacement
	[[nodiscard]] static constexpr MemoryOperand with_base_displ(Register base, int64_t displacement) noexcept {
		return MemoryOperand(base, Register::NONE, 1, displacement, 1, false, Register::NONE);
	}

	/// @brief Create memory operand with base register only
	/// @param base Base register
	[[nodiscard]] static constexpr MemoryOperand with_base(Register base) noexcept {
		return MemoryOperand(base, Register::NONE, 1, 0, 0, false, Register::NONE);
	}

	/// @brief Create memory operand with displacement only
	/// @param displacement Memory displacement
	/// @param displ_size 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	[[nodiscard]] static constexpr MemoryOperand with_displ(uint64_t displacement, uint32_t displ_size) noexcept {
		return MemoryOperand(Register::NONE, Register::NONE, 1, static_cast<int64_t>(displacement), displ_size, false, Register::NONE);
	}

	/// @brief Create memory operand with base, index, scale, displacement, and displacement size
	/// @param base Base register or Register::NONE
	/// @param index Index register or Register::NONE
	/// @param scale Index register scale (1, 2, 4, or 8)
	/// @param displacement Memory displacement
	/// @param displ_size 0 (no displ), 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	[[nodiscard]] static constexpr MemoryOperand with_base_index_scale_displ_size(
		Register base, Register index, uint32_t scale, int64_t displacement, uint32_t displ_size
	) noexcept {
		return MemoryOperand(base, index, scale, displacement, displ_size, false, Register::NONE);
	}

	/// @brief Create memory operand with index, scale, displacement, and displacement size
	/// @param index Index register or Register::NONE
	/// @param scale Index register scale (1, 2, 4, or 8)
	/// @param displacement Memory displacement
	/// @param displ_size 0 (no displ), 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	[[nodiscard]] static constexpr MemoryOperand with_index_scale_displ_size(
		Register index, uint32_t scale, int64_t displacement, uint32_t displ_size
	) noexcept {
		return MemoryOperand(Register::NONE, index, scale, displacement, displ_size, false, Register::NONE);
	}
};

} // namespace iced_x86

#endif // ICED_X86_MEMORY_OPERAND_HPP
