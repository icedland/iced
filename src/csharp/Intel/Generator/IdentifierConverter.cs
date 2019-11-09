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
		public abstract string EnumField(string name);
		public abstract string Constant(string name);

		protected string ToSnakeCase(string name) => ToSnakeCase(name, upper: false);
		protected string ToScreamingSnakeCase(string name) => ToSnakeCase(name, upper: true);

		string ToSnakeCase(string name, bool upper) {
			sb.Clear();
			foreach (var c in name) {
				if (char.IsUpper(c)) {
					if (sb.Length > 0)
						sb.Append('_');
				}
				sb.Append(upper ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
			}
			return sb.ToString();
		}
	}

	sealed class CSharpIdentifierConverter : IdentifierConverter {
		public static readonly IdentifierConverter Instance = new CSharpIdentifierConverter();
		public override string Type(string name) => name;
		public override string EnumField(string name) => name;
		public override string Constant(string name) => name;
	}

	sealed class RustIdentifierConverter : IdentifierConverter {
		public static readonly RustIdentifierConverter Instance = new RustIdentifierConverter();
		public override string Type(string name) => name;
		public override string EnumField(string name) => name;
		public override string Constant(string name) => ToScreamingSnakeCase(name);
	}
}
