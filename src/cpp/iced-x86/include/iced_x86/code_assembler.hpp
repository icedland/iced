// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_CODE_ASSEMBLER_HPP
#define ICED_X86_CODE_ASSEMBLER_HPP

#include "instruction.hpp"
#include "instruction_create.hpp"
#include "block_encoder.hpp"
#include "code_label.hpp"
#include "asm_memory_operand.hpp"
#include "asm_registers.hpp"
#include "asm_register_constants.hpp"
#include "code.hpp"
#include "memory_size.hpp"
#include "rounding_control.hpp"

#include <cstdint>
#include <vector>
#include <span>
#include <string>
#include <expected>
#include <stdexcept>

namespace iced_x86 {

/// @brief Prefix flags for CodeAssembler
struct AsmPrefixFlags {
  static constexpr uint8_t NONE = 0;
  static constexpr uint8_t LOCK = 1u << 0;
  static constexpr uint8_t REPE = 1u << 1;
  static constexpr uint8_t REPNE = 1u << 2;
  static constexpr uint8_t NOTRACK = 1u << 3;
  static constexpr uint8_t PREFER_VEX = 1u << 4;
  static constexpr uint8_t PREFER_EVEX = 1u << 5;
};

/// @brief Options for CodeAssembler
struct AsmOptions {
  static constexpr uint8_t PREFER_VEX = 1u << 0;
  static constexpr uint8_t PREFER_SHORT_BRANCH = 1u << 1;
};

/// @brief Result from assembling instructions
struct CodeAssemblerResult {
  /// @brief The encoded bytes
  std::vector<uint8_t> code;
  /// @brief The base RIP/EIP
  uint64_t rip = 0;
  /// @brief New instruction offsets (if BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS was used)
  std::vector<uint32_t> new_instruction_offsets;
  /// @brief Relocation info (if BlockEncoderOptions::RETURN_RELOC_INFOS was used)
  std::vector<RelocInfo> reloc_infos;
};

/// @brief High-level assembler for creating x86/x64 machine code.
///
/// CodeAssembler provides a fluent API for building machine code, similar to
/// writing assembly language. It handles instruction encoding, label resolution,
/// and jump optimizations automatically.
///
/// @example Creating a simple function:
/// @code
/// CodeAssembler a(64);
/// a.push(rbp);
/// a.mov(rbp, rsp);
/// a.xor_(eax, eax);  // Note: xor_ because xor is a C++ keyword
/// auto loop = a.create_label();
/// a.set_label(loop);
/// a.inc(eax);
/// a.cmp(eax, 10);
/// a.jl(loop);
/// a.pop(rbp);
/// a.ret();
/// auto bytes = a.assemble(0x401000);
/// @endcode
class CodeAssembler {
public:
  /// @brief Creates a new CodeAssembler for the specified bitness
  /// @param bitness 16, 32, or 64
  /// @throws std::invalid_argument if bitness is invalid
  explicit CodeAssembler( uint32_t bitness )
    : bitness_( bitness )
    , options_( AsmOptions::PREFER_VEX | AsmOptions::PREFER_SHORT_BRANCH )
  {
    if ( bitness != 16 && bitness != 32 && bitness != 64 ) {
      throw std::invalid_argument( "Invalid bitness: must be 16, 32, or 64" );
    }
  }

  /// @brief Gets the bitness (16, 32, or 64)
  [[nodiscard]] uint32_t bitness() const noexcept { return bitness_; }

  /// @brief Gets all added instructions
  [[nodiscard]] const std::vector<Instruction>& instructions() const noexcept {
    return instructions_;
  }

  /// @brief Gets whether to prefer VEX encoding over EVEX when both are available
  [[nodiscard]] bool prefer_vex() const noexcept {
    return ( options_ & AsmOptions::PREFER_VEX ) != 0;
  }

  /// @brief Sets whether to prefer VEX encoding over EVEX when both are available
  void set_prefer_vex( bool value ) noexcept {
    if ( value ) {
      options_ |= AsmOptions::PREFER_VEX;
    } else {
      options_ &= ~AsmOptions::PREFER_VEX;
    }
  }

  /// @brief Gets whether to prefer short branch encoding
  [[nodiscard]] bool prefer_short_branch() const noexcept {
    return ( options_ & AsmOptions::PREFER_SHORT_BRANCH ) != 0;
  }

  /// @brief Sets whether to prefer short branch encoding
  void set_prefer_short_branch( bool value ) noexcept {
    if ( value ) {
      options_ |= AsmOptions::PREFER_SHORT_BRANCH;
    } else {
      options_ &= ~AsmOptions::PREFER_SHORT_BRANCH;
    }
  }

  /// @brief Resets the assembler, clearing all instructions and labels
  void reset() noexcept {
    instructions_.clear();
    current_label_id_ = 0;
    current_label_ = CodeLabel{};
    current_anon_label_ = CodeLabel{};
    next_anon_label_ = CodeLabel{};
    defined_anon_label_ = false;
    prefix_flags_ = AsmPrefixFlags::NONE;
  }

  /// @brief Creates a label that can be used as a jump target
  [[nodiscard]] CodeLabel create_label() noexcept {
    ++current_label_id_;
    return CodeLabel{ current_label_id_ };
  }

  /// @brief Sets the label's position to the next instruction
  /// @param label The label to set
  /// @throws std::invalid_argument if label is invalid or already set
  void set_label( CodeLabel& label ) {
    if ( label.is_empty() ) {
      throw std::invalid_argument( "Invalid label. Must be created via create_label()" );
    }
    if ( label.has_instruction_index() ) {
      throw std::invalid_argument( "Labels can't be re-used and can only be set once" );
    }
    if ( !current_label_.is_empty() ) {
      throw std::invalid_argument( "Only one label per instruction is allowed" );
    }
    label.set_instruction_index( instructions_.size() );
    current_label_ = label;
  }

  /// @brief Creates an anonymous label for the next instruction
  /// @throws std::runtime_error if an anonymous label was already defined for the next instruction
  void anonymous_label() {
    if ( defined_anon_label_ ) {
      throw std::runtime_error( "At most one anonymous label per instruction is allowed" );
    }
    current_anon_label_ = next_anon_label_.is_empty() ? create_label() : next_anon_label_;
    next_anon_label_ = CodeLabel{};
    defined_anon_label_ = true;
  }

  /// @brief Gets the backward anonymous label (most recently defined)
  /// @throws std::runtime_error if no anonymous label has been created yet
  [[nodiscard]] CodeLabel bwd() const {
    if ( current_anon_label_.is_empty() ) {
      throw std::runtime_error( "No anonymous label has been created yet" );
    }
    return current_anon_label_;
  }

  /// @brief Gets the forward anonymous label (will be defined later)
  [[nodiscard]] CodeLabel fwd() {
    if ( next_anon_label_.is_empty() ) {
      next_anon_label_ = create_label();
    }
    return next_anon_label_;
  }

  // ============================================================================
  // Prefix methods (return *this for chaining)
  // ============================================================================

  /// @brief Adds a LOCK prefix to the next instruction
  CodeAssembler& lock() noexcept {
    prefix_flags_ |= AsmPrefixFlags::LOCK;
    return *this;
  }

  /// @brief Adds a REP prefix to the next instruction
  CodeAssembler& rep() noexcept {
    prefix_flags_ |= AsmPrefixFlags::REPE;
    return *this;
  }

  /// @brief Adds a REPE/REPZ prefix to the next instruction
  CodeAssembler& repe() noexcept {
    prefix_flags_ |= AsmPrefixFlags::REPE;
    return *this;
  }

  /// @brief Adds a REPE/REPZ prefix to the next instruction
  CodeAssembler& repz() noexcept {
    return repe();
  }

  /// @brief Adds a REPNE/REPNZ prefix to the next instruction
  CodeAssembler& repne() noexcept {
    prefix_flags_ |= AsmPrefixFlags::REPNE;
    return *this;
  }

  /// @brief Adds a REPNE/REPNZ prefix to the next instruction
  CodeAssembler& repnz() noexcept {
    return repne();
  }

  /// @brief Adds an XACQUIRE prefix to the next instruction
  CodeAssembler& xacquire() noexcept {
    prefix_flags_ |= AsmPrefixFlags::REPNE;
    return *this;
  }

  /// @brief Adds an XRELEASE prefix to the next instruction
  CodeAssembler& xrelease() noexcept {
    prefix_flags_ |= AsmPrefixFlags::REPE;
    return *this;
  }

  /// @brief Adds a BND prefix to the next instruction
  CodeAssembler& bnd() noexcept {
    prefix_flags_ |= AsmPrefixFlags::REPNE;
    return *this;
  }

  /// @brief Adds a NOTRACK prefix to the next instruction
  CodeAssembler& notrack() noexcept {
    prefix_flags_ |= AsmPrefixFlags::NOTRACK;
    return *this;
  }

  /// @brief Prefer VEX encoding for the next instruction
  CodeAssembler& vex() noexcept {
    prefix_flags_ |= AsmPrefixFlags::PREFER_VEX;
    return *this;
  }

  /// @brief Prefer EVEX encoding for the next instruction
  CodeAssembler& evex() noexcept {
    prefix_flags_ |= AsmPrefixFlags::PREFER_EVEX;
    return *this;
  }

  // ============================================================================
  // Data declaration methods
  // ============================================================================

  /// @brief Adds raw bytes
  void db( std::span<const uint8_t> data ) {
    verify_no_prefixes( "db" );
    constexpr size_t MAX_DB_COUNT = 16;
    for ( size_t i = 0; i < data.size(); i += MAX_DB_COUNT ) {
      size_t count = std::min( MAX_DB_COUNT, data.size() - i );
      auto instr = InstructionFactory::with_declare_byte_span( data.subspan( i, count ) );
      add_instruction( instr );
    }
  }

  /// @brief Adds 16-bit words
  void dw( std::span<const uint16_t> data ) {
    verify_no_prefixes( "dw" );
    constexpr size_t MAX_DW_COUNT = 8;
    for ( size_t i = 0; i < data.size(); i += MAX_DW_COUNT ) {
      size_t count = std::min( MAX_DW_COUNT, data.size() - i );
      auto instr = InstructionFactory::with_declare_word_span( data.subspan( i, count ) );
      add_instruction( instr );
    }
  }

  /// @brief Adds 32-bit dwords
  void dd( std::span<const uint32_t> data ) {
    verify_no_prefixes( "dd" );
    constexpr size_t MAX_DD_COUNT = 4;
    for ( size_t i = 0; i < data.size(); i += MAX_DD_COUNT ) {
      size_t count = std::min( MAX_DD_COUNT, data.size() - i );
      auto instr = InstructionFactory::with_declare_dword_span( data.subspan( i, count ) );
      add_instruction( instr );
    }
  }

