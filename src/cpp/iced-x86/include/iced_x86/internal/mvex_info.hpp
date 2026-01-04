// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_INTERNAL_MVEX_INFO_HPP
#define ICED_X86_INTERNAL_MVEX_INFO_HPP

#include "iced_x86/mvex_tuple_type_lut_kind.hpp"
#include "iced_x86/mvex_eh_bit.hpp"
#include "iced_x86/mvex_conv_fn.hpp"
#include "iced_x86/tuple_type.hpp"
#include "iced_x86/code.hpp"
#include "iced_x86/internal/mvex_info_flags1.hpp"
#include "iced_x86/internal/mvex_info_flags2.hpp"

#include <cstdint>
#include <array>

// MVEX constants (avoid including iced_constants.hpp which has complex dependencies)
namespace iced_x86::internal {
	inline constexpr uint32_t MVEX_START = 4611U;
	inline constexpr uint32_t MVEX_LENGTH = 207U;
}

namespace iced_x86 {
namespace internal {

/// @brief MVEX instruction info
struct MvexInfo {
	MvexTupleTypeLutKind tuple_type_lut_kind;
	MvexEHBit eh_bit;
	MvexConvFn conv_fn;
	uint8_t invalid_conv_fns;
	uint8_t invalid_swizzle_fns;
	uint8_t flags1;
	uint8_t flags2;
	uint8_t pad;

	/// @brief Checks if eviction hint can be used
	[[nodiscard]] constexpr bool can_use_eviction_hint() const noexcept {
		return (flags1 & MvexInfoFlags1::EVICTION_HINT) != 0;
	}

	/// @brief Checks if rounding control can be used
	[[nodiscard]] constexpr bool can_use_rounding_control() const noexcept {
		return (flags1 & MvexInfoFlags1::ROUNDING_CONTROL) != 0;
	}

	/// @brief Checks if suppress-all-exceptions can be used
	[[nodiscard]] constexpr bool can_use_suppress_all_exceptions() const noexcept {
		return (flags1 & MvexInfoFlags1::SUPPRESS_ALL_EXCEPTIONS) != 0;
	}

	/// @brief Checks if the instruction ignores the opmask register
	[[nodiscard]] constexpr bool ignores_op_mask_register() const noexcept {
		return (flags1 & MvexInfoFlags1::IGNORES_OP_MASK_REGISTER) != 0;
	}

	/// @brief Checks if the instruction requires an opmask register
	[[nodiscard]] constexpr bool require_op_mask_register() const noexcept {
		return (flags1 & MvexInfoFlags1::REQUIRE_OP_MASK_REGISTER) != 0;
	}

	/// @brief Checks if no SAE/rounding control is allowed
	[[nodiscard]] constexpr bool no_sae_rc() const noexcept {
		return (flags2 & MvexInfoFlags2::NO_SAE_ROUNDING_CONTROL) != 0;
	}

