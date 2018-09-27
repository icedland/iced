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

using System.Collections.Generic;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	static class NonDecodedInstructions {
		public static IEnumerable<(int bitness, string hexBytes, Instruction instruction)> GetTests() {
			foreach (var info in Infos16)
				yield return (16, info.hexBytes, info.instruction);
			foreach (var info in Infos32)
				yield return (32, info.hexBytes, info.instruction);
			foreach (var info in Infos64)
				yield return (64, info.hexBytes, info.instruction);
		}

		static readonly (string hexBytes, Instruction instruction)[] Infos16 = new (string hexBytes, Instruction instruction)[] {
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

		static readonly (string hexBytes, Instruction instruction)[] Infos32 = new (string hexBytes, Instruction instruction)[] {
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

		static readonly (string hexBytes, Instruction instruction)[] Infos64 = new (string hexBytes, Instruction instruction)[] {
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
