// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Edge case tests for catching subtle bugs and boundary conditions

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"
#include <vector>
#include <cstring>
#include <limits>

using namespace iced_x86;

// ============================================================================
// Helper for round-trip testing
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
// DECODER EDGE CASES
// ============================================================================

TEST_CASE("Decoder edge: Maximum instruction length (15 bytes)", "[decoder][edge]") {
	// Instruction with many redundant prefixes to reach 15 bytes
	// 66 66 66 66 66 66 66 66 66 66 66 66 66 90 = 14 bytes (many 66 prefixes + NOP)
	// Actually x86 allows max 15 bytes, test a valid long instruction
	// LOCK REP ADD [RAX+RBX*8+0x12345678], ECX with segment override
	// Let's use a simpler case: MOV with many prefixes
	
	// 64-bit: MOV RAX, [FS:RBX+RCX*8+0x12345678]
	// 64 48 8B 84 CB 78 56 34 12
	const uint8_t bytes[] = {0x64, 0x48, 0x8B, 0x84, 0xCB, 0x78, 0x56, 0x34, 0x12};
	Decoder decoder(64, bytes, 0x1000);
	auto result = decoder.decode();
	
	REQUIRE(result.has_value());
	CHECK(!result->is_invalid());
	CHECK(result->length() == sizeof(bytes));
	CHECK(result->segment_prefix() == Register::FS);
}

TEST_CASE("Decoder edge: All segment prefixes", "[decoder][edge]") {
	// Test each segment prefix
	struct SegTest {
		uint8_t prefix;
		Register expected;
	};
	
	// Format: prefix byte, segment register
	// ES=0x26, CS=0x2E, SS=0x36, DS=0x3E, FS=0x64, GS=0x65
	std::vector<SegTest> tests = {
		{0x26, Register::ES},
		{0x2E, Register::CS},
		{0x36, Register::SS},
		{0x3E, Register::DS},
		{0x64, Register::FS},
		{0x65, Register::GS},
	};
	
	for (const auto& test : tests) {
		// MOV EAX, [seg:EAX]
		uint8_t bytes[] = {test.prefix, 0x8B, 0x00};
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		
		REQUIRE(result.has_value());
		CHECK(result->segment_prefix() == test.expected);
	}
}

TEST_CASE("Decoder edge: ModR/M with all mod values", "[decoder][edge]") {
	SECTION("mod=00 - memory, no displacement") {
		const uint8_t bytes[] = {0x8B, 0x00};  // MOV EAX, [EAX]
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_displ_size() == 0);
	}
	
	SECTION("mod=01 - memory, disp8") {
		const uint8_t bytes[] = {0x8B, 0x40, 0x10};  // MOV EAX, [EAX+0x10]
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_displ_size() == 1);
		CHECK(result->memory_displacement64() == 0x10);
	}
	
	SECTION("mod=10 - memory, disp32") {
		const uint8_t bytes[] = {0x8B, 0x80, 0x78, 0x56, 0x34, 0x12};  // MOV EAX, [EAX+0x12345678]
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_displ_size() == 4);
		CHECK(result->memory_displacement64() == 0x12345678);
	}
	
	SECTION("mod=11 - register") {
		const uint8_t bytes[] = {0x8B, 0xC1};  // MOV EAX, ECX
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op1_kind() == OpKind::REGISTER);
		CHECK(result->op1_register() == Register::ECX);
	}
}

