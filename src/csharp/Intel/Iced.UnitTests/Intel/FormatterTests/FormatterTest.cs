// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class FormatterTest {
		protected void FormatBase(int index, InstructionInfo info, string formattedString, Formatter formatter) =>
			FormatterTestUtils.FormatTest(info.Bitness, info.HexBytes, info.IP, info.Code, info.Options, formattedString, formatter);

		protected void FormatBase(int index, Instruction instruction, string formattedString, Formatter formatter) =>
			FormatterTestUtils.FormatTest(instruction, formattedString, formatter);
	}
}
#endif
