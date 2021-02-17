// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest32_call : BlockEncoderTest {
		const int bitness = 32;
		const ulong origRip = 0x8000;
		const ulong newRip = 0x80000000;

		[Fact]
		void Call_near_fwd() {
			var originalData = new byte[] {
				/*0000*/ 0xE8, 0x07, 0x00, 0x00, 0x00,// call 0000800Ch
				/*0005*/ 0xB0, 0x00,// mov al,0
				/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
				/*000C*/ 0x90,// nop
			};
			var newData = new byte[] {
				/*0000*/ 0xE8, 0x07, 0x00, 0x00, 0x00,// call 8000000Ch
				/*0005*/ 0xB0, 0x00,// mov al,0
				/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
				/*000C*/ 0x90,// nop
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0005,
				0x0007,
				0x000C,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Call_near_bwd() {
			var originalData = new byte[] {
				/*0000*/ 0x90,// nop
				/*0001*/ 0xE8, 0xFA, 0xFF, 0xFF, 0xFF,// call 00008000h
				/*0006*/ 0xB0, 0x00,// mov al,0
				/*0008*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var newData = new byte[] {
				/*0000*/ 0x90,// nop
				/*0001*/ 0xE8, 0xFA, 0xFF, 0xFF, 0xFF,// call 80000000h
				/*0006*/ 0xB0, 0x00,// mov al,0
				/*0008*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0001,
				0x0006,
				0x0008,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Call_near_other_near() {
			var originalData = new byte[] {
				/*0000*/ 0xE8, 0x07, 0x00, 0x00, 0x00,// call 0000800Ch
				/*0005*/ 0xB0, 0x00,// mov al,0
				/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var newData = new byte[] {
				/*0000*/ 0xE8, 0x08, 0x00, 0x00, 0x00,// call 0000800Ch
				/*0005*/ 0xB0, 0x00,// mov al,0
				/*0007*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0005,
				0x0007,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, origRip - 1, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Call_near_other_near_os() {
			var originalData = new byte[] {
				/*0000*/ 0x66, 0xE8, 0x07, 0x00,// call 800Bh
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var newData = new byte[] {
				/*0000*/ 0x66, 0xE8, 0x07, 0x80,// call 800Bh
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0004,
				0x0006,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}
	}
}
#endif
