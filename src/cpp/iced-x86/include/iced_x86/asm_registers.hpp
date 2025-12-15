// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_ASM_REGISTERS_HPP
#define ICED_X86_ASM_REGISTERS_HPP

#include "register.hpp"
#include "asm_memory_operand.hpp"
#include <cstdint>

namespace iced_x86 {

// ============================================================================
// AsmRegister8 - 8-bit general purpose registers (AL-R15L)
// ============================================================================

/// @brief An 8-bit assembler register (AL-R15L)
struct AsmRegister8 {
  Register value = Register::NONE;

  constexpr AsmRegister8() noexcept = default;
  explicit constexpr AsmRegister8( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegister8& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegister8& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegister16 - 16-bit general purpose registers (AX-R15W)
// ============================================================================

/// @brief A 16-bit assembler register (AX-R15W)
struct AsmRegister16 {
  Register value = Register::NONE;

  constexpr AsmRegister16() noexcept = default;
  explicit constexpr AsmRegister16( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  // Operator overloads for memory operand creation

  /// @brief reg + reg -> memory operand with base and index
  [[nodiscard]] constexpr AsmMemoryOperand operator+( const AsmRegister16& other ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, other.value, 1, 0 );
  }

  /// @brief reg + displacement -> memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator+( int64_t displacement ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, Register::NONE, 1, displacement );
  }

  /// @brief reg - displacement -> memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator-( int64_t displacement ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, Register::NONE, 1, -displacement );
  }

  /// @brief reg * scale -> memory operand (index with scale)
  [[nodiscard]] constexpr AsmMemoryOperand operator*( int32_t scale ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, Register::NONE, value, scale, 0 );
  }

  [[nodiscard]] constexpr bool operator==( const AsmRegister16& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegister16& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegister32 - 32-bit general purpose registers (EAX-R15D)
// ============================================================================

/// @brief A 32-bit assembler register (EAX-R15D)
struct AsmRegister32 {
  Register value = Register::NONE;
  uint32_t flags = AsmOperandFlags::NONE;

  constexpr AsmRegister32() noexcept = default;
  explicit constexpr AsmRegister32( Register reg, uint32_t flags_ = AsmOperandFlags::NONE ) noexcept
    : value( reg ), flags( flags_ ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  // Operator overloads for memory operand creation

  /// @brief reg + reg -> memory operand with base and index
  [[nodiscard]] constexpr AsmMemoryOperand operator+( const AsmRegister32& other ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, other.value, 1, 0 );
  }

  /// @brief reg + memory -> add base to existing memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator+( const AsmMemoryOperand& mem ) const noexcept {
    bool has_base = mem.base != Register::NONE;
    return AsmMemoryOperand( mem.size, mem.segment,
                             has_base ? mem.base : value,
                             has_base ? value : mem.index,
                             mem.scale, mem.displacement, mem.flags );
  }

  /// @brief reg + displacement -> memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator+( int64_t displacement ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, Register::NONE, 1, displacement );
  }

