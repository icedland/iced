// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.IO;
using System.Text;
using Generator.Documentation.Cpp;
using Generator.Enums;
using Generator.IO;

namespace Generator.Encoder.Cpp {
	[Generator(TargetLanguage.Cpp)]
	sealed class CppInstrCreateGen : InstrCreateGen {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;
		readonly CppDocCommentWriter docWriter;
		readonly CppInstrCreateGenImpl gen;
		readonly StringBuilder sb;

		public CppInstrCreateGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = CppIdentifierConverter.Create();
			docWriter = new CppDocCommentWriter(idConverter);
			gen = new CppInstrCreateGenImpl(genTypes, idConverter, docWriter);
			sb = new StringBuilder();
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.Cpp, "Create", CppConstants.GetHeaderFilename(genTypes, "instruction_create.hpp"));

		// Override Generate() to write the entire file instead of using FileUpdater
		public new void Generate() {
			var (language, _, filename) = GetFileInfo();

			// Create directory if it doesn't exist
			var dir = Path.GetDirectoryName(filename);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				Directory.CreateDirectory(dir);

			using var writer = new FileWriter(language, FileUtils.OpenWrite(filename));
			writer.WriteFileHeader();
			GenerateAll(writer);
		}

		void GenerateAll(FileWriter writer) {
			// Write header file preamble
			WriteHeaderPreamble(writer);

			// Generate main with() methods
			GenCreateMethods(writer, 0);

			// Generate specialized methods
			WriteItemSeparator(writer);
			GenTheRest(writer);

			// Write footer
			WriteHeaderFooter(writer);
		}

		// This override is required by the base class but we don't use it
		protected override void Generate(FileWriter writer) {
			// Not used - we override Generate() directly
			throw new InvalidOperationException("This method should not be called");
		}

		void WriteHeaderPreamble(FileWriter writer) {
			var headerGuard = CppConstants.GetHeaderGuard("INSTRUCTION_CREATE");
			writer.WriteLine("#pragma once");
			writer.WriteLine($"#ifndef {headerGuard}");
			writer.WriteLine($"#define {headerGuard}");
			writer.WriteLine();
			writer.WriteLine("#include \"instruction.hpp\"");
			writer.WriteLine("#include \"memory_operand.hpp\"");
			writer.WriteLine("#include \"code.hpp\"");
			writer.WriteLine("#include \"register.hpp\"");
			writer.WriteLine("#include \"rep_prefix_kind.hpp\"");
			writer.WriteLine("#include <cstdint>");
			writer.WriteLine("#include <span>");
			writer.WriteLine("#include <stdexcept>");
			writer.WriteLine();
			writer.WriteLine($"namespace {CppConstants.Namespace} {{");
			writer.WriteLine();
			writer.WriteLine("/// @brief Static factory methods for creating Instruction objects.");
			writer.WriteLine("/// @details These methods provide a convenient way to create instructions");
			writer.WriteLine("/// without manually setting up all the fields.");
			writer.WriteLine("struct InstructionFactory {");
			writer.WriteLine();
		}

		void WriteHeaderFooter(FileWriter writer) {
			writer.WriteLine("};"); // Close InstructionFactory struct
			writer.WriteLine();
			writer.WriteLine($"}} // namespace {CppConstants.Namespace}");
			writer.WriteLine();
			writer.WriteLine($"#endif // {CppConstants.GetHeaderGuard("INSTRUCTION_CREATE")}");
		}

		void WriteDocs(FileWriter writer, CreateMethod method, Action? writeSection) {
			gen.WriteDocs(writer, method, string.Empty, writeSection);
		}

		void WriteDocsWithError(FileWriter writer, CreateMethod method, string errorMsg) {
			gen.WriteDocs(writer, method, errorMsg, null);
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group, int id) {
			if (id == 0) {
				int opCount = method.Args.Count - 1;
				string methodName = CppInstrCreateGenImpl.GetCppOverloadedCreateName(opCount);

				gen.WriteDocsSimple(writer, method);

				// Write static method declaration
				writer.Write($"[[nodiscard]] static Instruction {methodName}(");
				gen.WriteMethodDeclArgs(writer, method);
				writer.WriteLine(");");
				writer.WriteLine();
			}
			else {
				throw new InvalidOperationException();
			}
		}

		void GenCreateBody(FileWriter writer, CreateMethod method) {
			writer.WriteLine("Instruction instruction{};");
			var args = method.Args;
			if (args.Count == 0 || args[0].Type != MethodArgType.Code)
				throw new InvalidOperationException();
			var codeName = idConverter.Argument(args[0].Name);
			writer.WriteLine($"instruction.set_code({codeName});");

			for (int i = 1; i < args.Count; i++) {
				int op = i - 1;
				var arg = args[i];
				writer.WriteLine();
				switch (arg.Type) {
				case MethodArgType.Register:
					writer.WriteLine($"// OpKind::Register == 0, so no need to set it");
					writer.WriteLine($"instruction.set_op{op}_register({idConverter.Argument(arg.Name)});");
					break;

				case MethodArgType.Memory:
					writer.WriteLine($"instruction.set_op{op}_kind(OpKind::MEMORY);");
					writer.WriteLine($"init_memory_operand(instruction, {idConverter.Argument(arg.Name)});");
					break;

				case MethodArgType.Int32:
				case MethodArgType.UInt32:
					writer.WriteLine($"initialize_immediate(instruction, {op}, static_cast<{(arg.Type == MethodArgType.Int32 ? "int64_t" : "uint64_t")}>({idConverter.Argument(arg.Name)}));");
					break;

				case MethodArgType.Int64:
				case MethodArgType.UInt64:
					writer.WriteLine($"initialize_immediate(instruction, {op}, {idConverter.Argument(arg.Name)});");
					break;

				default:
					throw new InvalidOperationException($"Unsupported arg type: {arg.Type}");
				}
			}
		}

