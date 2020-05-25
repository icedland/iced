/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Iced.Intel {
	/// <summary>
	/// Defines an assembly memory operand used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{Base} + {Index} * {Scale} + {Displacement}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly struct AssemblerMemoryOperand : IEquatable<AssemblerMemoryOperand> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="size">Size of the operand.</param>
		/// <param name="prefix">Register prefix.</param>
		/// <param name="base">Base register.</param>
		/// <param name="index">Index register.</param>
		/// <param name="scale">Scale of the index.</param>
		/// <param name="displacement">Displacement.</param>
		/// <param name="flags">Flags attached to this operand.</param>
		internal AssemblerMemoryOperand(MemoryOperandSize size, Register prefix, Register @base, Register index, int scale, long displacement, AssemblerOperandFlags flags) {
			Size = size;
			Prefix = prefix;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			Flags = flags;
		}

		/// <summary>
		/// Gets the size of the operand.
		/// </summary>
		internal readonly MemoryOperandSize Size;

		/// <summary>
		/// Gets the register used as a prefix.
		/// </summary>
		public readonly Register Prefix;

		/// <summary>
		/// Gets the register used as a base.
		/// </summary>
		public readonly Register Base;

		/// <summary>
		/// Gets the register used as an index.
		/// </summary>
		public readonly Register Index;

		/// <summary>
		/// Gets the scale applied to the index register.
		/// </summary>
		public readonly int Scale;

		/// <summary>
		/// Gets the displacement.
		/// </summary>
		public readonly long Displacement;

		/// <summary>
		/// Gets the mask associated with this operand.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Gets a boolean indicating if this memory operand is a broadcast.
		/// </summary>
		public bool IsBroadcast => (Flags & AssemblerOperandFlags.Broadcast) != 0;

		/// <summary>
		/// Gets a boolean indicating if this memory operand is a memory access using displacement only (no base and index registers are used).
		/// </summary>
		internal bool IsDisplacementOnly => Base == Register.None && Index == Register.None;

		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerMemoryOperand k1 => new AssemblerMemoryOperand(Size, Prefix, Base, Index, Scale, Displacement, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerMemoryOperand k2 => new AssemblerMemoryOperand(Size, Prefix, Base, Index, Scale, Displacement, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerMemoryOperand k3 => new AssemblerMemoryOperand(Size, Prefix, Base, Index, Scale, Displacement, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);

		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerMemoryOperand k4 => new AssemblerMemoryOperand(Size, Prefix, Base, Index, Scale, Displacement, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerMemoryOperand k5 => new AssemblerMemoryOperand(Size, Prefix, Base, Index, Scale, Displacement, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);

		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerMemoryOperand k6 => new AssemblerMemoryOperand(Size, Prefix, Base, Index, Scale, Displacement, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);

		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerMemoryOperand k7 => new AssemblerMemoryOperand(Size, Prefix, Base, Index, Scale, Displacement, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);

		/// <summary>
		/// Adds a 16-bit memory operand with an new base or index.
		/// </summary>
		/// <param name="left">The base or index.</param>
		/// <param name="right">The memory operand.</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister16 left, AssemblerMemoryOperand right) {
			var hasBase = right.Base != Register.None;
			return new AssemblerMemoryOperand(right.Size, Register.None, hasBase ? right.Base : left.Value, hasBase ? left.Value : right.Index, right.Scale, right.Displacement, right.Flags);
		}

		/// <summary>
		/// Adds a 32-bit memory operand with an new base or index.
		/// </summary>
		/// <param name="left">The base or index.</param>
		/// <param name="right">The memory operand.</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister32 left, AssemblerMemoryOperand right) {
			var hasBase = right.Base != Register.None;
			return new AssemblerMemoryOperand(right.Size, Register.None, hasBase ? right.Base : left.Value, hasBase ? left.Value : right.Index, right.Scale, right.Displacement, right.Flags);
		}

		/// <summary>
		/// Adds a 64-bit memory operand with an new base or index.
		/// </summary>
		/// <param name="left">The base or index.</param>
		/// <param name="right">The memory operand.</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister64 left, AssemblerMemoryOperand right) {
			var hasBase = right.Base != Register.None;
			return new AssemblerMemoryOperand(right.Size, Register.None, hasBase ? right.Base : left.Value, hasBase ? left.Value : right.Index, right.Scale, right.Displacement, right.Flags);
		}

		/// <summary>
		/// Adds a displacement to a memory operand.
		/// </summary>
		/// <param name="left">The memory operand.</param>
		/// <param name="displacement">displacement.</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerMemoryOperand left, int displacement) {
			return new AssemblerMemoryOperand(left.Size, Register.None, left.Base, left.Index, left.Scale, left.Displacement + displacement, left.Flags);
		}

		/// <summary>
		/// Subtracts a displacement to a memory operand.
		/// </summary>
		/// <param name="left">The memory operand.</param>
		/// <param name="displacement">displacement.</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator -(AssemblerMemoryOperand left, int displacement) {
			return new AssemblerMemoryOperand(left.Size, Register.None, left.Base, left.Index, left.Scale, left.Displacement - displacement, left.Flags);
		}

		/// <summary>
		/// Gets a memory operand for the specified bitness.
		/// </summary>
		/// <param name="bitness">The bitness</param>
		public MemoryOperand ToMemoryOperand(int bitness) {
			int dispSize = 1;
			if (IsDisplacementOnly) {
				dispSize = bitness / 8;
			}
			else if (Displacement == 0) {
				dispSize = 0;
			}
			return new MemoryOperand(Base, Index, Scale, (int)Displacement, dispSize, (Flags & AssemblerOperandFlags.Broadcast) != 0, Prefix);
		}

		/// <inheritdoc />
		public bool Equals(AssemblerMemoryOperand other) => Size == other.Size && Prefix == other.Prefix && Base == other.Base && Index == other.Index && Scale == other.Scale && Displacement == other.Displacement && Flags == other.Flags;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerMemoryOperand other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() {
			unchecked {
				var hashCode = (int)Size;
				hashCode = (hashCode * 397) ^ (int)Prefix;
				hashCode = (hashCode * 397) ^ (int)Base;
				hashCode = (hashCode * 397) ^ (int)Index;
				hashCode = (hashCode * 397) ^ Scale;
				hashCode = (hashCode * 397) ^ Displacement.GetHashCode();
				hashCode = (hashCode * 397) ^ (int)Flags;
				return hashCode;
			}
		}

		/// <summary>
		/// Equality operator for <see cref="AssemblerMemoryOperand"/>.
		/// </summary>
		/// <param name="left">Left operand</param>
		/// <param name="right">Right operand</param>
		/// <returns><c>true</c> if equal; otherwise <c>false</c></returns>
		public static bool operator ==(AssemblerMemoryOperand left, AssemblerMemoryOperand right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerMemoryOperand"/>.
		/// </summary>
		/// <param name="left">Left operand</param>
		/// <param name="right">Right operand</param>
		/// <returns><c>true</c> if not equal; otherwise <c>false</c></returns>
		public static bool operator !=(AssemblerMemoryOperand left, AssemblerMemoryOperand right) => !left.Equals(right);
	}
}
#endif
