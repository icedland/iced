// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.VectorLength;
import com.github.icedland.iced.x86.internal.dec.StateFlags;

final class OpCodeHandler_VectorLength_EVEX extends OpCodeHandlerModRM {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_VectorLength_EVEX(OpCodeHandler handler128, OpCodeHandler handler256, OpCodeHandler handler512) {
		if (handler128 == null)
			throw new NullPointerException();
		if (handler256 == null)
			throw new NullPointerException();
		if (handler512 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler128,
			handler256,
			handler512,
			OpCodeHandler_Invalid.Instance,
		};
		assert handler128.hasModRM == hasModRM;
		assert handler256.hasModRM == hasModRM;
		assert handler512.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		handlers[decoder.state_vectorLength].decode(decoder, instruction);
	}
}

final class OpCodeHandler_VectorLength_EVEX_er extends OpCodeHandlerModRM {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_VectorLength_EVEX_er(OpCodeHandler handler128, OpCodeHandler handler256, OpCodeHandler handler512) {
		if (handler128 == null)
			throw new NullPointerException();
		if (handler256 == null)
			throw new NullPointerException();
		if (handler512 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler128,
			handler256,
			handler512,
			OpCodeHandler_Invalid.Instance,
		};
		assert handler128.hasModRM == hasModRM;
		assert handler256.hasModRM == hasModRM;
		assert handler512.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int index = decoder.state_vectorLength;
		if (decoder.state_mod == 3 && (decoder.state_zs_flags & StateFlags.B) != 0)
			index = VectorLength.L512;
		handlers[index].decode(decoder, instruction);
	}
}

