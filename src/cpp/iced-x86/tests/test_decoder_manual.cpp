// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Manual tests - not auto-generated

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

// ============================================================================
// Decoder tests with actual instruction decoding
// ============================================================================

TEST_CASE( "Decoder: decode NOP", "[decoder][manual]" ) {
	// 90 = NOP
	const uint8_t data[] = { 0x90 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::NOPD );
	CHECK( result->mnemonic() == Mnemonic::NOP );
	CHECK( result->length() == 1 );
	CHECK( result->ip() == 0x1000 );
}

TEST_CASE( "Decoder: decode MOV EAX, EBX", "[decoder][manual]" ) {
	// 89 D8 = MOV EAX, EBX (actually encoded as MOV r/m32, r32)
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_RM32_R32 );
	CHECK( result->mnemonic() == Mnemonic::MOV );
	CHECK( result->length() == 2 );
	CHECK( result->op_count() == 2 );
	CHECK( result->op_kind( 0 ) == OpKind::REGISTER );
	CHECK( result->op_kind( 1 ) == OpKind::REGISTER );
	CHECK( result->op_register( 0 ) == Register::EAX );
	CHECK( result->op_register( 1 ) == Register::EBX );
}

TEST_CASE( "Decoder: decode ADD EAX, 0x12345678", "[decoder][manual]" ) {
	// 05 78 56 34 12 = ADD EAX, 0x12345678
	const uint8_t data[] = { 0x05, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::ADD_EAX_IMM32 );
	CHECK( result->mnemonic() == Mnemonic::ADD );
	CHECK( result->length() == 5 );
	CHECK( result->op_count() == 2 );
	CHECK( result->op_kind( 0 ) == OpKind::REGISTER );
	CHECK( result->op_kind( 1 ) == OpKind::IMMEDIATE32 );
	CHECK( result->op_register( 0 ) == Register::EAX );
	CHECK( result->immediate32() == 0x12345678 );
}

TEST_CASE( "Decoder: decode PUSH immediate", "[decoder][manual]" ) {
	// 68 78 56 34 12 = PUSH 0x12345678
	const uint8_t data[] = { 0x68, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::PUSHD_IMM32 );
	CHECK( result->mnemonic() == Mnemonic::PUSH );
	CHECK( result->length() == 5 );
}

TEST_CASE( "Decoder: decode with memory operand", "[decoder][manual]" ) {
	// 8B 00 = MOV EAX, [EAX]
	const uint8_t data[] = { 0x8B, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_R32_RM32 );
	CHECK( result->mnemonic() == Mnemonic::MOV );
	CHECK( result->length() == 2 );
	CHECK( result->op_count() == 2 );
	CHECK( result->op_kind( 0 ) == OpKind::REGISTER );
	CHECK( result->op_kind( 1 ) == OpKind::MEMORY );
	CHECK( result->op_register( 0 ) == Register::EAX );
	CHECK( result->memory_base() == Register::EAX );
}

TEST_CASE( "Decoder: decode SIB byte", "[decoder][manual]" ) {
	// 8B 04 8B = MOV EAX, [EBX+ECX*4]
	const uint8_t data[] = { 0x8B, 0x04, 0x8B };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_R32_RM32 );
	CHECK( result->mnemonic() == Mnemonic::MOV );
	CHECK( result->length() == 3 );
	CHECK( result->op_count() == 2 );
	CHECK( result->op_kind( 1 ) == OpKind::MEMORY );
	CHECK( result->memory_base() == Register::EBX );
	CHECK( result->memory_index() == Register::ECX );
	CHECK( result->memory_index_scale() == 4 );
}

TEST_CASE( "Decoder: decode memory with displacement", "[decoder][manual]" ) {
	// 8B 80 78 56 34 12 = MOV EAX, [EAX+0x12345678]
	const uint8_t data[] = { 0x8B, 0x80, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_R32_RM32 );
	CHECK( result->memory_base() == Register::EAX );
	CHECK( result->memory_displacement64() == 0x12345678 );
}

