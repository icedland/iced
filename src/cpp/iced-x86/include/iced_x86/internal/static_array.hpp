// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#pragma once
#ifndef ICED_X86_INTERNAL_STATIC_ARRAY_HPP
#define ICED_X86_INTERNAL_STATIC_ARRAY_HPP

#include <cstddef>
#include <cstdint>

namespace iced_x86 {
namespace internal {

/// @brief A simple non-owning array view, similar to std::span but without STL
/// Used for static handler tables that are never deallocated
template<typename T>
struct ArrayView {
	const T* data_ = nullptr;
	std::size_t size_ = 0;
	
	constexpr ArrayView() noexcept = default;
	constexpr ArrayView( const T* data, std::size_t size ) noexcept 
		: data_( data ), size_( size ) {}
	
	// Construct from C array
	template<std::size_t N>
	constexpr ArrayView( const T (&arr)[N] ) noexcept 
		: data_( arr ), size_( N ) {}
	
	[[nodiscard]] constexpr const T* data() const noexcept { return data_; }
	[[nodiscard]] constexpr std::size_t size() const noexcept { return size_; }
	[[nodiscard]] constexpr bool empty() const noexcept { return size_ == 0; }
	
	[[nodiscard]] constexpr const T& operator[]( std::size_t index ) const noexcept {
		return data_[index];
	}
	
	[[nodiscard]] constexpr const T* begin() const noexcept { return data_; }
	[[nodiscard]] constexpr const T* end() const noexcept { return data_ + size_; }
};

/// @brief Static storage for handler tables
/// Allocates from a static buffer, never freed (static lifetime)
/// This replaces std::vector for the decoder tables
template<typename T, std::size_t MaxSize>
class StaticStorage {
public:
	StaticStorage() noexcept = default;
	
	// Non-copyable, non-movable (singleton per type)
	StaticStorage( const StaticStorage& ) = delete;
	StaticStorage& operator=( const StaticStorage& ) = delete;
	
	/// @brief Allocate space for N elements, returns pointer to first element
	T* allocate( std::size_t count ) noexcept {
		if ( size_ + count > MaxSize ) {
			// Out of space - should not happen if MaxSize is correct
			return nullptr;
		}
		T* result = &storage_[size_];
		size_ += count;
		return result;
	}
	
	/// @brief Get current size (number of elements allocated)
	[[nodiscard]] std::size_t size() const noexcept { return size_; }
	
	/// @brief Get a view of elements from start to start+count
	[[nodiscard]] ArrayView<T> view( std::size_t start, std::size_t count ) const noexcept {
		return ArrayView<T>( &storage_[start], count );
	}
	
private:
	T storage_[MaxSize] = {};
	std::size_t size_ = 0;
};

/// @brief A growable array using static storage
/// Similar to std::vector but uses StaticStorage instead of heap
template<typename T>
class StaticVector {
public:
	StaticVector() noexcept = default;
	
	explicit StaticVector( T* storage, std::size_t capacity ) noexcept
		: data_( storage ), capacity_( capacity ), size_( 0 ) {}
	
	void push_back( const T& value ) noexcept {
		if ( size_ < capacity_ ) {
			data_[size_++] = value;
		}
	}
	
	void push_back( T&& value ) noexcept {
		if ( size_ < capacity_ ) {
			data_[size_++] = static_cast<T&&>( value );
		}
	}
	
	template<typename... Args>
	T& emplace_back( Args&&... args ) noexcept {
		T& ref = data_[size_++];
		ref = T( static_cast<Args&&>( args )... );
		return ref;
	}
	
	[[nodiscard]] T& back() noexcept { return data_[size_ - 1]; }
	[[nodiscard]] const T& back() const noexcept { return data_[size_ - 1]; }
	
	void pop_back() noexcept { 
		if ( size_ > 0 ) --size_; 
	}
	
	void clear() noexcept { size_ = 0; }
	void reserve( std::size_t ) noexcept { /* no-op, pre-allocated */ }
	
	[[nodiscard]] T* data() noexcept { return data_; }
	[[nodiscard]] const T* data() const noexcept { return data_; }
	[[nodiscard]] std::size_t size() const noexcept { return size_; }
	[[nodiscard]] std::size_t capacity() const noexcept { return capacity_; }
	[[nodiscard]] bool empty() const noexcept { return size_ == 0; }
	
	[[nodiscard]] T& operator[]( std::size_t index ) noexcept { return data_[index]; }
	[[nodiscard]] const T& operator[]( std::size_t index ) const noexcept { return data_[index]; }
	
	[[nodiscard]] T* begin() noexcept { return data_; }
	[[nodiscard]] T* end() noexcept { return data_ + size_; }
	[[nodiscard]] const T* begin() const noexcept { return data_; }
	[[nodiscard]] const T* end() const noexcept { return data_ + size_; }
	
	// Convert to ArrayView
	[[nodiscard]] ArrayView<T> view() const noexcept {
		return ArrayView<T>( data_, size_ );
	}
	
private:
	T* data_ = nullptr;
	std::size_t capacity_ = 0;
	std::size_t size_ = 0;
};

} // namespace internal
} // namespace iced_x86

#endif // ICED_X86_INTERNAL_STATIC_ARRAY_HPP
