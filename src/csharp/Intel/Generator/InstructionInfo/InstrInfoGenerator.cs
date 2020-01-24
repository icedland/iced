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
using System.Collections.Generic;
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.InstructionInfo;

namespace Generator.InstructionInfo {
	abstract class InstrInfoGenerator {
		protected abstract void Generate(EnumType enumType);
		protected abstract void Generate(ConstantsType constantsType);
		protected abstract void Generate((InstrInfo info, uint dword1, uint dword2)[] infos);
		protected abstract void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified);
		protected abstract void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures);
		protected abstract void GenerateCore();

		public void Generate() {
			var enumTypes = new List<EnumType> {
				InstrInfoTypes.EnumCodeInfo,
				InstrInfoTypes.EnumRflagsInfo,
				InstrInfoTypes.EnumInfoFlags1,
				InstrInfoTypes.EnumInfoFlags2,
				InstrInfoTypes.EnumCpuidFeatureInternal,
			};
			enumTypes.AddRange(InstrInfoTypes.EnumOpInfos);
			foreach (var enumType in enumTypes)
				Generate(enumType);

			var constantsTypes = new ConstantsType[] {
				InstrInfoTypes.InstrInfoConstants,
			};
			foreach (var constantsType in constantsTypes)
				Generate(constantsType);

			{
				var shifts = new int[IcedConstants.MaxOpCount] {
					(int)InstrInfoTypes.EnumInfoFlags1["OpInfo0Shift"].Value,
					(int)InstrInfoTypes.EnumInfoFlags1["OpInfo1Shift"].Value,
					(int)InstrInfoTypes.EnumInfoFlags1["OpInfo2Shift"].Value,
					(int)InstrInfoTypes.EnumInfoFlags1["OpInfo3Shift"].Value,
					(int)InstrInfoTypes.EnumInfoFlags1["OpInfo4Shift"].Value,
				};
				var infos = InstrInfoTypes.InstrInfos;
				var instrInfos = new (InstrInfo info, uint dword1, uint dword2)[infos.Length];
				for (int i = 0; i < infos.Length; i++) {
					var info = infos[i];
					uint dword1 = 0;
					uint dword2 = 0;

					for (int j = 0; j < info.OpInfoEnum.Length; j++)
						dword1 |= info.OpInfoEnum[j].Value << shifts[j];
					var rflagsInfo = info.RflagsInfo ?? throw new InvalidOperationException();
					dword1 |= rflagsInfo.Value << (int)InfoFlags1.RflagsInfoShift;
					dword1 |= (uint)info.CodeInfo << (int)InfoFlags1.CodeInfoShift;
					if ((info.Flags & InstrInfoFlags.SaveRestore) != 0) dword1 |= (uint)InfoFlags1.SaveRestore;
					if ((info.Flags & InstrInfoFlags.StackInstruction) != 0) dword1 |= (uint)InfoFlags1.StackInstruction;
					if ((info.Flags & InstrInfoFlags.ProtectedMode) != 0) dword1 |= (uint)InfoFlags1.ProtectedMode;
					if ((info.Flags & InstrInfoFlags.Privileged) != 0) dword1 |= (uint)InfoFlags1.Privileged;
					if ((info.Flags & InstrInfoFlags.NoSegmentRead) != 0) dword1 |= (uint)InfoFlags1.NoSegmentRead;

					dword2 |= info.Encoding.Value << (int)InfoFlags2.EncodingShift;
					if ((info.Flags & InstrInfoFlags.AVX2_Check) != 0) dword2 |= (uint)InfoFlags2.AVX2_Check;
					if ((info.Flags & InstrInfoFlags.OpMaskRegReadWrite) != 0) dword2 |= (uint)InfoFlags2.OpMaskRegReadWrite;
					dword2 |= info.FlowControl.Value << (int)InfoFlags2.FlowControlShift;
					var cpuidInternal = info.CpuidInternal ?? throw new InvalidOperationException();
					dword2 |= cpuidInternal.Value << (int)InfoFlags2.CpuidFeatureInternalShift;

					instrInfos[i] = (info, dword1, dword2);
				}
				Generate(instrInfos);
			}

			{
				var rflagsInfos = InstrInfoTypes.RflagsInfos;
				var enumValues = new EnumValue[rflagsInfos.Length];
				var read = new RflagsBits[rflagsInfos.Length];
				var undefined = new RflagsBits[rflagsInfos.Length];
				var written = new RflagsBits[rflagsInfos.Length];
				var cleared = new RflagsBits[rflagsInfos.Length];
				var set = new RflagsBits[rflagsInfos.Length];
				var modified = new RflagsBits[rflagsInfos.Length];
				for (int i = 0; i < rflagsInfos.Length; i++) {
					var rflags = rflagsInfos[i].rflags;
					enumValues[i] = rflagsInfos[i].value;
					read[i] = rflags.read;
					undefined[i] = rflags.undefined;
					written[i] = rflags.written;
					cleared[i] = rflags.cleared;
					set[i] = rflags.set;
					modified[i] = rflags.undefined | rflags.written | rflags.cleared | rflags.set;
				}
				Generate(enumValues, read, undefined, written, cleared, set, modified);
			}

			Generate(InstrInfoTypes.CpuidFeatures);

			GenerateCore();
		}

		protected static EnumValue ToOpAccess(EnumValue opInfo) {
			if (opInfo.RawName == nameof(OpInfo.ReadP3))
				return OpAccessEnum.Instance[nameof(OpAccess.Read)];
			return OpAccessEnum.Instance[opInfo.RawName];
		}
	}
}
