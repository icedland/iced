// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_MEMORYSIZEINFO_HPP
#define ICED_X86_MEMORYSIZEINFO_HPP

#include "memory_size.hpp"
#include <cstdint>
#include <cstddef>

namespace iced_x86 {

/// @brief Contains information about a @ref MemorySize value
struct MemorySizeInfo {
	/// @brief The @ref MemorySize value
	MemorySize memory_size;
	/// @brief The element type if it's packed data or the type itself if it's not packed data
	MemorySize element_type;
	/// @brief Size in bytes of the memory location or 0 if it's not accessed or unknown
	uint16_t size;
	/// @brief Size in bytes of the packed element. If it's not a packed data type, it's equal to @ref size
	uint16_t element_size;
	/// @brief @c true if it's signed data (signed integer or a floating point value)
	bool is_signed;
	/// @brief @c true if it's a broadcast memory type
	bool is_broadcast;

	/// @brief @c true if this is a packed data type
	[[nodiscard]] constexpr bool is_packed() const noexcept {
		return element_size < size;
	}

	/// @brief Gets the number of elements in the packed data type or 1 if it's not packed data
	[[nodiscard]] constexpr std::size_t element_count() const noexcept {
		// element_size can be 0 so we don't divide by it if es == s
		if (element_size == size) {
			return 1;
		}
		return static_cast<std::size_t>(size) / static_cast<std::size_t>(element_size);
	}
};

/// @brief Memory size extension methods
namespace memory_size_ext {

/// @brief Gets the @ref MemorySizeInfo for a @ref MemorySize value
/// @param memory_size The memory size
/// @return The memory size info
[[nodiscard]] const MemorySizeInfo& get_info(MemorySize memory_size) noexcept;

/// @brief Gets the size in bytes of the memory location or 0 if it's not accessed or unknown
/// @param memory_size The memory size
/// @return Size in bytes
[[nodiscard]] inline std::size_t get_size(MemorySize memory_size) noexcept {
	return get_info(memory_size).size;
}

/// @brief Gets the size in bytes of the packed element. If it's not a packed data type, it's equal to @ref get_size
/// @param memory_size The memory size
/// @return Element size in bytes
[[nodiscard]] inline std::size_t get_element_size(MemorySize memory_size) noexcept {
	return get_info(memory_size).element_size;
}

/// @brief Gets the element type if it's packed data or the type itself if it's not packed data
/// @param memory_size The memory size
/// @return The element type
[[nodiscard]] inline MemorySize get_element_type(MemorySize memory_size) noexcept {
	return get_info(memory_size).element_type;
}

/// @brief Gets the element type info if it's packed data or the type itself if it's not packed data
/// @param memory_size The memory size
/// @return The element type info
[[nodiscard]] inline const MemorySizeInfo& get_element_type_info(MemorySize memory_size) noexcept {
	return get_info(get_info(memory_size).element_type);
}

/// @brief Checks if it's signed data (signed integer or a floating point value)
/// @param memory_size The memory size
/// @return @c true if signed
[[nodiscard]] inline bool is_signed(MemorySize memory_size) noexcept {
	return get_info(memory_size).is_signed;
}

/// @brief Checks if this is a packed data type
/// @param memory_size The memory size
/// @return @c true if packed
[[nodiscard]] inline bool is_packed(MemorySize memory_size) noexcept {
	return get_info(memory_size).is_packed();
}

/// @brief Gets the number of elements in the packed data type or 1 if it's not packed data
/// @param memory_size The memory size
/// @return Element count
[[nodiscard]] inline std::size_t get_element_count(MemorySize memory_size) noexcept {
	return get_info(memory_size).element_count();
}

/// @brief Checks if it's a broadcast memory type
/// @param memory_size The memory size
/// @return @c true if broadcast
[[nodiscard]] inline bool is_broadcast(MemorySize memory_size) noexcept {
	return get_info(memory_size).is_broadcast;
}

} // namespace memory_size_ext

} // namespace iced_x86

#endif // ICED_X86_MEMORYSIZEINFO_HPP
