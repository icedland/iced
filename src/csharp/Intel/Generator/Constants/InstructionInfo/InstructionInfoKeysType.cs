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
	[TypeGen(TypeGenOrders.CreateSimpleTypes)]
	sealed class InstructionInfoKeysType {
		InstructionInfoKeysType(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.InstructionInfoKeys, ConstantsTypeFlags.None, null, GetConstants());
			genTypes.Add(type);
		}

		static Constant[] GetConstants() =>
			new Constant[] {
				new Constant(ConstantKind.String, "IsProtectedMode", "pm"),
				new Constant(ConstantKind.String, "IsPrivileged", "priv"),
				new Constant(ConstantKind.String, "IsSaveRestoreInstruction", "saverestore"),
				new Constant(ConstantKind.String, "IsStackInstruction", "stack"),
				new Constant(ConstantKind.String, "IsSpecial", "special"),
				new Constant(ConstantKind.String, "RflagsRead", "fr"),
				new Constant(ConstantKind.String, "RflagsUndefined", "fu"),
				new Constant(ConstantKind.String, "RflagsWritten", "fw"),
				new Constant(ConstantKind.String, "RflagsCleared", "fc"),
				new Constant(ConstantKind.String, "RflagsSet", "fs"),
				new Constant(ConstantKind.String, "FlowControl", "flow"),
				new Constant(ConstantKind.String, "Op0Access", "op0"),
				new Constant(ConstantKind.String, "Op1Access", "op1"),
				new Constant(ConstantKind.String, "Op2Access", "op2"),
				new Constant(ConstantKind.String, "Op3Access", "op3"),
				new Constant(ConstantKind.String, "Op4Access", "op4"),
				new Constant(ConstantKind.String, "ReadRegister", "r"),
				new Constant(ConstantKind.String, "CondReadRegister", "cr"),
				new Constant(ConstantKind.String, "WriteRegister", "w"),
				new Constant(ConstantKind.String, "CondWriteRegister", "cw"),
				new Constant(ConstantKind.String, "ReadWriteRegister", "rw"),
				new Constant(ConstantKind.String, "ReadCondWriteRegister", "rcw"),
				new Constant(ConstantKind.String, "ReadMemory", "rm"),
				new Constant(ConstantKind.String, "CondReadMemory", "crm"),
				new Constant(ConstantKind.String, "ReadWriteMemory", "rwm"),
				new Constant(ConstantKind.String, "ReadCondWriteMemory", "rcwm"),
				new Constant(ConstantKind.String, "WriteMemory", "wm"),
				new Constant(ConstantKind.String, "CondWriteMemory", "cwm"),
				new Constant(ConstantKind.String, "DecoderOptions", "decopt"),
			};
	}
}
