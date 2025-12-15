// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Text;
using Generator.IO;

namespace Generator.Documentation.Cpp {
	sealed class CppDocCommentWriter : DocCommentWriter {
		readonly IdentifierConverter idConverter;
		readonly StringBuilder sb;

		static readonly Dictionary<string, string> toTypeInfo = new(StringComparer.Ordinal) {
			{ "bcd", "bcd" },
			{ "bf16", "bfloat16" },
			{ "f16", "float16" },
			{ "f32", "float" },
			{ "f64", "double" },
			{ "f80", "long double" },
			{ "f128", "float128" },
			{ "i8", "int8_t" },
			{ "i16", "int16_t" },
			{ "i32", "int32_t" },
			{ "i64", "int64_t" },
			{ "i128", "__int128" },
			{ "i256", "int256" },
			{ "i512", "int512" },
			{ "u8", "uint8_t" },
			{ "u16", "uint16_t" },
			{ "u32", "uint32_t" },
			{ "u52", "uint52" },
			{ "u64", "uint64_t" },
			{ "u128", "__uint128" },
			{ "u256", "uint256" },
			{ "u512", "uint512" },
		};

		public CppDocCommentWriter( IdentifierConverter idConverter ) {
			this.idConverter = idConverter;
			sb = new StringBuilder();
		}

		string GetStringAndReset() {
			while ( sb.Length > 0 && char.IsWhiteSpace( sb[^1] ) )
				sb.Length--;
			var s = sb.ToString();
			sb.Clear();
			return s;
		}

		void RawWriteWithComment( FileWriter writer, bool writeEmpty = true ) {
			var s = GetStringAndReset();
			if ( s.Length == 0 && !writeEmpty )
				return;
			writer.WriteLine( s.Length == 0 ? "///" : "/// " + s );
		}

		public void BeginWrite( FileWriter writer ) {
			if ( sb.Length != 0 )
				throw new InvalidOperationException();
		}

		public void EndWrite( FileWriter writer ) {
			RawWriteWithComment( writer, false );
		}

		public void WriteSummary( FileWriter writer, string? documentation, string typeName ) {
			WriteSummary( writer, documentation, typeName, null );
		}

		public void WriteSummary( FileWriter writer, string? documentation, string typeName, string? deprecMsg ) {
			if ( string.IsNullOrEmpty( documentation ) )
				return;
			BeginWrite( writer );
			sb.Append( "@brief " );
			WriteDoc( writer, documentation, typeName );
			RawWriteWithComment( writer );
			if ( deprecMsg is not null ) {
				WriteLine( writer, string.Empty );
				WriteLine( writer, $"@deprecated {deprecMsg}" );
			}
			EndWrite( writer );
		}

		public void Write( string text ) =>
			sb.Append( text );

		public void WriteLine( FileWriter writer, string text ) {
			Write( text );
			RawWriteWithComment( writer );
		}

		public void WriteDocLine( FileWriter writer, string text, string typeName ) {
			WriteDoc( writer, text, typeName );
			RawWriteWithComment( writer );
		}

		public void WriteDoc( FileWriter writer, string documentation, string typeName ) {
			foreach ( var info in GetTokens( typeName, documentation ) ) {
				switch ( info.kind ) {
				case TokenKind.NewParagraph:
					if ( !string.IsNullOrEmpty( info.value ) && !string.IsNullOrEmpty( info.value2 ) )
						throw new InvalidOperationException();
					RawWriteWithComment( writer );
					sb.Append( "@par" );
					RawWriteWithComment( writer );
					break;
				case TokenKind.HorizontalLine:
					RawWriteWithComment( writer );
					sb.Append( "---" );
					RawWriteWithComment( writer );
					break;
				case TokenKind.String:
					sb.Append( Escape( info.value ) );
					if ( !string.IsNullOrEmpty( info.value2 ) )
						throw new InvalidOperationException();
					break;
				case TokenKind.Code:
					sb.Append( "@c " );
					sb.Append( Escape( info.value ) );
					if ( !string.IsNullOrEmpty( info.value2 ) )
						throw new InvalidOperationException();
					break;
				case TokenKind.PrimitiveType:
					if ( !toTypeInfo.TryGetValue( info.value, out var typeStr ) )
						throw new InvalidOperationException( $"Unknown type '{info.value}, comment: {documentation}" );
					sb.Append( "@c " );
					sb.Append( typeStr );
					if ( !string.IsNullOrEmpty( info.value2 ) )
						throw new InvalidOperationException();
					break;
				case TokenKind.Type:
					sb.Append( "@ref " );
					sb.Append( TypeToCppName( info.value ) );
					if ( !string.IsNullOrEmpty( info.value2 ) )
						throw new InvalidOperationException();
					break;
				case TokenKind.EnumFieldReference:
				case TokenKind.FieldReference:
				case TokenKind.Property:
				case TokenKind.Method:
					var cppType = TypeToCppName( info.value );
					var cppMember = MemberToCppName( info.value, info.value2, info.kind );
					sb.Append( "@ref " );
					sb.Append( cppType );
					sb.Append( "::" );
					sb.Append( cppMember );
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		static string Escape( string value ) {
			// Escape special Doxygen characters
			var result = value
				.Replace( "@", "@@" )
				.Replace( "\\", "\\\\" )
				.Replace( "<", "\\<" )
				.Replace( ">", "\\>" );
			return result;
		}

		static string TypeToCppName( string type ) =>
			type switch {
				"Iced.Intel.Register" => "Register",
				"BlockEncoder" or "ConstantOffsets" or "Code" or "CpuidFeature" or
				"Instruction" or "Register" or "RepPrefixKind" or "RelocInfo" or
				"SymbolResult" => type,
				_ => type,
			};

		string MemberToCppName( string type, string member, TokenKind kind ) {
			switch ( kind ) {
			case TokenKind.EnumFieldReference:
				return idConverter.EnumField( member );
			case TokenKind.FieldReference:
				return idConverter.Field( member );
			case TokenKind.Property:
				return idConverter.Method( member );
			case TokenKind.Method:
				return idConverter.Method( GetMethodNameOnly( member ) );
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
