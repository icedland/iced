// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if FAST_FMT
using Iced.Intel.FormatterInternal;

namespace Iced.Intel.FastFormatterInternal {
	static class Registers {
		public const int ExtraRegisters = 0;
		public static readonly FormatterString[] AllRegisters = RegistersTable.GetRegisters();
	}
}
#endif
