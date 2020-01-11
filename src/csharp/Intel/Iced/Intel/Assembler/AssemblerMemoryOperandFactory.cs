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
			Flags = AssemblerOperandFlags.None;
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="size">Size of this memory operand.</param>
		/// <param name="prefix">Register prefix</param>
		/// <param name="flags">Flags</param>
		public AssemblerMemoryOperandFactory(MemoryOperandSize size, Register prefix, AssemblerOperandFlags flags) {
			Size = size;
			Prefix = prefix;
			Flags = flags;
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
		/// Gets the mask associated with this operand.
		/// </summary>
		public readonly AssemblerOperandFlags Flags;

		/// <summary>
		/// Use the ES register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory es => new AssemblerMemoryOperandFactory(Size, Register.ES, Flags); 
		/// <summary>
		/// Use the CS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory cs => new AssemblerMemoryOperandFactory(Size, Register.CS, Flags); 
		/// <summary>
		/// Use the CS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory ss => new AssemblerMemoryOperandFactory(Size, Register.SS, Flags); 
		/// <summary>
		/// Use the DS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory ds => new AssemblerMemoryOperandFactory(Size, Register.DS, Flags); 
		/// <summary>
		/// Use the FS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory fs => new AssemblerMemoryOperandFactory(Size, Register.FS, Flags); 
		/// <summary>
		/// Use the GS register with this memory operand.
		/// </summary>
		public AssemblerMemoryOperandFactory gs => new AssemblerMemoryOperandFactory(Size, Register.GS, Flags);
		/// <summary>
		/// Use broadcast.
		/// </summary>
		public AssemblerMemoryOperandFactory bcst => new AssemblerMemoryOperandFactory(Size, Prefix, Flags | AssemblerOperandFlags.Broadcast);
		/// <summary>
		/// Specify the content of the memory operand (Base + Index * Scale + Displacement).
		/// </summary>
		/// <param name="operand">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerMemoryOperand operand] => new AssemblerMemoryOperand(Size, Prefix, operand.Base, operand.Index, operand.Scale, operand.Displacement, Flags);

		/// <summary>
		/// Specify the base register used with this memory operand (Base + Index * Scale + Displacement)
		/// </summary>
		/// <param name="register">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerRegister register] => new AssemblerMemoryOperand(Size, Prefix, register, Register.None, 1, 0, Flags);
		
		/// <summary>
		/// Specify the offset displacement with this memory operand (Base + Index * Scale + Displacement)
		/// </summary>
		/// <param name="offset">Displacement of this memory operand.</param>
		public AssemblerMemoryOperand this[long offset] => new AssemblerMemoryOperand(Size, Prefix, Register.None, Register.None, 1, offset, Flags);
	}
}
