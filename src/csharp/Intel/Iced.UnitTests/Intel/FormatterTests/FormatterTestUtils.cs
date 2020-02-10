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
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class FormatterTestUtils {
		public static void FormatTest(int bitness, string hexBytes, Code code, DecoderOptions options, string formattedString, Formatter formatter) {
			var decoder = CreateDecoder(bitness, hexBytes, options, out ulong nextRip);
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

		public static void SimpleFormatTest(int bitness, string hexBytes, Code code, DecoderOptions options, string formattedString, Formatter formatter, Action<Decoder> initDecoder) {
			var decoder = CreateDecoder(bitness, hexBytes, options, out _);
			initDecoder?.Invoke(decoder);
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

			var output = new StringOutput();
			formatter.Format(instruction, output);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}

		static Decoder CreateDecoder(int bitness, string hexBytes, DecoderOptions options, out ulong rip) {
			var codeReader = new ByteArrayCodeReader(hexBytes);
			var decoder = Decoder.Create(bitness, codeReader, options);
			rip = bitness switch {
				16 => DecoderConstants.DEFAULT_IP16,
				32 => DecoderConstants.DEFAULT_IP32,
				64 => DecoderConstants.DEFAULT_IP64,
				_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
			};
			Assert.Equal(bitness, decoder.Bitness);
			decoder.IP = rip;
			return decoder;
		}

		public static void TestFormatterDoesNotThrow(Formatter formatter) {
			var output = new StringOutput();
			foreach (var info in DecoderTests.DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: true)) {
				var decoder = CreateDecoder(info.Bitness, info.HexBytes, info.Options, out _);
				decoder.Decode(out var instruction);
				formatter.Format(instruction, output);
				output.Reset();
			}
		}
	}
}
#endif
