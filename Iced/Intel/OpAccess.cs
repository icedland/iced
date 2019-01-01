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

#if !NO_INSTR_INFO
namespace Iced.Intel {
	/// <summary>
	/// Operand, register and memory access
	/// </summary>
	public enum OpAccess {
		/// <summary>
		/// Nothing is read and nothing is written
		/// </summary>
		None,

		/// <summary>
		/// The value is read
		/// </summary>
		Read,

		/// <summary>
		/// The value is sometimes read and sometimes not
		/// </summary>
		CondRead,

		/// <summary>
		/// The value is completely overwritten
		/// </summary>
		Write,

		/// <summary>
		/// Conditional write, sometimes it's written and sometimes it's not modified
		/// </summary>
		CondWrite,

		/// <summary>
		/// The value is read and written
		/// </summary>
		ReadWrite,

		/// <summary>
		/// The value is read and sometimes written
		/// </summary>
		ReadCondWrite,

		/// <summary>
		/// The memory operand doesn't refer to memory (eg. lea instruction) or it's an instruction that doesn't
		/// read the data to a register or doesn't write to the memory location, it just prefetches/invalidates it,
		/// eg. invlpg, prefetchnta, vgatherpf0dps, etc.
		/// </summary>
		NoMemAccess,
	}
}
#endif
