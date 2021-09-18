// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Decoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class EnumHashTableGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public EnumHashTableGen(GeneratorContext generatorContext) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var infos = new (string id, EnumType enumType, bool lowerCase, string filename)[] {
				("CodeHash", genTypes[TypeIds.Code], false, "test_utils/from_str_conv/code_table.rs"),
				("CpuidFeatureHash", genTypes[TypeIds.CpuidFeature], false, "test_utils/from_str_conv/cpuid_feature_table.rs"),
				("DecoderErrorHash", genTypes[TypeIds.DecoderError], false, "test_utils/from_str_conv/decoder_error_table.rs"),
				("DecoderOptionsHash", genTypes[TypeIds.DecoderOptions], false, "test_utils/from_str_conv/decoder_options_table.rs"),
				("EncodingKindHash", genTypes[TypeIds.EncodingKind], false, "test_utils/from_str_conv/encoding_kind_table.rs"),
				("FlowControlHash", genTypes[TypeIds.FlowControl], false, "test_utils/from_str_conv/flow_control_table.rs"),
				("MemorySizeHash", genTypes[TypeIds.MemorySize], false, "test_utils/from_str_conv/memory_size_table.rs"),
				("MnemonicHash", genTypes[TypeIds.Mnemonic], false, "test_utils/from_str_conv/mnemonic_table.rs"),
				("OpCodeOperandKindHash", genTypes[TypeIds.OpCodeOperandKind], false, "test_utils/from_str_conv/op_code_operand_kind_table.rs"),
				("RegisterHash", genTypes[TypeIds.Register], true, "test_utils/from_str_conv/register_table.rs"),
				("TupleTypeHash", genTypes[TypeIds.TupleType], false, "test_utils/from_str_conv/tuple_type_table.rs"),
				("ConditionCodeHash", genTypes[TypeIds.ConditionCode], false, "test_utils/from_str_conv/condition_code_table.rs"),
				("MemorySizeOptionsHash", genTypes[TypeIds.MemorySizeOptions], false, "test_utils/from_str_conv/memory_size_options_table.rs"),
				("NumberBaseHash", genTypes[TypeIds.NumberBase], false, "test_utils/from_str_conv/number_base_table.rs"),
				("OptionsPropsHash", genTypes[TypeIds.OptionsProps], false, "test_utils/from_str_conv/options_props_table.rs"),
				("CC_b_hash", genTypes[TypeIds.CC_b], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_ae_hash", genTypes[TypeIds.CC_ae], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_e_hash", genTypes[TypeIds.CC_e], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_ne_hash", genTypes[TypeIds.CC_ne], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_be_hash", genTypes[TypeIds.CC_be], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_a_hash", genTypes[TypeIds.CC_a], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_p_hash", genTypes[TypeIds.CC_p], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_np_hash", genTypes[TypeIds.CC_np], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_l_hash", genTypes[TypeIds.CC_l], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_ge_hash", genTypes[TypeIds.CC_ge], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_le_hash", genTypes[TypeIds.CC_le], false, "test_utils/from_str_conv/cc_table.rs"),
				("CC_g_hash", genTypes[TypeIds.CC_g], false, "test_utils/from_str_conv/cc_table.rs"),
				("MvexConvFnHash", genTypes[TypeIds.MvexConvFn], false, "test_utils/from_str_conv/mvex_conv_fn_table.rs"),
				("MvexTupleTypeLutKindHash", genTypes[TypeIds.MvexTupleTypeLutKind], false, "test_utils/from_str_conv/mvex_tt_lut_kind_table.rs"),
			};
			foreach (var info in infos) {
				var filename = generatorContext.Types.Dirs.GetRustFilename(info.filename.Split('/'));
				new FileUpdater(TargetLanguage.Rust, info.id, filename).Generate(writer => WriteHash(writer, info.lowerCase, info.enumType));
			}
		}

		void WriteHash(FileWriter writer, bool lowerCase, EnumType enumType) {
			var enumValues = enumType.Values.Where(a => !a.DeprecatedInfo.IsDeprecatedAndRenamed).ToArray();
			if (enumValues.Length == 0)
				writer.WriteLine($"let h = HashMap::new();");
			else
				writer.WriteLine($"let mut h = HashMap::with_capacity({enumValues.Length});");
			var enumStr = enumType.Name(idConverter);
			foreach (var value in enumValues) {
				if (value.DeprecatedInfo.IsDeprecated && value.DeprecatedInfo.IsError)
					continue;
				string name;
				if (enumType.IsFlags)
					name = idConverter.Constant(value.RawName);
				else
					name = value.Name(idConverter);
				var key = value.RawName;
				if (lowerCase)
					key = key.ToLowerInvariant();
				writer.WriteLine($"let _ = h.insert(\"{key}\", {enumStr}::{name});");
			}
		}
	}
}
