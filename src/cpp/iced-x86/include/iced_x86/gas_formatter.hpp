// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_GAS_FORMATTER_HPP
#define ICED_X86_GAS_FORMATTER_HPP

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

/// @brief GAS (GNU Assembler) formatter - AT&T syntax
/// 
/// Formats instructions using AT&T/GAS syntax:
/// - Operands are reversed (source, destination order)
/// - Registers have % prefix (%eax, %rbx)
/// - Immediates have $ prefix ($10, $0x1234)
/// - Memory uses parenthetical syntax: disp(%base,%index,scale)
/// Example: @c movl $10, %eax  or  movl 0x10(%ebx,%ecx,4), %eax
class GasFormatter {
public:
  /// @brief Creates a new GAS formatter with default options
  GasFormatter() = default;

  /// @brief Creates a new GAS formatter with the specified options
  /// @param options Formatter options
  explicit GasFormatter( const FormatterOptions& options ) : options_( options ) {}

  /// @brief Creates a new GAS formatter with a symbol resolver
  /// @param symbol_resolver Symbol resolver (can be nullptr)
  explicit GasFormatter( SymbolResolver* symbol_resolver )
      : symbol_resolver_( symbol_resolver ) {}

  /// @brief Creates a new GAS formatter with options and symbol resolver
  /// @param options Formatter options
  /// @param symbol_resolver Symbol resolver (can be nullptr)
  GasFormatter( const FormatterOptions& options, SymbolResolver* symbol_resolver )
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

  /// @brief Formats a register with % prefix
  /// @param reg Register
  /// @return Register name with % prefix
  std::string format_register( Register reg ) const;

  /// @brief Gets if naked registers are used (no % prefix)
  /// @return True if naked registers enabled
  bool naked_registers() const noexcept { return naked_registers_; }

  /// @brief Sets if naked registers are used (no % prefix)
  /// @param value True to enable naked registers
  void set_naked_registers( bool value ) noexcept { naked_registers_ = value; }

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
  char get_size_suffix( const Instruction& instruction ) const;

  FormatterOptions options_;
  SymbolResolver* symbol_resolver_ = nullptr;
  std::string number_buffer_;  // Reusable buffer for number formatting
  std::string register_buffer_;  // Reusable buffer for register formatting
  bool naked_registers_ = false;  // If true, don't use % prefix on registers
};

// ============================================================================
// Implementation
// ============================================================================

inline std::string GasFormatter::format_register( Register reg ) const {
  bool uppercase = options_.uppercase_registers() || options_.uppercase_all();
  std::string_view name = internal::get_register_name( static_cast<uint32_t>( reg ), uppercase );
  if ( naked_registers_ ) {
    return std::string( name );
  }
  return std::string( "%" ) + std::string( name );
}

inline std::string GasFormatter::format_to_string( const Instruction& instruction ) {
  std::string result;
  StringFormatterOutput output( result );
  format( instruction, output );
  return result;
}

inline void GasFormatter::format( const Instruction& instruction, FormatterOutput& output ) {
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

  // Format mnemonic (potentially with size suffix)
  format_mnemonic( instruction, output );

  // Format operands (in reversed order for AT&T syntax)
  uint32_t op_count = instruction.op_count();
  if ( op_count > 0 ) {
    output.write( " ", FormatterTextKind::TEXT );
    format_operands( instruction, output );
  }
}

inline void GasFormatter::format_mnemonic( const Instruction& instruction, FormatterOutput& output ) {
  std::string_view base_mnemonic = get_mnemonic( instruction.mnemonic() );
  
  // For AT&T syntax, we may want to add a size suffix (b, w, l, q)
  // This is optional and controlled by options
  char suffix = get_size_suffix( instruction );
  
  if ( suffix != 0 && options_.show_memory_size() ) {
    // Append size suffix to mnemonic
    std::string mnemonic_with_suffix( base_mnemonic );
    mnemonic_with_suffix += suffix;
    output.write_mnemonic( instruction, mnemonic_with_suffix );
  } else {
    output.write_mnemonic( instruction, base_mnemonic );
  }
}

inline void GasFormatter::format_operands( const Instruction& instruction, FormatterOutput& output ) {
  uint32_t op_count = instruction.op_count();
  
  // AT&T syntax: operands are reversed (source first, destination last)
  // For 2 operands: op1, op0
  // For 3 operands: op2, op1, op0
  // For 4 operands: op3, op2, op1, op0
  
  bool first = true;
  for ( int32_t i = static_cast<int32_t>( op_count ) - 1; i >= 0; --i ) {
    if ( !first ) {
      output.write( options_.space_after_operand_separator() ? ", " : ",", FormatterTextKind::PUNCTUATION );
    }
    first = false;
    format_operand( instruction, static_cast<uint32_t>( i ), output );
  }
}

