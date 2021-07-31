// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables {
	[Generator(TargetLanguage.Other)]
	sealed class RegisterInfoTxtGen {
		readonly GenTypes genTypes;

		public RegisterInfoTxtGen(GeneratorContext generatorContext) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			const string sepNoSpace = ",";
			const string sep = sepNoSpace + " ";

			var defs = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs).Defs;
			var filename = genTypes.Dirs.GetUnitTestFilename("InstructionInfo", "RegisterInfo.txt");
			using (var writer = new FileWriter(TargetLanguage.Other, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				EnumValue? lastRegKind = null;
				foreach (var def in defs) {
					if (lastRegKind is not null && def.RegisterKind != lastRegKind)
						writer.WriteLine();
					lastRegKind = def.RegisterKind;

					writer.Write(def.Register.RawName.ToLowerInvariant());

					writer.Write(sep);
					writer.Write(def.Index.ToString());

					writer.Write(sep);
					writer.Write(def.BaseRegister.RawName.ToLowerInvariant());

					writer.Write(sep);
					writer.Write(def.FullRegister.RawName.ToLowerInvariant());

					writer.Write(sep);
					writer.Write(def.FullRegister32.RawName.ToLowerInvariant());

					writer.Write(sep);
					writer.Write(def.Size.ToString());

					switch (def.GetRegisterKind()) {
					case RegisterKind.None:
						writer.Write(sepNoSpace);
						break;
					case RegisterKind.GPR8:
						writer.Write(sep);
						writer.Write(RegisterConstants.GPR);
						writer.Write(" ");
						writer.Write(RegisterConstants.GPR8);
						break;
					case RegisterKind.GPR16:
						writer.Write(sep);
						writer.Write(RegisterConstants.GPR);
						writer.Write(" ");
						writer.Write(RegisterConstants.GPR16);
						break;
					case RegisterKind.GPR32:
						writer.Write(sep);
						writer.Write(RegisterConstants.GPR);
						writer.Write(" ");
						writer.Write(RegisterConstants.GPR32);
						break;
					case RegisterKind.GPR64:
						writer.Write(sep);
						writer.Write(RegisterConstants.GPR);
						writer.Write(" ");
						writer.Write(RegisterConstants.GPR64);
						break;
					case RegisterKind.IP:
						writer.Write(sep);
						writer.Write(RegisterConstants.IP);
						break;
					case RegisterKind.Segment:
						writer.Write(sep);
						writer.Write(RegisterConstants.SegmentRegister);
						break;
					case RegisterKind.ST:
						writer.Write(sep);
						writer.Write(RegisterConstants.ST);
						break;
					case RegisterKind.CR:
						writer.Write(sep);
						writer.Write(RegisterConstants.CR);
						break;
					case RegisterKind.DR:
						writer.Write(sep);
						writer.Write(RegisterConstants.DR);
						break;
					case RegisterKind.TR:
						writer.Write(sep);
						writer.Write(RegisterConstants.TR);
						break;
					case RegisterKind.BND:
						writer.Write(sep);
						writer.Write(RegisterConstants.BND);
						break;
					case RegisterKind.K:
						writer.Write(sep);
						writer.Write(RegisterConstants.K);
						break;
					case RegisterKind.MM:
						writer.Write(sep);
						writer.Write(RegisterConstants.MM);
						break;
					case RegisterKind.XMM:
						writer.Write(sep);
						writer.Write(RegisterConstants.VectorRegister);
						writer.Write(" ");
						writer.Write(RegisterConstants.XMM);
						break;
					case RegisterKind.YMM:
						writer.Write(sep);
						writer.Write(RegisterConstants.VectorRegister);
						writer.Write(" ");
						writer.Write(RegisterConstants.YMM);
						break;
					case RegisterKind.ZMM:
						writer.Write(sep);
						writer.Write(RegisterConstants.VectorRegister);
						writer.Write(" ");
						writer.Write(RegisterConstants.ZMM);
						break;
					case RegisterKind.TMM:
						writer.Write(sep);
						writer.Write(RegisterConstants.TMM);
						break;
					default:
						throw new InvalidOperationException();
					}

					writer.WriteLine();
				}
			}
		}
	}
}
