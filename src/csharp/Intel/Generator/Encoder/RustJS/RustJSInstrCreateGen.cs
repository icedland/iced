// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Generator.Documentation.Rust;
using Generator.Enums;
using Generator.IO;

namespace Generator.Encoder.RustJS {
	[Generator(TargetLanguage.RustJS)]
	sealed class RustJSInstrCreateGen : InstrCreateGen {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;
		readonly IdentifierConverter rustIdConverter;
		readonly RustDocCommentWriter docWriter;
		readonly Rust.InstrCreateGenImpl gen;
		readonly Rust.GenCreateNameArgs genNames;
		readonly StringBuilder sb;

		public RustJSInstrCreateGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = RustJSIdentifierConverter.Create();
			rustIdConverter = RustIdentifierConverter.Create();
			docWriter = new RustDocCommentWriter(idConverter, ".", ".", ".", ".");
			gen = new Rust.InstrCreateGenImpl(genTypes, idConverter, docWriter);
			genNames = new Rust.GenCreateNameArgs {
				CreatePrefix = "create",
				Register = "Reg",
				Memory = "Mem",
				Int32 = "I32",
				UInt32 = "U32",
				Int64 = "I64",
				UInt64 = "U64",
			};
			sb = new StringBuilder();
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.RustJS, "Create", generatorContext.Types.Dirs.GetRustJSFilename("instruction.rs"));

		readonly struct SplitArg {
			public readonly int OrigIndex;
			public readonly int NewIndexHi;
			public readonly int NewIndexLo;
			public SplitArg(int origIndex, int newIndexHi, int newIndexLo) {
				OrigIndex = origIndex;
				NewIndexHi = newIndexHi;
				NewIndexLo = newIndexLo;
			}
		}

		struct GenMethodContext {
			public readonly FileWriter Writer;
			public readonly CreateMethod OrigMethod;
			public readonly CreateMethod Method;
			public string? Attribute;

			public GenMethodContext(FileWriter writer, CreateMethod origMethod, CreateMethod method, string? attribute) {
				Writer = writer;
				OrigMethod = origMethod;
				Method = method;
				Attribute = attribute;
			}
		}

		void WriteDocs(in GenMethodContext ctx, Action? writeThrows = null) =>
			gen.WriteDocs(ctx.Writer, ctx.Method, "Throws", writeThrows);

		static CreateMethod CloneAndUpdateDocs(CreateMethod method) {
			var newMethod = new CreateMethod(method.Docs.ToArray());
			foreach (var arg in method.Args) {
				var doc = arg.Doc;
				switch (arg.Type) {
				case MethodArgType.Code:
					doc = $"{doc} (a #(r:Code)# enum value)";
					break;
				case MethodArgType.Register:
					doc = $"{doc} (a #(r:Register)# enum value)";
					break;
				case MethodArgType.RepPrefixKind:
					doc = $"{doc} (a #(r:RepPrefixKind)# enum value)";
					break;
				}
				newMethod.Args.Add(new MethodArg(doc, arg.Type, arg.Name, arg.DefaultValue));
			}
			return newMethod;
		}

		static void GenerateMethod(FileWriter writer, CreateMethod method, Action<GenMethodContext> genMethod) {
			method = CloneAndUpdateDocs(method);
			genMethod(new GenMethodContext(writer, method, method, null));
		}

		void WriteCall(in GenMethodContext ctx, string rustName, bool canFail, bool isTryMethod) {
			using (ctx.Writer.Indent()) {
				sb.Clear();
				if (canFail)
					sb.Append("Ok(");
				sb.Append("Self(iced_x86_rust::Instruction::");
				if (isTryMethod)
					sb.Append("try_");
				sb.Append(rustName);
				sb.Append('(');
				for (int i = 0; i < ctx.OrigMethod.Args.Count; i++) {
					if (i > 0)
						sb.Append(", ");

					var arg = ctx.OrigMethod.Args[i];
					var name = idConverter.Argument(arg.Name);

					switch (arg.Type) {
					case MethodArgType.Code:
						sb.Append($"code_to_iced({name})");
						break;
					case MethodArgType.Register:
						sb.Append($"register_to_iced({name})");
						break;
					case MethodArgType.RepPrefixKind:
						sb.Append($"rep_prefix_kind_to_iced({name})");
						break;
					case MethodArgType.Memory:
						sb.Append($"{name}.0");
						break;
					default:
						sb.Append(name);
						break;
					}
				}
				sb.Append(')');
				if (canFail)
					sb.Append(".map_err(to_js_error)?");
				sb.Append(')');
				if (canFail)
					sb.Append(')');
				ctx.Writer.WriteLine(sb.ToString());
			}
		}

