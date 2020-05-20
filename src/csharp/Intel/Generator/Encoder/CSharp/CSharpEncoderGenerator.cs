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
using System.IO;
using System.Linq;
using Generator.Enums;
using Generator.Enums.CSharp;
using Generator.IO;
using Generator.Tables;

namespace Generator.Encoder.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Encoder)]
	sealed class CSharpEncoderGenerator : EncoderGenerator {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;
		readonly CSharpEnumsGenerator enumGenerator;

		public CSharpEncoderGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = CSharpIdentifierConverter.Create();
			enumGenerator = new CSharpEnumsGenerator(generatorContext);
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);

		protected override void Generate((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex) {
			GenerateOpCodeOperandKindTables(legacy, vex, xop, evex);
			GenerateOpTables(legacy, vex, xop, evex);
		}

		void GenerateOpCodeOperandKindTables((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.EncoderNamespace), "OpCodeOperandKinds.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.OpCodeInfoDefine}");

				writer.WriteLine($"namespace {CSharpConstants.EncoderNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static class OpCodeOperandKinds {");
					using (writer.Indent()) {
						Generate(writer, "LegacyOpKinds", legacy);
						Generate(writer, "VexOpKinds", vex);
						Generate(writer, "XopOpKinds", xop);
						Generate(writer, "EvexOpKinds", evex);
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}

			void Generate(FileWriter writer, string name, (EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				var declTypeStr = genTypes[TypeIds.OpCodeOperandKind].Name(idConverter);
				writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
				writer.WriteLine($"public static System.ReadOnlySpan<byte> {name} => new byte[{table.Length}] {{");
				writer.WriteLineNoIndent("#else");
				writer.WriteLine($"public static readonly byte[] {name} = new byte[{table.Length}] {{");
				writer.WriteLineNoIndent("#endif");
				using (writer.Indent()) {
					foreach (var info in table)
						writer.WriteLine($"(byte){declTypeStr}.{info.opCodeOperandKind.Name(idConverter)},// {info.opKind.Name(idConverter)}");
				}
				writer.WriteLine("};");
			}
		}

		void GenerateOpTables((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.EncoderNamespace), "OpTables.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.EncoderDefine}");

				writer.WriteLine($"namespace {CSharpConstants.EncoderNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static class OpHandlerData {");
					using (writer.Indent()) {
						Generate(writer, "LegacyOps", legacy);
						Generate(writer, "VexOps", vex);
						Generate(writer, "XopOps", xop);
						Generate(writer, "EvexOps", evex);
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}

			void Generate(FileWriter writer, string name, (EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] table) {
				var declTypeStr = genTypes[TypeIds.OpCodeOperandKind].Name(idConverter);
				if (table[0].opHandlerKind != OpHandlerKind.None)
					throw new InvalidOperationException();
				writer.WriteLine($"public static readonly Op[] {name} = new Op[{table.Length - 1}] {{");
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
								writer.Write($"{value.DeclaringType.Name(idConverter)}.{value.Name(idConverter)}");
								break;
							case int value:
								writer.Write(value.ToString());
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

		protected override void GenerateOpCodeInfo(InstructionDef[] defs) {
			GenerateTable(defs);
			GenerateNonZeroOpMaskRegisterCode(defs);
		}

		void GenerateTable(InstructionDef[] defs) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.EncoderNamespace), "OpCodeHandlers.Data.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.EncoderDefine}");
				writer.WriteLine($"namespace {CSharpConstants.EncoderNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static partial class OpCodeHandlers {");
					using (writer.Indent()) {
						writer.WriteLine("public static uint[] GetData() =>");
						using (writer.Indent()) {
							writer.WriteLine($"new uint[{defs.Length} * 3] {{");
							using (writer.Indent()) {
								foreach (var info in GetData(defs))
									writer.WriteLine($"0x{info.dword1:X8}, 0x{info.dword2:X8}, 0x{info.dword3:X8},// {info.opCode.Code.Name(idConverter)}");
							}
							writer.WriteLine("};");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}
		}

		void GenerateNonZeroOpMaskRegisterCode(InstructionDef[] defs) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IcedNamespace), "OpCodeInfo.cs");
			var codeValues = defs.Where(def => (def.OpCodeInfo.Flags & OpCodeFlags.NonZeroOpMaskRegister) != 0).Select(def => def.OpCodeInfo.Code).ToArray();
			GenerateCases(filename, "NonZeroOpMaskRegister", codeValues, "flags |= Flags.NonZeroOpMaskRegister;");
		}

		protected override void Generate((EnumValue value, uint size)[] immSizes) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IcedNamespace), "Encoder.cs");
			new FileUpdater(TargetLanguage.CSharp, "ImmSizes", filename).Generate(writer => {
				writer.WriteLine($"static readonly uint[] s_immSizes = new uint[{immSizes.Length}] {{");
				using (writer.Indent()) {
					foreach (var info in immSizes)
						writer.WriteLine($"{info.size},// {info.value.Name(idConverter)}");
				}
				writer.WriteLine("};");
			});
		}

		protected override void Generate((EnumValue allowedPrefixes, OpCodeFlags prefixes)[] infos, (EnumValue value, OpCodeFlags flag)[] flagsInfos) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IcedNamespace), "OpCodeInfo.cs");
			new FileUpdater(TargetLanguage.CSharp, "AllowedPrefixes", filename).Generate(writer => {
				foreach (var info in infos) {
					writer.Write($"{info.allowedPrefixes.DeclaringType.Name(idConverter)}.{info.allowedPrefixes.Name(idConverter)} => ");
					WriteFlags(writer, idConverter, info.prefixes, flagsInfos, " | ", ".", false);
					writer.WriteLine(",");
				}
			});
		}

		void GenerateCases(string filename, string id, EnumValue[] codeValues, params string[] statements) {
			new FileUpdater(TargetLanguage.CSharp, id, filename).Generate(writer => {
				if (codeValues.Length == 0)
					return;
				foreach (var value in codeValues)
					writer.WriteLine($"case {value.DeclaringType.Name(idConverter)}.{value.Name(idConverter)}:");
				using (writer.Indent()) {
					foreach (var statement in statements)
						writer.WriteLine(statement);
					if (!statements[statements.Length - 1].StartsWith("return "))
						writer.WriteLine("break;");
				}
			});
		}

		void GenerateNotInstrCases(string filename, string id, (EnumValue code, string result)[] notInstrStrings) {
			new FileUpdater(TargetLanguage.CSharp, id, filename).Generate(writer => {
				foreach (var info in notInstrStrings)
					writer.WriteLine($"{info.code.DeclaringType.Name(idConverter)}.{info.code.Name(idConverter)} => \"{info.result}\",");
			});
		}

		protected override void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] opMaskIsK1, EnumValue[] incVecIndex, EnumValue[] noVecIndex, EnumValue[] swapVecIndex12, EnumValue[] fpuStartOpIndex1) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.EncoderNamespace), "InstructionFormatter.cs");
			GenerateNotInstrCases(filename, "InstrFmtNotInstructionString", notInstrStrings);
			GenerateCases(filename, "OpMaskIsK1", opMaskIsK1, "opMaskIsK1 = true;");
			GenerateCases(filename, "IncVecIndex", incVecIndex, "vec_index++;");
			GenerateCases(filename, "NoVecIndex", noVecIndex, "noVecIndex = true;");
			GenerateCases(filename, "SwapVecIndex12", swapVecIndex12, "swapVecIndex12 = true;");
			GenerateCases(filename, "FpuStartOpIndex1", fpuStartOpIndex1, "startOpIndex = 1;");
		}

		protected override void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.EncoderNamespace), "OpCodeFormatter.cs");
			GenerateNotInstrCases(filename, "OpCodeFmtNotInstructionString", notInstrStrings);
			GenerateCases(filename, "HasModRM", hasModRM, "return true;");
			GenerateCases(filename, "HasVsib", hasVsib, "return true;");
		}

		protected override void GenerateCore() {
		}

		protected override void GenerateInstrSwitch(EnumValue[] jccInstr, EnumValue[] simpleBranchInstr, EnumValue[] callInstr, EnumValue[] jmpInstr, EnumValue[] xbeginInstr) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.BlockEncoderNamespace), "Instr.cs");
			GenerateCases(filename, "JccInstr", jccInstr, "return new JccInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "SimpleBranchInstr", simpleBranchInstr, "return new SimpleBranchInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "CallInstr", callInstr, "return new CallInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "JmpInstr", jmpInstr, "return new JmpInstr(blockEncoder, block, instruction);");
			GenerateCases(filename, "XbeginInstr", xbeginInstr, "return new XbeginInstr(blockEncoder, block, instruction);");
		}

		protected override void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IcedNamespace), "Instruction.cs");
			GenerateCases(filename, "Vsib32", vsib32, "vsib64 = false;", "return true;");
			GenerateCases(filename, "Vsib64", vsib64, "vsib64 = true;", "return true;");
		}
	}
}
