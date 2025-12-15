// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// Tests for the SymbolResolver functionality with all formatters

#include <catch2/catch_test_macros.hpp>
#include "iced_x86/iced_x86.hpp"
#include <string>
#include <unordered_map>
#include <optional>

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
// Custom Symbol Resolver for Testing
// ============================================================================

class TestSymbolResolver : public SymbolResolver {
public:
	std::unordered_map<uint64_t, std::string> symbols;
	
	void add_symbol(uint64_t address, const std::string& name) {
		symbols[address] = name;
	}
	
	[[nodiscard]] std::optional<SymbolResult> try_get_symbol(
			const Instruction& /*instruction*/,
			int /*operand*/,
			int /*instruction_operand*/,
			uint64_t address,
			int /*address_size*/) override {
		auto it = symbols.find(address);
		if (it != symbols.end()) {
			return SymbolResult(address, it->second);
		}
		return std::nullopt;
	}
};

// ============================================================================
// SymbolFlags Tests
// ============================================================================

TEST_CASE("SymbolFlags: bitwise operations", "[symbol_resolver][flags]") {
	SECTION("OR operation") {
		auto flags = SymbolFlags::RELATIVE | SymbolFlags::SIGNED;
		CHECK(has_flag(flags, SymbolFlags::RELATIVE));
		CHECK(has_flag(flags, SymbolFlags::SIGNED));
		CHECK_FALSE(has_flag(flags, SymbolFlags::HAS_SYMBOL_SIZE));
	}
	
	SECTION("AND operation") {
		auto flags = SymbolFlags::RELATIVE | SymbolFlags::SIGNED;
		auto result = flags & SymbolFlags::RELATIVE;
		CHECK(has_flag(result, SymbolFlags::RELATIVE));
		CHECK_FALSE(has_flag(result, SymbolFlags::SIGNED));
	}
	
	SECTION("NOT operation") {
		auto flags = ~SymbolFlags::RELATIVE;
		CHECK_FALSE(has_flag(flags, SymbolFlags::RELATIVE));
	}
	
	SECTION("NONE has no flags") {
		CHECK_FALSE(has_flag(SymbolFlags::NONE, SymbolFlags::RELATIVE));
		CHECK_FALSE(has_flag(SymbolFlags::NONE, SymbolFlags::SIGNED));
		CHECK_FALSE(has_flag(SymbolFlags::NONE, SymbolFlags::HAS_SYMBOL_SIZE));
	}
}

// ============================================================================
// TextPart Tests
// ============================================================================

TEST_CASE("TextPart: construction", "[symbol_resolver][textpart]") {
	SECTION("Default constructor") {
		TextPart part;
		CHECK(part.text.empty());
		CHECK(part.kind == FormatterTextKind::TEXT);
	}
	
	SECTION("String constructor") {
		TextPart part("my_symbol", FormatterTextKind::LABEL);
		CHECK(part.text == "my_symbol");
		CHECK(part.kind == FormatterTextKind::LABEL);
	}
	
	SECTION("String_view constructor") {
		std::string_view sv = "test_symbol";
		TextPart part(sv, FormatterTextKind::FUNCTION);
		CHECK(part.text == "test_symbol");
		CHECK(part.kind == FormatterTextKind::FUNCTION);
	}
	
	SECTION("C string constructor") {
		TextPart part("c_string_symbol");
		CHECK(part.text == "c_string_symbol");
		CHECK(part.kind == FormatterTextKind::TEXT);
	}
}

// ============================================================================
// TextInfo Tests
// ============================================================================