TEST_CASE( "Decoder: decode multiple instructions", "[decoder][manual]" ) {
	// 90 90 90 = NOP NOP NOP
	const uint8_t data[] = { 0x90, 0x90, 0x90 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result1 = decoder.decode();
	REQUIRE( result1.has_value() );
	CHECK( result1->ip() == 0x1000 );
	
	auto result2 = decoder.decode();
	REQUIRE( result2.has_value() );
	CHECK( result2->ip() == 0x1001 );
	
	auto result3 = decoder.decode();
	REQUIRE( result3.has_value() );
	CHECK( result3->ip() == 0x1002 );
	
	CHECK( !decoder.can_decode() );
}

TEST_CASE( "Decoder: 64-bit mode", "[decoder][manual]" ) {
	// 48 89 C3 = MOV RBX, RAX (REX.W prefix)
	const uint8_t data[] = { 0x48, 0x89, 0xC3 };
	Decoder decoder( 64, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_RM64_R64 );
	CHECK( result->op_register( 0 ) == Register::RBX );
	CHECK( result->op_register( 1 ) == Register::RAX );
}

TEST_CASE( "Decoder: 64-bit REX.R register extension", "[decoder][manual]" ) {
	// 4C 89 C0 = MOV RAX, R8 (REX.R extends reg field)
	const uint8_t data[] = { 0x4C, 0x89, 0xC0 };
	Decoder decoder( 64, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_RM64_R64 );
	CHECK( result->op_register( 0 ) == Register::RAX );
	CHECK( result->op_register( 1 ) == Register::R8 );
}

TEST_CASE( "Decoder: 64-bit REX.B register extension", "[decoder][manual]" ) {
	// 49 89 C0 = MOV R8, RAX (REX.B extends r/m field)
	const uint8_t data[] = { 0x49, 0x89, 0xC0 };
	Decoder decoder( 64, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_RM64_R64 );
	CHECK( result->op_register( 0 ) == Register::R8 );
	CHECK( result->op_register( 1 ) == Register::RAX );
}

TEST_CASE( "Decoder: 16-bit mode", "[decoder][manual]" ) {
	// 89 D8 = MOV AX, BX in 16-bit mode
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 16, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_RM16_R16 );
	CHECK( result->op_register( 0 ) == Register::AX );
	CHECK( result->op_register( 1 ) == Register::BX );
}

TEST_CASE( "Decoder: operand size prefix", "[decoder][manual]" ) {
	// 66 89 D8 = MOV AX, BX in 32-bit mode (with operand size override)
	const uint8_t data[] = { 0x66, 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOV_RM16_R16 );
	CHECK( result->op_register( 0 ) == Register::AX );
	CHECK( result->op_register( 1 ) == Register::BX );
}

TEST_CASE( "Decoder: LOCK prefix", "[decoder][manual]" ) {
	// F0 01 00 = LOCK ADD [EAX], EAX
	const uint8_t data[] = { 0xF0, 0x01, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::ADD_RM32_R32 );
	CHECK( result->has_lock_prefix() );
}

TEST_CASE( "Decoder: REP prefix", "[decoder][manual]" ) {
	// F3 A4 = REP MOVSB
	const uint8_t data[] = { 0xF3, 0xA4 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->code() == Code::MOVSB_M8_M8 );
	CHECK( result->has_rep_prefix() );
}

TEST_CASE( "Decoder: JMP short", "[decoder][manual]" ) {
	// EB 10 = JMP short +10h
	const uint8_t data[] = { 0xEB, 0x10 };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->mnemonic() == Mnemonic::JMP );
	// Branch target: 0x1000 + 2 + 0x10 = 0x1012
	CHECK( result->near_branch32() == 0x1012 );
}

TEST_CASE( "Decoder: CALL near", "[decoder][manual]" ) {
	// E8 FB FF FF FF = CALL near -5 (calls itself)
	const uint8_t data[] = { 0xE8, 0xFB, 0xFF, 0xFF, 0xFF };
	Decoder decoder( 32, data, 0x1000 );
	
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	CHECK( result->mnemonic() == Mnemonic::CALL );
	// Branch target: 0x1000 + 5 + (-5) = 0x1000
	CHECK( result->near_branch32() == 0x1000 );
}

// ============================================================================
// Formatter tests
// ============================================================================

TEST_CASE( "Formatter: basic formatting", "[formatter][manual]" ) {
	// 90 = NOP
	const uint8_t data[] = { 0x90 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "nop" );
}

TEST_CASE( "Formatter: register operands", "[formatter][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, ebx" );
}

TEST_CASE( "Formatter: uppercase mode", "[formatter][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	formatter.options().set_uppercase_all( true );
	std::string output = formatter.format_to_string( *result );
	
	// Uppercase mode: both mnemonic and registers should be uppercase
	CHECK( output == "MOV EAX, EBX" );
}

TEST_CASE( "Formatter: immediate operand", "[formatter][manual]" ) {
	// 05 78 56 34 12 = ADD EAX, 0x12345678
	const uint8_t data[] = { 0x05, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "add eax, 12345678h" );
}

