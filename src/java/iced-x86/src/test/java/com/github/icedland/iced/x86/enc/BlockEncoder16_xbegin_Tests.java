// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import org.junit.jupiter.api.*;

final class BlockEncoder16_xbegin_Tests extends BlockEncoderTests {
	static final int bitness = 16;
	static final long origRip = 0x8000L;
	static final long newRip = 0xF000L;

	@Test
	void xbegin_fwd_rel16() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, 0x06, 0x00, 0x00, 0x00,// xbegin 0000800Fh
			/*0009*/ (byte)0xB0, 0x01,// mov al,1
			/*000B*/ (byte)0xC7, (byte)0xF8, 0x02, 0x00,// xbegin 00008011h
			/*000F*/ (byte)0xB0, 0x02,// mov al,2
			/*0011*/ (byte)0xB0, 0x03,// mov al,3
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xC7, (byte)0xF8, 0x06, 0x00,// xbegin 0000F00Ch
			/*0006*/ (byte)0xB0, 0x01,// mov al,1
			/*0008*/ (byte)0xC7, (byte)0xF8, 0x02, 0x00,// xbegin 0000F00Eh
			/*000C*/ (byte)0xB0, 0x02,// mov al,2
			/*000E*/ (byte)0xB0, 0x03,// mov al,3
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0006,
			0x0008,
			0x000C,
			0x000E,
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
			/*0006*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF3, (byte)0xFF, (byte)0xFF, (byte)0xFF,// xbegin 00008000h
			/*000D*/ (byte)0xB0, 0x01,// mov al,1
			/*000F*/ (byte)0xC7, (byte)0xF8, (byte)0xEF, (byte)0xFF,// xbegin 00008002h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x02,// mov al,2
			/*0002*/ (byte)0xB0, 0x03,// mov al,3
			/*0004*/ (byte)0xB0, 0x00,// mov al,0
			/*0006*/ (byte)0xC7, (byte)0xF8, (byte)0xF6, (byte)0xFF,// xbegin 0000F000h
			/*000A*/ (byte)0xB0, 0x01,// mov al,1
			/*000C*/ (byte)0xC7, (byte)0xF8, (byte)0xF2, (byte)0xFF,// xbegin 0000F002h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
			0x0006,
			0x000A,
			0x000C,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}
}
