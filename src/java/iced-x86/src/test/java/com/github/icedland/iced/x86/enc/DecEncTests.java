// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashSet;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.TupleType;
import com.github.icedland.iced.x86.VsibFlags;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderError;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.dec.DecoderTestInfo;
import com.github.icedland.iced.x86.dec.DecoderTestOptions;
import com.github.icedland.iced.x86.dec.DecoderTestUtils;
import com.github.icedland.iced.x86.info.OpCodeInfo;
import com.github.icedland.iced.x86.info.OpCodeOperandKind;
import com.github.icedland.iced.x86.info.OpCodeTableKind;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class DecEncTests {
	@Test
	void verify_invalid_and_valid_lock_prefix() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;

			boolean hasLock;
			boolean canUseLock;

			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(info.hexBytes)), info.options);
				Instruction instruction = decoder.decode();
				assertEquals(info.code, instruction.getCode());
				hasLock = instruction.getLockPrefix();
				OpCodeInfo opCode = Code.toOpCode(info.code);
				canUseLock = opCode.canUseLockPrefix() && hasModRMMemoryOperand(instruction);
				if (opCode.getAmdLockRegBit())
					continue;
			}

			if (canUseLock) {
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(addLock(info.hexBytes, hasLock))), info.options);
				Instruction instruction = decoder.decode();
				assertEquals(info.code, instruction.getCode());
				assertTrue(instruction.getLockPrefix());
			}
			else {
				assert !hasLock;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(addLock(info.hexBytes, hasLock))), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(Code.INVALID, instruction.getCode());
					assertNotEquals(DecoderError.NONE, decoder.getLastError());
					assertFalse(instruction.getLockPrefix());
				}
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(addLock(info.hexBytes, hasLock))), info.options | DecoderOptions.NO_INVALID_CHECK);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(instruction.getLockPrefix());
				}
			}
		}
	}

	private static String addLock(String hexBytes, boolean hasLock) {
		return hasLock ? hexBytes : "F0" + hexBytes;
	}

	private static boolean hasModRMMemoryOperand(Instruction instruction) {
		int opCount = instruction.getOpCount();
		for (int i = 0; i < opCount; i++) {
			if (instruction.getOpKind(i) == OpKind.MEMORY)
				return true;
		}
		return false;
	}

	@Test
	void verify_invalid_REX_mandatory_prefixes_VEX_EVEX_XOP_MVEX() {
		String[] prefixes1632 = new String[] { "66", "F3", "F2" };
		String[] prefixes64   = new String[] { "66", "F3", "F2",
										  "40", "41", "42", "43", "44", "45", "46", "47",
										  "48", "49", "4A", "4B", "4C", "4D", "4E", "4F" };
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;

			switch (Code.toOpCode(info.code).getEncoding()) {
			case EncodingKind.LEGACY:
			case EncodingKind.D3NOW:
				continue;

			case EncodingKind.VEX:
			case EncodingKind.EVEX:
			case EncodingKind.XOP:
			case EncodingKind.MVEX:
				break;

			default:
				throw new UnsupportedOperationException();
			}

			String[] prefixes;
			switch (info.bitness) {
			case 16:
			case 32:
				prefixes = prefixes1632;
				break;
			case 64:
				prefixes = prefixes64;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			for (String prefix : prefixes) {
				Instruction origInstr;
				{
					byte[] bytes = HexUtils.toByteArray(info.hexBytes);
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					origInstr = decoder.decode();
					assertEquals(info.code, origInstr.getCode());
					// Mandatory prefix must be right before the opcode. If it has a seg override, there's also
					// a test without a seg override so just skip this.
					if (origInstr.getSegmentPrefix() != Register.NONE)
						continue;
					int memRegSize = getMemoryRegisterSize(origInstr);
					// 67h prefix
					if (memRegSize != 0 && memRegSize != info.bitness)
						continue;
					SkipPrefixesResult skipResult = skipPrefixes(bytes, info.bitness);
					int nonPrefixIndex = skipResult.index;
					boolean has67 = false;
					for (int i = 0; i < nonPrefixIndex; i++) {
						if (bytes[i] == 0x67) {
							has67 = true;
							break;
						}
					}
					if (has67)
						continue;
				}
				String hexBytes = prefix + info.hexBytes;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)), info.options | DecoderOptions.NO_INVALID_CHECK);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());

					instruction.setLength(instruction.getLength() - 1);
					instruction.setNextIP(instruction.getNextIP() - 1);
					if (prefix.equals("F3")) {
						assertTrue(instruction.getRepPrefix());
						assertTrue(instruction.getRepePrefix());
						instruction.setRepPrefix(false);
					}
					else if (prefix.equals("F2")) {
						assertTrue(instruction.getRepnePrefix());
						instruction.setRepnePrefix(false);
					}
					if (instruction.getOp1Kind() == OpKind.NEAR_BRANCH64)
						instruction.setNearBranch64(instruction.getNearBranch64() - 1);
					assertTrue(instruction.equalsAllBits(origInstr));
				}
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(Code.INVALID, instruction.getCode());
					assertNotEquals(DecoderError.NONE, decoder.getLastError());
				}
			}
		}
	}

	private static int getMemoryRegisterSize(Instruction instruction) {
		int opCount = instruction.getOpCount();
		for (int i = 0; i < opCount; i++) {
			switch (instruction.getOpKind(i)) {
			case OpKind.REGISTER:
			case OpKind.NEAR_BRANCH16:
			case OpKind.NEAR_BRANCH32:
			case OpKind.NEAR_BRANCH64:
			case OpKind.FAR_BRANCH16:
			case OpKind.FAR_BRANCH32:
			case OpKind.IMMEDIATE8:
			case OpKind.IMMEDIATE8_2ND:
			case OpKind.IMMEDIATE16:
			case OpKind.IMMEDIATE32:
			case OpKind.IMMEDIATE64:
			case OpKind.IMMEDIATE8TO16:
			case OpKind.IMMEDIATE8TO32:
			case OpKind.IMMEDIATE8TO64:
			case OpKind.IMMEDIATE32TO64:
				break;
			case OpKind.MEMORY_SEG_SI:
			case OpKind.MEMORY_SEG_DI:
			case OpKind.MEMORY_ESDI:
				return 16;
			case OpKind.MEMORY_SEG_ESI:
			case OpKind.MEMORY_SEG_EDI:
			case OpKind.MEMORY_ESEDI:
				return 32;
			case OpKind.MEMORY_SEG_RSI:
			case OpKind.MEMORY_SEG_RDI:
			case OpKind.MEMORY_ESRDI:
				return 64;
			case OpKind.MEMORY:
				int reg = instruction.getMemoryBase();
				if (reg == Register.NONE)
					reg = instruction.getMemoryIndex();
				if (reg != Register.NONE)
					return getSize(reg) * 8;
				if (instruction.getMemoryDisplSize() == 4)
					return 32;
				if (instruction.getMemoryDisplSize() == 8)
					return 64;
				break;
			default:
				throw new UnsupportedOperationException();
			}
		}
		return 0;
	}

	private static int getSize(int reg) {
		return Register.getSize(reg);
	}

	private static int getNumber(int reg) {
		return Register.getNumber(reg);
	}

	static class SkipPrefixesResult {
		public final int index;
		public final int rex;
		public SkipPrefixesResult(int index, int rex) {
			this.index = index;
			this.rex = rex;
		}
	}

	private static SkipPrefixesResult skipPrefixes(byte[] bytes, int bitness) {
		int rex = 0;
		for (int i = 0; i < bytes.length; i++) {
			int b = bytes[i] & 0xFF;
			switch (b) {
			case 0x26:
			case 0x2E:
			case 0x36:
			case 0x3E:
			case 0x64:
			case 0x65:
			case 0x66:
			case 0x67:
			case 0xF0:
			case 0xF2:
			case 0xF3:
				rex = 0;
				break;
			default:
				if (bitness == 64 && (b & 0xF0) == 0x40) {
					rex = b;
				}
				else
					return new SkipPrefixesResult(i, rex);
				break;
			}
		}
		throw new UnsupportedOperationException();
	}

	@Test
	void test_EVEX_reserved_bits() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if (Code.toOpCode(info.code).getEncoding() != EncodingKind.EVEX)
				continue;
			byte[] bytes = HexUtils.toByteArray(info.hexBytes);
			int evexIndex = getEvexIndex(bytes);
			for (int i = 1; i <= 1; i++) {
				bytes[evexIndex + 1] = (byte)((bytes[evexIndex + 1] & ~8) | (i << 3));
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(Code.INVALID, instruction.getCode());
					assertNotEquals(DecoderError.NONE, decoder.getLastError());
				}
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options ^ DecoderOptions.NO_INVALID_CHECK);
					Instruction instruction = decoder.decode();
					assertEquals(Code.INVALID, instruction.getCode());
					assertNotEquals(DecoderError.NONE, decoder.getLastError());
				}
			}
		}
	}

	private static int getEvexIndex(byte[] bytes) {
		for (int i = 0; ; i++) {
			if (i >= bytes.length)
				throw new UnsupportedOperationException();
			if (bytes[i] == 0x62)
				return i;
		}
	}

	private static int getVexXopIndex(byte[] bytes) {
		for (int i = 0; ; i++) {
			if (i >= bytes.length)
				throw new UnsupportedOperationException();
			int b = bytes[i] & 0xFF;
			if (b == 0xC4 || b == 0xC5 || b == 0x8F)
				return i;
		}
	}

	@Test
	void test_WIG_instructions_ignore_W() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			OpCodeInfo opCode = Code.toOpCode(info.code);
			int encoding = opCode.getEncoding();
			boolean isWIG = opCode.isWIG() || (opCode.isWIG32() && info.bitness != 64);
			if (encoding == EncodingKind.EVEX || encoding == EncodingKind.MVEX) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(bytes);

				if (isWIG) {
					Instruction instruction1, instruction2;
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction1 = decoder.decode();
						assertEquals(info.code, instruction1.getCode());
					}
					{
						bytes[evexIndex + 2] ^= 0x80;
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction2 = decoder.decode();
						assertEquals(info.code, instruction2.getCode());
					}
					assertTrue(instruction1.equalsAllBits(instruction2));
				}
				else {
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
					}
					{
						bytes[evexIndex + 2] ^= 0x80;
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertNotEquals(info.code, instruction.getCode());
					}
				}
			}
			else if (encoding == EncodingKind.VEX || encoding == EncodingKind.XOP) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(bytes);
				if ((bytes[vexIndex] & 0xFF) == 0xC5)
					continue;

				if (isWIG) {
					Instruction instruction1, instruction2;
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction1 = decoder.decode();
						assertEquals(info.code, instruction1.getCode());
					}
					{
						bytes[vexIndex + 2] ^= 0x80;
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction2 = decoder.decode();
						assertEquals(info.code, instruction2.getCode());
					}
					assertTrue(instruction1.equalsAllBits(instruction2));
				}
				else {
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
					}
					{
						bytes[vexIndex + 2] ^= 0x80;
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertNotEquals(info.code, instruction.getCode());
					}
				}
			}
			else if (encoding == EncodingKind.LEGACY || encoding == EncodingKind.D3NOW)
				continue;
			else
				throw new UnsupportedOperationException();
		}
	}

	@Test
	void test_LIG_instructions_ignore_L() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			OpCodeInfo opCode = Code.toOpCode(info.code);
			int encoding = opCode.getEncoding();
			if (encoding == EncodingKind.EVEX) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(bytes);

				boolean isRegOnly = ((bytes[evexIndex + 5] & 0xFF) >>> 6) == 3;
				boolean EVEX_b = (bytes[evexIndex + 3] & 0x10) != 0;
				if (opCode.canUseRoundingControl() && isRegOnly && EVEX_b)
					continue;
				boolean isSae = opCode.canSuppressAllExceptions() && isRegOnly && EVEX_b;

				if (opCode.isLIG()) {
					Instruction instruction1, instruction2;
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction1 = decoder.decode();
						assertEquals(info.code, instruction1.getCode());
					}
					byte origByte = bytes[evexIndex + 3];
					for (int i = 1; i <= 3; i++) {
						bytes[evexIndex + 3] = (byte)(origByte ^ (i << 5));
						int ll = (bytes[evexIndex + 3] >>> 5) & 3;
						boolean invalid = (info.options & DecoderOptions.NO_INVALID_CHECK) == 0 &&
							ll == 3 && ((bytes[evexIndex + 5] & 0xFF) < 0xC0 || (bytes[evexIndex + 3] & 0x10) == 0);
						if (invalid) {
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							instruction2 = decoder.decode();
							assertEquals(Code.INVALID, instruction2.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());

							decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
							instruction2 = decoder.decode();
							assertEquals(info.code, instruction2.getCode());
							assertTrue(instruction1.equalsAllBits(instruction2));
						}
						else {
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							instruction2 = decoder.decode();
							assertEquals(info.code, instruction2.getCode());
							assertTrue(instruction1.equalsAllBits(instruction2));
						}
					}
				}
				else {
					Instruction instruction1, instruction2;
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction1 = decoder.decode();
						assertEquals(info.code, instruction1.getCode());
					}
					byte origByte = bytes[evexIndex + 3];
					for (int i = 1; i <= 3; i++) {
						bytes[evexIndex + 3] = (byte)(origByte ^ (i << 5));
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction2 = decoder.decode();
						if (isSae) {
							assertEquals(info.code, instruction2.getCode());
							assertTrue(instruction1.equalsAllBits(instruction2));
						}
						else
							assertNotEquals(info.code, instruction2.getCode());
					}
				}
			}
			else if (encoding == EncodingKind.VEX || encoding == EncodingKind.XOP) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(bytes);
				int lIndex = (bytes[vexIndex] & 0xFF) == 0xC5 ? vexIndex + 1 : vexIndex + 2;

				if (opCode.isLIG()) {
					Instruction instruction1, instruction2;
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction1 = decoder.decode();
						assertEquals(info.code, instruction1.getCode());
					}
					{
						bytes[lIndex] ^= 4;
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						instruction2 = decoder.decode();
						assertEquals(info.code, instruction2.getCode());
					}
					assertTrue(instruction1.equalsAllBits(instruction2));
				}
				else {
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
					}
					{
						bytes[lIndex] ^= 4;
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertNotEquals(info.code, instruction.getCode());
					}
				}
			}
			else if (encoding == EncodingKind.LEGACY || encoding == EncodingKind.D3NOW)
				continue;
			else if (encoding == EncodingKind.MVEX)
				continue;
			else
				throw new UnsupportedOperationException();
		}
	}

	private static boolean hasIs4OrIs5Operands(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.XMM_IS4:
			case OpCodeOperandKind.XMM_IS5:
			case OpCodeOperandKind.YMM_IS4:
			case OpCodeOperandKind.YMM_IS5:
				return true;
			default:
				break;
			}
		}
		return false;
	}

	@Test
	void test_is4_is5_instructions_ignore_bit7_in_1632mode() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if (info.bitness != 16 && info.bitness != 32)
				continue;
			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (!hasIs4OrIs5Operands(opCode))
				continue;
			byte[] bytes = HexUtils.toByteArray(info.hexBytes);
			Instruction instruction1, instruction2;
			{
				Decoder decoder = new Decoder(info.bitness, bytes, info.options);
				instruction1 = decoder.decode();
			}
			bytes[bytes.length - 1] ^= 0x80;
			{
				Decoder decoder = new Decoder(info.bitness, bytes, info.options);
				instruction2 = decoder.decode();
			}
			assertEquals(info.code, instruction1.getCode());
			assertTrue(instruction1.equalsAllBits(instruction2));
		}
	}

	static final class TupleBoolByte {
		public final boolean cond;
		public final byte b;
		public TupleBoolByte(boolean cond, byte b) {
			this.cond = cond;
			this.b = b;
		}
	}

	@Test
	void test_EVEX_k1_z_bits() {
		TupleBoolByte[] p2Values_k1z = new TupleBoolByte[] { new TupleBoolByte(true, (byte)0x00), new TupleBoolByte(true, (byte)0x01), new TupleBoolByte(false, (byte)0x80), new TupleBoolByte(true, (byte)0x86) };
		TupleBoolByte[] p2Values_k1 = new TupleBoolByte[] { new TupleBoolByte(true, (byte)0x00), new TupleBoolByte(true, (byte)0x01), new TupleBoolByte(false, (byte)0x80), new TupleBoolByte(false, (byte)0x86) };
		TupleBoolByte[] p2Values_k1_fk = new TupleBoolByte[] { new TupleBoolByte(false, (byte)0x00), new TupleBoolByte(true, (byte)0x01), new TupleBoolByte(false, (byte)0x80), new TupleBoolByte(false, (byte)0x86) };
		TupleBoolByte[] p2Values_nothing = new TupleBoolByte[] { new TupleBoolByte(true, (byte)0x00), new TupleBoolByte(false, (byte)0x01), new TupleBoolByte(false, (byte)0x80), new TupleBoolByte(false, (byte)0x86) };
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;

			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (opCode.getEncoding() != EncodingKind.EVEX)
				continue;
			byte[] bytes = HexUtils.toByteArray(info.hexBytes);
			int evexIndex = getEvexIndex(bytes);
			TupleBoolByte[] p2Values;
			if (opCode.canUseZeroingMasking()) {
				assertTrue(opCode.canUseOpMaskRegister());
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
				Instruction instruction = decoder.decode();
				assert instruction.getCode() != Code.INVALID : instruction.getCode();
				if (instruction.getOp0Kind() == OpKind.MEMORY)
					p2Values = p2Values_k1;
				else
					p2Values = p2Values_k1z;
			}
			else if (opCode.canUseOpMaskRegister()) {
				if (opCode.getRequireOpMaskRegister())
					p2Values = p2Values_k1_fk;
				else
					p2Values = p2Values_k1;
			}
			else
				p2Values = p2Values_nothing;

			byte b = bytes[evexIndex + 3];
			for (TupleBoolByte p2v : p2Values) {
				for (int i = 0; i < 2; i++) {
					bytes[evexIndex + 3] = (byte)((b & ~0x87) | p2v.b);
					int options = info.options;
					if (i == 1)
						options |= DecoderOptions.NO_INVALID_CHECK;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), options);
					Instruction instruction = decoder.decode();
					if (p2v.cond || (options & DecoderOptions.NO_INVALID_CHECK) != 0) {
						assertEquals(info.code, instruction.getCode());
						assertEquals((p2v.b & 0x80) != 0, instruction.getZeroingMasking());
						if ((p2v.b & 7) != 0)
							assertEquals(Register.K0 + (p2v.b & 7), instruction.getOpMask());
						else
							assertEquals(Register.NONE, instruction.getOpMask());
					}
					else {
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
				}
			}
		}
	}

	@Test
	void test_EVEX_b_bit() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;

			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (opCode.getEncoding() != EncodingKind.EVEX)
				continue;
			byte[] bytes = HexUtils.toByteArray(info.hexBytes);
			int evexIndex = getEvexIndex(bytes);

			boolean isRegOnly = ((bytes[evexIndex + 5] & 0xFF) >>> 6) == 3;
			boolean isSaeOrEr = isRegOnly && (opCode.canUseRoundingControl() || opCode.canSuppressAllExceptions());
			Integer newCodeResult = tryGetSaeErInstruction(opCode);

			if (opCode.canBroadcast() && !isRegOnly) {
				{
					bytes[evexIndex + 3] &= 0xEF;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertFalse(instruction.getBroadcast());
				}
				{
					bytes[evexIndex + 3] |= 0x10;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(instruction.getBroadcast());
				}
			}
			else {
				if (!isSaeOrEr) {
					bytes[evexIndex + 3] &= 0xEF;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertFalse(instruction.getBroadcast());
				}
				{
					bytes[evexIndex + 3] |= 0x10;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					if (isSaeOrEr)
						assertEquals(info.code, instruction.getCode());
					else if (newCodeResult != null && isRegOnly)
						assertEquals(newCodeResult, instruction.getCode());
					else {
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
					assertFalse(instruction.getBroadcast());
				}
				{
					bytes[evexIndex + 3] |= 0x10;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
					Instruction instruction = decoder.decode();
					if (newCodeResult != null && isRegOnly)
						assertEquals(newCodeResult, instruction.getCode());
					else
						assertEquals(info.code, instruction.getCode());
					assertFalse(instruction.getBroadcast());
				}
			}
		}
	}

	private static Integer tryGetSaeErInstruction(OpCodeInfo opCode) {
		if (opCode.getEncoding() == EncodingKind.EVEX && !(opCode.canSuppressAllExceptions() || opCode.canUseRoundingControl())) {
			int mnemonic = opCode.getMnemonic();
			for (int i = opCode.getCode() + 1, j = 1; i < IcedConstants.CODE_ENUM_COUNT && j <= 2; i++, j++) {
				int nextCode = i;
				if (Code.mnemonic(nextCode) != mnemonic)
					break;
				OpCodeInfo nextOpCode = Code.toOpCode(nextCode);
				if (nextOpCode.getEncoding() != opCode.getEncoding())
					break;
				if (nextOpCode.canSuppressAllExceptions() || nextOpCode.canUseRoundingControl())
					return nextCode;
			}
		}
		return null;
	}

	@Test
	void verify_tuple_type_bcst() {
		String[] codeNames = ToCode.names();
		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			if (CodeUtils.isIgnored(codeNames[i]))
				continue;
			OpCodeInfo opCode = Code.toOpCode(i);
			boolean expectedBcst;
			switch (opCode.getTupleType()) {
			case TupleType.N8B4:
			case TupleType.N16B4:
			case TupleType.N32B4:
			case TupleType.N64B4:
			case TupleType.N16B8:
			case TupleType.N32B8:
			case TupleType.N64B8:
			case TupleType.N4B2:
			case TupleType.N8B2:
			case TupleType.N16B2:
			case TupleType.N32B2:
			case TupleType.N64B2:
				expectedBcst = true;
				break;
			default:
				expectedBcst = false;
				break;
			}
			assertEquals(expectedBcst, opCode.canBroadcast());
		}
	}

	@Test
	void verify_invalid_vvvv() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;

			OpCodeInfo opCode = Code.toOpCode(info.code);

			switch (opCode.getEncoding()) {
			case EncodingKind.LEGACY:
			case EncodingKind.D3NOW:
				continue;

			case EncodingKind.VEX:
			case EncodingKind.EVEX:
			case EncodingKind.XOP:
			case EncodingKind.MVEX:
				break;

			default:
				throw new UnsupportedOperationException();
			}

			VvvvvInfoResult vvvvInfo = get_Vvvvv_info(opCode);

			if (opCode.getEncoding() == EncodingKind.VEX || opCode.getEncoding() == EncodingKind.XOP) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(bytes);
				int b2i = vexIndex + 1;
				if ((bytes[vexIndex] & 0xFF) != 0xC5)
					b2i++;
				byte b2 = bytes[b2i];
				Instruction origInstr;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					origInstr = decoder.decode();
					assertEquals(info.code, origInstr.getCode());
				}
				boolean isVEX2 = (bytes[vexIndex] & 0xFF) == 0xC5;
				int b2_mask = info.bitness == 64 || !isVEX2 ? 0x78 : 0x38;
				if (vvvvInfo.uses_vvvv) {
					bytes[b2i] = (byte)((b2 & ~b2_mask) | (b2_mask & ~(vvvvInfo.vvvv_mask << 3)));
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
					}
					if (info.bitness != 64 && !isVEX2) {
						// vvvv[3] is ignored 16/32-bit modes, clear it (it's inverted, so 'set' it)
						bytes[b2i] = (byte)(b2 & ~0x40);
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						assertTrue(origInstr.equalsAllBits(instruction));
					}
					if (info.bitness == 64 && vvvvInfo.vvvv_mask != 0xF) {
						bytes[b2i] = (byte)(b2 & ~b2_mask);
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
						}
					}
				}
				else {
					bytes[b2i] = (byte)(b2 & ~b2_mask);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						assertTrue(origInstr.equalsAllBits(instruction));
					}
				}
			}
			else if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(bytes);
				byte b2 = bytes[evexIndex + 2];
				byte b3 = bytes[evexIndex + 3];
				Instruction origInstr;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					origInstr = decoder.decode();
					assertEquals(info.code, origInstr.getCode());
				}

				bytes[evexIndex + 2] = (byte)(b2 & 0x87);
				if (!vvvvInfo.isVsib) {
					bytes[evexIndex + 3] = (byte)(b3 & 0xF7);
				}
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					if (info.bitness != 64) {
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
					else if (vvvvInfo.uses_vvvv) {
						if (vvvvInfo.vvvv_mask != 0x1F)
							assertEquals(Code.INVALID, instruction.getCode());
						else
							assertEquals(info.code, instruction.getCode());
					}
					else {
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
						decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
						instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
					}
				}
				if (!vvvvInfo.uses_vvvv && info.bitness == 64) {
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}

				// vvvv[3] isn't ignored 16/32-bit mode if the operand doesn't use the vvvv bits
				bytes[evexIndex + 2] = (byte)(b2 & 0xBF);
				bytes[evexIndex + 3] = b3;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					if (vvvvInfo.uses_vvvv) {
						if (vvvvInfo.vvvv_mask != 0x1F)
							assertEquals(Code.INVALID, instruction.getCode());
						else
							assertEquals(info.code, instruction.getCode());
					}
					else {
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
						decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
						instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
					}
				}
				if (!vvvvInfo.uses_vvvv) {
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}

				// V' must be 1 16/32-bit modes
				bytes[evexIndex + 2] = b2;
				bytes[evexIndex + 3] = (byte)(b3 & 0xF7);
				if (info.bitness != 64) {
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(Code.INVALID, instruction.getCode());
					assertNotEquals(DecoderError.NONE, decoder.getLastError());
				}
			}
			else
				throw new UnsupportedOperationException();
		}
	}

	static final class VvvvvInfoResult {
		public boolean uses_vvvv;
		public boolean isVsib;
		public int vvvv_mask;

		public VvvvvInfoResult(boolean uses_vvvv, boolean isVsib, int vvvv_mask) {
			this.uses_vvvv = uses_vvvv;
			this.isVsib = isVsib;
			this.vvvv_mask = vvvv_mask;
		}
	}

	private static VvvvvInfoResult get_Vvvvv_info(OpCodeInfo opCode) {
		boolean uses_vvvv = false;
		boolean isVsib = false;
		int vvvv_mask;
		switch (opCode.getEncoding()) {
		case EncodingKind.EVEX:
		case EncodingKind.MVEX:
			vvvv_mask = 0x1F;
			break;
		case EncodingKind.VEX:
		case EncodingKind.XOP:
			vvvv_mask = 0xF;
			break;
		case EncodingKind.LEGACY:
		case EncodingKind.D3NOW:
		default:
			throw new UnsupportedOperationException();
		}
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64Z:
				isVsib = true;
				break;
			case OpCodeOperandKind.K_VVVV:
			case OpCodeOperandKind.TMM_VVVV:
				uses_vvvv = true;
				vvvv_mask = 0x7;
				break;
			case OpCodeOperandKind.R32_VVVV:
			case OpCodeOperandKind.R64_VVVV:
			case OpCodeOperandKind.XMM_VVVV:
			case OpCodeOperandKind.XMMP3_VVVV:
			case OpCodeOperandKind.YMM_VVVV:
			case OpCodeOperandKind.ZMM_VVVV:
			case OpCodeOperandKind.ZMMP3_VVVV:
				uses_vvvv = true;
				break;
			}
		}
		return new VvvvvInfoResult(uses_vvvv, isVsib, vvvv_mask);
	}

	@Test
	void verify_GPR_RRXB_bits() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;

			OpCodeInfo opCode = Code.toOpCode(info.code);

			switch (opCode.getEncoding()) {
			case EncodingKind.LEGACY:
			case EncodingKind.D3NOW:
				continue;

			case EncodingKind.VEX:
			case EncodingKind.EVEX:
			case EncodingKind.XOP:
			case EncodingKind.MVEX:
				break;

			default:
				throw new UnsupportedOperationException();
			}

			boolean uses_rm = false;
			boolean uses_reg = false;
			boolean other_rm = false;
			boolean other_reg = false;
			boolean mem_only = false;
			for (int i = 0; i < opCode.getOpCount(); i++) {
				switch (opCode.getOpKind(i)) {
				case OpCodeOperandKind.MEM:
				case OpCodeOperandKind.MEM_MPX:
				case OpCodeOperandKind.MEM_MIB:
				case OpCodeOperandKind.MEM_VSIB32X:
				case OpCodeOperandKind.MEM_VSIB64X:
				case OpCodeOperandKind.MEM_VSIB32Y:
				case OpCodeOperandKind.MEM_VSIB64Y:
				case OpCodeOperandKind.MEM_VSIB32Z:
				case OpCodeOperandKind.MEM_VSIB64Z:
				case OpCodeOperandKind.SIBMEM:
					mem_only = true;
					break;
				case OpCodeOperandKind.R32_OR_MEM:
				case OpCodeOperandKind.R64_OR_MEM:
				case OpCodeOperandKind.R32_OR_MEM_MPX:
				case OpCodeOperandKind.R64_OR_MEM_MPX:
				case OpCodeOperandKind.R32_RM:
				case OpCodeOperandKind.R64_RM:
					uses_rm = true;
					break;
				case OpCodeOperandKind.R32_REG:
				case OpCodeOperandKind.R64_REG:
					uses_reg = true;
					break;
				case OpCodeOperandKind.K_OR_MEM:
				case OpCodeOperandKind.K_RM:
				case OpCodeOperandKind.XMM_OR_MEM:
				case OpCodeOperandKind.YMM_OR_MEM:
				case OpCodeOperandKind.ZMM_OR_MEM:
				case OpCodeOperandKind.XMM_RM:
				case OpCodeOperandKind.YMM_RM:
				case OpCodeOperandKind.ZMM_RM:
				case OpCodeOperandKind.TMM_RM:
					other_rm = true;
					break;
				case OpCodeOperandKind.K_REG:
				case OpCodeOperandKind.KP1_REG:
				case OpCodeOperandKind.XMM_REG:
				case OpCodeOperandKind.YMM_REG:
				case OpCodeOperandKind.ZMM_REG:
				case OpCodeOperandKind.TMM_REG:
					other_reg = true;
					break;
				}
			}
			if (mem_only) {
				if (uses_reg)
					uses_rm = true;
				if (other_reg)
					other_rm = true;
			}
			if (!uses_rm && !uses_reg && opCode.getOpCount() > 0)
				continue;

			if (opCode.getEncoding() == EncodingKind.VEX || opCode.getEncoding() == EncodingKind.XOP) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(bytes);
				boolean isVEX2 = (bytes[vexIndex] & 0xFF) == 0xC5;
				int mrmi = vexIndex + 3 + (isVEX2 ? 0 : 1);
				boolean isRegOnly = mrmi >= bytes.length || ((bytes[mrmi] & 0xFF) >>> 6) == 3;
				byte b1 = bytes[vexIndex + 1];

				Instruction origInstr;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					origInstr = decoder.decode();
					assertEquals(info.code, origInstr.getCode());
				}
				if (uses_rm && !isVEX2) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x20);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly && info.bitness != 64)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x40);
					if (info.bitness == 64) {
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
				}
				else if (!other_rm && !isVEX2) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x60);
					if (info.bitness != 64)
						bytes[vexIndex + 1] |= 0x40;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}
				if (uses_reg) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
					if (info.bitness == 64) {
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly)
							assertFalse(origInstr.equalsAllBits(instruction));
					}
				}
				else if (!other_reg) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
					if (info.bitness != 64)
						bytes[vexIndex + 1] |= 0x80;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}
			}
			else if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(bytes);
				boolean isRegOnly = ((bytes[evexIndex + 5] & 0xFF) >>> 6) == 3;
				byte b1 = bytes[evexIndex + 1];

				Instruction origInstr;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					origInstr = decoder.decode();
					assertEquals(info.code, origInstr.getCode());
				}
				if (uses_rm) {
					bytes[evexIndex + 1] = (byte)(b1 ^ 0x20);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly && info.bitness != 64)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
					bytes[evexIndex + 1] = (byte)(b1 ^ 0x40);
					if (info.bitness == 64) {
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
				}
				else if (!other_rm) {
					bytes[evexIndex + 1] = (byte)(b1 ^ 0x60);
					if (info.bitness != 64)
						bytes[evexIndex + 1] |= 0x40;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}
				if (uses_reg) {
					if (info.bitness == 64) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
							assertTrue(origInstr.equalsAllBits(instruction));
						}
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x80);
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
						}
					}
					else {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
							assertTrue(origInstr.equalsAllBits(instruction));
						}
					}
				}
			}
			else
				throw new UnsupportedOperationException();
		}
	}

	@Test
	void verify_K_reg_RRXB_bits() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;

			OpCodeInfo opCode = Code.toOpCode(info.code);

			switch (opCode.getEncoding()) {
			case EncodingKind.LEGACY:
			case EncodingKind.D3NOW:
				continue;

			case EncodingKind.VEX:
			case EncodingKind.EVEX:
			case EncodingKind.XOP:
			case EncodingKind.MVEX:
				break;

			default:
				throw new UnsupportedOperationException();
			}

			boolean uses_rm = false;
			boolean maybe_uses_rm = false;
			boolean uses_reg = false;
			boolean other_rm = false;
			boolean other_reg = false;
			for (int i = 0; i < opCode.getOpCount(); i++) {
				switch (opCode.getOpKind(i)) {
				case OpCodeOperandKind.MEM:
					maybe_uses_rm = true;
					break;
				case OpCodeOperandKind.K_OR_MEM:
				case OpCodeOperandKind.K_RM:
					uses_rm = true;
					break;
				case OpCodeOperandKind.K_REG:
				case OpCodeOperandKind.KP1_REG:
					uses_reg = true;
					break;

				case OpCodeOperandKind.R32_OR_MEM:
				case OpCodeOperandKind.R64_OR_MEM:
				case OpCodeOperandKind.R32_OR_MEM_MPX:
				case OpCodeOperandKind.R64_OR_MEM_MPX:
				case OpCodeOperandKind.R32_RM:
				case OpCodeOperandKind.R64_RM:
				case OpCodeOperandKind.XMM_OR_MEM:
				case OpCodeOperandKind.YMM_OR_MEM:
				case OpCodeOperandKind.ZMM_OR_MEM:
				case OpCodeOperandKind.XMM_RM:
				case OpCodeOperandKind.YMM_RM:
				case OpCodeOperandKind.ZMM_RM:
				case OpCodeOperandKind.TMM_RM:
					other_rm = true;
					break;
				case OpCodeOperandKind.XMM_REG:
				case OpCodeOperandKind.YMM_REG:
				case OpCodeOperandKind.ZMM_REG:
				case OpCodeOperandKind.TMM_REG:
				case OpCodeOperandKind.R32_REG:
				case OpCodeOperandKind.R64_REG:
					other_reg = true;
					break;
				}
			}
			if (uses_reg && maybe_uses_rm)
				uses_rm = true;
			if (!uses_rm && !uses_reg && opCode.getOpCount() > 0)
				continue;

			if (opCode.getEncoding() == EncodingKind.VEX || opCode.getEncoding() == EncodingKind.XOP) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(bytes);
				boolean isVEX2 = (bytes[vexIndex] & 0xFF) == 0xC5;
				int mrmi = vexIndex + 3 + (isVEX2 ? 0 : 1);
				boolean isRegOnly = mrmi >= bytes.length || ((bytes[mrmi] & 0xFF) >>> 6) == 3;
				byte b1 = bytes[vexIndex + 1];

				Instruction origInstr;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					origInstr = decoder.decode();
					assertEquals(info.code, origInstr.getCode());
				}
				if (uses_rm && !isVEX2) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x20);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly && info.bitness != 64)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x40);
					if (info.bitness == 64) {
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
				}
				else if (!other_rm && !isVEX2) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x60);
					if (info.bitness != 64)
						bytes[vexIndex + 1] |= 0x40;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}
				if (uses_reg) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
					if (info.bitness == 64) {
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
							if (isRegOnly)
								assertTrue(origInstr.equalsAllBits(instruction));
						}
					}
				}
				else if (!other_reg) {
					bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
					if (info.bitness != 64)
						bytes[vexIndex + 1] |= 0x80;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}
			}
			else if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(bytes);
				boolean isRegOnly = ((bytes[evexIndex + 5] & 0xFF) >>> 6) == 3;
				byte b1 = bytes[evexIndex + 1];

				Instruction origInstr;
				{
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					origInstr = decoder.decode();
					assertEquals(info.code, origInstr.getCode());
				}
				if (uses_rm) {
					bytes[evexIndex + 1] = (byte)(b1 ^ 0x20);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly && info.bitness != 64)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
					bytes[evexIndex + 1] = (byte)(b1 ^ 0x40);
					if (info.bitness == 64) {
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(info.code, instruction.getCode());
						if (isRegOnly)
							assertTrue(origInstr.equalsAllBits(instruction));
					}
				}
				else if (!other_rm) {
					bytes[evexIndex + 1] = (byte)(b1 ^ 0x60);
					if (info.bitness != 64)
						bytes[evexIndex + 1] |= 0x40;
					Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
					Instruction instruction = decoder.decode();
					assertEquals(info.code, instruction.getCode());
					assertTrue(origInstr.equalsAllBits(instruction));
				}
				if (uses_reg) {
					if (info.bitness == 64) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
							assertTrue(origInstr.equalsAllBits(instruction));
						}
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x80);
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
							assertTrue(origInstr.equalsAllBits(instruction));
						}
					}
					else {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
							assertTrue(origInstr.equalsAllBits(instruction));
						}
					}
				}
			}
			else
				throw new UnsupportedOperationException();
		}
	}

	@Test
	void verify_vsib_with_invalid_index_register_EVEX() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;
			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (!canHaveInvalidIndexRegister_EVEX(opCode))
				continue;

			if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(bytes);
				byte p0 = bytes[evexIndex + 1];
				byte p2 = bytes[evexIndex + 3];
				byte m = bytes[evexIndex + 5];
				byte s = bytes[evexIndex + 6];
				for (int i = 0; i < 32; i++) {
					int regNum = info.bitness == 64 ? i : i & 7;
					boolean alwaysInvalid = info.bitness != 64 && (i & 0x10) != 0;
					int t = i ^ 0x1F;
					// reg  = R' R modrm.reg
					// vidx = V' X sib.index
					bytes[evexIndex + 1] = (byte)((p0 & ~0xD0) | /*R'*/(t & 0x10) | /*R*/((t & 0x08) << 4) | /*X*/((t & 0x08) << 3));
					if (info.bitness != 64)
						bytes[evexIndex + 1] |= 0xC0;
					bytes[evexIndex + 3] = (byte)((p2 & ~0x08) | /*V'*/((t & 0x10) >>> 1));
					bytes[evexIndex + 5] = (byte)((m & 0xC7) | /*modrm.reg*/((i & 7) << 3));
					bytes[evexIndex + 6] = (byte)((s & 0xC7) | /*sib.index*/((i & 7) << 3));

					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
						Instruction instruction = decoder.decode();
						if (alwaysInvalid) {
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
						else {
							assertEquals(info.code, instruction.getCode());
							assertEquals(OpKind.REGISTER, instruction.getOp0Kind());
							assertEquals(OpKind.MEMORY, instruction.getOp1Kind());
							assertNotEquals(Register.NONE, instruction.getMemoryIndex());
							assertEquals(regNum, getNumber(instruction.getOp0Register()));
							assertEquals(regNum, getNumber(instruction.getMemoryIndex()));
						}
					}
				}
			}
			else
				throw new UnsupportedOperationException();
		}
	}

	// All Vk_VSIB instructions, eg. EVEX_Vpgatherdd_xmm_k1_vm32x
	private static boolean canHaveInvalidIndexRegister_EVEX(OpCodeInfo opCode) {
		if (opCode.getEncoding() != EncodingKind.EVEX && opCode.getEncoding() != EncodingKind.MVEX)
			return false;

		switch (opCode.getOp0Kind()) {
		case OpCodeOperandKind.XMM_REG:
		case OpCodeOperandKind.YMM_REG:
		case OpCodeOperandKind.ZMM_REG:
			break;
		default:
			return false;
		}
		return opCode.getRequiresUniqueRegNums();
	}

	@Test
	void verify_vsib_with_invalid_index_mask_dest_register_VEX() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;
			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (!canHaveInvalidIndexMaskDestRegister_VEX(opCode))
				continue;

			if (opCode.getEncoding() == EncodingKind.VEX || opCode.getEncoding() == EncodingKind.XOP) {
				byte[] bytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(bytes);

				boolean isVEX2 = (bytes[vexIndex] & 0xFF) == 0xC5;
				int rIndex = vexIndex + 1;
				int vIndex = isVEX2 ? rIndex : rIndex + 1;
				int mIndex = vIndex + 2;
				int sIndex = vIndex + 3;

				byte r = bytes[rIndex];
				byte v = bytes[vIndex];
				byte m = bytes[mIndex];
				byte s = bytes[sIndex];

				final int reg_eq_vvvv = 0;
				final int reg_eq_vidx = 1;
				final int vvvv_eq_vidx = 2;
				final int all_eq_all = 3;
				for (int testKind : new int[] { reg_eq_vvvv, reg_eq_vidx, vvvv_eq_vidx, all_eq_all }) {
					for (int i = 0; i < 16; i++) {
						int regNum = info.bitness == 64 ? i : i & 7;
						// Use a small number (0-7) case it's VEX2 and 'other' is vidx (uses VEX.X bit)
						int other = regNum == 0 ? 1 : 0;
						int newReg, newVvvv, newVidx;

						switch (testKind) {
						case reg_eq_vvvv:
							newReg = newVvvv = regNum;
							newVidx = other;
							break;
						case reg_eq_vidx:
							newReg = newVidx = regNum;
							newVvvv = other;
							break;
						case vvvv_eq_vidx:
							newVvvv = newVidx = regNum;
							newReg = other;
							break;
						case all_eq_all:
							newReg = newVvvv = newVidx = regNum;
							break;
						default:
							throw new UnsupportedOperationException();
						}

						// reg  = R modrm.reg
						// vidx = X sib.index
						if (isVEX2) {
							if (newVidx >= 8)
								continue;
							bytes[rIndex] = (byte)((r & 0x07) | /*R*/(((newReg ^ 8) & 0x8) << 4) | /*vvvv*/((newVvvv ^ 0xF) << 3));
						}
						else {
							bytes[rIndex] = (byte)((r & 0x3F) | /*R*/(((newReg ^ 8) & 8) << 4) | /*X*/(((newVidx ^ 8) & 8) << 3));
							bytes[vIndex] = (byte)((v & 0x87) | /*vvvv*/((newVvvv ^ 0xF) << 3));
						}
						bytes[mIndex] = (byte)((m & 0xC7) | /*modrm.reg*/((newReg & 7) << 3));
						bytes[sIndex] = (byte)((s & 0xC7) | /*sib.index*/((newVidx & 7) << 3));

						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
							Instruction instruction = decoder.decode();
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
						{
							Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
							Instruction instruction = decoder.decode();
							assertEquals(info.code, instruction.getCode());
							assertEquals(OpKind.REGISTER, instruction.getOp0Kind());
							assertEquals(OpKind.MEMORY, instruction.getOp1Kind());
							assertEquals(OpKind.REGISTER, instruction.getOp2Kind());
							assertNotEquals(Register.NONE, instruction.getMemoryIndex());
							assertEquals(newReg, getNumber(instruction.getOp0Register()));
							assertEquals(newVidx, getNumber(instruction.getMemoryIndex()));
							assertEquals(newVvvv, getNumber(instruction.getOp2Register()));
						}
					}
				}
			}
			else
				throw new UnsupportedOperationException();
		}
	}

	// All VX_VSIB_HX instructions, eg. VEX_Vpgatherdd_xmm_vm32x_xmm
	private static boolean canHaveInvalidIndexMaskDestRegister_VEX(OpCodeInfo opCode) {
		if (opCode.getEncoding() != EncodingKind.VEX && opCode.getEncoding() != EncodingKind.XOP)
			return false;

		switch (opCode.getOp0Kind()) {
		case OpCodeOperandKind.XMM_REG:
		case OpCodeOperandKind.YMM_REG:
		case OpCodeOperandKind.ZMM_REG:
			break;
		default:
			return false;
		}

		return opCode.getRequiresUniqueRegNums();
	}

	private static boolean isVsib(OpCodeInfo opCode) {
		return (tryGetVsib(opCode) & VsibFlags.VSIB) != 0;
	}

	private static int tryGetVsib(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
				return VsibFlags.VSIB32 | VsibFlags.VSIB;

			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB64Z:
				return VsibFlags.VSIB64 | VsibFlags.VSIB;
			}
		}

		return VsibFlags.NONE;
	}

	@Test
	void test_vsib_props() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(info.hexBytes)), info.options);
			Instruction instruction = decoder.decode();
			assertEquals(info.code, instruction.getCode());

			int flags = tryGetVsib(Code.toOpCode(info.code));
			assertEquals(instruction.isVsib(), (flags & VsibFlags.VSIB) != 0);
			assertEquals(instruction.isVsib32(), (flags & VsibFlags.VSIB32) != 0);
			assertEquals(instruction.isVsib64(), (flags & VsibFlags.VSIB64) != 0);
		}
	}

	static final class TestedInfo {
		// VEX/XOP.L and EVEX.L'L values
		public int lBits;// bit 0 = L0/L128, bit 1 = L1/L256, etc
		public int vex2_LBits;

		// REX/VEX/XOP/EVEX/MVEX: W values
		public int wBits;// bit 0 = W0, bit 1 = W1

		// REX/VEX/XOP/EVEX/MVEX.R
		public int rBits;
		public int vex2_RBits;
		// REX/VEX/XOP/EVEX/MVEX.X
		public int xBits;
		// REX/VEX/XOP/EVEX/MVEX.B
		public int bBits;
		// EVEX/MVEX.R'
		public int r2Bits;
		// EVEX/MVEX.V'
		public int v2Bits;

		// mod=11
		public boolean regReg;
		// mod=00,01,10
		public boolean regMem;

		// EVEX/MVEX only
		public boolean memDisp8;

		// Tested VEX2 prefix
		public boolean vex2;
		// Tested VEX3 prefix
		public boolean vex3;

		// EVEX/MVEX: tested opmask
		public boolean opMask;
		// EVEX/MVEX: tested no opmask
		public boolean noOpMask;

		public boolean prefixXacquire;
		public boolean prefixNoXacquire;
		public boolean prefixXrelease;
		public boolean prefixNoXrelease;
		public boolean prefixLock;
		public boolean prefixNoLock;
		public boolean prefixHnt;
		public boolean prefixNoHnt;
		public boolean prefixHt;
		public boolean prefixNoHt;
		public boolean prefixRep;
		public boolean prefixNoRep;
		public boolean prefixRepne;
		public boolean prefixNoRepne;
		public boolean prefixNotrack;
		public boolean prefixNoNotrack;
		public boolean prefixBnd;
		public boolean prefixNoBnd;
	}

	static class TupleIntInt {
		public final int value1;
		public final int value2;

		public TupleIntInt(int value1, int value2) {
			this.value1 = value1;
			this.value2 = value2;
		}

		@Override
		public int hashCode() {
			return value1 ^ value2;
		}

		@Override
		public boolean equals(Object obj) {
			if (!(obj instanceof TupleIntInt))
				return false;
			TupleIntInt other = (TupleIntInt)obj;
			return value1 == other.value1 && value2 == other.value2;
		}
	}

	@Test
	void verify_that_test_cases_test_enough_bits() {
		TestedInfo[] testedInfos16 = new TestedInfo[IcedConstants.CODE_ENUM_COUNT];
		TestedInfo[] testedInfos32 = new TestedInfo[IcedConstants.CODE_ENUM_COUNT];
		TestedInfo[] testedInfos64 = new TestedInfo[IcedConstants.CODE_ENUM_COUNT];
		for (int i = 0; i < testedInfos16.length; i++)
			testedInfos16[i] = new TestedInfo();
		for (int i = 0; i < testedInfos32.length; i++)
			testedInfos32[i] = new TestedInfo();
		for (int i = 0; i < testedInfos64.length; i++)
			testedInfos64[i] = new TestedInfo();

		boolean[] canUseW = new boolean[IcedConstants.CODE_ENUM_COUNT];
		{
			HashSet<TupleIntInt> usesW = new HashSet<TupleIntInt>();
			for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
				int code = i;
				OpCodeInfo opCode = Code.toOpCode(code);
				if (opCode.getEncoding() != EncodingKind.LEGACY)
					continue;
				if (opCode.getOperandSize() != 0)
					usesW.add(new TupleIntInt(opCode.getTable(), opCode.getOpCode()));
			}
			for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
				int code = i;
				OpCodeInfo opCode = Code.toOpCode(code);
				switch (opCode.getEncoding()) {
				case EncodingKind.LEGACY:
				case EncodingKind.D3NOW:
					canUseW[i] = !usesW.contains(new TupleIntInt(opCode.getTable(), opCode.getOpCode()));
					break;

				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
				case EncodingKind.MVEX:
					break;

				default:
					throw new UnsupportedOperationException();
				}
			}
		}

		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;
			TestedInfo[] testedInfos;
			switch (info.bitness) {
			case 16:
				testedInfos = testedInfos16;
				break;
			case 32:
				testedInfos = testedInfos32;
				break;
			case 64:
				testedInfos = testedInfos64;
				break;
			default:
				throw new UnsupportedOperationException();
			}

			OpCodeInfo opCode = Code.toOpCode(info.code);
			TestedInfo tested = testedInfos[info.code];

			byte[] bytes = HexUtils.toByteArray(info.hexBytes);
			Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
			Instruction instruction = decoder.decode();
			assertEquals(info.code, instruction.getCode());

			if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX) {
				int evexIndex = getEvexIndex(bytes);

				if (opCode.getEncoding() == EncodingKind.EVEX) {
					if (instruction.getRoundingControl() == RoundingControl.NONE)
						tested.lBits |= 1 << ((bytes[evexIndex + 3] >>> 5) & 3);

					byte ll = (byte)((bytes[evexIndex + 3] >>> 5) & 3);
					boolean invalid = (info.options & DecoderOptions.NO_INVALID_CHECK) == 0 &&
						ll == 3 && ((bytes[evexIndex + 5] & 0xFF) < 0xC0 || (bytes[evexIndex + 3] & 0x10) == 0);
					if (!invalid)
						tested.lBits |= 1 << 3;
				}

				tested.wBits |= 1 << ((bytes[evexIndex + 2] & 0xFF) >>> 7);
				tested.rBits |= 1 << (((bytes[evexIndex + 1] & 0xFF) >>> 7) ^ 1);
				tested.xBits |= 1 << (((bytes[evexIndex + 1] >>> 6) & 1) ^ 1);
				tested.bBits |= 1 << (((bytes[evexIndex + 1] >>> 5) & 1) ^ 1);
				tested.r2Bits |= 1 << (((bytes[evexIndex + 1] >>> 4) & 1) ^ 1);
				tested.v2Bits |= 1 << (((bytes[evexIndex + 3] >>> 3) & 1) ^ 1);
				if (((bytes[evexIndex + 5] & 0xFF) >>> 6) != 3) {
					tested.regMem = true;
					if (instruction.getMemoryDisplSize() == 1 && instruction.getMemoryDisplacement64() != 0)
						tested.memDisp8 = true;
				}
				else
					tested.regReg = true;
				if (instruction.getOpMask() != Register.NONE)
					tested.opMask = true;
				else
					tested.noOpMask = true;
			}
			else if (opCode.getEncoding() == EncodingKind.VEX || opCode.getEncoding() == EncodingKind.XOP) {
				int vexIndex = getVexXopIndex(bytes);
				int mrmi;
				if ((bytes[vexIndex] & 0xFF) == 0xC5) {
					mrmi = vexIndex + 3;
					tested.vex2 = true;
					tested.vex2_RBits |= 1 << (((bytes[vexIndex + 1] & 0xFF) >>> 7) ^ 1);
					tested.vex2_LBits |= 1 << ((bytes[vexIndex + 1] >>> 2) & 1);
				}
				else {
					mrmi = vexIndex + 4;
					if (opCode.getEncoding() == EncodingKind.VEX)
						tested.vex3 = true;
					tested.rBits |= 1 << (((bytes[vexIndex + 1] & 0xFF) >>> 7) ^ 1);
					tested.xBits |= 1 << (((bytes[vexIndex + 1] >>> 6) & 1) ^ 1);
					tested.bBits |= 1 << (((bytes[vexIndex + 1] >>> 5) & 1) ^ 1);
					tested.wBits |= 1 << ((bytes[vexIndex + 2] & 0xFF) >>> 7);
					tested.lBits |= 1 << ((bytes[vexIndex + 2] >>> 2) & 1);
				}
				if (hasModRM(opCode)) {
					if (((bytes[mrmi] & 0xFF) >>> 6) != 3)
						tested.regMem = true;
					else
						tested.regReg = true;
				}
			}
			else if (opCode.getEncoding() == EncodingKind.LEGACY || opCode.getEncoding() == EncodingKind.D3NOW) {
				SkipPrefixesResult skipResult = skipPrefixes(bytes, info.bitness);
				int i = skipResult.index;
				int rex = skipResult.rex;
				if (info.bitness == 64) {
					tested.wBits |= 1 << ((rex >>> 3) & 1);
					tested.rBits |= 1 << ((rex >>> 2) & 1);
					tested.xBits |= 1 << ((rex >>> 1) & 1);
					tested.bBits |= 1 << (rex & 1);
					// Can't access regs dr8-dr15
					if (info.code == Code.MOV_R64_DR || info.code == Code.MOV_DR_R64)
						tested.rBits |= 1 << 1;
				}
				else {
					tested.wBits |= 1;
					tested.rBits |= 1;
					tested.xBits |= 1;
					tested.bBits |= 1;
				}
				if (hasModRM(opCode)) {
					switch (opCode.getTable()) {
					case OpCodeTableKind.NORMAL:
						break;
					case OpCodeTableKind.T0F:
						if (bytes[i++] != 0x0F)
							throw new UnsupportedOperationException();
						break;
					case OpCodeTableKind.T0F38:
						if (bytes[i++] != 0x0F)
							throw new UnsupportedOperationException();
						if (bytes[i++] != 0x38)
							throw new UnsupportedOperationException();
						break;
					case OpCodeTableKind.T0F3A:
						if (bytes[i++] != 0x0F)
							throw new UnsupportedOperationException();
						if (bytes[i++] != 0x3A)
							throw new UnsupportedOperationException();
						break;
					default:
						throw new UnsupportedOperationException();
					}
					i++;
					if (((bytes[i] & 0xFF) >>> 6) != 3)
						tested.regMem = true;
					else
						tested.regReg = true;
				}
				if (opCode.canUseXacquirePrefix()) {
					if (instruction.getXacquirePrefix())
						tested.prefixXacquire = true;
					else
						tested.prefixNoXacquire = true;
				}
				if (opCode.canUseXreleasePrefix()) {
					if (instruction.getXreleasePrefix())
						tested.prefixXrelease = true;
					else
						tested.prefixNoXrelease = true;
				}
				if (opCode.canUseLockPrefix()) {
					if (instruction.getLockPrefix())
						tested.prefixLock = true;
					else
						tested.prefixNoLock = true;
				}
				if (opCode.canUseHintTakenPrefix()) {
					if (instruction.getSegmentPrefix() == Register.CS)
						tested.prefixHnt = true;
					else
						tested.prefixNoHnt = true;
				}
				if (opCode.canUseHintTakenPrefix()) {
					if (instruction.getSegmentPrefix() == Register.DS)
						tested.prefixHt = true;
					else
						tested.prefixNoHt = true;
				}
				if (opCode.canUseRepPrefix()) {
					if (instruction.getRepPrefix())
						tested.prefixRep = true;
					else
						tested.prefixNoRep = true;
				}
				if (opCode.canUseRepnePrefix()) {
					if (instruction.getRepnePrefix())
						tested.prefixRepne = true;
					else
						tested.prefixNoRepne = true;
				}
				if (opCode.canUseNotrackPrefix()) {
					if (instruction.getSegmentPrefix() == Register.DS)
						tested.prefixNotrack = true;
					else
						tested.prefixNoNotrack = true;
				}
				if (opCode.canUseBndPrefix()) {
					if (instruction.getRepnePrefix())
						tested.prefixBnd = true;
					else
						tested.prefixNoBnd = true;
				}
			}
			else
				throw new UnsupportedOperationException();
		}

		ArrayList<Integer> wig32_16 = new ArrayList<Integer>();
		ArrayList<Integer> wig32_32 = new ArrayList<Integer>();

		ArrayList<Integer> wig_16 = new ArrayList<Integer>();
		ArrayList<Integer> wig_32 = new ArrayList<Integer>();
		ArrayList<Integer> wig_64 = new ArrayList<Integer>();

		ArrayList<Integer> w_64 = new ArrayList<Integer>();

		ArrayList<Integer> lig_16 = new ArrayList<Integer>();
		ArrayList<Integer> lig_32 = new ArrayList<Integer>();
		ArrayList<Integer> lig_64 = new ArrayList<Integer>();

		ArrayList<Integer> vex2_lig_16 = new ArrayList<Integer>();
		ArrayList<Integer> vex2_lig_32 = new ArrayList<Integer>();
		ArrayList<Integer> vex2_lig_64 = new ArrayList<Integer>();

		ArrayList<Integer> rr_16 = new ArrayList<Integer>();
		ArrayList<Integer> rr_32 = new ArrayList<Integer>();
		ArrayList<Integer> rr_64 = new ArrayList<Integer>();

		ArrayList<Integer> rm_16 = new ArrayList<Integer>();
		ArrayList<Integer> rm_32 = new ArrayList<Integer>();
		ArrayList<Integer> rm_64 = new ArrayList<Integer>();

		ArrayList<Integer> disp8_16 = new ArrayList<Integer>();
		ArrayList<Integer> disp8_32 = new ArrayList<Integer>();
		ArrayList<Integer> disp8_64 = new ArrayList<Integer>();

		ArrayList<Integer> vex2_16 = new ArrayList<Integer>();
		ArrayList<Integer> vex2_32 = new ArrayList<Integer>();
		ArrayList<Integer> vex2_64 = new ArrayList<Integer>();

		ArrayList<Integer> vex3_16 = new ArrayList<Integer>();
		ArrayList<Integer> vex3_32 = new ArrayList<Integer>();
		ArrayList<Integer> vex3_64 = new ArrayList<Integer>();

		ArrayList<Integer> opmask_16 = new ArrayList<Integer>();
		ArrayList<Integer> opmask_32 = new ArrayList<Integer>();
		ArrayList<Integer> opmask_64 = new ArrayList<Integer>();

		ArrayList<Integer> noopmask_16 = new ArrayList<Integer>();
		ArrayList<Integer> noopmask_32 = new ArrayList<Integer>();
		ArrayList<Integer> noopmask_64 = new ArrayList<Integer>();

		ArrayList<Integer> b_16 = new ArrayList<Integer>();
		ArrayList<Integer> b_32 = new ArrayList<Integer>();
		ArrayList<Integer> b_64 = new ArrayList<Integer>();

		ArrayList<Integer> r2_16 = new ArrayList<Integer>();
		ArrayList<Integer> r2_32 = new ArrayList<Integer>();
		ArrayList<Integer> r2_64 = new ArrayList<Integer>();

		ArrayList<Integer> r_64 = new ArrayList<Integer>();
		ArrayList<Integer> vex2_r_64 = new ArrayList<Integer>();
		ArrayList<Integer> x_64 = new ArrayList<Integer>();
		ArrayList<Integer> v2_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_xacquire_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_xacquire_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_xacquire_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_xrelease_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_xrelease_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_xrelease_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_lock_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_lock_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_lock_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_hnt_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_hnt_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_hnt_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_ht_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_ht_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_ht_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_rep_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_rep_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_rep_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_repne_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_repne_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_repne_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_notrack_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_notrack_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_notrack_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_bnd_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_bnd_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_bnd_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_xacquire_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_xacquire_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_xacquire_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_xrelease_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_xrelease_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_xrelease_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_lock_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_lock_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_lock_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_hnt_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_hnt_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_hnt_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_ht_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_ht_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_ht_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_rep_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_rep_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_rep_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_repne_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_repne_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_repne_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_notrack_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_notrack_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_notrack_64 = new ArrayList<Integer>();

		ArrayList<Integer> pfx_no_bnd_16 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_bnd_32 = new ArrayList<Integer>();
		ArrayList<Integer> pfx_no_bnd_64 = new ArrayList<Integer>();

		String[] codeNames = ToCode.names();
		for (int bitness : new int[] { 16, 32, 64 }) {
			TestedInfo[] testedInfos;
			switch (bitness) {
			case 16:
				testedInfos = testedInfos16;
				break;
			case 32:
				testedInfos = testedInfos32;
				break;
			case 64:
				testedInfos = testedInfos64;
				break;
			default:
				throw new UnsupportedOperationException();
			}

			for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
				if (CodeUtils.isIgnored(codeNames[i]))
					continue;
				int code = i;
				switch (code) {
				case Code.MONTMUL_16:
				case Code.MONTMUL_64:
					continue;
				}
				OpCodeInfo opCode = Code.toOpCode(code);
				if (!opCode.isInstruction() || opCode.getCode() == Code.POPW_CS)
					continue;
				if (opCode.getFwait())
					continue;

				switch (bitness) {
				case 16:
					if (!opCode.getMode16())
						continue;
					break;
				case 32:
					if (!opCode.getMode32())
						continue;
					break;
				case 64:
					if (!opCode.getMode64())
						continue;
					break;
				default:
					throw new UnsupportedOperationException();
				}

				TestedInfo tested = testedInfos[i];

				if ((bitness == 16 || bitness == 32) && opCode.isWIG32()) {
					if (tested.wBits != 3)
						getList2(bitness, wig32_16, wig32_32).add(code);
				}
				if (opCode.isWIG()) {
					if (tested.wBits != 3)
						getList(bitness, wig_16, wig_32, wig_64).add(code);
				}
				if (bitness == 64 && opCode.getMode64() && (opCode.getEncoding() == EncodingKind.LEGACY || opCode.getEncoding() == EncodingKind.D3NOW)) {
					assert !opCode.isWIG();
					assert !opCode.isWIG32();
					if (canUseW[code] && tested.wBits != 3)
						w_64.add(code);
				}
				if (opCode.isLIG()) {
					int allLBits;
					switch (opCode.getEncoding()) {
					case EncodingKind.VEX:
					case EncodingKind.XOP:
						allLBits = 3;// 1 bit = 2 values
						break;

					case EncodingKind.EVEX:
						allLBits = 0xF;// 2 bits = 4 values
						break;

					case EncodingKind.LEGACY:
					case EncodingKind.D3NOW:
					case EncodingKind.MVEX:
					default:
						throw new UnsupportedOperationException();
					}
					if (tested.lBits != allLBits)
						getList(bitness, lig_16, lig_32, lig_64).add(code);
				}
				if (opCode.isLIG() && opCode.getEncoding() == EncodingKind.VEX) {
					if (tested.vex2_LBits != 3 && canUseVEX2(opCode))
						getList(bitness, vex2_lig_16, vex2_lig_32, vex2_lig_64).add(code);
				}
				if (canUseModRM_rm_mem(opCode)) {
					if (!tested.regMem)
						getList(bitness, rm_16, rm_32, rm_64).add(code);
				}
				if (canUseModRM_rm_reg(opCode)) {
					if (!tested.regReg)
						getList(bitness, rr_16, rr_32, rr_64).add(code);
				}
				switch (opCode.getEncoding()) {
				case EncodingKind.LEGACY:
				case EncodingKind.VEX:
				case EncodingKind.XOP:
				case EncodingKind.D3NOW:
					break;
				case EncodingKind.EVEX:
				case EncodingKind.MVEX:
					if (!tested.memDisp8 && canUseModRM_rm_mem(opCode))
						getList(bitness, disp8_16, disp8_32, disp8_64).add(code);
					break;
				default:
					throw new UnsupportedOperationException();
				}
				if (opCode.getEncoding() == EncodingKind.VEX) {
					if (!tested.vex3)
						getList(bitness, vex3_16, vex3_32, vex3_64).add(code);
					if (!tested.vex2 && canUseVEX2(opCode))
						getList(bitness, vex2_16, vex2_32, vex2_64).add(code);
				}
				if (opCode.canUseOpMaskRegister()) {
					if (!tested.opMask)
						getList(bitness, opmask_16, opmask_32, opmask_64).add(code);
					if (!tested.noOpMask && !opCode.getRequireOpMaskRegister())
						getList(bitness, noopmask_16, noopmask_32, noopmask_64).add(code);
				}
				if (canUseB(bitness, opCode)) {
					if (tested.bBits != 3)
						getList(bitness, b_16, b_32, b_64).add(code);
				}
				else {
					if ((tested.bBits & 1) == 0)
						getList(bitness, b_16, b_32, b_64).add(code);
				}
				switch (opCode.getEncoding()) {
				case EncodingKind.EVEX:
				case EncodingKind.MVEX:
					if (canUseR2(opCode)) {
						if (tested.r2Bits != 3)
							getList(bitness, r2_16, r2_32, r2_64).add(code);
					}
					else {
						if ((tested.r2Bits & 1) == 0)
							getList(bitness, r2_16, r2_32, r2_64).add(code);
					}
					break;
				case EncodingKind.LEGACY:
				case EncodingKind.VEX:
				case EncodingKind.XOP:
				case EncodingKind.D3NOW:
					break;
				default:
					throw new UnsupportedOperationException();
				}
				if (bitness == 64 && opCode.getMode64()) {
					if (tested.vex2_RBits != 3 && opCode.getEncoding() == EncodingKind.VEX && canUseVEX2(opCode) && canUseR(opCode))
						vex2_r_64.add(code);
					if (canUseR(opCode)) {
						if (tested.rBits != 3)
							r_64.add(code);
					}
					else {
						if ((tested.rBits & 1) == 0)
							r_64.add(code);
					}
					if (isVsib(opCode)) {
						// The memory tests test vsib memory operands
					}
					else if (canUseX(opCode)) {
						if (tested.xBits != 3)
							x_64.add(code);
					}
					else {
						if ((tested.xBits & 1) == 0)
							x_64.add(code);
					}
					switch (opCode.getEncoding()) {
					case EncodingKind.EVEX:
					case EncodingKind.MVEX:
						if (isVsib(opCode)) {
							// The memory tests test vsib memory operands
						}
						else if (canUseV2(opCode)) {
							if (tested.v2Bits != 3)
								v2_64.add(code);
						}
						else {
							if ((tested.v2Bits & 1) == 0)
								v2_64.add(code);
						}
						break;
					case EncodingKind.LEGACY:
					case EncodingKind.VEX:
					case EncodingKind.XOP:
					case EncodingKind.D3NOW:
						break;
					default:
						throw new UnsupportedOperationException();
					}
				}
				if (opCode.canUseXacquirePrefix()) {
					if (!tested.prefixXacquire)
						getList(bitness, pfx_xacquire_16, pfx_xacquire_32, pfx_xacquire_64).add(code);
					if (!tested.prefixNoXacquire)
						getList(bitness, pfx_no_xacquire_16, pfx_no_xacquire_32, pfx_no_xacquire_64).add(code);
				}
				if (opCode.canUseXreleasePrefix()) {
					if (!tested.prefixXrelease)
						getList(bitness, pfx_xrelease_16, pfx_xrelease_32, pfx_xrelease_64).add(code);
					if (!tested.prefixNoXrelease)
						getList(bitness, pfx_no_xrelease_16, pfx_no_xrelease_32, pfx_no_xrelease_64).add(code);
				}
				if (opCode.canUseLockPrefix()) {
					if (!tested.prefixLock)
						getList(bitness, pfx_lock_16, pfx_lock_32, pfx_lock_64).add(code);
					if (!tested.prefixNoLock)
						getList(bitness, pfx_no_lock_16, pfx_no_lock_32, pfx_no_lock_64).add(code);
				}
				if (opCode.canUseHintTakenPrefix()) {
					if (!tested.prefixHnt)
						getList(bitness, pfx_hnt_16, pfx_hnt_32, pfx_hnt_64).add(code);
					if (!tested.prefixNoHnt)
						getList(bitness, pfx_no_hnt_16, pfx_no_hnt_32, pfx_no_hnt_64).add(code);
				}
				if (opCode.canUseHintTakenPrefix()) {
					if (!tested.prefixHt)
						getList(bitness, pfx_ht_16, pfx_ht_32, pfx_ht_64).add(code);
					if (!tested.prefixNoHt)
						getList(bitness, pfx_no_ht_16, pfx_no_ht_32, pfx_no_ht_64).add(code);
				}
				if (opCode.canUseRepPrefix()) {
					if (!tested.prefixRep)
						getList(bitness, pfx_rep_16, pfx_rep_32, pfx_rep_64).add(code);
					if (!tested.prefixNoRep)
						getList(bitness, pfx_no_rep_16, pfx_no_rep_32, pfx_no_rep_64).add(code);
				}
				if (opCode.canUseRepnePrefix()) {
					if (!tested.prefixRepne)
						getList(bitness, pfx_repne_16, pfx_repne_32, pfx_repne_64).add(code);
					if (!tested.prefixNoRepne)
						getList(bitness, pfx_no_repne_16, pfx_no_repne_32, pfx_no_repne_64).add(code);
				}
				if (opCode.canUseNotrackPrefix()) {
					if (!tested.prefixNotrack)
						getList(bitness, pfx_notrack_16, pfx_notrack_32, pfx_notrack_64).add(code);
					if (!tested.prefixNoNotrack)
						getList(bitness, pfx_no_notrack_16, pfx_no_notrack_32, pfx_no_notrack_64).add(code);
				}
				if (opCode.canUseBndPrefix()) {
					if (!tested.prefixBnd)
						getList(bitness, pfx_bnd_16, pfx_bnd_32, pfx_bnd_64).add(code);
					if (!tested.prefixNoBnd)
						getList(bitness, pfx_no_bnd_16, pfx_no_bnd_32, pfx_no_bnd_64).add(code);
				}
			}
		}

		assertEquals("wig32_16:", "wig32_16:" + joinInts(wig32_16));
		assertEquals("wig32_32:", "wig32_32:" + joinInts(wig32_32));
		assertEquals("wig_16:", "wig_16:" + joinInts(wig_16));
		assertEquals("wig_32:", "wig_32:" + joinInts(wig_32));
		assertEquals("wig_64:", "wig_64:" + joinInts(wig_64));
		assertEquals("w_64:", "w_64:" + joinInts(w_64));
		assertEquals("lig_16:", "lig_16:" + joinInts(lig_16));
		assertEquals("lig_32:", "lig_32:" + joinInts(lig_32));
		assertEquals("lig_64:", "lig_64:" + joinInts(lig_64));
		assertEquals("vex2_lig_16:", "vex2_lig_16:" + joinInts(vex2_lig_16));
		assertEquals("vex2_lig_32:", "vex2_lig_32:" + joinInts(vex2_lig_32));
		assertEquals("vex2_lig_64:", "vex2_lig_64:" + joinInts(vex2_lig_64));
		assertEquals("rr_16:", "rr_16:" + joinInts(rr_16));
		assertEquals("rr_32:", "rr_32:" + joinInts(rr_32));
		assertEquals("rr_64:", "rr_64:" + joinInts(rr_64));
		assertEquals("rm_16:", "rm_16:" + joinInts(rm_16));
		assertEquals("rm_32:", "rm_32:" + joinInts(rm_32));
		assertEquals("rm_64:", "rm_64:" + joinInts(rm_64));
		assertEquals("disp8_16:", "disp8_16:" + joinInts(disp8_16));
		assertEquals("disp8_32:", "disp8_32:" + joinInts(disp8_32));
		assertEquals("disp8_64:", "disp8_64:" + joinInts(disp8_64));
		assertEquals("vex2_16:", "vex2_16:" + joinInts(vex2_16));
		assertEquals("vex2_32:", "vex2_32:" + joinInts(vex2_32));
		assertEquals("vex2_64:", "vex2_64:" + joinInts(vex2_64));
		assertEquals("vex3_16:", "vex3_16:" + joinInts(vex3_16));
		assertEquals("vex3_32:", "vex3_32:" + joinInts(vex3_32));
		assertEquals("vex3_64:", "vex3_64:" + joinInts(vex3_64));
		assertEquals("opmask_16:", "opmask_16:" + joinInts(opmask_16));
		assertEquals("opmask_32:", "opmask_32:" + joinInts(opmask_32));
		assertEquals("opmask_64:", "opmask_64:" + joinInts(opmask_64));
		assertEquals("noopmask_16:", "noopmask_16:" + joinInts(noopmask_16));
		assertEquals("noopmask_32:", "noopmask_32:" + joinInts(noopmask_32));
		assertEquals("noopmask_64:", "noopmask_64:" + joinInts(noopmask_64));
		assertEquals("b_16:", "b_16:" + joinInts(b_16));
		assertEquals("b_32:", "b_32:" + joinInts(b_32));
		assertEquals("b_64:", "b_64:" + joinInts(b_64));
		assertEquals("r2_16:", "r2_16:" + joinInts(r2_16));
		assertEquals("r2_32:", "r2_32:" + joinInts(r2_32));
		assertEquals("r2_64:", "r2_64:" + joinInts(r2_64));
		assertEquals("r_64:", "r_64:" + joinInts(r_64));
		assertEquals("vex2_r_64:", "vex2_r_64:" + joinInts(vex2_r_64));
		assertEquals("x_64:", "x_64:" + joinInts(x_64));
		assertEquals("v2_64:", "v2_64:" + joinInts(v2_64));
		assertEquals("pfx_xacquire_16:", "pfx_xacquire_16:" + joinInts(pfx_xacquire_16));
		assertEquals("pfx_xacquire_32:", "pfx_xacquire_32:" + joinInts(pfx_xacquire_32));
		assertEquals("pfx_xacquire_64:", "pfx_xacquire_64:" + joinInts(pfx_xacquire_64));
		assertEquals("pfx_xrelease_16:", "pfx_xrelease_16:" + joinInts(pfx_xrelease_16));
		assertEquals("pfx_xrelease_32:", "pfx_xrelease_32:" + joinInts(pfx_xrelease_32));
		assertEquals("pfx_xrelease_64:", "pfx_xrelease_64:" + joinInts(pfx_xrelease_64));
		assertEquals("pfx_lock_16:", "pfx_lock_16:" + joinInts(pfx_lock_16));
		assertEquals("pfx_lock_32:", "pfx_lock_32:" + joinInts(pfx_lock_32));
		assertEquals("pfx_lock_64:", "pfx_lock_64:" + joinInts(pfx_lock_64));
		assertEquals("pfx_hnt_16:", "pfx_hnt_16:" + joinInts(pfx_hnt_16));
		assertEquals("pfx_hnt_32:", "pfx_hnt_32:" + joinInts(pfx_hnt_32));
		assertEquals("pfx_hnt_64:", "pfx_hnt_64:" + joinInts(pfx_hnt_64));
		assertEquals("pfx_ht_16:", "pfx_ht_16:" + joinInts(pfx_ht_16));
		assertEquals("pfx_ht_32:", "pfx_ht_32:" + joinInts(pfx_ht_32));
		assertEquals("pfx_ht_64:", "pfx_ht_64:" + joinInts(pfx_ht_64));
		assertEquals("pfx_rep_16:", "pfx_rep_16:" + joinInts(pfx_rep_16));
		assertEquals("pfx_rep_32:", "pfx_rep_32:" + joinInts(pfx_rep_32));
		assertEquals("pfx_rep_64:", "pfx_rep_64:" + joinInts(pfx_rep_64));
		assertEquals("pfx_repne_16:", "pfx_repne_16:" + joinInts(pfx_repne_16));
		assertEquals("pfx_repne_32:", "pfx_repne_32:" + joinInts(pfx_repne_32));
		assertEquals("pfx_repne_64:", "pfx_repne_64:" + joinInts(pfx_repne_64));
		assertEquals("pfx_notrack_16:", "pfx_notrack_16:" + joinInts(pfx_notrack_16));
		assertEquals("pfx_notrack_32:", "pfx_notrack_32:" + joinInts(pfx_notrack_32));
		assertEquals("pfx_notrack_64:", "pfx_notrack_64:" + joinInts(pfx_notrack_64));
		assertEquals("pfx_bnd_16:", "pfx_bnd_16:" + joinInts(pfx_bnd_16));
		assertEquals("pfx_bnd_32:", "pfx_bnd_32:" + joinInts(pfx_bnd_32));
		assertEquals("pfx_bnd_64:", "pfx_bnd_64:" + joinInts(pfx_bnd_64));
		assertEquals("pfx_no_xacquire_16:", "pfx_no_xacquire_16:" + joinInts(pfx_no_xacquire_16));
		assertEquals("pfx_no_xacquire_32:", "pfx_no_xacquire_32:" + joinInts(pfx_no_xacquire_32));
		assertEquals("pfx_no_xacquire_64:", "pfx_no_xacquire_64:" + joinInts(pfx_no_xacquire_64));
		assertEquals("pfx_no_xrelease_16:", "pfx_no_xrelease_16:" + joinInts(pfx_no_xrelease_16));
		assertEquals("pfx_no_xrelease_32:", "pfx_no_xrelease_32:" + joinInts(pfx_no_xrelease_32));
		assertEquals("pfx_no_xrelease_64:", "pfx_no_xrelease_64:" + joinInts(pfx_no_xrelease_64));
		assertEquals("pfx_no_lock_16:", "pfx_no_lock_16:" + joinInts(pfx_no_lock_16));
		assertEquals("pfx_no_lock_32:", "pfx_no_lock_32:" + joinInts(pfx_no_lock_32));
		assertEquals("pfx_no_lock_64:", "pfx_no_lock_64:" + joinInts(pfx_no_lock_64));
		assertEquals("pfx_no_hnt_16:", "pfx_no_hnt_16:" + joinInts(pfx_no_hnt_16));
		assertEquals("pfx_no_hnt_32:", "pfx_no_hnt_32:" + joinInts(pfx_no_hnt_32));
		assertEquals("pfx_no_hnt_64:", "pfx_no_hnt_64:" + joinInts(pfx_no_hnt_64));
		assertEquals("pfx_no_ht_16:", "pfx_no_ht_16:" + joinInts(pfx_no_ht_16));
		assertEquals("pfx_no_ht_32:", "pfx_no_ht_32:" + joinInts(pfx_no_ht_32));
		assertEquals("pfx_no_ht_64:", "pfx_no_ht_64:" + joinInts(pfx_no_ht_64));
		assertEquals("pfx_no_rep_16:", "pfx_no_rep_16:" + joinInts(pfx_no_rep_16));
		assertEquals("pfx_no_rep_32:", "pfx_no_rep_32:" + joinInts(pfx_no_rep_32));
		assertEquals("pfx_no_rep_64:", "pfx_no_rep_64:" + joinInts(pfx_no_rep_64));
		assertEquals("pfx_no_repne_16:", "pfx_no_repne_16:" + joinInts(pfx_no_repne_16));
		assertEquals("pfx_no_repne_32:", "pfx_no_repne_32:" + joinInts(pfx_no_repne_32));
		assertEquals("pfx_no_repne_64:", "pfx_no_repne_64:" + joinInts(pfx_no_repne_64));
		assertEquals("pfx_no_notrack_16:", "pfx_no_notrack_16:" + joinInts(pfx_no_notrack_16));
		assertEquals("pfx_no_notrack_32:", "pfx_no_notrack_32:" + joinInts(pfx_no_notrack_32));
		assertEquals("pfx_no_notrack_64:", "pfx_no_notrack_64:" + joinInts(pfx_no_notrack_64));
		assertEquals("pfx_no_bnd_16:", "pfx_no_bnd_16:" + joinInts(pfx_no_bnd_16));
		assertEquals("pfx_no_bnd_32:", "pfx_no_bnd_32:" + joinInts(pfx_no_bnd_32));
		assertEquals("pfx_no_bnd_64:", "pfx_no_bnd_64:" + joinInts(pfx_no_bnd_64));
	}

	private static String joinInts(ArrayList<Integer> list) {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < list.size(); i++) {
			if (i > 0)
				sb.append(",");
			sb.append(list.get(i));
		}
		return sb.toString();
	}

	private static boolean canUseModRM_rm_reg(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.R8_OR_MEM:
			case OpCodeOperandKind.R16_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM_MPX:
			case OpCodeOperandKind.R64_OR_MEM:
			case OpCodeOperandKind.R64_OR_MEM_MPX:
			case OpCodeOperandKind.MM_OR_MEM:
			case OpCodeOperandKind.XMM_OR_MEM:
			case OpCodeOperandKind.YMM_OR_MEM:
			case OpCodeOperandKind.ZMM_OR_MEM:
			case OpCodeOperandKind.BND_OR_MEM_MPX:
			case OpCodeOperandKind.K_OR_MEM:
			case OpCodeOperandKind.R16_RM:
			case OpCodeOperandKind.R32_RM:
			case OpCodeOperandKind.R64_RM:
			case OpCodeOperandKind.K_RM:
			case OpCodeOperandKind.MM_RM:
			case OpCodeOperandKind.XMM_RM:
			case OpCodeOperandKind.YMM_RM:
			case OpCodeOperandKind.ZMM_RM:
			case OpCodeOperandKind.TMM_RM:
				return true;
			}
		}
		return false;
	}

	private static boolean canUseModRM_rm_mem(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.MEM:
			case OpCodeOperandKind.SIBMEM:
			case OpCodeOperandKind.MEM_MPX:
			case OpCodeOperandKind.MEM_MIB:
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64Z:
			case OpCodeOperandKind.R8_OR_MEM:
			case OpCodeOperandKind.R16_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM_MPX:
			case OpCodeOperandKind.R64_OR_MEM:
			case OpCodeOperandKind.R64_OR_MEM_MPX:
			case OpCodeOperandKind.MM_OR_MEM:
			case OpCodeOperandKind.XMM_OR_MEM:
			case OpCodeOperandKind.YMM_OR_MEM:
			case OpCodeOperandKind.ZMM_OR_MEM:
			case OpCodeOperandKind.BND_OR_MEM_MPX:
			case OpCodeOperandKind.K_OR_MEM:
				return true;
			}
		}
		return false;
	}

	private static boolean canUseVEX2(OpCodeInfo opCode) {
		return opCode.getTable() == OpCodeTableKind.T0F && opCode.getW() == 0;
	}

	private static boolean canUseB(int bitness, OpCodeInfo opCode) {
		switch (opCode.getCode()) {
		case Code.NOPW:
		case Code.NOPD:
		case Code.NOPQ:
		case Code.BNDMOV_BND_BNDM128:
		case Code.BNDMOV_BNDM128_BND:
			return false;
		}

		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.MEM:
			case OpCodeOperandKind.SIBMEM:
			case OpCodeOperandKind.MEM_MPX:
			case OpCodeOperandKind.MEM_MIB:
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB64Z:
				// The memory test tests all combinations
				return false;
			}
		}

		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.TMM_RM:
				return false;

			case OpCodeOperandKind.K_RM:
			case OpCodeOperandKind.MM_RM:
			case OpCodeOperandKind.R16_RM:
			case OpCodeOperandKind.R32_RM:
			case OpCodeOperandKind.R64_RM:
			case OpCodeOperandKind.XMM_RM:
			case OpCodeOperandKind.YMM_RM:
			case OpCodeOperandKind.ZMM_RM:

			case OpCodeOperandKind.BND_OR_MEM_MPX:
			case OpCodeOperandKind.K_OR_MEM:
			case OpCodeOperandKind.MM_OR_MEM:
			case OpCodeOperandKind.R16_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM_MPX:
			case OpCodeOperandKind.R64_OR_MEM:
			case OpCodeOperandKind.R64_OR_MEM_MPX:
			case OpCodeOperandKind.R8_OR_MEM:
			case OpCodeOperandKind.XMM_OR_MEM:
			case OpCodeOperandKind.YMM_OR_MEM:
			case OpCodeOperandKind.ZMM_OR_MEM:
				if (opCode.getEncoding() == EncodingKind.LEGACY || opCode.getEncoding() == EncodingKind.D3NOW)
					return bitness == 64;
				return true;
			}
		}
		if (opCode.getEncoding() == EncodingKind.LEGACY || opCode.getEncoding() == EncodingKind.D3NOW)
			return bitness == 64;
		return true;
	}

	private static boolean canUseX(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.K_RM:
			case OpCodeOperandKind.MM_RM:
			case OpCodeOperandKind.R16_RM:
			case OpCodeOperandKind.R32_RM:
			case OpCodeOperandKind.R64_RM:
			case OpCodeOperandKind.XMM_RM:
			case OpCodeOperandKind.YMM_RM:
			case OpCodeOperandKind.ZMM_RM:
			case OpCodeOperandKind.TMM_RM:

			case OpCodeOperandKind.BND_OR_MEM_MPX:
			case OpCodeOperandKind.K_OR_MEM:
			case OpCodeOperandKind.MM_OR_MEM:
			case OpCodeOperandKind.R16_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM_MPX:
			case OpCodeOperandKind.R64_OR_MEM:
			case OpCodeOperandKind.R64_OR_MEM_MPX:
			case OpCodeOperandKind.R8_OR_MEM:
			case OpCodeOperandKind.XMM_OR_MEM:
			case OpCodeOperandKind.YMM_OR_MEM:
			case OpCodeOperandKind.ZMM_OR_MEM:
				return true;

			case OpCodeOperandKind.MEM:
			case OpCodeOperandKind.SIBMEM:
			case OpCodeOperandKind.MEM_MPX:
			case OpCodeOperandKind.MEM_MIB:
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB64Z:
				// The memory test tests all combinations
				return false;
			}
		}
		return true;
	}

	private static boolean canUseR(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.K_REG:
			case OpCodeOperandKind.KP1_REG:
			case OpCodeOperandKind.TR_REG:
			case OpCodeOperandKind.BND_REG:
			case OpCodeOperandKind.TMM_REG:
				return false;

			case OpCodeOperandKind.CR_REG:
			case OpCodeOperandKind.DR_REG:
			case OpCodeOperandKind.MM_REG:
			case OpCodeOperandKind.R16_REG:
			case OpCodeOperandKind.R32_REG:
			case OpCodeOperandKind.R64_REG:
			case OpCodeOperandKind.R8_REG:
			case OpCodeOperandKind.SEG_REG:
			case OpCodeOperandKind.XMM_REG:
			case OpCodeOperandKind.YMM_REG:
			case OpCodeOperandKind.ZMM_REG:
				return true;
			}
		}
		return true;
	}

	private static boolean canUseR2(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.K_REG:
			case OpCodeOperandKind.KP1_REG:
			case OpCodeOperandKind.TR_REG:
			case OpCodeOperandKind.BND_REG:
			case OpCodeOperandKind.CR_REG:
			case OpCodeOperandKind.DR_REG:
			case OpCodeOperandKind.MM_REG:
			case OpCodeOperandKind.R16_REG:
			case OpCodeOperandKind.R32_REG:
			case OpCodeOperandKind.R64_REG:
			case OpCodeOperandKind.R8_REG:
			case OpCodeOperandKind.SEG_REG:
			case OpCodeOperandKind.TMM_REG:
				return false;

			case OpCodeOperandKind.XMM_REG:
			case OpCodeOperandKind.YMM_REG:
			case OpCodeOperandKind.ZMM_REG:
				return true;
			}
		}
		return true;
	}

	private static boolean canUseV2(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.K_VVVV:
			case OpCodeOperandKind.R32_VVVV:
			case OpCodeOperandKind.R64_VVVV:
			case OpCodeOperandKind.TMM_VVVV:
				return false;

			case OpCodeOperandKind.XMM_VVVV:
			case OpCodeOperandKind.XMMP3_VVVV:
			case OpCodeOperandKind.YMM_VVVV:
			case OpCodeOperandKind.ZMM_VVVV:
			case OpCodeOperandKind.ZMMP3_VVVV:
				return true;

			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB64Z:
				// The memory test tests all combinations
				return false;
			}
		}
		return false;
	}

	private static boolean hasModRM(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.MEM:
			case OpCodeOperandKind.SIBMEM:
			case OpCodeOperandKind.MEM_MPX:
			case OpCodeOperandKind.MEM_MIB:
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64Z:
			case OpCodeOperandKind.R8_OR_MEM:
			case OpCodeOperandKind.R16_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM:
			case OpCodeOperandKind.R32_OR_MEM_MPX:
			case OpCodeOperandKind.R64_OR_MEM:
			case OpCodeOperandKind.R64_OR_MEM_MPX:
			case OpCodeOperandKind.MM_OR_MEM:
			case OpCodeOperandKind.XMM_OR_MEM:
			case OpCodeOperandKind.YMM_OR_MEM:
			case OpCodeOperandKind.ZMM_OR_MEM:
			case OpCodeOperandKind.BND_OR_MEM_MPX:
			case OpCodeOperandKind.K_OR_MEM:
			case OpCodeOperandKind.R8_REG:
			case OpCodeOperandKind.R16_REG:
			case OpCodeOperandKind.R16_RM:
			case OpCodeOperandKind.R32_REG:
			case OpCodeOperandKind.R32_RM:
			case OpCodeOperandKind.R64_REG:
			case OpCodeOperandKind.R64_RM:
			case OpCodeOperandKind.SEG_REG:
			case OpCodeOperandKind.K_REG:
			case OpCodeOperandKind.KP1_REG:
			case OpCodeOperandKind.K_RM:
			case OpCodeOperandKind.MM_REG:
			case OpCodeOperandKind.MM_RM:
			case OpCodeOperandKind.XMM_REG:
			case OpCodeOperandKind.XMM_RM:
			case OpCodeOperandKind.YMM_REG:
			case OpCodeOperandKind.YMM_RM:
			case OpCodeOperandKind.ZMM_REG:
			case OpCodeOperandKind.ZMM_RM:
			case OpCodeOperandKind.TMM_REG:
			case OpCodeOperandKind.TMM_RM:
			case OpCodeOperandKind.CR_REG:
			case OpCodeOperandKind.DR_REG:
			case OpCodeOperandKind.TR_REG:
			case OpCodeOperandKind.BND_REG:
				return true;
			}
		}
		return false;
	}

	private static ArrayList<Integer> getList2(int bitness, ArrayList<Integer> l16, ArrayList<Integer> l32) {
		switch (bitness) {
		case 16:
			return l16;
		case 32:
			return l32;
		default:
			throw new UnsupportedOperationException();
		}
	}

	private static ArrayList<Integer> getList(int bitness, ArrayList<Integer> l16, ArrayList<Integer> l32, ArrayList<Integer> l64) {
		switch (bitness) {
		case 16:
			return l16;
		case 32:
			return l32;
		case 64:
			return l64;
		default:
			throw new UnsupportedOperationException();
		}
	}

	@Test
	void test_invalid_zero_opmask_reg() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if ((info.options & DecoderOptions.NO_INVALID_CHECK) != 0)
				continue;
			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (!opCode.getRequireOpMaskRegister())
				continue;

			byte[] bytes = HexUtils.toByteArray(info.hexBytes);
			Instruction origInstr;
			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
				origInstr = decoder.decode();
				assertEquals(info.code, origInstr.getCode());
			}

			int evexIndex = getEvexIndex(bytes);
			bytes[evexIndex + 3] &= 0xF8;
			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
				Instruction instruction = decoder.decode();
				assertEquals(Code.INVALID, instruction.getCode());
				assertNotEquals(DecoderError.NONE, decoder.getLastError());
			}
			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options | DecoderOptions.NO_INVALID_CHECK);
				Instruction instruction = decoder.decode();
				assertEquals(info.code, instruction.getCode());
				assertEquals(Register.NONE, instruction.getOpMask());
				origInstr.setOpMask(Register.NONE);
				assertTrue(origInstr.equalsAllBits(instruction));
			}
		}
	}

	@Test
	void verify_cpu_mode() {
		HashSet<Integer> hash1632 = new HashSet<Integer>(DecoderTestUtils.code32Only);
		for (Integer code : DecoderTestUtils.notDecoded32Only)
			hash1632.add(code);
		HashSet<Integer> hash64 = new HashSet<Integer>(DecoderTestUtils.code64Only);
		for (Integer code : DecoderTestUtils.notDecoded64Only)
			hash64.add(code);
		String[] codeNames = ToCode.names();
		for (int i = 0; i < IcedConstants.CODE_ENUM_COUNT; i++) {
			if (CodeUtils.isIgnored(codeNames[i]))
				continue;
			int code = i;
			OpCodeInfo opCode = Code.toOpCode(code);
			if (hash1632.contains(code)) {
				assertTrue(opCode.getMode16());
				assertTrue(opCode.getMode32());
				assertFalse(opCode.getMode64());
			}
			else if (hash64.contains(code)) {
				assertFalse(opCode.getMode16());
				assertFalse(opCode.getMode32());
				assertTrue(opCode.getMode64());
			}
			else {
				assertTrue(opCode.getMode16());
				assertTrue(opCode.getMode32());
				assertTrue(opCode.getMode64());
			}
		}
	}

	@Test
	void verify_can_only_decode_in_correct_mode() {
		String extraBytes = String.join("", Collections.nCopies(IcedConstants.MAX_INSTRUCTION_LENGTH - 1, "0"));
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			OpCodeInfo opCode = Code.toOpCode(info.code);
			String newHexBytes = info.hexBytes + extraBytes;
			if (!opCode.getMode16()) {
				Decoder decoder = new Decoder(16, new ByteArrayCodeReader(HexUtils.toByteArray(newHexBytes)), info.options);
				Instruction instruction = decoder.decode();
				assertNotEquals(info.code, instruction.getCode());
			}
			if (!opCode.getMode32()) {
				Decoder decoder = new Decoder(32, new ByteArrayCodeReader(HexUtils.toByteArray(newHexBytes)), info.options);
				Instruction instruction = decoder.decode();
				assertNotEquals(info.code, instruction.getCode());
			}
			if (!opCode.getMode64()) {
				Decoder decoder = new Decoder(64, new ByteArrayCodeReader(HexUtils.toByteArray(newHexBytes)), info.options);
				Instruction instruction = decoder.decode();
				assertNotEquals(info.code, instruction.getCode());
			}
		}
	}

	@Test
	void verify_invalid_table_encoding() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX) {
				byte[] hexBytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(hexBytes);
				int maxTable = opCode.getEncoding() == EncodingKind.EVEX ? 8 : 0x10;
				for (int i = 0; i < 8; i++) {
					switch (opCode.getEncoding()) {
					case EncodingKind.EVEX:
						switch (i) {
						case 1:// 0F
						case 2:// 0F 38
						case 3:// 0F 3A
						case 5:// MAP5
						case 6:// MAP6
							continue;
						}
						break;
					case EncodingKind.MVEX:
						switch (i) {
						case 1:// 0F
						case 2:// 0F 38
						case 3:// 0F 3A
							continue;
						}
						break;
					default:
						throw new UnsupportedOperationException();
					}
					hexBytes[evexIndex + 1] = (byte)((hexBytes[evexIndex + 1] & ~(byte)(maxTable - 1)) | i);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options ^ DecoderOptions.NO_INVALID_CHECK);
						Instruction instruction = decoder.decode();
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
				}
			}
			else if (opCode.getEncoding() == EncodingKind.VEX) {
				byte[] hexBytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(hexBytes);
				if ((hexBytes[vexIndex] & 0xFF) == 0xC5)
					continue;
				for (int i = 0; i < 32; i++) {
					switch (i) {
					case 0:// MAP0
					case 1:// 0F
					case 2:// 0F 38
					case 3:// 0F 3A
						continue;
					}
					hexBytes[vexIndex + 1] = (byte)((hexBytes[vexIndex + 1] & 0xE0) | i);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options);
						Instruction instruction = decoder.decode();
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options ^ DecoderOptions.NO_INVALID_CHECK);
						Instruction instruction = decoder.decode();
						assertEquals(Code.INVALID, instruction.getCode());
						assertNotEquals(DecoderError.NONE, decoder.getLastError());
					}
				}
			}
			else if (opCode.getEncoding() == EncodingKind.XOP) {
				byte[] hexBytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(hexBytes);
				for (int i = 0; i < 32; i++) {
					switch (i) {
					case 8:// MAP8
					case 9:// MAP9
					case 10:// MAP10
						continue;
					}
					hexBytes[vexIndex + 1] = (byte)((hexBytes[vexIndex + 1] & 0xE0) | i);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options);
						Instruction instruction = decoder.decode();
						if (i < 8)
							assertNotEquals(info.code, instruction.getCode());
						else {
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
					}
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options ^ DecoderOptions.NO_INVALID_CHECK);
						Instruction instruction = decoder.decode();
						if (i < 8)
							assertNotEquals(info.code, instruction.getCode());
						else {
							assertEquals(Code.INVALID, instruction.getCode());
							assertNotEquals(DecoderError.NONE, decoder.getLastError());
						}
					}
				}
			}
			else if (opCode.getEncoding() == EncodingKind.LEGACY || opCode.getEncoding() == EncodingKind.D3NOW) {
			}
			else
				throw new UnsupportedOperationException();
		}
	}

	@Test
	void verify_invalid_pp_field() {
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX) {
				byte[] hexBytes = HexUtils.toByteArray(info.hexBytes);
				int evexIndex = getEvexIndex(hexBytes);
				byte b = hexBytes[evexIndex + 2];
				for (int i = 1; i <= 3; i++) {
					hexBytes[evexIndex + 2] = (byte)(b ^ i);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options);
						Instruction instruction = decoder.decode();
						assertNotEquals(info.code, instruction.getCode());
					}
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options ^ DecoderOptions.NO_INVALID_CHECK);
						Instruction instruction = decoder.decode();
						assertNotEquals(info.code, instruction.getCode());
					}
				}
			}
			else if (opCode.getEncoding() == EncodingKind.VEX || opCode.getEncoding() == EncodingKind.XOP) {
				byte[] hexBytes = HexUtils.toByteArray(info.hexBytes);
				int vexIndex = getVexXopIndex(hexBytes);
				int ppIndex = (hexBytes[vexIndex] & 0xFF) == 0xC5 ? vexIndex + 1 : vexIndex + 2;
				byte b = hexBytes[ppIndex];
				for (int i = 1; i <= 3; i++) {
					hexBytes[ppIndex] = (byte)(b ^ i);
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options);
						Instruction instruction = decoder.decode();
						assertNotEquals(info.code, instruction.getCode());
					}
					{
						Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(hexBytes), info.options ^ DecoderOptions.NO_INVALID_CHECK);
						Instruction instruction = decoder.decode();
						assertNotEquals(info.code, instruction.getCode());
					}
				}
			}
			else if (opCode.getEncoding() == EncodingKind.LEGACY || opCode.getEncoding() == EncodingKind.D3NOW) {
			}
			else
				throw new UnsupportedOperationException();
		}
	}

	@Test
	void verify_regonly_or_regmemonly_mod_bits() {
		String extraBytes = String.join("", Collections.nCopies(IcedConstants.MAX_INSTRUCTION_LENGTH - 1, "0"));
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			OpCodeInfo opCode = Code.toOpCode(info.code);
			if (!isRegOnlyOrRegMemOnlyModRM(opCode))
				continue;
			// There are a few instructions that ignore the mod bits...
			if (opCode.getIgnoresModBits())
				continue;

			byte[] bytes = HexUtils.toByteArray(info.hexBytes + extraBytes);
			int mIndex;
			if (opCode.getEncoding() == EncodingKind.EVEX || opCode.getEncoding() == EncodingKind.MVEX)
				mIndex = getEvexIndex(bytes) + 5;
			else if (opCode.getEncoding() == EncodingKind.VEX || opCode.getEncoding() == EncodingKind.XOP) {
				int vexIndex = getVexXopIndex(bytes);
				mIndex = (bytes[vexIndex] & 0xFF) == 0xC5 ? vexIndex + 3 : vexIndex + 4;
			}
			else if (opCode.getEncoding() == EncodingKind.LEGACY || opCode.getEncoding() == EncodingKind.D3NOW) {
				SkipPrefixesResult skipResult = skipPrefixes(bytes, info.bitness);
				mIndex = skipResult.index;
				switch (opCode.getTable()) {
				case OpCodeTableKind.NORMAL:
					break;
				case OpCodeTableKind.T0F:
					if (bytes[mIndex++] != 0x0F)
						throw new UnsupportedOperationException();
					break;
				case OpCodeTableKind.T0F38:
					if (bytes[mIndex++] != 0x0F)
						throw new UnsupportedOperationException();
					if (bytes[mIndex++] != 0x38)
						throw new UnsupportedOperationException();
					break;
				case OpCodeTableKind.T0F3A:
					if (bytes[mIndex++] != 0x0F)
						throw new UnsupportedOperationException();
					if (bytes[mIndex++] != 0x3A)
						throw new UnsupportedOperationException();
					break;
				default:
					throw new UnsupportedOperationException();
				}
				mIndex++;
			}
			else
				throw new UnsupportedOperationException();

			if ((bytes[mIndex] & 0xFF) >= 0xC0)
				bytes[mIndex] &= 0x3F;
			else
				bytes[mIndex] |= 0xC0;
			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options);
				Instruction instruction = decoder.decode();
				assertNotEquals(info.code, instruction.getCode());
			}
			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(bytes), info.options ^ DecoderOptions.NO_INVALID_CHECK);
				Instruction instruction = decoder.decode();
				assertNotEquals(info.code, instruction.getCode());
			}
		}
	}

	private static boolean isRegOnlyOrRegMemOnlyModRM(OpCodeInfo opCode) {
		for (int i = 0; i < opCode.getOpCount(); i++) {
			switch (opCode.getOpKind(i)) {
			case OpCodeOperandKind.MEM:
			case OpCodeOperandKind.SIBMEM:
			case OpCodeOperandKind.MEM_MPX:
			case OpCodeOperandKind.MEM_MIB:
			case OpCodeOperandKind.MEM_VSIB32X:
			case OpCodeOperandKind.MEM_VSIB64X:
			case OpCodeOperandKind.MEM_VSIB32Y:
			case OpCodeOperandKind.MEM_VSIB64Y:
			case OpCodeOperandKind.MEM_VSIB32Z:
			case OpCodeOperandKind.MEM_VSIB64Z:
			case OpCodeOperandKind.R16_RM:
			case OpCodeOperandKind.R32_RM:
			case OpCodeOperandKind.R64_RM:
			case OpCodeOperandKind.K_RM:
			case OpCodeOperandKind.MM_RM:
			case OpCodeOperandKind.XMM_RM:
			case OpCodeOperandKind.YMM_RM:
			case OpCodeOperandKind.ZMM_RM:
			case OpCodeOperandKind.TMM_RM:
				return true;
			}
		}
		return false;
	}

	@Test
	void disable_decoder_option_disables_instruction() {
		String extraBytes = String.join("", Collections.nCopies(IcedConstants.MAX_INSTRUCTION_LENGTH - 1, "0"));
		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			if (info.options == DecoderOptions.NONE)
				continue;
			final int NO_OPTIONS =
				DecoderOptions.NO_INVALID_CHECK |
				DecoderOptions.NO_PAUSE |
				DecoderOptions.NO_WBNOINVD |
				DecoderOptions.NO_MPFX_0FBC |
				DecoderOptions.NO_MPFX_0FBD |
				DecoderOptions.NO_LAHF_SAHF_64;
			if ((info.options & NO_OPTIONS) != 0)
				continue;
			if (!isPowerOfTwo(info.options))
				continue;
			if (info.options == DecoderOptions.FORCE_RESERVED_NOP)
				continue;
			if ((info.testOptions & DecoderTestOptions.NO_OPT_DISABLE_TEST) != 0)
				continue;

			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(info.hexBytes)), info.options);
				Instruction instr = decoder.decode();
				assertEquals(info.code, instr.getCode());
			}
			{
				Decoder decoder = new Decoder(info.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(info.hexBytes + extraBytes)), DecoderOptions.NONE);
				Instruction instr = decoder.decode();
				assertNotEquals(info.code, instr.getCode());
			}
		}
	}

	static boolean isPowerOfTwo(int v) {
		return v != 0 && (v & (v - 1)) == 0;
	}
}
