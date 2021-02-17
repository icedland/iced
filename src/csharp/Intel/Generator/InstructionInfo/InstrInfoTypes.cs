// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;
using Generator.Enums.InstructionInfo;

namespace Generator.InstructionInfo {
	[TypeGen(TypeGenOrders.CreatedInstructions)]
	sealed class InstrInfoTypes {
		public EnumType[] EnumOpInfos { get; }
		public (EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] CpuidFeatures { get; }
		public (EnumValue value, (RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) rflags)[] RflagsInfos { get; }

		InstrInfoTypes(GenTypes genTypes) {
			var gen = new InstrInfoTypesGen(genTypes);
			gen.Generate();
			genTypes.Add(gen.EnumRflagsInfo ?? throw new InvalidOperationException());
			EnumOpInfos = gen.EnumOpInfos ?? throw new InvalidOperationException();
			foreach (var enumType in EnumOpInfos)
				genTypes.Add(enumType);
			genTypes.Add(gen.EnumInfoFlags1 ?? throw new InvalidOperationException());
			genTypes.Add(gen.EnumInfoFlags2 ?? throw new InvalidOperationException());
			genTypes.Add(gen.EnumCpuidFeatureInternal ?? throw new InvalidOperationException());
			genTypes.Add(gen.InstrInfoConstants ?? throw new InvalidOperationException());
			CpuidFeatures = gen.CpuidFeatures ?? throw new InvalidOperationException();
			RflagsInfos = gen.RflagsInfos ?? throw new InvalidOperationException();
			genTypes.AddObject(TypeIds.InstrInfoTypes, this);
		}
	}
}
