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

namespace Generator.Enums.Encoder {
	[Enum("LegacyOpKind", NoInitialize = true)]
	enum LegacyOpKind : byte {
		None,
		al,
		ax,
		bnd_or_mem_mpx,
		bnd_reg,
		br16_1,
		br16_2,
		br32_1,
		br32_4,
		br64_1,
		br64_4,
		brdisp_2,
		brdisp_4,
		cl,
		cr_reg,
		cs,
		dr_reg,
		ds,
		dx,
		eax,
		es,
		es_rDI,
		farbr2_2,
		farbr4_2,
		fs,
		gs,
		imm16,
		imm32,
		imm32sex64,
		imm64,
		imm8,
		imm8_const_1,
		imm8sex16,
		imm8sex32,
		imm8sex64,
		mem,
		mem_mib,
		mem_mpx,
		mem_offs,
		mm_or_mem,
		mm_reg,
		mm_rm,
		r16_opcode,
		r16_or_mem,
		r16_reg,
		r16_reg_mem,
		r16_rm,
		r32_opcode,
		r32_or_mem,
		r32_or_mem_mpx,
		r32_reg,
		r32_reg_mem,
		r32_rm,
		r64_opcode,
		r64_or_mem,
		r64_or_mem_mpx,
		r64_reg,
		r64_reg_mem,
		r64_rm,
		r8_opcode,
		r8_or_mem,
		r8_reg,
		rax,
		seg_rBX_al,
		seg_rDI,
		seg_reg,
		seg_rSI,
		ss,
		st0,
		sti_opcode,
		tr_reg,
		xbegin_2,
		xbegin_4,
		xmm_or_mem,
		xmm_reg,
		xmm_rm,
	}
}
