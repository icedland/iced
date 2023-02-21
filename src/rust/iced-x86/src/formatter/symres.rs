// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::*;
use alloc::string::String;
use alloc::vec::Vec;

/// Used by a [`Formatter`] to resolve symbols
///
/// [`Formatter`]: trait.Formatter.html
pub trait SymbolResolver {
	/// Tries to resolve a symbol
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
	/// - `address`: Address
	/// - `address_size`: Size of `address` in bytes (eg. 1, 2, 4 or 8)
	fn symbol(
		&mut self, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, address: u64, address_size: u32,
	) -> Option<SymbolResult<'_>>;
}

/// Contains a `&'a str` or a `String`
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub enum SymResString<'a> {
	/// Contains a `&'a str`
	Str(#[doc = "The str"] &'a str),
	/// Contains a `String`
	String(#[doc = "The string"] String),
}
impl Default for SymResString<'_> {
	#[must_use]
	#[inline]
	fn default() -> Self {
		SymResString::Str("")
	}
}
impl SymResString<'_> {
	pub(super) fn to_owned<'b>(self) -> SymResString<'b> {
		match self {
			SymResString::Str(s) => SymResString::String(String::from(s)),
			SymResString::String(s) => SymResString::String(s),
		}
	}

	pub(super) fn to_owned2<'b>(&self) -> SymResString<'b> {
		match self {
			&SymResString::Str(s) => SymResString::String(String::from(s)),
			SymResString::String(s) => SymResString::String(s.clone()),
		}
	}
}

/// Contains text and colors
#[derive(Debug, Default, Clone, Eq, PartialEq, Hash)]
pub struct SymResTextPart<'a> {
	/// Text
	pub text: SymResString<'a>,
	/// Color
	pub color: FormatterTextKind,
}

