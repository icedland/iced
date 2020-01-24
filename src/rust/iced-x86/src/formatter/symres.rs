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

/// Contains text and colors
#[derive(Debug, Default, Clone, Eq, PartialEq, Hash)]
pub struct TextPart {
	/// Text
	pub text: String,
	/// Color
	pub color: FormatterTextKind,
}

impl TextPart {
	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(text: String, color: FormatterTextKind) -> Self {
		Self { text, color }
	}
}

/// Contains one or more [`TextPart`]s (text and color)
///
/// [`TextPart`]: struct.TextPart.html
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub enum TextInfo {
	/// Text and color
	Text(TextPart),
	/// Text and color (vector)
	TextVec(Vec<TextPart>),
}

impl TextInfo {
	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `color`: Color
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new(text: String, color: FormatterTextKind) -> Self {
		TextInfo::Text(TextPart::new(text, color))
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: Text
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text(text: TextPart) -> Self {
		TextInfo::Text(text)
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `text`: All text parts
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text_vec(text: Vec<TextPart>) -> Self {
		TextInfo::TextVec(text)
	}
}

/// Created by a [`SymbolResolver`]
///
/// [`SymbolResolver`]: trait.SymbolResolver.html
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub struct SymbolResult {
	/// The address of the symbol
	pub address: u64,

	/// Contains the symbol
	pub text: TextInfo,

	/// Symbol flags, see [`SymbolFlags`]
	///
	/// [`SymbolFlags`]: struct.SymbolFlags.html
	pub flags: u32,

	/// Symbol size if [`has_symbol_size()`] is `true`
	///
	/// [`has_symbol_size()`]: #method.has_symbol_size
	pub symbol_size: MemorySize,
}

impl SymbolResult {
	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_string(address: u64, text: String) -> Self {
		Self { address, text: TextInfo::new(text, FormatterTextKind::Label), flags: SymbolFlags::NONE, symbol_size: MemorySize::Unknown }
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
		Self { address, text: TextInfo::new(text, FormatterTextKind::Label), flags: SymbolFlags::HAS_SYMBOL_SIZE, symbol_size: size }
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
		Self { address, text: TextInfo::new(text, color), flags: SymbolFlags::NONE, symbol_size: MemorySize::Unknown }
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
		Self { address, text: TextInfo::new(text, color), flags: flags & !SymbolFlags::HAS_SYMBOL_SIZE, symbol_size: MemorySize::Unknown }
	}

	/// Constructor
	///
	/// # Arguments
	///
	/// - `address`: The address of the symbol
	/// - `text`: Symbol
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_text(address: u64, text: TextInfo) -> Self {
		Self { address, text, flags: SymbolFlags::NONE, symbol_size: MemorySize::Unknown }
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
	pub fn with_text_size(address: u64, text: TextInfo, size: MemorySize) -> Self {
		Self { address, text, flags: SymbolFlags::HAS_SYMBOL_SIZE, symbol_size: size }
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
	pub fn with_text_flags(address: u64, text: TextInfo, flags: u32) -> Self {
		Self { address, text, flags: flags & !SymbolFlags::HAS_SYMBOL_SIZE, symbol_size: MemorySize::Unknown }
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
	pub fn with_text_flags_size(address: u64, text: TextInfo, flags: u32, size: MemorySize) -> Self {
		Self { address, text, flags: flags | SymbolFlags::HAS_SYMBOL_SIZE, symbol_size: size }
	}

	/// Checks whether [`symbol_size`] is valid
	///
	/// [`symbol_size`]: #structfield.symbol_size
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn has_symbol_size(&self) -> bool {
		(self.flags & SymbolFlags::HAS_SYMBOL_SIZE) != 0
	}
}
