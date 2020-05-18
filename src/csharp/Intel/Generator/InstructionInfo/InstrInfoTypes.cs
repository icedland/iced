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
			genTypes.Add(gen.EnumCodeInfo ?? throw new InvalidOperationException());
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
