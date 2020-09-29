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
using System.IO;
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

			var dir = generatorContext.Types.Dirs.RustDir;
			toPartialFileInfo = new Dictionary<TypeId, PartialConstantsFileInfo?>();
			toPartialFileInfo.Add(TypeIds.IcedConstants, new PartialConstantsFileInfo("IcedConstants", Path.Combine(dir, "iced_constants.rs")));
			toPartialFileInfo.Add(TypeIds.DecoderTestParserConstants, new PartialConstantsFileInfo("DecoderTestText", Path.Combine(dir, "decoder", "tests", "test_parser.rs"), true));
			toPartialFileInfo.Add(TypeIds.DecoderConstants, new PartialConstantsFileInfo("DecoderConstants", Path.Combine(dir, "test_utils", "decoder_constants.rs")));
			toPartialFileInfo.Add(TypeIds.InstrInfoConstants, new PartialConstantsFileInfo("InstrInfoConstants", Path.Combine(dir, "info", "enums.rs")));
			toPartialFileInfo.Add(TypeIds.MiscInstrInfoTestConstants, new PartialConstantsFileInfo("MiscConstants", Path.Combine(dir, "info", "tests", "constants.rs")));
			toPartialFileInfo.Add(TypeIds.InstructionInfoKeys, new PartialConstantsFileInfo("KeysConstants", Path.Combine(dir, "info", "tests", "test_parser.rs"), true));
			toPartialFileInfo.Add(TypeIds.RflagsBitsConstants, new PartialConstantsFileInfo("RflagsBitsConstants", Path.Combine(dir, "info", "tests", "test_parser.rs")));
			toPartialFileInfo.Add(TypeIds.MiscSectionNames, new PartialConstantsFileInfo("MiscSectionNames", Path.Combine(dir, "info", "tests", "misc_test_data.rs")));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoKeys, new PartialConstantsFileInfo("OpCodeInfoKeys", Path.Combine(dir, "encoder", "tests", "op_code_test_case_parser.rs"), true));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoFlags, new PartialConstantsFileInfo("OpCodeInfoFlags", Path.Combine(dir, "encoder", "tests", "op_code_test_case_parser.rs"), true));
			toPartialFileInfo.Add(TypeIds.OpCodeFlags, new PartialConstantsFileInfo("Flags", Path.Combine(dir, "encoder", "op_code.rs")));
		}

		public override void Generate(ConstantsType constantsType) {
			if (toPartialFileInfo.TryGetValue(constantsType.TypeId, out var partialInfo)) {
				if (!(partialInfo is null))
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
				if (!(constant.RefValue is string s))
					throw new InvalidOperationException();
				newConstants[i] = new Constant(ConstantKind.UInt32, constant.RawName, (uint)i, ConstantsTypeFlags.None);
			}
			return new ConstantsType(constantsType.RawName, constantsType.TypeId, ConstantsTypeFlags.None, null, newConstants);
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
						writer.WriteLine($"h.insert(\"{(string)origConstant.RefValue!}\", {newType.Name(idConverter)}::{newConstant.Name(idConverter)});");
					}
					writer.WriteLine("h");
				}
				writer.WriteLine("};");
			}
			writer.WriteLine("}");
		}
	}
}
