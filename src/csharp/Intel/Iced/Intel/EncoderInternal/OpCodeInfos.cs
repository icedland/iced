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

#if ENCODER && OPCODE_INFO
using System.Text;

namespace Iced.Intel.EncoderInternal {
	static class OpCodeInfos {
		public static readonly OpCodeInfo[] Infos = CreateInfos();

		static OpCodeInfo[] CreateInfos() {
			var infos = new OpCodeInfo[IcedConstants.CodeEnumCount];
			var encFlags1 = EncoderData.EncFlags1;
			var encFlags2 = EncoderData.EncFlags2;
			var encFlags3 = EncoderData.EncFlags3;
			var opcFlags1 = OpCodeInfoData.OpcFlags1;
			var opcFlags2 = OpCodeInfoData.OpcFlags2;
			var sb = new StringBuilder();
			for (int i = 0; i < infos.Length; i++)
				infos[i] = new OpCodeInfo((Code)i, (EncFlags1)encFlags1[i], (EncFlags2)encFlags2[i], (EncFlags3)encFlags3[i], (OpCodeInfoFlags1)opcFlags1[i], (OpCodeInfoFlags2)opcFlags2[i], sb);
			return infos;
		}
	}
}
#endif
