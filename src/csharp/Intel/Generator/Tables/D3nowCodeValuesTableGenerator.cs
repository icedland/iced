// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;
using Generator.Enums;

namespace Generator.Tables {
	abstract class D3nowCodeValuesTableGenerator {
		protected abstract void Generate((int index, EnumValue enumValue)[] infos);

		protected readonly GenTypes genTypes;

		protected D3nowCodeValuesTableGenerator(GenTypes genTypes) =>
			this.genTypes = genTypes;

		public void Generate() {
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var infos = defs.Where(a => a.Encoding == EncodingKind.D3NOW).
				Select(a => (index: (int)a.OpCode, enumValue: a.Code)).
				OrderBy(a => a.index).
				ToArray();
			Generate(infos);
		}
	}
}
