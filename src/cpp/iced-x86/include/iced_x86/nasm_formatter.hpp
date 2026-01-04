// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_NASM_FORMATTER_HPP
#define ICED_X86_NASM_FORMATTER_HPP

#include "formatter_options.hpp"
#include "formatter_output.hpp"
#include "formatter_text_kind.hpp"
#include "symbol_resolver.hpp"
#include "instruction.hpp"
#include "register.hpp"
#include "op_kind.hpp"
#include "mnemonic.hpp"
#include "internal/formatter_regs.hpp"
#include "internal/formatter_mnemonics.hpp"
#include "internal/formatter_memory_size.hpp"
#include <string>
#include <string_view>
#include <cstdint>
#include <format>
#include <optional>

namespace iced_x86 {

/// @brief NASM (Netwide Assembler) formatter
/// 
/// Formats instructions using NASM syntax (destination, source order).
/// Uses bare keywords for memory size (e.g., "dword" instead of "dword ptr").
/// Example: @c mov dword [ebx+ecx*4+10h], eax
class NasmFormatter {
public:
  /// @brief Creates a new NASM formatter with default options
  NasmFormatter() = default;

  /// @brief Creates a new NASM formatter with the specified options
  /// @param options Formatter options
  explicit NasmFormatter( const FormatterOptions& options ) : options_( options ) {}

  /// @brief Creates a new NASM formatter with a symbol resolver
  /// @param symbol_resolver Symbol resolver (can be nullptr)
  explicit NasmFormatter( SymbolResolver* symbol_resolver )
      : symbol_resolver_( symbol_resolver ) {}

  /// @brief Creates a new NASM formatter with options and symbol resolver
  /// @param options Formatter options
  /// @param symbol_resolver Symbol resolver (can be nullptr)
  NasmFormatter( const FormatterOptions& options, SymbolResolver* symbol_resolver )
      : options_( options ), symbol_resolver_( symbol_resolver ) {}

  /// @brief Gets the formatter options
  /// @return Formatter options (mutable)
  FormatterOptions& options() noexcept { return options_; }

  /// @brief Gets the formatter options
  /// @return Formatter options (const)
  const FormatterOptions& options() const noexcept { return options_; }

  /// @brief Gets the symbol resolver
  /// @return Symbol resolver or nullptr
  [[nodiscard]] SymbolResolver* symbol_resolver() const noexcept { return symbol_resolver_; }

  /// @brief Sets the symbol resolver
  /// @param resolver Symbol resolver (can be nullptr)
  void set_symbol_resolver( SymbolResolver* resolver ) noexcept { symbol_resolver_ = resolver; }

  /// @brief Formats the instruction
  /// @param instruction Instruction to format
  /// @param output Output to write to
  void format( const Instruction& instruction, FormatterOutput& output );

  /// @brief Formats the instruction to a string
  /// @param instruction Instruction to format
  /// @return Formatted string
  std::string format_to_string( const Instruction& instruction );

  /// @brief Formats a register
  /// @param reg Register
  /// @return Register name
  std::string_view format_register( Register reg ) const noexcept;

private:
  void format_mnemonic( const Instruction& instruction, FormatterOutput& output );
  void format_operands( const Instruction& instruction, FormatterOutput& output );
  void format_operand( const Instruction& instruction, uint32_t operand, FormatterOutput& output );
  void format_register_operand( const Instruction& instruction, uint32_t operand, Register reg,
                                FormatterOutput& output );
  void format_immediate( const Instruction& instruction, uint32_t operand, FormatterOutput& output );
  void format_near_branch( const Instruction& instruction, uint32_t operand, FormatterOutput& output );
  void format_far_branch( const Instruction& instruction, uint32_t operand, FormatterOutput& output );
  void format_memory( const Instruction& instruction, uint32_t operand, FormatterOutput& output );
  void format_evex_decorators( const Instruction& instruction, uint32_t operand, FormatterOutput& output );

  void format_number( uint64_t value, FormatterOutput& output );
  void format_signed_number( int64_t value, FormatterOutput& output );
  void write_symbol( const Instruction& instruction, FormatterOutput& output,
                     uint64_t address, const SymbolResult& symbol, bool write_minus_if_signed = true );

  std::string_view get_mnemonic( Mnemonic mnemonic ) const;
  std::string_view get_memory_size_string( const Instruction& instruction ) const;

