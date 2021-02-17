// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator.Enums;

namespace Generator.Tables {
	sealed class RegisterDefsReader {
		readonly string filename;
		readonly Dictionary<string, EnumValue> toRegister;
		readonly Dictionary<string, EnumValue> toRegisterKind;
		readonly Dictionary<string, EnumValue> toRegisterClass;
		readonly HashSet<EnumValue> createdDefs;

		public RegisterDefsReader(GenTypes genTypes, string filename) {
			this.filename = filename;
			createdDefs = genTypes[TypeIds.Register].Values.ToHashSet();

			toRegister = CreateEnumDict(genTypes[TypeIds.Register]);
			toRegisterKind = CreateEnumDict(genTypes[TypeIds.RegisterKind]);
			toRegisterClass = CreateEnumDict(genTypes[TypeIds.RegisterClass]);
		}

		static Dictionary<string, EnumValue> CreateEnumDict(EnumType enumType, bool ignoreCase = false) =>
			enumType.Values.ToDictionary(a => a.RawName, a => a, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

		public RegisterDef[] Read() {
			var defs = new List<RegisterDef>();

			var lines = File.ReadAllLines(filename);
			for (int i = 0; i < lines.Length; i++) {
				var line = lines[i];
				if (line.Length == 0 || line[0] == '#')
					continue;

				var parts = line.Split(',').Select(a => a.Trim()).ToArray();
				if (parts.Length != 8)
					throw new InvalidOperationException();

				var register = toRegister[parts[0]];
				var baseRegister = toRegister[parts[1]];
				var fullRegister32 = toRegister[parts[2]];
				var fullRegister = toRegister[parts[3]];
				uint size = uint.Parse(parts[4]);
				var regKind = toRegisterKind[parts[5]];
				var regClass = toRegisterClass[parts[6]];
				var name = parts[7].ToLowerInvariant();

				if (!createdDefs.Remove(register))
					throw new InvalidOperationException($"Duplicate register def on line {i + 1}");

				var def = new RegisterDef(register, baseRegister, fullRegister32, fullRegister, regClass, regKind, size, name);
				defs.Add(def);
			}

			if (createdDefs.Count != 0)
				throw new InvalidOperationException("Missing register(s) in defs file: " + string.Join(", ", createdDefs.Select(a => a.RawName).ToArray()));
			return defs.OrderBy(a => a.Register.Value).ToArray();
		}
	}
}
