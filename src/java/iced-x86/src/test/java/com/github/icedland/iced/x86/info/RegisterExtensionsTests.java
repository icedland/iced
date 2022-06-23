// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RegisterInfo;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class RegisterExtensionsTests {
	@ParameterizedTest
	@MethodSource("getInfo_throws_if_invalid_value_Data")
	void GetInfo_throws_if_invalid_value(int register) {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getInfo(register));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getBaseRegister(register));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getNumber(register));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getFullRegister(register));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getFullRegister32(register));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getSize(register));
	}

	static Iterable<Arguments> getInfo_throws_if_invalid_value_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		result.add(Arguments.of(-1));
		result.add(Arguments.of(IcedConstants.REGISTER_ENUM_COUNT));
		return result;
	}

	@ParameterizedTest
	@MethodSource("verifyRegisterProperties_Data")
	void verifyRegisterProperties(int register, int number, int baseRegister, int fullRegister, int fullRegister32, int size, int flags) {
		RegisterInfo info = Register.getInfo(register);
		assertEquals(register, info.getRegister());
		assertEquals(baseRegister, info.getBase());
		assertEquals(number, info.getNumber());
		assertEquals(fullRegister, info.getFullRegister());
		assertEquals(fullRegister32, info.getFullRegister32());
		assertEquals(size, info.getSize());

		assertEquals(baseRegister, Register.getBaseRegister(register));
		assertEquals(number, Register.getNumber(register));
		assertEquals(fullRegister, Register.getFullRegister(register));
		assertEquals(fullRegister32, Register.getFullRegister32(register));
		assertEquals(size, Register.getSize(register));

		final int allFlags =
			RegisterFlags.SEGMENT_REGISTER |
			RegisterFlags.GPR |
			RegisterFlags.GPR8 |
			RegisterFlags.GPR16 |
			RegisterFlags.GPR32 |
			RegisterFlags.GPR64 |
			RegisterFlags.XMM |
			RegisterFlags.YMM |
			RegisterFlags.ZMM |
			RegisterFlags.VECTOR_REGISTER |
			RegisterFlags.IP |
			RegisterFlags.K |
			RegisterFlags.BND |
			RegisterFlags.CR |
			RegisterFlags.DR |
			RegisterFlags.TR |
			RegisterFlags.ST |
			RegisterFlags.MM |
			RegisterFlags.TMM;
		// If it fails, update the flags above and the code below, eg. add a IsTMM() test
		assertEquals(flags, flags & allFlags);

		assertEquals((flags & RegisterFlags.SEGMENT_REGISTER) != 0, Register.isSegmentRegister(register));
		assertEquals((flags & RegisterFlags.GPR) != 0, Register.isGPR(register));
		assertEquals((flags & RegisterFlags.GPR8) != 0, Register.isGPR8(register));
		assertEquals((flags & RegisterFlags.GPR16) != 0, Register.isGPR16(register));
		assertEquals((flags & RegisterFlags.GPR32) != 0, Register.isGPR32(register));
		assertEquals((flags & RegisterFlags.GPR64) != 0, Register.isGPR64(register));
		assertEquals((flags & RegisterFlags.XMM) != 0, Register.isXMM(register));
		assertEquals((flags & RegisterFlags.YMM) != 0, Register.isYMM(register));
		assertEquals((flags & RegisterFlags.ZMM) != 0, Register.isZMM(register));
		assertEquals((flags & RegisterFlags.VECTOR_REGISTER) != 0, Register.isVectorRegister(register));
		assertEquals((flags & RegisterFlags.IP) != 0, Register.isIP(register));
		assertEquals((flags & RegisterFlags.K) != 0, Register.isK(register));
		assertEquals((flags & RegisterFlags.BND) != 0, Register.isBND(register));
		assertEquals((flags & RegisterFlags.CR) != 0, Register.isCR(register));
		assertEquals((flags & RegisterFlags.DR) != 0, Register.isDR(register));
		assertEquals((flags & RegisterFlags.TR) != 0, Register.isTR(register));
		assertEquals((flags & RegisterFlags.ST) != 0, Register.isST(register));
		assertEquals((flags & RegisterFlags.MM) != 0, Register.isMM(register));
		assertEquals((flags & RegisterFlags.TMM) != 0, Register.isTMM(register));
	}

	public static Iterable<Arguments> verifyRegisterProperties_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (RegisterInfoTestCase tc : RegisterInfoTestReader.getTestCases())
			result.add(Arguments.of(tc.register, tc.number, tc.baseRegister, tc.fullRegister, tc.fullRegister32, tc.size, tc.flags));
		return result;
	}
}
