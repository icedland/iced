// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System.Collections.Generic;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class EncoderTest_Invalid : EncoderTest {
		[Theory]
		[MemberData(nameof(EncodeInvalid_Data))]
		void EncodeInvalid(uint id, int bitness, Code code, string hexBytes, ulong ip, DecoderOptions options, int invalidCodeSize) => EncodeInvalidBase(id, bitness, code, hexBytes, ip, options, invalidCodeSize);
		public static IEnumerable<object[]> EncodeInvalid_Data {
			get {
				foreach (var info in DecoderTestUtils.GetEncoderTests(includeOtherTests: false, includeInvalid: false)) {
					if (DecoderTestUtils.Code32Only.Contains(info.Code))
						yield return new object[] { info.Id, info.Bitness, info.Code, info.HexBytes, info.IP, info.Options, 64 };
					if (DecoderTestUtils.Code64Only.Contains(info.Code)) {
						yield return new object[] { info.Id, info.Bitness, info.Code, info.HexBytes, info.IP, info.Options, 16 };
						yield return new object[] { info.Id, info.Bitness, info.Code, info.HexBytes, info.IP, info.Options, 32 };
					}
				}
			}
		}
	}
}
#endif
