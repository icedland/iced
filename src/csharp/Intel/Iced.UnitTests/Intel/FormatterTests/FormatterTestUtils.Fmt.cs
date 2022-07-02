// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static partial class FormatterTestUtils {
		public static void FormatTest(int bitness, string hexBytes, ulong ip, Code code, DecoderOptions options, string formattedString, Formatter formatter) {
			var decoder = CreateDecoder(bitness, hexBytes, ip, options);
			var nextRip = decoder.IP;
			var instruction = decoder.Decode();
			Assert.Equal(code, instruction.Code);
			Assert.Equal((ushort)nextRip, instruction.IP16);
			Assert.Equal((uint)nextRip, instruction.IP32);
			Assert.Equal(nextRip, instruction.IP);
			nextRip += (uint)instruction.Length;
			Assert.Equal(nextRip, decoder.IP);
			Assert.Equal((ushort)nextRip, instruction.NextIP16);
			Assert.Equal((uint)nextRip, instruction.NextIP32);
			Assert.Equal(nextRip, instruction.NextIP);
			FormatTest(instruction, formattedString, formatter);
		}

		public static void FormatTest(in Instruction instruction, string formattedString, Formatter formatter) {
			var output = new StringOutput();

			formatter.Format(instruction, output);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check

			formatter.FormatMnemonic(instruction, output);
			var mnemonic = output.ToStringAndReset();
			int opCount = formatter.GetOperandCount(instruction);
			var operands = opCount == 0 ? Array.Empty<string>() : new string[opCount];
			for (int i = 0; i < operands.Length; i++) {
				formatter.FormatOperand(instruction, output, i);
				operands[i] = output.ToStringAndReset();
			}
			output.Write(mnemonic, FormatterTextKind.Text);
			if (operands.Length > 0) {
				output.Write(" ", FormatterTextKind.Text);
				for (int i = 0; i < operands.Length; i++) {
					if (i > 0)
						formatter.FormatOperandSeparator(instruction, output);
					output.Write(operands[i], FormatterTextKind.Text);
				}
			}
			actualFormattedString = output.ToStringAndReset();
			Assert.Equal(formattedString, actualFormattedString);

			formatter.FormatAllOperands(instruction, output);
			var allOperands = output.ToStringAndReset();
			actualFormattedString = allOperands.Length == 0 ? mnemonic : mnemonic + " " + allOperands;
			Assert.Equal(formattedString, actualFormattedString);
		}

		public static void SimpleFormatTest(int bitness, string hexBytes, ulong ip, Code code, DecoderOptions options, string formattedString, Formatter formatter, Action<Decoder> initDecoder) {
			FormatInstr format = (in Instruction instruction) => {
				var output = new StringOutput();
				formatter.Format(instruction, output);
				return output.ToStringAndReset();
			};
			SimpleFormatTest(bitness, hexBytes, ip, code, options, formattedString, format, initDecoder);
		}

		public static void TestFormatterDoesNotThrow(Formatter formatter) {
			var output = new StringOutput();
			foreach (var tc in DecoderTests.DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: true)) {
				var decoder = CreateDecoder(tc.Bitness, tc.HexBytes, tc.IP, tc.Options);
				decoder.Decode(out var instruction);
				formatter.Format(instruction, output);
				output.Reset();
			}
		}
	}
}
#endif
