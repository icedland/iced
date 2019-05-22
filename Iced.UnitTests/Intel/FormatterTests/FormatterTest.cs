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
	public readonly struct InstructionInfo {
		public readonly int CodeSize;
		public readonly string HexBytes;
		public readonly Code Code;
		public readonly DecoderOptions Options;
		public InstructionInfo(int codeSize, string hexBytes, Code code) {
			CodeSize = codeSize;
			HexBytes = hexBytes;
			Code = code;
			Options = DecoderOptions.None;
		}
		public InstructionInfo(int codeSize, string hexBytes, Code code, DecoderOptions options) {
			CodeSize = codeSize;
			HexBytes = hexBytes;
			Code = code;
			Options = options;
		}
	}

	public abstract class FormatterTest {
		static InstructionInfo[] instrInfos16;
		static InstructionInfo[] instrInfos32;
		static InstructionInfo[] instrInfos64;
		static InstructionInfo[] instrInfos16_Misc;
		static InstructionInfo[] instrInfos32_Misc;
		static InstructionInfo[] instrInfos64_Misc;

		public static InstructionInfo[] GetInstructionInfos(int bitness, bool isMisc) {
			if (isMisc) {
				switch (bitness) {
				case 16: return GetInstructionInfos(ref instrInfos16_Misc, bitness, isMisc);
				case 32: return GetInstructionInfos(ref instrInfos32_Misc, bitness, isMisc);
				case 64: return GetInstructionInfos(ref instrInfos64_Misc, bitness, isMisc);
				default: throw new ArgumentOutOfRangeException(nameof(bitness));
				}
			}
			else {
				switch (bitness) {
				case 16: return GetInstructionInfos(ref instrInfos16, bitness, isMisc);
				case 32: return GetInstructionInfos(ref instrInfos32, bitness, isMisc);
				case 64: return GetInstructionInfos(ref instrInfos64, bitness, isMisc);
				default: throw new ArgumentOutOfRangeException(nameof(bitness));
				}
			}
		}

		static InstructionInfo[] GetInstructionInfos(ref InstructionInfo[] instrInfos, int bitness, bool isMisc) {
			if (instrInfos == null) {
				var filename = "InstructionInfos" + bitness;
				if (isMisc)
					filename += "_Misc";
				instrInfos = GetInstructionInfos(filename, bitness).ToArray();
			}
			return instrInfos;
		}

		static Dictionary<string, Code> CreateToCode() {
			var dict = new Dictionary<string, Code>(StringComparer.Ordinal);
			foreach (var f in typeof(Code).GetFields()) {
				if (!f.IsLiteral)
					continue;
				var code = (Code)f.GetValue(null);
				var name = f.Name;
				dict.Add(name, code);
			}
			return dict;
		}

		static Dictionary<string, DecoderOptions> CreateToDecoderOptions() {
			var dict = new Dictionary<string, DecoderOptions>(StringComparer.Ordinal);
			foreach (var f in typeof(DecoderOptions).GetFields()) {
				if (!f.IsLiteral)
					continue;
				var value = (DecoderOptions)f.GetValue(null);
				var name = f.Name;
				dict.Add(name, value);
			}
			return dict;
		}

		static readonly char[] sep = new[] { ',' };
		static readonly Dictionary<string, Code> toCode = CreateToCode();
		static readonly Dictionary<string, DecoderOptions> toDecoderOptions = CreateToDecoderOptions();
		static IEnumerable<InstructionInfo> GetInstructionInfos(string filename, int bitness) {
			int lineNo = 0;
			filename = FileUtils.GetFormatterFilename(filename);
			foreach (var line in File.ReadLines(filename)) {
				lineNo++;
				if (line.Length == 0 || line[0] == '#')
					continue;
				var parts = line.Split(sep);
				DecoderOptions options;
				if (parts.Length == 2)
					options = default;
				else if (parts.Length == 3) {
					if (!toDecoderOptions.TryGetValue(parts[2].Trim(), out options))
						throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
				}
				else
					throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
				var hexBytes = parts[0].Trim();
				if (!toCode.TryGetValue(parts[1].Trim(), out var code))
					throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
				yield return new InstructionInfo(bitness, hexBytes, code, options);
			}
		}

		protected static IEnumerable<object[]> GetFormatData(int bitness, string formatterDir, string formattedStringsFile, bool isMisc = false) {
			var infos = GetInstructionInfos(bitness, isMisc);
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedStringsFile)).ToArray();
			return GetFormatData(infos, formattedStrings);
		}

		protected static IEnumerable<object[]> GetFormatData(InstructionInfo[] infos, string[] formattedStrings) {
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i], formattedStrings[i] };
			return res;
		}

		protected static IEnumerable<object[]> GetFormatData((string hexBytes, Instruction instruction)[] infos, string formatterDir, string formattedStringsFile) {
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedStringsFile)).ToArray();
			return GetFormatData(infos, formattedStrings);
		}

		protected static IEnumerable<object[]> GetFormatData((string hexBytes, Instruction instruction)[] infos, string[] formattedStrings) {
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].hexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].hexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i].instruction, formattedStrings[i] };
			return res;
		}

		protected void FormatBase(int index, InstructionInfo info, string formattedString, Formatter formatter) =>
			FormatterTestUtils.FormatTest(info.CodeSize, info.HexBytes, info.Code, info.Options, formattedString, formatter);

		protected void FormatBase(int index, Instruction instr, string formattedString, Formatter formatter) =>
			FormatterTestUtils.FormatTest(instr, formattedString, formatter);
	}
}
#endif
