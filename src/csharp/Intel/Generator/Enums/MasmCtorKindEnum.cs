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
	static class MasmCtorKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("Previous"),
				new EnumValue("Normal_1"),
				new EnumValue("Normal_2"),
				new EnumValue("AamAad"),
				new EnumValue("AX"),
				new EnumValue("AY"),
				new EnumValue("bnd_1"),
				new EnumValue("bnd_2"),
				new EnumValue("DeclareData"),
				new EnumValue("DX"),
				new EnumValue("fword"),
				new EnumValue("Ib"),
				new EnumValue("imul"),
				new EnumValue("invlpga"),
				new EnumValue("jcc"),
				new EnumValue("maskmovq"),
				new EnumValue("memsize"),
				new EnumValue("mmxmem_1"),
				new EnumValue("mmxmem_2"),
				new EnumValue("monitor"),
				new EnumValue("mwait"),
				new EnumValue("mwaitx"),
				new EnumValue("nop"),
				new EnumValue("OpSize_1"),
				new EnumValue("OpSize_2"),
				new EnumValue("OpSize2"),
				new EnumValue("OpSize2_bnd"),
				new EnumValue("pblendvb"),
				new EnumValue("pclmulqdq"),
				new EnumValue("pops_2"),
				new EnumValue("pops_3"),
				new EnumValue("pushm"),
				new EnumValue("reg"),
				new EnumValue("Reg16"),
				new EnumValue("reverse2"),
				new EnumValue("ST_STi"),
				new EnumValue("STi_ST"),
				new EnumValue("STi_ST2"),
				new EnumValue("STIG1_1"),
				new EnumValue("STIG1_2"),
				new EnumValue("XLAT"),
				new EnumValue("XY"),
				new EnumValue("YA"),
				new EnumValue("YD"),
				new EnumValue("YX"),
			};

		public static readonly EnumType Instance = new EnumType("CtorKind", TypeIds.MasmCtorKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
