namespace Iced.Intel
{
	public readonly struct AssemblerMemoryOperandFactory {
		public AssemblerMemoryOperandFactory(MemoryOperandSize size) {
			Size = size;
		}

		public readonly MemoryOperandSize Size;

		public AssemblerMemoryOperand this[AssemblerMemoryOperand operand] => new AssemblerMemoryOperand(Size, operand.Prefix, operand.Base, operand.Index, operand.Scale, operand.Displacement);

		public AssemblerMemoryOperand this[AssemblerRegister operand] => new AssemblerMemoryOperand(Size, Register.None, operand, Register.None, 0, 0);
	}
}
