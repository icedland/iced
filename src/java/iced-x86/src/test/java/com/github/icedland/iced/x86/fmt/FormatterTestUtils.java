// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.function.Consumer;
import java.util.function.Function;

import static org.junit.jupiter.api.Assertions.*;

import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderTestInfo;
import com.github.icedland.iced.x86.dec.DecoderTestUtils;
import com.github.icedland.iced.x86.fmt.fast.FastFormatter;
import com.github.icedland.iced.x86.fmt.fast.FastStringOutput;

final class FormatterTestUtils {
	static void simpleFormatTest(int bitness, String hexBytes, long ip, int code, int options, String formattedString,
			Function<Instruction, String> format, Consumer<Decoder> initDecoder) {
		Decoder decoder = createDecoder(bitness, hexBytes, ip, options);
		if (initDecoder != null)
			initDecoder.accept(decoder);
		long nextRip = decoder.getIP();
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		assertEquals((short)nextRip, instruction.getIP16());
		assertEquals((int)nextRip, instruction.getIP32());
		assertEquals(nextRip, instruction.getIP());
		nextRip += instruction.getLength();
		assertEquals(nextRip, decoder.getIP());
		assertEquals((short)nextRip, instruction.getNextIP16());
		assertEquals((int)nextRip, instruction.getNextIP32());
		assertEquals(nextRip, instruction.getNextIP());

		String actualFormattedString = format.apply(instruction);
		assertEquals(formattedString, actualFormattedString);
	}

	private static Decoder createDecoder(int bitness, String hexBytes, long ip, int options) {
		ByteArrayCodeReader codeReader = new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes));
		Decoder decoder = new Decoder(bitness, codeReader, options);
		assertEquals(bitness, decoder.getBitness());
		decoder.setIP(ip);
		return decoder;
	}

	static void formatTest(int bitness, String hexBytes, long ip, int code, int options, String formattedString, Formatter formatter) {
		Decoder decoder = createDecoder(bitness, hexBytes, ip, options);
		long nextRip = decoder.getIP();
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		assertEquals((short)nextRip, instruction.getIP16());
		assertEquals((int)nextRip, instruction.getIP32());
		assertEquals(nextRip, instruction.getIP());
		nextRip += instruction.getLength();
		assertEquals(nextRip, decoder.getIP());
		assertEquals((short)nextRip, instruction.getNextIP16());
		assertEquals((int)nextRip, instruction.getNextIP32());
		assertEquals(nextRip, instruction.getNextIP());
		formatTest(instruction, formattedString, formatter);
	}

	static void formatTest(Instruction instruction, String formattedString, Formatter formatter) {
		StringOutput output = new StringOutput();

		formatter.format(instruction, output);
		String actualFormattedString = output.toStringAndReset();
		assertEquals(formattedString, actualFormattedString);

		formatter.formatMnemonic(instruction, output);
		String mnemonic = output.toStringAndReset();
		int opCount = formatter.getOperandCount(instruction);
		String[] operands = new String[opCount];
		for (int i = 0; i < operands.length; i++) {
			formatter.formatOperand(instruction, output, i);
			operands[i] = output.toStringAndReset();
		}
		output.write(mnemonic, FormatterTextKind.TEXT);
		if (operands.length > 0) {
			output.write(" ", FormatterTextKind.TEXT);
			for (int i = 0; i < operands.length; i++) {
				if (i > 0)
					formatter.formatOperandSeparator(instruction, output);
				output.write(operands[i], FormatterTextKind.TEXT);
			}
		}
		actualFormattedString = output.toStringAndReset();
		assertEquals(formattedString, actualFormattedString);

		formatter.formatAllOperands(instruction, output);
		String allOperands = output.toStringAndReset();
		actualFormattedString = allOperands.length() == 0 ? mnemonic : mnemonic + " " + allOperands;
		assertEquals(formattedString, actualFormattedString);
	}

	static void simpleFormatTest(int bitness, String hexBytes, long ip, int code, int options, String formattedString, Formatter formatter,
			Consumer<Decoder> initDecoder) {
		Function<Instruction, String> format = (Instruction instruction) -> {
			StringOutput output = new StringOutput();
			formatter.format(instruction, output);
			return output.toStringAndReset();
		};
		simpleFormatTest(bitness, hexBytes, ip, code, options, formattedString, format, initDecoder);
	}

	static void testFormatterDoesNotThrow(Formatter formatter) {
		StringOutput output = new StringOutput();
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(true, true)) {
			Decoder decoder = createDecoder(info.bitness, info.hexBytes, info.ip, info.options);
			Instruction instruction = new Instruction();
			decoder.decode(instruction);
			formatter.format(instruction, output);
			output.reset();
		}
	}

	static void formatTest(int bitness, String hexBytes, long ip, int code, int options, String formattedString, FastFormatter formatter) {
		Decoder decoder = createDecoder(bitness, hexBytes, ip, options);
		long nextRip = decoder.getIP();
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		assertEquals((short)nextRip, instruction.getIP16());
		assertEquals((int)nextRip, instruction.getIP32());
		assertEquals(nextRip, instruction.getIP());
		nextRip += instruction.getLength();
		assertEquals(nextRip, decoder.getIP());
		assertEquals((short)nextRip, instruction.getNextIP16());
		assertEquals((int)nextRip, instruction.getNextIP32());
		assertEquals(nextRip, instruction.getNextIP());
		formatTest(instruction, formattedString, formatter);
	}

	static void formatTest(Instruction instruction, String formattedString, FastFormatter formatter) {
		FastStringOutput output = new FastStringOutput();

		formatter.format(instruction, output);
		String actualFormattedString = output.toString();
		assertEquals(formattedString, actualFormattedString);
	}

	static void simpleFormatTest(int bitness, String hexBytes, long ip, int code, int options, String formattedString, FastFormatter formatter,
			Consumer<Decoder> initDecoder) {
		Function<Instruction, String> format = (Instruction instruction) -> {
			FastStringOutput output = new FastStringOutput();
			formatter.format(instruction, output);
			return output.toString();
		};
		simpleFormatTest(bitness, hexBytes, ip, code, options, formattedString, format, initDecoder);
	}
}
