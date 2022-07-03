// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import java.util.function.Consumer;

import static org.junit.jupiter.api.Assertions.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.enc.BlockEncoderOptions;

abstract class CodeAssemblerTestsBase {
	protected final int FIRST_LABEL_ID = 1;
	final int bitness;

	protected CodeAssemblerTestsBase(int bitness) {
		this.bitness = bitness;
	}

	public int getBitness() {
		return bitness;
	}

	protected void testAssembler(Consumer<CodeAssembler> fAsm, Instruction expected) {
		testAssembler(fAsm, expected, TestInstrFlags.NONE, DecoderOptions.NONE);
	}

	protected void testAssembler(Consumer<CodeAssembler> fAsm, Instruction expected, int flags) {
		testAssembler(fAsm, expected, flags, DecoderOptions.NONE);
	}

	protected void testAssembler(Consumer<CodeAssembler> fAsm, Instruction expected, int flags, int decoderOptions) {
		CodeAssembler assembler = new CodeAssembler(bitness);

		// Encode the instruction
		if ((flags & TestInstrFlags.PREFER_VEX) != 0)
			assembler.setPreferVex(true);
		else if ((flags & TestInstrFlags.PREFER_EVEX) != 0)
			assembler.setPreferVex(false);
		if ((flags & TestInstrFlags.PREFER_SHORT_BRANCH) != 0)
			assembler.setPreferShortBranch(true);
		else if ((flags & TestInstrFlags.PREFER_NEAR_BRANCH) != 0)
			assembler.setPreferShortBranch(false);
		fAsm.accept(assembler);

		// Expecting only one instruction
		assertEquals(1, assembler.getInstructions().size());

		// Check that the instruction is the one expected
		if ((flags & TestInstrFlags.BROADCAST) != 0)
			expected.setBroadcast(true);
		Instruction asmInstr = assembler.getInstructions().get(0);
		assertEquals(expected, asmInstr);

		// Encode the instruction first to get any errors
		CodeWriterImpl writer = new CodeWriterImpl();
		assembler.assemble(writer, 0, (flags & TestInstrFlags.BRANCH_U64) != 0 ? BlockEncoderOptions.NONE : BlockEncoderOptions.DONT_FIX_BRANCHES);

		// Check decoding back against the original instruction
		StringBuilder instructionAsBytes = new StringBuilder();
		for (byte b : writer.toArray())
			instructionAsBytes.append(String.format("%02X ", b));

		byte[] instrBytes = writer.toArray();
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(instrBytes), decoderOptions);
		Instruction decodedInstr;
		if (expected.getCode() == Code.ZERO_BYTES && instrBytes.length == 0)
			decodedInstr = Instruction.create(Code.ZERO_BYTES);
		else
			decodedInstr = decoder.decode();
		if ((flags & TestInstrFlags.IGNORE_CODE) != 0)
			decodedInstr.setCode(asmInstr.getCode());
		if ((flags & TestInstrFlags.REMOVE_REP_REPNE_PREFIXES) != 0) {
			decodedInstr.setRepPrefix(false);
			decodedInstr.setRepnePrefix(false);
		}
		if ((flags & TestInstrFlags.FWAIT) != 0) {
			assertEquals(decodedInstr, Instruction.create(Code.WAIT));
			decodedInstr = decoder.decode();
			int newCode;
			switch (decodedInstr.getCode()) {
			case Code.FNSTENV_M14BYTE:
				newCode = Code.FSTENV_M14BYTE;
				break;
			case Code.FNSTENV_M28BYTE:
				newCode = Code.FSTENV_M28BYTE;
				break;
			case Code.FNSTCW_M2BYTE:
				newCode = Code.FSTCW_M2BYTE;
				break;
			case Code.FNENI:
				newCode = Code.FENI;
				break;
			case Code.FNDISI:
				newCode = Code.FDISI;
				break;
			case Code.FNCLEX:
				newCode = Code.FCLEX;
				break;
			case Code.FNINIT:
				newCode = Code.FINIT;
				break;
			case Code.FNSETPM:
				newCode = Code.FSETPM;
				break;
			case Code.FNSAVE_M94BYTE:
				newCode = Code.FSAVE_M94BYTE;
				break;
			case Code.FNSAVE_M108BYTE:
				newCode = Code.FSAVE_M108BYTE;
				break;
			case Code.FNSTSW_M2BYTE:
				newCode = Code.FSTSW_M2BYTE;
				break;
			case Code.FNSTSW_AX:
				newCode = Code.FSTSW_AX;
				break;
			case Code.FNSTDW_AX:
				newCode = Code.FSTDW_AX;
				break;
			case Code.FNSTSG_AX:
				newCode = Code.FSTSG_AX;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			decodedInstr.setCode(newCode);
		}

		if (asmInstr.getCode() != Code.JMPE_DISP16 && asmInstr.getCode() != Code.JMPE_DISP32 && (flags & TestInstrFlags.BRANCH) != 0)
			asmInstr.setNearBranch64(0);

		// Short branches can be re-written if the target is too far away.
		// Eg. `loopne target` => `loopne jmpt; jmp short skip; jmpt: jmp near target; skip:`
		if ((flags & TestInstrFlags.BRANCH_U64) != 0) {
			asmInstr.setCode(Code.toShortBranch(asmInstr.getCode()));
			decodedInstr.setCode(Code.toShortBranch(decodedInstr.getCode()));
			assertEquals(asmInstr.getCode(), decodedInstr.getCode());

			if (decodedInstr.getNearBranch64() == 4) {
				Instruction nextDecodedInst = decoder.decode();
				int expectedCode;
				switch (getBitness()) {
				case 16:
					expectedCode = Code.JMP_REL8_16;
					break;
				case 32:
					expectedCode = Code.JMP_REL8_32;
					break;
				case 64:
					expectedCode = Code.JMP_REL8_64;
					break;
				default:
					throw new UnsupportedOperationException();
				}

				assertEquals(expectedCode, nextDecodedInst.getCode(),
						String.format("Branch ulong next decoding failed!\nExpected: Code %d \nActual Decoded: %s\n", expectedCode, nextDecodedInst));
			}
			else
				assertEquals(decodedInstr, asmInstr, String.format("Branch decoding offset failed!\nExpected: %s (%s)\nActual Decoded: %s\n",
						asmInstr, instructionAsBytes, decodedInstr));
		}
		else
			assertEquals(decodedInstr, asmInstr,
					String.format("Decoding failed!\nExpected: %s (%s)\nActual Decoded: %s\n", asmInstr, instructionAsBytes, decodedInstr));
	}

