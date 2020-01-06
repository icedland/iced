using System.Diagnostics;

namespace Iced.Intel
{
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	public readonly struct ExtendedRegister {
		public ExtendedRegister(Register value) {
			Value = value;
		}

		public readonly Register Value;

		public bool IsEmpty => Value == Register.None;


		public static implicit operator ExtendedRegister(Register value) {
			return new ExtendedRegister(value);
		}

		public static implicit operator Register(ExtendedRegister reg) {
			return reg.Value;
		}

		public static ExtendedMemoryOperand operator +(ExtendedRegister left, ExtendedRegister right) {
			return new ExtendedMemoryOperand(MemoryOperandSize.None, Register.None, left, right, 1, 0);
		}

		public static ExtendedMemoryOperand operator +(ExtendedRegister left, int displacement) {
			return new ExtendedMemoryOperand(MemoryOperandSize.None, Register.None, left, Register.None, 1, displacement);
		}

		public static ExtendedMemoryOperand operator *(ExtendedRegister left, int scale) {
			return new ExtendedMemoryOperand(MemoryOperandSize.None, Register.None, Register.None, left, scale, 0);
		}
	}
}
