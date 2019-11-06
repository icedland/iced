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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System.IO;
using Generator.IO;

namespace Generator.Formatters {
	sealed class FormatterTableGenerator {
		readonly ProjectDirs projectDirs;

		public FormatterTableGenerator(ProjectDirs projectDirs) => this.projectDirs = projectDirs;

		public void Generate() {
			var serializers = new FormatterTableSerializer[] {
#if !NO_GAS_FORMATTER && !NO_FORMATTER
				new Gas.GasFormatterTableSerializer(),
#endif
#if !NO_INTEL_FORMATTER && !NO_FORMATTER
				new Intel.IntelFormatterTableSerializer(),
#endif
#if !NO_MASM_FORMATTER && !NO_FORMATTER
				new Masm.MasmFormatterTableSerializer(),
#endif
#if !NO_NASM_FORMATTER && !NO_FORMATTER
				new Nasm.NasmFormatterTableSerializer(),
#endif
			};

			const string @namespace = "Iced.Intel.FormatterInternal";
			const string className = "FormatterStringsTable";
			const string preprocessorExpr = "(!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER";
			var stringsTable = new StringsTableImpl(@namespace, className, preprocessorExpr);

			foreach (var serializer in serializers)
				serializer.Initialize(stringsTable);

			stringsTable.Freeze();

			using (var writer = new FileWriter(FileUtils.OpenWrite(Path.Combine(projectDirs.CSharpDir, "Intel", "FormatterInternal", className + ".g.cs"))))
				stringsTable.Serialize(writer);

			foreach (var serializer in serializers) {
				using (var writer = new FileWriter(FileUtils.OpenWrite(serializer.GetFilename(projectDirs))))
					serializer.Serialize(writer, stringsTable);
			}
		}
	}
}
#endif
