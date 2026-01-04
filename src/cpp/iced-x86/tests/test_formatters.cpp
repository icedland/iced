// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Comprehensive tests for all formatters (Intel, MASM, NASM, GAS, Fast)

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"
#include <string>
#include <vector>

using namespace iced_x86;

// ============================================================================
// Helper functions
// ============================================================================

static Instruction decode_instruction(uint32_t bitness, const uint8_t* bytes, size_t len, uint64_t ip = 0x1000) {
	Decoder decoder(bitness, std::span<const uint8_t>(bytes, len), ip);
	auto result = decoder.decode();
	return result.value();
}

// ============================================================================
// Intel Formatter Tests
// ============================================================================

TEST_CASE("IntelFormatter: basic formatting", "[formatter][intel]") {
	IntelFormatter formatter;
	
	SECTION("NOP") {
		const uint8_t bytes[] = {0x90};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		CHECK(formatter.format_to_string(instr) == "nop");
	}
	
	SECTION("MOV reg, reg") {
		const uint8_t bytes[] = {0x89, 0xD8};  // MOV EAX, EBX
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		CHECK(formatter.format_to_string(instr) == "mov eax, ebx");
	}
	
	SECTION("MOV reg, imm") {
		const uint8_t bytes[] = {0xB8, 0x78, 0x56, 0x34, 0x12};  // MOV EAX, 0x12345678
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("mov") != std::string::npos);
		CHECK(output.find("eax") != std::string::npos);
	}
	
	SECTION("MOV reg, mem") {
		const uint8_t bytes[] = {0x8B, 0x00};  // MOV EAX, [EAX]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("mov") != std::string::npos);
		CHECK(output.find("[") != std::string::npos);
	}
	
	SECTION("Memory with displacement") {
		const uint8_t bytes[] = {0x8B, 0x40, 0x10};  // MOV EAX, [EAX+0x10]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("[") != std::string::npos);
		CHECK(output.find("+") != std::string::npos);
	}
	
	SECTION("Memory with SIB") {
		const uint8_t bytes[] = {0x8B, 0x04, 0x88};  // MOV EAX, [EAX+ECX*4]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("*4") != std::string::npos);
	}
}

TEST_CASE("IntelFormatter: 64-bit instructions", "[formatter][intel]") {
	IntelFormatter formatter;
	
	SECTION("64-bit registers") {
		const uint8_t bytes[] = {0x48, 0x89, 0xD8};  // MOV RAX, RBX
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("rax") != std::string::npos);
		CHECK(output.find("rbx") != std::string::npos);
	}
	
	SECTION("Extended registers R8-R15") {
		const uint8_t bytes[] = {0x4D, 0x89, 0xC1};  // MOV R9, R8
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("r9") != std::string::npos);
		CHECK(output.find("r8") != std::string::npos);
	}
	
	SECTION("RIP-relative addressing") {
		const uint8_t bytes[] = {0x48, 0x8B, 0x05, 0x00, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("rip") != std::string::npos);
	}
}

TEST_CASE("IntelFormatter: branch instructions", "[formatter][intel]") {
	IntelFormatter formatter;
	
	SECTION("JMP short") {
		const uint8_t bytes[] = {0xEB, 0x10};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("jmp") != std::string::npos);
	}
	
	SECTION("CALL") {
		const uint8_t bytes[] = {0xE8, 0x00, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("call") != std::string::npos);
	}
	
	SECTION("Conditional jump") {
		const uint8_t bytes[] = {0x74, 0x10};  // JE
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK((output.find("je") != std::string::npos || output.find("jz") != std::string::npos));
	}
}

TEST_CASE("IntelFormatter: VEX instructions", "[formatter][intel]") {
	IntelFormatter formatter;
	
	SECTION("VADDPS xmm") {
		const uint8_t bytes[] = {0xC5, 0xE8, 0x58, 0xC2};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("vaddps") != std::string::npos);
		CHECK(output.find("xmm") != std::string::npos);
	}
	
	SECTION("VADDPS ymm") {
		const uint8_t bytes[] = {0xC5, 0xEC, 0x58, 0xC2};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("vaddps") != std::string::npos);
		CHECK(output.find("ymm") != std::string::npos);
	}
}

