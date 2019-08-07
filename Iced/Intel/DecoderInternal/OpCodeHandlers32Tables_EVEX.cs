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

#if !NO_DECODER32 && !NO_DECODER
namespace Iced.Intel.DecoderInternal.OpCodeHandlers32 {
	static class OpCodeHandlers32Tables_EVEX {
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F38XX;
		internal static readonly OpCodeHandler[] ThreeByteHandlers_0F3AXX;
		internal static readonly OpCodeHandler[] TwoByteHandlers_0FXX;

		static OpCodeHandlers32Tables_EVEX() =>
			OpCodeHandlersTables_EVEX.GetTables(32, out ThreeByteHandlers_0F38XX, out ThreeByteHandlers_0F3AXX, out TwoByteHandlers_0FXX);
	}
}
#endif
