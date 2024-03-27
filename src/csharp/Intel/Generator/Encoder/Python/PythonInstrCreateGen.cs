// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Text;
using Generator.Documentation.Python;
using Generator.Enums;
using Generator.IO;

namespace Generator.Encoder.Python {
	[Generator(TargetLanguage.Python)]
	sealed class PythonInstrCreateGen : InstrCreateGen {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;
		readonly IdentifierConverter rustIdConverter;
		readonly PythonDocCommentWriter docWriter;
		readonly Rust.GenCreateNameArgs genNames;
		readonly StringBuilder sb;

		public PythonInstrCreateGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = PythonIdentifierConverter.Create();
			rustIdConverter = RustIdentifierConverter.Create();
			docWriter = new PythonDocCommentWriter(idConverter, TargetLanguage.Rust, isInRootModule: true);
			genNames = new Rust.GenCreateNameArgs {
				CreatePrefix = "create",
				Register = "_reg",
				Memory = "_mem",
				Int32 = "_i32",
				UInt32 = "_u32",
				Int64 = "_i64",
				UInt64 = "_u64",
			};
			sb = new StringBuilder();
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.Rust, "Create", generatorContext.Types.Dirs.GetPythonRustFilename("instruction.rs"));

		sealed class MethodArgInfo {
			public readonly string RustName;
			public readonly string PythonName;

			public MethodArgInfo(string rustName, string pythonName) {
				RustName = rustName;
				PythonName = pythonName;
			}
		}

		sealed class GeneratedMethodInfo {
			public readonly CreateMethod Method;
			public readonly bool CanFail;
			/// <summary>iced_x86 method name</summary>
			public readonly string RustMethodName;
			/// <summary>iced_x86_py method name which is also the name used by all Python code</summary>
			public readonly string PythonMethodName;
			/// <summary>Extra argument info, eg. Rust/Python arg names</summary>
			public readonly MethodArgInfo[] ArgInfos;

			public GeneratedMethodInfo(CreateMethod method, bool canFail, string rustMethodName, string pythonMethodName, IdentifierConverter rustIdConverter, IdentifierConverter pythonIdConverter) {
				Method = method;
				CanFail = canFail;
				RustMethodName = rustMethodName;
				PythonMethodName = pythonMethodName;
				ArgInfos = new MethodArgInfo[method.Args.Count];
				for (int i = 0; i < method.Args.Count; i++) {
					var origName = method.Args[i].Name;
					var rustName = rustIdConverter.Argument(origName);
					var pythonName = pythonIdConverter.Argument(origName);
					ArgInfos[i] = new MethodArgInfo(rustName: rustName, pythonName: pythonName);
				}
			}
		}

		readonly struct GenerateMethodContext {
			public readonly FileWriter Writer;
			public readonly GeneratedMethodInfo Info;
			public CreateMethod Method => Info.Method;

			public GenerateMethodContext(FileWriter writer, GeneratedMethodInfo info) {
				Writer = writer;
				Info = info;
			}
		}

		void GenerateMethod(FileWriter writer, CreateMethod method, bool canFail, bool isTryMethod, Action<GenerateMethodContext> genMethod,
			string? rustMethodName = null, string? pythonMethodName = null) {
			if (rustMethodName is null)
				rustMethodName = Rust.InstrCreateGenImpl.GetRustOverloadedCreateName(method);
			else if (isTryMethod)
				rustMethodName = "try_" + rustMethodName;
			pythonMethodName ??= Rust.InstrCreateGenImpl.GetCreateName(sb, method, genNames);
			var info = new GeneratedMethodInfo(method, canFail, rustMethodName, pythonMethodName, rustIdConverter, idConverter);
			var ctx = new GenerateMethodContext(writer, info);
			genMethod(ctx);
		}

