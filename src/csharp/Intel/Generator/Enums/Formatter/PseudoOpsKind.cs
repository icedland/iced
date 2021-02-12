// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

namespace Generator.Enums.Formatter {
	[Enum("PseudoOpsKind")]
	enum PseudoOpsKind {
		cmpps,
		vcmpps,
		cmppd,
		vcmppd,
		cmpss,
		vcmpss,
		cmpsd,
		vcmpsd,
		pclmulqdq,
		vpclmulqdq,
		vpcomb,
		vpcomw,
		vpcomd,
		vpcomq,
		vpcomub,
		vpcomuw,
		vpcomud,
		vpcomuq,
	}
}
