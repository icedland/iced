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
using System;
using System.Collections.Generic;
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
		protected static IEnumerable<object[]> GetFormatData(InstructionInfo[] infos, string[] formattedStrings) {
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i], formattedStrings[i] };
			return res;
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
			FormatterTestUtils.FormatTest(ref instr, formattedString, formatter);
	}
}
#endif
