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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public readonly struct SymbolInstructionInfo {
		public readonly int CodeSize;
		public readonly string HexBytes;
		public readonly Code Code;
		public readonly Action<FormatterOptions> InitOptions;
		public readonly Action<Decoder> InitDecoder;
		internal readonly TestSymbolResolver SymbolResolver;
		internal SymbolInstructionInfo(int codeSize, string hexBytes, Code code, TestSymbolResolver symbolResolver) {
			CodeSize = codeSize;
			HexBytes = hexBytes;
			Code = code;
			InitOptions = initOptionsDefault;
			InitDecoder = initDecoderDefault;
			SymbolResolver = symbolResolver;
		}
		internal SymbolInstructionInfo(int codeSize, string hexBytes, Code code, Action<FormatterOptions> enableOption, TestSymbolResolver symbolResolver) {
			CodeSize = codeSize;
			HexBytes = hexBytes;
			Code = code;
			InitOptions = enableOption;
			InitDecoder = initDecoderDefault;
			SymbolResolver = symbolResolver;
		}
		static readonly Action<FormatterOptions> initOptionsDefault = a => { };
		static readonly Action<Decoder> initDecoderDefault = a => { };
	}

	public abstract class SymbolResolverTests {
		protected static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedStringsFile) {
			var infos = SymbolResolverTestInfos.AllInfos;
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedStringsFile)).ToArray();
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[4] { i, GetSymDispl(i), infos[i], formattedStrings[i] };
			return res;
		}

		static int GetSymDispl(int i) {
			const int DISPL = 0x123;
			return (i % 3) switch {
				0 => 0,
				1 => -DISPL,
				2 => DISPL,
				_ => throw new InvalidOperationException(),
			};
		}

		protected void FormatBase(int index, int resultDispl, in SymbolInstructionInfo info, string formattedString, (Formatter formatter, ISymbolResolver symbolResolver) formatterInfo) {
			var symbolResolver = (TestSymbolResolver)formatterInfo.symbolResolver;
			var formatter = formatterInfo.formatter;
			symbolResolver.resultDispl = resultDispl;
			info.InitOptions(formatter.Options);
			FormatterTestUtils.SimpleFormatTest(info.CodeSize, info.HexBytes, info.Code, DecoderOptions.None, formattedString, formatter, info.InitDecoder);
		}
	}
}
#endif
