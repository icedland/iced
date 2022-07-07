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

namespace Generator.Formatters.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaTableGen : TableGen {
		readonly IdentifierConverter idConverter;

		public JavaTableGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) => idConverter = JavaIdentifierConverter.Create();

		protected override void Generate(MemorySizeDef[] defs) {
			GenerateFast(defs);
			GenerateGas(defs);
			GenerateIntel(defs);
			GenerateMasm(defs);
			GenerateNasm(defs);
		}

		void GenerateFast(MemorySizeDef[] defs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.FastFormatterPackage, "MemorySizes.java");
			new FileUpdater(TargetLanguage.Java, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					writer.WriteByte(checked((byte)def.Fast.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.Java, "Switch", filename).Generate(writer => {
				foreach (var kw in defs.Select(a => a.Fast).Distinct().OrderBy(a => a.Value)) {
					var s = (FastMemoryKeywords)kw.Value == FastMemoryKeywords.None ? string.Empty : (kw.RawName + "_").Replace('_', ' ');
					writer.WriteLine($"case {kw.Value}:");
					using (writer.Indent()) {
						writer.WriteLine($"keywords = \"{s}\";");
						writer.WriteLine("break;");
					}
				}
			});
		}

		void GenerateGas(MemorySizeDef[] defs) {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.GasFormatterPackage, "MemorySizes.java");
			new FileUpdater(TargetLanguage.Java, "ConstData", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine($"FormatterString empty = new FormatterString(\"\");");
					else {
						var name = bcst.RawName;
						if (!name.StartsWith("b", StringComparison.Ordinal))
							throw new InvalidOperationException();
						var s = name[1..];
						writer.WriteLine($"FormatterString {name} = new FormatterString(\"{s}\");");
					}
				}
			});
			new FileUpdater(TargetLanguage.Java, "BcstTo", filename).Generate(writer => {
				int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
				for (int i = first; i < defs.Length; i++) {
					writer.WriteByte(checked((byte)defs[i].BroadcastToKind.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.Java, "BroadcastToKindSwitch", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					writer.WriteLine($"case 0x{bcst.Value:X2}:");
					using (writer.Indent()) {
						if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
							writer.WriteLine("bcstTo = empty;");
						else
							writer.WriteLine($"bcstTo = {bcst.RawName};");
						writer.WriteLine("break;");
					}
				}
			});
		}

		void GenerateIntel(MemorySizeDef[] defs) {
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IntelFormatterPackage, "MemorySizes.java");
			var intelKeywords = genTypes[TypeIds.IntelMemoryKeywords].Values;
			const int BroadcastToKindShift = 5;
			const int MemoryKeywordsMask = 0x1F;
			new FileUpdater(TargetLanguage.Java, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"final int {idConverter.Constant(nameof(BroadcastToKindShift))} = {BroadcastToKindShift};");
				writer.WriteLine($"final int {idConverter.Constant(nameof(MemoryKeywordsMask))} = {MemoryKeywordsMask};");
				var created = new HashSet<string>(StringComparer.Ordinal);
				foreach (var keywords in intelKeywords.Select(a => a.RawName)) {
					if (keywords == nameof(IntelMemoryKeywords.None))
						continue;
					var parts = keywords.Split('_');
					foreach (var kw in parts) {
						if (created.Add(kw))
							writer.WriteLine($"FormatterString {EscapeKeyword(kw)} = new FormatterString(\"{kw}\");");
					}
					writer.WriteLine($"FormatterString[] {keywords} = new FormatterString[] {{ {string.Join(", ", parts.Select(a => EscapeKeyword(a)))} }};");
				}
				foreach (var bcst in broadcastToKindValues) {
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine($"FormatterString empty = new FormatterString(\"\");");
					else {
						var name = bcst.RawName;
						if (!name.StartsWith("b", StringComparison.Ordinal))
							throw new InvalidOperationException();
						var s = name[1..];
						writer.WriteLine($"FormatterString {name} = new FormatterString(\"{s}\");");
					}
				}
			});
			new FileUpdater(TargetLanguage.Java, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					uint value = def.Intel.Value | (def.BroadcastToKind.Value << BroadcastToKindShift);
					if (value > 0xFF || def.Intel.Value > MemoryKeywordsMask)
						throw new InvalidOperationException();
					writer.WriteByte(checked((byte)value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.Java, "MemoryKeywordsSwitch", filename).Generate(writer => {
				foreach (var kw in intelKeywords) {
					writer.WriteLine($"case 0x{kw.Value:X2}:");
					using (writer.Indent()) {
						if ((IntelMemoryKeywords)kw.Value == IntelMemoryKeywords.None)
							writer.WriteLine("keywords = new FormatterString[0];");
						else
							writer.WriteLine($"keywords = {kw.RawName};");
						writer.WriteLine("break;");
					}
				}
			});
			new FileUpdater(TargetLanguage.Java, "BroadcastToKindSwitch", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					writer.WriteLine($"case 0x{bcst.Value:X2}:");
					using (writer.Indent()) {
						if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
							writer.WriteLine("bcstTo = empty;");
						else
							writer.WriteLine($"bcstTo = {bcst.RawName};");
						writer.WriteLine("break;");
					}
				}
			});
		}

		void GenerateMasm(MemorySizeDef[] defs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.MasmFormatterPackage, "MemorySizes.java");
			var masmKeywords = genTypes[TypeIds.MasmMemoryKeywords].Values;
			var sizeToIndex = new Dictionary<uint, uint>();
			uint index = 0;
			foreach (var size in defs.Select(a => a.Size).Distinct().OrderBy(a => a))
				sizeToIndex[size] = index++;
			const int SizeKindShift = 5;
			const int MemoryKeywordsMask = 0x1F;
			new FileUpdater(TargetLanguage.Java, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"final int {idConverter.Constant(nameof(SizeKindShift))} = {SizeKindShift};");
				writer.WriteLine($"final int {idConverter.Constant(nameof(MemoryKeywordsMask))} = {MemoryKeywordsMask};");
				var created = new HashSet<string>(StringComparer.Ordinal);
				foreach (var keywords in masmKeywords.Select(a => a.RawName).Concat(new[] { "mmword_ptr" })) {
					if (keywords == nameof(MasmMemoryKeywords.None))
						continue;
					var parts = keywords.Split('_');
					foreach (var kw in parts) {
						if (created.Add(kw))
							writer.WriteLine($"FormatterString {EscapeKeyword(kw)} = new FormatterString(\"{kw}\");");
					}
					writer.WriteLine($"FormatterString[] {Escape(keywords)} = new FormatterString[] {{ {string.Join(", ", parts.Select(a => EscapeKeyword(a)))} }};");
				}
				writer.WriteLine("short[] sizes = new short[] {");
				using (writer.Indent()) {
					foreach (var size in sizeToIndex.Select(a => a.Key).OrderBy(a => a))
						writer.WriteLine($"(short){size},");
				}
				writer.WriteLine("};");
			});
			new FileUpdater(TargetLanguage.Java, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					uint value = def.Masm.Value | (sizeToIndex[def.Size] << SizeKindShift);
					if (value > 0xFFFF || def.Masm.Value > MemoryKeywordsMask)
						throw new InvalidOperationException();
					writer.WriteLine($"(short)0x{value:X4},");
				}
			});
			new FileUpdater(TargetLanguage.Java, "MemoryKeywordsSwitch", filename).Generate(writer => {
				foreach (var kw in masmKeywords) {
					writer.WriteLine($"case 0x{kw.Value:X2}:");
					using (writer.Indent()) {
						if ((MasmMemoryKeywords)kw.Value == MasmMemoryKeywords.None)
							writer.WriteLine("keywords = new FormatterString[0];");
						else
							writer.WriteLine($"keywords = {Escape(kw.RawName)};");
						writer.WriteLine("break;");
					}
				}
			});
		}

		void GenerateNasm(MemorySizeDef[] defs) {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.NasmFormatterPackage, "MemorySizes.java");
			var nasmKeywords = genTypes[TypeIds.NasmMemoryKeywords].Values;
			new FileUpdater(TargetLanguage.Java, "ConstData", filename).Generate(writer => {
				foreach (var kw in nasmKeywords) {
					if ((NasmMemoryKeywords)kw.Value == NasmMemoryKeywords.None)
						continue;
					writer.WriteLine($"FormatterString {EscapeKeyword(kw.RawName)} = new FormatterString(\"{kw.RawName}\");");
				}
				foreach (var bcst in broadcastToKindValues) {
					if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
						writer.WriteLine($"FormatterString empty = new FormatterString(\"\");");
					else {
						var name = bcst.RawName;
						if (!name.StartsWith("b", StringComparison.Ordinal))
							throw new InvalidOperationException();
						var s = name[1..];
						writer.WriteLine($"FormatterString {name} = new FormatterString(\"{s}\");");
					}
				}
			});
			new FileUpdater(TargetLanguage.Java, "MemorySizes", filename).Generate(writer => {
				foreach (var def in defs) {
					writer.WriteByte(checked((byte)def.Nasm.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.Java, "BcstTo", filename).Generate(writer => {
				int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
				for (int i = first; i < defs.Length; i++) {
					writer.WriteByte(checked((byte)defs[i].BroadcastToKind.Value));
					writer.WriteLine();
				}
			});
			new FileUpdater(TargetLanguage.Java, "MemoryKeywordsSwitch", filename).Generate(writer => {
				foreach (var kw in nasmKeywords) {
					writer.WriteLine($"case 0x{kw.Value:X2}:");
					using (writer.Indent()) {
						if ((NasmMemoryKeywords)kw.Value == NasmMemoryKeywords.None)
							writer.WriteLine("keyword = empty;");
						else
							writer.WriteLine($"keyword = {EscapeKeyword(kw.RawName)};");
						writer.WriteLine("break;");
					}
				}
			});
			new FileUpdater(TargetLanguage.Java, "BroadcastToKindSwitch", filename).Generate(writer => {
				foreach (var bcst in broadcastToKindValues) {
					writer.WriteLine($"case 0x{bcst.Value:X2}:");
					using (writer.Indent()) {
						if ((BroadcastToKind)bcst.Value == BroadcastToKind.None)
							writer.WriteLine("bcstTo = empty;");
						else
							writer.WriteLine($"bcstTo = {bcst.RawName};");
						writer.WriteLine("break;");
					}
				}
			});
		}

		static string EscapeKeyword(string s) => s == "byte" ? s + "_" : s;
		static string Escape(string s) => s + "_";

		protected override void GenerateRegisters(string[] registers) {
			const string className = "RegistersTable";

			var rsrcFilename = JavaConstants.GetResourceFilename(genTypes, JavaConstants.FormatterPackage, className + ".bin");
			int maxLen = 0;
			using (var writer = new BinaryByteTableWriter(rsrcFilename)) {
				foreach (var register in registers) {
					maxLen = Math.Max(maxLen, register.Length);
					var bytes = Encoding.UTF8.GetBytes(register);
					writer.WriteByte(checked((byte)bytes.Length));
					foreach (var b in bytes)
						writer.WriteByte(b);
				}
			}

			var srcFilename = JavaConstants.GetFilename(genTypes, JavaConstants.FormatterInternalPackage, className + ".java");
			new FileUpdater(TargetLanguage.Java, "Registers", srcFilename).Generate(writer => {
				writer.WriteLine($"private static final int MAX_STRING_LENGTH = {maxLen};");
			});
		}

		protected override void GenerateFormatterFlowControl((EnumValue flowCtrl, EnumValue[] code)[] infos) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.FormatterInternalPackage, "FormatterUtils.java");
			new FileUpdater(TargetLanguage.Java, "FormatterFlowControlSwitch", filename).Generate(writer => {
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
