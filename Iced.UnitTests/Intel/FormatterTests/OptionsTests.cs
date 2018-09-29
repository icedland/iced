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
	}
}
#endif
