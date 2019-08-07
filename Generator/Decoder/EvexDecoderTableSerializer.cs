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
using Iced.Intel.DecoderInternal;

namespace Generator.Decoder {
	sealed class EvexDecoderTableSerializer : DecoderTableSerializer {
		public override string ClassName => "OpCodeHandlersTables_EVEX";
		protected override object[] GetTablesToSerialize() => OpCodeHandlersTables_EVEX.GetHandlers();
		protected override string[] GetTableIndexNames() => new string[] {
			OpCodeHandlersTables_EVEX.ThreeByteHandlers_0F38XX,
			OpCodeHandlersTables_EVEX.ThreeByteHandlers_0F3AXX,
			OpCodeHandlersTables_EVEX.TwoByteHandlers_0FXX,
		};
		static readonly object handlerReferenceValue = EvexOpCodeHandlerKind.HandlerReference;
		static readonly object arrayReferenceValue = EvexOpCodeHandlerKind.ArrayReference;
		static readonly object invalid2Value = EvexOpCodeHandlerKind.Invalid2;
		static readonly object dupValue = EvexOpCodeHandlerKind.Dup;
		protected override object GetNullValue() => throw new InvalidOperationException();
		protected override object GetHandlerReferenceValue() => handlerReferenceValue;
		protected override object GetArrayReferenceValue() => arrayReferenceValue;
		protected override object GetInvalid2Value() => invalid2Value;
		protected override object GetDupValue() => dupValue;
	}
}
#endif
