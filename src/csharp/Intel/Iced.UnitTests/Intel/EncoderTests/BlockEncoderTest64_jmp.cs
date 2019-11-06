/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if !NO_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest64_jmp : BlockEncoderTest {
		const int bitness = 64;
		const ulong origRip = 0x8000;
		const ulong newRip = 0x8000000000000000;

		[Fact]
		void Jmp_fwd() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
				/*000D*/ 0x90,// nop
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x06,// jmp short 800000000000000Ah
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xEB, 0x02,// jmp short 800000000000000Ah
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
				/*0003*/ 0xEB, 0xFB,// jmp short 0000000000008000h
				/*0005*/ 0xB0, 0x01,// mov al,1
				/*0007*/ 0xE9, 0xF4, 0xFF, 0xFF, 0xFF,// jmp near ptr 0000000000008000h
				/*000C*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0x90,// nop
				/*0001*/ 0xB0, 0x00,// mov al,0
				/*0003*/ 0xEB, 0xFB,// jmp short 8000000000000000h
				/*0005*/ 0xB0, 0x01,// mov al,1
				/*0007*/ 0xEB, 0xF7,// jmp short 8000000000000000h
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
		void Jmp_other_short() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x0A,// jmp short 000000000000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xEB, 0x06,// jmp short 000000000000800Dh
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
				/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xE9, 0x06, 0xF0, 0xFF, 0xFF,// jmp near ptr 000000000000800Dh
				/*0007*/ 0xB0, 0x01,// mov al,1
				/*0009*/ 0xE9, 0xFF, 0xEF, 0xFF, 0xFF,// jmp near ptr 000000000000800Dh
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
		void Jmp_other_long() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 123456789ABCDE0Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x03, 0x00, 0x00, 0x00,// jmp near ptr 123456789ABCDE0Eh
				/*000B*/ 0xB0, 0x02,// mov al,2
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xFF, 0x25, 0x10, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000018h]
				/*0008*/ 0xB0, 0x01,// mov al,1
				/*000A*/ 0xFF, 0x25, 0x10, 0x00, 0x00, 0x00,// jmp qword ptr [8000000000000020h]
				/*0010*/ 0xB0, 0x02,// mov al,2
				/*0012*/ 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC,
				/*0018*/ 0x0D, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
				/*0020*/ 0x0E, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12,
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				uint.MaxValue,
				0x0008,
				uint.MaxValue,
				0x0010,
			};
			var expectedRelocInfos = new RelocInfo[] {
				new RelocInfo(RelocKind.Offset64, 0x8000000000000018),
				new RelocInfo(RelocKind.Offset64, 0x8000000000000020),
			};
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			const ulong origRip = 0x123456789ABCDE00;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Jmp_fwd_no_opt() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
				/*000B*/ 0xB0, 0x02,// mov al,2
				/*000D*/ 0x90,// nop
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xEB, 0x09,// jmp short 000000000000800Dh
				/*0004*/ 0xB0, 0x01,// mov al,1
				/*0006*/ 0xE9, 0x02, 0x00, 0x00, 0x00,// jmp near ptr 000000000000800Dh
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
