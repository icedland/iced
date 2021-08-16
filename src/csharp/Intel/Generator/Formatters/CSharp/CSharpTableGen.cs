// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.Constants;
using Generator.Enums;
using Generator.IO;
using Generator.Tables;

namespace Generator.Formatters.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpTableGen : TableGen {
		readonly IdentifierConverter idConverter;

		public CSharpTableGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) => idConverter = CSharpIdentifierConverter.Create();

		protected override void Generate(MemorySizeDef[] defs) {
			GenerateFast(defs);
			GenerateGas(defs);
			GenerateIntel(defs);
			GenerateMasm(defs);
			GenerateNasm(defs);
		}

		void GenerateFast(MemorySizeDef[] defs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.FastFormatterNamespace, "MemorySizes.cs");
			new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					writer.WriteByte(checked((byte)def.Fast.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "Switch", filename).Generate(writer => {
				foreach (var kw in defs.Select(a => a.Fast).Distinct().OrderBy(a => a.Value)) {
					var s = (FastMemoryKeywords)kw.Value == FastMemoryKeywords.None ? string.Empty : (kw.RawName + "_").Replace('_', ' ');
					writer.WriteLine($"{kw.Value} => \"{s}\",");
				}
			});
		}

		void GenerateGas(MemorySizeDef[] defs) {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.GasFormatterNamespace, "MemorySizes.cs");
			new FileUpdater(TargetLanguage.CSharp, "ConstData", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine($"var empty = new FormatterString(\"\");");
					else {
						var name = bcst.RawName;
						if (!name.StartsWith("b", StringComparison.Ordinal))
							throw new InvalidOperationException();
						var s = name[1..];
						writer.WriteLine($"var {name} = new FormatterString(\"{s}\");");
					}
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "BcstTo", filename).Generate(writer => {
				int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
				for (int i = first; i < defs.Length; i++) {
					writer.WriteByte(checked((byte)defs[i].BroadcastToKind.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "BroadcastToKindSwitch", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					writer.Write($"0x{bcst.Value:X2} => ");
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine("empty,");
					else
						writer.WriteLine($"{bcst.RawName},");
				}
			});
		}

		void GenerateIntel(MemorySizeDef[] defs) {
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IntelFormatterNamespace, "MemorySizes.cs");
			var intelKeywords = genTypes[TypeIds.IntelMemoryKeywords].Values;
			const int BroadcastToKindShift = 5;
			const int MemoryKeywordsMask = 0x1F;
			new FileUpdater(TargetLanguage.CSharp, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"const int {idConverter.Constant(nameof(BroadcastToKindShift))} = {BroadcastToKindShift};");
				writer.WriteLine($"const int {idConverter.Constant(nameof(MemoryKeywordsMask))} = {MemoryKeywordsMask};");
				var created = new HashSet<string>(StringComparer.Ordinal);
				foreach (var keywords in intelKeywords.Select(a => a.RawName)) {
					if (keywords == nameof(IntelMemoryKeywords.None))
						continue;
					var parts = keywords.Split('_');
					foreach (var kw in parts) {
						if (created.Add(kw))
							writer.WriteLine($"var {EscapeKeyword(kw)} = new FormatterString(\"{kw}\");");
					}
					writer.WriteLine($"var {keywords} = new[] {{ {string.Join(", ", parts.Select(a => EscapeKeyword(a)))} }};");
				}
				foreach (var bcst in broadcastToKindValues) {
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine($"var empty = new FormatterString(\"\");");
					else {
						var name = bcst.RawName;
						if (!name.StartsWith("b", StringComparison.Ordinal))
							throw new InvalidOperationException();
						var s = name[1..];
						writer.WriteLine($"var {name} = new FormatterString(\"{s}\");");
					}
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					uint value = def.Intel.Value | (def.BroadcastToKind.Value << BroadcastToKindShift);
					if (value > 0xFF || def.Intel.Value > MemoryKeywordsMask)
						throw new InvalidOperationException();
					writer.WriteByte(checked((byte)value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "MemoryKeywordsSwitch", filename).Generate(writer => {
				foreach (var kw in intelKeywords) {
					writer.Write($"0x{kw.Value:X2} => ");
					if ((IntelMemoryKeywords)kw.Value == IntelMemoryKeywords.None)
						writer.WriteLine("Array2.Empty<FormatterString>(),");
					else
						writer.WriteLine($"{kw.RawName},");
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "BroadcastToKindSwitch", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					writer.Write($"0x{bcst.Value:X2} => ");
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine("empty,");
					else
						writer.WriteLine($"{bcst.RawName},");
				}
			});
		}

		void GenerateMasm(MemorySizeDef[] defs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.MasmFormatterNamespace, "MemorySizes.cs");
			var masmKeywords = genTypes[TypeIds.MasmMemoryKeywords].Values;
			var sizeToIndex = new Dictionary<uint, uint>();
			uint index = 0;
			foreach (var size in defs.Select(a => a.Size).Distinct().OrderBy(a => a))
				sizeToIndex[size] = index++;
			const int SizeKindShift = 5;
			const int MemoryKeywordsMask = 0x1F;
			new FileUpdater(TargetLanguage.CSharp, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"const int {idConverter.Constant(nameof(SizeKindShift))} = {SizeKindShift};");
				writer.WriteLine($"const int {idConverter.Constant(nameof(MemoryKeywordsMask))} = {MemoryKeywordsMask};");
				var created = new HashSet<string>(StringComparer.Ordinal);
				foreach (var keywords in masmKeywords.Select(a => a.RawName).Concat(new[] { "mmword_ptr" })) {
					if (keywords == nameof(MasmMemoryKeywords.None))
						continue;
					var parts = keywords.Split('_');
					foreach (var kw in parts) {
						if (created.Add(kw))
							writer.WriteLine($"var {EscapeKeyword(kw)} = new FormatterString(\"{kw}\");");
					}
					writer.WriteLine($"var {keywords} = new[] {{ {string.Join(", ", parts.Select(a => EscapeKeyword(a)))} }};");
				}
				writer.WriteLine("var sizes = new ushort[] {");
				using (writer.Indent()) {
					foreach (var size in sizeToIndex.Select(a => a.Key).OrderBy(a => a))
						writer.WriteLine($"{size},");
				}
				writer.WriteLine("};");
			});
			new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					uint value = def.Masm.Value | (sizeToIndex[def.Size] << SizeKindShift);
					if (value > 0xFFFF || def.Masm.Value > MemoryKeywordsMask)
						throw new InvalidOperationException();
					writer.WriteLine($"0x{value:X4},");
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "MemoryKeywordsSwitch", filename).Generate(writer => {
				foreach (var kw in masmKeywords) {
					writer.Write($"0x{kw.Value:X2} => ");
					if ((MasmMemoryKeywords)kw.Value == MasmMemoryKeywords.None)
						writer.WriteLine("Array2.Empty<FormatterString>(),");
					else
						writer.WriteLine($"{kw.RawName},");
				}
			});
		}

		void GenerateNasm(MemorySizeDef[] defs) {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.NasmFormatterNamespace, "MemorySizes.cs");
			var nasmKeywords = genTypes[TypeIds.NasmMemoryKeywords].Values;
			new FileUpdater(TargetLanguage.CSharp, "ConstData", filename).Generate(writer => {
				foreach (var kw in nasmKeywords) {
					if ((NasmMemoryKeywords)kw.Value == NasmMemoryKeywords.None)
						continue;
					writer.WriteLine($"var {EscapeKeyword(kw.RawName)} = new FormatterString(\"{kw.RawName}\");");
				}
				foreach (var bcst in broadcastToKindValues) {
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine($"var empty = new FormatterString(\"\");");
					else {
						var name = bcst.RawName;
						if (!name.StartsWith("b", StringComparison.Ordinal))
							throw new InvalidOperationException();
						var s = name[1..];
						writer.WriteLine($"var {name} = new FormatterString(\"{s}\");");
					}
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					writer.WriteByte(checked((byte)def.Nasm.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "BcstTo", filename).Generate(writer => {
				int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
				for (int i = first; i < defs.Length; i++) {
					writer.WriteByte(checked((byte)defs[i].BroadcastToKind.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "MemoryKeywordsSwitch", filename).Generate(writer => {
				foreach (var kw in nasmKeywords) {
					writer.Write($"0x{kw.Value:X2} => ");
					if ((NasmMemoryKeywords)kw.Value == NasmMemoryKeywords.None)
						writer.WriteLine("empty,");
					else
						writer.WriteLine($"{EscapeKeyword(kw.RawName)},");
				}
			});
			new FileUpdater(TargetLanguage.CSharp, "BroadcastToKindSwitch", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					writer.Write($"0x{bcst.Value:X2} => ");
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine("empty,");
					else
						writer.WriteLine($"{bcst.RawName},");
				}
			});
		}

		static string EscapeKeyword(string s) => s == "byte" ? "@" + s : s;

		protected override void GenerateRegisters(string[] registers) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.FormatterNamespace, "RegistersTable.cs");
			new FileUpdater(TargetLanguage.CSharp, "Registers", filename).Generate(writer => {
				writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
				writer.WriteLine("static ReadOnlySpan<byte> GetRegistersData() =>");
				writer.WriteLineNoIndent("#else");
				writer.WriteLine("static byte[] GetRegistersData() =>");
				writer.WriteLineNoIndent("#endif");
				int maxLen = 0;
				using (writer.Indent()) {
					writer.WriteLine("new byte[] {");
					using (writer.Indent()) {
						foreach (var register in registers) {
							maxLen = Math.Max(maxLen, register.Length);
							var bytes = Encoding.UTF8.GetBytes(register);
							writer.Write($"0x{bytes.Length:X2}");
							foreach (var b in bytes)
								writer.Write($", 0x{b:X2}");
							writer.Write(",");
							writer.WriteCommentLine(register);
						}
					}
					writer.WriteLine("};");
				}
				writer.WriteLine($"const int MaxStringLength = {maxLen};");
			});
		}

		protected override void GenerateFormatterFlowControl((EnumValue flowCtrl, EnumValue[] code)[] infos) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.FormatterNamespace, "FormatterUtils.cs");
			new FileUpdater(TargetLanguage.CSharp, "FormatterFlowControlSwitch", filename).Generate(writer => {
				foreach (var info in infos) {
					if (info.code.Length == 0)
						continue;
					foreach (var c in info.code)
						writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(c)}:");
					using (writer.Indent())
						writer.WriteLine($"return {idConverter.ToDeclTypeAndValue(info.flowCtrl)};");
				}
			});
		}
	}
}
