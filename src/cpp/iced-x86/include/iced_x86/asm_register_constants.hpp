// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// ⚠️ This file provides named register constants for CodeAssembler

#pragma once
#ifndef ICED_X86_ASM_REGISTER_CONSTANTS_HPP
#define ICED_X86_ASM_REGISTER_CONSTANTS_HPP

#include "asm_registers.hpp"

namespace iced_x86 {

// ============================================================================
// 8-bit registers
// ============================================================================

inline constexpr AsmRegister8 al{ Register::AL };
inline constexpr AsmRegister8 cl{ Register::CL };
inline constexpr AsmRegister8 dl{ Register::DL };
inline constexpr AsmRegister8 bl{ Register::BL };
inline constexpr AsmRegister8 ah{ Register::AH };
inline constexpr AsmRegister8 ch{ Register::CH };
inline constexpr AsmRegister8 dh{ Register::DH };
inline constexpr AsmRegister8 bh{ Register::BH };
inline constexpr AsmRegister8 spl{ Register::SPL };
inline constexpr AsmRegister8 bpl{ Register::BPL };
inline constexpr AsmRegister8 sil{ Register::SIL };
inline constexpr AsmRegister8 dil{ Register::DIL };
inline constexpr AsmRegister8 r8l{ Register::R8_L };
inline constexpr AsmRegister8 r9l{ Register::R9_L };
inline constexpr AsmRegister8 r10l{ Register::R10_L };
inline constexpr AsmRegister8 r11l{ Register::R11_L };
inline constexpr AsmRegister8 r12l{ Register::R12_L };
inline constexpr AsmRegister8 r13l{ Register::R13_L };
inline constexpr AsmRegister8 r14l{ Register::R14_L };
inline constexpr AsmRegister8 r15l{ Register::R15_L };

// ============================================================================
// 16-bit registers
// ============================================================================

inline constexpr AsmRegister16 ax{ Register::AX };
inline constexpr AsmRegister16 cx{ Register::CX };
inline constexpr AsmRegister16 dx{ Register::DX };
inline constexpr AsmRegister16 bx{ Register::BX };
inline constexpr AsmRegister16 sp{ Register::SP };
inline constexpr AsmRegister16 bp{ Register::BP };
inline constexpr AsmRegister16 si{ Register::SI };
inline constexpr AsmRegister16 di{ Register::DI };
inline constexpr AsmRegister16 r8w{ Register::R8_W };
inline constexpr AsmRegister16 r9w{ Register::R9_W };
inline constexpr AsmRegister16 r10w{ Register::R10_W };
inline constexpr AsmRegister16 r11w{ Register::R11_W };
inline constexpr AsmRegister16 r12w{ Register::R12_W };
inline constexpr AsmRegister16 r13w{ Register::R13_W };
inline constexpr AsmRegister16 r14w{ Register::R14_W };
inline constexpr AsmRegister16 r15w{ Register::R15_W };

// ============================================================================
// 32-bit registers
// ============================================================================

inline constexpr AsmRegister32 eax{ Register::EAX };
inline constexpr AsmRegister32 ecx{ Register::ECX };
inline constexpr AsmRegister32 edx{ Register::EDX };
inline constexpr AsmRegister32 ebx{ Register::EBX };
inline constexpr AsmRegister32 esp{ Register::ESP };
inline constexpr AsmRegister32 ebp{ Register::EBP };
inline constexpr AsmRegister32 esi{ Register::ESI };
inline constexpr AsmRegister32 edi{ Register::EDI };
inline constexpr AsmRegister32 r8d{ Register::R8_D };
inline constexpr AsmRegister32 r9d{ Register::R9_D };
inline constexpr AsmRegister32 r10d{ Register::R10_D };
inline constexpr AsmRegister32 r11d{ Register::R11_D };
inline constexpr AsmRegister32 r12d{ Register::R12_D };
inline constexpr AsmRegister32 r13d{ Register::R13_D };
inline constexpr AsmRegister32 r14d{ Register::R14_D };
inline constexpr AsmRegister32 r15d{ Register::R15_D };

// ============================================================================
// 64-bit registers
// ============================================================================

inline constexpr AsmRegister64 rax{ Register::RAX };
inline constexpr AsmRegister64 rcx{ Register::RCX };
inline constexpr AsmRegister64 rdx{ Register::RDX };
inline constexpr AsmRegister64 rbx{ Register::RBX };
inline constexpr AsmRegister64 rsp{ Register::RSP };
inline constexpr AsmRegister64 rbp{ Register::RBP };
inline constexpr AsmRegister64 rsi{ Register::RSI };
inline constexpr AsmRegister64 rdi{ Register::RDI };
inline constexpr AsmRegister64 r8{ Register::R8 };
inline constexpr AsmRegister64 r9{ Register::R9 };
inline constexpr AsmRegister64 r10{ Register::R10 };
inline constexpr AsmRegister64 r11{ Register::R11 };
inline constexpr AsmRegister64 r12{ Register::R12 };
inline constexpr AsmRegister64 r13{ Register::R13 };
inline constexpr AsmRegister64 r14{ Register::R14 };
inline constexpr AsmRegister64 r15{ Register::R15 };

// ============================================================================
// XMM registers
// ============================================================================

inline constexpr AsmRegisterXmm xmm0{ Register::XMM0 };
inline constexpr AsmRegisterXmm xmm1{ Register::XMM1 };
inline constexpr AsmRegisterXmm xmm2{ Register::XMM2 };
inline constexpr AsmRegisterXmm xmm3{ Register::XMM3 };
inline constexpr AsmRegisterXmm xmm4{ Register::XMM4 };
inline constexpr AsmRegisterXmm xmm5{ Register::XMM5 };
inline constexpr AsmRegisterXmm xmm6{ Register::XMM6 };
inline constexpr AsmRegisterXmm xmm7{ Register::XMM7 };
inline constexpr AsmRegisterXmm xmm8{ Register::XMM8 };
inline constexpr AsmRegisterXmm xmm9{ Register::XMM9 };
inline constexpr AsmRegisterXmm xmm10{ Register::XMM10 };
inline constexpr AsmRegisterXmm xmm11{ Register::XMM11 };
inline constexpr AsmRegisterXmm xmm12{ Register::XMM12 };
inline constexpr AsmRegisterXmm xmm13{ Register::XMM13 };
inline constexpr AsmRegisterXmm xmm14{ Register::XMM14 };
inline constexpr AsmRegisterXmm xmm15{ Register::XMM15 };
inline constexpr AsmRegisterXmm xmm16{ Register::XMM16 };
inline constexpr AsmRegisterXmm xmm17{ Register::XMM17 };
inline constexpr AsmRegisterXmm xmm18{ Register::XMM18 };
inline constexpr AsmRegisterXmm xmm19{ Register::XMM19 };
inline constexpr AsmRegisterXmm xmm20{ Register::XMM20 };
inline constexpr AsmRegisterXmm xmm21{ Register::XMM21 };
inline constexpr AsmRegisterXmm xmm22{ Register::XMM22 };
inline constexpr AsmRegisterXmm xmm23{ Register::XMM23 };
inline constexpr AsmRegisterXmm xmm24{ Register::XMM24 };
inline constexpr AsmRegisterXmm xmm25{ Register::XMM25 };
inline constexpr AsmRegisterXmm xmm26{ Register::XMM26 };
inline constexpr AsmRegisterXmm xmm27{ Register::XMM27 };
inline constexpr AsmRegisterXmm xmm28{ Register::XMM28 };
inline constexpr AsmRegisterXmm xmm29{ Register::XMM29 };
inline constexpr AsmRegisterXmm xmm30{ Register::XMM30 };
inline constexpr AsmRegisterXmm xmm31{ Register::XMM31 };

// ============================================================================
// YMM registers
// ============================================================================

inline constexpr AsmRegisterYmm ymm0{ Register::YMM0 };
inline constexpr AsmRegisterYmm ymm1{ Register::YMM1 };
inline constexpr AsmRegisterYmm ymm2{ Register::YMM2 };
inline constexpr AsmRegisterYmm ymm3{ Register::YMM3 };
inline constexpr AsmRegisterYmm ymm4{ Register::YMM4 };
inline constexpr AsmRegisterYmm ymm5{ Register::YMM5 };
inline constexpr AsmRegisterYmm ymm6{ Register::YMM6 };
inline constexpr AsmRegisterYmm ymm7{ Register::YMM7 };
inline constexpr AsmRegisterYmm ymm8{ Register::YMM8 };
inline constexpr AsmRegisterYmm ymm9{ Register::YMM9 };
inline constexpr AsmRegisterYmm ymm10{ Register::YMM10 };
inline constexpr AsmRegisterYmm ymm11{ Register::YMM11 };
inline constexpr AsmRegisterYmm ymm12{ Register::YMM12 };
inline constexpr AsmRegisterYmm ymm13{ Register::YMM13 };
inline constexpr AsmRegisterYmm ymm14{ Register::YMM14 };
inline constexpr AsmRegisterYmm ymm15{ Register::YMM15 };
inline constexpr AsmRegisterYmm ymm16{ Register::YMM16 };
inline constexpr AsmRegisterYmm ymm17{ Register::YMM17 };
inline constexpr AsmRegisterYmm ymm18{ Register::YMM18 };
inline constexpr AsmRegisterYmm ymm19{ Register::YMM19 };
inline constexpr AsmRegisterYmm ymm20{ Register::YMM20 };
inline constexpr AsmRegisterYmm ymm21{ Register::YMM21 };
inline constexpr AsmRegisterYmm ymm22{ Register::YMM22 };
inline constexpr AsmRegisterYmm ymm23{ Register::YMM23 };
inline constexpr AsmRegisterYmm ymm24{ Register::YMM24 };
inline constexpr AsmRegisterYmm ymm25{ Register::YMM25 };
inline constexpr AsmRegisterYmm ymm26{ Register::YMM26 };
inline constexpr AsmRegisterYmm ymm27{ Register::YMM27 };
inline constexpr AsmRegisterYmm ymm28{ Register::YMM28 };
inline constexpr AsmRegisterYmm ymm29{ Register::YMM29 };
inline constexpr AsmRegisterYmm ymm30{ Register::YMM30 };
inline constexpr AsmRegisterYmm ymm31{ Register::YMM31 };

// ============================================================================
// ZMM registers
// ============================================================================

inline constexpr AsmRegisterZmm zmm0{ Register::ZMM0 };
inline constexpr AsmRegisterZmm zmm1{ Register::ZMM1 };
inline constexpr AsmRegisterZmm zmm2{ Register::ZMM2 };
inline constexpr AsmRegisterZmm zmm3{ Register::ZMM3 };
inline constexpr AsmRegisterZmm zmm4{ Register::ZMM4 };
inline constexpr AsmRegisterZmm zmm5{ Register::ZMM5 };
inline constexpr AsmRegisterZmm zmm6{ Register::ZMM6 };
inline constexpr AsmRegisterZmm zmm7{ Register::ZMM7 };
inline constexpr AsmRegisterZmm zmm8{ Register::ZMM8 };
inline constexpr AsmRegisterZmm zmm9{ Register::ZMM9 };
inline constexpr AsmRegisterZmm zmm10{ Register::ZMM10 };
inline constexpr AsmRegisterZmm zmm11{ Register::ZMM11 };
inline constexpr AsmRegisterZmm zmm12{ Register::ZMM12 };
inline constexpr AsmRegisterZmm zmm13{ Register::ZMM13 };
inline constexpr AsmRegisterZmm zmm14{ Register::ZMM14 };
inline constexpr AsmRegisterZmm zmm15{ Register::ZMM15 };
inline constexpr AsmRegisterZmm zmm16{ Register::ZMM16 };
inline constexpr AsmRegisterZmm zmm17{ Register::ZMM17 };
inline constexpr AsmRegisterZmm zmm18{ Register::ZMM18 };
inline constexpr AsmRegisterZmm zmm19{ Register::ZMM19 };
inline constexpr AsmRegisterZmm zmm20{ Register::ZMM20 };
inline constexpr AsmRegisterZmm zmm21{ Register::ZMM21 };
inline constexpr AsmRegisterZmm zmm22{ Register::ZMM22 };
inline constexpr AsmRegisterZmm zmm23{ Register::ZMM23 };
inline constexpr AsmRegisterZmm zmm24{ Register::ZMM24 };
inline constexpr AsmRegisterZmm zmm25{ Register::ZMM25 };
inline constexpr AsmRegisterZmm zmm26{ Register::ZMM26 };
inline constexpr AsmRegisterZmm zmm27{ Register::ZMM27 };
inline constexpr AsmRegisterZmm zmm28{ Register::ZMM28 };
inline constexpr AsmRegisterZmm zmm29{ Register::ZMM29 };
inline constexpr AsmRegisterZmm zmm30{ Register::ZMM30 };
inline constexpr AsmRegisterZmm zmm31{ Register::ZMM31 };

// ============================================================================
// Opmask registers
// ============================================================================

inline constexpr AsmRegisterK k0{ Register::K0 };
inline constexpr AsmRegisterK k1{ Register::K1 };
inline constexpr AsmRegisterK k2{ Register::K2 };
inline constexpr AsmRegisterK k3{ Register::K3 };
inline constexpr AsmRegisterK k4{ Register::K4 };
inline constexpr AsmRegisterK k5{ Register::K5 };
inline constexpr AsmRegisterK k6{ Register::K6 };
inline constexpr AsmRegisterK k7{ Register::K7 };

// ============================================================================
// FPU registers
// ============================================================================

inline constexpr AsmRegisterSt st0{ Register::ST0 };
inline constexpr AsmRegisterSt st1{ Register::ST1 };
inline constexpr AsmRegisterSt st2{ Register::ST2 };
inline constexpr AsmRegisterSt st3{ Register::ST3 };
inline constexpr AsmRegisterSt st4{ Register::ST4 };
inline constexpr AsmRegisterSt st5{ Register::ST5 };
inline constexpr AsmRegisterSt st6{ Register::ST6 };
inline constexpr AsmRegisterSt st7{ Register::ST7 };

// ============================================================================
// MMX registers
// ============================================================================

inline constexpr AsmRegisterMm mm0{ Register::MM0 };
inline constexpr AsmRegisterMm mm1{ Register::MM1 };
inline constexpr AsmRegisterMm mm2{ Register::MM2 };
inline constexpr AsmRegisterMm mm3{ Register::MM3 };
inline constexpr AsmRegisterMm mm4{ Register::MM4 };
inline constexpr AsmRegisterMm mm5{ Register::MM5 };
inline constexpr AsmRegisterMm mm6{ Register::MM6 };
inline constexpr AsmRegisterMm mm7{ Register::MM7 };

// ============================================================================
// Segment registers
// ============================================================================

inline constexpr AsmRegisterSegment es_{ Register::ES };
inline constexpr AsmRegisterSegment cs_{ Register::CS };
inline constexpr AsmRegisterSegment ss_{ Register::SS };
inline constexpr AsmRegisterSegment ds_{ Register::DS };
inline constexpr AsmRegisterSegment fs_{ Register::FS };
inline constexpr AsmRegisterSegment gs_{ Register::GS };

// ============================================================================
// Control registers
// ============================================================================

inline constexpr AsmRegisterCr cr0{ Register::CR0 };
inline constexpr AsmRegisterCr cr1{ Register::CR1 };
inline constexpr AsmRegisterCr cr2{ Register::CR2 };
inline constexpr AsmRegisterCr cr3{ Register::CR3 };
inline constexpr AsmRegisterCr cr4{ Register::CR4 };
inline constexpr AsmRegisterCr cr5{ Register::CR5 };
inline constexpr AsmRegisterCr cr6{ Register::CR6 };
inline constexpr AsmRegisterCr cr7{ Register::CR7 };
inline constexpr AsmRegisterCr cr8{ Register::CR8 };
inline constexpr AsmRegisterCr cr9{ Register::CR9 };
inline constexpr AsmRegisterCr cr10{ Register::CR10 };
inline constexpr AsmRegisterCr cr11{ Register::CR11 };
inline constexpr AsmRegisterCr cr12{ Register::CR12 };
inline constexpr AsmRegisterCr cr13{ Register::CR13 };
inline constexpr AsmRegisterCr cr14{ Register::CR14 };
inline constexpr AsmRegisterCr cr15{ Register::CR15 };

// ============================================================================
// Debug registers
// ============================================================================

inline constexpr AsmRegisterDr dr0{ Register::DR0 };
inline constexpr AsmRegisterDr dr1{ Register::DR1 };
inline constexpr AsmRegisterDr dr2{ Register::DR2 };
inline constexpr AsmRegisterDr dr3{ Register::DR3 };
inline constexpr AsmRegisterDr dr4{ Register::DR4 };
inline constexpr AsmRegisterDr dr5{ Register::DR5 };
inline constexpr AsmRegisterDr dr6{ Register::DR6 };
inline constexpr AsmRegisterDr dr7{ Register::DR7 };
inline constexpr AsmRegisterDr dr8{ Register::DR8 };
inline constexpr AsmRegisterDr dr9{ Register::DR9 };
inline constexpr AsmRegisterDr dr10{ Register::DR10 };
inline constexpr AsmRegisterDr dr11{ Register::DR11 };
inline constexpr AsmRegisterDr dr12{ Register::DR12 };
inline constexpr AsmRegisterDr dr13{ Register::DR13 };
inline constexpr AsmRegisterDr dr14{ Register::DR14 };
inline constexpr AsmRegisterDr dr15{ Register::DR15 };

// ============================================================================
// Bound registers
// ============================================================================

inline constexpr AsmRegisterBnd bnd0{ Register::BND0 };
inline constexpr AsmRegisterBnd bnd1{ Register::BND1 };
inline constexpr AsmRegisterBnd bnd2{ Register::BND2 };
inline constexpr AsmRegisterBnd bnd3{ Register::BND3 };

} // namespace iced_x86

#endif // ICED_X86_ASM_REGISTER_CONSTANTS_HPP
