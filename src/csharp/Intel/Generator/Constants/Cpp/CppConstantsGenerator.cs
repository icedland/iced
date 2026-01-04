// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Generator.Documentation.Cpp;

namespace Generator.Constants.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppConstantsGenerator : ConstantsGenerator {
		readonly Dictionary<TypeId, FullConstantsFileInfo> toFullFileInfo;
		readonly CppConstantsWriter constantsWriter;

		sealed class FullConstantsFileInfo {
			public readonly string Filename;
			public readonly bool IsInternal;

			public FullConstantsFileInfo( string filename, bool isInternal = false ) {
				Filename = filename;
				IsInternal = isInternal;
			}
		}

		public CppConstantsGenerator( GeneratorContext generatorContext )
			: base( generatorContext.Types ) {
			var idConverter = CppIdentifierConverter.Create();
			var docWriter = new CppDocCommentWriter( idConverter );
			var deprecatedWriter = new CppDeprecatedWriter( idConverter );
			constantsWriter = new CppConstantsWriter( genTypes, idConverter, docWriter, deprecatedWriter );

			toFullFileInfo = new Dictionary<TypeId, FullConstantsFileInfo>();
			toFullFileInfo.Add( TypeIds.IcedConstants, new FullConstantsFileInfo( CppConstants.GetHeaderFilename( genTypes, "iced_constants.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.DecoderConstants, new FullConstantsFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "decoder_constants.hpp" ), true ) );
		}

		public override void Generate( ConstantsType constantsType ) {
			if ( toFullFileInfo.TryGetValue( constantsType.TypeId, out var fullFileInfo ) )
				constantsWriter.WriteFile( fullFileInfo.Filename, constantsType, fullFileInfo.IsInternal );
		}
	}
}
