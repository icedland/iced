// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/internal/handlers.hpp"
#include "iced_x86/decoder.hpp"

namespace iced_x86 {
namespace internal {

// Static instances for special handlers
const OpCodeHandler_Invalid g_null_handler{ true };
const OpCodeHandler_Invalid g_invalid_handler{ true };
const OpCodeHandler_Invalid g_invalid_no_modrm_handler{ false };

// ============================================================================
// Helper: get operand size index (0=16, 1=32, 2=64)
// ============================================================================
static inline std::size_t get_op_size_index( const Decoder& decoder ) {
  return static_cast<std::size_t>( decoder.state().operand_size );
}

// ============================================================================
// Helper: get register base for operand size
// ============================================================================
static inline Register get_gpr_base( OpSize op_size ) {
  switch ( op_size ) {
    case OpSize::SIZE16: return Register::AX;
    case OpSize::SIZE32: return Register::EAX;
    case OpSize::SIZE64: return Register::RAX;
    default: return Register::EAX;
  }
}

// Helper to add register index to base register
static inline Register add_reg( Register base, uint32_t index ) {
  return static_cast<Register>( static_cast<uint32_t>( base ) + index );
}

// ============================================================================
// Invalid Handler
// ============================================================================

void OpCodeHandler_Invalid::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& /*instruction*/ ) {
  decoder.set_invalid_instruction();
}

// ============================================================================
// Simple Handler
// ============================================================================

void OpCodeHandler_Simple::decode( const OpCodeHandler* self_ptr, Decoder& /*decoder*/, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple*>( self_ptr );
  instruction.set_code( self->code );
}

// ============================================================================
// Group Handler
// ============================================================================

void OpCodeHandler_Group::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Group*>( self_ptr );
  auto reg = decoder.state().reg;
  auto& entry = self->group_handlers[reg];
  entry.decode( entry.handler, decoder, instruction );
}

// ============================================================================
// Group8x8 Handler
// ============================================================================

void OpCodeHandler_Group8x8::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Group8x8*>( self_ptr );
  auto reg = decoder.state().reg;
  auto mod_ = decoder.state().mod_;

  const HandlerEntry* entry;
  if ( mod_ == 3 ) {
    entry = &self->table_high[reg];
  } else {
    entry = &self->table_low[reg];
  }
  entry->decode( entry->handler, decoder, instruction );
}

// ============================================================================
// Group8x64 Handler
// ============================================================================

void OpCodeHandler_Group8x64::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Group8x64*>( self_ptr );
  auto mod_ = decoder.state().mod_;
  auto reg = decoder.state().reg;
  auto modrm = decoder.state().modrm;

  const HandlerEntry* entry;
  if ( mod_ == 3 ) {
    entry = &self->table_high[modrm & 0x3F];
    // Check for null handler - fall back to table_low
    if ( is_null_instance_handler( entry->handler ) ) {
      entry = &self->table_low[reg];
    }
  } else {
    entry = &self->table_low[reg];
  }
  entry->decode( entry->handler, decoder, instruction );
}

// ============================================================================
// AnotherTable Handler
// ============================================================================

void OpCodeHandler_AnotherTable::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_AnotherTable*>( self_ptr );

  auto byte_opt = decoder.read_byte();
  if ( !byte_opt ) {
    decoder.set_invalid_instruction();
    return;
  }

  auto b = *byte_opt;
  auto& entry = self->handlers[b];

  // Read modrm if required and not already read
  if ( entry.handler->has_modrm && !decoder.state().modrm_read ) {
    auto modrm_opt = decoder.read_byte();
    if ( !modrm_opt ) {
      decoder.set_invalid_instruction();
      return;
    }
    auto m = static_cast<uint32_t>( *modrm_opt );
    decoder.state().modrm = m;
    decoder.state().reg = ( m >> 3 ) & 7;
    decoder.state().mod_ = m >> 6;
    decoder.state().rm = m & 7;
    decoder.state().modrm_read = true;
  }

  entry.decode( entry.handler, decoder, instruction );
}

// ============================================================================
// RM Handler
// ============================================================================

void OpCodeHandler_RM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_RM*>( self_ptr );
  auto mod_ = decoder.state().mod_;

  const HandlerEntry* entry;
  if ( mod_ == 3 ) {
    entry = &self->handler_reg;
  } else {
    entry = &self->handler_mem;
  }
  entry->decode( entry->handler, decoder, instruction );
}

// ============================================================================
// Bitness Handler
// ============================================================================

void OpCodeHandler_Bitness::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Bitness*>( self_ptr );

  const HandlerEntry* entry;
  if ( decoder.bitness() == 64 ) {
    entry = &self->handler_64;
  } else {
    entry = &self->handler_1632;
  }
  // Read modrm unconditionally if sub-handler needs it (like Rust behavior)
  if ( entry->handler->has_modrm ) {
    decoder.read_modrm();
  }
  entry->decode( entry->handler, decoder, instruction );
}

void OpCodeHandler_Bitness_DontReadModRM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Bitness_DontReadModRM*>( self_ptr );

  const HandlerEntry* entry;
  if ( decoder.bitness() == 64 ) {
    entry = &self->handler_64;
  } else {
    entry = &self->handler_1632;
  }
  // DontReadModRM - call directly without reading modrm
  entry->decode( entry->handler, decoder, instruction );
}

// ============================================================================
// MandatoryPrefix Handler
// ============================================================================

void OpCodeHandler_MandatoryPrefix::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_MandatoryPrefix*>( self_ptr );
  auto prefix_idx = static_cast<size_t>( decoder.state().mandatory_prefix );
  auto& entry = self->handlers[prefix_idx];
  // Call directly - modrm was already read by caller if needed
  entry.decode( entry.handler, decoder, instruction );
}

void OpCodeHandler_MandatoryPrefix3::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_MandatoryPrefix3*>( self_ptr );
  auto prefix_idx = static_cast<size_t>( decoder.state().mandatory_prefix );
  auto mod_ = decoder.state().mod_;

  const HandlerEntry* entry;
  if ( mod_ == 3 ) {
    entry = &self->handlers_reg[prefix_idx];
  } else {
    entry = &self->handlers_mem[prefix_idx];
  }
  entry->decode( entry->handler, decoder, instruction );
}

void OpCodeHandler_MandatoryPrefix4::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_MandatoryPrefix4*>( self_ptr );
  auto prefix_idx = static_cast<size_t>( decoder.state().mandatory_prefix );
  auto& entry = self->handlers[prefix_idx];
  entry.decode( entry.handler, decoder, instruction );
}

// ============================================================================
// Options Handler
// ============================================================================

void OpCodeHandler_Options::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Options*>( self_ptr );

  const HandlerEntry* entry;
  auto opts = decoder.options();
  if ( ( opts & self->decoder_options1 ) != 0 ) {
    entry = &self->handler_option1;
  } else if ( self->decoder_options2 != 0 && ( opts & self->decoder_options2 ) != 0 ) {
    entry = &self->handler_option2;
  } else {
    entry = &self->handler_default;
  }
  // Read modrm unconditionally if sub-handler needs it (like Rust behavior)
  if ( entry->handler->has_modrm ) {
    decoder.read_modrm();
  }
  entry->decode( entry->handler, decoder, instruction );
}

void OpCodeHandler_Options_DontReadModRM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Options_DontReadModRM*>( self_ptr );

  const HandlerEntry* entry;
  if ( ( decoder.options() & self->decoder_options ) != 0 ) {
    entry = &self->handler_option;
  } else {
    entry = &self->handler_default;
  }
  // DontReadModRM - call directly without reading modrm
  entry->decode( entry->handler, decoder, instruction );
}

void OpCodeHandler_Options1632::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Options1632*>( self_ptr );

  const HandlerEntry* entry;
  if ( decoder.bitness() != 64 ) {
    auto opts = decoder.options();
    if ( ( opts & self->decoder_options1 ) != 0 ) {
      entry = &self->handler_option1;
    } else if ( self->decoder_options2 != 0 && ( opts & self->decoder_options2 ) != 0 ) {
      entry = &self->handler_option2;
    } else {
      entry = &self->handler_default;
    }
  } else {
    entry = &self->handler_default;
  }
  // Read modrm unconditionally if sub-handler needs it (like Rust behavior)
  if ( entry->handler->has_modrm ) {
    decoder.read_modrm();
  }
  entry->decode( entry->handler, decoder, instruction );
}

// ============================================================================
// VEX/EVEX/XOP/D3NOW Handlers - stubs (just set invalid for now)
// ============================================================================

void OpCodeHandler_VEX2::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  decoder.decode_vex2( instruction );
}

void OpCodeHandler_VEX3::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  decoder.decode_vex3( instruction );
}

void OpCodeHandler_XOP::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_XOP*>( self_ptr );
  // Check if this is XOP prefix or POP instruction
  // XOP prefix: modrm & 0x1F >= 8 (m-mmmmm field >= 8)
  // POP r/m: modrm & 0x1F < 8 (mod != 3 and rm < 8)
  if ( ( decoder.state().modrm & 0x1F ) < 8 ) {
    // Not XOP prefix - fall through to POP handler
    auto& handler = self->handler_reg0;
    handler.decode( handler.handler, decoder, instruction );
  } else {
    // XOP prefix
    decoder.decode_xop( instruction );
  }
}

void OpCodeHandler_EVEX::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  decoder.decode_evex( instruction );
}

void OpCodeHandler_D3NOW::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  decoder.decode_3dnow( instruction );
}

// ============================================================================
// Prefix Handlers
// ============================================================================

void OpCodeHandler_PrefixEsCsSsDs::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PrefixEsCsSsDs*>( self_ptr );
  if ( !decoder.is_64bit_mode() || decoder.state().segment_prio == 0 ) {
    instruction.set_segment_prefix( self->seg );
  }
  decoder.reset_rex_prefix_state();
  decoder.call_opcode_handlers_map0_table( instruction );
}

void OpCodeHandler_PrefixFsGs::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PrefixFsGs*>( self_ptr );
  instruction.set_segment_prefix( self->seg );
  decoder.state().segment_prio = 1;
  decoder.reset_rex_prefix_state();
  decoder.call_opcode_handlers_map0_table( instruction );
}

void OpCodeHandler_Prefix66::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  decoder.state().flags |= StateFlags::HAS66;
  decoder.state().operand_size = decoder.bitness() == 16 ? OpSize::SIZE32 : OpSize::SIZE16;
  if ( decoder.state().mandatory_prefix == DecoderMandatoryPrefix::PNP ) {
    decoder.state().mandatory_prefix = DecoderMandatoryPrefix::P66;
  }
  decoder.reset_rex_prefix_state();
  decoder.call_opcode_handlers_map0_table( instruction );
}

void OpCodeHandler_Prefix67::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  // Toggle address size
  switch ( decoder.bitness() ) {
    case 64:
      decoder.state().address_size = OpSize::SIZE32;
      break;
    case 32:
      decoder.state().address_size = OpSize::SIZE16;
      break;
    case 16:
      decoder.state().address_size = OpSize::SIZE32;
      break;
  }
  decoder.reset_rex_prefix_state();
  decoder.call_opcode_handlers_map0_table( instruction );
}

void OpCodeHandler_PrefixF0::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  instruction.set_has_lock_prefix( true );
  decoder.state().flags |= StateFlags::LOCK;
  decoder.reset_rex_prefix_state();
  decoder.call_opcode_handlers_map0_table( instruction );
}

void OpCodeHandler_PrefixF2::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  instruction.set_has_repne_prefix( true );
  decoder.state().mandatory_prefix = DecoderMandatoryPrefix::PF2;
  decoder.reset_rex_prefix_state();
  decoder.call_opcode_handlers_map0_table( instruction );
}

void OpCodeHandler_PrefixF3::decode( const OpCodeHandler* /*self_ptr*/, Decoder& decoder, Instruction& instruction ) {
  instruction.set_has_repe_prefix( true );
  decoder.state().mandatory_prefix = DecoderMandatoryPrefix::PF3;
  decoder.reset_rex_prefix_state();
  decoder.call_opcode_handlers_map0_table( instruction );
}

void OpCodeHandler_PrefixREX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PrefixREX*>( self_ptr );

  if ( decoder.is_64bit_mode() ) {
    decoder.state().flags |= StateFlags::HAS_REX;
    auto rex = self->rex;
    if ( ( rex & 8 ) != 0 ) {
      decoder.state().flags |= StateFlags::W;
      decoder.state().operand_size = OpSize::SIZE64;
    } else {
      decoder.state().flags &= ~StateFlags::W;
      if ( ( decoder.state().flags & StateFlags::HAS66 ) == 0 ) {
        decoder.state().operand_size = OpSize::SIZE32;
      } else {
        decoder.state().operand_size = OpSize::SIZE16;
      }
    }
    decoder.state().extra_register_base = ( rex & 4 ) << 1;
    decoder.state().extra_index_register_base = ( rex & 2 ) << 2;
    decoder.state().extra_base_register_base = ( rex & 1 ) << 3;
    decoder.call_opcode_handlers_map0_table( instruction );
  } else {
    // Not 64-bit mode, use fallback handler
    auto& entry = self->handler;
    entry.decode( entry.handler, decoder, instruction );
  }
}

// ============================================================================
// Macro for generating simple 3-code handlers
// ============================================================================

#define DECODE_3CODE_HANDLER( handler_name )                                                       \
  void handler_name::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) { \
    auto* self = reinterpret_cast<const handler_name*>( self_ptr );                                \
    Code codes[] = { self->code16, self->code32, self->code64 };                                   \
    instr.set_code( codes[get_op_size_index( decoder )] );                                         \
  }

#define DECODE_2CODE_HANDLER( handler_name )                                                       \
  void handler_name::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) { \
    auto* self = reinterpret_cast<const handler_name*>( self_ptr );                                \
    instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );                       \
  }

#define DECODE_2CODE_HANDLER_16_32( handler_name )                                                  \
  void handler_name::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) { \
    auto* self = reinterpret_cast<const handler_name*>( self_ptr );                                \
    instr.set_code( decoder.state().operand_size == OpSize::SIZE16 ? self->code16 : self->code32 ); \
  }

#define DECODE_1CODE_HANDLER( handler_name )                                                       \
  void handler_name::decode( const OpCodeHandler* self_ptr, Decoder& /*decoder*/, Instruction& instr ) { \
    auto* self = reinterpret_cast<const handler_name*>( self_ptr );                                \
    instr.set_code( self->code );                                                                  \
  }

// ============================================================================
// Ev handlers - stubs that just set the code
// ============================================================================

