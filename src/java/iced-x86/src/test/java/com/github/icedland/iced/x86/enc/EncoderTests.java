// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.CodeWriter;
import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderTestInfo;
import com.github.icedland.iced.x86.dec.DecoderTestUtils;
import com.github.icedland.iced.x86.dec.NonDecodedInstructions;
import com.github.icedland.iced.x86.dec.NonDecodedTestCase;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class EncoderTests {
	@ParameterizedTest
	@MethodSource("encode16Data")
	void encode16(int id, int bitness, int code, String hexBytes, long ip, String encodedHexBytes, int options) {
		encodeBase(id, bitness, code, hexBytes, ip, encodedHexBytes, options);
	}

	static Iterable<Arguments> encode16Data() {
		return getEncodeData(16);
	}

	@ParameterizedTest
	@MethodSource("nonDecodeEncode16Data")
	void nonDecodeEncode16(int bitness, Instruction instruction, String hexBytes, long rip) {
		nonDecodeEncodeBase(bitness, instruction, hexBytes, rip);
	}

	static Iterable<Arguments> nonDecodeEncode16Data() {
		return getNonDecodedEncodeData(16);
	}

	@ParameterizedTest
	@MethodSource("encode32Data")
	void encode32(int id, int bitness, int code, String hexBytes, long ip, String encodedHexBytes, int options) {
		encodeBase(id, bitness, code, hexBytes, ip, encodedHexBytes, options);
	}

	static Iterable<Arguments> encode32Data() {
		return getEncodeData(32);
	}

	@ParameterizedTest
	@MethodSource("nonDecodeEncode32Data")
	void nonDecodeEncode32(int bitness, Instruction instruction, String hexBytes, long rip) {
		nonDecodeEncodeBase(bitness, instruction, hexBytes, rip);
	}

	static Iterable<Arguments> nonDecodeEncode32Data() {
		return getNonDecodedEncodeData(32);
	}

	@ParameterizedTest
	@MethodSource("encode64Data")
	void encode64(int id, int bitness, int code, String hexBytes, long ip, String encodedHexBytes, int options) {
		encodeBase(id, bitness, code, hexBytes, ip, encodedHexBytes, options);
	}

	static Iterable<Arguments> encode64Data() {
		return getEncodeData(64);
	}

	@ParameterizedTest
	@MethodSource("nonDecodeEncode64Data")
	void nonDecodeEncode64(int bitness, Instruction instruction, String hexBytes, long rip) {
		nonDecodeEncodeBase(bitness, instruction, hexBytes, rip);
	}

	static Iterable<Arguments> nonDecodeEncode64Data() {
		return getNonDecodedEncodeData(64);
	}

	@ParameterizedTest
	@MethodSource("encodeInvalidData")
	void encodeInvalid(int id, int bitness, int code, String hexBytes, long ip, int options, int invalidCodeSize) {
		encodeInvalidBase(id, bitness, code, hexBytes, ip, options, invalidCodeSize);
	}

	static Iterable<Arguments> encodeInvalidData() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (DecoderTestInfo info : DecoderTestUtils.getEncoderTests(false, false)) {
			if (DecoderTestUtils.code32Only.contains(info.code))
				result.add(Arguments.of(info.id, info.bitness, info.code, info.hexBytes, info.ip, info.options, 64));
			if (DecoderTestUtils.code64Only.contains(info.code)) {
				result.add(Arguments.of(info.id, info.bitness, info.code, info.hexBytes, info.ip, info.options, 16));
				result.add(Arguments.of(info.id, info.bitness, info.code, info.hexBytes, info.ip, info.options, 32));
			}
		}
		return result;
	}

	private static String toString(byte[] bytes) {
		if (bytes.length == 0)
			return "";
		StringBuilder builder = new StringBuilder(bytes.length * 3 - 1);
		for (int i = 0; i < bytes.length; i++) {
			if (builder.length() > 0)
				builder.append(' ');
			builder.append(String.format("%02X", bytes[i]));
		}
		return builder.toString();
	}

	private static boolean arrayEquals(byte[] a, byte[] b) {
		if (a.length != b.length)
			return false;
		for (int i = 0; i < a.length; i++) {
			if (a[i] != b[i])
				return false;
		}
		return true;
	}

	private void encodeBase(int id, int bitness, int code, String hexBytes, long ip, String encodedHexBytes, int options) {
		byte[] origBytes = HexUtils.toByteArray(hexBytes);
		Decoder decoder = createDecoder(bitness, origBytes, ip, options);
		long origRip = decoder.getIP();
		Instruction origInstr = decoder.decode();
		ConstantOffsets origConstantOffsets = decoder.getConstantOffsets(origInstr);
		assertEquals(code, origInstr.getCode());
		assertEquals(origBytes.length, origInstr.getLength());
		assertTrue(origInstr.getLength() <= IcedConstants.MAX_INSTRUCTION_LENGTH);
		assertEquals((short)origRip, origInstr.getIP16());
		assertEquals((int)origRip, origInstr.getIP32());
		assertEquals(origRip, origInstr.getIP());
		long afterRip = decoder.getIP();
		assertEquals((short)afterRip, origInstr.getNextIP16());
		assertEquals((int)afterRip, origInstr.getNextIP32());
		assertEquals(afterRip, origInstr.getNextIP());

		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(decoder.getBitness(), writer);
		assertEquals(bitness, encoder.getBitness());
		Instruction origInstrCopy = origInstr.copy();
		Object result = encoder.tryEncode(origInstr, origRip);
		assertTrue(result instanceof Integer);
		int encodedInstrLen = (Integer)result;
		ConstantOffsets encodedConstantOffsets = encoder.getConstantOffsets();
		fixConstantOffsets(encodedConstantOffsets, origInstr.getLength(), encodedInstrLen);
		assertTrue(equals(origConstantOffsets, encodedConstantOffsets));
		byte[] encodedBytes = writer.toArray();
		assertEquals(encodedBytes.length, encodedInstrLen);
		assertTrue(origInstr.equalsAllBits(origInstrCopy));

		byte[] expectedBytes = HexUtils.toByteArray(encodedHexBytes);
		if (!arrayEquals(expectedBytes, encodedBytes)) {
			assertEquals(toString(expectedBytes), toString(encodedBytes));
			throw new UnsupportedOperationException();
		}

		Instruction newInstr = createDecoder(bitness, encodedBytes, ip, options).decode();
		assertEquals(code, newInstr.getCode());
		assertEquals(encodedBytes.length, newInstr.getLength());
		newInstr.setLength(origInstr.getLength());
		newInstr.setNextIP(origInstr.getNextIP());
		assertTrue(origInstr.equalsAllBits(newInstr));
	}

	private static void fixConstantOffsets(ConstantOffsets co, int origInstrLen, int newInstrLen) {
		byte diff = (byte)(origInstrLen - newInstrLen);
		if (co.hasDisplacement())
			co.displacementOffset += diff;
		if (co.hasImmediate())
			co.immediateOffset += diff;
		if (co.hasImmediate2())
			co.immediateOffset2 += diff;
	}

	private static boolean equals(ConstantOffsets a, ConstantOffsets b) {
		return a.displacementOffset == b.displacementOffset &&
				a.immediateOffset == b.immediateOffset &&
				a.immediateOffset2 == b.immediateOffset2 &&
				a.displacementSize == b.displacementSize &&
				a.immediateSize == b.immediateSize &&
				a.immediateSize2 == b.immediateSize2;
	}

	private void nonDecodeEncodeBase(int bitness, Instruction instruction, String hexBytes, long rip) {
		byte[] expectedBytes = HexUtils.toByteArray(hexBytes);
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(bitness, writer);
		assertEquals(bitness, encoder.getBitness());
		Object result = encoder.tryEncode(instruction, rip);
		assertTrue(result instanceof Integer);
		int encodedInstrLen = (Integer)result;
		byte[] encodedBytes = writer.toArray();
		if (!arrayEquals(expectedBytes, encodedBytes)) {
			assertEquals(toString(expectedBytes), toString(encodedBytes));
			throw new UnsupportedOperationException();
		}
		assertEquals(encodedBytes.length, encodedInstrLen);
	}

	private void encodeInvalidBase(int id, int bitness, int code, String hexBytes, long ip, int options, int invalidBitness) {
		byte[] origBytes = HexUtils.toByteArray(hexBytes);
		Decoder decoder = createDecoder(bitness, origBytes, ip, options);
		long origRip = decoder.getIP();
		Instruction origInstr = decoder.decode();
		assertEquals(code, origInstr.getCode());
		assertEquals(origBytes.length, origInstr.getLength());
		assertTrue(origInstr.getLength() <= IcedConstants.MAX_INSTRUCTION_LENGTH);
		assertEquals((short)origRip, origInstr.getIP16());
		assertEquals((int)origRip, origInstr.getIP32());
		assertEquals(origRip, origInstr.getIP());
		long afterRip = decoder.getIP();
		assertEquals((short)afterRip, origInstr.getNextIP16());
		assertEquals((int)afterRip, origInstr.getNextIP32());
		assertEquals(afterRip, origInstr.getNextIP());

		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = createEncoder(invalidBitness, writer);
		Object result = encoder.tryEncode(origInstr, origRip);
		assertTrue(result instanceof String);
		String errorMessage = (String)result;
		assertEquals(invalidBitness == 64 ? Encoder.ERROR_ONLY_1632_BIT_MODE : Encoder.ERROR_ONLY_64_BIT_MODE, errorMessage);
	}

	private Encoder createEncoder(int bitness, CodeWriter writer) {
		Encoder encoder = new Encoder(bitness, writer);
		assertEquals(bitness, encoder.getBitness());
		return encoder;
	}

	private Decoder createDecoder(int bitness, byte[] hexBytes, long ip, int options) {
		ByteArrayCodeReader codeReader = new ByteArrayCodeReader(hexBytes);
		Decoder decoder = new Decoder(bitness, codeReader, options);
		decoder.setIP(ip);
		assertEquals(bitness, decoder.getBitness());
		return decoder;
	}

	private static Iterable<Arguments> getEncodeData(int bitness) {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (DecoderTestInfo info : DecoderTestUtils.getEncoderTests(true, false)) {
			if (bitness != info.bitness)
				continue;
			result.add(Arguments.of(info.id, info.bitness, info.code, info.hexBytes, info.ip, info.encodedHexBytes, info.options));
		}
		return result;
	}

	private static Iterable<Arguments> getNonDecodedEncodeData(int bitness) {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (NonDecodedTestCase info : NonDecodedInstructions.getTests()) {
			if (bitness != info.bitness)
				continue;
			long rip = 0;
			result.add(Arguments.of(info.bitness, info.instruction, info.hexBytes, rip));
		}
		return result;
	}
}