final class OpCodeHandler_EVEX_V_H_Ev_er extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int codeW0;
	private final int codeW1;
	private final int tupleTypeW0;
	private final int tupleTypeW1;

	OpCodeHandler_EVEX_V_H_Ev_er(int baseReg, int codeW0, int codeW1, int tupleTypeW0, int tupleTypeW1) {
		this.baseReg = baseReg;
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
		this.tupleTypeW0 = tupleTypeW0;
		this.tupleTypeW1 = tupleTypeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_aaa) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		int tupleType;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(codeW1);
			tupleType = tupleTypeW1;
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(codeW0);
			tupleType = tupleTypeW0;
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setRoundingControl(decoder.state_vectorLength + RoundingControl.ROUND_TO_NEAREST);
		}
		else {
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_V_H_Ev_Ib extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int codeW0;
	private final int codeW1;
	private final int tupleTypeW0;
	private final int tupleTypeW1;

	OpCodeHandler_EVEX_V_H_Ev_Ib(int baseReg, int codeW0, int codeW1, int tupleTypeW0, int tupleTypeW1) {
		this.baseReg = baseReg;
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
		this.tupleTypeW0 = tupleTypeW0;
		this.tupleTypeW1 = tupleTypeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_aaa) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(codeW1);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(codeW0);
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0)
				decoder.readOpMem(instruction, tupleTypeW1);
			else
				decoder.readOpMem(instruction, tupleTypeW0);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_Ed_V_Ib extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;
	private final int tupleType32;
	private final int tupleType64;

	OpCodeHandler_EVEX_Ed_V_Ib(int baseReg, int code32, int code64, int tupleType32, int tupleType64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
		this.tupleType32 = tupleType32;
		this.tupleType64 = tupleType64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
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
			if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0)
				decoder.readOpMem(instruction, tupleType64);
			else
				decoder.readOpMem(instruction, tupleType32);
		}
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_VkHW_er extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;
	private final boolean onlySAE;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkHW_er(int baseReg, int code, int tupleType, boolean onlySAE, boolean canBroadcast) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.onlySAE = onlySAE;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (onlySAE)
					instruction.setSuppressAllExceptions(true);
				else {
					instruction.setRoundingControl(decoder.state_vectorLength + RoundingControl.ROUND_TO_NEAREST);
				}
			}
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkHW_er_ur extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkHW_er_ur(int baseReg, int code, int tupleType, boolean canBroadcast) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		int regNum0 = (decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX);
		instruction.setOp0Register(regNum0 + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			int regNum2 = (decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX);
			instruction.setOp2Register(regNum2 + baseReg);
			if (decoder.invalidCheckMask != 0 && (regNum0 == decoder.state_vvvv || regNum0 == regNum2))
				decoder.setInvalidInstruction();
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				instruction.setRoundingControl(decoder.state_vectorLength + RoundingControl.ROUND_TO_NEAREST);
			}
		}
		else {
			if (decoder.invalidCheckMask != 0 && regNum0 == decoder.state_vvvv)
				decoder.setInvalidInstruction();
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkW_er extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;
	private final boolean onlySAE;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkW_er(int baseReg, int code, int tupleType, boolean onlySAE) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.onlySAE = onlySAE;
		canBroadcast = true;
	}

	OpCodeHandler_EVEX_VkW_er(int baseReg1, int baseReg2, int code, int tupleType, boolean onlySAE) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
		this.tupleType = tupleType;
		this.onlySAE = onlySAE;
		canBroadcast = true;
	}

	OpCodeHandler_EVEX_VkW_er(int baseReg1, int baseReg2, int code, int tupleType, boolean onlySAE, boolean canBroadcast) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
		this.tupleType = tupleType;
		this.onlySAE = onlySAE;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (onlySAE)
					instruction.setSuppressAllExceptions(true);
				else {
					instruction.setRoundingControl(decoder.state_vectorLength + RoundingControl.ROUND_TO_NEAREST);
				}
			}
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkWIb_er extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VkWIb_er(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setSuppressAllExceptions(true);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setBroadcast(true);
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_VkW extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkW(int baseReg, int code, int tupleType, boolean canBroadcast) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	OpCodeHandler_EVEX_VkW(int baseReg1, int baseReg2, int code, int tupleType, boolean canBroadcast) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_WkV extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;
	private final int disallowZeroingMasking;

	OpCodeHandler_EVEX_WkV(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		disallowZeroingMasking = 0;
	}

	OpCodeHandler_EVEX_WkV(int baseReg, int code, int tupleType, boolean allowZeroingMasking) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		disallowZeroingMasking = allowZeroingMasking ? 0 : 0xFFFF_FFFF;
	}

	OpCodeHandler_EVEX_WkV(int baseReg1, int baseReg2, int code, int tupleType) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
		this.tupleType = tupleType;
		disallowZeroingMasking = 0;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.B) | decoder.state_vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg2);
		if (((decoder.state_zs_flags & StateFlags.Z) & disallowZeroingMasking & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg1);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			if (((decoder.state_zs_flags & StateFlags.Z) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkM extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VkM(int baseReg, int code, int tupleType) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.B) | decoder.state_vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkWIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkWIb(int baseReg, int code, int tupleType, boolean canBroadcast) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_WkVIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_WkVIb(int baseReg1, int baseReg2, int code, int tupleType) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.B) | decoder.state_vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg1);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			if (((decoder.state_zs_flags & StateFlags.Z) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg2);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_HkWIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_HkWIb(int baseReg, int code, int tupleType, boolean canBroadcast) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_vvvv + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_HWIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_HWIb(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if ((((decoder.state_zs_flags & (StateFlags.Z | StateFlags.B)) | decoder.state_aaa) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();

		instruction.setOp0Register(decoder.state_vvvv + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_WkVIb_er extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_WkVIb_er(int baseReg1, int baseReg2, int code, int tupleType) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg1);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setSuppressAllExceptions(true);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			if (((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg2);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_VW_er extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VW_er(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_vvvv_invalidCheck | decoder.state_aaa) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setSuppressAllExceptions(true);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VW extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VW(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.Z | StateFlags.B)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_WV extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_WV(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.Z | StateFlags.B)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg2);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VM extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VM(int baseReg, int code, int tupleType) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.Z | StateFlags.B)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VK extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_EVEX_VK(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.K0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_EVEX_KR extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_EVEX_KR(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa
				| decoder.state_zs_extraRegisterBase | decoder.state_extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_EVEX_KkHWIb_sae extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_KkHWIb_sae(int baseReg, int code, int tupleType, boolean canBroadcast) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_zs_extraRegisterBase | decoder.state_extraRegisterBaseEVEX)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setSuppressAllExceptions(true);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_VkHW extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int baseReg3;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkHW(int baseReg, int code, int tupleType, boolean canBroadcast) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	OpCodeHandler_EVEX_VkHW(int baseReg1, int baseReg2, int baseReg3, int code, int tupleType, boolean canBroadcast) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.baseReg3 = baseReg3;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		instruction.setOp1Register(decoder.state_vvvv + baseReg2);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg3);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkHM extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VkHM(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		instruction.setOp1Register(decoder.state_vvvv + baseReg2);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkHWIb extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int baseReg3;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkHWIb(int baseReg, int code, int tupleType, boolean canBroadcast) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	OpCodeHandler_EVEX_VkHWIb(int baseReg1, int baseReg2, int baseReg3, int code, int tupleType, boolean canBroadcast) {
		this.baseReg1 = baseReg1;
		this.baseReg2 = baseReg2;
		this.baseReg3 = baseReg3;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		instruction.setOp1Register(decoder.state_vvvv + baseReg2);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg3);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_VkHWIb_er extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int baseReg3;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_VkHWIb_er(int baseReg, int code, int tupleType, boolean canBroadcast) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		instruction.setOp1Register(decoder.state_vvvv + baseReg2);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg3);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setSuppressAllExceptions(true);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_KkHW extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_KkHW(int baseReg, int code, int tupleType, boolean canBroadcast) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_zs_extraRegisterBase | decoder.state_extraRegisterBaseEVEX)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_KP1HW extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_KP1HW(int baseReg, int code, int tupleType) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_aaa | decoder.state_zs_extraRegisterBase | decoder.state_extraRegisterBaseEVEX)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0)
				instruction.setBroadcast(true);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_KkHWIb extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_KkHWIb(int baseReg, int code, int tupleType, boolean canBroadcast) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_zs_extraRegisterBase | decoder.state_extraRegisterBaseEVEX)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_WkHV extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;

	OpCodeHandler_EVEX_WkHV(int baseReg, int code) {
		this.baseReg = baseReg;
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		assert decoder.state_mod == 3 : decoder.state_mod;
		instruction.setOp0Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
		if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		instruction.setOp2Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
	}
}

