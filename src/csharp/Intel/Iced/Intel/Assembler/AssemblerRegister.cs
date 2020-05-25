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
#nullable enable
using System;
using System.Diagnostics;
using System.ComponentModel;

namespace Iced.Intel {
	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegister8 : IEquatable<AssemblerRegister8> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegister8(Register value) {
			if (!value.IsGPR8()) throw new ArgumentException($"Invalid register {value}. Must be a GPR8 register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegister8"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegister8</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegister8 reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegister8 other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegister8 other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegister8"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegister8 left, AssemblerRegister8 right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegister8"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegister8 left, AssemblerRegister8 right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegister16 : IEquatable<AssemblerRegister16> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegister16(Register value) {
			if (!value.IsGPR16()) throw new ArgumentException($"Invalid register {value}. Must be a GPR16 register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegister16"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegister16</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegister16 reg) {
			return reg.Value;
		}

		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister16 left, AssemblerRegister16 right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister16 left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Subtracts a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator -(AssemblerRegister16 left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, -displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Multiplies an index register by a scale and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="scale">The scale</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator *(AssemblerRegister16 left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0, AssemblerOperandFlags.None);
		}
		/// <inheritdoc />
		public bool Equals(AssemblerRegister16 other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegister16 other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegister16"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegister16 left, AssemblerRegister16 right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegister16"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegister16 left, AssemblerRegister16 right) => !left.Equals(right);
	}

	public readonly partial struct AssemblerMemoryOperandFactory {
		/// <summary>
		/// Specify a base register used with this memory operand (Base + Index * Scale + Displacement)
		/// </summary>
		/// <param name="register">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerRegister16 register] => new AssemblerMemoryOperand(Size, Prefix, register, Register.None, 1, 0, Flags);
	}
	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegister32 : IEquatable<AssemblerRegister32> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegister32(Register value) {
			if (!value.IsGPR32()) throw new ArgumentException($"Invalid register {value}. Must be a GPR32 register", nameof(value));
			Value = value;
			Flags = AssemblerOperandFlags.None;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A register</param>
		/// <param name="flags">The mask</param>
		internal AssemblerRegister32(Register value, AssemblerOperandFlags flags) {
			Value = value;
			Flags = flags;
		}

		/// <summary>
		/// Gets the mask associated with this register.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerRegister32 k1 => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerRegister32 k2 => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerRegister32 k3 => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);

		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerRegister32 k4 => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerRegister32 k5 => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);

		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerRegister32 k6 => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);

		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerRegister32 k7 => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);

		/// <summary>
		/// Apply mask Zeroing.
		/// </summary>
		public AssemblerRegister32 z => new AssemblerRegister32(Value, Flags | AssemblerOperandFlags.Zeroing);


		/// <summary>
		/// Suppress all exceptions.
		/// </summary>
		public AssemblerRegister32 sae => new AssemblerRegister32(Value, Flags | AssemblerOperandFlags.SuppressAllExceptions);

