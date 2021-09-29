// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums.Decoder {
	[Enum("DecoderOptions", Documentation = "Decoder options", Public = true, Flags = true, NoInitialize = true)]
	enum DecoderOptions {
		[Comment("No option is enabled")]
		None					= 0x00000000,
		[Comment("Disable some checks for invalid encodings of instructions, eg. most instructions can't use a #(c:LOCK)# prefix so if one is found, they're decoded as #(e:Code.INVALID)# unless this option is enabled.")]
		NoInvalidCheck			= 0x00000001,
		[Comment("AMD decoder: allow 16-bit branch/ret instructions in 64-bit mode, no #(c:o64 CALL/JMP FAR [mem], o64 LSS/LFS/LGS)#, #(c:UD0)# has no modr/m byte, decode #(c:LOCK MOV CR)#. The AMD decoder can still decode Intel instructions.")]
		AMD						= 0x00000002,
		[Comment("Decode opcodes #(c:0F0D)# and #(c:0F18-0F1F)# as reserved-nop instructions (eg. #(e:Code.Reservednop_rm32_r32_0F1D)#)")]
		ForceReservedNop		= 0x00000004,
		[Comment("Decode #(c:UMOV)# instructions")]
		Umov					= 0x00000008,
		[Comment("Decode #(c:XBTS)#/#(c:IBTS)#")]
		Xbts					= 0x00000010,
		[Comment("Decode #(c:0FA6)#/#(c:0FA7)# as #(c:CMPXCHG)#")]
		Cmpxchg486A				= 0x00000020,
		[Comment("Decode some old removed FPU instructions (eg. #(c:FRSTPM)#)")]
		OldFpu					= 0x00000040,
		[Comment("Decode #(c:PCOMMIT)#")]
		Pcommit					= 0x00000080,
		[Comment("Decode 286 #(c:STOREALL)#/#(c:LOADALL)# (#(c:0F04)# and #(c:0F05)#)")]
		Loadall286				= 0x00000100,
		[Comment("Decode 386 #(c:LOADALL)#")]
		Loadall386				= 0x00000200,
		[Comment("Decode #(c:CL1INVMB)#")]
		Cl1invmb				= 0x00000400,
		[Comment("Decode #(c:MOV r32,tr)# and #(c:MOV tr,r32)#")]
		MovTr					= 0x00000800,
		[Comment("Decode #(c:JMPE)# instructions")]
		Jmpe					= 0x00001000,
		[Comment("Don't decode #(c:PAUSE)#, decode #(c:NOP)# instead")]
		NoPause					= 0x00002000,
		[Comment("Don't decode #(c:WBNOINVD)#, decode #(c:WBINVD)# instead")]
		NoWbnoinvd				= 0x00004000,
		[Comment("Decode undocumented Intel #(c:RDUDBG)# and #(c:WRUDBG)# instructions")]
		Udbg					= 0x00008000,
		[Comment("Don't decode #(c:TZCNT)#, decode #(c:BSF)# instead")]
		NoMPFX_0FBC				= 0x00010000,
		[Comment("Don't decode #(c:LZCNT)#, decode #(c:BSR)# instead")]
		NoMPFX_0FBD				= 0x00020000,
		[Comment("Don't decode #(c:LAHF)# and #(c:SAHF)# in 64-bit mode")]
		NoLahfSahf64			= 0x00040000,
		[Comment("Decode #(c:MPX)# instructions")]
		MPX						= 0x00080000,
		[Comment("Decode most Cyrix instructions: #(c:FPU)#, #(c:EMMI)#, #(c:SMM)#, #(c:DDI)#")]
		Cyrix					= 0x00100000,
		[Comment("Decode Cyrix #(c:SMINT 0F7E)# (Cyrix 6x86 or earlier)")]
		Cyrix_SMINT_0F7E		= 0x00200000,
		[Comment("Decode Cyrix #(c:DMI)# instructions (AMD Geode GX/LX)")]
		Cyrix_DMI				= 0x00400000,
		[Comment("Decode Centaur #(c:ALTINST)#")]
		ALTINST					= 0x00800000,
		[Comment("Decode Intel Knights Corner instructions",
			Rust = "Decode Intel Knights Corner instructions (requires the #(c:mvex)# feature)",
			RustJS = "Decode Intel Knights Corner instructions (requires the #(c:mvex)# feature)")]
		KNC						= 0x01000000,
	}
}
