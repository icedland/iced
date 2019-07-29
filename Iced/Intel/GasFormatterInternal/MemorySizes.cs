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

#if !NO_GAS_FORMATTER && !NO_FORMATTER
using System;

namespace Iced.Intel.GasFormatterInternal {
	static class MemorySizes {
		public readonly struct Info {
			public readonly MemorySize memorySize;
			public readonly string bcstTo;
			public Info(MemorySize memorySize, string bcstTo) {
				this.memorySize = memorySize;
				this.bcstTo = bcstTo;
			}
		}

		public static readonly Info[] AllMemorySizes = GetMemorySizes();
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

			var infos = new Info[DecoderConstants.NumberOfMemorySizes];
			for (int i = 0; i < infos.Length; i++) {
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

				infos[i] = new Info((MemorySize)i, bcstTo);
			}

			return infos;
		}
	}
}
#endif
