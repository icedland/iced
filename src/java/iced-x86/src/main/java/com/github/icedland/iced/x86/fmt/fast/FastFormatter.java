// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.fast;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MvexConvFn;
import com.github.icedland.iced.x86.MvexRegMemConv;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.fmt.SymbolFlags;
import com.github.icedland.iced.x86.fmt.SymbolResolver;
import com.github.icedland.iced.x86.fmt.SymbolResult;
import com.github.icedland.iced.x86.fmt.TextInfo;
import com.github.icedland.iced.x86.fmt.TextPart;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.fmt.FormatterString;
import com.github.icedland.iced.x86.internal.fmt.PseudoOpsKind;

/**
 * Fast formatter with less formatting options and with a masm-like syntax.
 * Use it if formatting speed is more important than being able to re-assemble formatted instructions.
 */
@SuppressWarnings("deprecation") // Our internal classes are deprecated so the users get warnings if they use them
public final class FastFormatter {
	private final FastFormatterOptions options;
	private final SymbolResolver symbolResolver;
	private final FormatterString[] allRegisters;
	private final String[] codeMnemonics;
	private final byte[] codeFlags;
	private final String[] allMemorySizes;
	private final String[] rcStrings;
	private final String[] rcSaeStrings;
	private final String[] scaleNumbers;
	private final String[] mvexRegMemConsts32;
	private final String[] mvexRegMemConsts64;

	private static final boolean SHOW_USELESS_PREFIXES = true;

	private static final String[] s_rcStrings = new String[] {
		"{rn}",
		"{rd}",
		"{ru}",
		"{rz}",
	};
	private static final String[] s_rcSaeStrings = new String[] {
		"{rn-sae}",
		"{rd-sae}",
		"{ru-sae}",
		"{rz-sae}",
	};
	private static final String[] s_scaleNumbers = new String[] {
		"*1", "*2", "*4", "*8",
	};
	private static final String[] s_mvexRegMemConsts32 = new String[] {
		"",
		"",
		"{cdab}",
		"{badc}",
		"{dacb}",
		"{aaaa}",
		"{bbbb}",
		"{cccc}",
		"{dddd}",
		"",
		"{1to16}",
		"{4to16}",
		"{float16}",
		"{uint8}",
		"{sint8}",
		"{uint16}",
		"{sint16}",
	};
	private static final String[] s_mvexRegMemConsts64 = new String[] {
		"",
		"",
		"{cdab}",
		"{badc}",
		"{dacb}",
		"{aaaa}",
		"{bbbb}",
		"{cccc}",
		"{dddd}",
		"",
		"{1to8}",
		"{4to8}",
		"{float16}",
		"{uint8}",
		"{sint8}",
		"{uint16}",
		"{sint16}",
	};

	/**
	 * Gets the formatter options
	 */
	public FastFormatterOptions getOptions() {
		return options;
	}

	/**
	 * Constructor
	 */
	public FastFormatter() {
		this(null);
	}

	/**
	 * Constructor
	 *
	 * @param symbolResolver Symbol resolver or null
	 */
	public FastFormatter(SymbolResolver symbolResolver) {
		options = new FastFormatterOptions();
		this.symbolResolver = symbolResolver;
		allRegisters = Registers.allRegisters;
		codeMnemonics = FmtData.mnemonics;
		codeFlags = FmtData.flags;
		allMemorySizes = MemorySizes.allMemorySizes;
		rcStrings = s_rcStrings;
		rcSaeStrings = s_rcSaeStrings;
		scaleNumbers = s_scaleNumbers;
		mvexRegMemConsts32 = s_mvexRegMemConsts32;
		mvexRegMemConsts64 = s_mvexRegMemConsts64;
	}

