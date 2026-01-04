// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

// ============================================================================
// BlockEncoder tests
// ============================================================================

TEST_CASE( "BlockEncoder: encode single instruction", "[block_encoder][manual]" ) {
	// NOP
	auto nop = InstructionFactory::with( Code::NOPD );
	std::vector<Instruction> instructions = { nop };
	
	auto result = BlockEncoder::encode( 64, instructions, 0x1000 );
	REQUIRE( result.has_value() );
	
	CHECK( result->rip == 0x1000 );
	CHECK( !result->code_buffer.empty() );
	CHECK( result->code_buffer[0] == 0x90 );  // NOP
}

TEST_CASE( "BlockEncoder: encode multiple instructions", "[block_encoder][manual]" ) {
	std::vector<Instruction> instructions;
	
	// MOV EAX, EBX
	instructions.push_back( InstructionFactory::with2( Code::MOV_R32_RM32, Register::EAX, Register::EBX ) );
	// NOP
	instructions.push_back( InstructionFactory::with( Code::NOPD ) );
	// RET
	instructions.push_back( InstructionFactory::with( Code::RETNQ ) );
	
	auto result = BlockEncoder::encode( 64, instructions, 0x1000 );
	REQUIRE( result.has_value() );
	
	CHECK( result->rip == 0x1000 );
	CHECK( result->code_buffer.size() >= 4 );  // At least 2+1+1 bytes
}

TEST_CASE( "BlockEncoder: with instruction offsets", "[block_encoder][manual]" ) {
	std::vector<Instruction> instructions;
	instructions.push_back( InstructionFactory::with( Code::NOPD ) );  // 1 byte
	instructions.push_back( InstructionFactory::with( Code::NOPD ) );  // 1 byte
	instructions.push_back( InstructionFactory::with( Code::RETNQ ) ); // 1 byte
	
	auto result = BlockEncoder::encode( 
		64, 
		instructions, 
		0x1000, 
		BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS 
	);
	
	REQUIRE( result.has_value() );
	REQUIRE( result->new_instruction_offsets.size() == 3 );
	CHECK( result->new_instruction_offsets[0] == 0 );
	CHECK( result->new_instruction_offsets[1] == 1 );
	CHECK( result->new_instruction_offsets[2] == 2 );
}

TEST_CASE( "BlockEncoder: with constant offsets", "[block_encoder][manual]" ) {
	std::vector<Instruction> instructions;
	instructions.push_back( InstructionFactory::with( Code::NOPD ) );
	
	auto result = BlockEncoder::encode( 
		64, 
		instructions, 
		0x1000, 
		BlockEncoderOptions::RETURN_CONSTANT_OFFSETS 
	);
	
	REQUIRE( result.has_value() );
	CHECK( result->constant_offsets.size() == 1 );
}

TEST_CASE( "BlockEncoder: InstructionBlock API", "[block_encoder][manual]" ) {
	std::vector<Instruction> instructions;
	instructions.push_back( InstructionFactory::with( Code::NOPD ) );
	instructions.push_back( InstructionFactory::with( Code::RETNQ ) );
	
	InstructionBlock block( instructions, 0x2000 );
	
	auto result = BlockEncoder::encode( 64, block );
	REQUIRE( result.has_value() );
	CHECK( result->rip == 0x2000 );
}

TEST_CASE( "BlockEncoder: multiple blocks", "[block_encoder][manual]" ) {
	std::vector<Instruction> block1_instrs;
	block1_instrs.push_back( InstructionFactory::with( Code::NOPD ) );
	
	std::vector<Instruction> block2_instrs;
	block2_instrs.push_back( InstructionFactory::with( Code::RETNQ ) );
	
	std::vector<InstructionBlock> blocks;
	blocks.emplace_back( block1_instrs, 0x1000 );
	blocks.emplace_back( block2_instrs, 0x2000 );
	
	auto result = BlockEncoder::encode( 64, blocks );
	REQUIRE( result.has_value() );
	REQUIRE( result->size() == 2 );
	CHECK( (*result)[0].rip == 0x1000 );
	CHECK( (*result)[1].rip == 0x2000 );
}

TEST_CASE( "BlockEncoder: 32-bit mode", "[block_encoder][manual]" ) {
	std::vector<Instruction> instructions;
	instructions.push_back( InstructionFactory::with2( Code::MOV_R32_RM32, Register::EAX, Register::EBX ) );
	
	auto result = BlockEncoder::encode( 32, instructions, 0x1000 );
	REQUIRE( result.has_value() );
	CHECK( !result->code_buffer.empty() );
}

TEST_CASE( "BlockEncoder: 16-bit mode", "[block_encoder][manual]" ) {
	std::vector<Instruction> instructions;
	instructions.push_back( InstructionFactory::with2( Code::MOV_R16_RM16, Register::AX, Register::BX ) );
	
	auto result = BlockEncoder::encode( 16, instructions, 0x1000 );
	REQUIRE( result.has_value() );
	CHECK( !result->code_buffer.empty() );
}

TEST_CASE( "BlockEncoder: empty instruction list", "[block_encoder][manual]" ) {
	std::vector<Instruction> instructions;
	
	auto result = BlockEncoder::encode( 64, instructions, 0x1000 );
	REQUIRE( result.has_value() );
	CHECK( result->code_buffer.empty() );
}

TEST_CASE( "BlockEncoder: round-trip encode-decode", "[block_encoder][manual]" ) {
	// Create some instructions
	std::vector<Instruction> original;
	original.push_back( InstructionFactory::with2( Code::MOV_R64_RM64, Register::RAX, Register::RBX ) );
	original.push_back( InstructionFactory::with2( Code::ADD_R64_RM64, Register::RAX, Register::RCX ) );
	original.push_back( InstructionFactory::with( Code::RETNQ ) );
	
	// Encode
	auto result = BlockEncoder::encode( 64, original, 0x1000 );
	REQUIRE( result.has_value() );
	
	// Decode
	Decoder decoder( 64, result->code_buffer, 0x1000 );
	std::vector<Mnemonic> decoded_mnemonics;
	for ( const auto& instr : decoder ) {
		decoded_mnemonics.push_back( instr.mnemonic() );
	}
	
	REQUIRE( decoded_mnemonics.size() == 3 );
	CHECK( decoded_mnemonics[0] == Mnemonic::MOV );
	CHECK( decoded_mnemonics[1] == Mnemonic::ADD );
	CHECK( decoded_mnemonics[2] == Mnemonic::RET );
}
