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

using Generator.Enums;

namespace Generator.Decoder.CSharp {
	sealed class LegacyDecoderTableSerializer : DecoderTableSerializer {
		public override string ClassName => "OpCodeHandlersTables_Legacy";
		protected override object[] GetTablesToSerialize() => OpCodeHandlersTables_Legacy.GetHandlers();
		protected override string[] GetTableIndexNames() => new string[] {
			OpCodeHandlersTables_Legacy.OneByteHandlers,
		};
		static readonly object nullValue = OpCodeHandlerKindEnum.Instance["Null"];
		static readonly object handlerReferenceValue = OpCodeHandlerKindEnum.Instance["HandlerReference"];
		static readonly object arrayReferenceValue = OpCodeHandlerKindEnum.Instance["ArrayReference"];
		static readonly object invalid2Value = OpCodeHandlerKindEnum.Instance["Invalid2"];
		static readonly object dupValue = OpCodeHandlerKindEnum.Instance["Dup"];
		protected override object GetNullValue() => nullValue;
		protected override object GetHandlerReferenceValue() => handlerReferenceValue;
		protected override object GetArrayReferenceValue() => arrayReferenceValue;
		protected override object GetInvalid2Value() => invalid2Value;
		protected override object GetDupValue() => dupValue;
	}
}
