using System.Diagnostics;

namespace Iced.Intel
{
	[DebuggerDisplay("{Base} + {Index} * {Scale} + {Displacement}")]
	public readonly struct AssemblerMemoryOperand {
		public AssemblerMemoryOperand(MemoryOperandSize size, Register prefix, Register @base, Register index, int scale, int displacement) {
			Size = size;
			Prefix = prefix;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = displacement;
		}

		public readonly MemoryOperandSize Size;

		public readonly Register Prefix;

		public readonly Register Base;

		public readonly Register Index;

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

		public static AssemblerMemoryOperand operator +(AssemblerMemoryOperand left, AssemblerRegister right) {
			var hasBase = left.Base != Register.None;
			return new AssemblerMemoryOperand(left.Size, Register.None, hasBase ? left.Base : right.Value, hasBase ? right.Value : left.Index, left.Scale, left.Displacement);
		}

		public static AssemblerMemoryOperand operator +(AssemblerRegister left, AssemblerMemoryOperand right) {
			var hasBase = right.Base != Register.None;
			return new AssemblerMemoryOperand(right.Size, Register.None, hasBase ? right.Base : left.Value, hasBase ? left.Value : right.Index, right.Scale, right.Displacement);
		}

		public static AssemblerMemoryOperand operator +(AssemblerMemoryOperand left, int displacement) {
			return new AssemblerMemoryOperand(left.Size, Register.None, left.Base, left.Index, left.Scale, displacement);
		}

		public static implicit operator MemoryOperand(AssemblerMemoryOperand v) {
			return new MemoryOperand(v.Base, v.Index, v.Scale, v.Displacement, v.DisplacementSize);
		}
	}
}
