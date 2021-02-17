// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Constants.InstructionInfo {
	static class InstructionInfoKeys {
		public const string IsPrivileged = "priv";
		public const string IsSaveRestoreInstruction = "save-restore";
		public const string IsStackInstruction = "stack";
		public const string IsSpecial = "special";
		public const string RflagsRead = "fr";
		public const string RflagsUndefined = "fu";
		public const string RflagsWritten = "fw";
		public const string RflagsCleared = "fc";
		public const string RflagsSet = "fs";
		public const string FlowControl = "flow";
		public const string Op0Access = "op0";
		public const string Op1Access = "op1";
		public const string Op2Access = "op2";
		public const string Op3Access = "op3";
		public const string Op4Access = "op4";
		public const string ReadRegister = "r";
		public const string CondReadRegister = "cr";
		public const string WriteRegister = "w";
		public const string CondWriteRegister = "cw";
		public const string ReadWriteRegister = "rw";
		public const string ReadCondWriteRegister = "rcw";
		public const string ReadMemory = "rm";
		public const string CondReadMemory = "crm";
		public const string ReadWriteMemory = "rwm";
		public const string ReadCondWriteMemory = "rcwm";
		public const string WriteMemory = "wm";
		public const string CondWriteMemory = "cwm";
		public const string DecoderOptions = "decopt";
		public const string FpuTopIncrement = "fpu-inc";
		public const string FpuConditionalTop = "fpu-cond";
		public const string FpuWritesTop = "fpu-writes-top";
	}
}