void OpCodeHandler_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( decoder.state().mod_ == 3 ) {
    Register reg_base = get_gpr_base( op_size );
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

void OpCodeHandler_Ev_Iz::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Iz*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( decoder.state().mod_ == 3 ) {
    Register reg_base = get_gpr_base( op_size );
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Iz (immediate based on operand size)
  if ( op_size == OpSize::SIZE64 ) {
    // 64-bit mode with sign-extended 32-bit immediate
    instr.set_op1_kind( OpKind::IMMEDIATE32TO64 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op1_kind( OpKind::IMMEDIATE16 );
    auto imm = decoder.read_u16();
    if ( imm ) {
      instr.set_immediate16( *imm );
    }
  } else {
    instr.set_op1_kind( OpKind::IMMEDIATE32 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  }
}

void OpCodeHandler_Ev_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Ib*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( decoder.state().mod_ == 3 ) {
    Register reg_base = get_gpr_base( op_size );
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Ib (sign-extended based on operand size)
  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op1_kind( OpKind::IMMEDIATE8TO64 );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op1_kind( OpKind::IMMEDIATE8TO16 );
  } else {
    instr.set_op1_kind( OpKind::IMMEDIATE8TO32 );
  }
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_Ev_Ib2: Ev, Ib (shift/rotate with immediate, not sign-extended)
void OpCodeHandler_Ev_Ib2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Ib2*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op1: Ib (immediate byte, not sign-extended)
  instr.set_op1_kind( OpKind::IMMEDIATE8 );

  // Op0: Ev
  Register reg_base = get_gpr_base( op_size );
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

void OpCodeHandler_Ev_1::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_1*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( decoder.state().mod_ == 3 ) {
    Register reg_base = get_gpr_base( op_size );
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: immediate 1
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( 1 );
}

void OpCodeHandler_Ev_CL::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_CL*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( decoder.state().mod_ == 3 ) {
    Register reg_base = get_gpr_base( op_size );
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: CL register
  instr.set_op1_register( Register::CL );
  instr.set_op1_kind( OpKind::REGISTER );
}
void OpCodeHandler_Ev_Gv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Gv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Get register base for operand size
  Register reg_base = get_gpr_base( op_size );

  // Op1: Gv (reg field from ModR/M + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Ev (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    // Register operand
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    // Memory operand
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}
void OpCodeHandler_Ev_Gv_flags::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Gv_flags*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Get register base for operand size
  Register reg_base = get_gpr_base( op_size );

  // Op1: Gv (reg field from ModR/M + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Ev (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    // Register operand
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    // Memory operand - also apply flags to state
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}
void OpCodeHandler_Ev_Gv_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Gv_Ib*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op1: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op2: Ib (immediate byte)
  instr.set_op2_kind( OpKind::IMMEDIATE8 );

  // Op0: Ev
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Read the immediate byte
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

void OpCodeHandler_Ev_Gv_CL::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Gv_CL*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op1: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op2: CL register
  instr.set_op2_register( Register::CL );
  instr.set_op2_kind( OpKind::REGISTER );

  // Op0: Ev
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Ev_Sw: MOV Ev, Sreg
void OpCodeHandler_Ev_Sw::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Sw*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op1: Sreg (segment register from reg field)
  // Segment registers are ES, CS, SS, DS, FS, GS (0-5)
  // But don't return early - need to read memory operand for correct length
  uint32_t sreg_idx = decoder.state().reg;
  if ( sreg_idx > 5 ) {
    decoder.set_invalid_instruction();
  }
  instr.set_op1_register( add_reg( Register::ES, sreg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Ev (always read to get correct instruction length)
  Register reg_base = get_gpr_base( op_size );
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Evj: JMP/CALL Ev (indirect jump)
void OpCodeHandler_Evj::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Evj*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op0: Ev (register or memory for indirect jump)
  if ( decoder.state().mod_ == 3 ) {
    Register reg_base = get_gpr_base( op_size );
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Evw: Ev word (always 16-bit)
void OpCodeHandler_Evw::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Evw*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op0: Ev (but uses word registers)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( Register::AX, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Ew: word operand (always 16-bit register/memory)
void OpCodeHandler_Ew::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ew*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op0: Ew (word register/memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( Register::AX, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

void OpCodeHandler_Ev_Gv_32_64::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Gv_32_64*>( self_ptr );

  Register reg_base;
  if ( decoder.is_64bit_mode() ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op1: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Ev
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

void OpCodeHandler_Ev_Gv_REX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_Gv_REX*>( self_ptr );

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op1: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Ev (must be memory for this handler)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}
// OpCodeHandler_Ev_REXW: Ev based on REX.W
void OpCodeHandler_Ev_REXW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_REXW*>( self_ptr );

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op0: Ev
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Ev_P: Ev, Pq (MMX register)
void OpCodeHandler_Ev_P::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_P*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );

  // Op1: Pq (MMX register from reg field)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op1_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Ev
  Register reg_base = decoder.is_64bit_mode() ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Ev_VX: Ev, Vx (XMM register)
void OpCodeHandler_Ev_VX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ev_VX*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );

  // Op1: Vx (XMM register from reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Ev
  Register reg_base = decoder.is_64bit_mode() ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// ============================================================================
// Eb handlers
// ============================================================================

void OpCodeHandler_Eb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Eb*>( self_ptr );
  instr.set_code( self->code );

  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;
    }
    instr.set_op0_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

void OpCodeHandler_Eb_Gb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Eb_Gb*>( self_ptr );
  instr.set_code( self->code );

  // Op1: Gb (reg field + REX.R), with REX extension handling
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && reg_idx >= 4 ) {
    reg_idx += 4;  // SPL/BPL/SIL/DIL instead of AH/CH/DH/BH
  }
  instr.set_op1_register( add_reg( Register::AL, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Eb (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;
    }
    instr.set_op0_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    // Only allow LOCK prefix for memory operands
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}
void OpCodeHandler_Eb_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Eb_Ib*>( self_ptr );
  instr.set_code( self->code );

  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;
    }
    instr.set_op0_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.state().flags |= self->flags;
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Ib (immediate byte)
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

void OpCodeHandler_Eb_1::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Eb_1*>( self_ptr );
  instr.set_code( self->code );

  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;
    }
    instr.set_op0_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: immediate 1
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( 1 );
}

void OpCodeHandler_Eb_CL::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Eb_CL*>( self_ptr );
  instr.set_code( self->code );

  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;
    }
    instr.set_op0_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: CL register
  instr.set_op1_register( Register::CL );
  instr.set_op1_kind( OpKind::REGISTER );
}

// ============================================================================
// Gv handlers
// ============================================================================

void OpCodeHandler_Gv_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );
  
  // Get register base for operand size
  Register reg_base = get_gpr_base( op_size );
  
  // Op0: Gv (reg field from ModR/M + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  
  // Op1: Ev (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    // Register operand
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
  } else {
    // Memory operand
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Ev_Ib: 3-operand IMUL r16/32/64, r/m16/32/64, imm8
void OpCodeHandler_Gv_Ev_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev_Ib*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op1: Ev (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }

  // Op2: Ib (sign-extended based on operand size)
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op2_kind( OpKind::IMMEDIATE8TO64 );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op2_kind( OpKind::IMMEDIATE8TO16 );
  } else {
    instr.set_op2_kind( OpKind::IMMEDIATE8TO32 );
  }
}

// OpCodeHandler_Gv_Ev_Iz: 3-operand IMUL r16/32/64, r/m16/32/64, imm16/32
void OpCodeHandler_Gv_Ev_Iz::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev_Iz*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op1: Ev (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op2: Iz (immediate based on operand size)
  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op2_kind( OpKind::IMMEDIATE32TO64 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op2_kind( OpKind::IMMEDIATE16 );
    auto imm = decoder.read_u16();
    if ( imm ) {
      instr.set_immediate16( *imm );
    }
  } else {
    instr.set_op2_kind( OpKind::IMMEDIATE32 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  }
}

// OpCodeHandler_Gv_Ev2: Gv, Ev (2-operand, like TEST)
void OpCodeHandler_Gv_Ev2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev2*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op0: Gv uses operand size (AX/EAX/RAX)
  Register reg_base = get_gpr_base( op_size );
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev2 - source operand uses EAX for 32/64-bit, AX for 16-bit
  // This is for instructions like MOVSXD where source is always 32-bit
  Register src_base = ( op_size != OpSize::SIZE16 ) ? Register::EAX : Register::AX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( src_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Ev3: Gv, Ev (similar to Gv_Ev2)
void OpCodeHandler_Gv_Ev3::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev3*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Eb: MOVZX/MOVSX r16/32/64, r/m8
void OpCodeHandler_Gv_Eb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Eb*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Eb (r/m8 - byte register or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;  // SPL/BPL/SIL/DIL
    }
    instr.set_op1_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Ew: MOVZX/MOVSX r16/32/64, r/m16
void OpCodeHandler_Gv_Ew::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ew*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ew (r/m16 - word register or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::AX, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_M: LEA r16/32/64, m
void OpCodeHandler_Gv_M::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_M*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: M (memory only, no register form)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_M_as: Similar to Gv_M but uses address size
void OpCodeHandler_Gv_M_as::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_M_as*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: M (memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Mp: LDS/LES/LFS/LGS/LSS r16/32, m16:16/32
void OpCodeHandler_Gv_Mp::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Mp*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Mp (memory pointer, memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Mv: Gv, Mv (memory only for operand 1)
void OpCodeHandler_Gv_Mv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Mv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Mv (memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gdq_Ev: MOVD/MOVQ mm/xmm, r/m32/64 (or similar)
void OpCodeHandler_Gdq_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gdq_Ev*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Gv (reg field + REX.R)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Ev_32_64: Gv, Ev in 32/64-bit mode only
void OpCodeHandler_Gv_Ev_32_64::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev_32_64*>( self_ptr );

  Register reg_base;
  if ( decoder.is_64bit_mode() ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Ev_Ib_REX: Gv, Ev, Ib with REX.W determining size
void OpCodeHandler_Gv_Ev_Ib_REX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev_Ib_REX*>( self_ptr );

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op1: XMM register (for PEXTRW etc.)
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

// OpCodeHandler_Gv_Ev_REX: Gv, Ev with REX.W
void OpCodeHandler_Gv_Ev_REX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ev_REX*>( self_ptr );

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Eb_REX: Gv, Eb with REX.W
void OpCodeHandler_Gv_Eb_REX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Eb_REX*>( self_ptr );

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Eb
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;
    }
    instr.set_op1_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_Ma: Gv, Ma (BOUND instruction)
void OpCodeHandler_Gv_Ma::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_Ma*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  instr.set_code( op_size == OpSize::SIZE16 ? self->code16 : self->code32 );

  Register reg_base = op_size == OpSize::SIZE16 ? Register::AX : Register::EAX;

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ma (memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Gv_N: Gv, Nq (MMX register)
void OpCodeHandler_Gv_N::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_N*>( self_ptr );
  
  // Check REX.W for 64-bit vs 32-bit code selection
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code32 );  // code32 is actually code64 in the pair
  } else {
    instr.set_code( self->code16 );  // code16 is actually code32 in the pair
  }

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    reg_base = Register::RAX;
  } else {
    reg_base = Register::EAX;
  }

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Nq (MMX register from r/m field) - requires mod==3
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op1_register( add_reg( Register::MM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// OpCodeHandler_Gv_N_Ib_REX: Gv, Nq, Ib with REX.W
void OpCodeHandler_Gv_N_Ib_REX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_N_Ib_REX*>( self_ptr );

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Nq (MMX register)
  uint32_t rm_idx = decoder.state().rm;
  instr.set_op1_register( add_reg( Register::MM0, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_Gv_RX: Gv, Rx (XMM register, register form only)
void OpCodeHandler_Gv_RX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_RX*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );

  Register reg_base = decoder.is_64bit_mode() ? Register::RAX : Register::EAX;

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Rx (XMM register from r/m, must be register)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// OpCodeHandler_Gv_W: Gv, W (XMM register or memory)
void OpCodeHandler_Gv_W::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gv_W*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );

  Register reg_base = decoder.is_64bit_mode() ? Register::RAX : Register::EAX;

  // Op0: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( reg_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: W (XMM or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_GvM_VX_Ib: Gv/M, Vx, Ib
void OpCodeHandler_GvM_VX_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_GvM_VX_Ib*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );

  Register reg_base = decoder.is_64bit_mode() ? Register::RAX : Register::EAX;

  // Op0: Gv or M
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Vx (XMM register)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

void OpCodeHandler_Gb_Eb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gb_Eb*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Gb (reg field + REX.R), with REX extension handling
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && reg_idx >= 4 ) {
    reg_idx += 4;  // SPL/BPL/SIL/DIL instead of AH/CH/DH/BH
  }
  instr.set_op0_register( add_reg( Register::AL, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Eb (r/m field - register or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 && rm_idx >= 4 ) {
      rm_idx += 4;
    }
    instr.set_op1_register( add_reg( Register::AL, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}
// OpCodeHandler_Gd_Rd: Gd, Rd (register-only, 32-bit)
void OpCodeHandler_Gd_Rd::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Gd_Rd*>( self_ptr );
  instr.set_code( self->code );
  
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::EAX, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::EAX, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// ============================================================================
// Register handlers
// ============================================================================

void OpCodeHandler_Reg::decode( const OpCodeHandler* self_ptr, Decoder& /*decoder*/, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_register( self->reg );
}

void OpCodeHandler_RegIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_RegIb*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_register( self->reg );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
    instr.set_op1_kind( OpKind::IMMEDIATE8 );
  }
}

void OpCodeHandler_IbReg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_IbReg*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op1_register( self->reg );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
    instr.set_op0_kind( OpKind::IMMEDIATE8 );
  }
}

// OpCodeHandler_AL_DX: IN AL, DX
void OpCodeHandler_AL_DX::decode( const OpCodeHandler* self_ptr, Decoder& /*decoder*/, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_AL_DX*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_register( Register::AL );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_register( Register::DX );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_DX_AL: OUT DX, AL
void OpCodeHandler_DX_AL::decode( const OpCodeHandler* self_ptr, Decoder& /*decoder*/, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_DX_AL*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_register( Register::DX );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_register( Register::AL );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_DX_eAX: OUT DX, eAX
void OpCodeHandler_DX_eAX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_DX_eAX*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  instr.set_code( op_size == OpSize::SIZE16 ? self->code16 : self->code32 );
  instr.set_op0_register( Register::DX );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_register( op_size == OpSize::SIZE16 ? Register::AX : Register::EAX );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_eAX_DX: IN eAX, DX
void OpCodeHandler_eAX_DX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_eAX_DX*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  instr.set_code( op_size == OpSize::SIZE16 ? self->code16 : self->code32 );
  instr.set_op0_register( op_size == OpSize::SIZE16 ? Register::AX : Register::EAX );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_register( Register::DX );
  instr.set_op1_kind( OpKind::REGISTER );
}

// ============================================================================
// Immediate handlers
// ============================================================================

void OpCodeHandler_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ib*>( self_ptr );
  instr.set_code( self->code );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
    instr.set_op0_kind( OpKind::IMMEDIATE8 );
  }
}

void OpCodeHandler_Ib3::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ib3*>( self_ptr );
  instr.set_code( self->code );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
    instr.set_op0_kind( OpKind::IMMEDIATE8 );
  }
}

// ============================================================================
// Jump handlers
// ============================================================================

// OpCodeHandler_Jb: Short jumps (Jcc rel8, JMP rel8)
void OpCodeHandler_Jb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Jb*>( self_ptr );
  
  // Read the sign-extended immediate byte
  auto imm_opt = decoder.read_byte();
  if ( !imm_opt ) {
    decoder.set_invalid_instruction();
    return;
  }
  int8_t rel8 = static_cast<int8_t>( *imm_opt );
  
  if ( decoder.is_64bit_mode() ) {
    // 64-bit mode: default is 64-bit branch, unless 66 prefix for 16-bit
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( rel8 ) + static_cast<int64_t>( decoder.current_ip64() ) );
      instr.set_near_branch64( target );
      instr.set_code( self->code64 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH64 );
    } else {
      uint16_t target = static_cast<uint16_t>( static_cast<int32_t>( rel8 ) + static_cast<int32_t>( decoder.current_ip32() ) );
      instr.set_near_branch16( target );
      instr.set_code( self->code16 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH16 );
    }
  } else {
    // 16/32-bit mode
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      uint32_t target = static_cast<uint32_t>( static_cast<int32_t>( rel8 ) + static_cast<int32_t>( decoder.current_ip32() ) );
      instr.set_near_branch32( target );
      instr.set_code( self->code32 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH32 );
    } else {
      uint16_t target = static_cast<uint16_t>( static_cast<int32_t>( rel8 ) + static_cast<int32_t>( decoder.current_ip32() ) );
      instr.set_near_branch16( target );
      instr.set_code( self->code16 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH16 );
    }
  }
}

// OpCodeHandler_Jb2: Short jumps with more code variants
void OpCodeHandler_Jb2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Jb2*>( self_ptr );
  
  auto imm_opt = decoder.read_byte();
  if ( !imm_opt ) {
    decoder.set_invalid_instruction();
    return;
  }
  int8_t rel8 = static_cast<int8_t>( *imm_opt );
  
  if ( decoder.is_64bit_mode() ) {
    if ( decoder.state().operand_size == OpSize::SIZE64 ) {
      uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( rel8 ) + static_cast<int64_t>( decoder.current_ip64() ) );
      instr.set_near_branch64( target );
      instr.set_code( self->code64_64 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH64 );
    } else if ( decoder.state().operand_size == OpSize::SIZE16 ) {
      uint16_t target = static_cast<uint16_t>( static_cast<int32_t>( rel8 ) + static_cast<int32_t>( decoder.current_ip32() ) );
      instr.set_near_branch16( target );
      instr.set_code( self->code16_64 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH16 );
    } else {
      uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( rel8 ) + static_cast<int64_t>( decoder.current_ip64() ) );
      instr.set_near_branch64( target );
      instr.set_code( self->code64_32 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH64 );
    }
  } else {
    if ( decoder.state().operand_size == OpSize::SIZE32 ) {
      uint32_t target = static_cast<uint32_t>( static_cast<int32_t>( rel8 ) + static_cast<int32_t>( decoder.current_ip32() ) );
      instr.set_near_branch32( target );
      instr.set_code( self->code32_32 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH32 );
    } else {
      uint16_t target = static_cast<uint16_t>( static_cast<int32_t>( rel8 ) + static_cast<int32_t>( decoder.current_ip32() ) );
      instr.set_near_branch16( target );
      instr.set_code( self->code16_32 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH16 );
    }
  }
}

// OpCodeHandler_Jx: XBEGIN with variable-width offset
void OpCodeHandler_Jx::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Jx*>( self_ptr );
  
  if ( decoder.is_64bit_mode() ) {
    if ( decoder.state().operand_size == OpSize::SIZE32 || decoder.state().operand_size == OpSize::SIZE64 ) {
      auto imm_opt = decoder.read_u32();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      int32_t rel32 = static_cast<int32_t>( *imm_opt );
      uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( rel32 ) + static_cast<int64_t>( decoder.current_ip64() ) );
      instr.set_near_branch64( target );
      instr.set_code( decoder.state().operand_size == OpSize::SIZE64 ? self->code64 : self->code32 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH64 );
    } else {
      auto imm_opt = decoder.read_u16();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      int16_t rel16 = static_cast<int16_t>( *imm_opt );
      uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( rel16 ) + static_cast<int64_t>( decoder.current_ip64() ) );
      instr.set_near_branch64( target );
      instr.set_code( self->code16 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH64 );
    }
  } else {
    if ( decoder.state().operand_size == OpSize::SIZE32 ) {
      auto imm_opt = decoder.read_u32();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      uint32_t target = static_cast<uint32_t>( *imm_opt ) + decoder.current_ip32();
      instr.set_near_branch32( target );
      instr.set_code( self->code32 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH32 );
    } else {
      auto imm_opt = decoder.read_u16();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      int16_t rel16 = static_cast<int16_t>( *imm_opt );
      uint32_t target = static_cast<uint32_t>( static_cast<int32_t>( rel16 ) + static_cast<int32_t>( decoder.current_ip32() ) );
      instr.set_near_branch32( target );
      instr.set_code( self->code16 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH32 );
    }
  }
}

