// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;

namespace Generator.Enums.Formatter.Gas {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrOpKindEnum {
		InstrOpKindEnum(GenTypes genTypes) {
			var enumType = new EnumType("InstrOpKind", TypeIds.GasInstrOpKind, default, GetValues(genTypes), EnumTypeFlags.NoInitialize);
			genTypes.Add(enumType);
		}

		static EnumValue[] GetValues(GenTypes genTypes) {
			var list = genTypes[TypeIds.OpKind].Values.Select(a => new EnumValue(a.Value, a.RawName, default)).ToList();
			// Extra opkinds
			list.Add(new EnumValue((uint)list.Count, "Sae", default));
			list.Add(new EnumValue((uint)list.Count, "RnSae", default));
			list.Add(new EnumValue((uint)list.Count, "RdSae", default));
			list.Add(new EnumValue((uint)list.Count, "RuSae", default));
			list.Add(new EnumValue((uint)list.Count, "RzSae", default));
			list.Add(new EnumValue((uint)list.Count, "Rn", default));
			list.Add(new EnumValue((uint)list.Count, "Rd", default));
			list.Add(new EnumValue((uint)list.Count, "Ru", default));
			list.Add(new EnumValue((uint)list.Count, "Rz", default));
			list.Add(new EnumValue((uint)list.Count, "DeclareByte", default));
			list.Add(new EnumValue((uint)list.Count, "DeclareWord", default));
			list.Add(new EnumValue((uint)list.Count, "DeclareDword", default));
			list.Add(new EnumValue((uint)list.Count, "DeclareQword", default));
			return list.ToArray();
		}
	}
}
