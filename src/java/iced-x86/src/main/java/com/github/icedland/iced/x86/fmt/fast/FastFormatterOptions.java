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
	 * Always show memory operands' size
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,dword ptr [ebx]</code> / <code>add byte ptr [eax],0x12</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[ebx]</code> / <code>add byte ptr [eax],0x12</code>
	 */
	public boolean getAlwaysShowMemorySize() {
		return (flags1 & Flags1.ALWAYS_SHOW_MEMORY_SIZE) != 0;
	}

	/**
	 * Always show memory operands' size
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>mov eax,dword ptr [ebx]</code> / <code>add byte ptr [eax],0x12</code>
	 * <p>
	 * <code>false</code>: <code>mov eax,[ebx]</code> / <code>add byte ptr [eax],0x12</code>
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
	 * Use a hex prefix (<code>0x</code>) or a hex suffix (<code>h</code>)
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>0x5A</code>
	 * <p>
	 * <code>false</code>: <code>5Ah</code>
	 */
	public boolean getUseHexPrefix() {
		return (flags1 & Flags1.USE_HEX_PREFIX) != 0;
	}

	/**
	 * Use a hex prefix (<code>0x</code>) or a hex suffix (<code>h</code>)
	 * <p>
	 * Default: <code>false</code>
	 * <p>
	 * <code>true</code>: <code>0x5A</code>
	 * <p>
	 * <code>false</code>: <code>5Ah</code>
	 */
	public void setUseHexPrefix(boolean value) {
		if (value)
			flags1 |= Flags1.USE_HEX_PREFIX;
		else
			flags1 &= ~Flags1.USE_HEX_PREFIX;
	}
}
