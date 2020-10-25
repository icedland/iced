/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace Generator.Constants.InstructionInfo {
	static class InstructionInfoKeys {
		public const string IsPrivileged = "priv";
		public const string IsSaveRestoreInstruction = "saverestore";
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
