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
using Generator.Constants;
using Generator.Constants.Rust;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.Enums.Rust;
using Generator.IO;

namespace Generator.InstructionInfo.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.InstrInfo)]
	sealed class RustInstrInfoGenerator : InstrInfoGenerator {
		readonly IdentifierConverter idConverter;
		readonly RustEnumsGenerator enumGenerator;
		readonly RustConstantsGenerator constantsGenerator;
		readonly GeneratorContext generatorContext;

		public RustInstrInfoGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			enumGenerator = new RustEnumsGenerator(generatorContext);
			constantsGenerator = new RustConstantsGenerator(generatorContext);
			this.generatorContext = generatorContext;
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);
		protected override void Generate(ConstantsType constantsType) => constantsGenerator.Generate(constantsType);

		protected override void Generate((InstrInfo info, uint dword1, uint dword2)[] infos) {
			var filename = Path.Combine(generatorContext.RustDir, "info", "info_table.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(crate) static TABLE: [u32; {infos.Length * 2}] = [");
				using (writer.Indent()) {
					foreach (var info in infos)
						writer.WriteLine($"{NumberFormatter.FormatHexUInt32WithSep(info.dword1)}, {NumberFormatter.FormatHexUInt32WithSep(info.dword2)},// {info.info.Code.Name(idConverter)}");
				}
				writer.WriteLine("];");
			}
		}

		protected override void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified) {
			var filename = Path.Combine(generatorContext.RustDir, "info", "rflags_table.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				var infos = new (RflagsBits[] rflags, string name)[] {
					(read, "read"),
					(undefined, "undefined"),
					(written, "written"),
					(cleared, "cleared"),
					(set, "set"),
					(modified, "modified"),
				};
				foreach (var info in infos) {
					var rflags = info.rflags;
					if (rflags.Length != infos[0].rflags.Length)
						throw new InvalidOperationException();
					var name = idConverter.Static("flags" + info.name.Substring(0, 1).ToUpperInvariant() + info.name.Substring(1));
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"pub(crate) static {name}: [u16; {rflags.Length}] = [");
					using (writer.Indent()) {
						for (int i = 0; i < rflags.Length; i++) {
							var rfl = rflags[i];
							uint value = (uint)rfl;
							if (value > ushort.MaxValue)
								throw new InvalidOperationException();
							writer.WriteLine($"{NumberFormatter.FormatHexUInt32WithSep(value)},// {enumValues[i].Name(idConverter)}");
						}
					}
					writer.WriteLine("];");
				}
			}
		}

		protected override void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures) {
			var filename = Path.Combine(generatorContext.RustDir, "info", "cpuid_table.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				var cpuidFeatureTypeStr = genTypes[TypeIds.CpuidFeature].Name(idConverter);
				writer.WriteLine($"use super::super::{cpuidFeatureTypeStr};");
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(crate) static CPUID: [&[{cpuidFeatureTypeStr}]; {cpuidFeatures.Length}] = [");
				using (writer.Indent()) {
					foreach (var info in cpuidFeatures)
						writer.WriteLine($"&[{string.Join(", ", info.cpuidFeatures.Select(a => $"{cpuidFeatureTypeStr}::{a.Name(idConverter)}"))}],// {info.cpuidInternal.Name(idConverter)}");
				}
				writer.WriteLine("];");
			}
		}

		protected override void GenerateCore() => GenerateOpAccesses();

		void GenerateOpAccesses() {
			var filename = Path.Combine(generatorContext.RustDir, "info", "enums.rs");
			new FileUpdater(TargetLanguage.Rust, "OpAccesses", filename).Generate(writer => GenerateOpAccesses(writer));
		}

		void GenerateOpAccesses(FileWriter writer) {
			var opInfos = instrInfoTypes.EnumOpInfos;
			// We assume max op count is 5, update the code if not
			if (opInfos.Length != 5)
				throw new InvalidOperationException();

			var indexes = new int[] { 1, 2 };
			var opAccessTypeStr = genTypes[TypeIds.OpAccess].Name(idConverter);
			foreach (var index in indexes) {
				var opInfo = opInfos[index];
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				var name = idConverter.Constant($"OpAccess_{index}");
				writer.WriteLine($"pub(super) static {name}: [{opAccessTypeStr}; {opInfo.Values.Length}] = [");
				using (writer.Indent()) {
					foreach (var value in opInfo.Values) {
						var v = ToOpAccess(value);
						writer.WriteLine($"{opAccessTypeStr}::{v.Name(idConverter)},");
					}
				}
				writer.WriteLine("];");
			}
		}
	}
}
