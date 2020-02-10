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

#if GAS
using System.Diagnostics;
using Iced.Intel.FormatterInternal;

namespace Iced.Intel.GasFormatterInternal {
	static class Registers {
		public const int Register_ST = IcedConstants.NumberOfRegisters + 0;
		public const int ExtraRegisters = 1;
		public static readonly FormatterString[] AllRegistersNaked = RegistersTable.GetRegisters();
		public static readonly FormatterString[] AllRegisters = GetRegistersWithPrefix();
		static FormatterString[] GetRegistersWithPrefix() {
			var registers = AllRegistersNaked;
			Debug2.Assert(!(registers is null));
			var result = new FormatterString[registers.Length];
			for (int i = 0; i < registers.Length; i++)
				result[i] = new FormatterString("%" + registers[i].Get(false));
			return result;
		}
	}
}
#endif
