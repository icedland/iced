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
		public static uint GetRegisterCount(FuzzerRegisterKind register) =>
			register switch {
				FuzzerRegisterKind.GPR8 or FuzzerRegisterKind.GPR16 or FuzzerRegisterKind.GPR32 or FuzzerRegisterKind.GPR64 or
				FuzzerRegisterKind.CR or FuzzerRegisterKind.DR => 16,
				FuzzerRegisterKind.Segment or FuzzerRegisterKind.ST or FuzzerRegisterKind.TR or FuzzerRegisterKind.K or FuzzerRegisterKind.MM or
				FuzzerRegisterKind.TMM => 8,
				FuzzerRegisterKind.BND => 4,
				FuzzerRegisterKind.XMM or FuzzerRegisterKind.YMM or FuzzerRegisterKind.ZMM => 32,
				_ => throw ThrowHelpers.Unreachable,
			};

		public static uint GetRegisterCount(FuzzerRegisterClass registerClass) =>
			registerClass switch {
				FuzzerRegisterClass.GPR or FuzzerRegisterClass.CR or FuzzerRegisterClass.DR => 16,
				FuzzerRegisterClass.Segment or FuzzerRegisterClass.ST or FuzzerRegisterClass.TR or FuzzerRegisterClass.K or FuzzerRegisterClass.MM or
				FuzzerRegisterClass.TMM => 8,
				FuzzerRegisterClass.BND => 4,
				FuzzerRegisterClass.Vector => 32,
				_ => throw ThrowHelpers.Unreachable,
			};
	}
}
