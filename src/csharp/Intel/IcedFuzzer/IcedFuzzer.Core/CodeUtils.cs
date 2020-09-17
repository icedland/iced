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

using Iced.Intel;

namespace IcedFuzzer.Core {
	static class CodeUtils {
		public static bool IsReservedNop(Code code) {
			switch (code) {
			case Code.ReservedNop_rm16_r16_0F0D:
			case Code.ReservedNop_rm32_r32_0F0D:
			case Code.ReservedNop_rm64_r64_0F0D:
			case Code.ReservedNop_rm16_r16_0F18:
			case Code.ReservedNop_rm32_r32_0F18:
			case Code.ReservedNop_rm64_r64_0F18:
			case Code.ReservedNop_rm16_r16_0F19:
			case Code.ReservedNop_rm32_r32_0F19:
			case Code.ReservedNop_rm64_r64_0F19:
			case Code.ReservedNop_rm16_r16_0F1A:
			case Code.ReservedNop_rm32_r32_0F1A:
			case Code.ReservedNop_rm64_r64_0F1A:
			case Code.ReservedNop_rm16_r16_0F1B:
			case Code.ReservedNop_rm32_r32_0F1B:
			case Code.ReservedNop_rm64_r64_0F1B:
			case Code.ReservedNop_rm16_r16_0F1C:
			case Code.ReservedNop_rm32_r32_0F1C:
			case Code.ReservedNop_rm64_r64_0F1C:
			case Code.ReservedNop_rm16_r16_0F1D:
			case Code.ReservedNop_rm32_r32_0F1D:
			case Code.ReservedNop_rm64_r64_0F1D:
			case Code.ReservedNop_rm16_r16_0F1E:
			case Code.ReservedNop_rm32_r32_0F1E:
			case Code.ReservedNop_rm64_r64_0F1E:
			case Code.ReservedNop_rm16_r16_0F1F:
			case Code.ReservedNop_rm32_r32_0F1F:
			case Code.ReservedNop_rm64_r64_0F1F:
				return true;
			default:
				return false;
			}
		}
	}
}
