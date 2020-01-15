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
		protected string ToScreamingSnakeCase(string name) => ToSnakeCase(name, upper: true);

		string ToSnakeCase(string name, bool upper) {
			sb.Clear();
			bool prevWasUpper = false;
			foreach (var c in name) {
				bool isUpper = char.IsUpper(c);
				if (isUpper && !prevWasUpper) {
					if (sb.Length > 0 && sb[sb.Length - 1] != '_')
						sb.Append('_');
				}
				prevWasUpper = isUpper;
				sb.Append(upper ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
			}
			return sb.ToString();
		}
	}

	sealed class CSharpIdentifierConverter : IdentifierConverter {
		public static IdentifierConverter Create() => new CSharpIdentifierConverter();
		CSharpIdentifierConverter() { }
		public override string Type(string name) => name;
		public override string Field(string name) => name;
		public override string EnumField(string name) => name;
		public override string PropertyDoc(string name) => name;
		public override string MethodDoc(string name) => name;
		public override string Method(string name) => name;
		public override string Constant(string name) => name;
		public override string Static(string name) => name;
		public override string Namespace(string name) => name;
		public override string Argument(string name) => name;
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

		public override string Constant(string name) =>
			name switch {
				"Cmpxchg486A" => "CMPXCHG486A",
				"NoMPFX_0FBC" => "NO_MPFX_0FBC",
				"NoMPFX_0FBD" => "NO_MPFX_0FBD",
				"NoLahfSahf64" => "NO_LAHF_SAHF_64",
				"DecoderOptions_Cmpxchg486A" => "DECODER_OPTIONS_CMPXCHG486A",
				"DecoderOptions_NoMPFX_0FBC" => "DECODER_OPTIONS_NO_MPFX_0FBC",
				"DecoderOptions_NoMPFX_0FBD" => "DECODER_OPTIONS_NO_MPFX_0FBD",
				"DecoderOptions_NoLahfSahf64" => "DECODER_OPTIONS_NO_LAHF_SAHF_64",
				"OpKind_MemoryESDI" => "OP_KIND_MEMORY_ES_DI",
				"OpKind_MemoryESEDI" => "OP_KIND_MEMORY_ES_EDI",
				"OpKind_MemoryESRDI" => "OP_KIND_MEMORY_ES_RDI",
				"HighLegacy8BitRegs" => "HIGH_LEGACY_8_BIT_REGS",
				_ => ToScreamingSnakeCase(name),
			};

		public override string Static(string name) => ToScreamingSnakeCase(name);
		public override string Namespace(string name) => ToSnakeCase(name);
		public override string Argument(string name) => ToSnakeCase(name);
	}
}