TEST_CASE("TextInfo: construction", "[symbol_resolver][textinfo]") {
	SECTION("Default constructor") {
		TextInfo info;
		CHECK(info.is_default());
		CHECK_FALSE(info.has_parts());
	}
	
	SECTION("String constructor") {
		TextInfo info("my_function");
		CHECK_FALSE(info.is_default());
		CHECK_FALSE(info.has_parts());
		CHECK(info.text.text == "my_function");
		CHECK(info.text.kind == FormatterTextKind::LABEL);
	}
	
	SECTION("String with kind constructor") {
		TextInfo info("my_func", FormatterTextKind::FUNCTION);
		CHECK(info.text.text == "my_func");
		CHECK(info.text.kind == FormatterTextKind::FUNCTION);
	}
	
	SECTION("Multiple parts constructor") {
		std::vector<TextPart> parts;
		parts.emplace_back("module", FormatterTextKind::TEXT);
		parts.emplace_back("::", FormatterTextKind::PUNCTUATION);
		parts.emplace_back("function", FormatterTextKind::FUNCTION);
		
		TextInfo info(parts);
		CHECK(info.has_parts());
		CHECK(info.parts.size() == 3);
		CHECK(info.parts[0].text == "module");
		CHECK(info.parts[1].text == "::");
		CHECK(info.parts[2].text == "function");
	}
	
	SECTION("TextPart constructor") {
		TextPart part("symbol", FormatterTextKind::LABEL);
		TextInfo info(part);
		CHECK_FALSE(info.is_default());
		CHECK_FALSE(info.has_parts());
		CHECK(info.text.text == "symbol");
	}
}

// ============================================================================
// SymbolResult Tests
// ============================================================================

TEST_CASE("SymbolResult: construction", "[symbol_resolver][symbolresult]") {
	SECTION("Default constructor") {
		SymbolResult result;
		CHECK(result.address == 0);
		CHECK(result.text.is_default());
		CHECK(result.flags == SymbolFlags::NONE);
		CHECK(result.symbol_size == MemorySize::UNKNOWN);
		CHECK_FALSE(result.has_symbol_size());
	}
	
	SECTION("Address and text constructor") {
		SymbolResult result(0x1234, std::string("my_symbol"));
		CHECK(result.address == 0x1234);
		CHECK(result.text.text.text == "my_symbol");
		CHECK(result.flags == SymbolFlags::NONE);
	}
	
	SECTION("Address, text, and size constructor") {
		SymbolResult result(0x1234, std::string("my_var"), MemorySize::UINT32);
		CHECK(result.address == 0x1234);
		CHECK(result.text.text.text == "my_var");
		CHECK(result.has_symbol_size());
		CHECK(result.symbol_size == MemorySize::UINT32);
	}
	
	SECTION("Address, text, and kind constructor") {
		SymbolResult result(0x1234, "my_func", FormatterTextKind::FUNCTION);
		CHECK(result.address == 0x1234);
		CHECK(result.text.text.text == "my_func");
		CHECK(result.text.text.kind == FormatterTextKind::FUNCTION);
	}
	
	SECTION("Address, text, kind, and flags constructor") {
		SymbolResult result(0x1234, "offset", FormatterTextKind::LABEL, SymbolFlags::RELATIVE);
		CHECK(result.address == 0x1234);
		CHECK(result.is_relative());
		CHECK_FALSE(result.is_signed());
	}
	
	SECTION("Check helper methods") {
		SymbolResult result1(0x1234, std::string("sym1"));
		CHECK_FALSE(result1.is_relative());
		CHECK_FALSE(result1.is_signed());
		CHECK_FALSE(result1.has_symbol_size());
		
		SymbolResult result2(0x1234, TextInfo("sym2"), SymbolFlags::RELATIVE | SymbolFlags::SIGNED);
		CHECK(result2.is_relative());
		CHECK(result2.is_signed());
	}
}

// ============================================================================
// FunctionSymbolResolver Tests
// ============================================================================

static std::optional<SymbolResult> simple_symbol_callback(uint64_t address) {
	if (address == 0x1000) return SymbolResult(address, std::string("start"));
	if (address == 0x2000) return SymbolResult(address, std::string("main"));
	if (address == 0x3000) return SymbolResult(address, std::string("end"));
	return std::nullopt;
}

