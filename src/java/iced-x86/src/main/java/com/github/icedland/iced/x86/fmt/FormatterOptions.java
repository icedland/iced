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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code REP stosd}
	 * <p>
	 * {@code false}: {@code rep stosd}
	 */
	public boolean getUppercasePrefixes() {
		return (flags1 & Flags1.UPPERCASE_PREFIXES) != 0;
	}

	/**
	 * Prefixes are uppercased
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code REP stosd}
	 * <p>
	 * {@code false}: {@code rep stosd}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code MOV rcx,rax}
	 * <p>
	 * {@code false}: {@code mov rcx,rax}
	 */
	public boolean getUppercaseMnemonics() {
		return (flags1 & Flags1.UPPERCASE_MNEMONICS) != 0;
	}

	/**
	 * Mnemonics are uppercased
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code MOV rcx,rax}
	 * <p>
	 * {@code false}: {@code mov rcx,rax}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov RCX,[RAX+RDX*8]}
	 * <p>
	 * {@code false}: {@code mov rcx,[rax+rdx*8]}
	 */
	public boolean getUppercaseRegisters() {
		return (flags1 & Flags1.UPPERCASE_REGISTERS) != 0;
	}

	/**
	 * Registers are uppercased
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov RCX,[RAX+RDX*8]}
	 * <p>
	 * {@code false}: {@code mov rcx,[rax+rdx*8]}
	 */
	public void setUppercaseRegisters(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_REGISTERS;
		else
			flags1 &= ~Flags1.UPPERCASE_REGISTERS;
	}

	/**
	 * Keywords are uppercased (eg.<!-- --> {@code BYTE PTR}, {@code SHORT})
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov BYTE PTR [rcx],12h}
	 * <p>
	 * {@code false}: {@code mov byte ptr [rcx],12h}
	 */
	public boolean getUppercaseKeywords() {
		return (flags1 & Flags1.UPPERCASE_KEYWORDS) != 0;
	}

	/**
	 * Keywords are uppercased (eg.<!-- --> {@code BYTE PTR}, {@code SHORT})
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov BYTE PTR [rcx],12h}
	 * <p>
	 * {@code false}: {@code mov byte ptr [rcx],12h}
	 */
	public void setUppercaseKeywords(boolean value) {
		if (value)
			flags1 |= Flags1.UPPERCASE_KEYWORDS;
		else
			flags1 &= ~Flags1.UPPERCASE_KEYWORDS;
	}

	/**
	 * Uppercase decorators, eg.<!-- --> {@code {z}}, {@code {sae}}, {@code {rd-sae}} (but not opmask registers: {@code {k1}})
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]}
	 * <p>
	 * {@code false}: {@code vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]}
	 */
	public boolean getUppercaseDecorators() {
		return (flags1 & Flags1.UPPERCASE_DECORATORS) != 0;
	}

	/**
	 * Uppercase decorators, eg.<!-- --> {@code {z}}, {@code {sae}}, {@code {rd-sae}} (but not opmask registers: {@code {k1}})
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]}
	 * <p>
	 * {@code false}: {@code vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code MOV EAX,GS:[RCX*4+0ffh]}
	 * <p>
	 * {@code false}: {@code mov eax,gs:[rcx*4+0ffh]}
	 */
	public boolean getUppercaseAll() {
		return (flags1 & Flags1.UPPERCASE_ALL) != 0;
	}

	/**
	 * Everything is uppercased, except numbers and their prefixes/suffixes
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code MOV EAX,GS:[RCX*4+0ffh]}
	 * <p>
	 * {@code false}: {@code mov eax,gs:[rcx*4+0ffh]}
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
	 * Default: {@code 0}
	 * <p>
	 * {@code 0}: {@code mov•rcx,rbp}
	 * <p>
	 * {@code 8}: {@code mov•••••rcx,rbp}
	 */
	public int getFirstOperandCharIndex() {
		return firstOperandCharIndex;
	}

	/**
	 * Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	 * At least one space or tab is always added between the mnemonic and the first operand.
	 * <p>
	 * Default: {@code 0}
	 * <p>
	 * {@code 0}: {@code mov•rcx,rbp}
	 * <p>
	 * {@code 8}: {@code mov•••••rcx,rbp}
	 */
	public void setFirstOperandCharIndex(int value) {
		firstOperandCharIndex = value;
	}

	private int firstOperandCharIndex;

	/**
	 * Size of a tab character or &lt;= 0 to use spaces
	 * <p>
	 * Default: {@code 0}
	 */
	public int getTabSize() {
		return tabSize;
	}

	/**
	 * Size of a tab character or &lt;= 0 to use spaces
	 * <p>
	 * Default: {@code 0}
	 */
	public void setTabSize(int value) {
		tabSize = value;
	}

	private int tabSize;

	/**
	 * Add a space after the operand separator
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov rax, rcx}
	 * <p>
	 * {@code false}: {@code mov rax,rcx}
	 */
	public boolean getSpaceAfterOperandSeparator() {
		return (flags1 & Flags1.SPACE_AFTER_OPERAND_SEPARATOR) != 0;
	}

	/**
	 * Add a space after the operand separator
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov rax, rcx}
	 * <p>
	 * {@code false}: {@code mov rax,rcx}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[ rcx+rdx ]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx+rdx]}
	 */
	public boolean getSpaceAfterMemoryBracket() {
		return (flags1 & Flags1.SPACE_AFTER_MEMORY_BRACKET) != 0;
	}

	/**
	 * Add a space between the memory expression and the brackets
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[ rcx+rdx ]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx+rdx]}
	 */
	public void setSpaceAfterMemoryBracket(boolean value) {
		if (value)
			flags1 |= Flags1.SPACE_AFTER_MEMORY_BRACKET;
		else
			flags1 &= ~Flags1.SPACE_AFTER_MEMORY_BRACKET;
	}

	/**
	 * Add spaces between memory operand {@code +} and {@code -} operators
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rcx + rdx*8 - 80h]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx+rdx*8-80h]}
	 */
	public boolean getSpaceBetweenMemoryAddOperators() {
		return (flags1 & Flags1.SPACE_BETWEEN_MEMORY_ADD_OPERATORS) != 0;
	}

	/**
	 * Add spaces between memory operand {@code +} and {@code -} operators
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rcx + rdx*8 - 80h]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx+rdx*8-80h]}
	 */
	public void setSpaceBetweenMemoryAddOperators(boolean value) {
		if (value)
			flags1 |= Flags1.SPACE_BETWEEN_MEMORY_ADD_OPERATORS;
		else
			flags1 &= ~Flags1.SPACE_BETWEEN_MEMORY_ADD_OPERATORS;
	}

	/**
	 * Add spaces between memory operand {@code *} operator
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rcx+rdx * 8-80h]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx+rdx*8-80h]}
	 */
	public boolean getSpaceBetweenMemoryMulOperators() {
		return (flags1 & Flags1.SPACE_BETWEEN_MEMORY_MUL_OPERATORS) != 0;
	}

	/**
	 * Add spaces between memory operand {@code *} operator
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rcx+rdx * 8-80h]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx+rdx*8-80h]}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[8*rdx]}
	 * <p>
	 * {@code false}: {@code mov eax,[rdx*8]}
	 */
	public boolean getScaleBeforeIndex() {
		return (flags1 & Flags1.SCALE_BEFORE_INDEX) != 0;
	}

	/**
	 * Show memory operand scale value before the index register
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[8*rdx]}
	 * <p>
	 * {@code false}: {@code mov eax,[rdx*8]}
	 */
	public void setScaleBeforeIndex(boolean value) {
		if (value)
			flags1 |= Flags1.SCALE_BEFORE_INDEX;
		else
			flags1 &= ~Flags1.SCALE_BEFORE_INDEX;
	}

	/**
	 * Always show the scale value even if it's {@code *1}
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rbx+rcx*1]}
	 * <p>
	 * {@code false}: {@code mov eax,[rbx+rcx]}
	 */
	public boolean getAlwaysShowScale() {
		return (flags1 & Flags1.ALWAYS_SHOW_SCALE) != 0;
	}

	/**
	 * Always show the scale value even if it's {@code *1}
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rbx+rcx*1]}
	 * <p>
	 * {@code false}: {@code mov eax,[rbx+rcx]}
	 */
	public void setAlwaysShowScale(boolean value) {
		if (value)
			flags1 |= Flags1.ALWAYS_SHOW_SCALE;
		else
			flags1 &= ~Flags1.ALWAYS_SHOW_SCALE;
	}

	/**
	 * Always show the effective segment register. If the option is {@code false}, only show the segment register if
	 * there's a segment override prefix.
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,ds:[ecx]}
	 * <p>
	 * {@code false}: {@code mov eax,[ecx]}
	 */
	public boolean getAlwaysShowSegmentRegister() {
		return (flags1 & Flags1.ALWAYS_SHOW_SEGMENT_REGISTER) != 0;
	}

	/**
	 * Always show the effective segment register. If the option is {@code false}, only show the segment register if
	 * there's a segment override prefix.
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,ds:[ecx]}
	 * <p>
	 * {@code false}: {@code mov eax,[ecx]}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rcx*2+0]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx*2]}
	 */
	public boolean getShowZeroDisplacements() {
		return (flags1 & Flags1.SHOW_ZERO_DISPLACEMENTS) != 0;
	}

	/**
	 * Show zero displacements
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rcx*2+0]}
	 * <p>
	 * {@code false}: {@code mov eax,[rcx*2]}
	 */
	public void setShowZeroDisplacements(boolean value) {
		if (value)
			flags1 |= Flags1.SHOW_ZERO_DISPLACEMENTS;
		else
			flags1 &= ~Flags1.SHOW_ZERO_DISPLACEMENTS;
	}

	/**
	 * Hex number prefix or {@code null}/empty string, eg.<!-- --> "0x"
	 * <p>
	 * Default: {@code null} (masm/nasm/intel), {@code "0x"} (gas)
	 */
	public String getHexPrefix() {
		return hexPrefix;
	}

	/**
	 * Hex number prefix or {@code null}/empty string, eg.<!-- --> "0x"
	 * <p>
	 * Default: {@code null} (masm/nasm/intel), {@code "0x"} (gas)
	 */
	public void setHexPrefix(String value) {
		hexPrefix = value;
	}

	private String hexPrefix;

	/**
	 * Hex number suffix or {@code null}/empty string, eg.<!-- --> "h"
	 * <p>
	 * Default: {@code "h"} (masm/nasm/intel), {@code null} (gas)
	 */
	public String getHexSuffix() {
		return hexSuffix;
	}

	/**
	 * Hex number suffix or {@code null}/empty string, eg.<!-- --> "h"
	 * <p>
	 * Default: {@code "h"} (masm/nasm/intel), {@code null} (gas)
	 */
	public void setHexSuffix(String value) {
		hexSuffix = value;
	}

	private String hexSuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 4}
	 * <p>
	 * {@code 0}: {@code 0x12345678}
	 * <p>
	 * {@code 4}: {@code 0x1234_5678}
	 */
	public int getHexDigitGroupSize() {
		return hexDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 4}
	 * <p>
	 * {@code 0}: {@code 0x12345678}
	 * <p>
	 * {@code 4}: {@code 0x1234_5678}
	 */
	public void setHexDigitGroupSize(int value) {
		hexDigitGroupSize = value;
	}

	private int hexDigitGroupSize = 4;

	/**
	 * Decimal number prefix or {@code null}/empty string
	 * <p>
	 * Default: {@code null}
	 */
	public String getDecimalPrefix() {
		return decimalPrefix;
	}

	/**
	 * Decimal number prefix or {@code null}/empty string
	 * <p>
	 * Default: {@code null}
	 */
	public void setDecimalPrefix(String value) {
		decimalPrefix = value;
	}

	private String decimalPrefix;

	/**
	 * Decimal number suffix or {@code null}/empty string
	 * <p>
	 * Default: {@code null}
	 */
	public String getDecimalSuffix() {
		return decimalSuffix;
	}

	/**
	 * Decimal number suffix or {@code null}/empty string
	 * <p>
	 * Default: {@code null}
	 */
	public void setDecimalSuffix(String value) {
		decimalSuffix = value;
	}

	private String decimalSuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 3}
	 * <p>
	 * {@code 0}: {@code 12345678}
	 * <p>
	 * {@code 3}: {@code 12_345_678}
	 */
	public int getDecimalDigitGroupSize() {
		return decimalDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 3}
	 * <p>
	 * {@code 0}: {@code 12345678}
	 * <p>
	 * {@code 3}: {@code 12_345_678}
	 */
	public void setDecimalDigitGroupSize(int value) {
		decimalDigitGroupSize = value;
	}

	private int decimalDigitGroupSize = 3;

	/**
	 * Octal number prefix or {@code null}/empty string
	 * <p>
	 * Default: {@code null} (masm/nasm/intel), {@code "0"} (gas)
	 */
	public String getOctalPrefix() {
		return octalPrefix;
	}

	/**
	 * Octal number prefix or {@code null}/empty string
	 * <p>
	 * Default: {@code null} (masm/nasm/intel), {@code "0"} (gas)
	 */
	public void setOctalPrefix(String value) {
		octalPrefix = value;
	}

	private String octalPrefix;

	/**
	 * Octal number suffix or {@code null}/empty string
	 * <p>
	 * Default: {@code "o"} (masm/nasm/intel), {@code null} (gas)
	 */
	public String getOctalSuffix() {
		return octalSuffix;
	}

	/**
	 * Octal number suffix or {@code null}/empty string
	 * <p>
	 * Default: {@code "o"} (masm/nasm/intel), {@code null} (gas)
	 */
	public void setOctalSuffix(String value) {
		octalSuffix = value;
	}

	private String octalSuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 4}
	 * <p>
	 * {@code 0}: {@code 12345670}
	 * <p>
	 * {@code 4}: {@code 1234_5670}
	 */
	public int getOctalDigitGroupSize() {
		return octalDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 4}
	 * <p>
	 * {@code 0}: {@code 12345670}
	 * <p>
	 * {@code 4}: {@code 1234_5670}
	 */
	public void setOctalDigitGroupSize(int value) {
		octalDigitGroupSize = value;
	}

	private int octalDigitGroupSize = 4;

	/**
	 * Binary number prefix or {@code null}/empty string
	 * <p>
	 * Default: {@code null} (masm/nasm/intel), {@code "0b"} (gas)
	 */
	public String getBinaryPrefix() {
		return binaryPrefix;
	}

	/**
	 * Binary number prefix or {@code null}/empty string
	 * <p>
	 * Default: {@code null} (masm/nasm/intel), {@code "0b"} (gas)
	 */
	public void setBinaryPrefix(String value) {
		binaryPrefix = value;
	}

	private String binaryPrefix;

	/**
	 * Binary number suffix or {@code null}/empty string
	 * <p>
	 * Default: {@code "b"} (masm/nasm/intel), {@code null} (gas)
	 */
	public String getBinarySuffix() {
		return binarySuffix;
	}

	/**
	 * Binary number suffix or {@code null}/empty string
	 * <p>
	 * Default: {@code "b"} (masm/nasm/intel), {@code null} (gas)
	 */
	public void setBinarySuffix(String value) {
		binarySuffix = value;
	}

	private String binarySuffix;

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 4}
	 * <p>
	 * {@code 0}: {@code 11010111}
	 * <p>
	 * {@code 4}: {@code 1101_0111}
	 */
	public int getBinaryDigitGroupSize() {
		return binaryDigitGroupSize;
	}

	/**
	 * Size of a digit group, see also {@link #getDigitSeparator()}
	 * <p>
	 * Default: {@code 4}
	 * <p>
	 * {@code 0}: {@code 11010111}
	 * <p>
	 * {@code 4}: {@code 1101_0111}
	 */
	public void setBinaryDigitGroupSize(int value) {
		binaryDigitGroupSize = value;
	}

	private int binaryDigitGroupSize = 4;

	/**
	 * Digit separator or {@code null}/empty string. See also eg. {@link #getHexDigitGroupSize()}.
	 * <p>
	 * Default: {@code null}
	 * <p>
	 * {@code ""}: {@code 0x12345678}
	 * <p>
	 * {@code "_"}: {@code 0x1234_5678}
	 */
	public String getDigitSeparator() {
		return digitSeparator;
	}

	/**
	 * Digit separator or {@code null}/empty string. See also eg. {@link #getHexDigitGroupSize()}.
	 * <p>
	 * Default: {@code null}
	 * <p>
	 * {@code ""}: {@code 0x12345678}
	 * <p>
	 * {@code "_"}: {@code 0x1234_5678}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code 0x0000000A}/{@code 0000000Ah}
	 * <p>
	 * {@code false}: {@code 0xA}/{@code 0Ah}
	 */
	public boolean getLeadingZeros() {
		return (flags1 & Flags1.LEADING_ZEROS) != 0;
	}

	/**
	 * Add leading zeros to hexadecimal/octal/binary numbers.
	 * This option has no effect on branch targets and displacements, use {@link #getBranchLeadingZeros()}
	 * and {@link #getDisplacementLeadingZeros()}.
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code 0x0000000A}/{@code 0000000Ah}
	 * <p>
	 * {@code false}: {@code 0xA}/{@code 0Ah}
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
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code 0xFF}
	 * <p>
	 * {@code false}: {@code 0xff}
	 */
	public boolean getUppercaseHex() {
		return (flags1 & Flags1.UPPERCASE_HEX) != 0;
	}

	/**
	 * Use uppercase hex digits
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code 0xFF}
	 * <p>
	 * {@code false}: {@code 0xff}
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
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code 9}
	 * <p>
	 * {@code false}: {@code 0x9}
	 */
	public boolean getSmallHexNumbersInDecimal() {
		return (flags1 & Flags1.SMALL_HEX_NUMBERS_IN_DECIMAL) != 0;
	}

	/**
	 * Small hex numbers (-9 .<!-- -->.<!-- --> 9) are shown in decimal
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code 9}
	 * <p>
	 * {@code false}: {@code 0x9}
	 */
	public void setSmallHexNumbersInDecimal(boolean value) {
		if (value)
			flags1 |= Flags1.SMALL_HEX_NUMBERS_IN_DECIMAL;
		else
			flags1 &= ~Flags1.SMALL_HEX_NUMBERS_IN_DECIMAL;
	}

	/**
	 * Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits {@code A-F}
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code 0FFh}
	 * <p>
	 * {@code false}: {@code FFh}
	 */
	public boolean getAddLeadingZeroToHexNumbers() {
		return (flags1 & Flags1.ADD_LEADING_ZERO_TO_HEX_NUMBERS) != 0;
	}

	/**
	 * Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits {@code A-F}
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code 0FFh}
	 * <p>
	 * {@code false}: {@code FFh}
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
	 * Add leading zeros to branch offsets. Used by {@code CALL NEAR}, {@code CALL FAR}, {@code JMP NEAR}, {@code JMP FAR},
	 * {@code Jcc}, {@code LOOP}, {@code LOOPcc}, {@code XBEGIN}
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code je 00000123h}
	 * <p>
	 * {@code false}: {@code je 123h}
	 */
	public boolean getBranchLeadingZeros() {
		return (flags1 & Flags1.BRANCH_LEADING_ZEROS) != 0;
	}

	/**
	 * Add leading zeros to branch offsets. Used by {@code CALL NEAR}, {@code CALL FAR}, {@code JMP NEAR}, {@code JMP FAR},
	 * {@code Jcc}, {@code LOOP}, {@code LOOPcc}, {@code XBEGIN}
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code je 00000123h}
	 * <p>
	 * {@code false}: {@code je 123h}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,-1}
	 * <p>
	 * {@code false}: {@code mov eax,FFFFFFFF}
	 */
	public boolean getSignedImmediateOperands() {
		return (flags1 & Flags1.SIGNED_IMMEDIATE_OPERANDS) != 0;
	}

	/**
	 * Show immediate operands as signed numbers
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,-1}
	 * <p>
	 * {@code false}: {@code mov eax,FFFFFFFF}
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
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code mov al,[eax-2000h]}
	 * <p>
	 * {@code false}: {@code mov al,[eax+0FFFFE000h]}
	 */
	public boolean getSignedMemoryDisplacements() {
		return (flags1 & Flags1.SIGNED_MEMORY_DISPLACEMENTS) != 0;
	}

	/**
	 * Displacements are signed numbers
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code mov al,[eax-2000h]}
	 * <p>
	 * {@code false}: {@code mov al,[eax+0FFFFE000h]}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov al,[eax+00000012h]}
	 * <p>
	 * {@code false}: {@code mov al,[eax+12h]}
	 */
	public boolean getDisplacementLeadingZeros() {
		return (flags1 & Flags1.DISPLACEMENT_LEADING_ZEROS) != 0;
	}

	/**
	 * Add leading zeros to displacements
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov al,[eax+00000012h]}
	 * <p>
	 * {@code false}: {@code mov al,[eax+12h]}
	 */
	public void setDisplacementLeadingZeros(boolean value) {
		if (value)
			flags1 |= Flags1.DISPLACEMENT_LEADING_ZEROS;
		else
			flags1 &= ~Flags1.DISPLACEMENT_LEADING_ZEROS;
	}

	/**
	 * Options that control if the memory size (eg.<!-- --> {@code DWORD PTR}) is shown or not.
	 * This is ignored by the gas (AT&amp;T) formatter (a {@link MemorySizeOptions} enum variant).
	 * <p>
	 * Default: {@link MemorySizeOptions#DEFAULT}
	 */
	public int getMemorySizeOptions() {
		return memorySizeOptions;
	}

	/**
	 * Options that control if the memory size (eg.<!-- --> {@code DWORD PTR}) is shown or not.
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
	 * Show {@code RIP+displ} or the virtual address
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rip+12345678h]}
	 * <p>
	 * {@code false}: {@code mov eax,[1029384756AFBECDh]}
	 */
	public boolean getRipRelativeAddresses() {
		return (flags1 & Flags1.RIP_RELATIVE_ADDRESSES) != 0;
	}

	/**
	 * Show {@code RIP+displ} or the virtual address
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[rip+12345678h]}
	 * <p>
	 * {@code false}: {@code mov eax,[1029384756AFBECDh]}
	 */
	public void setRipRelativeAddresses(boolean value) {
		if (value)
			flags1 |= Flags1.RIP_RELATIVE_ADDRESSES;
		else
			flags1 &= ~Flags1.RIP_RELATIVE_ADDRESSES;
	}

	/**
	 * Show {@code NEAR}, {@code SHORT}, etc if it's a branch instruction
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code je short 1234h}
	 * <p>
	 * {@code false}: {@code je 1234h}
	 */
	public boolean getShowBranchSize() {
		return (flags1 & Flags1.SHOW_BRANCH_SIZE) != 0;
	}

	/**
	 * Show {@code NEAR}, {@code SHORT}, etc if it's a branch instruction
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code je short 1234h}
	 * <p>
	 * {@code false}: {@code je 1234h}
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
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code vcmpnltsd xmm2,xmm6,xmm3}
	 * <p>
	 * {@code false}: {@code vcmpsd xmm2,xmm6,xmm3,5}
	 */
	public boolean getUsePseudoOps() {
		return (flags1 & Flags1.USE_PSEUDO_OPS) != 0;
	}

	/**
	 * Use pseudo instructions
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code vcmpnltsd xmm2,xmm6,xmm3}
	 * <p>
	 * {@code false}: {@code vcmpsd xmm2,xmm6,xmm3,5}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[myfield (12345678)]}
	 * <p>
	 * {@code false}: {@code mov eax,[myfield]}
	 */
	public boolean getShowSymbolAddress() {
		return (flags1 & Flags1.SHOW_SYMBOL_ADDRESS) != 0;
	}

	/**
	 * Show the original value after the symbol name
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,[myfield (12345678)]}
	 * <p>
	 * {@code false}: {@code mov eax,[myfield]}
	 */
	public void setShowSymbolAddress(boolean value) {
		if (value)
			flags1 |= Flags1.SHOW_SYMBOL_ADDRESS;
		else
			flags1 &= ~Flags1.SHOW_SYMBOL_ADDRESS;
	}

	/**
	 * (gas only): If {@code true}, the formatter doesn't add {@code %} to registers
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,ecx}
	 * <p>
	 * {@code false}: {@code mov %eax,%ecx}
	 */
	public boolean getGasNakedRegisters() {
		return (flags1 & Flags1.GAS_NAKED_REGISTERS) != 0;
	}

	/**
	 * (gas only): If {@code true}, the formatter doesn't add {@code %} to registers
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,ecx}
	 * <p>
	 * {@code false}: {@code mov %eax,%ecx}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code movl %eax,%ecx}
	 * <p>
	 * {@code false}: {@code mov %eax,%ecx}
	 */
	public boolean getGasShowMnemonicSizeSuffix() {
		return (flags1 & Flags1.GAS_SHOW_MNEMONIC_SIZE_SUFFIX) != 0;
	}

	/**
	 * (gas only): Shows the mnemonic size suffix even when not needed
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code movl %eax,%ecx}
	 * <p>
	 * {@code false}: {@code mov %eax,%ecx}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code (%eax, %ecx, 2)}
	 * <p>
	 * {@code false}: {@code (%eax,%ecx,2)}
	 */
	public boolean getGasSpaceAfterMemoryOperandComma() {
		return (flags1 & Flags1.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA) != 0;
	}

	/**
	 * (gas only): Add a space after the comma if it's a memory operand
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code (%eax, %ecx, 2)}
	 * <p>
	 * {@code false}: {@code (%eax,%ecx,2)}
	 */
	public void setGasSpaceAfterMemoryOperandComma(boolean value) {
		if (value)
			flags1 |= Flags1.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA;
		else
			flags1 &= ~Flags1.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA;
	}

	/**
	 * (masm only): Add a {@code DS} segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code mov eax,ds:[12345678]}
	 * <p>
	 * {@code false}: {@code mov eax,[12345678]}
	 */
	public boolean getMasmAddDsPrefix32() {
		return (flags1 & Flags1.MASM_ADD_DS_PREFIX32) != 0;
	}

	/**
	 * (masm only): Add a {@code DS} segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code mov eax,ds:[12345678]}
	 * <p>
	 * {@code false}: {@code mov eax,[12345678]}
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
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code [ecx+symbol]} / {@code [symbol]}
	 * <p>
	 * {@code false}: {@code symbol[ecx]} / {@code symbol}
	 */
	public boolean getMasmSymbolDisplInBrackets() {
		return (flags1 & Flags1.MASM_SYMBOL_DISPL_IN_BRACKETS) != 0;
	}

	/**
	 * (masm only): Show symbols in brackets
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code [ecx+symbol]} / {@code [symbol]}
	 * <p>
	 * {@code false}: {@code symbol[ecx]} / {@code symbol}
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
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code [ecx+1234h]}
	 * <p>
	 * {@code false}: {@code 1234h[ecx]}
	 */
	public boolean getMasmDisplInBrackets() {
		return (flags1 & Flags1.MASM_DISPL_IN_BRACKETS) != 0;
	}

	/**
	 * (masm only): Show displacements in brackets
	 * <p>
	 * Default: {@code true}
	 * <p>
	 * {@code true}: {@code [ecx+1234h]}
	 * <p>
	 * {@code false}: {@code 1234h[ecx]}
	 */
	public void setMasmDisplInBrackets(boolean value) {
		if (value)
			flags1 |= Flags1.MASM_DISPL_IN_BRACKETS;
		else
			flags1 &= ~Flags1.MASM_DISPL_IN_BRACKETS;
	}

	/**
	 * (nasm only): Shows {@code BYTE}, {@code WORD}, {@code DWORD} or {@code QWORD} if it's a sign extended immediate operand
	 * value
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code or rcx,byte -1}
	 * <p>
	 * {@code false}: {@code or rcx,-1}
	 */
	public boolean getNasmShowSignExtendedImmediateSize() {
		return (flags2 & Flags2.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE) != 0;
	}

	/**
	 * (nasm only): Shows {@code BYTE}, {@code WORD}, {@code DWORD} or {@code QWORD} if it's a sign extended immediate operand
	 * value
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code or rcx,byte -1}
	 * <p>
	 * {@code false}: {@code or rcx,-1}
	 */
	public void setNasmShowSignExtendedImmediateSize(boolean value) {
		if (value)
			flags2 |= Flags2.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE;
		else
			flags2 &= ~Flags2.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE;
	}

	/**
	 * Use {@code st(0)} instead of {@code st} if {@code st} can be used. Ignored by the nasm formatter.
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code fadd st(0),st(3)}
	 * <p>
	 * {@code false}: {@code fadd st,st(3)}
	 */
	public boolean getPreferST0() {
		return (flags2 & Flags2.PREFER_ST0) != 0;
	}

	/**
	 * Use {@code st(0)} instead of {@code st} if {@code st} can be used. Ignored by the nasm formatter.
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code fadd st(0),st(3)}
	 * <p>
	 * {@code false}: {@code fadd st,st(3)}
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
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code es rep add eax,ecx}
	 * <p>
	 * {@code false}: {@code add eax,ecx}
	 */
	public boolean getShowUselessPrefixes() {
		return (flags2 & Flags2.SHOW_USELESS_PREFIXES) != 0;
	}

	/**
	 * Show useless prefixes. If it has useless prefixes, it could be data and not code.
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code es rep add eax,ecx}
	 * <p>
	 * {@code false}: {@code add eax,ecx}
	 */
	public void setShowUselessPrefixes(boolean value) {
		if (value)
			flags2 |= Flags2.SHOW_USELESS_PREFIXES;
		else
			flags2 &= ~Flags2.SHOW_USELESS_PREFIXES;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JB} / {@code JC} / {@code JNAE})
	 * <p>
	 * Default: {@code JB}, {@code CMOVB}, {@code SETB}
	 */
	public int getCC_b() {
		return cc_b;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JB} / {@code JC} / {@code JNAE})
	 * <p>
	 * Default: {@code JB}, {@code CMOVB}, {@code SETB}
	 */
	public void setCC_b(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_B_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_b = value;
	}

	private int cc_b = CC_b.B;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JAE} / {@code JNB} / {@code JNC})
	 * <p>
	 * Default: {@code JAE}, {@code CMOVAE}, {@code SETAE}
	 */
	public int getCC_ae() {
		return cc_ae;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JAE} / {@code JNB} / {@code JNC})
	 * <p>
	 * Default: {@code JAE}, {@code CMOVAE}, {@code SETAE}
	 */
	public void setCC_ae(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_AE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_ae = value;
	}

	private int cc_ae = CC_ae.AE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JE} / {@code JZ})
	 * <p>
	 * Default: {@code JE}, {@code CMOVE}, {@code SETE}, {@code LOOPE}, {@code REPE}
	 */
	public int getCC_e() {
		return cc_e;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JE} / {@code JZ})
	 * <p>
	 * Default: {@code JE}, {@code CMOVE}, {@code SETE}, {@code LOOPE}, {@code REPE}
	 */
	public void setCC_e(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_E_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_e = value;
	}

	private int cc_e = CC_e.E;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JNE} / {@code JNZ})
	 * <p>
	 * Default: {@code JNE}, {@code CMOVNE}, {@code SETNE}, {@code LOOPNE}, {@code REPNE}
	 */
	public int getCC_ne() {
		return cc_ne;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JNE} / {@code JNZ})
	 * <p>
	 * Default: {@code JNE}, {@code CMOVNE}, {@code SETNE}, {@code LOOPNE}, {@code REPNE}
	 */
	public void setCC_ne(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_NE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_ne = value;
	}

	private int cc_ne = CC_ne.NE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JBE} / {@code JNA})
	 * <p>
	 * Default: {@code JBE}, {@code CMOVBE}, {@code SETBE}
	 */
	public int getCC_be() {
		return cc_be;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JBE} / {@code JNA})
	 * <p>
	 * Default: {@code JBE}, {@code CMOVBE}, {@code SETBE}
	 */
	public void setCC_be(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_BE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_be = value;
	}

	private int cc_be = CC_be.BE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JA} / {@code JNBE})
	 * <p>
	 * Default: {@code JA}, {@code CMOVA}, {@code SETA}
	 */
	public int getCC_a() {
		return cc_a;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JA} / {@code JNBE})
	 * <p>
	 * Default: {@code JA}, {@code CMOVA}, {@code SETA}
	 */
	public void setCC_a(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_A_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_a = value;
	}

	private int cc_a = CC_a.A;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JP} / {@code JPE})
	 * <p>
	 * Default: {@code JP}, {@code CMOVP}, {@code SETP}
	 */
	public int getCC_p() {
		return cc_p;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JP} / {@code JPE})
	 * <p>
	 * Default: {@code JP}, {@code CMOVP}, {@code SETP}
	 */
	public void setCC_p(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_P_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_p = value;
	}

	private int cc_p = CC_p.P;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JNP} / {@code JPO})
	 * <p>
	 * Default: {@code JNP}, {@code CMOVNP}, {@code SETNP}
	 */
	public int getCC_np() {
		return cc_np;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JNP} / {@code JPO})
	 * <p>
	 * Default: {@code JNP}, {@code CMOVNP}, {@code SETNP}
	 */
	public void setCC_np(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_NP_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_np = value;
	}

	private int cc_np = CC_np.NP;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JL} / {@code JNGE})
	 * <p>
	 * Default: {@code JL}, {@code CMOVL}, {@code SETL}
	 */
	public int getCC_l() {
		return cc_l;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JL} / {@code JNGE})
	 * <p>
	 * Default: {@code JL}, {@code CMOVL}, {@code SETL}
	 */
	public void setCC_l(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_L_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_l = value;
	}

	private int cc_l = CC_l.L;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JGE} / {@code JNL})
	 * <p>
	 * Default: {@code JGE}, {@code CMOVGE}, {@code SETGE}
	 */
	public int getCC_ge() {
		return cc_ge;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JGE} / {@code JNL})
	 * <p>
	 * Default: {@code JGE}, {@code CMOVGE}, {@code SETGE}
	 */
	public void setCC_ge(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_GE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_ge = value;
	}

	private int cc_ge = CC_ge.GE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JLE} / {@code JNG})
	 * <p>
	 * Default: {@code JLE}, {@code CMOVLE}, {@code SETLE}
	 */
	public int getCC_le() {
		return cc_le;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JLE} / {@code JNG})
	 * <p>
	 * Default: {@code JLE}, {@code CMOVLE}, {@code SETLE}
	 */
	public void setCC_le(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_LE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_le = value;
	}

	private int cc_le = CC_le.LE;

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JG} / {@code JNLE})
	 * <p>
	 * Default: {@code JG}, {@code CMOVG}, {@code SETG}
	 */
	public int getCC_g() {
		return cc_g;
	}

	/**
	 * Mnemonic condition code selector (eg.<!-- --> {@code JG} / {@code JNLE})
	 * <p>
	 * Default: {@code JG}, {@code CMOVG}, {@code SETG}
	 */
	public void setCC_g(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CC_G_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException("value");
		cc_g = value;
	}

	private int cc_g = CC_g.G;
}
