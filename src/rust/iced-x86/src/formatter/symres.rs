/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

use super::super::*;
use super::enums::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;
#[cfg(not(feature = "std"))]
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
	) -> Option<SymbolResult>;
}

/// Contains a `&'a str` or a `String`
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub enum SymResString<'a> {
	/// Contains a `&'a str`
	Str(&'a str),
	/// Contains a `String`
	String(String),
}
impl<'a> Default for SymResString<'a> {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn default() -> Self {
		SymResString::Str("")
	}
}
impl<'a> SymResString<'a> {
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::wrong_self_convention))]
	pub(crate) fn to_owned<'b>(self) -> SymResString<'b> {
		match self {
			SymResString::Str(s) => SymResString::String(String::from(s)),
			SymResString::String(s) => SymResString::String(s),
		}
	}

	pub(crate) fn to_owned2<'b>(&self) -> SymResString<'b> {
		match self {
			&SymResString::Str(s) => SymResString::String(String::from(s)),
			&SymResString::String(ref s) => SymResString::String(s.clone()),
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(text: &'a str, color: FormatterTextKind) -> Self {
		Self { text: SymResString::Str(text), color }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_string(text: String, color: FormatterTextKind) -> Self {
		Self { text: SymResString::String(text), color }
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::wrong_self_convention))]
	pub(crate) fn to_owned<'b>(self) -> SymResTextPart<'b> {
		SymResTextPart { text: self.text.to_owned(), color: self.color }
	}

	pub(crate) fn to_owned2<'b>(&self) -> SymResTextPart<'b> {
		SymResTextPart { text: self.text.to_owned2(), color: self.color }
	}
}

/// Contains one or more [`SymResTextPart`]s (text and color)
///
/// [`SymResTextPart`]: struct.SymResTextPart.html
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub enum SymResTextInfo<'a> {
	/// Text and color
	Text(SymResTextPart<'a>),
	/// Text and color (vector)
	TextVec(&'a [SymResTextPart<'a>]),
}

impl<'a> SymResTextInfo<'a> {
	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(text: &'a str, color: FormatterTextKind) -> Self {
		SymResTextInfo::Text(SymResTextPart::new(text, color))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_string(text: String, color: FormatterTextKind) -> Self {
		SymResTextInfo::Text(SymResTextPart::with_string(text, color))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text(text: SymResTextPart<'a>) -> Self {
		SymResTextInfo::Text(text)
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_vec(text: &'a [SymResTextPart<'a>]) -> Self {
		SymResTextInfo::TextVec(text)
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::wrong_self_convention))]
	pub(crate) fn to_owned<'b>(self, vec: &'b mut Vec<SymResTextPart<'b>>) -> SymResTextInfo<'b> {
		match self {
			SymResTextInfo::Text(part) => SymResTextInfo::Text(part.to_owned()),
			SymResTextInfo::TextVec(parts) => {
				vec.clear();
				vec.extend(parts.iter().map(|a| a.to_owned2()));
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_str(address: u64, text: &'a str) -> Self {
		Self { address, text: SymResTextInfo::new(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `size`: Symbol size
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_str_size(address: u64, text: &'a str, size: MemorySize) -> Self {
		Self { address, text: SymResTextInfo::new(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: Some(size) }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `color`: Color
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_str_kind(address: u64, text: &'a str, color: FormatterTextKind) -> Self {
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_str_kind_flags(address: u64, text: &'a str, color: FormatterTextKind, flags: u32) -> Self {
		Self { address, text: SymResTextInfo::new(text, color), flags, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_string(address: u64, text: String) -> Self {
		Self { address, text: SymResTextInfo::with_string(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `size`: Symbol size
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_string_size(address: u64, text: String, size: MemorySize) -> Self {
		Self { address, text: SymResTextInfo::with_string(text, SymbolResult::DEFAULT_KIND), flags: SymbolFlags::NONE, symbol_size: Some(size) }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `color`: Color
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_string_kind(address: u64, text: String, color: FormatterTextKind) -> Self {
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_string_kind_flags(address: u64, text: String, color: FormatterTextKind, flags: u32) -> Self {
		Self { address, text: SymResTextInfo::with_string(text, color), flags, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text(address: u64, text: SymResTextInfo<'a>) -> Self {
		Self { address, text, flags: SymbolFlags::NONE, symbol_size: None }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	/// - `size`: Symbol size
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text_size(address: u64, text: SymResTextInfo<'a>, size: MemorySize) -> Self {
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text_flags(address: u64, text: SymResTextInfo<'a>, flags: u32) -> Self {
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
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text_flags_size(address: u64, text: SymResTextInfo<'a>, flags: u32, size: MemorySize) -> Self {
		Self { address, text, flags, symbol_size: Some(size) }
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::wrong_self_convention))]
	pub(crate) fn to_owned<'b>(self, vec: &'b mut Vec<SymResTextPart<'b>>) -> SymbolResult<'b> {
		SymbolResult { address: self.address, text: self.text.to_owned(vec), flags: self.flags, symbol_size: self.symbol_size }
	}
}
