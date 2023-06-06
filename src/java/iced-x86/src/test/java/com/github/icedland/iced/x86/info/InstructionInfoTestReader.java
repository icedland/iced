// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Locale;

import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.PathUtils;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RflagsBits;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.ToCpuidFeature;
import com.github.icedland.iced.x86.ToDecoderOptions;
import com.github.icedland.iced.x86.ToEncodingKind;
import com.github.icedland.iced.x86.ToFlowControl;
import com.github.icedland.iced.x86.ToMemorySize;
import com.github.icedland.iced.x86.ToRegister;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class InstructionInfoTestReader {
	public static InstructionInfoTestCase[] getTestCases(int bitness, int stackAddressSize) {
		HashMap<String, Integer> toRegister = ToRegister.copy();
		switch (stackAddressSize) {
		case 16:
			toRegister.put(MiscInstrInfoTestConstants.XSP, Register.SP);
			toRegister.put(MiscInstrInfoTestConstants.XBP, Register.BP);
			break;
		case 32:
			toRegister.put(MiscInstrInfoTestConstants.XSP, Register.ESP);
			toRegister.put(MiscInstrInfoTestConstants.XBP, Register.EBP);
			break;
		case 64:
			toRegister.put(MiscInstrInfoTestConstants.XSP, Register.RSP);
			toRegister.put(MiscInstrInfoTestConstants.XBP, Register.RBP);
			break;
		default:
			throw new UnsupportedOperationException();
		}

		for (int i = 0; i < IcedConstants.VMM_COUNT; i++)
			toRegister.put(MiscInstrInfoTestConstants.VMM_PREFIX + Integer.toString(i), IcedConstants.VMM_FIRST + i);

		String filename = PathUtils.getTestTextFilename("InstructionInfo", String.format("InstructionInfoTest_%d.txt", bitness));
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		ArrayList<InstructionInfoTestCase> result = new ArrayList<InstructionInfoTestCase>(lines.size());
		int lineNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			InstructionInfoTestCase testCase;
			try {
				testCase = parseLine(line, bitness, lineNo, toRegister);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
			if (testCase != null)
				result.add(testCase);
		}
		return result.toArray(new InstructionInfoTestCase[0]);
	}

	private static InstructionInfoTestCase parseLine(String line, int bitness, int lineNo, HashMap<String, Integer> toRegister) {
		int expValue = 5;
		if (MiscInstrInfoTestConstants.INSTR_INFO_ELEMS_PER_LINE != expValue)
			throw new UnsupportedOperationException();
		String[] elems = line.split(",", MiscInstrInfoTestConstants.INSTR_INFO_ELEMS_PER_LINE);
		if (elems.length != MiscInstrInfoTestConstants.INSTR_INFO_ELEMS_PER_LINE)
			throw new UnsupportedOperationException(String.format("Expected %d commas", MiscInstrInfoTestConstants.INSTR_INFO_ELEMS_PER_LINE - 1));

		InstructionInfoTestCase testCase = new InstructionInfoTestCase();
		testCase.lineNo = lineNo;
		switch (bitness) {
		case 16:
			testCase.ip = DecoderConstants.DEFAULT_IP16;
			break;
		case 32:
			testCase.ip = DecoderConstants.DEFAULT_IP32;
			break;
		case 64:
			testCase.ip = DecoderConstants.DEFAULT_IP64;
			break;
		default:
			throw new UnsupportedOperationException();
		}

		String hexBytesStr = elems[0].trim();
		// Ignore return value, verify that it's valid hex
		HexUtils.toByteArray(hexBytesStr);
		String codeStr = elems[1].trim();
		if (CodeUtils.isIgnored(codeStr))
			return null;
		int code = ToCode.get(codeStr);
		testCase.encoding = ToEncodingKind.get(elems[2].trim());
		String[] cpuidFeatureStrings = elems[3].trim().split(";");

		int[] cpuidFeatures = new int[cpuidFeatureStrings.length];
		testCase.cpuidFeatures = cpuidFeatures;
		for (int i = 0; i < cpuidFeatures.length; i++)
			cpuidFeatures[i] = ToCpuidFeature.get(cpuidFeatureStrings[i]);

		int options = DecoderOptions.NONE;
		Integer ivalue;
		for (String keyValue : elems[4].split(" ")) {
			if (keyValue.equals(""))
				continue;
			String key, value;
			int index = keyValue.indexOf('=');
			if (index >= 0) {
				key = keyValue.substring(0, index);
				value = keyValue.substring(index + 1);
			}
			else {
				key = keyValue;
				value = "";
			}

			switch (key) {
			case InstructionInfoKeys.IS_PRIVILEGED:
				if (!value.equals(""))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.isPrivileged = true;
				break;

			case InstructionInfoKeys.IS_SAVE_RESTORE_INSTRUCTION:
				if (!value.equals(""))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.isSaveRestoreInstruction = true;
				break;

			case InstructionInfoKeys.IS_STACK_INSTRUCTION:
				testCase.stackPointerIncrement = NumberConverter.toInt32(value);
				testCase.isStackInstruction = true;
				break;

			case InstructionInfoKeys.IS_SPECIAL:
				if (!value.equals(""))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.isSpecial = true;
				break;

			case InstructionInfoKeys.RFLAGS_READ:
				ivalue = parseRflags(value, testCase.rflagsRead);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.rflagsRead = ivalue;
				break;

			case InstructionInfoKeys.RFLAGS_UNDEFINED:
				ivalue = parseRflags(value, testCase.rflagsUndefined);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.rflagsUndefined = ivalue;
				break;

			case InstructionInfoKeys.RFLAGS_WRITTEN:
				ivalue = parseRflags(value, testCase.rflagsWritten);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.rflagsWritten = ivalue;
				break;

			case InstructionInfoKeys.RFLAGS_CLEARED:
				ivalue = parseRflags(value, testCase.rflagsCleared);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.rflagsCleared = ivalue;
				break;

			case InstructionInfoKeys.RFLAGS_SET:
				ivalue = parseRflags(value, testCase.rflagsSet);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.rflagsSet = ivalue;
				break;

			case InstructionInfoKeys.FLOW_CONTROL:
				testCase.flowControl = ToFlowControl.get(value);
				break;

			case InstructionInfoKeys.OP0_ACCESS:
				ivalue = InstrInfoDicts.toOpAccess.get(value);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.op0Access = ivalue;
				break;

			case InstructionInfoKeys.OP1_ACCESS:
				ivalue = InstrInfoDicts.toOpAccess.get(value);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.op1Access = ivalue;
				break;

			case InstructionInfoKeys.OP2_ACCESS:
				ivalue = InstrInfoDicts.toOpAccess.get(value);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.op2Access = ivalue;
				break;

			case InstructionInfoKeys.OP3_ACCESS:
				ivalue = InstrInfoDicts.toOpAccess.get(value);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.op3Access = ivalue;
				break;

			case InstructionInfoKeys.OP4_ACCESS:
				ivalue = InstrInfoDicts.toOpAccess.get(value);
				if (ivalue == null)
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.op4Access = ivalue;
				break;

			case InstructionInfoKeys.READ_REGISTER:
				if (!AddRegisters(toRegister, value, OpAccess.READ, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.COND_READ_REGISTER:
				if (!AddRegisters(toRegister, value, OpAccess.COND_READ, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.WRITE_REGISTER:
				if (!AddRegisters(toRegister, value, OpAccess.WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.COND_WRITE_REGISTER:
				if (!AddRegisters(toRegister, value, OpAccess.COND_WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.READ_WRITE_REGISTER:
				if (!AddRegisters(toRegister, value, OpAccess.READ_WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.READ_COND_WRITE_REGISTER:
				if (!AddRegisters(toRegister, value, OpAccess.READ_COND_WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.READ_MEMORY:
				if (!addMemory(bitness, toRegister, value, OpAccess.READ, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.COND_READ_MEMORY:
				if (!addMemory(bitness, toRegister, value, OpAccess.COND_READ, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.READ_WRITE_MEMORY:
				if (!addMemory(bitness, toRegister, value, OpAccess.READ_WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.READ_COND_WRITE_MEMORY:
				if (!addMemory(bitness, toRegister, value, OpAccess.READ_COND_WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.WRITE_MEMORY:
				if (!addMemory(bitness, toRegister, value, OpAccess.WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.COND_WRITE_MEMORY:
				if (!addMemory(bitness, toRegister, value, OpAccess.COND_WRITE, testCase))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				break;

			case InstructionInfoKeys.DECODER_OPTIONS:
				options = tryParseDecoderOptions(value.split(";"), options);
				break;

			case InstructionInfoKeys.FPU_TOP_INCREMENT:
				testCase.fpuTopIncrement = NumberConverter.toInt32(value);
				testCase.fpuWritesTop = true;
				break;

			case InstructionInfoKeys.FPU_CONDITIONAL_TOP:
				if (!value.equals(""))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.fpuConditionalTop = true;
				break;

			case InstructionInfoKeys.FPU_WRITES_TOP:
				if (!value.equals(""))
					throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
				testCase.fpuWritesTop = true;
				break;

			default:
				throw new UnsupportedOperationException(String.format("Invalid key-value value, '%s'", keyValue));
			}
		}

		testCase.hexBytes = hexBytesStr;
		testCase.code = code;
		testCase.options = options;
		return testCase;
	}

	private static int tryParseDecoderOptions(String[] stringOptions, int options) {
		for (String opt : stringOptions) {
			options |= ToDecoderOptions.get(opt);
		}
		return options;
	}

	private static boolean addMemory(int bitness, HashMap<String, Integer> toRegister, String value, int access, InstructionInfoTestCase testCase) {
		String[] elems = value.split(";");
		if (elems.length != 2)
			return false;
		String expr = elems[0].trim();
		int memorySize = ToMemorySize.get(elems[1].trim());

		ParsedMemExpr mem = tryParseMemExpr(toRegister, expr, bitness);
		if (mem == null)
			return false;

		switch (mem.addressSize) {
		case CodeSize.CODE16:
			if (!(-0x8000 <= mem.displ && mem.displ <= 0x7FFF) && Long.compareUnsigned(mem.displ, 0xFFFF) > 0)
				return false;
			mem.displ = mem.displ & 0xFFFF;
			break;

		case CodeSize.CODE32:
			if (!(-0x8000_0000 <= mem.displ && mem.displ <= 0x7FFF_FFFF) && Long.compareUnsigned(mem.displ, 0xFFFF_FFFFL) > 0)
				return false;
			mem.displ = mem.displ & 0xFFFF_FFFFL;
			break;

		case CodeSize.CODE64:
			break;

		default:
			throw new UnsupportedOperationException();
		}

		if (access != OpAccess.NO_MEM_ACCESS)
			testCase.usedMemory.add(
					new UsedMemory(mem.segReg, mem.baseReg, mem.indexReg, mem.scale, mem.displ, memorySize, access, mem.addressSize, mem.vsibSize));

		return true;
	}

	static class ParsedMemExpr {
		int segReg = Register.NONE;
		int baseReg = Register.NONE;
		int indexReg = Register.NONE;
		int scale = 1;
		long displ = 0;
		int addressSize = CodeSize.UNKNOWN;
		int vsibSize = 0;
	}

	static ParsedMemExpr tryParseMemExpr(HashMap<String, Integer> toRegister, String value, int bitness) {
		ParsedMemExpr mem = new ParsedMemExpr();

		String[] memArgs = value.split("\\|");
		value = memArgs[0];
		for (int i = 1; i < memArgs.length; i++) {
			String option = memArgs[i];
			switch (option) {
			case MiscInstrInfoTestConstants.MEM_SIZE_OPTION_ADDR16:
				mem.addressSize = CodeSize.CODE16;
				break;
			case MiscInstrInfoTestConstants.MEM_SIZE_OPTION_ADDR32:
				mem.addressSize = CodeSize.CODE32;
				break;
			case MiscInstrInfoTestConstants.MEM_SIZE_OPTION_ADDR64:
				mem.addressSize = CodeSize.CODE64;
				break;
			case MiscInstrInfoTestConstants.MEM_SIZE_OPTION_VSIB32:
				mem.vsibSize = 4;
				break;
			case MiscInstrInfoTestConstants.MEM_SIZE_OPTION_VSIB64:
				mem.vsibSize = 8;
				break;
			default:
				return null;
			}
		}

		boolean hasBase = false;
		Integer ivalue;
		for (String s : value.split("\\+")) {
			boolean isIndex = hasBase;
			int segIndex = s.indexOf(":");
			if (segIndex >= 0) {
				String segRegString = s.substring(0, segIndex);
				s = s.substring(segIndex + 1);
				ivalue = toRegister.get(segRegString);
				if (ivalue == null)
					return null;
				mem.segReg = ivalue;
				if (!(Register.ES <= mem.segReg && mem.segReg <= Register.GS))
					return null;
			}
			if (s.indexOf('*') >= 0) {
				if (s.endsWith("*1"))
					mem.scale = 1;
				else if (s.endsWith("*2"))
					mem.scale = 2;
				else if (s.endsWith("*4"))
					mem.scale = 4;
				else if (s.endsWith("*8"))
					mem.scale = 8;
				else
					return null;
				s = s.substring(0, s.length() - 2);
				isIndex = true;
			}
			ivalue = toRegister.get(s);
			if (ivalue != null) {
				if (isIndex)
					mem.indexReg = ivalue;
				else {
					mem.baseReg = ivalue;
					hasBase = true;
				}
			}
			else
				mem.displ = NumberConverter.toUInt64(s);
		}

		if (mem.addressSize == CodeSize.UNKNOWN) {
			int reg = mem.baseReg != Register.NONE ? mem.baseReg : mem.indexReg;
			if (Register.isGPR16(reg))
				mem.addressSize = CodeSize.CODE16;
			else if (Register.isGPR32(reg))
				mem.addressSize = CodeSize.CODE32;
			else if (Register.isGPR64(reg))
				mem.addressSize = CodeSize.CODE64;
		}
		if (mem.addressSize == CodeSize.UNKNOWN) {
			switch (bitness) {
			case 16:
				mem.addressSize = CodeSize.CODE16;
				break;
			case 32:
				mem.addressSize = CodeSize.CODE32;
				break;
			case 64:
				mem.addressSize = CodeSize.CODE64;
				break;
			default:
				throw new UnsupportedOperationException();
			}
		}
		if (mem.vsibSize == 0 && Register.isVectorRegister(mem.indexReg))
			return null;
		if (mem.vsibSize != 0 && !Register.isVectorRegister(mem.indexReg))
			return null;

		if (mem.segReg == Register.NONE)
			return null;
		return mem;
	}

	static String[] trySplit(String value, char sep) {
		int index = value.indexOf(sep);
		if (index >= 0) {
			String left = value.substring(0, index).trim();
			String right = value.substring(index + 1).trim();
			return new String[] { left, right };
		}
		else
			return null;
	}

	private static Integer tryGetRegister(HashMap<String, Integer> toRegister, String regString, int encoding, int access) {
		Integer register = toRegister.get(regString);
		if (register == null)
			return null;

		if (encoding != EncodingKind.LEGACY && encoding != EncodingKind.D3NOW) {
			switch (access) {
			case OpAccess.NONE:
			case OpAccess.READ:
			case OpAccess.NO_MEM_ACCESS:
			case OpAccess.COND_READ:
				break;

			case OpAccess.WRITE:
			case OpAccess.COND_WRITE:
			case OpAccess.READ_WRITE:
			case OpAccess.READ_COND_WRITE:
				if (Register.XMM0 <= register && register <= IcedConstants.VMM_LAST
						&& !regString.toLowerCase(Locale.ROOT).startsWith(MiscInstrInfoTestConstants.VMM_PREFIX.toLowerCase(Locale.ROOT)))
					throw new UnsupportedOperationException(String.format("Register %s is written (%d) but %s pseudo register should be used instead",
							regString, access, MiscInstrInfoTestConstants.VMM_PREFIX));
				break;

			default:
				throw new UnsupportedOperationException();
			}
		}

		return register;
	}

	private static boolean AddRegisters(HashMap<String, Integer> toRegister, String value, int access, InstructionInfoTestCase testCase) {
		for (String regString : value.split(";")) {
			if (regString.equals(""))
				continue;
			regString = regString.trim();
			String[] split = trySplit(regString, '-');
			if (split != null) {
				Integer firstReg = tryGetRegister(toRegister, split[0], testCase.encoding, access);
				if (firstReg == null)
					return false;
				Integer lastReg = tryGetRegister(toRegister, split[1], testCase.encoding, access);
				if (lastReg == null)
					return false;
				if (lastReg < firstReg)
					throw new UnsupportedOperationException(String.format("Invalid register range: %s", regString));
				for (int reg = firstReg.intValue(); reg <= lastReg.intValue(); reg++)
					testCase.usedRegisters.add(new UsedRegister(reg, access));
			}
			else {
				Integer register = tryGetRegister(toRegister, regString, testCase.encoding, access);
				if (register == null)
					return false;
				testCase.usedRegisters.add(new UsedRegister(register, access));
			}
		}
		return true;
	}

	static Integer parseRflags(String value, int rflags) {
		for (int i = 0; i < value.length(); i++) {
			switch (value.charAt(i)) {
			case RflagsBitsConstants.AF:
				rflags |= RflagsBits.AF;
				break;
			case RflagsBitsConstants.CF:
				rflags |= RflagsBits.CF;
				break;
			case RflagsBitsConstants.OF:
				rflags |= RflagsBits.OF;
				break;
			case RflagsBitsConstants.PF:
				rflags |= RflagsBits.PF;
				break;
			case RflagsBitsConstants.SF:
				rflags |= RflagsBits.SF;
				break;
			case RflagsBitsConstants.ZF:
				rflags |= RflagsBits.ZF;
				break;
			case RflagsBitsConstants.IF:
				rflags |= RflagsBits.IF;
				break;
			case RflagsBitsConstants.DF:
				rflags |= RflagsBits.DF;
				break;
			case RflagsBitsConstants.AC:
				rflags |= RflagsBits.AC;
				break;
			case RflagsBitsConstants.C0:
				rflags |= RflagsBits.C0;
				break;
			case RflagsBitsConstants.C1:
				rflags |= RflagsBits.C1;
				break;
			case RflagsBitsConstants.C2:
				rflags |= RflagsBits.C2;
				break;
			case RflagsBitsConstants.C3:
				rflags |= RflagsBits.C3;
				break;
			case RflagsBitsConstants.UIF:
				rflags |= RflagsBits.UIF;
				break;
			default:
				return null;
			}
		}

		return rflags;
	}
}
