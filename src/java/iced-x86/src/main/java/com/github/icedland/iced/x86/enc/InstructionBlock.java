// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.List;

import com.github.icedland.iced.x86.CodeWriter;
import com.github.icedland.iced.x86.Instruction;

/**
 * Contains a list of instructions that should be encoded by {@link BlockEncoder}
 */
public final class InstructionBlock {
	/**
	 * Code writer
	 */
	public final CodeWriter codeWriter;

	/**
	 * All instructions
	 */
	public final List<Instruction> instructions;

	/**
	 * Base IP of all encoded instructions
	 */
	public final long rip;

	/**
	 * Constructor
	 *
	 * @param codeWriter Code writer
	 * @param instructions Instructions
	 * @param rip Base IP of all encoded instructions
	 */
	public InstructionBlock(CodeWriter codeWriter, List<Instruction> instructions, long rip) {
		if (codeWriter == null)
			throw new NullPointerException("codeWriter");
		if (instructions == null)
			throw new NullPointerException("instructions");
		this.codeWriter = codeWriter;
		this.instructions = instructions;
		this.rip = rip;
	}
}
