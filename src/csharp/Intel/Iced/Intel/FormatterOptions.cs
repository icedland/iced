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
	/// Formatter options
	/// </summary>
	public sealed class FormatterOptions {
		[Flags]
		enum Flags1 : uint {
			None							= 0,
			UppercasePrefixes				= 0x00000001,
			UppercaseMnemonics				= 0x00000002,
			UppercaseRegisters				= 0x00000004,
			UppercaseKeywords				= 0x00000008,
			UppercaseDecorators				= 0x00000010,
			UppercaseAll					= 0x00000020,
			SpaceAfterOperandSeparator		= 0x00000040,
			SpaceAfterMemoryBracket			= 0x00000080,
			SpaceBetweenMemoryAddOperators	= 0x00000100,
			SpaceBetweenMemoryMulOperators	= 0x00000200,
			ScaleBeforeIndex				= 0x00000400,
			AlwaysShowScale					= 0x00000800,
			AlwaysShowSegmentRegister		= 0x00001000,
			ShowZeroDisplacements			= 0x00002000,
			LeadingZeroes					= 0x00004000,
			UppercaseHex					= 0x00008000,
			SmallHexNumbersInDecimal		= 0x00010000,
			AddLeadingZeroToHexNumbers		= 0x00020000,
			BranchLeadingZeroes				= 0x00040000,
			SignedImmediateOperands			= 0x00080000,
			SignedMemoryDisplacements		= 0x00100000,
			DisplacementLeadingZeroes		= 0x00200000,
			RipRelativeAddresses			= 0x00400000,
			ShowBranchSize					= 0x00800000,
			UsePseudoOps					= 0x01000000,
			ShowSymbolAddress				= 0x02000000,
			GasNakedRegisters				= 0x04000000,
			GasShowMnemonicSizeSuffix		= 0x08000000,
			GasSpaceAfterMemoryOperandComma	= 0x10000000,
			MasmAddDsPrefix32				= 0x20000000,
			MasmSymbolDisplInBrackets		= 0x40000000,
			MasmDisplInBrackets				= 0x80000000,
		}

		[Flags]
		enum Flags2 : uint {
			None							= 0,
			NasmShowSignExtendedImmediateSize=0x00000001,
			PreferST0						= 0x00000002,
			ShowUselessPrefixes				= 0x00000004,
		}

		Flags1 flags1;
		Flags2 flags2;

		/// <summary>
		/// Constructor
		/// </summary>
		public FormatterOptions() {
			flags1 = Flags1.UppercaseHex | Flags1.SmallHexNumbersInDecimal |
				Flags1.AddLeadingZeroToHexNumbers | Flags1.BranchLeadingZeroes |
				Flags1.SignedMemoryDisplacements | Flags1.ShowBranchSize |
				Flags1.UsePseudoOps | Flags1.MasmAddDsPrefix32 |
				Flags1.MasmSymbolDisplInBrackets | Flags1.MasmDisplInBrackets;
			flags2 = Flags2.None;
		}

		/// <summary>
		/// Prefixes are upper cased
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>REP stosd</c>
		/// <br/>
		/// <see langword="false"/>: <c>rep stosd</c>
		/// </summary>
		public bool UppercasePrefixes {
			get => (flags1 & Flags1.UppercasePrefixes) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercasePrefixes;
				else
					flags1 &= ~Flags1.UppercasePrefixes;
			}
		}

		/// <summary>
		/// Mnemonics are upper cased
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>MOV rcx,rax</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov rcx,rax</c>
		/// </summary>
		public bool UppercaseMnemonics {
			get => (flags1 & Flags1.UppercaseMnemonics) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercaseMnemonics;
				else
					flags1 &= ~Flags1.UppercaseMnemonics;
			}
		}

		/// <summary>
		/// Registers are upper cased
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov RCX,[RAX+RDX*8]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov rcx,[rax+rdx*8]</c>
		/// </summary>
		public bool UppercaseRegisters {
			get => (flags1 & Flags1.UppercaseRegisters) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercaseRegisters;
				else
					flags1 &= ~Flags1.UppercaseRegisters;
			}
		}

		/// <summary>
		/// Keywords are upper cased (eg. <c>BYTE PTR</c>, <c>SHORT</c>)
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov BYTE PTR [rcx],12h</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov byte ptr [rcx],12h</c>
		/// </summary>
		public bool UppercaseKeywords {
			get => (flags1 & Flags1.UppercaseKeywords) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercaseKeywords;
				else
					flags1 &= ~Flags1.UppercaseKeywords;
			}
		}

		/// <summary>
		/// Upper case decorators, eg. <c>{z}</c>, <c>{sae}</c>, <c>{rd-sae}</c> (but not op mask registers: <c>{k1}</c>)
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]</c>
		/// <br/>
		/// <see langword="false"/>: <c>vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]</c>
		/// </summary>
		public bool UppercaseDecorators {
			get => (flags1 & Flags1.UppercaseDecorators) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercaseDecorators;
				else
					flags1 &= ~Flags1.UppercaseDecorators;
			}
		}

		/// <summary>
		/// Everything is upper cased, except numbers and their prefixes/suffixes
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>MOV EAX,GS:[RCX*4+0ffh]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,gs:[rcx*4+0ffh]</c>
		/// </summary>
		public bool UppercaseAll {
			get => (flags1 & Flags1.UppercaseAll) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercaseAll;
				else
					flags1 &= ~Flags1.UppercaseAll;
			}
		}

		/// <summary>
		/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
		/// At least one space or tab is always added between the mnemonic and the first operand.
		/// <br/>
		/// Default: <c>0</c>
		/// <br/>
		/// <c>0</c>: <c>mov•rcx,rbp</c>
		/// <br/>
		/// <c>8</c>: <c>mov•••••rcx,rbp</c>
		/// <br/>
		/// </summary>
		public int FirstOperandCharIndex { get; set; }

		/// <summary>
		/// Size of a tab character or &lt;= 0 to use spaces
		/// <br/>
		/// Default: <c>0</c>
		/// </summary>
		public int TabSize { get; set; }

		/// <summary>
		/// Add a space after the operand separator
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov rax, rcx</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov rax,rcx</c>
		/// </summary>
		public bool SpaceAfterOperandSeparator {
			get => (flags1 & Flags1.SpaceAfterOperandSeparator) != 0;
			set {
				if (value)
					flags1 |= Flags1.SpaceAfterOperandSeparator;
				else
					flags1 &= ~Flags1.SpaceAfterOperandSeparator;
			}
		}

		/// <summary>
		/// Add a space between the memory expression and the brackets
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[ rcx+rdx ]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[rcx+rdx]</c>
		/// </summary>
		public bool SpaceAfterMemoryBracket {
			get => (flags1 & Flags1.SpaceAfterMemoryBracket) != 0;
			set {
				if (value)
					flags1 |= Flags1.SpaceAfterMemoryBracket;
				else
					flags1 &= ~Flags1.SpaceAfterMemoryBracket;
			}
		}

		/// <summary>
		/// Add spaces between memory operand <c>+</c> and <c>-</c> operators
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[rcx + rdx*8 - 80h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[rcx+rdx*8-80h]</c>
		/// </summary>
		public bool SpaceBetweenMemoryAddOperators {
			get => (flags1 & Flags1.SpaceBetweenMemoryAddOperators) != 0;
			set {
				if (value)
					flags1 |= Flags1.SpaceBetweenMemoryAddOperators;
				else
					flags1 &= ~Flags1.SpaceBetweenMemoryAddOperators;
			}
		}

		/// <summary>
		/// Add spaces between memory operand <c>*</c> operator
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[rcx+rdx * 8-80h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[rcx+rdx*8-80h]</c>
		/// </summary>
		public bool SpaceBetweenMemoryMulOperators {
			get => (flags1 & Flags1.SpaceBetweenMemoryMulOperators) != 0;
			set {
				if (value)
					flags1 |= Flags1.SpaceBetweenMemoryMulOperators;
				else
					flags1 &= ~Flags1.SpaceBetweenMemoryMulOperators;
			}
		}

		/// <summary>
		/// Show memory operand scale value before the index register
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[8*rdx]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[rdx*8]</c>
		/// </summary>
		public bool ScaleBeforeIndex {
			get => (flags1 & Flags1.ScaleBeforeIndex) != 0;
			set {
				if (value)
					flags1 |= Flags1.ScaleBeforeIndex;
				else
					flags1 &= ~Flags1.ScaleBeforeIndex;
			}
		}

		/// <summary>
		/// Always show the scale value even if it's <c>*1</c>
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[rbx+rcx*1]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[rbx+rcx]</c>
		/// </summary>
		public bool AlwaysShowScale {
			get => (flags1 & Flags1.AlwaysShowScale) != 0;
			set {
				if (value)
					flags1 |= Flags1.AlwaysShowScale;
				else
					flags1 &= ~Flags1.AlwaysShowScale;
			}
		}

		/// <summary>
		/// Always show the effective segment register. If the option is <see langword="false"/>, only show the segment register if
		/// there's a segment override prefix.
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,ds:[ecx]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[ecx]</c>
		/// </summary>
		public bool AlwaysShowSegmentRegister {
			get => (flags1 & Flags1.AlwaysShowSegmentRegister) != 0;
			set {
				if (value)
					flags1 |= Flags1.AlwaysShowSegmentRegister;
				else
					flags1 &= ~Flags1.AlwaysShowSegmentRegister;
			}
		}

		/// <summary>
		/// Show zero displacements
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[rcx*2+0]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[rcx*2]</c>
		/// </summary>
		public bool ShowZeroDisplacements {
			get => (flags1 & Flags1.ShowZeroDisplacements) != 0;
			set {
				if (value)
					flags1 |= Flags1.ShowZeroDisplacements;
				else
					flags1 &= ~Flags1.ShowZeroDisplacements;
			}
		}

		/// <summary>
		/// Hex number prefix or <see langword="null"/>/empty string, eg. "0x"
		/// <br/>
		/// Default: <see langword="null"/> (masm/nasm/intel), <c>"0x"</c> (gas)
		/// </summary>
		public string? HexPrefix { get; set; }

		/// <summary>
		/// Hex number suffix or <see langword="null"/>/empty string, eg. "h"
		/// <br/>
		/// Default: <c>"h"</c> (masm/nasm/intel), <see langword="null"/> (gas)
		/// </summary>
		public string? HexSuffix { get; set; }

		/// <summary>
		/// Size of a digit group, see also <see cref="DigitSeparator"/>
		/// <br/>
		/// Default: <c>4</c>
		/// <br/>
		/// <c>0</c>: <c>0x12345678</c>
		/// <br/>
		/// <c>4</c>: <c>0x1234_5678</c>
		/// </summary>
		public int HexDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Decimal number prefix or <see langword="null"/>/empty string
		/// <br/>
		/// Default: <see langword="null"/>
		/// </summary>
		public string? DecimalPrefix { get; set; }

		/// <summary>
		/// Decimal number suffix or <see langword="null"/>/empty string
		/// <br/>
		/// Default: <see langword="null"/>
		/// </summary>
		public string? DecimalSuffix { get; set; }

		/// <summary>
		/// Size of a digit group, see also <see cref="DigitSeparator"/>
		/// <br/>
		/// Default: <c>3</c>
		/// <br/>
		/// <c>0</c>: <c>12345678</c>
		/// <br/>
		/// <c>3</c>: <c>12_345_678</c>
		/// </summary>
		public int DecimalDigitGroupSize { get; set; } = 3;

		/// <summary>
		/// Octal number prefix or <see langword="null"/>/empty string
		/// <br/>
		/// Default: <see langword="null"/> (masm/nasm/intel), <c>"0"</c> (gas)
		/// </summary>
		public string? OctalPrefix { get; set; }

		/// <summary>
		/// Octal number suffix or <see langword="null"/>/empty string
		/// <br/>
		/// Default: <c>"o"</c> (masm/nasm/intel), <see langword="null"/> (gas)
		/// </summary>
		public string? OctalSuffix { get; set; }

		/// <summary>
		/// Size of a digit group, see also <see cref="DigitSeparator"/>
		/// <br/>
		/// Default: <c>4</c>
		/// <br/>
		/// <c>0</c>: <c>12345670</c>
		/// <br/>
		/// <c>4</c>: <c>1234_5670</c>
		/// </summary>
		public int OctalDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Binary number prefix or <see langword="null"/>/empty string
		/// <br/>
		/// Default: <see langword="null"/> (masm/nasm/intel), <c>"0b"</c> (gas)
		/// </summary>
		public string? BinaryPrefix { get; set; }

		/// <summary>
		/// Binary number suffix or <see langword="null"/>/empty string
		/// <br/>
		/// Default: <c>"b"</c> (masm/nasm/intel), <see langword="null"/> (gas)
		/// </summary>
		public string? BinarySuffix { get; set; }

		/// <summary>
		/// Size of a digit group, see also <see cref="DigitSeparator"/>
		/// <br/>
		/// Default: <c>4</c>
		/// <br/>
		/// <c>0</c>: <c>11010111</c>
		/// <br/>
		/// <c>4</c>: <c>1101_0111</c>
		/// </summary>
		public int BinaryDigitGroupSize { get; set; } = 4;

		/// <summary>
		/// Digit separator or <see langword="null"/>/empty string. See also eg. <see cref="HexDigitGroupSize"/>.
		/// <br/>
		/// Default: <see langword="null"/>
		/// <br/>
		/// <c>""</c>: <c>0x12345678</c>
		/// <br/>
		/// <c>"_"</c>: <c>0x1234_5678</c>
		/// </summary>
		public string? DigitSeparator { get; set; }

		/// <summary>
		/// Add leading zeroes to hexadecimal/octal/binary numbers.
		/// This option has no effect on branch targets and displacements, use <see cref="BranchLeadingZeroes"/>
		/// and <see cref="DisplacementLeadingZeroes"/>.
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>0x0000000A</c>/<c>0000000Ah</c>
		/// <br/>
		/// <see langword="false"/>: <c>0xA</c>/<c>0Ah</c>
		/// </summary>
		public bool LeadingZeroes {
			get => (flags1 & Flags1.LeadingZeroes) != 0;
			set {
				if (value)
					flags1 |= Flags1.LeadingZeroes;
				else
					flags1 &= ~Flags1.LeadingZeroes;
			}
		}

		/// <summary>
		/// Use upper case hex digits
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>0xFF</c>
		/// <br/>
		/// <see langword="false"/>: <c>0xff</c>
		/// </summary>
		public bool UppercaseHex {
			get => (flags1 & Flags1.UppercaseHex) != 0;
			set {
				if (value)
					flags1 |= Flags1.UppercaseHex;
				else
					flags1 &= ~Flags1.UppercaseHex;
			}
		}

		/// <summary>
		/// Small hex numbers (-9 .. 9) are shown in decimal
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>9</c>
		/// <br/>
		/// <see langword="false"/>: <c>0x9</c>
		/// </summary>
		public bool SmallHexNumbersInDecimal {
			get => (flags1 & Flags1.SmallHexNumbersInDecimal) != 0;
			set {
				if (value)
					flags1 |= Flags1.SmallHexNumbersInDecimal;
				else
					flags1 &= ~Flags1.SmallHexNumbersInDecimal;
			}
		}

		/// <summary>
		/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits <c>A-F</c>
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>0FFh</c>
		/// <br/>
		/// <see langword="false"/>: <c>FFh</c>
		/// </summary>
		public bool AddLeadingZeroToHexNumbers {
			get => (flags1 & Flags1.AddLeadingZeroToHexNumbers) != 0;
			set {
				if (value)
					flags1 |= Flags1.AddLeadingZeroToHexNumbers;
				else
					flags1 &= ~Flags1.AddLeadingZeroToHexNumbers;
			}
		}

		/// <summary>
		/// Number base
		/// <br/>
		/// Default: <see cref="NumberBase.Hexadecimal"/>
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
		/// Add leading zeroes to branch offsets. Used by <c>CALL NEAR</c>, <c>CALL FAR</c>, <c>JMP NEAR</c>, <c>JMP FAR</c>, <c>Jcc</c>, <c>LOOP</c>, <c>LOOPcc</c>, <c>XBEGIN</c>
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>je 00000123h</c>
		/// <br/>
		/// <see langword="false"/>: <c>je 123h</c>
		/// </summary>
		public bool BranchLeadingZeroes {
			get => (flags1 & Flags1.BranchLeadingZeroes) != 0;
			set {
				if (value)
					flags1 |= Flags1.BranchLeadingZeroes;
				else
					flags1 &= ~Flags1.BranchLeadingZeroes;
			}
		}

		/// <summary>
		/// Show immediate operands as signed numbers
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,-1</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,FFFFFFFF</c>
		/// </summary>
		public bool SignedImmediateOperands {
			get => (flags1 & Flags1.SignedImmediateOperands) != 0;
			set {
				if (value)
					flags1 |= Flags1.SignedImmediateOperands;
				else
					flags1 &= ~Flags1.SignedImmediateOperands;
			}
		}

		/// <summary>
		/// Displacements are signed numbers
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov al,[eax-2000h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov al,[eax+0FFFFE000h]</c>
		/// </summary>
		public bool SignedMemoryDisplacements {
			get => (flags1 & Flags1.SignedMemoryDisplacements) != 0;
			set {
				if (value)
					flags1 |= Flags1.SignedMemoryDisplacements;
				else
					flags1 &= ~Flags1.SignedMemoryDisplacements;
			}
		}

		/// <summary>
		/// Add leading zeroes to displacements
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov al,[eax+00000012h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov al,[eax+12h]</c>
		/// </summary>
		public bool DisplacementLeadingZeroes {
			get => (flags1 & Flags1.DisplacementLeadingZeroes) != 0;
			set {
				if (value)
					flags1 |= Flags1.DisplacementLeadingZeroes;
				else
					flags1 &= ~Flags1.DisplacementLeadingZeroes;
			}
		}

		/// <summary>
		/// Add leading zeroes to displacements
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov al,[eax+00000012h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov al,[eax+12h]</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(DisplacementLeadingZeroes) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool SignExtendMemoryDisplacements {
			get => DisplacementLeadingZeroes;
			set => DisplacementLeadingZeroes = value;
		}

		/// <summary>
		/// Options that control if the memory size (eg. <c>DWORD PTR</c>) is shown or not.
		/// This is ignored by the gas (AT&amp;T) formatter.
		/// <br/>
		/// Default: <see cref="Intel.MemorySizeOptions.Default"/>
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
		/// Show <c>RIP+displ</c> or the virtual address
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[rip+12345678h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[1029384756AFBECDh]</c>
		/// </summary>
		public bool RipRelativeAddresses {
			get => (flags1 & Flags1.RipRelativeAddresses) != 0;
			set {
				if (value)
					flags1 |= Flags1.RipRelativeAddresses;
				else
					flags1 &= ~Flags1.RipRelativeAddresses;
			}
		}

		/// <summary>
		/// Show <c>NEAR</c>, <c>SHORT</c>, etc if it's a branch instruction
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>je short 1234h</c>
		/// <br/>
		/// <see langword="false"/>: <c>je 1234h</c>
		/// </summary>
		public bool ShowBranchSize {
			get => (flags1 & Flags1.ShowBranchSize) != 0;
			set {
				if (value)
					flags1 |= Flags1.ShowBranchSize;
				else
					flags1 &= ~Flags1.ShowBranchSize;
			}
		}

		/// <summary>
		/// Use pseudo instructions
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>vcmpnltsd xmm2,xmm6,xmm3</c>
		/// <br/>
		/// <see langword="false"/>: <c>vcmpsd xmm2,xmm6,xmm3,5</c>
		/// </summary>
		public bool UsePseudoOps {
			get => (flags1 & Flags1.UsePseudoOps) != 0;
			set {
				if (value)
					flags1 |= Flags1.UsePseudoOps;
				else
					flags1 &= ~Flags1.UsePseudoOps;
			}
		}

		/// <summary>
		/// Show the original value after the symbol name
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,[myfield (12345678)]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[myfield]</c>
		/// </summary>
		public bool ShowSymbolAddress {
			get => (flags1 & Flags1.ShowSymbolAddress) != 0;
			set {
				if (value)
					flags1 |= Flags1.ShowSymbolAddress;
				else
					flags1 &= ~Flags1.ShowSymbolAddress;
			}
		}

		/// <summary>
		/// (gas only): If <see langword="true"/>, the formatter doesn't add <c>%</c> to registers
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,ecx</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov %eax,%ecx</c>
		/// </summary>
		public bool GasNakedRegisters {
			get => (flags1 & Flags1.GasNakedRegisters) != 0;
			set {
				if (value)
					flags1 |= Flags1.GasNakedRegisters;
				else
					flags1 &= ~Flags1.GasNakedRegisters;
			}
		}

		/// <summary>
		/// (gas only): Shows the mnemonic size suffix even when not needed
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>movl %eax,%ecx</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov %eax,%ecx</c>
		/// </summary>
		public bool GasShowMnemonicSizeSuffix {
			get => (flags1 & Flags1.GasShowMnemonicSizeSuffix) != 0;
			set {
				if (value)
					flags1 |= Flags1.GasShowMnemonicSizeSuffix;
				else
					flags1 &= ~Flags1.GasShowMnemonicSizeSuffix;
			}
		}

		/// <summary>
		/// (gas only): Add a space after the comma if it's a memory operand
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>(%eax, %ecx, 2)</c>
		/// <br/>
		/// <see langword="false"/>: <c>(%eax,%ecx,2)</c>
		/// </summary>
		public bool GasSpaceAfterMemoryOperandComma {
			get => (flags1 & Flags1.GasSpaceAfterMemoryOperandComma) != 0;
			set {
				if (value)
					flags1 |= Flags1.GasSpaceAfterMemoryOperandComma;
				else
					flags1 &= ~Flags1.GasSpaceAfterMemoryOperandComma;
			}
		}

		/// <summary>
		/// (masm only): Add a <c>DS</c> segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov eax,ds:[12345678]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,[12345678]</c>
		/// </summary>
		public bool MasmAddDsPrefix32 {
			get => (flags1 & Flags1.MasmAddDsPrefix32) != 0;
			set {
				if (value)
					flags1 |= Flags1.MasmAddDsPrefix32;
				else
					flags1 &= ~Flags1.MasmAddDsPrefix32;
			}
		}

		/// <summary>
		/// (masm only): Show symbols in brackets
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>[ecx+symbol]</c> / <c>[symbol]</c>
		/// <br/>
		/// <see langword="false"/>: <c>symbol[ecx]</c> / <c>symbol</c>
		/// </summary>
		public bool MasmSymbolDisplInBrackets {
			get => (flags1 & Flags1.MasmSymbolDisplInBrackets) != 0;
			set {
				if (value)
					flags1 |= Flags1.MasmSymbolDisplInBrackets;
				else
					flags1 &= ~Flags1.MasmSymbolDisplInBrackets;
			}
		}

		/// <summary>
		/// (masm only): Show displacements in brackets
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>[ecx+1234h]</c>
		/// <br/>
		/// <see langword="false"/>: <c>1234h[ecx]</c>
		/// </summary>
		public bool MasmDisplInBrackets {
			get => (flags1 & Flags1.MasmDisplInBrackets) != 0;
			set {
				if (value)
					flags1 |= Flags1.MasmDisplInBrackets;
				else
					flags1 &= ~Flags1.MasmDisplInBrackets;
			}
		}

		/// <summary>
		/// (nasm only): Shows <c>BYTE</c>, <c>WORD</c>, <c>DWORD</c> or <c>QWORD</c> if it's a sign extended immediate operand value
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>or rcx,byte -1</c>
		/// <br/>
		/// <see langword="false"/>: <c>or rcx,-1</c>
		/// </summary>
		public bool NasmShowSignExtendedImmediateSize {
			get => (flags2 & Flags2.NasmShowSignExtendedImmediateSize) != 0;
			set {
				if (value)
					flags2 |= Flags2.NasmShowSignExtendedImmediateSize;
				else
					flags2 &= ~Flags2.NasmShowSignExtendedImmediateSize;
			}
		}

		/// <summary>
		/// Use <c>st(0)</c> instead of <c>st</c> if <c>st</c> can be used. Ignored by the nasm formatter.
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>fadd st(0),st(3)</c>
		/// <br/>
		/// <see langword="false"/>: <c>fadd st,st(3)</c>
		/// </summary>
		public bool PreferST0 {
			get => (flags2 & Flags2.PreferST0) != 0;
			set {
				if (value)
					flags2 |= Flags2.PreferST0;
				else
					flags2 &= ~Flags2.PreferST0;
			}
		}

		/// <summary>
		/// Show useless prefixes. If it has useless prefixes, it could be data and not code.
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>es rep add eax,ecx</c>
		/// <br/>
		/// <see langword="false"/>: <c>add eax,ecx</c>
		/// </summary>
		public bool ShowUselessPrefixes {
			get => (flags2 & Flags2.ShowUselessPrefixes) != 0;
			set {
				if (value)
					flags2 |= Flags2.ShowUselessPrefixes;
				else
					flags2 &= ~Flags2.ShowUselessPrefixes;
			}
		}

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JB</c> / <c>JC</c> / <c>JNAE</c>)
		/// <br/>
		/// Default: <c>JB</c>, <c>CMOVB</c>, <c>SETB</c>
		/// </summary>
		public CC_b CC_b {
			get => cc_b;
			set {
				if (value >= (CC_b)3)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_b = value;
			}
		}
		CC_b cc_b = CC_b.b;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JAE</c> / <c>JNB</c> / <c>JNC</c>)
		/// <br/>
		/// Default: <c>JAE</c>, <c>CMOVAE</c>, <c>SETAE</c>
		/// </summary>
		public CC_ae CC_ae {
			get => cc_ae;
			set {
				if (value >= (CC_ae)3)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_ae = value;
			}
		}
		CC_ae cc_ae = CC_ae.ae;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JE</c> / <c>JZ</c>)
		/// <br/>
		/// Default: <c>JE</c>, <c>CMOVE</c>, <c>SETE</c>, <c>LOOPE</c>, <c>REPE</c>
		/// </summary>
		public CC_e CC_e {
			get => cc_e;
			set {
				if (value >= (CC_e)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_e = value;
			}
		}
		CC_e cc_e = CC_e.e;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JNE</c> / <c>JNZ</c>)
		/// <br/>
		/// Default: <c>JNE</c>, <c>CMOVNE</c>, <c>SETNE</c>, <c>LOOPNE</c>, <c>REPNE</c>
		/// </summary>
		public CC_ne CC_ne {
			get => cc_ne;
			set {
				if (value >= (CC_ne)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_ne = value;
			}
		}
		CC_ne cc_ne = CC_ne.ne;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JBE</c> / <c>JNA</c>)
		/// <br/>
		/// Default: <c>JBE</c>, <c>CMOVBE</c>, <c>SETBE</c>
		/// </summary>
		public CC_be CC_be {
			get => cc_be;
			set {
				if (value >= (CC_be)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_be = value;
			}
		}
		CC_be cc_be = CC_be.be;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JA</c> / <c>JNBE</c>)
		/// <br/>
		/// Default: <c>JA</c>, <c>CMOVA</c>, <c>SETA</c>
		/// </summary>
		public CC_a CC_a {
			get => cc_a;
			set {
				if (value >= (CC_a)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_a = value;
			}
		}
		CC_a cc_a = CC_a.a;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JP</c> / <c>JPE</c>)
		/// <br/>
		/// Default: <c>JP</c>, <c>CMOVP</c>, <c>SETP</c>
		/// </summary>
		public CC_p CC_p {
			get => cc_p;
			set {
				if (value >= (CC_p)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_p = value;
			}
		}
		CC_p cc_p = CC_p.p;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JNP</c> / <c>JPO</c>)
		/// <br/>
		/// Default: <c>JNP</c>, <c>CMOVNP</c>, <c>SETNP</c>
		/// </summary>
		public CC_np CC_np {
			get => cc_np;
			set {
				if (value >= (CC_np)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_np = value;
			}
		}
		CC_np cc_np = CC_np.np;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JL</c> / <c>JNGE</c>)
		/// <br/>
		/// Default: <c>JL</c>, <c>CMOVL</c>, <c>SETL</c>
		/// </summary>
		public CC_l CC_l {
			get => cc_l;
			set {
				if (value >= (CC_l)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_l = value;
			}
		}
		CC_l cc_l = CC_l.l;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JGE</c> / <c>JNL</c>)
		/// <br/>
		/// Default: <c>JGE</c>, <c>CMOVGE</c>, <c>SETGE</c>
		/// </summary>
		public CC_ge CC_ge {
			get => cc_ge;
			set {
				if (value >= (CC_ge)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_ge = value;
			}
		}
		CC_ge cc_ge = CC_ge.ge;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JLE</c> / <c>JNG</c>)
		/// <br/>
		/// Default: <c>JLE</c>, <c>CMOVLE</c>, <c>SETLE</c>
		/// </summary>
		public CC_le CC_le {
			get => cc_le;
			set {
				if (value >= (CC_le)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_le = value;
			}
		}
		CC_le cc_le = CC_le.le;

		/// <summary>
		/// Mnemonic condition code selector (eg. <c>JG</c> / <c>JNLE</c>)
		/// <br/>
		/// Default: <c>JG</c>, <c>CMOVG</c>, <c>SETG</c>
		/// </summary>
		public CC_g CC_g {
			get => cc_g;
			set {
				if (value >= (CC_g)2)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				cc_g = value;
			}
		}
		CC_g cc_g = CC_g.g;

		/// <summary>
		/// Prefixes are upper cased
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>REP stosd</c>
		/// <br/>
		/// <see langword="false"/>: <c>rep stosd</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(UppercasePrefixes) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool UpperCasePrefixes {
			get => UppercasePrefixes;
			set => UppercasePrefixes = value;
		}

		/// <summary>
		/// Mnemonics are upper cased
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>MOV rcx,rax</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov rcx,rax</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(UppercaseMnemonics) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool UpperCaseMnemonics {
			get => UppercaseMnemonics;
			set => UppercaseMnemonics = value;
		}

		/// <summary>
		/// Registers are upper cased
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov RCX,[RAX+RDX*8]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov rcx,[rax+rdx*8]</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(UppercaseRegisters) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool UpperCaseRegisters {
			get => UppercaseRegisters;
			set => UppercaseRegisters = value;
		}

		/// <summary>
		/// Keywords are upper cased (eg. <c>BYTE PTR</c>, <c>SHORT</c>)
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>mov BYTE PTR [rcx],12h</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov byte ptr [rcx],12h</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(UppercaseKeywords) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool UpperCaseKeywords {
			get => UppercaseKeywords;
			set => UppercaseKeywords = value;
		}

		/// <summary>
		/// Upper case decorators, eg. <c>{z}</c>, <c>{sae}</c>, <c>{rd-sae}</c> (but not op mask registers: <c>{k1}</c>)
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]</c>
		/// <br/>
		/// <see langword="false"/>: <c>vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(UppercaseDecorators) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool UpperCaseDecorators {
			get => UppercaseDecorators;
			set => UppercaseDecorators = value;
		}

		/// <summary>
		/// Everything is upper cased, except numbers and their prefixes/suffixes
		/// <br/>
		/// Default: <see langword="false"/>
		/// <br/>
		/// <see langword="true"/>: <c>MOV EAX,GS:[RCX*4+0ffh]</c>
		/// <br/>
		/// <see langword="false"/>: <c>mov eax,gs:[rcx*4+0ffh]</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(UppercaseAll) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool UpperCaseAll {
			get => UppercaseAll;
			set => UppercaseAll = value;
		}

		/// <summary>
		/// Use upper case hex digits
		/// <br/>
		/// Default: <see langword="true"/>
		/// <br/>
		/// <see langword="true"/>: <c>0xFF</c>
		/// <br/>
		/// <see langword="false"/>: <c>0xff</c>
		/// </summary>
		[System.Obsolete("Use " + nameof(UppercaseHex) + " instead of this property", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public bool UpperCaseHex {
			get => UppercaseHex;
			set => UppercaseHex= value;
		}

#if GAS
		/// <summary>
		/// Creates GNU assembler (AT&amp;T) formatter options
		/// </summary>
		/// <returns></returns>
		public static FormatterOptions CreateGas() =>
			new FormatterOptions {
				HexPrefix = "0x",
				OctalPrefix = "0",
				BinaryPrefix = "0b",
			};
#endif

#if INTEL
		/// <summary>
		/// Creates Intel (XED) formatter options
		/// </summary>
		/// <returns></returns>
		public static FormatterOptions CreateIntel() =>
			new FormatterOptions {
				HexSuffix = "h",
				OctalSuffix = "o",
				BinarySuffix = "b",
			};
#endif

#if MASM
		/// <summary>
		/// Creates masm formatter options
		/// </summary>
		/// <returns></returns>
		public static FormatterOptions CreateMasm() =>
			new FormatterOptions {
				HexSuffix = "h",
				OctalSuffix = "o",
				BinarySuffix = "b",
			};
#endif

#if NASM
		/// <summary>
		/// Creates nasm formatter options
		/// </summary>
		/// <returns></returns>
		public static FormatterOptions CreateNasm() =>
			new FormatterOptions {
				HexSuffix = "h",
				OctalSuffix = "o",
				BinarySuffix = "b",
			};
#endif
	}

	// GENERATOR-BEGIN: NumberBase
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Number base</summary>
	public enum NumberBase {
		/// <summary>Hex numbers (base 16)</summary>
		Hexadecimal = 0,
		/// <summary>Decimal numbers (base 10)</summary>
		Decimal = 1,
		/// <summary>Octal numbers (base 8)</summary>
		Octal = 2,
		/// <summary>Binary numbers (base 2)</summary>
		Binary = 3,
	}
	// GENERATOR-END: NumberBase

	// GENERATOR-BEGIN: MemorySizeOptions
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Memory size options used by the formatters</summary>
	public enum MemorySizeOptions {
		/// <summary>Show memory size if the assembler requires it, else don&apos;t show anything</summary>
		Default = 0,
		/// <summary>Always show the memory size, even if the assembler doesn&apos;t need it</summary>
		Always = 1,
		/// <summary>Show memory size if a human can&apos;t figure out the size of the operand</summary>
		Minimum = 2,
		/// <summary>Never show memory size</summary>
		Never = 3,
	}
	// GENERATOR-END: MemorySizeOptions

	// GENERATOR-BEGIN: CC_b
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JB</c> / <c>JC</c> / <c>JNAE</c>)</summary>
	public enum CC_b : byte {
		/// <summary><c>JB</c>, <c>CMOVB</c>, <c>SETB</c></summary>
		b = 0,
		/// <summary><c>JC</c>, <c>CMOVC</c>, <c>SETC</c></summary>
		c = 1,
		/// <summary><c>JNAE</c>, <c>CMOVNAE</c>, <c>SETNAE</c></summary>
		nae = 2,
	}
	// GENERATOR-END: CC_b

	// GENERATOR-BEGIN: CC_ae
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JAE</c> / <c>JNB</c> / <c>JNC</c>)</summary>
	public enum CC_ae : byte {
		/// <summary><c>JAE</c>, <c>CMOVAE</c>, <c>SETAE</c></summary>
		ae = 0,
		/// <summary><c>JNB</c>, <c>CMOVNB</c>, <c>SETNB</c></summary>
		nb = 1,
		/// <summary><c>JNC</c>, <c>CMOVNC</c>, <c>SETNC</c></summary>
		nc = 2,
	}
	// GENERATOR-END: CC_ae

	// GENERATOR-BEGIN: CC_e
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JE</c> / <c>JZ</c>)</summary>
	public enum CC_e : byte {
		/// <summary><c>JE</c>, <c>CMOVE</c>, <c>SETE</c>, <c>LOOPE</c>, <c>REPE</c></summary>
		e = 0,
		/// <summary><c>JZ</c>, <c>CMOVZ</c>, <c>SETZ</c>, <c>LOOPZ</c>, <c>REPZ</c></summary>
		z = 1,
	}
	// GENERATOR-END: CC_e

	// GENERATOR-BEGIN: CC_ne
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JNE</c> / <c>JNZ</c>)</summary>
	public enum CC_ne : byte {
		/// <summary><c>JNE</c>, <c>CMOVNE</c>, <c>SETNE</c>, <c>LOOPNE</c>, <c>REPNE</c></summary>
		ne = 0,
		/// <summary><c>JNZ</c>, <c>CMOVNZ</c>, <c>SETNZ</c>, <c>LOOPNZ</c>, <c>REPNZ</c></summary>
		nz = 1,
	}
	// GENERATOR-END: CC_ne

	// GENERATOR-BEGIN: CC_be
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JBE</c> / <c>JNA</c>)</summary>
	public enum CC_be : byte {
		/// <summary><c>JBE</c>, <c>CMOVBE</c>, <c>SETBE</c></summary>
		be = 0,
		/// <summary><c>JNA</c>, <c>CMOVNA</c>, <c>SETNA</c></summary>
		na = 1,
	}
	// GENERATOR-END: CC_be

	// GENERATOR-BEGIN: CC_a
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JA</c> / <c>JNBE</c>)</summary>
	public enum CC_a : byte {
		/// <summary><c>JA</c>, <c>CMOVA</c>, <c>SETA</c></summary>
		a = 0,
		/// <summary><c>JNBE</c>, <c>CMOVNBE</c>, <c>SETNBE</c></summary>
		nbe = 1,
	}
	// GENERATOR-END: CC_a

	// GENERATOR-BEGIN: CC_p
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JP</c> / <c>JPE</c>)</summary>
	public enum CC_p : byte {
		/// <summary><c>JP</c>, <c>CMOVP</c>, <c>SETP</c></summary>
		p = 0,
		/// <summary><c>JPE</c>, <c>CMOVPE</c>, <c>SETPE</c></summary>
		pe = 1,
	}
	// GENERATOR-END: CC_p

	// GENERATOR-BEGIN: CC_np
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JNP</c> / <c>JPO</c>)</summary>
	public enum CC_np : byte {
		/// <summary><c>JNP</c>, <c>CMOVNP</c>, <c>SETNP</c></summary>
		np = 0,
		/// <summary><c>JPO</c>, <c>CMOVPO</c>, <c>SETPO</c></summary>
		po = 1,
	}
	// GENERATOR-END: CC_np

	// GENERATOR-BEGIN: CC_l
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JL</c> / <c>JNGE</c>)</summary>
	public enum CC_l : byte {
		/// <summary><c>JL</c>, <c>CMOVL</c>, <c>SETL</c></summary>
		l = 0,
		/// <summary><c>JNGE</c>, <c>CMOVNGE</c>, <c>SETNGE</c></summary>
		nge = 1,
	}
	// GENERATOR-END: CC_l

	// GENERATOR-BEGIN: CC_ge
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JGE</c> / <c>JNL</c>)</summary>
	public enum CC_ge : byte {
		/// <summary><c>JGE</c>, <c>CMOVGE</c>, <c>SETGE</c></summary>
		ge = 0,
		/// <summary><c>JNL</c>, <c>CMOVNL</c>, <c>SETNL</c></summary>
		nl = 1,
	}
	// GENERATOR-END: CC_ge

	// GENERATOR-BEGIN: CC_le
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JLE</c> / <c>JNG</c>)</summary>
	public enum CC_le : byte {
		/// <summary><c>JLE</c>, <c>CMOVLE</c>, <c>SETLE</c></summary>
		le = 0,
		/// <summary><c>JNG</c>, <c>CMOVNG</c>, <c>SETNG</c></summary>
		ng = 1,
	}
	// GENERATOR-END: CC_le

	// GENERATOR-BEGIN: CC_g
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// <summary>Mnemonic condition code selector (eg. <c>JG</c> / <c>JNLE</c>)</summary>
	public enum CC_g : byte {
		/// <summary><c>JG</c>, <c>CMOVG</c>, <c>SETG</c></summary>
		g = 0,
		/// <summary><c>JNLE</c>, <c>CMOVNLE</c>, <c>SETNLE</c></summary>
		nle = 1,
	}
	// GENERATOR-END: CC_g
}
#endif
