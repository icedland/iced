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
using Generator.Constants;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustInstructionMemorySizesGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public RustInstructionMemorySizesGenerator(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var memSizeName = genTypes[TypeIds.MemorySize].Name(idConverter);
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(Path.Combine(generatorContext.RustDir, "instruction_memory_sizes.rs")))) {
				writer.WriteFileHeader();
				writer.WriteLine($"use super::iced_constants::{icedConstants.Name(idConverter)};");
				writer.WriteLine($"use super::{genTypes[TypeIds.MemorySize].Name(idConverter)};");
				writer.WriteLine();
				writer.WriteLine("// 0 = memory size");
				writer.WriteLine("// 1 = broadcast memory size");
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static SIZES: [{memSizeName}; {icedConstants.Name(idConverter)}::{icedConstants[IcedConstants.NumberOfCodeValuesName].Name(idConverter)} * 2] = [");
				using (writer.Indent()) {
					foreach (var def in defs) {
						if (def.Mem.Value > byte.MaxValue)
							throw new InvalidOperationException();
						string value = $"{memSizeName}::{def.Mem.Name(idConverter)}";
						writer.WriteLine($"{value},// {def.OpCodeInfo.Code.Name(idConverter)}");
					}
					foreach (var def in defs) {
						if (def.Bcst.Value > byte.MaxValue)
							throw new InvalidOperationException();
						string value = $"{memSizeName}::{def.Bcst.Name(idConverter)}";
						writer.WriteLine($"{value},// {def.OpCodeInfo.Code.Name(idConverter)}");
					}
				}
				writer.WriteLine("];");
			}
		}
	}
}
