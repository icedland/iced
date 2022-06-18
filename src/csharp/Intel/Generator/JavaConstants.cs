// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.IO;
using System.Linq;

namespace Generator {
	static class JavaConstants {
		public const string IcedPackage = "com.github.icedland.iced.x86";
		public const string CodeAssemblerPackage = IcedPackage + ".asm";
		public const string BlockEncoderPackage = IcedPackage + ".enc";
		public const string DecoderPackage = IcedPackage + ".dec";
		public const string EncoderPackage = IcedPackage + ".enc";
		public const string InstructionInfoPackage = IcedPackage + ".info";
		public const string FormatterPackage = IcedPackage + ".fmt";
		public const string GasFormatterPackage = FormatterPackage + ".gas";
		public const string IntelFormatterPackage = FormatterPackage + ".intel";
		public const string MasmFormatterPackage = FormatterPackage + ".masm";
		public const string NasmFormatterPackage = FormatterPackage + ".nasm";
		public const string FastFormatterPackage = FormatterPackage + ".fast";

		public const string IcedInternalPackage = IcedPackage + ".internal";
		public const string DecoderInternalPackage = IcedInternalPackage + ".dec";
		public const string EncoderInternalPackage = IcedInternalPackage + ".enc";
		public const string InstructionInfoInternalPackage = IcedInternalPackage + ".info";
		public const string FormatterInternalPackage = IcedInternalPackage + ".fmt";

		public const string InternalDoc = "DO NOT USE: INTERNAL API";

		public static string GetFilename(GenTypes genTypes, string package, params string[] names) =>
			Path.Combine(new[] { genTypes.Dirs.JavaDir, "src", "main", "java" }.Concat(package.Split('.')).Concat(names).ToArray());

		public static string GetTestFilename(GenTypes genTypes, string package, params string[] names) =>
			Path.Combine(new[] { genTypes.Dirs.JavaDir, "src", "test", "java" }.Concat(package.Split('.')).Concat(names).ToArray());

		public static string GetResourceFilename(GenTypes genTypes, string package, params string[] names) =>
			Path.Combine(new[] { genTypes.Dirs.JavaDir, "src", "main", "resources" }.Concat(package.Split('.')).Concat(names).ToArray());
	}
}