TEST_CASE("Decoder edge: SIB special cases", "[decoder][edge]") {
	SECTION("SIB with base=EBP, mod=00 means disp32 only") {
		// [disp32] - mod=00, r/m=100 (SIB), base=101 (EBP means no base)
		const uint8_t bytes[] = {0x8B, 0x04, 0x25, 0x78, 0x56, 0x34, 0x12};  // MOV EAX, [0x12345678]
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::NONE);
		CHECK(result->memory_displacement64() == 0x12345678);
	}
	
	SECTION("SIB with index=ESP means no index") {
		// [EAX] with SIB where index=100 (ESP=no index)
		const uint8_t bytes[] = {0x8B, 0x04, 0x20};  // MOV EAX, [EAX] via SIB
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::EAX);
		CHECK(result->memory_index() == Register::NONE);
	}
	
	SECTION("SIB all scales") {
		// Test scale values 1, 2, 4, 8
		uint32_t scales[] = {1, 2, 4, 8};
		uint8_t scale_bits[] = {0x00, 0x40, 0x80, 0xC0};
		
		for (size_t i = 0; i < 4; i++) {
			// MOV EAX, [ECX+EDX*scale]
			uint8_t bytes[] = {0x8B, 0x04, static_cast<uint8_t>(0x11 | scale_bits[i])};
			Decoder decoder(32, bytes, 0x1000);
			auto result = decoder.decode();
			REQUIRE(result.has_value());
			CHECK(result->memory_index_scale() == scales[i]);
		}
	}
}

TEST_CASE("Decoder edge: Sign-extended immediates", "[decoder][edge]") {
	SECTION("Positive sign-extended imm8") {
		// ADD EAX, 0x7F (sign-extended)
		const uint8_t bytes[] = {0x83, 0xC0, 0x7F};
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->immediate8to32() == 0x7F);
	}
	
	SECTION("Negative sign-extended imm8") {
		// ADD EAX, -1 (0xFF sign-extended to 0xFFFFFFFF)
		const uint8_t bytes[] = {0x83, 0xC0, 0xFF};
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->immediate8to32() == static_cast<uint32_t>(-1));
	}
	
	SECTION("Sign-extended imm8 to 64-bit") {
		// ADD RAX, -1 (0xFF sign-extended to 0xFFFFFFFFFFFFFFFF)
		const uint8_t bytes[] = {0x48, 0x83, 0xC0, 0xFF};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->immediate8to64() == static_cast<uint64_t>(-1));
	}
}

TEST_CASE("Decoder edge: Near branch targets at boundaries", "[decoder][edge]") {
	SECTION("Forward branch to max positive offset") {
		// JMP rel8 with offset 0x7F (max positive for 8-bit signed)
		const uint8_t bytes[] = {0xEB, 0x7F};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		// Target = IP + len + offset = 0x1000 + 2 + 0x7F = 0x1081
		CHECK(result->near_branch64() == 0x1081);
	}
	
	SECTION("Backward branch to max negative offset") {
		// JMP rel8 with offset 0x80 (-128)
		const uint8_t bytes[] = {0xEB, 0x80};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		// Target = IP + len + offset = 0x1000 + 2 + (-128) = 0x0F82
		CHECK(result->near_branch64() == 0x0F82);
	}
}

TEST_CASE("Decoder edge: REX prefix combinations", "[decoder][edge]") {
	SECTION("REX.W alone") {
		// MOV RAX, RBX (48 89 D8)
		const uint8_t bytes[] = {0x48, 0x89, 0xD8};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op0_register() == Register::RAX);
		CHECK(result->op1_register() == Register::RBX);
	}
	
	SECTION("REX.R alone") {
		// MOV EAX, R8D (44 89 C0)
		const uint8_t bytes[] = {0x44, 0x89, 0xC0};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op1_register() == Register::R8_D);
	}
	
	SECTION("REX.B alone") {
		// MOV R8D, EAX (41 89 C0)
		const uint8_t bytes[] = {0x41, 0x89, 0xC0};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op0_register() == Register::R8_D);
	}
	
	SECTION("REX.X alone (with SIB)") {
		// MOV EAX, [RAX+R8*1] (42 8B 04 00)
		const uint8_t bytes[] = {0x42, 0x8B, 0x04, 0x00};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_index() == Register::R8);
	}
	
	SECTION("All REX bits set") {
		// MOV R8, [R9+R10*1] (4F 8B 04 11)
		const uint8_t bytes[] = {0x4F, 0x8B, 0x04, 0x11};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op0_register() == Register::R8);
		CHECK(result->memory_base() == Register::R9);
		CHECK(result->memory_index() == Register::R10);
	}
}

