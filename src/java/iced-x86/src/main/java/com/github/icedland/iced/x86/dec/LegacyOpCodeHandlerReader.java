// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.internal.dec.LegacyOpCodeHandlerKind;

final class LegacyOpCodeHandlerReader extends OpCodeHandlerReader {
	@Override
	int readHandlers(TableDeserializer deserializer, OpCodeHandler[] result, int resultIndex) {
		OpCodeHandler elem;
		int code;
		switch (deserializer.readLegacyOpCodeHandlerKind()) {
		case LegacyOpCodeHandlerKind.BITNESS:
			elem = new OpCodeHandler_Bitness(deserializer.readHandler(), deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.BITNESS_DONT_READ_MOD_RM:
			elem = new OpCodeHandler_Bitness_DontReadModRM(deserializer.readHandler(), deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.INVALID:
			elem = OpCodeHandler_Invalid.Instance;
			break;

		case LegacyOpCodeHandlerKind.INVALID_NO_MOD_RM:
			elem = OpCodeHandler_Invalid_NoModRM.Instance;
			break;

		case LegacyOpCodeHandlerKind.INVALID2:
			result[resultIndex] = OpCodeHandler_Invalid.Instance;
			result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
			return 2;

		case LegacyOpCodeHandlerKind.DUP:
			int count = deserializer.readInt32();
			OpCodeHandler handler = deserializer.readHandlerOrNull();
			for (int i = 0; i < count; i++)
				result[resultIndex + i] = handler;
			return count;

		case LegacyOpCodeHandlerKind.NULL:
			elem = null;
			break;

		case LegacyOpCodeHandlerKind.HANDLER_REFERENCE:
			elem = deserializer.readHandlerReference();
			break;

		case LegacyOpCodeHandlerKind.ARRAY_REFERENCE:
			throw new UnsupportedOperationException();

		case LegacyOpCodeHandlerKind.RM:
			elem = new OpCodeHandler_RM(deserializer.readHandler(), deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.OPTIONS1632_1:
			elem = new OpCodeHandler_Options1632(deserializer.readHandler(), deserializer.readHandler(), deserializer.readDecoderOptions());
			break;

		case LegacyOpCodeHandlerKind.OPTIONS1632_2:
			elem = new OpCodeHandler_Options1632(deserializer.readHandler(), deserializer.readHandler(), deserializer.readDecoderOptions(), deserializer.readHandler(), deserializer.readDecoderOptions());
			break;

		case LegacyOpCodeHandlerKind.OPTIONS3:
			elem = new OpCodeHandler_Options(deserializer.readHandler(), deserializer.readHandler(), deserializer.readDecoderOptions());
			break;

		case LegacyOpCodeHandlerKind.OPTIONS5:
			elem = new OpCodeHandler_Options(deserializer.readHandler(), deserializer.readHandler(), deserializer.readDecoderOptions(), deserializer.readHandler(), deserializer.readDecoderOptions());
			break;

		case LegacyOpCodeHandlerKind.OPTIONS_DONT_READ_MOD_RM:
			elem = new OpCodeHandler_Options_DontReadModRM(deserializer.readHandler(), deserializer.readHandler(), deserializer.readDecoderOptions());
			break;

		case LegacyOpCodeHandlerKind.ANOTHER_TABLE:
			elem = new OpCodeHandler_AnotherTable(deserializer.readArrayReference(LegacyOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case LegacyOpCodeHandlerKind.GROUP:
			elem = new OpCodeHandler_Group(deserializer.readArrayReference(LegacyOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case LegacyOpCodeHandlerKind.GROUP8X64:
			elem = new OpCodeHandler_Group8x64(deserializer.readArrayReference(LegacyOpCodeHandlerKind.ARRAY_REFERENCE), deserializer.readArrayReference(LegacyOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case LegacyOpCodeHandlerKind.GROUP8X8:
			elem = new OpCodeHandler_Group8x8(deserializer.readArrayReference(LegacyOpCodeHandlerKind.ARRAY_REFERENCE), deserializer.readArrayReference(LegacyOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case LegacyOpCodeHandlerKind.MANDATORY_PREFIX:
			elem = new OpCodeHandler_MandatoryPrefix(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.MANDATORY_PREFIX4:
			elem = new OpCodeHandler_MandatoryPrefix4(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readInt32());
			break;

		case LegacyOpCodeHandlerKind.MANDATORY_PREFIX_NO_MOD_RM:
			elem = new OpCodeHandler_MandatoryPrefix_NoModRM(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.MANDATORY_PREFIX3:
			elem = new OpCodeHandler_MandatoryPrefix3(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readLegacyHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.D3NOW:
			elem = new OpCodeHandler_D3NOW();
			break;

		case LegacyOpCodeHandlerKind.EVEX:
			elem = new OpCodeHandler_EVEX(deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.VEX2:
			elem = new OpCodeHandler_VEX2(deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.VEX3:
			elem = new OpCodeHandler_VEX3(deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.XOP:
			elem = new OpCodeHandler_XOP(deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.AL_DX:
			elem = new OpCodeHandler_AL_DX(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.AP:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ap(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.B_BM:
			code = deserializer.readCode();
			elem = new OpCodeHandler_B_BM(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.B_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_B_Ev(code, code + 1, deserializer.readBoolean());
			break;

		case LegacyOpCodeHandlerKind.B_MIB:
			elem = new OpCodeHandler_B_MIB(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.BM_B:
			code = deserializer.readCode();
			elem = new OpCodeHandler_BM_B(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.BRANCH_IW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_BranchIw(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.BRANCH_SIMPLE:
			code = deserializer.readCode();
			elem = new OpCodeHandler_BranchSimple(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.C_R_3A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_C_R(code, code + 1, deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.C_R_3B:
			elem = new OpCodeHandler_C_R(deserializer.readCode(), Code.INVALID, deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.DX_AL:
			elem = new OpCodeHandler_DX_AL(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.DX_E_AX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_DX_eAX(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.E_AX_DX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_eAX_DX(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.EB_1:
			elem = new OpCodeHandler_Eb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.EB_2:
			elem = new OpCodeHandler_Eb(deserializer.readCode(), deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EB_CL:
			elem = new OpCodeHandler_Eb_CL(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.EB_GB_1:
			elem = new OpCodeHandler_Eb_Gb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.EB_GB_2:
			elem = new OpCodeHandler_Eb_Gb(deserializer.readCode(), deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EB_IB_1:
			elem = new OpCodeHandler_Eb_Ib(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.EB_IB_2:
			elem = new OpCodeHandler_Eb_Ib(deserializer.readCode(), deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EB1:
			elem = new OpCodeHandler_Eb_1(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.ED_V_IB:
			elem = new OpCodeHandler_Ed_V_Ib(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.EP:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ep(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_3A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_3B:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev(code, code + 1, Code.INVALID);
			break;

		case LegacyOpCodeHandlerKind.EV_4:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev(code, code + 1, code + 2, deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EV_CL:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_CL(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_GV_32_64:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Gv_32_64(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.EV_GV_3A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Gv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_GV_3B:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Gv(code, code + 1, Code.INVALID);
			break;

		case LegacyOpCodeHandlerKind.EV_GV_4:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Gv(code, code + 1, code + 2, deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EV_GV_CL:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Gv_CL(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_GV_IB:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Gv_Ib(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_GV_REX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Gv_REX(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.EV_IB_3:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Ib(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_IB_4:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Ib(code, code + 1, code + 2, deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EV_IB2_3:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Ib2(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_IB2_4:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Ib2(code, code + 1, code + 2, deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EV_IZ_3:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Iz(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_IZ_4:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Iz(code, code + 1, code + 2, deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.EV_P:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_P(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.EV_REXW_1A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_REXW(code, Code.INVALID, deserializer.readInt32());
			break;

		case LegacyOpCodeHandlerKind.EV_REXW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_REXW(code, code + 1, deserializer.readInt32());
			break;

		case LegacyOpCodeHandlerKind.EV_SW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_Sw(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EV_VX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_VX(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.EV1:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ev_1(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EVJ:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Evj(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EVW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Evw(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.EW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ew(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GB_EB:
			elem = new OpCodeHandler_Gb_Eb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.GDQ_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gdq_Ev(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_EB:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Eb(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_EB_REX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Eb_REX(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_EV_32_64:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev_32_64(code, code + 1, deserializer.readBoolean(), deserializer.readBoolean());
			break;

		case LegacyOpCodeHandlerKind.GV_EV_3A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_EV_3B:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev(code, code + 1, Code.INVALID);
			break;

		case LegacyOpCodeHandlerKind.GV_EV_IB:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev_Ib(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_EV_IB_REX:
			elem = new OpCodeHandler_Gv_Ev_Ib_REX(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_EV_IZ:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev_Iz(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_EV_REX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev_REX(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_EV2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev2(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_EV3:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ev3(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_EW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ew(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_M:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_M(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_M_AS:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_M_as(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_MA:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Ma(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_MP_2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Mp(code, code + 1, Code.INVALID);
			break;

		case LegacyOpCodeHandlerKind.GV_MP_3:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Mp(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_MV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_Mv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.GV_N:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_N(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_N_IB_REX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Gv_N_Ib_REX(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_RX:
			elem = new OpCodeHandler_Gv_RX(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_W:
			elem = new OpCodeHandler_Gv_W(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.GV_M_VX_IB:
			elem = new OpCodeHandler_GvM_VX_Ib(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.IB:
			elem = new OpCodeHandler_Ib(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.IB3:
			elem = new OpCodeHandler_Ib3(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.IB_REG:
			elem = new OpCodeHandler_IbReg(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.IB_REG2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_IbReg2(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.IW_IB:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Iw_Ib(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.JB:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Jb(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.JB2:
			elem = new OpCodeHandler_Jb2(deserializer.readCode(), deserializer.readCode(), deserializer.readCode(), deserializer.readCode(), deserializer.readCode(), deserializer.readCode(), deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.JDISP:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Jdisp(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.JX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Jx(code, code + 1, deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.JZ:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Jz(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.M_1:
			elem = new OpCodeHandler_M(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.M_2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_M(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.M_REXW_2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_M_REXW(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.M_REXW_4:
			code = deserializer.readCode();
			elem = new OpCodeHandler_M_REXW(code, code + 1, deserializer.readHandlerFlags(), deserializer.readHandlerFlags());
			break;

		case LegacyOpCodeHandlerKind.MEM_BX:
			elem = new OpCodeHandler_MemBx(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.MF_1:
			elem = new OpCodeHandler_Mf(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.MF_2A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Mf(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.MF_2B:
			elem = new OpCodeHandler_Mf(deserializer.readCode(), deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.MIB_B:
			elem = new OpCodeHandler_MIB_B(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.MP:
			elem = new OpCodeHandler_MP(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.MS:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ms(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.MV:
			elem = new OpCodeHandler_MV(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.MV_GV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Mv_Gv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.MV_GV_REXW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Mv_Gv_REXW(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.NIB:
			elem = new OpCodeHandler_NIb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.OB_REG:
			elem = new OpCodeHandler_Ob_Reg(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.OV_REG:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Ov_Reg(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.P_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_P_Ev(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.P_EV_IB:
			code = deserializer.readCode();
			elem = new OpCodeHandler_P_Ev_Ib(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.P_Q:
			elem = new OpCodeHandler_P_Q(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.P_Q_IB:
			elem = new OpCodeHandler_P_Q_Ib(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.P_R:
			elem = new OpCodeHandler_P_R(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.P_W:
			elem = new OpCodeHandler_P_W(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.PUSH_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_PushEv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.PUSH_IB2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_PushIb2(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.PUSH_IZ:
			code = deserializer.readCode();
			elem = new OpCodeHandler_PushIz(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.PUSH_OP_SIZE_REG_4A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_PushOpSizeReg(code, code + 1, code + 2, deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.PUSH_OP_SIZE_REG_4B:
			code = deserializer.readCode();
			elem = new OpCodeHandler_PushOpSizeReg(code, code + 1, Code.INVALID, deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.PUSH_SIMPLE2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_PushSimple2(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.PUSH_SIMPLE_REG:
			elem = new OpCodeHandler_PushSimpleReg(deserializer.readInt32(), code = deserializer.readCode(), code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.Q_P:
			elem = new OpCodeHandler_Q_P(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.R_C_3A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_R_C(code, code + 1, deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.R_C_3B:
			elem = new OpCodeHandler_R_C(deserializer.readCode(), Code.INVALID, deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.R_DI_P_N:
			elem = new OpCodeHandler_rDI_P_N(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.R_DI_VX_RX:
			elem = new OpCodeHandler_rDI_VX_RX(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.REG:
			elem = new OpCodeHandler_Reg(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.REG_IB2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Reg_Ib2(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.REG_IZ:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Reg_Iz(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.REG_OB:
			elem = new OpCodeHandler_Reg_Ob(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.REG_OV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Reg_Ov(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.REG_XB:
			elem = new OpCodeHandler_Reg_Xb(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.REG_XV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Reg_Xv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.REG_XV2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Reg_Xv2(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.REG_YB:
			elem = new OpCodeHandler_Reg_Yb(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.REG_YV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Reg_Yv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.REG_IB:
			elem = new OpCodeHandler_RegIb(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.REG_IB3:
			elem = new OpCodeHandler_RegIb3(deserializer.readInt32());
			break;

		case LegacyOpCodeHandlerKind.REG_IZ2:
			elem = new OpCodeHandler_RegIz2(deserializer.readInt32());
			break;

		case LegacyOpCodeHandlerKind.RESERVEDNOP:
			elem = new OpCodeHandler_Reservednop(deserializer.readHandler(), deserializer.readHandler());
			break;

		case LegacyOpCodeHandlerKind.RIB:
			elem = new OpCodeHandler_RIb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.RIB_IB:
			elem = new OpCodeHandler_RIbIb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.RV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Rv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.RV_32_64:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Rv_32_64(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.RV_MW_GW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_RvMw_Gw(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE:
			elem = new OpCodeHandler_Simple(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.SIMPLE_MOD_RM:
			elem = new OpCodeHandler_Simple_ModRM(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.SIMPLE2_3A:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Simple2(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE2_3B:
			elem = new OpCodeHandler_Simple2(deserializer.readCode(), deserializer.readCode(), deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.SIMPLE2_IW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Simple2Iw(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE3:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Simple3(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE4:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Simple4(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE4B:
			code = deserializer.readCode();
			int code2 = deserializer.readCode();
			elem = new OpCodeHandler_Simple4(code, code2);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE5:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Simple5(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE5_A32:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Simple5_a32(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE5_MOD_RM_AS:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Simple5_ModRM_as(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.SIMPLE_REG:
			elem = new OpCodeHandler_SimpleReg(deserializer.readCode(), deserializer.readInt32());
			break;

		case LegacyOpCodeHandlerKind.ST_STI:
			elem = new OpCodeHandler_ST_STi(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.STI:
			elem = new OpCodeHandler_STi(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.STI_ST:
			elem = new OpCodeHandler_STi_ST(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.SW_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Sw_Ev(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.V_EV:
			elem = new OpCodeHandler_V_Ev(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.VM:
			elem = new OpCodeHandler_VM(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.VN:
			elem = new OpCodeHandler_VN(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.VQ:
			elem = new OpCodeHandler_VQ(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.VRIB_IB:
			elem = new OpCodeHandler_VRIbIb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.VW_2:
			elem = new OpCodeHandler_VW(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.VW_3:
			elem = new OpCodeHandler_VW(deserializer.readCode(), deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.VWIB_2:
			elem = new OpCodeHandler_VWIb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.VWIB_3:
			elem = new OpCodeHandler_VWIb(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.VX_E_IB:
			elem = new OpCodeHandler_VX_E_Ib(code = deserializer.readCode(), code + 1);
			break;

		case LegacyOpCodeHandlerKind.VX_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VX_Ev(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.WBINVD:
			elem = new OpCodeHandler_Wbinvd();
			break;

		case LegacyOpCodeHandlerKind.WV:
			elem = new OpCodeHandler_WV(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.XB_YB:
			elem = new OpCodeHandler_Xb_Yb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.XCHG_REG_R_AX:
			elem = new OpCodeHandler_Xchg_Reg_rAX(deserializer.readInt32());
			break;

		case LegacyOpCodeHandlerKind.XV_YV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Xv_Yv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.YB_REG:
			elem = new OpCodeHandler_Yb_Reg(deserializer.readCode(), deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.YB_XB:
			elem = new OpCodeHandler_Yb_Xb(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.YV_REG:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Yv_Reg(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.YV_REG2:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Yv_Reg2(code, code + 1);
			break;

		case LegacyOpCodeHandlerKind.YV_XV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Yv_Xv(code, code + 1, code + 2);
			break;

		case LegacyOpCodeHandlerKind.M_SW:
			code = deserializer.readCode();
			elem = new OpCodeHandler_M_Sw(code);
			break;

		case LegacyOpCodeHandlerKind.SW_M:
			code = deserializer.readCode();
			elem = new OpCodeHandler_Sw_M(code);
			break;

		case LegacyOpCodeHandlerKind.RQ:
			elem = new OpCodeHandler_Rq(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.GD_RD:
			elem = new OpCodeHandler_Gd_Rd(deserializer.readCode());
			break;

		case LegacyOpCodeHandlerKind.PREFIX_ES_CS_SS_DS:
			elem = new OpCodeHandler_PrefixEsCsSsDs(deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.PREFIX_FS_GS:
			elem = new OpCodeHandler_PrefixFsGs(deserializer.readRegister());
			break;

		case LegacyOpCodeHandlerKind.PREFIX66:
			elem = new OpCodeHandler_Prefix66();
			break;

		case LegacyOpCodeHandlerKind.PREFIX67:
			elem = new OpCodeHandler_Prefix67();
			break;

		case LegacyOpCodeHandlerKind.PREFIX_F0:
			elem = new OpCodeHandler_PrefixF0();
			break;

		case LegacyOpCodeHandlerKind.PREFIX_F2:
			elem = new OpCodeHandler_PrefixF2();
			break;

		case LegacyOpCodeHandlerKind.PREFIX_F3:
			elem = new OpCodeHandler_PrefixF3();
			break;

		case LegacyOpCodeHandlerKind.PREFIX_REX:
			elem = new OpCodeHandler_PrefixREX(deserializer.readHandler(), deserializer.readInt32());
			break;

		default:
			throw new UnsupportedOperationException();
		}
		result[resultIndex] = elem;
		return 1;
	}
}
