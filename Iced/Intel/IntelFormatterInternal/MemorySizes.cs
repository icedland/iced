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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System;

namespace Iced.Intel.IntelFormatterInternal {
	static class MemorySizes {
		public readonly struct Info {
			public readonly MemorySize memorySize;
			public readonly string[] keywords;
			public readonly string? bcstTo;
			public Info(MemorySize memorySize, string[] keywords, string? bcstTo) {
				this.memorySize = memorySize;
				this.keywords = keywords;
				this.bcstTo = bcstTo;
			}
		}
		public static readonly Info[] AllMemorySizes = GetMemorySizes();
		enum MemoryKeywords {
			None,
			byte_ptr,
			dword_ptr,
			fpuenv14_ptr,
			fpuenv28_ptr,
			fpustate108_ptr,
			fpustate94_ptr,
			fword_ptr,
			qword_ptr,
			tbyte_ptr,
			word_ptr,
			xmmword_ptr,
			ymmword_ptr,
			zmmword_ptr,
		}
		enum BroadcastToKind {
			None,
			b1to2,
			b1to4,
			b1to8,
			b1to16,
		}
		static Info[] GetMemorySizes() {
			var ptr = "ptr";
			var byte_ptr = new string[] { "byte", ptr };
			var word_ptr = new string[] { "word", ptr };
			var dword_ptr = new string[] { "dword", ptr };
			var qword_ptr = new string[] { "qword", ptr };
			var xmmword_ptr = new string[] { "xmmword", ptr };
			var ymmword_ptr = new string[] { "ymmword", ptr };
			var zmmword_ptr = new string[] { "zmmword", ptr };
			var fword_ptr = new string[] { "fword", ptr };
			var tbyte_ptr = new string[] { "tbyte", ptr };
			var fpuenv14_ptr = new string[] { "fpuenv14", ptr };
			var fpuenv28_ptr = new string[] { "fpuenv28", ptr };
			var fpustate108_ptr = new string[] { "fpustate108", ptr };
			var fpustate94_ptr = new string[] { "fpustate94", ptr };

			var infos = new Info[DecoderConstants.NumberOfMemorySizes];
			const int BroadcastToKindShift = 5;
			const int MemoryKeywordsMask = 0x1F;
			var data = new byte[DecoderConstants.NumberOfMemorySizes] {
				(byte)((uint)MemoryKeywords.None | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.byte_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.word_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.byte_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.word_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.fword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.tbyte_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.word_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.fword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.fword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.word_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.tbyte_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.word_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.fpuenv14_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.fpuenv28_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.fpustate94_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.fpustate108_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.None | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.tbyte_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.word_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.word_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.xmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.ymmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.zmmword_ptr | ((uint)BroadcastToKind.None << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to16 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to16 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to16 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to16 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to2 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.qword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to4 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to8 << BroadcastToKindShift)),
				(byte)((uint)MemoryKeywords.dword_ptr | ((uint)BroadcastToKind.b1to16 << BroadcastToKindShift)),
			};

			for (int i = 0; i < infos.Length; i++) {
				var d = data[i];

				string[] keywords;
				switch ((MemoryKeywords)(d & MemoryKeywordsMask)) {
				case MemoryKeywords.None:				keywords = Array2.Empty<string>(); break;
				case MemoryKeywords.byte_ptr:			keywords = byte_ptr; break;
				case MemoryKeywords.dword_ptr:			keywords = dword_ptr; break;
				case MemoryKeywords.fpuenv14_ptr:		keywords = fpuenv14_ptr; break;
				case MemoryKeywords.fpuenv28_ptr:		keywords = fpuenv28_ptr; break;
				case MemoryKeywords.fpustate108_ptr:	keywords = fpustate108_ptr; break;
				case MemoryKeywords.fpustate94_ptr:		keywords = fpustate94_ptr; break;
				case MemoryKeywords.fword_ptr:			keywords = fword_ptr; break;
				case MemoryKeywords.qword_ptr:			keywords = qword_ptr; break;
				case MemoryKeywords.tbyte_ptr:			keywords = tbyte_ptr; break;
				case MemoryKeywords.word_ptr:			keywords = word_ptr; break;
				case MemoryKeywords.xmmword_ptr:		keywords = xmmword_ptr; break;
				case MemoryKeywords.ymmword_ptr:		keywords = ymmword_ptr; break;
				case MemoryKeywords.zmmword_ptr:		keywords = zmmword_ptr; break;
				default:								throw new InvalidOperationException();
				}

				string? bcstTo;
				switch ((BroadcastToKind)(d >> BroadcastToKindShift)) {
				case BroadcastToKind.None:		bcstTo = null; break;
				case BroadcastToKind.b1to2:		bcstTo = "1to2"; break;
				case BroadcastToKind.b1to4:		bcstTo = "1to4"; break;
				case BroadcastToKind.b1to8:		bcstTo = "1to8"; break;
				case BroadcastToKind.b1to16:	bcstTo = "1to16"; break;
				default:						throw new InvalidOperationException();
				}

				infos[i] = new Info((MemorySize)i, keywords, bcstTo);
			}

			return infos;
		}
	}
}
#endif
