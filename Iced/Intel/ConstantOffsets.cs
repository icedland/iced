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

#if ((!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER) || !NO_ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Contains the offsets of the displacement and immediate. Call decoder's GetConstantOffsets() method
	/// to get the offsets of the constants after the instruction has been decoded. The encoder has a similar method.
	/// </summary>
	public struct ConstantOffsets {
		/// <summary>
		/// The offset of the displacement, if any
		/// </summary>
		public byte DisplacementOffset;

		/// <summary>
		/// The offset of the first immediate, if any.
		/// 
		/// * This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
		/// of the instruction stream, eg. 'shl al,1'.
		/// 
		/// * If it's the enter instruction, this is the offset of the first immediate. The offset of the second
		/// immediate is this value + 2 bytes.
		/// 
		/// * If it's a far call/jmp, the selector constant (2 bytes) follows the offset (2 or 4 bytes).
		/// </summary>
		public byte ImmediateOffset;

		/// <summary>
		/// Size of the displacement in bytes, or 0 if there's no displacement
		/// </summary>
		public byte DisplacementSize;

		/// <summary>
		/// Size of the immediate in bytes, or 0 if there's no immediate
		/// </summary>
		public byte ImmediateSize;

		/// <summary>
		/// true if <see cref="DisplacementOffset"/> is valid
		/// </summary>
		public bool HasDisplacement => DisplacementSize != 0;

		/// <summary>
		/// true if <see cref="ImmediateOffset"/> is valid
		/// </summary>
		public bool HasImmediate => ImmediateSize != 0;
	}
}
#endif
