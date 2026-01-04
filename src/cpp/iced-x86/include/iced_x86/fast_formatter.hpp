// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_FAST_FORMATTER_HPP
#define ICED_X86_FAST_FORMATTER_HPP

#include "fast_formatter_options.hpp"
#include "fast_string_output.hpp"
#include "symbol_resolver.hpp"
#include "instruction.hpp"
#include "code.hpp"
#include "code_size.hpp"
#include "register.hpp"
#include "op_kind.hpp"
#include "mnemonic.hpp"
#include "memory_size.hpp"
#include "rounding_control.hpp"
#include "internal/formatter_regs.hpp"
#include "internal/formatter_mnemonics.hpp"
#include "internal/formatter_memory_size.hpp"
#include <string>
#include <string_view>
#include <cstdint>
#include <optional>

namespace iced_x86 {

/// @brief Fast formatter with less formatting options and with a masm-like syntax.
///
/// Use it if formatting speed is more important than being able to re-assemble
/// formatted instructions.
///
/// This formatter is optimized for speed by:
/// - Using simpler code paths
/// - Pre-computed lookup tables
/// - Minimal virtual calls
/// - Direct string operations
///
/// Example:
/// @code
/// FastFormatter formatter;
/// FastStringOutput output;
/// formatter.format(instruction, output);
/// std::cout << output.view() << std::endl;
/// @endcode
class FastFormatter {
public:
  /// @brief Creates a new fast formatter with default options
  FastFormatter() = default;

  /// @brief Creates a new fast formatter with the specified options
  /// @param options Formatter options
  explicit FastFormatter( const FastFormatterOptions& options ) : options_( options ) {}

  /// @brief Creates a new fast formatter with a symbol resolver
  /// @param symbol_resolver Symbol resolver (can be nullptr)
  explicit FastFormatter( SymbolResolver* symbol_resolver )
      : symbol_resolver_( symbol_resolver ) {}

  /// @brief Creates a new fast formatter with options and symbol resolver
  /// @param options Formatter options
  /// @param symbol_resolver Symbol resolver (can be nullptr)
  FastFormatter( const FastFormatterOptions& options, SymbolResolver* symbol_resolver )
      : options_( options ), symbol_resolver_( symbol_resolver ) {}

  /// @brief Gets the formatter options (mutable)
  /// @return Formatter options
  FastFormatterOptions& options() noexcept { return options_; }

  /// @brief Gets the formatter options (const)
  /// @return Formatter options
  [[nodiscard]] const FastFormatterOptions& options() const noexcept { return options_; }

  /// @brief Gets the symbol resolver
  /// @return Symbol resolver or nullptr
  [[nodiscard]] SymbolResolver* symbol_resolver() const noexcept { return symbol_resolver_; }

  /// @brief Sets the symbol resolver
  /// @param resolver Symbol resolver (can be nullptr)
  void set_symbol_resolver( SymbolResolver* resolver ) noexcept { symbol_resolver_ = resolver; }

  /// @brief Formats the whole instruction: prefixes, mnemonic, operands
  /// @param instruction Instruction to format
  /// @param output Output buffer
  void format( const Instruction& instruction, FastStringOutput& output );

  /// @brief Formats the instruction and returns it as a string
  /// @param instruction Instruction to format
  /// @return Formatted instruction string
  [[nodiscard]] std::string format_to_string( const Instruction& instruction );

private:
  // Format helpers
  void format_register( FastStringOutput& output, Register reg );
  void format_number( FastStringOutput& output, uint64_t value );
  void format_memory( FastStringOutput& output, const Instruction& instruction, uint32_t operand,
                      Register seg_reg, Register base_reg, Register index_reg,
                      uint32_t scale, uint32_t displ_size, int64_t displ, uint32_t addr_size );
  void write_symbol( FastStringOutput& output, uint64_t address, const SymbolResult& symbol,
                     bool write_minus_if_signed = true );
  
  [[nodiscard]] std::string_view get_mnemonic( Mnemonic mnemonic ) const;
  [[nodiscard]] std::string_view get_memory_size_string( MemorySize size ) const;
  [[nodiscard]] bool show_segment_prefix( const Instruction& instruction, uint32_t op_count ) const;
  [[nodiscard]] uint32_t get_address_size( Register base_reg, Register index_reg, 
                                            uint32_t displ_size, CodeSize code_size ) const;

