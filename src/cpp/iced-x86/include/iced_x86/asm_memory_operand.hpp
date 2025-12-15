// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_ASM_MEMORY_OPERAND_HPP
#define ICED_X86_ASM_MEMORY_OPERAND_HPP

#include "register.hpp"
#include "memory_operand.hpp"
#include <cstdint>

namespace iced_x86 {

// Forward declarations
struct AsmRegister16;
struct AsmRegister32;
struct AsmRegister64;
struct AsmRegisterXmm;
struct AsmRegisterYmm;
struct AsmRegisterZmm;

/// @brief Memory operand size hint for assembler
enum class AsmMemoryOperandSize : uint8_t {
  NONE = 0,
  BYTE = 1,
  WORD = 2,
  DWORD = 3,
  QWORD = 4,
  TBYTE = 5,
  FWORD = 6,
  XWORD = 7,   // 128-bit (xmmword)
  YWORD = 8,   // 256-bit (ymmword)
  ZWORD = 9,   // 512-bit (zmmword)
};

/// @brief Assembler operand flags for masking, zeroing, broadcast, etc.
struct AsmOperandFlags {
  static constexpr uint32_t NONE = 0;
  static constexpr uint32_t BROADCAST = 1u << 0;
  static constexpr uint32_t ZEROING = 1u << 1;
  static constexpr uint32_t SUPPRESS_ALL_EXCEPTIONS = 1u << 2;

  // Mask register (K1-K7) stored in bits 6-8
  static constexpr uint32_t K1 = 1u << 6;
  static constexpr uint32_t K2 = 2u << 6;
  static constexpr uint32_t K3 = 3u << 6;
  static constexpr uint32_t K4 = 4u << 6;
  static constexpr uint32_t K5 = 5u << 6;
  static constexpr uint32_t K6 = 6u << 6;
  static constexpr uint32_t K7 = 7u << 6;
  static constexpr uint32_t REGISTER_MASK = 7u << 6;

  // Rounding control stored in bits 3-5
  static constexpr uint32_t RN_SAE = 1u << 3;  // Round to nearest
  static constexpr uint32_t RD_SAE = 2u << 3;  // Round down
  static constexpr uint32_t RU_SAE = 3u << 3;  // Round up
  static constexpr uint32_t RZ_SAE = 4u << 3;  // Round toward zero
  static constexpr uint32_t ROUNDING_CONTROL_MASK = 7u << 3;
};

/// @brief An assembler memory operand used with CodeAssembler.
///
/// This struct represents a memory operand that can be constructed using
/// operator overloads on registers. For example:
/// @code
/// // [rax + rdx*4 + 0x10]
/// auto mem = rax + rdx * 4 + 0x10;
/// // dword ptr [eax + ecx*2 - 8]
/// auto mem2 = dword_ptr(eax + ecx * 2 - 8);
/// @endcode
struct AsmMemoryOperand {
  Register segment = Register::NONE;
  Register base = Register::NONE;
  Register index = Register::NONE;
  int32_t scale = 1;
  int64_t displacement = 0;
  AsmMemoryOperandSize size = AsmMemoryOperandSize::NONE;
  uint32_t flags = AsmOperandFlags::NONE;

  /// @brief Default constructor
  constexpr AsmMemoryOperand() noexcept = default;

  /// @brief Full constructor
  constexpr AsmMemoryOperand(
    AsmMemoryOperandSize size_,
    Register segment_,
    Register base_,
    Register index_,
    int32_t scale_,
    int64_t displacement_,
    uint32_t flags_ = AsmOperandFlags::NONE
  ) noexcept
    : segment( segment_ )
    , base( base_ )
    , index( index_ )
    , scale( scale_ )
    , displacement( displacement_ )
    , size( size_ )
    , flags( flags_ )
  {}

  /// @brief Checks if this is a displacement-only operand (no base or index)
  [[nodiscard]] constexpr bool is_displacement_only() const noexcept {
    return base == Register::NONE && index == Register::NONE;
  }

  /// @brief Checks if this operand has broadcast flag set
  [[nodiscard]] constexpr bool is_broadcast() const noexcept {
    return ( flags & AsmOperandFlags::BROADCAST ) != 0;
  }

  /// @brief Converts to a MemoryOperand for use with Instruction::with*()
  /// @param bitness Assembler bitness (16, 32, or 64)
  [[nodiscard]] MemoryOperand to_memory_operand( uint32_t bitness ) const noexcept {
    int32_t displ_size = 1;
    if ( is_displacement_only() ) {
      displ_size = static_cast<int32_t>( bitness / 8 );
    } else if ( displacement == 0 ) {
      displ_size = 0;
    }
    return MemoryOperand(
      base, index, scale, displacement, displ_size,
      is_broadcast(), segment
    );
  }

  // ============================================================================
  // Mask register methods (k1-k7)
  // ============================================================================

