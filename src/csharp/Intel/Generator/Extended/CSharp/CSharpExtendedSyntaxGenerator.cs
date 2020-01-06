using System;
using System.Collections.Generic;
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
									argType = "ExtendedMemoryOperand";
									break;
								case ArgKind.Immediate:
									argName = $"im{i}";
									argType = "long";
									break;
								case ArgKind.Immediate8:
									argName = $"im{i}";
									argType = "byte";
									break;
								default:
									throw new ArgumentOutOfRangeException();
								}

								renderArgs.Add(new RenderArg(argName, argType, argKind));
							}

							writer.Write($"public void {group.Name}(");
							for (var i = 0; i < renderArgs.Count; i++) {
								var renderArg = renderArgs[i];
								if (i > 0) writer.Write(", ");
								writer.Write($"{renderArg.Type} {renderArg.Name}");
							}
							writer.WriteLine(") {");
							using (writer.Indent()) {

								writer.WriteLine("Code op;");

								if (argDispatchIndex >= 0) {
									GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, renderArgs[argDispatchIndex], argDispatchIndex);
								}
								else {
									writer.WriteLine($"op = Code.{group.Items[0].Code.Name(Converter)};");
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

		void GenerateOpCodeSelectorFromRegisterOrMemory(FileWriter writer, OpCodeInfoGroup group, RenderArg arg, int argIndex) {
			bool isFirst = true;

			var regName = arg.Name;

			var isMemory = arg.Kind == ArgKind.RegisterMemory;

			// Order by priority
			group.Items.Sort(OrderByOpCodePriorityOp1);

			foreach (var item in group.Items) {
				if (item is LegacyOpCodeInfo legacy) {
					var opKind = legacy.OpKinds[argIndex];
					if (!isFirst) {
						writer.Write(" else ");
					}
					isFirst = false;
					switch (legacy.OpKinds[0]) {

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
							writer.WriteLine($"if ({regName}.IsGPR16()) {{");
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
							writer.WriteLine($"if ({regName}.IsGPR32()) {{");
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
							writer.WriteLine($"if ({regName}.IsGPR64()) {{");
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
				writer.WriteLine($"throw new ArgumentException($\"Invalid register `{{{regName}}}` for `{{nameof({group.Name})}}` instruction. Expecting 16/32/64\");");
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
	}
}
