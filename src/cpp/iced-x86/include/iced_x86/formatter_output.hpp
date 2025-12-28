// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_FORMATTER_OUTPUT_HPP
#define ICED_X86_FORMATTER_OUTPUT_HPP

#include "formatter_text_kind.hpp"
#include "register.hpp"
#include <string>
#include <string_view>

namespace iced_x86 {

// Forward declaration
struct Instruction;

/// @brief Used by a Formatter to write all text
/// 
/// The main method to override is write(). All other methods have default
/// implementations that call write().
class FormatterOutput {
public:
  virtual ~FormatterOutput() = default;

  /// @brief Writes text with the specified kind
  /// @param text Text to write
  /// @param kind Kind of text
  virtual void write( std::string_view text, FormatterTextKind kind ) = 0;

  /// @brief Writes a prefix
  /// @param instruction Instruction
  /// @param text Prefix text
  virtual void write_prefix( const Instruction& instruction, std::string_view text ) {
    (void)instruction;
    write( text, FormatterTextKind::PREFIX );
  }

  /// @brief Writes a mnemonic
  /// @param instruction Instruction  
  /// @param text Mnemonic text
  virtual void write_mnemonic( const Instruction& instruction, std::string_view text ) {
    (void)instruction;
    write( text, FormatterTextKind::MNEMONIC );
  }

  /// @brief Writes a number
  /// @param instruction Instruction
  /// @param operand Operand number (0-based)
  /// @param text Number text
  /// @param value Value
  /// @param kind Text kind
  virtual void write_number( const Instruction& instruction, uint32_t operand, std::string_view text,
                             uint64_t value, FormatterTextKind kind ) {
    (void)instruction;
    (void)operand;
    (void)value;
    write( text, kind );
  }

  /// @brief Writes a register
  /// @param instruction Instruction
  /// @param operand Operand number (0-based)
  /// @param text Register text
  /// @param reg Register value
  virtual void write_register( const Instruction& instruction, uint32_t operand, std::string_view text,
                               Register reg ) {
    (void)instruction;
    (void)operand;
    (void)reg;
    write( text, FormatterTextKind::REGISTER );
  }

  /// @brief Writes a decorator (eg. {z}, {sae})
  /// @param instruction Instruction
  /// @param operand Operand number (0-based)
  /// @param text Decorator text
  virtual void write_decorator( const Instruction& instruction, uint32_t operand, std::string_view text ) {
    (void)instruction;
    (void)operand;
    write( text, FormatterTextKind::DECORATOR );
  }

  /// @brief Writes a keyword (eg. byte ptr)
  /// @param instruction Instruction
  /// @param text Keyword text
  virtual void write_keyword( const Instruction& instruction, std::string_view text ) {
    (void)instruction;
    write( text, FormatterTextKind::KEYWORD );
  }
};

/// @brief Simple formatter output that writes to a string
class StringFormatterOutput : public FormatterOutput {
public:
  explicit StringFormatterOutput( std::string& output ) : output_( output ) {}

  void write( std::string_view text, FormatterTextKind kind ) override {
    (void)kind;
    output_ += text;
  }

  /// @brief Clears the output string
  void clear() { output_.clear(); }

  /// @brief Gets the output string
  const std::string& str() const noexcept { return output_; }

private:
  std::string& output_;
};

} // namespace iced_x86

#endif // ICED_X86_FORMATTER_OUTPUT_HPP
