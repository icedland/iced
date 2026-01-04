// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Tests for InstructionFactory, MemoryOperand, and related functionality

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

// ============================================================================
// InstructionFactory::with3() tests
// ============================================================================

TEST_CASE( "InstructionFactory: with3 register, register, register", "[instruction_create][manual]" ) {
	// IMUL EAX, EBX, ECX - not a real instruction, use VADDPS
	// VADDPS XMM0, XMM1, XMM2
	auto instr = InstructionFactory::with3( Code::VEX_VADDPS_XMM_XMM_XMMM128, 
	                                        Register::XMM0, Register::XMM1, Register::XMM2 );
	
	CHECK( instr.code() == Code::VEX_VADDPS_XMM_XMM_XMMM128 );
	CHECK( instr.op_count() == 3 );
	CHECK( instr.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 1 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 2 ) == OpKind::REGISTER );
	CHECK( instr.op_register( 0 ) == Register::XMM0 );
	CHECK( instr.op_register( 1 ) == Register::XMM1 );
	CHECK( instr.op_register( 2 ) == Register::XMM2 );
}

TEST_CASE( "InstructionFactory: with3 register, register, immediate", "[instruction_create][manual]" ) {
	// IMUL EAX, EBX, 0x10
	auto instr = InstructionFactory::with3( Code::IMUL_R32_RM32_IMM8, 
	                                        Register::EAX, Register::EBX, 0x10 );
	
	CHECK( instr.code() == Code::IMUL_R32_RM32_IMM8 );
	CHECK( instr.op_count() == 3 );
	CHECK( instr.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 1 ) == OpKind::REGISTER );
	CHECK( instr.op_register( 0 ) == Register::EAX );
	CHECK( instr.op_register( 1 ) == Register::EBX );
	CHECK( instr.immediate8() == 0x10 );
}

TEST_CASE( "InstructionFactory: with3 register, register, memory", "[instruction_create][manual]" ) {
	// VADDPS XMM0, XMM1, [RAX]
	auto mem = MemoryOperand::with_base( Register::RAX );
	auto instr = InstructionFactory::with3( Code::VEX_VADDPS_XMM_XMM_XMMM128, 
	                                        Register::XMM0, Register::XMM1, mem );
	
	CHECK( instr.code() == Code::VEX_VADDPS_XMM_XMM_XMMM128 );
	CHECK( instr.op_count() == 3 );
	CHECK( instr.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 1 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 2 ) == OpKind::MEMORY );
	CHECK( instr.op_register( 0 ) == Register::XMM0 );
	CHECK( instr.op_register( 1 ) == Register::XMM1 );
	CHECK( instr.memory_base() == Register::RAX );
}

TEST_CASE( "InstructionFactory: with3 register, memory, register", "[instruction_create][manual]" ) {
	// Some instructions have reg, mem, reg format
	auto mem = MemoryOperand::with_base( Register::RBX );
	auto instr = InstructionFactory::with3( Code::VEX_VADDPS_XMM_XMM_XMMM128, 
	                                        Register::XMM0, mem, Register::XMM2 );
	
	CHECK( instr.code() == Code::VEX_VADDPS_XMM_XMM_XMMM128 );
	CHECK( instr.op_count() == 3 );
}

TEST_CASE( "InstructionFactory: with3 memory, register, register", "[instruction_create][manual]" ) {
	// Some instructions have mem, reg, reg format
	auto mem = MemoryOperand::with_base( Register::RAX );
	auto instr = InstructionFactory::with3( Code::VEX_VADDPS_XMM_XMM_XMMM128, 
	                                        mem, Register::XMM1, Register::XMM2 );
	
	CHECK( instr.code() == Code::VEX_VADDPS_XMM_XMM_XMMM128 );
	CHECK( instr.op_count() == 3 );
}

// ============================================================================
// InstructionFactory::with4() tests
// ============================================================================

TEST_CASE( "InstructionFactory: with4 register, register, register, register", "[instruction_create][manual]" ) {
	// VPINSRB XMM0, XMM1, EAX, 0 -> use VBLENDVPS instead
	// VBLENDVPS XMM0, XMM1, XMM2, XMM3
	auto instr = InstructionFactory::with4( Code::VEX_VBLENDVPS_XMM_XMM_XMMM128_XMM, 
	                                        Register::XMM0, Register::XMM1, Register::XMM2, Register::XMM3 );
	
	CHECK( instr.code() == Code::VEX_VBLENDVPS_XMM_XMM_XMMM128_XMM );
	CHECK( instr.op_count() == 4 );
	CHECK( instr.op_kind( 0 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 1 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 2 ) == OpKind::REGISTER );
	CHECK( instr.op_kind( 3 ) == OpKind::REGISTER );
	CHECK( instr.op_register( 0 ) == Register::XMM0 );
	CHECK( instr.op_register( 1 ) == Register::XMM1 );
	CHECK( instr.op_register( 2 ) == Register::XMM2 );
	CHECK( instr.op_register( 3 ) == Register::XMM3 );
}

