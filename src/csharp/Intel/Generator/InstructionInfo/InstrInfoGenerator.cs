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
using Generator.Tables;

namespace Generator.InstructionInfo {
	abstract class InstrInfoGenerator {
		protected abstract void Generate(EnumType enumType);
		protected abstract void Generate(ConstantsType constantsType);
		protected abstract void Generate((InstructionDef def, uint dword1, uint dword2)[] infos);
		protected abstract void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified);
		protected abstract void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures);
		protected abstract void GenerateCore();

		protected readonly GenTypes genTypes;
		protected readonly InstrInfoTypes instrInfoTypes;

		protected InstrInfoGenerator(GenTypes genTypes) {
			this.genTypes = genTypes;
			instrInfoTypes = genTypes.GetObject<InstrInfoTypes>(TypeIds.InstrInfoTypes);
		}

		public void Generate() {
			var enumTypes = new List<EnumType> {
				genTypes[TypeIds.CodeInfo],
				genTypes[TypeIds.RflagsInfo],
				genTypes[TypeIds.InfoFlags1],
				genTypes[TypeIds.InfoFlags2],
				genTypes[TypeIds.CpuidFeatureInternal],
			};
			enumTypes.AddRange(instrInfoTypes.EnumOpInfos);
			foreach (var enumType in enumTypes)
				Generate(enumType);

			var constantsTypes = new ConstantsType[] {
				genTypes.GetConstantsType(TypeIds.InstrInfoConstants),
			};
			foreach (var constantsType in constantsTypes)
				Generate(constantsType);

			{
				var shifts = new int[IcedConstants.MaxOpCount] {
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo0Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo1Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo2Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo3Shift"].Value,
					(int)genTypes[TypeIds.InfoFlags1]["OpInfo4Shift"].Value,
				};
				var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
				var infos = new (InstructionDef def, uint dword1, uint dword2)[defs.Length];
				for (int i = 0; i < defs.Length; i++) {
					var def = defs[i];
					uint dword1 = 0;
					uint dword2 = 0;

					for (int j = 0; j < def.OpInfoEnum.Length; j++)
						dword1 |= def.OpInfoEnum[j].Value << shifts[j];
					var rflagsInfo = def.RflagsInfo ?? throw new InvalidOperationException();
					dword1 |= rflagsInfo.Value << (int)InfoFlags1.RflagsInfoShift;
					dword1 |= (uint)def.CodeInfo << (int)InfoFlags1.CodeInfoShift;
					if ((def.Flags1 & InstructionDefFlags1.OpMaskReadWrite) != 0) dword1 |= (uint)InfoFlags1.OpMaskReadWrite;
					if ((def.Flags1 & InstructionDefFlags1.IgnoresSegment) != 0) dword1 |= (uint)InfoFlags1.IgnoresSegment;

					dword2 |= def.EncodingValue.Value << (int)InfoFlags2.EncodingShift;
					if ((def.Flags1 & InstructionDefFlags1.SaveRestore) != 0) dword2 |= (uint)InfoFlags2.SaveRestore;
					if ((def.Flags1 & InstructionDefFlags1.StackInstruction) != 0) dword2 |= (uint)InfoFlags2.StackInstruction;
					if ((def.Flags3 & InstructionDefFlags3.Privileged) != 0) dword2 |= (uint)InfoFlags2.Privileged;
					dword2 |= def.ControlFlow.Value << (int)InfoFlags2.FlowControlShift;
					var cpuidInternal = def.CpuidInternal ?? throw new InvalidOperationException();
					dword2 |= cpuidInternal.Value << (int)InfoFlags2.CpuidFeatureInternalShift;

					infos[i] = (def, dword1, dword2);
				}
				Generate(infos);
			}

			{
				var rflagsInfos = instrInfoTypes.RflagsInfos;
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

			Generate(instrInfoTypes.CpuidFeatures);

			GenerateCore();
		}

		protected EnumValue ToOpAccess(EnumValue opInfo) {
			if (opInfo.RawName == nameof(OpInfo.ReadP3))
				return genTypes[TypeIds.OpAccess][nameof(OpAccess.Read)];
			return genTypes[TypeIds.OpAccess][opInfo.RawName];
		}
	}
}
