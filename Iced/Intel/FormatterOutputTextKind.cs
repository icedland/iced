/*
    Copyright (C) 2018 de4dot@gmail.com

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
