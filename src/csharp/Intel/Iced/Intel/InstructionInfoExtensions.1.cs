// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System.Runtime.CompilerServices;
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	/// <summary>
	/// Extension methods
	/// </summary>
	public static partial class InstructionInfoExtensions {
		/// <summary>
		/// Gets the encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static EncodingKind Encoding(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2 + 1;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (EncodingKind)((data[index] >> (int)InfoFlags2.EncodingShift) & (uint)InfoFlags2.EncodingMask);
		}

		/// <summary>
		/// Gets the CPU or CPUID feature flags
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static CpuidFeature[] CpuidFeatures(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2 + 1;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			var cpuidFeature = (CpuidFeatureInternal)((data[index] >> (int)InfoFlags2.CpuidFeatureInternalShift) & (uint)InfoFlags2.CpuidFeatureInternalMask);
			return CpuidFeatureInternalData.ToCpuidFeatures[(int)cpuidFeature];
		}

		/// <summary>
		/// Gets control flow info
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FlowControl FlowControl(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2 + 1;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (FlowControl)((data[index] >> (int)InfoFlags2.FlowControlShift) & (uint)InfoFlags2.FlowControlMask);
		}

		/// <summary>
		/// Checks if it's a privileged instruction (all CPL=0 instructions (except <c>VMCALL</c>) and IOPL instructions <c>IN</c>, <c>INS</c>, <c>OUT</c>, <c>OUTS</c>, <c>CLI</c>, <c>STI</c>)
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPrivileged(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2 + 1;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (data[index] & (uint)InfoFlags2.Privileged) != 0;
		}

		/// <summary>
		/// Checks if this is an instruction that implicitly uses the stack pointer (<c>SP</c>/<c>ESP</c>/<c>RSP</c>), eg. <c>CALL</c>, <c>PUSH</c>, <c>POP</c>, <c>RET</c>, etc.
		/// See also <see cref="Instruction.StackPointerIncrement"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsStackInstruction(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2 + 1;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (data[index] & (uint)InfoFlags2.StackInstruction) != 0;
		}

		/// <summary>
		/// Checks if it's an instruction that saves or restores too many registers (eg. <c>FXRSTOR</c>, <c>XSAVE</c>, etc).
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSaveRestoreInstruction(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2 + 1;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (data[index] & (uint)InfoFlags2.SaveRestore) != 0;
		}

		/// <summary>
		/// Checks if it's a <c>Jcc NEAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJccNear(this Code code) =>
			(uint)(code - Code.Jo_rel16) <= (uint)(Code.Jg_rel32_64 - Code.Jo_rel16);

		/// <summary>
		/// Checks if it's a <c>Jcc SHORT</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJccShort(this Code code) =>
			(uint)(code - Code.Jo_rel8_16) <= (uint)(Code.Jg_rel8_64 - Code.Jo_rel8_16);

		/// <summary>
		/// Checks if it's a <c>JMP SHORT</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJmpShort(this Code code) =>
			(uint)(code - Code.Jmp_rel8_16) <= (uint)(Code.Jmp_rel8_64 - Code.Jmp_rel8_16);

		/// <summary>
		/// Checks if it's a <c>JMP NEAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJmpNear(this Code code) =>
			(uint)(code - Code.Jmp_rel16) <= (uint)(Code.Jmp_rel32_64 - Code.Jmp_rel16);

		/// <summary>
		/// Checks if it's a <c>JMP SHORT</c> or a <c>JMP NEAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJmpShortOrNear(this Code code) =>
			(uint)(code - Code.Jmp_rel8_16) <= (uint)(Code.Jmp_rel8_64 - Code.Jmp_rel8_16) ||
			(uint)(code - Code.Jmp_rel16) <= (uint)(Code.Jmp_rel32_64 - Code.Jmp_rel16);

		/// <summary>
		/// Checks if it's a <c>JMP FAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJmpFar(this Code code) =>
			(uint)(code - Code.Jmp_ptr1616) <= (uint)(Code.Jmp_ptr1632 - Code.Jmp_ptr1616);

		/// <summary>
		/// Checks if it's a <c>CALL NEAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCallNear(this Code code) =>
			(uint)(code - Code.Call_rel16) <= (uint)(Code.Call_rel32_64 - Code.Call_rel16);

		/// <summary>
		/// Checks if it's a <c>CALL FAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCallFar(this Code code) =>
			(uint)(code - Code.Call_ptr1616) <= (uint)(Code.Call_ptr1632 - Code.Call_ptr1616);

		/// <summary>
		/// Checks if it's a <c>JMP NEAR reg/[mem]</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJmpNearIndirect(this Code code) =>
			(uint)(code - Code.Jmp_rm16) <= (uint)(Code.Jmp_rm64 - Code.Jmp_rm16);

		/// <summary>
		/// Checks if it's a <c>JMP FAR [mem]</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJmpFarIndirect(this Code code) =>
			(uint)(code - Code.Jmp_m1616) <= (uint)(Code.Jmp_m1664 - Code.Jmp_m1616);

		/// <summary>
		/// Checks if it's a <c>CALL NEAR reg/[mem]</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCallNearIndirect(this Code code) =>
			(uint)(code - Code.Call_rm16) <= (uint)(Code.Call_rm64 - Code.Call_rm16);

		/// <summary>
		/// Checks if it's a <c>CALL FAR [mem]</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsCallFarIndirect(this Code code) =>
			(uint)(code - Code.Call_m1616) <= (uint)(Code.Call_m1664 - Code.Call_m1616);

