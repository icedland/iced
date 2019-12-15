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

using System;
using System.Linq;

namespace Generator {
	static class ConstantUtils {
		public static (uint mask, uint bits) GetMaskBits(uint max) {
			if (max == 0)
				return (1, 1);
			uint mask = 0;
			uint bits = 0;
			while (max != 0) {
				max >>= 1;
				mask = (mask << 1) | 1;
				bits++;
			}
			return (mask, bits);
		}

		public static (uint mask, uint bits) GetMaskBits<T>() where T : Enum {
			var max = ((T[])Enum.GetValues(typeof(T))).Select(a => Convert.ToUInt32(a)).Max();
			return GetMaskBits(max);
		}

		public static void VerifyMask(uint mask, uint max) {
			if (GetMaskBits(max).mask != mask)
				throw new InvalidOperationException();
		}

		public static void VerifyMask<T>(uint mask) where T : Enum {
			if (GetMaskBits<T>().mask != mask)
				throw new InvalidOperationException();
		}
	}
}
