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
				(opCodeFlags["LockPrefix"], OpCodeFlags.LockPrefix),
				(opCodeFlags["XacquirePrefix"], OpCodeFlags.XacquirePrefix),
				(opCodeFlags["XreleasePrefix"], OpCodeFlags.XreleasePrefix),
				(opCodeFlags["RepPrefix"], OpCodeFlags.RepPrefix),
				(opCodeFlags["RepnePrefix"], OpCodeFlags.RepnePrefix),
				(opCodeFlags["BndPrefix"], OpCodeFlags.BndPrefix),
				(opCodeFlags["HintTakenPrefix"], OpCodeFlags.HintTakenPrefix),
				(opCodeFlags["NotrackPrefix"], OpCodeFlags.NotrackPrefix),
			};
			Generate(EncoderTypes.AllowedPrefixesMap.Select(a => (a.Value, a.Key)).OrderBy(a => a.Value.Value).ToArray(), flagsInfos);
			var code = CodeEnum.Instance;
			var notInstrStrings = new (EnumValue code, string result)[] {
				(code["INVALID"], "<invalid>"),
				(code["DeclareByte"], "<db>"),
				(code["DeclareWord"], "<dw>"),
				(code["DeclareDword"], "<dd>"),
				(code["DeclareQword"], "<dq>"),
			};
			var opMaskIsK1 = new EnumValue[] {
				code["EVEX_Vfpclassps_k_k1_xmmm128b32_imm8"],
				code["EVEX_Vfpclassps_k_k1_ymmm256b32_imm8"],
				code["EVEX_Vfpclassps_k_k1_zmmm512b32_imm8"],
				code["EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8"],
				code["EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8"],
				code["EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8"],
				code["EVEX_Vfpclassss_k_k1_xmmm32_imm8"],
				code["EVEX_Vfpclasssd_k_k1_xmmm64_imm8"],
				code["EVEX_Vptestmb_k_k1_xmm_xmmm128"],
				code["EVEX_Vptestmb_k_k1_ymm_ymmm256"],
				code["EVEX_Vptestmb_k_k1_zmm_zmmm512"],
				code["EVEX_Vptestmw_k_k1_xmm_xmmm128"],
				code["EVEX_Vptestmw_k_k1_ymm_ymmm256"],
				code["EVEX_Vptestmw_k_k1_zmm_zmmm512"],
				code["EVEX_Vptestnmb_k_k1_xmm_xmmm128"],
				code["EVEX_Vptestnmb_k_k1_ymm_ymmm256"],
				code["EVEX_Vptestnmb_k_k1_zmm_zmmm512"],
				code["EVEX_Vptestnmw_k_k1_xmm_xmmm128"],
				code["EVEX_Vptestnmw_k_k1_ymm_ymmm256"],
				code["EVEX_Vptestnmw_k_k1_zmm_zmmm512"],
				code["EVEX_Vptestmd_k_k1_xmm_xmmm128b32"],
				code["EVEX_Vptestmd_k_k1_ymm_ymmm256b32"],
				code["EVEX_Vptestmd_k_k1_zmm_zmmm512b32"],
				code["EVEX_Vptestmq_k_k1_xmm_xmmm128b64"],
				code["EVEX_Vptestmq_k_k1_ymm_ymmm256b64"],
				code["EVEX_Vptestmq_k_k1_zmm_zmmm512b64"],
				code["EVEX_Vptestnmd_k_k1_xmm_xmmm128b32"],
				code["EVEX_Vptestnmd_k_k1_ymm_ymmm256b32"],
				code["EVEX_Vptestnmd_k_k1_zmm_zmmm512b32"],
				code["EVEX_Vptestnmq_k_k1_xmm_xmmm128b64"],
				code["EVEX_Vptestnmq_k_k1_ymm_ymmm256b64"],
				code["EVEX_Vptestnmq_k_k1_zmm_zmmm512b64"],
			};
			var incVecIndex = new EnumValue[] {
				code["VEX_Vpextrw_r32m16_xmm_imm8"],
				code["VEX_Vpextrw_r64m16_xmm_imm8"],
				code["EVEX_Vpextrw_r32m16_xmm_imm8"],
				code["EVEX_Vpextrw_r64m16_xmm_imm8"],
				code["VEX_Vmovmskpd_r32_xmm"],
				code["VEX_Vmovmskpd_r64_xmm"],
				code["VEX_Vmovmskpd_r32_ymm"],
				code["VEX_Vmovmskpd_r64_ymm"],
				code["VEX_Vmovmskps_r32_xmm"],
				code["VEX_Vmovmskps_r64_xmm"],
				code["VEX_Vmovmskps_r32_ymm"],
				code["VEX_Vmovmskps_r64_ymm"],
				code["Pextrb_r32m8_xmm_imm8"],
				code["Pextrb_r64m8_xmm_imm8"],
				code["Pextrd_rm32_xmm_imm8"],
				code["Pextrq_rm64_xmm_imm8"],
				code["VEX_Vpextrb_r32m8_xmm_imm8"],
				code["VEX_Vpextrb_r64m8_xmm_imm8"],
				code["VEX_Vpextrd_rm32_xmm_imm8"],
				code["VEX_Vpextrq_rm64_xmm_imm8"],
				code["EVEX_Vpextrb_r32m8_xmm_imm8"],
				code["EVEX_Vpextrb_r64m8_xmm_imm8"],
				code["EVEX_Vpextrd_rm32_xmm_imm8"],
				code["EVEX_Vpextrq_rm64_xmm_imm8"],
			};
			var noVecIndex = new EnumValue[] {
				code["Pxor_mm_mmm64"],
				code["Punpckldq_mm_mmm32"],
				code["Punpcklwd_mm_mmm32"],
				code["Punpcklbw_mm_mmm32"],
				code["Punpckhdq_mm_mmm64"],
				code["Punpckhwd_mm_mmm64"],
				code["Punpckhbw_mm_mmm64"],
				code["Psubusb_mm_mmm64"],
				code["Psubusw_mm_mmm64"],
				code["Psubsw_mm_mmm64"],
				code["Psubsb_mm_mmm64"],
				code["Psubd_mm_mmm64"],
				code["Psubw_mm_mmm64"],
				code["Psubb_mm_mmm64"],
				code["Psrlq_mm_imm8"],
				code["Psrlq_mm_mmm64"],
				code["Psrld_mm_imm8"],
				code["Psrld_mm_mmm64"],
				code["Psrlw_mm_imm8"],
				code["Psrlw_mm_mmm64"],
				code["Psrad_mm_imm8"],
				code["Psrad_mm_mmm64"],
				code["Psraw_mm_imm8"],
				code["Psraw_mm_mmm64"],
				code["Psllq_mm_imm8"],
				code["Psllq_mm_mmm64"],
				code["Pslld_mm_imm8"],
				code["Pslld_mm_mmm64"],
				code["Psllw_mm_mmm64"],
				code["Por_mm_mmm64"],
				code["Pmullw_mm_mmm64"],
				code["Pmulhw_mm_mmm64"],
				code["Pmovmskb_r32_mm"],
				code["Pmovmskb_r64_mm"],
				code["Pmovmskb_r32_xmm"],
				code["Pmovmskb_r64_xmm"],
				code["Pmaddwd_mm_mmm64"],
				code["Pinsrw_mm_r32m16_imm8"],
				code["Pinsrw_mm_r64m16_imm8"],
				code["Pinsrw_xmm_r32m16_imm8"],
				code["Pinsrw_xmm_r64m16_imm8"],
				code["Pextrw_r32_xmm_imm8"],
				code["Pextrw_r64_xmm_imm8"],
				code["Pextrw_r32m16_xmm_imm8"],
				code["Pextrw_r64m16_xmm_imm8"],
				code["Pextrw_r32_mm_imm8"],
				code["Pextrw_r64_mm_imm8"],
				code["Cvtpd2pi_mm_xmmm128"],
				code["Cvtpi2pd_xmm_mmm64"],
				code["Cvtpi2ps_xmm_mmm64"],
				code["Cvtps2pi_mm_xmmm64"],
				code["Cvttpd2pi_mm_xmmm128"],
				code["Cvttps2pi_mm_xmmm64"],
				code["Movd_mm_rm32"],
				code["Movq_mm_rm64"],
				code["Movd_rm32_mm"],
				code["Movq_rm64_mm"],
				code["Movd_xmm_rm32"],
				code["Movq_xmm_rm64"],
				code["Movd_rm32_xmm"],
				code["Movq_rm64_xmm"],
				code["Movdq2q_mm_xmm"],
				code["Movmskpd_r32_xmm"],
				code["Movmskpd_r64_xmm"],
				code["Movmskps_r32_xmm"],
				code["Movmskps_r64_xmm"],
				code["Movntq_m64_mm"],
				code["Movq_mm_mmm64"],
				code["Movq_mmm64_mm"],
				code["Movq2dq_xmm_mm"],
				code["Packuswb_mm_mmm64"],
				code["Paddb_mm_mmm64"],
				code["Paddw_mm_mmm64"],
				code["Paddd_mm_mmm64"],
				code["Paddq_mm_mmm64"],
				code["Paddsb_mm_mmm64"],
				code["Paddsw_mm_mmm64"],
				code["Paddusb_mm_mmm64"],
				code["Paddusw_mm_mmm64"],
				code["Pand_mm_mmm64"],
				code["Pandn_mm_mmm64"],
				code["Pcmpeqb_mm_mmm64"],
				code["Pcmpeqw_mm_mmm64"],
				code["Pcmpeqd_mm_mmm64"],
				code["Pcmpgtb_mm_mmm64"],
				code["Pcmpgtw_mm_mmm64"],
				code["Pcmpgtd_mm_mmm64"],
			};
			var swapVecIndex12 = new EnumValue[] {
				code["Movapd_xmmm128_xmm"],
				code["VEX_Vmovapd_xmmm128_xmm"],
				code["VEX_Vmovapd_ymmm256_ymm"],
				code["EVEX_Vmovapd_xmmm128_k1z_xmm"],
				code["EVEX_Vmovapd_ymmm256_k1z_ymm"],
				code["EVEX_Vmovapd_zmmm512_k1z_zmm"],
				code["Movaps_xmmm128_xmm"],
				code["VEX_Vmovaps_xmmm128_xmm"],
				code["VEX_Vmovaps_ymmm256_ymm"],
				code["EVEX_Vmovaps_xmmm128_k1z_xmm"],
				code["EVEX_Vmovaps_ymmm256_k1z_ymm"],
				code["EVEX_Vmovaps_zmmm512_k1z_zmm"],
				code["Movdqa_xmmm128_xmm"],
				code["VEX_Vmovdqa_xmmm128_xmm"],
				code["VEX_Vmovdqa_ymmm256_ymm"],
				code["EVEX_Vmovdqa32_xmmm128_k1z_xmm"],
				code["EVEX_Vmovdqa32_ymmm256_k1z_ymm"],
				code["EVEX_Vmovdqa32_zmmm512_k1z_zmm"],
				code["EVEX_Vmovdqa64_xmmm128_k1z_xmm"],
				code["EVEX_Vmovdqa64_ymmm256_k1z_ymm"],
				code["EVEX_Vmovdqa64_zmmm512_k1z_zmm"],
				code["Movdqu_xmmm128_xmm"],
				code["VEX_Vmovdqu_xmmm128_xmm"],
				code["VEX_Vmovdqu_ymmm256_ymm"],
				code["EVEX_Vmovdqu8_xmmm128_k1z_xmm"],
				code["EVEX_Vmovdqu8_ymmm256_k1z_ymm"],
				code["EVEX_Vmovdqu8_zmmm512_k1z_zmm"],
				code["EVEX_Vmovdqu16_xmmm128_k1z_xmm"],
				code["EVEX_Vmovdqu16_ymmm256_k1z_ymm"],
				code["EVEX_Vmovdqu16_zmmm512_k1z_zmm"],
				code["EVEX_Vmovdqu32_xmmm128_k1z_xmm"],
				code["EVEX_Vmovdqu32_ymmm256_k1z_ymm"],
				code["EVEX_Vmovdqu32_zmmm512_k1z_zmm"],
				code["EVEX_Vmovdqu64_xmmm128_k1z_xmm"],
				code["EVEX_Vmovdqu64_ymmm256_k1z_ymm"],
				code["EVEX_Vmovdqu64_zmmm512_k1z_zmm"],
				code["VEX_Vmovhpd_xmm_xmm_m64"],
				code["EVEX_Vmovhpd_xmm_xmm_m64"],
				code["VEX_Vmovhps_xmm_xmm_m64"],
				code["EVEX_Vmovhps_xmm_xmm_m64"],
				code["VEX_Vmovlpd_xmm_xmm_m64"],
				code["EVEX_Vmovlpd_xmm_xmm_m64"],
				code["VEX_Vmovlps_xmm_xmm_m64"],
				code["EVEX_Vmovlps_xmm_xmm_m64"],
				code["Movq_xmmm64_xmm"],
				code["Movss_xmmm32_xmm"],
				code["Movupd_xmmm128_xmm"],
				code["VEX_Vmovupd_xmmm128_xmm"],
				code["VEX_Vmovupd_ymmm256_ymm"],
				code["EVEX_Vmovupd_xmmm128_k1z_xmm"],
				code["EVEX_Vmovupd_ymmm256_k1z_ymm"],
				code["EVEX_Vmovupd_zmmm512_k1z_zmm"],
				code["Movups_xmmm128_xmm"],
				code["VEX_Vmovups_xmmm128_xmm"],
				code["VEX_Vmovups_ymmm256_ymm"],
				code["EVEX_Vmovups_xmmm128_k1z_xmm"],
				code["EVEX_Vmovups_ymmm256_k1z_ymm"],
				code["EVEX_Vmovups_zmmm512_k1z_zmm"],
			};
			var fpuStartOpIndex1 = new EnumValue[] {
				code["Fcom_st0_sti"],
				code["Fcom_st0_sti_DCD0"],
				code["Fcomp_st0_sti"],
				code["Fcomp_st0_sti_DCD8"],
				code["Fcomp_st0_sti_DED0"],
				code["Fld_st0_sti"],
				code["Fucom_st0_sti"],
				code["Fucomp_st0_sti"],
				code["Fxch_st0_sti"],
				code["Fxch_st0_sti_DDC8"],
				code["Fxch_st0_sti_DFC8"],
			};
			GenerateInstructionFormatter(notInstrStrings, opMaskIsK1, incVecIndex, noVecIndex, swapVecIndex12, fpuStartOpIndex1);
			var opCodeOperandKind = OpCodeOperandKindEnum.Instance;
			var hasModRM = new EnumValue[] {
				opCodeOperandKind["mem"],
				opCodeOperandKind["mem_mpx"],
				opCodeOperandKind["mem_mib"],
				opCodeOperandKind["mem_vsib32x"],
				opCodeOperandKind["mem_vsib64x"],
				opCodeOperandKind["mem_vsib32y"],
				opCodeOperandKind["mem_vsib64y"],
				opCodeOperandKind["mem_vsib32z"],
				opCodeOperandKind["mem_vsib64z"],
				opCodeOperandKind["r8_or_mem"],
				opCodeOperandKind["r16_or_mem"],
				opCodeOperandKind["r32_or_mem"],
				opCodeOperandKind["r32_or_mem_mpx"],
				opCodeOperandKind["r64_or_mem"],
				opCodeOperandKind["r64_or_mem_mpx"],
				opCodeOperandKind["mm_or_mem"],
				opCodeOperandKind["xmm_or_mem"],
				opCodeOperandKind["ymm_or_mem"],
				opCodeOperandKind["zmm_or_mem"],
				opCodeOperandKind["bnd_or_mem_mpx"],
				opCodeOperandKind["k_or_mem"],
				opCodeOperandKind["r8_reg"],
				opCodeOperandKind["r16_reg"],
				opCodeOperandKind["r16_rm"],
				opCodeOperandKind["r32_reg"],
				opCodeOperandKind["r32_rm"],
				opCodeOperandKind["r64_reg"],
				opCodeOperandKind["r64_rm"],
				opCodeOperandKind["seg_reg"],
				opCodeOperandKind["k_reg"],
				opCodeOperandKind["kp1_reg"],
				opCodeOperandKind["k_rm"],
				opCodeOperandKind["mm_reg"],
				opCodeOperandKind["mm_rm"],
				opCodeOperandKind["xmm_reg"],
				opCodeOperandKind["xmm_rm"],
				opCodeOperandKind["ymm_reg"],
				opCodeOperandKind["ymm_rm"],
				opCodeOperandKind["zmm_reg"],
				opCodeOperandKind["zmm_rm"],
				opCodeOperandKind["cr_reg"],
				opCodeOperandKind["dr_reg"],
				opCodeOperandKind["tr_reg"],
				opCodeOperandKind["bnd_reg"],
			};
			var hasVsib = new EnumValue[] {
				opCodeOperandKind["mem_vsib32x"],
				opCodeOperandKind["mem_vsib64x"],
				opCodeOperandKind["mem_vsib32y"],
				opCodeOperandKind["mem_vsib64y"],
				opCodeOperandKind["mem_vsib32z"],
				opCodeOperandKind["mem_vsib64z"],
			};
			GenerateOpCodeFormatter(notInstrStrings, hasModRM, hasVsib);
			GenerateCore();
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
				var value = OpCodeFlagsEnum.Instance["None"];
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
