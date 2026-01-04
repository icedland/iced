// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"

using namespace iced_x86;

// ============================================================================
// InstructionInfoFactory tests
// ============================================================================

TEST_CASE( "InstructionInfoFactory: basic register analysis", "[instruction_info][manual]" ) {
	// MOV EAX, EBX (89 D8)
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	InstructionInfoFactory factory;
	const auto& info = factory.info( instr );
	
	// Check that we have register usage
	auto regs = info.used_registers();
	CHECK( regs.size() >= 1 );  // At least EAX should be used
}

TEST_CASE( "InstructionInfoFactory: memory operand analysis", "[instruction_info][manual]" ) {
	// MOV EAX, [EBX+ECX*4+0x10] (8B 44 8B 10)
	const uint8_t data[] = { 0x8B, 0x44, 0x8B, 0x10 };
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	InstructionInfoFactory factory;
	const auto& info = factory.info( instr );
	
	// Check memory usage
	auto mem = info.used_memory();
	CHECK( mem.size() >= 1 );
	
	if ( !mem.empty() ) {
		CHECK( mem[0].base == Register::EBX );
		CHECK( mem[0].index == Register::ECX );
		CHECK( mem[0].scale == 4 );
	}
	
	// Check register usage includes base and index
	auto regs = info.used_registers();
	CHECK( regs.size() >= 2 );  // EBX and ECX should be read
}

TEST_CASE( "InstructionInfoFactory: operand access types", "[instruction_info][manual]" ) {
	// ADD EAX, EBX - first operand is read/write, second is read
	const uint8_t data[] = { 0x01, 0xD8 };  // ADD EAX, EBX
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	InstructionInfoFactory factory;
	const auto& info = factory.info( instr );
	
	// Op0 should be write or read_write (destination)
	CHECK( ( info.op0_access() == OpAccess::WRITE || 
	         info.op0_access() == OpAccess::READ_WRITE ) );
	
	// Op1 should be read (source)
	CHECK( info.op1_access() == OpAccess::READ );
}

TEST_CASE( "InstructionInfoFactory: no memory option", "[instruction_info][manual]" ) {
	// MOV EAX, [EBX]
	const uint8_t data[] = { 0x8B, 0x03 };
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	InstructionInfoFactory factory;
	const auto& info = factory.info( instr, InstructionInfoOptions::NO_MEMORY_USAGE );
	
	// Memory should be empty when NO_MEMORY_USAGE is set
	CHECK( info.used_memory().empty() );
}

TEST_CASE( "InstructionInfoFactory: no register option", "[instruction_info][manual]" ) {
	// MOV EAX, EBX
	const uint8_t data[] = { 0x89, 0xD8 };
	Decoder decoder( 32, data, 0x1000 );
	auto instr = decoder.decode().value();
	
	InstructionInfoFactory factory;
	const auto& info = factory.info( instr, InstructionInfoOptions::NO_REGISTER_USAGE );
	
	// Registers should be empty when NO_REGISTER_USAGE is set
	CHECK( info.used_registers().empty() );
}

// ============================================================================
// RegisterExtensions tests
// ============================================================================

TEST_CASE( "RegisterExtensions: get_size", "[register_info][manual]" ) {
	using namespace RegisterExtensions;
	
	CHECK( get_size( Register::NONE ) == 0 );
	CHECK( get_size( Register::AL ) == 1 );
	CHECK( get_size( Register::AX ) == 2 );
	CHECK( get_size( Register::EAX ) == 4 );
	CHECK( get_size( Register::RAX ) == 8 );
	CHECK( get_size( Register::XMM0 ) == 16 );
	CHECK( get_size( Register::YMM0 ) == 32 );
	CHECK( get_size( Register::ZMM0 ) == 64 );
}

TEST_CASE( "RegisterExtensions: get_number", "[register_info][manual]" ) {
	using namespace RegisterExtensions;
	
	CHECK( get_number( Register::AL ) == 0 );
	CHECK( get_number( Register::CL ) == 1 );
	CHECK( get_number( Register::DL ) == 2 );
	CHECK( get_number( Register::BL ) == 3 );
	
	CHECK( get_number( Register::EAX ) == 0 );
	CHECK( get_number( Register::ECX ) == 1 );
	CHECK( get_number( Register::R8_D ) == 8 );
	
	CHECK( get_number( Register::XMM0 ) == 0 );
	CHECK( get_number( Register::XMM15 ) == 15 );
}

