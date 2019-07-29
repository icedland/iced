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

#if !NO_NASM_FORMATTER && !NO_FORMATTER
using System;

namespace Iced.Intel.NasmFormatterInternal {
	static class MemorySizes {
		public readonly struct Info {
			public readonly MemorySize memorySize;
			public readonly int size;
			public readonly string keyword;
			public readonly string bcstTo;
			public Info(MemorySize memorySize, int size, string keyword, string bcstTo) {
				this.memorySize = memorySize;
				this.size = size;
				this.keyword = keyword;
				this.bcstTo = bcstTo;
			}
		}
		public static readonly Info[] AllMemorySizes = GetMemorySizes();
		enum Size {
			S0,
			S1,
			S2,
			S4,
			S5,
			S6,
			S8,
			S10,
			S14,
			S16,
			S28,
			S32,
			S64,
			S94,
			S108,
			S512,
		}
		enum MemoryKeywords {
			None,
			@byte,
			dword,
			far,
			fpuenv14,
			fpuenv28,
			fpustate108,
			fpustate94,
			oword,
			qword,
			tword,
			word,
			yword,
			zword,
		}
		enum BroadcastToKind {
			b1to2,
			b1to4,
			b1to8,
			b1to16,
		}
		static Info[] GetMemorySizes() {
			var bcstToData = new byte[DecoderConstants.NumberOfMemorySizes - (int)MemorySize.Broadcast64_UInt32] {
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to16,
				(byte)BroadcastToKind.b1to16,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to16,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to16,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to2,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to4,
				(byte)BroadcastToKind.b1to8,
				(byte)BroadcastToKind.b1to16,
			};
			const int SizeKindShift = 4;
			const int MemoryKeywordsMask = 0xF;
			var data = new byte[DecoderConstants.NumberOfMemorySizes] {
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S0 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.@byte | ((uint)Size.S1 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.word | ((uint)Size.S2 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.@byte | ((uint)Size.S1 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.word | ((uint)Size.S2 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.far | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.far | ((uint)Size.S6 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.far | ((uint)Size.S10 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.word | ((uint)Size.S2 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S5 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S6 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S10 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.word | ((uint)Size.S2 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.tword | ((uint)Size.S10 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.word | ((uint)Size.S2 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.fpuenv14 | ((uint)Size.S14 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.fpuenv28 | ((uint)Size.S28 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.fpustate94 | ((uint)Size.S94 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.fpustate108 | ((uint)Size.S108 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S512 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S512 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S0 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)Size.S0 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.tword | ((uint)Size.S10 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.word | ((uint)Size.S2 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.word | ((uint)Size.S2 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.oword | ((uint)Size.S16 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.yword | ((uint)Size.S32 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.zword | ((uint)Size.S64 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.qword | ((uint)Size.S8 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
				(byte)((uint)MemoryKeywords.dword | ((uint)Size.S4 << SizeKindShift)),
			};
			var sizes = new ushort[] {
				0,
				1,
				2,
				4,
				5,
				6,
				8,
				10,
				14,
				16,
				28,
				32,
				64,
				94,
				108,
				512,
			};

			var infos = new Info[DecoderConstants.NumberOfMemorySizes];
			for (int i = 0; i < infos.Length; i++) {
				var d = data[i];

				string keyword;
				switch ((MemoryKeywords)(d & MemoryKeywordsMask)) {
				case MemoryKeywords.None:		keyword = null; break;
				case MemoryKeywords.@byte:		keyword = "byte"; break;
				case MemoryKeywords.dword:		keyword = "dword"; break;
				case MemoryKeywords.far:		keyword = "far"; break;
				case MemoryKeywords.fpuenv14:	keyword = "fpuenv14"; break;
				case MemoryKeywords.fpuenv28:	keyword = "fpuenv28"; break;
				case MemoryKeywords.fpustate108:keyword = "fpustate108"; break;
				case MemoryKeywords.fpustate94:	keyword = "fpustate94"; break;
				case MemoryKeywords.oword:		keyword = "oword"; break;
				case MemoryKeywords.qword:		keyword = "qword"; break;
				case MemoryKeywords.tword:		keyword = "tword"; break;
				case MemoryKeywords.word:		keyword = "word"; break;
				case MemoryKeywords.yword:		keyword = "yword"; break;
				case MemoryKeywords.zword:		keyword = "zword"; break;
				default:								throw new InvalidOperationException();
				}

				string bcstTo;
				if (i < (int)MemorySize.Broadcast64_UInt32)
					bcstTo = null;
				else {
					switch ((BroadcastToKind)bcstToData[i - (int)MemorySize.Broadcast64_UInt32]) {
					case BroadcastToKind.b1to2:		bcstTo = "1to2"; break;
					case BroadcastToKind.b1to4:		bcstTo = "1to4"; break;
					case BroadcastToKind.b1to8:		bcstTo = "1to8"; break;
					case BroadcastToKind.b1to16:	bcstTo = "1to16"; break;
					default:						throw new InvalidOperationException();
					}
				}

				infos[i] = new Info((MemorySize)i, sizes[d >> SizeKindShift], keyword, bcstTo);
			}

			return infos;
		}
	}
}
#endif
