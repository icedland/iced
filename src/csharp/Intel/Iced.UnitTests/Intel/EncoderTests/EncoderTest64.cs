// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class EncoderTest64 : EncoderTest {
		[Theory]
		[MemberData(nameof(Encode_Data))]
		void Encode(uint id, int bitness, Code code, string hexBytes, string encodedHexBytes, DecoderOptions options) => EncodeBase(id, bitness, code, hexBytes, encodedHexBytes, options);
		public static IEnumerable<object[]> Encode_Data => GetEncodeData(64);

		[Theory]
		[MemberData(nameof(NonDecodeEncode_Data))]
		void NonDecodeEncode(int bitness, Instruction instruction, string hexBytes, ulong rip) => NonDecodeEncodeBase(bitness, ref instruction, hexBytes, rip);
		public static IEnumerable<object[]> NonDecodeEncode_Data => GetNonDecodedEncodeData(64);
	}
}
#endif
