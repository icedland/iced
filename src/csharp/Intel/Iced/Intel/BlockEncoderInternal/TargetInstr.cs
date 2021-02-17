// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER
namespace Iced.Intel.BlockEncoderInternal {
	readonly struct TargetInstr {
		readonly Instr? instruction;
		readonly ulong address;

		public TargetInstr(Instr instruction) {
			this.instruction = instruction;
			address = 0;
		}

		public TargetInstr(ulong address) {
			instruction = null;
			this.address = address;
		}

		public bool IsInBlock(Block block) => instruction?.Block == block;

		public ulong GetAddress() {
			var instruction = this.instruction;
			if (instruction is null)
				return address;
			return instruction.IP;
		}
	}
}
#endif
