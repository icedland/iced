// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

import java.io.IOException;
import java.io.InputStream;

/**
 * DO NOT USE: INTERNAL API
 */
public final class ResourceReader {
	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static byte[] ReadByteArray(ClassLoader classLoader, String resourceName) {
		InputStream stream = classLoader.getResourceAsStream(resourceName);
		if (stream == null)
			throw new IllegalArgumentException();
		try {
			return stream.readAllBytes();
		} catch (IOException ioex) {
			throw new IllegalArgumentException();
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static short[] ReadShortArray(ClassLoader classLoader, String resourceName) {
		InputStream stream = classLoader.getResourceAsStream(resourceName);
		if (stream == null)
			throw new IllegalArgumentException();
		byte[] bytes;
		try {
			bytes = stream.readAllBytes();
		} catch (IOException ioex) {
			throw new IllegalArgumentException();
		}
		if ((bytes.length & 1) != 0)
			throw new IllegalArgumentException();
		short[] result = new short[bytes.length / 2];
		for (int i = 0, j = 0; i < bytes.length; i += 2, j++) {
			result[j] = (short)((bytes[i] & 0xFF) | (bytes[i + 1] << 8));
		}
		return result;
	}

	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static int[] ReadIntArray(ClassLoader classLoader, String resourceName) {
		InputStream stream = classLoader.getResourceAsStream(resourceName);
		if (stream == null)
			throw new IllegalArgumentException();
		byte[] bytes;
		try {
			bytes = stream.readAllBytes();
		} catch (IOException ioex) {
			throw new IllegalArgumentException();
		}
		if ((bytes.length & 3) != 0)
			throw new IllegalArgumentException();
		int[] result = new int[bytes.length / 4];
		for (int i = 0, j = 0; i < bytes.length; i += 4, j++) {
			result[j] = (bytes[i] & 0xFF) |
					((bytes[i + 1] & 0xFF) << 8) |
					((bytes[i + 2] & 0xFF) << 16) |
					(bytes[i + 3] << 24);
		}
		return result;
	}
}
