// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Generator.Documentation.CSharp;
using Generator.Enums;
using Generator.IO;
using Generator.Tables;

namespace Generator.Assembler.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpAssemblerSyntaxGenerator : AssemblerSyntaxGenerator {
		readonly IdentifierConverter idConverter;
		readonly CSharpDocCommentWriter docWriter;
		readonly EnumType memoryOperandSizeType;

		static readonly List<(string, int, string[], string)> declareDataList = new List<(string, int, string[], string)> {
			("db", 1, new[] { "byte", "sbyte" }, "CreateDeclareByte"),
			("dw", 2, new[] { "ushort", "short" }, "CreateDeclareWord"),
			("dd", 4, new[] { "uint", "int", "float" }, "CreateDeclareDword"),
			("dq", 8, new[] { "ulong", "long", "double" }, "CreateDeclareQword"),
		};

		static readonly Dictionary<int, HashSet<string>> IgnoredTestsPerBitness = new Dictionary<int, HashSet<string>>() {
			// generates  System.InvalidOperationException : Operand 0: Expected: NearBranch16, actual: NearBranch32 : 0x1 jecxz 000031D0h
			{ 16, new HashSet<string> { "jecxz_lu" } },
			// generates  System.InvalidOperationException : Operand 0: Expected: NearBranch32, actual: NearBranch16 : 0x1 jcxz 31D0h
			{ 32, new HashSet<string> { "jcxz_lu" } },
		};

		public CSharpAssemblerSyntaxGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(idConverter);
			memoryOperandSizeType = genTypes[TypeIds.CodeAsmMemoryOperandSize];
		}

		static string GetRegisterSuffix(RegisterDef def) => GetRegisterSuffix(def.GetRegisterKind());
		static string GetRegisterSuffix(RegisterKind kind) =>
			kind switch {
				RegisterKind.GPR8 => "8",
				RegisterKind.GPR16 => "16",
				RegisterKind.GPR32 => "32",
				RegisterKind.GPR64 => "64",
				RegisterKind.IP => "IP",
				RegisterKind.Segment => "Segment",
				RegisterKind.ST => "ST",
				RegisterKind.CR => "CR",
				RegisterKind.DR => "DR",
				RegisterKind.TR => "TR",
				RegisterKind.BND => "BND",
				RegisterKind.K => "K",
				RegisterKind.MM => "MM",
				RegisterKind.XMM => "XMM",
				RegisterKind.YMM => "YMM",
				RegisterKind.ZMM => "ZMM",
				RegisterKind.TMM => "TMM",
				_ => throw new InvalidOperationException(),
			};

		protected override void GenerateRegisters((RegisterKind kind, RegisterDef[] regs)[] regGroups) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "Assembler", "AssemblerRegisters.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.CodeAssemblerDefine}");

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				writer.WriteLine(CSharpConstants.PragmaMissingDocsDisable);
				using (writer.Indent()) {
					writer.WriteLine("public static partial class AssemblerRegisters {");
					using (writer.Indent()) {
						foreach (var regDef in regGroups.SelectMany(a => a.regs)) {
							var asmRegName = GetAsmRegisterName(regDef);
							var registerTypeName = $"AssemblerRegister{GetRegisterSuffix(regDef)}";
							writer.WriteLine($"public static readonly {registerTypeName} {asmRegName} = new {registerTypeName}({regDef.Register.DeclaringType.Name(idConverter)}.{regDef.Register.Name(idConverter)});");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void GenerateRegisterClasses(RegisterClassInfo[] infos) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "Assembler", "AssemblerRegister.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.CodeAssemblerDefine}");
				writer.WriteLine("using System;");
				writer.WriteLine("using System.Diagnostics;");
				writer.WriteLine("using System.ComponentModel;");
				writer.WriteLine();

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					var regNoneName = genTypes[TypeIds.Register][nameof(Register.None)].Name(idConverter);
					var registerTypeName = genTypes[TypeIds.Register].Name(idConverter);
					var memOpNoneName = genTypes[TypeIds.CodeAsmMemoryOperandSize][nameof(MemoryOperandSize.None)].Name(idConverter);
					var memOpTypeName = genTypes[TypeIds.CodeAsmMemoryOperandSize].Name(idConverter);
					for (int i = 0; i < infos.Length; i++) {
						var reg = infos[i];
						var className = $"AssemblerRegister{GetRegisterSuffix(reg.Kind)}";
						var isName = reg.Kind switch {
							RegisterKind.None => throw new InvalidOperationException(),
							RegisterKind.GPR8 => "GPR8",
							RegisterKind.GPR16 => "GPR16",
							RegisterKind.GPR32 => "GPR32",
							RegisterKind.GPR64 => "GPR64",
							RegisterKind.IP => "IP",
							RegisterKind.Segment => "SegmentRegister",
							RegisterKind.ST => "ST",
							RegisterKind.CR => "CR",
							RegisterKind.DR => "DR",
							RegisterKind.TR => "TR",
							RegisterKind.BND => "BND",
							RegisterKind.K => "K",
							RegisterKind.MM => "MM",
							RegisterKind.XMM => "XMM",
							RegisterKind.YMM => "YMM",
							RegisterKind.ZMM => "ZMM",
							RegisterKind.TMM => "TMM",
							_ => throw new InvalidOperationException(),
						};

						if (i != 0)
							writer.WriteLine();
						writer.WriteLine("/// <summary>");
						writer.WriteLine("/// An assembler register used with <see cref=\"Assembler\"/>.");
						writer.WriteLine("/// </summary>");
						writer.WriteLine("[DebuggerDisplay(\"{\" + nameof(Value) + \"}\")]");
						writer.WriteLine("[EditorBrowsable(EditorBrowsableState.Never)]");
						writer.WriteLine($"public readonly partial struct {className} : IEquatable<{className}> {{");
						using (writer.Indent()) {
							writer.WriteLine("/// <summary>");
							writer.WriteLine("/// Creates a new instance.");
							writer.WriteLine("/// </summary>");
							writer.WriteLine("/// <param name=\"value\">A Register</param>");
							writer.WriteLine($"public {className}({registerTypeName} value) {{");
							using (writer.Indent()) {
								writer.WriteLine($"if (!value.Is{isName}())");
								using (writer.Indent())
									writer.WriteLine($"throw new ArgumentOutOfRangeException($\"Invalid register {{value}}. Must be a {isName} register\", nameof(value));");
								writer.WriteLine("Value = value;");
								if (reg.NeedsState)
									writer.WriteLine("Flags = AssemblerOperandFlags.None;");
							}
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/// <summary>");
							writer.WriteLine("/// The register value.");
							writer.WriteLine("/// </summary>");
							writer.WriteLine($"public readonly {registerTypeName} Value;");
							if (reg.NeedsState) {
								writer.WriteLine();
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Creates a new instance.");
								writer.WriteLine("/// </summary>");
								writer.WriteLine("/// <param name=\"value\">A register</param>");
								writer.WriteLine("/// <param name=\"flags\">The mask</param>");
								writer.WriteLine($"public {className}({registerTypeName} value, AssemblerOperandFlags flags) {{");
								using (writer.Indent()) {
									writer.WriteLine("Value = value;");
									writer.WriteLine("Flags = flags;");
								}
								writer.WriteLine("}");
								writer.WriteLine();
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Gets the mask associated with this register.");
								writer.WriteLine("/// </summary>");
								writer.WriteLine("internal readonly AssemblerOperandFlags Flags;");
								for (int j = 1; j <= 7; j++) {
									writer.WriteLine();
									writer.WriteLine("/// <summary>");
									writer.WriteLine($"/// Apply mask Register K{j}.");
									writer.WriteLine("/// </summary>");
									writer.WriteLine($"public {className} k{j} => new {className}(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K{j});");
								}
								writer.WriteLine();
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Apply mask Zeroing.");
								writer.WriteLine("/// </summary>");
								writer.WriteLine($"public {className} z => new {className}(Value, Flags | AssemblerOperandFlags.Zeroing);");
								if (reg.HasSaeOrEr) {
									writer.WriteLine();
									writer.WriteLine("/// <summary>");
									writer.WriteLine("/// Suppress all exceptions.");
									writer.WriteLine("/// </summary>");
									writer.WriteLine($"public {className} sae => new {className}(Value, Flags | AssemblerOperandFlags.SuppressAllExceptions);");
									writer.WriteLine();
									writer.WriteLine("/// <summary>");
									writer.WriteLine("/// Rounding to nearest.");
									writer.WriteLine("/// </summary>");
									writer.WriteLine($"public {className} rn_sae => new {className}(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundToNearest);");
									writer.WriteLine();
									writer.WriteLine("/// <summary>");
									writer.WriteLine("/// Rounding down.");
									writer.WriteLine("/// </summary>");
									writer.WriteLine($"public {className} rd_sae => new {className}(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundDown);");
									writer.WriteLine();
									writer.WriteLine("/// <summary>");
									writer.WriteLine("/// Rounding up.");
									writer.WriteLine("/// </summary>");
									writer.WriteLine($"public {className} ru_sae => new {className}(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundUp);");
									writer.WriteLine();
									writer.WriteLine("/// <summary>");
									writer.WriteLine("/// Rounding toward zero.");
									writer.WriteLine("/// </summary>");
									writer.WriteLine($"public {className} rz_sae => new {className}(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundTowardZero);");
								}
							}
							writer.WriteLine();
							writer.WriteLine("/// <summary>");
							writer.WriteLine($"/// Converts a <see cref=\"{className}\"/> to a <see cref=\"{registerTypeName}\"/>.");
							writer.WriteLine("/// </summary>");
							writer.WriteLine($"/// <param name=\"reg\">{className}</param>");
							writer.WriteLine("/// <returns></returns>");
							writer.WriteLine($"public static implicit operator {registerTypeName}({className} reg) => reg.Value;");
							if (reg.IsGPR16_32_64) {
								writer.WriteLine();
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Adds a register (base) to another register (index) and return a memory operand.");
								writer.WriteLine("/// </summary>");
								writer.WriteLine("/// <param name=\"left\">The base register.</param>");
								writer.WriteLine("/// <param name=\"right\">The index register</param>");
								writer.WriteLine("/// <returns></returns>");
								writer.WriteLine($"public static AssemblerMemoryOperand operator +({className} left, {className} right) =>");
								using (writer.Indent())
									writer.WriteLine($"new AssemblerMemoryOperand({memOpTypeName}.{memOpNoneName}, {registerTypeName}.{regNoneName}, left, right, 1, 0, AssemblerOperandFlags.None);");
								if (reg.IsGPR32_64) {
									foreach (var mm in new[] { "XMM", "YMM", "ZMM" }) {
										writer.WriteLine();
										writer.WriteLine("/// <summary>");
										writer.WriteLine("/// Adds a register (base) to another register (index) and return a memory operand.");
										writer.WriteLine("/// </summary>");
										writer.WriteLine("/// <param name=\"left\">The base register.</param>");
										writer.WriteLine("/// <param name=\"right\">The index register</param>");
										writer.WriteLine("/// <returns></returns>");
										writer.WriteLine($"public static AssemblerMemoryOperand operator +({className} left, AssemblerRegister{mm} right) =>");
										using (writer.Indent())
											writer.WriteLine($"new AssemblerMemoryOperand({memOpTypeName}.{memOpNoneName}, {registerTypeName}.{regNoneName}, left, right, 1, 0, AssemblerOperandFlags.None);");
									}
								}
							}
							if (reg.IsGPR16_32_64 || reg.IsVector) {
								writer.WriteLine();
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Adds a register (base) with a displacement and return a memory operand.");
								writer.WriteLine("/// </summary>");
								writer.WriteLine("/// <param name=\"left\">The base register</param>");
								writer.WriteLine("/// <param name=\"displacement\">The displacement</param>");
								writer.WriteLine("/// <returns></returns>");
								writer.WriteLine($"public static AssemblerMemoryOperand operator +({className} left, int displacement) =>");
								using (writer.Indent())
									writer.WriteLine($"new AssemblerMemoryOperand({memOpTypeName}.{memOpNoneName}, {registerTypeName}.{regNoneName}, left, {registerTypeName}.{regNoneName}, 1, displacement, AssemblerOperandFlags.None);");
								writer.WriteLine();
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Subtracts a register (base) with a displacement and return a memory operand.");
								writer.WriteLine("/// </summary>");
								writer.WriteLine("/// <param name=\"left\">The base register</param>");
								writer.WriteLine("/// <param name=\"displacement\">The displacement</param>");
								writer.WriteLine("/// <returns></returns>");
								writer.WriteLine($"public static AssemblerMemoryOperand operator -({className} left, int displacement) =>");
								using (writer.Indent())
									writer.WriteLine($"new AssemblerMemoryOperand({memOpTypeName}.{memOpNoneName}, {registerTypeName}.{regNoneName}, left, {registerTypeName}.{regNoneName}, 1, -displacement, AssemblerOperandFlags.None);");
								writer.WriteLine();
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Multiplies an index register by a scale and return a memory operand.");
								writer.WriteLine("/// </summary>");
								writer.WriteLine("/// <param name=\"left\">The base register</param>");
								writer.WriteLine("/// <param name=\"scale\">The scale</param>");
								writer.WriteLine("/// <returns></returns>");
								writer.WriteLine($"public static AssemblerMemoryOperand operator *({className} left, int scale) =>");
								using (writer.Indent())
									writer.WriteLine($"new AssemblerMemoryOperand({memOpTypeName}.{memOpNoneName}, {registerTypeName}.{regNoneName}, {registerTypeName}.{regNoneName}, left, scale, 0, AssemblerOperandFlags.None);");
							}
							if (reg.NeedsState) {
								writer.WriteLine();
								writer.WriteLine("/// <inheritdoc />");
								writer.WriteLine($"public bool Equals({className} other) => Value == other.Value && Flags == other.Flags;");
								writer.WriteLine();
								writer.WriteLine("/// <inheritdoc />");
								writer.WriteLine("public override int GetHashCode() => ((int)Value * 397) ^ (int)Flags;");
							}
							else {
								writer.WriteLine();
								writer.WriteLine("/// <inheritdoc />");
								writer.WriteLine($"public bool Equals({className} other) => Value == other.Value;");
								writer.WriteLine();
								writer.WriteLine("/// <inheritdoc />");
								writer.WriteLine("public override int GetHashCode() => (int)Value;");
							}
							writer.WriteLine();
							writer.WriteLine("/// <inheritdoc />");
							writer.WriteLine($"public override bool Equals(object? obj) => obj is {className} other && Equals(other);");
							writer.WriteLine();
							writer.WriteLine("/// <summary>");
							writer.WriteLine($"/// Equality operator for <see cref=\"{className}\"/>");
							writer.WriteLine("/// </summary>");
							writer.WriteLine("/// <param name=\"left\">Register</param>");
							writer.WriteLine("/// <param name=\"right\">Register</param>");
							writer.WriteLine("/// <returns></returns>");
							writer.WriteLine($"public static bool operator ==({className} left, {className} right) => left.Equals(right);");
							writer.WriteLine();
							writer.WriteLine("/// <summary>");
							writer.WriteLine($"/// Inequality operator for <see cref=\"{className}\"/>");
							writer.WriteLine("/// </summary>");
							writer.WriteLine("/// <param name=\"left\">Register</param>");
							writer.WriteLine("/// <param name=\"right\">Register</param>");
							writer.WriteLine("/// <returns></returns>");
							writer.WriteLine($"public static bool operator !=({className} left, {className} right) => !left.Equals(right);");
						}
						writer.WriteLine("}");
						if (reg.IsGPR16_32_64) {
							writer.WriteLine();
							writer.WriteLine("public readonly partial struct AssemblerMemoryOperandFactory {");
							using (writer.Indent()) {
								writer.WriteLine("/// <summary>");
								writer.WriteLine("/// Specify a base register used with this memory operand (Base + Index * Scale + Displacement)");
								writer.WriteLine("/// </summary>");
								writer.WriteLine("/// <param name=\"register\">Size of this memory operand.</param>");
								writer.WriteLine($"public AssemblerMemoryOperand this[{className} register] => new AssemblerMemoryOperand(Size, Segment, register, {registerTypeName}.{regNoneName}, 1, 0, Flags);");
							}
							writer.WriteLine("}");
						}
					}
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void GenerateMemorySizeFunctions(MemorySizeFuncInfo[] infos) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "Assembler", "AssemblerRegisters2.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.CodeAssemblerDefine}");

				var memOpSizeType = genTypes[TypeIds.CodeAsmMemoryOperandSize];
				var memOpSizeTypeStr = memOpSizeType.Name(idConverter);
				var regType = genTypes[TypeIds.Register];
				var regNoneStr = $"{regType.Name(idConverter)}.{regType[nameof(Register.None)].Name(idConverter)}";

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("/// <summary>");
					writer.WriteLine("/// Registers used for <see cref=\"Assembler\"/>. ");
					writer.WriteLine("/// </summary>");
					writer.WriteLine("public static partial class AssemblerRegisters {");
					using (writer.Indent()) {
						for (int i = 0; i < infos.Length; i++) {
							var info = infos[i];
							var (name, desc) = info.Kind switch {
								MemorySizeFnKind.Ptr => (string.Empty, "no"),
								MemorySizeFnKind.Bcst => ("bcst", "no"),
								_ => (info.Name.Replace(' ', '_'), AOrAn(info.Kind, $"<c>{info.Name.ToUpperInvariant()}</c>")),
							};
							var bcstDesc = info.IsBroadcast ? "broadcasted " : string.Empty;

							if (i != 0)
								writer.WriteLine();
							writer.WriteLine("/// <summary>");
							writer.WriteLine($"/// Gets a {bcstDesc}memory operand with {desc} size hint");
							writer.WriteLine("/// </summary>");
							var enumValueStr = memOpSizeType[info.Size.ToString()].Name(idConverter);
							if (info.IsBroadcast)
								writer.WriteLine($"public static readonly AssemblerMemoryOperandFactory __{name} = new AssemblerMemoryOperandFactory({memOpSizeTypeStr}.{enumValueStr}, {regNoneStr}, AssemblerOperandFlags.Broadcast);");
							else
								writer.WriteLine($"public static readonly AssemblerMemoryOperandFactory __{name} = new AssemblerMemoryOperandFactory({memOpSizeTypeStr}.{enumValueStr});");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}
		protected override void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {
			GenerateCode(groups);
			GenerateTests(groups);
		}

		void GenerateCode(OpCodeInfoGroup[] groups) {
			var filename = CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "Assembler", "Assembler.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.CodeAssemblerDefine}");
				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("public partial class Assembler {");
					using (writer.Indent()) {
						foreach (var group in groups) {
							var renderArgs = GetRenderArgs(group);
							var methodName = idConverter.Method(group.Name);
							GenerateAssemblerCode(writer, methodName, group, renderArgs);
						}

						GenerateDeclareDataCode(writer);
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		void GenerateDeclareDataCode(FileWriter writer) {
			foreach (var (name, size, types, methodName) in declareDataList) {
				int maxSize = 16;
				int argCount = maxSize / size;

				for (var typeIndex = 0; typeIndex < types.Length; typeIndex++) {
					var type = types[typeIndex];
					bool isUnsafe = type == "float" || type == "double";
					for (int i = 1; i <= argCount; i++) {
						docWriter.WriteSummary(writer, $"Creates a {name} asm directive with the type {type}.", "");
						writer.Write($"public {(isUnsafe ? "unsafe " : "")}void {name}(");
						for (int j = 0; j < i; j++) {
							if (j > 0) writer.Write(", ");
							writer.Write($"{type} imm{j}");
						}

						writer.WriteLine(") {");
						using (writer.Indent()) {
							writer.Write($"AddInstruction(Instruction.{methodName}(");
							for (int j = 0; j < i; j++) {
								if (j > 0) writer.Write(", ");
								if (typeIndex == 0)
									writer.Write($"imm{j}");
								else
									writer.Write(isUnsafe ? $"*({types[0]}*)&imm{j}" : $"({types[0]})imm{j}");
							}

							writer.WriteLine("));");
						}

						writer.WriteLine("}");
					}
				}
			}
		}

		void GenerateTests(OpCodeInfoGroup[] groups) {
			foreach (var bitness in new[] { 16, 32, 64 })
				GenerateAssemblerTests(bitness, groups);
		}

		void GenerateAssemblerTests(int bitness, OpCodeInfoGroup[] groups) {
			const string assemblerTestsNameBase = "AssemblerTests";
			string testName = assemblerTestsNameBase + bitness;

			var filenameTests = genTypes.Dirs.GetCSharpTestFilename("Intel", assemblerTestsNameBase, $"{testName}.g.cs");
			using (var writerTests = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filenameTests))) {
				writerTests.WriteFileHeader();
				writerTests.WriteLine($"#if {CSharpConstants.CodeAssemblerDefine}");
				writerTests.WriteLine($"namespace {CSharpConstants.IcedUnitTestsNamespace}.{assemblerTestsNameBase} {{");
				using (writerTests.Indent()) {
					writerTests.WriteLine("using Iced.Intel;");
					writerTests.WriteLine("using Xunit;");
					writerTests.WriteLine("using static Iced.Intel.AssemblerRegisters;");

					writerTests.WriteLine($"public sealed partial class {testName} : AssemblerTestsBase {{");
					using (writerTests.Indent()) {
						writerTests.WriteLine($"public {testName}() : base({bitness}) {{ }}");
						writerTests.WriteLine();

						var bitnessFlags = bitness switch {
							64 => InstructionDefFlags1.Bit64,
							32 => InstructionDefFlags1.Bit32,
							16 => InstructionDefFlags1.Bit16,
							_ => throw new ArgumentException($"{bitness}"),
						};

						foreach (var group in groups) {
							switch (group.Name) {
							case "xbegin":
								// Implemented manually
								continue;
							}
							var groupBitness = group.AllOpCodeFlags & BitnessMaskFlags;
							if ((groupBitness & bitnessFlags) == 0)
								continue;

							var renderArgs = GetRenderArgs(@group);
							var methodName = idConverter.Method(@group.Name);
							RenderTests(bitness, bitnessFlags, writerTests, methodName, @group, renderArgs);
						}
					}

					if (bitness == 64)
						GenerateDeclareDataTests(writerTests);

					writerTests.WriteLine("}");
				}

				writerTests.WriteLine("}");
				writerTests.WriteLine("#endif");
			}
		}

		void GenerateDeclareDataTests(FileWriter writer) {
			foreach (var (name, size, types, _) in declareDataList) {
				int maxSize = 16;
				int argCount = maxSize / size;

				for (var typeIndex = 0; typeIndex < types.Length; typeIndex++) {
					var type = types[typeIndex];
					for (int i = 1; i <= argCount; i++) {
						docWriter.WriteSummary(writer, $"Creates a {name} asm directive with the type {type}.", "");
						writer.WriteLine("[Fact]");
						writer.WriteLine($"public void TestDeclareData_{name}_{type}_{i}() {{");
						using (writer.Indent()) {
							writer.Write($"TestAssemblerDeclareData(c => c.{name}(");
							for (int j = 0; j < i; j++) {
								if (j > 0) writer.Write(", ");
								writer.Write($"({type}){j + 1}");
							}
							writer.Write($"), new {type}[] {{");
							for (int j = 0; j < i; j++) {
								if (j > 0) writer.Write(", ");
								writer.Write($"({type}){j + 1}");
							}
							writer.WriteLine("});");
						}

						writer.WriteLine("}");
					}
				}
			}
		}

		static List<RenderArg> GetRenderArgs(OpCodeInfoGroup group) {
			var renderArgs = new List<RenderArg>();
			int immArg = 0;
			var signature = group.Signature;
			for (int i = 0; i < signature.ArgCount; i++) {
				string argName = i == 0 ? "dst" : "src";
				int maxArgSize = group.MaxArgSizes[i];
				var argKind = signature.GetArgKind(i);

				if (signature.ArgCount > 2 && i >= 1)
					argName = $"src{i}";

				string argType;
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
				case ArgKind.RegisterTMM:
					argType = "AssemblerRegisterTMM";
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

				case ArgKind.LabelUlong:
					argType = "ulong";
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

		void GenerateAssemblerCode(FileWriter writer, string methodName, OpCodeInfoGroup group, List<RenderArg> renderArgs) {
			// Write documentation
			var methodDoc = new StringBuilder();
			methodDoc.Append($"{group.Name} instruction.");
			foreach (var code in group.Items) {
				if (!string.IsNullOrEmpty(code.Code.Documentation)) {
					methodDoc.Append("#(p:)#");
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
						if (renderArg.Kind == ArgKind.Label)
							writer.Write(".Id");
						else if (renderArg.Kind == ArgKind.Memory)
							writer.Write(".ToMemoryOperand(Bitness)");
					}

					writer.WriteLine("));");
				}
				else if (group.Flags == OpCodeArgFlags.Pseudo) {
					Debug.Assert(group.ParentPseudoOpsKind is not null);
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

					if (group.HasLabel)
						writer.Write("AddInstruction(Instruction.CreateBranch(op");
					else
						writer.Write("AddInstruction(Instruction.Create(op");

					for (var i = 0; i < renderArgs.Count; i++) {
						var renderArg = renderArgs[i];
						writer.Write(", ");

						var argName = renderArg.Name;

						// Perform casting for unsigned
						if (IsArgKindImmediate(renderArg.Kind) && !renderArg.IsTypeSigned()) {
							if (renderArg.Type == "ulong")
								argName = $"(ulong){argName}";
							else if (renderArg.Type != "uint")
								argName = $"(uint){argName}";
						}

						writer.Write(argName);
						if (renderArg.Kind == ArgKind.Label)
							writer.Write(".Id");
						else if (renderArg.Kind == ArgKind.Memory)
							writer.Write(".ToMemoryOperand(Bitness)");
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
								if (hasFlags)
									writer.Write(" | ");
								else
									writer.Write(", ");

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

		[Flags]
		enum EncodingFlags {
			None = 0,
			Legacy = 1,
			VEX = 2,
			EVEX = 4,
			XOP = 8,
			D3NOW = 0x10,
		}

		EncodingFlags GetEncodingFlags(OpCodeInfoGroup group) {
			var flags = EncodingFlags.None;
			foreach (var def in group.Items.Select(a => defs[(int)a.Code.Value])) {
				flags |= def.Encoding switch {
					EncodingKind.Legacy => EncodingFlags.Legacy,
					EncodingKind.VEX => EncodingFlags.VEX,
					EncodingKind.EVEX => EncodingFlags.EVEX,
					EncodingKind.XOP => EncodingFlags.XOP,
					EncodingKind.D3NOW => EncodingFlags.D3NOW,
					_ => throw new InvalidOperationException(),
				};
			}
			return flags;
		}

		string? GetDefine(OpCodeInfoGroup group) {
			EncodingFlags flags;
			if (group.ParentPseudoOpsKind is OpCodeInfoGroup parent)
				flags = GetEncodingFlags(parent);
			else
				flags = GetEncodingFlags(group);
			if (flags == EncodingFlags.None)
				throw new InvalidOperationException();
			if (flags == EncodingFlags.Legacy)
				return null;
			var defines = new List<string>();
			if ((flags & EncodingFlags.VEX) != 0)
				defines.Add(CSharpConstants.VexDefine);
			if ((flags & EncodingFlags.EVEX) != 0)
				defines.Add(CSharpConstants.EvexDefine);
			if ((flags & EncodingFlags.XOP) != 0)
				defines.Add(CSharpConstants.XopDefine);
			if ((flags & EncodingFlags.D3NOW) != 0)
				defines.Add(CSharpConstants.D3nowDefine);
			return string.Join(" && ", defines.ToArray());
		}

		void RenderTests(int bitness, InstructionDefFlags1 bitnessFlags, FileWriter writer, string methodName, OpCodeInfoGroup group, List<RenderArg> renderArgs) {
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
				case ArgKind.RegisterTMM:
					fullMethodName.Append(renderArg.Kind.ToString().Replace("Register", "reg"));
					break;
				case ArgKind.Memory:
					fullMethodName.Append('m');
					break;
				case ArgKind.Immediate:
					fullMethodName.Append('i');
					break;
				case ArgKind.ImmediateUnsigned:
					fullMethodName.Append('u');
					break;
				case ArgKind.Label:
					fullMethodName.Append('l');
					break;
				case ArgKind.LabelUlong:
					fullMethodName.Append("lu");
					break;
				default:
					throw new ArgumentOutOfRangeException($"{renderArg.Kind}");
				}
			}

			var fullMethodNameStr = fullMethodName.ToString();
			if (IgnoredTestsPerBitness.TryGetValue(bitness, out var ignoredTests) && ignoredTests.Contains(fullMethodNameStr))
				return;
			var define = GetDefine(group);
			if (define is not null)
				writer.WriteLineNoIndent($"#if {define}");
			writer.WriteLine("[Fact]");
			writer.WriteLine($"public void {fullMethodNameStr}() {{");
			using (writer.Indent()) {
				var argValues = new List<object?>(renderArgs.Count);
				for (int i = 0; i < renderArgs.Count; i++)
					argValues.Add(null);

				if (group.Flags == OpCodeArgFlags.Pseudo) {
					Debug.Assert(group.ParentPseudoOpsKind is not null);
					GenerateTestAssemblerForOpCode(writer, bitness, bitnessFlags, @group, methodName, renderArgs, argValues, OpCodeArgFlags.Default, group.ParentPseudoOpsKind.Items[0]);
				}
				else
					GenerateOpCodeTest(writer, bitness, bitnessFlags, group, methodName, group.RootOpCodeNode, renderArgs, argValues, OpCodeArgFlags.Default);
			}
			writer.WriteLine("}");
			if (define is not null)
				writer.WriteLineNoIndent("#endif");
			writer.WriteLine(); ;
		}

		void GenerateOpCodeTest(FileWriter writer, int bitness, InstructionDefFlags1 bitnessFlags, OpCodeInfoGroup group, string methodName, OpCodeNode node, List<RenderArg> args, List<object?> argValues, OpCodeArgFlags contextFlags) {
			var opCodeInfo = node.Def;
			if (opCodeInfo is not null)
				GenerateTestAssemblerForOpCode(writer, bitness, bitnessFlags, @group, methodName, args, argValues, contextFlags, opCodeInfo);
			else {
				var selector = node.Selector;
				Debug.Assert(selector is not null);
				var argKind = selector.ArgIndex >= 0 ? args[selector.ArgIndex] : default;
				var condition = GetArgConditionForOpCodeKind(argKind, selector.Kind, selector.ArgIndex);
				var isSelectorSupportedByBitness = IsSelectorSupportedByBitness(bitness, selector.Kind, out var continueElse);
				var (contextIfFlags, contextElseFlags) = GetIfElseContextFlags(selector.Kind);
				if (isSelectorSupportedByBitness) {
					writer.WriteLine($"{{ /* if ({condition}) */");
					using (writer.Indent()) {
						foreach (var argValue in GetArgValue(bitness, selector.Kind, false, selector.ArgIndex, args)) {
							var newArgValues = new List<object?>(argValues);
							if (selector.ArgIndex >= 0)
								newArgValues[selector.ArgIndex] = argValue;
							GenerateOpCodeTest(writer, bitness, bitnessFlags, group, methodName, selector.IfTrue, args, newArgValues, contextFlags | contextIfFlags);
						}
					}
				}
				else
					writer.WriteLine($"{{ // skip ({condition}) not supported by this Assembler bitness");

				if (!selector.IfFalse.IsEmpty) {
					if (continueElse) {
						writer.Write("} /* else */ ");
						foreach (var argValue in GetArgValue(bitness, selector.Kind, true, selector.ArgIndex, args)) {
							var newArgValues = new List<object?>(argValues);
							if (selector.ArgIndex >= 0)
								newArgValues[selector.ArgIndex] = argValue;
							GenerateOpCodeTest(writer, bitness, bitnessFlags, group, methodName, selector.IfFalse, args, newArgValues, contextFlags | contextElseFlags);
						}
					}
					else
						writer.WriteLine($"}} /* else skip ({condition}) not supported by this Assembler bitness */");
				}
				else {
					writer.WriteLine("}");
					writer.WriteLine("{");
					using (writer.Indent()) {
						bool isGenerated = false;
						// Don't generate AssertInvalid for unsigned as they are already tested by signed
						if (isSelectorSupportedByBitness && selector.ArgIndex >= 0 && !group.HasImmediateUnsigned) {
							var newArg = GetInvalidArgValue(bitness, selector.Kind, selector.ArgIndex);
							if (newArg is not null) {
								// Force fake bitness support to allow to generate a throw for the last selector
								if (bitness == 64 && (group.Name == "bndcn" ||
													  group.Name == "bndmk" ||
													  group.Name == "bndcu" ||
													  group.Name == "bndcl")) {
									bitness = 32;
									bitnessFlags = InstructionDefFlags1.Bit32;
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
							if (group.HasImmediateUnsigned)
								writer.WriteLine($"// Already tested by signed version");
							else
								writer.WriteLine($"// See manual test for this case {methodName}");
						}
					}
					writer.WriteLine("}");
				}
			}
		}

		bool GenerateTestAssemblerForOpCode(FileWriter writer, int bitness, InstructionDefFlags1 bitnessFlags, OpCodeInfoGroup @group, string methodName, List<RenderArg> args, List<object?> argValues, OpCodeArgFlags contextFlags, InstructionDef def) {
			if ((def.Flags1 & bitnessFlags) == 0) {
				writer.WriteLine("{");
				using (writer.Indent())
					writer.WriteLine($"// Skipping {def.Code.RawName} - Not supported for {bitnessFlags}");

				writer.WriteLine("}");
				return false;
			}

			var assemblerArgs = new List<string>();
			var instructionCreateArgs = new List<string>();
			int forceBitness = 0;
			// Special case for movdir64b, the memory operand should match the register size
			// TODO: Ideally this should be handled in the base class
			switch (GetOrigCodeValue(def.Code)) {
			case Code.Bndmov_bndm64_bnd:
			case Code.Bndmov_bnd_bndm64:
			case Code.Bndldx_bnd_mib:
			case Code.Bndstx_mib_bnd:
				if (bitness == 16)
					forceBitness = 32;
				break;

			case Code.Movdir64b_r16_m512:
			case Code.Enqcmds_r16_m512:
			case Code.Enqcmd_r16_m512:
				forceBitness = 16;
				break;
			case Code.Movdir64b_r32_m512:
			case Code.Enqcmds_r32_m512:
			case Code.Enqcmd_r32_m512:
				forceBitness = 32;
				break;
			case Code.Movdir64b_r64_m512:
			case Code.Enqcmds_r64_m512:
			case Code.Enqcmd_r64_m512:
				forceBitness = 64;
				break;
			}

			for (var i = 0; i < argValues.Count; i++) {
				var renderArg = args[i];
				var isMemory = renderArg.Kind == ArgKind.Memory;
				var argValueForAssembler = argValues[i]?.ToString();
				var argValueForInstructionCreate = argValueForAssembler;

				if (argValueForAssembler is null) {
					var localBitness = forceBitness > 0 ? forceBitness : bitness;

					argValueForAssembler = GetDefaultArgument(localBitness, def.OpKindDefs[@group.NumberOfLeadingArgToDiscard + i], isMemory, true, i, renderArg);
					argValueForInstructionCreate = GetDefaultArgument(localBitness, def.OpKindDefs[@group.NumberOfLeadingArgToDiscard + i], isMemory, false, i, renderArg);
				}

				if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0 && i == 0) {
					argValueForAssembler += ".k1";
					argValueForInstructionCreate += ".k1";
				}

				if (renderArg.Kind == ArgKind.Memory)
					argValueForInstructionCreate += ".ToMemoryOperand(Bitness)";

				// Perform casting for unsigned
				if (IsArgKindImmediate(renderArg.Kind) && !renderArg.IsTypeSigned()) {
					if (renderArg.Type == "ulong") {
						argValueForAssembler = $"unchecked({argValueForAssembler})";
						argValueForInstructionCreate = $"unchecked({argValueForInstructionCreate})";
					}
					else {
						argValueForAssembler = $"({renderArg.Type}){argValueForAssembler}";
						argValueForInstructionCreate = $"(uint)({renderArg.Type}){argValueForInstructionCreate}";
					}
				}

				Debug.Assert(argValueForAssembler is not null);
				assemblerArgs.Add(argValueForAssembler);
				Debug.Assert(argValueForInstructionCreate is not null);
				instructionCreateArgs.Add(argValueForInstructionCreate);
			}

			var optionalOpCodeFlags = new List<string>();
			if ((contextFlags & OpCodeArgFlags.HasVex) != 0)
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferVex");
			if ((contextFlags & OpCodeArgFlags.HasEvex) != 0)
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferEvex");
			if ((contextFlags & OpCodeArgFlags.HasBroadcast) != 0)
				optionalOpCodeFlags.Add("LocalOpCodeFlags.Broadcast");
			if ((contextFlags & OpCodeArgFlags.HasBranchShort) != 0)
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferBranchShort");
			if ((contextFlags & OpCodeArgFlags.HasBranchNear) != 0)
				optionalOpCodeFlags.Add("LocalOpCodeFlags.PreferBranchNear");
			if ((def.Flags1 & InstructionDefFlags1.Fwait) != 0)
				optionalOpCodeFlags.Add("LocalOpCodeFlags.Fwait");
			if (@group.HasLabel)
				optionalOpCodeFlags.Add((group.Flags & OpCodeArgFlags.HasLabelUlong) == 0 ? "LocalOpCodeFlags.Branch" : "LocalOpCodeFlags.BranchUlong");

			if (group.Flags == OpCodeArgFlags.Pseudo)
				instructionCreateArgs.Add($"{group.PseudoOpsKindImmediateValue}");

			string beginInstruction = $"Instruction.Create(Code.{def.Code.Name(idConverter)}";
			string endInstruction = ")";
			if ((@group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0) {
				beginInstruction = $"Instruction.Create{@group.MemoName}(Bitness";
				if (@group.HasLabel && (group.Flags & OpCodeArgFlags.HasLabelUlong) == 0)
					beginInstruction = $"AssignLabel({beginInstruction}, {instructionCreateArgs[0]})";
			}
			else if (@group.HasLabel) {
				beginInstruction = (@group.Flags & OpCodeArgFlags.HasLabelUlong) == 0 ?
					$"AssignLabel(Instruction.CreateBranch(Code.{def.Code.Name(idConverter)}, {instructionCreateArgs[0]})" :
					$"Instruction.CreateBranch(Code.{def.Code.Name(idConverter)}";
			}

			if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0) {
				beginInstruction = $"ApplyK1({beginInstruction}";
				endInstruction = "))";
			}

			var assemblerArgsStr = string.Join(", ", assemblerArgs);
			var instructionCreateArgsStr = instructionCreateArgs.Count > 0 ? $", {string.Join(", ", instructionCreateArgs)}" : string.Empty;
			var optionalOpCodeFlagsStr = optionalOpCodeFlags.Count > 0 ? $", {string.Join(" | ", optionalOpCodeFlags)}" : string.Empty;

			writer.WriteLine($"TestAssembler(c => c.{methodName}({assemblerArgsStr}), {beginInstruction}{instructionCreateArgsStr}{endInstruction}{optionalOpCodeFlagsStr});");
			return true;
		}

		string GetDefaultArgument(int bitness, OpCodeOperandKindDef def, bool asMemory, bool isAssembler, int index, RenderArg arg) {
			switch (def.OperandEncoding) {
			case OperandEncoding.NearBranch:
			case OperandEncoding.Xbegin:
			case OperandEncoding.AbsNearBranch:
				if (arg.Kind == ArgKind.LabelUlong)
					return "12752";
				return isAssembler ? "CreateAndEmitLabel(c)" : "1"; // First id of label starts at 1

			case OperandEncoding.Immediate:
				return (def.ImmediateSize, def.ImmediateSignExtSize) switch {
					(4, 4) => "3",
					(8, 8) => arg.IsTypeSigned() ? "-5" : "127",
					(8, 16) => "-5",
					(8, 32) => "-9",
					(8, 64) => "-10",
					(16, 16) => "16567",
					(32, 32) => "int.MaxValue",
					(32, 64) => "int.MinValue",
					(64, 64) => "long.MinValue",
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.ImpliedConst:
				return def.ImpliedConst.ToString();

			case OperandEncoding.ImpliedRegister:
				var regDef = regDefs[(int)def.Register];
				return (regDef.GetRegisterKind() == RegisterKind.ST ? regDef.Register.RawName : regDef.Name).ToLowerInvariant();

			case OperandEncoding.RegImm:
			case OperandEncoding.RegOpCode:
			case OperandEncoding.RegModrmReg:
			case OperandEncoding.RegModrmRm:
			case OperandEncoding.RegVvvvv:
				return GetRegMemSizeInfo(def, index).regStr;

			case OperandEncoding.RegMemModrmRm:
				var (regStr, memSizeStr) = GetRegMemSizeInfo(def, index);
				if (asMemory && def.OperandEncoding == OperandEncoding.RegMemModrmRm) {
					return bitness switch {
						16 => $"__{memSizeStr}[si]",
						32 => $"__{memSizeStr}[ecx]",
						64 => $"__{memSizeStr}[rcx]",
						_ => throw new InvalidOperationException(),
					};
				}
				else
					return regStr;

			case OperandEncoding.MemModrmRm:
				if (def.SibRequired) {
					return bitness switch {
						16 => "__[ecx+edx*1]",
						32 => "__[ecx+edx*2]",
						64 => "__[rcx+rdx*4]",
						_ => throw new InvalidOperationException(),
					};
				}
				else if (def.MIB) {
					return bitness switch {
						16 => "__byte_ptr[si]",
						32 => "__byte_ptr[ecx]",
						64 => "__byte_ptr[rcx]",
						_ => throw new InvalidOperationException(),
					};
				}
				else if (def.Vsib) {
					var vsibReg = def.Register switch {
						Register.XMM0 => "xmm",
						Register.YMM0 => "ymm",
						Register.ZMM0 => "zmm",
						_ => throw new InvalidOperationException(),
					};
					return bitness switch {
						16 => $"__[esi + {vsibReg}{index + 2}]",
						32 => $"__[edx + {vsibReg}{index + 2}]",
						64 => $"__[rdx + {vsibReg}{index + 2}]",
						_ => throw new InvalidOperationException(),
					};
				}
				else {
					return bitness switch {
						16 => "__[si]",
						32 => "__[ecx]",
						64 => "__[rcx]",
						_ => throw new InvalidOperationException(),
					};
				}

			case OperandEncoding.MemOffset:
				return bitness switch {
					16 => "__[67]",
					32 => "__[12345]",
					64 => "__[123456789]",
					_ => throw new InvalidOperationException(),
				};

			case OperandEncoding.None:
			case OperandEncoding.FarBranch:
			case OperandEncoding.SegRBX:
			case OperandEncoding.SegRSI:
			case OperandEncoding.SegRDI:
			case OperandEncoding.ESRDI:
			default:
				throw new InvalidOperationException();
			}
		}

		static readonly string[] r8Values = new string[] { "dl", "bl", "ah", "ch", "dh" };
		static readonly string[] r16Values = new string[] { "dx", "bx", "sp", "bp", "si" };
		static readonly string[] r32Values = new string[] { "edx", "ebx", "esp", "ebp", "esi" };
		static readonly string[] r64Values = new string[] { "rdx", "rbx", "rsp", "rbp", "rsi" };
		static (string regStr, string memSizeStr) GetRegMemSizeInfo(OpCodeOperandKindDef def, int index) =>
			def.Register switch {
				Register.AL => (r8Values[index], "byte_ptr"),
				Register.AX => (r16Values[index], "word_ptr"),
				Register.EAX => (r32Values[index], "dword_ptr"),
				Register.RAX => (r64Values[index], "qword_ptr"),
				Register.MM0 => ($"mm{index + 2}", "qword_ptr"),
				Register.XMM0 => ($"xmm{index + 2}", "xmmword_ptr"),
				Register.YMM0 => ($"ymm{index + 2}", "ymmword_ptr"),
				Register.ZMM0 => ($"zmm{index + 2}", "zmmword_ptr"),
				Register.TMM0 => ($"tmm{index + 2}", string.Empty),
				Register.BND0 => ($"bnd{index + 2}", string.Empty),
				Register.K0 => ($"k{index + 2}", string.Empty),
				Register.ES => ("ds", string.Empty),
				Register.CR0 => ("cr2", string.Empty),
				Register.DR0 => ("dr1", string.Empty),
				Register.TR0 => ("tr1", string.Empty),
				Register.ST0 => ("st1", string.Empty),
				_ => throw new InvalidOperationException(),
			};

		void GenerateOpCodeSelector(FileWriter writer, OpCodeInfoGroup group, List<RenderArg> args) =>
			GenerateOpCodeSelector(writer, group, true, group.RootOpCodeNode, args);

		void GenerateOpCodeSelector(FileWriter writer, OpCodeInfoGroup group, bool isLeaf, OpCodeNode node, List<RenderArg> args) {
			var opCodeInfo = node.Def;
			if (opCodeInfo is not null) {
				if (isLeaf)
					writer.Write("op = ");
				writer.Write($"Code.{opCodeInfo.Code.Name(idConverter)}");
				if (isLeaf)
					writer.WriteLine(";");
			}
			else {
				var selector = node.Selector;
				Debug.Assert(selector is not null);
				var condition = GetArgConditionForOpCodeKind(selector.ArgIndex >= 0 ? args[selector.ArgIndex] : default, selector.Kind, selector.ArgIndex);
				if (selector.IsConditionInlineable) {
					writer.Write($"op = {condition} ? ");
					GenerateOpCodeSelector(writer, group, false, selector.IfTrue, args);
					writer.Write(" : ");
					GenerateOpCodeSelector(writer, group, false, selector.IfFalse, args);
					writer.WriteLine(";");
				}
				else {
					writer.WriteLine($"if ({condition}) {{");
					using (writer.Indent())
						GenerateOpCodeSelector(writer, group, true, selector.IfTrue, args);

					writer.Write("} else ");
					if (!selector.IfFalse.IsEmpty)
						GenerateOpCodeSelector(writer, group, true, selector.IfFalse, args);
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

		string GetArgConditionForOpCodeKind(RenderArg arg, OpCodeSelectorKind selectorKind, int index) {
			var regName = arg.Name;
			var otherRegName = arg.Name == "src" ? "dst" : "src";
			return selectorKind switch {
				OpCodeSelectorKind.MemOffs64_RAX => $"{otherRegName}.Value == Register.RAX && Bitness == 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.MemOffs64_EAX => $"{otherRegName}.Value == Register.EAX && Bitness == 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.MemOffs64_AX => $"{otherRegName}.Value == Register.AX && Bitness == 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.MemOffs64_AL => $"{otherRegName}.Value == Register.AL && Bitness == 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.MemOffs_RAX => $"{otherRegName}.Value == Register.RAX && Bitness < 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.MemOffs_EAX => $"{otherRegName}.Value == Register.EAX && Bitness < 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.MemOffs_AX => $"{otherRegName}.Value == Register.AX && Bitness < 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.MemOffs_AL => $"{otherRegName}.Value == Register.AL && Bitness < 64 && {regName}.IsDisplacementOnly",
				OpCodeSelectorKind.Bitness64 => "Bitness == 64",
				OpCodeSelectorKind.Bitness32 => "Bitness >= 32",
				OpCodeSelectorKind.Bitness16 => "Bitness >= 16",
				OpCodeSelectorKind.BranchShort => "PreferBranchShort",
				OpCodeSelectorKind.ImmediateByteEqual1 => $"{regName} == 1",
				OpCodeSelectorKind.ImmediateByteSigned8 => !arg.IsTypeSigned() ?
					$"{regName} <= ({arg.Type})sbyte.MaxValue || (0xFFFF_FF80 <= {regName} && {regName} <= 0xFFFF_FFFF)" :
					$"{regName} >= sbyte.MinValue && {regName} <= sbyte.MaxValue",
				OpCodeSelectorKind.ImmediateByteSigned8To16 => !arg.IsTypeSigned() ?
					$"{regName} <= ({arg.Type})sbyte.MaxValue || (0xFF80 <= {regName} && {regName} <= 0xFFFF)" :
					$"{regName} >= sbyte.MinValue && {regName} <= sbyte.MaxValue",
				OpCodeSelectorKind.ImmediateByteSigned8To32 => !arg.IsTypeSigned() ?
					$"{regName} <= ({arg.Type})sbyte.MaxValue || (0xFFFF_FF80 <= {regName} && {regName} <= 0xFFFF_FFFF)" :
					$"{regName} >= sbyte.MinValue && {regName} <= sbyte.MaxValue",
				OpCodeSelectorKind.Vex => "PreferVex",
				OpCodeSelectorKind.EvexBroadcastX => $"{regName}.IsBroadcast",
				OpCodeSelectorKind.EvexBroadcastY => $"{regName}.IsBroadcast",
				OpCodeSelectorKind.EvexBroadcastZ => $"{regName}.IsBroadcast",
				OpCodeSelectorKind.RegisterCL => $"{regName} == Register.CL",
				OpCodeSelectorKind.RegisterAL => $"{regName} == Register.AL",
				OpCodeSelectorKind.RegisterAX => $"{regName} == Register.AX",
				OpCodeSelectorKind.RegisterEAX => $"{regName} == Register.EAX",
				OpCodeSelectorKind.RegisterRAX => $"{regName} == Register.RAX",
				OpCodeSelectorKind.RegisterBND => $"{regName} == Register.IsBND()",
				OpCodeSelectorKind.RegisterES => $"{regName} == Register.ES",
				OpCodeSelectorKind.RegisterCS => $"{regName} == Register.CS",
				OpCodeSelectorKind.RegisterSS => $"{regName} == Register.SS",
				OpCodeSelectorKind.RegisterDS => $"{regName} == Register.DS",
				OpCodeSelectorKind.RegisterFS => $"{regName} == Register.FS",
				OpCodeSelectorKind.RegisterGS => $"{regName} == Register.GS",
				OpCodeSelectorKind.RegisterDX => $"{regName} == Register.DX",
				OpCodeSelectorKind.Register8 => $"{regName}.IsGPR8()",
				OpCodeSelectorKind.Register16 => $"{regName}.IsGPR16()",
				OpCodeSelectorKind.Register32 => $"{regName}.IsGPR32()",
				OpCodeSelectorKind.Register64 => $"{regName}.IsGPR64()",
				OpCodeSelectorKind.RegisterK => $"{regName}.IsK()",
				OpCodeSelectorKind.RegisterST0 => $"{regName} == Register.ST0",
				OpCodeSelectorKind.RegisterST => $"{regName}.IsST()",
				OpCodeSelectorKind.RegisterSegment => $"{regName}.IsSegmentRegister()",
				OpCodeSelectorKind.RegisterCR => $"{regName}.IsCR()",
				OpCodeSelectorKind.RegisterDR => $"{regName}.IsDR()",
				OpCodeSelectorKind.RegisterTR => $"{regName}.IsTR()",
				OpCodeSelectorKind.RegisterMM => $"{regName}.IsMM()",
				OpCodeSelectorKind.RegisterXMM => $"{regName}.IsXMM()",
				OpCodeSelectorKind.RegisterYMM => $"{regName}.IsYMM()",
				OpCodeSelectorKind.RegisterZMM => $"{regName}.IsZMM()",
				OpCodeSelectorKind.RegisterTMM => $"{regName}.IsTMM()",
				OpCodeSelectorKind.Memory8 => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Byte))}",
				OpCodeSelectorKind.Memory16 => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Word))}",
				OpCodeSelectorKind.Memory32 => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Dword))}",
				OpCodeSelectorKind.Memory48 => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Fword))}",
				OpCodeSelectorKind.Memory64 => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Qword))}",
				OpCodeSelectorKind.Memory80 => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Tbyte))}",
				OpCodeSelectorKind.MemoryMM => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Qword))}",
				OpCodeSelectorKind.MemoryXMM => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Xword))}",
				OpCodeSelectorKind.MemoryYMM => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Yword))}",
				OpCodeSelectorKind.MemoryZMM => $"{regName}.Size == {GetMemOpSizeString(nameof(MemoryOperandSize.Zword))}",
				OpCodeSelectorKind.MemoryIndex32Xmm => $"{regName}.Index.IsXMM()",
				OpCodeSelectorKind.MemoryIndex64Xmm => $"{regName}.Index.IsXMM()",
				OpCodeSelectorKind.MemoryIndex32Ymm => $"{regName}.Index.IsYMM()",
				OpCodeSelectorKind.MemoryIndex64Ymm => $"{regName}.Index.IsYMM()",
				OpCodeSelectorKind.MemoryIndex32Zmm => $"{regName}.Index.IsZMM()",
				OpCodeSelectorKind.MemoryIndex64Zmm => $"{regName}.Index.IsZMM()",
				_ => $"invalid_selector_{selectorKind}_for_arg_{regName}",
			};
		}

		string GetMemOpSizeString(string fieldName) =>
			$"{memoryOperandSizeType.Name(idConverter)}.{memoryOperandSizeType[fieldName].Name(idConverter)}";

		static string? GetInvalidArgValue(int bitness, OpCodeSelectorKind selectorKind, int argIndex) =>
			selectorKind switch {
				OpCodeSelectorKind.Memory8 or OpCodeSelectorKind.Memory16 or OpCodeSelectorKind.Memory32 or OpCodeSelectorKind.Memory48 or
				OpCodeSelectorKind.Memory80 or OpCodeSelectorKind.Memory64 =>
					bitness switch {
						16 => "__zmmword_ptr[di]",
						32 => "__zmmword_ptr[edx]",
						64 => "__zmmword_ptr[rdx]",
						_ => throw new InvalidOperationException(),
					},
				OpCodeSelectorKind.MemoryMM or OpCodeSelectorKind.MemoryXMM or OpCodeSelectorKind.MemoryYMM or OpCodeSelectorKind.MemoryZMM =>
					bitness switch {
						16 => "__byte_ptr[di]",
						32 => "__byte_ptr[edx]",
						64 => "__byte_ptr[rdx]",
						_ => throw new InvalidOperationException(),
					},
				OpCodeSelectorKind.MemoryIndex32Xmm or OpCodeSelectorKind.MemoryIndex64Xmm or OpCodeSelectorKind.MemoryIndex64Ymm or
				OpCodeSelectorKind.MemoryIndex32Ymm =>
					bitness switch {
						16 => $"__[edi + zmm{argIndex}]",
						32 => $"__[edx + zmm{argIndex}]",
						64 => $"__[rdx + zmm{argIndex}]",
						_ => throw new InvalidOperationException(),
					},
				_ => null,
			};

		static IEnumerable<string?> GetArgValue(int bitness, OpCodeSelectorKind selectorKind, bool isElseBranch, int index, List<RenderArg> args) {
			switch (selectorKind) {
			case OpCodeSelectorKind.MemOffs64_RAX:
			case OpCodeSelectorKind.MemOffs64_EAX:
			case OpCodeSelectorKind.MemOffs64_AX:
			case OpCodeSelectorKind.MemOffs64_AL:
				if (isElseBranch) {
					switch (bitness) {
					case 16: yield return index == 0 ? $"__[di]" : $"__[si]"; break;
					case 32: yield return index == 0 ? $"__[edi]" : $"__[esi]"; break;
					case 64: yield return index == 0 ? $"__[rdi]" : $"__[rsi]"; break;
					default: throw new InvalidOperationException();
					}
				}
				else
					yield return $"__[0x0123456789abcdef]";
				break;
			case OpCodeSelectorKind.MemOffs_RAX:
			case OpCodeSelectorKind.MemOffs_EAX:
			case OpCodeSelectorKind.MemOffs_AX:
			case OpCodeSelectorKind.MemOffs_AL:
				if (isElseBranch) {
					switch (bitness) {
					case 16: yield return index == 0 ? $"__[di]" : $"__[si]"; break;
					case 32: yield return index == 0 ? $"__[edi]" : $"__[esi]"; break;
					case 64: yield return index == 0 ? $"__[rdi]" : $"__[rsi]"; break;
					default: throw new InvalidOperationException();
					}
				}
				else
					yield return (bitness >= 32 ? $"__[0x01234567]" : $"__[0x01234]");
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
				if (isElseBranch)
					yield return "c.PreferBranchShort = false;";
				else
					yield return "c.PreferBranchShort = true;";
				break;
			case OpCodeSelectorKind.ImmediateByteEqual1:
				if (isElseBranch)
					yield return "2";
				else
					yield return "1";
				break;
			case OpCodeSelectorKind.ImmediateByteSigned8:
				if (isElseBranch)
					yield return null;
				else {
					var arg = args[index];
					if (arg.IsTypeSigned()) {
						yield return $"sbyte.MinValue";
						yield return $"sbyte.MaxValue";
					}
					else {
						yield return $"unchecked((uint)sbyte.MinValue)";
						yield return $"unchecked((uint)sbyte.MaxValue)";
					}
				}
				break;
			case OpCodeSelectorKind.ImmediateByteSigned8To16: {
				var arg = args[index];
				if (isElseBranch)
					yield return null;
				else {
					if (arg.IsTypeSigned()) {
						yield return $"sbyte.MinValue";
						yield return $"sbyte.MaxValue";
					}
					else {
						yield return $"unchecked((ushort)sbyte.MinValue)";
						yield return $"unchecked((ushort)sbyte.MaxValue)";
					}
				}

				break;
			}
			case OpCodeSelectorKind.ImmediateByteSigned8To32: {
				var arg = args[index];
				if (isElseBranch)
					yield return null;
				else {
					if (arg.IsTypeSigned()) {
						yield return $"sbyte.MinValue";
						yield return $"sbyte.MaxValue";
					}
					else {
						yield return $"unchecked(({arg.Type})sbyte.MinValue)";
						yield return $"unchecked(({arg.Type})sbyte.MaxValue)";
					}
				}

				break;
			}
			case OpCodeSelectorKind.Vex:
				if (isElseBranch)
					yield return "c.PreferVex = false;";
				else
					yield return "c.PreferVex = true;";
				break;
			case OpCodeSelectorKind.EvexBroadcastX:
			case OpCodeSelectorKind.EvexBroadcastY:
			case OpCodeSelectorKind.EvexBroadcastZ:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__dword_bcst[di]"; break;
					case 32: yield return "__dword_bcst[edx]"; break;
					case 64: yield return "__dword_bcst[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.RegisterCL:
				if (!isElseBranch)
					yield return $"cl";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterAL:
				if (!isElseBranch)
					yield return $"al";
				else
					yield return null;

				break;
			case OpCodeSelectorKind.RegisterAX:
				if (!isElseBranch)
					yield return $"ax";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterEAX:
				if (!isElseBranch)
					yield return $"eax";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterRAX:
				if (!isElseBranch)
					yield return $"rax";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterBND:
				if (!isElseBranch)
					yield return $"bnd0";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterES:
				if (!isElseBranch)
					yield return $"es";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterCS:
				if (!isElseBranch)
					yield return $"cs";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterSS:
				if (!isElseBranch)
					yield return $"ss";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterDS:
				if (!isElseBranch)
					yield return $"ds";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterFS:
				if (!isElseBranch)
					yield return $"fs";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterGS:
				if (!isElseBranch)
					yield return $"gs";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterDX:
				if (!isElseBranch)
					yield return $"dx";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.Register8:
				if (!isElseBranch)
					yield return $"bl";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.Register16:
				if (!isElseBranch)
					yield return $"bx";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.Register32:
				if (!isElseBranch)
					yield return $"ebx";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.Register64:
				if (!isElseBranch)
					yield return $"rbx";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterK:
				if (!isElseBranch)
					yield return $"k{index + 2}";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterST0:
				if (!isElseBranch)
					yield return $"st0";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterST:
				if (!isElseBranch)
					yield return $"st3";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterSegment:
				if (!isElseBranch)
					yield return $"fs";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterCR:
				if (!isElseBranch)
					yield return "cr3";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterDR:
				if (!isElseBranch)
					yield return "dr5";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterTR:
				if (!isElseBranch)
					yield return "tr4";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterMM:
				if (!isElseBranch)
					yield return $"mm{index + 2}";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterXMM:
				if (!isElseBranch)
					yield return $"xmm{index + 2}";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterYMM:
				if (!isElseBranch)
					yield return $"ymm{index + 2}";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterZMM:
				if (!isElseBranch)
					yield return $"zmm{index + 2}";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.RegisterTMM:
				if (!isElseBranch)
					yield return $"tmm{index + 2}";
				else
					yield return null;
				break;
			case OpCodeSelectorKind.Memory8:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__byte_ptr[di]"; break;
					case 32: yield return "__byte_ptr[edx]"; break;
					case 64: yield return "__byte_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.Memory16:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__word_ptr[di]"; break;
					case 32: yield return "__word_ptr[edx]"; break;
					case 64: yield return "__word_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.Memory32:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16:
						if (args.Count == 2 && (args[0].Kind == ArgKind.RegisterBND || args[1].Kind == ArgKind.RegisterBND))
							yield return "__dword_ptr[edi]";
						else
							yield return "__dword_ptr[di]";
						break;
					case 32: yield return "__dword_ptr[edx]"; break;
					case 64: yield return "__dword_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.Memory80:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__tword_ptr[di]"; break;
					case 32: yield return "__tword_ptr[edx]"; break;
					case 64: yield return "__tword_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.Memory48:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__fword_ptr[di]"; break;
					case 32: yield return "__fword_ptr[edx]"; break;
					case 64: yield return "__fword_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.Memory64:
			case OpCodeSelectorKind.MemoryMM:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__qword_ptr[di]"; break;
					case 32: yield return "__qword_ptr[edx]"; break;
					case 64: yield return "__qword_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.MemoryXMM:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__xmmword_ptr[di]"; break;
					case 32: yield return "__xmmword_ptr[edx]"; break;
					case 64: yield return "__xmmword_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.MemoryYMM:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__ymmword_ptr[di]"; break;
					case 32: yield return "__ymmword_ptr[edx]"; break;
					case 64: yield return "__ymmword_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.MemoryZMM:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return "__zmmword_ptr[di]"; break;
					case 32: yield return "__zmmword_ptr[edx]"; break;
					case 64: yield return "__zmmword_ptr[rdx]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Xmm:
			case OpCodeSelectorKind.MemoryIndex64Xmm:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return $"__[edi + xmm{index + 2}]"; break;
					case 32: yield return $"__[edx + xmm{index + 2}]"; break;
					case 64: yield return $"__[rdx + xmm{index + 2}]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Ymm:
			case OpCodeSelectorKind.MemoryIndex64Ymm:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return $"__[edi + ymm{index + 2}]"; break;
					case 32: yield return $"__[edx + ymm{index + 2}]"; break;
					case 64: yield return $"__[rdx + ymm{index + 2}]"; break;
					default: throw new InvalidOperationException();
					}
				}
				break;
			case OpCodeSelectorKind.MemoryIndex32Zmm:
			case OpCodeSelectorKind.MemoryIndex64Zmm:
				if (isElseBranch)
					yield return null;
				else {
					switch (bitness) {
					case 16: yield return $"__[edi + zmm{index + 2}]"; break;
					case 32: yield return $"__[edx + zmm{index + 2}]"; break;
					case 64: yield return $"__[rdx + zmm{index + 2}]"; break;
					default: throw new InvalidOperationException();
					}
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

			public bool IsTypeSigned() =>
				Type switch {
					"sbyte" or "short" or "int" or "long" => true,
					_ => false,
				};

			public int GetArgSize() =>
				Type switch {
					"byte" or "sbyte" => 1,
					"short" or "ushort" => 2,
					"int" or "uint" => 4,
					"long" or "ulong" => 8,
					_ => throw new ArgumentException($"Invalid {Type}"),
				};

			public string GetSignedTypeFromUnsigned() {
				Debug.Assert(!IsTypeSigned());
				return Type switch {
					"byte" => "sbyte",
					"ushort" => "short",
					"uint" => "int",
					"ulong" => "long",
					_ => throw new ArgumentException($"Invalid {Type}"),
				};
			}
		}
	}
}