TEST_CASE("FunctionSymbolResolver: simple callback", "[symbol_resolver][function]") {
	FunctionSymbolResolver resolver(simple_symbol_callback);
	
	// Create a dummy instruction for the API
	const uint8_t bytes[] = {0x90};
	auto instr = decode_instruction(64, bytes, sizeof(bytes));
	
	SECTION("Resolve known symbol") {
		auto result = resolver.try_get_symbol(instr, 0, 0, 0x1000, 8);
		REQUIRE(result.has_value());
		CHECK(result->address == 0x1000);
		CHECK(result->text.text.text == "start");
	}
	
	SECTION("Resolve another known symbol") {
		auto result = resolver.try_get_symbol(instr, 0, 0, 0x2000, 8);
		REQUIRE(result.has_value());
		CHECK(result->text.text.text == "main");
	}
	
	SECTION("Unknown address returns nullopt") {
		auto result = resolver.try_get_symbol(instr, 0, 0, 0x9999, 8);
		CHECK_FALSE(result.has_value());
	}
}

static std::optional<SymbolResult> full_symbol_callback(
		const Instruction& /*instruction*/,
		int operand,
		int /*instruction_operand*/,
		uint64_t address,
		int address_size) {
	// Use operand and address_size to create different symbols
	if (address == 0x1000) {
		std::string name = "sym_op" + std::to_string(operand) + "_size" + std::to_string(address_size);
		return SymbolResult(address, std::move(name));
	}
	return std::nullopt;
}

TEST_CASE("FunctionSymbolResolver: full callback", "[symbol_resolver][function]") {
	FunctionSymbolResolver resolver(full_symbol_callback);
	
	const uint8_t bytes[] = {0x90};
	auto instr = decode_instruction(64, bytes, sizeof(bytes));
	
	SECTION("Full callback receives all parameters") {
		auto result = resolver.try_get_symbol(instr, 2, 1, 0x1000, 4);
		REQUIRE(result.has_value());
		CHECK(result->text.text.text == "sym_op2_size4");
	}
	
	SECTION("Different operand number") {
		auto result = resolver.try_get_symbol(instr, 0, 0, 0x1000, 8);
		REQUIRE(result.has_value());
		CHECK(result->text.text.text == "sym_op0_size8");
	}
}

// ============================================================================
// IntelFormatter with Symbol Resolver Tests
// ============================================================================

TEST_CASE("IntelFormatter: symbol resolver integration", "[symbol_resolver][intel]") {
	TestSymbolResolver resolver;
	resolver.add_symbol(0x2012, "target_function");
	resolver.add_symbol(0x5000, "my_data");
	
	IntelFormatter formatter(&resolver);
	
	SECTION("CALL with symbol") {
		// CALL rel32 - E8 xx xx xx xx
		// At IP 0x1000, call to 0x2012 needs offset 0x100D (0x2012 - 0x1000 - 5 = 0x100D)
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("target_function") != std::string::npos);
	}
	
	SECTION("JMP with symbol") {
		// JMP rel32 - E9 xx xx xx xx
		// At IP 0x1000, jump to 0x2012 needs offset 0x100D
		const uint8_t bytes[] = {0xE9, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("target_function") != std::string::npos);
	}
	
	SECTION("Instruction without matching symbol shows address") {
		// CALL to address without symbol
		const uint8_t bytes[] = {0xE8, 0x00, 0x00, 0x00, 0x00};  // CALL next instruction
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x9000);
		
		std::string output = formatter.format_to_string(instr);
		// Should not contain our symbols
		CHECK(output.find("target_function") == std::string::npos);
		CHECK(output.find("my_data") == std::string::npos);
	}
}

TEST_CASE("IntelFormatter: set_symbol_resolver", "[symbol_resolver][intel]") {
	IntelFormatter formatter;
	
	// Initially no resolver
	CHECK(formatter.symbol_resolver() == nullptr);
	
	TestSymbolResolver resolver;
	resolver.add_symbol(0x2012, "my_symbol");
	
	// Set resolver
	formatter.set_symbol_resolver(&resolver);
	CHECK(formatter.symbol_resolver() == &resolver);
	
	// Format with resolver
	const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
	auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
	std::string output = formatter.format_to_string(instr);
	CHECK(output.find("my_symbol") != std::string::npos);
	
	// Clear resolver
	formatter.set_symbol_resolver(nullptr);
	CHECK(formatter.symbol_resolver() == nullptr);
}

