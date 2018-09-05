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

#if !NO_ENCODER
namespace Iced.Intel.BlockEncoderInternal {
	readonly struct TargetInstr {
		readonly Instr instruction;
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
			if (instruction == null)
				return address;
			return instruction.IP;
		}
	}
}
#endif
