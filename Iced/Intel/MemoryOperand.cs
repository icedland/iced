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

#if !NO_ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Memory operand
	/// </summary>
	public readonly struct MemoryOperand {
		/// <summary>
		/// Segment override or <see cref="Register.None"/>
		/// </summary>
		public readonly Register PrefixSegment;

		/// <summary>
		/// Base register or <see cref="Register.None"/>
		/// </summary>
		public readonly Register Base;

		/// <summary>
		/// Index register or <see cref="Register.None"/>
		/// </summary>
		public readonly Register Index;

		/// <summary>
		/// Index register scale (1, 2, 4, or 8)
		/// </summary>
		public readonly int Scale;

		/// <summary>
		/// Memory displacement, or ignored if <see cref="DisplSize"/> is 0
		/// </summary>
		public readonly int Displacement;

		/// <summary>
		/// 0 if no <see cref="Displacement"/>, else 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
		/// </summary>
		public readonly int DisplSize;

		/// <summary>
		/// Memory size. The encoder uses this to check if it's broadcasted memory. The formatters use it to show the memory size info
		/// </summary>
		public readonly MemorySize Size;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement, or ignored if <paramref name="displSize"/> is 0</param>
		/// <param name="displSize">0 if no <paramref name="displacement"/>, else 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		/// <param name="size">Memory size. The encoder uses this to check if it's broadcasted memory. The formatters use it to show the memory size info</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, Register index, int scale, int displacement, int displSize, MemorySize size, Register prefixSegment) {
			PrefixSegment = prefixSegment;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			Size = size;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="size">Memory size. The encoder uses this to check if it's broadcasted memory. The formatters use it to show the memory size info</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, Register index, int scale, MemorySize size, Register prefixSegment) {
			PrefixSegment = prefixSegment;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = 0;
			DisplSize = 0;
			Size = size;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement, or ignored if <paramref name="displSize"/> is 0</param>
		/// <param name="displSize">0 if no <paramref name="displacement"/>, else 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		/// <param name="size">Memory size. The encoder uses this to check if it's broadcasted memory. The formatters use it to show the memory size info</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, int displacement, int displSize, MemorySize size, Register prefixSegment) {
			PrefixSegment = prefixSegment;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = displSize;
			Size = size;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement, or ignored if <paramref name="displSize"/> is 0</param>
		/// <param name="displSize">0 if no <paramref name="displacement"/>, else 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		/// <param name="size">Memory size. The encoder uses this to check if it's broadcasted memory. The formatters use it to show the memory size info</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register index, int scale, int displacement, int displSize, MemorySize size, Register prefixSegment) {
			PrefixSegment = prefixSegment;
			Base = Register.None;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			Size = size;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="size">Memory size. The encoder uses this to check if it's broadcasted memory. The formatters use it to show the memory size info</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, int displacement, MemorySize size, Register prefixSegment) {
			PrefixSegment = prefixSegment;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = 1;
			Size = size;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement, or ignored if <paramref name="displSize"/> is 0</param>
		/// <param name="displSize">0 if no <paramref name="displacement"/>, else 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		public MemoryOperand(Register @base, Register index, int scale, int displacement, int displSize) {
			PrefixSegment = Register.None;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			Size = MemorySize.Unknown;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		public MemoryOperand(Register @base, Register index, int scale) {
			PrefixSegment = Register.None;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = 0;
			DisplSize = 0;
			Size = MemorySize.Unknown;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement, or ignored if <paramref name="displSize"/> is 0</param>
		/// <param name="displSize">0 if no <paramref name="displacement"/>, else 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		public MemoryOperand(Register @base, int displacement, int displSize) {
			PrefixSegment = Register.None;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = displSize;
			Size = MemorySize.Unknown;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement, or ignored if <paramref name="displSize"/> is 0</param>
		/// <param name="displSize">0 if no <paramref name="displacement"/>, else 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		public MemoryOperand(Register index, int scale, int displacement, int displSize) {
			PrefixSegment = Register.None;
			Base = Register.None;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			Size = MemorySize.Unknown;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement</param>
		public MemoryOperand(Register @base, int displacement) {
			PrefixSegment = Register.None;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = 1;
			Size = MemorySize.Unknown;
		}
	}
}
#endif
