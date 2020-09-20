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

namespace Generator.Enums.Formatter.Nasm {
	[Enum(nameof(CtorKind), "NasmCtorKind")]
	enum CtorKind {
		Previous,
		Normal_1,
		Normal_2,
		AamAad,
		asz,
		String,
		STIG2_2b,
		bcst,
		bnd,
		SEX2_4,
		DeclareData,
		XLAT,
		er_2,
		er_3,
		far,
		far_mem,
		invlpga,
		maskmovq,
		SEX2_3,
		STIG1,
		STIG2_2a,
		movabs,
		sae,
		nop,
		OpSize,
		OpSize2_bnd,
		OpSize3,
		os_2,
		os_3,
		os_call,
		SEX1a,
		CC_1,
		CC_2,
		CC_3,
		os_jcc_a_1,
		os_jcc_a_2,
		os_jcc_a_3,
		os_jcc_b_1,
		os_jcc_b_2,
		os_jcc_b_3,
		os_loopcc,
		os_loop,
		os_mem,
		os_mem_reg16,
		os_mem2,
		pblendvb,
		SEX1,
		pclmulqdq,
		pops,
		SEX3,
		Reg16,
		Reg32,
		reverse,
	}
}