  FormatterOptions options_;
  SymbolResolver* symbol_resolver_ = nullptr;
  std::string number_buffer_;  // Reusable buffer for number formatting
};

// ============================================================================
// Implementation
// ============================================================================

inline std::string_view NasmFormatter::format_register( Register reg ) const noexcept {
  bool uppercase = options_.uppercase_registers() || options_.uppercase_all();
  return internal::get_register_name( static_cast<uint32_t>( reg ), uppercase );
}

inline std::string NasmFormatter::format_to_string( const Instruction& instruction ) {
  std::string result;
  StringFormatterOutput output( result );
  format( instruction, output );
  return result;
}

inline void NasmFormatter::format( const Instruction& instruction, FormatterOutput& output ) {
  // Format prefixes
  if ( instruction.has_lock_prefix() ) {
    output.write_prefix( instruction, options_.uppercase_prefixes() ? "LOCK " : "lock " );
  }
  if ( instruction.has_rep_prefix() ) {
    output.write_prefix( instruction, options_.uppercase_prefixes() ? "REP " : "rep " );
  }
  if ( instruction.has_repne_prefix() ) {
    output.write_prefix( instruction, options_.uppercase_prefixes() ? "REPNE " : "repne " );
  }

  // Format mnemonic
  format_mnemonic( instruction, output );

  // Format operands
  uint32_t op_count = instruction.op_count();
  if ( op_count > 0 ) {
    output.write( " ", FormatterTextKind::TEXT );
    format_operands( instruction, output );
  }
}

inline void NasmFormatter::format_mnemonic( const Instruction& instruction, FormatterOutput& output ) {
  std::string_view mnemonic = get_mnemonic( instruction.mnemonic() );
  output.write_mnemonic( instruction, mnemonic );
}

inline void NasmFormatter::format_operands( const Instruction& instruction, FormatterOutput& output ) {
  uint32_t op_count = instruction.op_count();
  for ( uint32_t i = 0; i < op_count; ++i ) {
    if ( i > 0 ) {
      output.write( options_.space_after_operand_separator() ? ", " : ",", FormatterTextKind::PUNCTUATION );
    }
    format_operand( instruction, i, output );
  }
}

inline void NasmFormatter::format_operand( const Instruction& instruction, uint32_t operand,
                                            FormatterOutput& output ) {
  OpKind kind = instruction.op_kind( operand );

  switch ( kind ) {
    case OpKind::REGISTER:
      format_register_operand( instruction, operand, instruction.op_register( operand ), output );
      if ( operand == 0 ) {
        format_evex_decorators( instruction, operand, output );
      }
      break;

    case OpKind::NEAR_BRANCH16:
    case OpKind::NEAR_BRANCH32:
    case OpKind::NEAR_BRANCH64:
      format_near_branch( instruction, operand, output );
      break;

    case OpKind::FAR_BRANCH16:
    case OpKind::FAR_BRANCH32:
      format_far_branch( instruction, operand, output );
      break;

    case OpKind::IMMEDIATE8:
    case OpKind::IMMEDIATE16:
    case OpKind::IMMEDIATE32:
    case OpKind::IMMEDIATE64:
    case OpKind::IMMEDIATE8TO16:
    case OpKind::IMMEDIATE8TO32:
    case OpKind::IMMEDIATE8TO64:
    case OpKind::IMMEDIATE32TO64:
    case OpKind::IMMEDIATE8_2ND:
      format_immediate( instruction, operand, output );
      break;

    case OpKind::MEMORY:
    case OpKind::MEMORY_SEG_SI:
    case OpKind::MEMORY_SEG_ESI:
    case OpKind::MEMORY_SEG_RSI:
    case OpKind::MEMORY_SEG_DI:
    case OpKind::MEMORY_SEG_EDI:
    case OpKind::MEMORY_SEG_RDI:
    case OpKind::MEMORY_ESDI:
    case OpKind::MEMORY_ESEDI:
    case OpKind::MEMORY_ESRDI:
      format_memory( instruction, operand, output );
      if ( operand == 0 ) {
        format_evex_decorators( instruction, operand, output );
      }
      break;

    default:
      output.write( "???", FormatterTextKind::TEXT );
      break;
  }
}

inline void NasmFormatter::format_register_operand( const Instruction& instruction, uint32_t operand,
                                                     Register reg, FormatterOutput& output ) {
  std::string_view name = format_register( reg );
  output.write_register( instruction, operand, name, reg );
}

inline void NasmFormatter::format_immediate( const Instruction& instruction, uint32_t operand,
                                              FormatterOutput& output ) {
  OpKind kind = instruction.op_kind( operand );
  uint64_t value = 0;

  switch ( kind ) {
    case OpKind::IMMEDIATE8:
      value = instruction.immediate8();
      break;
    case OpKind::IMMEDIATE16:
      value = instruction.immediate16();
      break;
    case OpKind::IMMEDIATE32:
      value = instruction.immediate32();
      break;
    case OpKind::IMMEDIATE64:
      value = instruction.immediate64();
      break;
    case OpKind::IMMEDIATE8TO16:
      value = static_cast<uint16_t>( static_cast<int16_t>( static_cast<int8_t>( instruction.immediate8() ) ) );
      break;
    case OpKind::IMMEDIATE8TO32:
      value = static_cast<uint32_t>( static_cast<int32_t>( static_cast<int8_t>( instruction.immediate8() ) ) );
      break;
    case OpKind::IMMEDIATE8TO64:
      value = static_cast<uint64_t>( static_cast<int64_t>( static_cast<int8_t>( instruction.immediate8() ) ) );
      break;
    case OpKind::IMMEDIATE32TO64:
      value = static_cast<uint64_t>( static_cast<int64_t>( static_cast<int32_t>( instruction.immediate32() ) ) );
      break;
    case OpKind::IMMEDIATE8_2ND:
      value = instruction.immediate8_2nd();
      break;
    default:
      break;
  }

  format_number( value, output );
}

inline void NasmFormatter::format_near_branch( const Instruction& instruction, uint32_t operand,
                                                FormatterOutput& output ) {
  OpKind kind = instruction.op_kind( operand );
  uint64_t target = 0;
  int addr_size = 4;

  switch ( kind ) {
    case OpKind::NEAR_BRANCH16:
      target = instruction.near_branch16();
      addr_size = 2;
      break;
    case OpKind::NEAR_BRANCH32:
      target = instruction.near_branch32();
      addr_size = 4;
      break;
    case OpKind::NEAR_BRANCH64:
      target = instruction.near_branch64();
      addr_size = 8;
      break;
    default:
      break;
  }

  // Try symbol resolution
  if ( symbol_resolver_ ) {
    auto sym = symbol_resolver_->try_get_symbol( instruction, operand, operand, target, addr_size );
    if ( sym ) {
      write_symbol( instruction, output, target, *sym );
      return;
    }
  }

  format_number( target, output );
}

inline void NasmFormatter::format_far_branch( const Instruction& instruction, uint32_t operand,
                                               FormatterOutput& output ) {
  (void)operand;
  uint16_t selector = instruction.far_branch_selector();
  uint32_t offset = 0;

  OpKind kind = instruction.op_kind( 0 );
  if ( kind == OpKind::FAR_BRANCH16 ) {
    offset = instruction.far_branch16();
  } else {
    offset = instruction.far_branch32();
  }

  // Format as selector:offset
  format_number( selector, output );
  output.write( ":", FormatterTextKind::PUNCTUATION );
  format_number( offset, output );
}

inline void NasmFormatter::format_memory( const Instruction& instruction, uint32_t operand,
                                           FormatterOutput& output ) {
  OpKind kind = instruction.op_kind( operand );

  // Handle string instruction memory operands
  if ( kind == OpKind::MEMORY_ESDI || kind == OpKind::MEMORY_ESEDI || kind == OpKind::MEMORY_ESRDI ) {
    // NASM: [es:di] etc.
    bool uppercase = options_.uppercase_registers() || options_.uppercase_all();
    if ( kind == OpKind::MEMORY_ESDI ) {
      output.write( uppercase ? "[ES:DI]" : "[es:di]", FormatterTextKind::TEXT );
    } else if ( kind == OpKind::MEMORY_ESEDI ) {
      output.write( uppercase ? "[ES:EDI]" : "[es:edi]", FormatterTextKind::TEXT );
    } else {
      output.write( uppercase ? "[ES:RDI]" : "[es:rdi]", FormatterTextKind::TEXT );
    }
    return;
  }

  if ( kind == OpKind::MEMORY_SEG_SI || kind == OpKind::MEMORY_SEG_ESI || kind == OpKind::MEMORY_SEG_RSI ) {
    // NASM: [seg:si] etc.
    bool uppercase = options_.uppercase_registers() || options_.uppercase_all();
    Register seg = instruction.memory_segment();
    output.write( "[", FormatterTextKind::PUNCTUATION );
    output.write( format_register( seg ), FormatterTextKind::REGISTER );
    output.write( ":", FormatterTextKind::PUNCTUATION );
    if ( kind == OpKind::MEMORY_SEG_SI ) {
      output.write( uppercase ? "SI" : "si", FormatterTextKind::REGISTER );
    } else if ( kind == OpKind::MEMORY_SEG_ESI ) {
      output.write( uppercase ? "ESI" : "esi", FormatterTextKind::REGISTER );
    } else {
      output.write( uppercase ? "RSI" : "rsi", FormatterTextKind::REGISTER );
    }
    output.write( "]", FormatterTextKind::PUNCTUATION );
    return;
  }

  if ( kind == OpKind::MEMORY_SEG_DI || kind == OpKind::MEMORY_SEG_EDI || kind == OpKind::MEMORY_SEG_RDI ) {
    // NASM: [seg:di] etc.
    bool uppercase = options_.uppercase_registers() || options_.uppercase_all();
    Register seg = instruction.memory_segment();
    output.write( "[", FormatterTextKind::PUNCTUATION );
    output.write( format_register( seg ), FormatterTextKind::REGISTER );
    output.write( ":", FormatterTextKind::PUNCTUATION );
    if ( kind == OpKind::MEMORY_SEG_DI ) {
      output.write( uppercase ? "DI" : "di", FormatterTextKind::REGISTER );
    } else if ( kind == OpKind::MEMORY_SEG_EDI ) {
      output.write( uppercase ? "EDI" : "edi", FormatterTextKind::REGISTER );
    } else {
      output.write( uppercase ? "RDI" : "rdi", FormatterTextKind::REGISTER );
    }
    output.write( "]", FormatterTextKind::PUNCTUATION );
    return;
  }

  // NASM: Memory size prefix comes BEFORE brackets (e.g., "dword [...]")
  if ( options_.show_memory_size() ) {
    std::string_view size_str = get_memory_size_string( instruction );
    if ( !size_str.empty() ) {
      output.write_keyword( instruction, size_str );
      output.write( " ", FormatterTextKind::TEXT );
    }
  }

  // Memory brackets
  output.write( options_.space_after_memory_bracket() ? "[ " : "[", FormatterTextKind::PUNCTUATION );

  // Segment prefix inside brackets for NASM
  Register seg = instruction.memory_segment();
  Register base = instruction.memory_base();
  bool show_segment = options_.always_show_segment_register();
  
  // Show segment if it's not the default segment for this base register
  if ( !show_segment ) {
    Register default_seg = Register::DS;
    if ( base == Register::BP || base == Register::EBP || base == Register::RBP ||
         base == Register::SP || base == Register::ESP || base == Register::RSP ) {
      default_seg = Register::SS;
    }
    show_segment = ( seg != default_seg );
  }

  if ( show_segment && seg != Register::NONE ) {
    output.write( format_register( seg ), FormatterTextKind::REGISTER );
    output.write( ":", FormatterTextKind::PUNCTUATION );
  }

  bool need_plus = false;

  // Base register
  if ( base != Register::NONE ) {
    output.write_register( instruction, operand, format_register( base ), base );
    need_plus = true;
  }

  // Index register
  Register index = instruction.memory_index();
  if ( index != Register::NONE ) {
    if ( need_plus ) {
      output.write( options_.space_between_memory_add_operators() ? " + " : "+",
                    FormatterTextKind::OPERATOR );
    }
    output.write_register( instruction, operand, format_register( index ), index );

    // Scale
    uint32_t scale = instruction.memory_index_scale();
    if ( scale > 1 || options_.always_show_scale() ) {
      output.write( "*", FormatterTextKind::OPERATOR );
      format_number( scale, output );
    }
    need_plus = true;
  }

  // Displacement
  uint64_t disp = instruction.memory_displacement64();
  if ( disp != 0 || ( base == Register::NONE && index == Register::NONE ) ) {
    if ( need_plus ) {
      // Check if displacement is negative (sign-extended)
      int64_t signed_disp = static_cast<int64_t>( disp );
      if ( signed_disp < 0 && base != Register::NONE ) {
        output.write( options_.space_between_memory_add_operators() ? " - " : "-",
                      FormatterTextKind::OPERATOR );
        format_number( static_cast<uint64_t>( -signed_disp ), output );
      } else {
        output.write( options_.space_between_memory_add_operators() ? " + " : "+",
                      FormatterTextKind::OPERATOR );
        format_number( disp, output );
      }
    } else {
      format_number( disp, output );
    }
  }

  output.write( options_.space_after_memory_bracket() ? " ]" : "]", FormatterTextKind::PUNCTUATION );
}

inline void NasmFormatter::format_number( uint64_t value, FormatterOutput& output ) {
  bool uppercase = options_.uppercase_hex();

  // Handle small numbers in decimal
  if ( options_.small_hex_numbers_in_decimal() && value <= 9 ) {
    number_buffer_ = std::to_string( value );
    output.write( number_buffer_, FormatterTextKind::NUMBER );
    return;
  }

  // Format as hex
  std::string_view prefix = options_.hex_prefix();
  std::string_view suffix = options_.hex_suffix();

  number_buffer_.clear();
  number_buffer_ += prefix;

  // Add leading zero if needed
  if ( options_.add_leading_zero_to_hex_numbers() && prefix.empty() ) {
    char first_digit = uppercase ? std::format( "{:X}", value )[0] : std::format( "{:x}", value )[0];
    if ( first_digit >= 'A' && first_digit <= 'F' ) {
      number_buffer_ += '0';
    } else if ( first_digit >= 'a' && first_digit <= 'f' ) {
      number_buffer_ += '0';
    }
  }

  if ( uppercase ) {
    number_buffer_ += std::format( "{:X}", value );
  } else {
    number_buffer_ += std::format( "{:x}", value );
  }

  number_buffer_ += suffix;
  output.write( number_buffer_, FormatterTextKind::NUMBER );
}

inline void NasmFormatter::format_signed_number( int64_t value, FormatterOutput& output ) {
  if ( value < 0 ) {
    output.write( "-", FormatterTextKind::OPERATOR );
    format_number( static_cast<uint64_t>( -value ), output );
  } else {
    format_number( static_cast<uint64_t>( value ), output );
  }
}

inline void NasmFormatter::write_symbol( const Instruction& instruction, FormatterOutput& output,
                                          uint64_t address, const SymbolResult& symbol,
                                          bool write_minus_if_signed ) {
  (void)instruction;
  (void)address;
  (void)write_minus_if_signed;

  // Write the symbol text
  const TextInfo& text = symbol.text;
  if ( text.has_parts() ) {
    // Multiple text parts
    for ( const auto& part : text.parts ) {
      output.write( part.text, part.kind );
    }
  } else {
    // Single text part
    output.write( text.text.text, text.text.kind );
  }
}

inline void NasmFormatter::format_evex_decorators( const Instruction& instruction, uint32_t operand,
                                                    FormatterOutput& output ) {
  (void)operand;

  // Format opmask register {k1}-{k7}
  Register opmask = instruction.op_mask();
  if ( opmask != Register::NONE ) {
    output.write( "{", FormatterTextKind::PUNCTUATION );
    std::string_view mask_name = format_register( opmask );
    output.write( mask_name, FormatterTextKind::REGISTER );
    output.write( "}", FormatterTextKind::PUNCTUATION );
  }

  // Format zeroing-masking {z}
  if ( instruction.zeroing_masking() && opmask != Register::NONE ) {
    output.write( "{", FormatterTextKind::PUNCTUATION );
    bool uppercase = options_.uppercase_decorators() || options_.uppercase_all();
    output.write( uppercase ? "Z" : "z", FormatterTextKind::DECORATOR );
    output.write( "}", FormatterTextKind::PUNCTUATION );
  }
}

inline std::string_view NasmFormatter::get_mnemonic( Mnemonic mnemonic ) const {
  bool uppercase = options_.uppercase_mnemonics() || options_.uppercase_all();
  return internal::get_mnemonic_string( mnemonic, uppercase );
}

inline std::string_view NasmFormatter::get_memory_size_string( const Instruction& instruction ) const {
  bool uppercase = options_.uppercase_keywords() || options_.uppercase_all();
  MemorySize mem_size = instruction.memory_size();
  // Use NASM-style memory size strings (no "ptr")
  return internal::get_nasm_memory_size_string( mem_size, uppercase );
}

} // namespace iced_x86

#endif // ICED_X86_NASM_FORMATTER_HPP