#if MVEX
		/// <summary>
		/// Checks if it's a <c>JKccD SHORT</c> or <c>JKccD NEAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJkccShortOrNear(this Code code) =>
			code == Code.VEX_KNC_Jkzd_kr_rel32_64 || code == Code.VEX_KNC_Jknzd_kr_rel32_64 ||
			code == Code.VEX_KNC_Jkzd_kr_rel8_64 || code == Code.VEX_KNC_Jknzd_kr_rel8_64;

		/// <summary>
		/// Checks if it's a <c>JKccD NEAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJkccNear(this Code code) =>
			code == Code.VEX_KNC_Jkzd_kr_rel32_64 || code == Code.VEX_KNC_Jknzd_kr_rel32_64;

		/// <summary>
		/// Checks if it's a <c>JKccD SHORT</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJkccShort(this Code code) =>
			code == Code.VEX_KNC_Jkzd_kr_rel8_64 || code == Code.VEX_KNC_Jknzd_kr_rel8_64;
#endif

		/// <summary>
		/// Gets the condition code if it's <c>Jcc</c>, <c>SETcc</c>, <c>CMOVcc</c>, <c>CMPccXADD</c>, <c>LOOPcc</c> else
		/// <see cref="ConditionCode.None"/> is returned
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static ConditionCode ConditionCode(this Code code) {
			uint t;

			if ((t = (uint)(code - Code.Jo_rel16)) <= (uint)(Code.Jg_rel32_64 - Code.Jo_rel16) ||
				(t = (uint)(code - Code.Jo_rel8_16)) <= (uint)(Code.Jg_rel8_64 - Code.Jo_rel8_16) ||
				(t = (uint)(code - Code.Cmovo_r16_rm16)) <= (uint)(Code.Cmovg_r64_rm64 - Code.Cmovo_r16_rm16)) {
				return (int)(t / 3) + Intel.ConditionCode.o;
			}

			t = (uint)(code - Code.Seto_rm8);
			if (t <= (uint)(Code.Setg_rm8 - Code.Seto_rm8))
				return (int)t + Intel.ConditionCode.o;

			t = (uint)(code - Code.Loopne_rel8_16_CX);
			if (t <= (uint)(Code.Loopne_rel8_64_RCX - Code.Loopne_rel8_16_CX)) {
				return Intel.ConditionCode.ne;
			}

			t = (uint)(code - Code.Loope_rel8_16_CX);
			if (t <= (uint)(Code.Loope_rel8_64_RCX - Code.Loope_rel8_16_CX)) {
				return Intel.ConditionCode.e;
			}

			if ((t = (uint)(code - Code.VEX_Cmpoxadd_m32_r32_r32)) <= (uint)(Code.VEX_Cmpnlexadd_m64_r64_r64 - Code.VEX_Cmpoxadd_m32_r32_r32)) {
				return (int)(t / 2) + Intel.ConditionCode.o;
			}

#if MVEX
			switch (code) {
			case Code.VEX_KNC_Jkzd_kr_rel8_64:
			case Code.VEX_KNC_Jkzd_kr_rel32_64:
				return Intel.ConditionCode.e;
			case Code.VEX_KNC_Jknzd_kr_rel8_64:
			case Code.VEX_KNC_Jknzd_kr_rel32_64:
				return Intel.ConditionCode.ne;
			default:
				break;
			}
#endif

			return Intel.ConditionCode.None;
		}

		/// <summary>
		/// Checks if it's a string instruction such as <c>MOVS</c>, <c>LODS</c>, <c>STOS</c>, etc.
		/// </summary>
		/// <returns></returns>
		public static bool IsStringInstruction(this Code code) {
			switch (code) {
			// GENERATOR-BEGIN: IsStringOpTable
			// ⚠️This was generated by GENERATOR!🦹‍♂️
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
				return true;
			// GENERATOR-END: IsStringOpTable
				default:
					return false;
			}
		}

		/// <summary>
		/// Checks if it's a <c>JCXZ SHORT</c>, <c>JECXZ SHORT</c> or <c>JRCXZ SHORT</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJcxShort(this Code code) =>
			(uint)(code - Code.Jcxz_rel8_16) <= (uint)(Code.Jrcxz_rel8_64 - Code.Jcxz_rel8_16);

		/// <summary>
		/// Checks if it's a <c>LOOPcc SHORT</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsLoopcc(this Code code) =>
			(uint)(code - Code.Loopne_rel8_16_CX) <= (uint)(Code.Loope_rel8_64_RCX - Code.Loopne_rel8_16_CX);

		/// <summary>
		/// Checks if it's a <c>LOOP SHORT</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsLoop(this Code code) => 
			(uint)(code - Code.Loop_rel8_16_CX) <= (uint)(Code.Loop_rel8_64_RCX - Code.Loop_rel8_16_CX);
	}
}
#endif
