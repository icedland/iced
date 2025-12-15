// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;
using Generator.IO;

namespace Generator.Tables.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppRegisterInfoTableGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public CppRegisterInfoTableGenerator( GeneratorContext generatorContext ) {
			idConverter = CppIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var defs = generatorContext.Types.GetObject<RegisterDefs>( TypeIds.RegisterDefs ).Defs;
			GenerateHeader( defs );
			GenerateSource( defs );
		}

		void GenerateHeader( RegisterDef[] defs ) {
			var genTypes = generatorContext.Types;
			var filename = CppConstants.GetHeaderFilename( genTypes, "register_info.hpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "#pragma once" );
			writer.WriteLine( "#ifndef ICED_X86_REGISTER_INFO_HPP" );
			writer.WriteLine( "#define ICED_X86_REGISTER_INFO_HPP" );
			writer.WriteLine();
			writer.WriteLine( "#include \"iced_x86/register.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "#include <array>" );
			writer.WriteLine( "#include <cstdint>" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine();

			// RegisterInfo struct
			writer.WriteLine( "/// @brief Contains information about a register." );
			writer.WriteLine( "struct RegisterInfo {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "/// @brief The register." );
				writer.WriteLine( "Register register_;" );
				writer.WriteLine( "/// @brief The base register (eg. AL, AX, EAX, RAX, MM0, XMM0, YMM0, ZMM0, ES)." );
				writer.WriteLine( "Register base;" );
				writer.WriteLine( "/// @brief The full register that this one is a part of, except for GPRs where the 32-bit version is returned." );
				writer.WriteLine( "Register full_register32;" );
				writer.WriteLine( "/// @brief The full register that this one is a part of." );
				writer.WriteLine( "Register full_register;" );
				writer.WriteLine( "/// @brief Size of the register in bytes." );
				writer.WriteLine( "uint16_t size;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Gets the register number (index) relative to base()." );
				writer.WriteLine( "[[nodiscard]] constexpr std::size_t number() const noexcept {" );
				using ( writer.Indent() ) {
					writer.WriteLine( "return static_cast<std::size_t>( register_ ) - static_cast<std::size_t>( base );" );
				}
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "};" );
			writer.WriteLine();

			// Declare the table
			writer.WriteLine( "namespace internal {" );
			writer.WriteLine( $"extern const std::array< RegisterInfo, {defs.Length} > g_register_infos;" );
			writer.WriteLine( "} // namespace internal" );
			writer.WriteLine();

			// Free functions to get register info
			writer.WriteLine( "/// @brief Gets information about a register." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return Information about the register." );
			writer.WriteLine( "[[nodiscard]] inline const RegisterInfo& get_register_info( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return internal::g_register_infos[static_cast<std::size_t>( reg )];" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// Convenience functions
			writer.WriteLine( "/// @brief Gets the base register (eg. AL, AX, EAX, RAX, MM0, XMM0, YMM0, ZMM0, ES)." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return The base register." );
			writer.WriteLine( "[[nodiscard]] inline Register register_base( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return get_register_info( reg ).base;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "/// @brief Gets the register number (index) relative to its base register." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return The register number." );
			writer.WriteLine( "[[nodiscard]] inline std::size_t register_number( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return get_register_info( reg ).number();" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "/// @brief Gets the full register that this one is a part of." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return The full register." );
			writer.WriteLine( "[[nodiscard]] inline Register register_full_register( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return get_register_info( reg ).full_register;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "/// @brief Gets the full register (32-bit for GPRs) that this one is a part of." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return The full 32-bit register." );
			writer.WriteLine( "[[nodiscard]] inline Register register_full_register32( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return get_register_info( reg ).full_register32;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "/// @brief Gets the size of the register in bytes." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return Size in bytes." );
			writer.WriteLine( "[[nodiscard]] inline std::size_t register_size( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return get_register_info( reg ).size;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// Register type checking functions
			WriteRegisterCheckFunction( writer, "is_segment_register", "Checks if it's a segment register (ES, CS, SS, DS, FS, GS).",
				"Register::ES", "Register::GS" );
			WriteRegisterCheckFunction( writer, "is_gpr", "Checks if it's a general purpose register (AL-R15L, AX-R15W, EAX-R15D, RAX-R15).",
				"Register::AL", "Register::R15" );
			WriteRegisterCheckFunction( writer, "is_gpr8", "Checks if it's an 8-bit general purpose register (AL-R15L).",
				"Register::AL", "Register::R15_L" );
			WriteRegisterCheckFunction( writer, "is_gpr16", "Checks if it's a 16-bit general purpose register (AX-R15W).",
				"Register::AX", "Register::R15_W" );
			WriteRegisterCheckFunction( writer, "is_gpr32", "Checks if it's a 32-bit general purpose register (EAX-R15D).",
				"Register::EAX", "Register::R15_D" );
			WriteRegisterCheckFunction( writer, "is_gpr64", "Checks if it's a 64-bit general purpose register (RAX-R15).",
				"Register::RAX", "Register::R15" );
			WriteRegisterCheckFunction( writer, "is_xmm", "Checks if it's a 128-bit vector register (XMM0-XMM31).",
				"Register::XMM0", "Register::XMM31" );
			WriteRegisterCheckFunction( writer, "is_ymm", "Checks if it's a 256-bit vector register (YMM0-YMM31).",
				"Register::YMM0", "Register::YMM31" );
			WriteRegisterCheckFunction( writer, "is_zmm", "Checks if it's a 512-bit vector register (ZMM0-ZMM31).",
				"Register::ZMM0", "Register::ZMM31" );

			writer.WriteLine( "/// @brief Checks if it's an XMM, YMM or ZMM register." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return True if it's a vector register." );
			writer.WriteLine( "[[nodiscard]] inline bool is_vector_register( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return Register::XMM0 <= reg && reg <= Register::ZMM31;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			writer.WriteLine( "/// @brief Checks if it's EIP or RIP." );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( "/// @return True if it's EIP or RIP." );
			writer.WriteLine( "[[nodiscard]] inline bool is_ip( Register reg ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return reg == Register::EIP || reg == Register::RIP;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			WriteRegisterCheckFunction( writer, "is_k", "Checks if it's an opmask register (K0-K7).",
				"Register::K0", "Register::K7" );
			WriteRegisterCheckFunction( writer, "is_cr", "Checks if it's a control register (CR0-CR15).",
				"Register::CR0", "Register::CR15" );
			WriteRegisterCheckFunction( writer, "is_dr", "Checks if it's a debug register (DR0-DR15).",
				"Register::DR0", "Register::DR15" );
			WriteRegisterCheckFunction( writer, "is_tr", "Checks if it's a test register (TR0-TR7).",
				"Register::TR0", "Register::TR7" );
			WriteRegisterCheckFunction( writer, "is_st", "Checks if it's an FPU stack register (ST0-ST7).",
				"Register::ST0", "Register::ST7" );
			WriteRegisterCheckFunction( writer, "is_bnd", "Checks if it's a bound register (BND0-BND3).",
				"Register::BND0", "Register::BND3" );
			WriteRegisterCheckFunction( writer, "is_mm", "Checks if it's an MMX register (MM0-MM7).",
				"Register::MM0", "Register::MM7" );
			WriteRegisterCheckFunction( writer, "is_tmm", "Checks if it's a tile register (TMM0-TMM7).",
				"Register::TMM0", "Register::TMM7" );

			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			writer.WriteLine();
			writer.WriteLine( "#endif // ICED_X86_REGISTER_INFO_HPP" );
		}

		void WriteRegisterCheckFunction( FileWriter writer, string funcName, string doc, string minReg, string maxReg ) {
			writer.WriteLine( $"/// @brief {doc}" );
			writer.WriteLine( "/// @param reg The register." );
			writer.WriteLine( $"/// @return True if the condition is met." );
			writer.WriteLine( $"[[nodiscard]] inline bool {funcName}( Register reg ) noexcept {{" );
			using ( writer.Indent() ) {
				writer.WriteLine( $"return {minReg} <= reg && reg <= {maxReg};" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();
		}

		void GenerateSource( RegisterDef[] defs ) {
			var genTypes = generatorContext.Types;
			var filename = CppConstants.GetSourceFilename( genTypes, "register_info.cpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "#include \"iced_x86/register_info.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine( "namespace internal {" );
			writer.WriteLine();

			writer.WriteLine( $"const std::array< RegisterInfo, {defs.Length} > g_register_infos = {{{{" );
			using ( writer.Indent() ) {
				for ( int i = 0; i < defs.Length; i++ ) {
					var def = defs[i];
					var comma = i < defs.Length - 1 ? "," : "";
					var reg = idConverter.ToDeclTypeAndValue( def.Register );
					var baseReg = idConverter.ToDeclTypeAndValue( def.BaseRegister );
					var fullReg32 = idConverter.ToDeclTypeAndValue( def.FullRegister32 );
					var fullReg = idConverter.ToDeclTypeAndValue( def.FullRegister );
					writer.WriteLine( $"{{ {reg}, {baseReg}, {fullReg32}, {fullReg}, {def.Size} }}{comma} // {def.Register.Name( idConverter )}" );
				}
			}
			writer.WriteLine( "}};" );
			writer.WriteLine();

			writer.WriteLine( "} // namespace internal" );
			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
		}
	}
}
