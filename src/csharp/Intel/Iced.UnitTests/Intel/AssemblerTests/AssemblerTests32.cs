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

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public sealed partial class AssemblerTests32 {
		[Fact]
		void xlatb() {
			TestAssembler(c => c.xlatb(), Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.EBX, Register.AL)));
		}

		[Fact]
		void call_far() {
			TestAssembler(c => c.call(0x1234, 0x56789ABC), Instruction.CreateBranch(Code.Call_ptr1632, 0x1234, 0x56789ABC));
		}

		[Fact]
		void jmp_far() {
			TestAssembler(c => c.jmp(0x1234, 0x56789ABC), Instruction.CreateBranch(Code.Jmp_ptr1632, 0x1234, 0x56789ABC));
		}
	}
}
#endif
