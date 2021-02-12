// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System;
using System.Collections.Generic;
using System.Text;

namespace Generator {
	/// <summary>
	/// Converts C# PascalCase identifiers to eg. snake_case
	/// </summary>
	abstract class IdentifierConverter {
		readonly StringBuilder sb = new StringBuilder();

		public abstract string Type(string name);
		public abstract string Field(string name);
		public abstract string EnumField(string name);
		public abstract string PropertyDoc(string name);
		public abstract string MethodDoc(string name);
		public abstract string Method(string name);
		public abstract string Constant(string name);
		public abstract string Static(string name);
		public abstract string Namespace(string name);
		public abstract string Argument(string name);

		protected string ToSnakeCase(string name) => ToSnakeCase(name, upper: false);

		protected string ToScreamingSnakeCase(string name) =>
			name switch {
				"Fxsave_512Byte" => "FXSAVE_512BYTE",
				"Fxsave64_512Byte" => "FXSAVE64_512BYTE",
				"Cmpxchg486A" => "CMPXCHG486A",
				"NoMPFX_0FBC" => "NO_MPFX_0FBC",
				"NoMPFX_0FBD" => "NO_MPFX_0FBD",
				"NoLahfSahf64" => "NO_LAHF_SAHF_64",
				"OpKind_MemoryESDI" => "OP_KIND_MEMORY_ES_DI",
				"OpKind_MemoryESEDI" => "OP_KIND_MEMORY_ES_EDI",
				"OpKind_MemoryESRDI" => "OP_KIND_MEMORY_ES_RDI",
				"HighLegacy8BitRegs" => "HIGH_LEGACY_8_BIT_REGS",
				"TwoByteHandlers_0FXXIndex" => "TWO_BYTE_HANDLERS_0FXX_INDEX",
				"ThreeByteHandlers_0F38XXIndex" => "THREE_BYTE_HANDLERS_0F38XX_INDEX",
				"ThreeByteHandlers_0F3AXXIndex" => "THREE_BYTE_HANDLERS_0F3AXX_INDEX",
				"XOPAIndex" => "XOPA_INDEX",
				"Handler66Reg" => "HANDLER_66_REG",
				"Handler66Mem" => "HANDLER_66_MEM",
				"Cyrix_SMINT_0F7E" => "CYRIX_SMINT_0F7E",
				_ => ToSnakeCase(name, upper: true),
			};

		string ToSnakeCase(string name, bool upper) {
			sb.Clear();
			bool prevWasUpper = false;
			foreach (var c in name) {
				bool isUpper = char.IsUpper(c);
				if (isUpper && !prevWasUpper) {
					if (sb.Length > 0 && sb[^1] != '_')
						sb.Append('_');
				}
				prevWasUpper = isUpper;
				sb.Append(upper ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
			}
			return sb.ToString();
		}

		protected string ToLowerCamelCase(string name) {
			sb.Clear();
			int i = 0;
			while (i < name.Length && char.IsUpper(name[i]))
				sb.Append(char.ToLowerInvariant(name[i++]));
			sb.Append(name, i, name.Length - i);
			return sb.ToString();
		}
	}

	sealed class CSharpIdentifierConverter : IdentifierConverter {
		public static IdentifierConverter Create() => new CSharpIdentifierConverter();
		CSharpIdentifierConverter() { }
		public override string Type(string name) => name;
		public override string Field(string name) => Escape(name);
		public override string EnumField(string name) => Escape(name);
		public override string PropertyDoc(string name) => name;
		public override string MethodDoc(string name) => name;
		public override string Method(string name) => Escape(name);
		public override string Constant(string name) => Escape(name);
		public override string Static(string name) => Escape(name);
		public override string Namespace(string name) => Escape(name);
		public override string Argument(string name) => Escape(name);

		static readonly HashSet<string> keywords = new HashSet<string>(StringComparer.Ordinal) {
			"abstract", "as", "base", "bool",
			"break", "byte", "case", "catch",
			"char", "checked", "class", "const",
			"continue", "decimal", "default", "delegate",
			"do", "double", "else", "enum",
			"event", "explicit", "extern", "false",
			"finally", "fixed", "float", "for",
			"foreach", "goto", "if", "implicit",
			"in", "int", "interface", "internal",
			"is", "lock", "long", "namespace",
			"new", "null", "object", "operator",
			"out", "override", "params", "private",
			"protected", "public", "readonly", "ref",
			"return", "sbyte", "sealed", "short",
			"sizeof", "stackalloc", "static", "string",
			"struct", "switch", "this", "throw",
			"true", "try", "typeof", "uint",
			"ulong", "unchecked", "unsafe", "ushort",
			"using", "using", "static", "virtual", "void",
			"volatile", "while",
		};

		static string Escape(string name) => keywords.Contains(name) ? "@" + name : name;
	}

	sealed class RustIdentifierConverter : IdentifierConverter {
		public static IdentifierConverter Create() => new RustIdentifierConverter();
		RustIdentifierConverter() { }
		public override string Type(string name) => name;
		public override string Field(string name) => ToSnakeCase(name);
		public override string EnumField(string name) => name;
		public override string PropertyDoc(string name) => ToSnakeCase(name) + "()";
		public override string MethodDoc(string name) => ToSnakeCase(name) + "()";
		public override string Method(string name) => ToSnakeCase(name);
		public override string Constant(string name) => ToScreamingSnakeCase(name);
		public override string Static(string name) => ToScreamingSnakeCase(name);
		public override string Namespace(string name) => ToSnakeCase(name);
		public override string Argument(string name) => ToSnakeCase(name);
	}

	sealed class RustJSIdentifierConverter : IdentifierConverter {
		public static IdentifierConverter Create() => new RustJSIdentifierConverter();
		RustJSIdentifierConverter() { }
		public override string Type(string name) => name;
		public override string Field(string name) => ToLowerCamelCase(name);
		public override string EnumField(string name) => name;
		public override string PropertyDoc(string name) => ToLowerCamelCase(name);
		public override string MethodDoc(string name) => ToLowerCamelCase(name) + "()";
		public override string Method(string name) => ToLowerCamelCase(name);
		public override string Constant(string name) => name;
		public override string Static(string name) => name;
		public override string Namespace(string name) => name;
		public override string Argument(string name) => ToLowerCamelCase(name);
	}

	sealed class PythonIdentifierConverter : IdentifierConverter {
		public static IdentifierConverter Create() => new PythonIdentifierConverter();
		PythonIdentifierConverter() { }
		public override string Type(string name) => name;
		public override string Field(string name) => "__" + ToSnakeCase(name);
		public override string EnumField(string name) => ToScreamingSnakeCase(name);
		public override string PropertyDoc(string name) => ToSnakeCase(name);
		public override string MethodDoc(string name) => ToSnakeCase(name);
		public override string Method(string name) => ToSnakeCase(name);
		public override string Constant(string name) => ToScreamingSnakeCase(name);
		public override string Static(string name) => ToScreamingSnakeCase(name);
		public override string Namespace(string name) => ToSnakeCase(name);
		public override string Argument(string name) => ToSnakeCase(name);
	}
}
