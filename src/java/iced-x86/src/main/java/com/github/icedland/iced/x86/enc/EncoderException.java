// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Instruction;

/**
 * Thrown by an {@link Encoder} when it fails to encode an instruction.
 */
public final class EncoderException extends RuntimeException {
	/**
	 * The instruction the {@link Encoder} failed to encode
	 */
	public final Instruction instruction;

	EncoderException(String message, Instruction instruction) {
		super(message);
		this.instruction = instruction;
	}

	private static final long serialVersionUID = 1;
}
