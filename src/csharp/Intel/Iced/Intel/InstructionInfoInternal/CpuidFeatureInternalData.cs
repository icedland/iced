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

#if INSTR_INFO
using System;
using Iced.Intel.Internal;

namespace Iced.Intel.InstructionInfoInternal {
	static partial class CpuidFeatureInternalData {
		public static readonly CpuidFeature[][] ToCpuidFeatures = GetCpuidFeatures();
		static CpuidFeature[][] GetCpuidFeatures() {
			var data = GetGetCpuidFeaturesData();
			var reader = new DataReader(data);
			reader.Index = (IcedConstants.MaxCpuidFeatureInternalValues + 7) / 8;
			var cpuidFeatures = new CpuidFeature[IcedConstants.MaxCpuidFeatureInternalValues][];
			for (int i = 0; i < cpuidFeatures.Length; i++) {
				byte b = data[i / 8];
				var features = new CpuidFeature[((b >> (i % 8)) & 1) + 1];
				for (int j = 0; j < features.Length; j++)
					features[j] = (CpuidFeature)reader.ReadByte();
				cpuidFeatures[i] = features;
			}
			if (reader.CanRead)
				throw new InvalidOperationException();
			return cpuidFeatures;
		}
	}
}
#endif
