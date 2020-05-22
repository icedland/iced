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

#if DECODER && !NO_XOP
namespace Iced.Intel.DecoderInternal {
	static partial class OpCodeHandlersTables_XOP {
		internal static readonly OpCodeHandler[] XOP8;
		internal static readonly OpCodeHandler[] XOP9;
		internal static readonly OpCodeHandler[] XOPA;

		static OpCodeHandlersTables_XOP() {
			var handlerReader = new VexOpCodeHandlerReader();
			var deserializer = new TableDeserializer(handlerReader, MaxIdNames, GetSerializedTables());
			deserializer.Deserialize();
			XOP8 = deserializer.GetTable(XOP8Index);
			XOP9 = deserializer.GetTable(XOP9Index);
			XOPA = deserializer.GetTable(XOPAIndex);
		}
	}
}
#endif
