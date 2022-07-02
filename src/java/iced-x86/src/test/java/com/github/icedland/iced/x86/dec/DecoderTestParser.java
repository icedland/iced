// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.ArrayList;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.MvexRegMemConv;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.StringUtils2;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.ToDecoderError;
import com.github.icedland.iced.x86.ToDecoderOptions;
import com.github.icedland.iced.x86.ToMemorySize;
import com.github.icedland.iced.x86.ToMnemonic;
import com.github.icedland.iced.x86.ToRegister;

final class DecoderTestParser {
	public static DecoderTestCase[] readFile(int bitness, String filename) {
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		ArrayList<DecoderTestCase> result = new ArrayList<DecoderTestCase>(lines.size());
		int lineNumber = 0;
		for (String line : lines) {
			lineNumber++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;
			DecoderTestCase testCase;
			try {
				testCase = readTestCase(bitness, line, lineNumber);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(
						String.format("Error parsing decoder test case file '%s', line %d: %s", filename, lineNumber, ex.getMessage()));
			}
			if (testCase != null)
				result.add(testCase);
		}
		return result.toArray(new DecoderTestCase[result.size()]);
	}

	private static DecoderTestCase readTestCase(int bitness, String line, int lineNumber) {
		String[] parts = StringUtils2.split(line, ",");
		if (parts.length != 5)
			throw new UnsupportedOperationException(String.format("Invalid number of commas (%d commas)", parts.length - 1));

		DecoderTestCase tc = new DecoderTestCase();
		tc.lineNumber = lineNumber;
		tc.testOptions = DecoderTestOptions.NONE;
		tc.bitness = bitness;
		switch (bitness) {
		case 16:
			tc.ip = DecoderConstants.DEFAULT_IP16;
			break;
		case 32:
			tc.ip = DecoderConstants.DEFAULT_IP32;
			break;
		case 64:
			tc.ip = DecoderConstants.DEFAULT_IP64;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		tc.hexBytes = toHexBytes(parts[0].trim());
		tc.encodedHexBytes = tc.hexBytes;
		String code = parts[1].trim();
		if (CodeUtils.isIgnored(code))
			return null;
		tc.code = ToCode.get(code);
		tc.mnemonic = ToMnemonic.get(parts[2].trim());
		tc.opCount = NumberConverter.toInt32(parts[3].trim());
		tc.decoderError = tc.code == Code.INVALID ? DecoderError.INVALID_INSTRUCTION : DecoderError.NONE;

		boolean foundCode = false;
		for (String key : parts[4].split(" ")) {
			if (key.equals(""))
				continue;
			String value;
			int index = key.indexOf('=');
			if (index >= 0) {
				value = key.substring(index + 1);
				key = key.substring(0, index);
			}
			else
				value = null;
			Integer ivalue;
			switch (key) {
			case DecoderTestParserConstants.DECODER_ERROR:
				if (value == null)
					throw new UnsupportedOperationException("Missing decoder error value");
				ivalue = ToDecoderError.tryGet(value);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid decoder error value: %s", value));
				tc.decoderError = ivalue.intValue();
				break;

			case DecoderTestParserConstants.BROADCAST:
				tc.isBroadcast = true;
				break;

			case DecoderTestParserConstants.XACQUIRE:
				tc.hasXacquirePrefix = true;
				break;

			case DecoderTestParserConstants.XRELEASE:
				tc.hasXreleasePrefix = true;
				break;

			case DecoderTestParserConstants.REP:
			case DecoderTestParserConstants.REPE:
				tc.hasRepePrefix = true;
				break;

			case DecoderTestParserConstants.REPNE:
				tc.hasRepnePrefix = true;
				break;

			case DecoderTestParserConstants.LOCK:
				tc.hasLockPrefix = true;
				break;

			case DecoderTestParserConstants.ZEROING_MASKING:
				tc.zeroingMasking = true;
				break;

			case DecoderTestParserConstants.SUPPRESS_ALL_EXCEPTIONS:
				tc.suppressAllExceptions = true;
				break;

			case DecoderTestParserConstants.VSIB32:
				tc.vsibBitness = 32;
				break;

			case DecoderTestParserConstants.VSIB64:
				tc.vsibBitness = 64;
				break;

			case DecoderTestParserConstants.ROUND_TO_NEAREST:
				tc.roundingControl = RoundingControl.ROUND_TO_NEAREST;
				break;

			case DecoderTestParserConstants.ROUND_DOWN:
				tc.roundingControl = RoundingControl.ROUND_DOWN;
				break;

			case DecoderTestParserConstants.ROUND_UP:
				tc.roundingControl = RoundingControl.ROUND_UP;
				break;

			case DecoderTestParserConstants.ROUND_TOWARD_ZERO:
				tc.roundingControl = RoundingControl.ROUND_TOWARD_ZERO;
				break;

			case DecoderTestParserConstants.OP0_KIND:
				if (tc.opCount < 1)
					throw new UnsupportedOperationException(String.format("Invalid OpCount: %d < 1", tc.opCount));
				readOpKind(tc, 0, value);
				break;

			case DecoderTestParserConstants.OP1_KIND:
				if (tc.opCount < 2)
					throw new UnsupportedOperationException(String.format("Invalid OpCount: %d < 2", tc.opCount));
				readOpKind(tc, 1, value);
				break;

			case DecoderTestParserConstants.OP2_KIND:
				if (tc.opCount < 3)
					throw new UnsupportedOperationException(String.format("Invalid OpCount: %d < 3", tc.opCount));
				readOpKind(tc, 2, value);
				break;

			case DecoderTestParserConstants.OP3_KIND:
				if (tc.opCount < 4)
					throw new UnsupportedOperationException(String.format("Invalid OpCount: %d < 4", tc.opCount));
				readOpKind(tc, 3, value);
				break;

			case DecoderTestParserConstants.OP4_KIND:
				if (tc.opCount < 5)
					throw new UnsupportedOperationException(String.format("Invalid OpCount: %d < 5", tc.opCount));
				readOpKind(tc, 4, value);
				break;

			case DecoderTestParserConstants.ENCODED_HEX_BYTES:
				if (StringUtils2.isNullOrWhiteSpace(value))
					throw new UnsupportedOperationException(String.format("Invalid encoded hex bytes: '%s'", value));
				tc.encodedHexBytes = toHexBytes(value);
				break;

			case DecoderTestParserConstants.CODE:
				if (StringUtils2.isNullOrWhiteSpace(value))
					throw new UnsupportedOperationException(String.format("Invalid Code value: '%s'", value));
				if (CodeUtils.isIgnored(value))
					return null;
				foundCode = true;
				break;

			case DecoderTestParserConstants.DECODER_OPTIONS:
				if (StringUtils2.isNullOrWhiteSpace(value))
					throw new UnsupportedOperationException(String.format("Invalid DecoderOption value: '%s'", value));
				ivalue = tryParseDecoderOptions(value.split(";"));
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid DecoderOptions value, '%s'", value));
				tc.decoderOptions |= ivalue;
				break;

			case DecoderTestParserConstants.IP:
				if (StringUtils2.isNullOrWhiteSpace(value))
					throw new UnsupportedOperationException(String.format("Invalid IP value: '%s'", value));
				tc.ip = NumberConverter.toUInt64(value);
				break;

			case DecoderTestParserConstants.EVICTION_HINT:
				tc.mvexEvictionHint = true;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_NONE:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_NONE;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_CDAB:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_CDAB;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_BADC:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_BADC;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_DACB:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_DACB;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_AAAA:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_AAAA;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_BBBB:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_BBBB;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_CCCC:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_CCCC;
				break;

			case DecoderTestParserConstants.MVEX_REG_SWIZZLE_DDDD:
				tc.mvexRegMemConv = MvexRegMemConv.REG_SWIZZLE_DDDD;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_NONE:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_NONE;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_BROADCAST1:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_BROADCAST1;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_BROADCAST4:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_BROADCAST4;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_FLOAT16:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_FLOAT16;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_UINT8:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_UINT8;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_SINT8:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_SINT8;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_UINT16:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_UINT16;
				break;

			case DecoderTestParserConstants.MVEX_MEM_CONV_SINT16:
				tc.mvexRegMemConv = MvexRegMemConv.MEM_CONV_SINT16;
				break;

			case DecoderTestParserConstants.SEGMENT_PREFIX_ES:
				tc.segmentPrefix = Register.ES;
				break;

			case DecoderTestParserConstants.SEGMENT_PREFIX_CS:
				tc.segmentPrefix = Register.CS;
				break;

			case DecoderTestParserConstants.SEGMENT_PREFIX_SS:
				tc.segmentPrefix = Register.SS;
				break;

			case DecoderTestParserConstants.SEGMENT_PREFIX_DS:
				tc.segmentPrefix = Register.DS;
				break;

			case DecoderTestParserConstants.SEGMENT_PREFIX_FS:
				tc.segmentPrefix = Register.FS;
				break;

			case DecoderTestParserConstants.SEGMENT_PREFIX_GS:
				tc.segmentPrefix = Register.GS;
				break;

			case DecoderTestParserConstants.OP_MASK_K1:
				tc.opMask = Register.K1;
				break;

			case DecoderTestParserConstants.OP_MASK_K2:
				tc.opMask = Register.K2;
				break;

			case DecoderTestParserConstants.OP_MASK_K3:
				tc.opMask = Register.K3;
				break;

			case DecoderTestParserConstants.OP_MASK_K4:
				tc.opMask = Register.K4;
				break;

			case DecoderTestParserConstants.OP_MASK_K5:
				tc.opMask = Register.K5;
				break;

			case DecoderTestParserConstants.OP_MASK_K6:
				tc.opMask = Register.K6;
				break;

			case DecoderTestParserConstants.OP_MASK_K7:
				tc.opMask = Register.K7;
				break;

			case DecoderTestParserConstants.CONSTANT_OFFSETS:
				ConstantOffsets co = tryParseConstantOffsets(value);
				if (co == null)
					throw new UnsupportedOperationException(String.format("Invalid ConstantOffsets: '%s'", value));
				tc.constantOffsets = co;
				break;

			case DecoderTestParserConstants.DECODER_TEST_OPTIONS_NO_ENCODE:
				tc.testOptions |= DecoderTestOptions.NO_ENCODE;
				break;

			case DecoderTestParserConstants.DECODER_TEST_OPTIONS_NO_OPT_DISABLE_TEST:
				tc.testOptions |= DecoderTestOptions.NO_OPT_DISABLE_TEST;
				break;

			default:
				throw new UnsupportedOperationException(String.format("Invalid key '%s'", key));
			}
		}

		if (tc.code == Code.INVALID && !foundCode)
			throw new UnsupportedOperationException(String.format(
					"Test case decodes to Code.INVALID but there's no %s=xxx showing the original Code value so it can be filtered out if needed",
					DecoderTestParserConstants.CODE));

		return tc;
	}

	private static Integer tryParseDecoderOptions(String[] stringOptions) {
		int options = DecoderOptions.NONE;
		for (String opt : stringOptions) {
			Integer decOpts = ToDecoderOptions.tryGet(opt.trim());
			if (decOpts == null)
				return null;
			options |= decOpts.intValue();
		}
		return options;
	}

	static ConstantOffsets tryParseConstantOffsets(String value) {
		ConstantOffsets constantOffsets = new ConstantOffsets();
		if (value == null)
			return null;

		String[] parts = value.split(";");
		if (parts.length != 6)
			return null;
		constantOffsets.immediateOffset = NumberConverter.toUInt8(parts[0]);
		constantOffsets.immediateSize = NumberConverter.toUInt8(parts[1]);
		constantOffsets.immediateOffset2 = NumberConverter.toUInt8(parts[2]);
		constantOffsets.immediateSize2 = NumberConverter.toUInt8(parts[3]);
		constantOffsets.displacementOffset = NumberConverter.toUInt8(parts[4]);
		constantOffsets.displacementSize = NumberConverter.toUInt8(parts[5]);
		return constantOffsets;
	}

	private static void readOpKind(DecoderTestCase tc, int operand, String value) {
		String[] parts = value.split(";");
		switch (parts[0]) {
		case DecoderTestParserConstants.OP_KIND_REGISTER:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpRegister(operand, toRegister(parts[1]));
			tc.setOpKind(operand, OpKind.REGISTER);
			break;

		case DecoderTestParserConstants.OP_KIND_NEAR_BRANCH16:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.NEAR_BRANCH16);
			tc.nearBranch = NumberConverter.toUInt16(parts[1]) & 0xFFFF;
			break;

		case DecoderTestParserConstants.OP_KIND_NEAR_BRANCH32:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.NEAR_BRANCH32);
			tc.nearBranch = NumberConverter.toUInt32(parts[1]) & 0xFFFF_FFFFL;
			break;

		case DecoderTestParserConstants.OP_KIND_NEAR_BRANCH64:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.NEAR_BRANCH64);
			tc.nearBranch = NumberConverter.toUInt64(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_FAR_BRANCH16:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.FAR_BRANCH16);
			tc.farBranchSelector = NumberConverter.toUInt16(parts[1]);
			tc.farBranch = NumberConverter.toUInt16(parts[2]) & 0xFFFF;
			break;

