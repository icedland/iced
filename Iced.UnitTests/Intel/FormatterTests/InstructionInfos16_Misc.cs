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
	static class InstructionInfos16_Misc {
		public const int AllInfos_Length = 116;
		public static readonly InstructionInfo[] AllInfos = new InstructionInfo[AllInfos_Length] {
			new InstructionInfo(16, "2E 70 00", Code.Jo_rel8_16),
			new InstructionInfo(16, "2E 71 00", Code.Jno_rel8_16),
			new InstructionInfo(16, "2E 72 00", Code.Jb_rel8_16),
			new InstructionInfo(16, "2E 73 00", Code.Jae_rel8_16),
			new InstructionInfo(16, "2E 74 00", Code.Je_rel8_16),
			new InstructionInfo(16, "2E 75 00", Code.Jne_rel8_16),
			new InstructionInfo(16, "2E 76 00", Code.Jbe_rel8_16),
			new InstructionInfo(16, "2E 77 00", Code.Ja_rel8_16),
			new InstructionInfo(16, "2E 78 00", Code.Js_rel8_16),
			new InstructionInfo(16, "2E 79 00", Code.Jns_rel8_16),
			new InstructionInfo(16, "2E 7A 00", Code.Jp_rel8_16),
			new InstructionInfo(16, "2E 7B 00", Code.Jnp_rel8_16),
			new InstructionInfo(16, "2E 7C 00", Code.Jl_rel8_16),
			new InstructionInfo(16, "2E 7D 00", Code.Jge_rel8_16),
			new InstructionInfo(16, "2E 7E 00", Code.Jle_rel8_16),
			new InstructionInfo(16, "2E 7F 00", Code.Jg_rel8_16),
			new InstructionInfo(16, "3E 70 00", Code.Jo_rel8_16),
			new InstructionInfo(16, "3E 71 00", Code.Jno_rel8_16),
			new InstructionInfo(16, "3E 72 00", Code.Jb_rel8_16),
			new InstructionInfo(16, "3E 73 00", Code.Jae_rel8_16),
			new InstructionInfo(16, "3E 74 00", Code.Je_rel8_16),
			new InstructionInfo(16, "3E 75 00", Code.Jne_rel8_16),
			new InstructionInfo(16, "3E 76 00", Code.Jbe_rel8_16),
			new InstructionInfo(16, "3E 77 00", Code.Ja_rel8_16),
			new InstructionInfo(16, "3E 78 00", Code.Js_rel8_16),
			new InstructionInfo(16, "3E 79 00", Code.Jns_rel8_16),
			new InstructionInfo(16, "3E 7A 00", Code.Jp_rel8_16),
			new InstructionInfo(16, "3E 7B 00", Code.Jnp_rel8_16),
			new InstructionInfo(16, "3E 7C 00", Code.Jl_rel8_16),
			new InstructionInfo(16, "3E 7D 00", Code.Jge_rel8_16),
			new InstructionInfo(16, "3E 7E 00", Code.Jle_rel8_16),
			new InstructionInfo(16, "3E 7F 00", Code.Jg_rel8_16),
			new InstructionInfo(16, "2E 0F80 0000", Code.Jo_rel16),
			new InstructionInfo(16, "2E 0F81 0000", Code.Jno_rel16),
			new InstructionInfo(16, "2E 0F82 0000", Code.Jb_rel16),
			new InstructionInfo(16, "2E 0F83 0000", Code.Jae_rel16),
			new InstructionInfo(16, "2E 0F84 0000", Code.Je_rel16),
			new InstructionInfo(16, "2E 0F85 0000", Code.Jne_rel16),
			new InstructionInfo(16, "2E 0F86 0000", Code.Jbe_rel16),
			new InstructionInfo(16, "2E 0F87 0000", Code.Ja_rel16),
			new InstructionInfo(16, "2E 0F88 0000", Code.Js_rel16),
			new InstructionInfo(16, "2E 0F89 0000", Code.Jns_rel16),
			new InstructionInfo(16, "2E 0F8A 0000", Code.Jp_rel16),
			new InstructionInfo(16, "2E 0F8B 0000", Code.Jnp_rel16),
			new InstructionInfo(16, "2E 0F8C 0000", Code.Jl_rel16),
			new InstructionInfo(16, "2E 0F8D 0000", Code.Jge_rel16),
			new InstructionInfo(16, "2E 0F8E 0000", Code.Jle_rel16),
			new InstructionInfo(16, "2E 0F8F 0000", Code.Jg_rel16),
			new InstructionInfo(16, "3E 0F80 0000", Code.Jo_rel16),
			new InstructionInfo(16, "3E 0F81 0000", Code.Jno_rel16),
			new InstructionInfo(16, "3E 0F82 0000", Code.Jb_rel16),
			new InstructionInfo(16, "3E 0F83 0000", Code.Jae_rel16),
			new InstructionInfo(16, "3E 0F84 0000", Code.Je_rel16),
			new InstructionInfo(16, "3E 0F85 0000", Code.Jne_rel16),
			new InstructionInfo(16, "3E 0F86 0000", Code.Jbe_rel16),
			new InstructionInfo(16, "3E 0F87 0000", Code.Ja_rel16),
			new InstructionInfo(16, "3E 0F88 0000", Code.Js_rel16),
			new InstructionInfo(16, "3E 0F89 0000", Code.Jns_rel16),
			new InstructionInfo(16, "3E 0F8A 0000", Code.Jp_rel16),
			new InstructionInfo(16, "3E 0F8B 0000", Code.Jnp_rel16),
			new InstructionInfo(16, "3E 0F8C 0000", Code.Jl_rel16),
			new InstructionInfo(16, "3E 0F8D 0000", Code.Jge_rel16),
			new InstructionInfo(16, "3E 0F8E 0000", Code.Jle_rel16),
			new InstructionInfo(16, "3E 0F8F 0000", Code.Jg_rel16),
			new InstructionInfo(16, "F2 70 00", Code.Jo_rel8_16),
			new InstructionInfo(16, "F2 71 00", Code.Jno_rel8_16),
			new InstructionInfo(16, "F2 72 00", Code.Jb_rel8_16),
			new InstructionInfo(16, "F2 73 00", Code.Jae_rel8_16),
			new InstructionInfo(16, "F2 74 00", Code.Je_rel8_16),
			new InstructionInfo(16, "F2 75 00", Code.Jne_rel8_16),
			new InstructionInfo(16, "F2 76 00", Code.Jbe_rel8_16),
			new InstructionInfo(16, "F2 77 00", Code.Ja_rel8_16),
			new InstructionInfo(16, "F2 78 00", Code.Js_rel8_16),
			new InstructionInfo(16, "F2 79 00", Code.Jns_rel8_16),
			new InstructionInfo(16, "F2 7A 00", Code.Jp_rel8_16),
			new InstructionInfo(16, "F2 7B 00", Code.Jnp_rel8_16),
			new InstructionInfo(16, "F2 7C 00", Code.Jl_rel8_16),
			new InstructionInfo(16, "F2 7D 00", Code.Jge_rel8_16),
			new InstructionInfo(16, "F2 7E 00", Code.Jle_rel8_16),
			new InstructionInfo(16, "F2 7F 00", Code.Jg_rel8_16),
			new InstructionInfo(16, "F2 0F80 0000", Code.Jo_rel16),
			new InstructionInfo(16, "F2 0F81 0000", Code.Jno_rel16),
			new InstructionInfo(16, "F2 0F82 0000", Code.Jb_rel16),
			new InstructionInfo(16, "F2 0F83 0000", Code.Jae_rel16),
			new InstructionInfo(16, "F2 0F84 0000", Code.Je_rel16),
			new InstructionInfo(16, "F2 0F85 0000", Code.Jne_rel16),
			new InstructionInfo(16, "F2 0F86 0000", Code.Jbe_rel16),
			new InstructionInfo(16, "F2 0F87 0000", Code.Ja_rel16),
			new InstructionInfo(16, "F2 0F88 0000", Code.Js_rel16),
			new InstructionInfo(16, "F2 0F89 0000", Code.Jns_rel16),
			new InstructionInfo(16, "F2 0F8A 0000", Code.Jp_rel16),
			new InstructionInfo(16, "F2 0F8B 0000", Code.Jnp_rel16),
			new InstructionInfo(16, "F2 0F8C 0000", Code.Jl_rel16),
			new InstructionInfo(16, "F2 0F8D 0000", Code.Jge_rel16),
			new InstructionInfo(16, "F2 0F8E 0000", Code.Jle_rel16),
			new InstructionInfo(16, "F2 0F8F 0000", Code.Jg_rel16),
			new InstructionInfo(16, "F2 E9 0000", Code.Jmp_rel16),
			new InstructionInfo(16, "F2 FF 20", Code.Jmp_rm16),
			new InstructionInfo(16, "F2 FF E0", Code.Jmp_rm16),
			new InstructionInfo(16, "F2 E8 0000", Code.Call_rel16),
			new InstructionInfo(16, "F2 FF 10", Code.Call_rm16),
			new InstructionInfo(16, "F2 FF D0", Code.Call_rm16),
			new InstructionInfo(16, "F2 C2 00 00", Code.Retnw_imm16),
			new InstructionInfo(16, "F2 C3", Code.Retnw),
			new InstructionInfo(16, "3E FF 10", Code.Call_rm16),
			new InstructionInfo(16, "3E FF 56 11", Code.Call_rm16),
			new InstructionInfo(16, "3E FF D1", Code.Call_rm16),
			new InstructionInfo(16, "3E FF 20", Code.Jmp_rm16),
			new InstructionInfo(16, "3E FF 66 11", Code.Jmp_rm16),
			new InstructionInfo(16, "3E FF E1", Code.Jmp_rm16),
			new InstructionInfo(16, "3E F2 FF D1", Code.Call_rm16),
			new InstructionInfo(16, "64 3E FF 10", Code.Call_rm16),
			new InstructionInfo(16, "3E 64 FF 10", Code.Call_rm16),
			new InstructionInfo(16, "3E F2 FF E1", Code.Jmp_rm16),
			new InstructionInfo(16, "64 3E FF 20", Code.Jmp_rm16),
			new InstructionInfo(16, "3E 64 FF 20", Code.Jmp_rm16),
		};
	}
}
#endif
