// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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

		[Fact]
		public void xbegin_label() {
			TestAssembler(c => c.xbegin(CreateAndEmitLabel(c)), AssignLabel(Instruction.CreateXbegin(Bitness, FirstLabelId), FirstLabelId), TestInstrFlags.Branch);
		}

		[Fact]
		public void xbegin_offset() {
			TestAssembler(c => c.xbegin(12752), Instruction.CreateXbegin(Bitness, 12752), TestInstrFlags.BranchU64 | TestInstrFlags.IgnoreCode);
		}
	}
}
#endif
