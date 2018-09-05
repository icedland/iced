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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;

namespace Iced.Intel {
	enum FormatterFlowControl {
		AlwaysShortBranch,
		ShortBranch,
		NearBranch,
		NearCall,
		FarBranch,
		FarCall,
		Xbegin,
	}

	static class FormatterUtils {
		static readonly string[] spaceStrings = new string[] {
			" ",
			"  ",
			"   ",
			"    ",
			"     ",
			"      ",
			"       ",
			"        ",
			"         ",
			"          ",
			"           ",
			"            ",
			"             ",
			"              ",
			"               ",
			"                ",
			"                 ",
			"                  ",
			"                   ",
			"                    ",
		};
		static readonly string[] tabStrings = new string[] {
			"\t",
			"\t\t",
			"\t\t\t",
			"\t\t\t\t",
			"\t\t\t\t\t",
			"\t\t\t\t\t\t",
		};

		public static void AddTabs(FormatterOutput output, int column, int firstOperandCharIndex, int tabSize) {
#if DEBUG
			for (int i = 0; i < spaceStrings.Length; i++)
				System.Diagnostics.Debug.Assert(spaceStrings[i].Length == i + 1);
			for (int i = 0; i < tabStrings.Length; i++)
				System.Diagnostics.Debug.Assert(tabStrings[i].Length == i + 1);
#endif
			const int max_firstOperandCharIndex = 256;
			if (firstOperandCharIndex < 0)
				firstOperandCharIndex = 0;
			else if (firstOperandCharIndex > max_firstOperandCharIndex)
				firstOperandCharIndex = max_firstOperandCharIndex;

			if (tabSize <= 0) {
				int charsLeft = firstOperandCharIndex - column;
				if (charsLeft <= 0)
					charsLeft = 1;
				AddStrings(output, spaceStrings, charsLeft);
			}
			else {
				int endCol = firstOperandCharIndex;
				if (endCol <= column)
					endCol = column + 1;
				int endColRoundDown = endCol / tabSize * tabSize;
				bool addedTabs = endColRoundDown > column;
				if (addedTabs) {
					int tabs = (endColRoundDown - (column / tabSize * tabSize)) / tabSize;
					AddStrings(output, tabStrings, tabs);
					column = endColRoundDown;
				}
				int spaces = firstOperandCharIndex - column;
				if (spaces > 0)
					AddStrings(output, spaceStrings, spaces);
				else if (!addedTabs)
					AddStrings(output, spaceStrings, 1);
			}
		}

		static void AddStrings(FormatterOutput output, string[] strings, int count) {
			while (count > 0) {
				int n = count;
				if (n >= strings.Length)
					n = strings.Length;
				output.Write(strings[n - 1], FormatterOutputTextKind.Text);
				count -= n;
			}
		}

		public static bool IsCall(FormatterFlowControl kind) => kind == FormatterFlowControl.NearCall || kind == FormatterFlowControl.FarCall;

