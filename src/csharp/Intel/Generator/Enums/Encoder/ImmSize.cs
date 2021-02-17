// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Encoder {
	[Enum("ImmSize")]
	enum ImmSize {
		None,
		Size1,
		Size2,
		Size4,
		Size8,
		[Comment("#(c:ENTER xxxx,yy)#")]
		Size2_1,
		[Comment("#(c:EXTRQ/INSERTQ xx,yy)#")]
		Size1_1,
		[Comment("#(c:CALL16 FAR x:y)#")]
		Size2_2,
		[Comment("#(c:CALL32 FAR x:y)#")]
		Size4_2,
		RipRelSize1_Target16,
		RipRelSize1_Target32,
		RipRelSize1_Target64,
		RipRelSize2_Target16,
		RipRelSize2_Target32,
		RipRelSize2_Target64,
		RipRelSize4_Target32,
		RipRelSize4_Target64,
		SizeIbReg,
		Size1OpCode,
	}
}
