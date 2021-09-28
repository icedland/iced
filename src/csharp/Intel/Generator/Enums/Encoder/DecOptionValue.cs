// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Encoder {
	[Enum("DecOptionValue")]
	enum DecOptionValue {
		None,
		ALTINST,
		Cl1invmb,
		Cmpxchg486A,
		Cyrix,
		Cyrix_DMI,
		Cyrix_SMINT_0F7E,
		Jmpe,
		Loadall286,
		Loadall386,
		MovTr,
		MPX,
		OldFpu,
		Pcommit,
		Umov,
		Xbts,
		Udbg,
		KNC,
	}
}
