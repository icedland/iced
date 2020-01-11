namespace Iced.Intel
{
	/// <summary>
	/// Memory operand factory.
	/// </summary>
	public readonly struct AssemblerMemoryOperandFactory {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="size">Size of this memory operand.</param>
		public AssemblerMemoryOperandFactory(MemoryOperandSize size) {
			Prefix = Register.None;
			Size = size;
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="size">Size of this memory operand.</param>
		/// <param name="prefix">The prefix register.</param>
		public AssemblerMemoryOperandFactory(MemoryOperandSize size, Register prefix) {
			Size = size;
			Prefix = prefix;
		}

		/// <summary>
		/// Size of this memory operand.
		/// </summary>
		public readonly MemoryOperandSize Size;

		/// <summary>
		/// Prefix register.
		/// </summary>
		public readonly Register Prefix;

		/// <summary>
		/// Use the ES register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory es => new AssemblerMemoryOperandFactory(Size, Register.ES); 
		/// <summary>
		/// Use the CS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory cs => new AssemblerMemoryOperandFactory(Size, Register.CS); 
		/// <summary>
		/// Use the CS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory ss => new AssemblerMemoryOperandFactory(Size, Register.SS); 
		/// <summary>
		/// Use the DS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory ds => new AssemblerMemoryOperandFactory(Size, Register.DS); 
		/// <summary>
		/// Use the FS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory fs => new AssemblerMemoryOperandFactory(Size, Register.FS); 
		/// <summary>
		/// Use the GS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory gs => new AssemblerMemoryOperandFactory(Size, Register.GS); 
		
		/// <summary>
		/// Specify the content of the memory operand (Base + Index * Scale + Displacement).
		/// </summary>
		/// <param name="operand">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerMemoryOperand operand] => new AssemblerMemoryOperand(Size, Prefix, operand.Base, operand.Index, operand.Scale, operand.Displacement);

		/// <summary>
		/// Specify the base register used with this memory operand (Base + Index * Scale + Displacement)
		/// </summary>
		/// <param name="register">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerRegister register] => new AssemblerMemoryOperand(Size, Prefix, register, Register.None, 0, 0);
		
		/// <summary>
		/// Specify the offset displacement with this memory operand (Base + Index * Scale + Displacement)
		/// </summary>
		/// <param name="offset">Displacement of this memory operand.</param>
		public AssemblerMemoryOperand this[long offset] => new AssemblerMemoryOperand(Size, Prefix, Register.None, Register.None, 0, offset);
	}
}
