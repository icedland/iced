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
  bool use_hex_prefix = options_.use_hex_prefix();
  if ( use_hex_prefix ) {
    output.append( "0x" );
  }

  // Calculate number of hex digits needed
  int shift = 0;
  for ( uint64_t tmp = value; ; ) {
    shift += 4;
    tmp >>= 4;
    if ( tmp == 0 ) break;
  }

  // Add leading zero if first digit is a letter (for suffix format)
  if ( !use_hex_prefix && ( ( value >> ( shift - 4 ) ) & 0xF ) > 9 ) {
    output.append( '0' );
  }

  static constexpr char hex_upper[] = "0123456789ABCDEF";
  static constexpr char hex_lower[] = "0123456789abcdef";
  const char* hex_digits = options_.uppercase_hex() ? hex_upper : hex_lower;

  for ( ; ; ) {
    shift -= 4;
    int digit = static_cast<int>( ( value >> shift ) & 0xF );
    output.append( hex_digits[digit] );
    if ( shift == 0 ) break;
  }

  if ( !use_hex_prefix ) {
    output.append( 'h' );
  }
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