// OpCodeHandler_Jz: Near jumps (JMP rel16/32, CALL rel16/32, Jcc rel16/32)
void OpCodeHandler_Jz::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Jz*>( self_ptr );
  
  if ( decoder.is_64bit_mode() ) {
    // 64-bit mode: In 64-bit mode, these instructions always use 32-bit immediate
    // unless AMD mode is enabled AND operand_size is 16-bit
    // Rust logic: if (NOT_AMD_MODE | (operand_size != Size16)) != 0 => use 32-bit
    bool use_16bit = decoder.has_amd_option() && decoder.state().operand_size == OpSize::SIZE16;
    if ( !use_16bit ) {
      auto imm_opt = decoder.read_u32();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      int32_t rel32 = static_cast<int32_t>( *imm_opt );
      uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( rel32 ) + static_cast<int64_t>( decoder.current_ip64() ) );
      instr.set_near_branch64( target );
      instr.set_code( self->code64 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH64 );
    } else {
      auto imm_opt = decoder.read_u16();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      uint16_t target = static_cast<uint16_t>( *imm_opt + decoder.current_ip32() );
      instr.set_near_branch16( target );
      instr.set_code( self->code16 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH16 );
    }
  } else {
    // 16/32-bit mode
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      auto imm_opt = decoder.read_u32();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      uint32_t target = *imm_opt + decoder.current_ip32();
      instr.set_near_branch32( target );
      instr.set_code( self->code32 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH32 );
    } else {
      auto imm_opt = decoder.read_u16();
      if ( !imm_opt ) {
        decoder.set_invalid_instruction();
        return;
      }
      uint16_t target = static_cast<uint16_t>( *imm_opt + decoder.current_ip32() );
      instr.set_near_branch16( target );
      instr.set_code( self->code16 );
      instr.set_op0_kind( OpKind::NEAR_BRANCH16 );
    }
  }
}

// OpCodeHandler_Jdisp: JCXZ/JECXZ/JRCXZ (16/32-bit mode only)
void OpCodeHandler_Jdisp::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Jdisp*>( self_ptr );
  auto op_size = decoder.state().operand_size;

  if ( op_size != OpSize::SIZE16 ) {
    instr.set_code( self->code32 );
    instr.set_op0_kind( OpKind::NEAR_BRANCH32 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_near_branch32( *imm );
    }
  } else {
    instr.set_code( self->code16 );
    instr.set_op0_kind( OpKind::NEAR_BRANCH16 );
    auto imm = decoder.read_u16();
    if ( imm ) {
      instr.set_near_branch16( static_cast<uint16_t>( *imm ) );
    }
  }
}

// ============================================================================
// Control/Debug register handlers
// ============================================================================

// OpCodeHandler_C_R: MOV CRn, r32/64
void OpCodeHandler_C_R::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_C_R*>( self_ptr );

  Register gpr_base;
  if ( decoder.is_64bit_mode() ) {
    instr.set_code( self->code64 );
    gpr_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    gpr_base = Register::EAX;
  }

  // Op0: CRn (control register from reg field)
  uint32_t cr_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::CR0, cr_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: r32/64 (from r/m field)
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op1_register( add_reg( gpr_base, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_R_C: MOV r32/64, CRn
void OpCodeHandler_R_C::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_R_C*>( self_ptr );

  Register gpr_base;
  if ( decoder.is_64bit_mode() ) {
    instr.set_code( self->code64 );
    gpr_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    gpr_base = Register::EAX;
  }

  // Op0: r32/64 (from r/m field)
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op0_register( add_reg( gpr_base, rm_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: CRn (control register from reg field)
  uint32_t cr_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::CR0, cr_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// ============================================================================
// Memory handlers
// ============================================================================

void OpCodeHandler_M::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_M*>( self_ptr );
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_code( self->code );
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_M_REXW: Memory with REX.W selection
void OpCodeHandler_M_REXW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_M_REXW*>( self_ptr );

  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
  } else {
    instr.set_code( self->code32 );
  }

  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Ms: Memory with operand size selection
void OpCodeHandler_Ms::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ms*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Mf: Memory for FPU (16/32-bit operand size)
void OpCodeHandler_Mf::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Mf*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  instr.set_code( op_size == OpSize::SIZE16 ? self->code16 : self->code32 );

  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_MV: Memory, Vx (XMM)
void OpCodeHandler_MV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_MV*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Memory (must be memory, not register)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Vx (XMM register)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_Mv_Gv: Memory, Gv
void OpCodeHandler_Mv_Gv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Mv_Gv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );

  // Op0: Mv (memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_Mv_Gv_REXW: Memory, Gv with REX.W
void OpCodeHandler_Mv_Gv_REXW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Mv_Gv_REXW*>( self_ptr );

  Register reg_base;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  // Op0: Mv (memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Gv
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( reg_base, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_MemBx: XLAT (memory [BX+AL] or [RBX+AL])
void OpCodeHandler_MemBx::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_MemBx*>( self_ptr );
  instr.set_code( self->code );
  // XLAT uses memory operand [DS:BX+AL] or [DS:EBX+AL] or [DS:RBX+AL]
  instr.set_op0_kind( OpKind::MEMORY );
  instr.set_memory_index( Register::AL );
  // BX + 16 = EBX, BX + 32 = RBX
  Register base_reg = add_reg( Register::BX, static_cast<uint32_t>( decoder.state().address_size ) * 16 );
  instr.set_memory_base( base_reg );
}

// OpCodeHandler_MP: Memory pointer (far pointer)
void OpCodeHandler_MP::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_MP*>( self_ptr );
  instr.set_code( self->code );

  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Ep: Far pointer (CALL/JMP m16:16/32/64)
void OpCodeHandler_Ep::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ep*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_M_Sw: Memory, Sreg
void OpCodeHandler_M_Sw::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_M_Sw*>( self_ptr );
  instr.set_code( self->code );

  // Op1: Sreg (segment register from reg field)
  // But don't return early - need to read memory operand for correct length
  uint32_t sreg_idx = decoder.state().reg;
  if ( sreg_idx > 5 ) {
    decoder.set_invalid_instruction();
  }
  instr.set_op1_register( add_reg( Register::ES, sreg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Memory (must be memory, but still read for correct length)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_Sw_M: Sreg, Memory
void OpCodeHandler_Sw_M::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Sw_M*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Sreg (segment register from reg field)
  // But don't return early - need to read memory operand for correct length
  uint32_t sreg_idx = decoder.state().reg;
  if ( sreg_idx > 5 ) {
    decoder.set_invalid_instruction();
  }
  instr.set_op0_register( add_reg( Register::ES, sreg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Memory (must be memory, but still read for correct length)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// ============================================================================
// Push/Pop handlers
// ============================================================================

void OpCodeHandler_PushOpSizeReg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PushOpSizeReg*>( self_ptr );
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[get_op_size_index( decoder )] );
  instr.set_op0_register( self->reg );
}

// OpCodeHandler_PushEv: PUSH r/m16/32/64
void OpCodeHandler_PushEv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PushEv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  
  // Select code based on mode and operand size
  if ( decoder.is_64bit_mode() ) {
    if ( op_size != OpSize::SIZE16 ) {
      instr.set_code( self->code64 );
    } else {
      instr.set_code( self->code16 );
    }
  } else {
    if ( op_size == OpSize::SIZE32 ) {
      instr.set_code( self->code32 );
    } else {
      instr.set_code( self->code16 );
    }
  }
  
  // Op0: Ev (r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    if ( decoder.is_64bit_mode() ) {
      if ( op_size != OpSize::SIZE16 ) {
        instr.set_op0_register( add_reg( Register::RAX, rm_idx ) );
      } else {
        instr.set_op0_register( add_reg( Register::AX, rm_idx ) );
      }
    } else {
      if ( op_size == OpSize::SIZE32 ) {
        instr.set_op0_register( add_reg( Register::EAX, rm_idx ) );
      } else {
        instr.set_op0_register( add_reg( Register::AX, rm_idx ) );
      }
    }
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_PushIb2: PUSH imm8 (sign-extended)
void OpCodeHandler_PushIb2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PushIb2*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );
  
  // Sign-extended immediate byte
  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op0_kind( OpKind::IMMEDIATE8TO64 );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op0_kind( OpKind::IMMEDIATE8TO16 );
  } else {
    instr.set_op0_kind( OpKind::IMMEDIATE8TO32 );
  }
  
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_PushIz: PUSH imm16/32 (sign-extended in 64-bit mode)
void OpCodeHandler_PushIz::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PushIz*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );
  
  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op0_kind( OpKind::IMMEDIATE32TO64 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op0_kind( OpKind::IMMEDIATE16 );
    auto imm = decoder.read_u16();
    if ( imm ) {
      instr.set_immediate16( *imm );
    }
  } else {
    instr.set_op0_kind( OpKind::IMMEDIATE32 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  }
}

// OpCodeHandler_PushSimple2: PUSH (simple, size-dependent)
void OpCodeHandler_PushSimple2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PushSimple2*>( self_ptr );
  if ( decoder.is_64bit_mode() ) {
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      instr.set_code( self->code64 );
    } else {
      instr.set_code( self->code16 );
    }
  } else {
    if ( decoder.state().operand_size == OpSize::SIZE32 ) {
      instr.set_code( self->code32 );
    } else {
      instr.set_code( self->code16 );
    }
  }
}

void OpCodeHandler_PushSimpleReg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_PushSimpleReg*>( self_ptr );
  uint32_t reg_idx = self->index + decoder.state().extra_base_register_base;
  
  if ( decoder.is_64bit_mode() ) {
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      instr.set_code( self->code64 );
      instr.set_op0_register( add_reg( Register::RAX, reg_idx ) );
    } else {
      instr.set_code( self->code16 );
      instr.set_op0_register( add_reg( Register::AX, reg_idx ) );
    }
  } else {
    if ( decoder.state().operand_size == OpSize::SIZE32 ) {
      instr.set_code( self->code32 );
      instr.set_op0_register( add_reg( Register::EAX, reg_idx ) );
    } else {
      instr.set_code( self->code16 );
      instr.set_op0_register( add_reg( Register::AX, reg_idx ) );
    }
  }
  instr.set_op0_kind( OpKind::REGISTER );
}

// ============================================================================
// Simple2/3/4/5 handlers
// ============================================================================

// OpCodeHandler_Simple2: Simple instruction (operand size dependent)
void OpCodeHandler_Simple2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple2*>( self_ptr );
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( decoder.state().operand_size )] );
}

// OpCodeHandler_Simple2Iw: Simple instruction with imm16 (operand size dependent)
void OpCodeHandler_Simple2Iw::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple2Iw*>( self_ptr );
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( decoder.state().operand_size )] );
  instr.set_op0_kind( OpKind::IMMEDIATE16 );
  auto imm = decoder.read_u16();
  if ( imm ) {
    instr.set_immediate16( *imm );
  }
}

// OpCodeHandler_Simple3: Simple instruction (64-bit mode uses 64 unless size16)
void OpCodeHandler_Simple3::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple3*>( self_ptr );
  if ( decoder.is_64bit_mode() ) {
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      instr.set_code( self->code64 );
    } else {
      instr.set_code( self->code16 );
    }
  } else {
    if ( decoder.state().operand_size == OpSize::SIZE32 ) {
      instr.set_code( self->code32 );
    } else {
      instr.set_code( self->code16 );
    }
  }
}

// OpCodeHandler_Simple4: Simple instruction (REX.W dependent, 32 or 64)
void OpCodeHandler_Simple4::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple4*>( self_ptr );
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
  } else {
    instr.set_code( self->code32 );
  }
}

// OpCodeHandler_Simple5: Simple instruction (address size dependent)
void OpCodeHandler_Simple5::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple5*>( self_ptr );
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( decoder.state().address_size )] );
}

// OpCodeHandler_Simple5_a32: Simple instruction (address size dependent, requires 32-bit address)
void OpCodeHandler_Simple5_a32::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple5_a32*>( self_ptr );
  if ( decoder.state().address_size != OpSize::SIZE32 && decoder.invalid_check_mask() != 0 ) {
    decoder.set_invalid_instruction();
  }
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( decoder.state().address_size )] );
}

