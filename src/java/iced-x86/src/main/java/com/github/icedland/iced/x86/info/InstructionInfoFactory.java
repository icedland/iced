// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemorySize;
import com.github.icedland.iced.x86.MvexRegMemConv;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.VsibFlags;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.InstrInfoTable;
import com.github.icedland.iced.x86.internal.info.ImpliedAccess;
import com.github.icedland.iced.x86.internal.info.InfoFlags1;
import com.github.icedland.iced.x86.internal.info.InfoFlags2;

/**
 * Creates {@link InstructionInfo}s.
 */
public final class InstructionInfoFactory {
	private static final class Flags {
		static final int NO_MEMORY_USAGE = 0x0000_0001;
		static final int NO_REGISTER_USAGE = 0x0000_0002;
		static final int IS_64_BIT = 0x0000_0004;
		static final int ZERO_EXT_VEC_REGS = 0x0000_0008;
	}

	private InstructionInfo info;
	// Java doesn't have out/ref parameters and we need to return 3 values from getXSP()
	// so we store 2 of those return values here. No allocations needed.
	private long xspMask;
	private int xspAddressSize;

	/**
	 * Constructor
	 */
	public InstructionInfoFactory() {
		info = new InstructionInfo();
	}

	/**
	 * Creates an {@link InstructionInfo}. The return value is only valid until this instance creates a new {@link InstructionInfo} value.
	 *
	 * @param instruction Instruction
	 */
	public InstructionInfo getInfo(Instruction instruction) {
		return create(instruction, InstructionInfoOptions.NONE);
	}

	/**
	 * Creates an {@link InstructionInfo}. The return value is only valid until this instance creates a new {@link InstructionInfo} value.
	 *
	 * @param instruction Instruction (an {@link InstructionInfoOptions} flags value)
	 * @param options     Options
	 */
	public InstructionInfo getInfo(Instruction instruction, int options) {
		return create(instruction, options);
	}

