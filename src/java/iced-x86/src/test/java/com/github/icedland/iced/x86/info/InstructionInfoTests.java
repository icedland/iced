// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;
import java.util.HashSet;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.FpuStackIncrementInfo;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RflagsBits;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.MvexInfo;

final class InstructionInfoTests {
	@ParameterizedTest
	@MethodSource("test16_InstructionInfo_Data")
	void test16_InstructionInfo(InstructionInfoTestCase testCase) {
		testInstructionInfo(16, testCase);
	}

	static Iterable<Arguments> test16_InstructionInfo_Data() {
		return getTestCases(16);
	}

	@ParameterizedTest
	@MethodSource("test32_InstructionInfo_Data")
	void test32_InstructionInfo(InstructionInfoTestCase testCase) {
		testInstructionInfo(32, testCase);
	}

	static Iterable<Arguments> test32_InstructionInfo_Data() {
		return getTestCases(32);
	}

	@ParameterizedTest
	@MethodSource("test64_InstructionInfo_Data")
	void test64_InstructionInfo(InstructionInfoTestCase testCase) {
		testInstructionInfo(64, testCase);
	}

	static Iterable<Arguments> test64_InstructionInfo_Data() {
		return getTestCases(64);
	}

	private <T> ArrayList<T> toArrayList(Iterable<T> iterable) {
		ArrayList<T> result = new ArrayList<T>();
		for (T value : iterable)
			result.add(value);
		return result;
	}