// OpCodeHandler_Simple5_ModRM_as: Simple instruction with reg operand (address size dependent)
void OpCodeHandler_Simple5_ModRM_as::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Simple5_ModRM_as*>( self_ptr );
  auto addr_size = decoder.state().address_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( addr_size )] );
  
  Register reg_bases[] = { Register::AX, Register::EAX, Register::RAX };
  Register reg_base = reg_bases[static_cast<std::size_t>( addr_size )];
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op0_register( add_reg( reg_base, rm_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

void OpCodeHandler_SimpleReg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_SimpleReg*>( self_ptr );
  // Code values are sequential: code16, code32, code64
  uint32_t size_index = static_cast<uint32_t>( decoder.state().operand_size );
  instr.set_code( static_cast<Code>( static_cast<uint32_t>( self->code ) + size_index ) );
  
  // Register is AX/EAX/RAX + index + extra_base_register_base
  // AX + 16 = EAX, AX + 32 = RAX
  uint32_t reg_idx = size_index * 16 + self->index + decoder.state().extra_base_register_base;
  instr.set_op0_register( add_reg( Register::AX, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

// ============================================================================
// Register/Immediate combination handlers
// ============================================================================

// OpCodeHandler_Reg_Iz: MOV rAX, imm16/32/64 (opcodes A8, A9)
void OpCodeHandler_Reg_Iz::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Iz*>( self_ptr );
  auto op_size = decoder.state().operand_size;

  if ( op_size == OpSize::SIZE64 ) {
    instr.set_code( self->code64 );
    instr.set_op0_register( Register::RAX );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_kind( OpKind::IMMEDIATE32TO64 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_code( self->code16 );
    instr.set_op0_register( Register::AX );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_kind( OpKind::IMMEDIATE16 );
    auto imm = decoder.read_u16();
    if ( imm ) {
      instr.set_immediate16( *imm );
    }
  } else {
    instr.set_code( self->code32 );
    instr.set_op0_register( Register::EAX );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_kind( OpKind::IMMEDIATE32 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  }
}

// Look-up table for REX-aware byte registers for MOV r8, imm8
static const Register g_rex_byte_regs[] = {
  Register::AL, Register::CL, Register::DL, Register::BL,
  Register::SPL, Register::BPL, Register::SIL, Register::DIL,
  Register::R8_L, Register::R9_L, Register::R10_L, Register::R11_L,
  Register::R12_L, Register::R13_L, Register::R14_L, Register::R15_L
};

// OpCodeHandler_RegIb3: MOV r8, imm8 (opcodes B0-B7)
void OpCodeHandler_RegIb3::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_RegIb3*>( self_ptr );
  
  instr.set_code( Code::MOV_R8_IMM8 );
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
  
  uint32_t reg_idx = self->index + decoder.state().extra_base_register_base;
  if ( ( decoder.state().flags & StateFlags::HAS_REX ) != 0 ) {
    // Use SPL/BPL/SIL/DIL for indices 4-7 when REX prefix is present
    instr.set_op0_register( g_rex_byte_regs[reg_idx] );
  } else {
    instr.set_op0_register( add_reg( Register::AL, reg_idx ) );
  }
  instr.set_op0_kind( OpKind::REGISTER );
}

// OpCodeHandler_RegIz2: MOV r16/32/64, imm16/32/64 (opcodes B8-BF)
void OpCodeHandler_RegIz2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_RegIz2*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  
  uint32_t reg_idx = self->index + decoder.state().extra_base_register_base;
  
  if ( op_size == OpSize::SIZE64 ) {
    instr.set_code( Code::MOV_R64_IMM64 );
    instr.set_op0_register( add_reg( Register::RAX, reg_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_kind( OpKind::IMMEDIATE64 );
    auto imm = decoder.read_u64();
    if ( imm ) {
      instr.set_immediate64( *imm );
    }
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_code( Code::MOV_R16_IMM16 );
    instr.set_op0_register( add_reg( Register::AX, reg_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_kind( OpKind::IMMEDIATE16 );
    auto imm = decoder.read_u16();
    if ( imm ) {
      instr.set_immediate16( *imm );
    }
  } else {
    instr.set_code( Code::MOV_R32_IMM32 );
    instr.set_op0_register( add_reg( Register::EAX, reg_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_kind( OpKind::IMMEDIATE32 );
    auto imm = decoder.read_u32();
    if ( imm ) {
      instr.set_immediate32( *imm );
    }
  }
}

// OpCodeHandler_Reg_Ib2: rAX, Ib (16/32 bit operand size)
void OpCodeHandler_Reg_Ib2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Ib2*>( self_ptr );
  
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
  
  if ( decoder.state().operand_size != OpSize::SIZE16 ) {
    instr.set_code( self->code32 );
    instr.set_op0_register( Register::EAX );
  } else {
    instr.set_code( self->code16 );
    instr.set_op0_register( Register::AX );
  }
  instr.set_op0_kind( OpKind::REGISTER );
}

// OpCodeHandler_IbReg2: Ib, rAX (16/32 bit operand size)
void OpCodeHandler_IbReg2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_IbReg2*>( self_ptr );
  
  instr.set_op0_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
  
  if ( decoder.state().operand_size != OpSize::SIZE16 ) {
    instr.set_code( self->code32 );
    instr.set_op1_register( Register::EAX );
  } else {
    instr.set_code( self->code16 );
    instr.set_op1_register( Register::AX );
  }
  instr.set_op1_kind( OpKind::REGISTER );
}

// ============================================================================
// Xchg handler
// ============================================================================

// OpCodeHandler_Xchg_Reg_rAX: XCHG r16/32/64, rAX (opcodes 90-97)
void OpCodeHandler_Xchg_Reg_rAX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Xchg_Reg_rAX*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  
  uint32_t reg_idx = self->index + decoder.state().extra_base_register_base;
  
  // Check for PAUSE (F3 90)
  if ( self->index == 0 && decoder.state().mandatory_prefix == DecoderMandatoryPrefix::PF3 ) {
    instr.set_code( Code::PAUSE );
    return;
  }
  
  // For reg_idx == 0 and no F3 prefix, this is NOP (no operands)
  if ( reg_idx == 0 ) {
    if ( op_size == OpSize::SIZE64 ) {
      instr.set_code( Code::NOPQ );
    } else if ( op_size == OpSize::SIZE16 ) {
      instr.set_code( Code::NOPW );
    } else {
      instr.set_code( Code::NOPD );
    }
    return;
  }
  
  // Real XCHG r, rAX
  if ( op_size == OpSize::SIZE64 ) {
    instr.set_code( Code::XCHG_R64_RAX );
    instr.set_op0_register( add_reg( Register::RAX, reg_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_register( Register::RAX );
    instr.set_op1_kind( OpKind::REGISTER );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_code( Code::XCHG_R16_AX );
    instr.set_op0_register( add_reg( Register::AX, reg_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_register( Register::AX );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_code( Code::XCHG_R32_EAX );
    instr.set_op0_register( add_reg( Register::EAX, reg_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    instr.set_op1_register( Register::EAX );
    instr.set_op1_kind( OpKind::REGISTER );
  }
}

// ============================================================================
// Rv handlers
// ============================================================================

// OpCodeHandler_Rv: Rv (register from r/m field)
void OpCodeHandler_Rv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Rv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  Register reg_base = get_gpr_base( op_size );
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op0_register( add_reg( reg_base, rm_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

// OpCodeHandler_Rv_32_64: Rv in 32/64-bit mode
void OpCodeHandler_Rv_32_64::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Rv_32_64*>( self_ptr );

  Register reg_base;
  if ( decoder.is_64bit_mode() ) {
    instr.set_code( self->code64 );
    reg_base = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    reg_base = Register::EAX;
  }

  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op0_register( add_reg( reg_base, rm_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

// OpCodeHandler_RvMw_Gw: Rv/Mw, Gw
void OpCodeHandler_RvMw_Gw::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_RvMw_Gw*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  instr.set_code( op_size == OpSize::SIZE16 ? self->code16 : self->code32 );

  Register reg_base = op_size == OpSize::SIZE16 ? Register::AX : Register::EAX;

  // Op0: Rv or Mw
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( reg_base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }

  // Op1: Gw (word register)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::AX, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_Rq: Rq (64-bit register from r/m field)
void OpCodeHandler_Rq::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Rq*>( self_ptr );
  instr.set_code( self->code );

  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op0_register( add_reg( Register::RAX, rm_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

// ============================================================================
// String instruction handlers
// ============================================================================

// Helper function for memory address kind based on address size
static inline OpKind get_es_di_mem_kind( const Decoder& decoder ) {
  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    return OpKind::MEMORY_ESRDI;
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    return OpKind::MEMORY_ESEDI;
  } else {
    return OpKind::MEMORY_ESDI;
  }
}

static inline OpKind get_seg_si_mem_kind( const Decoder& decoder ) {
  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    return OpKind::MEMORY_SEG_RSI;
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    return OpKind::MEMORY_SEG_ESI;
  } else {
    return OpKind::MEMORY_SEG_SI;
  }
}

[[maybe_unused]]
static inline OpKind get_seg_di_mem_kind( const Decoder& decoder ) {
  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    return OpKind::MEMORY_SEG_RDI;
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    return OpKind::MEMORY_SEG_EDI;
  } else {
    return OpKind::MEMORY_SEG_DI;
  }
}

// OpCodeHandler_Yb_Reg: STOSB (ES:DI, AL)
void OpCodeHandler_Yb_Reg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Yb_Reg*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_kind( get_es_di_mem_kind( decoder ) );
  instr.set_op1_register( self->reg );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_Yv_Reg: STOSW/STOSD/STOSQ (ES:DI, AX/EAX/RAX)
void OpCodeHandler_Yv_Reg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Yv_Reg*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );
  instr.set_op0_kind( get_es_di_mem_kind( decoder ) );

  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op1_register( Register::RAX );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op1_register( Register::AX );
  } else {
    instr.set_op1_register( Register::EAX );
  }
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_Yv_Reg2: INSW/INSD (ES:DI, DX)
void OpCodeHandler_Yv_Reg2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Yv_Reg2*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  instr.set_code( op_size != OpSize::SIZE16 ? self->code32 : self->code16 );
  instr.set_op0_kind( get_es_di_mem_kind( decoder ) );
  instr.set_op1_register( Register::DX );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_Reg_Xb: LODSB (AL, DS:SI)
void OpCodeHandler_Reg_Xb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Xb*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_register( self->reg );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_kind( get_seg_si_mem_kind( decoder ) );
}

// OpCodeHandler_Reg_Xv: LODSW/LODSD/LODSQ (AX/EAX/RAX, DS:SI)
void OpCodeHandler_Reg_Xv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Xv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op0_register( Register::RAX );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op0_register( Register::AX );
  } else {
    instr.set_op0_register( Register::EAX );
  }
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_kind( get_seg_si_mem_kind( decoder ) );
}

// OpCodeHandler_Reg_Xv2: OUTSW/OUTSD (DX, DS:SI)
void OpCodeHandler_Reg_Xv2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Xv2*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  instr.set_code( op_size != OpSize::SIZE16 ? self->code32 : self->code16 );
  instr.set_op0_register( Register::DX );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_kind( get_seg_si_mem_kind( decoder ) );
}

// OpCodeHandler_Reg_Yb: SCASB (AL, ES:DI)
void OpCodeHandler_Reg_Yb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Yb*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_register( self->reg );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_kind( get_es_di_mem_kind( decoder ) );
}

// OpCodeHandler_Reg_Yv: SCASW/SCASD/SCASQ (AX/EAX/RAX, ES:DI)
void OpCodeHandler_Reg_Yv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Yv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op0_register( Register::RAX );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op0_register( Register::AX );
  } else {
    instr.set_op0_register( Register::EAX );
  }
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_kind( get_es_di_mem_kind( decoder ) );
}

// OpCodeHandler_Yb_Xb: MOVSB (ES:DI, DS:SI)
void OpCodeHandler_Yb_Xb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Yb_Xb*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_kind( get_es_di_mem_kind( decoder ) );
  instr.set_op1_kind( get_seg_si_mem_kind( decoder ) );
}

// OpCodeHandler_Yv_Xv: MOVSW/MOVSD/MOVSQ (ES:DI, DS:SI)
void OpCodeHandler_Yv_Xv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Yv_Xv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );
  instr.set_op0_kind( get_es_di_mem_kind( decoder ) );
  instr.set_op1_kind( get_seg_si_mem_kind( decoder ) );
}

// OpCodeHandler_Xb_Yb: CMPSB (DS:SI, ES:DI)
void OpCodeHandler_Xb_Yb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Xb_Yb*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_kind( get_seg_si_mem_kind( decoder ) );
  instr.set_op1_kind( get_es_di_mem_kind( decoder ) );
}

// OpCodeHandler_Xv_Yv: CMPSW/CMPSD/CMPSQ (DS:SI, ES:DI)
void OpCodeHandler_Xv_Yv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Xv_Yv*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );
  instr.set_op0_kind( get_seg_si_mem_kind( decoder ) );
  instr.set_op1_kind( get_es_di_mem_kind( decoder ) );
}

// ============================================================================
// Segment register handlers
// ============================================================================

// OpCodeHandler_Sw_Ev: MOV Sreg, Ev
void OpCodeHandler_Sw_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Sw_Ev*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op0: Sreg (segment register from reg field)
  uint32_t sreg_idx = decoder.state().reg;
  // Can't load CS (index 1) or invalid segment register (> 5)
  // But don't return early - need to read memory operand for correct length
  if ( sreg_idx == 1 || sreg_idx > 5 ) {
    decoder.set_invalid_instruction();
  }
  instr.set_op0_register( add_reg( Register::ES, sreg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev (always read to get correct instruction length)
  Register reg_base = get_gpr_base( op_size );
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( reg_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// ============================================================================
// Far pointer handlers
// ============================================================================

// OpCodeHandler_Ap: Far JMP/CALL ptr16:16/32
void OpCodeHandler_Ap::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ap*>( self_ptr );
  auto op_size = decoder.state().operand_size;

  if ( op_size != OpSize::SIZE16 ) {
    instr.set_code( self->code32 );
    instr.set_op0_kind( OpKind::FAR_BRANCH32 );
    auto off = decoder.read_u32();
    if ( off ) {
      instr.set_far_branch32( *off );
    }
    auto seg = decoder.read_u16();
    if ( seg ) {
      instr.set_far_branch_selector( *seg );
    }
  } else {
    instr.set_code( self->code16 );
    instr.set_op0_kind( OpKind::FAR_BRANCH16 );
    // Read 32-bit value: low 16 bits = offset, high 16 bits = selector
    auto d = decoder.read_u32();
    if ( d ) {
      instr.set_far_branch16( static_cast<uint16_t>( *d ) );
      instr.set_far_branch_selector( static_cast<uint16_t>( *d >> 16 ) );
    }
  }
}

// ============================================================================
// Offset handlers (MOV with moffs)
// ============================================================================

// OpCodeHandler_Reg_Ob: MOV AL, moffs8
void OpCodeHandler_Reg_Ob::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Ob*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op0_register( self->reg );
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_kind( OpKind::MEMORY );

  // Memory displacement only (no base/index)
  instr.set_memory_base( Register::NONE );
  instr.set_memory_index( Register::NONE );

  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    auto disp = decoder.read_u64();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    auto disp = decoder.read_u32();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else {
    auto disp = decoder.read_u16();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  }
}

// OpCodeHandler_Ob_Reg: MOV moffs8, AL
void OpCodeHandler_Ob_Reg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ob_Reg*>( self_ptr );
  instr.set_code( self->code );
  instr.set_op1_register( self->reg );
  instr.set_op1_kind( OpKind::REGISTER );
  instr.set_op0_kind( OpKind::MEMORY );

  instr.set_memory_base( Register::NONE );
  instr.set_memory_index( Register::NONE );

  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    auto disp = decoder.read_u64();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    auto disp = decoder.read_u32();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else {
    auto disp = decoder.read_u16();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  }
}

// OpCodeHandler_Reg_Ov: MOV rAX, moffs16/32/64
void OpCodeHandler_Reg_Ov::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reg_Ov*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op0_register( Register::RAX );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op0_register( Register::AX );
  } else {
    instr.set_op0_register( Register::EAX );
  }
  instr.set_op0_kind( OpKind::REGISTER );
  instr.set_op1_kind( OpKind::MEMORY );

  instr.set_memory_base( Register::NONE );
  instr.set_memory_index( Register::NONE );

  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    auto disp = decoder.read_u64();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    auto disp = decoder.read_u32();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else {
    auto disp = decoder.read_u16();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  }
}

// OpCodeHandler_Ov_Reg: MOV moffs16/32/64, rAX
void OpCodeHandler_Ov_Reg::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ov_Reg*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  if ( op_size == OpSize::SIZE64 ) {
    instr.set_op1_register( Register::RAX );
  } else if ( op_size == OpSize::SIZE16 ) {
    instr.set_op1_register( Register::AX );
  } else {
    instr.set_op1_register( Register::EAX );
  }
  instr.set_op1_kind( OpKind::REGISTER );
  instr.set_op0_kind( OpKind::MEMORY );

  instr.set_memory_base( Register::NONE );
  instr.set_memory_index( Register::NONE );

  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    auto disp = decoder.read_u64();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    auto disp = decoder.read_u32();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  } else {
    auto disp = decoder.read_u16();
    if ( disp ) {
      instr.set_memory_displacement64( *disp );
    }
  }
}

// ============================================================================
// Branch handlers
// ============================================================================

// OpCodeHandler_BranchIw: RET imm16
void OpCodeHandler_BranchIw::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_BranchIw*>( self_ptr );

  // Read immediate word first
  instr.set_op0_kind( OpKind::IMMEDIATE16 );
  auto imm = decoder.read_u16();
  if ( imm ) {
    instr.set_immediate16( *imm );
  }

  // Select code based on mode
  if ( decoder.is_64bit_mode() ) {
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      instr.set_code( self->code64 );
    } else {
      instr.set_code( self->code16 );
    }
  } else {
    if ( decoder.state().operand_size == OpSize::SIZE32 ) {
      instr.set_code( self->code32 );
    } else {
      instr.set_code( self->code16 );
    }
  }
}

// OpCodeHandler_BranchSimple: RET (no operands)
void OpCodeHandler_BranchSimple::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_BranchSimple*>( self_ptr );

  if ( decoder.is_64bit_mode() ) {
    if ( decoder.state().operand_size != OpSize::SIZE16 ) {
      instr.set_code( self->code64 );
    } else {
      instr.set_code( self->code16 );
    }
  } else {
    if ( decoder.state().operand_size == OpSize::SIZE32 ) {
      instr.set_code( self->code32 );
    } else {
      instr.set_code( self->code16 );
    }
  }
}

// ============================================================================
// Iw_Ib handler
// ============================================================================

// OpCodeHandler_Iw_Ib: ENTER imm16, imm8
void OpCodeHandler_Iw_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Iw_Ib*>( self_ptr );
  auto op_size = decoder.state().operand_size;
  Code codes[] = { self->code16, self->code32, self->code64 };
  instr.set_code( codes[static_cast<std::size_t>( op_size )] );

  // Op0: Iw
  instr.set_op0_kind( OpKind::IMMEDIATE16 );
  auto imm16 = decoder.read_u16();
  if ( imm16 ) {
    instr.set_immediate16( *imm16 );
  }

  // Op1: Ib
  instr.set_op1_kind( OpKind::IMMEDIATE8_2ND );
  auto imm8 = decoder.read_byte();
  if ( imm8 ) {
    instr.set_immediate8_2nd( *imm8 );
  }
}

// ============================================================================
// FPU handlers
// ============================================================================

