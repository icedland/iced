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

namespace IcedFuzzer.Core {
	static class RegisterUtils {
		public static uint GetRegisterCount(FuzzerRegisterKind register) {
			switch (register) {
			case FuzzerRegisterKind.GPR8:
			case FuzzerRegisterKind.GPR16:
			case FuzzerRegisterKind.GPR32:
			case FuzzerRegisterKind.GPR64:
			case FuzzerRegisterKind.CR:
			case FuzzerRegisterKind.DR:
				return 16;
			case FuzzerRegisterKind.Segment:
			case FuzzerRegisterKind.ST:
			case FuzzerRegisterKind.TR:
			case FuzzerRegisterKind.K:
			case FuzzerRegisterKind.MM:
			case FuzzerRegisterKind.TMM:
				return 8;
			case FuzzerRegisterKind.BND:
				return 4;
			case FuzzerRegisterKind.XMM:
			case FuzzerRegisterKind.YMM:
			case FuzzerRegisterKind.ZMM:
				return 32;
			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		public static uint GetRegisterCount(FuzzerRegisterClass registerClass) {
			switch (registerClass) {
			case FuzzerRegisterClass.GPR:
			case FuzzerRegisterClass.CR:
			case FuzzerRegisterClass.DR:
				return 16;
			case FuzzerRegisterClass.Segment:
			case FuzzerRegisterClass.ST:
			case FuzzerRegisterClass.TR:
			case FuzzerRegisterClass.K:
			case FuzzerRegisterClass.MM:
			case FuzzerRegisterClass.TMM:
				return 8;
			case FuzzerRegisterClass.BND:
				return 4;
			case FuzzerRegisterClass.Vector:
				return 32;
			default:
				throw ThrowHelpers.Unreachable;
			}
		}
	}
}
