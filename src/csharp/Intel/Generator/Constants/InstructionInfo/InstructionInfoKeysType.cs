// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;

namespace Generator.Constants.InstructionInfo {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstructionInfoKeysType {
		InstructionInfoKeysType(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.InstructionInfoKeys, ConstantsTypeFlags.None, default, GetConstants());
			genTypes.Add(type);
		}

		static Constant[] GetConstants() =>
			typeof(InstructionInfoKeys).GetFields().Where(a => a.IsLiteral).OrderBy(a => a.MetadataToken).
				Select(a => new Constant(ConstantKind.String, a.Name, a.GetRawConstantValue() ?? throw new InvalidOperationException())).ToArray();
	}
}
