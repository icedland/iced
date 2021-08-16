// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Generator.Enums;

namespace Generator.Misc.Python {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class InitExportedPythonTypes {
		InitExportedPythonTypes(GenTypes genTypes) =>
			genTypes.AddObject(TypeIds.ExportedPythonTypes, new ExportedPythonTypes());
	}

	sealed class ExportedPythonTypes {
		readonly Dictionary<string, EnumType> toEnumType = new(StringComparer.Ordinal);
		public List<EnumType> Enums { get; } = new List<EnumType>();

		public void AddEnum(EnumType enumType) {
			Enums.Add(enumType);
			toEnumType.Add(enumType.RawName, enumType);
		}

		public bool TryFindByName(string name, [NotNullWhen(true)] out EnumType? enumType) =>
			toEnumType.TryGetValue(name, out enumType);
	}
}