// ============================================================================
// MasmFormatter with Symbol Resolver Tests
// ============================================================================

TEST_CASE("MasmFormatter: symbol resolver integration", "[symbol_resolver][masm]") {
	TestSymbolResolver resolver;
	resolver.add_symbol(0x2012, "MyProcedure");
	
	MasmFormatter formatter(&resolver);
	
	SECTION("CALL with symbol") {
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("MyProcedure") != std::string::npos);
	}
	
	SECTION("set_symbol_resolver") {
		MasmFormatter fmt;
		CHECK(fmt.symbol_resolver() == nullptr);
		
		fmt.set_symbol_resolver(&resolver);
		CHECK(fmt.symbol_resolver() == &resolver);
	}
}

// ============================================================================
// NasmFormatter with Symbol Resolver Tests
// ============================================================================

TEST_CASE("NasmFormatter: symbol resolver integration", "[symbol_resolver][nasm]") {
	TestSymbolResolver resolver;
	resolver.add_symbol(0x2012, "_start");
	
	NasmFormatter formatter(&resolver);
	
	SECTION("CALL with symbol") {
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("_start") != std::string::npos);
	}
	
	SECTION("set_symbol_resolver") {
		NasmFormatter fmt;
		CHECK(fmt.symbol_resolver() == nullptr);
		
		fmt.set_symbol_resolver(&resolver);
		CHECK(fmt.symbol_resolver() == &resolver);
	}
}

// ============================================================================
// GasFormatter with Symbol Resolver Tests
// ============================================================================

TEST_CASE("GasFormatter: symbol resolver integration", "[symbol_resolver][gas]") {
	TestSymbolResolver resolver;
	resolver.add_symbol(0x2012, "function_entry");
	
	GasFormatter formatter(&resolver);
	
	SECTION("CALL with symbol") {
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("function_entry") != std::string::npos);
	}
	
	SECTION("set_symbol_resolver") {
		GasFormatter fmt;
		CHECK(fmt.symbol_resolver() == nullptr);
		
		fmt.set_symbol_resolver(&resolver);
		CHECK(fmt.symbol_resolver() == &resolver);
	}
}

// ============================================================================
// FastFormatter with Symbol Resolver Tests
// ============================================================================

TEST_CASE("FastFormatter: symbol resolver integration", "[symbol_resolver][fast]") {
	TestSymbolResolver resolver;
	resolver.add_symbol(0x2012, "fast_target");
	
	FastFormatter formatter(&resolver);
	
	SECTION("CALL with symbol") {
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find("fast_target") != std::string::npos);
	}
	
	SECTION("set_symbol_resolver") {
		FastFormatter fmt;
		CHECK(fmt.symbol_resolver() == nullptr);
		
		fmt.set_symbol_resolver(&resolver);
		CHECK(fmt.symbol_resolver() == &resolver);
	}
}

// ============================================================================
// All Formatters: Consistent Symbol Resolution
// ============================================================================

TEST_CASE("All formatters: consistent symbol resolution", "[symbol_resolver][all]") {
	TestSymbolResolver resolver;
	resolver.add_symbol(0x2012, "common_symbol");
	
	const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
	auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
	
	IntelFormatter intel(&resolver);
	MasmFormatter masm(&resolver);
	NasmFormatter nasm(&resolver);
	GasFormatter gas(&resolver);
	FastFormatter fast(&resolver);
	
	std::string intel_out = intel.format_to_string(instr);
	std::string masm_out = masm.format_to_string(instr);
	std::string nasm_out = nasm.format_to_string(instr);
	std::string gas_out = gas.format_to_string(instr);
	std::string fast_out = fast.format_to_string(instr);
	
	// All formatters should include the symbol
	CHECK(intel_out.find("common_symbol") != std::string::npos);
	CHECK(masm_out.find("common_symbol") != std::string::npos);
	CHECK(nasm_out.find("common_symbol") != std::string::npos);
	CHECK(gas_out.find("common_symbol") != std::string::npos);
	CHECK(fast_out.find("common_symbol") != std::string::npos);
}

