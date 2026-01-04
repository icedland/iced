// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_FAST_STRING_OUTPUT_HPP
#define ICED_X86_FAST_STRING_OUTPUT_HPP

#include <string>
#include <string_view>
#include <cstdint>
#include <cstring>

namespace iced_x86 {

/// @brief Fast string output for the fast formatter
///
/// This class is optimized for appending strings quickly using a fixed-size
/// inline buffer to avoid heap allocations for typical instruction strings.
class FastStringOutput {
public:
  static constexpr std::size_t INLINE_CAPACITY = 128;

  /// @brief Creates an empty output
  FastStringOutput() noexcept : size_( 0 ) {}

  /// @brief Creates output with reserved capacity (ignored for inline buffer)
  /// @param capacity Initial capacity (ignored if <= INLINE_CAPACITY)
  explicit FastStringOutput( [[maybe_unused]] std::size_t capacity ) noexcept : size_( 0 ) {}

  /// @brief Appends a string view
  /// @param text Text to append
  void append( std::string_view text ) noexcept {
    auto len = text.size();
    if ( size_ + len <= INLINE_CAPACITY ) [[likely]] {
      std::memcpy( buffer_ + size_, text.data(), len );
      size_ += len;
    } else {
      append_slow( text );
    }
  }

  /// @brief Appends a single character
  /// @param c Character to append
  void append( char c ) noexcept {
    if ( size_ < INLINE_CAPACITY ) [[likely]] {
      buffer_[size_++] = c;
    } else {
      char buf[2] = { c, '\0' };
      append_slow( std::string_view( buf, 1 ) );
    }
  }

  /// @brief Appends a C string (must not be null)
  /// @param text Text to append (must not be null)
  void append_not_null( const char* text ) noexcept {
    append( std::string_view( text ) );
  }

  /// @brief Clears the output
  void clear() noexcept { 
    size_ = 0;
    overflow_.clear();
  }

  /// @brief Gets the current length
  /// @return Length in characters
  [[nodiscard]] std::size_t size() const noexcept { 
    return overflow_.empty() ? size_ : overflow_.size();
  }

  /// @brief Checks if output is empty
  /// @return true if empty
  [[nodiscard]] bool empty() const noexcept { 
    return size_ == 0 && overflow_.empty();
  }

  /// @brief Gets the output as a string view
  /// @return String view of the output
  [[nodiscard]] std::string_view view() const noexcept {
    if ( overflow_.empty() ) [[likely]] {
      return std::string_view( buffer_, size_ );
    }
    return overflow_;
  }

  /// @brief Gets the output as a string reference
  /// @return Reference to the underlying string
  [[nodiscard]] std::string str() const {
    if ( overflow_.empty() ) [[likely]] {
      return std::string( buffer_, size_ );
    }
    return overflow_;
  }

  /// @brief Gets the output as a C string
  /// @return C string pointer
  [[nodiscard]] const char* c_str() noexcept {
    if ( overflow_.empty() ) [[likely]] {
      buffer_[size_] = '\0';
      return buffer_;
    }
    return overflow_.c_str();
  }

  /// @brief Reserves capacity (no-op for inline buffer)
  /// @param capacity Capacity to reserve
  void reserve( [[maybe_unused]] std::size_t capacity ) noexcept {
    // No-op for inline buffer - we don't pre-allocate overflow
  }

  /// @brief Gets the current capacity
  /// @return Current capacity
  [[nodiscard]] std::size_t capacity() const noexcept {
    return overflow_.empty() ? INLINE_CAPACITY : overflow_.capacity();
  }

private:
  void append_slow( std::string_view text ) {
    if ( overflow_.empty() ) {
      // First overflow - move inline buffer to overflow string
      overflow_.reserve( INLINE_CAPACITY * 2 );
      overflow_.append( buffer_, size_ );
    }
    overflow_.append( text );
  }

  char buffer_[INLINE_CAPACITY + 1];  // +1 for null terminator in c_str()
  std::size_t size_;
  std::string overflow_;  // Used only when buffer overflows
};

} // namespace iced_x86

#endif // ICED_X86_FAST_STRING_OUTPUT_HPP
