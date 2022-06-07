// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Constants;
using Generator.Constants.Rust;
using Generator.Enums;
using Generator.Enums.InstructionInfo;
using Generator.Enums.Rust;
using Generator.IO;
using Generator.Tables;

namespace Generator.InstructionInfo.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustInstrInfoGenerator : InstrInfoGenerator {
		readonly IdentifierConverter idConverter;
		readonly RustEnumsGenerator enumGenerator;
		readonly RustConstantsGenerator constantsGenerator;
		readonly GeneratorContext generatorContext;
		readonly EnumType opAccessType;
		readonly EnumType registerType;
		readonly EnumType codeSizeType;

		public RustInstrInfoGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			enumGenerator = new RustEnumsGenerator(generatorContext);
			constantsGenerator = new RustConstantsGenerator(generatorContext);
			this.generatorContext = generatorContext;
			opAccessType = generatorContext.Types[TypeIds.OpAccess];
			registerType = generatorContext.Types[TypeIds.Register];
			codeSizeType = generatorContext.Types[TypeIds.CodeSize];
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);
		protected override void Generate(ConstantsType constantsType) => constantsGenerator.Generate(constantsType);

		protected override void Generate((InstructionDef def, uint dword1, uint dword2)[] infos) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("info", "info_table.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(crate) static TABLE: [(u32, u32); {infos.Length}] = [");
				using (writer.Indent()) {
					foreach (var info in infos)
						writer.WriteLine($"({NumberFormatter.FormatHexUInt32WithSep(info.dword1)}, {NumberFormatter.FormatHexUInt32WithSep(info.dword2)}),// {info.def.Code.Name(idConverter)}");
				}
				writer.WriteLine("];");
			}
		}

		protected override void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("info", "rflags_table.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				var infos = new (RflagsBits[] rflags, string name)[] {
					(read, "read"),
					(undefined, "undefined"),
					(written, "written"),
					(cleared, "cleared"),
					(set, "set"),
					(modified, "modified"),
				};
				foreach (var info in infos) {
					var rflags = info.rflags;
					if (rflags.Length != infos[0].rflags.Length)
						throw new InvalidOperationException();
					var name = idConverter.Static("flags" + info.name[0..1].ToUpperInvariant() + info.name[1..]);
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"pub(crate) static {name}: [u16; {rflags.Length}] = [");
					using (writer.Indent()) {
						for (int i = 0; i < rflags.Length; i++) {
							var rfl = rflags[i];
							uint value = (uint)rfl;
							if (value > ushort.MaxValue)
								throw new InvalidOperationException();
							writer.WriteLine($"{NumberFormatter.FormatHexUInt32WithSep(value)},// {enumValues[i].Name(idConverter)}");
						}
					}
					writer.WriteLine("];");
				}
			}
		}

		protected override void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures) {
			var filename = generatorContext.Types.Dirs.GetRustFilename("info", "cpuid_table.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				var cpuidFeatureTypeStr = genTypes[TypeIds.CpuidFeature].Name(idConverter);
				writer.WriteLine($"use crate::{cpuidFeatureTypeStr};");
				writer.WriteLine();
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(crate) static CPUID: [&[{cpuidFeatureTypeStr}]; {cpuidFeatures.Length}] = [");
				using (writer.Indent()) {
					foreach (var info in cpuidFeatures)
						writer.WriteLine($"&[{string.Join(", ", info.cpuidFeatures.Select(a => idConverter.ToDeclTypeAndValue(a)))}],// {info.cpuidInternal.Name(idConverter)}");
				}
				writer.WriteLine("];");
			}
		}

		protected override void GenerateCore() => GenerateOpAccesses();

		void GenerateOpAccesses() {
			var filename = generatorContext.Types.Dirs.GetRustFilename("info", "enums.rs");
			new FileUpdater(TargetLanguage.Rust, "OpAccesses", filename).Generate(writer => GenerateOpAccesses(writer));
		}

		void GenerateOpAccesses(FileWriter writer) {
			var opInfos = instrInfoTypes.EnumOpInfos;
			// We assume max op count is 5, update the code if not
			Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);

			var indexes = new int[] { 1, 2 };
			var opAccessTypeStr = genTypes[TypeIds.OpAccess].Name(idConverter);
			foreach (var index in indexes) {
				var opInfo = opInfos[index];
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				var name = idConverter.Constant($"OpAccess_{index}");
				writer.WriteLine($"pub(super) static {name}: [{opAccessTypeStr}; {opInfo.Values.Length}] = [");
				using (writer.Indent()) {
					foreach (var value in opInfo.Values) {
						var v = ToOpAccess(value);
						writer.WriteLine($"{idConverter.ToDeclTypeAndValue(v)},");
					}
				}
				writer.WriteLine("];");
			}
		}

		protected override void GenerateImpliedAccesses(ImpliedAccessesDef[] defs) {
			var filename = genTypes.Dirs.GetRustFilename("info", "factory.rs");
			new FileUpdater(TargetLanguage.Rust, "ImpliedAccessHandler", filename).Generate(writer => GenerateImpliedAccesses(writer, defs));
		}

		void GenerateImpliedAccesses(FileWriter writer, ImpliedAccessesDef[] defs) {
			foreach (var def in defs) {
				writer.WriteLine($"{GetEnumName(def.EnumValue)} => {{");
				using (writer.Indent()) {
					foreach (var cond in def.ImpliedAccesses.Conditions) {
						var condStr = GetConditionString(cond.Kind);
						if (condStr is null) {
							if (cond.FalseStatements.Count > 0)
								throw new InvalidOperationException();
							GenerateImpliedAccesses(writer, cond.TrueStatements);
						}
						else {
							writer.WriteLine($"if {condStr} {{");
							using (writer.Indent())
								GenerateImpliedAccesses(writer, cond.TrueStatements);
							writer.WriteLine("}");
							if (cond.FalseStatements.Count > 0) {
								writer.WriteLine("else {");
								using (writer.Indent())
									GenerateImpliedAccesses(writer, cond.FalseStatements);
								writer.WriteLine("}");
							}
						}
					}
				}
				writer.WriteLine("}");
			}

			static string? GetConditionString(ImplAccConditionKind kind) =>
				kind switch {
					ImplAccConditionKind.None => null,
					ImplAccConditionKind.Bit64 => "(flags & Flags::IS_64BIT) != 0",
					ImplAccConditionKind.NotBit64 => "(flags & Flags::IS_64BIT) == 0",
					_ => throw new InvalidOperationException(),
				};
		}

		void GenerateImpliedAccesses(FileWriter writer, List<ImplAccStatement> stmts) {
			var stmtState = new StmtState(
				"if (flags & Flags::NO_REGISTER_USAGE) == 0 {",
				"}",
				"if (flags & Flags::NO_MEMORY_USAGE) == 0 {",
				"}");
			foreach (var stmt in stmts) {
				IntArgImplAccStatement arg1;
				IntX2ArgImplAccStatement arg2;
				stmtState.SetKind(writer, stmt.Kind);
				switch (stmt.Kind) {
				case ImplAccStatementKind.MemoryAccess:
					var mem = (MemoryImplAccStatement)stmt;
					writer.WriteLine($"Self::add_memory(info, {GetRegisterString(mem.Segment)}, {GetRegisterString(mem.Base)}, {GetRegisterString(mem.Index)}, {mem.Scale}, 0x{mem.Displacement}, {GetMemorySizeString(mem.MemorySize)}, {GetOpAccessString(mem.Access)}, {GetCodeSizeString(mem.AddressSize)}, {mem.VsibSize});");
					break;
				case ImplAccStatementKind.RegisterAccess:
					var reg = (RegisterImplAccStatement)stmt;
					if (reg.IsMemOpSegRead && CouldBeNullSegIn64BitMode(reg.Register, out var definitelyNullSeg)) {
						if (definitelyNullSeg) {
							writer.WriteLine("if (flags & Flags::IS_64BIT) == 0 {");
							using (writer.Indent())
								writer.WriteLine($"Self::add_register(flags, info, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
							writer.WriteLine("}");
						}
						else
							writer.WriteLine($"Self::add_memory_segment_register(flags, info, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
					}
					else
						writer.WriteLine($"Self::add_register(flags, info, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
					break;
				case ImplAccStatementKind.RegisterRangeAccess:
					var rreg = (RegisterRangeImplAccStatement)stmt;
					writer.WriteLine($"for reg_num in ({GetEnumName(rreg.RegisterFirst)} as u32)..(({GetEnumName(rreg.RegisterLast)} as u32) + 1) {{");
					using (writer.Indent())
						writer.WriteLine($"Self::add_register(flags, info, unsafe {{ mem::transmute(reg_num as RegisterUnderlyingType) }}, {GetOpAccessString(rreg.Access)});");
					writer.WriteLine("}");
					break;
				case ImplAccStatementKind.ShiftMask:
					arg1 = (IntArgImplAccStatement)stmt;
					break;
				case ImplAccStatementKind.ShiftMask1FMod:
					arg1 = (IntArgImplAccStatement)stmt;
					Verify_9_or_17(arg1.Arg);
					break;
				case ImplAccStatementKind.ZeroRegRflags:
					writer.WriteLine("Self::command_clear_rflags(instruction, info, flags);");
					break;
				case ImplAccStatementKind.ZeroRegRegmem:
					writer.WriteLine("Self::command_clear_reg_regmem(instruction, info, flags);");
					break;
				case ImplAccStatementKind.ZeroRegRegRegmem:
					writer.WriteLine("Self::command_clear_reg_reg_regmem(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Arpl:
					writer.WriteLine("Self::command_arpl(instruction, info, flags);");
					break;
				case ImplAccStatementKind.LastGpr8:
					writer.WriteLine($"Self::command_last_gpr(instruction, info, flags, {GetEnumName(registerType[nameof(Register.AL)])});");
					break;
				case ImplAccStatementKind.LastGpr16:
					writer.WriteLine($"Self::command_last_gpr(instruction, info, flags, {GetEnumName(registerType[nameof(Register.AX)])});");
					break;
				case ImplAccStatementKind.LastGpr32:
					writer.WriteLine($"Self::command_last_gpr(instruction, info, flags, {GetEnumName(registerType[nameof(Register.EAX)])});");
					break;
				case ImplAccStatementKind.EmmiReg:
					var emmi = (EmmiImplAccStatement)stmt;
					writer.WriteLine($"Self::command_emmi(instruction, info, flags, {GetEnumName(GetOpAccess(opAccessType, emmi.Access))});");
					break;
				case ImplAccStatementKind.Enter:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"Self::command_enter(instruction, info, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Leave:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"Self::command_leave(instruction, info, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Push:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					if (arg2.Arg1 != 0)
						writer.WriteLine($"Self::command_push(instruction, info, flags, {arg2.Arg1}, {Verify_2_4_or_8(arg2.Arg2)});");
					break;
				case ImplAccStatementKind.Pop:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					if (arg2.Arg1 != 0)
						writer.WriteLine($"Self::command_pop(instruction, info, flags, {arg2.Arg1}, {Verify_2_4_or_8(arg2.Arg2)});");
					break;
				case ImplAccStatementKind.PopRm:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"Self::command_pop_rm(instruction, info, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Pusha:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"Self::command_pusha(instruction, info, flags, {Verify_2_or_4(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Popa:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"Self::command_popa(instruction, info, flags, {Verify_2_or_4(arg1.Arg)});");
					break;
				case ImplAccStatementKind.lea:
					writer.WriteLine("Self::command_lea(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Cmps:
					writer.WriteLine("Self::command_cmps(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Ins:
					writer.WriteLine("Self::command_ins(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Lods:
					writer.WriteLine("Self::command_lods(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Movs:
					writer.WriteLine("Self::command_movs(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Outs:
					writer.WriteLine("Self::command_outs(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Scas:
					writer.WriteLine("Self::command_scas(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Stos:
					writer.WriteLine("Self::command_stos(instruction, info, flags);");
					break;
				case ImplAccStatementKind.Xstore:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"Self::command_xstore(instruction, info, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.MemDispl:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"Self::command_mem_displ(info, flags, {(int)arg1.Arg});");
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			stmtState.Done(writer);
		}

		string GetMemorySizeString(ImplAccMemorySize memorySize) {
			switch (memorySize.Kind) {
			case ImplAccMemorySizeKind.MemorySize:
				if (memorySize.MemorySize is null)
					throw new InvalidOperationException();
				return GetEnumName(memorySize.MemorySize);
			case ImplAccMemorySizeKind.Default:
				return "instruction.memory_size()";
			default:
				throw new InvalidOperationException();
			}
		}

		string GetRegisterString(ImplAccRegister? register) {
			if (register == null)
				return GetEnumName(registerType[nameof(Register.None)]);
			var reg = register.GetValueOrDefault();
			switch (reg.Kind) {
			case ImplAccRegisterKind.Register:
				if (reg.Register is null)
					throw new InvalidOperationException();
				return GetEnumName(reg.Register);
			case ImplAccRegisterKind.SegmentDefaultDS: return "Self::get_seg_default_ds(instruction)";
			case ImplAccRegisterKind.a_rDI: return "Self::get_a_rdi(instruction)";
			case ImplAccRegisterKind.Op0: return "instruction.op0_register()";
			case ImplAccRegisterKind.Op1: return "instruction.op1_register()";
			case ImplAccRegisterKind.Op2: return "instruction.op2_register()";
			case ImplAccRegisterKind.Op3: return "instruction.op3_register()";
			case ImplAccRegisterKind.Op4: return "instruction.op4_register()";
			default: throw new InvalidOperationException();
			}
		}

		string GetOpAccessString(OpAccess access) => GetEnumName(opAccessType[access.ToString()]);
		string GetCodeSizeString(CodeSize codeSize) => GetEnumName(codeSizeType[codeSize.ToString()]);

		string GetEnumName(EnumValue value) => idConverter.ToDeclTypeAndValue(value);

		void GenerateTable((EncodingKind encoding, InstructionDef[] defs)[] tdefs, string id, string filename) {
			new FileUpdater(TargetLanguage.Rust, id, filename).Generate(writer => {
				foreach (var (encoding, defs) in tdefs) {
					if (RustConstants.GetFeature(encoding) is string feature)
						writer.WriteLine(feature);
					var bar = string.Empty;
					foreach (var def in defs) {
						writer.WriteLine($"{bar}{idConverter.ToDeclTypeAndValue(def.Code)}");
						bar = "| ";
					}
					writer.WriteLine("=> true,");
				}
			});
		}

		protected override void GenerateIgnoresSegmentTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = genTypes.Dirs.GetRustFilename("code.rs");
			GenerateTable(defs, "IgnoresSegmentTable", filename);
		}

		protected override void GenerateIgnoresIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = genTypes.Dirs.GetRustFilename("code.rs");
			GenerateTable(defs, "IgnoresIndexTable", filename);
		}

		protected override void GenerateTileStrideIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = genTypes.Dirs.GetRustFilename("code.rs");
			GenerateTable(defs, "TileStrideIndexTable", filename);
		}

		protected override void GenerateIsStringOpTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = genTypes.Dirs.GetRustFilename("code.rs");
			GenerateTable(defs, "IsStringOpTable", filename);
		}

		protected override void GenerateFpuStackIncrementInfoTable((FpuStackInfo info, InstructionDef[] defs)[] tdefs) {
			var filename = genTypes.Dirs.GetRustFilename("instruction.rs");
			new FileUpdater(TargetLanguage.Rust, "FpuStackIncrementInfoTable", filename).Generate(writer => {
				foreach (var (info, defs) in tdefs) {
					var bar = string.Empty;
					foreach (var def in defs) {
						writer.WriteLine($"{bar}{idConverter.ToDeclTypeAndValue(def.Code)}");
						bar = "| ";
					}
					var conditionalStr = info.Conditional ? "true" : "false";
					var writesTopStr = info.WritesTop ? "true" : "false";
					writer.WriteLine($"=> FpuStackIncrementInfo {{ increment: {info.Increment}, conditional: {conditionalStr}, writes_top: {writesTopStr} }},");
				}
			});
		}

		protected override void GenerateStackPointerIncrementTable((EncodingKind encoding, StackInfo info, InstructionDef[] defs)[] tdefs) {
			var filename = genTypes.Dirs.GetRustFilename("instruction.rs");
			new FileUpdater(TargetLanguage.Rust, "StackPointerIncrementTable", filename).Generate(writer => {
				foreach (var (encoding, info, defs) in tdefs) {
					var expr = info.Kind switch {
						StackInfoKind.Increment => $" => {info.Value},",
						StackInfoKind.Enter => $" => -({info.Value} + (self.immediate8_2nd() as i32 & 0x1F) * {info.Value} + self.immediate16() as i32),",
						StackInfoKind.Iret => $" => if self.code_size() == CodeSize::Code64 {{ {info.Value} * 5 }} else {{ {info.Value} * 3 }},",
						StackInfoKind.PopImm16 => $" => {info.Value} + self.immediate16() as i32,",
						_ => throw new InvalidOperationException(),
					};

					if (RustConstants.GetFeature(encoding) is string feature)
						writer.WriteLine(feature);
					var bar = string.Empty;
					for (int i = 0; i < defs.Length; i++) {
						var def = defs[i];
						var finalExpr = i + 1 == defs.Length ? expr : string.Empty;
						writer.WriteLine($"{bar}{idConverter.ToDeclTypeAndValue(def.Code)}{finalExpr}");
						bar = "| ";
					}
				}
			});
		}
	}
}
