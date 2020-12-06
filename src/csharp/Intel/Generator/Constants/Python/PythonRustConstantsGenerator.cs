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
