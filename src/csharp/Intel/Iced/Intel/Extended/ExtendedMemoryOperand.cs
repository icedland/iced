using System.Diagnostics;

namespace Iced.Intel
{
	[DebuggerDisplay("{Base} + {Index} * {Scale} + {Displacement}")]
	public readonly struct ExtendedMemoryOperand {
		public ExtendedMemoryOperand(MemoryOperandSize size, ExtendedRegister prefix, ExtendedRegister @base, ExtendedRegister index, int scale, int displacement) {
			Size = size;
			Prefix = prefix;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = displacement;
		}

		public readonly MemoryOperandSize Size;

		public readonly ExtendedRegister Prefix;

		public readonly ExtendedRegister Base;

		public readonly ExtendedRegister Index;

		public readonly int Scale;

		public readonly int Displacement;
		
		public int DisplacementSize {
			get {
				if (Displacement == 0) return 0;

				if (Displacement >= sbyte.MinValue && Displacement <= sbyte.MaxValue) return 1;
				if (Displacement >= short.MinValue && Displacement <= short.MaxValue) return 2;
				return 4;
			}
		}

		public static ExtendedMemoryOperand operator +(ExtendedMemoryOperand left, ExtendedRegister right) {
			return new ExtendedMemoryOperand(left.Size, Register.None, left.Base, right, left.Scale, left.Displacement);
		}

		public static ExtendedMemoryOperand operator +(ExtendedRegister left, ExtendedMemoryOperand right) {
			return new ExtendedMemoryOperand(right.Size, Register.None, left, right.Index, right.Scale, right.Displacement);
		}

		public static ExtendedMemoryOperand operator +(ExtendedMemoryOperand left, int displacement) {
			return new ExtendedMemoryOperand(left.Size, Register.None, left.Base, left.Index, left.Scale, displacement);
		}

		public static implicit operator MemoryOperand(ExtendedMemoryOperand v) {
			return new MemoryOperand(v.Base, v.Index, v.Scale, v.Displacement, v.DisplacementSize);
		}
	}


	public enum MemoryOperandSize {
		None,
		BytePtr,
		WordPtr,
		DwordPtr,
		QwordPtr,
	}
}
