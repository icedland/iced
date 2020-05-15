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
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	public static partial class InstructionInfoExtensions {
		/// <summary>
		/// Gets the encoding, eg. legacy, VEX, EVEX, ...
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
		/// Gets flow control info
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
		/// Checks if the instruction isn't available in real mode or virtual 8086 mode
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsProtectedMode(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (data[index] & (uint)InfoFlags1.ProtectedMode) != 0;
		}

		/// <summary>
		/// Checks if this is a privileged instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPrivileged(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (data[index] & (uint)InfoFlags1.Privileged) != 0;
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
			int index = (int)code * 2;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (data[index] & (uint)InfoFlags1.StackInstruction) != 0;
		}

		/// <summary>
		/// Checks if it's an instruction that saves or restores too many registers (eg. <c>FXRSTOR</c>, <c>XSAVE</c>, etc).
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSaveRestoreInstruction(this Code code) {
			var data = InstrInfoTable.Data;
			int index = (int)code * 2;
			if ((uint)index >= (uint)data.Length)
				ThrowHelper.ThrowArgumentOutOfRangeException_code();
			return (data[index] & (uint)InfoFlags1.SaveRestore) != 0;
		}

		/// <summary>
		/// Checks if it's a <c>Jcc SHORT</c> or <c>Jcc NEAR</c> instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsJccShortOrNear(this Code code) =>
			(uint)(code - Code.Jo_rel8_16) <= (uint)(Code.Jg_rel8_64 - Code.Jo_rel8_16) ||
			(uint)(code - Code.Jo_rel16) <= (uint)(Code.Jg_rel32_64 - Code.Jo_rel16);

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

		/// <summary>
		/// Gets the condition code if it's <c>Jcc</c>, <c>SETcc</c>, <c>CMOVcc</c> else <see cref="ConditionCode.None"/> is returned
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		[System.Obsolete("Use " + nameof(ConditionCode) + " instead of this method", true)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public static ConditionCode GetConditionCode(this Code code) => code.ConditionCode();

		/// <summary>
		/// Gets the condition code if it's <c>Jcc</c>, <c>SETcc</c>, <c>CMOVcc</c>, <c>LOOPcc</c> else
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

			return Intel.ConditionCode.None;
		}
	}
}
#endif