  /// @brief Adds 64-bit qwords
  void dq( std::span<const uint64_t> data ) {
    verify_no_prefixes( "dq" );
    constexpr size_t MAX_DQ_COUNT = 2;
    for ( size_t i = 0; i < data.size(); i += MAX_DQ_COUNT ) {
      size_t count = std::min( MAX_DQ_COUNT, data.size() - i );
      auto instr = InstructionFactory::with_declare_qword_span( data.subspan( i, count ) );
      add_instruction( instr );
    }
  }

  /// @brief Adds a single byte
  void db( uint8_t value ) {
    verify_no_prefixes( "db" );
    auto instr = InstructionFactory::with_declare_byte_1( value );
    add_instruction( instr );
  }

  /// @brief Adds a single 16-bit word
  void dw( uint16_t value ) {
    verify_no_prefixes( "dw" );
    auto instr = InstructionFactory::with_declare_word_1( value );
    add_instruction( instr );
  }

  /// @brief Adds a single 32-bit dword
  void dd( uint32_t value ) {
    verify_no_prefixes( "dd" );
    auto instr = InstructionFactory::with_declare_dword_1( value );
    add_instruction( instr );
  }

  /// @brief Adds a single 64-bit qword
  void dq( uint64_t value ) {
    verify_no_prefixes( "dq" );
    auto instr = InstructionFactory::with_declare_qword_1( value );
    add_instruction( instr );
  }

  // ============================================================================
  // Add instruction method
  // ============================================================================

  /// @brief Adds an instruction directly
  void add_instruction( Instruction& instr ) {
    if ( !current_label_.is_empty() && defined_anon_label_ ) {
      throw std::runtime_error( "Cannot have both normal and anonymous label on same instruction" );
    }

    // Set label IP
    if ( !current_label_.is_empty() ) {
      instr.set_ip( current_label_.id() );
    } else if ( defined_anon_label_ ) {
      instr.set_ip( current_anon_label_.id() );
    }

    // Apply prefix flags
    if ( prefix_flags_ != AsmPrefixFlags::NONE ) {
      if ( prefix_flags_ & AsmPrefixFlags::LOCK ) {
        instr.set_has_lock_prefix( true );
      }
      if ( prefix_flags_ & AsmPrefixFlags::REPE ) {
        instr.set_has_rep_prefix( true );
      } else if ( prefix_flags_ & AsmPrefixFlags::REPNE ) {
        instr.set_has_repne_prefix( true );
      }
      if ( prefix_flags_ & AsmPrefixFlags::NOTRACK ) {
        instr.set_segment_prefix( Register::DS );
      }
    }

    instructions_.push_back( instr );

    // Reset per-instruction state
    current_label_ = CodeLabel{};
    defined_anon_label_ = false;
    prefix_flags_ = AsmPrefixFlags::NONE;
  }

  /// @brief Adds an instruction with operand flags
  void add_instruction_with_flags( Instruction& instr, uint32_t flags ) {
    if ( flags != AsmOperandFlags::NONE ) {
      if ( flags & AsmOperandFlags::BROADCAST ) {
        instr.set_is_broadcast( true );
      }
      if ( flags & AsmOperandFlags::ZEROING ) {
        instr.set_zeroing_masking( true );
      }
      if ( flags & AsmOperandFlags::SUPPRESS_ALL_EXCEPTIONS ) {
        instr.set_suppress_all_exceptions( true );
      }
      uint32_t mask_bits = ( flags & AsmOperandFlags::REGISTER_MASK ) >> 6;
      if ( mask_bits != 0 ) {
        instr.set_op_mask( static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + mask_bits ) );
      }
      uint32_t rc_bits = ( flags & AsmOperandFlags::ROUNDING_CONTROL_MASK ) >> 3;
      if ( rc_bits != 0 ) {
        instr.set_rounding_control( static_cast<RoundingControl>( rc_bits ) );
      }
    }
    add_instruction( instr );
  }

  // ============================================================================
  // Assemble methods
  // ============================================================================

  /// @brief Assembles all instructions and returns the encoded bytes
  /// @param ip Base address for the code
  /// @return Encoded bytes
  /// @throws std::runtime_error if assembling fails
  [[nodiscard]] std::vector<uint8_t> assemble( uint64_t ip ) {
    return assemble_options( ip, BlockEncoderOptions::NONE ).code;
  }

  /// @brief Assembles all instructions with options
  /// @param ip Base address for the code
  /// @param options BlockEncoder options
  /// @return Assembly result with code and metadata
  /// @throws std::runtime_error if assembling fails
  [[nodiscard]] CodeAssemblerResult assemble_options( uint64_t ip, uint32_t options ) {
    // Validate state
    if ( prefix_flags_ != AsmPrefixFlags::NONE ) {
      throw std::runtime_error( "Unused prefix. Did you forget to add an instruction?" );
    }
    if ( !current_label_.is_empty() ) {
      throw std::runtime_error( "Unused label. Did you forget to add an instruction?" );
    }
    if ( defined_anon_label_ ) {
      throw std::runtime_error( "Unused anonymous label. Did you forget to add an instruction?" );
    }
    if ( !next_anon_label_.is_empty() ) {
      throw std::runtime_error( "Unused forward anonymous label. Did you forget to call anonymous_label()?" );
    }

    // Use BlockEncoder to encode
    auto encode_result = BlockEncoder::encode( bitness_, instructions_, ip, options );
    if ( !encode_result ) {
      throw std::runtime_error( encode_result.error() );
    }
    BlockEncoderResult result = std::move( encode_result.value() );

    CodeAssemblerResult asm_result;
    asm_result.code = std::move( result.code_buffer );
    asm_result.rip = result.rip;
    asm_result.new_instruction_offsets = std::move( result.new_instruction_offsets );
    asm_result.reloc_infos = std::move( result.reloc_infos );
    return asm_result;
  }

  // ============================================================================
  // Common instructions (examples - full implementation would have many more)
  // ============================================================================

  /// @brief NOP instruction
  void nop() {
    // Use NOPD even in 64-bit mode to avoid REX prefix
    // NOPD encodes to simple 0x90 which works in all modes
    Code code = bitness_ == 16 ? Code::NOPW : Code::NOPD;
    auto instr = InstructionFactory::with( code );
    add_instruction( instr );
  }

  /// @brief RET instruction
  void ret() {
    Code code = bitness_ == 64 ? Code::RETNQ :
                bitness_ == 32 ? Code::RETND : Code::RETNW;
    auto instr = InstructionFactory::with( code );
    add_instruction( instr );
  }

