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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
		static readonly string[] spaceStrings = CreateStrings(' ', 20);
		static readonly string[] tabStrings = CreateStrings('\t', 6);

		static string[] CreateStrings(char c, int max) {
			var strings = new string[max];
			for (int i = 0; i < strings.Length; i++)
				strings[i] = new string(c, i + 1);
			return strings;
		}

		public static void AddTabs(FormatterOutput output, int column, int firstOperandCharIndex, int tabSize) {
#if DEBUG
			for (int i = 0; i < spaceStrings.Length; i++)
				Debug.Assert(spaceStrings[i].Length == i + 1);
			for (int i = 0; i < tabStrings.Length; i++)
				Debug.Assert(tabStrings[i].Length == i + 1);
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

		public static FormatterFlowControl GetFlowControl(in Instruction instruction) {
			switch (instruction.Code) {
			case Code.Jo_rel8_16:
			case Code.Jo_rel8_32:
			case Code.Jo_rel8_64:
			case Code.Jno_rel8_16:
			case Code.Jno_rel8_32:
			case Code.Jno_rel8_64:
			case Code.Jb_rel8_16:
			case Code.Jb_rel8_32:
			case Code.Jb_rel8_64:
			case Code.Jae_rel8_16:
			case Code.Jae_rel8_32:
			case Code.Jae_rel8_64:
			case Code.Je_rel8_16:
			case Code.Je_rel8_32:
			case Code.Je_rel8_64:
			case Code.Jne_rel8_16:
			case Code.Jne_rel8_32:
			case Code.Jne_rel8_64:
			case Code.Jbe_rel8_16:
			case Code.Jbe_rel8_32:
			case Code.Jbe_rel8_64:
			case Code.Ja_rel8_16:
			case Code.Ja_rel8_32:
			case Code.Ja_rel8_64:

			case Code.Js_rel8_16:
			case Code.Js_rel8_32:
			case Code.Js_rel8_64:
			case Code.Jns_rel8_16:
			case Code.Jns_rel8_32:
			case Code.Jns_rel8_64:
			case Code.Jp_rel8_16:
			case Code.Jp_rel8_32:
			case Code.Jp_rel8_64:
			case Code.Jnp_rel8_16:
			case Code.Jnp_rel8_32:
			case Code.Jnp_rel8_64:
			case Code.Jl_rel8_16:
			case Code.Jl_rel8_32:
			case Code.Jl_rel8_64:
			case Code.Jge_rel8_16:
			case Code.Jge_rel8_32:
			case Code.Jge_rel8_64:
			case Code.Jle_rel8_16:
			case Code.Jle_rel8_32:
			case Code.Jle_rel8_64:
			case Code.Jg_rel8_16:
			case Code.Jg_rel8_32:
			case Code.Jg_rel8_64:

			case Code.Jmp_rel8_16:
			case Code.Jmp_rel8_32:
			case Code.Jmp_rel8_64:
				return FormatterFlowControl.ShortBranch;

			case Code.Loopne_rel8_16_CX:
			case Code.Loopne_rel8_32_CX:
			case Code.Loopne_rel8_16_ECX:
			case Code.Loopne_rel8_32_ECX:
			case Code.Loopne_rel8_64_ECX:
			case Code.Loopne_rel8_16_RCX:
			case Code.Loopne_rel8_64_RCX:
			case Code.Loope_rel8_16_CX:
			case Code.Loope_rel8_32_CX:
			case Code.Loope_rel8_16_ECX:
			case Code.Loope_rel8_32_ECX:
			case Code.Loope_rel8_64_ECX:
			case Code.Loope_rel8_16_RCX:
			case Code.Loope_rel8_64_RCX:
			case Code.Loop_rel8_16_CX:
			case Code.Loop_rel8_32_CX:
			case Code.Loop_rel8_16_ECX:
			case Code.Loop_rel8_32_ECX:
			case Code.Loop_rel8_64_ECX:
			case Code.Loop_rel8_16_RCX:
			case Code.Loop_rel8_64_RCX:
			case Code.Jcxz_rel8_16:
			case Code.Jcxz_rel8_32:
			case Code.Jecxz_rel8_16:
			case Code.Jecxz_rel8_32:
			case Code.Jecxz_rel8_64:
			case Code.Jrcxz_rel8_16:
			case Code.Jrcxz_rel8_64:
				return FormatterFlowControl.AlwaysShortBranch;

			case Code.Call_rel16:
			case Code.Call_rel32_32:
			case Code.Call_rel32_64:
				return FormatterFlowControl.NearCall;

			case Code.Jo_rel16:
			case Code.Jo_rel32_32:
			case Code.Jo_rel32_64:
			case Code.Jno_rel16:
			case Code.Jno_rel32_32:
			case Code.Jno_rel32_64:
			case Code.Jb_rel16:
			case Code.Jb_rel32_32:
			case Code.Jb_rel32_64:
			case Code.Jae_rel16:
			case Code.Jae_rel32_32:
			case Code.Jae_rel32_64:
			case Code.Je_rel16:
			case Code.Je_rel32_32:
			case Code.Je_rel32_64:
			case Code.Jne_rel16:
			case Code.Jne_rel32_32:
			case Code.Jne_rel32_64:
			case Code.Jbe_rel16:
			case Code.Jbe_rel32_32:
			case Code.Jbe_rel32_64:
			case Code.Ja_rel16:
			case Code.Ja_rel32_32:
			case Code.Ja_rel32_64:

			case Code.Js_rel16:
			case Code.Js_rel32_32:
			case Code.Js_rel32_64:
			case Code.Jns_rel16:
			case Code.Jns_rel32_32:
			case Code.Jns_rel32_64:
			case Code.Jp_rel16:
			case Code.Jp_rel32_32:
			case Code.Jp_rel32_64:
			case Code.Jnp_rel16:
			case Code.Jnp_rel32_32:
			case Code.Jnp_rel32_64:
			case Code.Jl_rel16:
			case Code.Jl_rel32_32:
			case Code.Jl_rel32_64:
			case Code.Jge_rel16:
			case Code.Jge_rel32_32:
			case Code.Jge_rel32_64:
			case Code.Jle_rel16:
			case Code.Jle_rel32_32:
			case Code.Jle_rel32_64:
			case Code.Jg_rel16:
			case Code.Jg_rel32_32:
			case Code.Jg_rel32_64:

			case Code.Jmp_rel16:
			case Code.Jmp_rel32_32:
			case Code.Jmp_rel32_64:

			case Code.Jmpe_disp16:
			case Code.Jmpe_disp32:
				return FormatterFlowControl.NearBranch;

			case Code.Call_ptr1632:
			case Code.Call_ptr1616:
				return FormatterFlowControl.FarCall;

			case Code.Jmp_ptr1632:
			case Code.Jmp_ptr1616:
				return FormatterFlowControl.FarBranch;

			case Code.Xbegin_rel16:
			case Code.Xbegin_rel32:
				return FormatterFlowControl.Xbegin;

			default:
				throw new InvalidOperationException();
			}
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNoTrackPrefixBranch(Code code) {
			Debug.Assert(Code.Jmp_rm16 + 1 == Code.Jmp_rm32);
			Debug.Assert(Code.Jmp_rm16 + 2 == Code.Jmp_rm64);
			Debug.Assert(Code.Call_rm16 + 1 == Code.Call_rm32);
			Debug.Assert(Code.Call_rm16 + 2 == Code.Call_rm64);
			return (uint)code - (uint)Code.Jmp_rm16 <= 2 || (uint)code - (uint)Code.Call_rm16 <= 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static PrefixKind GetSegmentRegisterPrefixKind(Register register) {
			Debug.Assert(register == Register.ES || register == Register.CS || register == Register.SS ||
						register == Register.DS || register == Register.FS || register == Register.GS);
			Debug.Assert(PrefixKind.ES + 1 == PrefixKind.CS);
			Debug.Assert(PrefixKind.ES + 2 == PrefixKind.SS);
			Debug.Assert(PrefixKind.ES + 3 == PrefixKind.DS);
			Debug.Assert(PrefixKind.ES + 4 == PrefixKind.FS);
			Debug.Assert(PrefixKind.ES + 5 == PrefixKind.GS);
			return (register - Register.ES) + PrefixKind.ES;
		}
	}
}
#endif
