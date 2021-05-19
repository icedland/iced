// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if MASM
using Iced.Intel.FormatterInternal;

namespace Iced.Intel.MasmFormatterInternal {
	static class Registers {
		// Should be 1 past the last real register (not including DontUseF9-DontUseFF)
		public const int Register_ST = (int)Register.DontUseF9;
		public const int ExtraRegisters = 0;
		public static readonly FormatterString[] AllRegisters = RegistersTable.GetRegisters();
	}
}
#endif