	private InstructionInfo create(Instruction instruction, int options) {
		info.usedRegisters.clear();
		info.usedMemoryLocations.clear();

		int[] data = InstrInfoTable.data;
		int index = instruction.getCode() << 1;
		int flags1 = data[index];
		int flags2 = data[index + 1];

		int codeSize = instruction.getCodeSize();
		int flags = options & (Flags.NO_MEMORY_USAGE | Flags.NO_REGISTER_USAGE);
		if (codeSize == CodeSize.CODE64 || codeSize == CodeSize.UNKNOWN)
			flags |= Flags.IS_64_BIT;
		if ((flags2 & (InfoFlags2.ENCODING_MASK << InfoFlags2.ENCODING_SHIFT)) != (EncodingKind.LEGACY << InfoFlags2.ENCODING_SHIFT))
			flags |= Flags.ZERO_EXT_VEC_REGS;

		int op0Access;
		int op0Info = (flags1 >>> InfoFlags1.OP_INFO0_SHIFT) & InfoFlags1.OP_INFO0_MASK;
		switch (op0Info) {
		default:
		case OpInfo0.NONE:
			op0Access = OpAccess.NONE;
			break;

		case OpInfo0.READ:
			op0Access = OpAccess.READ;
			break;

		case OpInfo0.WRITE:
			if (instruction.hasOpMask() && instruction.getMergingMasking()) {
				if (instruction.getOp0Kind() != OpKind.REGISTER)
					op0Access = OpAccess.COND_WRITE;
				else
					op0Access = OpAccess.READ_WRITE;
			}
			else
				op0Access = OpAccess.WRITE;
			break;

		case OpInfo0.WRITE_VMM:
			if (instruction.hasOpMask() && instruction.getMergingMasking()) {
				if (instruction.getOp0Kind() != OpKind.REGISTER)
					op0Access = OpAccess.COND_WRITE;
				else
					op0Access = OpAccess.READ_COND_WRITE;
			}
			else
				op0Access = OpAccess.WRITE;
			break;

		case OpInfo0.WRITE_FORCE:
		case OpInfo0.WRITE_FORCE_P1:
			op0Access = OpAccess.WRITE;
			break;

		case OpInfo0.COND_WRITE:
			op0Access = OpAccess.COND_WRITE;
			break;

		case OpInfo0.COND_WRITE32_READ_WRITE64:
			if ((flags & Flags.IS_64_BIT) != 0)
				op0Access = OpAccess.READ_WRITE;
			else
				op0Access = OpAccess.COND_WRITE;
			break;

		case OpInfo0.READ_WRITE:
			op0Access = OpAccess.READ_WRITE;
			break;

		case OpInfo0.READ_WRITE_VMM:
			if (instruction.hasOpMask() && instruction.getMergingMasking())
				op0Access = OpAccess.READ_COND_WRITE;
			else
				op0Access = OpAccess.READ_WRITE;
			break;

		case OpInfo0.READ_COND_WRITE:
			op0Access = OpAccess.READ_COND_WRITE;
			break;

		case OpInfo0.NO_MEM_ACCESS:
			op0Access = OpAccess.NO_MEM_ACCESS;
			break;

		case OpInfo0.WRITE_MEM_READ_WRITE_REG:
			if (instruction.getOp0Kind() != OpKind.REGISTER || instruction.getOp1Kind() != OpKind.REGISTER)
				op0Access = OpAccess.WRITE;
			else
				op0Access = OpAccess.READ_WRITE;
			break;
		}

		assert instruction.getOpCount() <= IcedConstants.MAX_OP_COUNT : instruction.getOpCount();
		info.opAccesses[0] = (byte)op0Access;
		int op1Info = (flags1 >>> InfoFlags1.OP_INFO1_SHIFT) & InfoFlags1.OP_INFO1_MASK;
		info.opAccesses[1] = (byte)OpAccessTables.op1[op1Info];
		info.opAccesses[2] = (byte)OpAccessTables.op2[(flags1 >>> InfoFlags1.OP_INFO2_SHIFT) & InfoFlags1.OP_INFO2_MASK];
		if ((flags1 & ((InfoFlags1.OP_INFO3_MASK) << InfoFlags1.OP_INFO3_SHIFT)) != 0)
			info.opAccesses[3] = (byte)OpAccess.READ;
		else
			info.opAccesses[3] = (byte)OpAccess.NONE;
		if ((flags1 & ((InfoFlags1.OP_INFO4_MASK) << InfoFlags1.OP_INFO4_SHIFT)) != 0)
			info.opAccesses[4] = (byte)OpAccess.READ;
		else
			info.opAccesses[4] = (byte)OpAccess.NONE;

		int opCount = instruction.getOpCount();
		for (int i = 0; i < opCount; i++) {
			int access;
			access = info.opAccesses[i];
			if (access == OpAccess.NONE)
				continue;

			switch (instruction.getOpKind(i)) {
			case OpKind.REGISTER:
				if (access == OpAccess.NO_MEM_ACCESS) {
					access = OpAccess.READ;
					info.opAccesses[i] = (byte)OpAccess.READ;
				}
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					if (i == 0 && op0Info == OpInfo0.WRITE_FORCE_P1) {
						int reg = instruction.getOp0Register();
						addRegister(flags, reg, access);
						if (Register.K0 <= reg && reg <= Register.K7)
							addRegister(flags, ((reg - Register.K0) ^ 1) + Register.K0, access);
					}
					else if (i == 1 && op1Info == OpInfo1.READ_P3) {
						int reg = instruction.getOp1Register();
						if (Register.XMM0 <= reg && reg <= IcedConstants.VMM_LAST) {
							reg = IcedConstants.VMM_FIRST + ((reg - IcedConstants.VMM_FIRST) & ~3);
							for (int j = 0; j < 4; j++)
								addRegister(flags, reg + j, access);
						}
					}
					else
						addRegister(flags, instruction.getOpRegister(i), access);
				}
				break;

			case OpKind.MEMORY:
				int segReg = instruction.getMemorySegment() & ~(flags1 >> 31);
				int baseReg = instruction.getMemoryBase();
				if (baseReg == Register.RIP) {
					if ((flags & Flags.NO_MEMORY_USAGE) == 0)
						addMemory(segReg, Register.NONE, Register.NONE, 1, instruction.getMemoryDisplacement64(), instruction.getMemorySize(), access,
								CodeSize.CODE64, 0);
					if ((flags & Flags.NO_REGISTER_USAGE) == 0 && segReg != Register.NONE)
						addMemorySegmentRegister(flags, segReg, OpAccess.READ);
				}
				else if (baseReg == Register.EIP) {
					if ((flags & Flags.NO_MEMORY_USAGE) == 0)
						addMemory(segReg, Register.NONE, Register.NONE, 1, instruction.getMemoryDisplacement32() & 0xFFFF_FFFFL, instruction.getMemorySize(), access,
								CodeSize.CODE32, 0);
					if ((flags & Flags.NO_REGISTER_USAGE) == 0 && segReg != Register.NONE)
						addMemorySegmentRegister(flags, segReg, OpAccess.READ);
				}
				else {
					int scale;
					int indexReg;
					if ((flags1 & InfoFlags1.IGNORES_INDEX_VA) != 0) {
						indexReg = instruction.getMemoryIndex();
						if ((flags & Flags.NO_REGISTER_USAGE) == 0 && indexReg != Register.NONE)
							addRegister(flags, indexReg, OpAccess.READ);
						indexReg = Register.NONE;
						scale = 1;
					}
					else {
						indexReg = instruction.getMemoryIndex();
						scale = instruction.getMemoryIndexScale();
					}
					if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
						@SuppressWarnings("deprecation")
						int addrSizeBytes = com.github.icedland.iced.x86.InternalInstructionUtils.getAddressSizeInBytes(baseReg, indexReg,
								instruction.getMemoryDisplSize(), codeSize);
						int addrSize;
						switch (addrSizeBytes) {
						case 2:
							addrSize = CodeSize.CODE16;
							break;
						case 4:
							addrSize = CodeSize.CODE32;
							break;
						case 8:
							addrSize = CodeSize.CODE64;
							break;
						default:
							addrSize = CodeSize.UNKNOWN;
							break;
						}
						int vsibSize = 0;
						if (Register.isVectorRegister(indexReg)) {
							int vsib = instruction.getVsib();
							vsibSize = (vsib & VsibFlags.VSIB64) != 0 ? 8 : 4;
						}
						long displ;
						if (addrSizeBytes == 8)
							displ = instruction.getMemoryDisplacement64();
						else
							displ = instruction.getMemoryDisplacement32() & 0xFFFF_FFFFL;
						addMemory(segReg, baseReg, indexReg, scale, displ, instruction.getMemorySize(), access, addrSize, vsibSize);
					}
					if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
						if (segReg != Register.NONE)
							addMemorySegmentRegister(flags, segReg, OpAccess.READ);
						if (baseReg != Register.NONE)
							addRegister(flags, baseReg, OpAccess.READ);
						if (indexReg != Register.NONE)
							addRegister(flags, indexReg, OpAccess.READ);
					}
				}
				break;
			}
		}

		int impliedAccess = (flags1 >>> InfoFlags1.IMPLIED_ACCESS_SHIFT) & InfoFlags1.IMPLIED_ACCESS_MASK;
		if (impliedAccess != ImpliedAccess.NONE)
			addImpliedAccesses(impliedAccess, instruction, flags);

		if (instruction.hasOpMask() && (flags & Flags.NO_REGISTER_USAGE) == 0)
			addRegister(flags, instruction.getOpMask(), (flags1 & InfoFlags1.OP_MASK_READ_WRITE) != 0 ? OpAccess.READ_WRITE : OpAccess.READ);

		return info;
	}

	private int getXSP(int codeSize) {
		if (codeSize == CodeSize.CODE64 || codeSize == CodeSize.UNKNOWN) {
			xspMask = 0xFFFF_FFFF_FFFF_FFFFL;
			xspAddressSize = CodeSize.CODE64;
			return Register.RSP;
		}
		if (codeSize == CodeSize.CODE32) {
			xspMask = 0xFFFF_FFFFL;
			xspAddressSize = CodeSize.CODE32;
			return Register.ESP;
		}
		assert codeSize == CodeSize.CODE16 : codeSize;
		xspMask = 0xFFFF;
		xspAddressSize = CodeSize.CODE16;
		return Register.SP;
	}

	private void addImpliedAccesses(int impliedAccess, Instruction instruction, int flags) {
		assert impliedAccess != ImpliedAccess.NONE : impliedAccess;
		switch (impliedAccess) {
		// GENERATOR-BEGIN: ImpliedAccessHandler
		// ⚠️This was generated by GENERATOR!🦹‍♂️
		case ImpliedAccess.NONE:
			break;
		case ImpliedAccess.SHIFT_IB_MASK1_FMOD9:
			break;
		case ImpliedAccess.SHIFT_IB_MASK1_FMOD11:
			break;
		case ImpliedAccess.SHIFT_IB_MASK1_F:
			break;
		case ImpliedAccess.SHIFT_IB_MASK3_F:
			break;
		case ImpliedAccess.CLEAR_RFLAGS:
			commandClearRflags(instruction, flags);
			break;
		case ImpliedAccess.T_PUSH1X2:
			commandPush(instruction, flags, 1, 2);
			break;
		case ImpliedAccess.T_PUSH1X4:
			commandPush(instruction, flags, 1, 4);
			break;
		case ImpliedAccess.T_POP1X2:
			commandPop(instruction, flags, 1, 2);
			break;
		case ImpliedAccess.T_POP1X4:
			commandPop(instruction, flags, 1, 4);
			break;
		case ImpliedAccess.T_RWAL:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AL, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_PUSH1X8:
			commandPush(instruction, flags, 1, 8);
			break;
		case ImpliedAccess.T_POP1X8:
			commandPop(instruction, flags, 1, 8);
			break;
		case ImpliedAccess.T_PUSHA2:
			commandPusha(instruction, flags, 2);
			break;
		case ImpliedAccess.T_PUSHA4:
			commandPusha(instruction, flags, 4);
			break;
		case ImpliedAccess.T_POPA2:
			commandPopa(instruction, flags, 2);
			break;
		case ImpliedAccess.T_POPA4:
			commandPopa(instruction, flags, 4);
			break;
		case ImpliedAccess.T_ARPL:
			commandArpl(instruction, flags);
			break;
		case ImpliedAccess.T_INS:
			commandIns(instruction, flags);
			break;
		case ImpliedAccess.T_OUTS:
			commandOuts(instruction, flags);
			break;
		case ImpliedAccess.T_LEA:
			commandLea(instruction, flags);
			break;
		case ImpliedAccess.T_GPR16:
			commandLastGpr(instruction, flags, Register.AX);
			break;
		case ImpliedAccess.T_POPRM2:
			commandPopRm(instruction, flags, 2);
			break;
		case ImpliedAccess.T_POPRM4:
			commandPopRm(instruction, flags, 4);
			break;
		case ImpliedAccess.T_POPRM8:
			commandPopRm(instruction, flags, 8);
			break;
		case ImpliedAccess.T_RAL_WAH:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AL, OpAccess.READ);
				addRegister(flags, Register.AH, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RAX_WEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RWEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RAX_WDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.DX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_WEDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RRAX_WRDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ);
				addRegister(flags, Register.RDX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_PUSH2X2:
			commandPush(instruction, flags, 2, 2);
			break;
		case ImpliedAccess.T_PUSH2X4:
			commandPush(instruction, flags, 2, 4);
			break;
		case ImpliedAccess.T_RAH:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AH, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WAH:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AH, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_MOVS:
			commandMovs(instruction, flags);
			break;
		case ImpliedAccess.T_CMPS:
			commandCmps(instruction, flags);
			break;
		case ImpliedAccess.T_STOS:
			commandStos(instruction, flags);
			break;
		case ImpliedAccess.T_LODS:
			commandLods(instruction, flags);
			break;
		case ImpliedAccess.T_SCAS:
			commandScas(instruction, flags);
			break;
		case ImpliedAccess.T_WES:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ES, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WDS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.DS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_CWEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.COND_WRITE);
			}
			break;
		case ImpliedAccess.T_ENTER2:
			commandEnter(instruction, flags, 2);
			break;
		case ImpliedAccess.T_ENTER4:
			commandEnter(instruction, flags, 4);
			break;
		case ImpliedAccess.T_ENTER8:
			commandEnter(instruction, flags, 8);
			break;
		case ImpliedAccess.T_LEAVE2:
			commandLeave(instruction, flags, 2);
			break;
		case ImpliedAccess.T_LEAVE4:
			commandLeave(instruction, flags, 4);
			break;
		case ImpliedAccess.T_LEAVE8:
			commandLeave(instruction, flags, 8);
			break;
		case ImpliedAccess.T_POP2X2:
			commandPop(instruction, flags, 2, 2);
			break;
		case ImpliedAccess.T_POP2X4:
			commandPop(instruction, flags, 2, 4);
			break;
		case ImpliedAccess.T_POP2X8:
			commandPop(instruction, flags, 2, 8);
			break;
		case ImpliedAccess.B64_T_WSS_POP5X2_F_POP3X2:
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.SS, OpAccess.WRITE);
				}
				commandPop(instruction, flags, 5, 2);
			}
			else {
				commandPop(instruction, flags, 3, 2);
			}
			break;
		case ImpliedAccess.B64_T_WSS_POP5X4_F_POP3X4:
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.SS, OpAccess.WRITE);
				}
				commandPop(instruction, flags, 5, 4);
			}
			else {
				commandPop(instruction, flags, 3, 4);
			}
			break;
		case ImpliedAccess.T_WSS_POP5X8:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.SS, OpAccess.WRITE);
			}
			commandPop(instruction, flags, 5, 8);
			break;
		case ImpliedAccess.T_RAL_WAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AL, OpAccess.READ);
				addRegister(flags, Register.AX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WAL:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AL, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RWST0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ST0, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RST0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ST0, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RST0_RWST1:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ST0, OpAccess.READ);
				addRegister(flags, Register.ST1, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RCWST0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ST0, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_RST1_RWST0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ST1, OpAccess.READ);
				addRegister(flags, Register.ST0, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RST0_RST1:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ST0, OpAccess.READ);
				addRegister(flags, Register.ST1, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WST0_TOST7_WMM0_TOMM7:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				for (int reg = Register.ST0; reg <= Register.ST7; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
				for (int reg = Register.MM0; reg <= Register.MM7; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RST0_TOST7_RMM0_TOMM7:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				for (int reg = Register.ST0; reg <= Register.ST7; reg++)
					addRegister(flags, reg, OpAccess.READ);
				for (int reg = Register.MM0; reg <= Register.MM7; reg++)
					addRegister(flags, reg, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RWCX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWECX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWRCX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RCX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RECX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RRCX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WDX_RWAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.DX, OpAccess.WRITE);
				addRegister(flags, Register.AX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_WEDX_RWEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EDX, OpAccess.WRITE);
				addRegister(flags, Register.EAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_WRDX_RWRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RDX, OpAccess.WRITE);
				addRegister(flags, Register.RAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWAX_RWDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ_WRITE);
				addRegister(flags, Register.DX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWEAX_RWEDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ_WRITE);
				addRegister(flags, Register.EDX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWRAX_RWRDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ_WRITE);
				addRegister(flags, Register.RDX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_PUSH2X8:
			commandPush(instruction, flags, 2, 8);
			break;
		case ImpliedAccess.T_RCR0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CR0, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RWCR0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CR0, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_GPR16_RWCR0:
			commandLastGpr(instruction, flags, Register.AX);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CR0, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RCWEAX_B64_T_CRRCX_CRRDX_CRRBX_CWRCX_CWRDX_CWRBX_F_CRECX_CREDX_CREBX_CRDS_CWECX_CWEDX_CWEBX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ_COND_WRITE);
			}
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.RCX, OpAccess.COND_READ);
					addRegister(flags, Register.RDX, OpAccess.COND_READ);
					addRegister(flags, Register.RBX, OpAccess.COND_READ);
					addRegister(flags, Register.RCX, OpAccess.COND_WRITE);
					addRegister(flags, Register.RDX, OpAccess.COND_WRITE);
					addRegister(flags, Register.RBX, OpAccess.COND_WRITE);
				}
			}
			else {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.ECX, OpAccess.COND_READ);
					addRegister(flags, Register.EDX, OpAccess.COND_READ);
					addRegister(flags, Register.EBX, OpAccess.COND_READ);
					addRegister(flags, Register.DS, OpAccess.COND_READ);
					addRegister(flags, Register.ECX, OpAccess.COND_WRITE);
					addRegister(flags, Register.EDX, OpAccess.COND_WRITE);
					addRegister(flags, Register.EBX, OpAccess.COND_WRITE);
				}
			}
			break;
		case ImpliedAccess.T_CWECX_CWEDX_CWEBX_RWEAX_B64_T_CRRCX_CRRDX_CRRBX_F_CRECX_CREDX_CREBX_CRDS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EBX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EAX, OpAccess.READ_WRITE);
			}
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.RCX, OpAccess.COND_READ);
					addRegister(flags, Register.RDX, OpAccess.COND_READ);
					addRegister(flags, Register.RBX, OpAccess.COND_READ);
				}
			}
			else {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.ECX, OpAccess.COND_READ);
					addRegister(flags, Register.EDX, OpAccess.COND_READ);
					addRegister(flags, Register.EBX, OpAccess.COND_READ);
					addRegister(flags, Register.DS, OpAccess.COND_READ);
				}
			}
			break;
		case ImpliedAccess.T_RAX_RECX_REDX_RSEG:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REAX_RECX_REDX_RSEG:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RECX_REDX_RRAX_RSEG:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.RAX, OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REAX_RECX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RECX_WEAX_WEDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.WRITE);
				addRegister(flags, Register.EDX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_RECX_REDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RAX_WFS_WGS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.FS, OpAccess.WRITE);
				addRegister(flags, Register.GS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_WFS_WGS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.FS, OpAccess.WRITE);
				addRegister(flags, Register.GS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RRAX_WFS_WGS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ);
				addRegister(flags, Register.FS, OpAccess.WRITE);
				addRegister(flags, Register.GS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RAX_RFS_RGS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.FS, OpAccess.READ);
				addRegister(flags, Register.GS, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REAX_RFS_RGS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.FS, OpAccess.READ);
				addRegister(flags, Register.GS, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RRAX_RFS_RGS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ);
				addRegister(flags, Register.FS, OpAccess.READ);
				addRegister(flags, Register.GS, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REAX_WCR0_WDR6_WDR7_WES_TOGS_WCR2_TOCR4_WDR0_TODR3_B64_T_WRAX_TOR15_F_WEAX_TOEDI:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.CR0, OpAccess.WRITE);
				addRegister(flags, Register.DR6, OpAccess.WRITE);
				addRegister(flags, Register.DR7, OpAccess.WRITE);
				for (int reg = Register.ES; reg <= Register.GS; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
				for (int reg = Register.CR2; reg <= Register.CR4; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
				for (int reg = Register.DR0; reg <= Register.DR3; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
			}
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					for (int reg = Register.RAX; reg <= Register.R15; reg++)
						addRegister(flags, reg, OpAccess.WRITE);
				}
			}
			else {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					for (int reg = Register.EAX; reg <= Register.EDI; reg++)
						addRegister(flags, reg, OpAccess.WRITE);
				}
			}
			break;
		case ImpliedAccess.T_RAX_RECX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RECX_RRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.RAX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WEAX_WECX_WEDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.WRITE);
				addRegister(flags, Register.ECX, OpAccess.WRITE);
				addRegister(flags, Register.EDX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_RECX_CREBX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EBX, OpAccess.COND_READ);
			}
			break;
		case ImpliedAccess.T_RAX_RSEG:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REAX_RSEG:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RRAX_RSEG:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WECX_B64_T_WR11:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.WRITE);
			}
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.R11, OpAccess.WRITE);
				}
			}
			break;
		case ImpliedAccess.T_REDI_RES:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EDI, OpAccess.READ);
				addRegister(flags, Register.ES, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RECX_WCS_WSS_B64_T_RR11D:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.CS, OpAccess.WRITE);
				addRegister(flags, Register.SS, OpAccess.WRITE);
			}
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					addRegister(flags, Register.R11D, OpAccess.READ);
				}
			}
			break;
		case ImpliedAccess.T_RR11D_RRCX_WCS_WSS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.R11D, OpAccess.READ);
				addRegister(flags, Register.RCX, OpAccess.READ);
				addRegister(flags, Register.CS, OpAccess.WRITE);
				addRegister(flags, Register.SS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WEAX_WEDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.WRITE);
				addRegister(flags, Register.EDX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WESP:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ESP, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RECX_REDX_WESP_WCS_WSS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.ESP, OpAccess.WRITE);
				addRegister(flags, Register.CS, OpAccess.WRITE);
				addRegister(flags, Register.SS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RRCX_RRDX_WRSP_WCS_WSS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ);
				addRegister(flags, Register.RDX, OpAccess.READ);
				addRegister(flags, Register.RSP, OpAccess.WRITE);
				addRegister(flags, Register.CS, OpAccess.WRITE);
				addRegister(flags, Register.SS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_ZRRM:
			commandClearRegRegmem(instruction, flags);
			break;
		case ImpliedAccess.T_ZRRRM:
			commandClearRegRegRegmem(instruction, flags);
			break;
		case ImpliedAccess.B64_T_RWXMM0_TOXMM15_F_RWXMM0_TOXMM7:
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					for (int reg = Register.XMM0; reg <= Register.XMM15; reg++)
						addRegister(flags, reg, OpAccess.READ_WRITE);
				}
			}
			else {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					for (int reg = Register.XMM0; reg <= Register.XMM7; reg++)
						addRegister(flags, reg, OpAccess.READ_WRITE);
				}
			}
			break;
		case ImpliedAccess.B64_T_WZMM0_TOZMM15_F_WZMM0_TOZMM7:
			if ((flags & Flags.IS_64_BIT) != 0) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					for (int reg = Register.ZMM0; reg <= Register.ZMM15; reg++)
						addRegister(flags, reg, OpAccess.WRITE);
				}
			}
			else {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
					for (int reg = Register.ZMM0; reg <= Register.ZMM7; reg++)
						addRegister(flags, reg, OpAccess.WRITE);
				}
			}
			break;
		case ImpliedAccess.T_CRECX_WECX_WEDX_WEBX_RWEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.COND_READ);
				addRegister(flags, Register.ECX, OpAccess.WRITE);
				addRegister(flags, Register.EDX, OpAccess.WRITE);
				addRegister(flags, Register.EBX, OpAccess.WRITE);
				addRegister(flags, Register.EAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRSI_CREAX_CRES_CWEAX_CWEDX_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.SI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.SI, OpAccess.COND_READ);
				addRegister(flags, Register.EAX, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.EAX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDX, OpAccess.COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CREAX_CRESI_CRES_CWEAX_CWEDX_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.ESI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.EAX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDX, OpAccess.COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CREAX_CRRSI_CRES_CWEAX_CWEDX_RCWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.EAX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDX, OpAccess.COND_WRITE);
				addRegister(flags, Register.RCX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRSI_CRDI_CRES_CWSI_RCWAX_RCWCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.SI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE16, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.SI, OpAccess.COND_READ);
				addRegister(flags, Register.DI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_WRITE);
				addRegister(flags, Register.AX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.CX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRESI_CREDI_CRES_CWESI_RCWEAX_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.ESI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE32, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ESI, OpAccess.COND_READ);
				addRegister(flags, Register.EDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_WRITE);
				addRegister(flags, Register.EAX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRRSI_CRRDI_CRES_CWRSI_RCWRAX_RCWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RAX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.RCX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_RCL_RAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CL, OpAccess.READ);
				addRegister(flags, Register.AX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RCL_REAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CL, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_XSTORE2:
			commandXstore(instruction, flags, 2);
			break;
		case ImpliedAccess.T_XSTORE4:
			commandXstore(instruction, flags, 4);
			break;
		case ImpliedAccess.T_XSTORE8:
			commandXstore(instruction, flags, 8);
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CWMEM_CRDX_CRBX_CRSI_CRDI_CRES_CWSI_CWDI_RCWCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.DX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.BX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.SI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE16, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.DX, OpAccess.COND_READ);
				addRegister(flags, Register.BX, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_READ);
				addRegister(flags, Register.DI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_WRITE);
				addRegister(flags, Register.DI, OpAccess.COND_WRITE);
				addRegister(flags, Register.CX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CWMEM_CREDX_CREBX_CRESI_CREDI_CRES_CWESI_CWEDI_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.EDX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EBX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.ESI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE32, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EDX, OpAccess.COND_READ);
				addRegister(flags, Register.EBX, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_READ);
				addRegister(flags, Register.EDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDI, OpAccess.COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CWMEM_CRRDX_CRRBX_CRRSI_CRRDI_CRES_CWRSI_CWRDI_RCWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RDX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RBX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RDX, OpAccess.COND_READ);
				addRegister(flags, Register.RBX, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RDI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RCX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CRMEM_CWMEM_CWMEM_CRAX_CRDX_CRBX_CRSI_CRDI_CRES_CWSI_CWDI_RCWCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.AX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.BX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.SI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.AX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE16, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.COND_READ);
				addRegister(flags, Register.DX, OpAccess.COND_READ);
				addRegister(flags, Register.BX, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_READ);
				addRegister(flags, Register.DI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_WRITE);
				addRegister(flags, Register.DI, OpAccess.COND_WRITE);
				addRegister(flags, Register.CX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CRMEM_CWMEM_CWMEM_CREAX_CREDX_CREBX_CRESI_CREDI_CRES_CWESI_CWEDI_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.EAX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EBX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.ESI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EAX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE32, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.COND_READ);
				addRegister(flags, Register.EDX, OpAccess.COND_READ);
				addRegister(flags, Register.EBX, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_READ);
				addRegister(flags, Register.EDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDI, OpAccess.COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CRMEM_CWMEM_CWMEM_CRRAX_CRRDX_CRRBX_CRRSI_CRRDI_CRES_CWRSI_CWRDI_RCWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RAX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RBX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RAX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.COND_READ);
				addRegister(flags, Register.RDX, OpAccess.COND_READ);
				addRegister(flags, Register.RBX, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RDI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RCX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_RCWAL:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AL, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_RCWAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_RCWEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_REDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_GPR8:
			commandLastGpr(instruction, flags, Register.AL);
			break;
		case ImpliedAccess.T_GPR32_REAX_REDX:
			commandLastGpr(instruction, flags, Register.EAX);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RMEM_RSEG:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(getSegDefaultDS(instruction), instruction.getOp0Register(), Register.NONE, 1, 0x0, MemorySize.UINT8, OpAccess.READ, CodeSize.UNKNOWN, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RCWRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_WSS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.SS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WFS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.FS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WGS:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.GS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_CRECX_CREBX_RCWEAX_RCWEDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.COND_READ);
				addRegister(flags, Register.EBX, OpAccess.COND_READ);
				addRegister(flags, Register.EAX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.EDX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRRCX_CRRBX_RCWRAX_RCWRDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.COND_READ);
				addRegister(flags, Register.RBX, OpAccess.COND_READ);
				addRegister(flags, Register.RAX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.RDX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_WMEM_RAR_DI_RSEG:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(getSegDefaultDS(instruction), getARDI(instruction), Register.NONE, 1, 0x0, instruction.getMemorySize(), OpAccess.WRITE, CodeSize.UNKNOWN, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, getARDI(instruction), OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RXMM0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.XMM0, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EDX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RRDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RDX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WMEM_RES:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, instruction.getOp0Register(), Register.NONE, 1, 0x0, instruction.getMemorySize(), OpAccess.WRITE, CodeSize.UNKNOWN, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REAX_REDX_WXMM0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.XMM0, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RRAX_RRDX_WXMM0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ);
				addRegister(flags, Register.RDX, OpAccess.READ);
				addRegister(flags, Register.XMM0, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_REDX_WECX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RRAX_RRDX_WECX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ);
				addRegister(flags, Register.RDX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WXMM0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.XMM0, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WECX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RMEM_RDS:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.DS, instruction.getOp0Register(), Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.READ, CodeSize.UNKNOWN, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.DS, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RRCX_RRDX_RWRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ);
				addRegister(flags, Register.RDX, OpAccess.READ);
				addRegister(flags, Register.RAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RMEM_RRCX_RSEG_RWRAX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(getSegDefaultDS(instruction), Register.RCX, Register.NONE, 1, 0x0, MemorySize.UINT128, OpAccess.READ, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ);
				addMemorySegmentRegister(flags, getSegDefaultDS(instruction), OpAccess.READ);
				addRegister(flags, Register.RAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RAX_RECX_REDX_WEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RECX_REDX_RWEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RECX_REDX_RWRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.RAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RAX_RECX_REDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_RECX_REDX_RRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.RAX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WTMM0_TOTMM7:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				for (int reg = Register.TMM0; reg <= Register.TMM7; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_REBX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.EBX, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_REBX_WEAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EBX, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_EMMI_W:
			commandEmmi(instruction, flags, OpAccess.WRITE);
			break;
		case ImpliedAccess.T_EMMI_RW:
			commandEmmi(instruction, flags, OpAccess.READ_WRITE);
			break;
		case ImpliedAccess.T_EMMI_R:
			commandEmmi(instruction, flags, OpAccess.READ);
			break;
		case ImpliedAccess.T_CRRCX_CRRDX_CRR8_CRR9_RWRAX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.COND_READ);
				addRegister(flags, Register.RDX, OpAccess.COND_READ);
				addRegister(flags, Register.R8, OpAccess.COND_READ);
				addRegister(flags, Register.R9, OpAccess.COND_READ);
				addRegister(flags, Register.RAX, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_RWXMM0_TOXMM7:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				for (int reg = Register.XMM0; reg <= Register.XMM7; reg++)
					addRegister(flags, reg, OpAccess.READ_WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_RXMM0:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.XMM0, OpAccess.READ);
			}
			break;
		case ImpliedAccess.T_WXMM1_WXMM2_RWXMM0_WXMM4_TOXMM6:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.XMM1, OpAccess.WRITE);
				addRegister(flags, Register.XMM2, OpAccess.WRITE);
				addRegister(flags, Register.XMM0, OpAccess.READ_WRITE);
				for (int reg = Register.XMM4; reg <= Register.XMM6; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RWXMM0_RWXMM1_WXMM2_TOXMM6:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.XMM0, OpAccess.READ_WRITE);
				addRegister(flags, Register.XMM1, OpAccess.READ_WRITE);
				for (int reg = Register.XMM2; reg <= Register.XMM6; reg++)
					addRegister(flags, reg, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_POP3X8:
			commandPop(instruction, flags, 3, 8);
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRBX_CRSI_CRDI_CRES_CWSI_RCWAX_RCWCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.SI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE16, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.BX, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_READ);
				addRegister(flags, Register.DI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_WRITE);
				addRegister(flags, Register.AX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.CX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CREBX_CRESI_CREDI_CRES_CWESI_RCWEAX_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.ESI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE32, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EBX, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_READ);
				addRegister(flags, Register.EDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_WRITE);
				addRegister(flags, Register.EAX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRRBX_CRRSI_CRRDI_CRES_CWRSI_RCWRAX_RCWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RBX, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RAX, OpAccess.READ_COND_WRITE);
				addRegister(flags, Register.RCX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CWMEM_CRAX_CRDX_CRBX_CRSI_CRDI_CRES_CWSI_CWDI_RCWCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.DX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.BX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.SI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE16, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.AX, OpAccess.COND_READ);
				addRegister(flags, Register.DX, OpAccess.COND_READ);
				addRegister(flags, Register.BX, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_READ);
				addRegister(flags, Register.DI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_WRITE);
				addRegister(flags, Register.DI, OpAccess.COND_WRITE);
				addRegister(flags, Register.CX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CWMEM_CREAX_CREDX_CREBX_CRESI_CREDI_CRES_CWESI_CWEDI_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.EDX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EBX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.ESI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE32, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.COND_READ);
				addRegister(flags, Register.EDX, OpAccess.COND_READ);
				addRegister(flags, Register.EBX, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_READ);
				addRegister(flags, Register.EDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDI, OpAccess.COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CRMEM_CWMEM_CRRAX_CRRDX_CRRBX_CRRSI_CRRDI_CRES_CWRSI_CWRDI_RCWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RDX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RBX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RAX, OpAccess.COND_READ);
				addRegister(flags, Register.RDX, OpAccess.COND_READ);
				addRegister(flags, Register.RBX, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RDI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RCX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_GPR16_WGS:
			commandLastGpr(instruction, flags, Register.AX);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.GS, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_WRSP_WCS_WSS_POP6X8:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RSP, OpAccess.WRITE);
				addRegister(flags, Register.CS, OpAccess.WRITE);
				addRegister(flags, Register.SS, OpAccess.WRITE);
			}
			commandPop(instruction, flags, 6, 8);
			break;
		case ImpliedAccess.T_RCS_RSS_WRSP_POP6X8:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.CS, OpAccess.READ);
				addRegister(flags, Register.SS, OpAccess.READ);
				addRegister(flags, Register.RSP, OpAccess.WRITE);
			}
			commandPop(instruction, flags, 6, 8);
			break;
		case ImpliedAccess.T_REAX_RECX_WEDX_WEBX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.WRITE);
				addRegister(flags, Register.EBX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_REAX_RECX_REDX_CREBX_CWEDX_CWEBX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.EAX, OpAccess.READ);
				addRegister(flags, Register.ECX, OpAccess.READ);
				addRegister(flags, Register.EDX, OpAccess.READ);
				addRegister(flags, Register.EBX, OpAccess.COND_READ);
				addRegister(flags, Register.EDX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EBX, OpAccess.COND_WRITE);
			}
			break;
		case ImpliedAccess.T_MEMDISPLM64:
			commandMemDispl(flags, -64);
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRSI_CRDI_CRES_CWSI_RCWCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.SI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE16, 0);
				addMemory(Register.ES, Register.DI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE16, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.SI, OpAccess.COND_READ);
				addRegister(flags, Register.DI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.SI, OpAccess.COND_WRITE);
				addRegister(flags, Register.CX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRESI_CREDI_CRES_CWESI_RCWECX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.ESI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE32, 0);
				addMemory(Register.ES, Register.EDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE32, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.ESI, OpAccess.COND_READ);
				addRegister(flags, Register.EDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.ESI, OpAccess.COND_WRITE);
				addRegister(flags, Register.ECX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_CWMEM_CRRSI_CRRDI_CRES_CWRSI_RCWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, Register.RSI, OpAccess.COND_WRITE);
				addRegister(flags, Register.RCX, OpAccess.READ_COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CRMEM_RRCX_CRRSI_CRRDI_CRES_CRDS_CWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.DS, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ);
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.DS, OpAccess.COND_READ);
				addRegister(flags, Register.RCX, OpAccess.COND_WRITE);
			}
			break;
		case ImpliedAccess.T_CRMEM_CWMEM_RRCX_CRRSI_CRRDI_CRES_CRDS_CWRCX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.DS, Register.RSI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_READ, CodeSize.CODE64, 0);
				addMemory(Register.ES, Register.RDI, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ);
				addRegister(flags, Register.RSI, OpAccess.COND_READ);
				addRegister(flags, Register.RDI, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.DS, OpAccess.COND_READ);
				addRegister(flags, Register.RCX, OpAccess.COND_WRITE);
			}
			break;
		case ImpliedAccess.T_RDL_RRAX_WEAX_WRCX_WRDX:
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.DL, OpAccess.READ);
				addRegister(flags, Register.RAX, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.WRITE);
				addRegister(flags, Register.RCX, OpAccess.WRITE);
				addRegister(flags, Register.RDX, OpAccess.WRITE);
			}
			break;
		case ImpliedAccess.T_RMEM_WMEM_RRCX_RRBX_RDS_WEAX:
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.DS, Register.RBX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.READ, CodeSize.CODE64, 0);
				addMemory(Register.DS, Register.RCX, Register.NONE, 1, 0x0, MemorySize.UNKNOWN, OpAccess.WRITE, CodeSize.CODE64, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, Register.RCX, OpAccess.READ);
				addRegister(flags, Register.RBX, OpAccess.READ);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.DS, OpAccess.READ);
				addRegister(flags, Register.EAX, OpAccess.WRITE);
			}
			break;
		// GENERATOR-END: ImpliedAccessHandler

		default:
			throw new UnsupportedOperationException();
		}
	}

	private static int getARDI(Instruction instruction) {
		switch (instruction.getOp0Kind()) {
		case OpKind.MEMORY_SEG_DI:
			return Register.DI;
		case OpKind.MEMORY_SEG_EDI:
			return Register.EDI;
		default:
			return Register.RDI;
		}
	}

	private static int getSegDefaultDS(Instruction instruction) {
		int seg = instruction.getSegmentPrefix();
		return seg == Register.NONE ? Register.DS : seg;
	}

	private void commandPush(Instruction instruction, int flags, int count, int opSize) {
		assert count > 0 : count;
		int xsp = getXSP(instruction.getCodeSize());
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if ((flags & Flags.IS_64_BIT) == 0)
				addRegister(flags, Register.SS, OpAccess.READ);
			addRegister(flags, xsp, OpAccess.READ_WRITE);
		}
		if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
			int memSize;
			if (opSize == 8)
				memSize = MemorySize.UINT64;
			else if (opSize == 4)
				memSize = MemorySize.UINT32;
			else {
				assert opSize == 2 : opSize;
				memSize = MemorySize.UINT16;
			}
			long offset = -opSize;
			for (int i = 0; i < count; i++, offset -= opSize)
				addMemory(Register.SS, xsp, Register.NONE, 1, offset & xspMask, memSize, OpAccess.WRITE, xspAddressSize, 0);
		}
	}

	private void commandPop(Instruction instruction, int flags, int count, int opSize) {
		assert count > 0 : count;
		int xsp = getXSP(instruction.getCodeSize());
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if ((flags & Flags.IS_64_BIT) == 0)
				addRegister(flags, Register.SS, OpAccess.READ);
			addRegister(flags, xsp, OpAccess.READ_WRITE);
		}
		if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
			int memSize;
			if (opSize == 8)
				memSize = MemorySize.UINT64;
			else if (opSize == 4)
				memSize = MemorySize.UINT32;
			else {
				assert opSize == 2 : opSize;
				memSize = MemorySize.UINT16;
			}
			long offset = 0;
			for (int i = 0; i < count; i++, offset += opSize)
				addMemory(Register.SS, xsp, Register.NONE, 1, offset, memSize, OpAccess.READ, xspAddressSize, 0);
		}
	}

	private void commandPopRm(Instruction instruction, int flags, int opSize) {
		int xsp = getXSP(instruction.getCodeSize());
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if ((flags & Flags.IS_64_BIT) == 0)
				addRegister(flags, Register.SS, OpAccess.READ);
			addRegister(flags, xsp, OpAccess.READ_WRITE);
		}
		if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
			int memSize;
			if (opSize == 8)
				memSize = MemorySize.UINT64;
			else if (opSize == 4)
				memSize = MemorySize.UINT32;
			else {
				assert opSize == 2 : opSize;
				memSize = MemorySize.UINT16;
			}
			if (instruction.getOp0Kind() == OpKind.MEMORY) {
				assert info.usedMemoryLocations.size() == 1 : info.usedMemoryLocations.size();
				if (instruction.getMemoryBase() == Register.RSP || instruction.getMemoryBase() == Register.ESP) {
					UsedMemory mem = info.usedMemoryLocations.get(0);
					long displ = mem.getDisplacement() + opSize;
					if (instruction.getMemoryBase() == Register.ESP)
						displ = displ & 0xFFFF_FFFFL;
					info.usedMemoryLocations.set(0, new UsedMemory(mem.getSegment(), mem.getBase(), mem.getIndex(), mem.getScale(), displ,
							mem.getMemorySize(), mem.getAccess(), mem.getAddressSize(), mem.getVsibSize()));
				}
			}
			addMemory(Register.SS, xsp, Register.NONE, 1, 0, memSize, OpAccess.READ, xspAddressSize, 0);
		}
	}

	private void commandPusha(Instruction instruction, int flags, int opSize) {
		int xsp = getXSP(instruction.getCodeSize());
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if ((flags & Flags.IS_64_BIT) == 0)
				addRegister(flags, Register.SS, OpAccess.READ);
			addRegister(flags, xsp, OpAccess.READ_WRITE);
		}
		long displ;
		int memSize;
		int baseReg;
		if (opSize == 4) {
			displ = -4;
			memSize = MemorySize.UINT32;
			baseReg = Register.EAX;
		}
		else {
			assert opSize == 2 : opSize;
			displ = -2;
			memSize = MemorySize.UINT16;
			baseReg = Register.AX;
		}
		for (int i = 0; i < 8; i++) {
			if ((flags & Flags.NO_REGISTER_USAGE) == 0)
				addRegister(flags, baseReg + i, OpAccess.READ);
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.SS, xsp, Register.NONE, 1, (displ * (i + 1)) & xspMask, memSize, OpAccess.WRITE, xspAddressSize, 0);
		}
	}

	private void commandPopa(Instruction instruction, int flags, int opSize) {
		int xsp = getXSP(instruction.getCodeSize());
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if ((flags & Flags.IS_64_BIT) == 0)
				addRegister(flags, Register.SS, OpAccess.READ);
			addRegister(flags, xsp, OpAccess.READ_WRITE);
		}
		int memSize;
		int baseReg;
		if (opSize == 4) {
			memSize = MemorySize.UINT32;
			baseReg = Register.EAX;
		}
		else {
			assert opSize == 2 : opSize;
			memSize = MemorySize.UINT16;
			baseReg = Register.AX;
		}
		for (int i = 0; i < 8; i++) {
			// Ignore eSP
			if (i != 3) {
				if ((flags & Flags.NO_REGISTER_USAGE) == 0)
					addRegister(flags, baseReg + 7 - i, OpAccess.WRITE);
				if ((flags & Flags.NO_MEMORY_USAGE) == 0)
					addMemory(Register.SS, xsp, Register.NONE, 1, (opSize * i) & xspMask, memSize, OpAccess.READ, xspAddressSize, 0);
			}
		}
	}

	private void commandIns(Instruction instruction, int flags) {
		int addressSize;
		int rDI, rCX;
		switch (instruction.getOp0Kind()) {
		case OpKind.MEMORY_ESDI:
			addressSize = CodeSize.CODE16;
			rDI = Register.DI;
			rCX = Register.CX;
			break;
		case OpKind.MEMORY_ESEDI:
			addressSize = CodeSize.CODE32;
			rDI = Register.EDI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rDI = Register.RDI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			info.opAccesses[0] = (byte)OpAccess.COND_WRITE;
			info.opAccesses[1] = (byte)OpAccess.COND_READ;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				assert info.usedRegisters.size() == 1 : info.usedRegisters.size();
				info.usedRegisters.set(0, new UsedRegister(Register.DX, OpAccess.COND_READ));
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_WRITE);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.WRITE, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.READ);
				addRegister(flags, rDI, OpAccess.READ_WRITE);
			}
		}
	}

	private void commandOuts(Instruction instruction, int flags) {
		int addressSize;
		int rSI, rCX;
		switch (instruction.getOp1Kind()) {
		case OpKind.MEMORY_SEG_SI:
			addressSize = CodeSize.CODE16;
			rSI = Register.SI;
			rCX = Register.CX;
			break;
		case OpKind.MEMORY_SEG_ESI:
			addressSize = CodeSize.CODE32;
			rSI = Register.ESI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rSI = Register.RSI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			info.opAccesses[0] = (byte)OpAccess.COND_READ;
			info.opAccesses[1] = (byte)OpAccess.COND_READ;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_READ, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				assert info.usedRegisters.size() == 1 : info.usedRegisters.size();
				info.usedRegisters.set(0, new UsedRegister(Register.DX, OpAccess.COND_READ));
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_WRITE);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.READ, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.READ);
				addRegister(flags, rSI, OpAccess.READ_WRITE);
			}
		}
	}

	private void commandMovs(Instruction instruction, int flags) {
		int addressSize;
		int rSI, rDI, rCX;
		switch (instruction.getOp0Kind()) {
		case OpKind.MEMORY_ESDI:
			addressSize = CodeSize.CODE16;
			rSI = Register.SI;
			rDI = Register.DI;
			rCX = Register.CX;
			break;
		case OpKind.MEMORY_ESEDI:
			addressSize = CodeSize.CODE32;
			rSI = Register.ESI;
			rDI = Register.EDI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rSI = Register.RSI;
			rDI = Register.RDI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			info.opAccesses[0] = (byte)OpAccess.COND_WRITE;
			info.opAccesses[1] = (byte)OpAccess.COND_READ;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, addressSize, 0);
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_READ, addressSize, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_WRITE);
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_WRITE);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.WRITE, addressSize, 0);
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.READ, addressSize, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.READ);
				addRegister(flags, rDI, OpAccess.READ_WRITE);
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.READ);
				addRegister(flags, rSI, OpAccess.READ_WRITE);
			}
		}
	}

	private void commandCmps(Instruction instruction, int flags) {
		int addressSize;
		int rSI, rDI, rCX;
		switch (instruction.getOp0Kind()) {
		case OpKind.MEMORY_SEG_SI:
			addressSize = CodeSize.CODE16;
			rSI = Register.SI;
			rDI = Register.DI;
			rCX = Register.CX;
			break;
		case OpKind.MEMORY_SEG_ESI:
			addressSize = CodeSize.CODE32;
			rSI = Register.ESI;
			rDI = Register.EDI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rSI = Register.RSI;
			rDI = Register.RDI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			info.opAccesses[0] = (byte)OpAccess.COND_READ;
			info.opAccesses[1] = (byte)OpAccess.COND_READ;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_READ, addressSize, 0);
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_READ, addressSize, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_WRITE);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_WRITE);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.READ, addressSize, 0);
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.READ, addressSize, 0);
			}
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.READ);
				addRegister(flags, rSI, OpAccess.READ_WRITE);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.READ);
				addRegister(flags, rDI, OpAccess.READ_WRITE);
			}
		}
	}

	private void commandStos(Instruction instruction, int flags) {
		int addressSize;
		int rDI, rCX;
		switch (instruction.getOp0Kind()) {
		case OpKind.MEMORY_ESDI:
			addressSize = CodeSize.CODE16;
			rDI = Register.DI;
			rCX = Register.CX;
			break;
		case OpKind.MEMORY_ESEDI:
			addressSize = CodeSize.CODE32;
			rDI = Register.EDI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rDI = Register.RDI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			info.opAccesses[0] = (byte)OpAccess.COND_WRITE;
			info.opAccesses[1] = (byte)OpAccess.COND_READ;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				assert info.usedRegisters.size() == 1 : info.usedRegisters.size();
				info.usedRegisters.set(0, new UsedRegister(info.usedRegisters.get(0).getRegister(), OpAccess.COND_READ));
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_WRITE);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.WRITE, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.READ);
				addRegister(flags, rDI, OpAccess.READ_WRITE);
			}
		}
	}

	private void commandLods(Instruction instruction, int flags) {
		int addressSize;
		int rSI, rCX;
		switch (instruction.getOp1Kind()) {
		case OpKind.MEMORY_SEG_SI:
			addressSize = CodeSize.CODE16;
			rSI = Register.SI;
			rCX = Register.CX;
			break;
		case OpKind.MEMORY_SEG_ESI:
			addressSize = CodeSize.CODE32;
			rSI = Register.ESI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rSI = Register.RSI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			info.opAccesses[0] = (byte)OpAccess.COND_WRITE;
			info.opAccesses[1] = (byte)OpAccess.COND_READ;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_READ, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				assert info.usedRegisters.size() == 1 : info.usedRegisters.size();
				info.usedRegisters.set(0, new UsedRegister(info.usedRegisters.get(0).getRegister(), OpAccess.COND_WRITE));
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_READ);
				addRegister(flags, rSI, OpAccess.COND_WRITE);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(instruction.getMemorySegment(), rSI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.READ, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				addMemorySegmentRegister(flags, instruction.getMemorySegment(), OpAccess.READ);
				addRegister(flags, rSI, OpAccess.READ_WRITE);
			}
		}
	}

	private void commandScas(Instruction instruction, int flags) {
		int addressSize;
		int rDI, rCX;
		switch (instruction.getOp1Kind()) {
		case OpKind.MEMORY_ESDI:
			addressSize = CodeSize.CODE16;
			rDI = Register.DI;
			rCX = Register.CX;
			break;
		case OpKind.MEMORY_ESEDI:
			addressSize = CodeSize.CODE32;
			rDI = Register.EDI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rDI = Register.RDI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			info.opAccesses[0] = (byte)OpAccess.COND_READ;
			info.opAccesses[1] = (byte)OpAccess.COND_READ;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_READ, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				assert info.usedRegisters.size() == 1 : info.usedRegisters.size();
				info.usedRegisters.set(0, new UsedRegister(info.usedRegisters.get(0).getRegister(), OpAccess.COND_READ));
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_WRITE);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.READ, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.READ);
				addRegister(flags, rDI, OpAccess.READ_WRITE);
			}
		}
	}

	private void commandXstore(Instruction instruction, int flags, int size) {
		int addressSize;
		int rDI, rCX;
		switch (size) {
		case 2:
			addressSize = CodeSize.CODE16;
			rDI = Register.DI;
			rCX = Register.CX;
			break;
		case 4:
			addressSize = CodeSize.CODE32;
			rDI = Register.EDI;
			rCX = Register.ECX;
			break;
		default:
			addressSize = CodeSize.CODE64;
			rDI = Register.RDI;
			rCX = Register.RCX;
			break;
		}
		if (instruction.getRepePrefix() || instruction.getRepnePrefix()) {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, MemorySize.UNKNOWN, OpAccess.COND_WRITE, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				assert info.usedRegisters.size() == 0 : info.usedRegisters.size();
				addRegister(flags, rCX, OpAccess.READ_COND_WRITE);
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_READ);
				addRegister(flags, rDI, OpAccess.COND_WRITE);
				addRegister(flags, Register.EAX, OpAccess.COND_WRITE);
				addRegister(flags, Register.EDX, OpAccess.COND_READ);
			}
		}
		else {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.ES, rDI, Register.NONE, 1, 0, instruction.getMemorySize(), OpAccess.WRITE, addressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if ((flags & Flags.IS_64_BIT) == 0)
					addRegister(flags, Register.ES, OpAccess.READ);
				addRegister(flags, rDI, OpAccess.READ_WRITE);
				addRegister(flags, Register.EAX, OpAccess.WRITE);
				addRegister(flags, Register.EDX, OpAccess.READ);
			}
		}
	}

	private void commandEnter(Instruction instruction, int flags, int opSize) {
		int xsp = getXSP(instruction.getCodeSize());
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if ((flags & Flags.IS_64_BIT) == 0)
				addRegister(flags, Register.SS, OpAccess.READ);
			addRegister(flags, xsp, OpAccess.READ_WRITE);
		}

		int memSize;
		int rSP;
		if (opSize == 8) {
			memSize = MemorySize.UINT64;
			rSP = Register.RSP;
		}
		else if (opSize == 4) {
			memSize = MemorySize.UINT32;
			rSP = Register.ESP;
		}
		else {
			assert opSize == 2 : opSize;
			memSize = MemorySize.UINT16;
			rSP = Register.SP;
		}

		if (rSP != xsp && (flags & Flags.NO_REGISTER_USAGE) == 0)
			addRegister(flags, rSP, OpAccess.READ_WRITE);

		int nestingLevel = instruction.getImmediate8_2nd() & 0x1F;

		long xspOffset = 0;
		// push rBP
		if ((flags & Flags.NO_REGISTER_USAGE) == 0)
			addRegister(flags, rSP + 1, OpAccess.READ_WRITE);
		if ((flags & Flags.NO_MEMORY_USAGE) == 0)
			addMemory(Register.SS, xsp, Register.NONE, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.WRITE, xspAddressSize, 0);

		if (nestingLevel != 0) {
			int xbp = xsp + 1;// rBP immediately follows rSP
			long xbpOffset = 0;
			for (int i = 1; i < nestingLevel; i++) {
				if (i == 1 && rSP + 1 != xbp && (flags & Flags.NO_REGISTER_USAGE) == 0)
					addRegister(flags, xbp, OpAccess.READ_WRITE);
				// push [xbp]
				if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
					addMemory(Register.SS, xbp, Register.NONE, 1, (xbpOffset -= opSize) & xspMask, memSize, OpAccess.READ, xspAddressSize, 0);
					addMemory(Register.SS, xsp, Register.NONE, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.WRITE, xspAddressSize, 0);
				}
			}
			// push frameTemp
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.SS, xsp, Register.NONE, 1, (xspOffset -= opSize) & xspMask, memSize, OpAccess.WRITE, xspAddressSize, 0);
		}
	}

	private void commandLeave(Instruction instruction, int flags, int opSize) {
		int xsp = getXSP(instruction.getCodeSize());
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if ((flags & Flags.IS_64_BIT) == 0)
				addRegister(flags, Register.SS, OpAccess.READ);
			addRegister(flags, xsp, OpAccess.WRITE);
		}

		if (opSize == 8) {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.SS, xsp + 1, Register.NONE, 1, 0, MemorySize.UINT64, OpAccess.READ, xspAddressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if (xsp + 1 == Register.RBP)
					addRegister(flags, Register.RBP, OpAccess.READ_WRITE);
				else {
					addRegister(flags, xsp + 1, OpAccess.READ);
					addRegister(flags, Register.RBP, OpAccess.WRITE);
				}
			}
		}
		else if (opSize == 4) {
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.SS, xsp + 1, Register.NONE, 1, 0, MemorySize.UINT32, OpAccess.READ, xspAddressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if (xsp + 1 == Register.EBP)
					addRegister(flags, Register.EBP, OpAccess.READ_WRITE);
				else {
					addRegister(flags, xsp + 1, OpAccess.READ);
					addRegister(flags, Register.EBP, OpAccess.WRITE);
				}
			}
		}
		else {
			assert opSize == 2 : opSize;
			if ((flags & Flags.NO_MEMORY_USAGE) == 0)
				addMemory(Register.SS, xsp + 1, Register.NONE, 1, 0, MemorySize.UINT16, OpAccess.READ, xspAddressSize, 0);
			if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
				if (xsp + 1 == Register.BP)
					addRegister(flags, Register.BP, OpAccess.READ_WRITE);
				else {
					addRegister(flags, xsp + 1, OpAccess.READ);
					addRegister(flags, Register.BP, OpAccess.WRITE);
				}
			}
		}
	}

	private void commandClearRflags(Instruction instruction, int flags) {
		if (instruction.getOp0Register() != instruction.getOp1Register())
			return;
		if (instruction.getOp0Kind() != OpKind.REGISTER || instruction.getOp1Kind() != OpKind.REGISTER)
			return;
		info.opAccesses[0] = (byte)OpAccess.WRITE;
		info.opAccesses[1] = (byte)OpAccess.NONE;
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			assert info.usedRegisters.size() == 2 || info.usedRegisters.size() == 3 : info.usedRegisters.size();
			info.usedRegisters.clear();
			addRegister(flags, instruction.getOp0Register(), OpAccess.WRITE);
		}
	}

	private static boolean isClearInstr(Instruction instruction) {
		switch (instruction.getMvexRegMemConv()) {
		case MvexRegMemConv.NONE:
		case MvexRegMemConv.REG_SWIZZLE_NONE:
			return true;
		default:
			return false;
		}
	}

	private void commandClearRegRegmem(Instruction instruction, int flags) {
		if (instruction.getOp0Register() != instruction.getOp1Register())
			return;
		if (instruction.getOp1Kind() != OpKind.REGISTER)
			return;
		if (!isClearInstr(instruction))
			return;
		info.opAccesses[0] = (byte)OpAccess.WRITE;
		info.opAccesses[1] = (byte)OpAccess.NONE;
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			assert info.usedRegisters.size() == 2 || info.usedRegisters.size() == 3 : info.usedRegisters.size();
			info.usedRegisters.set(0, new UsedRegister(instruction.getOp0Register(), OpAccess.WRITE));
			info.usedRegisters.remove(info.usedRegisters.size() - 1);
			if (info.usedRegisters.size() != 1)
				info.usedRegisters.remove(info.usedRegisters.size() - 1);
			assert info.usedRegisters.size() == 1 : info.usedRegisters.size();
		}
	}

	private void commandClearRegRegRegmem(Instruction instruction, int flags) {
		if (instruction.getOp1Register() != instruction.getOp2Register())
			return;
		if (instruction.getOp2Kind() != OpKind.REGISTER)
			return;
		if (!isClearInstr(instruction))
			return;
		info.opAccesses[1] = (byte)OpAccess.NONE;
		info.opAccesses[2] = (byte)OpAccess.NONE;
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			assert info.usedRegisters.size() == 3 || info.usedRegisters.size() == 4 : info.usedRegisters.size();
			assert info.usedRegisters.get(info.usedRegisters.size() - 2).getRegister() == instruction.getOp1Register();
			assert info.usedRegisters.get(info.usedRegisters.size() - 1).getRegister() == instruction.getOp2Register();
			info.usedRegisters.remove(info.usedRegisters.size() - 1);
			info.usedRegisters.remove(info.usedRegisters.size() - 1);
		}
	}

	private void commandArpl(Instruction instruction, int flags) {
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			assert info.usedRegisters.size() != 0 : info.usedRegisters.size();
			// Skip memory operand, if any
			int startIndex = instruction.getOp0Kind() == OpKind.REGISTER ? 0 : info.usedRegisters.size() - 1;
			for (int i = 0; i < info.usedRegisters.size(); i++) {
				if (i < startIndex)
					continue;
				UsedRegister regInfo = info.usedRegisters.get(i);
				int index = tryGetGpr163264Index(regInfo.getRegister());
				if (index >= 4)
					index += 4;// Skip AH, CH, DH, BH
				if (index >= 0)
					info.usedRegisters.set(i, new UsedRegister(Register.AL + index, regInfo.getAccess()));
			}
		}
	}

	private void commandLastGpr(Instruction instruction, int flags, int baseReg) {
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			int opCount = instruction.getOpCount();
			int immCount = instruction.getOpKind(opCount - 1) == OpKind.IMMEDIATE8 ? 1 : 0;
			final int N = 1;
			int opIndex = opCount - N - immCount;
			if (instruction.getOpKind(opIndex) == OpKind.REGISTER) {
				assert info.usedRegisters.size() >= N : info.usedRegisters.size();
				assert info.usedRegisters.get(info.usedRegisters.size() - N).getRegister() == instruction.getOpRegister(opIndex);
				assert info.usedRegisters.get(info.usedRegisters.size() - N).getAccess() == OpAccess.READ;
				int index = tryGetGpr163264Index(instruction.getOpRegister(opIndex));
				if (index >= 4 && baseReg == Register.AL)
					index += 4;// Skip AH, CH, DH, BH
				if (index >= 0)
					info.usedRegisters.set(info.usedRegisters.size() - N, new UsedRegister(baseReg + index, OpAccess.READ));
			}
		}
	}

	private void commandLea(Instruction instruction, int flags) {
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			assert info.usedRegisters.size() >= 1 : info.usedRegisters.size();
			assert instruction.getOp0Kind() == OpKind.REGISTER : instruction.getOp0Kind();
			int reg = instruction.getOp0Register();
			// The memory operand's regs start at index 1
			for (int i = 1; i < info.usedRegisters.size(); i++) {
				UsedRegister regInfo = info.usedRegisters.get(i);
				if (reg >= Register.EAX && reg <= Register.R15D) {
					if (regInfo.getRegister() >= Register.RAX && regInfo.getRegister() <= Register.R15) {
						int memReg = regInfo.getRegister() - Register.RAX + Register.EAX;
						info.usedRegisters.set(i, new UsedRegister(memReg, regInfo.getAccess()));
					}
				}
				else if (reg >= Register.AX && reg <= Register.R15W) {
					if (regInfo.getRegister() >= Register.EAX && regInfo.getRegister() <= Register.R15) {
						int memReg = ((regInfo.getRegister() - Register.EAX) & 0xF) + Register.AX;
						info.usedRegisters.set(i, new UsedRegister(memReg, regInfo.getAccess()));
					}
				}
				else {
					assert reg >= Register.RAX && reg <= Register.R15 : reg;
					break;
				}
			}
		}
	}

	private void commandEmmi(Instruction instruction, int flags, int opAccess) {
		if ((flags & Flags.NO_REGISTER_USAGE) == 0) {
			if (instruction.getOp0Kind() == OpKind.REGISTER) {
				int reg = instruction.getOp0Register();
				if (reg >= Register.MM0 && reg <= Register.MM7) {
					reg = ((reg - Register.MM0) ^ 1) + Register.MM0;
					addRegister(flags, reg, opAccess);
				}
			}
		}
	}

	private void commandMemDispl(int flags, int extraDispl) {
		if ((flags & Flags.NO_MEMORY_USAGE) == 0) {
			if (info.usedMemoryLocations.size() == 1) {
				UsedMemory mem = info.usedMemoryLocations.get(0);
				long mask;
				switch (mem.getAddressSize()) {
				case CodeSize.CODE16:
					mask = 0xFFFF;
					break;
				case CodeSize.CODE32:
					mask = 0xFFFF_FFFFL;
					break;
				default:
					mask = 0xFFFF_FFFF_FFFF_FFFFL;
					break;
				}
				long displ = (mem.getDisplacement() + extraDispl) & mask;
				info.usedMemoryLocations.set(0, new UsedMemory(mem.getSegment(), mem.getBase(), mem.getIndex(), mem.getScale(), displ,
						mem.getMemorySize(), mem.getAccess(), mem.getAddressSize(), mem.getVsibSize()));
			}
			else
				assert false;
		}
	}

	private static int tryGetGpr163264Index(int register) {
		int index;
		index = register - Register.EAX;
		if (Integer.compareUnsigned(index, 15) <= 0)
			return index;
		index = register - Register.RAX;
		if (Integer.compareUnsigned(index, 15) <= 0)
			return index;
		index = register - Register.AX;
		if (Integer.compareUnsigned(index, 15) <= 0)
			return index;
		return -1;
	}

	private void addMemory(int segReg, int baseReg, int indexReg, int scale, long displ, int memorySize, int access, int addressSize, int vsibSize) {
		if (addressSize == CodeSize.UNKNOWN) {
			int reg = baseReg != Register.NONE ? baseReg : indexReg;
			if (Register.isGPR64(reg))
				addressSize = CodeSize.CODE64;
			else if (Register.isGPR32(reg))
				addressSize = CodeSize.CODE32;
			else if (Register.isGPR16(reg))
				addressSize = CodeSize.CODE16;
		}
		if (access != OpAccess.NO_MEM_ACCESS)
			info.usedMemoryLocations.add(new UsedMemory(segReg, baseReg, indexReg, scale, displ, memorySize, access, addressSize, vsibSize));
	}

	private void addMemorySegmentRegister(int flags, int seg, int access) {
		assert Register.ES <= seg && seg <= Register.GS : seg;
		// Ignore es,cs,ss,ds memory operand segment registers in 64-bit mode
		if ((flags & Flags.IS_64_BIT) == 0 || seg >= Register.FS)
			addRegister(flags, seg, access);
	}

	private void addRegister(int flags, int reg, int access) {
		assert (flags & Flags.NO_REGISTER_USAGE) == 0 : "Caller should check flags before calling this method";

		int writeReg = reg;
		if ((flags & (Flags.IS_64_BIT | Flags.ZERO_EXT_VEC_REGS)) != 0) {
			if (Integer.compareUnsigned(access - OpAccess.WRITE, 3) <= 0) {
				int index;
				if ((flags & Flags.IS_64_BIT) != 0 && Integer.compareUnsigned(index = reg - Register.EAX, Register.R15D - Register.EAX) <= 0)
					writeReg = Register.RAX + index;
				else if ((flags & Flags.ZERO_EXT_VEC_REGS) != 0
						&& Integer.compareUnsigned(index = reg - Register.XMM0, IcedConstants.VMM_LAST - Register.XMM0) <= 0)
					writeReg = Register.ZMM0 + (index % IcedConstants.VMM_COUNT);
				if (access != OpAccess.READ_WRITE && access != OpAccess.READ_COND_WRITE)
					reg = writeReg;
			}
		}

		if (writeReg == reg) {
			info.usedRegisters.add(new UsedRegister(reg, access));
		}
		else {
			assert access == OpAccess.READ_WRITE || access == OpAccess.READ_COND_WRITE : access;
			info.usedRegisters.add(new UsedRegister(reg, OpAccess.READ));
			int lastAccess = access == OpAccess.READ_WRITE ? OpAccess.WRITE : OpAccess.COND_WRITE;
			info.usedRegisters.add(new UsedRegister(writeReg, lastAccess));
		}
	}
}