// OpCodeHandler_ST_STi: FPU ST, ST(i)
void OpCodeHandler_ST_STi::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_ST_STi*>( self_ptr );
  instr.set_code( self->code );

  // Op0: ST(0)
  instr.set_op0_register( Register::ST0 );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: ST(i) from r/m field
  uint32_t sti_idx = decoder.state().rm;
  instr.set_op1_register( add_reg( Register::ST0, sti_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// OpCodeHandler_STi: FPU ST(i)
void OpCodeHandler_STi::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_STi*>( self_ptr );
  instr.set_code( self->code );

  // Op0: ST(i) from r/m field
  uint32_t sti_idx = decoder.state().rm;
  instr.set_op0_register( add_reg( Register::ST0, sti_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

// OpCodeHandler_STi_ST: FPU ST(i), ST
void OpCodeHandler_STi_ST::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_STi_ST*>( self_ptr );
  instr.set_code( self->code );

  // Op0: ST(i) from r/m field
  uint32_t sti_idx = decoder.state().rm;
  instr.set_op0_register( add_reg( Register::ST0, sti_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: ST(0)
  instr.set_op1_register( Register::ST0 );
  instr.set_op1_kind( OpKind::REGISTER );
}

// ============================================================================
// MMX/SSE handlers
// ============================================================================

// OpCodeHandler_P_Q: Pq, Qq (MMX)
void OpCodeHandler_P_Q::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_P_Q*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Pq (MMX register from reg field)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Qq (MMX register or memory from r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op1_register( add_reg( Register::MM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_Q_P: Qq, Pq (MMX)
void OpCodeHandler_Q_P::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Q_P*>( self_ptr );
  instr.set_code( self->code );

  // Op1: Pq (MMX register from reg field)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op1_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );

  // Op0: Qq (MMX register or memory from r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op0_register( add_reg( Register::MM0, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

// OpCodeHandler_P_Q_Ib: Pq, Qq, Ib
void OpCodeHandler_P_Q_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_P_Q_Ib*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Pq
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Qq
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op1_register( add_reg( Register::MM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }

  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_P_W: Pq, Wx
void OpCodeHandler_P_W::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_P_W*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Pq (MMX register)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Wx (XMM or memory)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_P_R: Pq, Rx
void OpCodeHandler_P_R::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_P_R*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Pq (MMX register)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Rx (XMM register only)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// OpCodeHandler_P_Ev: Pq, Ev
void OpCodeHandler_P_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_P_Ev*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );

  // Op0: Pq (MMX register)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev
  Register gpr_base = decoder.is_64bit_mode() ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( gpr_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_P_Ev_Ib: Pq, Ev, Ib
void OpCodeHandler_P_Ev_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_P_Ev_Ib*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );

  // Op0: Pq
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ev
  Register gpr_base = decoder.is_64bit_mode() ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( gpr_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }

  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_NIb: Nq, Ib (MMX with immediate)
void OpCodeHandler_NIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_NIb*>( self_ptr );
  instr.set_code( self->code );

  // Op0: Nq (MMX register from r/m field)
  uint32_t rm_idx = decoder.state().rm;
  instr.set_op0_register( add_reg( Register::MM0, rm_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );

  // Op1: Ib
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

void OpCodeHandler_Reservednop::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Reservednop*>( self_ptr );
  auto& entry = self->handler;
  entry.decode( entry.handler, decoder, instruction );
}

// OpCodeHandler_Ed_V_Ib: Ed/Eq, V, Ib (PEXTR*)
void OpCodeHandler_Ed_V_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_Ed_V_Ib*>( self_ptr );
  
  // Op1: XMM register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate)
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  
  Register gpr;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    gpr = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    gpr = Register::EAX;
  }
  
  // Op0: Ed/Eq or memory
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( gpr, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
  
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_VM: V, M (memory-only operand)
void OpCodeHandler_VM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VM*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: XMM register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Memory only (no register form)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_VN: V, N (XMM, MMX register-only)
void OpCodeHandler_VN::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VN*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: XMM register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: MMX register only (no memory form)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op1_register( add_reg( Register::MM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// OpCodeHandler_VQ: V, Q (XMM, MMX/mem)
void OpCodeHandler_VQ::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VQ*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: XMM register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: MMX register or memory
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op1_register( add_reg( Register::MM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_VRIbIb: V, R, Ib, Ib (INSERTQ/EXTRQ)
void OpCodeHandler_VRIbIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VRIbIb*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: XMM register (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op2 and Op3: Two immediate bytes
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_op3_kind( OpKind::IMMEDIATE8_2ND );
  
  // Op1: XMM register (r/m field) - register only
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
  
  // Read two immediate bytes
  auto imm1 = decoder.read_byte();
  auto imm2 = decoder.read_byte();
  if ( imm1 ) {
    instr.set_immediate8( *imm1 );
  }
  if ( imm2 ) {
    instr.set_immediate8_2nd( *imm2 );
  }
}

void OpCodeHandler_VW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VW*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: XMM register (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Memory or XMM register (r/m field)
  if ( decoder.state().mod_ < 3 ) {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  } else {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  }
}

void OpCodeHandler_VWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VWIb*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: XMM register (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Memory or XMM register (r/m field)
  if ( decoder.state().mod_ < 3 ) {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  } else {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  }
  
  // Op2: Immediate byte
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_VX_Ev: VX, Ev (MOVD/MOVQ etc.)
void OpCodeHandler_VX_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VX_Ev*>( self_ptr );
  
  Register gpr;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    gpr = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    gpr = Register::EAX;
  }
  
  // Op0: XMM register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: GPR or memory
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_VX_E_Ib: VX, E, Ib (PINSRW etc.)
void OpCodeHandler_VX_E_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VX_E_Ib*>( self_ptr );
  
  Register gpr;
  if ( ( decoder.state().flags & StateFlags::W ) != 0 ) {
    instr.set_code( self->code64 );
    gpr = Register::RAX;
  } else {
    instr.set_code( self->code32 );
    gpr = Register::EAX;
  }
  
  // Op0: XMM register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op2: Immediate
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  
  // Op1: GPR or memory
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
  
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// OpCodeHandler_V_Ev: V, Ev (CVTSI2SS/CVTSI2SD)
void OpCodeHandler_V_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_V_Ev*>( self_ptr );
  
  Register gpr;
  if ( decoder.state().operand_size != OpSize::SIZE64 ) {
    instr.set_code( self->code32 );
    gpr = Register::EAX;
  } else {
    instr.set_code( self->code64 );
    gpr = Register::RAX;
  }
  
  // Op0: XMM register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: GPR or memory
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_WV: W, V (MOVNTSS/etc - memory or XMM, XMM)
void OpCodeHandler_WV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_WV*>( self_ptr );
  instr.set_code( self->code );
  
  // Op1: XMM register (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op0: Memory or XMM register (r/m field)
  if ( decoder.state().mod_ < 3 ) {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  } else {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  }
}

// OpCodeHandler_rDI_P_N: rDI, P, N (MASKMOVQ)
void OpCodeHandler_rDI_P_N::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_rDI_P_N*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: Memory operand (seg:rDI)
  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    instr.set_op0_kind( OpKind::MEMORY_SEG_RDI );
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    instr.set_op0_kind( OpKind::MEMORY_SEG_EDI );
  } else {
    instr.set_op0_kind( OpKind::MEMORY_SEG_DI );
  }
  
  // Op1: MMX register (reg field)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op1_register( add_reg( Register::MM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: MMX register (r/m field) - register only
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op2_register( add_reg( Register::MM0, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// OpCodeHandler_rDI_VX_RX: rDI, VX, RX (MASKMOVDQU)
void OpCodeHandler_rDI_VX_RX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_rDI_VX_RX*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: Memory operand (seg:rDI)
  if ( decoder.state().address_size == OpSize::SIZE64 ) {
    instr.set_op0_kind( OpKind::MEMORY_SEG_RDI );
  } else if ( decoder.state().address_size == OpSize::SIZE32 ) {
    instr.set_op0_kind( OpKind::MEMORY_SEG_EDI );
  } else {
    instr.set_op0_kind( OpKind::MEMORY_SEG_DI );
  }
  
  // Op1: XMM register (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: XMM register (r/m field) - register only
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// ============================================================================
// MPX handlers
// ============================================================================

// OpCodeHandler_B_BM: BND, BND/M (BNDMOV, BNDCN, BNDCU, BNDMK)
void OpCodeHandler_B_BM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_B_BM*>( self_ptr );
  
  // Validate BND register index (0-3)
  if ( decoder.state().reg > 3 ) {
    decoder.set_invalid_instruction();
  }
  
  if ( decoder.is_64bit_mode() ) {
    instr.set_code( self->code64 );
  } else {
    instr.set_code( self->code32 );
  }
  
  // Op0: BND register
  instr.set_op0_register( add_reg( Register::BND0, decoder.state().reg & 3 ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: BND register or memory
  if ( decoder.state().mod_ == 3 ) {
    if ( decoder.state().rm > 3 ) {
      decoder.set_invalid_instruction();
    }
    instr.set_op1_register( add_reg( Register::BND0, decoder.state().rm & 3 ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

// OpCodeHandler_BM_B: BND/M, BND
void OpCodeHandler_BM_B::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_BM_B*>( self_ptr );
  
  // Validate BND register index (0-3)
  if ( decoder.state().reg > 3 ) {
    decoder.set_invalid_instruction();
  }
  
  if ( decoder.is_64bit_mode() ) {
    instr.set_code( self->code64 );
  } else {
    instr.set_code( self->code32 );
  }
  
  // Op1: BND register
  instr.set_op1_register( add_reg( Register::BND0, decoder.state().reg & 3 ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op0: BND register or memory
  if ( decoder.state().mod_ == 3 ) {
    if ( decoder.state().rm > 3 ) {
      decoder.set_invalid_instruction();
    }
    instr.set_op0_register( add_reg( Register::BND0, decoder.state().rm & 3 ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
}

void OpCodeHandler_B_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_B_Ev*>( self_ptr );
  instr.set_code( decoder.is_64bit_mode() ? self->code64 : self->code32 );
}

// OpCodeHandler_B_MIB: BND, MIB (memory indexed by base)
void OpCodeHandler_B_MIB::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_B_MIB*>( self_ptr );
  
  // Validate BND register index (0-3)
  if ( decoder.state().reg > 3 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: BND register
  instr.set_op0_register( add_reg( Register::BND0, decoder.state().reg & 3 ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Memory only
  if ( decoder.state().mod_ < 3 ) {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  } else {
    decoder.set_invalid_instruction();
  }
}

// OpCodeHandler_MIB_B: MIB, BND
void OpCodeHandler_MIB_B::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_MIB_B*>( self_ptr );
  
  // Validate BND register index (0-3)
  if ( decoder.state().reg > 3 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op1: BND register
  instr.set_op1_register( add_reg( Register::BND0, decoder.state().reg & 3 ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op0: Memory only
  if ( decoder.state().mod_ < 3 ) {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  } else {
    decoder.set_invalid_instruction();
  }
}

// ============================================================================
// RIb handler
// ============================================================================

// OpCodeHandler_RIb: R (XMM register), Ib
void OpCodeHandler_RIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_RIb*>( self_ptr );
  instr.set_code( self->code );
  
  // Op1: Immediate byte
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
  
  // Op0: XMM register (r/m field) - register only
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// OpCodeHandler_RIbIb: R (XMM register), Ib, Ib (EXTRQ)
void OpCodeHandler_RIbIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_RIbIb*>( self_ptr );
  instr.set_code( self->code );
  
  // Op1 and Op2: Two immediate bytes
  instr.set_op1_kind( OpKind::IMMEDIATE8 );
  instr.set_op2_kind( OpKind::IMMEDIATE8_2ND );
  
  // Op0: XMM register (r/m field) - register only
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( Register::XMM0, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
  
  // Read two immediate bytes
  auto imm1 = decoder.read_byte();
  auto imm2 = decoder.read_byte();
  if ( imm1 ) {
    instr.set_immediate8( *imm1 );
  }
  if ( imm2 ) {
    instr.set_immediate8_2nd( *imm2 );
  }
}

// ============================================================================
// Wbinvd handler
// ============================================================================

void OpCodeHandler_Wbinvd::decode( const OpCodeHandler* /*self_ptr*/, Decoder& /*decoder*/, Instruction& instr ) {
  instr.set_code( Code::WBINVD );
}

// ============================================================================
// VEX Handlers
// ============================================================================

// Helper to get XMM/YMM/ZMM register based on vector length
static Register get_vec_reg( Register base_reg, uint32_t index, VectorLength vl ) {
  // base_reg specifies the base register class (XMM0, YMM0, or ZMM0).
  // The index is added to the appropriate base depending on vector length.
  // For VEX instructions, base_reg is typically XMM0 and vl determines the actual size.
  
  // Determine the register class and offset within that class
  uint32_t base_val = static_cast<uint32_t>( base_reg );
  uint32_t xmm0_val = static_cast<uint32_t>( Register::XMM0 );
  uint32_t ymm0_val = static_cast<uint32_t>( Register::YMM0 );
  uint32_t zmm0_val = static_cast<uint32_t>( Register::ZMM0 );
  
  // Calculate offset within the XMM/YMM/ZMM class
  uint32_t offset;
  if ( base_val >= zmm0_val ) {
    offset = base_val - zmm0_val;
  } else if ( base_val >= ymm0_val ) {
    offset = base_val - ymm0_val;
  } else {
    offset = base_val - xmm0_val;
  }
  
  // Select the appropriate base based on vector length
  Register actual_base;
  if ( vl == VectorLength::L512 ) {
    actual_base = Register::ZMM0;
  } else if ( vl == VectorLength::L256 ) {
    actual_base = Register::YMM0;
  } else {
    actual_base = Register::XMM0;
  }
  
  return add_reg( actual_base, offset + index );
}

// VEX W handler - dispatches to W=0 or W=1 handler based on W flag
void OpCodeHandler_VEX_W::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_W*>( self_ptr );
  auto& handler = ( decoder.state().flags & StateFlags::W ) ? self->handler_w1 : self->handler_w0;
  // ModRM is already read by the parent handler - just forward to the child
  handler.decode( handler.handler, decoder, instruction );
}

// VEX VectorLength handler
void OpCodeHandler_VEX_VectorLength::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VectorLength*>( self_ptr );
  auto& handler = ( decoder.state().vector_length == VectorLength::L256 ) ? self->handler_l1 : self->handler_l0;
  handler.decode( handler.handler, decoder, instruction );
}

// VEX VectorLength handler (no ModRM)
void OpCodeHandler_VEX_VectorLength_NoModRM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VectorLength_NoModRM*>( self_ptr );
  auto& handler = ( decoder.state().vector_length == VectorLength::L256 ) ? self->handler_l1 : self->handler_l0;
  handler.decode( handler.handler, decoder, instruction );
}

// VEX MandatoryPrefix handler
void OpCodeHandler_VEX_MandatoryPrefix2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_MandatoryPrefix2*>( self_ptr );
  uint32_t index = static_cast<uint32_t>( decoder.state().mandatory_prefix );
  if ( index >= 4 ) index = 0;
  auto& handler = self->handlers[index];
  handler.decode( handler.handler, decoder, instruction );
}

// VEX Simple - no operands
void OpCodeHandler_VEX_Simple::decode( const OpCodeHandler* self_ptr, Decoder& /*decoder*/, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Simple*>( self_ptr );
  instr.set_code( self->code );
}

// VEX VHW - V=dest, H=vvvv, W=rm (3 operand XMM/YMM)
void OpCodeHandler_VEX_VHW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHW*>( self_ptr );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field) - destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg1, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( get_vec_reg( self->base_reg2, vvvv_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field) - can be register or memory
  if ( decoder.state().mod_ == 3 ) {
    instr.set_code( self->code_r );  // Use register variant code
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( get_vec_reg( self->base_reg3, rm_idx, vl ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    instr.set_code( self->code_m );  // Use memory variant code
    decoder.read_op_mem( instr, 2 );
  }
}

// VEX VW - V=dest, W=rm (2 operand)
void OpCodeHandler_VEX_VW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VW*>( self_ptr );
  
  // Validate: vvvv must be 1111 (unused) for 2-operand instructions
  if ( ( decoder.state().vvvv_invalid_check & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg1, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( get_vec_reg( self->base_reg2, rm_idx, vl ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
}

// VEX VWIb - V=dest, W=rm, Ib
void OpCodeHandler_VEX_VWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VWIb*>( self_ptr );
  
  // Select code based on W bit
  bool w_bit = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w_bit ? self->code_w1 : self->code_w0 );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg1, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( get_vec_reg( self->base_reg2, rm_idx, vl ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
  
  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// VEX VHWIb - V=dest, H=vvvv, W=rm, Ib
void OpCodeHandler_VEX_VHWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHWIb*>( self_ptr );
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg1, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv)
  instr.set_op1_register( get_vec_reg( self->base_reg2, decoder.state().vvvv, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( get_vec_reg( self->base_reg3, rm_idx, vl ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 2 );
  }
  
  // Op3: Ib
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// VEX WV - W=dest(rm), V=src(reg)
void OpCodeHandler_VEX_WV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_WV*>( self_ptr );
  
  // Validate vvvv (must be 0 for instructions that don't use vvvv)
  if ( ( decoder.state().vvvv_invalid_check & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: W (r/m) - destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( get_vec_reg( self->base_reg1, rm_idx, vl ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: V (reg) - source
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( get_vec_reg( self->base_reg2, reg_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// VEX VM - V=dest, M=memory only
void OpCodeHandler_VEX_VM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VM*>( self_ptr );
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: M (memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    decoder.read_op_mem( instr, 1 );
  }
}

// VEX MV - M=dest, V=src
void OpCodeHandler_VEX_MV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_MV*>( self_ptr );
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: M (memory only)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  decoder.read_op_mem( instr, 0 );
  
  // Op1: V (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// VEX M - Memory only
void OpCodeHandler_VEX_M::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_M*>( self_ptr );
  instr.set_code( self->code );
  
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    decoder.read_op_mem( instr, 0 );
  }
}

// VEX VHM - V=dest, H=vvvv, M=memory
void OpCodeHandler_VEX_VHM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHM*>( self_ptr );
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv)
  instr.set_op1_register( get_vec_reg( self->base_reg, decoder.state().vvvv, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: M (memory)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
  } else {
    decoder.read_op_mem( instr, 2 );
  }
}

// VEX MHV - M=dest, H=vvvv, V=src
void OpCodeHandler_VEX_MHV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_MHV*>( self_ptr );
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: M (memory)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  decoder.read_op_mem( instr, 0 );
  
  // Op1: H (vvvv)
  instr.set_op1_register( get_vec_reg( self->base_reg, decoder.state().vvvv, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: V (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op2_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

// VEX VHEv - V=dest(xmm/ymm), H=vvvv, Ev=gpr r/m
void OpCodeHandler_VEX_VHEv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHEv*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv)
  instr.set_op1_register( get_vec_reg( self->base_reg, decoder.state().vvvv, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ev (gpr r/m)
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 2 );
  }
}

// VEX VHEvIb
void OpCodeHandler_VEX_VHEvIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHEvIb*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv)
  instr.set_op1_register( get_vec_reg( self->base_reg, decoder.state().vvvv, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ev (gpr r/m)
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 2 );
  }
  
  // Op3: Ib
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// VEX Ev_VX - Ev=dest, VX=src(xmm)
void OpCodeHandler_VEX_Ev_VX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Ev_VX*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  // Op0: Ev (gpr r/m)
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: VX (xmm reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

// VEX VX_Ev - VX=dest, Ev=src
void OpCodeHandler_VEX_VX_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VX_Ev*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  // Op0: VX (xmm reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (gpr r/m)
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
}

// VEX Gv_W - Gv=dest, W=src(xmm/mem)
void OpCodeHandler_VEX_Gv_W::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_W*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  // Op0: Gv (gpr reg)
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( base_gpr, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (xmm/mem)
  auto vl = decoder.state().vector_length;
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
}

// VEX Gv_RX - Gv=dest, RX=src(xmm reg only)
void OpCodeHandler_VEX_Gv_RX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_RX*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  // Op0: Gv (gpr reg)
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( base_gpr, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: RX (xmm reg only)
  if ( decoder.state().mod_ == 3 ) {
    auto vl = decoder.state().vector_length;
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.set_invalid_instruction();
  }
}

// VEX Gv_Ev - Gv=dest, Ev=src (BMI)
void OpCodeHandler_VEX_Gv_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_Ev*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Gv (gpr reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( base_gpr, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (gpr r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
}

// VEX Ev - single Ev operand
void OpCodeHandler_VEX_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Ev*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Ev (gpr r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 0 );
  }
}

// VEX Ed_V_Ib
void OpCodeHandler_VEX_Ed_V_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Ed_V_Ib*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Ed (gpr r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: V (xmm reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// VEX GvM_VX_Ib
void OpCodeHandler_VEX_GvM_VX_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_GvM_VX_Ib*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  // Op0: Gv or M
  if ( decoder.state().mod_ == 3 ) {
    Register base_gpr = is_w ? Register::RAX : Register::EAX;
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: VX (xmm reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// VEX Gv_Ev_Ib (BMI)
void OpCodeHandler_VEX_Gv_Ev_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_Ev_Ib*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Gv (vvvv)
  instr.set_op0_register( add_reg( base_gpr, decoder.state().vvvv ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
  
  // Op2: Ib
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  auto imm = decoder.read_byte();
  if ( imm ) {
    instr.set_immediate8( *imm );
  }
}

// VEX Gv_Ev_Id (RORX with imm32)
void OpCodeHandler_VEX_Gv_Ev_Id::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_Ev_Id*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Gv (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( base_gpr, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
  
  // Op2: Id (imm32)
  instr.set_op2_kind( OpKind::IMMEDIATE32 );
  auto imm = decoder.read_u32();
  if ( imm ) {
    instr.set_immediate32( *imm );
  }
}

// VEX Ev_Gv_Gv (BMI2)
void OpCodeHandler_VEX_Ev_Gv_Gv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Ev_Gv_Gv*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Ev (r/m) - destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: Gv (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( add_reg( base_gpr, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Gv2 (vvvv)
  instr.set_op2_register( add_reg( base_gpr, decoder.state().vvvv ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

// VEX Gv_Ev_Gv (BMI2)
void OpCodeHandler_VEX_Gv_Ev_Gv::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_Ev_Gv*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Gv (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( base_gpr, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 1 );
  }
  
  // Op2: Gv2 (vvvv)
  instr.set_op2_register( add_reg( base_gpr, decoder.state().vvvv ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

// VEX Gv_Gv_Ev (BMI)
void OpCodeHandler_VEX_Gv_Gv_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_Gv_Ev*>( self_ptr );
  
  bool is_w = ( decoder.state().flags & StateFlags::W ) != 0;
  instr.set_code( is_w && self->code64 != Code::INVALID ? self->code64 : self->code32 );
  
  Register base_gpr = is_w ? Register::RAX : Register::EAX;
  
  // Op0: Gv (reg)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( base_gpr, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Gv2 (vvvv)
  instr.set_op1_register( add_reg( base_gpr, decoder.state().vvvv ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ev (r/m)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( add_reg( base_gpr, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    decoder.read_op_mem( instr, 2 );
  }
}

// VEX Group handler
void OpCodeHandler_VEX_Group::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Group*>( self_ptr );
  auto& handler = self->handlers[decoder.state().reg];
  handler.decode( handler.handler, decoder, instruction );
}

// VEX Bitness handler
void OpCodeHandler_VEX_Bitness::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Bitness*>( self_ptr );
  auto& handler = decoder.is_64bit_mode() ? self->handler_64 : self->handler_1632;
  handler.decode( handler.handler, decoder, instruction );
}

// VEX Bitness DontReadModRM
void OpCodeHandler_VEX_Bitness_DontReadModRM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Bitness_DontReadModRM*>( self_ptr );
  auto& handler = decoder.is_64bit_mode() ? self->handler_64 : self->handler_1632;
  handler.decode( handler.handler, decoder, instruction );
}

// VEX RM handler
void OpCodeHandler_VEX_RM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_RM*>( self_ptr );
  auto& handler = ( decoder.state().mod_ == 3 ) ? self->handler_reg : self->handler_mem;
  handler.decode( handler.handler, decoder, instruction );
}

// VEX Options DontReadModRM
void OpCodeHandler_VEX_Options_DontReadModRM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Options_DontReadModRM*>( self_ptr );
  auto& handler = ( ( decoder.options() & self->decoder_options ) != 0 ) ? self->handler_option : self->handler_default;
  handler.decode( handler.handler, decoder, instruction );
}

// VEX handlers for miscellaneous instructions

void OpCodeHandler_VEX_Gv_GPR_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gv_GPR_Ib*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: Gv (reg field) - GPR destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  Register gpr_base = w ? Register::RAX : Register::EAX;
  instr.set_op0_register( add_reg( gpr_base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: GPR (r/m field) - GPR source
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_VEX_Hv_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Hv_Ev*>( self_ptr );
  
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  Register gpr_base = w ? Register::RAX : Register::EAX;
  
  // Op0: Hv (vvvv field) - GPR destination
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op0_register( add_reg( gpr_base, vvvv_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (r/m field) - GPR or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( gpr_base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

void OpCodeHandler_VEX_Hv_Ed_Id::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Hv_Ed_Id*>( self_ptr );
  
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  Register gpr_base = w ? Register::RAX : Register::EAX;
  
  // Op0: Hv (vvvv field) - GPR destination
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op0_register( add_reg( gpr_base, vvvv_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ed (r/m field) - 32-bit GPR or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( add_reg( Register::EAX, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
  
  // Op2: Id (immediate dword)
  auto imm = decoder.read_u32();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE32 );
  instr.set_immediate32( *imm );
}

void OpCodeHandler_VEX_HRIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_HRIb*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: H (vvvv field) - vector register destination
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op0_register( get_vec_reg( self->base_reg, vvvv_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: R (r/m field) - vector register source (reg only)
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op1_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_VEX_rDI_VX_RX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_rDI_VX_RX*>( self_ptr );
  
  // Register-only instruction  
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: rDI - implied DI/EDI/RDI based on address size
  // For 64-bit mode, use RDI; for 32-bit, use EDI
  instr.set_op0_register( decoder.bitness() == 64 ? Register::RDI : Register::EDI );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: VX (reg field) - vector register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: RX (r/m field) - vector register
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op2_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_RdRq::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_RdRq*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  Register gpr_base = w ? Register::RAX : Register::EAX;
  
  // Op0: Rd/Rq (r/m field) - GPR
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op0_register( add_reg( gpr_base, rm_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_WHV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_WHV*>( self_ptr );
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: W (r/m field) - destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( get_vec_reg( self->base_reg, vvvv_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op2_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VWH::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VWH*>( self_ptr );
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field) - destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op1_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
  
  // Op2: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op2_register( get_vec_reg( self->base_reg, vvvv_idx, vl ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_WVIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_WVIb*>( self_ptr );
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: W (r/m field) - destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op0_register( get_vec_reg( self->base_reg1, rm_idx, vl ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op1_register( get_vec_reg( self->base_reg2, reg_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_VEX_VHWIs4::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHWIs4*>( self_ptr );
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field) - destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( get_vec_reg( self->base_reg, vvvv_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 2 );
  }
  
  // Op3: Is4 (immediate byte encodes register in bits 7:4)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  uint32_t is4_reg = ( *imm >> 4 ) & 0xF;
  instr.set_op3_register( get_vec_reg( self->base_reg, is4_reg, vl ) );
  instr.set_op3_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VHIs4W::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHIs4W*>( self_ptr );
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field) - destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( get_vec_reg( self->base_reg, vvvv_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Is4 (immediate byte encodes register in bits 7:4) - read immediate first
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  uint32_t is4_reg = ( *imm >> 4 ) & 0xF;
  instr.set_op2_register( get_vec_reg( self->base_reg, is4_reg, vl ) );
  instr.set_op2_kind( OpKind::REGISTER );
  
  // Op3: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op3_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op3_kind( OpKind::REGISTER );
  } else {
    instr.set_op3_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 3 );
  }
}

void OpCodeHandler_VEX_VHWIs5::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHWIs5*>( self_ptr );
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field) - destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( get_vec_reg( self->base_reg, vvvv_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op2_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 2 );
  }
  
  // Op3: Is5 (immediate byte encodes register in bits 7:4)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  uint32_t is5_reg = ( *imm >> 4 ) & 0xF;
  instr.set_op3_register( get_vec_reg( self->base_reg, is5_reg, vl ) );
  instr.set_op3_kind( OpKind::REGISTER );
  
  // Op4: Lower 4 bits of immediate as immediate value
  instr.set_op4_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( *imm & 0xF );
}

void OpCodeHandler_VEX_VHIs5W::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VHIs5W*>( self_ptr );
  
  instr.set_code( self->code );
  
  auto vl = decoder.state().vector_length;
  
  // Op0: V (reg field) - destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( get_vec_reg( self->base_reg, reg_idx, vl ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( get_vec_reg( self->base_reg, vvvv_idx, vl ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Is5 (immediate byte encodes register in bits 7:4) - read immediate first
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  uint32_t is5_reg = ( *imm >> 4 ) & 0xF;
  instr.set_op2_register( get_vec_reg( self->base_reg, is5_reg, vl ) );
  instr.set_op2_kind( OpKind::REGISTER );
  
  // Op3: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    instr.set_op3_register( get_vec_reg( self->base_reg, rm_idx, vl ) );
    instr.set_op3_kind( OpKind::REGISTER );
  } else {
    instr.set_op3_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 3 );
  }
  
  // Op4: Lower 4 bits of immediate as immediate value
  instr.set_op4_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( *imm & 0xF );
}

void OpCodeHandler_VEX_VK_HK_RK::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VK_HK_RK*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: VK (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: HK (vvvv field) - mask register source
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( Register::K0, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: RK (r/m field) - mask register source
  uint32_t rm_idx = decoder.state().rm;
  instr.set_op2_register( add_reg( Register::K0, rm_idx ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VK_RK::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VK_RK*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: VK (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: RK (r/m field) - mask register source
  uint32_t rm_idx = decoder.state().rm;
  instr.set_op1_register( add_reg( Register::K0, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VK_RK_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VK_RK_Ib*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: VK (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: RK (r/m field) - mask register source
  uint32_t rm_idx = decoder.state().rm;
  instr.set_op1_register( add_reg( Register::K0, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_VEX_VK_WK::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VK_WK*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: VK (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: WK (r/m field) - mask register or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op1_register( add_reg( Register::K0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

void OpCodeHandler_VEX_VK_R::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VK_R*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: VK (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: R (r/m field) - GPR source
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op1_register( add_reg( self->gpr, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VK_R_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VK_R_Ib*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: VK (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: R (r/m field) - GPR source
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
  instr.set_op1_register( add_reg( self->gpr, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_VEX_G_VK::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_G_VK*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: G (reg field) - GPR destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( self->gpr, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: VK (r/m field) - mask register source
  uint32_t rm_idx = decoder.state().rm;
  instr.set_op1_register( add_reg( Register::K0, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_M_VK::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_M_VK*>( self_ptr );
  
  // Memory-only instruction
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: M (r/m field) - memory destination
  instr.set_op0_kind( OpKind::MEMORY );
  decoder.read_op_mem( instr, 0 );
  
  // Op1: VK (reg field) - mask register source
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op1_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_Gq_HK_RK::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Gq_HK_RK*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: Gq (reg field) - 64-bit GPR destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( Register::RAX, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: HK (vvvv field) - mask register source
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( Register::K0, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: RK (r/m field) - mask register source
  uint32_t rm_idx = decoder.state().rm;
  instr.set_op2_register( add_reg( Register::K0, rm_idx ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VX_VSIB_HX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // VEX gather instructions: V, VSIB, H (three operand form)
  // Format: V (dest), VSIB_mem (source), H (vvvv, mask/second dest)
  // Example: VGATHERDPS xmm2, [rax+xmm1*4], xmm3
  // The mask register (H) is modified during execution
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VX_VSIB_HX*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: V (reg field) - destination vector register
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  instr.set_op0_register( add_reg( self->base_reg1, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: VSIB memory operand
  if ( decoder.state().mod_ == 3 ) {
    // VSIB requires memory operand
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_op1_kind( OpKind::MEMORY );
  // VEX doesn't have tuple type, use 0 (N1)
  decoder.read_op_mem_vsib( instr, 1, self->vsib_base, 0 );
  
  // Op2: H (vvvv field) - mask register (second vector that gets cleared)
  instr.set_op2_register( add_reg( self->base_reg3, decoder.state().vvvv ) );
  instr.set_op2_kind( OpKind::REGISTER );
  
}

void OpCodeHandler_VEX_VT_SIBMEM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // AMX TILELOADD/TILELOADDT1: tile register dest, SIBMEM src
  // Format: TMM, sibmem
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VT_SIBMEM*>( self_ptr );
  
  // Validation: vvvv must be 1111, no register extension allowed for tile regs
  if ( ( ( decoder.state().vvvv_invalid_check | decoder.state().extra_register_base ) & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: TMM (reg field) - tile register destination (TMM0-TMM7)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::TMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: SIBMEM - memory operand (requires SIB byte)
  if ( decoder.state().mod_ == 3 ) {
    // Register form is invalid for tile load
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_op1_kind( OpKind::MEMORY );
  decoder.read_op_mem( instr, 1 );
}

void OpCodeHandler_VEX_SIBMEM_VT::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // AMX TILESTORED: SIBMEM dest, tile register src
  // Format: sibmem, TMM
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_SIBMEM_VT*>( self_ptr );
  
  // Validation: vvvv must be 1111, no register extension allowed for tile regs
  if ( ( ( decoder.state().vvvv_invalid_check | decoder.state().extra_register_base ) & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: SIBMEM - memory operand destination (requires SIB byte)
  if ( decoder.state().mod_ == 3 ) {
    // Register form is invalid for tile store
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_op0_kind( OpKind::MEMORY );
  decoder.read_op_mem( instr, 0 );
  
  // Op1: TMM (reg field) - tile register source (TMM0-TMM7)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op1_register( add_reg( Register::TMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VT::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // AMX TILEZERO/TILERELEASE: single tile register operand
  // Format: TMM
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VT*>( self_ptr );
  
  // Validation: vvvv must be 1111, no register extension allowed for tile regs
  if ( ( ( decoder.state().vvvv_invalid_check | decoder.state().extra_register_base ) & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: TMM (reg field) - tile register (TMM0-TMM7)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::TMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
}

void OpCodeHandler_VEX_VT_RT_HT::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // AMX TDPBF16PS/TDPBSSD/etc: three tile register operands
  // Format: TMM (dest), TMM (rm), TMM (vvvv)
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_VT_RT_HT*>( self_ptr );
  
  // Validation: vvvv must be 0-7, no register extension allowed for tile regs
  if ( decoder.invalid_check_mask() != 0 && ( decoder.state().vvvv > 7 || decoder.state().extra_register_base != 0 ) ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: TMM (reg field) - tile register destination (TMM0-TMM7)
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::TMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op2: TMM (vvvv field) - tile register source 2
  uint32_t vvvv_idx = decoder.state().vvvv & 7;
  instr.set_op2_register( add_reg( Register::TMM0, vvvv_idx ) );
  instr.set_op2_kind( OpKind::REGISTER );
  
  // Op1: TMM (rm field) - tile register source 1
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm;
    instr.set_op1_register( add_reg( Register::TMM0, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Validate that all three registers are different
    if ( decoder.invalid_check_mask() != 0 ) {
      if ( decoder.state().extra_base_register_base != 0 ||
           reg_idx == vvvv_idx || reg_idx == rm_idx || rm_idx == vvvv_idx ) {
        decoder.set_invalid_instruction();
      }
    }
  } else {
    // Memory form is invalid for tile multiply
    decoder.set_invalid_instruction();
    return;
  }
}

void OpCodeHandler_VEX_K_Jb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // KNC/MVEX conditional branch with K mask: JKZD/JKNZD (short)
  // Format: K, rel8
  // Note: The ModRM byte contains the immediate displacement, not a reg/rm field
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_K_Jb*>( self_ptr );
  
  // Validation: vvvv must be 0-7 (K0-K7)
  if ( decoder.invalid_check_mask() != 0 && decoder.state().vvvv > 7 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: K (vvvv field) - mask register
  uint32_t k_idx = decoder.state().vvvv & 7;
  instr.set_op0_register( add_reg( Register::K0, k_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: rel8 - branch target (ModRM byte is the immediate!)
  // The displacement is sign-extended and added to current IP
  int8_t disp = static_cast<int8_t>( decoder.state().modrm );
  uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( decoder.ip() ) + disp );
  instr.set_near_branch64( target );
  instr.set_op1_kind( OpKind::NEAR_BRANCH64 );
}

void OpCodeHandler_VEX_K_Jz::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // KNC/MVEX conditional branch with K mask: JKZD/JKNZD (near)
  // Format: K, rel32
  // Note: The ModRM byte contains the low 8 bits of the immediate displacement
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_K_Jz*>( self_ptr );
  
  // Validation: vvvv must be 0-7 (K0-K7)
  if ( decoder.invalid_check_mask() != 0 && decoder.state().vvvv > 7 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: K (vvvv field) - mask register
  uint32_t k_idx = decoder.state().vvvv & 7;
  instr.set_op0_register( add_reg( Register::K0, k_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: rel32 - branch target
  // ModRM byte has low 8 bits, next 3 bytes have the rest
  uint32_t imm_low = decoder.state().modrm;
  auto byte1 = decoder.read_byte();
  auto word_high = decoder.read_u16();
  if ( !byte1 || !word_high ) {
    decoder.set_invalid_instruction();
    return;
  }
  uint32_t imm = imm_low | ( static_cast<uint32_t>( *byte1 ) << 8 ) | ( static_cast<uint32_t>( *word_high ) << 16 );
  int32_t disp = static_cast<int32_t>( imm );
  uint64_t target = static_cast<uint64_t>( static_cast<int64_t>( decoder.ip() ) + disp );
  instr.set_near_branch64( target );
  instr.set_op1_kind( OpKind::NEAR_BRANCH64 );
}

void OpCodeHandler_VEX_Group8x64::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_VEX_Group8x64*>( self_ptr );
  if ( decoder.state().mod_ == 3 ) {
    uint32_t index = ( decoder.state().modrm & 0x3F );
    auto& handler = self->table_high[index];
    handler.decode( handler.handler, decoder, instruction );
  } else {
    auto& handler = self->table_low[decoder.state().reg];
    handler.decode( handler.handler, decoder, instruction );
  }
}

// ============================================================================
// EVEX Handler Implementations
// ============================================================================

void OpCodeHandler_EVEX_VectorLength::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VectorLength*>( self_ptr );
  auto vl = decoder.state().vector_length;
  HandlerEntry handler;
  switch ( vl ) {
    case VectorLength::L128: handler = self->handler_128; break;
    case VectorLength::L256: handler = self->handler_256; break;
    case VectorLength::L512: handler = self->handler_512; break;
    default: decoder.set_invalid_instruction(); return;
  }
  handler.decode( handler.handler, decoder, instruction );
}

void OpCodeHandler_EVEX_VectorLength_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VectorLength_er*>( self_ptr );
  auto vl = decoder.state().vector_length;
  HandlerEntry handler;
  switch ( vl ) {
    case VectorLength::L128: handler = self->handler_128; break;
    case VectorLength::L256: handler = self->handler_256; break;
    case VectorLength::L512: handler = self->handler_512; break;
    default: decoder.set_invalid_instruction(); return;
  }
  handler.decode( handler.handler, decoder, instruction );
}

void OpCodeHandler_EVEX_W::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_W*>( self_ptr );
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  auto& handler = w ? self->handler_w1 : self->handler_w0;
  handler.decode( handler.handler, decoder, instruction );
}

void OpCodeHandler_EVEX_MandatoryPrefix2::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_MandatoryPrefix2*>( self_ptr );
  uint32_t index = static_cast<uint32_t>( decoder.state().mandatory_prefix );
  if ( index < 4 ) {
    auto& handler = self->handlers[index];
    handler.decode( handler.handler, decoder, instruction );
  } else {
    decoder.set_invalid_instruction();
  }
}

void OpCodeHandler_EVEX_Group::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_Group*>( self_ptr );
  auto& handler = self->handlers[decoder.state().reg];
  handler.decode( handler.handler, decoder, instruction );
}

void OpCodeHandler_EVEX_RM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_RM*>( self_ptr );
  auto& handler = ( decoder.state().mod_ == 3 ) ? self->handler_reg : self->handler_mem;
  handler.decode( handler.handler, decoder, instruction );
}

// Stub implementations for EVEX operand handlers - set invalid for now
// These need proper implementation for EVEX instruction decoding to work

void OpCodeHandler_EVEX_VkW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkW*>( self_ptr );
  
  // Validate: vvvv must be 1111 (unused) for 2-operand instructions
  // Don't return early - continue decoding to get correct instruction length
  if ( ( decoder.state().vvvv_invalid_check & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field) - destination with mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg1, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg2, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) & decoder.invalid_check_mask() ) != 0 ) {
      decoder.set_invalid_instruction();
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    // Handle broadcast
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else if ( decoder.invalid_check_mask() != 0 ) {
        decoder.set_invalid_instruction();
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_VkW_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkW_er*>( self_ptr );
  
  // Validate: vvvv must be 1111 (unused) for 2-operand instructions
  // Don't return early - continue decoding to get correct instruction length
  if ( ( decoder.state().vvvv_invalid_check & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg1, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg2, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    
    // Handle embedded rounding / SAE for reg-reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->only_sae ) {
        instr.set_suppress_all_exceptions( true );
      } else {
        // Embedded rounding: L'L encodes rounding mode (1-4)
        auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
        instr.set_rounding_control( rc );
      }
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    // Handle broadcast for memory form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      instr.set_is_broadcast( true );
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_VkHW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkHW*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  // For EVEX, base_reg already contains the correct register base (XMM0/YMM0/ZMM0)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg1, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg2, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg3, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg (no broadcast/rounding for this handler)
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    // Handle broadcast
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_VkHW_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkHW_er*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    
    // Handle embedded rounding / SAE for reg-reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->only_sae ) {
        instr.set_suppress_all_exceptions( true );
      } else {
        // Embedded rounding: L'L encodes rounding mode (1-4)
        auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
        instr.set_rounding_control( rc );
      }
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    // Handle broadcast for memory form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_VkHW_er_ur::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // This is like VkHW_er but with unconditional rounding (always uses embedded rounding for reg form)
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkHW_er_ur*>( self_ptr );
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    
    // Unconditional rounding: L'L always encodes rounding mode (1-4) for reg form
    auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
    instr.set_rounding_control( rc );
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    // Handle broadcast for memory form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_VkWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkWIb*>( self_ptr );
  
  // Validate: vvvv must be 1111 (unused) for 2-operand instructions
  if ( decoder.state().vvvv_invalid_check != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_VkWIb_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkWIb_er*>( self_ptr );
  
  // Validate: vvvv must be 1111 (unused) for 2-operand instructions
  if ( decoder.state().vvvv_invalid_check != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Handle embedded rounding for reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
      instr.set_rounding_control( rc );
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      instr.set_is_broadcast( true );
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_VkHWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkHWIb*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg1, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg2, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg3, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
  
  // Op3: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_VkHWIb_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkHWIb_er*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Handle embedded rounding for reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
      instr.set_rounding_control( rc );
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
  
  // Op3: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_VkM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkM*>( self_ptr );
  
  // Memory-only instruction - reject register form
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  // Validate: vvvv must be 1111 (unused)
  if ( decoder.state().vvvv_invalid_check != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: M (memory only)
  instr.set_op1_kind( OpKind::MEMORY );
  // Use tuple type for proper displacement scaling
  decoder.read_op_mem_evex( instr, 1, self->tuple_type );
}

void OpCodeHandler_EVEX_VM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VM*>( self_ptr );
  
  // Memory-only instruction - reject register form
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  // Validate: vvvv must be 1111 (unused)
  if ( decoder.state().vvvv_invalid_check != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: V (reg field) - no mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: M (memory only)
  instr.set_op1_kind( OpKind::MEMORY );
  // Use tuple type for proper displacement scaling
  decoder.read_op_mem_evex( instr, 1, self->tuple_type );
}

void OpCodeHandler_EVEX_MV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_MV*>( self_ptr );
  
  // Memory-only instruction - reject register form
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  // Validate: vvvv must be 1111 (unused)
  if ( decoder.state().vvvv_invalid_check != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: M (memory as dest)
  instr.set_op0_kind( OpKind::MEMORY );
  // Use tuple type for proper displacement scaling
  decoder.read_op_mem_evex( instr, 0, self->tuple_type );
  
  // Op1: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_VW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VW*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: V (reg field) - no mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Validate: z must be 0, vvvv must be 1111 (unused), aaa must be 0
  // Don't return early - must read memory operand for correct instruction length
  if ( ( ( ( decoder.state().flags & StateFlags::Z ) | decoder.state().vvvv_invalid_check | decoder.state().aaa ) 
         & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    // Don't return early for reg-reg since no more bytes to read
    if ( ( ( decoder.state().flags & StateFlags::B ) & decoder.invalid_check_mask() ) != 0 ) {
      decoder.set_invalid_instruction();
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_VW_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VW_er*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: V (reg field) - no mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Validate: z must be 0, vvvv must be 1111 (unused), aaa must be 0
  // Don't return early - must read memory operand for correct instruction length
  if ( ( ( ( decoder.state().flags & StateFlags::Z ) | decoder.state().vvvv_invalid_check | decoder.state().aaa ) 
         & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // VW_er always uses SAE (suppress all exceptions) for B bit
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      instr.set_suppress_all_exceptions( true );
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    // B bit must be 0 for memory operand (no broadcast for this handler)
    // Don't return early - must read memory operand for correct instruction length
    if ( ( ( decoder.state().flags & StateFlags::B ) & decoder.invalid_check_mask() ) != 0 ) {
      decoder.set_invalid_instruction();
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_WV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_WV*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Validate: z must be 0, vvvv must be 1111 (unused), aaa must be 0
  // Don't return early - must read memory operand for correct instruction length
  if ( ( ( ( decoder.state().flags & StateFlags::Z ) | decoder.state().vvvv_invalid_check | decoder.state().aaa ) 
         & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  // Op0: W (r/m field) - dest
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op0_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( ( decoder.state().flags & StateFlags::B ) & decoder.invalid_check_mask() ) != 0 ) {
      decoder.set_invalid_instruction();
    }
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 0, self->tuple_type );
  }
  
  // Op1: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_WkV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_WkV*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Validate: vvvv must be 1111 (unused)
  // Don't return early - must read memory operand for correct instruction length
  if ( ( decoder.state().vvvv_invalid_check & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  // Op0: W{k} (r/m field) - dest with mask
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op0_register( add_reg( self->base_reg1, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( ( decoder.state().flags & StateFlags::B ) & decoder.invalid_check_mask() ) != 0 ) {
      decoder.set_invalid_instruction();
    }
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else if ( decoder.invalid_check_mask() != 0 ) {
        decoder.set_invalid_instruction();
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 0, self->tuple_type );
  }
  
  // Op1: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg2, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_WkVIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_WkVIb*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Validate: vvvv must be 1111 (unused)
  // Don't return early - must read memory operand and immediate for correct instruction length
  if ( ( decoder.state().vvvv_invalid_check & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  // Op0: W{k} (r/m field) - dest with mask
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op0_register( add_reg( self->base_reg1, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( ( decoder.state().flags & StateFlags::B ) & decoder.invalid_check_mask() ) != 0 ) {
      decoder.set_invalid_instruction();
    }
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 0, self->tuple_type );
  }
  
  // Op1: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg2, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;  // OK to return here - can't read more bytes
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_WkVIb_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_WkVIb_er*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Validate: vvvv must be 1111 (unused)
  // Don't return early - must read memory operand and immediate for correct instruction length
  if ( ( decoder.state().vvvv_invalid_check & decoder.invalid_check_mask() ) != 0 ) {
    decoder.set_invalid_instruction();
  }
  
  // Op0: W{k} (r/m field) - dest with mask
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op0_register( add_reg( self->base_reg1, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    // Handle embedded rounding for reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
      instr.set_rounding_control( rc );
    }
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 0, self->tuple_type );
  }
  
  // Op1: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg2, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;  // OK to return here - can't read more bytes
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_WkHV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_WkHV*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: W{k} (r/m field) - dest with mask
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op0_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling (WkHV has no tuple_type field, use read_op_mem)
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: V (reg field)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op2_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op2_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_VHW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VHW*>( self_ptr );
  
  // Different code for reg vs mem form
  if ( decoder.state().mod_ == 3 ) {
    instr.set_code( self->code_r );
  } else {
    instr.set_code( self->code_m );
  }
  
  // Op0: V (reg field) - no mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_VHWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VHWIb*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: V (reg field) - no mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
  
  // Op3: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_VHM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VHM*>( self_ptr );
  
  // Memory-only instruction
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: V (reg field) - no mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: M (memory only)
  instr.set_op2_kind( OpKind::MEMORY );
  // Use tuple type for proper displacement scaling
  decoder.read_op_mem_evex( instr, 2, self->tuple_type );
}

void OpCodeHandler_EVEX_VkHM::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkHM*>( self_ptr );
  
  // Memory-only instruction
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: V{k} (reg field) - with mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field)
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: M (memory only)
  instr.set_op2_kind( OpKind::MEMORY );
  // Use tuple type for proper displacement scaling
  decoder.read_op_mem_evex( instr, 2, self->tuple_type );
}

void OpCodeHandler_EVEX_VK::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VK*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: V (reg field) - vector register destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: K (r/m field) - mask register source (only lower 3 bits)
  uint32_t rm_idx = decoder.state().rm & 7;
  instr.set_op1_register( add_reg( Register::K0, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_VkEv_REXW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VkEv_REXW*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: V{k} (reg field) - vector register destination with mask
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (r/m field) - GPR (32 or 64 bit based on W)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op1_register( add_reg( base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

void OpCodeHandler_EVEX_KR::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_KR*>( self_ptr );
  
  // Register-only instruction
  if ( decoder.state().mod_ != 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: K (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: R (r/m field) - vector register source
  uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_KkHW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_KkHW*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: K{k} (reg field) - mask register destination with mask
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field) - vector register
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_KkHWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_KkHWIb*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: K{k} (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field) - vector register
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
  
  // Op3: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_KkHWIb_sae::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_KkHWIb_sae*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: K{k} (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field) - vector register
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Handle SAE for reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      instr.set_suppress_all_exceptions( true );
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
  
  // Op3: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_KkWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_KkWIb*>( self_ptr );
  
  // Validate: vvvv must be 1111 (unused)
  if ( decoder.state().vvvv_invalid_check != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_code( self->code );
  
  // Op0: K{k} (reg field) - mask register destination
  uint32_t reg_idx = decoder.state().reg;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_KP1HW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_KP1HW*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: K (reg field + 1) - mask register destination (K reg is reg+1 to avoid K0)
  uint32_t reg_idx = ( decoder.state().reg + 1 ) & 7;
  instr.set_op0_register( add_reg( Register::K0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field) - vector register
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op2_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 2, self->tuple_type );
  }
}

void OpCodeHandler_EVEX_HkWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_HkWIb*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: H{k} (vvvv field) - destination with mask
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op0_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->can_broadcast ) {
        instr.set_is_broadcast( true );
      } else {
        decoder.set_invalid_instruction();
        return;
      }
    }
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_HWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_HWIb*>( self_ptr );
  
  instr.set_code( self->code );
  
  // Op0: H (vvvv field) - destination without mask
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op0_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field)
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Validate: b bit must be 0 for reg-reg
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      decoder.set_invalid_instruction();
      return;
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    // Use tuple type for proper displacement scaling
    decoder.read_op_mem_evex( instr, 1, self->tuple_type );
  }
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_VSIB_k1::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // EVEX scatter instructions: memory destination with VSIB addressing, k1 mask required
  // Format: VSIB_mem{k1}
  // Example: VPSCATTERDD [rax+xmm1*4]{k1}, xmm2
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VSIB_k1*>( self_ptr );
  
  // Validation: b and z must be 0, vvvv must be 1111 (unused), mask k1-k7 required (aaa != 0)
  if ( decoder.invalid_check_mask() != 0 &&
       ( ( ( static_cast<uint32_t>( decoder.state().flags & ( StateFlags::B | StateFlags::Z ) ) ) |
           ( decoder.state().vvvv_invalid_check & 0xF ) ) != 0 ||
         decoder.state().aaa == 0 ) ) {
    decoder.set_invalid_instruction();
  }
  
   instr.set_code( self->code );

   // Op0: VSIB memory operand
   if ( decoder.state().mod_ == 3 ) {
     // VSIB requires memory operand, register form is invalid
     decoder.set_invalid_instruction();
     return;
   }

   // Set opmask register
   Register opmask = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + decoder.state().aaa );
   instr.set_op_mask( opmask );

   instr.set_op0_kind( OpKind::MEMORY );
   decoder.read_op_mem_vsib( instr, 0, self->vsib_base, self->tuple_type );
}

void OpCodeHandler_EVEX_VSIB_k1_VX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  // EVEX scatter instructions with vector source: VSIB memory + vector register
  // Format: VSIB_mem{k1}, V
  // Example: VPSCATTERDD [rax+xmm1*4]{k1}, xmm2
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VSIB_k1_VX*>( self_ptr );
  
  // Validation: b and z must be 0, vvvv must be 1111 (unused), mask k1-k7 required
  if ( decoder.invalid_check_mask() != 0 &&
       ( ( ( static_cast<uint32_t>( decoder.state().flags & ( StateFlags::B | StateFlags::Z ) ) ) |
           ( decoder.state().vvvv_invalid_check & 0xF ) ) != 0 ||
         decoder.state().aaa == 0 ) ) {
    decoder.set_invalid_instruction();
  }
  
   instr.set_code( self->code );

   // Op0: VSIB memory operand (destination for scatter)
   if ( decoder.state().mod_ == 3 ) {
     decoder.set_invalid_instruction();
     return;
   }

   // Set opmask register
   Register opmask = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + decoder.state().aaa );
   instr.set_op_mask( opmask );

   instr.set_op0_kind( OpKind::MEMORY );
   decoder.read_op_mem_vsib( instr, 0, self->vsib_base, self->tuple_type );

   // Op1: V (reg field) - vector register source
   uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
   instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
   instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_Vk_VSIB::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
   // EVEX gather instructions: vector destination with VSIB memory source
   // Format: V{k}, VSIB_mem
   // Example: VPGATHERDD xmm2{k1}, [rax+xmm1*4]
   auto* self = reinterpret_cast<const OpCodeHandler_EVEX_Vk_VSIB*>( self_ptr );

   // Validation: b and z must be 0, vvvv must be 1111 (unused), mask k1-k7 required
   // Also: dest register and VSIB index must be different
   if ( decoder.invalid_check_mask() != 0 &&
        ( ( ( static_cast<uint32_t>( decoder.state().flags & ( StateFlags::Z | StateFlags::B ) ) ) |
            ( decoder.state().vvvv_invalid_check & 0xF ) ) != 0 ||
          decoder.state().aaa == 0 ) ) {
     decoder.set_invalid_instruction();
   }

   instr.set_code( self->code );

   // Op0: V{k} (reg field) - vector register destination with mask
   uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
   instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
   instr.set_op0_kind( OpKind::REGISTER );

   // Set opmask register
   Register opmask = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + decoder.state().aaa );
   instr.set_op_mask( opmask );
  
  // Op1: VSIB memory operand (source for gather)
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }
  
  instr.set_op1_kind( OpKind::MEMORY );
  decoder.read_op_mem_vsib( instr, 1, self->vsib_base, self->tuple_type );
  
  // Validate that dest register != VSIB index register
  if ( decoder.invalid_check_mask() != 0 ) {
    // VMM_count is 32 (number of ZMM registers), used for modulo to compare register indices
    constexpr uint32_t VMM_count = 32;
    uint32_t vsib_index_reg = static_cast<uint32_t>( instr.memory_index() ) - static_cast<uint32_t>( Register::XMM0 );
    if ( reg_idx == ( vsib_index_reg % VMM_count ) ) {
      decoder.set_invalid_instruction();
    }
  }
}

void OpCodeHandler_EVEX_Ed_V_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_Ed_V_Ib*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: Ed/Eq (r/m field) - GPR destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op0_register( add_reg( base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: V (reg field) - vector register source
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_Ev_VX::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_Ev_VX*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: Ev (r/m field) - GPR destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op0_register( add_reg( base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: VX (reg field) - vector register source (XMM)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
}

void OpCodeHandler_EVEX_Ev_VX_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_Ev_VX_Ib*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: Ev (r/m field) - GPR destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op0_register( add_reg( base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: VX (reg field) - vector register source
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_VX_Ev::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_VX_Ev*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: VX (reg field) - vector register destination (XMM)
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( Register::XMM0, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: Ev (r/m field) - GPR source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op1_register( add_reg( base, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

void OpCodeHandler_EVEX_Gv_W_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_Gv_W_er*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: Gv (reg field) - GPR destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base;
  Register base = w ? Register::RAX : Register::EAX;
  instr.set_op0_register( add_reg( base, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: W (r/m field) - vector register source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base + decoder.state().extra_base_register_base_evex;
    instr.set_op1_register( add_reg( self->base_reg, rm_idx ) );
    instr.set_op1_kind( OpKind::REGISTER );
    // Handle embedded rounding / SAE for reg-reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      if ( self->only_sae ) {
        instr.set_suppress_all_exceptions( true );
      } else {
        auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
        instr.set_rounding_control( rc );
      }
    }
  } else {
    instr.set_op1_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 1 );
  }
}

void OpCodeHandler_EVEX_GvM_VX_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_GvM_VX_Ib*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: Gv/M (r/m field) - GPR or memory destination
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op0_register( add_reg( base, rm_idx ) );
    instr.set_op0_kind( OpKind::REGISTER );
  } else {
    instr.set_op0_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 0 );
  }
  
  // Op1: VX (reg field) - vector register source
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op1_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op2_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

void OpCodeHandler_EVEX_V_H_Ev_er::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_V_H_Ev_er*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: V (reg field) - vector register destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field) - vector register source
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ev (r/m field) - GPR source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op2_register( add_reg( base, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
    // Handle embedded rounding for reg form
    if ( ( decoder.state().flags & static_cast<uint32_t>( StateFlags::B ) ) != 0 ) {
      auto rc = static_cast<RoundingControl>( static_cast<uint32_t>( decoder.state().vector_length ) + 1 );
      instr.set_rounding_control( rc );
    }
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 2 );
  }
}

void OpCodeHandler_EVEX_V_H_Ev_Ib::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instr ) {
  auto* self = reinterpret_cast<const OpCodeHandler_EVEX_V_H_Ev_Ib*>( self_ptr );
  
  // Select code based on W bit
  bool w = ( decoder.state().flags & static_cast<uint32_t>( StateFlags::W ) ) != 0;
  instr.set_code( w ? self->code64 : self->code32 );
  
  // Op0: V (reg field) - vector register destination
  uint32_t reg_idx = decoder.state().reg + decoder.state().extra_register_base + decoder.state().extra_register_base_evex;
  instr.set_op0_register( add_reg( self->base_reg, reg_idx ) );
  instr.set_op0_kind( OpKind::REGISTER );
  
  // Op1: H (vvvv field) - vector register source
  uint32_t vvvv_idx = decoder.state().vvvv;
  instr.set_op1_register( add_reg( self->base_reg, vvvv_idx ) );
  instr.set_op1_kind( OpKind::REGISTER );
  
  // Op2: Ev (r/m field) - GPR source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_idx = decoder.state().rm + decoder.state().extra_base_register_base;
    Register base = w ? Register::RAX : Register::EAX;
    instr.set_op2_register( add_reg( base, rm_idx ) );
    instr.set_op2_kind( OpKind::REGISTER );
  } else {
    instr.set_op2_kind( OpKind::MEMORY );
    decoder.read_op_mem( instr, 2 );
  }
  
  // Op3: Ib (immediate byte)
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instr.set_op3_kind( OpKind::IMMEDIATE8 );
  instr.set_immediate8( static_cast<uint8_t>( *imm ) );
}

// ============================================================================
// MVEX handlers - Stub implementations for constexpr mode
// ============================================================================

} // anonymous namespace

} // namespace internal

// ============================================================================
// MVEX handlers
// ============================================================================

namespace iced_x86 {
namespace internal {

// Helper function for MVEX register/memory conversion
MvexRegMemConv get_mvex_reg_mem_conv(uint32_t sss) {
  // SSS field determines conversion type
  switch (sss) {
    case 0: return MvexRegMemConv::NONE;
    case 1: return MvexRegMemConv::MEM_CONV_BROADCAST1;
    case 2: return MvexRegMemConv::MEM_CONV_BROADCAST4;
    case 3: return MvexRegMemConv::MEM_CONV_FLOAT16;
    case 4: return MvexRegMemConv::MEM_CONV_UINT8;
    case 5: return MvexRegMemConv::MEM_CONV_UINT16;
    case 6: return MvexRegMemConv::MEM_CONV_SINT16;
    case 7: return MvexRegMemConv::NONE; // reserved
    default: return MvexRegMemConv::NONE;
  }
}

// OpCodeHandler_MVEX_EH: Eviction hint dispatcher
void OpCodeHandler_MVEX_EH::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_EH*>( self_ptr );

  // Dispatch based on eviction hint bit
  bool has_eh = (decoder.state().flags & StateFlags::MVEX_EH) != 0;
  const HandlerEntry& handler = has_eh ? self.handler_eh1 : self.handler_eh0;
  decoder.decode_table( handler, instruction );
}

// OpCodeHandler_MVEX_M: Memory-only operand
void OpCodeHandler_MVEX_M::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_M*>( self_ptr );
  instruction.set_code( self.code );

  // Validate vvvv is zero
  if ( decoder.state().vvvv != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // Memory-only operand, no registers
  instruction.set_op_mask( Register::NONE );

  // If mod==3 (register), invalid
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }

  decoder.read_op_mem_evex( instruction, 0, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
  instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
}

// OpCodeHandler_MVEX_MV: Memory destination, ZMM source
void OpCodeHandler_MVEX_MV::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_MV*>( self_ptr );
  instruction.set_code( self.code );

  // Validate vvvv
  if ( decoder.state().vvvv != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // Memory destination
  instruction.set_op0_kind( OpKind::MEMORY );

  // ZMM source register
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op1_register( src_reg );

  // If mod==3 (register), invalid
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }

  decoder.read_op_mem_evex( instruction, 0, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
  instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
}

// OpCodeHandler_MVEX_VW: ZMM destination, ZMM/memory source
void OpCodeHandler_MVEX_VW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_VW*>( self_ptr );
  instruction.set_code( self.code );

  // Validate vvvv
  if ( decoder.state().vvvv != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // ZMM destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    // Register source
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op1_register( src_reg );
  } else {
    // Memory source
    decoder.read_op_mem_evex( instruction, 1, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }
}

// OpCodeHandler_MVEX_VKW: ZMM dest, K mask source, ZMM/memory source
void OpCodeHandler_MVEX_VKW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_VKW*>( self_ptr );
  instruction.set_code( self.code );

  // Validate vvvv ≤ 7 (K0-K7 only)
  if ( decoder.state().vvvv > 7 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // ZMM destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // K mask source from vvvv
  Register mask_reg = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + ( decoder.state().vvvv & 7 ) );
  instruction.set_op1_register( mask_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op2_register( src_reg );
  } else {
    decoder.read_op_mem_evex( instruction, 2, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }
}

// OpCodeHandler_MVEX_VWIb: ZMM dest, ZMM/memory source, immediate byte
void OpCodeHandler_MVEX_VWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_VWIb*>( self_ptr );

  // Use W bit to select code
  Code code = ( decoder.state().flags & StateFlags::W ) != 0 ? self.code : self.code;
  instruction.set_code( code );

  // Validate vvvv
  if ( decoder.state().vvvv != 0 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // ZMM destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op1_register( src_reg );
  } else {
    decoder.read_op_mem_evex( instruction, 1, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }

  // Immediate byte
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instruction.set_op2_kind( OpKind::IMMEDIATE8 );
  instruction.set_immediate8( *imm );
}

// OpCodeHandler_MVEX_VHW: ZMM dest, ZMM source from vvvv, ZMM/memory source
void OpCodeHandler_MVEX_VHW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_VHW*>( self_ptr );
  instruction.set_code( self.code );

  // ZMM destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // ZMM source from vvvv
  uint32_t vvvv_reg_num = ( decoder.state().vvvv & 0x0F );
  Register vvvv_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + vvvv_reg_num );
  instruction.set_op1_register( vvvv_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op2_register( src_reg );
  } else {
    decoder.read_op_mem_evex( instruction, 2, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }
}

// OpCodeHandler_MVEX_VHWIb: ZMM dest, ZMM source from vvvv, ZMM/memory source, immediate
void OpCodeHandler_MVEX_VHWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_VHWIb*>( self_ptr );
  instruction.set_code( self.code );

  // ZMM destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // ZMM source from vvvv
  uint32_t vvvv_reg_num = ( decoder.state().vvvv & 0x0F );
  Register vvvv_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + vvvv_reg_num );
  instruction.set_op1_register( vvvv_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op2_register( src_reg );
  } else {
    decoder.read_op_mem_evex( instruction, 2, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }

  // Immediate byte
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instruction.set_op3_kind( OpKind::IMMEDIATE8 );
  instruction.set_immediate8( *imm );
}

// OpCodeHandler_MVEX_HWIb: H-vector, W-operand, Immediate
void OpCodeHandler_MVEX_HWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_HWIb*>( self_ptr );
  instruction.set_code( self.code );

  // ZMM destination from vvvv
  uint32_t vvvv_reg_num = ( decoder.state().vvvv & 0x0F );
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + vvvv_reg_num );
  instruction.set_op0_register( dst_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op1_register( src_reg );
  } else {
    decoder.read_op_mem_evex( instruction, 1, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }

  // Immediate byte
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instruction.set_op2_kind( OpKind::IMMEDIATE8 );
  instruction.set_immediate8( *imm );
}

// OpCodeHandler_MVEX_KHW: K-mask, H-vector, W-operand
void OpCodeHandler_MVEX_KHW::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_KHW*>( self_ptr );
  instruction.set_code( self.code );

  // K destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // ZMM source from vvvv
  uint32_t vvvv_reg_num = ( decoder.state().vvvv & 0x0F );
  Register vvvv_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + vvvv_reg_num );
  instruction.set_op1_register( vvvv_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op2_register( src_reg );
  } else {
    decoder.read_op_mem_evex( instruction, 2, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }
}

// OpCodeHandler_MVEX_KHWIb: K-mask, H-vector, W-operand, Immediate
void OpCodeHandler_MVEX_KHWIb::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_KHWIb*>( self_ptr );
  instruction.set_code( self.code );

  // K destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // ZMM source from vvvv
  uint32_t vvvv_reg_num = ( decoder.state().vvvv & 0x0F );
  Register vvvv_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + vvvv_reg_num );
  instruction.set_op1_register( vvvv_reg );

  // ZMM or memory source
  if ( decoder.state().mod_ == 3 ) {
    uint32_t rm_reg_num = decoder.state().rm + decoder.state().extra_base_register_base;
    Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + rm_reg_num );
    instruction.set_op2_register( src_reg );
  } else {
    decoder.read_op_mem_evex( instruction, 2, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
    instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
  }

  // Immediate byte
  auto imm = decoder.read_byte();
  if ( !imm ) {
    decoder.set_invalid_instruction();
    return;
  }
  instruction.set_op2_kind( OpKind::IMMEDIATE8 );
  instruction.set_immediate8( *imm );
}

// OpCodeHandler_MVEX_VSIB: VSIB memory-only
void OpCodeHandler_MVEX_VSIB::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_VSIB*>( self_ptr );
  instruction.set_code( self.code );

  // Validate vvvv and require opmask
  if ( decoder.state().vvvv != 0 || decoder.state().aaa == 0 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // Memory-only operand
  instruction.set_op_mask( Register::NONE );

  // If mod==3 (register), invalid
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // VSIB memory with ZMM index
  uint32_t index_reg_num = decoder.state().rm + decoder.state().extra_index_register_base;
  if ( index_reg_num >= 8 ) {
    index_reg_num += decoder.state().extra_index_register_base_vsib;
  }
  Register index_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + index_reg_num );

  uint32_t base_reg_num = decoder.state().extra_base_register_base_evex;
  Register base_reg = base_reg_num != 0 ? static_cast<Register>( static_cast<uint32_t>( Register::RAX ) + base_reg_num - 1 ) : Register::NONE;

  decoder.read_op_mem_vsib( instruction, 0, index_reg, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
  instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
}

// OpCodeHandler_MVEX_VSIB_V: VSIB memory dest, ZMM source
void OpCodeHandler_MVEX_VSIB_V::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_VSIB_V*>( self_ptr );
  instruction.set_code( self.code );

  // Validate vvvv and require opmask
  if ( decoder.state().vvvv != 0 || decoder.state().aaa == 0 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // VSIB memory destination
  instruction.set_op0_kind( OpKind::MEMORY );

  // ZMM source
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register src_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op1_register( src_reg );

  // Opmask register
  Register opmask = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + decoder.state().aaa );
  instruction.set_op_mask( opmask );

  // If mod==3 (register), invalid
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // VSIB memory with ZMM index
  uint32_t index_reg_num = decoder.state().rm + decoder.state().extra_index_register_base;
  if ( index_reg_num >= 8 ) {
    index_reg_num += decoder.state().extra_index_register_base_vsib;
  }
  Register index_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + index_reg_num );

  uint32_t base_reg_num = decoder.state().extra_base_register_base_evex;
  Register base_reg = base_reg_num != 0 ? static_cast<Register>( static_cast<uint32_t>( Register::RAX ) + base_reg_num - 1 ) : Register::NONE;

  decoder.read_op_mem_vsib( instruction, 0, index_reg, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
  instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
}

// OpCodeHandler_MVEX_V_VSIB: ZMM dest, VSIB memory source
void OpCodeHandler_MVEX_V_VSIB::decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction ) {
  const auto& self = *reinterpret_cast<const OpCodeHandler_MVEX_V_VSIB*>( self_ptr );
  instruction.set_code( self.code );

  // Validate vvvv and require opmask
  if ( decoder.state().vvvv != 0 || decoder.state().aaa == 0 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // ZMM destination
  uint32_t reg_num = decoder.state().reg + decoder.state().extra_register_base;
  Register dst_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + reg_num );
  instruction.set_op0_register( dst_reg );

  // Opmask register
  Register opmask = static_cast<Register>( static_cast<uint32_t>( Register::K0 ) + decoder.state().aaa );
  instruction.set_op_mask( opmask );

  // If mod==3 (register), invalid
  if ( decoder.state().mod_ == 3 ) {
    decoder.set_invalid_instruction();
    return;
  }

  // VSIB memory with ZMM index
  uint32_t index_reg_num = decoder.state().rm + decoder.state().extra_index_register_base;
  if ( index_reg_num >= 8 ) {
    index_reg_num += decoder.state().extra_index_register_base_vsib;
  }
  Register index_reg = static_cast<Register>( static_cast<uint32_t>( Register::ZMM0 ) + index_reg_num );

  uint32_t base_reg_num = decoder.state().extra_base_register_base_evex;
  Register base_reg = base_reg_num != 0 ? static_cast<Register>( static_cast<uint32_t>( Register::RAX ) + base_reg_num - 1 ) : Register::NONE;

  // Validate destination register != index register (HW restriction)
  if ( reg_num == index_reg_num ) {
    decoder.set_invalid_instruction();
    return;
  }

  decoder.read_op_mem_vsib( instruction, 1, index_reg, static_cast<uint32_t>(get_mvex_reg_mem_conv( decoder.state().aaa )) );
  instruction.set_mvex_reg_mem_conv( get_mvex_reg_mem_conv( decoder.state().aaa ) );
}

} // namespace internal
} // namespace iced_x86
