using System;

namespace Iced.Intel
{
	/// <summary>
	/// Assembler operand flags.
	/// </summary>
	[Flags]
	public enum AssemblerOperandFlags {
		/// <summary>
		/// No flags.
		/// </summary>
		None = 0,
		/// <summary>
		/// Broadcast.
		/// </summary>
		Broadcast = 1,
		/// <summary>
		/// Zeroing mask.
		/// </summary>
		Zeroing = 1 << 1,
		/// <summary>
		/// Mask register K0
		/// </summary>
		K0 = 1 << 2,
		/// <summary>
		/// Mask register K1.
		/// </summary>
		K1 = 2 << 2,
		/// <summary>
		/// Mask register K2.
		/// </summary>
		K2 = 3 << 2,
		/// <summary>
		/// Mask register K3.
		/// </summary>
		K3 = 4 << 2,
		/// <summary>
		/// Mask register K4.
		/// </summary>
		K4 = 5 << 2,
		/// <summary>
		/// Mask register K5.
		/// </summary>
		K5 = 6 << 2,
		/// <summary>
		/// Mask register K6.
		/// </summary>
		K6 = 7 << 2,
		/// <summary>
		/// Mask register K7.
		/// </summary>
		K7 = 8 << 2,
		/// <summary>
		/// Mask for K registers.
		/// </summary>
		RegisterMask = 0xF << 2,
	}
}
