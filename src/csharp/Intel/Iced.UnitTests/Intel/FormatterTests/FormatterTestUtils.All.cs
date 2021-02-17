// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	static partial class FormatterTestUtils {
		delegate string FormatInstr(in Instruction instruction);

		static void SimpleFormatTest(int bitness, string hexBytes, ulong ip, Code code, DecoderOptions options, string formattedString, FormatInstr format, Action<Decoder> initDecoder) {
			var decoder = CreateDecoder(bitness, hexBytes, ip, options);
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

			var actualFormattedString = format(instruction);
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}

		static Decoder CreateDecoder(int bitness, string hexBytes, ulong ip, DecoderOptions options) {
			var codeReader = new ByteArrayCodeReader(hexBytes);
			var decoder = Decoder.Create(bitness, codeReader, options);
			Assert.Equal(bitness, decoder.Bitness);
			decoder.IP = ip;
			return decoder;
		}
	}
}
#endif
