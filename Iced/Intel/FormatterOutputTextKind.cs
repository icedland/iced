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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
namespace Iced.Intel {
	/// <summary>
	/// Formatter text kind
	/// </summary>
	public enum FormatterOutputTextKind {
		/// <summary>
		/// Normal text
		/// </summary>
		Text,

		/// <summary>
		/// Assembler directive
		/// </summary>
		Directive,

		/// <summary>
		/// Any prefix
		/// </summary>
		Prefix,

		/// <summary>
		/// Any mnemonic
		/// </summary>
		Mnemonic,

		/// <summary>
		/// Any keyword
		/// </summary>
		Keyword,

		/// <summary>
		/// Any operator
		/// </summary>
		Operator,

		/// <summary>
		/// Any punctuation
		/// </summary>
		Punctuation,

		/// <summary>
		/// Number
		/// </summary>
		Number,

		/// <summary>
		/// Any register
		/// </summary>
		Register,

		/// <summary>
		/// A decorator, eg. 'sae' in '{sae}'
		/// </summary>
		Decorator,

		/// <summary>
		/// Selector value (eg. far jmp/call)
		/// </summary>
		SelectorValue,

		/// <summary>
		/// Label address (eg. JE XXXXXX)
		/// </summary>
		LabelAddress,

		/// <summary>
		/// Function address (eg. CALL XXXXX)
		/// </summary>
		FunctionAddress,

		/// <summary>
		/// Data symbol
		/// </summary>
		Data,

		/// <summary>
		/// Label symbol
		/// </summary>
		Label,

		/// <summary>
		/// Function symbol
		/// </summary>
		Function,
	}
}
#endif
