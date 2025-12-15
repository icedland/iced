// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

TEST_CASE("Encoder: basic construction", "[encoder]") {
	Encoder encoder(64);
	CHECK(encoder.bitness() == 64);
	CHECK(encoder.position() == 0);
}

TEST_CASE("Encoder: 32-bit mode", "[encoder]") {
	Encoder encoder(32);
	CHECK(encoder.bitness() == 32);
}

TEST_CASE("Encoder: 16-bit mode", "[encoder]") {
	Encoder encoder(16);
	CHECK(encoder.bitness() == 16);
}

TEST_CASE("Encoder: encode NOP", "[encoder]") {
	// Decode a NOP instruction first
	const uint8_t nop_bytes[] = {0x90};
	Decoder decoder(64, nop_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::NOPD);
	
	// Now encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 1);  // NOP is 1 byte
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 1);
	CHECK(buffer[0] == 0x90);
}

TEST_CASE("Encoder: encode MOV reg, imm32", "[encoder]") {
	// MOV EAX, 0x12345678 -> B8 78 56 34 12
	const uint8_t mov_bytes[] = {0xB8, 0x78, 0x56, 0x34, 0x12};
	Decoder decoder(64, mov_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::MOV_R32_IMM32);
	REQUIRE(instr.op0_register() == Register::EAX);
	REQUIRE(instr.immediate32() == 0x12345678);
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 5);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 5);
	CHECK(buffer[0] == 0xB8);
	CHECK(buffer[1] == 0x78);
	CHECK(buffer[2] == 0x56);
	CHECK(buffer[3] == 0x34);
	CHECK(buffer[4] == 0x12);
}

TEST_CASE("Encoder: encode ADD reg, reg", "[encoder]") {
	// ADD ECX, EAX -> 01 C1
	const uint8_t add_bytes[] = {0x01, 0xC1};
	Decoder decoder(64, add_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::ADD_RM32_R32);
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 2);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 2);
	CHECK(buffer[0] == 0x01);
	CHECK(buffer[1] == 0xC1);
}

TEST_CASE("Encoder: encode with REX.R prefix", "[encoder]") {
	// ADD EAX, R8D -> 44 01 C0
	// 44 = REX.R (extends reg field to R8-R15)
	// 01 = ADD Ev, Gv opcode
	// C0 = ModR/M: mod=3 (reg), reg=0 (EAX with REX.R -> R8D), rm=0 (EAX)
	// So this is ADD EAX, R8D (op0=EAX, op1=R8D)
	const uint8_t add_bytes[] = {0x44, 0x01, 0xC0};
	Decoder decoder(64, add_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::ADD_RM32_R32);
	REQUIRE(instr.op0_register() == Register::EAX);   // Destination (r/m field)
	REQUIRE(instr.op1_register() == Register::R8_D);  // Source (reg field + REX.R)
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 3);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 3);
	CHECK(buffer[0] == 0x44);  // REX.R
	CHECK(buffer[1] == 0x01);
	CHECK(buffer[2] == 0xC0);
}

TEST_CASE("Encoder: encode with REX.B prefix", "[encoder]") {
	// ADD R8D, EAX -> 41 01 C0
	// 41 = REX.B (extends r/m field to R8-R15)
	// 01 = ADD Ev, Gv opcode
	// C0 = ModR/M: mod=3 (reg), reg=0 (EAX), rm=0 (with REX.B -> R8D)
	// So this is ADD R8D, EAX (op0=R8D, op1=EAX)
	const uint8_t add_bytes[] = {0x41, 0x01, 0xC0};
	Decoder decoder(64, add_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::ADD_RM32_R32);
	REQUIRE(instr.op0_register() == Register::R8_D);  // Destination (r/m field + REX.B)
	REQUIRE(instr.op1_register() == Register::EAX);   // Source (reg field)
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 3);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 3);
	CHECK(buffer[0] == 0x41);  // REX.B
	CHECK(buffer[1] == 0x01);
	CHECK(buffer[2] == 0xC0);
}

TEST_CASE("Encoder: encode memory operand", "[encoder]") {
	// MOV EAX, [RCX] -> 8B 01
	const uint8_t mov_bytes[] = {0x8B, 0x01};
	Decoder decoder(64, mov_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::MOV_R32_RM32);
	REQUIRE(instr.op0_register() == Register::EAX);
	REQUIRE(instr.op1_kind() == OpKind::MEMORY);
	REQUIRE(instr.memory_base() == Register::RCX);
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 2);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 2);
	CHECK(buffer[0] == 0x8B);
	CHECK(buffer[1] == 0x01);
}

