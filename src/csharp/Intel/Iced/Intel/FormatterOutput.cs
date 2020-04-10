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

#if GAS || INTEL || MASM || NASM
namespace Iced.Intel {
	/// <summary>
	/// Used by a <see cref="Formatter"/> to write all text
	/// </summary>
	public abstract class FormatterOutput {
		/// <summary>
		/// Writes text and text kind
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="kind">Text kind</param>
		public abstract void Write(string text, FormatterTextKind kind);

		/// <summary>
		/// Writes a prefix
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="text">Prefix text</param>
		/// <param name="prefix">Prefix</param>
		public virtual void WritePrefix(in Instruction instruction, string text, PrefixKind prefix) => Write(text, FormatterTextKind.Prefix);

		/// <summary>
		/// Writes a mnemonic (see <see cref="Instruction.Mnemonic"/>)
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="text">Mnemonic text</param>
		public virtual void WriteMnemonic(in Instruction instruction, string text) => Write(text, FormatterTextKind.Mnemonic);

		/// <summary>
		/// Writes a number
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="instructionOperand">Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.</param>
		/// <param name="text">Number text</param>
		/// <param name="value">Value</param>
		/// <param name="numberKind">Number kind</param>
		/// <param name="kind">Text kind</param>
		public virtual void WriteNumber(in Instruction instruction, int operand, int instructionOperand, string text, ulong value, NumberKind numberKind, FormatterTextKind kind) => Write(text, kind);

		/// <summary>
		/// Writes a decorator
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="instructionOperand">Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.</param>
		/// <param name="text">Decorator text</param>
		/// <param name="decorator">Decorator</param>
		public virtual void WriteDecorator(in Instruction instruction, int operand, int instructionOperand, string text, DecoratorKind decorator) => Write(text, FormatterTextKind.Decorator);

		/// <summary>
		/// Writes a register
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="instructionOperand">Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.</param>
		/// <param name="text">Register text</param>
		/// <param name="register">Register</param>
		public virtual void WriteRegister(in Instruction instruction, int operand, int instructionOperand, string text, Register register) => Write(text, FormatterTextKind.Register);

		/// <summary>
		/// Writes a symbol
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="instructionOperand">Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.</param>
		/// <param name="address">Address</param>
		/// <param name="symbol">Symbol</param>
		public virtual void WriteSymbol(in Instruction instruction, int operand, int instructionOperand, ulong address, in SymbolResult symbol) {
			var text = symbol.Text;
			var array = text.TextArray;
			if (!(array is null)) {
				foreach (var part in array)
					Write(part.Text, part.Color);
			}
			else if (text.Text.Text is string s)
				Write(s, text.Text.Color);
		}

		internal void Write(in Instruction instruction, int operand, int instructionOperand, FormatterOptions options, in NumberFormatter numberFormatter, in NumberFormattingOptions numberOptions, ulong address, in SymbolResult symbol, bool showSymbolAddress) =>
			Write(instruction, operand, instructionOperand, options, numberFormatter, numberOptions, address, symbol, showSymbolAddress, true, false);

		internal void Write(in Instruction instruction, int operand, int instructionOperand, FormatterOptions options, in NumberFormatter numberFormatter, in NumberFormattingOptions numberOptions, ulong address, in SymbolResult symbol, bool showSymbolAddress, bool writeMinusIfSigned, bool spacesBetweenOp) {
			long displ = (long)(address - symbol.Address);
			if ((symbol.Flags & SymbolFlags.Signed) != 0) {
				if (writeMinusIfSigned)
					Write("-", FormatterTextKind.Operator);
				displ = -displ;
			}
			WriteSymbol(instruction, operand, instructionOperand, address, symbol);
			NumberKind numberKind;
			if (displ != 0) {
				if (spacesBetweenOp)
					Write(" ", FormatterTextKind.Text);
				ulong origDispl = (ulong)displ;
				if (displ < 0) {
					Write("-", FormatterTextKind.Operator);
					displ = -displ;
					if (displ <= sbyte.MaxValue + 1)
						numberKind = NumberKind.Int8;
					else if (displ <= short.MaxValue + 1)
						numberKind = NumberKind.Int16;
					else if (displ <= (long)int.MaxValue + 1)
						numberKind = NumberKind.Int32;
					else
						numberKind = NumberKind.Int64;
				}
				else {
					Write("+", FormatterTextKind.Operator);
					if (displ <= sbyte.MaxValue)
						numberKind = NumberKind.Int8;
					else if (displ <= short.MaxValue)
						numberKind = NumberKind.Int16;
					else if (displ <= int.MaxValue)
						numberKind = NumberKind.Int32;
					else
						numberKind = NumberKind.Int64;
				}
				if (spacesBetweenOp)
					Write(" ", FormatterTextKind.Text);
				var s = numberFormatter.FormatUInt64(options, numberOptions, (ulong)displ, leadingZeroes: false);
				WriteNumber(instruction, operand, instructionOperand, s, origDispl, numberKind, FormatterTextKind.Number);
			}
			if (showSymbolAddress) {
				Write(" ", FormatterTextKind.Text);
				Write("(", FormatterTextKind.Punctuation);
				string s;
				if (address <= ushort.MaxValue) {
					s = numberFormatter.FormatUInt16(options, numberOptions, (ushort)address, leadingZeroes: true);
					numberKind = NumberKind.UInt16;
				}
				else if (address <= uint.MaxValue) {
					s = numberFormatter.FormatUInt32(options, numberOptions, (uint)address, leadingZeroes: true);
					numberKind = NumberKind.UInt32;
				}
				else {
					s = numberFormatter.FormatUInt64(options, numberOptions, address, leadingZeroes: true);
					numberKind = NumberKind.UInt64;
				}
				WriteNumber(instruction, operand, instructionOperand, s, address, numberKind, FormatterTextKind.Number);
				Write(")", FormatterTextKind.Punctuation);
			}
		}
	}

	// GENERATOR-BEGIN: PrefixKind
	// ⚠️This was generated by GENERATOR!🦹‍♂️
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	/// <summary>Prefix</summary>
	public enum PrefixKind {
		ES = 0,
		CS = 1,
		SS = 2,
		DS = 3,
		FS = 4,
		GS = 5,
		Lock = 6,
		Rep = 7,
		Repe = 8,
		Repne = 9,
		OperandSize = 10,
		AddressSize = 11,
		HintNotTaken = 12,
		HintTaken = 13,
		Bnd = 14,
		Notrack = 15,
		Xacquire = 16,
		Xrelease = 17,
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	// GENERATOR-END: PrefixKind

	// GENERATOR-BEGIN: DecoratorKind
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Decorator</summary>
	public enum DecoratorKind {
		/// <summary>Broadcast decorator, eg. <c>{1to4}</c></summary>
		Broadcast = 0,
		/// <summary>Rounding control, eg. <c>{rd-sae}</c></summary>
		RoundingControl = 1,
		/// <summary>Suppress all exceptions: <c>{sae}</c></summary>
		SuppressAllExceptions = 2,
		/// <summary>Zeroing masking: <c>{z}</c></summary>
		ZeroingMasking = 3,
	}
	// GENERATOR-END: DecoratorKind

	// GENERATOR-BEGIN: NumberKind
	// ⚠️This was generated by GENERATOR!🦹‍♂️
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	/// <summary>Number kind</summary>
	public enum NumberKind {
		Int8 = 0,
		UInt8 = 1,
		Int16 = 2,
		UInt16 = 3,
		Int32 = 4,
		UInt32 = 5,
		Int64 = 6,
		UInt64 = 7,
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	// GENERATOR-END: NumberKind
}
#endif
