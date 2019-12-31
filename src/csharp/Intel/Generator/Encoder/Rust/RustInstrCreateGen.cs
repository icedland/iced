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
using System.Text;
using Generator.Documentation.Rust;
using Generator.Enums;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Encoder.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.InstrCreateGen)]
	sealed class RustInstrCreateGen : InstrCreateGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;
		readonly RustDocCommentWriter docWriter;
		readonly StringBuilder sb;

		public RustInstrCreateGen(GeneratorOptions generatorOptions) {
			idConverter = RustIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
			docWriter = new RustDocCommentWriter(idConverter);
			sb = new StringBuilder();
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.Rust, "Create", Path.Combine(generatorOptions.RustDir, "instruction.rs"));

		void WriteDocs(FileWriter writer, CreateMethod method, Action? writePanics = null) {
			const string typeName = "Instruction";
			docWriter.BeginWrite(writer);
			docWriter.WriteDocLine(writer, method.Doc, typeName);
			docWriter.WriteLine(writer, string.Empty);
			if (!(writePanics is null)) {
				docWriter.WriteLine(writer, "# Panics");
				docWriter.WriteLine(writer, string.Empty);
				writePanics();
				docWriter.WriteLine(writer, string.Empty);
			}
			docWriter.WriteLine(writer, "# Arguments");
			docWriter.WriteLine(writer, string.Empty);
			for (int i = 0; i < method.Args.Count; i++) {
				var arg = method.Args[i];
				docWriter.Write($"* `{idConverter.Argument(arg.Name)}`: ");
				docWriter.WriteDocLine(writer, arg.Doc, typeName);
			}
			docWriter.EndWrite(writer);
		}

		void WriteMethodDeclArgs(FileWriter writer, CreateMethod method) {
			bool comma = false;
			foreach (var arg in method.Args) {
				if (comma)
					writer.Write(", ");
				comma = true;
				writer.Write(idConverter.Argument(arg.Name));
				writer.Write(": ");
				switch (arg.Type) {
				case MethodArgType.Code:
					writer.Write(CodeEnum.Instance.Name(idConverter));
					break;
				case MethodArgType.Register:
					writer.Write(RegisterEnum.Instance.Name(idConverter));
					break;
				case MethodArgType.RepPrefixKind:
					writer.Write(RepPrefixKindEnum.Instance.Name(idConverter));
					break;
				case MethodArgType.Memory:
					writer.Write("&MemoryOperand");
					break;
				case MethodArgType.UInt8:
					writer.Write("u8");
					break;
				case MethodArgType.UInt16:
					writer.Write("u16");
					break;
				case MethodArgType.Int32:
					writer.Write("i32");
					break;
				case MethodArgType.PreferedInt32:
				case MethodArgType.UInt32:
					writer.Write("u32");
					break;
				case MethodArgType.Int64:
					writer.Write("i64");
					break;
				case MethodArgType.UInt64:
					writer.Write("u64");
					break;
				case MethodArgType.ArrayIndex:
				case MethodArgType.ArrayLength:
					writer.Write("usize");
					break;
				case MethodArgType.ByteSlice:
					writer.Write("&[u8]");
					break;
				case MethodArgType.WordSlice:
					writer.Write("&[u16]");
					break;
				case MethodArgType.DwordSlice:
					writer.Write("&[u32]");
					break;
				case MethodArgType.QwordSlice:
					writer.Write("&[u64]");
					break;
				case MethodArgType.ByteArray:
				case MethodArgType.WordArray:
				case MethodArgType.DwordArray:
				case MethodArgType.QwordArray:
				default:
					throw new InvalidOperationException();
				}
			}
		}

		string GetCreateName(CreateMethod method) {
			if (method.Args.Count == 0 || method.Args[0].Type != MethodArgType.Code)
				throw new InvalidOperationException();

			sb.Clear();
			sb.Append("with");
			var args = method.Args;
			for (int i = 1; i < args.Count; i++) {
				var arg = args[i];
				switch (arg.Type) {
				case MethodArgType.Register:
					sb.Append("_reg");
					break;
				case MethodArgType.Memory:
					sb.Append("_mem");
					break;
				case MethodArgType.Int32:
					sb.Append("_i32");
					break;
				case MethodArgType.UInt32:
					sb.Append("_u32");
					break;
				case MethodArgType.Int64:
					sb.Append("_i64");
					break;
				case MethodArgType.UInt64:
					sb.Append("_u64");
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
				default:
					throw new InvalidOperationException();
				}
			}

			return sb.ToString();
		}

		void WriteMethodAttributes(FileWriter writer, CreateMethod method, bool inline) {
			writer.WriteLine(RustConstants.AttributeMustUse);
			writer.WriteLine(inline ? RustConstants.AttributeInline : RustConstants.AttributeAllowMissingInlineInPublicItems);
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			if (method.Args.Count > 7)
				writer.WriteLine(RustConstants.AttributeAllowTooManyArguments);
		}

		void WriteMethod(FileWriter writer, CreateMethod method, string name) {
			WriteMethodAttributes(writer, method, false);
			writer.Write($"pub fn {name}(");
			WriteMethodDeclArgs(writer, method);
			writer.WriteLine(") -> Self {");
		}

		void WriteInitializeInstruction(FileWriter writer, CreateMethod method) {
			writer.WriteLine("let mut instruction = Self::default();");
			var args = method.Args;
			if (args.Count == 0 || args[0].Type != MethodArgType.Code)
				throw new InvalidOperationException();
			var codeName = idConverter.Argument(args[0].Name);
			writer.WriteLine($"instruction_internal::internal_set_code(&mut instruction, {codeName});");
		}

		void WriteInitializeInstruction(FileWriter writer, EnumValue code) {
			writer.WriteLine("let mut instruction = Self::default();");
			writer.WriteLine($"instruction_internal::internal_set_code(&mut instruction, {code.DeclaringType.Name(idConverter)}::{code.Name(idConverter)});");
		}

		void WriteMethodFooter(FileWriter writer, int count) {
			writer.WriteLine();
			writer.WriteLine($"debug_assert_eq!({count}, instruction.op_count());");
			writer.WriteLine("instruction");
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group) {
			WriteDocs(writer, method);
			WriteMethod(writer, method, GetCreateName(method));
			using (writer.Indent()) {
				WriteInitializeInstruction(writer, method);
				var args = method.Args;
				var codeName = idConverter.Argument(args[0].Name);
				var opKindStr = OpKindEnum.Instance.Name(idConverter);
				var registerStr = OpKindEnum.Instance[nameof(OpKind.Register)].Name(idConverter);
				var memoryStr = OpKindEnum.Instance[nameof(OpKind.Memory)].Name(idConverter);
				var immediate64Str = OpKindEnum.Instance[nameof(OpKind.Immediate64)].Name(idConverter);
				var immediate8_2ndStr = OpKindEnum.Instance[nameof(OpKind.Immediate8_2nd)].Name(idConverter);
				bool multipleInts = args.Where(a => a.Type == MethodArgType.Int32 || a.Type == MethodArgType.UInt32).Count() > 1;
				int intCount = 0;
				for (int i = 1; i < args.Count; i++) {
					int op = i - 1;
					var arg = args[i];
					writer.WriteLine();
					string castType;
					switch (arg.Type) {
					case MethodArgType.Register:
						writer.WriteLine($"const_assert_eq!(0, {opKindStr}::{registerStr} as u32);");
						writer.WriteLine($"//instruction_internal::internal_set_op{op}_kind(&mut instruction, {opKindStr}::{registerStr});");
						writer.WriteLine($"instruction_internal::internal_set_op{op}_register(&mut instruction, {idConverter.Argument(arg.Name)});");
						break;

					case MethodArgType.Memory:
						writer.WriteLine($"instruction_internal::internal_set_op{op}_kind(&mut instruction, {opKindStr}::{memoryStr});");
						writer.WriteLine($"instruction_internal::internal_set_memory_base(&mut instruction, {idConverter.Argument(arg.Name)}.base);");
						writer.WriteLine($"instruction_internal::internal_set_memory_index(&mut instruction, {idConverter.Argument(arg.Name)}.index);");
						writer.WriteLine($"instruction.set_memory_index_scale({idConverter.Argument(arg.Name)}.scale);");
						writer.WriteLine($"instruction.set_memory_displ_size({idConverter.Argument(arg.Name)}.displ_size);");
						writer.WriteLine($"instruction.set_memory_displacement({idConverter.Argument(arg.Name)}.displacement as u32);");
						writer.WriteLine($"instruction.set_is_broadcast({idConverter.Argument(arg.Name)}.is_broadcast);");
						writer.WriteLine($"instruction.set_segment_prefix({idConverter.Argument(arg.Name)}.segment_prefix);");
						break;

					case MethodArgType.Int32:
					case MethodArgType.UInt32:
						castType = arg.Type == MethodArgType.Int32 ? " as u32" : string.Empty;
						if (multipleInts) {
							switch (intCount++) {
							case 0:
								writer.WriteLine($"instruction_internal::internal_set_op{op}_kind(&mut instruction, instruction_internal::get_immediate_op_kind({codeName}, {op}));");
								writer.WriteLine($"instruction.set_immediate32({idConverter.Argument(arg.Name)}{castType});");
								break;
							case 1:
								writer.WriteLine($"instruction_internal::internal_set_op{op}_kind(&mut instruction, {opKindStr}::{immediate8_2ndStr});");
								writer.WriteLine($"instruction.set_immediate8_2nd({idConverter.Argument(arg.Name)} as u8);");
								break;
							default:
								throw new InvalidOperationException();
							}
						}
						else {
							writer.WriteLine($"let op_kind = instruction_internal::get_immediate_op_kind({codeName}, {op});");
							writer.WriteLine($"instruction_internal::internal_set_op{op}_kind(&mut instruction, op_kind);");
							writer.WriteLine($"if op_kind == {opKindStr}::{immediate64Str} {{");
							using (writer.Indent())
								writer.WriteLine($"instruction.set_immediate64({idConverter.Argument(arg.Name)} as u64);");
							writer.WriteLine("} else {");
							using (writer.Indent())
								writer.WriteLine($"instruction.set_immediate32({idConverter.Argument(arg.Name)}{castType});");
							writer.WriteLine("}");
						}
						break;

					case MethodArgType.Int64:
					case MethodArgType.UInt64:
						castType = arg.Type == MethodArgType.Int64 ? " as u64" : string.Empty;
						writer.WriteLine($"instruction_internal::internal_set_op{op}_kind(&mut instruction, {opKindStr}::{immediate64Str});");
						writer.WriteLine($"instruction.set_immediate64({idConverter.Argument(arg.Name)}{castType});");
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
				writer.WriteLine($"instruction_internal::internal_set_op0_kind(&mut instruction, instruction_internal::get_near_branch_op_kind({idConverter.Argument(method.Args[0].Name)}, 0));");
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
				writer.WriteLine($"instruction_internal::internal_set_op0_kind(&mut instruction, instruction_internal::get_far_branch_op_kind({idConverter.Argument(method.Args[0].Name)}, 0));");
				writer.WriteLine($"instruction.set_far_branch_selector({idConverter.Argument(method.Args[1].Name)});");
				writer.WriteLine($"instruction.set_far_branch32({idConverter.Argument(method.Args[2].Name)});");
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

				var mem64Str = OpKindEnum.Instance[nameof(OpKind.Memory64)].Name(idConverter);
				writer.WriteLine($"instruction_internal::internal_set_op{memOp}_kind(&mut instruction, {OpKindEnum.Instance.Name(idConverter)}::{mem64Str});");
				writer.WriteLine($"instruction.set_memory_address64({idConverter.Argument(method.Args[1 + memOp].Name)});");
				writer.WriteLine("instruction_internal::internal_set_memory_displ_size(&mut instruction, 4);");
				writer.WriteLine($"instruction.set_segment_prefix({idConverter.Argument(method.Args[3].Name)});");

				writer.WriteLine();
				var opKindStr = OpKindEnum.Instance.Name(idConverter);
				var registerStr = OpKindEnum.Instance[nameof(OpKind.Register)].Name(idConverter);
				writer.WriteLine($"const_assert_eq!(0, {opKindStr}::{registerStr} as u32);");
				writer.WriteLine($"//instruction_internal::internal_set_op{regOp}_kind(&mut instruction, {opKindStr}::{registerStr});");
				writer.WriteLine($"instruction_internal::internal_set_op{regOp}_register(&mut instruction, {idConverter.Argument(method.Args[1 + regOp].Name)});");

				WriteMethodFooter(writer, 2);
			}
			writer.WriteLine("}");
		}

		void WriteComma(FileWriter writer) => writer.Write(", ");
		void Write(FileWriter writer, EnumValue value) => writer.Write($"{value.DeclaringType.Name(idConverter)}::{value.Name(idConverter)}");
		void Write(FileWriter writer, MethodArg arg) => writer.Write(idConverter.Argument(arg.Name));

		void WriteAddrSizePanic(FileWriter writer, CreateMethod method) {
			var arg = method.Args[0];
			if (arg.Name != "addressSize")
				throw new InvalidOperationException();
			docWriter.WriteLine(writer, $"Panics if `{idConverter.Argument(arg.Name)}` is not one of 16, 32, 64.");
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) {
			WriteDocs(writer, method, () => WriteAddrSizePanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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
					Write(writer, RegisterEnum.Instance[nameof(Register.None)]);
					WriteComma(writer);
					Write(writer, RepPrefixKindEnum.Instance[nameof(RepPrefixKind.Repe)]);
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
			WriteDocs(writer, method, () => WriteAddrSizePanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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
					Write(writer, kind == StringMethodKind.Repe ? RepPrefixKindEnum.Instance[nameof(RepPrefixKind.Repe)] : RepPrefixKindEnum.Instance[nameof(RepPrefixKind.Repne)]);
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
			WriteDocs(writer, method, () => WriteAddrSizePanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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
					Write(writer, RepPrefixKindEnum.Instance[nameof(RepPrefixKind.Repe)]);
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
			WriteDocs(writer, method, () => WriteAddrSizePanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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
					Write(writer, RegisterEnum.Instance[nameof(Register.None)]);
					WriteComma(writer);
					Write(writer, kind == StringMethodKind.Repe ? RepPrefixKindEnum.Instance[nameof(RepPrefixKind.Repe)] : RepPrefixKindEnum.Instance[nameof(RepPrefixKind.Repne)]);
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
			WriteDocs(writer, method, () => WriteAddrSizePanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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
					Write(writer, RegisterEnum.Instance[nameof(Register.None)]);
					WriteComma(writer);
					Write(writer, RepPrefixKindEnum.Instance[nameof(RepPrefixKind.Repe)]);
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
			WriteDocs(writer, method, () => WriteAddrSizePanic(writer, method));
			var methodName = idConverter.Method("With" + methodBaseName);
			WriteMethodAttributes(writer, method, true);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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
				code = CodeEnum.Instance[nameof(Code.DeclareByte)];
				setValueName = "set_declare_byte_value";
				methodName = "with_declare_byte";
				break;

			case DeclareDataKind.Word:
				code = CodeEnum.Instance[nameof(Code.DeclareWord)];
				setValueName = "set_declare_word_value";
				methodName = "with_declare_word";
				break;

			case DeclareDataKind.Dword:
				code = CodeEnum.Instance[nameof(Code.DeclareDword)];
				setValueName = "set_declare_dword_value";
				methodName = "with_declare_dword";
				break;

			case DeclareDataKind.Qword:
				code = CodeEnum.Instance[nameof(Code.DeclareQword)];
				setValueName = "set_declare_qword_value";
				methodName = "with_declare_qword";
				break;

			default:
				throw new InvalidOperationException();
			}
			methodName = methodName + "_" + method.Args.Count.ToString();

			writer.WriteLine();
			WriteDocs(writer, method);
			WriteMethodAttributes(writer, method, false);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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

		void WriteDataPanic(FileWriter writer, CreateMethod method, string extra) =>
			docWriter.WriteLine(writer, $"Panics if `{idConverter.Argument(method.Args[0].Name)}.len()` {extra}");

		void GenCreateDeclareDataSlice(FileWriter writer, CreateMethod method, int elemSize, EnumValue code, string methodName, string setDeclValueName) {
			writer.WriteLine();
			WriteDocs(writer, method, () => WriteDataPanic(writer, method, $"is not 1-{16 / elemSize}"));
			WriteMethodAttributes(writer, method, false);
			writer.Write($"pub fn {methodName}(");
			WriteMethodDeclArgs(writer, method);
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
				case ArrayType.ByteArray:
					break;

				case ArrayType.ByteSlice:
					GenCreateDeclareDataSlice(writer, method, 1, CodeEnum.Instance[nameof(Code.DeclareByte)], "with_declare_byte", "set_declare_byte_value");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Word:
				switch (arrayType) {
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
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") -> Self {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if {dataName}.len().wrapping_sub(1) > 16 - 1 || ({dataName}.len() & 1) != 0 {{");
						using (writer.Indent())
							writer.WriteLine("panic!();");
						writer.WriteLine("}");
						writer.WriteLine();
						WriteInitializeInstruction(writer, CodeEnum.Instance[nameof(Code.DeclareWord)]);
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
					GenCreateDeclareDataSlice(writer, method, 2, CodeEnum.Instance[nameof(Code.DeclareWord)], "with_declare_word", "set_declare_word_value");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Dword:
				switch (arrayType) {
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
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") -> Self {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if {dataName}.len().wrapping_sub(1) > 16 - 1 || ({dataName}.len() & 3) != 0 {{");
						using (writer.Indent())
							writer.WriteLine("panic!();");
						writer.WriteLine("}");
						writer.WriteLine();
						WriteInitializeInstruction(writer, CodeEnum.Instance[nameof(Code.DeclareDword)]);
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
					GenCreateDeclareDataSlice(writer, method, 4, CodeEnum.Instance[nameof(Code.DeclareDword)], "with_declare_dword", "set_declare_dword_value");
					break;

				default:
					throw new InvalidOperationException();
				}
				break;

			case DeclareDataKind.Qword:
				switch (arrayType) {
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
					WriteMethodDeclArgs(writer, method);
					writer.WriteLine(") -> Self {");
					using (writer.Indent()) {
						var dataName = idConverter.Argument(method.Args[0].Name);
						writer.WriteLine($"if {dataName}.len().wrapping_sub(1) > 16 - 1 || ({dataName}.len() & 7) != 0 {{");
						using (writer.Indent())
							writer.WriteLine("panic!();");
						writer.WriteLine("}");
						writer.WriteLine();
						WriteInitializeInstruction(writer, CodeEnum.Instance[nameof(Code.DeclareQword)]);
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
					GenCreateDeclareDataSlice(writer, method, 8, CodeEnum.Instance[nameof(Code.DeclareQword)], "with_declare_qword", "set_declare_qword_value");
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
