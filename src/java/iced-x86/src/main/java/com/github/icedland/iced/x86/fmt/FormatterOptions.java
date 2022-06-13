// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.internal.IcedConstants;

/**
 * Formatter options
 */
public final class FormatterOptions {
	private static final class Flags1 {
		static final int UPPERCASE_PREFIXES = 0x0000_0001;
		static final int UPPERCASE_MNEMONICS = 0x0000_0002;
		static final int UPPERCASE_REGISTERS = 0x0000_0004;
		static final int UPPERCASE_KEYWORDS = 0x0000_0008;
		static final int UPPERCASE_DECORATORS = 0x0000_0010;
		static final int UPPERCASE_ALL = 0x0000_0020;
		static final int SPACE_AFTER_OPERAND_SEPARATOR = 0x0000_0040;
		static final int SPACE_AFTER_MEMORY_BRACKET = 0x0000_0080;
		static final int SPACE_BETWEEN_MEMORY_ADD_OPERATORS = 0x0000_0100;
		static final int SPACE_BETWEEN_MEMORY_MUL_OPERATORS = 0x0000_0200;
		static final int SCALE_BEFORE_INDEX = 0x0000_0400;
		static final int ALWAYS_SHOW_SCALE = 0x0000_0800;
		static final int ALWAYS_SHOW_SEGMENT_REGISTER = 0x0000_1000;
		static final int SHOW_ZERO_DISPLACEMENTS = 0x0000_2000;
		static final int LEADING_ZEROS = 0x0000_4000;
		static final int UPPERCASE_HEX = 0x0000_8000;
		static final int SMALL_HEX_NUMBERS_IN_DECIMAL = 0x0001_0000;
		static final int ADD_LEADING_ZERO_TO_HEX_NUMBERS = 0x0002_0000;
		static final int BRANCH_LEADING_ZEROS = 0x0004_0000;
		static final int SIGNED_IMMEDIATE_OPERANDS = 0x0008_0000;
		static final int SIGNED_MEMORY_DISPLACEMENTS = 0x0010_0000;
		static final int DISPLACEMENT_LEADING_ZEROS = 0x0020_0000;
		static final int RIP_RELATIVE_ADDRESSES = 0x0040_0000;
		static final int SHOW_BRANCH_SIZE = 0x0080_0000;
		static final int USE_PSEUDO_OPS = 0x0100_0000;
		static final int SHOW_SYMBOL_ADDRESS = 0x0200_0000;
		static final int GAS_NAKED_REGISTERS = 0x0400_0000;
		static final int GAS_SHOW_MNEMONIC_SIZE_SUFFIX = 0x0800_0000;
		static final int GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA = 0x1000_0000;
		static final int MASM_ADD_DS_PREFIX32 = 0x2000_0000;
		static final int MASM_SYMBOL_DISPL_IN_BRACKETS = 0x4000_0000;
		static final int MASM_DISPL_IN_BRACKETS = 0x8000_0000;
	}

	private static final class Flags2 {
		static final int NONE = 0;
		static final int NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE = 0x0000_0001;
		static final int PREFER_ST0 = 0x0000_0002;
		static final int SHOW_USELESS_PREFIXES = 0x0000_0004;
	}

	private int flags1;
	private int flags2;

	/**
	 * Constructor
	 */
	public FormatterOptions() {
		flags1 = Flags1.UPPERCASE_HEX | Flags1.SMALL_HEX_NUMBERS_IN_DECIMAL |
				Flags1.ADD_LEADING_ZERO_TO_HEX_NUMBERS | Flags1.BRANCH_LEADING_ZEROS |
				Flags1.SIGNED_MEMORY_DISPLACEMENTS | Flags1.SHOW_BRANCH_SIZE |
				Flags1.USE_PSEUDO_OPS | Flags1.MASM_ADD_DS_PREFIX32 |
				Flags1.MASM_SYMBOL_DISPL_IN_BRACKETS | Flags1.MASM_DISPL_IN_BRACKETS;
		flags2 = Flags2.NONE;
	}

	/**
	 * Prefixes are uppercased
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>REP stosd</code>
	 * <p>
	 * <code>false</code>: <code>rep stosd</code>
	 */
	public boolean getUppercasePrefixes() {
		return (flags1 & Flags1.UPPERCASE_PREFIXES) != 0;
	}

	/**
	 * Prefixes are uppercased
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>REP stosd</code>
	 * <p>
	 * <code>false</code>: <code>rep stosd</code>
	 */
	public void setUppercasePrefixes(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_PREFIXES;
		else
			flags1 &= ~Flags1.UPPERCASE_PREFIXES;
	}

	/**
	 * Mnemonics are uppercased
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>MOV rcx,rax</code>
	 * <p>
	 * <code>false</code>: <code>mov rcx,rax</code>
	 */
	public boolean getUppercaseMnemonics() {
		return (flags1 & Flags1.UPPERCASE_MNEMONICS) != 0;
	}

	/**
	 * Mnemonics are uppercased
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>MOV rcx,rax</code>
	 * <p>
	 * <code>false</code>: <code>mov rcx,rax</code>
	 */
	public void setUppercaseMnemonics(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_MNEMONICS;
		else
			flags1 &= ~Flags1.UPPERCASE_MNEMONICS;
	}

	/**
	 * Registers are uppercased
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov RCX,[RAX+RDX*8]</code>
	 * <p>
	 * <code>false</code>: <code>mov rcx,[rax+rdx*8]</code>
	 */
	public boolean getUppercaseRegisters() {
		return (flags1 & Flags1.UPPERCASE_REGISTERS) != 0;
	}

	/**
	 * Registers are uppercased
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov RCX,[RAX+RDX*8]</code>
	 * <p>
	 * <code>false</code>: <code>mov rcx,[rax+rdx*8]</code>
	 */
	public void setUppercaseRegisters(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_REGISTERS;
		else
			flags1 &= ~Flags1.UPPERCASE_REGISTERS;
	}

