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
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	/// <summary>
	/// Can override options used by a <see cref="Formatter"/>
	/// </summary>
	public interface IFormatterOptionsProvider {
		/// <summary>
		/// Called by the formatter. The method can override any options before the formatter uses them.
		/// </summary>
		/// <param name="instruction">Instruction</param>
		/// <param name="operand">Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.</param>
		/// <param name="instructionOperand">Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.</param>
		/// <param name="options">Options. Only those options that will be used by the formatter are initialized.</param>
		/// <param name="numberOptions">Number formatting options</param>
		void GetOperandOptions(in Instruction instruction, int operand, int instructionOperand, ref FormatterOperandOptions options, ref NumberFormattingOptions numberOptions);
	}

	/// <summary>
	/// Operand options
	/// </summary>
	public struct FormatterOperandOptions {
		uint flags;

		[Flags]
		internal enum Flags : uint {
			None					= 0,
			NoBranchSize			= 0x00000001,
			RipRelativeAddresses	= 0x00000002,
			MemorySizeShift			= 30,
			MemorySizeMask			= 3U << (int)MemorySizeShift,
		}

		/// <summary>
		/// Show branch size (eg. <c>SHORT</c>, <c>NEAR PTR</c>)
		/// </summary>
		public bool BranchSize {
			readonly get => (flags & (uint)Flags.NoBranchSize) == 0;
			set {
				if (value)
					flags &= ~(uint)Flags.NoBranchSize;
				else
					flags |= (uint)Flags.NoBranchSize;
			}
		}

		/// <summary>
		/// If <see langword="true"/>, show <c>RIP</c> relative addresses as <c>[rip+12345678h]</c>, else show the linear address eg. <c>[1029384756AFBECDh]</c>
		/// </summary>
		public bool RipRelativeAddresses {
			readonly get => (flags & (uint)Flags.RipRelativeAddresses) != 0;
			set {
				if (value)
					flags |= (uint)Flags.RipRelativeAddresses;
				else
					flags &= ~(uint)Flags.RipRelativeAddresses;
			}
		}

		/// <summary>
		/// Memory size options
		/// </summary>
		public MemorySizeOptions MemorySizeOptions {
			readonly get => (MemorySizeOptions)(flags >> (int)Flags.MemorySizeShift);
			set => flags = (flags & ~(uint)Flags.MemorySizeMask) | ((uint)value << (int)Flags.MemorySizeShift);
		}

		internal FormatterOperandOptions(Flags flags) =>
			this.flags = (uint)flags;

		internal FormatterOperandOptions(MemorySizeOptions options) =>
			flags = (uint)options << (int)Flags.MemorySizeShift;
	}

	/// <summary>
	/// Gets initialized with the default options and can be overridden by a <see cref="IFormatterOptionsProvider"/>
	/// </summary>
	public struct NumberFormattingOptions {
		/// <summary>
		/// Digit separator or <see langword="null"/>/empty string to not use a digit separator
		/// </summary>
		public string? DigitSeparator;

		/// <summary>
		/// Number prefix or <see langword="null"/>/empty string
		/// </summary>
		public string? Prefix;

		/// <summary>
		/// Number suffix or <see langword="null"/>/empty string
		/// </summary>
		public string? Suffix;

		/// <summary>
		/// Size of a digit group or 0 to not use a digit separator
		/// </summary>
		public byte DigitGroupSize;

		/// <summary>
		/// Number base
		/// </summary>
		public NumberBase NumberBase {
			readonly get => (NumberBase)numberBaseByteValue;
			set => numberBaseByteValue = (byte)value;
		}
		byte numberBaseByteValue;

		/// <summary>
		/// Use upper case hex digits
		/// </summary>
		public bool UppercaseHex;

		/// <summary>
		/// Small hex numbers (-9 .. 9) are shown in decimal
		/// </summary>
		public bool SmallHexNumbersInDecimal;

		/// <summary>
		/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits <c>A-F</c>
		/// </summary>
		public bool AddLeadingZeroToHexNumbers;

		/// <summary>
		/// If <see langword="true"/>, add leading zeroes to numbers, eg. '1h' vs '00000001h'
		/// </summary>
		public bool LeadingZeroes;

		/// <summary>
		/// If <see langword="true"/>, the number is signed, and if <see langword="false"/> it's an unsigned number
		/// </summary>
		public bool SignedNumber;

		/// <summary>
		/// Add leading zeroes to displacements
		/// </summary>
		public bool DisplacementLeadingZeroes;

		/// <summary>
		/// Creates options used when formatting immediate values
		/// </summary>
		/// <param name="options">Formatter options to use</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NumberFormattingOptions CreateImmediate(FormatterOptions options) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			return CreateImmediateInternal(options);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static NumberFormattingOptions CreateImmediateInternal(FormatterOptions options) =>
			new NumberFormattingOptions(options, options.LeadingZeroes, options.SignedImmediateOperands, false);

		/// <summary>
		/// Creates options used when formatting displacements
		/// </summary>
		/// <param name="options">Formatter options to use</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NumberFormattingOptions CreateDisplacement(FormatterOptions options) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			return CreateDisplacementInternal(options);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static NumberFormattingOptions CreateDisplacementInternal(FormatterOptions options) =>
			new NumberFormattingOptions(options, options.LeadingZeroes, options.SignedMemoryDisplacements, options.DisplacementLeadingZeroes);

		/// <summary>
		/// Creates options used when formatting branch operands
		/// </summary>
		/// <param name="options">Formatter options to use</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static NumberFormattingOptions CreateBranch(FormatterOptions options) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			return CreateBranchInternal(options);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static NumberFormattingOptions CreateBranchInternal(FormatterOptions options) =>
			new NumberFormattingOptions(options, options.BranchLeadingZeroes, false, false);

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options">Formatter options to use</param>
		/// <param name="leadingZeroes">Add leading zeroes to numbers, eg. <c>1h</c> vs <c>00000001h</c></param>
		/// <param name="signedNumber">Signed numbers if <see langword="true"/>, and unsigned numbers if <see langword="false"/></param>
		/// <param name="displacementLeadingZeroes">Add leading zeroes to displacements</param>
		public NumberFormattingOptions(FormatterOptions options, bool leadingZeroes, bool signedNumber, bool displacementLeadingZeroes) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			LeadingZeroes = leadingZeroes;
			SignedNumber = signedNumber;
			DisplacementLeadingZeroes = displacementLeadingZeroes;
			numberBaseByteValue = (byte)options.NumberBase;
			DigitSeparator = options.DigitSeparator;
			UppercaseHex = options.UppercaseHex;
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
