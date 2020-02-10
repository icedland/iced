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
