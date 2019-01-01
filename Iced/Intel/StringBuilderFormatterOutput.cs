/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Text;

namespace Iced.Intel {
	/// <summary>
	/// Formatter output that stores the formatted text in a <see cref="StringBuilder"/>
	/// </summary>
	public sealed class StringBuilderFormatterOutput : FormatterOutput {
		readonly StringBuilder sb;

		/// <summary>
		/// Constructor
		/// </summary>
		public StringBuilderFormatterOutput() => sb = new StringBuilder();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sb">String builder</param>
		public StringBuilderFormatterOutput(StringBuilder sb) => this.sb = sb ?? throw new ArgumentNullException(nameof(sb));

		/// <summary>
		/// Writes text and text kind
		/// </summary>
		/// <param name="text">Text, can be an empty string</param>
		/// <param name="kind">Text kind. This value can be identical to the previous value passed to this method. It's
		/// the responsibility of the implementer to merge any such strings if needed.</param>
		public override void Write(string text, FormatterOutputTextKind kind) => sb.Append(text);

		/// <summary>
		/// Clears the <see cref="StringBuilder"/> instance so this class can be reused to format the next instruction
		/// </summary>
		public void Reset() => sb.Clear();

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
