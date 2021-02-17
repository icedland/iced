// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Formatter {
	[Enum("FormatterFlowControl")]
	enum FormatterFlowControl {
		AlwaysShortBranch,
		ShortBranch,
		NearBranch,
		NearCall,
		FarBranch,
		FarCall,
		Xbegin,
	}
}
