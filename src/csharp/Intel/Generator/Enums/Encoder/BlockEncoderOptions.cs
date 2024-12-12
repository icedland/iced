// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Encoder {
	[Enum("BlockEncoderOptions", Documentation = "#(r:BlockEncoder)# options", Public = true, Flags = true, NoInitialize = true)]
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

		[Comment("The #(r:BlockEncoder)# should return new instruction offsets. For instructions that have been rewritten (e.g. to fix branches), the offset to the resulting block of instructions is returned.")]
		ReturnAllNewInstructionOffsets = 0x00000014
	}
}
