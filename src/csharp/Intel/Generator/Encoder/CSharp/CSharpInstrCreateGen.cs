// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;
using Generator.Documentation.CSharp;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Encoder.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpInstrCreateGen : InstrCreateGen {
		readonly IdentifierConverter idConverter;
		readonly CSharpDocCommentWriter docWriter;

		public CSharpInstrCreateGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(idConverter);
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.CSharp, "Create", CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, "Instruction.Create.cs"));

		void WriteDocs(FileWriter writer, CreateMethod method) {
			const string typeName = "Instruction";
			docWriter.BeginWrite(writer);
			docWriter.WriteLine(writer, "<summary>");
			foreach (var doc in method.Docs)
				docWriter.WriteDocLine(writer, doc, typeName);
			docWriter.WriteLine(writer, "</summary>");
			for (int i = 0; i < method.Args.Count; i++) {
				var arg = method.Args[i];
				docWriter.Write($"<param name=\"{idConverter.Argument(arg.Name)}\">");
				docWriter.WriteDoc(writer, arg.Doc, typeName);
				docWriter.WriteLine(writer, "</param>");
			}
			docWriter.EndWrite(writer);
		}

		void WriteMethodDeclArgs(FileWriter writer, CreateMethod method) {
			bool comma = false;
			foreach (var arg in method.Args) {
				if (comma)
					writer.Write(", ");
				comma = true;
				switch (arg.Type) {
				case MethodArgType.Code:
					writer.Write(codeType.Name(idConverter));
					break;
				case MethodArgType.Register:
					writer.Write(genTypes[TypeIds.Register].Name(idConverter));
					break;
				case MethodArgType.RepPrefixKind:
					writer.Write(genTypes[TypeIds.RepPrefixKind].Name(idConverter));
					break;
				case MethodArgType.Memory:
					writer.Write("in MemoryOperand");
					break;
				case MethodArgType.UInt8:
					writer.Write("byte");
					break;
				case MethodArgType.UInt16:
					writer.Write("ushort");
					break;
				case MethodArgType.PreferredInt32:
				case MethodArgType.Int32:
				case MethodArgType.ArrayIndex:
				case MethodArgType.ArrayLength:
					writer.Write("int");
					break;
				case MethodArgType.UInt32:
					writer.Write("uint");
					break;
				case MethodArgType.Int64:
					writer.Write("long");
					break;
				case MethodArgType.UInt64:
					writer.Write("ulong");
					break;
				case MethodArgType.ByteArray:
					writer.Write("byte[]");
					break;
				case MethodArgType.WordArray:
					writer.Write("ushort[]");
					break;
				case MethodArgType.DwordArray:
					writer.Write("uint[]");
					break;
				case MethodArgType.QwordArray:
					writer.Write("ulong[]");
					break;
				case MethodArgType.ByteSlice:
					writer.Write("ReadOnlySpan<byte>");
					break;
				case MethodArgType.WordSlice:
					writer.Write("ReadOnlySpan<ushort>");
					break;
				case MethodArgType.DwordSlice:
					writer.Write("ReadOnlySpan<uint>");
					break;
				case MethodArgType.QwordSlice:
					writer.Write("ReadOnlySpan<ulong>");
					break;
				case MethodArgType.BytePtr:
					writer.Write("byte*");
					break;
				case MethodArgType.WordPtr:
					writer.Write("ushort*");
					break;
				case MethodArgType.DwordPtr:
					writer.Write("uint*");
					break;
				case MethodArgType.QwordPtr:
					writer.Write("ulong*");
					break;
				default:
					throw new InvalidOperationException();
				}
				writer.Write(" ");
				writer.Write(idConverter.Argument(arg.Name));
				switch (arg.DefaultValue) {
				case EnumValue enumValue:
					writer.Write($" = {idConverter.ToDeclTypeAndValue(enumValue)}");
					break;
				case null:
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		void WriteInitializeInstruction(FileWriter writer, CreateMethod method) {
			writer.WriteLine("Instruction instruction = default;");
			var args = method.Args;
			if (args.Count == 0 || args[0].Type != MethodArgType.Code)
				throw new InvalidOperationException();
			var codeName = idConverter.Argument(args[0].Name);
			writer.WriteLine($"instruction.Code = {codeName};");
		}

		void WriteInitializeInstruction(FileWriter writer, EnumValue code) {
			writer.WriteLine("Instruction instruction = default;");
			writer.WriteLine($"instruction.Code = {idConverter.ToDeclTypeAndValue(code)};");
		}

		static void WriteMethodFooter(FileWriter writer, int args) {
			writer.WriteLine();
			writer.WriteLine($"Debug.Assert(instruction.OpCount == {args});");
			writer.WriteLine("return instruction;");
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group, int id) {
			WriteDocs(writer, method);
			writer.Write("public static Instruction Create(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				var args = method.Args;
				var registerStr = idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.Register)]);
				var memoryStr = idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.Memory)]);
				bool multipleInts = args.Where(a => a.Type == MethodArgType.Int32 || a.Type == MethodArgType.UInt32).Count() > 1;
				for (int i = 1; i < args.Count; i++) {
					int op = i - 1;
					var arg = args[i];
					writer.WriteLine();
					switch (arg.Type) {
					case MethodArgType.Register:
						writer.WriteLine($"Static.Assert({registerStr} == 0 ? 0 : -1);");
						writer.WriteLine($"//instruction.Op{op}Kind = {registerStr};");
						writer.WriteLine($"instruction.Op{op}Register = {idConverter.Argument(arg.Name)};");
						break;

					case MethodArgType.Memory:
						writer.WriteLine($"instruction.Op{op}Kind = {memoryStr};");
						writer.WriteLine($"InitMemoryOperand(ref instruction, {idConverter.Argument(arg.Name)});");
						break;

					case MethodArgType.Int32:
					case MethodArgType.UInt32:
					case MethodArgType.Int64:
					case MethodArgType.UInt64:
						var methodName = arg.Type == MethodArgType.Int32 || arg.Type == MethodArgType.Int64 ? "InitializeSignedImmediate" : "InitializeUnsignedImmediate";
						writer.WriteLine($"{methodName}(ref instruction, {op}, {idConverter.Argument(arg.Name)});");
						break;

					case MethodArgType.Code:
					case MethodArgType.RepPrefixKind:
					case MethodArgType.UInt8:
					case MethodArgType.UInt16:
					case MethodArgType.PreferredInt32:
					case MethodArgType.ArrayIndex:
					case MethodArgType.ArrayLength:
					case MethodArgType.ByteArray:
					case MethodArgType.WordArray:
					case MethodArgType.DwordArray:
					case MethodArgType.QwordArray:
					case MethodArgType.ByteSlice:
					case MethodArgType.WordSlice:
					case MethodArgType.DwordSlice:
					case MethodArgType.QwordSlice:
					case MethodArgType.BytePtr:
					case MethodArgType.WordPtr:
					case MethodArgType.DwordPtr:
					case MethodArgType.QwordPtr:
					default:
						throw new InvalidOperationException();
					}
				}
				WriteMethodFooter(writer, args.Count - 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateBranch(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 2)
				throw new InvalidOperationException();
			WriteDocs(writer, method);
			writer.Write("public static Instruction CreateBranch(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				writer.WriteLine();
				writer.WriteLine($"instruction.Op0Kind = GetNearBranchOpKind({idConverter.Argument(method.Args[0].Name)}, 0);");
				writer.WriteLine($"instruction.NearBranch64 = {idConverter.Argument(method.Args[1].Name)};");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 3)
				throw new InvalidOperationException();
			WriteDocs(writer, method);
			writer.Write("public static Instruction CreateBranch(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				writer.WriteLine();
				writer.WriteLine($"instruction.Op0Kind = GetFarBranchOpKind({idConverter.Argument(method.Args[0].Name)}, 0);");
				writer.WriteLine($"instruction.FarBranchSelector = {idConverter.Argument(method.Args[1].Name)};");
				writer.WriteLine($"instruction.FarBranch32 = {idConverter.Argument(method.Args[2].Name)};");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 2)
				throw new InvalidOperationException();
			WriteDocs(writer, method);
			writer.Write("public static Instruction CreateXbegin(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.WriteLine("Instruction instruction = default;");
				var bitnessName = idConverter.Argument(method.Args[0].Name);
				writer.WriteLine($"switch ({bitnessName}) {{");
				writer.WriteLine($"case 16:");
				using (writer.Indent()) {
					writer.WriteLine($"instruction.Code = {idConverter.ToDeclTypeAndValue(codeType[nameof(Code.Xbegin_rel16)])};");
					writer.WriteLine($"instruction.Op0Kind = {idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch32)])};");
					writer.WriteLine($"instruction.NearBranch32 = (uint){idConverter.Argument(method.Args[1].Name)};");
					writer.WriteLine($"break;");
				}
				writer.WriteLine();
				writer.WriteLine($"case 32:");
				using (writer.Indent()) {
					writer.WriteLine($"instruction.Code = {idConverter.ToDeclTypeAndValue(codeType[nameof(Code.Xbegin_rel32)])};");
					writer.WriteLine($"instruction.Op0Kind = {idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch32)])};");
					writer.WriteLine($"instruction.NearBranch32 = (uint){idConverter.Argument(method.Args[1].Name)};");
					writer.WriteLine($"break;");
				}
				writer.WriteLine();
				writer.WriteLine($"case 64:");
				using (writer.Indent()) {
					writer.WriteLine($"instruction.Code = {idConverter.ToDeclTypeAndValue(codeType[nameof(Code.Xbegin_rel32)])};");
					writer.WriteLine($"instruction.Op0Kind = {idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch64)])};");
					writer.WriteLine($"instruction.NearBranch64 = {idConverter.Argument(method.Args[1].Name)};");
					writer.WriteLine($"break;");
				}
				writer.WriteLine();
				writer.WriteLine($"default:");
				using (writer.Indent())
					writer.WriteLine($"throw new ArgumentOutOfRangeException(nameof({bitnessName}));");
				writer.WriteLine($"}}");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		static void WriteComma(FileWriter writer) => writer.Write(", ");
		void Write(FileWriter writer, EnumValue value) => writer.Write(idConverter.ToDeclTypeAndValue(value));
		void Write(FileWriter writer, MethodArg arg) => writer.Write(idConverter.Argument(arg.Name));

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") =>");
			using (writer.Indent()) {
				writer.Write("CreateString_Reg_SegRSI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count != 3)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, method.Args[1]);
					WriteComma(writer);
					Write(writer, method.Args[2]);
					break;
				case StringMethodKind.Rep:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, genTypes[TypeIds.Register][nameof(Register.None)]);
					WriteComma(writer);
					Write(writer, genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.Repe)]);
					break;
				case StringMethodKind.Repe:
				case StringMethodKind.Repne:
				default:
					throw new InvalidOperationException();
				}
				writer.WriteLine(");");
			}
		}

		protected override void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") =>");
			using (writer.Indent()) {
				writer.Write("CreateString_Reg_ESRDI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count != 2)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, method.Args[1]);
					break;
				case StringMethodKind.Repe:
				case StringMethodKind.Repne:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, kind == StringMethodKind.Repe ? genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.Repe)] : genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.Repne)]);
					break;
				case StringMethodKind.Rep:
				default:
					throw new InvalidOperationException();
				}
				writer.WriteLine(");");
			}
		}

		protected override void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") =>");
			using (writer.Indent()) {
				writer.Write("CreateString_ESRDI_Reg(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count != 2)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, method.Args[1]);
					break;
				case StringMethodKind.Rep:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.Repe)]);
					break;
				case StringMethodKind.Repe:
				case StringMethodKind.Repne:
				default:
					throw new InvalidOperationException();
				}
				writer.WriteLine(");");
			}
		}

		protected override void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") =>");
			using (writer.Indent()) {
				writer.Write("CreateString_SegRSI_ESRDI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count != 3)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, method.Args[1]);
					WriteComma(writer);
					Write(writer, method.Args[2]);
					break;
				case StringMethodKind.Repe:
				case StringMethodKind.Repne:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, genTypes[TypeIds.Register][nameof(Register.None)]);
					WriteComma(writer);
					Write(writer, kind == StringMethodKind.Repe ? genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.Repe)] : genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.Repne)]);
					break;
				case StringMethodKind.Rep:
				default:
					throw new InvalidOperationException();
				}
				writer.WriteLine(");");
			}
		}

		protected override void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") =>");
			using (writer.Indent()) {
				writer.Write("CreateString_ESRDI_SegRSI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count != 3)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, method.Args[1]);
					WriteComma(writer);
					Write(writer, method.Args[2]);
					break;
				case StringMethodKind.Rep:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method.Args[0]);
					WriteComma(writer);
					Write(writer, genTypes[TypeIds.Register][nameof(Register.None)]);
					WriteComma(writer);
					Write(writer, genTypes[TypeIds.RepPrefixKind][nameof(RepPrefixKind.Repe)]);
					break;
				case StringMethodKind.Repe:
				case StringMethodKind.Repne:
				default:
					throw new InvalidOperationException();
				}
				writer.WriteLine(");");
			}
		}

		protected override void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") =>");
			using (writer.Indent()) {
				writer.Write("CreateMaskmov(");
				if (method.Args.Count != 4)
					throw new InvalidOperationException();
				Write(writer, code);
				WriteComma(writer);
				Write(writer, method.Args[0]);
				WriteComma(writer);
				Write(writer, method.Args[1]);
				WriteComma(writer);
				Write(writer, method.Args[2]);
				WriteComma(writer);
				Write(writer, method.Args[3]);
				writer.WriteLine(");");
			}
		}

		protected override void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind) {
			EnumValue code;
			string setValueName;
			string methodName;
			switch (kind) {
			case DeclareDataKind.Byte:
				code = codeType[nameof(Code.DeclareByte)];
				setValueName = "SetDeclareByteValue";
				methodName = "CreateDeclareByte";
				break;

			case DeclareDataKind.Word:
				code = codeType[nameof(Code.DeclareWord)];
				setValueName = "SetDeclareWordValue";
				methodName = "CreateDeclareWord";
				break;

			case DeclareDataKind.Dword:
				code = codeType[nameof(Code.DeclareDword)];
				setValueName = "SetDeclareDwordValue";
				methodName = "CreateDeclareDword";
				break;

			case DeclareDataKind.Qword:
				code = codeType[nameof(Code.DeclareQword)];
				setValueName = "SetDeclareQwordValue";
				methodName = "CreateDeclareQword";
				break;

			default:
				throw new InvalidOperationException();
			}

			writer.WriteLine();
			WriteDocs(writer, method);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, code);
				writer.WriteLine($"instruction.InternalDeclareDataCount = {method.Args.Count};");
				writer.WriteLine();
				for (int i = 0; i < method.Args.Count; i++)
					writer.WriteLine($"instruction.{setValueName}({i}, {idConverter.Argument(method.Args[i].Name)});");
				WriteMethodFooter(writer, 0);
			}
			writer.WriteLine("}");
		}

		void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, string methodName, string calledMethodName) {
			writer.WriteLine();
			WriteDocs(writer, method);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				var dataName = idConverter.Argument(method.Args[0].Name);
				writer.WriteLine($"if ({dataName} is null)");
				using (writer.Indent())
					writer.WriteLine($"ThrowHelper.ThrowArgumentNullException_{dataName}();");
				writer.WriteLine($"return {calledMethodName}({dataName}, 0, {dataName}.Length);");
			}
			writer.WriteLine("}");
		}

		void GenCreateDeclareDataSlice(FileWriter writer, CreateMethod method, int elemSize, EnumValue code, string methodName, string setDeclValueName) {
			writer.WriteLine();
			writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
			WriteDocs(writer, method);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				var dataName = idConverter.Argument(method.Args[0].Name);
				writer.WriteLine($"if ((uint){dataName}.Length - 1 > {16 / elemSize} - 1)");
				using (writer.Indent())
					writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{dataName}();");
				writer.WriteLine();
				WriteInitializeInstruction(writer, code);
				writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){dataName}.Length;");
				writer.WriteLine();
				writer.WriteLine($"for (int i = 0; i < {dataName}.Length; i++)");
				using (writer.Indent())
					writer.WriteLine($"instruction.{setDeclValueName}(i, {dataName}[i]);");
				WriteMethodFooter(writer, 0);
			}
			writer.WriteLine("}");
			writer.WriteLineNoIndent($"#endif");
		}

		protected override void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			switch (kind) {
			case DeclareDataKind.Byte:
				switch (arrayType) {
				case ArrayType.BytePtr:
					break;

				case ArrayType.ByteArray:
					GenCreateDeclareDataArray(writer, method, "CreateDeclareByte", "CreateDeclareByte");
					break;

				case ArrayType.ByteSlice:
					GenCreateDeclareDataSlice(writer, method, 1, codeType[nameof(Code.DeclareByte)], "CreateDeclareByte", "SetDeclareByteValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Word:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.WordPtr:
					break;

				case ArrayType.ByteArray:
				case ArrayType.WordArray:
					GenCreateDeclareDataArray(writer, method, "CreateDeclareWord", "CreateDeclareWord");
					break;

				case ArrayType.ByteSlice:
					writer.WriteLine();
					writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
					WriteDocs(writer, method);
					writer.Write($"public static Instruction CreateDeclareWord(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if ((uint){dataName}.Length - 1 > 16 - 1 || ((uint){dataName}.Length & 1) != 0)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{dataName}();");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareWord)]);
						writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){dataName}.Length / 2;");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {dataName}.Length; i += 2) {{");
						using (writer.Indent()) {
							writer.WriteLine($"uint v = {dataName}[i] | ((uint){dataName}[i + 1] << 8);");
							writer.WriteLine("instruction.SetDeclareWordValue(i / 2, (ushort)v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					writer.WriteLineNoIndent($"#endif");
					break;

				case ArrayType.WordSlice:
					GenCreateDeclareDataSlice(writer, method, 2, codeType[nameof(Code.DeclareWord)], "CreateDeclareWord", "SetDeclareWordValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Dword:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.DwordPtr:
					break;

				case ArrayType.ByteArray:
				case ArrayType.DwordArray:
					GenCreateDeclareDataArray(writer, method, "CreateDeclareDword", "CreateDeclareDword");
					break;

				case ArrayType.ByteSlice:
					writer.WriteLine();
					writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
					WriteDocs(writer, method);
					writer.Write($"public static Instruction CreateDeclareDword(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if ((uint){dataName}.Length - 1 > 16 - 1 || ((uint){dataName}.Length & 3) != 0)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{dataName}();");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareDword)]);
						writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){dataName}.Length / 4;");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {dataName}.Length; i += 4) {{");
						using (writer.Indent()) {
							writer.WriteLine($"uint v = {dataName}[i] | ((uint){dataName}[i + 1] << 8) | ((uint){dataName}[i + 2] << 16) | ((uint){dataName}[i + 3] << 24);");
							writer.WriteLine("instruction.SetDeclareDwordValue(i / 4, v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					writer.WriteLineNoIndent($"#endif");
					break;

				case ArrayType.DwordSlice:
					GenCreateDeclareDataSlice(writer, method, 4, codeType[nameof(Code.DeclareDword)], "CreateDeclareDword", "SetDeclareDwordValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Qword:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.QwordPtr:
					break;

				case ArrayType.ByteArray:
				case ArrayType.QwordArray:
					GenCreateDeclareDataArray(writer, method, "CreateDeclareQword", "CreateDeclareQword");
					break;

				case ArrayType.ByteSlice:
					writer.WriteLine();
					writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
					WriteDocs(writer, method);
					writer.Write($"public static Instruction CreateDeclareQword(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if ((uint){dataName}.Length - 1 > 16 - 1 || ((uint){dataName}.Length & 7) != 0)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{dataName}();");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareQword)]);
						writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){dataName}.Length / 8;");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {dataName}.Length; i += 8) {{");
						using (writer.Indent()) {
							writer.WriteLine($"uint v1 = {dataName}[i] | ((uint){dataName}[i + 1] << 8) | ((uint){dataName}[i + 2] << 16) | ((uint){dataName}[i + 3] << 24);");
							writer.WriteLine($"uint v2 = {dataName}[i + 4] | ((uint){dataName}[i + 5] << 8) | ((uint){dataName}[i + 6] << 16) | ((uint){dataName}[i + 7] << 24);");
							writer.WriteLine("instruction.SetDeclareQwordValue(i / 8, (ulong)v1 | ((ulong)v2 << 32));");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					writer.WriteLineNoIndent($"#endif");
					break;

				case ArrayType.QwordSlice:
					GenCreateDeclareDataSlice(writer, method, 8, codeType[nameof(Code.DeclareQword)], "CreateDeclareQword", "SetDeclareQwordValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, int elemSize, EnumValue code, string methodName, string setDeclValueName) {
			writer.WriteLine();
			WriteDocs(writer, method);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				var dataName = idConverter.Argument(method.Args[0].Name);
				var indexName = idConverter.Argument(method.Args[1].Name);
				var lengthName = idConverter.Argument(method.Args[2].Name);
				writer.WriteLine($"if ({dataName} is null)");
				using (writer.Indent())
					writer.WriteLine($"ThrowHelper.ThrowArgumentNullException_{dataName}();");
				writer.WriteLine($"if ((uint){lengthName} - 1 > {16 / elemSize} - 1)");
				using (writer.Indent())
					writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{lengthName}();");
				writer.WriteLine($"if ((ulong)(uint){indexName} + (uint){lengthName} > (uint){dataName}.Length)");
				using (writer.Indent())
					writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{indexName}();");
				writer.WriteLine();
				WriteInitializeInstruction(writer, code);
				writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){lengthName};");
				writer.WriteLine();
				writer.WriteLine($"for (int i = 0; i < {lengthName}; i++)");
				using (writer.Indent())
					writer.WriteLine($"instruction.{setDeclValueName}(i, {dataName}[{indexName} + i]);");
				WriteMethodFooter(writer, 0);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			string dataName, lengthName, indexName;
			switch (kind) {
			case DeclareDataKind.Byte:
				switch (arrayType) {
				case ArrayType.ByteArray:
					GenCreateDeclareDataArrayLength(writer, method, 1, codeType[nameof(Code.DeclareByte)], "CreateDeclareByte", "SetDeclareByteValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Word:
				switch (arrayType) {
				case ArrayType.ByteArray:
					writer.WriteLine();
					WriteDocs(writer, method);
					writer.Write($"public static Instruction CreateDeclareWord(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						dataName = idConverter.Argument(method.Args[0].Name);
						indexName = idConverter.Argument(method.Args[1].Name);
						lengthName = idConverter.Argument(method.Args[2].Name);
						writer.WriteLine($"if ({dataName} is null)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentNullException_{dataName}();");
						writer.WriteLine($"if ((uint){lengthName} - 1 > 16 - 1 || ((uint){lengthName} & 1) != 0)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{lengthName}();");
						writer.WriteLine($"if ((ulong)(uint){indexName} + (uint){lengthName} > (uint){dataName}.Length)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{indexName}();");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareWord)]);
						writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){lengthName} / 2;");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {lengthName}; i += 2) {{");
						using (writer.Indent()) {
							writer.WriteLine($"uint v = {dataName}[{indexName} + i] | ((uint){dataName}[{indexName} + i + 1] << 8);");
							writer.WriteLine("instruction.SetDeclareWordValue(i / 2, (ushort)v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.WordArray:
					GenCreateDeclareDataArrayLength(writer, method, 2, codeType[nameof(Code.DeclareWord)], "CreateDeclareWord", "SetDeclareWordValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Dword:
				switch (arrayType) {
				case ArrayType.ByteArray:
					writer.WriteLine();
					WriteDocs(writer, method);
					writer.Write($"public static Instruction CreateDeclareDword(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						dataName = idConverter.Argument(method.Args[0].Name);
						indexName = idConverter.Argument(method.Args[1].Name);
						lengthName = idConverter.Argument(method.Args[2].Name);
						writer.WriteLine($"if ({dataName} is null)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentNullException_{dataName}();");
						writer.WriteLine($"if ((uint){lengthName} - 1 > 16 - 1 || ((uint){lengthName} & 3) != 0)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{lengthName}();");
						writer.WriteLine($"if ((ulong)(uint){indexName} + (uint){lengthName} > (uint){dataName}.Length)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{indexName}();");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareDword)]);
						writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){lengthName} / 4;");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {lengthName}; i += 4) {{");
						using (writer.Indent()) {
							writer.WriteLine($"uint v = {dataName}[{indexName} + i] | ((uint){dataName}[{indexName} + i + 1] << 8) | ((uint){dataName}[{indexName} + i + 2] << 16) | ((uint){dataName}[{indexName} + i + 3] << 24);");
							writer.WriteLine("instruction.SetDeclareDwordValue(i / 4, v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.DwordArray:
					GenCreateDeclareDataArrayLength(writer, method, 4, codeType[nameof(Code.DeclareDword)], "CreateDeclareDword", "SetDeclareDwordValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Qword:
				switch (arrayType) {
				case ArrayType.ByteArray:
					writer.WriteLine();
					WriteDocs(writer, method);
					writer.Write($"public static Instruction CreateDeclareQword(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						dataName = idConverter.Argument(method.Args[0].Name);
						indexName = idConverter.Argument(method.Args[1].Name);
						lengthName = idConverter.Argument(method.Args[2].Name);
						writer.WriteLine($"if ({dataName} is null)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentNullException_{dataName}();");
						writer.WriteLine($"if ((uint){lengthName} - 1 > 16 - 1 || ((uint){lengthName} & 7) != 0)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{lengthName}();");
						writer.WriteLine($"if ((ulong)(uint){indexName} + (uint){lengthName} > (uint){dataName}.Length)");
						using (writer.Indent())
							writer.WriteLine($"ThrowHelper.ThrowArgumentOutOfRangeException_{indexName}();");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareQword)]);
						writer.WriteLine($"instruction.InternalDeclareDataCount = (uint){lengthName} / 8;");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {lengthName}; i += 8) {{");
						using (writer.Indent()) {
							writer.WriteLine($"uint v1 = {dataName}[{indexName} + i] | ((uint){dataName}[{indexName} + i + 1] << 8) | ((uint){dataName}[{indexName} + i + 2] << 16) | ((uint){dataName}[{indexName} + i + 3] << 24);");
							writer.WriteLine($"uint v2 = {dataName}[{indexName} + i + 4] | ((uint){dataName}[{indexName} + i + 5] << 8) | ((uint){dataName}[{indexName} + i + 6] << 16) | ((uint){dataName}[{indexName} + i + 7] << 24);");
							writer.WriteLine("instruction.SetDeclareQwordValue(i / 8, (ulong)v1 | ((ulong)v2 << 32));");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.QwordArray:
					GenCreateDeclareDataArrayLength(writer, method, 8, codeType[nameof(Code.DeclareQword)], "CreateDeclareQword", "SetDeclareQwordValue");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			default:
				throw new InvalidOperationException();
			}
		}
	}
}
