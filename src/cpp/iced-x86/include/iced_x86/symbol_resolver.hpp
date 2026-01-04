// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_SYMBOL_RESOLVER_HPP
#define ICED_X86_SYMBOL_RESOLVER_HPP

#include "formatter_text_kind.hpp"
#include "memory_size.hpp"
#include <cstdint>
#include <string>
#include <string_view>
#include <vector>
#include <optional>

namespace iced_x86 {

// Forward declaration
struct Instruction;

/// @brief Symbol flags returned by a symbol resolver
enum class SymbolFlags : uint32_t {
  /// @brief No flags set
  NONE = 0,
  /// @brief It's a symbol relative to a register, e.g., a struct offset `[ebx+some_struct.field1]`.
  /// If this is cleared, it's the address of a symbol.
  RELATIVE = 1 << 0,
  /// @brief It's a signed symbol and should be displayed as `-symbol` or `reg-symbol`
  /// instead of `symbol` or `reg+symbol`
  SIGNED = 1 << 1,
  /// @brief Set if symbol_size is valid
  HAS_SYMBOL_SIZE = 1 << 2,
};

/// @brief Bitwise OR for SymbolFlags
[[nodiscard]] inline constexpr SymbolFlags operator|( SymbolFlags a, SymbolFlags b ) noexcept {
  return static_cast<SymbolFlags>( static_cast<uint32_t>( a ) | static_cast<uint32_t>( b ) );
}

/// @brief Bitwise AND for SymbolFlags
[[nodiscard]] inline constexpr SymbolFlags operator&( SymbolFlags a, SymbolFlags b ) noexcept {
  return static_cast<SymbolFlags>( static_cast<uint32_t>( a ) & static_cast<uint32_t>( b ) );
}

/// @brief Bitwise NOT for SymbolFlags
[[nodiscard]] inline constexpr SymbolFlags operator~( SymbolFlags a ) noexcept {
  return static_cast<SymbolFlags>( ~static_cast<uint32_t>( a ) );
}

/// @brief Check if flag is set
[[nodiscard]] inline constexpr bool has_flag( SymbolFlags flags, SymbolFlags flag ) noexcept {
  return ( static_cast<uint32_t>( flags ) & static_cast<uint32_t>( flag ) ) != 0;
}

/// @brief A text part with associated formatting kind
struct TextPart {
  /// @brief The text
  std::string text;
  /// @brief The text kind (color/formatting)
  FormatterTextKind kind = FormatterTextKind::TEXT;

  /// @brief Default constructor
  TextPart() = default;

  /// @brief Constructor with text and kind
  /// @param t Text string
  /// @param k Text kind
  TextPart( std::string t, FormatterTextKind k = FormatterTextKind::TEXT )
      : text( std::move( t ) ), kind( k ) {}

  /// @brief Constructor with string_view and kind
  /// @param t Text string view
  /// @param k Text kind
  TextPart( std::string_view t, FormatterTextKind k = FormatterTextKind::TEXT )
      : text( t ), kind( k ) {}

  /// @brief Constructor with C string and kind
  /// @param t Text C string
  /// @param k Text kind
  TextPart( const char* t, FormatterTextKind k = FormatterTextKind::TEXT )
      : text( t ), kind( k ) {}
};

/// @brief Contains text information for a symbol (single part or multiple parts)
struct TextInfo {
  /// @brief Single text part (used when parts is empty)
  TextPart text;
  /// @brief Multiple text parts (if not empty, this takes precedence over text)
  std::vector<TextPart> parts;

  /// @brief Default constructor
  TextInfo() = default;

  /// @brief Constructor with single text
  /// @param t Text string
  /// @param kind Text kind
  TextInfo( std::string t, FormatterTextKind kind = FormatterTextKind::LABEL )
      : text( std::move( t ), kind ) {}

  /// @brief Constructor with string_view
  /// @param t Text string view
  /// @param kind Text kind
  TextInfo( std::string_view t, FormatterTextKind kind = FormatterTextKind::LABEL )
      : text( std::string( t ), kind ) {}