  /// @brief reg - displacement -> memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator-( int64_t displacement ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, Register::NONE, 1, -displacement );
  }

  /// @brief reg * scale -> memory operand (index with scale)
  [[nodiscard]] constexpr AsmMemoryOperand operator*( int32_t scale ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, Register::NONE, value, scale, 0 );
  }

  // Mask register methods (for AVX-512)
  [[nodiscard]] constexpr AsmRegister32 k1() const noexcept {
    return AsmRegister32( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K1 );
  }
  [[nodiscard]] constexpr AsmRegister32 k2() const noexcept {
    return AsmRegister32( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K2 );
  }
  [[nodiscard]] constexpr AsmRegister32 k3() const noexcept {
    return AsmRegister32( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K3 );
  }
  [[nodiscard]] constexpr AsmRegister32 k4() const noexcept {
    return AsmRegister32( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K4 );
  }
  [[nodiscard]] constexpr AsmRegister32 k5() const noexcept {
    return AsmRegister32( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K5 );
  }
  [[nodiscard]] constexpr AsmRegister32 k6() const noexcept {
    return AsmRegister32( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K6 );
  }
  [[nodiscard]] constexpr AsmRegister32 k7() const noexcept {
    return AsmRegister32( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K7 );
  }

  [[nodiscard]] constexpr bool operator==( const AsmRegister32& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegister32& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegister64 - 64-bit general purpose registers (RAX-R15)
// ============================================================================

/// @brief A 64-bit assembler register (RAX-R15)
struct AsmRegister64 {
  Register value = Register::NONE;
  uint32_t flags = AsmOperandFlags::NONE;

  constexpr AsmRegister64() noexcept = default;
  explicit constexpr AsmRegister64( Register reg, uint32_t flags_ = AsmOperandFlags::NONE ) noexcept
    : value( reg ), flags( flags_ ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  // Operator overloads for memory operand creation

  /// @brief reg + reg -> memory operand with base and index
  [[nodiscard]] constexpr AsmMemoryOperand operator+( const AsmRegister64& other ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, other.value, 1, 0 );
  }

  /// @brief reg + memory -> add base to existing memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator+( const AsmMemoryOperand& mem ) const noexcept {
    bool has_base = mem.base != Register::NONE;
    return AsmMemoryOperand( mem.size, mem.segment,
                             has_base ? mem.base : value,
                             has_base ? value : mem.index,
                             mem.scale, mem.displacement, mem.flags );
  }

  /// @brief reg + displacement -> memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator+( int64_t displacement ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, Register::NONE, 1, displacement );
  }

  /// @brief reg - displacement -> memory operand
  [[nodiscard]] constexpr AsmMemoryOperand operator-( int64_t displacement ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, value, Register::NONE, 1, -displacement );
  }

  /// @brief reg * scale -> memory operand (index with scale)
  [[nodiscard]] constexpr AsmMemoryOperand operator*( int32_t scale ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, Register::NONE, value, scale, 0 );
  }

  // Mask register methods (for AVX-512)
  [[nodiscard]] constexpr AsmRegister64 k1() const noexcept {
    return AsmRegister64( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K1 );
  }
  [[nodiscard]] constexpr AsmRegister64 k2() const noexcept {
    return AsmRegister64( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K2 );
  }
  [[nodiscard]] constexpr AsmRegister64 k3() const noexcept {
    return AsmRegister64( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K3 );
  }
  [[nodiscard]] constexpr AsmRegister64 k4() const noexcept {
    return AsmRegister64( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K4 );
  }
  [[nodiscard]] constexpr AsmRegister64 k5() const noexcept {
    return AsmRegister64( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K5 );
  }
  [[nodiscard]] constexpr AsmRegister64 k6() const noexcept {
    return AsmRegister64( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K6 );
  }
  [[nodiscard]] constexpr AsmRegister64 k7() const noexcept {
    return AsmRegister64( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K7 );
  }

  [[nodiscard]] constexpr bool operator==( const AsmRegister64& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegister64& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterXmm - XMM registers (XMM0-XMM31)
// ============================================================================

/// @brief An XMM assembler register (XMM0-XMM31)
struct AsmRegisterXmm {
  Register value = Register::NONE;
  uint32_t flags = AsmOperandFlags::NONE;

  constexpr AsmRegisterXmm() noexcept = default;
  explicit constexpr AsmRegisterXmm( Register reg, uint32_t flags_ = AsmOperandFlags::NONE ) noexcept
    : value( reg ), flags( flags_ ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  // VSIB addressing support
  [[nodiscard]] constexpr AsmMemoryOperand operator*( int32_t scale ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, Register::NONE, value, scale, 0 );
  }

  // Mask and zeroing methods
  [[nodiscard]] constexpr AsmRegisterXmm k1() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K1 );
  }
  [[nodiscard]] constexpr AsmRegisterXmm k2() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K2 );
  }
  [[nodiscard]] constexpr AsmRegisterXmm k3() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K3 );
  }
  [[nodiscard]] constexpr AsmRegisterXmm k4() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K4 );
  }
  [[nodiscard]] constexpr AsmRegisterXmm k5() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K5 );
  }
  [[nodiscard]] constexpr AsmRegisterXmm k6() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K6 );
  }
  [[nodiscard]] constexpr AsmRegisterXmm k7() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K7 );
  }
  [[nodiscard]] constexpr AsmRegisterXmm z() const noexcept {
    return AsmRegisterXmm( value, flags | AsmOperandFlags::ZEROING );
  }
  [[nodiscard]] constexpr AsmRegisterXmm sae() const noexcept {
    return AsmRegisterXmm( value, flags | AsmOperandFlags::SUPPRESS_ALL_EXCEPTIONS );
  }
  [[nodiscard]] constexpr AsmRegisterXmm rn_sae() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RN_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterXmm rd_sae() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RD_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterXmm ru_sae() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RU_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterXmm rz_sae() const noexcept {
    return AsmRegisterXmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RZ_SAE );
  }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterXmm& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterXmm& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterYmm - YMM registers (YMM0-YMM31)
// ============================================================================

