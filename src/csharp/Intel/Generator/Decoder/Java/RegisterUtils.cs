// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;

namespace Generator.Tables.Java {
	static class RegisterUtils {
		public static (string name, RegisterKind[] kinds)[] GetRegisterInfos(string allRegsClassName, (RegisterKind kind, RegisterDef[] regs)[] regGroups) {
			var infos = new (string name, RegisterKind[] kinds)[] {
				(allRegsClassName, Array.Empty<RegisterKind>()),
				(allRegsClassName + "GPR", new[] { RegisterKind.GPR8, RegisterKind.GPR16, RegisterKind.GPR32, RegisterKind.GPR64 }),
				(allRegsClassName + "Vec", new[] { RegisterKind.XMM, RegisterKind.YMM, RegisterKind.ZMM }),

				(allRegsClassName + "GPR8", new[] { RegisterKind.GPR8 }),
				(allRegsClassName + "GPR16", new[] { RegisterKind.GPR16 }),
				(allRegsClassName + "GPR32", new[] { RegisterKind.GPR32 }),
				(allRegsClassName + "GPR64", new[] { RegisterKind.GPR64 }),
				(allRegsClassName + "IP", new[] { RegisterKind.IP }),
				(allRegsClassName + "Segment", new[] { RegisterKind.Segment }),
				(allRegsClassName + "ST", new[] { RegisterKind.ST }),
				(allRegsClassName + "CR", new[] { RegisterKind.CR }),
				(allRegsClassName + "DR", new[] { RegisterKind.DR }),
				(allRegsClassName + "TR", new[] { RegisterKind.TR }),
				(allRegsClassName + "BND", new[] { RegisterKind.BND }),
				(allRegsClassName + "K", new[] { RegisterKind.K }),
				(allRegsClassName + "MM", new[] { RegisterKind.MM }),
				(allRegsClassName + "XMM", new[] { RegisterKind.XMM }),
				(allRegsClassName + "YMM", new[] { RegisterKind.YMM }),
				(allRegsClassName + "ZMM", new[] { RegisterKind.ZMM }),
				(allRegsClassName + "TMM", new[] { RegisterKind.TMM }),
			};

			var allGroups = regGroups.ToDictionary(a => a.kind, a => true);
			allGroups.Remove(RegisterKind.None);
			foreach (var (_, kinds) in infos) {
				if (kinds.Length == 1)
					allGroups.Remove(kinds[0]);
			}
			if (allGroups.Count != 0)
				throw new InvalidOperationException($"New register kind: {allGroups.ToArray()[0].Key}");

			return infos;
		}
    }
}