// ============================================================================
// Constructor Variants Tests
// ============================================================================

TEST_CASE("Formatter constructors with symbol resolver", "[symbol_resolver][constructors]") {
	TestSymbolResolver resolver;
	
	SECTION("IntelFormatter constructors") {
		// Default
		IntelFormatter f1;
		CHECK(f1.symbol_resolver() == nullptr);
		
		// With resolver only
		IntelFormatter f2(&resolver);
		CHECK(f2.symbol_resolver() == &resolver);
		
		// With options and resolver
		FormatterOptions opts;
		IntelFormatter f3(opts, &resolver);
		CHECK(f3.symbol_resolver() == &resolver);
	}
	
	SECTION("MasmFormatter constructors") {
		MasmFormatter f1;
		CHECK(f1.symbol_resolver() == nullptr);
		
		MasmFormatter f2(&resolver);
		CHECK(f2.symbol_resolver() == &resolver);
		
		FormatterOptions opts;
		MasmFormatter f3(opts, &resolver);
		CHECK(f3.symbol_resolver() == &resolver);
	}
	
	SECTION("NasmFormatter constructors") {
		NasmFormatter f1;
		CHECK(f1.symbol_resolver() == nullptr);
		
		NasmFormatter f2(&resolver);
		CHECK(f2.symbol_resolver() == &resolver);
		
		FormatterOptions opts;
		NasmFormatter f3(opts, &resolver);
		CHECK(f3.symbol_resolver() == &resolver);
	}
	
	SECTION("GasFormatter constructors") {
		GasFormatter f1;
		CHECK(f1.symbol_resolver() == nullptr);
		
		GasFormatter f2(&resolver);
		CHECK(f2.symbol_resolver() == &resolver);
		
		FormatterOptions opts;
		GasFormatter f3(opts, &resolver);
		CHECK(f3.symbol_resolver() == &resolver);
	}
	
	SECTION("FastFormatter constructors") {
		FastFormatter f1;
		CHECK(f1.symbol_resolver() == nullptr);
		
		FastFormatter f2(&resolver);
		CHECK(f2.symbol_resolver() == &resolver);
		
		FastFormatterOptions opts;
		FastFormatter f3(opts, &resolver);
		CHECK(f3.symbol_resolver() == &resolver);
	}
}

// ============================================================================
// Edge Cases
// ============================================================================

TEST_CASE("Symbol resolver: edge cases", "[symbol_resolver][edge]") {
	SECTION("Empty symbol name") {
		TestSymbolResolver resolver;
		resolver.add_symbol(0x2012, "");
		
		IntelFormatter formatter(&resolver);
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		// Should not crash
		std::string output = formatter.format_to_string(instr);
		CHECK(!output.empty());
	}
	
	SECTION("Symbol at address 0") {
		TestSymbolResolver resolver;
		resolver.add_symbol(0, "null_symbol");
		
		// This is a contrived test - just verify no crash
		const uint8_t bytes[] = {0x90};
		auto instr = decode_instruction(64, bytes, sizeof(bytes));
		
		IntelFormatter formatter(&resolver);
		std::string output = formatter.format_to_string(instr);
		CHECK(!output.empty());
	}
	
	SECTION("Very long symbol name") {
		TestSymbolResolver resolver;
		std::string long_name(1000, 'x');
		resolver.add_symbol(0x2012, long_name);
		
		IntelFormatter formatter(&resolver);
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(output.find(long_name) != std::string::npos);
	}
	
	SECTION("Unicode in symbol name") {
		TestSymbolResolver resolver;
		resolver.add_symbol(0x2012, "func_\xC3\xA9\xC3\xA0\xC3\xB9");  // UTF-8
		
		IntelFormatter formatter(&resolver);
		const uint8_t bytes[] = {0xE8, 0x0D, 0x10, 0x00, 0x00};
		auto instr = decode_instruction(64, bytes, sizeof(bytes), 0x1000);
		
		std::string output = formatter.format_to_string(instr);
		CHECK(!output.empty());
	}
}
