// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
namespace Iced.Intel {
	/// <summary>
	/// A register used by an instruction
	/// </summary>
	public readonly struct UsedRegister {
		readonly Register register;
		readonly OpAccess access;

		/// <summary>
		/// Register
		/// </summary>
		public Register Register => register;

		/// <summary>
		/// Register access
		/// </summary>
		public OpAccess Access => access;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="register">Register</param>
		/// <param name="access">Register access</param>
		public UsedRegister(Register register, OpAccess access) {
			this.register = register;
			this.access = access;
		}

		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() => Register.ToString() + ":" + Access.ToString();
	}
}
#endif
