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

using System;
using System.Linq;

namespace Generator.Enums {
	interface IEnumsGenerator {
		void Generate(EnumType enumType);
	}

	sealed class EnumsGenerator {
		readonly ProjectDirs projectDirs;

		public EnumsGenerator(ProjectDirs projectDirs) => this.projectDirs = projectDirs;

		static readonly EnumType[] allEnums = new EnumType[] {
			CodeEnum.Instance,
			CodeSizeEnum.Instance,
			CpuidFeatureEnum.Instance,
			CpuidFeatureInternalEnum.Instance,
			DecoderOptionsEnum.Instance,
			EvexOpCodeHandlerKindEnum.Instance,
			HandlerFlagsEnum.Instance,
			LegacyHandlerFlagsEnum.Instance,
			MemorySizeEnum.Instance,
			OpCodeHandlerKindEnum.Instance,
			PseudoOpsKindEnum.Instance,
			RegisterEnum.Instance,
			SerializedDataKindEnum.Instance,
			TupleTypeEnum.Instance,
			VexOpCodeHandlerKindEnum.Instance,
			MnemonicEnum.Instance,
			GasCtorKindEnum.Instance,
			IntelCtorKindEnum.Instance,
			MasmCtorKindEnum.Instance,
			NasmCtorKindEnum.Instance,
			GasSizeOverrideEnum.Instance,
			GasInstrOpInfoFlagsEnum.Instance,
			IntelSizeOverrideEnum.Instance,
			IntelBranchSizeInfoEnum.Instance,
			IntelInstrOpInfoFlagsEnum.Instance,
			MasmInstrOpInfoFlagsEnum.Instance,
			NasmSignExtendInfoEnum.Instance,
			NasmSizeOverrideEnum.Instance,
			NasmBranchSizeInfoEnum.Instance,
			NasmInstrOpInfoFlagsEnum.Instance,
			RoundingControlEnum.Instance,
			OpKindEnum.Instance,
			Instruction.CodeFlagsEnum.Instance,
			Instruction.MemoryFlagsEnum.Instance,
			Instruction.OpKindFlagsEnum.Instance,
		};

		public void Generate() {
			if (allEnums.Select(a => a.EnumKind).ToHashSet().Count != Enum.GetValues(typeof(EnumKind)).Length)
				throw new InvalidOperationException($"Missing at least one {nameof(EnumKind)} value");

			var generators = new IEnumsGenerator[(int)TargetLanguage.Last] {
				new CSharp.CSharpEnumsGenerator(projectDirs),
				new Rust.RustEnumsGenerator(projectDirs),
			};

			foreach (var generator in generators) {
				foreach (var enumType in allEnums)
					generator.Generate(enumType);
			}
		}
	}
}
