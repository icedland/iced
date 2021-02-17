// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;

namespace Iced.UnitTests.Intel {
	static class BitnessUtils {
		public static IEnumerable<int> GetInvalidBitnessValues() {
			yield return int.MinValue;
			yield return int.MaxValue;
			for (int bitness = -1; bitness <= 128; bitness++) {
				if (bitness == 16 || bitness == 32 || bitness == 64)
					continue;
				yield return bitness;
			}
		}
	}
}
