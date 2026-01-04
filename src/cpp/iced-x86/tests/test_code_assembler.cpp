// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/code_label.hpp"
#include "iced_x86/asm_memory_operand.hpp"
#include "iced_x86/asm_registers.hpp"
#include "iced_x86/asm_register_constants.hpp"
#include "iced_x86/code_assembler.hpp"
#include "iced_x86/decoder.hpp"

using namespace iced_x86;

// ============================================================================
// CodeLabel tests
// ============================================================================

TEST_CASE( "CodeLabel: default constructor creates empty label", "[assembler][label]" ) {
	CodeLabel label;
	CHECK( label.is_empty() );
	CHECK( !label.has_instruction_index() );
	CHECK( label.id() == 0 );
}

TEST_CASE( "CodeLabel: constructor with ID", "[assembler][label]" ) {
	CodeLabel label( 42 );
	CHECK( !label.is_empty() );
	CHECK( !label.has_instruction_index() );
	CHECK( label.id() == 42 );
}

TEST_CASE( "CodeLabel: equality comparison", "[assembler][label]" ) {
	CodeLabel label1( 1 );
	CodeLabel label2( 1 );
	CodeLabel label3( 2 );
	CodeLabel empty1;
	CodeLabel empty2;

	CHECK( label1 == label2 );
	CHECK( label1 != label3 );
	CHECK( empty1 == empty2 );
	CHECK( label1 != empty1 );
}

// ============================================================================
// AsmMemoryOperand tests
// ============================================================================

TEST_CASE( "AsmMemoryOperand: default constructor", "[assembler][memory]" ) {
	AsmMemoryOperand mem;
	CHECK( mem.base == Register::NONE );
	CHECK( mem.index == Register::NONE );
	CHECK( mem.scale == 1 );
	CHECK( mem.displacement == 0 );
	CHECK( mem.segment == Register::NONE );
	CHECK( mem.size == AsmMemoryOperandSize::NONE );
}

TEST_CASE( "AsmMemoryOperand: ptr from displacement", "[assembler][memory]" ) {
	auto mem = ptr( 0x1000 );
	CHECK( mem.is_displacement_only() );
	CHECK( mem.displacement == 0x1000 );
	CHECK( mem.size == AsmMemoryOperandSize::NONE );
}

TEST_CASE( "AsmMemoryOperand: sized ptr functions", "[assembler][memory]" ) {
	SECTION( "byte_ptr" ) {
		auto mem = byte_ptr( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::BYTE );
		CHECK( mem.displacement == 0x1000 );
	}

	SECTION( "word_ptr" ) {
		auto mem = word_ptr( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::WORD );
	}

	SECTION( "dword_ptr" ) {
		auto mem = dword_ptr( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::DWORD );
	}

	SECTION( "qword_ptr" ) {
		auto mem = qword_ptr( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::QWORD );
	}

	SECTION( "xmmword_ptr" ) {
		auto mem = xmmword_ptr( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::XWORD );
	}

	SECTION( "ymmword_ptr" ) {
		auto mem = ymmword_ptr( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::YWORD );
	}

	SECTION( "zmmword_ptr" ) {
		auto mem = zmmword_ptr( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::ZWORD );
	}
}

TEST_CASE( "AsmMemoryOperand: broadcast ptr functions", "[assembler][memory]" ) {
	SECTION( "dword_bcst" ) {
		auto mem = dword_bcst( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::DWORD );
		CHECK( mem.is_broadcast() );
	}

	SECTION( "qword_bcst" ) {
		auto mem = qword_bcst( 0x1000 );
		CHECK( mem.size == AsmMemoryOperandSize::QWORD );
		CHECK( mem.is_broadcast() );
	}
}

TEST_CASE( "AsmMemoryOperand: segment overrides", "[assembler][memory]" ) {
	auto mem = ptr( 0x1000 );

	CHECK( mem.cs().segment == Register::CS );
	CHECK( mem.ss().segment == Register::SS );
	CHECK( mem.ds().segment == Register::DS );
	CHECK( mem.es().segment == Register::ES );
	CHECK( mem.fs().segment == Register::FS );
	CHECK( mem.gs().segment == Register::GS );
}

