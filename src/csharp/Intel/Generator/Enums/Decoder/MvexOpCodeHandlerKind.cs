// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Decoder {
	[Enum("MvexOpCodeHandlerKind")]
	enum MvexOpCodeHandlerKind : byte {
		Invalid,
		Invalid2,
		Dup,
		HandlerReference,
		ArrayReference,
		RM,
		Group,
		W,
		MandatoryPrefix2,
		EH,
		M,
		MV,
		VW,
		HWIb,
		VWIb,
		VHW,
		VHWIb,
		VKW,
		KHW,
		KHWIb,
		VSIB,
		VSIB_V,
		V_VSIB,
	}
}
