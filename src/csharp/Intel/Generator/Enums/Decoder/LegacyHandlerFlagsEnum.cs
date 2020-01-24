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

namespace Generator.Enums.Decoder {
	[Flags]
	enum LegacyHandlerFlags : uint {
		HandlerReg = 0x00000001,
		HandlerMem = 0x00000002,
		Handler66Reg = 0x00000004,
		Handler66Mem = 0x00000008,
		HandlerF3Reg = 0x00000010,
		HandlerF3Mem = 0x00000020,
		HandlerF2Reg = 0x00000040,
		HandlerF2Mem = 0x00000080,
	}

	static class LegacyHandlerFlagsEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			typeof(LegacyHandlerFlags).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(LegacyHandlerFlags)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.LegacyHandlerFlags, documentation, GetValues(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
