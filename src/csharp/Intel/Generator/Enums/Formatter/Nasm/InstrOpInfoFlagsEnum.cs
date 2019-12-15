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

namespace Generator.Enums.Formatter.Nasm {
	static class InstrOpInfoFlagsEnum {
		const string? documentation = null;

		[Flags]
		enum Enum : uint {
			None						= 0,

			// show no mem size
			MemSize_Nothing				= 1,

			// AlwaysShowMemorySize is disabled: always show memory size
			ShowNoMemSize_ForceSize		= 2,
			ShowMinMemSize_ForceSize	= 4,

			SizeOverrideMask			= 3,
			OpSizeShift					= 3,
			OpSize16					= SizeOverride.Size16 << (int)OpSizeShift,
			OpSize32					= SizeOverride.Size32 << (int)OpSizeShift,
			OpSize64					= SizeOverride.Size64 << (int)OpSizeShift,
			AddrSizeShift				= 5,
			AddrSize16					= SizeOverride.Size16 << (int)AddrSizeShift,
			AddrSize32					= SizeOverride.Size32 << (int)AddrSizeShift,
			AddrSize64					= SizeOverride.Size64 << (int)AddrSizeShift,
			BranchSizeInfoShift			= 7,
			BranchSizeInfoMask			= 7,
			BranchSizeInfo_Short		= BranchSizeInfo.Short << (int)BranchSizeInfoShift,
			SignExtendInfoShift			= 10,
			SignExtendInfoMask			= 7,
			MemorySizeInfoShift			= 13,
			MemorySizeInfoMask			= 3,
			FarMemorySizeInfoShift		= 15,
			FarMemorySizeInfoMask		= 3,
			RegisterTo					= 0x00020000,
			BndPrefix					= 0x00040000,
			MnemonicIsDirective			= 0x00080000,
			MemorySizeBits				= 8,
			MemorySizeShift				= 20,
			MemorySizeMask				= (1 << (int)MemorySizeBits) - 1,
		}

		static EnumValue[] GetValues() {
			ConstantUtils.VerifyMask<SizeOverride>((uint)Enum.SizeOverrideMask);
			ConstantUtils.VerifyMask<BranchSizeInfo>((uint)Enum.BranchSizeInfoMask);
			ConstantUtils.VerifyMask<SignExtendInfo>((uint)Enum.SignExtendInfoMask);
			ConstantUtils.VerifyMask<MemorySizeInfo>((uint)Enum.MemorySizeInfoMask);
			ConstantUtils.VerifyMask<FarMemorySizeInfo>((uint)Enum.FarMemorySizeInfoMask);
			ConstantUtils.VerifyMask<MemorySize>((uint)Enum.MemorySizeMask);
			return typeof(Enum).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(Enum)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();
		}

		public static readonly EnumType Instance = new EnumType("InstrOpInfoFlags", TypeIds.NasmInstrOpInfoFlags, documentation, GetValues(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
