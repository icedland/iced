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

namespace Generator.InstructionInfo {
	enum OpInfo {
		None,
		CondRead,
		CondWrite,
		// CMOVcc with GPR32 dest in 64-bit mode: upper 32 bits of full 64-bit reg are always cleared.
		CondWrite32_ReadWrite64,
		NoMemAccess,
		Read,
		ReadCondWrite,
		ReadP3,
		ReadWrite,
		Write,
		// Writes to zmm, can get converted to rcw
		WriteVmm,
		ReadWriteVmm,
		// Don't convert Write to ReadWrite, eg. EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64 since it always overwrites dest
		WriteForce,
		WriteMem_ReadWriteReg,
	}
}
