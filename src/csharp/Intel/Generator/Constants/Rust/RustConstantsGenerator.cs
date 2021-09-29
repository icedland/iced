// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Constants.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustConstantsGenerator : ConstantsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, PartialConstantsFileInfo?> toPartialFileInfo;
		readonly RustConstantsWriter constantsWriter;

		sealed class PartialConstantsFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly bool IsMatchConstants;

			public PartialConstantsFileInfo(string id, string filename, bool isMatchConstants = false) {
				Id = id;
				Filename = filename;
				IsMatchConstants = isMatchConstants;
			}
		}

		public RustConstantsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			constantsWriter = new RustConstantsWriter(genTypes, idConverter, new RustDocCommentWriter(idConverter), new RustDeprecatedWriter(idConverter));

			var dirs = generatorContext.Types.Dirs;
			toPartialFileInfo = new();
			toPartialFileInfo.Add(TypeIds.IcedConstants, new PartialConstantsFileInfo("IcedConstants", dirs.GetRustFilename("iced_constants.rs")));
			toPartialFileInfo.Add(TypeIds.DecoderTestParserConstants, new PartialConstantsFileInfo("DecoderTestText", dirs.GetRustFilename("decoder", "tests", "test_parser.rs"), true));
			toPartialFileInfo.Add(TypeIds.DecoderConstants, new PartialConstantsFileInfo("DecoderConstants", dirs.GetRustFilename("test_utils", "decoder_constants.rs")));
			toPartialFileInfo.Add(TypeIds.InstrInfoConstants, new PartialConstantsFileInfo("InstrInfoConstants", dirs.GetRustFilename("info", "enums.rs")));
			toPartialFileInfo.Add(TypeIds.MiscInstrInfoTestConstants, new PartialConstantsFileInfo("MiscConstants", dirs.GetRustFilename("info", "tests", "constants.rs")));
			toPartialFileInfo.Add(TypeIds.InstructionInfoKeys, new PartialConstantsFileInfo("KeysConstants", dirs.GetRustFilename("info", "tests", "test_parser.rs"), true));
			toPartialFileInfo.Add(TypeIds.RflagsBitsConstants, new PartialConstantsFileInfo("RflagsBitsConstants", dirs.GetRustFilename("info", "tests", "test_parser.rs")));
			toPartialFileInfo.Add(TypeIds.MiscSectionNames, new PartialConstantsFileInfo("MiscSectionNames", dirs.GetRustFilename("info", "tests", "misc_test_data.rs")));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoKeys, new PartialConstantsFileInfo("OpCodeInfoKeys", dirs.GetRustFilename("encoder", "tests", "op_code_test_case_parser.rs"), true));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoFlags, new PartialConstantsFileInfo("OpCodeInfoFlags", dirs.GetRustFilename("encoder", "tests", "op_code_test_case_parser.rs"), true));
		}

		public override void Generate(ConstantsType constantsType) {
			if (toPartialFileInfo.TryGetValue(constantsType.TypeId, out var partialInfo)) {
				if (partialInfo is not null)
					new FileUpdater(TargetLanguage.Rust, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteConstants(writer, partialInfo, constantsType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteConstants(FileWriter writer, PartialConstantsFileInfo info, ConstantsType constantsType) {
			if (info.IsMatchConstants) {
				var newType = CreateMatchConstants(constantsType);
				WriteMacro(writer, newType, constantsType);
				writer.WriteLine();
				constantsWriter.Write(writer, newType, Array.Empty<string>());
			}
			else
				constantsWriter.Write(writer, constantsType, Array.Empty<string>());
		}

		static ConstantsType CreateMatchConstants(ConstantsType constantsType) {
			var constants = constantsType.Constants;
			var newConstants = new Constant[constants.Length];
			for (int i = 0; i < constants.Length; i++) {
				var constant = constants[i];
				if (constant.RefValue is not string)
					throw new InvalidOperationException();
				newConstants[i] = new Constant(ConstantKind.UInt32, constant.RawName, (uint)i, ConstantsTypeFlags.None);
			}
			return new ConstantsType(constantsType.RawName, constantsType.TypeId, ConstantsTypeFlags.None, default, newConstants);
		}

		void WriteMacro(FileWriter writer, ConstantsType newType, ConstantsType origType) {
			writer.WriteLine("lazy_static! {");
			using (writer.Indent()) {
				writer.WriteLine($"pub(super) static ref {idConverter.Constant("To" + newType.RawName)}: HashMap<&'static str, u32> = {{");
				using (writer.Indent()) {
					if (newType.Constants.Length == 0)
						writer.WriteLine($"let h = HashMap::new();");
					else
						writer.WriteLine($"let mut h = HashMap::with_capacity({newType.Constants.Length});");
					var origConstants = origType.Constants;
					var newConstants = newType.Constants;
					if (origConstants.Length != newConstants.Length)
						throw new InvalidOperationException();
					for (int i = 0; i < origConstants.Length; i++) {
						var origConstant = origConstants[i];
						if (origConstant.Kind != ConstantKind.String)
							throw new InvalidOperationException();
						var newConstant = newConstants[i];
						if (newConstant.Kind != ConstantKind.UInt32)
							throw new InvalidOperationException();
						writer.WriteLine($"let _ = h.insert(\"{(string)origConstant.RefValue!}\", {newType.Name(idConverter)}::{newConstant.Name(idConverter)});");
					}
					writer.WriteLine("h");
				}
				writer.WriteLine("};");
			}
			writer.WriteLine("}");
		}
	}
}