		static void WriteMethodDeclArgs(in GenerateMethodContext ctx) {
			bool comma = false;
			for (int i = 0; i < ctx.Method.Args.Count; i++) {
				var arg = ctx.Method.Args[i];
				if (comma)
					ctx.Writer.Write(", ");
				comma = true;
				ctx.Writer.Write(ctx.Info.ArgInfos[i].PythonName);
				ctx.Writer.Write(": ");
				switch (arg.Type) {
				case MethodArgType.Code:
				case MethodArgType.Register:
				case MethodArgType.RepPrefixKind:
					// All enums are u32 args
					ctx.Writer.Write("u32");
					break;
				case MethodArgType.Memory:
					ctx.Writer.Write("MemoryOperand");
					break;
				case MethodArgType.UInt8:
					ctx.Writer.Write("u8");
					break;
				case MethodArgType.UInt16:
					ctx.Writer.Write("u16");
					break;
				case MethodArgType.Int32:
					ctx.Writer.Write("i32");
					break;
				case MethodArgType.PreferredInt32:
				case MethodArgType.UInt32:
					ctx.Writer.Write("u32");
					break;
				case MethodArgType.Int64:
					ctx.Writer.Write("i64");
					break;
				case MethodArgType.UInt64:
					ctx.Writer.Write("u64");
					break;
				case MethodArgType.ByteSlice:
					ctx.Writer.Write("&Bound<'_, PyAny>");
					break;
				case MethodArgType.ByteArray:
				case MethodArgType.WordArray:
				case MethodArgType.DwordArray:
				case MethodArgType.QwordArray:
				case MethodArgType.BytePtr:
				case MethodArgType.WordPtr:
				case MethodArgType.DwordPtr:
				case MethodArgType.QwordPtr:
				case MethodArgType.WordSlice:
				case MethodArgType.DwordSlice:
				case MethodArgType.QwordSlice:
				case MethodArgType.ArrayIndex:
				case MethodArgType.ArrayLength:
				default:
					throw new InvalidOperationException();
				}
			}
		}

		void WriteTextSignature(in GenerateMethodContext ctx) {
			sb.Clear();
			sb.Append("#[pyo3(text_signature = \"(");
			var defaultArgsCount = WriteArgs(ctx);
			sb.Append(")\")]");
			ctx.Writer.WriteLine(sb.ToString());
		}

		void WriteSignature(in GenerateMethodContext ctx) {
			sb.Clear();
			sb.Append("#[pyo3(signature = (");
			var defaultArgsCount = WriteArgs(ctx);
			sb.Append("))]");
			if (defaultArgsCount > 0)
				ctx.Writer.WriteLine(sb.ToString());
		}

		int WriteArgs(in GenerateMethodContext ctx) {
			int defaultArgsCount = 0;
			for (int i = 0; i < ctx.Method.Args.Count; i++) {
				if (i > 0)
					sb.Append(", ");
				var defaultValue = ctx.Method.Args[i].DefaultValue;
				var argName = ctx.Info.ArgInfos[i].PythonName;
				sb.Append(argName);
				if (defaultValue is not null) {
					defaultArgsCount++;
					sb.Append(" = ");
					switch (defaultValue) {
					case EnumValue enumValue:
						sb.Append(enumValue.Value);
						break;
					default:
						throw new InvalidOperationException();
					}
				}
			}
			return defaultArgsCount;
		}

		static (string sphinxType, string pythonType, string? descType) GetArgType(MethodArgType type) =>
			type switch {
				MethodArgType.Code => (":class:`Code`", "Code", null),
				MethodArgType.Register => (":class:`Register`", "Register", null),
				MethodArgType.RepPrefixKind => (":class:`RepPrefixKind`", "RepPrefixKind", null),
				MethodArgType.Memory => (":class:`MemoryOperand`", "MemoryOperand", null),
				MethodArgType.UInt8 => ("int", "int", "``u8``"),
				MethodArgType.UInt16 => ("int", "int", "``u16``"),
				MethodArgType.Int32 => ("int", "int", "``i32``"),
				MethodArgType.PreferredInt32 or MethodArgType.UInt32 => ("int", "int", "``u32``"),
				MethodArgType.Int64 => ("int", "int", "``i64``"),
				MethodArgType.UInt64 => ("int", "int", "``u64``"),
				MethodArgType.ByteSlice => ("bytes, bytearray", "Union[bytes, bytearray]", null),
				_ => throw new InvalidOperationException(),
			};

