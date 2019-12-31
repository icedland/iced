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
	enum RepPrefixKind {
		[Comment("No #(c:REP)#/#(c:REPE)#/#(c:REPNE)# prefix")]
		None,

		[Comment("#(c:REP)#/#(c:REPE)# prefix")]
		Repe,

		[Comment("#(c:REPNE)# prefix")]
		Repne,
	}

	static class RepPrefixKindEnum {
		const string documentation = "#(c:REP)#/#(c:REPE)#/#(c:REPNE)# prefix";

		static EnumValue[] GetValues() =>
			typeof(RepPrefixKind).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(RepPrefixKind)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.RepPrefixKind, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
