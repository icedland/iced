// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

/**
 * A label that can be created by {@link CodeAssembler#createLabel()}.
 */
public final class CodeLabel {
	CodeLabel(String name, long id) {
		if (name == null)
			name = "___label";
		this.name = name;
		this.id = id;
		this.instructionIndex = -1;
	}

	/**
	 * Name of this label.
	 */
	public final String name;

	/**
	 * Id of this label.
	 */
	public final long id;

	int instructionIndex;

	/**
	 * Gets the instruction index associated with this label. This is setup after calling {@link CodeAssembler#label(CodeLabel)}
	 */
	public int getInstructionIndex() {
		return instructionIndex;
	}

	void setInstructionIndex(int value) {
		instructionIndex = value;
	}

	@Override
	public String toString() {
		return String.format("%s@%d", name, id);
	}

	@Override
	public boolean equals(Object obj) {
		if (obj == null || getClass() != obj.getClass())
			return false;
		CodeLabel other = (CodeLabel)obj;
		return name == other.name && id == other.id;
	}

	@Override
	public int hashCode() {
		return name.hashCode() * 397 + Long.hashCode(id);
	}
}
