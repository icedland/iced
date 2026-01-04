// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.IO;

namespace Generator.Decoder.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppDecoderTableGenerator {
		readonly GeneratorContext generatorContext;

		public CppDecoderTableGenerator( GeneratorContext generatorContext ) => this.generatorContext = generatorContext;

		public void Generate() {
			var genTypes = generatorContext.Types;

			// Always generate both binary data and constexpr handlers
			// CMake will choose which one to use based on ICED_X86_CONSTEXPR_HANDLERS
			GenerateBinaryData(genTypes);
			GenerateConstexprHandlers(genTypes);
		}

		void GenerateBinaryData(GenTypes genTypes) {
			var serializers = new CppDecoderTableSerializer[] {
				new CppDecoderTableSerializer( genTypes, "legacy", DecoderTableSerializerInfo.Legacy( genTypes ) ),
				new CppDecoderTableSerializer( genTypes, "vex", DecoderTableSerializerInfo.Vex( genTypes ) ),
				new CppDecoderTableSerializer( genTypes, "evex", DecoderTableSerializerInfo.Evex( genTypes ) ),
				new CppDecoderTableSerializer( genTypes, "xop", DecoderTableSerializerInfo.Xop( genTypes ) ),
				new CppDecoderTableSerializer( genTypes, "mvex", DecoderTableSerializerInfo.Mvex( genTypes ) ),
			};

			foreach ( var serializer in serializers ) {
				var filename = CppConstants.GetInternalHeaderFilename( genTypes, $"data_{serializer.TableName}.hpp" );
				using ( var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) ) )
					serializer.Serialize( writer );
			}
		}

		void GenerateConstexprHandlers(GenTypes genTypes) {
			var serializers = new CppConstexprHandlerSerializer[] {
				new CppConstexprHandlerSerializer( genTypes, "legacy", DecoderTableSerializerInfo.Legacy( genTypes ) ),
				new CppConstexprHandlerSerializer( genTypes, "vex", DecoderTableSerializerInfo.Vex( genTypes ) ),
				new CppConstexprHandlerSerializer( genTypes, "evex", DecoderTableSerializerInfo.Evex( genTypes ) ),
				new CppConstexprHandlerSerializer( genTypes, "xop", DecoderTableSerializerInfo.Xop( genTypes ) ),
				new CppConstexprHandlerSerializer( genTypes, "mvex", DecoderTableSerializerInfo.Mvex( genTypes ) ),
			};

			foreach ( var serializer in serializers ) {
				var filename = CppConstants.GetInternalHeaderFilename( genTypes, $"constexpr_{serializer.TableName}_tables.hpp" );
				using ( var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) ) )
					serializer.Serialize( writer );
			}
		}
	}
}
