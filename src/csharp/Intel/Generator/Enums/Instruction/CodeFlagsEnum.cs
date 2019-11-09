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

namespace Generator.Enums.Instruction {
	static class CodeFlagsEnum {
		const string? documentation = null;

		/// <summary>
		/// [12:0]	= <c>Code</c> enum
		/// [15:13]	= <c>RoundingControl</c> enum
		/// [18:16]	= Opmask register or 0 if none
		/// [22:19]	= Instruction length
		/// [24:23]	= Not used
		/// [25]	= Suppress all exceptions
		/// [26]	= Zeroing masking
		/// [27]	= xacquire prefix
		/// [28]	= xrelease prefix
		/// [29]	= repe prefix
		/// [30]	= repne prefix
		/// [31]	= lock prefix
		/// </summary>
		[Flags]
		enum Enum : uint {
			CodeBits				= 13,
			CodeMask				= (1 << (int)CodeBits) - 1,
			RoundingControlMask		= 7,
			RoundingControlShift	= 13,
			OpMaskMask				= 7,
			OpMaskShift				= 16,
			InstrLengthMask			= 0xF,
			InstrLengthShift		= 19,
			// Unused bits here
			SuppressAllExceptions	= 0x02000000,
			ZeroingMasking			= 0x04000000,
			XacquirePrefix			= 0x08000000,
			XreleasePrefix			= 0x10000000,
			RepePrefix				= 0x20000000,
			RepnePrefix				= 0x40000000,
			LockPrefix				= 0x80000000,

			// Bits ignored by Equals()
			EqualsIgnoreMask		= InstrLengthMask << (int)InstrLengthShift,
		}

		static EnumValue[] GetValues() =>
			typeof(Enum).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(Enum)a.GetValue(null)!, a.Name)).ToArray();

		public static readonly EnumType Instance = new EnumType("CodeFlags", EnumKind.Instruction_CodeFlags, documentation, GetValues(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