impl<'a> SymResTextPart<'a> {
	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[must_use]
	#[inline]
	pub const fn new(text: &'a str, color: FormatterTextKind) -> Self {
		Self { text: SymResString::Str(text), color }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[must_use]
	#[inline]
	pub const fn with_string(text: String, color: FormatterTextKind) -> Self {
		Self { text: SymResString::String(text), color }
	}

	pub(super) fn to_owned<'b>(self) -> SymResTextPart<'b> {
		SymResTextPart { text: self.text.to_owned(), color: self.color }
	}

	pub(super) fn to_owned2<'b>(&self) -> SymResTextPart<'b> {
		SymResTextPart { text: self.text.to_owned2(), color: self.color }
	}
}

/// Contains one or more [`SymResTextPart`]s (text and color)
///
/// [`SymResTextPart`]: struct.SymResTextPart.html
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub enum SymResTextInfo<'a> {
	/// Text and color
	Text(#[doc = "Text and color"] SymResTextPart<'a>),
	/// Text and color (vector)
	TextVec(#[doc = "Text and color"] &'a [SymResTextPart<'a>]),
}

impl<'a> SymResTextInfo<'a> {
	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[must_use]
	#[inline]
	pub const fn new(text: &'a str, color: FormatterTextKind) -> Self {
		SymResTextInfo::Text(SymResTextPart::new(text, color))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[must_use]
	#[inline]
	pub const fn with_string(text: String, color: FormatterTextKind) -> Self {
		SymResTextInfo::Text(SymResTextPart::with_string(text, color))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	#[must_use]
	#[inline]
	pub const fn with_text(text: SymResTextPart<'a>) -> Self {
		SymResTextInfo::Text(text)
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	#[must_use]
	#[inline]
	pub const fn with_vec(text: &'a [SymResTextPart<'a>]) -> Self {
		SymResTextInfo::TextVec(text)
	}

	pub(super) fn to_owned<'b>(self, vec: &'b mut Vec<SymResTextPart<'b>>) -> SymResTextInfo<'b> {
		match self {
			SymResTextInfo::Text(part) => SymResTextInfo::Text(part.to_owned()),
			SymResTextInfo::TextVec(parts) => {
				vec.clear();
				vec.extend(parts.iter().map(SymResTextPart::to_owned2));
				SymResTextInfo::TextVec(vec)
			}
		}
	}
}

/// Created by a [`SymbolResolver`]
///
/// [`SymbolResolver`]: trait.SymbolResolver.html
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub struct SymbolResult<'a> {
	/// The address of the symbol
	pub address: u64,

	/// Contains the symbol
	pub text: SymResTextInfo<'a>,

	/// Symbol flags, see [`SymbolFlags`]
	///
	/// [`SymbolFlags`]: struct.SymbolFlags.html
	pub flags: u32,

	/// Symbol size or `None`
	pub symbol_size: Option<MemorySize>,
}

impl<'a> SymbolResult<'a> {
	const DEFAULT_KIND: FormatterTextKind = FormatterTextKind::Label;

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	#[must_use]
	#[inline]
	pub const fn with_str(address: u64, text: &'a str) -> Self {
		Self { address, text: SymResTextInfo::new(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `size`: Symbol size
	#[must_use]
	#[inline]
	pub const fn with_str_size(address: u64, text: &'a str, size: MemorySize) -> Self {
		Self { address, text: SymResTextInfo::new(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: Some(size) }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `color`: Color
	#[must_use]
	#[inline]
	pub const fn with_str_kind(address: u64, text: &'a str, color: FormatterTextKind) -> Self {
		Self { address, text: SymResTextInfo::new(text, color), flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `color`: Color
	/// - `flags`: Symbol flags, see [`SymbolFlags`]
	///
	/// [`SymbolFlags`]: struct.SymbolFlags.html
	#[must_use]
	#[inline]
	pub const fn with_str_kind_flags(address: u64, text: &'a str, color: FormatterTextKind, flags: u32) -> Self {
		Self { address, text: SymResTextInfo::new(text, color), flags, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	#[must_use]
	#[inline]
	pub const fn with_string(address: u64, text: String) -> Self {
		Self { address, text: SymResTextInfo::with_string(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `size`: Symbol size
	#[must_use]
	#[inline]
	pub const fn with_string_size(address: u64, text: String, size: MemorySize) -> Self {
		Self { address, text: SymResTextInfo::with_string(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: Some(size) }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `color`: Color
	#[must_use]
	#[inline]
	pub const fn with_string_kind(address: u64, text: String, color: FormatterTextKind) -> Self {
		Self { address, text: SymResTextInfo::with_string(text, color), flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `color`: Color
	/// - `flags`: Symbol flags, see [`SymbolFlags`]
	///
	/// [`SymbolFlags`]: struct.SymbolFlags.html
	#[must_use]
	#[inline]
	pub const fn with_string_kind_flags(address: u64, text: String, color: FormatterTextKind, flags: u32) -> Self {
		Self { address, text: SymResTextInfo::with_string(text, color), flags, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	#[must_use]
	#[inline]
	pub const fn with_text(address: u64, text: SymResTextInfo<'a>) -> Self {
		Self { address, text, flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `size`: Symbol size
	#[must_use]
	#[inline]
	pub const fn with_text_size(address: u64, text: SymResTextInfo<'a>, size: MemorySize) -> Self {
		Self { address, text, flags: SymbolFlags::NONE, symbol_size: Some(size) }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `flags`: Symbol flags, see [`SymbolFlags`]
	///
	/// [`SymbolFlags`]: struct.SymbolFlags.html
	#[must_use]
	#[inline]
	pub const fn with_text_flags(address: u64, text: SymResTextInfo<'a>, flags: u32) -> Self {
		Self { address, text, flags, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `flags`: Symbol flags, see [`SymbolFlags`]
	/// - `size`: Symbol size
	///
	/// [`SymbolFlags`]: struct.SymbolFlags.html
	#[must_use]
	#[inline]
	pub const fn with_text_flags_size(address: u64, text: SymResTextInfo<'a>, flags: u32, size: MemorySize) -> Self {
		Self { address, text, flags, symbol_size: Some(size) }
	}

	pub(super) fn to_owned<'b>(self, vec: &'b mut Vec<SymResTextPart<'b>>) -> SymbolResult<'b> {
		SymbolResult { address: self.address, text: self.text.to_owned(vec), flags: self.flags, symbol_size: self.symbol_size }
	}
}
