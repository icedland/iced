// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.TupleType;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.dec.OpSize;
import com.github.icedland.iced.x86.internal.dec.StateFlags;

final class OpCodeHandler_VectorLength_VEX extends OpCodeHandlerModRM {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_VectorLength_VEX(OpCodeHandler handler128, OpCodeHandler handler256) {
		if (handler128 == null)
			throw new NullPointerException();
		if (handler256 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler128,
			handler256,
			OpCodeHandler_Invalid.Instance,
			OpCodeHandler_Invalid.Instance,
		};
		assert handler128.hasModRM == hasModRM;
		assert handler256.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		handlers[decoder.state_vectorLength].decode(decoder, instruction);
	}
}

final class OpCodeHandler_VectorLength_NoModRM_VEX extends OpCodeHandler {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_VectorLength_NoModRM_VEX(OpCodeHandler handler128, OpCodeHandler handler256) {
		if (handler128 == null)
			throw new NullPointerException();
		if (handler256 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler128,
			handler256,
			OpCodeHandler_Invalid.Instance,
			OpCodeHandler_Invalid.Instance,
		};
		assert handler128.hasModRM == hasModRM;
		assert handler256.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		handlers[decoder.state_vectorLength].decode(decoder, instruction);
	}
}

final class OpCodeHandler_VEX_Simple extends OpCodeHandler {
	private final int code;

	OpCodeHandler_VEX_Simple(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
	}
}

final class OpCodeHandler_VEX_VHEv extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_VEX_VHEv(int baseReg, int codeW0, int codeW1) {
		this.baseReg = baseReg;
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(codeW1);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(codeW0);
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_VHEvIb extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_VEX_VHEvIb(int baseReg, int codeW0, int codeW1) {
		this.baseReg = baseReg;
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(codeW1);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(codeW0);
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_VW extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;

	OpCodeHandler_VEX_VW(int baseReg, int code) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
	}

	OpCodeHandler_VEX_VW(int baseReg1, int baseReg2, int code) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg2);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_VX_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_VX_Ev(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_Ev_VX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Ev_VX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_WV extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;

	OpCodeHandler_VEX_WV(int baseReg, int code) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg2);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_VM extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_VM(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_MV extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_MV(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_M extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_M(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_RdRq extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_RdRq(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
		}
		if (decoder.state_mod != 3)
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_rDI_VX_RX extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_rDI_VX_RX(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_RDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_EDI);
		else
			instruction.setOp0Kind(OpKind.MEMORY_SEG_DI);
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_VWIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_VEX_VWIb(int baseReg, int code) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		codeW0 = code;
		codeW1 = code;
	}

	OpCodeHandler_VEX_VWIb(int baseReg, int codeW0, int codeW1) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0)
			instruction.setCode(codeW1);
		else
			instruction.setCode(codeW0);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg2);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_WVIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;

	OpCodeHandler_VEX_WVIb(int baseReg1, int baseReg2, int code) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg1);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg2);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_Ed_V_Ib extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Ed_V_Ib(int baseReg, int code32, int code64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_VHW extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int baseReg3;
	private final int codeR;
	private final int codeM;

	OpCodeHandler_VEX_VHW(int baseReg, int codeR, int codeM) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		this.codeR = codeR;
		this.codeM = codeM;
	}

	OpCodeHandler_VEX_VHW(int baseReg, int code) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		codeR = code;
		codeM = code;
	}

	OpCodeHandler_VEX_VHW(int baseReg1, int baseReg2, int baseReg3, int code) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.baseReg3 = baseReg3;
		codeR = code;
		codeM = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg1);
		instruction.setOp1Register(decoder.state_vvvv + baseReg2);
		if (decoder.state_mod == 3) {
			instruction.setCode(codeR);
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg3);
		}
		else {
			instruction.setCode(codeM);
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_VWH extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_VWH(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp2Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_WHV extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int codeR;

	OpCodeHandler_VEX_WHV(int baseReg, int code) {
		this.baseReg = baseReg;
		codeR = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		assert decoder.state_mod == 3 : decoder.state_mod;
		instruction.setCode(codeR);
		instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		instruction.setOp2Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
	}
}

final class OpCodeHandler_VEX_VHM extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_VHM(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_MHV extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_MHV(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		instruction.setOp2Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_VHWIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int baseReg3;
	private final int code;

	OpCodeHandler_VEX_VHWIb(int baseReg, int code) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		this.code = code;
	}

	OpCodeHandler_VEX_VHWIb(int baseReg1, int baseReg2, int baseReg3, int code) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.baseReg3 = baseReg3;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg1);
		instruction.setOp1Register(decoder.state_vvvv + baseReg2);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg3);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_HRIb extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_HRIb(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_VHWIs4 extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_VHWIs4(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp3Register(((decoder.readByte() >>> 4) & decoder.reg15Mask) + baseReg);
	}
}