inline void GasFormatter::format_operand( const Instruction& instruction, uint32_t operand,
                                           FormatterOutput& output ) {
  OpKind kind = instruction.op_kind( operand );

  switch ( kind ) {
    case OpKind::REGISTER:
      format_register_operand( instruction, operand, instruction.op_register( operand ), output );
      // GAS uses AT&T syntax with reversed operands, so operand 0 is last (destination)
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

inline void GasFormatter::format_register_operand( const Instruction& instruction, uint32_t operand,
                                                    Register reg, FormatterOutput& output ) {
  std::string name = format_register( reg );
  output.write_register( instruction, operand, name, reg );
}

inline void GasFormatter::format_immediate( const Instruction& instruction, uint32_t operand,
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

  // AT&T syntax: immediates have $ prefix
  output.write( "$", FormatterTextKind::OPERATOR );
  format_number( value, output );
}

inline void GasFormatter::format_near_branch( const Instruction& instruction, uint32_t operand,
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

inline void GasFormatter::format_far_branch( const Instruction& instruction, uint32_t operand,
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

  // AT&T syntax for far branch: $selector, $offset
  output.write( "$", FormatterTextKind::OPERATOR );
  format_number( selector, output );
  output.write( ", ", FormatterTextKind::PUNCTUATION );
  output.write( "$", FormatterTextKind::OPERATOR );
  format_number( offset, output );
}

inline void GasFormatter::format_memory( const Instruction& instruction, uint32_t operand,
                                          FormatterOutput& output ) {
  OpKind kind = instruction.op_kind( operand );

  // Handle string instruction memory operands
  if ( kind == OpKind::MEMORY_ESDI || kind == OpKind::MEMORY_ESEDI || kind == OpKind::MEMORY_ESRDI ) {
    // AT&T: %es:(%di) etc.
    std::string es_reg = format_register( Register::ES );
    output.write( es_reg, FormatterTextKind::REGISTER );
    output.write( ":", FormatterTextKind::PUNCTUATION );
    output.write( "(", FormatterTextKind::PUNCTUATION );
    if ( kind == OpKind::MEMORY_ESDI ) {
      output.write( format_register( Register::DI ), FormatterTextKind::REGISTER );
    } else if ( kind == OpKind::MEMORY_ESEDI ) {
      output.write( format_register( Register::EDI ), FormatterTextKind::REGISTER );
    } else {
      output.write( format_register( Register::RDI ), FormatterTextKind::REGISTER );
    }
    output.write( ")", FormatterTextKind::PUNCTUATION );
    return;
  }

  if ( kind == OpKind::MEMORY_SEG_SI || kind == OpKind::MEMORY_SEG_ESI || kind == OpKind::MEMORY_SEG_RSI ) {
    // AT&T: %seg:(%si) etc.
    Register seg = instruction.memory_segment();
    output.write( format_register( seg ), FormatterTextKind::REGISTER );
    output.write( ":", FormatterTextKind::PUNCTUATION );
    output.write( "(", FormatterTextKind::PUNCTUATION );
    if ( kind == OpKind::MEMORY_SEG_SI ) {
      output.write( format_register( Register::SI ), FormatterTextKind::REGISTER );
    } else if ( kind == OpKind::MEMORY_SEG_ESI ) {
      output.write( format_register( Register::ESI ), FormatterTextKind::REGISTER );
    } else {
      output.write( format_register( Register::RSI ), FormatterTextKind::REGISTER );
    }
    output.write( ")", FormatterTextKind::PUNCTUATION );
    return;
  }

  if ( kind == OpKind::MEMORY_SEG_DI || kind == OpKind::MEMORY_SEG_EDI || kind == OpKind::MEMORY_SEG_RDI ) {
    // AT&T: %seg:(%di) etc.
    Register seg = instruction.memory_segment();
    output.write( format_register( seg ), FormatterTextKind::REGISTER );
    output.write( ":", FormatterTextKind::PUNCTUATION );
    output.write( "(", FormatterTextKind::PUNCTUATION );
    if ( kind == OpKind::MEMORY_SEG_DI ) {
      output.write( format_register( Register::DI ), FormatterTextKind::REGISTER );
    } else if ( kind == OpKind::MEMORY_SEG_EDI ) {
      output.write( format_register( Register::EDI ), FormatterTextKind::REGISTER );
    } else {
      output.write( format_register( Register::RDI ), FormatterTextKind::REGISTER );
    }
    output.write( ")", FormatterTextKind::PUNCTUATION );
    return;
  }

  // AT&T memory syntax: seg:disp(base,index,scale)
  // Examples:
  //   (%rax)           - base only
  //   0x10(%rax)       - base + disp
  //   (%rax,%rbx)      - base + index
  //   (%rax,%rbx,4)    - base + index*scale
  //   0x10(%rax,%rbx,4) - base + index*scale + disp
  //   0x1234           - displacement only
  //   %fs:(%rax)       - segment override
  
  Register seg = instruction.memory_segment();
  Register base = instruction.memory_base();
  Register index = instruction.memory_index();
  uint32_t scale = instruction.memory_index_scale();
  uint64_t disp = instruction.memory_displacement64();
  
  // Segment override
  bool show_segment = options_.always_show_segment_register();
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

  // Displacement (comes before parentheses in AT&T)
  bool has_base_or_index = ( base != Register::NONE || index != Register::NONE );
  if ( disp != 0 || !has_base_or_index ) {
    format_number( disp, output );
  }

  // Base and index in parentheses
  if ( has_base_or_index ) {
    output.write( "(", FormatterTextKind::PUNCTUATION );
    
    if ( base != Register::NONE ) {
      output.write( format_register( base ), FormatterTextKind::REGISTER );
    }
    
    if ( index != Register::NONE ) {
      output.write( ",", FormatterTextKind::PUNCTUATION );
      output.write( format_register( index ), FormatterTextKind::REGISTER );
      
      // Scale (always shown for index in AT&T, even if 1)
      if ( scale > 1 || options_.always_show_scale() ) {
        output.write( ",", FormatterTextKind::PUNCTUATION );
        format_number( scale, output );
      }
    }
    
    output.write( ")", FormatterTextKind::PUNCTUATION );
  }
}

inline void GasFormatter::format_number( uint64_t value, FormatterOutput& output ) {
  bool uppercase = options_.uppercase_hex();

  // Handle small numbers in decimal
  if ( options_.small_hex_numbers_in_decimal() && value <= 9 ) {
    number_buffer_ = std::to_string( value );
    output.write( number_buffer_, FormatterTextKind::NUMBER );
    return;
  }

  // Format as hex with 0x prefix (standard for GAS)
  number_buffer_.clear();
  number_buffer_ += "0x";

  if ( uppercase ) {
    number_buffer_ += std::format( "{:X}", value );
  } else {
    number_buffer_ += std::format( "{:x}", value );
  }

  output.write( number_buffer_, FormatterTextKind::NUMBER );
}

inline void GasFormatter::format_signed_number( int64_t value, FormatterOutput& output ) {
  if ( value < 0 ) {
    output.write( "-", FormatterTextKind::OPERATOR );
    format_number( static_cast<uint64_t>( -value ), output );
  } else {
    format_number( static_cast<uint64_t>( value ), output );
  }
}

inline void GasFormatter::write_symbol( const Instruction& instruction, FormatterOutput& output,
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

inline void GasFormatter::format_evex_decorators( const Instruction& instruction, uint32_t operand,
                                                   FormatterOutput& output ) {
  (void)operand;

  // Format opmask register {k1}-{k7}
  // Note: GAS/AT&T syntax also uses {} for EVEX decorators
  Register opmask = instruction.op_mask();
  if ( opmask != Register::NONE ) {
    output.write( "{", FormatterTextKind::PUNCTUATION );
    std::string mask_name = format_register( opmask );
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

inline std::string_view GasFormatter::get_mnemonic( Mnemonic mnemonic ) const {
  bool uppercase = options_.uppercase_mnemonics() || options_.uppercase_all();
  return internal::get_mnemonic_string( mnemonic, uppercase );
}

inline char GasFormatter::get_size_suffix( const Instruction& instruction ) const {
  // Determine size suffix based on operand size
  // b = byte (8-bit)
  // w = word (16-bit)
  // l = long/dword (32-bit)
  // q = quad (64-bit)
  
  MemorySize mem_size = instruction.memory_size();
  
  switch ( mem_size ) {
    case MemorySize::UINT8:
    case MemorySize::INT8:
      return options_.uppercase_mnemonics() ? 'B' : 'b';
    case MemorySize::UINT16:
    case MemorySize::INT16:
      return options_.uppercase_mnemonics() ? 'W' : 'w';
    case MemorySize::UINT32:
    case MemorySize::INT32:
    case MemorySize::FLOAT32:
      return options_.uppercase_mnemonics() ? 'L' : 'l';
    case MemorySize::UINT64:
    case MemorySize::INT64:
    case MemorySize::FLOAT64:
      return options_.uppercase_mnemonics() ? 'Q' : 'q';
    default:
      return 0;  // No suffix
  }
}

} // namespace iced_x86

#endif // ICED_X86_GAS_FORMATTER_HPP