  /// @brief Constructor with C string
  /// @param t Text C string
  /// @param kind Text kind
  TextInfo( const char* t, FormatterTextKind kind = FormatterTextKind::LABEL )
      : text( t, kind ) {}

  /// @brief Constructor with TextPart
  /// @param part Text part
  explicit TextInfo( TextPart part ) : text( std::move( part ) ) {}

  /// @brief Constructor with multiple parts
  /// @param p Vector of text parts
  explicit TextInfo( std::vector<TextPart> p ) : parts( std::move( p ) ) {}

  /// @brief Check if this is the default/empty instance
  [[nodiscard]] bool is_default() const noexcept {
    return parts.empty() && text.text.empty();
  }

  /// @brief Check if this uses multiple parts
  [[nodiscard]] bool has_parts() const noexcept { return !parts.empty(); }
};

/// @brief Symbol result returned by a symbol resolver
struct SymbolResult {
  /// @brief The address of the symbol
  uint64_t address = 0;
  /// @brief Contains the symbol text
  TextInfo text;
  /// @brief Symbol flags
  SymbolFlags flags = SymbolFlags::NONE;
  /// @brief Symbol size (only valid if HAS_SYMBOL_SIZE flag is set)
  MemorySize symbol_size = MemorySize::UNKNOWN;

  /// @brief Default constructor
  SymbolResult() = default;

  /// @brief Constructor with address and text
  /// @param addr Symbol address
  /// @param txt Symbol text
  SymbolResult( uint64_t addr, std::string txt )
      : address( addr ), text( std::move( txt ) ) {}

  /// @brief Constructor with address, text, and size
  /// @param addr Symbol address
  /// @param txt Symbol text
  /// @param size Symbol size
  SymbolResult( uint64_t addr, std::string txt, MemorySize size )
      : address( addr )
      , text( std::move( txt ) )
      , flags( SymbolFlags::HAS_SYMBOL_SIZE )
      , symbol_size( size ) {}

  /// @brief Constructor with address, text, and kind
  /// @param addr Symbol address
  /// @param txt Symbol text
  /// @param kind Text kind
  SymbolResult( uint64_t addr, std::string txt, FormatterTextKind kind )
      : address( addr ), text( std::move( txt ), kind ) {}

  /// @brief Constructor with address, text, kind, and flags
  /// @param addr Symbol address
  /// @param txt Symbol text
  /// @param kind Text kind
  /// @param f Symbol flags
  SymbolResult( uint64_t addr, std::string txt, FormatterTextKind kind, SymbolFlags f )
      : address( addr )
      , text( std::move( txt ), kind )
      , flags( f & ~SymbolFlags::HAS_SYMBOL_SIZE ) {}

  /// @brief Constructor with address and TextInfo
  /// @param addr Symbol address
  /// @param info Text info
  SymbolResult( uint64_t addr, TextInfo info )
      : address( addr ), text( std::move( info ) ) {}

  /// @brief Constructor with address, TextInfo, and size
  /// @param addr Symbol address
  /// @param info Text info
  /// @param size Symbol size
  SymbolResult( uint64_t addr, TextInfo info, MemorySize size )
      : address( addr )
      , text( std::move( info ) )
      , flags( SymbolFlags::HAS_SYMBOL_SIZE )
      , symbol_size( size ) {}

  /// @brief Constructor with address, TextInfo, and flags
  /// @param addr Symbol address
  /// @param info Text info
  /// @param f Symbol flags
  SymbolResult( uint64_t addr, TextInfo info, SymbolFlags f )
      : address( addr )
      , text( std::move( info ) )
      , flags( f & ~SymbolFlags::HAS_SYMBOL_SIZE ) {}

  /// @brief Constructor with all fields
  /// @param addr Symbol address
  /// @param info Text info
  /// @param f Symbol flags
  /// @param size Symbol size
  SymbolResult( uint64_t addr, TextInfo info, SymbolFlags f, MemorySize size )
      : address( addr )
      , text( std::move( info ) )
      , flags( f | SymbolFlags::HAS_SYMBOL_SIZE )
      , symbol_size( size ) {}