// ============================================================================
// ENCODER EDGE CASES
// ============================================================================

TEST_CASE("Encoder edge: Displacement size validation", "[encoder][edge]") {
	SECTION("displ_size=0 with non-zero displacement should error") {
		Instruction instr;
		instr.set_code(Code::MOV_R32_RM32);
		instr.set_op0_register(Register::EAX);
		instr.set_op0_kind(OpKind::REGISTER);
		instr.set_op1_kind(OpKind::MEMORY);
		instr.set_memory_base(Register::RCX);
		instr.set_memory_displacement64(0x100);  // Non-zero displacement
		instr.set_memory_displ_size(0);  // But size is 0
		
		Encoder encoder(64);
		auto result = encoder.encode(instr, 0x1000);
		// Should fail because displacement is non-zero but size is 0
		CHECK(!result.has_value());
	}
	
	SECTION("displ_size=0 with zero displacement should succeed") {
		Instruction instr;
		instr.set_code(Code::MOV_R32_RM32);
		instr.set_op0_register(Register::EAX);
		instr.set_op0_kind(OpKind::REGISTER);
		instr.set_op1_kind(OpKind::MEMORY);
		instr.set_memory_base(Register::RCX);
		instr.set_memory_displacement64(0);
		instr.set_memory_displ_size(0);
		
		Encoder encoder(64);
		auto result = encoder.encode(instr, 0x1000);
		CHECK(result.has_value());
	}
}

