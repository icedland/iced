// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

/**
 * Gets initialized with the default options and can be overridden by a {@link FormatterOptionsProvider}
 */
public final class NumberFormattingOptions {
	/**
	 * Digit separator or {@code null}/empty string to not use a digit separator
	 */
	public String digitSeparator;

	/**
	 * Number prefix or {@code null}/empty string
	 */
	public String prefix;

	/**
	 * Number suffix or {@code null}/empty string
	 */
	public String suffix;

	/**
	 * Size of a digit group or 0 to not use a digit separator
	 */
	public byte digitGroupSize;

	/**
	 * Number base (a {@link NumberBase} enum variant)
	 */
	public int numberBase;

	/**
	 * Use uppercase hex digits
	 */
	public boolean uppercaseHex;

	/**
	 * Small hex numbers (-9 .. 9) are shown in decimal
	 */
	public boolean smallHexNumbersInDecimal;

	/**
	 * Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits {@code A-F}
	 */
	public boolean addLeadingZeroToHexNumbers;

	/**
	 * If {@code true}, add leading zeros to numbers, eg. '1h' vs '00000001h'
	 */
	public boolean leadingZeros;

	/**
	 * If {@code true}, the number is signed, and if {@code false} it's an unsigned number
	 */
	public boolean signedNumber;

	/**
	 * Add leading zeros to displacements
	 */
	public boolean displacementLeadingZeros;

	/**
	 * Creates options used when formatting immediate values
	 *
	 * @param options Formatter options to use
	 */
	public static NumberFormattingOptions createImmediate(FormatterOptions options) {
		if (options == null)
			throw new NullPointerException("options");
		return new NumberFormattingOptions(options, options.getLeadingZeros(), options.getSignedImmediateOperands(), false);
	}

	/**
	 * Creates options used when formatting displacements
	 *
	 * @param options Formatter options to use
	 */
	public static NumberFormattingOptions createDisplacement(FormatterOptions options) {
		if (options == null)
			throw new NullPointerException("options");
		return new NumberFormattingOptions(options, options.getLeadingZeros(), options.getSignedMemoryDisplacements(),
				options.getDisplacementLeadingZeros());
	}

	/**
	 * Creates options used when formatting branch operands
	 *
	 * @param options Formatter options to use
	 */
	public static NumberFormattingOptions createBranch(FormatterOptions options) {
		if (options == null)
			throw new NullPointerException("options");
		return new NumberFormattingOptions(options, options.getBranchLeadingZeros(), false, false);
	}

	/**
	 * Constructor
	 *
	 * @param options                  Formatter options to use
	 * @param leadingZeros             Add leading zeros to numbers, eg. {@code 1h} vs {@code 00000001h}
	 * @param signedNumber             Signed numbers if {@code true}, and unsigned numbers if {@code false}
	 * @param displacementLeadingZeros Add leading zeros to displacements
	 */
	public NumberFormattingOptions(FormatterOptions options, boolean leadingZeros, boolean signedNumber, boolean displacementLeadingZeros) {
		if (options == null)
			throw new NullPointerException("options");
		this.leadingZeros = leadingZeros;
		this.signedNumber = signedNumber;
		this.displacementLeadingZeros = displacementLeadingZeros;
		this.numberBase = options.getNumberBase();
		this.digitSeparator = options.getDigitSeparator();
		this.uppercaseHex = options.getUppercaseHex();
		this.smallHexNumbersInDecimal = options.getSmallHexNumbersInDecimal();
		this.addLeadingZeroToHexNumbers = options.getAddLeadingZeroToHexNumbers();
		int digitGroupSize;
		switch (options.getNumberBase()) {
		case NumberBase.HEXADECIMAL:
			prefix = options.getHexPrefix();
			suffix = options.getHexSuffix();
			digitGroupSize = options.getHexDigitGroupSize();
			break;

		case NumberBase.DECIMAL:
			prefix = options.getDecimalPrefix();
			suffix = options.getDecimalSuffix();
			digitGroupSize = options.getDecimalDigitGroupSize();
			break;

		case NumberBase.OCTAL:
			prefix = options.getOctalPrefix();
			suffix = options.getOctalSuffix();
			digitGroupSize = options.getOctalDigitGroupSize();
			break;

		case NumberBase.BINARY:
			prefix = options.getBinaryPrefix();
			suffix = options.getBinarySuffix();
			digitGroupSize = options.getBinaryDigitGroupSize();
			break;

		default:
			throw new UnsupportedOperationException();
		}
		if (digitGroupSize < 0)
			this.digitGroupSize = 0;
		else if (digitGroupSize > 0x7F)
			this.digitGroupSize = 0x7F;
		else
			this.digitGroupSize = (byte)digitGroupSize;
	}
}
