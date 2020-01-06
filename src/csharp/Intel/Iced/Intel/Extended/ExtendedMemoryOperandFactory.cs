namespace Iced.Intel
{
	public readonly struct ExtendedMemoryOperandFactory {
		public ExtendedMemoryOperandFactory(MemoryOperandSize size) {
			Size = size;
		}

		public readonly MemoryOperandSize Size;

		public ExtendedMemoryOperand this[ExtendedMemoryOperand operand] => new ExtendedMemoryOperand(Size, operand.Prefix, operand.Base, operand.Index, operand.Scale, operand.Displacement);

		public ExtendedMemoryOperand this[ExtendedRegister operand] => new ExtendedMemoryOperand(Size, Register.None, operand, Register.None, 0, 0);
	}
}
