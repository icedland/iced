// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_INTERNAL_HANDLERS_HPP
#define ICED_X86_INTERNAL_HANDLERS_HPP

#include <cstdint>
#include <cstddef>
#include <array>
#include <vector>
#include <memory>
#include "../code.hpp"
#include "../register.hpp"
#include "../instruction.hpp"

namespace iced_x86 {

// Forward declarations
class Decoder;

namespace internal {

struct OpCodeHandler;

/// @brief Function pointer type for handler decode functions
using OpCodeHandlerDecodeFn = void ( * )( const OpCodeHandler*, Decoder&, Instruction& );

/// @brief Handler entry: decode function + handler data pointer
struct HandlerEntry {
  OpCodeHandlerDecodeFn decode;
  const OpCodeHandler* handler;
};

/// @brief Base handler structure - all handlers start with has_modrm
struct OpCodeHandler {
  bool has_modrm;
};

// ============================================================================
// Invalid Handler
// ============================================================================

struct OpCodeHandler_Invalid {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// Static instances
extern const OpCodeHandler_Invalid g_null_handler;
extern const OpCodeHandler_Invalid g_invalid_handler;
extern const OpCodeHandler_Invalid g_invalid_no_modrm_handler;

inline bool is_null_instance_handler( const OpCodeHandler* handler ) {
  return handler == reinterpret_cast<const OpCodeHandler*>( &g_null_handler );
}

inline HandlerEntry get_null_handler() {
  return { OpCodeHandler_Invalid::decode, reinterpret_cast<const OpCodeHandler*>( &g_null_handler ) };
}

inline HandlerEntry get_invalid_handler() {
  return { OpCodeHandler_Invalid::decode, reinterpret_cast<const OpCodeHandler*>( &g_invalid_handler ) };
}

inline HandlerEntry get_invalid_no_modrm_handler() {
  return { OpCodeHandler_Invalid::decode, reinterpret_cast<const OpCodeHandler*>( &g_invalid_no_modrm_handler ) };
}

// ============================================================================
// Simple Handler - just sets the code
// ============================================================================

struct OpCodeHandler_Simple {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Group Handler - dispatches based on reg field (8 handlers)
// ============================================================================

struct OpCodeHandler_Group {
  bool has_modrm;
  std::array<HandlerEntry, 8> group_handlers;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Group8x8 Handler - mem vs reg dispatch (8+8 handlers)
// ============================================================================

struct OpCodeHandler_Group8x8 {
  bool has_modrm;
  std::array<HandlerEntry, 8> table_low;   // mod != 3 (memory)
  std::array<HandlerEntry, 8> table_high;  // mod == 3 (register)

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Group8x64 Handler - for FPU (8+64 handlers)
// ============================================================================

struct OpCodeHandler_Group8x64 {
  bool has_modrm;
  std::array<HandlerEntry, 8> table_low;
  std::array<HandlerEntry, 64> table_high;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// AnotherTable Handler - 256-entry table for escape bytes
// ============================================================================

struct OpCodeHandler_AnotherTable {
  bool has_modrm;
  std::array<HandlerEntry, 256> handlers;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// RM Handler - dispatches based on mod==3 (reg) vs mod!=3 (mem)
// ============================================================================

struct OpCodeHandler_RM {
  bool has_modrm;
  HandlerEntry handler_reg;  // mod == 3
  HandlerEntry handler_mem;  // mod != 3

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Bitness Handler - dispatches based on 16/32 vs 64-bit mode
// ============================================================================

struct OpCodeHandler_Bitness {
  bool has_modrm;
  HandlerEntry handler_1632;  // 16/32-bit mode
  HandlerEntry handler_64;    // 64-bit mode

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Bitness_DontReadModRM {
  bool has_modrm;
  HandlerEntry handler_1632;
  HandlerEntry handler_64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// MandatoryPrefix Handler - dispatches based on 66/F2/F3 prefixes
// ============================================================================

struct OpCodeHandler_MandatoryPrefix {
  bool has_modrm;
  std::array<HandlerEntry, 4> handlers;  // [0]=none, [1]=66, [2]=F3, [3]=F2

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_MandatoryPrefix3 {
  bool has_modrm;
  std::array<HandlerEntry, 4> handlers_reg;
  std::array<HandlerEntry, 4> handlers_mem;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_MandatoryPrefix4 {
  bool has_modrm;
  std::array<HandlerEntry, 4> handlers;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Options Handler - dispatches based on decoder options
// ============================================================================

struct OpCodeHandler_Options {
  bool has_modrm;
  HandlerEntry handler_default;
  HandlerEntry handler_option1;
  uint32_t decoder_options1;
  HandlerEntry handler_option2;
  uint32_t decoder_options2;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Options_DontReadModRM {
  bool has_modrm;
  HandlerEntry handler_default;
  HandlerEntry handler_option;
  uint32_t decoder_options;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Options1632 {
  bool has_modrm;
  HandlerEntry handler_default;
  HandlerEntry handler_option1;
  uint32_t decoder_options1;
  HandlerEntry handler_option2;
  uint32_t decoder_options2;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// VEX/EVEX/XOP entry handlers
// ============================================================================

struct OpCodeHandler_VEX2 {
  bool has_modrm;
  HandlerEntry handler_mem;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VEX3 {
  bool has_modrm;
  HandlerEntry handler_mem;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_XOP {
  bool has_modrm;
  HandlerEntry handler_reg0;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_EVEX {
  bool has_modrm;
  HandlerEntry handler_mem;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Prefix handlers
// ============================================================================

struct OpCodeHandler_PrefixEsCsSsDs {
  bool has_modrm;
  Register seg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PrefixFsGs {
  bool has_modrm;
  Register seg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Prefix66 {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Prefix67 {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PrefixF0 {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PrefixF2 {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PrefixF3 {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PrefixREX {
  bool has_modrm;
  HandlerEntry handler;
  uint32_t rex;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// D3NOW handler
// ============================================================================

struct OpCodeHandler_D3NOW {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Register handlers
// ============================================================================

struct OpCodeHandler_Reg {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_RegIb {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_IbReg {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_AL_DX {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_DX_AL {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_DX_eAX {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_eAX_DX {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Immediate handlers
// ============================================================================

struct OpCodeHandler_Ib {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ib3 {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Ev handlers (register/memory with operand size)
// ============================================================================

struct OpCodeHandler_Ev {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Iz {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Ib {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Ib2 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_1 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_CL {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Gv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Gv_flags {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Gv_32_64 {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Gv_Ib {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Gv_CL {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Gv_REX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_REXW {
  bool has_modrm;
  Code code32;
  Code code64;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_Sw {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_P {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ev_VX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Evj {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Evw {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ew {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Eb handlers (byte operands)
// ============================================================================

struct OpCodeHandler_Eb {
  bool has_modrm;
  Code code;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Eb_Gb {
  bool has_modrm;
  Code code;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Eb_Ib {
  bool has_modrm;
  Code code;
  uint32_t flags;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Eb_1 {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Eb_CL {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Gv handlers (general register destination)
// ============================================================================

struct OpCodeHandler_Gv_Ev {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ev_32_64 {
  bool has_modrm;
  Code code32;
  Code code64;
  bool disallow_reg;
  bool disallow_mem;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ev_Ib {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ev_Ib_REX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ev_Iz {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ev_REX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ev2 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ev3 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Eb {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Eb_REX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ew {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_M {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_M_as {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Ma {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Mp {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_Mv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_N {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_N_Ib_REX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_RX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gv_W {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_GvM_VX_Ib {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gdq_Ev {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gb_Eb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Gd_Rd {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Jump handlers
// ============================================================================

struct OpCodeHandler_Jb {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Jb2 {
  bool has_modrm;
  Code code16_16;
  Code code16_32;
  Code code16_64;
  Code code32_16;
  Code code32_32;
  Code code64_32;
  Code code64_64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Jx {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Jz {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Jdisp {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Control/Debug register handlers
// ============================================================================

struct OpCodeHandler_C_R {
  bool has_modrm;
  Code code32;
  Code code64;
  Register base_reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_R_C {
  bool has_modrm;
  Code code32;
  Code code64;
  Register base_reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Memory handlers
// ============================================================================

struct OpCodeHandler_M {
  bool has_modrm;
  Code code;
  Code code2;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_M_REXW {
  bool has_modrm;
  Code code32;
  Code code64;
  uint32_t flags32;
  uint32_t flags64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ms {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Mf {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_MV {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Mv_Gv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Mv_Gv_REXW {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_MemBx {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_MP {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ep {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Push/Pop handlers
// ============================================================================

struct OpCodeHandler_PushOpSizeReg {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PushEv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PushIb2 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PushIz {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PushSimple2 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_PushSimpleReg {
  bool has_modrm;
  uint32_t index;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Simple2/3/4/5 handlers (bitness variants)
// ============================================================================

struct OpCodeHandler_Simple2 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Simple2Iw {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Simple3 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Simple4 {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Simple5 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Simple5_a32 {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Simple5_ModRM_as {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_SimpleReg {
  bool has_modrm;
  Code code;
  uint32_t index;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Register/Immediate combination handlers
// ============================================================================

struct OpCodeHandler_Reg_Iz {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_RegIb3 {
  bool has_modrm;
  uint32_t index;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_RegIz2 {
  bool has_modrm;
  uint32_t index;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reg_Ib2 {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_IbReg2 {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Xchg handler
// ============================================================================

struct OpCodeHandler_Xchg_Reg_rAX {
  bool has_modrm;
  uint32_t index;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Rv handlers
// ============================================================================

struct OpCodeHandler_Rv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Rv_32_64 {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_RvMw_Gw {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Rq {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// String instruction handlers
// ============================================================================

struct OpCodeHandler_Yb_Reg {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Yv_Reg {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Yv_Reg2 {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reg_Xb {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reg_Xv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reg_Xv2 {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reg_Yb {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reg_Yv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Yb_Xb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Yv_Xv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Xb_Yb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Xv_Yv {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Segment register handlers
// ============================================================================

struct OpCodeHandler_Sw_Ev {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Sw_M {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_M_Sw {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Far pointer handlers
// ============================================================================

struct OpCodeHandler_Ap {
  bool has_modrm;
  Code code16;
  Code code32;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Offset handlers (moffs)
// ============================================================================

struct OpCodeHandler_Reg_Ob {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ob_Reg {
  bool has_modrm;
  Code code;
  Register reg;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reg_Ov {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ov_Reg {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Branch handlers
// ============================================================================

struct OpCodeHandler_BranchIw {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_BranchSimple {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Iw_Ib handler
// ============================================================================

struct OpCodeHandler_Iw_Ib {
  bool has_modrm;
  Code code16;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// FPU handlers
// ============================================================================

struct OpCodeHandler_ST_STi {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_STi {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_STi_ST {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// MMX/SSE handlers
// ============================================================================

struct OpCodeHandler_P_Q {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Q_P {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_P_Q_Ib {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_P_W {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_P_R {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_P_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_P_Ev_Ib {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_NIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Reservednop {
  bool has_modrm;
  HandlerEntry handler_rm;
  HandlerEntry handler;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_Ed_V_Ib {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VM {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VN {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VQ {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VRIbIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VW {
  bool has_modrm;
  Code code;
  Code code_w;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VWIb {
  bool has_modrm;
  Code code;
  Code code_w;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VX_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_VX_E_Ib {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_V_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_WV {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_rDI_P_N {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_rDI_VX_RX {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// MPX handlers
// ============================================================================

struct OpCodeHandler_B_BM {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_BM_B {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_B_Ev {
  bool has_modrm;
  Code code32;
  Code code64;
  bool riprel_mask;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_B_MIB {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_MIB_B {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// RIb handler
// ============================================================================

struct OpCodeHandler_RIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

struct OpCodeHandler_RIbIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// Wbinvd handler
// ============================================================================

struct OpCodeHandler_Wbinvd {
  bool has_modrm;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// VEX Handlers
// ============================================================================

// VEX W handler - dispatches based on VEX.W bit
struct OpCodeHandler_VEX_W {
  bool has_modrm;
  HandlerEntry handler_w0;
  HandlerEntry handler_w1;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VectorLength handler - dispatches based on VEX.L bit
struct OpCodeHandler_VEX_VectorLength {
  bool has_modrm;
  HandlerEntry handler_l0;  // L=0 (128-bit)
  HandlerEntry handler_l1;  // L=1 (256-bit)

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VectorLength handler (no ModRM read)
struct OpCodeHandler_VEX_VectorLength_NoModRM {
  bool has_modrm;
  HandlerEntry handler_l0;
  HandlerEntry handler_l1;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX MandatoryPrefix handler - dispatches based on pp field (4 handlers)
struct OpCodeHandler_VEX_MandatoryPrefix2 {
  bool has_modrm;
  std::array<HandlerEntry, 4> handlers;  // [0]=NP, [1]=66, [2]=F3, [3]=F2

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Simple - just sets the code, no operands
struct OpCodeHandler_VEX_Simple {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHW - V=dest(xmm/ymm), H=vvvv(xmm/ymm), W=rm(xmm/ymm/mem)
struct OpCodeHandler_VEX_VHW {
  bool has_modrm;
  Register base_reg1;  // Base for V operand (XMM0 or YMM0)
  Register base_reg2;  // Base for H operand  
  Register base_reg3;  // Base for W operand
  Code code_r;         // Code when mod=3 (register)
  Code code_m;         // Code when mod!=3 (memory)

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VW - V=dest(xmm/ymm), W=rm(xmm/ymm/mem) (2 operands)
struct OpCodeHandler_VEX_VW {
  bool has_modrm;
  Register base_reg1;  // Base for V operand
  Register base_reg2;  // Base for W operand
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VWIb - V=dest, W=rm, Ib=immediate byte
struct OpCodeHandler_VEX_VWIb {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Code code_w0;        // Code when W=0
  Code code_w1;        // Code when W=1

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHWIb - V=dest, H=vvvv, W=rm, Ib=immediate
struct OpCodeHandler_VEX_VHWIb {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Register base_reg3;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX WV - W=dest(rm), V=src(reg) - reversed operand order
struct OpCodeHandler_VEX_WV {
  bool has_modrm;
  Register base_reg1;  // Base for W operand
  Register base_reg2;  // Base for V operand
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VM - V=dest(reg), M=memory only
struct OpCodeHandler_VEX_VM {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX MV - M=dest(memory), V=src(reg)
struct OpCodeHandler_VEX_MV {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX M - Memory operand only
struct OpCodeHandler_VEX_M {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHM - V=dest, H=vvvv, M=memory
struct OpCodeHandler_VEX_VHM {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX MHV - M=dest(mem), H=vvvv, V=src(reg)
struct OpCodeHandler_VEX_MHV {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHEv - V=dest(xmm/ymm), H=vvvv(xmm/ymm), Ev=r/m general purpose reg
struct OpCodeHandler_VEX_VHEv {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHEvIb - V=dest, H=vvvv, Ev=rm, Ib=immediate
struct OpCodeHandler_VEX_VHEvIb {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Ev_VX - Ev=dest(gpr r/m), VX=src(xmm)
struct OpCodeHandler_VEX_Ev_VX {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VX_Ev - VX=dest(xmm), Ev=src(gpr r/m)
struct OpCodeHandler_VEX_VX_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_W - Gv=dest(gpr), W=src(xmm/mem)
struct OpCodeHandler_VEX_Gv_W {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_RX - Gv=dest(gpr), RX=src(xmm reg only)
struct OpCodeHandler_VEX_Gv_RX {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_Ev - Gv=dest(gpr), Ev=src(gpr r/m)
struct OpCodeHandler_VEX_Gv_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Ev - Ev=single operand (gpr r/m)
struct OpCodeHandler_VEX_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Ed_V_Ib - Ed=dest(gpr32 r/m), V=src(xmm), Ib=imm
struct OpCodeHandler_VEX_Ed_V_Ib {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX GvM_VX_Ib - Gv or M dest, VX src, Ib imm
struct OpCodeHandler_VEX_GvM_VX_Ib {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_Ev_Ib - Gv=dest, Ev=src, Ib=imm (BMI)
struct OpCodeHandler_VEX_Gv_Ev_Ib {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_Ev_Id - Gv=dest, Ev=src, Id=imm32
struct OpCodeHandler_VEX_Gv_Ev_Id {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_Ev_Gv - Gv=dest, Ev=src1, Gv2(vvvv)=src2 (BMI)
struct OpCodeHandler_VEX_Gv_Ev_Gv {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Ev_Gv_Gv - Ev=dest, Gv1(reg)=src1, Gv2(vvvv)=src2
struct OpCodeHandler_VEX_Ev_Gv_Gv {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_Gv_Ev - Gv=dest, Gv2(vvvv)=src1, Ev=src2
struct OpCodeHandler_VEX_Gv_Gv_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gv_GPR_Ib - Gv=dest(vvvv), GPR=src(rm), Ib=imm
struct OpCodeHandler_VEX_Gv_GPR_Ib {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Hv_Ev - Hv=dest(vvvv as gpr), Ev=src(gpr r/m)
struct OpCodeHandler_VEX_Hv_Ev {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Hv_Ed_Id - Hv=dest(vvvv), Ed=src(gpr32 r/m), Id=imm32
struct OpCodeHandler_VEX_Hv_Ed_Id {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX HRIb - H=dest(vvvv xmm), R=src(xmm reg), Ib=imm
struct OpCodeHandler_VEX_HRIb {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX rDI_VX_RX - rDI implied, VX=reg, RX=rm(reg)
struct OpCodeHandler_VEX_rDI_VX_RX {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX RdRq - Rd or Rq register (no memory)
struct OpCodeHandler_VEX_RdRq {
  bool has_modrm;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX WHV - W=dest(rm), H=vvvv, V=src(reg)  
struct OpCodeHandler_VEX_WHV {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VWH - V=dest(reg), W=rm, H=vvvv
struct OpCodeHandler_VEX_VWH {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX WVIb - W=dest(rm), V=src(reg), Ib=imm
struct OpCodeHandler_VEX_WVIb {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHWIs4 - V=dest, H=vvvv, W=rm, Is4=imm4(reg)
struct OpCodeHandler_VEX_VHWIs4 {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHIs4W - V=dest, H=vvvv, Is4=imm4(reg), W=rm
struct OpCodeHandler_VEX_VHIs4W {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHWIs5 - 5-operand form
struct OpCodeHandler_VEX_VHWIs5 {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VHIs5W - 5-operand form (different order)
struct OpCodeHandler_VEX_VHIs5W {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Group handler - dispatches based on reg field
struct OpCodeHandler_VEX_Group {
  bool has_modrm;
  std::array<HandlerEntry, 8> handlers;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Bitness handler
struct OpCodeHandler_VEX_Bitness {
  bool has_modrm;
  HandlerEntry handler_1632;
  HandlerEntry handler_64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Bitness (don't read ModRM)
struct OpCodeHandler_VEX_Bitness_DontReadModRM {
  bool has_modrm;
  HandlerEntry handler_1632;
  HandlerEntry handler_64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX RM handler - dispatch based on mod==3
struct OpCodeHandler_VEX_RM {
  bool has_modrm;
  HandlerEntry handler_reg;
  HandlerEntry handler_mem;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// VEX-specific AVX-512 mask register handlers (VEX-encoded, not EVEX)
// ============================================================================

// VEX VK_HK_RK - K reg operations
struct OpCodeHandler_VEX_VK_HK_RK {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VK_RK - K=dest, K=src
struct OpCodeHandler_VEX_VK_RK {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VK_RK_Ib - K=dest, K=src, Ib
struct OpCodeHandler_VEX_VK_RK_Ib {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VK_WK - K=dest, K/mem=src
struct OpCodeHandler_VEX_VK_WK {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VK_R - K=dest, R=src(gpr)
struct OpCodeHandler_VEX_VK_R {
  bool has_modrm;
  Register gpr;  // GPR32 or GPR64 base register
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VK_R_Ib - K=dest, R=src(gpr), Ib
struct OpCodeHandler_VEX_VK_R_Ib {
  bool has_modrm;
  Register gpr;  // GPR32 or GPR64 base register
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX G_VK - G=dest(gpr), K=src
struct OpCodeHandler_VEX_G_VK {
  bool has_modrm;
  Register gpr;  // GPR32 or GPR64 base register
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX M_VK - M=dest(mem), K=src
struct OpCodeHandler_VEX_M_VK {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Gq_HK_RK - for 64-bit GPR with K regs
struct OpCodeHandler_VEX_Gq_HK_RK {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// VEX VSIB handlers
// ============================================================================

// VEX VX_VSIB_HX - gather/scatter with VSIB addressing
struct OpCodeHandler_VEX_VX_VSIB_HX {
  bool has_modrm;
  Register base_reg1;
  Register vsib_base;
  Register base_reg3;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// VEX AMX (Advanced Matrix Extensions) handlers
// ============================================================================

// VEX VT_SIBMEM - tile register dest, SIBMEM src
struct OpCodeHandler_VEX_VT_SIBMEM {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX SIBMEM_VT - SIBMEM dest, tile register src
struct OpCodeHandler_VEX_SIBMEM_VT {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VT - single tile register operand
struct OpCodeHandler_VEX_VT {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX VT_RT_HT - three tile registers
struct OpCodeHandler_VEX_VT_RT_HT {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// VEX Jump handlers (for MVEX compatibility/KNC)
// ============================================================================

// VEX K_Jb - K mask conditional short jump
struct OpCodeHandler_VEX_K_Jb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX K_Jz - K mask conditional near jump
struct OpCodeHandler_VEX_K_Jz {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Group8x64 - FPU-style group with 64 register handlers
struct OpCodeHandler_VEX_Group8x64 {
  bool has_modrm;
  std::array<HandlerEntry, 8> table_low;
  std::array<HandlerEntry, 64> table_high;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// VEX Options handler (don't read ModRM)
struct OpCodeHandler_VEX_Options_DontReadModRM {
  bool has_modrm;
  HandlerEntry handler_default;
  HandlerEntry handler_option;
  uint32_t decoder_options;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// EVEX Handlers
// ============================================================================

// EVEX VectorLength - dispatch based on EVEX.LL (128/256/512)
struct OpCodeHandler_EVEX_VectorLength {
  bool has_modrm;
  HandlerEntry handler_128;
  HandlerEntry handler_256;
  HandlerEntry handler_512;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VectorLength with embedded rounding
struct OpCodeHandler_EVEX_VectorLength_er {
  bool has_modrm;
  HandlerEntry handler_128;
  HandlerEntry handler_256;
  HandlerEntry handler_512;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX W handler - dispatch based on W bit
struct OpCodeHandler_EVEX_W {
  bool has_modrm;
  HandlerEntry handler_w0;
  HandlerEntry handler_w1;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX MandatoryPrefix2 - dispatch based on pp bits
struct OpCodeHandler_EVEX_MandatoryPrefix2 {
  bool has_modrm;
  std::array<HandlerEntry, 4> handlers;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX Group - dispatch based on reg field
struct OpCodeHandler_EVEX_Group {
  bool has_modrm;
  std::array<HandlerEntry, 8> handlers;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX RM - dispatch based on mod field (reg vs mem)
struct OpCodeHandler_EVEX_RM {
  bool has_modrm;
  HandlerEntry handler_reg;
  HandlerEntry handler_mem;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkW - V=dest{k}, W=src (most common EVEX pattern)
struct OpCodeHandler_EVEX_VkW {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkHW - V=dest{k}, H=vvvv, W=src (3 operand)
struct OpCodeHandler_EVEX_VkHW {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Register base_reg3;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkHW with embedded rounding
struct OpCodeHandler_EVEX_VkHW_er {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool only_sae;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkHWIb - V=dest{k}, H=vvvv, W=src, Ib=immediate
struct OpCodeHandler_EVEX_VkHWIb {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Register base_reg3;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkHWIb with embedded rounding
struct OpCodeHandler_EVEX_VkHWIb_er {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkWIb - V=dest{k}, W=src, Ib=immediate
struct OpCodeHandler_EVEX_VkWIb {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkWIb with embedded rounding
struct OpCodeHandler_EVEX_VkWIb_er {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkW with embedded rounding
struct OpCodeHandler_EVEX_VkW_er {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Code code;
  uint8_t tuple_type;
  bool only_sae;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkM - V=dest{k}, M=memory only
struct OpCodeHandler_EVEX_VkM {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VM - V=dest, M=memory (no mask)
struct OpCodeHandler_EVEX_VM {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX MV - M=dest, V=src
struct OpCodeHandler_EVEX_MV {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VW - V=dest, W=src (no mask)
struct OpCodeHandler_EVEX_VW {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VW with embedded rounding
struct OpCodeHandler_EVEX_VW_er {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX WV - W=dest, V=src (reversed)
struct OpCodeHandler_EVEX_WV {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX WkV - W=dest{k}, V=src
struct OpCodeHandler_EVEX_WkV {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX WkVIb - W=dest{k}, V=src, Ib
struct OpCodeHandler_EVEX_WkVIb {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX WkVIb with embedded rounding
struct OpCodeHandler_EVEX_WkVIb_er {
  bool has_modrm;
  Register base_reg1;
  Register base_reg2;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX WkHV - W=dest{k}, H=vvvv, V=src
struct OpCodeHandler_EVEX_WkHV {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VHW - V=dest, H=vvvv, W=src (no mask)
struct OpCodeHandler_EVEX_VHW {
  bool has_modrm;
  Register base_reg;
  Code code_r;
  Code code_m;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VHWIb - V=dest, H=vvvv, W=src, Ib
struct OpCodeHandler_EVEX_VHWIb {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VHM - V=dest, H=vvvv, M=memory
struct OpCodeHandler_EVEX_VHM {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkHM - V=dest{k}, H=vvvv, M=memory
struct OpCodeHandler_EVEX_VkHM {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VK - V=dest, K=mask register
struct OpCodeHandler_EVEX_VK {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkEv_REXW - V=dest{k}, Ev=r/m (REXW dependent)
struct OpCodeHandler_EVEX_VkEv_REXW {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX KR - K=dest, R=src register
struct OpCodeHandler_EVEX_KR {
  bool has_modrm;
  Register base_reg;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX KkHW - K=dest{k}, H=vvvv, W=src
struct OpCodeHandler_EVEX_KkHW {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX KkHWIb - K=dest{k}, H=vvvv, W=src, Ib
struct OpCodeHandler_EVEX_KkHWIb {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX KkHWIb with SAE
struct OpCodeHandler_EVEX_KkHWIb_sae {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX KkWIb - K=dest{k}, W=src, Ib
struct OpCodeHandler_EVEX_KkWIb {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX KP1HW - K=dest+1, H=vvvv, W=src
struct OpCodeHandler_EVEX_KP1HW {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX HkWIb - H=dest{k}, W=src, Ib
struct OpCodeHandler_EVEX_HkWIb {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX HWIb - H=dest, W=src, Ib
struct OpCodeHandler_EVEX_HWIb {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VSIB_k1 - VSIB addressing with k1 mask
struct OpCodeHandler_EVEX_VSIB_k1 {
  bool has_modrm;
  Register vsib_base;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VSIB_k1_VX - VSIB with vector dest
struct OpCodeHandler_EVEX_VSIB_k1_VX {
  bool has_modrm;
  Register vsib_base;
  Register base_reg;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX Vk_VSIB - V{k} with VSIB addressing
struct OpCodeHandler_EVEX_Vk_VSIB {
  bool has_modrm;
  Register base_reg;
  Register vsib_base;
  Code code;
  uint8_t tuple_type;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VkHW with embedded rounding and UR (unit rounding)
struct OpCodeHandler_EVEX_VkHW_er_ur {
  bool has_modrm;
  Register base_reg;
  Code code;
  uint8_t tuple_type;
  bool can_broadcast;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX Ed_V_Ib
struct OpCodeHandler_EVEX_Ed_V_Ib {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;
  uint8_t tuple_type32;
  uint8_t tuple_type64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX Ev_VX
struct OpCodeHandler_EVEX_Ev_VX {
  bool has_modrm;
  Code code32;
  Code code64;
  uint8_t tuple_type32;
  uint8_t tuple_type64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX Ev_VX_Ib
struct OpCodeHandler_EVEX_Ev_VX_Ib {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX VX_Ev
struct OpCodeHandler_EVEX_VX_Ev {
  bool has_modrm;
  Code code32;
  Code code64;
  uint8_t tuple_type32;
  uint8_t tuple_type64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX Gv_W_er
struct OpCodeHandler_EVEX_Gv_W_er {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;
  uint8_t tuple_type;
  bool only_sae;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX GvM_VX_Ib
struct OpCodeHandler_EVEX_GvM_VX_Ib {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;
  uint8_t tuple_type32;
  uint8_t tuple_type64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX V_H_Ev_er
struct OpCodeHandler_EVEX_V_H_Ev_er {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;
  uint8_t tuple_type32;
  uint8_t tuple_type64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// EVEX V_H_Ev_Ib
struct OpCodeHandler_EVEX_V_H_Ev_Ib {
  bool has_modrm;
  Register base_reg;
  Code code32;
  Code code64;
  uint8_t tuple_type32;
  uint8_t tuple_type64;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// ============================================================================
// MVEX handlers (Knights Corner)
// ============================================================================

// MVEX M - memory only operand
struct OpCodeHandler_MVEX_M {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX MV - M=dest, V=src
struct OpCodeHandler_MVEX_MV {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX VW - V=dest, W=src
struct OpCodeHandler_MVEX_VW {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX VWIb - V=dest, W=src, Ib=imm
struct OpCodeHandler_MVEX_VWIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX VHW - V=dest, H=vvvv, W=src
struct OpCodeHandler_MVEX_VHW {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX VHWIb - V=dest, H=vvvv, W=src, Ib=imm
struct OpCodeHandler_MVEX_VHWIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX HWIb - H=dest(vvvv), W=src, Ib=imm
struct OpCodeHandler_MVEX_HWIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX VKW - V=dest, K=mask, W=src
struct OpCodeHandler_MVEX_VKW {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX KHW - K=dest, H=vvvv, W=src
struct OpCodeHandler_MVEX_KHW {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX KHWIb - K=dest, H=vvvv, W=src, Ib=imm
struct OpCodeHandler_MVEX_KHWIb {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX VSIB - VSIB memory addressing
struct OpCodeHandler_MVEX_VSIB {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX VSIB_V - VSIB with V operand
struct OpCodeHandler_MVEX_VSIB_V {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX V_VSIB - V with VSIB addressing
struct OpCodeHandler_MVEX_V_VSIB {
  bool has_modrm;
  Code code;

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

// MVEX EH - dispatches based on eviction hint flag
struct OpCodeHandler_MVEX_EH {
  bool has_modrm;
  HandlerEntry handler_eh0;  // When eviction hint not set
  HandlerEntry handler_eh1;  // When eviction hint set

  static void decode( const OpCodeHandler* self_ptr, Decoder& decoder, Instruction& instruction );
};

} // namespace internal
} // namespace iced_x86

#endif // ICED_X86_INTERNAL_HANDLERS_HPP
