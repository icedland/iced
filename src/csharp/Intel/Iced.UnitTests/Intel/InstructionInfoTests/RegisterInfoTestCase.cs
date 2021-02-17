// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using Iced.Intel;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	sealed class RegisterInfoTestCase {
		public int LineNumber;
		public Register Register;
		public int Number;
		public Register BaseRegister;
		public Register FullRegister;
		public Register FullRegister32;
		public int Size;
		public RegisterFlags Flags;
	}
}
#endif
