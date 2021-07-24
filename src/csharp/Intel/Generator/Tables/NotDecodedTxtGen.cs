// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables {
	[Generator(TargetLanguage.Other)]
	sealed class NotDecodedTxtGen {
		readonly GenTypes genTypes;

		public NotDecodedTxtGen(GeneratorContext generatorContext) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;

			var code32Only = new List<InstructionDef>();
			var code64Only = new List<InstructionDef>();
			var notDecoded = new List<InstructionDef>();
			var notDecoded32 = new List<InstructionDef>();
			var notDecoded64 = new List<InstructionDef>();
			foreach (var def in defs) {
				if ((def.Flags1 & InstructionDefFlags1.Fwait) != 0 || def.Code.RawName == nameof(Code.Popw_CS)) {
					int bits = 0;
					if ((def.Flags2 & (InstructionDefFlags2.IntelDecoder16 | InstructionDefFlags2.AmdDecoder16 | InstructionDefFlags2.IntelDecoder32 | InstructionDefFlags2.AmdDecoder32)) == 0)
						bits |= 16 | 32;
					if ((def.Flags2 & (InstructionDefFlags2.IntelDecoder64 | InstructionDefFlags2.AmdDecoder64)) == 0)
						bits |= 64;
					if (bits == 0)
						notDecoded.Add(def);
					else if ((bits & (16 | 32)) == 0)
						notDecoded32.Add(def);
					else if ((bits & 64) == 0)
						notDecoded64.Add(def);
				}
				else {
					if ((def.Flags1 & InstructionDefFlags1.Bit64) == 0)
						code32Only.Add(def);
					if ((def.Flags1 & (InstructionDefFlags1.Bit16 | InstructionDefFlags1.Bit32 | InstructionDefFlags1.Bit64)) == InstructionDefFlags1.Bit64)
						code64Only.Add(def);
				}
			}

			var fileInfos = new (string filename, List<InstructionDef> defs)[] {
				("Code.32Only.txt", code32Only),
				("Code.64Only.txt", code64Only),
				("Code.NotDecoded.txt", notDecoded),
				("Code.NotDecoded32Only.txt", notDecoded32),
				("Code.NotDecoded64Only.txt", notDecoded64),
			};
			foreach (var (filename, notDecodedDefs) in fileInfos) {
				var filenamePath = genTypes.Dirs.GetUnitTestFilename("Decoder", filename);
				using (var writer = new FileWriter(TargetLanguage.Other, FileUtils.OpenWrite(filenamePath))) {
					writer.WriteFileHeader();
					foreach (var def in notDecodedDefs)
						writer.WriteLine(def.Code.RawName);
				}
			}
		}
	}
}
