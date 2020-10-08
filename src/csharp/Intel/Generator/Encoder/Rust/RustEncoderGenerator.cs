/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
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

		protected override void Generate((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex) {
			GenerateOpCodeOperandKindTables(legacy, vex, xop, evex);
			GenerateOpTables(legacy, vex, xop, evex);
		}

		void GenerateOpCodeOperandKindTables((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex) {
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", "op_kind_tables.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				writer.WriteLine("use super::super::OpCodeOperandKind;");
				Generate(writer, "LEGACY_OP_KINDS", null, legacy);
				Generate(writer, "VEX_OP_KINDS", RustConstants.FeatureVex, vex);
				Generate(writer, "XOP_OP_KINDS", RustConstants.FeatureXop, xop);
				Generate(writer, "EVEX_OP_KINDS", RustConstants.FeatureEvex, evex);
			}

			void Generate(FileWriter writer, string name, string? feature, (EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				var declTypeStr = genTypes[TypeIds.OpCodeOperandKind].Name(idConverter);
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				if (feature is object)
					writer.WriteLine(feature);
				writer.WriteLine($"pub(super) static {name}: [{declTypeStr}; {table.Length}] = [");
				using (writer.Indent()) {
					foreach (var info in table)
						writer.WriteLine($"{declTypeStr}::{info.opCodeOperandKind.Name(idConverter)},// {info.opKind.Name(idConverter)}");
				}
				writer.WriteLine("];");
			}
		}

		void GenerateOpTables((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex) {
			var sb = new StringBuilder();
			var dict = new Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo>(new OpKeyComparer());
			Add(sb, dict, legacy.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.Legacy);
			Add(sb, dict, vex.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.VEX);
			Add(sb, dict, xop.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.XOP);
			Add(sb, dict, evex.Select(a => (a.opHandlerKind, a.args)), OpInfoFlags.EVEX);

			var usedNames = new HashSet<string>(dict.Count, StringComparer.Ordinal);
			foreach (var kv in dict) {
				if (!usedNames.Add(kv.Value.Name))
					throw new InvalidOperationException();
			}

			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", "ops_tables.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("use super::super::*;");
				writer.WriteLine("use super::ops::*;");
				writer.WriteLine();

				foreach (var kv in dict.OrderBy(a => a.Value.Name, StringComparer.Ordinal)) {
					var info = kv.Value;
					var structName = idConverter.Type(GetStructName(info.OpHandlerKind));
					var features = GetFeatures(info);
					if (features is object)
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
					case OpHandlerKind.OpIs4x:
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

					case OpHandlerKind.OpVMx:
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
					case OpHandlerKind.OpI2:
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
				WriteTable(writer, "LEGACY_TABLE", null, dict, legacy.Select(a => (a.legacyOpKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "VEX_TABLE", RustConstants.FeatureVex, dict, vex.Select(a => (a.vexOpKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "XOP_TABLE", RustConstants.FeatureXop, dict, xop.Select(a => (a.xopOpKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "EVEX_TABLE", RustConstants.FeatureEvex, dict, evex.Select(a => (a.evexOpKind, a.opHandlerKind, a.args)));
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
				if (features.Count == 0)
					return null;
				if (features.Count == 1)
					return string.Format(RustConstants.FeatureEncodingOne, features[0]);
				var featuresStr = string.Join(", ", features.ToArray());
				return string.Format(RustConstants.FeatureEncodingMany, featuresStr);
			}

			void WriteTable(FileWriter writer, string name, string? feature, Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo> dict, IEnumerable<(EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)> values) {
				var all = values.ToArray();
				if (feature is object)
					writer.WriteLine(feature);
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static {name}: [&(Op + Sync); {all.Length}] = [");
				using (writer.Indent()) {
					foreach (var value in all) {
						var info = dict[(value.opHandlerKind, value.args)];
						writer.WriteLine($"&{info.Name},// {value.opKind.Name(idConverter)}");
					}
				}
				writer.WriteLine("];");
			}

			void WriteField(FileWriter writer, string name, EnumValue value) =>
				writer.WriteLine($"{name}: {value.DeclaringType.Name(idConverter)}::{value.Name(idConverter)},");

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
						sb.Append(value.ToString());
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

		protected override void GenerateOpCodeInfo(InstructionDef[] defs) =>
			GenerateTable(defs);

		void GenerateTable(InstructionDef[] defs) {
			var allData = GetData(defs).ToArray();
			var encFlags1 = allData.Select(a => (a.def, a.encFlags1)).ToArray();
			var encFlags2 = allData.Select(a => (a.def, a.encFlags2)).ToArray();
			var encFlags3 = allData.Select(a => (a.def, a.encFlags3)).ToArray();
			var opcFlags1 = allData.Select(a => (a.def, a.opcFlags1)).ToArray();
			var opcFlags2 = allData.Select(a => (a.def, a.opcFlags2)).ToArray();
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
		}

		void GenerateTables(InstructionDef[] defs, (string name, (InstructionDef def, uint value)[] values)[] encoderInfo, string filename) {
			var fullFilename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", filename);
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

		protected override void Generate((EnumValue value, uint size)[] immSizes) {
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", "mod.rs");
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

		void GenerateCases(string filename, string id, EnumValue[] codeValues, string statement) {
			new FileUpdater(TargetLanguage.Rust, id, filename).Generate(writer => {
				if (codeValues.Length == 0)
					return;
				var bar = string.Empty;
				foreach (var value in codeValues) {
					writer.WriteLine($"{bar}{value.DeclaringType.Name(idConverter)}::{value.Name(idConverter)}");
					bar = "| ";
				}
				writer.WriteLine($"=> {statement},");
			});
		}

		void GenerateNotInstrCases(string filename, string id, (EnumValue code, string result)[] notInstrStrings, bool useReturn) {
			new FileUpdater(TargetLanguage.Rust, id, filename).Generate(writer => {
				string @return = useReturn ? "return " : string.Empty;
				foreach (var info in notInstrStrings)
					writer.WriteLine($"{info.code.DeclaringType.Name(idConverter)}::{info.code.Name(idConverter)} => {@return}String::from(\"{info.result}\"),");
			});
		}

		protected override void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings) {
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", "instruction_fmt.rs");
			GenerateNotInstrCases(filename, "InstrFmtNotInstructionString", notInstrStrings, true);
		}

		protected override void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib) {
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", "op_code_fmt.rs");
			GenerateNotInstrCases(filename, "OpCodeFmtNotInstructionString", notInstrStrings, false);
			GenerateCases(filename, "HasModRM", hasModRM, "return true");
			GenerateCases(filename, "HasVsib", hasVsib, "return true");
		}

		protected override void GenerateCore() =>
			GenerateMnemonicStringTable();

		void GenerateMnemonicStringTable() {
			var values = genTypes[TypeIds.Mnemonic].Values;
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", "mnemonic_str_tbl.rs");
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
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "block_enc", "instr", "mod.rs");
			GenerateCases(filename, "JccInstr", jccInstr, "return Rc::new(RefCell::new(JccInstr::new(block_encoder, block, instruction)))");
			GenerateCases(filename, "SimpleBranchInstr", simpleBranchInstr, "return Rc::new(RefCell::new(SimpleBranchInstr::new(block_encoder, block, instruction)))");
			GenerateCases(filename, "CallInstr", callInstr, "return Rc::new(RefCell::new(CallInstr::new(block_encoder, block, instruction)))");
			GenerateCases(filename, "JmpInstr", jmpInstr, "return Rc::new(RefCell::new(JmpInstr::new(block_encoder, block, instruction)))");
			GenerateCases(filename, "XbeginInstr", xbeginInstr, "return Rc::new(RefCell::new(XbeginInstr::new(block_encoder, block, instruction)))");
		}

		protected override void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64) {
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "instruction.rs");
			GenerateCases(filename, "Vsib32", vsib32, "Some(false)");
			GenerateCases(filename, "Vsib64", vsib64, "Some(true)");
		}

		protected override void GenerateDecoderOptionsTable((EnumValue decOptionValue, EnumValue decoderOptions)[] values) {
			var filename = Path.Combine(generatorContext.Types.Dirs.RustDir, "encoder", "op_code.rs");
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
	}
}
