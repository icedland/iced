// SPDX-License-Identifier: MIT
// Copyright wtfsck@protonmail.com
// Copyright iced contributors

using System.Linq;

namespace Generator.Enums.Formatter.Masm {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InstrOpKindEnum {
		InstrOpKindEnum(GenTypes genTypes) {
			var enumType = new EnumType("InstrOpKind", TypeIds.MasmInstrOpKind, null, GetValues(genTypes), EnumTypeFlags.NoInitialize);
			genTypes.Add(enumType);
		}

		static EnumValue[] GetValues(GenTypes genTypes) {
			var list = genTypes[TypeIds.OpKind].Values.Select(a => new EnumValue(a.Value, a.RawName, null)).ToList();
			// Extra opkinds
			list.Add(new EnumValue((uint)list.Count, "ExtraImmediate8_Value3", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareByte", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareWord", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareDword", null));
			list.Add(new EnumValue((uint)list.Count, "DeclareQword", null));
			return list.ToArray();
		}
	}
}
