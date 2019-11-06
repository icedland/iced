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

using System;
using System.Linq;
using Generator.Enums;

namespace Generator.Constants {
	static class IcedConstantsType {
		const string? documentation = null;
		public static readonly ConstantsType Instance = new ConstantsType(ConstantsTypeKind.IcedConstants, ConstantsTypeFlags.None, documentation: documentation, GetConstants());

		static Constant[] GetConstants() {
			var vmmFirst = RegisterEnum.Instance[Get_VMM_first()].Value;
			var vmmLast = RegisterEnum.Instance[Get_VMM_last()].Value;
			return new Constant[] {
				new Constant(ConstantKind.Int32, "MaxInstructionLength", 15, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.Int32, "MaxOpCount", 5, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.Int32, "NumberOfCodeValues", (uint)CodeEnum.Instance.Values.Length, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.Int32, "NumberOfRegisters", (uint)RegisterEnum.Instance.Values.Length, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.Int32, "NumberOfMemorySizes", (uint)MemorySizeEnum.Instance.Values.Length, ConstantsTypeFlags.None, null),
				// This is the largest vector register. If it's VEX/EVEX, the upper bits are always cleared when writing to any sub reg, eg. YMM0
				new Constant(ConstantKind.Register, "VMM_first", vmmFirst, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.Register, "VMM_last", vmmLast, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.Int32, "VMM_count", vmmLast - vmmFirst + 1, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.Int32, "MaxCpuidFeatureInternalValues", (uint)CpuidFeatureInternalEnum.Instance.Values.Length, ConstantsTypeFlags.None, null),
				new Constant(ConstantKind.MemorySize, "FirstBroadcastMemorySize", GetFirstBroadcastMemorySize(), ConstantsTypeFlags.None, null),
			};
		}

		const string VMM_prefix = "ZMM";
		static string Get_VMM_first() => VMM_prefix + "0";
		static string Get_VMM_last() {
			const int vmmLastNum = 31;
			var vmmLastStr = VMM_prefix + vmmLastNum.ToString();
			var vmmShouldNotExistStr = VMM_prefix + (vmmLastNum + 1).ToString();
			var vmmLast = RegisterEnum.Instance[vmmLastStr];
			if (RegisterEnum.Instance.Values.Any(a => a.Name == vmmShouldNotExistStr))
				throw new InvalidOperationException($"Register {vmmShouldNotExistStr} exists so {vmmLast.Name} isn't the last one");
			return vmmLast.Name;
		}

		static uint GetFirstBroadcastMemorySize() {
			var values = MemorySizeEnum.Instance.Values;
			uint? firstBroadcastValue = null;
			for (int i = 0; i < values.Length; i++) {
				var name = values[i].Name;
				bool isBroadcast = name.StartsWith("Broadcast");
				if (firstBroadcastValue != null) {
					if (!isBroadcast)
						throw new InvalidOperationException("Must be sorted so that all broadcast memory types are at the end of the enum");
				}
				else {
					if (isBroadcast)
						firstBroadcastValue = (uint)i;
				}
			}
			return firstBroadcastValue ?? throw new InvalidOperationException("Couldn't find a broadcast memory type");
		}
	}
}
