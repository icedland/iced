/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using Generator.Enums;
using Generator.Enums.InstructionInfo;

namespace Generator.InstructionInfo {
	[Flags]
	enum InstrInfoFlags : uint {
		None					= 0,
		SaveRestore				= 0x00000001,
		StackInstruction		= 0x00000002,
		ProtectedMode			= 0x00000004,
		Privileged				= 0x00000008,
		NoSegmentRead			= 0x00000010,
		AVX2_Check				= 0x00000020,
		OpMaskRegReadWrite		= 0x00000040,
	}

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
	}

	enum CodeInfo {
		None,
		Cdq,
		Cdqe,
		Clzero,
		Cmps,
		Cmpxchg,
		Cmpxchg8b,
		Cpuid,
		Cqo,
		Cwd,
		Cwde,
		Div,
		Encls,
		Enter,
		Ins,
		Invlpga,
		Iret,
		Jrcxz,
		KP1,
		Lahf,
		Lds,
		Leave,
		Llwpcb,
		Loadall386,
		Lods,
		Loop,
		Maskmovq,
		Monitor,
		Montmul,
		Movdir64b,
		Movs,
		Mul,
		Mulx,
		Mwait,
		Mwaitx,
		Outs,
		PcmpXstrY,
		Pconfig,
		Pop_2,
		Pop_2_2,
		Pop_4,
		Pop_4_4,
		Pop_8,
		Pop_8_8,
		Pop_Ev,
		Popa,
		Push_2,
		Push_2_2,
		Push_4,
		Push_4_4,
		Push_8,
		Push_8_8,
		Pusha,
		R_AL_W_AH,
		R_AL_W_AX,
		R_CR0,
		R_EAX_ECX_EDX,
		R_EAX_EDX,
		R_ECX_W_EAX_EDX,
		R_ST0,
		R_ST0_R_ST1,
		R_ST0_RW_ST1,
		R_ST0_ST1,
		R_XMM0,
		Read_Reg8_OpM1,
		Read_Reg8_OpM1_imm,
		Read_Reg16_OpM1,
		Read_Reg16_OpM1_imm,
		R_EAX_EDX_Op0_GPR32,
		Invlpgb,
		RW_AL,
		RW_AX,
		RW_CR0,
		RW_ST0,
		RW_ST0_R_ST1,
		Salc,
		Scas,
		Shift_Ib_MASK1FMOD9,
		Shift_Ib_MASK1FMOD11,
		Shift_Ib_MASK1F,
		Shift_Ib_MASK3F,
		Clear_rflags,
		Clear_reg_regmem,
		Clear_reg_reg_regmem,
		Stos,
		Syscall,
		Umonitor,
		Vmfunc,
		Vmload,
		Vzeroall,
		W_EAX_ECX_EDX,
		W_EAX_EDX,
		W_ST0,
		Xbts,
		Xcrypt,
		Xsha,
		Xstore,
		Rmpadjust,
		Rmpupdate,
		Psmash,
		Pvalidate,
		CW_EAX,
		Arpl,
		Lea,
		Tilerelease,
	}

	sealed class InstrInfo {
		public EnumValue Code { get; }
		public CodeInfo CodeInfo { get; }
		public EnumValue Encoding { get; }
		public EnumValue FlowControl { get; }
		public RflagsBits RflagsRead { get; }
		public RflagsBits RflagsUndefined { get; }
		public RflagsBits RflagsWritten { get; }
		public RflagsBits RflagsCleared { get; }
		public RflagsBits RflagsSet { get; }
		public EnumValue? RflagsInfo { get; internal set; }
		public EnumValue[] Cpuid { get; }
		public EnumValue? CpuidInternal { get; internal set; }
		public InstrInfoFlags Flags { get; }
		public OpInfo[] OpInfo { get; }
		public EnumValue[] OpInfoEnum { get; }
		public InstrInfo(EnumValue code, CodeInfo codeInfo, EnumValue encoding, EnumValue flowControl, RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set, EnumValue[] cpuid, OpInfo[] opInfo, InstrInfoFlags flags) {
			Code = code;
			CodeInfo = codeInfo;
			Encoding = encoding;
			FlowControl = flowControl;
			RflagsRead = read;
			RflagsUndefined = undefined;
			RflagsWritten = written;
			RflagsCleared = cleared;
			RflagsSet = set;
			RflagsInfo = null;
			Cpuid = cpuid;
			CpuidInternal = null;
			opInfo = Create(opInfo);
			OpInfo = opInfo;
			OpInfoEnum = new EnumValue[opInfo.Length];
			Flags = flags;
		}

		static OpInfo[] Create(OpInfo[] a) {
			var res = new OpInfo[5];
			for (int i = 0; i < res.Length; i++) {
				OpInfo info;
				if (i < a.Length)
					info = a[i];
				else
					info = InstructionInfo.OpInfo.None;
				res[i] = info;
			}
			return res;
		}
	}
}