	/**
	 * Keywords are uppercased (eg.<!-- --> <code>BYTE PTR</code>, <code>SHORT</code>)
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov BYTE PTR [rcx],12h</code>
	 * <p>
	 * <code>false</code>: <code>mov byte ptr [rcx],12h</code>
	 */
	public boolean getUppercaseKeywords() {
		return (flags1 & Flags1.UPPERCASE_KEYWORDS) != 0;
	}

	/**
	 * Keywords are uppercased (eg.<!-- --> <code>BYTE PTR</code>, <code>SHORT</code>)
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov BYTE PTR [rcx],12h</code>
	 * <p>
	 * <code>false</code>: <code>mov byte ptr [rcx],12h</code>
	 */
	public void setUppercaseKeywords(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_KEYWORDS;
		else
			flags1 &= ~Flags1.UPPERCASE_KEYWORDS;
	}

	/**
	 * Uppercase decorators, eg.<!-- --> <code>{z}</code>, <code>{sae}</code>, <code>{rd-sae}</code> (but not opmask registers: <code>{k1}</code>)
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]</code>
	 * <p>
	 * <code>false</code>: <code>vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]</code>
	 */
	public boolean getUppercaseDecorators() {
		return (flags1 & Flags1.UPPERCASE_DECORATORS) != 0;
	}

	/**
	 * Uppercase decorators, eg.<!-- --> <code>{z}</code>, <code>{sae}</code>, <code>{rd-sae}</code> (but not opmask registers: <code>{k1}</code>)
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]</code>
	 * <p>
	 * <code>false</code>: <code>vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]</code>
	 */
	public void setUppercaseDecorators(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_DECORATORS;
		else
			flags1 &= ~Flags1.UPPERCASE_DECORATORS;
	}

	/**
	 * Everything is uppercased, except numbers and their prefixes/suffixes
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>MOV EAX,GS:[RCX*4+0ffh]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,gs:[rcx*4+0ffh]</code>
	 */
	public boolean getUppercaseAll() {
		return (flags1 & Flags1.UPPERCASE_ALL) != 0;
	}

	/**
	 * Everything is uppercased, except numbers and their prefixes/suffixes
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>MOV EAX,GS:[RCX*4+0ffh]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,gs:[rcx*4+0ffh]</code>
	 */
	public void setUppercaseAll(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_ALL;
		else
			flags1 &= ~Flags1.UPPERCASE_ALL;
	}

	/**
	 * Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	 * At least one space or tab is always added between the mnemonic and the first operand.
	 * <p>
	 * Default: <code>0</code>
	 * <p>
	 * <code>0</code>: <code>mov•rcx,rbp</code>
	 * <p>
	 * <code>8</code>: <code>mov•••••rcx,rbp</code>
	 */
	public int getFirstOperandCharIndex() {
		return firstOperandCharIndex;
	}

	/**
	 * Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	 * At least one space or tab is always added between the mnemonic and the first operand.
	 * <p>
	 * Default: <code>0</code>
	 * <p>
	 * <code>0</code>: <code>mov•rcx,rbp</code>
	 * <p>
	 * <code>8</code>: <code>mov•••••rcx,rbp</code>
	 */
	public void setFirstOperandCharIndex(int value) {
		firstOperandCharIndex = value;
	}

	private int firstOperandCharIndex;

	/**
	 * Size of a tab character or &lt;= 0 to use spaces
	 * <p>
	 * Default: <code>0</code>
	 */
	public int getTabSize() {
		return tabSize;
	}

	/**
	 * Size of a tab character or &lt;= 0 to use spaces
	 * <p>
	 * Default: <code>0</code>
	 */
	public void setTabSize(int value) {
		tabSize = value;
	}

	private int tabSize;

	/**
	 * Add a space after the operand separator
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov rax, rcx</code>
	 * <p>
	 * <code>false</code>: <code>mov rax,rcx</code>
	 */
	public boolean getSpaceAfterOperandSeparator() {
		return (flags1 & Flags1.SPACE_AFTER_OPERAND_SEPARATOR) != 0;
	}

	/**
	 * Add a space after the operand separator
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov rax, rcx</code>
	 * <p>
	 * <code>false</code>: <code>mov rax,rcx</code>
	 */
	public void setSpaceAfterOperandSeparator(boolean value) {
		if (value)
			flags1 |= Flags1.SPACE_AFTER_OPERAND_SEPARATOR;
		else
			flags1 &= ~Flags1.SPACE_AFTER_OPERAND_SEPARATOR;
	}

	/**
	 * Add a space between the memory expression and the brackets
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[ rcx+rdx ]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx+rdx]</code>
	 */
	public boolean getSpaceAfterMemoryBracket() {
		return (flags1 & Flags1.SPACE_AFTER_MEMORY_BRACKET) != 0;
	}

	/**
	 * Add a space between the memory expression and the brackets
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[ rcx+rdx ]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx+rdx]</code>
	 */
	public void setSpaceAfterMemoryBracket(boolean value) {
		if (value)
			flags1 |= Flags1.SPACE_AFTER_MEMORY_BRACKET;
		else
			flags1 &= ~Flags1.SPACE_AFTER_MEMORY_BRACKET;
	}

	/**
	 * Add spaces between memory operand <code>+</code> and <code>-</code> operators
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rcx + rdx*8 - 80h]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx+rdx*8-80h]</code>
	 */
	public boolean getSpaceBetweenMemoryAddOperators() {
		return (flags1 & Flags1.SPACE_BETWEEN_MEMORY_ADD_OPERATORS) != 0;
	}

	/**
	 * Add spaces between memory operand <code>+</code> and <code>-</code> operators
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rcx + rdx*8 - 80h]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx+rdx*8-80h]</code>
	 */
	public void setSpaceBetweenMemoryAddOperators(boolean value) {
		if (value)
			flags1 |= Flags1.SPACE_BETWEEN_MEMORY_ADD_OPERATORS;
		else
			flags1 &= ~Flags1.SPACE_BETWEEN_MEMORY_ADD_OPERATORS;
	}

