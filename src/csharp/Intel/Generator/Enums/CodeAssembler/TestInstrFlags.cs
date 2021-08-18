// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums {
	[Enum("TestInstrFlags", Flags = true, NoInitialize = true)]
	enum TestInstrFlags {
		None					= 0,
		Fwait					= 0x00000001,
		PreferVex				= 0x00000002,
		PreferEvex				= 0x00000004,
		PreferShortBranch		= 0x00000008,
		PreferNearBranch		= 0x00000010,
		Branch					= 0x00000020,
		Broadcast				= 0x00000040,
		BranchU64				= 0x00000080,
		IgnoreCode				= 0x00000100,
		RemoveRepRepnePrefixes	= 0x00000200,
	}
}
