// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
namespace Iced.Intel {
	/// <summary>
	/// Registers used for <see cref="Assembler"/>. 
	/// </summary>
	public static partial class AssemblerRegisters {
		/// <summary>
		/// Gets an un-sized memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __ = new AssemblerMemoryOperandFactory(MemoryOperandSize.None);
		/// <summary>
		/// Gets a 8-bit / byte ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __byte_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.BytePtr);
		/// <summary>
		/// Gets a 16-bit / word ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __word_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.WordPtr);
		/// <summary>
		/// Gets a 32-bit / dword ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __dword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.DwordPtr);
		/// <summary>
		/// Gets a 32-bit / dword bcst memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __dword_bcst = new AssemblerMemoryOperandFactory(MemoryOperandSize.DwordPtr, Register.None, AssemblerOperandFlags.Broadcast);
		/// <summary>
		/// Gets a 64-bit / qword ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __qword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.QwordPtr);
		/// <summary>
		/// Gets a 64-bit / mmword ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __mmword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.QwordPtr);
		/// <summary>
		/// Gets a 64-bit / qword bcst memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __qword_bcst = new AssemblerMemoryOperandFactory(MemoryOperandSize.QwordPtr, Register.None, AssemblerOperandFlags.Broadcast);
		/// <summary>
		/// Gets a 80-bit / tword ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __tword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.TwordPtr);
		/// <summary>
		/// Gets a 80-bit / tword ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __tbyte_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.TwordPtr);
		/// <summary>
		/// Gets a 16-bit segment + 32-bit / fword ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __fword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.FwordPtr);
		/// <summary>
		/// Gets a 128-bit / xmm ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __xmmword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.OwordPtr);
		/// <summary>
		/// Gets a 128-bit / oword ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __oword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.OwordPtr);
		/// <summary>
		/// Gets a 256-bit / ymm ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __ymmword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.YwordPtr);
		/// <summary>
		/// Gets a 512-bit / zmm ptr memory operand.
		/// </summary>
		public static readonly AssemblerMemoryOperandFactory __zmmword_ptr = new AssemblerMemoryOperandFactory(MemoryOperandSize.ZwordPtr);
	}
}
#endif
