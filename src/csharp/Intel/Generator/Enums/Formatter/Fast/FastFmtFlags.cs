// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator.Enums.Formatter.Fast {
	[Enum(nameof(FastFmtFlags), Flags = true, NoInitialize = true)]
	[Flags]
	enum FastFmtFlags : byte {
		None					= 0,

		// Used by the deserializer: add a 'v' prefix to the mnemonic if set
		HasVPrefix				= 0x01,
		// Used by the deserializer: if set, everything else except the flags
		// (i.e., the mnemonic string index) is identical to the previous element.
		SameAsPrev				= 0x02,

		ForceMemSize			= 0x04,

		// Same as PseudoOpsKind except +1 is added to every value so 0 means 'not a pseudo ops instruction'
		// NOTE: it uses the highest bits of the flags so after shifting right, there's no need to mask out anything.
		PseudoOpsKindShift		= 3,
		cmpps					= (PseudoOpsKind.cmpps + 1) << PseudoOpsKindShift,
		vcmpps					= (PseudoOpsKind.vcmpps + 1) << PseudoOpsKindShift,
		cmppd					= (PseudoOpsKind.cmppd + 1) << PseudoOpsKindShift,
		vcmppd					= (PseudoOpsKind.vcmppd + 1) << PseudoOpsKindShift,
		cmpss					= (PseudoOpsKind.cmpss + 1) << PseudoOpsKindShift,
		vcmpss					= (PseudoOpsKind.vcmpss + 1) << PseudoOpsKindShift,
		cmpsd					= (PseudoOpsKind.cmpsd + 1) << PseudoOpsKindShift,
		vcmpsd					= (PseudoOpsKind.vcmpsd + 1) << PseudoOpsKindShift,
		pclmulqdq				= (PseudoOpsKind.pclmulqdq + 1) << PseudoOpsKindShift,
		vpclmulqdq				= (PseudoOpsKind.vpclmulqdq + 1) << PseudoOpsKindShift,
		vpcomb					= (PseudoOpsKind.vpcomb + 1) << PseudoOpsKindShift,
		vpcomw					= (PseudoOpsKind.vpcomw + 1) << PseudoOpsKindShift,
		vpcomd					= (PseudoOpsKind.vpcomd + 1) << PseudoOpsKindShift,
		vpcomq					= (PseudoOpsKind.vpcomq + 1) << PseudoOpsKindShift,
		vpcomub					= (PseudoOpsKind.vpcomub + 1) << PseudoOpsKindShift,
		vpcomuw					= (PseudoOpsKind.vpcomuw + 1) << PseudoOpsKindShift,
		vpcomud					= (PseudoOpsKind.vpcomud + 1) << PseudoOpsKindShift,
		vpcomuq					= (PseudoOpsKind.vpcomuq + 1) << PseudoOpsKindShift,
		vpcmpb					= (PseudoOpsKind.vpcmpb + 1) << PseudoOpsKindShift,
		vpcmpw					= (PseudoOpsKind.vpcmpw + 1) << PseudoOpsKindShift,
		vpcmpd					= (PseudoOpsKind.vpcmpd + 1) << PseudoOpsKindShift,
		vpcmpq					= (PseudoOpsKind.vpcmpq + 1) << PseudoOpsKindShift,
		vpcmpub					= (PseudoOpsKind.vpcmpub + 1) << PseudoOpsKindShift,
		vpcmpuw					= (PseudoOpsKind.vpcmpuw + 1) << PseudoOpsKindShift,
		vpcmpud					= (PseudoOpsKind.vpcmpud + 1) << PseudoOpsKindShift,
		vpcmpuq					= (PseudoOpsKind.vpcmpuq + 1) << PseudoOpsKindShift,
		vcmpph					= (PseudoOpsKind.vcmpph + 1) << PseudoOpsKindShift,
		vcmpsh					= (PseudoOpsKind.vcmpsh + 1) << PseudoOpsKindShift,
		vcmpps8					= (PseudoOpsKind.vcmpps8 + 1) << PseudoOpsKindShift,
		vcmppd8					= (PseudoOpsKind.vcmppd8 + 1) << PseudoOpsKindShift,
		vpcmpd6					= (PseudoOpsKind.vpcmpd6 + 1) << PseudoOpsKindShift,
		// Not enough bits left
		//vpcmpud6				= (PseudoOpsKind.vpcmpud6 + 1) << PseudoOpsKindShift,
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class FastFmtFlagsEnum {
		FastFmtFlagsEnum(GenTypes genTypes) {
			var pseudoOpsKind = genTypes[TypeIds.PseudoOpsKind];
			if (pseudoOpsKind.Values.Length != 32)
				throw new InvalidOperationException("Update FastFmtFlags enum above with new values");
		}
	}
}