TEST_CASE( "AsmMemoryOperand: mask register methods", "[assembler][memory]" ) {
	auto mem = ptr( 0x1000 );

	CHECK( ( mem.k1().flags & AsmOperandFlags::REGISTER_MASK ) == AsmOperandFlags::K1 );
	CHECK( ( mem.k2().flags & AsmOperandFlags::REGISTER_MASK ) == AsmOperandFlags::K2 );
	CHECK( ( mem.k3().flags & AsmOperandFlags::REGISTER_MASK ) == AsmOperandFlags::K3 );
	CHECK( ( mem.k4().flags & AsmOperandFlags::REGISTER_MASK ) == AsmOperandFlags::K4 );
	CHECK( ( mem.k5().flags & AsmOperandFlags::REGISTER_MASK ) == AsmOperandFlags::K5 );
	CHECK( ( mem.k6().flags & AsmOperandFlags::REGISTER_MASK ) == AsmOperandFlags::K6 );
	CHECK( ( mem.k7().flags & AsmOperandFlags::REGISTER_MASK ) == AsmOperandFlags::K7 );
}

TEST_CASE( "AsmMemoryOperand: arithmetic operators", "[assembler][memory]" ) {
	auto mem = ptr( 0x1000 );

	CHECK( ( mem + 0x100 ).displacement == 0x1100 );
	CHECK( ( mem - 0x100 ).displacement == 0x0F00 );
}

// ============================================================================
// Register constant tests
// ============================================================================

TEST_CASE( "Register constants: 8-bit registers", "[assembler][registers]" ) {
	CHECK( al.value == Register::AL );
	CHECK( cl.value == Register::CL );
	CHECK( dl.value == Register::DL );
	CHECK( bl.value == Register::BL );
	CHECK( ah.value == Register::AH );
	CHECK( ch.value == Register::CH );
	CHECK( dh.value == Register::DH );
	CHECK( bh.value == Register::BH );
	CHECK( spl.value == Register::SPL );
	CHECK( bpl.value == Register::BPL );
	CHECK( sil.value == Register::SIL );
	CHECK( dil.value == Register::DIL );
	CHECK( r8l.value == Register::R8_L );
	CHECK( r15l.value == Register::R15_L );
}

TEST_CASE( "Register constants: 16-bit registers", "[assembler][registers]" ) {
	CHECK( ax.value == Register::AX );
	CHECK( cx.value == Register::CX );
	CHECK( dx.value == Register::DX );
	CHECK( bx.value == Register::BX );
	CHECK( sp.value == Register::SP );
	CHECK( bp.value == Register::BP );
	CHECK( si.value == Register::SI );
	CHECK( di.value == Register::DI );
	CHECK( r8w.value == Register::R8_W );
	CHECK( r15w.value == Register::R15_W );
}

TEST_CASE( "Register constants: 32-bit registers", "[assembler][registers]" ) {
	CHECK( eax.value == Register::EAX );
	CHECK( ecx.value == Register::ECX );
	CHECK( edx.value == Register::EDX );
	CHECK( ebx.value == Register::EBX );
	CHECK( esp.value == Register::ESP );
	CHECK( ebp.value == Register::EBP );
	CHECK( esi.value == Register::ESI );
	CHECK( edi.value == Register::EDI );
	CHECK( r8d.value == Register::R8_D );
	CHECK( r15d.value == Register::R15_D );
}

TEST_CASE( "Register constants: 64-bit registers", "[assembler][registers]" ) {
	CHECK( rax.value == Register::RAX );
	CHECK( rcx.value == Register::RCX );
	CHECK( rdx.value == Register::RDX );
	CHECK( rbx.value == Register::RBX );
	CHECK( rsp.value == Register::RSP );
	CHECK( rbp.value == Register::RBP );
	CHECK( rsi.value == Register::RSI );
	CHECK( rdi.value == Register::RDI );
	CHECK( r8.value == Register::R8 );
	CHECK( r15.value == Register::R15 );
}

TEST_CASE( "Register constants: XMM registers", "[assembler][registers]" ) {
	CHECK( xmm0.value == Register::XMM0 );
	CHECK( xmm1.value == Register::XMM1 );
	CHECK( xmm15.value == Register::XMM15 );
	CHECK( xmm31.value == Register::XMM31 );
}

TEST_CASE( "Register constants: YMM registers", "[assembler][registers]" ) {
	CHECK( ymm0.value == Register::YMM0 );
	CHECK( ymm1.value == Register::YMM1 );
	CHECK( ymm15.value == Register::YMM15 );
	CHECK( ymm31.value == Register::YMM31 );
}