		/// <summary>
		/// Rounding to nearest.
		/// </summary>
		public AssemblerRegister32 rn_sae => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundToNearest);

		/// <summary>
		/// Rounding down.
		/// </summary>
		public AssemblerRegister32 rd_sae => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundDown);

		/// <summary>
		/// Rounding up.
		/// </summary>
		public AssemblerRegister32 ru_sae => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundUp);

		/// <summary>
		/// Rounding toward zero.
		/// </summary>
		public AssemblerRegister32 rz_sae => new AssemblerRegister32(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundTowardZero);
		/// <summary>
		/// Converts a <see cref="AssemblerRegister32"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegister32</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegister32 reg) {
			return reg.Value;
		}

		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister32 left, AssemblerRegister32 right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister32 left, AssemblerRegisterXMM right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister32 left, AssemblerRegisterYMM right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister32 left, AssemblerRegisterZMM right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister32 left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Subtracts a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator -(AssemblerRegister32 left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, -displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Multiplies an index register by a scale and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="scale">The scale</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator *(AssemblerRegister32 left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0, AssemblerOperandFlags.None);
		}
		/// <inheritdoc />
		public bool Equals(AssemblerRegister32 other) => Value == other.Value && Flags == other.Flags;

		/// <inheritdoc />
		public override int GetHashCode() => ((int) Value * 397) ^ (int)Flags;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegister32 other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegister32"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegister32 left, AssemblerRegister32 right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegister32"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegister32 left, AssemblerRegister32 right) => !left.Equals(right);
	}

	public readonly partial struct AssemblerMemoryOperandFactory {
		/// <summary>
		/// Specify a base register used with this memory operand (Base + Index * Scale + Displacement)
		/// </summary>
		/// <param name="register">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerRegister32 register] => new AssemblerMemoryOperand(Size, Prefix, register, Register.None, 1, 0, Flags);
	}
	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegister64 : IEquatable<AssemblerRegister64> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegister64(Register value) {
			if (!value.IsGPR64()) throw new ArgumentException($"Invalid register {value}. Must be a GPR64 register", nameof(value));
			Value = value;
			Flags = AssemblerOperandFlags.None;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A register</param>
		/// <param name="flags">The mask</param>
		internal AssemblerRegister64(Register value, AssemblerOperandFlags flags) {
			Value = value;
			Flags = flags;
		}

		/// <summary>
		/// Gets the mask associated with this register.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerRegister64 k1 => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerRegister64 k2 => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerRegister64 k3 => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);

		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerRegister64 k4 => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerRegister64 k5 => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);

		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerRegister64 k6 => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);

		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerRegister64 k7 => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);

		/// <summary>
		/// Apply mask Zeroing.
		/// </summary>
		public AssemblerRegister64 z => new AssemblerRegister64(Value, Flags | AssemblerOperandFlags.Zeroing);


		/// <summary>
		/// Suppress all exceptions.
		/// </summary>
		public AssemblerRegister64 sae => new AssemblerRegister64(Value, Flags | AssemblerOperandFlags.SuppressAllExceptions);

		/// <summary>
		/// Rounding to nearest.
		/// </summary>
		public AssemblerRegister64 rn_sae => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundToNearest);

		/// <summary>
		/// Rounding down.
		/// </summary>
		public AssemblerRegister64 rd_sae => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundDown);

		/// <summary>
		/// Rounding up.
		/// </summary>
		public AssemblerRegister64 ru_sae => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundUp);

		/// <summary>
		/// Rounding toward zero.
		/// </summary>
		public AssemblerRegister64 rz_sae => new AssemblerRegister64(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundTowardZero);
		/// <summary>
		/// Converts a <see cref="AssemblerRegister64"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegister64</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegister64 reg) {
			return reg.Value;
		}

		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister64 left, AssemblerRegister64 right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister64 left, AssemblerRegisterXMM right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister64 left, AssemblerRegisterYMM right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) to another register (index) and return a memory operand.
		/// </summary>
		/// <param name="left">The base register.</param>
		/// <param name="right">The index register</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister64 left, AssemblerRegisterZMM right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0, AssemblerOperandFlags.None);
		}
		/// <summary>
		/// Adds a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegister64 left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Subtracts a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator -(AssemblerRegister64 left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, -displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Multiplies an index register by a scale and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="scale">The scale</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator *(AssemblerRegister64 left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0, AssemblerOperandFlags.None);
		}
		/// <inheritdoc />
		public bool Equals(AssemblerRegister64 other) => Value == other.Value && Flags == other.Flags;

		/// <inheritdoc />
		public override int GetHashCode() => ((int) Value * 397) ^ (int)Flags;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegister64 other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegister64"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegister64 left, AssemblerRegister64 right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegister64"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegister64 left, AssemblerRegister64 right) => !left.Equals(right);
	}

	public readonly partial struct AssemblerMemoryOperandFactory {
		/// <summary>
		/// Specify a base register used with this memory operand (Base + Index * Scale + Displacement)
		/// </summary>
		/// <param name="register">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerRegister64 register] => new AssemblerMemoryOperand(Size, Prefix, register, Register.None, 1, 0, Flags);
	}
	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterMM : IEquatable<AssemblerRegisterMM> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterMM(Register value) {
			if (!value.IsMM()) throw new ArgumentException($"Invalid register {value}. Must be a MM register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterMM"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterMM</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterMM reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterMM other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterMM other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterMM left, AssemblerRegisterMM right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterMM left, AssemblerRegisterMM right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterXMM : IEquatable<AssemblerRegisterXMM> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterXMM(Register value) {
			if (!value.IsXMM()) throw new ArgumentException($"Invalid register {value}. Must be a XMM register", nameof(value));
			Value = value;
			Flags = AssemblerOperandFlags.None;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A register</param>
		/// <param name="flags">The mask</param>
		internal AssemblerRegisterXMM(Register value, AssemblerOperandFlags flags) {
			Value = value;
			Flags = flags;
		}

		/// <summary>
		/// Gets the mask associated with this register.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerRegisterXMM k1 => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerRegisterXMM k2 => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerRegisterXMM k3 => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);

		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerRegisterXMM k4 => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerRegisterXMM k5 => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);

		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerRegisterXMM k6 => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);

		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerRegisterXMM k7 => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);

		/// <summary>
		/// Apply mask Zeroing.
		/// </summary>
		public AssemblerRegisterXMM z => new AssemblerRegisterXMM(Value, Flags | AssemblerOperandFlags.Zeroing);


		/// <summary>
		/// Suppress all exceptions.
		/// </summary>
		public AssemblerRegisterXMM sae => new AssemblerRegisterXMM(Value, Flags | AssemblerOperandFlags.SuppressAllExceptions);

		/// <summary>
		/// Rounding to nearest.
		/// </summary>
		public AssemblerRegisterXMM rn_sae => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundToNearest);

		/// <summary>
		/// Rounding down.
		/// </summary>
		public AssemblerRegisterXMM rd_sae => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundDown);

		/// <summary>
		/// Rounding up.
		/// </summary>
		public AssemblerRegisterXMM ru_sae => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundUp);

		/// <summary>
		/// Rounding toward zero.
		/// </summary>
		public AssemblerRegisterXMM rz_sae => new AssemblerRegisterXMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundTowardZero);
		/// <summary>
		/// Converts a <see cref="AssemblerRegisterXMM"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterXMM</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterXMM reg) {
			return reg.Value;
		}

		/// <summary>
		/// Adds a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegisterXMM left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Subtracts a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator -(AssemblerRegisterXMM left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, -displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Multiplies an index register by a scale and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="scale">The scale</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator *(AssemblerRegisterXMM left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0, AssemblerOperandFlags.None);
		}
		/// <inheritdoc />
		public bool Equals(AssemblerRegisterXMM other) => Value == other.Value && Flags == other.Flags;

		/// <inheritdoc />
		public override int GetHashCode() => ((int) Value * 397) ^ (int)Flags;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterXMM other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterXMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterXMM left, AssemblerRegisterXMM right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterXMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterXMM left, AssemblerRegisterXMM right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterYMM : IEquatable<AssemblerRegisterYMM> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterYMM(Register value) {
			if (!value.IsYMM()) throw new ArgumentException($"Invalid register {value}. Must be a YMM register", nameof(value));
			Value = value;
			Flags = AssemblerOperandFlags.None;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A register</param>
		/// <param name="flags">The mask</param>
		internal AssemblerRegisterYMM(Register value, AssemblerOperandFlags flags) {
			Value = value;
			Flags = flags;
		}

		/// <summary>
		/// Gets the mask associated with this register.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerRegisterYMM k1 => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerRegisterYMM k2 => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerRegisterYMM k3 => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);

		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerRegisterYMM k4 => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerRegisterYMM k5 => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);

		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerRegisterYMM k6 => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);

		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerRegisterYMM k7 => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);

		/// <summary>
		/// Apply mask Zeroing.
		/// </summary>
		public AssemblerRegisterYMM z => new AssemblerRegisterYMM(Value, Flags | AssemblerOperandFlags.Zeroing);


		/// <summary>
		/// Suppress all exceptions.
		/// </summary>
		public AssemblerRegisterYMM sae => new AssemblerRegisterYMM(Value, Flags | AssemblerOperandFlags.SuppressAllExceptions);

		/// <summary>
		/// Rounding to nearest.
		/// </summary>
		public AssemblerRegisterYMM rn_sae => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundToNearest);

		/// <summary>
		/// Rounding down.
		/// </summary>
		public AssemblerRegisterYMM rd_sae => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundDown);

		/// <summary>
		/// Rounding up.
		/// </summary>
		public AssemblerRegisterYMM ru_sae => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundUp);

		/// <summary>
		/// Rounding toward zero.
		/// </summary>
		public AssemblerRegisterYMM rz_sae => new AssemblerRegisterYMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundTowardZero);
		/// <summary>
		/// Converts a <see cref="AssemblerRegisterYMM"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterYMM</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterYMM reg) {
			return reg.Value;
		}

		/// <summary>
		/// Adds a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegisterYMM left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Subtracts a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator -(AssemblerRegisterYMM left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, -displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Multiplies an index register by a scale and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="scale">The scale</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator *(AssemblerRegisterYMM left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0, AssemblerOperandFlags.None);
		}
		/// <inheritdoc />
		public bool Equals(AssemblerRegisterYMM other) => Value == other.Value && Flags == other.Flags;

		/// <inheritdoc />
		public override int GetHashCode() => ((int) Value * 397) ^ (int)Flags;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterYMM other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterYMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterYMM left, AssemblerRegisterYMM right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterYMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterYMM left, AssemblerRegisterYMM right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterZMM : IEquatable<AssemblerRegisterZMM> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterZMM(Register value) {
			if (!value.IsZMM()) throw new ArgumentException($"Invalid register {value}. Must be a ZMM register", nameof(value));
			Value = value;
			Flags = AssemblerOperandFlags.None;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A register</param>
		/// <param name="flags">The mask</param>
		internal AssemblerRegisterZMM(Register value, AssemblerOperandFlags flags) {
			Value = value;
			Flags = flags;
		}

		/// <summary>
		/// Gets the mask associated with this register.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerRegisterZMM k1 => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerRegisterZMM k2 => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerRegisterZMM k3 => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);

		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerRegisterZMM k4 => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerRegisterZMM k5 => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);

		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerRegisterZMM k6 => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);

		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerRegisterZMM k7 => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);

		/// <summary>
		/// Apply mask Zeroing.
		/// </summary>
		public AssemblerRegisterZMM z => new AssemblerRegisterZMM(Value, Flags | AssemblerOperandFlags.Zeroing);


		/// <summary>
		/// Suppress all exceptions.
		/// </summary>
		public AssemblerRegisterZMM sae => new AssemblerRegisterZMM(Value, Flags | AssemblerOperandFlags.SuppressAllExceptions);

		/// <summary>
		/// Rounding to nearest.
		/// </summary>
		public AssemblerRegisterZMM rn_sae => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundToNearest);

		/// <summary>
		/// Rounding down.
		/// </summary>
		public AssemblerRegisterZMM rd_sae => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundDown);

		/// <summary>
		/// Rounding up.
		/// </summary>
		public AssemblerRegisterZMM ru_sae => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundUp);

		/// <summary>
		/// Rounding toward zero.
		/// </summary>
		public AssemblerRegisterZMM rz_sae => new AssemblerRegisterZMM(Value, (Flags & ~AssemblerOperandFlags.RoundControlMask) | AssemblerOperandFlags.RoundTowardZero);
		/// <summary>
		/// Converts a <see cref="AssemblerRegisterZMM"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterZMM</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterZMM reg) {
			return reg.Value;
		}

		/// <summary>
		/// Adds a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator +(AssemblerRegisterZMM left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Subtracts a register (base) with a displacement and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="displacement">The displacement</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator -(AssemblerRegisterZMM left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, -displacement, AssemblerOperandFlags.None);
		}

		/// <summary>
		/// Multiplies an index register by a scale and return a memory operand.
		/// </summary>
		/// <param name="left">The base register</param>
		/// <param name="scale">The scale</param>
		/// <returns></returns>
		public static AssemblerMemoryOperand operator *(AssemblerRegisterZMM left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0, AssemblerOperandFlags.None);
		}
		/// <inheritdoc />
		public bool Equals(AssemblerRegisterZMM other) => Value == other.Value && Flags == other.Flags;

		/// <inheritdoc />
		public override int GetHashCode() => ((int) Value * 397) ^ (int)Flags;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterZMM other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterZMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterZMM left, AssemblerRegisterZMM right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterZMM"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterZMM left, AssemblerRegisterZMM right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterK : IEquatable<AssemblerRegisterK> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterK(Register value) {
			if (!value.IsK()) throw new ArgumentException($"Invalid register {value}. Must be a K register", nameof(value));
			Value = value;
			Flags = AssemblerOperandFlags.None;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A register</param>
		/// <param name="flags">The mask</param>
		internal AssemblerRegisterK(Register value, AssemblerOperandFlags flags) {
			Value = value;
			Flags = flags;
		}

		/// <summary>
		/// Gets the mask associated with this register.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Apply mask Register K1.
		/// </summary>
		public AssemblerRegisterK k1 => new AssemblerRegisterK(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K1);

		/// <summary>
		/// Apply mask Register K2.
		/// </summary>
		public AssemblerRegisterK k2 => new AssemblerRegisterK(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K2);

		/// <summary>
		/// Apply mask Register K3.
		/// </summary>
		public AssemblerRegisterK k3 => new AssemblerRegisterK(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K3);

		/// <summary>
		/// Apply mask Register K4.
		/// </summary>
		public AssemblerRegisterK k4 => new AssemblerRegisterK(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K4);

		/// <summary>
		/// Apply mask Register K5.
		/// </summary>
		public AssemblerRegisterK k5 => new AssemblerRegisterK(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K5);

		/// <summary>
		/// Apply mask Register K6.
		/// </summary>
		public AssemblerRegisterK k6 => new AssemblerRegisterK(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K6);

		/// <summary>
		/// Apply mask Register K7.
		/// </summary>
		public AssemblerRegisterK k7 => new AssemblerRegisterK(Value, (Flags & ~AssemblerOperandFlags.RegisterMask) | AssemblerOperandFlags.K7);

		/// <summary>
		/// Apply mask Zeroing.
		/// </summary>
		public AssemblerRegisterK z => new AssemblerRegisterK(Value, Flags | AssemblerOperandFlags.Zeroing);

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterK"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterK</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterK reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterK other) => Value == other.Value && Flags == other.Flags;

		/// <inheritdoc />
		public override int GetHashCode() => ((int) Value * 397) ^ (int)Flags;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterK other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterK"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterK left, AssemblerRegisterK right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterK"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterK left, AssemblerRegisterK right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterCR : IEquatable<AssemblerRegisterCR> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterCR(Register value) {
			if (!value.IsCR()) throw new ArgumentException($"Invalid register {value}. Must be a CR register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterCR"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterCR</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterCR reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterCR other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterCR other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterCR"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterCR left, AssemblerRegisterCR right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterCR"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterCR left, AssemblerRegisterCR right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterTR : IEquatable<AssemblerRegisterTR> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterTR(Register value) {
			if (!value.IsTR()) throw new ArgumentException($"Invalid register {value}. Must be a TR register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterTR"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterTR</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterTR reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterTR other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterTR other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterTR"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterTR left, AssemblerRegisterTR right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterTR"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterTR left, AssemblerRegisterTR right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterDR : IEquatable<AssemblerRegisterDR> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterDR(Register value) {
			if (!value.IsDR()) throw new ArgumentException($"Invalid register {value}. Must be a DR register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterDR"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterDR</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterDR reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterDR other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterDR other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterDR"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterDR left, AssemblerRegisterDR right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterDR"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterDR left, AssemblerRegisterDR right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterST : IEquatable<AssemblerRegisterST> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterST(Register value) {
			if (!value.IsST()) throw new ArgumentException($"Invalid register {value}. Must be a ST register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterST"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterST</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterST reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterST other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterST other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterST"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterST left, AssemblerRegisterST right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterST"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterST left, AssemblerRegisterST right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterSegment : IEquatable<AssemblerRegisterSegment> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterSegment(Register value) {
			if (!value.IsSegmentRegister()) throw new ArgumentException($"Invalid register {value}. Must be a SegmentRegister register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterSegment"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterSegment</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterSegment reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterSegment other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterSegment other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterSegment"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterSegment left, AssemblerRegisterSegment right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterSegment"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterSegment left, AssemblerRegisterSegment right) => !left.Equals(right);
	}

	/// <summary>
	/// An assembler register used with <see cref="Assembler"/>.
	/// </summary>
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerRegisterBND : IEquatable<AssemblerRegisterBND> {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="value">A Register</param>
		internal AssemblerRegisterBND(Register value) {
			if (!value.IsBND()) throw new ArgumentException($"Invalid register {value}. Must be a BND register", nameof(value));
			Value = value;
		} 

		/// <summary>
		/// The register value.
		/// </summary>
		public readonly Register Value;

		/// <summary>
		/// Converts a <see cref="AssemblerRegisterBND"/> to a <see cref="Register"/>.
		/// </summary>
		/// <param name="reg">AssemblerRegisterBND</param>
		/// <returns></returns>
		public static implicit operator Register(AssemblerRegisterBND reg) {
			return reg.Value;
		}

		/// <inheritdoc />
		public bool Equals(AssemblerRegisterBND other) => Value == other.Value;

		/// <inheritdoc />
		public override int GetHashCode() => (int) Value;
		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is AssemblerRegisterBND other && Equals(other);

		/// <summary>
		/// Equality operator for <see cref="AssemblerRegisterBND"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator ==(AssemblerRegisterBND left, AssemblerRegisterBND right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="AssemblerRegisterBND"/>
		/// </summary>
		/// <param name="left">Register</param>
		/// <param name="right">Register</param>
		/// <returns></returns>
		public static bool operator !=(AssemblerRegisterBND left, AssemblerRegisterBND right) => !left.Equals(right);
	}

}
#endif
