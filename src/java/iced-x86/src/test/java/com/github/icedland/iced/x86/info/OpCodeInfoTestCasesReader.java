// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.MvexEHBit;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.ToDecoderOptions;
import com.github.icedland.iced.x86.ToMemorySize;
import com.github.icedland.iced.x86.ToMnemonic;
import com.github.icedland.iced.x86.ToMvexConvFn;
import com.github.icedland.iced.x86.ToMvexTupleTypeLutKind;
import com.github.icedland.iced.x86.ToOpCodeOperandKind;
import com.github.icedland.iced.x86.ToTupleType;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class OpCodeInfoTestCasesReader {
	static OpCodeInfoTestCase[] readFile(String filename) {
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		ArrayList<OpCodeInfoTestCase> result = new ArrayList<OpCodeInfoTestCase>(lines.size());
		int lineNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;
			OpCodeInfoTestCase testCase;
			try {
				testCase = readTestCase(line, lineNo);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(
						String.format("Error parsing opcode test case file '%s', line %d: %s", filename, lineNo, ex.getMessage()));
			}
			if (testCase != null)
				result.add(testCase);
		}
		return result.toArray(new OpCodeInfoTestCase[result.size()]);
	}

	private static OpCodeInfoTestCase readTestCase(String line, int lineNo) {
		String[] parts = line.split(",");
		if (parts.length != 11)
			throw new UnsupportedOperationException(String.format("Invalid number of commas (%d commas)", parts.length - 1));

		OpCodeInfoTestCase tc = new OpCodeInfoTestCase();
		tc.lineNumber = lineNo;
		tc.isInstruction = true;
		tc.groupIndex = -1;

		String code = parts[0].trim();
		if (CodeUtils.isIgnored(code))
			return null;
		tc.code = ToCode.get(code);
		tc.mnemonic = ToMnemonic.get(parts[1].trim());
		tc.memorySize = ToMemorySize.get(parts[2].trim());
		tc.broadcastMemorySize = ToMemorySize.get(parts[3].trim());
		tc.encoding = toEncoding(parts[4].trim());
		tc.mandatoryPrefix = toMandatoryPrefix(parts[5].trim());
		tc.table = toTable(parts[6].trim());
		OpCodeResult opCodeResult = toOpCode(parts[7].trim());
		tc.opCode = opCodeResult.opCode;
		tc.opCodeLength = opCodeResult.length;
		tc.opCodeString = parts[8].trim();
		tc.instructionString = parts[9].trim().replace('|', ',');

		boolean gotVectorLength = false;
		boolean gotW = false;
		for (String part : parts[10].split(" ")) {
			String key = part.trim();
			if (key.length() == 0)
				continue;
			int index = key.indexOf('=');
			if (index >= 0) {
				String value = key.substring(index + 1);
				key = key.substring(0, index);
				switch (key) {
				case OpCodeInfoKeys.GROUP_INDEX:
					tc.groupIndex = NumberConverter.toInt32(value);
					if (tc.groupIndex > 7)
						throw new UnsupportedOperationException(String.format("Invalid group index: %s", value));
					tc.isGroup = true;
					break;

				case OpCodeInfoKeys.RM_GROUP_INDEX:
					tc.rmGroupIndex = NumberConverter.toInt32(value);
					if (tc.rmGroupIndex > 7)
						throw new UnsupportedOperationException(String.format("Invalid group index: %s", value));
					tc.isRmGroup = true;
					break;

				case OpCodeInfoKeys.OP_CODE_OPERAND_KIND:
					String[] opParts = value.split(";");
					tc.opCount = opParts.length;
					if (opParts.length >= 1)
						tc.op0Kind = ToOpCodeOperandKind.get(opParts[0]);
					if (opParts.length >= 2)
						tc.op1Kind = ToOpCodeOperandKind.get(opParts[1]);
					if (opParts.length >= 3)
						tc.op2Kind = ToOpCodeOperandKind.get(opParts[2]);
					if (opParts.length >= 4)
						tc.op3Kind = ToOpCodeOperandKind.get(opParts[3]);
					if (opParts.length >= 5)
						tc.op4Kind = ToOpCodeOperandKind.get(opParts[4]);
					int expected = 5;
					assert IcedConstants.MAX_OP_COUNT == expected : IcedConstants.MAX_OP_COUNT;
					if (opParts.length >= 6)
						throw new UnsupportedOperationException(String.format("Invalid number of operands: '%s'", value));
					break;

				case OpCodeInfoKeys.TUPLE_TYPE:
					tc.tupleType = ToTupleType.get(value.trim());
					break;

				case OpCodeInfoKeys.DECODER_OPTION:
					tc.decoderOption = ToDecoderOptions.get(value.trim());
					break;

				case OpCodeInfoKeys.MVEX:
					String[] mvexParts = value.split(";");
					if (mvexParts.length != 4)
						throw new UnsupportedOperationException(
								String.format("Invalid number of semicolons. Expected 3, found %d", mvexParts.length - 1));
					tc.mvexTupleTypeLutKind = ToMvexTupleTypeLutKind.get(mvexParts[0].trim());
					tc.mvexConversionFunc = ToMvexConvFn.get(mvexParts[1].trim());
					tc.mvexValidConversionFuncsMask = NumberConverter.toUInt8(mvexParts[2].trim());
					tc.mvexValidSwizzleFuncsMask = NumberConverter.toUInt8(mvexParts[3].trim());
					break;

				default:
					throw new UnsupportedOperationException(String.format("Invalid key: '%s'", key));
				}
			}
			else {
				switch (key) {
				case OpCodeInfoFlags.NO_INSTRUCTION:
					tc.isInstruction = false;
					break;

				case OpCodeInfoFlags.BIT16:
					tc.mode16 = true;
					break;

				case OpCodeInfoFlags.BIT32:
					tc.mode32 = true;
					break;

				case OpCodeInfoFlags.BIT64:
					tc.mode64 = true;
					break;

				case OpCodeInfoFlags.FWAIT:
					tc.fwait = true;
					break;

				case OpCodeInfoFlags.OPERAND_SIZE16:
					tc.operandSize = 16;
					break;

				case OpCodeInfoFlags.OPERAND_SIZE32:
					tc.operandSize = 32;
					break;

				case OpCodeInfoFlags.OPERAND_SIZE64:
					tc.operandSize = 64;
					break;

				case OpCodeInfoFlags.ADDRESS_SIZE16:
					tc.addressSize = 16;
					break;

				case OpCodeInfoFlags.ADDRESS_SIZE32:
					tc.addressSize = 32;
					break;

				case OpCodeInfoFlags.ADDRESS_SIZE64:
					tc.addressSize = 64;
					break;

				case OpCodeInfoFlags.LIG:
					tc.isLIG = true;
					gotVectorLength = true;
					break;

				case OpCodeInfoFlags.L0:
					tc.l = 0;
					gotVectorLength = true;
					break;

				case OpCodeInfoFlags.L1:
					tc.l = 1;
					gotVectorLength = true;
					break;

				case OpCodeInfoFlags.L128:
					tc.l = 0;
					gotVectorLength = true;
					break;

				case OpCodeInfoFlags.L256:
					tc.l = 1;
					gotVectorLength = true;
					break;

				case OpCodeInfoFlags.L512:
					tc.l = 2;
					gotVectorLength = true;
					break;

				case OpCodeInfoFlags.WIG:
					tc.isWIG = true;
					gotW = true;
					break;

				case OpCodeInfoFlags.WIG32:
					tc.w = 0;
					tc.isWIG32 = true;
					gotW = true;
					break;

				case OpCodeInfoFlags.W0:
					tc.w = 0;
					gotW = true;
					break;

				case OpCodeInfoFlags.W1:
					tc.w = 1;
					gotW = true;
					break;

				case OpCodeInfoFlags.BROADCAST:
					tc.canBroadcast = true;
					break;

				case OpCodeInfoFlags.ROUNDING_CONTROL:
					tc.canUseRoundingControl = true;
					break;

				case OpCodeInfoFlags.SUPPRESS_ALL_EXCEPTIONS:
					tc.canSuppressAllExceptions = true;
					break;

				case OpCodeInfoFlags.OP_MASK_REGISTER:
					tc.canUseOpMaskRegister = true;
					break;

				case OpCodeInfoFlags.REQUIRE_OP_MASK_REGISTER:
					tc.canUseOpMaskRegister = true;
					tc.requireOpMaskRegister = true;
					break;

				case OpCodeInfoFlags.ZEROING_MASKING:
					tc.canUseZeroingMasking = true;
					break;

				case OpCodeInfoFlags.LOCK:
					tc.canUseLockPrefix = true;
					break;

				case OpCodeInfoFlags.XACQUIRE:
					tc.canUseXacquirePrefix = true;
					break;

				case OpCodeInfoFlags.XRELEASE:
					tc.canUseXreleasePrefix = true;
					break;

				case OpCodeInfoFlags.REP:
				case OpCodeInfoFlags.REPE:
					tc.canUseRepPrefix = true;
					break;

				case OpCodeInfoFlags.REPNE:
					tc.canUseRepnePrefix = true;
					break;

				case OpCodeInfoFlags.BND:
					tc.canUseBndPrefix = true;
					break;

				case OpCodeInfoFlags.HINT_TAKEN:
					tc.canUseHintTakenPrefix = true;
					break;

				case OpCodeInfoFlags.NOTRACK:
					tc.canUseNotrackPrefix = true;
					break;

				case OpCodeInfoFlags.IGNORES_ROUNDING_CONTROL:
					tc.ignoresRoundingControl = true;
					break;

				case OpCodeInfoFlags.AMD_LOCK_REG_BIT:
					tc.amdLockRegBit = true;
					break;

				case OpCodeInfoFlags.DEFAULT_OP_SIZE64:
					tc.defaultOpSize64 = true;
					break;

				case OpCodeInfoFlags.FORCE_OP_SIZE64:
					tc.forceOpSize64 = true;
					break;

				case OpCodeInfoFlags.INTEL_FORCE_OP_SIZE64:
					tc.intelForceOpSize64 = true;
					break;

				case OpCodeInfoFlags.CPL0:
					tc.cpl0 = true;
					break;

				case OpCodeInfoFlags.CPL1:
					tc.cpl1 = true;
					break;

				case OpCodeInfoFlags.CPL2:
					tc.cpl2 = true;
					break;

				case OpCodeInfoFlags.CPL3:
					tc.cpl3 = true;
					break;

				case OpCodeInfoFlags.INPUT_OUTPUT:
					tc.isInputOutput = true;
					break;

				case OpCodeInfoFlags.NOP:
					tc.isNop = true;
					break;

				case OpCodeInfoFlags.RESERVED_NOP:
					tc.isReservedNop = true;
					break;

				case OpCodeInfoFlags.SERIALIZING_INTEL:
					tc.isSerializingIntel = true;
					break;

				case OpCodeInfoFlags.SERIALIZING_AMD:
					tc.isSerializingAmd = true;
					break;

				case OpCodeInfoFlags.MAY_REQUIRE_CPL0:
					tc.mayRequireCpl0 = true;
					break;

				case OpCodeInfoFlags.CET_TRACKED:
					tc.isCetTracked = true;
					break;

				case OpCodeInfoFlags.NON_TEMPORAL:
					tc.isNonTemporal = true;
					break;

				case OpCodeInfoFlags.FPU_NO_WAIT:
					tc.isFpuNoWait = true;
					break;

				case OpCodeInfoFlags.IGNORES_MOD_BITS:
					tc.ignoresModBits = true;
					break;

				case OpCodeInfoFlags.NO66:
					tc.no66 = true;
					break;

				case OpCodeInfoFlags.NFX:
					tc.nFx = true;
					break;

				case OpCodeInfoFlags.REQUIRES_UNIQUE_REG_NUMS:
					tc.requiresUniqueRegNums = true;
					break;

				case OpCodeInfoFlags.PRIVILEGED:
					tc.isPrivileged = true;
					break;

				case OpCodeInfoFlags.SAVE_RESTORE:
					tc.isSaveRestore = true;
					break;

				case OpCodeInfoFlags.STACK_INSTRUCTION:
					tc.isStackInstruction = true;
					break;

				case OpCodeInfoFlags.IGNORES_SEGMENT:
					tc.ignoresSegment = true;
					break;

				case OpCodeInfoFlags.OP_MASK_READ_WRITE:
					tc.isOpMaskReadWrite = true;
					break;

				case OpCodeInfoFlags.REAL_MODE:
					tc.realMode = true;
					break;

				case OpCodeInfoFlags.PROTECTED_MODE:
					tc.protectedMode = true;
					break;

				case OpCodeInfoFlags.VIRTUAL8086_MODE:
					tc.virtual8086Mode = true;
					break;

				case OpCodeInfoFlags.COMPATIBILITY_MODE:
					tc.compatibilityMode = true;
					break;

				case OpCodeInfoFlags.LONG_MODE:
					tc.longMode = true;
					break;

				case OpCodeInfoFlags.USE_OUTSIDE_SMM:
					tc.useOutsideSmm = true;
					break;

				case OpCodeInfoFlags.USE_IN_SMM:
					tc.useInSmm = true;
					break;

				case OpCodeInfoFlags.USE_OUTSIDE_ENCLAVE_SGX:
					tc.useOutsideEnclaveSgx = true;
					break;

				case OpCodeInfoFlags.USE_IN_ENCLAVE_SGX1:
					tc.useInEnclaveSgx1 = true;
					break;

				case OpCodeInfoFlags.USE_IN_ENCLAVE_SGX2:
					tc.useInEnclaveSgx2 = true;
					break;

				case OpCodeInfoFlags.USE_OUTSIDE_VMX_OP:
					tc.useOutsideVmxOp = true;
					break;

				case OpCodeInfoFlags.USE_IN_VMX_ROOT_OP:
					tc.useInVmxRootOp = true;
					break;

				case OpCodeInfoFlags.USE_IN_VMX_NON_ROOT_OP:
					tc.useInVmxNonRootOp = true;
					break;

				case OpCodeInfoFlags.USE_OUTSIDE_SEAM:
					tc.useOutsideSeam = true;
					break;

				case OpCodeInfoFlags.USE_IN_SEAM:
					tc.useInSeam = true;
					break;

				case OpCodeInfoFlags.TDX_NON_ROOT_GEN_UD:
					tc.tdxNonRootGenUd = true;
					break;

				case OpCodeInfoFlags.TDX_NON_ROOT_GEN_VE:
					tc.tdxNonRootGenVe = true;
					break;

				case OpCodeInfoFlags.TDX_NON_ROOT_MAY_GEN_EX:
					tc.tdxNonRootMayGenEx = true;
					break;

				case OpCodeInfoFlags.INTEL_VM_EXIT:
					tc.intelVmExit = true;
					break;

				case OpCodeInfoFlags.INTEL_MAY_VM_EXIT:
					tc.intelMayVmExit = true;
					break;

				case OpCodeInfoFlags.INTEL_SMM_VM_EXIT:
					tc.intelSmmVmExit = true;
					break;

				case OpCodeInfoFlags.AMD_VM_EXIT:
					tc.amdVmExit = true;
					break;

				case OpCodeInfoFlags.AMD_MAY_VM_EXIT:
					tc.amdMayVmExit = true;
					break;

				case OpCodeInfoFlags.TSX_ABORT:
					tc.tsxAbort = true;
					break;

				case OpCodeInfoFlags.TSX_IMPL_ABORT:
					tc.tsxImplAbort = true;
					break;

				case OpCodeInfoFlags.TSX_MAY_ABORT:
					tc.tsxMayAbort = true;
					break;

				case OpCodeInfoFlags.INTEL_DECODER16:
					tc.intelDecoder16 = true;
					break;

				case OpCodeInfoFlags.INTEL_DECODER32:
					tc.intelDecoder32 = true;
					break;

				case OpCodeInfoFlags.INTEL_DECODER64:
					tc.intelDecoder64 = true;
					break;

				case OpCodeInfoFlags.AMD_DECODER16:
					tc.amdDecoder16 = true;
					break;

				case OpCodeInfoFlags.AMD_DECODER32:
					tc.amdDecoder32 = true;
					break;

				case OpCodeInfoFlags.AMD_DECODER64:
					tc.amdDecoder64 = true;
					break;

				case OpCodeInfoFlags.REQUIRES_UNIQUE_DEST_REG_NUM:
					tc.requiresUniqueDestRegNum = true;
					break;

				case OpCodeInfoFlags.EH0:
					tc.mvexEHBit = MvexEHBit.EH0;
					break;

				case OpCodeInfoFlags.EH1:
					tc.mvexEHBit = MvexEHBit.EH1;
					break;

				case OpCodeInfoFlags.EVICTION_HINT:
					tc.mvexCanUseEvictionHint = true;
					break;

				case OpCodeInfoFlags.IMM_ROUNDING_CONTROL:
					tc.mvexCanUseImmRoundingControl = true;
					break;

				case OpCodeInfoFlags.IGNORES_OP_MASK_REGISTER:
					tc.mvexIgnoresOpMaskRegister = true;
					break;

				case OpCodeInfoFlags.NO_SAE_ROUNDING_CONTROL:
					tc.mvexNoSaeRc = true;
					break;

				default:
					throw new UnsupportedOperationException(String.format("Invalid key: '%s'", key));
				}
			}
		}
		switch (tc.encoding) {
		case EncodingKind.LEGACY:
		case EncodingKind.D3NOW:
			break;
		case EncodingKind.VEX:
		case EncodingKind.EVEX:
		case EncodingKind.XOP:
		case EncodingKind.MVEX:
			if (!gotVectorLength)
				throw new UnsupportedOperationException("Missing vector length: L0/L1/L128/L256/L512/LIG");
			if (!gotW)
				throw new UnsupportedOperationException("Missing W bit: W0/W1/WIG/WIG32");
			break;
		default:
			throw new UnsupportedOperationException();
		}

		return tc;
	}

	private static int toEncoding(String value) {
		Integer i = OpCodeInfoDicts.toEncodingKind.get(value);
		if (i != null)
			return i;
		throw new UnsupportedOperationException(String.format("Invalid encoding value: '%s'", value));
	}

	private static int toMandatoryPrefix(String value) {
		Integer i = OpCodeInfoDicts.toMandatoryPrefix.get(value);
		if (i != null)
			return i;
		throw new UnsupportedOperationException(String.format("Invalid mandatory prefix value: '%s'", value));
	}

	private static int toTable(String value) {
		Integer i = OpCodeInfoDicts.toOpCodeTableKind.get(value);
		if (i != null)
			return i;
		throw new UnsupportedOperationException(String.format("Invalid opcode table value: '%s'", value));
	}

	static final class OpCodeResult {
		public int opCode;
		public int length;

		public OpCodeResult(int opCode, int length) {
			this.opCode = opCode;
			this.length = length;
		}
	}

	private static OpCodeResult toOpCode(String value) {
		if (value.length() == 2 || value.length() == 4) {
			int length = value.length() / 2;
			try {
				int opCode = Integer.parseUnsignedInt(value, 16);
				return new OpCodeResult(opCode, length);
			}
			catch (NumberFormatException ex) {
			}
		}
		throw new UnsupportedOperationException(String.format("Invalid opcode: '%s'", value));
	}
}
