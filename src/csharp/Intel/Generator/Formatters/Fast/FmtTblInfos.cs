// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Enums;
using Generator.Tables;

namespace Generator.Formatters.Fast {
	[TypeGen(TypeGenOrders.PreCreateInstructions)]
	sealed class FmtTblInfos : ICreatedInstructions {
		public FastFmtInstructionDef[] Infos {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return infos;
			}
		}

		FastFmtInstructionDef[] infos;
		bool filtered;

		FmtTblInfos(GenTypes genTypes) {
			infos = Array.Empty<FastFmtInstructionDef>();
			genTypes.AddObject(TypeIds.FastFmtTblInfos, this);
		}

		void ICreatedInstructions.OnCreatedInstructions(GenTypes genTypes, HashSet<EnumValue> filteredCodeValues) {
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			infos = defs.Select(a => a.Fast).ToArray();
			filtered = true;
		}
	}
}
