// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;

namespace Generator.Enums.Formatter.Nasm {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrOpKindEnum {
		InstrOpKindEnum(GenTypes genTypes) {
			var enumType = new EnumType("InstrOpKind", TypeIds.NasmInstrOpKind, null, GetValues(genTypes), EnumTypeFlags.NoInitialize);
			genTypes.Add(enumType);
		}

		static EnumValue[] GetValues(GenTypes genTypes) {
			var list = genTypes[TypeIds.OpKind].Values.Select(a => new EnumValue(a.Value, a.RawName, null)).ToList();
			// Extra opkinds
			list.Add(new EnumValue((uint)list.Count, "Sae", null));
			list.Add(new EnumValue((uint)list.Count, "RnSae", null));
			list.Add(new EnumValue((uint)list.Count, "RdSae", null));
			list.Add(new EnumValue((uint)list.Count, "RuSae", null));
			list.Add(new EnumValue((uint)list.Count, "RzSae", null));
			list.Add(new EnumValue((uint)list.Count, "Rn", null));
			list.Add(new EnumValue((uint)list.Count, "Rd", null));
			list.Add(new EnumValue((uint)list.Count, "Ru", null));
			list.Add(new EnumValue((uint)list.Count, "Rz", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareByte", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareWord", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareDword", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareQword", null));
			return list.ToArray();
		}
	}
}
