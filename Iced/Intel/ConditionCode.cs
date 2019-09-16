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

#if !NO_INSTR_INFO
namespace Iced.Intel {
	/// <summary>
	/// Instruction condition code (used by jcc, setcc, cmovcc)
	/// </summary>
	public enum ConditionCode {
		/// <summary>
		/// The instruction doesn't have a condition code
		/// </summary>
		None,

		/// <summary>
		/// Overflow (OF=1)
		/// </summary>
		o,

		/// <summary>
		/// No overflow (OF=0)
		/// </summary>
		no,

		/// <summary>
		/// Below (unsigned) (CF=1)
		/// </summary>
		b,

		/// <summary>
		/// Above or equal (unsigned) (CF=0)
		/// </summary>
		ae,

		/// <summary>
		/// Equal / zero (ZF=1)
		/// </summary>
		e,

		/// <summary>
		/// Not equal / zero (ZF=0)
		/// </summary>
		ne,

		/// <summary>
		/// Below or equal (unsigned) (CF=1 or ZF=1)
		/// </summary>
		be,

		/// <summary>
		/// Above (unsigned) (CF=0 and ZF=0)
		/// </summary>
		a,

		/// <summary>
		/// Signed (SF=1)
		/// </summary>
		s,

		/// <summary>
		/// Not signed (SF=0)
		/// </summary>
		ns,

		/// <summary>
		/// Parity (PF=1)
		/// </summary>
		p,

		/// <summary>
		/// Not parity (PF=0)
		/// </summary>
		np,

		/// <summary>
		/// Less (signed) (SF!=OF)
		/// </summary>
		l,

		/// <summary>
		/// Greater than or equal (signed) (SF=OF)
		/// </summary>
		ge,

		/// <summary>
		/// Less than or equal (signed) (ZF=1 or SF!=OF)
		/// </summary>
		le,

		/// <summary>
		/// Greater (signed) (ZF=0 and SF=OF)
		/// </summary>
		g,
	}
}
#endif
