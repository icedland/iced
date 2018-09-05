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
	public abstract class SymbolResolver {
		/// <summary>
		/// This method is called if you don't override any of the other virtual methods. It should return true if
		/// <paramref name="symbol"/> was updated. If false is returned, <paramref name="options"/> can be updated to
		/// override the default number formatting options.
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="code">Code value</param>
		/// <param name="address">Address</param>
		/// <param name="symbol">Updated with symbol information if this method returns true</param>
		/// <param name="options">Number formatting options if this method returns false</param>
		/// <returns></returns>
		protected virtual bool TryGetSymbol(int operand, Code code, ulong address, out SymbolResult symbol, ref NumberFormattingOptions options) {
			symbol = default;
			return false;
		}

		/// <summary>
		/// Updates <paramref name="symbol"/> with the symbol and returns true, else it returns false and can update <paramref name="options"/>
		/// to override the default number formatting options
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="code">Code value, eg. a call, jmp, jcc, xbegin</param>
		/// <param name="address">Address</param>
		/// <param name="symbol">Updated with symbol information if this method returns true</param>
		/// <param name="showBranchSize">true if branch info (short, near ptr, far ptr) should be shown</param>
		/// <param name="options">Number formatting options if this method returns false</param>
		/// <returns></returns>
		public virtual bool TryGetBranchSymbol(int operand, Code code, ulong address, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) =>
			TryGetSymbol(operand, code, address, out symbol, ref options);

		/// <summary>
		/// Returns true if <paramref name="symbol"/> was updated with a symbol. <paramref name="symbolSelector"/> can be set to default value
		/// if it should be formatted as a number. <paramref name="options"/> is used if this method returns false or if <paramref name="symbolSelector"/>
		/// has the default value.
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="code">Code value, eg. a far call or far jmp</param>
		/// <param name="selector">Selector/segment</param>
		/// <param name="address">Address</param>
		/// <param name="symbolSelector">Updated with the selector symbol or with the default value if it should be formatted as a number</param>
		/// <param name="symbol">Updated with symbol information if this method returns true</param>
		/// <param name="showBranchSize">true if branch info (short, near ptr, far ptr) should be shown</param>
		/// <param name="options">Number formatting options if this method returns false or if <paramref name="symbolSelector"/> has the default value</param>
		/// <returns></returns>
		public virtual bool TryGetFarBranchSymbol(int operand, Code code, ushort selector, uint address, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) {
			symbolSelector = default;
			return TryGetSymbol(operand, code, address, out symbol, ref options);
		}

		/// <summary>
		/// Gets a symbol and returns true. If it returns false, <paramref name="options"/> can be updated to override
		/// the default number formatting options.
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate value</param>
		/// <param name="symbol">Updated with symbol information if this method returns true</param>
		/// <param name="options">Number formatting options if this method returns false</param>
		/// <returns></returns>
		public virtual bool TryGetImmediateSymbol(int operand, Code code, ulong immediate, out SymbolResult symbol, ref NumberFormattingOptions options) =>
			TryGetSymbol(operand, code, immediate, out symbol, ref options);

		/// <summary>
		/// Gets a symbol and returns true. If it returns false, <paramref name="options"/> can be updated to override
		/// the default number formatting options.
		/// 
		/// This method gets called even if the memory operand has no displacement, eg. [eax].
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="code">Code value</param>
		/// <param name="displacement">Displacement. If it's RIP-relative addressing, this is the absolute address (rip/eip + displ)</param>
		/// <param name="ripRelativeAddresses">true to use RIP relative addresses</param>
		/// <param name="symbol">Updated with symbol information if this method returns true</param>
		/// <param name="options">Number formatting options if this method returns false</param>
		/// <returns></returns>
		public virtual bool TryGetDisplSymbol(int operand, Code code, ulong displacement, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) =>
			TryGetSymbol(operand, code, displacement, out symbol, ref options);
	}

	/// <summary>
	/// Gets initialized with the default options and can be overridden by a <see cref="SymbolResolver"/>
	/// </summary>
	public struct NumberFormattingOptions {
		/// <summary>
		/// Digit separator or null/empty string
		/// </summary>
		public string DigitSeparator;

		/// <summary>
		/// Number prefix or null/empty string
		/// </summary>
		public string Prefix;

		/// <summary>
		/// Number suffix or null/empty string
		/// </summary>
		public string Suffix;

		/// <summary>
		/// Size of a digit group. Used if <see cref="AddDigitSeparators"/> is true
		/// </summary>
		public byte DigitGroupSize;

		/// <summary>
		/// Number base
		/// </summary>
		public NumberBase NumberBase {
			get => (NumberBase)numberBaseByteValue;
			set => numberBaseByteValue = (byte)value;
		}
		internal byte numberBaseByteValue;

		/// <summary>
		/// Enables digit separators, see <see cref="DigitSeparator"/>, <see cref="DigitGroupSize"/>
		/// </summary>
		public bool AddDigitSeparators;

		/// <summary>
		/// Use upper case hex digits
		/// </summary>
		public bool UpperCaseHex;

		/// <summary>
		/// Small hex numbers (-9 .. 9) are shown in decimal
		/// </summary>
		public bool SmallHexNumbersInDecimal;

		/// <summary>
		/// Add a leading zero to numbers if there's no prefix and the number begins with hex digits A-F, eg. Ah vs 0Ah
		/// </summary>
		public bool AddLeadingZeroToHexNumbers;

		/// <summary>
		/// If true, use short numbers, and if false, add leading zeroes, eg. '1h' vs '00000001h'
		/// </summary>
		public bool ShortNumbers;

		/// <summary>
		/// If true, the number is signed, and if false it's an unsigned number
		/// </summary>
		public bool SignedNumber;

		/// <summary>
		/// Sign extend the number to the real size (16-bit, 32-bit, 64-bit), eg. 'mov al,[eax+12h]' vs 'mov al,[eax+00000012h]'
		/// </summary>
		public bool SignExtendImmediate;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options">Options</param>
		/// <param name="shortNumbers">If true, use short numbers, and if false, add leading zeroes, eg. '1h' vs '00000001h'</param>
		/// <param name="signedNumber">Signed numbers if true, and unsigned numbers if false</param>
		/// <param name="signExtendImmediate">Sign extend the number to the real size (16-bit, 32-bit, 64-bit), eg. 'mov al,[eax+12h]' vs 'mov al,[eax+00000012h]'</param>
		public NumberFormattingOptions(FormatterOptions options, bool shortNumbers, bool signedNumber, bool signExtendImmediate) {
			if (options == null)
				throw new ArgumentNullException(nameof(options));
			ShortNumbers = shortNumbers;
			SignedNumber = signedNumber;
			SignExtendImmediate = signExtendImmediate;
			numberBaseByteValue = (byte)options.NumberBase;
			DigitSeparator = options.DigitSeparator;
			AddDigitSeparators = options.AddDigitSeparators;
			UpperCaseHex = options.UpperCaseHex;
			SmallHexNumbersInDecimal = options.SmallHexNumbersInDecimal;
			AddLeadingZeroToHexNumbers = options.AddLeadingZeroToHexNumbers;
			int digitGroupSize;
			switch (options.NumberBase) {
			case NumberBase.Hexadecimal:
				Prefix = options.HexPrefix;
				Suffix = options.HexSuffix;
				digitGroupSize = options.HexDigitGroupSize;
				break;

			case NumberBase.Decimal:
				Prefix = options.DecimalPrefix;
				Suffix = options.DecimalSuffix;
				digitGroupSize = options.DecimalDigitGroupSize;
				break;

			case NumberBase.Octal:
				Prefix = options.OctalPrefix;
				Suffix = options.OctalSuffix;
				digitGroupSize = options.OctalDigitGroupSize;
				break;

			case NumberBase.Binary:
				Prefix = options.BinaryPrefix;
				Suffix = options.BinarySuffix;
				digitGroupSize = options.BinaryDigitGroupSize;
				break;

			default:
				throw new ArgumentException();
			}
			if (digitGroupSize < 0)
				DigitGroupSize = 0;
			else if (digitGroupSize > byte.MaxValue)
				DigitGroupSize = byte.MaxValue;
			else
				DigitGroupSize = (byte)digitGroupSize;
		}
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
		/// <param name="text">Symbol</param>
		/// <param name="color">Color</param>
		public SymbolResult(string text, FormatterOutputTextKind color) {
			Text = new TextInfo(text, color);
			Flags = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">Symbol</param>
		/// <param name="color">Color</param>
		/// <param name="flags">Symbol flags</param>
		public SymbolResult(string text, FormatterOutputTextKind color, SymbolFlags flags) {
			Text = new TextInfo(text, color);
			Flags = flags;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">Symbol</param>
		public SymbolResult(TextInfo text) {
			Text = text;
			Flags = 0;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text">Symbol</param>
		/// <param name="flags">Symbol flags</param>
		public SymbolResult(TextInfo text, SymbolFlags flags) {
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