TEST_CASE("IntelFormatter: EVEX instructions", "[formatter][intel]") {
	IntelFormatter formatter;
	
	SECTION("VMOVDQA32 xmm") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x08, 0x6F, 0xC1};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("vmovdqa32") != std::string::npos);
	}
	
	SECTION("EVEX with mask") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x09, 0x6F, 0xC1};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("vmovdqa32") != std::string::npos);
		CHECK(output.find("{k1}") != std::string::npos);
	}
	
	SECTION("EVEX with zeroing") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7D, 0x89, 0x6F, 0xC1};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("vmovdqa32") != std::string::npos);
		CHECK(output.find("{k1}") != std::string::npos);
		CHECK(output.find("{z}") != std::string::npos);
	}
}

// ============================================================================
// MASM Formatter Tests
// ============================================================================

TEST_CASE("MasmFormatter: basic formatting", "[formatter][masm]") {
	MasmFormatter formatter;
	
	SECTION("MOV reg, reg") {
		const uint8_t bytes[] = {0x89, 0xD8};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("mov") != std::string::npos);
		CHECK(output.find("eax") != std::string::npos);
	}
	
	SECTION("Memory operand shows size") {
		const uint8_t bytes[] = {0x8B, 0x00};  // MOV EAX, [EAX]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		formatter.options().set_show_memory_size(true);
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("dword ptr") != std::string::npos);
	}
}

TEST_CASE("MasmFormatter: segment overrides", "[formatter][masm]") {
	MasmFormatter formatter;
	
	SECTION("FS segment") {
		const uint8_t bytes[] = {0x64, 0x8B, 0x00};  // MOV EAX, FS:[EAX]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("fs:") != std::string::npos);
	}
	
	SECTION("GS segment") {
		const uint8_t bytes[] = {0x65, 0x8B, 0x00};  // MOV EAX, GS:[EAX]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("gs:") != std::string::npos);
	}
}

// ============================================================================
// NASM Formatter Tests
// ============================================================================

TEST_CASE("NasmFormatter: basic formatting", "[formatter][nasm]") {
	NasmFormatter formatter;
	
	SECTION("MOV reg, reg") {
		const uint8_t bytes[] = {0x89, 0xD8};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("mov") != std::string::npos);
		CHECK(output.find("eax") != std::string::npos);
	}
	
	SECTION("Memory operand NASM style") {
		const uint8_t bytes[] = {0x8B, 0x00};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		formatter.options().set_show_memory_size(true);
		std::string output = formatter.format_to_string(instr);
		// NASM uses "dword" not "dword ptr"
		CHECK(output.find("dword") != std::string::npos);
	}
}

TEST_CASE("NasmFormatter: hex formatting", "[formatter][nasm]") {
	NasmFormatter formatter;
	
	SECTION("Default hex format (0x prefix)") {
		const uint8_t bytes[] = {0x05, 0x78, 0x56, 0x34, 0x12};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		formatter.options().set_hex_prefix("0x");
		formatter.options().set_hex_suffix("");
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("0x") != std::string::npos);
	}
	
	SECTION("h suffix format") {
		const uint8_t bytes[] = {0x05, 0x78, 0x56, 0x34, 0x12};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		formatter.options().set_hex_prefix("");
		formatter.options().set_hex_suffix("h");
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("h") != std::string::npos);
	}
}

// ============================================================================
// GAS Formatter Tests
// ============================================================================