TEST_CASE( "Register constants: ZMM registers", "[assembler][registers]" ) {
	CHECK( zmm0.value == Register::ZMM0 );
	CHECK( zmm1.value == Register::ZMM1 );
	CHECK( zmm15.value == Register::ZMM15 );
	CHECK( zmm31.value == Register::ZMM31 );
}

TEST_CASE( "Register constants: mask registers", "[assembler][registers]" ) {
	CHECK( k0.value == Register::K0 );
	CHECK( k1.value == Register::K1 );
	CHECK( k7.value == Register::K7 );
}

TEST_CASE( "Register constants: FPU registers", "[assembler][registers]" ) {
	CHECK( st0.value == Register::ST0 );
	CHECK( st7.value == Register::ST7 );
}

TEST_CASE( "Register constants: MMX registers", "[assembler][registers]" ) {
	CHECK( mm0.value == Register::MM0 );
	CHECK( mm7.value == Register::MM7 );
}

// ============================================================================
// Register expression tests (building memory operands)
// ============================================================================

TEST_CASE( "Register expressions: register + displacement", "[assembler][expressions]" ) {
	auto mem = rax + 0x10;
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::NONE );
	CHECK( mem.displacement == 0x10 );
	CHECK( mem.scale == 1 );
}

TEST_CASE( "Register expressions: register - displacement", "[assembler][expressions]" ) {
	auto mem = rax - 0x10;
	CHECK( mem.base == Register::RAX );
	CHECK( mem.displacement == -0x10 );
}

TEST_CASE( "Register expressions: register * scale", "[assembler][expressions]" ) {
	auto mem = rax * 4;
	CHECK( mem.base == Register::NONE );
	CHECK( mem.index == Register::RAX );
	CHECK( mem.scale == 4 );
}

TEST_CASE( "Register expressions: register + register", "[assembler][expressions]" ) {
	auto mem = rax + rcx;
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::RCX );
	CHECK( mem.scale == 1 );
}

TEST_CASE( "Register expressions: base + index*scale", "[assembler][expressions]" ) {
	auto mem = rax + rcx * 4;
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::RCX );
	CHECK( mem.scale == 4 );
}

TEST_CASE( "Register expressions: base + index*scale + displacement", "[assembler][expressions]" ) {
	auto mem = rax + rcx * 4 + 0x10;
	CHECK( mem.base == Register::RAX );
	CHECK( mem.index == Register::RCX );
	CHECK( mem.scale == 4 );
	CHECK( mem.displacement == 0x10 );
}

TEST_CASE( "Register expressions: 32-bit registers", "[assembler][expressions]" ) {
	auto mem = eax + ecx * 2 + 0x100;
	CHECK( mem.base == Register::EAX );
	CHECK( mem.index == Register::ECX );
	CHECK( mem.scale == 2 );
	CHECK( mem.displacement == 0x100 );
}

// ============================================================================
// CodeAssembler construction tests
// ============================================================================

TEST_CASE( "CodeAssembler: construction with valid bitness", "[assembler][core]" ) {
	SECTION( "64-bit" ) {
		CodeAssembler a( 64 );
		CHECK( a.bitness() == 64 );
	}

	SECTION( "32-bit" ) {
		CodeAssembler a( 32 );
		CHECK( a.bitness() == 32 );
	}

	SECTION( "16-bit" ) {
		CodeAssembler a( 16 );
		CHECK( a.bitness() == 16 );
	}
}

TEST_CASE( "CodeAssembler: construction with invalid bitness throws", "[assembler][core]" ) {
	CHECK_THROWS_AS( CodeAssembler( 0 ), std::invalid_argument );
	CHECK_THROWS_AS( CodeAssembler( 8 ), std::invalid_argument );
	CHECK_THROWS_AS( CodeAssembler( 48 ), std::invalid_argument );
	CHECK_THROWS_AS( CodeAssembler( 128 ), std::invalid_argument );
}

TEST_CASE( "CodeAssembler: default options", "[assembler][core]" ) {
	CodeAssembler a( 64 );
	CHECK( a.prefer_vex() == true );
	CHECK( a.prefer_short_branch() == true );
}

