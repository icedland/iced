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

using Generator.Enums;
using Generator.Enums.InstructionInfo;

namespace Generator.InstructionInfo {
	sealed class DictConstants {
		public static readonly (string name, EnumValue value)[] OpAccessConstants = new (string name, EnumValue value)[] {
			("n", OpAccessEnum.Instance["None"]),
			("r", OpAccessEnum.Instance["Read"]),
			("cr", OpAccessEnum.Instance["CondRead"]),
			("w", OpAccessEnum.Instance["Write"]),
			("cw", OpAccessEnum.Instance["CondWrite"]),
			("rw", OpAccessEnum.Instance["ReadWrite"]),
			("rcw", OpAccessEnum.Instance["ReadCondWrite"]),
			("nma", OpAccessEnum.Instance["NoMemAccess"]),
		};

		public static readonly (string value, EnumValue flags)[] MemorySizeFlagsTable = new (string value, EnumValue flags)[] {
			("signed", MemorySizeFlagsEnum.Instance["Signed"]),
			("bcst", MemorySizeFlagsEnum.Instance["Broadcast"]),
			("packed", MemorySizeFlagsEnum.Instance["Packed"]),
		};

		public static readonly (string value, EnumValue flags)[] RegisterFlagsTable = new (string value, EnumValue flags)[] {
			("seg", RegisterFlagsEnum.Instance["SegmentRegister"]),
			("gpr", RegisterFlagsEnum.Instance["GPR"]),
			("gpr8", RegisterFlagsEnum.Instance["GPR8"]),
			("gpr16", RegisterFlagsEnum.Instance["GPR16"]),
			("gpr32", RegisterFlagsEnum.Instance["GPR32"]),
			("gpr64", RegisterFlagsEnum.Instance["GPR64"]),
			("xmm", RegisterFlagsEnum.Instance["XMM"]),
			("ymm", RegisterFlagsEnum.Instance["YMM"]),
			("zmm", RegisterFlagsEnum.Instance["ZMM"]),
			("vec", RegisterFlagsEnum.Instance["VectorRegister"]),
		};
	}
}
