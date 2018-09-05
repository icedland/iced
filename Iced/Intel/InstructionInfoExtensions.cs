/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_INSTR_INFO
using Iced.Intel.InstructionInfoInternal;

namespace Iced.Intel {
	/// <summary>
	/// Extension methods
	/// </summary>
	public static class InstructionInfoExtensions {
		/// <summary>
		/// Gets the encoding, eg. legacy, VEX, EVEX, ...
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static EncodingKind Encoding(this Code code) =>
			(EncodingKind)((InfoHandlers.Data[((int)code << 1) + 1] >> (int)InfoFlags2.EncodingShift) & (uint)InfoFlags2.EncodingMask);

		/// <summary>
		/// Gets the CPU or CPUID feature flag
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static CpuidFeature CpuidFeature(this Code code) =>
			(CpuidFeature)((InfoHandlers.Data[((int)code << 1) + 1] >> (int)InfoFlags2.CpuidFeatureShift) & (uint)InfoFlags2.CpuidFeatureMask);

		/// <summary>
		/// Gets flow control info
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static FlowControl FlowControl(this Code code) =>
			(FlowControl)((InfoHandlers.Data[((int)code << 1) + 1] >> (int)InfoFlags2.FlowControlShift) & (uint)InfoFlags2.FlowControlMask);

		/// <summary>
		/// Checks if the instruction isn't available in real mode or virtual 8086 mode
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static bool ProtectedMode(this Code code) =>
			(InfoHandlers.Data[(int)code << 1] & (uint)InfoFlags1.ProtectedMode) != 0;

		/// <summary>
		/// Checks if this is a privileged instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static bool Privileged(this Code code) =>
			(InfoHandlers.Data[(int)code << 1] & (uint)InfoFlags1.Privileged) != 0;

		/// <summary>
		/// Checks if this is an instruction that implicitly uses the stack pointer (SP/ESP/RSP), eg. call, push, pop, ret, etc.
		/// See also <see cref="Instruction.StackPointerIncrement"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static bool StackInstruction(this Code code) =>
			(InfoHandlers.Data[(int)code << 1] & (uint)InfoFlags1.StackInstruction) != 0;

		/// <summary>
		/// Checks if it's an instruction that saves or restores too many registers (eg. fxrstor, xsave, etc).
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static bool SaveRestoreInstruction(this Code code) =>
			(InfoHandlers.Data[(int)code << 1] & (uint)InfoFlags1.SaveRestore) != 0;
	}
}
#endif