		void WriteDocs(in GenerateMethodContext ctx, Func<IEnumerable<(string type, string text)>>? getThrowsDocs) {
			const string typeName = "Instruction";
			docWriter.BeginWrite(ctx.Writer);
			foreach (var doc in ctx.Info.Method.Docs)
				docWriter.WriteDocLine(ctx.Writer, doc, typeName);
			docWriter.WriteLine(ctx.Writer, string.Empty);
			docWriter.WriteLine(ctx.Writer, "Args:");
			for (int i = 0; i < ctx.Info.Method.Args.Count; i++) {
				var arg = ctx.Info.Method.Args[i];
				var typeInfo = GetArgType(arg.Type);
				docWriter.Write($"    `{idConverter.Argument(arg.Name)}` ({typeInfo.sphinxType}): ");
				if (typeInfo.descType is not null)
					docWriter.Write($"({typeInfo.descType}) ");
				docWriter.WriteDocLine(ctx.Writer, arg.Doc, typeName);
			}
			docWriter.WriteLine(ctx.Writer, string.Empty);
			docWriter.WriteLine(ctx.Writer, "Returns:");
			docWriter.WriteLine(ctx.Writer, $"    :class:`{typeName}`: Created instruction");
			if (getThrowsDocs is not null) {
				docWriter.WriteLine(ctx.Writer, string.Empty);
				docWriter.WriteLine(ctx.Writer, "Raises:");
				foreach (var doc in getThrowsDocs()) {
					if (!doc.text.StartsWith("If ", StringComparison.Ordinal))
						throw new InvalidOperationException();
					docWriter.WriteLine(ctx.Writer, $"    {doc.type}: {doc.text}");
				}
			}
			docWriter.EndWrite(ctx.Writer);
		}

		void WriteMethod(in GenerateMethodContext ctx, Func<IEnumerable<(string type, string text)>>? getThrowsDocs) {
			WriteDocs(ctx, getThrowsDocs);
			ctx.Writer.WriteLine("#[rustfmt::skip]");
			ctx.Writer.WriteLine("#[staticmethod]");
			WriteTextSignature(ctx);
			WriteSignature(ctx);
			ctx.Writer.Write($"fn {ctx.Info.PythonMethodName}(");
			WriteMethodDeclArgs(ctx);
			ctx.Writer.WriteLine(") -> PyResult<Self> {");
			using (ctx.Writer.Indent()) {
				for (int i = 0; i < ctx.Method.Args.Count; i++) {
					var arg = ctx.Method.Args[i];

					// Verify all enum args (they're u32 args)
					string? checkFunc;
					bool isUnsafe = false;
					switch (arg.Type) {
					case MethodArgType.Code:
						checkFunc = "to_code";
						break;
					case MethodArgType.Register:
						checkFunc = "to_register";
						break;
					case MethodArgType.RepPrefixKind:
						checkFunc = "to_rep_prefix_kind";
						break;
					case MethodArgType.ByteSlice:
						checkFunc = "get_temporary_byte_array_ref";
						isUnsafe = true;
						break;
					default:
						checkFunc = null;
						break;
					}
					if (checkFunc is not null) {
						var argName = ctx.Info.ArgInfos[i].PythonName;
						if (isUnsafe)
							ctx.Writer.WriteLine($"let {argName} = unsafe {{ {checkFunc}({argName})? }};");
						else
							ctx.Writer.WriteLine($"let {argName} = {checkFunc}({argName})?;");
					}
				}
			}
		}

		void WriteCall(in GenerateMethodContext ctx) {
			using (ctx.Writer.Indent()) {
				sb.Clear();
				sb.Append("Ok(");
				sb.Append("Instruction { instr: iced_x86::Instruction::");
				sb.Append(ctx.Info.RustMethodName);
				sb.Append('(');
				for (int i = 0; i < ctx.Method.Args.Count; i++) {
					if (i > 0)
						sb.Append(", ");
					switch (ctx.Method.Args[i].Type) {
					case MethodArgType.Memory:
						sb.Append(ctx.Info.ArgInfos[i].PythonName);
						sb.Append(".mem");
						break;
					default:
						sb.Append(ctx.Info.ArgInfos[i].PythonName);
						break;
					}
				}
				sb.Append(')');
				if (ctx.Info.CanFail)
					sb.Append(".map_err(to_value_error)?");
				sb.Append(" })");
				ctx.Writer.WriteLine(sb.ToString());
			}
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group, int id) {
			bool canFail = method.Args.Count > 1;
			GenerateMethod(writer, method, canFail, canFail, GenCreate);
		}

		void GenCreate(GenerateMethodContext ctx) {
			Func<IEnumerable<(string type, string text)>>? getThrowsDocs = null;
			if (ctx.Info.CanFail)
				getThrowsDocs = () => new[] { ("ValueError", "If one of the operands is invalid (basic checks)") };
			WriteMethod(ctx, getThrowsDocs);
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenCreateBranch, Rust.RustInstrCreateGenNames.with_branch, "create_branch");

