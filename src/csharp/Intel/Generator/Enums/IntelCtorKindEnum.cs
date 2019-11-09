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

namespace Generator.Enums {
	static class IntelCtorKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("Previous"),
				new EnumValue("Normal_1"),
				new EnumValue("Normal_2"),
				new EnumValue("asz"),
				new EnumValue("AX"),
				new EnumValue("AY"),
				new EnumValue("bcst"),
				new EnumValue("bnd_1"),
				new EnumValue("bnd_2"),
				new EnumValue("DeclareData"),
				new EnumValue("fpu_ST_STi"),
				new EnumValue("fpu_STi_ST"),
				new EnumValue("imul"),
				new EnumValue("k1"),
				new EnumValue("k2"),
				new EnumValue("maskmovq"),
				new EnumValue("memsize"),
				new EnumValue("movabs"),
				new EnumValue("nop"),
				new EnumValue("nop0F1F"),
				new EnumValue("os2"),
				new EnumValue("os3"),
				new EnumValue("os_bnd"),
				new EnumValue("os_jcc_2"),
				new EnumValue("os_jcc_3"),
				new EnumValue("os_loop"),
				new EnumValue("os_mem"),
				new EnumValue("pclmulqdq"),
				new EnumValue("pops"),
				new EnumValue("reg"),
				new EnumValue("Reg16"),
				new EnumValue("ST_STi"),
				new EnumValue("ST1_2"),
				new EnumValue("ST1_3"),
				new EnumValue("ST2"),
				new EnumValue("STi_ST"),
				new EnumValue("xbegin"),
				new EnumValue("YA"),
			};

		public static readonly EnumType Instance = new EnumType("CtorKind", TypeIds.IntelCtorKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
