// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

/**
 * DO NOT USE: INTERNAL API
 */
public final class ResourceReader {
	// InputStream.readAllBytes() is available in JDK 9
	private static byte[] readAllBytes(InputStream stream) throws IOException {
		byte[] buffer = new byte[2048];
		ByteArrayOutputStream result = new ByteArrayOutputStream();
		while (true) {
			int read = stream.read(buffer);
			if (read == -1)
				break;
			result.write(buffer, 0, read);
		}
		return result.toByteArray();
	}

	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static byte[] readByteArray(ClassLoader classLoader, String resourceName) {
		try (InputStream stream = classLoader.getResourceAsStream(resourceName)) {
			if (stream == null)
				throw new IllegalArgumentException(String.format("Missing resource: %s", resourceName));
			return readAllBytes(stream);
		} catch (IOException ioex) {
			throw new IllegalArgumentException(ioex);
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 */
	public static short[] readShortArray(ClassLoader classLoader, String resourceName) {
		byte[] bytes;
		try (InputStream stream = classLoader.getResourceAsStream(resourceName)) {
			if (stream == null)
				throw new IllegalArgumentException(String.format("Missing resource: %s", resourceName));
			bytes = readAllBytes(stream);
		} catch (IOException ioex) {
			throw new IllegalArgumentException(ioex);
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
	public static int[] readIntArray(ClassLoader classLoader, String resourceName) {
		byte[] bytes;
		try (InputStream stream = classLoader.getResourceAsStream(resourceName)) {
			if (stream == null)
				throw new IllegalArgumentException(String.format("Missing resource: %s", resourceName));
			bytes = readAllBytes(stream);
		} catch (IOException ioex) {
			throw new IllegalArgumentException(ioex);
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
