// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;
using Generator.IO;

namespace Generator.Decoder.Cpp {
	sealed class CppDecoderTableSerializer : DecoderTableSerializer {
		public string TableName { get; }

		public CppDecoderTableSerializer( GenTypes genTypes, string tableName, DecoderTableSerializerInfo info )
			: base( genTypes, CppIdentifierConverter.Create(), info ) => TableName = tableName;

		public void Serialize( FileWriter writer ) {
			writer.WriteFileHeader();

			writer.WriteLine( "#pragma once" );
			writer.WriteLine( $"#ifndef ICED_X86_INTERNAL_DATA_{TableName.ToUpperInvariant()}_HPP" );
			writer.WriteLine( $"#define ICED_X86_INTERNAL_DATA_{TableName.ToUpperInvariant()}_HPP" );
			writer.WriteLine();
			writer.WriteLine( "#include <cstdint>" );
			writer.WriteLine( "#include <cstddef>" );
			writer.WriteLine( "#include <array>" );
			writer.WriteLine();
			writer.WriteLine( "namespace iced_x86 {" );
			writer.WriteLine( "namespace internal {" );
			writer.WriteLine();

			// Serialize to memory first to get size and content
			using var memStream = new MemoryStream();
			using ( var streamWriter = new StreamWriter( memStream, leaveOpen: true ) ) {
				var memFileWriter = new FileWriter( TargetLanguage.Cpp, streamWriter );
				SerializeCore( new TextFileByteTableWriter( memFileWriter ) );
			}

			// Count bytes written (count comma-separated hex values)
			memStream.Position = 0;
			var content = new StreamReader( memStream ).ReadToEnd();
			var tableSize = CountHexBytes( content );

			writer.WriteLine( "// clang-format off" );
			writer.WriteLine( $"inline constexpr std::array<uint8_t, {tableSize}> g_{TableName}_tbl_data = {{" );
			// Write indented content
			using ( writer.Indent() ) {
				foreach ( var line in content.Split( '\n', StringSplitOptions.RemoveEmptyEntries ) )
					writer.WriteLine( line.TrimEnd( '\r' ) );
			}
			writer.WriteLine( "};" );
			writer.WriteLine( "// clang-format on" );
			writer.WriteLine();

		writer.WriteLine( $"inline constexpr std::size_t {TableName.ToUpperInvariant()}_MAX_ID_NAMES = {info.TablesToSerialize.Length};" );
		// Prefix constant names with table name to avoid conflicts between legacy/vex/evex
		var prefix = TableName.ToUpperInvariant() + "_";
		foreach ( var name in info.TableIndexNames ) {
			var constName = idConverter.Constant( $"{name}Index" );
			writer.WriteLine( $"inline constexpr std::size_t {prefix}{constName} = {GetInfo( name ).Index};" );
		}

			writer.WriteLine();
			writer.WriteLine( "} // namespace internal" );
			writer.WriteLine( "} // namespace iced_x86" );
			writer.WriteLine();
			writer.WriteLine( $"#endif // ICED_X86_INTERNAL_DATA_{TableName.ToUpperInvariant()}_HPP" );
		}

		static int CountHexBytes( string content ) {
			// Count hex bytes that are actual array elements (not in comments)
			// Each element line starts with optional whitespace, then "0x"
			int count = 0;
			foreach ( var line in content.Split( '\n' ) ) {
				var trimmed = line.TrimStart();
				// Skip comment-only lines and empty lines
				if ( trimmed.StartsWith( "//" ) || string.IsNullOrWhiteSpace( trimmed ) )
					continue;
				// Count hex values before any comment on this line
				var beforeComment = trimmed;
				var commentIdx = trimmed.IndexOf( "//" );
				if ( commentIdx >= 0 )
					beforeComment = trimmed.Substring( 0, commentIdx );
				// Count 0x occurrences in the non-comment part
				int pos = 0;
				while ( ( pos = beforeComment.IndexOf( "0x", pos, StringComparison.Ordinal ) ) >= 0 ) {
					count++;
					pos += 2;
				}
			}
			return count;
		}
	}
}
