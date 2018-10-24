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
	/// Can override options used by a <see cref="Formatter"/>
	/// </summary>
	public abstract class FormatterOptionsProvider {
		/// <summary>
		/// Called by the formatter. The method can override any options before the formatter uses them.
		/// </summary>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="instruction">Instruction</param>
		/// <param name="options">Options. Only those options that will be used by the formatter are initialized.</param>
		/// <param name="numberOptions">Number formatting options</param>
		public virtual void GetOperandOptions(int operand, ref Instruction instruction, ref FormatterOperandOptions options, ref NumberFormattingOptions numberOptions) { }
	}

	/// <summary>
	/// Operand options
	/// </summary>
	[Flags]
	public enum FormatterOperandOptions : uint {
		/// <summary>
		/// No option is enabled
		/// </summary>
		None					= 0,

		/// <summary>
		/// Don't show branch size (short, near ptr)
		/// </summary>
		NoBranchSize			= 0x00000001,

		/// <summary>
		/// If set, show RIP relative addresses as '[rip+12345678h]', else show RIP relative addresses as '[1029384756AFBECDh]'
		/// </summary>
		RipRelativeAddresses	= 0x00000002,

		/// <summary>
		/// Bit position of <see cref="MemorySizeOptions"/> bits
		/// </summary>
		MemorySizeShift			= 30,

		/// <summary>
		/// MemorySizeXXX mask
		/// </summary>
		MemorySizeMask			= 3U << (int)MemorySizeShift,

		/// <summary>
		/// Show memory size if the assembler requires it, else don't show any
		/// </summary>
		MemorySizeDefault		= MemorySizeOptions.Default << (int)MemorySizeShift,

		/// <summary>
		/// Always show the memory size, even if the assembler doesn't need it
		/// </summary>
		MemorySizeAlways		= MemorySizeOptions.Always << (int)MemorySizeShift,

		/// <summary>
		/// Show memory size if a human can't figure out the size of the operand
		/// </summary>
		MemorySizeMinimum		= (uint)MemorySizeOptions.Minimum << (int)MemorySizeShift,

		/// <summary>
		/// Never show memory size
		/// </summary>
		MemorySizeNever			= (uint)MemorySizeOptions.Never << (int)MemorySizeShift,
	}

	/// <summary>
	/// Gets initialized with the default options and can be overridden by a <see cref="FormatterOptionsProvider"/>
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
		/// Size of a digit group
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
}
#endif
