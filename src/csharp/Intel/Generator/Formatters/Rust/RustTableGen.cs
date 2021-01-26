// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.Constants;
using Generator.Enums;
using Generator.IO;
using Generator.Tables;

namespace Generator.Formatters.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustTableGen : TableGen {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;

		public RustTableGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = RustIdentifierConverter.Create();
		}

		protected override void Generate(MemorySizeDef[] defs) {
			var fmtConsts1 = new Dictionary<string, string>(StringComparer.Ordinal);
			var fmtConsts2 = new Dictionary<string, string[]>(StringComparer.Ordinal);
			GenerateFast(defs);
			GenerateGas(defs, fmtConsts1);
			GenerateIntel(defs, fmtConsts1, fmtConsts2);
			GenerateMasm(defs, fmtConsts1, fmtConsts2);
			GenerateNasm(defs, fmtConsts1, fmtConsts2);
			GenerateFmtStrings(fmtConsts1, fmtConsts2);
		}

		void GenerateFmtStrings(Dictionary<string, string> fmtConsts1, Dictionary<string, string[]> fmtConsts2) {
			var consts1 = fmtConsts1.OrderBy(a => a.Key, StringComparer.Ordinal).ToArray();
			var consts2 = fmtConsts2.OrderBy(a => a.Key, StringComparer.Ordinal).ToArray();
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "fmt_consts.rs");
			new FileUpdater(TargetLanguage.Rust, "FormatterConstantsDef", filename).Generate(writer => {
				foreach (var kv in consts1)
					writer.WriteLine($"pub(super) {kv.Key}: FormatterString,");
			});
			new FileUpdater(TargetLanguage.Rust, "FormatterConstantsInit", filename).Generate(writer => {
				foreach (var kv in consts1)
					writer.WriteLine($"{kv.Key}: FormatterString::new_str(\"{kv.Value}\"),");
			});
			new FileUpdater(TargetLanguage.Rust, "FormatterArrayConstantsDef", filename).Generate(writer => {
				foreach (var kv in consts2)
					writer.WriteLine($"pub(super) {kv.Key}: [&'static FormatterString; 2],");
			});
			new FileUpdater(TargetLanguage.Rust, "FormatterArrayConstantsCreate", filename).Generate(writer => {
				foreach (var kv in consts2) {
					var strings = kv.Value;
					writer.WriteLine($"let {kv.Key}: [&'static FormatterString; 2] = [{string.Join(", ", strings.Select(a => $"&c.{a}"))}];");
				}
			});
			new FileUpdater(TargetLanguage.Rust, "FormatterArrayConstantsInit", filename).Generate(writer => {
				foreach (var kv in consts2)
					writer.WriteLine($"{kv.Key},");
			});
		}

		static void Add(Dictionary<string, string> fmtConsts1, BroadcastToKind bcst) {
			var s = bcst.ToString();
			if (!s.StartsWith("b", StringComparison.Ordinal))
				throw new InvalidOperationException();
			var value = s[1..];
			fmtConsts1[s] = value;
		}
		static void AddKeywords(Dictionary<string, string> fmtConsts1, Dictionary<string, string[]> fmtConsts2, string name) {
			var parts = name.Split('_');
			if (parts.Length > 2)
				throw new InvalidOperationException();
			foreach (var kw in parts)
				fmtConsts1[kw] = kw;
			if (parts.Length == 2)
				fmtConsts2[name] = parts;
		}

		void GenerateFast(MemorySizeDef[] defs) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "fast", "mem_size_tbl.rs");
			new FileUpdater(TargetLanguage.Rust, "MemorySizes", filename).Generate(writer => {
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static MEM_SIZE_TBL_DATA: [u8; {defs.Length}] = [");
				using (writer.Indent()) {
					foreach (var def in defs) {
						writer.WriteByte(checked((byte)def.Fast.Value));
						writer.WriteLine();
					}
				}
				writer.WriteLine("];");
			});
			new FileUpdater(TargetLanguage.Rust, "Match", filename).Generate(writer => {
				foreach (var kw in defs.Select(a => a.Fast).Distinct().OrderBy(a => a.Value)) {
					var s = (FastMemoryKeywords)kw.Value == FastMemoryKeywords.None ? string.Empty : (kw.RawName + "_").Replace('_', ' ');
					writer.WriteLine($"{kw.Value} => \"{s}\",");
				}
			});
		}

		void GenerateGas(MemorySizeDef[] defs, Dictionary<string, string> fmtConsts1) {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "gas", "mem_size_tbl.rs");
			new FileUpdater(TargetLanguage.Rust, "BcstTo", filename).Generate(writer => {
				int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
				int len = defs.Length - first;
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static BCST_TO_DATA: [u8; {len}] = [");
				using (writer.Indent()) {
					for (int i = first; i < defs.Length; i++) {
						writer.WriteByte(checked((byte)defs[i].BroadcastToKind.Value));
						writer.WriteLine();
					}
				}
				writer.WriteLine("];");
			});
			new FileUpdater(TargetLanguage.Rust, "BroadcastToKindMatch", filename).Generate(writer => {
				foreach (var kw in broadcastToKindValues) {
					writer.Write($"0x{kw.Value:X2} => ");
					var bcst = (BroadcastToKind)kw.Value;
					if (bcst == BroadcastToKind.None)
						writer.WriteLine("&c.empty,");
					else {
						Add(fmtConsts1, bcst);
						writer.WriteLine($"&c.{kw.RawName},");
					}
				}
			});
		}

		void GenerateIntel(MemorySizeDef[] defs, Dictionary<string, string> fmtConsts1, Dictionary<string, string[]> fmtConsts2) {
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "intel", "mem_size_tbl.rs");
			var intelKeywords = genTypes[TypeIds.IntelMemoryKeywords].Values;
			const int BroadcastToKindShift = 5;
			const int MemoryKeywordsMask = 0x1F;
			new FileUpdater(TargetLanguage.Rust, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"const {idConverter.Constant(nameof(BroadcastToKindShift))}: u32 = {BroadcastToKindShift};");
				writer.WriteLine($"const {idConverter.Constant(nameof(MemoryKeywordsMask))}: u8 = {MemoryKeywordsMask};");
			});
			new FileUpdater(TargetLanguage.Rust, "MemorySizes", filename).Generate(writer => {
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static MEM_SIZE_TBL_DATA: [u8; {defs.Length}] = [");
				using (writer.Indent()) {
					foreach (var def in defs) {
						uint value = def.Intel.Value | (def.BroadcastToKind.Value << BroadcastToKindShift);
						if (value > 0xFF || def.Intel.Value > MemoryKeywordsMask)
							throw new InvalidOperationException();
						writer.WriteByte(checked((byte)value));
						writer.WriteLine();
					}
				}
				writer.WriteLine("];");
			});
			new FileUpdater(TargetLanguage.Rust, "MemoryKeywordsMatch", filename).Generate(writer => {
				foreach (var kw in intelKeywords) {
					writer.Write($"0x{kw.Value:X2} => ");
					if ((IntelMemoryKeywords)kw.Value == IntelMemoryKeywords.None)
						writer.WriteLine("&ac.nothing,");
					else {
						AddKeywords(fmtConsts1, fmtConsts2, kw.RawName);
						writer.WriteLine($"&ac.{kw.RawName},");
					}
				}
			});
			new FileUpdater(TargetLanguage.Rust, "BroadcastToKindMatch", filename).Generate(writer => {
				foreach (var kw in broadcastToKindValues) {
					writer.Write($"0x{kw.Value:X2} => ");
					var bcst = (BroadcastToKind)kw.Value;
					if (bcst == BroadcastToKind.None)
						writer.WriteLine("&c.empty,");
					else {
						Add(fmtConsts1, bcst);
						writer.WriteLine($"&c.{kw.RawName},");
					}
				}
			});
		}

		void GenerateMasm(MemorySizeDef[] defs, Dictionary<string, string> fmtConsts1, Dictionary<string, string[]> fmtConsts2) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "masm", "mem_size_tbl.rs");
			var masmKeywords = genTypes[TypeIds.MasmMemoryKeywords].Values;
			var sizeToIndex = new Dictionary<uint, uint>();
			uint index = 0;
			foreach (var size in defs.Select(a => a.Size).Distinct().OrderBy(a => a))
				sizeToIndex[size] = index++;
			const int SizeKindShift = 5;
			const int MemoryKeywordsMask = 0x1F;
			new FileUpdater(TargetLanguage.CSharp, "ConstData", filename).Generate(writer => {
				writer.WriteLine($"const {idConverter.Constant(nameof(SizeKindShift))}: u32 = {SizeKindShift};");
				writer.WriteLine($"const {idConverter.Constant(nameof(MemoryKeywordsMask))}: u16 = {MemoryKeywordsMask};");
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static SIZES: [u16; {sizeToIndex.Count}] = [");
				using (writer.Indent()) {
					foreach (var size in sizeToIndex.Select(a => a.Key).OrderBy(a => a))
						writer.WriteLine($"{size},");
				}
				writer.WriteLine("];");
			});
			new FileUpdater(TargetLanguage.Rust, "MemorySizes", filename).Generate(writer => {
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static MEM_SIZE_TBL_DATA: [u16; {defs.Length}] = [");
				using (writer.Indent()) {
					foreach (var def in defs) {
						uint value = def.Masm.Value | (sizeToIndex[def.Size] << SizeKindShift);
						if (value > 0xFFFF || def.Masm.Value > MemoryKeywordsMask)
							throw new InvalidOperationException();
						writer.WriteLine($"0x{value:X4},");
					}
				}
				writer.WriteLine("];");
			});
			new FileUpdater(TargetLanguage.Rust, "MemoryKeywordsMatch", filename).Generate(writer => {
				foreach (var kw in masmKeywords) {
					writer.Write($"0x{kw.Value:X2} => ");
					if ((MasmMemoryKeywords)kw.Value == MasmMemoryKeywords.None)
						writer.WriteLine("&ac.nothing,");
					else {
						AddKeywords(fmtConsts1, fmtConsts2, kw.RawName);
						writer.WriteLine($"&ac.{kw.RawName},");
					}
				}
			});
			AddKeywords(fmtConsts1, fmtConsts2, "mmword_ptr");
		}

		void GenerateNasm(MemorySizeDef[] defs, Dictionary<string, string> fmtConsts1, Dictionary<string, string[]> fmtConsts2) {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var broadcastToKindValues = genTypes[TypeIds.BroadcastToKind].Values;
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "nasm", "mem_size_tbl.rs");
			var nasmKeywords = genTypes[TypeIds.NasmMemoryKeywords].Values;
			new FileUpdater(TargetLanguage.Rust, "BcstTo", filename).Generate(writer => {
				int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
				int len = defs.Length - first;
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static BCST_TO_DATA: [u8; {len}] = [");
				using (writer.Indent()) {
					for (int i = first; i < defs.Length; i++) {
						writer.WriteByte(checked((byte)defs[i].BroadcastToKind.Value));
						writer.WriteLine();
					}
				}
				writer.WriteLine("];");
			});
			new FileUpdater(TargetLanguage.Rust, "MemorySizes", filename).Generate(writer => {
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static MEM_SIZE_TBL_DATA: [u8; {defs.Length}] = [");
				using (writer.Indent()) {
					foreach (var def in defs) {
						writer.WriteByte(checked((byte)def.Nasm.Value));
						writer.WriteLine();
					}
				}
				writer.WriteLine("];");
			});
			new FileUpdater(TargetLanguage.Rust, "MemoryKeywordsMatch", filename).Generate(writer => {
				foreach (var kw in nasmKeywords) {
					writer.Write($"0x{kw.Value:X2} => ");
					if ((NasmMemoryKeywords)kw.Value == NasmMemoryKeywords.None)
						writer.WriteLine("&c.empty,");
					else {
						AddKeywords(fmtConsts1, fmtConsts2, kw.RawName);
						writer.WriteLine($"&c.{kw.RawName},");
					}
				}
			});
			new FileUpdater(TargetLanguage.Rust, "BroadcastToKindMatch", filename).Generate(writer => {
				foreach (var kw in broadcastToKindValues) {
					writer.Write($"0x{kw.Value:X2} => ");
					var bcst = (BroadcastToKind)kw.Value;
					if (bcst == BroadcastToKind.None)
						writer.WriteLine("&c.empty,");
					else {
						Add(fmtConsts1, bcst);
						writer.WriteLine($"&c.{kw.RawName},");
					}
				}
			});
		}

		protected override void GenerateRegisters(string[] registers) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "regs_tbl.rs");
			new FileUpdater(TargetLanguage.Rust, "Registers", filename).Generate(writer => {
				var totalLen = registers.Length + registers.Sum(a => a.Length);
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static REGS_DATA: [u8; {totalLen}] = [");
				int maxLen = 0;
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
				writer.WriteLine("];");
				writer.WriteLine($"pub(super) const MAX_STRING_LENGTH: usize = {maxLen};");
				writer.WriteLine($"const STRINGS_COUNT: usize = {registers.Length};");
			});
		}

		protected override void GenerateFormatterFlowControl((EnumValue flowCtrl, EnumValue[] code)[] infos) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("formatter", "fmt_utils.rs");
			new FileUpdater(TargetLanguage.Rust, "FormatterFlowControlSwitch", filename).Generate(writer => {
				var codeStr = genTypes[TypeIds.Code].Name(idConverter);
				var flowCtrlStr = genTypes[TypeIds.FormatterFlowControl].Name(idConverter);
				foreach (var info in infos) {
					if (info.code.Length == 0)
						continue;
					var bar = string.Empty;
					foreach (var c in info.code) {
						writer.WriteLine($"{bar}{codeStr}::{c.Name(idConverter)}");
						bar = "| ";
					}
					writer.WriteLine($"=> {flowCtrlStr}::{info.flowCtrl.Name(idConverter)},");
				}
			});
		}
	}
}
