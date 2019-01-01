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
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class FormatterTestUtils {
		public static void FormatTest(int codeSize, string hexBytes, Code code, DecoderOptions options, string formattedString, Formatter formatter) {
			var decoder = CreateDecoder(codeSize, hexBytes, options, out ulong nextRip);
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			Assert.Equal((ushort)nextRip, instr.IP16);
			Assert.Equal((uint)nextRip, instr.IP32);
			Assert.Equal(nextRip, instr.IP64);
			nextRip += (uint)instr.ByteLength;
			Assert.Equal(nextRip, decoder.InstructionPointer);
			Assert.Equal((ushort)nextRip, instr.NextIP16);
			Assert.Equal((uint)nextRip, instr.NextIP32);
			Assert.Equal(nextRip, instr.NextIP64);
			FormatTest(ref instr, formattedString, formatter);
		}

		public static void FormatTest(ref Instruction instr, string formattedString, Formatter formatter) {
			var output = new StringBuilderFormatterOutput();

			formatter.Format(ref instr, output);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check

			formatter.FormatMnemonic(ref instr, output);
			var mnemonic = output.ToStringAndReset();
			int opCount = formatter.GetOperandCount(ref instr);
			var operands = opCount == 0 ? Array.Empty<string>() : new string[opCount];
			for (int i = 0; i < operands.Length; i++) {
				formatter.FormatOperand(ref instr, output, i);
				operands[i] = output.ToStringAndReset();
			}
			output.Write(mnemonic, FormatterOutputTextKind.Text);
			if (operands.Length > 0) {
				output.Write(" ", FormatterOutputTextKind.Text);
				for (int i = 0; i < operands.Length; i++) {
					if (i > 0)
						formatter.FormatOperandSeparator(ref instr, output);
					output.Write(operands[i], FormatterOutputTextKind.Text);
				}
			}
			actualFormattedString = output.ToStringAndReset();
			Assert.Equal(formattedString, actualFormattedString);

			formatter.FormatAllOperands(ref instr, output);
			var allOperands = output.ToStringAndReset();
			actualFormattedString = allOperands.Length == 0 ? mnemonic : mnemonic + " " + allOperands;
			Assert.Equal(formattedString, actualFormattedString);
		}

		public static void SimpleFormatTest(int codeSize, string hexBytes, Code code, DecoderOptions options, string formattedString, Formatter formatter, Action<Decoder> initDecoder) {
			var decoder = CreateDecoder(codeSize, hexBytes, options, out _);
			initDecoder?.Invoke(decoder);
			var nextRip = decoder.InstructionPointer;
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			Assert.Equal((ushort)nextRip, instr.IP16);
			Assert.Equal((uint)nextRip, instr.IP32);
			Assert.Equal(nextRip, instr.IP64);
			nextRip += (uint)instr.ByteLength;
			Assert.Equal(nextRip, decoder.InstructionPointer);
			Assert.Equal((ushort)nextRip, instr.NextIP16);
			Assert.Equal((uint)nextRip, instr.NextIP32);
			Assert.Equal(nextRip, instr.NextIP64);

			var output = new StringBuilderFormatterOutput();

			formatter.Format(ref instr, output);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}

		static Decoder CreateDecoder(int codeSize, string hexBytes, DecoderOptions options, out ulong rip) {
			Decoder decoder;
			var codeReader = new ByteArrayCodeReader(hexBytes);
			switch (codeSize) {
			case 16:
				decoder = Decoder.Create16(codeReader, options);
				rip = DecoderConstants.DEFAULT_IP16;
				break;

			case 32:
				decoder = Decoder.Create32(codeReader, options);
				rip = DecoderConstants.DEFAULT_IP32;
				break;

			case 64:
				decoder = Decoder.Create64(codeReader, options);
				rip = DecoderConstants.DEFAULT_IP64;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(codeSize));
			}

			Assert.Equal(codeSize, decoder.Bitness);
			decoder.InstructionPointer = rip;
			return decoder;
		}
	}
}
#endif
