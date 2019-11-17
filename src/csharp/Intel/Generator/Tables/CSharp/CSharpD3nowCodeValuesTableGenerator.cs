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

namespace Generator.Tables.CSharp {
	sealed class CSharpD3nowCodeValuesTableGenerator : ID3nowCodeValuesTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;

		public CSharpD3nowCodeValuesTableGenerator(GeneratorOptions generatorOptions) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
		}

		public void Generate((int index, EnumValue enumValue)[] infos) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.DecoderNamespace), "OpCodeHandlers_D3NOW.cs");
			var updater = new FileUpdater(TargetLanguage.CSharp, "D3nowCodeValues", filename);
			updater.Generate(writer => WriteTable(writer, infos));
		}

		void WriteTable(FileWriter writer, (int index, EnumValue enumValue)[] infos) {
			var codeName = CodeEnum.Instance.Name(idConverter);
			foreach (var info in infos)
				writer.WriteLine($"result[0x{info.index:X2}] = {codeName}.{info.enumValue.Name(idConverter)};");
		}
	}
}
