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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Generator.Documentation.CSharp;
using Generator.Encoder;
using Generator.Enums;
using Generator.IO;

namespace Generator.Assembler.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Encoder)]
	sealed class CSharpAssemblerSyntaxGenerator : AssemblerSyntaxGenerator {
		readonly GeneratorOptions _generatorOptions;
		readonly CSharpDocCommentWriter docWriter;

		public CSharpAssemblerSyntaxGenerator(GeneratorOptions generatorOptions) {
			Converter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(Converter);
			_generatorOptions = generatorOptions;
		}

		protected override void GenerateRegisters(EnumType registers) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(_generatorOptions, CSharpConstants.IcedNamespace), "Assembler", "AssemblerRegisters.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.EncoderDefine}");

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				writer.WriteLine("#pragma warning disable 1591 // Missing XML comment for publicly visible type or member");
				using (writer.Indent()) {
					writer.WriteLine("public static partial class AssemblerRegisters {");
					using (writer.Indent()) {
						foreach (var register in registers.Values) {
							if (register.Value == 0) {
								continue;
							}
							var name = register.Name(Converter);
							writer.WriteLine($"public static readonly AssemblerRegister {name.ToLowerInvariant()} = Register.{name};");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}
		}

		protected override void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {
			GenerateCode(map, groups);
			GenerateTests(map, groups);
		}
		
		void GenerateCode(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(_generatorOptions, CSharpConstants.IcedNamespace), "Assembler", "Assembler.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.EncoderDefine}");
				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("using System;");
					writer.WriteLine("public sealed partial class Assembler {");
					using (writer.Indent()) {
						foreach (var group in groups) {
							var renderArgs = GetRenderArgs(group);
							var methodName = Converter.Method(group.Name);
							RenderCode(writer, methodName, group, renderArgs);
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}
		}
		
		void GenerateTests(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {
			const string assemblerTestsNameBase = "AssemblerTests";

			foreach (var bitness in new int[] {64, 32, 16}) {
				string testName = assemblerTestsNameBase + bitness;
			
				var filenameTests = Path.Combine(Path.Combine(_generatorOptions.CSharpTestsDir, "Intel", assemblerTestsNameBase, $"{testName}.g.cs"));
				using (var writerTests = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filenameTests))) {
					writerTests.WriteFileHeader();
					writerTests.WriteLine($"#if {CSharpConstants.EncoderDefine}");
					writerTests.WriteLine($"namespace {CSharpConstants.IcedUnitTestsNamespace}.{assemblerTestsNameBase} {{");
					using (writerTests.Indent()) {
						writerTests.WriteLine("using System;");
						writerTests.WriteLine("using System.Linq;");
						writerTests.WriteLine("using System.Text;");
						writerTests.WriteLine("using Iced.Intel;");
						writerTests.WriteLine("using Xunit;");

						writerTests.WriteLine($"public sealed class {testName} : AssemblerTests {{");
						using (writerTests.Indent()) {
							writerTests.WriteLine($"public {testName}() : base({bitness}) {{ }}");
							writerTests.WriteLine();
							
							foreach (var group in groups) {
								var renderArgs = GetRenderArgs(group);
								var methodName = Converter.Method(group.Name);
								RenderTests(writerTests, methodName, group, renderArgs);
							}
						}
						writerTests.WriteLine("}");					
					}
					writerTests.WriteLine("}");
					writerTests.WriteLine("#endif");
				}
			}
		}

		List<RenderArg> GetRenderArgs(OpCodeInfoGroup group) {
			var renderArgs = new List<RenderArg>();

			int immArg = 0;
							
			var signature = group.Signature;
			for (int i = 0; i < signature.ArgCount; i++) {
				string argName = i == 0 ? "dst" : i == 1 ? "src" : $"arg{i}";
				string argType = $"arg{i}";
				int maxArgSize = group.MaxArgSizes[i];
				var argKind = signature.GetArgKind(i);
								
				switch (argKind) {
				case ArgKind.Register:
					argType = "AssemblerRegister";
					break;
								
				case ArgKind.Label:
					argType = "Label";
					break;

				case ArgKind.RegisterMemory:
					argType = "AssemblerMemoryOperand";
					break;
				case ArgKind.Memory:
					argType = "AssemblerMemoryOperand";
					break;
								
				case ArgKind.Immediate:
					argName = $"imm{(immArg == 0 ? "" : immArg.ToString(CultureInfo.InvariantCulture))}";
					immArg++;
					Debug.Assert(maxArgSize > 0 && maxArgSize <= 8);
					argType = maxArgSize == 8 ? "long" : maxArgSize == 4 ? "int" : maxArgSize == 2 ? "short" : "byte";
					break;
								
				case ArgKind.ImmediateByte:
					argName = $"imm{(immArg == 0 ? "" : immArg.ToString(CultureInfo.InvariantCulture))}";
					immArg++;
					argType = "byte";
					break;								

				default:
					throw new ArgumentOutOfRangeException($"{argKind}");
				}

				renderArgs.Add(new RenderArg(argName, argType, argKind));
			}
			return renderArgs;
		}

		void RenderCode(FileWriter writer, string methodName, OpCodeInfoGroup group, List<RenderArg> renderArgs) {
			// Write documentation
			var methodDoc = new StringBuilder();
			methodDoc.Append($"{group.Name} instruction.");
			foreach (var code in group.Items) {
				if (!string.IsNullOrEmpty(code.Code.Documentation)) {
					methodDoc.Append("#(p:)##(p:)#");
					methodDoc.Append(code.Code.Documentation);
				}
			}

			docWriter.WriteSummary(writer, methodDoc.ToString(), "");

			writer.Write($"public void {methodName}(");
			int realArgCount = 0;
			for (var i = 0; i < renderArgs.Count; i++) {
				var renderArg = renderArgs[i];
				if (realArgCount > 0) writer.Write(", ");
				writer.Write($"{renderArg.Type} {renderArg.Name}");
				realArgCount++;
			}

			writer.WriteLine(") {");
			using (writer.Indent()) {
				if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0) {
					writer.Write($"AddInstruction(Instruction.Create{group.MemoName}(Bitness");
					for (var i = 0; i < renderArgs.Count; i++) {
						var renderArg = renderArgs[i];
						writer.Write(", ");
						writer.Write(renderArg.Name);
					}

					writer.WriteLine("));");
				}
				else {

					writer.WriteLine("Code op;");
					GenerateOpCodeSelector(writer, group, renderArgs);

					if (group.HasLabel) {
						writer.Write("AddInstruction(Instruction.CreateBranch(op");
					}
					else {
						writer.Write("AddInstruction(Instruction.Create(op");
					}

					for (var i = 0; i < renderArgs.Count; i++) {
						var renderArg = renderArgs[i];
						writer.Write(", ");
						writer.Write(renderArg.Name);
						if (renderArg.Kind == ArgKind.Label) {
							writer.Write(".Id");
						}
					}

					writer.Write(")");

					bool hasFlags = false;
					if ((group.Flags & (OpCodeArgFlags.HasKMask | OpCodeArgFlags.HasZeroingMask)) != 0) {
						writer.Write($", {renderArgs[0].Name}.Flags");
						hasFlags = true;
					}

					if ((group.Flags & OpCodeArgFlags.HasBroadcast) != 0) {
						for (int i = renderArgs.Count - 1; i >= 0; i--) {
							if (renderArgs[i].Kind == ArgKind.RegisterMemory || renderArgs[i].Kind == ArgKind.Memory) {
								if (hasFlags) {
									writer.Write(" | ");
								}
								else {
									writer.Write(", ");
									hasFlags = true;
								}

								writer.Write($"{renderArgs[i].Name}.Flags");
							}
						}
					}

					writer.WriteLine(");");
				}
			}

			writer.WriteLine("}");
		}
		
		void RenderTests(FileWriter writer, string methodName, OpCodeInfoGroup group, List<RenderArg> renderArgs) {
			var fullMethodName = new StringBuilder();
			fullMethodName.Append(methodName);
			foreach (var renderArg in renderArgs) {
				fullMethodName.Append('_');
				switch (renderArg.Kind) {
				case ArgKind.Register:
					fullMethodName.Append("reg");
					break;
				case ArgKind.RegisterMemory:
					fullMethodName.Append("rm");
					break;
				case ArgKind.Memory:
					fullMethodName.Append("m");
					break;
				case ArgKind.Immediate:
					fullMethodName.Append("i");
					break;
				case ArgKind.ImmediateByte:
					fullMethodName.Append("ib");
					break;
				case ArgKind.Label:
					fullMethodName.Append("l");
					break;
				default:
					throw new ArgumentOutOfRangeException($"{renderArg.Kind}");
				}
			}

			writer.WriteLine("[Fact]");
			writer.WriteLine($"public void {fullMethodName}() {{");
			using (writer.Indent()) {
				// Generate simple test for one opcode
				if (group.Items.Count == 1 && renderArgs.Count == 0 && !(group.Items[0] is D3nowOpCodeInfo)) {
					writer.WriteLine($"TestAssembler(c => c.{methodName}(), ins => ins.Code == Code.{group.Items[0].Code.Name(Converter)});");
				}
				else {
					// TODO: We have more than one opcode to test in this method
				}
			}
			writer.WriteLine("}");
			writer.WriteLine();;
		}		

		void GenerateOpCodeSelector(FileWriter writer, OpCodeInfoGroup group, List<RenderArg> args) {
			GenerateOpCodeSelector(writer, group, true, group.RootOpCodeNode, args);
		}
		
		void GenerateOpCodeSelector(FileWriter writer, OpCodeInfoGroup group, bool isLeaf, OpCodeNode node, List<RenderArg> args) {
			var opCodeInfo = node.OpCodeInfo;
			if (opCodeInfo != null) {
				if (isLeaf) {
					writer.Write("op = ");
				}
				writer.Write($"Code.{opCodeInfo.Code.Name(Converter)}");
				if (isLeaf) {
					writer.WriteLine(";");
				}
			}
			else {
				var selector = node.Selector;
				Debug.Assert(selector != null);
				var condition = GetArgConditionForOpCodeKind(selector.ArgIndex >= 0 ? args[selector.ArgIndex] : default, selector.Kind);
				if (selector.IsConditionInlineable) {
					writer.Write($"op = {condition} ? ");
					GenerateOpCodeSelector(writer, group, false, selector.IfTrue, args);
					writer.Write(" : ");
					GenerateOpCodeSelector(writer, group, false, selector.IfFalse, args);
					writer.WriteLine(";");
				}
				else {
					writer.WriteLine($"if ({condition}) {{");
					using (writer.Indent()) {
						GenerateOpCodeSelector(writer, group, true, selector.IfTrue, args);

						if (selector.Kind == OpCodeSelectorKind.MemOffs64) {
							var argIndex = selector.ArgIndex;
							if (argIndex == 1) {
								writer.WriteLine($"AddInstruction(Instruction.CreateMemory64(op, {args[0].Name}, (ulong){args[1].Name}.Displacement, {args[1].Name}.Prefix));");
							}
							else {
								writer.WriteLine($"AddInstruction(Instruction.CreateMemory64(op, (ulong){args[0].Name}.Displacement, {args[1].Name}, {args[0].Name}.Prefix));");
							}
							writer.WriteLine("return;");
						}
					}

					writer.Write("} else ");
					if (!selector.IfFalse.IsEmpty) {
						GenerateOpCodeSelector(writer, group, true, selector.IfFalse, args);
					}
					else {
						writer.WriteLine("{");
						using (writer.Indent()) {
							writer.Write($"throw NoOpCodeFoundFor(Mnemonic.{group.MemoName}");
							for (var i = 0; i < args.Count; i++) {
								var renderArg = args[i];
								writer.Write(", ");
								writer.Write(renderArg.Name);
							}

							writer.WriteLine(");");
						}

						writer.WriteLine("}");
					}
				}
			}
		}

		static string GetArgConditionForOpCodeKind(RenderArg arg, OpCodeSelectorKind selectorKind) {
			var regName = arg.Name;
			switch (selectorKind) {
			case OpCodeSelectorKind.MemOffs64:
				return $"{regName}.IsDisplacement64BitOnly";
			case OpCodeSelectorKind.Bitness64:
				return "Bitness == 64";
			case OpCodeSelectorKind.Bitness32:
				return "Bitness >= 32";
			case OpCodeSelectorKind.Bitness16:
				return "Bitness >= 16";
			case OpCodeSelectorKind.BranchShort:
				return "PreferBranchShort";
			case OpCodeSelectorKind.ImmediateByteEqual1:
				return $"{regName} == 1";
			case OpCodeSelectorKind.ImmediateByteSigned:
				return $"{regName} >= sbyte.MinValue &&  {regName} <= byte.MaxValue";
			case OpCodeSelectorKind.Vex:
				return "PreferVex";
			case OpCodeSelectorKind.RegisterCL:
				return $"{regName} == Register.CL";
			case OpCodeSelectorKind.RegisterAL:
				return $"{regName} == Register.AL";
			case OpCodeSelectorKind.RegisterAX:
				return $"{regName} == Register.AX";
			case OpCodeSelectorKind.RegisterEAX:
				return $"{regName} == Register.EAX";
			case OpCodeSelectorKind.RegisterRAX:
				return $"{regName} == Register.RAX";
			case OpCodeSelectorKind.RegisterBND:
				return $"{regName} == Register.IsBND()";
			case OpCodeSelectorKind.RegisterES:
				return $"{regName} == Register.ES";
			case OpCodeSelectorKind.RegisterCS:
				return $"{regName} == Register.CS";
			case OpCodeSelectorKind.RegisterSS:
				return $"{regName} == Register.SS";
			case OpCodeSelectorKind.RegisterDS:
				return $"{regName} == Register.DS";
			case OpCodeSelectorKind.RegisterFS:
				return $"{regName} == Register.FS";
			case OpCodeSelectorKind.RegisterGS:
				return $"{regName} == Register.GS";
			case OpCodeSelectorKind.RegisterDX:
				return $"{regName} == Register.DX";
			case OpCodeSelectorKind.Register8:
				return $"{regName}.IsGPR8()";
			case OpCodeSelectorKind.Register16:
				return $"{regName}.IsGPR16()";
			case OpCodeSelectorKind.Register32:
				return $"{regName}.IsGPR32()";
			case OpCodeSelectorKind.Register64:
				return $"{regName}.IsGPR64()";
			case OpCodeSelectorKind.RegisterK:
				return $"{regName}.IsK()";
			case OpCodeSelectorKind.RegisterST0:
				return $"{regName} == Register.ST0";
			case OpCodeSelectorKind.RegisterST:
				return $"{regName}.IsST()";
			case OpCodeSelectorKind.RegisterSegment:
				return $"{regName}.IsSegmentRegister()";
			case OpCodeSelectorKind.RegisterCR:
				return $"{regName}.IsCR()";
			case OpCodeSelectorKind.RegisterDR:
				return $"{regName}.IsDR()";
			case OpCodeSelectorKind.RegisterTR:
				return $"{regName}.IsTR()";
			case OpCodeSelectorKind.RegisterMM:
				return $"{regName}.IsMM()";
			case OpCodeSelectorKind.RegisterXMM:
				return $"{regName}.IsXMM()";
			case OpCodeSelectorKind.RegisterYMM:
				return $"{regName}.IsYMM()";
			case OpCodeSelectorKind.RegisterZMM:
				return $"{regName}.IsZMM()";
			case OpCodeSelectorKind.Memory8:
				return $"{regName}.Size == MemoryOperandSize.BytePtr";
			case OpCodeSelectorKind.Memory16:
				return $"{regName}.Size == MemoryOperandSize.WordPtr";
			case OpCodeSelectorKind.Memory32:
				return $"{regName}.Size == MemoryOperandSize.DwordPtr";
			case OpCodeSelectorKind.Memory80:
				return $"{regName}.Size == MemoryOperandSize.TwordPtr";
			case OpCodeSelectorKind.Memory64:
			case OpCodeSelectorKind.MemoryMM:
				return $"{regName}.Size == MemoryOperandSize.QwordPtr";
			case OpCodeSelectorKind.MemoryXMM:
				return $"{regName}.Size == MemoryOperandSize.OwordPtr";
			case OpCodeSelectorKind.MemoryYMM:
				return $"{regName}.Size == MemoryOperandSize.YwordPtr";
			case OpCodeSelectorKind.MemoryZMM:
				return $"{regName}.Size == MemoryOperandSize.ZwordPtr";
			default:
				return $"invalid_selector_{selectorKind}_for_arg_{regName}";
			}
		}
		
		struct RenderArg {
			public RenderArg(string name, string type, ArgKind kind) {
				Name = name;
				Type = type;
				Kind = kind;
			}

			public string Name;

			public string Type;

			public ArgKind Kind;
		}
	}
}
