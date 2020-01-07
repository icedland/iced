using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
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

							int regArgIndex = -1;
							int rmArgIndex = -1;

							var signature = group.Signature;
							for (int i = 0; i < signature.ArgCount; i++) {
								string argName = i == 0 ? "dst" : i == 1 ? "src" : $"arg{i}";
								string argType = $"arg{i}";
								var argKind = signature.GetArgKind(i);
								switch (argKind) {
								case ArgKind.Register:
									argType = "Register";
									if (regArgIndex < 0) {
										regArgIndex = i;
									}
									break;
								case ArgKind.RegisterMemory:
									if (regArgIndex < 0) {
										regArgIndex = i;
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

								if (regArgIndex >= 0) {
									GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, renderArgs, regArgIndex, methodName);
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
													              legacy.AddressSize == AddressSize.Size16 ||
														          legacy.Flags == OpCodeFlags.Mode16 ? 16 : legacy.OperandSize == OperandSize.Size32  || 
														                                                       legacy.AddressSize == AddressSize.Size32  || 
														                                                       legacy.Flags == (OpCodeFlags.Mode16 | OpCodeFlags.Mode32) ? 32 : 64;
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

		void GenerateOpCodeSelectorFromRegisterOrMemory(FileWriter writer, OpCodeInfoGroup group, List<RenderArg> args, int argIndex, string methodName) {
			bool isFirst = true;

			var arg = args[argIndex];
			
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

					int otherRegIndex = GetOtherRegLikeIndex(legacy, argIndex);
					RenderArg otherArg = default;
					if (otherRegIndex >= 0) {
						otherArg = args[otherRegIndex];
					}
					
					// Check if we need a disambiguation from another parameter (handling only rm for now)
					string refineCheck = string.Empty;
					for (int j = 0; j < group.Items.Count; j++) {
						if (i == j) continue;
						var againstItem = group.Items[j];
						if (againstItem is LegacyOpCodeInfo nearLegacy && nearLegacy.OpKinds[argIndex] == legacy.OpKinds[argIndex]) {
							var againstRegIndex = GetOtherRegLikeIndex(nearLegacy, argIndex);
							if (otherRegIndex == againstRegIndex && otherRegIndex >= 0 && nearLegacy.OpKinds[otherRegIndex] != legacy.OpKinds[otherRegIndex]) {
								refineCheck = $" && {GetLegacyArgCondition(otherArg, legacy.OpKinds[otherRegIndex])}";
							}
							else {
								refineCheck = $" && conflicting_opcode_{legacy.Code.RawName}_with_{againstItem.Code.RawName}";
								Console.WriteLine($"Conflicting OpCode `{legacy.Code.RawName}` with `{againstItem.Code.RawName}`");
							}
						}
					}

					isFirst = false;
					writer.WriteLine($"if ({GetLegacyArgCondition(arg, opKind)}{refineCheck}) {{");
					using (writer.Indent()) {
						writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
					}
					writer.Write("}");
				}
			}

			writer.WriteLine(" else {");
			using (writer.Indent()) {
				writer.WriteLine($"throw new ArgumentException($\"Invalid register `{{{regName}}}` for `{{nameof({methodName})}}` instruction. Expecting 16/32/64\");");
			}
			writer.WriteLine("}");
		}

		static string GetLegacyArgCondition(RenderArg arg, LegacyOpKind opKind) {
			var regName = arg.Name;
			var isMemory = arg.Kind == ArgKind.RegisterMemory;
			switch (opKind) {
			case LegacyOpKind.AL:
			case LegacyOpKind.AX:
			case LegacyOpKind.EAX:
			case LegacyOpKind.RAX:
				return $"{regName} == Register.{opKind}";
			case LegacyOpKind.r8_rb:
			case LegacyOpKind.Gb:
			case LegacyOpKind.Eb:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.BytePtr" : $"{regName}.IsGPR8()";
			case LegacyOpKind.r16_rw:
			case LegacyOpKind.Gw:
			case LegacyOpKind.Ew:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.WordPtr" : $"{regName}.IsGPR16()";
			case LegacyOpKind.r32_rd:
			case LegacyOpKind.Gd:
			case LegacyOpKind.Ed:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.DwordPtr" : $"{regName}.IsGPR32()";
			case LegacyOpKind.r64_ro:
			case LegacyOpKind.Gq:
			case LegacyOpKind.Eq:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.QwordPtr" : $"{regName}.IsGPR64()";
			}
			throw new InvalidOperationException($"Invalid {opKind} for argument {arg.Kind}");			
		}

		static int GetOtherRegLikeIndex(LegacyOpCodeInfo legacy, int regIndex) {
			for (int j = 0; j < legacy.OpKinds.Length; j++) {
				if (j == regIndex) continue;
				var otherKind = legacy.OpKinds[j];
				if (IsKindRegLike(otherKind)) {
					return j;
				}
			}
			return -1;
		}

		static int OrderByOpCodePriorityOp1(OpCodeInfo x, OpCodeInfo y) {
			if (x is LegacyOpCodeInfo x1 && y is LegacyOpCodeInfo y1) {

				return GetPriorityFromKind(x1.OpKinds[0]).CompareTo(GetPriorityFromKind(y1.OpKinds[0]));
			}
			return 0;
		}

		static int GetPriorityFromKind(LegacyOpKind kind) {
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
		
		static bool IsKindRegLike(LegacyOpKind kind) {
			switch (kind) {

			case LegacyOpKind.RAX:
			case LegacyOpKind.r64_ro:
			case LegacyOpKind.Gq:
			case LegacyOpKind.Eq:
			case LegacyOpKind.EAX:
			case LegacyOpKind.r32_rd:
			case LegacyOpKind.Gd:
			case LegacyOpKind.Ed:
			case LegacyOpKind.AX:
			case LegacyOpKind.r16_rw:
			case LegacyOpKind.Gw:
			case LegacyOpKind.Ew:
			case LegacyOpKind.AL:
			case LegacyOpKind.r8_rb:
			case LegacyOpKind.Gb:
			case LegacyOpKind.Eb:
				return true;
			default:
				return false;
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
