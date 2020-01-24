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

using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	public readonly struct DecoderTestInfo {
		public readonly uint Id;
		public readonly int Bitness;
		public readonly Code Code;
		public readonly string HexBytes;
		public readonly string EncodedHexBytes;
		public readonly DecoderOptions Options;
		public readonly bool CanEncode;

		public DecoderTestInfo(uint id, int bitness, Code code, string hexBytes, string encodedHexBytes, DecoderOptions options, bool canEncode) {
			Id = id;
			Bitness = bitness;
			Code = code;
			HexBytes = hexBytes;
			EncodedHexBytes = encodedHexBytes;
			Options = options;
			CanEncode = canEncode;
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
				if (info.CanEncode)
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
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);
			foreach (var tc in DecoderTestCases.TestCases32)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);
			foreach (var tc in DecoderTestCases.TestCases64)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);

			if (!includeOtherTests)
				yield break;
			foreach (var tc in DecoderTestCases.TestCasesMisc16)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);
			foreach (var tc in DecoderTestCases.TestCasesMisc32)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);
			foreach (var tc in DecoderTestCases.TestCasesMisc64)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);

			foreach (var tc in DecoderTestCases.TestCasesMemory16)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);
			foreach (var tc in DecoderTestCases.TestCasesMemory32)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);
			foreach (var tc in DecoderTestCases.TestCasesMemory64)
				yield return new DecoderTestInfo(id++, tc.Bitness, tc.Code, tc.HexBytes, tc.EncodedHexBytes, tc.DecoderOptions, tc.CanEncode);
		}
	}
}
