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

namespace Generator.Enums.Encoder {
	[Flags]
	public enum BlockEncoderOptions : uint {
		[Comment("No option is set")]
		None						= 0,

		[Comment("By default, branches get updated if the target is too far away, eg. #(c:Jcc SHORT)# -> #(c:Jcc NEAR)# or if 64-bit mode, #(c:Jcc + JMP [RIP+mem])#. If this option is enabled, no branches are fixed.")]
		DontFixBranches				= 0x00000001,

		[Comment("The #(r:BlockEncoder)# should return #(r:RelocInfo)#s")]
		ReturnRelocInfos			= 0x00000002,

		[Comment("The #(r:BlockEncoder)# should return new instruction offsets")]
		ReturnNewInstructionOffsets	= 0x00000004,

		[Comment("The #(r:BlockEncoder)# should return #(r:ConstantOffsets)#")]
		ReturnConstantOffsets		= 0x00000008,
	}

	static class BlockEncoderOptionsEnum {
		const string documentation = "Encoder options";

		static EnumValue[] GetValues() =>
			typeof(BlockEncoderOptions).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(BlockEncoderOptions)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.BlockEncoderOptions, documentation, GetValues(), EnumTypeFlags.Public | EnumTypeFlags.Flags | EnumTypeFlags.NoInitialize);
	}
}
