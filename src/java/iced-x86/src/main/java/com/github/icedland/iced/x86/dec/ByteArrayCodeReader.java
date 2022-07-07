// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

/**
 * A {@link CodeReader} that reads data from a byte array
 */
public final class ByteArrayCodeReader extends CodeReader {
	private final byte[] data;
	private int currentPosition;
	private final int startPosition;
	private final int endPosition;

	/**
	 * Current position
	 */
	public int getPosition() {
		return currentPosition - startPosition;
	}

	/**
	 * Current position
	 */
	public void setPosition(int value) {
		if (Integer.compareUnsigned(value, size()) > 0)
			throw new IllegalArgumentException("value");
		currentPosition = startPosition + value;
	}

	/**
	 * Number of bytes that can be read
	 */
	public int size() {
		return endPosition - startPosition;
	}

	/**
	 * Checks if it's possible to read another byte
	 */
	public boolean canReadByte() {
		return currentPosition < endPosition;
	}

	/**
	 * Constructor
	 *
	 * @param data Data
	 */
	public ByteArrayCodeReader(byte[] data) {
		if (data == null)
			throw new NullPointerException("data");
		this.data = data;
		currentPosition = 0;
		startPosition = 0;
		endPosition = data.length;
	}

	/**
	 * Constructor
	 *
	 * @param data Data
	 * @param index Start index
	 * @param count Number of bytes
	 */
	public ByteArrayCodeReader(byte[] data, int index, int count) {
		if (data == null)
			throw new NullPointerException("data");
		this.data = data;
		if (index < 0)
			throw new IllegalArgumentException("index");
		if (count < 0)
			throw new IllegalArgumentException("count");
		if (Integer.compareUnsigned(index + count, data.length) > 0)
			throw new IllegalArgumentException("count");
		currentPosition = index;
		startPosition = index;
		endPosition = index + count;
	}

	/**
	 * Reads the next byte or returns less than 0 if there are no more bytes
	 */
	@Override
	public int readByte() {
		if (currentPosition >= endPosition)
			return -1;
		return data[currentPosition++] & 0xFF;
	}
}
