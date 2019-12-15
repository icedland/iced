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
using Generator.Constants;
using Generator.Enums;
using Generator.Enums.InstructionInfo;

namespace Generator.InstructionInfo {
	static class InstrInfoTypes {
		public static EnumType EnumCodeInfo { get; }
		public static EnumType EnumRflagsInfo { get; }
		public static EnumType[] EnumOpInfos { get; }
		public static EnumType EnumInfoFlags1 { get; }
		public static EnumType EnumInfoFlags2 { get; }
		public static EnumType EnumCpuidFeatureInternal { get; }
		public static ConstantsType InstrInfoConstants { get; }
		public static (EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] CpuidFeatures { get; }
		public static (EnumValue value, (RflagsBits read, RflagsBits undefined, RflagsBits written, RflagsBits cleared, RflagsBits set) rflags)[] RflagsInfos { get; }
		public static InstrInfo[] InstrInfos { get; }

		static InstrInfoTypes() {
			var gen = new InstrInfoTypesGen();
			gen.Generate();
			EnumCodeInfo = gen.EnumCodeInfo ?? throw new InvalidOperationException();
			EnumRflagsInfo = gen.EnumRflagsInfo ?? throw new InvalidOperationException();
			EnumOpInfos = gen.EnumOpInfos ?? throw new InvalidOperationException();
			EnumInfoFlags1 = gen.EnumInfoFlags1 ?? throw new InvalidOperationException();
			EnumInfoFlags2 = gen.EnumInfoFlags2 ?? throw new InvalidOperationException();
			EnumCpuidFeatureInternal = gen.EnumCpuidFeatureInternal ?? throw new InvalidOperationException();
			InstrInfoConstants = gen.InstrInfoConstants ?? throw new InvalidOperationException();
			CpuidFeatures = gen.CpuidFeatures ?? throw new InvalidOperationException();
			RflagsInfos = gen.RflagsInfos ?? throw new InvalidOperationException();
			InstrInfos = gen.InstrInfos;
		}
	}
}
