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
		readonly GeneratorContext generatorContext;

		public CSharpFormatterTableGenerator(GeneratorContext generatorContext) =>
			this.generatorContext = generatorContext;

		public void Generate() {
			var genTypes = generatorContext.Types;
			var serializers = new List<IFormatterTableSerializer>();
			if (genTypes.Options.HasGasFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(genTypes.GetObject<Gas.CtorInfos>(TypeIds.GasCtorInfos).Infos, genTypes[TypeIds.GasCtorKind], CSharpConstants.GasFormatterDefine, CSharpConstants.GasFormatterNamespace));
			if (genTypes.Options.HasIntelFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(genTypes.GetObject<Intel.CtorInfos>(TypeIds.IntelCtorInfos).Infos, genTypes[TypeIds.IntelCtorKind], CSharpConstants.IntelFormatterDefine, CSharpConstants.IntelFormatterNamespace));
			if (genTypes.Options.HasMasmFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(genTypes.GetObject<Masm.CtorInfos>(TypeIds.MasmCtorInfos).Infos, genTypes[TypeIds.MasmCtorKind], CSharpConstants.MasmFormatterDefine, CSharpConstants.MasmFormatterNamespace));
			if (genTypes.Options.HasNasmFormatter)
				serializers.Add(new CSharpFormatterTableSerializer(genTypes.GetObject<Nasm.CtorInfos>(TypeIds.NasmCtorInfos).Infos, genTypes[TypeIds.NasmCtorKind], CSharpConstants.NasmFormatterDefine, CSharpConstants.NasmFormatterNamespace));
			if (genTypes.Options.HasFastFormatter)
				serializers.Add(new CSharpFastFormatterTableSerializer(genTypes.GetObject<Fast.FmtTblInfos>(TypeIds.FastFmtTblInfos).Infos, CSharpConstants.FastFormatterDefine, CSharpConstants.FastFormatterNamespace));

			var stringsTable = new StringsTable();

			foreach (var serializer in serializers)
				serializer.Initialize(genTypes, stringsTable);

			stringsTable.Freeze();

			const string FormatterStringsTableName = "FormatterStringsTable";
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.FormatterNamespace), FormatterStringsTableName + ".g.cs")))) {
				var serializer = new CSharpStringsTableSerializer(stringsTable, CSharpConstants.FormatterNamespace, FormatterStringsTableName, CSharpConstants.AnyFormatterDefine);
				serializer.Serialize(writer);
			}

			foreach (var serializer in serializers) {
				using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(serializer.GetFilename(generatorContext))))
					serializer.Serialize(genTypes, writer, stringsTable);
			}
		}
	}
}
