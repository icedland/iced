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
using Generator.Enums.Decoder;
using Generator.Enums.Encoder;
using Generator.Enums.InstructionInfo;
using Generator.IO;

namespace Generator.Decoder.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.Enums_Table)]
	sealed class EnumHashTableGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;

		public EnumHashTableGen(GeneratorOptions generatorOptions) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
		}

		public void Generate() {
			var infos = new (string id, EnumType enumType, bool lowerCase, string filename)[] {
				("CodeHash", CodeEnum.Instance, false, "test_utils/from_str_conv/code_table.rs"),
				("CpuidFeatureHash", CpuidFeatureEnum.Instance, false, "test_utils/from_str_conv/cpuid_feature_table.rs"),
				("DecoderOptionsHash", DecoderOptionsEnum.Instance, false, "test_utils/from_str_conv/decoder_options_table.rs"),
				("EncodingKindHash", EncodingKindEnum.Instance, false, "test_utils/from_str_conv/encoding_kind_table.rs"),
				("FlowControlHash", FlowControlEnum.Instance, false, "test_utils/from_str_conv/flow_control_table.rs"),
				("MemorySizeHash", MemorySizeEnum.Instance, false, "test_utils/from_str_conv/memory_size_table.rs"),
				("MnemonicHash", MnemonicEnum.Instance, false, "test_utils/from_str_conv/mnemonic_table.rs"),
				("OpCodeOperandKindHash", OpCodeOperandKindEnum.Instance, false, "test_utils/from_str_conv/op_code_operand_kind_table.rs"),
				("RegisterHash", RegisterEnum.Instance, true, "test_utils/from_str_conv/register_table.rs"),
				("TupleTypeHash", TupleTypeEnum.Instance, false, "test_utils/from_str_conv/tuple_type_table.rs"),
				("ConditionCodeHash", ConditionCodeEnum.Instance, false, "test_utils/from_str_conv/condition_code_table.rs"),
			};
			foreach (var info in infos) {
				var filename = Path.Combine(generatorOptions.RustDir, Path.Combine(info.filename.Split('/')));
				new FileUpdater(TargetLanguage.Rust, info.id, filename).Generate(writer => WriteHash(writer, info.lowerCase, info.enumType));
			}
		}

		void WriteHash(FileWriter writer, bool lowerCase, EnumType enumType) {
			writer.WriteLine($"let mut h = HashMap::with_capacity({enumType.Values.Length});");
			var enumStr = enumType.Name(idConverter);
			foreach (var value in enumType.Values) {
				string name;
				if (enumType.IsFlags)
					name = idConverter.Constant(value.RawName);
				else
					name = value.Name(idConverter);
				var key = value.RawName;
				if (lowerCase)
					key = key.ToLowerInvariant();
				writer.WriteLine($"h.insert(\"{key}\", {enumStr}::{name});");
			}
		}
	}
}
