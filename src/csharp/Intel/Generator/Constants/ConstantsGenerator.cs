// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Constants {
	abstract class ConstantsGenerator {
		public abstract void Generate(ConstantsType constantsType);

		protected readonly GenTypes genTypes;

		protected ConstantsGenerator(GenTypes genTypes) => this.genTypes = genTypes;

		public void Generate() {
			var allConstants = new ConstantsType[] {
				genTypes.GetConstantsType(TypeIds.IcedConstants),
				genTypes.GetConstantsType(TypeIds.DecoderTestParserConstants),
				genTypes.GetConstantsType(TypeIds.DecoderConstants),
				genTypes.GetConstantsType(TypeIds.InstructionInfoKeys),
				genTypes.GetConstantsType(TypeIds.MiscInstrInfoTestConstants),
				genTypes.GetConstantsType(TypeIds.RflagsBitsConstants),
				genTypes.GetConstantsType(TypeIds.MiscSectionNames),
				genTypes.GetConstantsType(TypeIds.OpCodeInfoKeys),
				genTypes.GetConstantsType(TypeIds.OpCodeInfoFlags),
			};

			foreach (var constantsType in allConstants)
				Generate(constantsType);
		}
	}
}