	protected void testAssemblerDeclareByte(Consumer<CodeAssembler> fAsm, byte[] data) {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		fAsm.accept(assembler);

		CodeWriterImpl writer = new CodeWriterImpl();
		assembler.assemble(writer, 0);
		byte[] buffer = writer.toArray();

		assertArrayEquals(data, buffer);
	}

	protected void testAssemblerDeclareWord(Consumer<CodeAssembler> fAsm, short[] data) {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		fAsm.accept(assembler);

		CodeWriterImpl writer = new CodeWriterImpl();
		assembler.assemble(writer, 0);
		byte[] buffer = writer.toArray();

		final int SIZE_OF_ELEM = 2;
		byte[] cvtData = new byte[SIZE_OF_ELEM * data.length];
		for (int i = 0; i < data.length; i++) {
			short d = data[i];
			cvtData[i * SIZE_OF_ELEM + 0] = (byte)d;
			cvtData[i * SIZE_OF_ELEM + 1] = (byte)(d >>> 8);
		}
		assertArrayEquals(cvtData, buffer);
	}

	protected void testAssemblerDeclareDword(Consumer<CodeAssembler> fAsm, int[] data) {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		fAsm.accept(assembler);

		CodeWriterImpl writer = new CodeWriterImpl();
		assembler.assemble(writer, 0);
		byte[] buffer = writer.toArray();

		final int SIZE_OF_ELEM = 4;
		byte[] cvtData = new byte[SIZE_OF_ELEM * data.length];
		for (int i = 0; i < data.length; i++) {
			int d = data[i];
			cvtData[i * SIZE_OF_ELEM + 0] = (byte)d;
			cvtData[i * SIZE_OF_ELEM + 1] = (byte)(d >>> 8);
			cvtData[i * SIZE_OF_ELEM + 2] = (byte)(d >>> 16);
			cvtData[i * SIZE_OF_ELEM + 3] = (byte)(d >>> 24);
		}
		assertArrayEquals(cvtData, buffer);
	}

