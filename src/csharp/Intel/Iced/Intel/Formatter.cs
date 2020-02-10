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
using System;

namespace Iced.Intel {
	/// <summary>
	/// Formats instructions
	/// </summary>
	public abstract class Formatter {
		/// <summary>
		/// Gets the formatter options
		/// </summary>
		public abstract FormatterOptions Options { get; }

		/// <summary>
		/// Formats the mnemonic and any prefixes
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public void FormatMnemonic(in Instruction instruction, FormatterOutput output) =>
			FormatMnemonic(instruction, output, FormatMnemonicOptions.None);

		/// <summary>
		/// Formats the mnemonic and any prefixes
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		/// <param name="options">Options</param>
		public abstract void FormatMnemonic(in Instruction instruction, FormatterOutput output, FormatMnemonicOptions options);

		/// <summary>
		/// Gets the number of operands that will be formatted. A formatter can add and remove operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public abstract int GetOperandCount(in Instruction instruction);

#if INSTR_INFO
		/// <summary>
		/// Returns the operand access but only if it's an operand added by the formatter. If it's an
		/// operand that is part of <see cref="Instruction"/>, you should call eg. <see cref="InstructionInfoFactory.GetInfo(in Instruction)"/>.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
		/// See <see cref="GetOperandCount(in Instruction)"/></param>
		/// <param name="access">Updated with operand access if successful</param>
		/// <returns></returns>
		public abstract bool TryGetOpAccess(in Instruction instruction, int operand, out OpAccess access);
#endif

		/// <summary>
		/// Converts a formatter operand index to an instruction operand index. Returns -1 if it's an operand added by the formatter
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
		/// See <see cref="GetOperandCount(in Instruction)"/></param>
		/// <returns></returns>
		public abstract int GetInstructionOperand(in Instruction instruction, int operand);

		/// <summary>
		/// Converts an instruction operand index to a formatter operand index. Returns -1 if the instruction operand isn't used by the formatter
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="instructionOperand">Instruction operand</param>
		/// <returns></returns>
		public abstract int GetFormatterOperand(in Instruction instruction, int instructionOperand);

		/// <summary>
		/// Formats an operand. This is a formatter operand and not necessarily a real instruction operand.
		/// A formatter can add and remove operands.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
		/// See <see cref="GetOperandCount(in Instruction)"/></param>
		public abstract void FormatOperand(in Instruction instruction, FormatterOutput output, int operand);

		/// <summary>
		/// Formats an operand separator
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public abstract void FormatOperandSeparator(in Instruction instruction, FormatterOutput output);

		/// <summary>
		/// Formats all operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public abstract void FormatAllOperands(in Instruction instruction, FormatterOutput output);

		/// <summary>
		/// Formats the whole instruction: prefixes, mnemonic, operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public abstract void Format(in Instruction instruction, FormatterOutput output);

		/// <summary>
		/// Formats a register
		/// </summary>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public abstract string Format(Register register);

		/// <summary>
		/// Formats a <see cref="sbyte"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatInt8(sbyte value) => FormatInt8(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="short"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatInt16(short value) => FormatInt16(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="int"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatInt32(int value) => FormatInt32(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="long"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatInt64(long value) => FormatInt64(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="byte"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt8(byte value) => FormatUInt8(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="ushort"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt16(ushort value) => FormatUInt16(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="uint"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt32(uint value) => FormatUInt32(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="ulong"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt64(ulong value) => FormatUInt64(value, NumberFormattingOptions.CreateImmediateInternal(Options));

		/// <summary>
		/// Formats a <see cref="sbyte"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatInt8(sbyte value, in NumberFormattingOptions numberOptions);

		/// <summary>
		/// Formats a <see cref="short"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatInt16(short value, in NumberFormattingOptions numberOptions);

		/// <summary>
		/// Formats a <see cref="int"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatInt32(int value, in NumberFormattingOptions numberOptions);

		/// <summary>
		/// Formats a <see cref="long"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatInt64(long value, in NumberFormattingOptions numberOptions);

		/// <summary>
		/// Formats a <see cref="byte"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatUInt8(byte value, in NumberFormattingOptions numberOptions);

		/// <summary>
		/// Formats a <see cref="ushort"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatUInt16(ushort value, in NumberFormattingOptions numberOptions);

		/// <summary>
		/// Formats a <see cref="uint"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatUInt32(uint value, in NumberFormattingOptions numberOptions);

		/// <summary>
		/// Formats a <see cref="ulong"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="numberOptions">Options</param>
		/// <returns></returns>
		public abstract string FormatUInt64(ulong value, in NumberFormattingOptions numberOptions);
	}

	// GENERATOR-BEGIN: FormatMnemonicOptions
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Format mnemonic options</summary>
	[Flags]
	public enum FormatMnemonicOptions : uint {
		/// <summary>No option is set</summary>
		None = 0x00000000,
		/// <summary>Don&apos;t add any prefixes</summary>
		NoPrefixes = 0x00000001,
		/// <summary>Don&apos;t add the mnemonic</summary>
		NoMnemonic = 0x00000002,
	}
	// GENERATOR-END: FormatMnemonicOptions
}
#endif