		public static FormatterFlowControl GetFlowControl(ref Instruction instruction) {
			switch (instruction.Code) {
			case Code.Jo_Jb16:
			case Code.Jo_Jb32:
			case Code.Jo_Jb64:
			case Code.Jno_Jb16:
			case Code.Jno_Jb32:
			case Code.Jno_Jb64:
			case Code.Jb_Jb16:
			case Code.Jb_Jb32:
			case Code.Jb_Jb64:
			case Code.Jae_Jb16:
			case Code.Jae_Jb32:
			case Code.Jae_Jb64:
			case Code.Je_Jb16:
			case Code.Je_Jb32:
			case Code.Je_Jb64:
			case Code.Jne_Jb16:
			case Code.Jne_Jb32:
			case Code.Jne_Jb64:
			case Code.Jbe_Jb16:
			case Code.Jbe_Jb32:
			case Code.Jbe_Jb64:
			case Code.Ja_Jb16:
			case Code.Ja_Jb32:
			case Code.Ja_Jb64:

			case Code.Js_Jb16:
			case Code.Js_Jb32:
			case Code.Js_Jb64:
			case Code.Jns_Jb16:
			case Code.Jns_Jb32:
			case Code.Jns_Jb64:
			case Code.Jp_Jb16:
			case Code.Jp_Jb32:
			case Code.Jp_Jb64:
			case Code.Jnp_Jb16:
			case Code.Jnp_Jb32:
			case Code.Jnp_Jb64:
			case Code.Jl_Jb16:
			case Code.Jl_Jb32:
			case Code.Jl_Jb64:
			case Code.Jge_Jb16:
			case Code.Jge_Jb32:
			case Code.Jge_Jb64:
			case Code.Jle_Jb16:
			case Code.Jle_Jb32:
			case Code.Jle_Jb64:
			case Code.Jg_Jb16:
			case Code.Jg_Jb32:
			case Code.Jg_Jb64:

			case Code.Jmp_Jb16:
			case Code.Jmp_Jb32:
			case Code.Jmp_Jb64:
				return FormatterFlowControl.ShortBranch;

			case Code.Loopne_Jb16_CX:
			case Code.Loopne_Jb32_CX:
			case Code.Loopne_Jb16_ECX:
			case Code.Loopne_Jb32_ECX:
			case Code.Loopne_Jb64_ECX:
			case Code.Loopne_Jb64_RCX:
			case Code.Loope_Jb16_CX:
			case Code.Loope_Jb32_CX:
			case Code.Loope_Jb16_ECX:
			case Code.Loope_Jb32_ECX:
			case Code.Loope_Jb64_ECX:
			case Code.Loope_Jb64_RCX:
			case Code.Loop_Jb16_CX:
			case Code.Loop_Jb32_CX:
			case Code.Loop_Jb16_ECX:
			case Code.Loop_Jb32_ECX:
			case Code.Loop_Jb64_ECX:
			case Code.Loop_Jb64_RCX:
			case Code.Jcxz_Jb16:
			case Code.Jcxz_Jb32:
			case Code.Jecxz_Jb16:
			case Code.Jecxz_Jb32:
			case Code.Jecxz_Jb64:
			case Code.Jrcxz_Jb64:
				return FormatterFlowControl.AlwaysShortBranch;

			case Code.Call_Jw16:
			case Code.Call_Jd32:
			case Code.Call_Jd64:
				return FormatterFlowControl.NearCall;

			case Code.Jo_Jw16:
			case Code.Jo_Jd32:
			case Code.Jo_Jd64:
			case Code.Jno_Jw16:
			case Code.Jno_Jd32:
			case Code.Jno_Jd64:
			case Code.Jb_Jw16:
			case Code.Jb_Jd32:
			case Code.Jb_Jd64:
			case Code.Jae_Jw16:
			case Code.Jae_Jd32:
			case Code.Jae_Jd64:
			case Code.Je_Jw16:
			case Code.Je_Jd32:
			case Code.Je_Jd64:
			case Code.Jne_Jw16:
			case Code.Jne_Jd32:
			case Code.Jne_Jd64:
			case Code.Jbe_Jw16:
			case Code.Jbe_Jd32:
			case Code.Jbe_Jd64:
			case Code.Ja_Jw16:
			case Code.Ja_Jd32:
			case Code.Ja_Jd64:

			case Code.Js_Jw16:
			case Code.Js_Jd32:
			case Code.Js_Jd64:
			case Code.Jns_Jw16:
			case Code.Jns_Jd32:
			case Code.Jns_Jd64:
			case Code.Jp_Jw16:
			case Code.Jp_Jd32:
			case Code.Jp_Jd64:
			case Code.Jnp_Jw16:
			case Code.Jnp_Jd32:
			case Code.Jnp_Jd64:
			case Code.Jl_Jw16:
			case Code.Jl_Jd32:
			case Code.Jl_Jd64:
			case Code.Jge_Jw16:
			case Code.Jge_Jd32:
			case Code.Jge_Jd64:
			case Code.Jle_Jw16:
			case Code.Jle_Jd32:
			case Code.Jle_Jd64:
			case Code.Jg_Jw16:
			case Code.Jg_Jd32:
			case Code.Jg_Jd64:

			case Code.Jmp_Jw16:
			case Code.Jmp_Jd32:
			case Code.Jmp_Jd64:
				return FormatterFlowControl.NearBranch;

			case Code.Call_Adw:
			case Code.Call_Aww:
				return FormatterFlowControl.FarCall;

			case Code.Jmp_Adw:
			case Code.Jmp_Aww:
				return FormatterFlowControl.FarBranch;

			case Code.Xbegin_Jw16:
			case Code.Xbegin_Jd32:
			case Code.Xbegin_Jd64:
				return FormatterFlowControl.Xbegin;

			default:
				throw new InvalidOperationException();
			}
		}

		public static bool IsRepeOrRepneInstruction(Code code) {
			switch (code) {
			case Code.Cmpsb_Xb_Yb:
			case Code.Cmpsw_Xw_Yw:
			case Code.Cmpsd_Xd_Yd:
			case Code.Cmpsq_Xq_Yq:
			case Code.Scasb_AL_Yb:
			case Code.Scasw_AX_Yw:
			case Code.Scasd_EAX_Yd:
			case Code.Scasq_RAX_Yq:
				return true;

			default:
				return false;
			}
		}
	}
}
#endif
