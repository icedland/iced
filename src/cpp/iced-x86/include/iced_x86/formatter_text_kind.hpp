// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_FORMATTER_TEXT_KIND_HPP
#define ICED_X86_FORMATTER_TEXT_KIND_HPP

#include <cstdint>

namespace iced_x86 {

/// @brief Text kind for formatter output
enum class FormatterTextKind : uint8_t {
  /// @brief Normal text
  TEXT = 0,
  /// @brief Assembler directive
  DIRECTIVE = 1,
  /// @brief Any prefix
  PREFIX = 2,
  /// @brief Any mnemonic
  MNEMONIC = 3,
  /// @brief Any keyword
  KEYWORD = 4,
  /// @brief Any operator
  OPERATOR = 5,
  /// @brief Any punctuation
  PUNCTUATION = 6,
  /// @brief Number
  NUMBER = 7,
  /// @brief Any register
  REGISTER = 8,
  /// @brief A decorator, eg. @c {z}, @c {sae}
  DECORATOR = 9,
  /// @brief Selector value (eg. far call/jmp)
  SELECTOR_VALUE = 10,
  /// @brief Label address (eg. near call/jmp target)
  LABEL_ADDRESS = 11,
  /// @brief Function address (eg. @c call func)
  FUNCTION_ADDRESS = 12,
  /// @brief Data symbol
  DATA = 13,
  /// @brief Label symbol
  LABEL = 14,
  /// @brief Function symbol
  FUNCTION = 15,
};

} // namespace iced_x86

#endif // ICED_X86_FORMATTER_TEXT_KIND_HPP
