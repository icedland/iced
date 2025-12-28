// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_INTERNAL_TABLE_DESERIALIZER_HPP
#define ICED_X86_INTERNAL_TABLE_DESERIALIZER_HPP

#include <cstdint>
#include <cstddef>
#include <span>
#include <vector>

#include "handlers.hpp"
#include "serialized_data_kind.hpp"
#include "legacy_op_code_handler_kind.hpp"
#include "compiler_intrinsics.hpp"
#include "../code.hpp"
#include "../register.hpp"

namespace iced_x86 {
namespace internal {

/// @brief Data reader for binary table data
class DataReader {
public:
  explicit DataReader( std::span<const uint8_t> data ) noexcept
    : data_( data ), pos_( 0 ) {}

  [[nodiscard]] bool can_read() const noexcept {
    return pos_ < data_.size();
  }
  
  [[nodiscard]] std::size_t position() const noexcept {
    return pos_;
  }

  [[nodiscard]] uint8_t read_u8() noexcept {
    if ( pos_ >= data_.size() ) return 0;
    return data_[pos_++];
  }

  [[nodiscard]] uint32_t read_compressed_u32() noexcept {
    uint32_t result = 0;
    uint32_t shift = 0;
    while ( true ) {
      uint8_t b = read_u8();
      result |= static_cast<uint32_t>( b & 0x7F ) << shift;
      if ( ( b & 0x80 ) == 0 )
        break;
      shift += 7;
    }
    return result;
  }

private:
  std::span<const uint8_t> data_;
  std::size_t pos_;
};

/// @brief Handler info - either single handler or array of handlers (RTTI-free, pointer-based)
struct HandlerInfo {
  // Uses pointer + size to distinguish:
  // - single handler: array_ptr == nullptr, single contains the handler
  // - array: array_ptr != nullptr, points to heap-allocated vector
  HandlerEntry single{};
  std::vector<HandlerEntry>* array_ptr = nullptr;
  
  // Default constructor - creates empty single handler
  HandlerInfo() noexcept = default;
  
  // Constructor from single handler
  HandlerInfo( HandlerEntry entry ) noexcept : single( entry ), array_ptr( nullptr ) {}
  
  // Constructor from array (move) - takes ownership via heap allocation
  HandlerInfo( std::vector<HandlerEntry>&& arr ) noexcept 
    : single{}, array_ptr( new std::vector<HandlerEntry>( std::move( arr ) ) ) {}
  
  // Destructor
  ~HandlerInfo() {
    delete array_ptr;
  }
  
  // Move constructor
  HandlerInfo( HandlerInfo&& other ) noexcept 
    : single( other.single ), array_ptr( other.array_ptr ) {
    other.array_ptr = nullptr;
  }
  
  // Move assignment
  HandlerInfo& operator=( HandlerInfo&& other ) noexcept {
    if ( this != &other ) {
      delete array_ptr;
      single = other.single;
      array_ptr = other.array_ptr;
      other.array_ptr = nullptr;
    }
    return *this;
  }
  
  // Delete copy operations
  HandlerInfo( const HandlerInfo& ) = delete;
  HandlerInfo& operator=( const HandlerInfo& ) = delete;
  
  // Accessors
  [[nodiscard]] bool is_single() const noexcept { return array_ptr == nullptr; }
  [[nodiscard]] bool is_array() const noexcept { return array_ptr != nullptr; }
  
  [[nodiscard]] HandlerEntry* get_single() noexcept {
    return array_ptr == nullptr ? &single : nullptr;
  }
  [[nodiscard]] const HandlerEntry* get_single() const noexcept {
    return array_ptr == nullptr ? &single : nullptr;
  }
  
  [[nodiscard]] std::vector<HandlerEntry>* get_array() noexcept {
    return array_ptr;
  }
  [[nodiscard]] const std::vector<HandlerEntry>* get_array() const noexcept {
    return array_ptr;
  }
};

/// @brief Handler reader function type - using raw function pointer for performance
/// std::function has significant overhead: heap allocation, type erasure, cannot be inlined
using HandlerReaderFn = void (*)( class TableDeserializer&, std::vector<HandlerEntry>& );

/// @brief Table deserializer - reads binary table data and builds handler tree
class TableDeserializer {
public:
  TableDeserializer( 
    std::span<const uint8_t> data, 
    std::size_t max_ids,
    HandlerReaderFn handler_reader 
  ) noexcept;

  /// @brief Deserialize all handlers from the data
  void deserialize();

  /// @brief Get a table by index (moves it out)
  [[nodiscard]] std::vector<HandlerEntry> table( std::size_t index );

  // Reader methods for handler readers
  [[nodiscard]] LegacyOpCodeHandlerKind read_legacy_op_code_handler_kind() noexcept;
  [[nodiscard]] uint8_t read_u8() noexcept { return reader_.read_u8(); }
  [[nodiscard]] Code read_code() noexcept;
  [[nodiscard]] std::pair<Code, Code> read_code2() noexcept;
  [[nodiscard]] std::tuple<Code, Code, Code> read_code3() noexcept;
  [[nodiscard]] Register read_register() noexcept;
  [[nodiscard]] uint32_t read_decoder_options() noexcept;
  [[nodiscard]] uint32_t read_handler_flags() noexcept;
  [[nodiscard]] uint32_t read_legacy_handler_flags() noexcept;
  [[nodiscard]] bool read_boolean() noexcept;
  [[nodiscard]] uint32_t read_u32() noexcept;

  [[nodiscard]] HandlerEntry read_handler();
  [[nodiscard]] HandlerEntry read_handler_or_null_instance();
  [[nodiscard]] std::vector<HandlerEntry> read_handlers( std::size_t count );
  [[nodiscard]] HandlerEntry read_handler_reference();
  [[nodiscard]] std::vector<HandlerEntry> read_array_reference( uint32_t kind );
  [[nodiscard]] std::vector<HandlerEntry> read_array_reference_no_clone( uint32_t kind );

private:
  DataReader reader_;
  HandlerReaderFn handler_reader_;
  std::vector<HandlerInfo> id_to_handler_;
  std::vector<std::vector<HandlerEntry>> temp_vecs_;
};

/// @brief Read legacy handler tables
[[nodiscard]] std::vector<HandlerEntry> read_legacy_tables();

/// @brief Read VEX handler tables (returns 3 tables: 0F, 0F38, 0F3A)
[[nodiscard]] std::vector<std::vector<HandlerEntry>> read_vex_tables();

/// @brief Read EVEX handler tables (returns 5 tables: 0F, 0F38, 0F3A, MAP5, MAP6)
[[nodiscard]] std::vector<std::vector<HandlerEntry>> read_evex_tables();

} // namespace internal
} // namespace iced_x86

#endif // ICED_X86_INTERNAL_TABLE_DESERIALIZER_HPP
