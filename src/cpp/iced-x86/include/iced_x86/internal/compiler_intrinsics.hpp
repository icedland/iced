// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_INTERNAL_COMPILER_INTRINSICS_HPP
#define ICED_X86_INTERNAL_COMPILER_INTRINSICS_HPP

// ============================================================================
// Compiler detection and intrinsics for performance optimization
// ============================================================================

// Branch prediction hints
#if defined(__cplusplus) && __cplusplus >= 202002L
  // C++20 [[likely]] and [[unlikely]]
  #define ICED_LIKELY [[likely]]
  #define ICED_UNLIKELY [[unlikely]]
#elif defined(__GNUC__) || defined(__clang__)
  #define ICED_LIKELY
  #define ICED_UNLIKELY
  #define ICED_EXPECT_TRUE(x) __builtin_expect(!!(x), 1)
  #define ICED_EXPECT_FALSE(x) __builtin_expect(!!(x), 0)
#else
  #define ICED_LIKELY
  #define ICED_UNLIKELY
#endif

#ifndef ICED_EXPECT_TRUE
  #define ICED_EXPECT_TRUE(x) (x)
  #define ICED_EXPECT_FALSE(x) (x)
#endif

// Unreachable code hint - helps optimizer eliminate dead code paths
#if defined(_MSC_VER)
  #define ICED_UNREACHABLE() __assume(0)
#elif defined(__GNUC__) || defined(__clang__)
  #define ICED_UNREACHABLE() __builtin_unreachable()
#else
  #define ICED_UNREACHABLE() ((void)0)
#endif

// Force inline hint
#if defined(_MSC_VER)
  #define ICED_FORCE_INLINE __forceinline
#elif defined(__GNUC__) || defined(__clang__)
  #define ICED_FORCE_INLINE __attribute__((always_inline)) inline
#else
  #define ICED_FORCE_INLINE inline
#endif

// Prefetch hints
#if defined(__GNUC__) || defined(__clang__)
  #define ICED_PREFETCH_READ(addr) __builtin_prefetch((addr), 0, 3)
  #define ICED_PREFETCH_WRITE(addr) __builtin_prefetch((addr), 1, 3)
#elif defined(_MSC_VER)
  #include <intrin.h>
  #define ICED_PREFETCH_READ(addr) _mm_prefetch(reinterpret_cast<const char*>(addr), _MM_HINT_T0)
  #define ICED_PREFETCH_WRITE(addr) _mm_prefetch(reinterpret_cast<const char*>(addr), _MM_HINT_T0)
#else
  #define ICED_PREFETCH_READ(addr) ((void)0)
  #define ICED_PREFETCH_WRITE(addr) ((void)0)
#endif

// Assume hint (helps optimizer)
#if defined(_MSC_VER)
  #define ICED_ASSUME(cond) __assume(cond)
#elif defined(__clang__)
  #define ICED_ASSUME(cond) __builtin_assume(cond)
#elif defined(__GNUC__) && __GNUC__ >= 13
  #define ICED_ASSUME(cond) __attribute__((assume(cond)))
#else
  #define ICED_ASSUME(cond) ((void)0)
#endif

// Restrict pointer (no aliasing)
#if defined(_MSC_VER)
  #define ICED_RESTRICT __restrict
#elif defined(__GNUC__) || defined(__clang__)
  #define ICED_RESTRICT __restrict__
#else
  #define ICED_RESTRICT
#endif

// Cache line size for alignment
#ifndef ICED_CACHE_LINE_SIZE
  #define ICED_CACHE_LINE_SIZE 64
#endif

// Alignment hints
#if defined(_MSC_VER)
  #define ICED_ALIGNAS(x) __declspec(align(x))
#else
  #define ICED_ALIGNAS(x) alignas(x)
#endif

#endif // ICED_X86_INTERNAL_COMPILER_INTRINSICS_HPP
