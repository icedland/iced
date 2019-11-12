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

using System.IO;
using Generator.Enums;
using Generator.IO;

namespace Generator.Tables.Rust {
	sealed class RustMemorySizeInfoTableGenerator : IMemorySizeInfoTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly ProjectDirs projectDirs;

		public RustMemorySizeInfoTableGenerator(ProjectDirs projectDirs) {
			idConverter = RustIdentifierConverter.Create();
			this.projectDirs = projectDirs;
		}

		public void Generate(MemorySizeInfo[] infos) {
			var filename = Path.Combine(projectDirs.RustDir, "memorysize.rs");
			var updater = new FileUpdater(TargetLanguage.Rust, "MemorySizeInfoTable", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, MemorySizeInfo[] infos) {
			var memSizeName = MemorySizeEnum.Instance.Name(idConverter);
			foreach (var info in infos)
				writer.WriteLine($"MemorySizeInfo {{ size: {info.Size}, element_size: {info.ElementSize}, memory_size: {memSizeName}::{info.MemorySize.Name(idConverter)} as u8, element_type: {memSizeName}::{info.ElementType.Name(idConverter)} as u8, is_signed: {(info.IsSigned ? "true" : "false")}, is_broadcast: {(info.IsBroadcast ? "true" : "false")} }},");
		}
	}
}
