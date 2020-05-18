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

namespace Generator.Enums.Formatter.Gas {
	[Enum(nameof(InstrOpInfoFlags), "GasInstrOpInfoFlags", Flags = true, NoInitialize = true)]
	[Flags]
	internal enum InstrOpInfoFlags : ushort {
		None						= 0,
		MnemonicSuffixIfMem			= 1,
		SizeOverrideMask			= 3,
		OpSizeShift					= 1,
		OpSize16					= SizeOverride.Size16 << (int)OpSizeShift,
		OpSize32					= SizeOverride.Size32 << (int)OpSizeShift,
		OpSize64					= SizeOverride.Size64 << (int)OpSizeShift,
		AddrSizeShift				= 3,
		AddrSize16					= SizeOverride.Size16 << (int)AddrSizeShift,
		AddrSize32					= SizeOverride.Size32 << (int)AddrSizeShift,
		AddrSize64					= SizeOverride.Size64 << (int)AddrSizeShift,
		IndirectOperand				= 0x0020,
		OpSizeIsByteDirective		= 0x0040,
		KeepOperandOrder			= 0x0080,
		JccNotTaken					= 0x0100,
		JccTaken					= 0x0200,
		BndPrefix					= 0x0400,
		IgnoreIndexReg				= 0x0800,
		MnemonicIsDirective			= 0x1000,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrOpInfoFlagsEnum {
		InstrOpInfoFlagsEnum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<SizeOverride>((uint)InstrOpInfoFlags.SizeOverrideMask);
		}
	}
}