  /// @brief INT3 instruction (breakpoint)
  void int3() {
    auto instr = InstructionFactory::with( Code::INT3 );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // PUSH instructions
  // --------------------------------------------------------------------------

  void push( AsmRegister16 reg ) {
    auto instr = InstructionFactory::with1( Code::PUSH_R16, reg.value );
    add_instruction( instr );
  }

  void push( AsmRegister32 reg ) {
    auto instr = InstructionFactory::with1( Code::PUSH_R32, reg.value );
    add_instruction( instr );
  }

  void push( AsmRegister64 reg ) {
    auto instr = InstructionFactory::with1( Code::PUSH_R64, reg.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // POP instructions
  // --------------------------------------------------------------------------

  void pop( AsmRegister16 reg ) {
    auto instr = InstructionFactory::with1( Code::POP_R16, reg.value );
    add_instruction( instr );
  }

  void pop( AsmRegister32 reg ) {
    auto instr = InstructionFactory::with1( Code::POP_R32, reg.value );
    add_instruction( instr );
  }

  void pop( AsmRegister64 reg ) {
    auto instr = InstructionFactory::with1( Code::POP_R64, reg.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // MOV instructions
  // --------------------------------------------------------------------------

  void mov( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void mov( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void mov( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void mov( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void mov( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::MOV_R32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void mov( AsmRegister64 dst, int64_t imm ) {
    auto instr = InstructionFactory::with2( Code::MOV_R64_IMM64, dst.value, imm );
    add_instruction( instr );
  }

  void mov( AsmRegister64 dst, uint64_t imm ) {
    auto instr = InstructionFactory::with2( Code::MOV_R64_IMM64, dst.value, static_cast<int64_t>( imm ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // XOR instructions
  // --------------------------------------------------------------------------

  void xor_( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::XOR_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void xor_( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::XOR_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void xor_( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::XOR_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void xor_( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::XOR_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // ADD instructions
  // --------------------------------------------------------------------------

  void add( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void add( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void add( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void add( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SUB instructions
  // --------------------------------------------------------------------------

  void sub( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void sub( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // INC/DEC instructions
  // --------------------------------------------------------------------------

  void inc( AsmRegister32 dst ) {
    auto instr = InstructionFactory::with1( Code::INC_RM32, dst.value );
    add_instruction( instr );
  }

  void inc( AsmRegister64 dst ) {
    auto instr = InstructionFactory::with1( Code::INC_RM64, dst.value );
    add_instruction( instr );
  }

  void dec( AsmRegister32 dst ) {
    auto instr = InstructionFactory::with1( Code::DEC_RM32, dst.value );
    add_instruction( instr );
  }

  void dec( AsmRegister64 dst ) {
    auto instr = InstructionFactory::with1( Code::DEC_RM64, dst.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // CMP instructions
  // --------------------------------------------------------------------------

  void cmp( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmp( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void cmp( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void cmp( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // JMP instruction
  // --------------------------------------------------------------------------

  void jmp( CodeLabel label ) {
    Code code = prefer_short_branch() ? Code::JMP_REL8_64 : Code::JMP_REL32_64;
    if ( bitness_ == 32 ) {
      code = prefer_short_branch() ? Code::JMP_REL8_32 : Code::JMP_REL32_32;
    } else if ( bitness_ == 16 ) {
      code = prefer_short_branch() ? Code::JMP_REL8_16 : Code::JMP_REL16;
    }
    auto instr = InstructionFactory::with_branch( code, label.id() );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // Conditional jump instructions
  // --------------------------------------------------------------------------

  void je( CodeLabel label ) { jcc( Code::JE_REL8_64, Code::JE_REL32_64, label ); }
  void jz( CodeLabel label ) { je( label ); }
  void jne( CodeLabel label ) { jcc( Code::JNE_REL8_64, Code::JNE_REL32_64, label ); }
  void jnz( CodeLabel label ) { jne( label ); }
  void jl( CodeLabel label ) { jcc( Code::JL_REL8_64, Code::JL_REL32_64, label ); }
  void jb( CodeLabel label ) { jcc( Code::JB_REL8_64, Code::JB_REL32_64, label ); }
  void jle( CodeLabel label ) { jcc( Code::JLE_REL8_64, Code::JLE_REL32_64, label ); }
  void jbe( CodeLabel label ) { jcc( Code::JBE_REL8_64, Code::JBE_REL32_64, label ); }
  void jg( CodeLabel label ) { jcc( Code::JG_REL8_64, Code::JG_REL32_64, label ); }
  void ja( CodeLabel label ) { jcc( Code::JA_REL8_64, Code::JA_REL32_64, label ); }
  void jge( CodeLabel label ) { jcc( Code::JGE_REL8_64, Code::JGE_REL32_64, label ); }
  void jae( CodeLabel label ) { jcc( Code::JAE_REL8_64, Code::JAE_REL32_64, label ); }
  void js( CodeLabel label ) { jcc( Code::JS_REL8_64, Code::JS_REL32_64, label ); }
  void jns( CodeLabel label ) { jcc( Code::JNS_REL8_64, Code::JNS_REL32_64, label ); }
  void jp( CodeLabel label ) { jcc( Code::JP_REL8_64, Code::JP_REL32_64, label ); }
  void jnp( CodeLabel label ) { jcc( Code::JNP_REL8_64, Code::JNP_REL32_64, label ); }
  void jo( CodeLabel label ) { jcc( Code::JO_REL8_64, Code::JO_REL32_64, label ); }
  void jno( CodeLabel label ) { jcc( Code::JNO_REL8_64, Code::JNO_REL32_64, label ); }

  // --------------------------------------------------------------------------
  // CALL instruction
  // --------------------------------------------------------------------------

  void call( CodeLabel label ) {
    Code code = bitness_ == 64 ? Code::CALL_REL32_64 :
                bitness_ == 32 ? Code::CALL_REL32_32 : Code::CALL_REL16;
    auto instr = InstructionFactory::with_branch( code, label.id() );
    add_instruction( instr );
  }

  void call( AsmRegister64 reg ) {
    auto instr = InstructionFactory::with1( Code::CALL_RM64, reg.value );
    add_instruction( instr );
  }

  void call( AsmRegister32 reg ) {
    auto instr = InstructionFactory::with1( Code::CALL_RM32, reg.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // LEA instruction
  // --------------------------------------------------------------------------

  void lea( AsmRegister32 dst, const AsmMemoryOperand& mem ) {
    auto instr = InstructionFactory::with2( Code::LEA_R32_M, dst.value, mem.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void lea( AsmRegister64 dst, const AsmMemoryOperand& mem ) {
    auto instr = InstructionFactory::with2( Code::LEA_R64_M, dst.value, mem.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // AND instructions
  // --------------------------------------------------------------------------

  void and_( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::AND_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void and_( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::AND_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void and_( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::AND_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void and_( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::AND_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void and_( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::AND_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void and_( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::AND_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // OR instructions
  // --------------------------------------------------------------------------

  void or_( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::OR_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void or_( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::OR_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void or_( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::OR_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void or_( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::OR_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void or_( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::OR_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void or_( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::OR_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // NOT/NEG instructions
  // --------------------------------------------------------------------------

  void not_( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::NOT_RM8, dst.value );
    add_instruction( instr );
  }

  void not_( AsmRegister16 dst ) {
    auto instr = InstructionFactory::with1( Code::NOT_RM16, dst.value );
    add_instruction( instr );
  }

  void not_( AsmRegister32 dst ) {
    auto instr = InstructionFactory::with1( Code::NOT_RM32, dst.value );
    add_instruction( instr );
  }

  void not_( AsmRegister64 dst ) {
    auto instr = InstructionFactory::with1( Code::NOT_RM64, dst.value );
    add_instruction( instr );
  }

  void neg( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::NEG_RM8, dst.value );
    add_instruction( instr );
  }

  void neg( AsmRegister16 dst ) {
    auto instr = InstructionFactory::with1( Code::NEG_RM16, dst.value );
    add_instruction( instr );
  }

  void neg( AsmRegister32 dst ) {
    auto instr = InstructionFactory::with1( Code::NEG_RM32, dst.value );
    add_instruction( instr );
  }

  void neg( AsmRegister64 dst ) {
    auto instr = InstructionFactory::with1( Code::NEG_RM64, dst.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // TEST instructions
  // --------------------------------------------------------------------------

  void test( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::TEST_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void test( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::TEST_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void test( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::TEST_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void test( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::TEST_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void test( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::TEST_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void test( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::TEST_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // XCHG instructions
  // --------------------------------------------------------------------------

  void xchg( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::XCHG_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void xchg( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::XCHG_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void xchg( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::XCHG_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void xchg( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::XCHG_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // Shift/Rotate instructions
  // --------------------------------------------------------------------------

  void shl( AsmRegister32 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::SHL_RM32_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void shl( AsmRegister64 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::SHL_RM64_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void shl( AsmRegister32 dst, AsmRegister8 cl ) {
    (void)cl;  // Must be CL
    auto instr = InstructionFactory::with1( Code::SHL_RM32_CL, dst.value );
    add_instruction( instr );
  }

  void shl( AsmRegister64 dst, AsmRegister8 cl ) {
    (void)cl;  // Must be CL
    auto instr = InstructionFactory::with1( Code::SHL_RM64_CL, dst.value );
    add_instruction( instr );
  }

  void shr( AsmRegister32 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::SHR_RM32_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void shr( AsmRegister64 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::SHR_RM64_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void shr( AsmRegister32 dst, AsmRegister8 cl ) {
    (void)cl;
    auto instr = InstructionFactory::with1( Code::SHR_RM32_CL, dst.value );
    add_instruction( instr );
  }

  void shr( AsmRegister64 dst, AsmRegister8 cl ) {
    (void)cl;
    auto instr = InstructionFactory::with1( Code::SHR_RM64_CL, dst.value );
    add_instruction( instr );
  }

  void sar( AsmRegister32 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::SAR_RM32_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void sar( AsmRegister64 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::SAR_RM64_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void sar( AsmRegister32 dst, AsmRegister8 cl ) {
    (void)cl;
    auto instr = InstructionFactory::with1( Code::SAR_RM32_CL, dst.value );
    add_instruction( instr );
  }

  void sar( AsmRegister64 dst, AsmRegister8 cl ) {
    (void)cl;
    auto instr = InstructionFactory::with1( Code::SAR_RM64_CL, dst.value );
    add_instruction( instr );
  }

  void rol( AsmRegister32 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::ROL_RM32_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void rol( AsmRegister64 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::ROL_RM64_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void ror( AsmRegister32 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::ROR_RM32_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void ror( AsmRegister64 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::ROR_RM64_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // MUL/DIV/IMUL/IDIV instructions
  // --------------------------------------------------------------------------

  void mul( AsmRegister8 src ) {
    auto instr = InstructionFactory::with1( Code::MUL_RM8, src.value );
    add_instruction( instr );
  }

  void mul( AsmRegister16 src ) {
    auto instr = InstructionFactory::with1( Code::MUL_RM16, src.value );
    add_instruction( instr );
  }

  void mul( AsmRegister32 src ) {
    auto instr = InstructionFactory::with1( Code::MUL_RM32, src.value );
    add_instruction( instr );
  }

  void mul( AsmRegister64 src ) {
    auto instr = InstructionFactory::with1( Code::MUL_RM64, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister8 src ) {
    auto instr = InstructionFactory::with1( Code::IMUL_RM8, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister16 src ) {
    auto instr = InstructionFactory::with1( Code::IMUL_RM16, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister32 src ) {
    auto instr = InstructionFactory::with1( Code::IMUL_RM32, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister64 src ) {
    auto instr = InstructionFactory::with1( Code::IMUL_RM64, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::IMUL_R16_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::IMUL_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::IMUL_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void imul( AsmRegister32 dst, AsmRegister32 src, int32_t imm ) {
    auto instr = InstructionFactory::with3( Code::IMUL_R32_RM32_IMM32, dst.value, src.value, imm );
    add_instruction( instr );
  }

  void imul( AsmRegister64 dst, AsmRegister64 src, int32_t imm ) {
    auto instr = InstructionFactory::with3( Code::IMUL_R64_RM64_IMM32, dst.value, src.value, imm );
    add_instruction( instr );
  }

  void div( AsmRegister8 src ) {
    auto instr = InstructionFactory::with1( Code::DIV_RM8, src.value );
    add_instruction( instr );
  }

  void div( AsmRegister16 src ) {
    auto instr = InstructionFactory::with1( Code::DIV_RM16, src.value );
    add_instruction( instr );
  }

  void div( AsmRegister32 src ) {
    auto instr = InstructionFactory::with1( Code::DIV_RM32, src.value );
    add_instruction( instr );
  }

  void div( AsmRegister64 src ) {
    auto instr = InstructionFactory::with1( Code::DIV_RM64, src.value );
    add_instruction( instr );
  }

  void idiv( AsmRegister8 src ) {
    auto instr = InstructionFactory::with1( Code::IDIV_RM8, src.value );
    add_instruction( instr );
  }

  void idiv( AsmRegister16 src ) {
    auto instr = InstructionFactory::with1( Code::IDIV_RM16, src.value );
    add_instruction( instr );
  }

  void idiv( AsmRegister32 src ) {
    auto instr = InstructionFactory::with1( Code::IDIV_RM32, src.value );
    add_instruction( instr );
  }

  void idiv( AsmRegister64 src ) {
    auto instr = InstructionFactory::with1( Code::IDIV_RM64, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // CDQ/CQO/CBW/CWDE/CDQE instructions (sign extension)
  // --------------------------------------------------------------------------

  void cbw() {
    auto instr = InstructionFactory::with( Code::CBW );
    add_instruction( instr );
  }

  void cwde() {
    auto instr = InstructionFactory::with( Code::CWDE );
    add_instruction( instr );
  }

  void cdqe() {
    auto instr = InstructionFactory::with( Code::CDQE );
    add_instruction( instr );
  }

  void cwd() {
    auto instr = InstructionFactory::with( Code::CWD );
    add_instruction( instr );
  }

  void cdq() {
    auto instr = InstructionFactory::with( Code::CDQ );
    add_instruction( instr );
  }

  void cqo() {
    auto instr = InstructionFactory::with( Code::CQO );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // LEAVE instruction
  // --------------------------------------------------------------------------

  void leave() {
    Code code = bitness_ == 64 ? Code::LEAVEQ :
                bitness_ == 32 ? Code::LEAVED : Code::LEAVEW;
    auto instr = InstructionFactory::with( code );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // MOVZX/MOVSX instructions
  // --------------------------------------------------------------------------

  void movzx( AsmRegister16 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOVZX_R16_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void movzx( AsmRegister32 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOVZX_R32_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void movzx( AsmRegister64 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOVZX_R64_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void movzx( AsmRegister32 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::MOVZX_R32_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  void movzx( AsmRegister64 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::MOVZX_R64_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  void movsx( AsmRegister16 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOVSX_R16_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void movsx( AsmRegister32 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOVSX_R32_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void movsx( AsmRegister64 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOVSX_R64_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void movsx( AsmRegister32 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::MOVSX_R32_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  void movsx( AsmRegister64 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::MOVSX_R64_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  void movsxd( AsmRegister64 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::MOVSXD_R64_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SETCC instructions
  // --------------------------------------------------------------------------

  void sete( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETE_RM8, dst.value );
    add_instruction( instr );
  }
  void setz( AsmRegister8 dst ) { sete( dst ); }

  void setne( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETNE_RM8, dst.value );
    add_instruction( instr );
  }
  void setnz( AsmRegister8 dst ) { setne( dst ); }

  void setl( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETL_RM8, dst.value );
    add_instruction( instr );
  }

  void setle( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETLE_RM8, dst.value );
    add_instruction( instr );
  }

  void setg( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETG_RM8, dst.value );
    add_instruction( instr );
  }

  void setge( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETGE_RM8, dst.value );
    add_instruction( instr );
  }

  void setb( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETB_RM8, dst.value );
    add_instruction( instr );
  }

  void setbe( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETBE_RM8, dst.value );
    add_instruction( instr );
  }

  void seta( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETA_RM8, dst.value );
    add_instruction( instr );
  }

  void setae( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::SETAE_RM8, dst.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // CMOVCC instructions
  // --------------------------------------------------------------------------

  void cmove( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVE_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmove( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVE_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovne( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVNE_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovne( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVNE_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovl( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVL_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovl( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVL_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovle( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVLE_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovle( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVLE_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovg( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVG_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovg( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVG_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovge( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVGE_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmovge( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMOVGE_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // BSF/BSR/POPCNT/LZCNT/TZCNT instructions
  // --------------------------------------------------------------------------

  void bsf( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::BSF_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void bsf( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::BSF_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void bsr( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::BSR_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void bsr( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::BSR_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void popcnt( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::POPCNT_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void popcnt( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::POPCNT_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void lzcnt( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::LZCNT_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void lzcnt( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::LZCNT_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void tzcnt( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::TZCNT_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void tzcnt( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::TZCNT_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // BT/BTS/BTR/BTC instructions (bit test)
  // --------------------------------------------------------------------------

  void bt( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::BT_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void bt( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::BT_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void bt( AsmRegister32 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::BT_RM32_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void bt( AsmRegister64 dst, uint8_t imm ) {
    auto instr = InstructionFactory::with2( Code::BT_RM64_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void bts( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::BTS_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void bts( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::BTS_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void btr( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::BTR_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void btr( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::BTR_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void btc( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::BTC_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void btc( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::BTC_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // String instructions (use with rep/repe/repne prefixes)
  // --------------------------------------------------------------------------

  void movsb() {
    auto instr = InstructionFactory::with( Code::MOVSB_M8_M8 );
    add_instruction( instr );
  }

  void movsw() {
    auto instr = InstructionFactory::with( Code::MOVSW_M16_M16 );
    add_instruction( instr );
  }

  void movsd_string() {
    auto instr = InstructionFactory::with( Code::MOVSD_M32_M32 );
    add_instruction( instr );
  }

  void movsq() {
    auto instr = InstructionFactory::with( Code::MOVSQ_M64_M64 );
    add_instruction( instr );
  }

  void stosb() {
    auto instr = InstructionFactory::with( Code::STOSB_M8_AL );
    add_instruction( instr );
  }

  void stosw() {
    auto instr = InstructionFactory::with( Code::STOSW_M16_AX );
    add_instruction( instr );
  }

  void stosd() {
    auto instr = InstructionFactory::with( Code::STOSD_M32_EAX );
    add_instruction( instr );
  }

  void stosq() {
    auto instr = InstructionFactory::with( Code::STOSQ_M64_RAX );
    add_instruction( instr );
  }

  void lodsb() {
    auto instr = InstructionFactory::with( Code::LODSB_AL_M8 );
    add_instruction( instr );
  }

  void lodsw() {
    auto instr = InstructionFactory::with( Code::LODSW_AX_M16 );
    add_instruction( instr );
  }

  void lodsd() {
    auto instr = InstructionFactory::with( Code::LODSD_EAX_M32 );
    add_instruction( instr );
  }

  void lodsq() {
    auto instr = InstructionFactory::with( Code::LODSQ_RAX_M64 );
    add_instruction( instr );
  }

  void cmpsb() {
    auto instr = InstructionFactory::with( Code::CMPSB_M8_M8 );
    add_instruction( instr );
  }

  void cmpsw() {
    auto instr = InstructionFactory::with( Code::CMPSW_M16_M16 );
    add_instruction( instr );
  }

  void cmpsd_string() {
    auto instr = InstructionFactory::with( Code::CMPSD_M32_M32 );
    add_instruction( instr );
  }

  void cmpsq() {
    auto instr = InstructionFactory::with( Code::CMPSQ_M64_M64 );
    add_instruction( instr );
  }

  void scasb() {
    auto instr = InstructionFactory::with( Code::SCASB_AL_M8 );
    add_instruction( instr );
  }

  void scasw() {
    auto instr = InstructionFactory::with( Code::SCASW_AX_M16 );
    add_instruction( instr );
  }

  void scasd() {
    auto instr = InstructionFactory::with( Code::SCASD_EAX_M32 );
    add_instruction( instr );
  }

  void scasq() {
    auto instr = InstructionFactory::with( Code::SCASQ_RAX_M64 );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SYSCALL/SYSENTER/CPUID/RDTSC instructions
  // --------------------------------------------------------------------------

  void syscall() {
    auto instr = InstructionFactory::with( Code::SYSCALL );
    add_instruction( instr );
  }

  void sysenter() {
    auto instr = InstructionFactory::with( Code::SYSENTER );
    add_instruction( instr );
  }

  void cpuid() {
    auto instr = InstructionFactory::with( Code::CPUID );
    add_instruction( instr );
  }

  void rdtsc() {
    auto instr = InstructionFactory::with( Code::RDTSC );
    add_instruction( instr );
  }

  void rdtscp() {
    auto instr = InstructionFactory::with( Code::RDTSCP );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // Flag manipulation instructions
  // --------------------------------------------------------------------------

  void clc() {
    auto instr = InstructionFactory::with( Code::CLC );
    add_instruction( instr );
  }

  void stc() {
    auto instr = InstructionFactory::with( Code::STC );
    add_instruction( instr );
  }

  void cmc() {
    auto instr = InstructionFactory::with( Code::CMC );
    add_instruction( instr );
  }

  void cld() {
    auto instr = InstructionFactory::with( Code::CLD );
    add_instruction( instr );
  }

  void std_() {
    auto instr = InstructionFactory::with( Code::STD );
    add_instruction( instr );
  }

  void pushfq() {
    auto instr = InstructionFactory::with( Code::PUSHFQ );
    add_instruction( instr );
  }

  void popfq() {
    auto instr = InstructionFactory::with( Code::POPFQ );
    add_instruction( instr );
  }

  void pushfd() {
    auto instr = InstructionFactory::with( Code::PUSHFD );
    add_instruction( instr );
  }

  void popfd() {
    auto instr = InstructionFactory::with( Code::POPFD );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // Memory fence instructions
  // --------------------------------------------------------------------------

  void mfence() {
    auto instr = InstructionFactory::with( Code::MFENCE );
    add_instruction( instr );
  }

  void sfence() {
    auto instr = InstructionFactory::with( Code::SFENCE );
    add_instruction( instr );
  }

  void lfence() {
    auto instr = InstructionFactory::with( Code::LFENCE );
    add_instruction( instr );
  }

  void pause() {
    auto instr = InstructionFactory::with( Code::PAUSE );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // UD2 instruction (undefined instruction for traps)
  // --------------------------------------------------------------------------

  void ud2() {
    auto instr = InstructionFactory::with( Code::UD2 );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // INC/DEC for 8/16-bit registers
  // --------------------------------------------------------------------------

  void inc( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::INC_RM8, dst.value );
    add_instruction( instr );
  }

  void inc( AsmRegister16 dst ) {
    auto instr = InstructionFactory::with1( Code::INC_RM16, dst.value );
    add_instruction( instr );
  }

  void dec( AsmRegister8 dst ) {
    auto instr = InstructionFactory::with1( Code::DEC_RM8, dst.value );
    add_instruction( instr );
  }

  void dec( AsmRegister16 dst ) {
    auto instr = InstructionFactory::with1( Code::DEC_RM16, dst.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // ADD/SUB with more operand combinations
  // --------------------------------------------------------------------------

  void add( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void add( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void add( AsmRegister8 dst, int8_t imm ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM8_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void add( AsmRegister16 dst, int16_t imm ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM16_IMM16, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void sub( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void sub( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void sub( AsmRegister8 dst, int8_t imm ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM8_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void sub( AsmRegister16 dst, int16_t imm ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM16_IMM16, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void sub( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void sub( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // ADC (add with carry) instructions
  // --------------------------------------------------------------------------

  void adc( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::ADC_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void adc( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::ADC_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void adc( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::ADC_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void adc( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::ADC_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void adc( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::ADC_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void adc( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::ADC_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SBB (subtract with borrow) instructions
  // --------------------------------------------------------------------------

  void sbb( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::SBB_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void sbb( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::SBB_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void sbb( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::SBB_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void sbb( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::SBB_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void sbb( AsmRegister32 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::SBB_RM32_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  void sbb( AsmRegister64 dst, int32_t imm ) {
    auto instr = InstructionFactory::with2( Code::SBB_RM64_IMM32, dst.value, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // CMP with more sizes
  // --------------------------------------------------------------------------

  void cmp( AsmRegister8 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM8_R8, dst.value, src.value );
    add_instruction( instr );
  }

  void cmp( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM16_R16, dst.value, src.value );
    add_instruction( instr );
  }

  void cmp( AsmRegister8 dst, int8_t imm ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM8_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void cmp( AsmRegister16 dst, int16_t imm ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM16_IMM16, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // MOV with memory operands
  // --------------------------------------------------------------------------

  void mov( AsmRegister8 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOV_R8_RM8, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void mov( AsmRegister16 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOV_R16_RM16, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void mov( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOV_R32_RM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void mov( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOV_R64_RM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void mov( const AsmMemoryOperand& dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM8_R8, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void mov( const AsmMemoryOperand& dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM16_R16, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void mov( const AsmMemoryOperand& dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM32_R32, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void mov( const AsmMemoryOperand& dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::MOV_RM64_R64, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  // MOV immediate to register (more sizes)
  void mov( AsmRegister8 dst, int8_t imm ) {
    auto instr = InstructionFactory::with2( Code::MOV_R8_IMM8, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void mov( AsmRegister16 dst, int16_t imm ) {
    auto instr = InstructionFactory::with2( Code::MOV_R16_IMM16, dst.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // ADD/SUB/AND/OR/XOR/CMP with memory operands
  // --------------------------------------------------------------------------

  void add( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::ADD_R32_RM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void add( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::ADD_R64_RM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void add( const AsmMemoryOperand& dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM32_R32, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void add( const AsmMemoryOperand& dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::ADD_RM64_R64, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void sub( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::SUB_R32_RM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void sub( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::SUB_R64_RM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void sub( const AsmMemoryOperand& dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM32_R32, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void sub( const AsmMemoryOperand& dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::SUB_RM64_R64, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void and_( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::AND_R32_RM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void and_( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::AND_R64_RM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void or_( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::OR_R32_RM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void or_( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::OR_R64_RM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void xor_( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::XOR_R32_RM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void xor_( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::XOR_R64_RM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void cmp( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::CMP_R32_RM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void cmp( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::CMP_R64_RM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void cmp( const AsmMemoryOperand& dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM32_R32, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void cmp( const AsmMemoryOperand& dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMP_RM64_R64, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // PUSH/POP with memory operands
  // --------------------------------------------------------------------------

  void push( const AsmMemoryOperand& src ) {
    Code code = bitness_ == 64 ? Code::PUSH_RM64 :
                bitness_ == 32 ? Code::PUSH_RM32 : Code::PUSH_RM16;
    auto instr = InstructionFactory::with1( code, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void pop( const AsmMemoryOperand& dst ) {
    Code code = bitness_ == 64 ? Code::POP_RM64 :
                bitness_ == 32 ? Code::POP_RM32 : Code::POP_RM16;
    auto instr = InstructionFactory::with1( code, dst.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // PUSH immediate
  void push( int8_t imm ) {
    Code code = bitness_ == 64 ? Code::PUSHQ_IMM8 :
                bitness_ == 32 ? Code::PUSHD_IMM8 : Code::PUSHW_IMM8;
    auto instr = InstructionFactory::with1( code, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void push( int32_t imm ) {
    Code code = bitness_ == 64 ? Code::PUSHQ_IMM32 :
                bitness_ == 32 ? Code::PUSHD_IMM32 : Code::PUSH_IMM16;
    auto instr = InstructionFactory::with1( code, imm );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // CALL/JMP with memory operands
  // --------------------------------------------------------------------------

  void call( const AsmMemoryOperand& target ) {
    Code code = bitness_ == 64 ? Code::CALL_RM64 :
                bitness_ == 32 ? Code::CALL_RM32 : Code::CALL_RM16;
    auto instr = InstructionFactory::with1( code, target.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void jmp( AsmRegister64 reg ) {
    auto instr = InstructionFactory::with1( Code::JMP_RM64, reg.value );
    add_instruction( instr );
  }

  void jmp( AsmRegister32 reg ) {
    auto instr = InstructionFactory::with1( Code::JMP_RM32, reg.value );
    add_instruction( instr );
  }

  void jmp( const AsmMemoryOperand& target ) {
    Code code = bitness_ == 64 ? Code::JMP_RM64 :
                bitness_ == 32 ? Code::JMP_RM32 : Code::JMP_RM16;
    auto instr = InstructionFactory::with1( code, target.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // INC/DEC with memory operands
  // --------------------------------------------------------------------------

  void inc( const AsmMemoryOperand& dst, MemorySize size ) {
    Code code;
    switch ( size ) {
      case MemorySize::UINT8:
      case MemorySize::INT8:
        code = Code::INC_RM8;
        break;
      case MemorySize::UINT16:
      case MemorySize::INT16:
        code = Code::INC_RM16;
        break;
      case MemorySize::UINT32:
      case MemorySize::INT32:
        code = Code::INC_RM32;
        break;
      default:
        code = Code::INC_RM64;
        break;
    }
    auto instr = InstructionFactory::with1( code, dst.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void dec( const AsmMemoryOperand& dst, MemorySize size ) {
    Code code;
    switch ( size ) {
      case MemorySize::UINT8:
      case MemorySize::INT8:
        code = Code::DEC_RM8;
        break;
      case MemorySize::UINT16:
      case MemorySize::INT16:
        code = Code::DEC_RM16;
        break;
      case MemorySize::UINT32:
      case MemorySize::INT32:
        code = Code::DEC_RM32;
        break;
      default:
        code = Code::DEC_RM64;
        break;
    }
    auto instr = InstructionFactory::with1( code, dst.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // LOOP instructions
  // --------------------------------------------------------------------------

  void loop( CodeLabel label ) {
    Code code = bitness_ == 64 ? Code::LOOP_REL8_64_RCX :
                bitness_ == 32 ? Code::LOOP_REL8_32_ECX : Code::LOOP_REL8_16_CX;
    auto instr = InstructionFactory::with_branch( code, label.id() );
    add_instruction( instr );
  }

  void loope( CodeLabel label ) {
    Code code = bitness_ == 64 ? Code::LOOPE_REL8_64_RCX :
                bitness_ == 32 ? Code::LOOPE_REL8_32_ECX : Code::LOOPE_REL8_16_CX;
    auto instr = InstructionFactory::with_branch( code, label.id() );
    add_instruction( instr );
  }

  void loopz( CodeLabel label ) { loope( label ); }

  void loopne( CodeLabel label ) {
    Code code = bitness_ == 64 ? Code::LOOPNE_REL8_64_RCX :
                bitness_ == 32 ? Code::LOOPNE_REL8_32_ECX : Code::LOOPNE_REL8_16_CX;
    auto instr = InstructionFactory::with_branch( code, label.id() );
    add_instruction( instr );
  }

  void loopnz( CodeLabel label ) { loopne( label ); }

  // --------------------------------------------------------------------------
  // ENTER instruction
  // --------------------------------------------------------------------------

  void enter( uint16_t alloc_size, uint8_t nesting_level ) {
    Code code = bitness_ == 64 ? Code::ENTERQ_IMM16_IMM8 :
                bitness_ == 32 ? Code::ENTERD_IMM16_IMM8 : Code::ENTERW_IMM16_IMM8;
    auto instr = InstructionFactory::with2( code, static_cast<int32_t>( alloc_size ),
                                            static_cast<int32_t>( nesting_level ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // RET with immediate (stack cleanup)
  // --------------------------------------------------------------------------

  void ret( uint16_t imm ) {
    Code code = bitness_ == 64 ? Code::RETNQ_IMM16 :
                bitness_ == 32 ? Code::RETND_IMM16 : Code::RETNW_IMM16;
    auto instr = InstructionFactory::with1( code, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // INT instruction
  // --------------------------------------------------------------------------

  void int_( uint8_t vector ) {
    auto instr = InstructionFactory::with1( Code::INT_IMM8, static_cast<int32_t>( vector ) );
    add_instruction( instr );
  }

  void into() {
    auto instr = InstructionFactory::with( Code::INTO );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // BSWAP instruction
  // --------------------------------------------------------------------------

  void bswap( AsmRegister32 dst ) {
    auto instr = InstructionFactory::with1( Code::BSWAP_R32, dst.value );
    add_instruction( instr );
  }

  void bswap( AsmRegister64 dst ) {
    auto instr = InstructionFactory::with1( Code::BSWAP_R64, dst.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // XADD/CMPXCHG instructions (atomic operations)
  // --------------------------------------------------------------------------

  void xadd( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::XADD_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void xadd( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::XADD_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void xadd( const AsmMemoryOperand& dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::XADD_RM32_R32, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void xadd( const AsmMemoryOperand& dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::XADD_RM64_R64, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void cmpxchg( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMPXCHG_RM32_R32, dst.value, src.value );
    add_instruction( instr );
  }

  void cmpxchg( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMPXCHG_RM64_R64, dst.value, src.value );
    add_instruction( instr );
  }

  void cmpxchg( const AsmMemoryOperand& dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CMPXCHG_RM32_R32, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void cmpxchg( const AsmMemoryOperand& dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CMPXCHG_RM64_R64, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void cmpxchg8b( const AsmMemoryOperand& dst ) {
    auto instr = InstructionFactory::with1( Code::CMPXCHG8B_M64, dst.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void cmpxchg16b( const AsmMemoryOperand& dst ) {
    auto instr = InstructionFactory::with1( Code::CMPXCHG16B_M128, dst.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // Basic SSE move instructions
  // --------------------------------------------------------------------------

  void movss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void movss( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVSS_XMM_XMMM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movss( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVSS_XMMM32_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void movsd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVSD_XMM_XMMM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movsd( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVSD_XMMM64_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movaps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVAPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void movaps( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVAPS_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movaps( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVAPS_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movups( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVUPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void movups( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVUPS_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movups( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVUPS_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movapd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVAPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void movapd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVAPD_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movapd( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVAPD_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movupd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVUPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void movupd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVUPD_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movupd( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVUPD_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movdqa( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVDQA_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void movdqa( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVDQA_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movdqa( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVDQA_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movdqu( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVDQU_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void movdqu( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVDQU_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movdqu( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVDQU_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // MOVD/MOVQ - move between GP and XMM registers
  // --------------------------------------------------------------------------

  void movd( AsmRegisterXmm dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::MOVD_XMM_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void movd( AsmRegister32 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVD_RM32_XMM, dst.value, src.value );
    add_instruction( instr );
  }

  void movq( AsmRegisterXmm dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::MOVQ_XMM_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void movq( AsmRegister64 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVQ_RM64_XMM, dst.value, src.value );
    add_instruction( instr );
  }

  void movq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MOVQ_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // Basic SSE arithmetic
  // --------------------------------------------------------------------------

  void addss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ADDSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void addss( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::ADDSS_XMM_XMMM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void addsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ADDSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void addsd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::ADDSD_XMM_XMMM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void subss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SUBSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void subss( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::SUBSS_XMM_XMMM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void subsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SUBSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void subsd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::SUBSD_XMM_XMMM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void mulss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MULSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void mulss( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MULSS_XMM_XMMM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void mulsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MULSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void mulsd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MULSD_XMM_XMMM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void divss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::DIVSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void divss( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::DIVSS_XMM_XMMM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void divsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::DIVSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void divsd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::DIVSD_XMM_XMMM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SSE packed arithmetic
  // --------------------------------------------------------------------------

  void addps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ADDPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void addps( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::ADDPS_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void addpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ADDPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void addpd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::ADDPD_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void subps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SUBPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void subpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SUBPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void mulps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MULPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void mulpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MULPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void divps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::DIVPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void divpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::DIVPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SSE comparison
  // --------------------------------------------------------------------------

  void ucomiss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::UCOMISS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void ucomiss( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::UCOMISS_XMM_XMMM32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void ucomisd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::UCOMISD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void ucomisd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::UCOMISD_XMM_XMMM64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SSE conversion
  // --------------------------------------------------------------------------

  void cvtsi2ss( AsmRegisterXmm dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CVTSI2SS_XMM_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtsi2ss( AsmRegisterXmm dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CVTSI2SS_XMM_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtsi2sd( AsmRegisterXmm dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CVTSI2SD_XMM_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtsi2sd( AsmRegisterXmm dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CVTSI2SD_XMM_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtss2si( AsmRegister32 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTSS2SI_R32_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtss2si( AsmRegister64 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTSS2SI_R64_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtsd2si( AsmRegister32 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTSD2SI_R32_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtsd2si( AsmRegister64 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTSD2SI_R64_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtss2sd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTSS2SD_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cvtsd2ss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTSD2SS_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cvttss2si( AsmRegister32 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTTSS2SI_R32_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cvttss2si( AsmRegister64 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTTSS2SI_R64_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void cvttsd2si( AsmRegister32 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTTSD2SI_R32_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void cvttsd2si( AsmRegister64 dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::CVTTSD2SI_R64_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SSE bitwise operations
  // --------------------------------------------------------------------------

  void xorps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::XORPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void xorps( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::XORPS_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void xorpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::XORPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void xorpd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::XORPD_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void andps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ANDPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void andpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ANDPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void orps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ORPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void orpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::ORPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // PXOR/PAND/POR (integer SSE)
  // --------------------------------------------------------------------------

  void pxor( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PXOR_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void pxor( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::PXOR_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void pand( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PAND_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void por( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::POR_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SSE shuffle
  // --------------------------------------------------------------------------

  void pshufd( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm ) {
    auto instr = InstructionFactory::with3( Code::PSHUFD_XMM_XMMM128_IMM8, dst.value, src.value,
                                            static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void shufps( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm ) {
    auto instr = InstructionFactory::with3( Code::SHUFPS_XMM_XMMM128_IMM8, dst.value, src.value,
                                            static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void shufpd( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm ) {
    auto instr = InstructionFactory::with3( Code::SHUFPD_XMM_XMMM128_IMM8, dst.value, src.value,
                                            static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SSE sqrt/rsqrt/rcp
  // --------------------------------------------------------------------------

  void sqrtss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SQRTSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void sqrtsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SQRTSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void sqrtps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SQRTPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void sqrtpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SQRTPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void rsqrtss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::RSQRTSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void rsqrtps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::RSQRTPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void rcpss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::RCPSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void rcpps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::RCPPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // SSE min/max
  // --------------------------------------------------------------------------

  void minss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MINSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void minsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MINSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void minps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MINPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void minpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MINPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void maxss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MAXSS_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void maxsd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MAXSD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void maxps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MAXPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void maxpd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::MAXPD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // AVX (VEX-encoded) instructions
  // ==========================================================================

  // --------------------------------------------------------------------------
  // AVX move instructions
  // --------------------------------------------------------------------------

  void vmovss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMOVSS_XMM_XMM_XMM, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmovss( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVSS_XMM_M32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovss( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVSS_M32_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovsd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMOVSD_XMM_XMM_XMM, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmovsd( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVSD_XMM_M64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovsd( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVSD_M64_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovaps( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVAPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vmovaps( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVAPS_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovaps( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVAPS_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovaps( AsmRegisterYmm dst, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVAPS_YMM_YMMM256, dst.value, src.value );
    add_instruction( instr );
  }

  void vmovaps( AsmRegisterYmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVAPS_YMM_YMMM256, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovaps( const AsmMemoryOperand& dst, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVAPS_YMMM256_YMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovups( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVUPS_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vmovups( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVUPS_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovups( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVUPS_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovups( AsmRegisterYmm dst, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVUPS_YMM_YMMM256, dst.value, src.value );
    add_instruction( instr );
  }

  void vmovups( AsmRegisterYmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVUPS_YMM_YMMM256, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovups( const AsmMemoryOperand& dst, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVUPS_YMMM256_YMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovdqa( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQA_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vmovdqa( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQA_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovdqa( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQA_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovdqa( AsmRegisterYmm dst, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQA_YMM_YMMM256, dst.value, src.value );
    add_instruction( instr );
  }

  void vmovdqu( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQU_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vmovdqu( AsmRegisterXmm dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQU_XMM_XMMM128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmovdqu( const AsmMemoryOperand& dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQU_XMMM128_XMM, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void vmovdqu( AsmRegisterYmm dst, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VMOVDQU_YMM_YMMM256, dst.value, src.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // AVX scalar arithmetic
  // --------------------------------------------------------------------------

  void vaddss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VADDSS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaddsd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VADDSD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vsubss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VSUBSS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vsubsd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VSUBSD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmulss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMULSS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmulsd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMULSD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vdivss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VDIVSS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vdivsd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VDIVSD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // AVX packed arithmetic (XMM and YMM)
  // --------------------------------------------------------------------------

  void vaddps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VADDPS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaddps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VADDPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaddpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VADDPD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaddpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VADDPD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vsubps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VSUBPS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vsubps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VSUBPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vsubpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VSUBPD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vsubpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VSUBPD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmulps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMULPS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmulps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMULPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmulpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMULPD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vmulpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMULPD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vdivps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VDIVPS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vdivps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VDIVPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vdivpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VDIVPD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vdivpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VDIVPD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // AVX bitwise operations
  // --------------------------------------------------------------------------

  void vxorps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VXORPS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vxorps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VXORPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vxorpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VXORPD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vxorpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VXORPD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vandps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VANDPS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vandps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VANDPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vandpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VANDPD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vandpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VANDPD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vorps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VORPS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vorps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VORPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vorpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VORPD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vorpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VORPD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // AVX integer XOR/AND/OR
  // --------------------------------------------------------------------------

  void vpxor( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPXOR_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpxor( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPXOR_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpand( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPAND_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpand( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPAND_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpor( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPOR_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpor( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPOR_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // AVX comparison
  // --------------------------------------------------------------------------

  void vucomiss( AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with2( Code::VEX_VUCOMISS_XMM_XMMM32, src1.value, src2.value );
    add_instruction( instr );
  }

  void vucomisd( AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with2( Code::VEX_VUCOMISD_XMM_XMMM64, src1.value, src2.value );
    add_instruction( instr );
  }

  // --------------------------------------------------------------------------
  // VZEROUPPER/VZEROALL
  // --------------------------------------------------------------------------

  void vzeroupper() {
    auto instr = InstructionFactory::with( Code::VEX_VZEROUPPER );
    add_instruction( instr );
  }

  void vzeroall() {
    auto instr = InstructionFactory::with( Code::VEX_VZEROALL );
    add_instruction( instr );
  }

  // ==========================================================================
  // BMI1/BMI2 instructions
  // ==========================================================================

  void andn( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_ANDN_R32_R32_RM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void andn( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_ANDN_R64_R64_RM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void bextr( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_BEXTR_R32_RM32_R32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void bextr( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_BEXTR_R64_RM64_R64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void blsi( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::VEX_BLSI_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void blsi( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::VEX_BLSI_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void blsmsk( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::VEX_BLSMSK_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void blsmsk( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::VEX_BLSMSK_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void blsr( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::VEX_BLSR_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void blsr( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::VEX_BLSR_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  // BMI2
  void bzhi( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_BZHI_R32_RM32_R32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void bzhi( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_BZHI_R64_RM64_R64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void mulx( AsmRegister32 dst1, AsmRegister32 dst2, AsmRegister32 src ) {
    auto instr = InstructionFactory::with3( Code::VEX_MULX_R32_R32_RM32, dst1.value, dst2.value, src.value );
    add_instruction( instr );
  }

  void mulx( AsmRegister64 dst1, AsmRegister64 dst2, AsmRegister64 src ) {
    auto instr = InstructionFactory::with3( Code::VEX_MULX_R64_R64_RM64, dst1.value, dst2.value, src.value );
    add_instruction( instr );
  }

  void pdep( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_PDEP_R32_R32_RM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void pdep( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_PDEP_R64_R64_RM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void pext( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_PEXT_R32_R32_RM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void pext( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_PEXT_R64_R64_RM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void rorx( AsmRegister32 dst, AsmRegister32 src, uint8_t imm ) {
    auto instr = InstructionFactory::with3( Code::VEX_RORX_R32_RM32_IMM8, dst.value, src.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void rorx( AsmRegister64 dst, AsmRegister64 src, uint8_t imm ) {
    auto instr = InstructionFactory::with3( Code::VEX_RORX_R64_RM64_IMM8, dst.value, src.value, static_cast<int32_t>( imm ) );
    add_instruction( instr );
  }

  void sarx( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_SARX_R32_RM32_R32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void sarx( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_SARX_R64_RM64_R64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void shlx( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_SHLX_R32_RM32_R32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void shlx( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_SHLX_R64_RM64_R64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void shrx( AsmRegister32 dst, AsmRegister32 src1, AsmRegister32 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_SHRX_R32_RM32_R32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void shrx( AsmRegister64 dst, AsmRegister64 src1, AsmRegister64 src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_SHRX_R64_RM64_R64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // CRC32 instruction
  // ==========================================================================

  void crc32( AsmRegister32 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::CRC32_R32_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void crc32( AsmRegister32 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::CRC32_R32_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  void crc32( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::CRC32_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void crc32( AsmRegister64 dst, AsmRegister8 src ) {
    auto instr = InstructionFactory::with2( Code::CRC32_R64_RM8, dst.value, src.value );
    add_instruction( instr );
  }

  void crc32( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::CRC32_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // MOVBE instruction (move with byte swap)
  // ==========================================================================

  void movbe( AsmRegister16 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVBE_R16_M16, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movbe( AsmRegister32 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVBE_R32_M32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movbe( AsmRegister64 dst, const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with2( Code::MOVBE_R64_M64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void movbe( const AsmMemoryOperand& dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::MOVBE_M16_R16, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movbe( const AsmMemoryOperand& dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::MOVBE_M32_R32, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  void movbe( const AsmMemoryOperand& dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::MOVBE_M64_R64, dst.to_memory_operand( bitness_ ), src.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // PREFETCH instructions
  // ==========================================================================

  void prefetchnta( const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with1( Code::PREFETCHNTA_M8, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void prefetcht0( const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with1( Code::PREFETCHT0_M8, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void prefetcht1( const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with1( Code::PREFETCHT1_M8, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void prefetcht2( const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with1( Code::PREFETCHT2_M8, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void prefetchw( const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with1( Code::PREFETCHW_M8, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // ==========================================================================
  // CLFLUSH/CLFLUSHOPT instructions
  // ==========================================================================

  void clflush( const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with1( Code::CLFLUSH_M8, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void clflushopt( const AsmMemoryOperand& src ) {
    auto instr = InstructionFactory::with1( Code::CLFLUSHOPT_M8, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // ==========================================================================
  // RDRAND/RDSEED instructions
  // ==========================================================================

  void rdrand( AsmRegister16 dst ) {
    auto instr = InstructionFactory::with1( Code::RDRAND_R16, dst.value );
    add_instruction( instr );
  }

  void rdrand( AsmRegister32 dst ) {
    auto instr = InstructionFactory::with1( Code::RDRAND_R32, dst.value );
    add_instruction( instr );
  }

  void rdrand( AsmRegister64 dst ) {
    auto instr = InstructionFactory::with1( Code::RDRAND_R64, dst.value );
    add_instruction( instr );
  }

  void rdseed( AsmRegister16 dst ) {
    auto instr = InstructionFactory::with1( Code::RDSEED_R16, dst.value );
    add_instruction( instr );
  }

  void rdseed( AsmRegister32 dst ) {
    auto instr = InstructionFactory::with1( Code::RDSEED_R32, dst.value );
    add_instruction( instr );
  }

  void rdseed( AsmRegister64 dst ) {
    auto instr = InstructionFactory::with1( Code::RDSEED_R64, dst.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // XGETBV/XSETBV instructions
  // ==========================================================================

  void xgetbv() {
    auto instr = InstructionFactory::with( Code::XGETBV );
    add_instruction( instr );
  }

  void xsetbv() {
    auto instr = InstructionFactory::with( Code::XSETBV );
    add_instruction( instr );
  }

  // ==========================================================================
  // FMA3 instructions (Fused Multiply-Add)
  // ==========================================================================

  // VFMADD132 - Fused Multiply-Add (a = a*c + b)
  void vfmadd132ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD132PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd132ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD132PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd132pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD132PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd132pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD132PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd132ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD132SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd132sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD132SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // VFMADD213 - Fused Multiply-Add (a = b*a + c)
  void vfmadd213ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD213PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd213ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD213PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd213pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD213PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd213pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD213PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd213ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD213SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd213sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD213SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // VFMADD231 - Fused Multiply-Add (a = b*c + a)
  void vfmadd231ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD231PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd231ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD231PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd231pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD231PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd231pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD231PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd231ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD231SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmadd231sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADD231SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // VFMSUB - Fused Multiply-Subtract (a*b - c, b*a - c, b*c - a)
  void vfmsub132ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB132PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub132ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB132PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub132pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB132PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub132pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB132PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub132ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB132SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub132sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB132SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub213ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB213PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub213ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB213PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub213pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB213PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub213pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB213PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub213ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB213SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub213sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB213SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub231ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB231PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub231ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB231PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub231pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB231PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub231pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB231PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub231ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB231SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsub231sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUB231SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // VFNMADD - Fused Negative Multiply-Add (-(a*c) + b, -(b*a) + c, -(b*c) + a)
  void vfnmadd132ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD132PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd132ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD132PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd132pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD132PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd132pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD132PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd132ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD132SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd132sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD132SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd213ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD213PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd213ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD213PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd213pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD213PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd213pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD213PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd213ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD213SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd213sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD213SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd231ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD231PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd231ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD231PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd231pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD231PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd231pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD231PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd231ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD231SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmadd231sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMADD231SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // VFNMSUB - Fused Negative Multiply-Subtract (-(a*c) - b, -(b*a) - c, -(b*c) - a)
  void vfnmsub132ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB132PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub132ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB132PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub132pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB132PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub132pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB132PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub132ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB132SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub132sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB132SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub213ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB213PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub213ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB213PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub213pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB213PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub213pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB213PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub213ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB213SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub213sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB213SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub231ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB231PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub231ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB231PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub231pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB231PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub231pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB231PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub231ss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB231SS_XMM_XMM_XMMM32, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfnmsub231sd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFNMSUB231SD_XMM_XMM_XMMM64, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // VFMADDSUB/VFMSUBADD - Fused Multiply-Alternating Add/Sub
  void vfmaddsub132ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB132PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub132ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB132PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub132pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB132PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub132pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB132PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub213ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB213PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub213ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB213PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub213pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB213PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub213pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB213PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub231ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB231PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub231ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB231PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub231pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB231PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmaddsub231pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMADDSUB231PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd132ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD132PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd132ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD132PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd132pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD132PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd132pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD132PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd213ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD213PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd213ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD213PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd213pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD213PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd213pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD213PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd231ps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD231PS_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd231ps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD231PS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd231pd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD231PD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vfmsubadd231pd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VFMSUBADD231PD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // AES-NI instructions
  // ==========================================================================

  // Legacy SSE AES instructions
  void aesenc( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::AESENC_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void aesenclast( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::AESENCLAST_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void aesdec( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::AESDEC_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void aesdeclast( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::AESDECLAST_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void aesimc( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::AESIMC_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void aeskeygenassist( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::AESKEYGENASSIST_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  // VEX-encoded AES instructions
  void vaesenc( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESENC_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesenc( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESENC_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesenclast( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESENCLAST_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesenclast( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESENCLAST_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesdec( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESDEC_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesdec( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESDEC_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesdeclast( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESDECLAST_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesdeclast( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESDECLAST_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vaesimc( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VAESIMC_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vaeskeygenassist( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VAESKEYGENASSIST_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  // ==========================================================================
  // PCLMULQDQ instruction (Carry-less Multiplication)
  // ==========================================================================

  void pclmulqdq( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PCLMULQDQ_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpclmulqdq( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPCLMULQDQ_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vpclmulqdq( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPCLMULQDQ_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  // ==========================================================================
  // SSE4.1 instructions
  // ==========================================================================

  // ROUND instructions
  void roundps( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::ROUNDPS_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void roundpd( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::ROUNDPD_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void roundss( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::ROUNDSS_XMM_XMMM32_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void roundsd( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::ROUNDSD_XMM_XMMM64_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vroundps( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VROUNDPS_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vroundps( AsmRegisterYmm dst, AsmRegisterYmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VROUNDPS_YMM_YMMM256_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vroundpd( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VROUNDPD_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vroundpd( AsmRegisterYmm dst, AsmRegisterYmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VROUNDPD_YMM_YMMM256_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vroundss( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VROUNDSS_XMM_XMM_XMMM32_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vroundsd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VROUNDSD_XMM_XMM_XMMM64_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  // BLEND instructions
  void blendps( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::BLENDPS_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void blendpd( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::BLENDPD_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vblendps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VBLENDPS_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vblendps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VBLENDPS_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vblendpd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VBLENDPD_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vblendpd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VBLENDPD_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void pblendw( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PBLENDW_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpblendw( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPBLENDW_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vpblendw( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPBLENDW_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vpblendd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPBLENDD_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vpblendd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPBLENDD_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  // INSERT/EXTRACT instructions
  void insertps( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::INSERTPS_XMM_XMMM32_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vinsertps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VINSERTPS_XMM_XMM_XMMM32_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void extractps( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::EXTRACTPS_RM32_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void extractps( AsmRegister64 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::EXTRACTPS_R64M32_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vextractps( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VEXTRACTPS_RM32_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vextractps( AsmRegister64 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VEXTRACTPS_R64M32_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  // PINSRB/W/D/Q - Insert byte/word/dword/qword
  void pinsrb( AsmRegisterXmm dst, AsmRegister32 src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PINSRB_XMM_R32M8_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void pinsrw( AsmRegisterXmm dst, AsmRegister32 src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PINSRW_XMM_R32M16_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void pinsrd( AsmRegisterXmm dst, AsmRegister32 src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PINSRD_XMM_RM32_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void pinsrq( AsmRegisterXmm dst, AsmRegister64 src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PINSRQ_XMM_RM64_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpinsrb( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegister32 src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPINSRB_XMM_XMM_R32M8_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vpinsrw( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegister32 src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPINSRW_XMM_XMM_R32M16_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vpinsrd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegister32 src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPINSRD_XMM_XMM_RM32_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vpinsrq( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegister64 src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPINSRQ_XMM_XMM_RM64_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  // PEXTRB/W/D/Q - Extract byte/word/dword/qword
  void pextrb( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PEXTRB_R32M8_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void pextrw( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PEXTRW_R32M16_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void pextrd( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PEXTRD_RM32_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void pextrq( AsmRegister64 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PEXTRQ_RM64_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpextrb( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPEXTRB_R32M8_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpextrw( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPEXTRW_R32M16_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpextrd( AsmRegister32 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPEXTRD_RM32_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpextrq( AsmRegister64 dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPEXTRQ_RM64_XMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  // PMOVSX/PMOVZX - Sign/Zero extend packed integers
  void pmovsxbw( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVSXBW_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovsxbd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVSXBD_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovsxbq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVSXBQ_XMM_XMMM16, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovsxwd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVSXWD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovsxwq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVSXWQ_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovsxdq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVSXDQ_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovzxbw( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVZXBW_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovzxbd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVZXBD_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovzxbq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVZXBQ_XMM_XMMM16, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovzxwd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVZXWD_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovzxwq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVZXWQ_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void pmovzxdq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMOVZXDQ_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  // Other SSE4.1 instructions
  void dpps( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::DPPS_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void dppd( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::DPPD_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vdpps( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VDPPS_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vdpps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VDPPS_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vdppd( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VDPPD_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void mpsadbw( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::MPSADBW_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vmpsadbw( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VMPSADBW_XMM_XMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vmpsadbw( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VMPSADBW_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  // PCMPEQQ/PCMPGTQ - Compare packed qwords
  void pcmpeqq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PCMPEQQ_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vpcmpeqq( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPCMPEQQ_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpcmpeqq( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPCMPEQQ_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void pcmpgtq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PCMPGTQ_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vpcmpgtq( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPCMPGTQ_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpcmpgtq( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPCMPGTQ_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // PTEST - Logical compare
  void ptest( AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with2( Code::PTEST_XMM_XMMM128, src1.value, src2.value );
    add_instruction( instr );
  }

  void vptest( AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPTEST_XMM_XMMM128, src1.value, src2.value );
    add_instruction( instr );
  }

  void vptest( AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPTEST_YMM_YMMM256, src1.value, src2.value );
    add_instruction( instr );
  }

  // PMULLD/PMULDQ - Multiply packed integers
  void pmulld( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMULLD_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vpmulld( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMULLD_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpmulld( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMULLD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void pmuldq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::PMULDQ_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void vpmuldq( AsmRegisterXmm dst, AsmRegisterXmm src1, AsmRegisterXmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMULDQ_XMM_XMM_XMMM128, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpmuldq( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMULDQ_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // SSE4.2 instructions
  // ==========================================================================

  // String comparison instructions
  void pcmpestri( AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PCMPESTRI_XMM_XMMM128_IMM8, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void pcmpestrm( AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PCMPESTRM_XMM_XMMM128_IMM8, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void pcmpistri( AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PCMPISTRI_XMM_XMMM128_IMM8, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void pcmpistrm( AsmRegisterXmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::PCMPISTRM_XMM_XMMM128_IMM8, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  // POPCNT - Population count (16-bit variant - 32/64-bit defined earlier)
  void popcnt( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::POPCNT_R16_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // AVX2 instructions
  // ==========================================================================

  // VPERM - Permute
  void vpermq( AsmRegisterYmm dst, AsmRegisterYmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPERMQ_YMM_YMMM256_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vpermd( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPERMD_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpermps( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPERMPS_YMM_YMM_YMMM256, dst.value, src1.value, src2.value );
    add_instruction( instr );
  }

  void vpermpd( AsmRegisterYmm dst, AsmRegisterYmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPERMPD_YMM_YMMM256_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vperm2i128( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPERM2I128_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vperm2f128( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterYmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VPERM2F128_YMM_YMM_YMMM256_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  // VPBROADCAST - Broadcast
  void vpbroadcastb( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTB_XMM_XMMM8, dst.value, src.value );
    add_instruction( instr );
  }

  void vpbroadcastb( AsmRegisterYmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTB_YMM_XMMM8, dst.value, src.value );
    add_instruction( instr );
  }

  void vpbroadcastw( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTW_XMM_XMMM16, dst.value, src.value );
    add_instruction( instr );
  }

  void vpbroadcastw( AsmRegisterYmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTW_YMM_XMMM16, dst.value, src.value );
    add_instruction( instr );
  }

  void vpbroadcastd( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTD_XMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void vpbroadcastd( AsmRegisterYmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTD_YMM_XMMM32, dst.value, src.value );
    add_instruction( instr );
  }

  void vpbroadcastq( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTQ_XMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void vpbroadcastq( AsmRegisterYmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VPBROADCASTQ_YMM_XMMM64, dst.value, src.value );
    add_instruction( instr );
  }

  void vbroadcastss( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VBROADCASTSS_XMM_XMM, dst.value, src.value );
    add_instruction( instr );
  }

  void vbroadcastss( AsmRegisterYmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VBROADCASTSS_YMM_XMM, dst.value, src.value );
    add_instruction( instr );
  }

  void vbroadcastss( AsmRegisterXmm dst, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VBROADCASTSS_XMM_M32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vbroadcastss( AsmRegisterYmm dst, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VBROADCASTSS_YMM_M32, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vbroadcastsd( AsmRegisterYmm dst, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VBROADCASTSD_YMM_M64, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vbroadcasti128( AsmRegisterYmm dst, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with2( Code::VEX_VBROADCASTI128_YMM_M128, dst.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  // VGATHER - Gather
  void vgatherdps( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VGATHERDPS_XMM_VM32X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vgatherdps( AsmRegisterYmm dst, AsmMemoryOperand src, AsmRegisterYmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VGATHERDPS_YMM_VM32Y_YMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vgatherdpd( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VGATHERDPD_XMM_VM32X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vgatherdpd( AsmRegisterYmm dst, AsmMemoryOperand src, AsmRegisterYmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VGATHERDPD_YMM_VM32X_YMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vgatherqps( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VGATHERQPS_XMM_VM64X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vgatherqpd( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VGATHERQPD_XMM_VM64X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vgatherqpd( AsmRegisterYmm dst, AsmMemoryOperand src, AsmRegisterYmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VGATHERQPD_YMM_VM64Y_YMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vpgatherdd( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPGATHERDD_XMM_VM32X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vpgatherdd( AsmRegisterYmm dst, AsmMemoryOperand src, AsmRegisterYmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPGATHERDD_YMM_VM32Y_YMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vpgatherdq( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPGATHERDQ_XMM_VM32X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vpgatherdq( AsmRegisterYmm dst, AsmMemoryOperand src, AsmRegisterYmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPGATHERDQ_YMM_VM32X_YMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vpgatherqd( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPGATHERQD_XMM_VM64X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vpgatherqq( AsmRegisterXmm dst, AsmMemoryOperand src, AsmRegisterXmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPGATHERQQ_XMM_VM64X_XMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  void vpgatherqq( AsmRegisterYmm dst, AsmMemoryOperand src, AsmRegisterYmm mask ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPGATHERQQ_YMM_VM64Y_YMM, dst.value, src.to_memory_operand( bitness_ ), mask.value );
    add_instruction( instr );
  }

  // VINSERTI128/VEXTRACTI128
  void vinserti128( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VINSERTI128_YMM_YMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vextracti128( AsmRegisterXmm dst, AsmRegisterYmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VEXTRACTI128_XMMM128_YMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void vinsertf128( AsmRegisterYmm dst, AsmRegisterYmm src1, AsmRegisterXmm src2, uint8_t imm8 ) {
    auto instr = InstructionFactory::with4( Code::VEX_VINSERTF128_YMM_YMM_XMMM128_IMM8, dst.value, src1.value, src2.value, imm8 );
    add_instruction( instr );
  }

  void vextractf128( AsmRegisterXmm dst, AsmRegisterYmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::VEX_VEXTRACTF128_XMMM128_YMM_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  // VMASKMOV - Conditional load/store
  void vmaskmovps( AsmRegisterXmm dst, AsmRegisterXmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPS_XMM_XMM_M128, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmaskmovps( AsmRegisterYmm dst, AsmRegisterYmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPS_YMM_YMM_M256, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmaskmovps( AsmMemoryOperand dst, AsmRegisterXmm mask, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPS_M128_XMM_XMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  void vmaskmovps( AsmMemoryOperand dst, AsmRegisterYmm mask, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPS_M256_YMM_YMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  void vmaskmovpd( AsmRegisterXmm dst, AsmRegisterXmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPD_XMM_XMM_M128, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmaskmovpd( AsmRegisterYmm dst, AsmRegisterYmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPD_YMM_YMM_M256, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vmaskmovpd( AsmMemoryOperand dst, AsmRegisterXmm mask, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPD_M128_XMM_XMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  void vmaskmovpd( AsmMemoryOperand dst, AsmRegisterYmm mask, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VMASKMOVPD_M256_YMM_YMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  // VPMASKMOV - Integer conditional load/store
  void vpmaskmovd( AsmRegisterXmm dst, AsmRegisterXmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVD_XMM_XMM_M128, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vpmaskmovd( AsmRegisterYmm dst, AsmRegisterYmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVD_YMM_YMM_M256, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vpmaskmovd( AsmMemoryOperand dst, AsmRegisterXmm mask, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVD_M128_XMM_XMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  void vpmaskmovd( AsmMemoryOperand dst, AsmRegisterYmm mask, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVD_M256_YMM_YMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  void vpmaskmovq( AsmRegisterXmm dst, AsmRegisterXmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVQ_XMM_XMM_M128, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vpmaskmovq( AsmRegisterYmm dst, AsmRegisterYmm mask, AsmMemoryOperand src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVQ_YMM_YMM_M256, dst.value, mask.value, src.to_memory_operand( bitness_ ) );
    add_instruction( instr );
  }

  void vpmaskmovq( AsmMemoryOperand dst, AsmRegisterXmm mask, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVQ_M128_XMM_XMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  void vpmaskmovq( AsmMemoryOperand dst, AsmRegisterYmm mask, AsmRegisterYmm src ) {
    auto instr = InstructionFactory::with3( Code::VEX_VPMASKMOVQ_M256_YMM_YMM, dst.to_memory_operand( bitness_ ), mask.value, src.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // ADX instructions (Multi-Precision Add-Carry)
  // ==========================================================================

  void adcx( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::ADCX_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void adcx( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::ADCX_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  void adox( AsmRegister32 dst, AsmRegister32 src ) {
    auto instr = InstructionFactory::with2( Code::ADOX_R32_RM32, dst.value, src.value );
    add_instruction( instr );
  }

  void adox( AsmRegister64 dst, AsmRegister64 src ) {
    auto instr = InstructionFactory::with2( Code::ADOX_R64_RM64, dst.value, src.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // SHA instructions (Secure Hash Algorithm)
  // ==========================================================================

  void sha1nexte( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SHA1NEXTE_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void sha1msg1( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SHA1MSG1_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void sha1msg2( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SHA1MSG2_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void sha1rnds4( AsmRegisterXmm dst, AsmRegisterXmm src, uint8_t imm8 ) {
    auto instr = InstructionFactory::with3( Code::SHA1RNDS4_XMM_XMMM128_IMM8, dst.value, src.value, imm8 );
    add_instruction( instr );
  }

  void sha256rnds2( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SHA256RNDS2_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void sha256msg1( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SHA256MSG1_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  void sha256msg2( AsmRegisterXmm dst, AsmRegisterXmm src ) {
    auto instr = InstructionFactory::with2( Code::SHA256MSG2_XMM_XMMM128, dst.value, src.value );
    add_instruction( instr );
  }

  // ==========================================================================
  // LZCNT/TZCNT instructions (Bit manipulation) - 16-bit variants
  // Note: 32/64-bit variants defined earlier in the file
  // ==========================================================================

  void lzcnt( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::LZCNT_R16_RM16, dst.value, src.value );
    add_instruction( instr );
  }

  void tzcnt( AsmRegister16 dst, AsmRegister16 src ) {
    auto instr = InstructionFactory::with2( Code::TZCNT_R16_RM16, dst.value, src.value );
    add_instruction( instr );
  }

private:
  void verify_no_prefixes( const char* mnemonic ) {
    if ( prefix_flags_ != AsmPrefixFlags::NONE ) {
      throw std::runtime_error( std::string( mnemonic ) + ": No prefixes are allowed" );
    }
  }

  void jcc( Code short_code, Code near_code, CodeLabel label ) {
    Code code = prefer_short_branch() ? short_code : near_code;
    auto instr = InstructionFactory::with_branch( code, label.id() );
    add_instruction( instr );
  }

  [[nodiscard]] bool instruction_prefer_vex() const noexcept {
    if ( ( prefix_flags_ & ( AsmPrefixFlags::PREFER_VEX | AsmPrefixFlags::PREFER_EVEX ) ) != 0 ) {
      return ( prefix_flags_ & AsmPrefixFlags::PREFER_VEX ) != 0;
    }
    return ( options_ & AsmOptions::PREFER_VEX ) != 0;
  }

  uint32_t bitness_;
  uint8_t options_;
  uint8_t prefix_flags_ = AsmPrefixFlags::NONE;
  uint64_t current_label_id_ = 0;
  CodeLabel current_label_;
  CodeLabel current_anon_label_;
  CodeLabel next_anon_label_;
  bool defined_anon_label_ = false;
  std::vector<Instruction> instructions_;
};

} // namespace iced_x86

#endif // ICED_X86_CODE_ASSEMBLER_HPP