		void GenCreateBranch(GenerateMethodContext ctx) {
			WriteMethod(ctx, () => new[] { ("ValueError", "If the created instruction doesn't have a near branch operand") });
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenCreateFarBranch, Rust.RustInstrCreateGenNames.with_far_branch, "create_far_branch");

		void GenCreateFarBranch(GenerateMethodContext ctx) {
			WriteMethod(ctx, () => new[] { ("ValueError", "If the created instruction doesn't have a far branch operand") });
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenCreateXbegin, Rust.RustInstrCreateGenNames.with_xbegin, "create_xbegin");

		void GenCreateXbegin(GenerateMethodContext ctx) {
			WriteMethod(ctx, () => GetAddressSizeThrowsDocs(ctx));
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		(string type, string text)[] GetAddressSizeThrowsDocs(in GenerateMethodContext ctx) {
			var arg = ctx.Method.Args[0];
			if (arg.Name != "addressSize" && arg.Name != "bitness")
				throw new InvalidOperationException();
			return new[] { ("ValueError", $"If `{idConverter.Argument(arg.Name)}` is not one of 16, 32, 64.") };
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenStringInstr(writer, method, methodBaseName);

		void GenStringInstr(FileWriter writer, CreateMethod method, string methodBaseName) {
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			var pythonName = idConverter.Method("Create" + methodBaseName);
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenStringInstr, rustName, pythonName);
		}

		void GenStringInstr(GenerateMethodContext ctx) {
			WriteMethod(ctx, () => GetAddressSizeThrowsDocs(ctx));
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenStringInstr(writer, method, methodBaseName);

		protected override void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenStringInstr(writer, method, methodBaseName);

		protected override void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) =>
			GenStringInstr(writer, method, methodBaseName);

		protected override void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) =>
			GenStringInstr(writer, method, methodBaseName);

		protected override void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code) =>
			GenStringInstr(writer, method, methodBaseName);

		protected override void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind) {
			var (rustName, pythonName) = kind switch {
				DeclareDataKind.Byte => (Rust.RustInstrCreateGenNames.with_declare_byte, "create_declare_byte"),
				DeclareDataKind.Word => (Rust.RustInstrCreateGenNames.with_declare_word, "create_declare_word"),
				DeclareDataKind.Dword => (Rust.RustInstrCreateGenNames.with_declare_dword, "create_declare_dword"),
				DeclareDataKind.Qword => (Rust.RustInstrCreateGenNames.with_declare_qword, "create_declare_qword"),
				_ => throw new InvalidOperationException(),
			};
			pythonName = pythonName + "_" + method.Args.Count.ToString();
			rustName = Rust.RustInstrCreateGenNames.AppendArgCount(rustName, method.Args.Count);
			// It can't fail since the 'db' feature is always enabled, but we must still call the try_xxx methods
			GenerateMethod(writer, method, canFail: true, isTryMethod: true, GenCreateDeclareData, rustName, pythonName);
		}

		void GenCreateDeclareData(GenerateMethodContext ctx) {
			ctx.Writer.WriteLine();
			WriteMethod(ctx, null);
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		void GenCreateDeclareDataSlice(FileWriter writer, CreateMethod method, int elemSize, string rustName, string pythonName) =>
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, ctx => GenCreateDeclareDataSlice(ctx, elemSize), rustName, pythonName);

		void GenCreateDeclareDataSlice(GenerateMethodContext ctx, int elemSize) {
			ctx.Writer.WriteLine();
			var errors = new[] {
				("ValueError", $"If `len({idConverter.Argument(ctx.Method.Args[0].Name)})` is not 1-{16 / elemSize}"),
				("TypeError", $"If `{idConverter.Argument(ctx.Method.Args[0].Name)}` is not a supported type"),
			};
			WriteMethod(ctx, () => errors);
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			switch (kind) {
			case DeclareDataKind.Byte:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.ByteArray:
					break;

				case ArrayType.ByteSlice:
					GenCreateDeclareDataSlice(writer, method, 1, Rust.RustInstrCreateGenNames.with_declare_byte, "create_declare_byte");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			default:
				break;
			}
		}

		protected override void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
		}
	}
}
