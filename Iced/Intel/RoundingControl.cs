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
	/// <summary>
	/// Rounding control
	/// </summary>
	public enum RoundingControl {
		/// <summary>
		/// No rounding mode
		/// </summary>
		None = 0,

		/// <summary>
		/// Round to nearest (even)
		/// </summary>
		RoundToNearest = 1,

		/// <summary>
		/// Round down (toward -inf)
		/// </summary>
		RoundDown = 2,

		/// <summary>
		/// Round up (toward +inf)
		/// </summary>
		RoundUp = 3,

		/// <summary>
		/// Round toward zero (truncate)
		/// </summary>
		RoundTowardZero = 4,
	}
}
