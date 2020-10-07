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
using Generator.Tables;

namespace Generator.Enums.Encoder {
	[Enum("OpCodeInfoFlags2", Flags = true, NoInitialize = true)]
	enum OpCodeInfoFlags2 : uint {
		None					= 0,
		RealMode				= 0x00000001,
		ProtectedMode			= 0x00000002,
		Virtual8086Mode			= 0x00000004,
		CompatibilityMode		= 0x00000008,
		UseOutsideSmm			= 0x00000010,
		UseInSmm				= 0x00000020,
		UseOutsideEnclaveSgx	= 0x00000040,
		UseInEnclaveSgx1		= 0x00000080,
		UseInEnclaveSgx2		= 0x00000100,
		UseOutsideVmxOp			= 0x00000200,
		UseInVmxRootOp			= 0x00000400,
		UseInVmxNonRootOp		= 0x00000800,
		UseOutsideSeam			= 0x00001000,
		UseInSeam				= 0x00002000,
		TdxNonRootGenUd			= 0x00004000,
		TdxNonRootGenVe			= 0x00008000,
		TdxNonRootMayGenEx		= 0x00010000,
		IntelVmExit				= 0x00020000,
		IntelMayVmExit			= 0x00040000,
		IntelSmmVmExit			= 0x00080000,
		AmdVmExit				= 0x00100000,
		AmdMayVmExit			= 0x00200000,
		TsxAbort				= 0x00400000,
		TsxImplAbort			= 0x00800000,
		TsxMayAbort				= 0x01000000,
		IntelDecoder16or32		= 0x02000000,
		IntelDecoder64			= 0x04000000,
		AmdDecoder16or32		= 0x08000000,
		AmdDecoder64			= 0x10000000,
		/// <summary><see cref="InstrStrFmtOption"/></summary>
		InstrStrFmtOptionMask	= 7,
		InstrStrFmtOptionShift	= 29,
	}
}
