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
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public readonly struct OptionsInstructionInfo {
		public readonly int CodeSize;
		public readonly string HexBytes;
		public readonly Code Code;
		public readonly Action<FormatterOptions> InitOptions;
		public readonly Action<Decoder> InitDecoder;
		public OptionsInstructionInfo(int codeSize, string hexBytes, Code code, Action<FormatterOptions> enableOption) {
			CodeSize = codeSize;
			HexBytes = hexBytes;
			Code = code;
			InitOptions = enableOption;
			InitDecoder = initDecoderDefault;
		}
		public OptionsInstructionInfo(int codeSize, string hexBytes, Code code, Action<FormatterOptions> enableOption, Action<Decoder> initDecoder) {
			CodeSize = codeSize;
			HexBytes = hexBytes;
			Code = code;
			InitOptions = enableOption;
			InitDecoder = initDecoder;
		}
		static readonly Action<Decoder> initDecoderDefault = a => { };
	}

	public abstract class OptionsTests {
		protected static IEnumerable<object[]> GetFormatData(OptionsInstructionInfo[] infos, string[] formattedStrings) {
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i], formattedStrings[i] };
			return res;
		}

		protected void FormatBase(int index, OptionsInstructionInfo info, string formattedString, Formatter formatter) {
			info.InitOptions(formatter.Options);
			FormatterTestUtils.SimpleFormatTest(info.CodeSize, info.HexBytes, info.Code, DecoderOptions.None, formattedString, formatter, info.InitDecoder);
		}

		static IEnumerable<T> GetEnumValues<T>() where T : struct {
			var t = typeof(T);
			if (!t.IsEnum)
				throw new InvalidOperationException();
			foreach (var value in Enum.GetValues(t))
				yield return (T)value;
		}

		protected void TestOptionsBase(FormatterOptions options) {
			{
				int min = int.MaxValue, max = int.MinValue;
				foreach (var value in GetEnumValues<NumberBase>()) {
					min = Math.Min(min, (int)value);
					max = Math.Max(max, (int)value);
					options.NumberBase = value;
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)(min - 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)(max + 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)int.MinValue);
				Assert.Throws<ArgumentOutOfRangeException>(() => options.NumberBase = (NumberBase)int.MaxValue);
			}

			{
				int min = int.MaxValue, max = int.MinValue;
				foreach (var value in GetEnumValues<MemorySizeOptions>()) {
					min = Math.Min(min, (int)value);
					max = Math.Max(max, (int)value);
					options.MemorySizeOptions = value;
				}
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)(min - 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)(max + 1));
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)int.MinValue);
				Assert.Throws<ArgumentOutOfRangeException>(() => options.MemorySizeOptions = (MemorySizeOptions)int.MaxValue);
			}
		}
	}
}
#endif
