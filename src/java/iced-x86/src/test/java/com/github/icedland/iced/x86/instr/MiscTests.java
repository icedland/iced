// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemoryOperand;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class MiscTests {
	@Test
	void invalid_code_value_is_zero() {
		// A 'default' Instruction should be an invalid instruction
		assertEquals(0, Code.INVALID);
		Instruction instruction1 = new Instruction();
		assertEquals(Code.INVALID, instruction1.getCode());
		Instruction instruction2 = new Instruction();
		assertEquals(Code.INVALID, instruction2.getCode());
		assertTrue(instruction1.equalsAllBits(instruction2));
	}

	@Test
	void equals_and_gethashcode_ignore_some_fields() {
		Instruction instruction1 = Instruction.create(Code.VEX_VPERMIL2PS_XMM_XMM_XMMM128_XMM_IMM4, ICRegisters.xmm1, ICRegisters.xmm2, new MemoryOperand(ICRegisters.rcx, ICRegisters.r14, 8, 0x12345678, 8, false, ICRegisters.fs), ICRegisters.xmm10, 0xA5);
		Instruction instruction2 = instruction1.copy();
		assertTrue(instruction1.equalsAllBits(instruction2));
		instruction1.setCodeSize(CodeSize.CODE32);
		instruction2.setCodeSize(CodeSize.CODE64);
		assertFalse(instruction1.equalsAllBits(instruction2));
		instruction1.setLength(10);
		instruction2.setLength(5);
		instruction1.setIP(0x97333795FA7CEAABL);
		instruction2.setIP(0x9BE5A3A07A66FC05L);
		assertTrue(instruction1.equals(instruction2));
		assertEquals(instruction1, instruction2);
		assertEquals(instruction1.hashCode(), instruction2.hashCode());
	}

	@Test
	void write_all_properties() {
		Instruction instruction = new Instruction();

		instruction.setIP(0x8A6BD04A9B683A92L);
		instruction.setIP16((short)0);
		assertEquals((short)0, instruction.getIP16());
		assertEquals((short)0, instruction.getIP32());
		assertEquals((short)0, instruction.getIP());
		instruction.setIP(0x8A6BD04A9B683A92L);
		instruction.setIP16((short)0xFFFF);
		assertEquals((short)0xFFFF, instruction.getIP16());
		assertEquals(0xFFFF, instruction.getIP32());
		assertEquals(0xFFFF, instruction.getIP());

		instruction.setIP(0x8A6BD04A9B683A92L);
		instruction.setIP32(0);
		assertEquals((short)0, instruction.getIP16());
		assertEquals(0, instruction.getIP32());
		assertEquals(0, instruction.getIP());
		instruction.setIP(0x8A6BD04A9B683A92L);
		instruction.setIP32(0xFFFF_FFFF);
		assertEquals((short)0xFFFF, instruction.getIP16());
		assertEquals(0xFFFF_FFFF, instruction.getIP32());
		assertEquals(0xFFFF_FFFFL, instruction.getIP());

		instruction.setIP(0L);
		assertEquals((short)0, instruction.getIP16());
		assertEquals(0, instruction.getIP32());
		assertEquals(0L, instruction.getIP());
		instruction.setIP(0xFFFF_FFFF_FFFF_FFFFL);
		assertEquals((short)0xFFFF, instruction.getIP16());
		assertEquals(0xFFFF_FFFF, instruction.getIP32());
		assertEquals(0xFFFF_FFFF_FFFF_FFFFL, instruction.getIP());

		instruction.setNextIP(0x8A6BD04A9B683A92L);
		instruction.setNextIP16((short)0);
		assertEquals((short)0, instruction.getNextIP16());
		assertEquals((short)0, instruction.getNextIP32());
		assertEquals((short)0, instruction.getNextIP());
		instruction.setNextIP(0x8A6BD04A9B683A92L);
		instruction.setNextIP16((short)0xFFFF);
		assertEquals((short)0xFFFF, instruction.getNextIP16());
		assertEquals(0xFFFF, instruction.getNextIP32());
		assertEquals(0xFFFF, instruction.getNextIP());

		instruction.setNextIP(0x8A6BD04A9B683A92L);
		instruction.setNextIP32(0);
		assertEquals((short)0, instruction.getNextIP16());
		assertEquals(0, instruction.getNextIP32());
		assertEquals(0, instruction.getNextIP());
		instruction.setNextIP(0x8A6BD04A9B683A92L);
		instruction.setNextIP32(0xFFFF_FFFF);
		assertEquals((short)0xFFFF, instruction.getNextIP16());
		assertEquals(0xFFFF_FFFF, instruction.getNextIP32());
		assertEquals(0xFFFF_FFFFL, instruction.getNextIP());

		instruction.setNextIP(0L);
		assertEquals((short)0, instruction.getNextIP16());
		assertEquals(0, instruction.getNextIP32());
		assertEquals(0L, instruction.getNextIP());
		instruction.setNextIP(0xFFFF_FFFF_FFFF_FFFFL);
		assertEquals((short)0xFFFF, instruction.getNextIP16());
		assertEquals(0xFFFF_FFFF, instruction.getNextIP32());
		assertEquals(0xFFFF_FFFF_FFFF_FFFFL, instruction.getNextIP());

		instruction.setMemoryDisplacement32(0);
		assertEquals(0, instruction.getMemoryDisplacement32());
		assertEquals(0, instruction.getMemoryDisplacement64());
		instruction.setMemoryDisplacement32(0xFFFF_FFFF);
		assertEquals(0xFFFF_FFFF, instruction.getMemoryDisplacement32());
		assertEquals(0xFFFF_FFFFL, instruction.getMemoryDisplacement64());

		instruction.setMemoryDisplacement64(0L);
		assertEquals(0, instruction.getMemoryDisplacement32());
		assertEquals(0L, instruction.getMemoryDisplacement64());
		instruction.setMemoryDisplacement64(0xFFFF_FFFF_FFFF_FFFFL);
		assertEquals(0xFFFF_FFFF, instruction.getMemoryDisplacement32());
		assertEquals(0xFFFF_FFFF_FFFF_FFFFL, instruction.getMemoryDisplacement64());

		instruction.setMemoryDisplacement64(0x1234_5678_9ABC_DEF1L);
		instruction.setMemoryDisplacement32(0x5AA5_4321);
		assertEquals(0x5AA5_4321, instruction.getMemoryDisplacement32());
		assertEquals(0x5AA5_4321, instruction.getMemoryDisplacement64());

		instruction.setImmediate8((byte)0);
		assertEquals((byte)0, instruction.getImmediate8());
		instruction.setImmediate8((byte)0xFF);
		assertEquals((byte)0xFF, instruction.getImmediate8());

		instruction.setImmediate8_2nd((byte)0);
		assertEquals((byte)0, instruction.getImmediate8_2nd());
		instruction.setImmediate8_2nd((byte)0xFF);
		assertEquals((byte)0xFF, instruction.getImmediate8_2nd());

		instruction.setImmediate16((short)0);
		assertEquals((short)0, instruction.getImmediate16());
		instruction.setImmediate16((short)0xFFFF);
		assertEquals((short)0xFFFF, instruction.getImmediate16());

		instruction.setImmediate32(0);
		assertEquals(0, instruction.getImmediate32());
		instruction.setImmediate32(0xFFFF_FFFF);
		assertEquals(0xFFFF_FFFF, instruction.getImmediate32());

		instruction.setImmediate64(0L);
		assertEquals(0L, instruction.getImmediate64());
		instruction.setImmediate64(0xFFFF_FFFF_FFFF_FFFFL);
		assertEquals(0xFFFF_FFFF_FFFF_FFFFL, instruction.getImmediate64());

		instruction.setImmediate8to16((byte)0);
		assertEquals((byte)0, instruction.getImmediate8to16());
		instruction.setImmediate8to16((byte)0xFF);
		assertEquals((byte)0xFF, instruction.getImmediate8to16());

		instruction.setImmediate8to32((byte)0);
		assertEquals((byte)0, instruction.getImmediate8to32());
		instruction.setImmediate8to32((byte)0xFF);
		assertEquals((byte)0xFF, instruction.getImmediate8to32());

		instruction.setImmediate8to64((byte)0);
		assertEquals((byte)0, instruction.getImmediate8to64());
		instruction.setImmediate8to64((byte)0xFF);
		assertEquals((byte)0xFF, instruction.getImmediate8to64());

		instruction.setImmediate32to64(-0x8000_0000);
		assertEquals(-0x8000_0000, instruction.getImmediate32to64());
		instruction.setImmediate32to64(0x7FFF_FFFF);
		assertEquals(0x7FFF_FFFF, instruction.getImmediate32to64());

		instruction.setOp0Kind(OpKind.NEAR_BRANCH16);
		instruction.setNearBranch16((short)0);
		assertEquals((short)0, instruction.getNearBranch16());
		assertEquals((short)0, instruction.getNearBranchTarget());
		instruction.setNearBranch16((short)0xFFFF);
		assertEquals((short)0xFFFF, instruction.getNearBranch16());
		assertEquals(0xFFFF, instruction.getNearBranchTarget());

		instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
		instruction.setNearBranch32(0);
		assertEquals(0, instruction.getNearBranch32());
		assertEquals(0, instruction.getNearBranchTarget());
		instruction.setNearBranch32(0xFFFF_FFFF);
		assertEquals(0xFFFF_FFFF, instruction.getNearBranch32());
		assertEquals(0xFFFF_FFFFL, instruction.getNearBranchTarget());

		instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
		instruction.setNearBranch64(0L);
		assertEquals(0L, instruction.getNearBranch64());
		assertEquals(0L, instruction.getNearBranchTarget());
		instruction.setNearBranch64(0xFFFF_FFFF_FFFF_FFFFL);
		assertEquals(0xFFFF_FFFF_FFFF_FFFFL, instruction.getNearBranch64());
		assertEquals(0xFFFF_FFFF_FFFF_FFFFL, instruction.getNearBranchTarget());

		instruction.setFarBranch16((short)0);
		assertEquals((short)0, instruction.getFarBranch16());
		instruction.setFarBranch16((short)0xFFFF);
		assertEquals((short)0xFFFF, instruction.getFarBranch16());

		instruction.setFarBranch32(0);
		assertEquals(0, instruction.getFarBranch32());
		instruction.setFarBranch32(0xFFFF_FFFF);
		assertEquals(0xFFFF_FFFF, instruction.getFarBranch32());

		instruction.setFarBranchSelector((short)0);
		assertEquals((short)0, instruction.getFarBranchSelector());
		instruction.setFarBranchSelector((short)0xFFFF);
		assertEquals((short)0xFFFF, instruction.getFarBranchSelector());

		{
			Instruction instr = instruction.copy();
			instr.setCode(Code.CMPXCHG8B_M64);
			instr.setOp0Kind(OpKind.MEMORY);
			instr.setLockPrefix(true);

			instr.setXacquirePrefix(false);
			assertFalse(instr.getXacquirePrefix());
			instr.setXacquirePrefix(true);
			assertTrue(instr.getXacquirePrefix());

			instr.setXreleasePrefix(false);
			assertFalse(instr.getXreleasePrefix());
			instr.setXreleasePrefix(true);
			assertTrue(instr.getXreleasePrefix());
		}

		instruction.setRepPrefix(false);
		assertFalse(instruction.getRepPrefix());
		assertFalse(instruction.getRepePrefix());
		instruction.setRepPrefix(true);
		assertTrue(instruction.getRepPrefix());
		assertTrue(instruction.getRepePrefix());

		instruction.setRepePrefix(false);
		assertFalse(instruction.getRepPrefix());
		assertFalse(instruction.getRepePrefix());
		instruction.setRepePrefix(true);
		assertTrue(instruction.getRepPrefix());
		assertTrue(instruction.getRepePrefix());

		instruction.setRepnePrefix(false);
		assertFalse(instruction.getRepnePrefix());
		instruction.setRepnePrefix(true);
		assertTrue(instruction.getRepnePrefix());

		instruction.setLockPrefix(false);
		assertFalse(instruction.getLockPrefix());
		instruction.setLockPrefix(true);
		assertTrue(instruction.getLockPrefix());

		instruction.setBroadcast(false);
		assertFalse(instruction.getBroadcast());
		instruction.setBroadcast(true);
		assertTrue(instruction.getBroadcast());

		instruction.setSuppressAllExceptions(false);
		assertFalse(instruction.getSuppressAllExceptions());
		instruction.setSuppressAllExceptions(true);
		assertTrue(instruction.getSuppressAllExceptions());

		for (int i = 0; i <= IcedConstants.MAX_INSTRUCTION_LENGTH; i++) {
			instruction.setLength(i);
			assertEquals(i, instruction.getLength());
		}

		for (int codeSize : getCodeSizeValues()) {
			instruction.setCodeSize(codeSize);
			assertEquals(codeSize, instruction.getCodeSize());
		}

		for (int code : getCodeValues()) {
			instruction.setCode(code);
			assertEquals(code, instruction.getCode());
		}
		for (int code : getCodeValues()) {
			instruction.setCode(code);
			assertEquals(code, instruction.getCode());
		}
		assertThrows(IllegalArgumentException.class, () -> instruction.setCode(-1));
		assertThrows(IllegalArgumentException.class, () -> instruction.setCode(IcedConstants.CODE_ENUM_COUNT));

		assertEquals(5, IcedConstants.MAX_OP_COUNT);
		for (int opKind : getOpKindValues()) {
			instruction.setOp0Kind(opKind);
			assertEquals(opKind, instruction.getOp0Kind());
		}

		for (int opKind : getOpKindValues()) {
			instruction.setOp1Kind(opKind);
			assertEquals(opKind, instruction.getOp1Kind());
		}

		for (int opKind : getOpKindValues()) {
			instruction.setOp2Kind(opKind);
			assertEquals(opKind, instruction.getOp2Kind());
		}

		for (int opKind : getOpKindValues()) {
			instruction.setOp3Kind(opKind);
			assertEquals(opKind, instruction.getOp3Kind());
		}

		for (int opKind : getOpKindValues()) {
			if (opKind == OpKind.IMMEDIATE8) {
				instruction.setOp4Kind(opKind);
				assertEquals(opKind, instruction.getOp4Kind());
			}
			else
				assertThrows(IllegalArgumentException.class, () -> instruction.setOp4Kind(opKind));
		}

		for (int opKind : getOpKindValues()) {
			instruction.setOpKind(0, opKind);
			assertEquals(opKind, instruction.getOp0Kind());
			assertEquals(opKind, instruction.getOpKind(0));
		}

		for (int opKind : getOpKindValues()) {
			instruction.setOpKind(1, opKind);
			assertEquals(opKind, instruction.getOp1Kind());
			assertEquals(opKind, instruction.getOpKind(1));
		}

		for (int opKind : getOpKindValues()) {
			instruction.setOpKind(2, opKind);
			assertEquals(opKind, instruction.getOp2Kind());
			assertEquals(opKind, instruction.getOpKind(2));
		}

		for (int opKind : getOpKindValues()) {
			instruction.setOpKind(3, opKind);
			assertEquals(opKind, instruction.getOp3Kind());
			assertEquals(opKind, instruction.getOpKind(3));
		}

		for (int opKind : getOpKindValues()) {
			if (opKind == OpKind.IMMEDIATE8) {
				instruction.setOpKind(4, opKind);
				assertEquals(opKind, instruction.getOp4Kind());
				assertEquals(opKind, instruction.getOpKind(4));
			}
			else
				assertThrows(IllegalArgumentException.class, () -> instruction.setOpKind(4, opKind));
		}

		int[] segValues = new int[] {
			Register.ES,
			Register.CS,
			Register.SS,
			Register.DS,
			Register.FS,
			Register.GS,
			Register.NONE,
		};
		for (int seg : segValues) {
			instruction.setSegmentPrefix(seg);
			assertEquals(seg, instruction.getSegmentPrefix());
			if (instruction.getSegmentPrefix() == Register.NONE)
				assertFalse(instruction.hasSegmentPrefix());
			else
				assertTrue(instruction.hasSegmentPrefix());
		}

		int[] displSizes = new int[] { 8, 4, 2, 1, 0 };
		for (int displSize : displSizes) {
			instruction.setMemoryDisplSize(displSize);
			assertEquals(displSize, instruction.getMemoryDisplSize());
		}

		int[] scaleValues = new int[] { 8, 4, 2, 1 };
		for (int scaleValue : scaleValues) {
			instruction.setMemoryIndexScale(scaleValue);
			assertEquals(scaleValue, instruction.getMemoryIndexScale());
		}

		for (int reg : getRegisterValues()) {
			instruction.setMemoryBase(reg);
			assertEquals(reg, instruction.getMemoryBase());
		}

		for (int reg : getRegisterValues()) {
			instruction.setMemoryIndex(reg);
			assertEquals(reg, instruction.getMemoryIndex());
		}

		for (int reg : getRegisterValues()) {
			instruction.setOp0Register(reg);
			assertEquals(reg, instruction.getOp0Register());
		}

		for (int reg : getRegisterValues()) {
			instruction.setOp1Register(reg);
			assertEquals(reg, instruction.getOp1Register());
		}

		for (int reg : getRegisterValues()) {
			instruction.setOp2Register(reg);
			assertEquals(reg, instruction.getOp2Register());
		}

		for (int reg : getRegisterValues()) {
			instruction.setOp3Register(reg);
			assertEquals(reg, instruction.getOp3Register());
		}

		for (int reg : getRegisterValues()) {
			if (reg == Register.NONE) {
				instruction.setOp4Register(reg);
				assertEquals(reg, instruction.getOp4Register());
			}
			else
				assertThrows(IllegalArgumentException.class, () -> instruction.setOp4Register(reg));
		}

		for (int reg : getRegisterValues()) {
			instruction.setOpRegister(0, reg);
			assertEquals(reg, instruction.getOp0Register());
			assertEquals(reg, instruction.getOpRegister(0));
		}

		for (int reg : getRegisterValues()) {
			instruction.setOpRegister(1, reg);
			assertEquals(reg, instruction.getOp1Register());
			assertEquals(reg, instruction.getOpRegister(1));
		}

		for (int reg : getRegisterValues()) {
			instruction.setOpRegister(2, reg);
			assertEquals(reg, instruction.getOp2Register());
			assertEquals(reg, instruction.getOpRegister(2));
		}

		for (int reg : getRegisterValues()) {
			instruction.setOpRegister(3, reg);
			assertEquals(reg, instruction.getOp3Register());
			assertEquals(reg, instruction.getOpRegister(3));
		}

		for (int reg : getRegisterValues()) {
			if (reg == Register.NONE) {
				instruction.setOpRegister(4, reg);
				assertEquals(reg, instruction.getOp4Register());
				assertEquals(reg, instruction.getOpRegister(4));
			}
			else
				assertThrows(IllegalArgumentException.class, () -> instruction.setOpRegister(4, reg));
		}

		int[] opMasks = new int[] {
			Register.K1,
			Register.K2,
			Register.K3,
			Register.K4,
			Register.K5,
			Register.K6,
			Register.K7,
			Register.NONE,
		};
		for (int opMask : opMasks) {
			instruction.setOpMask(opMask);
			assertEquals(opMask, instruction.getOpMask());
			assertEquals(opMask != Register.NONE, instruction.hasOpMask());
		}

		instruction.setZeroingMasking(false);
		assertFalse(instruction.getZeroingMasking());
		assertTrue(instruction.getMergingMasking());
		instruction.setZeroingMasking(true);
		assertTrue(instruction.getZeroingMasking());
		assertFalse(instruction.getMergingMasking());
		instruction.setMergingMasking(false);
		assertFalse(instruction.getMergingMasking());
		assertTrue(instruction.getZeroingMasking());
		instruction.setMergingMasking(true);
		assertTrue(instruction.getMergingMasking());
		assertFalse(instruction.getZeroingMasking());

		for (int rc : getRoundingControlValues()) {
			instruction.setRoundingControl(rc);
			assertEquals(rc, instruction.getRoundingControl());
		}

		for (int reg : getRegisterValues()) {
			instruction.setMemoryBase(reg);
			assertEquals(reg == Register.RIP || reg == Register.EIP, instruction.isIPRelativeMemoryOperand());
		}

		instruction.setMemoryBase(Register.EIP);
		instruction.setNextIP(0x123456709EDCBA98L);
		instruction.setMemoryDisplacement64(0x876543219ABCDEF5L);
		assertTrue(instruction.isIPRelativeMemoryOperand());
		assertEquals(0x9ABCDEF5L, instruction.ipRelativeMemoryAddress());

		instruction.setMemoryBase(Register.RIP);
		instruction.setNextIP(0x123456709EDCBA98L);
		instruction.setMemoryDisplacement64(0x876543219ABCDEF5L);
		assertTrue(instruction.isIPRelativeMemoryOperand());
		assertEquals(0x876543219ABCDEF5L, instruction.ipRelativeMemoryAddress());

		instruction.setDeclareDataCount(1);
		assertEquals(1, instruction.getDeclareDataCount());
		instruction.setDeclareDataCount(15);
		assertEquals(15, instruction.getDeclareDataCount());
		instruction.setDeclareDataCount(16);
		assertEquals(16, instruction.getDeclareDataCount());
	}

	private static int[] getCodeSizeValues() {
		int[] result = new int[IcedConstants.CODE_SIZE_ENUM_COUNT];
		for (int i = 0; i < IcedConstants.CODE_SIZE_ENUM_COUNT; i++)
			result[i] = i;
		return result;
	}

	private static int[] getCodeValues() {
		int[] result = new int[IcedConstants.CODE_ENUM_COUNT];
		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++)
			result[i] = i;
		return result;
	}

	private static int[] getOpKindValues() {
		int[] result = new int[IcedConstants.OP_KIND_ENUM_COUNT];
		for (int i = 0; i < IcedConstants.OP_KIND_ENUM_COUNT; i++)
			result[i] = i;
		return result;
	}

	private static int[] getRegisterValues() {
		int[] result = new int[IcedConstants.REGISTER_ENUM_COUNT];
		for (int i = 0; i < IcedConstants.REGISTER_ENUM_COUNT; i++)
			result[i] = i;
		return result;
	}

	private static int[] getRoundingControlValues() {
		int[] result = new int[IcedConstants.ROUNDING_CONTROL_ENUM_COUNT];
		for (int i = 0; i < IcedConstants.ROUNDING_CONTROL_ENUM_COUNT; i++)
			result[i] = i;
		return result;
	}

	@Test
	void verify_getsetimmediate() {
		Instruction instruction = new Instruction();

		instruction.setCode(Code.ADD_AL_IMM8);
		instruction.setOp1Kind(OpKind.IMMEDIATE8);
		instruction.setImmediate(1, 0x5A);
		assertEquals(0x5AL, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA5);
		assertEquals(0xA5L, instruction.getImmediate(1));

		instruction.setCode(Code.ADD_AX_IMM16);
		instruction.setOp1Kind(OpKind.IMMEDIATE16);
		instruction.setImmediate(1, 0x5AA5);
		assertEquals(0x5AA5L, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA55A);
		assertEquals(0xA55AL, instruction.getImmediate(1));

		instruction.setCode(Code.ADD_EAX_IMM32);
		instruction.setOp1Kind(OpKind.IMMEDIATE32);
		instruction.setImmediate(1, 0x5AA51234);
		assertEquals(0x5AA51234L, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA54A1234);
		assertEquals(0xA54A1234L, instruction.getImmediate(1));

		instruction.setCode(Code.ADD_RAX_IMM32);
		instruction.setOp1Kind(OpKind.IMMEDIATE32TO64);
		instruction.setImmediate(1, 0x5AA51234);
		assertEquals(0x5AA51234L, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA54A1234);
		assertEquals(0xFFFFFFFFA54A1234L, instruction.getImmediate(1));

		instruction.setCode(Code.ENTERQ_IMM16_IMM8);
		instruction.setOp1Kind(OpKind.IMMEDIATE8_2ND);
		instruction.setImmediate(1, 0x5A);
		assertEquals(0x5AL, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA5);
		assertEquals(0xA5L, instruction.getImmediate(1));

		instruction.setCode(Code.ADC_RM16_IMM8);
		instruction.setOp1Kind(OpKind.IMMEDIATE8TO16);
		instruction.setImmediate(1, 0x5A);
		assertEquals(0x5AL, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA5);
		assertEquals(0xFFFFFFFFFFFFFFA5L, instruction.getImmediate(1));

		instruction.setCode(Code.ADC_RM32_IMM8);
		instruction.setOp1Kind(OpKind.IMMEDIATE8TO32);
		instruction.setImmediate(1, 0x5A);
		assertEquals(0x5AL, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA5);
		assertEquals(0xFFFFFFFFFFFFFFA5L, instruction.getImmediate(1));

		instruction.setCode(Code.ADC_RM64_IMM8);
		instruction.setOp1Kind(OpKind.IMMEDIATE8TO64);
		instruction.setImmediate(1, 0x5A);
		assertEquals(0x5AL, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA5);
		assertEquals(0xFFFFFFFFFFFFFFA5L, instruction.getImmediate(1));

		instruction.setCode(Code.MOV_R64_IMM64);
		instruction.setOp1Kind(OpKind.IMMEDIATE64);
		instruction.setImmediate(1, 0x5AA5123456789ABCL);
		assertEquals(0x5AA5123456789ABCL, instruction.getImmediate(1));
		instruction.setImmediate(1, 0xA54A123456789ABCL);
		assertEquals(0xA54A123456789ABCL, instruction.getImmediate(1));

		assertThrows(IllegalArgumentException.class, () -> instruction.getImmediate(0));
		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(0, 0));
		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(0, 0L));
	}
}