final class OpCodeHandler_VEX_VHIs4W extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_VHIs4W(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp3Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp3Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Register(((decoder.readByte() >>> 4) & decoder.reg15Mask) + baseReg);
	}
}

final class OpCodeHandler_VEX_VHWIs5 extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_VHWIs5(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		int ib = decoder.readByte();
		instruction.setOp3Register(((ib >>> 4) & decoder.reg15Mask) + baseReg);
		assert instruction.getOp4Kind() == OpKind.IMMEDIATE8 : instruction.getOp4Kind();// It's hard coded
		instruction.setImmediate8((byte)(ib & 0xF));
	}
}

final class OpCodeHandler_VEX_VHIs5W extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_VEX_VHIs5W(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp3Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp3Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		int ib = decoder.readByte();
		instruction.setOp2Register(((ib >>> 4) & decoder.reg15Mask) + baseReg);
		assert instruction.getOp4Kind() == OpKind.IMMEDIATE8 : instruction.getOp4Kind();// It's hard coded
		instruction.setImmediate8((byte)(ib & 0xF));
	}
}

final class OpCodeHandler_VEX_VK_HK_RK extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_VK_HK_RK(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0 && (decoder.state_vvvv > 7 || decoder.state_zs_extraRegisterBase != 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.K0);
		instruction.setOp1Register((decoder.state_vvvv & 7) + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + Register.K0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_VK_RK extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_VK_RK(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.K0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_VK_RK_Ib extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_VK_RK_Ib(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.K0);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_VK_WK extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_VK_WK(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.K0);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_M_VK extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_M_VK(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_VK_R extends OpCodeHandlerModRM {
	private final int code;
	private final int gpr;

	OpCodeHandler_VEX_VK_R(int code, int gpr) {
		this.code = code;
		this.gpr = gpr;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_G_VK extends OpCodeHandlerModRM {
	private final int code;
	private final int gpr;

	OpCodeHandler_VEX_G_VK(int code, int gpr) {
		this.code = code;
		this.gpr = gpr;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.K0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_Gv_W extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_VEX_Gv_W(int baseReg, int codeW0, int codeW1) {
		this.baseReg = baseReg;
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(codeW1);
			instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + Register.RAX);
		}
		else {
			instruction.setCode(codeW0);
			instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + Register.EAX);
		}
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + baseReg);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_Gv_RX extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Gv_RX(int baseReg, int code32, int code64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + baseReg);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_Gv_GPR_Ib extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Gv_GPR_Ib(int baseReg, int code32, int code64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + baseReg);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_VX_VSIB_HX extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int vsibIndex;
	private final int baseReg3;
	private final int code;

	OpCodeHandler_VEX_VX_VSIB_HX(int baseReg1, int vsibIndex, int baseReg3, int code) {
		this.baseReg1 = baseReg1;
		this.vsibIndex = vsibIndex;
		this.baseReg3 = baseReg3;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		int regNum = (decoder.state_reg + decoder.state_zs_extraRegisterBase);
		instruction.setOp0Register(regNum + baseReg1);
		instruction.setOp2Register(decoder.state_vvvv + baseReg3);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem_VSIB(instruction, vsibIndex, TupleType.N1);
			if (decoder.invalidCheckMask != 0) {
				int indexNum = Integer.remainderUnsigned(instruction.getMemoryIndex() - Register.XMM0, IcedConstants.VMM_COUNT);
				if (regNum == indexNum || decoder.state_vvvv == indexNum || regNum == decoder.state_vvvv)
					decoder.setInvalidInstruction();
			}
		}
	}
}

final class OpCodeHandler_VEX_Gv_Gv_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Gv_Gv_Ev(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		instruction.setOp1Register(decoder.state_vvvv + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_Gv_Ev_Gv extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Gv_Ev_Gv(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		instruction.setOp2Register(decoder.state_vvvv + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_Ev_Gv_Gv extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Ev_Gv_Gv(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp1Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		instruction.setOp2Register(decoder.state_vvvv + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_Hv_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Hv_Ev(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_vvvv + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_Hv_Ed_Id extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Hv_Ed_Id(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			instruction.setOp0Register(decoder.state_vvvv + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_vvvv + Register.EAX);
		}
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + Register.EAX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE32);
		instruction.setImmediate32(decoder.readUInt32());
	}
}

final class OpCodeHandler_VEX_GvM_VX_Ib extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_GvM_VX_Ib(int baseReg, int code32, int code64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp1Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + baseReg);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_Gv_Ev_Ib extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Gv_Ev_Ib(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_Gv_Ev_Id extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Gv_Ev_Id(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE32);
		instruction.setImmediate32(decoder.readUInt32());
	}
}

final class OpCodeHandler_VEX_VT_SIBMEM extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_VT_SIBMEM(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.TMM0);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMemSib(instruction);
		}
	}
}

final class OpCodeHandler_VEX_SIBMEM_VT extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_SIBMEM_VT(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + Register.TMM0);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMemSib(instruction);
		}
	}
}

