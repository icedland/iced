// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.ArrayList;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.ICRegister;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemoryOperand;

public final class NonDecodedInstructions {
	public static Iterable<NonDecodedTestCase> getTests() {
		ArrayList<NonDecodedTestCase> result = new ArrayList<NonDecodedTestCase>(infos16.length + infos32.length + infos64.length);
		for (NonDecodedTestCase info : infos16)
			result.add(info);
		for (NonDecodedTestCase info : infos32)
			result.add(info);
		for (NonDecodedTestCase info : infos64)
			result.add(info);
		return result;
	}

	private static Instruction c16(Instruction instruction) {
		instruction.setCodeSize(CodeSize.CODE16);
		return instruction;
	}

	private static Instruction c32(Instruction instruction) {
		instruction.setCodeSize(CodeSize.CODE32);
		return instruction;
	}

	private static Instruction c64(Instruction instruction) {
		instruction.setCodeSize(CodeSize.CODE64);
		return instruction;
	}

	public static final NonDecodedTestCase[] infos16;
	public static final NonDecodedTestCase[] infos32;
	public static final NonDecodedTestCase[] infos64;

	static {
		infos16 = new NonDecodedTestCase[] {
			new NonDecodedTestCase(16, "0F", c16(Instruction.create(Code.POPW_CS, ICRegisters.cs))),
			new NonDecodedTestCase(16, "9B D9 30", c16(Instruction.create(Code.FSTENV_M14BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(16, "9B 64 D9 30", c16(Instruction.create(Code.FSTENV_M14BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(16, "9B 66 D9 30", c16(Instruction.create(Code.FSTENV_M28BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(16, "9B 64 66 D9 30", c16(Instruction.create(Code.FSTENV_M28BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(16, "9B D9 38", c16(Instruction.create(Code.FSTCW_M2BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(16, "9B 64 D9 38", c16(Instruction.create(Code.FSTCW_M2BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(16, "9B DB E0", c16(Instruction.create(Code.FENI))),
			new NonDecodedTestCase(16, "9B DB E1", c16(Instruction.create(Code.FDISI))),
			new NonDecodedTestCase(16, "9B DB E2", c16(Instruction.create(Code.FCLEX))),
			new NonDecodedTestCase(16, "9B DB E3", c16(Instruction.create(Code.FINIT))),
			new NonDecodedTestCase(16, "9B DB E4", c16(Instruction.create(Code.FSETPM))),
			new NonDecodedTestCase(16, "9B DD 30", c16(Instruction.create(Code.FSAVE_M94BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(16, "9B 64 DD 30", c16(Instruction.create(Code.FSAVE_M94BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(16, "9B 66 DD 30", c16(Instruction.create(Code.FSAVE_M108BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(16, "9B 64 66 DD 30", c16(Instruction.create(Code.FSAVE_M108BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(16, "9B DD 38", c16(Instruction.create(Code.FSTSW_M2BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(16, "9B 64 DD 38", c16(Instruction.create(Code.FSTSW_M2BYTE, new MemoryOperand(ICRegisters.bx, ICRegisters.si, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(16, "9B DF E0", c16(Instruction.create(Code.FSTSW_AX, ICRegisters.ax))),
			new NonDecodedTestCase(16, "9B DF E1", c16(Instruction.create(Code.FSTDW_AX, ICRegisters.ax))),
			new NonDecodedTestCase(16, "9B DF E2", c16(Instruction.create(Code.FSTSG_AX, ICRegisters.ax))),
			new NonDecodedTestCase(16, "", c16(Instruction.create(Code.ZERO_BYTES))),
			new NonDecodedTestCase(16, "77", c16(Instruction.createDeclareByte(0x77))),
			new NonDecodedTestCase(16, "77 A9", c16(Instruction.createDeclareByte(0x77, 0xA9))),
			new NonDecodedTestCase(16, "77 A9 CE", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE))),
			new NonDecodedTestCase(16, "77 A9 CE 9D", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86 32", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86 32 FE", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA))),
			new NonDecodedTestCase(16, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", c16(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08))),
			new NonDecodedTestCase(16, "A977", c16(Instruction.createDeclareWord(0x77A9))),
			new NonDecodedTestCase(16, "A977 9DCE", c16(Instruction.createDeclareWord(0x77A9, 0xCE9D))),
			new NonDecodedTestCase(16, "A977 9DCE 0555", c16(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505))),
			new NonDecodedTestCase(16, "A977 9DCE 0555 6C42", c16(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C))),
			new NonDecodedTestCase(16, "A977 9DCE 0555 6C42 3286", c16(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632))),
			new NonDecodedTestCase(16, "A977 9DCE 0555 6C42 3286 4FFE", c16(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F))),
			new NonDecodedTestCase(16, "A977 9DCE 0555 6C42 3286 4FFE 2734", c16(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427))),
			new NonDecodedTestCase(16, "A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", c16(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08))),
			new NonDecodedTestCase(16, "9DCEA977", c16(Instruction.createDeclareDword(0x77A9CE9D))),
			new NonDecodedTestCase(16, "9DCEA977 6C420555", c16(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C))),
			new NonDecodedTestCase(16, "9DCEA977 6C420555 4FFE3286", c16(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F))),
			new NonDecodedTestCase(16, "9DCEA977 6C420555 4FFE3286 08AA2734", c16(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08))),
			new NonDecodedTestCase(16, "6C4205559DCEA977", c16(Instruction.createDeclareQword(0x77A9CE9D5505426CL))),
			new NonDecodedTestCase(16, "6C4205559DCEA977 08AA27344FFE3286", c16(Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L))),
		};

		infos32 = new NonDecodedTestCase[] {
			new NonDecodedTestCase(32, "66 0F", c32(Instruction.create(Code.POPW_CS, ICRegisters.cs))),
			new NonDecodedTestCase(32, "9B 66 D9 30", c32(Instruction.create(Code.FSTENV_M14BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(32, "9B 64 66 D9 30", c32(Instruction.create(Code.FSTENV_M14BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(32, "9B D9 30", c32(Instruction.create(Code.FSTENV_M28BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(32, "9B 64 D9 30", c32(Instruction.create(Code.FSTENV_M28BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(32, "9B D9 38", c32(Instruction.create(Code.FSTCW_M2BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(32, "9B 64 D9 38", c32(Instruction.create(Code.FSTCW_M2BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(32, "9B DB E0", c32(Instruction.create(Code.FENI))),
			new NonDecodedTestCase(32, "9B DB E1", c32(Instruction.create(Code.FDISI))),
			new NonDecodedTestCase(32, "9B DB E2", c32(Instruction.create(Code.FCLEX))),
			new NonDecodedTestCase(32, "9B DB E3", c32(Instruction.create(Code.FINIT))),
			new NonDecodedTestCase(32, "9B DB E4", c32(Instruction.create(Code.FSETPM))),
			new NonDecodedTestCase(32, "9B 66 DD 30", c32(Instruction.create(Code.FSAVE_M94BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(32, "9B 64 66 DD 30", c32(Instruction.create(Code.FSAVE_M94BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(32, "9B DD 30", c32(Instruction.create(Code.FSAVE_M108BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(32, "9B 64 DD 30", c32(Instruction.create(Code.FSAVE_M108BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(32, "9B DD 38", c32(Instruction.create(Code.FSTSW_M2BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(32, "9B 64 DD 38", c32(Instruction.create(Code.FSTSW_M2BYTE, new MemoryOperand(ICRegisters.eax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(32, "9B DF E0", c32(Instruction.create(Code.FSTSW_AX, ICRegisters.ax))),
			new NonDecodedTestCase(32, "9B DF E1", c32(Instruction.create(Code.FSTDW_AX, ICRegisters.ax))),
			new NonDecodedTestCase(32, "9B DF E2", c32(Instruction.create(Code.FSTSG_AX, ICRegisters.ax))),
			new NonDecodedTestCase(32, "", c32(Instruction.create(Code.ZERO_BYTES))),
			new NonDecodedTestCase(32, "77", c32(Instruction.createDeclareByte(0x77))),
			new NonDecodedTestCase(32, "77 A9", c32(Instruction.createDeclareByte(0x77, 0xA9))),
			new NonDecodedTestCase(32, "77 A9 CE", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE))),
			new NonDecodedTestCase(32, "77 A9 CE 9D", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86 32", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86 32 FE", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA))),
			new NonDecodedTestCase(32, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", c32(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08))),
			new NonDecodedTestCase(32, "A977", c32(Instruction.createDeclareWord(0x77A9))),
			new NonDecodedTestCase(32, "A977 9DCE", c32(Instruction.createDeclareWord(0x77A9, 0xCE9D))),
			new NonDecodedTestCase(32, "A977 9DCE 0555", c32(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505))),
			new NonDecodedTestCase(32, "A977 9DCE 0555 6C42", c32(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C))),
			new NonDecodedTestCase(32, "A977 9DCE 0555 6C42 3286", c32(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632))),
			new NonDecodedTestCase(32, "A977 9DCE 0555 6C42 3286 4FFE", c32(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F))),
			new NonDecodedTestCase(32, "A977 9DCE 0555 6C42 3286 4FFE 2734", c32(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427))),
			new NonDecodedTestCase(32, "A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", c32(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08))),
			new NonDecodedTestCase(32, "9DCEA977", c32(Instruction.createDeclareDword(0x77A9CE9D))),
			new NonDecodedTestCase(32, "9DCEA977 6C420555", c32(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C))),
			new NonDecodedTestCase(32, "9DCEA977 6C420555 4FFE3286", c32(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F))),
			new NonDecodedTestCase(32, "9DCEA977 6C420555 4FFE3286 08AA2734", c32(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08))),
			new NonDecodedTestCase(32, "6C4205559DCEA977", c32(Instruction.createDeclareQword(0x77A9CE9D5505426CL))),
			new NonDecodedTestCase(32, "6C4205559DCEA977 08AA27344FFE3286", c32(Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L))),
		};

		infos64 = new NonDecodedTestCase[] {
			new NonDecodedTestCase(64, "9B 66 D9 30", c64(Instruction.create(Code.FSTENV_M14BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(64, "9B 64 66 D9 30", c64(Instruction.create(Code.FSTENV_M14BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(64, "9B D9 30", c64(Instruction.create(Code.FSTENV_M28BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(64, "9B 64 D9 30", c64(Instruction.create(Code.FSTENV_M28BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(64, "9B D9 38", c64(Instruction.create(Code.FSTCW_M2BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(64, "9B 64 D9 38", c64(Instruction.create(Code.FSTCW_M2BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(64, "9B DB E0", c64(Instruction.create(Code.FENI))),
			new NonDecodedTestCase(64, "9B DB E1", c64(Instruction.create(Code.FDISI))),
			new NonDecodedTestCase(64, "9B DB E2", c64(Instruction.create(Code.FCLEX))),
			new NonDecodedTestCase(64, "9B DB E3", c64(Instruction.create(Code.FINIT))),
			new NonDecodedTestCase(64, "9B DB E4", c64(Instruction.create(Code.FSETPM))),
			new NonDecodedTestCase(64, "9B 66 DD 30", c64(Instruction.create(Code.FSAVE_M94BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(64, "9B 64 66 DD 30", c64(Instruction.create(Code.FSAVE_M94BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(64, "9B DD 30", c64(Instruction.create(Code.FSAVE_M108BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(64, "9B 64 DD 30", c64(Instruction.create(Code.FSAVE_M108BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(64, "9B DD 38", c64(Instruction.create(Code.FSTSW_M2BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegister.NONE)))),
			new NonDecodedTestCase(64, "9B 64 DD 38", c64(Instruction.create(Code.FSTSW_M2BYTE, new MemoryOperand(ICRegisters.rax, ICRegister.NONE, 1, 0, 0, false, ICRegisters.fs)))),
			new NonDecodedTestCase(64, "9B DF E0", c64(Instruction.create(Code.FSTSW_AX, ICRegisters.ax))),
			new NonDecodedTestCase(64, "", c64(Instruction.create(Code.ZERO_BYTES))),
			new NonDecodedTestCase(64, "77", c64(Instruction.createDeclareByte(0x77))),
			new NonDecodedTestCase(64, "77 A9", c64(Instruction.createDeclareByte(0x77, 0xA9))),
			new NonDecodedTestCase(64, "77 A9 CE", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE))),
			new NonDecodedTestCase(64, "77 A9 CE 9D", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86 32", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86 32 FE", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA))),
			new NonDecodedTestCase(64, "77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08", c64(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08))),
			new NonDecodedTestCase(64, "A977", c64(Instruction.createDeclareWord(0x77A9))),
			new NonDecodedTestCase(64, "A977 9DCE", c64(Instruction.createDeclareWord(0x77A9, 0xCE9D))),
			new NonDecodedTestCase(64, "A977 9DCE 0555", c64(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505))),
			new NonDecodedTestCase(64, "A977 9DCE 0555 6C42", c64(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C))),
			new NonDecodedTestCase(64, "A977 9DCE 0555 6C42 3286", c64(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632))),
			new NonDecodedTestCase(64, "A977 9DCE 0555 6C42 3286 4FFE", c64(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F))),
			new NonDecodedTestCase(64, "A977 9DCE 0555 6C42 3286 4FFE 2734", c64(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427))),
			new NonDecodedTestCase(64, "A977 9DCE 0555 6C42 3286 4FFE 2734 08AA", c64(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08))),
			new NonDecodedTestCase(64, "9DCEA977", c64(Instruction.createDeclareDword(0x77A9CE9D))),
			new NonDecodedTestCase(64, "9DCEA977 6C420555", c64(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C))),
			new NonDecodedTestCase(64, "9DCEA977 6C420555 4FFE3286", c64(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F))),
			new NonDecodedTestCase(64, "9DCEA977 6C420555 4FFE3286 08AA2734", c64(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08))),
			new NonDecodedTestCase(64, "6C4205559DCEA977", c64(Instruction.createDeclareQword(0x77A9CE9D5505426CL))),
			new NonDecodedTestCase(64, "6C4205559DCEA977 08AA27344FFE3286", c64(Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L))),
		};
	}
}
