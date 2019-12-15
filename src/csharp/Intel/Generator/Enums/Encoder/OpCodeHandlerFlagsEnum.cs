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

namespace Generator.Enums.Encoder {
	[Flags]
	enum OpCodeHandlerFlags : uint {
		None					= 0,
		Fwait					= 0x00000001,
		DeclareData				= 0x00000002,
	}

	static class OpCodeHandlerFlagsEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			typeof(OpCodeHandlerFlags).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(OpCodeHandlerFlags)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.OpCodeHandlerFlags, documentation, GetValues(), EnumTypeFlags.NoInitialize | EnumTypeFlags.Flags);
	}
}
