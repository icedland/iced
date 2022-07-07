// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Java;
using Generator.IO;
using Generator.Tables;

namespace Generator.Encoder.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaEncoderGenerator : EncoderGenerator {
		readonly IdentifierConverter idConverter;
		readonly JavaEnumsGenerator enumGenerator;

		public JavaEncoderGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = JavaIdentifierConverter.Create();
			enumGenerator = new JavaEnumsGenerator(generatorContext);
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);

		protected override void Generate(OpCodeHandlers handlers) {
			GenerateOpCodeOperandKindTables(handlers);
			GenerateOpTables(handlers);
		}

		void GenerateOpCodeOperandKindTables(OpCodeHandlers handlers) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoPackage, "OpCodeOperandKinds.java");
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				writer.WriteLine($"package {JavaConstants.InstructionInfoPackage};");
				writer.WriteLine();
				writer.WriteLine("final class OpCodeOperandKinds {");
				using (writer.Indent()) {
					Generate(writer, "legacyOpKinds", handlers.Legacy);
					Generate(writer, "vexOpKinds", handlers.Vex);
					Generate(writer, "xopOpKinds", handlers.Xop);
					Generate(writer, "evexOpKinds", handlers.Evex);
					Generate(writer, "mvexOpKinds", handlers.Mvex);
				}
				writer.WriteLine("}");
			}

			void Generate(FileWriter writer, string name, (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				writer.WriteLine($"static final byte[] {name} = new byte[] {{");
				using (writer.Indent()) {
					foreach (var info in table)
						writer.WriteLine($"(byte){idConverter.ToDeclTypeAndValue(info.opCodeOperandKind)},");
				}
				writer.WriteLine("};");
			}
		}

		void GenerateOpTables(OpCodeHandlers handlers) {
			var className = "OpTables";
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.EncoderPackage, className + ".java");
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();

				writer.WriteLine($"package {JavaConstants.EncoderPackage};");
				writer.WriteLine();
				writer.WriteLine("import com.github.icedland.iced.x86.OpKind;");
				writer.WriteLine("import com.github.icedland.iced.x86.Register;");
				writer.WriteLine();
				writer.WriteLine($"final class {className} {{");
				using (writer.Indent()) {
					Generate(writer, "legacyOps", handlers.Legacy);
					Generate(writer, "vexOps", handlers.Vex);
					Generate(writer, "xopOps", handlers.Xop);
					Generate(writer, "evexOps", handlers.Evex);
					Generate(writer, "mvexOps", handlers.Mvex);
				}
				writer.WriteLine("}");
			}

			void Generate(FileWriter writer, string name, (EnumValue opCodeOperandKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				var declTypeStr = genTypes[TypeIds.OpCodeOperandKind].Name(idConverter);
				if (table[0].opHandlerKind != OpHandlerKind.None)
					throw new InvalidOperationException();
				writer.WriteLine($"static final Op[] {name} = new Op[] {{");
				using (writer.Indent()) {
					for (int i = 1; i < table.Length; i++) {
						var info = table[i];
						writer.Write("new ");
						writer.Write(info.opHandlerKind.ToString());
						writer.Write("(");
						var ctorArgs = info.args;
						for (int j = 0; j < ctorArgs.Length; j++) {
							if (j > 0)
								writer.Write(", ");
							switch (ctorArgs[j]) {
							case EnumValue value:
								writer.Write(idConverter.ToDeclTypeAndValue(value));
								break;
							case int value:
								writer.Write(value.ToString());
								break;
							case bool value:
								writer.Write(value ? "true" : "false");
								break;
							default:
								throw new InvalidOperationException();
							}
						}
						writer.WriteLine("),");
					}
				}
				writer.WriteLine("};");
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
				("encFlags1", encFlags1),
				("encFlags2", encFlags2),
				("encFlags3", encFlags3),
			};
			var opCodeInfo = new (string name, (InstructionDef def, uint value)[] values)[] {
				("opcFlags1", opcFlags1),
				("opcFlags2", opcFlags2),
			};

			GenerateTables(defs, encoderInfo, "EncoderData", JavaConstants.EncoderPackage);
			GenerateTables(defs, opCodeInfo, "OpCodeInfoData", JavaConstants.InstructionInfoPackage);
			GenerateTables(mvexInfos, "MvexInfoData");
			GenerateTables(mvexTupleTypeData, "MvexTupleTypeLut", JavaConstants.IcedPackage);
			GenerateTables(mvexMemorySizeData, "MvexMemorySizeLut", JavaConstants.IcedPackage);
		}

		void GenerateTables(InstructionDef[] defs, (string name, (InstructionDef def, uint value)[] values)[] tableData, string className, string package) {
			foreach (var info in tableData) {
				var rsrcFilename = JavaConstants.GetResourceFilename(genTypes, package, className + "." + info.name + ".bin");
				using (var writer = new BinaryByteTableWriter(rsrcFilename)) {
					foreach (var vinfo in info.values) {
						var v = vinfo.value;
						writer.WriteByte((byte)v);
						writer.WriteByte((byte)(v >> 8));
						writer.WriteByte((byte)(v >> 16));
						writer.WriteByte((byte)(v >> 24));
					}
				}
			}
		}

		void GenerateTables((InstructionDef def, MvexEncInfo mvex)[] mvexInfos, string className) {
			var infos = mvexInfos.Where(x => x.def.Encoding == EncodingKind.MVEX).ToArray();
			var srcFilename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedInternalPackage, className + ".java");
			var rsrcFilename = JavaConstants.GetResourceFilename(genTypes, JavaConstants.IcedPackage, className + ".bin");
			const int StructSize = 8;
			const int TupleTypeLutKindIndex = 0;
			const int EHBitIndex = 1;
			const int ConvFnIndex = 2;
			const int InvalidConvFnsIndex = 3;
			const int InvalidSwizzleFnsIndex = 4;
			const int Flags1Index = 5;
			const int Flags2Index = 6;
			using (var writer = new BinaryByteTableWriter(rsrcFilename)) {
				var data = new byte[StructSize];
				foreach (var (def, mvex) in infos) {
					data[TupleTypeLutKindIndex] = (byte)mvex.TupleTypeLutKind.Value;
					data[EHBitIndex] = (byte)mvex.EHBit.Value;
					data[ConvFnIndex] = (byte)mvex.ConvFn.Value;
					data[InvalidConvFnsIndex] = mvex.InvalidConvFns;
					data[InvalidSwizzleFnsIndex] = mvex.InvalidSwizzleFns;
					data[Flags1Index] = (byte)mvex.Flags1;
					data[Flags2Index] = (byte)mvex.Flags2;
					foreach (var b in data)
						writer.WriteByte(b);
				}
			}
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(srcFilename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.IcedInternalPackage};");
				writer.WriteLine();
				writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
				writer.WriteLine($"public final class {className} {{");
				using (writer.Indent()) {
					var vars = new[] {
						("STRUCT_SIZE", StructSize),
						("TUPLE_TYPE_LUT_KIND_INDEX", TupleTypeLutKindIndex),
						("EH_BIT_INDEX", EHBitIndex),
						("CONV_FN_INDEX", ConvFnIndex),
						("INVALID_CONV_FNS_INDEX", InvalidConvFnsIndex),
						("INVALID_SWIZZLE_FNS_INDEX", InvalidSwizzleFnsIndex),
						("FLAGS1_INDEX", Flags1Index),
						("FLAGS2_INDEX", Flags2Index),
					};
					foreach (var (name, value) in vars) {
						writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
						writer.WriteLine($"public static final int {name} = {value};");
					}
				}
				writer.WriteLine("}");
			}
		}

		void GenerateTables((MvexTupleTypeLutKind ttLutKind, EnumValue[] enumValues)[] mvexData, string className, string valuePackageName) {
			var fullFilename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedInternalPackage, className + ".java");
			using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(fullFilename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"package {JavaConstants.IcedInternalPackage};");
				writer.WriteLine();
				var typeName = mvexData[0].enumValues[0].DeclaringType.Name(idConverter);
				writer.WriteLine($"import {valuePackageName}.{typeName};");
				writer.WriteLine();
				writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
				writer.WriteLine($"public final class {className} {{");
				using (writer.Indent()) {
					writer.WriteLine($"/** {JavaConstants.InternalDoc} */");
					writer.WriteLine("public static final byte[] data = new byte[] {");
					using (writer.Indent()) {
						foreach (var (ttLutKind, enumValues) in mvexData) {
							var ttLutKindValue = genTypes[TypeIds.MvexTupleTypeLutKind][ttLutKind.ToString()];
							writer.WriteLine($"// {idConverter.ToDeclTypeAndValue(ttLutKindValue)}");
							for (int i = 0; i < enumValues.Length; i++) {
								var enumValue = enumValues[i];
								if (enumValue.Value > byte.MaxValue)
									throw new InvalidOperationException();
								writer.WriteLine($"(byte){idConverter.ToDeclTypeAndValue(enumValue)},// {i}");
							}
						}
					}
					writer.WriteLine("};");
				}
				writer.WriteLine("}");
			}
		}

		protected override void Generate((EnumValue value, uint size)[] immSizes) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.EncoderPackage, "Encoder.java");
			new FileUpdater(TargetLanguage.Java, "ImmSizes", filename).Generate(writer => {
				writer.WriteLine("private static final int[] s_immSizes = new int[] {");
				using (writer.Indent()) {
					foreach (var info in immSizes)
						writer.WriteLine($"{info.size},// {info.value.Name(idConverter)}");
				}
				writer.WriteLine("};");
			});
		}

		void GenerateCases(string filename, string id, EnumValue[] codeValues, params string[] statements) =>
			new FileUpdater(TargetLanguage.Java, id, filename).Generate(writer => {
				if (codeValues.Length == 0)
					return;
				foreach (var value in codeValues)
					writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(value)}:");
				using (writer.Indent()) {
					foreach (var statement in statements)
						writer.WriteLine(statement);
					if (!statements[^1].StartsWith("return ", StringComparison.Ordinal))
						writer.WriteLine("break;");
				}
			});

		void GenerateNotInstrCases(string filename, string id, (EnumValue code, string result)[] notInstrStrings) =>
			new FileUpdater(TargetLanguage.Java, id, filename).Generate(writer => {
				foreach (var info in notInstrStrings) {
					writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(info.code)}:");
					using (writer.Indent()) {
						writer.WriteLine($"return \"{info.result}\";");
					}
				}
			});

		protected override void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoPackage, "InstructionFormatter.java");
			GenerateNotInstrCases(filename, "InstrFmtNotInstructionString", notInstrStrings);
		}

		protected override void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoPackage, "OpCodeFormatter.java");
			GenerateNotInstrCases(filename, "OpCodeFmtNotInstructionString", notInstrStrings);
			GenerateCases(filename, "HasModRM", hasModRM, "return true;");
			GenerateCases(filename, "HasVsib", hasVsib, "return true;");
		}

		protected override void GenerateCore() {
		}

		protected override void GenerateInstrSwitch(EnumValue[] jccInstr, EnumValue[] simpleBranchInstr, EnumValue[] callInstr, EnumValue[] jmpInstr, EnumValue[] xbeginInstr) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.BlockEncoderPackage, "Instr.java");
			GenerateCases(filename, "JccInstr", jccInstr, "return new JccInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "SimpleBranchInstr", simpleBranchInstr, "return new SimpleBranchInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "CallInstr", callInstr, "return new CallInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "JmpInstr", jmpInstr, "return new JmpInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "XbeginInstr", xbeginInstr, "return new XbeginInstr(blockEncoder, block, instruction);");
		}

		protected override void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Instruction.java");
			GenerateCases(filename, "Vsib32", vsib32, "return VsibFlags.VSIB | VsibFlags.VSIB32;");
			GenerateCases(filename, "Vsib64", vsib64, "return VsibFlags.VSIB | VsibFlags.VSIB64;");
		}

		protected override void GenerateDecoderOptionsTable((EnumValue decOptionValue, EnumValue decoderOptions)[] values) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoPackage, "OpCodeInfo.java");
			new FileUpdater(TargetLanguage.Java, "ToDecoderOptionsTable", filename).Generate(writer => {
				foreach (var (_, decoderOptions) in values)
					writer.WriteLine($"{idConverter.ToDeclTypeAndValue(decoderOptions)},");
			});
		}

		protected override void GenerateImpliedOps((EncodingKind Encoding, InstrStrImpliedOp[] Ops, InstructionDef[] defs)[] impliedOpsInfo) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.InstructionInfoPackage, "InstructionFormatter.java");
			new FileUpdater(TargetLanguage.Java, "PrintImpliedOps", filename).Generate(writer => {
				foreach (var info in impliedOpsInfo) {
					foreach (var def in info.defs)
						writer.WriteLine($"case {idConverter.ToDeclTypeAndValue(def.Code)}:");
					using (writer.Indent()) {
						foreach (var op in info.Ops) {
							writer.WriteLine("writeOpSeparator();");
							writer.WriteLine($"write(\"{op.Operand}\", {(op.IsUpper ? "true" : "false")});");
						}
						writer.WriteLine("break;");
					}
				}
			});
		}
	}
}
