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
	/// Formatter options
	/// </summary>
	public abstract class FormatterOptions {
		/// <summary>
		/// Prefixes are upper cased
		/// </summary>
		public bool UpperCasePrefixes { get; set; }

		/// <summary>
		/// Mnemonics are upper cased
		/// </summary>
		public bool UpperCaseMnemonics { get; set; }

		/// <summary>
		/// Registers are upper cased
		/// </summary>
		public bool UpperCaseRegisters { get; set; }

		/// <summary>
		/// Keywords are upper cased (eg. BYTE PTR, SHORT)
		/// </summary>
		public bool UpperCaseKeywords { get; set; }

		/// <summary>
		/// Upper case other stuff, eg. {z}, {sae}, {rd-sae}
		/// </summary>
		public bool UpperCaseOther { get; set; }

		/// <summary>
		/// Everything is upper cased, except numbers and their prefixes/suffixes
		/// </summary>
		public bool UpperCaseAll { get; set; }

		/// <summary>
		/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
		/// At least one space or tab is always added betewen the mnemonic and the first operand.
		/// </summary>
		public int FirstOperandCharIndex { get; set; }

		/// <summary>
		/// Size of a tab character or &lt;= 0 to use spaces
		/// </summary>
		public int TabSize { get; set; }

		/// <summary>
		/// Add a space after the operand separator, eg. "rax, rcx" vs "rax,rcx"
		/// </summary>
		public bool SpaceAfterOperandSeparator { get; set; }

		/// <summary>
		/// Add a space after the open memory bracket, eg. "[ rax]" vs "[rax]"
		/// </summary>
		public bool SpaceAfterMemoryOpenBracket { get; set; }

		/// <summary>
		/// Add a space before the close memory bracket, eg. "[rax ]" vs "[rax]"
		/// </summary>
		public bool SpaceBeforeMemoryCloseBracket { get; set; }

		/// <summary>
		/// Add spaces between memory operand "+" and "-" operators, eg. "[rax + rcx]" vs "[rax+rcx]"
		/// </summary>
		public bool SpacesBetweenMemoryAddOperators { get; set; }

		/// <summary>
		/// Add spaces between memory operand "*" operator, eg. "[rax * 4]" vs "[rax*4]"
		/// </summary>
		public bool SpacesBetweenMemoryMulOperators { get; set; }

		/// <summary>
		/// Show memory operand scale value before the index register, eg. "[4*rax]" vs "[rax*4]"
		/// </summary>
		public bool ScaleBeforeIndex { get; set; }

		/// <summary>
		/// Always show the scale value even if it's *1, eg. "[rax+rcx*1]" vs "[rax+rcx]"
		/// </summary>
		public bool AlwaysShowScale { get; set; }

		/// <summary>
		/// Always show the effective segment register. If the option is false, only show the segment register if
		/// there's a segment override prefix. Eg. "ds:[rax]" vs "[rax]"
		/// </summary>
		public bool AlwaysShowSegmentRegister { get; set; }

		/// <summary>
		/// Show zero displacements, eg. '[rcx*2+0]' vs '[rcx*2]'
		/// </summary>
		public bool ShowZeroDisplacements { get; set; }

		/// <summary>
		/// Hex number prefix or null/empty string, eg. "0x"
		/// </summary>
		public string HexPrefix { get; set; }

		/// <summary>
		/// Hex number suffix or null/empty string, eg. "h"
		/// </summary>
		public string HexSuffix { get; set; }

		/// <summary>
		/// Size of a digit group. Used if <see cref="AddDigitSeparators"/> is true
		/// </summary>
		public int HexDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Decimal number prefix or null/empty string
		/// </summary>
		public string DecimalPrefix { get; set; }

		/// <summary>
		/// Decimal number suffix or null/empty string
		/// </summary>
		public string DecimalSuffix { get; set; }

		/// <summary>
		/// Size of a digit group. Used if <see cref="AddDigitSeparators"/> is true
		/// </summary>
		public int DecimalDigitGroupSize { get; set; } = 3;

		/// <summary>
		/// Octal number prefix or null/empty string
		/// </summary>
		public string OctalPrefix { get; set; }

		/// <summary>
		/// Octal number suffix or null/empty string
		/// </summary>
		public string OctalSuffix { get; set; }

		/// <summary>
		/// Size of a digit group. Used if <see cref="AddDigitSeparators"/> is true
		/// </summary>
		public int OctalDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Binary number prefix or null/empty string
		/// </summary>
		public string BinaryPrefix { get; set; }

		/// <summary>
		/// Binary number suffix or null/empty string
		/// </summary>
		public string BinarySuffix { get; set; }

		/// <summary>
		/// Size of a digit group. Used if <see cref="AddDigitSeparators"/> is true
		/// </summary>
		public int BinaryDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Digit separator or null/empty string
		/// </summary>
		public string DigitSeparator { get; set; } = "_";

		/// <summary>
		/// Enables digit separators, see <see cref="DigitSeparator"/>, <see cref="HexDigitGroupSize"/>, <see cref="DecimalDigitGroupSize"/>, <see cref="OctalDigitGroupSize"/>, <see cref="BinaryDigitGroupSize"/>
		/// </summary>
		public bool AddDigitSeparators { get; set; }

		/// <summary>
		/// Use shortest possible hexadecimal/octal/binary numbers, eg. 0xA/0Ah instead of eg. 0x0000000A/0000000Ah.
		/// This option has no effect on branch targets, use <see cref="ShortBranchNumbers"/>.
		/// </summary>
		public bool ShortNumbers { get; set; } = true;

		/// <summary>
		/// Use upper case hex digits
		/// </summary>
		public bool UpperCaseHex { get; set; } = true;

		/// <summary>
		/// Small hex numbers (-9 .. 9) are shown in decimal
		/// </summary>
		public bool SmallHexNumbersInDecimal { get; set; } = true;

		/// <summary>
		/// Add a leading zero to numbers if there's no prefix and the number begins with hex digits A-F, eg. Ah vs 0Ah
		/// </summary>
		public bool AddLeadingZeroToHexNumbers { get; set; } = true;

		/// <summary>
		/// Number base
		/// </summary>
		public NumberBase NumberBase {
			get => (NumberBase)numberBase;
			set {
				if (value < 0 || value > NumberBase.Binary)
					throw new ArgumentOutOfRangeException(nameof(value));
				numberBase = (byte)value;
			}
		}
		byte numberBase = (byte)NumberBase.Hexadecimal;

		/// <summary>
		/// Don't add leading zeroes to branch offsets, eg. 'je 123h' vs 'je 00000123h'. Used by call near, call far, jmp near, jmp far, jcc, loop, loopcc, xbegin
		/// </summary>
		public bool ShortBranchNumbers { get; set; }

		/// <summary>
		/// Show immediate operands as signed numbers, eg. 'mov eax,FFFFFFFF' vs 'mov eax,-1'
		/// </summary>
		public bool SignedImmediateOperands { get; set; }

		/// <summary>
		/// Displacements are signed numbers, eg. 'mov al,[eax-2000h]' vs 'mov al,[eax+0FFFFE000h]'
		/// </summary>
		public bool SignedMemoryDisplacements { get; set; } = true;

		/// <summary>
		/// Sign extend memory displacements to the address size (16-bit, 32-bit, 64-bit), eg. 'mov al,[eax+12h]' vs 'mov al,[eax+00000012h]'
		/// </summary>
		public bool SignExtendMemoryDisplacements { get; set; }

		/// <summary>
		/// Always show the memory size, even when not needed eg. "mov al,[rax]" vs "mov al,byte ptr [rax]".
		/// This is ignored by the GAS (AT&amp;T) formatter.
		/// </summary>
		public bool AlwaysShowMemorySize { get; set; }

		/// <summary>
		/// true to show RIP relative addresses as '[rip+12345678h]', false to show RIP relative addresses as '[1029384756AFBECDh]'
		/// </summary>
		public bool RipRelativeAddresses { get; set; }

		/// <summary>
		/// Shows near, short, etc if it's a branch instruction, eg. 'je short 1234h' vs 'je 1234h'
		/// </summary>
		public bool ShowBranchSize { get; set; } = true;

		/// <summary>
		/// Use pseudo instructions, eg. vcmpngesd vs vcmpsd+imm8
		/// </summary>
		public bool UsePseudoOps { get; set; } = true;
	}

	/// <summary>
	/// Number base
	/// </summary>
	public enum NumberBase {
		/// <summary>
		/// Hex numbers (base 16)
		/// </summary>
		Hexadecimal,

		/// <summary>
		/// Decimal numbers (base 10)
		/// </summary>
		Decimal,

		/// <summary>
		/// Octal numbers (base 8)
		/// </summary>
		Octal,

		/// <summary>
		/// Binary numbers (base 2)
		/// </summary>
		Binary,
	}
}
#endif
