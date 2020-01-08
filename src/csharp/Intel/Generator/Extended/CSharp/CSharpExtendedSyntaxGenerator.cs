using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Generator.Documentation.CSharp;
using Generator.Encoder;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Extended.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Encoder)]
	sealed class CSharpExtendedSyntaxGenerator : ExtendedSyntaxGenerator {
		readonly GeneratorOptions _generatorOptions;
		readonly CSharpDocCommentWriter docWriter;

		public CSharpExtendedSyntaxGenerator(GeneratorOptions generatorOptions) {
			Converter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(Converter);
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
								case ArgKind.Memory:
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

							// Write documentation
							var methodDoc = new StringBuilder();
							methodDoc.Append($"{group.Name} instruction.#(p:)#");
							foreach (var code in group.Items) {
								if (!string.IsNullOrEmpty(code.Code.Documentation)) {
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
								GenerateOpCodeSelectorFromRegisterOrMemory(writer, group, renderArgs, regArgIndex, methodName);

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


			var listGroupedItems = group.GroupedItems.ToList();

			for (var i = 0; i < listGroupedItems.Count; i++) {
				var groupedItems = listGroupedItems[i];
				if (hasIf) {
					writer.Write("else ");
				}				
				
				switch (groupedItems.Key) {
				case ImmediateKind.Bit1: {
					var immReg = args[@group.ImmediateArgIndex];
					writer.WriteLine($"if ({immReg.Name} == 1) {{");
					hasIf = true;
					using (writer.Indent()) {
						GenerateOpCodeSelectorFromRegisterOrMemory(writer, @group, groupedItems.Value, args, argIndex, methodName);
					}

					writer.WriteLine("}");
				}
					break;
				case ImmediateKind.Bits2: {
					if (i + 1 < listGroupedItems.Count) {
						var immReg = args[@group.ImmediateArgIndex];
						writer.WriteLine($"if ({immReg.Name} >= 0 && {immReg.Name} < 4) {{");
						indenter = writer.Indent();
						hasIf = true;
					}
					else {
						hasIf = false;
					}

					GenerateOpCodeSelectorFromRegisterOrMemory(writer, @group, groupedItems.Value, args, argIndex, methodName);

					if (hasIf) {
						indenter.Dispose();
						writer.WriteLine("}");
					}					
				}
					break;
				case ImmediateKind.Byte: {
					if (i + 1 < listGroupedItems.Count) {
						var immReg = args[@group.ImmediateArgIndex];
						writer.WriteLine($"if ({immReg.Name} >= sbyte.MinValue && {immReg.Name} <= byte.MaxValue) {{");
						indenter = writer.Indent();
						hasIf = true;
					}
					else {
						hasIf = false;
					}

					GenerateOpCodeSelectorFromRegisterOrMemory(writer, @group, groupedItems.Value, args, argIndex, methodName);

					if (hasIf) {
						indenter.Dispose();
						writer.WriteLine("}");
					}
				}
					break;
				case ImmediateKind.None:
				case ImmediateKind.Standard: {
					GenerateOpCodeSelectorFromRegisterOrMemory(writer, @group, groupedItems.Value, args, argIndex, methodName);
				}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		void GenerateOpCodeSelectorFromRegisterOrMemory(FileWriter writer, OpCodeInfoGroup group, List<OpCodeInfo> opcodes,  List<RenderArg> args, int argIndex, string methodName) {
			bool isFirst = true;

			var arg = argIndex >= 0 ? args[argIndex] : default;
			var isMemory = arg.Kind == ArgKind.RegisterMemory;

			// Order by priority
			opcodes.Sort(OrderOpCodesPerOpKindPriority);

			bool has64 = false;
			bool needToThrowIfNotFound = false;
			for (var i = 0; i < @opcodes.Count; i++) {
				var opCodeInfo = @opcodes[i];
				if (!isFirst) {
					writer.Write(" else ");
				}
				isFirst = false;

				if (argIndex >= 0) {
					var opKind = opCodeInfo.OpKind(argIndex);
					int otherArgIndex = GetOtherArgIndex(opCodeInfo, argIndex);
					RenderArg otherArg = default;
					if (otherArgIndex >= 0) {
						otherArg = args[otherArgIndex];
					}
				
					// Check if we need a disambiguation from another parameter (handling only rm for now)
					string refineCheck = string.Empty;
					for (int j = 0; j < opcodes.Count; j++) {
						if (i == j) continue;
						var againstOpCodeInfo = opcodes[j];
						if (GetContextualCommonOpKind(againstOpCodeInfo.OpKind(argIndex), !isMemory) == GetContextualCommonOpKind(opCodeInfo.OpKind(argIndex), !isMemory)) {
							var againstRegIndex = GetOtherArgIndex(againstOpCodeInfo, argIndex);
							if (otherArgIndex == againstRegIndex && otherArgIndex >= 0) {
								if (againstOpCodeInfo.OpKind(otherArgIndex) != opCodeInfo.OpKind(otherArgIndex)) {
									refineCheck = $" && {GetArgConditionForOpCodeKind(otherArg, opCodeInfo.OpKind(otherArgIndex))}";
								}
								else {
									refineCheck = $" && {GetArgConditionForOpCode(opCodeInfo)}";
								}
							}
							else {
								refineCheck = $" && conflicting_opcode_{opCodeInfo.Code.RawName}_with_{againstOpCodeInfo.Code.RawName}";
								Console.WriteLine($"Conflicting OpCode `{opCodeInfo.Code.RawName}` with `{againstOpCodeInfo.Code.RawName}`");
							}
							break;
						}
					}
					
					writer.WriteLine($"if ({GetArgConditionForOpCodeKind(arg, opKind)}{refineCheck}) {{");
					using (writer.Indent()) {
						if (IsSegmentRegister(opKind)) {
							if (methodName == "push" || methodName == "pop") {
								writer.WriteLine($"op = Bitness >= 32 ? Code.{Converter.Field(opCodeInfo.Code.RawName.Replace('w', 'd').Replace("Popd_CS", "Popw_CS"))} : Code.{Converter.Field(opCodeInfo.Code.RawName.Replace('d', 'w'))};");
							}
							else {
								writer.WriteLine($"op = Unexpected_Segment_Register_used_Code.{opCodeInfo.Code.RawName};");
								Console.WriteLine($"Unsupported Segment register used {opCodeInfo.Code.RawName}");
							}
						}
						else {
							writer.WriteLine($"op = Code.{opCodeInfo.Code.Name(Converter)};");
						}
					}
					writer.Write("}");

					needToThrowIfNotFound = true;
				}
				else {
					if (opcodes.Count > 1 && opCodeInfo is LegacyOpCodeInfo legacy) {
						var bitness = legacy.OperandSize == OperandSize.Size16 ||
						              legacy.AddressSize == AddressSize.Size16 ||
						              legacy.Flags == OpCodeFlags.Mode16 ? 16 :
							legacy.OperandSize == OperandSize.Size32 ||
							legacy.AddressSize == AddressSize.Size32 ||
							legacy.Flags == (OpCodeFlags.Mode16 | OpCodeFlags.Mode32) ? 32 : 64;
						if (bitness == 64) has64 = true;
						writer.WriteLine($"if (Bitness {(has64 ? "==" : ">=")} {bitness}) {{");

						using (writer.Indent()) {
							writer.WriteLine($"op = Code.{legacy.Code.Name(Converter)};");
						}

						writer.Write("}");
						needToThrowIfNotFound = true;
					}
					else {
						writer.WriteLine($"op = Code.{opCodeInfo.Code.Name(Converter)};");
					}
				}
			}

			if (needToThrowIfNotFound) {
				writer.WriteLine(" else {");
				using (writer.Indent()) {
					writer.Write($"throw NoOpCodeFoundFor(nameof({methodName})");
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

		static string GetArgConditionForOpCode(OpCodeInfo opCode) {
			if (opCode is VexOpCodeInfo) {
				return $"PreferVex";
			}
			else if (opCode is EvexOpCodeInfo) {
				return $"!PreferVex";
			}
			return $"unsupported_condition_for_op_code_{opCode.Code.RawName}";
		}

		static string GetArgConditionForOpCodeKind(RenderArg arg, CommonOpKind opKind) {
			var regName = arg.Name;
			var isMemory = arg.Kind == ArgKind.RegisterMemory || arg.Kind == ArgKind.Memory;
			switch (opKind) {
			case CommonOpKind.CL:
			case CommonOpKind.AL:
			case CommonOpKind.AX:
			case CommonOpKind.EAX:
			case CommonOpKind.RAX:
			case CommonOpKind.ES:
			case CommonOpKind.CS:
			case CommonOpKind.SS:
			case CommonOpKind.DS:
			case CommonOpKind.FS:
			case CommonOpKind.GS:
			case CommonOpKind.DX:
				return $"{regName} == Register.{opKind}";
			case CommonOpKind.r8_rb:
			case CommonOpKind.Gb:
			case CommonOpKind.Eb:
			case CommonOpKind.RdMb:
			case CommonOpKind.RqMb:
			case CommonOpKind.Mb:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.BytePtr" : $"{regName}.IsGPR8()";
			case CommonOpKind.r16_rw:
			case CommonOpKind.Gw:
			case CommonOpKind.Ew:
			case CommonOpKind.RdMw:
			case CommonOpKind.RqMw:
			case CommonOpKind.Mw:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.WordPtr" : $"{regName}.IsGPR16()";
			case CommonOpKind.r32_rd:
			case CommonOpKind.Hd:
			case CommonOpKind.Gd:
			case CommonOpKind.Ed:
			case CommonOpKind.Rd:
			case CommonOpKind.Md:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.DwordPtr" : $"{regName}.IsGPR32()";
			case CommonOpKind.r64_ro:
			case CommonOpKind.Hq:
			case CommonOpKind.Gq:
			case CommonOpKind.Eq:
			case CommonOpKind.Rq:
			case CommonOpKind.Mq:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.QwordPtr" : $"{regName}.IsGPR64()";
			case CommonOpKind.Imm1:
				return $"{regName} == 1";			
			case CommonOpKind.Ib:
			case CommonOpKind.Ib16:
			case CommonOpKind.Ib32:
			case CommonOpKind.Ib64:
				return $"(uint){regName} <= byte.MaxValue";			
			case CommonOpKind.VK:
			case CommonOpKind.HK:
			case CommonOpKind.RK:
				return $"{regName}.IsK()";
			case CommonOpKind.Cd:
				return $"{regName}.IsCr()";
			case CommonOpKind.Dd:
				return $"{regName}.IsDr()";
			case CommonOpKind.Td:
				return $"{regName}.IsTr()";
			case CommonOpKind.Is4X:
			case CommonOpKind.Is5X:
			case CommonOpKind.VX:
			case CommonOpKind.HX:
			case CommonOpKind.RX:
				return $"{regName}.IsXMM()";
			case CommonOpKind.Is4Y:
			case CommonOpKind.Is5Y:
			case CommonOpKind.VY:
			case CommonOpKind.HY:
			case CommonOpKind.RY:
				return $"{regName}.IsYMM()";
			case CommonOpKind.VZ:
			case CommonOpKind.HZ:
			case CommonOpKind.RZ:
				return $"{regName}.IsZMM()";
			case CommonOpKind.N:
			case CommonOpKind.P:
				return $"{regName}.IsMM()";
			case CommonOpKind.Q:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.QwordPtr" : $"{regName}.IsMM()";
			case CommonOpKind.WK:
				return isMemory ? $"{regName}.Size == unknown" : $"{regName}.IsK()";
			case CommonOpKind.WX:
			case CommonOpKind.M:
			case CommonOpKind.VM32X:
			case CommonOpKind.VM64X:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.DQwordPtr" : $"{regName}.IsXMM()";
			case CommonOpKind.WY:
			case CommonOpKind.VM32Y:
			case CommonOpKind.VM64Y:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.YwordPtr" : $"{regName}.IsYMM()";
			case CommonOpKind.WZ:
			case CommonOpKind.VM32Z:
			case CommonOpKind.VM64Z:
				return isMemory ? $"{regName}.Size == MemoryOperandSize.ZwordPtr" : $"{regName}.IsZMM()";
			case CommonOpKind.ST:
				return $"{regName} == Register.ST0";
			case CommonOpKind.STi:
				return $"{regName}.IsST()";
			default:
				return "true";
			}
		}

		static int GetOtherArgIndex(OpCodeInfo legacy, int regIndex) {
			int index = -1;
			for (int j = 0; j < legacy.OpKindsLength; j++) {
				if (j == regIndex) continue;
				index = j;
				var otherKind = legacy.OpKind(j);
				if (IsKindRegLike(otherKind)) {
					break;
				}
			}
			return index;
		}

		static int OrderOpCodesPerOpKindPriority(OpCodeInfo x, OpCodeInfo y) {
			Debug.Assert(x.OpKindsLength == y.OpKindsLength);
			for (int i = 0; i < x.OpKindsLength; i++) {
				var result = GetPriorityFromKind(x.OpKind(i)).CompareTo(GetPriorityFromKind(y.OpKind(i)));
				if (result != 0) return result;
			}

			// Case for ordering by decreasing bitness
			if (x is LegacyOpCodeInfo x1 && y is LegacyOpCodeInfo y1) {
				var result = (x1.AddressSize == AddressSize.None ? AddressSize.Size64 : x1.AddressSize).CompareTo((y1.AddressSize == AddressSize.None ? AddressSize.Size64 : y1.AddressSize));
				if (result != 0) return -result;
				
				result = (x1.OperandSize == OperandSize.None ? OperandSize.Size64 : x1.OperandSize).CompareTo((y1.OperandSize == OperandSize.None ? OperandSize.Size64 : y1.OperandSize));
				if (result != 0) return -result;
			}
			
			return 0;
		}

		static int GetPriorityFromKind(CommonOpKind kind) {
			switch (kind) {

			case CommonOpKind.VZ:
			case CommonOpKind.HZ:
			case CommonOpKind.RZ:
			case CommonOpKind.WZ:
			case CommonOpKind.VM32Z:
			case CommonOpKind.VM64Z:
				return -3;

			case CommonOpKind.VY:
			case CommonOpKind.Is4Y:
			case CommonOpKind.Is5Y:
			case CommonOpKind.HY:
			case CommonOpKind.RY:
			case CommonOpKind.WY:
			case CommonOpKind.VM32Y:
			case CommonOpKind.VM64Y:
				return -2;
			
			case CommonOpKind.M:
			case CommonOpKind.Is4X:
			case CommonOpKind.Is5X:
			case CommonOpKind.VX:
			case CommonOpKind.HX:
			case CommonOpKind.RX:
			case CommonOpKind.WX:
			case CommonOpKind.VM32X:
			case CommonOpKind.VM64X:
				return -1;
		
			case CommonOpKind.ST:
			case CommonOpKind.RAX:
			case CommonOpKind.r64_ro:
			case CommonOpKind.Hq:
			case CommonOpKind.Mq:
			case CommonOpKind.Gq:
			case CommonOpKind.Eq:
			case CommonOpKind.Id64:							
			case CommonOpKind.Iq:
			case CommonOpKind.Rq:
				return 0;

			case CommonOpKind.STi:
			case CommonOpKind.EAX:
			case CommonOpKind.r32_rd:
			case CommonOpKind.Hd:
			case CommonOpKind.Md:
			case CommonOpKind.Gd:
			case CommonOpKind.Ed:
			case CommonOpKind.Id:
			case CommonOpKind.Rd:
				return 1;

			case CommonOpKind.AX:
			case CommonOpKind.r16_rw:
			case CommonOpKind.RdMw:
			case CommonOpKind.RqMw:
			case CommonOpKind.Mw:
			case CommonOpKind.Gw:
			case CommonOpKind.Ew:
			case CommonOpKind.Iw: 
				return 2;

			case CommonOpKind.Imm1:
				return 3;

			case CommonOpKind.AL:
			case CommonOpKind.r8_rb:
			case CommonOpKind.Gb:
			case CommonOpKind.Eb:
			case CommonOpKind.Mb:
			case CommonOpKind.Ib:
			case CommonOpKind.RdMb:
			case CommonOpKind.RqMb:
			case CommonOpKind.Ib16:
			case CommonOpKind.Ib32:
			case CommonOpKind.Ib64:
				return 4;
			
			default:
				return int.MaxValue;
			}
		}
		
		static bool IsKindRegLike(CommonOpKind kind) {
			switch (kind) {

			case CommonOpKind.RAX:
			case CommonOpKind.r64_ro:
			case CommonOpKind.Gq:
			case CommonOpKind.Eq:
			case CommonOpKind.EAX:
			case CommonOpKind.r32_rd:
			case CommonOpKind.Hd:
			case CommonOpKind.Hq:
			case CommonOpKind.Gd:
			case CommonOpKind.Ed:
			case CommonOpKind.CL:
			case CommonOpKind.AX:
			case CommonOpKind.ES:
			case CommonOpKind.CS:
			case CommonOpKind.SS:
			case CommonOpKind.DS:
			case CommonOpKind.DX:
			case CommonOpKind.r16_rw:
			case CommonOpKind.Gw:
			case CommonOpKind.Ew:
			case CommonOpKind.AL:
			case CommonOpKind.r8_rb:
			case CommonOpKind.Gb:
			case CommonOpKind.Rd:
			case CommonOpKind.Rq:
			case CommonOpKind.RdMb:
			case CommonOpKind.RqMb:
			case CommonOpKind.RdMw:
			case CommonOpKind.RqMw:
			case CommonOpKind.Mb:
			case CommonOpKind.Mw:
			case CommonOpKind.Md:
			case CommonOpKind.Mq:
			case CommonOpKind.Eb:
			case CommonOpKind.VZ:
			case CommonOpKind.HZ:
			case CommonOpKind.RZ:
			case CommonOpKind.VM32X:
			case CommonOpKind.VM64X:
			case CommonOpKind.VM32Y:
			case CommonOpKind.VM64Y:
			case CommonOpKind.VM32Z:
			case CommonOpKind.VM64Z:
			case CommonOpKind.WZ:
			case CommonOpKind.VY:
			case CommonOpKind.HY:
			case CommonOpKind.RY:
			case CommonOpKind.WY:
			case CommonOpKind.Is4X:
			case CommonOpKind.Is5X:
			case CommonOpKind.Is4Y:
			case CommonOpKind.Is5Y:
			case CommonOpKind.VX:
			case CommonOpKind.HX:
			case CommonOpKind.RX:
			case CommonOpKind.WX:			
			case CommonOpKind.P:
			case CommonOpKind.N:			
			case CommonOpKind.Q:			
			case CommonOpKind.M:
			case CommonOpKind.WK:

				//case CommonOpKind.VK:
			//case CommonOpKind.RK:
			//case CommonOpKind.HK:

				return true;
			default:
				return false;
			}
		}
	}
}
