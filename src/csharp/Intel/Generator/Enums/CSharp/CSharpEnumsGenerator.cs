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
using Generator.Documentation.CSharp;
using Generator.IO;

namespace Generator.Enums.CSharp {
	sealed class CSharpEnumsGenerator : EnumGenerator {
		readonly Dictionary<EnumKind, FullEnumFileInfo> tooFullFileInfo;
		readonly CSharpDocCommentWriter docWriter;

		sealed class FullEnumFileInfo {
			public readonly string Filename;
			public readonly string Namespace;
			public readonly string? Define;
			public readonly string? BaseType;

			public FullEnumFileInfo(string filename, string @namespace, string? define = null, bool isPublic = false, string? baseType = null) {
				Filename = filename;
				Namespace = @namespace;
				Define = define;
				BaseType = baseType;
			}
		}

		public CSharpEnumsGenerator(ProjectDirs projectDirs) {
			docWriter = new CSharpDocCommentWriter();

			const string decoderDefine = "!NO_DECODER";
			const string instrInfoDefine = "!NO_INSTR_INFO";
			const string decoderOrEncoderDefine = "!NO_DECODER || !NO_ENCODER";
			const string anyFormaterDefine = "(!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER";
			const string ns = "Iced.Intel";
			const string nsDecInt = "Iced.Intel.DecoderInternal";
			const string nsInstrInfoInt = "Iced.Intel.InstructionInfoInternal";

			var baseDir = Path.Combine(projectDirs.CSharpDir, "Intel");
			tooFullFileInfo = new Dictionary<EnumKind, FullEnumFileInfo>();
			tooFullFileInfo.Add(EnumKind.Code, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.Code) + ".g.cs"), ns));
			tooFullFileInfo.Add(EnumKind.CodeSize, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.CodeSize) + ".g.cs"), ns));
			tooFullFileInfo.Add(EnumKind.CpuidFeature, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.CpuidFeature) + ".g.cs"), ns, instrInfoDefine));
			tooFullFileInfo.Add(EnumKind.CpuidFeatureInternal, new FullEnumFileInfo(Path.Combine(baseDir, "InstructionInfoInternal", nameof(EnumKind.CpuidFeatureInternal) + ".g.cs"), nsInstrInfoInt, instrInfoDefine));
			tooFullFileInfo.Add(EnumKind.DecoderOptions, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.DecoderOptions) + ".g.cs"), ns, decoderDefine, baseType: "uint"));
			tooFullFileInfo.Add(EnumKind.EvexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.EvexOpCodeHandlerKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
			tooFullFileInfo.Add(EnumKind.HandlerFlags, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.HandlerFlags) + ".g.cs"), nsDecInt, decoderDefine, baseType: "uint"));
			tooFullFileInfo.Add(EnumKind.LegacyHandlerFlags, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.LegacyHandlerFlags) + ".g.cs"), nsDecInt, decoderDefine, baseType: "uint"));
			tooFullFileInfo.Add(EnumKind.MemorySize, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.MemorySize) + ".g.cs"), ns));
			tooFullFileInfo.Add(EnumKind.OpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.OpCodeHandlerKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
			tooFullFileInfo.Add(EnumKind.PseudoOpsKind, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.PseudoOpsKind) + ".g.cs"), ns, anyFormaterDefine));
			tooFullFileInfo.Add(EnumKind.Register, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.Register) + ".g.cs"), ns));
			tooFullFileInfo.Add(EnumKind.SerializedDataKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.SerializedDataKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
			tooFullFileInfo.Add(EnumKind.TupleType, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.TupleType) + ".g.cs"), ns, decoderOrEncoderDefine));
			tooFullFileInfo.Add(EnumKind.VexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.VexOpCodeHandlerKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
		}

		public override void Generate(EnumType enumType) {
			if (tooFullFileInfo.TryGetValue(enumType.EnumKind, out var fullFileInfo))
				WriteFile(fullFileInfo, enumType);
			else
				throw new InvalidOperationException();
		}

		void WriteFile(FullEnumFileInfo info, EnumType enumType) {
			using (var writer = new FileWriter(FileUtils.OpenWrite(info.Filename))) {
				writer.WriteCSharpHeader();
				if (!(info.Define is null))
					writer.WriteLine($"#if {info.Define}");

				if (enumType.IsFlags) {
					writer.WriteLine("using System;");
					writer.WriteLine();
				}

				writer.WriteLine($"namespace {info.Namespace} {{");

				if (enumType.IsPublic && enumType.IsMissingDocs)
					writer.WriteLine("#pragma warning disable 1591 // Missing XML comment for publicly visible type or member");
				writer.Indent();
				docWriter.Write(writer, enumType.Documentation, enumType.Name);
				if (enumType.IsFlags)
					writer.WriteLine("[Flags]");
				var pub = enumType.IsPublic ? "public " : string.Empty;
				var baseType = !(info.BaseType is null) ? $" : {info.BaseType}" : string.Empty;
				writer.WriteLine($"{pub}enum {enumType.Name}{baseType} {{");

				writer.Indent();
				uint expectedValue = 0;
				foreach (var value in enumType.Values) {
					docWriter.Write(writer, value.Documentation, enumType.Name);
					if (enumType.IsFlags)
						writer.WriteLine($"{value.Name} = 0x{value.Value:X8},");
					else if (expectedValue != value.Value)
						writer.WriteLine($"{value.Name} = {value.Value},");
					else
						writer.WriteLine($"{value.Name},");
					expectedValue = value.Value + 1;
				}
				writer.Unindent();

				writer.WriteLine("}");
				writer.Unindent();
				writer.WriteLine("}");

				if (!(info.Define is null))
					writer.WriteLine("#endif");
			}
		}
	}
}
