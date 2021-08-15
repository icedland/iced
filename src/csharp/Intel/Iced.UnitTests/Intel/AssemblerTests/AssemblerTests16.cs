// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.AssemblerTests {
	public sealed partial class AssemblerTests16 {
		[Fact]
		void xlatb() {
			TestAssembler(c => c.xlatb(), Instruction.Create(Code.Xlat_m8, new MemoryOperand(Register.BX, Register.AL)));
		}

		[Fact]
		void call_far() {
			TestAssembler(c => c.call(0x1234, 0x5678), Instruction.CreateBranch(Code.Call_ptr1616, 0x1234, 0x5678));
		}

		[Fact]
		void jmp_far() {
			TestAssembler(c => c.jmp(0x1234, 0x5678), Instruction.CreateBranch(Code.Jmp_ptr1616, 0x1234, 0x5678));
		}

		[Fact]
		public void xbegin_label() {
			TestAssembler(c => c.xbegin(CreateAndEmitLabel(c)), AssignLabel(Instruction.CreateXbegin(Bitness, FirstLabelId), FirstLabelId), TestInstrFlags.Branch);
		}

		[Fact]
		public void xbegin_offset() {
			TestAssembler(c => c.xbegin(12752), Instruction.CreateXbegin(Bitness, 12752), TestInstrFlags.BranchU64);
		}
	}
}
#endif
