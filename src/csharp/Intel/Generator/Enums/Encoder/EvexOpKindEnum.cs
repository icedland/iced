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

namespace Generator.Enums.Encoder {
	enum EvexOpKind : byte {
		None,
		Ed,
		Eq,
		Gd,
		Gq,
		RdMb,
		RqMb,
		RdMw,
		RqMw,
		HX,
		HY,
		HZ,
		HXP3,
		HZP3,
		Ib,
		M,
		Rd,
		Rq,
		RX,
		RY,
		RZ,
		RK,
		VM32X,
		VM32Y,
		VM32Z,
		VM64X,
		VM64Y,
		VM64Z,
		VK,
		VKP1,
		VX,
		VY,
		VZ,
		WX,
		WY,
		WZ,
	}

	static class EvexOpKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			typeof(EvexOpKind).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(EvexOpKind)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.EvexOpKind, documentation, GetValues(), EnumTypeFlags.NoInitialize);
	}
}
