// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;

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

		public (RegisterKind kind, RegisterDef[] regs)[] GetRegisterGroups(Func<RegisterKind, bool>? isValidReg = null) {
			isValidReg ??= _ => true;
			var regGroups = Defs.
				Where(a => isValidReg(a.GetRegisterKind())).
				GroupBy(a => a.GetRegisterKind()).Select(a => (kind: a.Key, regs: a.ToArray())).ToList();
			regGroups.Sort((a, b) => a.CompareTo(b));

			return regGroups.ToArray();
		}
	}
}
