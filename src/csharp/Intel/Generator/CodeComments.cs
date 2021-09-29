// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Text;
using Generator.Tables;

namespace Generator {
	static class CodeComments {
		public static void AddComments(GenTypes genTypes) {
			var sb = new StringBuilder();
			foreach (var def in genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs) {
				var cpuidDocs = string.Join(" and ", def.CpuidFeatureStrings);
				var docStr = $"#(c:{def.InstructionString})##(p:)##(c:{def.OpCodeString})##(p:)##(c:{cpuidDocs})##(p:)##(c:{GetMode(sb, def)})#";
				if (!def.Code.Documentation.HasDefaultComment)
					def.Code.Documentation = new(docStr);
			}
		}

		static string GetMode(StringBuilder sb, InstructionDef def) {
			sb.Clear();
			if ((def.Flags1 & InstructionDefFlags1.Bit16) != 0)
				sb.Append("16");
			if ((def.Flags1 & InstructionDefFlags1.Bit32) != 0) {
				if (sb.Length > 0)
					sb.Append('/');
				sb.Append("32");
			}
			if ((def.Flags1 & InstructionDefFlags1.Bit64) != 0) {
				if (sb.Length > 0)
					sb.Append('/');
				sb.Append("64");
			}
			if (sb.Length == 0)
				throw new InvalidOperationException();
			sb.Append("-bit");
			return sb.ToString();
		}
	}
}
