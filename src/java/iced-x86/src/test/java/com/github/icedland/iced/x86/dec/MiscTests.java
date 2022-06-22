// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.BitnessUtils;
import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;

final class MiscTests {
	static final class DecodeMultipleCodeReader extends CodeReader {
		byte[] data;
		int offset;

		public void setArray(byte[] data) {
			this.data = data;
			offset = 0;
		}

		@Override
		public int readByte() {
			if (Integer.compareUnsigned(offset, data.length) >= 0)
				return -1;
			return data[offset++] & 0xFF;
		}
	}

	@Test
	void decode_multiple_instrs_with_one_instance() {
		DecodeMultipleCodeReader reader16 = new DecodeMultipleCodeReader();
		DecodeMultipleCodeReader reader32 = new DecodeMultipleCodeReader();
		DecodeMultipleCodeReader reader64 = new DecodeMultipleCodeReader();
		HashMap<Integer, Decoder> decoderDict16 = new HashMap<Integer, Decoder>();
		HashMap<Integer, Decoder> decoderDict32 = new HashMap<Integer, Decoder>();
		HashMap<Integer, Decoder> decoderDict64 = new HashMap<Integer, Decoder>();
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(true, true)) {
			byte[] data = HexUtils.toByteArray(info.hexBytes);
			Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(data), info.options);
			decoder.setIP(info.ip);
			Decoder decoderAll;
			switch (info.bitness) {
			case 16:
				reader16.setArray(data);
				decoderAll = decoderDict16.get(info.options);
				if (decoderAll == null)
					decoderDict16.put(info.options, decoderAll = new Decoder(info.bitness, reader16, info.options));
				break;
			case 32:
				reader32.setArray(data);
				decoderAll = decoderDict32.get(info.options);
				if (decoderAll == null)
					decoderDict32.put(info.options, decoderAll = new Decoder(info.bitness, reader32, info.options));
				break;
			case 64:
				reader64.setArray(data);
				decoderAll = decoderDict64.get(info.options);
				if (decoderAll == null)
					decoderDict64.put(info.options, decoderAll = new Decoder(info.bitness, reader64, info.options));
				break;
			default:
				throw new UnsupportedOperationException();
			}
			decoderAll.setIP(decoder.getIP());
			Instruction instruction1 = decoder.decode();
			Instruction instruction2 = decoderAll.decode();
			ConstantOffsets co1 = decoder.getConstantOffsets(instruction1);
			ConstantOffsets co2 = decoderAll.getConstantOffsets(instruction2);
			assertEquals(info.code, instruction1.getCode());
			assertTrue(instruction1.equalsAllBits(instruction2));
			DecoderTests.verifyConstantOffsets(co1, co2);
		}
	}

	@ParameterizedTest
	@MethodSource("test_ByteArrayCodeReader_ctor_data")
	void test_ByteArrayCodeReader_ctor(ByteArrayCodeReader reader, byte[] expectedData) {
		int i = 0;
		assertEquals(0, reader.getPosition());
		while (reader.canReadByte()) {
			assertEquals(i, reader.getPosition());
			assertTrue(i < expectedData.length);
			assertEquals(expectedData[i] & 0xFF, reader.readByte());
			i++;
		}
		assertEquals(i, reader.getPosition());
		assertEquals(expectedData.length, i);
		assertEquals(-1, reader.readByte());
		assertEquals(i, reader.getPosition());

		reader.setPosition(0);
		assertEquals(0, reader.getPosition());
		i = 0;
		while (reader.canReadByte()) {
			assertEquals(i, reader.getPosition());
			assertTrue(i < expectedData.length);
			assertEquals(expectedData[i] & 0xFF, reader.readByte());
			i++;
		}
		assertEquals(i, reader.getPosition());
		assertEquals(expectedData.length, i);
		assertEquals(-1, reader.readByte());
		assertEquals(i, reader.getPosition());

		reader.setPosition(reader.size());
		assertEquals(reader.size(), reader.getPosition());
		assertFalse(reader.canReadByte());
		assertEquals(-1, reader.readByte());

		for (i = expectedData.length - 1; i >= 0; i--) {
			reader.setPosition(i);
			assertEquals(i, reader.getPosition());
			assertTrue(reader.canReadByte());
			assertEquals(expectedData[i] & 0xFF, reader.readByte());
			assertEquals(i + 1, reader.getPosition());
		}

		assertThrows(IllegalArgumentException.class, () -> reader.setPosition(-0x8000_0000));
		assertThrows(IllegalArgumentException.class, () -> reader.setPosition(-1));
		assertThrows(IllegalArgumentException.class, () -> reader.setPosition(expectedData.length + 1));
		assertThrows(IllegalArgumentException.class, () -> reader.setPosition(0x7FFF_FFFF));
	}

	static Iterator<Arguments> test_ByteArrayCodeReader_ctor_data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();

		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] {}), new byte[] {}));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x23 }), new byte[] { 0x23 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x23, 0x45 }), new byte[] { 0x23, 0x45 }));

		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] {}, 0, 0), new byte[] {}));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45 }, 0, 1), new byte[] { 0x45 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67 }, 0, 2), new byte[] { 0x45, 0x67 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89 }, 0, 3), new byte[] { 0x45, 0x67, (byte)0x89 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 0, 4),
				new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 1, 3),
				new byte[] { 0x67, (byte)0x89, (byte)0xAB }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 2, 2),
				new byte[] { (byte)0x89, (byte)0xAB }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 3, 1), new byte[] { (byte)0xAB }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 4, 0), new byte[] {}));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 0, 3),
				new byte[] { 0x45, 0x67, (byte)0x89 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 0, 2), new byte[] { 0x45, 0x67 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 0, 1), new byte[] { 0x45 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 0, 0), new byte[] {}));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 1, 2), new byte[] { 0x67, (byte)0x89 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 1, 1), new byte[] { 0x67 }));
		result.add(Arguments.of(new ByteArrayCodeReader(new byte[] { 0x45, 0x67, (byte)0x89, (byte)0xAB }, 2, 1), new byte[] { (byte)0x89 }));

		return result.iterator();
	}

	@Test
	void test_bytearraycodereader_ctor_throws() {
		assertThrows(NullPointerException.class, () -> new ByteArrayCodeReader((byte[])null));
		assertThrows(NullPointerException.class, () -> new ByteArrayCodeReader((byte[])null, 0, 0));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, -1, 0));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 0, -1));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, -0x8000_0000, 0));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 0, 5));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 0, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, -0x8000_0000, 0x7FFF_FFFF));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 4, 1));
		assertThrows(IllegalArgumentException.class, () -> new ByteArrayCodeReader(new byte[] { 1, 2, 3, 4 }, 4, 0x7FFF_FFFF));
	}

	@Test
	void test_decoder_create_throws() {
		for (Integer bitness : BitnessUtils.getInvalidBitnessValues()) {
			assertThrows(IllegalArgumentException.class,
					() -> new Decoder(bitness, new ByteArrayCodeReader(new byte[] { (byte)0x90 }), 0, DecoderOptions.NONE));
			assertThrows(IllegalArgumentException.class, () -> new Decoder(bitness, new byte[] { (byte)0x90 }, 0, DecoderOptions.NONE));
			assertThrows(IllegalArgumentException.class,
					() -> new Decoder(bitness, new ByteArrayCodeReader(new byte[] { (byte)0x90 }), DecoderOptions.NONE));
			assertThrows(IllegalArgumentException.class, () -> new Decoder(bitness, new byte[] { (byte)0x90 }, DecoderOptions.NONE));

			assertThrows(IllegalArgumentException.class, () -> new Decoder(bitness, new ByteArrayCodeReader(new byte[] { (byte)0x90 }), 0));
			assertThrows(IllegalArgumentException.class, () -> new Decoder(bitness, new byte[] { (byte)0x90 }, 0));
			assertThrows(IllegalArgumentException.class, () -> new Decoder(bitness, new ByteArrayCodeReader(new byte[] { (byte)0x90 })));
			assertThrows(IllegalArgumentException.class, () -> new Decoder(bitness, new byte[] { (byte)0x90 }));
		}

		for (int bitness : new int[] { 16, 32, 64 }) {
			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (CodeReader)null, 0));
			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (byte[])null, 0));
			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (CodeReader)null));
			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (byte[])null));

			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (CodeReader)null, 0, DecoderOptions.NONE));
			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (byte[])null, 0, DecoderOptions.NONE));
			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (CodeReader)null, DecoderOptions.NONE));
			assertThrows(NullPointerException.class, () -> new Decoder(bitness, (byte[])null, DecoderOptions.NONE));
		}
	}

	@Test
	void instruction_operator_eq_neq() {
		Instruction instr1a = Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, ICRegisters.rcx);
		Instruction instr1b = instr1a.copy();
		assertFalse(instr1a == instr1b);
		Instruction instruction2 = Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, ICRegisters.rdx);
		assertTrue(instr1a.equals(instr1b));
		assertFalse(instr1a.equals(instruction2));
		assertTrue(!instr1a.equals(instruction2));
		assertFalse(!instr1a.equals(instr1b));
	}

	@Test
	void decode_with_too_few_bytes_left() {
		for (DecoderTestInfo tc : DecoderTestUtils.getDecoderTests(true, false)) {
			byte[] bytes = HexUtils.toByteArray(tc.hexBytes);
			for (int i = 0; i + 1 < bytes.length; i++) {
				Decoder decoder = new Decoder(tc.bitness, new ByteArrayCodeReader(bytes, 0, i), tc.options);
				decoder.setIP(0x1000);
				Instruction instr = new Instruction();
				decoder.decode(instr);
				assertEquals(0x1000L + (long)i, decoder.getIP());
				assertEquals(Code.INVALID, instr.getCode());
				assertEquals(DecoderError.NO_MORE_BYTES, decoder.getLastError());
			}
		}
	}

	@Test
	void decode_ctor_with_byte_array_arg() {
		Decoder decoder = new Decoder(16, new byte[] { 0x01, (byte)0xCE }, DecoderOptions.NONE);
		assertEquals(16, decoder.getBitness());
		Instruction instr = new Instruction();
		decoder.decode(instr);
		assertEquals(Code.ADD_RM16_R16, instr.getCode());

		decoder = new Decoder(32, new byte[] { 0x01, (byte)0xCE }, DecoderOptions.NONE);
		assertEquals(32, decoder.getBitness());
		decoder.decode(instr);
		assertEquals(Code.ADD_RM32_R32, instr.getCode());

		decoder = new Decoder(64, new byte[] { 0x48, 0x01, (byte)0xCE }, DecoderOptions.NONE);
		assertEquals(64, decoder.getBitness());
		decoder.decode(instr);
		assertEquals(Code.ADD_RM64_R64, instr.getCode());
	}

	private ArrayList<Instruction> enumeratorDecode(Decoder decoder) {
		ArrayList<Instruction> list = new ArrayList<Instruction>();
		for (Instruction instr : decoder)
			list.add(instr);
		return list;
	}

	@Test
	void decode_enumerator_empty() {
		byte[] data = new byte[0];
		Decoder decoder = new Decoder(64, data);
		ArrayList<Instruction> list = enumeratorDecode(decoder);
		assertEquals(0, list.size());
	}

	@Test
	void decode_enumerator_one() {
		byte[] data = new byte[] { 0x00, (byte)0xCE };
		Decoder decoder = new Decoder(64, data);
		ArrayList<Instruction> list = enumeratorDecode(decoder);
		assertEquals(1, list.size());
		assertEquals(Code.ADD_RM8_R8, list.get(0).getCode());
	}

	@Test
	void decode_enumerator_two() {
		byte[] data = new byte[] { 0x00, (byte)0xCE, 0x66, 0x09, (byte)0xCE };
		Decoder decoder = new Decoder(64, data);
		ArrayList<Instruction> list = enumeratorDecode(decoder);
		assertEquals(2, list.size());
		assertEquals(Code.ADD_RM8_R8, list.get(0).getCode());
		assertEquals(Code.OR_RM16_R16, list.get(1).getCode());
	}

	@Test
	void decode_enumerator_incomplete_instruction_one() {
		byte[] data = new byte[] { 0x66 };
		Decoder decoder = new Decoder(64, data);
		ArrayList<Instruction> list = enumeratorDecode(decoder);
		assertEquals(1, list.size());
		assertEquals(Code.INVALID, list.get(0).getCode());
	}

	@Test
	void decode_enumerator_incomplete_instruction_two() {
		byte[] data = new byte[] { 0x00, (byte)0xCE, 0x66, 0x09 };
		Decoder decoder = new Decoder(64, data);
		ArrayList<Instruction> list = enumeratorDecode(decoder);
		assertEquals(2, list.size());
		assertEquals(Code.ADD_RM8_R8, list.get(0).getCode());
		assertEquals(Code.INVALID, list.get(1).getCode());
	}

	@Test
	void decoder_without_ip() {
		{
			Decoder decoder = new Decoder(64, new ByteArrayCodeReader(new byte[] {}), DecoderOptions.NONE);
			assertEquals(0L, decoder.getIP());
		}
		{
			Decoder decoder = new Decoder(64, new byte[] {}, DecoderOptions.NONE);
			assertEquals(0L, decoder.getIP());
		}
	}

	@Test
	void decoder_with_ip() {
		{
			Decoder decoder = new Decoder(64, new ByteArrayCodeReader(new byte[] {}), 0x123456789ABCDEF1L, DecoderOptions.NONE);
			assertEquals(0x123456789ABCDEF1L, decoder.getIP());
		}
		{
			Decoder decoder = new Decoder(64, new byte[] {}, 0x123456789ABCDEF1L, DecoderOptions.NONE);
			assertEquals(0x123456789ABCDEF1L, decoder.getIP());
		}
	}
}
