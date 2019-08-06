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

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System;

namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_VEX {
		public static void GetTables(int bitness, out OpCodeHandler[] threeByteHandlers_0F38XX, out OpCodeHandler[] threeByteHandlers_0F3AXX, out OpCodeHandler[] twoByteHandlers_0FXX) {
			OpCodeHandlerReader handlerReader;
			switch (bitness) {
#if !NO_DECODER32
			case 32: handlerReader = new OpCodeHandlers32.VexOpCodeHandlerReader32(); break;
#endif
#if !NO_DECODER64
			case 64: handlerReader = new OpCodeHandlers64.VexOpCodeHandlerReader64(); break;
#endif
			default: throw new InvalidOperationException();
			}
			var deserializer = new TableDeserializer(handlerReader, GetSerializedTables());
			deserializer.Deserialize();
			threeByteHandlers_0F38XX = deserializer.GetTable(ThreeByteHandlers_0F38XXIndex);
			threeByteHandlers_0F3AXX = deserializer.GetTable(ThreeByteHandlers_0F3AXXIndex);
			twoByteHandlers_0FXX = deserializer.GetTable(TwoByteHandlers_0FXXIndex);
		}
	}
}
#endif
