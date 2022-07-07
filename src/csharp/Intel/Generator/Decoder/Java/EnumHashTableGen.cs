// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.Decoder.Java {
	[Generator(TargetLanguage.Java)]
	sealed class EnumHashTableGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public EnumHashTableGen(GeneratorContext generatorContext) {
			idConverter = JavaIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var infos = new (EnumType enumType, bool lowerCase, string enumPackage, string name)[] {
				(genTypes[TypeIds.Code], false, JavaConstants.IcedPackage, "Code"),
				(genTypes[TypeIds.CpuidFeature], false, JavaConstants.IcedPackage, "CpuidFeature"),
				(genTypes[TypeIds.DecoderError], false, JavaConstants.DecoderPackage, "DecoderError"),
				(genTypes[TypeIds.DecoderOptions], false, JavaConstants.DecoderPackage, "DecoderOptions"),
				(genTypes[TypeIds.EncodingKind], false, JavaConstants.IcedPackage, "EncodingKind"),
				(genTypes[TypeIds.FlowControl], false, JavaConstants.IcedPackage, "FlowControl"),
				(genTypes[TypeIds.MemorySize], false, JavaConstants.IcedPackage, "MemorySize"),
				(genTypes[TypeIds.Mnemonic], false, JavaConstants.IcedPackage, "Mnemonic"),
				(genTypes[TypeIds.OpCodeOperandKind], false, JavaConstants.InstructionInfoPackage, "OpCodeOperandKind"),
				(genTypes[TypeIds.Register], true, JavaConstants.IcedPackage, "Register"),
				(genTypes[TypeIds.TupleType], false, JavaConstants.IcedPackage, "TupleType"),
				(genTypes[TypeIds.ConditionCode], false, JavaConstants.IcedPackage, "ConditionCode"),
				(genTypes[TypeIds.MemorySizeOptions], false, JavaConstants.FormatterPackage, "MemorySizeOptions"),
				(genTypes[TypeIds.NumberBase], false, JavaConstants.FormatterPackage, "NumberBase"),
				(genTypes[TypeIds.OptionsProps], false, JavaConstants.FormatterPackage, "OptionsProps"),
				(genTypes[TypeIds.CC_b], false, JavaConstants.FormatterPackage, "CC_b"),
				(genTypes[TypeIds.CC_ae], false, JavaConstants.FormatterPackage, "CC_ae"),
				(genTypes[TypeIds.CC_e], false, JavaConstants.FormatterPackage, "CC_e"),
				(genTypes[TypeIds.CC_ne], false, JavaConstants.FormatterPackage, "CC_ne"),
				(genTypes[TypeIds.CC_be], false, JavaConstants.FormatterPackage, "CC_be"),
				(genTypes[TypeIds.CC_a], false, JavaConstants.FormatterPackage, "CC_a"),
				(genTypes[TypeIds.CC_p], false, JavaConstants.FormatterPackage, "CC_p"),
				(genTypes[TypeIds.CC_np], false, JavaConstants.FormatterPackage, "CC_np"),
				(genTypes[TypeIds.CC_l], false, JavaConstants.FormatterPackage, "CC_l"),
				(genTypes[TypeIds.CC_ge], false, JavaConstants.FormatterPackage, "CC_ge"),
				(genTypes[TypeIds.CC_le], false, JavaConstants.FormatterPackage, "CC_le"),
				(genTypes[TypeIds.CC_g], false, JavaConstants.FormatterPackage, "CC_g"),
				(genTypes[TypeIds.MvexConvFn], false, JavaConstants.IcedPackage, "MvexConvFn"),
				(genTypes[TypeIds.MvexTupleTypeLutKind], false, JavaConstants.IcedPackage, "MvexTupleTypeLutKind"),
			};
			foreach (var info in infos) {
				// Java method byte code limitations
				const int maxVariantsPerMethod = 500;

				var className = "To" + info.name;
				var filename = JavaConstants.GetTestFilename(genTypes, JavaConstants.IcedPackage, className + ".java");
				using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
					writer.WriteFileHeader();
					writer.WriteLine($"package {JavaConstants.IcedPackage};");
					writer.WriteLine();
					writer.WriteLine("import java.util.HashMap;");
					if (info.enumPackage != JavaConstants.IcedPackage) {
						writer.WriteLine();
						writer.WriteLine($"import {info.enumPackage}.{info.name};");
					}
					writer.WriteLine();
					writer.WriteLine($"public final class {className} {{");
					using (writer.Indent()) {
						var enumValues = info.enumType.Values.
							Where(a => !a.DeprecatedInfo.IsDeprecatedAndRenamed &&
									!(a.DeprecatedInfo.IsDeprecated && a.DeprecatedInfo.IsError)).ToArray();
						writer.WriteLine("public static Integer tryGet(String key) {");
						using (writer.Indent())
							writer.WriteLine("return map.get(key);");
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("public static int get(String key) {");
						using (writer.Indent()) {
							writer.WriteLine("Integer value = tryGet(key);");
							writer.WriteLine("if (value == null)");
							using (writer.Indent())
								writer.WriteLine($"throw new UnsupportedOperationException(String.format(\"Couldn't find enum variant {info.name}.%s\", key));");
							writer.WriteLine("return value.intValue();");
						}
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("public static String[] names() {");
						using (writer.Indent())
							writer.WriteLine("return map.entrySet().stream().sorted((a, b) -> Integer.compareUnsigned(a.getValue(), b.getValue())).map(a -> a.getKey()).toArray(String[]::new);");
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("public static Iterable<Integer> values() {");
						using (writer.Indent())
							writer.WriteLine("return map.values();");
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("public static int size() {");
						using (writer.Indent())
							writer.WriteLine("return map.size();");
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("public static HashMap<String, Integer> copy() {");
						using (writer.Indent())
							writer.WriteLine("return new HashMap<String, Integer>(map);");
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("private static final HashMap<String, Integer> map = getMap();");
						writer.WriteLine();
						int variantMethods = (info.enumType.Values.Length + maxVariantsPerMethod - 1) / maxVariantsPerMethod;
						writer.WriteLine("private static HashMap<String, Integer> getMap() {");
						using (writer.Indent()) {
							writer.WriteLine($"HashMap<String, Integer> map = new HashMap<String, Integer>({enumValues.Length});");
							for (int i = 0; i < variantMethods; i++)
								writer.WriteLine($"initMap{i}(map);");
							writer.WriteLine("return map;");
						}
						writer.WriteLine("}");
						for (int i = 0; i < variantMethods; i++) {
							writer.WriteLine();
							int start = i * maxVariantsPerMethod;
							int end = Math.Min(start + maxVariantsPerMethod, enumValues.Length);
							bool anyDeprec = enumValues.Skip(start).Take(end - start).Any(a => a.DeprecatedInfo.IsDeprecated);
							if (anyDeprec)
								writer.WriteLine("@SuppressWarnings(\"deprecation\")");
							writer.WriteLine($"private static void initMap{i}(HashMap<String, Integer> map) {{");
							using (writer.Indent()) {
								for (int j = start; j < end; j++) {
									var value = enumValues[j];
									var key = value.RawName;
									if (info.lowerCase)
										key = key.ToLowerInvariant();
									writer.WriteLine($"map.put(\"{key}\", {idConverter.ToDeclTypeAndValue(value)});");
								}
							}
							writer.WriteLine("}");
						}
					}
					writer.WriteLine("}");
				}
			}
		}
	}
}
