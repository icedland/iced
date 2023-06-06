// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.nasm;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MvexConvFn;
import com.github.icedland.iced.x86.MvexRegMemConv;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.fmt.DecoratorKind;
import com.github.icedland.iced.x86.fmt.FormatMnemonicOptions;
import com.github.icedland.iced.x86.fmt.Formatter;
import com.github.icedland.iced.x86.fmt.FormatterOperandOptions;
import com.github.icedland.iced.x86.fmt.FormatterOptions;
import com.github.icedland.iced.x86.fmt.FormatterOptionsProvider;
import com.github.icedland.iced.x86.fmt.FormatterOutput;
import com.github.icedland.iced.x86.fmt.FormatterTextKind;
import com.github.icedland.iced.x86.fmt.MemorySizeOptions;
import com.github.icedland.iced.x86.fmt.NumberFormattingOptions;
import com.github.icedland.iced.x86.fmt.NumberKind;
import com.github.icedland.iced.x86.fmt.PrefixKind;
import com.github.icedland.iced.x86.fmt.SymbolFlags;
import com.github.icedland.iced.x86.fmt.SymbolResolver;
import com.github.icedland.iced.x86.fmt.SymbolResult;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.fmt.FormatterString;

/**
 * Nasm formatter
 */
@SuppressWarnings("deprecation") // Our internal classes are deprecated so the users get warnings if they use them
public final class NasmFormatter extends Formatter {
	/**
	 * Gets the formatter options
	 */
	@Override
	public FormatterOptions getOptions() {
		return options;
	}

	private final FormatterOptions options;
	private final SymbolResolver symbolResolver;
	private final FormatterOptionsProvider optionsProvider;
	private final FormatterString[] allRegisters;
	private final InstrInfo[] instrInfos;
	private final MemorySizes.Info[] allMemorySizes;
	private final com.github.icedland.iced.x86.internal.fmt.NumberFormatter numberFormatter;
	private final FormatterString[] opSizeStrings;
	private final FormatterString[] addrSizeStrings;
	private final FormatterString[][] branchInfos;
	private final String[] scaleNumbers;
	private final FormatterString[] mvexRegMemConsts32;
	private final FormatterString[] mvexRegMemConsts64;
	private final FormatterString[] memSizeInfos;
	private final FormatterString[] farMemSizeInfos;

	/**
	 * Constructor
	 */
	public NasmFormatter() {
		this(null, null);
	}

	/**
	 * Constructor
	 *
	 * @param symbolResolver Symbol resolver or null
	 */
	public NasmFormatter(SymbolResolver symbolResolver) {
		this(symbolResolver, null);
	}

	private static FormatterOptions createNasm() {
		FormatterOptions options = new FormatterOptions();
		options.setHexSuffix("h");
		options.setOctalSuffix("o");
		options.setBinarySuffix("b");
		return options;
	}

	/**
	 * Constructor
	 *
	 * @param symbolResolver  Symbol resolver or null
	 * @param optionsProvider Operand options provider or null
	 */
	public NasmFormatter(SymbolResolver symbolResolver, FormatterOptionsProvider optionsProvider) {
		this.options = createNasm();
		this.symbolResolver = symbolResolver;
		this.optionsProvider = optionsProvider;
		allRegisters = Registers.allRegisters;
		instrInfos = InstrInfos.allInfos;
		allMemorySizes = MemorySizes.allMemorySizes;
		numberFormatter = new com.github.icedland.iced.x86.internal.fmt.NumberFormatter(true);
		opSizeStrings = s_opSizeStrings;
		addrSizeStrings = s_addrSizeStrings;
		branchInfos = s_branchInfos;
		scaleNumbers = s_scaleNumbers;
		mvexRegMemConsts32 = s_mvexRegMemConsts32;
		mvexRegMemConsts64 = s_mvexRegMemConsts64;
		memSizeInfos = s_memSizeInfos;
		farMemSizeInfos = s_farMemSizeInfos;
	}

	private static final FormatterString str_bnd = new FormatterString("bnd");
	private static final FormatterString str_byte = new FormatterString("byte");
	private static final FormatterString str_dword = new FormatterString("dword");
	private static final FormatterString str_lock = new FormatterString("lock");
	private static final FormatterString str_notrack = new FormatterString("notrack");
	private static final FormatterString str_qword = new FormatterString("qword");
	private static final FormatterString str_rel = new FormatterString("rel");
	private static final FormatterString str_rep = new FormatterString("rep");
	private static final FormatterString[] str_repe = new FormatterString[] {
		new FormatterString("repe"),
		new FormatterString("repz"),
	};
	private static final FormatterString[] str_repne = new FormatterString[] {
		new FormatterString("repne"),
		new FormatterString("repnz"),
	};
	private static final FormatterString str_rn_sae = new FormatterString("rn-sae");
	private static final FormatterString str_rd_sae = new FormatterString("rd-sae");
	private static final FormatterString str_ru_sae = new FormatterString("ru-sae");
	private static final FormatterString str_rz_sae = new FormatterString("rz-sae");
	private static final FormatterString str_sae = new FormatterString("sae");
	private static final FormatterString str_rn = new FormatterString("rn");
	private static final FormatterString str_rd = new FormatterString("rd");
	private static final FormatterString str_ru = new FormatterString("ru");
	private static final FormatterString str_rz = new FormatterString("rz");
	private static final FormatterString str_to = new FormatterString("to");
	private static final FormatterString str_word = new FormatterString("word");
	private static final FormatterString str_xacquire = new FormatterString("xacquire");
	private static final FormatterString str_xrelease = new FormatterString("xrelease");
	private static final FormatterString str_z = new FormatterString("z");
	private static final FormatterString[] s_opSizeStrings = new FormatterString[] {
		new FormatterString(""),
		new FormatterString("o16"),
		new FormatterString("o32"),
		new FormatterString("o64"),
	};
	private static final FormatterString[] s_addrSizeStrings = new FormatterString[] {
		new FormatterString(""),
		new FormatterString("a16"),
		new FormatterString("a32"),
		new FormatterString("a64"),
	};
	private static final FormatterString[][] s_branchInfos = new FormatterString[][] {
		null,
		new FormatterString[] { new FormatterString("near") },
		new FormatterString[] { new FormatterString("near"), new FormatterString("word") },
		new FormatterString[] { new FormatterString("near"), new FormatterString("dword") },
		new FormatterString[] { new FormatterString("word") },
		new FormatterString[] { new FormatterString("dword") },
		new FormatterString[] { new FormatterString("short") },
		null,
	};
	private static final FormatterString[] s_memSizeInfos = new FormatterString[] {
		new FormatterString(""),
		new FormatterString("word"),
		new FormatterString("dword"),
		new FormatterString("qword"),
	};
	private static final FormatterString[] s_farMemSizeInfos = new FormatterString[] {
		new FormatterString(""),
		new FormatterString("word"),
		new FormatterString("dword"),
		new FormatterString(""),
	};
	private static final String[] s_scaleNumbers = new String[] {
		"1", "2", "4", "8",
	};
	private static final FormatterString[] s_mvexRegMemConsts32 = new FormatterString[] {
		new FormatterString(""),
		new FormatterString(""),
		new FormatterString("cdab"),
		new FormatterString("badc"),
		new FormatterString("dacb"),
		new FormatterString("aaaa"),
		new FormatterString("bbbb"),
		new FormatterString("cccc"),
		new FormatterString("dddd"),
		new FormatterString(""),
		new FormatterString("1to16"),
		new FormatterString("4to16"),
		new FormatterString("float16"),
		new FormatterString("uint8"),
		new FormatterString("sint8"),
		new FormatterString("uint16"),
		new FormatterString("sint16"),
	};
	private static final FormatterString[] s_mvexRegMemConsts64 = new FormatterString[] {
		new FormatterString(""),
		new FormatterString(""),
		new FormatterString("cdab"),
		new FormatterString("badc"),
		new FormatterString("dacb"),
		new FormatterString("aaaa"),
		new FormatterString("bbbb"),
		new FormatterString("cccc"),
		new FormatterString("dddd"),
		new FormatterString(""),
		new FormatterString("1to8"),
		new FormatterString("4to8"),
		new FormatterString("float16"),
		new FormatterString("uint8"),
		new FormatterString("sint8"),
		new FormatterString("uint16"),
		new FormatterString("sint16"),
	};
	private static final FormatterString str_eh = new FormatterString("eh");

