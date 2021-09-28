// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Linq;
using Generator.Enums;
using Generator.Enums.Formatter;
using Generator.Tables;

namespace Generator.Formatters {
	abstract class TableGen {
		protected abstract void Generate(MemorySizeDef[] defs);
		protected abstract void GenerateRegisters(string[] registers);
		protected abstract void GenerateFormatterFlowControl((EnumValue flowCtrl, EnumValue[] code)[] infos);

		protected readonly GenTypes genTypes;

		protected TableGen(GenTypes genTypes) =>
			this.genTypes = genTypes;

		public void Generate() {
			Generate(genTypes.GetObject<MemorySizeDefs>(TypeIds.MemorySizeDefs).Defs);
			var list = genTypes.GetObject<RegisterDefs>(TypeIds.RegisterDefs).Defs.Select(a => a.Name).ToList();
			GenerateRegisters(list.ToArray());
			var flowCtrl = genTypes[TypeIds.FormatterFlowControl];
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var formatterFlowControlInfo = new (EnumValue flowCtrl, EnumValue[] code)[] {
				(flowCtrl[nameof(FormatterFlowControl.ShortBranch)], defs.Where(a => a.BranchKind == BranchKind.JccShort || a.BranchKind == BranchKind.JmpShort || a.BranchKind == BranchKind.JkccShort).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.AlwaysShortBranch)], defs.Where(a => a.BranchKind == BranchKind.Loop || a.BranchKind == BranchKind.Jrcxz).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.NearCall)], defs.Where(a => a.BranchKind == BranchKind.CallNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.NearBranch)], defs.Where(a => a.BranchKind == BranchKind.JccNear || a.BranchKind == BranchKind.JmpNear || a.BranchKind == BranchKind.JmpeNear || a.BranchKind == BranchKind.JkccNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.FarCall)], defs.Where(a => a.BranchKind == BranchKind.CallFar).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.FarBranch)], defs.Where(a => a.BranchKind == BranchKind.JmpFar).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.Xbegin)], defs.Where(a => a.BranchKind == BranchKind.Xbegin).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
			};
			GenerateFormatterFlowControl(formatterFlowControlInfo);
		}
	}
}
