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
		/// MemorySizeXXX mask, use <see cref="FormatterOperandOptionsExtensions.WithMemorySize(FormatterOperandOptions, MemorySizeOptions)"/> to change this value
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
	/// Extension methods
	/// </summary>
	public static class FormatterOperandOptionsExtensions {
		/// <summary>
		/// Returns new options with a new <see cref="MemorySizeOptions"/> value
		/// </summary>
		/// <param name="self">Operand options</param>
		/// <param name="options">Memory size options</param>
		/// <returns></returns>
		public static FormatterOperandOptions WithMemorySize(this FormatterOperandOptions self, MemorySizeOptions options) =>
			(self & ~FormatterOperandOptions.MemorySizeMask) | (FormatterOperandOptions)((uint)options << (int)FormatterOperandOptions.MemorySizeShift);
	}

	/// <summary>
	/// Gets initialized with the default options and can be overridden by a <see cref="IFormatterOptionsProvider"/>
	/// </summary>
	public struct NumberFormattingOptions {
		/// <summary>
		/// Digit separator or null/empty string
		/// </summary>
		public string? DigitSeparator;

		/// <summary>
		/// Number prefix or null/empty string
		/// </summary>
		public string? Prefix;

		/// <summary>
		/// Number suffix or null/empty string
		/// </summary>
		public string? Suffix;

		/// <summary>
		/// Size of a digit group
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
		/// If true, add leading zeroes to numbers, eg. '1h' vs '00000001h'
		/// </summary>
		public bool LeadingZeroes;

		/// <summary>
		/// If true, the number is signed, and if false it's an unsigned number
		/// </summary>
		public bool SignedNumber;

		/// <summary>
		/// Sign extend the number to the real size (16-bit, 32-bit, 64-bit), eg. 'mov al,[eax+12h]' vs 'mov al,[eax+00000012h]'
		/// </summary>
		public bool SignExtendImmediate;

		/// <summary>
		/// Creates options used when formatting immediate values
		/// </summary>
		/// <param name="options">Options</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static NumberFormattingOptions CreateImmediate(FormatterOptions options) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			return CreateImmediateInternal(options);
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal static NumberFormattingOptions CreateImmediateInternal(FormatterOptions options) =>
			new NumberFormattingOptions(options, options.LeadingZeroes, options.SignedImmediateOperands, false);

		/// <summary>
		/// Creates options used when formatting displacements
		/// </summary>
		/// <param name="options">Options</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static NumberFormattingOptions CreateDisplacement(FormatterOptions options) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			return CreateDisplacementInternal(options);
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal static NumberFormattingOptions CreateDisplacementInternal(FormatterOptions options) =>
			new NumberFormattingOptions(options, options.LeadingZeroes, options.SignedMemoryDisplacements, options.SignExtendMemoryDisplacements);

		/// <summary>
		/// Creates options used when formatting branch operands
		/// </summary>
		/// <param name="options">Options</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static NumberFormattingOptions CreateBranch(FormatterOptions options) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			return CreateBranchInternal(options);
		}

		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal static NumberFormattingOptions CreateBranchInternal(FormatterOptions options) =>
			new NumberFormattingOptions(options, options.BranchLeadingZeroes, false, false);

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options">Options</param>
		/// <param name="leadingZeroes">Add leading zeroes to numbers, eg. '1h' vs '00000001h'</param>
		/// <param name="signedNumber">Signed numbers if true, and unsigned numbers if false</param>
		/// <param name="signExtendImmediate">Sign extend the number to the real size (16-bit, 32-bit, 64-bit), eg. 'mov al,[eax+12h]' vs 'mov al,[eax+00000012h]'</param>
		public NumberFormattingOptions(FormatterOptions options, bool leadingZeroes, bool signedNumber, bool signExtendImmediate) {
			if (options is null)
				ThrowHelper.ThrowArgumentNullException_options();
			LeadingZeroes = leadingZeroes;
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
