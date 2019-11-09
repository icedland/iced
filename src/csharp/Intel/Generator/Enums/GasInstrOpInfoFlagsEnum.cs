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

namespace Generator.Enums {
	static class GasInstrOpInfoFlagsEnum {
		const string? documentation = null;

		[Flags]
		enum Enum : ushort {
			None						= 0,
			MnemonicSuffixIfMem			= 1,
			SizeOverrideMask			= 3,
			OpSizeShift					= 1,
			OpSize16					= GasSizeOverrideEnum.Enum.Size16 << (int)OpSizeShift,
			OpSize32					= GasSizeOverrideEnum.Enum.Size32 << (int)OpSizeShift,
			OpSize64					= GasSizeOverrideEnum.Enum.Size64 << (int)OpSizeShift,
			AddrSizeShift				= 3,
			AddrSize16					= GasSizeOverrideEnum.Enum.Size16 << (int)AddrSizeShift,
			AddrSize32					= GasSizeOverrideEnum.Enum.Size32 << (int)AddrSizeShift,
			AddrSize64					= GasSizeOverrideEnum.Enum.Size64 << (int)AddrSizeShift,
			IndirectOperand				= 0x0020,
			OpSizeIsByteDirective		= 0x0040,
			KeepOperandOrder			= 0x0080,
			JccNotTaken					= 0x0100,
			JccTaken					= 0x0200,
			BndPrefix					= 0x0400,
			IgnoreIndexReg				= 0x0800,
			MnemonicIsDirective			= 0x1000,
		}

		static EnumValue[] GetValues() =>
			typeof(Enum).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(Enum)a.GetValue(null)!, a.Name)).ToArray();

		public static readonly EnumType Instance = new EnumType("InstrOpInfoFlags", TypeIds.GasInstrOpInfoFlags, documentation, GetValues(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
