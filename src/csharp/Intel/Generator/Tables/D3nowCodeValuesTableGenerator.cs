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

using System.Linq;
using Generator.Enums;

namespace Generator.Tables {
	abstract class D3nowCodeValuesTableGenerator {
		protected abstract void Generate((int index, EnumValue enumValue)[] infos);

		protected readonly GenTypes genTypes;

		protected D3nowCodeValuesTableGenerator(GenTypes genTypes) =>
			this.genTypes = genTypes;

		public void Generate() {
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var infos = defs.Where(a => a.Encoding == EncodingKind.D3NOW).
				Select(a => (index: (int)a.OpCode, enumValue: a.Code)).
				OrderBy(a => a.index).
				ToArray();
			Generate(infos);
		}
	}
}
