// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_INTERNAL_HANDLERS_TABLE_HPP
#define ICED_X86_INTERNAL_HANDLERS_TABLE_HPP

#include "handlers.hpp"
#include <array>

namespace iced_x86 {
namespace internal {

// ============================================================================
// Helper to create HandlerEntry from a handler struct
// Note: Uses reinterpret_cast which is not constexpr-compatible, so handlers
// using this must be declared with constinit instead of constexpr
// ============================================================================

template<typename T>
inline HandlerEntry make_handler_entry( const T* handler ) noexcept {
	return HandlerEntry{ T::decode, reinterpret_cast<const OpCodeHandler*>( handler ) };
}

// ============================================================================
// Static/constexpr handler instances
// These are the actual handler objects that the tables point to
// ============================================================================

// Invalid handlers (pre-defined)
inline constexpr OpCodeHandler_Invalid handler_null{ true };
inline constexpr OpCodeHandler_Invalid handler_invalid{ true };
inline constexpr OpCodeHandler_Invalid handler_invalid_no_modrm{ false };

// Helper to get invalid handler entries
// Note: These are not constexpr because make_handler_entry uses reinterpret_cast
inline HandlerEntry null_handler_entry() noexcept {
	return make_handler_entry( &handler_null );
}

inline HandlerEntry invalid_handler_entry() noexcept {
	return make_handler_entry( &handler_invalid );
}

inline HandlerEntry invalid_no_modrm_handler_entry() noexcept {
	return make_handler_entry( &handler_invalid_no_modrm );
}

// ============================================================================
// Forward declaration of generated handler tables
// These will be defined in generated headers
// ============================================================================

// Legacy (x86) handlers table - 256 entries for map0
extern const std::array<HandlerEntry, 256>& get_legacy_map0_handlers() noexcept;

// VEX handlers tables
extern const std::array<HandlerEntry, 256>& get_vex_0f_handlers() noexcept;
extern const std::array<HandlerEntry, 256>& get_vex_0f38_handlers() noexcept;
extern const std::array<HandlerEntry, 256>& get_vex_0f3a_handlers() noexcept;

// EVEX handlers tables
extern const std::array<HandlerEntry, 256>& get_evex_0f_handlers() noexcept;
extern const std::array<HandlerEntry, 256>& get_evex_0f38_handlers() noexcept;
extern const std::array<HandlerEntry, 256>& get_evex_0f3a_handlers() noexcept;
extern const std::array<HandlerEntry, 256>& get_evex_map5_handlers() noexcept;
extern const std::array<HandlerEntry, 256>& get_evex_map6_handlers() noexcept;

} // namespace internal
} // namespace iced_x86

#endif // ICED_X86_INTERNAL_HANDLERS_TABLE_HPP
