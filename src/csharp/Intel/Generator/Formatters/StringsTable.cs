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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Generator.IO;

namespace Generator.Formatters {
	abstract class StringsTable {
		public abstract void Add(string s, bool ignoreVPrefix);
		public abstract uint GetIndex(string s, bool ignoreVPrefix, out bool hasVPrefix);
	}

	sealed class StringsTableImpl : StringsTable {
		readonly string @namespace;
		readonly string className;
		readonly string preprocessorExpr;
		readonly Dictionary<string, Info> strings;
		Info[]? sortedInfos;
		bool isFrozen;

		[DebuggerDisplay("{Count} {String}")]
		sealed class Info {
			public readonly string String;
			public int Count;
			public uint Index;
			public Info(string s) => String = s;
		}

		public StringsTableImpl(string @namespace, string className, string preprocessorExpr) {
			this.@namespace = @namespace;
			this.className = className;
			this.preprocessorExpr = preprocessorExpr;
			strings = new Dictionary<string, Info>(StringComparer.Ordinal);
		}

		public void Freeze() {
			if (isFrozen)
				throw new InvalidOperationException();
			isFrozen = true;

			var infos = strings.Values.ToArray();
			Array.Sort(infos, InfoSorter);
			for (int i = 0; i < infos.Length; i++)
				infos[i].Index = (uint)i;
			sortedInfos = infos;
		}

		static int InfoSorter(Info x, Info y) {
			int c = y.Count - x.Count;
			if (c != 0)
				return c;
			return StringComparer.Ordinal.Compare(x.String, y.String);
		}

		public override void Add(string s, bool ignoreVPrefix) {
			if (isFrozen)
				throw new InvalidOperationException();
			if (ignoreVPrefix && s.StartsWith("v", StringComparison.Ordinal))
				s = s.Substring(1);
			if (!strings.TryGetValue(s, out var info))
				strings.Add(s, info = new Info(s));
			info.Count++;
		}

		public override uint GetIndex(string s, bool ignoreVPrefix, out bool hasVPrefix) {
			if (!isFrozen)
				throw new InvalidOperationException();
			if (ignoreVPrefix && s.StartsWith("v", StringComparison.Ordinal)) {
				s = s.Substring(1);
				hasVPrefix = true;
			}
			else
				hasVPrefix = false;
			if (!strings.TryGetValue(s, out var info))
				throw new ArgumentException();
			return info.Index;
		}

		public void Serialize(FileWriter writer) {
			if (!isFrozen)
				throw new InvalidOperationException();
			if (sortedInfos is null)
				throw new InvalidOperationException();

			int maxStringLength = 0;
			foreach (var info in sortedInfos)
				maxStringLength = Math.Max(maxStringLength, info.String.Length);

			writer.WriteCSharpHeader();
			if (!(preprocessorExpr is null))
				writer.WriteLine($"#if {preprocessorExpr}");
			writer.WriteLine($"namespace {@namespace} {{");
			writer.Indent();
			writer.WriteLine($"static partial class {className} {{");
			writer.Indent();
			writer.WriteLine($"const int MaxStringLength = {maxStringLength};");
			writer.WriteLine($"const int StringsCount = {sortedInfos.Length};");
			writer.WriteLine("static byte[] GetSerializedStrings() =>");
			writer.Indent();
			writer.WriteLine("new byte[] {");
			writer.Indent();

			foreach (var info in sortedInfos) {
				var s = info.String;
				if (s.Length > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)s.Length);
				foreach (var c in s) {
					if ((ushort)c > byte.MaxValue)
						throw new InvalidOperationException();
					writer.WriteByte((byte)c);
				}
				writer.WriteCommentLine(s);
			}

			writer.Unindent();
			writer.WriteLine("};");
			writer.Unindent();
			writer.Unindent();
			writer.WriteLine("}");
			writer.Unindent();
			writer.WriteLine("}");
			if (!(preprocessorExpr is null))
				writer.WriteLine("#endif");
		}
	}
}
#endif
