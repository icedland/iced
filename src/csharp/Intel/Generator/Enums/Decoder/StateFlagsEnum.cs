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
	static class StateFlagsEnum {
		const string? documentation = null;

		[Flags]
		enum Enum : uint {
			EncodingMask			= 7,
			HasRex					= 0x00000008,
			b						= 0x00000010,
			z						= 0x00000020,
			IsInvalid				= 0x00000040,
			W						= 0x00000080,
			NoImm					= 0x00000100,
			Addr64					= 0x00000200,
			BranchImm8				= 0x00000400,
			Xbegin					= 0x00000800,
			Lock					= 0x00001000,
			AllowLock				= 0x00002000,
		}

		static EnumValue[] GetValues() =>
			typeof(Enum).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(Enum)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.StateFlags, documentation, GetValues(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
