// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.internal.MandatoryPrefixByte;
import com.github.icedland.iced.x86.internal.dec.HandlerFlags;
import com.github.icedland.iced.x86.internal.dec.LegacyHandlerFlags;
import com.github.icedland.iced.x86.internal.dec.OpSize;
import com.github.icedland.iced.x86.internal.dec.StateFlags;

final class OpCodeHandler_VEX2 extends OpCodeHandlerModRM {
	private final OpCodeHandler handlerMem;

	OpCodeHandler_VEX2(OpCodeHandler handlerMem) {
		if (handlerMem == null)
			throw new NullPointerException();
		this.handlerMem = handlerMem;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode)
			decoder.vex2(instruction);
		else if (decoder.state_mod == 3)
			decoder.vex2(instruction);
		else
			handlerMem.decode(decoder, instruction);
	}
}

final class OpCodeHandler_VEX3 extends OpCodeHandlerModRM {
	private final OpCodeHandler handlerMem;

	OpCodeHandler_VEX3(OpCodeHandler handlerMem) {
		if (handlerMem == null)
			throw new NullPointerException();
		this.handlerMem = handlerMem;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode)
			decoder.vex3(instruction);
		else if (decoder.state_mod == 3)
			decoder.vex3(instruction);
		else
			handlerMem.decode(decoder, instruction);
	}
}

final class OpCodeHandler_XOP extends OpCodeHandlerModRM {
	private final OpCodeHandler handler_reg0;

	OpCodeHandler_XOP(OpCodeHandler handler_reg0) {
		if (handler_reg0 == null)
			throw new NullPointerException();
		this.handler_reg0 = handler_reg0;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_modrm & 0x1F) < 8)
			handler_reg0.decode(decoder, instruction);
		else
			decoder.xop(instruction);
	}
}

final class OpCodeHandler_EVEX extends OpCodeHandlerModRM {
	private final OpCodeHandler handlerMem;

	OpCodeHandler_EVEX(OpCodeHandler handlerMem) {
		if (handlerMem == null)
			throw new NullPointerException();
		this.handlerMem = handlerMem;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode)
			decoder.evex_mvex(instruction);
		else if (decoder.state_mod == 3)
			decoder.evex_mvex(instruction);
		else
			handlerMem.decode(decoder, instruction);
	}
}

final class OpCodeHandler_PrefixEsCsSsDs extends OpCodeHandler {
	private final int seg;

	OpCodeHandler_PrefixEsCsSsDs(int seg) {
		assert seg == Register.ES || seg == Register.CS || seg == Register.SS || seg == Register.DS : seg;
		this.seg = seg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		if (!decoder.is64bMode || decoder.state_zs_segmentPrio <= 0)
			instruction.setSegmentPrefix(seg);

		decoder.resetRexPrefixState();
		decoder.callOpCodeHandlerXXTable(instruction);
	}
}

final class OpCodeHandler_PrefixFsGs extends OpCodeHandler {
	private final int seg;

	OpCodeHandler_PrefixFsGs(int seg) {
		assert seg == Register.FS || seg == Register.GS : seg;
		this.seg = seg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		instruction.setSegmentPrefix(seg);
		decoder.state_zs_segmentPrio = 1;

		decoder.resetRexPrefixState();
		decoder.callOpCodeHandlerXXTable(instruction);
	}
}

final class OpCodeHandler_Prefix66 extends OpCodeHandler {
	OpCodeHandler_Prefix66() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		decoder.state_zs_flags |= StateFlags.HAS66;
		decoder.state_operandSize = decoder.defaultInvertedOperandSize;
		if (decoder.state_zs_mandatoryPrefix == MandatoryPrefixByte.NONE)
			decoder.state_zs_mandatoryPrefix = MandatoryPrefixByte.P66;

		decoder.resetRexPrefixState();
		decoder.callOpCodeHandlerXXTable(instruction);
	}
}

final class OpCodeHandler_Prefix67 extends OpCodeHandler {
	OpCodeHandler_Prefix67() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		decoder.state_addressSize = decoder.defaultInvertedAddressSize;

		decoder.resetRexPrefixState();
		decoder.callOpCodeHandlerXXTable(instruction);
	}
}

final class OpCodeHandler_PrefixF0 extends OpCodeHandler {
	OpCodeHandler_PrefixF0() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		instruction.setLockPrefix(true);
		decoder.state_zs_flags |= StateFlags.LOCK;

		decoder.resetRexPrefixState();
		decoder.callOpCodeHandlerXXTable(instruction);
	}
}

final class OpCodeHandler_PrefixF2 extends OpCodeHandler {
	OpCodeHandler_PrefixF2() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		instruction.setRepePrefix(false);
		instruction.setRepnePrefix(true);
		decoder.state_zs_mandatoryPrefix = MandatoryPrefixByte.PF2;

		decoder.resetRexPrefixState();
		decoder.callOpCodeHandlerXXTable(instruction);
	}
}

final class OpCodeHandler_PrefixF3 extends OpCodeHandler {
	OpCodeHandler_PrefixF3() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		instruction.setRepePrefix(true);
		instruction.setRepnePrefix(false);
		decoder.state_zs_mandatoryPrefix = MandatoryPrefixByte.PF3;

		decoder.resetRexPrefixState();
		decoder.callOpCodeHandlerXXTable(instruction);
	}
}

final class OpCodeHandler_PrefixREX extends OpCodeHandler {
	private final OpCodeHandler handler;
	private final int rex;

