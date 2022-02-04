// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Iced.Intel.FormatterInternal {
	static partial class FormatterUtils {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNotrackPrefixBranch(Code code) {
			Static.Assert(Code.Jmp_rm16 + 1 == Code.Jmp_rm32 ? 0 : -1);
			Static.Assert(Code.Jmp_rm16 + 2 == Code.Jmp_rm64 ? 0 : -1);
			Static.Assert(Code.Call_rm16 + 1 == Code.Call_rm32 ? 0 : -1);
			Static.Assert(Code.Call_rm16 + 2 == Code.Call_rm64 ? 0 : -1);
			return (uint)code - (uint)Code.Jmp_rm16 <= 2 || (uint)code - (uint)Code.Call_rm16 <= 2;
		}

		static bool IsCode64(CodeSize codeSize) =>
			codeSize == CodeSize.Code64 || codeSize == CodeSize.Unknown;

		static Register GetDefaultSegmentRegister(in Instruction instruction) {
			var baseReg = instruction.MemoryBase;
			if (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP || baseReg == Register.RBP || baseReg == Register.RSP)
				return Register.SS;
			return Register.DS;
		}

		public static bool ShowSegmentPrefix(Register defaultSegReg, in Instruction instruction, bool showUselessPrefixes) {
			if (instruction.Code.IgnoresSegment())
				return showUselessPrefixes;
			var prefixSeg = instruction.SegmentPrefix;
			Debug.Assert(prefixSeg != Register.None);
			if (IsCode64(instruction.CodeSize)) {
				// ES,CS,SS,DS are ignored
				if (prefixSeg == Register.FS || prefixSeg == Register.GS)
					return true;
				return showUselessPrefixes;
			}
			else {
				if (defaultSegReg == Register.None)
					defaultSegReg = GetDefaultSegmentRegister(instruction);
				if (prefixSeg != defaultSegReg)
					return true;
				return showUselessPrefixes;
			}
		}

		public static bool ShowRepOrRepePrefix(Code code, bool showUselessPrefixes) {
			if (showUselessPrefixes || IsRepRepeRepneInstruction(code))
				return true;

			switch (code) {
			// We allow 'rep ret' too since some old code use it to work around an old AMD bug
			case Code.Retnw:
			case Code.Retnd:
			case Code.Retnq:
				return true;

			default:
				return showUselessPrefixes;
			}
		}

		public static bool ShowRepnePrefix(Code code, bool showUselessPrefixes) {
			// If it's a 'rep/repne' instruction, always show the prefix
			if (showUselessPrefixes || IsRepRepeRepneInstruction(code))
				return true;
			return showUselessPrefixes;
		}

		public static bool IsRepeOrRepneInstruction(Code code) {
			switch (code) {
			case Code.Cmpsb_m8_m8:
			case Code.Cmpsw_m16_m16:
			case Code.Cmpsd_m32_m32:
			case Code.Cmpsq_m64_m64:
			case Code.Scasb_AL_m8:
			case Code.Scasw_AX_m16:
			case Code.Scasd_EAX_m32:
			case Code.Scasq_RAX_m64:
				return true;

			default:
				return false;
			}
		}

		static bool IsRepRepeRepneInstruction(Code code) {
			switch (code) {
			case Code.Insb_m8_DX:
			case Code.Insw_m16_DX:
			case Code.Insd_m32_DX:
			case Code.Outsb_DX_m8:
			case Code.Outsw_DX_m16:
			case Code.Outsd_DX_m32:
			case Code.Movsb_m8_m8:
			case Code.Movsw_m16_m16:
			case Code.Movsd_m32_m32:
			case Code.Movsq_m64_m64:
			case Code.Cmpsb_m8_m8:
			case Code.Cmpsw_m16_m16:
			case Code.Cmpsd_m32_m32:
			case Code.Cmpsq_m64_m64:
			case Code.Stosb_m8_AL:
			case Code.Stosw_m16_AX:
			case Code.Stosd_m32_EAX:
			case Code.Stosq_m64_RAX:
			case Code.Lodsb_AL_m8:
			case Code.Lodsw_AX_m16:
			case Code.Lodsd_EAX_m32:
			case Code.Lodsq_RAX_m64:
			case Code.Scasb_AL_m8:
			case Code.Scasw_AX_m16:
			case Code.Scasd_EAX_m32:
			case Code.Scasq_RAX_m64:
			case Code.Montmul_16:
			case Code.Montmul_32:
			case Code.Montmul_64:
			case Code.Xsha1_16:
			case Code.Xsha1_32:
			case Code.Xsha1_64:
			case Code.Xsha256_16:
			case Code.Xsha256_32:
			case Code.Xsha256_64:
			case Code.Xstore_16:
			case Code.Xstore_32:
			case Code.Xstore_64:
			case Code.Xcryptecb_16:
			case Code.Xcryptecb_32:
			case Code.Xcryptecb_64:
			case Code.Xcryptcbc_16:
			case Code.Xcryptcbc_32:
			case Code.Xcryptcbc_64:
			case Code.Xcryptctr_16:
			case Code.Xcryptctr_32:
			case Code.Xcryptctr_64:
			case Code.Xcryptcfb_16:
			case Code.Xcryptcfb_32:
			case Code.Xcryptcfb_64:
			case Code.Xcryptofb_16:
			case Code.Xcryptofb_32:
			case Code.Xcryptofb_64:
			case Code.Ccs_hash_16:
			case Code.Ccs_hash_32:
			case Code.Ccs_hash_64:
			case Code.Ccs_encrypt_16:
			case Code.Ccs_encrypt_32:
			case Code.Ccs_encrypt_64:
			case Code.Via_undoc_F30FA6F0_16:
			case Code.Via_undoc_F30FA6F0_32:
			case Code.Via_undoc_F30FA6F0_64:
			case Code.Via_undoc_F30FA6F8_16:
			case Code.Via_undoc_F30FA6F8_32:
			case Code.Via_undoc_F30FA6F8_64:
			case Code.Xsha512_16:
			case Code.Xsha512_32:
			case Code.Xsha512_64:
			case Code.Xstore_alt_16:
			case Code.Xstore_alt_32:
			case Code.Xstore_alt_64:
			case Code.Xsha512_alt_16:
			case Code.Xsha512_alt_32:
			case Code.Xsha512_alt_64:
				return true;

			default:
				return false;
			}
		}
	}
}
#endif
