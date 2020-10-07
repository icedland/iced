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
	static class OpCodeHandlers {
		public static readonly OpCodeHandler[] Handlers;

		static OpCodeHandlers() {
			var encFlags1 = EncoderData.EncFlags1;
			var encFlags2 = EncoderData.EncFlags2;
			var encFlags3Data = EncoderData.EncFlags3;
			var handlers = new OpCodeHandler[IcedConstants.NumberOfCodeValues];
			int i = 0;
			var invalidHandler = new InvalidHandler();
			for (; i < encFlags1.Length; i++) {
				var encFlags3 = (EncFlags3)encFlags3Data[i];
				OpCodeHandler handler;
				switch ((EncodingKind)(((uint)encFlags3 >> (int)EncFlags3.EncodingShift) & (uint)EncFlags3.EncodingMask)) {
				case EncodingKind.Legacy:
					var code = (Code)i;
					if (code == Code.INVALID)
						handler = invalidHandler;
					else if (code <= Code.DeclareQword)
						handler = new DeclareDataHandler(code);
					else
						handler = new LegacyHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
					break;

				case EncodingKind.VEX:
#if !NO_VEX
					handler = new VexHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.EVEX:
#if !NO_EVEX
					handler = new EvexHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.XOP:
#if !NO_XOP
					handler = new XopHandler((EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				case EncodingKind.D3NOW:
#if !NO_D3NOW
					handler = new D3nowHandler((EncFlags2)encFlags2[i], encFlags3);
#else
					handler = invalidHandler;
#endif
					break;

				default:
					throw new InvalidOperationException();
				}
				handlers[i] = handler;
			}
			if (i != handlers.Length)
				throw new InvalidOperationException();
			Handlers = handlers;
		}
	}
}
#endif
