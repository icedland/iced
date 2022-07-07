// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Documentation.Java;
using Generator.IO;

namespace Generator.Encoder.Java {
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
				// If the parameter types are byte/short, the user has to cast literals to the
				// target type, eg. `(byte)123` even though the literal fits in the target type.
				// We use int args and then check at runtime if they fit in 8 or 16 bits instead.
				MethodArgType.UInt8 => "int",
				MethodArgType.UInt16 => "int",
				MethodArgType.Register => "ICRegister",
				MethodArgType.Code or MethodArgType.RepPrefixKind or
				MethodArgType.Int32 or MethodArgType.PreferredInt32 or MethodArgType.UInt32 or
				MethodArgType.ArrayIndex or MethodArgType.ArrayLength => "int",
				MethodArgType.Int64 or MethodArgType.UInt64 => "long",
				MethodArgType.ByteArray => "byte[]",
				MethodArgType.WordArray => "short[]",
				MethodArgType.DwordArray => "int[]",
				MethodArgType.QwordArray => "long[]",
				_ => throw new InvalidOperationException(),
			};

		internal static string? GetIntConvertFunc(MethodArgType kind) =>
			kind switch {
				MethodArgType.UInt8 => "toByte",
				MethodArgType.UInt16 => "toShort",
				_ => null,
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
