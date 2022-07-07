// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.enc.EncFlags3;
import com.github.icedland.iced.x86.internal.enc.EncoderData;

/** DO NOT USE: INTERNAL API */
public final class InternalEncoderOpCodeHandlers {
	private InternalEncoderOpCodeHandlers() {
	}

	/** DO NOT USE: INTERNAL API */
	public static final OpCodeHandler[] handlers = createHandlers();

	@SuppressWarnings("deprecation")
	private static OpCodeHandler[] createHandlers() {
		int[] encFlags1 = EncoderData.encFlags1;
		int[] encFlags2 = EncoderData.encFlags2;
		int[] encFlags3Data = EncoderData.encFlags3;
		OpCodeHandler[] handlers = new OpCodeHandler[IcedConstants.CODE_ENUM_COUNT];
		int i = 0;
		InternalOpCodeHandlers.InvalidHandler invalidHandler = new InternalOpCodeHandlers.InvalidHandler();
		for (; i < encFlags1.length; i++) {
			int encFlags3 = encFlags3Data[i];
			OpCodeHandler handler;
			switch ((encFlags3 >>> EncFlags3.ENCODING_SHIFT) & EncFlags3.ENCODING_MASK) {
			case EncodingKind.LEGACY:
				if (i == Code.INVALID)
					handler = invalidHandler;
				else if (i <= Code.DECLAREQWORD)
					handler = new InternalOpCodeHandlers.DeclareDataHandler(i);
				else if (i == Code.ZERO_BYTES)
					handler = new InternalOpCodeHandlers.ZeroBytesHandler(i);
				else
					handler = new InternalOpCodeHandlers.LegacyHandler(encFlags1[i], encFlags2[i], encFlags3);
				break;

			case EncodingKind.VEX:
				handler = new InternalOpCodeHandlers.VexHandler(encFlags1[i], encFlags2[i], encFlags3);
				break;

			case EncodingKind.EVEX:
				handler = new InternalOpCodeHandlers.EvexHandler(encFlags1[i], encFlags2[i], encFlags3);
				break;

			case EncodingKind.XOP:
				handler = new InternalOpCodeHandlers.XopHandler(encFlags1[i], encFlags2[i], encFlags3);
				break;

			case EncodingKind.D3NOW:
				handler = new InternalOpCodeHandlers.D3nowHandler(encFlags2[i], encFlags3);
				break;

			case EncodingKind.MVEX:
				handler = new InternalOpCodeHandlers.MvexHandler(encFlags1[i], encFlags2[i], encFlags3);
				break;

			default:
				throw new UnsupportedOperationException();
			}
			handlers[i] = handler;
		}
		if (i != handlers.length)
			throw new UnsupportedOperationException();

		return handlers;
	}
}
