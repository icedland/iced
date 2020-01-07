using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
									argName = $"imm";
									argType = group.MaxImmediateSize < 8 ? "int" : "long";
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

								if (regArgIndex >= 0) {
									GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, renderArgs, regArgIndex, methodName);
								}
								else {
									Debug.Assert(group.ItemsWithImmediateByte1 == null);
									
									List<OpCodeInfo> items = null;
									if (group.Items.Count > 0) {
										items = group.Items;
									}
									else if (group.ItemsWithImmediateByte != null) {
										items = group.ItemsWithImmediateByte;
									}
									else {
										Debug.Assert(false, "Unexpected case");
									}
									
									if (items.Count == 1) {
										writer.WriteLine($"op = Code.{items[0].Code.Name(Converter)};");
									}
									else {
										bool isFirst = true;
										bool has64 = false;
										for (int i = items.Count - 1; i >=0; i--) {
											var opcode = items[i];
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
									if (renderArg.Kind == ArgKind.HiddenMemory) continue;
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
			bool hasIf = false;
			FileWriter.Indenter indenter = default;
			if (group.ItemsWithImmediateByte1 != null) {
				var immReg = args[group.ImmediateArgIndex];
				writer.WriteLine($"if ({immReg.Name} == 1) {{");
				hasIf = true;
				using (writer.Indent()) {
					GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, group.ItemsWithImmediateByte1, args, argIndex, methodName);
				}

				writer.WriteLine("}");
			}

			if (group.ItemsWithImmediateByte != null) {
				if (hasIf) {
					writer.Write("else ");
				}

				if (group.Items.Count > 0) {
					var immReg = args[group.ImmediateArgIndex];
					writer.WriteLine($"if ((uint){immReg.Name} <= byte.MaxValue) {{");
					indenter = writer.Indent();
					hasIf = true;
				}
				else {
					hasIf = false;
				}
				
				GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, group.ItemsWithImmediateByte, args, argIndex, methodName);

				if (hasIf) {
					indenter.Dispose();
					writer.WriteLine("}");
				}
			}

			if (group.Items.Count > 0) {
				if (hasIf) {
					writer.Write("else ");
				}

				GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, group.Items, args, argIndex, methodName);
			}
		}

		void GenerateOpCodeSelectorFromRegisterOrMemory(FileWriter writer, OpCodeInfoGroup group, List<OpCodeInfo> opcodes,  List<RenderArg> args, int argIndex, string methodName) {
			bool isFirst = true;

			var arg = args[argIndex];
			var regName = arg.Name;
			var isMemory = arg.Kind == ArgKind.RegisterMemory;

			// Order by priority
			opcodes.Sort(OrderOpCodesPerOpKindPriority);

			for (var i = 0; i < @opcodes.Count; i++) {
				var item = @opcodes[i];
				if (item is LegacyOpCodeInfo legacy) {
					var opKind = legacy.OpKinds[argIndex];
					if (!isFirst) {
						writer.Write(" else ");
					}

					int otherArgIndex = GetOtherArgIndex(legacy, argIndex);
					RenderArg otherArg = default;
					if (otherArgIndex >= 0) {
						otherArg = args[otherArgIndex];
					}
					
					// Check if we need a disambiguation from another parameter (handling only rm for now)
					string refineCheck = string.Empty;
					for (int j = 0; j < opcodes.Count; j++) {
						if (i == j) continue;
						var againstItem = opcodes[j];
						if (againstItem is LegacyOpCodeInfo nearLegacy && GetContextualLegacyOpKind(nearLegacy.OpKinds[argIndex], !isMemory) == GetContextualLegacyOpKind(legacy.OpKinds[argIndex], !isMemory)) {
							var againstRegIndex = GetOtherArgIndex(nearLegacy, argIndex);
							if (otherArgIndex == againstRegIndex && otherArgIndex >= 0 && nearLegacy.OpKinds[otherArgIndex] != legacy.OpKinds[otherArgIndex]) {
								refineCheck = $" && {GetLegacyArgCondition(otherArg, legacy.OpKinds[otherArgIndex])}";
							}
							else {
								refineCheck = $" && conflicting_opcode_{legacy.Code.RawName}_with_{againstItem.Code.RawName}";
								Console.WriteLine($"Conflicting OpCode `{legacy.Code.RawName}` with `{againstItem.Code.RawName}`");
							}
							break;
						}
					}

					isFirst = false;
					writer.WriteLine($"if ({GetLegacyArgCondition(arg, opKind)}{refineCheck}) {{");
					using (writer.Indent()) {
						if (IsSegmentRegister(opKind)) {
							if (methodName == "push" || methodName == "pop") {
								writer.WriteLine($"op = Bitness >= 32 ? Code.{Converter.Field(legacy.Code.RawName.Replace('w', 'd').Replace("Popd_CS", "Popw_CS"))} : Code.{Converter.Field(legacy.Code.RawName.Replace('d', 'w'))};");
							}
							else {
								writer.WriteLine($"op = Unexpected_Segment_Register_used_Code.{legacy.Code.RawName};");
								Console.WriteLine($"Unsupported Segment register used {legacy.Code.RawName}");
							}
						}
						else {
							writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
						}
					}
					writer.Write("}");
				}
			}

			writer.WriteLine(" else {");
			using (writer.Indent()) {
				writer.WriteLine($"throw new ArgumentException($\"Invalid register for `{{nameof({methodName})}}` instruction. Expecting 16/32/64\");");
			}
			writer.WriteLine("}");
		}

		static string GetLegacyArgCondition(RenderArg arg, LegacyOpKind opKind) {
			var regName = arg.Name;
			var isMemory = arg.Kind == ArgKind.RegisterMemory;
			switch (opKind) {
			case LegacyOpKind.CL:
			case LegacyOpKind.AL:
			case LegacyOpKind.AX:
			case LegacyOpKind.EAX:
			case LegacyOpKind.RAX:
			case LegacyOpKind.ES:
			case LegacyOpKind.CS:
			case LegacyOpKind.SS:
			case LegacyOpKind.DS:
			case LegacyOpKind.FS:
			case LegacyOpKind.GS:
			case LegacyOpKind.DX:
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
			case LegacyOpKind.Imm1:
				return $"{regName} == 1";			
			case LegacyOpKind.Ib:
			case LegacyOpKind.Ib16:
			case LegacyOpKind.Ib32:
			case LegacyOpKind.Ib64:
				return $"(uint){regName} <= byte.MaxValue";			
			default:
				return "true";
			}
		}

		static int GetOtherArgIndex(LegacyOpCodeInfo legacy, int regIndex) {
			int index = -1;
			for (int j = 0; j < legacy.OpKinds.Length; j++) {
				if (j == regIndex) continue;
				index = j;
				var otherKind = legacy.OpKinds[j];
				if (IsKindRegLike(otherKind)) {
					break;
				}
			}
			return index;
		}

		static int OrderOpCodesPerOpKindPriority(OpCodeInfo x, OpCodeInfo y) {
			if (x is LegacyOpCodeInfo x1 && y is LegacyOpCodeInfo y1) {
				Debug.Assert(x1.OpKinds.Length == y1.OpKinds.Length);
				for (int i = 0; i < x1.OpKinds.Length; i++) {
					var result = GetPriorityFromKind(x1.OpKinds[i]).CompareTo(GetPriorityFromKind(y1.OpKinds[i]));
					if (result != 0) return result;
				}
			}
			return 0;
		}

		static int GetPriorityFromKind(LegacyOpKind kind) {
			switch (kind) {

			case LegacyOpKind.RAX:
			case LegacyOpKind.r64_ro:
			case LegacyOpKind.Gq:
			case LegacyOpKind.Eq:
			case LegacyOpKind.Id64:							
			case LegacyOpKind.Iq:
				return 0;

			case LegacyOpKind.EAX:
			case LegacyOpKind.r32_rd:
			case LegacyOpKind.Gd:
			case LegacyOpKind.Ed:
			case LegacyOpKind.Id:
				return 1;

			case LegacyOpKind.AX:
			case LegacyOpKind.r16_rw:
			case LegacyOpKind.Gw:
			case LegacyOpKind.Ew:
			case LegacyOpKind.Iw: 
				return 2;

			case LegacyOpKind.Imm1:
				return 3;

			case LegacyOpKind.AL:
			case LegacyOpKind.r8_rb:
			case LegacyOpKind.Gb:
			case LegacyOpKind.Eb:
			case LegacyOpKind.Ib:
			case LegacyOpKind.Ib16:
			case LegacyOpKind.Ib32:
			case LegacyOpKind.Ib64:
				return 4;
			
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
			case LegacyOpKind.CL:
			case LegacyOpKind.AX:
			case LegacyOpKind.ES:
			case LegacyOpKind.CS:
			case LegacyOpKind.SS:
			case LegacyOpKind.DS:
			case LegacyOpKind.DX:
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
	}
}
