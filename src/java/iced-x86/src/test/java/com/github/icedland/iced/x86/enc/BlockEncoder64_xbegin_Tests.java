// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import org.junit.jupiter.api.*;

final class BlockEncoder64_xbegin_Tests extends BlockEncoderTests {
	static final int bitness = 64;
	static final long origRip = 0x8000L;
	static final long newRip = 0x8000000000000000L;

	@Test
	void xbegin_fwd_rel16() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, 0x11, 0x00,// xbegin 0000000000008018h
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ (byte)0xC7, (byte)0xF8, 0x0B, 0x00, 0x00, 0x00,// xbegin 000000000000801Ah
			/*000F*/ (byte)0xB0, 0x02,// mov al,2
			/*0011*/ 0x48, (byte)0xC7, (byte)0xF8, 0x04, 0x00, 0x00, 0x00,// xbegin 000000000000801Ch
			/*0018*/ (byte)0xB0, 0x03,// mov al,3
			/*001A*/ (byte)0xB0, 0x04,// mov al,4
			/*001C*/ (byte)0xB0, 0x05,// mov al,5
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, 0x0E, 0x00,// xbegin 8000000000000015h
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ 0x66, (byte)0xC7, (byte)0xF8, 0x09, 0x00,// xbegin 8000000000000017h
			/*000E*/ (byte)0xB0, 0x02,// mov al,2
			/*0010*/ 0x66, (byte)0xC7, (byte)0xF8, 0x04, 0x00,// xbegin 8000000000000019h
			/*0015*/ (byte)0xB0, 0x03,// mov al,3
			/*0017*/ (byte)0xB0, 0x04,// mov al,4
			/*0019*/ (byte)0xB0, 0x05,// mov al,5
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0007,
			0x0009,
			0x000E,
			0x0010,
			0x0015,
			0x0017,
			0x0019,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void xbegin_bwd_rel16() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x03,// mov al,3
			/*0002*/ (byte)0xB0, 0x04,// mov al,4
			/*0004*/ (byte)0xB0, 0x05,// mov al,5
			/*0006*/ (byte)0xB0, 0x00,// mov al,0
			/*0008*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF3, (byte)0xFF,// xbegin 0000000000008000h
			/*000D*/ (byte)0xB0, 0x01,// mov al,1
			/*000F*/ (byte)0xC7, (byte)0xF8, (byte)0xED, (byte)0xFF, (byte)0xFF, (byte)0xFF,// xbegin 0000000000008002h
			/*0015*/ (byte)0xB0, 0x02,// mov al,2
			/*0017*/ 0x48, (byte)0xC7, (byte)0xF8, (byte)0xE6, (byte)0xFF, (byte)0xFF, (byte)0xFF,// xbegin 0000000000008004h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x03,// mov al,3
			/*0002*/ (byte)0xB0, 0x04,// mov al,4
			/*0004*/ (byte)0xB0, 0x05,// mov al,5
			/*0006*/ (byte)0xB0, 0x00,// mov al,0
			/*0008*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF3, (byte)0xFF,// xbegin 8000000000000000h
			/*000D*/ (byte)0xB0, 0x01,// mov al,1
			/*000F*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xEE, (byte)0xFF,// xbegin 8000000000000002h
			/*0014*/ (byte)0xB0, 0x02,// mov al,2
			/*0016*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xE9, (byte)0xFF,// xbegin 8000000000000004h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
			0x0006,
			0x0008,
			0x000D,
			0x000F,
			0x0014,
			0x0016,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void xbegin_fwd_rel32() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, 0x11, 0x00,// xbegin 8000000000000018h
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ (byte)0xC7, (byte)0xF8, 0x09, 0x00, 0x00, 0x00,// xbegin 8000000000000018h
			/*000F*/ (byte)0xB0, 0x02,// mov al,2
			/*0011*/ 0x48, (byte)0xC7, (byte)0xF8, 0x00, 0x00, 0x00, 0x00,// xbegin 8000000000000018h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xC7, (byte)0xF8, 0x10, 0x00, 0x01, 0x00,// xbegin 8000000000000018h
			/*0008*/ (byte)0xB0, 0x01,// mov al,1
			/*000A*/ (byte)0xC7, (byte)0xF8, 0x08, 0x00, 0x01, 0x00,// xbegin 8000000000000018h
			/*0010*/ (byte)0xB0, 0x02,// mov al,2
			/*0012*/ (byte)0xC7, (byte)0xF8, 0x00, 0x00, 0x01, 0x00,// xbegin 8000000000000018h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0008,
			0x000A,
			0x0010,
			0x0012,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		final long origRip = 0x8000000000000000L;
		encodeBase(bitness, origRip, originalData, origRip - 0x10000, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void xbegin_bwd_rel32() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xC7, (byte)0xF8, (byte)0xF8, (byte)0xFF,// xbegin 7FFFFFFFFFFFFFFFh
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ (byte)0xC7, (byte)0xF8, (byte)0xF0, (byte)0xFF, (byte)0xFF, (byte)0xFF,// xbegin 7FFFFFFFFFFFFFFFh
			/*000F*/ (byte)0xB0, 0x02,// mov al,2
			/*0011*/ 0x48, (byte)0xC7, (byte)0xF8, (byte)0xE7, (byte)0xFF, (byte)0xFF, (byte)0xFF,// xbegin 7FFFFFFFFFFFFFFFh
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xC7, (byte)0xF8, (byte)0xF7, (byte)0xFF, (byte)0xFE, (byte)0xFF,// xbegin 7FFFFFFFFFFFFFFFh
			/*0008*/ (byte)0xB0, 0x01,// mov al,1
			/*000A*/ (byte)0xC7, (byte)0xF8, (byte)0xEF, (byte)0xFF, (byte)0xFE, (byte)0xFF,// xbegin 7FFFFFFFFFFFFFFFh
			/*0010*/ (byte)0xB0, 0x02,// mov al,2
			/*0012*/ (byte)0xC7, (byte)0xF8, (byte)0xE7, (byte)0xFF, (byte)0xFE, (byte)0xFF,// xbegin 7FFFFFFFFFFFFFFFh
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0008,
			0x000A,
			0x0010,
			0x0012,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		final long origRip = 0x8000000000000000L;
		encodeBase(bitness, origRip, originalData, origRip + 0x10000, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}
}
