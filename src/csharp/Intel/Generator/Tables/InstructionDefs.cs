// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;

namespace Generator.Tables {
	[TypeGen(TypeGenOrders.PreCreateInstructions)]
	sealed class InstructionDefs : ICreatedInstructions {
		public InstructionDef[] Defs {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return defs;
			}
		}

		public ImpliedAccessesDef[] ImpliedAccessesDefs {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return impliedAccessesDefs;
			}
		}

		InstructionDef[] defs;
		ImpliedAccessesDef[] impliedAccessesDefs;
		readonly (InstructionDef def, ImpliedAccesses? accesses)[] allDefs;
		bool filtered;

		InstructionDefs(GenTypes genTypes) {
			var filename = genTypes.Dirs.GetGeneratorFilename("Tables", "InstructionDefs.txt");
			allDefs = new InstructionDefsReader(genTypes, filename).Read();
			defs = Array.Empty<InstructionDef>();
			impliedAccessesDefs = Array.Empty<ImpliedAccessesDef>();
			genTypes.AddObject(TypeIds.InstructionDefs, this);
		}

		public InstructionDef[] GetDefsPreFiltered() {
			if (filtered)
				throw new InvalidOperationException();
			return allDefs.Select(a => a.def).ToArray();
		}

		double ICreatedInstructions.Order => -10000;
		void ICreatedInstructions.OnCreatedInstructions(GenTypes genTypes, HashSet<EnumValue> filteredCodeValues) {
			var defs = new List<InstructionDef>();
			var impAccFactory = new ImpliedAccessEnumFactory();
			foreach (var (def, accesses) in allDefs) {
				if (!filteredCodeValues.Contains(def.Code))
					continue;
				defs.Add(def);
				def.SetImpliedAccess(impAccFactory.Add(accesses));
			}
			this.defs = defs.ToArray();
			var (impAccType, impAccDefs) = impAccFactory.CreateEnum();
			impliedAccessesDefs = impAccDefs;
			genTypes.Add(impAccType);

			filtered = true;
		}
	}
}
