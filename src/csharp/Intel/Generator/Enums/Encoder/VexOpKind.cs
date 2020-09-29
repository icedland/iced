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
	[Enum("VexOpKind", NoInitialize = true)]
	enum VexOpKind : byte {
		None,
		imm2_m2z,
		imm8,
		k_or_mem,
		k_reg,
		k_rm,
		k_vvvv,
		mem,
		mem_vsib32x,
		mem_vsib32y,
		mem_vsib64x,
		mem_vsib64y,
		r32_or_mem,
		r32_reg,
		r32_rm,
		r32_vvvv,
		r64_or_mem,
		r64_reg,
		r64_rm,
		r64_vvvv,
		seg_rDI,
		sibmem,
		tmm_reg,
		tmm_rm,
		tmm_vvvv,
		xmm_is4,
		xmm_is5,
		xmm_or_mem,
		xmm_reg,
		xmm_rm,
		xmm_vvvv,
		ymm_is4,
		ymm_is5,
		ymm_or_mem,
		ymm_reg,
		ymm_rm,
		ymm_vvvv,
	}
}
