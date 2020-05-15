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

#if INSTR_INFO
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	partial struct Instruction {
		/// <summary>
		/// Gets the number of bytes added to <c>SP</c>/<c>ESP</c>/<c>RSP</c> or 0 if it's not an instruction that pushes or pops data. This method assumes
		/// the instruction doesn't change the privilege level (eg. <c>IRET/D/Q</c>). If it's the <c>LEAVE</c> instruction, this method returns 0.
		/// </summary>
		/// <returns></returns>
		public readonly int StackPointerIncrement {
			get {
				switch (Code) {
				case Code.Pushw_ES:
				case Code.Pushw_CS:
				case Code.Pushw_SS:
				case Code.Pushw_DS:
				case Code.Push_r16:
				case Code.Push_imm16:
				case Code.Pushw_imm8:
				case Code.Pushfw:
				case Code.Push_rm16:
				case Code.Pushw_FS:
				case Code.Pushw_GS:
					return -2;

				case Code.Pushd_ES:
				case Code.Pushd_CS:
				case Code.Pushd_SS:
				case Code.Pushd_DS:
				case Code.Push_r32:
				case Code.Pushd_imm32:
				case Code.Pushd_imm8:
				case Code.Pushfd:
				case Code.Push_rm32:
				case Code.Pushd_FS:
				case Code.Pushd_GS:
					return -4;

				case Code.Push_r64:
				case Code.Pushq_imm32:
				case Code.Pushq_imm8:
				case Code.Pushfq:
				case Code.Push_rm64:
				case Code.Pushq_FS:
				case Code.Pushq_GS:
					return -8;

				case Code.Pushaw:
					return -2 * 8;

				case Code.Pushad:
					return -4 * 8;

				case Code.Popw_ES:
				case Code.Popw_CS:
				case Code.Popw_SS:
				case Code.Popw_DS:
				case Code.Pop_r16:
				case Code.Pop_rm16:
				case Code.Popfw:
				case Code.Popw_FS:
				case Code.Popw_GS:
					return 2;

				case Code.Popd_ES:
				case Code.Popd_SS:
				case Code.Popd_DS:
				case Code.Pop_r32:
				case Code.Pop_rm32:
				case Code.Popfd:
				case Code.Popd_FS:
				case Code.Popd_GS:
					return 4;

				case Code.Pop_r64:
				case Code.Pop_rm64:
				case Code.Popfq:
				case Code.Popq_FS:
				case Code.Popq_GS:
					return 8;

				case Code.Popaw:
					return 2 * 8;

				case Code.Popad:
					return 4 * 8;

				case Code.Call_ptr1616:
				case Code.Call_m1616:
					return -(2 + 2);

				case Code.Call_ptr1632:
				case Code.Call_m1632:
					return -(4 + 4);

				case Code.Call_m1664:
					return -(8 + 8);

				case Code.Call_rel16:
				case Code.Call_rm16:
					return -2;

				case Code.Call_rel32_32:
				case Code.Call_rm32:
					return -4;

				case Code.Call_rel32_64:
				case Code.Call_rm64:
					return -8;

				case Code.Retnw_imm16:
					return 2 + Immediate16;

				case Code.Retnd_imm16:
					return 4 + Immediate16;

				case Code.Retnq_imm16:
					return 8 + Immediate16;

				case Code.Retnw:
					return 2;

				case Code.Retnd:
					return 4;

				case Code.Retnq:
					return 8;

				case Code.Retfw_imm16:
					return 2 + 2 + Immediate16;

				case Code.Retfd_imm16:
					return 4 + 4 + Immediate16;

				case Code.Retfq_imm16:
					return 8 + 8 + Immediate16;

				case Code.Retfw:
					return 2 + 2;

				case Code.Retfd:
					return 4 + 4;

				case Code.Retfq:
					return 8 + 8;

				case Code.Iretw:
					if (CodeSize == CodeSize.Code64)
						return 2 * 5;
					return 2 * 3;

				case Code.Iretd:
					if (CodeSize == CodeSize.Code64)
						return 4 * 5;
					return 4 * 3;

				case Code.Iretq:
					return 8 * 5;

				case Code.Enterw_imm16_imm8:
					return -(2 + (Immediate8_2nd & 0x1F) * 2 + Immediate16);

				case Code.Enterd_imm16_imm8:
					return -(4 + (Immediate8_2nd & 0x1F) * 4 + Immediate16);

				case Code.Enterq_imm16_imm8:
					return -(8 + (Immediate8_2nd & 0x1F) * 8 + Immediate16);

				case Code.Leavew:
				case Code.Leaved:
				case Code.Leaveq:
					return 0;

				default:
					return 0;
				}
			}
		}

		/// <summary>
		/// Instruction encoding, eg. legacy, VEX, EVEX, ...
		/// </summary>
		public readonly EncodingKind Encoding {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.Encoding();
		}

		/// <summary>
		/// Gets the CPU or CPUID feature flags
		/// </summary>
		public readonly CpuidFeature[] CpuidFeatures {
			get {
				var code = Code;
				uint flags2 = InstructionInfoInternal.InstrInfoTable.Data[(int)code * 2 + 1];
				var cpuidFeature = (InstructionInfoInternal.CpuidFeatureInternal)(flags2 >> (int)InstructionInfoInternal.InfoFlags2.CpuidFeatureInternalShift & (uint)InstructionInfoInternal.InfoFlags2.CpuidFeatureInternalMask);
				if ((flags2 & (uint)InstructionInfoInternal.InfoFlags2.AVX2_Check) != 0 && Op1Kind == OpKind.Register)
					cpuidFeature = InstructionInfoInternal.CpuidFeatureInternal.AVX2;
				return InstructionInfoInternal.CpuidFeatureInternalData.ToCpuidFeatures[(int)cpuidFeature];
			}
		}

		/// <summary>
		/// Flow control info
		/// </summary>
		public readonly FlowControl FlowControl {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.FlowControl();
		}

		/// <summary>
		/// <see langword="true"/> if the instruction isn't available in real mode or virtual 8086 mode
		/// </summary>
		public readonly bool IsProtectedMode {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsProtectedMode();
		}

		/// <summary>
		/// <see langword="true"/> if this is a privileged instruction
		/// </summary>
		public readonly bool IsPrivileged {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsPrivileged();
		}

		/// <summary>
		/// <see langword="true"/> if this is an instruction that implicitly uses the stack pointer (<c>SP</c>/<c>ESP</c>/<c>RSP</c>), eg. <c>CALL</c>, <c>PUSH</c>, <c>POP</c>, <c>RET</c>, etc.
		/// See also <see cref="StackPointerIncrement"/>
		/// </summary>
		public readonly bool IsStackInstruction {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsStackInstruction();
		}

		/// <summary>
		/// <see langword="true"/> if it's an instruction that saves or restores too many registers (eg. <c>FXRSTOR</c>, <c>XSAVE</c>, etc).
		/// </summary>
		public readonly bool IsSaveRestoreInstruction {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsSaveRestoreInstruction();
		}

		readonly InstructionInfoInternal.RflagsInfo GetRflagsInfo() {
			var flags1 = InstructionInfoInternal.InstrInfoTable.Data[(int)Code << 1];
			var codeInfo = (InstructionInfoInternal.CodeInfo)((flags1 >> (int)InstructionInfoInternal.InfoFlags1.CodeInfoShift) & (uint)InstructionInfoInternal.InfoFlags1.CodeInfoMask);
			Static.Assert(InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 + 1 == InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD11 ? 0 : -1);
			Static.Assert(InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 + 2 == InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1F ? 0 : -1);
			Static.Assert(InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 + 3 == InstructionInfoInternal.CodeInfo.Shift_Ib_MASK3F ? 0 : -1);
			Static.Assert(InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 + 4 == InstructionInfoInternal.CodeInfo.Clear_rflags ? 0 : -1);
			switch ((uint)(codeInfo - InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9)) {
			case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 - InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9:
				if ((Immediate8 & 0x1F) % 9 == 0)
					return InstructionInfoInternal.RflagsInfo.None;
				break;

			case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD11 - InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9:
				if ((Immediate8 & 0x1F) % 17 == 0)
					return InstructionInfoInternal.RflagsInfo.None;
				break;

			case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1F - InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9:
				if ((Immediate8 & 0x1F) == 0)
					return InstructionInfoInternal.RflagsInfo.None;
				break;

			case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK3F - InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9:
				if ((Immediate8 & 0x3F) == 0)
					return InstructionInfoInternal.RflagsInfo.None;
				break;

			case InstructionInfoInternal.CodeInfo.Clear_rflags - InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9:
				if (Op0Register != Op1Register)
					break;
				if (Op0Kind != OpKind.Register || Op1Kind != OpKind.Register)
					break;
				return InstructionInfoInternal.RflagsInfo.C_cos_S_pz_U_a;
			}
			return (InstructionInfoInternal.RflagsInfo)((flags1 >> (int)InstructionInfoInternal.InfoFlags1.RflagsInfoShift) & (uint)InstructionInfoInternal.InfoFlags1.RflagsInfoMask);
		}

		/// <summary>
		/// All flags that are read by the CPU when executing the instruction. See also <see cref="RflagsModified"/>
		/// </summary>
		public readonly RflagsBits RflagsRead {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				// If the method call is used without a temp index, the jitter generates worse code.
				// It stores the array in a temp local, then it calls the method, and then it reads
				// the temp local and checks if we can read the array.
				int index = (int)GetRflagsInfo();
				return (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsRead[index];
			}
		}

		/// <summary>
		/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared. See also <see cref="RflagsModified"/>
		/// </summary>
		public readonly RflagsBits RflagsWritten {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				// See RflagsRead for the reason why a temp index is used here
				int index = (int)GetRflagsInfo();
				return (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsWritten[index];
			}
		}

		/// <summary>
		/// All flags that are always cleared by the CPU. See also <see cref="RflagsModified"/>
		/// </summary>
		public readonly RflagsBits RflagsCleared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				// See RflagsRead for the reason why a temp index is used here
				int index = (int)GetRflagsInfo();
				return (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsCleared[index];
			}
		}

		/// <summary>
		/// All flags that are always set by the CPU. See also <see cref="RflagsModified"/>
		/// </summary>
		public readonly RflagsBits RflagsSet {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				// See RflagsRead for the reason why a temp index is used here
				int index = (int)GetRflagsInfo();
				return (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsSet[index];
			}
		}

		/// <summary>
		/// All flags that are undefined after executing the instruction. See also <see cref="RflagsModified"/>
		/// </summary>
		public readonly RflagsBits RflagsUndefined {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				// See RflagsRead for the reason why a temp index is used here
				int index = (int)GetRflagsInfo();
				return (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsUndefined[index];
			}
		}

		/// <summary>
		/// All flags that are modified by the CPU. This is <see cref="RflagsWritten"/> + <see cref="RflagsCleared"/> + <see cref="RflagsSet"/> + <see cref="RflagsUndefined"/>
		/// </summary>
		public readonly RflagsBits RflagsModified {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				// See RflagsRead for the reason why a temp index is used here
				int index = (int)GetRflagsInfo();
				return (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsModified[index];
			}
		}

		/// <summary>
		/// Checks if it's a <c>Jcc SHORT</c> or <c>Jcc NEAR</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJccShortOrNear {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJccShortOrNear();
		}

		/// <summary>
		/// Checks if it's a <c>Jcc NEAR</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJccNear {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJccNear();
		}

		/// <summary>
		/// Checks if it's a <c>Jcc SHORT</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJccShort {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJccShort();
		}

		/// <summary>
		/// Checks if it's a <c>JMP SHORT</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJmpShort {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJmpShort();
		}

		/// <summary>
		/// Checks if it's a <c>JMP NEAR</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJmpNear {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJmpNear();
		}

		/// <summary>
		/// Checks if it's a <c>JMP SHORT</c> or a <c>JMP NEAR</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJmpShortOrNear {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJmpShortOrNear();
		}

		/// <summary>
		/// Checks if it's a <c>JMP FAR</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJmpFar {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJmpFar();
		}

		/// <summary>
		/// Checks if it's a <c>CALL NEAR</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsCallNear {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsCallNear();
		}

		/// <summary>
		/// Checks if it's a <c>CALL FAR</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsCallFar {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsCallFar();
		}

		/// <summary>
		/// Checks if it's a <c>JMP NEAR reg/[mem]</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJmpNearIndirect {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJmpNearIndirect();
		}

		/// <summary>
		/// Checks if it's a <c>JMP FAR [mem]</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsJmpFarIndirect {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsJmpFarIndirect();
		}

		/// <summary>
		/// Checks if it's a <c>CALL NEAR reg/[mem]</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsCallNearIndirect {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsCallNearIndirect();
		}

		/// <summary>
		/// Checks if it's a <c>CALL FAR [mem]</c> instruction
		/// </summary>
		/// <returns></returns>
		public readonly bool IsCallFarIndirect {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.IsCallFarIndirect();
		}

		/// <summary>
		/// Negates the condition code, eg. <c>JE</c> -> <c>JNE</c>. Can be used if it's <c>Jcc</c>, <c>SETcc</c>, <c>CMOVcc</c>, <c>LOOPcc</c>
		/// and does nothing if the instruction doesn't have a condition code.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NegateConditionCode() => Code = Code.NegateConditionCode();

		/// <summary>
		/// Converts <c>Jcc/JMP NEAR</c> to <c>Jcc/JMP SHORT</c> and does nothing if it's not a <c>Jcc/JMP NEAR</c> instruction
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ToShortBranch() => Code = Code.ToShortBranch();

		/// <summary>
		/// Converts <c>Jcc/JMP SHORT</c> to <c>Jcc/JMP NEAR</c> and does nothing if it's not a <c>Jcc/JMP SHORT</c> instruction
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ToNearBranch() => Code = Code.ToNearBranch();

		/// <summary>
		/// Gets the condition code if it's <c>Jcc</c>, <c>SETcc</c>, <c>CMOVcc</c>, <c>LOOPcc</c> else <see cref="ConditionCode.None"/> is returned
		/// </summary>
		public readonly ConditionCode ConditionCode {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Code.ConditionCode();
		}
	}
}
#endif
