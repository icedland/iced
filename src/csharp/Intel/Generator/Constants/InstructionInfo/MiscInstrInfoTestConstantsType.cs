// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Constants.InstructionInfo {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class MiscInstrInfoTestConstantsType {
		MiscInstrInfoTestConstantsType(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.MiscInstrInfoTestConstants, ConstantsTypeFlags.None, default, GetConstants());
			genTypes.Add(type);
		}

		static Constant[] GetConstants() =>
			new Constant[] {
				new Constant(ConstantKind.String, "VMM_prefix", "vmm"),
				// XSP = SP/ESP/RSP depending on stack address size, XBP = BP/EBP/RBP depending on stack address size
				new Constant(ConstantKind.String, "XSP", "xsp"),
				new Constant(ConstantKind.String, "XBP", "xbp"),
				new Constant(ConstantKind.Index, "InstrInfoElemsPerLine", 5),
				new Constant(ConstantKind.Index, "MemorySizeElemsPerLine", 6),
				new Constant(ConstantKind.Index, "RegisterElemsPerLine", 7),
				new Constant(ConstantKind.String, "MemSizeOption_Addr16", "16"),
				new Constant(ConstantKind.String, "MemSizeOption_Addr32", "32"),
				new Constant(ConstantKind.String, "MemSizeOption_Addr64", "64"),
				new Constant(ConstantKind.String, "MemSizeOption_Vsib32", "vsib32"),
				new Constant(ConstantKind.String, "MemSizeOption_Vsib64", "vsib64"),
			};
	}
}
