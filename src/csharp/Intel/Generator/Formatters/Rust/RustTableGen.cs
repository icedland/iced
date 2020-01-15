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
using System.IO;
using System.Linq;
using System.Text;
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.Formatter;
using Generator.IO;
using Generator.Tables;

namespace Generator.Formatters.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.FormatterMemSize)]
	sealed class RustTableGen : TableGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;

		public RustTableGen(GeneratorOptions generatorOptions) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
		}

		protected override void Generate((EnumValue memSize, BroadcastToKind bcst, IntelMemoryKeywords intel, MasmMemoryKeywords masm, NasmMemoryKeywords nasm)[] memInfos) {
			{
				var filename = Path.Combine(generatorOptions.RustDir, "formatter", "gas", "mem_size_tbl.rs");
				new FileUpdater(TargetLanguage.Rust, "BcstTo", filename).Generate(writer => {
					int first = (int)IcedConstantsType.Instance[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
					int len = memInfos.Length - first;
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"static BCST_TO_DATA: [BroadcastToKind; {len}] = [");
					using (writer.Indent()) {
						for (int i = first; i < memInfos.Length; i++)
							writer.WriteLine($"BroadcastToKind::{memInfos[i].bcst},");
					}
					writer.WriteLine("];");
				});
			}
			{
				var filename = Path.Combine(generatorOptions.RustDir, "formatter", "intel", "mem_size_tbl.rs");
				new FileUpdater(TargetLanguage.Rust, "MemorySizes", filename).Generate(writer => {
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"static MEM_SIZE_TBL_DATA: [u8; {memInfos.Length}] = [");
					using (writer.Indent()) {
						foreach (var info in memInfos)
							writer.WriteLine($"((MemoryKeywords::{info.intel} as u8) | ((BroadcastToKind::{info.bcst} as u8) << BROADCAST_TO_KIND_SHIFT)),");
					}
					writer.WriteLine("];");
				});
			}
			{
				var filename = Path.Combine(generatorOptions.RustDir, "formatter", "masm", "mem_size_tbl.rs");
				new FileUpdater(TargetLanguage.Rust, "MemorySizes", filename).Generate(writer => {
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"static MEM_SIZE_TBL_DATA: [u16; {memInfos.Length}] = [");
					using (writer.Indent()) {
						var sizeTbl = MemorySizeInfoTable.Data;
						foreach (var info in memInfos) {
							int size = sizeTbl[(int)info.memSize.Value].Size;
							writer.WriteLine($"((MemoryKeywords::{info.masm} as u16) | ((Size::S{size} as u16) << SIZE_KIND_SHIFT)),");
						}
					}
					writer.WriteLine("];");
				});
			}
			{
				var filename = Path.Combine(generatorOptions.RustDir, "formatter", "nasm", "mem_size_tbl.rs");
				new FileUpdater(TargetLanguage.Rust, "BcstTo", filename).Generate(writer => {
					int first = (int)IcedConstantsType.Instance[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
					int len = memInfos.Length - first;
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"static BCST_TO_DATA: [BroadcastToKind; {len}] = [");
					using (writer.Indent()) {
						for (int i = first; i < memInfos.Length; i++)
							writer.WriteLine($"BroadcastToKind::{memInfos[i].bcst},");
					}
					writer.WriteLine("];");
				});
				new FileUpdater(TargetLanguage.Rust, "MemorySizes", filename).Generate(writer => {
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"static MEM_SIZE_TBL_DATA: [u8; {memInfos.Length}] = [");
					using (writer.Indent()) {
						var sizeTbl = MemorySizeInfoTable.Data;
						foreach (var info in memInfos) {
							int size = sizeTbl[(int)info.memSize.Value].Size;
							writer.WriteLine($"((MemoryKeywords::{info.nasm} as u8) | ((Size::S{size} as u8) << SIZE_KIND_SHIFT)),");
						}
					}
					writer.WriteLine("];");
				});
			}
		}

		static string ToString(BroadcastToKind bcst) =>
			bcst switch {
				BroadcastToKind.None => "",
				BroadcastToKind.b1to2 => "1to2",
				BroadcastToKind.b1to4 => "1to4",
				BroadcastToKind.b1to8 => "1to8",
				BroadcastToKind.b1to16 => "1to16",
				_ => throw new InvalidOperationException(),
			};

		protected override void GenerateRegisters(string[] registers) {
			var filename = Path.Combine(generatorOptions.RustDir, "formatter", "regs_tbl.rs");
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
			var filename = Path.Combine(generatorOptions.RustDir, "formatter", "fmt_utils.rs");
			new FileUpdater(TargetLanguage.Rust, "FormatterFlowControlSwitch", filename).Generate(writer => {
				var codeStr = CodeEnum.Instance.Name(idConverter);
				var flowCtrlStr = FormatterFlowControlEnum.Instance.Name(idConverter);
				foreach (var info in infos) {
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
