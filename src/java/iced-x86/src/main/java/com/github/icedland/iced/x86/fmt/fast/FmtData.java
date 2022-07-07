// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.fast;

import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.ResourceReader;
import com.github.icedland.iced.x86.internal.fmt.FormatterStringsTable;

@SuppressWarnings("deprecation")
final class FmtData {
	static final String[] mnemonics;
	static final byte[] flags;

	static byte[] getSerializedData() {
		return ResourceReader.readByteArray(FmtData.class.getClassLoader(), "com/github/icedland/iced/x86/fmt/fast/FmtData.bin");
	}

	static {
		com.github.icedland.iced.x86.internal.DataReader reader = new com.github.icedland.iced.x86.internal.DataReader(getSerializedData());
		String[] strings = FormatterStringsTable.getStringsTable();
		String[] mnemonicsTmp = new String[IcedConstants.CODE_ENUM_COUNT];
		byte[] flagsTmp = new byte[IcedConstants.CODE_ENUM_COUNT];

		char[] ca = new char[1];
		int prevIndex = -1;
		byte prevFlags = FastFmtFlags.NONE;
		for (int i = 0; i < mnemonicsTmp.length; i++) {
			byte f = (byte)reader.readByte();
			int currentIndex;
			if ((f & FastFmtFlags.SAME_AS_PREV) != 0) {
				currentIndex = reader.getIndex();
				reader.setIndex(prevIndex);
			}
			else {
				currentIndex = -1;
				prevIndex = reader.getIndex();
			}
			String mnemonic = strings[reader.readCompressedUInt32()];
			if ((prevFlags & FastFmtFlags.HAS_VPREFIX) == (f & FastFmtFlags.HAS_VPREFIX) &&
					(f & FastFmtFlags.SAME_AS_PREV) != 0) {
				mnemonic = mnemonicsTmp[i - 1];
			}
			else if ((f & FastFmtFlags.HAS_VPREFIX) != 0) {
				ca[0] = 'v';
				mnemonic = (new String(ca) + mnemonic).intern();
			}

			flagsTmp[i] = f;
			mnemonicsTmp[i] = mnemonic;
			prevFlags = f;
			if (currentIndex >= 0)
				reader.setIndex(currentIndex);
		}
		if (reader.canRead())
			throw new UnsupportedOperationException();

		flags = flagsTmp;
		mnemonics = mnemonicsTmp;
	}
}
