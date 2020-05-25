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
		public const string BlockEncoderNamespace = "Iced.Intel.BlockEncoderInternal";
		public const string DecoderNamespace = "Iced.Intel.DecoderInternal";
		public const string EncoderNamespace = "Iced.Intel.EncoderInternal";
		public const string InstructionInfoNamespace = "Iced.Intel.InstructionInfoInternal";
		public const string FormatterNamespace = "Iced.Intel.FormatterInternal";
		public const string GasFormatterNamespace = "Iced.Intel.GasFormatterInternal";
		public const string IntelFormatterNamespace = "Iced.Intel.IntelFormatterInternal";
		public const string MasmFormatterNamespace = "Iced.Intel.MasmFormatterInternal";
		public const string NasmFormatterNamespace = "Iced.Intel.NasmFormatterInternal";

		public const string IcedUnitTestsNamespace = "Iced.UnitTests.Intel";

		public const string HasSpanDefine = "HAS_SPAN";
		public const string DecoderDefine = "DECODER";
		public const string VexDefine = "!NO_VEX";
		public const string XopDefine = "!NO_XOP";
		public const string EvexDefine = "!NO_EVEX";
		public const string D3nowDefine = "!NO_D3NOW";
		public const string DecoderVexDefine = "DECODER && !NO_VEX";
		public const string DecoderXopDefine = "DECODER && !NO_XOP";
		public const string DecoderVexOrXopDefine = "DECODER && (!NO_VEX || !NO_XOP)";
		public const string DecoderEvexDefine = "DECODER && !NO_EVEX";
		public const string EncoderDefine = "ENCODER";
		public const string CodeAssemblerDefine = "ENCODER && BLOCK_ENCODER && CODE_ASSEMBLER";
		public const string OpCodeInfoDefine = "ENCODER && OPCODE_INFO";
		public const string InstructionInfoDefine = "INSTR_INFO";
		public const string DecoderOrEncoderDefine = "DECODER || ENCODER";
		public const string DecoderOrEncoderOrInstrInfoDefine = "DECODER || ENCODER || INSTR_INFO";
		public const string AnyFormatterDefine = "GAS || INTEL || MASM || NASM";
		public const string GasFormatterDefine = "GAS";
		public const string IntelFormatterDefine = "INTEL";
		public const string MasmFormatterDefine = "MASM";
		public const string NasmFormatterDefine = "NASM";

		public const string PragmaMissingDocsDisable = "#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member";
		public const string PragmaMissingDocsRestore = "#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member";

		public static string GetDirectory(GeneratorContext generatorContext, string @namespace) =>
			Path.Combine(new[] { generatorContext.CSharpDir }.Concat(@namespace.Split('.').Skip(1)).ToArray());
	}
}