/// @brief A YMM assembler register (YMM0-YMM31)
struct AsmRegisterYmm {
  Register value = Register::NONE;
  uint32_t flags = AsmOperandFlags::NONE;

  constexpr AsmRegisterYmm() noexcept = default;
  explicit constexpr AsmRegisterYmm( Register reg, uint32_t flags_ = AsmOperandFlags::NONE ) noexcept
    : value( reg ), flags( flags_ ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  // VSIB addressing support
  [[nodiscard]] constexpr AsmMemoryOperand operator*( int32_t scale ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, Register::NONE, value, scale, 0 );
  }

  // Mask and zeroing methods
  [[nodiscard]] constexpr AsmRegisterYmm k1() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K1 );
  }
  [[nodiscard]] constexpr AsmRegisterYmm k2() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K2 );
  }
  [[nodiscard]] constexpr AsmRegisterYmm k3() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K3 );
  }
  [[nodiscard]] constexpr AsmRegisterYmm k4() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K4 );
  }
  [[nodiscard]] constexpr AsmRegisterYmm k5() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K5 );
  }
  [[nodiscard]] constexpr AsmRegisterYmm k6() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K6 );
  }
  [[nodiscard]] constexpr AsmRegisterYmm k7() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K7 );
  }
  [[nodiscard]] constexpr AsmRegisterYmm z() const noexcept {
    return AsmRegisterYmm( value, flags | AsmOperandFlags::ZEROING );
  }
  [[nodiscard]] constexpr AsmRegisterYmm sae() const noexcept {
    return AsmRegisterYmm( value, flags | AsmOperandFlags::SUPPRESS_ALL_EXCEPTIONS );
  }
  [[nodiscard]] constexpr AsmRegisterYmm rn_sae() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RN_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterYmm rd_sae() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RD_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterYmm ru_sae() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RU_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterYmm rz_sae() const noexcept {
    return AsmRegisterYmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RZ_SAE );
  }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterYmm& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterYmm& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterZmm - ZMM registers (ZMM0-ZMM31)
// ============================================================================

/// @brief A ZMM assembler register (ZMM0-ZMM31)
struct AsmRegisterZmm {
  Register value = Register::NONE;
  uint32_t flags = AsmOperandFlags::NONE;

  constexpr AsmRegisterZmm() noexcept = default;
  explicit constexpr AsmRegisterZmm( Register reg, uint32_t flags_ = AsmOperandFlags::NONE ) noexcept
    : value( reg ), flags( flags_ ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  // VSIB addressing support
  [[nodiscard]] constexpr AsmMemoryOperand operator*( int32_t scale ) const noexcept {
    return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, Register::NONE, value, scale, 0 );
  }

  // Mask and zeroing methods
  [[nodiscard]] constexpr AsmRegisterZmm k1() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K1 );
  }
  [[nodiscard]] constexpr AsmRegisterZmm k2() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K2 );
  }
  [[nodiscard]] constexpr AsmRegisterZmm k3() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K3 );
  }
  [[nodiscard]] constexpr AsmRegisterZmm k4() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K4 );
  }
  [[nodiscard]] constexpr AsmRegisterZmm k5() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K5 );
  }
  [[nodiscard]] constexpr AsmRegisterZmm k6() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K6 );
  }
  [[nodiscard]] constexpr AsmRegisterZmm k7() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K7 );
  }
  [[nodiscard]] constexpr AsmRegisterZmm z() const noexcept {
    return AsmRegisterZmm( value, flags | AsmOperandFlags::ZEROING );
  }
  [[nodiscard]] constexpr AsmRegisterZmm sae() const noexcept {
    return AsmRegisterZmm( value, flags | AsmOperandFlags::SUPPRESS_ALL_EXCEPTIONS );
  }
  [[nodiscard]] constexpr AsmRegisterZmm rn_sae() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RN_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterZmm rd_sae() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RD_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterZmm ru_sae() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RU_SAE );
  }
  [[nodiscard]] constexpr AsmRegisterZmm rz_sae() const noexcept {
    return AsmRegisterZmm( value, ( flags & ~AsmOperandFlags::ROUNDING_CONTROL_MASK ) | AsmOperandFlags::RZ_SAE );
  }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterZmm& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterZmm& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterK - Opmask registers (K0-K7)
// ============================================================================

/// @brief An opmask assembler register (K0-K7)
struct AsmRegisterK {
  Register value = Register::NONE;

