// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_FAST_FORMATTER_OPTIONS_HPP
#define ICED_X86_FAST_FORMATTER_OPTIONS_HPP

#include <cstdint>

namespace iced_x86 {

/// @brief Fast formatter options
///
/// These options control how the fast formatter formats instructions.
/// The fast formatter is optimized for speed and has fewer options than
/// the other formatters.
class FastFormatterOptions {
public:
  /// @brief Creates default options
  ///
  /// Default values:
  /// - space_after_operand_separator: false
  /// - rip_relative_addresses: false
  /// - use_pseudo_ops: true
  /// - show_symbol_address: false
  /// - always_show_segment_register: false
  /// - always_show_memory_size: false
  /// - uppercase_hex: true
  /// - use_hex_prefix: false
  constexpr FastFormatterOptions() noexcept
      : flags_( Flags::USE_PSEUDO_OPS | Flags::UPPERCASE_HEX ) {}

  /// @brief Add a space after the operand separator
  ///
  /// Default: false
  ///
  /// true: `mov rax, rcx`
  /// false: `mov rax,rcx`
  [[nodiscard]] constexpr bool space_after_operand_separator() const noexcept {
    return ( flags_ & Flags::SPACE_AFTER_OPERAND_SEPARATOR ) != 0;
  }
  constexpr void set_space_after_operand_separator( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::SPACE_AFTER_OPERAND_SEPARATOR;
    else
      flags_ &= ~Flags::SPACE_AFTER_OPERAND_SEPARATOR;
  }

  /// @brief Show RIP+displ or the virtual address
  ///
  /// Default: false
  ///
  /// true: `mov eax,[rip+12345678h]`
  /// false: `mov eax,[1029384756AFBECDh]`
  [[nodiscard]] constexpr bool rip_relative_addresses() const noexcept {
    return ( flags_ & Flags::RIP_RELATIVE_ADDRESSES ) != 0;
  }
  constexpr void set_rip_relative_addresses( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::RIP_RELATIVE_ADDRESSES;
    else
      flags_ &= ~Flags::RIP_RELATIVE_ADDRESSES;
  }

  /// @brief Use pseudo instructions
  ///
  /// Default: true
  ///
  /// true: `vcmpnltsd xmm2,xmm6,xmm3`
  /// false: `vcmpsd xmm2,xmm6,xmm3,5`
  [[nodiscard]] constexpr bool use_pseudo_ops() const noexcept {
    return ( flags_ & Flags::USE_PSEUDO_OPS ) != 0;
  }
  constexpr void set_use_pseudo_ops( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::USE_PSEUDO_OPS;
    else
      flags_ &= ~Flags::USE_PSEUDO_OPS;
  }

  /// @brief Show the original value after the symbol name
  ///
  /// Default: false
  ///
  /// true: `mov eax,[myfield (12345678)]`
  /// false: `mov eax,[myfield]`
  [[nodiscard]] constexpr bool show_symbol_address() const noexcept {
    return ( flags_ & Flags::SHOW_SYMBOL_ADDRESS ) != 0;
  }
  constexpr void set_show_symbol_address( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::SHOW_SYMBOL_ADDRESS;
    else
      flags_ &= ~Flags::SHOW_SYMBOL_ADDRESS;
  }

  /// @brief Always show the effective segment register
  ///
  /// Default: false
  ///
  /// true: `mov eax,ds:[ecx]`
  /// false: `mov eax,[ecx]`
  [[nodiscard]] constexpr bool always_show_segment_register() const noexcept {
    return ( flags_ & Flags::ALWAYS_SHOW_SEGMENT_REGISTER ) != 0;
  }
  constexpr void set_always_show_segment_register( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::ALWAYS_SHOW_SEGMENT_REGISTER;
    else
      flags_ &= ~Flags::ALWAYS_SHOW_SEGMENT_REGISTER;
  }

  /// @brief Always show memory operands' size
  ///
  /// Default: false
  ///
  /// true: `mov eax,dword ptr [ebx]`
  /// false: `mov eax,[ebx]`
  [[nodiscard]] constexpr bool always_show_memory_size() const noexcept {
    return ( flags_ & Flags::ALWAYS_SHOW_MEMORY_SIZE ) != 0;
  }
  constexpr void set_always_show_memory_size( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::ALWAYS_SHOW_MEMORY_SIZE;
    else
      flags_ &= ~Flags::ALWAYS_SHOW_MEMORY_SIZE;
  }

  /// @brief Use uppercase hex digits
  ///
  /// Default: true
  ///
  /// true: `0xFF`
  /// false: `0xff`
  [[nodiscard]] constexpr bool uppercase_hex() const noexcept {
    return ( flags_ & Flags::UPPERCASE_HEX ) != 0;
  }
  constexpr void set_uppercase_hex( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::UPPERCASE_HEX;
    else
      flags_ &= ~Flags::UPPERCASE_HEX;
  }

  /// @brief Use a hex prefix (0x) or a hex suffix (h)
  ///
  /// Default: false
  ///
  /// true: `0x5A`
  /// false: `5Ah`
  [[nodiscard]] constexpr bool use_hex_prefix() const noexcept {
    return ( flags_ & Flags::USE_HEX_PREFIX ) != 0;
  }
  constexpr void set_use_hex_prefix( bool value ) noexcept {
    if ( value )
      flags_ |= Flags::USE_HEX_PREFIX;
    else
      flags_ &= ~Flags::USE_HEX_PREFIX;
  }

private:
  enum Flags : uint8_t {
    SPACE_AFTER_OPERAND_SEPARATOR = 0x01,
    RIP_RELATIVE_ADDRESSES = 0x02,
    USE_PSEUDO_OPS = 0x04,
    SHOW_SYMBOL_ADDRESS = 0x08,
    ALWAYS_SHOW_SEGMENT_REGISTER = 0x10,
    ALWAYS_SHOW_MEMORY_SIZE = 0x20,
    UPPERCASE_HEX = 0x40,
    USE_HEX_PREFIX = 0x80,
  };

  uint8_t flags_;
};

} // namespace iced_x86

#endif // ICED_X86_FAST_FORMATTER_OPTIONS_HPP
