// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest16_xbegin : BlockEncoderTest {
		const int bitness = 16;
		const ulong origRip = 0x8000;
		const ulong newRip = 0xF000;

		[Fact]
		void Xbegin_fwd_rel16() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xC7, 0xF8, 0x06, 0x00, 0x00, 0x00,// xbegin 0000800Fh
				/*0009*/ 0xB0, 0x01,// mov al,1
				/*000B*/ 0xC7, 0xF8, 0x02, 0x00,// xbegin 00008011h
				/*000F*/ 0xB0, 0x02,// mov al,2
				/*0011*/ 0xB0, 0x03,// mov al,3
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xC7, 0xF8, 0x06, 0x00,// xbegin 0000F00Ch
				/*0006*/ 0xB0, 0x01,// mov al,1
				/*0008*/ 0xC7, 0xF8, 0x02, 0x00,// xbegin 0000F00Eh
				/*000C*/ 0xB0, 0x02,// mov al,2
				/*000E*/ 0xB0, 0x03,// mov al,3
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0006,
				0x0008,
				0x000C,
				0x000E,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Xbegin_bwd_rel16() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x02,// mov al,2
				/*0002*/ 0xB0, 0x03,// mov al,3
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0x66, 0xC7, 0xF8, 0xF3, 0xFF, 0xFF, 0xFF,// xbegin 00008000h
				/*000D*/ 0xB0, 0x01,// mov al,1
				/*000F*/ 0xC7, 0xF8, 0xEF, 0xFF,// xbegin 00008002h
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x02,// mov al,2
				/*0002*/ 0xB0, 0x03,// mov al,3
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0xC7, 0xF8, 0xF6, 0xFF,// xbegin 0000F000h
				/*000A*/ 0xB0, 0x01,// mov al,1
				/*000C*/ 0xC7, 0xF8, 0xF2, 0xFF,// xbegin 0000F002h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0004,
				0x0006,
				0x000A,
				0x000C,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}
	}
}
#endif
