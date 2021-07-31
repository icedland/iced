// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Linq;

namespace Generator {
	static class ConstantUtils {
		public static (uint mask, uint bits) GetMaskBits(uint max) {
			if (max == 0)
				return (1, 1);
			uint mask = 0;
			uint bits = 0;
			while (max != 0) {
				max >>= 1;
				mask = (mask << 1) | 1;
				bits++;
			}
			return (mask, bits);
		}

		public static (uint mask, uint bits) GetMaskBits<T>() where T : struct, Enum {
			var max = Enum.GetValues<T>().Select(a => Convert.ToUInt32(a)).Max();
			return GetMaskBits(max);
		}

		public static void VerifyMask(uint mask, uint max) {
			if (GetMaskBits(max).mask != mask)
				throw new InvalidOperationException();
		}

		public static void VerifyMask<T>(uint mask) where T : struct, Enum {
			if (GetMaskBits<T>().mask != mask)
				throw new InvalidOperationException();
		}
	}
}
