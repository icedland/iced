using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Generator.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Extended.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Encoder)]
	sealed class CSharpExtendedSyntaxGenerator : ExtendedSyntaxGenerator {
		readonly GeneratorOptions _generatorOptions;

		public CSharpExtendedSyntaxGenerator(GeneratorOptions generatorOptions) {
			Converter = CSharpIdentifierConverter.Create();
			_generatorOptions = generatorOptions;
		}

		protected override void GenerateRegisters(EnumType registers) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(_generatorOptions, CSharpConstants.IcedNamespace), "Extended", "ExtendedRegisters.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.EncoderDefine}");

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("public static partial class ExtendedRegisters {");
					using (writer.Indent()) {
						foreach (var register in registers.Values) {
							if (register.Value == 0) {
								continue;
							}
							var name = register.Name(Converter);
							writer.WriteLine($"public static readonly ExtendedRegister {name.ToLowerInvariant()} = Register.{name};");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}
		}

		protected override void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {

			var filename = Path.Combine(CSharpConstants.GetDirectory(_generatorOptions, CSharpConstants.IcedNamespace), "Extended", "ExtendedEncoder.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLine($"#if {CSharpConstants.EncoderDefine}");
				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("using System;");
					writer.WriteLine("public sealed partial class ExtendedEncoder {");
					using (writer.Indent()) {

						var renderArgs = new List<RenderArg>();

						foreach (var group in groups) {
							renderArgs.Clear();

							int argDispatchIndex = -1;
							int rmArgIndex = -1;

							var signature = group.Signature;
							for (int i = 0; i < signature.ArgCount; i++) {
								string argName = i == 0 ? "dst" : i == 1 ? "src" : $"arg{i}";
								string argType = $"arg{i}";
								var argKind = signature.GetArgKind(i);
								switch (argKind) {
								case ArgKind.Register:
									argType = "Register";
									argDispatchIndex = i;
									break;
								case ArgKind.RegisterMemory:
									if (argDispatchIndex < 0) {
										argDispatchIndex = i;
									}

									if (rmArgIndex < 0) {
										rmArgIndex = i;
									}
									argType = "ExtendedMemoryOperand";
									break;
								case ArgKind.Immediate:
									argName = $"im{i}";
									argType = group.MaxImmediateSize < 8 ? group.MaxImmediateSize == 1 ? "byte" : "int" : "long";
									break;
								default:
									throw new ArgumentOutOfRangeException();
								}

								renderArgs.Add(new RenderArg(argName, argType, argKind));
							}

							var methodName = Converter.Method(group.Name);
							writer.Write($"public void {methodName}(");
							for (var i = 0; i < renderArgs.Count; i++) {
								var renderArg = renderArgs[i];
								if (i > 0) writer.Write(", ");
								writer.Write($"{renderArg.Type} {renderArg.Name}");
							}
							writer.WriteLine(") {");
							using (writer.Indent()) {

								writer.WriteLine("Code op;");

								if (argDispatchIndex >= 0) {
									GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, renderArgs[argDispatchIndex], argDispatchIndex, methodName, rmArgIndex >= 0 ? renderArgs[rmArgIndex] : default, rmArgIndex);
								}
								else {
									if (group.Items.Count == 1) {
										writer.WriteLine($"op = Code.{group.Items[0].Code.Name(Converter)};");
									}
									else {
										bool isFirst = true;
										bool has64 = false;
										for (int i = group.Items.Count - 1; i >=0; i--) {
											var opcode = group.Items[i];
											if (opcode is LegacyOpCodeInfo legacy) {
												if (!isFirst) writer.Write(" else ");
												isFirst = false;
												if (i == 0) {
													writer.WriteLine("{");
												}
												else {
													var bitness = legacy.OperandSize == OperandSize.Size16 ||
													              legacy.AddressSize == AddressSize.Size16 ? 16 : legacy.OperandSize == OperandSize.Size32  || 
													                                                              legacy.AddressSize == AddressSize.Size32 ? 32 : 64;
													if (bitness == 64) has64 = true;
													writer.WriteLine($"if (Bitness {(has64 ? "==" : ">=")} {bitness}) {{");
												}
												using (writer.Indent()) {
													writer.WriteLine($"op = Code.{opcode.Code.Name(Converter)};");
												}
												writer.Write("}");
											}
										}
										writer.WriteLine();
									}
								}

								writer.Write("AddInstruction(Instruction.Create(op");

								for (var i = 0; i < renderArgs.Count; i++) {
									var renderArg = renderArgs[i];
									writer.Write(", ");
									writer.Write(renderArg.Name);
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

		void GenerateOpCodeSelectorFromRegisterOrMemory(FileWriter writer, OpCodeInfoGroup group, RenderArg arg, int argIndex, string methodName, RenderArg rmArg, int rmIndex) {
			bool isFirst = true;

			var regName = arg.Name;

			var isMemory = arg.Kind == ArgKind.RegisterMemory;

			// Order by priority
			group.Items.Sort(OrderByOpCodePriorityOp1);

			for (var i = 0; i < @group.Items.Count; i++) {
				var item = @group.Items[i];
				if (item is LegacyOpCodeInfo legacy) {
					var opKind = legacy.OpKinds[argIndex];
					if (!isFirst) {
						writer.Write(" else ");
					}

					// Check if we need a disambiguation from another parameter (handling only rm for now)
					bool requiresRmToDisambiguate = false;
					string legacyRmSize = null;
					if (legacy.OperandSize != OperandSize.None && rmIndex >= 0) {
						for (int j = 0; j < group.Items.Count; j++) {
							if (i == j) continue;
							var nearItem = group.Items[j];
							if (nearItem is LegacyOpCodeInfo nearLegacy && nearLegacy.OpKinds[argIndex] == legacy.OpKinds[argIndex]) {
								requiresRmToDisambiguate = nearLegacy.OpKinds[rmIndex] != legacy.OpKinds[rmIndex] ;
								if (requiresRmToDisambiguate) {
									switch (GetSizeFromKind(legacy.OpKinds[rmIndex])) {
									case 1:
										legacyRmSize = "BytePtr";
										break;
									case 2:
										legacyRmSize = "WordPtr";
										break;
									case 4:
										legacyRmSize = "DwordPtr";
										break;
									default:
										legacyRmSize = "QwordPtr";
										break;
									}
									break;
								}
							}
						}
					}

					isFirst = false;
					switch (legacy.OpKinds[argIndex]) {
					case LegacyOpKind.AL:
					case LegacyOpKind.AX:
					case LegacyOpKind.EAX:
					case LegacyOpKind.RAX:
						writer.WriteLine($"if ({regName} == Register.{opKind}) {{");
						using (writer.Indent()) {
							writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
						}

						writer.WriteLine("}");
						break;
					case LegacyOpKind.r8_rb:
					case LegacyOpKind.Gb:
					case LegacyOpKind.Eb:
						if (isMemory) {
							writer.WriteLine($"if ({regName}.Size == MemoryOperandSize.BytePtr) {{");
						}
						else {
							writer.WriteLine($"if ({regName}.IsGPR8()) {{");
						}

						using (writer.Indent()) {
							writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
						}

						writer.Write("}");
						break;
					case LegacyOpKind.r16_rw:
					case LegacyOpKind.Gw:
					case LegacyOpKind.Ew:
						if (isMemory) {
							writer.WriteLine($"if ({regName}.Size == MemoryOperandSize.WordPtr) {{");
						}
						else {
							if (requiresRmToDisambiguate) {
								writer.WriteLine($"if ({regName}.IsGPR16() && {rmArg.Name}.Size == MemoryOperandSize.{legacyRmSize}) {{");
							}
							else {
								writer.WriteLine($"if ({regName}.IsGPR16()) {{");
							}
						}

						using (writer.Indent()) {
							writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
						}

						writer.Write("}");
						break;
					case LegacyOpKind.r32_rd:
					case LegacyOpKind.Gd:
					case LegacyOpKind.Ed:
						if (isMemory) {
							writer.WriteLine($"if ({regName}.Size == MemoryOperandSize.DwordPtr) {{");
						}
						else {
							if (requiresRmToDisambiguate) {
								writer.WriteLine($"if ({regName}.IsGPR32() && {rmArg.Name}.Size == MemoryOperandSize.{legacyRmSize}) {{");
							}
							else {
								writer.WriteLine($"if ({regName}.IsGPR32()) {{");
							}
						}

						using (writer.Indent()) {
							writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
						}

						writer.Write("}");
						break;
					case LegacyOpKind.r64_ro:
					case LegacyOpKind.Gq:
					case LegacyOpKind.Eq:
						if (isMemory) {
							writer.WriteLine($"if ({regName}.Size == MemoryOperandSize.QwordPtr) {{");
						}
						else {
							if (requiresRmToDisambiguate) {
								writer.WriteLine($"if ({regName}.IsGPR64() && {rmArg.Name}.Size == MemoryOperandSize.{legacyRmSize}) {{");
							}
							else {
								writer.WriteLine($"if ({regName}.IsGPR64()) {{");
							}
						}

						using (writer.Indent()) {
							writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
						}

						writer.Write("}");
						break;
					}
				}
			}

			writer.WriteLine(" else {");
			using (writer.Indent()) {
				writer.WriteLine($"throw new ArgumentException($\"Invalid register `{{{regName}}}` for `{{nameof({methodName})}}` instruction. Expecting 16/32/64\");");
			}
			writer.WriteLine("}");
		}

		static int OrderByOpCodePriorityOp1(OpCodeInfo x, OpCodeInfo y) {
			if (x is LegacyOpCodeInfo x1 && y is LegacyOpCodeInfo y1) {

				return GetPriorityFromKind(x1.OpKinds[0]).CompareTo(GetPriorityFromKind(y1.OpKinds[0]));
			}
			return 0;
		}

		private static int GetPriorityFromKind(LegacyOpKind kind) {
			switch (kind) {

			case LegacyOpKind.RAX:
			case LegacyOpKind.r64_ro:
			case LegacyOpKind.Gq:
			case LegacyOpKind.Eq:
				return 0;

			case LegacyOpKind.EAX:
			case LegacyOpKind.r32_rd:
			case LegacyOpKind.Gd:
			case LegacyOpKind.Ed:
				return 1;

			case LegacyOpKind.AX:
			case LegacyOpKind.r16_rw:
			case LegacyOpKind.Gw:
			case LegacyOpKind.Ew:
				return 2;

			case LegacyOpKind.AL:
			case LegacyOpKind.r8_rb:
			case LegacyOpKind.Gb:
			case LegacyOpKind.Eb:
				return 3;
			default:
				return int.MaxValue;
			}
		}
		
		private static int GetSizeFromKind(LegacyOpKind kind) {
			switch (kind) {

			case LegacyOpKind.RAX:
			case LegacyOpKind.r64_ro:
			case LegacyOpKind.Gq:
			case LegacyOpKind.Eq:
				return 8;

			case LegacyOpKind.EAX:
			case LegacyOpKind.r32_rd:
			case LegacyOpKind.Gd:
			case LegacyOpKind.Ed:
				return 4;

			case LegacyOpKind.AX:
			case LegacyOpKind.r16_rw:
			case LegacyOpKind.Gw:
			case LegacyOpKind.Ew:
				return 2;

			case LegacyOpKind.AL:
			case LegacyOpKind.r8_rb:
			case LegacyOpKind.Gb:
			case LegacyOpKind.Eb:
				return 1;
			default:
				return 0;
			}
		}
	}
}
