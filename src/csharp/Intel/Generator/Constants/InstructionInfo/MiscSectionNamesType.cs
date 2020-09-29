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

namespace Generator.Constants.InstructionInfo {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class MiscSectionNamesType {
		MiscSectionNamesType(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.MiscSectionNames, ConstantsTypeFlags.None, null, GetConstants());
			genTypes.Add(type);
		}

		static Constant[] GetConstants() =>
			new Constant[] {
				new Constant(ConstantKind.String, "JccShort", "jcc-short"),
				new Constant(ConstantKind.String, "JccNear", "jcc-near"),
				new Constant(ConstantKind.String, "JmpShort", "jmp-short"),
				new Constant(ConstantKind.String, "JmpNear", "jmp-near"),
				new Constant(ConstantKind.String, "JmpFar", "jmp-far"),
				new Constant(ConstantKind.String, "JmpNearIndirect", "jmp-near-indirect"),
				new Constant(ConstantKind.String, "JmpFarIndirect", "jmp-far-indirect"),
				new Constant(ConstantKind.String, "CallNear", "call-near"),
				new Constant(ConstantKind.String, "CallFar", "call-far"),
				new Constant(ConstantKind.String, "CallNearIndirect", "call-near-indirect"),
				new Constant(ConstantKind.String, "CallFarIndirect", "call-far-indirect"),
				new Constant(ConstantKind.String, "Loop", "loop"),
				new Constant(ConstantKind.String, "Jrcxz", "jrcxz"),
				new Constant(ConstantKind.String, "Xbegin", "xbegin"),
				new Constant(ConstantKind.String, "JmpInfo", "jmp-info"),
				new Constant(ConstantKind.String, "JccShortInfo", "jcc-short-info"),
				new Constant(ConstantKind.String, "JccNearInfo", "jcc-near-info"),
				new Constant(ConstantKind.String, "SetccInfo", "setcc-info"),
				new Constant(ConstantKind.String, "CmovccInfo", "cmovcc-info"),
				new Constant(ConstantKind.String, "LoopccInfo", "loopcc-info"),
			};
	}
}
