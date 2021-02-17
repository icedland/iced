// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	public abstract class FastFormatterTest {
		protected void FormatBase(int index, InstructionInfo info, string formattedString, FastFormatter formatter) =>
			FormatterTestUtils.FormatTest(info.Bitness, info.HexBytes, info.IP, info.Code, info.Options, formattedString, formatter);

		protected void FormatBase(int index, Instruction instruction, string formattedString, FastFormatter formatter) =>
			FormatterTestUtils.FormatTest(instruction, formattedString, formatter);
	}
}
#endif
