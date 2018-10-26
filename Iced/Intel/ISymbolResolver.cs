/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;

namespace Iced.Intel {
	/// <summary>
	/// Used by a <see cref="Formatter"/> to resolve symbols. It can also override number formatting options
	/// </summary>
	public interface ISymbolResolver {
		/// <summary>
		/// Tries to resolve a symbol. It returns true if <paramref name="symbol"/> was updated.
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="instructionOperand">Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.</param>
		/// <param name="instruction">Instruction</param>
		/// <param name="address">Address</param>
		/// <param name="addressSize">Size of <paramref name="address"/> in bytes</param>
		/// <param name="symbol">Updated with symbol information if this method returns true</param>
		/// <returns></returns>
		bool TryGetSymbol(int operand, int instructionOperand, ref Instruction instruction, ulong address, int addressSize, out SymbolResult symbol);
	}

	/// <summary>
	/// Symbol flags
	/// </summary>
	[Flags]
	public enum SymbolFlags : uint {
		/// <summary>
		/// No bit is set
		/// </summary>
		None				= 0,

		/// <summary>
		/// If set it's the address of a symbol, else it's a symbol relative to the base and index registers (eg. a struct field offset)
		/// </summary>
		Address				= 0x00000001,

		/// <summary>
		/// It's a signed symbol and it should be displayed as '-symbol' or 'reg-symbol' instead of 'symbol' or 'reg+symbol'
		/// </summary>
		Signed				= 0x00000002,
	}

	/// <summary>
	/// The result of resolving a symbol
	/// </summary>
	public readonly struct SymbolResult {
		/// <summary>
		/// The address of the symbol
		/// </summary>
		public readonly ulong Address;

		/// <summary>
		/// Contains the symbol
		/// </summary>
		public readonly TextInfo Text;

		/// <summary>
		/// Symbol flags
		/// </summary>
		public readonly SymbolFlags Flags;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="address">The address of the symbol</param>
		/// <param name="text">Symbol</param>
		public SymbolResult(ulong address, string text) {
			Address = address;
			Text = new TextInfo(text, FormatterOutputTextKind.Label);
			Flags = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="address">The address of the symbol</param>
		/// <param name="text">Symbol</param>
		/// <param name="color">Color</param>
		public SymbolResult(ulong address, string text, FormatterOutputTextKind color) {
			Address = address;
			Text = new TextInfo(text, color);
			Flags = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="address">The address of the symbol</param>
		/// <param name="text">Symbol</param>
		/// <param name="color">Color</param>
		/// <param name="flags">Symbol flags</param>
		public SymbolResult(ulong address, string text, FormatterOutputTextKind color, SymbolFlags flags) {
			Address = address;
			Text = new TextInfo(text, color);
			Flags = flags;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="address">The address of the symbol</param>
		/// <param name="text">Symbol</param>
		public SymbolResult(ulong address, TextInfo text) {
			Address = address;
			Text = text;
			Flags = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="address">The address of the symbol</param>
		/// <param name="text">Symbol</param>
		/// <param name="flags">Symbol flags</param>
		public SymbolResult(ulong address, TextInfo text, SymbolFlags flags) {
			Address = address;
			Text = text;
			Flags = flags;
		}
	}

	/// <summary>
	/// Contains one or more <see cref="TextPart"/>s (text and color)
	/// </summary>
	public readonly struct TextInfo {
		/// <summary>
		/// true if this is the default instance
		/// </summary>
		public bool IsDefault => TextArray == null && Text.Text == null;

		/// <summary>
		/// The text and color unless <see cref="TextArray"/> is non-null
		/// </summary>
		public readonly TextPart Text;

		/// <summary>
		/// Text and color or null if <see cref="Text"/> should be used
		/// </summary>
		public readonly TextPart[] TextArray;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="color">Color</param>
		public TextInfo(string text, FormatterOutputTextKind color) {
			Text = new TextPart(text, color);
			TextArray = null;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">Text</param>
		public TextInfo(TextPart text) {
			Text = text;
			TextArray = null;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">All text parts</param>
		public TextInfo(TextPart[] text) {
			Text = default;
			TextArray = text;
		}
	}

	/// <summary>
	/// Contains text and colors
	/// </summary>
	public readonly struct TextPart {
		/// <summary>
		/// Text
		/// </summary>
		public readonly string Text;

		/// <summary>
		/// Color
		/// </summary>
		public readonly FormatterOutputTextKind Color;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="color">Color</param>
		public TextPart(string text, FormatterOutputTextKind color) {
			Text = text;
			Color = color;
		}
	}
}
#endif
