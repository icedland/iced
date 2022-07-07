// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import com.github.icedland.iced.x86.Mnemonic;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.enc.EncoderData;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class InternalOpCodeInfos {
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
		String[] mnemonics = getMnemonics();
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < infos.length; i++)
			infos[i] = new OpCodeInfo(i, encFlags1[i], encFlags2[i], encFlags3[i], opcFlags1[i], opcFlags2[i], sb, mnemonics);
		return infos;
	}

	private static String[] getMnemonics() {
		String[] mnemonics = new String[IcedConstants.MNEMONIC_ENUM_COUNT];
		for (Field field : Mnemonic.class.getDeclaredFields()) {
			if ((field.getModifiers() & (Modifier.STATIC | Modifier.FINAL)) != (Modifier.STATIC | Modifier.FINAL))
				continue;
			int value;
			try {
				value = field.getInt(null);
			} catch (IllegalAccessException ex) {
				throw new UnsupportedOperationException();
			}
			mnemonics[value] = field.getName();
		}
		for (String s : mnemonics) {
			if (s == null)
				throw new UnsupportedOperationException();
		}
		return mnemonics;
	}
}