	protected void testAssemblerDeclareDword(Consumer<CodeAssembler> fAsm, float[] data) {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		fAsm.accept(assembler);

		CodeWriterImpl writer = new CodeWriterImpl();
		assembler.assemble(writer, 0);
		byte[] buffer = writer.toArray();

		final int SIZE_OF_ELEM = 4;
		byte[] cvtData = new byte[SIZE_OF_ELEM * data.length];
		for (int i = 0; i < data.length; i++) {
			int d = Float.floatToRawIntBits(data[i]);
			cvtData[i * SIZE_OF_ELEM + 0] = (byte)d;
			cvtData[i * SIZE_OF_ELEM + 1] = (byte)(d >>> 8);
			cvtData[i * SIZE_OF_ELEM + 2] = (byte)(d >>> 16);
			cvtData[i * SIZE_OF_ELEM + 3] = (byte)(d >>> 24);
		}
		assertArrayEquals(cvtData, buffer);
	}

	protected void testAssemblerDeclareQword(Consumer<CodeAssembler> fAsm, long[] data) {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		fAsm.accept(assembler);

		CodeWriterImpl writer = new CodeWriterImpl();
		assembler.assemble(writer, 0);
		byte[] buffer = writer.toArray();

		final int SIZE_OF_ELEM = 8;
		byte[] cvtData = new byte[SIZE_OF_ELEM * data.length];
		for (int i = 0; i < data.length; i++) {
			long l = data[i];
			int d = (int)l;
			cvtData[i * SIZE_OF_ELEM + 0] = (byte)d;
			cvtData[i * SIZE_OF_ELEM + 1] = (byte)(d >>> 8);
			cvtData[i * SIZE_OF_ELEM + 2] = (byte)(d >>> 16);
			cvtData[i * SIZE_OF_ELEM + 3] = (byte)(d >>> 24);
			d = (int)(l >>> 32);
			cvtData[i * SIZE_OF_ELEM + 4] = (byte)d;
			cvtData[i * SIZE_OF_ELEM + 5] = (byte)(d >>> 8);
			cvtData[i * SIZE_OF_ELEM + 6] = (byte)(d >>> 16);
			cvtData[i * SIZE_OF_ELEM + 7] = (byte)(d >>> 24);
		}
		assertArrayEquals(cvtData, buffer);
	}

	protected void testAssemblerDeclareQword(Consumer<CodeAssembler> fAsm, double[] data) {
		CodeAssembler assembler = new CodeAssembler(getBitness());
		fAsm.accept(assembler);

		CodeWriterImpl writer = new CodeWriterImpl();
		assembler.assemble(writer, 0);
		byte[] buffer = writer.toArray();

		final int SIZE_OF_ELEM = 8;
		byte[] cvtData = new byte[SIZE_OF_ELEM * data.length];
		for (int i = 0; i < data.length; i++) {
			long l = Double.doubleToRawLongBits(data[i]);
			int d = (int)l;
			cvtData[i * SIZE_OF_ELEM + 0] = (byte)d;
			cvtData[i * SIZE_OF_ELEM + 1] = (byte)(d >>> 8);
			cvtData[i * SIZE_OF_ELEM + 2] = (byte)(d >>> 16);
			cvtData[i * SIZE_OF_ELEM + 3] = (byte)(d >>> 24);
			d = (int)(l >>> 32);
			cvtData[i * SIZE_OF_ELEM + 4] = (byte)d;
			cvtData[i * SIZE_OF_ELEM + 5] = (byte)(d >>> 8);
			cvtData[i * SIZE_OF_ELEM + 6] = (byte)(d >>> 16);
			cvtData[i * SIZE_OF_ELEM + 7] = (byte)(d >>> 24);
		}
		assertArrayEquals(cvtData, buffer);
	}

	protected static CodeLabel createAndEmitLabel(CodeAssembler c) {
		CodeLabel label = c.createLabel();
		c.label(label);
		return label;
	}

	protected static Instruction assignLabel(Instruction instruction, long value) {
		instruction.setIP(value);
		return instruction;
	}

	protected static Instruction applyK(Instruction instruction, int k) {
		instruction.setOpMask(k);
		return instruction;
	}

	protected static void assertInvalid(Runnable action) {
		try {
			action.run();
		} catch (IllegalArgumentException ex) {
			assertTrue(ex.getMessage().contains("Unable to calculate an OpCode"));
			return;
		}
		assertTrue(false);
	}
}
