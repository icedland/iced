// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.fast;

/**
 * Fast formatter options
 */
public final class FastFormatterOptions {
	private int flags1;

	private static final class Flags1 {
		static final int SPACE_AFTER_OPERAND_SEPARATOR = 0x0000_0001;
		static final int RIP_RELATIVE_ADDRESSES = 0x0000_0002;
		static final int USE_PSEUDO_OPS = 0x0000_0004;
		static final int SHOW_SYMBOL_ADDRESS = 0x0000_0008;
		static final int ALWAYS_SHOW_SEGMENT_REGISTER = 0x0000_0010;
		static final int ALWAYS_SHOW_MEMORY_SIZE = 0x0000_0020;
		static final int UPPERCASE_HEX = 0x0000_0040;
		static final int USE_HEX_PREFIX = 0x0000_0080;
	}

	FastFormatterOptions() {
		flags1 = Flags1.USE_PSEUDO_OPS | Flags1.UPPERCASE_HEX;
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
	 * Always show memory operands' size
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,dword ptr [ebx]} / {@code add byte ptr [eax],0x12}
	 * <p>
	 * {@code false}: {@code mov eax,[ebx]} / {@code add byte ptr [eax],0x12}
	 */
	public boolean getAlwaysShowMemorySize() {
		return (flags1 & Flags1.ALWAYS_SHOW_MEMORY_SIZE) != 0;
	}

	/**
	 * Always show memory operands' size
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code mov eax,dword ptr [ebx]} / {@code add byte ptr [eax],0x12}
	 * <p>
	 * {@code false}: {@code mov eax,[ebx]} / {@code add byte ptr [eax],0x12}
	 */
	public void setAlwaysShowMemorySize(boolean value) {
		if (value)
			flags1 |= Flags1.ALWAYS_SHOW_MEMORY_SIZE;
		else
			flags1 &= ~Flags1.ALWAYS_SHOW_MEMORY_SIZE;
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
	 * Use a hex prefix ({@code 0x}) or a hex suffix ({@code h})
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code 0x5A}
	 * <p>
	 * {@code false}: {@code 5Ah}
	 */
	public boolean getUseHexPrefix() {
		return (flags1 & Flags1.USE_HEX_PREFIX) != 0;
	}

	/**
	 * Use a hex prefix ({@code 0x}) or a hex suffix ({@code h})
	 * <p>
	 * Default: {@code false}
	 * <p>
	 * {@code true}: {@code 0x5A}
	 * <p>
	 * {@code false}: {@code 5Ah}
	 */
	public void setUseHexPrefix(boolean value) {
		if (value)
			flags1 |= Flags1.USE_HEX_PREFIX;
		else
			flags1 &= ~Flags1.USE_HEX_PREFIX;
	}
}
