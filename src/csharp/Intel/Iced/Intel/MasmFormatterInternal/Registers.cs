// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if MASM
using Iced.Intel.FormatterInternal;

namespace Iced.Intel.MasmFormatterInternal {
	static class Registers {
		public const int Register_ST = IcedConstants.RegisterEnumCount + 0;
		public const int ExtraRegisters = 1;
		public static readonly FormatterString[] AllRegisters = RegistersTable.GetRegisters();
	}
}
#endif
