// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import com.github.icedland.iced.x86.enc.BlockEncoderResult;

/**
 * Result of {@link CodeAssembler#assemble(com.github.icedland.iced.x86.CodeWriter, long)}.
 */
public final class CodeAssemblerResult {
	CodeAssemblerResult(BlockEncoderResult[] result) {
		this.result = result;
	}

	/**
	 * The associated block encoder result.
	 */
	public final BlockEncoderResult[] result;

	/**
	 * Gets the RIP of the specified label.
	 *
	 * @param label A label.
	 */
	public long getLabelRIP(CodeLabel label) {
		return getLabelRIP(label, 0);
	}

	/**
	 * Gets the RIP of the specified label.
	 *
	 * @param label A label.
	 * @param index Result index
	 */
	public long getLabelRIP(CodeLabel label, int index) {
		if (label == null)
			throw new IllegalArgumentException("Invalid label. Must be created via CodeAssembler.createLabel()");
		if (label.getInstructionIndex() < 0)
			throw new IllegalArgumentException(
					"The label is not associated with an instruction index. It must be emitted via CodeAssembler.label().");
		if (this.result == null || Integer.compareUnsigned(index, this.result.length) >= 0)
			throw new IllegalArgumentException("index");
		BlockEncoderResult result = this.result[index];
		if (result.newInstructionOffsets == null || Integer.compareUnsigned(label.getInstructionIndex(), result.newInstructionOffsets.length) >= 0)
			throw new IllegalArgumentException(String.format(
					"The label instruction index %d is out of range of the instruction offsets results %d. Did you forget to pass BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS to CodeAssembler.assemble()?",
					label.getInstructionIndex(), result.newInstructionOffsets == null ? 0 : result.newInstructionOffsets.length));
		return result.rip + result.newInstructionOffsets[label.getInstructionIndex()];
	}
}
