// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	public readonly struct DecoderTestInfo {
		public readonly uint Id;
		public readonly int Bitness;
		public readonly ulong IP;
		public readonly Code Code;
		public readonly string HexBytes;
		public readonly string EncodedHexBytes;
		public readonly DecoderOptions Options;
		public readonly DecoderTestOptions TestOptions;

		public DecoderTestInfo(uint id, int bitness, ulong ip, Code code, string hexBytes, string encodedHexBytes, DecoderOptions options, DecoderTestOptions testOptions) {
			Id = id;
			Bitness = bitness;
			IP = ip;
			Code = code;
			HexBytes = hexBytes;
			EncodedHexBytes = encodedHexBytes;
			Options = options;
			TestOptions = testOptions;
		}
	}

	public static class DecoderTestUtils {
		static readonly HashSet<Code> notDecoded = CodeValueReader.Read("Code.NotDecoded.txt");
		static readonly HashSet<Code> notDecoded32Only = CodeValueReader.Read("Code.NotDecoded32Only.txt");
		static readonly HashSet<Code> notDecoded64Only = CodeValueReader.Read("Code.NotDecoded64Only.txt");
		static readonly HashSet<Code> code32Only = CodeValueReader.Read("Code.32Only.txt");
		static readonly HashSet<Code> code64Only = CodeValueReader.Read("Code.64Only.txt");

		public static HashSet<Code> NotDecoded => notDecoded;
		public static HashSet<Code> NotDecoded32Only => notDecoded32Only;
		public static HashSet<Code> NotDecoded64Only => notDecoded64Only;
		public static HashSet<Code> Code32Only => code32Only;
		public static HashSet<Code> Code64Only => code64Only;

		public static IEnumerable<DecoderTestInfo> GetEncoderTests(bool includeOtherTests, bool includeInvalid) {
			foreach (var info in GetDecoderTests(includeOtherTests, includeInvalid)) {
				if ((info.TestOptions & DecoderTestOptions.NoEncode) == 0)
					yield return info;
			}
		}

		public static IEnumerable<DecoderTestInfo> GetDecoderTests(bool includeOtherTests, bool includeInvalid) {
			foreach (var info in GetDecoderTests(includeOtherTests)) {
				if (includeInvalid || info.Code != Code.INVALID)
					yield return info;
			}
		}

		static IEnumerable<DecoderTestInfo> GetDecoderTests(bool includeOtherTests) {
			uint id = 0;
			foreach (var tc in DecoderTestCases.TestCases16)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);
			foreach (var tc in DecoderTestCases.TestCases32)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);
			foreach (var tc in DecoderTestCases.TestCases64)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);

			if (!includeOtherTests)
				yield break;
			foreach (var tc in DecoderTestCases.TestCasesMisc16)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);
			foreach (var tc in DecoderTestCases.TestCasesMisc32)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);
			foreach (var tc in DecoderTestCases.TestCasesMisc64)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);

			foreach (var tc in DecoderTestCases.TestCasesMemory16)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);
			foreach (var tc in DecoderTestCases.TestCasesMemory32)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);
			foreach (var tc in DecoderTestCases.TestCasesMemory64)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.IP, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.TestOptions);
		}
	}
}
