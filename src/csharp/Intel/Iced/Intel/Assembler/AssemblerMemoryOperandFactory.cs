// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System.ComponentModel;

namespace Iced.Intel {
	/// <summary>
	/// Memory operand factory.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly partial struct AssemblerMemoryOperandFactory {
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="size">Size of this memory operand.</param>
		internal AssemblerMemoryOperandFactory(MemoryOperandSize size) {
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
		internal AssemblerMemoryOperandFactory(MemoryOperandSize size, Register prefix, AssemblerOperandFlags flags) {
			Size = size;
			Prefix = prefix;
			Flags = flags;
		}

		/// <summary>
		/// Size of this memory operand.
		/// </summary>
		internal readonly MemoryOperandSize Size;

		/// <summary>
		/// Prefix register.
		/// </summary>
		internal readonly Register Prefix;

		/// <summary>
		/// Gets the mask associated with this operand.
		/// </summary>
		internal readonly AssemblerOperandFlags Flags;

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
		/// Specify the content of the memory operand (Base + Index * Scale + Displacement).
		/// </summary>
		/// <param name="operand">Size of this memory operand.</param>
		public AssemblerMemoryOperand this[AssemblerMemoryOperand operand] => new AssemblerMemoryOperand(Size, Prefix, operand.Base, operand.Index, operand.Scale, operand.Displacement, Flags);

		/// <summary>
		/// Specify a long offset displacement.
		/// </summary>
		/// <param name="offset">Offset of this memory operand.</param>
		public AssemblerMemoryOperand this[long offset] => new AssemblerMemoryOperand(Size, Prefix, Register.None, Register.None, 1, offset, Flags);

		/// <summary>
		/// Specify a ulong offset displacement.
		/// </summary>
		/// <param name="offset">Offset of this memory operand.</param>
		public AssemblerMemoryOperand this[ulong offset] => new AssemblerMemoryOperand(Size, Prefix, Register.None, Register.None, 1, (long)offset, Flags);

		/// <summary>
		/// Specify a memory operand with a label.
		/// </summary>
		public AssemblerMemoryOperand this[Label label] => new AssemblerMemoryOperand(Size, Prefix, Register.RIP, Register.None, 1, (long)label.Id, Flags);
	}
}
#endif
