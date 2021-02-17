// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class EncoderTest64 : EncoderTest {
		[Theory]
		[MemberData(nameof(Encode_Data))]
		void Encode(uint id, int bitness, Code code, string hexBytes, ulong ip, string encodedHexBytes, DecoderOptions options) => EncodeBase(id, bitness, code, hexBytes, ip, encodedHexBytes, options);
		public static IEnumerable<object[]> Encode_Data => GetEncodeData(64);

		[Theory]
		[MemberData(nameof(NonDecodeEncode_Data))]
		void NonDecodeEncode(int bitness, Instruction instruction, string hexBytes, ulong rip) => NonDecodeEncodeBase(bitness, ref instruction, hexBytes, rip);
		public static IEnumerable<object[]> NonDecodeEncode_Data => GetNonDecodedEncodeData(64);
	}
}
#endif
