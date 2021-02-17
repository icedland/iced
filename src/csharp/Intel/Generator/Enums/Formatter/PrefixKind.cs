// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter {
	[Enum("PrefixKind", Documentation = "Prefix", Public = true)]
	enum PrefixKind {
		ES,
		CS,
		SS,
		DS,
		FS,
		GS,
		Lock,
		Rep,
		Repe,
		Repne,
		OperandSize,
		AddressSize,
		HintNotTaken,
		HintTaken,
		Bnd,
		Notrack,
		Xacquire,
		Xrelease,
	}
}
