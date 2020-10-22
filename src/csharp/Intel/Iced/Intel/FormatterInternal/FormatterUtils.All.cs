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
	}
}
#endif
