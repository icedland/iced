// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.BitnessUtils;
import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeWriterImpl;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.ICRegister;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemoryOperand;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RepPrefixKind;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class MiscTests {
	@Test
	@SuppressWarnings("deprecation")
	void encode_INVALID_Code_value_is_an_error() {
		Encoder encoder;
		Instruction instruction = new Instruction();
		instruction.setCode(Code.INVALID);
		Object result;
		final String ERROR_MESSAGE = com.github.icedland.iced.x86.enc.InternalOpCodeHandlers.InvalidHandler.ERROR_MESSAGE;

		encoder = new Encoder(16, new CodeWriterImpl());
		result = encoder.tryEncode(instruction, 0);
		assertTrue(result instanceof String);
		assertEquals(ERROR_MESSAGE, (String)result);

		encoder = new Encoder(32, new CodeWriterImpl());
		result = encoder.tryEncode(instruction, 0);
		assertTrue(result instanceof String);
		assertEquals(ERROR_MESSAGE, (String)result);

		encoder = new Encoder(64, new CodeWriterImpl());
		result = encoder.tryEncode(instruction, 0);
		assertTrue(result instanceof String);
		assertEquals(ERROR_MESSAGE, (String)result);
	}

	@Test
	void encode_throws() {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.INVALID);
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		assertThrows(EncoderException.class, () -> {
			Instruction instrCopy = instruction.copy();
			encoder.encode(instrCopy, 0);
		});
	}

	@ParameterizedTest
	@MethodSource("displSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte_Data")
	void displSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte(int bitness, String hexBytes, long rip, Instruction instruction) {
		byte[] expectedBytes = HexUtils.toByteArray(hexBytes);
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		Encoder encoder = new Encoder(bitness, codeWriter);
		Object result = encoder.tryEncode(instruction, rip);
		assertTrue(result instanceof Integer);
		assertArrayEquals(expectedBytes, codeWriter.toArray());
		assertEquals(expectedBytes.length, (Integer)result);
	}

	static Iterable<Arguments> displSize_eq_1_uses_long_form_if_it_does_not_fit_in_1_byte_Data() {
		final long rip = 0L;

		MemoryOperand memory16 = new MemoryOperand(ICRegisters.si, 0x1234, 1);
		MemoryOperand memory32 = new MemoryOperand(ICRegisters.esi, 0x12345678, 1);
		MemoryOperand memory64 = new MemoryOperand(ICRegisters.r14, 0x12345678, 1);

		ArrayList<Arguments> result = new ArrayList<Arguments>();

		result.add(Arguments.of(16, "0F10 8C 3412", rip, Instruction.create(Code.MOVUPS_XMM_XMMM128, ICRegisters.xmm1, memory16)));
		result.add(Arguments.of(32, "0F10 8E 78563412", rip, Instruction.create(Code.MOVUPS_XMM_XMMM128, ICRegisters.xmm1, memory32)));
		result.add(Arguments.of(64, "41 0F10 8E 78563412", rip, Instruction.create(Code.MOVUPS_XMM_XMMM128, ICRegisters.xmm1, memory64)));

		result.add(Arguments.of(16, "C5F8 10 8C 3412", rip, Instruction.create(Code.VEX_VMOVUPS_XMM_XMMM128, ICRegisters.xmm1, memory16)));
		result.add(Arguments.of(32, "C5F8 10 8E 78563412", rip, Instruction.create(Code.VEX_VMOVUPS_XMM_XMMM128, ICRegisters.xmm1, memory32)));
		result.add(Arguments.of(64, "C4C178 10 8E 78563412", rip, Instruction.create(Code.VEX_VMOVUPS_XMM_XMMM128, ICRegisters.xmm1, memory64)));

		result.add(Arguments.of(16, "62 F17C08 10 8C 3412", rip, Instruction.create(Code.EVEX_VMOVUPS_XMM_K1Z_XMMM128, ICRegisters.xmm1, memory16)));
		result.add(Arguments.of(32, "62 F17C08 10 8E 78563412", rip, Instruction.create(Code.EVEX_VMOVUPS_XMM_K1Z_XMMM128, ICRegisters.xmm1, memory32)));
		result.add(Arguments.of(64, "62 D17C08 10 8E 78563412", rip, Instruction.create(Code.EVEX_VMOVUPS_XMM_K1Z_XMMM128, ICRegisters.xmm1, memory64)));

		result.add(Arguments.of(16, "8F E878C0 8C 3412 A5", rip, Instruction.create(Code.XOP_VPROTB_XMM_XMMM128_IMM8, ICRegisters.xmm1, memory16, 0xA5)));
		result.add(Arguments.of(32, "8F E878C0 8E 78563412 A5", rip, Instruction.create(Code.XOP_VPROTB_XMM_XMMM128_IMM8, ICRegisters.xmm1, memory32, 0xA5)));
		result.add(Arguments.of(64, "8F C878C0 8E 78563412 A5", rip, Instruction.create(Code.XOP_VPROTB_XMM_XMMM128_IMM8, ICRegisters.xmm1, memory64, 0xA5)));

		result.add(Arguments.of(16, "0F0F 8C 3412 0C", rip, Instruction.create(Code.D3NOW_PI2FW_MM_MMM64, ICRegisters.mm1, memory16)));
		result.add(Arguments.of(32, "0F0F 8E 78563412 0C", rip, Instruction.create(Code.D3NOW_PI2FW_MM_MMM64, ICRegisters.mm1, memory32)));
		result.add(Arguments.of(64, "0F0F 8E 78563412 0C", rip, Instruction.create(Code.D3NOW_PI2FW_MM_MMM64, ICRegisters.mm1, memory64)));

		result.add(Arguments.of(64, "62 D17808 28 8E 78563412", rip, Instruction.create(Code.MVEX_VMOVAPS_ZMM_K1_ZMMMT, ICRegisters.zmm1, memory64)));

		// If it fails, add more tests above (16-bit, 32-bit, and 64-bit test cases)
		assertEquals(6, IcedConstants.ENCODING_KIND_ENUM_COUNT);

		return result;
	}

	@Test
	void encode_BP_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(16, writer);
		Instruction instruction = Instruction.create(Code.MOV_R16_RM16, ICRegisters.ax, new MemoryOperand(ICRegisters.bp));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { (byte)0x8B, 0x46, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_EBP_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(32, writer);
		Instruction instruction = Instruction.create(Code.MOV_R32_RM32, ICRegisters.eax, new MemoryOperand(ICRegisters.ebp));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { (byte)0x8B, 0x45, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_EBP_EDX_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(32, writer);
		Instruction instruction = Instruction.create(Code.MOV_R32_RM32, ICRegisters.eax, new MemoryOperand(ICRegisters.ebp, ICRegisters.edx));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { (byte)0x8B, 0x44, 0x15, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_R13D_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, writer);
		Instruction instruction = Instruction.create(Code.MOV_R32_RM32, ICRegisters.eax, new MemoryOperand(ICRegisters.r13d));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { 0x67, 0x41, (byte)0x8B, 0x45, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_R13D_EDX_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, writer);
		Instruction instruction = Instruction.create(Code.MOV_R32_RM32, ICRegisters.eax, new MemoryOperand(ICRegisters.r13d, ICRegisters.edx));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { 0x67, 0x41, (byte)0x8B, 0x44, 0x15, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_RBP_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, writer);
		Instruction instruction = Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, new MemoryOperand(ICRegisters.rbp));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { 0x48, (byte)0x8B, 0x45, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_RBP_RDX_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, writer);
		Instruction instruction = Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, new MemoryOperand(ICRegisters.rbp, ICRegisters.rdx));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { 0x48, (byte)0x8B, 0x44, 0x15, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_R13_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, writer);
		Instruction instruction = Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, new MemoryOperand(ICRegisters.r13));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { 0x49, (byte)0x8B, 0x45, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@Test
	void encode_R13_RDX_with_no_displ() {
		CodeWriterImpl writer = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, writer);
		Instruction instruction = Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, new MemoryOperand(ICRegisters.r13, ICRegisters.rdx));
		int len = encoder.encode(instruction, 0);
		byte[] expected = new byte[] { 0x49, (byte)0x8B, 0x44, 0x15, 0x00 };
		byte[] actual = writer.toArray();
		assertEquals(actual.length, len);
		assertArrayEquals(expected, actual);
	}

	@ParameterizedTest
	@ValueSource(ints = {16, 32, 64})
	void verify_encoder_options(int bitness) {
		Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
		assertFalse(encoder.getPreventVEX2());
		assertEquals(0, encoder.getVEX_WIG());
		assertEquals(0, encoder.getVEX_LIG());
		assertEquals(0, encoder.getEVEX_WIG());
		assertEquals(0, encoder.getEVEX_LIG());
		assertEquals(0, encoder.getMVEX_WIG());
	}

	@ParameterizedTest
	@ValueSource(ints = {16, 32, 64})
	void getSet_WIG_LIG_options(int bitness) {
		Encoder encoder = new Encoder(bitness, new CodeWriterImpl());

		encoder.setVEX_LIG(1);
		encoder.setVEX_WIG(0);
		assertEquals(0, encoder.getVEX_WIG());
		assertEquals(1, encoder.getVEX_LIG());
		encoder.setVEX_WIG(1);
		assertEquals(1, encoder.getVEX_WIG());
		assertEquals(1, encoder.getVEX_LIG());

		encoder.setVEX_WIG(0xFFFFFFFE);
		assertEquals(0, encoder.getVEX_WIG());
		assertEquals(1, encoder.getVEX_LIG());
		encoder.setVEX_WIG(0xFFFFFFFF);
		assertEquals(1, encoder.getVEX_WIG());
		assertEquals(1, encoder.getVEX_LIG());

		encoder.setVEX_WIG(1);
		encoder.setVEX_LIG(0);
		assertEquals(0, encoder.getVEX_LIG());
		assertEquals(1, encoder.getVEX_WIG());
		encoder.setVEX_LIG(1);
		assertEquals(1, encoder.getVEX_LIG());
		assertEquals(1, encoder.getVEX_WIG());

		encoder.setVEX_LIG(0xFFFFFFFE);
		assertEquals(0, encoder.getVEX_LIG());
		assertEquals(1, encoder.getVEX_WIG());
		encoder.setVEX_LIG(0xFFFFFFFF);
		assertEquals(1, encoder.getVEX_LIG());
		assertEquals(1, encoder.getVEX_WIG());

		encoder.setEVEX_LIG(3);
		encoder.setEVEX_WIG(0);
		assertEquals(0, encoder.getEVEX_WIG());
		assertEquals(3, encoder.getEVEX_LIG());
		encoder.setEVEX_WIG(1);
		assertEquals(1, encoder.getEVEX_WIG());
		assertEquals(3, encoder.getEVEX_LIG());

		encoder.setEVEX_WIG(0xFFFFFFFE);
		assertEquals(0, encoder.getEVEX_WIG());
		assertEquals(3, encoder.getEVEX_LIG());
		encoder.setEVEX_WIG(0xFFFFFFFF);
		assertEquals(1, encoder.getEVEX_WIG());
		assertEquals(3, encoder.getEVEX_LIG());

		encoder.setEVEX_WIG(1);
		encoder.setEVEX_LIG(0);
		assertEquals(0, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());
		encoder.setEVEX_LIG(1);
		assertEquals(1, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());
		encoder.setEVEX_LIG(2);
		assertEquals(2, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());
		encoder.setEVEX_LIG(3);
		assertEquals(3, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());

		encoder.setEVEX_LIG(0xFFFFFFFC);
		assertEquals(0, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());
		encoder.setEVEX_LIG(0xFFFFFFFD);
		assertEquals(1, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());
		encoder.setEVEX_LIG(0xFFFFFFFE);
		assertEquals(2, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());
		encoder.setEVEX_LIG(0xFFFFFFFF);
		assertEquals(3, encoder.getEVEX_LIG());
		assertEquals(1, encoder.getEVEX_WIG());

		encoder.setMVEX_WIG(0);
		assertEquals(0, encoder.getMVEX_WIG());
		encoder.setMVEX_WIG(1);
		assertEquals(1, encoder.getMVEX_WIG());

		encoder.setMVEX_WIG(0xFFFFFFFE);
		assertEquals(0, encoder.getMVEX_WIG());
		encoder.setMVEX_WIG(0xFFFFFFFF);
		assertEquals(1, encoder.getMVEX_WIG());
	}

	@ParameterizedTest
	@MethodSource("prevent_VEX2_encoding_Data")
	void prevent_VEX2_encoding(String hexBytes, String expectedBytes, int code, boolean preventVEX2) {
		Decoder decoder = new Decoder(64, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(DecoderConstants.DEFAULT_IP64);
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		Encoder encoder = new Encoder(decoder.getBitness(), codeWriter);
		encoder.setPreventVEX2(preventVEX2);
		encoder.encode(instruction, DecoderConstants.DEFAULT_IP64);
		byte[] encodedBytes = codeWriter.toArray();
		byte[] expectedBytesArray = HexUtils.toByteArray(expectedBytes);
		assertArrayEquals(expectedBytesArray, encodedBytes);
	}

	static Iterable<Arguments> prevent_VEX2_encoding_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		result.add(Arguments.of("C5FC 10 10", "C4E17C 10 10", Code.VEX_VMOVUPS_YMM_YMMM256, true));
		result.add(Arguments.of("C5FC 10 10", "C5FC 10 10", Code.VEX_VMOVUPS_YMM_YMMM256, false));
		return result;
	}

	@ParameterizedTest
	@MethodSource("test_VEX_WIG_LIG_Data")
	void test_VEX_WIG_LIG(String hexBytes, String expectedBytes, int code, int wig, int lig) {
		Decoder decoder = new Decoder(64, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(DecoderConstants.DEFAULT_IP64);
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		Encoder encoder = new Encoder(decoder.getBitness(), codeWriter);
		encoder.setVEX_WIG(wig);
		encoder.setVEX_LIG(lig);
		encoder.encode(instruction, DecoderConstants.DEFAULT_IP64);
		byte[] encodedBytes = codeWriter.toArray();
		byte[] expectedBytesArray = HexUtils.toByteArray(expectedBytes);
		assertArrayEquals(expectedBytesArray, encodedBytes);
	}

	static Iterable<Arguments> test_VEX_WIG_LIG_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		result.add(Arguments.of("C5CA 10 CD", "C5CA 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 0, 0));
		result.add(Arguments.of("C5CA 10 CD", "C5CE 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 0, 1));
		result.add(Arguments.of("C5CA 10 CD", "C5CA 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 1, 0));
		result.add(Arguments.of("C5CA 10 CD", "C5CE 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 1, 1));

		result.add(Arguments.of("C4414A 10 CD", "C4414A 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 0, 0));
		result.add(Arguments.of("C4414A 10 CD", "C4414E 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 0, 1));
		result.add(Arguments.of("C4414A 10 CD", "C441CA 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 1, 0));
		result.add(Arguments.of("C4414A 10 CD", "C441CE 10 CD", Code.VEX_VMOVSS_XMM_XMM_XMM, 1, 1));

		result.add(Arguments.of("C5F9 50 D3", "C5F9 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 0, 0));
		result.add(Arguments.of("C5F9 50 D3", "C5F9 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 0, 1));
		result.add(Arguments.of("C5F9 50 D3", "C5F9 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 1, 0));
		result.add(Arguments.of("C5F9 50 D3", "C5F9 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 1, 1));

		result.add(Arguments.of("C4C179 50 D3", "C4C179 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 0, 0));
		result.add(Arguments.of("C4C179 50 D3", "C4C179 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 0, 1));
		result.add(Arguments.of("C4C179 50 D3", "C4C179 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 1, 0));
		result.add(Arguments.of("C4C179 50 D3", "C4C179 50 D3", Code.VEX_VMOVMSKPD_R32_XMM, 1, 1));
		return result;
	}

	@ParameterizedTest
	@MethodSource("test_EVEX_WIG_LIG_Data")
	void test_EVEX_WIG_LIG(String hexBytes, String expectedBytes, int code, int wig, int lig) {
		Decoder decoder = new Decoder(64, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(DecoderConstants.DEFAULT_IP64);
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		Encoder encoder = new Encoder(decoder.getBitness(), codeWriter);
		encoder.setEVEX_WIG(wig);
		encoder.setEVEX_LIG(lig);
		encoder.encode(instruction, DecoderConstants.DEFAULT_IP64);
		byte[] encodedBytes = codeWriter.toArray();
		byte[] expectedBytesArray = HexUtils.toByteArray(expectedBytes);
		assertArrayEquals(expectedBytesArray, encodedBytes);
	}

	static Iterable<Arguments> test_EVEX_WIG_LIG_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E08 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 0, 0));
		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E28 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 0, 1));
		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E48 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 0, 2));
		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E68 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 0, 3));

		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E08 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 1, 0));
		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E28 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 1, 1));
		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E48 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 1, 2));
		result.add(Arguments.of("62 F14E08 10 D3", "62 F14E68 10 D3", Code.EVEX_VMOVSS_XMM_K1Z_XMM_XMM, 1, 3));

		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 0, 0));
		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 0, 1));
		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 0, 2));
		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F14D0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 0, 3));

		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 1, 0));
		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 1, 1));
		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 1, 2));
		result.add(Arguments.of("62 F14D0B 60 50 01", "62 F1CD0B 60 50 01", Code.EVEX_VPUNPCKLBW_XMM_K1Z_XMM_XMMM128, 1, 3));

		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 0, 0));
		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 0, 1));
		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 0, 2));
		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 0, 3));

		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 1, 0));
		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 1, 1));
		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 1, 2));
		result.add(Arguments.of("62 F17C0B 51 50 01", "62 F17C0B 51 50 01", Code.EVEX_VSQRTPS_XMM_K1Z_XMMM128B32, 1, 3));
		return result;
	}

	@Test
	void test_new_Encoder_throws() {
		for (int bitness : BitnessUtils.getInvalidBitnessValues())
			assertThrows(IllegalArgumentException.class, () -> new Encoder(bitness, new CodeWriterImpl()));

		for (int bitness : new int[] { 16, 32, 64 })
			assertThrows(NullPointerException.class, () -> new Encoder(bitness, null));
	}

	@Test
	void toOpCode_throws_if_input_is_invalid() {
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.toOpCode(-0x8000_0000));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.toOpCode(-1));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.toOpCode(IcedConstants.CODE_ENUM_COUNT));
		assertThrows(ArrayIndexOutOfBoundsException.class, () -> Code.toOpCode(0x7FFF_FFFF));
	}

	@Test
	void verify_MemoryOperand_ctors() {
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, ICRegisters.rsi, 4, -0x1234_5678_9ABC_DEF1L, 8, true, ICRegisters.fs);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegisters.rsi, op.index);
			assertEquals(4, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(8, op.displSize);
			assertTrue(op.isBroadcast);
			assertEquals(ICRegisters.fs, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, ICRegisters.rsi, 4, true, ICRegisters.fs);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegisters.rsi, op.index);
			assertEquals(4, op.scale);
			assertEquals(0, op.displacement);
			assertEquals(0, op.displSize);
			assertTrue(op.isBroadcast);
			assertEquals(ICRegisters.fs, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, -0x1234_5678_9ABC_DEF1L, 8, true, ICRegisters.fs);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegister.NONE, op.index);
			assertEquals(1, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(8, op.displSize);
			assertTrue(op.isBroadcast);
			assertEquals(ICRegisters.fs, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rsi, 4, -0x1234_5678_9ABC_DEF1L, 8, true, ICRegisters.fs);
			assertEquals(ICRegister.NONE, op.base);
			assertEquals(ICRegisters.rsi, op.index);
			assertEquals(4, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(8, op.displSize);
			assertTrue(op.isBroadcast);
			assertEquals(ICRegisters.fs, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, -0x1234_5678_9ABC_DEF1L, true, ICRegisters.fs);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegister.NONE, op.index);
			assertEquals(1, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(1, op.displSize);
			assertTrue(op.isBroadcast);
			assertEquals(ICRegisters.fs, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, ICRegisters.rsi, 4, -0x1234_5678_9ABC_DEF1L, 8);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegisters.rsi, op.index);
			assertEquals(4, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(8, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, ICRegisters.rsi, 4);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegisters.rsi, op.index);
			assertEquals(4, op.scale);
			assertEquals(0, op.displacement);
			assertEquals(0, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, ICRegisters.rsi);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegisters.rsi, op.index);
			assertEquals(1, op.scale);
			assertEquals(0, op.displacement);
			assertEquals(0, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, -0x1234_5678_9ABC_DEF1L, 8);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegister.NONE, op.index);
			assertEquals(1, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(8, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rsi, 4, -0x1234_5678_9ABC_DEF1L, 8);
			assertEquals(ICRegister.NONE, op.base);
			assertEquals(ICRegisters.rsi, op.index);
			assertEquals(4, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(8, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx, -0x1234_5678_9ABC_DEF1L);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegister.NONE, op.index);
			assertEquals(1, op.scale);
			assertEquals(-0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(1, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(ICRegisters.rcx);
			assertEquals(ICRegisters.rcx, op.base);
			assertEquals(ICRegister.NONE, op.index);
			assertEquals(1, op.scale);
			assertEquals(0, op.displacement);
			assertEquals(0, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
		{
			MemoryOperand op = new MemoryOperand(0x1234_5678_9ABC_DEF1L, 8);
			assertEquals(ICRegister.NONE, op.base);
			assertEquals(ICRegister.NONE, op.index);
			assertEquals(1, op.scale);
			assertEquals(0x1234_5678_9ABC_DEF1L, op.displacement);
			assertEquals(8, op.displSize);
			assertFalse(op.isBroadcast);
			assertEquals(ICRegister.NONE, op.segmentPrefix);
		}
	}

	@Test
	void opCodeInfo_IsAvailableInMode_throws_if_invalid_bitness() {
		for (int bitness : BitnessUtils.getInvalidBitnessValues())
			assertThrows(IllegalArgumentException.class, () -> Code.toOpCode(Code.NOPD).isAvailableInMode(bitness));
	}

	@Test
	void writeByte_works() {
		CodeWriterImpl codeWriter = new CodeWriterImpl();
		Encoder encoder = new Encoder(64, codeWriter);
		Instruction instruction = Instruction.create(Code.ADD_R64_RM64, ICRegisters.r8, ICRegisters.rbp);
		encoder.writeByte((byte)0x90);
		encoder.encode(instruction, 0x55555555);
		encoder.writeByte((byte)0xCC);
		assertArrayEquals(new byte[] { (byte)0x90, 0x4C, 0x03, (byte)0xC5, (byte)0xCC }, codeWriter.toArray());
	}

	@ParameterizedTest
	@MethodSource("encodeInvalidRegOpSize_Data")
	void encodeInvalidRegOpSize(int bitness, Instruction instruction) {
		Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
		Object result = encoder.tryEncode(instruction, 0);
		assertTrue(result instanceof String);
		assertTrue(((String)result).contains("Register operand size must equal memory addressing mode (16/32/64)"));
	}

	static Iterable<Arguments> encodeInvalidRegOpSize_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();

		result.add(Arguments.of(16, Instruction.create(Code.MOVDIR64B_R16_M512, ICRegisters.cx, new MemoryOperand(ICRegisters.ebx))));
		result.add(Arguments.of(32, Instruction.create(Code.MOVDIR64B_R16_M512, ICRegisters.cx, new MemoryOperand(ICRegisters.ebx))));

		result.add(Arguments.of(16, Instruction.create(Code.MOVDIR64B_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.bx))));
		result.add(Arguments.of(32, Instruction.create(Code.MOVDIR64B_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.bx))));
		result.add(Arguments.of(64, Instruction.create(Code.MOVDIR64B_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.rbx))));

		result.add(Arguments.of(64, Instruction.create(Code.MOVDIR64B_R64_M512, ICRegisters.rcx, new MemoryOperand(ICRegisters.ebx))));

		result.add(Arguments.of(16, Instruction.create(Code.ENQCMDS_R16_M512, ICRegisters.cx, new MemoryOperand(ICRegisters.ebx))));
		result.add(Arguments.of(32, Instruction.create(Code.ENQCMDS_R16_M512, ICRegisters.cx, new MemoryOperand(ICRegisters.ebx))));

		result.add(Arguments.of(16, Instruction.create(Code.ENQCMDS_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.bx))));
		result.add(Arguments.of(32, Instruction.create(Code.ENQCMDS_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.bx))));
		result.add(Arguments.of(64, Instruction.create(Code.ENQCMDS_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.rbx))));

		result.add(Arguments.of(64, Instruction.create(Code.ENQCMDS_R64_M512, ICRegisters.rcx, new MemoryOperand(ICRegisters.ebx))));

		result.add(Arguments.of(16, Instruction.create(Code.ENQCMD_R16_M512, ICRegisters.cx, new MemoryOperand(ICRegisters.ebx))));
		result.add(Arguments.of(32, Instruction.create(Code.ENQCMD_R16_M512, ICRegisters.cx, new MemoryOperand(ICRegisters.ebx))));

		result.add(Arguments.of(16, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.bx))));
		result.add(Arguments.of(32, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.bx))));
		result.add(Arguments.of(64, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.ecx, new MemoryOperand(ICRegisters.rbx))));

		result.add(Arguments.of(64, Instruction.create(Code.ENQCMD_R64_M512, ICRegisters.rcx, new MemoryOperand(ICRegisters.ebx))));

		return result;
	}

	private static boolean encodeOk(int bitness, Instruction instruction) {
		Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
		return encoder.tryEncode(instruction, 0x1234) instanceof Integer;
	}

	private static boolean encodeErr(int bitness, Instruction instruction) {
		Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
		return encoder.tryEncode(instruction, 0x1234) instanceof String;
	}

	@Test
	void invalidDispl16() {
		final int bitness = 16;

		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_0000L, 2))));
		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_FFFFL, 2))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x1_0000L, 2))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFL, 2))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(0x0_0000L, 2))));
		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(0x0_FFFFL, 2))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(0x1_0000L, 2))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFL, 2))));

		for (int displSize : new int[] { 1, 2 }) {
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 0L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, -1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, -0x0_8000, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 0x0_FFFF, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, -0x0_8001, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 0x1_0000, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, -0x8000_0000_0000_0000L, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 0x7FFF_FFFF_FFFF_FFFFL, displSize))));
		}

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bp, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bp, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bp, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -1, 0))));

		for (int displSize : new int[] { 1, 4 }) {
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -0x8000_0000L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0xFFFF_FFFFL, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -0x8000_0001L, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0x1_0000_0000L, displSize))));
		}
	}

	@Test
	void invalidDispl32() {
		final int bitness = 32;

		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_0000L, 2))));
		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_FFFFL, 2))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x1_0000L, 2))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFL, 2))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_0000_0000L, 4))));
		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_FFFF_FFFFL, 4))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x1_0000_0000L, 4))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFL, 4))));

		for (int displSize : new int[] { 1, 4 }) {
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 0L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, -1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, -0x8000_0000L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 0xFFFF_FFFFL, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, -0x8000_0001L, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 0x1_0000_0000L, displSize))));
		}

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bp, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bp, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bp, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -1, 0))));

		for (int displSize : new int[] { 1, 4 }) {
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -0x8000_0000L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0xFFFF_FFFFL, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -0x8000_0001L, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0x1_0000_0000L, displSize))));
		}
	}

	@Test
	void invalidDispl64() {
		final int bitness = 64;

		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_0000_0000L, 4))));
		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0_FFFF_FFFFL, 4))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x1_0000_0000L, 4))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFL, 4))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0x0000_0000_0000_0000L, 8))));
		assertTrue(encodeOk(bitness, Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(0xFFFF_FFFF_FFFF_FFFFL, 8))));

		for (int displSize : new int[] { 1, 8 }) {
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 0L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, -1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, -0x8000_0000L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 0x7FFF_FFFF, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, -0x8000_0001L, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, 0x8000_0000L, displSize))));
		}

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebp, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.r13d, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.r13d, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.r13d, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbp, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbp, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbp, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.r13, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.r13, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.r13, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -1, 0))));

		assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, 0L, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, 1, 0))));
		assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, -1, 0))));

		for (int displSize : new int[] { 1, 4 }) {
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -0x8000_0000L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0xFFFF_FFFFL, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, -0x8000_0001L, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.ebx, 0x1_0000_0000L, displSize))));
		}

		for (int displSize : new int[] { 1, 8 }) {
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, 0L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, -1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, 1, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, -0x8000_0000L, displSize))));
			assertTrue(encodeOk(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, 0x7FFF_FFFF, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, -0x8000_0001L, displSize))));
			assertTrue(encodeErr(bitness, Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rbx, 0x8000_0000L, displSize))));
		}
	}

	@Test
	void testUnsupportedBitness() {
		{
			Encoder encoder = new Encoder(16, new CodeWriterImpl());
			assertTrue(encoder.tryEncode(Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, ICRegisters.rcx), 0) instanceof String);
		}
		{
			Encoder encoder = new Encoder(32, new CodeWriterImpl());
			assertTrue(encoder.tryEncode(Instruction.create(Code.MOV_R64_RM64, ICRegisters.rax, ICRegisters.rcx), 0) instanceof String);
		}
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			assertTrue(encoder.tryEncode(Instruction.create(Code.PUSHAD), 0) instanceof String);
		}
	}

	@Test
	void testTooLongInstruction() {
		Encoder encoder = new Encoder(16, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.ADD_RM32_IMM32,
			new MemoryOperand(ICRegisters.esp, ICRegister.NONE, 1, 0x1234_5678, 4, false, ICRegisters.ss), 0x1234_5678);
		instr.setXacquirePrefix(true);
		instr.setLockPrefix(true);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testWrongOpKind() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.PUSH_R64, ICRegisters.rax);
		instr.setOp0Kind(OpKind.IMMEDIATE16);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testWrongImpliedRegister() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.IN_AL_DX, ICRegisters.rax, ICRegisters.edx);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testWrongRegister() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.PUSH_R64, ICRegisters.eax);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	static class TupleRegReg {
		public final ICRegister reg1;
		public final ICRegister reg2;
		public TupleRegReg(ICRegister reg1, ICRegister reg2) {
			this.reg1 = reg1;
			this.reg2 = reg2;
		}
	}

	static class TupleRegInt {
		public final ICRegister reg;
		public final int value;
		public TupleRegInt(ICRegister reg, int value) {
			this.reg = reg;
			this.value = value;
		}
	}

	static class TupleIntInt {
		public final int value1;
		public final int value2;
		public TupleIntInt(int value1, int value2) {
			this.value1 = value1;
			this.value2 = value2;
		}
	}

	static class TupleLongInt {
		public long value1;
		public int value2;
		public TupleLongInt(long value1, int value2) {
			this.value1 = value1;
			this.value2 = value2;
		}
	}

	static class TupleIntLongInt {
		public int value1;
		public long value2;
		public int value3;
		public TupleIntLongInt(int value1, long value2, int value3) {
			this.value1 = value1;
			this.value2 = value2;
			this.value3 = value3;
		}
	}

	static class TupleIntInstrInt {
		public int value1;
		public Instruction instr;
		public int value2;
		public TupleIntInstrInt(int value1, Instruction instr, int value2) {
			this.value1 = value1;
			this.instr = instr;
			this.value2 = value2;
		}
	}

	static class TupleIntInstrIntInt {
		public int value1;
		public Instruction instr;
		public int value2;
		public int value3;
		public TupleIntInstrIntInt(int value1, Instruction instr, int value2, int value3) {
			this.value1 = value1;
			this.instr = instr;
			this.value2 = value2;
			this.value3 = value3;
		}
	}

	@Test
	void testInvalidMaskmov() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.createMaskmovq(16, ICRegisters.mm0, ICRegisters.mm1, ICRegister.NONE), OpKind.MEMORY_SEG_RDI),
			new TupleIntInstrInt(16, Instruction.createMaskmovdqu(16, ICRegisters.xmm0, ICRegisters.xmm1, ICRegister.NONE), OpKind.MEMORY_SEG_RDI),
			new TupleIntInstrInt(16, Instruction.createVmaskmovdqu(16, ICRegisters.xmm0, ICRegisters.xmm1, ICRegister.NONE), OpKind.MEMORY_SEG_RDI),
			new TupleIntInstrInt(32, Instruction.createMaskmovq(32, ICRegisters.mm0, ICRegisters.mm1, ICRegister.NONE), OpKind.MEMORY_SEG_RDI),
			new TupleIntInstrInt(32, Instruction.createMaskmovdqu(32, ICRegisters.xmm0, ICRegisters.xmm1, ICRegister.NONE), OpKind.MEMORY_SEG_RDI),
			new TupleIntInstrInt(32, Instruction.createVmaskmovdqu(32, ICRegisters.xmm0, ICRegisters.xmm1, ICRegister.NONE), OpKind.MEMORY_SEG_RDI),
			new TupleIntInstrInt(64, Instruction.createMaskmovq(64, ICRegisters.mm0, ICRegisters.mm1, ICRegister.NONE), OpKind.MEMORY_SEG_DI),
			new TupleIntInstrInt(64, Instruction.createMaskmovdqu(64, ICRegisters.xmm0, ICRegisters.xmm1, ICRegister.NONE), OpKind.MEMORY_SEG_DI),
			new TupleIntInstrInt(64, Instruction.createVmaskmovdqu(64, ICRegisters.xmm0, ICRegisters.xmm1, ICRegister.NONE), OpKind.MEMORY_SEG_DI),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidOuts() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.createOutsb(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(16, Instruction.createOutsw(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(16, Instruction.createOutsd(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(32, Instruction.createOutsb(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(32, Instruction.createOutsw(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(32, Instruction.createOutsd(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(64, Instruction.createOutsb(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI),
			new TupleIntInstrInt(64, Instruction.createOutsw(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI),
			new TupleIntInstrInt(64, Instruction.createOutsd(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp1Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp1Kind(badOpKind);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidMovs() {
		TupleIntInstrIntInt[] tests = new TupleIntInstrIntInt[] {
			new TupleIntInstrIntInt(16, Instruction.createMovsb(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(16, Instruction.createMovsw(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(16, Instruction.createMovsd(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(16, Instruction.createMovsq(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createMovsb(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createMovsw(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createMovsd(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createMovsq(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(64, Instruction.createMovsb(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
			new TupleIntInstrIntInt(64, Instruction.createMovsw(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
			new TupleIntInstrIntInt(64, Instruction.createMovsd(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
			new TupleIntInstrIntInt(64, Instruction.createMovsq(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
		};
		for (TupleIntInstrIntInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind1 = info.value2;
			int badOpKind0 = info.value3;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(OpKind.FAR_BRANCH16);
				instr.setOp1Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}

			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind1);
				instr.setOp1Kind(badOpKind1);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp1Kind(badOpKind1);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}

			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind0);
				instr.setOp1Kind(badOpKind0);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind0);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidCmps() {
		TupleIntInstrIntInt[] tests = new TupleIntInstrIntInt[] {
			new TupleIntInstrIntInt(16, Instruction.createCmpsb(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(16, Instruction.createCmpsw(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(16, Instruction.createCmpsd(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(16, Instruction.createCmpsq(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createCmpsb(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createCmpsw(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createCmpsd(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(32, Instruction.createCmpsq(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI, OpKind.MEMORY_ESRDI),
			new TupleIntInstrIntInt(64, Instruction.createCmpsb(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
			new TupleIntInstrIntInt(64, Instruction.createCmpsw(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
			new TupleIntInstrIntInt(64, Instruction.createCmpsd(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
			new TupleIntInstrIntInt(64, Instruction.createCmpsq(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI, OpKind.MEMORY_ESDI),
		};
		for (TupleIntInstrIntInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind1 = info.value2;
			int badOpKind0 = info.value3;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(OpKind.FAR_BRANCH16);
				instr.setOp1Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}

			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind1);
				instr.setOp1Kind(badOpKind1);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp1Kind(badOpKind1);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}

			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind0);
				instr.setOp1Kind(badOpKind0);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind0);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidLods() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.createLodsb(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(16, Instruction.createLodsw(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(16, Instruction.createLodsd(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(16, Instruction.createLodsq(16, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(32, Instruction.createLodsb(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(32, Instruction.createLodsw(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(32, Instruction.createLodsd(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(32, Instruction.createLodsq(32, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_RSI),
			new TupleIntInstrInt(64, Instruction.createLodsb(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI),
			new TupleIntInstrInt(64, Instruction.createLodsw(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI),
			new TupleIntInstrInt(64, Instruction.createLodsd(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI),
			new TupleIntInstrInt(64, Instruction.createLodsq(64, ICRegister.NONE, RepPrefixKind.NONE), OpKind.MEMORY_SEG_SI),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(OpKind.FAR_BRANCH16);
				instr.setOp1Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind);
				instr.setOp1Kind(badOpKind);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidIns() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.createInsb(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createInsw(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createInsd(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createInsb(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createInsw(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createInsd(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(64, Instruction.createInsb(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createInsw(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createInsd(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidStos() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.createStosb(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createStosw(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createStosd(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createStosq(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createStosb(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createStosw(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createStosd(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createStosq(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(64, Instruction.createStosb(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createStosw(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createStosd(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createStosq(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidScas() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.createScasb(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createScasw(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createScasd(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(16, Instruction.createScasq(16, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createScasb(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createScasw(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createScasd(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(32, Instruction.createScasq(32, RepPrefixKind.NONE), OpKind.MEMORY_ESRDI),
			new TupleIntInstrInt(64, Instruction.createScasb(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createScasw(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createScasd(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
			new TupleIntInstrInt(64, Instruction.createScasq(64, RepPrefixKind.NONE), OpKind.MEMORY_ESDI),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int badOpKind = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(OpKind.FAR_BRANCH16);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Kind(badOpKind);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidXlatb() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.create(Code.XLAT_M8, new MemoryOperand(ICRegisters.bx, ICRegisters.al, 1, 0, 0, false, ICRegister.NONE)), Register.RBX),
			new TupleIntInstrInt(32, Instruction.create(Code.XLAT_M8, new MemoryOperand(ICRegisters.ebx, ICRegisters.al, 1, 0, 0, false, ICRegister.NONE)), Register.RBX),
			new TupleIntInstrInt(64, Instruction.create(Code.XLAT_M8, new MemoryOperand(ICRegisters.rbx, ICRegisters.al, 1, 0, 0, false, ICRegister.NONE)), Register.BX),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int invalidRbx = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				assertTrue(encoder.tryEncode(instr2, 0) instanceof Integer);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryBase(invalidRbx);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryBase(Register.ESI);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryIndex(Register.AX);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryIndex(Register.NONE);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			for (int scale : new int[] { 2, 4, 8 }) {
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryIndexScale(scale);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			int invalidDisplSize = bitness == 64 ? 4 : 8;
			for (TupleLongInt info2 : new TupleLongInt[] { new TupleLongInt(0, 1), new TupleLongInt(1, invalidDisplSize), new TupleLongInt(1, 1) }) {
				long displ = info2.value1;
				int displSize = info2.value2;
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryDisplacement64(displ);
				instr.setMemoryDisplSize(displSize);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidConstImmOp() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.ROL_RM8_1, ICRegisters.al, 0);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testInvalidIs5ImmOp() {
		for (int imm = 0; imm < 0x100; imm++) {
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.VEX_VPERMIL2PS_XMM_XMM_XMMM128_XMM_IMM4, ICRegisters.xmm0, ICRegisters.xmm1, ICRegisters.xmm2, ICRegisters.xmm3, imm);
			if (imm <= 0x0F)
				assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			else
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testEncodeInvalidInstr() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = new Instruction();
		assertEquals(Code.INVALID, instr.getCode());
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testHighR8RegWithRexPrefix() {
		for (ICRegister reg : new ICRegister[] { ICRegisters.ah, ICRegisters.ch, ICRegisters.dh, ICRegisters.bh }) {
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.MOVZX_R64_RM8, ICRegisters.rax, reg);
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testEvexInvalidK1() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.EVEX_VUCOMISS_XMM_XMMM32_SAE, ICRegisters.xmm0, ICRegisters.xmm1);
		instr.setOpMask(Register.K1);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void encodeWithoutRequiredOpMaskRegister() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.EVEX_VPGATHERDD_XMM_K1_VM32X, ICRegisters.xmm0, new MemoryOperand(ICRegisters.rax, ICRegisters.xmm1, 1, 0x10, 1, false, ICRegister.NONE));
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		instr.setOpMask(Register.K1);
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
	}

	@Test
	void encodeInvalidSae() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.EVEX_VMOVUPS_XMM_K1Z_XMMM128, ICRegisters.xmm0, ICRegisters.xmm1);
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		instr.setSuppressAllExceptions(true);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void encodeInvalidEr() {
		for (int er = 0; er < IcedConstants.ROUNDING_CONTROL_ENUM_COUNT; er++) {
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.EVEX_VMOVUPS_XMM_K1Z_XMMM128, ICRegisters.xmm0, ICRegisters.xmm1);
			instr.setRoundingControl(er);
			if (er == RoundingControl.NONE)
				assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			else
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void encodeInvalidBcst() {
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.EVEX_VMOVUPS_XMM_K1Z_XMMM128, ICRegisters.xmm0, new MemoryOperand(ICRegisters.rax));
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			instr.setBroadcast(true);
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm0, ICRegisters.xmm1, new MemoryOperand(ICRegisters.rax));
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			instr.setBroadcast(true);
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		}
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.EVEX_VUNPCKLPS_XMM_K1Z_XMM_XMMM128B32, ICRegisters.xmm0, ICRegisters.xmm1, ICRegisters.xmm2);
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			instr.setBroadcast(true);
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void encodeInvalidZmsk() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.EVEX_VMOVSS_M32_K1_XMM, new MemoryOperand(ICRegisters.rax), ICRegisters.xmm1);
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		instr.setZeroingMasking(true);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		instr.setOpMask(Register.K1);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void encodeInvalidAbsAddress() {
		TupleIntLongInt[] tests1 = new TupleIntLongInt[] {
			new TupleIntLongInt(16, 0x1234, 2),
			new TupleIntLongInt(16, 0x1234_5678, 4),
			new TupleIntLongInt(32, 0x1234, 2),
			new TupleIntLongInt(32, 0x1234_5678, 4),
			new TupleIntLongInt(64, 0x1234_5678, 4),
			new TupleIntLongInt(64, 0x1234_5678_9ABC_DEF0L, 8),
		};
		for (TupleIntLongInt info : tests1) {
			int bitness = info.value1;
			long address = info.value2;
			int displSize = info.value3;
			int memReg;
			switch (displSize) {
			case 2:
				memReg = Register.BX;
				break;
			case 4:
				memReg = Register.EBX;
				break;
			case 8:
				memReg = Register.RBX;
				break;
			default:
				throw new UnsupportedOperationException();
			}

			Instruction instr2 = Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(address, displSize));
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				assertTrue(encoder.tryEncode(instr2, 0) instanceof Integer);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryBase(memReg);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryIndex(memReg);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			for (int scale : new int[] { 2, 4, 8 }) {
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setMemoryIndexScale(scale);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp1Kind(OpKind.IMMEDIATE8);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}

		TupleIntLongInt[] tests2 = new TupleIntLongInt[] {
			new TupleIntLongInt(16, 0x1234, 8),
			new TupleIntLongInt(32, 0x1234, 8),
			new TupleIntLongInt(64, 0x1234, 2),
		};
		for (TupleIntLongInt info : tests2) {
			int bitness = info.value1;
			long address = info.value2;
			int displSize = info.value3;
			Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.MOV_EAX_MOFFS32, ICRegisters.eax, new MemoryOperand(address, displSize));
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testRegOpNotAllowed() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr1 = Instruction.create(Code.LEA_R32_M, ICRegisters.eax, new MemoryOperand(ICRegisters.rax));
		assertTrue(encoder.tryEncode(instr1, 0) instanceof Integer);
		Instruction instr2 = Instruction.create(Code.LEA_R32_M, ICRegisters.eax, ICRegisters.ecx);
		assertTrue(encoder.tryEncode(instr2, 0) instanceof String);
	}

	@Test
	void testMemOpNotAllowed() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr1 = Instruction.create(Code.MOVHLPS_XMM_XMM, ICRegisters.xmm0, ICRegisters.xmm1);
		assertTrue(encoder.tryEncode(instr1, 0) instanceof Integer);
		Instruction instr2 = Instruction.create(Code.MOVHLPS_XMM_XMM, ICRegisters.xmm0, new MemoryOperand(ICRegisters.rax));
		assertTrue(encoder.tryEncode(instr2, 0) instanceof String);
	}

	@Test
	void testRegmemOpIsWrongSize() {
		TupleIntInstrInt[] tests = new TupleIntInstrInt[] {
			new TupleIntInstrInt(16, Instruction.create(Code.ENQCMD_R16_M512, ICRegisters.ax, new MemoryOperand(ICRegisters.bx)), Register.EAX),
			new TupleIntInstrInt(16, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.eax, new MemoryOperand(ICRegisters.eax)), Register.AX),
			new TupleIntInstrInt(16, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.eax, new MemoryOperand(ICRegisters.eax)), Register.RAX),
			new TupleIntInstrInt(32, Instruction.create(Code.ENQCMD_R16_M512, ICRegisters.ax, new MemoryOperand(ICRegisters.bx)), Register.EAX),
			new TupleIntInstrInt(32, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.eax, new MemoryOperand(ICRegisters.eax)), Register.AX),
			new TupleIntInstrInt(32, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.eax, new MemoryOperand(ICRegisters.eax)), Register.RAX),
			new TupleIntInstrInt(64, Instruction.create(Code.ENQCMD_R32_M512, ICRegisters.eax, new MemoryOperand(ICRegisters.eax)), Register.RAX),
			new TupleIntInstrInt(64, Instruction.create(Code.ENQCMD_R64_M512, ICRegisters.rax, new MemoryOperand(ICRegisters.rax)), Register.EAX),
			new TupleIntInstrInt(64, Instruction.create(Code.ENQCMD_R64_M512, ICRegisters.rax, new MemoryOperand(ICRegisters.rax)), Register.AX),
		};
		for (TupleIntInstrInt info : tests) {
			int bitness = info.value1;
			Instruction instr2 = info.instr;
			int invalidReg = info.value2;
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				assertTrue(encoder.tryEncode(instr2, 0) instanceof Integer);
			}
			{
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = instr2.copy();
				instr.setOp0Register(invalidReg);
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testVsib16bitAddr() {
		for (int bitness : new int[] { 16, 32, 64 }) {
			Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.EVEX_VPGATHERDD_XMM_K1_VM32X, ICRegisters.xmm0, new MemoryOperand(ICRegisters.eax, ICRegisters.xmm1, 1, 0x10, 1, false, ICRegister.NONE));
			instr.setOpMask(Register.K1);
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			instr.setMemoryBase(Register.BX);
			instr.setMemoryIndex(Register.SI);
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testExpectedRegOrMemOpKind() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.ADD_RM8_IMM8, ICRegisters.al, 123);
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		instr.setOp0Kind(OpKind.IMMEDIATE8);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void test16bitAddrIn64bitMode() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.LEA_R32_M, ICRegisters.eax, new MemoryOperand(ICRegisters.bx));
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void test64bitAddrIn1632bitMode() {
		for (int bitness : new int[] { 16, 32 }) {
			Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.LEA_R32_M, ICRegisters.eax, new MemoryOperand(ICRegisters.rax));
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testInvalid16bitMemRegs() {
		TupleRegReg[] tests = new TupleRegReg[] {
			new TupleRegReg(ICRegisters.ax, ICRegister.NONE),
			new TupleRegReg(ICRegisters.r8w, ICRegister.NONE),
			new TupleRegReg(ICRegisters.bl, ICRegister.NONE),
			new TupleRegReg(ICRegister.NONE, ICRegisters.cx),
			new TupleRegReg(ICRegister.NONE, ICRegisters.r9w),
			new TupleRegReg(ICRegister.NONE, ICRegisters.sil),
			new TupleRegReg(ICRegisters.bx, ICRegisters.bp),
			new TupleRegReg(ICRegisters.bp, ICRegisters.bx),
		};
		for (TupleRegReg info : tests) {
			ICRegister base = info.reg1;
			ICRegister index = info.reg2;
			Encoder encoder = new Encoder(16, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(base, index));
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testInvalid16bitDisplSize() {
		Encoder encoder = new Encoder(16, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.bx, 1));
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		instr.setMemoryDisplSize(4);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		instr.setMemoryDisplSize(8);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testInvalid32bitDisplSize() {
		Encoder encoder = new Encoder(32, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.eax, 1));
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		instr.setMemoryDisplSize(2);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		instr.setMemoryDisplSize(8);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testInvalid64bitDisplSize() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rax, 1));
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		instr.setMemoryDisplSize(2);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		instr.setMemoryDisplSize(4);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testInvalidIpRelMemory() {
		for (TupleRegReg info : new TupleRegReg[] { new TupleRegReg(ICRegisters.eip, ICRegisters.edi), new TupleRegReg(ICRegisters.rip, ICRegisters.rdi) }) {
			ICRegister ipReg = info.reg1;
			ICRegister invalidIndex = info.reg2;
			{
				Encoder encoder = new Encoder(64, new CodeWriterImpl());
				Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ipReg, ICRegister.NONE, 1, 0, 8, false, ICRegister.NONE));
				assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			}
			for (int displSize : new int[] { 0, 1, 4, 8 }) {
				Encoder encoder = new Encoder(64, new CodeWriterImpl());
				Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ipReg, ICRegister.NONE, 1, 0, displSize, false, ICRegister.NONE));
				assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
			}
			{
				Encoder encoder = new Encoder(64, new CodeWriterImpl());
				Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ipReg, ICRegister.NONE, 1, 0, 2, false, ICRegister.NONE));
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			{
				Encoder encoder = new Encoder(64, new CodeWriterImpl());
				Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ipReg, invalidIndex, 1, 0, 8, false, ICRegister.NONE));
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
			for (int scale : new int[] { 2, 4, 8 }) {
				Encoder encoder = new Encoder(64, new CodeWriterImpl());
				Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ipReg, ICRegister.NONE, scale, 0, 8, false, ICRegister.NONE));
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidIpRelMemory1632() {
		for (int bitness : new int[] { 16, 32 }) {
			for (TupleRegInt info : new TupleRegInt[] { new TupleRegInt(ICRegisters.eip, 4), new TupleRegInt(ICRegisters.rip, 8) }) {
				ICRegister ipReg = info.reg;
				int displSize = info.value;
				Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
				Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ipReg, ICRegister.NONE, 1, 0, displSize, false, ICRegister.NONE));
				assertTrue(encoder.tryEncode(instr, 0) instanceof String);
			}
		}
	}

	@Test
	void testInvalidIpRelMemorySibRequired() {
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.VEX_TILELOADDT1_TMM_SIBMEM, ICRegisters.tmm1, new MemoryOperand(ICRegisters.rcx, ICRegisters.rdx, 1, 0x1234_5678, 8, false, ICRegister.NONE));
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		}
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.VEX_TILELOADDT1_TMM_SIBMEM, ICRegisters.tmm1, new MemoryOperand(ICRegisters.rip, ICRegister.NONE, 1, 0x1234_5678, 8, false, ICRegister.NONE));
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.VEX_TILELOADDT1_TMM_SIBMEM, ICRegisters.tmm1, new MemoryOperand(ICRegisters.ecx, ICRegisters.edx, 1, 0x1234_5678, 4, false, ICRegister.NONE));
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		}
		{
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.VEX_TILELOADDT1_TMM_SIBMEM, ICRegisters.tmm1, new MemoryOperand(ICRegisters.eip, ICRegister.NONE, 1, 0x1234_5678, 4, false, ICRegister.NONE));
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testInvalidEipRelMemTargetAddr() {
		for (long target : new long[] { 0, 0x7FFF_FFFF, 0xFFFF_FFFFL }) {
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.eip, ICRegister.NONE, 1, target, 4, false, ICRegister.NONE));
			assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		}
		for (long target : new long[] { 0x1_0000_0000L, 0xFFFF_FFFF_FFFF_FFFFL }) {
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.eip, ICRegister.NONE, 1, target, 4, false, ICRegister.NONE));
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testVsibWithOffsetOnlyMem() {
		Encoder encoder = new Encoder(64, new CodeWriterImpl());
		Instruction instr = Instruction.create(Code.EVEX_VPGATHERDD_XMM_K1_VM32X, ICRegisters.xmm0, new MemoryOperand(ICRegisters.rax, ICRegisters.xmm1, 1, 0x1234_5678, 8, false, ICRegister.NONE));
		instr.setOpMask(Register.K1);
		assertTrue(encoder.tryEncode(instr, 0) instanceof Integer);
		instr.setMemoryBase(Register.NONE);
		instr.setMemoryIndex(Register.NONE);
		assertTrue(encoder.tryEncode(instr, 0) instanceof String);
	}

	@Test
	void testInvalidEspRspIndexRegs() {
		for (ICRegister spReg : new ICRegister[] { ICRegisters.esp, ICRegisters.rsp }) {
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegister.NONE, spReg, 2));
			assertTrue(encoder.tryEncode(instr, 0) instanceof String);
		}
	}

	@Test
	void testRipRelDistTooFarAway() {
		final int instrLen = 6;
		final long instrAddr = 0x1234_5678_9ABC_DEF0L;
		for (long diff : new long[] { -0x8000_0000L, 0x7FFF_FFFFL, -1, 0, 1, -0x1234_5678L, 0x1234_5678L }) {
			CodeWriterImpl writer = new CodeWriterImpl();
			Encoder encoder = new Encoder(64, writer);
			long target = instrAddr + instrLen + diff;
			Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rip, ICRegister.NONE, 1, target, 8, false, ICRegister.NONE));
			Object result = encoder.tryEncode(instr, instrAddr);
			assertTrue(result instanceof Integer);
			assertEquals(instrLen, (Integer)result);

			byte[] bytes = writer.toArray();
			Instruction decoded = new Decoder(64, new ByteArrayCodeReader(bytes), instrAddr, DecoderOptions.NONE).decode();
			assertEquals(Code.NOT_RM8, decoded.getCode());
			assertEquals(Register.RIP, decoded.getMemoryBase());
			assertEquals(target, decoded.getMemoryDisplacement64());
		}
		for (long diff : new long[] { -0x8000_0001L, 0x8000_0000L, -0x1234_5678_9ABC_DEF0L, 0x1234_5678_9ABC_DEF0L, -0x8000_0000_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL }) {
			Encoder encoder = new Encoder(64, new CodeWriterImpl());
			long target = instrAddr + instrLen + diff;
			Instruction instr = Instruction.create(Code.NOT_RM8, new MemoryOperand(ICRegisters.rip, ICRegister.NONE, 1, target, 8, false, ICRegister.NONE));
			assertFalse(encoder.tryEncode(instr, instrAddr) instanceof Integer);
		}
	}

	@Test
	void testInvalidJccRel8_16() {
		long[] validDiffs = new long[] { (long)-0x80, (long)0x7F, -1, 0, 1, -0x12, 0x12 };
		long[] invalidDiffs = new long[] { (long)-0x81, (long)0x80, -0x1234, 0x1234, -0x8000, 0x7FFF };
		testInvalidJcc(16, Code.JE_REL8_16, 0x1234, 2, 0xFFFF, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidJccRel8_32() {
		long[] validDiffs = new long[] { (long)-0x80, (long)0x7F, -1, 0, 1, -0x12, 0x12 };
		long[] invalidDiffs = new long[] { (long)-0x81, (long)0x80, -0x1234_5678L, 0x1234_5678L, -0x8000_0000L, 0x7FFF_FFFFL };
		testInvalidJcc(32, Code.JE_REL8_32, 0x1234_5678, 2, 0xFFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidJccRel8_64() {
		long[] validDiffs = new long[] { (long)-0x80, (long)0x7F, -1, 0, 1, -0x12, 0x12 };
		long[] invalidDiffs = new long[] { (long)-0x81, (long)0x80, -0x1234_5678_9ABC_DEF0L, 0x1234_5678_9ABC_DEF0L, -0x8000_0000_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL };
		testInvalidJcc(64, Code.JE_REL8_64, 0x1234_5678_9ABC_DEF0L, 2, 0xFFFF_FFFF_FFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidJccRel16_16() {
		long[] validDiffs = new long[] { (long)-0x8000, (long)0x7FFF, -1, 0, 1, -0x1234, 0x1234 };
		long[] invalidDiffs = new long[] { };
		testInvalidJcc(16, Code.JE_REL16, 0x1234, 4, 0xFFFF, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidJccRel32_32() {
		long[] validDiffs = new long[] { -0x8000_0000L, 0x7FFF_FFFFL, -1, 0, 1, -0x1234_5678L, 0x1234_5678L };
		long[] invalidDiffs = new long[] { };
		testInvalidJcc(32, Code.JE_REL32_32, 0x1234_5678, 6, 0xFFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidJccRel32_64() {
		long[] validDiffs = new long[] { -0x8000_0000L, 0x7FFF_FFFFL, -1, 0, 1, -0x1234_5678L, 0x1234_5678L };
		long[] invalidDiffs = new long[] { -0x8000_0001L, 0x8000_0000L, -0x1234_5678_9ABC_DEF0L, 0x1234_5678_9ABC_DEF0L, -0x8000_0000_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL };
		testInvalidJcc(64, Code.JE_REL32_64, 0x1234_5678_9ABC_DEF0L, 6, 0xFFFF_FFFF_FFFF_FFFFL, validDiffs, invalidDiffs);
	}

	private static void testInvalidJcc(int bitness, int code, long instrAddr, int instrLen, long addrMask, long[] validDiffs, long[] invalidDiffs) {
		testInvalidBr(bitness, code, instrAddr, instrLen, addrMask, validDiffs, invalidDiffs, (code2, _ig, target) -> Instruction.createBranch(code2, target));
	}

	@Test
	void testInvalidXbeginRel16_16() {
		long[] validDiffs = new long[] { (long)-0x8000, (long)0x7FFF, -1, 0, 1, -0x1234, 0x1234 };
		long[] invalidDiffs = new long[] { (long)-0x8001, (long)0x8000, -0x1234_5678L, 0x1234_5678L, -0x8000_0000L, 0x7FFF_FFFFL };
		testInvalidXbegin(16, Code.XBEGIN_REL16, 0x1234, 4, 0xFFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidXbeginRel32_16() {
		long[] validDiffs = new long[] { -0x8000_0000L, 0x7FFF_FFFFL, -1, 0, 1, -0x1234_5678L, 0x1234_5678L };
		long[] invalidDiffs = new long[] { };
		testInvalidXbegin(16, Code.XBEGIN_REL32, 0x1234, 7, 0xFFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidXbeginRel16_32() {
		long[] validDiffs = new long[] { (long)-0x8000, (long)0x7FFF, -1, 0, 1, -0x1234, 0x1234 };
		long[] invalidDiffs = new long[] { (long)-0x8001, (long)0x8000, -0x1234_5678L, 0x1234_5678L, -0x8000_0000L, 0x7FFF_FFFFL };
		testInvalidXbegin(32, Code.XBEGIN_REL16, 0x1234_5678, 5, 0xFFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidXbeginRel32_32() {
		long[] validDiffs = new long[] { -0x8000_0000L, 0x7FFF_FFFFL, -1, 0, 1, -0x1234_5678L, 0x1234_5678L };
		long[] invalidDiffs = new long[] { };
		testInvalidXbegin(32, Code.XBEGIN_REL32, 0x1234_5678, 6, 0xFFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidXbeginRel16_64() {
		long[] validDiffs = new long[] { (long)-0x8000, (long)0x7FFF, -1, 0, 1, -0x1234, 0x1234 };
		long[] invalidDiffs = new long[] { (long)-0x8001, (long)0x8000, -0x1234_5678_9ABC_DEF0L, 0x1234_5678_9ABC_DEF0L, -0x8000_0000_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL };
		testInvalidXbegin(64, Code.XBEGIN_REL16, 0x1234_5678_9ABC_DEF0L, 5, 0xFFFF_FFFF_FFFF_FFFFL, validDiffs, invalidDiffs);
	}

	@Test
	void testInvalidXbeginRel32_64() {
		long[] validDiffs = new long[] { -0x8000_0000L, 0x7FFF_FFFFL, -1, 0, 1, -0x1234_5678L, 0x1234_5678L };
		long[] invalidDiffs = new long[] { -0x8000_0001L, 0x8000_0000L, -0x1234_5678_9ABC_DEF0L, 0x1234_5678_9ABC_DEF0L, -0x8000_0000_0000_0000L, 0x7FFF_FFFF_FFFF_FFFFL };
		testInvalidXbegin(64, Code.XBEGIN_REL32, 0x1234_5678_9ABC_DEF0L, 6, 0xFFFF_FFFF_FFFF_FFFFL, validDiffs, invalidDiffs);
	}

	private static void testInvalidXbegin(int bitness, int code, long instrAddr, int instrLen, long addrMask, long[] validDiffs, long[] invalidDiffs) {
		testInvalidBr(bitness, code, instrAddr, instrLen, addrMask, validDiffs, invalidDiffs, (code2, _ig, target) -> {
			Instruction instr = Instruction.createXbegin(bitness, target);
			instr.setCode(code2);
			return instr;
		});
	}

	@FunctionalInterface
	static interface FuncCodeIntLongRetInstr {
		Instruction create(int code, int bitness, long target);
	}

	private static void testInvalidBr(int bitness, int code, long instrAddr, int instrLen, long addrMask, long[] validDiffs, long[] invalidDiffs,
		FuncCodeIntLongRetInstr createInstr) {
		for (long diff : validDiffs) {
			CodeWriterImpl writer = new CodeWriterImpl();
			Encoder encoder = new Encoder(bitness, writer);
			long target = (instrAddr + instrLen + diff) & addrMask;
			Instruction instr = createInstr.create(code, bitness, target);
			Object result = encoder.tryEncode(instr, instrAddr);
			assertTrue(result instanceof Integer);
			assertEquals(instrLen, (Integer)result);

			byte[] bytes = writer.toArray();
			Instruction decoded = new Decoder(bitness, new ByteArrayCodeReader(bytes), instrAddr, DecoderOptions.NONE).decode();
			assertEquals(code, decoded.getCode());
			assertEquals(target, decoded.getNearBranch64());
		}
		for (long diff : invalidDiffs) {
			Encoder encoder = new Encoder(bitness, new CodeWriterImpl());
			long target = (instrAddr + instrLen + diff) & addrMask;
			Instruction instr = createInstr.create(code, bitness, target);
			assertTrue(encoder.tryEncode(instr, instrAddr) instanceof String);
		}
	}
}
