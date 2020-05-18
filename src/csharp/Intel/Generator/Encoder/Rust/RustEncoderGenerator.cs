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
	[Generator(TargetLanguage.Rust, GeneratorNames.Encoder)]
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

		sealed class OpInfo {
			public readonly OpHandlerKind OpHandlerKind;
			public readonly object[] Args;
			public readonly string Name;
			public OpInfo(OpHandlerKind opHandlerKind, object[] args, string name) {
				OpHandlerKind = opHandlerKind;
				Args = args;
				Name = name;
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
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "op_kind_tables.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				writer.WriteLine("use super::super::OpCodeOperandKind;");
				Generate(writer, "LEGACY_OP_KINDS", legacy);
				Generate(writer, "VEX_OP_KINDS", vex);
				Generate(writer, "XOP_OP_KINDS", xop);
				Generate(writer, "EVEX_OP_KINDS", evex);
			}

			void Generate(FileWriter writer, string name, (EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				var declTypeStr = genTypes[TypeIds.OpCodeOperandKind].Name(idConverter);
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
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
			Add(sb, dict, legacy.Select(a => (a.opHandlerKind, a.args)));
			Add(sb, dict, vex.Select(a => (a.opHandlerKind, a.args)));
			Add(sb, dict, xop.Select(a => (a.opHandlerKind, a.args)));
			Add(sb, dict, evex.Select(a => (a.opHandlerKind, a.args)));

			var usedNames = new HashSet<string>(dict.Count, StringComparer.Ordinal);
			foreach (var kv in dict) {
				if (!usedNames.Add(kv.Value.Name))
					throw new InvalidOperationException();
			}

			var filename = Path.Combine(generatorContext.RustDir, "encoder", "ops_tables.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine(RustConstants.AttributeNoRustFmtInner);
				writer.WriteLine();
				writer.WriteLine("use super::super::*;");
				writer.WriteLine("use super::ops::*;");
				writer.WriteLine();

				foreach (var kv in dict.OrderBy(a => a.Value.Name, StringComparer.Ordinal)) {
					var info = kv.Value;
					var structName = idConverter.Type(GetStructName(info.OpHandlerKind));
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
					case OpHandlerKind.OpIb11:
					case OpHandlerKind.OpIb21:
					case OpHandlerKind.OpIq:
					case OpHandlerKind.OpIw:
					case OpHandlerKind.OpModRM_rm_mem_only:
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

					default:
						throw new InvalidOperationException();
					}
				}

				writer.WriteLine();
				WriteTable(writer, "LEGACY_TABLE", dict, legacy.Select(a => (a.legacyOpKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "VEX_TABLE", dict, vex.Select(a => (a.vexOpKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "XOP_TABLE", dict, xop.Select(a => (a.xopOpKind, a.opHandlerKind, a.args)));
				WriteTable(writer, "EVEX_TABLE", dict, evex.Select(a => (a.evexOpKind, a.opHandlerKind, a.args)));
			}

			void WriteTable(FileWriter writer, string name, Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo> dict, IEnumerable<(EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)> values) {
				var all = values.ToArray();
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

			void Add(StringBuilder sb, Dictionary<(OpHandlerKind opHandlerKind, object[] args), OpInfo> dict, IEnumerable<(OpHandlerKind opHandlerKind, object[] args)> values) {
				foreach (var value in values) {
					if (!dict.ContainsKey(value))
						dict.Add(value, new OpInfo(value.opHandlerKind, value.args, GetName(sb, value.opHandlerKind, value.args)));
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

		protected override void GenerateOpCodeInfo(InstructionDef[] defs) {
			GenerateTable(defs);
			GenerateNonZeroOpMaskRegisterCode(defs);
		}

		void GenerateTable(InstructionDef[] defs) {
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "op_code_data.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(super) static OP_CODE_DATA: [u32; {defs.Length} * 3] = [");
				using (writer.Indent()) {
					foreach (var info in GetData(defs))
						writer.WriteLine($"{NumberFormatter.FormatHexUInt32WithSep(info.dword1)}, {NumberFormatter.FormatHexUInt32WithSep(info.dword2)}, {NumberFormatter.FormatHexUInt32WithSep(info.dword3)},// {info.opCode.Code.Name(idConverter)}");
				}
				writer.WriteLine("];");
			}
		}

		void GenerateNonZeroOpMaskRegisterCode(InstructionDef[] defs) {
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "op_code.rs");
			new FileUpdater(TargetLanguage.Rust, "NonZeroOpMaskRegister", filename).Generate(writer => {
				var codeStr = genTypes[TypeIds.Code].Name(idConverter);
				var bar = string.Empty;
				foreach (var def in defs) {
					if ((def.OpCodeInfo.Flags & OpCodeFlags.NonZeroOpMaskRegister) != 0) {
						writer.WriteLine($"{bar}{codeStr}::{def.OpCodeInfo.Code.Name(idConverter)}");
						bar = "| ";
					}
				}
			});
		}

		protected override void Generate((EnumValue value, uint size)[] immSizes) {
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "mod.rs");
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

		protected override void Generate((EnumValue allowedPrefixes, OpCodeFlags prefixes)[] infos, (EnumValue value, OpCodeFlags flag)[] flagsInfos) {
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "op_code.rs");
			new FileUpdater(TargetLanguage.Rust, "AllowedPrefixes", filename).Generate(writer => {
				foreach (var info in infos) {
					writer.Write($"{info.allowedPrefixes.DeclaringType.Name(idConverter)}::{info.allowedPrefixes.Name(idConverter)} => ");
					WriteFlags(writer, idConverter, info.prefixes, flagsInfos, " | ", "::", true);
					writer.WriteLine(",");
				}
			});
		}

		void GenerateCases(string filename, string id, EnumValue[] codeValues) {
			new FileUpdater(TargetLanguage.Rust, id, filename).Generate(writer => {
				var bar = string.Empty;
				foreach (var value in codeValues) {
					writer.WriteLine($"{bar}{value.DeclaringType.Name(idConverter)}::{value.Name(idConverter)}");
					bar = "| ";
				}
			});
		}

		void GenerateNotInstrCases(string filename, string id, (EnumValue code, string result)[] notInstrStrings, bool useReturn) {
			new FileUpdater(TargetLanguage.Rust, id, filename).Generate(writer => {
				string @return = useReturn ? "return " : string.Empty;
				foreach (var info in notInstrStrings)
					writer.WriteLine($"{info.code.DeclaringType.Name(idConverter)}::{info.code.Name(idConverter)} => {@return}String::from(\"{info.result}\"),");
			});
		}

		protected override void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] opMaskIsK1, EnumValue[] incVecIndex, EnumValue[] noVecIndex, EnumValue[] swapVecIndex12, EnumValue[] fpuStartOpIndex1) {
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "instruction_fmt.rs");
			GenerateNotInstrCases(filename, "InstrFmtNotInstructionString", notInstrStrings, true);
			GenerateCases(filename, "OpMaskIsK1", opMaskIsK1);
			GenerateCases(filename, "IncVecIndex", incVecIndex);
			GenerateCases(filename, "NoVecIndex", noVecIndex);
			GenerateCases(filename, "SwapVecIndex12", swapVecIndex12);
			GenerateCases(filename, "FpuStartOpIndex1", fpuStartOpIndex1);
			GenerateCases(filename, "OpMaskIsK1", opMaskIsK1);
		}

		protected override void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib) {
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "op_code_fmt.rs");
			GenerateNotInstrCases(filename, "OpCodeFmtNotInstructionString", notInstrStrings, false);
			GenerateCases(filename, "HasModRM", hasModRM);
			GenerateCases(filename, "HasVsib", hasVsib);
		}

		protected override void GenerateCore() =>
			GenerateMnemonicStringTable();

		void GenerateMnemonicStringTable() {
			var values = genTypes[TypeIds.Mnemonic].Values;
			var filename = Path.Combine(generatorContext.RustDir, "encoder", "mnemonic_str_tbl.rs");
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
			var filename = Path.Combine(generatorContext.RustDir, "block_enc", "instr", "mod.rs");
			GenerateCases(filename, "JccInstr", jccInstr);
			GenerateCases(filename, "SimpleBranchInstr", simpleBranchInstr);
			GenerateCases(filename, "CallInstr", callInstr);
			GenerateCases(filename, "JmpInstr", jmpInstr);
			GenerateCases(filename, "XbeginInstr", xbeginInstr);
		}

		protected override void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64) {
			var filename = Path.Combine(generatorContext.RustDir, "instruction.rs");
			GenerateCases(filename, "Vsib32", vsib32);
			GenerateCases(filename, "Vsib64", vsib64);
		}
	}
}
