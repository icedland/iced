// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest16_call : BlockEncoderTest {
		const int bitness = 16;
		const ulong origRip = 0x8000;
		const ulong newRip = 0xF000;

		[Fact]
		void Call_near_fwd() {
			var originalData = new byte[] {
				/*0000*/ 0xE8, 0x08, 0x00,// call 800Bh
				/*0003*/ 0xB0, 0x00,// mov al,0
				/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
				/*000B*/ 0x90,// nop
			};
			var newData = new byte[] {
				/*0000*/ 0xE8, 0x08, 0x00,// call 0F00Bh
				/*0003*/ 0xB0, 0x00,// mov al,0
				/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
				/*000B*/ 0x90,// nop
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0003,
				0x0005,
				0x000B,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Call_near_bwd() {
			var originalData = new byte[] {
				/*0000*/ 0x90,// nop
				/*0001*/ 0xE8, 0xFC, 0xFF,// call 8000h
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var newData = new byte[] {
				/*0000*/ 0x90,// nop
				/*0001*/ 0xE8, 0xFC, 0xFF,// call 0F000h
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0001,
				0x0004,
				0x0006,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Call_near_other_near() {
			var originalData = new byte[] {
				/*0000*/ 0xE8, 0x08, 0x00,// call 800Bh
				/*0003*/ 0xB0, 0x00,// mov al,0
				/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var newData = new byte[] {
				/*0000*/ 0xE8, 0x09, 0x00,// call 800Bh
				/*0003*/ 0xB0, 0x00,// mov al,0
				/*0005*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0003,
				0x0005,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, origRip - 1, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Call_near_other_near_os() {
			var originalData = new byte[] {
				/*0000*/ 0x66, 0xE8, 0x08, 0x00, 0x00, 0x00,// call 0000800Eh
				/*0006*/ 0xB0, 0x00,// mov al,0
				/*0008*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var newData = new byte[] {
				/*0000*/ 0x66, 0xE8, 0x08, 0x90, 0xFF, 0xFF,// call 0000800Eh
				/*0006*/ 0xB0, 0x00,// mov al,0
				/*0008*/ 0x66, 0xB8, 0x78, 0x56, 0x34, 0x12,// mov eax,12345678h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0006,
				0x0008,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}
	}
}
#endif
