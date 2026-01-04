// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_CODE_LABEL_HPP
#define ICED_X86_CODE_LABEL_HPP

#include <cstdint>
#include <cstddef>
#include <functional>

namespace iced_x86 {

/// @brief A label created by CodeAssembler that can be used as a branch target
///
/// Labels allow you to create forward and backward branches in assembled code.
/// Create labels using CodeAssembler::create_label() and set their position
/// using CodeAssembler::set_label().
///
/// @example
/// @code
/// CodeAssembler a(64);
/// auto loop_label = a.create_label();
/// a.xor_(eax, eax);
/// a.set_label(loop_label);
/// a.inc(eax);
/// a.cmp(eax, 10);
/// a.jl(loop_label);
/// @endcode
class CodeLabel {
public:
  /// @brief Creates an empty/invalid label
  constexpr CodeLabel() noexcept = default;

  /// @brief Creates a label with the specified ID (used internally by CodeAssembler)
  /// @param id Label ID
  explicit constexpr CodeLabel( uint64_t id ) noexcept
    : id_( id ) {}

  /// @brief Gets the label ID
  /// @return Label ID
  [[nodiscard]] constexpr uint64_t id() const noexcept { return id_; }

  /// @brief Checks if this label is empty (not created by CodeAssembler)
  /// @return true if empty
  [[nodiscard]] constexpr bool is_empty() const noexcept { return id_ == 0; }

  /// @brief Checks if this label has been assigned to an instruction
  /// @return true if assigned
  [[nodiscard]] constexpr bool has_instruction_index() const noexcept {
    return instruction_index_ != INVALID_INDEX;
  }

  /// @brief Gets the instruction index this label is assigned to
  /// @return Instruction index, or INVALID_INDEX if not assigned
  [[nodiscard]] constexpr size_t instruction_index() const noexcept {
    return instruction_index_;
  }

  /// @brief Equality comparison
  [[nodiscard]] constexpr bool operator==( const CodeLabel& other ) const noexcept {
    return id_ == other.id_;
  }

  /// @brief Inequality comparison
  [[nodiscard]] constexpr bool operator!=( const CodeLabel& other ) const noexcept {
    return id_ != other.id_;
  }

  /// @brief Invalid instruction index value
  static constexpr size_t INVALID_INDEX = static_cast<size_t>( -1 );

private:
  friend class CodeAssembler;

  /// @brief Sets the instruction index (called by CodeAssembler)
  constexpr void set_instruction_index( size_t index ) noexcept {
    instruction_index_ = index;
  }

  uint64_t id_ = 0;
  size_t instruction_index_ = INVALID_INDEX;
};

} // namespace iced_x86

// Hash support for CodeLabel
template<>
struct std::hash<iced_x86::CodeLabel> {
  [[nodiscard]] size_t operator()( const iced_x86::CodeLabel& label ) const noexcept {
    return std::hash<uint64_t>{}( label.id() );
  }
};

#endif // ICED_X86_CODE_LABEL_HPP
