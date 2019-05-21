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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.ComponentModel;

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
		/// Upper case decorators, eg. {z}, {sae}, {rd-sae}
		/// </summary>
		[Obsolete("Use " + nameof(UpperCaseDecorators) + " instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool UpperCaseOther {
			get => UpperCaseDecorators;
			set => UpperCaseDecorators = value;
		}

		/// <summary>
		/// Upper case decorators, eg. {z}, {sae}, {rd-sae}
		/// </summary>
		public bool UpperCaseDecorators { get; set; }

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
		/// Add a space between the memory expression and the brackets, eg. "[ rax ]" vs "[rax]"
		/// </summary>
		public bool SpaceAfterMemoryBracket { get; set; }

		/// <summary>
		/// Add spaces between memory operand "+" and "-" operators, eg. "[rax + rcx]" vs "[rax+rcx]"
		/// </summary>
		public bool SpaceBetweenMemoryAddOperators { get; set; }

		/// <summary>
		/// Add spaces between memory operand "*" operator, eg. "[rax * 4]" vs "[rax*4]"
		/// </summary>
		public bool SpaceBetweenMemoryMulOperators { get; set; }

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
		public string? HexPrefix { get; set; }

		/// <summary>
		/// Hex number suffix or null/empty string, eg. "h"
		/// </summary>
		public string? HexSuffix { get; set; }

		/// <summary>
		/// Size of a digit group
		/// </summary>
		public int HexDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Decimal number prefix or null/empty string
		/// </summary>
		public string? DecimalPrefix { get; set; }

		/// <summary>
		/// Decimal number suffix or null/empty string
		/// </summary>
		public string? DecimalSuffix { get; set; }

		/// <summary>
		/// Size of a digit group
		/// </summary>
		public int DecimalDigitGroupSize { get; set; } = 3;

		/// <summary>
		/// Octal number prefix or null/empty string
		/// </summary>
		public string? OctalPrefix { get; set; }

		/// <summary>
		/// Octal number suffix or null/empty string
		/// </summary>
		public string? OctalSuffix { get; set; }

		/// <summary>
		/// Size of a digit group
		/// </summary>
		public int OctalDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Binary number prefix or null/empty string
		/// </summary>
		public string? BinaryPrefix { get; set; }

		/// <summary>
		/// Binary number suffix or null/empty string
		/// </summary>
		public string? BinarySuffix { get; set; }

		/// <summary>
		/// Size of a digit group
		/// </summary>
		public int BinaryDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Digit separator or null/empty string
		/// </summary>
		public string? DigitSeparator { get; set; }

		/// <summary>
		/// Use shortest possible hexadecimal/octal/binary numbers, eg. 0xA/0Ah instead of eg. 0x0000000A/0000000Ah.
		/// This option has no effect on branch targets, use <see cref="ShortBranchNumbers"/>.
		/// </summary>
		[Obsolete("Use " + nameof(LeadingZeroes) + " instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShortNumbers {
			get => !LeadingZeroes;
			set => LeadingZeroes = !value;
		}

		/// <summary>
		/// Add leading zeroes to hexadecimal/octal/binary numbers, eg. 0x0000000A/0000000Ah vs 0xA/0Ah.
		/// This option has no effect on branch targets, use <see cref="BranchLeadingZeroes"/>.
		/// </summary>
		public bool LeadingZeroes { get; set; }

		/// <summary>
		/// Use upper case hex digits
		/// </summary>
		public bool UpperCaseHex { get; set; } = true;

		/// <summary>
		/// Small hex numbers (-9 .. 9) are shown in decimal
		/// </summary>
		public bool SmallHexNumbersInDecimal { get; set; } = true;

		/// <summary>
		/// Add a leading zero to numbers if there's no prefix and the number starts with hex digits A-F, eg. Ah vs 0Ah
		/// </summary>
		public bool AddLeadingZeroToHexNumbers { get; set; } = true;

		/// <summary>
		/// Number base
		/// </summary>
		public NumberBase NumberBase {
			get => numberBase;
			set {
				if ((uint)value > (uint)NumberBase.Binary)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				numberBase = value;
			}
		}
		NumberBase numberBase = NumberBase.Hexadecimal;

		/// <summary>
		/// Don't add leading zeroes to branch offsets, eg. 'je 123h' vs 'je 00000123h'. Used by call near, call far, jmp near, jmp far, jcc, loop, loopcc, xbegin
		/// </summary>
		[Obsolete("Use " + nameof(BranchLeadingZeroes) + " instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShortBranchNumbers {
			get => !BranchLeadingZeroes;
			set => BranchLeadingZeroes = !value;
		}

		/// <summary>
		/// Add leading zeroes to branch offsets, eg. 'je 00000123h' vs 'je 123h'. Used by call near, call far, jmp near, jmp far, jcc, loop, loopcc, xbegin
		/// </summary>
		public bool BranchLeadingZeroes { get; set; } = true;

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
		/// Options that control if the memory size (eg. dword ptr) is shown or not.
		/// This is ignored by the GAS (AT&amp;T) formatter.
		/// </summary>
		public MemorySizeOptions MemorySizeOptions {
			get => memorySizeOptions;
			set {
				if ((uint)value > (uint)MemorySizeOptions.Never)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				memorySizeOptions = value;
			}
		}
		MemorySizeOptions memorySizeOptions = MemorySizeOptions.Default;

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

		/// <summary>
		/// Show the original value after the symbol name, eg. 'mov eax,[myfield (12345678)]' vs 'mov eax,[myfield]'
		/// </summary>
		public bool ShowSymbolAddress { get; set; }
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

	/// <summary>
	/// Memory size options used by the formatters
	/// </summary>
	public enum MemorySizeOptions {
		/// <summary>
		/// Show memory size if the assembler requires it, else don't show any
		/// </summary>
		Default,

		/// <summary>
		/// Always show the memory size, even if the assembler doesn't need it
		/// </summary>
		Always,

		/// <summary>
		/// Show memory size if a human can't figure out the size of the operand
		/// </summary>
		Minimum,

		/// <summary>
		/// Never show memory size
		/// </summary>
		Never,
	}
}
#endif
