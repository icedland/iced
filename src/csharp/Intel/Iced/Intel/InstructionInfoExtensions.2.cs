// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO || ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Extension methods
	/// </summary>
	public static partial class InstructionInfoExtensions {
		/// <summary>
		/// Negates the condition code, eg. <c>JE</c> -> <c>JNE</c>. Can be used if it's <c>Jcc</c>, <c>SETcc</c>, <c>CMOVcc</c>, <c>CMPccXADD</c>, <c>LOOPcc</c>
		/// and returns the original value if it's none of those instructions.
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static Code NegateConditionCode(this Code code) {
			uint t;

			if ((t = (uint)(code - Code.Jo_rel16)) <= (uint)(Code.Jg_rel32_64 - Code.Jo_rel16) ||
				(t = (uint)(code - Code.Jo_rel8_16)) <= (uint)(Code.Jg_rel8_64 - Code.Jo_rel8_16) ||
				(t = (uint)(code - Code.Cmovo_r16_rm16)) <= (uint)(Code.Cmovg_r64_rm64 - Code.Cmovo_r16_rm16)) {
				// They're ordered, eg. je_16, je_32, je_64, jne_16, jne_32, jne_64
				// if low 3, add 3, else if high 3, subtract 3.
				//return (((int)((t / 3) << 31) >> 31) | 1) * 3 + code;
				if (((t / 3) & 1) != 0)
					return code - 3;
				return code + 3;
			}

			t = (uint)(code - Code.Seto_rm8);
			if (t <= (uint)(Code.Setg_rm8 - Code.Seto_rm8))
				return (int)(t ^ 1) + Code.Seto_rm8;

			Static.Assert(Code.Loopne_rel8_16_CX + 7 == Code.Loope_rel8_16_CX ? 0 : -1);
			t = (uint)(code - Code.Loopne_rel8_16_CX);
			if (t <= (uint)(Code.Loope_rel8_64_RCX - Code.Loopne_rel8_16_CX)) {
				return Code.Loopne_rel8_16_CX + (int)((t + 7) % 14);
			}

			if ((t = (uint)(code - Code.VEX_Cmpoxadd_m32_r32_r32)) <= (uint)(Code.VEX_Cmpnlexadd_m64_r64_r64 - Code.VEX_Cmpoxadd_m32_r32_r32)) {
				if ((t & 2) != 0)
					return code - 2;
				return code + 2;
			}

#if MVEX
			switch (code) {
			case Code.VEX_KNC_Jkzd_kr_rel8_64: return Code.VEX_KNC_Jknzd_kr_rel8_64;
			case Code.VEX_KNC_Jknzd_kr_rel8_64: return Code.VEX_KNC_Jkzd_kr_rel8_64;
			case Code.VEX_KNC_Jkzd_kr_rel32_64: return Code.VEX_KNC_Jknzd_kr_rel32_64;
			case Code.VEX_KNC_Jknzd_kr_rel32_64: return Code.VEX_KNC_Jkzd_kr_rel32_64;
			default: break;
			}
#endif

			return code;
		}

		/// <summary>
		/// Converts <c>Jcc/JMP NEAR</c> to <c>Jcc/JMP SHORT</c>. Returns the input if it's not a <c>Jcc/JMP NEAR</c> instruction.
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static Code ToShortBranch(this Code code) {
			uint t;

			t = (uint)(code - Code.Jo_rel16);
			if (t <= (uint)(Code.Jg_rel32_64 - Code.Jo_rel16))
				return (int)t + Code.Jo_rel8_16;

			t = (uint)(code - Code.Jmp_rel16);
			if (t <= (uint)(Code.Jmp_rel32_64 - Code.Jmp_rel16))
				return (int)t + Code.Jmp_rel8_16;

#if MVEX
			switch (code) {
			case Code.VEX_KNC_Jkzd_kr_rel32_64: return Code.VEX_KNC_Jkzd_kr_rel8_64;
			case Code.VEX_KNC_Jknzd_kr_rel32_64: return Code.VEX_KNC_Jknzd_kr_rel8_64;
			default: break;
			}
#endif

			return code;
		}

		/// <summary>
		/// Converts <c>Jcc/JMP SHORT</c> to <c>Jcc/JMP NEAR</c>. Returns the input if it's not a <c>Jcc/JMP SHORT</c> instruction.
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static Code ToNearBranch(this Code code) {
			uint t;

			t = (uint)(code - Code.Jo_rel8_16);
			if (t <= (uint)(Code.Jg_rel8_64 - Code.Jo_rel8_16))
				return (int)t + Code.Jo_rel16;

			t = (uint)(code - Code.Jmp_rel8_16);
			if (t <= (uint)(Code.Jmp_rel8_64 - Code.Jmp_rel8_16))
				return (int)t + Code.Jmp_rel16;

#if MVEX
			switch (code) {
			case Code.VEX_KNC_Jkzd_kr_rel8_64: return Code.VEX_KNC_Jkzd_kr_rel32_64;
			case Code.VEX_KNC_Jknzd_kr_rel8_64: return Code.VEX_KNC_Jknzd_kr_rel32_64;
			default: break;
			}
#endif

			return code;
		}
	}
}
#endif
