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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Documentation.Rust;
using Generator.Enums;
using Generator.IO;

namespace Generator.Encoder.RustJS {
	[Generator(TargetLanguage.RustJS, GeneratorNames.InstrCreateGen)]
	sealed class RustJSInstrCreateGen : InstrCreateGen {
		readonly IdentifierConverter idConverter;
		readonly IdentifierConverter rustIdConverter;
		readonly GeneratorOptions generatorOptions;
		readonly RustDocCommentWriter docWriter;
		readonly Rust.InstrCreateGenImpl gen;
		readonly Rust.GenCreateNameArgs genNames;
		readonly StringBuilder sb;

		public RustJSInstrCreateGen(GeneratorOptions generatorOptions) {
			idConverter = RustJSIdentifierConverter.Create();
			rustIdConverter = RustIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
			docWriter = new RustDocCommentWriter(idConverter, ".");
			gen = new Rust.InstrCreateGenImpl(idConverter, docWriter);
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
			(TargetLanguage.RustJS, "Create", Path.Combine(generatorOptions.RustJSDir, "instruction.rs"));

		struct SplitArg {
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
			public FileWriter Writer;
			public CreateMethod OrigMethod;
			public CreateMethod Method;
			public string? Attribute;
			public List<SplitArg> SplitArgs;

			public GenMethodContext(FileWriter writer, CreateMethod origMethod, CreateMethod method, string? attribute, List<SplitArg>? splitArgs) {
				Writer = writer;
				OrigMethod = origMethod;
				Method = method;
				Attribute = attribute;
				SplitArgs = splitArgs ?? new List<SplitArg>();
			}
		}

		void WriteDocs(in GenMethodContext ctx, Action? writeThrows = null) =>
			gen.WriteDocs(ctx.Writer, ctx.Method, "Throws", writeThrows);

		static bool TryCreateNo64Api(CreateMethod method, [NotNullWhen(true)] out CreateMethod? no64Method, [NotNullWhen(true)] out List<SplitArg>? splitArgs) {
			bool is64 = method.Args.Any(a => Rust.InstrCreateGenImpl.Is64BitArgument(a.Type));
			if (!is64) {
				no64Method = null;
				splitArgs = null;
				return false;
			}

			splitArgs = new List<SplitArg>();
			no64Method = new CreateMethod(method.Docs.ToArray());
			no64Method.Docs.Add(string.Empty);
			no64Method.Docs.Add("Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).");
			for (int i = 0; i < method.Args.Count; i++) {
				var arg = method.Args[i];
				if (Rust.InstrCreateGenImpl.Is64BitArgument(arg.Type)) {
					if (!(arg.DefaultValue is null))
						throw new InvalidOperationException();
					int newIndex = no64Method.Args.Count;
					splitArgs.Add(new SplitArg(i, newIndex, newIndex + 1));
					no64Method.Args.Add(new MethodArg($"High 32 bits ({arg.Doc})", MethodArgType.UInt32, arg.Name + "_hi", arg.DefaultValue));
					no64Method.Args.Add(new MethodArg($"Low 32 bits ({arg.Doc})", MethodArgType.UInt32, arg.Name + "_lo", arg.DefaultValue));
				}
				else
					no64Method.Args.Add(new MethodArg(arg.Doc, arg.Type, arg.Name, arg.DefaultValue));
			}
			return true;
		}

		CreateMethod CloneAndUpdateDocs(CreateMethod method) {
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

		// Some methods take an i64/u64 argument. That will translate to BigInt in JS but not all JS impls
		// support BigInt yet. Generate two methods, one with bigint and one with two u32 args. The 'bigint'
		// feature enables the i64/u64 method and disables the other one.
		void GenerateMethod(FileWriter writer, CreateMethod method, Action<GenMethodContext> genMethod) {
			method = CloneAndUpdateDocs(method);
			if (TryCreateNo64Api(method, out var no64Method, out var splitArgs)) {
				genMethod(new GenMethodContext(writer, method, method, RustConstants.FeatureBigInt, null));
				writer.WriteLine();
				genMethod(new GenMethodContext(writer, method, no64Method, RustConstants.FeatureNotBigInt, splitArgs));
			}
			else
				genMethod(new GenMethodContext(writer, method, method, null, null));
		}

		void WriteCall(in GenMethodContext ctx, string rustName) {
			using (ctx.Writer.Indent()) {
				var toLocalName = new Dictionary<int, string>();
				foreach (var info in ctx.SplitArgs) {
					var local = rustIdConverter.Argument(ctx.OrigMethod.Args[info.OrigIndex].Name);
					var argHi = idConverter.Argument(ctx.Method.Args[info.NewIndexHi].Name);
					var argLo = idConverter.Argument(ctx.Method.Args[info.NewIndexLo].Name);
					var expr = $"(({argHi} as u64) << 32) | ({argLo} as u64)";
					if (ctx.OrigMethod.Args[info.OrigIndex].Type == MethodArgType.Int64)
						expr = $"({expr}) as i64";
					ctx.Writer.WriteLine($"let {local} = {expr};");
					toLocalName.Add(info.OrigIndex, local);
				}
				sb.Clear();
				sb.Append("Self(iced_x86::Instruction::");
				sb.Append(rustName);
				sb.Append('(');
				for (int i = 0; i < ctx.OrigMethod.Args.Count; i++) {
					if (i > 0)
						sb.Append(", ");

					var arg = ctx.OrigMethod.Args[i];
					if (!toLocalName.TryGetValue(i, out var name))
						name = idConverter.Argument(arg.Name);

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
				sb.Append("))");
				ctx.Writer.WriteLine(sb.ToString());
			}
		}

		void WriteMethodAttributes(in GenMethodContext ctx) {
			ctx.Writer.WriteLine(RustConstants.AttributeNoRustFmt);
			if (ctx.Attribute is string attr)
				ctx.Writer.WriteLine(attr);
		}

		void WriteMethod(in GenMethodContext ctx, string rustName, string jsName) {
			WriteMethodAttributes(ctx);
			ctx.Writer.WriteLine(string.Format(RustConstants.AttributeWasmBindgenJsName, jsName));
			ctx.Writer.Write($"pub fn {rustName}(");
			gen.WriteMethodDeclArgs(ctx.Writer, ctx.Method);
			ctx.Writer.WriteLine(") -> Self {");
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group) =>
			GenerateMethod(writer, method, GenCreate);

		void GenCreate(GenMethodContext ctx) {
			Action? writeThrows = null;
			if (Rust.InstrCreateGenImpl.HasImmediateArg_8_16_32(ctx.OrigMethod))
				writeThrows = () => docWriter.WriteLine(ctx.Writer, $"Throws if the immediate is invalid");
			WriteDocs(ctx, writeThrows);
			var rustName = gen.GetCreateName(ctx.OrigMethod, Rust.GenCreateNameArgs.RustNames);
			WriteMethod(ctx, rustName, gen.GetCreateName(ctx.OrigMethod, genNames));
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, GenCreateBranch);

		void GenCreateBranch(GenMethodContext ctx) {
			WriteDocs(ctx);
			const string rustName = "with_branch";
			WriteMethod(ctx, rustName, "createBranch");
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, GenCreateFarBranch);

		void GenCreateFarBranch(GenMethodContext ctx) {
			WriteDocs(ctx);
			const string rustName = "with_far_branch";
			WriteMethod(ctx, rustName, "createFarBranch");
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, GenCreateXbegin);

		void GenCreateXbegin(GenMethodContext ctx) {
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			const string rustName = "with_xbegin";
			WriteMethod(ctx, rustName, "createXbegin");
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateMemory64(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, GenCreateMemory64);

		void GenCreateMemory64(GenMethodContext ctx) {
			var (rustName, jsName) = ctx.OrigMethod.Args[1].Type switch {
				MethodArgType.UInt64 => ("with_mem64_reg", "createMem64Reg"),
				_ => ("with_reg_mem64", "createRegMem64"),
			};
			WriteDocs(ctx);
			WriteMethod(ctx, rustName, jsName);
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		void WriteAddrSizeOrBitnessThrows(in GenMethodContext ctx) {
			var arg = ctx.OrigMethod.Args[0];
			if (arg.Name != "addressSize" && arg.Name != "bitness")
				throw new InvalidOperationException();
			docWriter.WriteLine(ctx.Writer, $"Throws if `{idConverter.Argument(arg.Name)}` is not one of 16, 32, 64.");
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenerateMethod(writer, method, ctx => GenCreateString_Reg_SegRSI(ctx, methodBaseName));

		void GenCreateString_Reg_SegRSI(GenMethodContext ctx, string methodBaseName) {
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			WriteMethod(ctx, rustName, idConverter.Method("Create" + methodBaseName));
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenerateMethod(writer, method, ctx => GenCreateString_Reg_ESRDI(ctx, methodBaseName));

		void GenCreateString_Reg_ESRDI(GenMethodContext ctx, string methodBaseName) {
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			WriteMethod(ctx, rustName, idConverter.Method("Create" + methodBaseName));
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenerateMethod(writer, method, ctx => GenCreateString_ESRDI_Reg(ctx, methodBaseName));

		void GenCreateString_ESRDI_Reg(GenMethodContext ctx, string methodBaseName) {
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			WriteMethod(ctx, rustName, idConverter.Method("Create" + methodBaseName));
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) =>
			GenerateMethod(writer, method, ctx => GenCreateString_SegRSI_ESRDI(ctx, methodBaseName));

		void GenCreateString_SegRSI_ESRDI(GenMethodContext ctx, string methodBaseName) {
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			WriteMethod(ctx, rustName, idConverter.Method("Create" + methodBaseName));
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) =>
			GenerateMethod(writer, method, ctx => GenCreateString_ESRDI_SegRSI(ctx, methodBaseName));

		void GenCreateString_ESRDI_SegRSI(GenMethodContext ctx, string methodBaseName) {
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			WriteMethod(ctx, rustName, idConverter.Method("Create" + methodBaseName));
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code) =>
			GenerateMethod(writer, method, ctx => GenCreateMaskmov(ctx, methodBaseName));

		void GenCreateMaskmov(GenMethodContext ctx, string methodBaseName) {
			WriteDocs(ctx, () => WriteAddrSizeOrBitnessThrows(ctx));
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			WriteMethod(ctx, rustName, idConverter.Method("Create" + methodBaseName));
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind) =>
			GenerateMethod(writer, method, ctx => GenCreateDeclareData(ctx, kind));

		void GenCreateDeclareData(GenMethodContext ctx, DeclareDataKind kind) {
			if (ctx.Method == ctx.OrigMethod)
				ctx.Writer.WriteLine();
			WriteDocs(ctx);
			var (rustName, jsName) = kind switch {
				DeclareDataKind.Byte => ("with_declare_byte", "createDeclareByte"),
				DeclareDataKind.Word => ("with_declare_word", "createDeclareWord"),
				DeclareDataKind.Dword => ("with_declare_dword", "createDeclareDword"),
				DeclareDataKind.Qword => ("with_declare_qword", "createDeclareQword"),
				_ => throw new InvalidOperationException(),
			};
			jsName = jsName + "_" + ctx.OrigMethod.Args.Count.ToString();
			rustName = rustName + "_" + ctx.OrigMethod.Args.Count.ToString();
			WriteMethod(ctx, rustName, jsName);
			WriteCall(ctx, rustName);
			ctx.Writer.WriteLine("}");
		}

		void WriteDataThrows(in GenMethodContext ctx, string extra) =>
			docWriter.WriteLine(ctx.Writer, $"Throws if `{idConverter.Argument(ctx.OrigMethod.Args[0].Name)}.length` {extra}");

		void GenCreateDeclareDataSlice(FileWriter writer, CreateMethod method, int elemSize, string rustName, string jsName) =>
			GenerateMethod(writer, method, ctx => GenCreateDeclareDataSlice(ctx, elemSize, rustName, jsName));

		void GenCreateDeclareDataSlice(GenMethodContext ctx, int elemSize, string rustName, string jsName) {
			// &[u64] isn't supported if bigint feature is disabled
			if (elemSize == 8) {
				if (!(ctx.Attribute is null))
					throw new InvalidOperationException();
				ctx.Attribute = RustConstants.FeatureBigInt;
			}

			ctx.Writer.WriteLine();
			WriteDocs(ctx, () => WriteDataThrows(ctx, $"is not 1-{16 / elemSize}"));
			WriteMethod(ctx, rustName, jsName);
			WriteCall(ctx, rustName);
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
					GenCreateDeclareDataSlice(writer, method, 1, "with_declare_byte", "createDeclareByte");
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
					GenCreateDeclareDataSlice(writer, method, 2, "with_declare_word", "createDeclareWord");
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
					GenCreateDeclareDataSlice(writer, method, 4, "with_declare_dword", "createDeclareDword");
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
					GenCreateDeclareDataSlice(writer, method, 8, "with_declare_qword", "createDeclareQword");
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
