// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;
import java.util.Arrays;

import static org.junit.jupiter.api.Assertions.*;

import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.CodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderOptions;

class BlockEncoderTests {
	protected static final int DECODER_OPTIONS = DecoderOptions.NONE;

	static Instruction[] decode(int bitness, long rip, byte[] data, int options) {
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(data), options);
		decoder.setIP(rip);
		ArrayList<Instruction> list = new ArrayList<Instruction>();
		while (Long.compareUnsigned(decoder.getIP() - rip, data.length) < 0)
			list.add(decoder.decode());
		if (decoder.getIP() - rip != data.length)
			throw new UnsupportedOperationException();
		return list.toArray(new Instruction[0]);
	}

	private static RelocInfo[] sort(ArrayList<RelocInfo> list) {
		list.sort((a, b) -> {
			int c = Long.compareUnsigned(a.address, b.address);
			if (c != 0)
				return c;
			return a.kind - b.kind;
		});
		return list.toArray(new RelocInfo[0]);
	}

	static final class CodeReaderImpl extends CodeReader {
		final byte[] data;
		public int index;

		public CodeReaderImpl(byte[] data) {
			this.data = data;
		}

		@Override
		public int readByte() {
			if (Integer.compareUnsigned(index, data.length) >= 0)
				return -1;
			return data[index++] & 0xFF;
		}
	}

	void encodeBase(int bitness, long origRip, byte[] originalData, long newRip, byte[] newData, int options, int decoderOptions, int[] expectedInstructionOffsets, RelocInfo[] expectedRelocInfos) {
		Instruction[] origInstrs = decode(bitness, origRip, originalData, decoderOptions);
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		options |= BlockEncoderOptions.RETURN_RELOC_INFOS | BlockEncoderOptions.RETURN_NEW_INSTRUCTION_OFFSETS | BlockEncoderOptions.RETURN_CONSTANT_OFFSETS;
		Object result = BlockEncoder.tryEncode(bitness, new InstructionBlock(codeWriter, Arrays.asList(origInstrs), newRip), options);
		assertTrue(result instanceof BlockEncoderResult);
		BlockEncoderResult blockResult = (BlockEncoderResult)result;
		byte[] encodedBytes = codeWriter.toArray();
		assertArrayEquals(newData, encodedBytes);
		assertEquals(newRip, blockResult.rip);
		ArrayList<RelocInfo> relocInfos = blockResult.relocInfos;
		int[] newInstructionOffsets = blockResult.newInstructionOffsets;
		ConstantOffsets[] constantOffsets = blockResult.constantOffsets;
		assertNotNull(relocInfos);
		assertNotNull(newInstructionOffsets);
		assertEquals(origInstrs.length, newInstructionOffsets.length);
		assertNotNull(constantOffsets);
		assertEquals(origInstrs.length, constantOffsets.length);
		ArrayList<RelocInfo> expectedList = new ArrayList<RelocInfo>(expectedRelocInfos.length);
		expectedList.addAll(Arrays.asList(expectedRelocInfos));
		assertArrayEquals(sort(expectedList), sort(relocInfos));
		assertArrayEquals(expectedInstructionOffsets, newInstructionOffsets);

		ConstantOffsets[] expectedConstantOffsets = new ConstantOffsets[constantOffsets.length];
		CodeReaderImpl reader = new CodeReaderImpl(encodedBytes);
		Decoder decoder = new Decoder(bitness, reader, decoderOptions);
		for (int i = 0; i < newInstructionOffsets.length; i++) {
			if (newInstructionOffsets[i] == 0xFFFF_FFFF)
				expectedConstantOffsets[i] = new ConstantOffsets();
			else {
				reader.index = newInstructionOffsets[i];
				decoder.setIP(newRip + newInstructionOffsets[i]);
				Instruction instruction = decoder.decode();
				expectedConstantOffsets[i] = decoder.getConstantOffsets(instruction);
			}
		}
		assertArrayEquals(expectedConstantOffsets, constantOffsets);
	}
}
