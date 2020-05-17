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

namespace Generator.Enums.Formatter {
	[Enum("CC_b", Documentation = "Mnemonic condition code selector (eg. #(c:JB)# / #(c:JC)# / #(c:JNAE)#)", Public = true)]
	enum CC_b {
		[Comment("#(c:JB)#, #(c:CMOVB)#, #(c:SETB)#")]
		b,
		[Comment("#(c:JC)#, #(c:CMOVC)#, #(c:SETC)#")]
		c,
		[Comment("#(c:JNAE)#, #(c:CMOVNAE)#, #(c:SETNAE)#")]
		nae,
	}

	[Enum("CC_ae", Documentation = "Mnemonic condition code selector (eg. #(c:JAE)# / #(c:JNB)# / #(c:JNC)#)", Public = true)]
	enum CC_ae {
		[Comment("#(c:JAE)#, #(c:CMOVAE)#, #(c:SETAE)#")]
		ae,
		[Comment("#(c:JNB)#, #(c:CMOVNB)#, #(c:SETNB)#")]
		nb,
		[Comment("#(c:JNC)#, #(c:CMOVNC)#, #(c:SETNC)#")]
		nc,
	}

	[Enum("CC_e", Documentation = "Mnemonic condition code selector (eg. #(c:JE)# / #(c:JZ)#)", Public = true)]
	enum CC_e {
		[Comment("#(c:JE)#, #(c:CMOVE)#, #(c:SETE)#, #(c:LOOPE)#")]
		e,
		[Comment("#(c:JZ)#, #(c:CMOVZ)#, #(c:SETZ)#, #(c:LOOPZ)#")]
		z,
	}

	[Enum("CC_ne", Documentation = "Mnemonic condition code selector (eg. #(c:JNE)# / #(c:JNZ)#)", Public = true)]
	enum CC_ne {
		[Comment("#(c:JNE)#, #(c:CMOVNE)#, #(c:SETNE)#, #(c:LOOPNE)#")]
		ne,
		[Comment("#(c:JNZ)#, #(c:CMOVNZ)#, #(c:SETNZ)#, #(c:LOOPNZ)#")]
		nz,
	}

	[Enum("CC_be", Documentation = "Mnemonic condition code selector (eg. #(c:JBE)# / #(c:JNA)#)", Public = true)]
	enum CC_be {
		[Comment("#(c:JBE)#, #(c:CMOVBE)#, #(c:SETBE)#")]
		be,
		[Comment("#(c:JNA)#, #(c:CMOVNA)#, #(c:SETNA)#")]
		na,
	}

	[Enum("CC_a", Documentation = "Mnemonic condition code selector (eg. #(c:JA)# / #(c:JNBE)#)", Public = true)]
	enum CC_a {
		[Comment("#(c:JA)#, #(c:CMOVA)#, #(c:SETA)#")]
		a,
		[Comment("#(c:JNBE)#, #(c:CMOVNBE)#, #(c:SETNBE)#")]
		nbe,
	}

	[Enum("CC_p", Documentation = "Mnemonic condition code selector (eg. #(c:JP)# / #(c:JPE)#)", Public = true)]
	enum CC_p {
		[Comment("#(c:JP)#, #(c:CMOVP)#, #(c:SETP)#")]
		p,
		[Comment("#(c:JPE)#, #(c:CMOVPE)#, #(c:SETPE)#")]
		pe,
	}

	[Enum("CC_np", Documentation = "Mnemonic condition code selector (eg. #(c:JNP)# / #(c:JPO)#)", Public = true)]
	enum CC_np {
		[Comment("#(c:JNP)#, #(c:CMOVNP)#, #(c:SETNP)#")]
		np,
		[Comment("#(c:JPO)#, #(c:CMOVPO)#, #(c:SETPO)#")]
		po,
	}

	[Enum("CC_l", Documentation = "Mnemonic condition code selector (eg. #(c:JL)# / #(c:JNGE)#)", Public = true)]
	enum CC_l {
		[Comment("#(c:JL)#, #(c:CMOVL)#, #(c:SETL)#")]
		l,
		[Comment("#(c:JNGE)#, #(c:CMOVNGE)#, #(c:SETNGE)#")]
		nge,
	}

	[Enum("CC_ge", Documentation = "Mnemonic condition code selector (eg. #(c:JGE)# / #(c:JNL)#)", Public = true)]
	enum CC_ge {
		[Comment("#(c:JGE)#, #(c:CMOVGE)#, #(c:SETGE)#")]
		ge,
		[Comment("#(c:JNL)#, #(c:CMOVNL)#, #(c:SETNL)#")]
		nl,
	}

	[Enum("CC_le", Documentation = "Mnemonic condition code selector (eg. #(c:JLE)# / #(c:JNG)#)", Public = true)]
	enum CC_le {
		[Comment("#(c:JLE)#, #(c:CMOVLE)#, #(c:SETLE)#")]
		le,
		[Comment("#(c:JNG)#, #(c:CMOVNG)#, #(c:SETNG)#")]
		ng,
	}

	[Enum("CC_g", Documentation = "Mnemonic condition code selector (eg. #(c:JG)# / #(c:JNLE)#)", Public = true)]
	enum CC_g {
		[Comment("#(c:JG)#, #(c:CMOVG)#, #(c:SETG)#")]
		g,
		[Comment("#(c:JNLE)#, #(c:CMOVNLE)#, #(c:SETNLE)#")]
		nle,
	}
}
