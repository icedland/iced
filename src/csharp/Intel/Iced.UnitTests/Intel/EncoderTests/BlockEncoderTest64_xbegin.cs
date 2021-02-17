// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest64_xbegin : BlockEncoderTest {
		const int bitness = 64;
		const ulong origRip = 0x8000;
		const ulong newRip = 0x8000000000000000;

		[Fact]
		void Xbegin_fwd_rel16() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xC7, 0xF8, 0x11, 0x00,// xbegin 0000000000008018h
				/*0007*/ 0xB0, 0x01,// mov al,1
				/*0009*/ 0xC7, 0xF8, 0x0B, 0x00, 0x00, 0x00,// xbegin 000000000000801Ah
				/*000F*/ 0xB0, 0x02,// mov al,2
				/*0011*/ 0x48, 0xC7, 0xF8, 0x04, 0x00, 0x00, 0x00,// xbegin 000000000000801Ch
				/*0018*/ 0xB0, 0x03,// mov al,3
				/*001A*/ 0xB0, 0x04,// mov al,4
				/*001C*/ 0xB0, 0x05,// mov al,5
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xC7, 0xF8, 0x0E, 0x00,// xbegin 8000000000000015h
				/*0007*/ 0xB0, 0x01,// mov al,1
				/*0009*/ 0x66, 0xC7, 0xF8, 0x09, 0x00,// xbegin 8000000000000017h
				/*000E*/ 0xB0, 0x02,// mov al,2
				/*0010*/ 0x66, 0xC7, 0xF8, 0x04, 0x00,// xbegin 8000000000000019h
				/*0015*/ 0xB0, 0x03,// mov al,3
				/*0017*/ 0xB0, 0x04,// mov al,4
				/*0019*/ 0xB0, 0x05,// mov al,5
			};
			var expectedInstructionOffsets = new uint[] {
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
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Xbegin_bwd_rel16() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x03,// mov al,3
				/*0002*/ 0xB0, 0x04,// mov al,4
				/*0004*/ 0xB0, 0x05,// mov al,5
				/*0006*/ 0xB0, 0x00,// mov al,0
				/*0008*/ 0x66, 0xC7, 0xF8, 0xF3, 0xFF,// xbegin 0000000000008000h
				/*000D*/ 0xB0, 0x01,// mov al,1
				/*000F*/ 0xC7, 0xF8, 0xED, 0xFF, 0xFF, 0xFF,// xbegin 0000000000008002h
				/*0015*/ 0xB0, 0x02,// mov al,2
				/*0017*/ 0x48, 0xC7, 0xF8, 0xE6, 0xFF, 0xFF, 0xFF,// xbegin 0000000000008004h
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x03,// mov al,3
				/*0002*/ 0xB0, 0x04,// mov al,4
				/*0004*/ 0xB0, 0x05,// mov al,5
				/*0006*/ 0xB0, 0x00,// mov al,0
				/*0008*/ 0x66, 0xC7, 0xF8, 0xF3, 0xFF,// xbegin 8000000000000000h
				/*000D*/ 0xB0, 0x01,// mov al,1
				/*000F*/ 0x66, 0xC7, 0xF8, 0xEE, 0xFF,// xbegin 8000000000000002h
				/*0014*/ 0xB0, 0x02,// mov al,2
				/*0016*/ 0x66, 0xC7, 0xF8, 0xE9, 0xFF,// xbegin 8000000000000004h
			};
			var expectedInstructionOffsets = new uint[] {
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
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Xbegin_fwd_rel32() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xC7, 0xF8, 0x11, 0x00,// xbegin 8000000000000018h
				/*0007*/ 0xB0, 0x01,// mov al,1
				/*0009*/ 0xC7, 0xF8, 0x09, 0x00, 0x00, 0x00,// xbegin 8000000000000018h
				/*000F*/ 0xB0, 0x02,// mov al,2
				/*0011*/ 0x48, 0xC7, 0xF8, 0x00, 0x00, 0x00, 0x00,// xbegin 8000000000000018h
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xC7, 0xF8, 0x10, 0x00, 0x01, 0x00,// xbegin 8000000000000018h
				/*0008*/ 0xB0, 0x01,// mov al,1
				/*000A*/ 0xC7, 0xF8, 0x08, 0x00, 0x01, 0x00,// xbegin 8000000000000018h
				/*0010*/ 0xB0, 0x02,// mov al,2
				/*0012*/ 0xC7, 0xF8, 0x00, 0x00, 0x01, 0x00,// xbegin 8000000000000018h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0008,
				0x000A,
				0x0010,
				0x0012,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			const ulong origRip = 0x8000000000000000;
			EncodeBase(bitness, origRip, originalData, origRip - 0x10000, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Xbegin_bwd_rel32() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xC7, 0xF8, 0xF8, 0xFF,// xbegin 7FFFFFFFFFFFFFFFh
				/*0007*/ 0xB0, 0x01,// mov al,1
				/*0009*/ 0xC7, 0xF8, 0xF0, 0xFF, 0xFF, 0xFF,// xbegin 7FFFFFFFFFFFFFFFh
				/*000F*/ 0xB0, 0x02,// mov al,2
				/*0011*/ 0x48, 0xC7, 0xF8, 0xE7, 0xFF, 0xFF, 0xFF,// xbegin 7FFFFFFFFFFFFFFFh
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xC7, 0xF8, 0xF7, 0xFF, 0xFE, 0xFF,// xbegin 7FFFFFFFFFFFFFFFh
				/*0008*/ 0xB0, 0x01,// mov al,1
				/*000A*/ 0xC7, 0xF8, 0xEF, 0xFF, 0xFE, 0xFF,// xbegin 7FFFFFFFFFFFFFFFh
				/*0010*/ 0xB0, 0x02,// mov al,2
				/*0012*/ 0xC7, 0xF8, 0xE7, 0xFF, 0xFE, 0xFF,// xbegin 7FFFFFFFFFFFFFFFh
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0008,
				0x000A,
				0x0010,
				0x0012,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			const ulong origRip = 0x8000000000000000;
			EncodeBase(bitness, origRip, originalData, origRip + 0x10000, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}
	}
}
#endif
