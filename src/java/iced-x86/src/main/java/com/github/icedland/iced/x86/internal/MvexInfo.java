// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal;

/** DO NOT USE: INTERNAL API */
public final class MvexInfo {
	private static final byte[] data = ResourceReader.readByteArray(MvexInfo.class.getClassLoader(),
			"com/github/icedland/iced/x86/MvexInfoData.bin");

	/** DO NOT USE: INTERNAL API */
	public static boolean isMvex(int code) {
		return Integer.compareUnsigned(code - IcedConstants.MVEX_START, IcedConstants.MVEX_LENGTH) < 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static int getTupleTypeLutKind(int code) {
		int index = code - IcedConstants.MVEX_START;
		return data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.TUPLE_TYPE_LUT_KIND_INDEX];
	}

	/** DO NOT USE: INTERNAL API */
	public static int getEHBit(int code) {
		int index = code - IcedConstants.MVEX_START;
		return data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.EH_BIT_INDEX];
	}

	/** DO NOT USE: INTERNAL API */
	public static int getConvFn(int code) {
		int index = code - IcedConstants.MVEX_START;
		return data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.CONV_FN_INDEX];
	}

	/** DO NOT USE: INTERNAL API */
	public static int getInvalidConvFns(int code) {
		int index = code - IcedConstants.MVEX_START;
		return data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.INVALID_CONV_FNS_INDEX];
	}

	/** DO NOT USE: INTERNAL API */
	public static int getInvalidSwizzleFns(int code) {
		int index = code - IcedConstants.MVEX_START;
		return data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.INVALID_SWIZZLE_FNS_INDEX];
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean isNDD(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.NDD) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean isNDS(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.NDS) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean canUseEvictionHint(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.EVICTION_HINT) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean canUseImmRoundingControl(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.IMM_ROUNDING_CONTROL) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean canUseRoundingControl(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.ROUNDING_CONTROL) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean canUseSuppressAllExceptions(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.SUPPRESS_ALL_EXCEPTIONS) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean getIgnoresOpMaskRegister(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.IGNORES_OP_MASK_REGISTER) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean getRequireOpMaskRegister(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS1_INDEX] & MvexInfoFlags1.REQUIRE_OP_MASK_REGISTER) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean getNoSaeRc(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS2_INDEX] & MvexInfoFlags2.NO_SAE_ROUNDING_CONTROL) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean isConvFn32(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS2_INDEX] & MvexInfoFlags2.CONV_FN32) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static boolean getIgnoresEvictionHint(int code) {
		int index = code - IcedConstants.MVEX_START;
		return (data[index * MvexInfoData.STRUCT_SIZE + MvexInfoData.FLAGS2_INDEX] & MvexInfoFlags2.IGNORES_EVICTION_HINT) != 0;
	}

	/** DO NOT USE: INTERNAL API */
	public static int getTupleType(int code, int sss) {
		return MvexTupleTypeLut.data[getTupleTypeLutKind(code) * 8 + sss];
	}
}