  FastFormatterOptions options_;
  SymbolResolver* symbol_resolver_ = nullptr;
};

// ============================================================================
// Implementation
// ============================================================================

inline std::string FastFormatter::format_to_string( const Instruction& instruction ) {
  FastStringOutput output( 64 );  // Pre-allocate reasonable size
  format( instruction, output );
  return std::string( output.view() );
}

inline void FastFormatter::format_register( FastStringOutput& output, Register reg ) {
  output.append( internal::get_register_name( static_cast<uint32_t>( reg ), false ) );
}

inline std::string_view FastFormatter::get_mnemonic( Mnemonic mnemonic ) const {
  return internal::get_mnemonic_string( mnemonic, false );
}

inline std::string_view FastFormatter::get_memory_size_string( MemorySize size ) const {
  return internal::get_memory_size_string( size, false );
}

inline uint32_t FastFormatter::get_address_size( Register base_reg, Register index_reg,
                                                   uint32_t displ_size, CodeSize code_size ) const {
  // Determine address size from registers used
  if ( base_reg != Register::NONE ) {
    auto base_val = static_cast<uint32_t>( base_reg );
    // 16-bit registers: AX-DI (21-28), SP, BP, SI, DI
    if ( base_val >= static_cast<uint32_t>( Register::AX ) && 
         base_val <= static_cast<uint32_t>( Register::DI ) ) {
      return 2;
    }
    // 32-bit registers: EAX-EDI (37-44), EIP
    if ( base_val >= static_cast<uint32_t>( Register::EAX ) && 
         base_val <= static_cast<uint32_t>( Register::R15_D ) ) {
      return 4;
    }
    if ( base_reg == Register::EIP ) {
      return 4;
    }
    // 64-bit registers
    if ( base_val >= static_cast<uint32_t>( Register::RAX ) && 
         base_val <= static_cast<uint32_t>( Register::R15 ) ) {
      return 8;
    }
    if ( base_reg == Register::RIP ) {
      return 8;
    }
  }
  
  if ( index_reg != Register::NONE ) {
    auto index_val = static_cast<uint32_t>( index_reg );
    if ( index_val >= static_cast<uint32_t>( Register::AX ) && 
         index_val <= static_cast<uint32_t>( Register::DI ) ) {
      return 2;
    }
    if ( index_val >= static_cast<uint32_t>( Register::EAX ) && 
         index_val <= static_cast<uint32_t>( Register::R15_D ) ) {
      return 4;
    }
    if ( index_val >= static_cast<uint32_t>( Register::RAX ) && 
         index_val <= static_cast<uint32_t>( Register::R15 ) ) {
      return 8;
    }
  }
  
  // Use code size as fallback
  switch ( code_size ) {
    case CodeSize::CODE16: return 2;
    case CodeSize::CODE32: return 4;
    case CodeSize::CODE64: return 8;
    default: return 4;
  }
}

inline bool FastFormatter::show_segment_prefix( const Instruction& instruction, uint32_t op_count ) const {
  // Check if any operand is a memory operand - if so, don't show segment prefix as separate
  for ( uint32_t i = 0; i < op_count; ++i ) {
    OpKind kind = instruction.op_kind( i );
    switch ( kind ) {
      case OpKind::MEMORY_SEG_SI:
      case OpKind::MEMORY_SEG_ESI:
      case OpKind::MEMORY_SEG_RSI:
      case OpKind::MEMORY_SEG_DI:
      case OpKind::MEMORY_SEG_EDI:
      case OpKind::MEMORY_SEG_RDI:
      case OpKind::MEMORY:
        return false;
      default:
        break;
    }
  }
  return true;  // Show useless prefixes
}

inline void FastFormatter::format_number( FastStringOutput& output, uint64_t value ) {
  // Pre-computed lookup tables for 2-digit hex values (much faster than per-digit)
  static constexpr char hex_upper_2[256][2] = {
    {'0','0'},{'0','1'},{'0','2'},{'0','3'},{'0','4'},{'0','5'},{'0','6'},{'0','7'},
    {'0','8'},{'0','9'},{'0','A'},{'0','B'},{'0','C'},{'0','D'},{'0','E'},{'0','F'},
    {'1','0'},{'1','1'},{'1','2'},{'1','3'},{'1','4'},{'1','5'},{'1','6'},{'1','7'},
    {'1','8'},{'1','9'},{'1','A'},{'1','B'},{'1','C'},{'1','D'},{'1','E'},{'1','F'},
    {'2','0'},{'2','1'},{'2','2'},{'2','3'},{'2','4'},{'2','5'},{'2','6'},{'2','7'},
    {'2','8'},{'2','9'},{'2','A'},{'2','B'},{'2','C'},{'2','D'},{'2','E'},{'2','F'},
    {'3','0'},{'3','1'},{'3','2'},{'3','3'},{'3','4'},{'3','5'},{'3','6'},{'3','7'},
    {'3','8'},{'3','9'},{'3','A'},{'3','B'},{'3','C'},{'3','D'},{'3','E'},{'3','F'},
    {'4','0'},{'4','1'},{'4','2'},{'4','3'},{'4','4'},{'4','5'},{'4','6'},{'4','7'},
    {'4','8'},{'4','9'},{'4','A'},{'4','B'},{'4','C'},{'4','D'},{'4','E'},{'4','F'},
    {'5','0'},{'5','1'},{'5','2'},{'5','3'},{'5','4'},{'5','5'},{'5','6'},{'5','7'},
    {'5','8'},{'5','9'},{'5','A'},{'5','B'},{'5','C'},{'5','D'},{'5','E'},{'5','F'},
    {'6','0'},{'6','1'},{'6','2'},{'6','3'},{'6','4'},{'6','5'},{'6','6'},{'6','7'},
    {'6','8'},{'6','9'},{'6','A'},{'6','B'},{'6','C'},{'6','D'},{'6','E'},{'6','F'},
    {'7','0'},{'7','1'},{'7','2'},{'7','3'},{'7','4'},{'7','5'},{'7','6'},{'7','7'},
    {'7','8'},{'7','9'},{'7','A'},{'7','B'},{'7','C'},{'7','D'},{'7','E'},{'7','F'},
    {'8','0'},{'8','1'},{'8','2'},{'8','3'},{'8','4'},{'8','5'},{'8','6'},{'8','7'},
    {'8','8'},{'8','9'},{'8','A'},{'8','B'},{'8','C'},{'8','D'},{'8','E'},{'8','F'},
    {'9','0'},{'9','1'},{'9','2'},{'9','3'},{'9','4'},{'9','5'},{'9','6'},{'9','7'},
    {'9','8'},{'9','9'},{'9','A'},{'9','B'},{'9','C'},{'9','D'},{'9','E'},{'9','F'},
    {'A','0'},{'A','1'},{'A','2'},{'A','3'},{'A','4'},{'A','5'},{'A','6'},{'A','7'},
    {'A','8'},{'A','9'},{'A','A'},{'A','B'},{'A','C'},{'A','D'},{'A','E'},{'A','F'},
    {'B','0'},{'B','1'},{'B','2'},{'B','3'},{'B','4'},{'B','5'},{'B','6'},{'B','7'},
    {'B','8'},{'B','9'},{'B','A'},{'B','B'},{'B','C'},{'B','D'},{'B','E'},{'B','F'},
    {'C','0'},{'C','1'},{'C','2'},{'C','3'},{'C','4'},{'C','5'},{'C','6'},{'C','7'},
    {'C','8'},{'C','9'},{'C','A'},{'C','B'},{'C','C'},{'C','D'},{'C','E'},{'C','F'},
    {'D','0'},{'D','1'},{'D','2'},{'D','3'},{'D','4'},{'D','5'},{'D','6'},{'D','7'},
    {'D','8'},{'D','9'},{'D','A'},{'D','B'},{'D','C'},{'D','D'},{'D','E'},{'D','F'},
    {'E','0'},{'E','1'},{'E','2'},{'E','3'},{'E','4'},{'E','5'},{'E','6'},{'E','7'},
    {'E','8'},{'E','9'},{'E','A'},{'E','B'},{'E','C'},{'E','D'},{'E','E'},{'E','F'},
    {'F','0'},{'F','1'},{'F','2'},{'F','3'},{'F','4'},{'F','5'},{'F','6'},{'F','7'},
    {'F','8'},{'F','9'},{'F','A'},{'F','B'},{'F','C'},{'F','D'},{'F','E'},{'F','F'}
  };
  static constexpr char hex_lower_2[256][2] = {
    {'0','0'},{'0','1'},{'0','2'},{'0','3'},{'0','4'},{'0','5'},{'0','6'},{'0','7'},
    {'0','8'},{'0','9'},{'0','a'},{'0','b'},{'0','c'},{'0','d'},{'0','e'},{'0','f'},
    {'1','0'},{'1','1'},{'1','2'},{'1','3'},{'1','4'},{'1','5'},{'1','6'},{'1','7'},
    {'1','8'},{'1','9'},{'1','a'},{'1','b'},{'1','c'},{'1','d'},{'1','e'},{'1','f'},
    {'2','0'},{'2','1'},{'2','2'},{'2','3'},{'2','4'},{'2','5'},{'2','6'},{'2','7'},
    {'2','8'},{'2','9'},{'2','a'},{'2','b'},{'2','c'},{'2','d'},{'2','e'},{'2','f'},
    {'3','0'},{'3','1'},{'3','2'},{'3','3'},{'3','4'},{'3','5'},{'3','6'},{'3','7'},
    {'3','8'},{'3','9'},{'3','a'},{'3','b'},{'3','c'},{'3','d'},{'3','e'},{'3','f'},
    {'4','0'},{'4','1'},{'4','2'},{'4','3'},{'4','4'},{'4','5'},{'4','6'},{'4','7'},
    {'4','8'},{'4','9'},{'4','a'},{'4','b'},{'4','c'},{'4','d'},{'4','e'},{'4','f'},
    {'5','0'},{'5','1'},{'5','2'},{'5','3'},{'5','4'},{'5','5'},{'5','6'},{'5','7'},
    {'5','8'},{'5','9'},{'5','a'},{'5','b'},{'5','c'},{'5','d'},{'5','e'},{'5','f'},
    {'6','0'},{'6','1'},{'6','2'},{'6','3'},{'6','4'},{'6','5'},{'6','6'},{'6','7'},
    {'6','8'},{'6','9'},{'6','a'},{'6','b'},{'6','c'},{'6','d'},{'6','e'},{'6','f'},
    {'7','0'},{'7','1'},{'7','2'},{'7','3'},{'7','4'},{'7','5'},{'7','6'},{'7','7'},
    {'7','8'},{'7','9'},{'7','a'},{'7','b'},{'7','c'},{'7','d'},{'7','e'},{'7','f'},
    {'8','0'},{'8','1'},{'8','2'},{'8','3'},{'8','4'},{'8','5'},{'8','6'},{'8','7'},
    {'8','8'},{'8','9'},{'8','a'},{'8','b'},{'8','c'},{'8','d'},{'8','e'},{'8','f'},
    {'9','0'},{'9','1'},{'9','2'},{'9','3'},{'9','4'},{'9','5'},{'9','6'},{'9','7'},
    {'9','8'},{'9','9'},{'9','a'},{'9','b'},{'9','c'},{'9','d'},{'9','e'},{'9','f'},
    {'a','0'},{'a','1'},{'a','2'},{'a','3'},{'a','4'},{'a','5'},{'a','6'},{'a','7'},
    {'a','8'},{'a','9'},{'a','a'},{'a','b'},{'a','c'},{'a','d'},{'a','e'},{'a','f'},
    {'b','0'},{'b','1'},{'b','2'},{'b','3'},{'b','4'},{'b','5'},{'b','6'},{'b','7'},
    {'b','8'},{'b','9'},{'b','a'},{'b','b'},{'b','c'},{'b','d'},{'b','e'},{'b','f'},
    {'c','0'},{'c','1'},{'c','2'},{'c','3'},{'c','4'},{'c','5'},{'c','6'},{'c','7'},
    {'c','8'},{'c','9'},{'c','a'},{'c','b'},{'c','c'},{'c','d'},{'c','e'},{'c','f'},
    {'d','0'},{'d','1'},{'d','2'},{'d','3'},{'d','4'},{'d','5'},{'d','6'},{'d','7'},
    {'d','8'},{'d','9'},{'d','a'},{'d','b'},{'d','c'},{'d','d'},{'d','e'},{'d','f'},
    {'e','0'},{'e','1'},{'e','2'},{'e','3'},{'e','4'},{'e','5'},{'e','6'},{'e','7'},
    {'e','8'},{'e','9'},{'e','a'},{'e','b'},{'e','c'},{'e','d'},{'e','e'},{'e','f'},
    {'f','0'},{'f','1'},{'f','2'},{'f','3'},{'f','4'},{'f','5'},{'f','6'},{'f','7'},
    {'f','8'},{'f','9'},{'f','a'},{'f','b'},{'f','c'},{'f','d'},{'f','e'},{'f','f'}
  };

  const auto (&hex_table)[256][2] = options_.uppercase_hex() ? hex_upper_2 : hex_lower_2;
  bool use_hex_prefix = options_.use_hex_prefix();

  // Build output into local buffer (max 16 hex digits + "0x" + "h" + leading 0)
  char buf[20];
  char* p = buf + 20;

  // Add suffix 'h' first (we're building backwards)
  if ( !use_hex_prefix ) {
    *--p = 'h';
  }

  // Extract bytes and look up 2 hex digits at a time
  if ( value == 0 ) {
    *--p = '0';
  } else {
    // Output bytes from low to high (building string backwards)
    do {
      uint8_t byte = static_cast<uint8_t>( value );
      value >>= 8;
      const char* hex = hex_table[byte];
      *--p = hex[1];
      *--p = hex[0];
    } while ( value != 0 );

    // Skip leading zeros (but keep at least one digit)
    while ( *p == '0' && p[1] != 'h' && p[1] != '\0' ) {
      ++p;
    }
  }

  // Add leading zero if first digit is a letter (for suffix format without 0x prefix)
  if ( !use_hex_prefix && *p >= 'A' ) {
    *--p = '0';
  }

  // Add "0x" prefix
  if ( use_hex_prefix ) {
    *--p = 'x';
    *--p = '0';
  }

  output.append( std::string_view( p, static_cast<std::size_t>( buf + 20 - p ) ) );
}

inline void FastFormatter::format_memory( FastStringOutput& output, const Instruction& instruction,
                                           uint32_t operand, Register seg_reg, Register base_reg,
                                           Register index_reg, uint32_t scale, uint32_t displ_size,
                                           int64_t displ, uint32_t addr_size ) {
  (void)operand;  // Unused in fast formatter
  
  uint64_t abs_addr;
  if ( base_reg == Register::RIP ) {
    abs_addr = static_cast<uint64_t>( displ );
    if ( options_.rip_relative_addresses() ) {
      displ -= static_cast<int64_t>( instruction.next_ip() );
    } else {
      base_reg = Register::NONE;
    }
    displ_size = 8;
  } else if ( base_reg == Register::EIP ) {
    abs_addr = static_cast<uint32_t>( displ );
    if ( options_.rip_relative_addresses() ) {
      displ = static_cast<int32_t>( static_cast<uint32_t>( displ ) - instruction.next_ip32() );
    } else {
      base_reg = Register::NONE;
    }
    displ_size = 4;
  } else {
    abs_addr = static_cast<uint64_t>( displ );
  }

  (void)abs_addr;  // Could be used for symbol resolution

  bool use_scale = scale != 0;
  if ( !use_scale ) {
    // [rsi] = base reg, [rsi*1] = index reg
    if ( base_reg == Register::NONE ) {
      use_scale = true;
    }
  }
  if ( addr_size == 2 ) {
    use_scale = false;
  }

  // Show memory size if needed
  bool show_mem_size = instruction.is_broadcast() || options_.always_show_memory_size();
  if ( show_mem_size ) {
    std::string_view size_str = get_memory_size_string( instruction.memory_size() );
    if ( !size_str.empty() ) {
      output.append( size_str );
    }
  }

  // Segment prefix
  if ( options_.always_show_segment_register() || seg_reg != Register::NONE ) {
    format_register( output, seg_reg );
    output.append( ':' );
  }

  output.append( '[' );

  bool need_plus = false;
  if ( base_reg != Register::NONE ) {
    format_register( output, base_reg );
    need_plus = true;
  }

  if ( index_reg != Register::NONE ) {
    if ( need_plus ) {
      output.append( '+' );
    }
    need_plus = true;
    format_register( output, index_reg );
    if ( use_scale ) {
      static constexpr std::string_view scale_numbers[] = { "*1", "*2", "*4", "*8" };
      output.append( scale_numbers[scale] );
    }
  }

  if ( !need_plus || ( displ_size != 0 && displ != 0 ) ) {
    if ( need_plus ) {
      if ( addr_size == 8 ) {
        if ( displ < 0 ) {
          displ = -displ;
          output.append( '-' );
        } else {
          output.append( '+' );
        }
      } else if ( addr_size == 4 ) {
        if ( static_cast<int32_t>( displ ) < 0 ) {
          displ = static_cast<uint32_t>( -static_cast<int32_t>( displ ) );
          output.append( '-' );
        } else {
          output.append( '+' );
        }
      } else {
        if ( static_cast<int16_t>( displ ) < 0 ) {
          displ = static_cast<uint16_t>( -static_cast<int16_t>( displ ) );
          output.append( '-' );
        } else {
          output.append( '+' );
        }
      }
    }
    format_number( output, static_cast<uint64_t>( displ ) );
  }

  output.append( ']' );
}

inline void FastFormatter::format( const Instruction& instruction, FastStringOutput& output ) {
  Code code = instruction.code();
  Mnemonic mnemonic_val = instruction.mnemonic();
  uint32_t op_count = instruction.op_count();

  // Handle prefixes
  Register prefix_seg = instruction.segment_prefix();
  bool has_any_prefix = ( prefix_seg != Register::NONE ) || 
                        instruction.has_lock_prefix() ||
                        instruction.has_rep_prefix() ||
                        instruction.has_repne_prefix();

  if ( has_any_prefix ) {
    // Segment override shown separately only if not used by memory operand
    if ( prefix_seg != Register::NONE && show_segment_prefix( instruction, op_count ) ) {
      format_register( output, prefix_seg );
      output.append( ' ' );
    }

    if ( instruction.has_lock_prefix() ) {
      output.append( "lock " );
    }
    if ( instruction.has_rep_prefix() ) {
      output.append( "rep " );
    }
    if ( instruction.has_repne_prefix() ) {
      output.append( "repne " );
    }
  }

  // Mnemonic
  std::string_view mnemonic_str = get_mnemonic( mnemonic_val );
  output.append( mnemonic_str );

  // Handle declare data instructions
  bool is_declare_data = false;
  OpKind declare_data_op_kind = OpKind::REGISTER;
  if ( code >= Code::DECLARE_BYTE && code <= Code::DECLARE_QWORD ) {
    op_count = instruction.declare_data_len();
    is_declare_data = true;
    switch ( code ) {
      case Code::DECLARE_BYTE:
        declare_data_op_kind = OpKind::IMMEDIATE8;
        break;
      case Code::DECLARE_WORD:
        declare_data_op_kind = OpKind::IMMEDIATE16;
        break;
      case Code::DECLARE_DWORD:
        declare_data_op_kind = OpKind::IMMEDIATE32;
        break;
      case Code::DECLARE_QWORD:
        declare_data_op_kind = OpKind::IMMEDIATE64;
        break;
      default:
        break;
    }
  }

  if ( op_count > 0 ) {
    output.append( ' ' );

    for ( uint32_t operand = 0; operand < op_count; ++operand ) {
      if ( operand > 0 ) {
        if ( options_.space_after_operand_separator() ) {
          output.append( ", " );
        } else {
          output.append( ',' );
        }
      }

      OpKind op_kind = is_declare_data ? declare_data_op_kind : instruction.op_kind( operand );

      switch ( op_kind ) {
        case OpKind::REGISTER:
          format_register( output, instruction.op_register( operand ) );
          break;

        case OpKind::NEAR_BRANCH16:
        case OpKind::NEAR_BRANCH32:
        case OpKind::NEAR_BRANCH64: {
          uint64_t imm64;
          int imm_size;
          if ( op_kind == OpKind::NEAR_BRANCH64 ) {
            imm_size = 8;
            imm64 = instruction.near_branch64();
          } else if ( op_kind == OpKind::NEAR_BRANCH32 ) {
            imm_size = 4;
            imm64 = instruction.near_branch32();
          } else {
            imm_size = 2;
            imm64 = instruction.near_branch16();
          }
          if ( symbol_resolver_ ) {
            auto sym = symbol_resolver_->try_get_symbol( instruction, operand, operand, imm64, imm_size );
            if ( sym ) {
              write_symbol( output, imm64, *sym );
              break;
            }
          }
          format_number( output, imm64 );
          break;
        }

        case OpKind::FAR_BRANCH16:
          format_number( output, instruction.far_branch_selector() );
          output.append( ':' );
          format_number( output, instruction.far_branch16() );
          break;

        case OpKind::FAR_BRANCH32:
          format_number( output, instruction.far_branch_selector() );
          output.append( ':' );
          format_number( output, instruction.far_branch32() );
          break;

        case OpKind::IMMEDIATE8:
          if ( is_declare_data ) {
            format_number( output, instruction.get_declare_byte_value( operand ) );
          } else {
            format_number( output, instruction.immediate8() );
          }
          break;

        case OpKind::IMMEDIATE8_2ND:
          format_number( output, instruction.immediate8_2nd() );
          break;

        case OpKind::IMMEDIATE16:
          if ( is_declare_data ) {
            format_number( output, instruction.get_declare_word_value( operand ) );
          } else {
            format_number( output, instruction.immediate16() );
          }
          break;

        case OpKind::IMMEDIATE32:
          if ( is_declare_data ) {
            format_number( output, instruction.get_declare_dword_value( operand ) );
          } else {
            format_number( output, instruction.immediate32() );
          }
          break;

        case OpKind::IMMEDIATE64:
          if ( is_declare_data ) {
            format_number( output, instruction.get_declare_qword_value( operand ) );
          } else {
            format_number( output, instruction.immediate64() );
          }
          break;

        case OpKind::IMMEDIATE8TO16:
          format_number( output, static_cast<uint16_t>( 
            static_cast<int16_t>( static_cast<int8_t>( instruction.immediate8() ) ) ) );
          break;

        case OpKind::IMMEDIATE8TO32:
          format_number( output, static_cast<uint32_t>( 
            static_cast<int32_t>( static_cast<int8_t>( instruction.immediate8() ) ) ) );
          break;

        case OpKind::IMMEDIATE8TO64:
          format_number( output, static_cast<uint64_t>( 
            static_cast<int64_t>( static_cast<int8_t>( instruction.immediate8() ) ) ) );
          break;

        case OpKind::IMMEDIATE32TO64:
          format_number( output, static_cast<uint64_t>( 
            static_cast<int64_t>( static_cast<int32_t>( instruction.immediate32() ) ) ) );
          break;

        case OpKind::MEMORY_SEG_SI:
          format_memory( output, instruction, operand, instruction.memory_segment(),
                         Register::SI, Register::NONE, 0, 0, 0, 2 );
          break;

        case OpKind::MEMORY_SEG_ESI:
          format_memory( output, instruction, operand, instruction.memory_segment(),
                         Register::ESI, Register::NONE, 0, 0, 0, 4 );
          break;

        case OpKind::MEMORY_SEG_RSI:
          format_memory( output, instruction, operand, instruction.memory_segment(),
                         Register::RSI, Register::NONE, 0, 0, 0, 8 );
          break;

        case OpKind::MEMORY_SEG_DI:
          format_memory( output, instruction, operand, instruction.memory_segment(),
                         Register::DI, Register::NONE, 0, 0, 0, 2 );
          break;

        case OpKind::MEMORY_SEG_EDI:
          format_memory( output, instruction, operand, instruction.memory_segment(),
                         Register::EDI, Register::NONE, 0, 0, 0, 4 );
          break;

        case OpKind::MEMORY_SEG_RDI:
          format_memory( output, instruction, operand, instruction.memory_segment(),
                         Register::RDI, Register::NONE, 0, 0, 0, 8 );
          break;

        case OpKind::MEMORY_ESDI:
          format_memory( output, instruction, operand, Register::ES,
                         Register::DI, Register::NONE, 0, 0, 0, 2 );
          break;

        case OpKind::MEMORY_ESEDI:
          format_memory( output, instruction, operand, Register::ES,
                         Register::EDI, Register::NONE, 0, 0, 0, 4 );
          break;

        case OpKind::MEMORY_ESRDI:
          format_memory( output, instruction, operand, Register::ES,
                         Register::RDI, Register::NONE, 0, 0, 0, 8 );
          break;

        case OpKind::MEMORY: {
          uint32_t displ_size = instruction.memory_displ_size();
          Register base_reg = instruction.memory_base();
          Register index_reg = instruction.memory_index();
          uint32_t addr_size = get_address_size( base_reg, index_reg, displ_size, instruction.code_size() );
          int64_t displ;
          if ( addr_size == 8 ) {
            displ = static_cast<int64_t>( instruction.memory_displacement64() );
          } else {
            displ = static_cast<int64_t>( static_cast<int32_t>( instruction.memory_displacement32() ) );
          }
          // For XLAT, don't show index register
          if ( code == Code::XLAT_M8 ) {
            index_reg = Register::NONE;
          }
          uint32_t scale = 0;
          uint32_t mem_scale = instruction.memory_index_scale();
          if ( mem_scale == 2 ) scale = 1;
          else if ( mem_scale == 4 ) scale = 2;
          else if ( mem_scale == 8 ) scale = 3;
          format_memory( output, instruction, operand, instruction.memory_segment(),
                         base_reg, index_reg, scale, displ_size, displ, addr_size );
          break;
        }

        default:
          output.append( "???" );
          break;
      }

      // Opmask and zeroing for first operand
      if ( operand == 0 ) {
        Register op_mask_reg = instruction.op_mask();
        if ( op_mask_reg != Register::NONE ) {
          output.append( '{' );
          format_register( output, op_mask_reg );
          output.append( '}' );
        }
        if ( instruction.zeroing_masking() ) {
          output.append( "{z}" );
        }
      }
    }

    // Rounding control / SAE
    RoundingControl rc = instruction.rounding_control();
    if ( rc != RoundingControl::NONE ) {
      static constexpr std::string_view rc_sae_strings[] = {
        "{rn-sae}", "{rd-sae}", "{ru-sae}", "{rz-sae}"
      };
      output.append( rc_sae_strings[static_cast<uint32_t>( rc ) - 1] );
    } else if ( instruction.suppress_all_exceptions() ) {
      output.append( "{sae}" );
    }
  }
}

inline void FastFormatter::write_symbol( FastStringOutput& output, uint64_t address,
                                          const SymbolResult& symbol, bool write_minus_if_signed ) {
  int64_t displ = static_cast<int64_t>( address - symbol.address );
  if ( has_flag( symbol.flags, SymbolFlags::SIGNED ) ) {
    if ( write_minus_if_signed ) {
      output.append( '-' );
    }
    displ = -displ;
  }

  // Write the symbol text
  const auto& text = symbol.text;
  if ( text.has_parts() ) {
    for ( const auto& part : text.parts ) {
      output.append( part.text );
    }
  } else if ( !text.text.text.empty() ) {
    output.append( text.text.text );
  }

  // Write displacement if non-zero
  if ( displ != 0 ) {
    if ( displ < 0 ) {
      output.append( '-' );
      displ = -displ;
    } else {
      output.append( '+' );
    }
    format_number( output, static_cast<uint64_t>( displ ) );
  }

  // Optionally show the address
  if ( options_.show_symbol_address() ) {
    output.append( " (" );
    format_number( output, address );
    output.append( ')' );
  }
}

} // namespace iced_x86

#endif // ICED_X86_FAST_FORMATTER_HPP