	OpCodeHandler_PrefixREX(OpCodeHandler handler, int rex) {
		if (handler == null)
			throw new NullPointerException();
		assert rex <= 0x0F : rex;
		this.handler = handler;
		this.rex = rex;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		if (decoder.is64bMode) {
			if ((rex & 8) != 0) {
				decoder.state_operandSize = OpSize.SIZE64;
				decoder.state_zs_flags |= StateFlags.HAS_REX | StateFlags.W;
			}
			else {
				decoder.state_zs_flags |= StateFlags.HAS_REX;
				decoder.state_zs_flags &= ~StateFlags.W;
				if ((decoder.state_zs_flags & StateFlags.HAS66) == 0)
					decoder.state_operandSize = OpSize.SIZE32;
				else
					decoder.state_operandSize = OpSize.SIZE16;
			}
			decoder.state_zs_extraRegisterBase = (rex << 1) & 8;
			decoder.state_zs_extraIndexRegisterBase = (rex << 2) & 8;
			decoder.state_zs_extraBaseRegisterBase = (rex << 3) & 8;

			decoder.callOpCodeHandlerXXTable(instruction);
		}
		else
			handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_Reg extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_Reg(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(reg);
	}
}

final class OpCodeHandler_RegIb extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_RegIb(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(reg);
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_IbReg extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_IbReg(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(reg);
		instruction.setOp0Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_AL_DX extends OpCodeHandler {
	private final int code;

	OpCodeHandler_AL_DX(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(Register.AL);
		instruction.setOp1Register(Register.DX);
	}
}

final class OpCodeHandler_DX_AL extends OpCodeHandler {
	private final int code;

	OpCodeHandler_DX_AL(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(Register.DX);
		instruction.setOp1Register(Register.AL);
	}
}

final class OpCodeHandler_Ib extends OpCodeHandler {
	private final int code;

	OpCodeHandler_Ib(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Ib3 extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Ib3(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_MandatoryPrefix extends OpCodeHandlerModRM {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_MandatoryPrefix(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
		if (handler == null)
			throw new NullPointerException();
		if (handler66 == null)
			throw new NullPointerException();
		if (handlerF3 == null)
			throw new NullPointerException();
		if (handlerF2 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler,
			handler66,
			handlerF3,
			handlerF2,
		};
		assert handler.hasModRM == hasModRM;
		assert handler66.hasModRM == hasModRM;
		assert handlerF3.hasModRM == hasModRM;
		assert handlerF2.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.clearMandatoryPrefix(instruction);
		handlers[decoder.state_zs_mandatoryPrefix].decode(decoder, instruction);
	}
}

final class OpCodeHandler_MandatoryPrefix3 extends OpCodeHandlerModRM {
	private final Info[] handlers_reg;
	private final Info[] handlers_mem;

	private static final class Info {
		final OpCodeHandler handler;
		final boolean mandatoryPrefix;

		Info(OpCodeHandler handler, boolean mandatoryPrefix) {
			this.handler = handler;
			this.mandatoryPrefix = mandatoryPrefix;
		}
	}

	OpCodeHandler_MandatoryPrefix3(OpCodeHandler handler_reg, OpCodeHandler handler_mem, OpCodeHandler handler66_reg, OpCodeHandler handler66_mem,
			OpCodeHandler handlerF3_reg, OpCodeHandler handlerF3_mem, OpCodeHandler handlerF2_reg, OpCodeHandler handlerF2_mem, int flags) {
		if (handler_reg == null)
			throw new NullPointerException();
		if (handler66_reg == null)
			throw new NullPointerException();
		if (handlerF3_reg == null)
			throw new NullPointerException();
		if (handlerF2_reg == null)
			throw new NullPointerException();
		if (handler_mem == null)
			throw new NullPointerException();
		if (handler66_mem == null)
			throw new NullPointerException();
		if (handlerF3_mem == null)
			throw new NullPointerException();
		if (handlerF2_mem == null)
			throw new NullPointerException();
		handlers_reg = new Info[] {
			new Info(handler_reg, (flags & LegacyHandlerFlags.HANDLER_REG) == 0),
			new Info(handler66_reg, (flags & LegacyHandlerFlags.HANDLER_66_REG) == 0),
			new Info(handlerF3_reg, (flags & LegacyHandlerFlags.HANDLER_F3_REG) == 0),
			new Info(handlerF2_reg, (flags & LegacyHandlerFlags.HANDLER_F2_REG) == 0),
		};
		handlers_mem = new Info[] {
			new Info(handler_mem, (flags & LegacyHandlerFlags.HANDLER_MEM) == 0),
			new Info(handler66_mem, (flags & LegacyHandlerFlags.HANDLER_66_MEM) == 0),
			new Info(handlerF3_mem, (flags & LegacyHandlerFlags.HANDLER_F3_MEM) == 0),
			new Info(handlerF2_mem, (flags & LegacyHandlerFlags.HANDLER_F2_MEM) == 0),
		};
		assert handler_reg.hasModRM == hasModRM;
		assert handler_mem.hasModRM == hasModRM;
		assert handler66_reg.hasModRM == hasModRM;
		assert handler66_mem.hasModRM == hasModRM;
		assert handlerF3_reg.hasModRM == hasModRM;
		assert handlerF3_mem.hasModRM == hasModRM;
		assert handlerF2_reg.hasModRM == hasModRM;
		assert handlerF2_mem.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		Info[] handlers = decoder.state_mod == 3 ? handlers_reg : handlers_mem;
		Info info = handlers[decoder.state_zs_mandatoryPrefix];
		if (info.mandatoryPrefix)
			decoder.clearMandatoryPrefix(instruction);
		info.handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_MandatoryPrefix4 extends OpCodeHandler {
	private final OpCodeHandler handlerNP;
	private final OpCodeHandler handler66;
	private final OpCodeHandler handlerF3;
	private final OpCodeHandler handlerF2;
	private final int flags;

	OpCodeHandler_MandatoryPrefix4(OpCodeHandler handlerNP, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2, int flags) {
		if (handlerNP == null)
			throw new NullPointerException();
		if (handler66 == null)
			throw new NullPointerException();
		if (handlerF3 == null)
			throw new NullPointerException();
		if (handlerF2 == null)
			throw new NullPointerException();
		this.handlerNP = handlerNP;
		this.handler66 = handler66;
		this.handlerF3 = handlerF3;
		this.handlerF2 = handlerF2;
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler;
		int prefix = decoder.state_zs_mandatoryPrefix;
		switch (prefix) {
		case MandatoryPrefixByte.NONE:
			handler = handlerNP;
			break;
		case MandatoryPrefixByte.P66:
			handler = handler66;
			break;
		case MandatoryPrefixByte.PF3:
			if ((flags & 4) != 0)
				decoder.clearMandatoryPrefixF3(instruction);
			handler = handlerF3;
			break;
		case MandatoryPrefixByte.PF2:
			if ((flags & 8) != 0)
				decoder.clearMandatoryPrefixF2(instruction);
			handler = handlerF2;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		if (handler.hasModRM && (flags & 0x10) != 0)
			decoder.readModRM();
		handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_MandatoryPrefix_NoModRM extends OpCodeHandler {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_MandatoryPrefix_NoModRM(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
		if (handler == null)
			throw new NullPointerException();
		if (handler66 == null)
			throw new NullPointerException();
		if (handlerF3 == null)
			throw new NullPointerException();
		if (handlerF2 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler,
			handler66,
			handlerF3,
			handlerF2,
		};
		assert handler.hasModRM == hasModRM;
		assert handler66.hasModRM == hasModRM;
		assert handlerF3.hasModRM == hasModRM;
		assert handlerF2.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.clearMandatoryPrefix(instruction);
		handlers[decoder.state_zs_mandatoryPrefix].decode(decoder, instruction);
	}
}

final class OpCodeHandler_NIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_NIb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + Register.MM0);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Reservednop extends OpCodeHandlerModRM {
	private final OpCodeHandler reservedNopHandler;
	private final OpCodeHandler otherHandler;

	OpCodeHandler_Reservednop(OpCodeHandler reservedNopHandler, OpCodeHandler otherHandler) {
		if (reservedNopHandler == null)
			throw new NullPointerException();
		if (otherHandler == null)
			throw new NullPointerException();
		this.reservedNopHandler = reservedNopHandler;
		this.otherHandler = otherHandler;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		((decoder.options & DecoderOptions.FORCE_RESERVED_NOP) != 0 ? reservedNopHandler : otherHandler).decode(decoder, instruction);
	}
}

final class OpCodeHandler_Ev_Iz extends OpCodeHandlerModRM {
	private final int[] codes;
	private final int flags;

	OpCodeHandler_Ev_Iz(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
		flags = 0;
	}

	OpCodeHandler_Ev_Iz(int code16, int code32, int code64, int flags) {
		codes = new int[] { code16, code32, code64 };
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_mod < 3) {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		else {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		if (operandSize == OpSize.SIZE32) {
			instruction.setOp1Kind(OpKind.IMMEDIATE32);
			instruction.setImmediate32(decoder.readUInt32());
		}
		else if (operandSize == OpSize.SIZE64) {
			instruction.setOp1Kind(OpKind.IMMEDIATE32TO64);
			instruction.setImmediate32(decoder.readUInt32());
		}
		else {
			instruction.setOp1Kind(OpKind.IMMEDIATE16);
			instruction.setImmediate16((short)decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_Ev_Ib extends OpCodeHandlerModRM {
	private final int[] codes;
	private final int flags;

	OpCodeHandler_Ev_Ib(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
		flags = 0;
	}

	OpCodeHandler_Ev_Ib(int code16, int code32, int code64, int flags) {
		codes = new int[] { code16, code32, code64 };
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		if (operandSize == OpSize.SIZE32)
			instruction.setOp1Kind(OpKind.IMMEDIATE8TO32);
		else if (operandSize == OpSize.SIZE64)
			instruction.setOp1Kind(OpKind.IMMEDIATE8TO64);
		else
			instruction.setOp1Kind(OpKind.IMMEDIATE8TO16);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Ev_Ib2 extends OpCodeHandlerModRM {
	private final int[] codes;
	private final int flags;

	OpCodeHandler_Ev_Ib2(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
		flags = 0;
	}

	OpCodeHandler_Ev_Ib2(int code16, int code32, int code64, int flags) {
		codes = new int[] { code16, code32, code64 };
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Ev_1 extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Ev_1(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)1);
		decoder.state_zs_flags |= StateFlags.NO_IMM;
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ev_CL extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Ev_CL(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Register(Register.CL);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ev extends OpCodeHandlerModRM {
	private final int[] codes;
	private final int flags;

	OpCodeHandler_Ev(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
		flags = 0;
	}

	OpCodeHandler_Ev(int code16, int code32, int code64, int flags) {
		codes = new int[] { code16, code32, code64 };
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Rv extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Rv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		assert decoder.state_mod == 3 : decoder.state_mod;
	}
}

final class OpCodeHandler_Rv_32_64 extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Rv_32_64(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int baseReg;
		if (decoder.is64bMode) {
			instruction.setCode(code64);
			baseReg = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			baseReg = Register.EAX;
		}
		assert decoder.state_mod == 3 : decoder.state_mod;
		instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
	}
}

final class OpCodeHandler_Rq extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Rq(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		assert decoder.state_mod == 3 : decoder.state_mod;
		instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
	}
}

final class OpCodeHandler_Ev_REXW extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int flags;
	private final int disallowReg;
	private final int disallowMem;

	OpCodeHandler_Ev_REXW(int code32, int code64, int flags) {
		this.code32 = code32;
		this.code64 = code64;
		this.flags = flags;
		disallowReg = (flags & 1) != 0 ? 0 : 0xFFFF_FFFF;
		disallowMem = (flags & 2) != 0 ? 0 : 0xFFFF_FFFF;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
		if ((((flags & 4) | (decoder.state_zs_flags & StateFlags.HAS66)) & decoder.invalidCheckMask) == (4 | StateFlags.HAS66))
			decoder.setInvalidInstruction();
		if (decoder.state_mod == 3) {
			if ((decoder.state_zs_flags & StateFlags.W) != 0)
				instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
			else
				instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
			if ((disallowReg & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			if ((disallowMem & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Evj extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Evj(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
			if (decoder.state_mod < 3) {
				instruction.setOp0Kind(OpKind.MEMORY);
				decoder.readOpMem(instruction);
			}
			else {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state_operandSize != OpSize.SIZE16)
					instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
				else
					instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
			}
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
			if (decoder.state_mod < 3) {
				instruction.setOp0Kind(OpKind.MEMORY);
				decoder.readOpMem(instruction);
			}
			else {
				if (decoder.state_operandSize == OpSize.SIZE32)
					instruction.setOp0Register(decoder.state_rm + Register.EAX);
				else
					instruction.setOp0Register(decoder.state_rm + Register.AX);
			}
		}
	}
}

final class OpCodeHandler_Ep extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Ep(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize == OpSize.SIZE64 && (decoder.options & DecoderOptions.AMD) == 0)
			instruction.setCode(code64);
		else if (decoder.state_operandSize == OpSize.SIZE16)
			instruction.setCode(code16);
		else
			instruction.setCode(code32);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Evw extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Evw(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ew extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Ew(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ms extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Ms(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode)
			instruction.setCode(code64);
		else if (decoder.state_operandSize == OpSize.SIZE32)
			instruction.setCode(code32);
		else
			instruction.setCode(code16);
		assert decoder.state_mod != 3 : decoder.state_mod;
		instruction.setOp0Kind(OpKind.MEMORY);
		decoder.readOpMem(instruction);
	}
}

final class OpCodeHandler_Gv_Ev extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Ev(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod < 3) {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		else {
			instruction.setOp1Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
	}
}

final class OpCodeHandler_Gd_Rd extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Gd_Rd(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_Gv_M_as extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_M_as(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int addressSize = decoder.state_addressSize;
		instruction.setCode(codes[addressSize]);
		instruction.setOp0Register((addressSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gdq_Ev extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gdq_Ev(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (operandSize != OpSize.SIZE64)
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		else
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Ev3 extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Ev3(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Ev2 extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Ev2(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if (decoder.state_operandSize != OpSize.SIZE16)
				instruction.setOp1Register(index + Register.EAX);
			else
				instruction.setOp1Register(index + Register.AX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_R_C extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int baseReg;

	OpCodeHandler_R_C(int code32, int code64, int baseReg) {
		this.code32 = code32;
		this.code64 = code64;
		this.baseReg = baseReg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			instruction.setCode(code64);
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
		}
		int extraRegisterBase = decoder.state_zs_extraRegisterBase;
		// LOCK MOV CR0 is supported by some AMD CPUs
		if (baseReg == Register.CR0 && instruction.getLockPrefix() && (decoder.options & DecoderOptions.AMD) != 0) {
			if ((extraRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			extraRegisterBase = 8;
			instruction.setLockPrefix(false);
			decoder.state_zs_flags &= ~StateFlags.LOCK;
		}
		int reg = (decoder.state_reg + extraRegisterBase);
		if (decoder.invalidCheckMask != 0) {
			if (baseReg == Register.CR0) {
				if (reg == 1 || (reg != 8 && reg >= 5))
					decoder.setInvalidInstruction();
			}
			else if (baseReg == Register.DR0) {
				if (reg > 7)
					decoder.setInvalidInstruction();
			}
			else {
				assert !decoder.is64bMode;
				assert baseReg == Register.TR0 : baseReg;
			}
		}
		instruction.setOp1Register(reg + baseReg);
	}
}

final class OpCodeHandler_C_R extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int baseReg;

	OpCodeHandler_C_R(int code32, int code64, int baseReg) {
		this.code32 = code32;
		this.code64 = code64;
		this.baseReg = baseReg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			instruction.setCode(code64);
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
		}
		int extraRegisterBase = decoder.state_zs_extraRegisterBase;
		// LOCK MOV CR0 is supported by some AMD CPUs
		if (baseReg == Register.CR0 && instruction.getLockPrefix() && (decoder.options & DecoderOptions.AMD) != 0) {
			if ((extraRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			extraRegisterBase = 8;
			instruction.setLockPrefix(false);
			decoder.state_zs_flags &= ~StateFlags.LOCK;
		}
		int reg = (decoder.state_reg + extraRegisterBase);
		if (decoder.invalidCheckMask != 0) {
			if (baseReg == Register.CR0) {
				if (reg == 1 || (reg != 8 && reg >= 5))
					decoder.setInvalidInstruction();
			}
			else if (baseReg == Register.DR0) {
				if (reg > 7)
					decoder.setInvalidInstruction();
			}
			else {
				assert !decoder.is64bMode;
				assert baseReg == Register.TR0 : baseReg;
			}
		}
		instruction.setOp0Register(reg + baseReg);
	}
}

final class OpCodeHandler_Jb extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Jb(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.state_zs_flags |= StateFlags.BRANCH_IMM8;
		if (decoder.is64bMode) {
			if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state_operandSize != OpSize.SIZE16) {
				instruction.setCode(code64);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
				instruction.setNearBranch64((byte)decoder.readByte() + decoder.getCurrentInstructionPointer64());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
				instruction.setNearBranch16((short)((byte)decoder.readByte() + decoder.getCurrentInstructionPointer32()));
			}
		}
		else {
			if (decoder.state_operandSize != OpSize.SIZE16) {
				instruction.setCode(code32);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
				instruction.setNearBranch32((byte)decoder.readByte() + decoder.getCurrentInstructionPointer32());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
				instruction.setNearBranch16((short)((byte)decoder.readByte() + decoder.getCurrentInstructionPointer32()));
			}
		}
	}
}

final class OpCodeHandler_Jx extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Jx(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.state_zs_flags |= StateFlags.XBEGIN;
		if (decoder.is64bMode) {
			if (decoder.state_operandSize == OpSize.SIZE32) {
				instruction.setCode(code32);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
				instruction.setNearBranch64(decoder.readUInt32() + decoder.getCurrentInstructionPointer64());
			}
			else if (decoder.state_operandSize == OpSize.SIZE64) {
				instruction.setCode(code64);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
				instruction.setNearBranch64(decoder.readUInt32() + decoder.getCurrentInstructionPointer64());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
				instruction.setNearBranch64((short)decoder.readUInt16() + decoder.getCurrentInstructionPointer64());
			}
		}
		else {
			assert decoder.defaultCodeSize == CodeSize.CODE16 || decoder.defaultCodeSize == CodeSize.CODE32 : decoder.defaultCodeSize;
			if (decoder.state_operandSize == OpSize.SIZE32) {
				instruction.setCode(code32);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
				instruction.setNearBranch32(decoder.readUInt32() + decoder.getCurrentInstructionPointer32());
			}
			else {
				assert decoder.state_operandSize == OpSize.SIZE16 : decoder.state_operandSize;
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
				instruction.setNearBranch32((short)decoder.readUInt16() + decoder.getCurrentInstructionPointer32());
			}
		}
	}
}

final class OpCodeHandler_Jz extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Jz(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state_operandSize != OpSize.SIZE16) {
				instruction.setCode(code64);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
				instruction.setNearBranch64(decoder.readUInt32() + decoder.getCurrentInstructionPointer64());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
				instruction.setNearBranch16((short)(decoder.readUInt16() + decoder.getCurrentInstructionPointer32()));
			}
		}
		else {
			if (decoder.state_operandSize != OpSize.SIZE16) {
				instruction.setCode(code32);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
				instruction.setNearBranch32(decoder.readUInt32() + decoder.getCurrentInstructionPointer32());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
				instruction.setNearBranch16((short)(decoder.readUInt16() + decoder.getCurrentInstructionPointer32()));
			}
		}
	}
}

final class OpCodeHandler_Jb2 extends OpCodeHandler {
	private final int code16_16;
	private final int code16_32;
	private final int code16_64;
	private final int code32_16;
	private final int code32_32;
	private final int code64_32;
	private final int code64_64;

	OpCodeHandler_Jb2(int code16_16, int code16_32, int code16_64, int code32_16, int code32_32, int code64_32, int code64_64) {
		this.code16_16 = code16_16;
		this.code16_32 = code16_32;
		this.code16_64 = code16_64;
		this.code32_16 = code32_16;
		this.code32_32 = code32_32;
		this.code64_32 = code64_32;
		this.code64_64 = code64_64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.state_zs_flags |= StateFlags.BRANCH_IMM8;
		if (decoder.is64bMode) {
			if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state_operandSize != OpSize.SIZE16) {
				if (decoder.state_addressSize == OpSize.SIZE64)
					instruction.setCode(code64_64);
				else
					instruction.setCode(code64_32);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
				instruction.setNearBranch64((byte)decoder.readByte() + decoder.getCurrentInstructionPointer64());
			}
			else {
				if (decoder.state_addressSize == OpSize.SIZE64)
					instruction.setCode(code16_64);
				else
					instruction.setCode(code16_32);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
				instruction.setNearBranch16((short)((byte)decoder.readByte() + decoder.getCurrentInstructionPointer32()));
			}
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32) {
				if (decoder.state_addressSize == OpSize.SIZE32)
					instruction.setCode(code32_32);
				else
					instruction.setCode(code32_16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
				instruction.setNearBranch32((byte)decoder.readByte() + decoder.getCurrentInstructionPointer32());
			}
			else {
				if (decoder.state_addressSize == OpSize.SIZE32)
					instruction.setCode(code16_32);
				else
					instruction.setCode(code16_16);
				instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
				instruction.setNearBranch16((short)((byte)decoder.readByte() + decoder.getCurrentInstructionPointer32()));
			}
		}
	}
}

final class OpCodeHandler_Jdisp extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_Jdisp(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		assert !decoder.is64bMode;
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
			instruction.setNearBranch32(decoder.readUInt32());
		}
		else {
			instruction.setCode(code16);
			instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
			instruction.setNearBranch16((short)decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_PushOpSizeReg extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;
	private final int reg;

	OpCodeHandler_PushOpSizeReg(int code16, int code32, int code64, int reg) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
		}
		instruction.setOp0Register(reg);
	}
}

final class OpCodeHandler_PushEv extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_PushEv(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
		}
		if (decoder.state_mod == 3) {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if (decoder.is64bMode) {
				if (decoder.state_operandSize != OpSize.SIZE16)
					instruction.setOp0Register(index + Register.RAX);
				else
					instruction.setOp0Register(index + Register.AX);
			}
			else {
				if (decoder.state_operandSize == OpSize.SIZE32)
					instruction.setOp0Register(index + Register.EAX);
				else
					instruction.setOp0Register(index + Register.AX);
			}
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ev_Gv extends OpCodeHandlerModRM {
	private final int[] codes;
	private final int flags;

	OpCodeHandler_Ev_Gv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
		flags = 0;
	}

	OpCodeHandler_Ev_Gv(int code16, int code32, int code64, int flags) {
		codes = new int[] { code16, code32, code64 };
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ev_Gv_32_64 extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Ev_Gv_32_64(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int baseReg;
		if (decoder.is64bMode) {
			instruction.setCode(code64);
			baseReg = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			baseReg = Register.EAX;
		}
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ev_Gv_Ib extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Ev_Gv_Ib(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Ev_Gv_CL extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Ev_Gv_CL(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp2Register(Register.CL);
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Mp extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Gv_Mp(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize == OpSize.SIZE64 && (decoder.options & DecoderOptions.AMD) == 0) {
			instruction.setCode(code64);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else if (decoder.state_operandSize == OpSize.SIZE16) {
			instruction.setCode(code16);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Eb extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Eb(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp1Register(index + Register.AL);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Ew extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Ew(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_PushSimple2 extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_PushSimple2(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
		}
	}
}

final class OpCodeHandler_Simple2 extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Simple2(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
	}
}

final class OpCodeHandler_Simple2Iw extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Simple2Iw(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Kind(OpKind.IMMEDIATE16);
		instruction.setImmediate16((short)decoder.readUInt16());
	}
}

final class OpCodeHandler_Simple3 extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Simple3(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
		}
	}
}

final class OpCodeHandler_Simple5 extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Simple5(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int addressSize = decoder.state_addressSize;
		instruction.setCode(codes[addressSize]);
	}
}

final class OpCodeHandler_Simple5_a32 extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Simple5_a32(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_addressSize != OpSize.SIZE32 && decoder.invalidCheckMask != 0)
			decoder.setInvalidInstruction();
		int addressSize = decoder.state_addressSize;
		instruction.setCode(codes[addressSize]);
	}
}

final class OpCodeHandler_Simple5_ModRM_as extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Simple5_ModRM_as(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int addressSize = decoder.state_addressSize;
		instruction.setCode(codes[addressSize]);
		instruction.setOp0Register((addressSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
	}
}

final class OpCodeHandler_Simple4 extends OpCodeHandler {
	private final int code32;
	private final int code64;

	OpCodeHandler_Simple4(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
	}
}

final class OpCodeHandler_PushSimpleReg extends OpCodeHandler {
	private final int index;
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_PushSimpleReg(int index, int code16, int code32, int code64) {
		assert 0 <= index && index <= 7 : index;
		this.index = index;
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16) {
				instruction.setCode(code64);
				instruction.setOp0Register(index + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Register(index + decoder.state_zs_extraBaseRegisterBase + Register.AX);
			}
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32) {
				instruction.setCode(code32);
				instruction.setOp0Register(index + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Register(index + decoder.state_zs_extraBaseRegisterBase + Register.AX);
			}
		}
	}
}

final class OpCodeHandler_SimpleReg extends OpCodeHandler {
	private final int code;
	private final int index;

	OpCodeHandler_SimpleReg(int code, int index) {
		assert 0 <= index && index <= 7 : index;
		this.code = code;
		this.index = index;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int sizeIndex = decoder.state_operandSize;

		instruction.setCode(sizeIndex + code);
		instruction.setOp0Register(sizeIndex * 16 + index + decoder.state_zs_extraBaseRegisterBase + Register.AX);
	}
}

final class OpCodeHandler_Xchg_Reg_rAX extends OpCodeHandler {
	private final int index;
	private final int[] codes;

	OpCodeHandler_Xchg_Reg_rAX(int index) {
		assert 0 <= index && index <= 7 : index;
		this.index = index;
		codes = s_codes;
	}

	private static final int[] s_codes = new int[] {
		Code.NOPW,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,
		Code.XCHG_R16_AX,

		Code.NOPD,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,
		Code.XCHG_R32_EAX,

		Code.NOPQ,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
		Code.XCHG_R64_RAX,
	};

	@Override
	void decode(Decoder decoder, Instruction instruction) {

		if (index == 0 && decoder.state_zs_mandatoryPrefix == MandatoryPrefixByte.PF3 && (decoder.options & DecoderOptions.NO_PAUSE) == 0) {
			decoder.clearMandatoryPrefixF3(instruction);
			instruction.setCode(Code.PAUSE);
		}
		else {
			int sizeIndex = decoder.state_operandSize;
			int codeIndex = index + decoder.state_zs_extraBaseRegisterBase;

			instruction.setCode(codes[sizeIndex * 16 + codeIndex]);
			if (codeIndex != 0) {
				int reg = sizeIndex * 16 + codeIndex + Register.AX;
				instruction.setOp0Register(reg);
				instruction.setOp1Register(sizeIndex * 16 + Register.AX);
			}
		}
	}
}

final class OpCodeHandler_Reg_Iz extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Reg_Iz(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize == OpSize.SIZE32) {
			instruction.setCode(code32);
			instruction.setOp0Register(Register.EAX);
			instruction.setOp1Kind(OpKind.IMMEDIATE32);
			instruction.setImmediate32(decoder.readUInt32());
		}
		else if (decoder.state_operandSize == OpSize.SIZE64) {
			instruction.setCode(code64);
			instruction.setOp0Register(Register.RAX);
			instruction.setOp1Kind(OpKind.IMMEDIATE32TO64);
			instruction.setImmediate32(decoder.readUInt32());
		}
		else {
			instruction.setCode(code16);
			instruction.setOp0Register(Register.AX);
			instruction.setOp1Kind(OpKind.IMMEDIATE16);
			instruction.setImmediate16((short)decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_RegIb3 extends OpCodeHandler {
	private final int index;
	private final int[] withRexPrefix;

	OpCodeHandler_RegIb3(int index) {
		assert 0 <= index && index <= 7 : index;
		this.index = index;
		withRexPrefix = s_withRexPrefix;
	}

	private static final int[] s_withRexPrefix = new int[] {
		Register.AL,
		Register.CL,
		Register.DL,
		Register.BL,
		Register.SPL,
		Register.BPL,
		Register.SIL,
		Register.DIL,
		Register.R8L,
		Register.R9L,
		Register.R10L,
		Register.R11L,
		Register.R12L,
		Register.R13L,
		Register.R14L,
		Register.R15L,
	};

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int register;
		if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0)
			register = withRexPrefix[index + decoder.state_zs_extraBaseRegisterBase];
		else
			register = index + Register.AL;
		instruction.setCode(Code.MOV_R8_IMM8);
		instruction.setOp0Register(register);
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_RegIz2 extends OpCodeHandler {
	private final int index;

	OpCodeHandler_RegIz2(int index) {
		assert 0 <= index && index <= 7 : index;
		this.index = index;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize == OpSize.SIZE32) {
			instruction.setCode(Code.MOV_R32_IMM32);
			instruction.setOp0Register(index + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
			instruction.setOp1Kind(OpKind.IMMEDIATE32);
			instruction.setImmediate32(decoder.readUInt32());
		}
		else if (decoder.state_operandSize == OpSize.SIZE64) {
			instruction.setCode(Code.MOV_R64_IMM64);
			instruction.setOp0Register(index + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
			instruction.setOp1Kind(OpKind.IMMEDIATE64);
			instruction.setImmediate64(decoder.readUInt64());
		}
		else {
			instruction.setCode(Code.MOV_R16_IMM16);
			instruction.setOp0Register(index + decoder.state_zs_extraBaseRegisterBase + Register.AX);
			instruction.setOp1Kind(OpKind.IMMEDIATE16);
			instruction.setImmediate16((short)decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_PushIb2 extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_PushIb2(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16) {
				instruction.setCode(code64);
				instruction.setOp0Kind(OpKind.IMMEDIATE8TO64);
				instruction.setImmediate8((byte)decoder.readByte());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.IMMEDIATE8TO16);
				instruction.setImmediate8((byte)decoder.readByte());
			}
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32) {
				instruction.setCode(code32);
				instruction.setOp0Kind(OpKind.IMMEDIATE8TO32);
				instruction.setImmediate8((byte)decoder.readByte());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.IMMEDIATE8TO16);
				instruction.setImmediate8((byte)decoder.readByte());
			}
		}
	}
}

final class OpCodeHandler_PushIz extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_PushIz(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16) {
				instruction.setCode(code64);
				instruction.setOp0Kind(OpKind.IMMEDIATE32TO64);
				instruction.setImmediate32(decoder.readUInt32());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.IMMEDIATE16);
				instruction.setImmediate16((short)decoder.readUInt16());
			}
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32) {
				instruction.setCode(code32);
				instruction.setOp0Kind(OpKind.IMMEDIATE32);
				instruction.setImmediate32(decoder.readUInt32());
			}
			else {
				instruction.setCode(code16);
				instruction.setOp0Kind(OpKind.IMMEDIATE16);
				instruction.setImmediate16((short)decoder.readUInt16());
			}
		}
	}
}

final class OpCodeHandler_Gv_Ma extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;

	OpCodeHandler_Gv_Ma(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		else {
			instruction.setCode(code16);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		}
		assert decoder.state_mod != 3 : decoder.state_mod;
		instruction.setOp1Kind(OpKind.MEMORY);
		decoder.readOpMem(instruction);
	}
}

final class OpCodeHandler_RvMw_Gw extends OpCodeHandlerModRM {
	private final int code16;
	private final int code32;

	OpCodeHandler_RvMw_Gw(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int baseReg;
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
			baseReg = Register.EAX;
		}
		else {
			instruction.setCode(code16);
			instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
			baseReg = Register.AX;
		}
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Ev_Ib extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Ev_Ib(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		if (operandSize == OpSize.SIZE32) {
			instruction.setOp2Kind(OpKind.IMMEDIATE8TO32);
			instruction.setImmediate8((byte)decoder.readByte());
		}
		else if (operandSize == OpSize.SIZE64) {
			instruction.setOp2Kind(OpKind.IMMEDIATE8TO64);
			instruction.setImmediate8((byte)decoder.readByte());
		}
		else {
			instruction.setOp2Kind(OpKind.IMMEDIATE8TO16);
			instruction.setImmediate8((byte)decoder.readByte());
		}
	}
}

final class OpCodeHandler_Gv_Ev_Ib_REX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Gv_Ev_Ib_REX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(code64);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		assert decoder.state_mod == 3 : decoder.state_mod;
		instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Gv_Ev_32_64 extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int disallowReg;
	private final int disallowMem;

	OpCodeHandler_Gv_Ev_32_64(int code32, int code64, boolean allowReg, boolean allowMem) {
		this.code32 = code32;
		this.code64 = code64;
		disallowMem = allowMem ? 0 : 0xFFFF_FFFF;
		disallowReg = allowReg ? 0 : 0xFFFF_FFFF;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int baseReg;
		if (decoder.is64bMode) {
			instruction.setCode(code64);
			baseReg = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			baseReg = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + baseReg);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
			if ((disallowReg & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			if ((disallowMem & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Ev_Iz extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Ev_Iz(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		if (operandSize == OpSize.SIZE32) {
			instruction.setOp2Kind(OpKind.IMMEDIATE32);
			instruction.setImmediate32(decoder.readUInt32());
		}
		else if (operandSize == OpSize.SIZE64) {
			instruction.setOp2Kind(OpKind.IMMEDIATE32TO64);
			instruction.setImmediate32(decoder.readUInt32());
		}
		else {
			instruction.setOp2Kind(OpKind.IMMEDIATE16);
			instruction.setImmediate16((short)decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_Yb_Reg extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_Yb_Reg(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(reg);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp0Kind(OpKind.MEMORY_ESRDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp0Kind(OpKind.MEMORY_ESEDI);
		else
			instruction.setOp0Kind(OpKind.MEMORY_ESDI);
	}
}

final class OpCodeHandler_Yv_Reg extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Yv_Reg(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Register((operandSize << 4) + Register.AX);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp0Kind(OpKind.MEMORY_ESRDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp0Kind(OpKind.MEMORY_ESEDI);
		else
			instruction.setOp0Kind(OpKind.MEMORY_ESDI);
	}
}

final class OpCodeHandler_Yv_Reg2 extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_Yv_Reg2(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp1Register(Register.DX);
		}
		else {
			instruction.setCode(code16);
			instruction.setOp1Register(Register.DX);
		}
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp0Kind(OpKind.MEMORY_ESRDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp0Kind(OpKind.MEMORY_ESEDI);
		else
			instruction.setOp0Kind(OpKind.MEMORY_ESDI);
	}
}

final class OpCodeHandler_Reg_Xb extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_Reg_Xb(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(reg);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_RSI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_ESI);
		else
			instruction.setOp1Kind(OpKind.MEMORY_SEG_SI);
	}
}

final class OpCodeHandler_Reg_Xv extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Reg_Xv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + Register.AX);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_RSI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_ESI);
		else
			instruction.setOp1Kind(OpKind.MEMORY_SEG_SI);
	}
}

final class OpCodeHandler_Reg_Xv2 extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_Reg_Xv2(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp0Register(Register.DX);
		}
		else {
			instruction.setCode(code16);
			instruction.setOp0Register(Register.DX);
		}
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_RSI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_ESI);
		else
			instruction.setOp1Kind(OpKind.MEMORY_SEG_SI);
	}
}

final class OpCodeHandler_Reg_Yb extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_Reg_Yb(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(reg);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp1Kind(OpKind.MEMORY_ESRDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp1Kind(OpKind.MEMORY_ESEDI);
		else
			instruction.setOp1Kind(OpKind.MEMORY_ESDI);
	}
}

final class OpCodeHandler_Reg_Yv extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Reg_Yv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + Register.AX);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp1Kind(OpKind.MEMORY_ESRDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp1Kind(OpKind.MEMORY_ESEDI);
		else
			instruction.setOp1Kind(OpKind.MEMORY_ESDI);
	}
}

final class OpCodeHandler_Yb_Xb extends OpCodeHandler {
	private final int code;

	OpCodeHandler_Yb_Xb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setOp0Kind(OpKind.MEMORY_ESRDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_RSI);
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setOp0Kind(OpKind.MEMORY_ESEDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_ESI);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY_ESDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_SI);
		}
	}
}

final class OpCodeHandler_Yv_Xv extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Yv_Xv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setOp0Kind(OpKind.MEMORY_ESRDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_RSI);
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setOp0Kind(OpKind.MEMORY_ESEDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_ESI);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY_ESDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_SI);
		}
	}
}

final class OpCodeHandler_Xb_Yb extends OpCodeHandler {
	private final int code;

	OpCodeHandler_Xb_Yb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_RSI);
			instruction.setOp1Kind(OpKind.MEMORY_ESRDI);
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_ESI);
			instruction.setOp1Kind(OpKind.MEMORY_ESEDI);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_SI);
			instruction.setOp1Kind(OpKind.MEMORY_ESDI);
		}
	}
}

