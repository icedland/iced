// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.IO;
using Generator.Documentation.Cpp;
using Generator.IO;

namespace Generator.Decoder.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppInstructionGenerator {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;
		readonly CppDocCommentWriter docWriter;

		public CppInstructionGenerator( GeneratorContext generatorContext ) {
			genTypes = generatorContext.Types;
			idConverter = CppIdentifierConverter.Create();
			docWriter = new CppDocCommentWriter( idConverter );
		}

		public void Generate() {
			GenerateInstructionHeader();
			GenerateInstructionSource();
		}

		void GenerateInstructionHeader() {
			var filename = CppConstants.GetHeaderFilename( genTypes, "instruction.hpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard( "INSTRUCTION" );

			writer.WriteLine( "#pragma once" );
			writer.WriteLine( $"#ifndef {headerGuard}" );
			writer.WriteLine( $"#define {headerGuard}" );
			writer.WriteLine();

			// Includes
			writer.WriteLine( "#include \"iced_x86/code.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/register.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/op_kind.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/mnemonic.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/memory_size.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/code_size.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/rounding_control.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/mvex_reg_mem_conv.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "#include <cstdint>" );
			writer.WriteLine( "#include <cstddef>" );
			writer.WriteLine( "#include <array>" );
			writer.WriteLine();

			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine();

			WriteInstructionStruct( writer );

			writer.WriteLine();
			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			writer.WriteLine();
			writer.WriteLine( $"#endif // {headerGuard}" );
		}

		void WriteInstructionStruct( FileWriter writer ) {
			writer.WriteLine( "/// @brief A decoded x86/x64 instruction." );
			writer.WriteLine( "///" );
			writer.WriteLine( "/// @details This struct contains all information about a decoded instruction." );
			writer.WriteLine( "/// It is designed to match the Rust implementation layout (40 bytes)." );
			writer.WriteLine( "struct Instruction {" );

			using ( writer.Indent() ) {
				// Private fields
				writer.WriteLine( "// Internal fields - do not access directly" );
				writer.WriteLine( "uint64_t next_rip_ = 0;           ///< @private Address of next instruction" );
				writer.WriteLine( "uint64_t mem_displ_ = 0;          ///< @private Memory displacement / immediate64 high / branch target" );
				writer.WriteLine( "uint32_t flags1_ = 0;             ///< @private InstrFlags1 bitfield" );
				writer.WriteLine( "uint32_t immediate_ = 0;          ///< @private Immediate value / far branch offset" );
				writer.WriteLine( "Code code_ = Code::INVALID;       ///< @private Instruction code" );
				writer.WriteLine( "Register mem_base_reg_ = Register::NONE;  ///< @private Memory base register" );
				writer.WriteLine( "Register mem_index_reg_ = Register::NONE; ///< @private Memory index register" );
				writer.WriteLine( "std::array< Register, 4 > regs_ = {};     ///< @private Operand registers" );
				writer.WriteLine( "std::array< OpKind, 4 > op_kinds_ = {};   ///< @private Operand kinds" );
				writer.WriteLine( "uint8_t scale_ = 0;               ///< @private Memory index scale (0-3 = 1/2/4/8)" );
				writer.WriteLine( "uint8_t displ_size_ = 0;          ///< @private Displacement size encoding" );
				writer.WriteLine( "uint8_t len_ = 0;                 ///< @private Instruction length (0-15)" );
				writer.WriteLine( "uint8_t pad_ = 0;                 ///< @private Padding" );
				writer.WriteLine();

				writer.WriteLine( "public:" );
				writer.WriteLine( "/// @brief Default constructor - creates an invalid instruction." );
				writer.WriteLine( "constexpr Instruction() noexcept = default;" );
				writer.WriteLine();

				// Basic accessors
				WriteCodeAccessors( writer );
				WriteIpAccessors( writer );
				WriteOperandAccessors( writer );
				WriteMemoryAccessors( writer );
				WriteImmediateAccessors( writer );
				WriteBranchAccessors( writer );
				WritePrefixAccessors( writer );
				WriteMiscAccessors( writer );
				WriteDeclareDataAccessors( writer );
				WriteMvexAccessors( writer );
			}

			writer.WriteLine( "};" );
			writer.WriteLine();
			writer.WriteLine( "static_assert( sizeof( Instruction ) == 40, \"Instruction size mismatch with Rust implementation\" );" );
		}

		void WriteCodeAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Code and Mnemonic ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the instruction code." );
			writer.WriteLine( "[[nodiscard]] constexpr Code code() const noexcept { return code_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the instruction code." );
			writer.WriteLine( "constexpr void set_code( Code value ) noexcept { code_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the mnemonic." );
			writer.WriteLine( "[[nodiscard]] Mnemonic mnemonic() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if this is an invalid instruction." );
			writer.WriteLine( "[[nodiscard]] constexpr bool is_invalid() const noexcept { return code_ == Code::INVALID; }" );
			writer.WriteLine();
		}

		void WriteIpAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Instruction Pointer ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 16-bit IP of this instruction." );
			writer.WriteLine( "[[nodiscard]] constexpr uint16_t ip16() const noexcept { return static_cast< uint16_t >( next_rip_ - len_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 32-bit IP of this instruction." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t ip32() const noexcept { return static_cast< uint32_t >( next_rip_ - len_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 64-bit IP of this instruction." );
			writer.WriteLine( "[[nodiscard]] constexpr uint64_t ip() const noexcept { return next_rip_ - len_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 64-bit IP of this instruction." );
			writer.WriteLine( "constexpr void set_ip( uint64_t value ) noexcept { next_rip_ = value + len_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 16-bit IP of the next instruction." );
			writer.WriteLine( "[[nodiscard]] constexpr uint16_t next_ip16() const noexcept { return static_cast< uint16_t >( next_rip_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 32-bit IP of the next instruction." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t next_ip32() const noexcept { return static_cast< uint32_t >( next_rip_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 64-bit IP of the next instruction." );
			writer.WriteLine( "[[nodiscard]] constexpr uint64_t next_ip() const noexcept { return next_rip_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 64-bit IP of the next instruction." );
			writer.WriteLine( "constexpr void set_next_ip( uint64_t value ) noexcept { next_rip_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the instruction length in bytes (1-15)." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t length() const noexcept { return len_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the instruction length in bytes." );
			writer.WriteLine( "constexpr void set_length( uint32_t value ) noexcept { len_ = static_cast< uint8_t >( value ); }" );
			writer.WriteLine();
		}

		void WriteOperandAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Operand Access ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the number of operands." );
			writer.WriteLine( "[[nodiscard]] uint32_t op_count() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the operand kind for the specified operand." );
			writer.WriteLine( "/// @param operand Operand index (0-4)" );
			writer.WriteLine( "[[nodiscard]] OpKind op_kind( uint32_t operand ) const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the operand kind for the specified operand." );
			writer.WriteLine( "void set_op_kind( uint32_t operand, OpKind kind ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the register for the specified operand." );
			writer.WriteLine( "/// @param operand Operand index (0-4)" );
			writer.WriteLine( "[[nodiscard]] Register op_register( uint32_t operand ) const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the register for the specified operand." );
			writer.WriteLine( "void set_op_register( uint32_t operand, Register reg ) noexcept;" );
			writer.WriteLine();

			// Specific operand accessors for operands 0-3 (stored in arrays)
			for ( int i = 0; i < 4; i++ ) {
				writer.WriteLine( $"/// @brief Gets the operand kind for operand {i}." );
				writer.WriteLine( $"[[nodiscard]] constexpr OpKind op{i}_kind() const noexcept {{ return op_kinds_[{i}]; }}" );
				writer.WriteLine();
			}

			// Operand 4 is special - always IMMEDIATE8 (matches Rust implementation)
			writer.WriteLine( "/// @brief Gets the operand kind for operand 4." );
			writer.WriteLine( "/// @details Operand 4 is always OpKind::IMMEDIATE8 when present." );
			writer.WriteLine( "[[nodiscard]] constexpr OpKind op4_kind() const noexcept { return OpKind::IMMEDIATE8; }" );
			writer.WriteLine();

			for ( int i = 0; i < 4; i++ ) {
				writer.WriteLine( $"/// @brief Sets the operand kind for operand {i}." );
				writer.WriteLine( $"constexpr void set_op{i}_kind( OpKind value ) noexcept {{ op_kinds_[{i}] = value; }}" );
				writer.WriteLine();
			}

			// set_op4_kind is a no-op (Rust debug_asserts it's IMMEDIATE8)
			writer.WriteLine( "/// @brief Sets the operand kind for operand 4 (no-op, value must be IMMEDIATE8)." );
			writer.WriteLine( "/// @details Operand 4 kind is always IMMEDIATE8 and cannot be changed." );
			writer.WriteLine( "constexpr void set_op4_kind( [[maybe_unused]] OpKind value ) noexcept { /* no-op, op4 is always IMMEDIATE8 */ }" );
			writer.WriteLine();

			for ( int i = 0; i < 4; i++ ) {
				writer.WriteLine( $"/// @brief Gets the register for operand {i}." );
				writer.WriteLine( $"[[nodiscard]] constexpr Register op{i}_register() const noexcept {{ return regs_[{i}]; }}" );
				writer.WriteLine();
			}

			// Operand 4 register is always NONE (matches Rust implementation)
			writer.WriteLine( "/// @brief Gets the register for operand 4." );
			writer.WriteLine( "/// @details Operand 4 register is always Register::NONE." );
			writer.WriteLine( "[[nodiscard]] constexpr Register op4_register() const noexcept { return Register::NONE; }" );
			writer.WriteLine();

			for ( int i = 0; i < 4; i++ ) {
				writer.WriteLine( $"/// @brief Sets the register for operand {i}." );
				writer.WriteLine( $"constexpr void set_op{i}_register( Register value ) noexcept {{ regs_[{i}] = value; }}" );
				writer.WriteLine();
			}

			// set_op4_register is a no-op (Rust debug_asserts it's NONE)
			writer.WriteLine( "/// @brief Sets the register for operand 4 (no-op, value must be NONE)." );
			writer.WriteLine( "/// @details Operand 4 register is always NONE and cannot be changed." );
			writer.WriteLine( "constexpr void set_op4_register( [[maybe_unused]] Register value ) noexcept { /* no-op, op4 reg is always NONE */ }" );
			writer.WriteLine();
		}

		void WriteMemoryAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Memory Operand ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the memory operand base register." );
			writer.WriteLine( "[[nodiscard]] constexpr Register memory_base() const noexcept { return mem_base_reg_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the memory operand base register." );
			writer.WriteLine( "constexpr void set_memory_base( Register value ) noexcept { mem_base_reg_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the memory operand index register." );
			writer.WriteLine( "[[nodiscard]] constexpr Register memory_index() const noexcept { return mem_index_reg_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the memory operand index register." );
			writer.WriteLine( "constexpr void set_memory_index( Register value ) noexcept { mem_index_reg_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the memory operand index scale (1, 2, 4, or 8)." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t memory_index_scale() const noexcept { return 1U << scale_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the memory operand index scale (1, 2, 4, or 8)." );
			writer.WriteLine( "void set_memory_index_scale( uint32_t value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Internal: Sets scale directly (0-3 maps to 1/2/4/8). For decoder use only." );
			writer.WriteLine( "constexpr void set_scale_internal( uint8_t value ) noexcept { scale_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 32-bit memory displacement." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t memory_displacement32() const noexcept { return static_cast< uint32_t >( mem_displ_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 32-bit memory displacement." );
			writer.WriteLine( "constexpr void set_memory_displacement32( uint32_t value ) noexcept { mem_displ_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 64-bit memory displacement." );
			writer.WriteLine( "[[nodiscard]] constexpr uint64_t memory_displacement64() const noexcept { return mem_displ_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 64-bit memory displacement." );
			writer.WriteLine( "constexpr void set_memory_displacement64( uint64_t value ) noexcept { mem_displ_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the memory operand displacement size in bytes (0, 1, 2, 4, or 8)." );
			writer.WriteLine( "/// @details Internal encoding: 0=0, 1=1, 2=2, 3=4, 4=8 (values 3 and 4 are mapped)" );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t memory_displ_size() const noexcept {" );
			writer.WriteLine( "    uint32_t size = displ_size_;" );
			writer.WriteLine( "    if ( size <= 2 ) return size;" );
			writer.WriteLine( "    return size == 3 ? 4 : 8;" );
			writer.WriteLine( "}" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the memory operand displacement size in bytes (0, 1, 2, 4, or 8)." );
			writer.WriteLine( "/// @details Valid values: 0 (none), 1 (byte), 2 (word/16-bit), 4 (dword/32-bit), 8 (qword/64-bit)" );
			writer.WriteLine( "constexpr void set_memory_displ_size( uint32_t value ) noexcept {" );
			writer.WriteLine( "    switch ( value ) {" );
			writer.WriteLine( "    case 0: displ_size_ = 0; break;" );
			writer.WriteLine( "    case 1: displ_size_ = 1; break;" );
			writer.WriteLine( "    case 2: displ_size_ = 2; break;" );
			writer.WriteLine( "    case 4: displ_size_ = 3; break;" );
			writer.WriteLine( "    default: displ_size_ = 4; break; // 8 or any other value" );
			writer.WriteLine( "    }" );
			writer.WriteLine( "}" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the memory operand size." );
			writer.WriteLine( "[[nodiscard]] MemorySize memory_size() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the segment prefix (or Register::NONE if none)." );
			writer.WriteLine( "[[nodiscard]] Register segment_prefix() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the segment prefix." );
			writer.WriteLine( "void set_segment_prefix( Register value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the effective segment register used for memory access." );
			writer.WriteLine( "[[nodiscard]] Register memory_segment() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if this is an IP-relative memory operand (RIP/EIP relative addressing)." );
			writer.WriteLine( "/// @return true if the memory base register is RIP or EIP" );
			writer.WriteLine( "[[nodiscard]] constexpr bool is_ip_rel_memory_operand() const noexcept {" );
			writer.WriteLine( "    return mem_base_reg_ == Register::RIP || mem_base_reg_ == Register::EIP;" );
			writer.WriteLine( "}" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the RIP/EIP relative address (the absolute address the instruction accesses)." );
			writer.WriteLine( "/// @details This method is only valid if is_ip_rel_memory_operand() returns true." );
			writer.WriteLine( "/// For RIP-relative addressing, returns memory_displacement64()." );
			writer.WriteLine( "/// For EIP-relative addressing, returns memory_displacement32()." );
			writer.WriteLine( "/// @return The absolute target address of the RIP/EIP relative memory operand" );
			writer.WriteLine( "[[nodiscard]] constexpr uint64_t ip_rel_memory_address() const noexcept {" );
			writer.WriteLine( "    return mem_base_reg_ == Register::RIP ? mem_displ_ : static_cast< uint64_t >( static_cast< uint32_t >( mem_displ_ ) );" );
			writer.WriteLine( "}" );
			writer.WriteLine();
		}

		void WriteImmediateAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Immediate Values ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the immediate value for an 8-bit immediate operand." );
			writer.WriteLine( "[[nodiscard]] constexpr uint8_t immediate8() const noexcept { return static_cast< uint8_t >( immediate_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the immediate value for an 8-bit immediate operand." );
			writer.WriteLine( "/// @details Preserves upper 24 bits of immediate_ for MVEX instruction flags (matches Rust with mvex feature)" );
			writer.WriteLine( "constexpr void set_immediate8( uint8_t value ) noexcept { immediate_ = ( immediate_ & 0xFFFFFF00u ) | value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the second 8-bit immediate value (ENTER instruction)." );
			writer.WriteLine( "[[nodiscard]] constexpr uint8_t immediate8_2nd() const noexcept { return static_cast< uint8_t >( mem_displ_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the second 8-bit immediate value (ENTER instruction)." );
			writer.WriteLine( "constexpr void set_immediate8_2nd( uint8_t value ) noexcept { mem_displ_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the immediate value for a 16-bit immediate operand." );
			writer.WriteLine( "[[nodiscard]] constexpr uint16_t immediate16() const noexcept { return static_cast< uint16_t >( immediate_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the immediate value for a 16-bit immediate operand." );
			writer.WriteLine( "constexpr void set_immediate16( uint16_t value ) noexcept { immediate_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the immediate value for a 32-bit immediate operand." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t immediate32() const noexcept { return immediate_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the immediate value for a 32-bit immediate operand." );
			writer.WriteLine( "constexpr void set_immediate32( uint32_t value ) noexcept { immediate_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the immediate value for a 64-bit immediate operand." );
			writer.WriteLine( "[[nodiscard]] constexpr uint64_t immediate64() const noexcept { return ( static_cast< uint64_t >( mem_displ_ ) << 32 ) | immediate_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the immediate value for a 64-bit immediate operand." );
			writer.WriteLine( "constexpr void set_immediate64( uint64_t value ) noexcept { immediate_ = static_cast< uint32_t >( value ); mem_displ_ = value >> 32; }" );
			writer.WriteLine();

			// Sign-extension immediate methods (matching Rust implementation)
			writer.WriteLine( "/// @brief Gets the 8-bit immediate sign-extended to 16 bits." );
			writer.WriteLine( "/// @details Use this if operand kind is OpKind::IMMEDIATE8TO16" );
			writer.WriteLine( "[[nodiscard]] constexpr int16_t immediate8to16() const noexcept { return static_cast< int16_t >( static_cast< int8_t >( immediate_ ) ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 8-bit immediate (sign-extended to 16 bits)." );
			writer.WriteLine( "constexpr void set_immediate8to16( int16_t value ) noexcept { immediate_ = static_cast< uint32_t >( static_cast< int8_t >( value ) ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 8-bit immediate sign-extended to 32 bits." );
			writer.WriteLine( "/// @details Use this if operand kind is OpKind::IMMEDIATE8TO32" );
			writer.WriteLine( "[[nodiscard]] constexpr int32_t immediate8to32() const noexcept { return static_cast< int32_t >( static_cast< int8_t >( immediate_ ) ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 8-bit immediate (sign-extended to 32 bits)." );
			writer.WriteLine( "constexpr void set_immediate8to32( int32_t value ) noexcept { immediate_ = static_cast< uint32_t >( static_cast< int8_t >( value ) ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 8-bit immediate sign-extended to 64 bits." );
			writer.WriteLine( "/// @details Use this if operand kind is OpKind::IMMEDIATE8TO64" );
			writer.WriteLine( "[[nodiscard]] constexpr int64_t immediate8to64() const noexcept { return static_cast< int64_t >( static_cast< int8_t >( immediate_ ) ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 8-bit immediate (sign-extended to 64 bits)." );
			writer.WriteLine( "constexpr void set_immediate8to64( int64_t value ) noexcept { immediate_ = static_cast< uint32_t >( static_cast< int8_t >( value ) ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the 32-bit immediate sign-extended to 64 bits." );
			writer.WriteLine( "/// @details Use this if operand kind is OpKind::IMMEDIATE32TO64" );
			writer.WriteLine( "[[nodiscard]] constexpr int64_t immediate32to64() const noexcept { return static_cast< int64_t >( static_cast< int32_t >( immediate_ ) ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the 32-bit immediate (sign-extended to 64 bits)." );
			writer.WriteLine( "constexpr void set_immediate32to64( int64_t value ) noexcept { immediate_ = static_cast< uint32_t >( value ); }" );
			writer.WriteLine();
		}

		void WriteBranchAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Branch Targets ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the near branch 16-bit target." );
			writer.WriteLine( "[[nodiscard]] constexpr uint16_t near_branch16() const noexcept { return static_cast< uint16_t >( mem_displ_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the near branch 16-bit target." );
			writer.WriteLine( "constexpr void set_near_branch16( uint16_t value ) noexcept { mem_displ_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the near branch 32-bit target." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t near_branch32() const noexcept { return static_cast< uint32_t >( mem_displ_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the near branch 32-bit target." );
			writer.WriteLine( "constexpr void set_near_branch32( uint32_t value ) noexcept { mem_displ_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the near branch 64-bit target." );
			writer.WriteLine( "[[nodiscard]] constexpr uint64_t near_branch64() const noexcept { return mem_displ_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the near branch 64-bit target." );
			writer.WriteLine( "constexpr void set_near_branch64( uint64_t value ) noexcept { mem_displ_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the near branch target address based on operand kind." );
			writer.WriteLine( "/// @details Checks the first operand kind (or second for JKZD/JKNZD) and returns" );
			writer.WriteLine( "/// the appropriately sized branch target. Returns 0 if not a near branch." );
			writer.WriteLine( "[[nodiscard]] uint64_t near_branch_target() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the far branch 16-bit offset." );
			writer.WriteLine( "[[nodiscard]] constexpr uint16_t far_branch16() const noexcept { return static_cast< uint16_t >( immediate_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the far branch 16-bit offset." );
			writer.WriteLine( "constexpr void set_far_branch16( uint16_t value ) noexcept { immediate_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the far branch 32-bit offset." );
			writer.WriteLine( "[[nodiscard]] constexpr uint32_t far_branch32() const noexcept { return immediate_; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the far branch 32-bit offset." );
			writer.WriteLine( "constexpr void set_far_branch32( uint32_t value ) noexcept { immediate_ = value; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the far branch selector." );
			writer.WriteLine( "[[nodiscard]] constexpr uint16_t far_branch_selector() const noexcept { return static_cast< uint16_t >( mem_displ_ ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the far branch selector." );
			writer.WriteLine( "constexpr void set_far_branch_selector( uint16_t value ) noexcept { mem_displ_ = value; }" );
			writer.WriteLine();
		}

		void WritePrefixAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Prefixes ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if the instruction has a LOCK prefix." );
			writer.WriteLine( "[[nodiscard]] constexpr bool has_lock_prefix() const noexcept { return ( flags1_ & 0x8000'0000U ) != 0; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets whether the instruction has a LOCK prefix." );
			writer.WriteLine( "constexpr void set_has_lock_prefix( bool value ) noexcept { if ( value ) flags1_ |= 0x8000'0000U; else flags1_ &= ~0x8000'0000U; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if the instruction has a REP/REPE prefix." );
			writer.WriteLine( "[[nodiscard]] constexpr bool has_rep_prefix() const noexcept { return ( flags1_ & 0x2000'0000U ) != 0; }" );
			writer.WriteLine( "/// @brief Alias for has_rep_prefix()." );
			writer.WriteLine( "[[nodiscard]] constexpr bool has_repe_prefix() const noexcept { return has_rep_prefix(); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets whether the instruction has a REP/REPE prefix." );
			writer.WriteLine( "constexpr void set_has_rep_prefix( bool value ) noexcept { if ( value ) flags1_ |= 0x2000'0000U; else flags1_ &= ~0x2000'0000U; }" );
			writer.WriteLine( "/// @brief Alias for set_has_rep_prefix()." );
			writer.WriteLine( "constexpr void set_has_repe_prefix( bool value ) noexcept { set_has_rep_prefix( value ); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if the instruction has a REPNE prefix." );
			writer.WriteLine( "[[nodiscard]] constexpr bool has_repne_prefix() const noexcept { return ( flags1_ & 0x4000'0000U ) != 0; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets whether the instruction has a REPNE prefix." );
			writer.WriteLine( "constexpr void set_has_repne_prefix( bool value ) noexcept { if ( value ) flags1_ |= 0x4000'0000U; else flags1_ &= ~0x4000'0000U; }" );
			writer.WriteLine();
		}

		void WriteMiscAccessors( FileWriter writer ) {
			writer.WriteLine( "// === EVEX/VEX/XOP/MVEX Features ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if this is a broadcast instruction (EVEX.b)." );
			writer.WriteLine( "[[nodiscard]] constexpr bool is_broadcast() const noexcept { return ( flags1_ & 0x0400'0000U ) != 0; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the broadcast flag." );
			writer.WriteLine( "constexpr void set_is_broadcast( bool value ) noexcept { if ( value ) flags1_ |= 0x0400'0000U; else flags1_ &= ~0x0400'0000U; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if suppress-all-exceptions is enabled (EVEX/MVEX)." );
			writer.WriteLine( "[[nodiscard]] constexpr bool suppress_all_exceptions() const noexcept { return ( flags1_ & 0x0800'0000U ) != 0; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the suppress-all-exceptions flag." );
			writer.WriteLine( "constexpr void set_suppress_all_exceptions( bool value ) noexcept { if ( value ) flags1_ |= 0x0800'0000U; else flags1_ &= ~0x0800'0000U; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if zeroing-masking is used (EVEX.z)." );
			writer.WriteLine( "[[nodiscard]] constexpr bool zeroing_masking() const noexcept { return ( flags1_ & 0x1000'0000U ) != 0; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets zeroing-masking mode." );
			writer.WriteLine( "constexpr void set_zeroing_masking( bool value ) noexcept { if ( value ) flags1_ |= 0x1000'0000U; else flags1_ &= ~0x1000'0000U; }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Checks if merging-masking is used." );
			writer.WriteLine( "[[nodiscard]] constexpr bool merging_masking() const noexcept { return !zeroing_masking(); }" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the rounding control." );
			writer.WriteLine( "[[nodiscard]] RoundingControl rounding_control() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the rounding control." );
			writer.WriteLine( "void set_rounding_control( RoundingControl value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the opmask register (k1-k7) or Register::NONE." );
			writer.WriteLine( "[[nodiscard]] Register op_mask() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the opmask register." );
			writer.WriteLine( "void set_op_mask( Register value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the code size used when decoding this instruction." );
			writer.WriteLine( "[[nodiscard]] CodeSize code_size() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the code size." );
			writer.WriteLine( "void set_code_size( CodeSize value ) noexcept;" );
			writer.WriteLine();
		}

		void WriteDeclareDataAccessors( FileWriter writer ) {
			writer.WriteLine( "// === Declare Data Methods ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the number of elements in a db/dw/dd/dq directive." );
			writer.WriteLine( "/// Can only be called if code() is DeclareByte, DeclareWord, DeclareDword, or DeclareQword." );
			writer.WriteLine( "[[nodiscard]] uint32_t declare_data_len() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the number of elements in a db/dw/dd/dq directive." );
			writer.WriteLine( "/// @param value New value: db: 1-16; dw: 1-8; dd: 1-4; dq: 1-2" );
			writer.WriteLine( "void set_declare_data_len( uint32_t value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets a db value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-15)" );
			writer.WriteLine( "[[nodiscard]] uint8_t get_declare_byte_value( uint32_t index ) const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets a db value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-15)" );
			writer.WriteLine( "/// @param value New value" );
			writer.WriteLine( "void set_declare_byte_value( uint32_t index, uint8_t value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets a dw value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-7)" );
			writer.WriteLine( "[[nodiscard]] uint16_t get_declare_word_value( uint32_t index ) const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets a dw value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-7)" );
			writer.WriteLine( "/// @param value New value" );
			writer.WriteLine( "void set_declare_word_value( uint32_t index, uint16_t value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets a dd value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-3)" );
			writer.WriteLine( "[[nodiscard]] uint32_t get_declare_dword_value( uint32_t index ) const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets a dd value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-3)" );
			writer.WriteLine( "/// @param value New value" );
			writer.WriteLine( "void set_declare_dword_value( uint32_t index, uint32_t value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets a dq value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-1)" );
			writer.WriteLine( "[[nodiscard]] uint64_t get_declare_qword_value( uint32_t index ) const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets a dq value at the specified index." );
			writer.WriteLine( "/// @param index Index (0-1)" );
			writer.WriteLine( "/// @param value New value" );
			writer.WriteLine( "void set_declare_qword_value( uint32_t index, uint64_t value ) noexcept;" );
			writer.WriteLine();
		}

		void WriteMvexAccessors( FileWriter writer ) {
			writer.WriteLine( "// === MVEX Methods ===" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Returns true if eviction hint bit is set ({eh}) (MVEX instructions only)." );
			writer.WriteLine( "[[nodiscard]] bool is_mvex_eviction_hint() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the eviction hint bit (MVEX instructions only)." );
			writer.WriteLine( "void set_is_mvex_eviction_hint( bool value ) noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Gets the MVEX register/memory operand conversion function." );
			writer.WriteLine( "[[nodiscard]] MvexRegMemConv mvex_reg_mem_conv() const noexcept;" );
			writer.WriteLine();
			writer.WriteLine( "/// @brief Sets the MVEX register/memory operand conversion function." );
			writer.WriteLine( "void set_mvex_reg_mem_conv( MvexRegMemConv value ) noexcept;" );
			writer.WriteLine();
		}

		void GenerateInstructionSource() {
			var filename = CppConstants.GetSourceFilename( genTypes, "instruction.cpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "#include \"iced_x86/instruction.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/internal/tables.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/internal/mvex_instr_flags.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/iced_constants.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine();

			// Implement methods that need tables or complex logic
			WriteInstructionSourceMethods( writer );
			WriteDeclareDataSourceMethods( writer );
			WriteMvexSourceMethods( writer );

			writer.WriteLine();
			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
		}

		void WriteInstructionSourceMethods( FileWriter writer ) {
			writer.WriteLine( "Mnemonic Instruction::mnemonic() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return internal::g_code_to_mnemonic[static_cast< std::size_t >( code_ )];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "uint32_t Instruction::op_count() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return internal::g_instruction_op_counts[static_cast< std::size_t >( code_ )];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "OpKind Instruction::op_kind( uint32_t operand ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( operand < 4 ) return op_kinds_[operand];" );
				writer.WriteLine( "if ( operand == 4 ) return OpKind::IMMEDIATE8; // op4 is always IMMEDIATE8" );
				writer.WriteLine( "return OpKind::REGISTER; // Invalid operand, but match default behavior" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_op_kind( uint32_t operand, OpKind kind ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( operand < 4 ) op_kinds_[operand] = kind;" );
				writer.WriteLine( "// operand 4: no-op (op4_kind is always IMMEDIATE8)" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "Register Instruction::op_register( uint32_t operand ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( operand < 4 ) return regs_[operand];" );
				writer.WriteLine( "return Register::NONE; // op4_register is always NONE" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_op_register( uint32_t operand, Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( operand < 4 ) regs_[operand] = reg;" );
				writer.WriteLine( "// operand 4: no-op (op4_register is always NONE)" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_memory_index_scale( uint32_t value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( value ) {" );
				writer.WriteLine( "case 1: scale_ = 0; break;" );
				writer.WriteLine( "case 2: scale_ = 1; break;" );
				writer.WriteLine( "case 4: scale_ = 2; break;" );
				writer.WriteLine( "case 8: scale_ = 3; break;" );
				writer.WriteLine( "default: scale_ = 0; break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "MemorySize Instruction::memory_size() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return internal::g_instruction_memory_sizes[static_cast< std::size_t >( code_ )];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "Register Instruction::segment_prefix() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t SEGMENT_PREFIX_SHIFT = 5;" );
				writer.WriteLine( "constexpr uint32_t SEGMENT_PREFIX_MASK = 0x7;" );
				writer.WriteLine( "uint32_t index = ( flags1_ >> SEGMENT_PREFIX_SHIFT ) & SEGMENT_PREFIX_MASK;" );
				writer.WriteLine( "constexpr Register segments[] = { Register::NONE, Register::ES, Register::CS, Register::SS, Register::DS, Register::FS, Register::GS };" );
				writer.WriteLine( "return index < 7 ? segments[index] : Register::NONE;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_segment_prefix( Register value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t SEGMENT_PREFIX_SHIFT = 5;" );
				writer.WriteLine( "constexpr uint32_t SEGMENT_PREFIX_MASK = 0x7;" );
				writer.WriteLine( "uint32_t index = 0;" );
				writer.WriteLine( "switch ( value ) {" );
				writer.WriteLine( "case Register::ES: index = 1; break;" );
				writer.WriteLine( "case Register::CS: index = 2; break;" );
				writer.WriteLine( "case Register::SS: index = 3; break;" );
				writer.WriteLine( "case Register::DS: index = 4; break;" );
				writer.WriteLine( "case Register::FS: index = 5; break;" );
				writer.WriteLine( "case Register::GS: index = 6; break;" );
				writer.WriteLine( "default: index = 0; break;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "flags1_ = ( flags1_ & ~( SEGMENT_PREFIX_MASK << SEGMENT_PREFIX_SHIFT ) ) | ( index << SEGMENT_PREFIX_SHIFT );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "Register Instruction::memory_segment() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Register prefix = segment_prefix();" );
				writer.WriteLine( "if ( prefix != Register::NONE ) return prefix;" );
				writer.WriteLine( "Register base = memory_base();" );
				writer.WriteLine( "if ( base == Register::BP || base == Register::EBP || base == Register::ESP || base == Register::RBP || base == Register::RSP )" );
				writer.WriteLine( "    return Register::SS;" );
				writer.WriteLine( "return Register::DS;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "RoundingControl Instruction::rounding_control() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t RC_SHIFT = 12;" );
				writer.WriteLine( "constexpr uint32_t RC_MASK = 0x7;" );
				writer.WriteLine( "return static_cast< RoundingControl >( ( flags1_ >> RC_SHIFT ) & RC_MASK );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_rounding_control( RoundingControl value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t RC_SHIFT = 12;" );
				writer.WriteLine( "constexpr uint32_t RC_MASK = 0x7;" );
				writer.WriteLine( "flags1_ = ( flags1_ & ~( RC_MASK << RC_SHIFT ) ) | ( ( static_cast< uint32_t >( value ) & RC_MASK ) << RC_SHIFT );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "Register Instruction::op_mask() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t OP_MASK_SHIFT = 15;" );
				writer.WriteLine( "constexpr uint32_t OP_MASK_MASK = 0x7;" );
				writer.WriteLine( "uint32_t index = ( flags1_ >> OP_MASK_SHIFT ) & OP_MASK_MASK;" );
				writer.WriteLine( "if ( index == 0 ) return Register::NONE;" );
				writer.WriteLine( "return static_cast< Register >( static_cast< uint32_t >( Register::K0 ) + index );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_op_mask( Register value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t OP_MASK_SHIFT = 15;" );
				writer.WriteLine( "constexpr uint32_t OP_MASK_MASK = 0x7;" );
				writer.WriteLine( "uint32_t index = 0;" );
				writer.WriteLine( "if ( value >= Register::K0 && value <= Register::K7 )" );
				writer.WriteLine( "    index = static_cast< uint32_t >( value ) - static_cast< uint32_t >( Register::K0 );" );
				writer.WriteLine( "flags1_ = ( flags1_ & ~( OP_MASK_MASK << OP_MASK_SHIFT ) ) | ( ( index & OP_MASK_MASK ) << OP_MASK_SHIFT );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "CodeSize Instruction::code_size() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t CODE_SIZE_SHIFT = 18;" );
				writer.WriteLine( "constexpr uint32_t CODE_SIZE_MASK = 0x3;" );
				writer.WriteLine( "return static_cast< CodeSize >( ( flags1_ >> CODE_SIZE_SHIFT ) & CODE_SIZE_MASK );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_code_size( CodeSize value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t CODE_SIZE_SHIFT = 18;" );
				writer.WriteLine( "constexpr uint32_t CODE_SIZE_MASK = 0x3;" );
				writer.WriteLine( "flags1_ = ( flags1_ & ~( CODE_SIZE_MASK << CODE_SIZE_SHIFT ) ) | ( ( static_cast< uint32_t >( value ) & CODE_SIZE_MASK ) << CODE_SIZE_SHIFT );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// near_branch_target - checks operand kind to return appropriately sized value (matches Rust)
			writer.WriteLine( "uint64_t Instruction::near_branch_target() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "OpKind kind = op0_kind();" );
				writer.WriteLine( "// Check if JKZD/JKNZD (MVEX instructions with 2 operands where branch is op1)" );
				writer.WriteLine( "// Only check this for MVEX codes to avoid breaking normal 2-operand instructions" );
				writer.WriteLine( "if ( op_count() == 2 && static_cast<uint32_t>( code_ ) >= static_cast<uint32_t>( Code::MVEX_VPREFETCHNTA_M ) ) {" );
				writer.WriteLine( "    kind = op1_kind();" );
				writer.WriteLine( "}" );
				writer.WriteLine( "switch ( kind ) {" );
				writer.WriteLine( "case OpKind::NEAR_BRANCH16: return near_branch16();" );
				writer.WriteLine( "case OpKind::NEAR_BRANCH32: return near_branch32();" );
				writer.WriteLine( "case OpKind::NEAR_BRANCH64: return near_branch64();" );
				writer.WriteLine( "default: return 0;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
		}

		void WriteDeclareDataSourceMethods( FileWriter writer ) {
			// declare_data_len - uses bits 8-11 of flags1_ (DATA_LENGTH_SHIFT=8, DATA_LENGTH_MASK=0xF)
			writer.WriteLine();
			writer.WriteLine( "uint32_t Instruction::declare_data_len() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t DATA_LENGTH_SHIFT = 8;" );
				writer.WriteLine( "constexpr uint32_t DATA_LENGTH_MASK = 0xF;" );
				writer.WriteLine( "return ( ( flags1_ >> DATA_LENGTH_SHIFT ) & DATA_LENGTH_MASK ) + 1;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_declare_data_len( uint32_t value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "constexpr uint32_t DATA_LENGTH_SHIFT = 8;" );
				writer.WriteLine( "constexpr uint32_t DATA_LENGTH_MASK = 0xF;" );
				writer.WriteLine( "flags1_ = ( flags1_ & ~( DATA_LENGTH_MASK << DATA_LENGTH_SHIFT ) ) | ( ( ( value - 1 ) & DATA_LENGTH_MASK ) << DATA_LENGTH_SHIFT );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// get_declare_byte_value - bytes are stored in regs_[0-3] (as u8), immediate_ (4 bytes), mem_displ_ (8 bytes)
			writer.WriteLine( "uint8_t Instruction::get_declare_byte_value( uint32_t index ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0: return static_cast< uint8_t >( regs_[0] );" );
				writer.WriteLine( "case 1: return static_cast< uint8_t >( regs_[1] );" );
				writer.WriteLine( "case 2: return static_cast< uint8_t >( regs_[2] );" );
				writer.WriteLine( "case 3: return static_cast< uint8_t >( regs_[3] );" );
				writer.WriteLine( "case 4: return static_cast< uint8_t >( immediate_ );" );
				writer.WriteLine( "case 5: return static_cast< uint8_t >( immediate_ >> 8 );" );
				writer.WriteLine( "case 6: return static_cast< uint8_t >( immediate_ >> 16 );" );
				writer.WriteLine( "case 7: return static_cast< uint8_t >( immediate_ >> 24 );" );
				writer.WriteLine( "case 8: return static_cast< uint8_t >( mem_displ_ );" );
				writer.WriteLine( "case 9: return static_cast< uint8_t >( mem_displ_ >> 8 );" );
				writer.WriteLine( "case 10: return static_cast< uint8_t >( mem_displ_ >> 16 );" );
				writer.WriteLine( "case 11: return static_cast< uint8_t >( mem_displ_ >> 24 );" );
				writer.WriteLine( "case 12: return static_cast< uint8_t >( mem_displ_ >> 32 );" );
				writer.WriteLine( "case 13: return static_cast< uint8_t >( mem_displ_ >> 40 );" );
				writer.WriteLine( "case 14: return static_cast< uint8_t >( mem_displ_ >> 48 );" );
				writer.WriteLine( "case 15: return static_cast< uint8_t >( mem_displ_ >> 56 );" );
				writer.WriteLine( "default: return 0;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_declare_byte_value( uint32_t index, uint8_t value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0: regs_[0] = static_cast< Register >( value ); break;" );
				writer.WriteLine( "case 1: regs_[1] = static_cast< Register >( value ); break;" );
				writer.WriteLine( "case 2: regs_[2] = static_cast< Register >( value ); break;" );
				writer.WriteLine( "case 3: regs_[3] = static_cast< Register >( value ); break;" );
				writer.WriteLine( "case 4: immediate_ = ( immediate_ & 0xFFFFFF00U ) | value; break;" );
				writer.WriteLine( "case 5: immediate_ = ( immediate_ & 0xFFFF00FFU ) | ( static_cast< uint32_t >( value ) << 8 ); break;" );
				writer.WriteLine( "case 6: immediate_ = ( immediate_ & 0xFF00FFFFU ) | ( static_cast< uint32_t >( value ) << 16 ); break;" );
				writer.WriteLine( "case 7: immediate_ = ( immediate_ & 0x00FFFFFFU ) | ( static_cast< uint32_t >( value ) << 24 ); break;" );
				writer.WriteLine( "case 8: mem_displ_ = ( mem_displ_ & 0xFFFFFFFFFFFFFF00ULL ) | value; break;" );
				writer.WriteLine( "case 9: mem_displ_ = ( mem_displ_ & 0xFFFFFFFFFFFF00FFULL ) | ( static_cast< uint64_t >( value ) << 8 ); break;" );
				writer.WriteLine( "case 10: mem_displ_ = ( mem_displ_ & 0xFFFFFFFFFF00FFFFULL ) | ( static_cast< uint64_t >( value ) << 16 ); break;" );
				writer.WriteLine( "case 11: mem_displ_ = ( mem_displ_ & 0xFFFFFFFF00FFFFFFULL ) | ( static_cast< uint64_t >( value ) << 24 ); break;" );
				writer.WriteLine( "case 12: mem_displ_ = ( mem_displ_ & 0xFFFFFF00FFFFFFFFULL ) | ( static_cast< uint64_t >( value ) << 32 ); break;" );
				writer.WriteLine( "case 13: mem_displ_ = ( mem_displ_ & 0xFFFF00FFFFFFFFFFULL ) | ( static_cast< uint64_t >( value ) << 40 ); break;" );
				writer.WriteLine( "case 14: mem_displ_ = ( mem_displ_ & 0xFF00FFFFFFFFFFFFULL ) | ( static_cast< uint64_t >( value ) << 48 ); break;" );
				writer.WriteLine( "case 15: mem_displ_ = ( mem_displ_ & 0x00FFFFFFFFFFFFFFULL ) | ( static_cast< uint64_t >( value ) << 56 ); break;" );
				writer.WriteLine( "default: break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// get_declare_word_value - words stored as pairs
			writer.WriteLine( "uint16_t Instruction::get_declare_word_value( uint32_t index ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0: return static_cast< uint16_t >( regs_[0] ) | ( static_cast< uint16_t >( regs_[1] ) << 8 );" );
				writer.WriteLine( "case 1: return static_cast< uint16_t >( regs_[2] ) | ( static_cast< uint16_t >( regs_[3] ) << 8 );" );
				writer.WriteLine( "case 2: return static_cast< uint16_t >( immediate_ );" );
				writer.WriteLine( "case 3: return static_cast< uint16_t >( immediate_ >> 16 );" );
				writer.WriteLine( "case 4: return static_cast< uint16_t >( mem_displ_ );" );
				writer.WriteLine( "case 5: return static_cast< uint16_t >( mem_displ_ >> 16 );" );
				writer.WriteLine( "case 6: return static_cast< uint16_t >( mem_displ_ >> 32 );" );
				writer.WriteLine( "case 7: return static_cast< uint16_t >( mem_displ_ >> 48 );" );
				writer.WriteLine( "default: return 0;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_declare_word_value( uint32_t index, uint16_t value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0:" );
				writer.WriteLine( "	regs_[0] = static_cast< Register >( value & 0xFF );" );
				writer.WriteLine( "	regs_[1] = static_cast< Register >( ( value >> 8 ) & 0xFF );" );
				writer.WriteLine( "	break;" );
				writer.WriteLine( "case 1:" );
				writer.WriteLine( "	regs_[2] = static_cast< Register >( value & 0xFF );" );
				writer.WriteLine( "	regs_[3] = static_cast< Register >( ( value >> 8 ) & 0xFF );" );
				writer.WriteLine( "	break;" );
				writer.WriteLine( "case 2: immediate_ = ( immediate_ & 0xFFFF0000U ) | value; break;" );
				writer.WriteLine( "case 3: immediate_ = ( immediate_ & 0x0000FFFFU ) | ( static_cast< uint32_t >( value ) << 16 ); break;" );
				writer.WriteLine( "case 4: mem_displ_ = ( mem_displ_ & 0xFFFFFFFFFFFF0000ULL ) | value; break;" );
				writer.WriteLine( "case 5: mem_displ_ = ( mem_displ_ & 0xFFFFFFFF0000FFFFULL ) | ( static_cast< uint64_t >( value ) << 16 ); break;" );
				writer.WriteLine( "case 6: mem_displ_ = ( mem_displ_ & 0xFFFF0000FFFFFFFFULL ) | ( static_cast< uint64_t >( value ) << 32 ); break;" );
				writer.WriteLine( "case 7: mem_displ_ = ( mem_displ_ & 0x0000FFFFFFFFFFFFULL ) | ( static_cast< uint64_t >( value ) << 48 ); break;" );
				writer.WriteLine( "default: break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// get_declare_dword_value
			writer.WriteLine( "uint32_t Instruction::get_declare_dword_value( uint32_t index ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0: return static_cast< uint32_t >( regs_[0] ) | ( static_cast< uint32_t >( regs_[1] ) << 8 ) | ( static_cast< uint32_t >( regs_[2] ) << 16 ) | ( static_cast< uint32_t >( regs_[3] ) << 24 );" );
				writer.WriteLine( "case 1: return immediate_;" );
				writer.WriteLine( "case 2: return static_cast< uint32_t >( mem_displ_ );" );
				writer.WriteLine( "case 3: return static_cast< uint32_t >( mem_displ_ >> 32 );" );
				writer.WriteLine( "default: return 0;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_declare_dword_value( uint32_t index, uint32_t value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0:" );
				writer.WriteLine( "	regs_[0] = static_cast< Register >( value & 0xFF );" );
				writer.WriteLine( "	regs_[1] = static_cast< Register >( ( value >> 8 ) & 0xFF );" );
				writer.WriteLine( "	regs_[2] = static_cast< Register >( ( value >> 16 ) & 0xFF );" );
				writer.WriteLine( "	regs_[3] = static_cast< Register >( ( value >> 24 ) & 0xFF );" );
				writer.WriteLine( "	break;" );
				writer.WriteLine( "case 1: immediate_ = value; break;" );
				writer.WriteLine( "case 2: mem_displ_ = ( mem_displ_ & 0xFFFFFFFF00000000ULL ) | value; break;" );
				writer.WriteLine( "case 3: mem_displ_ = ( mem_displ_ & 0x00000000FFFFFFFFULL ) | ( static_cast< uint64_t >( value ) << 32 ); break;" );
				writer.WriteLine( "default: break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// get_declare_qword_value
			writer.WriteLine( "uint64_t Instruction::get_declare_qword_value( uint32_t index ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0: return static_cast< uint64_t >( regs_[0] ) | ( static_cast< uint64_t >( regs_[1] ) << 8 ) | ( static_cast< uint64_t >( regs_[2] ) << 16 ) | ( static_cast< uint64_t >( regs_[3] ) << 24 ) | ( static_cast< uint64_t >( immediate_ ) << 32 );" );
				writer.WriteLine( "case 1: return mem_displ_;" );
				writer.WriteLine( "default: return 0;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_declare_qword_value( uint32_t index, uint64_t value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( index ) {" );
				writer.WriteLine( "case 0:" );
				writer.WriteLine( "	regs_[0] = static_cast< Register >( value & 0xFF );" );
				writer.WriteLine( "	regs_[1] = static_cast< Register >( ( value >> 8 ) & 0xFF );" );
				writer.WriteLine( "	regs_[2] = static_cast< Register >( ( value >> 16 ) & 0xFF );" );
				writer.WriteLine( "	regs_[3] = static_cast< Register >( ( value >> 24 ) & 0xFF );" );
				writer.WriteLine( "	immediate_ = static_cast< uint32_t >( value >> 32 );" );
				writer.WriteLine( "	break;" );
				writer.WriteLine( "case 1: mem_displ_ = value; break;" );
				writer.WriteLine( "default: break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
		}

		void WriteMvexSourceMethods( FileWriter writer ) {
			writer.WriteLine();
			// Helper lambda inline check for MVEX range
			// MVEX codes are in range [MVEX_START, MVEX_START + MVEX_LENGTH)
			writer.WriteLine( "// Helper to check if a code is MVEX" );
			writer.WriteLine( "static inline bool is_mvex_code( Code code ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "uint32_t c = static_cast< uint32_t >( code );" );
				writer.WriteLine( "return c >= IcedConstants::MVEX_START && c < IcedConstants::MVEX_START + IcedConstants::MVEX_LENGTH;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// is_mvex_eviction_hint - uses EVICTION_HINT bit in immediate_ for MVEX instructions
			writer.WriteLine( "bool Instruction::is_mvex_eviction_hint() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return is_mvex_code( code_ ) && ( immediate_ & internal::MvexInstrFlags::EVICTION_HINT ) != 0;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_is_mvex_eviction_hint( bool value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( value )" );
				writer.WriteLine( "	immediate_ |= internal::MvexInstrFlags::EVICTION_HINT;" );
				writer.WriteLine( "else" );
				writer.WriteLine( "	immediate_ &= ~internal::MvexInstrFlags::EVICTION_HINT;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// mvex_reg_mem_conv - uses bits 16-20 of immediate_ (MVEX_REG_MEM_CONV_SHIFT=16, MVEX_REG_MEM_CONV_MASK=0x1F)
			writer.WriteLine( "MvexRegMemConv Instruction::mvex_reg_mem_conv() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( !is_mvex_code( code_ ) )" );
				writer.WriteLine( "	return MvexRegMemConv::NONE;" );
				writer.WriteLine( "return static_cast< MvexRegMemConv >( ( immediate_ >> internal::MvexInstrFlags::MVEX_REG_MEM_CONV_SHIFT ) & internal::MvexInstrFlags::MVEX_REG_MEM_CONV_MASK );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "void Instruction::set_mvex_reg_mem_conv( MvexRegMemConv value ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "immediate_ = ( immediate_ & ~( internal::MvexInstrFlags::MVEX_REG_MEM_CONV_MASK << internal::MvexInstrFlags::MVEX_REG_MEM_CONV_SHIFT ) )" );
				writer.WriteLine( "	| ( static_cast< uint32_t >( value ) << internal::MvexInstrFlags::MVEX_REG_MEM_CONV_SHIFT );" );
			}
			writer.WriteLine( "}" );
		}
	}
}