TEST_CASE( "RegisterExtensions: get_full_register", "[register_info][manual]" ) {
	using namespace RegisterExtensions;
	
	// GPRs should expand to 64-bit
	CHECK( get_full_register( Register::AL ) == Register::RAX );
	CHECK( get_full_register( Register::AX ) == Register::RAX );
	CHECK( get_full_register( Register::EAX ) == Register::RAX );
	CHECK( get_full_register( Register::RAX ) == Register::RAX );
	
	CHECK( get_full_register( Register::CL ) == Register::RCX );
	CHECK( get_full_register( Register::R8_L ) == Register::R8 );
	
	// Vector regs should expand to ZMM
	CHECK( get_full_register( Register::XMM0 ) == Register::ZMM0 );
	CHECK( get_full_register( Register::YMM0 ) == Register::ZMM0 );
	CHECK( get_full_register( Register::ZMM0 ) == Register::ZMM0 );
}

TEST_CASE( "RegisterExtensions: get_full_register32", "[register_info][manual]" ) {
	using namespace RegisterExtensions;
	
	// GPRs should expand to 32-bit
	CHECK( get_full_register32( Register::AL ) == Register::EAX );
	CHECK( get_full_register32( Register::AX ) == Register::EAX );
	CHECK( get_full_register32( Register::EAX ) == Register::EAX );
	CHECK( get_full_register32( Register::RAX ) == Register::EAX );
}

TEST_CASE( "RegisterExtensions: type checks", "[register_info][manual]" ) {
	using namespace RegisterExtensions;
	
	// Segment registers
	CHECK( is_segment_register( Register::ES ) );
	CHECK( is_segment_register( Register::CS ) );
	CHECK( is_segment_register( Register::FS ) );
	CHECK( !is_segment_register( Register::EAX ) );
	
	// GPR types
	CHECK( is_gpr8( Register::AL ) );
	CHECK( is_gpr8( Register::AH ) );
	CHECK( is_gpr8( Register::R8_L ) );
	CHECK( !is_gpr8( Register::EAX ) );
	
	CHECK( is_gpr16( Register::AX ) );
	CHECK( is_gpr16( Register::R8_W ) );
	CHECK( !is_gpr16( Register::EAX ) );
	
	CHECK( is_gpr32( Register::EAX ) );
	CHECK( is_gpr32( Register::R8_D ) );
	CHECK( !is_gpr32( Register::RAX ) );
	
	CHECK( is_gpr64( Register::RAX ) );
	CHECK( is_gpr64( Register::R8 ) );
	CHECK( !is_gpr64( Register::EAX ) );
	
	CHECK( is_gpr( Register::AL ) );
	CHECK( is_gpr( Register::EAX ) );
	CHECK( is_gpr( Register::RAX ) );
	CHECK( !is_gpr( Register::XMM0 ) );
	
	// Vector registers
	CHECK( is_xmm( Register::XMM0 ) );
	CHECK( is_xmm( Register::XMM31 ) );
	CHECK( !is_xmm( Register::YMM0 ) );
	
	CHECK( is_ymm( Register::YMM0 ) );
	CHECK( is_ymm( Register::YMM31 ) );
	CHECK( !is_ymm( Register::XMM0 ) );
	
	CHECK( is_zmm( Register::ZMM0 ) );
	CHECK( is_zmm( Register::ZMM31 ) );
	CHECK( !is_zmm( Register::XMM0 ) );
	
	CHECK( is_vector_register( Register::XMM0 ) );
	CHECK( is_vector_register( Register::YMM0 ) );
	CHECK( is_vector_register( Register::ZMM0 ) );
	CHECK( !is_vector_register( Register::EAX ) );
	
	// Opmask registers
	CHECK( is_k( Register::K0 ) );
	CHECK( is_k( Register::K7 ) );
	CHECK( !is_k( Register::EAX ) );
	
	// Other register types
	CHECK( is_st( Register::ST0 ) );
	CHECK( is_mm( Register::MM0 ) );
	CHECK( is_cr( Register::CR0 ) );
	CHECK( is_dr( Register::DR0 ) );
}