final class OpCodeHandler_VEX_VT extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_VT(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.TMM0);
	}
}

final class OpCodeHandler_VEX_VT_RT_HT extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_VT_RT_HT(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0 && (decoder.state_vvvv > 7 || decoder.state_zs_extraRegisterBase != 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.TMM0);
		instruction.setOp2Register((decoder.state_vvvv & 7) + Register.TMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.TMM0);
			if (decoder.invalidCheckMask != 0) {
				if (decoder.state_zs_extraBaseRegisterBase != 0 || decoder.state_reg == decoder.state_vvvv || decoder.state_reg == decoder.state_rm
						|| decoder.state_rm == decoder.state_vvvv)
					decoder.setInvalidInstruction();
			}
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_Gq_HK_RK extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_Gq_HK_RK(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0 && decoder.state_vvvv > 7)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + Register.RAX);
		instruction.setOp1Register((decoder.state_vvvv & 7) + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + Register.K0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VEX_VK_R_Ib extends OpCodeHandlerModRM {
	private final int code;
	private final int gpr;

	OpCodeHandler_VEX_VK_R_Ib(int code, int gpr) {
		this.code = code;
		this.gpr = gpr;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (((decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VEX_K_Jb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_K_Jb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.state_zs_flags |= StateFlags.BRANCH_IMM8;
		if (decoder.invalidCheckMask != 0 && decoder.state_vvvv > 7)
			decoder.setInvalidInstruction();
		instruction.setOp0Register((decoder.state_vvvv & 7) + Register.K0);
		assert decoder.is64bMode;
		instruction.setCode(code);
		instruction.setOp1Kind(OpKind.NEAR_BRANCH64);
		// The modrm byte has the imm8 value
		instruction.setNearBranch64((byte)decoder.state_modrm + decoder.getCurrentInstructionPointer64());
	}
}

final class OpCodeHandler_VEX_K_Jz extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VEX_K_Jz(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0 && decoder.state_vvvv > 7)
			decoder.setInvalidInstruction();
		instruction.setOp0Register((decoder.state_vvvv & 7) + Register.K0);
		assert decoder.is64bMode;
		instruction.setCode(code);
		instruction.setOp1Kind(OpKind.NEAR_BRANCH64);
		// The modrm byte has the low 8 bits of imm32
		int imm = decoder.state_modrm | (decoder.readByte() << 8) | (decoder.readByte() << 16) | (decoder.readByte() << 24);
		instruction.setNearBranch64(imm + decoder.getCurrentInstructionPointer64());
	}
}

final class OpCodeHandler_VEX_Gv_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Gv_Ev(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register((decoder.state_reg + decoder.state_zs_extraRegisterBase) + gpr);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VEX_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VEX_Ev(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((decoder.state_rm + decoder.state_zs_extraBaseRegisterBase) + gpr);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}
