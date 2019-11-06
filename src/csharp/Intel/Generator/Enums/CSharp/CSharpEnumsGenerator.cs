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
		readonly Dictionary<EnumKind, FullEnumFileInfo> toFullFileInfo;
		readonly CSharpDocCommentWriter docWriter;

		sealed class FullEnumFileInfo {
			public readonly string Filename;
			public readonly string Namespace;
			public readonly string? Define;
			public readonly string? BaseType;

			public FullEnumFileInfo(string filename, string @namespace, string? define = null, string? baseType = null) {
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
			const string ns = "Iced.Intel";
			const string nsDecInt = "Iced.Intel.DecoderInternal";
			const string nsInstrInfoInt = "Iced.Intel.InstructionInfoInternal";
			const string formatterDefine = "(!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER";
			const string gasFormatterDefine = "!NO_GAS_FORMATTER && !NO_FORMATTER";
			const string intelFormatterDefine = "!NO_INTEL_FORMATTER && !NO_FORMATTER";
			const string masmFormatterDefine = "!NO_MASM_FORMATTER && !NO_FORMATTER";
			const string nasmFormatterDefine = "!NO_NASM_FORMATTER && !NO_FORMATTER";
			const string nsFormatter = "Iced.Intel.FormatterInternal";
			const string nsFormatterGas = "Iced.Intel.GasFormatterInternal";
			const string nsFormatterIntel = "Iced.Intel.IntelFormatterInternal";
			const string nsFormatterMasm = "Iced.Intel.MasmFormatterInternal";
			const string nsFormatterNasm = "Iced.Intel.NasmFormatterInternal";

			var baseDir = Path.Combine(projectDirs.CSharpDir, "Intel");
			toFullFileInfo = new Dictionary<EnumKind, FullEnumFileInfo>();
			toFullFileInfo.Add(EnumKind.Code, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.Code) + ".g.cs"), ns));
			toFullFileInfo.Add(EnumKind.CodeSize, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.CodeSize) + ".g.cs"), ns));
			toFullFileInfo.Add(EnumKind.CpuidFeature, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.CpuidFeature) + ".g.cs"), ns, instrInfoDefine));
			toFullFileInfo.Add(EnumKind.CpuidFeatureInternal, new FullEnumFileInfo(Path.Combine(baseDir, "InstructionInfoInternal", nameof(EnumKind.CpuidFeatureInternal) + ".g.cs"), nsInstrInfoInt, instrInfoDefine));
			toFullFileInfo.Add(EnumKind.DecoderOptions, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.DecoderOptions) + ".g.cs"), ns, decoderDefine, baseType: "uint"));
			toFullFileInfo.Add(EnumKind.EvexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.EvexOpCodeHandlerKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.HandlerFlags, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.HandlerFlags) + ".g.cs"), nsDecInt, decoderDefine, baseType: "uint"));
			toFullFileInfo.Add(EnumKind.LegacyHandlerFlags, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.LegacyHandlerFlags) + ".g.cs"), nsDecInt, decoderDefine, baseType: "uint"));
			toFullFileInfo.Add(EnumKind.MemorySize, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.MemorySize) + ".g.cs"), ns));
			toFullFileInfo.Add(EnumKind.OpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.OpCodeHandlerKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.PseudoOpsKind, new FullEnumFileInfo(Path.Combine(baseDir, "FormatterInternal", nameof(EnumKind.PseudoOpsKind) + ".g.cs"), nsFormatter, formatterDefine));
			toFullFileInfo.Add(EnumKind.Register, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.Register) + ".g.cs"), ns));
			toFullFileInfo.Add(EnumKind.SerializedDataKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.SerializedDataKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.TupleType, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.TupleType) + ".g.cs"), ns, decoderOrEncoderDefine));
			toFullFileInfo.Add(EnumKind.VexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(baseDir, "DecoderInternal", nameof(EnumKind.VexOpCodeHandlerKind) + ".g.cs"), nsDecInt, decoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.Mnemonic, new FullEnumFileInfo(Path.Combine(baseDir, nameof(EnumKind.Mnemonic) + ".g.cs"), ns));
			toFullFileInfo.Add(EnumKind.GasCtorKind, new FullEnumFileInfo(Path.Combine(baseDir, "GasFormatterInternal", "CtorKind.g.cs"), nsFormatterGas, gasFormatterDefine));
			toFullFileInfo.Add(EnumKind.IntelCtorKind, new FullEnumFileInfo(Path.Combine(baseDir, "IntelFormatterInternal", "CtorKind.g.cs"), nsFormatterIntel, intelFormatterDefine));
			toFullFileInfo.Add(EnumKind.MasmCtorKind, new FullEnumFileInfo(Path.Combine(baseDir, "MasmFormatterInternal", "CtorKind.g.cs"), nsFormatterMasm, masmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmCtorKind, new FullEnumFileInfo(Path.Combine(baseDir, "NasmFormatterInternal", "CtorKind.g.cs"), nsFormatterNasm, nasmFormatterDefine));

			toFullFileInfo.Add(EnumKind.GasSizeOverride, new FullEnumFileInfo(Path.Combine(baseDir, "GasFormatterInternal", "SizeOverride.g.cs"), nsFormatterGas, gasFormatterDefine));
			toFullFileInfo.Add(EnumKind.GasInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(baseDir, "GasFormatterInternal", "InstrOpInfoFlags.g.cs"), nsFormatterGas, gasFormatterDefine, "ushort"));

			toFullFileInfo.Add(EnumKind.IntelSizeOverride, new FullEnumFileInfo(Path.Combine(baseDir, "IntelFormatterInternal", "SizeOverride.g.cs"), nsFormatterIntel, intelFormatterDefine));
			toFullFileInfo.Add(EnumKind.IntelBranchSizeInfo, new FullEnumFileInfo(Path.Combine(baseDir, "IntelFormatterInternal", "BranchSizeInfo.g.cs"), nsFormatterIntel, intelFormatterDefine));
			toFullFileInfo.Add(EnumKind.IntelInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(baseDir, "IntelFormatterInternal", "InstrOpInfoFlags.g.cs"), nsFormatterIntel, intelFormatterDefine, "ushort"));

			toFullFileInfo.Add(EnumKind.MasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(baseDir, "MasmFormatterInternal", "InstrOpInfoFlags.g.cs"), nsFormatterMasm, masmFormatterDefine, "ushort"));

			toFullFileInfo.Add(EnumKind.NasmSignExtendInfo, new FullEnumFileInfo(Path.Combine(baseDir, "NasmFormatterInternal", "SignExtendInfo.g.cs"), nsFormatterNasm, nasmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmSizeOverride, new FullEnumFileInfo(Path.Combine(baseDir, "NasmFormatterInternal", "SizeOverride.g.cs"), nsFormatterNasm, nasmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmBranchSizeInfo, new FullEnumFileInfo(Path.Combine(baseDir, "NasmFormatterInternal", "BranchSizeInfo.g.cs"), nsFormatterNasm, nasmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(baseDir, "NasmFormatterInternal", "InstrOpInfoFlags.g.cs"), nsFormatterNasm, nasmFormatterDefine, "uint"));
		}

		public override void Generate(EnumType enumType) {
			if (toFullFileInfo.TryGetValue(enumType.EnumKind, out var fullFileInfo))
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