	/**
	 * Add spaces between memory operand <code>*</code> operator
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rcx+rdx * 8-80h]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx+rdx*8-80h]</code>
	 */
	public boolean getSpaceBetweenMemoryMulOperators() {
		return (flags1 & Flags1.SPACE_BETWEEN_MEMORY_MUL_OPERATORS) != 0;
	}

	/**
	 * Add spaces between memory operand <code>*</code> operator
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rcx+rdx * 8-80h]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx+rdx*8-80h]</code>
	 */
	public void setSpaceBetweenMemoryMulOperators(boolean value) {
		if (value)
			flags1 |= Flags1.SPACE_BETWEEN_MEMORY_MUL_OPERATORS;
		else
			flags1 &= ~Flags1.SPACE_BETWEEN_MEMORY_MUL_OPERATORS;
	}

	/**
	 * Show memory operand scale value before the index register
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[8*rdx]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rdx*8]</code>
	 */
	public boolean getScaleBeforeIndex() {
		return (flags1 & Flags1.SCALE_BEFORE_INDEX) != 0;
	}

	/**
	 * Show memory operand scale value before the index register
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[8*rdx]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rdx*8]</code>
	 */
	public void setScaleBeforeIndex(boolean value) {
		if (value)
			flags1 |= Flags1.SCALE_BEFORE_INDEX;
		else
			flags1 &= ~Flags1.SCALE_BEFORE_INDEX;
	}

	/**
	 * Always show the scale value even if it's <code>*1</code>
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rbx+rcx*1]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rbx+rcx]</code>
	 */
	public boolean getAlwaysShowScale() {
		return (flags1 & Flags1.ALWAYS_SHOW_SCALE) != 0;
	}

	/**
	 * Always show the scale value even if it's <code>*1</code>
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rbx+rcx*1]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rbx+rcx]</code>
	 */
	public void setAlwaysShowScale(boolean value) {
		if (value)
			flags1 |= Flags1.ALWAYS_SHOW_SCALE;
		else
			flags1 &= ~Flags1.ALWAYS_SHOW_SCALE;
	}

	/**
	 * Always show the effective segment register. If the option is <code>false</code>, only show the segment register if
	 * there's a segment override prefix.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,ds:[ecx]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[ecx]</code>
	 */
	public boolean getAlwaysShowSegmentRegister() {
		return (flags1 & Flags1.ALWAYS_SHOW_SEGMENT_REGISTER) != 0;
	}

	/**
	 * Always show the effective segment register. If the option is <code>false</code>, only show the segment register if
	 * there's a segment override prefix.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,ds:[ecx]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[ecx]</code>
	 */
	public void setAlwaysShowSegmentRegister(boolean value) {
		if (value)
			flags1 |= Flags1.ALWAYS_SHOW_SEGMENT_REGISTER;
		else
			flags1 &= ~Flags1.ALWAYS_SHOW_SEGMENT_REGISTER;
	}

	/**
	 * Show zero displacements
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rcx*2+0]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx*2]</code>
	 */
	public boolean getShowZeroDisplacements() {
		return (flags1 & Flags1.SHOW_ZERO_DISPLACEMENTS) != 0;
	}

	/**
	 * Show zero displacements
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rcx*2+0]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[rcx*2]</code>
	 */
	public void setShowZeroDisplacements(boolean value) {
		if (value)
			flags1 |= Flags1.SHOW_ZERO_DISPLACEMENTS;
		else
			flags1 &= ~Flags1.SHOW_ZERO_DISPLACEMENTS;
	}

	/**
	 * Hex number prefix or <code>null</code>/empty string, eg.<!-- --> "0x"
	 * <p>
	 * Default: <code>null</code> (masm/nasm/intel), <code>"0x"</code> (gas)
	 */
	public String getHexPrefix() {
		return hexPrefix;
	}

	/**
	 * Hex number prefix or <code>null</code>/empty string, eg.<!-- --> "0x"
	 * <p>
	 * Default: <code>null</code> (masm/nasm/intel), <code>"0x"</code> (gas)
	 */
	public void setHexPrefix(String value) {
		hexPrefix = value;
	}

	private String hexPrefix;

	/**
	 * Hex number suffix or <code>null</code>/empty string, eg.<!-- --> "h"
	 * <p>
	 * Default: <code>"h"</code> (masm/nasm/intel), <code>null</code> (gas)
	 */
	public String getHexSuffix() {
		return hexSuffix;
	}

	/**
	 * Hex number suffix or <code>null</code>/empty string, eg.<!-- --> "h"
	 * <p>
	 * Default: <code>"h"</code> (masm/nasm/intel), <code>null</code> (gas)
	 */
	public void setHexSuffix(String value) {
		hexSuffix = value;
	}

