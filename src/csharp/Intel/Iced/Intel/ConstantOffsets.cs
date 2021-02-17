// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER || ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Contains the offsets of the displacement and immediate. Call the decoder's GetConstantOffsets() method
	/// to get the offsets of the constants after the instruction has been decoded. The encoder has a similar method.
	/// </summary>
	public struct ConstantOffsets {
		/// <summary>
		/// The offset of the displacement, if any
		/// </summary>
		public byte DisplacementOffset;

		/// <summary>
		/// Size in bytes of the displacement, or 0 if there's no displacement
		/// </summary>
		public byte DisplacementSize;

		/// <summary>
		/// The offset of the first immediate, if any.
		/// 
		/// This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
		/// of the instruction stream, eg. <c>SHL AL,1</c>.
		/// </summary>
		public byte ImmediateOffset;

		/// <summary>
		/// Size in bytes of the first immediate, or 0 if there's no immediate
		/// </summary>
		public byte ImmediateSize;

		/// <summary>
		/// The offset of the second immediate, if any.
		/// </summary>
		public byte ImmediateOffset2;

		/// <summary>
		/// Size in bytes of the second immediate, or 0 if there's no second immediate
		/// </summary>
		public byte ImmediateSize2;

#pragma warning disable CS0169
		// pad to 8 bytes so the jitter can generate better code
		byte pad1;
		byte pad2;
#pragma warning restore CS0169

		/// <summary>
		/// <see langword="true"/> if <see cref="DisplacementOffset"/> and <see cref="DisplacementSize"/> are valid
		/// </summary>
		public readonly bool HasDisplacement => DisplacementSize != 0;

		/// <summary>
		/// <see langword="true"/> if <see cref="ImmediateOffset"/> and <see cref="ImmediateSize"/> are valid
		/// </summary>
		public readonly bool HasImmediate => ImmediateSize != 0;

		/// <summary>
		/// <see langword="true"/> if <see cref="ImmediateOffset2"/> and <see cref="ImmediateSize2"/> are valid
		/// </summary>
		public readonly bool HasImmediate2 => ImmediateSize2 != 0;
	}
}
#endif
