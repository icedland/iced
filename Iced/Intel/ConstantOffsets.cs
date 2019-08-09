/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if !NO_DECODER || !NO_ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Contains the offsets of the displacement and immediate. Call decoder's GetConstantOffsets() method
	/// to get the offsets of the constants after the instruction has been decoded. The encoder has a similar method.
	/// </summary>
	public struct ConstantOffsets {
		/// <summary>
		/// The offset of the displacement, if any
		/// </summary>
		public byte DisplacementOffset;

		/// <summary>
		/// Size of the displacement in bytes, or 0 if there's no displacement
		/// </summary>
		public byte DisplacementSize;

		/// <summary>
		/// The offset of the first immediate, if any.
		/// 
		/// This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
		/// of the instruction stream, eg. 'shl al,1'.
		/// </summary>
		public byte ImmediateOffset;

		/// <summary>
		/// Size of the first immediate in bytes, or 0 if there's no immediate
		/// </summary>
		public byte ImmediateSize;

		/// <summary>
		/// The offset of the second immediate, if any.
		/// </summary>
		public byte ImmediateOffset2;

		/// <summary>
		/// Size of the second immediate in bytes, or 0 if there's no second immediate
		/// </summary>
		public byte ImmediateSize2;

#pragma warning disable CS0169
		// pad to 8 bytes so the jitter can generate better code
		byte pad1;
		byte pad2;
#pragma warning restore CS0169

		/// <summary>
		/// true if <see cref="DisplacementOffset"/> is valid
		/// </summary>
		public bool HasDisplacement => DisplacementSize != 0;

		/// <summary>
		/// true if <see cref="ImmediateOffset"/> is valid
		/// </summary>
		public bool HasImmediate => ImmediateSize != 0;

		/// <summary>
		/// true if <see cref="ImmediateOffset2"/> is valid
		/// </summary>
		public bool HasImmediate2 => ImmediateSize2 != 0;
	}
}
#endif