	private String hexSuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>4</code>
	 * <p>
	 * <code>0</code>: <code>0x12345678</code>
	 * <p>
	 * <code>4</code>: <code>0x1234_5678</code>
	 */
	public int getHexDigitGroupSize() {
		return hexDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>4</code>
	 * <p>
	 * <code>0</code>: <code>0x12345678</code>
	 * <p>
	 * <code>4</code>: <code>0x1234_5678</code>
	 */
	public void setHexDigitGroupSize(int value) {
		hexDigitGroupSize = value;
	}

	private int hexDigitGroupSize = 4;

	/**
	 * Decimal number prefix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code>
	 */
	public String getDecimalPrefix() {
		return decimalPrefix;
	}

	/**
	 * Decimal number prefix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code>
	 */
	public void setDecimalPrefix(String value) {
		decimalPrefix = value;
	}

	private String decimalPrefix;

	/**
	 * Decimal number suffix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code>
	 */
	public String getDecimalSuffix() {
		return decimalSuffix;
	}

	/**
	 * Decimal number suffix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code>
	 */
	public void setDecimalSuffix(String value) {
		decimalSuffix = value;
	}

	private String decimalSuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>3</code>
	 * <p>
	 * <code>0</code>: <code>12345678</code>
	 * <p>
	 * <code>3</code>: <code>12_345_678</code>
	 */
	public int getDecimalDigitGroupSize() {
		return decimalDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>3</code>
	 * <p>
	 * <code>0</code>: <code>12345678</code>
	 * <p>
	 * <code>3</code>: <code>12_345_678</code>
	 */
	public void setDecimalDigitGroupSize(int value) {
		decimalDigitGroupSize = value;
	}

	private int decimalDigitGroupSize = 3;

	/**
	 * Octal number prefix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code> (masm/nasm/intel), <code>"0"</code> (gas)
	 */
	public String getOctalPrefix() {
		return octalPrefix;
	}

	/**
	 * Octal number prefix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code> (masm/nasm/intel), <code>"0"</code> (gas)
	 */
	public void setOctalPrefix(String value) {
		octalPrefix = value;
	}

	private String octalPrefix;

	/**
	 * Octal number suffix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>"o"</code> (masm/nasm/intel), <code>null</code> (gas)
	 */
	public String getOctalSuffix() {
		return octalSuffix;
	}

	/**
	 * Octal number suffix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>"o"</code> (masm/nasm/intel), <code>null</code> (gas)
	 */
	public void setOctalSuffix(String value) {
		octalSuffix = value;
	}

	private String octalSuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>4</code>
	 * <p>
	 * <code>0</code>: <code>12345670</code>
	 * <p>
	 * <code>4</code>: <code>1234_5670</code>
	 */
	public int getOctalDigitGroupSize() {
		return octalDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>4</code>
	 * <p>
	 * <code>0</code>: <code>12345670</code>
	 * <p>
	 * <code>4</code>: <code>1234_5670</code>
	 */
	public void setOctalDigitGroupSize(int value) {
		octalDigitGroupSize = value;
	}

	private int octalDigitGroupSize = 4;

	/**
	 * Binary number prefix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code> (masm/nasm/intel), <code>"0b"</code> (gas)
	 */
	public String getBinaryPrefix() {
		return binaryPrefix;
	}

	/**
	 * Binary number prefix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>null</code> (masm/nasm/intel), <code>"0b"</code> (gas)
	 */
	public void setBinaryPrefix(String value) {
		binaryPrefix = value;
	}

	private String binaryPrefix;

	/**
	 * Binary number suffix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>"b"</code> (masm/nasm/intel), <code>null</code> (gas)
	 */
	public String getBinarySuffix() {
		return binarySuffix;
	}

	/**
	 * Binary number suffix or <code>null</code>/empty string
	 * <p>
	 * Default: <code>"b"</code> (masm/nasm/intel), <code>null</code> (gas)
	 */
	public void setBinarySuffix(String value) {
		binarySuffix = value;
	}

	private String binarySuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>4</code>
	 * <p>
	 * <code>0</code>: <code>11010111</code>
	 * <p>
	 * <code>4</code>: <code>1101_0111</code>
	 */
	public int getBinaryDigitGroupSize() {
		return binaryDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: <code>4</code>
	 * <p>
	 * <code>0</code>: <code>11010111</code>
	 * <p>
	 * <code>4</code>: <code>1101_0111</code>
	 */
	public void setBinaryDigitGroupSize(int value) {
		binaryDigitGroupSize = value;
	}

	private int binaryDigitGroupSize = 4;

	/**
	 * Digit separator or <code>null</code>/empty string. See also eg. {@link #getHexDigitGroupSize()}.
	 * <p>
	 * Default: <code>null</code>
	 * <p>
	 * <code>""</code>: <code>0x12345678</code>
	 * <p>
	 * <code>"_"</code>: <code>0x1234_5678</code>
	 */
	public String getDigitSeparator() {
		return digitSeparator;
	}

	/**
	 * Digit separator or <code>null</code>/empty string. See also eg. {@link #getHexDigitGroupSize()}.
	 * <p>
	 * Default: <code>null</code>
	 * <p>
	 * <code>""</code>: <code>0x12345678</code>
	 * <p>
	 * <code>"_"</code>: <code>0x1234_5678</code>
	 */
	public void setDigitSeparator(String value) {
		digitSeparator = value;
	}

	private String digitSeparator;

	/**
	 * Add leading zeros to hexadecimal/octal/binary numbers.
	 * This option has no effect on branch targets and displacements, use {@link #getBranchLeadingZeros()}
	 * and {@link #getDisplacementLeadingZeros()}.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>0x0000000A</code>/<code>0000000Ah</code>
	 * <p>
	 * <code>false</code>: <code>0xA</code>/<code>0Ah</code>
	 */
	public boolean getLeadingZeros() {
		return (flags1 & Flags1.LEADING_ZEROS) != 0;
	}

	/**
	 * Add leading zeros to hexadecimal/octal/binary numbers.
	 * This option has no effect on branch targets and displacements, use {@link #getBranchLeadingZeros()}
	 * and {@link #getDisplacementLeadingZeros()}.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>0x0000000A</code>/<code>0000000Ah</code>
	 * <p>
	 * <code>false</code>: <code>0xA</code>/<code>0Ah</code>
	 */
	public void setLeadingZeros(boolean value) {
		if (value)
			flags1 |= Flags1.LEADING_ZEROS;
		else
			flags1 &= ~Flags1.LEADING_ZEROS;
	}

	/**
	 * Use uppercase hex digits
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>0xFF</code>
	 * <p>
	 * <code>false</code>: <code>0xff</code>
	 */
	public boolean getUppercaseHex() {
		return (flags1 & Flags1.UPPERCASE_HEX) != 0;
	}

	/**
	 * Use uppercase hex digits
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>0xFF</code>
	 * <p>
	 * <code>false</code>: <code>0xff</code>
	 */
	public void setUppercaseHex(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_HEX;
		else
			flags1 &= ~Flags1.UPPERCASE_HEX;
	}

	/**
	 * Small hex numbers (-9 .<!-- -->.<!-- --> 9) are shown in decimal
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>9</code>
	 * <p>
	 * <code>false</code>: <code>0x9</code>
	 */
	public boolean getSmallHexNumbersInDecimal() {
		return (flags1 & Flags1.SMALL_HEX_NUMBERS_IN_DECIMAL) != 0;
	}

	/**
	 * Small hex numbers (-9 .<!-- -->.<!-- --> 9) are shown in decimal
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>9</code>
	 * <p>
	 * <code>false</code>: <code>0x9</code>
	 */
	public void setSmallHexNumbersInDecimal(boolean value) {
		if (value)
			flags1 |= Flags1.SMALL_HEX_NUMBERS_IN_DECIMAL;
		else
			flags1 &= ~Flags1.SMALL_HEX_NUMBERS_IN_DECIMAL;
	}

	/**
	 * Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits <code>A-F</code>
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>0FFh</code>
	 * <p>
	 * <code>false</code>: <code>FFh</code>
	 */
	public boolean getAddLeadingZeroToHexNumbers() {
		return (flags1 & Flags1.ADD_LEADING_ZERO_TO_HEX_NUMBERS) != 0;
	}

	/**
	 * Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits <code>A-F</code>
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>0FFh</code>
	 * <p>
	 * <code>false</code>: <code>FFh</code>
	 */
	public void setAddLeadingZeroToHexNumbers(boolean value) {
		if (value)
			flags1 |= Flags1.ADD_LEADING_ZERO_TO_HEX_NUMBERS;
		else
			flags1 &= ~Flags1.ADD_LEADING_ZERO_TO_HEX_NUMBERS;
	}

	/**
	 * Number base (a {@link NumberBase} enum variant)
	 * <p>
	 * Default: {@link NumberBase#HEXADECIMAL}
	 */
	public int getNumberBase() {
		return numberBase;
	}

	/**
	 * Number base (a {@link NumberBase} enum variant)
	 * <p>
	 * Default: {@link NumberBase#HEXADECIMAL}
	 */
	public void setNumberBase(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.NUMBER_BASE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		numberBase = value;
	}

	private int numberBase = NumberBase.HEXADECIMAL;

	/**
	 * Add leading zeros to branch offsets. Used by <code>CALL NEAR</code>, <code>CALL FAR</code>, <code>JMP NEAR</code>, <code>JMP FAR</code>,
	 * <code>Jcc</code>, <code>LOOP</code>, <code>LOOPcc</code>, <code>XBEGIN</code>
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>je 00000123h</code>
	 * <p>
	 * <code>false</code>: <code>je 123h</code>
	 */
	public boolean getBranchLeadingZeros() {
		return (flags1 & Flags1.BRANCH_LEADING_ZEROS) != 0;
	}

	/**
	 * Add leading zeros to branch offsets. Used by <code>CALL NEAR</code>, <code>CALL FAR</code>, <code>JMP NEAR</code>, <code>JMP FAR</code>,
	 * <code>Jcc</code>, <code>LOOP</code>, <code>LOOPcc</code>, <code>XBEGIN</code>
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>je 00000123h</code>
	 * <p>
	 * <code>false</code>: <code>je 123h</code>
	 */
	public void setBranchLeadingZeros(boolean value) {
		if (value)
			flags1 |= Flags1.BRANCH_LEADING_ZEROS;
		else
			flags1 &= ~Flags1.BRANCH_LEADING_ZEROS;
	}

	/**
	 * Show immediate operands as signed numbers
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,-1</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,FFFFFFFF</code>
	 */
	public boolean getSignedImmediateOperands() {
		return (flags1 & Flags1.SIGNED_IMMEDIATE_OPERANDS) != 0;
	}

	/**
	 * Show immediate operands as signed numbers
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,-1</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,FFFFFFFF</code>
	 */
	public void setSignedImmediateOperands(boolean value) {
		if (value)
			flags1 |= Flags1.SIGNED_IMMEDIATE_OPERANDS;
		else
			flags1 &= ~Flags1.SIGNED_IMMEDIATE_OPERANDS;
	}

	/**
	 * Displacements are signed numbers
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>mov al,[eax-2000h]</code>
	 * <p>
	 * <code>false</code>: <code>mov al,[eax+0FFFFE000h]</code>
	 */
	public boolean getSignedMemoryDisplacements() {
		return (flags1 & Flags1.SIGNED_MEMORY_DISPLACEMENTS) != 0;
	}

	/**
	 * Displacements are signed numbers
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>mov al,[eax-2000h]</code>
	 * <p>
	 * <code>false</code>: <code>mov al,[eax+0FFFFE000h]</code>
	 */
	public void setSignedMemoryDisplacements(boolean value) {
		if (value)
			flags1 |= Flags1.SIGNED_MEMORY_DISPLACEMENTS;
		else
			flags1 &= ~Flags1.SIGNED_MEMORY_DISPLACEMENTS;
	}

	/**
	 * Add leading zeros to displacements
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov al,[eax+00000012h]</code>
	 * <p>
	 * <code>false</code>: <code>mov al,[eax+12h]</code>
	 */
	public boolean getDisplacementLeadingZeros() {
		return (flags1 & Flags1.DISPLACEMENT_LEADING_ZEROS) != 0;
	}

	/**
	 * Add leading zeros to displacements
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov al,[eax+00000012h]</code>
	 * <p>
	 * <code>false</code>: <code>mov al,[eax+12h]</code>
	 */
	public void setDisplacementLeadingZeros(boolean value) {
		if (value)
			flags1 |= Flags1.DISPLACEMENT_LEADING_ZEROS;
		else
			flags1 &= ~Flags1.DISPLACEMENT_LEADING_ZEROS;
	}

	/**
	 * Options that control if the memory size (eg.<!-- --> <code>DWORD PTR</code>) is shown or not.
	 * This is ignored by the gas (AT&amp;T) formatter (a {@link MemorySizeOptions} enum variant).
	 * <p>
	 * Default: {@link MemorySizeOptions#DEFAULT}
	 */
	public int getMemorySizeOptions() {
		return memorySizeOptions;
	}

	/**
	 * Options that control if the memory size (eg.<!-- --> <code>DWORD PTR</code>) is shown or not.
	 * This is ignored by the gas (AT&amp;T) formatter (a {@link MemorySizeOptions} enum variant).
	 * <p>
	 * Default: {@link MemorySizeOptions#DEFAULT}
	 */
	public void setMemorySizeOptions(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.MEMORY_SIZE_OPTIONS_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		memorySizeOptions = value;
	}

	private int memorySizeOptions = MemorySizeOptions.DEFAULT;

	/**
	 * Show <code>RIP+displ</code> or the virtual address
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rip+12345678h]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[1029384756AFBECDh]</code>
	 */
	public boolean getRipRelativeAddresses() {
		return (flags1 & Flags1.RIP_RELATIVE_ADDRESSES) != 0;
	}

	/**
	 * Show <code>RIP+displ</code> or the virtual address
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[rip+12345678h]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[1029384756AFBECDh]</code>
	 */
	public void setRipRelativeAddresses(boolean value) {
		if (value)
			flags1 |= Flags1.RIP_RELATIVE_ADDRESSES;
		else
			flags1 &= ~Flags1.RIP_RELATIVE_ADDRESSES;
	}

	/**
	 * Show <code>NEAR</code>, <code>SHORT</code>, etc if it's a branch instruction
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>je short 1234h</code>
	 * <p>
	 * <code>false</code>: <code>je 1234h</code>
	 */
	public boolean getShowBranchSize() {
		return (flags1 & Flags1.SHOW_BRANCH_SIZE) != 0;
	}

	/**
	 * Show <code>NEAR</code>, <code>SHORT</code>, etc if it's a branch instruction
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>je short 1234h</code>
	 * <p>
	 * <code>false</code>: <code>je 1234h</code>
	 */
	public void setShowBranchSize(boolean value) {
		if (value)
			flags1 |= Flags1.SHOW_BRANCH_SIZE;
		else
			flags1 &= ~Flags1.SHOW_BRANCH_SIZE;
	}

	/**
	 * Use pseudo instructions
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>vcmpnltsd xmm2,xmm6,xmm3</code>
	 * <p>
	 * <code>false</code>: <code>vcmpsd xmm2,xmm6,xmm3,5</code>
	 */
	public boolean getUsePseudoOps() {
		return (flags1 & Flags1.USE_PSEUDO_OPS) != 0;
	}

	/**
	 * Use pseudo instructions
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>vcmpnltsd xmm2,xmm6,xmm3</code>
	 * <p>
	 * <code>false</code>: <code>vcmpsd xmm2,xmm6,xmm3,5</code>
	 */
	public void setUsePseudoOps(boolean value) {
		if (value)
			flags1 |= Flags1.USE_PSEUDO_OPS;
		else
			flags1 &= ~Flags1.USE_PSEUDO_OPS;
	}

	/**
	 * Show the original value after the symbol name
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[myfield (12345678)]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[myfield]</code>
	 */
	public boolean getShowSymbolAddress() {
		return (flags1 & Flags1.SHOW_SYMBOL_ADDRESS) != 0;
	}

	/**
	 * Show the original value after the symbol name
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,[myfield (12345678)]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[myfield]</code>
	 */
	public void setShowSymbolAddress(boolean value) {
		if (value)
			flags1 |= Flags1.SHOW_SYMBOL_ADDRESS;
		else
			flags1 &= ~Flags1.SHOW_SYMBOL_ADDRESS;
	}

	/**
	 * (gas only): If <code>true</code>, the formatter doesn't add <code>%</code> to registers
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,ecx</code>
	 * <p>
	 * <code>false</code>: <code>mov %eax,%ecx</code>
	 */
	public boolean getGasNakedRegisters() {
		return (flags1 & Flags1.GAS_NAKED_REGISTERS) != 0;
	}

	/**
	 * (gas only): If <code>true</code>, the formatter doesn't add <code>%</code> to registers
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,ecx</code>
	 * <p>
	 * <code>false</code>: <code>mov %eax,%ecx</code>
	 */
	public void setGasNakedRegisters(boolean value) {
		if (value)
			flags1 |= Flags1.GAS_NAKED_REGISTERS;
		else
			flags1 &= ~Flags1.GAS_NAKED_REGISTERS;
	}

	/**
	 * (gas only): Shows the mnemonic size suffix even when not needed
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>movl %eax,%ecx</code>
	 * <p>
	 * <code>false</code>: <code>mov %eax,%ecx</code>
	 */
	public boolean getGasShowMnemonicSizeSuffix() {
		return (flags1 & Flags1.GAS_SHOW_MNEMONIC_SIZE_SUFFIX) != 0;
	}

	/**
	 * (gas only): Shows the mnemonic size suffix even when not needed
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>movl %eax,%ecx</code>
	 * <p>
	 * <code>false</code>: <code>mov %eax,%ecx</code>
	 */
	public void setGasShowMnemonicSizeSuffix(boolean value) {
		if (value)
			flags1 |= Flags1.GAS_SHOW_MNEMONIC_SIZE_SUFFIX;
		else
			flags1 &= ~Flags1.GAS_SHOW_MNEMONIC_SIZE_SUFFIX;
	}

	/**
	 * (gas only): Add a space after the comma if it's a memory operand
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>(%eax, %ecx, 2)</code>
	 * <p>
	 * <code>false</code>: <code>(%eax,%ecx,2)</code>
	 */
	public boolean getGasSpaceAfterMemoryOperandComma() {
		return (flags1 & Flags1.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA) != 0;
	}

	/**
	 * (gas only): Add a space after the comma if it's a memory operand
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>(%eax, %ecx, 2)</code>
	 * <p>
	 * <code>false</code>: <code>(%eax,%ecx,2)</code>
	 */
	public void setGasSpaceAfterMemoryOperandComma(boolean value) {
		if (value)
			flags1 |= Flags1.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA;
		else
			flags1 &= ~Flags1.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA;
	}

	/**
	 * (masm only): Add a <code>DS</code> segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,ds:[12345678]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[12345678]</code>
	 */
	public boolean getMasmAddDsPrefix32() {
		return (flags1 & Flags1.MASM_ADD_DS_PREFIX32) != 0;
	}

	/**
	 * (masm only): Add a <code>DS</code> segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,ds:[12345678]</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[12345678]</code>
	 */
	public void setMasmAddDsPrefix32(boolean value) {
		if (value)
			flags1 |= Flags1.MASM_ADD_DS_PREFIX32;
		else
			flags1 &= ~Flags1.MASM_ADD_DS_PREFIX32;
	}

	/**
	 * (masm only): Show symbols in brackets
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>[ecx+symbol]</code> / <code>[symbol]</code>
	 * <p>
	 * <code>false</code>: <code>symbol[ecx]</code> / <code>symbol</code>
	 */
	public boolean getMasmSymbolDisplInBrackets() {
		return (flags1 & Flags1.MASM_SYMBOL_DISPL_IN_BRACKETS) != 0;
	}

	/**
	 * (masm only): Show symbols in brackets
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>[ecx+symbol]</code> / <code>[symbol]</code>
	 * <p>
	 * <code>false</code>: <code>symbol[ecx]</code> / <code>symbol</code>
	 */
	public void setMasmSymbolDisplInBrackets(boolean value) {
		if (value)
			flags1 |= Flags1.MASM_SYMBOL_DISPL_IN_BRACKETS;
		else
			flags1 &= ~Flags1.MASM_SYMBOL_DISPL_IN_BRACKETS;
	}

	/**
	 * (masm only): Show displacements in brackets
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>[ecx+1234h]</code>
	 * <p>
	 * <code>false</code>: <code>1234h[ecx]</code>
	 */
	public boolean getMasmDisplInBrackets() {
		return (flags1 & Flags1.MASM_DISPL_IN_BRACKETS) != 0;
	}

	/**
	 * (masm only): Show displacements in brackets
	 * <p>
	 * Default: <code>true</code>
	 * <p>
	 * <code>true</code>: <code>[ecx+1234h]</code>
	 * <p>
	 * <code>false</code>: <code>1234h[ecx]</code>
	 */
	public void setMasmDisplInBrackets(boolean value) {
		if (value)
			flags1 |= Flags1.MASM_DISPL_IN_BRACKETS;
		else
			flags1 &= ~Flags1.MASM_DISPL_IN_BRACKETS;
	}

	/**
	 * (nasm only): Shows <code>BYTE</code>, <code>WORD</code>, <code>DWORD</code> or <code>QWORD</code> if it's a sign extended immediate operand
	 * value
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>or rcx,byte -1</code>
	 * <p>
	 * <code>false</code>: <code>or rcx,-1</code>
	 */
	public boolean getNasmShowSignExtendedImmediateSize() {
		return (flags2 & Flags2.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE) != 0;
	}

	/**
	 * (nasm only): Shows <code>BYTE</code>, <code>WORD</code>, <code>DWORD</code> or <code>QWORD</code> if it's a sign extended immediate operand
	 * value
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>or rcx,byte -1</code>
	 * <p>
	 * <code>false</code>: <code>or rcx,-1</code>
	 */
	public void setNasmShowSignExtendedImmediateSize(boolean value) {
		if (value)
			flags2 |= Flags2.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE;
		else
			flags2 &= ~Flags2.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE;
	}

	/**
	 * Use <code>st(0)</code> instead of <code>st</code> if <code>st</code> can be used. Ignored by the nasm formatter.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>fadd st(0),st(3)</code>
	 * <p>
	 * <code>false</code>: <code>fadd st,st(3)</code>
	 */
	public boolean getPreferST0() {
		return (flags2 & Flags2.PREFER_ST0) != 0;
	}

	/**
	 * Use <code>st(0)</code> instead of <code>st</code> if <code>st</code> can be used. Ignored by the nasm formatter.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>fadd st(0),st(3)</code>
	 * <p>
	 * <code>false</code>: <code>fadd st,st(3)</code>
	 */
	public void setPreferST0(boolean value) {
		if (value)
			flags2 |= Flags2.PREFER_ST0;
		else
			flags2 &= ~Flags2.PREFER_ST0;
	}

	/**
	 * Show useless prefixes. If it has useless prefixes, it could be data and not code.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>es rep add eax,ecx</code>
	 * <p>
	 * <code>false</code>: <code>add eax,ecx</code>
	 */
	public boolean getShowUselessPrefixes() {
		return (flags2 & Flags2.SHOW_USELESS_PREFIXES) != 0;
	}

	/**
	 * Show useless prefixes. If it has useless prefixes, it could be data and not code.
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>es rep add eax,ecx</code>
	 * <p>
	 * <code>false</code>: <code>add eax,ecx</code>
	 */
	public void setShowUselessPrefixes(boolean value) {
		if (value)
			flags2 |= Flags2.SHOW_USELESS_PREFIXES;
		else
			flags2 &= ~Flags2.SHOW_USELESS_PREFIXES;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JB</code> / <code>JC</code> / <code>JNAE</code>)
	 * <p>
	 * Default: <code>JB</code>, <code>CMOVB</code>, <code>SETB</code>
	 */
	public int getCC_b() {
		return cc_b;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JB</code> / <code>JC</code> / <code>JNAE</code>)
	 * <p>
	 * Default: <code>JB</code>, <code>CMOVB</code>, <code>SETB</code>
	 */
	public void setCC_b(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_B_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_b = value;
	}

	private int cc_b = CC_b.B;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JAE</code> / <code>JNB</code> / <code>JNC</code>)
	 * <p>
	 * Default: <code>JAE</code>, <code>CMOVAE</code>, <code>SETAE</code>
	 */
	public int getCC_ae() {
		return cc_ae;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JAE</code> / <code>JNB</code> / <code>JNC</code>)
	 * <p>
	 * Default: <code>JAE</code>, <code>CMOVAE</code>, <code>SETAE</code>
	 */
	public void setCC_ae(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_AE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_ae = value;
	}

	private int cc_ae = CC_ae.AE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JE</code> / <code>JZ</code>)
	 * <p>
	 * Default: <code>JE</code>, <code>CMOVE</code>, <code>SETE</code>, <code>LOOPE</code>, <code>REPE</code>
	 */
	public int getCC_e() {
		return cc_e;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JE</code> / <code>JZ</code>)
	 * <p>
	 * Default: <code>JE</code>, <code>CMOVE</code>, <code>SETE</code>, <code>LOOPE</code>, <code>REPE</code>
	 */
	public void setCC_e(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_E_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_e = value;
	}

	private int cc_e = CC_e.E;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JNE</code> / <code>JNZ</code>)
	 * <p>
	 * Default: <code>JNE</code>, <code>CMOVNE</code>, <code>SETNE</code>, <code>LOOPNE</code>, <code>REPNE</code>
	 */
	public int getCC_ne() {
		return cc_ne;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JNE</code> / <code>JNZ</code>)
	 * <p>
	 * Default: <code>JNE</code>, <code>CMOVNE</code>, <code>SETNE</code>, <code>LOOPNE</code>, <code>REPNE</code>
	 */
	public void setCC_ne(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_NE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_ne = value;
	}

	private int cc_ne = CC_ne.NE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JBE</code> / <code>JNA</code>)
	 * <p>
	 * Default: <code>JBE</code>, <code>CMOVBE</code>, <code>SETBE</code>
	 */
	public int getCC_be() {
		return cc_be;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JBE</code> / <code>JNA</code>)
	 * <p>
	 * Default: <code>JBE</code>, <code>CMOVBE</code>, <code>SETBE</code>
	 */
	public void setCC_be(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_BE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_be = value;
	}

