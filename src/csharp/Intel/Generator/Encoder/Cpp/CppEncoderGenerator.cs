// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Enums;
using Generator.IO;
using Generator.Tables;

namespace Generator.Encoder.Cpp {
	[Generator(TargetLanguage.Cpp)]
	sealed class CppEncoderGenerator : EncoderGenerator {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;

		public CppEncoderGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = CppIdentifierConverter.Create();
		}

		protected override void Generate(EnumType enumType) {
			// Generate encoder-specific enums
			var filename = CppConstants.GetInternalHeaderFilename(genTypes, $"encoder_{idConverter.Type(enumType.Name(idConverter))}.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();
			WriteEnum(writer, enumType);
		}

		void WriteEnum(FileWriter writer, EnumType enumType) {
			var enumName = enumType.Name(idConverter);
			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", enumName.ToUpperInvariant());

			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();

			if (enumType.IsFlags) {
				writer.WriteLine($"/// @brief {enumType.Documentation}");
				writer.WriteLine($"struct {enumName} {{");
				using (writer.Indent()) {
					foreach (var value in enumType.Values) {
						writer.WriteLine($"static constexpr uint32_t {value.Name(idConverter)} = 0x{value.Value:X8}U;");
					}
				}
				writer.WriteLine("};");
			} else {
				writer.WriteLine($"/// @brief {enumType.Documentation}");
				writer.WriteLine($"enum class {enumName} : uint32_t {{");
				using (writer.Indent()) {
					foreach (var value in enumType.Values) {
						var comma = value == enumType.Values[^1] ? "" : ",";
						writer.WriteLine($"{value.Name(idConverter)} = {value.Value}{comma}");
					}
				}
				writer.WriteLine("};");
			}

			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");
		}

		[Flags]
		enum OpInfoFlags {
			None = 0,
			Legacy = 1,
			VEX = 2,
			EVEX = 4,
			XOP = 8,
			MVEX = 0x10,
		}

		sealed class OpInfo {
			public readonly OpHandlerKind OpHandlerKind;
			public readonly object[] Args;
			public readonly string Name;
			public OpInfoFlags Flags;
			public OpInfo(OpHandlerKind opHandlerKind, object[] args, string name) {
				OpHandlerKind = opHandlerKind;
				Args = args;
				Name = name;
				Flags = OpInfoFlags.None;
			}
		}

		sealed class OpKeyComparer : IEqualityComparer<(OpHandlerKind opHandlerKind, object[] args)> {
			public bool Equals((OpHandlerKind opHandlerKind, object[] args) x, (OpHandlerKind opHandlerKind, object[] args) y) {
				if (x.opHandlerKind != y.opHandlerKind)
					return false;
				var xa = x.args;
				var ya = y.args;
				if (xa.Length != ya.Length)
					return false;
				for (int i = 0; i < xa.Length; i++) {
					if (!Equals(xa[i], ya[i]))
						return false;
				}
				return true;
			}

			public int GetHashCode((OpHandlerKind opHandlerKind, object[] args) obj) {
				var args = obj.args;
				int hc = HashCode.Combine((int)obj.opHandlerKind, args.Length);
				for (int i = 0; i < args.Length; i++)
					hc = HashCode.Combine(args[i].GetHashCode());
				return hc;
			}
		}

		protected override void Generate(OpCodeHandlers handlers) {
			GenerateOpCodeOperandKindTables(handlers);
			GenerateOpTables(handlers);
		}

		void GenerateOpCodeOperandKindTables(OpCodeHandlers handlers) {
			var filename = CppConstants.GetInternalHeaderFilename(genTypes, "encoder_op_kind_tables.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", "OP_KIND_TABLES");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include \"iced_x86/op_code_operand_kind.hpp\"");
			writer.WriteLine("#include <array>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();

			Generate(writer, "LEGACY_OP_KINDS", handlers.Legacy);
			Generate(writer, "VEX_OP_KINDS", handlers.Vex);
			Generate(writer, "XOP_OP_KINDS", handlers.Xop);
			Generate(writer, "EVEX_OP_KINDS", handlers.Evex);
			Generate(writer, "MVEX_OP_KINDS", handlers.Mvex);

			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");

			void Generate(FileWriter writer, string name, (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				var declTypeStr = $"{CppConstants.Namespace}::OpCodeOperandKind";
				writer.WriteLine();
				writer.WriteLine($"inline constexpr std::array<{declTypeStr}, {table.Length}> {name} = {{{{");
				using (writer.Indent()) {
					foreach (var info in table)
						writer.WriteLine($"{declTypeStr}::{info.opCodeOperandKind.Name(idConverter)},");
				}
				writer.WriteLine("}};");
			}
		}

		void GenerateOpTables(OpCodeHandlers handlers) {
			var sb = new StringBuilder();
			var dict = new Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo>(new OpKeyComparer());
			Add(sb, dict, handlers.Legacy.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.Legacy);
			Add(sb, dict, handlers.Vex.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.VEX);
			Add(sb, dict, handlers.Xop.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.XOP);
			Add(sb, dict, handlers.Evex.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.EVEX);
			Add(sb, dict, handlers.Mvex.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.MVEX);

			var filename = CppConstants.GetInternalHeaderFilename(genTypes, "encoder_ops_tables.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", "OPS_TABLES");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include \"iced_x86/internal/encoder_ops.hpp\"");
			writer.WriteLine("#include \"iced_x86/register.hpp\"");
			writer.WriteLine("#include \"iced_x86/op_kind.hpp\"");
			writer.WriteLine("#include <array>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();

			// Generate static operand handler instances
			foreach (var kv in dict.OrderBy(a => a.Value.Name, StringComparer.Ordinal)) {
				var info = kv.Value;
				var structName = GetStructName(info.OpHandlerKind);
				WriteOpHandlerInstance(writer, info, structName);
			}

			writer.WriteLine();

			// Generate the lookup tables
			WriteTable(writer, "LEGACY_TABLE", dict, handlers.Legacy.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
			WriteTable(writer, "VEX_TABLE", dict, handlers.Vex.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
			WriteTable(writer, "XOP_TABLE", dict, handlers.Xop.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
			WriteTable(writer, "EVEX_TABLE", dict, handlers.Evex.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
			WriteTable(writer, "MVEX_TABLE", dict, handlers.Mvex.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));

			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");
		}

		void WriteOpHandlerInstance(FileWriter writer, OpInfo info, string structName) {
			writer.Write($"inline constexpr {structName} {info.Name}");

			switch (info.OpHandlerKind) {
			case OpHandlerKind.OpA:
				writer.WriteLine($" {{ {(int)info.Args[0]} }};");
				break;

			case OpHandlerKind.OpHx:
			case OpHandlerKind.OpIsX:
			case OpHandlerKind.OpModRM_reg:
			case OpHandlerKind.OpModRM_reg_mem:
			case OpHandlerKind.OpModRM_regF0:
			case OpHandlerKind.OpModRM_rm:
			case OpHandlerKind.OpModRM_rm_reg_only:
			case OpHandlerKind.OpRegEmbed8:
				writer.WriteLine($" {{ {CppConstants.Namespace}::Register::{((EnumValue)info.Args[0]).Name(idConverter)}, {CppConstants.Namespace}::Register::{((EnumValue)info.Args[1]).Name(idConverter)} }};");
				break;

			case OpHandlerKind.OpIb:
			case OpHandlerKind.OpId:
				writer.WriteLine($" {{ {CppConstants.Namespace}::OpKind::{((EnumValue)info.Args[0]).Name(idConverter)} }};");
				break;

			case OpHandlerKind.OpImm:
				writer.WriteLine($" {{ {(int)info.Args[0]} }};");
				break;

			case OpHandlerKind.OpJ:
				writer.WriteLine($" {{ {CppConstants.Namespace}::OpKind::{((EnumValue)info.Args[0]).Name(idConverter)}, {(int)info.Args[1]} }};");
				break;

			case OpHandlerKind.OpJdisp:
				writer.WriteLine($" {{ {(int)info.Args[0]} }};");
				break;

			case OpHandlerKind.OpJx:
				writer.WriteLine($" {{ {(int)info.Args[0]} }};");
				break;

			case OpHandlerKind.OpReg:
				writer.WriteLine($" {{ {CppConstants.Namespace}::Register::{((EnumValue)info.Args[0]).Name(idConverter)} }};");
				break;

			case OpHandlerKind.OpVsib:
				writer.WriteLine($" {{ {CppConstants.Namespace}::Register::{((EnumValue)info.Args[0]).Name(idConverter)}, {CppConstants.Namespace}::Register::{((EnumValue)info.Args[1]).Name(idConverter)} }};");
				break;

			case OpHandlerKind.None:
			case OpHandlerKind.OpI4:
			case OpHandlerKind.OpIq:
			case OpHandlerKind.OpIw:
			case OpHandlerKind.OpMRBX:
			case OpHandlerKind.OpO:
			case OpHandlerKind.OprDI:
			case OpHandlerKind.OpRegSTi:
			case OpHandlerKind.OpX:
			case OpHandlerKind.OpY:
				writer.WriteLine(" {};");
				break;

			case OpHandlerKind.OpModRM_rm_mem_only:
				writer.WriteLine($" {{ {((bool)info.Args[0] ? "true" : "false")} }};");
				break;

			default:
				throw new InvalidOperationException($"Unknown OpHandlerKind: {info.OpHandlerKind}");
			}
		}

		void WriteTable(FileWriter writer, string name, Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo> dict,
			IEnumerable<(EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)> values) {
			var all = values.ToArray();
			writer.WriteLine($"inline constexpr std::array<const Op*, {all.Length}> {name} = {{{{");
			using (writer.Indent()) {
				foreach (var value in all) {
					var info = dict[(value.opHandlerKind, value.args)];
					writer.WriteLine($"&{info.Name}, // {value.opKind.Name(idConverter)}");
				}
			}
			writer.WriteLine("}};");
			writer.WriteLine();
		}

		void Add(StringBuilder sb, Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo> dict,
			IEnumerable<(OpHandlerKind opHandlerKind, object[] args)> values, OpInfoFlags flags) {
			foreach (var value in values) {
				if (!dict.TryGetValue(value, out var opInfo))
					dict.Add(value, opInfo = new OpInfo(value.opHandlerKind, value.args, GetName(sb, value.opHandlerKind, value.args)));
				opInfo.Flags |= flags;
			}
		}

		string GetName(StringBuilder sb, OpHandlerKind opHandlerKind, object[] args) {
			sb.Clear();
			sb.Append(opHandlerKind.ToString());
			foreach (var obj in args) {
				sb.Append('_');
				switch (obj) {
				case EnumValue value:
					sb.Append(value.RawName);
					break;
				case int value:
					sb.Append(value);
					break;
				case bool value:
					sb.Append(value ? "true" : "false");
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			return idConverter.Static(sb.ToString());
		}

		static string GetStructName(OpHandlerKind kind) {
			if (kind == OpHandlerKind.None)
				return "InvalidOpHandler";
			return kind.ToString();
		}

		protected override void GenerateOpCodeInfo(InstructionDef[] defs,
			(MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexTupleTypeData,
			(MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexMemorySizeData) {
			GenerateEncoderDataTables(defs);
		}

		void GenerateEncoderDataTables(InstructionDef[] defs) {
			var allData = GetData(defs).ToArray();
			var encFlags1 = allData.Select(a => (a.def, a.encFlags1)).ToArray();
			var encFlags2 = allData.Select(a => (a.def, a.encFlags2)).ToArray();
			var encFlags3 = allData.Select(a => (a.def, a.encFlags3)).ToArray();
			var opcFlags1 = allData.Select(a => (a.def, a.opcFlags1)).ToArray();
			var opcFlags2 = allData.Select(a => (a.def, a.opcFlags2)).ToArray();

			var filename = CppConstants.GetInternalHeaderFilename(genTypes, "encoder_data.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", "DATA");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine("#include <array>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();

			WriteDataTable(writer, "ENC_FLAGS1", encFlags1);
			WriteDataTable(writer, "ENC_FLAGS2", encFlags2);
			WriteDataTable(writer, "ENC_FLAGS3", encFlags3);
			WriteDataTable(writer, "OPC_FLAGS1", opcFlags1);
			WriteDataTable(writer, "OPC_FLAGS2", opcFlags2);

			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");
		}

		void WriteDataTable(FileWriter writer, string name, (InstructionDef def, uint value)[] values) {
			writer.WriteLine($"inline constexpr std::array<uint32_t, {values.Length}> {name} = {{{{");
			using (writer.Indent()) {
				foreach (var (def, value) in values)
					writer.WriteLine($"0x{value:X8}U, // {def.Code.Name(idConverter)}");
			}
			writer.WriteLine("}};");
			writer.WriteLine();
		}

		protected override void Generate((EnumValue value, uint size)[] immSizes) {
			var filename = CppConstants.GetInternalHeaderFilename(genTypes, "encoder_imm_sizes.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", "IMM_SIZES");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine("#include <array>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();
			writer.WriteLine($"inline constexpr std::array<uint32_t, {immSizes.Length}> IMM_SIZES = {{{{");
			using (writer.Indent()) {
				foreach (var (value, size) in immSizes)
					writer.WriteLine($"{size}, // {value.Name(idConverter)}");
			}
			writer.WriteLine("}};");
			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");
		}

		protected override void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings) {
			// Not needed for initial implementation
		}

		protected override void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib) {
			// Not needed for initial implementation
		}

		protected override void GenerateCore() {
			GenerateEncoderEnums();
		}

		void GenerateEncoderEnums() {
			// Generate DisplSize enum
			GenerateDisplSizeEnum();
			// Generate ImmSize enum
			GenerateImmSizeEnum();
			// Generate EncoderFlags struct
			GenerateEncoderFlagsStruct();
		}

		void GenerateDisplSizeEnum() {
			var filename = CppConstants.GetInternalHeaderFilename(genTypes, "encoder_displ_size.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", "DISPL_SIZE");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();
			writer.WriteLine("/// @brief Displacement size for encoder");
			writer.WriteLine("enum class DisplSize : uint8_t {");
			using (writer.Indent()) {
				writer.WriteLine("NONE = 0,");
				writer.WriteLine("SIZE1 = 1,");
				writer.WriteLine("SIZE2 = 2,");
				writer.WriteLine("SIZE4 = 3,");
				writer.WriteLine("SIZE8 = 4,");
				writer.WriteLine("RIP_REL_SIZE4_TARGET32 = 5,");
				writer.WriteLine("RIP_REL_SIZE4_TARGET64 = 6");
			}
			writer.WriteLine("};");
			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");
		}

		void GenerateImmSizeEnum() {
			var filename = CppConstants.GetInternalHeaderFilename(genTypes, "encoder_imm_size.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", "IMM_SIZE");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();
			writer.WriteLine("/// @brief Immediate size for encoder");
			writer.WriteLine("enum class ImmSize : uint8_t {");
			using (writer.Indent()) {
				writer.WriteLine("NONE = 0,");
				writer.WriteLine("SIZE1 = 1,");
				writer.WriteLine("SIZE2 = 2,");
				writer.WriteLine("SIZE4 = 3,");
				writer.WriteLine("SIZE8 = 4,");
				writer.WriteLine("SIZE2_1 = 5,   // ENTER xxxx,yy");
				writer.WriteLine("SIZE1_1 = 6,   // EXTRQ/INSERTQ xx,yy");
				writer.WriteLine("SIZE2_2 = 7,   // CALL16 FAR x:y");
				writer.WriteLine("SIZE4_2 = 8,   // CALL32 FAR x:y");
				writer.WriteLine("RIP_REL_SIZE1_TARGET16 = 9,");
				writer.WriteLine("RIP_REL_SIZE1_TARGET32 = 10,");
				writer.WriteLine("RIP_REL_SIZE1_TARGET64 = 11,");
				writer.WriteLine("RIP_REL_SIZE2_TARGET16 = 12,");
				writer.WriteLine("RIP_REL_SIZE2_TARGET32 = 13,");
				writer.WriteLine("RIP_REL_SIZE2_TARGET64 = 14,");
				writer.WriteLine("RIP_REL_SIZE4_TARGET32 = 15,");
				writer.WriteLine("RIP_REL_SIZE4_TARGET64 = 16,");
				writer.WriteLine("SIZE_IB_REG = 17,");
				writer.WriteLine("SIZE1_OP_CODE = 18");
			}
			writer.WriteLine("};");
			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");
		}

		void GenerateEncoderFlagsStruct() {
			var filename = CppConstants.GetInternalHeaderFilename(genTypes, "encoder_flags.hpp");
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(TargetLanguage.Cpp, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();

			var headerGuard = CppConstants.GetHeaderGuard("ENCODER", "FLAGS");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.InternalNamespace} {{");
			writer.WriteLine();
			writer.WriteLine("/// @brief Encoder internal flags");
			writer.WriteLine("struct EncoderFlags {");
			using (writer.Indent()) {
				writer.WriteLine("static constexpr uint32_t NONE = 0x00000000U;");
				writer.WriteLine("static constexpr uint32_t B = 0x00000001U;");
				writer.WriteLine("static constexpr uint32_t X = 0x00000002U;");
				writer.WriteLine("static constexpr uint32_t R = 0x00000004U;");
				writer.WriteLine("static constexpr uint32_t W = 0x00000008U;");
				writer.WriteLine("static constexpr uint32_t MOD_RM = 0x00000010U;");
				writer.WriteLine("static constexpr uint32_t SIB = 0x00000020U;");
				writer.WriteLine("static constexpr uint32_t REX = 0x00000040U;");
				writer.WriteLine("static constexpr uint32_t P66 = 0x00000080U;");
				writer.WriteLine("static constexpr uint32_t P67 = 0x00000100U;");
				writer.WriteLine("static constexpr uint32_t R2 = 0x00000200U;  // EVEX.R'");
				writer.WriteLine("static constexpr uint32_t BROADCAST = 0x00000400U;");
				writer.WriteLine("static constexpr uint32_t HIGH_LEGACY_8_BIT_REGS = 0x00000800U;");
				writer.WriteLine("static constexpr uint32_t DISPL = 0x00001000U;");
				writer.WriteLine("static constexpr uint32_t PF0 = 0x00002000U;");
				writer.WriteLine("static constexpr uint32_t REG_IS_MEMORY = 0x00004000U;");
				writer.WriteLine("static constexpr uint32_t MUST_USE_SIB = 0x00008000U;");
				writer.WriteLine("static constexpr uint32_t VVVVV_SHIFT = 0x0000001BU;");
				writer.WriteLine("static constexpr uint32_t VVVVV_MASK = 0x0000001FU;");
			}
			writer.WriteLine("};");
			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.InternalNamespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {headerGuard}");
		}

		protected override void GenerateInstrSwitch(EnumValue[] jccInstr, EnumValue[] simpleBranchInstr, EnumValue[] callInstr, EnumValue[] jmpInstr, EnumValue[] xbeginInstr) {
			// Not needed for initial implementation
		}

		protected override void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64) {
			// Not needed for initial implementation
		}

		protected override void GenerateDecoderOptionsTable((EnumValue decOptionValue, EnumValue decoderOptions)[] values) {
			// Not needed for initial implementation
		}

		protected override void GenerateImpliedOps((EncodingKind Encoding, InstrStrImpliedOp[] Ops, InstructionDef[] defs)[] impliedOpsInfo) {
			// Not needed for initial implementation
		}
	}
}
