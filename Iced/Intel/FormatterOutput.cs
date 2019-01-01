/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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
namespace Iced.Intel {
	/// <summary>
	/// Used by a <see cref="Formatter"/> to write all text
	/// </summary>
	public abstract class FormatterOutput {
		/// <summary>
		/// Writes text and text kind
		/// </summary>
		/// <param name="text">Text, can be an empty string</param>
		/// <param name="kind">Text kind. This value can be identical to the previous value passed to this method. It's
		/// the responsibility of the implementer to merge any such strings if needed.</param>
		public abstract void Write(string text, FormatterOutputTextKind kind);

		/// <summary>
		/// Called just before and just after formatting an operand
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="begin">true if we're about to format the operand, false if we've formatted it</param>
		public virtual void OnOperand(int operand, bool begin) { }

		internal void Write(in NumberFormatter numberFormatter, in NumberFormattingOptions numberOptions, ulong address, in SymbolResult symbol, bool showSymbolAddress) =>
			Write(numberFormatter, numberOptions, address, symbol, showSymbolAddress, true, false);

		internal void Write(in NumberFormatter numberFormatter, in NumberFormattingOptions numberOptions, ulong address, in SymbolResult symbol, bool showSymbolAddress, bool writeMinusIfSigned, bool spacesBetweenOp) {
			long displ = (long)(address - symbol.Address);
			if ((symbol.Flags & SymbolFlags.Signed) != 0) {
				if (writeMinusIfSigned)
					Write("-", FormatterOutputTextKind.Operator);
				displ = -displ;
			}
			Write(symbol.Text);
			if (displ != 0) {
				if (spacesBetweenOp)
					Write(" ", FormatterOutputTextKind.Text);
				if (displ < 0) {
					Write("-", FormatterOutputTextKind.Operator);
					displ = -displ;
				}
				else
					Write("+", FormatterOutputTextKind.Operator);
				if (spacesBetweenOp)
					Write(" ", FormatterOutputTextKind.Text);
				var s = numberFormatter.FormatUInt64(numberOptions, (ulong)displ, leadingZeroes: false);
				Write(s, FormatterOutputTextKind.Number);
			}
			if (showSymbolAddress) {
				Write(" ", FormatterOutputTextKind.Text);
				Write("(", FormatterOutputTextKind.Punctuation);
				string s;
				if (address <= ushort.MaxValue)
					s = numberFormatter.FormatUInt16(numberOptions, (ushort)address, leadingZeroes: true);
				else if (address <= uint.MaxValue)
					s = numberFormatter.FormatUInt32(numberOptions, (uint)address, leadingZeroes: true);
				else
					s = numberFormatter.FormatUInt64(numberOptions, address, leadingZeroes: true);
				Write(s, FormatterOutputTextKind.Number);
				Write(")", FormatterOutputTextKind.Punctuation);
			}
		}

		internal void Write(in TextInfo text) {
			var array = text.TextArray;
			if (array != null) {
				foreach (var part in array)
					Write(part.Text, part.Color);
			}
			else if (text.Text.Text is string s)
				Write(s, text.Text.Color);
		}
	}
}
#endif