final class OpCodeHandler_Xv_Yv extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Xv_Yv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_RSI);
			instruction.setOp1Kind(OpKind.MEMORY_ESRDI);
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_ESI);
			instruction.setOp1Kind(OpKind.MEMORY_ESEDI);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_SI);
			instruction.setOp1Kind(OpKind.MEMORY_ESDI);
		}
	}
}

final class OpCodeHandler_Ev_Sw extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Ev_Sw(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp1Register(decoder.readOpSegReg());
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_M_Sw extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_M_Sw(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(decoder.readOpSegReg());
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_M extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_M(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod < 3) {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_Sw_Ev extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Sw_Ev(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		int sreg = decoder.readOpSegReg();
		if (decoder.invalidCheckMask != 0 && sreg == Register.CS)
			decoder.setInvalidInstruction();
		instruction.setOp0Register(sreg);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register((operandSize << 4) + decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.AX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Sw_M extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Sw_M(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.readOpSegReg());
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ap extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_Ap(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_operandSize != OpSize.SIZE16)
			instruction.setCode(code32);
		else
			instruction.setCode(code16);
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setOp0Kind(OpKind.FAR_BRANCH32);
			instruction.setFarBranch32(decoder.readUInt32());
		}
		else {
			instruction.setOp0Kind(OpKind.FAR_BRANCH16);
			instruction.setFarBranch16((short)decoder.readUInt16());
		}
		instruction.setFarBranchSelector((short)decoder.readUInt16());
	}
}

final class OpCodeHandler_Reg_Ob extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_Reg_Ob(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(reg);
		decoder.displIndex = decoder.state_zs_instructionLength;
		instruction.setOp1Kind(OpKind.MEMORY);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setMemoryDisplSize(8);
			decoder.state_zs_flags |= StateFlags.ADDR64;
			instruction.setMemoryDisplacement64(decoder.readUInt64());
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setMemoryDisplSize(4);
			instruction.setMemoryDisplacement64(decoder.readUInt32());
		}
		else {
			instruction.setMemoryDisplSize(2);
			instruction.setMemoryDisplacement64(decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_Ob_Reg extends OpCodeHandler {
	private final int code;
	private final int reg;

	OpCodeHandler_Ob_Reg(int code, int reg) {
		this.code = code;
		this.reg = reg;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		decoder.displIndex = decoder.state_zs_instructionLength;
		instruction.setOp0Kind(OpKind.MEMORY);
		instruction.setOp1Register(reg);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setMemoryDisplSize(8);
			decoder.state_zs_flags |= StateFlags.ADDR64;
			instruction.setMemoryDisplacement64(decoder.readUInt64());
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setMemoryDisplSize(4);
			instruction.setMemoryDisplacement64(decoder.readUInt32());
		}
		else {
			instruction.setMemoryDisplSize(2);
			instruction.setMemoryDisplacement64(decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_Reg_Ov extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Reg_Ov(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.displIndex = decoder.state_zs_instructionLength;
		instruction.setOp1Kind(OpKind.MEMORY);
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + Register.AX);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setMemoryDisplSize(8);
			decoder.state_zs_flags |= StateFlags.ADDR64;
			instruction.setMemoryDisplacement64(decoder.readUInt64());
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setMemoryDisplSize(4);
			instruction.setMemoryDisplacement64(decoder.readUInt32());
		}
		else {
			instruction.setMemoryDisplSize(2);
			instruction.setMemoryDisplacement64(decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_Ov_Reg extends OpCodeHandler {
	private final int[] codes;

	OpCodeHandler_Ov_Reg(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.displIndex = decoder.state_zs_instructionLength;
		instruction.setOp0Kind(OpKind.MEMORY);
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Register((operandSize << 4) + Register.AX);
		if (decoder.state_addressSize == OpSize.SIZE64) {
			instruction.setMemoryDisplSize(8);
			decoder.state_zs_flags |= StateFlags.ADDR64;
			instruction.setMemoryDisplacement64(decoder.readUInt64());
		}
		else if (decoder.state_addressSize == OpSize.SIZE32) {
			instruction.setMemoryDisplSize(4);
			instruction.setMemoryDisplacement64(decoder.readUInt32());
		}
		else {
			instruction.setMemoryDisplSize(2);
			instruction.setMemoryDisplacement64(decoder.readUInt16());
		}
	}
}

final class OpCodeHandler_BranchIw extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_BranchIw(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
		}
		instruction.setOp0Kind(OpKind.IMMEDIATE16);
		instruction.setImmediate16((short)decoder.readUInt16());
	}
}

final class OpCodeHandler_BranchSimple extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_BranchSimple(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.is64bMode) {
			if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
		}
	}
}

final class OpCodeHandler_Iw_Ib extends OpCodeHandler {
	private final int code16;
	private final int code32;
	private final int code64;

	OpCodeHandler_Iw_Ib(int code16, int code32, int code64) {
		this.code16 = code16;
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp0Kind(OpKind.IMMEDIATE16);
		instruction.setImmediate16((short)decoder.readUInt16());
		instruction.setOp1Kind(OpKind.IMMEDIATE8_2ND);
		instruction.setImmediate8_2nd((byte)decoder.readByte());
		if (decoder.is64bMode) {
			if (decoder.state_operandSize != OpSize.SIZE16)
				instruction.setCode(code64);
			else
				instruction.setCode(code16);
		}
		else {
			if (decoder.state_operandSize == OpSize.SIZE32)
				instruction.setCode(code32);
			else
				instruction.setCode(code16);
		}
	}
}

final class OpCodeHandler_Reg_Ib2 extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_Reg_Ib2(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp0Register(Register.EAX);
		}
		else {
			instruction.setCode(code16);
			instruction.setOp0Register(Register.AX);
		}
	}
}

final class OpCodeHandler_IbReg2 extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_IbReg2(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp0Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp1Register(Register.EAX);
		}
		else {
			instruction.setCode(code16);
			instruction.setOp1Register(Register.AX);
		}
	}
}

