// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class DataReader {
	final byte[] data;
	final char[] stringData;
	int index;

	/** DO NOT USE: INTERNAL API */
	public DataReader(byte[] data) {
		this(data, 0);
	}

	/** DO NOT USE: INTERNAL API */
	public DataReader(byte[] data, int maxStringLength) {
		this.data = data;
		stringData = new char[maxStringLength];
		index = 0;
	}

	public boolean canRead() {
		return Integer.compareUnsigned(index, data.length) < 0;
	}

	/** DO NOT USE: INTERNAL API */
	public void setIndex(int index) {
		this.index = index;
	}

	/** DO NOT USE: INTERNAL API */
	public int ReadByte() {
		return data[index++] & 0xFF;
	}

	/** DO NOT USE: INTERNAL API */
	public int ReadCompressedUInt32() {
		int result = 0;
		for (int shift = 0; shift < 32; shift += 7) {
			int b = ReadByte();
			if ((b & 0x80) == 0)
				return result | (b << shift);
			result |= (b & 0x7F) << shift;
		}
		throw new UnsupportedOperationException();
	}

	/** DO NOT USE: INTERNAL API */
	public String ReadAsciiString() {
		int length = ReadByte();
		char[] stringData = this.stringData;
		for (int i = 0; i < length; i++)
			stringData[i] = (char)ReadByte();
		return new String(stringData, 0, length);
	}
}
