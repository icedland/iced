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

namespace Iced.Intel {
	static class InstructionUtils {
		public static int GetAddressSizeInBytes(Register baseReg, Register indexReg, int displSize, CodeSize codeSize) {
			if ((Register.RAX <= baseReg && baseReg <= Register.R15) || (Register.RAX <= indexReg && indexReg <= Register.R15) || baseReg == Register.RIP)
				return 8;
			if ((Register.EAX <= baseReg && baseReg <= Register.R15D) || (Register.EAX <= indexReg && indexReg <= Register.R15D) || baseReg == Register.EIP)
				return 4;
			if ((Register.AX <= baseReg && baseReg <= Register.DI) || (Register.AX <= indexReg && indexReg <= Register.DI))
				return 2;
			if (displSize == 2 || displSize == 4 || displSize == 8)
				return displSize;

			if (codeSize == CodeSize.Code64)
				return 8;
			if (codeSize == CodeSize.Code32)
				return 4;
			if (codeSize == CodeSize.Code16)
				return 2;
			return 8;
		}
	}
}