TEST_CASE( "Formatter: memory operand with size prefix", "[formatter][manual]" ) {
	// 8B 00 = MOV EAX, [EAX]
	const uint8_t data[] = { 0x8B, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, dword ptr [eax]" );
}

TEST_CASE( "Formatter: memory operand without size prefix", "[formatter][manual]" ) {
	// 8B 00 = MOV EAX, [EAX]
	const uint8_t data[] = { 0x8B, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, [eax]" );
}

TEST_CASE( "Formatter: memory with displacement", "[formatter][manual]" ) {
	// 8B 80 78 56 34 12 = MOV EAX, [EAX+0x12345678]
	const uint8_t data[] = { 0x8B, 0x80, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, [eax+12345678h]" );
}

TEST_CASE( "Formatter: SIB addressing", "[formatter][manual]" ) {
	// 8B 04 8B = MOV EAX, [EBX+ECX*4]
	const uint8_t data[] = { 0x8B, 0x04, 0x8B };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, [ebx+ecx*4]" );
}

TEST_CASE( "Formatter: LEA instruction", "[formatter][manual]" ) {
	// 8D 04 8B = LEA EAX, [EBX+ECX*4]
	const uint8_t data[] = { 0x8D, 0x04, 0x8B };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	// LEA doesn't use memory size prefix
	CHECK( output == "lea eax, [ebx+ecx*4]" );
}

TEST_CASE( "Formatter: format JMP instruction", "[formatter][manual]" ) {
	// EB 10 = JMP short +10h (relative)
	const uint8_t data[] = { 0xEB, 0x10 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	// Branch target: 0x1000 + 2 + 0x10 = 0x1012
	CHECK( output == "jmp 1012h" );
}

TEST_CASE( "Formatter: format CALL instruction", "[formatter][manual]" ) {
	// E8 FB FF FF FF = CALL near -5 (calls itself)
	const uint8_t data[] = { 0xE8, 0xFB, 0xFF, 0xFF, 0xFF };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	// Branch target: 0x1000 + 5 + (-5) = 0x1000
	CHECK( output == "call 1000h" );
}

TEST_CASE( "Formatter: format PUSH immediate", "[formatter][manual]" ) {
	// 68 78 56 34 12 = PUSH 0x12345678
	const uint8_t data[] = { 0x68, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	IntelFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "push 12345678h" );
}

TEST_CASE( "Formatter: register names", "[formatter][manual]" ) {
	IntelFormatter formatter;
	
	CHECK( formatter.format_register( Register::EAX ) == "eax" );
	CHECK( formatter.format_register( Register::RAX ) == "rax" );
	CHECK( formatter.format_register( Register::AL ) == "al" );
	CHECK( formatter.format_register( Register::R8 ) == "r8" );
	CHECK( formatter.format_register( Register::XMM0 ) == "xmm0" );
}

TEST_CASE( "Formatter: register names uppercase", "[formatter][manual]" ) {
	IntelFormatter formatter;
	formatter.options().set_uppercase_registers( true );
	
	CHECK( formatter.format_register( Register::EAX ) == "EAX" );
	CHECK( formatter.format_register( Register::RAX ) == "RAX" );
}

// ============================================================================
// VEX/EVEX infrastructure tests
// ============================================================================

TEST_CASE( "Decoder: VEX2 VZEROUPPER", "[decoder][vex][manual]" ) {
	// C5 F8 77 = VZEROUPPER (VEX2: C5, byte2=F8, opcode=77)
	// VEX.L=0, VEX.pp=00, VEX.R=1 (inverted), VEX.vvvv=1111 (inverted)
	const uint8_t data[] = { 0xC5, 0xF8, 0x77 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	// VEX prefix parsing: C5 + F8 + 77 = 3 bytes
	CHECK( result.length() == 3 );
	CHECK( result.code() == Code::VEX_VZEROUPPER );
}

TEST_CASE( "Decoder: VEX3 VMOVD xmm,r32", "[decoder][vex][manual]" ) {
	// C4 E1 79 6E C0 = VMOVD XMM0, EAX (VEX3: C4, RXB=E1, Wvvvv=79, opcode=6E, modrm=C0)
	// This is VEX.128.66.0F with W=0
	const uint8_t data[] = { 0xC4, 0xE1, 0x79, 0x6E, 0xC0 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	// VEX3: C4 + E1 + 79 + 6E + C0 = 5 bytes
	CHECK( result.length() == 5 );
	CHECK( result.code() == Code::VEX_VMOVD_XMM_RM32 );
	CHECK( result.op_register( 0 ) == Register::XMM0 );
	CHECK( result.op_register( 1 ) == Register::EAX );
}

TEST_CASE( "Decoder: EVEX VMOVDQA32 xmm,xmm", "[decoder][evex][manual]" ) {
	// 62 F1 7D 08 6F C1 = VMOVDQA32 XMM0, XMM1 (EVEX encoded)
	// EVEX: 62, P0=F1, P1=7D, P2=08, opcode=6F, modrm=C1
	// P0: R=1, X=1, B=1, R'=1, 00, mm=01 (0F map)
	// P1: W=0, vvvv=1111, 1, pp=01 (66 prefix)
	// P2: z=0, L'L=00, b=0, V'=1, aaa=000
	const uint8_t data[] = { 0x62, 0xF1, 0x7D, 0x08, 0x6F, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	// EVEX: 62 + P0 + P1 + P2 + opcode + modrm = 6 bytes
	CHECK( result.length() == 6 );
	CHECK( result.code() == Code::EVEX_VMOVDQA32_XMM_K1Z_XMMM128 );
	CHECK( result.op_register( 0 ) == Register::XMM0 );
	CHECK( result.op_register( 1 ) == Register::XMM1 );
}

TEST_CASE( "Decoder: EVEX VMOVDQA32 with opmask k1", "[decoder][evex][manual]" ) {
	// 62 F1 7D 09 6F C1 = VMOVDQA32 XMM0{k1}, XMM1
	// P2=09: z=0, L'L=00, b=0, V'=0, aaa=001 (k1)
	const uint8_t data[] = { 0x62, 0xF1, 0x7D, 0x09, 0x6F, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 6 );
	CHECK( result.code() == Code::EVEX_VMOVDQA32_XMM_K1Z_XMMM128 );
	CHECK( result.op_register( 0 ) == Register::XMM0 );
	CHECK( result.op_register( 1 ) == Register::XMM1 );
	CHECK( result.op_mask() == Register::K1 );
	CHECK( result.zeroing_masking() == false );
}

TEST_CASE( "Decoder: EVEX VMOVDQA32 with zeroing mask", "[decoder][evex][manual]" ) {
	// 62 F1 7D 89 6F C1 = VMOVDQA32 XMM0{k1}{z}, XMM1
	// P2=89: z=1, L'L=00, b=0, V'=0, aaa=001 (k1)
	const uint8_t data[] = { 0x62, 0xF1, 0x7D, 0x89, 0x6F, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 6 );
	CHECK( result.code() == Code::EVEX_VMOVDQA32_XMM_K1Z_XMMM128 );
	CHECK( result.op_register( 0 ) == Register::XMM0 );
	CHECK( result.op_register( 1 ) == Register::XMM1 );
	CHECK( result.op_mask() == Register::K1 );
	CHECK( result.zeroing_masking() == true );
}

TEST_CASE( "Decoder: EVEX ZMM registers (512-bit)", "[decoder][evex][manual]" ) {
	// 62 F1 7D 48 6F C1 = VMOVDQA32 ZMM0, ZMM1
	// P2=48: z=0, L'L=10 (512-bit), b=0, V'=0, aaa=000
	const uint8_t data[] = { 0x62, 0xF1, 0x7D, 0x48, 0x6F, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 6 );
	CHECK( result.code() == Code::EVEX_VMOVDQA32_ZMM_K1Z_ZMMM512 );
	CHECK( result.op_register( 0 ) == Register::ZMM0 );
	CHECK( result.op_register( 1 ) == Register::ZMM1 );
}

TEST_CASE( "Decoder: EVEX YMM registers (256-bit)", "[decoder][evex][manual]" ) {
	// 62 F1 7D 28 6F C1 = VMOVDQA32 YMM0, YMM1
	// P2=28: z=0, L'L=01 (256-bit), b=0, V'=0, aaa=000
	const uint8_t data[] = { 0x62, 0xF1, 0x7D, 0x28, 0x6F, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 6 );
	CHECK( result.code() == Code::EVEX_VMOVDQA32_YMM_K1Z_YMMM256 );
	CHECK( result.op_register( 0 ) == Register::YMM0 );
	CHECK( result.op_register( 1 ) == Register::YMM1 );
}

TEST_CASE( "Decoder: EVEX extended registers (XMM16+)", "[decoder][evex][manual]" ) {
	// 62 E1 7D 08 6F C1 = VMOVDQA32 XMM16, XMM1
	// P0=E1: R=1(inv=0), X=1(inv=0), B=1(inv=0), R'=0(inv=1), 00, mm=01
	// dest: reg=0, R=0, R'=1 -> 0 + 0 + 16 = 16 -> XMM16
	// src: rm=1, B=0 -> 1 + 0 = 1 -> XMM1
	const uint8_t data[] = { 0x62, 0xE1, 0x7D, 0x08, 0x6F, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 6 );
	CHECK( result.code() == Code::EVEX_VMOVDQA32_XMM_K1Z_XMMM128 );
	CHECK( result.op_register( 0 ) == Register::XMM16 );
	CHECK( result.op_register( 1 ) == Register::XMM1 );
}

TEST_CASE( "Decoder: VEX VADDPS xmm,xmm,xmm", "[decoder][vex][manual]" ) {
	// C5 E8 58 C2 = VADDPS XMM0, XMM2, XMM2 (VEX.128.0F.WIG 58 /r)
	// VEX2: C5, byte2=E8 (R=1, vvvv=1101=XMM2, L=0, pp=00), opcode=58, modrm=C2
	const uint8_t data[] = { 0xC5, 0xE8, 0x58, 0xC2 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 4 );
	CHECK( result.code() == Code::VEX_VADDPS_XMM_XMM_XMMM128 );
	CHECK( result.op_register( 0 ) == Register::XMM0 );  // dest from reg field
	CHECK( result.op_register( 1 ) == Register::XMM2 );  // src1 from vvvv
	CHECK( result.op_register( 2 ) == Register::XMM2 );  // src2 from r/m
}

TEST_CASE( "Decoder: VEX in 32-bit mode", "[decoder][vex][manual]" ) {
	// In 32-bit mode, C5 can be VEX2 or LES depending on modrm
	// C5 F8 77 = VZEROUPPER when modrm indicates register (mod=11)
	// When mod != 11, C5 is LES instruction
	const uint8_t data[] = { 0xC5, 0xF8, 0x77 };
	Decoder decoder( 32, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	// In 32-bit mode, C5 F8 has mod=11 (F8 >> 6 = 3), so it's VEX
	CHECK( result.length() == 3 );
	CHECK( result.code() == Code::VEX_VZEROUPPER );
}

TEST_CASE( "Formatter: EVEX with opmask", "[formatter][evex][manual]" ) {
	// 62 F1 7D 09 6F C1 = VMOVDQA32 XMM0{k1}, XMM1
	const uint8_t data[] = { 0x62, 0xF1, 0x7D, 0x09, 0x6F, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto instr = decoder.decode_out( error );
	
	// Verify opmask is set correctly
	CHECK( instr.op_mask() == Register::K1 );
	
	IntelFormatter formatter;
	std::string output;
	StringFormatterOutput fmt_output( output );
	formatter.format( instr, fmt_output );
	
	// Should contain the mnemonic
	CHECK( output.find( "vmovdqa32" ) != std::string::npos );
	// Should have xmm0 and xmm1
	CHECK( output.find( "xmm0" ) != std::string::npos );
	CHECK( output.find( "xmm1" ) != std::string::npos );
	// Check for {k1} decorator in output
	CHECK( output.find( "{k1}" ) != std::string::npos );
}

TEST_CASE( "Decoder: EVEX with embedded rounding", "[decoder][evex][manual]" ) {
	// VADDPS ZMM0{k1}, ZMM1, ZMM2, {rn-sae}
	// 62 F1 74 19 58 C2 - L'L=00 (rn-sae), B=1
	// P0=F1, P1=74 (vvvv=1110=ZMM1), P2=19 (z=0, L'L=00, b=1, aaa=001)
	const uint8_t data[] = { 0x62, 0xF1, 0x74, 0x19, 0x58, 0xC2 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 6 );
	// Should have rounding control set
	CHECK( result.rounding_control() == RoundingControl::ROUND_TO_NEAREST );
	CHECK( result.op_mask() == Register::K1 );
}

TEST_CASE( "Decoder: EVEX with SAE", "[decoder][evex][manual]" ) {
	// Some instructions use SAE (suppress all exceptions) instead of rounding
	// VUCOMISS XMM0, XMM1, {sae}
	// 62 F1 7C 18 2E C1 - P2=18 (z=0, L'L=00, b=1, aaa=000)
	const uint8_t data[] = { 0x62, 0xF1, 0x7C, 0x18, 0x2E, 0xC1 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( result.length() == 6 );
	// Check SAE flag is set
	CHECK( result.suppress_all_exceptions() == true );
}

// ============================================================================
// VSIB (Vector SIB) addressing tests - gather/scatter instructions
// ============================================================================

TEST_CASE( "Decoder: VEX VGATHERDPS (VSIB gather)", "[decoder][vex][vsib][manual]" ) {
	// VGATHERDPS xmm2, [rax+xmm1*4], xmm3
	// VEX.128.66.0F38.W0 92 /r
	// C4 E2 61 92 14 88 = VGATHERDPS XMM2, [RAX+XMM1*4], XMM3
	// VEX3: C4 = VEX3 prefix
	//       E2 = ~R:0 X:1 B:1 m-mmmm:00010 (0F38)
	//       61 = W:0 ~vvvv:1100 (xmm3) L:0 pp:01 (66)
	//       92 = opcode
	//       14 88 = ModRM (mod=00, reg=010, rm=100) + SIB (scale=10, index=001, base=000)
	const uint8_t data[] = { 0xC4, 0xE2, 0x61, 0x92, 0x14, 0x88 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( error == DecoderError::NONE );
	CHECK( result.length() == 6 );
	// Op0 should be XMM2 (reg=010)
	CHECK( result.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( result.op_register( 0 ) == Register::XMM2 );
	// Op1 should be memory with VSIB
	CHECK( result.op_kind( 1 ) == OpKind::MEMORY );
	CHECK( result.memory_base() == Register::RAX );
	CHECK( result.memory_index() == Register::XMM1 );
	CHECK( result.memory_index_scale() == 4 );
	// Op2 should be XMM3 (vvvv)
	CHECK( result.op_kind( 2 ) == OpKind::REGISTER );
	CHECK( result.op_register( 2 ) == Register::XMM3 );
}

TEST_CASE( "Decoder: EVEX VPGATHERDD (VSIB gather)", "[decoder][evex][vsib][manual]" ) {
	// VPGATHERDD xmm2{k1}, [rax+xmm1*4]
	// EVEX.128.66.0F38.W0 90 /vsib
	// 62 F2 7D 09 90 14 88
	// P0: 62 = EVEX prefix
	// P1: F2 = R:1 X:1 B:1 R':1 0 0 mm:10 (0F38)
	// P2: 7D = W:0 vvvv:1111 1 pp:01 (66)
	// P3: 09 = z:0 L'L:00 b:0 V':1 aaa:001 (k1)
	// 90 = opcode
	// 14 88 = ModRM + SIB
	const uint8_t data[] = { 0x62, 0xF2, 0x7D, 0x09, 0x90, 0x14, 0x88 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( error == DecoderError::NONE );
	CHECK( result.length() == 7 );
	// Op0: XMM2{k1}
	CHECK( result.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( result.op_register( 0 ) == Register::XMM2 );
	CHECK( result.op_mask() == Register::K1 );
	// Op1: memory with VSIB
	CHECK( result.op_kind( 1 ) == OpKind::MEMORY );
	CHECK( result.memory_base() == Register::RAX );
	CHECK( result.memory_index() == Register::XMM1 );
	CHECK( result.memory_index_scale() == 4 );
}

TEST_CASE( "Decoder: EVEX VPSCATTERDD (VSIB scatter)", "[decoder][evex][vsib][manual]" ) {
	// VPSCATTERDD [rax+xmm1*4]{k1}, xmm2
	// EVEX.128.66.0F38.W0 A0 /vsib
	// 62 F2 7D 09 A0 14 88
	const uint8_t data[] = { 0x62, 0xF2, 0x7D, 0x09, 0xA0, 0x14, 0x88 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( error == DecoderError::NONE );
	CHECK( result.length() == 7 );
	// Op0: memory with VSIB
	CHECK( result.op_kind( 0 ) == OpKind::MEMORY );
	CHECK( result.memory_base() == Register::RAX );
	CHECK( result.memory_index() == Register::XMM1 );
	CHECK( result.memory_index_scale() == 4 );
	// Op1: XMM2
	CHECK( result.op_kind( 1 ) == OpKind::REGISTER );
	CHECK( result.op_register( 1 ) == Register::XMM2 );
	// Mask
	CHECK( result.op_mask() == Register::K1 );
}

// ============================================================================
// AMX (Advanced Matrix Extensions) tests - tile register instructions
// ============================================================================

TEST_CASE( "Decoder: VEX TILEZERO (AMX)", "[decoder][vex][amx][manual]" ) {
	// TILEZERO TMM1
	// VEX.128.F2.0F38.W0 49 /r (mod=11, reg=tile, rm=0)
	// C4 E2 7B 49 C8
	// C4 = VEX3 prefix
	// E2 = R:1 X:1 B:1 mmmmm:00010 (0F38)
	// 7B = W:0 vvvv:1111 L:0 pp:11 (F2)
	// 49 = opcode
	// C8 = mod:11 reg:001 rm:000 (rm must be 0 for valid TILEZERO)
	const uint8_t data[] = { 0xC4, 0xE2, 0x7B, 0x49, 0xC8 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( error == DecoderError::NONE );
	CHECK( result.length() == 5 );
	CHECK( result.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( result.op_register( 0 ) == Register::TMM1 );
}

TEST_CASE( "Decoder: VEX TILELOADD (AMX)", "[decoder][vex][amx][manual]" ) {
	// TILELOADD TMM1, [rax+rbx*8]
	// VEX.128.F2.0F38.W0 4B /r
	// C4 E2 7B 4B 0C D8
	// 0C = mod:00 reg:001 rm:100 (SIB)
	// D8 = scale:11 (8) index:011 (rbx) base:000 (rax)
	const uint8_t data[] = { 0xC4, 0xE2, 0x7B, 0x4B, 0x0C, 0xD8 };
	Decoder decoder( 64, data, 0x1000 );
	
	DecoderError error;
	auto result = decoder.decode_out( error );
	
	CHECK( error == DecoderError::NONE );
	CHECK( result.length() == 6 );
	// Op0: TMM1
	CHECK( result.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( result.op_register( 0 ) == Register::TMM1 );
	// Op1: memory [rax+rbx*8]
	CHECK( result.op_kind( 1 ) == OpKind::MEMORY );
	CHECK( result.memory_base() == Register::RAX );
	CHECK( result.memory_index() == Register::RBX );
	CHECK( result.memory_index_scale() == 8 );
}

// ============================================================================
// Additional formatter tests - MASM, NASM, GAS formatters
// ============================================================================

TEST_CASE( "MasmFormatter: basic formatting", "[formatter][masm][manual]" ) {
	// 90 = NOP
	const uint8_t data[] = { 0x90 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	MasmFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "nop" );
}

TEST_CASE( "MasmFormatter: register operands", "[formatter][masm][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	MasmFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, ebx" );
}

TEST_CASE( "MasmFormatter: memory operand with size", "[formatter][masm][manual]" ) {
	// 8B 00 = MOV EAX, [EAX]
	const uint8_t data[] = { 0x8B, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	MasmFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	// MASM uses "dword ptr"
	CHECK( output == "mov eax, dword ptr [eax]" );
}

TEST_CASE( "NasmFormatter: basic formatting", "[formatter][nasm][manual]" ) {
	// 90 = NOP
	const uint8_t data[] = { 0x90 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	NasmFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "nop" );
}

TEST_CASE( "NasmFormatter: register operands", "[formatter][nasm][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	NasmFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, ebx" );
}

TEST_CASE( "NasmFormatter: memory operand with size", "[formatter][nasm][manual]" ) {
	// 8B 00 = MOV EAX, [EAX]
	const uint8_t data[] = { 0x8B, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	NasmFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	// NASM uses bare "dword" without "ptr"
	CHECK( output == "mov eax, dword [eax]" );
}

TEST_CASE( "GasFormatter: basic formatting", "[formatter][gas][manual]" ) {
	// 90 = NOP
	const uint8_t data[] = { 0x90 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "nop" );
}

TEST_CASE( "GasFormatter: register operands reversed", "[formatter][gas][manual]" ) {
	// 89 D8 = MOV EAX, EBX (Intel: mov eax, ebx)
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	// Disable size suffix in mnemonic for this test
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: operands reversed, registers have % prefix
	CHECK( output == "mov %ebx, %eax" );
}

TEST_CASE( "GasFormatter: immediate with $ prefix", "[formatter][gas][manual]" ) {
	// 05 78 56 34 12 = ADD EAX, 0x12345678
	const uint8_t data[] = { 0x05, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: immediate has $ prefix, operands reversed
	CHECK( output == "add $0x12345678, %eax" );
}

TEST_CASE( "GasFormatter: memory operand AT&T style", "[formatter][gas][manual]" ) {
	// 8B 00 = MOV EAX, [EAX] (Intel: mov eax, [eax])
	const uint8_t data[] = { 0x8B, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	// Disable size suffix in mnemonic for this test
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: memory uses parentheses
	CHECK( output == "mov (%eax), %eax" );
}

TEST_CASE( "GasFormatter: memory with displacement", "[formatter][gas][manual]" ) {
	// 8B 40 10 = MOV EAX, [EAX+0x10]
	const uint8_t data[] = { 0x8B, 0x40, 0x10 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	// Disable size suffix in mnemonic for this test
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: disp(%base)
	CHECK( output == "mov 0x10(%eax), %eax" );
}

TEST_CASE( "GasFormatter: memory with SIB", "[formatter][gas][manual]" ) {
	// 8B 04 88 = MOV EAX, [EAX+ECX*4]
	const uint8_t data[] = { 0x8B, 0x04, 0x88 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	// Disable size suffix in mnemonic for this test
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: (%base,%index,scale)
	CHECK( output == "mov (%eax,%ecx,4), %eax" );
}

// ============================================================================
// Additional comprehensive formatter tests
// ============================================================================

TEST_CASE( "MasmFormatter: immediate operand", "[formatter][masm][manual]" ) {
	// 05 78 56 34 12 = ADD EAX, 0x12345678
	const uint8_t data[] = { 0x05, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	MasmFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "add eax, 12345678h" );
}

TEST_CASE( "MasmFormatter: uppercase mode", "[formatter][masm][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	MasmFormatter formatter;
	formatter.options().set_uppercase_all( true );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "MOV EAX, EBX" );
}

TEST_CASE( "MasmFormatter: memory with SIB and displacement", "[formatter][masm][manual]" ) {
	// 8B 44 88 10 = MOV EAX, [EAX+ECX*4+0x10]
	const uint8_t data[] = { 0x8B, 0x44, 0x88, 0x10 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	MasmFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "mov eax, dword ptr [eax+ecx*4+10h]" );
}

TEST_CASE( "MasmFormatter: lock prefix", "[formatter][masm][manual]" ) {
	// F0 01 00 = LOCK ADD [EAX], EAX
	const uint8_t data[] = { 0xF0, 0x01, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	MasmFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "lock add dword ptr [eax], eax" );
}

TEST_CASE( "NasmFormatter: immediate operand", "[formatter][nasm][manual]" ) {
	// 05 78 56 34 12 = ADD EAX, 0x12345678
	const uint8_t data[] = { 0x05, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	NasmFormatter formatter;
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "add eax, 12345678h" );
}

TEST_CASE( "NasmFormatter: uppercase mode", "[formatter][nasm][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	NasmFormatter formatter;
	formatter.options().set_uppercase_all( true );
	std::string output = formatter.format_to_string( *result );
	
	CHECK( output == "MOV EAX, EBX" );
}

TEST_CASE( "NasmFormatter: memory with SIB and displacement", "[formatter][nasm][manual]" ) {
	// 8B 44 88 10 = MOV EAX, [EAX+ECX*4+0x10]
	const uint8_t data[] = { 0x8B, 0x44, 0x88, 0x10 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	NasmFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	// NASM uses bare "dword" without "ptr"
	CHECK( output == "mov eax, dword [eax+ecx*4+10h]" );
}

TEST_CASE( "NasmFormatter: lock prefix", "[formatter][nasm][manual]" ) {
	// F0 01 00 = LOCK ADD [EAX], EAX
	const uint8_t data[] = { 0xF0, 0x01, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	NasmFormatter formatter;
	formatter.options().set_show_memory_size( true );
	std::string output = formatter.format_to_string( *result );
	
	// NASM uses bare "dword" without "ptr"
	CHECK( output == "lock add dword [eax], eax" );
}

TEST_CASE( "GasFormatter: uppercase mode", "[formatter][gas][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	formatter.options().set_uppercase_all( true );
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: uppercase, operands reversed, registers have % prefix
	CHECK( output == "MOV %EBX, %EAX" );
}

TEST_CASE( "GasFormatter: memory with SIB and displacement", "[formatter][gas][manual]" ) {
	// 8B 44 88 10 = MOV EAX, [EAX+ECX*4+0x10]
	const uint8_t data[] = { 0x8B, 0x44, 0x88, 0x10 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: disp(%base,%index,scale)
	CHECK( output == "mov 0x10(%eax,%ecx,4), %eax" );
}

TEST_CASE( "GasFormatter: lock prefix", "[formatter][gas][manual]" ) {
	// F0 01 00 = LOCK ADD [EAX], EAX
	const uint8_t data[] = { 0xF0, 0x01, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	formatter.options().set_show_memory_size( false );
	std::string output = formatter.format_to_string( *result );
	
	// AT&T syntax: operands reversed
	CHECK( output == "lock add %eax, (%eax)" );
}

TEST_CASE( "GasFormatter: naked registers option", "[formatter][gas][manual]" ) {
	// 89 D8 = MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	formatter.options().set_show_memory_size( false );
	formatter.set_naked_registers( true );  // No % prefix
	std::string output = formatter.format_to_string( *result );
	
	// Naked registers: no % prefix
	CHECK( output == "mov ebx, eax" );
}

TEST_CASE( "GasFormatter: size suffix enabled", "[formatter][gas][manual]" ) {
	// 89 D8 = MOV EAX, EBX (32-bit)
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	GasFormatter formatter;
	formatter.options().set_show_memory_size( true );  // Enable size suffix
	std::string output = formatter.format_to_string( *result );
	
	// AT&T with size suffix: movl for 32-bit
	CHECK( output == "movl %ebx, %eax" );
}

TEST_CASE( "All formatters: 64-bit register operands", "[formatter][manual]" ) {
	// 48 89 D8 = MOV RAX, RBX (REX.W prefix)
	const uint8_t data[] = { 0x48, 0x89, 0xD8 };
	Decoder decoder( 64, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	// Intel
	{
		IntelFormatter formatter;
		std::string output = formatter.format_to_string( *result );
		CHECK( output == "mov rax, rbx" );
	}
	
	// MASM
	{
		MasmFormatter formatter;
		std::string output = formatter.format_to_string( *result );
		CHECK( output == "mov rax, rbx" );
	}
	
	// NASM
	{
		NasmFormatter formatter;
		std::string output = formatter.format_to_string( *result );
		CHECK( output == "mov rax, rbx" );
	}
	
	// GAS
	{
		GasFormatter formatter;
		formatter.options().set_show_memory_size( false );
		std::string output = formatter.format_to_string( *result );
		CHECK( output == "mov %rbx, %rax" );
	}
}

TEST_CASE( "All formatters: RIP-relative memory", "[formatter][manual]" ) {
	// 8B 05 10 00 00 00 = MOV EAX, [RIP+0x10] in 64-bit mode
	const uint8_t data[] = { 0x8B, 0x05, 0x10, 0x00, 0x00, 0x00 };
	Decoder decoder( 64, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	// Intel with memory size
	{
		IntelFormatter formatter;
		formatter.options().set_show_memory_size( true );
		std::string output = formatter.format_to_string( *result );
		// RIP-relative, target = 0x1000 + 6 + 0x10 = 0x1016
		CHECK( output == "mov eax, dword ptr [rip+1016h]" );
	}
	
	// NASM with memory size
	{
		NasmFormatter formatter;
		formatter.options().set_show_memory_size( true );
		std::string output = formatter.format_to_string( *result );
		// NASM uses bare "dword"
		CHECK( output == "mov eax, dword [rip+1016h]" );
	}
}

TEST_CASE( "All formatters: PUSH immediate", "[formatter][manual]" ) {
	// 68 78 56 34 12 = PUSH 0x12345678
	const uint8_t data[] = { 0x68, 0x78, 0x56, 0x34, 0x12 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	// Intel
	{
		IntelFormatter formatter;
		std::string output = formatter.format_to_string( *result );
		CHECK( output == "push 12345678h" );
	}
	
	// GAS
	{
		GasFormatter formatter;
		std::string output = formatter.format_to_string( *result );
		// AT&T: immediate has $ prefix
		CHECK( output == "push $0x12345678" );
	}
}

TEST_CASE( "All formatters: JMP near", "[formatter][manual]" ) {
	// E9 10 00 00 00 = JMP +0x10 (relative)
	const uint8_t data[] = { 0xE9, 0x10, 0x00, 0x00, 0x00 };
	Decoder decoder( 32, data, 0x1000 );
	auto result = decoder.decode();
	REQUIRE( result.has_value() );
	
	// Intel
	{
		IntelFormatter formatter;
		std::string output = formatter.format_to_string( *result );
		// Target = 0x1000 + 5 + 0x10 = 0x1015
		CHECK( output == "jmp 1015h" );
	}
	
	// GAS - branch targets don't have $ prefix
	{
		GasFormatter formatter;
		std::string output = formatter.format_to_string( *result );
		CHECK( output == "jmp 0x1015" );
	}
}
