// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.IO;
using Generator.IO;

namespace Generator.Decoder.Cpp {
	[Generator( TargetLanguage.Cpp, 98 )]
	sealed class CppTestGenerator {
		readonly GenTypes genTypes;

		public CppTestGenerator( GeneratorContext generatorContext ) {
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			GenerateDecoderTests();
			GenerateInstructionTests();
		}

		void GenerateDecoderTests() {
			var filename = CppConstants.GetTestFilename( genTypes, "test_decoder.cpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "#include <catch2/catch_test_macros.hpp>" );
			writer.WriteLine( "#include \"iced_x86/iced_x86.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "using namespace iced_x86;" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Decoder: basic construction\", \"[decoder]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "const uint8_t data[] = { 0x90 }; // NOP" );
				writer.WriteLine( "Decoder decoder( 64, data );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( decoder.bitness() == 64 );" );
				writer.WriteLine( "CHECK( decoder.position() == 0 );" );
				writer.WriteLine( "CHECK( decoder.ip() == 0 );" );
				writer.WriteLine( "CHECK( decoder.can_decode() );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Decoder: 32-bit mode\", \"[decoder]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "const uint8_t data[] = { 0x90 };" );
				writer.WriteLine( "Decoder decoder( 32, data, 0x10000 );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( decoder.bitness() == 32 );" );
				writer.WriteLine( "CHECK( decoder.ip() == 0x10000 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Decoder: 16-bit mode\", \"[decoder]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "const uint8_t data[] = { 0x90 };" );
				writer.WriteLine( "Decoder decoder( 16, data );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( decoder.bitness() == 16 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Decoder: decode returns instruction\", \"[decoder]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "const uint8_t data[] = { 0x90 };" );
				writer.WriteLine( "Decoder decoder( 64, data );" );
				writer.WriteLine();
				writer.WriteLine( "auto result = decoder.decode();" );
				writer.WriteLine( "// Note: actual decoding not yet implemented" );
				writer.WriteLine( "// Just verify the API works" );
				writer.WriteLine( "CHECK( decoder.position() == 1 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Decoder: empty input\", \"[decoder]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "std::span< const uint8_t > empty_data;" );
				writer.WriteLine( "Decoder decoder( 64, empty_data );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( !decoder.can_decode() );" );
				writer.WriteLine( "CHECK( decoder.position() == 0 );" );
				writer.WriteLine( "CHECK( decoder.max_position() == 0 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Decoder: position management\", \"[decoder]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "const uint8_t data[] = { 0x90, 0x90, 0x90 };" );
				writer.WriteLine( "Decoder decoder( 64, data, 0x1000 );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( decoder.position() == 0 );" );
				writer.WriteLine( "CHECK( decoder.ip() == 0x1000 );" );
				writer.WriteLine();
				writer.WriteLine( "decoder.set_position( 2 );" );
				writer.WriteLine( "CHECK( decoder.position() == 2 );" );
				writer.WriteLine( "CHECK( decoder.ip() == 0x1002 );" );
			}
			writer.WriteLine( "}" );
		}

		void GenerateInstructionTests() {
			var filename = CppConstants.GetTestFilename( genTypes, "test_instruction.cpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "#include <catch2/catch_test_macros.hpp>" );
			writer.WriteLine( "#include \"iced_x86/iced_x86.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "using namespace iced_x86;" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Instruction: default construction\", \"[instruction]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Instruction instr;" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( instr.code() == Code::INVALID );" );
				writer.WriteLine( "CHECK( instr.is_invalid() );" );
				writer.WriteLine( "CHECK( instr.length() == 0 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Instruction: set code\", \"[instruction]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Instruction instr;" );
				writer.WriteLine( "instr.set_code( Code::NOPD );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( instr.code() == Code::NOPD );" );
				writer.WriteLine( "CHECK( !instr.is_invalid() );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Instruction: IP accessors\", \"[instruction]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Instruction instr;" );
				writer.WriteLine( "instr.set_length( 2 );" );
				writer.WriteLine( "instr.set_next_ip( 0x1002 );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( instr.ip() == 0x1000 );" );
				writer.WriteLine( "CHECK( instr.next_ip() == 0x1002 );" );
				writer.WriteLine( "CHECK( instr.length() == 2 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Instruction: memory accessors\", \"[instruction]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Instruction instr;" );
				writer.WriteLine();
				writer.WriteLine( "instr.set_memory_base( Register::RAX );" );
				writer.WriteLine( "instr.set_memory_index( Register::RBX );" );
				writer.WriteLine( "instr.set_memory_index_scale( 4 );" );
				writer.WriteLine( "instr.set_memory_displacement64( 0x1234 );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( instr.memory_base() == Register::RAX );" );
				writer.WriteLine( "CHECK( instr.memory_index() == Register::RBX );" );
				writer.WriteLine( "CHECK( instr.memory_index_scale() == 4 );" );
				writer.WriteLine( "CHECK( instr.memory_displacement64() == 0x1234 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Instruction: immediate accessors\", \"[instruction]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Instruction instr;" );
				writer.WriteLine();
				writer.WriteLine( "instr.set_immediate32( 0xDEADBEEF );" );
				writer.WriteLine( "CHECK( instr.immediate32() == 0xDEADBEEF );" );
				writer.WriteLine();
				writer.WriteLine( "instr.set_immediate8( 0x42 );" );
				writer.WriteLine( "CHECK( instr.immediate8() == 0x42 );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Instruction: prefix flags\", \"[instruction]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Instruction instr;" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( !instr.has_lock_prefix() );" );
				writer.WriteLine( "instr.set_has_lock_prefix( true );" );
				writer.WriteLine( "CHECK( instr.has_lock_prefix() );" );
				writer.WriteLine();
				writer.WriteLine( "CHECK( !instr.has_rep_prefix() );" );
				writer.WriteLine( "instr.set_has_rep_prefix( true );" );
				writer.WriteLine( "CHECK( instr.has_rep_prefix() );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "TEST_CASE( \"Instruction: struct size\", \"[instruction]\" ) {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Verify struct matches Rust layout" );
				writer.WriteLine( "CHECK( sizeof( Instruction ) == 40 );" );
			}
			writer.WriteLine( "}" );
		}
	}
}
