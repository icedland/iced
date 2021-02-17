// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Formatter {
	[Enum("FormatMnemonicOptions", Documentation = "Format mnemonic options", Public = true, Flags = true, NoInitialize = true)]
	[Flags]
	public enum FormatMnemonicOptions {
		[Comment("No option is set")]
		None				= 0,

		[Comment("Don't add any prefixes")]
		NoPrefixes			= 0x00000001,

		[Comment("Don't add the mnemonic")]
		NoMnemonic			= 0x00000002,
	}
}