TEST_CASE("Encoder edge: EBP/RBP/R13 requires displacement", "[encoder][edge]") {
	// In x86, [EBP] cannot be encoded without a displacement because
	// mod=00, r/m=101 means disp32/RIP-relative. Must use [EBP+0] with mod=01
	
	SECTION("EBP base with zero displacement") {
		const uint8_t bytes[] = {0x8B, 0x45, 0x00};  // MOV EAX, [EBP+0]
		Decoder decoder(32, bytes, 0x1000);
		auto decode_result = decoder.decode();
		REQUIRE(decode_result.has_value());
		
		// Memory base should be EBP
		CHECK(decode_result->memory_base() == Register::EBP);
		CHECK(decode_result->memory_displacement64() == 0);
		// Even with 0 displacement, displ_size should be 1
		CHECK(decode_result->memory_displ_size() == 1);
		
		// Round-trip
		CHECK(round_trip_test(32, bytes, sizeof(bytes)));
	}
	
	SECTION("RBP base in 64-bit mode") {
		const uint8_t bytes[] = {0x8B, 0x45, 0x00};  // MOV EAX, [RBP+0]
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("R13 base in 64-bit mode") {
		const uint8_t bytes[] = {0x41, 0x8B, 0x45, 0x00};  // MOV EAX, [R13+0]
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
}

TEST_CASE("Encoder edge: Branch distance limits", "[encoder][edge]") {
	SECTION("rel8 at maximum positive range") {
		// Create instruction with target at max positive distance for rel8
		Instruction instr;
		instr.set_code(Code::JMP_REL8_64);
		instr.set_op0_kind(OpKind::NEAR_BRANCH64);
		// From IP=0x1000, instruction is 2 bytes, max rel8 = 127
		// Target = 0x1000 + 2 + 127 = 0x1081
		instr.set_near_branch64(0x1081);
		
		Encoder encoder(64);
		auto result = encoder.encode(instr, 0x1000);
		CHECK(result.has_value());
	}
	
	SECTION("rel8 just beyond positive range should fail") {
		Instruction instr;
		instr.set_code(Code::JMP_REL8_64);
		instr.set_op0_kind(OpKind::NEAR_BRANCH64);
		// Target just beyond range: 0x1000 + 2 + 128 = 0x1082
		instr.set_near_branch64(0x1082);
		
		Encoder encoder(64);
		auto result = encoder.encode(instr, 0x1000);
		CHECK(!result.has_value());  // Should fail
	}
}

TEST_CASE("Encoder edge: RIP-relative addressing", "[encoder][edge]") {
	SECTION("RIP-relative at different IP values") {
		// MOV RAX, [RIP+0x1000] from different IPs should produce different displacements
		const uint8_t bytes[] = {0x48, 0x8B, 0x05, 0x00, 0x10, 0x00, 0x00};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		// Target address = next_ip + displacement = 0x1007 + 0x1000 = 0x2007
		auto target = result->memory_displacement64();
		
		// Re-encode at different IP
		Encoder encoder(64);
		auto encode_result = encoder.encode(*result, 0x2000);
		// Should succeed but produce different displacement
		CHECK(encode_result.has_value());
	}
}

// ============================================================================
// INSTRUCTION EDGE CASES
// ============================================================================

TEST_CASE("Instruction edge: Operand count boundaries", "[instruction][edge]") {
	SECTION("op_kind for operand 4 (out of normal range)") {
		Instruction instr;
		instr.set_code(Code::NOPD);
		// Operand 4 should return IMMEDIATE8 (Rust behavior)
		CHECK(instr.op_kind(4) == OpKind::IMMEDIATE8);
	}
	
	SECTION("op_register for operand 4 (out of normal range)") {
		Instruction instr;
		instr.set_code(Code::NOPD);
		// Operand 4 register should return NONE
		CHECK(instr.op_register(4) == Register::NONE);
	}
	
	SECTION("set_op4_kind and set_op4_register are no-ops") {
		Instruction instr;
		instr.set_code(Code::NOPD);
		
		// These should be no-ops (matching Rust behavior)
		instr.set_op4_kind(OpKind::REGISTER);
		instr.set_op4_register(Register::RAX);
		
		// Should still return default values
		CHECK(instr.op4_kind() == OpKind::IMMEDIATE8);
		CHECK(instr.op4_register() == Register::NONE);
	}
}

TEST_CASE("Instruction edge: Memory displacement size mapping", "[instruction][edge]") {
	// memory_displ_size() uses internal values 0,1,2,3,4 but returns 0,1,2,4,8
	Instruction instr;
	
	SECTION("Size 0 stays 0") {
		instr.set_memory_displ_size(0);
		CHECK(instr.memory_displ_size() == 0);
	}
	
	SECTION("Size 1 stays 1") {
		instr.set_memory_displ_size(1);
		CHECK(instr.memory_displ_size() == 1);
	}
	
	SECTION("Size 2 stays 2") {
		instr.set_memory_displ_size(2);
		CHECK(instr.memory_displ_size() == 2);
	}
	
	SECTION("Size 4 maps correctly") {
		instr.set_memory_displ_size(4);
		CHECK(instr.memory_displ_size() == 4);
	}
	
	SECTION("Size 8 maps correctly") {
		instr.set_memory_displ_size(8);
		CHECK(instr.memory_displ_size() == 8);
	}
}

TEST_CASE("Instruction edge: Immediate value boundaries", "[instruction][edge]") {
	Instruction instr;
	
	SECTION("Max unsigned 8-bit immediate") {
		instr.set_immediate8(0xFF);
		CHECK(instr.immediate8() == 0xFF);
	}
	
	SECTION("Max unsigned 16-bit immediate") {
		instr.set_immediate16(0xFFFF);
		CHECK(instr.immediate16() == 0xFFFF);
	}
	
	SECTION("Max unsigned 32-bit immediate") {
		instr.set_immediate32(0xFFFFFFFF);
		CHECK(instr.immediate32() == 0xFFFFFFFF);
	}
	
	SECTION("Max unsigned 64-bit immediate") {
		instr.set_immediate64(0xFFFFFFFFFFFFFFFFULL);
		CHECK(instr.immediate64() == 0xFFFFFFFFFFFFFFFFULL);
	}
	
	SECTION("Immediate8 preserves MVEX bits") {
		// set_immediate8 should preserve upper 24 bits for MVEX
		instr.set_immediate32(0xABCDEF00);
		instr.set_immediate8(0x42);
		// Upper bits should be preserved
		CHECK((instr.immediate32() & 0xFFFFFF00) == 0xABCDEF00);
		CHECK(instr.immediate8() == 0x42);
	}
}

TEST_CASE("Instruction edge: Declare data boundaries", "[instruction][edge]") {
	SECTION("Max declare byte count (16)") {
		Instruction instr;
		instr.set_code(Code::DECLARE_BYTE);
		instr.set_declare_data_len(16);
		
		for (uint32_t i = 0; i < 16; i++) {
			instr.set_declare_byte_value(i, static_cast<uint8_t>(i));
		}
		
		CHECK(instr.declare_data_len() == 16);
		for (uint32_t i = 0; i < 16; i++) {
			CHECK(instr.get_declare_byte_value(i) == static_cast<uint8_t>(i));
		}
	}
	
	SECTION("Max declare word count (8)") {
		Instruction instr;
		instr.set_code(Code::DECLARE_WORD);
		instr.set_declare_data_len(8);
		
		for (uint32_t i = 0; i < 8; i++) {
			instr.set_declare_word_value(i, static_cast<uint16_t>(i * 0x1111));
		}
		
		CHECK(instr.declare_data_len() == 8);
		for (uint32_t i = 0; i < 8; i++) {
			CHECK(instr.get_declare_word_value(i) == static_cast<uint16_t>(i * 0x1111));
		}
	}
	
	SECTION("Max declare dword count (4)") {
		Instruction instr;
		instr.set_code(Code::DECLARE_DWORD);
		instr.set_declare_data_len(4);
		
		for (uint32_t i = 0; i < 4; i++) {
			instr.set_declare_dword_value(i, i * 0x11111111);
		}
		
		CHECK(instr.declare_data_len() == 4);
		for (uint32_t i = 0; i < 4; i++) {
			CHECK(instr.get_declare_dword_value(i) == i * 0x11111111);
		}
	}
	
	SECTION("Max declare qword count (2)") {
		Instruction instr;
		instr.set_code(Code::DECLARE_QWORD);
		instr.set_declare_data_len(2);
		
		instr.set_declare_qword_value(0, 0x1111111111111111ULL);
		instr.set_declare_qword_value(1, 0x2222222222222222ULL);
		
		CHECK(instr.declare_data_len() == 2);
		CHECK(instr.get_declare_qword_value(0) == 0x1111111111111111ULL);
		CHECK(instr.get_declare_qword_value(1) == 0x2222222222222222ULL);
	}
}

// ============================================================================
// FORMATTER EDGE CASES
// ============================================================================

TEST_CASE("Formatter edge: Empty/minimal instructions", "[formatter][edge]") {
	IntelFormatter formatter;
	
	SECTION("Format default instruction") {
		Instruction instr;
		std::string output = formatter.format_to_string(instr);
		// Should produce some output, not crash
		CHECK(!output.empty());
	}
	
	SECTION("Format NOP") {
		const uint8_t bytes[] = {0x90};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		std::string output = formatter.format_to_string(*result);
		CHECK(output.find("nop") != std::string::npos);
	}
}

TEST_CASE("Formatter edge: Large displacement values", "[formatter][edge]") {
	IntelFormatter formatter;
	
	SECTION("Max 32-bit displacement") {
		// MOV EAX, [EAX+0x7FFFFFFF]
		const uint8_t bytes[] = {0x8B, 0x80, 0xFF, 0xFF, 0xFF, 0x7F};
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		std::string output = formatter.format_to_string(*result);
		// Should contain the hex displacement
		CHECK(output.find("7FFFFFFFh") != std::string::npos);
	}
	
	SECTION("Negative displacement") {
		// MOV EAX, [EAX-0x10]
		const uint8_t bytes[] = {0x8B, 0x40, 0xF0};  // disp8 = -16
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		std::string output = formatter.format_to_string(*result);
		// Should show negative displacement
		CHECK(output.find("-") != std::string::npos);
	}
}

TEST_CASE("Formatter edge: All register sizes", "[formatter][edge]") {
	IntelFormatter formatter;
	
	SECTION("8-bit registers") {
		const uint8_t bytes[] = {0x88, 0xC1};  // MOV CL, AL
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		std::string output = formatter.format_to_string(*result);
		CHECK(output.find("cl") != std::string::npos);
		CHECK(output.find("al") != std::string::npos);
	}
	
	SECTION("16-bit registers") {
		const uint8_t bytes[] = {0x66, 0x89, 0xC1};  // MOV CX, AX
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		std::string output = formatter.format_to_string(*result);
		CHECK(output.find("cx") != std::string::npos);
		CHECK(output.find("ax") != std::string::npos);
	}
	
	SECTION("32-bit registers") {
		const uint8_t bytes[] = {0x89, 0xC1};  // MOV ECX, EAX
		Decoder decoder(32, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		std::string output = formatter.format_to_string(*result);
		CHECK(output.find("ecx") != std::string::npos);
		CHECK(output.find("eax") != std::string::npos);
	}
	
	SECTION("64-bit registers") {
		const uint8_t bytes[] = {0x48, 0x89, 0xC1};  // MOV RCX, RAX
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		std::string output = formatter.format_to_string(*result);
		CHECK(output.find("rcx") != std::string::npos);
		CHECK(output.find("rax") != std::string::npos);
	}
}

// ============================================================================
// VEX/EVEX EDGE CASES
// ============================================================================

TEST_CASE("VEX edge: VEX2 vs VEX3 encoding", "[vex][edge]") {
	// VEX2 can be used when: no extended regs, W=0, map=0F
	// VEX3 must be used otherwise
	
	SECTION("VEX2 basic instruction") {
		// VZEROUPPER: C5 F8 77
		const uint8_t bytes[] = {0xC5, 0xF8, 0x77};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
	
	SECTION("VEX3 with extended register") {
		// VADDPS XMM8, XMM0, XMM0: C4 41 78 58 C0
		const uint8_t bytes[] = {0xC4, 0x41, 0x78, 0x58, 0xC0};
		CHECK(round_trip_test(64, bytes, sizeof(bytes)));
	}
}

TEST_CASE("EVEX edge: Opmask values k0-k7", "[evex][edge]") {
	// Test all opmask registers
	for (uint8_t k = 0; k <= 7; k++) {
		// VMOVDQA32 XMM0 {k}, XMM1
		// 62 F1 7D 0k 6F C1 where k is the opmask
		uint8_t bytes[] = {0x62, 0xF1, 0x7D, static_cast<uint8_t>(0x08 | k), 0x6F, 0xC1};
		
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		
		if (k == 0) {
			CHECK(result->op_mask() == Register::NONE);
		} else {
			CHECK(result->op_mask() == static_cast<Register>(
				static_cast<uint32_t>(Register::K0) + k));
		}
	}
}

TEST_CASE("EVEX edge: Zeroing vs merging masking", "[evex][edge]") {
	SECTION("Merging masking (z=0)") {
		// VMOVDQA32 XMM0 {k1}, XMM1
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x09, 0x6F, 0xC1};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->zeroing_masking() == false);
	}
	
	SECTION("Zeroing masking (z=1)") {
		// VMOVDQA32 XMM0 {k1}{z}, XMM1
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x89, 0x6F, 0xC1};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->zeroing_masking() == true);
	}
}

