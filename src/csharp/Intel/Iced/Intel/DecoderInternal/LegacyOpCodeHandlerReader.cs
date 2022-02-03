// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
using System;

namespace Iced.Intel.DecoderInternal {
	sealed class LegacyOpCodeHandlerReader : OpCodeHandlerReader {
		public override int ReadHandlers(ref TableDeserializer deserializer, OpCodeHandler?[] result, int resultIndex) {
			ref var elem = ref result[resultIndex];
			Code code;
			switch (deserializer.ReadLegacyOpCodeHandlerKind()) {
			case LegacyOpCodeHandlerKind.Bitness:
				elem = new OpCodeHandler_Bitness(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.Bitness_DontReadModRM:
				elem = new OpCodeHandler_Bitness_DontReadModRM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.Invalid:
				elem = OpCodeHandler_Invalid.Instance;
				return 1;

			case LegacyOpCodeHandlerKind.Invalid_NoModRM:
				elem = OpCodeHandler_Invalid_NoModRM.Instance;
				return 1;

			case LegacyOpCodeHandlerKind.Invalid2:
				result[resultIndex] = OpCodeHandler_Invalid.Instance;
				result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
				return 2;

			case LegacyOpCodeHandlerKind.Dup:
				int count = deserializer.ReadInt32();
				var handler = deserializer.ReadHandlerOrNull();
				for (int i = 0; i < count; i++)
					result[resultIndex + i] = handler;
				return count;

			case LegacyOpCodeHandlerKind.Null:
				elem = null;
				return 1;

			case LegacyOpCodeHandlerKind.HandlerReference:
				elem = deserializer.ReadHandlerReference();
				return 1;

			case LegacyOpCodeHandlerKind.ArrayReference:
				throw new InvalidOperationException();

			case LegacyOpCodeHandlerKind.RM:
				elem = new OpCodeHandler_RM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.Options1632_1:
				elem = new OpCodeHandler_Options1632(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case LegacyOpCodeHandlerKind.Options1632_2:
				elem = new OpCodeHandler_Options1632(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case LegacyOpCodeHandlerKind.Options3:
				elem = new OpCodeHandler_Options(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case LegacyOpCodeHandlerKind.Options5:
				elem = new OpCodeHandler_Options(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case LegacyOpCodeHandlerKind.Options_DontReadModRM:
				elem = new OpCodeHandler_Options_DontReadModRM(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case LegacyOpCodeHandlerKind.AnotherTable:
				elem = new OpCodeHandler_AnotherTable(deserializer.ReadArrayReference((uint)LegacyOpCodeHandlerKind.ArrayReference));
				return 1;

			case LegacyOpCodeHandlerKind.Group:
				elem = new OpCodeHandler_Group(deserializer.ReadArrayReference((uint)LegacyOpCodeHandlerKind.ArrayReference));
				return 1;

			case LegacyOpCodeHandlerKind.Group8x64:
				elem = new OpCodeHandler_Group8x64(deserializer.ReadArrayReference((uint)LegacyOpCodeHandlerKind.ArrayReference), deserializer.ReadArrayReference((uint)LegacyOpCodeHandlerKind.ArrayReference));
				return 1;

			case LegacyOpCodeHandlerKind.Group8x8:
				elem = new OpCodeHandler_Group8x8(deserializer.ReadArrayReference((uint)LegacyOpCodeHandlerKind.ArrayReference), deserializer.ReadArrayReference((uint)LegacyOpCodeHandlerKind.ArrayReference));
				return 1;

			case LegacyOpCodeHandlerKind.MandatoryPrefix:
				elem = new OpCodeHandler_MandatoryPrefix(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.MandatoryPrefix4:
				elem = new OpCodeHandler_MandatoryPrefix4(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), (uint)deserializer.ReadInt32());
				return 1;

			case LegacyOpCodeHandlerKind.MandatoryPrefix_NoModRM:
				elem = new OpCodeHandler_MandatoryPrefix_NoModRM(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.MandatoryPrefix3:
				elem = new OpCodeHandler_MandatoryPrefix3(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadLegacyHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.D3NOW:
				elem = new OpCodeHandler_D3NOW();
				return 1;

			case LegacyOpCodeHandlerKind.EVEX:
				elem = new OpCodeHandler_EVEX(deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.VEX2:
				elem = new OpCodeHandler_VEX2(deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.VEX3:
				elem = new OpCodeHandler_VEX3(deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.XOP:
				elem = new OpCodeHandler_XOP(deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.AL_DX:
				elem = new OpCodeHandler_AL_DX(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Ap:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ap(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.B_BM:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_B_BM(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.B_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_B_Ev(code, code + 1, deserializer.ReadBoolean());
				return 1;

			case LegacyOpCodeHandlerKind.B_MIB:
				elem = new OpCodeHandler_B_MIB(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.BM_B:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_BM_B(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.BranchIw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_BranchIw(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.BranchSimple:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_BranchSimple(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.C_R_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_C_R(code, code + 1, deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.C_R_3b:
				elem = new OpCodeHandler_C_R(deserializer.ReadCode(), Code.INVALID, deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.DX_AL:
				elem = new OpCodeHandler_DX_AL(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.DX_eAX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_DX_eAX(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.eAX_DX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_eAX_DX(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Eb_1:
				elem = new OpCodeHandler_Eb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Eb_2:
				elem = new OpCodeHandler_Eb(deserializer.ReadCode(), deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Eb_CL:
				elem = new OpCodeHandler_Eb_CL(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Eb_Gb_1:
				elem = new OpCodeHandler_Eb_Gb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Eb_Gb_2:
				elem = new OpCodeHandler_Eb_Gb(deserializer.ReadCode(), deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Eb_Ib_1:
				elem = new OpCodeHandler_Eb_Ib(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Eb_Ib_2:
				elem = new OpCodeHandler_Eb_Ib(deserializer.ReadCode(), deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Eb1:
				elem = new OpCodeHandler_Eb_1(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Ed_V_Ib:
				elem = new OpCodeHandler_Ed_V_Ib(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Ep:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ep(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_3b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev(code, code + 1, Code.INVALID);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Ev_CL:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_CL(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Gv_32_64:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_32_64(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Gv_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Gv_3b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv(code, code + 1, Code.INVALID);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Gv_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Gv_CL:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_CL(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Gv_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_Ib(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Gv_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_REX(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Ib_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Ib_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Ib2_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib2(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Ib2_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib2(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Iz_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Iz(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Iz_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Iz(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.Ev_P:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_P(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_REXW_1a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_REXW(code, Code.INVALID, (uint)deserializer.ReadInt32());
				return 1;

			case LegacyOpCodeHandlerKind.Ev_REXW:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_REXW(code, code + 1, (uint)deserializer.ReadInt32());
				return 1;

			case LegacyOpCodeHandlerKind.Ev_Sw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Sw(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ev_VX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_VX(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Ev1:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_1(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Evj:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Evj(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Evw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Evw(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Ew:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ew(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gb_Eb:
				elem = new OpCodeHandler_Gb_Eb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Gdq_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gdq_Ev(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Eb:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Eb(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Eb_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Eb_REX(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev_32_64:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_32_64(code, code + 1, deserializer.ReadBoolean(), deserializer.ReadBoolean());
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev_3b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev(code, code + 1, Code.INVALID);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_Ib(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev_Ib_REX:
				elem = new OpCodeHandler_Gv_Ev_Ib_REX(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev_Iz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_Iz(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_REX(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev2(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ev3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev3(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ew:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ew(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_M:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_M(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_M_as:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_M_as(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Ma:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ma(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Mp_2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Mp(code, code + 1, Code.INVALID);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Mp_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Mp(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_Mv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Mv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_N:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_N(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_N_Ib_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_N_Ib_REX(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_RX:
				elem = new OpCodeHandler_Gv_RX(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Gv_W:
				elem = new OpCodeHandler_Gv_W(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.GvM_VX_Ib:
				elem = new OpCodeHandler_GvM_VX_Ib(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Ib:
				elem = new OpCodeHandler_Ib(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Ib3:
				elem = new OpCodeHandler_Ib3(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.IbReg:
				elem = new OpCodeHandler_IbReg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.IbReg2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_IbReg2(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Iw_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Iw_Ib(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Jb:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jb(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Jb2:
				elem = new OpCodeHandler_Jb2(deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Jdisp:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jdisp(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Jx:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jx(code, code + 1, deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Jz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jz(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.M_1:
				elem = new OpCodeHandler_M(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.M_2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_M(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.M_REXW_2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_M_REXW(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.M_REXW_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_M_REXW(code, code + 1, deserializer.ReadHandlerFlags(), deserializer.ReadHandlerFlags());
				return 1;

			case LegacyOpCodeHandlerKind.MemBx:
				elem = new OpCodeHandler_MemBx(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Mf_1:
				elem = new OpCodeHandler_Mf(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Mf_2a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Mf(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Mf_2b:
				elem = new OpCodeHandler_Mf(deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.MIB_B:
				elem = new OpCodeHandler_MIB_B(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.MP:
				elem = new OpCodeHandler_MP(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Ms:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ms(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.MV:
				elem = new OpCodeHandler_MV(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Mv_Gv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Mv_Gv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Mv_Gv_REXW:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Mv_Gv_REXW(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.NIb:
				elem = new OpCodeHandler_NIb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Ob_Reg:
				elem = new OpCodeHandler_Ob_Reg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.Ov_Reg:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ov_Reg(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.P_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_P_Ev(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.P_Ev_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_P_Ev_Ib(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.P_Q:
				elem = new OpCodeHandler_P_Q(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.P_Q_Ib:
				elem = new OpCodeHandler_P_Q_Ib(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.P_R:
				elem = new OpCodeHandler_P_R(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.P_W:
				elem = new OpCodeHandler_P_W(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.PushEv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushEv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.PushIb2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushIb2(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.PushIz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushIz(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.PushOpSizeReg_4a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushOpSizeReg(code, code + 1, code + 2, deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.PushOpSizeReg_4b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushOpSizeReg(code, code + 1, Code.INVALID, deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.PushSimple2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushSimple2(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.PushSimpleReg:
				elem = new OpCodeHandler_PushSimpleReg(deserializer.ReadInt32(), code = deserializer.ReadCode(), code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Q_P:
				elem = new OpCodeHandler_Q_P(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.R_C_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_R_C(code, code + 1, deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.R_C_3b:
				elem = new OpCodeHandler_R_C(deserializer.ReadCode(), Code.INVALID, deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.rDI_P_N:
				elem = new OpCodeHandler_rDI_P_N(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.rDI_VX_RX:
				elem = new OpCodeHandler_rDI_VX_RX(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Reg:
				elem = new OpCodeHandler_Reg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Ib2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Ib2(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Iz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Iz(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Ob:
				elem = new OpCodeHandler_Reg_Ob(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Ov:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Ov(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Xb:
				elem = new OpCodeHandler_Reg_Xb(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Xv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Xv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Xv2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Xv2(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Yb:
				elem = new OpCodeHandler_Reg_Yb(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.Reg_Yv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Yv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.RegIb:
				elem = new OpCodeHandler_RegIb(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.RegIb3:
				elem = new OpCodeHandler_RegIb3(deserializer.ReadInt32());
				return 1;

			case LegacyOpCodeHandlerKind.RegIz2:
				elem = new OpCodeHandler_RegIz2(deserializer.ReadInt32());
				return 1;

			case LegacyOpCodeHandlerKind.Reservednop:
				elem = new OpCodeHandler_Reservednop(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case LegacyOpCodeHandlerKind.RIb:
				elem = new OpCodeHandler_RIb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.RIbIb:
				elem = new OpCodeHandler_RIbIb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Rv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Rv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Rv_32_64:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Rv_32_64(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.RvMw_Gw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_RvMw_Gw(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Simple:
				elem = new OpCodeHandler_Simple(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Simple_ModRM:
				elem = new OpCodeHandler_Simple_ModRM(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Simple2_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple2(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Simple2_3b:
				elem = new OpCodeHandler_Simple2(deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Simple2Iw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple2Iw(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Simple3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple3(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Simple4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple4(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Simple4b:
				code = deserializer.ReadCode();
				var code2 = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple4(code, code2);
				return 1;

			case LegacyOpCodeHandlerKind.Simple5:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple5(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Simple5_a32:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple5_a32(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Simple5_ModRM_as:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple5_ModRM_as(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.SimpleReg:
				elem = new OpCodeHandler_SimpleReg(deserializer.ReadCode(), deserializer.ReadInt32());
				return 1;

			case LegacyOpCodeHandlerKind.ST_STi:
				elem = new OpCodeHandler_ST_STi(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.STi:
				elem = new OpCodeHandler_STi(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.STi_ST:
				elem = new OpCodeHandler_STi_ST(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Sw_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Sw_Ev(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.V_Ev:
				elem = new OpCodeHandler_V_Ev(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.VM:
				elem = new OpCodeHandler_VM(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.VN:
				elem = new OpCodeHandler_VN(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.VQ:
				elem = new OpCodeHandler_VQ(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.VRIbIb:
				elem = new OpCodeHandler_VRIbIb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.VW_2:
				elem = new OpCodeHandler_VW(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.VW_3:
				elem = new OpCodeHandler_VW(deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.VWIb_2:
				elem = new OpCodeHandler_VWIb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.VWIb_3:
				elem = new OpCodeHandler_VWIb(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.VX_E_Ib:
				elem = new OpCodeHandler_VX_E_Ib(code = deserializer.ReadCode(), code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.VX_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VX_Ev(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Wbinvd:
				elem = new OpCodeHandler_Wbinvd();
				return 1;

			case LegacyOpCodeHandlerKind.WV:
				elem = new OpCodeHandler_WV(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Xb_Yb:
				elem = new OpCodeHandler_Xb_Yb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Xchg_Reg_rAX:
				elem = new OpCodeHandler_Xchg_Reg_rAX(deserializer.ReadInt32());
				return 1;

			case LegacyOpCodeHandlerKind.Xv_Yv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Xv_Yv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Yb_Reg:
				elem = new OpCodeHandler_Yb_Reg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.Yb_Xb:
				elem = new OpCodeHandler_Yb_Xb(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Yv_Reg:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Yv_Reg(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.Yv_Reg2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Yv_Reg2(code, code + 1);
				return 1;

			case LegacyOpCodeHandlerKind.Yv_Xv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Yv_Xv(code, code + 1, code + 2);
				return 1;

			case LegacyOpCodeHandlerKind.M_Sw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_M_Sw(code);
				return 1;

			case LegacyOpCodeHandlerKind.Sw_M:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Sw_M(code);
				return 1;

			case LegacyOpCodeHandlerKind.Rq:
				elem = new OpCodeHandler_Rq(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.Gd_Rd:
				elem = new OpCodeHandler_Gd_Rd(deserializer.ReadCode());
				return 1;

			case LegacyOpCodeHandlerKind.PrefixEsCsSsDs:
				elem = new OpCodeHandler_PrefixEsCsSsDs(deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.PrefixFsGs:
				elem = new OpCodeHandler_PrefixFsGs(deserializer.ReadRegister());
				return 1;

			case LegacyOpCodeHandlerKind.Prefix66:
				elem = new OpCodeHandler_Prefix66();
				return 1;

			case LegacyOpCodeHandlerKind.Prefix67:
				elem = new OpCodeHandler_Prefix67();
				return 1;

			case LegacyOpCodeHandlerKind.PrefixF0:
				elem = new OpCodeHandler_PrefixF0();
				return 1;

			case LegacyOpCodeHandlerKind.PrefixF2:
				elem = new OpCodeHandler_PrefixF2();
				return 1;

			case LegacyOpCodeHandlerKind.PrefixF3:
				elem = new OpCodeHandler_PrefixF3();
				return 1;

			case LegacyOpCodeHandlerKind.PrefixREX:
				elem = new OpCodeHandler_PrefixREX(deserializer.ReadHandler(), (uint)deserializer.ReadInt32());
				return 1;

			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