TEST_CASE( "RegisterExtensions: get_info", "[register_info][manual]" ) {
	using namespace RegisterExtensions;
	
	auto info = get_info( Register::ECX );
	CHECK( info.register_ == Register::ECX );
	CHECK( info.base == Register::EAX );
	CHECK( info.full_register == Register::RCX );
	CHECK( info.full_register32 == Register::ECX );
	CHECK( info.number == 1 );
	CHECK( info.size == 4 );
}

// ============================================================================
// InstructionExtensions tests
// ============================================================================

TEST_CASE( "InstructionExtensions: flow_control", "[instruction_info][manual]" ) {
	using namespace InstructionExtensions;
	
	// JMP rel8 - unconditional branch
	{
		const uint8_t data[] = { 0xEB, 0x10 };  // JMP SHORT +0x10
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( flow_control( instr ) == FlowControl::UNCONDITIONAL_BRANCH );
	}
	
	// JE rel8 - conditional branch
	{
		const uint8_t data[] = { 0x74, 0x10 };  // JE SHORT +0x10
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( flow_control( instr ) == FlowControl::CONDITIONAL_BRANCH );
	}
	
	// RET - return
	{
		const uint8_t data[] = { 0xC3 };  // RET
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( flow_control( instr ) == FlowControl::RETURN );
	}
	
	// NOP - next
	{
		const uint8_t data[] = { 0x90 };  // NOP
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( flow_control( instr ) == FlowControl::NEXT );
	}
}