		static void WriteMethodAttributes(in GenMethodContext ctx) {
			ctx.Writer.WriteLine("#[rustfmt::skip]");
			if (ctx.Attribute is string attr)
				ctx.Writer.WriteLine(attr);
		}

		void WriteMethod(in GenMethodContext ctx, string rustName, string jsName, bool canFail) {
			WriteMethodAttributes(ctx);
			ctx.Writer.WriteLine(string.Format(RustConstants.AttributeWasmBindgenJsName, jsName));
			ctx.Writer.Write($"pub fn {rustName}(");
			gen.WriteMethodDeclArgs(ctx.Writer, ctx.Method);
			if (canFail)
				ctx.Writer.WriteLine(") -> Result<Instruction, JsValue> {");
			else
				ctx.Writer.WriteLine(") -> Self {");
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group, int id) =>
			GenerateMethod(writer, method, GenCreate);

		void GenCreate(GenMethodContext ctx) {
			bool canFail = ctx.Method.Args.Count > 1;
			Action? writeThrows = null;
			if (canFail)
				writeThrows = () => docWriter.WriteLine(ctx.Writer, "Throws if one of the operands is invalid (basic checks)");
			WriteDocs(ctx, writeThrows);
			var rustJsName = gen.GetCreateName(ctx.OrigMethod, Rust.GenCreateNameArgs.RustNames);
			var rustName = Rust.InstrCreateGenImpl.GetRustOverloadedCreateName(ctx.OrigMethod);
			WriteMethod(ctx, rustJsName, gen.GetCreateName(ctx.OrigMethod, genNames), canFail);
			WriteCall(ctx, rustName, canFail, false);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, GenCreateBranch);

		void GenCreateBranch(GenMethodContext ctx) {
			const bool canFail = true;
			WriteDocs(ctx, () => docWriter.WriteLine(ctx.Writer, "Throws if the created instruction doesn't have a near branch operand"));
			const string rustName = Rust.RustInstrCreateGenNames.with_branch;
			WriteMethod(ctx, rustName, "createBranch", canFail);
			WriteCall(ctx, rustName, canFail, false);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, GenCreateFarBranch);

		void GenCreateFarBranch(GenMethodContext ctx) {
			const bool canFail = true;
			WriteDocs(ctx, () => docWriter.WriteLine(ctx.Writer, "Throws if the created instruction doesn't have a far branch operand"));
			const string rustName = Rust.RustInstrCreateGenNames.with_far_branch;
			WriteMethod(ctx, rustName, "createFarBranch", canFail);
			WriteCall(ctx, rustName, canFail, false);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, GenCreateXbegin);

		void GenCreateXbegin(GenMethodContext ctx) {
			const bool canFail = true;
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			const string rustName = Rust.RustInstrCreateGenNames.with_xbegin;
			WriteMethod(ctx, rustName, "createXbegin", canFail);
			WriteCall(ctx, rustName, canFail, false);
			ctx.Writer.WriteLine("}");
		}

