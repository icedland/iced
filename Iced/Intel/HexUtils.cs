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

namespace Iced.Intel {
	static class HexUtils {
		public static byte[] ToByteArray(string hexData) {
			if (hexData == null)
				throw new ArgumentNullException(nameof(hexData));
			if (hexData.Length == 0)
				return Array.Empty<byte>();
			var data = new byte[hexData.Length / 2];
			int w = 0;
			for (int i = 0; ;) {
				while (i < hexData.Length && char.IsWhiteSpace(hexData[i]))
					i++;
				if (i >= hexData.Length)
					break;
				int hi = TryParseHexChar(hexData[i++]);
				if (hi < 0)
					throw new ArgumentOutOfRangeException(nameof(hexData));

				while (i < hexData.Length && char.IsWhiteSpace(hexData[i]))
					i++;
				if (i >= hexData.Length)
					throw new ArgumentOutOfRangeException(nameof(hexData));
				int lo = TryParseHexChar(hexData[i++]);
				if (lo < 0)
					throw new ArgumentOutOfRangeException(nameof(hexData));
				data[w++] = (byte)((hi << 4) | lo);
			}
			if (w != data.Length)
				Array.Resize(ref data, w);
			return data;
		}

		static int TryParseHexChar(char c) {
			if ('0' <= c && c <= '9')
				return c - '0';
			if ('a' <= c && c <= 'f')
				return c - 'a' + 10;
			if ('A' <= c && c <= 'F')
				return c - 'A' + 10;
			return -1;
		}
	}
}
#endif
