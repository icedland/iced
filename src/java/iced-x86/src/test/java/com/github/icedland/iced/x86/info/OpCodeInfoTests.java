// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class OpCodeInfoTests {
	@ParameterizedTest
	@MethodSource("test_all_OpCodeInfos_Data")
	void test_all_OpCodeInfos(int lineNo, int code, String opCodeString, String instructionString, OpCodeInfoTestCase tc) {
		OpCodeInfo info = Code.toOpCode(tc.code);
		assertEquals(tc.code, info.getCode());
		assertEquals(tc.opCodeString, info.toOpCodeString());
		assertEquals(tc.instructionString, info.toInstructionString());
		assertTrue(info.toInstructionString() == info.toString());
		assertEquals(tc.mnemonic, info.getMnemonic());
		assertEquals(tc.encoding, info.getEncoding());
		assertEquals(tc.isInstruction, info.isInstruction());
		assertEquals(tc.mode16, info.getMode16());
		assertEquals(tc.mode16, info.isAvailableInMode(16));
		assertEquals(tc.mode32, info.getMode32());
		assertEquals(tc.mode32, info.isAvailableInMode(32));
		assertEquals(tc.mode64, info.getMode64());
		assertEquals(tc.mode64, info.isAvailableInMode(64));
		assertEquals(tc.fwait, info.getFwait());
		assertEquals(tc.operandSize, info.getOperandSize());
		assertEquals(tc.addressSize, info.getAddressSize());
		assertEquals(tc.l, info.getL());
		assertEquals(tc.w, info.getW());
		assertEquals(tc.isLIG, info.isLIG());
		assertEquals(tc.isWIG, info.isWIG());
		assertEquals(tc.isWIG32, info.isWIG32());
		assertEquals(tc.tupleType, info.getTupleType());
		assertEquals(tc.memorySize, info.getMemorySize());
		assertEquals(tc.broadcastMemorySize, info.getBroadcastMemorySize());
		assertEquals(tc.decoderOption, info.getDecoderOption());
		assertEquals(tc.canBroadcast, info.canBroadcast());
		assertEquals(tc.canUseRoundingControl, info.canUseRoundingControl());
		assertEquals(tc.canSuppressAllExceptions, info.canSuppressAllExceptions());
		assertEquals(tc.canUseOpMaskRegister, info.canUseOpMaskRegister());
		assertEquals(tc.requireOpMaskRegister, info.getRequireOpMaskRegister());
		if (tc.requireOpMaskRegister) {
			assertTrue(info.canUseOpMaskRegister());
			assertFalse(info.canUseZeroingMasking());
		}
		assertEquals(tc.canUseZeroingMasking, info.canUseZeroingMasking());
		assertEquals(tc.canUseLockPrefix, info.canUseLockPrefix());
		assertEquals(tc.canUseXacquirePrefix, info.canUseXacquirePrefix());
		assertEquals(tc.canUseXreleasePrefix, info.canUseXreleasePrefix());
		assertEquals(tc.canUseRepPrefix, info.canUseRepPrefix());
		assertEquals(tc.canUseRepnePrefix, info.canUseRepnePrefix());
		assertEquals(tc.canUseBndPrefix, info.canUseBndPrefix());
		assertEquals(tc.canUseHintTakenPrefix, info.canUseHintTakenPrefix());
		assertEquals(tc.canUseNotrackPrefix, info.canUseNotrackPrefix());
		assertEquals(tc.ignoresRoundingControl, info.getIgnoresRoundingControl());
		assertEquals(tc.amdLockRegBit, info.getAmdLockRegBit());
		assertEquals(tc.defaultOpSize64, info.getDefaultOpSize64());
		assertEquals(tc.forceOpSize64, info.getForceOpSize64());
		assertEquals(tc.intelForceOpSize64, info.getIntelForceOpSize64());
		assertEquals(tc.cpl0 && !tc.cpl1 && !tc.cpl2 && !tc.cpl3, info.getMustBeCpl0());
		assertEquals(tc.cpl0, info.getCpl0());
		assertEquals(tc.cpl1, info.getCpl1());
		assertEquals(tc.cpl2, info.getCpl2());
		assertEquals(tc.cpl3, info.getCpl3());
		assertEquals(tc.isInputOutput, info.isInputOutput());
		assertEquals(tc.isNop, info.isNop());
		assertEquals(tc.isReservedNop, info.isReservedNop());
		assertEquals(tc.isSerializingIntel, info.isSerializingIntel());
		assertEquals(tc.isSerializingAmd, info.isSerializingAmd());
		assertEquals(tc.mayRequireCpl0, info.getMayRequireCpl0());
		assertEquals(tc.isCetTracked, info.isCetTracked());
		assertEquals(tc.isNonTemporal, info.isNonTemporal());
		assertEquals(tc.isFpuNoWait, info.isFpuNoWait());
		assertEquals(tc.ignoresModBits, info.getIgnoresModBits());
		assertEquals(tc.no66, info.getNo66());
		assertEquals(tc.nFx, info.getNFx());
		assertEquals(tc.requiresUniqueRegNums, info.getRequiresUniqueRegNums());
		assertEquals(tc.requiresUniqueDestRegNum, info.getRequiresUniqueDestRegNum());
		assertEquals(tc.isPrivileged, info.isPrivileged());
		assertEquals(tc.isSaveRestore, info.isSaveRestore());
		assertEquals(tc.isStackInstruction, info.isStackInstruction());
		assertEquals(tc.ignoresSegment, info.getIgnoresSegment());
		assertEquals(tc.isOpMaskReadWrite, info.isOpMaskReadWrite());
		assertEquals(tc.realMode, info.getRealMode());
		assertEquals(tc.protectedMode, info.getProtectedMode());
		assertEquals(tc.virtual8086Mode, info.getVirtual8086Mode());
		assertEquals(tc.compatibilityMode, info.getCompatibilityMode());
		assertEquals(tc.longMode, info.getLongMode());
		assertEquals(tc.useOutsideSmm, info.getUseOutsideSmm());
		assertEquals(tc.useInSmm, info.getUseInSmm());
		assertEquals(tc.useOutsideEnclaveSgx, info.getUseOutsideEnclaveSgx());
		assertEquals(tc.useInEnclaveSgx1, info.getUseInEnclaveSgx1());
		assertEquals(tc.useInEnclaveSgx2, info.getUseInEnclaveSgx2());
		assertEquals(tc.useOutsideVmxOp, info.getUseOutsideVmxOp());
		assertEquals(tc.useInVmxRootOp, info.getUseInVmxRootOp());
		assertEquals(tc.useInVmxNonRootOp, info.getUseInVmxNonRootOp());
		assertEquals(tc.useOutsideSeam, info.getUseOutsideSeam());
		assertEquals(tc.useInSeam, info.getUseInSeam());
		assertEquals(tc.tdxNonRootGenUd, info.getTdxNonRootGenUd());
		assertEquals(tc.tdxNonRootGenVe, info.getTdxNonRootGenVe());
		assertEquals(tc.tdxNonRootMayGenEx, info.getTdxNonRootMayGenEx());
		assertEquals(tc.intelVmExit, info.getIntelVmExit());
		assertEquals(tc.intelMayVmExit, info.getIntelMayVmExit());
		assertEquals(tc.intelSmmVmExit, info.getIntelSmmVmExit());
		assertEquals(tc.amdVmExit, info.getAmdVmExit());
		assertEquals(tc.amdMayVmExit, info.getAmdMayVmExit());
		assertEquals(tc.tsxAbort, info.getTsxAbort());
		assertEquals(tc.tsxImplAbort, info.getTsxImplAbort());
		assertEquals(tc.tsxMayAbort, info.getTsxMayAbort());
		assertEquals(tc.intelDecoder16, info.getIntelDecoder16());
		assertEquals(tc.intelDecoder32, info.getIntelDecoder32());
		assertEquals(tc.intelDecoder64, info.getIntelDecoder64());
		assertEquals(tc.amdDecoder16, info.getAmdDecoder16());
		assertEquals(tc.amdDecoder32, info.getAmdDecoder32());
		assertEquals(tc.amdDecoder64, info.getAmdDecoder64());
		assertEquals(tc.table, info.getTable());
		assertEquals(tc.mandatoryPrefix, info.getMandatoryPrefix());
		assertEquals(tc.opCode, info.getOpCode());
		assertEquals(tc.opCodeLength, info.getOpCodeLength());
		assertEquals(tc.isGroup, info.isGroup());
		assertEquals(tc.groupIndex, info.getGroupIndex());
		assertEquals(tc.isRmGroup, info.isRmGroup());
		assertEquals(tc.rmGroupIndex, info.getRmGroupIndex());
		assertEquals(tc.opCount, info.getOpCount());
		assertEquals(tc.op0Kind, info.getOp0Kind());
		assertEquals(tc.op1Kind, info.getOp1Kind());
		assertEquals(tc.op2Kind, info.getOp2Kind());
		assertEquals(tc.op3Kind, info.getOp3Kind());
		assertEquals(tc.op4Kind, info.getOp4Kind());
		assertEquals(tc.op0Kind, info.getOpKind(0));
		assertEquals(tc.op1Kind, info.getOpKind(1));
		assertEquals(tc.op2Kind, info.getOpKind(2));
		assertEquals(tc.op3Kind, info.getOpKind(3));
		assertEquals(tc.op4Kind, info.getOpKind(4));
		assertEquals(5, IcedConstants.MAX_OP_COUNT);
		for (int i = tc.opCount; i < IcedConstants.MAX_OP_COUNT; i++)
			assertEquals(OpCodeOperandKind.NONE, info.getOpKind(i));
		assertEquals(tc.mvexEHBit, info.getMvexEHBit());
		assertEquals(tc.mvexCanUseEvictionHint, info.getMvexCanUseEvictionHint());
		assertEquals(tc.mvexCanUseImmRoundingControl, info.getMvexCanUseImmRoundingControl());
		assertEquals(tc.mvexIgnoresOpMaskRegister, info.getMvexIgnoresOpMaskRegister());
		assertEquals(tc.mvexNoSaeRc, info.getMvexNoSaeRc());
		assertEquals(tc.mvexTupleTypeLutKind, info.getMvexTupleTypeLutKind());
		assertEquals(tc.mvexConversionFunc, info.getMvexConversionFunc());
		assertEquals(tc.mvexValidConversionFuncsMask, info.getMvexValidConversionFuncsMask());
		assertEquals(tc.mvexValidSwizzleFuncsMask, info.getMvexValidSwizzleFuncsMask());
	}

	public static Iterable<Arguments> test_all_OpCodeInfos_Data() {
		OpCodeInfoTestCase[] allTests = OpCodeInfoTestCases.opCodeInfoTests;
		ArrayList<Arguments> result = new ArrayList<Arguments>(allTests.length);
		for (OpCodeInfoTestCase tc : allTests)
			result.add(Arguments.of(tc.lineNumber, tc.code, tc.opCodeString, tc.instructionString, tc));
		return result;
	}

	@Test
	void getOpKindThrowsIfInvalidInput() {
		OpCodeInfo info = Code.toOpCode(Code.AAA);
		assertThrows(IllegalArgumentException.class, () -> info.getOpKind(-0x8000_0000));
		assertThrows(IllegalArgumentException.class, () -> info.getOpKind(-1));
		info.getOpKind(0);
		info.getOpKind(IcedConstants.MAX_OP_COUNT - 1);
		assertThrows(IllegalArgumentException.class, () -> info.getOpKind(IcedConstants.MAX_OP_COUNT));
		assertThrows(IllegalArgumentException.class, () -> info.getOpKind(0x7FFF_FFFF));
	}

	@Test
	void verify_Instruction_OpCodeInfo() {
		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			Instruction instruction = new Instruction();
			instruction.setCode(i);
			assertTrue(Code.toOpCode(i) == instruction.getOpCode());
		}
	}

	@Test
	void make_sure_all_Code_values_are_tested_exactly_once() {
		boolean[] tested = new boolean[IcedConstants.CODE_ENUM_COUNT];
		for (OpCodeInfoTestCase info : OpCodeInfoTestCases.opCodeInfoTests) {
			assertFalse(tested[info.code]);
			tested[info.code] = true;
		}
		StringBuilder sb = new StringBuilder();
		String[] codeNames = ToCode.names();
		for (int i = 0; i < tested.length; i++) {
			if (!tested[i] && !CodeUtils.isIgnored(codeNames[i])) {
				if (sb.length() > 0)
					sb.append(',');
				sb.append(codeNames[i]);
			}
		}
		assertEquals("", sb.toString());
	}
}
