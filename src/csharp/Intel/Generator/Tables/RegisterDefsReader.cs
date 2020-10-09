/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
		readonly HashSet<EnumValue> createdRegs;

		public RegisterDefsReader(GenTypes genTypes, string filename) {
			this.filename = filename;
			createdRegs = genTypes[TypeIds.Register].Values.ToHashSet();

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
				if (parts.Length != 7)
					throw new InvalidOperationException();

				var register = toRegister[parts[0]];
				var baseRegister = toRegister[parts[1]];
				var fullRegister32 = toRegister[parts[2]];
				var fullRegister = toRegister[parts[3]];
				uint size = uint.Parse(parts[4]);
				var regKind = toRegisterKind[parts[5]];
				var regClass = toRegisterClass[parts[6]];

				if (!createdRegs.Remove(register))
					throw new InvalidOperationException($"Duplicate register def on line {i + 1}");

				var def = new RegisterDef(register, baseRegister, fullRegister32, fullRegister, regClass, regKind, size);
				defs.Add(def);
			}

			if (createdRegs.Count != 0)
				throw new InvalidOperationException("Missing register(s) in defs file: " + string.Join(", ", createdRegs.Select(a => a.RawName).ToArray()));
			return defs.OrderBy(a => a.Register.Value).ToArray();
		}
	}
}
