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
	enum CC_b {
		[Comment("#(c:JB)#, #(c:CMOVB)#, #(c:SETB)#")]
		b,
		[Comment("#(c:JC)#, #(c:CMOVC)#, #(c:SETC)#")]
		c,
		[Comment("#(c:JNAE)#, #(c:CMOVNAE)#, #(c:SETNAE)#")]
		nae,
	}

	static class CC_b_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JB)# / #(c:JC)# / #(c:JNAE)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_b).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_b)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_b, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_ae {
		[Comment("#(c:JAE)#, #(c:CMOVAE)#, #(c:SETAE)#")]
		ae,
		[Comment("#(c:JNB)#, #(c:CMOVNB)#, #(c:SETNB)#")]
		nb,
		[Comment("#(c:JNC)#, #(c:CMOVNC)#, #(c:SETNC)#")]
		nc,
	}

	static class CC_ae_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JAE)# / #(c:JNB)# / #(c:JNC)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_ae).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_ae)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_ae, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_e {
		[Comment("#(c:JE)#, #(c:CMOVE)#, #(c:SETE)#, #(c:LOOPE)#")]
		e,
		[Comment("#(c:JZ)#, #(c:CMOVZ)#, #(c:SETZ)#, #(c:LOOPZ)#")]
		z,
	}

	static class CC_e_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JE)# / #(c:JZ)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_e).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_e)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_e, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_ne {
		[Comment("#(c:JNE)#, #(c:CMOVNE)#, #(c:SETNE)#, #(c:LOOPNE)#")]
		ne,
		[Comment("#(c:JNZ)#, #(c:CMOVNZ)#, #(c:SETNZ)#, #(c:LOOPNZ)#")]
		nz,
	}

	static class CC_ne_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JNE)# / #(c:JNZ)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_ne).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_ne)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_ne, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_be {
		[Comment("#(c:JBE)#, #(c:CMOVBE)#, #(c:SETBE)#")]
		be,
		[Comment("#(c:JNA)#, #(c:CMOVNA)#, #(c:SETNA)#")]
		na,
	}

	static class CC_be_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JBE)# / #(c:JNA)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_be).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_be)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_be, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_a {
		[Comment("#(c:JA)#, #(c:CMOVA)#, #(c:SETA)#")]
		a,
		[Comment("#(c:JNBE)#, #(c:CMOVNBE)#, #(c:SETNBE)#")]
		nbe,
	}

	static class CC_a_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JA)# / #(c:JNBE)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_a).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_a)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_a, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_p {
		[Comment("#(c:JP)#, #(c:CMOVP)#, #(c:SETP)#")]
		p,
		[Comment("#(c:JPE)#, #(c:CMOVPE)#, #(c:SETPE)#")]
		pe,
	}

	static class CC_p_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JP)# / #(c:JPE)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_p).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_p)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_p, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_np {
		[Comment("#(c:JNP)#, #(c:CMOVNP)#, #(c:SETNP)#")]
		np,
		[Comment("#(c:JPO)#, #(c:CMOVPO)#, #(c:SETPO)#")]
		po,
	}

	static class CC_np_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JNP)# / #(c:JPO)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_np).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_np)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_np, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_l {
		[Comment("#(c:JL)#, #(c:CMOVL)#, #(c:SETL)#")]
		l,
		[Comment("#(c:JNGE)#, #(c:CMOVNGE)#, #(c:SETNGE)#")]
		nge,
	}

	static class CC_l_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JL)# / #(c:JNGE)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_l).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_l)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_l, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_ge {
		[Comment("#(c:JGE)#, #(c:CMOVGE)#, #(c:SETGE)#")]
		ge,
		[Comment("#(c:JNL)#, #(c:CMOVNL)#, #(c:SETNL)#")]
		nl,
	}

	static class CC_ge_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JGE)# / #(c:JNL)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_ge).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_ge)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_ge, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_le {
		[Comment("#(c:JLE)#, #(c:CMOVLE)#, #(c:SETLE)#")]
		le,
		[Comment("#(c:JNG)#, #(c:CMOVNG)#, #(c:SETNG)#")]
		ng,
	}

	static class CC_le_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JLE)# / #(c:JNG)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_le).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_le)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_le, documentation, GetValues(), EnumTypeFlags.Public);
	}

	enum CC_g {
		[Comment("#(c:JG)#, #(c:CMOVG)#, #(c:SETG)#")]
		g,
		[Comment("#(c:JNLE)#, #(c:CMOVNLE)#, #(c:SETNLE)#")]
		nle,
	}

	static class CC_g_Enum {
		const string documentation = "Mnemonic condition code selector (eg. #(c:JG)# / #(c:JNLE)#)";

		static EnumValue[] GetValues() =>
			typeof(CC_g).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(CC_g)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.CC_g, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
