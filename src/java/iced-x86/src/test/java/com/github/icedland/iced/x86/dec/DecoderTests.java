// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.VsibFlags;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class DecoderTests {
	@ParameterizedTest
	@MethodSource("decoder16Data")
	void decoder16(int bitness, int lineNo, String hexBytes, DecoderTestCase tc) {
		decoderTest(bitness, lineNo, hexBytes, tc);
	}

	static Iterable<Arguments> decoder16Data() {
		return getDecoderTestData(16);
	}

	@ParameterizedTest
	@MethodSource("decoderMisc16Data")
	void decoderMisc16(int bitness, int lineNo, String hexBytes, DecoderTestCase tc) {
		decoderTest(bitness, lineNo, hexBytes, tc);
	}

	static Iterable<Arguments> decoderMisc16Data() {
		return getMiscDecoderTestData(16);
	}

	@ParameterizedTest
	@MethodSource("decodeMemOps16Data")
	void decodeMemOps16(String hexBytes, int code, DecoderMemoryTestCase tc) {
		decodeMemOps(16, hexBytes, code, tc);
	}

	static Iterable<Arguments> decodeMemOps16Data() {
		return getMemOpsData(16);
	}

	@ParameterizedTest
	@MethodSource("decoder32Data")
	void decoder32(int bitness, int lineNo, String hexBytes, DecoderTestCase tc) {
		decoderTest(bitness, lineNo, hexBytes, tc);
	}

	static Iterable<Arguments> decoder32Data() {
		return getDecoderTestData(32);
	}

	@ParameterizedTest
	@MethodSource("decoderMisc32Data")
	void decoderMisc32(int bitness, int lineNo, String hexBytes, DecoderTestCase tc) {
		decoderTest(bitness, lineNo, hexBytes, tc);
	}

	static Iterable<Arguments> decoderMisc32Data() {
		return getMiscDecoderTestData(32);
	}

	@ParameterizedTest
	@MethodSource("decodeMemOps32Data")
	void decodeMemOps32(String hexBytes, int code, DecoderMemoryTestCase tc) {
		decodeMemOps(32, hexBytes, code, tc);
	}

	static Iterable<Arguments> decodeMemOps32Data() {
		return getMemOpsData(32);
	}

	@ParameterizedTest
	@MethodSource("decoder64Data")
	void decoder64(int bitness, int lineNo, String hexBytes, DecoderTestCase tc) {
		decoderTest(bitness, lineNo, hexBytes, tc);
	}

	static Iterable<Arguments> decoder64Data() {
		return getDecoderTestData(64);
	}

	@ParameterizedTest
	@MethodSource("decoderMisc64Data")
	void decoderMisc64(int bitness, int lineNo, String hexBytes, DecoderTestCase tc) {
		decoderTest(bitness, lineNo, hexBytes, tc);
	}

	static Iterable<Arguments> decoderMisc64Data() {
		return getMiscDecoderTestData(64);
	}

	@ParameterizedTest
	@MethodSource("decodeMemOps64Data")
	void decodeMemOps64(String hexBytes, int code, DecoderMemoryTestCase tc) {
		decodeMemOps(64, hexBytes, code, tc);
	}

	static Iterable<Arguments> decodeMemOps64Data() {
		return getMemOpsData(64);
	}

	static final class CreatedDecoder {
		public final Decoder decoder;
		public final int length;
		public final boolean canRead;
		public final ByteArrayCodeReader codeReader;

		public CreatedDecoder(Decoder decoder, int length, boolean canRead, ByteArrayCodeReader codeReader) {
			this.decoder = decoder;
			this.length = length;
			this.canRead = canRead;
			this.codeReader = codeReader;
		}
	}

	private CreatedDecoder createDecoder(int bitness, String hexBytes, long ip, int options) {
		ByteArrayCodeReader codeReader = new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes));
		Decoder decoder = new Decoder(bitness, codeReader, options);
		decoder.setIP(ip);
		assertEquals(bitness, decoder.getBitness());
		int length = Math.min(IcedConstants.MAX_INSTRUCTION_LENGTH, codeReader.size());
		boolean canRead = length < codeReader.size();
		return new CreatedDecoder(decoder, length, canRead, codeReader);
	}

	private void decodeMemOps(int bitness, String hexBytes, int code, DecoderMemoryTestCase tc) {
		assert bitness == tc.bitness : bitness;
		assert hexBytes == tc.hexBytes : hexBytes;
		CreatedDecoder cdec = createDecoder(tc.bitness, tc.hexBytes, tc.ip, tc.decoderOptions);
		Instruction instruction = cdec.decoder.decode();
		assertEquals(DecoderError.NONE, cdec.decoder.getLastError());
		assertEquals(cdec.canRead, cdec.codeReader.canReadByte());

		assertEquals(code, instruction.getCode());
		assertEquals(code == Code.INVALID, instruction.isInvalid());
		assertEquals(2, instruction.getOpCount());
		assertEquals(cdec.length, instruction.getLength());
		assertFalse(instruction.getRepPrefix());
		assertFalse(instruction.getRepePrefix());
		assertFalse(instruction.getRepnePrefix());
		assertFalse(instruction.getLockPrefix());
		assertEquals(tc.segmentPrefix, instruction.getSegmentPrefix());
		if (instruction.getSegmentPrefix() == Register.NONE)
			assertFalse(instruction.hasSegmentPrefix());
		else
			assertTrue(instruction.hasSegmentPrefix());

		assertEquals(OpKind.MEMORY, instruction.getOp0Kind());
		assertEquals(tc.segmentRegister, instruction.getMemorySegment());
		assertEquals(tc.baseRegister, instruction.getMemoryBase());
		assertEquals(tc.indexRegister, instruction.getMemoryIndex());
		assertEquals((int)tc.displacement, instruction.getMemoryDisplacement32());
		assertEquals(tc.displacement, instruction.getMemoryDisplacement64());
		assertEquals(1 << tc.scale, instruction.getMemoryIndexScale());
		assertEquals(tc.displacementSize, instruction.getMemoryDisplSize());

		assertEquals(OpKind.REGISTER, instruction.getOp1Kind());
		assertEquals(tc.register, instruction.getOp1Register());
		verifyConstantOffsets(tc.constantOffsets, cdec.decoder.getConstantOffsets(instruction));
	}

	private static Iterable<Arguments> getMemOpsData(int bitness) {
		DecoderMemoryTestCase[] allTestCases = DecoderTestCases.getMemoryTestCases(bitness);
		ArrayList<Arguments> result = new ArrayList<Arguments>(allTestCases.length);
		for (DecoderMemoryTestCase tc : allTestCases)
			result.add(Arguments.of(tc.hexBytes, tc.code, tc));
		return result;
	}

	private static Iterable<Arguments> getDecoderTestData(int bitness) {
		DecoderTestCase[] allTestCases = DecoderTestCases.getTestCases(bitness);
		Integer boxedBitness = bitness;
		ArrayList<Arguments> result = new ArrayList<Arguments>(allTestCases.length);
		for (DecoderTestCase tc : allTestCases) {
			assert bitness == tc.bitness : bitness;
			result.add(Arguments.of(boxedBitness, tc.lineNumber, tc.hexBytes, tc));
		}
		return result;
	}

	private static Iterable<Arguments> getMiscDecoderTestData(int bitness) {
		DecoderTestCase[] allTestCases = DecoderTestCases.getMiscTestCases(bitness);
		Integer boxedBitness = bitness;
		ArrayList<Arguments> result = new ArrayList<Arguments>(allTestCases.length);
		for (DecoderTestCase tc : allTestCases) {
			assert bitness == tc.bitness : bitness;
			result.add(Arguments.of(boxedBitness, tc.lineNumber, tc.hexBytes, tc));
		}
		return result;
	}

	private void decoderTest(int bitness, int lineNo, String hexBytes, DecoderTestCase tc) {
		CreatedDecoder cdec = createDecoder(bitness, hexBytes, tc.ip, tc.decoderOptions);
		long rip = cdec.decoder.getIP();
		Instruction instruction = new Instruction();
		cdec.decoder.decode(instruction);
		assertEquals(tc.decoderError, cdec.decoder.getLastError());
		assertEquals(cdec.canRead, cdec.codeReader.canReadByte());
		assertEquals(tc.code, instruction.getCode());
		assertEquals(tc.code == Code.INVALID, instruction.isInvalid());
		assertEquals(tc.mnemonic, instruction.getMnemonic());
		assertEquals(instruction.getMnemonic(), Code.mnemonic(instruction.getCode()));
		assertEquals(cdec.length, instruction.getLength());
		assertEquals(rip, instruction.getIP());
		assertEquals(cdec.decoder.getIP(), instruction.getNextIP());
		assertEquals(rip + cdec.length, instruction.getNextIP());
		switch (bitness) {
		case 16:
			assertEquals(CodeSize.CODE16, instruction.getCodeSize());
			break;
		case 32:
			assertEquals(CodeSize.CODE32, instruction.getCodeSize());
			break;
		case 64:
			assertEquals(CodeSize.CODE64, instruction.getCodeSize());
			break;
		default:
			throw new UnsupportedOperationException();
		}
		assertEquals(tc.opCount, instruction.getOpCount());
		assertEquals(tc.zeroingMasking, instruction.getZeroingMasking());
		assertEquals(!tc.zeroingMasking, instruction.getMergingMasking());
		assertEquals(tc.suppressAllExceptions, instruction.getSuppressAllExceptions());
		assertEquals(tc.isBroadcast, instruction.getBroadcast());
		assertEquals(tc.hasXacquirePrefix, instruction.getXacquirePrefix());
		assertEquals(tc.hasXreleasePrefix, instruction.getXreleasePrefix());
		assertEquals(tc.hasRepePrefix, instruction.getRepPrefix());
		assertEquals(tc.hasRepePrefix, instruction.getRepePrefix());
		assertEquals(tc.hasRepnePrefix, instruction.getRepnePrefix());
		assertEquals(tc.hasLockPrefix, instruction.getLockPrefix());
		assertEquals(tc.mvexEvictionHint, instruction.getMvexEvictionHint());
		assertEquals(tc.mvexRegMemConv, instruction.getMvexRegMemConv());
		switch (tc.vsibBitness) {
		case 0:
			assertFalse(instruction.isVsib());
			assertFalse(instruction.isVsib32());
			assertFalse(instruction.isVsib64());
			assertEquals(VsibFlags.NONE, instruction.getVsib());
			break;

		case 32:
			assertTrue(instruction.isVsib());
			assertTrue(instruction.isVsib32());
			assertFalse(instruction.isVsib64());
			assertEquals(VsibFlags.VSIB | VsibFlags.VSIB32, instruction.getVsib());
			break;

		case 64:
			assertTrue(instruction.isVsib());
			assertFalse(instruction.isVsib32());
			assertTrue(instruction.isVsib64());
			assertEquals(VsibFlags.VSIB | VsibFlags.VSIB64, instruction.getVsib());
			break;

		default:
			throw new UnsupportedOperationException();
		}
		assertEquals(tc.opMask, instruction.getOpMask());
		assertEquals(tc.opMask != Register.NONE, instruction.hasOpMask());
		assertEquals(tc.roundingControl, instruction.getRoundingControl());
		assertEquals(tc.segmentPrefix, instruction.getSegmentPrefix());
		if (instruction.getSegmentPrefix() == Register.NONE)
			assertFalse(instruction.hasSegmentPrefix());
		else
			assertTrue(instruction.hasSegmentPrefix());
		for (int i = 0; i < tc.opCount; i++) {
			int opKind = tc.getOpKind(i);
			assertEquals(opKind, instruction.getOpKind(i));
			switch (opKind) {
			case OpKind.REGISTER:
				assertEquals(tc.getOpRegister(i), instruction.getOpRegister(i));
				break;

			case OpKind.NEAR_BRANCH16:
				assertEquals(tc.nearBranch, instruction.getNearBranch16() & 0xFFFF);
				assertEquals(tc.nearBranch, instruction.getNearBranchTarget());
				break;

			case OpKind.NEAR_BRANCH32:
				assertEquals(tc.nearBranch, instruction.getNearBranch32() & 0xFFFF_FFFFL);
				assertEquals(tc.nearBranch, instruction.getNearBranchTarget());
				break;

			case OpKind.NEAR_BRANCH64:
				assertEquals(tc.nearBranch, instruction.getNearBranch64());
				assertEquals(tc.nearBranch, instruction.getNearBranchTarget());
				break;

			case OpKind.FAR_BRANCH16:
				assertEquals(tc.farBranch, instruction.getFarBranch16());
				assertEquals(tc.farBranchSelector, instruction.getFarBranchSelector());
				break;

			case OpKind.FAR_BRANCH32:
				assertEquals(tc.farBranch, instruction.getFarBranch32());
				assertEquals(tc.farBranchSelector, instruction.getFarBranchSelector());
				break;

			case OpKind.IMMEDIATE8:
				assertEquals((byte)tc.immediate, instruction.getImmediate8());
				break;

			case OpKind.IMMEDIATE8_2ND:
				assertEquals(tc.immediate_2nd, instruction.getImmediate8_2nd());
				break;

			case OpKind.IMMEDIATE16:
				assertEquals((short)tc.immediate, instruction.getImmediate16());
				break;

			case OpKind.IMMEDIATE32:
				assertEquals((int)tc.immediate, instruction.getImmediate32());
				break;

			case OpKind.IMMEDIATE64:
				assertEquals(tc.immediate, instruction.getImmediate64());
				break;

			case OpKind.IMMEDIATE8TO16:
				assertEquals((short)tc.immediate, instruction.getImmediate8to16());
				break;

			case OpKind.IMMEDIATE8TO32:
				assertEquals((int)tc.immediate, instruction.getImmediate8to32());
				break;

			case OpKind.IMMEDIATE8TO64:
				assertEquals(tc.immediate, instruction.getImmediate8to64());
				break;

			case OpKind.IMMEDIATE32TO64:
				assertEquals(tc.immediate, instruction.getImmediate32to64());
				break;

			case OpKind.MEMORY_SEG_SI:
			case OpKind.MEMORY_SEG_ESI:
			case OpKind.MEMORY_SEG_RSI:
			case OpKind.MEMORY_SEG_DI:
			case OpKind.MEMORY_SEG_EDI:
			case OpKind.MEMORY_SEG_RDI:
				assertEquals(tc.memorySegment, instruction.getMemorySegment());
				assertEquals(tc.memorySize, instruction.getMemorySize());
				break;

			case OpKind.MEMORY_ESDI:
			case OpKind.MEMORY_ESEDI:
			case OpKind.MEMORY_ESRDI:
				assertEquals(tc.memorySize, instruction.getMemorySize());
				break;

			case OpKind.MEMORY:
				assertEquals(tc.memorySegment, instruction.getMemorySegment());
				assertEquals(tc.memoryBase, instruction.getMemoryBase());
				assertEquals(tc.memoryIndex, instruction.getMemoryIndex());
				assertEquals(tc.memoryIndexScale, instruction.getMemoryIndexScale());
				assertEquals((int)tc.memoryDisplacement, instruction.getMemoryDisplacement32());
				assertEquals(tc.memoryDisplacement, instruction.getMemoryDisplacement64());
				assertEquals(tc.memoryDisplSize, instruction.getMemoryDisplSize());
				assertEquals(tc.memorySize, instruction.getMemorySize());
				break;

			default:
				throw new UnsupportedOperationException();
			}
		}
		if (tc.opCount >= 1) {
			assertEquals(tc.op0Kind, instruction.getOp0Kind());
			if (tc.op0Kind == OpKind.REGISTER)
				assertEquals(tc.op0Register, instruction.getOp0Register());
			if (tc.opCount >= 2) {
				assertEquals(tc.op1Kind, instruction.getOp1Kind());
				if (tc.op1Kind == OpKind.REGISTER)
					assertEquals(tc.op1Register, instruction.getOp1Register());
				if (tc.opCount >= 3) {
					assertEquals(tc.op2Kind, instruction.getOp2Kind());
					if (tc.op2Kind == OpKind.REGISTER)
						assertEquals(tc.op2Register, instruction.getOp2Register());
					if (tc.opCount >= 4) {
						assertEquals(tc.op3Kind, instruction.getOp3Kind());
						if (tc.op3Kind == OpKind.REGISTER)
							assertEquals(tc.op3Register, instruction.getOp3Register());
						if (tc.opCount >= 5) {
							assertEquals(tc.op4Kind, instruction.getOp4Kind());
							if (tc.op4Kind == OpKind.REGISTER)
								assertEquals(tc.op4Register, instruction.getOp4Register());
							assertEquals(5, tc.opCount);
						}
					}
				}
			}
		}
		verifyConstantOffsets(tc.constantOffsets, cdec.decoder.getConstantOffsets(instruction));
	}

	static void verifyConstantOffsets(ConstantOffsets expected, ConstantOffsets actual) {
		assertEquals(expected.immediateOffset, actual.immediateOffset);
		assertEquals(expected.immediateSize, actual.immediateSize);
		assertEquals(expected.immediateOffset2, actual.immediateOffset2);
		assertEquals(expected.immediateSize2, actual.immediateSize2);
		assertEquals(expected.displacementOffset, actual.displacementOffset);
		assertEquals(expected.displacementSize, actual.displacementSize);
	}
}
