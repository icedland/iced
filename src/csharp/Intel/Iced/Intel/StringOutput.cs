// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System.Text;

namespace Iced.Intel {
	/// <summary>
	/// Formatter output that stores the formatted text in a <see cref="StringBuilder"/>
	/// </summary>
	public sealed class StringOutput : FormatterOutput {
		readonly StringBuilder sb;

		/// <summary>
		/// Constructor
		/// </summary>
		public StringOutput() => sb = new StringBuilder();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sb">String builder</param>
		public StringOutput(StringBuilder sb) {
			if (sb is null)
				ThrowHelper.ThrowArgumentNullException_sb();
			this.sb = sb;
		}

		/// <summary>
		/// Writes text and text kind
		/// </summary>
		/// <param name="text">Text, can be an empty string</param>
		/// <param name="kind">Text kind. This value can be identical to the previous value passed to this method. It's
		/// the responsibility of the implementer to merge any such strings if needed.</param>
		public override void Write(string text, FormatterTextKind kind) => sb.Append(text);

		/// <summary>
		/// Clears the <see cref="StringBuilder"/> instance so this class can be reused to format the next instruction
		/// </summary>
		public void Reset() => sb.Length = 0;

		/// <summary>
		/// Returns the current formatted text and clears the <see cref="StringBuilder"/> instance so this class can be reused to format the next instruction
		/// </summary>
		/// <returns></returns>
		public string ToStringAndReset() {
			var result = ToString();
			Reset();
			return result;
		}

		/// <summary>
		/// Gets the current output
		/// </summary>
		/// <returns></returns>
		public override string ToString() => sb.ToString();
	}
}
#endif
