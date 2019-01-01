/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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

namespace Iced.Intel {
	static class InstructionUtils {
		public static int GetAddressSizeInBytes(Register baseReg, Register indexReg, int displSize, CodeSize codeSize) {
			if ((Register.RAX <= baseReg && baseReg <= Register.R15) || (Register.RAX <= indexReg && indexReg <= Register.R15) || baseReg == Register.RIP)
				return 8;
			if ((Register.EAX <= baseReg && baseReg <= Register.R15D) || (Register.EAX <= indexReg && indexReg <= Register.R15D) || baseReg == Register.EIP)
				return 4;
			if (baseReg == Register.BX || baseReg == Register.BP || baseReg == Register.SI || baseReg == Register.DI || indexReg == Register.SI || indexReg == Register.DI)
				return 2;
			if (displSize == 2 || displSize == 4 || displSize == 8)
				return displSize;

			if (codeSize == CodeSize.Code64)
				return 8;
			if (codeSize == CodeSize.Code32)
				return 4;
			if (codeSize == CodeSize.Code16)
				return 2;
			return 8;
		}
	}
}
