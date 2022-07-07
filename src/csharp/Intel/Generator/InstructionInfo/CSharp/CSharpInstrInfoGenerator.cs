// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Constants;
using Generator.Constants.CSharp;
using Generator.Enums;
using Generator.Enums.CSharp;
using Generator.Enums.InstructionInfo;
using Generator.IO;
using Generator.Tables;

namespace Generator.InstructionInfo.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpInstrInfoGenerator : InstrInfoGenerator {
		readonly IdentifierConverter idConverter;
		readonly CSharpEnumsGenerator enumGenerator;
		readonly CSharpConstantsGenerator constantsGenerator;
		readonly EnumType opAccessType;
		readonly EnumType registerType;
		readonly EnumType codeSizeType;

		public CSharpInstrInfoGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = CSharpIdentifierConverter.Create();
			enumGenerator = new CSharpEnumsGenerator(generatorContext);
			constantsGenerator = new CSharpConstantsGenerator(generatorContext);
			opAccessType = genTypes[TypeIds.OpAccess];
			registerType = genTypes[TypeIds.Register];
			codeSizeType = generatorContext.Types[TypeIds.CodeSize];
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);
		protected override void Generate(ConstantsType constantsType) => constantsGenerator.Generate(constantsType);

		protected override void Generate((InstructionDef def, uint dword1, uint dword2)[] infos) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.InstructionInfoNamespace, "InstrInfoTable.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.InstructionInfoDefine}");
				writer.WriteLine($"namespace {CSharpConstants.InstructionInfoNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static class InstrInfoTable {");
					using (writer.Indent()) {
						writer.WriteLine($"internal static readonly uint[] Data = new uint[{infos.Length * 2}] {{");
						using (writer.Indent()) {
							foreach (var info in infos)
								writer.WriteLine($"0x{info.dword1:X8}, 0x{info.dword2:X8},// {info.def.Code.Name(idConverter)}");
						}
						writer.WriteLine("};");
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.InstructionInfoNamespace, "RflagsInfoConstants.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.InstructionInfoDefine}");
				writer.WriteLine($"namespace {CSharpConstants.InstructionInfoNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static class RflagsInfoConstants {");
					using (writer.Indent()) {
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
							var name = idConverter.Field("flags" + info.name[0..1].ToUpperInvariant() + info.name[1..]);
							writer.WriteLine($"public static readonly ushort[] {name} = new ushort[{rflags.Length}] {{");
							using (writer.Indent()) {
								for (int i = 0; i < rflags.Length; i++) {
									var rfl = rflags[i];
									uint value = (uint)rfl;
									if (value > ushort.MaxValue)
										throw new InvalidOperationException();
									writer.WriteLine($"0x{value:X4},// {enumValues[i].Name(idConverter)}");
								}
							}
							writer.WriteLine("};");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures) {
			var header = new byte[(cpuidFeatures.Length + 7) / 8];
			for (int i = 0; i < cpuidFeatures.Length; i++) {
				int len = cpuidFeatures[i].cpuidFeatures.Length;
				if (len < 1 || len > 2)
					throw new InvalidOperationException();
				header[i / 8] |= (byte)((len - 1) << (i % 8));
			}

			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(CSharpConstants.GetFilename(genTypes, CSharpConstants.InstructionInfoNamespace, "CpuidFeatureInternalData.g.cs")))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.InstructionInfoDefine}");
				writer.WriteLine($"namespace {CSharpConstants.InstructionInfoNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static partial class CpuidFeatureInternalData {");
					using (writer.Indent()) {
						writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
						writer.WriteLine("internal static System.ReadOnlySpan<byte> GetCpuidFeaturesData() =>");
						writer.WriteLineNoIndent("#else");
						writer.WriteLine("internal static byte[] GetCpuidFeaturesData() =>");
						writer.WriteLineNoIndent("#endif");
						using (writer.Indent()) {
							writer.WriteLine("new byte[] {");
							using (writer.Indent()) {
								writer.WriteCommentLine("Header");
								foreach (var b in header) {
									writer.WriteByte(b);
									writer.WriteLine();
								}
								writer.WriteLine();
								foreach (var info in cpuidFeatures) {
									foreach (var f in info.cpuidFeatures) {
										if ((uint)f.Value > byte.MaxValue)
											throw new InvalidOperationException();
										writer.WriteByte((byte)f.Value);
									}
									writer.WriteCommentLine(info.cpuidInternal.Name(idConverter));
								}
							}
							writer.WriteLine("};");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void GenerateCore() => GenerateOpAccesses();

		void GenerateOpAccesses() {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.InstructionInfoNamespace, "InfoHandlerFlags.cs");
			new FileUpdater(TargetLanguage.CSharp, "OpAccesses", filename).Generate(writer => GenerateOpAccesses(writer));
		}

		void GenerateOpAccesses(FileWriter writer) {
			var opInfos = instrInfoTypes.EnumOpInfos;
			// We assume max op count is 5, update the code if not
			if (opInfos.Length != 5)
				throw new InvalidOperationException();

			var indexes = new int[] { 1, 2 };
			var opAccessTypeStr = genTypes[TypeIds.OpAccess].Name(idConverter);
			foreach (var index in indexes) {
				var opInfo = opInfos[index];
				writer.WriteLine($"public static readonly {opAccessTypeStr}[] Op{index} = new {opAccessTypeStr}[{opInfo.Values.Length}] {{");
				using (writer.Indent()) {
					foreach (var value in opInfo.Values) {
						var v = ToOpAccess(value);
						writer.WriteLine($"{idConverter.ToDeclTypeAndValue(v)},");
					}
				}
				writer.WriteLine("};");
			}
		}

		protected override void GenerateImpliedAccesses(ImpliedAccessesDef[] defs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "InstructionInfoFactory.cs");
			new FileUpdater(TargetLanguage.CSharp, "ImpliedAccessHandler", filename).Generate(writer => GenerateImpliedAccesses(writer, defs));
		}

		void GenerateImpliedAccesses(FileWriter writer, ImpliedAccessesDef[] defs) {
			foreach (var def in defs) {
				writer.WriteLine($"case {GetEnumName(def.EnumValue)}:");
				using (writer.Indent()) {
					foreach (var cond in def.ImpliedAccesses.Conditions) {
						var condStr = GetConditionString(cond.Kind);
						if (condStr is null) {
							if (cond.FalseStatements.Count > 0)
								throw new InvalidOperationException();
							GenerateImpliedAccesses(writer, cond.TrueStatements);
						}
						else {
							writer.WriteLine($"if ({condStr}) {{");
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
					writer.WriteLine("break;");
				}
			}

			static string? GetConditionString(ImplAccConditionKind kind) =>
				kind switch {
					ImplAccConditionKind.None => null,
					ImplAccConditionKind.Bit64 => "(flags & Flags.Is64Bit) != 0",
					ImplAccConditionKind.NotBit64 => "(flags & Flags.Is64Bit) == 0",
					_ => throw new InvalidOperationException(),
				};
		}

		void GenerateImpliedAccesses(FileWriter writer, List<ImplAccStatement> stmts) {
			var stmtState = new StmtState(
				"if ((flags & Flags.NoRegisterUsage) == 0) {",
				"}",
				"if ((flags & Flags.NoMemoryUsage) == 0) {",
				"}");
			foreach (var stmt in stmts) {
				IntArgImplAccStatement arg1;
				IntX2ArgImplAccStatement arg2;
				stmtState.SetKind(writer, stmt.Kind);
				switch (stmt.Kind) {
				case ImplAccStatementKind.MemoryAccess:
					var mem = (MemoryImplAccStatement)stmt;
					writer.WriteLine($"AddMemory({GetRegisterString(mem.Segment)}, {GetRegisterString(mem.Base)}, {GetRegisterString(mem.Index)}, {mem.Scale}, 0x{mem.Displacement}, {GetMemorySizeString(mem.MemorySize)}, {GetOpAccessString(mem.Access)}, {GetCodeSizeString(mem.AddressSize)}, {mem.VsibSize});");
					break;
				case ImplAccStatementKind.RegisterAccess:
					var reg = (RegisterImplAccStatement)stmt;
					if (reg.IsMemOpSegRead && CouldBeNullSegIn64BitMode(reg.Register, out var definitelyNullSeg)) {
						if (definitelyNullSeg) {
							writer.WriteLine("if ((flags & Flags.Is64Bit) == 0)");
							using (writer.Indent())
								writer.WriteLine($"AddRegister(flags, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
						}
						else
							writer.WriteLine($"AddMemorySegmentRegister(flags, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
					}
					else
						writer.WriteLine($"AddRegister(flags, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
					break;
				case ImplAccStatementKind.RegisterRangeAccess:
					var rreg = (RegisterRangeImplAccStatement)stmt;
					writer.WriteLine($"for (var reg = {GetEnumName(rreg.RegisterFirst)}; reg <= {GetEnumName(rreg.RegisterLast)}; reg++)");
					using (writer.Indent())
						writer.WriteLine($"AddRegister(flags, reg, {GetOpAccessString(rreg.Access)});");
					break;
				case ImplAccStatementKind.ShiftMask:
					arg1 = (IntArgImplAccStatement)stmt;
					break;
				case ImplAccStatementKind.ShiftMask1FMod:
					arg1 = (IntArgImplAccStatement)stmt;
					Verify_9_or_17(arg1.Arg);
					break;
				case ImplAccStatementKind.ZeroRegRflags:
					writer.WriteLine("CommandClearRflags(instruction, flags);");
					break;
				case ImplAccStatementKind.ZeroRegRegmem:
					writer.WriteLine("CommandClearRegRegmem(instruction, flags);");
					break;
				case ImplAccStatementKind.ZeroRegRegRegmem:
					writer.WriteLine("CommandClearRegRegRegmem(instruction, flags);");
					break;
				case ImplAccStatementKind.Arpl:
					writer.WriteLine("CommandArpl(instruction, flags);");
					break;
				case ImplAccStatementKind.LastGpr8:
					writer.WriteLine($"CommandLastGpr(instruction, flags, {GetEnumName(registerType[nameof(Register.AL)])});");
					break;
				case ImplAccStatementKind.LastGpr16:
					writer.WriteLine($"CommandLastGpr(instruction, flags, {GetEnumName(registerType[nameof(Register.AX)])});");
					break;
				case ImplAccStatementKind.LastGpr32:
					writer.WriteLine($"CommandLastGpr(instruction, flags, {GetEnumName(registerType[nameof(Register.EAX)])});");
					break;
				case ImplAccStatementKind.EmmiReg:
					var emmi = (EmmiImplAccStatement)stmt;
					writer.WriteLine($"CommandEmmi(instruction, flags, {GetEnumName(GetOpAccess(opAccessType, emmi.Access))});");
					break;
				case ImplAccStatementKind.Enter:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"CommandEnter(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Leave:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"CommandLeave(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Push:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					if (arg2.Arg1 != 0)
						writer.WriteLine($"CommandPush(instruction, flags, {arg2.Arg1}, {Verify_2_4_or_8(arg2.Arg2)});");
					break;
				case ImplAccStatementKind.Pop:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					if (arg2.Arg1 != 0)
						writer.WriteLine($"CommandPop(instruction, flags, {arg2.Arg1}, {Verify_2_4_or_8(arg2.Arg2)});");
					break;
				case ImplAccStatementKind.PopRm:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"CommandPopRm(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Pusha:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"CommandPusha(instruction, flags, {Verify_2_or_4(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Popa:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"CommandPopa(instruction, flags, {Verify_2_or_4(arg1.Arg)});");
					break;
				case ImplAccStatementKind.lea:
					writer.WriteLine("CommandLea(instruction, flags);");
					break;
				case ImplAccStatementKind.Cmps:
					writer.WriteLine("CommandCmps(instruction, flags);");
					break;
				case ImplAccStatementKind.Ins:
					writer.WriteLine("CommandIns(instruction, flags);");
					break;
				case ImplAccStatementKind.Lods:
					writer.WriteLine("CommandLods(instruction, flags);");
					break;
				case ImplAccStatementKind.Movs:
					writer.WriteLine("CommandMovs(instruction, flags);");
					break;
				case ImplAccStatementKind.Outs:
					writer.WriteLine("CommandOuts(instruction, flags);");
					break;
				case ImplAccStatementKind.Scas:
					writer.WriteLine("CommandScas(instruction, flags);");
					break;
				case ImplAccStatementKind.Stos:
					writer.WriteLine("CommandStos(instruction, flags);");
					break;
				case ImplAccStatementKind.Xstore:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"CommandXstore(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.MemDispl:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"CommandMemDispl(flags, {(int)arg1.Arg});");
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
				return "instruction.MemorySize";
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
			case ImplAccRegisterKind.SegmentDefaultDS: return "GetSegDefaultDS(instruction)";
			case ImplAccRegisterKind.a_rDI: return "GetARDI(instruction)";
			case ImplAccRegisterKind.Op0: return "instruction.Op0Register";
			case ImplAccRegisterKind.Op1: return "instruction.Op1Register";
			case ImplAccRegisterKind.Op2: return "instruction.Op2Register";
			case ImplAccRegisterKind.Op3: return "instruction.Op3Register";
			case ImplAccRegisterKind.Op4: return "instruction.Op4Register";
			default: throw new InvalidOperationException();
			}
		}

		string GetOpAccessString(OpAccess access) => GetEnumName(opAccessType[access.ToString()]);
		string GetCodeSizeString(CodeSize codeSize) => GetEnumName(codeSizeType[codeSize.ToString()]);

		string GetEnumName(EnumValue value) => idConverter.ToDeclTypeAndValue(value);

		void GenerateTable((EncodingKind encoding, InstructionDef[] defs)[] tdefs, string id, string filename) {
			new FileUpdater(TargetLanguage.CSharp, id, filename).Generate(writer => {
				foreach (var (encoding, defs) in tdefs) {
					var feature = CSharpConstants.GetDefine(encoding);
					if (feature is not null)
						writer.WriteLineNoIndent($"#if {feature}");
					foreach (var def in defs)
						writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(def.Code)}:");
					using (writer.Indent())
						writer.WriteLine("return true;");
					if (feature is not null)
						writer.WriteLineNoIndent("#endif");
				}
			});
		}

		protected override void GenerateIgnoresSegmentTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "CodeExtensions.cs");
			GenerateTable(defs, "IgnoresSegmentTable", filename);
		}

		protected override void GenerateIgnoresIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "CodeExtensions.cs");
			GenerateTable(defs, "IgnoresIndexTable", filename);
		}

		protected override void GenerateTileStrideIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "CodeExtensions.cs");
			GenerateTable(defs, "TileStrideIndexTable", filename);
		}

		protected override void GenerateIsStringOpTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "InstructionInfoExtensions.1.cs");
			GenerateTable(defs, "IsStringOpTable", filename);
		}

		protected override void GenerateFpuStackIncrementInfoTable((FpuStackInfo info, InstructionDef[] defs)[] tdefs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "Instruction.Info.cs");
			new FileUpdater(TargetLanguage.CSharp, "FpuStackIncrementInfoTable", filename).Generate(writer => {
				foreach (var (info, defs) in tdefs) {
					foreach (var def in defs)
						writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(def.Code)}:");
					using (writer.Indent()) {
						var conditionalStr = info.Conditional ? "true" : "false";
						var writesTopStr = info.WritesTop ? "true" : "false";
						writer.WriteLine($"return new FpuStackIncrementInfo({info.Increment}, {conditionalStr}, {writesTopStr});");
					}
				}
			});
		}

		protected override void GenerateStackPointerIncrementTable((EncodingKind encoding, StackInfo info, InstructionDef[] defs)[] tdefs) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "Instruction.Info.cs");
			new FileUpdater(TargetLanguage.CSharp, "StackPointerIncrementTable", filename).Generate(writer => {
				foreach (var (encoding, info, defs) in tdefs) {
					var feature = CSharpConstants.GetDefine(encoding);
					if (feature is not null)
						writer.WriteLineNoIndent($"#if {feature}");
					foreach (var def in defs)
						writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(def.Code)}:");
					using (writer.Indent()) {
						switch (info.Kind) {
						case StackInfoKind.Increment:
							writer.WriteLine($"return {info.Value};");
							break;
						case StackInfoKind.Enter:
							writer.WriteLine($"return -({info.Value} + (Immediate8_2nd & 0x1F) * {info.Value} + Immediate16);");
							break;
						case StackInfoKind.Iret:
							writer.WriteLine($"return CodeSize == CodeSize.Code64 ? {info.Value} * 5 : {info.Value} * 3;");
							break;
						case StackInfoKind.PopImm16:
							writer.WriteLine($"return {info.Value} + Immediate16;");
							break;
						case StackInfoKind.None:
						default:
							throw new InvalidOperationException();
						}
					}
					if (feature is not null)
						writer.WriteLineNoIndent("#endif");
				}
			});
		}
	}
}
