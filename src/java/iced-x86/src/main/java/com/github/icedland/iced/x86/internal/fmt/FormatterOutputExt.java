// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.fmt;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.fmt.FormatterOptions;
import com.github.icedland.iced.x86.fmt.FormatterOutput;
import com.github.icedland.iced.x86.fmt.FormatterTextKind;
import com.github.icedland.iced.x86.fmt.NumberFormattingOptions;
import com.github.icedland.iced.x86.fmt.NumberKind;
import com.github.icedland.iced.x86.fmt.SymbolFlags;
import com.github.icedland.iced.x86.fmt.SymbolResult;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class FormatterOutputExt {
	private FormatterOutputExt() {
	}

	public static void write(FormatterOutput output, Instruction instruction, int operand, int instructionOperand, FormatterOptions options,
			NumberFormatter numberFormatter, NumberFormattingOptions numberOptions, long address, SymbolResult symbol, boolean showSymbolAddress) {
		write(output, instruction, operand, instructionOperand, options, numberFormatter, numberOptions, address, symbol, showSymbolAddress, true,
				false);
	}

	public static void write(FormatterOutput output, Instruction instruction, int operand, int instructionOperand, FormatterOptions options,
			NumberFormatter numberFormatter, NumberFormattingOptions numberOptions, long address, SymbolResult symbol, boolean showSymbolAddress,
			boolean writeMinusIfSigned, boolean spacesBetweenOp) {
		long displ = address - symbol.address;
		if ((symbol.flags & SymbolFlags.SIGNED) != 0) {
			if (writeMinusIfSigned)
				output.write("-", FormatterTextKind.OPERATOR);
			displ = -displ;
		}
		output.writeSymbol(instruction, operand, instructionOperand, address, symbol);
		int numberKind;
		if (displ != 0) {
			if (spacesBetweenOp)
				output.write(" ", FormatterTextKind.TEXT);
			long origDispl = displ;
			if (displ < 0) {
				output.write("-", FormatterTextKind.OPERATOR);
				displ = -displ;
				if (displ <= 0x80)
					numberKind = NumberKind.INT8;
				else if (displ <= 0x8000)
					numberKind = NumberKind.INT16;
				else if (displ <= 0x8000_0000L)
					numberKind = NumberKind.INT32;
				else
					numberKind = NumberKind.INT64;
			} else {
				output.write("+", FormatterTextKind.OPERATOR);
				if (displ <= 0x7F)
					numberKind = NumberKind.INT8;
				else if (displ <= 0x7FFF)
					numberKind = NumberKind.INT16;
				else if (displ <= 0x7FFF_FFFF)
					numberKind = NumberKind.INT32;
				else
					numberKind = NumberKind.INT64;
			}
			if (spacesBetweenOp)
				output.write(" ", FormatterTextKind.TEXT);
			String s = numberFormatter.formatUInt64(options, numberOptions, displ, false);
			output.writeNumber(instruction, operand, instructionOperand, s, origDispl, numberKind, FormatterTextKind.NUMBER);
		}
		if (showSymbolAddress) {
			output.write(" ", FormatterTextKind.TEXT);
			output.write("(", FormatterTextKind.PUNCTUATION);
			String s;
			if (Long.compareUnsigned(address, 0xFFFF) <= 0) {
				s = numberFormatter.formatUInt16(options, numberOptions, (short)address, true);
				numberKind = NumberKind.UINT16;
			} else if (Long.compareUnsigned(address, 0xFFFF_FFFFL) <= 0) {
				s = numberFormatter.formatUInt32(options, numberOptions, (int)address, true);
				numberKind = NumberKind.UINT32;
			} else {
				s = numberFormatter.formatUInt64(options, numberOptions, address, true);
				numberKind = NumberKind.UINT64;
			}
			output.writeNumber(instruction, operand, instructionOperand, s, address, numberKind, FormatterTextKind.NUMBER);
			output.write(")", FormatterTextKind.PUNCTUATION);
		}
	}
}