	private void testInstructionInfo(int bitness, InstructionInfoTestCase testCase) {
		byte[] codeBytes = HexUtils.toByteArray(testCase.hexBytes);
		Instruction instruction;
		if (testCase.isSpecial) {
			if (bitness == 16 && testCase.code == Code.POPW_CS && testCase.hexBytes.equals("0F")) {
				instruction = new Instruction();
				instruction.setCode(Code.POPW_CS);
				instruction.setOp0Kind(OpKind.REGISTER);
				instruction.setOp0Register(Register.CS);
				instruction.setCodeSize(CodeSize.CODE16);
				instruction.setLength(1);
			}
			else if (testCase.code <= Code.DECLAREQWORD) {
				instruction = new Instruction();
				instruction.setCode(testCase.code);
				instruction.setDeclareDataCount(1);
				assertEquals(64, bitness);
				instruction.setCodeSize(CodeSize.CODE64);
				switch (testCase.code) {
				case Code.DECLAREBYTE:
					assertEquals("66", testCase.hexBytes);
					instruction.setDeclareByteValue(0, 0x66);
					break;
				case Code.DECLAREWORD:
					assertEquals("6644", testCase.hexBytes);
					instruction.setDeclareWordValue(0, 0x4466);
					break;
				case Code.DECLAREDWORD:
					assertEquals("664422EE", testCase.hexBytes);
					instruction.setDeclareDwordValue(0, 0xEE224466);
					break;
				case Code.DECLAREQWORD:
					assertEquals("664422EE12345678", testCase.hexBytes);
					instruction.setDeclareQwordValue(0, 0x78563412EE224466L);
					break;
				default: throw new UnsupportedOperationException();
				}
			}
			else if (testCase.code == Code.ZERO_BYTES) {
				instruction = new Instruction();
				instruction.setCode(testCase.code);
				assertEquals(64, bitness);
				instruction.setCodeSize(CodeSize.CODE64);
				assertEquals("", testCase.hexBytes);
			}
			else {
				Decoder decoder = createDecoder(bitness, codeBytes, testCase.ip, testCase.options);
				instruction = decoder.decode();
				if (codeBytes.length > 1 && (codeBytes[0] & 0xFF) == 0x9B && instruction.getLength() == 1) {
					instruction = decoder.decode();
					int code2;
					switch (instruction.getCode()) {
					case Code.FNSTENV_M14BYTE:
						code2 = Code.FSTENV_M14BYTE;
						break;
					case Code.FNSTENV_M28BYTE:
						code2 = Code.FSTENV_M28BYTE;
						break;
					case Code.FNSTCW_M2BYTE:
						code2 = Code.FSTCW_M2BYTE;
						break;
					case Code.FNENI:
						code2 = Code.FENI;
						break;
					case Code.FNDISI:
						code2 = Code.FDISI;
						break;
					case Code.FNCLEX:
						code2 = Code.FCLEX;
						break;
					case Code.FNINIT:
						code2 = Code.FINIT;
						break;
					case Code.FNSETPM:
						code2 = Code.FSETPM;
						break;
					case Code.FNSAVE_M94BYTE:
						code2 = Code.FSAVE_M94BYTE;
						break;
					case Code.FNSAVE_M108BYTE:
						code2 = Code.FSAVE_M108BYTE;
						break;
					case Code.FNSTSW_M2BYTE:
						code2 = Code.FSTSW_M2BYTE;
						break;
					case Code.FNSTSW_AX:
						code2 = Code.FSTSW_AX;
						break;
					case Code.FNSTDW_AX:
						code2 = Code.FSTDW_AX;
						break;
					case Code.FNSTSG_AX:
						code2 = Code.FSTSG_AX;
						break;
					default:
						throw new UnsupportedOperationException();
					}
					instruction.setCode(code2);
				}
				else
					throw new UnsupportedOperationException();
			}
		}
		else {
			Decoder decoder = createDecoder(bitness, codeBytes, testCase.ip, testCase.options);
			instruction = decoder.decode();
		}
		assertEquals(testCase.code, instruction.getCode());

		assertEquals(testCase.stackPointerIncrement, instruction.getStackPointerIncrement());

		InstructionInfo info = new InstructionInfoFactory().getInfo(instruction);
		assertEquals(testCase.op0Access, info.getOp0Access());
		assertEquals(testCase.op1Access, info.getOp1Access());
		assertEquals(testCase.op2Access, info.getOp2Access());
		assertEquals(testCase.op3Access, info.getOp3Access());
		assertEquals(testCase.op4Access, info.getOp4Access());
		FpuStackIncrementInfo fpuInfo = instruction.getFpuStackIncrementInfo();
		assertEquals(testCase.fpuTopIncrement, fpuInfo.increment);
		assertEquals(testCase.fpuConditionalTop, fpuInfo.conditional);
		assertEquals(testCase.fpuWritesTop, fpuInfo.writesTop);
		assertEquals(
			new HashSet<UsedMemory>(testCase.usedMemory),
			new HashSet<UsedMemory>(toArrayList(info.getUsedMemory())));
		assertEquals(
			new HashSet<UsedRegister>(toArrayList(getUsedRegisters(testCase.usedRegisters))),
			new HashSet<UsedRegister>(toArrayList(getUsedRegisters(info.getUsedRegisters()))));

		int expValue = 5;
		if (IcedConstants.MAX_OP_COUNT != expValue)
			throw new UnsupportedOperationException();
		assert instruction.getOpCount() <= IcedConstants.MAX_OP_COUNT : instruction.getOpCount();
		for (int i = 0; i < instruction.getOpCount(); i++) {
			switch (i) {
			case 0:
				assertEquals(testCase.op0Access, info.getOpAccess(i));
				break;

			case 1:
				assertEquals(testCase.op1Access, info.getOpAccess(i));
				break;

			case 2:
				assertEquals(testCase.op2Access, info.getOpAccess(i));
				break;

			case 3:
				assertEquals(testCase.op3Access, info.getOpAccess(i));
				break;

			case 4:
				assertEquals(testCase.op4Access, info.getOpAccess(i));
				break;

			default:
				throw new UnsupportedOperationException();
			}
		}
		for (int i = instruction.getOpCount(); i < IcedConstants.MAX_OP_COUNT; i++)
			assertEquals(OpAccess.NONE, info.getOpAccess(i));

		InstructionInfo info2 = new InstructionInfoFactory().getInfo(instruction, InstructionInfoOptions.NONE);
		checkEqual(info, info2, true, true);
		info2 = new InstructionInfoFactory().getInfo(instruction, InstructionInfoOptions.NO_MEMORY_USAGE);
		checkEqual(info, info2, true, false);
		info2 = new InstructionInfoFactory().getInfo(instruction, InstructionInfoOptions.NO_REGISTER_USAGE);
		checkEqual(info, info2, false, true);
		info2 = new InstructionInfoFactory().getInfo(instruction, InstructionInfoOptions.NO_REGISTER_USAGE | InstructionInfoOptions.NO_MEMORY_USAGE);
		checkEqual(info, info2, false, false);

		assertEquals(testCase.encoding, Code.encoding(instruction.getCode()));
		assertEquals(Code.toOpCode(testCase.code).getEncoding(), testCase.encoding);
		assertArrayEquals(testCase.cpuidFeatures, Code.cpuidFeatures(instruction.getCode()));
		assertEquals(testCase.flowControl, Code.flowControl(instruction.getCode()));
		assertEquals(testCase.isPrivileged, Code.isPrivileged(instruction.getCode()));
		assertEquals(testCase.isStackInstruction, Code.isStackInstruction(instruction.getCode()));
		assertEquals(testCase.isSaveRestoreInstruction, Code.isSaveRestoreInstruction(instruction.getCode()));

		assertEquals(testCase.encoding, instruction.getEncoding());
		if (instruction.getEncoding() == EncodingKind.MVEX)
			assertTrue(MvexInfo.isMvex(instruction.getCode()));
		else
			assertFalse(MvexInfo.isMvex(instruction.getCode()));
		assertArrayEquals(testCase.cpuidFeatures, instruction.getCpuidFeatures());
		assertEquals(testCase.flowControl, instruction.getFlowControl());
		assertEquals(testCase.isPrivileged, instruction.isPrivileged());
		assertEquals(testCase.isStackInstruction, instruction.isStackInstruction());
		assertEquals(testCase.isSaveRestoreInstruction, instruction.isSaveRestoreInstruction());
		assertEquals(testCase.rflagsRead, instruction.getRflagsRead());
		assertEquals(testCase.rflagsWritten, instruction.getRflagsWritten());
		assertEquals(testCase.rflagsCleared, instruction.getRflagsCleared());
		assertEquals(testCase.rflagsSet, instruction.getRflagsSet());
		assertEquals(testCase.rflagsUndefined, instruction.getRflagsUndefined());
		assertEquals(testCase.rflagsWritten | testCase.rflagsCleared | testCase.rflagsSet | testCase.rflagsUndefined, instruction.getRflagsModified());

		assertEquals(RflagsBits.NONE, instruction.getRflagsWritten() & (instruction.getRflagsCleared() | instruction.getRflagsSet() | instruction.getRflagsUndefined()));
		assertEquals(RflagsBits.NONE, instruction.getRflagsCleared() & (instruction.getRflagsWritten() | instruction.getRflagsSet() | instruction.getRflagsUndefined()));
		assertEquals(RflagsBits.NONE, instruction.getRflagsSet() & (instruction.getRflagsWritten() | instruction.getRflagsCleared() | instruction.getRflagsUndefined()));
		assertEquals(RflagsBits.NONE, instruction.getRflagsUndefined() & (instruction.getRflagsWritten() | instruction.getRflagsCleared() | instruction.getRflagsSet()));
	}

