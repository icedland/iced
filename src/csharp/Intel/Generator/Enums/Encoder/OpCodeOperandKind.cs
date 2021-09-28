// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("OpCodeOperandKind", Documentation = "Operand kind", Public = true)]
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
		[Comment("Memory (modrm), MPX:#(p:)#16/32-bit mode: must be 32-bit addressing#(p:)#64-bit mode: 64-bit addressing is forced and must not be RIP relative")]
		mem_mpx,
		[Comment("Memory (modrm), MPX:#(p:)#16/32-bit mode: must be 32-bit addressing#(p:)#64-bit mode: 64-bit addressing is forced and must not be RIP relative")]
		mem_mib,
		[Comment("Memory (modrm), vsib32, #(c:XMM)# registers")]
		mem_vsib32x,
		[Comment("Memory (modrm), vsib64, #(c:XMM)# registers")]
		mem_vsib64x,
		[Comment("Memory (modrm), vsib32, #(c:YMM)# registers")]
		mem_vsib32y,
		[Comment("Memory (modrm), vsib64, #(c:YMM)# registers")]
		mem_vsib64y,
		[Comment("Memory (modrm), vsib32, #(c:ZMM)# registers")]
		mem_vsib32z,
		[Comment("Memory (modrm), vsib64, #(c:ZMM)# registers")]
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
		[Comment("#(c:MM)# register or memory")]
		mm_or_mem,
		[Comment("#(c:XMM)# register or memory")]
		xmm_or_mem,
		[Comment("#(c:YMM)# register or memory")]
		ymm_or_mem,
		[Comment("#(c:ZMM)# register or memory")]
		zmm_or_mem,
		[Comment("#(c:BND)# register or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced")]
		bnd_or_mem_mpx,
		[Comment("#(c:K)# register or memory")]
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
		[Comment("#(c:K)# register encoded in the #(c:reg)# field of the modrm byte")]
		k_reg,
		[Comment("#(c:K)# register (+1) encoded in the #(c:reg)# field of the modrm byte")]
		kp1_reg,
		[Comment("#(c:K)# register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		k_rm,
		[Comment("#(c:K)# register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/MVEX/XOP)")]
		k_vvvv,
		[Comment("#(c:MM)# register encoded in the #(c:reg)# field of the modrm byte")]
		mm_reg,
		[Comment("#(c:MM)# register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		mm_rm,
		[Comment("#(c:XMM)# register encoded in the #(c:reg)# field of the modrm byte")]
		xmm_reg,
		[Comment("#(c:XMM)# register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		xmm_rm,
		[Comment("#(c:XMM)# register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		xmm_vvvv,
		[Comment("#(c:XMM)# register (+3) encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		xmmp3_vvvv,
		[Comment("#(c:XMM)# register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only #(c:XMM0)#-#(c:XMM15)#)")]
		xmm_is4,
		[Comment("#(c:XMM)# register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only #(c:XMM0)#-#(c:XMM15)#)")]
		xmm_is5,
		[Comment("#(c:YMM)# register encoded in the #(c:reg)# field of the modrm byte")]
		ymm_reg,
		[Comment("#(c:YMM)# register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		ymm_rm,
		[Comment("#(c:YMM)# register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		ymm_vvvv,
		[Comment("#(c:YMM)# register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only #(c:YMM0)#-#(c:YMM15)#)")]
		ymm_is4,
		[Comment("#(c:YMM)# register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only #(c:YMM0)#-#(c:YMM15)#)")]
		ymm_is5,
		[Comment("#(c:ZMM)# register encoded in the #(c:reg)# field of the modrm byte")]
		zmm_reg,
		[Comment("#(c:ZMM)# register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		zmm_rm,
		[Comment("#(c:ZMM)# register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/MVEX/XOP)")]
		zmm_vvvv,
		[Comment("#(c:ZMM)# register (+3) encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		zmmp3_vvvv,
		[Comment("#(c:CR)# register encoded in the #(c:reg)# field of the modrm byte")]
		cr_reg,
		[Comment("#(c:DR)# register encoded in the #(c:reg)# field of the modrm byte")]
		dr_reg,
		[Comment("#(c:TR)# register encoded in the #(c:reg)# field of the modrm byte")]
		tr_reg,
		[Comment("#(c:BND)# register encoded in the #(c:reg)# field of the modrm byte")]
		bnd_reg,
		[Comment("#(c:ES)# register")]
		es,
		[Comment("#(c:CS)# register")]
		cs,
		[Comment("#(c:SS)# register")]
		ss,
		[Comment("#(c:DS)# register")]
		ds,
		[Comment("#(c:FS)# register")]
		fs,
		[Comment("#(c:GS)# register")]
		gs,
		[Comment("#(c:AL)# register")]
		al,
		[Comment("#(c:CL)# register")]
		cl,
		[Comment("#(c:AX)# register")]
		ax,
		[Comment("#(c:DX)# register")]
		dx,
		[Comment("#(c:EAX)# register")]
		eax,
		[Comment("#(c:RAX)# register")]
		rax,
		[Comment("#(c:ST(0))# register")]
		st0,
		[Comment("#(c:ST(i))# register encoded in the low 3 bits of the opcode")]
		sti_opcode,
		[Comment("4-bit immediate (m2z field, low 4 bits of the /is5 immediate, eg. #(c:VPERMIL2PS)#)")]
		imm4_m2z,
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
		[Comment("Memory (modrm) and the sib byte must be present")]
		sibmem,
		[Comment("#(c:TMM)# register encoded in the #(c:reg)# field of the modrm byte")]
		tmm_reg,
		[Comment("#(c:TMM)# register encoded in the #(c:mod + r/m)# fields of the modrm byte")]
		tmm_rm,
		[Comment("#(c:TMM)# register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)")]
		tmm_vvvv,
	}
}