		case DecoderTestParserConstants.OP_KIND_FAR_BRANCH32:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.FAR_BRANCH32);
			tc.farBranchSelector = NumberConverter.toUInt16(parts[1]);
			tc.farBranch = NumberConverter.toUInt32(parts[2]);
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE8:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE8);
			tc.immediate = NumberConverter.toUInt8(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE16:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE16);
			tc.immediate = NumberConverter.toUInt16(parts[1]) & 0xFFFF;
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE32:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE32);
			tc.immediate = NumberConverter.toUInt32(parts[1]) & 0xFFFF_FFFFL;
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE64:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE64);
			tc.immediate = NumberConverter.toUInt64(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE8TO16:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE8TO16);
			tc.immediate = NumberConverter.toUInt16(parts[1]) & 0xFFFF;
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE8TO32:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE8TO32);
			tc.immediate = NumberConverter.toUInt32(parts[1]) & 0xFFFF_FFFFL;
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE8TO64:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE8TO64);
			tc.immediate = NumberConverter.toUInt64(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE32TO64:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE32TO64);
			tc.immediate = NumberConverter.toUInt64(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_IMMEDIATE8_2ND:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.IMMEDIATE8_2ND);
			tc.immediate_2nd = NumberConverter.toUInt8(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_SEG_SI:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_SEG_SI);
			tc.memorySegment = toRegister(parts[1]);
			tc.memorySize = ToMemorySize.get(parts[2]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_SEG_ESI:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_SEG_ESI);
			tc.memorySegment = toRegister(parts[1]);
			tc.memorySize = ToMemorySize.get(parts[2]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_SEG_RSI:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_SEG_RSI);
			tc.memorySegment = toRegister(parts[1]);
			tc.memorySize = ToMemorySize.get(parts[2]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_SEG_DI:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_SEG_DI);
			tc.memorySegment = toRegister(parts[1]);
			tc.memorySize = ToMemorySize.get(parts[2]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_SEG_EDI:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_SEG_EDI);
			tc.memorySegment = toRegister(parts[1]);
			tc.memorySize = ToMemorySize.get(parts[2]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_SEG_RDI:
			if (parts.length != 3)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 3 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_SEG_RDI);
			tc.memorySegment = toRegister(parts[1]);
			tc.memorySize = ToMemorySize.get(parts[2]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_ES_DI:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_ESDI);
			tc.memorySize = ToMemorySize.get(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_ES_EDI:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_ESEDI);
			tc.memorySize = ToMemorySize.get(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY_ES_RDI:
			if (parts.length != 2)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 2 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY_ESRDI);
			tc.memorySize = ToMemorySize.get(parts[1]);
			break;

		case DecoderTestParserConstants.OP_KIND_MEMORY:
			if (parts.length != 8)
				throw new UnsupportedOperationException(String.format("Operand %d: expected 8 values, actual = %d", operand, parts.length));
			tc.setOpKind(operand, OpKind.MEMORY);
			tc.memorySegment = toRegister(parts[1]);
			tc.memoryBase = toRegister(parts[2]);
			tc.memoryIndex = toRegister(parts[3]);
			tc.memoryIndexScale = NumberConverter.toInt32(parts[4]);
			tc.memoryDisplacement = NumberConverter.toUInt64(parts[5]);
			tc.memoryDisplSize = NumberConverter.toInt32(parts[6]);
			tc.memorySize = ToMemorySize.get(parts[7]);
			break;

		default:
			throw new UnsupportedOperationException(String.format("Invalid opkind: '%s'", parts[0]));
		}
	}

	private static String toHexBytes(String value) {
		try {
			HexUtils.toByteArray(value);
		}
		catch (Exception ex) {
			throw new UnsupportedOperationException(String.format("Invalid hex bytes: '%s'", value));
		}
		return value;
	}

	private static int toRegister(String value) {
		if (value.equals(""))
			return Register.NONE;
		Integer reg = ToRegister.tryGet(value);
		if (reg == null)
			throw new UnsupportedOperationException(String.format("Invalid Register value: '%s'", value));
		return reg.intValue();
	}
}
