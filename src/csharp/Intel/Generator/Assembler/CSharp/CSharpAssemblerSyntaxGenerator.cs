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
		readonly CSharpDocCommentWriter _docWriter;

		public CSharpAssemblerSyntaxGenerator(GeneratorOptions generatorOptions) {
			Converter = CSharpIdentifierConverter.Create();
			_docWriter = new CSharpDocCommentWriter(Converter);
			_generatorOptions = generatorOptions;
		}

		IdentifierConverter Converter { get; }

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
							// Discard EIP/RIP from assembler registers
							if (name == "EIP" || name == "RIP") continue;
							var registerTypeName = $"AssemblerRegister{GetRegisterPostfix((Register)register.Value)}";
							writer.WriteLine($"public static readonly {registerTypeName} {name.ToLowerInvariant()} = new {registerTypeName}(Register.{name});");
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

			foreach (var bitness in new int[] {16, 32, 64}) {
				GenerateTests(bitness, groups);
			}
		}

		void GenerateTests(int bitness, OpCodeInfoGroup[] groups)
		{
			const string assemblerTestsNameBase = "AssemblerTests";
			string testName = assemblerTestsNameBase + bitness;

			var filenameTests = Path.Combine(Path.Combine(_generatorOptions.CSharpTestsDir, "Intel", assemblerTestsNameBase, $"{testName}.g.cs"));
			using (var writerTests = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filenameTests)))
			{
				writerTests.WriteFileHeader();
				writerTests.WriteLine($"#if {CSharpConstants.EncoderDefine}");
				writerTests.WriteLine($"namespace {CSharpConstants.IcedUnitTestsNamespace}.{assemblerTestsNameBase} {{");
				using (writerTests.Indent())
				{
					writerTests.WriteLine("using System;");
					writerTests.WriteLine("using System.Linq;");
					writerTests.WriteLine("using System.Text;");
					writerTests.WriteLine("using Iced.Intel;");
					writerTests.WriteLine("using Xunit;");
					writerTests.WriteLine("using static Iced.Intel.AssemblerRegisters;");

					writerTests.WriteLine($"public sealed partial class {testName} : AssemblerTestsBase {{");
					using (writerTests.Indent())
					{
						writerTests.WriteLine($"public {testName}() : base({bitness}) {{ }}");
						writerTests.WriteLine();

						OpCodeFlags bitnessFlags;
						switch (bitness)
						{
						case 64:
							bitnessFlags = OpCodeFlags.Mode64;
							break;
						case 32:
							bitnessFlags = OpCodeFlags.Mode32;
							break;
						case 16:
							bitnessFlags = OpCodeFlags.Mode16;
							break;
						default:
							throw new ArgumentException($"{bitness}");
						}

						foreach (var group in groups) {
							var groupBitness = group.AllOpCodeFlags & BitnessMaskFlags;
							if ((groupBitness & bitnessFlags) == 0) {
								continue;
							}
							
							var renderArgs = GetRenderArgs(@group);
							var methodName = Converter.Method(@group.Name);
							RenderTests(bitness, bitnessFlags, writerTests, methodName, @group, renderArgs);
						}
					}

					writerTests.WriteLine("}");
				}

				writerTests.WriteLine("}");
				writerTests.WriteLine("#endif");
			}
		}

		List<RenderArg> GetRenderArgs(OpCodeInfoGroup group) {
			var renderArgs = new List<RenderArg>();

			int immArg = 0;
							
			var signature = group.Signature;
			var lastArgKind = signature.ArgCount > 0 ? signature.GetArgKind(signature.ArgCount - 1) : ArgKind.Unknown;
			for (int i = 0; i < signature.ArgCount; i++) {
				string argName = i == 0 ? "dst" : "src";
				string argType = $"<unknown>";
				int maxArgSize = group.MaxArgSizes[i];
				var argKind = signature.GetArgKind(i);

				if (signature.ArgCount > 2 && i >= 1) {
					argName = $"src{i}";
				}
								
				switch (argKind) {
				case ArgKind.Register8:
					argType = "AssemblerRegister8";
					break;
				case ArgKind.Register16:
					argType = "AssemblerRegister16";
					break;
				case ArgKind.Register32:
					argType = "AssemblerRegister32";
					break;
				case ArgKind.Register64:
					argType = "AssemblerRegister64";
					break;
				case ArgKind.RegisterMM:
					argType = "AssemblerRegisterMM";
					break;
				case ArgKind.RegisterXMM:
					argType = "AssemblerRegisterXMM";
					break;
				case ArgKind.RegisterYMM:
					argType = "AssemblerRegisterYMM";
					break;
				case ArgKind.RegisterZMM:
					argType = "AssemblerRegisterZMM";
					break;
				case ArgKind.RegisterK:
					argType = "AssemblerRegisterK";
					break;
				case ArgKind.RegisterCR:
					argType = "AssemblerRegisterCR";
					break;
				case ArgKind.RegisterTR:
					argType = "AssemblerRegisterTR";
					break;
				case ArgKind.RegisterDR:
					argType = "AssemblerRegisterDR";
					break;
				case ArgKind.RegisterST:
					argType = "AssemblerRegisterST";
					break;
				case ArgKind.RegisterBND:
					argType = "AssemblerRegisterBND";
					break;
				case ArgKind.RegisterSegment:
					argType = "AssemblerRegisterSegment";
					break;
				
				case ArgKind.Label:
					argType = "Label";
					break;

				case ArgKind.Memory:
					argType = "AssemblerMemoryOperand";
					break;
								
				case ArgKind.Immediate:
				case ArgKind.ImmediateUnsigned:
					argName = $"imm{(immArg == 0 ? "" : immArg.ToString(CultureInfo.InvariantCulture))}";
					immArg++;
					Debug.Assert(maxArgSize > 0 && maxArgSize <= 8);
					bool isSigned = argKind == ArgKind.Immediate;
					argType = maxArgSize == 8 ? (isSigned ? "long" : "ulong") :
						maxArgSize == 4 ? (isSigned ? "int" : "uint") :
						maxArgSize == 2 ? (isSigned ? "short" : "ushort") :
						(isSigned ? "sbyte" : "byte");
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

			_docWriter.WriteSummary(writer, methodDoc.ToString(), "");

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
						if (renderArg.Kind == ArgKind.Label) {
							writer.Write(".Id");
						}
						else if (renderArg.Kind == ArgKind.Memory) {
							writer.Write(".ToMemoryOperand(Bitness)");
						}
					}

					writer.WriteLine("));");
				}
				else if (group.Flags == OpCodeArgFlags.Pseudo) {
					Debug.Assert(group.ParentPseudoOpsKind != null);
					writer.Write($"{group.ParentPseudoOpsKind.Name}(");
					for (var i = 0; i < renderArgs.Count; i++) {
						var renderArg = renderArgs[i];
						if (i > 0) writer.Write(", ");
						writer.Write(renderArg.Name);
					}
					writer.Write(", ");
					writer.Write($"{group.PseudoOpsKindImmediateValue}");
					writer.WriteLine(");");
				}
				else {
					if (!group.RootOpCodeNode.IsEmpty) {
						writer.WriteLine("Code op;");
						GenerateOpCodeSelector(writer, group, renderArgs);
					}

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
						else if (renderArg.Kind == ArgKind.Memory) {
							writer.Write(".ToMemoryOperand(Bitness)");
						}
					}

					writer.Write(")");

					bool hasFlags = false;
					if ((group.Flags & (OpCodeArgFlags.HasKMask | OpCodeArgFlags.HasZeroingMask)) != 0) {
						writer.Write($", {renderArgs[0].Name}.Flags");
						hasFlags = true;
					}

					bool hasBroadcast = (group.Flags & OpCodeArgFlags.HasBroadcast) != 0; 
					bool hasSaeOrRoundingControl = (group.Flags & (OpCodeArgFlags.SuppressAllExceptions | OpCodeArgFlags.RoundingControl)) != 0; 
					if (hasBroadcast || hasSaeOrRoundingControl) {
						for (int i = renderArgs.Count - 1; i >= 0; i--) {
							var argKind = renderArgs[i].Kind;
							if (hasBroadcast && argKind == ArgKind.Memory || hasSaeOrRoundingControl && !IsArgKindImmediate(argKind)) {
								if (hasFlags) {
									writer.Write(" | ");
								}
								else {
									writer.Write(", ");
								}

								writer.Write($"{renderArgs[i].Name}.Flags");
								break;
							}
						}
					}

					writer.WriteLine(");");
				}
			}

			writer.WriteLine("}");
		}
		
		void RenderTests(int bitness, OpCodeFlags bitnessFlags, FileWriter writer, string methodName, OpCodeInfoGroup group, List<RenderArg> renderArgs) {
			var fullMethodName = new StringBuilder();
			fullMethodName.Append(methodName);
			foreach (var renderArg in renderArgs) {
				fullMethodName.Append('_');
				switch (renderArg.Kind) {
				case ArgKind.Register8:
				case ArgKind.Register16:
				case ArgKind.Register32:
				case ArgKind.Register64:
				case ArgKind.RegisterK:
				case ArgKind.RegisterST:
				case ArgKind.RegisterSegment:
				case ArgKind.RegisterBND:
				case ArgKind.RegisterMM:
				case ArgKind.RegisterXMM:
				case ArgKind.RegisterYMM:
				case ArgKind.RegisterZMM:
				case ArgKind.RegisterCR:
				case ArgKind.RegisterDR:
				case ArgKind.RegisterTR:
					fullMethodName.Append(renderArg.Kind.ToString().Replace("Register", "reg"));
					break;
				case ArgKind.Memory:
					fullMethodName.Append("m");
					break;
				case ArgKind.Immediate:
					fullMethodName.Append("i");
					break;
				case ArgKind.ImmediateUnsigned:
					fullMethodName.Append("u");
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
				var argValues = new List<object?>(renderArgs.Count);
				for (int i = 0; i < renderArgs.Count; i++) {
					argValues.Add(null);
				}
				
				if (group.Flags == OpCodeArgFlags.Pseudo) {
					Debug.Assert(group.ParentPseudoOpsKind != null);
					GenerateTestAssemblerForOpCode(writer, bitness, bitnessFlags, @group, methodName, renderArgs, argValues, OpCodeArgFlags.Default, group.ParentPseudoOpsKind.Items[0]);
				}
				else {
					GenerateOpCodeTest(writer, bitness, bitnessFlags, group, methodName, group.RootOpCodeNode, renderArgs, argValues, OpCodeArgFlags.Default);
				}
			}
			writer.WriteLine("}");
			writer.WriteLine();;
		}
		
		void GenerateOpCodeTest(FileWriter writer, int bitness, OpCodeFlags bitnessFlags, OpCodeInfoGroup group, string methodName, OpCodeNode node, List<RenderArg> args, List<object?> argValues, OpCodeArgFlags contextFlags) {
			var opCodeInfo = node.OpCodeInfo;
			if (opCodeInfo != null) {
				GenerateTestAssemblerForOpCode(writer, bitness, bitnessFlags, @group, methodName, args, argValues, contextFlags, opCodeInfo);
			}
			else {
				var selector = node.Selector;
				Debug.Assert(selector != null);
				var argKind = selector.ArgIndex >= 0 ? args[selector.ArgIndex] : default;
				var condition = GetArgConditionForOpCodeKind(argKind, selector.Kind, selector.ArgIndex, group.Flags);
				var isSelectorSupportedByBitness = IsSelectorSupportedByBitness(bitness, selector.Kind, out var continueElse);
				var (contextIfFlags, contextElseFlags) = GetIfElseContextFlags(selector.Kind);
				if (isSelectorSupportedByBitness) {
					writer.WriteLine($"{{ /* if ({condition}) */");
					using (writer.Indent()) {
						foreach (var argValue in GetArgValue(bitness, selector.Kind, false, selector.ArgIndex, args)) {
							var newArgValues = new List<object?>(argValues);
							if (selector.ArgIndex >= 0) {
								newArgValues[selector.ArgIndex] = argValue;
							}
							GenerateOpCodeTest(writer, bitness, bitnessFlags, group, methodName, selector.IfTrue, args, newArgValues, contextFlags | contextIfFlags);
						}
					}
				}
				else {
					writer.WriteLine($"{{ // skip ({condition}) not supported by this Assembler bitness");
				}

				if (!selector.IfFalse.IsEmpty) {
					if (continueElse) {
						writer.Write("} /* else */ ");
						foreach (var argValue in GetArgValue(bitness, selector.Kind, true, selector.ArgIndex, args)) {
							var newArgValues = new List<object?>(argValues);
							if (selector.ArgIndex >= 0) {
								newArgValues[selector.ArgIndex] = argValue;
							}
							GenerateOpCodeTest(writer, bitness, bitnessFlags, group, methodName, selector.IfFalse, args, newArgValues, contextFlags | contextElseFlags);
						}
					}
					else {
						writer.WriteLine($"}} /* else skip ({condition}) not supported by this Assembler bitness */");
					}
				}
				else {
					writer.WriteLine("}");
					writer.WriteLine("{");
					using (writer.Indent()) {

						bool isGenerated = false;
						if (isSelectorSupportedByBitness && selector.ArgIndex >= 0) {
							var newArg = GetInvalidArgValue(bitness, selector.Kind, selector.ArgIndex);
							if (newArg != null) {

								// Force fake bitness support to allow to generate a throw for the last selector
								if (bitness == 64 && (group.Name == "bndcn" ||
								                      group.Name == "bndmk" || 
								                      group.Name == "bndcu" || 
								                      group.Name == "bndcl")) {
									bitness = bitness == 64 ? 32 : 16;
									bitnessFlags = bitness == 64 ? OpCodeFlags.Mode32:  OpCodeFlags.Mode16;
								}								
								
								writer.WriteLine("AssertInvalid( () => {");
								using (writer.Indent()) {

									var newArgValues = new List<object?>(argValues);
									newArgValues[selector.ArgIndex] = newArg;
									GenerateOpCodeTest(writer, bitness, bitnessFlags, group, methodName, selector.IfTrue, args, newArgValues, contextFlags | contextIfFlags);
									isGenerated = true;
								}
								writer.WriteLine("});");
							}
						}

						if (!isGenerated) {
							writer.WriteLine($"// See manual test for this case {methodName}");
						}
					}
					writer.WriteLine("}");
				}
			}
		}

		bool GenerateTestAssemblerForOpCode(FileWriter writer, int bitness, OpCodeFlags bitnessFlags, OpCodeInfoGroup @group, string methodName, List<RenderArg> args, List<object?> argValues, OpCodeArgFlags contextFlags, OpCodeInfo opCodeInfo)
		{
			if ((opCodeInfo.Flags & bitnessFlags) == 0 || (bitness == 16 && (methodName == "bndmov" || methodName == "bndldx" || methodName == "bndstx")))
			{
				writer.WriteLine("{");
				using (writer.Indent())
				{
					writer.WriteLine($"// Skipping {opCodeInfo.Code.RawName} - Not supported for {bitnessFlags}");
				}

				writer.WriteLine("}");
				return false;
			}

			bool isMoffs = IsMoffs(opCodeInfo);

			var assemblerArgs = new List<string>();
			var instructionCreateArgs = new List<string>();
			for (var i = 0; i < argValues.Count; i++)
			{
				var renderArg = args[i];
				var isMemory = renderArg.Kind == ArgKind.Memory;
				var argValueForAssembler = argValues[i]?.ToString();
				var argValueForInstructionCreate = argValueForAssembler;

				if (argValueForAssembler == null)
				{
					var localBitness = bitness;

					// Special case for movdir64b, the memory operand should match the register size
					// TODO: Ideally this should be handled in the base class
					switch ((Code) opCodeInfo.Code.Value)
					{
					case Code.Movdir64b_r16_m512:
					case Code.Enqcmds_r16_m512:
					case Code.Enqcmd_r16_m512:
						localBitness = 16;
						break;
					case Code.Movdir64b_r32_m512:
					case Code.Enqcmds_r32_m512:
					case Code.Enqcmd_r32_m512:
						localBitness = 32;
						break;
					case Code.Movdir64b_r64_m512:
					case Code.Enqcmds_r64_m512:
					case Code.Enqcmd_r64_m512:
						localBitness = 64;
						break;
					}

					argValueForAssembler = GetDefaultArgument(localBitness, opCodeInfo.OpKind(@group.NumberOfLeadingArgToDiscard + i), isMemory, true, i, renderArg);
					argValueForInstructionCreate = GetDefaultArgument(localBitness, opCodeInfo.OpKind(@group.NumberOfLeadingArgToDiscard + i), isMemory, false, i, renderArg);
				}

				if ((opCodeInfo.Flags & (OpCodeFlags.OpMaskRegister | OpCodeFlags.NonZeroOpMaskRegister)) != 0 && i == 0)
				{
					argValueForAssembler += ".k1";
					argValueForInstructionCreate += ".k1";
				}

				Debug.Assert(argValueForAssembler != null);
				assemblerArgs.Add(argValueForAssembler);
				Debug.Assert(argValueForInstructionCreate != null);
				if (renderArg.Kind == ArgKind.Memory && (!isMoffs || bitness != 64))
				{
					argValueForInstructionCreate += ".ToMemoryOperand(Bitness)";
				}

				instructionCreateArgs.Add(argValueForInstructionCreate);
			}
			
			if (group.Flags == OpCodeArgFlags.Pseudo)
			{
				instructionCreateArgs.Add($"{group.PseudoOpsKindImmediateValue}");			
			}

			var optionalOpCodeFlags = new List<string>();
			switch (contextFlags)
			{
			case OpCodeArgFlags.HasVex:
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferVex");
				break;
			case OpCodeArgFlags.HasEvex:
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferEvex");
				break;
			case OpCodeArgFlags.HasEvex | OpCodeArgFlags.HasBroadcast:
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferEvex");
				optionalOpCodeFlags.Add("LocalOpCodeFlags.Broadcast");
				break;
			case OpCodeArgFlags.HasBranchShort:
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferBranchShort");
				break;
			case OpCodeArgFlags.HasBranchNear:
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferBranchNear");
				break;
			}

			if ((opCodeInfo.Flags & OpCodeFlags.Fwait) != 0)
			{
				optionalOpCodeFlags.Add("LocalOpCodeFlags.Fwait");
			}

			if (@group.HasLabel)
			{
				optionalOpCodeFlags.Add("LocalOpCodeFlags.Branch");
			}

			string beginInstruction = $"Instruction.Create(Code.{opCodeInfo.Code.Name(Converter)}";
			string endInstruction = ")";
			if ((@group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0)
			{
				beginInstruction = $"Instruction.Create{@group.MemoName}(Bitness";
				if (@group.HasLabel)
				{
					beginInstruction = $"AssignLabel({beginInstruction}, {instructionCreateArgs[0]})";
				}
			}
			else if (@group.HasLabel)
			{
				beginInstruction = $"AssignLabel(Instruction.CreateBranch(Code.{opCodeInfo.Code.Name(Converter)}, {instructionCreateArgs[0]})";
			}

			if ((opCodeInfo.Flags & (OpCodeFlags.OpMaskRegister | OpCodeFlags.NonZeroOpMaskRegister)) != 0)
			{
				beginInstruction = $"ApplyK1({beginInstruction}";
				endInstruction = "))";
			}

			// Special case for moffs
			if (isMoffs && bitness == 64)
			{
				beginInstruction = $"CreateMemory64(Code.{opCodeInfo.Code.Name(Converter)}";
			}

			var assemblerArgsStr = string.Join(", ", assemblerArgs);
			var instructionCreateArgsStr = instructionCreateArgs.Count > 0 ? $", {string.Join(", ", instructionCreateArgs)}" : string.Empty;
			var optionalOpCodeFlagsStr = optionalOpCodeFlags.Count > 0 ? $", {string.Join(" | ", optionalOpCodeFlags)}" : string.Empty;

			writer.WriteLine($"TestAssembler(c => c.{methodName}({assemblerArgsStr}), {beginInstruction}{instructionCreateArgsStr}{endInstruction}{optionalOpCodeFlagsStr});");
			return true;
		}

		string GetDefaultArgument(int bitness, OpCodeOperandKind kind, bool asMemory, bool isAssembler, int index, RenderArg arg) {
			switch (kind) {
			case OpCodeOperandKind.farbr2_2:
				break;
			case OpCodeOperandKind.farbr4_2:
				break;
			case OpCodeOperandKind.mem_offs:
				if (bitness == 64 || bitness == 32) {
					return "__[12345]";
				}
				else if (bitness == 16) {
					return "__[67]";
				}

				break;
			case OpCodeOperandKind.mem:
			case OpCodeOperandKind.mem_mpx:
				if (bitness == 64) {
					return "__[rcx]";
				}
				else if (bitness == 32) {
					return "__[ecx]";
				}
				else if (bitness == 16) {
					return "__[si]";
				}

				break;
			case OpCodeOperandKind.mem_mib:
				if (bitness == 64) {
					return "__byte_ptr[rcx]";
				}
				else if (bitness == 32) {
					return "__byte_ptr[ecx]";
				}
				else if (bitness == 16) {
					return "__byte_ptr[si]";
				}
				break;
			case OpCodeOperandKind.mem_vsib32x:
			case OpCodeOperandKind.mem_vsib64x:
				if (bitness == 16) {
					return $"__[esi + xmm{index}]";
				} else if (bitness == 32) {
					return $"__[edx + xmm{index}]";
				} else if (bitness == 64) {
					return $"__[rdx + xmm{index}]";
				}			
				break;
			case OpCodeOperandKind.mem_vsib32y:
			case OpCodeOperandKind.mem_vsib64y:
				if (bitness == 16) {
					return $"__[esi + ymm{index}]";
				} else if (bitness == 32) {
					return $"__[edx + ymm{index}]";
				} else if (bitness == 64) {
					return $"__[rdx + ymm{index}]";
				}			
				break;
			case OpCodeOperandKind.mem_vsib32z:
			case OpCodeOperandKind.mem_vsib64z:
				if (bitness == 16) {
					return $"__[esi + zmm{index}]";
				} else if (bitness == 32) {
					return $"__[edx + zmm{index}]";
				} else if (bitness == 64) {
					return $"__[rdx + zmm{index}]";
				}			
				break;
			case OpCodeOperandKind.r8_or_mem:
				if (asMemory) {
					if (bitness == 64) {
						return "__byte_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__byte_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__byte_ptr[si]";
					}

					break;
				}
				else {
					return "bl";
				}
			case OpCodeOperandKind.r16_or_mem:
				if (asMemory) {
					if (bitness == 64) {
						return "__word_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__word_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__word_ptr[si]";
					}
				}
				else {
					return "bx";
				}

				break;
			case OpCodeOperandKind.r32_or_mem:
			case OpCodeOperandKind.r32_or_mem_mpx:
				if (asMemory) {
					if (bitness == 64) {
						return "__dword_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__dword_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__dword_ptr[si]";
					}
				}
				else {
					return "ebx";
				}

				break;
			case OpCodeOperandKind.r64_or_mem:
			case OpCodeOperandKind.r64_or_mem_mpx:
				if (asMemory) {
					if (bitness == 64) {
						return "__qword_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__qword_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__qword_ptr[si]";
					}
				}
				else {
					return "rbx";
				}

				break;
			case OpCodeOperandKind.mm_or_mem:
				if (asMemory) {
					if (bitness == 64) {
						return "__qword_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__qword_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__qword_ptr[si]";
					}
				}
				else {
					return "mm7";
				}

				break;
			case OpCodeOperandKind.xmm_or_mem:
				if (asMemory) {
					if (bitness == 64) {
						return "__xmmword_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__xmmword_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__xmmword_ptr[si]";
					}
				}
				else {
					return bitness <= 32 ? "xmm7" : "xmm9";
				}

				break;
			case OpCodeOperandKind.ymm_or_mem:
				if (asMemory) {
					if (bitness == 64) {
						return "__ymmword_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__ymmword_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__ymmword_ptr[si]";
					}
				}
				else {
					return $"ymm{index}";
				}
				break;
			case OpCodeOperandKind.zmm_or_mem:
				if (asMemory) {
					if (bitness == 64) {
						return "__zmmword_ptr[rcx]";
					}
					else if (bitness == 32) {
						return "__zmmword_ptr[ecx]";
					}
					else if (bitness == 16) {
						return "__zmmword_ptr[si]";
					}
				}
				else {
					return $"zmm{index}";
				}

				break;
			case OpCodeOperandKind.bnd_or_mem_mpx:
				if (asMemory) {
					if (bitness == 64) {
						return "__[rcx]";
					}
					else if (bitness == 32) {
						return "__[ecx]";
					}
					else if (bitness == 16) {
						return "__[si]";
					}
				}
				else {
					return "bnd2";
				}				
				break;
			case OpCodeOperandKind.k_or_mem:
				if (asMemory) {
					if (bitness == 64) {
						return "__[rcx]";
					}
					else if (bitness == 32) {
						return "__[ecx]";
					}
					else if (bitness == 16) {
						return "__[si]";
					}
				}
				else {
					return "k2";
				}				
				break;
			case OpCodeOperandKind.r8_reg:
			case OpCodeOperandKind.r8_opcode:
				return "cl";
			case OpCodeOperandKind.r16_reg:
			case OpCodeOperandKind.r16_rm:
			case OpCodeOperandKind.r16_opcode:
			case OpCodeOperandKind.r16_reg_mem:
				return index == 0 ? "cx" : "dx"; 
			case OpCodeOperandKind.r32_reg:
			case OpCodeOperandKind.r32_rm:
			case OpCodeOperandKind.r32_opcode:
			case OpCodeOperandKind.r32_vvvv:
			case OpCodeOperandKind.r32_reg_mem:
				return index == 0 ? "ecx" : "edx";
			case OpCodeOperandKind.r64_reg:
			case OpCodeOperandKind.r64_rm:
			case OpCodeOperandKind.r64_opcode:
			case OpCodeOperandKind.r64_vvvv:
			case OpCodeOperandKind.r64_reg_mem:
				return index == 0 ? "rcx" : "rdx";
			case OpCodeOperandKind.seg_reg:
				return "ds";
			case OpCodeOperandKind.k_reg:
			case OpCodeOperandKind.k_rm:
			case OpCodeOperandKind.kp1_reg:
			case OpCodeOperandKind.k_vvvv:
				return "k1";
			case OpCodeOperandKind.mm_reg:
			case OpCodeOperandKind.mm_rm:
				return "mm1";
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.xmm_rm:
			case OpCodeOperandKind.xmm_vvvv:
			case OpCodeOperandKind.xmmp3_vvvv:
			case OpCodeOperandKind.xmm_is4:
			case OpCodeOperandKind.xmm_is5:
				return $"xmm{index}";
			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.ymm_rm:
			case OpCodeOperandKind.ymm_vvvv:
			case OpCodeOperandKind.ymm_is4:
			case OpCodeOperandKind.ymm_is5:
				return $"ymm{index}";
			case OpCodeOperandKind.zmm_reg:
			case OpCodeOperandKind.zmm_rm:
			case OpCodeOperandKind.zmm_vvvv:
			case OpCodeOperandKind.zmmp3_vvvv:
				return $"zmm{index}";
			case OpCodeOperandKind.cr_reg:
				return "cr1";
			case OpCodeOperandKind.dr_reg:
				return "dr1";
			case OpCodeOperandKind.tr_reg:
				return "tr1";
			case OpCodeOperandKind.bnd_reg:
				return "bnd1";
			case OpCodeOperandKind.es:
				return "es";
			case OpCodeOperandKind.cs:
				return "cs";
			case OpCodeOperandKind.ss:
				return "ss";
			case OpCodeOperandKind.ds:
				return "ds";
			case OpCodeOperandKind.fs:
				return "fs";
			case OpCodeOperandKind.gs:
				return "gs";
			case OpCodeOperandKind.al:
				return "al";
			case OpCodeOperandKind.cl:
				return "cl";
			case OpCodeOperandKind.ax:
				return "ax";
			case OpCodeOperandKind.dx:
				return "dx";
			case OpCodeOperandKind.eax:
				return "eax";
			case OpCodeOperandKind.rax:
				return "rax";
			case OpCodeOperandKind.st0:
				return "st0";
			case OpCodeOperandKind.sti_opcode:
				return "st1";
			case OpCodeOperandKind.imm2_m2z:
				return "3";
			case OpCodeOperandKind.imm8:
				return arg.IsTypeSigned() ? "(sbyte)-5" : "(byte)127";
			case OpCodeOperandKind.imm8_const_1:
				return "1";
			case OpCodeOperandKind.imm8sex16:
				return "-5";
			case OpCodeOperandKind.imm8sex32:
				return "-9";
			case OpCodeOperandKind.imm8sex64:
				return "-10";
			case OpCodeOperandKind.imm16:
				return "16567";
			case OpCodeOperandKind.imm32:
				return "int.MaxValue";
			case OpCodeOperandKind.imm32sex64:
				return "int.MinValue";
			case OpCodeOperandKind.imm64:
				return "long.MinValue";
			case OpCodeOperandKind.seg_rSI:
				return "__[rsi]";
			case OpCodeOperandKind.es_rDI:
				return "__[rdi]";
			case OpCodeOperandKind.seg_rDI:
				return "__[rdi]";
			case OpCodeOperandKind.seg_rBX_al:
				return "__[rbx]";
			case OpCodeOperandKind.br16_1:
			case OpCodeOperandKind.br32_1:
			case OpCodeOperandKind.br64_1:
			case OpCodeOperandKind.br16_2:
			case OpCodeOperandKind.br32_4:
			case OpCodeOperandKind.br64_4:
			case OpCodeOperandKind.xbegin_2:
			case OpCodeOperandKind.xbegin_4:
			case OpCodeOperandKind.brdisp_2:
			case OpCodeOperandKind.brdisp_4:
				return isAssembler ? "CreateAndEmitLabel(c)" : "2"; // labels starts at 2 because label 1 is for first instruction
			
			default:
				throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
			}
			
			return $"GetArg_{kind}";
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
				var condition = GetArgConditionForOpCodeKind(selector.ArgIndex >= 0 ? args[selector.ArgIndex] : default, selector.Kind, selector.ArgIndex, group.Flags);
				if (selector.IsConditionInlineable && !IsMemOffs46Selector(selector.Kind)) {
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

						if (IsMemOffs46Selector(selector.Kind)) {
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

		static string GetArgConditionForOpCodeKind(RenderArg arg, OpCodeSelectorKind selectorKind, int index, OpCodeArgFlags groupFlags) {
			var regName = arg.Name;
			var otherRegName = arg.Name == "src" ? "dst" : "src";
			switch (selectorKind) {
			case OpCodeSelectorKind.MemOffs64_RAX:
				return $"{otherRegName}.Value == Register.RAX && Bitness == 64 && {regName}.IsDisplacementOnly";
			case OpCodeSelectorKind.MemOffs64_EAX:
				return $"{otherRegName}.Value == Register.EAX && Bitness == 64 && {regName}.IsDisplacementOnly";
			case OpCodeSelectorKind.MemOffs64_AX:
				return $"{otherRegName}.Value == Register.AX && Bitness == 64 && {regName}.IsDisplacementOnly";
			case OpCodeSelectorKind.MemOffs64_AL:
				return $"{otherRegName}.Value == Register.AL && Bitness == 64 && {regName}.IsDisplacementOnly";
			case OpCodeSelectorKind.MemOffs_RAX:
				return $"{otherRegName}.Value == Register.RAX && Bitness < 64 && {regName}.IsDisplacementOnly";
			case OpCodeSelectorKind.MemOffs_EAX:
				return $"{otherRegName}.Value == Register.EAX && Bitness < 64 && {regName}.IsDisplacementOnly";
			case OpCodeSelectorKind.MemOffs_AX:
				return $"{otherRegName}.Value == Register.AX && Bitness < 64 && {regName}.IsDisplacementOnly";
			case OpCodeSelectorKind.MemOffs_AL:
				return $"{otherRegName}.Value == Register.AL && Bitness < 64 && {regName}.IsDisplacementOnly";
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
			case OpCodeSelectorKind.ImmediateByteSigned: {
				return !arg.IsTypeSigned()
					? $"({arg.GetSignedTypeFromUnsigned()}){regName} >= sbyte.MinValue && ({arg.GetSignedTypeFromUnsigned()}){regName} <= sbyte.MaxValue"
					: $"{regName} >= sbyte.MinValue && {regName} <= sbyte.MaxValue";
			}
			case OpCodeSelectorKind.Vex:
				return "PreferVex";
			
			case OpCodeSelectorKind.EvexBroadcastX:
			case OpCodeSelectorKind.EvexBroadcastY:
			case OpCodeSelectorKind.EvexBroadcastZ:
				return $"{regName}.IsBroadcast";
			
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
			case OpCodeSelectorKind.Memory48:
				return $"{regName}.Size == MemoryOperandSize.FwordPtr";
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
			case OpCodeSelectorKind.MemoryIndex32Xmm:
			case OpCodeSelectorKind.MemoryIndex64Xmm:
				return $"{regName}.Index.IsXMM()";
			case OpCodeSelectorKind.MemoryIndex64Ymm:
			case OpCodeSelectorKind.MemoryIndex32Ymm:
				return $"{regName}.Index.IsYMM()";
			case OpCodeSelectorKind.MemoryIndex64Zmm:
			case OpCodeSelectorKind.MemoryIndex32Zmm:
				return $"{regName}.Index.IsZMM()";
			default:
				return $"invalid_selector_{selectorKind}_for_arg_{regName}";
			}
		}

		static string? GetInvalidArgValue(int bitness, OpCodeSelectorKind selectorKind, int argIndex) {
			switch (selectorKind) {
			case OpCodeSelectorKind.Memory8:
			case OpCodeSelectorKind.Memory16:
			case OpCodeSelectorKind.Memory32:
			case OpCodeSelectorKind.Memory48:
			case OpCodeSelectorKind.Memory80:
			case OpCodeSelectorKind.Memory64:
				if (bitness == 16) {
					return "__zmmword_ptr[di]";
				}
				else if (bitness == 32) {
					return "__zmmword_ptr[edx]";
				}
				else if (bitness == 64) {
					return "__zmmword_ptr[rdx]";
				}
				break;
			case OpCodeSelectorKind.MemoryMM:
			case OpCodeSelectorKind.MemoryXMM:
			case OpCodeSelectorKind.MemoryYMM:
			case OpCodeSelectorKind.MemoryZMM:
				if (bitness == 16) {
					return "__byte_ptr[di]";
				}
				else if (bitness == 32) {
					return "__byte_ptr[edx]";
				}
				else {
					return "__byte_ptr[rdx]";
				}
			case OpCodeSelectorKind.MemoryIndex32Xmm:
			case OpCodeSelectorKind.MemoryIndex64Xmm:
			case OpCodeSelectorKind.MemoryIndex64Ymm:
			case OpCodeSelectorKind.MemoryIndex32Ymm:
				if (bitness == 16) {
					return $"__[edi + zmm{argIndex}]";
				} else if (bitness == 32) {
					return $"__[edx + zmm{argIndex}]";
				} else {
					return $"__[rdx + zmm{argIndex}]";
				}
			}
	
			// Not supported
			return null;
		}
	
		static IEnumerable<string?> GetArgValue(int bitness, OpCodeSelectorKind selectorKind, bool isElseBranch, int index, List<RenderArg> args) {
			switch (selectorKind) {
			case OpCodeSelectorKind.MemOffs64_RAX:
			case OpCodeSelectorKind.MemOffs64_EAX:
			case OpCodeSelectorKind.MemOffs64_AX:
			case OpCodeSelectorKind.MemOffs64_AL:
				if (isElseBranch) {
					if (bitness == 64) {
						yield return index == 0 ? $"__[rdi]" : $"__[rsi]";
					}
					else if (bitness == 32) {
						yield return index == 0 ? $"__[edi]" : $"__[esi]";
					}
					else if (bitness == 16) {
						yield return index == 0 ? $"__[di]" : $"__[si]";
					}
				}
				else {
					yield return $"__[0x0123456789abcdef]";
				}
				break;			
			case OpCodeSelectorKind.MemOffs_RAX:
			case OpCodeSelectorKind.MemOffs_EAX:
			case OpCodeSelectorKind.MemOffs_AX:
			case OpCodeSelectorKind.MemOffs_AL:
				if (isElseBranch) {
					if (bitness == 64) {
						yield return index == 0 ? $"__[rdi]" : $"__[rsi]";
					}
					else if (bitness == 32) {
						yield return index == 0 ? $"__[edi]" : $"__[esi]";
					}
					else if (bitness == 16) {
						yield return index == 0 ? $"__[di]" : $"__[si]";
					}
				}
				else {
					yield return (bitness >= 32 ? $"__[0x01234567]" : $"__[0x01234]");
				}
				break;			
			case OpCodeSelectorKind.Bitness64:
				yield return "todo_bitness_64";
				break;			
			case OpCodeSelectorKind.Bitness32:
				yield return "todo_bitness_32";
				break;			
			case OpCodeSelectorKind.Bitness16:
				yield return "todo_bitness_16";
				break;			
			case OpCodeSelectorKind.BranchShort:
				if (isElseBranch) {
					yield return "c.PreferBranchShort = false;";
				}
				else {
					yield return "c.PreferBranchShort = true;";
				}
				break;			
			case OpCodeSelectorKind.ImmediateByteEqual1:
				if (isElseBranch) {
					yield return "2";
				}
				else {
					yield return "1";
				}
				break;			
			case OpCodeSelectorKind.ImmediateByteSigned:
				var arg = args[index];
				if (isElseBranch) {
					if (arg.IsTypeSigned()) {
						yield return $"({arg.Type})(sbyte.MinValue - 1)";
						yield return $"({arg.Type})(sbyte.MaxValue + 1)";
					}
					else {
						yield return $"({arg.GetSignedTypeFromUnsigned()})(sbyte.MinValue - 1)";
						yield return $"({arg.GetSignedTypeFromUnsigned()})(sbyte.MaxValue + 1)";
					}
				}
				else {
					if (arg.IsTypeSigned()) {
						yield return $"({arg.Type})sbyte.MinValue";
						yield return $"({arg.Type})sbyte.MaxValue";
					}
					else {
						yield return $"({arg.GetSignedTypeFromUnsigned()})sbyte.MinValue";
						yield return $"({arg.GetSignedTypeFromUnsigned()})sbyte.MaxValue";
					}
				}
				break;			
			case OpCodeSelectorKind.Vex:
				if (isElseBranch) {
					yield return "c.PreferVex = false;";
				}
				else {
					yield return "c.PreferVex = true;";
				}
				break;			
			case OpCodeSelectorKind.EvexBroadcastX:
			case OpCodeSelectorKind.EvexBroadcastY:
			case OpCodeSelectorKind.EvexBroadcastZ:
				if (isElseBranch) {
					yield return null;
				}
				else {
					if (bitness == 16) {
						yield return "__dword_bcst[di]";
					}
					else if (bitness == 32) {
						yield return "__dword_bcst[edx]";
					}
					else if (bitness == 64) {
						yield return "__dword_bcst[rdx]";
					}
				}
				break;			
			case OpCodeSelectorKind.RegisterCL:
				if (!isElseBranch) {
					yield return $"cl";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterAL:
				if (!isElseBranch) {
					yield return $"al";
				} else {
					yield return null;
				}

				break;			
			case OpCodeSelectorKind.RegisterAX:
				if (!isElseBranch) {
					yield return $"ax";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterEAX:
				if (!isElseBranch) {
					yield return $"eax";
				} else {
					yield return null;
				}

				break;			
			case OpCodeSelectorKind.RegisterRAX:
				if (!isElseBranch) {
					yield return $"rax";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterBND:
				if (!isElseBranch) {
					yield return $"bnd0";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterES:
				if (!isElseBranch) {
					yield return $"es";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterCS:
				if (!isElseBranch) {
					yield return $"cs";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterSS:
				if (!isElseBranch) {
					yield return $"ss";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterDS:
				if (!isElseBranch) {
					yield return $"ds";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterFS:
				if (!isElseBranch) {
					yield return $"fs";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterGS:
				if (!isElseBranch) {
					yield return $"gs";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterDX:
				if (!isElseBranch) {
					yield return $"dx";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.Register8:
				if (!isElseBranch) {
					yield return $"bl";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.Register16:
				if (!isElseBranch) {
					yield return $"bx";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.Register32:
				if (!isElseBranch) {
					yield return $"ebx";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.Register64:
				if (!isElseBranch) {
					yield return $"rbx";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterK:
				if (!isElseBranch) {
					yield return $"k1";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterST0:
				if (!isElseBranch) {
					yield return $"st0";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterST:
				if (!isElseBranch) {
					yield return $"st3";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterSegment:
				if (!isElseBranch) {
					yield return $"fs";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterCR:
				if (!isElseBranch) {
					yield return "cr3";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterDR:
				if (!isElseBranch) {
					yield return "dr5";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterTR:
				if (!isElseBranch) {
					yield return "tr4";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterMM:
				if (!isElseBranch) {
					yield return $"mm{index}";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterXMM:
				if (!isElseBranch) {
					yield return $"xmm{index}";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterYMM:
				if (!isElseBranch) {
					yield return $"ymm{index}";
				}
				else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.RegisterZMM:
				if (!isElseBranch) {
					yield return $"zmm{index}";
				} else {
					yield return null;
				}
				break;			
			case OpCodeSelectorKind.Memory8:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__byte_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__byte_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__byte_ptr[rdx]";
				}
				break;			
			case OpCodeSelectorKind.Memory16:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__word_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__word_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__word_ptr[rdx]";
				}
				break;			
			case OpCodeSelectorKind.Memory32:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					if (args.Count == 2 && (args[0].Kind == ArgKind.RegisterBND || args[1].Kind == ArgKind.RegisterBND)) {
						yield return "__dword_ptr[edi]";
					}
					else {
						yield return "__dword_ptr[di]";
					}
				}
				else if (bitness == 32) {
					yield return "__dword_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__dword_ptr[rdx]";
				}
				break;			
			case OpCodeSelectorKind.Memory80:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__tword_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__tword_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__tword_ptr[rdx]";
				}
				break;	
			case OpCodeSelectorKind.Memory48:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__fword_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__fword_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__fword_ptr[rdx]";
				}
				break;
			case OpCodeSelectorKind.Memory64:
			case OpCodeSelectorKind.MemoryMM:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__qword_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__qword_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__qword_ptr[rdx]";
				}
				break;			
			case OpCodeSelectorKind.MemoryXMM:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__xmmword_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__xmmword_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__xmmword_ptr[rdx]";
				}
				break;			
			case OpCodeSelectorKind.MemoryYMM:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__ymmword_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__ymmword_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__ymmword_ptr[rdx]";
				}
				break;			
			case OpCodeSelectorKind.MemoryZMM:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return "__zmmword_ptr[di]";
				}
				else if (bitness == 32) {
					yield return "__zmmword_ptr[edx]";
				}
				else if (bitness == 64) {
					yield return "__zmmword_ptr[rdx]";
				}
				break;
			// case OpCodeSelectorKind.ImmediateInt:
			// 	break;
			// case OpCodeSelectorKind.ImmediateByte:
			// 	break;
			// case OpCodeSelectorKind.ImmediateByteWith2Bits:
			// 	break;
			// case OpCodeSelectorKind.Memory:
			// 	break;
			// case OpCodeSelectorKind.MemoryK:
			// 	break;
			case OpCodeSelectorKind.MemoryIndex32Xmm:
			case OpCodeSelectorKind.MemoryIndex64Xmm:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return $"__[edi + xmm{index}]";
				} else if (bitness == 32) {
					yield return $"__[edx + xmm{index}]";
				} else if (bitness == 64) {
					yield return $"__[rdx + xmm{index}]";
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Ymm:
			case OpCodeSelectorKind.MemoryIndex64Ymm:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return $"__[edi + ymm{index}]";
				} else if (bitness == 32) {
					yield return $"__[edx + ymm{index}]";
				} else if (bitness == 64) {
					yield return $"__[rdx + ymm{index}]";
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Zmm:
			case OpCodeSelectorKind.MemoryIndex64Zmm:
				if (isElseBranch) {
					yield return null;
				} else if (bitness == 16) {
					yield return $"__[edi + zmm{index}]";
				} else if (bitness == 32) {
					yield return $"__[edx + zmm{index}]";
				} else if (bitness == 64) {
					yield return $"__[rdx + zmm{index}]";
				}
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(selectorKind), selectorKind, null);
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

			public bool IsTypeSigned() {
				switch (Type) {
				case "sbyte":
				case "short":
				case "int":
				case "long":
					return true;
				}
				return false;
			}

			public string GetSignedTypeFromUnsigned() {
				Debug.Assert(!IsTypeSigned());
				switch (Type) {
				case "byte":
					return "sbyte";
				case "ushort":
					return "short";
				case "uint":
					return "int";
				case "ulong":
					return "long";
				}
				throw new ArgumentException($"Invalid {Type}");
			}
		}
	}
}
