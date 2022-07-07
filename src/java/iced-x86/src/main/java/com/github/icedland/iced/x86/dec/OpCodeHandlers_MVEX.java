// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MvexRegMemConv;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.dec.StateFlags;

final class OpCodeHandler_EH extends OpCodeHandlerModRM {
	private final OpCodeHandler handlerEH0;
	private final OpCodeHandler handlerEH1;

	OpCodeHandler_EH(OpCodeHandler handlerEH0, OpCodeHandler handlerEH1) {
		if (handlerEH0 == null)
			throw new NullPointerException();
		if (handlerEH1 == null)
			throw new NullPointerException();
		this.handlerEH0 = handlerEH0;
		this.handlerEH1 = handlerEH1;
		assert handlerEH0.hasModRM == hasModRM;
		assert handlerEH1.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0 ? handlerEH1 : handlerEH0).decode(decoder, instruction);
	}
}

final class OpCodeHandler_MVEX_M extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_M(int code) {
		assert MvexInfo.getIgnoresOpMaskRegister(code);
		assert !MvexInfo.canUseEvictionHint(code);
		assert MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setOpMask(Register.NONE); // It's ignored (see ctor)
		instruction.setCode(code);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_MV extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_MV(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			if (MvexInfo.canUseEvictionHint(code) && (decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_VW extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_VW(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_HWIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_HWIb(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_vvvv + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_MVEX_VWIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_VWIb(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_MVEX_VHW extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_VHW(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.ZMM0);
		instruction.setOp1Register(decoder.state_vvvv + Register.ZMM0);
		if (MvexInfo.getRequireOpMaskRegister(code) && decoder.invalidCheckMask != 0 && decoder.state_aaa == 0)
			decoder.setInvalidInstruction();
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_VHWIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_VHWIb(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.ZMM0);
		instruction.setOp1Register(decoder.state_vvvv + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_MVEX_VKW extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_VKW(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_vvvv & decoder.invalidCheckMask) > 7)
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.ZMM0);
		instruction.setOp1Register((decoder.state_vvvv & 7) + Register.K0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_KHW extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_KHW(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		instruction.setOp1Register(decoder.state_vvvv + Register.ZMM0);
		if (((decoder.state_zs_extraRegisterBase | decoder.state_extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_KHWIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_KHWIb(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);

		instruction.setOp0Register(decoder.state_reg + Register.K0);
		instruction.setOp1Register(decoder.state_vvvv + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_extraBaseRegisterBaseEVEX + Register.ZMM0);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0) {
				if (MvexInfo.canUseSuppressAllExceptions(code)) {
					if ((sss & 4) != 0)
						instruction.setSuppressAllExceptions(true);
					if (MvexInfo.canUseRoundingControl(code)) {
						instruction.setRoundingControl((sss & 3) + RoundingControl.ROUND_TO_NEAREST);
					}
				}
				else if (MvexInfo.getNoSaeRc(code) && (sss & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
			}
			else {
				if ((MvexInfo.getInvalidSwizzleFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
					decoder.setInvalidInstruction();
				assert Integer.compareUnsigned(sss, 7) <= 0 : sss;
				instruction.setMvexRegMemConv(MvexRegMemConv.REG_SWIZZLE_NONE + sss);
			}
		}
		else {
			instruction.setOp2Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem(instruction, MvexInfo.getTupleType(code, sss));
		}
		if (((decoder.state_zs_extraRegisterBase | decoder.state_extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setOp3Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_MVEX_VSIB extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_VSIB(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0 && ((decoder.state_vvvv_invalidCheck & 0xF) != 0 || decoder.state_aaa == 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		int sss = decoder.getSss();
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem_VSIB(instruction, Register.ZMM0, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_VSIB_V extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_VSIB_V(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0 && ((decoder.state_vvvv_invalidCheck & 0xF) != 0 || decoder.state_aaa == 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem_VSIB(instruction, Register.ZMM0, MvexInfo.getTupleType(code, sss));
		}
	}
}

final class OpCodeHandler_MVEX_V_VSIB extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MVEX_V_VSIB(int code) {
		assert !MvexInfo.getIgnoresOpMaskRegister(code);
		assert MvexInfo.canUseEvictionHint(code);
		assert !MvexInfo.getIgnoresEvictionHint(code);
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.invalidCheckMask != 0 && ((decoder.state_vvvv_invalidCheck & 0xF) != 0 || decoder.state_aaa == 0))
			decoder.setInvalidInstruction();
		instruction.setCode(code);

		int regNum = (decoder.state_reg + decoder.state_zs_extraRegisterBase + decoder.state_extraRegisterBaseEVEX);
		instruction.setOp0Register(regNum + Register.ZMM0);
		int sss = decoder.getSss();
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			if ((decoder.state_zs_flags & StateFlags.MVEX_EH) != 0)
				instruction.setMvexEvictionHint(true);
			if ((MvexInfo.getInvalidConvFns(code) & (1 << sss) & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setMvexRegMemConv(MvexRegMemConv.MEM_CONV_NONE + sss);
			decoder.readOpMem_VSIB(instruction, Register.ZMM0, MvexInfo.getTupleType(code, sss));
			if (decoder.invalidCheckMask != 0) {
				if (regNum == Integer.remainderUnsigned(instruction.getMemoryIndex() - Register.XMM0, IcedConstants.VMM_COUNT))
					decoder.setInvalidInstruction();
			}
		}
	}
}
