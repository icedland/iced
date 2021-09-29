// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Constants {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class DecoderConstantsType {
		DecoderConstantsType(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.DecoderConstants, ConstantsTypeFlags.None, default, GetConstants());
			genTypes.Add(type);
		}

		static Constant[] GetConstants() =>
			new Constant[] {
				new Constant(ConstantKind.UInt64, "DEFAULT_IP16", 0x7FF0, ConstantsTypeFlags.Hex),
				new Constant(ConstantKind.UInt64, "DEFAULT_IP32", 0x7FFF_FFF0, ConstantsTypeFlags.Hex),
				new Constant(ConstantKind.UInt64, "DEFAULT_IP64", 0x7FFF_FFFF_FFFF_FFF0, ConstantsTypeFlags.Hex),
			};
	}
}