final class OpCodeHandler_EVEX_VHWIb extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VHWIb(int baseReg, int code, int tupleType) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_aaa) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_VHW extends OpCodeHandlerModRM {
	private final int baseReg1;
	private final int baseReg2;
	private final int baseReg3;
	private final int codeR;
	private final int codeM;
	private final int tupleType;

	OpCodeHandler_EVEX_VHW(int baseReg, int codeR, int codeM, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		this.codeR = codeR;
		this.codeM = codeM;
		this.tupleType = tupleType;
	}

	OpCodeHandler_EVEX_VHW(int baseReg, int code, int tupleType) {
		baseReg1 = baseReg;
		baseReg2 = baseReg;
		baseReg3 = baseReg;
		codeR = code;
		codeM = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_aaa) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg1);
		instruction.setOp1Register(decoder.state_vvvv + baseReg2);
		if (decoder.state_mod == 3) {
			instruction.setCode(codeR);
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg3);
		}
		else {
			instruction.setCode(codeM);
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VHM extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VHM(int baseReg, int code, int tupleType) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_aaa) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		instruction.setOp1Register(decoder.state_vvvv + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_Gv_W_er extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int codeW0;
	private final int codeW1;
	private final int tupleType;
	private final boolean onlySAE;

	OpCodeHandler_EVEX_Gv_W_er(int baseReg, int codeW0, int codeW1, int tupleType, boolean onlySAE) {
		this.baseReg = baseReg;
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
		this.tupleType = tupleType;
		this.onlySAE = onlySAE;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_vvvv_invalidCheck | decoder.state_aaa | decoder.state_extraRegisterBaseEVEX)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(codeW1);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(codeW0);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (onlySAE)
					instruction.setSuppressAllExceptions(true);
				else {
					instruction.setRoundingControl(decoder.state_vectorLength + RoundingControl.ROUND_TO_NEAREST);
				}
			}
		}
		else {
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VX_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int tupleTypeW0;
	private final int tupleTypeW1;

	OpCodeHandler_EVEX_VX_Ev(int code32, int code64, int tupleTypeW0, int tupleTypeW1) {
		this.code32 = code32;
		this.code64 = code64;
		this.tupleTypeW0 = tupleTypeW0;
		this.tupleTypeW1 = tupleTypeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		int tupleType;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			tupleType = tupleTypeW1;
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			tupleType = tupleTypeW0;
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_Ev_VX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int tupleTypeW0;
	private final int tupleTypeW1;

	OpCodeHandler_EVEX_Ev_VX(int code32, int code64, int tupleTypeW0, int tupleTypeW1) {
		this.code32 = code32;
		this.code64 = code64;
		this.tupleTypeW0 = tupleTypeW0;
		this.tupleTypeW1 = tupleTypeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.XMM0);
		int gpr;
		int tupleType;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			instruction.setCode(code64);
			tupleType = tupleTypeW1;
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			tupleType = tupleTypeW0;
			gpr = Register.EAX;
		}
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_Ev_VX_Ib extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;

	OpCodeHandler_EVEX_Ev_VX_Ib(int baseReg, int code32, int code64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa
				| decoder.state_extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
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
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + gpr);
		assert decoder.state_mod == 3 : decoder.state_mod;
		instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_MV extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_MV(int baseReg, int code, int tupleType) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VkEv_REXW extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;

	OpCodeHandler_EVEX_VkEv_REXW(int baseReg, int code32) {
		this.baseReg = baseReg;
		this.code32 = code32;
		code64 = Code.INVALID;
	}

	OpCodeHandler_EVEX_VkEv_REXW(int baseReg, int code32, int code64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.B) | decoder.state_vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int gpr;
		if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0) {
			assert code64 != Code.INVALID : code64;
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_EVEX_Vk_VSIB extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int vsibBase;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_Vk_VSIB(int baseReg, int vsibBase, int code, int tupleType) {
		this.baseReg = baseReg;
		this.vsibBase = vsibBase;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0
				&& (((decoder.state_zs_flags & (StateFlags.Z | StateFlags.B)) | (decoder.state_vvvv_invalidCheck & 0xF)) != 0
						|| decoder.state_aaa == 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		int regNum = (decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX);
		instruction.setOp0Register(regNum + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem_VSIB(instruction, vsibBase, tupleType);
			if (decoder.invalidCheckMask != 0) {
				if (regNum == Integer.remainderUnsigned(instruction.getMemoryIndex() - Register.XMM0, IcedConstants.VMM_COUNT))
					decoder.setInvalidInstruction();
			}
		}
	}
}

final class OpCodeHandler_EVEX_VSIB_k1_VX extends OpCodeHandlerModRM {
	private final int vsibIndex;
	private final int baseReg;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VSIB_k1_VX(int vsibIndex, int baseReg, int code, int tupleType) {
		this.vsibIndex = vsibIndex;
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0
				&& (((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | (decoder.state_vvvv_invalidCheck & 0xF)) != 0
						|| decoder.state_aaa == 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem_VSIB(instruction, vsibIndex, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_VSIB_k1 extends OpCodeHandlerModRM {
	private final int vsibIndex;
	private final int code;
	private final int tupleType;

	OpCodeHandler_EVEX_VSIB_k1(int vsibIndex, int code, int tupleType) {
		this.vsibIndex = vsibIndex;
		this.code = code;
		this.tupleType = tupleType;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0
				&& (((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | (decoder.state_vvvv_invalidCheck & 0xF)) != 0
						|| decoder.state_aaa == 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem_VSIB(instruction, vsibIndex, tupleType);
		}
	}
}

final class OpCodeHandler_EVEX_GvM_VX_Ib extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code32;
	private final int code64;
	private final int tupleType32;
	private final int tupleType64;

	OpCodeHandler_EVEX_GvM_VX_Ib(int baseReg, int code32, int code64, int tupleType32, int tupleType64) {
		this.baseReg = baseReg;
		this.code32 = code32;
		this.code64 = code64;
		this.tupleType32 = tupleType32;
		this.tupleType64 = tupleType64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & (StateFlags.B | StateFlags.Z)) | decoder.state_vvvv_invalidCheck | decoder.state_aaa)
				& decoder.invalidCheckMask) != 0)
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
			if ((decoder.state_zs_flags & decoder.is64bMode_and_W) != 0)
				decoder.readOpMem(instruction, tupleType64);
			else
				decoder.readOpMem(instruction, tupleType32);
		}
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + baseReg);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_EVEX_KkWIb extends OpCodeHandlerModRM {
	private final int baseReg;
	private final int code;
	private final int tupleType;
	private final boolean canBroadcast;

	OpCodeHandler_EVEX_KkWIb(int baseReg, int code, int tupleType, boolean canBroadcast) {
		this.baseReg = baseReg;
		this.code = code;
		this.tupleType = tupleType;
		this.canBroadcast = canBroadcast;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((((decoder.state_zs_flags & StateFlags.Z) | decoder.state_vvvv_invalidCheck | decoder.state_zs_extraRegisterBase
				| decoder.state_extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + baseReg);
			if (((decoder.state_zs_flags & StateFlags.B) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.B) != 0) {
				if (canBroadcast)
					instruction.setBroadcast(true);
				else if (decoder.invalidCheckMask != 0)
					decoder.setInvalidInstruction();
			}
			decoder.readOpMem(instruction, tupleType);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}
