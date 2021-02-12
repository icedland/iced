// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;
using System.Collections.Generic;
using Generator.Constants.Rust;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Constants.Python {
	[Generator(TargetLanguage.Python)]
	sealed class PythonRustConstantsGenerator : ConstantsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, PartialConstantsFileInfo?> toPartialFileInfo;
		readonly RustConstantsWriter constantsWriter;

		sealed class PartialConstantsFileInfo {
			public readonly string Id;
			public readonly string Filename;

			public PartialConstantsFileInfo(string id, string filename) {
				Id = id;
				Filename = filename;
			}
		}

		public PythonRustConstantsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			constantsWriter = new RustConstantsWriter(genTypes, idConverter, new RustDocCommentWriter(idConverter), new RustDeprecatedWriter(idConverter));

			var dirs = generatorContext.Types.Dirs;
			toPartialFileInfo = new Dictionary<TypeId, PartialConstantsFileInfo?>();
			toPartialFileInfo.Add(TypeIds.IcedConstants, new PartialConstantsFileInfo("IcedConstants", dirs.GetPythonRustFilename("iced_constants.rs")));
		}

		public override void Generate(ConstantsType constantsType) {
			if (toPartialFileInfo.TryGetValue(constantsType.TypeId, out var partialInfo)) {
				if (partialInfo is not null)
					new FileUpdater(TargetLanguage.Rust, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteConstants(writer, constantsType));
			}
		}

		void WriteConstants(FileWriter writer, ConstantsType constantsType) =>
			constantsWriter.Write(writer, constantsType, Array.Empty<string>());
	}
}
