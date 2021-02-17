// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Iced.Intel {
	static class InstructionUtils {
		public static int GetAddressSizeInBytes(Register baseReg, Register indexReg, int displSize, CodeSize codeSize) {
			if ((Register.RAX <= baseReg && baseReg <= Register.R15) || (Register.RAX <= indexReg && indexReg <= Register.R15) || baseReg == Register.RIP)
				return 8;
			if ((Register.EAX <= baseReg && baseReg <= Register.R15D) || (Register.EAX <= indexReg && indexReg <= Register.R15D) || baseReg == Register.EIP)
				return 4;
			if ((Register.AX <= baseReg && baseReg <= Register.DI) || (Register.AX <= indexReg && indexReg <= Register.DI))
				return 2;
			if (displSize == 2 || displSize == 4 || displSize == 8)
				return displSize;

			return codeSize switch {
				CodeSize.Code64 => 8,
				CodeSize.Code32 => 4,
				CodeSize.Code16 => 2,
				_ => 8
			};
		}
	}
}
