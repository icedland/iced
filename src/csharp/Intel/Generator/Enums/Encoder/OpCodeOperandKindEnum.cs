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

namespace Generator.Enums.Encoder {
	enum OpCodeOperandKind {
		[Comment("No operand")]
		None,
		[Comment("Far branch 16-bit offset, 16-bit segment/selector")]
		farbr2_2,
		[Comment("Far branch 32-bit offset, 16-bit segment/selector")]
		farbr4_2,
		[Comment("Memory offset without a modrm byte (eg. #(c:MOV AL,[offset])#)")]
		mem_offs,
		[Comment("Memory (modrm)")]
		mem,
		[Comment("Memory (modrm), MPX:#(p:)#16/32-bit mode: must be 32-bit addressing#(p:)#64-bit mode: 64-bit addressing is forced")]
		mem_mpx,
		[Comment("Memory (modrm), MPX:#(p:)#16/32-bit mode: must be 32-bit addressing#(p:)#64-bit mode: 64-bit addressing is forced and must not be RIP relative")]
		mem_mib,
		[Comment("Memory (modrm), vsib32, xmm registers")]
		mem_vsib32x,
		[Comment("Memory (modrm), vsib64, xmm registers")]
		mem_vsib64x,
		[Comment("Memory (modrm), vsib32, ymm registers")]
		mem_vsib32y,
		[Comment("Memory (modrm), vsib64, ymm registers")]
		mem_vsib64y,
		[Comment("Memory (modrm), vsib32, zmm registers")]
		mem_vsib32z,
		[Comment("Memory (modrm), vsib64, zmm registers")]
		mem_vsib64z,
		[Comment("8-bit GPR or memory")]
		r8_or_mem,
		[Comment("16-bit GPR or memory")]
		r16_or_mem,
		[Comment("32-bit GPR or memory")]
		r32_or_mem,
		[Comment("32-bit GPR or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced")]
		r32_or_mem_mpx,
		[Comment("64-bit GPR or memory")]
		r64_or_mem,
		[Comment("64-bit GPR or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced")]
		r64_or_mem_mpx,
		[Comment("MM register or memory")]
		mm_or_mem,
		[Comment("XMM register or memory")]
		xmm_or_mem,
		[Comment("YMM register or memory")]
		ymm_or_mem,
		[Comment("ZMM register or memory")]
		zmm_or_mem,
		[Comment("BND register or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced")]
		bnd_or_mem_mpx,
		[Comment("K register or memory")]
		k_or_mem,
		[Comment("8-bit GPR encoded in the #(c:reg)# field of the modrm byte")]
		r8_reg,
		[Comment("8-bit GPR encoded in the low 3 bits of the opcode")]
		r8_opcode,
		[Comment("16-bit GPR encoded in the #(c:reg)# field of the modrm byte")]
		r16_reg,
		[Comment("16-bit GPR encoded in the #(c:reg)# field of the modrm byte. This is a memory operand and it uses the address size prefix (#(c:67h)#) not the operand size prefix (#(c:66h)#).")]
		r16_reg_mem,
		[Comment("16-bit GPR encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		r16_rm,
		[Comment("16-bit GPR encoded in the low 3 bits of the opcode")]
		r16_opcode,
		[Comment("32-bit GPR encoded in the #(c:reg)# field of the modrm byte")]
		r32_reg,
		[Comment("32-bit GPR encoded in the #(c:reg)# field of the modrm byte. This is a memory operand and it uses the address size prefix (#(c:67h)#) not the operand size prefix (#(c:66h)#).")]
		r32_reg_mem,
		[Comment("32-bit GPR encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		r32_rm,
		[Comment("32-bit GPR encoded in the low 3 bits of the opcode")]
		r32_opcode,
		[Comment("32-bit GPR encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		r32_vvvv,
		[Comment("64-bit GPR encoded in the #(c:reg)# field of the modrm byte")]
		r64_reg,
		[Comment("64-bit GPR encoded in the #(c:reg)# field of the modrm byte. This is a memory operand and it uses the address size prefix (#(c:67h)#) not the operand size prefix (#(c:66h)#).")]
		r64_reg_mem,
		[Comment("64-bit GPR encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		r64_rm,
		[Comment("64-bit GPR encoded in the low 3 bits of the opcode")]
		r64_opcode,
		[Comment("64-bit GPR encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		r64_vvvv,
		[Comment("Segment register encoded in the #(c:reg)# field of the modrm byte")]
		seg_reg,
		[Comment("K register encoded in the #(c:reg)# field of the modrm byte")]
		k_reg,
		[Comment("K register (+1) encoded in the #(c:reg)# field of the modrm byte")]
		kp1_reg,
		[Comment("K register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		k_rm,
		[Comment("K register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		k_vvvv,
		[Comment("MM register encoded in the #(c:reg)# field of the modrm byte")]
		mm_reg,
		[Comment("MM register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		mm_rm,
		[Comment("XMM register encoded in the #(c:reg)# field of the modrm byte")]
		xmm_reg,
		[Comment("XMM register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		xmm_rm,
		[Comment("XMM register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		xmm_vvvv,
		[Comment("XMM register (+3) encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		xmmp3_vvvv,
		[Comment("XMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only XMM0-XMM15)")]
		xmm_is4,
		[Comment("XMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only XMM0-XMM15)")]
		xmm_is5,
		[Comment("YMM register encoded in the #(c:reg)# field of the modrm byte")]
		ymm_reg,
		[Comment("YMM register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		ymm_rm,
		[Comment("YMM register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		ymm_vvvv,
		[Comment("YMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only YMM0-YMM15)")]
		ymm_is4,
		[Comment("YMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only YMM0-YMM15)")]
		ymm_is5,
		[Comment("ZMM register encoded in the #(c:reg)# field of the modrm byte")]
		zmm_reg,
		[Comment("ZMM register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		zmm_rm,
		[Comment("ZMM register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		zmm_vvvv,
		[Comment("ZMM register (+3) encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		zmmp3_vvvv,
		[Comment("CR register encoded in the #(c:reg)# field of the modrm byte")]
		cr_reg,
		[Comment("DR register encoded in the #(c:reg)# field of the modrm byte")]
		dr_reg,
		[Comment("TR register encoded in the #(c:reg)# field of the modrm byte")]
		tr_reg,
		[Comment("BND register encoded in the #(c:reg)# field of the modrm byte")]
		bnd_reg,
		[Comment("ES register")]
		es,
		[Comment("CS register")]
		cs,
		[Comment("SS register")]
		ss,
		[Comment("DS register")]
		ds,
		[Comment("FS register")]
		fs,
		[Comment("GS register")]
		gs,
		[Comment("AL register")]
		al,
		[Comment("CL register")]
		cl,
		[Comment("AX register")]
		ax,
		[Comment("DX register")]
		dx,
		[Comment("EAX register")]
		eax,
		[Comment("RAX register")]
		rax,
		[Comment("ST0 register")]
		st0,
		[Comment("ST(i) register encoded in the low 3 bits of the opcode")]
		sti_opcode,
		[Comment("2-bit immediate (m2z field, low 2 bits of the /is5 immediate, eg. #(c:VPERMIL2PS)#)")]
		imm2_m2z,
		[Comment("8-bit immediate")]
		imm8,
		[Comment("Constant 1 (8-bit immediate)")]
		imm8_const_1,
		[Comment("8-bit immediate sign extended to 16 bits")]
		imm8sex16,
		[Comment("8-bit immediate sign extended to 32 bits")]
		imm8sex32,
		[Comment("8-bit immediate sign extended to 64 bits")]
		imm8sex64,
		[Comment("16-bit immediate")]
		imm16,
		[Comment("32-bit immediate")]
		imm32,
		[Comment("32-bit immediate sign extended to 64 bits")]
		imm32sex64,
		[Comment("64-bit immediate")]
		imm64,
		[Comment("#(c:seg:[rSI])# memory operand (string instructions)")]
		seg_rSI,
		[Comment("#(c:es:[rDI])# memory operand (string instructions)")]
		es_rDI,
		[Comment("#(c:seg:[rDI])# memory operand (#(c:(V)MASKMOVQ)# instructions)")]
		seg_rDI,
		[Comment("#(c:seg:[rBX+al])# memory operand (#(c:XLATB)# instruction)")]
		seg_rBX_al,
		[Comment("16-bit branch, 1-byte signed relative offset")]
		br16_1,
		[Comment("32-bit branch, 1-byte signed relative offset")]
		br32_1,
		[Comment("64-bit branch, 1-byte signed relative offset")]
		br64_1,
		[Comment("16-bit branch, 2-byte signed relative offset")]
		br16_2,
		[Comment("32-bit branch, 4-byte signed relative offset")]
		br32_4,
		[Comment("64-bit branch, 4-byte signed relative offset")]
		br64_4,
		[Comment("#(c:XBEGIN)#, 2-byte signed relative offset")]
		xbegin_2,
		[Comment("#(c:XBEGIN)#, 4-byte signed relative offset")]
		xbegin_4,
		[Comment("2-byte branch offset (#(c:JMPE)# instruction)")]
		brdisp_2,
		[Comment("4-byte branch offset (#(c:JMPE)# instruction)")]
		brdisp_4,
	}

	static class OpCodeOperandKindEnum {
		const string documentation = "Operand kind";

		static EnumValue[] GetValues() =>
			typeof(OpCodeOperandKind).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(OpCodeOperandKind)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.OpCodeOperandKind, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