  /// @brief Apply mask register K1
  [[nodiscard]] constexpr AsmMemoryOperand k1() const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement,
                             ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K1 );
  }

  /// @brief Apply mask register K2
  [[nodiscard]] constexpr AsmMemoryOperand k2() const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement,
                             ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K2 );
  }

  /// @brief Apply mask register K3
  [[nodiscard]] constexpr AsmMemoryOperand k3() const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement,
                             ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K3 );
  }

  /// @brief Apply mask register K4
  [[nodiscard]] constexpr AsmMemoryOperand k4() const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement,
                             ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K4 );
  }

  /// @brief Apply mask register K5
  [[nodiscard]] constexpr AsmMemoryOperand k5() const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement,
                             ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K5 );
  }

  /// @brief Apply mask register K6
  [[nodiscard]] constexpr AsmMemoryOperand k6() const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement,
                             ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K6 );
  }

  /// @brief Apply mask register K7
  [[nodiscard]] constexpr AsmMemoryOperand k7() const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement,
                             ( flags & ~AsmOperandFlags::REGISTER_MASK ) | AsmOperandFlags::K7 );
  }

  // ============================================================================
  // Segment override methods
  // ============================================================================

  /// @brief Apply CS segment override
  [[nodiscard]] constexpr AsmMemoryOperand cs() const noexcept {
    return AsmMemoryOperand( size, Register::CS, base, index, scale, displacement, flags );
  }

  /// @brief Apply SS segment override
  [[nodiscard]] constexpr AsmMemoryOperand ss() const noexcept {
    return AsmMemoryOperand( size, Register::SS, base, index, scale, displacement, flags );
  }

  /// @brief Apply DS segment override
  [[nodiscard]] constexpr AsmMemoryOperand ds() const noexcept {
    return AsmMemoryOperand( size, Register::DS, base, index, scale, displacement, flags );
  }

  /// @brief Apply ES segment override
  [[nodiscard]] constexpr AsmMemoryOperand es() const noexcept {
    return AsmMemoryOperand( size, Register::ES, base, index, scale, displacement, flags );
  }

  /// @brief Apply FS segment override
  [[nodiscard]] constexpr AsmMemoryOperand fs() const noexcept {
    return AsmMemoryOperand( size, Register::FS, base, index, scale, displacement, flags );
  }

  /// @brief Apply GS segment override
  [[nodiscard]] constexpr AsmMemoryOperand gs() const noexcept {
    return AsmMemoryOperand( size, Register::GS, base, index, scale, displacement, flags );
  }

  // ============================================================================
  // Arithmetic operators for building memory operands
  // ============================================================================

  /// @brief Add displacement
  [[nodiscard]] constexpr AsmMemoryOperand operator+( int64_t disp ) const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement + disp, flags );
  }

  /// @brief Subtract displacement
  [[nodiscard]] constexpr AsmMemoryOperand operator-( int64_t disp ) const noexcept {
    return AsmMemoryOperand( size, segment, base, index, scale, displacement - disp, flags );
  }

  /// @brief Equality comparison
  [[nodiscard]] constexpr bool operator==( const AsmMemoryOperand& other ) const noexcept {
    return segment == other.segment && base == other.base && index == other.index &&
           scale == other.scale && displacement == other.displacement &&
           size == other.size && flags == other.flags;
  }

  /// @brief Inequality comparison
  [[nodiscard]] constexpr bool operator!=( const AsmMemoryOperand& other ) const noexcept {
    return !( *this == other );
  }
};

// ============================================================================
// Memory pointer functions (byte_ptr, word_ptr, dword_ptr, etc.)
// ============================================================================

/// @brief Create a memory operand with no size hint
[[nodiscard]] inline constexpr AsmMemoryOperand ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::NONE, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a memory operand with no size hint from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::NONE, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create a byte ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand byte_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::BYTE, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a byte ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand byte_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::BYTE, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create a word ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand word_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::WORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a word ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand word_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::WORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create a dword ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand dword_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a dword ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand dword_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create a qword ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand qword_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a qword ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand qword_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create a tbyte ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand tbyte_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::TBYTE, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a tbyte ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand tbyte_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::TBYTE, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create an fword ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand fword_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::FWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create an fword ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand fword_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::FWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create an xmmword ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand xmmword_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::XWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create an xmmword ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand xmmword_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::XWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create a ymmword ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand ymmword_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::YWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a ymmword ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand ymmword_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::YWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

/// @brief Create a zmmword ptr memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand zmmword_ptr( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::ZWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags );
}

/// @brief Create a zmmword ptr memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand zmmword_ptr( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::ZWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::NONE );
}

// ============================================================================
// Broadcast memory pointer functions (dword_bcst, qword_bcst)
// ============================================================================

/// @brief Create a dword broadcast memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand dword_bcst( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags | AsmOperandFlags::BROADCAST );
}

/// @brief Create a dword broadcast memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand dword_bcst( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::DWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::BROADCAST );
}

/// @brief Create a qword broadcast memory operand
[[nodiscard]] inline constexpr AsmMemoryOperand qword_bcst( const AsmMemoryOperand& mem ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, mem.segment, mem.base, mem.index,
                           mem.scale, mem.displacement, mem.flags | AsmOperandFlags::BROADCAST );
}

/// @brief Create a qword broadcast memory operand from displacement
[[nodiscard]] inline constexpr AsmMemoryOperand qword_bcst( int64_t displacement ) noexcept {
  return AsmMemoryOperand( AsmMemoryOperandSize::QWORD, Register::NONE, Register::NONE,
                           Register::NONE, 1, displacement, AsmOperandFlags::BROADCAST );
}

} // namespace iced_x86

#endif // ICED_X86_ASM_MEMORY_OPERAND_HPP
