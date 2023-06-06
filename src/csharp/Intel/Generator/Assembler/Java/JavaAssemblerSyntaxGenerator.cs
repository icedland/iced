// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Generator.Documentation.Java;
using Generator.Enums;
using Generator.IO;
using Generator.Tables;

namespace Generator.Assembler.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaAssemblerSyntaxGenerator : AssemblerSyntaxGenerator {
		const string AllRegistersClassName = "AsmRegisters";
		readonly IdentifierConverter idConverter;
		readonly JavaDocCommentWriter docWriter;
		readonly EnumType registerType;
		readonly EnumType memoryOperandSizeType;

		static readonly List<(string, int, string[], string, string)> declareDataList = new() {
			("db", 1, new[] { "byte" }, "createDeclareByte", "testAssemblerDeclareByte"),
			("dw", 2, new[] { "short" }, "createDeclareWord", "testAssemblerDeclareWord"),
			("dd", 4, new[] { "int", "float" }, "createDeclareDword", "testAssemblerDeclareDword"),
			("dq", 8, new[] { "long", "double" }, "createDeclareQword", "testAssemblerDeclareQword"),
		};

		protected override bool SupportsUnsigned => false;

		public JavaAssemblerSyntaxGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = JavaIdentifierConverter.Create();
			docWriter = new JavaDocCommentWriter(idConverter);
			registerType = genTypes[TypeIds.Register];
			memoryOperandSizeType = genTypes[TypeIds.CodeAsmMemoryOperandSize];
		}

		static string GetRegisterClassName(RegisterKind kind) =>
			"AsmRegister" + GetRegisterInfo(kind).suffix;
		static (string suffix, string isName) GetRegisterInfo(RegisterKind kind) =>
			kind switch {
				RegisterKind.None => throw new InvalidOperationException(),
				RegisterKind.GPR8 => ("8", "GPR8"),
				RegisterKind.GPR16 => ("16", "GPR16"),
				RegisterKind.GPR32 => ("32", "GPR32"),
				RegisterKind.GPR64 => ("64", "GPR64"),
				RegisterKind.IP => ("IP", "IP"),
				RegisterKind.Segment => ("Segment", "SegmentRegister"),
				RegisterKind.ST => ("ST", "ST"),
				RegisterKind.CR => ("CR", "CR"),
				RegisterKind.DR => ("DR", "DR"),
				RegisterKind.TR => ("TR", "TR"),
				RegisterKind.BND => ("BND", "BND"),
				RegisterKind.K => ("K", "K"),
				RegisterKind.MM => ("MM", "MM"),
				RegisterKind.XMM => ("XMM", "XMM"),
				RegisterKind.YMM => ("YMM", "YMM"),
				RegisterKind.ZMM => ("ZMM", "ZMM"),
				RegisterKind.TMM => ("TMM", "TMM"),
				_ => throw new InvalidOperationException(),
			};

		protected override void GenerateRegisters((RegisterKind kind, RegisterDef[] regs)[] regGroups) {
			var infos = Tables.Java.RegisterUtils.GetRegisterInfos(AllRegistersClassName, regGroups);
			var toDefs = regGroups.ToDictionary(a => a.kind, a => a.regs);
			var allNames = infos.Select(a => a.name).ToArray();
			foreach (var (className, tmp) in infos) {
				var kinds = tmp;
				bool isAllRegs = kinds.Length == 0;
				if (isAllRegs)
					kinds = regGroups.Select(a => a.kind).ToArray();

				var filename = JavaConstants.GetFilename(genTypes, JavaConstants.CodeAssemblerPackage, className + ".java");
				if (isAllRegs) {
					new FileUpdater(TargetLanguage.Java, "Registers", filename).Generate(writer => {
						WriteRegisters(writer, toDefs, kinds, isAllRegs);
					});
				}
				else {
					using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
						writer.WriteFileHeader();

						writer.WriteLine($"package {JavaConstants.CodeAssemblerPackage};");
						if (isAllRegs) {
							writer.WriteLine();
							writer.WriteLine($"import {JavaConstants.IcedPackage}.ICRegisters;");
						}
						writer.WriteLine();
						writer.WriteLine("/**");
						writer.WriteLine(" * Registers passed to {@link CodeAssembler} methods");
						writer.WriteLine(" *");
						foreach (var otherName in allNames) {
							if (otherName != className)
								writer.WriteLine($" * @see {otherName}");
						}
						writer.WriteLine(" */");
						writer.WriteLine($"public final class {className} {{");
						using (writer.Indent()) {
							writer.WriteLine($"private {className}() {{");
							writer.WriteLine("}");
							writer.WriteLine();
							WriteRegisters(writer, toDefs, kinds, isAllRegs);
						}
						writer.WriteLine("}");
					}
				}
			}
		}

		static void WriteRegisters(FileWriter writer, Dictionary<RegisterKind, RegisterDef[]> toDefs, RegisterKind[] kinds, bool isAllRegs) {
			foreach (var kind in kinds) {
				if (!toDefs.TryGetValue(kind, out var defs)) {
					if (kind != RegisterKind.IP)
						throw new InvalidOperationException();
					continue;
				}
				foreach (var def in defs) {
					var asmRegName = def.GetAsmRegisterName();
					var registerTypeName = GetRegisterClassName(def.GetRegisterKind());
					if (isAllRegs)
						writer.WriteLine($"public static final {registerTypeName} {asmRegName} = new {registerTypeName}(ICRegisters.{asmRegName});");
					else
						writer.WriteLine($"public static final {registerTypeName} {asmRegName} = {AllRegistersClassName}.{asmRegName};");
				}
			}
		}

		protected override void GenerateRegisterClasses(RegisterClassInfo[] infos) {
			var registerTypeName = registerType.Name(idConverter);
			var memOpNoneName = idConverter.ToDeclTypeAndValue(memoryOperandSizeType[nameof(MemoryOperandSize.None)]);
			foreach (var reg in infos) {
				var isName = GetRegisterInfo(reg.Kind).isName;
				var className = GetRegisterClassName(reg.Kind);
				var filename = JavaConstants.GetFilename(genTypes, JavaConstants.CodeAssemblerPackage, className + ".java");
				using (var writer = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filename))) {
					writer.WriteFileHeader();
					writer.WriteLine($"package {JavaConstants.CodeAssemblerPackage};");
					writer.WriteLine();
					writer.WriteLine($"import {JavaConstants.IcedPackage}.ICRegister;");
					writer.WriteLine($"import {JavaConstants.IcedPackage}.{registerTypeName};");
					writer.WriteLine();
					writer.WriteLine("/**");
					writer.WriteLine(" * An assembler register used with {@link CodeAssembler}.");
					writer.WriteLine(" */");
					writer.WriteLine($"public final class {className} {{");
					using (writer.Indent()) {
						writer.WriteLine("/**");
						writer.WriteLine(" * Creates a new instance.");
						writer.WriteLine(" *");
						writer.WriteLine($" * @param register Register");
						writer.WriteLine(" */");
						writer.WriteLine($"public {className}(ICRegister register) {{");
						using (writer.Indent()) {
							writer.WriteLine($"if (!{registerTypeName}.is{isName}(register.get()))");
							using (writer.Indent())
								writer.WriteLine($"throw new IllegalArgumentException(\"Invalid register value. Must be a {isName} register\");");
							writer.WriteLine("this.register = register;");
							if (reg.NeedsState)
								writer.WriteLine("this.flags = AsmOperandFlags.NONE;");
						}
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("private final ICRegister register;");
						writer.WriteLine();
						writer.WriteLine("/**");
						writer.WriteLine($" * The register value");
						writer.WriteLine(" */");
						writer.WriteLine("public ICRegister get() {");
						using (writer.Indent())
							writer.WriteLine("return register;");
						writer.WriteLine("}");
						writer.WriteLine();
						writer.WriteLine("/**");
						writer.WriteLine($" * The register value (a {{@link {JavaConstants.IcedPackage}.{registerTypeName}}} enum variant)");
						writer.WriteLine(" */");
						writer.WriteLine("public int getRegister() {");
						using (writer.Indent())
							writer.WriteLine("return register.get();");
						writer.WriteLine("}");
						if (reg.NeedsState) {
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Creates a new instance.");
							writer.WriteLine(" *");
							writer.WriteLine($" * @param register Register");
							writer.WriteLine(" * @param flags Flags (an {@link AsmOperandFlags} flags value)");
							writer.WriteLine(" */");
							writer.WriteLine($"public {className}(ICRegister register, int flags) {{");
							using (writer.Indent()) {
								writer.WriteLine($"if (!{registerTypeName}.is{isName}(register.get()))");
								using (writer.Indent())
									writer.WriteLine($"throw new IllegalArgumentException(\"Invalid register value. Must be a {isName} register\");");
								writer.WriteLine("this.register = register;");
								writer.WriteLine("this.flags = flags;");
							}
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("final int flags;");
							for (int j = 1; j <= 7; j++) {
								writer.WriteLine();
								writer.WriteLine("/**");
								writer.WriteLine($" * Apply op mask register {{@code K{j}}}.");
								writer.WriteLine(" */");
								writer.WriteLine($"public {className} k{j}() {{");
								using (writer.Indent())
									writer.WriteLine($"return new {className}(register, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K{j});");
								writer.WriteLine("}");
							}
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Apply op mask zeroing.");
							writer.WriteLine(" */");
							writer.WriteLine($"public {className} z() {{");
							using (writer.Indent())
								writer.WriteLine($"return new {className}(register, flags | AsmOperandFlags.ZEROING);");
							writer.WriteLine("}");
							if (reg.HasSaeOrEr) {
								writer.WriteLine();
								writer.WriteLine("/**");
								writer.WriteLine(" * Suppress all exceptions");
								writer.WriteLine(" */");
								writer.WriteLine($"public {className} sae() {{");
								using (writer.Indent())
									writer.WriteLine($"return new {className}(register, flags | AsmOperandFlags.SUPPRESS_ALL_EXCEPTIONS);");
								writer.WriteLine("}");
								writer.WriteLine();
								writer.WriteLine("/**");
								writer.WriteLine(" * Round to nearest (even)");
								writer.WriteLine(" */");
								writer.WriteLine($"public {className} rn_sae() {{");
								using (writer.Indent())
									writer.WriteLine($"return new {className}(register, (flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_TO_NEAREST);");
								writer.WriteLine("}");
								writer.WriteLine();
								writer.WriteLine("/**");
								writer.WriteLine(" * Round down (toward -inf)");
								writer.WriteLine(" */");
								writer.WriteLine($"public {className} rd_sae() {{");
								using (writer.Indent())
									writer.WriteLine($"return new {className}(register, (flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_DOWN);");
								writer.WriteLine("}");
								writer.WriteLine();
								writer.WriteLine("/**");
								writer.WriteLine(" * Round up (toward +inf)");
								writer.WriteLine(" */");
								writer.WriteLine($"public {className} ru_sae() {{");
								using (writer.Indent())
									writer.WriteLine($"return new {className}(register, (flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_UP);");
								writer.WriteLine("}");
								writer.WriteLine();
								writer.WriteLine("/**");
								writer.WriteLine(" * Round toward zero (truncate)");
								writer.WriteLine(" */");
								writer.WriteLine($"public {className} rz_sae() {{");
								using (writer.Indent())
									writer.WriteLine($"return new {className}(register, (flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_TOWARD_ZERO);");
								writer.WriteLine("}");
							}
						}
						if (reg.IsGPR16_32_64) {
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Adds a register (base) to another register (index) and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param index The index register");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand add({className} index) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, get(), index.get(), 1, 0, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							if (reg.IsGPR32_64) {
								foreach (var mm in new[] { "XMM", "YMM", "ZMM" }) {
									writer.WriteLine();
									writer.WriteLine("/**");
									writer.WriteLine(" * Adds a register (base) to another register (index) and returns a memory operand.");
									writer.WriteLine(" *");
									writer.WriteLine(" * @param index The index register");
									writer.WriteLine(" */");
									writer.WriteLine($"public AsmMemoryOperand add(AsmRegister{mm} index) {{");
									using (writer.Indent())
										writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, get(), index.get(), 1, 0, AsmOperandFlags.NONE);");
									writer.WriteLine("}");
								}
							}
						}
						if (reg.IsGPR16_32_64) {
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Adds this register (base) to a displacement and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param displacement The displacement");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand add(long displacement) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, get(), ICRegister.NONE, 1, displacement, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Subtracts a displacement from this register (base) and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param displacement The displacement");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand sub(long displacement) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, get(), ICRegister.NONE, 1, -displacement, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							if (reg.IsGPR32_64) {
								writer.WriteLine();
								writer.WriteLine("/**");
								writer.WriteLine(" * Multiplies an index register by a scale and returns a memory operand.");
								writer.WriteLine(" *");
								writer.WriteLine(" * @param scale The scale (1, 2, 4 or 8)");
								writer.WriteLine(" */");
								writer.WriteLine($"public AsmMemoryOperand scale(int scale) {{");
								using (writer.Indent())
									writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, ICRegister.NONE, get(), scale, 0, AsmOperandFlags.NONE);");
								writer.WriteLine("}");
							}
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Adds this register (base or index) to a memory operand and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param mem Memory operand");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand add(AsmMemoryOperand mem) {{");
							using (writer.Indent())
								writer.WriteLine($"return mem.add(this);");
							writer.WriteLine("}");
						}
						if (reg.IsVector) {
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Adds this register (index) to a displacement and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param displacement The displacement");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand add(long displacement) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, ICRegister.NONE, get(), 1, displacement, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Subtracts a displacement from this register (index) and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param displacement The displacement");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand sub(long displacement) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, ICRegister.NONE, get(), 1, -displacement, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Multiplies an index register by a scale and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param scale The scale (1, 2, 4 or 8)");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand scale(int scale) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, ICRegister.NONE, get(), scale, 0, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Adds this register (index) to a base register and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param base Base register");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand add(AsmRegister32 base) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, base.get(), get(), 1, 0, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Adds this register (index) to a base register and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param base Base register");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand add(AsmRegister64 base) {{");
							using (writer.Indent())
								writer.WriteLine($"return new AsmMemoryOperand({memOpNoneName}, ICRegister.NONE, base.get(), get(), 1, 0, AsmOperandFlags.NONE);");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/**");
							writer.WriteLine(" * Adds this register (index) to a memory operand and returns a memory operand.");
							writer.WriteLine(" *");
							writer.WriteLine(" * @param mem Memory operand");
							writer.WriteLine(" */");
							writer.WriteLine($"public AsmMemoryOperand add(AsmMemoryOperand mem) {{");
							using (writer.Indent())
								writer.WriteLine($"return mem.add(this);");
							writer.WriteLine("}");
						}
						writer.WriteLine();
						writer.WriteLine("/** Checks if {@code obj} equals this object */");
						writer.WriteLine("@Override");
						writer.WriteLine($"public boolean equals(Object obj) {{");
						using (writer.Indent()) {
							writer.WriteLine("if (obj == null || getClass() != obj.getClass())");
							using (writer.Indent())
								writer.WriteLine("return false;");
							writer.WriteLine($"{className} other = ({className})obj;");
							if (reg.NeedsState)
								writer.WriteLine("return register.get() == other.register.get() && flags == other.flags;");
							else
								writer.WriteLine("return register.get() == other.register.get();");
						}
						writer.WriteLine("}");
						if (reg.NeedsState) {
							writer.WriteLine();
							writer.WriteLine("/** Gets the hash code */");
							writer.WriteLine("@Override");
							writer.WriteLine("public int hashCode() {");
							using (writer.Indent())
								writer.WriteLine("return (register.get() * 397) ^ flags;");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/** toString() */");
							writer.WriteLine("@Override");
							writer.WriteLine("public String toString() {");
							using (writer.Indent())
								writer.WriteLine("return String.format(\"Register %d, flags: %d\", getRegister(), flags);");
							writer.WriteLine("}");
						}
						else {
							writer.WriteLine();
							writer.WriteLine("/** Gets the hash code */");
							writer.WriteLine("@Override");
							writer.WriteLine("public int hashCode() {");
							using (writer.Indent())
								writer.WriteLine("return register.get();");
							writer.WriteLine("}");
							writer.WriteLine();
							writer.WriteLine("/** toString() */");
							writer.WriteLine("@Override");
							writer.WriteLine("public String toString() {");
							using (writer.Indent())
								writer.WriteLine("return String.format(\"Register %d\", getRegister());");
							writer.WriteLine("}");
						}
					}
					writer.WriteLine("}");
				}
			}
		}

		static string GetName(MemorySizeFuncInfo fn) {
			var name = fn.Kind switch {
				MemorySizeFnKind.Ptr => "mem_ptr",
				_ => fn.Name.Replace(' ', '_'),
			};
			return name;
		}

		protected override void GenerateMemorySizeFunctions(MemorySizeFuncInfo[] infos) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.CodeAssemblerPackage, AllRegistersClassName + ".java");
			new FileUpdater(TargetLanguage.Java, "MemFns", filename).Generate(writer => {
				WriteMemFuncs(writer, infos);
			});
		}

		void WriteMemFuncs(FileWriter writer, MemorySizeFuncInfo[] infos) {
			string GPR16 = GetRegisterClassName(RegisterKind.GPR16);
			string GPR32 = GetRegisterClassName(RegisterKind.GPR32);
			string GPR64 = GetRegisterClassName(RegisterKind.GPR64);
			string XMM = GetRegisterClassName(RegisterKind.XMM);
			string YMM = GetRegisterClassName(RegisterKind.YMM);
			string ZMM = GetRegisterClassName(RegisterKind.ZMM);
			var parameters = new(string? baseType, string? indexType, bool scale, bool displ)[] {
				// 16-bit addressing
				(GPR16, null, false, false),	// [base]
				(GPR16, GPR16, false, false),	// [base+index]
				(GPR16, GPR16, false, true),	// [base+index+displ]
				(GPR16, null, false, true),		// [base+displ]

				// 32-bit addressing
				(GPR32, null, false, false),	// [base]
				(GPR32, GPR32, false, false),	// [base+index]
				(GPR32, GPR32, true, false),	// [base+index*scale]
				(GPR32, GPR32, true, true),		// [base+index*scale+displ]
				(GPR32, null , false, true),	// [base+displ]
				// Too similar to [base+displ], i.e., int vs long
				// (null , GPR32, true, false),	// [index*scale]
				(null , GPR32, true, true),		// [index*scale+displ]

				(GPR32, XMM, false, false),		// [base+xmm]
				(GPR32, XMM, true, false),		// [base+xmm*scale]
				(GPR32, XMM, true, true),		// [base+xmm*scale+displ]

				(GPR32, YMM, false, false),		// [base+ymm]
				(GPR32, YMM, true, false),		// [base+ymm*scale]
				(GPR32, YMM, true, true),		// [base+ymm*scale+displ]

				(GPR32, ZMM, false, false),		// [base+zmm]
				(GPR32, ZMM, true, false),		// [base+zmm*scale]
				(GPR32, ZMM, true, true),		// [base+zmm*scale+displ]

				// 64-bit addressing
				(GPR64, null, false, false),	// [base]
				(GPR64, GPR64, false, false),	// [base+index]
				(GPR64, GPR64, true, false),	// [base+index*scale]
				(GPR64, GPR64, true, true),		// [base+index*scale+displ]
				(GPR64, null , false, true),	// [base+displ]
				// Too similar to [base+displ], i.e., int vs long
				// (null , GPR64, true, false),	// [index*scale]
				(null , GPR64, true, true),		// [index*scale+displ]

				(GPR64, XMM, false, false),		// [base+xmm]
				(GPR64, XMM, true, false),		// [base+xmm*scale]
				(GPR64, XMM, true, true),		// [base+xmm*scale+displ]

				(GPR64, YMM, false, false),		// [base+ymm]
				(GPR64, YMM, true, false),		// [base+ymm*scale]
				(GPR64, YMM, true, true),		// [base+ymm*scale+displ]

				(GPR64, ZMM, false, false),		// [base+zmm]
				(GPR64, ZMM, true, false),		// [base+zmm*scale]
				(GPR64, ZMM, true, true),		// [base+zmm*scale+displ]

				// 32/64-bit addressing
				(null , XMM, true, false),		// [index*scale]
				(null , XMM, true, true),		// [index*scale+displ]

				(null , YMM, true, false),		// [index*scale]
				(null , YMM, true, true),		// [index*scale+displ]

				(null , ZMM, true, false),		// [index*scale]
				(null , ZMM, true, true),		// [index*scale+displ]

				// All modes
				(null , null, false, true),		// [displ]
			};
			bool addEmptyLine = false;
			var paramDefs = new List<(string type, string name, string docs)>();
			foreach (var info in infos) {
				var doc = info.GetMethodDocs("Gets", s => $"{{@code {s}}}");
				var name = GetName(info);

				var enumValueStr = idConverter.ToDeclTypeAndValue(memoryOperandSizeType[info.Size.ToString()]);
				var flags = info.IsBroadcast ? "AsmOperandFlags.BROADCAST" : "AsmOperandFlags.NONE";

				if (addEmptyLine)
					writer.WriteLine();
				addEmptyLine = true;

				writer.WriteLine("/**");
				writer.WriteLine(" * Gets a memory operand referencing a label");
				writer.WriteLine(" *");
				writer.WriteLine(" * @param label The label");
				writer.WriteLine(" */");
				writer.WriteLine($"public static AsmMemoryOperand {name}(CodeLabel label) {{");
				using (writer.Indent())
					writer.WriteLine($"return new AsmMemoryOperand({enumValueStr}, ICRegister.NONE, ICRegisters.rip, ICRegister.NONE, 1, label.id, AsmOperandFlags.NONE);");
				writer.WriteLine("}");

				foreach (var paramInfo in parameters) {
					paramDefs.Clear();
					const string baseName = "base";
					const string indexName = "index";
					const string scaleName = "scale";
					const string displName = "displacement";
					const string noRegister = "ICRegister.NONE";
					string @base, index, scale, displacement;
					if (paramInfo.baseType is not null) {
						paramDefs.Add((paramInfo.baseType, baseName, "Base register"));
						@base = baseName + ".get()";
					}
					else
						@base = noRegister;
					if (paramInfo.indexType is not null) {
						paramDefs.Add((paramInfo.indexType, indexName, "Index register"));
						index = indexName + ".get()";
					}
					else
						index = noRegister;
					if (paramInfo.scale) {
						paramDefs.Add(("int", scaleName, "Scale (1, 2, 4 or 8)"));
						scale = scaleName;
					}
					else
						scale = "1";
					if (paramInfo.displ) {
						paramDefs.Add(("long", displName, "Displacement"));
						displacement = displName;
					}
					else
						displacement = "0";

					writer.WriteLine();
					writer.WriteLine("/**");
					writer.WriteLine($" * {doc}");
					writer.WriteLine(" *");
					foreach (var paramDef in paramDefs)
						writer.WriteLine($" * @param {paramDef.name} {paramDef.docs}");
					writer.WriteLine(" */");
					writer.Write($"public static AsmMemoryOperand {name}(");
					bool addComma = false;
					foreach (var paramDef in paramDefs) {
						if (addComma)
							writer.Write(", ");
						addComma = true;
						writer.Write(paramDef.type);
						writer.Write(" ");
						writer.Write(paramDef.name);
					}
					writer.WriteLine(") {");
					using (writer.Indent()) {
						writer.WriteLine($"return new AsmMemoryOperand({enumValueStr}, ICRegister.NONE, {@base}, {index}, {scale}, {displacement}, {flags});");
					}
					writer.WriteLine("}");
				}
			}
		}

		protected override void Generate(Dictionary<GroupKey, OpCodeInfoGroup> map, OpCodeInfoGroup[] groups) {
			GenerateCode(groups);
			GenerateTests(groups);
		}

		void GenerateCode(OpCodeInfoGroup[] groups) {
			var filename = JavaConstants.GetFilename(genTypes, JavaConstants.CodeAssemblerPackage, "CodeAssembler.java");
			new FileUpdater(TargetLanguage.Java, "Code", filename).Generate(writer => {
				bool methodSep = false;
				foreach (var group in groups) {
					if (methodSep)
						writer.WriteLine();
					methodSep = true;
					var renderArgs = GetRenderArgs(group);
					var methodName = idConverter.Method(group.Name);
					GenerateAssemblerCode(writer, methodName, group, renderArgs);
				}

				GenerateDeclareDataCode(writer);
			});
		}

		void GenerateDeclareDataCode(FileWriter writer) {
			foreach (var (name, size, types, methodName, _) in declareDataList) {
				int maxSize = 16;
				int argCount = maxSize / size;

				for (var typeIndex = 0; typeIndex < types.Length; typeIndex++) {
					var type = types[typeIndex];
					// So the user doesn't have to cast byte/short literals, eg. (byte)123, use int types.
					// An exception is thrown at runtime if the value doesn't fit in 8 or 16 bits.
					var paramType = type == "byte" || type == "short" ? "int" : type;
					for (int i = 1; i <= argCount; i++) {
						writer.WriteLine();
						docWriter.WriteSummary(writer, $"Creates a #(c:{name})# asm directive with type #(c:{type})#", "", null);
						writer.Write($"public void {name}(");
						for (int j = 0; j < i; j++) {
							if (j > 0) writer.Write(", ");
							writer.Write($"{paramType} imm{j}");
						}

						writer.WriteLine(") {");
						using (writer.Indent()) {
							writer.Write($"addInstruction(Instruction.{methodName}(");
							for (int j = 0; j < i; j++) {
								if (j > 0) writer.Write(", ");
								switch (type) {
								case "float":
									writer.Write($"Float.floatToRawIntBits(imm{j})");
									break;
								case "double":
									writer.Write($"Double.doubleToRawLongBits(imm{j})");
									break;
								case "byte":
								case "short":
								case "int":
								case "long":
									writer.Write($"imm{j}");
									break;
								default:
									throw new InvalidOperationException();
								}
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

		static bool IsVPrefixInstruction(OpCodeInfoGroup group) {
			foreach (var def in group.Defs) {
				switch (def.Encoding) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					break;
				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
				case EncodingKind.MVEX:
					// These instructions have a 'v' mnemonic prefix
					return true;
				default:
					throw new InvalidOperationException();
				}
			}
			return false;
		}

		static string GetRealName(OpCodeInfoGroup group) {
			var name = group.Name;
			if (IsVPrefixInstruction(group) && name.StartsWith("v", StringComparison.Ordinal))
				name = name.Substring(1);
			if (name == string.Empty)
				throw new InvalidOperationException();
			return name;
		}

		static string GetGroupId(OpCodeInfoGroup group) {
			var name = GetRealName(group);
			return name.Substring(0, 1).ToUpperInvariant();
		}

		// Too many constants in the class file so split them up into multiple files...
		void GenerateAssemblerTests(int bitness, OpCodeInfoGroup[] groups) {
			var dict = new Dictionary<String, List<OpCodeInfoGroup>>(StringComparer.Ordinal);
			foreach (var group in groups) {
				var key = GetGroupId(group);
				if (!dict.TryGetValue(key, out var list))
					dict.Add(key, list = new());
				list.Add(group);
			}
			bool generateDeclData = bitness == 64;
			foreach (var kv in dict.OrderBy(a => a.Key, StringComparer.Ordinal)) {
				var id = kv.Key;
				var smallGroups = kv.Value.ToArray();
				GenerateAssemblerTests(bitness, id, smallGroups, generateDeclData);
				generateDeclData = false;
			}
		}

		void GenerateAssemblerTests(int bitness, string id, OpCodeInfoGroup[] groups, bool generateDeclData) {
			string testName = $"CodeAssembler{bitness}Gen{id}Tests";

			var filenameTests = JavaConstants.GetTestFilename(genTypes, JavaConstants.CodeAssemblerPackage, testName + ".java");
			using (var writerTests = new FileWriter(TargetLanguage.Java, FileUtils.OpenWrite(filenameTests))) {
				writerTests.WriteFileHeader();
				writerTests.WriteLine($"package {JavaConstants.CodeAssemblerPackage};");
				writerTests.WriteLine();
				writerTests.WriteLine("import org.junit.jupiter.api.Test;");
				writerTests.WriteLine();
				writerTests.WriteLine("import com.github.icedland.iced.x86.*;");
				writerTests.WriteLine("import static com.github.icedland.iced.x86.asm.AsmRegisters.*;");
				writerTests.WriteLine();

				writerTests.WriteLine("@SuppressWarnings(\"cast\")");
				writerTests.WriteLine($"final class {testName} extends CodeAssemblerTestsBase {{");
				using (writerTests.Indent()) {
					writerTests.WriteLine($"{testName}() {{");
					using (writerTests.Indent())
						writerTests.WriteLine($"super({bitness});");
					writerTests.WriteLine("}");
					writerTests.WriteLine();

					foreach (var group in groups) {
						if (group.Name == "xbegin")
							continue; // Implemented manually
						if (!IsBitnessSupported(bitness, group.AllDefFlags))
							continue;

						var renderArgs = GetRenderArgs(group);
						RenderTests(bitness, writerTests, group, renderArgs);
					}

					if (generateDeclData)
						GenerateDeclareDataTests(writerTests);
				}

				writerTests.WriteLine("}");
			}
		}

		void GenerateDeclareDataTests(FileWriter writer) {
			foreach (var (name, size, types, _, testMethodName) in declareDataList) {
				int maxSize = 16;
				int argCount = maxSize / size;

				bool addEmptyLine = false;
				for (var typeIndex = 0; typeIndex < types.Length; typeIndex++) {
					var type = types[typeIndex];
					for (int i = 1; i <= argCount; i++) {
						if (addEmptyLine)
							writer.WriteLine();
						addEmptyLine = true;
						docWriter.WriteSummary(writer, $"Creates a {name} asm directive with the type {type}.", "", null);
						writer.WriteLine("@Test");
						writer.WriteLine($"void {testMethodName}_{name}_{type}_{i}() {{");
						using (writer.Indent()) {
								writer.Write($"{testMethodName}(c -> c.{name}(");
							for (int j = 0; j < i; j++) {
								if (j > 0)
									writer.Write(", ");
								writer.Write($"({type}){j + 1}");
							}
							writer.Write($"), new {type}[] {{");
							for (int j = 0; j < i; j++) {
								if (j > 0)
									writer.Write(", ");
								writer.Write($"({type}){j + 1}");
							}
							writer.WriteLine("});");
						}

						writer.WriteLine("}");
					}
				}
			}
		}

		static RenderArg[] GetRenderArgs(OpCodeInfoGroup group) {
			var renderArgs = new RenderArg[group.Signature.ArgCount];
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
					argType = "AsmRegister8";
					break;
				case ArgKind.Register16:
					argType = "AsmRegister16";
					break;
				case ArgKind.Register32:
					argType = "AsmRegister32";
					break;
				case ArgKind.Register64:
					argType = "AsmRegister64";
					break;
				case ArgKind.RegisterMm:
					argType = "AsmRegisterMM";
					break;
				case ArgKind.RegisterXmm:
					argType = "AsmRegisterXMM";
					break;
				case ArgKind.RegisterYmm:
					argType = "AsmRegisterYMM";
					break;
				case ArgKind.RegisterZmm:
					argType = "AsmRegisterZMM";
					break;
				case ArgKind.RegisterTmm:
					argType = "AsmRegisterTMM";
					break;
				case ArgKind.RegisterK:
					argType = "AsmRegisterK";
					break;
				case ArgKind.RegisterCr:
					argType = "AsmRegisterCR";
					break;
				case ArgKind.RegisterTr:
					argType = "AsmRegisterTR";
					break;
				case ArgKind.RegisterDr:
					argType = "AsmRegisterDR";
					break;
				case ArgKind.RegisterSt:
					argType = "AsmRegisterST";
					break;
				case ArgKind.RegisterBnd:
					argType = "AsmRegisterBND";
					break;
				case ArgKind.RegisterSegment:
					argType = "AsmRegisterSegment";
					break;

				case ArgKind.Label:
					argType = "CodeLabel";
					break;

				case ArgKind.LabelU64:
					argType = "long";
					break;

				case ArgKind.Memory:
					argType = "AsmMemoryOperand";
					break;

				case ArgKind.Immediate:
					argName = $"imm{(immArg == 0 ? "" : immArg.ToString(CultureInfo.InvariantCulture))}";
					immArg++;
					argType = maxArgSize switch {
						// Use int so they don't need to cast to byte/short, eg. (byte)123, then check at runtime that it fits in 8 or 16 bits.
						1 => "int",
						2 => "int",
						4 => "int",
						8 => "long",
						_ => throw new InvalidOperationException(),
					};
					break;

				case ArgKind.ImmediateUnsigned:
					throw new InvalidOperationException();

				default:
					throw new ArgumentOutOfRangeException($"{argKind}");
				}

				renderArgs[i] = new RenderArg(argName, argType, argKind, maxArgSize);
			}
			return renderArgs;
		}

		void GenerateAssemblerCode(FileWriter writer, string methodName, OpCodeInfoGroup group, RenderArg[] renderArgs) {
			// Write documentation
			var methodDoc = new StringBuilder();
			methodDoc.Append($"#(c:{group.Name.ToUpperInvariant()})# instruction");
			foreach (var def in group.GetDefsAndParentDefs()) {
				if (def.Code.Documentation.GetComment(TargetLanguage.Java) is string comment) {
					methodDoc.Append("#(h:)#");
					methodDoc.Append("#(p:)#");
					methodDoc.Append(comment);
				}
			}

			docWriter.WriteSummary(writer, methodDoc.ToString(), "", null);

			writer.Write($"public void {methodName}(");
			for (var i = 0; i < renderArgs.Length; i++) {
				var renderArg = renderArgs[i];
				if (i > 0)
					writer.Write(", ");
				writer.Write($"{renderArg.Type} {renderArg.Name}");
			}

			static void WriteArg(FileWriter writer, string argExpr, ArgKind kind, int maxArgSize) {
				if (kind == ArgKind.Label) {
					writer.Write(argExpr);
					writer.Write(".id");
				}
				else if (kind == ArgKind.Memory) {
					writer.Write(argExpr);
					writer.Write(".toMemoryOperand(getBitness())");
				}
				else if (IsRegister(kind)) {
					writer.Write(argExpr);
					writer.Write(".get()");
				}
				else
					writer.Write(argExpr);
			}

			writer.WriteLine(") {");
			using (writer.Indent()) {
				if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0) {
					writer.Write($"addInstruction(Instruction.create{group.MnemonicName}(getBitness()");
					for (var i = 0; i < renderArgs.Length; i++) {
						var renderArg = renderArgs[i];
						writer.Write(", ");
						WriteArg(writer, renderArg.Name, renderArg.Kind, renderArg.MaxArgSize);
					}
					writer.WriteLine("));");
				}
				else if (group.ParentPseudoOpsKind is not null) {
					writer.Write($"{group.ParentPseudoOpsKind.Name}(");
					for (var i = 0; i < renderArgs.Length; i++) {
						var renderArg = renderArgs[i];
						if (i > 0)
							writer.Write(", ");
						writer.Write(renderArg.Name);
					}
					writer.Write(", ");
					writer.Write($"{group.PseudoOpsKindImmediateValue}");
					writer.WriteLine(");");
				}
				else {
					string codeExpr;
					if (group.RootOpCodeNode.Def is InstructionDef def)
						codeExpr = idConverter.ToDeclTypeAndValue(def.Code);
					else {
						codeExpr = "code";
						writer.WriteLine($"int {codeExpr};");
						GenerateOpCodeSelector(writer, group, renderArgs);
					}

					if (group.HasLabel)
						writer.Write("addInstruction(Instruction.createBranch(");
					else
						writer.Write("addInstruction(Instruction.create(");
					writer.Write(codeExpr);

					for (var i = 0; i < renderArgs.Length; i++) {
						var renderArg = renderArgs[i];
						writer.Write(", ");

						var argExpr = renderArg.Name;

						if (renderArg.Kind == ArgKind.ImmediateUnsigned)
							throw new InvalidOperationException();

						WriteArg(writer, argExpr, renderArg.Kind, renderArg.MaxArgSize);
					}

					writer.Write(")");

					var stateArgsList = new List<string>();
					foreach (var index in GetStateArgIndexes(group))
						stateArgsList.Add($"{renderArgs[index].Name}.flags");
					if (stateArgsList.Count > 0) {
						var s = string.Join(" | ", stateArgsList);
						writer.Write(", " + s);
					}

					writer.WriteLine(");");
				}
			}

			writer.WriteLine("}");
		}

		void RenderTests(int bitness, FileWriter writer, OpCodeInfoGroup group, RenderArg[] renderArgs) {
			var fullMethodName = new StringBuilder();
			fullMethodName.Append(idConverter.Method(group.Name));
			foreach (var renderArg in renderArgs) {
				fullMethodName.Append('_');
				fullMethodName.Append(GetTestMethodArgName(renderArg.Kind));
			}

			var fullMethodNameStr = fullMethodName.ToString();
			if (ignoredTestsPerBitness.TryGetValue(bitness, out var ignoredTests) && ignoredTests.Contains(fullMethodNameStr))
				return;
			writer.WriteLine("@Test");
			writer.WriteLine($"void {fullMethodNameStr}() {{");
			using (writer.Indent()) {
				var args = new TestArgValues(renderArgs.Length);

				if (group.ParentPseudoOpsKind is not null)
					GenerateTestAssemblerForOpCode(writer, bitness, group, args, OpCodeArgFlags.None, group.ParentPseudoOpsKind.Defs[0]);
				else
					GenerateOpCodeTest(writer, bitness, group, group.RootOpCodeNode, renderArgs, args, OpCodeArgFlags.None);
			}
			writer.WriteLine("}");
			writer.WriteLine();
		}

		void GenerateOpCodeTest(FileWriter writer, int bitness, OpCodeInfoGroup group, OpCodeNode node, RenderArg[] renderArgs,
			TestArgValues args, OpCodeArgFlags contextFlags) {
			if (node.Def is InstructionDef def)
				GenerateTestAssemblerForOpCode(writer, bitness, group, args, contextFlags, def);
			else if (node.Selector is OpCodeSelector selector) {
				var maxArgSize = selector.ArgIndex >= 0 ? group.MaxArgSizes[selector.ArgIndex] : 0;
				var argKind = selector.ArgIndex >= 0 ? renderArgs[selector.ArgIndex] : default;
				var condition = GetArgConditionForOpCodeKind(argKind, selector.Kind);
				var isSelectorSupportedByBitness = IsSelectorSupportedByBitness(bitness, selector.Kind, out var continueElse);
				var (contextIfFlags, contextElseFlags) = GetIfElseContextFlags(selector.Kind);
				if (isSelectorSupportedByBitness) {
					writer.WriteLine($"{{ /* if ({condition}) */");
					using (writer.Indent()) {
						foreach (var argValue in GetArgValue(selector.Kind, false, selector.ArgIndex, group.Signature, maxArgSize * 8)) {
							var oldValue = args.Set(selector.ArgIndex, argValue);
							GenerateOpCodeTest(writer, bitness, group, selector.IfTrue, renderArgs, args, contextFlags | contextIfFlags);
							args.Restore(selector.ArgIndex, oldValue);
						}
					}
				}
				else
					writer.WriteLine($"{{ // skip ({condition}) not supported by this CodeAssembler bitness");

				if (!selector.IfFalse.IsEmpty) {
					if (continueElse) {
						writer.Write("} /* else */ ");
						foreach (var argValue in GetArgValue(selector.Kind, true, selector.ArgIndex, group.Signature, maxArgSize * 8)) {
							var oldValue = args.Set(selector.ArgIndex, argValue);
							GenerateOpCodeTest(writer, bitness, group, selector.IfFalse, renderArgs, args, contextFlags | contextElseFlags);
							args.Restore(selector.ArgIndex, oldValue);
						}
					}
					else
						writer.WriteLine($"}} /* else skip !({condition}) not supported by this CodeAssembler bitness */");
				}
				else {
					writer.WriteLine("}");
					if (isSelectorSupportedByBitness && selector.ArgIndex >= 0) {
						var newArg = GetInvalidArgValue(selector.Kind, selector.ArgIndex);
						if (newArg is not null) {
							writer.WriteLine("{");
							using (writer.Indent()) {
								int testBitness = GetInvalidTestBitness(bitness, group);
								writer.WriteLine("assertInvalid(() -> {");
								using (writer.Indent()) {
									var oldValue = args.Set(selector.ArgIndex, newArg);
									GenerateOpCodeTest(writer, testBitness, group, selector.IfTrue, renderArgs, args, contextFlags | contextIfFlags);
									args.Restore(selector.ArgIndex, oldValue);
								}
								writer.WriteLine("});");
							}
							writer.WriteLine("}");
						}
					}
				}
			}
			else
				throw new InvalidOperationException();
		}

		bool GenerateTestAssemblerForOpCode(FileWriter writer, int bitness, OpCodeInfoGroup group, TestArgValues args,
			OpCodeArgFlags contextFlags, InstructionDef def) {
			if (!IsBitnessSupported(bitness, def.Flags1)) {
				writer.WriteLine($"// Skipping {def.Code.Name(idConverter)} - Not supported by current bitness");
				return false;
			}

			var withFns = new List<(string pre, string post)>();
			var asmArgs = new List<string>();
			var withArgs = new List<string>();
			int argBitness = GetArgBitness(bitness, def);
			if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0)
				withArgs.Add(bitness.ToString(CultureInfo.InvariantCulture));
			else
				withArgs.Add(idConverter.ToDeclTypeAndValue(def.Code));
			for (var i = 0; i < args.Args.Count; i++) {
				var argKind = group.Signature.GetArgKind(i);
				var asmArg = args.GetArgValue(argBitness, i)?.AsmStr;
				var withArg = args.GetArgValue(argBitness, i)?.WithStr;

				if (asmArg is null || withArg is null) {
					var argValue = GetDefaultArgument(def.OpKindDefs[group.NumberOfLeadingArgsToDiscard + i], i, argKind, group.MaxArgSizes[i] * 8);
					asmArg = argValue.Get(argBitness).AsmStr;
					withArg = argValue.Get(argBitness).WithStr;
				}

				if ((def.Flags1 & InstructionDefFlags1.OpMaskRegister) != 0 && i == 0) {
					asmArg += ".k1()";
					var opMask = idConverter.ToDeclTypeAndValue(GetRegisterDef(Register.K1).Register);
					withFns.Add(("applyK(", $", {opMask})"));
				}

				asmArgs.Add(asmArg);
				withArgs.Add(withArg);
			}
			if (group.ParentPseudoOpsKind is not null)
				withArgs.Add($"{group.PseudoOpsKindImmediateValue}");
			if (group.HasLabel && (group.Flags & OpCodeArgFlags.HasLabelUlong) == 0)
				withFns.Add(("assignLabel(", $", {withArgs[1]})"));

			string withFnName;
			if ((group.Flags & OpCodeArgFlags.HasSpecialInstructionEncoding) != 0)
				withFnName = $"create{group.MnemonicName}";
			else if (group.HasLabel)
				withFnName = "createBranch";
			else
				withFnName = "create";
			var asmName = idConverter.Method(group.Name);
			var asmArgsStr = string.Join(", ", asmArgs);
			var instrFlags = GetInstrTestFlags(def, group, contextFlags);
			var decoderOptions = GetDecoderOptions(bitness, def);
			var testInstrFlagsStr = instrFlags.Count > 0 ?
				$", {string.Join(" | ", instrFlags.Select(x => idConverter.ToDeclTypeAndValue(x)))}" :
				decoderOptions.Count != 0 ?
				$", {idConverter.ToDeclTypeAndValue(testInstrFlags[nameof(TestInstrFlags.None)])}" :
				string.Empty;
			string decoderOptionsStr;
			if (decoderOptions.Count != 0) {
				var options = string.Join(" | ", decoderOptions.Select(x => JavaConstants.DecoderPackage + "." + idConverter.ToDeclTypeAndValue(x)));
				decoderOptionsStr = $", {options}";
			}
			else
				decoderOptionsStr = string.Empty;

			var withArgsStr = string.Join(", ", withArgs);
			var withFnsPreStr = string.Join(string.Empty, ((IEnumerable<(string pre, string post)>)withFns).Reverse().Select(x => x.pre));
			var withFnsPostStr = string.Join(string.Empty, withFns.Select(x => x.post));
			writer.WriteLine($"testAssembler(c -> c.{asmName}({asmArgsStr}), {withFnsPreStr}Instruction.{withFnName}({withArgsStr}){withFnsPostStr}{testInstrFlagsStr}{decoderOptionsStr});");
			return true;
		}

		void GenerateOpCodeSelector(FileWriter writer, OpCodeInfoGroup group, RenderArg[] args) =>
			GenerateOpCodeSelector(writer, group, true, group.RootOpCodeNode, args);

		void GenerateOpCodeSelector(FileWriter writer, OpCodeInfoGroup group, bool isLeaf, OpCodeNode node, RenderArg[] args) {
			if (node.Def is InstructionDef def) {
				if (isLeaf)
					writer.Write("code = ");
				writer.Write(idConverter.ToDeclTypeAndValue(def.Code));
				if (isLeaf)
					writer.WriteLine(";");
			}
			else if (node.Selector is OpCodeSelector selector) {
				var condition = GetArgConditionForOpCodeKind(selector.ArgIndex >= 0 ? args[selector.ArgIndex] : default, selector.Kind);
				if (selector.IsConditionInlineable) {
					writer.Write($"code = {condition} ? ");
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
							writer.Write($"throw noOpCodeFoundFor(Mnemonic.{group.MnemonicName.ToUpperInvariant()}");
							for (var i = 0; i < args.Length; i++) {
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
			else
				throw new InvalidOperationException();
		}

		string GetArgConditionForOpCodeKind(RenderArg arg, OpCodeSelectorKind selectorKind) {
			var argName = arg.Name;
			var otherArgName = arg.Name == "src" ? "dst" : "src";
			return selectorKind switch {
				OpCodeSelectorKind.MemOffs64_RAX => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.RAX))} && getBitness() == 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.MemOffs64_EAX => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.EAX))} && getBitness() == 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.MemOffs64_AX => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.AX))} && getBitness() == 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.MemOffs64_AL => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.AL))} && getBitness() == 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.MemOffs_RAX => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.RAX))} && getBitness() < 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.MemOffs_EAX => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.EAX))} && getBitness() < 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.MemOffs_AX => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.AX))} && getBitness() < 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.MemOffs_AL => $"{otherArgName}.getRegister() == {GetRegisterString(nameof(Register.AL))} && getBitness() < 64 && {argName}.isDisplacementOnly()",
				OpCodeSelectorKind.Bitness64 => "getBitness() == 64",
				OpCodeSelectorKind.Bitness32 => "getBitness() >= 32",
				OpCodeSelectorKind.Bitness16 => "getBitness() >= 16",
				OpCodeSelectorKind.ShortBranch => "getPreferShortBranch()",
				OpCodeSelectorKind.ImmediateByteEqual1 => $"{argName} == 1",
				OpCodeSelectorKind.ImmediateByteSigned8To32 or OpCodeSelectorKind.ImmediateByteSigned8To64 => arg.Kind == ArgKind.ImmediateUnsigned ?
					throw new InvalidOperationException() :
					$"{argName} >= -0x80 && {argName} <= 0x7F",
				OpCodeSelectorKind.ImmediateByteSigned8To16 => arg.Kind == ArgKind.ImmediateUnsigned ?
					throw new InvalidOperationException() :
					$"{argName} >= -0x80 && {argName} <= 0x7F",
				OpCodeSelectorKind.Vex => "getInstructionPreferVex()",
				OpCodeSelectorKind.EvexBroadcastX or OpCodeSelectorKind.EvexBroadcastY or OpCodeSelectorKind.EvexBroadcastZ =>
					$"{argName}.isBroadcast()",
				OpCodeSelectorKind.RegisterCL => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.CL))}",
				OpCodeSelectorKind.RegisterAL => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.AL))}",
				OpCodeSelectorKind.RegisterAX => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.AX))}",
				OpCodeSelectorKind.RegisterEAX => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.EAX))}",
				OpCodeSelectorKind.RegisterRAX => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.RAX))}",
				OpCodeSelectorKind.RegisterBND => $"Register.isBND({argName}.getRegister())",
				OpCodeSelectorKind.RegisterES => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.ES))}",
				OpCodeSelectorKind.RegisterCS => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.CS))}",
				OpCodeSelectorKind.RegisterSS => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.SS))}",
				OpCodeSelectorKind.RegisterDS => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.DS))}",
				OpCodeSelectorKind.RegisterFS => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.FS))}",
				OpCodeSelectorKind.RegisterGS => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.GS))}",
				OpCodeSelectorKind.RegisterDX => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.DX))}",
				OpCodeSelectorKind.Register8 => $"Register.isGPR8({argName}.getRegister())",
				OpCodeSelectorKind.Register16 => $"Register.isGPR16({argName}.getRegister())",
				OpCodeSelectorKind.Register32 => $"Register.isGPR32({argName}.getRegister())",
				OpCodeSelectorKind.Register64 => $"Register.isGPR64({argName}.getRegister())",
				OpCodeSelectorKind.RegisterK => $"Register.isK({argName}.getRegister())",
				OpCodeSelectorKind.RegisterST0 => $"{argName}.getRegister() == {GetRegisterString(nameof(Register.ST0))}",
				OpCodeSelectorKind.RegisterST => $"Register.isST({argName}.getRegister())",
				OpCodeSelectorKind.RegisterSegment => $"Register.isSegmentRegister({argName}.getRegister())",
				OpCodeSelectorKind.RegisterCR => $"Register.isCR({argName}.getRegister())",
				OpCodeSelectorKind.RegisterDR => $"Register.isDR({argName}.getRegister())",
				OpCodeSelectorKind.RegisterTR => $"Register.isTR({argName}.getRegister())",
				OpCodeSelectorKind.RegisterMM => $"Register.isMM({argName}.getRegister())",
				OpCodeSelectorKind.RegisterXMM => $"Register.isXMM({argName}.getRegister())",
				OpCodeSelectorKind.RegisterYMM => $"Register.isYMM({argName}.getRegister())",
				OpCodeSelectorKind.RegisterZMM => $"Register.isZMM({argName}.getRegister())",
				OpCodeSelectorKind.RegisterTMM => $"Register.isTMM({argName}.getRegister())",
				OpCodeSelectorKind.Memory8 => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Byte))}",
				OpCodeSelectorKind.Memory16 => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Word))}",
				OpCodeSelectorKind.Memory32 => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Dword))}",
				OpCodeSelectorKind.Memory48 => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Fword))}",
				OpCodeSelectorKind.Memory64 => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Qword))}",
				OpCodeSelectorKind.Memory80 => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Tbyte))}",
				OpCodeSelectorKind.MemoryMM => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Qword))}",
				OpCodeSelectorKind.MemoryXMM => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Xword))}",
				OpCodeSelectorKind.MemoryYMM => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Yword))}",
				OpCodeSelectorKind.MemoryZMM => $"{argName}.size == {GetMemOpSizeString(nameof(MemoryOperandSize.Zword))}",
				OpCodeSelectorKind.MemoryIndex32Xmm or OpCodeSelectorKind.MemoryIndex64Xmm => $"Register.isXMM({argName}.index.get())",
				OpCodeSelectorKind.MemoryIndex32Ymm or OpCodeSelectorKind.MemoryIndex64Ymm => $"Register.isYMM({argName}.index.get())",
				OpCodeSelectorKind.MemoryIndex32Zmm or OpCodeSelectorKind.MemoryIndex64Zmm => $"Register.isZMM({argName}.index.get())",
				_ => throw new InvalidOperationException(),
			};
		}

		string GetRegisterString(string fieldName) =>
			idConverter.ToDeclTypeAndValue(registerType[fieldName]);

		string GetMemOpSizeString(string fieldName) =>
			idConverter.ToDeclTypeAndValue(memoryOperandSizeType[fieldName]);

		protected override TestArgValueBitness MemToTestArgValue(MemorySizeFuncInfo size, int bitness, ulong address) {
			var memName = GetName(size);
			var asmStr = $"{memName}(0x{address:X}L)";
			var withStr = $"new MemoryOperand(0x{address:X}L, {bitness / 8})";
			return new TestArgValueBitness(asmStr, withStr);
		}

		protected override TestArgValueBitness MemToTestArgValue(MemorySizeFuncInfo size, Register @base, Register index, int scale, int displ) {
			if (scale != 1 && scale != 2 && scale != 4 && scale != 8)
				throw new InvalidOperationException();
			var sb = new StringBuilder();
			sb.Append(GetName(size));
			bool isNeg = displ < 0;
			if (isNeg)
				displ = -displ;
			sb.Append("(0x");
			sb.Append(displ.ToString("X", CultureInfo.InvariantCulture));
			sb.Append("L)");
			if (@base != Register.None)
				sb.Append($".base({GetRegisterDef(@base).GetAsmRegisterName()})");
			if (index != Register.None) {
				sb.Append($".index({GetRegisterDef(index).GetAsmRegisterName()})");
				if (scale > 1)
					sb.Append($".scale({scale})");
			}
			var asmStr = sb.ToString();

			string baseStr;
			string indexStr;
			if (@base == Register.None)
				baseStr = "ICRegister.NONE";
			else
				baseStr = "ICRegisters." + GetRegisterDef(@base).GetAsmRegisterName();
			if (index == Register.None)
				indexStr = "ICRegister.NONE";
			else
				indexStr = "ICRegisters." + GetRegisterDef(index).GetAsmRegisterName();
			var displStr = displ < 0 ?
				"-0x" + (-displ).ToString("X", CultureInfo.InvariantCulture) :
				"0x" + displ.ToString("X", CultureInfo.InvariantCulture);
			var displSize = displ == 0 ? "0" : "1";
			var isBcstStr = size.IsBroadcast ? "true" : "false";
			var regNoneStr = "ICRegister.NONE";
			var withStr = $"new MemoryOperand({baseStr}, {indexStr}, {scale}, {displStr}L, {displSize}, {isBcstStr}, {regNoneStr})";

			return new(asmStr, withStr);
		}

		protected override TestArgValueBitness RegToTestArgValue(Register register) {
			var regDef = GetRegisterDef(register);
			var asmReg = regDef.GetAsmRegisterName();
			var withReg = "ICRegisters." + regDef.GetAsmRegisterName();
			return new(asmReg, withReg);
		}

		static string AddCastOrSuffix(string number, string castOrSuffix) {
			if (castOrSuffix.StartsWith("("))
				return castOrSuffix + number;
			return number + castOrSuffix;
		}

		protected override TestArgValueBitness UnsignedImmToTestArgValue(ulong immediate, int encImmSizeBits, int immSizeBits, int argSizeBits) {
			throw new InvalidOperationException();
		}

		protected override TestArgValueBitness SignedImmToTestArgValue(long immediate, int encImmSizeBits, int immSizeBits, int argSizeBits) {
			if (encImmSizeBits > immSizeBits)
				throw new InvalidOperationException();
			bool isNeg = immediate < 0;
			if (isNeg)
				immediate = -immediate;
			string numStr;
			if ((ulong)immediate <= 9)
				numStr = immediate.ToString(CultureInfo.InvariantCulture);
			else
				numStr = "0x" + immediate.ToString("X", CultureInfo.InvariantCulture);
			if (isNeg)
				numStr = "-" + numStr;

			if (encImmSizeBits == 64 && immSizeBits == 64)
				numStr += "L";
			return new(numStr);
		}

		protected override TestArgValueBitness LabelToTestArgValue() => new("createAndEmitLabel(c)", "FIRST_LABEL_ID");

		readonly struct RenderArg {
			public RenderArg(string name, string type, ArgKind kind, int maxArgSize) {
				Name = name;
				Type = type;
				Kind = kind;
				MaxArgSize = maxArgSize;
			}

			public readonly string Name;
			public readonly string Type;
			public readonly ArgKind Kind;
			public readonly int MaxArgSize;
		}
	}
}
