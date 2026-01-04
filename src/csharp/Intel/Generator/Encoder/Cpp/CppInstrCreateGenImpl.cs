// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Text;
using Generator.Documentation.Cpp;
using Generator.IO;

namespace Generator.Encoder.Cpp {
	sealed class GenCreateNameArgs {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
		public string CreatePrefix;
		public string Register;
		public string Memory;
		public string Int32;
		public string UInt32;
		public string Int64;
		public string UInt64;
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

		public static readonly GenCreateNameArgs CppNames = new() {
			CreatePrefix = "with",
			Register = "_reg",
			Memory = "_mem",
			Int32 = "_i32",
			UInt32 = "_u32",
			Int64 = "_i64",
			UInt64 = "_u64",
		};
	}

	sealed class CppInstrCreateGenImpl {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;
		readonly CppDocCommentWriter docWriter;
		readonly StringBuilder sb;

		public CppInstrCreateGenImpl(GenTypes genTypes, IdentifierConverter idConverter, CppDocCommentWriter docWriter) {
			this.genTypes = genTypes;
			this.idConverter = idConverter;
			this.docWriter = docWriter;
			sb = new StringBuilder();
		}

		public void WriteDocs(FileWriter writer, CreateMethod method, string sectionTitle, Action? writeSection) {
			const string typeName = "Instruction";
			docWriter.BeginWrite(writer);
			foreach (var doc in method.Docs)
				docWriter.WriteDocLine(writer, doc, typeName);
			docWriter.WriteLine(writer, string.Empty);
			if (writeSection is not null) {
				docWriter.WriteLine(writer, $"@throws std::invalid_argument {sectionTitle}");
				writeSection();
				docWriter.WriteLine(writer, string.Empty);
			}
			docWriter.WriteLine(writer, "@param code Code value");
			for (int i = 1; i < method.Args.Count; i++) {
				var arg = method.Args[i];
				docWriter.Write($"@param {idConverter.Argument(arg.Name)} ");
				docWriter.WriteDocLine(writer, arg.Doc, typeName);
			}
			docWriter.WriteLine(writer, "@return Created instruction");
			docWriter.EndWrite(writer);
		}

		public void WriteDocsSimple(FileWriter writer, CreateMethod method) {
			const string typeName = "Instruction";
			docWriter.BeginWrite(writer);
			foreach (var doc in method.Docs)
				docWriter.WriteDocLine(writer, doc, typeName);
			docWriter.WriteLine(writer, string.Empty);
			for (int i = 0; i < method.Args.Count; i++) {
				var arg = method.Args[i];
				docWriter.Write($"@param {idConverter.Argument(arg.Name)} ");
				docWriter.WriteDocLine(writer, arg.Doc, typeName);
			}
			docWriter.WriteLine(writer, "@return Created instruction");
			docWriter.EndWrite(writer);
		}

		public string GetArgTypeString(MethodArg arg) =>
			arg.Type switch {
				MethodArgType.Code => "Code",
				MethodArgType.Register => "Register",
				MethodArgType.RepPrefixKind => "RepPrefixKind",
				MethodArgType.Memory => "const MemoryOperand&",
				MethodArgType.UInt8 => "uint8_t",
				MethodArgType.UInt16 => "uint16_t",
				MethodArgType.Int32 => "int32_t",
				MethodArgType.PreferredInt32 or MethodArgType.UInt32 => "uint32_t",
				MethodArgType.Int64 => "int64_t",
				MethodArgType.UInt64 => "uint64_t",
				MethodArgType.ByteSlice => "std::span<const uint8_t>",
				MethodArgType.WordSlice => "std::span<const uint16_t>",
				MethodArgType.DwordSlice => "std::span<const uint32_t>",
				MethodArgType.QwordSlice => "std::span<const uint64_t>",
				MethodArgType.BytePtr => "const uint8_t*",
				MethodArgType.WordPtr => "const uint16_t*",
				MethodArgType.DwordPtr => "const uint32_t*",
				MethodArgType.QwordPtr => "const uint64_t*",
				MethodArgType.ArrayLength => "size_t",
				_ => throw new InvalidOperationException($"Unknown arg type: {arg.Type}"),
			};

		public void WriteMethodDeclArgs(FileWriter writer, CreateMethod method) {
			bool comma = false;
			foreach (var arg in method.Args) {
				if (comma)
					writer.Write(", ");
				comma = true;
				writer.Write(GetArgTypeString(arg));
				writer.Write(" ");
				writer.Write(idConverter.Argument(arg.Name));
			}
		}

		public static string GetCppOverloadedCreateName(CreateMethod method) => GetCppOverloadedCreateName(method.Args.Count - 1);
		public static string GetCppOverloadedCreateName(int argCount) => argCount == 0 ? "with" : "with" + argCount.ToString();

		public string GetCreateName(CreateMethod method, GenCreateNameArgs genNames) => GetCreateName(sb, method, genNames);

		public static string GetCreateName(StringBuilder sb, CreateMethod method, GenCreateNameArgs genNames) {
			if (method.Args.Count == 0 || method.Args[0].Type != MethodArgType.Code)
				throw new InvalidOperationException();

			sb.Clear();
			sb.Append(genNames.CreatePrefix);
			var args = method.Args;
			for (int i = 1; i < args.Count; i++) {
				var arg = args[i];
				switch (arg.Type) {
				case MethodArgType.Register:
					sb.Append(genNames.Register);
					break;
				case MethodArgType.Memory:
					sb.Append(genNames.Memory);
					break;
				case MethodArgType.Int32:
					sb.Append(genNames.Int32);
					break;
				case MethodArgType.UInt32:
					sb.Append(genNames.UInt32);
					break;
				case MethodArgType.Int64:
					sb.Append(genNames.Int64);
					break;
				case MethodArgType.UInt64:
					sb.Append(genNames.UInt64);
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

			return sb.ToString();
		}

		static bool HasImmediateArg_8_16_32_64(CreateMethod method) {
			foreach (var arg in method.Args) {
				switch (arg.Type) {
				case MethodArgType.UInt8:
				case MethodArgType.UInt16:
				case MethodArgType.Int32:
				case MethodArgType.UInt32:
				case MethodArgType.Int64:
				case MethodArgType.UInt64:
					return true;
				}
			}
			return false;
		}

		// Assumes it's a generic with_*() method (not a specialized method such as with_movsb() etc)
		public static bool HasTryMethod(CreateMethod method) =>
			HasImmediateArg_8_16_32_64(method);
	}
}
