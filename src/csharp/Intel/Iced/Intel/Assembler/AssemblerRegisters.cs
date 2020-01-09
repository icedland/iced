namespace Iced.Intel
{
	public static partial class AssemblerRegisters {
		public static readonly AssemblerMemoryOperandFactory __ = new AssemblerMemoryOperandFactory(MemoryOperandSize.None);
		public static readonly AssemblerMemoryOperandFactory __byte_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.BytePtr);
		public static readonly AssemblerMemoryOperandFactory __word_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.WordPtr);
		public static readonly AssemblerMemoryOperandFactory __dword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.DwordPtr);
		public static readonly AssemblerMemoryOperandFactory __qword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.QwordPtr);
	}
}
