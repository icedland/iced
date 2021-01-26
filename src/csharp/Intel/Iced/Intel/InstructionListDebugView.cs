// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

using System;
using System.Diagnostics;

namespace Iced.Intel {
	sealed class InstructionListDebugView {
		readonly InstructionList list;

		public InstructionListDebugView(InstructionList list) =>
			this.list = list ?? throw new ArgumentNullException(nameof(list));

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public Instruction[] Items => list.ToArray();
	}
}
