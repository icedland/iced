// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import org.junit.jupiter.api.*;

final class BlockEncoder64_iprel_Tests extends BlockEncoderTests {
	static final int bitness = 64;
	static final long origRip = 0x8000L;
	static final long newRip = 0x8000000000000000L;

	@Test
	void ipRel_fwd_bwd() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x01,// mov al,1
			/*0002*/ 0x48, (byte)0x8B, 0x05, 0x1F, 0x00, 0x00, 0x00,// mov rax,[8028h]
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ 0x48, (byte)0x8B, 0x05, (byte)0xEE, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[8000h]
			/*0012*/ (byte)0xB0, 0x03,// mov al,3
			/*0014*/ 0x67, 0x48, (byte)0x8B, 0x05, 0x0C, 0x00, 0x00, 0x00,// mov rax,[8028h]
			/*001C*/ (byte)0xB0, 0x04,// mov al,4
			/*001E*/ 0x67, 0x48, (byte)0x8B, 0x05, (byte)0xDA, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[8000h]
			/*0026*/ (byte)0xB0, 0x05,// mov al,5
			/*0028*/ (byte)0xB0, 0x06,// mov al,6
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x01,// mov al,1
			/*0002*/ 0x48, (byte)0x8B, 0x05, 0x1D, 0x00, 0x00, 0x00,// mov rax,[8000000000000026h]
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ 0x48, (byte)0x8B, 0x05, (byte)0xEE, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[8000000000000000h]
			/*0012*/ (byte)0xB0, 0x03,// mov al,3
			/*0014*/ 0x48, (byte)0x8B, 0x05, 0x0B, 0x00, 0x00, 0x00,// mov rax,[8000000000000026h]
			/*001B*/ (byte)0xB0, 0x04,// mov al,4
			/*001D*/ 0x48, (byte)0x8B, 0x05, (byte)0xDC, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[8000000000000000h]
			/*0024*/ (byte)0xB0, 0x05,// mov al,5
			/*0026*/ (byte)0xB0, 0x06,// mov al,6
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0009,
			0x000B,
			0x0012,
			0x0014,
			0x001B,
			0x001D,
			0x0024,
			0x0026,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void ipRel_fwd_bwd_other_near() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x01,// mov al,1
			/*0002*/ 0x48, (byte)0x8B, 0x05, 0x1F, 0x00, 0x00, 0x00,// mov rax,[8028h]
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ 0x48, (byte)0x8B, 0x05, (byte)0xED, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[7FFFh]
			/*0012*/ (byte)0xB0, 0x03,// mov al,3
			/*0014*/ 0x67, 0x48, (byte)0x8B, 0x05, 0x0C, 0x00, 0x00, 0x00,// mov rax,[8028h]
			/*001C*/ (byte)0xB0, 0x04,// mov al,4
			/*001E*/ 0x67, 0x48, (byte)0x8B, 0x05, (byte)0xD9, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[7FFFh]
			/*0026*/ (byte)0xB0, 0x05,// mov al,5
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x01,// mov al,1
			/*0002*/ 0x48, (byte)0x8B, 0x05, 0x1F, (byte)0xF0, (byte)0xFF, (byte)0xFF,// mov rax,[8028h]
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ 0x48, (byte)0x8B, 0x05, (byte)0xED, (byte)0xEF, (byte)0xFF, (byte)0xFF,// mov rax,[7FFFh]
			/*0012*/ (byte)0xB0, 0x03,// mov al,3
			/*0014*/ 0x48, (byte)0x8B, 0x05, 0x0D, (byte)0xF0, (byte)0xFF, (byte)0xFF,// mov rax,[8028h]
			/*001B*/ (byte)0xB0, 0x04,// mov al,4
			/*001D*/ 0x48, (byte)0x8B, 0x05, (byte)0xDB, (byte)0xEF, (byte)0xFF, (byte)0xFF,// mov rax,[7FFFh]
			/*0024*/ (byte)0xB0, 0x05,// mov al,5
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x0009,
			0x000B,
			0x0012,
			0x0014,
			0x001B,
			0x001D,
			0x0024,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, origRip + 0x1000, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}

	@Test
	void ipRel_fwd_bwd_other_long_low4GB() {
		byte[] originalData = new byte[] {
			/*0000*/ (byte)0xB0, 0x01,// mov al,1
			/*0002*/ 0x48, (byte)0x8B, 0x05, 0x1F, 0x00, 0x00, 0x00,// mov rax,[8028h]
			/*0009*/ (byte)0xB0, 0x02,// mov al,2
			/*000B*/ 0x48, (byte)0x8B, 0x05, (byte)0xED, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[7FFFh]
			/*0012*/ (byte)0xB0, 0x03,// mov al,3
			/*0014*/ 0x67, 0x48, (byte)0x8B, 0x05, 0x0C, 0x00, 0x00, 0x00,// mov rax,[8028h]
			/*001C*/ (byte)0xB0, 0x04,// mov al,4
			/*001E*/ 0x67, 0x48, (byte)0x8B, 0x05, (byte)0xD9, (byte)0xFF, (byte)0xFF, (byte)0xFF,// mov rax,[7FFFh]
			/*0026*/ (byte)0xB0, 0x05,// mov al,5
		};
		byte[] newData = new byte[] {
			/*0000*/ (byte)0xB0, 0x01,// mov al,1
			/*0002*/ 0x67, 0x48, (byte)0x8B, 0x05, 0x1E, (byte)0x80, 0x00, 0x00,// mov rax,[8028h]
			/*000A*/ (byte)0xB0, 0x02,// mov al,2
			/*000C*/ 0x67, 0x48, (byte)0x8B, 0x05, (byte)0xEB, 0x7F, 0x00, 0x00,// mov rax,[7FFFh]
			/*0014*/ (byte)0xB0, 0x03,// mov al,3
			/*0016*/ 0x67, 0x48, (byte)0x8B, 0x05, 0x0A, (byte)0x80, 0x00, 0x00,// mov rax,[8028h]
			/*001E*/ (byte)0xB0, 0x04,// mov al,4
			/*0020*/ 0x67, 0x48, (byte)0x8B, 0x05, (byte)0xD7, 0x7F, 0x00, 0x00,// mov rax,[7FFFh]
			/*0028*/ (byte)0xB0, 0x05,// mov al,5
		};
		int[] expectedInstructionOffsets = new int[] {
			0x0000,
			0x0002,
			0x000A,
			0x000C,
			0x0014,
			0x0016,
			0x001E,
			0x0020,
			0x0028,
		};
		RelocInfo[] expectedRelocInfos = new RelocInfo[0];
		final int options = BlockEncoderOptions.NONE;
		encodeBase(bitness, origRip, originalData, newRip, newData, options, DECODER_OPTIONS, expectedInstructionOffsets, expectedRelocInfos);
	}
}
