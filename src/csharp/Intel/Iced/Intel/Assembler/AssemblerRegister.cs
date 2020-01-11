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
using System;
using System.Diagnostics;

namespace Iced.Intel
{
	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	public readonly struct AssemblerRegister : IEquatable<AssemblerRegister> {

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		public AssemblerRegister(Register value) {
			Value = value;
			Flags = AssemblerOperandFlags.None;
		} 

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A register</param>
		/// <param name="flags">The mask</param>
		public AssemblerRegister(Register value, AssemblerOperandFlags flags) {
			Value = value;
			Flags = flags;
		}

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Gets the mask associated with this register.
		/// </summary>
		public readonly AssemblerOperandFlags Flags;
		
		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerRegister k1 => new AssemblerRegister(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerRegister k2 => new AssemblerRegister(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerRegister k3 => new AssemblerRegister(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);
		
		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerRegister k4 => new AssemblerRegister(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerRegister k5 => new AssemblerRegister(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);
		
		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerRegister k6 => new AssemblerRegister(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);
		
		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerRegister k7 => new AssemblerRegister(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);
		
		/// <summary>
		/// Apply mask Zeroing.
		/// </summary>
		public AssemblerRegister z => new AssemblerRegister(Value, Flags | AssemblerOperandFlags.Zeroing);

		/// <summary>
		/// Checks if it's a general purpose register (<c>AL</c>-<c>R15L</c>, <c>AX</c>-<c>R15W</c>, <c>EAX</c>-<c>R15D</c>, <c>RAX</c>-<c>R15</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsGPR() => Value.IsGPR();

		/// <summary>
		/// Checks if it's an 8-bit general purpose register (<c>AL</c>-<c>R15L</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsGPR8() => Value.IsGPR8();

		/// <summary>
		/// Checks if it's a 16-bit general purpose register (<c>AX</c>-<c>R15W</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsGPR16() => Value.IsGPR16();
		
		/// <summary>
		/// Checks if it's a 32-bit general purpose register (<c>EAX</c>-<c>R15D</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsGPR32() => Value.IsGPR32();
		
		/// <summary>
		/// Checks if it's a 64-bit general purpose register (<c>RAX</c>-<c>R15</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsGPR64() => Value.IsGPR64();
		
		/// <summary>
		/// Check if it is a K0-K7 register.
		/// </summary>
		/// <returns></returns>
		public bool IsK() => Value.IsK();

		/// <summary>
		/// Checks if it's a 64-bit vector register (<c>MM0</c>-<c>MM7</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsMM() => Value.IsMM();

		/// <summary>
		/// Checks if it's a 128-bit vector register (<c>XMM0</c>-<c>XMM31</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsXMM() => Value.IsXMM();

		/// <summary>
		/// Checks if it's a 256-bit vector register (<c>YMM0</c>-<c>YMM31</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsYMM() => Value.IsYMM();

		/// <summary>
		/// Checks if it's a 512-bit vector register (<c>ZMM0</c>-<c>ZMM31</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsZMM() => Value.IsZMM();

		/// <summary>
		/// Check if it is a CR0-CR15 register.
		/// </summary>
		/// <returns></returns>
		public bool IsCR() => Value.IsCR();

		/// <summary>
		/// Check if it is a DR0-DR15 register.
		/// </summary>
		/// <returns></returns>
		public bool IsDR() => Value.IsDR();

		/// <summary>
		/// Check if it is a TR0-TR7 register.
		/// </summary>
		/// <returns></returns>
		public bool IsTR() => Value.IsTR();

		/// <summary>
		/// Check if it is a ST0-ST7 register.
		/// </summary>
		/// <returns></returns>
		public bool IsST() => Value.IsST();

		/// <summary>
		/// Checks if it's a segment register (<c>ES</c>, <c>CS</c>, <c>SS</c>, <c>DS</c>, <c>FS</c>, <c>GS</c>)
		/// </summary>
		/// <returns></returns>
		public bool IsSegmentRegister() => Value.IsSegmentRegister();

		/// <summary>
		/// Converts a <see cref="Register"/> to a <see cref="AssemblerRegister"/>.
		/// </summary>
		/// <param name="value">Register</param>
		/// <returns></returns>
		public static implicit operator AssemblerRegister(Register value) {
			return new AssemblerRegister(value);
		}

		/// <summary>
		/// Converts a <see cref="AssemblerRegister"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegister</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegister reg) {
			return reg.Value;
		}

		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister left, AssemblerRegister right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Adds a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Multiplies an index register by a scale and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="scale">The scale</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator *(AssemblerRegister left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0, AssemblerOperandFlags.None);
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegister other) => Value == other.Value;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegister other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegister"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegister left, AssemblerRegister right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegister"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegister left, AssemblerRegister right) => !left.Equals(right);
	}
}
