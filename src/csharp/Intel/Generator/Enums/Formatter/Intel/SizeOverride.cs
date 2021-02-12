// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

namespace Generator.Enums.Formatter.Intel {
	[Enum(nameof(SizeOverride), "IntelSizeOverride", NoInitialize = true)]
	enum SizeOverride {
		None,
		Size16,
		Size32,
		Size64,
	}
}
