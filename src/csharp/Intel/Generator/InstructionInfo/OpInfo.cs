// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.InstructionInfo {
	enum OpInfo {
		None,
		CondRead,
		CondWrite,
		// CMOVcc with GPR32 dest in 64-bit mode: upper 32 bits of full 64-bit reg are always cleared.
		CondWrite32_ReadWrite64,
		NoMemAccess,
		Read,
		ReadCondWrite,
		ReadP3,
		ReadWrite,
		Write,
		// Writes to zmm, can get converted to rcw
		WriteVmm,
		ReadWriteVmm,
		// Don't convert Write to ReadWrite, eg. EVEX_Vblendmpd_xmm_k1z_xmm_xmmm128b64 since it always overwrites dest
		WriteForce,
		WriteMem_ReadWriteReg,
		WriteForceP1,
	}
}
