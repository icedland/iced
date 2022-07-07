// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.internal.dec.MvexOpCodeHandlerKind;

final class MvexOpCodeHandlerReader extends OpCodeHandlerReader {
	@Override
	int readHandlers(TableDeserializer deserializer, OpCodeHandler[] result, int resultIndex) {
		OpCodeHandler elem;
		switch (deserializer.readMvexOpCodeHandlerKind()) {
		case MvexOpCodeHandlerKind.INVALID:
			elem = OpCodeHandler_Invalid.Instance;
			break;

		case MvexOpCodeHandlerKind.INVALID2:
			result[resultIndex] = OpCodeHandler_Invalid.Instance;
			result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
			return 2;

		case MvexOpCodeHandlerKind.DUP:
			int count = deserializer.readInt32();
			OpCodeHandler handler = deserializer.readHandler();
			for (int i = 0; i < count; i++)
				result[resultIndex + i] = handler;
			return count;

		case MvexOpCodeHandlerKind.HANDLER_REFERENCE:
			elem = deserializer.readHandlerReference();
			break;

		case MvexOpCodeHandlerKind.ARRAY_REFERENCE:
			throw new UnsupportedOperationException();

		case MvexOpCodeHandlerKind.RM:
			elem = new OpCodeHandler_RM(deserializer.readHandler(), deserializer.readHandler());
			break;

		case MvexOpCodeHandlerKind.GROUP:
			elem = new OpCodeHandler_Group(deserializer.readArrayReference(MvexOpCodeHandlerKind.ARRAY_REFERENCE));
			break;

		case MvexOpCodeHandlerKind.W:
			elem = new OpCodeHandler_W(deserializer.readHandler(), deserializer.readHandler());
			break;

		case MvexOpCodeHandlerKind.MANDATORY_PREFIX2:
			elem = new OpCodeHandler_MandatoryPrefix2(deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler(), deserializer.readHandler());
			break;

		case MvexOpCodeHandlerKind.EH:
			elem = new OpCodeHandler_EH(deserializer.readHandler(), deserializer.readHandler());
			break;

		case MvexOpCodeHandlerKind.M:
			elem = new OpCodeHandler_MVEX_M(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.MV:
			elem = new OpCodeHandler_MVEX_MV(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.VW:
			elem = new OpCodeHandler_MVEX_VW(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.HWIB:
			elem = new OpCodeHandler_MVEX_HWIb(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.VWIB:
			elem = new OpCodeHandler_MVEX_VWIb(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.VHW:
			elem = new OpCodeHandler_MVEX_VHW(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.VHWIB:
			elem = new OpCodeHandler_MVEX_VHWIb(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.VKW:
			elem = new OpCodeHandler_MVEX_VKW(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.KHW:
			elem = new OpCodeHandler_MVEX_KHW(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.KHWIB:
			elem = new OpCodeHandler_MVEX_KHWIb(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.VSIB:
			elem = new OpCodeHandler_MVEX_VSIB(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.VSIB_V:
			elem = new OpCodeHandler_MVEX_VSIB_V(deserializer.readCode());
			break;

		case MvexOpCodeHandlerKind.V_VSIB:
			elem = new OpCodeHandler_MVEX_V_VSIB(deserializer.readCode());
			break;

		default:
			throw new UnsupportedOperationException();
		}
		result[resultIndex] = elem;
		return 1;
	}
}
