// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if MASM
using Iced.Intel.FormatterInternal;

namespace Iced.Intel.MasmFormatterInternal {
	static class Registers {
#pragma warning disable CS0618 // Type or member is obsolete
		public const Register Register_ST = Register.DontUse0;
#pragma warning restore CS0618 // Type or member is obsolete
		public static readonly FormatterString[] AllRegisters = RegistersTable.GetRegisters();
	}
}
#endif
