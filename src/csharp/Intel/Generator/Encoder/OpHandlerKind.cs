// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Encoder {
	enum OpHandlerKind {
		None,
		OpA,
		OpHx,
		OpI4,
		OpIb,
		OpId,
		OpImm,
		OpIq,
		OpIsX,
		OpIw,
		OpJ,
		OpJdisp,
		OpJx,
		OpModRM_reg,
		OpModRM_reg_mem,
		OpModRM_regF0,
		OpModRM_rm,
		OpModRM_rm_mem_only,
		OpModRM_rm_reg_only,
		OpMRBX,
		OpO,
		OprDI,
		OpReg,
		OpRegEmbed8,
		OpRegSTi,
		OpVsib,
		OpX,
		OpY,
	}
}
