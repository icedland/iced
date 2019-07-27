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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System;
using System.IO;
using Generator.IO;
using Iced.Intel;
using Iced.Intel.IntelFormatterInternal;

namespace Generator.Formatters.Intel {
	sealed class IntelFormatterTableSerializer : FormatterTableSerializer {
		public override void Initialize(StringsTable stringsTable) {
			foreach (var info in CtorInfos.Infos) {
				foreach (var o in info) {
					if (o is string s)
						stringsTable.Add(s);
				}
			}
		}

		public override string GetFilename(string icedProjectDir) =>
			Path.Combine(icedProjectDir, "Intel", "IntelFormatterInternal", "InstrInfos.g.cs");

		public override void Serialize(FileWriter writer, StringsTable stringsTable) {
			writer.WriteHeader();
			writer.WriteLine("#if !NO_INTEL_FORMATTER && !NO_FORMATTER");
			writer.WriteLine("namespace Iced.Intel.IntelFormatterInternal {");
			writer.Indent();
			writer.WriteLine("static partial class InstrInfos {");
			writer.Indent();
			writer.WriteLine("static byte[] GetSerializedInstrInfos() =>");
			writer.Indent();
			writer.WriteLine("new byte[] {");
			writer.Indent();

			int index = -1;
			foreach (var info in CtorInfos.Infos) {
				index++;
				var ctorKind = (CtorKind)info[0];
				var code = (Code)info[1];
				if (code != (Code)index)
					throw new InvalidOperationException();

				if (index != 0)
					writer.WriteLine();
				writer.WriteComment(code.ToString());
				writer.WriteLine();

				if ((uint)ctorKind > byte.MaxValue)
					throw new InvalidOperationException();
				writer.WriteByte((byte)ctorKind);
				writer.WriteComment($"{ctorKind}");
				writer.WriteLine();
				uint si;
				for (int i = 2; i < info.Length; i++) {
					switch (info[i]) {
					case string s:
						writer.WriteCompressedUInt32(si = stringsTable.GetIndex(s));
						writer.WriteComment($"{si} = \"{s}\"");
						break;

					case InstrOpInfoFlags flags:
						writer.WriteCompressedUInt32((uint)flags);
						writer.WriteComment($"0x{(uint)flags:X} = {flags.ToString()}");
						break;

					case int ival:
						writer.WriteCompressedUInt32((uint)ival);
						writer.WriteComment($"0x{ival:X}");
						break;

					case PseudoOpsKind pseudoOpsKind:
						if ((uint)pseudoOpsKind > byte.MaxValue)
							throw new InvalidOperationException();
						writer.WriteByte((byte)pseudoOpsKind);
						writer.WriteComment(pseudoOpsKind.ToString());
						break;

					case Register register:
						if ((uint)register > byte.MaxValue)
							throw new InvalidOperationException();
						writer.WriteByte((byte)register);
						writer.WriteComment(register.ToString());
						break;

					case bool b:
						writer.WriteByte((byte)(b ? 1 : 0));
						writer.WriteComment(b.ToString());
						break;

					default:
						throw new InvalidOperationException();
					}
					writer.WriteLine();
				}
			}

			writer.Unindent();
			writer.WriteLine("};");
			writer.Unindent();
			writer.Unindent();
			writer.WriteLine("}");
			writer.Unindent();
			writer.WriteLine("}");
			writer.WriteLine("#endif");
		}
	}
}
#endif