TEST_CASE("Encoder: encode memory with displacement", "[encoder]") {
	// MOV EAX, [RCX+0x10] -> 8B 41 10
	const uint8_t mov_bytes[] = {0x8B, 0x41, 0x10};
	Decoder decoder(64, mov_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::MOV_R32_RM32);
	REQUIRE(instr.memory_displacement64() == 0x10);
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 3);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 3);
	CHECK(buffer[0] == 0x8B);
	CHECK(buffer[1] == 0x41);
	CHECK(buffer[2] == 0x10);
}

TEST_CASE("Encoder: encode SIB", "[encoder]") {
	// MOV EAX, [RCX+RDX*4] -> 8B 04 91
	const uint8_t mov_bytes[] = {0x8B, 0x04, 0x91};
	Decoder decoder(64, mov_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::MOV_R32_RM32);
	REQUIRE(instr.memory_base() == Register::RCX);
	REQUIRE(instr.memory_index() == Register::RDX);
	REQUIRE(instr.memory_index_scale() == 4);
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 3);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 3);
	CHECK(buffer[0] == 0x8B);
	CHECK(buffer[1] == 0x04);
	CHECK(buffer[2] == 0x91);
}

TEST_CASE("Encoder: encode 64-bit immediate", "[encoder]") {
	// MOV RAX, 0x123456789ABCDEF0 -> 48 B8 F0 DE BC 9A 78 56 34 12
	const uint8_t mov_bytes[] = {0x48, 0xB8, 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12};
	Decoder decoder(64, mov_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::MOV_R64_IMM64);
	REQUIRE(instr.op0_register() == Register::RAX);
	REQUIRE(instr.immediate64() == 0x123456789ABCDEF0ULL);
	
	// Encode it back
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 10);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 10);
	for (size_t i = 0; i < 10; ++i) {
		CHECK(buffer[i] == mov_bytes[i]);
	}
}

TEST_CASE("Encoder: encode relative branch", "[encoder]") {
	// JMP rel8 (EB 10) - jump forward 0x10 bytes
	const uint8_t jmp_bytes[] = {0xEB, 0x10};
	Decoder decoder(64, jmp_bytes, 0x1000);
	auto decode_result = decoder.decode();
	
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	REQUIRE(instr.code() == Code::JMP_REL8_64);
	// Target = IP + len + rel8 = 0x1000 + 2 + 0x10 = 0x1012
	REQUIRE(instr.near_branch64() == 0x1012);
	
	// Encode it back at same address
	Encoder encoder(64);
	auto result = encoder.encode(instr, 0x1000);
	
	REQUIRE(result.has_value());
	CHECK(*result == 2);
	
	auto buffer = encoder.take_buffer();
	REQUIRE(buffer.size() == 2);
	CHECK(buffer[0] == 0xEB);
	CHECK(buffer[1] == 0x10);
}

TEST_CASE("Encoder: buffer management", "[encoder]") {
	Encoder encoder(64);
	
	// Encode something
	const uint8_t nop_bytes[] = {0x90};
	Decoder decoder(64, nop_bytes, 0x1000);
	auto decode_result = decoder.decode();
	REQUIRE(decode_result.has_value());
	auto instr = *decode_result;
	
	auto result = encoder.encode(instr, 0x1000);
	REQUIRE(result.has_value());
	
	// Take the buffer
	auto buffer = encoder.take_buffer();
	CHECK(buffer.size() == 1);
	
	// Buffer should now be empty
	CHECK(encoder.position() == 0);
	
	// Set a new buffer
	std::vector<uint8_t> new_buffer = {0x01, 0x02, 0x03};
	encoder.set_buffer(std::move(new_buffer));
	CHECK(encoder.position() == 3);
}

TEST_CASE("Encoder: multiple instructions", "[encoder]") {
	Encoder encoder(64);
	
	// Encode NOP
	{
		const uint8_t nop_bytes[] = {0x90};
		Decoder decoder(64, nop_bytes, 0x1000);
		auto decode_result = decoder.decode();
		REQUIRE(decode_result.has_value());
		auto result = encoder.encode(*decode_result, 0x1000);
		REQUIRE(result.has_value());
	}
	
	// Encode another NOP
	{
		const uint8_t nop_bytes[] = {0x90};
		Decoder decoder(64, nop_bytes, 0x1001);
		auto decode_result = decoder.decode();
		REQUIRE(decode_result.has_value());
		auto result = encoder.encode(*decode_result, 0x1001);
		REQUIRE(result.has_value());
	}
	
	auto buffer = encoder.take_buffer();
	CHECK(buffer.size() == 2);
	CHECK(buffer[0] == 0x90);
	CHECK(buffer[1] == 0x90);
}
