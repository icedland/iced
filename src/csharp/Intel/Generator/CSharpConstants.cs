// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;
using System.Linq;
using Generator.Enums;

namespace Generator {
	static class CSharpConstants {
		public const string IcedNamespace = "Iced.Intel";
		public const string BlockEncoderNamespace = "Iced.Intel.BlockEncoderInternal";
		public const string DecoderNamespace = "Iced.Intel.DecoderInternal";
		public const string EncoderNamespace = "Iced.Intel.EncoderInternal";
		public const string InstructionInfoNamespace = "Iced.Intel.InstructionInfoInternal";
		public const string FormatterNamespace = "Iced.Intel.FormatterInternal";
		public const string GasFormatterNamespace = "Iced.Intel.GasFormatterInternal";
		public const string IntelFormatterNamespace = "Iced.Intel.IntelFormatterInternal";
		public const string MasmFormatterNamespace = "Iced.Intel.MasmFormatterInternal";
		public const string NasmFormatterNamespace = "Iced.Intel.NasmFormatterInternal";
		public const string FastFormatterNamespace = "Iced.Intel.FastFormatterInternal";

		public const string IcedUnitTestsNamespace = "Iced.UnitTests.Intel";

		public const string HasSpanDefine = "HAS_SPAN";
		public const string DecoderDefine = "DECODER";
		public const string VexDefine = "!NO_VEX";
		public const string XopDefine = "!NO_XOP";
		public const string EvexDefine = "!NO_EVEX";
		public const string MvexDefine = "MVEX";
		public const string D3nowDefine = "!NO_D3NOW";
		public const string DecoderVexDefine = "DECODER && !NO_VEX";
		public const string DecoderXopDefine = "DECODER && !NO_XOP";
		public const string DecoderVexOrXopDefine = "DECODER && (!NO_VEX || !NO_XOP)";
		public const string DecoderEvexDefine = "DECODER && !NO_EVEX";
		public const string DecoderMvexDefine = "DECODER && MVEX";
		public const string EncoderDefine = "ENCODER";
		public const string CodeAssemblerDefine = "ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER";
		public const string OpCodeInfoDefine = "ENCODER && OPCODE_INFO";
		public const string InstructionInfoDefine = "INSTR_INFO";
		public const string DecoderOrEncoderDefine = "DECODER || ENCODER";
		public const string DecoderOrEncoderOrOpCodeInfoDefine = "DECODER || ENCODER || (ENCODER && OPCODE_INFO)";
		public const string DecoderOrEncoderOrInstrInfoOrOpCodeInfoDefine = "DECODER || ENCODER || INSTR_INFO || (ENCODER && OPCODE_INFO)";
		public const string AnyFormatterDefine = "GAS || INTEL || MASM || NASM || FAST_FMT";
		public const string GasIntelNasmFormatterDefine = "GAS || INTEL || NASM";
		public const string GasFormatterDefine = "GAS";
		public const string IntelFormatterDefine = "INTEL";
		public const string MasmFormatterDefine = "MASM";
		public const string NasmFormatterDefine = "NASM";
		public const string FastFormatterDefine = "FAST_FMT";

		public const string PragmaMissingDocsDisable = "#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member";
		public const string PragmaMissingDocsRestore = "#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member";

		public static string GetFilename(GenTypes genTypes, string @namespace, params string[] names) =>
			Path.Combine(new[] { genTypes.Dirs.CSharpDir }.Concat(@namespace.Split('.').Skip(1)).Concat(names).ToArray());

		public static string? GetDefine(EncodingKind encoding) =>
			encoding switch {
				EncodingKind.Legacy => null,
				EncodingKind.VEX => VexDefine,
				EncodingKind.EVEX => EvexDefine,
				EncodingKind.XOP => XopDefine,
				EncodingKind.D3NOW => D3nowDefine,
				EncodingKind.MVEX => MvexDefine,
				_ => throw new InvalidOperationException(),
			};
	}
}
