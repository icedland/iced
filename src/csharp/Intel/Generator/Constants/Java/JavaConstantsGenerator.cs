// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Documentation.Java;
using Generator.IO;

namespace Generator.Constants.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaConstantsGenerator : ConstantsGenerator {
		readonly Dictionary<TypeId, FullConstantsFileInfo> toFullFileInfo;
		readonly JavaConstantsWriter constantsWriter;

		sealed class FullConstantsFileInfo {
			public readonly string Filename;
			public readonly string Package;
			public readonly bool IsTestFile;

			public static FullConstantsFileInfo Create(GenTypes genTypes, string package, string filename) =>
				Create(genTypes, package, new[] { filename });

			public static FullConstantsFileInfo Create(GenTypes genTypes, string package, string[] paths) =>
				new(JavaConstants.GetFilename(genTypes, package, paths), package, false);

			public static FullConstantsFileInfo CreateTest(GenTypes genTypes, string package, string filename) =>
				CreateTest(genTypes, package, new[] { filename });

			public static FullConstantsFileInfo CreateTest(GenTypes genTypes, string package, string[] paths) =>
				new(JavaConstants.GetTestFilename(genTypes, package, paths), package, true);

			public FullConstantsFileInfo(string filename, string package, bool isTestFile) {
				Filename = filename;
				Package = package;
				IsTestFile = isTestFile;
			}
		}

		public JavaConstantsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			var idConverter = JavaIdentifierConverter.Create();
			constantsWriter = new JavaConstantsWriter(genTypes, idConverter, new JavaDocCommentWriter(idConverter), new JavaDeprecatedWriter(idConverter));

			var dirs = genTypes.Dirs;
			toFullFileInfo = new Dictionary<TypeId, FullConstantsFileInfo>();
			toFullFileInfo.Add(TypeIds.IcedConstants, FullConstantsFileInfo.Create(genTypes, JavaConstants.IcedInternalPackage, nameof(TypeIds.IcedConstants) + ".java"));
			toFullFileInfo.Add(TypeIds.DecoderConstants, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.DecoderConstants) + ".java"));

			toFullFileInfo.Add(TypeIds.DecoderTestParserConstants, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.DecoderPackage, nameof(TypeIds.DecoderTestParserConstants) + ".java"));
			toFullFileInfo.Add(TypeIds.InstrInfoConstants, FullConstantsFileInfo.Create(genTypes, JavaConstants.InstructionInfoInternalPackage, nameof(TypeIds.InstrInfoConstants) + ".java"));
			toFullFileInfo.Add(TypeIds.MiscInstrInfoTestConstants, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.MiscInstrInfoTestConstants) + ".java"));
			toFullFileInfo.Add(TypeIds.InstructionInfoKeys, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.InstructionInfoKeys) + ".java"));
			toFullFileInfo.Add(TypeIds.RflagsBitsConstants, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.RflagsBitsConstants) + ".java"));
			toFullFileInfo.Add(TypeIds.MiscSectionNames, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.MiscSectionNames) + ".java"));
			toFullFileInfo.Add(TypeIds.OpCodeInfoKeys, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.EncoderPackage, nameof(TypeIds.OpCodeInfoKeys) + ".java"));
			toFullFileInfo.Add(TypeIds.OpCodeInfoFlags, FullConstantsFileInfo.CreateTest(genTypes, JavaConstants.EncoderPackage, nameof(TypeIds.OpCodeInfoFlags) + ".java"));
		}

		public override void Generate(ConstantsType constantsType) {
			if (toFullFileInfo.TryGetValue(constantsType.TypeId, out var fullFileInfo))
				WriteFile(fullFileInfo, constantsType);
			else
				throw new InvalidOperationException();
		}

		void WriteFile(FullConstantsFileInfo info, ConstantsType constantsType) =>
			constantsWriter.WriteFile(info.Package, info.Filename, constantsType, Array.Empty<string>(), info.IsTestFile);
	}
}
