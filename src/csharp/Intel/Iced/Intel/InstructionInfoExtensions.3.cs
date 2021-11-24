// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO || FAST_FMT
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	/// <summary>
	/// Extension methods
	/// </summary>
	public static partial class InstructionInfoExtensions {
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
		/// Checks if it's a <c>Jcxz SHORT</c>, <c>Jecxz SHORT</c> or <c>Jrcxz SHORT</c> instruction
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
