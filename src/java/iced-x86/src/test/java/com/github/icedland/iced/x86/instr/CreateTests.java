// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

import java.util.ArrayList;
import java.util.function.Consumer;
import java.util.function.IntConsumer;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.BitnessUtils;
import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.ICRegister;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemoryOperand;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RepPrefixKind;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.VAGetRegisterValue;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.enc.Encoder;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class CreateTests {
	@Test
	void encoderIgnoresPrefixesIfDeclareData() {
		Instruction instruction;

		Consumer<Instruction> verify = instr -> {
			byte[] origData = getData(instr);
			instr.setLockPrefix(true);
			instr.setRepePrefix(true);
			instr.setRepnePrefix(true);
			instr.setSegmentPrefix(Register.GS);
			instr.setXreleasePrefix(true);
			instr.setXacquirePrefix(true);
			instr.setSuppressAllExceptions(true);
			instr.setZeroingMasking(true);
			for (int bitness : new int[] { 16, 32, 64 }) {
				CodeWriterImpl writer = new CodeWriterImpl();
				Encoder encoder = new Encoder(bitness, writer);
				Object result = encoder.tryEncode(instr, 0);
				assertTrue(result instanceof Integer);
				assertArrayEquals(origData, writer.toArray());
			}
		};

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
		verify.accept(instruction);

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
		verify.accept(instruction);

		instruction = Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
		verify.accept(instruction);

		instruction = Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L);
		verify.accept(instruction);
	}

	private static byte[] getData(Instruction instruction) {
		int length = instruction.getDeclareDataCount();
		switch (instruction.getCode()) {
		case Code.DECLAREBYTE:
			break;
		case Code.DECLAREWORD:
			length *= 2;
			break;
		case Code.DECLAREDWORD:
			length *= 4;
			break;
		case Code.DECLAREQWORD:
			length *= 8;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		byte[] res = new byte[length];
		for (int i = 0; i < res.length; i++)
			res[i] = instruction.getDeclareByteValue(i);
		return res;
	}

	@Test
	void declareDataByteOrderIsSame() {
		byte[] data = new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, (byte)0xAA, 0x08 };
		Instruction db = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
		Instruction dw = Instruction.createDeclareWord(0xA977, 0x9DCE, 0x0555, 0x6C42, 0x3286, 0x4FFE, 0x2734, 0x08AA);
		Instruction dd = Instruction.createDeclareDword(0x9DCEA977, 0x6C420555, 0x4FFE3286, 0x08AA2734);
		Instruction dq = Instruction.createDeclareQword(0x6C4205559DCEA977L, 0x08AA27344FFE3286L);
		byte[] data1 = getData(db);
		byte[] data2 = getData(dw);
		byte[] data4 = getData(dd);
		byte[] data8 = getData(dq);
		assertArrayEquals(data, data1);
		assertArrayEquals(data, data2);
		assertArrayEquals(data, data4);
		assertArrayEquals(data, data8);
	}

	@Test
	void declareByteCanGetSet() {
		Instruction db = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
		db.setDeclareByteValue(0, 0xE2);
		db.setDeclareByteValue(1, 0xC5);
		db.setDeclareByteValue(2, 0xFA);
		db.setDeclareByteValue(3, 0xB4);
		db.setDeclareByteValue(4, 0xCB);
		db.setDeclareByteValue(5, 0xE3);
		db.setDeclareByteValue(6, 0x4D);
		db.setDeclareByteValue(7, 0xE4);
		db.setDeclareByteValue(8, 0x96);
		db.setDeclareByteValue(9, 0x98);
		db.setDeclareByteValue(10, 0xFD);
		db.setDeclareByteValue(11, 0x56);
		db.setDeclareByteValue(12, 0x82);
		db.setDeclareByteValue(13, 0x8D);
		db.setDeclareByteValue(14, 0x06);
		db.setDeclareByteValue(15, 0xC3);
		assertEquals((byte)0xE2, db.getDeclareByteValue(0));
		assertEquals((byte)0xC5, db.getDeclareByteValue(1));
		assertEquals((byte)0xFA, db.getDeclareByteValue(2));
		assertEquals((byte)0xB4, db.getDeclareByteValue(3));
		assertEquals((byte)0xCB, db.getDeclareByteValue(4));
		assertEquals((byte)0xE3, db.getDeclareByteValue(5));
		assertEquals((byte)0x4D, db.getDeclareByteValue(6));
		assertEquals((byte)0xE4, db.getDeclareByteValue(7));
		assertEquals((byte)0x96, db.getDeclareByteValue(8));
		assertEquals((byte)0x98, db.getDeclareByteValue(9));
		assertEquals((byte)0xFD, db.getDeclareByteValue(10));
		assertEquals((byte)0x56, db.getDeclareByteValue(11));
		assertEquals((byte)0x82, db.getDeclareByteValue(12));
		assertEquals((byte)0x8D, db.getDeclareByteValue(13));
		assertEquals((byte)0x06, db.getDeclareByteValue(14));
		assertEquals((byte)0xC3, db.getDeclareByteValue(15));
	}

	@Test
	void declareByteCanGetSetRev() {
		Instruction db = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
		db.setDeclareByteValue(15, 0xC3);
		db.setDeclareByteValue(14, 0x06);
		db.setDeclareByteValue(13, 0x8D);
		db.setDeclareByteValue(12, 0x82);
		db.setDeclareByteValue(11, 0x56);
		db.setDeclareByteValue(10, 0xFD);
		db.setDeclareByteValue(9, 0x98);
		db.setDeclareByteValue(8, 0x96);
		db.setDeclareByteValue(7, 0xE4);
		db.setDeclareByteValue(6, 0x4D);
		db.setDeclareByteValue(5, 0xE3);
		db.setDeclareByteValue(4, 0xCB);
		db.setDeclareByteValue(3, 0xB4);
		db.setDeclareByteValue(2, 0xFA);
		db.setDeclareByteValue(1, 0xC5);
		db.setDeclareByteValue(0, 0xE2);
		assertEquals((byte)0xE2, db.getDeclareByteValue(0));
		assertEquals((byte)0xC5, db.getDeclareByteValue(1));
		assertEquals((byte)0xFA, db.getDeclareByteValue(2));
		assertEquals((byte)0xB4, db.getDeclareByteValue(3));
		assertEquals((byte)0xCB, db.getDeclareByteValue(4));
		assertEquals((byte)0xE3, db.getDeclareByteValue(5));
		assertEquals((byte)0x4D, db.getDeclareByteValue(6));
		assertEquals((byte)0xE4, db.getDeclareByteValue(7));
		assertEquals((byte)0x96, db.getDeclareByteValue(8));
		assertEquals((byte)0x98, db.getDeclareByteValue(9));
		assertEquals((byte)0xFD, db.getDeclareByteValue(10));
		assertEquals((byte)0x56, db.getDeclareByteValue(11));
		assertEquals((byte)0x82, db.getDeclareByteValue(12));
		assertEquals((byte)0x8D, db.getDeclareByteValue(13));
		assertEquals((byte)0x06, db.getDeclareByteValue(14));
		assertEquals((byte)0xC3, db.getDeclareByteValue(15));
	}

	@Test
	void declareWordCanGetSet() {
		Instruction dw = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
		dw.setDeclareWordValue(0, 0xE2C5);
		dw.setDeclareWordValue(1, 0xFAB4);
		dw.setDeclareWordValue(2, 0xCBE3);
		dw.setDeclareWordValue(3, 0x4DE4);
		dw.setDeclareWordValue(4, 0x9698);
		dw.setDeclareWordValue(5, 0xFD56);
		dw.setDeclareWordValue(6, 0x828D);
		dw.setDeclareWordValue(7, 0x06C3);
		assertEquals((short)0xE2C5, dw.getDeclareWordValue(0));
		assertEquals((short)0xFAB4, dw.getDeclareWordValue(1));
		assertEquals((short)0xCBE3, dw.getDeclareWordValue(2));
		assertEquals((short)0x4DE4, dw.getDeclareWordValue(3));
		assertEquals((short)0x9698, dw.getDeclareWordValue(4));
		assertEquals((short)0xFD56, dw.getDeclareWordValue(5));
		assertEquals((short)0x828D, dw.getDeclareWordValue(6));
		assertEquals((short)0x06C3, dw.getDeclareWordValue(7));
	}

	@Test
	void declareWordCanGetSetRev() {
		Instruction dw = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
		dw.setDeclareWordValue(7, 0x06C3);
		dw.setDeclareWordValue(6, 0x828D);
		dw.setDeclareWordValue(5, 0xFD56);
		dw.setDeclareWordValue(4, 0x9698);
		dw.setDeclareWordValue(3, 0x4DE4);
		dw.setDeclareWordValue(2, 0xCBE3);
		dw.setDeclareWordValue(1, 0xFAB4);
		dw.setDeclareWordValue(0, 0xE2C5);
		assertEquals((short)0xE2C5, dw.getDeclareWordValue(0));
		assertEquals((short)0xFAB4, dw.getDeclareWordValue(1));
		assertEquals((short)0xCBE3, dw.getDeclareWordValue(2));
		assertEquals((short)0x4DE4, dw.getDeclareWordValue(3));
		assertEquals((short)0x9698, dw.getDeclareWordValue(4));
		assertEquals((short)0xFD56, dw.getDeclareWordValue(5));
		assertEquals((short)0x828D, dw.getDeclareWordValue(6));
		assertEquals((short)0x06C3, dw.getDeclareWordValue(7));
	}

	@Test
	void declareDwordCanGetSet() {
		Instruction dd = Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
		dd.setDeclareDwordValue(0, 0xE2C5FAB4);
		dd.setDeclareDwordValue(1, 0xCBE34DE4);
		dd.setDeclareDwordValue(2, 0x9698FD56);
		dd.setDeclareDwordValue(3, 0x828D06C3);
		assertEquals(0xE2C5FAB4, dd.getDeclareDwordValue(0));
		assertEquals(0xCBE34DE4, dd.getDeclareDwordValue(1));
		assertEquals(0x9698FD56, dd.getDeclareDwordValue(2));
		assertEquals(0x828D06C3, dd.getDeclareDwordValue(3));
	}

	@Test
	void declareDwordCanGetSetRev() {
		Instruction dd = Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
		dd.setDeclareDwordValue(3, 0x828D06C3);
		dd.setDeclareDwordValue(2, 0x9698FD56);
		dd.setDeclareDwordValue(1, 0xCBE34DE4);
		dd.setDeclareDwordValue(0, 0xE2C5FAB4);
		assertEquals(0xE2C5FAB4, dd.getDeclareDwordValue(0));
		assertEquals(0xCBE34DE4, dd.getDeclareDwordValue(1));
		assertEquals(0x9698FD56, dd.getDeclareDwordValue(2));
		assertEquals(0x828D06C3, dd.getDeclareDwordValue(3));
	}

	@Test
	void declareQwordCanGetSet() {
		Instruction dq = Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L);
		dq.setDeclareQwordValue(0, 0xE2C5FAB4CBE34DE4L);
		dq.setDeclareQwordValue(1, 0x9698FD56828D06C3L);
		assertEquals(0xE2C5FAB4CBE34DE4L, dq.getDeclareQwordValue(0));
		assertEquals(0x9698FD56828D06C3L, dq.getDeclareQwordValue(1));
	}

	@Test
	void declareQwordCanGetSetRev() {
		Instruction dq = Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L);
		dq.setDeclareQwordValue(1, 0x9698FD56828D06C3L);
		dq.setDeclareQwordValue(0, 0xE2C5FAB4CBE34DE4L);
		assertEquals(0xE2C5FAB4CBE34DE4L, dq.getDeclareQwordValue(0));
		assertEquals(0x9698FD56828D06C3L, dq.getDeclareQwordValue(1));
	}

	@Test
	void declareDataDoesNotUseOtherProperties() {
		Instruction instruction;

		byte[] data = new byte[16];
		for (int i = 0; i < data.length; i++)
			data[i] = (byte)0xFF;

		Consumer<Instruction> verify = instr -> {
			assertEquals(Register.NONE, instr.getSegmentPrefix());
			assertEquals(CodeSize.UNKNOWN, instr.getCodeSize());
			assertEquals(RoundingControl.NONE, instr.getRoundingControl());
			assertEquals(0L, instr.getIP());
			assertFalse(instr.getBroadcast());
			assertFalse(instr.hasOpMask());
			assertFalse(instr.getSuppressAllExceptions());
			assertFalse(instr.getZeroingMasking());
			assertFalse(instr.getXacquirePrefix());
			assertFalse(instr.getXreleasePrefix());
			assertFalse(instr.getRepPrefix());
			assertFalse(instr.getRepePrefix());
			assertFalse(instr.getRepnePrefix());
			assertFalse(instr.getLockPrefix());
		};

		instruction = Instruction.createDeclareByte(data);
		verify.accept(instruction);

		instruction = Instruction.createDeclareWord(data);
		verify.accept(instruction);

		instruction = Instruction.createDeclareDword(data);
		verify.accept(instruction);

		instruction = Instruction.createDeclareQword(data);
		verify.accept(instruction);
	}

	@Test
	void createDeclareByte() {
		Instruction instruction;

		instruction = Instruction.createDeclareByte(0x77);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(1, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(2, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(3, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(4, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(5, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(6, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(7, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(8, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(9, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(10, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(11, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(12, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(13, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(14, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27 }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(15, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, (byte)0xAA }, getData(instruction));

		instruction = Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08);
		assertEquals(Code.DECLAREBYTE, instruction.getCode());
		assertEquals(16, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, (byte)0xAA, 0x08 }, getData(instruction));
	}

	@Test
	void createDeclareWord() {
		Instruction instruction;

		instruction = Instruction.createDeclareWord(0x77A9);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(1, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77 }, getData(instruction));

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(2, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE }, getData(instruction));

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(3, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55 }, getData(instruction));

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(4, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42 }, getData(instruction));

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(5, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86 }, getData(instruction));

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(6, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE }, getData(instruction));

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(7, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE, 0x27, 0x34 }, getData(instruction));

		instruction = Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08);
		assertEquals(Code.DECLAREWORD, instruction.getCode());
		assertEquals(8, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE, 0x27, 0x34, 0x08, (byte)0xAA }, getData(instruction));
	}

	@Test
	void createDeclareDword() {
		Instruction instruction;

		instruction = Instruction.createDeclareDword(0x77A9CE9D);
		assertEquals(Code.DECLAREDWORD, instruction.getCode());
		assertEquals(1, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77 }, getData(instruction));

		instruction = Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C);
		assertEquals(Code.DECLAREDWORD, instruction.getCode());
		assertEquals(2, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55 }, getData(instruction));

		instruction = Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F);
		assertEquals(Code.DECLAREDWORD, instruction.getCode());
		assertEquals(3, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, (byte)0xFE, 0x32, (byte)0x86 }, getData(instruction));

		instruction = Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08);
		assertEquals(Code.DECLAREDWORD, instruction.getCode());
		assertEquals(4, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, (byte)0xFE, 0x32, (byte)0x86, 0x08, (byte)0xAA, 0x27, 0x34 }, getData(instruction));
	}

	@Test
	void createDeclareQword() {
		Instruction instruction;

		instruction = Instruction.createDeclareQword(0x77A9CE9D5505426CL);
		assertEquals(Code.DECLAREQWORD, instruction.getCode());
		assertEquals(1, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x6C, 0x42, 0x05, 0x55, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77 }, getData(instruction));

		instruction = Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L);
		assertEquals(Code.DECLAREQWORD, instruction.getCode());
		assertEquals(2, instruction.getDeclareDataCount());
		assertArrayEquals(new byte[] { 0x6C, 0x42, 0x05, 0x55, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x08, (byte)0xAA, 0x27, 0x34, 0x4F, (byte)0xFE, 0x32, (byte)0x86 }, getData(instruction));
	}

	static class InstrByteArr {
		public Instruction instruction;
		public byte[] data;
		public InstrByteArr(Instruction instruction, byte[] data) {
			this.instruction = instruction;
			this.data = data;
		}
	}

	static class InstrShortArr {
		public Instruction instruction;
		public short[] data;
		public InstrShortArr(Instruction instruction, short[] data) {
			this.instruction = instruction;
			this.data = data;
		}
	}

	static class InstrIntArr {
		public Instruction instruction;
		public int[] data;
		public InstrIntArr(Instruction instruction, int[] data) {
			this.instruction = instruction;
			this.data = data;
		}
	}

	static class InstrLongArr {
		public Instruction instruction;
		public long[] data;
		public InstrLongArr(Instruction instruction, long[] data) {
			this.instruction = instruction;
			this.data = data;
		}
	}

	@Test
	void createDeclareByteArray() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareByte(0x77), new byte[] { 0x77 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9), new byte[] { 0x77, (byte)0xA9 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE), new byte[] { 0x77, (byte)0xA9, (byte)0xCE }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27 }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, (byte)0xAA }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08), new byte[] { 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, (byte)0xAA, 0x08 }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareByte(info.data);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareWordArray() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareWord(0x77A9), new byte[] { (byte)0xA9, 0x77 }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D), new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505), new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55 }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42 }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86 }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE, 0x27, 0x34 }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new byte[] { (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE, 0x27, 0x34, 0x08, (byte)0xAA }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareWord(info.data);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareDwordArray() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D), new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77 }),
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C), new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55 }),
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, (byte)0xFE, 0x32, (byte)0x86 }),
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new byte[] { (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, (byte)0xFE, 0x32, (byte)0x86, 0x08, (byte)0xAA, 0x27, 0x34 }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareDword(info.data);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareQwordArray() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL), new byte[] { 0x6C, 0x42, 0x05, 0x55, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77 }),
			new InstrByteArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L), new byte[] { 0x6C, 0x42, 0x05, 0x55, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x08, (byte)0xAA, 0x27, 0x34, 0x4F, (byte)0xFE, 0x32, (byte)0x86 }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareQword(info.data);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareByteArray2() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareByte(0x77), new byte[] { (byte)0xA5, 0x77, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, (byte)0xAA, 0x5A }),
			new InstrByteArr(Instruction.createDeclareByte(0x77, 0xA9, 0xCE, 0x9D, 0x55, 0x05, 0x42, 0x6C, 0x86, 0x32, 0xFE, 0x4F, 0x34, 0x27, 0xAA, 0x08), new byte[] { (byte)0xA5, 0x77, (byte)0xA9, (byte)0xCE, (byte)0x9D, 0x55, 0x05, 0x42, 0x6C, (byte)0x86, 0x32, (byte)0xFE, 0x4F, 0x34, 0x27, (byte)0xAA, 0x08, 0x5A }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareByte(info.data, 1, info.data.length - 2);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareWordArray2() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareWord(0x77A9), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, 0x5A }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x5A }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x5A }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x5A }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x5A }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE, 0x5A }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE, 0x27, 0x34, 0x5A }),
			new InstrByteArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new byte[] { (byte)0xA5, (byte)0xA9, 0x77, (byte)0x9D, (byte)0xCE, 0x05, 0x55, 0x6C, 0x42, 0x32, (byte)0x86, 0x4F, (byte)0xFE, 0x27, 0x34, 0x08, (byte)0xAA, 0x5A }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareWord(info.data, 1, info.data.length - 2);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareDwordArray2() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D), new byte[] { (byte)0xA5, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x5A }),
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C), new byte[] { (byte)0xA5, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x5A }),
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new byte[] { (byte)0xA5, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, (byte)0xFE, 0x32, (byte)0x86, 0x5A }),
			new InstrByteArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new byte[] { (byte)0xA5, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x6C, 0x42, 0x05, 0x55, 0x4F, (byte)0xFE, 0x32, (byte)0x86, 0x08, (byte)0xAA, 0x27, 0x34, 0x5A }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareDword(info.data, 1, info.data.length - 2);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareQwordArray2() {
		InstrByteArr[] data = new InstrByteArr[] {
			new InstrByteArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL), new byte[] { (byte)0xA5, 0x6C, 0x42, 0x05, 0x55, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x5A }),
			new InstrByteArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L), new byte[] { (byte)0xA5, 0x6C, 0x42, 0x05, 0x55, (byte)0x9D, (byte)0xCE, (byte)0xA9, 0x77, 0x08, (byte)0xAA, 0x27, 0x34, 0x4F, (byte)0xFE, 0x32, (byte)0x86, 0x5A }),
		};
		for (InstrByteArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareQword(info.data, 1, info.data.length - 2);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareWordArray3() {
		InstrShortArr[] data = new InstrShortArr[] {
			new InstrShortArr(Instruction.createDeclareWord(0x77A9), new short[] { 0x77A9 }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D), new short[] { 0x77A9, (short)0xCE9D }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505), new short[] { 0x77A9, (short)0xCE9D, 0x5505 }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new short[] { 0x77A9, (short)0xCE9D, 0x5505, 0x426C }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new short[] { 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632 }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new short[] { 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632, (short)0xFE4F }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new short[] { 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632, (short)0xFE4F, 0x3427 }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new short[] { 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632, (short)0xFE4F, 0x3427, (short)0xAA08 }),
		};
		for (InstrShortArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareWord(info.data);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareDwordArray3() {
		InstrIntArr[] data = new InstrIntArr[] {
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D), new int[] { 0x77A9CE9D }),
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C), new int[] { 0x77A9CE9D, 0x5505426C }),
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new int[] { 0x77A9CE9D, 0x5505426C, 0x8632FE4F }),
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new int[] { 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08 }),
		};
		for (InstrIntArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareDword(info.data);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareQwordArray3() {
		InstrLongArr[] data = new InstrLongArr[] {
			new InstrLongArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL), new long[] { 0x77A9CE9D5505426CL }),
			new InstrLongArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L), new long[] { 0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L }),
		};
		for (InstrLongArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareQword(info.data);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareWordArray4() {
		InstrShortArr[] data = new InstrShortArr[] {
			new InstrShortArr(Instruction.createDeclareWord(0x77A9), new short[] { 0x5AA5, 0x77A9, (short)0xA55A }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D), new short[] { 0x5AA5, 0x77A9, (short)0xCE9D, (short)0xA55A }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505), new short[] { 0x5AA5, 0x77A9, (short)0xCE9D, 0x5505, (short)0xA55A }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C), new short[] { 0x5AA5, 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0xA55A }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632), new short[] { 0x5AA5, 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632, (short)0xA55A }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F), new short[] { 0x5AA5, 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632, (short)0xFE4F, (short)0xA55A }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427), new short[] { 0x5AA5, 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632, (short)0xFE4F, 0x3427, (short)0xA55A }),
			new InstrShortArr(Instruction.createDeclareWord(0x77A9, 0xCE9D, 0x5505, 0x426C, 0x8632, 0xFE4F, 0x3427, 0xAA08), new short[] { 0x5AA5, 0x77A9, (short)0xCE9D, 0x5505, 0x426C, (short)0x8632, (short)0xFE4F, 0x3427, (short)0xAA08, (short)0xA55A }),
		};
		for (InstrShortArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareWord(info.data, 1, info.data.length - 2);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareDwordArray4() {
		InstrIntArr[] data = new InstrIntArr[] {
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D), new int[] { 0x5AA5A55A, 0x77A9CE9D, 0xA55A5AA5 }),
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C), new int[] { 0x5AA5A55A, 0x77A9CE9D, 0x5505426C, 0xA55A5AA5 }),
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F), new int[] { 0x5AA5A55A, 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0xA55A5AA5 }),
			new InstrIntArr(Instruction.createDeclareDword(0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08), new int[] { 0x5AA5A55A, 0x77A9CE9D, 0x5505426C, 0x8632FE4F, 0x3427AA08, 0xA55A5AA5 }),
		};
		for (InstrIntArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareDword(info.data, 1, info.data.length - 2);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@Test
	void createDeclareQwordArray4() {
		InstrLongArr[] data = new InstrLongArr[] {
			new InstrLongArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL), new long[] { 0x5AA5A55A5AA5A55AL, 0x77A9CE9D5505426CL, 0xA55A5AA5A55A5AA5L }),
			new InstrLongArr(Instruction.createDeclareQword(0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L), new long[] { 0x5AA5A55A5AA5A55AL, 0x77A9CE9D5505426CL, 0x8632FE4F3427AA08L, 0xA55A5AA5A55A5AA5L }),
		};
		for (InstrLongArr info : data) {
			Instruction instruction1 = info.instruction;
			Instruction instruction2 = Instruction.createDeclareQword(info.data, 1, info.data.length - 2);
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	@ParameterizedTest
	@MethodSource("createTestData")
	void createTest(int bitness, String hexBytes, int options, Instruction createdInstr) {
		byte[] bytes = HexUtils.toByteArray(hexBytes);
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(bytes), options);
		switch (bitness) {
		case 16:
			decoder.setIP(DecoderConstants.DEFAULT_IP16);
			break;
		case 32:
			decoder.setIP(DecoderConstants.DEFAULT_IP32);
			break;
		case 64:
			decoder.setIP(DecoderConstants.DEFAULT_IP64);
			break;
		default:
			throw new UnsupportedOperationException();
		}
		long origRip = decoder.getIP();
		Instruction decodedInstr = decoder.decode();
		decodedInstr.setCodeSize(0);
		decodedInstr.setLength(0);
		decodedInstr.setNextIP(0);

		assertTrue(decodedInstr.equalsAllBits(createdInstr));

		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(decoder.getBitness(), writer);
		Object result = encoder.tryEncode(createdInstr, origRip);
		assertTrue(result instanceof Integer);
		assertArrayEquals(bytes, writer.toArray());
	}

	public static Iterable<Arguments> createTestData() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		result.add(Arguments.of(64, "90", DecoderOptions.NONE, Instruction.create(Code.NOPD)));
		result.add(Arguments.of(64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions.NONE, Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, -1L)));
		result.add(Arguments.of(64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions.NONE, Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, -1)));
		result.add(Arguments.of(64, "48B9123456789ABCDE31", DecoderOptions.NONE, Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, 0x31DEBC9A78563412L)));
		result.add(Arguments.of(64, "48B9FFFFFFFF00000000", DecoderOptions.NONE, Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, 0xFFFFFFFFL)));
		result.add(Arguments.of(64, "8FC1", DecoderOptions.NONE, Instruction.create(Code.POP_RM64, ICRegisters.rcx)));
		result.add(Arguments.of(64, "648F847501EFCDAB", DecoderOptions.NONE, Instruction.create(Code.POP_RM64, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs))));
		result.add(Arguments.of(64, "C6F85A", DecoderOptions.NONE, Instruction.create(Code.XABORT_IMM8, 0x5A)));
		result.add(Arguments.of(64, "66685AA5", DecoderOptions.NONE, Instruction.create(Code.PUSH_IMM16, 0xA55A)));
		result.add(Arguments.of(32, "685AA51234", DecoderOptions.NONE, Instruction.create(Code.PUSHD_IMM32, 0x3412A55A)));
		result.add(Arguments.of(64, "666A5A", DecoderOptions.NONE, Instruction.create(Code.PUSHW_IMM8, 0x5A)));
		result.add(Arguments.of(32, "6A5A", DecoderOptions.NONE, Instruction.create(Code.PUSHD_IMM8, 0x5A)));
		result.add(Arguments.of(64, "6A5A", DecoderOptions.NONE, Instruction.create(Code.PUSHQ_IMM8, 0x5A)));
		result.add(Arguments.of(64, "685AA512A4", DecoderOptions.NONE, Instruction.create(Code.PUSHQ_IMM32, -0x5BED5AA6)));
		result.add(Arguments.of(32, "66705A", DecoderOptions.NONE, Instruction.createBranch(Code.JO_REL8_16, 0x4D)));
		result.add(Arguments.of(32, "705A", DecoderOptions.NONE, Instruction.createBranch(Code.JO_REL8_32, 0x8000004CL)));
		result.add(Arguments.of(64, "705A", DecoderOptions.NONE, Instruction.createBranch(Code.JO_REL8_64, 0x800000000000004CL)));
		result.add(Arguments.of(32, "669A12345678", DecoderOptions.NONE, Instruction.createBranch(Code.CALL_PTR1616, 0x7856, 0x3412)));
		result.add(Arguments.of(32, "9A123456789ABC", DecoderOptions.NONE, Instruction.createBranch(Code.CALL_PTR1632, 0xBC9A, 0x78563412)));
		result.add(Arguments.of(16, "C7F85AA5", DecoderOptions.NONE, Instruction.createXbegin(16, 0x254E)));
		result.add(Arguments.of(32, "C7F85AA51234", DecoderOptions.NONE, Instruction.createXbegin(32, 0xB412A550)));
		result.add(Arguments.of(64, "C7F85AA51234", DecoderOptions.NONE, Instruction.createXbegin(64, 0x800000003412A550L)));
		result.add(Arguments.of(64, "00D1", DecoderOptions.NONE, Instruction.create(Code.ADD_RM8_R8, ICRegisters.cl, ICRegisters.dl)));
		result.add(Arguments.of(64, "64028C7501EFCDAB", DecoderOptions.NONE, Instruction.create(Code.ADD_R8_RM8, ICRegisters.cl, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs))));
		result.add(Arguments.of(64, "80C15A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, 0x5A)));
		result.add(Arguments.of(64, "6681C15AA5", DecoderOptions.NONE, Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, 0xA55A)));
		result.add(Arguments.of(64, "81C15AA51234", DecoderOptions.NONE, Instruction.create(Code.ADD_RM32_IMM32, ICRegisters.ecx, 0x3412A55A)));
		result.add(Arguments.of(64, "48B904152637A55A5678", DecoderOptions.NONE, Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, 0x78565AA537261504L)));
		result.add(Arguments.of(64, "6683C15A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, 0x5A)));
		result.add(Arguments.of(64, "83C15A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, 0x5A)));
		result.add(Arguments.of(64, "4883C15A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, 0x5A)));
		result.add(Arguments.of(64, "4881C15AA51234", DecoderOptions.NONE, Instruction.create(Code.ADD_RM64_IMM32, ICRegisters.rcx, 0x3412A55A)));
		result.add(Arguments.of(64, "64A0123456789ABCDEF0", DecoderOptions.NONE, Instruction.create(Code.MOV_AL_MOFFS8, ICRegisters.al, new MemoryOperand(ICRegister.NONE, 0xF0DEBC9A78563412L, 8, false, ICRegisters.fs))));
		result.add(Arguments.of(64, "6400947501EFCDAB", DecoderOptions.NONE, Instruction.create(Code.ADD_RM8_R8, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), ICRegisters.dl)));
		result.add(Arguments.of(64, "6480847501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM8_IMM8, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x5A)));
		result.add(Arguments.of(64, "646681847501EFCDAB5AA5", DecoderOptions.NONE, Instruction.create(Code.ADD_RM16_IMM16, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0xA55A)));
		result.add(Arguments.of(64, "6481847501EFCDAB5AA51234", DecoderOptions.NONE, Instruction.create(Code.ADD_RM32_IMM32, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x3412A55A)));
		result.add(Arguments.of(64, "646683847501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM16_IMM8, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x5A)));
		result.add(Arguments.of(64, "6483847501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM32_IMM8, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x5A)));
		result.add(Arguments.of(64, "644883847501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.ADD_RM64_IMM8, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x5A)));
		result.add(Arguments.of(64, "644881847501EFCDAB5AA51234", DecoderOptions.NONE, Instruction.create(Code.ADD_RM64_IMM32, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x3412A55A)));
		result.add(Arguments.of(64, "E65A", DecoderOptions.NONE, Instruction.create(Code.OUT_IMM8_AL, 0x5A, ICRegisters.al)));
		result.add(Arguments.of(64, "66C85AA5A6", DecoderOptions.NONE, Instruction.create(Code.ENTERW_IMM16_IMM8, 0xA55A, 0xA6)));
		result.add(Arguments.of(64, "64A2123456789ABCDEF0", DecoderOptions.NONE, Instruction.create(Code.MOV_MOFFS8_AL, new MemoryOperand(ICRegister.NONE, 0xF0DEBC9A78563412L, 8, false, ICRegisters.fs), ICRegisters.al)));
		result.add(Arguments.of(64, "6669CAA55A", DecoderOptions.NONE, Instruction.create(Code.IMUL_R16_RM16_IMM16, ICRegisters.cx, ICRegisters.dx, 0x5AA5)));
		result.add(Arguments.of(64, "69CA5AA51234", DecoderOptions.NONE, Instruction.create(Code.IMUL_R32_RM32_IMM32, ICRegisters.ecx, ICRegisters.edx, 0x3412A55A)));
		result.add(Arguments.of(64, "666BCA5A", DecoderOptions.NONE, Instruction.create(Code.IMUL_R16_RM16_IMM8, ICRegisters.cx, ICRegisters.dx, 0x5A)));
		result.add(Arguments.of(64, "6BCA5A", DecoderOptions.NONE, Instruction.create(Code.IMUL_R32_RM32_IMM8, ICRegisters.ecx, ICRegisters.edx, 0x5A)));
		result.add(Arguments.of(64, "486BCA5A", DecoderOptions.NONE, Instruction.create(Code.IMUL_R64_RM64_IMM8, ICRegisters.rcx, ICRegisters.rdx, 0x5A)));
		result.add(Arguments.of(64, "4869CA5AA512A4", DecoderOptions.NONE, Instruction.create(Code.IMUL_R64_RM64_IMM32, ICRegisters.rcx, ICRegisters.rdx, -0x5BED5AA6)));
		result.add(Arguments.of(64, "6466698C7501EFCDAB5AA5", DecoderOptions.NONE, Instruction.create(Code.IMUL_R16_RM16_IMM16, ICRegisters.cx, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0xA55A)));
		result.add(Arguments.of(64, "64698C7501EFCDAB5AA51234", DecoderOptions.NONE, Instruction.create(Code.IMUL_R32_RM32_IMM32, ICRegisters.ecx, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x3412A55A)));
		result.add(Arguments.of(64, "64666B8C7501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.IMUL_R16_RM16_IMM8, ICRegisters.cx, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x5A)));
		result.add(Arguments.of(64, "646B8C7501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.IMUL_R32_RM32_IMM8, ICRegisters.ecx, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x5A)));
		result.add(Arguments.of(64, "64486B8C7501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.IMUL_R64_RM64_IMM8, ICRegisters.rcx, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x5A)));
		result.add(Arguments.of(64, "6448698C7501EFCDAB5AA512A4", DecoderOptions.NONE, Instruction.create(Code.IMUL_R64_RM64_IMM32, ICRegisters.rcx, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), -0x5BED5AA6)));
		result.add(Arguments.of(64, "660F78C1A5FD", DecoderOptions.NONE, Instruction.create(Code.EXTRQ_XMM_IMM8_IMM8, ICRegisters.xmm1, 0xA5, 0xFD)));
		result.add(Arguments.of(64, "64660FA4947501EFCDAB5A", DecoderOptions.NONE, Instruction.create(Code.SHLD_RM16_R16_IMM8, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), ICRegisters.dx, 0x5A)));
		result.add(Arguments.of(64, "F20F78CAA5FD", DecoderOptions.NONE, Instruction.create(Code.INSERTQ_XMM_XMM_IMM8_IMM8, ICRegisters.xmm1, ICRegisters.xmm2, 0xA5, 0xFD)));
		result.add(Arguments.of(16, "0FB855AA", DecoderOptions.JMPE, Instruction.createBranch(Code.JMPE_DISP16, 0xAA55)));
		result.add(Arguments.of(32, "0FB8123455AA", DecoderOptions.JMPE, Instruction.createBranch(Code.JMPE_DISP32, 0xAA553412L)));
		result.add(Arguments.of(32, "64676E", DecoderOptions.NONE, Instruction.createOutsb(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "64676E", DecoderOptions.NONE, Instruction.createOutsb(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "646E", DecoderOptions.NONE, Instruction.createOutsb(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "6466676F", DecoderOptions.NONE, Instruction.createOutsw(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "6466676F", DecoderOptions.NONE, Instruction.createOutsw(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "64666F", DecoderOptions.NONE, Instruction.createOutsw(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "64676F", DecoderOptions.NONE, Instruction.createOutsd(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "64676F", DecoderOptions.NONE, Instruction.createOutsd(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "646F", DecoderOptions.NONE, Instruction.createOutsd(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "67AE", DecoderOptions.NONE, Instruction.createScasb(16)));
		result.add(Arguments.of(64, "67AE", DecoderOptions.NONE, Instruction.createScasb(32)));
		result.add(Arguments.of(64, "AE", DecoderOptions.NONE, Instruction.createScasb(64)));
		result.add(Arguments.of(32, "6667AF", DecoderOptions.NONE, Instruction.createScasw(16)));
		result.add(Arguments.of(64, "6667AF", DecoderOptions.NONE, Instruction.createScasw(32)));
		result.add(Arguments.of(64, "66AF", DecoderOptions.NONE, Instruction.createScasw(64)));
		result.add(Arguments.of(32, "67AF", DecoderOptions.NONE, Instruction.createScasd(16)));
		result.add(Arguments.of(64, "67AF", DecoderOptions.NONE, Instruction.createScasd(32)));
		result.add(Arguments.of(64, "AF", DecoderOptions.NONE, Instruction.createScasd(64)));
		result.add(Arguments.of(64, "6748AF", DecoderOptions.NONE, Instruction.createScasq(32)));
		result.add(Arguments.of(64, "48AF", DecoderOptions.NONE, Instruction.createScasq(64)));
		result.add(Arguments.of(32, "6467AC", DecoderOptions.NONE, Instruction.createLodsb(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467AC", DecoderOptions.NONE, Instruction.createLodsb(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "64AC", DecoderOptions.NONE, Instruction.createLodsb(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "646667AD", DecoderOptions.NONE, Instruction.createLodsw(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "646667AD", DecoderOptions.NONE, Instruction.createLodsw(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "6466AD", DecoderOptions.NONE, Instruction.createLodsw(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "6467AD", DecoderOptions.NONE, Instruction.createLodsd(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467AD", DecoderOptions.NONE, Instruction.createLodsd(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "64AD", DecoderOptions.NONE, Instruction.createLodsd(64, ICRegisters.fs)));
		result.add(Arguments.of(64, "646748AD", DecoderOptions.NONE, Instruction.createLodsq(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "6448AD", DecoderOptions.NONE, Instruction.createLodsq(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "676C", DecoderOptions.NONE, Instruction.createInsb(16)));
		result.add(Arguments.of(64, "676C", DecoderOptions.NONE, Instruction.createInsb(32)));
		result.add(Arguments.of(64, "6C", DecoderOptions.NONE, Instruction.createInsb(64)));
		result.add(Arguments.of(32, "66676D", DecoderOptions.NONE, Instruction.createInsw(16)));
		result.add(Arguments.of(64, "66676D", DecoderOptions.NONE, Instruction.createInsw(32)));
		result.add(Arguments.of(64, "666D", DecoderOptions.NONE, Instruction.createInsw(64)));
		result.add(Arguments.of(32, "676D", DecoderOptions.NONE, Instruction.createInsd(16)));
		result.add(Arguments.of(64, "676D", DecoderOptions.NONE, Instruction.createInsd(32)));
		result.add(Arguments.of(64, "6D", DecoderOptions.NONE, Instruction.createInsd(64)));
		result.add(Arguments.of(32, "67AA", DecoderOptions.NONE, Instruction.createStosb(16)));
		result.add(Arguments.of(64, "67AA", DecoderOptions.NONE, Instruction.createStosb(32)));
		result.add(Arguments.of(64, "AA", DecoderOptions.NONE, Instruction.createStosb(64)));
		result.add(Arguments.of(32, "6667AB", DecoderOptions.NONE, Instruction.createStosw(16)));
		result.add(Arguments.of(64, "6667AB", DecoderOptions.NONE, Instruction.createStosw(32)));
		result.add(Arguments.of(64, "66AB", DecoderOptions.NONE, Instruction.createStosw(64)));
		result.add(Arguments.of(32, "67AB", DecoderOptions.NONE, Instruction.createStosd(16)));
		result.add(Arguments.of(64, "67AB", DecoderOptions.NONE, Instruction.createStosd(32)));
		result.add(Arguments.of(64, "AB", DecoderOptions.NONE, Instruction.createStosd(64)));
		result.add(Arguments.of(64, "6748AB", DecoderOptions.NONE, Instruction.createStosq(32)));
		result.add(Arguments.of(64, "48AB", DecoderOptions.NONE, Instruction.createStosq(64)));
		result.add(Arguments.of(32, "6467A6", DecoderOptions.NONE, Instruction.createCmpsb(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467A6", DecoderOptions.NONE, Instruction.createCmpsb(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "64A6", DecoderOptions.NONE, Instruction.createCmpsb(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "646667A7", DecoderOptions.NONE, Instruction.createCmpsw(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "646667A7", DecoderOptions.NONE, Instruction.createCmpsw(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "6466A7", DecoderOptions.NONE, Instruction.createCmpsw(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "6467A7", DecoderOptions.NONE, Instruction.createCmpsd(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467A7", DecoderOptions.NONE, Instruction.createCmpsd(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "64A7", DecoderOptions.NONE, Instruction.createCmpsd(64, ICRegisters.fs)));
		result.add(Arguments.of(64, "646748A7", DecoderOptions.NONE, Instruction.createCmpsq(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "6448A7", DecoderOptions.NONE, Instruction.createCmpsq(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "6467A4", DecoderOptions.NONE, Instruction.createMovsb(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467A4", DecoderOptions.NONE, Instruction.createMovsb(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "64A4", DecoderOptions.NONE, Instruction.createMovsb(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "646667A5", DecoderOptions.NONE, Instruction.createMovsw(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "646667A5", DecoderOptions.NONE, Instruction.createMovsw(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "6466A5", DecoderOptions.NONE, Instruction.createMovsw(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "6467A5", DecoderOptions.NONE, Instruction.createMovsd(16, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467A5", DecoderOptions.NONE, Instruction.createMovsd(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "64A5", DecoderOptions.NONE, Instruction.createMovsd(64, ICRegisters.fs)));
		result.add(Arguments.of(64, "646748A5", DecoderOptions.NONE, Instruction.createMovsq(32, ICRegisters.fs)));
		result.add(Arguments.of(64, "6448A5", DecoderOptions.NONE, Instruction.createMovsq(64, ICRegisters.fs)));
		result.add(Arguments.of(32, "64670FF7D3", DecoderOptions.NONE, Instruction.createMaskmovq(16, ICRegisters.mm2, ICRegisters.mm3, ICRegisters.fs)));
		result.add(Arguments.of(64, "64670FF7D3", DecoderOptions.NONE, Instruction.createMaskmovq(32, ICRegisters.mm2, ICRegisters.mm3, ICRegisters.fs)));
		result.add(Arguments.of(64, "640FF7D3", DecoderOptions.NONE, Instruction.createMaskmovq(64, ICRegisters.mm2, ICRegisters.mm3, ICRegisters.fs)));
		result.add(Arguments.of(32, "6467660FF7D3", DecoderOptions.NONE, Instruction.createMaskmovdqu(16, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467660FF7D3", DecoderOptions.NONE, Instruction.createMaskmovdqu(32, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs)));
		result.add(Arguments.of(64, "64660FF7D3", DecoderOptions.NONE, Instruction.createMaskmovdqu(64, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs)));

		result.add(Arguments.of(32, "6467F36E", DecoderOptions.NONE, Instruction.createOutsb(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F36E", DecoderOptions.NONE, Instruction.createOutsb(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F36E", DecoderOptions.NONE, Instruction.createOutsb(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "646667F36F", DecoderOptions.NONE, Instruction.createOutsw(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "646667F36F", DecoderOptions.NONE, Instruction.createOutsw(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6466F36F", DecoderOptions.NONE, Instruction.createOutsw(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6467F36F", DecoderOptions.NONE, Instruction.createOutsd(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F36F", DecoderOptions.NONE, Instruction.createOutsd(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F36F", DecoderOptions.NONE, Instruction.createOutsd(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "67F3AE", DecoderOptions.NONE, Instruction.createScasb(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F3AE", DecoderOptions.NONE, Instruction.createScasb(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F3AE", DecoderOptions.NONE, Instruction.createScasb(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6667F3AF", DecoderOptions.NONE, Instruction.createScasw(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6667F3AF", DecoderOptions.NONE, Instruction.createScasw(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "66F3AF", DecoderOptions.NONE, Instruction.createScasw(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "67F3AF", DecoderOptions.NONE, Instruction.createScasd(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F3AF", DecoderOptions.NONE, Instruction.createScasd(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F3AF", DecoderOptions.NONE, Instruction.createScasd(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F348AF", DecoderOptions.NONE, Instruction.createScasq(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F348AF", DecoderOptions.NONE, Instruction.createScasq(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6467F3AC", DecoderOptions.NONE, Instruction.createLodsb(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F3AC", DecoderOptions.NONE, Instruction.createLodsb(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F3AC", DecoderOptions.NONE, Instruction.createLodsb(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "646667F3AD", DecoderOptions.NONE, Instruction.createLodsw(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "646667F3AD", DecoderOptions.NONE, Instruction.createLodsw(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6466F3AD", DecoderOptions.NONE, Instruction.createLodsw(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6467F3AD", DecoderOptions.NONE, Instruction.createLodsd(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F3AD", DecoderOptions.NONE, Instruction.createLodsd(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F3AD", DecoderOptions.NONE, Instruction.createLodsd(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F348AD", DecoderOptions.NONE, Instruction.createLodsq(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F348AD", DecoderOptions.NONE, Instruction.createLodsq(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "67F36C", DecoderOptions.NONE, Instruction.createInsb(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F36C", DecoderOptions.NONE, Instruction.createInsb(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F36C", DecoderOptions.NONE, Instruction.createInsb(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6667F36D", DecoderOptions.NONE, Instruction.createInsw(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6667F36D", DecoderOptions.NONE, Instruction.createInsw(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "66F36D", DecoderOptions.NONE, Instruction.createInsw(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "67F36D", DecoderOptions.NONE, Instruction.createInsd(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F36D", DecoderOptions.NONE, Instruction.createInsd(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F36D", DecoderOptions.NONE, Instruction.createInsd(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "67F3AA", DecoderOptions.NONE, Instruction.createStosb(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F3AA", DecoderOptions.NONE, Instruction.createStosb(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F3AA", DecoderOptions.NONE, Instruction.createStosb(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6667F3AB", DecoderOptions.NONE, Instruction.createStosw(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6667F3AB", DecoderOptions.NONE, Instruction.createStosw(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "66F3AB", DecoderOptions.NONE, Instruction.createStosw(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "67F3AB", DecoderOptions.NONE, Instruction.createStosd(16, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F3AB", DecoderOptions.NONE, Instruction.createStosd(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F3AB", DecoderOptions.NONE, Instruction.createStosd(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "67F348AB", DecoderOptions.NONE, Instruction.createStosq(32, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "F348AB", DecoderOptions.NONE, Instruction.createStosq(64, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6467F3A6", DecoderOptions.NONE, Instruction.createCmpsb(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F3A6", DecoderOptions.NONE, Instruction.createCmpsb(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F3A6", DecoderOptions.NONE, Instruction.createCmpsb(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "646667F3A7", DecoderOptions.NONE, Instruction.createCmpsw(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "646667F3A7", DecoderOptions.NONE, Instruction.createCmpsw(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6466F3A7", DecoderOptions.NONE, Instruction.createCmpsw(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6467F3A7", DecoderOptions.NONE, Instruction.createCmpsd(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F3A7", DecoderOptions.NONE, Instruction.createCmpsd(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F3A7", DecoderOptions.NONE, Instruction.createCmpsd(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F348A7", DecoderOptions.NONE, Instruction.createCmpsq(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F348A7", DecoderOptions.NONE, Instruction.createCmpsq(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6467F3A4", DecoderOptions.NONE, Instruction.createMovsb(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F3A4", DecoderOptions.NONE, Instruction.createMovsb(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F3A4", DecoderOptions.NONE, Instruction.createMovsb(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "646667F3A5", DecoderOptions.NONE, Instruction.createMovsw(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "646667F3A5", DecoderOptions.NONE, Instruction.createMovsw(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6466F3A5", DecoderOptions.NONE, Instruction.createMovsw(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(32, "6467F3A5", DecoderOptions.NONE, Instruction.createMovsd(16, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F3A5", DecoderOptions.NONE, Instruction.createMovsd(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F3A5", DecoderOptions.NONE, Instruction.createMovsd(64, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "6467F348A5", DecoderOptions.NONE, Instruction.createMovsq(32, ICRegisters.fs, RepPrefixKind.REPE)));
		result.add(Arguments.of(64, "64F348A5", DecoderOptions.NONE, Instruction.createMovsq(64, ICRegisters.fs, RepPrefixKind.REPE)));

		result.add(Arguments.of(32, "6467F26E", DecoderOptions.NONE, Instruction.createOutsb(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F26E", DecoderOptions.NONE, Instruction.createOutsb(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F26E", DecoderOptions.NONE, Instruction.createOutsb(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "646667F26F", DecoderOptions.NONE, Instruction.createOutsw(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "646667F26F", DecoderOptions.NONE, Instruction.createOutsw(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6466F26F", DecoderOptions.NONE, Instruction.createOutsw(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6467F26F", DecoderOptions.NONE, Instruction.createOutsd(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F26F", DecoderOptions.NONE, Instruction.createOutsd(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F26F", DecoderOptions.NONE, Instruction.createOutsd(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "67F2AE", DecoderOptions.NONE, Instruction.createScasb(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F2AE", DecoderOptions.NONE, Instruction.createScasb(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F2AE", DecoderOptions.NONE, Instruction.createScasb(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6667F2AF", DecoderOptions.NONE, Instruction.createScasw(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6667F2AF", DecoderOptions.NONE, Instruction.createScasw(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "66F2AF", DecoderOptions.NONE, Instruction.createScasw(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "67F2AF", DecoderOptions.NONE, Instruction.createScasd(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F2AF", DecoderOptions.NONE, Instruction.createScasd(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F2AF", DecoderOptions.NONE, Instruction.createScasd(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F248AF", DecoderOptions.NONE, Instruction.createScasq(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F248AF", DecoderOptions.NONE, Instruction.createScasq(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6467F2AC", DecoderOptions.NONE, Instruction.createLodsb(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F2AC", DecoderOptions.NONE, Instruction.createLodsb(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F2AC", DecoderOptions.NONE, Instruction.createLodsb(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "646667F2AD", DecoderOptions.NONE, Instruction.createLodsw(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "646667F2AD", DecoderOptions.NONE, Instruction.createLodsw(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6466F2AD", DecoderOptions.NONE, Instruction.createLodsw(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6467F2AD", DecoderOptions.NONE, Instruction.createLodsd(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F2AD", DecoderOptions.NONE, Instruction.createLodsd(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F2AD", DecoderOptions.NONE, Instruction.createLodsd(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F248AD", DecoderOptions.NONE, Instruction.createLodsq(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F248AD", DecoderOptions.NONE, Instruction.createLodsq(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "67F26C", DecoderOptions.NONE, Instruction.createInsb(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F26C", DecoderOptions.NONE, Instruction.createInsb(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F26C", DecoderOptions.NONE, Instruction.createInsb(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6667F26D", DecoderOptions.NONE, Instruction.createInsw(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6667F26D", DecoderOptions.NONE, Instruction.createInsw(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "66F26D", DecoderOptions.NONE, Instruction.createInsw(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "67F26D", DecoderOptions.NONE, Instruction.createInsd(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F26D", DecoderOptions.NONE, Instruction.createInsd(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F26D", DecoderOptions.NONE, Instruction.createInsd(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "67F2AA", DecoderOptions.NONE, Instruction.createStosb(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F2AA", DecoderOptions.NONE, Instruction.createStosb(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F2AA", DecoderOptions.NONE, Instruction.createStosb(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6667F2AB", DecoderOptions.NONE, Instruction.createStosw(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6667F2AB", DecoderOptions.NONE, Instruction.createStosw(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "66F2AB", DecoderOptions.NONE, Instruction.createStosw(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "67F2AB", DecoderOptions.NONE, Instruction.createStosd(16, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F2AB", DecoderOptions.NONE, Instruction.createStosd(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F2AB", DecoderOptions.NONE, Instruction.createStosd(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "67F248AB", DecoderOptions.NONE, Instruction.createStosq(32, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "F248AB", DecoderOptions.NONE, Instruction.createStosq(64, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6467F2A6", DecoderOptions.NONE, Instruction.createCmpsb(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F2A6", DecoderOptions.NONE, Instruction.createCmpsb(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F2A6", DecoderOptions.NONE, Instruction.createCmpsb(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "646667F2A7", DecoderOptions.NONE, Instruction.createCmpsw(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "646667F2A7", DecoderOptions.NONE, Instruction.createCmpsw(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6466F2A7", DecoderOptions.NONE, Instruction.createCmpsw(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6467F2A7", DecoderOptions.NONE, Instruction.createCmpsd(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F2A7", DecoderOptions.NONE, Instruction.createCmpsd(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F2A7", DecoderOptions.NONE, Instruction.createCmpsd(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F248A7", DecoderOptions.NONE, Instruction.createCmpsq(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F248A7", DecoderOptions.NONE, Instruction.createCmpsq(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6467F2A4", DecoderOptions.NONE, Instruction.createMovsb(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F2A4", DecoderOptions.NONE, Instruction.createMovsb(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F2A4", DecoderOptions.NONE, Instruction.createMovsb(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "646667F2A5", DecoderOptions.NONE, Instruction.createMovsw(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "646667F2A5", DecoderOptions.NONE, Instruction.createMovsw(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6466F2A5", DecoderOptions.NONE, Instruction.createMovsw(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(32, "6467F2A5", DecoderOptions.NONE, Instruction.createMovsd(16, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F2A5", DecoderOptions.NONE, Instruction.createMovsd(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F2A5", DecoderOptions.NONE, Instruction.createMovsd(64, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "6467F248A5", DecoderOptions.NONE, Instruction.createMovsq(32, ICRegisters.fs, RepPrefixKind.REPNE)));
		result.add(Arguments.of(64, "64F248A5", DecoderOptions.NONE, Instruction.createMovsq(64, ICRegisters.fs, RepPrefixKind.REPNE)));

		result.add(Arguments.of(32, "67F36E", DecoderOptions.NONE, Instruction.createRepOutsb(16)));
		result.add(Arguments.of(64, "67F36E", DecoderOptions.NONE, Instruction.createRepOutsb(32)));
		result.add(Arguments.of(64, "F36E", DecoderOptions.NONE, Instruction.createRepOutsb(64)));
		result.add(Arguments.of(32, "6667F36F", DecoderOptions.NONE, Instruction.createRepOutsw(16)));
		result.add(Arguments.of(64, "6667F36F", DecoderOptions.NONE, Instruction.createRepOutsw(32)));
		result.add(Arguments.of(64, "66F36F", DecoderOptions.NONE, Instruction.createRepOutsw(64)));
		result.add(Arguments.of(32, "67F36F", DecoderOptions.NONE, Instruction.createRepOutsd(16)));
		result.add(Arguments.of(64, "67F36F", DecoderOptions.NONE, Instruction.createRepOutsd(32)));
		result.add(Arguments.of(64, "F36F", DecoderOptions.NONE, Instruction.createRepOutsd(64)));
		result.add(Arguments.of(32, "67F3AE", DecoderOptions.NONE, Instruction.createRepeScasb(16)));
		result.add(Arguments.of(64, "67F3AE", DecoderOptions.NONE, Instruction.createRepeScasb(32)));
		result.add(Arguments.of(64, "F3AE", DecoderOptions.NONE, Instruction.createRepeScasb(64)));
		result.add(Arguments.of(32, "6667F3AF", DecoderOptions.NONE, Instruction.createRepeScasw(16)));
		result.add(Arguments.of(64, "6667F3AF", DecoderOptions.NONE, Instruction.createRepeScasw(32)));
		result.add(Arguments.of(64, "66F3AF", DecoderOptions.NONE, Instruction.createRepeScasw(64)));
		result.add(Arguments.of(32, "67F3AF", DecoderOptions.NONE, Instruction.createRepeScasd(16)));
		result.add(Arguments.of(64, "67F3AF", DecoderOptions.NONE, Instruction.createRepeScasd(32)));
		result.add(Arguments.of(64, "F3AF", DecoderOptions.NONE, Instruction.createRepeScasd(64)));
		result.add(Arguments.of(64, "67F348AF", DecoderOptions.NONE, Instruction.createRepeScasq(32)));
		result.add(Arguments.of(64, "F348AF", DecoderOptions.NONE, Instruction.createRepeScasq(64)));
		result.add(Arguments.of(32, "67F2AE", DecoderOptions.NONE, Instruction.createRepneScasb(16)));
		result.add(Arguments.of(64, "67F2AE", DecoderOptions.NONE, Instruction.createRepneScasb(32)));
		result.add(Arguments.of(64, "F2AE", DecoderOptions.NONE, Instruction.createRepneScasb(64)));
		result.add(Arguments.of(32, "6667F2AF", DecoderOptions.NONE, Instruction.createRepneScasw(16)));
		result.add(Arguments.of(64, "6667F2AF", DecoderOptions.NONE, Instruction.createRepneScasw(32)));
		result.add(Arguments.of(64, "66F2AF", DecoderOptions.NONE, Instruction.createRepneScasw(64)));
		result.add(Arguments.of(32, "67F2AF", DecoderOptions.NONE, Instruction.createRepneScasd(16)));
		result.add(Arguments.of(64, "67F2AF", DecoderOptions.NONE, Instruction.createRepneScasd(32)));
		result.add(Arguments.of(64, "F2AF", DecoderOptions.NONE, Instruction.createRepneScasd(64)));
		result.add(Arguments.of(64, "67F248AF", DecoderOptions.NONE, Instruction.createRepneScasq(32)));
		result.add(Arguments.of(64, "F248AF", DecoderOptions.NONE, Instruction.createRepneScasq(64)));
		result.add(Arguments.of(32, "67F3AC", DecoderOptions.NONE, Instruction.createRepLodsb(16)));
		result.add(Arguments.of(64, "67F3AC", DecoderOptions.NONE, Instruction.createRepLodsb(32)));
		result.add(Arguments.of(64, "F3AC", DecoderOptions.NONE, Instruction.createRepLodsb(64)));
		result.add(Arguments.of(32, "6667F3AD", DecoderOptions.NONE, Instruction.createRepLodsw(16)));
		result.add(Arguments.of(64, "6667F3AD", DecoderOptions.NONE, Instruction.createRepLodsw(32)));
		result.add(Arguments.of(64, "66F3AD", DecoderOptions.NONE, Instruction.createRepLodsw(64)));
		result.add(Arguments.of(32, "67F3AD", DecoderOptions.NONE, Instruction.createRepLodsd(16)));
		result.add(Arguments.of(64, "67F3AD", DecoderOptions.NONE, Instruction.createRepLodsd(32)));
		result.add(Arguments.of(64, "F3AD", DecoderOptions.NONE, Instruction.createRepLodsd(64)));
		result.add(Arguments.of(64, "67F348AD", DecoderOptions.NONE, Instruction.createRepLodsq(32)));
		result.add(Arguments.of(64, "F348AD", DecoderOptions.NONE, Instruction.createRepLodsq(64)));
		result.add(Arguments.of(32, "67F36C", DecoderOptions.NONE, Instruction.createRepInsb(16)));
		result.add(Arguments.of(64, "67F36C", DecoderOptions.NONE, Instruction.createRepInsb(32)));
		result.add(Arguments.of(64, "F36C", DecoderOptions.NONE, Instruction.createRepInsb(64)));
		result.add(Arguments.of(32, "6667F36D", DecoderOptions.NONE, Instruction.createRepInsw(16)));
		result.add(Arguments.of(64, "6667F36D", DecoderOptions.NONE, Instruction.createRepInsw(32)));
		result.add(Arguments.of(64, "66F36D", DecoderOptions.NONE, Instruction.createRepInsw(64)));
		result.add(Arguments.of(32, "67F36D", DecoderOptions.NONE, Instruction.createRepInsd(16)));
		result.add(Arguments.of(64, "67F36D", DecoderOptions.NONE, Instruction.createRepInsd(32)));
		result.add(Arguments.of(64, "F36D", DecoderOptions.NONE, Instruction.createRepInsd(64)));
		result.add(Arguments.of(32, "67F3AA", DecoderOptions.NONE, Instruction.createRepStosb(16)));
		result.add(Arguments.of(64, "67F3AA", DecoderOptions.NONE, Instruction.createRepStosb(32)));
		result.add(Arguments.of(64, "F3AA", DecoderOptions.NONE, Instruction.createRepStosb(64)));
		result.add(Arguments.of(32, "6667F3AB", DecoderOptions.NONE, Instruction.createRepStosw(16)));
		result.add(Arguments.of(64, "6667F3AB", DecoderOptions.NONE, Instruction.createRepStosw(32)));
		result.add(Arguments.of(64, "66F3AB", DecoderOptions.NONE, Instruction.createRepStosw(64)));
		result.add(Arguments.of(32, "67F3AB", DecoderOptions.NONE, Instruction.createRepStosd(16)));
		result.add(Arguments.of(64, "67F3AB", DecoderOptions.NONE, Instruction.createRepStosd(32)));
		result.add(Arguments.of(64, "F3AB", DecoderOptions.NONE, Instruction.createRepStosd(64)));
		result.add(Arguments.of(64, "67F348AB", DecoderOptions.NONE, Instruction.createRepStosq(32)));
		result.add(Arguments.of(64, "F348AB", DecoderOptions.NONE, Instruction.createRepStosq(64)));
		result.add(Arguments.of(32, "67F3A6", DecoderOptions.NONE, Instruction.createRepeCmpsb(16)));
		result.add(Arguments.of(64, "67F3A6", DecoderOptions.NONE, Instruction.createRepeCmpsb(32)));
		result.add(Arguments.of(64, "F3A6", DecoderOptions.NONE, Instruction.createRepeCmpsb(64)));
		result.add(Arguments.of(32, "6667F3A7", DecoderOptions.NONE, Instruction.createRepeCmpsw(16)));
		result.add(Arguments.of(64, "6667F3A7", DecoderOptions.NONE, Instruction.createRepeCmpsw(32)));
		result.add(Arguments.of(64, "66F3A7", DecoderOptions.NONE, Instruction.createRepeCmpsw(64)));
		result.add(Arguments.of(32, "67F3A7", DecoderOptions.NONE, Instruction.createRepeCmpsd(16)));
		result.add(Arguments.of(64, "67F3A7", DecoderOptions.NONE, Instruction.createRepeCmpsd(32)));
		result.add(Arguments.of(64, "F3A7", DecoderOptions.NONE, Instruction.createRepeCmpsd(64)));
		result.add(Arguments.of(64, "67F348A7", DecoderOptions.NONE, Instruction.createRepeCmpsq(32)));
		result.add(Arguments.of(64, "F348A7", DecoderOptions.NONE, Instruction.createRepeCmpsq(64)));
		result.add(Arguments.of(32, "67F2A6", DecoderOptions.NONE, Instruction.createRepneCmpsb(16)));
		result.add(Arguments.of(64, "67F2A6", DecoderOptions.NONE, Instruction.createRepneCmpsb(32)));
		result.add(Arguments.of(64, "F2A6", DecoderOptions.NONE, Instruction.createRepneCmpsb(64)));
		result.add(Arguments.of(32, "6667F2A7", DecoderOptions.NONE, Instruction.createRepneCmpsw(16)));
		result.add(Arguments.of(64, "6667F2A7", DecoderOptions.NONE, Instruction.createRepneCmpsw(32)));
		result.add(Arguments.of(64, "66F2A7", DecoderOptions.NONE, Instruction.createRepneCmpsw(64)));
		result.add(Arguments.of(32, "67F2A7", DecoderOptions.NONE, Instruction.createRepneCmpsd(16)));
		result.add(Arguments.of(64, "67F2A7", DecoderOptions.NONE, Instruction.createRepneCmpsd(32)));
		result.add(Arguments.of(64, "F2A7", DecoderOptions.NONE, Instruction.createRepneCmpsd(64)));
		result.add(Arguments.of(64, "67F248A7", DecoderOptions.NONE, Instruction.createRepneCmpsq(32)));
		result.add(Arguments.of(64, "F248A7", DecoderOptions.NONE, Instruction.createRepneCmpsq(64)));
		result.add(Arguments.of(32, "67F3A4", DecoderOptions.NONE, Instruction.createRepMovsb(16)));
		result.add(Arguments.of(64, "67F3A4", DecoderOptions.NONE, Instruction.createRepMovsb(32)));
		result.add(Arguments.of(64, "F3A4", DecoderOptions.NONE, Instruction.createRepMovsb(64)));
		result.add(Arguments.of(32, "6667F3A5", DecoderOptions.NONE, Instruction.createRepMovsw(16)));
		result.add(Arguments.of(64, "6667F3A5", DecoderOptions.NONE, Instruction.createRepMovsw(32)));
		result.add(Arguments.of(64, "66F3A5", DecoderOptions.NONE, Instruction.createRepMovsw(64)));
		result.add(Arguments.of(32, "67F3A5", DecoderOptions.NONE, Instruction.createRepMovsd(16)));
		result.add(Arguments.of(64, "67F3A5", DecoderOptions.NONE, Instruction.createRepMovsd(32)));
		result.add(Arguments.of(64, "F3A5", DecoderOptions.NONE, Instruction.createRepMovsd(64)));
		result.add(Arguments.of(64, "67F348A5", DecoderOptions.NONE, Instruction.createRepMovsq(32)));
		result.add(Arguments.of(64, "F348A5", DecoderOptions.NONE, Instruction.createRepMovsq(64)));
		result.add(Arguments.of(64, "C5E814CB", DecoderOptions.NONE, Instruction.create(Code.VEX_VUNPCKLPS_XMM_XMM_XMMM128, ICRegisters.xmm1, ICRegisters.xmm2, ICRegisters.xmm3)));
		result.add(Arguments.of(64, "64C5E8148C7501EFCDAB", DecoderOptions.NONE, Instruction.create(Code.VEX_VUNPCKLPS_XMM_XMM_XMMM128, ICRegisters.xmm1, ICRegisters.xmm2, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs))));
		result.add(Arguments.of(64, "64C4E261908C7501EFCDAB", DecoderOptions.NONE, Instruction.create(Code.VEX_VPGATHERDD_XMM_VM32X_XMM, ICRegisters.xmm1, new MemoryOperand(ICRegisters.rbp, ICRegisters.xmm6, 2, -0x543210FF, 8, false, ICRegisters.fs), ICRegisters.xmm3)));
		result.add(Arguments.of(64, "64C4E2692E9C7501EFCDAB", DecoderOptions.NONE, Instruction.create(Code.VEX_VMASKMOVPS_M128_XMM_XMM, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), ICRegisters.xmm2, ICRegisters.xmm3)));
		result.add(Arguments.of(64, "C4E3694ACB40", DecoderOptions.NONE, Instruction.create(Code.VEX_VBLENDVPS_XMM_XMM_XMMM128_XMM, ICRegisters.xmm1, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.xmm4)));
		result.add(Arguments.of(64, "64C4E3E95C8C7501EFCDAB30", DecoderOptions.NONE, Instruction.create(Code.VEX_VFMADDSUBPS_XMM_XMM_XMM_XMMM128, ICRegisters.xmm1, ICRegisters.xmm2, ICRegisters.xmm3, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs))));
		result.add(Arguments.of(64, "64C4E3694A8C7501EFCDAB40", DecoderOptions.NONE, Instruction.create(Code.VEX_VBLENDVPS_XMM_XMM_XMMM128_XMM, ICRegisters.xmm1, ICRegisters.xmm2, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), ICRegisters.xmm4)));
		result.add(Arguments.of(64, "C4E36948CB40", DecoderOptions.NONE, Instruction.create(Code.VEX_VPERMIL2PS_XMM_XMM_XMMM128_XMM_IMM4, ICRegisters.xmm1, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.xmm4, 0x0)));
		result.add(Arguments.of(64, "64C4E3E9488C7501EFCDAB31", DecoderOptions.NONE, Instruction.create(Code.VEX_VPERMIL2PS_XMM_XMM_XMM_XMMM128_IMM4, ICRegisters.xmm1, ICRegisters.xmm2, ICRegisters.xmm3, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0x1)));
		result.add(Arguments.of(64, "64C4E369488C7501EFCDAB41", DecoderOptions.NONE, Instruction.create(Code.VEX_VPERMIL2PS_XMM_XMM_XMMM128_XMM_IMM4, ICRegisters.xmm1, ICRegisters.xmm2, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), ICRegisters.xmm4, 0x1)));
		result.add(Arguments.of(32, "6467C5F9F7D3", DecoderOptions.NONE, Instruction.createVmaskmovdqu(16, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs)));
		result.add(Arguments.of(64, "6467C5F9F7D3", DecoderOptions.NONE, Instruction.createVmaskmovdqu(32, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs)));
		result.add(Arguments.of(64, "64C5F9F7D3", DecoderOptions.NONE, Instruction.createVmaskmovdqu(64, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs)));
		result.add(Arguments.of(64, "62F1F50873D2A5", DecoderOptions.NONE, Instruction.create(Code.EVEX_VPSRLQ_XMM_K1Z_XMMM128B64_IMM8, ICRegisters.xmm1, ICRegisters.xmm2, 0xA5)));
		result.add(Arguments.of(64, "6462F1F50873947501EFCDABA5", DecoderOptions.NONE, Instruction.create(Code.EVEX_VPSRLQ_XMM_K1Z_XMMM128B64_IMM8, ICRegisters.xmm1, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0xA5)));
		result.add(Arguments.of(64, "62F16D08C4CBA5", DecoderOptions.NONE, Instruction.create(Code.EVEX_VPINSRW_XMM_XMM_R32M16_IMM8, ICRegisters.xmm1, ICRegisters.xmm2, ICRegisters.ebx, 0xA5)));
		result.add(Arguments.of(64, "6462F16D08C48C7501EFCDABA5", DecoderOptions.NONE, Instruction.create(Code.EVEX_VPINSRW_XMM_XMM_R32M16_IMM8, ICRegisters.xmm1, ICRegisters.xmm2, new MemoryOperand(ICRegisters.rbp, ICRegisters.rsi, 2, -0x543210FF, 8, false, ICRegisters.fs), 0xA5)));
		return result;
	}

	@ParameterizedTest
	@MethodSource("createThrowsIfInvalidBitnessData")
	void createThrowsIfInvalidBitness(IntConsumer create) {
		for (int bitness : BitnessUtils.getInvalidBitnessValues())
			assertThrows(IllegalArgumentException.class, () -> create.accept(bitness));
	}

	static IntConsumer createIntConsumer(IntConsumer ic) {
		return ic;
	}

	public static Iterable<Arguments> createThrowsIfInvalidBitnessData() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createXbegin(bitness, 0x800000003412A550L))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createOutsb(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createOutsw(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createOutsd(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createScasb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createScasw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createScasd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createScasq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createLodsb(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createLodsw(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createLodsd(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createLodsq(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createInsb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createInsw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createInsd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createStosb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createStosw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createStosd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createStosq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createCmpsb(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createCmpsw(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createCmpsd(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createCmpsq(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createMovsb(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createMovsw(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createMovsd(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createMovsq(bitness, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createMaskmovq(bitness, ICRegisters.mm2, ICRegisters.mm3, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createMaskmovdqu(bitness, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createVmaskmovdqu(bitness, ICRegisters.xmm2, ICRegisters.xmm3, ICRegisters.fs))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepOutsb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepOutsw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepOutsd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeScasb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeScasw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeScasd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeScasq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneScasb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneScasw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneScasd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneScasq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepLodsb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepLodsw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepLodsd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepLodsq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepInsb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepInsw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepInsd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepStosb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepStosw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepStosd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepStosq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeCmpsb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeCmpsw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeCmpsd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepeCmpsq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneCmpsb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneCmpsw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneCmpsd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepneCmpsq(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepMovsb(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepMovsw(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepMovsd(bitness))));
		result.add(Arguments.of(createIntConsumer(bitness -> Instruction.createRepMovsq(bitness))));
		return result;
	}

	@Test
	void encoding_instruction_requiring_opmask_fails_if_no_opmask() {
		Instruction instruction = Instruction.create(Code.EVEX_VPGATHERDD_XMM_K1_VM32X, ICRegisters.xmm1, new MemoryOperand(ICRegisters.rdx, ICRegisters.xmm3));
		assertFalse(instruction.hasOpMask());
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, writer);
		Object result = encoder.tryEncode(instruction, 0);
		assertTrue(result instanceof String);
		assertEquals("The instruction must use an opmask register", (String)result);
	}

	@Test
	void createDeclareXXX_throws_if_null_array() {
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareByte(null));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareByte(null, 0, 0));

		assertThrows(NullPointerException.class, () -> Instruction.createDeclareWord((byte[])null));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareWord((byte[])null, 0, 0));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareWord((short[])null));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareWord((short[])null, 0, 0));

		assertThrows(NullPointerException.class, () -> Instruction.createDeclareDword((byte[])null));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareDword((byte[])null, 0, 0));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareDword((int[])null));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareDword((int[])null, 0, 0));

		assertThrows(NullPointerException.class, () -> Instruction.createDeclareQword((byte[])null));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareQword((byte[])null, 0, 0));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareQword((long[])null));
		assertThrows(NullPointerException.class, () -> Instruction.createDeclareQword((long[])null, 0, 0));
	}

	@Test
	void createDeclareXXX_throws_if_invalid_index_length() {
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[0]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[4], -1, 0));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[4], -1, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[4], -0x8000_0000, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[4], 0, 5));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[4], 0, 0x7FFF_FFFF));
		Instruction.createDeclareByte(new byte[16]);
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[17]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareByte(new byte[17], 0, 17));

		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[0]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[1]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[3]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[5]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[7]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[9]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[11]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[13]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[15]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[17]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[4], -1, 0));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[4], -1, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[4], -0x8000_0000, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[4], 0, 5));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[4], 0, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[64], 63, 2));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[64], 32, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[64]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[64], 0, 64));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[0]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[4], -1, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[4], -0x8000_0000, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[4], 0, 5));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[4], 0, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[4], 3, 2));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[9]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new short[9], 0, 9));
		Instruction.createDeclareWord(new short[8]);
		Instruction.createDeclareWord(new byte[16]);
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareWord(new byte[17], 0, 17));

		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[0]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[1]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[2]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[3]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[5]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[6]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[7]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[9]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[10]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[11]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[13]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[14]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[15]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[17]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[16], -1, 0));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[16], -1, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[16], -0x8000_0000, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[16], 1, 16));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[16], 0, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[64], 0, 9));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[64], 0, 10));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[64], 0, 11));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[64], 63, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[64], 62, 2));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[64], 61, 3));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[0]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[4], -1, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[4], -0x8000_0000, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[4], 0, 5));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[4], 0, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[4], 3, 2));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[5]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new int[5], 0, 5));
		Instruction.createDeclareDword(new int[4]);
		Instruction.createDeclareDword(new byte[16]);
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareDword(new byte[17], 0, 17));

		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[0]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[1]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[2]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[3]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[4]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[5]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[6]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[7]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[9]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[10]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[11]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[12]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[13]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[14]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[15]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[17]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[16], -1, 0));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[16], -1, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[16], -0x8000_0000, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[16], 1, 16));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[16], 0, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 0, 9));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 0, 10));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 0, 11));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 0, 12));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 0, 13));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 0, 14));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 0, 15));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 63, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 62, 2));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 61, 3));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 60, 4));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 59, 5));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 58, 6));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[64], 57, 7));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[0]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[2], -1, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[2], -0x8000_0000, 1));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[2], 0, 3));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[2], 0, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[2], 1, 2));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[3]));
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new long[3], 0, 3));
		Instruction.createDeclareQword(new long[2]);
		Instruction.createDeclareQword(new byte[16]);
		assertThrows(IllegalArgumentException.class, () -> Instruction.createDeclareQword(new byte[17], 0, 17));
	}

	@Test
	void equals_returns_false_if_null_input() {
		assertFalse(new Instruction().equals(null));
		assertFalse(Instruction.create(Code.NOPD).equals(null));
	}

	@Test
	void code_prop_throws_if_invalid() {
		Instruction instruction = new Instruction();
		assertThrows(IllegalArgumentException.class, () -> instruction.setCode(-1));
		assertThrows(IllegalArgumentException.class, () -> instruction.setCode(IcedConstants.CODE_ENUM_COUNT));
	}

	@Test
	void getSetOpKind_throws_if_invalid_input() {
		Instruction instruction = Instruction.create(Code.ADC_EAX_IMM32, ICRegisters.eax, 0xFFFF_FFFF);

		assertThrows(IllegalArgumentException.class, () -> instruction.getOpKind(-1));
		instruction.getOpKind(0);
		instruction.getOpKind(1);
		for (int i = 2; i < IcedConstants.MAX_OP_COUNT; i++)
			instruction.getOpKind(i);
		assertThrows(IllegalArgumentException.class, () -> instruction.getOpKind(IcedConstants.MAX_OP_COUNT));

		assertThrows(IllegalArgumentException.class, () -> instruction.setOpKind(-1, OpKind.REGISTER));
		instruction.setOpKind(0, OpKind.REGISTER);
		instruction.setOpKind(1, OpKind.IMMEDIATE32);
		for (int i = 2; i < IcedConstants.MAX_OP_COUNT; i++)
			instruction.setOpKind(i, OpKind.IMMEDIATE8);
		assertThrows(IllegalArgumentException.class, () -> instruction.setOpKind(IcedConstants.MAX_OP_COUNT, OpKind.REGISTER));
	}

	@Test
	void getSetImmediate_throws_if_invalid_input() {
		Instruction instruction = Instruction.create(Code.ADC_EAX_IMM32, ICRegisters.eax, 0xFFFF_FFFF);

		assertThrows(IllegalArgumentException.class, () -> instruction.getImmediate(-1));
		assertThrows(IllegalArgumentException.class, () -> instruction.getImmediate(0));
		instruction.getImmediate(1);
		for (int i = 2; i < IcedConstants.MAX_OP_COUNT; i++) {
			if (i == 4 && instruction.getOp4Kind() == OpKind.IMMEDIATE8)
				continue;
			final int fi = i;
			assertThrows(IllegalArgumentException.class, () -> instruction.getImmediate(fi));
		}
		assertThrows(IllegalArgumentException.class, () -> instruction.getImmediate(IcedConstants.MAX_OP_COUNT));

		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(-1, 0));
		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(-1, 0L));

		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(0, 0));
		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(0, 0L));

		instruction.setImmediate(1, 0);
		instruction.setImmediate(1, 0L);

		for (int i = 2; i < IcedConstants.MAX_OP_COUNT; i++) {
			if (i == 4 && instruction.getOp4Kind() == OpKind.IMMEDIATE8)
				continue;
			final int fi = i;
			assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(fi, 0));
			assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(fi, 0L));
		}
		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(IcedConstants.MAX_OP_COUNT, 0));
		assertThrows(IllegalArgumentException.class, () -> instruction.setImmediate(IcedConstants.MAX_OP_COUNT, 0L));
	}

	@Test
	void getSetRegister_throws_if_invalid_input() {
		Instruction instruction = Instruction.create(Code.ADC_EAX_IMM32, ICRegisters.eax, 0xFFFF_FFFF);

		assertThrows(IllegalArgumentException.class, () -> instruction.getOpRegister(-1));
		for (int i = 0; i < IcedConstants.MAX_OP_COUNT; i++)
			instruction.getOpRegister(i);
		assertThrows(IllegalArgumentException.class, () -> instruction.getOpRegister(IcedConstants.MAX_OP_COUNT));

		assertThrows(IllegalArgumentException.class, () -> instruction.setOpRegister(-1, Register.EAX));
		for (int i = 0; i < IcedConstants.MAX_OP_COUNT; i++) {
			if (i == 4 && instruction.getOp4Kind() == OpKind.IMMEDIATE8)
				continue;
			instruction.setOpRegister(i, Register.EAX);
		}
		assertThrows(IllegalArgumentException.class, () -> instruction.setOpRegister(IcedConstants.MAX_OP_COUNT, Register.EAX));
	}

	@Test
	void setDeclareXXXValue_throws_if_invalid_input() {
		{
			Instruction instruction = Instruction.createDeclareByte(new byte[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareByteValue(-1, (byte)0));
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareByteValue(16, (byte)0));
			for (int i = 0; i < 16; i++)
				instruction.setDeclareByteValue(i, (byte)0);
		}
		{
			Instruction instruction = Instruction.createDeclareWord(new short[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareWordValue(-1, (short)0));
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareWordValue(8, (short)0));
			for (int i = 0; i < 8; i++)
				instruction.setDeclareWordValue(i, (short)0);
		}
		{
			Instruction instruction = Instruction.createDeclareDword(new int[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareDwordValue(-1, 0));
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareDwordValue(4, 0));
			for (int i = 0; i < 4; i++)
				instruction.setDeclareDwordValue(i, 0);
		}
		{
			Instruction instruction = Instruction.createDeclareQword(new long[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareQwordValue(-1, 0L));
			assertThrows(IllegalArgumentException.class, () -> instruction.setDeclareQwordValue(2, 0L));
			for (int i = 0; i < 2; i++)
				instruction.setDeclareQwordValue(i, (long)0);
		}
	}

	@Test
	void getDeclareXXXValue_throws_if_invalid_input() {
		{
			Instruction instruction = Instruction.createDeclareByte(new byte[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareByteValue(-1));
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareByteValue(16));
			for (int i = 0; i < 16; i++)
				instruction.getDeclareByteValue(i);
		}
		{
			Instruction instruction = Instruction.createDeclareWord(new short[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareWordValue(-1));
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareWordValue(8));
			for (int i = 0; i < 8; i++)
				instruction.getDeclareWordValue(i);
		}
		{
			Instruction instruction = Instruction.createDeclareDword(new int[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareDwordValue(-1));
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareDwordValue(4));
			for (int i = 0; i < 4; i++)
				instruction.getDeclareDwordValue(i);
		}
		{
			Instruction instruction = Instruction.createDeclareQword(new long[1]);
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareQwordValue(-1));
			assertThrows(IllegalArgumentException.class, () -> instruction.getDeclareQwordValue(2));
			for (int i = 0; i < 2; i++)
				instruction.getDeclareQwordValue(i);
		}
	}

	@Test
	void getVirtualAddress_throws_if_null_input() {
		Instruction instruction = Instruction.create(Code.LEA_R64_M, ICRegisters.rax, new MemoryOperand(ICRegisters.rcx, ICRegisters.rdi, 8));
		assertThrows(NullPointerException.class, () -> instruction.getVirtualAddress(1, 0, (VAGetRegisterValue)null));
	}

	@Test
	void create_imm_works() {
		// OpKind.IMMEDIATE8
		for (int imm : new int[] { -0x80, 0xFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm);
			assertEquals((byte)imm, instruction.getImmediate8());
		}
		for (int imm : new int[] { -0x81, 0x100 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm));
		for (long imm : new long[] { -0x80, 0xFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm);
			assertEquals((byte)imm, instruction.getImmediate8());
		}
		for (long imm : new long[] { -0x81, 0x100 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm));
		for (int imm : new int[] { 0, 0xFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm);
			assertEquals(imm, instruction.getImmediate8() & 0xFF);
		}
		for (int imm : new int[] { 0x100, 0x7FFF_FFFF })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm));
		for (long imm : new long[] { 0, 0xFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm);
			assertEquals(imm, instruction.getImmediate8() & 0xFF);
		}
		for (long imm : new long[] { 0x100, 0xFFFF_FFFFL, 0x7FFF_FFFF_FFFF_FFFFL })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.cl, imm));

		// OpKind.IMMEDIATE8_2nd
		for (int imm : new int[] { -0x80, 0xFF }) {
			Instruction instruction = Instruction.create(Code.ENTERQ_IMM16_IMM8, 0, imm);
			assertEquals((byte)imm, instruction.getImmediate8_2nd());
		}
		for (int imm : new int[] { -0x81, 0x100 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ENTERQ_IMM16_IMM8, 0, imm));
		for (int imm : new int[] { 0, 0xFF }) {
			Instruction instruction = Instruction.create(Code.ENTERQ_IMM16_IMM8, 0, imm);
			assertEquals(imm, instruction.getImmediate8_2nd() & 0xFF);
		}
		for (int imm : new int[] { 0x100, 0x7FFF_FFFF })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ENTERQ_IMM16_IMM8, 0, imm));

		// OpKind.IMMEDIATE8TO16
		for (int imm : new int[] { -0x80, 0x7F }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm);
			assertEquals(imm, instruction.getImmediate8to16());
		}
		for (int imm : new int[] { -0x81, 0x80 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm));
		for (long imm : new long[] { -0x80, 0x7F }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm);
			assertEquals(imm, instruction.getImmediate8to16());
		}
		for (long imm : new long[] { -0x81, 0x80 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm));
		for (int imm : new int[] { 0, 0x7F, 0xFF80, 0xFFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm);
			assertEquals(imm, instruction.getImmediate8to16() & 0xFFFF);
		}
		for (int imm : new int[] { 0x80, 0xFF7F, 0x0001_0000, 0x7FFF_FFFF, 0x0001_FF80, 0x0001_FFFF })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm));
		for (long imm : new long[] { 0, 0x7F, 0xFF80, 0xFFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm);
			assertEquals(imm, instruction.getImmediate8to16() & 0xFFFF);
		}
		for (long imm : new long[] { 0x80, 0xFF7F, 0x0001_0000, 0xFFFF_FFFFL, 0x7FFF_FFFF_FFFF_FFFFL, 0x0001_FF80, 0x0001_FFFF })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM8, ICRegisters.cx, imm));

		// OpKind.IMMEDIATE8TO32
		for (int imm : new int[] { -0x80, 0x7F }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm);
			assertEquals(imm, instruction.getImmediate8to32());
		}
		for (int imm : new int[] { -0x81, 0x80 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm));
		for (long imm : new long[] { -0x80, 0x7F }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm);
			assertEquals(imm, instruction.getImmediate8to32());
		}
		for (long imm : new long[] { -0x81, 0x80 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm));
		for (int imm : new int[] { 0, 0x7F, 0xFFFF_FF80, 0xFFFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm);
			assertEquals(imm, instruction.getImmediate8to32());
		}
		for (int imm : new int[] { 0x80, 0xFFFF_FF7F })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm));
		for (long imm : new long[] { 0, 0x7F, 0xFFFF_FF80L, 0xFFFF_FFFFL }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm);
			assertEquals(imm, (long)instruction.getImmediate8to32() & 0xFFFF_FFFFL);
		}
		for (long imm : new long[] { 0x80, 0xFFFF_FF7FL, 0x0001_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL, 0x0001_FFFF_FF80L, 0x0001_FFFF_FFFFL })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM32_IMM8, ICRegisters.ecx, imm));

		// OpKind.IMMEDIATE8TO64
		for (int imm : new int[] { -0x80, 0x7F }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate8to64());
		}
		for (int imm : new int[] { -0x81, 0x80 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm));
		for (long imm : new long[] { -0x80, 0x7F }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate8to64());
		}
		for (long imm : new long[] { -0x81, 0x80 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm));
		for (int imm : new int[] { 0, 0x7F }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate8to64());
		}
		for (int imm : new int[] { 0x80, 0x7FFF_FFFF })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm));
		for (long imm : new long[] { 0, 0x7F, 0xFFFF_FFFF_FFFF_FF80L, 0xFFFF_FFFF_FFFF_FFFFL }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate8to64());
		}
		for (long imm : new long[] { 0x80, 0xFFFF_FFFF_FFFF_FF7FL })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM64_IMM8, ICRegisters.rcx, imm));

		// OpKind.IMMEDIATE32TO64
		for (int imm : new int[] { -0x8000_0000, 0x7FFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM32, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate32to64());
		}
		for (long imm : new long[] { -0x8000_0000, 0x7FFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM32, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate32to64());
		}
		for (long imm : new long[] { -0x8000_0001L, 0x8000_0000L })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM64_IMM32, ICRegisters.rcx, imm));
		for (int imm : new int[] { 0, 0x7FFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM32, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate32to64());
		}
		for (long imm : new long[] { 0, 0x7FFF_FFFF, 0xFFFF_FFFF_8000_0000L, 0xFFFF_FFFF_FFFF_FFFFL }) {
			Instruction instruction = Instruction.create(Code.ADD_RM64_IMM32, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate32to64());
		}
		for (long imm : new long[] { 0x8000_0000L, 0x0001_0000_0000L, 0xFFFF_FFFF_7FFF_FFFFL })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM64_IMM32, ICRegisters.rcx, imm));

		// OpKind.IMMEDIATE16
		for (int imm : new int[] { -0x8000, 0xFFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm);
			assertEquals(imm & 0xFFFF, instruction.getImmediate16() & 0xFFFF);
		}
		for (int imm : new int[] { -0x8001, 0x0001_0000 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm));
		for (long imm : new long[] { -0x8000, 0xFFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm);
			assertEquals(imm & 0xFFFF, instruction.getImmediate16() & 0xFFFF);
		}
		for (long imm : new long[] { -0x8001, 0x0001_0000 })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm));
		for (int imm : new int[] { 0, 0xFFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm);
			assertEquals(imm, instruction.getImmediate16() & 0xFFFF);
		}
		for (int imm : new int[] { 0x0001_0000, 0x7FFF_FFFF })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm));
		for (long imm : new long[] { 0, 0xFFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm);
			assertEquals(imm, instruction.getImmediate16() & 0xFFFF);
		}
		for (long imm : new long[] { 0x0001_0000, 0xFFFF_FFFFL, 0x7FFF_FFFF_FFFF_FFFFL })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM16_IMM16, ICRegisters.cx, imm));

		// OpKind.IMMEDIATE32
		for (int imm : new int[] { -0x8000_0000, 0x7FFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM32, ICRegisters.ecx, imm);
			assertEquals(imm, instruction.getImmediate32());
		}
		for (long imm : new long[] { -0x8000_0000, 0xFFFF_FFFFL }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM32, ICRegisters.ecx, imm);
			assertEquals((int)imm, instruction.getImmediate32());
		}
		for (long imm : new long[] { -0x8000_0001L, 0x0001_0000_0000L })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM32_IMM32, ICRegisters.ecx, imm));
		for (int imm : new int[] { 0, 0xFFFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM32, ICRegisters.ecx, imm);
			assertEquals(imm, instruction.getImmediate32());
		}
		for (long imm : new long[] { 0, 0xFFFF_FFFFL }) {
			Instruction instruction = Instruction.create(Code.ADD_RM32_IMM32, ICRegisters.ecx, imm);
			assertEquals(imm, (long)instruction.getImmediate32() & 0xFFFF_FFFFL);
		}
		for (long imm : new long[] { 0x0001_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL })
			assertThrows(IllegalArgumentException.class, () -> Instruction.create(Code.ADD_RM32_IMM32, ICRegisters.ecx, imm));

		// OpKind.IMMEDIATE64
		for (int imm : new int[] { -0x8000_0000, 0x7FFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate64());
		}
		for (long imm : new long[] { -0x8000_0000_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL }) {
			Instruction instruction = Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate64());
		}
		for (int imm : new int[] { 0, 0xFFFF_FFFF }) {
			Instruction instruction = Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate64());
		}
		for (long imm : new long[] { 0, 0xFFFF_FFFF_FFFF_FFFFL }) {
			Instruction instruction = Instruction.create(Code.MOV_R64_IMM64, ICRegisters.rcx, imm);
			assertEquals(imm, instruction.getImmediate64());
		}
	}

	@Test
	void encodeInvalidLenDwDdDq() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Object result;

		Instruction dw = Instruction.createDeclareWord(1);
		dw.setDeclareDataCount(8);
		result = encoder.tryEncode(dw, 0);
		assertTrue(result instanceof Integer);
		assertEquals(16, (Integer)result);
		dw.setDeclareDataCount(8 + 1);
		assertTrue(encoder.tryEncode(dw, 0) instanceof String);

		Instruction dd = Instruction.createDeclareDword(1);
		dd.setDeclareDataCount(4);
		result = encoder.tryEncode(dd, 0);
		assertTrue(result instanceof Integer);
		assertEquals(16, (Integer)result);
		dd.setDeclareDataCount(4 + 1);
		assertTrue(encoder.tryEncode(dd, 0) instanceof String);

		Instruction dq = Instruction.createDeclareQword(1);
		dq.setDeclareDataCount(2);
		result = encoder.tryEncode(dq, 0);
		assertTrue(result instanceof Integer);
		assertEquals(16, (Integer)result);
		dq.setDeclareDataCount(2 + 1);
		assertTrue(encoder.tryEncode(dq, 0) instanceof String);
	}
}
