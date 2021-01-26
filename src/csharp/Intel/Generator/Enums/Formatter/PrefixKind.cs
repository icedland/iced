// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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
