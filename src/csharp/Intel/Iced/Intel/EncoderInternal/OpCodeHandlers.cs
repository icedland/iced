// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
using System;

namespace Iced.Intel.EncoderInternal {
	static class OpCodeHandlers {
		public static readonly OpCodeHandler[] Handlers;

		static OpCodeHandlers() {
			var encFlags1 = EncoderData.EncFlags1;
			var encFlags2 = EncoderData.EncFlags2;
			var encFlags3Data = EncoderData.EncFlags3;
			var handlers = new OpCodeHandler[IcedConstants.CodeEnumCount];
			int i = 0;
			var invalidHandler = new InvalidHandler();
			for (; i < encFlags1.Length; i++) {
				var encFlags3 = (EncFlags3)encFlags3Data[i];
				OpCodeHandler handler;
				switch ((EncodingKind)(((uint)encFlags3 >> (int)EncFlags3.EncodingShift) & (uint)EncFlags3.EncodingMask)) {
				case EncodingKind.Legacy:
					var code = (Code)i;
					if (code == Code.INVALID)
						handler = invalidHandler;
					else if (code <= Code.DeclareQword)
						handler = new DeclareDataHandler(code);
					else if (code == Code.Zero_bytes)
						handler = new ZeroBytesHandler(code);
					else
						handler = new LegacyHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
					break;

				case EncodingKind.VEX:
#if !NO_VEX
					handler = new VexHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.EVEX:
#if !NO_EVEX
					handler = new EvexHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.XOP:
#if !NO_XOP
					handler = new XopHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.D3NOW:
#if !NO_D3NOW
					handler = new D3nowHandler((EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.MVEX:
#if MVEX
					handler = new MvexHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				default:
					throw new InvalidOperationException();
				}
				handlers[i] = handler;
			}
			if (i != handlers.Length)
				throw new InvalidOperationException();
			Handlers = handlers;
		}
	}
}
#endif