TEST_CASE( "CodeAssembler: set options", "[assembler][core]" ) {
	CodeAssembler a( 64 );

	a.set_prefer_vex( false );
	CHECK( a.prefer_vex() == false );
	a.set_prefer_vex( true );
	CHECK( a.prefer_vex() == true );

	a.set_prefer_short_branch( false );
	CHECK( a.prefer_short_branch() == false );
	a.set_prefer_short_branch( true );
	CHECK( a.prefer_short_branch() == true );
}

// ============================================================================
// CodeAssembler label tests
// ============================================================================

TEST_CASE( "CodeAssembler: create_label", "[assembler][labels]" ) {
	CodeAssembler a( 64 );

	auto label1 = a.create_label();
	auto label2 = a.create_label();

	CHECK( !label1.is_empty() );
	CHECK( !label2.is_empty() );
	CHECK( label1.id() != label2.id() );
}

TEST_CASE( "CodeAssembler: set_label", "[assembler][labels]" ) {
	CodeAssembler a( 64 );

	auto label = a.create_label();
	CHECK( !label.has_instruction_index() );

	a.set_label( label );
	CHECK( label.has_instruction_index() );
	CHECK( label.instruction_index() == 0 );

	a.nop();
	auto label2 = a.create_label();
	a.set_label( label2 );
	CHECK( label2.instruction_index() == 1 );
}

TEST_CASE( "CodeAssembler: set_label throws for empty label", "[assembler][labels]" ) {
	CodeAssembler a( 64 );
	CodeLabel empty;
	CHECK_THROWS_AS( a.set_label( empty ), std::invalid_argument );
}

TEST_CASE( "CodeAssembler: set_label throws for already set label", "[assembler][labels]" ) {
	CodeAssembler a( 64 );
	auto label = a.create_label();
	a.set_label( label );
	a.nop();
	CHECK_THROWS_AS( a.set_label( label ), std::invalid_argument );
}

TEST_CASE( "CodeAssembler: anonymous labels", "[assembler][labels]" ) {
	CodeAssembler a( 64 );

	// Define anonymous label
	a.anonymous_label();
	a.nop();

	// Get backward reference
	auto bwd_label = a.bwd();
	CHECK( !bwd_label.is_empty() );

	// Get forward reference
	auto fwd_label = a.fwd();
	CHECK( !fwd_label.is_empty() );
}

TEST_CASE( "CodeAssembler: bwd throws if no anonymous label defined", "[assembler][labels]" ) {
	CodeAssembler a( 64 );
	CHECK_THROWS_AS( a.bwd(), std::runtime_error );
}

// ============================================================================
// CodeAssembler reset tests
// ============================================================================

TEST_CASE( "CodeAssembler: reset clears instructions", "[assembler][core]" ) {
	CodeAssembler a( 64 );
	a.nop();
	a.nop();
	a.nop();
	CHECK( a.instructions().size() == 3 );

	a.reset();
	CHECK( a.instructions().size() == 0 );
}

// ============================================================================
// CodeAssembler prefix tests
// ============================================================================

TEST_CASE( "CodeAssembler: prefix methods return reference for chaining", "[assembler][prefixes]" ) {
	CodeAssembler a( 64 );

	CHECK( &a.lock() == &a );
	a.nop();  // consume prefix

	CHECK( &a.rep() == &a );
	a.nop();

	CHECK( &a.repe() == &a );
	a.nop();

	CHECK( &a.repz() == &a );
	a.nop();

	CHECK( &a.repne() == &a );
	a.nop();

	CHECK( &a.repnz() == &a );
	a.nop();

	CHECK( &a.xacquire() == &a );
	a.nop();

	CHECK( &a.xrelease() == &a );
	a.nop();

	CHECK( &a.bnd() == &a );
	a.nop();

	CHECK( &a.notrack() == &a );
	a.nop();

	CHECK( &a.vex() == &a );
	a.nop();

	CHECK( &a.evex() == &a );
	a.nop();
}

// ============================================================================
// CodeAssembler instruction tests - basic
// ============================================================================

TEST_CASE( "CodeAssembler: nop instruction", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );
	a.nop();
	CHECK( a.instructions().size() == 1 );

	auto bytes = a.assemble( 0x1000 );
	CHECK( bytes.size() == 1 );
	CHECK( bytes[0] == 0x90 );
}

TEST_CASE( "CodeAssembler: ret instruction", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );
	a.ret();
	CHECK( a.instructions().size() == 1 );

	auto bytes = a.assemble( 0x1000 );
	CHECK( bytes.size() == 1 );
	CHECK( bytes[0] == 0xC3 );
}

