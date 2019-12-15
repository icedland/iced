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

namespace Generator.Enums.Formatter.Intel {
	static class InstrOpInfoFlagsEnum {
		const string? documentation = null;

		[Flags]
		enum Enum : ushort {
			None						= 0,

			// show no mem size
			MemSize_Nothing				= 1,

			// AlwaysShowMemorySize is disabled: always show memory size
			ShowNoMemSize_ForceSize		= 2,
			ShowMinMemSize_ForceSize	= 4,

			BranchSizeInfoShift			= 3,
			BranchSizeInfoMask			= 1,
			BranchSizeInfo_Short		= BranchSizeInfo.Short << (int)BranchSizeInfoShift,

			SizeOverrideMask			= 3,
			OpSizeShift					= 4,
			OpSize16					= SizeOverride.Size16 << (int)OpSizeShift,
			OpSize32					= SizeOverride.Size32 << (int)OpSizeShift,
			OpSize64					= SizeOverride.Size64 << (int)OpSizeShift,
			AddrSizeShift				= 6,
			AddrSize16					= SizeOverride.Size16 << (int)AddrSizeShift,
			AddrSize32					= SizeOverride.Size32 << (int)AddrSizeShift,
			AddrSize64					= SizeOverride.Size64 << (int)AddrSizeShift,

			IgnoreOpMask				= 0x00000100,
			FarMnemonic					= 0x00000200,
			JccNotTaken					= 0x00000400,
			JccTaken					= 0x00000800,
			BndPrefix					= 0x00001000,
			IgnoreIndexReg				= 0x00002000,
			IgnoreSegmentPrefix			= 0x00004000,
			MnemonicIsDirective			= 0x00008000,
		}

		static EnumValue[] GetValues() {
			ConstantUtils.VerifyMask<BranchSizeInfo>((uint)Enum.BranchSizeInfoMask);
			ConstantUtils.VerifyMask<SizeOverride>((uint)Enum.SizeOverrideMask);
			return typeof(Enum).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(Enum)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();
		}

		public static readonly EnumType Instance = new EnumType("InstrOpInfoFlags", TypeIds.IntelInstrOpInfoFlags, documentation, GetValues(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