TEST_CASE( "InstructionFactory: with4 register, register, register, immediate", "[instruction_create][manual]" ) {
	// VPINSRB XMM0, XMM1, EAX, 5
	auto instr = InstructionFactory::with4( Code::VEX_VPINSRB_XMM_XMM_R32M8_IMM8, 
	                                        Register::XMM0, Register::XMM1, Register::EAX, 5 );
	
	CHECK( instr.code() == Code::VEX_VPINSRB_XMM_XMM_R32M8_IMM8 );
	CHECK( instr.op_count() == 4 );
	CHECK( instr.op_register( 0 ) == Register::XMM0 );
	CHECK( instr.op_register( 1 ) == Register::XMM1 );
	CHECK( instr.op_register( 2 ) == Register::EAX );
	CHECK( instr.immediate8() == 5 );
}

TEST_CASE( "InstructionFactory: with4 register, register, register, memory", "[instruction_create][manual]" ) {
	// VBLENDVPS XMM0, XMM1, [RAX], XMM3
	auto mem = MemoryOperand::with_base( Register::RAX );
	auto instr = InstructionFactory::with4( Code::VEX_VBLENDVPS_XMM_XMM_XMMM128_XMM, 
	                                        Register::XMM0, Register::XMM1, mem, Register::XMM3 );
	
	CHECK( instr.code() == Code::VEX_VBLENDVPS_XMM_XMM_XMMM128_XMM );
	CHECK( instr.op_count() == 4 );
	CHECK( instr.op_kind( 2 ) == OpKind::MEMORY );
	CHECK( instr.memory_base() == Register::RAX );
}

TEST_CASE( "InstructionFactory: with4 register, register, memory, immediate", "[instruction_create][manual]" ) {
	// VPINSRB XMM0, XMM1, [RAX], 5
	auto mem = MemoryOperand::with_base( Register::RAX );
	auto instr = InstructionFactory::with4( Code::VEX_VPINSRB_XMM_XMM_R32M8_IMM8, 
	                                        Register::XMM0, Register::XMM1, mem, 5 );
	
	CHECK( instr.code() == Code::VEX_VPINSRB_XMM_XMM_R32M8_IMM8 );
	CHECK( instr.op_count() == 4 );
	CHECK( instr.op_kind( 2 ) == OpKind::MEMORY );
	CHECK( instr.immediate8() == 5 );
}

// ============================================================================
// InstructionFactory::with5() tests
// ============================================================================

TEST_CASE( "InstructionFactory: with5 register, register, register, register, immediate", "[instruction_create][manual]" ) {
	// VPERMIL2PS XMM0, XMM1, XMM2, XMM3, 0 (VEX.128.66.0F3A.W0 48 /r is4)
	// This is an XOP instruction, let's use a simpler 5-operand pattern if available
	auto instr = InstructionFactory::with5( Code::VEX_VPERMIL2PS_XMM_XMM_XMMM128_XMM_IMM4, 
	                                        Register::XMM0, Register::XMM1, Register::XMM2, Register::XMM3, 0 );
	
	CHECK( instr.op_count() == 5 );
	CHECK( instr.op_register( 0 ) == Register::XMM0 );
	CHECK( instr.op_register( 1 ) == Register::XMM1 );
	CHECK( instr.op_register( 2 ) == Register::XMM2 );
	CHECK( instr.op_register( 3 ) == Register::XMM3 );
}

// ============================================================================
// String instruction factory tests
// ============================================================================

TEST_CASE( "InstructionFactory: with_movsb", "[instruction_create][string][manual]" ) {
	// MOVSB with 64-bit address size
	auto instr = InstructionFactory::with_movsb( 64, Register::NONE, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::MOVSB );
	CHECK( instr.has_rep_prefix() == false );
	CHECK( instr.has_repne_prefix() == false );
}

TEST_CASE( "InstructionFactory: with_rep_movsb", "[instruction_create][string][manual]" ) {
	// REP MOVSB with 64-bit address size
	auto instr = InstructionFactory::with_rep_movsb( 64 );
	
	CHECK( instr.mnemonic() == Mnemonic::MOVSB );
	CHECK( instr.has_rep_prefix() == true );
}

TEST_CASE( "InstructionFactory: with_movsd", "[instruction_create][string][manual]" ) {
	// MOVSD with 32-bit address size
	auto instr = InstructionFactory::with_movsd( 32, Register::NONE, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::MOVSD );
}

