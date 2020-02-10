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

#if GAS || INTEL || MASM || NASM
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class RegisterTests {
		protected static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedRegistersFile) {
			var formattedRegisters = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedRegistersFile)).ToArray();
			if (IcedConstants.NumberOfRegisters != formattedRegisters.Length)
				throw new ArgumentException($"({nameof(IcedConstants.NumberOfRegisters)}) {IcedConstants.NumberOfRegisters} != (formattedRegisters.Length) {formattedRegisters.Length}");
			var res = new object[formattedRegisters.Length][];
			for (int i = 0; i < res.Length; i++)
				res[i] = new object[2] { (Register)i, formattedRegisters[i] };
			return res;
		}

		protected void FormatBase(Register register, string formattedString, Formatter formatter) {
			{
				var actualFormattedString = formatter.Format(register);
				Assert.Equal(formattedString, actualFormattedString);
			}
			{
				formatter.Options.UppercaseRegisters = false;
				var actualFormattedString = formatter.Format(register);
				Assert.Equal(formattedString.ToLowerInvariant(), actualFormattedString);
			}
			{
				formatter.Options.UppercaseRegisters = true;
				var actualFormattedString = formatter.Format(register);
				Assert.Equal(formattedString.ToUpperInvariant(), actualFormattedString);
			}
		}
	}
}
#endif
