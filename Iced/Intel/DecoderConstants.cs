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

namespace Iced.Intel {
	static class DecoderConstants {
		public const int MaxInstructionLength = 15;
		public const int MaxOpCount = 5;
		public const int NumberOfCodeValues = (int)Code.D3NOW_Pavgusb_mm_mmm64 + 1;
		public const int NumberOfRegisters = (int)Register.TR7 + 1;
		public const int NumberOfMemorySizes = (int)MemorySize.Broadcast512_Float64 + 1;
	}
}
