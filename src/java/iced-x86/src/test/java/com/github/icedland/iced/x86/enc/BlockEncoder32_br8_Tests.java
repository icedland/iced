// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import org.junit.jupiter.api.*;

final class BlockEncoder32_br8_Tests extends BlockEncoderTests {
	static final int bitness = 32;
	static final long origRip = 0x8000L;
	static final long newRip = 0x80000000L;

	@Test
	void br8_fwd() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xE2, 0x22,// loop 00008026h
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ 0x67, (byte)0xE2, 0x1D,// loop 00008026h
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ (byte)0xE1, 0x19,// loope 00008026h
			/*000D*/ (byte)0xB0, 0x03,// mov al,3
			/*000F*/ 0x67, (byte)0xE1, 0x14,// loope 00008026h
			/*0012*/ (byte)0xB0, 0x04,// mov al,4
			/*0014*/ (byte)0xE0, 0x10,// loopne 00008026h
			/*0016*/ (byte)0xB0, 0x05,// mov al,5
			/*0018*/ 0x67, (byte)0xE0, 0x0B,// loopne 00008026h
			/*001B*/ (byte)0xB0, 0x06,// mov al,6
			/*001D*/ 0x67, (byte)0xE3, 0x06,// jcxz 00008026h
			/*0020*/ (byte)0xB0, 0x07,// mov al,7
			/*0022*/ (byte)0xE3, 0x02,// jecxz 00008026h
			/*0024*/ (byte)0xB0, 0x08,// mov al,8
			/*0026*/ (byte)0x90,// nop
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xE2, 0x22,// loop 80000026h
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ 0x67, (byte)0xE2, 0x1D,// loop 80000026h
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ (byte)0xE1, 0x19,// loope 80000026h
			/*000D*/ (byte)0xB0, 0x03,// mov al,3
			/*000F*/ 0x67, (byte)0xE1, 0x14,// loope 80000026h
			/*0012*/ (byte)0xB0, 0x04,// mov al,4
			/*0014*/ (byte)0xE0, 0x10,// loopne 80000026h
			/*0016*/ (byte)0xB0, 0x05,// mov al,5
			/*0018*/ 0x67, (byte)0xE0, 0x0B,// loopne 80000026h
			/*001B*/ (byte)0xB0, 0x06,// mov al,6
			/*001D*/ 0x67, (byte)0xE3, 0x06,// jcxz 80000026h
			/*0020*/ (byte)0xB0, 0x07,// mov al,7
			/*0022*/ (byte)0xE3, 0x02,// jecxz 80000026h
			/*0024*/ (byte)0xB0, 0x08,// mov al,8
			/*0026*/ (byte)0x90,// nop
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
			0x0006,
			0x0009,
			0x000B,
			0x000D,
			0x000F,
			0x0012,
			0x0014,
			0x0016,
			0x0018,
			0x001B,
			0x001D,
			0x0020,
			0x0022,
			0x0024,
			0x0026,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void br8_bwd() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0x90,// nop
			/*0001*/ (byte)0xB0, 0x00,// mov al,0
			/*0003*/ (byte)0xE2, (byte)0xFB,// loop 00008000h
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ 0x67, (byte)0xE2, (byte)0xF6,// loop 00008000h
			/*000A*/ (byte)0xB0, 0x02,// mov al,2
			/*000C*/ (byte)0xE1, (byte)0xF2,// loope 00008000h
			/*000E*/ (byte)0xB0, 0x03,// mov al,3
			/*0010*/ 0x67, (byte)0xE1, (byte)0xED,// loope 00008000h
			/*0013*/ (byte)0xB0, 0x04,// mov al,4
			/*0015*/ (byte)0xE0, (byte)0xE9,// loopne 00008000h
			/*0017*/ (byte)0xB0, 0x05,// mov al,5
			/*0019*/ 0x67, (byte)0xE0, (byte)0xE4,// loopne 00008000h
			/*001C*/ (byte)0xB0, 0x06,// mov al,6
			/*001E*/ 0x67, (byte)0xE3, (byte)0xDF,// jcxz 00008000h
			/*0021*/ (byte)0xB0, 0x07,// mov al,7
			/*0023*/ (byte)0xE3, (byte)0xDB,// jecxz 00008000h
			/*0025*/ (byte)0xB0, 0x08,// mov al,8
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0x90,// nop
			/*0001*/ (byte)0xB0, 0x00,// mov al,0
			/*0003*/ (byte)0xE2, (byte)0xFB,// loop 80000000h
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ 0x67, (byte)0xE2, (byte)0xF6,// loop 80000000h
			/*000A*/ (byte)0xB0, 0x02,// mov al,2
			/*000C*/ (byte)0xE1, (byte)0xF2,// loope 80000000h
			/*000E*/ (byte)0xB0, 0x03,// mov al,3
			/*0010*/ 0x67, (byte)0xE1, (byte)0xED,// loope 80000000h
			/*0013*/ (byte)0xB0, 0x04,// mov al,4
			/*0015*/ (byte)0xE0, (byte)0xE9,// loopne 80000000h
			/*0017*/ (byte)0xB0, 0x05,// mov al,5
			/*0019*/ 0x67, (byte)0xE0, (byte)0xE4,// loopne 80000000h
			/*001C*/ (byte)0xB0, 0x06,// mov al,6
			/*001E*/ 0x67, (byte)0xE3, (byte)0xDF,// jcxz 80000000h
			/*0021*/ (byte)0xB0, 0x07,// mov al,7
			/*0023*/ (byte)0xE3, (byte)0xDB,// jecxz 80000000h
			/*0025*/ (byte)0xB0, 0x08,// mov al,8
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0001,
			0x0003,
			0x0005,
			0x0007,
			0x000A,
			0x000C,
			0x000E,
			0x0010,
			0x0013,
			0x0015,
			0x0017,
			0x0019,
			0x001C,
			0x001E,
			0x0021,
			0x0023,
			0x0025,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void br8_fwd_os() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xE2, 0x29,// loopw 802Eh
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ 0x66, 0x67, (byte)0xE2, 0x23,// loopw 802Eh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
			/*000D*/ 0x66, (byte)0xE1, 0x1E,// loopew 802Eh
			/*0010*/ (byte)0xB0, 0x03,// mov al,3
			/*0012*/ 0x66, 0x67, (byte)0xE1, 0x18,// loopew 802Eh
			/*0016*/ (byte)0xB0, 0x04,// mov al,4
			/*0018*/ 0x66, (byte)0xE0, 0x13,// loopnew 802Eh
			/*001B*/ (byte)0xB0, 0x05,// mov al,5
			/*001D*/ 0x66, 0x67, (byte)0xE0, 0x0D,// loopnew 802Eh
			/*0021*/ (byte)0xB0, 0x06,// mov al,6
			/*0023*/ 0x66, 0x67, (byte)0xE3, 0x07,// jcxz 802Eh
			/*0027*/ (byte)0xB0, 0x07,// mov al,7
			/*0029*/ 0x66, (byte)0xE3, 0x02,// jecxz 802Eh
			/*002C*/ (byte)0xB0, 0x08,// mov al,8
			/*002E*/ (byte)0x90,// nop
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ 0x66, (byte)0xE2, 0x29,// loopw 802Dh
			/*0005*/ (byte)0xB0, 0x01,// mov al,1
			/*0007*/ 0x66, 0x67, (byte)0xE2, 0x23,// loopw 802Dh
			/*000B*/ (byte)0xB0, 0x02,// mov al,2
			/*000D*/ 0x66, (byte)0xE1, 0x1E,// loopew 802Dh
			/*0010*/ (byte)0xB0, 0x03,// mov al,3
			/*0012*/ 0x66, 0x67, (byte)0xE1, 0x18,// loopew 802Dh
			/*0016*/ (byte)0xB0, 0x04,// mov al,4
			/*0018*/ 0x66, (byte)0xE0, 0x13,// loopnew 802Dh
			/*001B*/ (byte)0xB0, 0x05,// mov al,5
			/*001D*/ 0x66, 0x67, (byte)0xE0, 0x0D,// loopnew 802Dh
			/*0021*/ (byte)0xB0, 0x06,// mov al,6
			/*0023*/ 0x66, 0x67, (byte)0xE3, 0x07,// jcxz 802Dh
			/*0027*/ (byte)0xB0, 0x07,// mov al,7
			/*0029*/ 0x66, (byte)0xE3, 0x02,// jecxz 802Dh
			/*002C*/ (byte)0xB0, 0x08,// mov al,8
			/*002E*/ (byte)0x90,// nop
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0005,
			0x0007,
			0x000B,
			0x000D,
			0x0010,
			0x0012,
			0x0016,
			0x0018,
			0x001B,
			0x001D,
			0x0021,
			0x0023,
			0x0027,
			0x0029,
			0x002C,
			0x002E,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip - 1, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void br8_short_other_short() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xE2, 0x22,// loop 00008026h
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ 0x67, (byte)0xE2, 0x1D,// loop 00008026h
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ (byte)0xE1, 0x19,// loope 00008026h
			/*000D*/ (byte)0xB0, 0x03,// mov al,3
			/*000F*/ 0x67, (byte)0xE1, 0x14,// loope 00008026h
			/*0012*/ (byte)0xB0, 0x04,// mov al,4
			/*0014*/ (byte)0xE0, 0x10,// loopne 00008026h
			/*0016*/ (byte)0xB0, 0x05,// mov al,5
			/*0018*/ 0x67, (byte)0xE0, 0x0B,// loopne 00008026h
			/*001B*/ (byte)0xB0, 0x06,// mov al,6
			/*001D*/ 0x67, (byte)0xE3, 0x06,// jcxz 00008026h
			/*0020*/ (byte)0xB0, 0x07,// mov al,7
			/*0022*/ (byte)0xE3, 0x02,// jecxz 00008026h
			/*0024*/ (byte)0xB0, 0x08,// mov al,8
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xE2, 0x23,// loop 00008026h
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ 0x67, (byte)0xE2, 0x1E,// loop 00008026h
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ (byte)0xE1, 0x1A,// loope 00008026h
			/*000D*/ (byte)0xB0, 0x03,// mov al,3
			/*000F*/ 0x67, (byte)0xE1, 0x15,// loope 00008026h
			/*0012*/ (byte)0xB0, 0x04,// mov al,4
			/*0014*/ (byte)0xE0, 0x11,// loopne 00008026h
			/*0016*/ (byte)0xB0, 0x05,// mov al,5
			/*0018*/ 0x67, (byte)0xE0, 0x0C,// loopne 00008026h
			/*001B*/ (byte)0xB0, 0x06,// mov al,6
			/*001D*/ 0x67, (byte)0xE3, 0x07,// jcxz 00008026h
			/*0020*/ (byte)0xB0, 0x07,// mov al,7
			/*0022*/ (byte)0xE3, 0x03,// jecxz 00008026h
			/*0024*/ (byte)0xB0, 0x08,// mov al,8
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
			0x0006,
			0x0009,
			0x000B,
			0x000D,
			0x000F,
			0x0012,
			0x0014,
			0x0016,
			0x0018,
			0x001B,
			0x001D,
			0x0020,
			0x0022,
			0x0024,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip - 1, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void br8_short_other_near() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xE2, 0x22,// loop 00008026h
			/*0004*/ (byte)0xB0, 0x01,// mov al,1
			/*0006*/ 0x67, (byte)0xE2, 0x1E,// loop 00008027h
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ (byte)0xE1, 0x1B,// loope 00008028h
			/*000D*/ (byte)0xB0, 0x03,// mov al,3
			/*000F*/ 0x67, (byte)0xE1, 0x17,// loope 00008029h
			/*0012*/ (byte)0xB0, 0x04,// mov al,4
			/*0014*/ (byte)0xE0, 0x14,// loopne 0000802Ah
			/*0016*/ (byte)0xB0, 0x05,// mov al,5
			/*0018*/ 0x67, (byte)0xE0, 0x10,// loopne 0000802Bh
			/*001B*/ (byte)0xB0, 0x06,// mov al,6
			/*001D*/ 0x67, (byte)0xE3, 0x0C,// jcxz 0000802Ch
			/*0020*/ (byte)0xB0, 0x07,// mov al,7
			/*0022*/ (byte)0xE3, 0x09,// jecxz 0000802Dh
			/*0024*/ (byte)0xB0, 0x08,// mov al,8
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x00,// mov al,0
			/*0002*/ (byte)0xE2, 0x02,// loop 00009006h
			/*0004*/ (byte)0xEB, 0x05,// jmp short 0000900Bh
			/*0006*/ (byte)0xE9, 0x1B, (byte)0xF0, (byte)0xFF, (byte)0xFF,// jmp near ptr 00008026h
			/*000B*/ (byte)0xB0, 0x01,// mov al,1
			/*000D*/ 0x67, (byte)0xE2, 0x02,// loop 00009012h
			/*0010*/ (byte)0xEB, 0x05,// jmp short 00009017h
			/*0012*/ (byte)0xE9, 0x10, (byte)0xF0, (byte)0xFF, (byte)0xFF,// jmp near ptr 00008027h
			/*0017*/ (byte)0xB0, 0x02,// mov al,2
			/*0019*/ (byte)0xE1, 0x02,// loope 0000901Dh
			/*001B*/ (byte)0xEB, 0x05,// jmp short 00009022h
			/*001D*/ (byte)0xE9, 0x06, (byte)0xF0, (byte)0xFF, (byte)0xFF,// jmp near ptr 00008028h
			/*0022*/ (byte)0xB0, 0x03,// mov al,3
			/*0024*/ 0x67, (byte)0xE1, 0x02,// loope 00009029h
			/*0027*/ (byte)0xEB, 0x05,// jmp short 0000902Eh
			/*0029*/ (byte)0xE9, (byte)0xFB, (byte)0xEF, (byte)0xFF, (byte)0xFF,// jmp near ptr 00008029h
			/*002E*/ (byte)0xB0, 0x04,// mov al,4
			/*0030*/ (byte)0xE0, 0x02,// loopne 00009034h
			/*0032*/ (byte)0xEB, 0x05,// jmp short 00009039h
			/*0034*/ (byte)0xE9, (byte)0xF1, (byte)0xEF, (byte)0xFF, (byte)0xFF,// jmp near ptr 0000802Ah
			/*0039*/ (byte)0xB0, 0x05,// mov al,5
			/*003B*/ 0x67, (byte)0xE0, 0x02,// loopne 00009040h
			/*003E*/ (byte)0xEB, 0x05,// jmp short 00009045h
			/*0040*/ (byte)0xE9, (byte)0xE6, (byte)0xEF, (byte)0xFF, (byte)0xFF,// jmp near ptr 0000802Bh
			/*0045*/ (byte)0xB0, 0x06,// mov al,6
			/*0047*/ 0x67, (byte)0xE3, 0x02,// jcxz 0000904Ch
			/*004A*/ (byte)0xEB, 0x05,// jmp short 00009051h
			/*004C*/ (byte)0xE9, (byte)0xDB, (byte)0xEF, (byte)0xFF, (byte)0xFF,// jmp near ptr 0000802Ch
			/*0051*/ (byte)0xB0, 0x07,// mov al,7
			/*0053*/ (byte)0xE3, 0x02,// jecxz 00009057h
			/*0055*/ (byte)0xEB, 0x05,// jmp short 0000905Ch
			/*0057*/ (byte)0xE9, (byte)0xD1, (byte)0xEF, (byte)0xFF, (byte)0xFF,// jmp near ptr 0000802Dh
			/*005C*/ (byte)0xB0, 0x08,// mov al,8
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0xFFFF_FFFF,
			0x000B,
			0xFFFF_FFFF,
			0x0017,
			0xFFFF_FFFF,
			0x0022,
			0xFFFF_FFFF,
			0x002E,
			0xFFFF_FFFF,
			0x0039,
			0xFFFF_FFFF,
			0x0045,
			0xFFFF_FFFF,
			0x0051,
			0xFFFF_FFFF,
			0x005C,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip + 0x1000, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void br8_same_br() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xE2, (byte)0xFE,// loop 8000h
			/*0002*/ (byte)0xE2, (byte)0xFC,// loop 8000h
			/*0004*/ (byte)0xE2, (byte)0xFA,// loop 8000h
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xE2, (byte)0xFE,// loop 8000h
			/*0002*/ (byte)0xE2, (byte)0xFC,// loop 8000h
			/*0004*/ (byte)0xE2, (byte)0xFA,// loop 8000h
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0004,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}
}
