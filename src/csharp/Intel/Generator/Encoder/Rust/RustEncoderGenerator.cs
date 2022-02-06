// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.Enums;
using Generator.Enums.Rust;
using Generator.IO;
using Generator.Tables;

namespace Generator.Encoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustEncoderGenerator : EncoderGenerator {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;
		readonly RustEnumsGenerator enumGenerator;

		public RustEncoderGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = RustIdentifierConverter.Create();
			enumGenerator = new RustEnumsGenerator(generatorContext);
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);

		[Flags]
		enum OpInfoFlags {
			None			= 0,
			Legacy			= 1,
			VEX				= 2,
			EVEX			= 4,
			XOP				= 8,
			MVEX			= 0x10,
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
			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder", "op_kind_tables.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				writer.WriteLine("use crate::OpCodeOperandKind;");
				Generate(writer, "LEGACY_OP_KINDS", null, handlers.Legacy);
				Generate(writer, "VEX_OP_KINDS", RustConstants.FeatureVex, handlers.Vex);
				Generate(writer, "XOP_OP_KINDS", RustConstants.FeatureXop, handlers.Xop);
				Generate(writer, "EVEX_OP_KINDS", RustConstants.FeatureEvex, handlers.Evex);
				Generate(writer, "MVEX_OP_KINDS", RustConstants.FeatureMvex, handlers.Mvex);
			}

			void Generate(FileWriter writer, string name, string? feature, (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				var declTypeStr = genTypes[TypeIds.OpCodeOperandKind].Name(idConverter);
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				if (feature is not null)
					writer.WriteLine(feature);
				writer.WriteLine($"pub(super) static {name}: [{declTypeStr}; {table.Length}] = [");
				using (writer.Indent()) {
					foreach (var info in table)
						writer.WriteLine($"{idConverter.ToDeclTypeAndValue(info.opCodeOperandKind)},");
				}
				writer.WriteLine("];");
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

			var usedNames = new HashSet<string>(dict.Count, StringComparer.Ordinal);
			foreach (var kv in dict) {
				if (!usedNames.Add(kv.Value.Name))
					throw new InvalidOperationException();
			}

			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder", "ops_tables.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("use crate::encoder::ops::*;");
				writer.WriteLine("use crate::*;");
				writer.WriteLine();

				foreach (var kv in dict.OrderBy(a => a.Value.Name, StringComparer.Ordinal)) {
					var info = kv.Value;
					var structName = idConverter.Type(GetStructName(info.OpHandlerKind));
					var features = GetFeatures(info);
					if (features is not null)
						writer.WriteLine(features);
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.Write($"static {info.Name}: {structName} = {structName}");
					switch (info.OpHandlerKind) {
					case OpHandlerKind.OpA:
						if (info.Args.Length != 1)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent())
							writer.WriteLine($"size: {(int)info.Args[0]}");
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpHx:
					case OpHandlerKind.OpIsX:
					case OpHandlerKind.OpModRM_reg:
					case OpHandlerKind.OpModRM_reg_mem:
					case OpHandlerKind.OpModRM_regF0:
					case OpHandlerKind.OpModRM_rm:
					case OpHandlerKind.OpModRM_rm_reg_only:
					case OpHandlerKind.OpRegEmbed8:
						if (info.Args.Length != 2)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent()) {
							WriteField(writer, "reg_lo", (EnumValue)info.Args[0]);
							WriteField(writer, "reg_hi", (EnumValue)info.Args[1]);
						}
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpIb:
					case OpHandlerKind.OpId:
						if (info.Args.Length != 1)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent())
							WriteField(writer, "op_kind", (EnumValue)info.Args[0]);
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpImm:
						if (info.Args.Length != 1)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent())
							writer.WriteLine($"value: {(int)info.Args[0]}");
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpJ:
						if (info.Args.Length != 2)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent()) {
							WriteField(writer, "op_kind", (EnumValue)info.Args[0]);
							writer.WriteLine($"imm_size: {(int)info.Args[1]}");
						}
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpJdisp:
						if (info.Args.Length != 1)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent())
							writer.WriteLine($"displ_size: {(int)info.Args[0]}");
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpJx:
						if (info.Args.Length != 1)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent())
							writer.WriteLine($"imm_size: {(int)info.Args[0]}");
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpReg:
						if (info.Args.Length != 1)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent())
							WriteField(writer, "register", (EnumValue)info.Args[0]);
						writer.WriteLine("};");
						break;

					case OpHandlerKind.OpVsib:
						if (info.Args.Length != 2)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent()) {
							WriteField(writer, "vsib_index_reg_lo", (EnumValue)info.Args[0]);
							WriteField(writer, "vsib_index_reg_hi", (EnumValue)info.Args[1]);
						}
						writer.WriteLine("};");
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
						if (info.Args.Length != 0)
							throw new InvalidOperationException();
						writer.WriteLine(";");
						break;

					case OpHandlerKind.OpModRM_rm_mem_only:
						if (info.Args.Length != 1)
							throw new InvalidOperationException();
						writer.WriteLine(" {");
						using (writer.Indent())
							WriteFieldBool(writer, "must_use_sib", (bool)info.Args[0]);
						writer.WriteLine("};");
						break;

					default:
						throw new InvalidOperationException();
					}
				}

				writer.WriteLine();
				WriteTable(writer, "LEGACY_TABLE", null, dict, handlers.Legacy.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "VEX_TABLE", RustConstants.FeatureVex, dict, handlers.Vex.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "XOP_TABLE", RustConstants.FeatureXop, dict, handlers.Xop.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "EVEX_TABLE", RustConstants.FeatureEvex, dict, handlers.Evex.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "MVEX_TABLE", RustConstants.FeatureMvex, dict, handlers.Mvex.Select(a => (a.opCodeOperandKind, a.opHandlerKind, a.args)));
			}

			static string? GetFeatures(OpInfo info) {
				if (info.Flags == OpInfoFlags.None)
					throw new InvalidOperationException("No refs");
				if ((info.Flags & OpInfoFlags.Legacy) != 0)
					return null;
				var features = new List<string>();
				if ((info.Flags & OpInfoFlags.VEX) != 0)
					features.Add(RustConstants.Vex);
				if ((info.Flags & OpInfoFlags.EVEX) != 0)
					features.Add(RustConstants.Evex);
				if ((info.Flags & OpInfoFlags.XOP) != 0)
					features.Add(RustConstants.Xop);
				if ((info.Flags & OpInfoFlags.MVEX) != 0)
					features.Add(RustConstants.Mvex);
				if (features.Count == 0)
					return null;
				if (features.Count == 1)
					return string.Format(RustConstants.FeatureEncodingOne, features[0]);
				var featuresStr = string.Join(", ", features.ToArray());
				return string.Format(RustConstants.FeatureEncodingMany, featuresStr);
			}

			void WriteTable(FileWriter writer, string name, string? feature, Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo> dict, IEnumerable<(EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)> values) {
				var all = values.ToArray();
				if (feature is not null)
					writer.WriteLine(feature);
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static {name}: [&(dyn Op + Sync); {all.Length}] = [");
				using (writer.Indent()) {
					foreach (var value in all) {
						var info = dict[(value.opHandlerKind, value.args)];
						writer.WriteLine($"&{info.Name},// {value.opKind.Name(idConverter)}");
					}
				}
				writer.WriteLine("];");
			}

			void WriteField(FileWriter writer, string name, EnumValue value) =>
				writer.WriteLine($"{name}: {idConverter.ToDeclTypeAndValue(value)},");

			void WriteFieldBool(FileWriter writer, string name, bool value) =>
				writer.WriteLine($"{name}: {(value ? "true" : "false")},");

			void Add(StringBuilder sb, Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo> dict, IEnumerable<(OpHandlerKind opHandlerKind, object[] args)> values, OpInfoFlags flags) {
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
		}

		protected override void GenerateOpCodeInfo(InstructionDef[] defs, (MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexTupleTypeData,
			(MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexMemorySizeData) =>
			GenerateTable(defs, mvexTupleTypeData, mvexMemorySizeData);

		void GenerateTable(InstructionDef[] defs, (MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexTupleTypeData,
			(MvexTupleTypeLutKind ttLutKind, EnumValue[] tupleTypes)[] mvexMemorySizeData) {
			var allData = GetData(defs).ToArray();
			var encFlags1 = allData.Select(a => (a.def, a.encFlags1)).ToArray();
			var encFlags2 = allData.Select(a => (a.def, a.encFlags2)).ToArray();
			var encFlags3 = allData.Select(a => (a.def, a.encFlags3)).ToArray();
			var opcFlags1 = allData.Select(a => (a.def, a.opcFlags1)).ToArray();
			var opcFlags2 = allData.Select(a => (a.def, a.opcFlags2)).ToArray();
			var mvexInfos = allData.Where(a => a.mvex is not null).Select(a => (a.def, a.mvex.GetValueOrDefault())).ToArray();
			var encoderInfo = new (string name, (InstructionDef def, uint value)[] values)[] {
				("ENC_FLAGS1", encFlags1),
				("ENC_FLAGS2", encFlags2),
				("ENC_FLAGS3", encFlags3),
			};
			var opCodeInfo = new (string name, (InstructionDef def, uint value)[] values)[] {
				("OPC_FLAGS1", opcFlags1),
				("OPC_FLAGS2", opcFlags2),
			};

			GenerateTables(defs, encoderInfo, "encoder_data.rs");
			GenerateTables(defs, opCodeInfo, "op_code_data.rs");
			GenerateTables(mvexInfos, "mvex_data.rs");
			GenerateTables(mvexTupleTypeData, "mvex_tt_lut.rs", "MVEX_TUPLE_TYPE_LUT");
			GenerateTables(mvexMemorySizeData, "mvex_memsz_lut.rs", "MVEX_MEMSZ_LUT");
		}

		void GenerateTables(InstructionDef[] defs, (string name, (InstructionDef def, uint value)[] values)[] encoderInfo, string filename) {
			var fullFilename = generatorContext.Types.Dirs.GetRustFilename("encoder", filename);
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(fullFilename))) {
				writer.WriteFileHeader();
				foreach (var info in encoderInfo) {
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"pub(super) static {info.name}: [u32; {defs.Length}] = [");
					using (writer.Indent()) {
						foreach (var vinfo in info.values)
							writer.WriteLine($"{NumberFormatter.FormatHexUInt32WithSep(vinfo.value)},// {vinfo.def.Code.Name(idConverter)}");
					}
					writer.WriteLine("];");
				}
			}
		}

		void GenerateTables((InstructionDef def, MvexEncInfo mvex)[] mvexInfos, string filename) {
			var infos = mvexInfos.Where(x => x.def.Encoding == EncodingKind.MVEX).ToArray();
			var fullFilename = generatorContext.Types.Dirs.GetRustFilename("mvex", filename);
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(fullFilename))) {
				writer.WriteFileHeader();
				writer.WriteLine("use crate::mvex::mvex_info::MvexInfo;");
				writer.WriteLine("use crate::{MvexConvFn, MvexEHBit, MvexTupleTypeLutKind};");
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static MVEX_INFO: [MvexInfo; {infos.Length}] = [");
				using (writer.Indent()) {
					foreach (var (def, mvex) in infos)
						writer.WriteLine($"MvexInfo::new({idConverter.ToDeclTypeAndValue(mvex.TupleTypeLutKind)}, {idConverter.ToDeclTypeAndValue(mvex.EHBit)}, {idConverter.ToDeclTypeAndValue(mvex.ConvFn)}, 0x{mvex.InvalidConvFns:X02}, 0x{mvex.InvalidSwizzleFns:X02}, 0x{(uint)mvex.Flags1:X02}, 0x{(uint)mvex.Flags2:X02}),// {idConverter.ToDeclTypeAndValue(def.Code)}");
				}
				writer.WriteLine("];");
			}
		}

		void GenerateTables((MvexTupleTypeLutKind ttLutKind, EnumValue[] enumValues)[] mvexData, string filename, string tableName) {
			var fullFilename = generatorContext.Types.Dirs.GetRustFilename("mvex", filename);
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(fullFilename))) {
				writer.WriteFileHeader();
				var declTypeStr = mvexData[0].enumValues[0].DeclaringType.Name(idConverter);
				writer.WriteLine($"use crate::{declTypeStr};");
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				var totalSize = mvexData.Select(x => x.enumValues.Length).Sum();
				writer.WriteLine($"pub(crate) static {tableName}: [{declTypeStr}; {totalSize}] = [");
				using (writer.Indent()) {
					foreach (var (ttLutKind, enumValues) in mvexData) {
						var ttLutKindValue = genTypes[TypeIds.MvexTupleTypeLutKind][ttLutKind.ToString()];
						writer.WriteLine($"// {idConverter.ToDeclTypeAndValue(ttLutKindValue)}");
						for (int i = 0; i < enumValues.Length; i++) {
							var enumValue = enumValues[i];
							writer.WriteLine($"{idConverter.ToDeclTypeAndValue(enumValue)},// {i}");
						}
					}
				}
				writer.WriteLine("];");
			}
		}

		protected override void Generate((EnumValue value, uint size)[] immSizes) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder.rs");
			new FileUpdater(TargetLanguage.Rust, "ImmSizes", filename).Generate(writer => {
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static IMM_SIZES: [u32; {immSizes.Length}] = [");
				using (writer.Indent()) {
					foreach (var info in immSizes)
						writer.WriteLine($"{info.size},// {info.value.Name(idConverter)}");
				}
				writer.WriteLine("];");
			});
		}

		void GenerateCases(string filename, string id, EnumValue[] codeValues, string statement) =>
			new FileUpdater(TargetLanguage.Rust, id, filename).Generate(writer => {
				if (codeValues.Length == 0)
					return;
				var bar = string.Empty;
				foreach (var value in codeValues) {
					writer.WriteLine($"{bar}{idConverter.ToDeclTypeAndValue(value)}");
					bar = "| ";
				}
				writer.WriteLine($"=> {statement},");
			});

		void GenerateNotInstrCases(string filename, string id, (EnumValue code, string result)[] notInstrStrings, bool useReturn) =>
			new FileUpdater(TargetLanguage.Rust, id, filename).Generate(writer => {
				string @return = useReturn ? "return " : string.Empty;
				foreach (var info in notInstrStrings)
					writer.WriteLine($"{idConverter.ToDeclTypeAndValue(info.code)} => {@return}String::from(\"{info.result}\"),");
			});

		protected override void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder", "instruction_fmt.rs");
			GenerateNotInstrCases(filename, "InstrFmtNotInstructionString", notInstrStrings, true);
		}

		protected override void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder", "op_code_fmt.rs");
			GenerateNotInstrCases(filename, "OpCodeFmtNotInstructionString", notInstrStrings, false);
			GenerateCases(filename, "HasModRM", hasModRM, "return true");
			GenerateCases(filename, "HasVsib", hasVsib, "return true");
		}

		protected override void GenerateCore() =>
			GenerateMnemonicStringTable();

		void GenerateMnemonicStringTable() {
			var values = genTypes[TypeIds.Mnemonic].Values;
			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder", "mnemonic_str_tbl.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static TO_MNEMONIC_STR: [&str; {values.Length}] = [");
				using (writer.Indent()) {
					foreach (var value in values)
						writer.WriteLine($"\"{value.RawName.ToLowerInvariant()}\",");
				}
				writer.WriteLine("];");
			}
		}

		protected override void GenerateInstrSwitch(EnumValue[] jccInstr, EnumValue[] simpleBranchInstr, EnumValue[] callInstr, EnumValue[] jmpInstr, EnumValue[] xbeginInstr) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("block_enc", "instr", "mod.rs");
			GenerateCases(filename, "JccInstr", jccInstr, "return Box::new(JccInstr::new(block_encoder, base, instruction))");
			GenerateCases(filename, "SimpleBranchInstr", simpleBranchInstr, "return Box::new(SimpleBranchInstr::new(block_encoder, base, instruction))");
			GenerateCases(filename, "CallInstr", callInstr, "return Box::new(CallInstr::new(block_encoder, base, instruction))");
			GenerateCases(filename, "JmpInstr", jmpInstr, "return Box::new(JmpInstr::new(block_encoder, base, instruction))");
			GenerateCases(filename, "XbeginInstr", xbeginInstr, "return Box::new(XbeginInstr::new(block_encoder, base, instruction))");
		}

		protected override void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("instruction.rs");
			GenerateCases(filename, "Vsib32", vsib32, "Some(false)");
			GenerateCases(filename, "Vsib64", vsib64, "Some(true)");
		}

		protected override void GenerateDecoderOptionsTable((EnumValue decOptionValue, EnumValue decoderOptions)[] values) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder", "op_code.rs");
			new FileUpdater(TargetLanguage.Rust, "ToDecoderOptionsTable", filename).Generate(writer => {
				writer.WriteLine(RustConstants.FeatureDecoder);
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"static TO_DECODER_OPTIONS: [u32; {values.Length}] = [");
				using (writer.Indent()) {
					foreach (var (_, decoderOptions) in values)
						writer.WriteLine($"{decoderOptions.DeclaringType.Name(idConverter)}::{idConverter.Constant(decoderOptions.RawName)},");
				}
				writer.WriteLine("];");
			});
		}

		protected override void GenerateImpliedOps((EncodingKind Encoding, InstrStrImpliedOp[] Ops, InstructionDef[] defs)[] impliedOpsInfo) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("encoder", "instruction_fmt.rs");
			new FileUpdater(TargetLanguage.Rust, "PrintImpliedOps", filename).Generate(writer => {
				foreach (var info in impliedOpsInfo) {
					if (RustConstants.GetFeature(info.Encoding) is string feature)
						writer.WriteLine(feature);
					var bar = string.Empty;
					foreach (var def in info.defs) {
						writer.Write($"{bar}{idConverter.ToDeclTypeAndValue(def.Code)}");
						bar = " | ";
					}
					writer.WriteLine(" => {");
					using (writer.Indent()) {
						foreach (var op in info.Ops) {
							writer.WriteLine("self.write_op_separator();");
							writer.WriteLine($"self.write(\"{op.Operand}\", {(op.IsUpper ? "true" : "false")});");
						}
					}
					writer.WriteLine("}");
				}
			});
		}
	}
}
