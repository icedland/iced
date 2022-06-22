// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;
using System.Text;
using Generator.Documentation.Java;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Encoder.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaInstrCreateGen : InstrCreateGen {
		readonly IdentifierConverter idConverter;
		readonly JavaDocCommentWriter docWriter;
		readonly InstrCreateGenImpl gen;
		readonly StringBuilder sb;

		protected override bool SupportsUnsignedIntegers => false;
		protected override bool SupportsDefaultArguments => false;

		public JavaInstrCreateGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = JavaIdentifierConverter.Create();
			docWriter = new JavaDocCommentWriter(idConverter);
			gen = new InstrCreateGenImpl(genTypes, idConverter, docWriter);
			sb = new StringBuilder();
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.Java, "Create", JavaConstants.GetFilename(genTypes, JavaConstants.IcedPackage, "Instruction.java"));

		void WriteDocs(FileWriter writer, CreateMethod method) {
			const string typeName = "Instruction";
			docWriter.BeginWrite(writer);
			foreach (var doc in method.Docs)
				docWriter.WriteDocLine(writer, doc, typeName);
			docWriter.WriteLine(writer, string.Empty);
			for (int i = 0; i < method.Args.Count; i++) {
				var arg = method.Args[i];
				docWriter.Write($"@param {idConverter.Argument(arg.Name)} ");
				var docs = arg.Doc;
				switch (arg.Type) {
				case MethodArgType.Code:
					docs = $"{docs} (a {{@link {JavaConstants.IcedPackage}.Code}} enum variant)";
					break;
				case MethodArgType.Register:
					docs = $"{docs} (see {{@link ICRegisters}})";
					break;
				case MethodArgType.RepPrefixKind:
					docs = $"{docs} (a {{@link {JavaConstants.IcedPackage}.RepPrefixKind}} enum variant)";
					break;
				default:
					break;
				}
				docWriter.WriteDoc(writer, docs, typeName);
				docWriter.WriteLine(writer, string.Empty);
			}
			docWriter.EndWrite(writer);
		}

		void WriteMethodDeclArgs(FileWriter writer, CreateMethod method) =>
			gen.WriteMethodDeclArgs(writer, method);

		void WriteInitializeInstruction(FileWriter writer, CreateMethod method) {
			writer.WriteLine("Instruction instruction = new Instruction();");
			var args = method.Args;
			if (args.Count == 0 || args[0].Type != MethodArgType.Code)
				throw new InvalidOperationException();
			var codeName = idConverter.Argument(args[0].Name);
			writer.WriteLine($"instruction.setCode({codeName});");
		}

		void WriteInitializeInstruction(FileWriter writer, EnumValue code) {
			writer.WriteLine("Instruction instruction = new Instruction();");
			writer.WriteLine($"instruction.setCode({idConverter.ToDeclTypeAndValue(code)});");
		}

		static void WriteMethodFooter(FileWriter writer, int args) {
			writer.WriteLine();
			writer.WriteLine($"assert instruction.getOpCount() == {args} : instruction.getOpCount();");
			writer.WriteLine("return instruction;");
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group, int id) {
			WriteDocs(writer, method);
			writer.Write("public static Instruction create(");
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
						writer.WriteLine($"instruction.setOp{op}Register({idConverter.Argument(arg.Name)}.get());");
						break;

					case MethodArgType.Memory:
						writer.WriteLine($"instruction.setOp{op}Kind({memoryStr});");
						writer.WriteLine($"initMemoryOperand(instruction, {idConverter.Argument(arg.Name)});");
						break;

					case MethodArgType.Int32:
					case MethodArgType.UInt32:
					case MethodArgType.Int64:
					case MethodArgType.UInt64:
						var methodName = arg.Type == MethodArgType.Int32 || arg.Type == MethodArgType.Int64 ? "initializeSignedImmediate" : "initializeUnsignedImmediate";
						writer.WriteLine($"{methodName}(instruction, {op}, {idConverter.Argument(arg.Name)});");
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
			writer.Write("public static Instruction createBranch(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				writer.WriteLine();
				writer.WriteLine($"instruction.setOp0Kind(getNearBranchOpKind({idConverter.Argument(method.Args[0].Name)}, 0));");
				writer.WriteLine($"instruction.setNearBranch64({idConverter.Argument(method.Args[1].Name)});");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 3)
				throw new InvalidOperationException();
			WriteDocs(writer, method);
			writer.Write("public static Instruction createBranch(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				writer.WriteLine();
				writer.WriteLine($"instruction.setOp0Kind(getFarBranchOpKind({idConverter.Argument(method.Args[0].Name)}, 0));");
				writer.Write($"instruction.setFarBranchSelector(");
				Write(writer, method, 1);
				writer.WriteLine(");");
				writer.WriteLine($"instruction.setFarBranch32({idConverter.Argument(method.Args[2].Name)});");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 2)
				throw new InvalidOperationException();
			WriteDocs(writer, method);
			writer.Write("public static Instruction createXbegin(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.WriteLine("Instruction instruction = new Instruction();");
				var bitnessName = idConverter.Argument(method.Args[0].Name);
				writer.WriteLine($"switch ({bitnessName}) {{");
				writer.WriteLine($"case 16:");
				using (writer.Indent()) {
					writer.WriteLine($"instruction.setCode({idConverter.ToDeclTypeAndValue(codeType[nameof(Code.Xbegin_rel16)])});");
					writer.WriteLine($"instruction.setOp0Kind({idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch32)])});");
					writer.WriteLine($"instruction.setNearBranch32((int){idConverter.Argument(method.Args[1].Name)});");
					writer.WriteLine($"break;");
				}
				writer.WriteLine();
				writer.WriteLine($"case 32:");
				using (writer.Indent()) {
					writer.WriteLine($"instruction.setCode({idConverter.ToDeclTypeAndValue(codeType[nameof(Code.Xbegin_rel32)])});");
					writer.WriteLine($"instruction.setOp0Kind({idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch32)])});");
					writer.WriteLine($"instruction.setNearBranch32((int){idConverter.Argument(method.Args[1].Name)});");
					writer.WriteLine($"break;");
				}
				writer.WriteLine();
				writer.WriteLine($"case 64:");
				using (writer.Indent()) {
					writer.WriteLine($"instruction.setCode({idConverter.ToDeclTypeAndValue(codeType[nameof(Code.Xbegin_rel32)])});");
					writer.WriteLine($"instruction.setOp0Kind({idConverter.ToDeclTypeAndValue(genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch64)])});");
					writer.WriteLine($"instruction.setNearBranch64({idConverter.Argument(method.Args[1].Name)});");
					writer.WriteLine($"break;");
				}
				writer.WriteLine();
				writer.WriteLine($"default:");
				using (writer.Indent())
					writer.WriteLine($"throw new IllegalArgumentException(\"{bitnessName}\");");
				writer.WriteLine($"}}");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		static void WriteComma(FileWriter writer) => writer.Write(", ");
		void Write(FileWriter writer, EnumValue value) => writer.Write(idConverter.ToDeclTypeAndValue(value));
		void Write(FileWriter writer, CreateMethod method, int argIndex) {
			if ((uint)argIndex < (uint)method.Args.Count) {
				var arg = method.Args[argIndex];
				var convFn = InstrCreateGenImpl.GetIntConvertFunc(arg.Type);
				if (convFn is not null)
					writer.Write($"{convFn}(");
				if (arg.Type == MethodArgType.Register)
					writer.Write($"{arg.Name}.get()");
				else
					writer.Write(idConverter.Argument(arg.Name));
				if (convFn is not null)
					writer.Write(")");
			} else {
				argIndex -= method.Args.Count;
				switch (method.DefaultArgs[argIndex]) {
				case EnumValue enumValue:
					writer.Write(idConverter.ToDeclTypeAndValue(enumValue));
					break;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.Write("return createString_Reg_SegRSI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count + method.DefaultArgs.Count != 3)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, method, 1);
					WriteComma(writer);
					Write(writer, method, 2);
					break;
				case StringMethodKind.Rep:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
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
			writer.WriteLine("}");
		}

		protected override void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.Write("return createString_Reg_ESRDI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count + method.DefaultArgs.Count != 2)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, method, 1);
					break;
				case StringMethodKind.Repe:
				case StringMethodKind.Repne:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
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
			writer.WriteLine("}");
		}

		protected override void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.Write("return createString_ESRDI_Reg(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count + method.DefaultArgs.Count != 2)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
					WriteComma(writer);
					Write(writer, register);
					WriteComma(writer);
					Write(writer, method, 1);
					break;
				case StringMethodKind.Rep:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
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
			writer.WriteLine("}");
		}

		protected override void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.Write("return createString_SegRSI_ESRDI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count + method.DefaultArgs.Count != 3)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
					WriteComma(writer);
					Write(writer, method, 1);
					WriteComma(writer);
					Write(writer, method, 2);
					break;
				case StringMethodKind.Repe:
				case StringMethodKind.Repne:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
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
			writer.WriteLine("}");
		}

		protected override void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.Write("return createString_ESRDI_SegRSI(");
				switch (kind) {
				case StringMethodKind.Full:
					if (method.Args.Count + method.DefaultArgs.Count != 3)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
					WriteComma(writer);
					Write(writer, method, 1);
					WriteComma(writer);
					Write(writer, method, 2);
					break;
				case StringMethodKind.Rep:
					if (method.Args.Count != 1)
						throw new InvalidOperationException();
					Write(writer, code);
					WriteComma(writer);
					Write(writer, method, 0);
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
			writer.WriteLine("}");
		}

		protected override void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method);
			var methodName = idConverter.Method("Create" + methodBaseName);
			writer.Write($"public static Instruction {methodName}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") {");
			using (writer.Indent()) {
				writer.Write("return createMaskmov(");
				if (method.Args.Count + method.DefaultArgs.Count != 4)
					throw new InvalidOperationException();
				Write(writer, code);
				WriteComma(writer);
				Write(writer, method, 0);
				WriteComma(writer);
				Write(writer, method, 1);
				WriteComma(writer);
				Write(writer, method, 2);
				WriteComma(writer);
				Write(writer, method, 3);
				writer.WriteLine(");");
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind) {
			EnumValue code;
			string setValueName;
			string methodName;
			switch (kind) {
			case DeclareDataKind.Byte:
				code = codeType[nameof(Code.DeclareByte)];
				setValueName = "setDeclareByteValue";
				methodName = "createDeclareByte";
				break;

			case DeclareDataKind.Word:
				code = codeType[nameof(Code.DeclareWord)];
				setValueName = "setDeclareWordValue";
				methodName = "createDeclareWord";
				break;

			case DeclareDataKind.Dword:
				code = codeType[nameof(Code.DeclareDword)];
				setValueName = "setDeclareDwordValue";
				methodName = "createDeclareDword";
				break;

			case DeclareDataKind.Qword:
				code = codeType[nameof(Code.DeclareQword)];
				setValueName = "setDeclareQwordValue";
				methodName = "createDeclareQword";
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
				writer.WriteLine($"instruction.setDeclareDataCount({method.Args.Count});");
				writer.WriteLine();
				for (int i = 0; i < method.Args.Count; i++) {
					writer.Write($"instruction.{setValueName}({i}, ");
					Write(writer, method, i);
					writer.WriteLine(");");
				}
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
				writer.WriteLine($"if ({dataName} == null)");
				using (writer.Indent())
					writer.WriteLine($"throw new NullPointerException(\"{dataName}\");");
				writer.WriteLine($"return {calledMethodName}({dataName}, 0, {dataName}.length);");
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			switch (kind) {
			case DeclareDataKind.Byte:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.ByteSlice:
					break;

				case ArrayType.ByteArray:
					GenCreateDeclareDataArray(writer, method, "createDeclareByte", "createDeclareByte");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Word:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.WordPtr:
				case ArrayType.ByteSlice:
				case ArrayType.WordSlice:
					break;

				case ArrayType.ByteArray:
				case ArrayType.WordArray:
					GenCreateDeclareDataArray(writer, method, "createDeclareWord", "createDeclareWord");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Dword:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.DwordPtr:
				case ArrayType.ByteSlice:
				case ArrayType.DwordSlice:
					break;

				case ArrayType.ByteArray:
				case ArrayType.DwordArray:
					GenCreateDeclareDataArray(writer, method, "createDeclareDword", "createDeclareDword");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Qword:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.QwordPtr:
				case ArrayType.ByteSlice:
				case ArrayType.QwordSlice:
					break;

				case ArrayType.ByteArray:
				case ArrayType.QwordArray:
					GenCreateDeclareDataArray(writer, method, "createDeclareQword", "createDeclareQword");
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
				writer.WriteLine($"if ({dataName} == null)");
				using (writer.Indent())
					writer.WriteLine($"throw new NullPointerException(\"{dataName}\");");
				writer.WriteLine($"if (Integer.compareUnsigned({lengthName} - 1, {16 / elemSize} - 1) > 0)");
				using (writer.Indent())
					writer.WriteLine($"throw new IllegalArgumentException(\"{lengthName}\");");
				writer.WriteLine($"if (Long.compareUnsigned(((long){indexName} & 0xFFFF_FFFFL) + ((long){lengthName} & 0xFFFF_FFFFL), (long){dataName}.length & 0xFFFF_FFFFL) > 0)");
				using (writer.Indent())
					writer.WriteLine($"throw new IllegalArgumentException(\"{indexName}\");");
				writer.WriteLine();
				WriteInitializeInstruction(writer, code);
				writer.WriteLine($"instruction.setDeclareDataCount({lengthName});");
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
					GenCreateDeclareDataArrayLength(writer, method, 1, codeType[nameof(Code.DeclareByte)], "createDeclareByte", "setDeclareByteValue");
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
					writer.Write($"public static Instruction createDeclareWord(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						dataName = idConverter.Argument(method.Args[0].Name);
						indexName = idConverter.Argument(method.Args[1].Name);
						lengthName = idConverter.Argument(method.Args[2].Name);
						writer.WriteLine($"if ({dataName} == null)");
						using (writer.Indent())
							writer.WriteLine($"throw new NullPointerException(\"{dataName}\");");
						writer.WriteLine($"if (Integer.compareUnsigned({lengthName} - 1, 16 - 1) > 0 || ({lengthName} & 1) != 0)");
						using (writer.Indent())
							writer.WriteLine($"throw new IllegalArgumentException(\"{lengthName}\");");
						writer.WriteLine($"if (Long.compareUnsigned(((long){indexName} & 0xFFFF_FFFFL) + ((long){lengthName} & 0xFFFF_FFFFL), (long){dataName}.length & 0xFFFF_FFFFL) > 0)");
						using (writer.Indent())
							writer.WriteLine($"throw new IllegalArgumentException(\"{indexName}\");");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareWord)]);
						writer.WriteLine($"instruction.setDeclareDataCount({lengthName} / 2);");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {lengthName}; i += 2) {{");
						using (writer.Indent()) {
							writer.WriteLine($"int v = ({dataName}[{indexName} + i] & 0xFF) | (({dataName}[{indexName} + i + 1] & 0xFF) << 8);");
							writer.WriteLine("instruction.setDeclareWordValue(i / 2, (short)v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.WordArray:
					GenCreateDeclareDataArrayLength(writer, method, 2, codeType[nameof(Code.DeclareWord)], "createDeclareWord", "setDeclareWordValue");
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
					writer.Write($"public static Instruction createDeclareDword(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						dataName = idConverter.Argument(method.Args[0].Name);
						indexName = idConverter.Argument(method.Args[1].Name);
						lengthName = idConverter.Argument(method.Args[2].Name);
						writer.WriteLine($"if ({dataName} == null)");
						using (writer.Indent())
							writer.WriteLine($"throw new NullPointerException(\"{dataName}\");");
						writer.WriteLine($"if (Integer.compareUnsigned({lengthName} - 1, 16 - 1) > 0 || ({lengthName} & 3) != 0)");
						using (writer.Indent())
							writer.WriteLine($"throw new IllegalArgumentException(\"{lengthName}\");");
						writer.WriteLine($"if (Long.compareUnsigned(((long){indexName} & 0xFFFF_FFFFL) + ((long){lengthName} & 0xFFFF_FFFFL), (long){dataName}.length & 0xFFFF_FFFFL) > 0)");
						using (writer.Indent())
							writer.WriteLine($"throw new IllegalArgumentException(\"{indexName}\");");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareDword)]);
						writer.WriteLine($"instruction.setDeclareDataCount({lengthName} / 4);");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {lengthName}; i += 4) {{");
						using (writer.Indent()) {
							writer.WriteLine($"int v = ({dataName}[{indexName} + i] & 0xFF) | (({dataName}[{indexName} + i + 1] & 0xFF) << 8) | (({dataName}[{indexName} + i + 2] & 0xFF) << 16) | ({dataName}[{indexName} + i + 3] << 24);");
							writer.WriteLine("instruction.setDeclareDwordValue(i / 4, v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.DwordArray:
					GenCreateDeclareDataArrayLength(writer, method, 4, codeType[nameof(Code.DeclareDword)], "createDeclareDword", "setDeclareDwordValue");
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
					writer.Write($"public static Instruction createDeclareQword(");
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") {");
					using (writer.Indent()) {
						dataName = idConverter.Argument(method.Args[0].Name);
						indexName = idConverter.Argument(method.Args[1].Name);
						lengthName = idConverter.Argument(method.Args[2].Name);
						writer.WriteLine($"if ({dataName} == null)");
						using (writer.Indent())
							writer.WriteLine($"throw new NullPointerException(\"{dataName}\");");
						writer.WriteLine($"if (Integer.compareUnsigned({lengthName} - 1, 16 - 1) > 0 || ({lengthName} & 7) != 0)");
						using (writer.Indent())
							writer.WriteLine($"throw new IllegalArgumentException(\"{lengthName}\");");
						writer.WriteLine($"if (Long.compareUnsigned(((long){indexName} & 0xFFFF_FFFFL) + ((long){lengthName} & 0xFFFF_FFFFL), (long){dataName}.length & 0xFFFF_FFFFL) > 0)");
						using (writer.Indent())
							writer.WriteLine($"throw new IllegalArgumentException(\"{indexName}\");");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareQword)]);
						writer.WriteLine($"instruction.setDeclareDataCount({lengthName} / 8);");
						writer.WriteLine();
						writer.WriteLine($"for (int i = 0; i < {lengthName}; i += 8) {{");
						using (writer.Indent()) {
							writer.WriteLine($"int v1 = ({dataName}[{indexName} + i] & 0xFF) | (({dataName}[{indexName} + i + 1] & 0xFF) << 8) | (({dataName}[{indexName} + i + 2] & 0xFF) << 16) | ({dataName}[{indexName} + i + 3] << 24);");
							writer.WriteLine($"int v2 = ({dataName}[{indexName} + i + 4] & 0xFF) | (({dataName}[{indexName} + i + 5] & 0xFF) << 8) | (({dataName}[{indexName} + i + 6] & 0xFF) << 16) | ({dataName}[{indexName} + i + 7] << 24);");
							writer.WriteLine("instruction.setDeclareQwordValue(i / 8, ((long)v1 & 0xFFFF_FFFFL) | ((long)v2 << 32));");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.QwordArray:
					GenCreateDeclareDataArrayLength(writer, method, 8, codeType[nameof(Code.DeclareQword)], "createDeclareQword", "setDeclareQwordValue");
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
