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

#if !NO_ENCODER
using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	public static class NonDecodedInstructions {
		public static IEnumerable<(int bitness, string hexBytes, Instruction instruction)> GetTests() {
			foreach (var info in Infos16)
				yield return (16, info.hexBytes, info.instruction);
			foreach (var info in Infos32)
				yield return (32, info.hexBytes, info.instruction);
			foreach (var info in Infos64)
				yield return (64, info.hexBytes, info.instruction);
		}

		public const int Infos16_Count = 19;
		public const int Infos32_Count = 19;
		public const int Infos64_Count = 18;

		public static readonly (string hexBytes, Instruction instruction)[] Infos16 = new (string hexBytes, Instruction instruction)[Infos16_Count] {
			("0F", Instruction.Create(Code.Popw_CS, Register.CS)),
			("9B D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B 66 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 66 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B DB E0", Instruction.Create(Code.Feni)),
			("9B DB E1", Instruction.Create(Code.Fdisi)),
			("9B DB E2", Instruction.Create(Code.Fclex)),
			("9B DB E3", Instruction.Create(Code.Finit)),
			("9B DB E4", Instruction.Create(Code.Fsetpm)),
			("9B DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B 66 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 66 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.None))),
			("9B 64 DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.BX, Register.SI, 1, 0, 0, false, Register.FS))),
			("9B DF E0", Instruction.Create(Code.Fstsw_AX, Register.AX)),
		};

		public static readonly (string hexBytes, Instruction instruction)[] Infos32 = new (string hexBytes, Instruction instruction)[Infos32_Count] {
			("66 0F", Instruction.Create(Code.Popw_CS, Register.CS)),
			("9B 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DB E0", Instruction.Create(Code.Feni)),
			("9B DB E1", Instruction.Create(Code.Fdisi)),
			("9B DB E2", Instruction.Create(Code.Fclex)),
			("9B DB E3", Instruction.Create(Code.Finit)),
			("9B DB E4", Instruction.Create(Code.Fsetpm)),
			("9B 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.EAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DF E0", Instruction.Create(Code.Fstsw_AX, Register.AX)),
		};

		public static readonly (string hexBytes, Instruction instruction)[] Infos64 = new (string hexBytes, Instruction instruction)[Infos64_Count] {
			("9B 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 D9 30", Instruction.Create(Code.Fstenv_m14byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 30", Instruction.Create(Code.Fstenv_m28byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 D9 38", Instruction.Create(Code.Fstcw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DB E0", Instruction.Create(Code.Feni)),
			("9B DB E1", Instruction.Create(Code.Fdisi)),
			("9B DB E2", Instruction.Create(Code.Fclex)),
			("9B DB E3", Instruction.Create(Code.Finit)),
			("9B DB E4", Instruction.Create(Code.Fsetpm)),
			("9B 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 66 DD 30", Instruction.Create(Code.Fsave_m94byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 30", Instruction.Create(Code.Fsave_m108byte, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.None))),
			("9B 64 DD 38", Instruction.Create(Code.Fstsw_m16, new MemoryOperand(Register.RAX, Register.None, 1, 0, 0, false, Register.FS))),
			("9B DF E0", Instruction.Create(Code.Fstsw_AX, Register.AX)),
		};
	}
}
#endif
