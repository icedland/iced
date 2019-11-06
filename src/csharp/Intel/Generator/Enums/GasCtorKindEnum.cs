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
	static class GasCtorKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("Previous"),
				new EnumValue("Normal_1"),
				new EnumValue("Normal_2a"),
				new EnumValue("Normal_2b"),
				new EnumValue("Normal_2c"),
				new EnumValue("Normal_3"),
				new EnumValue("AamAad"),
				new EnumValue("asz"),
				new EnumValue("bnd2_2"),
				new EnumValue("bnd2_3"),
				new EnumValue("DeclareData"),
				new EnumValue("er_2"),
				new EnumValue("er_4"),
				new EnumValue("far"),
				new EnumValue("imul"),
				new EnumValue("maskmovq"),
				new EnumValue("movabs"),
				new EnumValue("nop"),
				new EnumValue("OpSize"),
				new EnumValue("OpSize2_bnd"),
				new EnumValue("OpSize3"),
				new EnumValue("os_A"),
				new EnumValue("os_B"),
				new EnumValue("os_bnd"),
				new EnumValue("os_jcc"),
				new EnumValue("os_loop"),
				new EnumValue("os_mem"),
				new EnumValue("os_mem_reg16"),
				new EnumValue("os_mem2"),
				new EnumValue("os2_3"),
				new EnumValue("os2_4"),
				new EnumValue("os2_bnd"),
				new EnumValue("pblendvb"),
				new EnumValue("pclmulqdq"),
				new EnumValue("pops"),
				new EnumValue("Reg16"),
				new EnumValue("sae"),
				new EnumValue("sae_pops"),
				new EnumValue("ST_STi"),
				new EnumValue("STi_ST"),
				new EnumValue("STi_ST2"),
				new EnumValue("STIG_1a"),
				new EnumValue("STIG_1b"),
				new EnumValue("xbegin"),
			};

		public static readonly EnumType Instance = new EnumType("CtorKind", EnumKind.GasCtorKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
