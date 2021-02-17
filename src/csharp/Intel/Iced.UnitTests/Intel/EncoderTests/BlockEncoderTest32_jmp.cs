// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest32_jmp : BlockEncoderTest {
		const int bitness = 32;
		const ulong origRip = 0x8000;
		const ulong newRip = 0x80000000;

		[Fact]
		void Jmp_fwd() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 0000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 0000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
				/*000D*/ 0x90,// nop
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x06,// jmp short 8000000Ah
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xEB, 0x02,// jmp short 8000000Ah
				/*0008*/ 0xB0, 0x02,// mov al,2
				/*000A*/ 0x90,// nop
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0004,
				0x0006,
				0x0008,
				0x000A,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Jmp_bwd() {
			var originalData = new byte[] {
				/*0000*/ 0x90,// nop
				/*0001*/ 0xB0, 0x00,// mov al,0
				/*0003*/ 0xEB, 0xFB,// jmp short 00008000h
				/*0005*/ 0xB0, 0x01,// mov al,1
				/*0007*/ 0xE9, 0xF4, 0xFF, 0xFF, 0xFF,// jmp near ptr 00008000h
				/*000C*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0x90,// nop
				/*0001*/ 0xB0, 0x00,// mov al,0
				/*0003*/ 0xEB, 0xFB,// jmp short 80000000h
				/*0005*/ 0xB0, 0x01,// mov al,1
				/*0007*/ 0xEB, 0xF7,// jmp short 80000000h
				/*0009*/ 0xB0, 0x02,// mov al,2
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0001,
				0x0003,
				0x0005,
				0x0007,
				0x0009,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Jmp_other_short_os() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xEB, 0x08,// jmp short 800Dh
				/*0005*/ 0xB0, 0x01,// mov al,1
				/*0007*/ 0x66, 0xE9, 0x02, 0x00,// jmp near ptr 800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xEB, 0x09,// jmp short 800Dh
				/*0005*/ 0xB0, 0x01,// mov al,1
				/*0007*/ 0x66, 0xEB, 0x04,// jmp short 800Dh
				/*000A*/ 0xB0, 0x02,// mov al,2
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0005,
				0x0007,
				0x000A,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, origRip - 1, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Jmp_other_near_os() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xEB, 0x08,// jmp short 800Dh
				/*0005*/ 0xB0, 0x01,// mov al,1
				/*0007*/ 0x66, 0xE9, 0x02, 0x00,// jmp near ptr 800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xE9, 0x07, 0xF0,// jmp near ptr 800Dh
				/*0006*/ 0xB0, 0x01,// mov al,1
				/*0008*/ 0x66, 0xE9, 0x01, 0xF0,// jmp near ptr 800Dh
				/*000C*/ 0xB0, 0x02,// mov al,2
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0006,
				0x0008,
				0x000C,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, origRip + 0x1000, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Jmp_other_short() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 0000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 0000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x0A,// jmp short 0000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xEB, 0x06,// jmp short 0000800Dh
				/*0008*/ 0xB0, 0x02,// mov al,2
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0004,
				0x0006,
				0x0008,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, origRip - 1, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Jmp_other_near() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 0000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 0000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xE9, 0x06, 0xF0, 0xFF, 0xFF,// jmp near ptr 0000800Dh
				/*0007*/ 0xB0, 0x01,// mov al,1
				/*0009*/ 0xE9, 0xFF, 0xEF, 0xFF, 0xFF,// jmp near ptr 0000800Dh
				/*000E*/ 0xB0, 0x02,// mov al,2
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0007,
				0x0009,
				0x000E,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, origRip + 0x1000, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Jmp_fwd_no_opt() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 0000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 0000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
				/*000D*/ 0x90,// nop
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 0000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 0000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
				/*000D*/ 0x90,// nop
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0004,
				0x0006,
				0x000B,
				0x000D,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.DontFixBranches;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}
	}
}
#endif