  /// @brief Check if symbol size is valid
  [[nodiscard]] bool has_symbol_size() const noexcept {
    return has_flag( flags, SymbolFlags::HAS_SYMBOL_SIZE );
  }

  /// @brief Check if this is a relative symbol
  [[nodiscard]] bool is_relative() const noexcept {
    return has_flag( flags, SymbolFlags::RELATIVE );
  }

  /// @brief Check if this is a signed symbol
  [[nodiscard]] bool is_signed() const noexcept {
    return has_flag( flags, SymbolFlags::SIGNED );
  }
};

/// @brief Interface used by formatters to resolve symbols
///
/// Implement this interface to provide custom symbol resolution for addresses.
/// This allows formatters to display symbol names instead of raw addresses.
///
/// Example:
/// @code
/// class MySymbolResolver : public SymbolResolver {
/// public:
///   std::optional<SymbolResult> try_get_symbol(
///       const Instruction& instruction,
///       int operand,
///       int instruction_operand,
///       uint64_t address,
///       int address_size) override {
///     // Look up symbol in your symbol table
///     auto it = symbols.find(address);
///     if (it != symbols.end()) {
///       return SymbolResult(address, it->second);
///     }
///     return std::nullopt;
///   }
/// private:
///   std::unordered_map<uint64_t, std::string> symbols;
/// };
/// @endcode
class SymbolResolver {
public:
  virtual ~SymbolResolver() = default;

  /// @brief Tries to resolve a symbol
  ///
  /// @param instruction The instruction being formatted
  /// @param operand Operand number (0-based). This is a formatter operand and isn't
  ///                necessarily the same as an instruction operand.
  /// @param instruction_operand Instruction operand number (0-based), or -1 if it's
  ///                            an operand created by the formatter.
  /// @param address The address to resolve
  /// @param address_size Size of the address in bytes (1, 2, 4, or 8)
  /// @return The symbol result if found, or std::nullopt if no symbol exists
  [[nodiscard]] virtual std::optional<SymbolResult> try_get_symbol(
      const Instruction& instruction,
      int operand,
      int instruction_operand,
      uint64_t address,
      int address_size ) = 0;
};

/// @brief A simple symbol resolver using a callback function
///
/// This is a convenience class for users who prefer lambdas or function pointers
/// over inheritance.
///
/// Example:
/// @code
/// FunctionSymbolResolver resolver([](uint64_t addr) -> std::optional<SymbolResult> {
///   if (addr == 0x1000) return SymbolResult(addr, "main");
///   if (addr == 0x2000) return SymbolResult(addr, "foo");
///   return std::nullopt;
/// });
/// @endcode
class FunctionSymbolResolver : public SymbolResolver {
public:
  /// @brief Callback function type (simple version - just takes address)
  using SimpleCallback = std::optional<SymbolResult> ( * )( uint64_t address );

  /// @brief Callback function type (full version)
  using FullCallback = std::optional<SymbolResult> ( * )(
      const Instruction& instruction,
      int operand,
      int instruction_operand,
      uint64_t address,
      int address_size );

  /// @brief Constructor with simple callback
  /// @param callback Callback function that takes just the address
  explicit FunctionSymbolResolver( SimpleCallback callback ) : simple_callback_( callback ) {}

  /// @brief Constructor with full callback
  /// @param callback Callback function with full signature
  explicit FunctionSymbolResolver( FullCallback callback ) : full_callback_( callback ) {}

  [[nodiscard]] std::optional<SymbolResult> try_get_symbol(
      const Instruction& instruction,
      int operand,
      int instruction_operand,
      uint64_t address,
      int address_size ) override {
    if ( full_callback_ ) {
      return full_callback_( instruction, operand, instruction_operand, address, address_size );
    }
    if ( simple_callback_ ) {
      return simple_callback_( address );
    }
    return std::nullopt;
  }

private:
  SimpleCallback simple_callback_ = nullptr;
  FullCallback full_callback_ = nullptr;
};

} // namespace iced_x86

#endif // ICED_X86_SYMBOL_RESOLVER_HPP
