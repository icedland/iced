// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_FAST_STRING_OUTPUT_HPP
#define ICED_X86_FAST_STRING_OUTPUT_HPP

#include <string>
#include <string_view>
#include <cstdint>

namespace iced_x86 {

/// @brief Fast string output for the fast formatter
///
/// This class is optimized for appending strings quickly. It's similar
/// to std::string but with a simpler interface focused on append operations.
class FastStringOutput {
public:
  /// @brief Creates an empty output
  FastStringOutput() = default;

  /// @brief Creates output with reserved capacity
  /// @param capacity Initial capacity to reserve
  explicit FastStringOutput( std::size_t capacity ) { buffer_.reserve( capacity ); }

  /// @brief Appends a string view
  /// @param text Text to append
  void append( std::string_view text ) { buffer_.append( text ); }

  /// @brief Appends a single character
  /// @param c Character to append
  void append( char c ) { buffer_.push_back( c ); }

  /// @brief Appends a C string (must not be null)
  /// @param text Text to append (must not be null)
  void append_not_null( const char* text ) { buffer_.append( text ); }

  /// @brief Clears the output
  void clear() noexcept { buffer_.clear(); }

  /// @brief Gets the current length
  /// @return Length in characters
  [[nodiscard]] std::size_t size() const noexcept { return buffer_.size(); }

  /// @brief Checks if output is empty
  /// @return true if empty
  [[nodiscard]] bool empty() const noexcept { return buffer_.empty(); }

  /// @brief Gets the output as a string view
  /// @return String view of the output
  [[nodiscard]] std::string_view view() const noexcept { return buffer_; }

  /// @brief Gets the output as a string reference
  /// @return Reference to the underlying string
  [[nodiscard]] const std::string& str() const noexcept { return buffer_; }

  /// @brief Gets the output as a C string
  /// @return C string pointer
  [[nodiscard]] const char* c_str() const noexcept { return buffer_.c_str(); }

  /// @brief Reserves capacity
  /// @param capacity Capacity to reserve
  void reserve( std::size_t capacity ) { buffer_.reserve( capacity ); }

  /// @brief Gets the current capacity
  /// @return Current capacity
  [[nodiscard]] std::size_t capacity() const noexcept { return buffer_.capacity(); }

private:
  std::string buffer_;
};

} // namespace iced_x86

#endif // ICED_X86_FAST_STRING_OUTPUT_HPP