	/**
	 * Formats the whole instruction: prefixes, mnemonic, operands
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	public void format(Instruction instruction, FastStringOutput output) {
		if (output == null)
			throw new NullPointerException("output");

		int code = instruction.getCode();
		String mnemonic = codeMnemonics[code];
		int flags = codeFlags[code] & 0xFF;
		int opCount = instruction.getOpCount();
		int pseudoOpsNum = flags >>> FastFmtFlags.PSEUDO_OPS_KIND_SHIFT;
		if (pseudoOpsNum != 0 && options.getUsePseudoOps() && instruction.getOpKind(opCount - 1) == OpKind.IMMEDIATE8) {
			int index = instruction.getImmediate8() & 0xFF;
			int pseudoOpKind = pseudoOpsNum - 1;
			if (pseudoOpKind == PseudoOpsKind.VPCMPD6) {
				switch (code) {
				case Code.MVEX_VPCMPUD_KR_K1_ZMM_ZMMMT_IMM8:
					pseudoOpKind = PseudoOpsKind.VPCMPUD6;
					break;
				default:
					break;
				}
			}
			FormatterString[] pseudoOps = com.github.icedland.iced.x86.internal.fmt.FormatterConstants.getPseudoOps(pseudoOpKind);
			if (pseudoOpKind == PseudoOpsKind.PCLMULQDQ || pseudoOpKind == PseudoOpsKind.VPCLMULQDQ) {
				if (index <= 1) {
					// nothing
				}
				else if (index == 0x10)
					index = 2;
				else if (index == 0x11)
					index = 3;
				else
					index = -1;
			}
			if (Integer.compareUnsigned(index, pseudoOps.length) < 0) {
				mnemonic = pseudoOps[index].getLower();
				opCount--;
			}
		}

		int prefixSeg = instruction.getSegmentPrefix();
		boolean hasNoTrackPrefix = prefixSeg == Register.DS && com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isNotrackPrefixBranch(code);
		if (!hasNoTrackPrefix && prefixSeg != Register.NONE && showSegmentPrefix(instruction, opCount)) {
			formatRegister(output, prefixSeg);
			output.append(' ');
		}

		boolean hasXacquirePrefix = false;
		if (instruction.getXacquirePrefix()) {
			output.append("xacquire ");
			hasXacquirePrefix = true;
		}
		if (instruction.getXreleasePrefix()) {
			output.append("xrelease ");
			hasXacquirePrefix = true;
		}
		if (instruction.getLockPrefix())
			output.append("lock ");
		if (hasNoTrackPrefix)
			output.append("notrack ");
		if (!hasXacquirePrefix) {
			if (instruction.getRepePrefix() && (SHOW_USELESS_PREFIXES
					|| com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showRepOrRepePrefix(code, SHOW_USELESS_PREFIXES))) {
				if (com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isRepeOrRepneInstruction(code))
					output.append("repe ");
				else
					output.append("rep ");
			}
			if (instruction.getRepnePrefix()) {
				if ((Code.RETNW_IMM16 <= code && code <= Code.RETNQ) ||
						(Code.CALL_REL16 <= code && code <= Code.JMP_REL32_64) ||
						(Code.CALL_RM16 <= code && code <= Code.CALL_RM64) ||
						(Code.JMP_RM16 <= code && code <= Code.JMP_RM64) ||
						Code.isJccShortOrNear(code)) {
					output.append("bnd ");
				}
				else if (SHOW_USELESS_PREFIXES
						|| com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showRepnePrefix(code, SHOW_USELESS_PREFIXES))
					output.append("repne ");
			}
		}

		output.append(mnemonic);

		boolean isDeclareData;
		int declareDataOpKind;
		if (Integer.compareUnsigned(code - Code.DECLAREBYTE, Code.DECLAREQWORD - Code.DECLAREBYTE) <= 0) {
			opCount = instruction.getDeclareDataCount();
			isDeclareData = true;
			switch (code) {
			case Code.DECLAREBYTE:
				declareDataOpKind = OpKind.IMMEDIATE8;
				break;
			case Code.DECLAREWORD:
				declareDataOpKind = OpKind.IMMEDIATE16;
				break;
			case Code.DECLAREDWORD:
				declareDataOpKind = OpKind.IMMEDIATE32;
				break;
			default:
				assert code == Code.DECLAREQWORD : code;
				declareDataOpKind = OpKind.IMMEDIATE64;
				break;
			}
		}
		else {
			isDeclareData = false;
			declareDataOpKind = OpKind.REGISTER;
		}

		if (opCount > 0) {
			output.append(' ');

			int mvexRmOperand;
			if (MvexInfo.isMvex(instruction.getCode())) {
				assert opCount != 0 : opCount;
				mvexRmOperand = instruction.getOpKind(opCount - 1) == OpKind.IMMEDIATE8 ? opCount - 2 : opCount - 1;
			}
			else
				mvexRmOperand = -1;

			for (int operand = 0; operand < opCount; operand++) {
				if (operand > 0) {
					if (options.getSpaceAfterOperandSeparator())
						output.append(", ");
					else
						output.append(',');
				}

				byte imm8;
				short imm16;
				int imm32;
				long imm64;
				int immSize;
				SymbolResult symbol;
				int opKind = isDeclareData ? declareDataOpKind : instruction.getOpKind(operand);
				switch (opKind) {
				case OpKind.REGISTER:
					formatRegister(output, instruction.getOpRegister(operand));
					break;

				case OpKind.NEAR_BRANCH16:
				case OpKind.NEAR_BRANCH32:
				case OpKind.NEAR_BRANCH64:
					if (opKind == OpKind.NEAR_BRANCH64) {
						immSize = 8;
						imm64 = instruction.getNearBranch64();
					}
					else if (opKind == OpKind.NEAR_BRANCH32) {
						immSize = 4;
						imm64 = instruction.getNearBranch32() & 0xFFFF_FFFFL;
					}
					else {
						immSize = 2;
						imm64 = instruction.getNearBranch16() & 0xFFFF;
					}
					if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, operand, imm64, immSize)) != null)
						writeSymbol(output, imm64, symbol);
					else
						formatNumber(output, imm64);
					break;

				case OpKind.FAR_BRANCH16:
				case OpKind.FAR_BRANCH32:
					if (opKind == OpKind.FAR_BRANCH32) {
						immSize = 4;
						imm64 = instruction.getFarBranch32() & 0xFFFF_FFFFL;
					}
					else {
						immSize = 2;
						imm64 = instruction.getFarBranch16() & 0xFFFF;
					}
					if (symbolResolver != null
							&& (symbol = symbolResolver.getSymbol(instruction, operand, operand, imm64, immSize)) != null) {
						assert operand + 1 == 1 : operand;
						SymbolResult selectorSymbol = symbolResolver.getSymbol(instruction, operand + 1, operand,
								instruction.getFarBranchSelector() & 0xFFFF, 2);
						if (selectorSymbol == null)
							formatNumber(output, instruction.getFarBranchSelector() & 0xFFFF);
						else
							writeSymbol(output, instruction.getFarBranchSelector() & 0xFFFF, selectorSymbol);
						output.append(':');
						writeSymbol(output, imm64, symbol);
					}
					else {
						formatNumber(output, instruction.getFarBranchSelector() & 0xFFFF);
						output.append(':');
						if (opKind == OpKind.FAR_BRANCH32)
							formatNumber(output, instruction.getFarBranch32() & 0xFFFF_FFFFL);
						else
							formatNumber(output, instruction.getFarBranch16() & 0xFFFF);
					}
					break;

				case OpKind.IMMEDIATE8:
				case OpKind.IMMEDIATE8_2ND:
					if (isDeclareData)
						imm8 = instruction.getDeclareByteValue(operand);
					else if (opKind == OpKind.IMMEDIATE8)
						imm8 = instruction.getImmediate8();
					else {
						assert opKind == OpKind.IMMEDIATE8_2ND : opKind;
						imm8 = instruction.getImmediate8_2nd();
					}
					if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, operand, imm8 & 0xFF, 1)) != null) {
						if ((symbol.flags & SymbolFlags.RELATIVE) == 0)
							output.append("offset ");
						writeSymbol(output, imm8 & 0xFF, symbol);
					}
					else
						formatNumber(output, imm8 & 0xFF);
					break;

				case OpKind.IMMEDIATE16:
				case OpKind.IMMEDIATE8TO16:
					if (isDeclareData)
						imm16 = instruction.getDeclareWordValue(operand);
					else if (opKind == OpKind.IMMEDIATE16)
						imm16 = instruction.getImmediate16();
					else {
						assert opKind == OpKind.IMMEDIATE8TO16 : opKind;
						imm16 = instruction.getImmediate8to16();
					}
					if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, operand, imm16 & 0xFFFF, 2)) != null) {
						if ((symbol.flags & SymbolFlags.RELATIVE) == 0)
							output.append("offset ");
						writeSymbol(output, imm16 & 0xFFFF, symbol);
					}
					else
						formatNumber(output, imm16 & 0xFFFF);
					break;

				case OpKind.IMMEDIATE32:
				case OpKind.IMMEDIATE8TO32:
					if (isDeclareData)
						imm32 = instruction.getDeclareDwordValue(operand);
					else if (opKind == OpKind.IMMEDIATE32)
						imm32 = instruction.getImmediate32();
					else {
						assert opKind == OpKind.IMMEDIATE8TO32 : opKind;
						imm32 = instruction.getImmediate8to32();
					}
					if (symbolResolver != null
							&& (symbol = symbolResolver.getSymbol(instruction, operand, operand, (long)imm32 & 0xFFFF_FFFFL, 4)) != null) {
						if ((symbol.flags & SymbolFlags.RELATIVE) == 0)
							output.append("offset ");
						writeSymbol(output, (long)imm32 & 0xFFFF_FFFFL, symbol);
					}
					else
						formatNumber(output, (long)imm32 & 0xFFFF_FFFFL);
					break;

				case OpKind.IMMEDIATE64:
				case OpKind.IMMEDIATE8TO64:
				case OpKind.IMMEDIATE32TO64:
					if (isDeclareData)
						imm64 = instruction.getDeclareQwordValue(operand);
					else if (opKind == OpKind.IMMEDIATE32TO64)
						imm64 = instruction.getImmediate32to64();
					else if (opKind == OpKind.IMMEDIATE8TO64)
						imm64 = instruction.getImmediate8to64();
					else {
						assert opKind == OpKind.IMMEDIATE64 : opKind;
						imm64 = instruction.getImmediate64();
					}
					if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, operand, imm64, 8)) != null) {
						if ((symbol.flags & SymbolFlags.RELATIVE) == 0)
							output.append("offset ");
						writeSymbol(output, imm64, symbol);
					}
					else
						formatNumber(output, imm64);
					break;

				case OpKind.MEMORY_SEG_SI:
					formatMemory(output, instruction, operand, instruction.getMemorySegment(), Register.SI, Register.NONE, 0, 0, 0, 2);
					break;

				case OpKind.MEMORY_SEG_ESI:
					formatMemory(output, instruction, operand, instruction.getMemorySegment(), Register.ESI, Register.NONE, 0, 0, 0, 4);
					break;

				case OpKind.MEMORY_SEG_RSI:
					formatMemory(output, instruction, operand, instruction.getMemorySegment(), Register.RSI, Register.NONE, 0, 0, 0, 8);
					break;

				case OpKind.MEMORY_SEG_DI:
					formatMemory(output, instruction, operand, instruction.getMemorySegment(), Register.DI, Register.NONE, 0, 0, 0, 2);
					break;

				case OpKind.MEMORY_SEG_EDI:
					formatMemory(output, instruction, operand, instruction.getMemorySegment(), Register.EDI, Register.NONE, 0, 0, 0, 4);
					break;

				case OpKind.MEMORY_SEG_RDI:
					formatMemory(output, instruction, operand, instruction.getMemorySegment(), Register.RDI, Register.NONE, 0, 0, 0, 8);
					break;

				case OpKind.MEMORY_ESDI:
					formatMemory(output, instruction, operand, Register.ES, Register.DI, Register.NONE, 0, 0, 0, 2);
					break;

				case OpKind.MEMORY_ESEDI:
					formatMemory(output, instruction, operand, Register.ES, Register.EDI, Register.NONE, 0, 0, 0, 4);
					break;

				case OpKind.MEMORY_ESRDI:
					formatMemory(output, instruction, operand, Register.ES, Register.RDI, Register.NONE, 0, 0, 0, 8);
					break;

				case OpKind.MEMORY:
					int displSize = instruction.getMemoryDisplSize();
					int baseReg = instruction.getMemoryBase();
					int indexReg = instruction.getMemoryIndex();
					int addrSize = com.github.icedland.iced.x86.InternalInstructionUtils.getAddressSizeInBytes(baseReg, indexReg, displSize,
							instruction.getCodeSize());
					long displ;
					if (addrSize == 8)
						displ = instruction.getMemoryDisplacement64();
					else
						displ = instruction.getMemoryDisplacement32() & 0xFFFF_FFFFL;
					if (code == Code.XLAT_M8)
						indexReg = Register.NONE;
					formatMemory(output, instruction, operand, instruction.getMemorySegment(), baseReg, indexReg,
							instruction.getRawMemoryIndexScale(), displSize, displ, addrSize);
					break;

				default:
					throw new UnsupportedOperationException();
				}

				if (operand == 0 && (instruction.hasOpMask() || instruction.getZeroingMasking())) {
					if (instruction.hasOpMask()) {
						output.append('{');
						formatRegister(output, instruction.getOpMask());
						output.append('}');
					}
					if (instruction.getZeroingMasking())
						output.append("{z}");
				}
				if (mvexRmOperand == operand) {
					int conv = instruction.getMvexRegMemConv();
					if (conv != MvexRegMemConv.NONE) {
						if (MvexInfo.getConvFn(instruction.getCode()) != MvexConvFn.NONE) {
							String[] tbl = MvexInfo.isConvFn32(instruction.getCode()) ? mvexRegMemConsts32 : mvexRegMemConsts64;
							String s = tbl[conv];
							if (s.length() != 0)
								output.append(s);
						}
					}
				}
			}
			int rc = instruction.getRoundingControl();
			if (rc != RoundingControl.NONE) {
				if (MvexInfo.isMvex(instruction.getCode()) && !instruction.getSuppressAllExceptions())
					output.append(rcStrings[rc - 1]);
				else
					output.append(rcSaeStrings[rc - 1]);
			}
			else if (instruction.getSuppressAllExceptions())
				output.append("{sae}");
		}
	}

	// Only one caller
	private static boolean showSegmentPrefix(Instruction instruction, int opCount) {
		for (int i = 0; i < opCount; i++) {
			switch (instruction.getOpKind(i)) {
			case OpKind.REGISTER:
			case OpKind.NEAR_BRANCH16:
			case OpKind.NEAR_BRANCH32:
			case OpKind.NEAR_BRANCH64:
			case OpKind.FAR_BRANCH16:
			case OpKind.FAR_BRANCH32:
			case OpKind.IMMEDIATE8:
			case OpKind.IMMEDIATE8_2ND:
			case OpKind.IMMEDIATE16:
			case OpKind.IMMEDIATE32:
			case OpKind.IMMEDIATE64:
			case OpKind.IMMEDIATE8TO16:
			case OpKind.IMMEDIATE8TO32:
			case OpKind.IMMEDIATE8TO64:
			case OpKind.IMMEDIATE32TO64:
			case OpKind.MEMORY_ESDI:
			case OpKind.MEMORY_ESEDI:
			case OpKind.MEMORY_ESRDI:
				break;

			case OpKind.MEMORY_SEG_SI:
			case OpKind.MEMORY_SEG_ESI:
			case OpKind.MEMORY_SEG_RSI:
			case OpKind.MEMORY_SEG_DI:
			case OpKind.MEMORY_SEG_EDI:
			case OpKind.MEMORY_SEG_RDI:
			case OpKind.MEMORY:
				return false;

			default:
				throw new UnsupportedOperationException();
			}
		}
		return SHOW_USELESS_PREFIXES;
	}

	private void formatRegister(FastStringOutput output, int register) {
		output.append(allRegisters[register].getLower());
	}

	private void formatNumber(FastStringOutput output, long value) {
		boolean useHexPrefix = options.getUseHexPrefix();
		if (useHexPrefix)
			output.append("0x");

		int shift = 0;
		for (long tmp = value;;) {
			shift += 4;
			tmp >>>= 4;
			if (tmp == 0)
				break;
		}

		if (!useHexPrefix && (int)((value >>> (shift - 4)) & 0xF) > 9)
			output.append('0');
		String hexDigits = options.getUppercaseHex() ? "0123456789ABCDEF" : "0123456789abcdef";
		for (;;) {
			shift -= 4;
			int digit = (int)(value >>> shift) & 0xF;
			output.append(hexDigits.charAt(digit));
			if (shift == 0)
				break;
		}

		if (!useHexPrefix)
			output.append('h');
	}

	private void writeSymbol(FastStringOutput output, long address, SymbolResult symbol) {
		writeSymbol(output, address, symbol, true);
	}

	private void writeSymbol(FastStringOutput output, long address, SymbolResult symbol, boolean writeMinusIfSigned) {
		long displ = address - symbol.address;
		if ((symbol.flags & SymbolFlags.SIGNED) != 0) {
			if (writeMinusIfSigned)
				output.append('-');
			displ = -displ;
		}

		TextInfo text = symbol.text;
		TextPart[] array = text.textArray;
		if (array != null) {
			for (TextPart part : array) {
				if (part.text != null)
					output.append(part.text);
			}
		}
		else if (text.text != null)
			output.append(text.text.text);

		if (displ != 0) {
			if (displ < 0) {
				output.append('-');
				displ = -displ;
			}
			else
				output.append('+');
			formatNumber(output, displ);
		}
		if (options.getShowSymbolAddress()) {
			output.append(" (");
			formatNumber(output, address);
			output.append(')');
		}
	}

	private void formatMemory(FastStringOutput output, Instruction instruction, int operand, int segReg, int baseReg, int indexReg, int scale,
			int displSize, long displ, int addrSize) {
		assert Integer.compareUnsigned(scale, scaleNumbers.length) < 0;
		assert com.github.icedland.iced.x86.InternalInstructionUtils.getAddressSizeInBytes(baseReg, indexReg, displSize,
				instruction.getCodeSize()) == addrSize : addrSize;

		long absAddr;
		if (baseReg == Register.RIP) {
			absAddr = displ;
			if (options.getRipRelativeAddresses())
				displ -= instruction.getNextIP();
			else {
				assert indexReg == Register.NONE : indexReg;
				baseReg = Register.NONE;
			}
			displSize = 8;
		}
		else if (baseReg == Register.EIP) {
			absAddr = displ & 0xFFFF_FFFFL;
			if (options.getRipRelativeAddresses())
				displ = (int)displ - instruction.getNextIP32();
			else {
				assert indexReg == Register.NONE : indexReg;
				baseReg = Register.NONE;
			}
			displSize = 4;
		}
		else
			absAddr = displ;

		SymbolResult symbol;
		if (symbolResolver != null)
			symbol = symbolResolver.getSymbol(instruction, operand, operand, absAddr, addrSize);
		else
			symbol = null;

		boolean useScale = scale != 0;
		if (!useScale) {
			// [rsi] = base reg, [rsi*1] = index reg
			if (baseReg == Register.NONE)
				useScale = true;
		}
		if (addrSize == 2)
			useScale = false;

		byte flags = codeFlags[instruction.getCode()];
		boolean showMemSize = (flags & FastFmtFlags.FORCE_MEM_SIZE) != 0 || instruction.getBroadcast() || options.getAlwaysShowMemorySize();
		if (showMemSize) {
			assert Integer.compareUnsigned(instruction.getMemorySize(), allMemorySizes.length) < 0;
			String keywords = allMemorySizes[instruction.getMemorySize()];
			output.append(keywords);
		}

		int codeSize = instruction.getCodeSize();
		int segOverride = instruction.getSegmentPrefix();
		boolean noTrackPrefix = segOverride == Register.DS
				&& com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isNotrackPrefixBranch(instruction.getCode()) &&
				!((codeSize == CodeSize.CODE16 || codeSize == CodeSize.CODE32)
						&& (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP));
		if (options.getAlwaysShowSegmentRegister() || (segOverride != Register.NONE && !noTrackPrefix &&
				(SHOW_USELESS_PREFIXES || com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.NONE, instruction,
						SHOW_USELESS_PREFIXES)))) {
			formatRegister(output, segReg);
			output.append(':');
		}
		output.append('[');

		boolean needPlus = false;
		if (baseReg != Register.NONE) {
			formatRegister(output, baseReg);
			needPlus = true;
		}

		if (indexReg != Register.NONE) {
			if (needPlus)
				output.append('+');
			needPlus = true;

			formatRegister(output, indexReg);
			if (useScale)
				output.append(scaleNumbers[scale]);
		}

		if (symbol != null) {
			if (needPlus) {
				if ((symbol.flags & SymbolFlags.SIGNED) != 0)
					output.append('-');
				else
					output.append('+');
			}
			else if ((symbol.flags & SymbolFlags.SIGNED) != 0)
				output.append('-');

			writeSymbol(output, absAddr, symbol, false);
		}
		else if (!needPlus || (displSize != 0 && displ != 0)) {
			if (needPlus) {
				if (addrSize == 8) {
					if (displ < 0) {
						displ = -displ;
						output.append('-');
					}
					else
						output.append('+');
				}
				else if (addrSize == 4) {
					if ((int)displ < 0) {
						displ = ((long)-(int)displ) & 0xFFFF_FFFFL;
						output.append('-');
					}
					else
						output.append('+');
				}
				else {
					assert addrSize == 2 : addrSize;
					if ((short)displ < 0) {
						displ = (-(short)displ) & 0xFFFF;
						output.append('-');
					}
					else
						output.append('+');
				}
			}
			formatNumber(output, displ);
		}

		output.append(']');
		if (instruction.getMvexEvictionHint())
			output.append("{eh}");
	}
}
