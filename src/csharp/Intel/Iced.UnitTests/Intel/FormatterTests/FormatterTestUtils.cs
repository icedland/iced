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
			Assert.Equal(nextRip, instr.IP);
			nextRip += (uint)instr.Length;
			Assert.Equal(nextRip, decoder.IP);
			Assert.Equal((ushort)nextRip, instr.NextIP16);
			Assert.Equal((uint)nextRip, instr.NextIP32);
			Assert.Equal(nextRip, instr.NextIP);
			FormatTest(instr, formattedString, formatter);
		}

		public static void FormatTest(in Instruction instr, string formattedString, Formatter formatter) {
			var output = new StringBuilderFormatterOutput();

			formatter.Format(instr, output);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check

			formatter.FormatMnemonic(instr, output);
			var mnemonic = output.ToStringAndReset();
			int opCount = formatter.GetOperandCount(instr);
			var operands = opCount == 0 ? Array.Empty<string>() : new string[opCount];
			for (int i = 0; i < operands.Length; i++) {
				formatter.FormatOperand(instr, output, i);
				operands[i] = output.ToStringAndReset();
			}
			output.Write(mnemonic, FormatterOutputTextKind.Text);
			if (operands.Length > 0) {
				output.Write(" ", FormatterOutputTextKind.Text);
				for (int i = 0; i < operands.Length; i++) {
					if (i > 0)
						formatter.FormatOperandSeparator(instr, output);
					output.Write(operands[i], FormatterOutputTextKind.Text);
				}
			}
			actualFormattedString = output.ToStringAndReset();
			Assert.Equal(formattedString, actualFormattedString);

			formatter.FormatAllOperands(instr, output);
			var allOperands = output.ToStringAndReset();
			actualFormattedString = allOperands.Length == 0 ? mnemonic : mnemonic + " " + allOperands;
			Assert.Equal(formattedString, actualFormattedString);
		}

		public static void SimpleFormatTest(int codeSize, string hexBytes, Code code, DecoderOptions options, string formattedString, Formatter formatter, Action<Decoder> initDecoder) {
			var decoder = CreateDecoder(codeSize, hexBytes, options, out _);
			initDecoder?.Invoke(decoder);
			var nextRip = decoder.IP;
			var instr = decoder.Decode();
			Assert.Equal(code, instr.Code);
			Assert.Equal((ushort)nextRip, instr.IP16);
			Assert.Equal((uint)nextRip, instr.IP32);
			Assert.Equal(nextRip, instr.IP);
			nextRip += (uint)instr.Length;
			Assert.Equal(nextRip, decoder.IP);
			Assert.Equal((ushort)nextRip, instr.NextIP16);
			Assert.Equal((uint)nextRip, instr.NextIP32);
			Assert.Equal(nextRip, instr.NextIP);

			var output = new StringBuilderFormatterOutput();

			formatter.Format(instr, output);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}

		static Decoder CreateDecoder(int codeSize, string hexBytes, DecoderOptions options, out ulong rip) {
			var codeReader = new ByteArrayCodeReader(hexBytes);
			var decoder = Decoder.Create(codeSize, codeReader, options);
			rip = codeSize switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new ArgumentOutOfRangeException(nameof(codeSize)),
			};
			Assert.Equal(codeSize, decoder.Bitness);
			decoder.IP = rip;
			return decoder;
		}

		public static void TestFormatterDoesNotThrow(Formatter formatter) {
			var output = new StringBuilderFormatterOutput();
			foreach (var info in DecoderTests.DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: true)) {
				var decoder = CreateDecoder(info.Bitness, info.HexBytes, info.Options, out _);
				decoder.Decode(out var instr);
				formatter.Format(instr, output);
				output.Reset();
			}
		}
	}
}
#endif
