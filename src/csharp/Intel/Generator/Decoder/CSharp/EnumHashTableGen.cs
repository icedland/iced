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

namespace Generator.Decoder.CSharp {
	sealed class EnumHashTableGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;

		public EnumHashTableGen(GeneratorOptions generatorOptions) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
		}

		public void Generate() {
			var infos = new (string id, EnumType enumType, bool lowerCase, string filename)[] {
				("CodeHash", CodeEnum.Instance, false, "Intel/ToEnumConverter.Code.cs"),
				("CpuidFeatureHash", CpuidFeatureEnum.Instance, false, "Intel/ToEnumConverter.CpuidFeature.cs"),
				("DecoderOptionsHash", DecoderOptionsEnum.Instance, false, "Intel/ToEnumConverter.DecoderOptions.cs"),
				("EncodingKindHash", EncodingKindEnum.Instance, false, "Intel/ToEnumConverter.EncodingKind.cs"),
				("FlowControlHash", FlowControlEnum.Instance, false, "Intel/ToEnumConverter.FlowControl.cs"),
				("MemorySizeHash", MemorySizeEnum.Instance, false, "Intel/ToEnumConverter.MemorySize.cs"),
				("MnemonicHash", MnemonicEnum.Instance, false, "Intel/ToEnumConverter.Mnemonic.cs"),
				("OpCodeOperandKindHash", OpCodeOperandKindEnum.Instance, false, "Intel/ToEnumConverter.OpCodeOperandKind.cs"),
				("RegisterHash", RegisterEnum.Instance, true, "Intel/ToEnumConverter.Register.cs"),
				("TupleTypeHash", TupleTypeEnum.Instance, false, "Intel/ToEnumConverter.TupleType.cs"),
			};
			foreach (var info in infos) {
				var filename = Path.Combine(generatorOptions.CSharpTestsDir, Path.Combine(info.filename.Split('/')));
				new FileUpdater(TargetLanguage.CSharp, info.id, filename).Generate(writer => WriteHash(writer, info.lowerCase, info.enumType));
			}
		}

		void WriteHash(FileWriter writer, bool lowerCase, EnumType enumType) {
			var enumStr = enumType.Name(idConverter);
			writer.WriteLine($"new Dictionary<string, {enumStr}>({enumType.Values.Length}, StringComparer.Ordinal) {{");
			writer.Indent();
			foreach (var value in enumType.Values) {
				var name = value.Name(idConverter);
				var key = value.RawName;
				if (lowerCase)
					key = key.ToLowerInvariant();
				writer.WriteLine($"{{ \"{key}\", {enumStr}.{name} }},");
			}
			writer.Unindent();
			writer.WriteLine("};");
		}
	}
}
