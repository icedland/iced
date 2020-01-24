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

using System.Collections.Generic;
using System.IO;
using Generator.IO;

namespace Generator.Formatters.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Formatter_Table)]
	sealed class CSharpFormatterTableGenerator {
		readonly GeneratorOptions generatorOptions;

		public CSharpFormatterTableGenerator(GeneratorOptions generatorOptions) =>
			this.generatorOptions = generatorOptions;

		public void Generate() {
			var serializers = new List<CSharpFormatterTableSerializer>();
			if (generatorOptions.HasGasFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(Gas.CtorInfos.Infos, Enums.Formatter.Gas.CtorKindEnum.Instance, CSharpConstants.GasFormatterDefine, CSharpConstants.GasFormatterNamespace));
			if (generatorOptions.HasIntelFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(Intel.CtorInfos.Infos, Enums.Formatter.Intel.CtorKindEnum.Instance, CSharpConstants.IntelFormatterDefine, CSharpConstants.IntelFormatterNamespace));
			if (generatorOptions.HasMasmFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(Masm.CtorInfos.Infos, Enums.Formatter.Masm.CtorKindEnum.Instance, CSharpConstants.MasmFormatterDefine, CSharpConstants.MasmFormatterNamespace));
			if (generatorOptions.HasNasmFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(Nasm.CtorInfos.Infos, Enums.Formatter.Nasm.CtorKindEnum.Instance, CSharpConstants.NasmFormatterDefine, CSharpConstants.NasmFormatterNamespace));

			var stringsTable = new StringsTable();

			foreach (var serializer in serializers)
				serializer.Initialize(stringsTable);

			stringsTable.Freeze();

			const string FormatterStringsTableName = "FormatterStringsTable";
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.FormatterNamespace), FormatterStringsTableName + ".g.cs")))) {
				var serializer = new CSharpStringsTableSerializer(stringsTable, CSharpConstants.FormatterNamespace, FormatterStringsTableName, CSharpConstants.AnyFormatterDefine);
				serializer.Serialize(writer);
			}

			foreach (var serializer in serializers) {
				using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(serializer.GetFilename(generatorOptions))))
					serializer.Serialize(writer, stringsTable);
			}
		}
	}
}