TEST_CASE("GasFormatter: basic formatting", "[formatter][gas]") {
	GasFormatter formatter;
	
	SECTION("MOV reg, reg - AT&T syntax") {
		const uint8_t bytes[] = {0x89, 0xD8};  // MOV EAX, EBX
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		// GAS uses AT&T syntax: reversed operands, % prefix
		CHECK(output.find("%") != std::string::npos);
	}
	
	SECTION("Immediate has $ prefix") {
		const uint8_t bytes[] = {0x83, 0xC0, 0x10};  // ADD EAX, 0x10
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("$") != std::string::npos);
	}
	
	SECTION("Memory operand format") {
		const uint8_t bytes[] = {0x8B, 0x40, 0x10};  // MOV EAX, [EAX+0x10]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		// GAS format: displacement(base) -> 0x10(%eax)
		CHECK(output.find("(") != std::string::npos);
		CHECK(output.find(")") != std::string::npos);
	}
}

TEST_CASE("GasFormatter: suffix notation", "[formatter][gas]") {
	GasFormatter formatter;
	
	SECTION("Byte suffix") {
		const uint8_t bytes[] = {0x88, 0xD8};  // MOV AL, BL
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("mov") != std::string::npos);
	}
	
	SECTION("Long suffix") {
		const uint8_t bytes[] = {0x89, 0xD8};  // MOV EAX, EBX
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("mov") != std::string::npos);
	}
}

// ============================================================================
// Fast Formatter Tests
// ============================================================================

TEST_CASE("FastFormatter: basic formatting", "[formatter][fast]") {
	FastFormatter formatter;
	
	SECTION("NOP") {
		const uint8_t bytes[] = {0x90};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		CHECK(formatter.format_to_string(instr) == "nop");
	}
	
	SECTION("MOV reg, reg") {
		const uint8_t bytes[] = {0x89, 0xD8};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("mov") != std::string::npos);
	}
	
	SECTION("Memory operand") {
		const uint8_t bytes[] = {0x8B, 0x00};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("[") != std::string::npos);
	}
}

TEST_CASE("FastFormatter: options", "[formatter][fast]") {
	FastFormatter formatter;
	
	SECTION("Uppercase hex") {
		const uint8_t bytes[] = {0xB8, 0xAB, 0xCD, 0xEF, 0x12};  // MOV EAX, 0x12EFCDAB
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		
		formatter.options().set_uppercase_hex(true);
		std::string output1 = formatter.format_to_string(instr);
		CHECK((output1.find("A") != std::string::npos || output1.find("B") != std::string::npos));
		
		formatter.options().set_uppercase_hex(false);
		std::string output2 = formatter.format_to_string(instr);
		CHECK((output2.find("a") != std::string::npos || output2.find("b") != std::string::npos));
	}
	
	SECTION("Space after operand separator") {
		const uint8_t bytes[] = {0x89, 0xD8};
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		
		formatter.options().set_space_after_operand_separator(false);
		std::string output1 = formatter.format_to_string(instr);
		
		formatter.options().set_space_after_operand_separator(true);
		std::string output2 = formatter.format_to_string(instr);
		
		// With space should be longer
		CHECK(output2.length() >= output1.length());
	}
}

// ============================================================================
// Formatter Options Consistency Tests
// ============================================================================

TEST_CASE("Formatter options: consistent across formatters", "[formatter][options]") {
	const uint8_t bytes[] = {0x89, 0xD8};  // MOV EAX, EBX
	auto instr = decode_instruction(32, bytes, sizeof(bytes));
	
	SECTION("Uppercase mnemonics") {
		IntelFormatter intel;
		MasmFormatter masm;
		NasmFormatter nasm;
		
		intel.options().set_uppercase_mnemonics(true);
		masm.options().set_uppercase_mnemonics(true);
		nasm.options().set_uppercase_mnemonics(true);
		
		CHECK(intel.format_to_string(instr).find("MOV") != std::string::npos);
		CHECK(masm.format_to_string(instr).find("MOV") != std::string::npos);
		CHECK(nasm.format_to_string(instr).find("MOV") != std::string::npos);
	}
	
	SECTION("Uppercase registers") {
		IntelFormatter intel;
		MasmFormatter masm;
		NasmFormatter nasm;
		
		intel.options().set_uppercase_registers(true);
		masm.options().set_uppercase_registers(true);
		nasm.options().set_uppercase_registers(true);
		
		CHECK(intel.format_to_string(instr).find("EAX") != std::string::npos);
		CHECK(masm.format_to_string(instr).find("EAX") != std::string::npos);
		CHECK(nasm.format_to_string(instr).find("EAX") != std::string::npos);
	}
}

