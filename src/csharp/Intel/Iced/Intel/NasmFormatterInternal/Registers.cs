// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

#if NASM
using Iced.Intel.FormatterInternal;

namespace Iced.Intel.NasmFormatterInternal {
	static class Registers {
		public const int ExtraRegisters = 0;
		public static readonly FormatterString[] AllRegisters = GetRegisters();
		static FormatterString[] GetRegisters() {
			var registers = RegistersTable.GetRegisters();
			for (int i = 0; i < 8; i++)
				registers[(int)Register.ST0 + i] = new FormatterString("st" + i.ToString());
			return registers;
		}
	}
}
#endif
