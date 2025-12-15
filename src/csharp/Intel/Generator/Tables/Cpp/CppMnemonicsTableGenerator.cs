// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;
using Generator.Constants;
using Generator.IO;

namespace Generator.Tables.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppMnemonicsTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public CppMnemonicsTableGenerator( GeneratorContext generatorContext ) {
			idConverter = CppIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var icedConstants = genTypes.GetConstantsType( TypeIds.IcedConstants );
			var defs = genTypes.GetObject<InstructionDefs>( TypeIds.InstructionDefs ).Defs;

			GenerateHeader( genTypes, defs );
			GenerateSource( genTypes, icedConstants, defs );
		}

		void GenerateHeader( GenTypes genTypes, InstructionDef[] defs ) {
			var filename = CppConstants.GetInternalHeaderFilename( genTypes, "tables.hpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard( "INTERNAL", "TABLES" );

			writer.WriteLine( "#pragma once" );
			writer.WriteLine( $"#ifndef {headerGuard}" );
			writer.WriteLine( $"#define {headerGuard}" );
			writer.WriteLine();
			writer.WriteLine( "#include \"iced_x86/mnemonic.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/memory_size.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/code.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "#include <array>" );
			writer.WriteLine( "#include <cstdint>" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine( "namespace internal {" );
			writer.WriteLine();

			writer.WriteLine( $"/// @brief Code to Mnemonic mapping table." );
			writer.WriteLine( $"extern const std::array< Mnemonic, {defs.Length} > g_code_to_mnemonic;" );
			writer.WriteLine();

			writer.WriteLine( $"/// @brief Instruction operand counts table." );
			writer.WriteLine( $"extern const std::array< uint8_t, {defs.Length} > g_instruction_op_counts;" );
			writer.WriteLine();

			writer.WriteLine( $"/// @brief Instruction memory sizes table." );
			writer.WriteLine( $"extern const std::array< MemorySize, {defs.Length} > g_instruction_memory_sizes;" );
			writer.WriteLine();

			writer.WriteLine( "} // namespace internal" );
			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			writer.WriteLine();
			writer.WriteLine( $"#endif // {headerGuard}" );
		}

		void GenerateSource( GenTypes genTypes, ConstantsType icedConstants, InstructionDef[] defs ) {
			var filename = CppConstants.GetSourceFilename( genTypes, "tables.cpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "#include \"iced_x86/internal/tables.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine( "namespace internal {" );
			writer.WriteLine();

			// Mnemonic table
			writer.WriteLine( $"const std::array< Mnemonic, {defs.Length} > g_code_to_mnemonic = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < defs.Length; i++ ) {
					var def = defs[i];
					if ( def.Mnemonic.Value > ushort.MaxValue )
						throw new InvalidOperationException();
					var comma = i < defs.Length - 1 ? "," : "";
					writer.WriteLine( $"{idConverter.ToDeclTypeAndValue( def.Mnemonic )}{comma} // {def.Code.Name( idConverter )}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Operand counts table
			writer.WriteLine( $"const std::array< uint8_t, {defs.Length} > g_instruction_op_counts = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < defs.Length; i++ ) {
					var def = defs[i];
					var comma = i < defs.Length - 1 ? "," : "";
					writer.WriteLine( $"{def.OpCount}{comma} // {def.Code.Name( idConverter )}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			// Memory sizes table
			writer.WriteLine( $"const std::array< MemorySize, {defs.Length} > g_instruction_memory_sizes = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < defs.Length; i++ ) {
					var def = defs[i];
					var comma = i < defs.Length - 1 ? "," : "";
					writer.WriteLine( $"{idConverter.ToDeclTypeAndValue( def.Memory )}{comma} // {def.Code.Name( idConverter )}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			writer.WriteLine( "} // namespace internal" );
			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
		}
	}
}