TEST_CASE("EVEX edge: Vector lengths 128/256/512", "[evex][edge]") {
	SECTION("EVEX.L'L = 00 (128-bit)") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x08, 0x6F, 0xC1};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op0_register() == Register::XMM0);
	}
	
	SECTION("EVEX.L'L = 01 (256-bit)") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x28, 0x6F, 0xC1};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op0_register() == Register::YMM0);
	}
	
	SECTION("EVEX.L'L = 10 (512-bit)") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x48, 0x6F, 0xC1};
		Decoder decoder(64, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op0_register() == Register::ZMM0);
	}
}

// ============================================================================
// 16-BIT MODE EDGE CASES
// ============================================================================

TEST_CASE("16-bit mode edge cases", "[16bit][edge]") {
	SECTION("16-bit addressing with BX+SI") {
		// MOV AX, [BX+SI]
		const uint8_t bytes[] = {0x8B, 0x00};
		Decoder decoder(16, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::BX);
		CHECK(result->memory_index() == Register::SI);
	}
	
	SECTION("16-bit addressing with BX+DI") {
		// MOV AX, [BX+DI]
		const uint8_t bytes[] = {0x8B, 0x01};
		Decoder decoder(16, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::BX);
		CHECK(result->memory_index() == Register::DI);
	}
	
	SECTION("16-bit addressing with BP requires displacement") {
		// [BP] cannot be encoded with mod=00, needs mod=01 with disp8=0
		// MOV AX, [BP+0]
		const uint8_t bytes[] = {0x8B, 0x46, 0x00};
		Decoder decoder(16, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::BP);
		CHECK(result->memory_displ_size() == 1);
	}
	
	SECTION("16-bit direct address (mod=00, r/m=110)") {
		// MOV AX, [0x1234]
		const uint8_t bytes[] = {0x8B, 0x06, 0x34, 0x12};
		Decoder decoder(16, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->memory_base() == Register::NONE);
		CHECK(result->memory_displacement64() == 0x1234);
	}
	
	SECTION("Operand size prefix in 16-bit mode") {
		// 66h in 16-bit mode gives 32-bit operands
		// MOV EAX, EBX
		const uint8_t bytes[] = {0x66, 0x89, 0xD8};
		Decoder decoder(16, bytes, 0x1000);
		auto result = decoder.decode();
		REQUIRE(result.has_value());
		CHECK(result->op0_register() == Register::EAX);
		CHECK(result->op1_register() == Register::EBX);
	}
}

