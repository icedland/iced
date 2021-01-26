// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
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
