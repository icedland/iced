// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace IcedFuzzer.Core {
	static class RegisterUtils {
		public static uint GetRegisterCount(FuzzerRegisterKind register) =>
			register switch {
				FuzzerRegisterKind.GPR8 or FuzzerRegisterKind.GPR16 or FuzzerRegisterKind.GPR32 or FuzzerRegisterKind.GPR64 or
				FuzzerRegisterKind.CR or FuzzerRegisterKind.DR => 16,
				FuzzerRegisterKind.Segment or FuzzerRegisterKind.ST or FuzzerRegisterKind.TR or FuzzerRegisterKind.K or FuzzerRegisterKind.MM or
				FuzzerRegisterKind.TMM => 8,
				FuzzerRegisterKind.BND => 4,
				FuzzerRegisterKind.XMM or FuzzerRegisterKind.YMM or FuzzerRegisterKind.ZMM => 32,
				_ => throw ThrowHelpers.Unreachable,
			};

		public static uint GetRegisterCount(FuzzerRegisterClass registerClass) =>
			registerClass switch {
				FuzzerRegisterClass.GPR or FuzzerRegisterClass.CR or FuzzerRegisterClass.DR => 16,
				FuzzerRegisterClass.Segment or FuzzerRegisterClass.ST or FuzzerRegisterClass.TR or FuzzerRegisterClass.K or FuzzerRegisterClass.MM or
				FuzzerRegisterClass.TMM => 8,
				FuzzerRegisterClass.BND => 4,
				FuzzerRegisterClass.Vector => 32,
				_ => throw ThrowHelpers.Unreachable,
			};
	}
}
