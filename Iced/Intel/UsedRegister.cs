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
using System.Diagnostics;

namespace Iced.Intel {
	/// <summary>
	/// A register used by an instruction
	/// </summary>
	public readonly struct UsedRegister {
		readonly byte register;
		readonly byte access;

		/// <summary>
		/// Register
		/// </summary>
		public Register Register => (Register)register;

		/// <summary>
		/// Register access
		/// </summary>
		public OpAccess Access => (OpAccess)access;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="register">Register</param>
		/// <param name="access">Register access</param>
		public UsedRegister(Register register, OpAccess access) {
			Debug.Assert((uint)register <= byte.MaxValue);
			this.register = (byte)register;
			Debug.Assert((uint)access <= byte.MaxValue);
			this.access = (byte)access;
		}

		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Register.ToString() + ":" + Access.ToString();
	}
}
#endif
