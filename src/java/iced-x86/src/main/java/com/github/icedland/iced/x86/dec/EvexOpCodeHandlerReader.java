// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.internal.dec.EvexOpCodeHandlerKind;

final class EvexOpCodeHandlerReader extends OpCodeHandlerReader {
	@Override
	int readHandlers(TableDeserializer deserializer, OpCodeHandler[] result, int resultIndex) {
		OpCodeHandler elem;
		int code;
		switch (deserializer.readEvexOpCodeHandlerKind()) {
		case EvexOpCodeHandlerKind.INVALID:
			elem = OpCodeHandler_Invalid.Instance;
			break;

		case EvexOpCodeHandlerKind.INVALID2:
			result[resultIndex] = OpCodeHandler_Invalid.Instance;
			result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
			return 2;

		case EvexOpCodeHandlerKind.DUP:
			int count = deserializer.readInt32();
			OpCodeHandler handler = deserializer.readHandler();
			for (int i = 0; i < count; i++)
				result[resultIndex + i] = handler;
			return count;

		case EvexOpCodeHandlerKind.HANDLER_REFERENCE:
			elem = deserializer.readHandlerReference();
			break;

		case EvexOpCodeHandlerKind.ARRAY_REFERENCE:
			throw new UnsupportedOperationException();

		case EvexOpCodeHandlerKind.RM:
			elem = new OpCodeHandler_RM(deserializer.readHandler(), deserializer.readHandler());
			break;

		case EvexOpCodeHandlerKind.GROUP:
			elem = new OpCodeHandler_Group(deserializer.readArrayReference(EvexOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case EvexOpCodeHandlerKind.W:
			elem = new OpCodeHandler_W(deserializer.readHandler(), deserializer.readHandler());
			break;

		case EvexOpCodeHandlerKind.MANDATORY_PREFIX2:
			elem = new OpCodeHandler_MandatoryPrefix2(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case EvexOpCodeHandlerKind.VECTOR_LENGTH:
			elem = new OpCodeHandler_VectorLength_EVEX(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case EvexOpCodeHandlerKind.VECTOR_LENGTH_ER:
			elem = new OpCodeHandler_VectorLength_EVEX_er(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case EvexOpCodeHandlerKind.ED_V_IB:
			elem = new OpCodeHandler_EVEX_Ed_V_Ib(deserializer.readRegister(), code = deserializer.readCode(), code + 1, deserializer.readTupleType(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.EV_VX:
			code = deserializer.readCode();
			elem = new OpCodeHandler_EVEX_Ev_VX(code, code + 1, deserializer.readTupleType(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.EV_VX_IB:
			elem = new OpCodeHandler_EVEX_Ev_VX_Ib(deserializer.readRegister(), code = deserializer.readCode(), code + 1);
			break;

		case EvexOpCodeHandlerKind.GV_W_ER:
			elem = new OpCodeHandler_EVEX_Gv_W_er(deserializer.readRegister(), code = deserializer.readCode(), code + 1, deserializer.readTupleType(), deserializer.readBoolean());
			break;

		case EvexOpCodeHandlerKind.GV_M_VX_IB:
			elem = new OpCodeHandler_EVEX_GvM_VX_Ib(deserializer.readRegister(), code = deserializer.readCode(), code + 1, deserializer.readTupleType(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.HK_WIB_3:
			elem = new OpCodeHandler_EVEX_HkWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.HK_WIB_3B:
			elem = new OpCodeHandler_EVEX_HkWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.HWIB:
			elem = new OpCodeHandler_EVEX_HWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.KK_HW_3:
			elem = new OpCodeHandler_EVEX_KkHW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.KK_HW_3B:
			elem = new OpCodeHandler_EVEX_KkHW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.KK_HWIB_SAE_3:
			elem = new OpCodeHandler_EVEX_KkHWIb_sae(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.KK_HWIB_SAE_3B:
			elem = new OpCodeHandler_EVEX_KkHWIb_sae(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.KK_HWIB_3:
			elem = new OpCodeHandler_EVEX_KkHWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.KK_HWIB_3B:
			elem = new OpCodeHandler_EVEX_KkHWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.KK_WIB_3:
			elem = new OpCodeHandler_EVEX_KkWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.KK_WIB_3B:
			elem = new OpCodeHandler_EVEX_KkWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.KP1_HW:
			elem = new OpCodeHandler_EVEX_KP1HW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.KR:
			elem = new OpCodeHandler_EVEX_KR(deserializer.readRegister(), deserializer.readCode());
			break;

		case EvexOpCodeHandlerKind.MV:
			elem = new OpCodeHandler_EVEX_MV(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.V_H_EV_ER:
			elem = new OpCodeHandler_EVEX_V_H_Ev_er(deserializer.readRegister(), code = deserializer.readCode(), code + 1, deserializer.readTupleType(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.V_H_EV_IB:
			elem = new OpCodeHandler_EVEX_V_H_Ev_Ib(deserializer.readRegister(), code = deserializer.readCode(), code + 1, deserializer.readTupleType(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VHM:
			elem = new OpCodeHandler_EVEX_VHM(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VHW_3:
			elem = new OpCodeHandler_EVEX_VHW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VHW_4:
			elem = new OpCodeHandler_EVEX_VHW(deserializer.readRegister(), deserializer.readCode(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VHWIB:
			elem = new OpCodeHandler_EVEX_VHWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VK:
			elem = new OpCodeHandler_EVEX_VK(deserializer.readRegister(), deserializer.readCode());
			break;

		case EvexOpCodeHandlerKind.VK_VSIB:
			elem = new OpCodeHandler_EVEX_Vk_VSIB(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VK_EV_REXW_2:
			elem = new OpCodeHandler_EVEX_VkEv_REXW(deserializer.readRegister(), deserializer.readCode());
			break;

		case EvexOpCodeHandlerKind.VK_EV_REXW_3:
			elem = new OpCodeHandler_EVEX_VkEv_REXW(deserializer.readRegister(), deserializer.readCode(), deserializer.readCode());
			break;

		case EvexOpCodeHandlerKind.VK_HM:
			elem = new OpCodeHandler_EVEX_VkHM(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VK_HW_3:
			elem = new OpCodeHandler_EVEX_VkHW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_HW_3B:
			elem = new OpCodeHandler_EVEX_VkHW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.VK_HW_5:
			elem = new OpCodeHandler_EVEX_VkHW(deserializer.readRegister(), deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_HW_ER_4:
			elem = new OpCodeHandler_EVEX_VkHW_er(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), deserializer.readBoolean(), false);
			break;

		case EvexOpCodeHandlerKind.VK_HW_ER_4B:
			elem = new OpCodeHandler_EVEX_VkHW_er(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), deserializer.readBoolean(), true);
			break;

		case EvexOpCodeHandlerKind.VK_HW_ER_UR_3:
			elem = new OpCodeHandler_EVEX_VkHW_er_ur(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_HW_ER_UR_3B:
			elem = new OpCodeHandler_EVEX_VkHW_er_ur(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.VK_HWIB_3:
			elem = new OpCodeHandler_EVEX_VkHWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_HWIB_3B:
			elem = new OpCodeHandler_EVEX_VkHWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.VK_HWIB_5:
			elem = new OpCodeHandler_EVEX_VkHWIb(deserializer.readRegister(), deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_HWIB_ER_4:
			elem = new OpCodeHandler_EVEX_VkHWIb_er(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_HWIB_ER_4B:
			elem = new OpCodeHandler_EVEX_VkHWIb_er(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.VK_M:
			elem = new OpCodeHandler_EVEX_VkM(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VK_W_3:
			elem = new OpCodeHandler_EVEX_VkW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_W_3B:
			elem = new OpCodeHandler_EVEX_VkW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.VK_W_4:
			elem = new OpCodeHandler_EVEX_VkW(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_W_4B:
			elem = new OpCodeHandler_EVEX_VkW(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.VK_W_ER_4:
			elem = new OpCodeHandler_EVEX_VkW_er(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), deserializer.readBoolean());
			break;

		case EvexOpCodeHandlerKind.VK_W_ER_5:
			elem = new OpCodeHandler_EVEX_VkW_er(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), deserializer.readBoolean());
			break;

		case EvexOpCodeHandlerKind.VK_W_ER_6:
			elem = new OpCodeHandler_EVEX_VkW_er(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), deserializer.readBoolean(), deserializer.readBoolean());
			break;

		case EvexOpCodeHandlerKind.VK_WIB_3:
			elem = new OpCodeHandler_EVEX_VkWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), false);
			break;

		case EvexOpCodeHandlerKind.VK_WIB_3B:
			elem = new OpCodeHandler_EVEX_VkWIb(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), true);
			break;

		case EvexOpCodeHandlerKind.VK_WIB_ER:
			elem = new OpCodeHandler_EVEX_VkWIb_er(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VM:
			elem = new OpCodeHandler_EVEX_VM(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VSIB_K1:
			elem = new OpCodeHandler_EVEX_VSIB_k1(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VSIB_K1_VX:
			elem = new OpCodeHandler_EVEX_VSIB_k1_VX(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VW:
			elem = new OpCodeHandler_EVEX_VW(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VW_ER:
			elem = new OpCodeHandler_EVEX_VW_er(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.VX_EV:
			code = deserializer.readCode();
			elem = new OpCodeHandler_EVEX_VX_Ev(code, code + 1, deserializer.readTupleType(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.WK_HV:
			elem = new OpCodeHandler_EVEX_WkHV(deserializer.readRegister(), deserializer.readCode());
			break;

		case EvexOpCodeHandlerKind.WK_V_3:
			elem = new OpCodeHandler_EVEX_WkV(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.WK_V_4A:
			elem = new OpCodeHandler_EVEX_WkV(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.WK_V_4B:
			elem = new OpCodeHandler_EVEX_WkV(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType(), deserializer.readBoolean());
			break;

		case EvexOpCodeHandlerKind.WK_VIB:
			elem = new OpCodeHandler_EVEX_WkVIb(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.WK_VIB_ER:
			elem = new OpCodeHandler_EVEX_WkVIb_er(deserializer.readRegister(), deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		case EvexOpCodeHandlerKind.WV:
			elem = new OpCodeHandler_EVEX_WV(deserializer.readRegister(), deserializer.readCode(), deserializer.readTupleType());
			break;

		default:
			throw new UnsupportedOperationException();
		}
		result[resultIndex] = elem;
		return 1;
	}
}
