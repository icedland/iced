// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;

namespace Generator.Constants.InstructionInfo {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class RflagsBitsConstantsType {
		RflagsBitsConstantsType(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.RflagsBitsConstants, ConstantsTypeFlags.None, default, GetConstants());
			genTypes.Add(type);
		}

		static Constant[] GetConstants() =>
			typeof(RflagsBitsConstants).GetFields().Where(a => a.IsLiteral).OrderBy(a => a.MetadataToken).
				Select(a => new Constant(ConstantKind.Char, a.Name, (char)(a.GetRawConstantValue() ?? throw new InvalidOperationException()))).ToArray();
	}
}
