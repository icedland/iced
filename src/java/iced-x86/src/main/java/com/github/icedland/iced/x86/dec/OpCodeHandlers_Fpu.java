// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.internal.dec.OpSize;

final class OpCodeHandler_ST_STi extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_ST_STi(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(Register.ST0);
		instruction.setOp1Register(Register.ST0 + decoder.state_rm);
	}
}

final class OpCodeHandler_STi_ST extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_STi_ST(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(Register.ST0 + decoder.state_rm);
		instruction.setOp1Register(Register.ST0);
	}
}

final class OpCodeHandler_STi extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_STi(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(Register.ST0 + decoder.state_rm);
	}
}

final class OpCodeHandler_Mf extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;

	OpCodeHandler_Mf(int code) {
		code16 = code;
		code32 = code;
	}

	OpCodeHandler_Mf(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize != OpSize.SIZE16)
			instruction.setCode(code32);
		else
			instruction.setCode(code16);
		assert decoder.state_mod != 3 : decoder.state_mod;
		instruction.setOp0Kind(OpKind.MEMORY);
		decoder.readOpMem(instruction);
	}
}
