// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.BitnessUtils;
import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.ICRegister;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemoryOperand;
import com.github.icedland.iced.x86.dec.DecoderOptions;

final class BlockEncoderMiscTests {
	@Test
	void encode_zero_blocks() {
		Object result;
		BlockEncoderResult[] array;

		result = BlockEncoder.tryEncode(16, new InstructionBlock[0], BlockEncoderOptions.NONE);
		assertTrue(result instanceof BlockEncoderResult[]);
		array = (BlockEncoderResult[])result;
		assertEquals(0, array.length);

		result = BlockEncoder.tryEncode(32, new InstructionBlock[0], BlockEncoderOptions.NONE);
		assertTrue(result instanceof BlockEncoderResult[]);
		array = (BlockEncoderResult[])result;
		assertEquals(0, array.length);

		result = BlockEncoder.tryEncode(64, new InstructionBlock[0], BlockEncoderOptions.NONE);
		assertTrue(result instanceof BlockEncoderResult[]);
		array = (BlockEncoderResult[])result;
		assertEquals(0, array.length);
	}

	@Test
	void encode_zero_instructions() {
		Object result;
		BlockEncoderResult blockResult;
		CodeWriterImpl codeWriter = new CodeWriterImpl();

		result = BlockEncoder.tryEncode(16, new InstructionBlock(codeWriter, new ArrayList<Instruction>(), 0), BlockEncoderOptions.NONE);
		assertTrue(result instanceof BlockEncoderResult);
		blockResult = (BlockEncoderResult)result;
		assertEquals(0, codeWriter.toArray().length);
		assertEquals(0L, blockResult.rip);
		assertNull(blockResult.relocInfos);
		assertNotNull(blockResult.newInstructionOffsets);
		assertTrue(blockResult.newInstructionOffsets.length == 0);
		assertNotNull(blockResult.constantOffsets);
		assertTrue(blockResult.constantOffsets.length == 0);

		result = BlockEncoder.tryEncode(32, new InstructionBlock(codeWriter, new ArrayList<Instruction>(), 0), BlockEncoderOptions.NONE);
		assertTrue(result instanceof BlockEncoderResult);
		blockResult = (BlockEncoderResult)result;
		assertEquals(0, codeWriter.toArray().length);
		assertEquals(0L, blockResult.rip);
		assertNull(blockResult.relocInfos);
		assertNotNull(blockResult.newInstructionOffsets);
		assertTrue(blockResult.newInstructionOffsets.length == 0);
		assertNotNull(blockResult.constantOffsets);
		assertTrue(blockResult.constantOffsets.length == 0);

		result = BlockEncoder.tryEncode(64, new InstructionBlock(codeWriter, new ArrayList<Instruction>(), 0), BlockEncoderOptions.NONE);
		assertTrue(result instanceof BlockEncoderResult);
		blockResult = (BlockEncoderResult)result;
		assertEquals(0, codeWriter.toArray().length);
		assertEquals(0L, blockResult.rip);
		assertNull(blockResult.relocInfos);
		assertNotNull(blockResult.newInstructionOffsets);
		assertTrue(blockResult.newInstructionOffsets.length == 0);
		assertNotNull(blockResult.constantOffsets);
		assertTrue(blockResult.constantOffsets.length == 0);
	}

