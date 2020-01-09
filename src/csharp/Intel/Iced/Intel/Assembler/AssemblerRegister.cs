using System.Diagnostics;

namespace Iced.Intel
{
	[DebuggerDisplay("{" + nameof(Value) + "}")]
	public readonly struct AssemblerRegister {
		public AssemblerRegister(Register value) {
			Value = value;
		}

		public readonly Register Value;

		public bool IsEmpty => Value == Register.None;


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
	}
}
