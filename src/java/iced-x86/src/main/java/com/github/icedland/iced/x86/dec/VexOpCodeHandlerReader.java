// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.internal.dec.VexOpCodeHandlerKind;

final class VexOpCodeHandlerReader extends OpCodeHandlerReader {
	@Override
	int readHandlers(TableDeserializer deserializer, OpCodeHandler[] result, int resultIndex) {
		OpCodeHandler elem;
		int code;
		switch (deserializer.readVexOpCodeHandlerKind()) {
		case VexOpCodeHandlerKind.INVALID:
			elem = OpCodeHandler_Invalid.Instance;
			break;

		case VexOpCodeHandlerKind.INVALID2:
			result[resultIndex] = OpCodeHandler_Invalid.Instance;
			result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
			return 2;

		case VexOpCodeHandlerKind.DUP:
			int count = deserializer.readInt32();
			OpCodeHandler handler = deserializer.readHandlerOrNull();
			for (int i = 0; i < count; i++)
				result[resultIndex + i] = handler;
			return count;

		case VexOpCodeHandlerKind.NULL:
			elem = null;
			break;

		case VexOpCodeHandlerKind.INVALID_NO_MOD_RM:
			elem = OpCodeHandler_Invalid_NoModRM.Instance;
			break;

		case VexOpCodeHandlerKind.BITNESS:
			elem = new OpCodeHandler_Bitness(deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.BITNESS_DONT_READ_MOD_RM:
			elem = new OpCodeHandler_Bitness_DontReadModRM(deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.HANDLER_REFERENCE:
			elem = deserializer.readHandlerReference();
			break;

		case VexOpCodeHandlerKind.ARRAY_REFERENCE:
			throw new UnsupportedOperationException();

		case VexOpCodeHandlerKind.RM:
			elem = new OpCodeHandler_RM(deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.GROUP:
			elem = new OpCodeHandler_Group(deserializer.readArrayReference(VexOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case VexOpCodeHandlerKind.GROUP8X64:
			elem = new OpCodeHandler_Group8x64(deserializer.readArrayReference(VexOpCodeHandlerKind.ARRAY_REFERENCE), deserializer.readArrayReference(VexOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case VexOpCodeHandlerKind.W:
			elem = new OpCodeHandler_W(deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.MANDATORY_PREFIX2_1:
			elem = new OpCodeHandler_MandatoryPrefix2(deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.MANDATORY_PREFIX2_4:
			elem = new OpCodeHandler_MandatoryPrefix2(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.MANDATORY_PREFIX2_NO_MOD_RM:
			elem = new OpCodeHandler_MandatoryPrefix2_NoModRM(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.VECTOR_LENGTH_NO_MOD_RM:
			elem = new OpCodeHandler_VectorLength_NoModRM_VEX(deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.VECTOR_LENGTH:
			elem = new OpCodeHandler_VectorLength_VEX(deserializer.readHandler(), deserializer.readHandler());
			break;

		case VexOpCodeHandlerKind.ED_V_IB:
			elem = new OpCodeHandler_VEX_Ed_V_Ib(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.EV_VX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Ev_VX(code, code + 1);
			break;

		case VexOpCodeHandlerKind.G_VK:
			elem = new OpCodeHandler_VEX_G_VK(deserializer.readCode(), deserializer.readRegister());
			break;

		case VexOpCodeHandlerKind.GV_EV_GV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Gv_Ev_Gv(code, code + 1);
			break;

		case VexOpCodeHandlerKind.EV_GV_GV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Ev_Gv_Gv(code, code + 1);
			break;

		case VexOpCodeHandlerKind.GV_EV_IB:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Gv_Ev_Ib(code, code + 1);
			break;

		case VexOpCodeHandlerKind.GV_EV_ID:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Gv_Ev_Id(code, code + 1);
			break;

		case VexOpCodeHandlerKind.GV_GPR_IB:
			elem = new OpCodeHandler_VEX_Gv_GPR_Ib(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.GV_GV_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Gv_Gv_Ev(code, code + 1);
			break;

		case VexOpCodeHandlerKind.GV_RX:
			elem = new OpCodeHandler_VEX_Gv_RX(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.GV_W:
			elem = new OpCodeHandler_VEX_Gv_W(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.GV_M_VX_IB:
			elem = new OpCodeHandler_VEX_GvM_VX_Ib(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.HRIB:
			elem = new OpCodeHandler_VEX_HRIb(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.HV_ED_ID:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Hv_Ed_Id(code, code + 1);
			break;

		case VexOpCodeHandlerKind.HV_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Hv_Ev(code, code + 1);
			break;

		case VexOpCodeHandlerKind.M:
			elem = new OpCodeHandler_VEX_M(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.MHV:
			elem = new OpCodeHandler_VEX_MHV(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.M_VK:
			elem = new OpCodeHandler_VEX_M_VK(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.MV:
			elem = new OpCodeHandler_VEX_MV(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.R_DI_VX_RX:
			elem = new OpCodeHandler_VEX_rDI_VX_RX(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.RD_RQ:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_RdRq(code, code + 1);
			break;

		case VexOpCodeHandlerKind.SIMPLE:
			elem = new OpCodeHandler_VEX_Simple(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHEV:
			elem = new OpCodeHandler_VEX_VHEv(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.VHEV_IB:
			elem = new OpCodeHandler_VEX_VHEvIb(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.VHIS4_W:
			elem = new OpCodeHandler_VEX_VHIs4W(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHIS5_W:
			elem = new OpCodeHandler_VEX_VHIs5W(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHM:
			elem = new OpCodeHandler_VEX_VHM(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHW_2:
			elem = new OpCodeHandler_VEX_VHW(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHW_3:
			elem = new OpCodeHandler_VEX_VHW(deserializer.readRegister(), deserializer.readCode(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHW_4:
			elem = new OpCodeHandler_VEX_VHW(deserializer.readRegister(), deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHWIB_2:
			elem = new OpCodeHandler_VEX_VHWIb(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHWIB_4:
			elem = new OpCodeHandler_VEX_VHWIb(deserializer.readRegister(), deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHWIS4:
			elem = new OpCodeHandler_VEX_VHWIs4(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VHWIS5:
			elem = new OpCodeHandler_VEX_VHWIs5(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VK_HK_RK:
			elem = new OpCodeHandler_VEX_VK_HK_RK(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VK_R:
			elem = new OpCodeHandler_VEX_VK_R(deserializer.readCode(), deserializer.readRegister());
			break;

		case VexOpCodeHandlerKind.VK_RK:
			elem = new OpCodeHandler_VEX_VK_RK(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VK_RK_IB:
			elem = new OpCodeHandler_VEX_VK_RK_Ib(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VK_WK:
			elem = new OpCodeHandler_VEX_VK_WK(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VM:
			elem = new OpCodeHandler_VEX_VM(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VW_2:
			elem = new OpCodeHandler_VEX_VW(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VW_3:
			elem = new OpCodeHandler_VEX_VW(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VWH:
			elem = new OpCodeHandler_VEX_VWH(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VWIB_2:
			elem = new OpCodeHandler_VEX_VWIb(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VWIB_3:
			elem = new OpCodeHandler_VEX_VWIb(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case VexOpCodeHandlerKind.VX_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_VX_Ev(code, code + 1);
			break;

		case VexOpCodeHandlerKind.VX_VSIB_HX:
			elem = new OpCodeHandler_VEX_VX_VSIB_HX(deserializer.readRegister(), deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.WHV:
			elem = new OpCodeHandler_VEX_WHV(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.WV:
			elem = new OpCodeHandler_VEX_WV(deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.WVIB:
			elem = new OpCodeHandler_VEX_WVIb(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VT_SIBMEM:
			elem = new OpCodeHandler_VEX_VT_SIBMEM(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.SIBMEM_VT:
			elem = new OpCodeHandler_VEX_SIBMEM_VT(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VT:
			elem = new OpCodeHandler_VEX_VT(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VT_RT_HT:
			elem = new OpCodeHandler_VEX_VT_RT_HT(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.OPTIONS_DONT_READ_MOD_RM:
			elem = new OpCodeHandler_Options_DontReadModRM(deserializer.readHandler(), deserializer.readHandler(), deserializer.readDecoderOptions());
			break;

		case VexOpCodeHandlerKind.GQ_HK_RK:
			elem = new OpCodeHandler_VEX_Gq_HK_RK(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.VK_R_IB:
			elem = new OpCodeHandler_VEX_VK_R_Ib(deserializer.readCode(), deserializer.readRegister());
			break;

		case VexOpCodeHandlerKind.GV_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Gv_Ev(code, code + 1);
			break;

		case VexOpCodeHandlerKind.EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_VEX_Ev(code, code + 1);
			break;

		case VexOpCodeHandlerKind.K_JB:
			elem = new OpCodeHandler_VEX_K_Jb(deserializer.readCode());
			break;

		case VexOpCodeHandlerKind.K_JZ:
			elem = new OpCodeHandler_VEX_K_Jz(deserializer.readCode());
			break;

		default:
			throw new UnsupportedOperationException();
		}
		result[resultIndex] = elem;
		return 1;
	}
}
