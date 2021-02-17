// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Tables {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class OpCodeOperandKindDefs {
		public readonly OpCodeOperandKindDef[] Defs;

		OpCodeOperandKindDefs(GenTypes genTypes) {
			Defs = CreateDefs(genTypes);
			genTypes.AddObject(TypeIds.OpCodeOperandKindDefs, this);
		}

		static OpCodeOperandKindDef[] CreateDefs(GenTypes genTypes) {
			var filename = genTypes.Dirs.GetGeneratorFilename("Tables", "OpCodeOperandKindDefs.txt");
			var reader = new OpCodeOperandKindDefsReader(genTypes, filename);
			return reader.Read();
		}
	}
}
