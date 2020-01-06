namespace Iced.Intel
{
	public static partial class ExtendedRegisters {
		public static readonly ExtendedMemoryOperandFactory __ = new ExtendedMemoryOperandFactory(MemoryOperandSize.None);
		public static readonly ExtendedMemoryOperandFactory __byte_ptr = new ExtendedMemoryOperandFactory(MemoryOperandSize.BytePtr);
		public static readonly ExtendedMemoryOperandFactory __word_ptr = new ExtendedMemoryOperandFactory(MemoryOperandSize.WordPtr);
		public static readonly ExtendedMemoryOperandFactory __dword_ptr = new ExtendedMemoryOperandFactory(MemoryOperandSize.DwordPtr);
		public static readonly ExtendedMemoryOperandFactory __qword_ptr = new ExtendedMemoryOperandFactory(MemoryOperandSize.QwordPtr);
	}
}
