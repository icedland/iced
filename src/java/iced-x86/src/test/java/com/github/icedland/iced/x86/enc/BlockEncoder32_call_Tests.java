// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import org.junit.jupiter.api.*;

final class BlockEncoder32_call_Tests extends BlockEncoderTests {
	static final int bitness = 32;
	static final long origRip = 0x8000L;
	static final long newRip = 0x80000000L;

	@Test
	void call_near_fwd() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xE8, 0x07, 0x00, 0x00, 0x00,// call 0000800Ch
			/*0005*/ (byte)0xB0, 0x00,// mov al,0
			/*0007*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			/*000C*/ (byte)0x90,// nop
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xE8, 0x07, 0x00, 0x00, 0x00,// call 8000000Ch
			/*0005*/ (byte)0xB0, 0x00,// mov al,0
			/*0007*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			/*000C*/ (byte)0x90,// nop
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0005,
			0x0007,
			0x000C,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void call_near_bwd() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0x90,// nop
			/*0001*/ (byte)0xE8, (byte)0xFA, (byte)0xFF, (byte)0xFF, (byte)0xFF,// call 00008000h
			/*0006*/ (byte)0xB0, 0x00,// mov al,0
			/*0008*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0x90,// nop
			/*0001*/ (byte)0xE8, (byte)0xFA, (byte)0xFF, (byte)0xFF, (byte)0xFF,// call 80000000h
			/*0006*/ (byte)0xB0, 0x00,// mov al,0
			/*0008*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0001,
			0x0006,
			0x0008,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void call_near_other_near() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xE8, 0x07, 0x00, 0x00, 0x00,// call 0000800Ch
			/*0005*/ (byte)0xB0, 0x00,// mov al,0
			/*0007*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xE8, 0x08, 0x00, 0x00, 0x00,// call 0000800Ch
			/*0005*/ (byte)0xB0, 0x00,// mov al,0
			/*0007*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0005,
			0x0007,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip - 1, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void call_near_other_near_os() {
		byte[] originalData = new byte[] {
			/*0000*/ 0x66, (byte)0xE8, 0x07, 0x00,// call 800Bh
			/*0004*/ (byte)0xB0, 0x00,// mov al,0
			/*0006*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		};
		byte[] newData = new byte[] {
			/*0000*/ 0x66, (byte)0xE8, 0x07, (byte)0x80,// call 800Bh
			/*0004*/ (byte)0xB0, 0x00,// mov al,0
			/*0006*/ (byte)0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0004,
			0x0006,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}
}