	private int cc_be = CC_be.BE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JA</code> / <code>JNBE</code>)
	 * <p>
	 * Default: <code>JA</code>, <code>CMOVA</code>, <code>SETA</code>
	 */
	public int getCC_a() {
		return cc_a;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JA</code> / <code>JNBE</code>)
	 * <p>
	 * Default: <code>JA</code>, <code>CMOVA</code>, <code>SETA</code>
	 */
	public void setCC_a(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_A_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_a = value;
	}

	private int cc_a = CC_a.A;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JP</code> / <code>JPE</code>)
	 * <p>
	 * Default: <code>JP</code>, <code>CMOVP</code>, <code>SETP</code>
	 */
	public int getCC_p() {
		return cc_p;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JP</code> / <code>JPE</code>)
	 * <p>
	 * Default: <code>JP</code>, <code>CMOVP</code>, <code>SETP</code>
	 */
	public void setCC_p(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_P_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_p = value;
	}

	private int cc_p = CC_p.P;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JNP</code> / <code>JPO</code>)
	 * <p>
	 * Default: <code>JNP</code>, <code>CMOVNP</code>, <code>SETNP</code>
	 */
	public int getCC_np() {
		return cc_np;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JNP</code> / <code>JPO</code>)
	 * <p>
	 * Default: <code>JNP</code>, <code>CMOVNP</code>, <code>SETNP</code>
	 */
	public void setCC_np(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_NP_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_np = value;
	}

	private int cc_np = CC_np.NP;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JL</code> / <code>JNGE</code>)
	 * <p>
	 * Default: <code>JL</code>, <code>CMOVL</code>, <code>SETL</code>
	 */
	public int getCC_l() {
		return cc_l;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JL</code> / <code>JNGE</code>)
	 * <p>
	 * Default: <code>JL</code>, <code>CMOVL</code>, <code>SETL</code>
	 */
	public void setCC_l(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_L_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_l = value;
	}

	private int cc_l = CC_l.L;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JGE</code> / <code>JNL</code>)
	 * <p>
	 * Default: <code>JGE</code>, <code>CMOVGE</code>, <code>SETGE</code>
	 */
	public int getCC_ge() {
		return cc_ge;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JGE</code> / <code>JNL</code>)
	 * <p>
	 * Default: <code>JGE</code>, <code>CMOVGE</code>, <code>SETGE</code>
	 */
	public void setCC_ge(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_GE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_ge = value;
	}

	private int cc_ge = CC_ge.GE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JLE</code> / <code>JNG</code>)
	 * <p>
	 * Default: <code>JLE</code>, <code>CMOVLE</code>, <code>SETLE</code>
	 */
	public int getCC_le() {
		return cc_le;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JLE</code> / <code>JNG</code>)
	 * <p>
	 * Default: <code>JLE</code>, <code>CMOVLE</code>, <code>SETLE</code>
	 */
	public void setCC_le(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_LE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_le = value;
	}

	private int cc_le = CC_le.LE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JG</code> / <code>JNLE</code>)
	 * <p>
	 * Default: <code>JG</code>, <code>CMOVG</code>, <code>SETG</code>
	 */
	public int getCC_g() {
		return cc_g;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> <code>JG</code> / <code>JNLE</code>)
	 * <p>
	 * Default: <code>JG</code>, <code>CMOVG</code>, <code>SETG</code>
	 */
	public void setCC_g(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_G_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_g = value;
	}

	private int cc_g = CC_g.G;
}
