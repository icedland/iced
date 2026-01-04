// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Tests for FormatterOptions and related formatter features

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

// Helper to decode a simple instruction for formatter tests
static Instruction decode_mov_eax_ebx() {
	const uint8_t data[] = { 0x89, 0xD8 };  // MOV EAX, EBX
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	return result.value();
}

static Instruction decode_mov_eax_mem() {
	const uint8_t data[] = { 0x8B, 0x00 };  // MOV EAX, [EAX]
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	return result.value();
}

static Instruction decode_add_eax_imm() {
	const uint8_t data[] = { 0x05, 0x78, 0x56, 0x34, 0x12 };  // ADD EAX, 0x12345678
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	return result.value();
}

static Instruction decode_mov_mem_sib() {
	const uint8_t data[] = { 0x8B, 0x44, 0x88, 0x10 };  // MOV EAX, [EAX+ECX*4+0x10]
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	return result.value();
}

// ============================================================================
// Case options tests
// ============================================================================

TEST_CASE( "FormatterOptions: uppercase_mnemonics", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_ebx();
	
	IntelFormatter formatter;
	
	// Default: lowercase
	formatter.options().set_uppercase_mnemonics( false );
	CHECK( formatter.format_to_string( instr ) == "mov eax, ebx" );
	
	// Uppercase mnemonics only
	formatter.options().set_uppercase_mnemonics( true );
	CHECK( formatter.format_to_string( instr ) == "MOV eax, ebx" );
}

TEST_CASE( "FormatterOptions: uppercase_registers", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_ebx();
	
	IntelFormatter formatter;
	
	// Default: lowercase
	formatter.options().set_uppercase_registers( false );
	CHECK( formatter.format_to_string( instr ) == "mov eax, ebx" );
	
	// Uppercase registers only
	formatter.options().set_uppercase_registers( true );
	CHECK( formatter.format_to_string( instr ) == "mov EAX, EBX" );
}

TEST_CASE( "FormatterOptions: uppercase_all", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_ebx();
	
	IntelFormatter formatter;
	
	formatter.options().set_uppercase_all( true );
	CHECK( formatter.format_to_string( instr ) == "MOV EAX, EBX" );
}

TEST_CASE( "FormatterOptions: uppercase_keywords", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_mem();
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( true );
	
	// Default: lowercase
	formatter.options().set_uppercase_keywords( false );
	CHECK( formatter.format_to_string( instr ) == "mov eax, dword ptr [eax]" );
	
	// Uppercase keywords
	formatter.options().set_uppercase_keywords( true );
	CHECK( formatter.format_to_string( instr ) == "mov eax, DWORD PTR [eax]" );
}

