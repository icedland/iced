// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_FORMATTER_OPTIONS_HPP
#define ICED_X86_FORMATTER_OPTIONS_HPP

#include <cstdint>
#include <string>
#include <string_view>

namespace iced_x86 {

/// @brief Formatter options
class FormatterOptions {
public:
  FormatterOptions() = default;

  /// @brief Use uppercase hex digits
  /// @details Default: @c true
  bool uppercase_hex() const noexcept { return uppercase_hex_; }
  void set_uppercase_hex( bool value ) noexcept { uppercase_hex_ = value; }

  /// @brief Use uppercase prefixes (@c REP, @c LOCK, etc.)
  /// @details Default: @c false
  bool uppercase_prefixes() const noexcept { return uppercase_prefixes_; }
  void set_uppercase_prefixes( bool value ) noexcept { uppercase_prefixes_ = value; }

  /// @brief Use uppercase mnemonics (@c MOV, @c ADD, etc.)
  /// @details Default: @c false
  bool uppercase_mnemonics() const noexcept { return uppercase_mnemonics_; }
  void set_uppercase_mnemonics( bool value ) noexcept { uppercase_mnemonics_ = value; }

  /// @brief Use uppercase registers (@c EAX, @c RAX, etc.)
  /// @details Default: @c false
  bool uppercase_registers() const noexcept { return uppercase_registers_; }
  void set_uppercase_registers( bool value ) noexcept { uppercase_registers_ = value; }

  /// @brief Use uppercase keywords (@c BYTE PTR, @c DWORD PTR, etc.)
  /// @details Default: @c false
  bool uppercase_keywords() const noexcept { return uppercase_keywords_; }
  void set_uppercase_keywords( bool value ) noexcept { uppercase_keywords_ = value; }

  /// @brief Use uppercase decorators (@c {Z}, @c {SAE}, etc.)
  /// @details Default: @c false
  bool uppercase_decorators() const noexcept { return uppercase_decorators_; }
  void set_uppercase_decorators( bool value ) noexcept { uppercase_decorators_ = value; }

  /// @brief Use uppercase everything
  /// @details Default: @c false
  bool uppercase_all() const noexcept { return uppercase_all_; }
  void set_uppercase_all( bool value ) noexcept { uppercase_all_ = value; }

  /// @brief Digit separator or empty string to not use one
  /// @details Default: empty string
  std::string_view digit_separator() const noexcept { return digit_separator_; }
  void set_digit_separator( std::string_view value ) { digit_separator_ = value; }

  /// @brief Hex prefix or empty string
  /// @details Default: empty string
  std::string_view hex_prefix() const noexcept { return hex_prefix_; }
  void set_hex_prefix( std::string_view value ) { hex_prefix_ = value; }

  /// @brief Hex suffix or empty string
  /// @details Default: "h"
  std::string_view hex_suffix() const noexcept { return hex_suffix_; }
  void set_hex_suffix( std::string_view value ) { hex_suffix_ = value; }

  /// @brief Size of a digit group. 0 to not group digits.
  /// @details Default: 4
  uint8_t hex_digit_group_size() const noexcept { return hex_digit_group_size_; }
  void set_hex_digit_group_size( uint8_t value ) noexcept { hex_digit_group_size_ = value; }

  /// @brief Decimal prefix or empty string
  /// @details Default: empty string
  std::string_view decimal_prefix() const noexcept { return decimal_prefix_; }
  void set_decimal_prefix( std::string_view value ) { decimal_prefix_ = value; }

  /// @brief Decimal suffix or empty string
  /// @details Default: empty string
  std::string_view decimal_suffix() const noexcept { return decimal_suffix_; }
  void set_decimal_suffix( std::string_view value ) { decimal_suffix_ = value; }

  /// @brief Size of a digit group. 0 to not group digits.
  /// @details Default: 3
  uint8_t decimal_digit_group_size() const noexcept { return decimal_digit_group_size_; }
  void set_decimal_digit_group_size( uint8_t value ) noexcept { decimal_digit_group_size_ = value; }

  /// @brief Octal prefix or empty string
  /// @details Default: empty string
  std::string_view octal_prefix() const noexcept { return octal_prefix_; }
  void set_octal_prefix( std::string_view value ) { octal_prefix_ = value; }

  /// @brief Octal suffix or empty string
  /// @details Default: "o"
  std::string_view octal_suffix() const noexcept { return octal_suffix_; }
  void set_octal_suffix( std::string_view value ) { octal_suffix_ = value; }

  /// @brief Size of a digit group. 0 to not group digits.
  /// @details Default: 4
  uint8_t octal_digit_group_size() const noexcept { return octal_digit_group_size_; }
  void set_octal_digit_group_size( uint8_t value ) noexcept { octal_digit_group_size_ = value; }

  /// @brief Binary prefix or empty string
  /// @details Default: empty string
  std::string_view binary_prefix() const noexcept { return binary_prefix_; }
  void set_binary_prefix( std::string_view value ) { binary_prefix_ = value; }

  /// @brief Binary suffix or empty string
  /// @details Default: "b"
  std::string_view binary_suffix() const noexcept { return binary_suffix_; }
  void set_binary_suffix( std::string_view value ) { binary_suffix_ = value; }

