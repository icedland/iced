// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.CodeWriter;

final class CodeWriterImpl implements CodeWriter {
	public int bytesWritten;
	final CodeWriter codeWriter;

	public CodeWriterImpl(CodeWriter codeWriter) {
		if (codeWriter == null)
			throw new NullPointerException("codeWriter");
		this.codeWriter = codeWriter;
	}

	@Override
	public void writeByte(byte value) {
		bytesWritten++;
		codeWriter.writeByte(value);
	}
}
