// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Text;
using Generator.Documentation.Lua;
using Generator.Enums;
using Generator.IO;

namespace Generator.Encoder.Lua {
	[Generator(TargetLanguage.Lua)]
	sealed class LuaInstrCreateGen : InstrCreateGen {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;
		readonly IdentifierConverter rustIdConverter;
		readonly LuaDocCommentWriter docWriter;
		readonly StringBuilder sb;

		public LuaInstrCreateGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = LuaIdentifierConverter.Create();
			rustIdConverter = RustIdentifierConverter.Create();
			docWriter = new LuaDocCommentWriter(idConverter, TargetLanguage.Rust);
			sb = new StringBuilder();
		}

		protected override (TargetLanguage language, string id, string filename) GetFileInfo() =>
			(TargetLanguage.Rust, "Create", generatorContext.Types.Dirs.GetLuaRustFilename("instr.rs"));

		sealed class MethodArgInfo {
			public readonly string RustName;
			public readonly string LuaName;

			public MethodArgInfo(string rustName, string luaName) {
				RustName = rustName;
				LuaName = luaName;
			}
		}

		sealed class GeneratedMethodInfo {
			public readonly CreateMethod Method;
			public readonly bool CanFail;
			public readonly string RustMethodName;
			public readonly string LuaMethodName;
			public readonly MethodArgInfo[] ArgInfos;
			public readonly List<string> Overloads;

