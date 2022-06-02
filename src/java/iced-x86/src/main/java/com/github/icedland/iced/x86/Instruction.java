// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * TODO: docs here
 */
public final class Instruction {
	long nextRip;
	long memDispl;
	int flags1; // InstrFlags1
	int immediate;
	short code;
	byte memBaseReg; // Register
	byte memIndexReg; // Register
	byte reg0, reg1, reg2, reg3; // Register
	byte opKind0, opKind1, opKind2, opKind3; // OpKind
	byte scale;
	byte displSize;
	byte len;
	byte pad;
}
