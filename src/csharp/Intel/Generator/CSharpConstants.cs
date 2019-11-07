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

using System.IO;
using System.Linq;

namespace Generator {
	static class CSharpConstants {
		public const string IcedNamespace = "Iced.Intel";
		public const string DecoderNamespace = "Iced.Intel.DecoderInternal";
		public const string InstructionInfoNamespace = "Iced.Intel.InstructionInfoInternal";
		public const string FormatterNamespace = "Iced.Intel.FormatterInternal";
		public const string GasFormatterNamespace = "Iced.Intel.GasFormatterInternal";
		public const string IntelFormatterNamespace = "Iced.Intel.IntelFormatterInternal";
		public const string MasmFormatterNamespace = "Iced.Intel.MasmFormatterInternal";
		public const string NasmFormatterNamespace = "Iced.Intel.NasmFormatterInternal";

		public const string DecoderDefine = "!NO_DECODER";
		public const string InstructionInfoDefine = "!NO_INSTR_INFO";
		public const string DecoderOrEncoderDefine = "!NO_DECODER || !NO_ENCODER";
		public const string AnyFormatterDefine = "(!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER";
		public const string GasFormatterDefine = "!NO_GAS_FORMATTER && !NO_FORMATTER";
		public const string IntelFormatterDefine = "!NO_INTEL_FORMATTER && !NO_FORMATTER";
		public const string MasmFormatterDefine = "!NO_MASM_FORMATTER && !NO_FORMATTER";
		public const string NasmFormatterDefine = "!NO_NASM_FORMATTER && !NO_FORMATTER";

		public static string GetDirectory(ProjectDirs projectDirs, string @namespace) =>
			Path.Combine(new[] { projectDirs.CSharpDir }.Concat(@namespace.Split('.').Skip(1)).ToArray());
	}
}
