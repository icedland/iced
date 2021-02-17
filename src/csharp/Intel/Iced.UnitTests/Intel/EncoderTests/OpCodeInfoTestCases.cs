// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Iced.UnitTests.Intel.EncoderTests {
	static class OpCodeInfoTestCases {
		public static readonly OpCodeInfoTestCase[] OpCodeInfoTests = CreateOpCodeInfos();

		static OpCodeInfoTestCase[] CreateOpCodeInfos() {
			var filename = PathUtils.GetTestTextFilename("OpCodeInfos.txt", "Encoder");
			Debug.Assert(File.Exists(filename));
			return OpCodeInfoTestCasesReader.ReadFile(filename).ToArray();
		}
	}
}
#endif
