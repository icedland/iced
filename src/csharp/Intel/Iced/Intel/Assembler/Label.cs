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

#if ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER
using System;

namespace Iced.Intel {
	/// <summary>
	/// A label that can be created by <see cref="Assembler.CreateLabel"/>.
	/// </summary>
	public struct Label : IEquatable<Label> {
		internal Label(string? name, ulong id) {
			Name = name ?? "___label";
			Id = id;
			InstructionIndex = -1;
		}

		/// <summary>
		/// Name of this label.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Id of this label.
		/// </summary>
		public readonly ulong Id;

		/// <summary>
		/// Gets the instruction index associated with this label. This is setup after calling <see cref="Assembler.Label"/>.
		/// </summary>
		public int InstructionIndex { readonly get; internal set; }

		/// <summary>
		/// <c>true</c> if this label is empty and was not created by <see cref="Assembler.CreateLabel"/>.
		/// </summary>
		public readonly bool IsEmpty => Id == 0;

		/// <inheritdoc />
		public override readonly string ToString() => $"{Name}@{Id}";

		/// <inheritdoc />
		public readonly bool Equals(Label other) => Name == other.Name && Id == other.Id;

		/// <inheritdoc />
		public override readonly bool Equals(object? obj) => obj is Label other && Equals(other);

		/// <inheritdoc />
		public override readonly int GetHashCode() {
			unchecked {
				return (Name.GetHashCode() * 397) ^ Id.GetHashCode();
			}
		}

		/// <summary>
		/// Equality operator for <see cref="Label"/>
		/// </summary>
		/// <param name="left">Label</param>
		/// <param name="right">Label</param>
		/// <returns></returns>
		public static bool operator ==(Label left, Label right) => left.Equals(right);

		/// <summary>
		/// Inequality operator for <see cref="Label"/>
		/// </summary>
		/// <param name="left">Label</param>
		/// <param name="right">Label</param>
		/// <returns></returns>
		public static bool operator !=(Label left, Label right) => !left.Equals(right);
	}
}
#endif
