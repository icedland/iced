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

#if FAST_FMT
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static partial class FormatterTestUtils {
		public static void FormatTest(int bitness, string hexBytes, Code code, DecoderOptions options, string formattedString, FastFormatter formatter) {
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

		public static void FormatTest(in Instruction instruction, string formattedString, FastFormatter formatter) {
			var output = new FastStringOutput();

			formatter.Format(instruction, output);
			var actualFormattedString = output.ToString();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}

		public static void SimpleFormatTest(int bitness, string hexBytes, Code code, DecoderOptions options, string formattedString, FastFormatter formatter, Action<Decoder> initDecoder) {
			FormatInstr format = (in Instruction instruction) => {
				var output = new FastStringOutput();
				formatter.Format(instruction, output);
				return output.ToString();
			};
			SimpleFormatTest(bitness, hexBytes, code, options, formattedString, format, initDecoder);
		}
	}
}
#endif
