// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Constants;
using Generator.Constants.Java;
using Generator.Enums;
using Generator.Enums.Java;
using Generator.Enums.InstructionInfo;
using Generator.IO;
using Generator.Tables;

namespace Generator.InstructionInfo.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaInstrInfoGenerator : InstrInfoGenerator {
		readonly IdentifierConverter idConverter;
		readonly JavaEnumsGenerator enumGenerator;
		readonly JavaConstantsGenerator constantsGenerator;
		readonly EnumType opAccessType;
		readonly EnumType registerType;
		readonly EnumType codeSizeType;

		public JavaInstrInfoGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = JavaIdentifierConverter.Create();
			enumGenerator = new JavaEnumsGenerator(generatorContext);
			constantsGenerator = new JavaConstantsGenerator(generatorContext);
			opAccessType = genTypes[TypeIds.OpAccess];
			registerType = genTypes[TypeIds.Register];
			codeSizeType = generatorContext.Types[TypeIds.CodeSize];
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);
		protected override void Generate(ConstantsType constantsType) => constantsGenerator.Generate(constantsType);

		protected override void Generate((InstructionDef def, uint dword1, uint dword2)[] infos) {
			var filename = JavaConstants.GetResourceFilename(genTypes, JavaConstants.InstructionInfoPackage, "InstrInfoTable.bin");
			using (var writer = new BinaryByteTableWriter(filename)) {
				foreach (var info in infos) {
					writer.WriteUInt32(info.dword1);
					writer.WriteUInt32(info.dword2);
				}
			}
		}

		protected override void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoInternalPackage, "RflagsInfoConstants.java");
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.InstructionInfoInternalPackage};");
				writer.WriteLine();
				writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
				writer.WriteLine("public final class RflagsInfoConstants {");
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
						writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
						writer.WriteLine($"public static final short[] {name} = new short[] {{");
						using (writer.Indent()) {
							for (int i = 0; i < rflags.Length; i++) {
								var rfl = rflags[i];
								uint value = (uint)rfl;
								if (value > ushort.MaxValue)
									throw new InvalidOperationException();
								writer.WriteLine($"(short)0x{value:X4},// {enumValues[i].Name(idConverter)}");
							}
						}
						writer.WriteLine("};");
					}
				}
				writer.WriteLine("}");
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

			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoInternalPackage, "CpuidFeatureInternalData.java");
			new FileUpdater(TargetLanguage.Java, "Table", filename).Generate(writer => {
				writer.WriteLine("private static byte[] getCpuidFeaturesData() {");
				using (writer.Indent()) {
					writer.WriteLine("return new byte[] {");
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
				writer.WriteLine("}");
			});
		}

		protected override void GenerateCore() => GenerateOpAccesses();

		void GenerateOpAccesses() {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoPackage, "OpAccessTables.java");
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.InstructionInfoPackage};");
				writer.WriteLine();
				writer.WriteLine("final class OpAccessTables {");
				using (writer.Indent()) {
					var opInfos = instrInfoTypes.EnumOpInfos;
					// We assume max op count is 5, update the code if not
					if (opInfos.Length != 5)
						throw new InvalidOperationException();

					var indexes = new int[] { 1, 2 };
					foreach (var index in indexes) {
						var opInfo = opInfos[index];
						writer.WriteLine($"static final int[] op{index} = new int[] {{");
						using (writer.Indent()) {
							foreach (var value in opInfo.Values) {
								var v = ToOpAccess(value);
								writer.WriteLine($"{idConverter.ToDeclTypeAndValue(v)},");
							}
						}
						writer.WriteLine("};");
					}
				}
				writer.WriteLine("}");
			}
		}

		protected override void GenerateImpliedAccesses(ImpliedAccessesDef[] defs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoPackage, "InstructionInfoFactory.java");
			new FileUpdater(TargetLanguage.Java, "ImpliedAccessHandler", filename).Generate(writer => GenerateImpliedAccesses(writer, defs));
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
					ImplAccConditionKind.Bit64 => "(flags & Flags.IS_64_BIT) != 0",
					ImplAccConditionKind.NotBit64 => "(flags & Flags.IS_64_BIT) == 0",
					_ => throw new InvalidOperationException(),
				};
		}

		void GenerateImpliedAccesses(FileWriter writer, List<ImplAccStatement> stmts) {
			var stmtState = new StmtState(
				"if ((flags & Flags.NO_REGISTER_USAGE) == 0) {",
				"}",
				"if ((flags & Flags.NO_MEMORY_USAGE) == 0) {",
				"}");
			foreach (var stmt in stmts) {
				IntArgImplAccStatement arg1;
				IntX2ArgImplAccStatement arg2;
				stmtState.SetKind(writer, stmt.Kind);
				switch (stmt.Kind) {
				case ImplAccStatementKind.MemoryAccess:
					var mem = (MemoryImplAccStatement)stmt;
					writer.WriteLine($"addMemory({GetRegisterString(mem.Segment)}, {GetRegisterString(mem.Base)}, {GetRegisterString(mem.Index)}, {mem.Scale}, 0x{mem.Displacement}, {GetMemorySizeString(mem.MemorySize)}, {GetOpAccessString(mem.Access)}, {GetCodeSizeString(mem.AddressSize)}, {mem.VsibSize});");
					break;
				case ImplAccStatementKind.RegisterAccess:
					var reg = (RegisterImplAccStatement)stmt;
					if (reg.IsMemOpSegRead && CouldBeNullSegIn64BitMode(reg.Register, out var definitelyNullSeg)) {
						if (definitelyNullSeg) {
							writer.WriteLine("if ((flags & Flags.IS_64_BIT) == 0)");
							using (writer.Indent())
								writer.WriteLine($"addRegister(flags, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
						}
						else
							writer.WriteLine($"addMemorySegmentRegister(flags, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
					}
					else
						writer.WriteLine($"addRegister(flags, {GetRegisterString(reg.Register)}, {GetOpAccessString(reg.Access)});");
					break;
				case ImplAccStatementKind.RegisterRangeAccess:
					var rreg = (RegisterRangeImplAccStatement)stmt;
					writer.WriteLine($"for (int reg = {GetEnumName(rreg.RegisterFirst)}; reg <= {GetEnumName(rreg.RegisterLast)}; reg++)");
					using (writer.Indent())
						writer.WriteLine($"addRegister(flags, reg, {GetOpAccessString(rreg.Access)});");
					break;
				case ImplAccStatementKind.ShiftMask:
					arg1 = (IntArgImplAccStatement)stmt;
					break;
				case ImplAccStatementKind.ShiftMask1FMod:
					arg1 = (IntArgImplAccStatement)stmt;
					Verify_9_or_17(arg1.Arg);
					break;
				case ImplAccStatementKind.ZeroRegRflags:
					writer.WriteLine("commandClearRflags(instruction, flags);");
					break;
				case ImplAccStatementKind.ZeroRegRegmem:
					writer.WriteLine("commandClearRegRegmem(instruction, flags);");
					break;
				case ImplAccStatementKind.ZeroRegRegRegmem:
					writer.WriteLine("commandClearRegRegRegmem(instruction, flags);");
					break;
				case ImplAccStatementKind.Arpl:
					writer.WriteLine("commandArpl(instruction, flags);");
					break;
				case ImplAccStatementKind.LastGpr8:
					writer.WriteLine($"commandLastGpr(instruction, flags, {GetEnumName(registerType[nameof(Register.AL)])});");
					break;
				case ImplAccStatementKind.LastGpr16:
					writer.WriteLine($"commandLastGpr(instruction, flags, {GetEnumName(registerType[nameof(Register.AX)])});");
					break;
				case ImplAccStatementKind.LastGpr32:
					writer.WriteLine($"commandLastGpr(instruction, flags, {GetEnumName(registerType[nameof(Register.EAX)])});");
					break;
				case ImplAccStatementKind.EmmiReg:
					var emmi = (EmmiImplAccStatement)stmt;
					writer.WriteLine($"commandEmmi(instruction, flags, {GetEnumName(GetOpAccess(opAccessType, emmi.Access))});");
					break;
				case ImplAccStatementKind.Enter:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"commandEnter(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Leave:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"commandLeave(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Push:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					if (arg2.Arg1 != 0)
						writer.WriteLine($"commandPush(instruction, flags, {arg2.Arg1}, {Verify_2_4_or_8(arg2.Arg2)});");
					break;
				case ImplAccStatementKind.Pop:
					arg2 = (IntX2ArgImplAccStatement)stmt;
					if (arg2.Arg1 != 0)
						writer.WriteLine($"commandPop(instruction, flags, {arg2.Arg1}, {Verify_2_4_or_8(arg2.Arg2)});");
					break;
				case ImplAccStatementKind.PopRm:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"commandPopRm(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Pusha:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"commandPusha(instruction, flags, {Verify_2_or_4(arg1.Arg)});");
					break;
				case ImplAccStatementKind.Popa:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"commandPopa(instruction, flags, {Verify_2_or_4(arg1.Arg)});");
					break;
				case ImplAccStatementKind.lea:
					writer.WriteLine("commandLea(instruction, flags);");
					break;
				case ImplAccStatementKind.Cmps:
					writer.WriteLine("commandCmps(instruction, flags);");
					break;
				case ImplAccStatementKind.Ins:
					writer.WriteLine("commandIns(instruction, flags);");
					break;
				case ImplAccStatementKind.Lods:
					writer.WriteLine("commandLods(instruction, flags);");
					break;
				case ImplAccStatementKind.Movs:
					writer.WriteLine("commandMovs(instruction, flags);");
					break;
				case ImplAccStatementKind.Outs:
					writer.WriteLine("commandOuts(instruction, flags);");
					break;
				case ImplAccStatementKind.Scas:
					writer.WriteLine("commandScas(instruction, flags);");
					break;
				case ImplAccStatementKind.Stos:
					writer.WriteLine("commandStos(instruction, flags);");
					break;
				case ImplAccStatementKind.Xstore:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"commandXstore(instruction, flags, {Verify_2_4_or_8(arg1.Arg)});");
					break;
				case ImplAccStatementKind.MemDispl:
					arg1 = (IntArgImplAccStatement)stmt;
					writer.WriteLine($"commandMemDispl(flags, {(int)arg1.Arg});");
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
				return "instruction.getMemorySize()";
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
			case ImplAccRegisterKind.SegmentDefaultDS: return "getSegDefaultDS(instruction)";
			case ImplAccRegisterKind.a_rDI: return "getARDI(instruction)";
			case ImplAccRegisterKind.Op0: return "instruction.getOp0Register()";
			case ImplAccRegisterKind.Op1: return "instruction.getOp1Register()";
			case ImplAccRegisterKind.Op2: return "instruction.getOp2Register()";
			case ImplAccRegisterKind.Op3: return "instruction.getOp3Register()";
			case ImplAccRegisterKind.Op4: return "instruction.getOp4Register()";
			default: throw new InvalidOperationException();
			}
		}

		string GetOpAccessString(OpAccess access) => GetEnumName(opAccessType[access.ToString()]);
		string GetCodeSizeString(CodeSize codeSize) => GetEnumName(codeSizeType[codeSize.ToString()]);

		string GetEnumName(EnumValue value) => idConverter.ToDeclTypeAndValue(value);

		void GenerateTable((EncodingKind encoding, InstructionDef[] defs)[] tdefs, string id, string filename) {
			new FileUpdater(TargetLanguage.Java, id, filename).Generate(writer => {
				foreach (var (encoding, defs) in tdefs) {
					foreach (var def in defs)
						writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(def.Code)}:");
					using (writer.Indent())
						writer.WriteLine("return true;");
				}
			});
		}

		protected override void GenerateIgnoresSegmentTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Code.java");
			GenerateTable(defs, "IgnoresSegmentTable", filename);
		}

		protected override void GenerateIgnoresIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Code.java");
			GenerateTable(defs, "IgnoresIndexTable", filename);
		}

		protected override void GenerateTileStrideIndexTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Code.java");
			GenerateTable(defs, "TileStrideIndexTable", filename);
		}

		protected override void GenerateIsStringOpTable((EncodingKind encoding, InstructionDef[] defs)[] defs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Code.java");
			GenerateTable(defs, "IsStringOpTable", filename);
		}

		protected override void GenerateFpuStackIncrementInfoTable((FpuStackInfo info, InstructionDef[] defs)[] tdefs) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Instruction.java");
			new FileUpdater(TargetLanguage.Java, "FpuStackIncrementInfoTable", filename).Generate(writer => {
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
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Instruction.java");
			new FileUpdater(TargetLanguage.Java, "StackPointerIncrementTable", filename).Generate(writer => {
				foreach (var (encoding, info, defs) in tdefs) {
					foreach (var def in defs)
						writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(def.Code)}:");
					using (writer.Indent()) {
						switch (info.Kind) {
						case StackInfoKind.Increment:
							writer.WriteLine($"return {info.Value};");
							break;
						case StackInfoKind.Enter:
							writer.WriteLine($"return -({info.Value} + (getImmediate8_2nd() & 0x1F) * {info.Value} + (getImmediate16() & 0xFFFF));");
							break;
						case StackInfoKind.Iret:
							writer.WriteLine($"return getCodeSize() == CodeSize.CODE64 ? {info.Value} * 5 : {info.Value} * 3;");
							break;
						case StackInfoKind.PopImm16:
							writer.WriteLine($"return {info.Value} + (getImmediate16() & 0xFFFF);");
							break;
						case StackInfoKind.None:
						default:
							throw new InvalidOperationException();
						}
					}
				}
			});
		}
	}
}