TEST_CASE( "InstructionFactory: with_rep_movsd", "[instruction_create][string][manual]" ) {
	// REP MOVSD with 32-bit address size
	auto instr = InstructionFactory::with_rep_movsd( 32 );
	
	CHECK( instr.mnemonic() == Mnemonic::MOVSD );
	CHECK( instr.has_rep_prefix() == true );
}

TEST_CASE( "InstructionFactory: with_movsq", "[instruction_create][string][manual]" ) {
	// MOVSQ with 64-bit address size
	auto instr = InstructionFactory::with_movsq( 64, Register::NONE, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::MOVSQ );
}

TEST_CASE( "InstructionFactory: with_stosb", "[instruction_create][string][manual]" ) {
	// STOSB with 64-bit address size
	auto instr = InstructionFactory::with_stosb( 64, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::STOSB );
}

TEST_CASE( "InstructionFactory: with_rep_stosb", "[instruction_create][string][manual]" ) {
	// REP STOSB with 64-bit address size
	auto instr = InstructionFactory::with_rep_stosb( 64 );
	
	CHECK( instr.mnemonic() == Mnemonic::STOSB );
	CHECK( instr.has_rep_prefix() == true );
}

TEST_CASE( "InstructionFactory: with_stosd", "[instruction_create][string][manual]" ) {
	// STOSD with 32-bit address size
	auto instr = InstructionFactory::with_stosd( 32, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::STOSD );
}

TEST_CASE( "InstructionFactory: with_cmpsb", "[instruction_create][string][manual]" ) {
	// CMPSB with 64-bit address size
	auto instr = InstructionFactory::with_cmpsb( 64, Register::NONE, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::CMPSB );
}

TEST_CASE( "InstructionFactory: with_repe_cmpsb", "[instruction_create][string][manual]" ) {
	// REPE CMPSB
	auto instr = InstructionFactory::with_repe_cmpsb( 64 );
	
	CHECK( instr.mnemonic() == Mnemonic::CMPSB );
	CHECK( instr.has_rep_prefix() == true );  // REPE uses rep prefix
}

TEST_CASE( "InstructionFactory: with_repne_cmpsb", "[instruction_create][string][manual]" ) {
	// REPNE CMPSB
	auto instr = InstructionFactory::with_repne_cmpsb( 64 );
	
	CHECK( instr.mnemonic() == Mnemonic::CMPSB );
	CHECK( instr.has_repne_prefix() == true );
}

TEST_CASE( "InstructionFactory: with_scasb", "[instruction_create][string][manual]" ) {
	// SCASB with 64-bit address size
	auto instr = InstructionFactory::with_scasb( 64, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::SCASB );
}

TEST_CASE( "InstructionFactory: with_repe_scasb", "[instruction_create][string][manual]" ) {
	// REPE SCASB
	auto instr = InstructionFactory::with_repe_scasb( 64 );
	
	CHECK( instr.mnemonic() == Mnemonic::SCASB );
	CHECK( instr.has_rep_prefix() == true );
}

TEST_CASE( "InstructionFactory: with_lodsb", "[instruction_create][string][manual]" ) {
	// LODSB with 64-bit address size
	auto instr = InstructionFactory::with_lodsb( 64, Register::NONE, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::LODSB );
}

TEST_CASE( "InstructionFactory: with_rep_lodsb", "[instruction_create][string][manual]" ) {
	// REP LODSB
	auto instr = InstructionFactory::with_rep_lodsb( 64 );
	
	CHECK( instr.mnemonic() == Mnemonic::LODSB );
	CHECK( instr.has_rep_prefix() == true );
}

TEST_CASE( "InstructionFactory: with_outsb", "[instruction_create][string][manual]" ) {
	// OUTSB with 32-bit address size
	auto instr = InstructionFactory::with_outsb( 32, Register::NONE, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::OUTSB );
}

TEST_CASE( "InstructionFactory: with_rep_outsb", "[instruction_create][string][manual]" ) {
	// REP OUTSB
	auto instr = InstructionFactory::with_rep_outsb( 32 );
	
	CHECK( instr.mnemonic() == Mnemonic::OUTSB );
	CHECK( instr.has_rep_prefix() == true );
}

TEST_CASE( "InstructionFactory: with_insb", "[instruction_create][string][manual]" ) {
	// INSB with 32-bit address size
	auto instr = InstructionFactory::with_insb( 32, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::INSB );
}

TEST_CASE( "InstructionFactory: with_rep_insb", "[instruction_create][string][manual]" ) {
	// REP INSB
	auto instr = InstructionFactory::with_rep_insb( 32 );
	
	CHECK( instr.mnemonic() == Mnemonic::INSB );
	CHECK( instr.has_rep_prefix() == true );
}

// ============================================================================
// String instructions with segment overrides
// ============================================================================