// ============================================================================
// Prefix Formatting Tests
// ============================================================================

TEST_CASE("Formatters: prefix handling", "[formatter][prefix]") {
	SECTION("LOCK prefix") {
		const uint8_t bytes[] = {0xF0, 0x01, 0x00};  // LOCK ADD [EAX], EAX
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		
		IntelFormatter intel;
		std::string output = intel.format_to_string(instr);
		CHECK(output.find("lock") != std::string::npos);
	}
	
	SECTION("REP prefix") {
		const uint8_t bytes[] = {0xF3, 0xA4};  // REP MOVSB
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		
		IntelFormatter intel;
		std::string output = intel.format_to_string(instr);
		CHECK(output.find("rep") != std::string::npos);
	}
	
	SECTION("REPNE prefix") {
		const uint8_t bytes[] = {0xF2, 0xAE};  // REPNE SCASB
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		
		IntelFormatter intel;
		std::string output = intel.format_to_string(instr);
		CHECK(output.find("repne") != std::string::npos);
	}
}

// ============================================================================
// Special Instructions Formatting
// ============================================================================

TEST_CASE("Formatters: special instructions", "[formatter][special]") {
	IntelFormatter formatter;
	
	SECTION("RET") {
		const uint8_t bytes[] = {0xC3};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		CHECK(formatter.format_to_string(instr) == "ret");
	}
	
	SECTION("INT 3") {
		const uint8_t bytes[] = {0xCC};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("int") != std::string::npos);
	}
	
	SECTION("CPUID") {
		const uint8_t bytes[] = {0x0F, 0xA2};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		CHECK(formatter.format_to_string(instr) == "cpuid");
	}
	
	SECTION("RDTSC") {
		const uint8_t bytes[] = {0x0F, 0x31};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		CHECK(formatter.format_to_string(instr) == "rdtsc");
	}
}

// ============================================================================
// FPU Instructions Formatting
// ============================================================================

TEST_CASE("Formatters: FPU instructions", "[formatter][fpu]") {
	IntelFormatter formatter;
	
	SECTION("FLD ST(i)") {
		const uint8_t bytes[] = {0xD9, 0xC1};  // FLD ST(1)
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("fld") != std::string::npos);
		CHECK(output.find("st") != std::string::npos);
	}
	
	SECTION("FADD") {
		const uint8_t bytes[] = {0xD8, 0xC1};  // FADD ST, ST(1)
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("fadd") != std::string::npos);
	}
}

// ============================================================================
// SSE/AVX Register Formatting
// ============================================================================

TEST_CASE("Formatters: SIMD registers", "[formatter][simd]") {
	IntelFormatter formatter;
	
	SECTION("XMM registers") {
		const uint8_t bytes[] = {0x0F, 0x28, 0xC1};  // MOVAPS XMM0, XMM1
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("xmm0") != std::string::npos);
		CHECK(output.find("xmm1") != std::string::npos);
	}
	
	SECTION("YMM registers") {
		const uint8_t bytes[] = {0xC5, 0xFC, 0x28, 0xC1};  // VMOVAPS YMM0, YMM1
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("ymm0") != std::string::npos);
		CHECK(output.find("ymm1") != std::string::npos);
	}
	
	SECTION("ZMM registers") {
		const uint8_t bytes[] = {0x62, 0xF1, 0x7C, 0x48, 0x28, 0xC1};  // VMOVAPS ZMM0, ZMM1
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("zmm0") != std::string::npos);
		CHECK(output.find("zmm1") != std::string::npos);
	}
	
	SECTION("Extended XMM registers (XMM8-XMM15)") {
		const uint8_t bytes[] = {0x45, 0x0F, 0x28, 0xC1};  // MOVAPS XMM8, XMM9
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("xmm8") != std::string::npos);
		CHECK(output.find("xmm9") != std::string::npos);
	}
}

