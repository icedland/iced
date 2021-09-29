// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;

namespace Generator.Enums.Formatter.Intel {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrOpKindEnum {
		InstrOpKindEnum(GenTypes genTypes) {
			var enumType = new EnumType("InstrOpKind", TypeIds.IntelInstrOpKind, default, GetValues(genTypes), EnumTypeFlags.NoInitialize);
			genTypes.Add(enumType);
		}

		static EnumValue[] GetValues(GenTypes genTypes) {
			var list = genTypes[TypeIds.OpKind].Values.Select(a => new EnumValue(a.Value, a.RawName, default)).ToList();
			// Extra opkinds
			list.Add(new EnumValue((uint)list.Count, "DeclareByte", default));
			list.Add(new EnumValue((uint)list.Count, "DeclareWord", default));
			list.Add(new EnumValue((uint)list.Count, "DeclareDword", default));
			list.Add(new EnumValue((uint)list.Count, "DeclareQword", default));
			return list.ToArray();
		}
	}
}