	@Test
	void defaultArgs() {
		final int bitness = 64;
		final long origRip = 0x123456789ABCDE00L;
		final long newRip = 0x8000000000000000L;

		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x09,// jmp short 123456789ABCDE0Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xE9, 0x03, 0x00, 0x00, 0x00,// jmp near ptr 123456789ABCDE0Eh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
		};
		Instruction[] instructions = BlockEncoderTests.decode(bitness, origRip, originalData, DecoderOptions.NONE);
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		Object result = BlockEncoder.tryEncode(bitness, new InstructionBlock(codeWriter, Arrays.asList(instructions), newRip));
		assertTrue(result instanceof BlockEncoderResult);
		BlockEncoderResult blockResult = (BlockEncoderResult)result;
		assertEquals(newRip, blockResult.rip);
		assertEquals(0x28, codeWriter.toArray().length);
		assertNull(blockResult.relocInfos);
		assertNotNull(blockResult.newInstructionOffsets);
		assertTrue(blockResult.newInstructionOffsets.length == 0);
		assertNotNull(blockResult.constantOffsets);
		assertTrue(blockResult.constantOffsets.length == 0);
	}

	@ParameterizedTest
	@MethodSource("verifyResultArraysData")
	void verifyResultArrays(int options) {
		final int bitness = 64;
		final long origRip1 = 0x123456789ABCDE00L;
		final long origRip2 = 0x223456789ABCDE00L;
		final long newRip1 = 0x8000000000000000L;
		final long newRip2 = 0x9000000000000000L;

		{
			Instruction[] instructions1 = BlockEncoderTests.decode(bitness, origRip1, new byte[] { (byte)0xE9, 0x56, 0x78, (byte)0xA5, 0x5A },
					DecoderOptions.NONE);
			CodeWriterImpl codeWriter1 = new CodeWriterImpl();
			Object result = BlockEncoder.tryEncode(bitness, new InstructionBlock(codeWriter1, Arrays.asList(instructions1), newRip1), options);
			assertTrue(result instanceof BlockEncoderResult);
			BlockEncoderResult blockResult = (BlockEncoderResult)result;
			assertEquals(newRip1, blockResult.rip);
			if ((options & BlockEncoderOptions.RETURN_RELOC_INFOS) != 0) {
				assertNotNull(blockResult.relocInfos);
				assertTrue(blockResult.relocInfos.size() == 1);
			} else
				assertNull(blockResult.relocInfos);
			if ((options & BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS) != 0) {
				assertNotNull(blockResult.newInstructionOffsets);
				assertTrue(blockResult.newInstructionOffsets.length == 1);
			} else {
				assertNotNull(blockResult.newInstructionOffsets);
				assertTrue(blockResult.newInstructionOffsets.length == 0);
			}
			if ((options & BlockEncoderOptions.RETURN_CONSTANT_OFFSETS) != 0) {
				assertNotNull(blockResult.constantOffsets);
				assertTrue(blockResult.constantOffsets.length == 1);
			} else {
				assertNotNull(blockResult.constantOffsets);
				assertTrue(blockResult.constantOffsets.length == 0);
			}
		}
		{
			Instruction[] instructions1 = BlockEncoderTests.decode(bitness, origRip1, new byte[] { (byte)0xE9, 0x56, 0x78, (byte)0xA5, 0x5A },
					DecoderOptions.NONE);
			Instruction[] instructions2 = BlockEncoderTests.decode(bitness, origRip2,
					new byte[] { (byte)0x90, (byte)0xE9, 0x56, 0x78, (byte)0xA5, 0x5A }, DecoderOptions.NONE);
			CodeWriterImpl codeWriter1 = new CodeWriterImpl();
			CodeWriterImpl codeWriter2 = new CodeWriterImpl();
			InstructionBlock block1 = new InstructionBlock(codeWriter1, Arrays.asList(instructions1), newRip1);
			InstructionBlock block2 = new InstructionBlock(codeWriter2, Arrays.asList(instructions2), newRip2);
			Object result = BlockEncoder.tryEncode(bitness, new InstructionBlock[] { block1, block2 }, options);
			assertTrue(result instanceof BlockEncoderResult[]);
			BlockEncoderResult[] resultArray = (BlockEncoderResult[])result;
			assertEquals(2, resultArray.length);
			assertEquals(newRip1, resultArray[0].rip);
			assertEquals(newRip2, resultArray[1].rip);
			if ((options & BlockEncoderOptions.RETURN_RELOC_INFOS) != 0) {
				assertNotNull(resultArray[0].relocInfos);
				assertNotNull(resultArray[1].relocInfos);
				assertTrue(resultArray[0].relocInfos.size() == 1);
				assertTrue(resultArray[1].relocInfos.size() == 1);
			}
			else {
				assertNull(resultArray[0].relocInfos);
				assertNull(resultArray[1].relocInfos);
			}
			if ((options & BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS) != 0) {
				assertNotNull(resultArray[0].newInstructionOffsets);
				assertNotNull(resultArray[1].newInstructionOffsets);
				assertTrue(resultArray[0].newInstructionOffsets.length == 1);
				assertTrue(resultArray[1].newInstructionOffsets.length == 2);
			}
			else {
				assertNotNull(resultArray[0].newInstructionOffsets);
				assertTrue(resultArray[0].newInstructionOffsets.length == 0);
				assertNotNull(resultArray[1].newInstructionOffsets);
				assertTrue(resultArray[1].newInstructionOffsets.length == 0);
			}
			if ((options & BlockEncoderOptions.RETURN_CONSTANT_OFFSETS) != 0) {
				assertNotNull(resultArray[0].constantOffsets);
				assertNotNull(resultArray[1].constantOffsets);
				assertTrue(resultArray[0].constantOffsets.length == 1);
				assertTrue(resultArray[1].constantOffsets.length == 2);
			}
			else {
				assertNotNull(resultArray[0].constantOffsets);
				assertTrue(resultArray[0].constantOffsets.length == 0);
				assertNotNull(resultArray[1].constantOffsets);
				assertTrue(resultArray[1].constantOffsets.length == 0);
			}
		}
	}

	static Iterable<Arguments> verifyResultArraysData() {
		ArrayList<Arguments> result = new ArrayList<Arguments>(3);
		result.add(Arguments.of(BlockEncoderOptions.RETURN_RELOC_INFOS));
		result.add(Arguments.of(BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS));
		result.add(Arguments.of(BlockEncoderOptions.RETURN_CONSTANT_OFFSETS));
		return result;
	}

	@ParameterizedTest
	@ValueSource(strings = {
		"5A",
		"F0 D2 7A 18 A0",
		"77 A9 CE 9D 55 05 42 6C 86 32 FE 4F 34 27 AA 08",
	})
	void encodeDeclareByte(String hexBytes) {
		final int bitness = 64;
		final long newRip = 0x8000000000000000L;

		byte[] data = HexUtils.toByteArray(hexBytes);
		List<Instruction> instructions = new ArrayList<Instruction>();
		instructions.add(Instruction.create(Code.NOPD));
		instructions.add(Instruction.createDeclareByte(data));
		instructions.add(Instruction.create(Code.NOPD));

		byte[] expectedData = new byte[data.length + 2];
		expectedData[0] = (byte)0x90;
		for (int i = 0; i < data.length; i++)
			expectedData[i + 1] = data[i];
		expectedData[expectedData.length - 1] = (byte)0x90;

		CodeWriterImpl codeWriter = new CodeWriterImpl();
		Object result = BlockEncoder.tryEncode(bitness, new InstructionBlock(codeWriter, instructions, newRip));
		assertTrue(result instanceof BlockEncoderResult);
		BlockEncoderResult blockResult = (BlockEncoderResult)result;
		assertArrayEquals(expectedData, codeWriter.toArray());
		assertEquals(newRip, blockResult.rip);
		assertNull(blockResult.relocInfos);
		assertNotNull(blockResult.newInstructionOffsets);
		assertTrue(blockResult.newInstructionOffsets.length == 0);
		assertNotNull(blockResult.constantOffsets);
		assertTrue(blockResult.constantOffsets.length == 0);
	}

	@Test
	void tryEncode_with_default_InstructionBlock_throws() {
		assertThrows(NullPointerException.class, () -> BlockEncoder.tryEncode(64, (InstructionBlock)null));
		assertThrows(NullPointerException.class, () -> BlockEncoder.tryEncode(64, new InstructionBlock[3]));
	}

	@Test
	void tryEncode_with_null_array_throws() {
		assertThrows(NullPointerException.class, () -> BlockEncoder.tryEncode(64, (InstructionBlock[])null));
	}

	@Test
	void tryEncode_with_invalid_bitness_throws() {
		for (int bitness : BitnessUtils.getInvalidBitnessValues())
			assertThrows(IllegalArgumentException.class,
					() -> BlockEncoder.tryEncode(bitness, new InstructionBlock(new CodeWriterImpl(), new ArrayList<Instruction>(), 0)));
		for (int bitness : BitnessUtils.getInvalidBitnessValues())
			assertThrows(IllegalArgumentException.class,
					() -> BlockEncoder.tryEncode(bitness, new InstructionBlock[] { new InstructionBlock(new CodeWriterImpl(), new ArrayList<Instruction>(), 0) }));
	}

	@Test
	void instructionBlock_throws_if_invalid_input() {
		assertThrows(NullPointerException.class, () -> new InstructionBlock(null, new ArrayList<Instruction>(), 0));
		assertThrows(NullPointerException.class, () -> new InstructionBlock(new CodeWriterImpl(), null, 0));
	}

	@Test
	void encodeRipRelMemOp() {
		Instruction instr = Instruction.create(Code.ADD_R32_RM32, ICRegisters.ecx,
				new MemoryOperand(ICRegisters.rip, ICRegister.NONE, 1, 0x1234_5678_9ABC_DEF1L, 8, false, ICRegister.NONE));
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		List<Instruction> instrs = new ArrayList<Instruction>();
		instrs.add(instr);
		Object result = BlockEncoder.tryEncode(64, new InstructionBlock(codeWriter, instrs, 0x1234_5678_ABCD_EF02L));
		assertTrue(result instanceof BlockEncoderResult);
		byte[] encoded = codeWriter.toArray();
		byte[] expected = new byte[] { 0x03, 0x0D, (byte)0xE9, (byte)0xEF, (byte)0xEE, (byte)0xEE };
		assertArrayEquals(expected, encoded);
	}
}
