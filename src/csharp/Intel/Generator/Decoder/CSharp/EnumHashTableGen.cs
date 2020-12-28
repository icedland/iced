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

using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Decoder.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class EnumHashTableGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public EnumHashTableGen(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var infos = new (string id, EnumType enumType, bool lowerCase, string filename)[] {
				("CodeHash", genTypes[TypeIds.Code], false, "Intel/ToEnumConverter.Code.cs"),
				("CpuidFeatureHash", genTypes[TypeIds.CpuidFeature], false, "Intel/ToEnumConverter.CpuidFeature.cs"),
				("DecoderErrorHash", genTypes[TypeIds.DecoderError], false, "Intel/ToEnumConverter.DecoderError.cs"),
				("DecoderOptionsHash", genTypes[TypeIds.DecoderOptions], false, "Intel/ToEnumConverter.DecoderOptions.cs"),
				("EncodingKindHash", genTypes[TypeIds.EncodingKind], false, "Intel/ToEnumConverter.EncodingKind.cs"),
				("FlowControlHash", genTypes[TypeIds.FlowControl], false, "Intel/ToEnumConverter.FlowControl.cs"),
				("MemorySizeHash", genTypes[TypeIds.MemorySize], false, "Intel/ToEnumConverter.MemorySize.cs"),
				("MnemonicHash", genTypes[TypeIds.Mnemonic], false, "Intel/ToEnumConverter.Mnemonic.cs"),
				("OpCodeOperandKindHash", genTypes[TypeIds.OpCodeOperandKind], false, "Intel/ToEnumConverter.OpCodeOperandKind.cs"),
				("RegisterHash", genTypes[TypeIds.Register], true, "Intel/ToEnumConverter.Register.cs"),
				("TupleTypeHash", genTypes[TypeIds.TupleType], false, "Intel/ToEnumConverter.TupleType.cs"),
				("ConditionCodeHash", genTypes[TypeIds.ConditionCode], false, "Intel/ToEnumConverter.ConditionCode.cs"),
				("MemorySizeOptionsHash", genTypes[TypeIds.MemorySizeOptions], false, "Intel/ToEnumConverter.MemorySizeOptions.cs"),
				("NumberBaseHash", genTypes[TypeIds.NumberBase], false, "Intel/ToEnumConverter.NumberBase.cs"),
				("OptionsPropsHash", genTypes[TypeIds.OptionsProps], false, "Intel/ToEnumConverter.OptionsProps.cs"),
				("CC_b_hash", genTypes[TypeIds.CC_b], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_ae_hash", genTypes[TypeIds.CC_ae], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_e_hash", genTypes[TypeIds.CC_e], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_ne_hash", genTypes[TypeIds.CC_ne], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_be_hash", genTypes[TypeIds.CC_be], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_a_hash", genTypes[TypeIds.CC_a], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_p_hash", genTypes[TypeIds.CC_p], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_np_hash", genTypes[TypeIds.CC_np], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_l_hash", genTypes[TypeIds.CC_l], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_ge_hash", genTypes[TypeIds.CC_ge], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_le_hash", genTypes[TypeIds.CC_le], false, "Intel/ToEnumConverter.CC.cs"),
				("CC_g_hash", genTypes[TypeIds.CC_g], false, "Intel/ToEnumConverter.CC.cs"),
			};
			foreach (var info in infos) {
				var filename = generatorContext.Types.Dirs.GetCSharpTestFilename(info.filename.Split('/'));
				new FileUpdater(TargetLanguage.CSharp, info.id, filename).Generate(writer => WriteHash(writer, info.lowerCase, info.enumType));
			}
		}

		void WriteHash(FileWriter writer, bool lowerCase, EnumType enumType) {
			var enumStr = enumType.Name(idConverter);
			var enumValues = enumType.Values.Where(a => !a.DeprecatedInfo.IsDeprecatedAndRenamed).ToArray();
			writer.WriteLine($"new Dictionary<string, {enumStr}>({enumValues.Length}, StringComparer.Ordinal) {{");
			using (writer.Indent()) {
				foreach (var value in enumValues) {
					if (value.DeprecatedInfo.IsDeprecatedAndRenamed)
						continue;
					var name = value.Name(idConverter);
					var key = value.RawName;
					if (lowerCase)
						key = key.ToLowerInvariant();
					writer.WriteLine($"{{ \"{key}\", {enumStr}.{name} }},");
				}
			}
			writer.WriteLine("};");
		}
	}
}
