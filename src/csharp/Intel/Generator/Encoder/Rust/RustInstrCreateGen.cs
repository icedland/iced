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
using System.IO;
using System.Linq;
using Generator.Documentation.Rust;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Encoder.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustInstrCreateGen : InstrCreateGen {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;
		readonly RustDocCommentWriter docWriter;
		readonly InstrCreateGenImpl gen;
		readonly GenCreateNameArgs genNames;

		public RustInstrCreateGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = RustIdentifierConverter.Create();
			docWriter = new RustDocCommentWriter(idConverter);
			gen = new InstrCreateGenImpl(genTypes, idConverter, docWriter);
			genNames = GenCreateNameArgs.RustNames;
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.Rust, "Create", Path.Combine(generatorContext.Types.Dirs.RustDir, "instruction.rs"));

		void WriteDocs(FileWriter writer, CreateMethod method, Action? writePanics = null) =>
			gen.WriteDocs(writer, method, "Panics", writePanics);

		void WriteMethodAttributes(FileWriter writer, CreateMethod method, bool inline) {
			writer.WriteLine(RustConstants.AttributeMustUse);
			writer.WriteLine(inline ? RustConstants.AttributeInline : RustConstants.AttributeAllowMissingInlineInPublicItems);
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
		}

		void WriteMethod(FileWriter writer, CreateMethod method, string name) {
			WriteMethodAttributes(writer, method, false);
			writer.Write($"pub fn {name}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
		}

		void WriteInitializeInstruction(FileWriter writer, CreateMethod method) {
			writer.WriteLine("let mut instruction = Self::default();");
			var args = method.Args;
			if (args.Count == 0 || args[0].Type != MethodArgType.Code)
				throw new InvalidOperationException();
			var codeName = idConverter.Argument(args[0].Name);
			writer.WriteLine($"super::instruction_internal::internal_set_code(&mut instruction, {codeName});");
		}

		void WriteInitializeInstruction(FileWriter writer, EnumValue code) {
			writer.WriteLine("let mut instruction = Self::default();");
			writer.WriteLine($"super::instruction_internal::internal_set_code(&mut instruction, {code.DeclaringType.Name(idConverter)}::{code.Name(idConverter)});");
		}

		void WriteMethodFooter(FileWriter writer, int count) {
			writer.WriteLine();
			writer.WriteLine($"debug_assert_eq!({count}, instruction.op_count());");
			writer.WriteLine("instruction");
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group) {
			Action? writePanics = null;
			if (InstrCreateGenImpl.HasImmediateArg_8_16_32(method))
				writePanics = () => docWriter.WriteLine(writer, $"Panics if the immediate is invalid");
			WriteDocs(writer, method, writePanics);
			WriteMethod(writer, method, gen.GetCreateName(method, genNames));
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				var args = method.Args;
				var codeName = idConverter.Argument(args[0].Name);
				var opKindStr = genTypes[TypeIds.OpKind].Name(idConverter);
				var registerStr = genTypes[TypeIds.OpKind][nameof(OpKind.Register)].Name(idConverter);
				var memoryStr = genTypes[TypeIds.OpKind][nameof(OpKind.Memory)].Name(idConverter);
				var immediate64Str = genTypes[TypeIds.OpKind][nameof(OpKind.Immediate64)].Name(idConverter);
				var immediate8_2ndStr = genTypes[TypeIds.OpKind][nameof(OpKind.Immediate8_2nd)].Name(idConverter);
				bool multipleInts = args.Where(a => a.Type == MethodArgType.Int32 || a.Type == MethodArgType.UInt32).Count() > 1;
				string methodName;
				for (int i = 1; i < args.Count; i++) {
					int op = i - 1;
					var arg = args[i];
					writer.WriteLine();
					switch (arg.Type) {
					case MethodArgType.Register:
						writer.WriteLine($"const_assert_eq!(0, {opKindStr}::{registerStr} as u32);");
						writer.WriteLine($"//super::instruction_internal::internal_set_op{op}_kind(&mut instruction, {opKindStr}::{registerStr});");
						writer.WriteLine($"super::instruction_internal::internal_set_op{op}_register(&mut instruction, {idConverter.Argument(arg.Name)});");
						break;

					case MethodArgType.Memory:
						writer.WriteLine($"super::instruction_internal::internal_set_op{op}_kind(&mut instruction, {opKindStr}::{memoryStr});");
						writer.WriteLine($"super::instruction_internal::internal_set_memory_base(&mut instruction, {idConverter.Argument(arg.Name)}.base);");
						writer.WriteLine($"super::instruction_internal::internal_set_memory_index(&mut instruction, {idConverter.Argument(arg.Name)}.index);");
						writer.WriteLine($"instruction.set_memory_index_scale({idConverter.Argument(arg.Name)}.scale);");
						writer.WriteLine($"instruction.set_memory_displ_size({idConverter.Argument(arg.Name)}.displ_size);");
						writer.WriteLine($"instruction.set_memory_displacement({idConverter.Argument(arg.Name)}.displacement as u32);");
						writer.WriteLine($"instruction.set_is_broadcast({idConverter.Argument(arg.Name)}.is_broadcast);");
						writer.WriteLine($"instruction.set_segment_prefix({idConverter.Argument(arg.Name)}.segment_prefix);");
						break;

					case MethodArgType.Int32:
					case MethodArgType.UInt32:
						methodName = arg.Type == MethodArgType.Int32 ? "initialize_signed_immediate" : "initialize_unsigned_immediate";
						var castType = arg.Type == MethodArgType.Int32 ? " as i64" : " as u64";
						writer.WriteLine($"super::instruction_internal::{methodName}(&mut instruction, {op}, {idConverter.Argument(arg.Name)}{castType});");
						break;

					case MethodArgType.Int64:
					case MethodArgType.UInt64:
						methodName = arg.Type == MethodArgType.Int64 ? "initialize_signed_immediate" : "initialize_unsigned_immediate";
						writer.WriteLine($"super::instruction_internal::{methodName}(&mut instruction, {op}, {idConverter.Argument(arg.Name)});");
						break;

					case MethodArgType.Code:
					case MethodArgType.RepPrefixKind:
					case MethodArgType.UInt8:
					case MethodArgType.UInt16:
					case MethodArgType.PreferedInt32:
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
			WriteMethod(writer, method, "with_branch");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				writer.WriteLine();
				writer.WriteLine($"super::instruction_internal::internal_set_op0_kind(&mut instruction, super::instruction_internal::get_near_branch_op_kind({idConverter.Argument(method.Args[0].Name)}, 0));");
				writer.WriteLine($"instruction.set_near_branch64({idConverter.Argument(method.Args[1].Name)});");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 3)
				throw new InvalidOperationException();
			WriteDocs(writer, method);
			WriteMethod(writer, method, "with_far_branch");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				writer.WriteLine();
				writer.WriteLine($"super::instruction_internal::internal_set_op0_kind(&mut instruction, super::instruction_internal::get_far_branch_op_kind({idConverter.Argument(method.Args[0].Name)}, 0));");
				writer.WriteLine($"instruction.set_far_branch_selector({idConverter.Argument(method.Args[1].Name)});");
				writer.WriteLine($"instruction.set_far_branch32({idConverter.Argument(method.Args[2].Name)});");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 2)
				throw new InvalidOperationException();
			WriteDocs(writer, method, () => WriteAddrSizeOrBitnessPanic(writer, method));
			WriteMethod(writer, method, "with_xbegin");
			using (writer.Indent()) {
				writer.WriteLine($"let mut instruction = Self::default();");
				var bitnessName = idConverter.Argument(method.Args[0].Name);
				var opKindName = genTypes[TypeIds.OpKind].Name(idConverter);
				var codeName = codeType.Name(idConverter);
				writer.WriteLine();
				writer.WriteLine($"match bitness {{");
				writer.WriteLine($"	16 => {{");
				writer.WriteLine($"		super::instruction_internal::internal_set_code(&mut instruction, {codeName}::{codeType[nameof(Code.Xbegin_rel16)].Name(idConverter)});");
				writer.WriteLine($"		super::instruction_internal::internal_set_op0_kind(&mut instruction, {opKindName}::{genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch32)].Name(idConverter)});");
				writer.WriteLine($"		instruction.set_near_branch32({idConverter.Argument(method.Args[1].Name)} as u32);");
				writer.WriteLine($"	}}");
				writer.WriteLine();
				writer.WriteLine($"	32 => {{");
				writer.WriteLine($"		super::instruction_internal::internal_set_code(&mut instruction, {codeName}::{codeType[nameof(Code.Xbegin_rel32)].Name(idConverter)});");
				writer.WriteLine($"		super::instruction_internal::internal_set_op0_kind(&mut instruction, {opKindName}::{genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch32)].Name(idConverter)});");
				writer.WriteLine($"		instruction.set_near_branch32({idConverter.Argument(method.Args[1].Name)} as u32);");
				writer.WriteLine($"	}}");
				writer.WriteLine();
				writer.WriteLine($"	64 => {{");
				writer.WriteLine($"		super::instruction_internal::internal_set_code(&mut instruction, {codeName}::{codeType[nameof(Code.Xbegin_rel32)].Name(idConverter)});");
				writer.WriteLine($"		super::instruction_internal::internal_set_op0_kind(&mut instruction, {opKindName}::{genTypes[TypeIds.OpKind][nameof(OpKind.NearBranch64)].Name(idConverter)});");
				writer.WriteLine($"		instruction.set_near_branch64({idConverter.Argument(method.Args[1].Name)});");
				writer.WriteLine($"	}}");
				writer.WriteLine();
				writer.WriteLine($"	_ => panic!(),");
				writer.WriteLine($"}}");
				WriteMethodFooter(writer, 1);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateMemory64(FileWriter writer, CreateMethod method) {
			if (method.Args.Count != 4)
				throw new InvalidOperationException();

			int memOp, regOp;
			string name;
			if (method.Args[1].Type == MethodArgType.UInt64) {
				memOp = 0;
				regOp = 1;
				name = "with_mem64_reg";
			}
			else {
				memOp = 1;
				regOp = 0;
				name = "with_reg_mem64";
			}

			WriteDocs(writer, method);
			WriteMethod(writer, method, name);
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				writer.WriteLine();

				var mem64Str = genTypes[TypeIds.OpKind][nameof(OpKind.Memory64)].Name(idConverter);
				writer.WriteLine($"super::instruction_internal::internal_set_op{memOp}_kind(&mut instruction, {genTypes[TypeIds.OpKind].Name(idConverter)}::{mem64Str});");
				writer.WriteLine($"instruction.set_memory_address64({idConverter.Argument(method.Args[1 + memOp].Name)});");
				writer.WriteLine("super::instruction_internal::internal_set_memory_displ_size(&mut instruction, 4);");
				writer.WriteLine($"instruction.set_segment_prefix({idConverter.Argument(method.Args[3].Name)});");

				writer.WriteLine();
				var opKindStr = genTypes[TypeIds.OpKind].Name(idConverter);
				var registerStr = genTypes[TypeIds.OpKind][nameof(OpKind.Register)].Name(idConverter);
				writer.WriteLine($"const_assert_eq!(0, {opKindStr}::{registerStr} as u32);");
				writer.WriteLine($"//super::instruction_internal::internal_set_op{regOp}_kind(&mut instruction, {opKindStr}::{registerStr});");
				writer.WriteLine($"super::instruction_internal::internal_set_op{regOp}_register(&mut instruction, {idConverter.Argument(method.Args[1 + regOp].Name)});");

				WriteMethodFooter(writer, 2);
			}
			writer.WriteLine("}");
		}

		void WriteComma(FileWriter writer) => writer.Write(", ");
		void Write(FileWriter writer, EnumValue value) => writer.Write($"{value.DeclaringType.Name(idConverter)}::{value.Name(idConverter)}");
		void Write(FileWriter writer, MethodArg arg) => writer.Write(idConverter.Argument(arg.Name));

		void WriteAddrSizeOrBitnessPanic(FileWriter writer, CreateMethod method) {
			var arg = method.Args[0];
			if (arg.Name != "addressSize" && arg.Name != "bitness")
				throw new InvalidOperationException();
			docWriter.WriteLine(writer, $"Panics if `{idConverter.Argument(arg.Name)}` is not one of 16, 32, 64.");
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method, () => WriteAddrSizeOrBitnessPanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				writer.Write("super::instruction_internal::with_string_reg_segrsi(");
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
				writer.WriteLine(")");
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateString_Reg_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method, () => WriteAddrSizeOrBitnessPanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				writer.Write("super::instruction_internal::with_string_reg_esrdi(");
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
				writer.WriteLine(")");
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateString_ESRDI_Reg(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method, () => WriteAddrSizeOrBitnessPanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				writer.Write("super::instruction_internal::with_string_esrdi_reg(");
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
				writer.WriteLine(")");
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateString_SegRSI_ESRDI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method, () => WriteAddrSizeOrBitnessPanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				writer.Write("super::instruction_internal::with_string_segrsi_esrdi(");
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
				writer.WriteLine(")");
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateString_ESRDI_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method, () => WriteAddrSizeOrBitnessPanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				writer.Write("super::instruction_internal::with_string_esrdi_segrsi(");
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
				writer.WriteLine(")");
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateMaskmov(FileWriter writer, CreateMethod method, string methodBaseName, EnumValue code) {
			WriteDocs(writer, method, () => WriteAddrSizeOrBitnessPanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				writer.Write("super::instruction_internal::with_maskmov(");
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
				writer.WriteLine(")");
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
				setValueName = "set_declare_byte_value";
				methodName = "with_declare_byte";
				break;

			case DeclareDataKind.Word:
				code = codeType[nameof(Code.DeclareWord)];
				setValueName = "set_declare_word_value";
				methodName = "with_declare_word";
				break;

			case DeclareDataKind.Dword:
				code = codeType[nameof(Code.DeclareDword)];
				setValueName = "set_declare_dword_value";
				methodName = "with_declare_dword";
				break;

			case DeclareDataKind.Qword:
				code = codeType[nameof(Code.DeclareQword)];
				setValueName = "set_declare_qword_value";
				methodName = "with_declare_qword";
				break;

			default:
				throw new InvalidOperationException();
			}
			methodName = methodName + "_" + method.Args.Count.ToString();

			writer.WriteLine();
			WriteDocs(writer, method, () => WriteDeclareDataPanic(writer));
			WriteMethodAttributes(writer, method, false);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, code);
				writer.WriteLine($"super::instruction_internal::internal_set_declare_data_len(&mut instruction, {method.Args.Count});");
				writer.WriteLine();
				for (int i = 0; i < method.Args.Count; i++)
					writer.WriteLine($"instruction.{setValueName}({i}, {idConverter.Argument(method.Args[i].Name)});");
				WriteMethodFooter(writer, 0);
			}
			writer.WriteLine("}");
		}

		const string dbPanicMsg = "Panics if `db` feature wasn't enabled";
		void WriteDeclareDataPanic(FileWriter writer) =>
			docWriter.WriteLine(writer, dbPanicMsg);

		void WriteDataPanic(FileWriter writer, CreateMethod method, string extra) {
			docWriter.WriteLine(writer, $"- Panics if `{idConverter.Argument(method.Args[0].Name)}.len()` {extra}");
			docWriter.WriteLine(writer, $"- {dbPanicMsg}");
		}

		void GenCreateDeclareDataSlice(FileWriter writer, CreateMethod method, int elemSize, EnumValue code, string methodName, string setDeclValueName) {
			writer.WriteLine();
			WriteDocs(writer, method, () => WriteDataPanic(writer, method, $"is not 1-{16 / elemSize}"));
			WriteMethodAttributes(writer, method, false);
			writer.Write($"pub fn {methodName}(");
			gen.WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
			using (writer.Indent()) {
				var dataName = idConverter.Argument(method.Args[0].Name);
				writer.WriteLine($"if {dataName}.len().wrapping_sub(1) > {16 / elemSize} - 1 {{");
				using (writer.Indent())
					writer.WriteLine("panic!();");
				writer.WriteLine("}");
				writer.WriteLine();
				WriteInitializeInstruction(writer, code);
				writer.WriteLine($"super::instruction_internal::internal_set_declare_data_len(&mut instruction, {dataName}.len() as u32);");
				writer.WriteLine();
				writer.WriteLine($"for i in {dataName}.iter().enumerate() {{");
				using (writer.Indent())
					writer.WriteLine($"instruction.{setDeclValueName}(i.0, *i.1);");
				writer.WriteLine("}");
				WriteMethodFooter(writer, 0);
			}
			writer.WriteLine("}");
		}

		protected override void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) {
			switch (kind) {
			case DeclareDataKind.Byte:
				switch (arrayType) {
				case ArrayType.BytePtr:
				case ArrayType.ByteArray:
					break;

				case ArrayType.ByteSlice:
					GenCreateDeclareDataSlice(writer, method, 1, codeType[nameof(Code.DeclareByte)], "with_declare_byte", "set_declare_byte_value");
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
					break;

				case ArrayType.ByteSlice:
					writer.WriteLine();
					WriteDocs(writer, method, () => WriteDataPanic(writer, method, $"is not 2-16 or not a multiple of 2"));
					WriteMethodAttributes(writer, method, false);
					writer.WriteLine(RustConstants.AttributeAllowTrivialCasts);
					writer.WriteLine(RustConstants.AttributeAllowCastPtrAlignment);
					writer.Write($"pub fn with_declare_word_slice_u8(");
					gen.WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") -> Self {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if {dataName}.len().wrapping_sub(1) > 16 - 1 || ({dataName}.len() & 1) != 0 {{");
						using (writer.Indent())
							writer.WriteLine("panic!();");
						writer.WriteLine("}");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareWord)]);
						writer.WriteLine($"super::instruction_internal::internal_set_declare_data_len(&mut instruction, {dataName}.len() as u32 / 2);");
						writer.WriteLine();
						writer.WriteLine($"for i in 0..{dataName}.len() / 2 {{");
						using (writer.Indent()) {
							writer.WriteLine($"let v = unsafe {{ u16::from_le(ptr::read_unaligned({dataName}.get_unchecked(i * 2) as *const _ as *const u16)) }};");
							writer.WriteLine("instruction.set_declare_word_value(i, v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.WordSlice:
					GenCreateDeclareDataSlice(writer, method, 2, codeType[nameof(Code.DeclareWord)], "with_declare_word", "set_declare_word_value");
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
					break;

				case ArrayType.ByteSlice:
					writer.WriteLine();
					WriteDocs(writer, method, () => WriteDataPanic(writer, method, $"is not 4-16 or not a multiple of 4"));
					WriteMethodAttributes(writer, method, false);
					writer.WriteLine(RustConstants.AttributeAllowTrivialCasts);
					writer.WriteLine(RustConstants.AttributeAllowCastPtrAlignment);
					writer.Write($"pub fn with_declare_dword_slice_u8(");
					gen.WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") -> Self {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if {dataName}.len().wrapping_sub(1) > 16 - 1 || ({dataName}.len() & 3) != 0 {{");
						using (writer.Indent())
							writer.WriteLine("panic!();");
						writer.WriteLine("}");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareDword)]);
						writer.WriteLine($"super::instruction_internal::internal_set_declare_data_len(&mut instruction, {dataName}.len() as u32 / 4);");
						writer.WriteLine();
						writer.WriteLine($"for i in 0..{dataName}.len() / 4 {{");
						using (writer.Indent()) {
							writer.WriteLine($"let v = unsafe {{ u32::from_le(ptr::read_unaligned({dataName}.get_unchecked(i * 4) as *const _ as *const u32)) }};");
							writer.WriteLine("instruction.set_declare_dword_value(i, v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.DwordSlice:
					GenCreateDeclareDataSlice(writer, method, 4, codeType[nameof(Code.DeclareDword)], "with_declare_dword", "set_declare_dword_value");
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
					break;

				case ArrayType.ByteSlice:
					writer.WriteLine();
					WriteDocs(writer, method, () => WriteDataPanic(writer, method, $"is not 8-16 or not a multiple of 8"));
					WriteMethodAttributes(writer, method, false);
					writer.WriteLine(RustConstants.AttributeAllowTrivialCasts);
					writer.WriteLine(RustConstants.AttributeAllowCastPtrAlignment);
					writer.Write($"pub fn with_declare_qword_slice_u8(");
					gen.WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") -> Self {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if {dataName}.len().wrapping_sub(1) > 16 - 1 || ({dataName}.len() & 7) != 0 {{");
						using (writer.Indent())
							writer.WriteLine("panic!();");
						writer.WriteLine("}");
						writer.WriteLine();
						WriteInitializeInstruction(writer, codeType[nameof(Code.DeclareQword)]);
						writer.WriteLine($"super::instruction_internal::internal_set_declare_data_len(&mut instruction, {dataName}.len() as u32 / 8);");
						writer.WriteLine();
						writer.WriteLine($"for i in 0..{dataName}.len() / 8 {{");
						using (writer.Indent()) {
							writer.WriteLine($"let v = unsafe {{ u64::from_le(ptr::read_unaligned({dataName}.get_unchecked(i * 8) as *const _ as *const u64)) }};");
							writer.WriteLine("instruction.set_declare_qword_value(i, v);");
						}
						writer.WriteLine("}");
						WriteMethodFooter(writer, 0);
					}
					writer.WriteLine("}");
					break;

				case ArrayType.QwordSlice:
					GenCreateDeclareDataSlice(writer, method, 8, codeType[nameof(Code.DeclareQword)], "with_declare_qword", "set_declare_qword_value");
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
