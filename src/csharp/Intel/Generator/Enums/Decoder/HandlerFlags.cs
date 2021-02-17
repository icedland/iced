// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
