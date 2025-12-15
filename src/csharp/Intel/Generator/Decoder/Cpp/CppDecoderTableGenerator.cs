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
	}
}
