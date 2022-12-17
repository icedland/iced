// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.HashMap;
import java.util.HashSet;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.ConditionCode;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemorySize;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.info.MiscTestsData.CmovccInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.CmpccxaddInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.JccNearInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.JccShortInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.JkccNearInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.JkccShortInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.JmpInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.LoopccInfo;
import com.github.icedland.iced.x86.info.MiscTestsData.SetccInfo;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class MiscTests {
	@Test
	void isBranchCall() {
		HashSet<Integer> jccShort = MiscTestsData.jccShort;
		HashSet<Integer> jcxShort = MiscTestsData.jrcxz;
		HashSet<Integer> jmpNear = MiscTestsData.jmpNear;
		HashSet<Integer> jmpFar = MiscTestsData.jmpFar;
		HashSet<Integer> jmpShort = MiscTestsData.jmpShort;
		HashSet<Integer> jmpNearIndirect = MiscTestsData.jmpNearIndirect;
		HashSet<Integer> jmpFarIndirect = MiscTestsData.jmpFarIndirect;
		HashSet<Integer> jccNear = MiscTestsData.jccNear;
		HashSet<Integer> callFar = MiscTestsData.callFar;
		HashSet<Integer> callNear = MiscTestsData.callNear;
		HashSet<Integer> callNearIndirect = MiscTestsData.callNearIndirect;
		HashSet<Integer> callFarIndirect = MiscTestsData.callFarIndirect;
		HashSet<Integer> jkccShort = MiscTestsData.jkccShort;
		HashSet<Integer> jkccNear = MiscTestsData.jkccNear;
		HashSet<Integer> loop = MiscTestsData.loop;

		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			int code = i;
			Instruction instruction = new Instruction();
			instruction.setCode(code);

			assertEquals(jccShort.contains(code) || jccNear.contains(code), Code.isJccShortOrNear(code));
			assertEquals(Code.isJccShortOrNear(code), instruction.isJccShortOrNear());

			assertEquals(jccNear.contains(code), Code.isJccNear(code));
			assertEquals(Code.isJccNear(code), instruction.isJccNear());

			assertEquals(jccShort.contains(code), Code.isJccShort(code));
			assertEquals(Code.isJccShort(code), instruction.isJccShort());

			assertEquals(jcxShort.contains(code), Code.isJcxShort(code));
			assertEquals(Code.isJcxShort(code), instruction.isJcxShort());

			assertEquals(jmpShort.contains(code), Code.isJmpShort(code));
			assertEquals(Code.isJmpShort(code), instruction.isJmpShort());

			assertEquals(jmpNear.contains(code), Code.isJmpNear(code));
			assertEquals(Code.isJmpNear(code), instruction.isJmpNear());

			assertEquals(jmpShort.contains(code) || jmpNear.contains(code), Code.isJmpShortOrNear(code));
			assertEquals(Code.isJmpShortOrNear(code), instruction.isJmpShortOrNear());

			assertEquals(jmpFar.contains(code), Code.isJmpFar(code));
			assertEquals(Code.isJmpFar(code), instruction.isJmpFar());

			assertEquals(callNear.contains(code), Code.isCallNear(code));
			assertEquals(Code.isCallNear(code), instruction.isCallNear());

			assertEquals(callFar.contains(code), Code.isCallFar(code));
			assertEquals(Code.isCallFar(code), instruction.isCallFar());

			assertEquals(jmpNearIndirect.contains(code), Code.isJmpNearIndirect(code));
			assertEquals(Code.isJmpNearIndirect(code), instruction.isJmpNearIndirect());

			assertEquals(jmpFarIndirect.contains(code), Code.isJmpFarIndirect(code));
			assertEquals(Code.isJmpFarIndirect(code), instruction.isJmpFarIndirect());

			assertEquals(callNearIndirect.contains(code), Code.isCallNearIndirect(code));
			assertEquals(Code.isCallNearIndirect(code), instruction.isCallNearIndirect());

			assertEquals(callFarIndirect.contains(code), Code.isCallFarIndirect(code));
			assertEquals(Code.isCallFarIndirect(code), instruction.isCallFarIndirect());

			assertEquals(jkccShort.contains(code) || jkccNear.contains(code), Code.isJkccShortOrNear(code));
			assertEquals(Code.isJkccShortOrNear(code), instruction.isJkccShortOrNear());

			assertEquals(jkccNear.contains(code), Code.isJkccNear(code));
			assertEquals(Code.isJkccNear(code), instruction.isJkccNear());

			assertEquals(jkccShort.contains(code), Code.isJkccShort(code));
			assertEquals(Code.isJkccShort(code), instruction.isJkccShort());
			assertEquals(loop.contains(code), Code.isLoop(code) || Code.isLoopcc(code));
			assertEquals(Code.isLoop(code), instruction.isLoop());
			assertEquals(Code.isLoopcc(code), instruction.isLoopcc());
		}
	}

	@Test
	void verify_NegateConditionCode() {
		HashMap<Integer, Integer> toNegatedCodeValue = new HashMap<Integer, Integer>();
		for (JccShortInfo info : MiscTestsData.jccShortInfos)
			toNegatedCodeValue.put(info.jcc, info.negated);
		for (JccNearInfo info : MiscTestsData.jccNearInfos)
			toNegatedCodeValue.put(info.jcc, info.negated);
		for (SetccInfo info : MiscTestsData.setccInfos)
			toNegatedCodeValue.put(info.setcc, info.negated);
		for (CmovccInfo info : MiscTestsData.cmovccInfos)
			toNegatedCodeValue.put(info.cmovcc, info.negated);
		for (CmpccxaddInfo info : MiscTestsData.cmpccxaddInfos)
			toNegatedCodeValue.put(info.cmpccxadd, info.negated);
		for (LoopccInfo info : MiscTestsData.loopccInfos)
			toNegatedCodeValue.put(info.loopcc, info.negated);
		for (JkccShortInfo info : MiscTestsData.jkccShortInfos)
			toNegatedCodeValue.put(info.jkcc, info.negated);
		for (JkccNearInfo info : MiscTestsData.jkccNearInfos)
			toNegatedCodeValue.put(info.jkcc, info.negated);

		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			int code = i;
			Instruction instruction = new Instruction();
			instruction.setCode(code);

			Integer negatedCodeValue = toNegatedCodeValue.get(code);
			if (negatedCodeValue == null)
				negatedCodeValue = code;

			assertEquals(negatedCodeValue, Code.negateConditionCode(code));
			instruction.negateConditionCode();
			assertEquals(negatedCodeValue, instruction.getCode());
		}
	}

	@Test
	void verify_ToShortBranch() {
		HashMap<Integer, Integer> toShortBranch = new HashMap<Integer, Integer>();
		for (JccNearInfo info : MiscTestsData.jccNearInfos)
			toShortBranch.put(info.jcc, info.jccShort);
		for (JmpInfo info : MiscTestsData.jmpInfos)
			toShortBranch.put(info.jmpNear, info.jmpShort);
		for (JkccNearInfo info : MiscTestsData.jkccNearInfos)
			toShortBranch.put(info.jkcc, info.jkccShort);

		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			int code = i;
			Instruction instruction = new Instruction();
			instruction.setCode(code);

			Integer shortCodeValue = toShortBranch.get(code);
			if (shortCodeValue == null)
				shortCodeValue = code;

			assertEquals(shortCodeValue, Code.toShortBranch(code));
			instruction.toShortBranch();
			assertEquals(shortCodeValue, instruction.getCode());
		}
	}

	@Test
	void verify_ToNearBranch() {
		HashMap<Integer, Integer> toNearBranch = new HashMap<Integer, Integer>();
		for (JccShortInfo info : MiscTestsData.jccShortInfos)
			toNearBranch.put(info.jcc, info.jccNear);
		for (JmpInfo info : MiscTestsData.jmpInfos)
			toNearBranch.put(info.jmpShort, info.jmpNear);
		for (JkccShortInfo info : MiscTestsData.jkccShortInfos)
			toNearBranch.put(info.jkcc, info.jkccNear);

		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			int code = i;
			Instruction instruction = new Instruction();
			instruction.setCode(code);

			Integer nearCodeValue = toNearBranch.get(code);
			if (nearCodeValue == null)
				nearCodeValue = code;

			assertEquals(nearCodeValue, Code.toNearBranch(code));
			instruction.toNearBranch();
			assertEquals(nearCodeValue, instruction.getCode());
		}
	}

	@Test
	void verify_ConditionCode() {
		HashMap<Integer, Integer> toConditionCode = new HashMap<Integer, Integer>();
		for (JccShortInfo info : MiscTestsData.jccShortInfos)
			toConditionCode.put(info.jcc, info.cc);
		for (JccNearInfo info : MiscTestsData.jccNearInfos)
			toConditionCode.put(info.jcc, info.cc);
		for (SetccInfo info : MiscTestsData.setccInfos)
			toConditionCode.put(info.setcc, info.cc);
		for (CmovccInfo info : MiscTestsData.cmovccInfos)
			toConditionCode.put(info.cmovcc, info.cc);
		for (CmpccxaddInfo info : MiscTestsData.cmpccxaddInfos)
			toConditionCode.put(info.cmpccxadd, info.cc);
		for (LoopccInfo info : MiscTestsData.loopccInfos)
			toConditionCode.put(info.loopcc, info.cc);
		for (JkccShortInfo info : MiscTestsData.jkccShortInfos)
			toConditionCode.put(info.jkcc, info.cc);
		for (JkccNearInfo info : MiscTestsData.jkccNearInfos)
			toConditionCode.put(info.jkcc, info.cc);

		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			int code = i;
			Instruction instruction = new Instruction();
			instruction.setCode(code);

			Integer cc = toConditionCode.get(code);
			if (cc == null)
				cc = ConditionCode.NONE;

			assertEquals(cc, Code.conditionCode(code));
			assertEquals(cc, instruction.getConditionCode());
		}
	}

	@Test
	void verify_StringInstr() {
		HashSet<Integer> stringInstr = MiscTestsData.stringInstr;

		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			int code = i;
			Instruction instruction = new Instruction();
			instruction.setCode(code);

			assertEquals(stringInstr.contains(code), Code.isStringInstruction(code));
			assertEquals(Code.isStringInstruction(code), instruction.isStringInstruction());
		}
	}

	@Test
	void instructionInfoExtensions_Encoding_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.encoding(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.encoding(IcedConstants.CODE_ENUM_COUNT));
	}

	@Test
	void instructionInfoExtensions_CpuidFeatures_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.cpuidFeatures(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.cpuidFeatures(IcedConstants.CODE_ENUM_COUNT));
	}

	@Test
	void instructionInfoExtensions_FlowControl_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.flowControl(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.flowControl(IcedConstants.CODE_ENUM_COUNT));
	}

	@Test
	void instructionInfoExtensions_IsPrivileged_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.isPrivileged(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.isPrivileged(IcedConstants.CODE_ENUM_COUNT));
	}

	@Test
	void instructionInfoExtensions_IsStackInstruction_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.isStackInstruction(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.isStackInstruction(IcedConstants.CODE_ENUM_COUNT));
	}

	@Test
	void instructionInfoExtensions_IsSaveRestoreInstruction_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.isSaveRestoreInstruction(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.isSaveRestoreInstruction(IcedConstants.CODE_ENUM_COUNT));
	}

	@Test
	void instructionInfo_GetOpAccess_throws_if_invalid_input() {
		Instruction instr = new Instruction();
		instr.setCode(Code.NOPD);
		InstructionInfo info = new InstructionInfoFactory().getInfo(instr);
		assertThrows(IllegalArgumentException.class, () -> info.getOpAccess(-1));
		assertThrows(IllegalArgumentException.class, () -> info.getOpAccess(IcedConstants.MAX_OP_COUNT));
	}

	@Test
	void memorySizeExtensions_GetInfo_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getInfo(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getInfo(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void memorySizeExtensions_GetSize_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getSize(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getSize(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void memorySizeExtensions_GetElementSize_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementSize(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementSize(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void memorySizeExtensions_GetElementType_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementType(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementType(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void memorySizeExtensions_GetElementTypeInfo_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementTypeInfo(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementTypeInfo(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void memorySizeExtensions_IsSigned_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.isSigned(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.isSigned(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void memorySizeExtensions_IsPacked_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.isPacked(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.isPacked(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void memorySizeExtensions_GetElementCount_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementCount(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> MemorySize.getElementCount(IcedConstants.MEMORY_SIZE_ENUM_COUNT));
	}

	@Test
	void registerExtensions_GetInfo_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getInfo(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getInfo(IcedConstants.REGISTER_ENUM_COUNT));
	}

	@Test
	void registerExtensions_GetBaseRegister_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getBaseRegister(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getBaseRegister(IcedConstants.REGISTER_ENUM_COUNT));
	}

	@Test
	void registerExtensions_GetNumber_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getNumber(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getNumber(IcedConstants.REGISTER_ENUM_COUNT));
	}

	@Test
	void registerExtensions_GetFullRegister_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getFullRegister(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getFullRegister(IcedConstants.REGISTER_ENUM_COUNT));
	}

	@Test
	void registerExtensions_GetFullRegister32_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getFullRegister32(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getFullRegister32(IcedConstants.REGISTER_ENUM_COUNT));
	}

	@Test
	void registerExtensions_GetSize_throws_if_invalid_input() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getSize(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Register.getSize(IcedConstants.REGISTER_ENUM_COUNT));
	}
}