TEST_CASE( "CodeAssembler: int3 instruction", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );
	a.int3();
	CHECK( a.instructions().size() == 1 );

	auto bytes = a.assemble( 0x1000 );
	CHECK( bytes.size() == 1 );
	CHECK( bytes[0] == 0xCC );
}

// ============================================================================
// CodeAssembler instruction tests - push/pop
// ============================================================================

TEST_CASE( "CodeAssembler: push instructions", "[assembler][instructions]" ) {
	SECTION( "push r64" ) {
		CodeAssembler a( 64 );
		a.push( rbp );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "push r32" ) {
		CodeAssembler a( 32 );
		a.push( ebp );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "push r16" ) {
		CodeAssembler a( 16 );
		a.push( bp );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

TEST_CASE( "CodeAssembler: pop instructions", "[assembler][instructions]" ) {
	SECTION( "pop r64" ) {
		CodeAssembler a( 64 );
		a.pop( rbp );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "pop r32" ) {
		CodeAssembler a( 32 );
		a.pop( ebp );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "pop r16" ) {
		CodeAssembler a( 16 );
		a.pop( bp );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

// ============================================================================
// CodeAssembler instruction tests - mov
// ============================================================================

TEST_CASE( "CodeAssembler: mov reg, reg", "[assembler][instructions]" ) {
	SECTION( "mov r64, r64" ) {
		CodeAssembler a( 64 );
		a.mov( rax, rcx );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "mov r32, r32" ) {
		CodeAssembler a( 64 );
		a.mov( eax, ecx );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "mov r16, r16" ) {
		CodeAssembler a( 64 );
		a.mov( ax, cx );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "mov r8, r8" ) {
		CodeAssembler a( 64 );
		a.mov( al, cl );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

TEST_CASE( "CodeAssembler: mov reg, imm", "[assembler][instructions]" ) {
	SECTION( "mov r32, imm32" ) {
		CodeAssembler a( 64 );
		a.mov( eax, 0x12345678 );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "mov r64, imm64" ) {
		CodeAssembler a( 64 );
		a.mov( rax, static_cast<int64_t>( 0x123456789ABCDEF0 ) );
		auto bytes = a.assemble( 0x1000 );
		CHECK( bytes.size() == 10 );  // REX.W + opcode + 8-byte immediate
	}
}

// ============================================================================
// CodeAssembler instruction tests - arithmetic
// ============================================================================

TEST_CASE( "CodeAssembler: xor instructions", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );

	SECTION( "xor r32, r32" ) {
		a.xor_( eax, eax );  // Common idiom to zero a register
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "xor r64, r64" ) {
		a.xor_( rax, rax );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

TEST_CASE( "CodeAssembler: add instructions", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );

	SECTION( "add r64, r64" ) {
		a.add( rax, rcx );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "add r32, imm32" ) {
		a.add( eax, 100 );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

TEST_CASE( "CodeAssembler: sub instructions", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );
	a.sub( rax, rcx );
	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );
}

TEST_CASE( "CodeAssembler: inc/dec instructions", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );

	a.inc( eax );
	a.dec( eax );
	a.inc( rax );
	a.dec( rax );

	CHECK( a.instructions().size() == 4 );
	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );
}

TEST_CASE( "CodeAssembler: cmp instructions", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );

	SECTION( "cmp r64, r64" ) {
		a.cmp( rax, rcx );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "cmp r32, imm32" ) {
		a.cmp( eax, 10 );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

// ============================================================================
// CodeAssembler instruction tests - jumps
// ============================================================================

TEST_CASE( "CodeAssembler: jmp instruction", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );

	auto label = a.create_label();
	a.set_label( label );
	a.nop();
	a.jmp( label );

	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );
}

TEST_CASE( "CodeAssembler: conditional jumps", "[assembler][instructions]" ) {
	CodeAssembler a( 64 );

	auto label = a.create_label();
	a.set_label( label );
	a.nop();

	a.je( label );
	a.jz( label );   // alias for je
	a.jne( label );
	a.jnz( label );  // alias for jne
	a.jl( label );
	a.jb( label );
	a.jle( label );
	a.jbe( label );
	a.jg( label );
	a.ja( label );
	a.jge( label );
	a.jae( label );
	a.js( label );
	a.jns( label );
	a.jp( label );
	a.jnp( label );
	a.jo( label );
	a.jno( label );

	CHECK( a.instructions().size() == 19 );  // 1 nop + 18 jumps
	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );
}

// ============================================================================
// CodeAssembler instruction tests - call
// ============================================================================

TEST_CASE( "CodeAssembler: call instructions", "[assembler][instructions]" ) {
	SECTION( "call label" ) {
		CodeAssembler a( 64 );
		auto label = a.create_label();
		a.set_label( label );
		a.nop();
		a.call( label );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "call r64" ) {
		CodeAssembler a( 64 );
		a.call( rax );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "call r32 (32-bit mode)" ) {
		CodeAssembler a( 32 );
		a.call( eax );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

// ============================================================================
// CodeAssembler instruction tests - lea
// ============================================================================

TEST_CASE( "CodeAssembler: lea instructions", "[assembler][instructions]" ) {
	SECTION( "lea r64, [r64 + disp]" ) {
		CodeAssembler a( 64 );
		a.lea( rax, rsp + 0x10 );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "lea r64, [r64 + r64*scale + disp]" ) {
		CodeAssembler a( 64 );
		a.lea( rax, rbx + rcx * 4 + 0x100 );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}

	SECTION( "lea r32, [r32 + disp]" ) {
		CodeAssembler a( 32 );
		a.lea( eax, esp + 0x10 );
		auto bytes = a.assemble( 0x1000 );
		CHECK( !bytes.empty() );
	}
}

// ============================================================================
// CodeAssembler data declaration tests
// ============================================================================

TEST_CASE( "CodeAssembler: db single byte", "[assembler][data]" ) {
	CodeAssembler a( 64 );
	a.db( 0x90 );
	CHECK( a.instructions().size() == 1 );

	auto bytes = a.assemble( 0x1000 );
	CHECK( bytes.size() == 1 );
	CHECK( bytes[0] == 0x90 );
}

TEST_CASE( "CodeAssembler: dw single word", "[assembler][data]" ) {
	CodeAssembler a( 64 );
	a.dw( static_cast<uint16_t>( 0x1234 ) );
	CHECK( a.instructions().size() == 1 );

	auto bytes = a.assemble( 0x1000 );
	CHECK( bytes.size() == 2 );
}

TEST_CASE( "CodeAssembler: dd single dword", "[assembler][data]" ) {
	CodeAssembler a( 64 );
	a.dd( 0x12345678u );
	CHECK( a.instructions().size() == 1 );

	auto bytes = a.assemble( 0x1000 );
	CHECK( bytes.size() == 4 );
}

TEST_CASE( "CodeAssembler: dq single qword", "[assembler][data]" ) {
	CodeAssembler a( 64 );
	a.dq( 0x123456789ABCDEF0ull );
	CHECK( a.instructions().size() == 1 );

	auto bytes = a.assemble( 0x1000 );
	CHECK( bytes.size() == 8 );
}

// ============================================================================
// CodeAssembler assemble tests
// ============================================================================

TEST_CASE( "CodeAssembler: assemble simple function", "[assembler][assemble]" ) {
	CodeAssembler a( 64 );

	// Simple function: return 42
	a.mov( eax, 42 );
	a.ret();

	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );

	// Decode and verify
	Decoder decoder( 64, bytes, 0x1000 );
	Instruction instr;

	CHECK( decoder.can_decode() );
	instr = decoder.decode().value();
	CHECK( instr.mnemonic() == Mnemonic::MOV );

	CHECK( decoder.can_decode() );
	instr = decoder.decode().value();
	CHECK( instr.mnemonic() == Mnemonic::RET );
}

TEST_CASE( "CodeAssembler: assemble function with loop", "[assembler][assemble]" ) {
	CodeAssembler a( 64 );

	// Function: count from 0 to 9
	a.xor_( eax, eax );        // eax = 0

	auto loop = a.create_label();
	a.set_label( loop );
	a.inc( eax );              // eax++
	a.cmp( eax, 10 );          // compare with 10
	a.jl( loop );              // jump if less

	a.ret();

	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );

	// Verify we can decode all instructions
	Decoder decoder( 64, bytes, 0x1000 );
	int count = 0;
	while ( decoder.can_decode() ) {
		auto instr = decoder.decode().value();
		(void)instr;
		count++;
	}
	CHECK( count == 5 );  // xor, inc, cmp, jl, ret
}

TEST_CASE( "CodeAssembler: assemble with forward jump", "[assembler][assemble]" ) {
	CodeAssembler a( 64 );

	auto skip = a.create_label();
	a.jmp( skip );             // Forward jump
	a.nop();                   // This will be skipped
	a.nop();
	a.set_label( skip );
	a.ret();

	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );
}

TEST_CASE( "CodeAssembler: assemble_options returns metadata", "[assembler][assemble]" ) {
	CodeAssembler a( 64 );
	a.nop();
	a.nop();
	a.ret();

	auto result = a.assemble_options( 0x1000, BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS );
	CHECK( !result.code.empty() );
	CHECK( result.rip == 0x1000 );
	CHECK( result.new_instruction_offsets.size() == 3 );
}

// ============================================================================
// CodeAssembler error handling tests
// ============================================================================

TEST_CASE( "CodeAssembler: assemble throws for unused prefix", "[assembler][errors]" ) {
	CodeAssembler a( 64 );
	a.lock();  // Prefix without instruction
	CHECK_THROWS_AS( a.assemble( 0x1000 ), std::runtime_error );
}

TEST_CASE( "CodeAssembler: assemble throws for unused label", "[assembler][errors]" ) {
	CodeAssembler a( 64 );
	auto label = a.create_label();
	a.set_label( label );  // Label without instruction following
	CHECK_THROWS_AS( a.assemble( 0x1000 ), std::runtime_error );
}

// ============================================================================
// CodeAssembler bitness-specific tests
// ============================================================================

TEST_CASE( "CodeAssembler: 32-bit mode", "[assembler][bitness]" ) {
	CodeAssembler a( 32 );

	a.push( ebp );
	a.mov( ebp, esp );
	a.xor_( eax, eax );
	a.pop( ebp );
	a.ret();

	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );

	// Verify with decoder
	Decoder decoder( 32, bytes, 0x1000 );
	int count = 0;
	while ( decoder.can_decode() ) {
		auto instr = decoder.decode().value();
		(void)instr;
		count++;
	}
	CHECK( count == 5 );
}

