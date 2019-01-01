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
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class InstructionInfos32_Misc {
		public const int AllInfos_Length = 116;
		public static readonly InstructionInfo[] AllInfos = new InstructionInfo[AllInfos_Length] {
			new InstructionInfo(32, "2E 70 00", Code.Jo_rel8_32),
			new InstructionInfo(32, "2E 71 00", Code.Jno_rel8_32),
			new InstructionInfo(32, "2E 72 00", Code.Jb_rel8_32),
			new InstructionInfo(32, "2E 73 00", Code.Jae_rel8_32),
			new InstructionInfo(32, "2E 74 00", Code.Je_rel8_32),
			new InstructionInfo(32, "2E 75 00", Code.Jne_rel8_32),
			new InstructionInfo(32, "2E 76 00", Code.Jbe_rel8_32),
			new InstructionInfo(32, "2E 77 00", Code.Ja_rel8_32),
			new InstructionInfo(32, "2E 78 00", Code.Js_rel8_32),
			new InstructionInfo(32, "2E 79 00", Code.Jns_rel8_32),
			new InstructionInfo(32, "2E 7A 00", Code.Jp_rel8_32),
			new InstructionInfo(32, "2E 7B 00", Code.Jnp_rel8_32),
			new InstructionInfo(32, "2E 7C 00", Code.Jl_rel8_32),
			new InstructionInfo(32, "2E 7D 00", Code.Jge_rel8_32),
			new InstructionInfo(32, "2E 7E 00", Code.Jle_rel8_32),
			new InstructionInfo(32, "2E 7F 00", Code.Jg_rel8_32),
			new InstructionInfo(32, "3E 70 00", Code.Jo_rel8_32),
			new InstructionInfo(32, "3E 71 00", Code.Jno_rel8_32),
			new InstructionInfo(32, "3E 72 00", Code.Jb_rel8_32),
			new InstructionInfo(32, "3E 73 00", Code.Jae_rel8_32),
			new InstructionInfo(32, "3E 74 00", Code.Je_rel8_32),
			new InstructionInfo(32, "3E 75 00", Code.Jne_rel8_32),
			new InstructionInfo(32, "3E 76 00", Code.Jbe_rel8_32),
			new InstructionInfo(32, "3E 77 00", Code.Ja_rel8_32),
			new InstructionInfo(32, "3E 78 00", Code.Js_rel8_32),
			new InstructionInfo(32, "3E 79 00", Code.Jns_rel8_32),
			new InstructionInfo(32, "3E 7A 00", Code.Jp_rel8_32),
			new InstructionInfo(32, "3E 7B 00", Code.Jnp_rel8_32),
			new InstructionInfo(32, "3E 7C 00", Code.Jl_rel8_32),
			new InstructionInfo(32, "3E 7D 00", Code.Jge_rel8_32),
			new InstructionInfo(32, "3E 7E 00", Code.Jle_rel8_32),
			new InstructionInfo(32, "3E 7F 00", Code.Jg_rel8_32),
			new InstructionInfo(32, "2E 0F80 00000000", Code.Jo_rel32_32),
			new InstructionInfo(32, "2E 0F81 00000000", Code.Jno_rel32_32),
			new InstructionInfo(32, "2E 0F82 00000000", Code.Jb_rel32_32),
			new InstructionInfo(32, "2E 0F83 00000000", Code.Jae_rel32_32),
			new InstructionInfo(32, "2E 0F84 00000000", Code.Je_rel32_32),
			new InstructionInfo(32, "2E 0F85 00000000", Code.Jne_rel32_32),
			new InstructionInfo(32, "2E 0F86 00000000", Code.Jbe_rel32_32),
			new InstructionInfo(32, "2E 0F87 00000000", Code.Ja_rel32_32),
			new InstructionInfo(32, "2E 0F88 00000000", Code.Js_rel32_32),
			new InstructionInfo(32, "2E 0F89 00000000", Code.Jns_rel32_32),
			new InstructionInfo(32, "2E 0F8A 00000000", Code.Jp_rel32_32),
			new InstructionInfo(32, "2E 0F8B 00000000", Code.Jnp_rel32_32),
			new InstructionInfo(32, "2E 0F8C 00000000", Code.Jl_rel32_32),
			new InstructionInfo(32, "2E 0F8D 00000000", Code.Jge_rel32_32),
			new InstructionInfo(32, "2E 0F8E 00000000", Code.Jle_rel32_32),
			new InstructionInfo(32, "2E 0F8F 00000000", Code.Jg_rel32_32),
			new InstructionInfo(32, "3E 0F80 00000000", Code.Jo_rel32_32),
			new InstructionInfo(32, "3E 0F81 00000000", Code.Jno_rel32_32),
			new InstructionInfo(32, "3E 0F82 00000000", Code.Jb_rel32_32),
			new InstructionInfo(32, "3E 0F83 00000000", Code.Jae_rel32_32),
			new InstructionInfo(32, "3E 0F84 00000000", Code.Je_rel32_32),
			new InstructionInfo(32, "3E 0F85 00000000", Code.Jne_rel32_32),
			new InstructionInfo(32, "3E 0F86 00000000", Code.Jbe_rel32_32),
			new InstructionInfo(32, "3E 0F87 00000000", Code.Ja_rel32_32),
			new InstructionInfo(32, "3E 0F88 00000000", Code.Js_rel32_32),
			new InstructionInfo(32, "3E 0F89 00000000", Code.Jns_rel32_32),
			new InstructionInfo(32, "3E 0F8A 00000000", Code.Jp_rel32_32),
			new InstructionInfo(32, "3E 0F8B 00000000", Code.Jnp_rel32_32),
			new InstructionInfo(32, "3E 0F8C 00000000", Code.Jl_rel32_32),
			new InstructionInfo(32, "3E 0F8D 00000000", Code.Jge_rel32_32),
			new InstructionInfo(32, "3E 0F8E 00000000", Code.Jle_rel32_32),
			new InstructionInfo(32, "3E 0F8F 00000000", Code.Jg_rel32_32),
			new InstructionInfo(32, "F2 70 00", Code.Jo_rel8_32),
			new InstructionInfo(32, "F2 71 00", Code.Jno_rel8_32),
			new InstructionInfo(32, "F2 72 00", Code.Jb_rel8_32),
			new InstructionInfo(32, "F2 73 00", Code.Jae_rel8_32),
			new InstructionInfo(32, "F2 74 00", Code.Je_rel8_32),
			new InstructionInfo(32, "F2 75 00", Code.Jne_rel8_32),
			new InstructionInfo(32, "F2 76 00", Code.Jbe_rel8_32),
			new InstructionInfo(32, "F2 77 00", Code.Ja_rel8_32),
			new InstructionInfo(32, "F2 78 00", Code.Js_rel8_32),
			new InstructionInfo(32, "F2 79 00", Code.Jns_rel8_32),
			new InstructionInfo(32, "F2 7A 00", Code.Jp_rel8_32),
			new InstructionInfo(32, "F2 7B 00", Code.Jnp_rel8_32),
			new InstructionInfo(32, "F2 7C 00", Code.Jl_rel8_32),
			new InstructionInfo(32, "F2 7D 00", Code.Jge_rel8_32),
			new InstructionInfo(32, "F2 7E 00", Code.Jle_rel8_32),
			new InstructionInfo(32, "F2 7F 00", Code.Jg_rel8_32),
			new InstructionInfo(32, "F2 0F80 00000000", Code.Jo_rel32_32),
			new InstructionInfo(32, "F2 0F81 00000000", Code.Jno_rel32_32),
			new InstructionInfo(32, "F2 0F82 00000000", Code.Jb_rel32_32),
			new InstructionInfo(32, "F2 0F83 00000000", Code.Jae_rel32_32),
			new InstructionInfo(32, "F2 0F84 00000000", Code.Je_rel32_32),
			new InstructionInfo(32, "F2 0F85 00000000", Code.Jne_rel32_32),
			new InstructionInfo(32, "F2 0F86 00000000", Code.Jbe_rel32_32),
			new InstructionInfo(32, "F2 0F87 00000000", Code.Ja_rel32_32),
			new InstructionInfo(32, "F2 0F88 00000000", Code.Js_rel32_32),
			new InstructionInfo(32, "F2 0F89 00000000", Code.Jns_rel32_32),
			new InstructionInfo(32, "F2 0F8A 00000000", Code.Jp_rel32_32),
			new InstructionInfo(32, "F2 0F8B 00000000", Code.Jnp_rel32_32),
			new InstructionInfo(32, "F2 0F8C 00000000", Code.Jl_rel32_32),
			new InstructionInfo(32, "F2 0F8D 00000000", Code.Jge_rel32_32),
			new InstructionInfo(32, "F2 0F8E 00000000", Code.Jle_rel32_32),
			new InstructionInfo(32, "F2 0F8F 00000000", Code.Jg_rel32_32),
			new InstructionInfo(32, "F2 E9 00000000", Code.Jmp_rel32_32),
			new InstructionInfo(32, "F2 FF 20", Code.Jmp_rm32),
			new InstructionInfo(32, "F2 FF E0", Code.Jmp_rm32),
			new InstructionInfo(32, "F2 E8 00000000", Code.Call_rel32_32),
			new InstructionInfo(32, "F2 FF 10", Code.Call_rm32),
			new InstructionInfo(32, "F2 FF D0", Code.Call_rm32),
			new InstructionInfo(32, "F2 C2 00 00", Code.Retnd_imm16),
			new InstructionInfo(32, "F2 C3", Code.Retnd),
			new InstructionInfo(32, "3E FF 10", Code.Call_rm32),
			new InstructionInfo(32, "3E FF 55 11", Code.Call_rm32),
			new InstructionInfo(32, "3E FF D1", Code.Call_rm32),
			new InstructionInfo(32, "3E FF 20", Code.Jmp_rm32),
			new InstructionInfo(32, "3E FF 65 11", Code.Jmp_rm32),
			new InstructionInfo(32, "3E FF E1", Code.Jmp_rm32),
			new InstructionInfo(32, "3E F2 FF D1", Code.Call_rm32),
			new InstructionInfo(32, "64 3E FF 10", Code.Call_rm32),
			new InstructionInfo(32, "3E 64 FF 10", Code.Call_rm32),
			new InstructionInfo(32, "3E F2 FF E1", Code.Jmp_rm32),
			new InstructionInfo(32, "64 3E FF 20", Code.Jmp_rm32),
			new InstructionInfo(32, "3E 64 FF 20", Code.Jmp_rm32),
		};
	}
}
#endif
