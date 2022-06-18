// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;

namespace Iced.Intel {
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
		/// Suppress all exceptions (.sae).
		/// </summary>
		SuppressAllExceptions = 1 << 2,
		/// <summary>
		/// Round to nearest (.rn_sae).
		/// </summary>
		RoundToNearest = RoundingControl.RoundToNearest << 3,
		/// <summary>
		/// Round to down (.rd_sae).
		/// </summary>
		RoundDown = RoundingControl.RoundDown << 3,
		/// <summary>
		/// Round to up (.ru_sae).
		/// </summary>
		RoundUp = RoundingControl.RoundUp << 3,
		/// <summary>
		/// Round towards zero (.rz_sae).
		/// </summary>
		RoundTowardZero = RoundingControl.RoundTowardZero << 3,
		/// <summary>
		/// RoundingControl mask.
		/// </summary>
		RoundingControlMask = 0x7 << 3,
		/// <summary>
		/// Mask register K1.
		/// </summary>
		K1 = 1 << 6,
		/// <summary>
		/// Mask register K2.
		/// </summary>
		K2 = 2 << 6,
		/// <summary>
		/// Mask register K3.
		/// </summary>
		K3 = 3 << 6,
		/// <summary>
		/// Mask register K4.
		/// </summary>
		K4 = 4 << 6,
		/// <summary>
		/// Mask register K5.
		/// </summary>
		K5 = 5 << 6,
		/// <summary>
		/// Mask register K6.
		/// </summary>
		K6 = 6 << 6,
		/// <summary>
		/// Mask register K7.
		/// </summary>
		K7 = 7 << 6,
		/// <summary>
		/// Mask for K registers.
		/// </summary>
		RegisterMask = 0x7 << 6,
	}
}
#endif
