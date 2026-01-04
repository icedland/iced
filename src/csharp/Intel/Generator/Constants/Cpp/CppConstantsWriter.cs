// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;
using System.Linq;
using Generator.Documentation.Cpp;
using Generator.IO;

namespace Generator.Constants.Cpp {
	sealed class CppConstantsWriter {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;
		readonly CppDocCommentWriter docWriter;
		readonly CppDeprecatedWriter deprecatedWriter;

		public CppConstantsWriter( GenTypes genTypes, IdentifierConverter idConverter, CppDocCommentWriter docWriter, CppDeprecatedWriter deprecatedWriter ) {
			this.genTypes = genTypes;
			this.idConverter = idConverter;
			this.docWriter = docWriter;
			this.deprecatedWriter = deprecatedWriter;
		}

		public void Write( FileWriter writer, ConstantsType constantsType, bool isInternal ) {
			docWriter.WriteSummary( writer, constantsType.Documentation.GetComment( TargetLanguage.Cpp ), constantsType.RawName );
			writer.WriteLine( $"namespace {constantsType.Name( idConverter )} {{" );

			using ( writer.Indent() ) {
				WriteConstants( writer, constantsType );
			}
			writer.WriteLine( $"}} // namespace {constantsType.Name( idConverter )}" );
		}

		public void WriteConstants( FileWriter writer, ConstantsType constantsType ) {
			foreach ( var constant in constantsType.Constants ) {
				var deprecMsg = deprecatedWriter.GetDeprecatedString( constant );
				docWriter.WriteSummary( writer, constant.Documentation.GetComment( TargetLanguage.Cpp ), constantsType.RawName, deprecMsg );
				if ( constant.DeprecatedInfo.IsDeprecated )
					deprecatedWriter.WriteDeprecated( writer, constant );

				var type = GetType( constant.Kind );
				var name = constant.Name( idConverter );
				var value = GetValue( constant );
				writer.WriteLine( $"constexpr {type} {name} = {value};" );
			}
		}

		string GetType( ConstantKind kind ) =>
			kind switch {
				ConstantKind.Char => "char",
				ConstantKind.String => "const char*",
				ConstantKind.Int32 => "int32_t",
				ConstantKind.UInt32 => "uint32_t",
				ConstantKind.UInt64 => "uint64_t",
				ConstantKind.Index => "std::size_t",
				ConstantKind.Register => "uint32_t",
				ConstantKind.MemorySize => "uint32_t",
				_ => throw new InvalidOperationException(),
			};

		string GetValue( Constant constant ) {
			switch ( constant.Kind ) {
			case ConstantKind.Char:
				var c = (char)constant.ValueUInt64;
				return "'" + c.ToString() + "'";

			case ConstantKind.String:
				if ( constant.RefValue is string s )
					return "\"" + EscapeStringValue( s ) + "\"";
				throw new InvalidOperationException();

			case ConstantKind.Int32:
				var i32 = (int)constant.ValueUInt64;
				if ( constant.UseHex )
					return $"0x{(uint)i32:X}";
				return i32.ToString();

			case ConstantKind.UInt32:
			case ConstantKind.Index:
				if ( constant.UseHex )
					return $"0x{(uint)constant.ValueUInt64:X}U";
				return $"{(uint)constant.ValueUInt64}U";

			case ConstantKind.UInt64:
				if ( constant.UseHex )
					return $"0x{constant.ValueUInt64:X}ULL";
				return $"{constant.ValueUInt64}ULL";

			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return GetValueString( constant );

			default:
				throw new InvalidOperationException();
			}
		}

		static string EscapeStringValue( string s ) =>
			s.Replace( "\\", "\\\\" ).Replace( "\"", "\\\"" );

		string GetValueString( Constant constant ) {
			var enumType = EnumUtils.GetEnumType( genTypes, constant.Kind );
			var enumValue = enumType.Values.First( a => a.Value == constant.ValueUInt64 );
			return $"static_cast< uint32_t >( {idConverter.ToDeclTypeAndValue( enumValue )} )";
		}

		public void WriteFile( string filename, ConstantsType constantsType, bool isInternal ) {
			// Ensure directory exists
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard( isInternal ? new[] { "INTERNAL", constantsType.RawName } : new[] { constantsType.RawName } );

			writer.WriteLine( "#pragma once" );
			writer.WriteLine( $"#ifndef {headerGuard}" );
			writer.WriteLine( $"#define {headerGuard}" );
			writer.WriteLine();
			writer.WriteLine( "#include <cstdint>" );
			writer.WriteLine( "#include <cstddef>" );
			writer.WriteLine();

			// Open namespace
			if ( isInternal ) {
				writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
				writer.WriteLine( "namespace internal {" );
			}
			else {
				writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			}
			writer.WriteLine();

			Write( writer, constantsType, isInternal );

			writer.WriteLine();
			// Close namespace
			if ( isInternal ) {
				writer.WriteLine( "} // namespace internal" );
				writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			}
			else {
				writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			}
			writer.WriteLine();
			writer.WriteLine( $"#endif // {headerGuard}" );
		}
	}
}
