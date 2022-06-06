// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Text;
using Generator.Documentation.Java;
using Generator.IO;

namespace Generator.Encoder.Java {
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

		public static readonly GenCreateNameArgs JavaNames = new() {
			CreatePrefix = "create",
			Register = "Reg",
			Memory = "Mem",
			Int32 = "I32",
			UInt32 = "U32",
			Int64 = "I64",
			UInt64 = "U64",
		};
	}

	sealed class InstrCreateGenImpl {
		readonly GenTypes genTypes;
		readonly IdentifierConverter idConverter;
		readonly JavaDocCommentWriter docWriter;

		public InstrCreateGenImpl(GenTypes genTypes, IdentifierConverter idConverter, JavaDocCommentWriter docWriter) {
			this.genTypes = genTypes;
			this.idConverter = idConverter;
			this.docWriter = docWriter;
		}

		public string GetArgTypeString(MethodArg arg) =>
			arg.Type switch {
				MethodArgType.Memory => "MemoryOperand",
				MethodArgType.UInt8 => "byte",
				MethodArgType.UInt16 => "short",
				MethodArgType.Code or MethodArgType.Register or MethodArgType.RepPrefixKind or
				MethodArgType.Int32 or MethodArgType.PreferredInt32 or MethodArgType.UInt32 or
				MethodArgType.ArrayIndex or MethodArgType.ArrayLength => "int",
				MethodArgType.Int64 or MethodArgType.UInt64 => "long",
				MethodArgType.ByteArray => "byte[]",
				MethodArgType.WordArray => "short[]",
				MethodArgType.DwordArray => "int[]",
				MethodArgType.QwordArray => "long[]",
				_ => throw new InvalidOperationException(),
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

		// Assumes it's a generic create*() method (not a specialized method such as createMovsb() etc)
		public static bool HasTryMethod(CreateMethod method) =>
			HasImmediateArg_8_16_32_64(method);
	}
}
