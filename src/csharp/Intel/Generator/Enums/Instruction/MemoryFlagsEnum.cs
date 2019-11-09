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
	static class MemoryFlagsEnum {
		const string? documentation = null;

		/// <summary>
		/// [1:0]	= Scale
		/// [4:2]	= Size of displacement: 0, 1, 2, 4, 8
		/// [7:5]	= Segment register prefix: none, es, cs, ss, ds, fs, gs, reserved
		/// [14:8]	= Not used
		/// [15]	= Broadcasted memory
		/// </summary>
		[Flags]
		enum Enum : ushort {
			ScaleMask				= 3,
			DisplSizeShift			= 2,
			DisplSizeMask			= 7,
			SegmentPrefixShift		= 5,
			SegmentPrefixMask		= 7,
			// Unused bits here
			Broadcast				= 0x8000,
		}

		static EnumValue[] GetValues() =>
			typeof(Enum).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(Enum)a.GetValue(null)!, a.Name)).ToArray();

		public static readonly EnumType Instance = new EnumType("MemoryFlags", TypeIds.Instruction_MemoryFlags, documentation, GetValues(), EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