	/**
	 * Formats the mnemonic and/or any prefixes
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 * @param options     Options (a {@link FormatMnemonicOptions} flags value)
	 */
	@Override
	public void formatMnemonic(Instruction instruction, FormatterOutput output, int options) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(this.options, instruction);
		formatMnemonic(instruction, output, opInfo, 0, options);
	}

	/**
	 * Gets the number of operands that will be formatted. A formatter can add and remove operands
	 *
	 * @param instruction Instruction
	 */
	@Override
	public int getOperandCount(Instruction instruction) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(options, instruction);
		return opInfo.opCount;
	}

	/**
	 * Returns the operand access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant) but only if it's an operand added by the
	 * formatter. If it's an operand that is part of {@link com.github.icedland.iced.x86.Instruction}, you should call eg.
	 * {@link com.github.icedland.iced.x86.info.InstructionInfoFactory#getInfo(Instruction)}.
	 *
	 * @param instruction Instruction
	 * @param operand     Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See
	 *                    {@link #getOperandCount(Instruction)}
	 * @return The operand access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant) or {@code null}
	 */
	@Override
	public Integer tryGetOpAccess(Instruction instruction, int operand) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(options, instruction);
		// Although it's a tryXXX() method, it should only accept valid instruction operand indexes
		if (Integer.compareUnsigned(operand, opInfo.opCount) >= 0)
			throw new IllegalArgumentException("operand");
		return opInfo.tryGetOpAccess(operand);
	}

	/**
	 * Converts a formatter operand index to an instruction operand index. Returns -1 if it's an operand added by the formatter
	 *
	 * @param instruction Instruction
	 * @param operand     Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See
	 *                    {@link #getOperandCount(Instruction)}
	 */
	@Override
	public int getInstructionOperand(Instruction instruction, int operand) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(options, instruction);
		if (Integer.compareUnsigned(operand, opInfo.opCount) >= 0)
			throw new IllegalArgumentException("operand");
		return opInfo.getInstructionIndex(operand);
	}

	/**
	 * Converts an instruction operand index to a formatter operand index. Returns -1 if the instruction operand isn't used by the formatter
	 *
	 * @param instruction        Instruction
	 * @param instructionOperand Instruction operand
	 */
	@Override
	public int getFormatterOperand(Instruction instruction, int instructionOperand) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(options, instruction);
		if (Integer.compareUnsigned(instructionOperand, instruction.getOpCount()) >= 0)
			throw new IllegalArgumentException("instructionOperand");
		return opInfo.getOperandIndex(instructionOperand);
	}

	/**
	 * Formats an operand. This is a formatter operand and not necessarily a real instruction operand.
	 * A formatter can add and remove operands.
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 * @param operand     Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See
	 *                    {@link #getOperandCount(Instruction)}
	 */
	@Override
	public void formatOperand(Instruction instruction, FormatterOutput output, int operand) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(options, instruction);

		if (Integer.compareUnsigned(operand, opInfo.opCount) >= 0)
			throw new IllegalArgumentException("operand");
		formatOperand(instruction, output, opInfo, operand);
	}

	/**
	 * Formats an operand separator
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	@Override
	public void formatOperandSeparator(Instruction instruction, FormatterOutput output) {
		if (output == null)
			throw new NullPointerException("output");
		output.write(",", FormatterTextKind.PUNCTUATION);
		if (options.getSpaceAfterOperandSeparator())
			output.write(" ", FormatterTextKind.TEXT);
	}

	/**
	 * Formats all operands
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	@Override
	public void formatAllOperands(Instruction instruction, FormatterOutput output) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(options, instruction);
		formatOperands(instruction, output, opInfo);
	}

	/**
	 * Formats the whole instruction: prefixes, mnemonic, operands
	 *
	 * @param instruction Instruction
	 * @param output      Output
	 */
	@Override
	public void format(Instruction instruction, FormatterOutput output) {
		assert Integer.compareUnsigned(instruction.getCode(), instrInfos.length) < 0;
		InstrInfo instrInfo = instrInfos[instruction.getCode()];
		InstrOpInfo opInfo = instrInfo.getOpInfo(options, instruction);

		int column = formatMnemonic(instruction, output, opInfo, 0, FormatMnemonicOptions.NONE);

		if (opInfo.opCount != 0) {
			com.github.icedland.iced.x86.internal.fmt.FormatterUtils.addTabs(output, column, options.getFirstOperandCharIndex(),
					options.getTabSize());
			formatOperands(instruction, output, opInfo);
		}
	}

	private int formatMnemonic(Instruction instruction, FormatterOutput output, InstrOpInfo opInfo, int column, int mnemonicOptions) {
		if (output == null)
			throw new NullPointerException("output");
		if ((mnemonicOptions & FormatMnemonicOptions.NO_PREFIXES) == 0 && (opInfo.flags & InstrOpInfoFlags.MNEMONIC_IS_DIRECTIVE) == 0) {
			int prefixSeg = instruction.getSegmentPrefix();
			FormatterString prefix;

			prefix = opSizeStrings[(opInfo.flags >>> InstrOpInfoFlags.OP_SIZE_SHIFT) & InstrOpInfoFlags.SIZE_OVERRIDE_MASK];
			if (prefix.getLength() != 0)
				column = formatPrefix(output, instruction, column, prefix, PrefixKind.OPERAND_SIZE);

			prefix = addrSizeStrings[(opInfo.flags >>> InstrOpInfoFlags.ADDR_SIZE_SHIFT) & InstrOpInfoFlags.SIZE_OVERRIDE_MASK];
			if (prefix.getLength() != 0)
				column = formatPrefix(output, instruction, column, prefix, PrefixKind.ADDRESS_SIZE);

			boolean hasNoTrackPrefix = prefixSeg == Register.DS
					&& com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isNotrackPrefixBranch(instruction.getCode());
			if (!hasNoTrackPrefix && prefixSeg != Register.NONE && showSegmentPrefix(instruction, opInfo))
				column = formatPrefix(output, instruction, column, allRegisters[prefixSeg],
						com.github.icedland.iced.x86.internal.fmt.FormatterUtils.getSegmentRegisterPrefixKind(prefixSeg));

			if (instruction.getXacquirePrefix())
				column = formatPrefix(output, instruction, column, str_xacquire, PrefixKind.XACQUIRE);
			if (instruction.getXreleasePrefix())
				column = formatPrefix(output, instruction, column, str_xrelease, PrefixKind.XRELEASE);
			if (instruction.getLockPrefix())
				column = formatPrefix(output, instruction, column, str_lock, PrefixKind.LOCK);

			if (hasNoTrackPrefix)
				column = formatPrefix(output, instruction, column, str_notrack, PrefixKind.NOTRACK);
			boolean hasBnd = (opInfo.flags & InstrOpInfoFlags.BND_PREFIX) != 0;
			if (hasBnd)
				column = formatPrefix(output, instruction, column, str_bnd, PrefixKind.BND);

			if (instruction.getRepePrefix()
					&& com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showRepOrRepePrefix(instruction.getCode(), options)) {
				if (com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isRepeOrRepneInstruction(instruction.getCode()))
					column = formatPrefix(output, instruction, column,
							com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, 4, str_repe), PrefixKind.REPE);
				else
					column = formatPrefix(output, instruction, column, str_rep, PrefixKind.REP);
			}
			if (instruction.getRepnePrefix() && !hasBnd
					&& com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showRepnePrefix(instruction.getCode(), options))
				column = formatPrefix(output, instruction, column,
						com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, 5, str_repne), PrefixKind.REPNE);
		}

		if ((mnemonicOptions & FormatMnemonicOptions.NO_MNEMONIC) == 0) {
			if (column < 0) {
				output.write(" ", FormatterTextKind.TEXT);
				column++;
			}
			FormatterString mnemonic = opInfo.mnemonic;
			if ((opInfo.flags & InstrOpInfoFlags.MNEMONIC_IS_DIRECTIVE) != 0) {
				output.write(mnemonic.get(options.getUppercaseKeywords() || options.getUppercaseAll()), FormatterTextKind.DIRECTIVE);
			}
			else {
				output.writeMnemonic(instruction, mnemonic.get(options.getUppercaseMnemonics() || options.getUppercaseAll()));
			}
			column += mnemonic.getLength();
		}
		return column & 0x7FFF_FFFF;
	}

	private boolean showSegmentPrefix(Instruction instruction, InstrOpInfo opInfo) {
		if ((opInfo.flags & (InstrOpInfoFlags.JCC_NOT_TAKEN | InstrOpInfoFlags.JCC_TAKEN)) != 0)
			return true;

		switch (instruction.getCode()) {
		case Code.MONITORW:
		case Code.MONITORD:
		case Code.MONITORQ:
		case Code.MONITORXW:
		case Code.MONITORXD:
		case Code.MONITORXQ:
		case Code.CLZEROW:
		case Code.CLZEROD:
		case Code.CLZEROQ:
		case Code.UMONITOR_R16:
		case Code.UMONITOR_R32:
		case Code.UMONITOR_R64:
		case Code.MASKMOVQ_RDI_MM_MM:
		case Code.MASKMOVDQU_RDI_XMM_XMM:
		case Code.VEX_VMASKMOVDQU_RDI_XMM_XMM:
		case Code.XLAT_M8:
		case Code.OUTSB_DX_M8:
		case Code.OUTSW_DX_M16:
		case Code.OUTSD_DX_M32:
		case Code.MOVSB_M8_M8:
		case Code.MOVSW_M16_M16:
		case Code.MOVSD_M32_M32:
		case Code.MOVSQ_M64_M64:
		case Code.CMPSB_M8_M8:
		case Code.CMPSW_M16_M16:
		case Code.CMPSD_M32_M32:
		case Code.CMPSQ_M64_M64:
		case Code.LODSB_AL_M8:
		case Code.LODSW_AX_M16:
		case Code.LODSD_EAX_M32:
		case Code.LODSQ_RAX_M64:
			return com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options);

		default:
			break;
		}

		for (int i = 0; i < opInfo.opCount; i++) {
			switch (opInfo.getOpKind(i)) {
			case InstrOpKind.REGISTER:
			case InstrOpKind.NEAR_BRANCH16:
			case InstrOpKind.NEAR_BRANCH32:
			case InstrOpKind.NEAR_BRANCH64:
			case InstrOpKind.FAR_BRANCH16:
			case InstrOpKind.FAR_BRANCH32:
			case InstrOpKind.IMMEDIATE8:
			case InstrOpKind.IMMEDIATE8_2ND:
			case InstrOpKind.IMMEDIATE16:
			case InstrOpKind.IMMEDIATE32:
			case InstrOpKind.IMMEDIATE64:
			case InstrOpKind.IMMEDIATE8TO16:
			case InstrOpKind.IMMEDIATE8TO32:
			case InstrOpKind.IMMEDIATE8TO64:
			case InstrOpKind.IMMEDIATE32TO64:
			case InstrOpKind.MEMORY_ESDI:
			case InstrOpKind.MEMORY_ESEDI:
			case InstrOpKind.MEMORY_ESRDI:
			case InstrOpKind.SAE:
			case InstrOpKind.RN_SAE:
			case InstrOpKind.RD_SAE:
			case InstrOpKind.RU_SAE:
			case InstrOpKind.RZ_SAE:
			case InstrOpKind.RN:
			case InstrOpKind.RD:
			case InstrOpKind.RU:
			case InstrOpKind.RZ:
			case InstrOpKind.DECLARE_BYTE:
			case InstrOpKind.DECLARE_WORD:
			case InstrOpKind.DECLARE_DWORD:
			case InstrOpKind.DECLARE_QWORD:
				break;

			case InstrOpKind.MEMORY_SEG_SI:
			case InstrOpKind.MEMORY_SEG_ESI:
			case InstrOpKind.MEMORY_SEG_RSI:
			case InstrOpKind.MEMORY_SEG_DI:
			case InstrOpKind.MEMORY_SEG_EDI:
			case InstrOpKind.MEMORY_SEG_RDI:
			case InstrOpKind.MEMORY:
				return false;

			default:
				throw new UnsupportedOperationException();
			}
		}
		return options.getShowUselessPrefixes();
	}

	private int formatPrefix(FormatterOutput output, Instruction instruction, int column, FormatterString prefix, int prefixKind) {
		if (column < 0) {
			column++;
			output.write(" ", FormatterTextKind.TEXT);
		}
		output.writePrefix(instruction, prefix.get(options.getUppercasePrefixes() || options.getUppercaseAll()), prefixKind);
		column += prefix.getLength();
		column |= 0x8000_0000;
		return column;
	}

	private void formatOperands(Instruction instruction, FormatterOutput output, InstrOpInfo opInfo) {
		if (output == null)
			throw new NullPointerException("output");
		for (int i = 0; i < opInfo.opCount; i++) {
			if (i > 0) {
				output.write(",", FormatterTextKind.PUNCTUATION);
				if (options.getSpaceAfterOperandSeparator())
					output.write(" ", FormatterTextKind.TEXT);
			}
			formatOperand(instruction, output, opInfo, i);
		}
	}

	private void formatOperand(Instruction instruction, FormatterOutput output, InstrOpInfo opInfo, int operand) {
		assert Integer.compareUnsigned(operand, opInfo.opCount) < 0 : operand;
		if (output == null)
			throw new NullPointerException("output");

		int mvexRmOperand;
		if (MvexInfo.isMvex(instruction.getCode())) {
			int opCount = instruction.getOpCount();
			assert opCount != 0 : opCount;
			mvexRmOperand = instruction.getOpKind(opCount - 1) == OpKind.IMMEDIATE8 ? opCount - 2 : opCount - 1;
		}
		else
			mvexRmOperand = -1;
		int instructionOperand = opInfo.getInstructionIndex(operand);

		String s;
		int flowControl;
		byte imm8;
		short imm16;
		int imm32;
		long imm64, value64;
		int immSize;
		NumberFormattingOptions numberOptions;
		SymbolResult symbol;
		FormatterOperandOptions operandOptions;
		int numberKind;
		int opKind = opInfo.getOpKind(operand);
		switch (opKind) {
		case InstrOpKind.REGISTER:
			if ((opInfo.flags & InstrOpInfoFlags.REGISTER_TO) != 0) {
				formatKeyword(output, str_to);
				output.write(" ", FormatterTextKind.TEXT);
			}
			formatRegister(output, instruction, operand, instructionOperand, opInfo.getOpRegister(operand));
			break;

		case InstrOpKind.NEAR_BRANCH16:
		case InstrOpKind.NEAR_BRANCH32:
		case InstrOpKind.NEAR_BRANCH64:
			if (opKind == InstrOpKind.NEAR_BRANCH64) {
				immSize = 8;
				imm64 = instruction.getNearBranch64();
				numberKind = NumberKind.UINT64;
			}
			else if (opKind == InstrOpKind.NEAR_BRANCH32) {
				immSize = 4;
				imm64 = instruction.getNearBranch32() & 0xFFFF_FFFFL;
				numberKind = NumberKind.UINT32;
			}
			else {
				immSize = 2;
				imm64 = instruction.getNearBranch16() & 0xFFFF;
				numberKind = NumberKind.UINT16;
			}
			numberOptions = NumberFormattingOptions.createBranch(options);
			operandOptions = new FormatterOperandOptions();
			operandOptions.setBranchSize(options.getShowBranchSize());
			if (optionsProvider != null)
				optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);
			if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, instructionOperand, imm64, immSize)) != null) {
				formatFlowControl(output, opInfo.flags, operandOptions);
				com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
						numberFormatter, numberOptions, imm64, symbol, options.getShowSymbolAddress());
			}
			else {
				operandOptions = new FormatterOperandOptions();
				operandOptions.setBranchSize(options.getShowBranchSize());
				if (optionsProvider != null)
					optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);
				flowControl = com.github.icedland.iced.x86.internal.fmt.FormatterUtils.getFlowControl(instruction);
				formatFlowControl(output, opInfo.flags, operandOptions);
				if (opKind == InstrOpKind.NEAR_BRANCH32)
					s = numberFormatter.formatUInt32(options, numberOptions, instruction.getNearBranch32(), numberOptions.leadingZeros);
				else if (opKind == InstrOpKind.NEAR_BRANCH64)
					s = numberFormatter.formatUInt64(options, numberOptions, instruction.getNearBranch64(), numberOptions.leadingZeros);
				else
					s = numberFormatter.formatUInt16(options, numberOptions, instruction.getNearBranch16(), numberOptions.leadingZeros);
				output.writeNumber(instruction, operand, instructionOperand, s, imm64, numberKind,
						com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isCall(flowControl) ? FormatterTextKind.FUNCTION_ADDRESS
								: FormatterTextKind.LABEL_ADDRESS);
			}
			break;

		case InstrOpKind.FAR_BRANCH16:
		case InstrOpKind.FAR_BRANCH32:
			if (opKind == InstrOpKind.FAR_BRANCH32) {
				immSize = 4;
				imm64 = instruction.getFarBranch32() & 0xFFFF_FFFFL;
				numberKind = NumberKind.UINT32;
			}
			else {
				immSize = 2;
				imm64 = instruction.getFarBranch16() & 0xFFFF;
				numberKind = NumberKind.UINT16;
			}
			numberOptions = NumberFormattingOptions.createBranch(options);
			operandOptions = new FormatterOperandOptions();
			operandOptions.setBranchSize(options.getShowBranchSize());
			if (optionsProvider != null)
				optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);
			if (symbolResolver != null
					&& (symbol = symbolResolver.getSymbol(instruction, operand, instructionOperand, imm64, immSize)) != null) {
				formatFlowControl(output, opInfo.flags, operandOptions);
				assert operand + 1 == 1 : operand;
				SymbolResult selectorSymbol = symbolResolver.getSymbol(instruction, operand + 1, instructionOperand,
						instruction.getFarBranchSelector() & 0xFFFF, 2);
				if (selectorSymbol == null) {
					s = numberFormatter.formatUInt16(options, numberOptions, instruction.getFarBranchSelector(), numberOptions.leadingZeros);
					output.writeNumber(instruction, operand, instructionOperand, s, instruction.getFarBranchSelector() & 0xFFFF, NumberKind.UINT16,
							FormatterTextKind.SELECTOR_VALUE);
				}
				else
					com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
							numberFormatter, numberOptions, instruction.getFarBranchSelector() & 0xFFFF, selectorSymbol,
							options.getShowSymbolAddress());
				output.write(":", FormatterTextKind.PUNCTUATION);
				com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
						numberFormatter, numberOptions, imm64, symbol, options.getShowSymbolAddress());
			}
			else {
				flowControl = com.github.icedland.iced.x86.internal.fmt.FormatterUtils.getFlowControl(instruction);
				formatFlowControl(output, opInfo.flags, operandOptions);
				s = numberFormatter.formatUInt16(options, numberOptions, instruction.getFarBranchSelector(), numberOptions.leadingZeros);
				output.writeNumber(instruction, operand, instructionOperand, s, instruction.getFarBranchSelector() & 0xFFFF, NumberKind.UINT16,
						FormatterTextKind.SELECTOR_VALUE);
				output.write(":", FormatterTextKind.PUNCTUATION);
				if (opKind == InstrOpKind.FAR_BRANCH32)
					s = numberFormatter.formatUInt32(options, numberOptions, instruction.getFarBranch32(), numberOptions.leadingZeros);
				else
					s = numberFormatter.formatUInt16(options, numberOptions, instruction.getFarBranch16(), numberOptions.leadingZeros);
				output.writeNumber(instruction, operand, instructionOperand, s, imm64, numberKind,
						com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isCall(flowControl) ? FormatterTextKind.FUNCTION_ADDRESS
								: FormatterTextKind.LABEL_ADDRESS);
			}
			break;

		case InstrOpKind.IMMEDIATE8:
		case InstrOpKind.IMMEDIATE8_2ND:
		case InstrOpKind.DECLARE_BYTE:
			if (opKind == InstrOpKind.IMMEDIATE8)
				imm8 = instruction.getImmediate8();
			else if (opKind == InstrOpKind.IMMEDIATE8_2ND)
				imm8 = instruction.getImmediate8_2nd();
			else
				imm8 = instruction.getDeclareByteValue(operand);
			numberOptions = NumberFormattingOptions.createImmediate(options);
			operandOptions = new FormatterOperandOptions();
			if (optionsProvider != null)
				optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);
			if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, instructionOperand, imm8 & 0xFF, 1)) != null)
				com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
						numberFormatter, numberOptions, imm8 & 0xFF, symbol, options.getShowSymbolAddress());
			else {
				if (numberOptions.signedNumber) {
					imm64 = imm8;
					numberKind = NumberKind.INT8;
					if (imm8 < 0) {
						output.write("-", FormatterTextKind.OPERATOR);
						imm8 = (byte)-imm8;
					}
				}
				else {
					imm64 = imm8 & 0xFF;
					numberKind = NumberKind.UINT8;
				}
				s = numberFormatter.formatUInt8(options, numberOptions, imm8);
				output.writeNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterTextKind.NUMBER);
			}
			break;

		case InstrOpKind.IMMEDIATE16:
		case InstrOpKind.IMMEDIATE8TO16:
		case InstrOpKind.DECLARE_WORD:
			showSignExtendInfo(output, opInfo.flags);
			if (opKind == InstrOpKind.IMMEDIATE16)
				imm16 = instruction.getImmediate16();
			else if (opKind == InstrOpKind.IMMEDIATE8TO16)
				imm16 = instruction.getImmediate8to16();
			else
				imm16 = instruction.getDeclareWordValue(operand);
			numberOptions = NumberFormattingOptions.createImmediate(options);
			operandOptions = new FormatterOperandOptions();
			if (optionsProvider != null)
				optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);
			if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, instructionOperand, imm16 & 0xFFFF, 2)) != null)
				com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
						numberFormatter, numberOptions, imm16 & 0xFFFF, symbol, options.getShowSymbolAddress());
			else {
				if (numberOptions.signedNumber) {
					imm64 = imm16;
					numberKind = NumberKind.INT16;
					if (imm16 < 0) {
						output.write("-", FormatterTextKind.OPERATOR);
						imm16 = (short)-imm16;
					}
				}
				else {
					imm64 = imm16 & 0xFFFF;
					numberKind = NumberKind.UINT16;
				}
				s = numberFormatter.formatUInt16(options, numberOptions, imm16);
				output.writeNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterTextKind.NUMBER);
			}
			break;

		case InstrOpKind.IMMEDIATE32:
		case InstrOpKind.IMMEDIATE8TO32:
		case InstrOpKind.DECLARE_DWORD:
			showSignExtendInfo(output, opInfo.flags);
			if (opKind == InstrOpKind.IMMEDIATE32)
				imm32 = instruction.getImmediate32();
			else if (opKind == InstrOpKind.IMMEDIATE8TO32)
				imm32 = instruction.getImmediate8to32();
			else
				imm32 = instruction.getDeclareDwordValue(operand);
			numberOptions = NumberFormattingOptions.createImmediate(options);
			operandOptions = new FormatterOperandOptions();
			if (optionsProvider != null)
				optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);
			if (symbolResolver != null
					&& (symbol = symbolResolver.getSymbol(instruction, operand, instructionOperand, (long)imm32 & 0xFFFF_FFFFL, 4)) != null)
				com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
						numberFormatter, numberOptions, (long)imm32 & 0xFFFF_FFFFL, symbol, options.getShowSymbolAddress());
			else {
				if (numberOptions.signedNumber) {
					imm64 = imm32;
					numberKind = NumberKind.INT32;
					if (imm32 < 0) {
						output.write("-", FormatterTextKind.OPERATOR);
						imm32 = -imm32;
					}
				}
				else {
					imm64 = (long)imm32 & 0xFFFF_FFFFL;
					numberKind = NumberKind.UINT32;
				}
				s = numberFormatter.formatUInt32(options, numberOptions, imm32);
				output.writeNumber(instruction, operand, instructionOperand, s, imm64, numberKind, FormatterTextKind.NUMBER);
			}
			break;

		case InstrOpKind.IMMEDIATE64:
		case InstrOpKind.IMMEDIATE8TO64:
		case InstrOpKind.IMMEDIATE32TO64:
		case InstrOpKind.DECLARE_QWORD:
			showSignExtendInfo(output, opInfo.flags);
			if (opKind == InstrOpKind.IMMEDIATE32TO64)
				imm64 = instruction.getImmediate32to64();
			else if (opKind == InstrOpKind.IMMEDIATE8TO64)
				imm64 = instruction.getImmediate8to64();
			else if (opKind == InstrOpKind.IMMEDIATE64)
				imm64 = instruction.getImmediate64();
			else
				imm64 = instruction.getDeclareQwordValue(operand);
			numberOptions = NumberFormattingOptions.createImmediate(options);
			operandOptions = new FormatterOperandOptions();
			if (optionsProvider != null)
				optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);
			if (symbolResolver != null && (symbol = symbolResolver.getSymbol(instruction, operand, instructionOperand, imm64, 8)) != null)
				com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
						numberFormatter, numberOptions, imm64, symbol, options.getShowSymbolAddress());
			else {
				value64 = imm64;
				if (numberOptions.signedNumber) {
					numberKind = NumberKind.INT64;
					if (imm64 < 0) {
						output.write("-", FormatterTextKind.OPERATOR);
						imm64 = -imm64;
					}
				}
				else
					numberKind = NumberKind.UINT64;
				s = numberFormatter.formatUInt64(options, numberOptions, imm64);
				output.writeNumber(instruction, operand, instructionOperand, s, value64, numberKind, FormatterTextKind.NUMBER);
			}
			break;

		case InstrOpKind.MEMORY_SEG_SI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), instruction.getMemorySegment(), Register.SI,
					Register.NONE, 0, 0, 0, 2, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_SEG_ESI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), instruction.getMemorySegment(), Register.ESI,
					Register.NONE, 0, 0, 0, 4, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_SEG_RSI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), instruction.getMemorySegment(), Register.RSI,
					Register.NONE, 0, 0, 0, 8, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_SEG_DI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), instruction.getMemorySegment(), Register.DI,
					Register.NONE, 0, 0, 0, 2, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_SEG_EDI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), instruction.getMemorySegment(), Register.EDI,
					Register.NONE, 0, 0, 0, 4, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_SEG_RDI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), instruction.getMemorySegment(), Register.RDI,
					Register.NONE, 0, 0, 0, 8, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_ESDI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), Register.ES, Register.DI, Register.NONE, 0, 0, 0,
					2, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_ESEDI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), Register.ES, Register.EDI, Register.NONE, 0, 0, 0,
					4, opInfo.flags);
			break;

		case InstrOpKind.MEMORY_ESRDI:
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), Register.ES, Register.RDI, Register.NONE, 0, 0, 0,
					8, opInfo.flags);
			break;

		case InstrOpKind.MEMORY:
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
			formatMemory(output, instruction, operand, instructionOperand, opInfo.getMemorySize(), instruction.getMemorySegment(), baseReg, indexReg,
					instruction.getRawMemoryIndexScale(), displSize, displ, addrSize, opInfo.flags);
			break;

		case InstrOpKind.SAE:
			formatDecorator(output, instruction, operand, instructionOperand, str_sae, DecoratorKind.SUPPRESS_ALL_EXCEPTIONS);
			break;

		case InstrOpKind.RN_SAE:
			formatDecorator(output, instruction, operand, instructionOperand, str_rn_sae, DecoratorKind.ROUNDING_CONTROL);
			break;

		case InstrOpKind.RD_SAE:
			formatDecorator(output, instruction, operand, instructionOperand, str_rd_sae, DecoratorKind.ROUNDING_CONTROL);
			break;

		case InstrOpKind.RU_SAE:
			formatDecorator(output, instruction, operand, instructionOperand, str_ru_sae, DecoratorKind.ROUNDING_CONTROL);
			break;

		case InstrOpKind.RZ_SAE:
			formatDecorator(output, instruction, operand, instructionOperand, str_rz_sae, DecoratorKind.ROUNDING_CONTROL);
			break;

		case InstrOpKind.RN:
			formatDecorator(output, instruction, operand, instructionOperand, str_rn, DecoratorKind.ROUNDING_CONTROL);
			break;

		case InstrOpKind.RD:
			formatDecorator(output, instruction, operand, instructionOperand, str_rd, DecoratorKind.ROUNDING_CONTROL);
			break;

		case InstrOpKind.RU:
			formatDecorator(output, instruction, operand, instructionOperand, str_ru, DecoratorKind.ROUNDING_CONTROL);
			break;

		case InstrOpKind.RZ:
			formatDecorator(output, instruction, operand, instructionOperand, str_rz, DecoratorKind.ROUNDING_CONTROL);
			break;

		default:
			throw new UnsupportedOperationException();
		}

		if (operand == 0 && (instruction.hasOpMask() || instruction.getZeroingMasking())) {
			if (instruction.hasOpMask()) {
				output.write("{", FormatterTextKind.PUNCTUATION);
				formatRegister(output, instruction, operand, instructionOperand, instruction.getOpMask());
				output.write("}", FormatterTextKind.PUNCTUATION);
			}
			if (instruction.getZeroingMasking())
				formatDecorator(output, instruction, operand, instructionOperand, str_z, DecoratorKind.ZEROING_MASKING);
		}
		if (mvexRmOperand == operand) {
			int conv = instruction.getMvexRegMemConv();
			if (conv != MvexRegMemConv.NONE) {
				if (MvexInfo.getConvFn(instruction.getCode()) != MvexConvFn.NONE) {
					FormatterString[] tbl = MvexInfo.isConvFn32(instruction.getCode()) ? mvexRegMemConsts32 : mvexRegMemConsts64;
					FormatterString fs = tbl[conv];
					if (fs.getLength() != 0)
						formatDecorator(output, instruction, operand, instructionOperand, fs, DecoratorKind.SWIZZLE_MEM_CONV);
				}
			}
		}
	}

	private void showSignExtendInfo(FormatterOutput output, int flags) {
		if (!options.getNasmShowSignExtendedImmediateSize())
			return;

		FormatterString keyword;
		switch ((flags >>> InstrOpInfoFlags.SIGN_EXTEND_INFO_SHIFT) & InstrOpInfoFlags.SIGN_EXTEND_INFO_MASK) {
		case SignExtendInfo.NONE:
			return;

		case SignExtendInfo.SEX1TO2:
		case SignExtendInfo.SEX1TO4:
		case SignExtendInfo.SEX1TO8:
			keyword = str_byte;
			break;

		case SignExtendInfo.SEX2:
			keyword = str_word;
			break;

		case SignExtendInfo.SEX4:
			keyword = str_dword;
			break;

		case SignExtendInfo.SEX4TO8:
			keyword = str_qword;
			break;

		default:
			throw new UnsupportedOperationException();
		}

		formatKeyword(output, keyword);
		output.write(" ", FormatterTextKind.TEXT);
	}

	private void formatFlowControl(FormatterOutput output, int flags, FormatterOperandOptions operandOptions) {
		if (!operandOptions.getBranchSize())
			return;
		FormatterString[] keywords = branchInfos[(flags >>> InstrOpInfoFlags.BRANCH_SIZE_INFO_SHIFT) & InstrOpInfoFlags.BRANCH_SIZE_INFO_MASK];
		if (keywords == null)
			return;
		for (FormatterString keyword : keywords) {
			formatKeyword(output, keyword);
			output.write(" ", FormatterTextKind.TEXT);
		}
	}

	private void formatDecorator(FormatterOutput output, Instruction instruction, int operand, int instructionOperand, FormatterString text,
			int decorator) {
		output.write("{", FormatterTextKind.PUNCTUATION);
		output.writeDecorator(instruction, operand, instructionOperand, text.get(options.getUppercaseDecorators() || options.getUppercaseAll()),
				decorator);
		output.write("}", FormatterTextKind.PUNCTUATION);
	}

	private String toRegisterString(int reg) {
		assert Integer.compareUnsigned(reg, allRegisters.length) < 0 : reg;
		FormatterString regStr = allRegisters[reg];
		return regStr.get(options.getUppercaseRegisters() || options.getUppercaseAll());
	}

	private void formatRegister(FormatterOutput output, Instruction instruction, int operand, int instructionOperand, int reg) {
		output.writeRegister(instruction, operand, instructionOperand, toRegisterString(reg), reg);
	}

	private void formatMemory(FormatterOutput output, Instruction instruction, int operand, int instructionOperand, int memSize, int segReg,
			int baseReg, int indexReg, int scale, int displSize, long displ, int addrSize, int flags) {
		assert Integer.compareUnsigned(scale, scaleNumbers.length) < 0 : scale;
		assert com.github.icedland.iced.x86.InternalInstructionUtils.getAddressSizeInBytes(baseReg, indexReg, displSize,
				instruction.getCodeSize()) == addrSize : addrSize;

		NumberFormattingOptions numberOptions = NumberFormattingOptions.createDisplacement(options);
		SymbolResult symbol;

		FormatterOperandOptions operandOptions = FormatterOperandOptions.withMemorySizeOptions(options.getMemorySizeOptions());
		operandOptions.setRipRelativeAddresses(options.getRipRelativeAddresses());
		if (optionsProvider != null)
			optionsProvider.getOperandOptions(instruction, operand, instructionOperand, operandOptions, numberOptions);

		long absAddr;
		boolean addRelKeyword = false;
		if (baseReg == Register.RIP) {
			absAddr = displ;
			if (options.getRipRelativeAddresses())
				displ -= instruction.getNextIP();
			else {
				assert indexReg == Register.NONE : indexReg;
				baseReg = Register.NONE;
				flags &= ~(InstrOpInfoFlags.MEMORY_SIZE_INFO_MASK << InstrOpInfoFlags.MEMORY_SIZE_INFO_SHIFT);
				addRelKeyword = true;
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
				flags = (flags & ~(InstrOpInfoFlags.MEMORY_SIZE_INFO_MASK << InstrOpInfoFlags.MEMORY_SIZE_INFO_SHIFT))
						| (MemorySizeInfo.DWORD << InstrOpInfoFlags.MEMORY_SIZE_INFO_SHIFT);
				addRelKeyword = true;
			}
			displSize = 4;
		}
		else
			absAddr = displ;

		if (symbolResolver != null)
			symbol = symbolResolver.getSymbol(instruction, operand, instructionOperand, absAddr, addrSize);
		else
			symbol = null;

		boolean useScale = scale != 0 || options.getAlwaysShowScale();
		if (!useScale) {
			// [rsi] = base reg, [rsi*1] = index reg
			if (baseReg == Register.NONE)
				useScale = true;
		}
		if (addrSize == 2 || !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showIndexScale(instruction, options))
			useScale = false;

		formatMemorySize(output, memSize, flags, operandOptions);

		output.write("[", FormatterTextKind.PUNCTUATION);
		if (options.getSpaceAfterMemoryBracket())
			output.write(" ", FormatterTextKind.TEXT);

		FormatterString memSizeName = memSizeInfos[(flags >>> InstrOpInfoFlags.MEMORY_SIZE_INFO_SHIFT) & InstrOpInfoFlags.MEMORY_SIZE_INFO_MASK];
		if (memSizeName.getLength() != 0) {
			formatKeyword(output, memSizeName);
			output.write(" ", FormatterTextKind.TEXT);
		}

		if (addRelKeyword) {
			formatKeyword(output, str_rel);
			output.write(" ", FormatterTextKind.TEXT);
		}

		int codeSize = instruction.getCodeSize();
		int segOverride = instruction.getSegmentPrefix();
		boolean noTrackPrefix = segOverride == Register.DS
				&& com.github.icedland.iced.x86.internal.fmt.FormatterUtils.isNotrackPrefixBranch(instruction.getCode()) &&
				!((codeSize == CodeSize.CODE16 || codeSize == CodeSize.CODE32)
						&& (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP));
		if (options.getAlwaysShowSegmentRegister() || (segOverride != Register.NONE && !noTrackPrefix
				&& com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.NONE, instruction, options))) {
			formatRegister(output, instruction, operand, instructionOperand, segReg);
			output.write(":", FormatterTextKind.PUNCTUATION);
		}

		boolean needPlus = false;
		if (baseReg != Register.NONE) {
			formatRegister(output, instruction, operand, instructionOperand, baseReg);
			needPlus = true;
		}

		if (indexReg != Register.NONE) {
			if (needPlus) {
				if (options.getSpaceBetweenMemoryAddOperators())
					output.write(" ", FormatterTextKind.TEXT);
				output.write("+", FormatterTextKind.OPERATOR);
				if (options.getSpaceBetweenMemoryAddOperators())
					output.write(" ", FormatterTextKind.TEXT);
			}
			needPlus = true;

			if (!useScale)
				formatRegister(output, instruction, operand, instructionOperand, indexReg);
			else if (options.getScaleBeforeIndex()) {
				output.writeNumber(instruction, operand, instructionOperand, scaleNumbers[scale], 1 << scale, NumberKind.INT32,
						FormatterTextKind.NUMBER);
				if (options.getSpaceBetweenMemoryMulOperators())
					output.write(" ", FormatterTextKind.TEXT);
				output.write("*", FormatterTextKind.OPERATOR);
				if (options.getSpaceBetweenMemoryMulOperators())
					output.write(" ", FormatterTextKind.TEXT);
				formatRegister(output, instruction, operand, instructionOperand, indexReg);
			}
			else {
				formatRegister(output, instruction, operand, instructionOperand, indexReg);
				if (options.getSpaceBetweenMemoryMulOperators())
					output.write(" ", FormatterTextKind.TEXT);
				output.write("*", FormatterTextKind.OPERATOR);
				if (options.getSpaceBetweenMemoryMulOperators())
					output.write(" ", FormatterTextKind.TEXT);
				output.writeNumber(instruction, operand, instructionOperand, scaleNumbers[scale], 1 << scale, NumberKind.INT32,
						FormatterTextKind.NUMBER);
			}
		}

		if (symbol != null) {
			if (needPlus) {
				if (options.getSpaceBetweenMemoryAddOperators())
					output.write(" ", FormatterTextKind.TEXT);
				if ((symbol.flags & SymbolFlags.SIGNED) != 0)
					output.write("-", FormatterTextKind.OPERATOR);
				else
					output.write("+", FormatterTextKind.OPERATOR);
				if (options.getSpaceBetweenMemoryAddOperators())
					output.write(" ", FormatterTextKind.TEXT);
			}
			else if ((symbol.flags & SymbolFlags.SIGNED) != 0)
				output.write("-", FormatterTextKind.OPERATOR);

			com.github.icedland.iced.x86.internal.fmt.FormatterOutputExt.write(output, instruction, operand, instructionOperand, options,
					numberFormatter, numberOptions, absAddr, symbol, options.getShowSymbolAddress(), false,
					options.getSpaceBetweenMemoryAddOperators());
		}
		else if (!needPlus || (displSize != 0 && (options.getShowZeroDisplacements() || displ != 0))) {
			long origDispl = displ;
			boolean isSigned;
			if (needPlus) {
				isSigned = numberOptions.signedNumber;
				if (options.getSpaceBetweenMemoryAddOperators())
					output.write(" ", FormatterTextKind.TEXT);

				if (addrSize == 8) {
					if (!numberOptions.signedNumber)
						output.write("+", FormatterTextKind.OPERATOR);
					else if (displ < 0) {
						displ = -displ;
						output.write("-", FormatterTextKind.OPERATOR);
					}
					else
						output.write("+", FormatterTextKind.OPERATOR);
					if (numberOptions.displacementLeadingZeros) {
						displSize = 4;
					}
				}
				else if (addrSize == 4) {
					if (!numberOptions.signedNumber)
						output.write("+", FormatterTextKind.OPERATOR);
					else if ((int)displ < 0) {
						displ = ((long)-(int)displ) & 0xFFFF_FFFFL;
						output.write("-", FormatterTextKind.OPERATOR);
					}
					else
						output.write("+", FormatterTextKind.OPERATOR);
					if (numberOptions.displacementLeadingZeros) {
						displSize = 4;
					}
				}
				else {
					assert addrSize == 2 : addrSize;
					if (!numberOptions.signedNumber)
						output.write("+", FormatterTextKind.OPERATOR);
					else if ((short)displ < 0) {
						displ = (-(short)displ) & 0xFFFF;
						output.write("-", FormatterTextKind.OPERATOR);
					}
					else
						output.write("+", FormatterTextKind.OPERATOR);
					if (numberOptions.displacementLeadingZeros) {
						displSize = 2;
					}
				}
				if (options.getSpaceBetweenMemoryAddOperators())
					output.write(" ", FormatterTextKind.TEXT);
			}
			else
				isSigned = false;

			int displKind;
			String s;
			if (displSize <= 1 && Long.compareUnsigned(displ, 0xFF) <= 0) {
				s = numberFormatter.formatDisplUInt8(options, numberOptions, (byte)displ);
				displKind = isSigned ? NumberKind.INT8 : NumberKind.UINT8;
			}
			else if (displSize <= 2 && Long.compareUnsigned(displ, 0xFFFF) <= 0) {
				s = numberFormatter.formatDisplUInt16(options, numberOptions, (short)displ);
				displKind = isSigned ? NumberKind.INT16 : NumberKind.UINT16;
			}
			else if (displSize <= 4 && Long.compareUnsigned(displ, 0xFFFF_FFFFL) <= 0) {
				s = numberFormatter.formatDisplUInt32(options, numberOptions, (int)displ);
				displKind = isSigned ? NumberKind.INT32 : NumberKind.UINT32;
			}
			else if (displSize <= 8) {
				s = numberFormatter.formatDisplUInt64(options, numberOptions, displ);
				displKind = isSigned ? NumberKind.INT64 : NumberKind.UINT64;
			}
			else
				throw new UnsupportedOperationException();
			output.writeNumber(instruction, operand, instructionOperand, s, origDispl, displKind, FormatterTextKind.NUMBER);
		}

		if (options.getSpaceAfterMemoryBracket())
			output.write(" ", FormatterTextKind.TEXT);
		output.write("]", FormatterTextKind.PUNCTUATION);

		assert Integer.compareUnsigned(memSize, allMemorySizes.length) < 0 : memSize;
		FormatterString bcstTo = allMemorySizes[memSize].bcstTo;
		if (bcstTo.getLength() != 0)
			formatDecorator(output, instruction, operand, instructionOperand, bcstTo, DecoratorKind.BROADCAST);
		if (instruction.getMvexEvictionHint())
			formatDecorator(output, instruction, operand, instructionOperand, str_eh, DecoratorKind.EVICTION_HINT);
	}

	private void formatMemorySize(FormatterOutput output, int memSize, int flags, FormatterOperandOptions operandOptions) {
		int memSizeOptions = operandOptions.getMemorySizeOptions();
		if (memSizeOptions == MemorySizeOptions.NEVER)
			return;

		if ((flags & InstrOpInfoFlags.MEM_SIZE_NOTHING) != 0)
			return;

		assert Integer.compareUnsigned(memSize, allMemorySizes.length) < 0 : memSize;
		MemorySizes.Info memInfo = allMemorySizes[memSize];
		FormatterString keyword = memInfo.keyword;
		if (keyword.getLength() == 0)
			return;

		if (memSizeOptions == MemorySizeOptions.DEFAULT) {
			if ((flags & InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE) == 0)
				return;
		}
		else if (memSizeOptions == MemorySizeOptions.MINIMAL) {
			if ((flags & InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE) == 0)
				return;
		}
		else
			assert memSizeOptions == MemorySizeOptions.ALWAYS : memSizeOptions;

		FormatterString farKind = farMemSizeInfos[(flags >>> InstrOpInfoFlags.FAR_MEMORY_SIZE_INFO_SHIFT)
				& InstrOpInfoFlags.FAR_MEMORY_SIZE_INFO_MASK];
		if (farKind.getLength() != 0) {
			formatKeyword(output, farKind);
			output.write(" ", FormatterTextKind.TEXT);
		}
		formatKeyword(output, keyword);
		output.write(" ", FormatterTextKind.TEXT);
	}

	private void formatKeyword(FormatterOutput output, FormatterString keyword) {
		output.write(keyword.get(options.getUppercaseKeywords() || options.getUppercaseAll()), FormatterTextKind.KEYWORD);
	}

	/**
	 * Formats a register
	 *
	 * @param register Register (a {@link com.github.icedland.iced.x86.Register} enum variant)
	 */
	@Override
	public String formatRegister(int register) {
		return toRegisterString(register);
	}

	/**
	 * Formats a signed 8-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatInt8(byte value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatInt8(options, numberOptions, value);
	}

	/**
	 * Formats a signed 16-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatInt16(short value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatInt16(options, numberOptions, value);
	}

	/**
	 * Formats a signed 32-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatInt32(int value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatInt32(options, numberOptions, value);
	}

	/**
	 * Formats a signed 64-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatInt64(long value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatInt64(options, numberOptions, value);
	}

	/**
	 * Formats an unsigned 8-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatUInt8(byte value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatUInt8(options, numberOptions, value);
	}

	/**
	 * Formats an unsigned 16-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatUInt16(short value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatUInt16(options, numberOptions, value);
	}

	/**
	 * Formats an unsigned 32-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatUInt32(int value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatUInt32(options, numberOptions, value);
	}

	/**
	 * Formats an unsigned 64-bit integer
	 *
	 * @param value         Value
	 * @param numberOptions Options
	 */
	@Override
	public String formatUInt64(long value, NumberFormattingOptions numberOptions) {
		return numberFormatter.formatUInt64(options, numberOptions, value);
	}
}