			public GeneratedMethodInfo(CreateMethod method, bool canFail, string rustMethodName, string luaMethodName, IdentifierConverter rustIdConverter, IdentifierConverter luaIdConverter) {
				Method = method;
				CanFail = canFail;
				RustMethodName = rustMethodName;
				LuaMethodName = luaMethodName;
				ArgInfos = new MethodArgInfo[method.Args.Count];
				Overloads = new();
				for (int i = 0; i < method.Args.Count; i++) {
					var origName = method.Args[i].Name;
					var rustName = rustIdConverter.Argument(origName);
					var luaName = luaIdConverter.Argument(origName);
					ArgInfos[i] = new MethodArgInfo(rustName: rustName, luaName: luaName);
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
			string? rustMethodName = null, string? luaMethodName = null) {
			if (rustMethodName is null)
				rustMethodName = Rust.InstrCreateGenImpl.GetRustOverloadedCreateName(method);
			else if (isTryMethod)
				rustMethodName = "try_" + rustMethodName;
			luaMethodName ??= rustMethodName;
			var info = new GeneratedMethodInfo(method, canFail, rustMethodName, luaMethodName, rustIdConverter, idConverter);
			var ctx = new GenerateMethodContext(writer, info);
			genMethod(ctx);
		}

		void WriteMethodDeclArgs(in GenerateMethodContext ctx) {
			ctx.Writer.Write("lua");
			for (int i = 0; i < ctx.Method.Args.Count; i++) {
				var arg = ctx.Method.Args[i];
				ctx.Writer.Write(", ");
				ctx.Writer.Write(ctx.Info.ArgInfos[i].LuaName);
				ctx.Writer.Write(": ");
				ctx.Writer.Write(GetArgType(arg));
			}
		}
		static (string luaType, string? descType) GetArgType(MethodArgType type) =>
			type switch {
				MethodArgType.Code => ("integer", "A `Code` enum variant"),
				MethodArgType.Register => ("integer", "A `Register` enum variant"),
				MethodArgType.RepPrefixKind => ("integer", "A `RepPrefixKind` enum variant"),
				MethodArgType.Memory => ("MemoryOperand", null),
				MethodArgType.UInt8 => ("integer", "`u8`"),
				MethodArgType.UInt16 => ("integer", "`u16`"),
				MethodArgType.Int32 => ("integer", "`i32`"),
				MethodArgType.PreferredInt32 or MethodArgType.UInt32 => ("integer", "`u32`"),
				MethodArgType.Int64 => ("integer", "`i64`"),
				MethodArgType.UInt64 => ("integer", "`u64`"),
				MethodArgType.ByteSlice => ("string", null),
				_ => throw new InvalidOperationException(),
			};

		void WriteDocs(in GenerateMethodContext ctx) {
			const string typeName = "Instruction";
			docWriter.BeginWrite(ctx.Writer);
			foreach (var doc in ctx.Info.Method.Docs)
				docWriter.WriteDocLine(ctx.Writer, doc, typeName);
			docWriter.WriteLine(ctx.Writer, string.Empty);
			for (int i = 0; i < ctx.Info.Method.Args.Count; i++) {
				var arg = ctx.Info.Method.Args[i];
				var typeInfo = GetArgType(arg.Type);
				var opt = arg.DefaultValue is not null ? "?" : string.Empty;
				docWriter.Write($"@param {idConverter.Argument(arg.Name)}{opt} {typeInfo.luaType} #");
				if (arg.DefaultValue is not null) {
					switch (arg.DefaultValue) {
					case EnumValue enumValue:
						docWriter.Write($"(default = `{enumValue.Name(idConverter)}`) ");
						break;
					default:
						throw new InvalidOperationException();
					}
				}
				if (typeInfo.descType is not null)
					docWriter.Write($"({typeInfo.descType}) ");
				docWriter.WriteDocLine(ctx.Writer, arg.Doc, typeName);
			}
			docWriter.WriteLine(ctx.Writer, $"@return {typeName}");
			foreach (var overload in ctx.Info.Overloads)
				docWriter.WriteLine(ctx.Writer, overload);
			docWriter.EndWrite(ctx.Writer);
		}

		void WriteMethod(in GenerateMethodContext ctx) {
			WriteDocs(ctx);
			ctx.Writer.WriteLine(RustConstants.AttributeNoRustFmt);
			ctx.Writer.Write($"unsafe fn {ctx.Info.LuaMethodName}(");
			WriteMethodDeclArgs(ctx);
			ctx.Writer.WriteLine(") -> 1 {");
			using (ctx.Writer.Indent())
				WriteConvertArgsCode(ctx);
		}

		static int LuaArgCount(InstructionGroup group) =>
			ArgIndexToLuaStackIndex(group.Operands.Length);
		static int ArgIndexToLuaStackIndex(int argIndex) =>
			// +1 == 1-based indexes
			argIndex + 1;

		void WriteCreateArgsCode(in GenerateMethodContext ctx) {
			// First arg is the code value, ignore it
			for (int i = 1; i < ctx.Method.Args.Count; i++) {
				var arg = ctx.Method.Args[i];
				var luaArgIndex = ArgIndexToLuaStackIndex(i);
				var argType = GetArgType(arg);
				ctx.Writer.WriteLine($"let {arg.Name}: <{argType} as loona::tofrom::FromLua<'_>>::RetType = unsafe {{ <{argType} as loona::tofrom::FromLua<'_>>::from_lua(lua, {luaArgIndex}) }};");
			}
		}

		string GetArgType(MethodArg arg) {
			string argType;
			switch (arg.Type) {
			case MethodArgType.Code:
			case MethodArgType.Register:
			case MethodArgType.RepPrefixKind:
				argType = "u32";
				break;
			case MethodArgType.Memory:
				argType = "&crate::mem_op::MemoryOperand";
				break;
			case MethodArgType.UInt8:
				argType = "u8";
				break;
			case MethodArgType.UInt16:
				argType = "u16";
				break;
			case MethodArgType.Int32:
				argType = "i32";
				break;
			case MethodArgType.UInt32:
			case MethodArgType.PreferredInt32:
				argType = "u32";
				break;
			case MethodArgType.Int64:
				argType = "i64";
				break;
			case MethodArgType.UInt64:
				argType = "u64";
				break;
			case MethodArgType.ByteSlice:
				argType = "&[u8]";
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
			if (arg.DefaultValue is not null) {
				string defaultValue;
				switch (arg.DefaultValue) {
				case EnumValue enumValue:
					defaultValue = "iced_x86::" + rustIdConverter.ToDeclTypeAndValue(enumValue) + " as " + argType;
					break;
				default:
					throw new InvalidOperationException();
				}
				argType = "LuaDefault" + argType[0..1].ToUpperInvariant() + argType[1..] + "<{" + defaultValue + "}>";
			}
			return argType;
		}

		void WriteConvertArgsCode(in GenerateMethodContext ctx, int startArgIndex = 0) {
			for (int i = startArgIndex; i < ctx.Method.Args.Count; i++) {
				var arg = ctx.Method.Args[i];

				// Verify all enum args (they're u32 args)
				string? checkFunc;
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
				default:
					checkFunc = null;
					break;
				}
				if (checkFunc is not null) {
					var argName = ctx.Info.ArgInfos[i].LuaName;
					ctx.Writer.WriteLine($"let {argName} = unsafe {{ {checkFunc}(lua, {argName}) }};");
				}
			}
		}

		void WriteCall(in GenerateMethodContext ctx) {
			using (ctx.Writer.Indent()) {
				WriteCreateInstrCode(ctx, true);
				WritePushInstruction(ctx);
			}
		}

		void WriteCreateInstrCode(in GenerateMethodContext ctx, bool useLet) {
			sb.Clear();
			sb.Append("iced_x86::Instruction::");
			sb.Append(ctx.Info.RustMethodName);
			sb.Append('(');
			for (int i = 0; i < ctx.Method.Args.Count; i++) {
				if (i > 0)
					sb.Append(", ");
				switch (ctx.Method.Args[i].Type) {
				case MethodArgType.Memory:
					sb.Append(ctx.Info.ArgInfos[i].LuaName);
					sb.Append(".inner");
					break;
				default:
					sb.Append(ctx.Info.ArgInfos[i].LuaName);
					break;
				}
			}
			sb.Append(')');

			var let = useLet ? "let " : string.Empty;
			if (ctx.Info.CanFail) {
				ctx.Writer.WriteLine($"{let}instr = Instruction {{ inner: match {sb.ToString()} {{");
				using (ctx.Writer.Indent()) {
					ctx.Writer.WriteLine("Ok(instr) => instr,");
					ctx.Writer.WriteLine("Err(e) => unsafe { lua.throw_error(e) },");
				}
				ctx.Writer.WriteLine("}};");
			}
			else
				ctx.Writer.WriteLine($"{let}instr = Instruction {{ inner: {sb.ToString()} }};");
		}

		void WritePushInstruction(in GenerateMethodContext ctx) =>
			ctx.Writer.WriteLine("let _ = unsafe { Instruction::init_and_push(lua, &instr) };");

		protected override void Generate(FileWriter writer) {
			GenCreateMethods(writer);
			writer.WriteLine();
			GenTheRest(writer);
		}

		protected override void GenCreate(FileWriter writer, CreateMethod method, InstructionGroup group, int id) =>
			// Only called by the base class from a method we override so this method is never called
			throw new InvalidOperationException();

		void GenCreateMethods(FileWriter writer) {
			var table = GetDynCreateMethodTable();
			GenerateGroupIndexesFile(table);
			GenCreateMethod(writer, table, isSigned: true);
		}

		void GenCreateMethod(FileWriter writer, in DynCreateMethodTable table, bool isSigned) {
			const string signedMethodName = "create";
			string methodName;
			string docs;
			if (isSigned) {
				docs = $"Creates an instruction. All immediate values are assumed to be signed";
				methodName = signedMethodName;
			}
			else
				throw new InvalidOperationException();
			var method = new CreateMethod(docs);
			AddCodeArg(method);
			var info = new GeneratedMethodInfo(method, true, "create", methodName, rustIdConverter, idConverter);
			info.Overloads.AddRange(GetOverloadStrings(table.OrigGroups));
			// Lua language server shows them in reverse order so reverse them first
			info.Overloads.Reverse();
			var ctx = new GenerateMethodContext(writer, info);
			WriteMethod(ctx);
			using (ctx.Writer.Indent()) {
				ctx.Writer.WriteLine("let arg_count = unsafe { lua.get_top() };");
				ctx.Writer.WriteLine("let instr: Instruction;");
				ctx.Writer.WriteLine("match crate::grp_idx::GROUP_INDEXES[code as usize] {");
				using (ctx.Writer.Indent()) {
					for (int i = 0; i < table.Groups.Length; i++) {
						if (table.Groups[i] is InstructionGroup group) {
							// If it has an op that can be reg or mem, we have to check that at runtime
							var regOrMemIndex = Array.IndexOf(group.Operands, InstructionOperand.RegisterMemory);
							ctx.Writer.WriteLine($"{i} => {{");
							using (ctx.Writer.Indent()) {
								ctx.Writer.WriteLine($"if arg_count != {LuaArgCount(group)} {{");
								using (ctx.Writer.Indent())
									ctx.Writer.WriteLine("unsafe { lua.throw_error_msg(\"Invalid arg count\") }");
								ctx.Writer.WriteLine("}");
								if (regOrMemIndex >= 0) {
									// +1 == skip the 'code' arg
									var idx = ArgIndexToLuaStackIndex(regOrMemIndex + 1);
									ctx.Writer.WriteLine($"if unsafe {{ lua.type_({idx}) }} == loona::lua_api::LUA_TUSERDATA {{");
									using (ctx.Writer.Indent()) {
										const bool useReg = false;
										GenCreateMethod(writer, group, methodName, isSigned, useReg);
									}
									ctx.Writer.WriteLine("} else {");
									using (ctx.Writer.Indent()) {
										const bool useReg = true;
										GenCreateMethod(writer, group, methodName, isSigned, useReg);
									}
									ctx.Writer.WriteLine("}");
								}
								else {
									// Can be anything since no operand can be InstructionOperand.RegisterMemory
									const bool useReg = false;
									GenCreateMethod(writer, group, methodName, isSigned, useReg);
								}
							}
							ctx.Writer.WriteLine("}");
						}
						else {
							ctx.Writer.WriteCommentLine("Invalid Code value, call some other create method instead, eg. create_branch(), etc");
							ctx.Writer.WriteLine($"{i} => unsafe {{ lua.throw_error_msg(\"Invalid Code value\") }},");
						}
					}
					ctx.Writer.WriteCommentLine("Unreachable");
					ctx.Writer.WriteLine($"_ => unsafe {{ lua.throw_error_msg(\"Invalid Code value\") }},");
				}
				ctx.Writer.WriteLine("}");
				WritePushInstruction(ctx);
			}
			ctx.Writer.WriteLine("}");
		}

		void GenCreateMethod(FileWriter writer, InstructionGroup group, string methodName, bool isSigned, bool useReg) {
			writer.WriteCommentLine(GetOverloadString(group, useReg));
			var createMethod = GetMethod(group, unsigned: !isSigned, useReg);
			bool canFail = createMethod.Args.Count > 1;
			var rustMethodName = Rust.InstrCreateGenImpl.GetRustOverloadedCreateName(createMethod);
			var createInfo = new GeneratedMethodInfo(createMethod, canFail, rustMethodName, methodName, rustIdConverter, idConverter);
			var createCtx = new GenerateMethodContext(writer, createInfo);
			WriteCreateArgsCode(createCtx);
			WriteConvertArgsCode(createCtx, 1);
			WriteCreateInstrCode(createCtx, false);
		}

        void GenerateGroupIndexesFile(in DynCreateMethodTable table) {
			var filename = genTypes.Dirs.GetLuaRustFilename("grp_idx.rs");
			using (var writer = new FileWriter(TargetLanguage.Rust, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				var type = table.Groups.Length <= 256 ? "u8" : "u16";
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"pub(crate) static GROUP_INDEXES: [{type}; {table.CodeIndexes.Length}] = [");
				using (writer.Indent()) {
					const int valuesPerLine = 32;
					for (int i = 0; i < table.CodeIndexes.Length; i += valuesPerLine) {
						for (int j = 0; j < valuesPerLine && i + j < table.CodeIndexes.Length; j++) {
							if (j > 0)
								writer.Write(" ");
							var b = table.CodeIndexes[i + j];
							writer.Write($"{b},");
						}
						writer.WriteLine();
					}
				}
				writer.WriteLine("];");
			}
		}

        IEnumerable<string> GetOverloadStrings(InstructionGroup[] groups) {
			var seen = new HashSet<string>(StringComparer.Ordinal);
			foreach (var group in groups) {
				foreach (var overload in GetOverloadString(group)) {
					if (seen.Add(overload))
						yield return overload;
				}
			}
		}

		IEnumerable<string> GetOverloadString(InstructionGroup group) {
			var s1 = GetOverloadString(group, true);
			yield return s1;
			var s2 = GetOverloadString(group, false);
			if (s1 != s2)
				yield return s2;
		}

		string GetOverloadString(InstructionGroup group, bool useReg) {
			var method = GetMethod(group, unsigned: false, useReg);
			// First arg is the `code` arg
			if (method.Args.Count != group.Operands.Length + 1)
				throw new InvalidOperationException();
			sb.Clear();
			sb.Append($"@overload fun({method.Args[0].Name}: integer");
			for (int i = 0; i < group.Operands.Length; i++) {
				var type = group.Operands[i].Split(useReg) switch {
					InstructionOperand.RegisterMemory => throw new InvalidOperationException(),
					InstructionOperand.Register or InstructionOperand.Imm32 or InstructionOperand.Imm64 => "integer",
					InstructionOperand.Memory => "MemoryOperand",
					_ => throw new InvalidOperationException(),
				};
				var arg = method.Args[i + 1];
				sb.AppendFormat($", {arg.Name}: {type}");
			}
			sb.Append("): Instruction");
			return sb.ToString();
		}

		void GenCreate(GenerateMethodContext ctx) {
			WriteMethod(ctx);
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenCreateBranch, Rust.RustInstrCreateGenNames.with_branch, "create_branch");

		void GenCreateBranch(GenerateMethodContext ctx) {
			WriteMethod(ctx);
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateFarBranch(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenCreateFarBranch, Rust.RustInstrCreateGenNames.with_far_branch, "create_far_branch");

		void GenCreateFarBranch(GenerateMethodContext ctx) {
			WriteMethod(ctx);
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateXbegin(FileWriter writer, CreateMethod method) =>
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenCreateXbegin, Rust.RustInstrCreateGenNames.with_xbegin, "create_xbegin");

		void GenCreateXbegin(GenerateMethodContext ctx) {
			WriteMethod(ctx);
			WriteCall(ctx);
			ctx.Writer.WriteLine("}");
		}

		protected override void GenCreateString_Reg_SegRSI(FileWriter writer, CreateMethod method, StringMethodKind kind, string methodBaseName, EnumValue code, EnumValue register) =>
			GenStringInstr(writer, method, methodBaseName);

		void GenStringInstr(FileWriter writer, CreateMethod method, string methodBaseName) {
			var rustName = rustIdConverter.Method("With" + methodBaseName);
			var luaName = idConverter.Method("Create" + methodBaseName);
			GenerateMethod(writer, method, canFail: true, isTryMethod: false, GenStringInstr, rustName, luaName);
		}

		void GenStringInstr(GenerateMethodContext ctx) {
			WriteMethod(ctx);
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

		protected override void GenCreateDeclareData(FileWriter writer, CreateMethod method, DeclareDataKind kind) { }
		protected override void GenCreateDeclareDataArray(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) { }
		protected override void GenCreateDeclareDataArrayLength(FileWriter writer, CreateMethod method, DeclareDataKind kind, ArrayType arrayType) { }
	}
}
