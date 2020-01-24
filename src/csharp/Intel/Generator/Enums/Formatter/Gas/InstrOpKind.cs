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

using System.Linq;

namespace Generator.Enums.Formatter.Gas {
	static class InstrOpKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() {
			var list = OpKindEnum.Instance.Values.Select(a => new EnumValue(a.Value, a.RawName, null)).ToList();
			// Extra opkinds
			list.Add(new EnumValue((uint)list.Count, "Sae", null));
			list.Add(new EnumValue((uint)list.Count, "RnSae", null));
			list.Add(new EnumValue((uint)list.Count, "RdSae", null));
			list.Add(new EnumValue((uint)list.Count, "RuSae", null));
			list.Add(new EnumValue((uint)list.Count, "RzSae", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareByte", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareWord", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareDword", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareQword", null));
			return list.ToArray();
		}

		public static readonly EnumType Instance = new EnumType("InstrOpKind", TypeIds.GasInstrOpKind, documentation, GetValues(), EnumTypeFlags.NoInitialize);
	}
}
