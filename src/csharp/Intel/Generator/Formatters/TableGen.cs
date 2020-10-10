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
			Generate(genTypes.GetObject<MemorySizeInfoTable>(TypeIds.MemorySizeInfoTable).Data);
			var list = genTypes.GetObject<RegisterInfoTable>(TypeIds.RegisterInfoTable).Data.Select(a => a.Name).ToList();
			// gas, intel, masm use 'st', nasm doesn't.
			list.Add("st");
			GenerateRegisters(list.ToArray());
			var flowCtrl = genTypes[TypeIds.FormatterFlowControl];
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			var formatterFlowControlInfo = new (EnumValue flowCtrl, EnumValue[] code)[] {
				(flowCtrl[nameof(FormatterFlowControl.ShortBranch)], defs.Where(a => a.BranchKind == BranchKind.JccShort || a.BranchKind == BranchKind.JmpShort).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.AlwaysShortBranch)], defs.Where(a => a.BranchKind == BranchKind.Loop || a.BranchKind == BranchKind.Jrcxz).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.NearCall)], defs.Where(a => a.BranchKind == BranchKind.CallNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.NearBranch)], defs.Where(a => a.BranchKind == BranchKind.JccNear || a.BranchKind == BranchKind.JmpNear || a.BranchKind == BranchKind.JmpeNear).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.FarCall)], defs.Where(a => a.BranchKind == BranchKind.CallFar).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.FarBranch)], defs.Where(a => a.BranchKind == BranchKind.JmpFar).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
				(flowCtrl[nameof(FormatterFlowControl.Xbegin)], defs.Where(a => a.BranchKind == BranchKind.Xbegin).Select(a => a.Code).OrderBy(a => a.Value).ToArray()),
			};
			GenerateFormatterFlowControl(formatterFlowControlInfo);
		}
	}
}