TEST_CASE( "InstructionFactory: string instruction with segment override", "[instruction_create][string][manual]" ) {
	// MOVSB with FS segment override
	auto instr = InstructionFactory::with_movsb( 64, Register::FS, RepPrefixKind::NONE );
	
	CHECK( instr.mnemonic() == Mnemonic::MOVSB );
	CHECK( instr.memory_segment() == Register::FS );
}

// ============================================================================
// XBEGIN instruction
// ============================================================================

TEST_CASE( "InstructionFactory: with_xbegin 32-bit", "[instruction_create][manual]" ) {
	auto instr = InstructionFactory::with_xbegin( 32, 0x12345678 );
	
	CHECK( instr.mnemonic() == Mnemonic::XBEGIN );
}

TEST_CASE( "InstructionFactory: with_xbegin 64-bit", "[instruction_create][manual]" ) {
	auto instr = InstructionFactory::with_xbegin( 64, 0x123456789ABCDEF0ULL );
	
	CHECK( instr.mnemonic() == Mnemonic::XBEGIN );
}

// ============================================================================
// MemoryOperand factory tests
// ============================================================================

TEST_CASE( "MemoryOperand: with_base", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_base( Register::RAX );
	
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::NONE );
	CHECK( mem.scale == 1 );
	CHECK( mem.displacement == 0 );
}

TEST_CASE( "MemoryOperand: with_displ", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_displ( 0x12345678, 4 );
	
	CHECK( mem.base == Register::NONE );
	CHECK( mem.index == Register::NONE );
	CHECK( mem.displacement == 0x12345678 );
	CHECK( mem.displ_size == 4 );
}

TEST_CASE( "MemoryOperand: with_base_displ", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_base_displ( Register::RBX, 0x100 );
	
	CHECK( mem.base == Register::RBX );
	CHECK( mem.index == Register::NONE );
	CHECK( mem.displacement == 0x100 );
}

TEST_CASE( "MemoryOperand: with_base_displ_size", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_base_displ_size( Register::RCX, 0x10, 1 );
	
	CHECK( mem.base == Register::RCX );
	CHECK( mem.displacement == 0x10 );
	CHECK( mem.displ_size == 1 );
}

TEST_CASE( "MemoryOperand: with_base_index", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_base_index( Register::RAX, Register::RBX );
	
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::RBX );
	CHECK( mem.scale == 1 );
}

TEST_CASE( "MemoryOperand: with_base_index_scale", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_base_index_scale( Register::RAX, Register::RCX, 4 );
	
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::RCX );
	CHECK( mem.scale == 4 );
}

TEST_CASE( "MemoryOperand: with_base_index_scale_displ_size", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_base_index_scale_displ_size( Register::RAX, Register::RCX, 8, 0x1000, 4 );
	
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::RCX );
	CHECK( mem.scale == 8 );
	CHECK( mem.displacement == 0x1000 );
	CHECK( mem.displ_size == 4 );
}

TEST_CASE( "MemoryOperand: with_index_scale_displ_size", "[memory_operand][manual]" ) {
	auto mem = MemoryOperand::with_index_scale_displ_size( Register::RCX, 4, 0x2000, 4 );
	
	CHECK( mem.base == Register::NONE );
	CHECK( mem.index == Register::RCX );
	CHECK( mem.scale == 4 );
	CHECK( mem.displacement == 0x2000 );
}

TEST_CASE( "MemoryOperand: with segment override", "[memory_operand][manual]" ) {
	MemoryOperand mem;
	mem.base = Register::RAX;
	mem.segment_prefix = Register::FS;
	
	CHECK( mem.segment_prefix == Register::FS );
}

// ============================================================================
// Instruction creation with memory operand encoding
// ============================================================================

TEST_CASE( "InstructionFactory: encode and decode with SIB", "[instruction_create][encoder][manual]" ) {
	// Create MOV EAX, [EBX+ECX*4+0x100]
	auto mem = MemoryOperand::with_base_index_scale_displ_size( Register::EBX, Register::ECX, 4, 0x100, 4 );
	auto instr = InstructionFactory::with2( Code::MOV_R32_RM32, Register::EAX, mem );
	
	// Encode
	Encoder encoder( 32 );
	auto encode_result = encoder.encode( instr, 0x1000 );
	REQUIRE( encode_result.has_value() );
	
	// Decode
	auto bytes = encoder.buffer();
	Decoder decoder( 32, bytes, 0x1000 );
	auto decoded = decoder.decode();
	REQUIRE( decoded.has_value() );
	
	CHECK( decoded->memory_base() == Register::EBX );
	CHECK( decoded->memory_index() == Register::ECX );
	CHECK( decoded->memory_index_scale() == 4 );
}
