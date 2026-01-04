// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;
using Generator.Enums;
using Generator.IO;

namespace Generator.Formatters.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppFormatterStringsGenerator {
		readonly GenTypes genTypes;

		public CppFormatterStringsGenerator( GeneratorContext generatorContext ) =>
			genTypes = generatorContext.Types;

		public void Generate() {
			GenerateMnemonicStrings();
			GenerateRegisterStrings();
			GenerateMemorySizeStrings();
		}

		void GenerateMnemonicStrings() {
			var mnemonicEnum = genTypes[TypeIds.Mnemonic];
			var values = mnemonicEnum.Values;

			var filename = CppConstants.GetInternalHeaderFilename( genTypes, "formatter_mnemonics.hpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "// Generated mnemonic string tables for formatter" );
			writer.WriteLine();
			writer.WriteLine( "#pragma once" );
			writer.WriteLine( "#ifndef ICED_X86_INTERNAL_FORMATTER_MNEMONICS_HPP" );
			writer.WriteLine( "#define ICED_X86_INTERNAL_FORMATTER_MNEMONICS_HPP" );
			writer.WriteLine();
			writer.WriteLine( "#include <array>" );
			writer.WriteLine( "#include <string_view>" );
			writer.WriteLine( "#include \"../mnemonic.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.InternalNamespace} {{" );
			writer.WriteLine();

			// Lowercase array
			writer.WriteLine( "/// @brief Mnemonic strings (lowercase)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, MNEMONIC_COUNT> MNEMONIC_STRINGS_LOWER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var name = value.RawName;
					// Special case for INVALID which displays as "???"
					var str = name == "INVALID" ? "???" : name.ToLowerInvariant();
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Uppercase array
			writer.WriteLine( "/// @brief Mnemonic strings (uppercase)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, MNEMONIC_COUNT> MNEMONIC_STRINGS_UPPER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var name = value.RawName;
					// Special case for INVALID which displays as "???"
					var str = name == "INVALID" ? "???" : name.ToUpperInvariant();
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Helper function
			writer.WriteLine( "/// @brief Get mnemonic string" );
			writer.WriteLine( "/// @param mnemonic Mnemonic value" );
			writer.WriteLine( "/// @param uppercase If true, return uppercase version" );
			writer.WriteLine( "/// @return Mnemonic string" );
			writer.WriteLine( "inline constexpr std::string_view get_mnemonic_string( Mnemonic mnemonic, bool uppercase = false ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "auto idx = static_cast<std::size_t>( mnemonic );" );
				writer.WriteLine( "if ( idx >= MNEMONIC_STRINGS_LOWER.size() ) {" );
				using ( writer.Indent() ) {
					writer.WriteLine( "return \"???\";" );
				}
				writer.WriteLine( "}" );
				writer.WriteLine( "return uppercase ? MNEMONIC_STRINGS_UPPER[idx] : MNEMONIC_STRINGS_LOWER[idx];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( $"}} // namespace {CppConstants.InternalNamespace}" );
			writer.WriteLine();
			writer.WriteLine( "#endif // ICED_X86_INTERNAL_FORMATTER_MNEMONICS_HPP" );
		}

		void GenerateRegisterStrings() {
			var registerEnum = genTypes[TypeIds.Register];
			var values = registerEnum.Values;

			var filename = CppConstants.GetInternalHeaderFilename( genTypes, "formatter_regs.hpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "// Generated register string tables for formatter" );
			writer.WriteLine();
			writer.WriteLine( "#pragma once" );
			writer.WriteLine( "#ifndef ICED_X86_INTERNAL_FORMATTER_REGS_HPP" );
			writer.WriteLine( "#define ICED_X86_INTERNAL_FORMATTER_REGS_HPP" );
			writer.WriteLine();
			writer.WriteLine( "#include <array>" );
			writer.WriteLine( "#include <string_view>" );
			writer.WriteLine( "#include \"../register.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.InternalNamespace} {{" );
			writer.WriteLine();

			// Lowercase array
			writer.WriteLine( "/// @brief Register names (lowercase)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, REGISTER_COUNT> REGISTER_NAMES_LOWER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var name = value.RawName;
					// NONE displays as empty string, EIP/RIP are special
					var str = name == "NONE" ? "" : name.ToLowerInvariant();
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Uppercase array
			writer.WriteLine( "/// @brief Register names (uppercase)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, REGISTER_COUNT> REGISTER_NAMES_UPPER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var name = value.RawName;
					// NONE displays as empty string
					var str = name == "NONE" ? "" : name.ToUpperInvariant();
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Helper function
			writer.WriteLine( "/// @brief Get register name" );
			writer.WriteLine( "/// @param index Register index" );
			writer.WriteLine( "/// @param uppercase If true, return uppercase version" );
			writer.WriteLine( "/// @return Register name" );
			writer.WriteLine( "inline constexpr std::string_view get_register_name( uint32_t index, bool uppercase = false ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( index >= REGISTER_NAMES_LOWER.size() ) {" );
				using ( writer.Indent() ) {
					writer.WriteLine( "return \"???\";" );
				}
				writer.WriteLine( "}" );
				writer.WriteLine( "return uppercase ? REGISTER_NAMES_UPPER[index] : REGISTER_NAMES_LOWER[index];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( $"}} // namespace {CppConstants.InternalNamespace}" );
			writer.WriteLine();
			writer.WriteLine( "#endif // ICED_X86_INTERNAL_FORMATTER_REGS_HPP" );
		}

		void GenerateMemorySizeStrings() {
			var memorySizeEnum = genTypes[TypeIds.MemorySize];
			var values = memorySizeEnum.Values;

			var filename = CppConstants.GetInternalHeaderFilename( genTypes, "formatter_memory_size.hpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "// Generated memory size string tables for formatter" );
			writer.WriteLine();
			writer.WriteLine( "#pragma once" );
			writer.WriteLine( "#ifndef ICED_X86_INTERNAL_FORMATTER_MEMORY_SIZE_HPP" );
			writer.WriteLine( "#define ICED_X86_INTERNAL_FORMATTER_MEMORY_SIZE_HPP" );
			writer.WriteLine();
			writer.WriteLine( "#include <array>" );
			writer.WriteLine( "#include <string_view>" );
			writer.WriteLine( "#include \"../memory_size.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.InternalNamespace} {{" );
			writer.WriteLine();

			// Lowercase array - Intel format
			writer.WriteLine( "/// @brief Memory size strings for Intel format (lowercase)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, MEMORY_SIZE_COUNT> MEMORY_SIZE_STRINGS_LOWER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var str = GetMemorySizeString( value.RawName, false );
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma} // {value.RawName}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Uppercase array - Intel format
			writer.WriteLine( "/// @brief Memory size strings for Intel format (uppercase)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, MEMORY_SIZE_COUNT> MEMORY_SIZE_STRINGS_UPPER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var str = GetMemorySizeString( value.RawName, true );
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma} // {value.RawName}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Helper function
			writer.WriteLine( "/// @brief Get memory size string" );
			writer.WriteLine( "/// @param size Memory size" );
			writer.WriteLine( "/// @param uppercase If true, return uppercase version" );
			writer.WriteLine( "/// @return Memory size string" );
			writer.WriteLine( "inline constexpr std::string_view get_memory_size_string( MemorySize size, bool uppercase = false ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "auto idx = static_cast<std::size_t>( size );" );
				writer.WriteLine( "if ( idx >= MEMORY_SIZE_STRINGS_LOWER.size() ) {" );
				using ( writer.Indent() ) {
					writer.WriteLine( "return \"\";" );
				}
				writer.WriteLine( "}" );
				writer.WriteLine( "return uppercase ? MEMORY_SIZE_STRINGS_UPPER[idx] : MEMORY_SIZE_STRINGS_LOWER[idx];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// NASM-style arrays (no "ptr" suffix)
			writer.WriteLine( "/// @brief Memory size strings for NASM format (lowercase, no \"ptr\" suffix)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, MEMORY_SIZE_COUNT> NASM_MEMORY_SIZE_STRINGS_LOWER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var str = GetNasmMemorySizeString( value.RawName, false );
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma} // {value.RawName}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			writer.WriteLine( "/// @brief Memory size strings for NASM format (uppercase, no \"ptr\" suffix)" );
			writer.WriteLine( $"constexpr std::array<std::string_view, MEMORY_SIZE_COUNT> NASM_MEMORY_SIZE_STRINGS_UPPER = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < values.Length; i++ ) {
					var value = values[i];
					var str = GetNasmMemorySizeString( value.RawName, true );
					var comma = i < values.Length - 1 ? "," : "";
					writer.WriteLine( $"\"{str}\"{comma} // {value.RawName}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// NASM helper function
			writer.WriteLine( "/// @brief Get NASM memory size string (no \"ptr\" suffix)" );
			writer.WriteLine( "/// @param size Memory size" );
			writer.WriteLine( "/// @param uppercase If true, return uppercase version" );
			writer.WriteLine( "/// @return NASM-style memory size string" );
			writer.WriteLine( "inline constexpr std::string_view get_nasm_memory_size_string( MemorySize size, bool uppercase = false ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "auto idx = static_cast<std::size_t>( size );" );
				writer.WriteLine( "if ( idx >= NASM_MEMORY_SIZE_STRINGS_LOWER.size() ) {" );
				using ( writer.Indent() ) {
					writer.WriteLine( "return \"\";" );
				}
				writer.WriteLine( "}" );
				writer.WriteLine( "return uppercase ? NASM_MEMORY_SIZE_STRINGS_UPPER[idx] : NASM_MEMORY_SIZE_STRINGS_LOWER[idx];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( $"}} // namespace {CppConstants.InternalNamespace}" );
			writer.WriteLine();
			writer.WriteLine( "#endif // ICED_X86_INTERNAL_FORMATTER_MEMORY_SIZE_HPP" );
		}

		static string GetMemorySizeString( string name, bool uppercase ) {
			// Map MemorySize enum names to Intel format strings
			// Use a simple pattern-matching approach based on the name
			string result;
			
			if ( name == "Unknown" || name.StartsWith( "Fpu", StringComparison.OrdinalIgnoreCase ) )
				result = "";
			else if ( name.Contains( "Broadcast" ) ) {
				// Broadcast types: "dword bcst" or "qword bcst" depending on element size
				if ( name.Contains( "32" ) || name.Contains( "Float32" ) || name.Contains( "Int16" ) || name.Contains( "BFloat16" ) )
					result = "dword bcst";
				else if ( name.Contains( "Float16" ) )
					result = "word bcst";
				else
					result = "qword bcst";
			}
			else if ( name.Contains( "512" ) )
				result = "zmmword ptr";
			else if ( name.Contains( "256" ) )
				result = "ymmword ptr";
			else if ( name.Contains( "128" ) || name.Contains( "Xmm" ) )
				result = "xmmword ptr";
			else if ( name.Contains( "80" ) || name == "Bcd" || name == "Fword10" || name.Contains( "SegPtr64" ) )
				result = "tbyte ptr";
			else if ( name.Contains( "64" ) || name == "UInt64" || name == "Int64" || name == "QwordOffset" || 
			          name.Contains( "Bound32" ) || name == "Bnd64" )
				result = "qword ptr";
			else if ( name.Contains( "Fword" ) || name == "SegPtr32" )
				result = "fword ptr";
			else if ( name.Contains( "32" ) || name == "UInt32" || name == "Int32" || name == "Float32" ||
			          name == "DwordOffset" || name.Contains( "Bound16" ) || name == "Bnd32" || name == "SegPtr16" )
				result = "dword ptr";
			else if ( name.Contains( "16" ) || name == "UInt16" || name == "Int16" || name == "Float16" || 
			          name == "BFloat16" || name == "WordOffset" )
				result = "word ptr";
			else if ( name.Contains( "8" ) || name == "UInt8" || name == "Int8" )
				result = "byte ptr";
			else if ( name == "UInt52" )
				result = "qword ptr";
			else if ( name.Contains( "Tile" ) )
				result = "tile ptr";
			else if ( name.Contains( "Xsave" ) )
				result = "xsave ptr";
			else
				result = "";  // Unknown format
			
			return uppercase ? result.ToUpperInvariant() : result;
		}

		static string GetNasmMemorySizeString( string name, bool uppercase ) {
			// Map MemorySize enum names to NASM format strings (no "ptr" suffix)
			// NASM uses: byte, word, dword, qword, tword, oword, yword, zword
			string result;
			
			if ( name == "Unknown" || name.StartsWith( "Fpu", StringComparison.OrdinalIgnoreCase ) ||
			     name.Contains( "Tile" ) || name.Contains( "Xsave" ) || name == "SegmentDescSelector" )
				result = "";
			else if ( name.Contains( "Broadcast" ) ) {
				// Broadcast types: same element sizes but no "bcst" suffix for NASM
				if ( name.Contains( "32" ) || name.Contains( "Float32" ) || name.Contains( "Int16" ) || name.Contains( "BFloat16" ) )
					result = "dword";
				else if ( name.Contains( "Float16" ) )
					result = "word";
				else
					result = "qword";
			}
			else if ( name.Contains( "512" ) )
				result = "zword";
			else if ( name.Contains( "256" ) )
				result = "yword";
			else if ( name.Contains( "128" ) || name.Contains( "Xmm" ) )
				result = "oword";  // NASM uses oword for 128-bit
			else if ( name.Contains( "80" ) || name == "Bcd" || name == "Fword10" || name.Contains( "SegPtr64" ) )
				result = "tword";
			else if ( name.Contains( "64" ) || name == "UInt64" || name == "Int64" || name == "QwordOffset" || 
			          name.Contains( "Bound32" ) || name == "Bnd64" )
				result = "qword";
			else if ( name.Contains( "Fword" ) || name == "SegPtr32" )
				result = "fword";
			else if ( name.Contains( "32" ) || name == "UInt32" || name == "Int32" || name == "Float32" ||
			          name == "DwordOffset" || name.Contains( "Bound16" ) || name == "Bnd32" || name == "SegPtr16" )
				result = "dword";
			else if ( name.Contains( "16" ) || name == "UInt16" || name == "Int16" || name == "Float16" || 
			          name == "BFloat16" || name == "WordOffset" )
				result = "word";
			else if ( name.Contains( "8" ) || name == "UInt8" || name == "Int8" )
				result = "byte";
			else if ( name == "UInt52" )
				result = "qword";
			else
				result = "";  // Unknown format
			
			return uppercase ? result.ToUpperInvariant() : result;
		}
	}
}