	private void checkEqual(InstructionInfo info1, InstructionInfo info2, boolean hasRegs2, boolean hasMem2) {
		if (hasRegs2)
			assertEquals(toArrayList(info1.getUsedRegisters()), info2.getUsedRegisters());
		else
			assertEquals(toArrayList(info2.getUsedRegisters()), new ArrayList<>());
		if (hasMem2)
			assertEquals(toArrayList(info1.getUsedMemory()), info2.getUsedMemory());
		else
			assertEquals(toArrayList(info2.getUsedMemory()), new ArrayList<>());
		assertEquals(info1.getOp0Access(), info2.getOp0Access());
		assertEquals(info1.getOp1Access(), info2.getOp1Access());
		assertEquals(info1.getOp2Access(), info2.getOp2Access());
		assertEquals(info1.getOp3Access(), info2.getOp3Access());
		assertEquals(info1.getOp4Access(), info2.getOp4Access());
	}

	private Iterable<UsedRegister> getUsedRegisters(Iterable<UsedRegister> usedRegisterIterator) {
		ArrayList<Integer> read = new ArrayList<Integer>();
		ArrayList<Integer> write = new ArrayList<Integer>();
		ArrayList<Integer> condRead = new ArrayList<Integer>();
		ArrayList<Integer> condWrite = new ArrayList<Integer>();

		for (UsedRegister info : usedRegisterIterator) {
			switch (info.getAccess()) {
			case OpAccess.READ:
				read.add(info.getRegister());
				break;

			case OpAccess.COND_READ:
				condRead.add(info.getRegister());
				break;

			case OpAccess.WRITE:
				write.add(info.getRegister());
				break;

			case OpAccess.COND_WRITE:
				condWrite.add(info.getRegister());
				break;

			case OpAccess.READ_WRITE:
				read.add(info.getRegister());
				write.add(info.getRegister());
				break;

			case OpAccess.READ_COND_WRITE:
				read.add(info.getRegister());
				condWrite.add(info.getRegister());
				break;

			case OpAccess.NONE:
			case OpAccess.NO_MEM_ACCESS:
			default:
				throw new UnsupportedOperationException();
			}
		}

		ArrayList<UsedRegister> result = new ArrayList<UsedRegister>();
		for (int reg : getRegisters(read))
			result.add(new UsedRegister(reg, OpAccess.READ));
		for (int reg : getRegisters(write))
			result.add(new UsedRegister(reg, OpAccess.WRITE));
		for (int reg : getRegisters(condRead))
			result.add(new UsedRegister(reg, OpAccess.COND_READ));
		for (int reg : getRegisters(condWrite))
			result.add(new UsedRegister(reg, OpAccess.COND_WRITE));
		return result;
	}

