using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Documentation.CSharp;
using Generator.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
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

			var filename = Path.Combine(CSharpConstants.GetDirectory(_generatorOptions, CSharpConstants.IcedNamespace), "Assembler", "Assembler.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.EncoderDefine}");
				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("using System;");
					writer.WriteLine("public sealed partial class Assembler {");
					using (writer.Indent()) {

						var renderArgs = new List<RenderArg>();

						foreach (var group in groups) {
							renderArgs.Clear();

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
								
								case ArgKind.Branch:
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
								
								case ArgKind.HiddenMemory:
									argName = "<none>";
									argType = "<none>";
									break;
								default:
									throw new ArgumentOutOfRangeException();
								}

								renderArgs.Add(new RenderArg(argName, argType, argKind));
							}

							var methodName = Converter.Method(group.Name);

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
								if (renderArg.Kind == ArgKind.HiddenMemory) continue;
								if (realArgCount > 0) writer.Write(", ");
								writer.Write($"{renderArg.Type} {renderArg.Name}");
								realArgCount++;
							}
							writer.WriteLine(") {");
							using (writer.Indent()) {
								writer.WriteLine("Code op;");
								GenerateOpCodeSelector(writer, group, renderArgs);

								if (group.IsBranch) {
									writer.Write("AddInstruction(Instruction.CreateBranch(op");
								}
								else {
									writer.Write("AddInstruction(Instruction.Create(op");
								}

								for (var i = 0; i < renderArgs.Count; i++) {
									var renderArg = renderArgs[i];
									if (renderArg.Kind == ArgKind.HiddenMemory) continue;
									writer.Write(", ");
									writer.Write(renderArg.Name);
									if (renderArg.Kind == ArgKind.Branch) {
										writer.Write(".Id");
									}
								}

								writer.WriteLine("));");
							}
							writer.WriteLine("}");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}
		}

		private struct RenderArg {
			public RenderArg(string name, string type, ArgKind kind) {
				Name = name;
				Type = type;
				Kind = kind;
			}

			public string Name;

			public string Type;

			public ArgKind Kind;
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
								if (renderArg.Kind == ArgKind.HiddenMemory) continue;
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
	}
}