	/// @brief Checks if eviction hint is ignored
	[[nodiscard]] constexpr bool ignores_eviction_hint() const noexcept {
		return (flags2 & MvexInfoFlags2::IGNORES_EVICTION_HINT) != 0;
	}
};

static_assert(sizeof(MvexInfo) == 8, "MvexInfo size mismatch");

// MVEX tuple type lookup table (14 kinds * 8 SSS values = 112 entries)
// Index: tuple_type_lut_kind * 8 + sss
inline constexpr std::array<TupleType, 112> MVEX_TUPLE_TYPE_LUT = {{
	// MvexTupleTypeLutKind::INT32
	TupleType::N64, TupleType::N4, TupleType::N16, TupleType::N32,
	TupleType::N16, TupleType::N16, TupleType::N32, TupleType::N32,
	// MvexTupleTypeLutKind::INT32_HALF
	TupleType::N32, TupleType::N4, TupleType::N16, TupleType::N16,
	TupleType::N8, TupleType::N8, TupleType::N16, TupleType::N16,
	// MvexTupleTypeLutKind::INT32_4TO16
	TupleType::N16, TupleType::N1, TupleType::N1, TupleType::N8,
	TupleType::N4, TupleType::N4, TupleType::N8, TupleType::N8,
	// MvexTupleTypeLutKind::INT32_1TO16_OR_ELEM
	TupleType::N4, TupleType::N1, TupleType::N1, TupleType::N2,
	TupleType::N1, TupleType::N1, TupleType::N2, TupleType::N2,
	// MvexTupleTypeLutKind::INT64
	TupleType::N64, TupleType::N8, TupleType::N32, TupleType::N16,
	TupleType::N8, TupleType::N8, TupleType::N16, TupleType::N16,
	// MvexTupleTypeLutKind::INT64_4TO8
	TupleType::N32, TupleType::N1, TupleType::N1, TupleType::N8,
	TupleType::N4, TupleType::N4, TupleType::N8, TupleType::N8,
	// MvexTupleTypeLutKind::INT64_1TO8_OR_ELEM
	TupleType::N8, TupleType::N1, TupleType::N1, TupleType::N2,
	TupleType::N1, TupleType::N1, TupleType::N2, TupleType::N2,
	// MvexTupleTypeLutKind::FLOAT32
	TupleType::N64, TupleType::N4, TupleType::N16, TupleType::N32,
	TupleType::N16, TupleType::N16, TupleType::N32, TupleType::N32,
	// MvexTupleTypeLutKind::FLOAT32_HALF
	TupleType::N32, TupleType::N4, TupleType::N16, TupleType::N16,
	TupleType::N8, TupleType::N8, TupleType::N16, TupleType::N16,
	// MvexTupleTypeLutKind::FLOAT32_4TO16
	TupleType::N16, TupleType::N1, TupleType::N1, TupleType::N8,
	TupleType::N4, TupleType::N4, TupleType::N8, TupleType::N8,
	// MvexTupleTypeLutKind::FLOAT32_1TO16_OR_ELEM
	TupleType::N4, TupleType::N1, TupleType::N1, TupleType::N2,
	TupleType::N1, TupleType::N1, TupleType::N2, TupleType::N2,
	// MvexTupleTypeLutKind::FLOAT64
	TupleType::N64, TupleType::N8, TupleType::N32, TupleType::N16,
	TupleType::N8, TupleType::N8, TupleType::N16, TupleType::N16,
	// MvexTupleTypeLutKind::FLOAT64_4TO8
	TupleType::N32, TupleType::N1, TupleType::N1, TupleType::N8,
	TupleType::N4, TupleType::N4, TupleType::N8, TupleType::N8,
	// MvexTupleTypeLutKind::FLOAT64_1TO8_OR_ELEM
	TupleType::N8, TupleType::N1, TupleType::N1, TupleType::N2,
	TupleType::N1, TupleType::N1, TupleType::N2, TupleType::N2,
}};

// Helper to get disp8n scale factor from TupleType
inline constexpr uint32_t get_disp8n(TupleType tuple_type) noexcept {
	// TupleType::N1=0, N2=1, N4=2, N8=3, N16=4, N32=5, N64=6
	// Scale = 1 << tuple_type for simple cases
	switch (tuple_type) {
		case TupleType::N1: return 1;
		case TupleType::N2: return 2;
		case TupleType::N4: return 4;
		case TupleType::N8: return 8;
		case TupleType::N16: return 16;
		case TupleType::N32: return 32;
		case TupleType::N64: return 64;
		default: return 1;  // MVEX doesn't use broadcast types
	}
}

// MVEX_INFO table - 207 entries for MVEX instructions
// This is a large table, defined in mvex_info_data.cpp
extern const std::array<MvexInfo, MVEX_LENGTH> MVEX_INFO;

/// @brief Gets MVEX info for an instruction code
/// @param code Instruction code (must be an MVEX instruction)
/// @return Reference to MvexInfo
inline const MvexInfo& get_mvex_info(Code code) noexcept {
	auto idx = static_cast<uint32_t>(code) - MVEX_START;
	return MVEX_INFO[idx];
}

} // namespace internal
} // namespace iced_x86

#endif // ICED_X86_INTERNAL_MVEX_INFO_HPP