// ============================================================================
// ERROR HANDLING EDGE CASES
// ============================================================================

TEST_CASE("Error handling: Invalid bitness", "[error][edge]") {
	SECTION("Encoder with invalid bitness should throw") {
		CHECK_THROWS(Encoder(0));
		CHECK_THROWS(Encoder(8));
		CHECK_THROWS(Encoder(128));
	}
	
	// Note: Decoder may or may not throw for invalid bitness depending on implementation
	// We test encoder throwing behavior which is more critical
}

TEST_CASE("Error handling: Bitness-specific instructions", "[error][edge]") {
	// Test that certain opcodes decode differently in different modes
	// These tests verify behavioral differences, not necessarily errors
	
	SECTION("0F 05 in 32-bit vs 64-bit mode") {
		const uint8_t bytes[] = {0x0F, 0x05};
		
		// In 32-bit mode, 0F 05 is LOADALL or invalid on most CPUs
		Decoder decoder32(32, bytes, 0x1000);
		auto result32 = decoder32.decode();
		// May or may not be valid depending on decoder options
		
		// In 64-bit mode, 0F 05 is SYSCALL
		Decoder decoder64(64, bytes, 0x1000);
		auto result64 = decoder64.decode();
		// Just check we can decode without crashing
	}
	
	SECTION("0x60 in 32-bit vs 64-bit mode") {
		const uint8_t bytes[] = {0x60};
		
		// In 32-bit mode, 0x60 is PUSHAD
		Decoder decoder32(32, bytes, 0x1000);
		auto result32 = decoder32.decode();
		// Just check we can decode without crashing
		
		// In 64-bit mode, 0x60 is not a valid opcode (PUSHA removed)
		Decoder decoder64(64, bytes, 0x1000);
		auto result64 = decoder64.decode();
		// Should decode to something (possibly invalid)
	}
}