// ============================================================================
// Immediate Value Formatting
// ============================================================================

TEST_CASE("Formatters: immediate values", "[formatter][immediate]") {
	IntelFormatter formatter;
	
	SECTION("Small decimal numbers") {
		const uint8_t bytes[] = {0x83, 0xC0, 0x05};  // ADD EAX, 5
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		
		formatter.options().set_small_hex_numbers_in_decimal(true);
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("5") != std::string::npos);
	}
	
	SECTION("Signed negative displacement") {
		const uint8_t bytes[] = {0x8B, 0x40, 0xF0};  // MOV EAX, [EAX-0x10]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		// Should show negative displacement
		CHECK(output.find("-") != std::string::npos);
	}
}

// ============================================================================
// Multiple Operand Formatting
// ============================================================================

TEST_CASE("Formatters: multi-operand instructions", "[formatter][multiop]") {
	IntelFormatter formatter;
	
	SECTION("IMUL r32, r/m32, imm8") {
		const uint8_t bytes[] = {0x6B, 0xC1, 0x10};  // IMUL EAX, ECX, 0x10
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("imul") != std::string::npos);
	}
	
	SECTION("SHLD r32, r32, imm8") {
		const uint8_t bytes[] = {0x0F, 0xA4, 0xC1, 0x08};  // SHLD ECX, EAX, 8
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("shld") != std::string::npos);
	}
	
	SECTION("VEX 3-operand") {
		const uint8_t bytes[] = {0xC5, 0xE8, 0x58, 0xC2};  // VADDPS XMM0, XMM2, XMM2
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		// Should have 3 operands
		size_t comma_count = 0;
		for (char c : output) {
			if (c == ',') comma_count++;
		}
		CHECK(comma_count == 2);
	}
}

// ============================================================================
// Formatter Output to StringOutput
// ============================================================================

TEST_CASE("Formatters: StringFormatterOutput interface", "[formatter][output]") {
	IntelFormatter formatter;
	const uint8_t bytes[] = {0x89, 0xD8};
	auto instr = decode_instruction(32, bytes, sizeof(bytes));
	
	SECTION("format method") {
		std::string str;
		StringFormatterOutput output(str);
		formatter.format(instr, output);
		CHECK(str.find("mov") != std::string::npos);
	}
	
	SECTION("Multiple format calls append") {
		std::string str;
		StringFormatterOutput output(str);
		formatter.format(instr, output);
		output.write("; ", FormatterTextKind::TEXT);
		formatter.format(instr, output);
		
		// Should have two "mov" occurrences
		size_t first = str.find("mov");
		size_t second = str.find("mov", first + 1);
		CHECK(first != std::string::npos);
		CHECK(second != std::string::npos);
	}
}

// ============================================================================
// Edge Cases
// ============================================================================

TEST_CASE("Formatters: edge cases", "[formatter][edge]") {
	IntelFormatter formatter;
	
	SECTION("Very long displacement") {
		const uint8_t bytes[] = {0x8B, 0x80, 0xFF, 0xFF, 0xFF, 0x7F};  // MOV EAX, [EAX+0x7FFFFFFF]
		auto instr = decode_instruction(32, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(!output.empty());
	}
	
	SECTION("All extended registers") {
		const uint8_t bytes[] = {0x4D, 0x8D, 0xBC, 0xFD, 0x00, 0x00, 0x00, 0x00};  
		// LEA R15, [R13+R15*8]
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("r15") != std::string::npos);
		CHECK(output.find("r13") != std::string::npos);
	}
}