TEST_CASE( "InstructionExtensions: is_stack_instruction", "[instruction_info][manual]" ) {
	using namespace InstructionExtensions;
	
	// PUSH RAX
	{
		const uint8_t data[] = { 0x50 };
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( is_stack_instruction( instr ) );
	}
	
	// POP RAX
	{
		const uint8_t data[] = { 0x58 };
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( is_stack_instruction( instr ) );
	}
	
	// MOV EAX, EBX - not a stack instruction
	{
		const uint8_t data[] = { 0x89, 0xD8 };
		Decoder decoder( 32, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( !is_stack_instruction( instr ) );
	}
}

TEST_CASE( "InstructionExtensions: branch type checks", "[instruction_info][manual]" ) {
	using namespace InstructionExtensions;
	
	// JMP SHORT
	{
		const uint8_t data[] = { 0xEB, 0x10 };
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( is_jmp_short( instr ) );
		CHECK( is_jmp_short_or_near( instr ) );
		CHECK( !is_jmp_near( instr ) );
	}
	
	// JE SHORT
	{
		const uint8_t data[] = { 0x74, 0x10 };
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( is_jcc_short( instr ) );
		CHECK( is_jcc_short_or_near( instr ) );
		CHECK( !is_jcc_near( instr ) );
	}
}

TEST_CASE( "InstructionExtensions: condition_code", "[instruction_info][manual]" ) {
	using namespace InstructionExtensions;
	
	// JE - equals/zero
	{
		const uint8_t data[] = { 0x74, 0x10 };
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( condition_code( instr ) == ConditionCode::E );
	}
	
	// JNE - not equals/not zero
	{
		const uint8_t data[] = { 0x75, 0x10 };
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( condition_code( instr ) == ConditionCode::NE );
	}
	
	// JO - overflow
	{
		const uint8_t data[] = { 0x70, 0x10 };
		Decoder decoder( 64, data, 0x1000 );
		auto instr = decoder.decode().value();
		CHECK( condition_code( instr ) == ConditionCode::O );
	}
}

TEST_CASE( "InstructionExtensions: negate_condition_code", "[instruction_info][manual]" ) {
	using namespace InstructionExtensions;
	
	// JE -> JNE
	CHECK( negate_condition_code( Code::JE_REL8_64 ) == Code::JNE_REL8_64 );
	CHECK( negate_condition_code( Code::JNE_REL8_64 ) == Code::JE_REL8_64 );
	
	// JO -> JNO
	CHECK( negate_condition_code( Code::JO_REL8_64 ) == Code::JNO_REL8_64 );
	CHECK( negate_condition_code( Code::JNO_REL8_64 ) == Code::JO_REL8_64 );
}

TEST_CASE( "InstructionExtensions: to_short_branch", "[instruction_info][manual]" ) {
	using namespace InstructionExtensions;
	
	// JE NEAR -> JE SHORT
	CHECK( to_short_branch( Code::JE_REL32_64 ) == Code::JE_REL8_64 );
	
	// JMP NEAR -> JMP SHORT
	CHECK( to_short_branch( Code::JMP_REL32_64 ) == Code::JMP_REL8_64 );
}

TEST_CASE( "InstructionExtensions: to_near_branch", "[instruction_info][manual]" ) {
	using namespace InstructionExtensions;
	
	// JE SHORT -> JE NEAR
	CHECK( to_near_branch( Code::JE_REL8_64 ) == Code::JE_REL32_64 );
	
	// JMP SHORT -> JMP NEAR
	CHECK( to_near_branch( Code::JMP_REL8_64 ) == Code::JMP_REL32_64 );
}

// ============================================================================
// Decoder Iterator tests
// ============================================================================

TEST_CASE( "Decoder: iterator support", "[decoder][iterator][manual]" ) {
	// Multiple instructions: NOP, NOP, RET
	const uint8_t data[] = { 0x90, 0x90, 0xC3 };
	Decoder decoder( 64, data, 0x1000 );
	
	int count = 0;
	for ( const auto& instr : decoder ) {
		(void)instr;
		++count;
	}
	
	CHECK( count == 3 );
}

TEST_CASE( "Decoder: iterator with complex instructions", "[decoder][iterator][manual]" ) {
	// MOV EAX, 0x12345678 (B8 78 56 34 12)
	// ADD EAX, EBX (01 D8)
	// RET (C3)
	const uint8_t data[] = { 0xB8, 0x78, 0x56, 0x34, 0x12, 0x01, 0xD8, 0xC3 };
	Decoder decoder( 32, data, 0x1000 );
	
	std::vector<Mnemonic> mnemonics;
	for ( const auto& instr : decoder ) {
		mnemonics.push_back( instr.mnemonic() );
	}
	
	REQUIRE( mnemonics.size() == 3 );
	CHECK( mnemonics[0] == Mnemonic::MOV );
	CHECK( mnemonics[1] == Mnemonic::ADD );
	CHECK( mnemonics[2] == Mnemonic::RET );
}

TEST_CASE( "Decoder: empty data iterator", "[decoder][iterator][manual]" ) {
	std::span<const uint8_t> empty_data;
	Decoder decoder( 64, empty_data, 0x1000 );
	
	int count = 0;
	for ( const auto& instr : decoder ) {
		(void)instr;
		++count;
	}
	
	CHECK( count == 0 );
}

// ============================================================================
// MemorySizeInfo tests
// ============================================================================

TEST_CASE( "MemorySizeInfo: basic scalar types", "[memory_size_info][manual]" ) {
	using namespace memory_size_ext;
	
	// UINT8
	{
		auto info = get_info( MemorySize::UINT8 );
		CHECK( info.memory_size == MemorySize::UINT8 );
		CHECK( info.size == 1 );
		CHECK( info.element_size == 1 );
		CHECK( info.element_type == MemorySize::UINT8 );
		CHECK( info.is_signed == false );
		CHECK( info.is_broadcast == false );
		CHECK( !info.is_packed() );
		CHECK( info.element_count() == 1 );
	}
	
	// UINT32
	{
		auto info = get_info( MemorySize::UINT32 );
		CHECK( info.memory_size == MemorySize::UINT32 );
		CHECK( info.size == 4 );
		CHECK( info.element_size == 4 );
		CHECK( info.element_type == MemorySize::UINT32 );
		CHECK( info.is_signed == false );
		CHECK( !info.is_packed() );
	}
	
	// INT32
	{
		auto info = get_info( MemorySize::INT32 );
		CHECK( info.memory_size == MemorySize::INT32 );
		CHECK( info.size == 4 );
		CHECK( info.is_signed == true );
	}
	
	// UINT64
	{
		auto info = get_info( MemorySize::UINT64 );
		CHECK( info.size == 8 );
		CHECK( info.element_size == 8 );
	}
}

TEST_CASE( "MemorySizeInfo: floating point types", "[memory_size_info][manual]" ) {
	using namespace memory_size_ext;
	
	// FLOAT32
	{
		auto info = get_info( MemorySize::FLOAT32 );
		CHECK( info.size == 4 );
		CHECK( info.is_signed == true );  // Floats are signed
		CHECK( !info.is_broadcast );
	}
	
	// FLOAT64
	{
		auto info = get_info( MemorySize::FLOAT64 );
		CHECK( info.size == 8 );
		CHECK( info.is_signed == true );
	}
	
	// FLOAT80
	{
		auto info = get_info( MemorySize::FLOAT80 );
		CHECK( info.size == 10 );
		CHECK( info.is_signed == true );
	}
}

TEST_CASE( "MemorySizeInfo: packed types", "[memory_size_info][manual]" ) {
	using namespace memory_size_ext;
	
	// PACKED128_UINT8 - 16 bytes, 16 elements of 1 byte each
	{
		auto info = get_info( MemorySize::PACKED128_UINT8 );
		CHECK( info.size == 16 );
		CHECK( info.element_size == 1 );
		CHECK( info.element_type == MemorySize::UINT8 );
		CHECK( info.is_packed() );
		CHECK( info.element_count() == 16 );
		CHECK( !info.is_broadcast );
	}
	
	// PACKED256_UINT16 - 32 bytes, 16 elements of 2 bytes each
	{
		auto info = get_info( MemorySize::PACKED256_UINT16 );
		CHECK( info.size == 32 );
		CHECK( info.element_size == 2 );
		CHECK( info.element_type == MemorySize::UINT16 );
		CHECK( info.is_packed() );
		CHECK( info.element_count() == 16 );
	}
	
	// PACKED512_FLOAT32 - 64 bytes, 16 elements of 4 bytes each
	{
		auto info = get_info( MemorySize::PACKED512_FLOAT32 );
		CHECK( info.size == 64 );
		CHECK( info.element_size == 4 );
		CHECK( info.element_type == MemorySize::FLOAT32 );
		CHECK( info.is_packed() );
		CHECK( info.element_count() == 16 );
		CHECK( info.is_signed == true );  // Float is signed
	}
	
	// PACKED128_FLOAT64 - 16 bytes, 2 elements of 8 bytes each
	{
		auto info = get_info( MemorySize::PACKED128_FLOAT64 );
		CHECK( info.size == 16 );
		CHECK( info.element_size == 8 );
		CHECK( info.element_type == MemorySize::FLOAT64 );
		CHECK( info.is_packed() );
		CHECK( info.element_count() == 2 );
	}
}

TEST_CASE( "MemorySizeInfo: broadcast types", "[memory_size_info][manual]" ) {
	using namespace memory_size_ext;
	
	// BROADCAST512_UINT64 - broadcasts 8 bytes to 512-bit register
	{
		auto info = get_info( MemorySize::BROADCAST512_UINT64 );
		CHECK( info.size == 8 );  // Memory access size
		CHECK( info.element_size == 8 );
		CHECK( info.element_type == MemorySize::UINT64 );
		CHECK( info.is_broadcast == true );
		CHECK( !info.is_packed() );  // Broadcast is not packed
		CHECK( info.element_count() == 1 );
	}
	
	// BROADCAST256_FLOAT32
	{
		auto info = get_info( MemorySize::BROADCAST256_FLOAT32 );
		CHECK( info.size == 4 );
		CHECK( info.element_type == MemorySize::FLOAT32 );
		CHECK( info.is_broadcast == true );
		CHECK( info.is_signed == true );
	}
	
	// BROADCAST128_INT32
	{
		auto info = get_info( MemorySize::BROADCAST128_INT32 );
		CHECK( info.size == 4 );
		CHECK( info.element_type == MemorySize::INT32 );
		CHECK( info.is_broadcast == true );
		CHECK( info.is_signed == true );
	}
}

TEST_CASE( "MemorySizeInfo: helper functions", "[memory_size_info][manual]" ) {
	using namespace memory_size_ext;
	
	// get_size
	CHECK( get_size( MemorySize::UINT32 ) == 4 );
	CHECK( get_size( MemorySize::PACKED256_UINT16 ) == 32 );
	CHECK( get_size( MemorySize::BROADCAST512_UINT64 ) == 8 );
	
	// get_element_size
	CHECK( get_element_size( MemorySize::UINT32 ) == 4 );
	CHECK( get_element_size( MemorySize::PACKED256_UINT16 ) == 2 );
	CHECK( get_element_size( MemorySize::BROADCAST512_UINT64 ) == 8 );
	
	// get_element_type
	CHECK( get_element_type( MemorySize::UINT32 ) == MemorySize::UINT32 );
	CHECK( get_element_type( MemorySize::PACKED256_UINT16 ) == MemorySize::UINT16 );
	CHECK( get_element_type( MemorySize::BROADCAST512_UINT64 ) == MemorySize::UINT64 );
	
	// is_signed
	CHECK( !is_signed( MemorySize::UINT32 ) );
	CHECK( is_signed( MemorySize::INT32 ) );
	CHECK( is_signed( MemorySize::FLOAT64 ) );
	
	// is_packed
	CHECK( !is_packed( MemorySize::UINT32 ) );
	CHECK( is_packed( MemorySize::PACKED256_UINT16 ) );
	CHECK( !is_packed( MemorySize::BROADCAST512_UINT64 ) );
	
	// get_element_count
	CHECK( get_element_count( MemorySize::UINT32 ) == 1 );
	CHECK( get_element_count( MemorySize::PACKED256_UINT16 ) == 16 );
	CHECK( get_element_count( MemorySize::BROADCAST512_UINT64 ) == 1 );
	
	// is_broadcast
	CHECK( !is_broadcast( MemorySize::UINT32 ) );
	CHECK( !is_broadcast( MemorySize::PACKED256_UINT16 ) );
	CHECK( is_broadcast( MemorySize::BROADCAST512_UINT64 ) );
}

TEST_CASE( "MemorySizeInfo: special sizes", "[memory_size_info][manual]" ) {
	using namespace memory_size_ext;
	
	// SEG_PTR16 - 4 bytes (2 byte offset + 2 byte selector)
	{
		auto info = get_info( MemorySize::SEG_PTR16 );
		CHECK( info.size == 4 );
	}
	
	// SEG_PTR32 - 6 bytes (4 byte offset + 2 byte selector)
	{
		auto info = get_info( MemorySize::SEG_PTR32 );
		CHECK( info.size == 6 );
	}
	
	// SEG_PTR64 - 10 bytes (8 byte offset + 2 byte selector)
	{
		auto info = get_info( MemorySize::SEG_PTR64 );
		CHECK( info.size == 10 );
	}
	
	// FWORD6 - 6 bytes (limit + 32-bit address)
	{
		auto info = get_info( MemorySize::FWORD6 );
		CHECK( info.size == 6 );
	}
	
	// FWORD10 - 10 bytes (limit + 64-bit address)
	{
		auto info = get_info( MemorySize::FWORD10 );
		CHECK( info.size == 10 );
	}
	
	// BCD - 10 bytes
	{
		auto info = get_info( MemorySize::BCD );
		CHECK( info.size == 10 );
		CHECK( info.is_signed == true );
	}
	
	// FXSAVE_512BYTE - 512 bytes
	{
		auto info = get_info( MemorySize::FXSAVE_512BYTE );
		CHECK( info.size == 512 );
	}
	
	// UNKNOWN - 0 bytes
	{
		auto info = get_info( MemorySize::UNKNOWN );
		CHECK( info.size == 0 );
		CHECK( info.element_size == 0 );
	}
}

TEST_CASE( "MemorySizeInfo: element_type_info", "[memory_size_info][manual]" ) {
	using namespace memory_size_ext;
	
	// For packed type, element_type_info should give info about the element
	{
		auto& elem_info = get_element_type_info( MemorySize::PACKED256_UINT16 );
		CHECK( elem_info.memory_size == MemorySize::UINT16 );
		CHECK( elem_info.size == 2 );
		CHECK( elem_info.element_size == 2 );
	}
	
	// For scalar type, element_type_info should be same as the type itself
	{
		auto& elem_info = get_element_type_info( MemorySize::UINT32 );
		CHECK( elem_info.memory_size == MemorySize::UINT32 );
		CHECK( elem_info.size == 4 );
	}
	
	// For broadcast type, element_type_info should give info about the broadcasted element
	{
		auto& elem_info = get_element_type_info( MemorySize::BROADCAST512_FLOAT64 );
		CHECK( elem_info.memory_size == MemorySize::FLOAT64 );
		CHECK( elem_info.size == 8 );
	}
}
