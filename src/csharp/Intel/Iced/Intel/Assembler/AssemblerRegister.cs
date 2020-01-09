using System;
using System.Diagnostics;

namespace Iced.Intel
{
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	public readonly struct AssemblerRegister : IEquatable<AssemblerRegister> {
		public AssemblerRegister(Register value) {
			Value = value;
		}

		public readonly Register Value;

		public bool IsEmpty => Value == Register.None;

		public bool IsGPR() => Value.IsGPR();

		public bool IsGPR8() => Value.IsGPR8();

		public bool IsGPR16() => Value.IsGPR16();
		
		public bool IsGPR32() => Value.IsGPR32();
		
		public bool IsGPR64() => Value.IsGPR64();
		
		public bool IsK() => Value.IsK();

		public bool IsMM() => Value.IsMM();

		public bool IsXMM() => Value.IsXMM();

		public bool IsYMM() => Value.IsYMM();

		public bool IsZMM() => Value.IsZMM();

		public bool IsCR() => Value.IsCR();

		public bool IsDR() => Value.IsDR();

		public bool IsTR() => Value.IsTR();

		public bool IsST() => Value.IsST();

		public static implicit operator AssemblerRegister(Register value) {
			return new AssemblerRegister(value);
		}

		public static implicit operator Register(AssemblerRegister reg) {
			return reg.Value;
		}

		public static AssemblerMemoryOperand operator +(AssemblerRegister left, AssemblerRegister right) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0);
		}

		public static AssemblerMemoryOperand operator +(AssemblerRegister left, int displacement) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement);
		}

		public static AssemblerMemoryOperand operator *(AssemblerRegister left, int scale) {
			return new AssemblerMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0);
		}

		public bool Equals(AssemblerRegister other) => Value == other.Value;

		public override bool Equals(object? obj) => obj is AssemblerRegister other && Equals(other);

		public override int GetHashCode() => (int) Value;

		public static bool operator ==(AssemblerRegister left, AssemblerRegister right) => left.Equals(right);

		public static bool operator !=(AssemblerRegister left, AssemblerRegister right) => !left.Equals(right);
	}
}
