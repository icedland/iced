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

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.Enums.Encoder;
using Generator.IO;

namespace Generator.Encoder {
	abstract class EncoderGenerator {
		protected abstract void Generate(EnumType enumType);
		protected abstract void Generate((EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] legacy, (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] vex, (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] xop, (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] evex);
		protected abstract void Generate(OpCodeInfo[] opCodes);
		protected abstract void Generate((EnumValue value, uint size)[] immSizes);
		protected abstract void Generate((EnumValue allowedPrefixes, OpCodeFlags prefixes)[] infos, (EnumValue value, OpCodeFlags flag)[] flagsInfos);
		protected abstract void GenerateInstructionFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] opMaskIsK1, EnumValue[] incVecIndex, EnumValue[] noVecIndex, EnumValue[] swapVecIndex12, EnumValue[] fpuStartOpIndex1);
		protected abstract void GenerateOpCodeFormatter((EnumValue code, string result)[] notInstrStrings, EnumValue[] hasModRM, EnumValue[] hasVsib);
		protected abstract void GenerateCore();
		protected abstract void GenerateInstrSwitch(EnumValue[] jccInstr, EnumValue[] simpleBranchInstr, EnumValue[] callInstr, EnumValue[] jmpInstr, EnumValue[] xbeginInstr);
		protected abstract void GenerateVsib(EnumValue[] vsib32, EnumValue[] vsib64);

		public void Generate() {
			var enumTypes = new EnumType[] {
				EncoderTypes.EncFlags1,
				EncoderTypes.LegacyFlags3,
				EncoderTypes.VexFlags3,
				EncoderTypes.XopFlags3,
				EncoderTypes.EvexFlags3,
				EncoderTypes.AllowedPrefixes,
				EncoderTypes.LegacyFlags,
				EncoderTypes.VexFlags,
				EncoderTypes.XopFlags,
				EncoderTypes.EvexFlags,
				EncoderTypes.D3nowFlags,
			};
			foreach (var enumType in enumTypes)
				Generate(enumType);

			Generate(EncoderTypes.LegacyOpHandlers, EncoderTypes.VexOpHandlers, EncoderTypes.XopOpHandlers, EncoderTypes.EvexOpHandlers);
			Generate(OpCodeInfoTable.Data);
			Generate(EncoderTypes.ImmSizes);
			var opCodeFlags = OpCodeFlagsEnum.Instance;
			var flagsInfos = new (EnumValue value, OpCodeFlags flag)[] {
				(opCodeFlags[nameof(OpCodeFlags.LockPrefix)], OpCodeFlags.LockPrefix),
				(opCodeFlags[nameof(OpCodeFlags.XacquirePrefix)], OpCodeFlags.XacquirePrefix),
				(opCodeFlags[nameof(OpCodeFlags.XreleasePrefix)], OpCodeFlags.XreleasePrefix),
				(opCodeFlags[nameof(OpCodeFlags.RepPrefix)], OpCodeFlags.RepPrefix),
				(opCodeFlags[nameof(OpCodeFlags.RepnePrefix)], OpCodeFlags.RepnePrefix),
				(opCodeFlags[nameof(OpCodeFlags.BndPrefix)], OpCodeFlags.BndPrefix),
				(opCodeFlags[nameof(OpCodeFlags.HintTakenPrefix)], OpCodeFlags.HintTakenPrefix),
				(opCodeFlags[nameof(OpCodeFlags.NotrackPrefix)], OpCodeFlags.NotrackPrefix),
			};
			Generate(EncoderTypes.AllowedPrefixesMap.Select(a => (a.Value, a.Key)).OrderBy(a => a.Value.Value).ToArray(), flagsInfos);
			var code = CodeEnum.Instance;
			var notInstrStrings = new (EnumValue code, string result)[] {
				(code[nameof(Code.INVALID)], "<invalid>"),
				(code[nameof(Code.DeclareByte)], "<db>"),
				(code[nameof(Code.DeclareWord)], "<dw>"),
				(code[nameof(Code.DeclareDword)], "<dd>"),
				(code[nameof(Code.DeclareQword)], "<dq>"),
			};
			var opMaskIsK1 = new EnumValue[] {
				code[nameof(Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8)],
				code[nameof(Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8)],
				code[nameof(Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8)],
				code[nameof(Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8)],
				code[nameof(Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8)],
				code[nameof(Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8)],
				code[nameof(Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8)],
				code[nameof(Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8)],
				code[nameof(Code.EVEX_Vptestmb_k_k1_xmm_xmmm128)],
				code[nameof(Code.EVEX_Vptestmb_k_k1_ymm_ymmm256)],
				code[nameof(Code.EVEX_Vptestmb_k_k1_zmm_zmmm512)],
				code[nameof(Code.EVEX_Vptestmw_k_k1_xmm_xmmm128)],
				code[nameof(Code.EVEX_Vptestmw_k_k1_ymm_ymmm256)],
				code[nameof(Code.EVEX_Vptestmw_k_k1_zmm_zmmm512)],
				code[nameof(Code.EVEX_Vptestnmb_k_k1_xmm_xmmm128)],
				code[nameof(Code.EVEX_Vptestnmb_k_k1_ymm_ymmm256)],
				code[nameof(Code.EVEX_Vptestnmb_k_k1_zmm_zmmm512)],
				code[nameof(Code.EVEX_Vptestnmw_k_k1_xmm_xmmm128)],
				code[nameof(Code.EVEX_Vptestnmw_k_k1_ymm_ymmm256)],
				code[nameof(Code.EVEX_Vptestnmw_k_k1_zmm_zmmm512)],
				code[nameof(Code.EVEX_Vptestmd_k_k1_xmm_xmmm128b32)],
				code[nameof(Code.EVEX_Vptestmd_k_k1_ymm_ymmm256b32)],
				code[nameof(Code.EVEX_Vptestmd_k_k1_zmm_zmmm512b32)],
				code[nameof(Code.EVEX_Vptestmq_k_k1_xmm_xmmm128b64)],
				code[nameof(Code.EVEX_Vptestmq_k_k1_ymm_ymmm256b64)],
				code[nameof(Code.EVEX_Vptestmq_k_k1_zmm_zmmm512b64)],
				code[nameof(Code.EVEX_Vptestnmd_k_k1_xmm_xmmm128b32)],
				code[nameof(Code.EVEX_Vptestnmd_k_k1_ymm_ymmm256b32)],
				code[nameof(Code.EVEX_Vptestnmd_k_k1_zmm_zmmm512b32)],
				code[nameof(Code.EVEX_Vptestnmq_k_k1_xmm_xmmm128b64)],
				code[nameof(Code.EVEX_Vptestnmq_k_k1_ymm_ymmm256b64)],
				code[nameof(Code.EVEX_Vptestnmq_k_k1_zmm_zmmm512b64)],
			};
			var incVecIndex = new EnumValue[] {
				code[nameof(Code.VEX_Vpextrw_r32m16_xmm_imm8)],
				code[nameof(Code.VEX_Vpextrw_r64m16_xmm_imm8)],
				code[nameof(Code.EVEX_Vpextrw_r32m16_xmm_imm8)],
				code[nameof(Code.EVEX_Vpextrw_r64m16_xmm_imm8)],
				code[nameof(Code.VEX_Vmovmskpd_r32_xmm)],
				code[nameof(Code.VEX_Vmovmskpd_r64_xmm)],
				code[nameof(Code.VEX_Vmovmskpd_r32_ymm)],
				code[nameof(Code.VEX_Vmovmskpd_r64_ymm)],
				code[nameof(Code.VEX_Vmovmskps_r32_xmm)],
				code[nameof(Code.VEX_Vmovmskps_r64_xmm)],
				code[nameof(Code.VEX_Vmovmskps_r32_ymm)],
				code[nameof(Code.VEX_Vmovmskps_r64_ymm)],
				code[nameof(Code.Pextrb_r32m8_xmm_imm8)],
				code[nameof(Code.Pextrb_r64m8_xmm_imm8)],
				code[nameof(Code.Pextrd_rm32_xmm_imm8)],
				code[nameof(Code.Pextrq_rm64_xmm_imm8)],
				code[nameof(Code.VEX_Vpextrb_r32m8_xmm_imm8)],
				code[nameof(Code.VEX_Vpextrb_r64m8_xmm_imm8)],
				code[nameof(Code.VEX_Vpextrd_rm32_xmm_imm8)],
				code[nameof(Code.VEX_Vpextrq_rm64_xmm_imm8)],
				code[nameof(Code.EVEX_Vpextrb_r32m8_xmm_imm8)],
				code[nameof(Code.EVEX_Vpextrb_r64m8_xmm_imm8)],
				code[nameof(Code.EVEX_Vpextrd_rm32_xmm_imm8)],
				code[nameof(Code.EVEX_Vpextrq_rm64_xmm_imm8)],
			};
			var noVecIndex = new EnumValue[] {
				code[nameof(Code.Pxor_mm_mmm64)],
				code[nameof(Code.Punpckldq_mm_mmm32)],
				code[nameof(Code.Punpcklwd_mm_mmm32)],
				code[nameof(Code.Punpcklbw_mm_mmm32)],
				code[nameof(Code.Punpckhdq_mm_mmm64)],
				code[nameof(Code.Punpckhwd_mm_mmm64)],
				code[nameof(Code.Punpckhbw_mm_mmm64)],
				code[nameof(Code.Psubusb_mm_mmm64)],
				code[nameof(Code.Psubusw_mm_mmm64)],
				code[nameof(Code.Psubsw_mm_mmm64)],
				code[nameof(Code.Psubsb_mm_mmm64)],
				code[nameof(Code.Psubd_mm_mmm64)],
				code[nameof(Code.Psubw_mm_mmm64)],
				code[nameof(Code.Psubb_mm_mmm64)],
				code[nameof(Code.Psrlq_mm_imm8)],
				code[nameof(Code.Psrlq_mm_mmm64)],
				code[nameof(Code.Psrld_mm_imm8)],
				code[nameof(Code.Psrld_mm_mmm64)],
				code[nameof(Code.Psrlw_mm_imm8)],
				code[nameof(Code.Psrlw_mm_mmm64)],
				code[nameof(Code.Psrad_mm_imm8)],
				code[nameof(Code.Psrad_mm_mmm64)],
				code[nameof(Code.Psraw_mm_imm8)],
				code[nameof(Code.Psraw_mm_mmm64)],
				code[nameof(Code.Psllq_mm_imm8)],
				code[nameof(Code.Psllq_mm_mmm64)],
				code[nameof(Code.Pslld_mm_imm8)],
				code[nameof(Code.Pslld_mm_mmm64)],
				code[nameof(Code.Psllw_mm_mmm64)],
				code[nameof(Code.Por_mm_mmm64)],
				code[nameof(Code.Pmullw_mm_mmm64)],
				code[nameof(Code.Pmulhw_mm_mmm64)],
				code[nameof(Code.Pmovmskb_r32_mm)],
				code[nameof(Code.Pmovmskb_r64_mm)],
				code[nameof(Code.Pmovmskb_r32_xmm)],
				code[nameof(Code.Pmovmskb_r64_xmm)],
				code[nameof(Code.Pmaddwd_mm_mmm64)],
				code[nameof(Code.Pinsrw_mm_r32m16_imm8)],
				code[nameof(Code.Pinsrw_mm_r64m16_imm8)],
				code[nameof(Code.Pinsrw_xmm_r32m16_imm8)],
				code[nameof(Code.Pinsrw_xmm_r64m16_imm8)],
				code[nameof(Code.Pextrw_r32_xmm_imm8)],
				code[nameof(Code.Pextrw_r64_xmm_imm8)],
				code[nameof(Code.Pextrw_r32m16_xmm_imm8)],
				code[nameof(Code.Pextrw_r64m16_xmm_imm8)],
				code[nameof(Code.Pextrw_r32_mm_imm8)],
				code[nameof(Code.Pextrw_r64_mm_imm8)],
				code[nameof(Code.Cvtpd2pi_mm_xmmm128)],
				code[nameof(Code.Cvtpi2pd_xmm_mmm64)],
				code[nameof(Code.Cvtpi2ps_xmm_mmm64)],
				code[nameof(Code.Cvtps2pi_mm_xmmm64)],
				code[nameof(Code.Cvttpd2pi_mm_xmmm128)],
				code[nameof(Code.Cvttps2pi_mm_xmmm64)],
				code[nameof(Code.Movd_mm_rm32)],
				code[nameof(Code.Movq_mm_rm64)],
				code[nameof(Code.Movd_rm32_mm)],
				code[nameof(Code.Movq_rm64_mm)],
				code[nameof(Code.Movd_xmm_rm32)],
				code[nameof(Code.Movq_xmm_rm64)],
				code[nameof(Code.Movd_rm32_xmm)],
				code[nameof(Code.Movq_rm64_xmm)],
				code[nameof(Code.Movdq2q_mm_xmm)],
				code[nameof(Code.Movmskpd_r32_xmm)],
				code[nameof(Code.Movmskpd_r64_xmm)],
				code[nameof(Code.Movmskps_r32_xmm)],
				code[nameof(Code.Movmskps_r64_xmm)],
				code[nameof(Code.Movntq_m64_mm)],
				code[nameof(Code.Movq_mm_mmm64)],
				code[nameof(Code.Movq_mmm64_mm)],
				code[nameof(Code.Movq2dq_xmm_mm)],
				code[nameof(Code.Packuswb_mm_mmm64)],
				code[nameof(Code.Paddb_mm_mmm64)],
				code[nameof(Code.Paddw_mm_mmm64)],
				code[nameof(Code.Paddd_mm_mmm64)],
				code[nameof(Code.Paddq_mm_mmm64)],
				code[nameof(Code.Paddsb_mm_mmm64)],
				code[nameof(Code.Paddsw_mm_mmm64)],
				code[nameof(Code.Paddusb_mm_mmm64)],
				code[nameof(Code.Paddusw_mm_mmm64)],
				code[nameof(Code.Pand_mm_mmm64)],
				code[nameof(Code.Pandn_mm_mmm64)],
				code[nameof(Code.Pcmpeqb_mm_mmm64)],
				code[nameof(Code.Pcmpeqw_mm_mmm64)],
				code[nameof(Code.Pcmpeqd_mm_mmm64)],
				code[nameof(Code.Pcmpgtb_mm_mmm64)],
				code[nameof(Code.Pcmpgtw_mm_mmm64)],
				code[nameof(Code.Pcmpgtd_mm_mmm64)],
			};
			var swapVecIndex12 = new EnumValue[] {
				code[nameof(Code.Movapd_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovapd_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovapd_ymmm256_ymm)],
				code[nameof(Code.EVEX_Vmovapd_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovapd_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovapd_zmmm512_k1z_zmm)],
				code[nameof(Code.Movaps_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovaps_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovaps_ymmm256_ymm)],
				code[nameof(Code.EVEX_Vmovaps_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovaps_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovaps_zmmm512_k1z_zmm)],
				code[nameof(Code.Movdqa_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovdqa_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovdqa_ymmm256_ymm)],
				code[nameof(Code.EVEX_Vmovdqa32_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovdqa32_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovdqa32_zmmm512_k1z_zmm)],
				code[nameof(Code.EVEX_Vmovdqa64_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovdqa64_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovdqa64_zmmm512_k1z_zmm)],
				code[nameof(Code.Movdqu_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovdqu_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovdqu_ymmm256_ymm)],
				code[nameof(Code.EVEX_Vmovdqu8_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovdqu8_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovdqu8_zmmm512_k1z_zmm)],
				code[nameof(Code.EVEX_Vmovdqu16_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovdqu16_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovdqu16_zmmm512_k1z_zmm)],
				code[nameof(Code.EVEX_Vmovdqu32_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovdqu32_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovdqu32_zmmm512_k1z_zmm)],
				code[nameof(Code.EVEX_Vmovdqu64_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovdqu64_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovdqu64_zmmm512_k1z_zmm)],
				code[nameof(Code.VEX_Vmovhpd_xmm_xmm_m64)],
				code[nameof(Code.EVEX_Vmovhpd_xmm_xmm_m64)],
				code[nameof(Code.VEX_Vmovhps_xmm_xmm_m64)],
				code[nameof(Code.EVEX_Vmovhps_xmm_xmm_m64)],
				code[nameof(Code.VEX_Vmovlpd_xmm_xmm_m64)],
				code[nameof(Code.EVEX_Vmovlpd_xmm_xmm_m64)],
				code[nameof(Code.VEX_Vmovlps_xmm_xmm_m64)],
				code[nameof(Code.EVEX_Vmovlps_xmm_xmm_m64)],
				code[nameof(Code.Movq_xmmm64_xmm)],
				code[nameof(Code.Movss_xmmm32_xmm)],
				code[nameof(Code.Movupd_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovupd_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovupd_ymmm256_ymm)],
				code[nameof(Code.EVEX_Vmovupd_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovupd_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovupd_zmmm512_k1z_zmm)],
				code[nameof(Code.Movups_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovups_xmmm128_xmm)],
				code[nameof(Code.VEX_Vmovups_ymmm256_ymm)],
				code[nameof(Code.EVEX_Vmovups_xmmm128_k1z_xmm)],
				code[nameof(Code.EVEX_Vmovups_ymmm256_k1z_ymm)],
				code[nameof(Code.EVEX_Vmovups_zmmm512_k1z_zmm)],
			};
			var fpuStartOpIndex1 = new EnumValue[] {
				code[nameof(Code.Fcom_st0_sti)],
				code[nameof(Code.Fcom_st0_sti_DCD0)],
				code[nameof(Code.Fcomp_st0_sti)],
				code[nameof(Code.Fcomp_st0_sti_DCD8)],
				code[nameof(Code.Fcomp_st0_sti_DED0)],
				code[nameof(Code.Fld_st0_sti)],
				code[nameof(Code.Fucom_st0_sti)],
				code[nameof(Code.Fucomp_st0_sti)],
				code[nameof(Code.Fxch_st0_sti)],
				code[nameof(Code.Fxch_st0_sti_DDC8)],
				code[nameof(Code.Fxch_st0_sti_DFC8)],
			};
			GenerateInstructionFormatter(notInstrStrings, opMaskIsK1, incVecIndex, noVecIndex, swapVecIndex12, fpuStartOpIndex1);
			var opCodeOperandKind = OpCodeOperandKindEnum.Instance;
			var hasModRM = new EnumValue[] {
				opCodeOperandKind[nameof(OpCodeOperandKind.mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_mib)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32z)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64z)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r8_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r16_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.zmm_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.bnd_or_mem_mpx)],
				opCodeOperandKind[nameof(OpCodeOperandKind.k_or_mem)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r8_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r16_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r16_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.seg_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.k_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.kp1_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.k_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.ymm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.zmm_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.zmm_rm)],
				opCodeOperandKind[nameof(OpCodeOperandKind.cr_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.dr_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.tr_reg)],
				opCodeOperandKind[nameof(OpCodeOperandKind.bnd_reg)],
			};
			var hasVsib = new EnumValue[] {
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32z)],
				opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64z)],
			};
			GenerateOpCodeFormatter(notInstrStrings, hasModRM, hasVsib);
			GenerateCore();
			var jccInstr = new EnumValue[] {
				code[nameof(Code.Jo_rel8_16)],
				code[nameof(Code.Jo_rel8_32)],
				code[nameof(Code.Jo_rel8_64)],
				code[nameof(Code.Jno_rel8_16)],
				code[nameof(Code.Jno_rel8_32)],
				code[nameof(Code.Jno_rel8_64)],
				code[nameof(Code.Jb_rel8_16)],
				code[nameof(Code.Jb_rel8_32)],
				code[nameof(Code.Jb_rel8_64)],
				code[nameof(Code.Jae_rel8_16)],
				code[nameof(Code.Jae_rel8_32)],
				code[nameof(Code.Jae_rel8_64)],
				code[nameof(Code.Je_rel8_16)],
				code[nameof(Code.Je_rel8_32)],
				code[nameof(Code.Je_rel8_64)],
				code[nameof(Code.Jne_rel8_16)],
				code[nameof(Code.Jne_rel8_32)],
				code[nameof(Code.Jne_rel8_64)],
				code[nameof(Code.Jbe_rel8_16)],
				code[nameof(Code.Jbe_rel8_32)],
				code[nameof(Code.Jbe_rel8_64)],
				code[nameof(Code.Ja_rel8_16)],
				code[nameof(Code.Ja_rel8_32)],
				code[nameof(Code.Ja_rel8_64)],
				code[nameof(Code.Js_rel8_16)],
				code[nameof(Code.Js_rel8_32)],
				code[nameof(Code.Js_rel8_64)],
				code[nameof(Code.Jns_rel8_16)],
				code[nameof(Code.Jns_rel8_32)],
				code[nameof(Code.Jns_rel8_64)],
				code[nameof(Code.Jp_rel8_16)],
				code[nameof(Code.Jp_rel8_32)],
				code[nameof(Code.Jp_rel8_64)],
				code[nameof(Code.Jnp_rel8_16)],
				code[nameof(Code.Jnp_rel8_32)],
				code[nameof(Code.Jnp_rel8_64)],
				code[nameof(Code.Jl_rel8_16)],
				code[nameof(Code.Jl_rel8_32)],
				code[nameof(Code.Jl_rel8_64)],
				code[nameof(Code.Jge_rel8_16)],
				code[nameof(Code.Jge_rel8_32)],
				code[nameof(Code.Jge_rel8_64)],
				code[nameof(Code.Jle_rel8_16)],
				code[nameof(Code.Jle_rel8_32)],
				code[nameof(Code.Jle_rel8_64)],
				code[nameof(Code.Jg_rel8_16)],
				code[nameof(Code.Jg_rel8_32)],
				code[nameof(Code.Jg_rel8_64)],
				code[nameof(Code.Jo_rel16)],
				code[nameof(Code.Jo_rel32_32)],
				code[nameof(Code.Jo_rel32_64)],
				code[nameof(Code.Jno_rel16)],
				code[nameof(Code.Jno_rel32_32)],
				code[nameof(Code.Jno_rel32_64)],
				code[nameof(Code.Jb_rel16)],
				code[nameof(Code.Jb_rel32_32)],
				code[nameof(Code.Jb_rel32_64)],
				code[nameof(Code.Jae_rel16)],
				code[nameof(Code.Jae_rel32_32)],
				code[nameof(Code.Jae_rel32_64)],
				code[nameof(Code.Je_rel16)],
				code[nameof(Code.Je_rel32_32)],
				code[nameof(Code.Je_rel32_64)],
				code[nameof(Code.Jne_rel16)],
				code[nameof(Code.Jne_rel32_32)],
				code[nameof(Code.Jne_rel32_64)],
				code[nameof(Code.Jbe_rel16)],
				code[nameof(Code.Jbe_rel32_32)],
				code[nameof(Code.Jbe_rel32_64)],
				code[nameof(Code.Ja_rel16)],
				code[nameof(Code.Ja_rel32_32)],
				code[nameof(Code.Ja_rel32_64)],
				code[nameof(Code.Js_rel16)],
				code[nameof(Code.Js_rel32_32)],
				code[nameof(Code.Js_rel32_64)],
				code[nameof(Code.Jns_rel16)],
				code[nameof(Code.Jns_rel32_32)],
				code[nameof(Code.Jns_rel32_64)],
				code[nameof(Code.Jp_rel16)],
				code[nameof(Code.Jp_rel32_32)],
				code[nameof(Code.Jp_rel32_64)],
				code[nameof(Code.Jnp_rel16)],
				code[nameof(Code.Jnp_rel32_32)],
				code[nameof(Code.Jnp_rel32_64)],
				code[nameof(Code.Jl_rel16)],
				code[nameof(Code.Jl_rel32_32)],
				code[nameof(Code.Jl_rel32_64)],
				code[nameof(Code.Jge_rel16)],
				code[nameof(Code.Jge_rel32_32)],
				code[nameof(Code.Jge_rel32_64)],
				code[nameof(Code.Jle_rel16)],
				code[nameof(Code.Jle_rel32_32)],
				code[nameof(Code.Jle_rel32_64)],
				code[nameof(Code.Jg_rel16)],
				code[nameof(Code.Jg_rel32_32)],
				code[nameof(Code.Jg_rel32_64)],
			};
			var simpleBranchInstr = new EnumValue[] {
				code[nameof(Code.Loopne_rel8_16_CX)],
				code[nameof(Code.Loopne_rel8_32_CX)],
				code[nameof(Code.Loopne_rel8_16_ECX)],
				code[nameof(Code.Loopne_rel8_32_ECX)],
				code[nameof(Code.Loopne_rel8_64_ECX)],
				code[nameof(Code.Loopne_rel8_16_RCX)],
				code[nameof(Code.Loopne_rel8_64_RCX)],
				code[nameof(Code.Loope_rel8_16_CX)],
				code[nameof(Code.Loope_rel8_32_CX)],
				code[nameof(Code.Loope_rel8_16_ECX)],
				code[nameof(Code.Loope_rel8_32_ECX)],
				code[nameof(Code.Loope_rel8_64_ECX)],
				code[nameof(Code.Loope_rel8_16_RCX)],
				code[nameof(Code.Loope_rel8_64_RCX)],
				code[nameof(Code.Loop_rel8_16_CX)],
				code[nameof(Code.Loop_rel8_32_CX)],
				code[nameof(Code.Loop_rel8_16_ECX)],
				code[nameof(Code.Loop_rel8_32_ECX)],
				code[nameof(Code.Loop_rel8_64_ECX)],
				code[nameof(Code.Loop_rel8_16_RCX)],
				code[nameof(Code.Loop_rel8_64_RCX)],
				code[nameof(Code.Jcxz_rel8_16)],
				code[nameof(Code.Jcxz_rel8_32)],
				code[nameof(Code.Jecxz_rel8_16)],
				code[nameof(Code.Jecxz_rel8_32)],
				code[nameof(Code.Jecxz_rel8_64)],
				code[nameof(Code.Jrcxz_rel8_16)],
				code[nameof(Code.Jrcxz_rel8_64)],
			};
			var callInstr = new EnumValue[] {
				code[nameof(Code.Call_rel16)],
				code[nameof(Code.Call_rel32_32)],
				code[nameof(Code.Call_rel32_64)],
			};
			var jmpInstr = new EnumValue[] {
				code[nameof(Code.Jmp_rel16)],
				code[nameof(Code.Jmp_rel32_32)],
				code[nameof(Code.Jmp_rel32_64)],
				code[nameof(Code.Jmp_rel8_16)],
				code[nameof(Code.Jmp_rel8_32)],
				code[nameof(Code.Jmp_rel8_64)],
			};
			var xbeginInstr = new EnumValue[] {
				code[nameof(Code.Xbegin_rel16)],
				code[nameof(Code.Xbegin_rel32)],
			};
			GenerateInstrSwitch(jccInstr, simpleBranchInstr, callInstr, jmpInstr, xbeginInstr);
			var vsib32 = new EnumValue[] {
				code[nameof(Code.VEX_Vpgatherdd_xmm_vm32x_xmm)],
				code[nameof(Code.VEX_Vpgatherdd_ymm_vm32y_ymm)],
				code[nameof(Code.VEX_Vpgatherdq_xmm_vm32x_xmm)],
				code[nameof(Code.VEX_Vpgatherdq_ymm_vm32x_ymm)],
				code[nameof(Code.EVEX_Vpgatherdd_xmm_k1_vm32x)],
				code[nameof(Code.EVEX_Vpgatherdd_ymm_k1_vm32y)],
				code[nameof(Code.EVEX_Vpgatherdd_zmm_k1_vm32z)],
				code[nameof(Code.EVEX_Vpgatherdq_xmm_k1_vm32x)],
				code[nameof(Code.EVEX_Vpgatherdq_ymm_k1_vm32x)],
				code[nameof(Code.EVEX_Vpgatherdq_zmm_k1_vm32y)],
				code[nameof(Code.VEX_Vgatherdps_xmm_vm32x_xmm)],
				code[nameof(Code.VEX_Vgatherdps_ymm_vm32y_ymm)],
				code[nameof(Code.VEX_Vgatherdpd_xmm_vm32x_xmm)],
				code[nameof(Code.VEX_Vgatherdpd_ymm_vm32x_ymm)],
				code[nameof(Code.EVEX_Vgatherdps_xmm_k1_vm32x)],
				code[nameof(Code.EVEX_Vgatherdps_ymm_k1_vm32y)],
				code[nameof(Code.EVEX_Vgatherdps_zmm_k1_vm32z)],
				code[nameof(Code.EVEX_Vgatherdpd_xmm_k1_vm32x)],
				code[nameof(Code.EVEX_Vgatherdpd_ymm_k1_vm32x)],
				code[nameof(Code.EVEX_Vgatherdpd_zmm_k1_vm32y)],
				code[nameof(Code.EVEX_Vpscatterdd_vm32x_k1_xmm)],
				code[nameof(Code.EVEX_Vpscatterdd_vm32y_k1_ymm)],
				code[nameof(Code.EVEX_Vpscatterdd_vm32z_k1_zmm)],
				code[nameof(Code.EVEX_Vpscatterdq_vm32x_k1_xmm)],
				code[nameof(Code.EVEX_Vpscatterdq_vm32x_k1_ymm)],
				code[nameof(Code.EVEX_Vpscatterdq_vm32y_k1_zmm)],
				code[nameof(Code.EVEX_Vscatterdps_vm32x_k1_xmm)],
				code[nameof(Code.EVEX_Vscatterdps_vm32y_k1_ymm)],
				code[nameof(Code.EVEX_Vscatterdps_vm32z_k1_zmm)],
				code[nameof(Code.EVEX_Vscatterdpd_vm32x_k1_xmm)],
				code[nameof(Code.EVEX_Vscatterdpd_vm32x_k1_ymm)],
				code[nameof(Code.EVEX_Vscatterdpd_vm32y_k1_zmm)],
				code[nameof(Code.EVEX_Vgatherpf0dps_vm32z_k1)],
				code[nameof(Code.EVEX_Vgatherpf0dpd_vm32y_k1)],
				code[nameof(Code.EVEX_Vgatherpf1dps_vm32z_k1)],
				code[nameof(Code.EVEX_Vgatherpf1dpd_vm32y_k1)],
				code[nameof(Code.EVEX_Vscatterpf0dps_vm32z_k1)],
				code[nameof(Code.EVEX_Vscatterpf0dpd_vm32y_k1)],
				code[nameof(Code.EVEX_Vscatterpf1dps_vm32z_k1)],
				code[nameof(Code.EVEX_Vscatterpf1dpd_vm32y_k1)],
			};
			var vsib64 = new EnumValue[] {
				code[nameof(Code.VEX_Vpgatherqd_xmm_vm64x_xmm)],
				code[nameof(Code.VEX_Vpgatherqd_xmm_vm64y_xmm)],
				code[nameof(Code.VEX_Vpgatherqq_xmm_vm64x_xmm)],
				code[nameof(Code.VEX_Vpgatherqq_ymm_vm64y_ymm)],
				code[nameof(Code.EVEX_Vpgatherqd_xmm_k1_vm64x)],
				code[nameof(Code.EVEX_Vpgatherqd_xmm_k1_vm64y)],
				code[nameof(Code.EVEX_Vpgatherqd_ymm_k1_vm64z)],
				code[nameof(Code.EVEX_Vpgatherqq_xmm_k1_vm64x)],
				code[nameof(Code.EVEX_Vpgatherqq_ymm_k1_vm64y)],
				code[nameof(Code.EVEX_Vpgatherqq_zmm_k1_vm64z)],
				code[nameof(Code.VEX_Vgatherqps_xmm_vm64x_xmm)],
				code[nameof(Code.VEX_Vgatherqps_xmm_vm64y_xmm)],
				code[nameof(Code.VEX_Vgatherqpd_xmm_vm64x_xmm)],
				code[nameof(Code.VEX_Vgatherqpd_ymm_vm64y_ymm)],
				code[nameof(Code.EVEX_Vgatherqps_xmm_k1_vm64x)],
				code[nameof(Code.EVEX_Vgatherqps_xmm_k1_vm64y)],
				code[nameof(Code.EVEX_Vgatherqps_ymm_k1_vm64z)],
				code[nameof(Code.EVEX_Vgatherqpd_xmm_k1_vm64x)],
				code[nameof(Code.EVEX_Vgatherqpd_ymm_k1_vm64y)],
				code[nameof(Code.EVEX_Vgatherqpd_zmm_k1_vm64z)],
				code[nameof(Code.EVEX_Vpscatterqd_vm64x_k1_xmm)],
				code[nameof(Code.EVEX_Vpscatterqd_vm64y_k1_xmm)],
				code[nameof(Code.EVEX_Vpscatterqd_vm64z_k1_ymm)],
				code[nameof(Code.EVEX_Vpscatterqq_vm64x_k1_xmm)],
				code[nameof(Code.EVEX_Vpscatterqq_vm64y_k1_ymm)],
				code[nameof(Code.EVEX_Vpscatterqq_vm64z_k1_zmm)],
				code[nameof(Code.EVEX_Vscatterqps_vm64x_k1_xmm)],
				code[nameof(Code.EVEX_Vscatterqps_vm64y_k1_xmm)],
				code[nameof(Code.EVEX_Vscatterqps_vm64z_k1_ymm)],
				code[nameof(Code.EVEX_Vscatterqpd_vm64x_k1_xmm)],
				code[nameof(Code.EVEX_Vscatterqpd_vm64y_k1_ymm)],
				code[nameof(Code.EVEX_Vscatterqpd_vm64z_k1_zmm)],
				code[nameof(Code.EVEX_Vgatherpf0qps_vm64z_k1)],
				code[nameof(Code.EVEX_Vgatherpf0qpd_vm64z_k1)],
				code[nameof(Code.EVEX_Vgatherpf1qps_vm64z_k1)],
				code[nameof(Code.EVEX_Vgatherpf1qpd_vm64z_k1)],
				code[nameof(Code.EVEX_Vscatterpf0qps_vm64z_k1)],
				code[nameof(Code.EVEX_Vscatterpf0qpd_vm64z_k1)],
				code[nameof(Code.EVEX_Vscatterpf1qps_vm64z_k1)],
				code[nameof(Code.EVEX_Vscatterpf1qpd_vm64z_k1)],
			};
			GenerateVsib(vsib32, vsib64);
		}

		protected static IEnumerable<(OpCodeInfo opCode, uint dword1, uint dword2, uint dword3)> GetData(OpCodeInfo[] opCodes) {
			int encodingShift = (int)EncoderTypes.EncFlags1["EncodingShift"].Value;
			int opCodeShift = (int)EncoderTypes.EncFlags1["OpCodeShift"].Value;

			var legacyOpShifts = new[] {
				(int)EncoderTypes.LegacyFlags3["Op0Shift"].Value,
				(int)EncoderTypes.LegacyFlags3["Op1Shift"].Value,
				(int)EncoderTypes.LegacyFlags3["Op2Shift"].Value,
				(int)EncoderTypes.LegacyFlags3["Op3Shift"].Value,
			};
			var vexOpShifts = new[] {
				(int)EncoderTypes.VexFlags3["Op0Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op1Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op2Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op3Shift"].Value,
				(int)EncoderTypes.VexFlags3["Op4Shift"].Value,
			};
			var xopOpShifts = new[] {
				(int)EncoderTypes.XopFlags3["Op0Shift"].Value,
				(int)EncoderTypes.XopFlags3["Op1Shift"].Value,
				(int)EncoderTypes.XopFlags3["Op2Shift"].Value,
				(int)EncoderTypes.XopFlags3["Op3Shift"].Value,
			};
			var evexOpShifts = new[] {
				(int)EncoderTypes.EvexFlags3["Op0Shift"].Value,
				(int)EncoderTypes.EvexFlags3["Op1Shift"].Value,
				(int)EncoderTypes.EvexFlags3["Op2Shift"].Value,
				(int)EncoderTypes.EvexFlags3["Op3Shift"].Value,
			};

			var legacyMandatoryPrefixShift = (int)EncoderTypes.LegacyFlags["MandatoryPrefixByteShift"].Value;
			var legacyOpCodeTableShift = (int)EncoderTypes.LegacyFlags["LegacyOpCodeTableShift"].Value;
			var legacyEncodableShift = (int)EncoderTypes.LegacyFlags["EncodableShift"].Value;
			var legacyHasGroupIndex = EncoderTypes.LegacyFlags["HasGroupIndex"].Value;
			var legacyGroupShift = (int)EncoderTypes.LegacyFlags["GroupShift"].Value;
			var legacyAllowedPrefixesShift = (int)EncoderTypes.LegacyFlags["AllowedPrefixesShift"].Value;
			var legacyFwait = EncoderTypes.LegacyFlags["Fwait"].Value;
			var legacyHasMandatoryPrefix = EncoderTypes.LegacyFlags["HasMandatoryPrefix"].Value;
			var legacyOperandSizeShift = (int)EncoderTypes.LegacyFlags["OperandSizeShift"].Value;
			var legacyAddressSizeShift = (int)EncoderTypes.LegacyFlags["AddressSizeShift"].Value;

			var vexMandatoryPrefixShift = (int)EncoderTypes.VexFlags["MandatoryPrefixByteShift"].Value;
			var vexOpCodeTableShift = (int)EncoderTypes.VexFlags["VexOpCodeTableShift"].Value;
			var vexEncodableShift = (int)EncoderTypes.VexFlags["EncodableShift"].Value;
			var vexHasGroupIndex = EncoderTypes.VexFlags["HasGroupIndex"].Value;
			var vexGroupShift = (int)EncoderTypes.VexFlags["GroupShift"].Value;
			var vexVectorLengthShift = (int)EncoderTypes.VexFlags["VexVectorLengthShift"].Value;
			var vexWBitShift = (int)EncoderTypes.VexFlags["WBitShift"].Value;

			var xopMandatoryPrefixShift = (int)EncoderTypes.XopFlags["MandatoryPrefixByteShift"].Value;
			var xopOpCodeTableShift = (int)EncoderTypes.XopFlags["XopOpCodeTableShift"].Value;
			var xopEncodableShift = (int)EncoderTypes.XopFlags["EncodableShift"].Value;
			var xopHasGroupIndex = EncoderTypes.XopFlags["HasGroupIndex"].Value;
			var xopGroupShift = (int)EncoderTypes.XopFlags["GroupShift"].Value;
			var xopVectorLengthShift = (int)EncoderTypes.XopFlags["XopVectorLengthShift"].Value;
			var xopWBitShift = (int)EncoderTypes.XopFlags["WBitShift"].Value;

			var evexMandatoryPrefixShift = (int)EncoderTypes.EvexFlags["MandatoryPrefixByteShift"].Value;
			var evexOpCodeTableShift = (int)EncoderTypes.EvexFlags["EvexOpCodeTableShift"].Value;
			var evexEncodableShift = (int)EncoderTypes.EvexFlags["EncodableShift"].Value;
			var evexHasGroupIndex = EncoderTypes.EvexFlags["HasGroupIndex"].Value;
			var evexGroupShift = (int)EncoderTypes.EvexFlags["GroupShift"].Value;
			var evexVectorLengthShift = (int)EncoderTypes.EvexFlags["EvexVectorLengthShift"].Value;
			var evexWBitShift = (int)EncoderTypes.EvexFlags["WBitShift"].Value;
			var evexTupleTypeShift = (int)EncoderTypes.EvexFlags["TupleTypeShift"].Value;
			var evex_LIG = EncoderTypes.EvexFlags["LIG"].Value;
			var evex_b = EncoderTypes.EvexFlags["b"].Value;
			var evex_er = EncoderTypes.EvexFlags["er"].Value;
			var evex_sae = EncoderTypes.EvexFlags["sae"].Value;
			var evex_k1 = EncoderTypes.EvexFlags["k1"].Value;
			var evex_z = EncoderTypes.EvexFlags["z"].Value;

			var d3nowEncodableShift = (int)EncoderTypes.D3nowFlags["EncodableShift"].Value;

			foreach (var opCode in opCodes) {
				uint dword1, dword2, dword3;

				dword1 = (uint)opCode.Encoding << encodingShift;

				switch (opCode.Encoding) {
				case EncodingKind.Legacy:
					var linfo = (LegacyOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << legacyMandatoryPrefixShift;
					dword2 |= (uint)GetLegacyTable(linfo.Table) << legacyOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << legacyEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= legacyHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << legacyGroupShift;
					}
					dword2 |= (uint)GetAllowedPrefixes(opCode) << legacyAllowedPrefixesShift;
					if ((opCode.Flags & OpCodeFlags.Fwait) != 0)
						dword2 |= legacyFwait;
					if (opCode.MandatoryPrefix != MandatoryPrefix.None)
						dword2 |= legacyHasMandatoryPrefix;
					dword2 |= (uint)linfo.OperandSize << legacyOperandSizeShift;
					dword2 |= (uint)linfo.AddressSize << legacyAddressSizeShift;

					dword3 = 0;
					for (int i = 0; i < linfo.OpKinds.Length; i++)
						dword3 |= (uint)linfo.OpKinds[i] << legacyOpShifts[i];
					break;

				case EncodingKind.VEX:
					var vinfo = (VexOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << vexMandatoryPrefixShift;
					dword2 |= (uint)GetVexTable(vinfo.Table) << vexOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << vexEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= vexHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << vexGroupShift;
					}
					dword2 |= (uint)vinfo.VectorLength << vexVectorLengthShift;
					dword2 |= (uint)GetWBit(opCode) << vexWBitShift;

					dword3 = 0;
					for (int i = 0; i < vinfo.OpKinds.Length; i++)
						dword3 |= (uint)vinfo.OpKinds[i] << vexOpShifts[i];
					break;

				case EncodingKind.EVEX:
					var einfo = (EvexOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << evexMandatoryPrefixShift;
					dword2 |= (uint)GetEvexTable(einfo.Table) << evexOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << evexEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= evexHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << evexGroupShift;
					}
					dword2 |= (uint)einfo.VectorLength << evexVectorLengthShift;
					dword2 |= (uint)GetWBit(opCode) << evexWBitShift;
					dword2 |= (uint)einfo.TupleType << evexTupleTypeShift;
					if ((opCode.Flags & OpCodeFlags.LIG) != 0)
						dword2 |= evex_LIG;
					if ((opCode.Flags & OpCodeFlags.Broadcast) != 0)
						dword2 |= evex_b;
					if ((opCode.Flags & OpCodeFlags.RoundingControl) != 0)
						dword2 |= evex_er;
					if ((opCode.Flags & OpCodeFlags.SuppressAllExceptions) != 0)
						dword2 |= evex_sae;
					if ((opCode.Flags & OpCodeFlags.OpMaskRegister) != 0)
						dword2 |= evex_k1;
					if ((opCode.Flags & OpCodeFlags.ZeroingMasking) != 0)
						dword2 |= evex_z;

					dword3 = 0;
					for (int i = 0; i < einfo.OpKinds.Length; i++)
						dword3 |= (uint)einfo.OpKinds[i] << evexOpShifts[i];
					break;

				case EncodingKind.XOP:
					var xinfo = (XopOpCodeInfo)opCode;

					dword1 |= opCode.OpCode << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetMandatoryPrefixByte(opCode.MandatoryPrefix) << xopMandatoryPrefixShift;
					dword2 |= (uint)GetXopTable(xinfo.Table) << xopOpCodeTableShift;
					dword2 |= (uint)GetEncodable(opCode) << xopEncodableShift;
					if (opCode.GroupIndex >= 0) {
						dword2 |= xopHasGroupIndex;
						dword2 |= (uint)opCode.GroupIndex << xopGroupShift;
					}
					dword2 |= (uint)xinfo.VectorLength << xopVectorLengthShift;
					dword2 |= (uint)GetWBit(opCode) << xopWBitShift;

					dword3 = 0;
					for (int i = 0; i < xinfo.OpKinds.Length; i++)
						dword3 |= (uint)xinfo.OpKinds[i] << xopOpShifts[i];
					break;

				case EncodingKind.D3NOW:
					var dinfo = (D3nowOpCodeInfo)opCode;

					dword1 |= dinfo.Immediate8 << opCodeShift;

					dword2 = 0;
					dword2 |= (uint)GetEncodable(opCode) << d3nowEncodableShift;

					dword3 = 0;
					break;

				default:
					throw new InvalidOperationException();
				}

				yield return (opCode, dword1, dword2, dword3);
			}
		}

		static MandatoryPrefixByte GetMandatoryPrefixByte(MandatoryPrefix mandatoryPrefix) =>
			mandatoryPrefix switch {
				MandatoryPrefix.None => MandatoryPrefixByte.None,
				MandatoryPrefix.PNP => MandatoryPrefixByte.None,
				MandatoryPrefix.P66 => MandatoryPrefixByte.P66,
				MandatoryPrefix.PF3 => MandatoryPrefixByte.PF3,
				MandatoryPrefix.PF2 => MandatoryPrefixByte.PF2,
				_ => throw new InvalidOperationException(),
			};

		static Encodable GetEncodable(OpCodeInfo opCode) =>
			(opCode.Flags & (OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64)) switch {
				OpCodeFlags.Mode16 | OpCodeFlags.Mode32 | OpCodeFlags.Mode64 => Encodable.Any,
				OpCodeFlags.Mode16 | OpCodeFlags.Mode32 => Encodable.Only1632,
				OpCodeFlags.Mode64 => Encodable.Only64,
				_ => throw new InvalidOperationException(),
			};

		static LegacyOpCodeTable GetLegacyTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.Normal => LegacyOpCodeTable.Normal,
				OpCodeTableKind.T0F => LegacyOpCodeTable.Table0F,
				OpCodeTableKind.T0F38 => LegacyOpCodeTable.Table0F38,
				OpCodeTableKind.T0F3A => LegacyOpCodeTable.Table0F3A,
				_ => throw new InvalidOperationException(),
			};

		static VexOpCodeTable GetVexTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.T0F => VexOpCodeTable.Table0F,
				OpCodeTableKind.T0F38 => VexOpCodeTable.Table0F38,
				OpCodeTableKind.T0F3A => VexOpCodeTable.Table0F3A,
				_ => throw new InvalidOperationException(),
			};

		static EvexOpCodeTable GetEvexTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.T0F => EvexOpCodeTable.Table0F,
				OpCodeTableKind.T0F38 => EvexOpCodeTable.Table0F38,
				OpCodeTableKind.T0F3A => EvexOpCodeTable.Table0F3A,
				_ => throw new InvalidOperationException(),
			};

		static XopOpCodeTable GetXopTable(OpCodeTableKind table) =>
			table switch {
				OpCodeTableKind.XOP8 => XopOpCodeTable.XOP8,
				OpCodeTableKind.XOP9 => XopOpCodeTable.XOP9,
				OpCodeTableKind.XOPA => XopOpCodeTable.XOPA,
				_ => throw new InvalidOperationException(),
			};

		static uint GetAllowedPrefixes(OpCodeInfo opCode) {
			var flags = opCode.Flags & EncoderTypesGen.PrefixesMask;
			return EncoderTypes.AllowedPrefixesMap[flags].Value;
		}

		static WBit GetWBit(OpCodeInfo opCode) {
			if ((opCode.Flags & OpCodeFlags.WIG32) != 0)
				return WBit.WIG32;
			if ((opCode.Flags & OpCodeFlags.WIG) != 0)
				return WBit.WIG;
			if ((opCode.Flags & OpCodeFlags.W) != 0)
				return WBit.W1;
			return WBit.W0;
		}

		protected static void WriteFlags(FileWriter writer, IdentifierConverter idConverter, OpCodeFlags prefixes, (EnumValue value, OpCodeFlags flag)[] flagsInfos, string orSep, string enumItemSep, bool forceConstant) {
			bool printed = false;
			foreach (var info in flagsInfos) {
				if ((prefixes & info.flag) != 0) {
					prefixes &= ~info.flag;
					if (printed)
						writer.Write(orSep);
					printed = true;
					WriteEnum(writer, idConverter, info.value, enumItemSep, forceConstant);
				}
			}
			if (!printed) {
				var value = OpCodeFlagsEnum.Instance[nameof(OpCodeFlags.None)];
				WriteEnum(writer, idConverter, value, enumItemSep, forceConstant);
			}
			if (prefixes != 0)
				throw new InvalidOperationException();

			static void WriteEnum(FileWriter writer, IdentifierConverter idConverter, EnumValue value, string enumItemSep, bool forceConstant) {
				var name = forceConstant ? idConverter.Constant(value.RawName) : value.Name(idConverter);
				writer.Write($"{value.DeclaringType.Name(idConverter)}{enumItemSep}{name}");
			}
		}
	}
}