		protected override void GenCreateBranch(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 2)
				throw new InvalidOperationException();

			WriteDocsWithError(writer, method, "if the created instruction doesn't have a near branch operand");

			writer.Write($"[[nodiscard]] static Instruction {CppInstrCreateGenNames.with_branch}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 3)
				throw new InvalidOperationException();

			WriteDocsWithError(writer, method, "if the created instruction doesn't have a far branch operand");

			writer.Write($"[[nodiscard]] static Instruction {CppInstrCreateGenNames.with_far_branch}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 2)
				throw new InvalidOperationException();

			WriteDocsWithError(writer, method, "if bitness is not one of 16, 32, 64");

			writer.Write($"[[nodiscard]] static Instruction {CppInstrCreateGenNames.with_xbegin}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			var methodName = idConverter.Method("With" + methodBaseName);

			WriteDocsWithError(writer, method, "if address_size is not one of 16, 32, 64");

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			var methodName = idConverter.Method("With" + methodBaseName);

			WriteDocsWithError(writer, method, "if address_size is not one of 16, 32, 64");

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			var methodName = idConverter.Method("With" + methodBaseName);

			WriteDocsWithError(writer, method, "if address_size is not one of 16, 32, 64");

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			var methodName = idConverter.Method("With" + methodBaseName);

			WriteDocsWithError(writer, method, "if address_size is not one of 16, 32, 64");

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			var methodName = idConverter.Method("With" + methodBaseName);

			WriteDocsWithError(writer, method, "if address_size is not one of 16, 32, 64");

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code) {
			var methodName = idConverter.Method("With" + methodBaseName);

			WriteDocsWithError(writer, method, "if address_size is not one of 16, 32, 64");

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind) {
			string methodName;
			switch (kind) {
			case DeclareDataKind.Byte:
				methodName = CppInstrCreateGenNames.with_declare_byte;
				break;
			case DeclareDataKind.Word:
				methodName = CppInstrCreateGenNames.with_declare_word;
				break;
			case DeclareDataKind.Dword:
				methodName = CppInstrCreateGenNames.with_declare_dword;
				break;
			case DeclareDataKind.Qword:
				methodName = CppInstrCreateGenNames.with_declare_qword;
				break;
			default:
				throw new InvalidOperationException();
			}
			methodName = CppInstrCreateGenNames.AppendArgCount(methodName, method.Args.Count);

			gen.WriteDocsSimple(writer, method);

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			// Skip unsupported array types for C++ - we'll use span instead
			switch (arrayType) {
			case ArrayType.ByteArray:
			case ArrayType.WordArray:
			case ArrayType.DwordArray:
			case ArrayType.QwordArray:
				// Skip - use span versions instead
				return;

			case ArrayType.BytePtr:
			case ArrayType.WordPtr:
			case ArrayType.DwordPtr:
			case ArrayType.QwordPtr:
				// Generate pointer + length versions
				GenCreateDeclareDataPtr(writer, method, kind, arrayType);
				return;

			case ArrayType.ByteSlice:
			case ArrayType.WordSlice:
			case ArrayType.DwordSlice:
			case ArrayType.QwordSlice:
				// Generate span versions
				GenCreateDeclareDataSpan(writer, method, kind, arrayType);
				return;

			default:
				throw new InvalidOperationException();
			}
		}

		void GenCreateDeclareDataPtr(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			string methodName = kind switch {
				DeclareDataKind.Byte => CppInstrCreateGenNames.with_declare_byte,
				DeclareDataKind.Word => CppInstrCreateGenNames.with_declare_word,
				DeclareDataKind.Dword => CppInstrCreateGenNames.with_declare_dword,
				DeclareDataKind.Qword => CppInstrCreateGenNames.with_declare_qword,
				_ => throw new InvalidOperationException(),
			};

			gen.WriteDocsSimple(writer, method);

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		void GenCreateDeclareDataSpan(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			string methodName = kind switch {
				DeclareDataKind.Byte => CppInstrCreateGenNames.with_declare_byte_span,
				DeclareDataKind.Word => CppInstrCreateGenNames.with_declare_word_span,
				DeclareDataKind.Dword => CppInstrCreateGenNames.with_declare_dword_span,
				DeclareDataKind.Qword => CppInstrCreateGenNames.with_declare_qword_span,
				_ => throw new InvalidOperationException(),
			};

			gen.WriteDocsSimple(writer, method);

			writer.Write($"[[nodiscard]] static Instruction {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(");");
			writer.WriteLine();
		}

		protected override void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			// Skip - C++ uses span with built-in length
		}
	}
}
