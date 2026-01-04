// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/internal/table_deserializer.hpp"
#include "iced_x86/internal/data_legacy.hpp"
#include "iced_x86/internal/data_vex.hpp"
#include "iced_x86/internal/data_evex.hpp"
#include "iced_x86/internal/vex_op_code_handler_kind.hpp"
#include "iced_x86/internal/evex_op_code_handler_kind.hpp"

namespace iced_x86 {
namespace internal {

// ============================================================================
// TableDeserializer Implementation
// ============================================================================

TableDeserializer::TableDeserializer(
  std::span<const uint8_t> data,
  std::size_t max_ids,
  HandlerReaderFn handler_reader
) noexcept
  : reader_( data )
  , handler_reader_( std::move( handler_reader ) )
{
  id_to_handler_.reserve( max_ids );
}

void TableDeserializer::deserialize() {
  while ( reader_.can_read() ) {
    auto kind = static_cast<SerializedDataKind>( reader_.read_u8() );
    switch ( kind ) {
      case SerializedDataKind::HANDLER_REFERENCE: {
        auto handler = read_handler();
        id_to_handler_.emplace_back( handler );
        break;
      }
      case SerializedDataKind::ARRAY_REFERENCE: {
        auto size = static_cast<std::size_t>( reader_.read_compressed_u32() );
        auto handlers = read_handlers( size );
        id_to_handler_.emplace_back( std::move( handlers ) );
        break;
      }
    }
  }
}

std::vector<HandlerEntry> TableDeserializer::table( std::size_t index ) {
  auto& info = id_to_handler_[index];
  if ( auto* handlers = info.get_array() ) {
    return std::move( *handlers );
  }
  // Should not happen for table entries
  return {};
}

LegacyOpCodeHandlerKind TableDeserializer::read_legacy_op_code_handler_kind() noexcept {
  return static_cast<LegacyOpCodeHandlerKind>( reader_.read_u8() );
}

Code TableDeserializer::read_code() noexcept {
  auto v = reader_.read_compressed_u32();
  return static_cast<Code>( v );
}

std::pair<Code, Code> TableDeserializer::read_code2() noexcept {
  auto v = reader_.read_compressed_u32();
  return { static_cast<Code>( v ), static_cast<Code>( v + 1 ) };
}

std::tuple<Code, Code, Code> TableDeserializer::read_code3() noexcept {
  auto v = reader_.read_compressed_u32();
  return { static_cast<Code>( v ), static_cast<Code>( v + 1 ), static_cast<Code>( v + 2 ) };
}

Register TableDeserializer::read_register() noexcept {
  return static_cast<Register>( reader_.read_u8() );
}

uint32_t TableDeserializer::read_decoder_options() noexcept {
  return reader_.read_compressed_u32();
}

uint32_t TableDeserializer::read_handler_flags() noexcept {
  return reader_.read_compressed_u32();
}

uint32_t TableDeserializer::read_legacy_handler_flags() noexcept {
  return reader_.read_compressed_u32();
}

bool TableDeserializer::read_boolean() noexcept {
  return reader_.read_u8() != 0;
}

uint32_t TableDeserializer::read_u32() noexcept {
  return reader_.read_compressed_u32();
}

HandlerEntry TableDeserializer::read_handler() {
  auto result = read_handler_or_null_instance();
  // Assert not null in debug
  return result;
}

HandlerEntry TableDeserializer::read_handler_or_null_instance() {
  std::vector<HandlerEntry> tmp_vec;
  if ( !temp_vecs_.empty() ) {
    tmp_vec = std::move( temp_vecs_.back() );
    temp_vecs_.pop_back();
    tmp_vec.clear();
  }
  tmp_vec.reserve( 1 );

  handler_reader_( *this, tmp_vec );

  auto result = tmp_vec.back();
  tmp_vec.pop_back();
  temp_vecs_.push_back( std::move( tmp_vec ) );
  return result;
}

std::vector<HandlerEntry> TableDeserializer::read_handlers( std::size_t count ) {
  std::vector<HandlerEntry> handlers;
  handlers.reserve( count );

  while ( handlers.size() < count ) {
    auto len = handlers.size();
    handler_reader_( *this, handlers );
    auto added = handlers.size() - len;
    if ( added == 0 ) {
      break;
    }
  }

  return handlers;
}

HandlerEntry TableDeserializer::read_handler_reference() {
  auto index = static_cast<std::size_t>( reader_.read_u8() );
  auto& info = id_to_handler_[index];
  if ( auto* handler = info.get_single() ) {
    return *handler;
  }
  // Should not happen
  return get_invalid_handler();
}

std::vector<HandlerEntry> TableDeserializer::read_array_reference( uint32_t kind ) {
  [[maybe_unused]] auto read_kind = reader_.read_u8();
  // Assert: read_kind == kind
  auto index = static_cast<std::size_t>( reader_.read_u8() );
  auto& info = id_to_handler_[index];
  if ( auto* handlers = info.get_array() ) {
    // Clone the vector (there can be duplicate references)
    return *handlers;
  }
  return {};
}

std::vector<HandlerEntry> TableDeserializer::read_array_reference_no_clone( uint32_t kind ) {
  [[maybe_unused]] auto read_kind = reader_.read_u8();
  // Assert: read_kind == kind
  auto index = static_cast<std::size_t>( reader_.read_u8() );
  return table( index );
}

// ============================================================================
// Legacy Handler Reader
// ============================================================================

// Forward declaration of the legacy handler reader
void read_legacy_handlers( TableDeserializer& deserializer, std::vector<HandlerEntry>& result );

std::vector<HandlerEntry> read_legacy_tables() {
  // Static cache - tables are deserialized only once and reused
  static std::vector<HandlerEntry> cached_table = []() {
    TableDeserializer deserializer(
      std::span<const uint8_t>( g_legacy_tbl_data.data(), g_legacy_tbl_data.size() ),
      LEGACY_MAX_ID_NAMES,
      read_legacy_handlers
    );
    deserializer.deserialize();
    return deserializer.table( LEGACY_HANDLERS_MAP0_INDEX );
  }();
  return cached_table;
}

// ============================================================================
// Legacy Handler Reader Implementation
// ============================================================================

// Helper to translate HandlerFlags to StateFlags
// HandlerFlags::LOCK = 1 << 3, StateFlags::ALLOW_LOCK = 1 << 6
static inline uint32_t handler_flags_to_state_flags( uint32_t handler_flags ) {
  // Only LOCK flag needs translation: shift from bit 3 to bit 6
  return ( handler_flags & ( 1u << 3 ) ) << 3;
}

// Helper to create handlers (heap allocated and leaked - static lifetime)
template<typename T>
HandlerEntry make_handler( T handler ) {
  auto* ptr = new T( std::move( handler ) );
  return { T::decode, reinterpret_cast<const OpCodeHandler*>( ptr ) };
}

void read_legacy_handlers( TableDeserializer& deserializer, std::vector<HandlerEntry>& result ) {
  auto kind = deserializer.read_legacy_op_code_handler_kind();

  switch ( kind ) {
    // ==========================================================================
    // Meta handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::INVALID:
      result.push_back( get_invalid_handler() );
      return;

    case LegacyOpCodeHandlerKind::INVALID_NO_MOD_RM:
      result.push_back( get_invalid_no_modrm_handler() );
      return;

    case LegacyOpCodeHandlerKind::INVALID2:
      result.push_back( get_invalid_handler() );
      result.push_back( get_invalid_handler() );
      return;

    case LegacyOpCodeHandlerKind::DUP: {
      auto count = deserializer.read_u32();
      auto handler = deserializer.read_handler_or_null_instance();
      for ( uint32_t i = 0; i < count; ++i ) {
        result.push_back( handler );
      }
      return;
    }

    case LegacyOpCodeHandlerKind::NULL_:
      result.push_back( get_null_handler() );
      return;

    case LegacyOpCodeHandlerKind::HANDLER_REFERENCE:
      result.push_back( deserializer.read_handler_reference() );
      return;

    case LegacyOpCodeHandlerKind::ARRAY_REFERENCE:
      // Should not happen in handler context
      return;

    // ==========================================================================
    // Bitness handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::BITNESS: {
      auto handler_1632 = deserializer.read_handler();
      auto handler_64 = deserializer.read_handler();
      // has_modrm = false - this is a dispatch handler
      result.push_back( make_handler( OpCodeHandler_Bitness{ false, handler_1632, handler_64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::BITNESS_DONT_READ_MOD_RM: {
      auto handler_1632 = deserializer.read_handler();
      auto handler_64 = deserializer.read_handler();
      // has_modrm = false - this is a dispatch handler
      result.push_back( make_handler( OpCodeHandler_Bitness_DontReadModRM{ false, handler_1632, handler_64 } ) );
      return;
    }

    // ==========================================================================
    // RM handler
    // ==========================================================================

    case LegacyOpCodeHandlerKind::RM: {
      auto handler_reg = deserializer.read_handler();
      auto handler_mem = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_RM{ true, handler_reg, handler_mem } ) );
      return;
    }

    // ==========================================================================
    // Options handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::OPTIONS3: {
      auto default_handler = deserializer.read_handler();
      auto handler1 = deserializer.read_handler();
      auto options1 = deserializer.read_decoder_options();
      // has_modrm = false - this is a dispatch handler
      result.push_back( make_handler( OpCodeHandler_Options{
        false, default_handler, handler1, options1, get_invalid_handler(), 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::OPTIONS5: {
      auto default_handler = deserializer.read_handler();
      auto handler1 = deserializer.read_handler();
      auto options1 = deserializer.read_decoder_options();
      auto handler2 = deserializer.read_handler();
      auto options2 = deserializer.read_decoder_options();
      // has_modrm = false - this is a dispatch handler
      result.push_back( make_handler( OpCodeHandler_Options{
        false, default_handler, handler1, options1, handler2, options2 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::OPTIONS_DONT_READ_MOD_RM: {
      auto default_handler = deserializer.read_handler();
      auto handler1 = deserializer.read_handler();
      auto options = deserializer.read_decoder_options();
      result.push_back( make_handler( OpCodeHandler_Options_DontReadModRM{
        false, default_handler, handler1, options } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::OPTIONS1632_1: {
      auto default_handler = deserializer.read_handler();
      auto handler1 = deserializer.read_handler();
      auto options1 = deserializer.read_decoder_options();
      result.push_back( make_handler( OpCodeHandler_Options1632{
        false, default_handler, handler1, options1, get_invalid_handler(), 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::OPTIONS1632_2: {
      auto default_handler = deserializer.read_handler();
      auto handler1 = deserializer.read_handler();
      auto options1 = deserializer.read_decoder_options();
      auto handler2 = deserializer.read_handler();
      auto options2 = deserializer.read_decoder_options();
      result.push_back( make_handler( OpCodeHandler_Options1632{
        false, default_handler, handler1, options1, handler2, options2 } ) );
      return;
    }

    // ==========================================================================
    // Table/Group handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::ANOTHER_TABLE: {
      auto handlers = deserializer.read_array_reference_no_clone(
        static_cast<uint32_t>( LegacyOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto* h = new OpCodeHandler_AnotherTable{ false, {} };
      for ( std::size_t i = 0; i < 256 && i < handlers.size(); ++i ) {
        h->handlers[i] = handlers[i];
      }
      result.push_back( { OpCodeHandler_AnotherTable::decode, reinterpret_cast<const OpCodeHandler*>( h ) } );
      return;
    }

    case LegacyOpCodeHandlerKind::GROUP: {
      auto handlers = deserializer.read_array_reference(
        static_cast<uint32_t>( LegacyOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto* h = new OpCodeHandler_Group{ true, {} };
      for ( std::size_t i = 0; i < 8 && i < handlers.size(); ++i ) {
        h->group_handlers[i] = handlers[i];
      }
      result.push_back( { OpCodeHandler_Group::decode, reinterpret_cast<const OpCodeHandler*>( h ) } );
      return;
    }

    case LegacyOpCodeHandlerKind::GROUP8X8: {
      auto table_low = deserializer.read_array_reference(
        static_cast<uint32_t>( LegacyOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto table_high = deserializer.read_array_reference(
        static_cast<uint32_t>( LegacyOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto* h = new OpCodeHandler_Group8x8{ true, {}, {} };
      for ( std::size_t i = 0; i < 8 && i < table_low.size(); ++i ) {
        h->table_low[i] = table_low[i];
      }
      for ( std::size_t i = 0; i < 8 && i < table_high.size(); ++i ) {
        h->table_high[i] = table_high[i];
      }
      result.push_back( { OpCodeHandler_Group8x8::decode, reinterpret_cast<const OpCodeHandler*>( h ) } );
      return;
    }

    case LegacyOpCodeHandlerKind::GROUP8X64: {
      auto table_low = deserializer.read_array_reference(
        static_cast<uint32_t>( LegacyOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto table_high = deserializer.read_array_reference(
        static_cast<uint32_t>( LegacyOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto* h = new OpCodeHandler_Group8x64{ true, {}, {} };
      for ( std::size_t i = 0; i < 8 && i < table_low.size(); ++i ) {
        h->table_low[i] = table_low[i];
      }
      for ( std::size_t i = 0; i < 64 && i < table_high.size(); ++i ) {
        h->table_high[i] = table_high[i];
      }
      result.push_back( { OpCodeHandler_Group8x64::decode, reinterpret_cast<const OpCodeHandler*>( h ) } );
      return;
    }

    // ==========================================================================
    // MandatoryPrefix handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::MANDATORY_PREFIX: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      auto h3 = deserializer.read_handler();
      auto h4 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_MandatoryPrefix{
        true, { h1, h2, h3, h4 } } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MANDATORY_PREFIX_NO_MOD_RM: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      auto h3 = deserializer.read_handler();
      auto h4 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_MandatoryPrefix{
        false, { h1, h2, h3, h4 } } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MANDATORY_PREFIX3: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      auto h3 = deserializer.read_handler();
      auto h4 = deserializer.read_handler();
      auto h5 = deserializer.read_handler();
      auto h6 = deserializer.read_handler();
      auto h7 = deserializer.read_handler();
      auto h8 = deserializer.read_handler();
      auto flags = deserializer.read_legacy_handler_flags();
      result.push_back( make_handler( OpCodeHandler_MandatoryPrefix3{
        true, { h1, h2, h3, h4 }, { h5, h6, h7, h8 }, flags } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MANDATORY_PREFIX4: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      auto h3 = deserializer.read_handler();
      auto h4 = deserializer.read_handler();
      auto flags = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_MandatoryPrefix4{
        true, { h1, h2, h3, h4 }, flags } ) );
      return;
    }

    // ==========================================================================
    // VEX/EVEX/XOP/D3NOW entry points
    // ==========================================================================

    case LegacyOpCodeHandlerKind::D3NOW:
      result.push_back( make_handler( OpCodeHandler_D3NOW{ true } ) );
      return;

    case LegacyOpCodeHandlerKind::EVEX: {
      auto handler_mem = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_EVEX{ true, handler_mem } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VEX2: {
      auto handler_mem = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX2{ true, handler_mem } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VEX3: {
      auto handler_mem = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX3{ true, handler_mem } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::XOP: {
      auto handler_reg0 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_XOP{ true, handler_reg0 } ) );
      return;
    }

    // ==========================================================================
    // Simple handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::SIMPLE: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Simple{ false, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE_MOD_RM: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Simple{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE2_3A: {
      auto [c1, c2, c3] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Simple2{ false, c1, c2, c3 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE2_3B: {
      auto c1 = deserializer.read_code();
      auto c2 = deserializer.read_code();
      auto c3 = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Simple2{ false, c1, c2, c3 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE2_IW: {
      auto [c1, c2, c3] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Simple2Iw{ false, c1, c2, c3 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE3: {
      auto [c1, c2, c3] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Simple3{ false, c1, c2, c3 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE4: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Simple4{ false, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE4B: {
      auto c32 = deserializer.read_code();
      auto c64 = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Simple4{ false, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE5: {
      auto [c1, c2, c3] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Simple5{ false, c1, c2, c3 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE5_A32: {
      auto [c1, c2, c3] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Simple5_a32{ false, c1, c2, c3 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE5_MOD_RM_AS: {
      auto [c1, c2, c3] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Simple5_ModRM_as{ true, c1, c2, c3 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SIMPLE_REG: {
      auto code = deserializer.read_code();
      auto index = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_SimpleReg{ false, code, index } ) );
      return;
    }

    // ==========================================================================
    // Register handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::REG: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_Reg{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_IB: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_RegIb{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_IB3: {
      auto index = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_RegIb3{ false, index } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_IZ2: {
      auto index = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_RegIz2{ false, index } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::IB_REG: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_IbReg{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::IB_REG2: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_IbReg2{ false, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_IB2: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Reg_Ib2{ false, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_IZ: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Reg_Iz{ false, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // Immediate handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::IB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Ib{ false, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::IB3: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Ib3{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::IW_IB: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Iw_Ib{ false, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // I/O handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::AL_DX: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_AL_DX{ false, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::DX_AL: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_DX_AL{ false, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::DX_E_AX: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_DX_eAX{ false, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::E_AX_DX: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_eAX_DX{ false, c16, c32 } ) );
      return;
    }

    // ==========================================================================
    // Ev handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::EV_3A: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev{ true, c16, c32, c64, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_3B: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ev{ true, c16, c32, Code::INVALID, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_4: {
      auto [c16, c32, c64] = deserializer.read_code3();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Ev{ true, c16, c32, c64, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_CL: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_CL{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_GV_32_64: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ev_Gv_32_64{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_GV_3A: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_Gv{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_GV_3B: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ev_Gv{ true, c16, c32, Code::INVALID } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_GV_4: {
      auto [c16, c32, c64] = deserializer.read_code3();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Ev_Gv_flags{ true, c16, c32, c64, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_GV_CL: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_Gv_CL{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_GV_IB: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_Gv_Ib{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_GV_REX: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ev_Gv_REX{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_IB_3: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_Ib{ true, c16, c32, c64, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_IB_4: {
      auto [c16, c32, c64] = deserializer.read_code3();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Ev_Ib{ true, c16, c32, c64, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_IB2_3: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_Ib2{ true, c16, c32, c64, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_IB2_4: {
      auto [c16, c32, c64] = deserializer.read_code3();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Ev_Ib2{ true, c16, c32, c64, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_IZ_3: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_Iz{ true, c16, c32, c64, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_IZ_4: {
      auto [c16, c32, c64] = deserializer.read_code3();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Ev_Iz{ true, c16, c32, c64, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_P: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ev_P{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_REXW_1A: {
      auto code = deserializer.read_code();
      auto flags = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_Ev_REXW{ true, code, Code::INVALID, flags } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_REXW: {
      auto [c32, c64] = deserializer.read_code2();
      auto flags = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_Ev_REXW{ true, c32, c64, flags } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_SW: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_Sw{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV_VX: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ev_VX{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EV1: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ev_1{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EVJ: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Evj{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EVW: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Evw{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EW: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ew{ true, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // Eb handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::EB_1: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Eb{ true, code, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EB_2: {
      auto code = deserializer.read_code();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Eb{ true, code, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EB_CL: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Eb_CL{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EB_GB_1: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Eb_Gb{ true, code, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EB_GB_2: {
      auto code = deserializer.read_code();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Eb_Gb{ true, code, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EB_IB_1: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Eb_Ib{ true, code, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EB_IB_2: {
      auto code = deserializer.read_code();
      auto flags = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_Eb_Ib{ true, code, handler_flags_to_state_flags( flags ) } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EB1: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Eb_1{ true, code } ) );
      return;
    }

    // ==========================================================================
    // Gv handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::GB_EB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Gb_Eb{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GDQ_EV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gdq_Ev{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EB: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Eb{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EB_REX: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_Eb_REX{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV_32_64: {
      auto [c32, c64] = deserializer.read_code2();
      auto disallow_reg = deserializer.read_boolean();
      auto disallow_mem = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev_32_64{ true, c32, c64, disallow_reg, disallow_mem } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV_3A: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV_3B: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev{ true, c16, c32, Code::INVALID } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV_IB: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev_Ib{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV_IB_REX: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev_Ib_REX{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV_IZ: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev_Iz{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV_REX: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev_REX{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV2: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev2{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EV3: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Ev3{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_EW: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Ew{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_M: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_M{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_M_AS: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_M_as{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_MA: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_Ma{ true, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_MP_2: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_Mp{ true, c16, c32, Code::INVALID } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_MP_3: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Mp{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_MV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Gv_Mv{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_N: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_N{ true, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_N_IB_REX: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_N_Ib_REX{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_RX: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_RX{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_W: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Gv_W{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GV_M_VX_IB: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_GvM_VX_Ib{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::GD_RD: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Gd_Rd{ true, code } ) );
      return;
    }

    // ==========================================================================
    // Jump handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::JB: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Jb{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::JB2: {
      auto c1 = deserializer.read_code();
      auto c2 = deserializer.read_code();
      auto c3 = deserializer.read_code();
      auto c4 = deserializer.read_code();
      auto c5 = deserializer.read_code();
      auto c6 = deserializer.read_code();
      auto c7 = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Jb2{ false, c1, c2, c3, c4, c5, c6, c7 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::JDISP: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Jdisp{ false, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::JX: {
      auto [c16, c32] = deserializer.read_code2();
      auto c64 = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Jx{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::JZ: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Jz{ false, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // Memory handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::M_1: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_M{ true, code, Code::INVALID } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::M_2: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_M{ true, c1, c2 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::M_REXW_2: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_M_REXW{ true, c32, c64, 0, 0 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::M_REXW_4: {
      auto [c32, c64] = deserializer.read_code2();
      auto flags32 = deserializer.read_handler_flags();
      auto flags64 = deserializer.read_handler_flags();
      result.push_back( make_handler( OpCodeHandler_M_REXW{ true, c32, c64, flags32, flags64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MEM_BX: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_MemBx{ false, code } ) );  // XLAT has no modrm
      return;
    }

    case LegacyOpCodeHandlerKind::MS: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ms{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MF_1: {
      auto code = deserializer.read_code();
      // Single code used for both 16-bit and 32-bit operand sizes
      result.push_back( make_handler( OpCodeHandler_Mf{ true, code, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MF_2A: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Mf{ true, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MF_2B: {
      auto c1 = deserializer.read_code();
      auto c2 = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Mf{ true, c1, c2 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MV: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_MV{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MV_GV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Mv_Gv{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MV_GV_REXW: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Mv_Gv_REXW{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MP: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_MP{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::EP: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ep{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::M_SW: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_M_Sw{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::SW_M: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Sw_M{ true, code } ) );
      return;
    }

    // ==========================================================================
    // Push/Pop handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::PUSH_EV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_PushEv{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PUSH_IB2: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_PushIb2{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PUSH_IZ: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_PushIz{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PUSH_OP_SIZE_REG_4A: {
      auto [c16, c32, c64] = deserializer.read_code3();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_PushOpSizeReg{ false, c16, c32, c64, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PUSH_OP_SIZE_REG_4B: {
      auto [c16, c32] = deserializer.read_code2();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_PushOpSizeReg{ false, c16, c32, Code::INVALID, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PUSH_SIMPLE2: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_PushSimple2{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PUSH_SIMPLE_REG: {
      auto index = deserializer.read_u32();
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_PushSimpleReg{ false, index, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // Rv handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::RV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Rv{ true, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::RV_32_64: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Rv_32_64{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::RV_MW_GW: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_RvMw_Gw{ true, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::RQ: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Rq{ true, code } ) );
      return;
    }

    // ==========================================================================
    // Control/Debug register handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::C_R_3A: {
      auto [c32, c64] = deserializer.read_code2();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_C_R{ true, c32, c64, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::C_R_3B: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_C_R{ true, code, Code::INVALID, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::R_C_3A: {
      auto [c32, c64] = deserializer.read_code2();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_R_C{ true, c32, c64, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::R_C_3B: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_R_C{ true, code, Code::INVALID, reg } ) );
      return;
    }

    // ==========================================================================
    // Far pointer handler
    // ==========================================================================

    case LegacyOpCodeHandlerKind::AP: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ap{ false, c16, c32 } ) );
      return;
    }

    // ==========================================================================
    // Offset handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::OB_REG: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_Ob_Reg{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::OV_REG: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Ov_Reg{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_OB: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_Reg_Ob{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_OV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Reg_Ov{ false, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // Branch handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::BRANCH_IW: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_BranchIw{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::BRANCH_SIMPLE: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_BranchSimple{ false, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // Segment register handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::SW_EV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Sw_Ev{ true, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // String instruction handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::XB_YB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Xb_Yb{ false, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::XV_YV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Xv_Yv{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::YB_REG: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_Yb_Reg{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::YB_XB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Yb_Xb{ false, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::YV_REG: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Yv_Reg{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::YV_REG2: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Yv_Reg2{ false, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::YV_XV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Yv_Xv{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_XB: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_Reg_Xb{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_XV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Reg_Xv{ false, c16, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_XV2: {
      auto [c16, c32] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Reg_Xv2{ false, c16, c32 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_YB: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_Reg_Yb{ false, code, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::REG_YV: {
      auto [c16, c32, c64] = deserializer.read_code3();
      result.push_back( make_handler( OpCodeHandler_Reg_Yv{ false, c16, c32, c64 } ) );
      return;
    }

    // ==========================================================================
    // Xchg handler
    // ==========================================================================

    case LegacyOpCodeHandlerKind::XCHG_REG_R_AX: {
      auto index = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_Xchg_Reg_rAX{ false, index } ) );
      return;
    }

    // ==========================================================================
    // FPU handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::ST_STI: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_ST_STi{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::STI: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_STi{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::STI_ST: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_STi_ST{ true, code } ) );
      return;
    }

    // ==========================================================================
    // MMX/SSE handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::P_EV: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_P_Ev{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::P_EV_IB: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_P_Ev_Ib{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::P_Q: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_P_Q{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::P_Q_IB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_P_Q_Ib{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::P_R: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_P_R{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::P_W: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_P_W{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::Q_P: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_Q_P{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::R_DI_P_N: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_rDI_P_N{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::R_DI_VX_RX: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_rDI_VX_RX{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::NIB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_NIb{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::RESERVEDNOP: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_Reservednop{ true, h1, h2 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::ED_V_IB: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_Ed_V_Ib{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::V_EV: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_V_Ev{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VM: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VM{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VN: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VN{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VQ: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VQ{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VRIB_IB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VRIbIb{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VW_2: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VW{ true, code, Code::INVALID } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VW_3: {
      auto c1 = deserializer.read_code();
      auto c2 = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VW{ true, c1, c2 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VWIB_2: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VWIb{ true, code, Code::INVALID } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VWIB_3: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VWIb{ true, c1, c2 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VX_E_IB: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VX_E_Ib{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::VX_EV: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VX_Ev{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::WV: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_WV{ true, code } ) );
      return;
    }

    // ==========================================================================
    // MPX handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::B_BM: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_B_BM{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::BM_B: {
      auto [c32, c64] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_BM_B{ true, c32, c64 } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::B_EV: {
      auto [c32, c64] = deserializer.read_code2();
      auto riprel = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_B_Ev{ true, c32, c64, riprel } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::B_MIB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_B_MIB{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::MIB_B: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_MIB_B{ true, code } ) );
      return;
    }

    // ==========================================================================
    // RIb handler
    // ==========================================================================

    case LegacyOpCodeHandlerKind::RIB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_RIb{ true, code } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::RIB_IB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_RIbIb{ true, code } ) );
      return;
    }

    // ==========================================================================
    // Misc handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::WBINVD:
      result.push_back( make_handler( OpCodeHandler_Wbinvd{ false } ) );
      return;

    // ==========================================================================
    // Prefix handlers
    // ==========================================================================

    case LegacyOpCodeHandlerKind::PREFIX_ES_CS_SS_DS: {
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_PrefixEsCsSsDs{ false, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PREFIX_FS_GS: {
      auto reg = deserializer.read_register();
      result.push_back( make_handler( OpCodeHandler_PrefixFsGs{ false, reg } ) );
      return;
    }

    case LegacyOpCodeHandlerKind::PREFIX66:
      result.push_back( make_handler( OpCodeHandler_Prefix66{ false } ) );
      return;

    case LegacyOpCodeHandlerKind::PREFIX67:
      result.push_back( make_handler( OpCodeHandler_Prefix67{ false } ) );
      return;

    case LegacyOpCodeHandlerKind::PREFIX_F0:
      result.push_back( make_handler( OpCodeHandler_PrefixF0{ false } ) );
      return;

    case LegacyOpCodeHandlerKind::PREFIX_F2:
      result.push_back( make_handler( OpCodeHandler_PrefixF2{ false } ) );
      return;

    case LegacyOpCodeHandlerKind::PREFIX_F3:
      result.push_back( make_handler( OpCodeHandler_PrefixF3{ false } ) );
      return;

    case LegacyOpCodeHandlerKind::PREFIX_REX: {
      auto handler = deserializer.read_handler();
      auto rex = deserializer.read_u32();
      result.push_back( make_handler( OpCodeHandler_PrefixREX{ false, handler, rex } ) );
      return;
    }

    default:
      // Unknown handler kind - treat as invalid
      result.push_back( get_invalid_handler() );
      return;
  }
}

// ============================================================================
// VEX Handler Reader
// ============================================================================

void read_vex_handlers( TableDeserializer& deserializer, std::vector<HandlerEntry>& result ) {
  auto kind = static_cast<VexOpCodeHandlerKind>( deserializer.read_u8() );

  switch ( kind ) {
    // ==========================================================================
    // Meta handlers
    // ==========================================================================

    case VexOpCodeHandlerKind::INVALID:
      result.push_back( get_invalid_handler() );
      return;

    case VexOpCodeHandlerKind::INVALID2:
      result.push_back( get_invalid_handler() );
      result.push_back( get_invalid_handler() );
      return;

    case VexOpCodeHandlerKind::DUP: {
      auto count = deserializer.read_u32();
      auto handler = deserializer.read_handler_or_null_instance();
      for ( uint32_t i = 0; i < count; ++i ) {
        result.push_back( handler );
      }
      return;
    }

    case VexOpCodeHandlerKind::NULL_:
      result.push_back( get_null_handler() );
      return;

    case VexOpCodeHandlerKind::INVALID_NO_MOD_RM:
      // For VEX, invalid opcodes should still read modrm to match Rust behavior
      result.push_back( get_invalid_handler() );
      return;

    case VexOpCodeHandlerKind::HANDLER_REFERENCE:
      result.push_back( deserializer.read_handler_reference() );
      return;

    case VexOpCodeHandlerKind::ARRAY_REFERENCE:
      // Should not happen in handler context
      return;

    // ==========================================================================
    // Dispatch handlers
    // ==========================================================================

    case VexOpCodeHandlerKind::BITNESS: {
      auto handler_1632 = deserializer.read_handler();
      auto handler_64 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_Bitness{ true, handler_1632, handler_64 } ) );
      return;
    }

    case VexOpCodeHandlerKind::BITNESS_DONT_READ_MOD_RM: {
      auto handler_1632 = deserializer.read_handler();
      auto handler_64 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_Bitness_DontReadModRM{ false, handler_1632, handler_64 } ) );
      return;
    }

    case VexOpCodeHandlerKind::RM: {
      auto handler_reg = deserializer.read_handler();
      auto handler_mem = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_RM{ true, handler_reg, handler_mem } ) );
      return;
    }

    case VexOpCodeHandlerKind::GROUP: {
      auto handlers = deserializer.read_array_reference(
        static_cast<uint32_t>( VexOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto* h = new OpCodeHandler_VEX_Group{ true, {} };
      for ( std::size_t i = 0; i < 8 && i < handlers.size(); ++i ) {
        h->handlers[i] = handlers[i];
      }
      result.push_back( { OpCodeHandler_VEX_Group::decode, reinterpret_cast<const OpCodeHandler*>( h ) } );
      return;
    }

    case VexOpCodeHandlerKind::GROUP8X64: {
      auto table_low = deserializer.read_array_reference(
        static_cast<uint32_t>( VexOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto table_high = deserializer.read_array_reference(
        static_cast<uint32_t>( VexOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto* h = new OpCodeHandler_Group8x64{ true, {}, {} };
      for ( std::size_t i = 0; i < 8 && i < table_low.size(); ++i ) {
        h->table_low[i] = table_low[i];
      }
      for ( std::size_t i = 0; i < 64 && i < table_high.size(); ++i ) {
        h->table_high[i] = table_high[i];
      }
      result.push_back( { OpCodeHandler_Group8x64::decode, reinterpret_cast<const OpCodeHandler*>( h ) } );
      return;
    }

    case VexOpCodeHandlerKind::W: {
      auto handler_w0 = deserializer.read_handler();
      auto handler_w1 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_W{ true, handler_w0, handler_w1 } ) );
      return;
    }

    case VexOpCodeHandlerKind::MANDATORY_PREFIX2_1: {
      auto h_none = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_MandatoryPrefix2{
        true, { h_none, get_invalid_handler(), get_invalid_handler(), get_invalid_handler() } } ) );
      return;
    }

    case VexOpCodeHandlerKind::MANDATORY_PREFIX2_4: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      auto h3 = deserializer.read_handler();
      auto h4 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_MandatoryPrefix2{
        true, { h1, h2, h3, h4 } } ) );
      return;
    }

    case VexOpCodeHandlerKind::MANDATORY_PREFIX2_NO_MOD_RM: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      auto h3 = deserializer.read_handler();
      auto h4 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_MandatoryPrefix2{
        false, { h1, h2, h3, h4 } } ) );
      return;
    }

    case VexOpCodeHandlerKind::VECTOR_LENGTH_NO_MOD_RM: {
      auto handler_128 = deserializer.read_handler();
      auto handler_256 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_VectorLength_NoModRM{ false, handler_128, handler_256 } ) );
      return;
    }

    case VexOpCodeHandlerKind::VECTOR_LENGTH: {
      auto handler_128 = deserializer.read_handler();
      auto handler_256 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_VEX_VectorLength{ true, handler_128, handler_256 } ) );
      return;
    }

    case VexOpCodeHandlerKind::OPTIONS_DONT_READ_MOD_RM: {
      auto default_handler = deserializer.read_handler();
      auto handler1 = deserializer.read_handler();
      auto options = deserializer.read_decoder_options();
      result.push_back( make_handler( OpCodeHandler_Options_DontReadModRM{
        false, default_handler, handler1, options } ) );
      return;
    }

    // ==========================================================================
    // VEX instruction handlers
    // ==========================================================================

    case VexOpCodeHandlerKind::SIMPLE: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_Simple{ false, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHW_2: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHW{ true, reg, reg, reg, code, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHW_3: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_VHW{ true, reg, reg, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHW_4: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto reg3 = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHW{ true, reg1, reg2, reg3, code, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHWIB_2: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHWIb{ true, reg, reg, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHWIB_4: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto reg3 = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHWIb{ true, reg1, reg2, reg3, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VW_2: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VW{ true, reg, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VW_3: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VW{ true, reg1, reg2, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VWIB_2: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      // VWIb struct: has_modrm, base_reg1, base_reg2, code_w0, code_w1
      result.push_back( make_handler( OpCodeHandler_VEX_VWIb{ true, reg, reg, code, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VWIB_3: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      // VWIb struct: has_modrm, base_reg1, base_reg2, code_w0, code_w1
      result.push_back( make_handler( OpCodeHandler_VEX_VWIb{ true, reg, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::WV: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      // WV struct: has_modrm, base_reg1, base_reg2, code
      result.push_back( make_handler( OpCodeHandler_VEX_WV{ true, reg, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::WVIB: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_WVIb{ true, reg1, reg2, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VM: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VM{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::MV: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_MV{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::M: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_M{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHM: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHM{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::MHV: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_MHV{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VWH: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VWH{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::WHV: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_WHV{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHEV: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_VHEv{ true, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHEV_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_VHEvIb{ true, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::EV_VX: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Ev_VX{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::VX_EV: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_VX_Ev{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::ED_V_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Ed_V_Ib{ true, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_M_VX_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_GvM_VX_Ib{ true, reg, c1, c2 } ) );
      return;
    }

    // ==========================================================================
    // BMI handlers
    // ==========================================================================

    case VexOpCodeHandlerKind::GV_EV: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_Ev{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::EV: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Ev{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_EV_GV: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_Ev_Gv{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::EV_GV_GV: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Ev_Gv_Gv{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_GV_EV: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_Gv_Ev{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_EV_IB: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_Ev_Ib{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_EV_ID: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_Ev_Id{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_GPR_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_GPR_Ib{ true, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_RX: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_RX{ true, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::GV_W: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Gv_W{ true, reg, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::HV_EV: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Hv_Ev{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::HV_ED_ID: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_Hv_Ed_Id{ true, c1, c2 } ) );
      return;
    }

    case VexOpCodeHandlerKind::HRIB: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_HRIb{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::R_DI_VX_RX: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_rDI_VX_RX{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::RD_RQ: {
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_VEX_RdRq{ true, c1, c2 } ) );
      return;
    }

    // ==========================================================================
    // FMA handlers (VHWIs4, VHIs4W, etc.)
    // ==========================================================================

    case VexOpCodeHandlerKind::VHWIS4: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHWIs4{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHIS4_W: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHIs4W{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHWIS5: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHWIs5{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VHIS5_W: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VHIs5W{ true, reg, code } ) );
      return;
    }

    // ==========================================================================
    // K-register handlers (AVX-512 mask ops in VEX encoding)
    // ==========================================================================

    case VexOpCodeHandlerKind::VK_HK_RK: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VK_HK_RK{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VK_RK: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VK_RK{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VK_RK_IB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VK_RK_Ib{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VK_WK: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VK_WK{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VK_R: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      // VK_R struct: has_modrm, gpr, code
      result.push_back( make_handler( OpCodeHandler_VEX_VK_R{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VK_R_IB: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      // VK_R_Ib struct: has_modrm, gpr, code
      result.push_back( make_handler( OpCodeHandler_VEX_VK_R_Ib{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::G_VK: {
      auto code = deserializer.read_code();
      auto reg = deserializer.read_register();
      // G_VK struct: has_modrm, gpr, code
      result.push_back( make_handler( OpCodeHandler_VEX_G_VK{ true, reg, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::M_VK: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_M_VK{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::GQ_HK_RK: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_Gq_HK_RK{ true, code } ) );
      return;
    }

    // ==========================================================================
    // VSIB handlers
    // ==========================================================================

    case VexOpCodeHandlerKind::VX_VSIB_HX: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto reg3 = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VX_VSIB_HX{ true, reg1, reg2, reg3, code } ) );
      return;
    }

    // ==========================================================================
    // AMX handlers
    // ==========================================================================

    case VexOpCodeHandlerKind::VT_SIBMEM: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VT_SIBMEM{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::SIBMEM_VT: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_SIBMEM_VT{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VT: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VT{ true, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::VT_RT_HT: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_VT_RT_HT{ true, code } ) );
      return;
    }

    // ==========================================================================
    // Jump handlers (APX)
    // ==========================================================================

    case VexOpCodeHandlerKind::K_JB: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_K_Jb{ false, code } ) );
      return;
    }

    case VexOpCodeHandlerKind::K_JZ: {
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_VEX_K_Jz{ false, code } ) );
      return;
    }

    default:
      // Unknown handler kind - treat as invalid
      result.push_back( get_invalid_handler() );
      return;
  }
}

// ============================================================================
// VEX Table Deserialization
// ============================================================================

std::vector<std::vector<HandlerEntry>> read_vex_tables() {
  // Static cache - tables are deserialized only once and reused
  static std::vector<std::vector<HandlerEntry>> cached_tables = []() {
    TableDeserializer deserializer(
      std::span<const uint8_t>( g_vex_tbl_data.data(), g_vex_tbl_data.size() ),
      VEX_MAX_ID_NAMES,
      read_vex_handlers
    );
    deserializer.deserialize();

    // Return 3 tables: 0F, 0F38, 0F3A (MAP0 is not used for VEX instructions)
    std::vector<std::vector<HandlerEntry>> tables( 3 );
    tables[0] = deserializer.table( VEX_HANDLERS_0F_INDEX );
    tables[1] = deserializer.table( VEX_HANDLERS_0F38_INDEX );
    tables[2] = deserializer.table( VEX_HANDLERS_0F3A_INDEX );
    return tables;
  }();
  return cached_tables;
}

// ============================================================================
// EVEX Handler Reader
// ============================================================================

// Helper to read TupleType from data stream
static uint8_t read_tuple_type( TableDeserializer& deserializer ) {
  return deserializer.read_u8();
}

void read_evex_handlers( TableDeserializer& deserializer, std::vector<HandlerEntry>& result ) {
  auto kind = static_cast<EvexOpCodeHandlerKind>( deserializer.read_u8() );

  switch ( kind ) {
    // ==========================================================================
    // Meta handlers
    // ==========================================================================

    case EvexOpCodeHandlerKind::INVALID:
      result.push_back( get_invalid_handler() );
      return;

    case EvexOpCodeHandlerKind::INVALID2:
      result.push_back( get_invalid_handler() );
      result.push_back( get_invalid_handler() );
      return;

    case EvexOpCodeHandlerKind::DUP: {
      auto count = deserializer.read_u32();
      auto handler = deserializer.read_handler();
      for ( uint32_t i = 0; i < count; ++i ) {
        result.push_back( handler );
      }
      return;
    }

    case EvexOpCodeHandlerKind::HANDLER_REFERENCE:
      result.push_back( deserializer.read_handler_reference() );
      return;

    case EvexOpCodeHandlerKind::ARRAY_REFERENCE:
      // Should not happen in handler context
      return;

    // ==========================================================================
    // Dispatch handlers
    // ==========================================================================

    case EvexOpCodeHandlerKind::RM: {
      auto handler_reg = deserializer.read_handler();
      auto handler_mem = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_EVEX_RM{ true, handler_reg, handler_mem } ) );
      return;
    }

    case EvexOpCodeHandlerKind::GROUP: {
      auto handlers = deserializer.read_array_reference(
        static_cast<uint32_t>( EvexOpCodeHandlerKind::ARRAY_REFERENCE )
      );
      auto* h = new OpCodeHandler_EVEX_Group{ true, {} };
      for ( std::size_t i = 0; i < 8 && i < handlers.size(); ++i ) {
        h->handlers[i] = handlers[i];
      }
      result.push_back( { OpCodeHandler_EVEX_Group::decode, reinterpret_cast<const OpCodeHandler*>( h ) } );
      return;
    }

    case EvexOpCodeHandlerKind::W: {
      auto handler_w0 = deserializer.read_handler();
      auto handler_w1 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_EVEX_W{ true, handler_w0, handler_w1 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::MANDATORY_PREFIX2: {
      auto h1 = deserializer.read_handler();
      auto h2 = deserializer.read_handler();
      auto h3 = deserializer.read_handler();
      auto h4 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_EVEX_MandatoryPrefix2{ true, { h1, h2, h3, h4 } } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VECTOR_LENGTH: {
      auto h128 = deserializer.read_handler();
      auto h256 = deserializer.read_handler();
      auto h512 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_EVEX_VectorLength{ true, h128, h256, h512 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VECTOR_LENGTH_ER: {
      auto h128 = deserializer.read_handler();
      auto h256 = deserializer.read_handler();
      auto h512 = deserializer.read_handler();
      result.push_back( make_handler( OpCodeHandler_EVEX_VectorLength_er{ true, h128, h256, h512 } ) );
      return;
    }

    // ==========================================================================
    // VkW variants (V=dest{k}, W=src)
    // ==========================================================================

    case EvexOpCodeHandlerKind::VK_W_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkW{ true, reg, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_W_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkW{ true, reg, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_W_4: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkW{ true, reg1, reg2, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_W_4B: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkW{ true, reg1, reg2, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_W_ER_4: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      auto only_sae = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_EVEX_VkW_er{ true, reg, reg, code, tt, only_sae, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_W_ER_5: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      auto only_sae = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_EVEX_VkW_er{ true, reg1, reg2, code, tt, only_sae, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_W_ER_6: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      auto only_sae = deserializer.read_boolean();
      auto can_broadcast = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_EVEX_VkW_er{ true, reg1, reg2, code, tt, only_sae, can_broadcast } ) );
      return;
    }

    // ==========================================================================
    // VkHW variants (V=dest{k}, H=vvvv, W=src)
    // ==========================================================================

    case EvexOpCodeHandlerKind::VK_HW_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHW{ true, reg, reg, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HW_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHW{ true, reg, reg, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HW_5: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto reg3 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHW{ true, reg1, reg2, reg3, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HW_ER_4: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      auto only_sae = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHW_er{ true, reg, code, tt, only_sae, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HW_ER_4B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      auto only_sae = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHW_er{ true, reg, code, tt, only_sae, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HW_ER_UR_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHW_er_ur{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HW_ER_UR_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHW_er_ur{ true, reg, code, tt, true } ) );
      return;
    }

    // ==========================================================================
    // VkWIb/VkHWIb variants
    // ==========================================================================

    case EvexOpCodeHandlerKind::VK_WIB_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkWIb{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_WIB_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkWIb{ true, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_WIB_ER: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkWIb_er{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HWIB_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHWIb{ true, reg, reg, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HWIB_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHWIb{ true, reg, reg, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HWIB_5: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto reg3 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHWIb{ true, reg1, reg2, reg3, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HWIB_ER_4: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHWIb_er{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HWIB_ER_4B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHWIb_er{ true, reg, code, tt, true } ) );
      return;
    }

    // ==========================================================================
    // Memory variants
    // ==========================================================================

    case EvexOpCodeHandlerKind::VK_M: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkM{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VM: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VM{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::MV: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_MV{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_HM: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VkHM{ true, reg, code, tt } ) );
      return;
    }

    // ==========================================================================
    // VW/VHW variants (no mask)
    // ==========================================================================

    case EvexOpCodeHandlerKind::VW: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VW{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VW_ER: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VW_er{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VHW_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VHW{ true, reg, code, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VHW_4: {
      auto reg = deserializer.read_register();
      auto code_r = deserializer.read_code();
      auto code_m = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VHW{ true, reg, code_r, code_m, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VHWIB: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VHWIb{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VHM: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VHM{ true, reg, code, tt } ) );
      return;
    }

    // ==========================================================================
    // WV/WkV variants (reversed operand order)
    // ==========================================================================

    case EvexOpCodeHandlerKind::WV: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_WV{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::WK_V_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_WkV{ true, reg, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::WK_V_4A: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_WkV{ true, reg1, reg2, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::WK_V_4B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      [[maybe_unused]] auto _ = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_EVEX_WkV{ true, reg, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::WK_VIB: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_WkVIb{ true, reg1, reg2, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::WK_VIB_ER: {
      auto reg1 = deserializer.read_register();
      auto reg2 = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_WkVIb_er{ true, reg1, reg2, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::WK_HV: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_EVEX_WkHV{ true, reg, code } ) );
      return;
    }

    // ==========================================================================
    // K-register handlers
    // ==========================================================================

    case EvexOpCodeHandlerKind::VK: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_EVEX_VK{ true, reg, code } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KR: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_EVEX_KR{ true, reg, code } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_EV_REXW_2: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_EVEX_VkEv_REXW{ true, reg, code, Code::INVALID } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_EV_REXW_3: {
      auto reg = deserializer.read_register();
      auto code32 = deserializer.read_code();
      auto code64 = deserializer.read_code();
      result.push_back( make_handler( OpCodeHandler_EVEX_VkEv_REXW{ true, reg, code32, code64 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_HW_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkHW{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_HW_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkHW{ true, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_HWIB_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkHWIb{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_HWIB_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkHWIb{ true, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_HWIB_SAE_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkHWIb_sae{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_HWIB_SAE_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkHWIb_sae{ true, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_WIB_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkWIb{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KK_WIB_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KkWIb{ true, reg, code, tt, true } ) );
      return;
    }

    case EvexOpCodeHandlerKind::KP1_HW: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_KP1HW{ true, reg, code, tt } ) );
      return;
    }

    // ==========================================================================
    // HW/HkW variants
    // ==========================================================================

    case EvexOpCodeHandlerKind::HWIB: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_HWIb{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::HK_WIB_3: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_HkWIb{ true, reg, code, tt, false } ) );
      return;
    }

    case EvexOpCodeHandlerKind::HK_WIB_3B: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_HkWIb{ true, reg, code, tt, true } ) );
      return;
    }

    // ==========================================================================
    // VSIB handlers
    // ==========================================================================

    case EvexOpCodeHandlerKind::VSIB_K1: {
      auto reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VSIB_k1{ true, reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VSIB_K1_VX: {
      auto vsib_reg = deserializer.read_register();
      auto base_reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VSIB_k1_VX{ true, vsib_reg, base_reg, code, tt } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VK_VSIB: {
      auto base_reg = deserializer.read_register();
      auto vsib_reg = deserializer.read_register();
      auto code = deserializer.read_code();
      auto tt = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_Vk_VSIB{ true, base_reg, vsib_reg, code, tt } ) );
      return;
    }

    // ==========================================================================
    // GPR/Ev handlers
    // ==========================================================================

    case EvexOpCodeHandlerKind::ED_V_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      auto tt1 = read_tuple_type( deserializer );
      auto tt2 = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_Ed_V_Ib{ true, reg, c1, c2, tt1, tt2 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::EV_VX: {
      auto [c1, c2] = deserializer.read_code2();
      auto tt1 = read_tuple_type( deserializer );
      auto tt2 = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_Ev_VX{ true, c1, c2, tt1, tt2 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::EV_VX_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      result.push_back( make_handler( OpCodeHandler_EVEX_Ev_VX_Ib{ true, reg, c1, c2 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::VX_EV: {
      auto [c1, c2] = deserializer.read_code2();
      auto tt1 = read_tuple_type( deserializer );
      auto tt2 = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_VX_Ev{ true, c1, c2, tt1, tt2 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::GV_W_ER: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      auto tt = read_tuple_type( deserializer );
      auto only_sae = deserializer.read_boolean();
      result.push_back( make_handler( OpCodeHandler_EVEX_Gv_W_er{ true, reg, c1, c2, tt, only_sae } ) );
      return;
    }

    case EvexOpCodeHandlerKind::GV_M_VX_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      auto tt1 = read_tuple_type( deserializer );
      auto tt2 = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_GvM_VX_Ib{ true, reg, c1, c2, tt1, tt2 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::V_H_EV_ER: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      auto tt1 = read_tuple_type( deserializer );
      auto tt2 = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_V_H_Ev_er{ true, reg, c1, c2, tt1, tt2 } ) );
      return;
    }

    case EvexOpCodeHandlerKind::V_H_EV_IB: {
      auto reg = deserializer.read_register();
      auto [c1, c2] = deserializer.read_code2();
      auto tt1 = read_tuple_type( deserializer );
      auto tt2 = read_tuple_type( deserializer );
      result.push_back( make_handler( OpCodeHandler_EVEX_V_H_Ev_Ib{ true, reg, c1, c2, tt1, tt2 } ) );
      return;
    }

    default:
      // Unknown handler kind - treat as invalid
      result.push_back( get_invalid_handler() );
      return;
  }
}

// ============================================================================
// EVEX Table Deserialization
// ============================================================================

std::vector<std::vector<HandlerEntry>> read_evex_tables() {
  // Static cache - tables are deserialized only once and reused
  static std::vector<std::vector<HandlerEntry>> cached_tables = []() {
    TableDeserializer deserializer(
      std::span<const uint8_t>( g_evex_tbl_data.data(), g_evex_tbl_data.size() ),
      EVEX_MAX_ID_NAMES,
      read_evex_handlers
    );
    deserializer.deserialize();

    // Return 5 tables: 0F, 0F38, 0F3A, MAP5, MAP6
    std::vector<std::vector<HandlerEntry>> tables( 5 );
    tables[0] = deserializer.table( EVEX_HANDLERS_0F_INDEX );
    tables[1] = deserializer.table( EVEX_HANDLERS_0F38_INDEX );
    tables[2] = deserializer.table( EVEX_HANDLERS_0F3A_INDEX );
    tables[3] = deserializer.table( EVEX_HANDLERS_MAP5_INDEX );
    tables[4] = deserializer.table( EVEX_HANDLERS_MAP6_INDEX );
    return tables;
  }();
  return cached_tables;
}

} // namespace internal
} // namespace iced_x86
