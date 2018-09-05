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

#if !NO_INSTR_INFO
using System;

namespace Iced.Intel {
	/// <summary>
	/// RFLAGS bits supported by the instruction info code
	/// </summary>
	[Flags]
	public enum RflagsBits {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		None	= 0,
		OF		= 0x00000001,
		SF		= 0x00000002,
		ZF		= 0x00000004,
		AF		= 0x00000008,
		CF		= 0x00000010,
		PF		= 0x00000020,
		DF		= 0x00000040,
		IF		= 0x00000080,
		AC		= 0x00000100,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}
}
#endif
