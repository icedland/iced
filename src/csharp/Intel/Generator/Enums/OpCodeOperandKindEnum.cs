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
	static class OpCodeOperandKindEnum {
		const string documentation = "Operand kind";

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("None", "No operand"),
				new EnumValue("farbr2_2", "Far branch 16-bit offset, 16-bit segment/selector"),
				new EnumValue("farbr4_2", "Far branch 32-bit offset, 16-bit segment/selector"),
				new EnumValue("mem_offs", "Memory offset without a modrm byte (eg. #(c:mov al,[offset])#)"),
				new EnumValue("mem", "Memory (modrm)"),
				new EnumValue("mem_mpx", "Memory (modrm), MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced"),
				new EnumValue("mem_mib", "Memory (modrm), MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced and must not be RIP relative"),
				new EnumValue("mem_vsib32x", "Memory (modrm), vsib32, xmm registers"),
				new EnumValue("mem_vsib64x", "Memory (modrm), vsib64, xmm registers"),
				new EnumValue("mem_vsib32y", "Memory (modrm), vsib32, ymm registers"),
				new EnumValue("mem_vsib64y", "Memory (modrm), vsib64, ymm registers"),
				new EnumValue("mem_vsib32z", "Memory (modrm), vsib32, zmm registers"),
				new EnumValue("mem_vsib64z", "Memory (modrm), vsib64, zmm registers"),
				new EnumValue("r8_or_mem", "8-bit GPR or memory"),
				new EnumValue("r16_or_mem", "16-bit GPR or memory"),
				new EnumValue("r32_or_mem", "32-bit GPR or memory"),
				new EnumValue("r32_or_mem_mpx", "32-bit GPR or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced"),
				new EnumValue("r64_or_mem", "64-bit GPR or memory"),
				new EnumValue("r64_or_mem_mpx", "64-bit GPR or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced"),
				new EnumValue("mm_or_mem", "MM register or memory"),
				new EnumValue("xmm_or_mem", "XMM register or memory"),
				new EnumValue("ymm_or_mem", "YMM register or memory"),
				new EnumValue("zmm_or_mem", "ZMM register or memory"),
				new EnumValue("bnd_or_mem_mpx", "BND register or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced"),
				new EnumValue("k_or_mem", "K register or memory"),
				new EnumValue("r8_reg", "8-bit GPR encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("r8_opcode", "8-bit GPR encoded in the low 3 bits of the opcode"),
				new EnumValue("r16_reg", "16-bit GPR encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("r16_rm", "16-bit GPR encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("r16_opcode", "16-bit GPR encoded in the low 3 bits of the opcode"),
				new EnumValue("r32_reg", "32-bit GPR encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("r32_rm", "32-bit GPR encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("r32_opcode", "32-bit GPR encoded in the low 3 bits of the opcode"),
				new EnumValue("r32_vvvv", "32-bit GPR encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("r64_reg", "64-bit GPR encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("r64_rm", "64-bit GPR encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("r64_opcode", "64-bit GPR encoded in the low 3 bits of the opcode"),
				new EnumValue("r64_vvvv", "64-bit GPR encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("seg_reg", "Segment register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("k_reg", "K register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("kp1_reg", "K register (+1) encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("k_rm", "K register encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("k_vvvv", "K register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("mm_reg", "MM register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("mm_rm", "MM register encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("xmm_reg", "XMM register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("xmm_rm", "XMM register encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("xmm_vvvv", "XMM register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("xmmp3_vvvv", "XMM register (+3) encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("xmm_is4", "XMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only XMM0-XMM15)"),
				new EnumValue("xmm_is5", "XMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only XMM0-XMM15)"),
				new EnumValue("ymm_reg", "YMM register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("ymm_rm", "YMM register encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("ymm_vvvv", "YMM register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("ymm_is4", "YMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only YMM0-YMM15)"),
				new EnumValue("ymm_is5", "YMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only YMM0-YMM15)"),
				new EnumValue("zmm_reg", "ZMM register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("zmm_rm", "ZMM register encoded in the #(c:mod + r/m)# fields of the modrm byte"),
				new EnumValue("zmm_vvvv", "ZMM register encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("zmmp3_vvvv", "ZMM register (+3) encoded in the the #(c:V'vvvv)# field (VEX/EVEX/XOP)"),
				new EnumValue("cr_reg", "CR register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("dr_reg", "DR register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("tr_reg", "TR register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("bnd_reg", "BND register encoded in the #(c:reg)# field of the modrm byte"),
				new EnumValue("es", "ES register"),
				new EnumValue("cs", "CS register"),
				new EnumValue("ss", "SS register"),
				new EnumValue("ds", "DS register"),
				new EnumValue("fs", "FS register"),
				new EnumValue("gs", "GS register"),
				new EnumValue("al", "AL register"),
				new EnumValue("cl", "CL register"),
				new EnumValue("ax", "AX register"),
				new EnumValue("dx", "DX register"),
				new EnumValue("eax", "EAX register"),
				new EnumValue("rax", "RAX register"),
				new EnumValue("st0", "ST0 register"),
				new EnumValue("sti_opcode", "ST(i) register encoded in the low 3 bits of the opcode"),
				new EnumValue("imm2_m2z", "2-bit immediate (m2z field, low 2 bits of the /is5 immediate, eg. #(c:vpermil2ps)#)"),
				new EnumValue("imm8", "8-bit immediate"),
				new EnumValue("imm8_const_1", "Constant 1 (8-bit immediate)"),
				new EnumValue("imm8sex16", "8-bit immediate sign extended to 16 bits"),
				new EnumValue("imm8sex32", "8-bit immediate sign extended to 32 bits"),
				new EnumValue("imm8sex64", "8-bit immediate sign extended to 64 bits"),
				new EnumValue("imm16", "16-bit immediate"),
				new EnumValue("imm32", "32-bit immediate"),
				new EnumValue("imm32sex64", "32-bit immediate sign extended to 64 bits"),
				new EnumValue("imm64", "64-bit immediate"),
				new EnumValue("seg_rSI", "#(c:seg:[rSI])# memory operand (string instructions)"),
				new EnumValue("es_rDI", "#(c:es:[rDI])# memory operand (string instructions)"),
				new EnumValue("seg_rDI", "#(c:seg:[rDI])# memory operand (#(c:(v)maskmovq)# instructions)"),
				new EnumValue("seg_rBX_al", "#(c:seg:[rBX+al])# memory operand (#(c:xlatb)# instruction)"),
				new EnumValue("br16_1", "16-bit branch, 1-byte signed relative offset"),
				new EnumValue("br32_1", "32-bit branch, 1-byte signed relative offset"),
				new EnumValue("br64_1", "64-bit branch, 1-byte signed relative offset"),
				new EnumValue("br16_2", "16-bit branch, 2-byte signed relative offset"),
				new EnumValue("br32_4", "32-bit branch, 4-byte signed relative offset"),
				new EnumValue("br64_4", "64-bit branch, 4-byte signed relative offset"),
				new EnumValue("xbegin_2", "#(c:xbegin)#, 2-byte signed relative offset"),
				new EnumValue("xbegin_4", "#(c:xbegin)#, 4-byte signed relative offset"),
				new EnumValue("brdisp_2", "2-byte branch offset (#(c:jmpe)# instruction)"),
				new EnumValue("brdisp_4", "4-byte branch offset (#(c:jmpe)# instruction)"),
			};

		public static readonly EnumType Instance = new EnumType(TypeIds.OpCodeOperandKind, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
