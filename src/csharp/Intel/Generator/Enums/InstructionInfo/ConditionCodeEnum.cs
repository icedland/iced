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

namespace Generator.Enums.InstructionInfo {
	enum ConditionCode {
		[Comment("The instruction doesn't have a condition code")]
		None,
		[Comment("Overflow (#(c:OF=1)#)")]
		o,
		[Comment("Not overflow (#(c:OF=0)#)")]
		no,
		[Comment("Below (unsigned) (#(c:CF=1)#)")]
		b,
		[Comment("Above or equal (unsigned) (#(c:CF=0)#)")]
		ae,
		[Comment("Equal / zero (#(c:ZF=1)#)")]
		e,
		[Comment("Not equal / zero (#(c:ZF=0)#)")]
		ne,
		[Comment("Below or equal (unsigned) (#(c:CF=1 or ZF=1)#)")]
		be,
		[Comment("Above (unsigned) (#(c:CF=0 and ZF=0)#)")]
		a,
		[Comment("Signed (#(c:SF=1)#)")]
		s,
		[Comment("Not signed (#(c:SF=0)#)")]
		ns,
		[Comment("Parity (#(c:PF=1)#)")]
		p,
		[Comment("Not parity (#(c:PF=0)#)")]
		np,
		[Comment("Less (signed) (#(c:SF!=OF)#)")]
		l,
		[Comment("Greater than or equal (signed) (#(c:SF=OF)#)")]
		ge,
		[Comment("Less than or equal (signed) (#(c:ZF=1 or SF!=OF)#)")]
		le,
		[Comment("Greater (signed) (#(c:ZF=0 and SF=OF)#)")]
		g,
	}

	static class ConditionCodeEnum {
		const string documentation = "Instruction condition code (used by #(c:Jcc)#, #(c:SETcc)#, #(c:CMOVcc)#)";

		static EnumValue[] GetValues() =>
			typeof(ConditionCode).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(ConditionCode)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.ConditionCode, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
