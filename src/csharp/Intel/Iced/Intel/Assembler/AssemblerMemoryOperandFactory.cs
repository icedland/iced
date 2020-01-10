namespace Iced.Intel
{
	public readonly struct AssemblerMemoryOperandFactory {
		public AssemblerMemoryOperandFactory(MemoryOperandSize size) {
			Prefix = Register.None;
			Size = size;
		}

		public AssemblerMemoryOperandFactory(MemoryOperandSize size, Register prefix) {
			Size = size;
			Prefix = prefix;
		}

		public readonly MemoryOperandSize Size;

		public readonly Register Prefix;

		public AssemblerMemoryOperandFactory es => new AssemblerMemoryOperandFactory(Size, Register.ES); 
		public AssemblerMemoryOperandFactory cs => new AssemblerMemoryOperandFactory(Size, Register.CS); 
		public AssemblerMemoryOperandFactory ss => new AssemblerMemoryOperandFactory(Size, Register.SS); 
		public AssemblerMemoryOperandFactory ds => new AssemblerMemoryOperandFactory(Size, Register.DS); 
		public AssemblerMemoryOperandFactory fs => new AssemblerMemoryOperandFactory(Size, Register.FS); 
		public AssemblerMemoryOperandFactory gs => new AssemblerMemoryOperandFactory(Size, Register.GS); 
		
		public AssemblerMemoryOperand this[AssemblerMemoryOperand operand] => new AssemblerMemoryOperand(Size, Prefix, operand.Base, operand.Index, operand.Scale, operand.Displacement);

		public AssemblerMemoryOperand this[AssemblerRegister operand] => new AssemblerMemoryOperand(Size, Prefix, operand, Register.None, 0, 0);
	}
}
