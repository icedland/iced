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
	static class NasmCtorKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("Previous"),
				new EnumValue("Normal_1"),
				new EnumValue("Normal_2"),
				new EnumValue("AamAad"),
				new EnumValue("asz"),
				new EnumValue("AX"),
				new EnumValue("AY"),
				new EnumValue("bcst"),
				new EnumValue("bnd_1"),
				new EnumValue("bnd_2"),
				new EnumValue("DeclareData"),
				new EnumValue("DX"),
				new EnumValue("er_2"),
				new EnumValue("er_3"),
				new EnumValue("far"),
				new EnumValue("far_mem"),
				new EnumValue("invlpga"),
				new EnumValue("maskmovq"),
				new EnumValue("mmxmem_1"),
				new EnumValue("mmxmem_2"),
				new EnumValue("mmxmem_3"),
				new EnumValue("movabs"),
				new EnumValue("ms_pops"),
				new EnumValue("nop"),
				new EnumValue("OpSize"),
				new EnumValue("OpSize2_bnd"),
				new EnumValue("OpSize3"),
				new EnumValue("os_2"),
				new EnumValue("os_3"),
				new EnumValue("os_call_2"),
				new EnumValue("os_call_3"),
				new EnumValue("os_jcc_2"),
				new EnumValue("os_jcc_3"),
				new EnumValue("os_loop"),
				new EnumValue("os_mem"),
				new EnumValue("os_mem_reg16"),
				new EnumValue("os_mem2"),
				new EnumValue("pblendvb_1"),
				new EnumValue("pblendvb_2"),
				new EnumValue("pclmulqdq"),
				new EnumValue("pops_2"),
				new EnumValue("pops_3"),
				new EnumValue("Reg16"),
				new EnumValue("reverse2"),
				new EnumValue("sae"),
				new EnumValue("sae_pops"),
				new EnumValue("SEX1"),
				new EnumValue("SEX1a"),
				new EnumValue("SEX2_2"),
				new EnumValue("SEX2_3"),
				new EnumValue("SEX2_4"),
				new EnumValue("SEX3"),
				new EnumValue("STIG1_1"),
				new EnumValue("STIG1_2"),
				new EnumValue("STIG2_2a"),
				new EnumValue("STIG2_2b"),
				new EnumValue("xbegin"),
				new EnumValue("XLAT"),
				new EnumValue("XY"),
				new EnumValue("YA"),
				new EnumValue("YD"),
				new EnumValue("YX"),
			};

		public static readonly EnumType Instance = new EnumType("CtorKind", TypeIds.NasmCtorKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
