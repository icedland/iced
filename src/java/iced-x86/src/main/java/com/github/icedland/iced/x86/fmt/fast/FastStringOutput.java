// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.fast;

/**
 * {@link FastFormatter} output
 */
public final class FastStringOutput {
	private char[] buffer;
	private int bufferLen;

	/**
	 * Gets the current length
	 */
	public int size() {
		return bufferLen;
	}

	/**
	 * Constructor
	 */
	public FastStringOutput() {
		buffer = new char[64];
	}

	/**
	 * Constructor
	 *
	 * @param capacity Initial capacity
	 */
	public FastStringOutput(int capacity) {
		buffer = new char[capacity];
	}

	/**
	 * Append a char
	 *
	 * @param c Character to append
	 */
	public void append(char c) {
		char[] buffer = this.buffer;
		int bufferLen = this.bufferLen;
		if (Integer.compareUnsigned(bufferLen, buffer.length) >= 0) {
			resize(1);
			buffer = this.buffer;
		}
		buffer[bufferLen] = c;
		this.bufferLen = bufferLen + 1;
	}

	/**
	 * Append a string
	 *
	 * @param value String to append (may be {@code null})
	 */
	public void append(String value) {
		if (value != null) {
			char[] buffer = this.buffer;
			int bufferLen = this.bufferLen;
			if (Integer.compareUnsigned(bufferLen + value.length(), buffer.length) > 0) {
				resize(value.length());
				buffer = this.buffer;
			}
			for (int i = 0; i < value.length(); i++) {
				buffer[bufferLen] = value.charAt(i);
				bufferLen++;
			}
			this.bufferLen = bufferLen;
		}
	}

	private void resize(int extraCount) {
		int capacity = buffer.length;
		long required = (long)capacity + (long)extraCount;
		long newCount = Math.min(Math.max((long)capacity << 1, required), 0x7FFF_FFFF);
		if (newCount < required)
			throw new OutOfMemoryError();
		char[] newArray = new char[(int)newCount];
		System.arraycopy(buffer, 0, newArray, 0, bufferLen);
		this.buffer = newArray;
	}

	/**
	 * Resets the buffer to an empty string
	 */
	public void clear() {
		bufferLen = 0;
	}

	/**
	 * Gets the current string
	 */
	@Override
	public String toString() {
		return new String(buffer, 0, bufferLen);
	}
}
