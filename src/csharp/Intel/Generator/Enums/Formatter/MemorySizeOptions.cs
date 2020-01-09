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

namespace Generator.Enums.Formatter {
	enum MemorySizeOptions {
		[Comment("Show memory size if the assembler requires it, else don't show anything")]
		Default,

		[Comment("Always show the memory size, even if the assembler doesn't need it")]
		Always,

		[Comment("Show memory size if a human can't figure out the size of the operand")]
		Minimum,

		[Comment("Never show memory size")]
		Never,
	}

	static class MemorySizeOptionsEnum {
		const string documentation = "Memory size options used by the formatters";

		static EnumValue[] GetValues() =>
			typeof(MemorySizeOptions).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(MemorySizeOptions)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.MemorySizeOptions, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
