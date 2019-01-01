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

#if ((!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER) || !NO_ENCODER || !NO_INSTR_INFO
namespace Iced.Intel {
	/// <summary>
	/// Instruction encoding
	/// </summary>
	public enum EncodingKind {
		/// <summary>
		/// Legacy encoding
		/// </summary>
		Legacy,

		/// <summary>
		/// VEX encoding
		/// </summary>
		VEX,

		/// <summary>
		/// EVEX encoding
		/// </summary>
		EVEX,

		/// <summary>
		/// XOP encoding
		/// </summary>
		XOP,

		/// <summary>
		/// 3DNow! encoding
		/// </summary>
		D3NOW,

		// If you add a new value, verify that all values fit in the following flags:
		//		StateFlags.EncodingMask
		//		InfoFlags2.EncodingMask
		//		EncFlags1.EncodingMask
	}
}
#endif