  /// @brief Size of a digit group. 0 to not group digits.
  /// @details Default: 4
  uint8_t binary_digit_group_size() const noexcept { return binary_digit_group_size_; }
  void set_binary_digit_group_size( uint8_t value ) noexcept { binary_digit_group_size_ = value; }

  /// @brief Add leading zeros to displacements
  /// @details Default: @c false
  bool leading_zeros() const noexcept { return leading_zeros_; }
  void set_leading_zeros( bool value ) noexcept { leading_zeros_ = value; }

  /// @brief Small hex numbers in decimal (0-9)
  /// @details Default: @c true
  bool small_hex_numbers_in_decimal() const noexcept { return small_hex_numbers_in_decimal_; }
  void set_small_hex_numbers_in_decimal( bool value ) noexcept { small_hex_numbers_in_decimal_ = value; }

  /// @brief Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits A-F
  /// @details Default: @c true
  bool add_leading_zero_to_hex_numbers() const noexcept { return add_leading_zero_to_hex_numbers_; }
  void set_add_leading_zero_to_hex_numbers( bool value ) noexcept { add_leading_zero_to_hex_numbers_ = value; }

  /// @brief Use @c st(0) instead of @c st for FPU instructions
  /// @details Default: @c false
  bool always_show_scale() const noexcept { return always_show_scale_; }
  void set_always_show_scale( bool value ) noexcept { always_show_scale_ = value; }

  /// @brief Always show the segment register prefix
  /// @details Default: @c false
  bool always_show_segment_register() const noexcept { return always_show_segment_register_; }
  void set_always_show_segment_register( bool value ) noexcept { always_show_segment_register_ = value; }

  /// @brief Show memory size (eg. @c dword ptr)
  /// @details Default: @c true for Intel/Masm, @c false for Nasm/Gas
  bool show_memory_size() const noexcept { return show_memory_size_; }
  void set_show_memory_size( bool value ) noexcept { show_memory_size_ = value; }

  /// @brief Add a space after the operand separator
  /// @details Default: @c true
  bool space_after_operand_separator() const noexcept { return space_after_operand_separator_; }
  void set_space_after_operand_separator( bool value ) noexcept { space_after_operand_separator_ = value; }

  /// @brief Add a space between the memory bracket and the expression
  /// @details Default: @c false
  bool space_after_memory_bracket() const noexcept { return space_after_memory_bracket_; }
  void set_space_after_memory_bracket( bool value ) noexcept { space_after_memory_bracket_ = value; }

  /// @brief Add a space between the memory bracket and the expression
  /// @details Default: @c false
  bool space_between_memory_add_operators() const noexcept { return space_between_memory_add_operators_; }
  void set_space_between_memory_add_operators( bool value ) noexcept { space_between_memory_add_operators_ = value; }

  /// @brief Use RIP-relative addresses
  /// @details Default: @c true for Intel/Masm/Nasm, @c false for Gas
  bool rip_relative_addresses() const noexcept { return rip_relative_addresses_; }
  void set_rip_relative_addresses( bool value ) noexcept { rip_relative_addresses_ = value; }

  /// @brief Show branch size (eg. @c short, @c near ptr)
  /// @details Default: @c true
  bool show_branch_size() const noexcept { return show_branch_size_; }
  void set_show_branch_size( bool value ) noexcept { show_branch_size_ = value; }

  /// @brief Use pseudo instructions (eg. vcmpps instead of vcmpeqps)
  /// @details Default: @c true
  bool use_pseudo_ops() const noexcept { return use_pseudo_ops_; }
  void set_use_pseudo_ops( bool value ) noexcept { use_pseudo_ops_ = value; }

  /// @brief Show symbol addresses
  /// @details Default: @c false
  bool show_symbol_address() const noexcept { return show_symbol_address_; }
  void set_show_symbol_address( bool value ) noexcept { show_symbol_address_ = value; }

private:
  bool uppercase_hex_ = true;
  bool uppercase_prefixes_ = false;
  bool uppercase_mnemonics_ = false;
  bool uppercase_registers_ = false;
  bool uppercase_keywords_ = false;
  bool uppercase_decorators_ = false;
  bool uppercase_all_ = false;
  bool leading_zeros_ = false;
  bool small_hex_numbers_in_decimal_ = true;
  bool add_leading_zero_to_hex_numbers_ = true;
  bool always_show_scale_ = false;
  bool always_show_segment_register_ = false;
  bool show_memory_size_ = true;
  bool space_after_operand_separator_ = true;
  bool space_after_memory_bracket_ = false;
  bool space_between_memory_add_operators_ = false;
  bool rip_relative_addresses_ = true;
  bool show_branch_size_ = true;
  bool use_pseudo_ops_ = true;
  bool show_symbol_address_ = false;

  std::string digit_separator_;
  std::string hex_prefix_;
  std::string hex_suffix_ = "h";
  std::string decimal_prefix_;
  std::string decimal_suffix_;
  std::string octal_prefix_;
  std::string octal_suffix_ = "o";
  std::string binary_prefix_;
  std::string binary_suffix_ = "b";

  uint8_t hex_digit_group_size_ = 4;
  uint8_t decimal_digit_group_size_ = 3;
  uint8_t octal_digit_group_size_ = 4;
  uint8_t binary_digit_group_size_ = 4;
};

} // namespace iced_x86

#endif // ICED_X86_FORMATTER_OPTIONS_HPP