  constexpr AsmRegisterK() noexcept = default;
  explicit constexpr AsmRegisterK( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterK& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterK& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterSt - FPU registers (ST0-ST7)
// ============================================================================

/// @brief An FPU assembler register (ST0-ST7)
struct AsmRegisterSt {
  Register value = Register::NONE;

  constexpr AsmRegisterSt() noexcept = default;
  explicit constexpr AsmRegisterSt( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterSt& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterSt& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterMm - MMX registers (MM0-MM7)
// ============================================================================

/// @brief An MMX assembler register (MM0-MM7)
struct AsmRegisterMm {
  Register value = Register::NONE;

  constexpr AsmRegisterMm() noexcept = default;
  explicit constexpr AsmRegisterMm( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterMm& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterMm& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterSegment - Segment registers
// ============================================================================

/// @brief A segment assembler register (ES, CS, SS, DS, FS, GS)
struct AsmRegisterSegment {
  Register value = Register::NONE;

  constexpr AsmRegisterSegment() noexcept = default;
  explicit constexpr AsmRegisterSegment( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterSegment& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterSegment& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterCr - Control registers (CR0-CR15)
// ============================================================================

/// @brief A control assembler register (CR0-CR15)
struct AsmRegisterCr {
  Register value = Register::NONE;

  constexpr AsmRegisterCr() noexcept = default;
  explicit constexpr AsmRegisterCr( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterCr& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterCr& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterDr - Debug registers (DR0-DR15)
// ============================================================================

/// @brief A debug assembler register (DR0-DR15)
struct AsmRegisterDr {
  Register value = Register::NONE;

  constexpr AsmRegisterDr() noexcept = default;
  explicit constexpr AsmRegisterDr( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterDr& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterDr& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// AsmRegisterBnd - Bound registers (BND0-BND3)
// ============================================================================

/// @brief A bound assembler register (BND0-BND3)
struct AsmRegisterBnd {
  Register value = Register::NONE;

  constexpr AsmRegisterBnd() noexcept = default;
  explicit constexpr AsmRegisterBnd( Register reg ) noexcept : value( reg ) {}

  /// @brief Implicit conversion to Register
  [[nodiscard]] constexpr operator Register() const noexcept { return value; }

  [[nodiscard]] constexpr bool operator==( const AsmRegisterBnd& other ) const noexcept {
    return value == other.value;
  }
  [[nodiscard]] constexpr bool operator!=( const AsmRegisterBnd& other ) const noexcept {
    return value != other.value;
  }
};

// ============================================================================
// Memory operand creation from typed registers with ptr functions
// ============================================================================

[[nodiscard]] inline constexpr AsmMemoryOperand ptr( AsmRegister16 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

[[nodiscard]] inline constexpr AsmMemoryOperand byte_ptr( AsmRegister16 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::BYTE, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand byte_ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::BYTE, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand byte_ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::BYTE, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

[[nodiscard]] inline constexpr AsmMemoryOperand word_ptr( AsmRegister16 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::WORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand word_ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::WORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand word_ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::WORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

[[nodiscard]] inline constexpr AsmMemoryOperand dword_ptr( AsmRegister16 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand dword_ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand dword_ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

[[nodiscard]] inline constexpr AsmMemoryOperand qword_ptr( AsmRegister16 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand qword_ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand qword_ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

[[nodiscard]] inline constexpr AsmMemoryOperand xmmword_ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::XWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand xmmword_ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::XWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

[[nodiscard]] inline constexpr AsmMemoryOperand ymmword_ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::YWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand ymmword_ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::YWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

[[nodiscard]] inline constexpr AsmMemoryOperand zmmword_ptr( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::ZWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}
[[nodiscard]] inline constexpr AsmMemoryOperand zmmword_ptr( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::ZWORD, Register::NONE, reg.value, Register::NONE, 1, 0 );
}

// Broadcast ptr functions from registers
[[nodiscard]] inline constexpr AsmMemoryOperand dword_bcst( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, Register::NONE, reg.value, Register::NONE, 1, 0, AsmOperandFlags::BROADCAST );
}
[[nodiscard]] inline constexpr AsmMemoryOperand dword_bcst( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, Register::NONE, reg.value, Register::NONE, 1, 0, AsmOperandFlags::BROADCAST );
}
[[nodiscard]] inline constexpr AsmMemoryOperand qword_bcst( AsmRegister32 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, Register::NONE, reg.value, Register::NONE, 1, 0, AsmOperandFlags::BROADCAST );
}
[[nodiscard]] inline constexpr AsmMemoryOperand qword_bcst( AsmRegister64 reg ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, Register::NONE, reg.value, Register::NONE, 1, 0, AsmOperandFlags::BROADCAST );
}

} // namespace iced_x86

#endif // ICED_X86_ASM_REGISTERS_HPP
