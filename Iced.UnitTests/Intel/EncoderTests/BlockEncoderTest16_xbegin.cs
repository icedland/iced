/*
    Copyright (C) 2018 de4dot@gmail.com

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
	public sealed class BlockEncoderTest16_xbegin : BlockEncoderTest {
		const int bitness = 16;
		const ulong origRip = 0x8000;
		const ulong newRip = 0xF000;

		[Fact]
		void Xbegin_fwd() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0x66, 0xC7, 0xF8, 0x06, 0x00, 0x00, 0x00,// xbegin 800Fh
				/*0009*/ 0xB0, 0x01,// mov al,1
				/*000B*/ 0xC7, 0xF8, 0x02, 0x00,// xbegin 8011h
				/*000F*/ 0xB0, 0x02,// mov al,2
				/*0011*/ 0xB0, 0x03,// mov al,3
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x00,// mov al,0
				/*0002*/ 0xC7, 0xF8, 0x06, 0x00,// xbegin 0F00Ch
				/*0006*/ 0xB0, 0x01,// mov al,1
				/*0008*/ 0xC7, 0xF8, 0x02, 0x00,// xbegin 0F00Eh
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
		void Xbegin_bwd() {
			var originalData = new byte[] {
				/*0000*/ 0xB0, 0x02,// mov al,2
				/*0002*/ 0xB0, 0x03,// mov al,3
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0x66, 0xC7, 0xF8, 0xF3, 0xFF, 0xFF, 0xFF,// xbegin 8000h
				/*000D*/ 0xB0, 0x01,// mov al,1
				/*000F*/ 0xC7, 0xF8, 0xEF, 0xFF,// xbegin 8002h
			};
			var newData = new byte[] {
				/*0000*/ 0xB0, 0x02,// mov al,2
				/*0002*/ 0xB0, 0x03,// mov al,3
				/*0004*/ 0xB0, 0x00,// mov al,0
				/*0006*/ 0xC7, 0xF8, 0xF6, 0xFF,// xbegin 0F000h
				/*000A*/ 0xB0, 0x01,// mov al,1
				/*000C*/ 0xC7, 0xF8, 0xF2, 0xFF,// xbegin 0F002h
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