TEST_CASE( "CodeAssembler: 16-bit mode", "[assembler][bitness]" ) {
	CodeAssembler a( 16 );

	a.push( bp );
	a.mov( bp, sp );
	a.xor_( ax, ax );
	a.pop( bp );
	a.ret();

	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );

	// Verify with decoder
	Decoder decoder( 16, bytes, 0x1000 );
	int count = 0;
	while ( decoder.can_decode() ) {
		auto instr = decoder.decode().value();
		(void)instr;
		count++;
	}
	CHECK( count == 5 );
}

// ============================================================================
// Integration test - realistic function
// ============================================================================

TEST_CASE( "CodeAssembler: realistic function prologue/epilogue", "[assembler][integration]" ) {
	CodeAssembler a( 64 );

	// Function prologue
	a.push( rbp );
	a.mov( rbp, rsp );
	a.push( rbx );
	a.push( r12 );
	a.push( r13 );

	// Function body - just return 0
	a.xor_( eax, eax );

	// Function epilogue
	a.pop( r13 );
	a.pop( r12 );
	a.pop( rbx );
	a.pop( rbp );
	a.ret();

	auto bytes = a.assemble( 0x1000 );
	CHECK( !bytes.empty() );

	// Decode and verify structure
	Decoder decoder( 64, bytes, 0x1000 );
	std::vector<Mnemonic> mnemonics;
	while ( decoder.can_decode() ) {
		auto instr = decoder.decode().value();
		mnemonics.push_back( instr.mnemonic() );
	}

	REQUIRE( mnemonics.size() == 11 );
	CHECK( mnemonics[0] == Mnemonic::PUSH );
	CHECK( mnemonics[1] == Mnemonic::MOV );
	CHECK( mnemonics[2] == Mnemonic::PUSH );
	CHECK( mnemonics[3] == Mnemonic::PUSH );
	CHECK( mnemonics[4] == Mnemonic::PUSH );
	CHECK( mnemonics[5] == Mnemonic::XOR );
	CHECK( mnemonics[6] == Mnemonic::POP );
	CHECK( mnemonics[7] == Mnemonic::POP );
	CHECK( mnemonics[8] == Mnemonic::POP );
	CHECK( mnemonics[9] == Mnemonic::POP );
	CHECK( mnemonics[10] == Mnemonic::RET );
}