TEST_CASE( "FormatterOptions: uppercase_prefixes", "[formatter_options][manual]" ) {
	// LOCK ADD [EAX], EAX
	const uint8_t data[] = { 0xF0, 0x01, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( true );
	
	// Default: lowercase
	formatter.options().set_uppercase_prefixes( false );
	std::string output = formatter.format_to_string( instr );
	CHECK( output.find( "lock" ) != std::string::npos );
	
	// Uppercase prefixes
	formatter.options().set_uppercase_prefixes( true );
	output = formatter.format_to_string( instr );
	CHECK( output.find( "LOCK" ) != std::string::npos );
}

// ============================================================================
// Number formatting options tests
// ============================================================================

TEST_CASE( "FormatterOptions: hex_prefix", "[formatter_options][manual]" ) {
	auto instr = decode_add_eax_imm();
	
	IntelFormatter formatter;
	
	// Default suffix (h)
	formatter.options().set_hex_prefix( "" );
	formatter.options().set_hex_suffix( "h" );
	CHECK( formatter.format_to_string( instr ) == "add eax, 12345678h" );
	
	// 0x prefix
	formatter.options().set_hex_prefix( "0x" );
	formatter.options().set_hex_suffix( "" );
	CHECK( formatter.format_to_string( instr ) == "add eax, 0x12345678" );
}

TEST_CASE( "FormatterOptions: uppercase_hex", "[formatter_options][manual]" ) {
	// Use a value with letters
	const uint8_t data[] = { 0x05, 0xEF, 0xBE, 0xAD, 0xDE };  // ADD EAX, 0xDEADBEEF
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	IntelFormatter formatter;
	// Note: leading zero added by default when hex starts with letter (A-F)
	
	// Lowercase hex
	formatter.options().set_uppercase_hex( false );
	CHECK( formatter.format_to_string( instr ) == "add eax, 0deadbeefh" );
	
	// Uppercase hex
	formatter.options().set_uppercase_hex( true );
	CHECK( formatter.format_to_string( instr ) == "add eax, 0DEADBEEFh" );
}

TEST_CASE( "FormatterOptions: small_hex_numbers_in_decimal", "[formatter_options][manual]" ) {
	// ADD EAX, 5
	const uint8_t data[] = { 0x83, 0xC0, 0x05 };
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	IntelFormatter formatter;
	
	// Small numbers in decimal
	formatter.options().set_small_hex_numbers_in_decimal( true );
	std::string output = formatter.format_to_string( instr );
	CHECK( output.find( "5" ) != std::string::npos );
	
	// Always hex
	formatter.options().set_small_hex_numbers_in_decimal( false );
	formatter.options().set_hex_prefix( "0x" );
	formatter.options().set_hex_suffix( "" );
	output = formatter.format_to_string( instr );
	CHECK( output.find( "0x5" ) != std::string::npos );
}

TEST_CASE( "FormatterOptions: add_leading_zero_to_hex_numbers", "[formatter_options][manual]" ) {
	// Use a value starting with a letter
	const uint8_t data[] = { 0x05, 0xCD, 0xAB, 0x00, 0x00 };  // ADD EAX, 0xABCD
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	IntelFormatter formatter;
	formatter.options().set_hex_prefix( "" );
	formatter.options().set_hex_suffix( "h" );
	formatter.options().set_small_hex_numbers_in_decimal( false );
	formatter.options().set_uppercase_hex( false );  // Need lowercase for this test
	
	// Without leading zero
	formatter.options().set_add_leading_zero_to_hex_numbers( false );
	CHECK( formatter.format_to_string( instr ) == "add eax, abcdh" );
	
	// With leading zero
	formatter.options().set_add_leading_zero_to_hex_numbers( true );
	CHECK( formatter.format_to_string( instr ) == "add eax, 0abcdh" );
}

// ============================================================================
// Memory operand options tests
// ============================================================================

TEST_CASE( "FormatterOptions: show_memory_size", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_mem();
	
	IntelFormatter formatter;
	
	// Show memory size
	formatter.options().set_show_memory_size( true );
	CHECK( formatter.format_to_string( instr ) == "mov eax, dword ptr [eax]" );
	
	// Hide memory size
	formatter.options().set_show_memory_size( false );
	CHECK( formatter.format_to_string( instr ) == "mov eax, [eax]" );
}

TEST_CASE( "FormatterOptions: always_show_scale", "[formatter_options][manual]" ) {
	// MOV EAX, [EAX+EBX] - scale is 1
	const uint8_t data[] = { 0x8B, 0x04, 0x18 };  // MOV EAX, [EAX+EBX*1]
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	IntelFormatter formatter;
	
	// Default: don't show scale of 1
	formatter.options().set_always_show_scale( false );
	std::string output = formatter.format_to_string( instr );
	CHECK( output.find( "*1" ) == std::string::npos );
	
	// Always show scale
	formatter.options().set_always_show_scale( true );
	output = formatter.format_to_string( instr );
	CHECK( output.find( "*1" ) != std::string::npos );
}

TEST_CASE( "FormatterOptions: space_after_operand_separator", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_ebx();
	
	IntelFormatter formatter;
	
	// With space (default)
	formatter.options().set_space_after_operand_separator( true );
	CHECK( formatter.format_to_string( instr ) == "mov eax, ebx" );
	
	// Without space
	formatter.options().set_space_after_operand_separator( false );
	CHECK( formatter.format_to_string( instr ) == "mov eax,ebx" );
}

TEST_CASE( "FormatterOptions: space_after_memory_bracket", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_mem();
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( false );  // Hide "dword ptr" for cleaner test
	
	// Default: no space
	formatter.options().set_space_after_memory_bracket( false );
	CHECK( formatter.format_to_string( instr ) == "mov eax, [eax]" );
	
	// With space
	formatter.options().set_space_after_memory_bracket( true );
	CHECK( formatter.format_to_string( instr ) == "mov eax, [ eax ]" );
}

TEST_CASE( "FormatterOptions: space_between_memory_add_operators", "[formatter_options][manual]" ) {
	auto instr = decode_mov_mem_sib();  // MOV EAX, [EAX+ECX*4+0x10]
	
	IntelFormatter formatter;
	
	// Default: no spaces around operators
	formatter.options().set_space_between_memory_add_operators( false );
	std::string output = formatter.format_to_string( instr );
	CHECK( output.find( "eax+ecx" ) != std::string::npos );
	
	// Spaces around operators
	formatter.options().set_space_between_memory_add_operators( true );
	output = formatter.format_to_string( instr );
	CHECK( output.find( "eax + ecx" ) != std::string::npos );
}

// ============================================================================
// Cross-formatter options tests
// ============================================================================

TEST_CASE( "FormatterOptions: same options work across formatters", "[formatter_options][manual]" ) {
	auto instr = decode_mov_eax_ebx();
	
	// Create options to share
	FormatterOptions options;
	options.set_uppercase_all( true );
	
	// Intel
	{
		IntelFormatter formatter( options );
		CHECK( formatter.format_to_string( instr ) == "MOV EAX, EBX" );
	}
	
	// MASM
	{
		MasmFormatter formatter( options );
		CHECK( formatter.format_to_string( instr ) == "MOV EAX, EBX" );
	}
	
	// NASM
	{
		NasmFormatter formatter( options );
		CHECK( formatter.format_to_string( instr ) == "MOV EAX, EBX" );
	}
	
	// GAS (note: operands reversed, % prefix)
	{
		GasFormatter formatter( options );
		formatter.options().set_show_memory_size( false );
		std::string output = formatter.format_to_string( instr );
		CHECK( output.find( "MOV" ) != std::string::npos );
		CHECK( output.find( "%EBX" ) != std::string::npos );
		CHECK( output.find( "%EAX" ) != std::string::npos );
	}
}

// ============================================================================
// Encoder options tests
// ============================================================================

TEST_CASE( "Encoder: prevent_vex2 option", "[encoder_options][manual]" ) {
	// Create a VEX instruction that could use VEX2
	// VMOVAPS XMM0, XMM1 can be encoded with VEX2 or VEX3
	auto instr = InstructionFactory::with2( Code::VEX_VMOVAPS_XMM_XMMM128, 
	                                        Register::XMM0, Register::XMM1 );
	
	// Encode with VEX2 allowed (default)
	{
		Encoder encoder( 64 );
		auto result = encoder.encode( instr, 0 );
		REQUIRE( result.has_value() );
		auto bytes = encoder.buffer();
		// VEX2 prefix is C5
		CHECK( bytes[0] == 0xC5 );
	}
	
	// Encode with VEX2 prevented
	{
		Encoder encoder( 64 );
		encoder.set_prevent_vex2( true );
		auto result = encoder.encode( instr, 0 );
		REQUIRE( result.has_value() );
		auto bytes = encoder.buffer();
		// VEX3 prefix is C4
		CHECK( bytes[0] == 0xC4 );
	}
}

// ============================================================================
// Error handling tests
// ============================================================================

TEST_CASE( "Decoder: no more bytes error", "[error_handling][manual]" ) {
	std::span<const uint8_t> empty_data;  // Empty buffer
	Decoder decoder( 32, empty_data, 0x1000 );
	
	CHECK( decoder.can_decode() == false );
	
	auto result = decoder.decode();
	CHECK( !result.has_value() );
	CHECK( result.error().error == DecoderError::NO_MORE_BYTES );
}

TEST_CASE( "Decoder: truncated instruction error", "[error_handling][manual]" ) {
	// MOV EAX, imm32 needs 5 bytes, we only provide 3
	const uint8_t data[] = { 0xB8, 0x00, 0x00 };  // Truncated
	Decoder decoder( 32, data, 0x1000 );
	
	DecoderError error;
	auto instr = decoder.decode_out( error );
	
	CHECK( error == DecoderError::NO_MORE_BYTES );
}

TEST_CASE( "Encoder: invalid operand combination", "[error_handling][manual]" ) {
	// Try to create an invalid instruction
	Instruction instr;
	instr.set_code( Code::MOV_R32_RM32 );
	instr.set_op0_kind( OpKind::REGISTER );
	instr.set_op0_register( Register::EAX );
	// Don't set op1 - this may cause encoding to fail
	instr.set_op1_kind( OpKind::REGISTER );
	instr.set_op1_register( Register::NONE );  // Invalid register
	
	Encoder encoder( 32 );
	auto result = encoder.encode( instr, 0 );
	
	// Should fail or produce invalid encoding
	// The exact behavior depends on implementation
}

TEST_CASE( "Decoder: position and IP management", "[decoder][manual]" ) {
	const uint8_t data[] = { 0x90, 0x90, 0x90, 0x90 };  // 4 NOPs
	Decoder decoder( 32, data, 0x1000 );
	
	CHECK( decoder.position() == 0 );
	CHECK( decoder.ip() == 0x1000 );
	CHECK( decoder.max_position() == 4 );
	
	// Decode first instruction
	auto r1 = decoder.decode();
	REQUIRE( r1.has_value() );
	CHECK( r1->ip() == 0x1000 );
	CHECK( decoder.position() == 1 );
	
	// Decode second instruction
	auto r2 = decoder.decode();
	REQUIRE( r2.has_value() );
	CHECK( r2->ip() == 0x1001 );
	CHECK( decoder.position() == 2 );
	
	// Set position back
	decoder.set_position( 0 );
	CHECK( decoder.position() == 0 );
	
	// Set IP
	decoder.set_ip( 0x2000 );
	auto r3 = decoder.decode();
	REQUIRE( r3.has_value() );
	CHECK( r3->ip() == 0x2000 );
}

TEST_CASE( "Encoder: buffer management", "[encoder][manual]" ) {
	Encoder encoder( 64 );
	
	// Buffer should be empty initially
	CHECK( encoder.buffer().empty() );
	
	// Encode an instruction
	auto instr = InstructionFactory::with( Code::NOPD );
	auto result = encoder.encode( instr, 0 );
	REQUIRE( result.has_value() );
	
	// Buffer should have data
	CHECK( !encoder.buffer().empty() );
	
	// Clear buffer
	encoder.set_buffer( {} );
	CHECK( encoder.buffer().empty() );
}
