// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

namespace Generator.Enums.Decoder {
	[Enum("HandlerFlags", Flags = true)]
	enum HandlerFlags {
		None,
		Xacquire,
		Xrelease,
		XacquireXreleaseNoLock,
		Lock,
	}
}
