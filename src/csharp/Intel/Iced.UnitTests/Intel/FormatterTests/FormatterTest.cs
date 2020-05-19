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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public readonly struct InstructionInfo {
		public readonly int Bitness;
		public readonly string HexBytes;
		public readonly Code Code;
		public readonly DecoderOptions Options;
		public InstructionInfo(int bitness, string hexBytes, Code code) {
			Bitness = bitness;
			HexBytes = hexBytes;
			Code = code;
			Options = DecoderOptions.None;
		}
		public InstructionInfo(int bitness, string hexBytes, Code code, DecoderOptions options) {
			Bitness = bitness;
			HexBytes = hexBytes;
			Code = code;
			Options = options;
		}
	}

	public abstract class FormatterTest {
		static (InstructionInfo[] infos, HashSet<int> ignored) instrInfos16;
		static (InstructionInfo[] infos, HashSet<int> ignored) instrInfos32;
		static (InstructionInfo[] infos, HashSet<int> ignored) instrInfos64;
		static (InstructionInfo[] infos, HashSet<int> ignored) instrInfos16_Misc;
		static (InstructionInfo[] infos, HashSet<int> ignored) instrInfos32_Misc;
		static (InstructionInfo[] infos, HashSet<int> ignored) instrInfos64_Misc;

		public static (InstructionInfo[] infos, HashSet<int> ignored) GetInstructionInfos(int bitness, bool isMisc) {
			if (isMisc) {
				return bitness switch {
					16 => GetInstructionInfos(ref instrInfos16_Misc, bitness, isMisc),
					32 => GetInstructionInfos(ref instrInfos32_Misc, bitness, isMisc),
					64 => GetInstructionInfos(ref instrInfos64_Misc, bitness, isMisc),
					_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
				};
			}
			else {
				return bitness switch {
					16 => GetInstructionInfos(ref instrInfos16, bitness, isMisc),
					32 => GetInstructionInfos(ref instrInfos32, bitness, isMisc),
					64 => GetInstructionInfos(ref instrInfos64, bitness, isMisc),
					_ => throw new ArgumentOutOfRangeException(nameof(bitness)),
				};
			}
		}

		static (InstructionInfo[] infos, HashSet<int> ignored) GetInstructionInfos(ref (InstructionInfo[] infos, HashSet<int> ignored) data, int bitness, bool isMisc) {
			if (data.infos is null) {
				var filename = "InstructionInfos" + bitness;
				if (isMisc)
					filename += "_Misc";
				data.ignored = new HashSet<int>();
				data.infos = GetInstructionInfos(filename, bitness, data.ignored).ToArray();
			}
			return data;
		}

		static readonly char[] sep = new[] { ',' };
		static IEnumerable<InstructionInfo> GetInstructionInfos(string filename, int bitness, HashSet<int> ignored) {
			int lineNo = 0;
			int testCaseNo = 0;
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
					if (!ToEnumConverter.TryDecoderOptions(parts[2].Trim(), out options))
						throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
				}
				else
					throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
				var hexBytes = parts[0].Trim();
				var codeStr = parts[1].Trim();
				if (CodeUtils.IsIgnored(codeStr))
					ignored.Add(testCaseNo);
				else {
					if (!ToEnumConverter.TryCode(codeStr, out var code))
						throw new InvalidOperationException($"Invalid line #{lineNo} in file {filename}");
					yield return new InstructionInfo(bitness, hexBytes, code, options);
				}
				testCaseNo++;
			}
		}

		protected static IEnumerable<object[]> GetFormatData(int bitness, string formatterDir, string formattedStringsFile, bool isMisc = false) {
			var data = GetInstructionInfos(bitness, isMisc);
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, $"Test{bitness}_{formattedStringsFile}")).ToArray();
			return GetFormatData(data.infos, data.ignored, formattedStrings);
		}

		static IEnumerable<object[]> GetFormatData(InstructionInfo[] infos, HashSet<int> ignored, string[] formattedStrings) {
			formattedStrings = Utils.Filter(formattedStrings, ignored);
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i], formattedStrings[i] };
			return res;
		}

		protected static IEnumerable<object[]> GetFormatData(int bitness, (string hexBytes, Instruction instruction)[] infos, string formatterDir, string formattedStringsFile) {
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, $"Test{bitness}_{formattedStringsFile}")).ToArray();
			return GetFormatData(infos, formattedStrings);
		}

		static IEnumerable<object[]> GetFormatData((string hexBytes, Instruction instruction)[] infos, string[] formattedStrings) {
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].hexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].hexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i].instruction, formattedStrings[i] };
			return res;
		}

		protected void FormatBase(int index, InstructionInfo info, string formattedString, Formatter formatter) =>
			FormatterTestUtils.FormatTest(info.Bitness, info.HexBytes, info.Code, info.Options, formattedString, formatter);

		protected void FormatBase(int index, Instruction instruction, string formattedString, Formatter formatter) =>
			FormatterTestUtils.FormatTest(instruction, formattedString, formatter);
	}
}
#endif
