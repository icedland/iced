// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && MVEX
using System;

namespace Iced.Intel.DecoderInternal {
	sealed class MvexOpCodeHandlerReader : OpCodeHandlerReader {
		public override int ReadHandlers(ref TableDeserializer deserializer, OpCodeHandler?[] result, int resultIndex) {
			ref var elem = ref result[resultIndex];
			switch (deserializer.ReadMvexOpCodeHandlerKind()) {
			case MvexOpCodeHandlerKind.Invalid:
				elem = OpCodeHandler_Invalid.Instance;
				return 1;

			case MvexOpCodeHandlerKind.Invalid2:
				result[resultIndex] = OpCodeHandler_Invalid.Instance;
				result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
				return 2;

			case MvexOpCodeHandlerKind.Dup:
				int count = deserializer.ReadInt32();
				var handler = deserializer.ReadHandler();
				for (int i = 0; i < count; i++)
					result[resultIndex + i] = handler;
				return count;

			case MvexOpCodeHandlerKind.HandlerReference:
				elem = deserializer.ReadHandlerReference();
				return 1;

			case MvexOpCodeHandlerKind.ArrayReference:
				throw new InvalidOperationException();

			case MvexOpCodeHandlerKind.RM:
				elem = new OpCodeHandler_RM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case MvexOpCodeHandlerKind.Group:
				elem = new OpCodeHandler_Group(deserializer.ReadArrayReference((uint)MvexOpCodeHandlerKind.ArrayReference));
				return 1;

			case MvexOpCodeHandlerKind.W:
				elem = new OpCodeHandler_W(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case MvexOpCodeHandlerKind.MandatoryPrefix2:
				elem = new OpCodeHandler_MandatoryPrefix2(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case MvexOpCodeHandlerKind.EH:
				elem = new OpCodeHandler_EH(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case MvexOpCodeHandlerKind.M:
				elem = new OpCodeHandler_MVEX_M(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.MV:
				elem = new OpCodeHandler_MVEX_MV(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.VW:
				elem = new OpCodeHandler_MVEX_VW(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.HWIb:
				elem = new OpCodeHandler_MVEX_HWIb(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.VWIb:
				elem = new OpCodeHandler_MVEX_VWIb(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.VHW:
				elem = new OpCodeHandler_MVEX_VHW(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.VHWIb:
				elem = new OpCodeHandler_MVEX_VHWIb(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.VKW:
				elem = new OpCodeHandler_MVEX_VKW(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.KHW:
				elem = new OpCodeHandler_MVEX_KHW(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.KHWIb:
				elem = new OpCodeHandler_MVEX_KHWIb(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.VSIB:
				elem = new OpCodeHandler_MVEX_VSIB(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.VSIB_V:
				elem = new OpCodeHandler_MVEX_VSIB_V(deserializer.ReadCode());
				return 1;

			case MvexOpCodeHandlerKind.V_VSIB:
				elem = new OpCodeHandler_MVEX_V_VSIB(deserializer.ReadCode());
				return 1;

			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