	private Iterable<Integer> getRegisters(ArrayList<Integer> regs) {
		if (regs.size() <= 1)
			return regs;

		regs.sort(InstructionInfoTests::registerSorter);

		HashSet<Integer> hash = new HashSet<Integer>();
		int index;
		for (int reg : regs) {
			if (Register.EAX <= reg && reg <= Register.R15D) {
				index = reg - Register.EAX;
				if (hash.contains(Register.RAX + index))
					continue;
			}
			else if (Register.AX <= reg && reg <= Register.R15W) {
				index = reg - Register.AX;
				if (hash.contains(Register.RAX + index))
					continue;
				if (hash.contains(Register.EAX + index))
					continue;
			}
			else if (Register.AL <= reg && reg <= Register.R15L) {
				index = reg - Register.AL;
				if (Register.AH <= reg && reg <= Register.BH)
					index -= 4;
				if (hash.contains(Register.RAX + index))
					continue;
				if (hash.contains(Register.EAX + index))
					continue;
				if (hash.contains(Register.AX + index))
					continue;
			}
			else if (Register.YMM0 <= reg && reg <= IcedConstants.YMM_LAST) {
				index = reg - Register.YMM0;
				if (hash.contains(Register.ZMM0 + index))
					continue;
			}
			else if (Register.XMM0 <= reg && reg <= IcedConstants.XMM_LAST) {
				index = reg - Register.XMM0;
				if (hash.contains(Register.ZMM0 + index))
					continue;
				if (hash.contains(Register.YMM0 + index))
					continue;
			}
			hash.add(reg);
		}

		for (LowRegs info : lowRegs) {
			if (hash.contains(info.rl) && hash.contains(info.rh)) {
				hash.remove(info.rl);
				hash.remove(info.rh);
				hash.add(info.rx);
			}
		}

		return hash;
	}
	static class LowRegs {
		int rl, rh, rx;
		LowRegs(int rl, int rh, int rx) {
			this.rl = rl;
			this.rh = rh;
			this.rx = rx;
		}
	}
	static final LowRegs[] lowRegs = new LowRegs[] {
		new LowRegs(Register.AL, Register.AH, Register.AX),
		new LowRegs(Register.CL, Register.CH, Register.CX),
		new LowRegs(Register.DL, Register.DH, Register.DX),
		new LowRegs(Register.BL, Register.BH, Register.BX),
	};

	private static int registerSorter(int x, int y) {
		int c = getRegisterGroupOrder(x) - getRegisterGroupOrder(y);
		if (c != 0)
			return c;
		return x - y;
	}

	private static int getRegisterGroupOrder(int reg) {
		if (Register.RAX <= reg && reg <= Register.R15)
			return 0;
		if (Register.EAX <= reg && reg <= Register.R15D)
			return 1;
		if (Register.AX <= reg && reg <= Register.R15W)
			return 2;
		if (Register.AL <= reg && reg <= Register.R15L)
			return 3;

		if (Register.ZMM0 <= reg && reg <= IcedConstants.ZMM_LAST)
			return 4;
		if (Register.YMM0 <= reg && reg <= IcedConstants.YMM_LAST)
			return 5;
		if (Register.XMM0 <= reg && reg <= IcedConstants.XMM_LAST)
			return 6;

		return -1;
	}

	private Decoder createDecoder(int bitness, byte[] codeBytes, long ip, int options) {
		ByteArrayCodeReader codeReader = new ByteArrayCodeReader(codeBytes);
		Decoder decoder = new Decoder(bitness, codeReader, options);
		decoder.setIP(ip);
		assertEquals(bitness, decoder.getBitness());
		return decoder;
	}

	private static Iterable<Arguments> getTestCases(int bitness) {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (InstructionInfoTestCase tc : InstructionInfoTestReader.getTestCases(bitness, bitness))
			result.add(Arguments.of(tc));
		return result;
	}
}
