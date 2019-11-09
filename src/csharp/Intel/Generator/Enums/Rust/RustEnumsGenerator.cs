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

namespace Generator.Enums.Rust {
	sealed class RustEnumsGenerator : IEnumsGenerator {
		readonly RustIdentifierConverter idConverter;
		readonly Dictionary<EnumKind, PartialEnumFileInfo?> toPartialFileInfo;
		readonly RustDocCommentWriter docWriter;

		sealed class PartialEnumFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly string[] Attributes;

			public PartialEnumFileInfo(string id, string filename, string? attribute = null) {
				Id = id;
				Filename = filename;
				Attributes = attribute is null ? Array.Empty<string>() : new string[] { attribute };
			}

			public PartialEnumFileInfo(string id, string filename, string[] attributes) {
				Id = id;
				Filename = filename;
				Attributes = attributes;
			}
		}

		public RustEnumsGenerator(ProjectDirs projectDirs) {
			idConverter = RustIdentifierConverter.Instance;
			docWriter = new RustDocCommentWriter(idConverter);

			const string attrCopyEq = "#[derive(Copy, Clone, Eq, PartialEq)]";
			const string attrCopyEqOrdHash = "#[derive(Copy, Clone, Eq, PartialEq, Ord, PartialOrd, Hash)]";

			toPartialFileInfo = new Dictionary<EnumKind, PartialEnumFileInfo?>();
			toPartialFileInfo.Add(EnumKind.Code, new PartialEnumFileInfo("Code", Path.Combine(projectDirs.RustDir, "common", "code.rs"), new[] { attrCopyEqOrdHash, "#[allow(non_camel_case_types)]", "#[repr(u32)]" }));
			toPartialFileInfo.Add(EnumKind.CodeSize, new PartialEnumFileInfo("CodeSize", Path.Combine(projectDirs.RustDir, "common", "enums.rs"), attrCopyEq));
			toPartialFileInfo.Add(EnumKind.CpuidFeature, null);
			toPartialFileInfo.Add(EnumKind.CpuidFeatureInternal, null);
			toPartialFileInfo.Add(EnumKind.DecoderOptions, null);
			toPartialFileInfo.Add(EnumKind.EvexOpCodeHandlerKind, null);
			toPartialFileInfo.Add(EnumKind.HandlerFlags, null);
			toPartialFileInfo.Add(EnumKind.LegacyHandlerFlags, null);
			toPartialFileInfo.Add(EnumKind.MemorySize, new PartialEnumFileInfo("MemorySize", Path.Combine(projectDirs.RustDir, "common", "memorysize.rs"), new[] { attrCopyEqOrdHash, "#[allow(non_camel_case_types)]" }));
			toPartialFileInfo.Add(EnumKind.OpCodeHandlerKind, null);
			toPartialFileInfo.Add(EnumKind.PseudoOpsKind, null);
			toPartialFileInfo.Add(EnumKind.Register, new PartialEnumFileInfo("Register", Path.Combine(projectDirs.RustDir, "common", "register.rs"), attrCopyEqOrdHash));
			toPartialFileInfo.Add(EnumKind.SerializedDataKind, null);
			toPartialFileInfo.Add(EnumKind.TupleType, null);
			toPartialFileInfo.Add(EnumKind.VexOpCodeHandlerKind, null);
			toPartialFileInfo.Add(EnumKind.Mnemonic, new PartialEnumFileInfo("Mnemonic", Path.Combine(projectDirs.RustDir, "common", "mnemonic.rs"), attrCopyEqOrdHash));
			toPartialFileInfo.Add(EnumKind.GasCtorKind, null);
			toPartialFileInfo.Add(EnumKind.IntelCtorKind, null);
			toPartialFileInfo.Add(EnumKind.MasmCtorKind, null);
			toPartialFileInfo.Add(EnumKind.NasmCtorKind, null);
			toPartialFileInfo.Add(EnumKind.GasSizeOverride, null);
			toPartialFileInfo.Add(EnumKind.GasInstrOpInfoFlags, null);
			toPartialFileInfo.Add(EnumKind.IntelSizeOverride, null);
			toPartialFileInfo.Add(EnumKind.IntelBranchSizeInfo, null);
			toPartialFileInfo.Add(EnumKind.IntelInstrOpInfoFlags, null);
			toPartialFileInfo.Add(EnumKind.MasmInstrOpInfoFlags, null);
			toPartialFileInfo.Add(EnumKind.NasmSignExtendInfo, null);
			toPartialFileInfo.Add(EnumKind.NasmSizeOverride, null);
			toPartialFileInfo.Add(EnumKind.NasmBranchSizeInfo, null);
			toPartialFileInfo.Add(EnumKind.NasmInstrOpInfoFlags, null);

			if (toPartialFileInfo.Count != Enum.GetValues(typeof(EnumKind)).Length)
				throw new InvalidOperationException();
		}

		public void Generate(EnumType enumType) {
			if (toPartialFileInfo.TryGetValue(enumType.EnumKind, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.Rust, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo partialInfo, EnumType enumType) {
			docWriter.Write(writer, enumType.Documentation, enumType.RawName);
			foreach (var attr in partialInfo.Attributes)
				writer.WriteLine(attr);
			if (enumType.IsPublic && enumType.IsMissingDocs)
				writer.WriteLine("#[allow(missing_docs)]");
			var pub = enumType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}enum {enumType.Name(idConverter)} {{");
			writer.Indent();

			uint expectedValue = 0;
			foreach (var value in enumType.Values) {
				docWriter.Write(writer, value.Documentation, enumType.RawName);
				if (expectedValue != value.Value)
					writer.WriteLine($"{value.Name(idConverter)} = {value.Value},");
				else
					writer.WriteLine($"{value.Name(idConverter)},");
				expectedValue = value.Value + 1;
			}

			writer.Unindent();
			writer.WriteLine("}");
		}
	}
}
