// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

namespace Generator.Tables {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class RegisterDefs {
		public readonly RegisterDef[] Defs;

		RegisterDefs(GenTypes genTypes) {
			Defs = CreateData(genTypes);
			genTypes.AddObject(TypeIds.RegisterDefs, this);
		}

		static RegisterDef[] CreateData(GenTypes genTypes) {
			var filename = genTypes.Dirs.GetGeneratorFilename("Tables", "RegisterDefs.txt");
			var reader = new RegisterDefsReader(genTypes, filename);
			return reader.Read();
		}
	}
}
