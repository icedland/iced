// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.IO;
using Generator.Documentation.Cpp;
using Generator.IO;

namespace Generator.Decoder.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppDecoderGenerator {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;
		readonly CppDocCommentWriter docWriter;

		public CppDecoderGenerator( GeneratorContext generatorContext ) {
			genTypes = generatorContext.Types;
			idConverter = CppIdentifierConverter.Create();
			docWriter = new CppDocCommentWriter( idConverter );
		}

		public void Generate() {
			GenerateDecoderHeader();
			GenerateDecoderSource();
		}

		void GenerateDecoderHeader() {
			var filename = CppConstants.GetHeaderFilename( genTypes, "decoder.hpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard( "DECODER" );

			writer.WriteLine( "#pragma once" );
			writer.WriteLine( $"#ifndef {headerGuard}" );
			writer.WriteLine( $"#define {headerGuard}" );
			writer.WriteLine();

			// Includes
			writer.WriteLine( "#include \"iced_x86/instruction.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/decoder_error.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/decoder_options.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/code_size.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/internal/handlers.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "#include \"iced_x86/internal/compiler_intrinsics.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "#include <cstdint>" );
			writer.WriteLine( "#include <cstddef>" );
			writer.WriteLine( "#include <cstring>" );
			writer.WriteLine( "#include <span>" );
			writer.WriteLine( "#include <expected>" );
			writer.WriteLine( "#include <optional>" );
			writer.WriteLine( "#include <vector>" );
			writer.WriteLine();

			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine();

			WriteDecoderClass( writer );

			writer.WriteLine();
			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			writer.WriteLine();
			writer.WriteLine( $"#endif // {headerGuard}" );
		}

		void WriteDecoderClass( FileWriter writer ) {
			writer.WriteLine( "/// @brief Error information returned when decoding fails." );
			writer.WriteLine( "struct DecodeError {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "DecoderError error = DecoderError::NONE;" );
				writer.WriteLine( "uint64_t ip = 0;" );
			}
			writer.WriteLine( "};" );
			writer.WriteLine();

			// Operand size enum
			writer.WriteLine( "/// @brief Operand size enumeration" );
			writer.WriteLine( "enum class OpSize : uint8_t {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "SIZE16 = 0," );
				writer.WriteLine( "SIZE32 = 1," );
				writer.WriteLine( "SIZE64 = 2" );
			}
			writer.WriteLine( "};" );
			writer.WriteLine();

			// Mandatory prefix enum
			writer.WriteLine( "/// @brief Mandatory prefix state" );
			writer.WriteLine( "enum class DecoderMandatoryPrefix : uint8_t {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "PNP = 0,  // No prefix or 66/F2/F3 not treated as mandatory prefix" );
				writer.WriteLine( "P66 = 1,  // 66 prefix" );
				writer.WriteLine( "PF3 = 2,  // F3 prefix" );
				writer.WriteLine( "PF2 = 3   // F2 prefix" );
			}
			writer.WriteLine( "};" );
			writer.WriteLine();

			// State flags
			writer.WriteLine( "/// @brief State flags for decoder" );
			writer.WriteLine( "struct StateFlags {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "static constexpr uint32_t HAS_REX = 1u << 0;" );
				writer.WriteLine( "static constexpr uint32_t W = 1u << 1;" );
				writer.WriteLine( "static constexpr uint32_t IS_INVALID = 1u << 2;" );
				writer.WriteLine( "static constexpr uint32_t NO_MORE_BYTES = 1u << 3;" );
				writer.WriteLine( "static constexpr uint32_t HAS66 = 1u << 4;" );
				writer.WriteLine( "static constexpr uint32_t LOCK = 1u << 5;" );
				writer.WriteLine( "static constexpr uint32_t ALLOW_LOCK = 1u << 6;" );
				writer.WriteLine( "static constexpr uint32_t ADDR64 = 1u << 7;" );
				writer.WriteLine( "static constexpr uint32_t IP_REL64 = 1u << 8;" );
				writer.WriteLine( "static constexpr uint32_t IP_REL32 = 1u << 9;" );
				writer.WriteLine( "static constexpr uint32_t B = 1u << 10;  // EVEX.b broadcast/rounding" );
				writer.WriteLine( "static constexpr uint32_t Z = 1u << 11;  // EVEX.z zeroing-masking" );
			}
			writer.WriteLine( "};" );
			writer.WriteLine();

			// Vector length enum
			writer.WriteLine( "/// @brief Vector length for VEX/EVEX instructions" );
			writer.WriteLine( "enum class VectorLength : uint8_t {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "L128 = 0,  // 128-bit (XMM)" );
				writer.WriteLine( "L256 = 1,  // 256-bit (YMM)" );
				writer.WriteLine( "L512 = 2,  // 512-bit (ZMM) - EVEX only" );
				writer.WriteLine( "UNKNOWN = 3" );
			}
			writer.WriteLine( "};" );
			writer.WriteLine();

			// Decoder state struct - layout optimized to match Rust for efficient bulk clearing
			writer.WriteLine( "/// @brief Decoder state" );
			writer.WriteLine( "/// Layout optimized to match Rust: fields cleared together are adjacent," );
			writer.WriteLine( "/// small fields grouped to avoid padding." );
			writer.WriteLine( "struct DecoderState {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// First 4 fields - read from modrm byte" );
				writer.WriteLine( "uint32_t modrm = 0;" );
				writer.WriteLine( "uint32_t mod_ = 0;" );
				writer.WriteLine( "uint32_t reg = 0;" );
				writer.WriteLine( "uint32_t rm = 0;" );
				writer.WriteLine();
				writer.WriteLine( "// Fields cleared together in decode_internal() - keep adjacent for cache efficiency" );
				writer.WriteLine( "uint32_t extra_register_base = 0;" );
				writer.WriteLine( "uint32_t extra_index_register_base = 0;" );
				writer.WriteLine( "uint32_t extra_base_register_base = 0;" );
				writer.WriteLine( "uint32_t extra_index_register_base_vsib = 0; // EVEX.V' for VSIB" );
				writer.WriteLine( "uint32_t flags = 0;" );
				writer.WriteLine();
				writer.WriteLine( "// These are also cleared together" );
				writer.WriteLine( "uint32_t vvvv = 0;" );
				writer.WriteLine( "uint32_t vvvv_invalid_check = 0;  // For validation" );
				writer.WriteLine();
				writer.WriteLine( "// EVEX-specific fields" );
				writer.WriteLine( "uint32_t aaa = 0;  // EVEX opmask register (k0-k7)" );
				writer.WriteLine( "uint32_t extra_register_base_evex = 0;    // EVEX.R' extension" );
				writer.WriteLine( "uint32_t extra_base_register_base_evex = 0; // EVEX.X' and B' extensions" );
				writer.WriteLine();
				writer.WriteLine( "// Memory index for dispatch tables" );
				writer.WriteLine( "uint32_t mem_index = 0;" );
				writer.WriteLine();
				writer.WriteLine( "// These 4 bytes are accessed/written together - keep 4-byte aligned" );
				writer.WriteLine( "OpSize address_size = OpSize::SIZE64;" );
				writer.WriteLine( "OpSize operand_size = OpSize::SIZE32;" );
				writer.WriteLine( "uint8_t segment_prio = 0;" );
				writer.WriteLine( "uint8_t dummy = 0;  // Padding to align, also helps compiler clear all 4 at once" );
				writer.WriteLine();
				writer.WriteLine( "// Less frequently used fields at end" );
				writer.WriteLine( "DecoderMandatoryPrefix mandatory_prefix = DecoderMandatoryPrefix::PNP;" );
				writer.WriteLine( "VectorLength vector_length = VectorLength::L128;" );
				writer.WriteLine( "bool modrm_read = false;  // Track if modrm has been read for this instruction" );
				writer.WriteLine( "uint8_t pad_ = 0;  // Explicit padding" );
			}
			writer.WriteLine( "};" );
			writer.WriteLine();

			writer.WriteLine( "/// @brief x86/x64 instruction decoder." );
			writer.WriteLine( "///" );
			writer.WriteLine( "/// @details Decodes x86/x64 instructions from a byte buffer. Supports 16-bit," );
			writer.WriteLine( "/// 32-bit, and 64-bit modes." );
			writer.WriteLine( "class Decoder {" );
			writer.WriteLine( "public:" );

			using ( writer.Indent() ) {
				writer.WriteLine( "/// @brief Creates a decoder for the specified bitness." );
				writer.WriteLine( "/// @param bitness 16, 32, or 64" );
				writer.WriteLine( "/// @param data Code bytes to decode" );
				writer.WriteLine( "/// @param ip Instruction pointer of first byte" );
				writer.WriteLine( "/// @param options Decoder options" );
				writer.WriteLine( "Decoder(" );
				writer.WriteLine( "  uint32_t bitness," );
				writer.WriteLine( "  std::span< const uint8_t > data," );
				writer.WriteLine( "  uint64_t ip = 0," );
				writer.WriteLine( "  DecoderOptions::Value options = DecoderOptions::NONE" );
				writer.WriteLine( ") noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Decodes the next instruction." );
				writer.WriteLine( "/// @return Decoded instruction or error" );
				writer.WriteLine( "[[nodiscard]] std::expected< Instruction, DecodeError > decode() noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Decodes the next instruction (never fails, returns invalid on error)." );
				writer.WriteLine( "/// @param[out] error Set to the error code if decoding fails" );
				writer.WriteLine( "[[nodiscard]] Instruction decode_out( DecoderError& error ) noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Checks if there are more bytes to decode." );
				writer.WriteLine( "[[nodiscard]] bool can_decode() const noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets current position in bytes." );
				writer.WriteLine( "[[nodiscard]] std::size_t position() const noexcept { return static_cast<std::size_t>( data_ptr_ - data_.data() ); }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Sets current position in bytes." );
				writer.WriteLine( "void set_position( std::size_t pos ) noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets current instruction pointer." );
				writer.WriteLine( "[[nodiscard]] uint64_t ip() const noexcept { return ip_; }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets current read position IP as 32-bit value." );
				writer.WriteLine( "/// This returns the IP at the current read position (ip_ + bytes read so far)." );
				writer.WriteLine( "[[nodiscard]] uint32_t current_ip32() const noexcept { return static_cast<uint32_t>( ip_ + ( data_ptr_ - instr_start_ptr_ ) ); }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets current read position IP as 64-bit value." );
				writer.WriteLine( "/// This returns the IP at the current read position (ip_ + bytes read so far)." );
				writer.WriteLine( "[[nodiscard]] uint64_t current_ip64() const noexcept { return ip_ + static_cast<uint64_t>( data_ptr_ - instr_start_ptr_ ); }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Sets current instruction pointer." );
				writer.WriteLine( "void set_ip( uint64_t ip ) noexcept { ip_ = ip; }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets the bitness (16, 32, or 64)." );
				writer.WriteLine( "[[nodiscard]] uint32_t bitness() const noexcept { return bitness_; }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets the total number of data bytes." );
				writer.WriteLine( "[[nodiscard]] std::size_t max_position() const noexcept { return data_.size(); }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets the decoder options." );
				writer.WriteLine( "[[nodiscard]] DecoderOptions::Value options() const noexcept { return options_; }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Checks if AMD decoder option is enabled." );
				writer.WriteLine( "[[nodiscard]] bool has_amd_option() const noexcept { return ( options_ & DecoderOptions::AMD ) != 0; }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Gets the decoder state (for handler use)." );
				writer.WriteLine( "[[nodiscard]] DecoderState& state() noexcept { return state_; }" );
				writer.WriteLine( "[[nodiscard]] const DecoderState& state() const noexcept { return state_; }" );
				writer.WriteLine();

				// Read methods
				writer.WriteLine( "/// @brief Reads a byte from the input stream." );
				writer.WriteLine( "[[nodiscard]] std::optional<uint8_t> read_byte() noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Reads a word (2 bytes) from the input stream." );
				writer.WriteLine( "[[nodiscard]] std::optional<uint16_t> read_u16() noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Reads a dword (4 bytes) from the input stream." );
				writer.WriteLine( "[[nodiscard]] std::optional<uint32_t> read_u32() noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Reads a qword (8 bytes) from the input stream." );
				writer.WriteLine( "[[nodiscard]] std::optional<uint64_t> read_u64() noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast byte read - returns 0 and sets error flag on failure." );
				writer.WriteLine( "/// Like Rust's read_u8(), errors are checked later via state flags." );
				writer.WriteLine( "/// Uses pointer arithmetic for optimal codegen." );
				writer.WriteLine( "[[nodiscard]] ICED_FORCE_INLINE uint32_t read_u8_fast() noexcept {" );
				writer.WriteLine( "  if ( data_ptr_ < max_data_ptr_ ) [[likely]] {" );
				writer.WriteLine( "    return *data_ptr_++;" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return 0;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast u16 read - returns 0 and sets error flag on failure." );
				writer.WriteLine( "[[nodiscard]] ICED_FORCE_INLINE uint32_t read_u16_fast() noexcept {" );
				writer.WriteLine( "  if ( data_ptr_ + 1 < max_data_ptr_ ) [[likely]] {" );
				writer.WriteLine( "    uint16_t result;" );
				writer.WriteLine( "    std::memcpy( &result, data_ptr_, 2 );" );
				writer.WriteLine( "    data_ptr_ += 2;" );
				writer.WriteLine( "    return result;" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return 0;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast u32 read - returns 0 and sets error flag on failure." );
				writer.WriteLine( "[[nodiscard]] ICED_FORCE_INLINE uint32_t read_u32_fast() noexcept {" );
				writer.WriteLine( "  if ( data_ptr_ + 3 < max_data_ptr_ ) [[likely]] {" );
				writer.WriteLine( "    uint32_t result;" );
				writer.WriteLine( "    std::memcpy( &result, data_ptr_, 4 );" );
				writer.WriteLine( "    data_ptr_ += 4;" );
				writer.WriteLine( "    return result;" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return 0;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast u64 read - returns 0 and sets error flag on failure." );
				writer.WriteLine( "[[nodiscard]] ICED_FORCE_INLINE uint64_t read_u64_fast() noexcept {" );
				writer.WriteLine( "  if ( data_ptr_ + 7 < max_data_ptr_ ) [[likely]] {" );
				writer.WriteLine( "    uint64_t result;" );
				writer.WriteLine( "    std::memcpy( &result, data_ptr_, 8 );" );
				writer.WriteLine( "    data_ptr_ += 8;" );
				writer.WriteLine( "    return result;" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return 0;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast unchecked byte read - caller must ensure bytes are available." );
				writer.WriteLine( "/// Use can_read(1) to check first." );
				writer.WriteLine( "[[nodiscard]] uint8_t read_byte_unchecked() noexcept {" );
				writer.WriteLine( "  return *data_ptr_++;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast unchecked u16 read - caller must ensure bytes are available." );
				writer.WriteLine( "[[nodiscard]] uint16_t read_u16_unchecked() noexcept {" );
				writer.WriteLine( "  uint16_t result;" );
				writer.WriteLine( "  std::memcpy( &result, data_ptr_, 2 );" );
				writer.WriteLine( "  data_ptr_ += 2;" );
				writer.WriteLine( "  return result;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast unchecked u32 read - caller must ensure bytes are available." );
				writer.WriteLine( "[[nodiscard]] uint32_t read_u32_unchecked() noexcept {" );
				writer.WriteLine( "  uint32_t result;" );
				writer.WriteLine( "  std::memcpy( &result, data_ptr_, 4 );" );
				writer.WriteLine( "  data_ptr_ += 4;" );
				writer.WriteLine( "  return result;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Fast unchecked u64 read - caller must ensure bytes are available." );
				writer.WriteLine( "[[nodiscard]] uint64_t read_u64_unchecked() noexcept {" );
				writer.WriteLine( "  uint64_t result;" );
				writer.WriteLine( "  std::memcpy( &result, data_ptr_, 8 );" );
				writer.WriteLine( "  data_ptr_ += 8;" );
				writer.WriteLine( "  return result;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Check if n bytes can be read within the current instruction." );
				writer.WriteLine( "[[nodiscard]] bool can_read( std::size_t n ) const noexcept {" );
				writer.WriteLine( "  return data_ptr_ + n <= max_data_ptr_;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				// State manipulation
				writer.WriteLine( "/// @brief Sets the instruction as invalid." );
				writer.WriteLine( "void set_invalid_instruction() noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Reads modrm byte unconditionally (for sub-handlers that need fresh modrm)." );
				writer.WriteLine( "void read_modrm() noexcept {" );
				writer.WriteLine( "  if ( data_ptr_ >= max_data_ptr_ ) [[unlikely]] {" );
				writer.WriteLine( "    state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "    return;" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  auto m = static_cast<uint32_t>( *data_ptr_++ );" );
				writer.WriteLine( "  state_.modrm = m;" );
				writer.WriteLine( "  state_.reg = ( m >> 3 ) & 7;" );
				writer.WriteLine( "  state_.mod_ = m >> 6;" );
				writer.WriteLine( "  state_.rm = m & 7;" );
				writer.WriteLine( "  state_.mem_index = ( state_.mod_ << 3 ) | state_.rm;" );
				writer.WriteLine( "  state_.modrm_read = true;" );
				writer.WriteLine( "}" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Checks if running in 64-bit mode." );
				writer.WriteLine( "[[nodiscard]] bool is_64bit_mode() const noexcept { return bitness_ == 64; }" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Resets REX prefix state (called by prefix handlers)." );
				writer.WriteLine( "void reset_rex_prefix_state() noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Calls the map0 opcode handler table." );
				writer.WriteLine( "void call_opcode_handlers_map0_table( Instruction& instruction ) noexcept;" );
				writer.WriteLine();

				// Memory operand decoding
				writer.WriteLine( "/// @brief Reads a memory operand." );
				writer.WriteLine( "/// @param instruction The instruction being decoded" );
				writer.WriteLine( "/// @param operand_index Which operand slot (0-4)" );
				writer.WriteLine( "void read_op_mem( Instruction& instruction, uint32_t operand_index ) noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Reads a VSIB memory operand (for gather/scatter instructions)." );
				writer.WriteLine( "/// @param instruction The instruction being decoded" );
				writer.WriteLine( "/// @param operand_index Which operand slot (0-4)" );
				writer.WriteLine( "/// @param vsib_index Base register for VSIB index (e.g., XMM0, YMM0, ZMM0)" );
				writer.WriteLine( "/// @param tuple_type Tuple type for displacement scaling" );
				writer.WriteLine( "void read_op_mem_vsib( Instruction& instruction, uint32_t operand_index, Register vsib_index, uint32_t tuple_type ) noexcept;" );
				writer.WriteLine();

				// VEX/EVEX methods
				writer.WriteLine( "/// @brief Decodes VEX2 (C5) prefix and dispatches to VEX handler." );
				writer.WriteLine( "void decode_vex2( Instruction& instruction ) noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Decodes VEX3 (C4) prefix and dispatches to VEX handler." );
				writer.WriteLine( "void decode_vex3( Instruction& instruction ) noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Decodes EVEX (62) prefix and dispatches to EVEX handler." );
				writer.WriteLine( "void decode_evex( Instruction& instruction ) noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Decodes XOP prefix and dispatches to XOP handler." );
				writer.WriteLine( "void decode_xop( Instruction& instruction ) noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Decodes 3DNow! prefix and dispatches to 3DNow! handler." );
				writer.WriteLine( "void decode_3dnow( Instruction& instruction ) noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Gets the VEX handler table for the specified map." );
				writer.WriteLine( "/// @param map_index Map index (0=0F, 1=0F38, 2=0F3A)" );
				writer.WriteLine( "/// @return Handler table span (empty if invalid)" );
				writer.WriteLine( "[[nodiscard]] std::span<const internal::HandlerEntry> get_vex_table( uint32_t map_index ) const noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Gets the EVEX handler table for the specified map." );
				writer.WriteLine( "/// @param map_index Map index (0=0F, 1=0F38, 2=0F3A, 4=MAP5, 5=MAP6)" );
				writer.WriteLine( "/// @return Handler table span (empty if invalid)" );
				writer.WriteLine( "[[nodiscard]] std::span<const internal::HandlerEntry> get_evex_table( uint32_t map_index ) const noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Gets the mask for register extension bits (0xF in 64-bit, 0x7 in 32/16-bit)." );
				writer.WriteLine( "[[nodiscard]] uint32_t reg15_mask() const noexcept { return bitness_ == 64 ? 0xF : 0x7; }" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Gets the invalid check mask for VEX/EVEX prefix validation." );
				writer.WriteLine( "[[nodiscard]] uint32_t invalid_check_mask() const noexcept { return invalid_check_mask_; }" );
				writer.WriteLine();
				writer.WriteLine( "/// @brief Reads an EVEX memory operand with tuple type for displacement scaling." );
				writer.WriteLine( "/// @param instruction The instruction being decoded" );
				writer.WriteLine( "/// @param operand_index Which operand slot (0-4)" );
				writer.WriteLine( "/// @param tuple_type Tuple type for EVEX displacement scaling" );
				writer.WriteLine( "void read_op_mem_evex( Instruction& instruction, uint32_t operand_index, uint32_t tuple_type ) noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "/// @brief Dispatch to a handler, reading modrm if required." );
				writer.WriteLine( "/// @param handler The handler entry to dispatch to" );
				writer.WriteLine( "/// @param instruction The instruction being decoded" );
				writer.WriteLine( "void decode_table( internal::HandlerEntry handler, Instruction& instruction ) noexcept;" );
			}

			writer.WriteLine();
			writer.WriteLine( "private:" );
			using ( writer.Indent() ) {
				writer.WriteLine( "void decode_internal( Instruction& instruction ) noexcept;" );
				writer.WriteLine();
				writer.WriteLine( "// Memory decoding helpers" );
				writer.WriteLine( "void read_op_mem_32_or_64( Instruction& instruction, uint32_t operand_index ) noexcept;" );
				writer.WriteLine( "void read_op_mem_16( Instruction& instruction, uint32_t operand_index ) noexcept;" );
				writer.WriteLine( "bool read_sib( Instruction& instruction ) noexcept;" );
				writer.WriteLine();

				writer.WriteLine( "// Pointer-based data access (like Rust) for better codegen" );
				writer.WriteLine( "const uint8_t* data_ptr_ = nullptr;       // Current read position" );
				writer.WriteLine( "const uint8_t* data_ptr_end_ = nullptr;   // End of data buffer" );
				writer.WriteLine( "const uint8_t* max_data_ptr_ = nullptr;   // Max position for current instruction (data_ptr + 15)" );
				writer.WriteLine( "const uint8_t* instr_start_ptr_ = nullptr; // Start of current instruction" );
				writer.WriteLine();
				writer.WriteLine( "// Keep span for API compatibility (position(), max_position(), etc.)" );
				writer.WriteLine( "std::span< const uint8_t > data_;" );
				writer.WriteLine( "uint64_t ip_ = 0;" );
				writer.WriteLine( "uint32_t bitness_ = 64;" );
				writer.WriteLine( "DecoderOptions::Value options_ = DecoderOptions::NONE;" );
				writer.WriteLine();
				writer.WriteLine( "// Default sizes based on bitness" );
				writer.WriteLine( "OpSize default_operand_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "OpSize default_inverted_operand_size_ = OpSize::SIZE16;" );
				writer.WriteLine( "OpSize default_address_size_ = OpSize::SIZE64;" );
				writer.WriteLine( "OpSize default_inverted_address_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "CodeSize default_code_size_ = CodeSize::CODE64;" );
				writer.WriteLine();
				writer.WriteLine( "// Decoder state" );
				writer.WriteLine( "DecoderState state_;" );
				writer.WriteLine();
				writer.WriteLine( "// Pointers to static handler tables (shared across all Decoder instances)" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_map0_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_vex_0f_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_vex_0f38_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_vex_0f3a_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_evex_0f_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_evex_0f38_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_evex_0f3a_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_evex_map5_;" );
				writer.WriteLine( "std::span<const internal::HandlerEntry> handlers_evex_map6_;" );
				writer.WriteLine();
				writer.WriteLine( "// Masks for bitness-dependent behavior" );
				writer.WriteLine( "uint32_t mask_e0_ = 0;  // E0 mask for inverted bits (0xE0 in 64-bit, 0 in 32/16-bit)" );
				writer.WriteLine( "uint32_t invalid_check_mask_ = 0;  // For checking invalid prefix combinations" );
				writer.WriteLine();
				writer.WriteLine( "static constexpr std::size_t MAX_INSTRUCTION_LENGTH = 15;" );
				writer.WriteLine();
				writer.WriteLine( "// Static handler tables - initialized once, shared by all Decoder instances" );
				writer.WriteLine( "struct Tables {" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_map0;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_vex_0f;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_vex_0f38;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_vex_0f3a;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_evex_0f;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_evex_0f38;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_evex_0f3a;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_evex_map5;" );
				writer.WriteLine( "  std::vector<internal::HandlerEntry> handlers_evex_map6;" );
				writer.WriteLine( "};" );
				writer.WriteLine();
				writer.WriteLine( "static const Tables& get_tables();" );
			}

			writer.WriteLine( "};" );
		}

		void GenerateDecoderSource() {
			var filename = CppConstants.GetSourceFilename( genTypes, "decoder.cpp" );
			var dir = Path.GetDirectoryName( filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( filename ) );
			writer.WriteFileHeader();

			writer.WriteLine( "#include \"iced_x86/decoder.hpp\"" );
			writer.WriteLine( "#include \"iced_x86/internal/table_deserializer.hpp\"" );
			writer.WriteLine();
			writer.WriteLine( "#include <algorithm>" );
			writer.WriteLine();
			writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			writer.WriteLine();

			WriteDecoderSourceMethods( writer );

			writer.WriteLine();
			writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
		}

		void WriteDecoderSourceMethods( FileWriter writer ) {
			// Static tables initialization (Meyers singleton)
			writer.WriteLine( "// Static tables - initialized once, shared by all Decoder instances (like Rust's lazy_static)" );
			writer.WriteLine( "const Decoder::Tables& Decoder::get_tables() {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Meyers singleton - thread-safe in C++11 and later" );
				writer.WriteLine( "static Tables tables = []() {" );
				using ( writer.Indent() ) {
					writer.WriteLine( "Tables t;" );
					writer.WriteLine( "t.handlers_map0 = internal::read_legacy_tables();" );
					writer.WriteLine();
					writer.WriteLine( "auto vex_tables = internal::read_vex_tables();" );
					writer.WriteLine( "if ( vex_tables.size() >= 3 ) {" );
					writer.WriteLine( "  t.handlers_vex_0f = std::move( vex_tables[0] );" );
					writer.WriteLine( "  t.handlers_vex_0f38 = std::move( vex_tables[1] );" );
					writer.WriteLine( "  t.handlers_vex_0f3a = std::move( vex_tables[2] );" );
					writer.WriteLine( "}" );
					writer.WriteLine();
					writer.WriteLine( "auto evex_tables = internal::read_evex_tables();" );
					writer.WriteLine( "if ( evex_tables.size() >= 5 ) {" );
					writer.WriteLine( "  t.handlers_evex_0f = std::move( evex_tables[0] );" );
					writer.WriteLine( "  t.handlers_evex_0f38 = std::move( evex_tables[1] );" );
					writer.WriteLine( "  t.handlers_evex_0f3a = std::move( evex_tables[2] );" );
					writer.WriteLine( "  t.handlers_evex_map5 = std::move( evex_tables[3] );" );
					writer.WriteLine( "  t.handlers_evex_map6 = std::move( evex_tables[4] );" );
					writer.WriteLine( "}" );
					writer.WriteLine();
					writer.WriteLine( "return t;" );
				}
				writer.WriteLine( "}();" );
				writer.WriteLine( "return tables;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// Constructor
			writer.WriteLine( "Decoder::Decoder(" );
			writer.WriteLine( "  uint32_t bitness," );
			writer.WriteLine( "  std::span< const uint8_t > data," );
			writer.WriteLine( "  uint64_t ip," );
			writer.WriteLine( "  DecoderOptions::Value options" );
			writer.WriteLine( ") noexcept" );
			writer.WriteLine( "  : data_ptr_( data.data() )" );
			writer.WriteLine( "  , data_ptr_end_( data.data() + data.size() )" );
			writer.WriteLine( "  , max_data_ptr_( data.data() )" );
			writer.WriteLine( "  , instr_start_ptr_( data.data() )" );
			writer.WriteLine( "  , data_( data )" );
			writer.WriteLine( "  , ip_( ip )" );
			writer.WriteLine( "  , bitness_( bitness )" );
			writer.WriteLine( "  , options_( options )" );
			writer.WriteLine( "{" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Set default sizes based on bitness" );
				writer.WriteLine( "switch ( bitness ) {" );
				writer.WriteLine( "  case 64:" );
				writer.WriteLine( "    default_operand_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "    default_inverted_operand_size_ = OpSize::SIZE16;" );
				writer.WriteLine( "    default_address_size_ = OpSize::SIZE64;" );
				writer.WriteLine( "    default_inverted_address_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "    default_code_size_ = CodeSize::CODE64;" );
				writer.WriteLine( "    break;" );
				writer.WriteLine( "  case 32:" );
				writer.WriteLine( "    default_operand_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "    default_inverted_operand_size_ = OpSize::SIZE16;" );
				writer.WriteLine( "    default_address_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "    default_inverted_address_size_ = OpSize::SIZE16;" );
				writer.WriteLine( "    default_code_size_ = CodeSize::CODE32;" );
				writer.WriteLine( "    break;" );
				writer.WriteLine( "  case 16:" );
				writer.WriteLine( "  default:" );
				writer.WriteLine( "    default_operand_size_ = OpSize::SIZE16;" );
				writer.WriteLine( "    default_inverted_operand_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "    default_address_size_ = OpSize::SIZE16;" );
				writer.WriteLine( "    default_inverted_address_size_ = OpSize::SIZE32;" );
				writer.WriteLine( "    default_code_size_ = CodeSize::CODE16;" );
				writer.WriteLine( "    break;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Get reference to static tables (initialized once, shared by all decoders)" );
				writer.WriteLine( "const auto& tables = get_tables();" );
				writer.WriteLine( "handlers_map0_ = tables.handlers_map0;" );
				writer.WriteLine( "handlers_vex_0f_ = tables.handlers_vex_0f;" );
				writer.WriteLine( "handlers_vex_0f38_ = tables.handlers_vex_0f38;" );
				writer.WriteLine( "handlers_vex_0f3a_ = tables.handlers_vex_0f3a;" );
				writer.WriteLine( "handlers_evex_0f_ = tables.handlers_evex_0f;" );
				writer.WriteLine( "handlers_evex_0f38_ = tables.handlers_evex_0f38;" );
				writer.WriteLine( "handlers_evex_0f3a_ = tables.handlers_evex_0f3a;" );
				writer.WriteLine( "handlers_evex_map5_ = tables.handlers_evex_map5;" );
				writer.WriteLine( "handlers_evex_map6_ = tables.handlers_evex_map6;" );
				writer.WriteLine();
				writer.WriteLine( "// Set up masks for bitness-dependent behavior" );
				writer.WriteLine( "mask_e0_ = ( bitness == 64 ) ? 0xE0u : 0u;" );
				writer.WriteLine( "// invalid_check_mask is based on NO_INVALID_CHECK option, not bitness (matches Rust)" );
				writer.WriteLine( "invalid_check_mask_ = ( ( options & DecoderOptions::NO_INVALID_CHECK ) == 0 ) ? 0xFFFFFFFFu : 0u;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// decode()
			writer.WriteLine( "std::expected< Instruction, DecodeError > Decoder::decode() noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "DecoderError error = DecoderError::NONE;" );
				writer.WriteLine( "Instruction instr = decode_out( error );" );
				writer.WriteLine( "if ( error != DecoderError::NONE ) {" );
				writer.WriteLine( "  return std::unexpected( DecodeError{ error, ip_ } );" );
				writer.WriteLine( "}" );
				writer.WriteLine( "return instr;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// decode_out()
			writer.WriteLine( "Instruction Decoder::decode_out( DecoderError& error ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "Instruction instr{};" );
				writer.WriteLine( "error = DecoderError::NONE;" );
				writer.WriteLine();
				writer.WriteLine( "if ( data_ptr_ >= data_ptr_end_ ) {" );
				writer.WriteLine( "  error = DecoderError::NO_MORE_BYTES;" );
				writer.WriteLine( "  return instr;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "decode_internal( instr );" );
				writer.WriteLine();
				writer.WriteLine( "// Check for errors" );
				writer.WriteLine( "if ( ( state_.flags & StateFlags::NO_MORE_BYTES ) != 0 ) {" );
				writer.WriteLine( "  error = DecoderError::NO_MORE_BYTES;" );
				writer.WriteLine( "} else if ( ( state_.flags & StateFlags::IS_INVALID ) != 0 ) {" );
				writer.WriteLine( "  error = DecoderError::INVALID_INSTRUCTION;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "return instr;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// decode_internal()
			writer.WriteLine( "void Decoder::decode_internal( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Reset state - clear 5 consecutive uint32_t fields at once" );
				writer.WriteLine( "// Fields: extra_register_base, extra_index_register_base, extra_base_register_base," );
				writer.WriteLine( "//         extra_index_register_base_vsib, flags" );
				writer.WriteLine( "std::memset( &state_.extra_register_base, 0, 5 * sizeof( uint32_t ) );" );
				writer.WriteLine();
				writer.WriteLine( "// Clear vvvv fields (2 consecutive uint32_t)" );
				writer.WriteLine( "state_.vvvv = 0;" );
				writer.WriteLine( "state_.vvvv_invalid_check = 0;" );
				writer.WriteLine();
				writer.WriteLine( "// Set address/operand size (these are set, not cleared)" );
				writer.WriteLine( "state_.address_size = default_address_size_;" );
				writer.WriteLine( "state_.operand_size = default_operand_size_;" );
				writer.WriteLine( "state_.segment_prio = 0;" );
				writer.WriteLine( "state_.dummy = 0;" );
				writer.WriteLine();
				writer.WriteLine( "// Less frequently used" );
				writer.WriteLine( "state_.mandatory_prefix = DecoderMandatoryPrefix::PNP;" );
				writer.WriteLine( "state_.modrm_read = false;" );
				writer.WriteLine();
				writer.WriteLine( "// Set up pointers for this instruction" );
				writer.WriteLine( "instr_start_ptr_ = data_ptr_;" );
				writer.WriteLine( "// Max instruction length is 15 bytes, but don't exceed data end" );
				writer.WriteLine( "auto remaining = static_cast<std::size_t>( data_ptr_end_ - data_ptr_ );" );
				writer.WriteLine( "max_data_ptr_ = data_ptr_ + ( remaining < MAX_INSTRUCTION_LENGTH ? remaining : MAX_INSTRUCTION_LENGTH );" );
				writer.WriteLine();
				writer.WriteLine( "// Read first byte - use direct pointer access for speed" );
				writer.WriteLine( "if ( data_ptr_ >= max_data_ptr_ ) [[unlikely]] {" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "auto b = static_cast<std::size_t>( *data_ptr_++ );" );
				writer.WriteLine();
				writer.WriteLine( "// Check for REX prefix in 64-bit mode" );
				writer.WriteLine( "if ( bitness_ == 64 && ( b & 0xF0 ) == 0x40 ) {" );
				writer.WriteLine( "  // REX prefix - need another byte" );
				writer.WriteLine( "  if ( data_ptr_ >= max_data_ptr_ ) [[unlikely]] {" );
				writer.WriteLine( "    state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "    return;" );
				writer.WriteLine( "  }" );
				writer.WriteLine();
				writer.WriteLine( "  uint32_t flags = state_.flags | StateFlags::HAS_REX;" );
				writer.WriteLine( "  if ( ( b & 8 ) != 0 ) {" );
				writer.WriteLine( "    flags |= StateFlags::W;" );
				writer.WriteLine( "    state_.operand_size = OpSize::SIZE64;" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  state_.flags = flags;" );
				writer.WriteLine( "  state_.extra_register_base = ( static_cast<uint32_t>( b ) & 4 ) << 1;" );
				writer.WriteLine( "  state_.extra_index_register_base = ( static_cast<uint32_t>( b ) & 2 ) << 2;" );
				writer.WriteLine( "  state_.extra_base_register_base = ( static_cast<uint32_t>( b ) & 1 ) << 3;" );
				writer.WriteLine();
				writer.WriteLine( "  b = static_cast<std::size_t>( *data_ptr_++ );" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Look up handler" );
				writer.WriteLine( "if ( b < handlers_map0_.size() ) {" );
				writer.WriteLine( "  auto& handler = handlers_map0_[b];" );
				writer.WriteLine( "  decode_table( handler, instruction );" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Calculate instruction length from pointers" );
				writer.WriteLine( "auto instr_len = static_cast<uint32_t>( data_ptr_ - instr_start_ptr_ );" );
				writer.WriteLine( "instruction.set_length( instr_len );" );
				writer.WriteLine();
				writer.WriteLine( "// Update IP" );
				writer.WriteLine( "auto orig_ip = ip_;" );
				writer.WriteLine( "ip_ += instr_len;" );
				writer.WriteLine( "instruction.set_next_ip( ip_ );" );
				writer.WriteLine( "instruction.set_code_size( default_code_size_ );" );
				writer.WriteLine();
				writer.WriteLine( "// Post-process RIP/EIP-relative addressing: convert displacement to absolute address" );
				writer.WriteLine( "auto flags = state_.flags;" );
				writer.WriteLine( "if ( ( flags & ( StateFlags::IP_REL64 | StateFlags::IP_REL32 | StateFlags::IS_INVALID ) ) != 0 ) {" );
				writer.WriteLine( "  if ( ( flags & StateFlags::IP_REL64 ) != 0 ) {" );
				writer.WriteLine( "    // RIP-relative: target = next_ip + displacement" );
				writer.WriteLine( "    auto addr = ip_ + instruction.memory_displacement64();" );
				writer.WriteLine( "    instruction.set_memory_displacement64( addr );" );
				writer.WriteLine( "  } else if ( ( flags & StateFlags::IP_REL32 ) != 0 ) {" );
				writer.WriteLine( "    // EIP-relative: target = next_ip + displacement (32-bit)" );
				writer.WriteLine( "    auto addr = static_cast<uint32_t>( ip_ ) + static_cast<uint32_t>( instruction.memory_displacement64() );" );
				writer.WriteLine( "    instruction.set_memory_displacement64( addr );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Handle invalid instructions and LOCK prefix validation (matches Rust decoder.rs line ~1442-1443)" );
				writer.WriteLine( "// Invalid if: IS_INVALID flag is set, OR LOCK prefix used without ALLOW_LOCK (when invalid checking is enabled)" );
				writer.WriteLine( "bool is_invalid = ( state_.flags & StateFlags::IS_INVALID ) != 0;" );
				writer.WriteLine( "if ( !is_invalid ) {" );
				writer.WriteLine( "  // Check LOCK prefix validation: LOCK set but ALLOW_LOCK not set" );
				writer.WriteLine( "  is_invalid = ( ( ( state_.flags & ( StateFlags::LOCK | StateFlags::ALLOW_LOCK ) ) & invalid_check_mask_ ) == StateFlags::LOCK );" );
				writer.WriteLine( "}" );
				writer.WriteLine( "if ( is_invalid ) {" );
				writer.WriteLine( "  instruction = Instruction{};" );
				writer.WriteLine( "  instruction.set_code( Code::INVALID );" );
				writer.WriteLine();
				writer.WriteLine( "  instr_len = static_cast<uint32_t>( data_ptr_ - instr_start_ptr_ );" );
				writer.WriteLine( "  instruction.set_length( instr_len );" );
				writer.WriteLine( "  ip_ = orig_ip + instr_len;" );
				writer.WriteLine( "  instruction.set_next_ip( ip_ );" );
				writer.WriteLine( "  instruction.set_code_size( default_code_size_ );" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// decode_table()
			writer.WriteLine( "void Decoder::decode_table( internal::HandlerEntry handler, Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Only read modrm if:" );
				writer.WriteLine( "// 1. Handler requires modrm, AND" );
				writer.WriteLine( "// 2. Modrm hasn't already been read for this instruction" );
				writer.WriteLine( "if ( handler.handler->has_modrm && !state_.modrm_read ) {" );
				writer.WriteLine( "  if ( data_ptr_ >= max_data_ptr_ ) [[unlikely]] {" );
				writer.WriteLine( "    set_invalid_instruction();" );
				writer.WriteLine( "    return;" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  auto m = static_cast<uint32_t>( *data_ptr_++ );" );
				writer.WriteLine( "  state_.modrm = m;" );
				writer.WriteLine( "  state_.reg = ( m >> 3 ) & 7;" );
				writer.WriteLine( "  state_.mod_ = m >> 6;" );
				writer.WriteLine( "  state_.rm = m & 7;" );
				writer.WriteLine( "  state_.mem_index = ( state_.mod_ << 3 ) | state_.rm;" );
				writer.WriteLine( "  state_.modrm_read = true;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "handler.decode( handler.handler, *this, instruction );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// can_decode()
			writer.WriteLine( "bool Decoder::can_decode() const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "return data_ptr_ < data_ptr_end_;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// set_position()
			writer.WriteLine( "void Decoder::set_position( std::size_t pos ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( pos <= data_.size() ) {" );
				writer.WriteLine( "  auto new_ptr = data_.data() + pos;" );
				writer.WriteLine( "  int64_t diff = new_ptr - data_ptr_;" );
				writer.WriteLine( "  data_ptr_ = new_ptr;" );
				writer.WriteLine( "  ip_ = static_cast<uint64_t>( static_cast<int64_t>( ip_ ) + diff );" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_byte()
			writer.WriteLine( "std::optional<uint8_t> Decoder::read_byte() noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( data_ptr_ >= max_data_ptr_ ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return std::nullopt;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "return *data_ptr_++;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_u16()
			writer.WriteLine( "std::optional<uint16_t> Decoder::read_u16() noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( data_ptr_ + 2 > max_data_ptr_ ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return std::nullopt;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint16_t result;" );
				writer.WriteLine( "std::memcpy( &result, data_ptr_, 2 );" );
				writer.WriteLine( "data_ptr_ += 2;" );
				writer.WriteLine( "return result;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_u32()
			writer.WriteLine( "std::optional<uint32_t> Decoder::read_u32() noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( data_ptr_ + 4 > max_data_ptr_ ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return std::nullopt;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t result;" );
				writer.WriteLine( "std::memcpy( &result, data_ptr_, 4 );" );
				writer.WriteLine( "data_ptr_ += 4;" );
				writer.WriteLine( "return result;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_u64()
			writer.WriteLine( "std::optional<uint64_t> Decoder::read_u64() noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( data_ptr_ + 8 > max_data_ptr_ ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::IS_INVALID | StateFlags::NO_MORE_BYTES;" );
				writer.WriteLine( "  return std::nullopt;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint64_t result;" );
				writer.WriteLine( "std::memcpy( &result, data_ptr_, 8 );" );
				writer.WriteLine( "data_ptr_ += 8;" );
				writer.WriteLine( "return result;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// Note: read_byte_unchecked, read_u16_unchecked, read_u32_unchecked, read_u64_unchecked
			// are now defined inline in the header for better inlining

			// set_invalid_instruction()
			writer.WriteLine( "void Decoder::set_invalid_instruction() noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "state_.flags |= StateFlags::IS_INVALID;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// reset_rex_prefix_state()
			writer.WriteLine( "void Decoder::reset_rex_prefix_state() noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "state_.flags &= ~( StateFlags::HAS_REX | StateFlags::W );" );
				writer.WriteLine( "if ( ( state_.flags & StateFlags::HAS66 ) == 0 ) {" );
				writer.WriteLine( "  state_.operand_size = default_operand_size_;" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  state_.operand_size = default_inverted_operand_size_;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "state_.extra_register_base = 0;" );
				writer.WriteLine( "state_.extra_index_register_base = 0;" );
				writer.WriteLine( "state_.extra_base_register_base = 0;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// call_opcode_handlers_map0_table()
			writer.WriteLine( "void Decoder::call_opcode_handlers_map0_table( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "auto b_opt = read_byte();" );
				writer.WriteLine( "if ( !b_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "auto b = static_cast<std::size_t>( *b_opt );" );
				writer.WriteLine( "if ( b < handlers_map0_.size() ) {" );
				writer.WriteLine( "  decode_table( handlers_map0_[b], instruction );" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_op_mem()
			writer.WriteLine( "void Decoder::read_op_mem( Instruction& instruction, uint32_t operand_index ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "if ( state_.address_size == OpSize::SIZE16 ) {" );
				writer.WriteLine( "  read_op_mem_16( instruction, operand_index );" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  read_op_mem_32_or_64( instruction, operand_index );" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_op_mem_32_or_64() - 32/64-bit addressing
			writer.WriteLine( "void Decoder::read_op_mem_32_or_64( Instruction& instruction, uint32_t operand_index ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Base register for 32 vs 64-bit addressing" );
				writer.WriteLine( "Register base_reg = ( state_.address_size == OpSize::SIZE64 ) ? Register::RAX : Register::EAX;" );
				writer.WriteLine();
				writer.WriteLine( "if ( state_.mod_ == 0 ) {" );
				writer.WriteLine( "  // No displacement (except special cases)" );
				writer.WriteLine( "  if ( state_.rm == 4 ) {" );
				writer.WriteLine( "    // SIB byte" );
				writer.WriteLine( "    read_sib( instruction );" );
				writer.WriteLine( "  } else if ( state_.rm == 5 ) {" );
				writer.WriteLine( "    // RIP/EIP-relative or disp32" );
				writer.WriteLine( "    auto disp = read_u32();" );
				writer.WriteLine( "    if ( !disp ) return;" );
				writer.WriteLine( "    instruction.set_memory_displacement64( static_cast<int32_t>( *disp ) );" );
				writer.WriteLine( "    instruction.set_memory_displ_size( 4 );" );
				writer.WriteLine( "    if ( bitness_ == 64 ) {" );
				writer.WriteLine( "      instruction.set_memory_base( Register::RIP );" );
				writer.WriteLine( "      state_.flags |= StateFlags::IP_REL64;" );
				writer.WriteLine( "    } else if ( state_.address_size == OpSize::SIZE64 ) {" );
				writer.WriteLine( "      instruction.set_memory_base( Register::EIP );" );
				writer.WriteLine( "      state_.flags |= StateFlags::IP_REL32;" );
				writer.WriteLine( "    }" );
				writer.WriteLine( "  } else {" );
				writer.WriteLine( "    // Simple base register" );
				writer.WriteLine( "    instruction.set_memory_base( static_cast<Register>(" );
				writer.WriteLine( "      static_cast<uint32_t>( base_reg ) + state_.rm + state_.extra_base_register_base ) );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "} else if ( state_.mod_ == 1 ) {" );
				writer.WriteLine( "  // 8-bit displacement" );
				writer.WriteLine( "  if ( state_.rm == 4 ) {" );
				writer.WriteLine( "    read_sib( instruction );" );
				writer.WriteLine( "  } else {" );
				writer.WriteLine( "    instruction.set_memory_base( static_cast<Register>(" );
				writer.WriteLine( "      static_cast<uint32_t>( base_reg ) + state_.rm + state_.extra_base_register_base ) );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  auto disp = read_byte();" );
				writer.WriteLine( "  if ( !disp ) return;" );
				writer.WriteLine( "  instruction.set_memory_displacement64( static_cast<int8_t>( *disp ) );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 1 );" );
				writer.WriteLine( "} else if ( state_.mod_ == 2 ) {" );
				writer.WriteLine( "  // 32-bit displacement" );
				writer.WriteLine( "  if ( state_.rm == 4 ) {" );
				writer.WriteLine( "    read_sib( instruction );" );
				writer.WriteLine( "  } else {" );
				writer.WriteLine( "    instruction.set_memory_base( static_cast<Register>(" );
				writer.WriteLine( "      static_cast<uint32_t>( base_reg ) + state_.rm + state_.extra_base_register_base ) );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  auto disp = read_u32();" );
				writer.WriteLine( "  if ( !disp ) return;" );
				writer.WriteLine( "  instruction.set_memory_displacement64( static_cast<int32_t>( *disp ) );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 4 );" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Set operand kind based on operand_index" );
				writer.WriteLine( "switch ( operand_index ) {" );
				writer.WriteLine( "  case 0: instruction.set_op0_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 1: instruction.set_op1_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 2: instruction.set_op2_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 3: instruction.set_op3_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_sib()
			writer.WriteLine( "bool Decoder::read_sib( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "auto sib_opt = read_byte();" );
				writer.WriteLine( "if ( !sib_opt ) return false;" );
				writer.WriteLine( "auto sib = static_cast<uint32_t>( *sib_opt );" );
				writer.WriteLine();
				writer.WriteLine( "// Scale: bits 7-6 (0-3 maps to 1, 2, 4, 8)" );
				writer.WriteLine( "instruction.set_memory_index_scale( 1u << ( sib >> 6 ) );" );
				writer.WriteLine();
				writer.WriteLine( "// Base register for 32 vs 64-bit addressing" );
				writer.WriteLine( "Register base_reg = ( state_.address_size == OpSize::SIZE64 ) ? Register::RAX : Register::EAX;" );
				writer.WriteLine();
				writer.WriteLine( "// Index: bits 5-3 + REX.X extension" );
				writer.WriteLine( "uint32_t index = ( ( sib >> 3 ) & 7 ) + state_.extra_index_register_base;" );
				writer.WriteLine( "if ( index != 4 ) {  // index=4 means no index register" );
				writer.WriteLine( "  instruction.set_memory_index( static_cast<Register>(" );
				writer.WriteLine( "    static_cast<uint32_t>( base_reg ) + index ) );" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Base: bits 2-0 + REX.B extension" );
				writer.WriteLine( "uint32_t base = ( sib & 7 ) + state_.extra_base_register_base;" );
				writer.WriteLine( "if ( ( sib & 7 ) == 5 && state_.mod_ == 0 ) {" );
				writer.WriteLine( "  // Special case: base=5 with mod=0 means disp32 only" );
				writer.WriteLine( "  auto disp = read_u32();" );
				writer.WriteLine( "  if ( !disp ) return false;" );
				writer.WriteLine( "  instruction.set_memory_displacement64( static_cast<int32_t>( *disp ) );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 4 );" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  instruction.set_memory_base( static_cast<Register>(" );
				writer.WriteLine( "    static_cast<uint32_t>( base_reg ) + base ) );" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "return true;" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_op_mem_16() - 16-bit addressing
			writer.WriteLine( "void Decoder::read_op_mem_16( Instruction& instruction, uint32_t operand_index ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// 16-bit addressing mode lookup table" );
				writer.WriteLine( "static constexpr struct { Register base; Register index; } mem_regs_16[] = {" );
				writer.WriteLine( "  { Register::BX, Register::SI },  // rm=0: [BX+SI]" );
				writer.WriteLine( "  { Register::BX, Register::DI },  // rm=1: [BX+DI]" );
				writer.WriteLine( "  { Register::BP, Register::SI },  // rm=2: [BP+SI]" );
				writer.WriteLine( "  { Register::BP, Register::DI },  // rm=3: [BP+DI]" );
				writer.WriteLine( "  { Register::SI, Register::NONE },// rm=4: [SI]" );
				writer.WriteLine( "  { Register::DI, Register::NONE },// rm=5: [DI]" );
				writer.WriteLine( "  { Register::BP, Register::NONE },// rm=6: [BP] or disp16 if mod=0" );
				writer.WriteLine( "  { Register::BX, Register::NONE } // rm=7: [BX]" );
				writer.WriteLine( "};" );
				writer.WriteLine();
				writer.WriteLine( "if ( state_.mod_ == 0 && state_.rm == 6 ) {" );
				writer.WriteLine( "  // disp16 only" );
				writer.WriteLine( "  auto disp = read_u16();" );
				writer.WriteLine( "  if ( !disp ) return;" );
				writer.WriteLine( "  instruction.set_memory_displacement64( *disp );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 2 );" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  auto& regs = mem_regs_16[state_.rm];" );
				writer.WriteLine( "  instruction.set_memory_base( regs.base );" );
				writer.WriteLine( "  if ( regs.index != Register::NONE ) {" );
				writer.WriteLine( "    instruction.set_memory_index( regs.index );" );
				writer.WriteLine( "  }" );
				writer.WriteLine();
				writer.WriteLine( "  if ( state_.mod_ == 1 ) {" );
				writer.WriteLine( "    auto disp = read_byte();" );
				writer.WriteLine( "    if ( !disp ) return;" );
				writer.WriteLine( "    instruction.set_memory_displacement64( static_cast<int8_t>( *disp ) );" );
				writer.WriteLine( "    instruction.set_memory_displ_size( 1 );" );
				writer.WriteLine( "  } else if ( state_.mod_ == 2 ) {" );
				writer.WriteLine( "    auto disp = read_u16();" );
				writer.WriteLine( "    if ( !disp ) return;" );
				writer.WriteLine( "    instruction.set_memory_displacement64( *disp );" );
				writer.WriteLine( "    instruction.set_memory_displ_size( 2 );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Set operand kind" );
				writer.WriteLine( "switch ( operand_index ) {" );
				writer.WriteLine( "  case 0: instruction.set_op0_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 1: instruction.set_op1_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 2: instruction.set_op2_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 3: instruction.set_op3_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// read_op_mem_vsib() - VSIB memory operand for gather/scatter
			writer.WriteLine( "void Decoder::read_op_mem_vsib( Instruction& instruction, uint32_t operand_index, Register vsib_index, uint32_t tuple_type ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// VSIB addressing always requires a SIB byte (mod != 3, rm == 4)" );
				writer.WriteLine( "// The index register comes from VSIB, not from SIB.index" );
				writer.WriteLine();
				writer.WriteLine( "if ( state_.address_size == OpSize::SIZE16 ) {" );
				writer.WriteLine( "  // 16-bit addressing doesn't support VSIB" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Read the SIB byte" );
				writer.WriteLine( "auto sib_opt = read_byte();" );
				writer.WriteLine( "if ( !sib_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t sib = *sib_opt;" );
				writer.WriteLine();
				writer.WriteLine( "// Extract SIB fields" );
				writer.WriteLine( "uint32_t scale = 1u << ( sib >> 6 );" );
				writer.WriteLine( "uint32_t index = ( ( sib >> 3 ) & 7 ) + state_.extra_index_register_base + state_.extra_index_register_base_vsib;" );
				writer.WriteLine( "uint32_t base = ( sib & 7 ) + state_.extra_base_register_base;" );
				writer.WriteLine();
				writer.WriteLine( "// Set scale" );
				writer.WriteLine( "instruction.set_memory_index_scale( scale );" );
				writer.WriteLine();
				writer.WriteLine( "// Set VSIB index register" );
				writer.WriteLine( "instruction.set_memory_index( static_cast<Register>( static_cast<uint32_t>( vsib_index ) + index ) );" );
				writer.WriteLine();
				writer.WriteLine( "// Base register (64-bit or 32-bit addressing)" );
				writer.WriteLine( "Register base_reg = ( state_.address_size == OpSize::SIZE64 ) ? Register::RAX : Register::EAX;" );
				writer.WriteLine();
				writer.WriteLine( "// Handle displacement based on mod" );
				writer.WriteLine( "if ( state_.mod_ == 0 ) {" );
				writer.WriteLine( "  if ( ( sib & 7 ) == 5 ) {" );
				writer.WriteLine( "    // No base register, just disp32" );
				writer.WriteLine( "    auto disp = read_u32();" );
				writer.WriteLine( "    if ( !disp ) return;" );
				writer.WriteLine( "    instruction.set_memory_displacement64( static_cast<int32_t>( *disp ) );" );
				writer.WriteLine( "    instruction.set_memory_displ_size( 4 );" );
				writer.WriteLine( "  } else {" );
				writer.WriteLine( "    instruction.set_memory_base( static_cast<Register>( static_cast<uint32_t>( base_reg ) + base ) );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "} else if ( state_.mod_ == 1 ) {" );
				writer.WriteLine( "  // 8-bit displacement (scaled by tuple_type for EVEX)" );
				writer.WriteLine( "  instruction.set_memory_base( static_cast<Register>( static_cast<uint32_t>( base_reg ) + base ) );" );
				writer.WriteLine( "  auto disp = read_byte();" );
				writer.WriteLine( "  if ( !disp ) return;" );
				writer.WriteLine( "  int32_t scaled_disp = static_cast<int8_t>( *disp );" );
				writer.WriteLine( "  if ( tuple_type != 0 ) {" );
				writer.WriteLine( "    scaled_disp *= static_cast<int32_t>( tuple_type );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  instruction.set_memory_displacement64( scaled_disp );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 1 );" );
				writer.WriteLine( "} else if ( state_.mod_ == 2 ) {" );
				writer.WriteLine( "  // 32-bit displacement" );
				writer.WriteLine( "  instruction.set_memory_base( static_cast<Register>( static_cast<uint32_t>( base_reg ) + base ) );" );
				writer.WriteLine( "  auto disp = read_u32();" );
				writer.WriteLine( "  if ( !disp ) return;" );
				writer.WriteLine( "  instruction.set_memory_displacement64( static_cast<int32_t>( *disp ) );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 4 );" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Set operand kind" );
				writer.WriteLine( "switch ( operand_index ) {" );
				writer.WriteLine( "  case 0: instruction.set_op0_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 1: instruction.set_op1_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 2: instruction.set_op2_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 3: instruction.set_op3_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// VEX2 decode
			WriteVex2DecodeMethod( writer );

			// VEX3 decode
			WriteVex3DecodeMethod( writer );

			// EVEX decode
			WriteEvexDecodeMethod( writer );

			// get_vex_table()
			writer.WriteLine( "std::span<const internal::HandlerEntry> Decoder::get_vex_table( uint32_t map_index ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( map_index ) {" );
				writer.WriteLine( "  case 0: return handlers_vex_0f_;" );
				writer.WriteLine( "  case 1: return handlers_vex_0f38_;" );
				writer.WriteLine( "  case 2: return handlers_vex_0f3a_;" );
				writer.WriteLine( "  default: return {};" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();

			// get_evex_table()
			writer.WriteLine( "std::span<const internal::HandlerEntry> Decoder::get_evex_table( uint32_t map_index ) const noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "switch ( map_index ) {" );
				writer.WriteLine( "  case 0: return handlers_evex_0f_;" );
				writer.WriteLine( "  case 1: return handlers_evex_0f38_;" );
				writer.WriteLine( "  case 2: return handlers_evex_0f3a_;" );
				writer.WriteLine( "  case 4: return handlers_evex_map5_;" );
				writer.WriteLine( "  case 5: return handlers_evex_map6_;" );
				writer.WriteLine( "  default: return {};" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
		}

		void WriteVex2DecodeMethod( FileWriter writer ) {
			writer.WriteLine( "void Decoder::decode_vex2( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Validate: no REX prefix and no mandatory prefix already set" );
				writer.WriteLine( "if ( ( ( ( state_.flags & StateFlags::HAS_REX ) |" );
				writer.WriteLine( "        static_cast<uint32_t>( state_.mandatory_prefix ) ) & invalid_check_mask_ ) != 0 ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Clear W flag and reset REX extension bits" );
				writer.WriteLine( "state_.flags &= ~StateFlags::W;" );
				writer.WriteLine( "state_.extra_index_register_base = 0;" );
				writer.WriteLine( "state_.extra_base_register_base = 0;" );
				writer.WriteLine( "state_.extra_register_base_evex = 0;" );
				writer.WriteLine( "state_.extra_base_register_base_evex = 0;" );
				writer.WriteLine();
				writer.WriteLine( "// state_.modrm contains the VEX byte2 (already read)" );
				writer.WriteLine( "uint32_t b2 = state_.modrm;" );
				writer.WriteLine();
				writer.WriteLine( "// Read opcode byte" );
				writer.WriteLine( "auto opcode_opt = read_byte();" );
				writer.WriteLine( "if ( !opcode_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t opcode = *opcode_opt;" );
				writer.WriteLine();
				writer.WriteLine( "// Extract VEX fields from b2:" );
				writer.WriteLine( "// Bit 7: ~R (inverted REX.R)" );
				writer.WriteLine( "// Bits 6-3: ~vvvv (inverted register specifier)" );
				writer.WriteLine( "// Bit 2: L (vector length: 0=128, 1=256)" );
				writer.WriteLine( "// Bits 1-0: pp (implied mandatory prefix)" );
				writer.WriteLine( "state_.vector_length = static_cast<VectorLength>( ( b2 >> 2 ) & 1 );" );
				writer.WriteLine( "state_.mandatory_prefix = static_cast<DecoderMandatoryPrefix>( b2 & 3 );" );
				writer.WriteLine();
				writer.WriteLine( "uint32_t b2_inv = ~b2;" );
				writer.WriteLine( "state_.extra_register_base = ( b2_inv >> 4 ) & 8;  // R bit -> bit 3" );
				writer.WriteLine();
				writer.WriteLine( "uint32_t vvvv = ( b2_inv >> 3 ) & 0x0F;" );
				writer.WriteLine( "state_.vvvv_invalid_check = vvvv;" );
				writer.WriteLine( "state_.vvvv = vvvv & reg15_mask();" );
				writer.WriteLine();
				writer.WriteLine( "// VEX2 implies map 0F (map_index = 0)" );
				writer.WriteLine( "auto* table = get_vex_table( 0 );" );
				writer.WriteLine( "if ( !table || opcode >= table->size() ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "decode_table( (*table)[opcode], instruction );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();
		}

		void WriteVex3DecodeMethod( FileWriter writer ) {
			writer.WriteLine( "void Decoder::decode_vex3( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Validate: no REX prefix and no mandatory prefix already set" );
				writer.WriteLine( "if ( ( ( ( state_.flags & StateFlags::HAS_REX ) |" );
				writer.WriteLine( "        static_cast<uint32_t>( state_.mandatory_prefix ) ) & invalid_check_mask_ ) != 0 ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Clear W flag" );
				writer.WriteLine( "state_.flags &= ~StateFlags::W;" );
				writer.WriteLine( "state_.extra_register_base_evex = 0;" );
				writer.WriteLine( "state_.extra_base_register_base_evex = 0;" );
				writer.WriteLine();
				writer.WriteLine( "// state_.modrm contains VEX byte2 (P0: RXBmmmmm)" );
				writer.WriteLine( "uint32_t p0 = state_.modrm;" );
				writer.WriteLine();
				writer.WriteLine( "// Read VEX byte3 (P1: WvvvvLpp) and opcode" );
				writer.WriteLine( "auto p1_opt = read_byte();" );
				writer.WriteLine( "if ( !p1_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t p1 = *p1_opt;" );
				writer.WriteLine();
				writer.WriteLine( "auto opcode_opt = read_byte();" );
				writer.WriteLine( "if ( !opcode_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t opcode = *opcode_opt;" );
				writer.WriteLine();
				writer.WriteLine( "// Extract P1 fields:" );
				writer.WriteLine( "// Bit 7: W (REX.W equivalent)" );
				writer.WriteLine( "// Bits 6-3: ~vvvv (inverted register specifier)" );
				writer.WriteLine( "// Bit 2: L (vector length)" );
				writer.WriteLine( "// Bits 1-0: pp (implied mandatory prefix)" );
				writer.WriteLine( "if ( ( p1 & 0x80 ) != 0 ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::W;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "state_.vector_length = static_cast<VectorLength>( ( p1 >> 2 ) & 1 );" );
				writer.WriteLine( "state_.mandatory_prefix = static_cast<DecoderMandatoryPrefix>( p1 & 3 );" );
				writer.WriteLine();
				writer.WriteLine( "uint32_t vvvv = ( ~p1 >> 3 ) & 0x0F;" );
				writer.WriteLine( "state_.vvvv_invalid_check = vvvv;" );
				writer.WriteLine( "state_.vvvv = vvvv & reg15_mask();" );
				writer.WriteLine();
				writer.WriteLine( "// Extract P0 fields (inverted R, X, B bits):" );
				writer.WriteLine( "// Bit 7: ~R, Bit 6: ~X, Bit 5: ~B" );
				writer.WriteLine( "// Bits 4-0: mmmmm (map select)" );
				writer.WriteLine( "uint32_t p0_inv = ~p0 & mask_e0_;" );
				writer.WriteLine( "state_.extra_register_base = ( p0_inv >> 4 ) & 8;" );
				writer.WriteLine( "state_.extra_index_register_base = ( p0_inv >> 3 ) & 8;" );
				writer.WriteLine( "state_.extra_base_register_base = ( p0_inv >> 2 ) & 8;" );
				writer.WriteLine();
				writer.WriteLine( "// Map select: mmmmm field (1=0F, 2=0F38, 3=0F3A)" );
				writer.WriteLine( "uint32_t map = ( p0 & 0x1F );" );
				writer.WriteLine( "if ( map == 0 || map > 3 ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t map_index = map - 1;  // Convert to 0-based index" );
				writer.WriteLine();
				writer.WriteLine( "auto* table = get_vex_table( map_index );" );
				writer.WriteLine( "if ( !table || opcode >= table->size() ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "decode_table( (*table)[opcode], instruction );" );
			}
			writer.WriteLine( "}" );
			writer.WriteLine();
		}

		void WriteEvexDecodeMethod( FileWriter writer ) {
			writer.WriteLine( "void Decoder::decode_evex( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// Validate: no REX prefix and no mandatory prefix already set" );
				writer.WriteLine( "if ( ( ( ( state_.flags & StateFlags::HAS_REX ) |" );
				writer.WriteLine( "        static_cast<uint32_t>( state_.mandatory_prefix ) ) & invalid_check_mask_ ) != 0 ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// state_.modrm contains P0 (first EVEX payload byte)" );
				writer.WriteLine( "uint32_t p0 = state_.modrm;" );
				writer.WriteLine();
				writer.WriteLine( "// Read P1, P2, and opcode" );
				writer.WriteLine( "auto p1_opt = read_byte();" );
				writer.WriteLine( "if ( !p1_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t p1 = *p1_opt;" );
				writer.WriteLine();
				writer.WriteLine( "// Validate EVEX: P1 bit 2 must be 1" );
				writer.WriteLine( "if ( ( p1 & 0x04 ) == 0 ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "auto p2_opt = read_byte();" );
				writer.WriteLine( "if ( !p2_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t p2 = *p2_opt;" );
				writer.WriteLine();
				writer.WriteLine( "auto opcode_opt = read_byte();" );
				writer.WriteLine( "if ( !opcode_opt ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine( "uint32_t opcode = *opcode_opt;" );
				writer.WriteLine();
				writer.WriteLine( "// Extract P1 fields:" );
				writer.WriteLine( "// Bit 7: W" );
				writer.WriteLine( "// Bits 6-3: ~vvvv" );
				writer.WriteLine( "// Bit 2: must be 1 (already checked)" );
				writer.WriteLine( "// Bits 1-0: pp" );
				writer.WriteLine( "state_.mandatory_prefix = static_cast<DecoderMandatoryPrefix>( p1 & 3 );" );
				writer.WriteLine( "if ( ( p1 & 0x80 ) != 0 ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::W;" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  state_.flags &= ~StateFlags::W;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Extract P2 fields:" );
				writer.WriteLine( "// Bit 7: z (zeroing-masking)" );
				writer.WriteLine( "// Bits 6-5: LL' (vector length)" );
				writer.WriteLine( "// Bit 4: b (broadcast/rounding)" );
				writer.WriteLine( "// Bit 3: V' (vvvv extension)" );
				writer.WriteLine( "// Bits 2-0: aaa (opmask register)" );
				writer.WriteLine( "state_.aaa = p2 & 7;" );
				writer.WriteLine( "instruction.set_op_mask( static_cast<Register>(" );
				writer.WriteLine( "  static_cast<uint32_t>( Register::K0 ) + state_.aaa ) );" );
				writer.WriteLine();
				writer.WriteLine( "if ( ( p2 & 0x80 ) != 0 ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::Z;" );
				writer.WriteLine( "  instruction.set_zeroing_masking( true );" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  state_.flags &= ~StateFlags::Z;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "if ( ( p2 & 0x10 ) != 0 ) {" );
				writer.WriteLine( "  state_.flags |= StateFlags::B;" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  state_.flags &= ~StateFlags::B;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "state_.vector_length = static_cast<VectorLength>( ( p2 >> 5 ) & 3 );" );
				writer.WriteLine();
				writer.WriteLine( "// vvvv from P1 and V' from P2" );
				writer.WriteLine( "uint32_t vvvv_low = ( ~p1 >> 3 ) & 0x0F;" );
				writer.WriteLine( "if ( bitness_ == 64 ) {" );
				writer.WriteLine( "  uint32_t v_prime = ( ~p2 & 8 ) << 1;  // V' bit -> bit 4" );
				writer.WriteLine( "  state_.extra_index_register_base_vsib = v_prime;" );
				writer.WriteLine( "  state_.vvvv = v_prime + vvvv_low;" );
				writer.WriteLine( "  state_.vvvv_invalid_check = state_.vvvv;" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  state_.vvvv = vvvv_low & 0x7;" );
				writer.WriteLine( "  state_.vvvv_invalid_check = vvvv_low;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Extract P0 fields (EVEX-specific R', X', B' extensions):" );
				writer.WriteLine( "// Bit 7: ~R, Bit 6: ~X, Bit 5: ~B, Bit 4: ~R'" );
				writer.WriteLine( "// Bit 3: must be 0 for EVEX (vs MVEX)" );
				writer.WriteLine( "// Bits 2-0: mm (map select)" );
				writer.WriteLine( "if ( ( p0 & 0x08 ) != 0 ) {" );
				writer.WriteLine( "  // Bit 3 must be 0 for EVEX" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "if ( bitness_ == 64 ) {" );
				writer.WriteLine( "  uint32_t p0_inv = ~p0;" );
				writer.WriteLine( "  state_.extra_register_base = ( p0_inv >> 4 ) & 8;       // R -> bit 3" );
				writer.WriteLine( "  state_.extra_index_register_base = ( p0_inv >> 3 ) & 8; // X -> bit 3" );
				writer.WriteLine( "  state_.extra_register_base_evex = p0_inv & 0x10;        // R' -> bit 4" );
				writer.WriteLine( "  state_.extra_base_register_base_evex = ( p0_inv >> 2 ) & 0x18; // X' and B'" );
				writer.WriteLine( "  state_.extra_base_register_base = ( p0_inv >> 2 ) & 8; // B -> bit 3" );
				writer.WriteLine( "} else {" );
				writer.WriteLine( "  state_.extra_register_base = 0;" );
				writer.WriteLine( "  state_.extra_index_register_base = 0;" );
				writer.WriteLine( "  state_.extra_register_base_evex = 0;" );
				writer.WriteLine( "  state_.extra_base_register_base_evex = 0;" );
				writer.WriteLine( "  state_.extra_base_register_base = 0;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Map select: mm field (1=0F, 2=0F38, 3=0F3A, 5=MAP5, 6=MAP6)" );
				writer.WriteLine( "uint32_t map = ( p0 & 0x07 );" );
				writer.WriteLine( "uint32_t map_index;" );
				writer.WriteLine( "switch ( map ) {" );
				writer.WriteLine( "  case 1: map_index = 0; break;  // 0F" );
				writer.WriteLine( "  case 2: map_index = 1; break;  // 0F38" );
				writer.WriteLine( "  case 3: map_index = 2; break;  // 0F3A" );
				writer.WriteLine( "  case 5: map_index = 4; break;  // MAP5" );
				writer.WriteLine( "  case 6: map_index = 5; break;  // MAP6" );
				writer.WriteLine( "  default:" );
				writer.WriteLine( "    set_invalid_instruction();" );
				writer.WriteLine( "    return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "auto* table = get_evex_table( map_index );" );
				writer.WriteLine( "if ( !table || opcode >= table->size() ) {" );
				writer.WriteLine( "  set_invalid_instruction();" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "decode_table( (*table)[opcode], instruction );" );
			}
			writer.WriteLine( "}" );

			// decode_xop
			writer.WriteLine();
			writer.WriteLine( "void Decoder::decode_xop( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// XOP prefix (0x8F followed by XOP-specific bytes)" );
				writer.WriteLine( "// XOP uses same basic structure as VEX3 but different map values" );
				writer.WriteLine( "// For now, mark as invalid - XOP is AMD-specific and rarely used" );
				writer.WriteLine( "set_invalid_instruction();" );
			}
			writer.WriteLine( "}" );

			// decode_3dnow
			writer.WriteLine();
			writer.WriteLine( "void Decoder::decode_3dnow( Instruction& instruction ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// 3DNow! instructions (0x0F 0x0F ... suffix)" );
				writer.WriteLine( "// These are legacy AMD instructions" );
				writer.WriteLine( "// For now, mark as invalid - 3DNow! is deprecated" );
				writer.WriteLine( "set_invalid_instruction();" );
			}
			writer.WriteLine( "}" );

			// read_op_mem_evex
			writer.WriteLine();
			writer.WriteLine( "void Decoder::read_op_mem_evex( Instruction& instruction, uint32_t operand_index, uint32_t tuple_type ) noexcept {" );
			using ( writer.Indent() ) {
				writer.WriteLine( "// EVEX memory operand with tuple type scaling for compressed displacement" );
				writer.WriteLine( "if ( state_.address_size == OpSize::SIZE16 ) {" );
				writer.WriteLine( "  read_op_mem_16( instruction, operand_index );" );
				writer.WriteLine( "  return;" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Base register for 32 vs 64-bit addressing" );
				writer.WriteLine( "Register base_reg = ( state_.address_size == OpSize::SIZE64 ) ? Register::RAX : Register::EAX;" );
				writer.WriteLine();
				writer.WriteLine( "if ( state_.mod_ == 0 ) {" );
				writer.WriteLine( "  // No displacement (except special cases)" );
				writer.WriteLine( "  if ( state_.rm == 4 ) {" );
				writer.WriteLine( "    // SIB byte" );
				writer.WriteLine( "    read_sib( instruction );" );
				writer.WriteLine( "  } else if ( state_.rm == 5 ) {" );
				writer.WriteLine( "    // RIP/EIP-relative or disp32" );
				writer.WriteLine( "    auto disp = read_u32();" );
				writer.WriteLine( "    if ( !disp ) return;" );
				writer.WriteLine( "    instruction.set_memory_displacement64( static_cast<int32_t>( *disp ) );" );
				writer.WriteLine( "    instruction.set_memory_displ_size( 4 );" );
				writer.WriteLine( "    if ( bitness_ == 64 ) {" );
				writer.WriteLine( "      instruction.set_memory_base( Register::RIP );" );
				writer.WriteLine( "      state_.flags |= StateFlags::IP_REL64;" );
				writer.WriteLine( "    } else if ( state_.address_size == OpSize::SIZE64 ) {" );
				writer.WriteLine( "      instruction.set_memory_base( Register::EIP );" );
				writer.WriteLine( "      state_.flags |= StateFlags::IP_REL32;" );
				writer.WriteLine( "    }" );
				writer.WriteLine( "  } else {" );
				writer.WriteLine( "    // Simple base register" );
				writer.WriteLine( "    instruction.set_memory_base( static_cast<Register>(" );
				writer.WriteLine( "      static_cast<uint32_t>( base_reg ) + state_.rm + state_.extra_base_register_base + state_.extra_base_register_base_evex ) );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "} else if ( state_.mod_ == 1 ) {" );
				writer.WriteLine( "  // 8-bit displacement with EVEX compressed displacement scaling" );
				writer.WriteLine( "  if ( state_.rm == 4 ) {" );
				writer.WriteLine( "    read_sib( instruction );" );
				writer.WriteLine( "  } else {" );
				writer.WriteLine( "    instruction.set_memory_base( static_cast<Register>(" );
				writer.WriteLine( "      static_cast<uint32_t>( base_reg ) + state_.rm + state_.extra_base_register_base + state_.extra_base_register_base_evex ) );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  auto disp = read_byte();" );
				writer.WriteLine( "  if ( !disp ) return;" );
				writer.WriteLine( "  int32_t scaled_disp = static_cast<int8_t>( *disp );" );
				writer.WriteLine( "  if ( tuple_type != 0 ) {" );
				writer.WriteLine( "    scaled_disp *= static_cast<int32_t>( tuple_type );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  instruction.set_memory_displacement64( scaled_disp );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 1 );" );
				writer.WriteLine( "} else if ( state_.mod_ == 2 ) {" );
				writer.WriteLine( "  // 32-bit displacement (no scaling)" );
				writer.WriteLine( "  if ( state_.rm == 4 ) {" );
				writer.WriteLine( "    read_sib( instruction );" );
				writer.WriteLine( "  } else {" );
				writer.WriteLine( "    instruction.set_memory_base( static_cast<Register>(" );
				writer.WriteLine( "      static_cast<uint32_t>( base_reg ) + state_.rm + state_.extra_base_register_base + state_.extra_base_register_base_evex ) );" );
				writer.WriteLine( "  }" );
				writer.WriteLine( "  auto disp = read_u32();" );
				writer.WriteLine( "  if ( !disp ) return;" );
				writer.WriteLine( "  instruction.set_memory_displacement64( static_cast<int32_t>( *disp ) );" );
				writer.WriteLine( "  instruction.set_memory_displ_size( 4 );" );
				writer.WriteLine( "}" );
				writer.WriteLine();
				writer.WriteLine( "// Set operand kind based on operand_index" );
				writer.WriteLine( "switch ( operand_index ) {" );
				writer.WriteLine( "  case 0: instruction.set_op0_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 1: instruction.set_op1_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 2: instruction.set_op2_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "  case 3: instruction.set_op3_kind( OpKind::MEMORY ); break;" );
				writer.WriteLine( "}" );
			}
			writer.WriteLine( "}" );
		}
	}
}
