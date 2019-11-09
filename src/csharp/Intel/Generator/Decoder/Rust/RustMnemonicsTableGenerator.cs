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
using Generator.Enums;
using Generator.IO;

namespace Generator.Decoder.Rust {
	sealed class RustMnemonicsTableGenerator : IMnemonicsTableGenerator {
		readonly ProjectDirs projectDirs;

		public RustMnemonicsTableGenerator(ProjectDirs projectDirs) =>
			this.projectDirs = projectDirs;

		public void Generate((EnumValue codeEnum, EnumValue mnemonicEnum)[] data) {
			var mnemonicName = MnemonicEnum.Instance.Name;
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(Path.Combine(projectDirs.RustDir, "common", "mnemonics.rs")))) {
				writer.WriteFileHeader();

				writer.WriteLine($"use super::icedconstants::{IcedConstantsType.Instance.Name};");
				writer.WriteLine($"use super::{MnemonicEnum.Instance.Name};");
				writer.WriteLine();
				writer.WriteLine("#[rustfmt::skip]");
				writer.WriteLine($"pub(crate) static TO_MNEMONIC: &[u16; {IcedConstantsType.Instance.Name}::NUMBER_OF_CODE_VALUES as usize] = &[");
				writer.Indent();

				foreach (var d in data) {
					if (d.mnemonicEnum.Value > ushort.MaxValue)
						throw new InvalidOperationException();
					writer.WriteLine($"{mnemonicName}::{d.mnemonicEnum.Name} as u16,// {d.codeEnum.Name}");
				}

				writer.Unindent();
				writer.WriteLine("];");
			}
		}
	}
}
