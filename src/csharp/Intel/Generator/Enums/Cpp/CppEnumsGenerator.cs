// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator.Documentation.Cpp;
using Generator.IO;

namespace Generator.Enums.Cpp {
	[Generator( TargetLanguage.Cpp )]
	sealed class CppEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter idConverter;
		readonly CppDocCommentWriter docWriter;
		readonly CppDeprecatedWriter deprecatedWriter;
		readonly Dictionary<TypeId, FullEnumFileInfo?> toFullFileInfo;

		sealed class FullEnumFileInfo {
			public readonly string Filename;
			public readonly string[] ExtraIncludes;
			public readonly bool IsInternal;
			public readonly string? Id;

			public FullEnumFileInfo( string filename, bool isInternal = false, string? id = null, params string[] extraIncludes ) {
				Filename = filename;
				IsInternal = isInternal;
				Id = id;
				ExtraIncludes = extraIncludes;
			}
		}

		public CppEnumsGenerator( GeneratorContext generatorContext )
			: base( generatorContext.Types ) {
			idConverter = CppIdentifierConverter.Create();
			docWriter = new CppDocCommentWriter( idConverter );
			deprecatedWriter = new CppDeprecatedWriter( idConverter );

			toFullFileInfo = new();

			// Public enums (in include/iced_x86/)
			toFullFileInfo.Add( TypeIds.Code, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "code.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.CodeSize, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "code_size.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.ConditionCode, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "condition_code.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.CpuidFeature, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "cpuid_feature.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.DecoderError, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "decoder_error.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.DecoderOptions, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "decoder_options.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.EncodingKind, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "encoding_kind.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.FlowControl, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "flow_control.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.MemorySize, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "memory_size.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.Mnemonic, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "mnemonic.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.OpAccess, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "op_access.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.OpCodeOperandKind, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "op_code_operand_kind.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.OpCodeTableKind, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "op_code_table_kind.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.OpKind, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "op_kind.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.Register, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "register.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.RoundingControl, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "rounding_control.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.TupleType, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "tuple_type.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.MandatoryPrefix, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "mandatory_prefix.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.RflagsBits, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "rflags_bits.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.MvexConvFn, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "mvex_conv_fn.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.MvexEHBit, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "mvex_eh_bit.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.MvexRegMemConv, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "mvex_reg_mem_conv.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.MvexTupleTypeLutKind, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "mvex_tuple_type_lut_kind.hpp" ) ) );
			toFullFileInfo.Add( TypeIds.RepPrefixKind, new FullEnumFileInfo( CppConstants.GetHeaderFilename( genTypes, "rep_prefix_kind.hpp" ) ) );

			// Internal enums (in include/iced_x86/internal/)
			toFullFileInfo.Add( TypeIds.CpuidFeatureInternal, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "cpuid_feature_internal.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.HandlerFlags, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "handler_flags.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.LegacyHandlerFlags, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "legacy_handler_flags.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.LegacyOpCodeHandlerKind, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "legacy_op_code_handler_kind.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.VexOpCodeHandlerKind, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "vex_op_code_handler_kind.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.EvexOpCodeHandlerKind, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "evex_op_code_handler_kind.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.MvexOpCodeHandlerKind, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "mvex_op_code_handler_kind.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.SerializedDataKind, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "serialized_data_kind.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.OpSize, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "op_size.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.StateFlags, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "state_flags.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.ImpliedAccess, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "implied_access.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.RflagsInfo, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "rflags_info.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.OpInfo0, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "op_info0.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.OpInfo1, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "op_info1.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.OpInfo2, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "op_info2.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.OpInfo3, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "op_info3.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.OpInfo4, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "op_info4.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.InfoFlags1, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "info_flags1.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.InfoFlags2, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "info_flags2.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.VectorLength, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "vector_length.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.MandatoryPrefixByte, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "mandatory_prefix_byte.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.MvexInfoFlags1, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "mvex_info_flags1.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.MvexInfoFlags2, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "mvex_info_flags2.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.InstrFlags1, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "instr_flags1.hpp" ), true ) );
			toFullFileInfo.Add( TypeIds.MvexInstrFlags, new FullEnumFileInfo( CppConstants.GetInternalHeaderFilename( genTypes, "mvex_instr_flags.hpp" ), true ) );

			// Skip enums we don't need for decoder
			toFullFileInfo.Add( TypeIds.PseudoOpsKind, null );
			toFullFileInfo.Add( TypeIds.FormatterFlowControl, null );
			toFullFileInfo.Add( TypeIds.GasCtorKind, null );
			toFullFileInfo.Add( TypeIds.GasSizeOverride, null );
			toFullFileInfo.Add( TypeIds.GasInstrOpInfoFlags, null );
			toFullFileInfo.Add( TypeIds.GasInstrOpKind, null );
			toFullFileInfo.Add( TypeIds.IntelCtorKind, null );
			toFullFileInfo.Add( TypeIds.IntelSizeOverride, null );
			toFullFileInfo.Add( TypeIds.IntelBranchSizeInfo, null );
			toFullFileInfo.Add( TypeIds.IntelInstrOpInfoFlags, null );
			toFullFileInfo.Add( TypeIds.IntelInstrOpKind, null );
			toFullFileInfo.Add( TypeIds.MasmCtorKind, null );
			toFullFileInfo.Add( TypeIds.MasmInstrOpInfoFlags, null );
			toFullFileInfo.Add( TypeIds.MasmInstrOpKind, null );
			toFullFileInfo.Add( TypeIds.MasmSymbolTestFlags, null );
			toFullFileInfo.Add( TypeIds.NasmCtorKind, null );
			toFullFileInfo.Add( TypeIds.NasmSignExtendInfo, null );
			toFullFileInfo.Add( TypeIds.NasmSizeOverride, null );
			toFullFileInfo.Add( TypeIds.NasmBranchSizeInfo, null );
			toFullFileInfo.Add( TypeIds.NasmInstrOpInfoFlags, null );
			toFullFileInfo.Add( TypeIds.NasmInstrOpKind, null );
			toFullFileInfo.Add( TypeIds.NasmMemorySizeInfo, null );
			toFullFileInfo.Add( TypeIds.NasmFarMemorySizeInfo, null );
			toFullFileInfo.Add( TypeIds.FastFmtFlags, null );
			toFullFileInfo.Add( TypeIds.NumberBase, null );
			toFullFileInfo.Add( TypeIds.MemorySizeOptions, null );
			toFullFileInfo.Add( TypeIds.FormatMnemonicOptions, null );
			toFullFileInfo.Add( TypeIds.PrefixKind, null );
			toFullFileInfo.Add( TypeIds.DecoratorKind, null );
			toFullFileInfo.Add( TypeIds.NumberKind, null );
			toFullFileInfo.Add( TypeIds.FormatterTextKind, null );
			toFullFileInfo.Add( TypeIds.SymbolFlags, null );
			toFullFileInfo.Add( TypeIds.CC_b, null );
			toFullFileInfo.Add( TypeIds.CC_ae, null );
			toFullFileInfo.Add( TypeIds.CC_e, null );
			toFullFileInfo.Add( TypeIds.CC_ne, null );
			toFullFileInfo.Add( TypeIds.CC_be, null );
			toFullFileInfo.Add( TypeIds.CC_a, null );
			toFullFileInfo.Add( TypeIds.CC_p, null );
			toFullFileInfo.Add( TypeIds.CC_np, null );
			toFullFileInfo.Add( TypeIds.CC_l, null );
			toFullFileInfo.Add( TypeIds.CC_ge, null );
			toFullFileInfo.Add( TypeIds.CC_le, null );
			toFullFileInfo.Add( TypeIds.CC_g, null );
			toFullFileInfo.Add( TypeIds.OptionsProps, null );
			toFullFileInfo.Add( TypeIds.DecoderTestOptions, null );

			// Skip encoder-only enums for now
			toFullFileInfo.Add( TypeIds.LegacyOpCodeTable, null );
			toFullFileInfo.Add( TypeIds.VexOpCodeTable, null );
			toFullFileInfo.Add( TypeIds.XopOpCodeTable, null );
			toFullFileInfo.Add( TypeIds.EvexOpCodeTable, null );
			toFullFileInfo.Add( TypeIds.MvexOpCodeTable, null );
			toFullFileInfo.Add( TypeIds.DisplSize, null );
			toFullFileInfo.Add( TypeIds.ImmSize, null );
			toFullFileInfo.Add( TypeIds.EncoderFlags, null );
			toFullFileInfo.Add( TypeIds.EncFlags1, null );
			toFullFileInfo.Add( TypeIds.EncFlags2, null );
			toFullFileInfo.Add( TypeIds.EncFlags3, null );
			toFullFileInfo.Add( TypeIds.OpCodeInfoFlags1, null );
			toFullFileInfo.Add( TypeIds.OpCodeInfoFlags2, null );
			toFullFileInfo.Add( TypeIds.DecOptionValue, null );
			toFullFileInfo.Add( TypeIds.InstrStrFmtOption, null );
			toFullFileInfo.Add( TypeIds.WBit, null );
			toFullFileInfo.Add( TypeIds.LBit, null );
			toFullFileInfo.Add( TypeIds.LKind, null );
			toFullFileInfo.Add( TypeIds.RelocKind, null );
			toFullFileInfo.Add( TypeIds.BlockEncoderOptions, null );
			toFullFileInfo.Add( TypeIds.CodeAsmMemoryOperandSize, null );
			toFullFileInfo.Add( TypeIds.TestInstrFlags, null );
			toFullFileInfo.Add( TypeIds.MemorySizeFlags, null );
			toFullFileInfo.Add( TypeIds.RegisterFlags, null );
			toFullFileInfo.Add( TypeIds.InstrScale, null );
			toFullFileInfo.Add( TypeIds.FormatterSyntax, null );
		}

		public override void Generate( EnumType enumType ) {
			if ( toFullFileInfo.TryGetValue( enumType.TypeId, out var fullFileInfo ) ) {
				if ( fullFileInfo is not null )
					WriteFile( fullFileInfo, enumType );
			}
		}

		void WriteFile( FullEnumFileInfo info, EnumType enumType ) {
			// Ensure directory exists
			var dir = Path.GetDirectoryName( info.Filename );
			if ( !string.IsNullOrEmpty( dir ) && !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			using var writer = new FileWriter( TargetLanguage.Cpp, FileUtils.OpenWrite( info.Filename ) );
			writer.WriteFileHeader();

			var enumTypeName = enumType.Name( idConverter );
			var headerGuard = CppConstants.GetHeaderGuard( info.IsInternal ? new[] { "INTERNAL", enumType.RawName } : new[] { enumType.RawName } );

			writer.WriteLine( "#pragma once" );
			writer.WriteLine( $"#ifndef {headerGuard}" );
			writer.WriteLine( $"#define {headerGuard}" );
			writer.WriteLine();

			// Standard includes
			writer.WriteLine( "#include <cstdint>" );
			writer.WriteLine( "#include <cstddef>" );
			foreach ( var include in info.ExtraIncludes )
				writer.WriteLine( $"#include {include}" );
			writer.WriteLine();

			// Open namespace
			if ( info.IsInternal ) {
				writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
				writer.WriteLine( "namespace internal {" );
			}
			else {
				writer.WriteLine( $"namespace {CppConstants.Namespace} {{" );
			}
			writer.WriteLine();

			WriteEnum( writer, info, enumType );

			writer.WriteLine();
			// Close namespace
			if ( info.IsInternal ) {
				writer.WriteLine( "} // namespace internal" );
				writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			}
			else {
				writer.WriteLine( $"}} // namespace {CppConstants.Namespace}" );
			}
			writer.WriteLine();
			writer.WriteLine( $"#endif // {headerGuard}" );
		}

		void WriteEnum( FileWriter writer, FullEnumFileInfo info, EnumType enumType ) {
			var enumTypeName = enumType.Name( idConverter );

			// Write documentation
			docWriter.WriteSummary( writer, enumType.Documentation.GetComment( TargetLanguage.Cpp ), enumType.RawName );

			// Determine underlying type
			var underlyingType = GetUnderlyingType( enumType );

			// Identical enum values aren't allowed in C++ enum class, so remove deprecated renamed values
			var enumValues = enumType.Values.Where( a => !a.DeprecatedInfo.IsDeprecatedAndRenamed ).ToArray();

			// Check if this is a flags enum
			if ( enumType.IsFlags ) {
				// For flags enums, generate a namespace with constexpr values
				writer.WriteLine( $"namespace {enumTypeName} {{" );
				using ( writer.Indent() ) {
					writer.WriteLine( $"using Value = {underlyingType};" );
					writer.WriteLine();
					foreach ( var value in enumValues ) {
						docWriter.WriteSummary( writer, value.Documentation.GetComment( TargetLanguage.Cpp ), enumType.RawName );
						if ( value.DeprecatedInfo.IsDeprecated )
							deprecatedWriter.WriteDeprecated( writer, value );
						writer.WriteLine( $"constexpr Value {value.Name( idConverter )} = 0x{value.Value:X}U;" );
					}
				}
				writer.WriteLine( $"}} // namespace {enumTypeName}" );
			}
			else {
				// Regular enum class
				writer.WriteLine( $"enum class {enumTypeName} : {underlyingType} {{" );
				using ( writer.Indent() ) {
					for ( int i = 0; i < enumValues.Length; i++ ) {
						var value = enumValues[i];
						docWriter.WriteSummary( writer, value.Documentation.GetComment( TargetLanguage.Cpp ), enumType.RawName );
						var comma = i < enumValues.Length - 1 ? "," : "";
						// [[deprecated]] must be inline with the enumerator for MSVC compatibility
						if ( value.DeprecatedInfo.IsDeprecated ) {
							var deprecMsg = deprecatedWriter.GetDeprecatedString( value );
							if ( deprecMsg is not null )
								writer.WriteLine( $"{value.Name( idConverter )} [[deprecated( \"{deprecMsg}\" )]] = {value.Value}{comma}" );
							else
								writer.WriteLine( $"{value.Name( idConverter )} = {value.Value}{comma}" );
						}
						else {
							writer.WriteLine( $"{value.Name( idConverter )} = {value.Value}{comma}" );
						}
					}
				}
				writer.WriteLine( "};" );

				// Write count constant
				var countName = idConverter.Constant( enumType.RawName + "Count" );
				writer.WriteLine();
				writer.WriteLine( $"/// @brief Number of {enumTypeName} enum values." );
				writer.WriteLine( $"constexpr std::size_t {countName} = {enumValues.Length};" );
			}
		}

		static string GetUnderlyingType( EnumType enumType ) {
			var maxValue = enumType.Values.Where( a => !a.DeprecatedInfo.IsDeprecatedAndRenamed ).Max( a => a.Value );
			if ( enumType.IsFlags ) {
				// For flags, we need to accommodate the highest flag value
				if ( maxValue <= 0xFF )
					return "uint8_t";
				if ( maxValue <= 0xFFFF )
					return "uint16_t";
				return "uint32_t";
			}
			else {
				var count = enumType.Values.Count( a => !a.DeprecatedInfo.IsDeprecatedAndRenamed );
				if ( count <= 256 )
					return "uint8_t";
				if ( count <= 65536 )
					return "uint16_t";
				return "uint32_t";
			}
		}
	}
}
