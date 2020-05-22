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

#if ENCODER
using System;

namespace Iced.Intel.EncoderInternal {
	static partial class OpCodeHandlers {
		public static readonly OpCodeHandler[] Handlers;

		static OpCodeHandlers() {
			var info = GetData();
			var handlers = new OpCodeHandler[IcedConstants.NumberOfCodeValues];
			int j = 0;
			var invalidHandler = new InvalidHandler();
			for (int i = 0; i < info.Length; i += 3, j++) {
				uint dword1 = info[i];
				OpCodeHandler handler;
				switch ((EncodingKind)((dword1 >> (int)EncFlags1.EncodingShift) & (uint)EncFlags1.EncodingMask)) {
				case EncodingKind.Legacy:
					var code = (Code)j;
					if (code == Code.INVALID)
						handler = invalidHandler;
					else if (code <= Code.DeclareQword)
						handler = new DeclareDataHandler(code);
					else
						handler = new LegacyHandler(dword1, info[i + 1], info[i + 2]);
					break;

				case EncodingKind.VEX:
#if !NO_VEX
					handler = new VexHandler(dword1, info[i + 1], info[i + 2]);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.EVEX:
#if !NO_EVEX
					handler = new EvexHandler(dword1, info[i + 1], info[i + 2]);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.XOP:
#if !NO_XOP
					handler = new XopHandler(dword1, info[i + 1], info[i + 2]);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.D3NOW:
#if !NO_D3NOW
					handler = new D3nowHandler(dword1, info[i + 1], info[i + 2]);
#else
					handler = invalidHandler;
#endif
					break;

				default:
					throw new InvalidOperationException();
				}
				handlers[j] = handler;
			}
			if (j != handlers.Length)
				throw new InvalidOperationException();
			Handlers = handlers;
		}
	}
}
#endif
