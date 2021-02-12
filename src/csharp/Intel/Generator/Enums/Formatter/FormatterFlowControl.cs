// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

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
