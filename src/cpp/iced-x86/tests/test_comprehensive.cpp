// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Comprehensive tests for decoding, encoding, and instruction creation

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"
#include <vector>
#include <array>
#include <cstring>

using namespace iced_x86;

// ============================================================================
// Helper function for round-trip testing (decode -> encode -> compare)
// ============================================================================

static bool round_trip_test(uint32_t bitness, const uint8_t* bytes, size_t len, uint64_t ip = 0x1000) {
	Decoder decoder(bitness, std::span<const uint8_t>(bytes, len), ip);
	auto decode_result = decoder.decode();
	if (!decode_result.has_value()) return false;
	
	auto instr = *decode_result;
	if (instr.is_invalid()) return false;
	if (instr.length() != len) return false;
	
	Encoder encoder(bitness);
	auto encode_result = encoder.encode(instr, ip);
	if (!encode_result.has_value()) return false;
	if (*encode_result != len) return false;
	
	auto buffer = encoder.take_buffer();
	if (buffer.size() != len) return false;
	
	return std::memcmp(buffer.data(), bytes, len) == 0;
}

// ============================================================================
// Round-trip tests: decode then encode, verify bytes match
// ============================================================================

TEST_CASE("Round-trip: Legacy instructions 32-bit", "[roundtrip][legacy]") {
	SECTION("NOP") {
		const uint8_t bytes[] = {0x90};
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, imm32") {
		const uint8_t bytes[] = {0xB8, 0x78, 0x56, 0x34, 0x12};  // MOV EAX, 0x12345678
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, r32") {
		const uint8_t bytes[] = {0x89, 0xD8};  // MOV EAX, EBX
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("ADD r/m32, r32") {
		const uint8_t bytes[] = {0x01, 0xC8};  // ADD EAX, ECX
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("SUB r/m32, imm8") {
		const uint8_t bytes[] = {0x83, 0xE8, 0x10};  // SUB EAX, 0x10
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("PUSH imm32") {
		const uint8_t bytes[] = {0x68, 0x78, 0x56, 0x34, 0x12};  // PUSH 0x12345678
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, [r32]") {
		const uint8_t bytes[] = {0x8B, 0x00};  // MOV EAX, [EAX]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, [r32+disp8]") {
		const uint8_t bytes[] = {0x8B, 0x40, 0x10};  // MOV EAX, [EAX+0x10]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, [r32+disp32]") {
		const uint8_t bytes[] = {0x8B, 0x80, 0x00, 0x10, 0x00, 0x00};  // MOV EAX, [EAX+0x1000]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, [r32+r32*scale]") {
		const uint8_t bytes[] = {0x8B, 0x04, 0x88};  // MOV EAX, [EAX+ECX*4]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("LEA r32, [r32+r32*scale+disp8]") {
		const uint8_t bytes[] = {0x8D, 0x44, 0x88, 0x10};  // LEA EAX, [EAX+ECX*4+0x10]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
}

TEST_CASE("Round-trip: Legacy instructions 64-bit", "[roundtrip][legacy]") {
	SECTION("MOV r64, imm64") {
		const uint8_t bytes[] = {0x48, 0xB8, 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r64, r64") {
		const uint8_t bytes[] = {0x48, 0x89, 0xC3};  // MOV RBX, RAX
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r64, r64 with REX.R") {
		const uint8_t bytes[] = {0x4C, 0x89, 0xC0};  // MOV RAX, R8
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r64, r64 with REX.B") {
		const uint8_t bytes[] = {0x49, 0x89, 0xC0};  // MOV R8, RAX
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, r32 with REX.R in 64-bit") {
		const uint8_t bytes[] = {0x44, 0x89, 0xC0};  // MOV EAX, R8D
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r32, r32 with REX.B in 64-bit") {
		const uint8_t bytes[] = {0x41, 0x89, 0xC0};  // MOV R8D, EAX
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOV r64, [RIP+disp32]") {
		const uint8_t bytes[] = {0x48, 0x8B, 0x05, 0x00, 0x10, 0x00, 0x00};  // MOV RAX, [RIP+0x1000]
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("Extended registers R8-R15") {
		const uint8_t bytes[] = {0x4D, 0x89, 0xC1};  // MOV R9, R8
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
}

TEST_CASE("Round-trip: Branch instructions", "[roundtrip][branch]") {
	SECTION("JMP rel8 - 32-bit") {
		const uint8_t bytes[] = {0xEB, 0x10};  // JMP short +16
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("JMP rel8 - 64-bit") {
		const uint8_t bytes[] = {0xEB, 0x10};  // JMP short +16
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("JMP rel32 - 32-bit") {
		const uint8_t bytes[] = {0xE9, 0xFB, 0x0F, 0x00, 0x00};  // JMP near +0x1000
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("JMP rel32 - 64-bit") {
		const uint8_t bytes[] = {0xE9, 0xFB, 0x0F, 0x00, 0x00};  // JMP near +0x1000
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("CALL rel32") {
		const uint8_t bytes[] = {0xE8, 0xFB, 0xFF, 0xFF, 0xFF};  // CALL -5 (calls itself)
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("Jcc rel8") {
		const uint8_t bytes[] = {0x74, 0x10};  // JE short +16
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("Jcc rel32") {
		const uint8_t bytes[] = {0x0F, 0x84, 0xFA, 0x0F, 0x00, 0x00};  // JE near +0x1000
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
}

TEST_CASE("Round-trip: Prefix combinations", "[roundtrip][prefix]") {
	SECTION("LOCK ADD") {
		const uint8_t bytes[] = {0xF0, 0x01, 0x00};  // LOCK ADD [EAX], EAX
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("REP MOVSB") {
		const uint8_t bytes[] = {0xF3, 0xA4};  // REP MOVSB
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("REPNE SCASB") {
		const uint8_t bytes[] = {0xF2, 0xAE};  // REPNE SCASB
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("Operand size prefix 16-bit in 32-bit mode") {
		const uint8_t bytes[] = {0x66, 0x89, 0xD8};  // MOV AX, BX
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("Address size prefix") {
		const uint8_t bytes[] = {0x67, 0x8B, 0x00};  // MOV EAX, [EAX] with 16-bit addressing
		// Note: In 32-bit mode, 67h gives 16-bit addressing
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("Segment override CS") {
		const uint8_t bytes[] = {0x2E, 0x8B, 0x00};  // MOV EAX, CS:[EAX]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("Segment override FS") {
		const uint8_t bytes[] = {0x64, 0x8B, 0x00};  // MOV EAX, FS:[EAX]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
}

TEST_CASE("Round-trip: VEX instructions", "[roundtrip][vex]") {
	SECTION("VEX2 VZEROUPPER") {
		const uint8_t bytes[] = {0xC5, 0xF8, 0x77};  // VZEROUPPER
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("VEX2 VMOVD xmm, r32") {
		// VEX.128.66.0F.WIG 6E /r
		const uint8_t bytes[] = {0xC5, 0xF9, 0x6E, 0xC0};  // VMOVD XMM0, EAX
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("VEX3 VMOVD xmm, r32") {
		// Note: VEX3 encoding C4 E1 79 6E C0 can be optimized to VEX2 encoding C5 F9 6E C0
		// The encoder will use VEX2 when possible (no extended registers, W=0, table=0F)
		// This test verifies the instruction decodes and re-encodes correctly (semantically)
		const uint8_t vex3_bytes[] = {0xC4, 0xE1, 0x79, 0x6E, 0xC0};  // VMOVD XMM0, EAX (VEX3)
		const uint8_t vex2_bytes[] = {0xC5, 0xF9, 0x6E, 0xC0};         // VMOVD XMM0, EAX (VEX2)
		
		Decoder decoder(64, std::span<const uint8_t>(vex3_bytes, sizeof(vex3_bytes)), 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		auto instr = *result;
		CHECK(!instr.is_invalid());
		
		Encoder encoder(64);
		auto encode_result = encoder.encode(instr, 0x1000);
		REQUIRE(encode_result.has_value());
		
		// Encoder may produce VEX2 (4 bytes) or VEX3 (5 bytes) - both are valid
		auto buffer = encoder.take_buffer();
		bool matches_vex2 = (buffer.size() == sizeof(vex2_bytes)) && 
		                    (std::memcmp(buffer.data(), vex2_bytes, sizeof(vex2_bytes)) == 0);
		bool matches_vex3 = (buffer.size() == sizeof(vex3_bytes)) && 
		                    (std::memcmp(buffer.data(), vex3_bytes, sizeof(vex3_bytes)) == 0);
		CHECK((matches_vex2 || matches_vex3));
	}
	
	SECTION("VEX VADDPS xmm, xmm, xmm") {
		// VEX.128.0F.WIG 58 /r
		const uint8_t bytes[] = {0xC5, 0xE8, 0x58, 0xC2};  // VADDPS XMM0, XMM2, XMM2
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("VEX VADDPS ymm, ymm, ymm") {
		// VEX.256.0F.WIG 58 /r (L=1)
		const uint8_t bytes[] = {0xC5, 0xEC, 0x58, 0xC2};  // VADDPS YMM0, YMM2, YMM2
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("VEX3 with extended registers") {
		// VADDPS XMM8, XMM9, XMM10
		const uint8_t bytes[] = {0xC4, 0x41, 0x30, 0x58, 0xC2};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
}

TEST_CASE("Round-trip: EVEX instructions", "[roundtrip][evex]") {
	SECTION("EVEX VMOVDQA32 xmm, xmm") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x08, 0x6F, 0xC1};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("EVEX VMOVDQA32 ymm, ymm") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x28, 0x6F, 0xC1};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("EVEX VMOVDQA32 zmm, zmm") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x48, 0x6F, 0xC1};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("EVEX with opmask k1") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x09, 0x6F, 0xC1};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("EVEX with zeroing mask") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x89, 0x6F, 0xC1};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("EVEX extended registers XMM16") {
		const uint8_t bytes[] = {0x62, 0xE1, 0x7D, 0x08, 0x6F, 0xC1};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("EVEX with memory operand") {
		// VMOVDQA32 XMM0, [RAX]
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x08, 0x6F, 0x00};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("EVEX with broadcast") {
		// VADDPS ZMM0, ZMM1, dword ptr [RAX]{1to16}
		const uint8_t bytes[] = {0x62, 0xF1, 0x74, 0x58, 0x58, 0x00};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
}

// ============================================================================
// Instruction creation tests
// ============================================================================

TEST_CASE("Instruction creation: basic factory methods", "[instruction][factory]") {
	SECTION("Create NOP instruction") {
		auto instr = InstructionFactory::with(Code::NOPD);
		CHECK(instr.code() == Code::NOPD);
		CHECK(instr.op_count() == 0);
	}
	
	SECTION("Create instruction with register operand") {
		auto instr = InstructionFactory::with1(Code::PUSH_R64, Register::RAX);
		CHECK(instr.code() == Code::PUSH_R64);
		CHECK(instr.op_count() == 1);
		CHECK(instr.op0_kind() == OpKind::REGISTER);
		CHECK(instr.op0_register() == Register::RAX);
	}
	
	SECTION("Create instruction with two registers") {
		auto instr = InstructionFactory::with2(Code::MOV_R64_RM64, Register::RAX, Register::RBX);
		CHECK(instr.code() == Code::MOV_R64_RM64);
		CHECK(instr.op_count() == 2);
		CHECK(instr.op0_register() == Register::RAX);
		CHECK(instr.op1_register() == Register::RBX);
	}
	
	SECTION("Create instruction with register and immediate") {
		auto instr = InstructionFactory::with2(Code::MOV_R32_IMM32, Register::EAX, 0x12345678u);
		CHECK(instr.code() == Code::MOV_R32_IMM32);
		CHECK(instr.op0_register() == Register::EAX);
		CHECK(instr.immediate32() == 0x12345678u);
	}
	
	SECTION("Create instruction with memory operand") {
		MemoryOperand mem;
		mem.base = Register::RAX;
		mem.displacement = 0x100;
		mem.displ_size = 4;
		
		auto instr = InstructionFactory::with2(Code::MOV_R32_RM32, Register::EAX, mem);
		CHECK(instr.code() == Code::MOV_R32_RM32);
		CHECK(instr.op0_register() == Register::EAX);
		CHECK(instr.op1_kind() == OpKind::MEMORY);
		CHECK(instr.memory_base() == Register::RAX);
		CHECK(instr.memory_displacement64() == 0x100);
	}
	
	SECTION("Create instruction with SIB memory operand") {
		MemoryOperand mem = MemoryOperand::with_base_index_scale_displ_size(
			Register::RAX, Register::RCX, 4, 0x80, 1);
		
		auto instr = InstructionFactory::with2(Code::LEA_R64_M, Register::RDX, mem);
		CHECK(instr.code() == Code::LEA_R64_M);
		CHECK(instr.op0_register() == Register::RDX);
		CHECK(instr.memory_base() == Register::RAX);
		CHECK(instr.memory_index() == Register::RCX);
		CHECK(instr.memory_index_scale() == 4);
		CHECK(instr.memory_displacement64() == 0x80);
	}
}

TEST_CASE("Instruction creation: branch instructions", "[instruction][factory][branch]") {
	SECTION("Create JMP with target") {
		auto instr = InstructionFactory::with_branch(Code::JMP_REL32_64, 0x12345678);
		CHECK(instr.code() == Code::JMP_REL32_64);
		CHECK(instr.op0_kind() == OpKind::NEAR_BRANCH64);
		CHECK(instr.near_branch64() == 0x12345678);
	}
	
	SECTION("Create far CALL") {
		auto instr = InstructionFactory::with_far_branch(Code::CALL_PTR1632, 0x1234, 0x56789ABC);
		CHECK(instr.code() == Code::CALL_PTR1632);
		CHECK(instr.far_branch_selector() == 0x1234);
		CHECK(instr.far_branch32() == 0x56789ABC);
	}
}

// ============================================================================
// Declare data tests
// ============================================================================

TEST_CASE("Instruction: declare byte data", "[instruction][declare]") {
	Instruction instr;
	instr.set_code(Code::DECLARE_BYTE);
	instr.set_declare_data_len(4);
	
	instr.set_declare_byte_value(0, 0x12);
	instr.set_declare_byte_value(1, 0x34);
	instr.set_declare_byte_value(2, 0x56);
	instr.set_declare_byte_value(3, 0x78);
	
	CHECK(instr.declare_data_len() == 4);
	CHECK(instr.get_declare_byte_value(0) == 0x12);
	CHECK(instr.get_declare_byte_value(1) == 0x34);
	CHECK(instr.get_declare_byte_value(2) == 0x56);
	CHECK(instr.get_declare_byte_value(3) == 0x78);
}

TEST_CASE("Instruction: declare word data", "[instruction][declare]") {
	Instruction instr;
	instr.set_code(Code::DECLARE_WORD);
	instr.set_declare_data_len(4);
	
	instr.set_declare_word_value(0, 0x1234);
	instr.set_declare_word_value(1, 0x5678);
	instr.set_declare_word_value(2, 0x9ABC);
	instr.set_declare_word_value(3, 0xDEF0);
	
	CHECK(instr.declare_data_len() == 4);
	CHECK(instr.get_declare_word_value(0) == 0x1234);
	CHECK(instr.get_declare_word_value(1) == 0x5678);
	CHECK(instr.get_declare_word_value(2) == 0x9ABC);
	CHECK(instr.get_declare_word_value(3) == 0xDEF0);
}

TEST_CASE("Instruction: declare dword data", "[instruction][declare]") {
	Instruction instr;
	instr.set_code(Code::DECLARE_DWORD);
	instr.set_declare_data_len(4);
	
	instr.set_declare_dword_value(0, 0x12345678);
	instr.set_declare_dword_value(1, 0x9ABCDEF0);
	instr.set_declare_dword_value(2, 0x11223344);
	instr.set_declare_dword_value(3, 0x55667788);
	
	CHECK(instr.declare_data_len() == 4);
	CHECK(instr.get_declare_dword_value(0) == 0x12345678);
	CHECK(instr.get_declare_dword_value(1) == 0x9ABCDEF0);
	CHECK(instr.get_declare_dword_value(2) == 0x11223344);
	CHECK(instr.get_declare_dword_value(3) == 0x55667788);
}

TEST_CASE("Instruction: declare qword data", "[instruction][declare]") {
	Instruction instr;
	instr.set_code(Code::DECLARE_QWORD);
	instr.set_declare_data_len(2);
	
	instr.set_declare_qword_value(0, 0x123456789ABCDEF0ULL);
	instr.set_declare_qword_value(1, 0xFEDCBA9876543210ULL);
	
	CHECK(instr.declare_data_len() == 2);
	CHECK(instr.get_declare_qword_value(0) == 0x123456789ABCDEF0ULL);
	CHECK(instr.get_declare_qword_value(1) == 0xFEDCBA9876543210ULL);
}

TEST_CASE("Instruction: declare byte all 16 values", "[instruction][declare]") {
	Instruction instr;
	instr.set_code(Code::DECLARE_BYTE);
	instr.set_declare_data_len(16);
	
	for (uint32_t i = 0; i < 16; ++i) {
		instr.set_declare_byte_value(i, static_cast<uint8_t>(i * 10));
	}
	
	CHECK(instr.declare_data_len() == 16);
	for (uint32_t i = 0; i < 16; ++i) {
		CHECK(instr.get_declare_byte_value(i) == static_cast<uint8_t>(i * 10));
	}
}

// ============================================================================
// MVEX instruction tests
// ============================================================================

TEST_CASE("Instruction: MVEX eviction hint", "[instruction][mvex]") {
	Instruction instr;
	// Use an MVEX code (need to know the actual MVEX code range)
	// MVEX codes start at IcedConstants::MVEX_START
	// For testing, just verify the API works with flag manipulation
	
	// Test on a non-MVEX instruction first - should return false
	instr.set_code(Code::NOPD);
	CHECK(instr.is_mvex_eviction_hint() == false);
	
	// Set eviction hint (modifies immediate_)
	instr.set_is_mvex_eviction_hint(true);
	// For non-MVEX, is_mvex_eviction_hint still returns false (code check)
	CHECK(instr.is_mvex_eviction_hint() == false);
}

TEST_CASE("Instruction: MVEX reg/mem conversion", "[instruction][mvex]") {
	Instruction instr;
	
	// Test on a non-MVEX instruction - should return NONE
	instr.set_code(Code::NOPD);
	CHECK(instr.mvex_reg_mem_conv() == MvexRegMemConv::NONE);
	
	// Set a conversion value
	instr.set_mvex_reg_mem_conv(MvexRegMemConv::MEM_CONV_FLOAT16);
	// For non-MVEX, still returns NONE (code check)
	CHECK(instr.mvex_reg_mem_conv() == MvexRegMemConv::NONE);
}

// ============================================================================
// Encoder-specific tests
// ============================================================================

TEST_CASE("Encoder: encode with different bitness", "[encoder]") {
	SECTION("32-bit mode MOV") {
		Instruction instr;
		instr.set_code(Code::MOV_R32_IMM32);
		instr.set_op0_register(Register::EAX);
		instr.set_op0_kind(OpKind::REGISTER);
		instr.set_op1_kind(OpKind::IMMEDIATE32);
		instr.set_immediate32(0x12345678);
		
		Encoder encoder(32);
		auto result = encoder.encode(instr, 0x1000);
		REQUIRE(result.has_value());
		CHECK(*result == 5);
		
		auto buffer = encoder.take_buffer();
		CHECK(buffer[0] == 0xB8);  // MOV EAX, imm32
	}
	
	SECTION("64-bit mode MOV with REX.W") {
		Instruction instr;
		instr.set_code(Code::MOV_R64_IMM64);
		instr.set_op0_register(Register::RAX);
		instr.set_op0_kind(OpKind::REGISTER);
		instr.set_op1_kind(OpKind::IMMEDIATE64);
		instr.set_immediate64(0x123456789ABCDEF0ULL);
		
		Encoder encoder(64);
		auto result = encoder.encode(instr, 0x1000);
		REQUIRE(result.has_value());
		CHECK(*result == 10);  // REX.W + B8 + 8 bytes imm
		
		auto buffer = encoder.take_buffer();
		CHECK(buffer[0] == 0x48);  // REX.W
		CHECK(buffer[1] == 0xB8);  // MOV RAX, imm64
	}
}

TEST_CASE("Encoder: encode multiple instructions sequentially", "[encoder]") {
	Encoder encoder(64);
	
	// Encode: NOP; NOP; NOP; RET
	std::vector<Code> codes = {Code::NOPD, Code::NOPD, Code::NOPD, Code::RETNQ};
	
	uint64_t ip = 0x1000;
	for (auto code : codes) {
		Instruction instr;
		instr.set_code(code);
		auto result = encoder.encode(instr, ip);
		REQUIRE(result.has_value());
		ip += *result;
	}
	
	auto buffer = encoder.take_buffer();
	CHECK(buffer.size() == 4);  // 3 NOPs + 1 RET
	CHECK(buffer[0] == 0x90);
	CHECK(buffer[1] == 0x90);
	CHECK(buffer[2] == 0x90);
	CHECK(buffer[3] == 0xC3);  // RET
}

TEST_CASE("Encoder: error handling for invalid instructions", "[encoder]") {
	Encoder encoder(64);
	
	SECTION("Invalid instruction code") {
		Instruction instr;
		instr.set_code(Code::INVALID);
		
		auto result = encoder.encode(instr, 0x1000);
		CHECK(!result.has_value());
	}
}

// ============================================================================
// Decoder error handling tests
// ============================================================================

TEST_CASE("Decoder: error handling", "[decoder][error]") {
	SECTION("Empty input") {
		std::span<const uint8_t> empty;
		Decoder decoder(64, empty, 0x1000);
		
		CHECK(!decoder.can_decode());
		auto result = decoder.decode();
		CHECK(!result.has_value());
	}
	
	SECTION("Truncated instruction") {
		// MOV EAX, imm32 needs 5 bytes, only provide 3
		const uint8_t bytes[] = {0xB8, 0x12, 0x34};
		Decoder decoder(64, bytes, 0x1000);
		
		auto result = decoder.decode();
		// Should either fail or return an invalid/partial instruction
		// depending on implementation
	}
	
	SECTION("Invalid opcode") {
		// 0F 0F is 3DNow! prefix without valid suffix
		const uint8_t bytes[] = {0x0F, 0x0F, 0xC0, 0xFF};
		Decoder decoder(64, bytes, 0x1000);
		
		DecoderError error;
		auto instr = decoder.decode_out(error);
		// Should handle gracefully
	}
}

// ============================================================================
// Complex addressing modes
// ============================================================================

TEST_CASE("Decoder: complex addressing modes", "[decoder][memory]") {
	SECTION("RIP-relative addressing") {
		// MOV RAX, [RIP+0x12345678]
		const uint8_t bytes[] = {0x48, 0x8B, 0x05, 0x78, 0x56, 0x34, 0x12};
		Decoder decoder(64, bytes, 0x1000);
		
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::RIP);
	}
	
	SECTION("SIB with all components") {
		// MOV EAX, [EBX+ECX*8+0x12345678]
		const uint8_t bytes[] = {0x8B, 0x84, 0xCB, 0x78, 0x56, 0x34, 0x12};
		Decoder decoder(32, bytes, 0x1000);
		
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::EBX);
		CHECK(result->memory_index() == Register::ECX);
		CHECK(result->memory_index_scale() == 8);
		CHECK(result->memory_displacement64() == 0x12345678);
	}
	
	SECTION("SIB with no base (disp32)") {
		// MOV EAX, [ECX*4+0x12345678]
		const uint8_t bytes[] = {0x8B, 0x04, 0x8D, 0x78, 0x56, 0x34, 0x12};
		Decoder decoder(32, bytes, 0x1000);
		
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_index() == Register::ECX);
		CHECK(result->memory_index_scale() == 4);
		CHECK(result->memory_displacement64() == 0x12345678);
	}
}

// ============================================================================
// FPU and SSE instructions
// ============================================================================

TEST_CASE("Round-trip: FPU instructions", "[roundtrip][fpu]") {
	SECTION("FLD m32fp") {
		const uint8_t bytes[] = {0xD9, 0x00};  // FLD dword ptr [EAX]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("FADD ST(0), ST(1)") {
		const uint8_t bytes[] = {0xD8, 0xC1};  // FADD ST, ST(1)
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("FIST m32int") {
		const uint8_t bytes[] = {0xDB, 0x10};  // FIST dword ptr [EAX]
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
}

TEST_CASE("Round-trip: SSE instructions", "[roundtrip][sse]") {
	SECTION("MOVAPS xmm, xmm") {
		const uint8_t bytes[] = {0x0F, 0x28, 0xC1};  // MOVAPS XMM0, XMM1
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOVAPS xmm, m128") {
		const uint8_t bytes[] = {0x0F, 0x28, 0x00};  // MOVAPS XMM0, [RAX]
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("ADDPS xmm, xmm") {
		const uint8_t bytes[] = {0x0F, 0x58, 0xC1};  // ADDPS XMM0, XMM1
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOVD xmm, r32") {
		const uint8_t bytes[] = {0x66, 0x0F, 0x6E, 0xC0};  // MOVD XMM0, EAX
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
}

// ============================================================================
// Special instruction encodings
// ============================================================================

TEST_CASE("Round-trip: Special encodings", "[roundtrip][special]") {
	SECTION("XCHG EAX, r32 (short form)") {
		const uint8_t bytes[] = {0x91};  // XCHG EAX, ECX
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("INC r32 (short form in 32-bit)") {
		const uint8_t bytes[] = {0x40};  // INC EAX
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("PUSH r64 (short form)") {
		const uint8_t bytes[] = {0x50};  // PUSH RAX
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("PUSH r64 extended") {
		const uint8_t bytes[] = {0x41, 0x50};  // PUSH R8
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("MOVSXD r64, r32") {
		const uint8_t bytes[] = {0x48, 0x63, 0xC0};  // MOVSXD RAX, EAX
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
}

// Debug test for VEX encoding
TEST_CASE("Debug: VEX VADDPS ymm encoding", "[debug]") {
	// VADDPS YMM0, YMM2, YMM2: C5 EC 58 C2
	const uint8_t bytes[] = {0xC5, 0xEC, 0x58, 0xC2};
	
	Decoder decoder(64, std::span<const uint8_t>(bytes, sizeof(bytes)), 0x1000);
	auto result = decoder.decode();
	
	REQUIRE(result.has_value());
	auto instr = *result;
	
	INFO("Decoded code: " << static_cast<unsigned>(instr.code()));
	INFO("Decoded length: " << instr.length());
	INFO("op_count: " << instr.op_count());
	INFO("op0_kind: " << static_cast<unsigned>(instr.op0_kind()));
	INFO("op0_register: " << static_cast<unsigned>(instr.op0_register()));
	INFO("op1_kind: " << static_cast<unsigned>(instr.op1_kind()));
	INFO("op1_register: " << static_cast<unsigned>(instr.op1_register()));
	INFO("op2_kind: " << static_cast<unsigned>(instr.op2_kind()));
	INFO("op2_register: " << static_cast<unsigned>(instr.op2_register()));
	
	CHECK(!instr.is_invalid());
	CHECK(instr.length() == 4);
	
	Encoder encoder(64);
	auto encode_result = encoder.encode(instr, 0x1000);
	
	if (encode_result.has_value()) {
		INFO("Encode succeeded, length: " << *encode_result);
		auto buffer = encoder.take_buffer();
		
		std::string encoded_hex;
		for (auto b : buffer) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", b);
			encoded_hex += hex;
		}
		INFO("Encoded bytes: " << encoded_hex);
		
		std::string original_hex;
		for (size_t i = 0; i < sizeof(bytes); i++) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", bytes[i]);
			original_hex += hex;
		}
		INFO("Original bytes: " << original_hex);
		
		CHECK(*encode_result == sizeof(bytes));
		if (buffer.size() == sizeof(bytes)) {
			CHECK(std::memcmp(buffer.data(), bytes, sizeof(bytes)) == 0);
		}
	} else {
		INFO("Encode failed! Error: " << encode_result.error().message);
		CHECK(false);
	}
}

// Debug test to understand encoding issues
TEST_CASE("Debug: MOVAPS encoding analysis", "[debug]") {
	// MOVAPS XMM0, XMM1: 0F 28 C1
	const uint8_t bytes[] = {0x0F, 0x28, 0xC1};
	
	Decoder decoder(64, std::span<const uint8_t>(bytes, sizeof(bytes)), 0x1000);
	auto result = decoder.decode();
	
	REQUIRE(result.has_value());
	auto instr = *result;
	
	INFO("Decoded code: " << static_cast<unsigned>(instr.code()));
	INFO("Decoded length: " << instr.length());
	INFO("op_count: " << instr.op_count());
	INFO("op0_kind: " << static_cast<unsigned>(instr.op0_kind()));
	INFO("op0_register: " << static_cast<unsigned>(instr.op0_register()));
	INFO("op1_kind: " << static_cast<unsigned>(instr.op1_kind()));
	INFO("op1_register: " << static_cast<unsigned>(instr.op1_register()));
	
	CHECK(!instr.is_invalid());
	CHECK(instr.length() == 3);
	
	Encoder encoder(64);
	auto encode_result = encoder.encode(instr, 0x1000);
	
	if (encode_result.has_value()) {
		INFO("Encode succeeded, length: " << *encode_result);
		auto buffer = encoder.take_buffer();
		
		std::string encoded_hex;
		for (auto b : buffer) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", b);
			encoded_hex += hex;
		}
		INFO("Encoded bytes: " << encoded_hex);
		
		std::string original_hex;
		for (size_t i = 0; i < sizeof(bytes); i++) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", bytes[i]);
			original_hex += hex;
		}
		INFO("Original bytes: " << original_hex);
		
		CHECK(*encode_result == sizeof(bytes));
		if (buffer.size() == sizeof(bytes)) {
			CHECK(std::memcmp(buffer.data(), bytes, sizeof(bytes)) == 0);
		}
	} else {
		INFO("Encode failed!");
		CHECK(false);
	}
}

// Debug test for RIP-relative addressing
TEST_CASE("Debug: RIP-relative addressing", "[debug]") {
	// MOV RAX, [RIP+0x1000]: 48 8B 05 00 10 00 00
	const uint8_t bytes[] = {0x48, 0x8B, 0x05, 0x00, 0x10, 0x00, 0x00};
	
	Decoder decoder(64, std::span<const uint8_t>(bytes, sizeof(bytes)), 0x1000);
	auto result = decoder.decode();
	
	REQUIRE(result.has_value());
	auto instr = *result;
	
	INFO("Decoded code: " << static_cast<unsigned>(instr.code()));
	INFO("Decoded length: " << instr.length());
	INFO("op_count: " << instr.op_count());
	INFO("op0_kind: " << static_cast<unsigned>(instr.op0_kind()));
	INFO("op0_register: " << static_cast<unsigned>(instr.op0_register()));
	INFO("op1_kind: " << static_cast<unsigned>(instr.op1_kind()));
	INFO("memory_base: " << static_cast<unsigned>(instr.memory_base()));
	INFO("memory_displacement64: " << instr.memory_displacement64());
	
	CHECK(!instr.is_invalid());
	CHECK(instr.length() == 7);
	CHECK(instr.op1_kind() == OpKind::MEMORY);
	CHECK(instr.memory_base() == Register::RIP);
	
	Encoder encoder(64);
	auto encode_result = encoder.encode(instr, 0x1000);
	
	if (encode_result.has_value()) {
		INFO("Encode succeeded, length: " << *encode_result);
		auto buffer = encoder.take_buffer();
		
		std::string encoded_hex;
		for (auto b : buffer) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", b);
			encoded_hex += hex;
		}
		INFO("Encoded bytes: " << encoded_hex);
		
		std::string original_hex;
		for (size_t i = 0; i < sizeof(bytes); i++) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", bytes[i]);
			original_hex += hex;
		}
		INFO("Original bytes: " << original_hex);
		
		CHECK(*encode_result == sizeof(bytes));
		
		// Also check byte-by-byte match
		bool matches = (buffer.size() == sizeof(bytes)) && 
		               (std::memcmp(buffer.data(), bytes, sizeof(bytes)) == 0);
		CHECK(matches);
	} else {
		INFO("Encode failed! Error: " << encode_result.error().message);
		CHECK(false);
	}
}

// Debug test for FPU FLD
TEST_CASE("Debug: FPU FLD m32fp", "[debug]") {
	// FLD dword ptr [EAX]: D9 00
	const uint8_t bytes[] = {0xD9, 0x00};
	
	Decoder decoder(32, std::span<const uint8_t>(bytes, sizeof(bytes)), 0x1000);
	auto result = decoder.decode();
	
	REQUIRE(result.has_value());
	auto instr = *result;
	
	INFO("Decoded code: " << static_cast<unsigned>(instr.code()));
	INFO("Decoded length: " << instr.length());
	INFO("op_count: " << instr.op_count());
	INFO("op0_kind: " << static_cast<unsigned>(instr.op0_kind()));
	INFO("memory_base: " << static_cast<unsigned>(instr.memory_base()));
	
	CHECK(!instr.is_invalid());
	CHECK(instr.length() == 2);
	
	Encoder encoder(32);
	auto encode_result = encoder.encode(instr, 0x1000);
	
	if (encode_result.has_value()) {
		INFO("Encode succeeded, length: " << *encode_result);
		auto buffer = encoder.take_buffer();
		
		std::string encoded_hex;
		for (auto b : buffer) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", b);
			encoded_hex += hex;
		}
		INFO("Encoded bytes: " << encoded_hex);
		
		CHECK(*encode_result == sizeof(bytes));
		
		// Check byte-by-byte match
		bool matches = (buffer.size() == sizeof(bytes)) && 
		               (std::memcmp(buffer.data(), bytes, sizeof(bytes)) == 0);
		INFO("Bytes match: " << matches);
		CHECK(matches);
	} else {
		INFO("Encode failed! Error: " << encode_result.error().message);
		CHECK(false);
	}
}

// Debug test for EVEX VMOVDQA32
TEST_CASE("Debug: EVEX VMOVDQA32 xmm, xmm", "[debug]") {
	// VMOVDQA32 XMM0, XMM1: 62 F1 7D 08 6F C1
	const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x08, 0x6F, 0xC1};
	
	// Full decode-encode cycle with detailed output
	Decoder decoder(64, std::span<const uint8_t>(bytes, sizeof(bytes)), 0x1000);
	auto result = decoder.decode();
	
	REQUIRE(result.has_value());
	auto instr = *result;
	
	INFO("Decoded code: " << static_cast<unsigned>(instr.code()));
	INFO("Decoded length: " << instr.length());
	INFO("op_count: " << instr.op_count());
	INFO("op0_kind: " << static_cast<unsigned>(instr.op0_kind()));
	INFO("op0_register: " << static_cast<unsigned>(instr.op0_register()));
	INFO("op1_kind: " << static_cast<unsigned>(instr.op1_kind()));
	INFO("op1_register: " << static_cast<unsigned>(instr.op1_register()));
	
	CHECK(!instr.is_invalid());
	CHECK(instr.length() == 6);
	
	Encoder encoder(64);
	auto encode_result = encoder.encode(instr, 0x1000);
	
	if (encode_result.has_value()) {
		INFO("Encode succeeded, length: " << *encode_result);
		auto buffer = encoder.take_buffer();
		
		std::string encoded_hex;
		for (auto b : buffer) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", b);
			encoded_hex += hex;
		}
		INFO("Encoded bytes: " << encoded_hex);
		
		std::string original_hex;
		for (size_t i = 0; i < sizeof(bytes); i++) {
			char hex[4];
			snprintf(hex, sizeof(hex), "%02X ", bytes[i]);
			original_hex += hex;
		}
		INFO("Original bytes: " << original_hex);
		
		CHECK(*encode_result == sizeof(bytes));
	} else {
		INFO("Encode failed! Error: " << encode_result.error().message);
		CHECK(false);
	}
}
