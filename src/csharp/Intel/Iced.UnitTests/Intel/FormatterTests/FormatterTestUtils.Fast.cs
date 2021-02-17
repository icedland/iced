// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static partial class FormatterTestUtils {
		public static void FormatTest(int bitness, string hexBytes, ulong ip, Code code, DecoderOptions options, string formattedString, FastFormatter formatter) {
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

		public static void FormatTest(in Instruction instruction, string formattedString, FastFormatter formatter) {
			var output = new FastStringOutput();

			formatter.Format(instruction, output);
			var actualFormattedString = output.ToString();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}

		public static void SimpleFormatTest(int bitness, string hexBytes, ulong ip, Code code, DecoderOptions options, string formattedString, FastFormatter formatter, Action<Decoder> initDecoder) {
			FormatInstr format = (in Instruction instruction) => {
				var output = new FastStringOutput();
				formatter.Format(instruction, output);
				return output.ToString();
			};
			SimpleFormatTest(bitness, hexBytes, ip, code, options, formattedString, format, initDecoder);
		}
	}
}
#endif
