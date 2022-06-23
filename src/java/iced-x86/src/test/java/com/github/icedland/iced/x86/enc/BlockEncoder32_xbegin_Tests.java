// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import org.junit.jupiter.api.*;

final class BlockEncoder32_xbegin_Tests extends BlockEncoderTests {
	static final int bitness = 32;
	static final long origRip = 0x8000L;
	static final long newRip = 0x80000000L;

	@Test
	void xbegin_fwd_rel16() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, 0x08, 0x00,// xbegin 0000800Fh
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ (byte)0xC7, (byte)0xF8, 0x02, 0x00, 0x00, 0x00,// xbegin 00008011h
			/*000F*/ (byte)0xB0, 0x02,// mov al,2
			/*0011*/ (byte)0xB0, 0x03,// mov al,3
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, 0x07, 0x00,// xbegin 8000000Eh
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ 0x66, (byte)0xC7, (byte)0xF8, 0x02, 0x00,// xbegin 80000010h
			/*000E*/ (byte)0xB0, 0x02,// mov al,2
			/*0010*/ (byte)0xB0, 0x03,// mov al,3
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0007,
			0x0009,
			0x000E,
			0x0010,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void xbegin_bwd_rel16() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x02,// mov al,2
			/*0002*/ (byte)0xB0, 0x03,// mov al,3
			/*0004*/ (byte)0xB0, 0x00,// mov al,0
			/*0006*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF5, (byte)0xFF,// xbegin 00008000h
			/*000B*/ (byte)0xB0, 0x01,// mov al,1
			/*000D*/ (byte)0xC7, (byte)0xF8, (byte)0xEF, (byte)0xFF, (byte)0xFF, (byte)0xFF,// xbegin 00008002h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x02,// mov al,2
			/*0002*/ (byte)0xB0, 0x03,// mov al,3
			/*0004*/ (byte)0xB0, 0x00,// mov al,0
			/*0006*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF5, (byte)0xFF,// xbegin 80000000h
			/*000B*/ (byte)0xB0, 0x01,// mov al,1
			/*000D*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF0, (byte)0xFF,// xbegin 80000002h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
			0x0006,
			0x000B,
			0x000D,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void xbegin_fwd_rel32() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, 0x11, 0x00,// xbegin 80000018h
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ (byte)0xC7, (byte)0xF8, 0x09, 0x00, 0x00, 0x00,// xbegin 80000018h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xC7, (byte)0xF8, 0x10, 0x00, 0x01, 0x00,// xbegin 80000018h
			/*0008*/ (byte)0xB0, 0x01,// mov al,1
			/*000A*/ (byte)0xC7, (byte)0xF8, 0x08, 0x00, 0x01, 0x00,// xbegin 80000018h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0008,
			0x000A,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		final long origRip = 0x80000000L;
		encodeBase(bitness, origRip, originalData, origRip - 0x10000, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void xbegin_bwd_rel32() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF8, (byte)0xFF,// xbegin 7FFFFFFFh
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ (byte)0xC7, (byte)0xF8, (byte)0xF0, (byte)0xFF, (byte)0xFF, (byte)0xFF,// xbegin 7FFFFFFFh
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xC7, (byte)0xF8, (byte)0xF7, (byte)0xFF, (byte)0xFE, (byte)0xFF,// xbegin 7FFFFFFFh
			/*0008*/ (byte)0xB0, 0x01,// mov al,1
			/*000A*/ (byte)0xC7, (byte)0xF8, (byte)0xEF, (byte)0xFF, (byte)0xFE, (byte)0xFF,// xbegin 7FFFFFFFh
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0008,
			0x000A,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		final long origRip = 0x80000000L;
		encodeBase(bitness, origRip, originalData, origRip + 0x10000, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}
}
