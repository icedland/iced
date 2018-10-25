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
namespace Iced.Intel {
	/// <summary>
	/// Formats decoded instructions
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
		public abstract void FormatMnemonic(ref Instruction instruction, FormatterOutput output);

		/// <summary>
		/// Gets the number of operands that will be formatted. A formatter can add and remove operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <returns></returns>
		public abstract int GetOperandCount(ref Instruction instruction);

		/// <summary>
		/// Formats an operand. This is a formatter operand and not necessarily a real instruction operand.
		/// A formatter can add and remove operands.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
		/// See <see cref="GetOperandCount(ref Instruction)"/></param>
		public abstract void FormatOperand(ref Instruction instruction, FormatterOutput output, int operand);

		/// <summary>
		/// Formats an operand separator
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public abstract void FormatOperandSeparator(ref Instruction instruction, FormatterOutput output);

		/// <summary>
		/// Formats all operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public abstract void FormatAllOperands(ref Instruction instruction, FormatterOutput output);

		/// <summary>
		/// Formats the whole instruction: prefixes, mnemonic, operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public abstract void Format(ref Instruction instruction, FormatterOutput output);

		/// <summary>
		/// Formats the whole instruction: prefixes, mnemonic, operands
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="output">Output</param>
		public void Format(Instruction instruction, FormatterOutput output) => Format(ref instruction, output);

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
		public string FormatInt8(sbyte value) => FormatInt8(value, NumberFormattingOptions.CreateImmediate(Options));

		/// <summary>
		/// Formats a <see cref="short"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatInt16(short value) => FormatInt16(value, NumberFormattingOptions.CreateImmediate(Options));

		/// <summary>
		/// Formats a <see cref="int"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatInt32(int value) => FormatInt32(value, NumberFormattingOptions.CreateImmediate(Options));

		/// <summary>
		/// Formats a <see cref="long"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatInt64(long value) => FormatInt64(value, NumberFormattingOptions.CreateImmediate(Options));

		/// <summary>
		/// Formats a <see cref="byte"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt8(byte value) => FormatUInt8(value, NumberFormattingOptions.CreateImmediate(Options));

		/// <summary>
		/// Formats a <see cref="ushort"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt16(ushort value) => FormatUInt16(value, NumberFormattingOptions.CreateImmediate(Options));

		/// <summary>
		/// Formats a <see cref="uint"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt32(uint value) => FormatUInt32(value, NumberFormattingOptions.CreateImmediate(Options));

		/// <summary>
		/// Formats a <see cref="ulong"/>
		/// </summary>
		/// <param name="value">Value</param>
		/// <returns></returns>
		public string FormatUInt64(ulong value) => FormatUInt64(value, NumberFormattingOptions.CreateImmediate(Options));

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
}
#endif
