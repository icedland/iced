// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import org.junit.jupiter.api.*;
import com.github.icedland.iced.x86.dec.DecoderOptions;

final class BlockEncoder64_jmp_Tests extends BlockEncoderTests {
	static final int bitness = 64;
	static final long origRip = 0x8000L;
	static final long newRip = 0x8000000000000000L;

	@Test
	void jmp_fwd() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x09,// jmp short 000000000000800Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
			/*000D*/ (byte)0x90,// nop
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x06,// jmp short 800000000000000Ah
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xEB, 0x02,// jmp short 800000000000000Ah
			/*0008*/ (byte)0xB0, 0x02,// mov al,2
			/*000A*/ (byte)0x90,// nop
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
			0x0006,
			0x0008,
			0x000A,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void jmp_bwd() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0x90,// nop
			/*0001*/ (byte)0xB0, 0x00,// mov al,0
			/*0003*/ (byte)0xEB, (byte)0xFB,// jmp short 0000000000008000h
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ (byte)0xE9, (byte)0xF4, (byte)0xFF, (byte)0xFF, (byte)0xFF,// jmp near ptr 0000000000008000h
			/*000C*/ (byte)0xB0, 0x02,// mov al,2
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0x90,// nop
			/*0001*/ (byte)0xB0, 0x00,// mov al,0
			/*0003*/ (byte)0xEB, (byte)0xFB,// jmp short 8000000000000000h
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ (byte)0xEB, (byte)0xF7,// jmp short 8000000000000000h
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0001,
			0x0003,
			0x0005,
			0x0007,
			0x0009,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void jmp_other_short_os() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xEB, 0x08,// jmp short 800Dh
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ 0x66, (byte)0xE9, 0x02, 0x00,// jmp near ptr 800Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xEB, 0x09,// jmp short 800Dh
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ 0x66, (byte)0xEB, 0x04,// jmp short 800Dh
			/*000A*/ (byte)0xB0, 0x02,// mov al,2
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0005,
			0x0007,
			0x000A,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip - 1, newData, options, DECODER_OPTIONS | DecoderOptions.AMD, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void jmp_other_near_os() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xEB, 0x08,// jmp short 800Dh
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ 0x66, (byte)0xE9, 0x02, 0x00,// jmp near ptr 800Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xE9, 0x07, (byte)0xF0,// jmp near ptr 800Dh
			/*0006*/ (byte)0xB0, 0x01,// mov al,1
			/*0008*/ 0x66, (byte)0xE9, 0x01, (byte)0xF0,// jmp near ptr 800Dh
			/*000C*/ (byte)0xB0, 0x02,// mov al,2
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0006,
			0x0008,
			0x000C,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip + 0x1000, newData, options, DECODER_OPTIONS | DecoderOptions.AMD, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void jmp_other_short() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x09,// jmp short 000000000000800Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x0A,// jmp short 000000000000800Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xEB, 0x06,// jmp short 000000000000800Dh
			/*0008*/ (byte)0xB0, 0x02,// mov al,2
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
			0x0006,
			0x0008,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip - 1, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void jmp_other_near() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x09,// jmp short 000000000000800Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xE9, 0x06, (byte)0xF0, (byte)0xFF, (byte)0xFF,// jmp near ptr 000000000000800Dh
			/*0007*/ (byte)0xB0, 0x01,// mov al,1
			/*0009*/ (byte)0xE9, (byte)0xFF, (byte)0xEF, (byte)0xFF, (byte)0xFF,// jmp near ptr 000000000000800Dh
			/*000E*/ (byte)0xB0, 0x02,// mov al,2
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0007,
			0x0009,
			0x000E,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip + 0x1000, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void jmp_other_long() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x09,// jmp short 123456789ABCDE0Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xE9, 0x03, 0x00, 0x00, 0x00,// jmp near ptr 123456789ABCDE0Eh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xFF, 0x25, 0x10, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000018h]
			/*0008*/ (byte)0xB0, 0x01,// mov al,1
			/*000A*/ (byte)0xFF, 0x25, 0x10, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000020h]
			/*0010*/ (byte)0xB0, 0x02,// mov al,2
			/*0012*/ (byte)0xCC, (byte)0xCC, (byte)0xCC, (byte)0xCC, (byte)0xCC, (byte)0xCC,
			/*0018*/ 0x0D, (byte)0xDE, (byte)0xBC, (byte)0x9A, 0x78, 0x56, 0x34, 0x12,
			/*0020*/ 0x0E, (byte)0xDE, (byte)0xBC, (byte)0x9A, 0x78, 0x56, 0x34, 0x12,
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0xFFFF_FFFF,
			0x0008,
			0xFFFF_FFFF,
			0x0010,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[] {
			new RelocInfo(RelocKind.OFFSET64, 0x8000000000000018L),
			new RelocInfo(RelocKind.OFFSET64, 0x8000000000000020L),
		};
		final int options = BlockEncoderOptions.NONE;
		final long origRip = 0x123456789ABCDE00L;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void jmp_fwd_no_opt() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x09,// jmp short 000000000000800Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
			/*000D*/ (byte)0x90,// nop
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xEB, 0x09,// jmp short 000000000000800Dh
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ (byte)0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
			/*000D*/ (byte)0x90,// nop
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
		final int options = BlockEncoderOptions.DONT_FIX_BRANCHES;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}
}
