// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.enc.EncoderData;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class OpCodeInfos {
	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final OpCodeInfo[] opCodes = createOpCodes();

	private static OpCodeInfo[] createOpCodes() {
		OpCodeInfo[] infos = new OpCodeInfo[IcedConstants.CODE_ENUM_COUNT];
		int[] encFlags1 = EncoderData.encFlags1;
		int[] encFlags2 = EncoderData.encFlags2;
		int[] encFlags3 = EncoderData.encFlags3;
		int[] opcFlags1 = OpCodeInfoData.opcFlags1;
		int[] opcFlags2 = OpCodeInfoData.opcFlags2;
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < infos.length; i++)
			infos[i] = new OpCodeInfo(i, encFlags1[i], encFlags2[i], encFlags3[i], opcFlags1[i], opcFlags2[i], sb);
		return infos;
	}
}
