/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_ENCODER
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class BlockEncoderTest64_xbegin : BlockEncoderTest {
		const int bitness = 64;
		const ulong origRip = 0x8000;
		const ulong newRip = 0x8000000000000000;

		[Fact]
		void Xbegin_fwd() {
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
				/*0002*/ 0xC7, 0xF8, 0x10, 0x00, 0x00, 0x00,// xbegin 8000000000000018h
				/*0008*/ 0xB0, 0x01,// mov al,1
				/*000A*/ 0xC7, 0xF8, 0x0A, 0x00, 0x00, 0x00,// xbegin 800000000000001Ah
				/*0010*/ 0xB0, 0x02,// mov al,2
				/*0012*/ 0xC7, 0xF8, 0x04, 0x00, 0x00, 0x00,// xbegin 800000000000001Ch
				/*0018*/ 0xB0, 0x03,// mov al,3
				/*001A*/ 0xB0, 0x04,// mov al,4
				/*001C*/ 0xB0, 0x05,// mov al,5
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0008,
				0x000A,
				0x0010,
				0x0012,
				0x0018,
				0x001A,
				0x001C,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}

		[Fact]
		void Xbegin_bwd() {
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
				/*0008*/ 0xC7, 0xF8, 0xF2, 0xFF, 0xFF, 0xFF,// xbegin 8000000000000000h
				/*000E*/ 0xB0, 0x01,// mov al,1
				/*0010*/ 0xC7, 0xF8, 0xEC, 0xFF, 0xFF, 0xFF,// xbegin 8000000000000002h
				/*0016*/ 0xB0, 0x02,// mov al,2
				/*0018*/ 0xC7, 0xF8, 0xE6, 0xFF, 0xFF, 0xFF,// xbegin 8000000000000004h
			};
			var expectedInstructionOffsets = new uint[] {
				0x0000,
				0x0002,
				0x0004,
				0x0006,
				0x0008,
				0x000E,
				0x0010,
				0x0016,
				0x0018,
			};
			var expectedRelocInfos = Array.Empty<RelocInfo>();
			const BlockEncoderOptions options = BlockEncoderOptions.None;
			EncodeBase(bitness, origRip, originalData, newRip, newData, options, decoderOptions, expectedInstructionOffsets, expectedRelocInfos);
		}
	}
}
#endif
