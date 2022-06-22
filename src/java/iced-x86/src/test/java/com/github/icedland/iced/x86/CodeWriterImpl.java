// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import java.io.ByteArrayOutputStream;

public final class CodeWriterImpl implements CodeWriter {
	final ByteArrayOutputStream stream = new ByteArrayOutputStream();

	@Override
	public void writeByte(byte value) {
		stream.write(value);
	}

	public byte[] toArray() {
		return stream.toByteArray();
	}
}