final class OpCodeHandler_eAX_DX extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_eAX_DX(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp1Register(Register.DX);
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp0Register(Register.EAX);
		}
		else {
			instruction.setCode(code16);
			instruction.setOp0Register(Register.AX);
		}
	}
}

final class OpCodeHandler_DX_eAX extends OpCodeHandler {
	private final int code16;
	private final int code32;

	OpCodeHandler_DX_eAX(int code16, int code32) {
		this.code16 = code16;
		this.code32 = code32;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp0Register(Register.DX);
		if (decoder.state_operandSize != OpSize.SIZE16) {
			instruction.setCode(code32);
			instruction.setOp1Register(Register.EAX);
		}
		else {
			instruction.setCode(code16);
			instruction.setOp1Register(Register.AX);
		}
	}
}

final class OpCodeHandler_Eb_Ib extends OpCodeHandlerModRM {
	private final int code;
	private final int flags;

	OpCodeHandler_Eb_Ib(int code) {
		this.code = code;
		flags = 0;
	}

	OpCodeHandler_Eb_Ib(int code, int flags) {
		this.code = code;
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_mod < 3) {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		else {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp0Register(index + Register.AL);
		}
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Eb_1 extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Eb_1(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)1);
		decoder.state_zs_flags |= StateFlags.NO_IMM;
		if (decoder.state_mod == 3) {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp0Register(index + Register.AL);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Eb_CL extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Eb_CL(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(Register.CL);
		if (decoder.state_mod == 3) {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp0Register(index + Register.AL);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Eb extends OpCodeHandlerModRM {
	private final int code;
	private final int flags;

	OpCodeHandler_Eb(int code) {
		this.code = code;
		flags = 0;
	}

	OpCodeHandler_Eb(int code, int flags) {
		this.code = code;
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_mod == 3) {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp0Register(index + Register.AL);
		}
		else {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Eb_Gb extends OpCodeHandlerModRM {
	private final int code;
	private final int flags;

	OpCodeHandler_Eb_Gb(int code) {
		this.code = code;
		flags = 0;
	}

	OpCodeHandler_Eb_Gb(int code, int flags) {
		this.code = code;
		this.flags = flags;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		int index;
		index = decoder.state_reg + decoder.state_zs_extraRegisterBase;
		if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
			index += 4;
		instruction.setOp1Register(index + Register.AL);
		if (decoder.state_mod == 3) {
			index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp0Register(index + Register.AL);
		}
		else {
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gb_Eb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Gb_Eb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		int index = decoder.state_reg + decoder.state_zs_extraRegisterBase;
		if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
			index += 4;
		instruction.setOp0Register(index + Register.AL);

		if (decoder.state_mod == 3) {
			index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp1Register(index + Register.AL);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_M extends OpCodeHandlerModRM {
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_M(int codeW0, int codeW1) {
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	OpCodeHandler_M(int codeW0) {
		this.codeW0 = codeW0;
		codeW1 = codeW0;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(codeW1);
		else
			instruction.setCode(codeW0);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_M_REXW extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int flags32;
	private final int flags64;

	OpCodeHandler_M_REXW(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
		flags32 = 0;
		flags64 = 0;
	}

	OpCodeHandler_M_REXW(int code32, int code64, int flags32, int flags64) {
		this.code32 = code32;
		this.code64 = code64;
		this.flags32 = flags32;
		this.flags64 = flags64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			int flags = (decoder.state_zs_flags & StateFlags.W) != 0 ? flags64 : flags32;
			if ((flags & (HandlerFlags.XACQUIRE | HandlerFlags.XRELEASE)) != 0)
				decoder.setXacquireXrelease(instruction);
			decoder.state_zs_flags |= (flags & HandlerFlags.LOCK) << (13 - 3);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_MemBx extends OpCodeHandler {
	private final int code;

	OpCodeHandler_MemBx(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setMemoryIndex(Register.AL);
		instruction.setOp0Kind(OpKind.MEMORY);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setMemoryBase(Register.RBX);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setMemoryBase(Register.EBX);
		else
			instruction.setMemoryBase(Register.BX);
	}
}

final class OpCodeHandler_VW extends OpCodeHandlerModRM {
	private final int codeR;
	private final int codeM;

	OpCodeHandler_VW(int codeR, int codeM) {
		this.codeR = codeR;
		this.codeM = codeM;
	}

	OpCodeHandler_VW(int code) {
		codeR = code;
		codeM = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setCode(codeR);
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else {
			instruction.setCode(codeM);
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_WV extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_WV(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod < 3) {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		else {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
	}
}

final class OpCodeHandler_rDI_VX_RX extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_rDI_VX_RX(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_RDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_EDI);
		else
			instruction.setOp0Kind(OpKind.MEMORY_SEG_DI);
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_rDI_P_N extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_rDI_P_N(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_addressSize == OpSize.SIZE64)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_RDI);
		else if (decoder.state_addressSize == OpSize.SIZE32)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_EDI);
		else
			instruction.setOp0Kind(OpKind.MEMORY_SEG_DI);
		instruction.setOp1Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp2Register(decoder.state_rm + Register.MM0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VM extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VM(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_MV extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MV(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_VQ extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VQ(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.MM0);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_P_Q extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_P_Q(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.MM0);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Q_P extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Q_P(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + Register.MM0);
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_MP extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MP(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_P_Q_Ib extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_P_Q_Ib(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.MM0);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_P_W extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_P_W(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_P_R extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_P_R(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_P_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_P_Ev(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_P_Ev_Ib extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_P_Ev_Ib(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(code64);
			gpr = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			gpr = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + Register.MM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + gpr);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Ev_P extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Ev_P(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp1Register(decoder.state_reg + Register.MM0);
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
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

final class OpCodeHandler_Gv_W extends OpCodeHandlerModRM {
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_Gv_W(int codeW0, int codeW1) {
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(codeW1);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(codeW0);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_V_Ev extends OpCodeHandlerModRM {
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_V_Ev(int codeW0, int codeW1) {
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if (decoder.state_operandSize != OpSize.SIZE64) {
			instruction.setCode(codeW0);
			gpr = Register.EAX;
		}
		else {
			instruction.setCode(codeW1);
			gpr = Register.RAX;
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

final class OpCodeHandler_VWIb extends OpCodeHandlerModRM {
	private final int codeW0;
	private final int codeW1;

	OpCodeHandler_VWIb(int code) {
		codeW0 = code;
		codeW1 = code;
	}

	OpCodeHandler_VWIb(int codeW0, int codeW1) {
		this.codeW0 = codeW0;
		this.codeW1 = codeW1;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(codeW1);
		else
			instruction.setCode(codeW0);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VRIbIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VRIbIb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
		instruction.setOp3Kind(OpKind.IMMEDIATE8_2ND);
		instruction.setImmediate8_2nd((byte)decoder.readByte());
	}
}

final class OpCodeHandler_RIbIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_RIbIb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
		instruction.setOp2Kind(OpKind.IMMEDIATE8_2ND);
		instruction.setImmediate8_2nd((byte)decoder.readByte());
	}
}

final class OpCodeHandler_RIb extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_RIb(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Ed_V_Ib extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Ed_V_Ib(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
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
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_VX_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VX_Ev(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
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

final class OpCodeHandler_Ev_VX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Ev_VX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
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

final class OpCodeHandler_VX_E_Ib extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_VX_E_Ib(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
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
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Gv_RX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Gv_RX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		else
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.XMM0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_B_MIB extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_B_MIB(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_reg > 3 || (decoder.state_zs_extraRegisterBase & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + Register.BND0);
		assert decoder.state_mod != 3 : decoder.state_mod;
		instruction.setOp1Kind(OpKind.MEMORY);
		decoder.readOpMem_MPX(instruction);
		// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
		if (decoder.invalidCheckMask != 0 && instruction.getMemoryBase() == Register.RIP)
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_MIB_B extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_MIB_B(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_reg > 3 || (decoder.state_zs_extraRegisterBase & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		instruction.setCode(code);
		instruction.setOp1Register(decoder.state_reg + Register.BND0);
		assert decoder.state_mod != 3 : decoder.state_mod;
		instruction.setOp0Kind(OpKind.MEMORY);
		decoder.readOpMem_MPX(instruction);
		// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
		if (decoder.invalidCheckMask != 0 && instruction.getMemoryBase() == Register.RIP)
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_B_BM extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_B_BM(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_reg > 3 || (decoder.state_zs_extraRegisterBase & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		if (decoder.is64bMode)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
		instruction.setOp0Register(decoder.state_reg + Register.BND0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.BND0);
			if (decoder.state_rm > 3 || (decoder.state_zs_extraBaseRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem_MPX(instruction);
		}
	}
}

final class OpCodeHandler_BM_B extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_BM_B(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_reg > 3 || ((decoder.state_zs_extraRegisterBase & decoder.invalidCheckMask) != 0))
			decoder.setInvalidInstruction();
		if (decoder.is64bMode)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
		instruction.setOp1Register(decoder.state_reg + Register.BND0);
		if (decoder.state_mod == 3) {
			instruction.setOp0Register(decoder.state_rm + Register.BND0);
			if (decoder.state_rm > 3 || (decoder.state_zs_extraBaseRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.setInvalidInstruction();
		}
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem_MPX(instruction);
		}
	}
}

final class OpCodeHandler_B_Ev extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;
	private final int ripRelMask;

	OpCodeHandler_B_Ev(int code32, int code64, boolean supportsRipRel) {
		this.code32 = code32;
		this.code64 = code64;
		ripRelMask = supportsRipRel ? 0 : 0xFFFF_FFFF;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if (decoder.state_reg > 3 || (decoder.state_zs_extraRegisterBase & decoder.invalidCheckMask) != 0)
			decoder.setInvalidInstruction();
		int baseReg;
		if (decoder.is64bMode) {
			instruction.setCode(code64);
			baseReg = Register.RAX;
		}
		else {
			instruction.setCode(code32);
			baseReg = Register.EAX;
		}
		instruction.setOp0Register(decoder.state_reg + Register.BND0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + baseReg);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem_MPX(instruction);
			// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
			if ((ripRelMask & decoder.invalidCheckMask) != 0 && instruction.getMemoryBase() == Register.RIP)
				decoder.setInvalidInstruction();
		}
	}
}

final class OpCodeHandler_Mv_Gv_REXW extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Mv_Gv_REXW(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(code64);
			instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_N_Ib_REX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Gv_N_Ib_REX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		else
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.MM0);
		}
		else
			decoder.setInvalidInstruction();
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Gv_N extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Gv_N(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setCode(code64);
		else
			instruction.setCode(code32);
		if ((decoder.state_zs_flags & StateFlags.W) != 0)
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		else
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.MM0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_VN extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_VN(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
		instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		if (decoder.state_mod == 3) {
			instruction.setOp1Register(decoder.state_rm + Register.MM0);
		}
		else
			decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_Gv_Mv extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Gv_Mv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp0Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Mv_Gv extends OpCodeHandlerModRM {
	private final int[] codes;

	OpCodeHandler_Mv_Gv(int code16, int code32, int code64) {
		codes = new int[] { code16, code32, code64 };
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		int operandSize = decoder.state_operandSize;
		instruction.setCode(codes[operandSize]);
		instruction.setOp1Register((operandSize << 4) + decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.AX);
		if (decoder.state_mod == 3)
			decoder.setInvalidInstruction();
		else {
			instruction.setOp0Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Eb_REX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Gv_Eb_REX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(code64);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		if (decoder.state_mod == 3) {
			int index = decoder.state_rm + decoder.state_zs_extraBaseRegisterBase;
			if ((decoder.state_zs_flags & StateFlags.HAS_REX) != 0 && index >= 4)
				index += 4;
			instruction.setOp1Register(index + Register.AL);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Gv_Ev_REX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Gv_Ev_REX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(code64);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp0Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		if (decoder.state_mod == 3) {
			if ((decoder.state_zs_flags & StateFlags.W) != 0)
				instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.RAX);
			else
				instruction.setOp1Register(decoder.state_rm + decoder.state_zs_extraBaseRegisterBase + Register.EAX);
		}
		else {
			instruction.setOp1Kind(OpKind.MEMORY);
			decoder.readOpMem(instruction);
		}
	}
}

final class OpCodeHandler_Ev_Gv_REX extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_Ev_Gv_REX(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
			instruction.setCode(code64);
			instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.RAX);
		}
		else {
			instruction.setCode(code32);
			instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.EAX);
		}
		assert decoder.state_mod != 3 : decoder.state_mod;
		instruction.setOp0Kind(OpKind.MEMORY);
		decoder.readOpMem(instruction);
	}
}

final class OpCodeHandler_GvM_VX_Ib extends OpCodeHandlerModRM {
	private final int code32;
	private final int code64;

	OpCodeHandler_GvM_VX_Ib(int code32, int code64) {
		this.code32 = code32;
		this.code64 = code64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setOp1Register(decoder.state_reg + decoder.state_zs_extraRegisterBase + Register.XMM0);
		int gpr;
		if ((decoder.state_zs_flags & StateFlags.W) != 0) {
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
		instruction.setOp2Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate8((byte)decoder.readByte());
	}
}

final class OpCodeHandler_Wbinvd extends OpCodeHandler {
	OpCodeHandler_Wbinvd() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		if ((decoder.options & DecoderOptions.NO_WBNOINVD) != 0 || decoder.state_zs_mandatoryPrefix != MandatoryPrefixByte.PF3)
			instruction.setCode(Code.WBINVD);
		else {
			decoder.clearMandatoryPrefixF3(instruction);
			instruction.setCode(Code.WBNOINVD);
		}
	}
}