		void WriteAddrSizeOrBitnessThrows(in GenMethodContext ctx) {
			var arg = ctx.OrigMethod.Args[0];
			if (arg.Name != "addressSize" && arg.Name != "bitness")
				throw new InvalidOperationException();
			docWriter.WriteLine(ctx.Writer, $"Throws if `{idConverter.Argument(arg.Name)}` is not one of 16, 32, 64.");
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenStringInstr(writer, method, methodBaseName);

		void GenStringInstr(FileWriter writer, CreateMethod method, string methodBaseName) =>
			GenerateMethod(writer, method, ctx => GenStringInstr(ctx, methodBaseName));

		void GenStringInstr(GenMethodContext ctx, string methodBaseName) {
			const bool canFail = true;
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			WriteMethod(ctx, rustName, idConverter.Method("Create" + methodBaseName), canFail);
			WriteCall(ctx, rustName, canFail, false);
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

		protected override void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind) =>
			GenerateMethod(writer, method, ctx => GenCreateDeclareData(ctx, kind));

		void GenCreateDeclareData(GenMethodContext ctx, DeclareDataKind kind) {
			const bool canFail = true;
			if (ctx.Method == ctx.OrigMethod)
				ctx.Writer.WriteLine();
			WriteDocs(ctx);
			var (rustName, jsName) = kind switch {
				DeclareDataKind.Byte => (Rust.RustInstrCreateGenNames.with_declare_byte, "createDeclareByte"),
				DeclareDataKind.Word => (Rust.RustInstrCreateGenNames.with_declare_word, "createDeclareWord"),
				DeclareDataKind.Dword => (Rust.RustInstrCreateGenNames.with_declare_dword, "createDeclareDword"),
				DeclareDataKind.Qword => (Rust.RustInstrCreateGenNames.with_declare_qword, "createDeclareQword"),
				_ => throw new InvalidOperationException(),
			};
			jsName = jsName + "_" + ctx.OrigMethod.Args.Count.ToString();
			rustName = Rust.RustInstrCreateGenNames.AppendArgCount(rustName, ctx.OrigMethod.Args.Count);
			WriteMethod(ctx, rustName, jsName, canFail);
			WriteCall(ctx, rustName, canFail, canFail);
			ctx.Writer.WriteLine("}");
		}

		void WriteDataThrows(in GenMethodContext ctx, string extra) =>
			docWriter.WriteLine(ctx.Writer, $"Throws if `{idConverter.Argument(ctx.OrigMethod.Args[0].Name)}.length` {extra}");

		void GenCreateDeclareDataSlice(FileWriter writer, CreateMethod method, int elemSize, string rustName, string jsName) =>
			GenerateMethod(writer, method, ctx => GenCreateDeclareDataSlice(ctx, elemSize, rustName, jsName));

		void GenCreateDeclareDataSlice(GenMethodContext ctx, int elemSize, string rustName, string jsName) {
			const bool canFail = true;
			ctx.Writer.WriteLine();
			WriteDocs(ctx, () => WriteDataThrows(ctx, $"is not 1-{16 / elemSize}"));
			WriteMethod(ctx, rustName, jsName, canFail);
			WriteCall(ctx, rustName, canFail, false);
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
					GenCreateDeclareDataSlice(writer, method, 1, Rust.RustInstrCreateGenNames.with_declare_byte, "createDeclareByte");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Word:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.WordPtr:
				case ArrayType.ByteArray:
				case ArrayType.WordArray:
				case ArrayType.ByteSlice:
					break;

				case ArrayType.WordSlice:
					GenCreateDeclareDataSlice(writer, method, 2, Rust.RustInstrCreateGenNames.with_declare_word, "createDeclareWord");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Dword:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.DwordPtr:
				case ArrayType.ByteArray:
				case ArrayType.DwordArray:
				case ArrayType.ByteSlice:
					break;

				case ArrayType.DwordSlice:
					GenCreateDeclareDataSlice(writer, method, 4, Rust.RustInstrCreateGenNames.with_declare_dword, "createDeclareDword");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Qword:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.QwordPtr:
				case ArrayType.ByteArray:
				case ArrayType.QwordArray:
				case ArrayType.ByteSlice:
					break;

				case ArrayType.QwordSlice:
					GenCreateDeclareDataSlice(writer, method, 8, Rust.RustInstrCreateGenNames.with_declare_qword, "createDeclareQword");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			default:
				throw new InvalidOperationException();
			}
		}

		protected override void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
		}
	}
}
