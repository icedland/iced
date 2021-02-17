// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator.Enums;

namespace Generator.Tables {
	sealed class MemorySizeDefsReader {
		readonly string filename;
		readonly Dictionary<string, EnumValue> toMemorySize;
		readonly Dictionary<string, EnumValue> toBroadcastToKind;
		readonly Dictionary<string, EnumValue> toFastMemoryKeywords;
		readonly Dictionary<string, EnumValue> toIntelMemoryKeywords;
		readonly Dictionary<string, EnumValue> toMasmMemoryKeywords;
		readonly Dictionary<string, EnumValue> toNasmMemoryKeywords;
		readonly HashSet<EnumValue> createdDefs;

		public MemorySizeDefsReader(GenTypes genTypes, string filename) {
			this.filename = filename;
			createdDefs = genTypes[TypeIds.MemorySize].Values.ToHashSet();

			toMemorySize = CreateEnumDict(genTypes[TypeIds.MemorySize]);
			toBroadcastToKind = CreateEnumDict(genTypes[TypeIds.BroadcastToKind]);
			toFastMemoryKeywords = CreateEnumDict(genTypes[TypeIds.FastMemoryKeywords]);
			toIntelMemoryKeywords = CreateEnumDict(genTypes[TypeIds.IntelMemoryKeywords]);
			toMasmMemoryKeywords = CreateEnumDict(genTypes[TypeIds.MasmMemoryKeywords]);
			toNasmMemoryKeywords = CreateEnumDict(genTypes[TypeIds.NasmMemoryKeywords]);
		}

		static Dictionary<string, EnumValue> CreateEnumDict(EnumType enumType, bool ignoreCase = false) =>
			enumType.Values.ToDictionary(a => a.RawName, a => a, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

		public MemorySizeDef[] Read() {
			var defs = new List<MemorySizeDef>();

			var lines = File.ReadAllLines(filename);
			for (int i = 0; i < lines.Length; i++) {
				var line = lines[i];
				if (line.Length == 0 || line[0] == '#')
					continue;

				var parts = line.Split(',').Select(a => a.Trim()).ToArray();
				if (parts.Length != 9)
					throw new InvalidOperationException();

				var size = uint.Parse(parts[0]);
				var memSize = toMemorySize[parts[1]];
				var elemMemSize = toMemorySize[parts[2]];
				var bcst = toBroadcastToKind[parts[3]];
				var fast = toFastMemoryKeywords[parts[4]];
				var intel = toIntelMemoryKeywords[parts[5]];
				var masm = toMasmMemoryKeywords[parts[6]];
				var nasm = toNasmMemoryKeywords[parts[7]];
				var flags = MemorySizeDefFlags.None;
				foreach (var value in parts[8].Split(' ', StringSplitOptions.RemoveEmptyEntries)) {
					flags |= value switch {
						"signed" => MemorySizeDefFlags.Signed,
						"bcst" => MemorySizeDefFlags.Broadcast,
						_ => throw new InvalidOperationException(),
					};
				}

				if (!createdDefs.Remove(memSize))
					throw new InvalidOperationException($"Duplicate MemorySize def on line {i + 1}");

				var def = new MemorySizeDef(memSize, size, elemMemSize, 0, flags, bcst, fast, intel, masm, nasm);
				defs.Add(def);
			}

			if (createdDefs.Count != 0)
				throw new InvalidOperationException("Missing MemorySize values in defs file: " + string.Join(", ", createdDefs.Select(a => a.RawName).ToArray()));

			var toDef = defs.ToDictionary(a => a.MemorySize, a => a);
			for (int i = 0; i < defs.Count; i++) {
				var def = defs[i];
				var elemDef = toDef[def.ElementType];
				var newDef = new MemorySizeDef(def.MemorySize, def.Size, def.ElementType, elemDef.Size, def.Flags, def.BroadcastToKind, def.Fast, def.Intel, def.Masm, def.Nasm);
				defs[i] = newDef;
			}

			return defs.OrderBy(a => a.MemorySize.Value).ToArray();
		}
	}
}
