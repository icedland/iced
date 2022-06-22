// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.Iterator;

import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.TupleType;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.MandatoryPrefixByte;
import com.github.icedland.iced.x86.internal.TupleTypeTable;
import com.github.icedland.iced.x86.internal.dec.OpSize;
import com.github.icedland.iced.x86.internal.dec.StateFlags;

/**
 * Decodes 16/32/64-bit x86 instructions
 */
public final class Decoder implements Iterable<Instruction> {
	private long instructionPointer;
	private final CodeReader reader;
	private final RegInfo2[] memRegs16;
	private final OpCodeHandler[] handlers_MAP0;
	private final OpCodeHandler[] handlers_VEX_MAP0;
	private final OpCodeHandler[] handlers_VEX_0F;
	private final OpCodeHandler[] handlers_VEX_0F38;
	private final OpCodeHandler[] handlers_VEX_0F3A;
	private final OpCodeHandler[] handlers_EVEX_0F;
	private final OpCodeHandler[] handlers_EVEX_0F38;
	private final OpCodeHandler[] handlers_EVEX_0F3A;
	private final OpCodeHandler[] handlers_EVEX_MAP5;
	private final OpCodeHandler[] handlers_EVEX_MAP6;
	private final OpCodeHandler[] handlers_XOP_MAP8;
	private final OpCodeHandler[] handlers_XOP_MAP9;
	private final OpCodeHandler[] handlers_XOP_MAP10;
	private final OpCodeHandler[] handlers_MVEX_0F;
	private final OpCodeHandler[] handlers_MVEX_0F38;
	private final OpCodeHandler[] handlers_MVEX_0F3A;
	int state_modrm, state_mod, state_reg, state_rm;
	int state_zs_instructionLength;
	int state_zs_extraRegisterBase; // R << 3
	int state_zs_extraIndexRegisterBase; // X << 3
	int state_zs_extraBaseRegisterBase; // B << 3
	private int state_zs_extraIndexRegisterBaseVSIB;
	int state_zs_flags;
	byte state_zs_mandatoryPrefix;
	// 0=ES/CS/SS/DS, 1=FS/GS
	byte state_zs_segmentPrio;
	int state_vvvv;// V`vvvv. Not stored in inverted form. If 16/32-bit mode, bits [4:3] are cleared
	int state_vvvv_invalidCheck;// vvvv bits, even in 16/32-bit mode.
	int state_aaa;
	int state_extraRegisterBaseEVEX; // EVEX/MVEX.R' << 4
	int state_extraBaseRegisterBaseEVEX; // EVEX/MVEX.XB << 3
	int state_vectorLength;
	byte state_operandSize;
	byte state_addressSize;
	int displIndex;
	final int options;
	final int invalidCheckMask;// All 1s if we should check for invalid instructions, else 0
	final int is64bMode_and_W;// StateFlags.W if 64-bit mode, 0 if 16/32-bit mode
	final int reg15Mask;// 7 in 16/32-bit mode, 15 in 64-bit mode
	private final int maskE0;
	private final int rexMask;
	final byte defaultCodeSize;
	final byte defaultOperandSize;
	private final byte defaultAddressSize;
	final byte defaultInvertedOperandSize;
	final byte defaultInvertedAddressSize;
	final boolean is64bMode;
	private int bitness;

	int getSss() {
		return (state_zs_flags >>> StateFlags.MVEX_SSS_SHIFT) & StateFlags.MVEX_SSS_MASK;
	}

	/**
	 * Current {@code IP}/{@code EIP}/{@code RIP} value.
	 * <p>
	 * Writing to this property only updates the IP value, it does not change a {@link CodeReader}'s byte position.
	 * You can use {@link ByteArrayCodeReader#setPosition(int)} to change its position.
	 */
	public long getIP() {
		return instructionPointer;
	}

	/**
	 * Current {@code IP}/{@code EIP}/{@code RIP} value.
	 * <p>
	 * Writing to this property only updates the IP value, it does not change a {@link CodeReader}'s byte position.
	 * You can use {@link ByteArrayCodeReader#setPosition(int)} to change its position.
	 */
	public void setIP(long value) {
		instructionPointer = value;
	}

	/**
	 * Gets the bitness (16, 32 or 64)
	 */
	public int getBitness() {
		return bitness;
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param reader  Code reader
	 * @param ip      {@code RIP} value
	 */
	public Decoder(int bitness, CodeReader reader, long ip) {
		this(bitness, reader, ip, DecoderOptions.NONE);
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param reader  Code reader
	 * @param ip      {@code RIP} value
	 * @param options Decoder options (a {@link DecoderOptions} flags value)
	 */
	public Decoder(int bitness, CodeReader reader, long ip, int options) {
		if (bitness != 16 && bitness != 32 && bitness != 64)
			throw new IllegalArgumentException("bitness");
		if (reader == null)
			throw new NullPointerException("reader");
		this.reader = reader;
		instructionPointer = ip;
		this.options = options;
		invalidCheckMask = (options & DecoderOptions.NO_INVALID_CHECK) == 0 ? 0xFFFF_FFFF : 0;
		memRegs16 = s_memRegs16;
		this.bitness = bitness;
		if (bitness == 64) {
			is64bMode = true;
			defaultCodeSize = CodeSize.CODE64;
			defaultOperandSize = OpSize.SIZE32;
			defaultInvertedOperandSize = OpSize.SIZE16;
			defaultAddressSize = OpSize.SIZE64;
			defaultInvertedAddressSize = OpSize.SIZE32;
			maskE0 = 0xE0;
			rexMask = 0xF0;
		}
		else if (bitness == 32) {
			is64bMode = false;
			defaultCodeSize = CodeSize.CODE32;
			defaultOperandSize = OpSize.SIZE32;
			defaultInvertedOperandSize = OpSize.SIZE16;
			defaultAddressSize = OpSize.SIZE32;
			defaultInvertedAddressSize = OpSize.SIZE16;
			maskE0 = 0;
			rexMask = 0;
		}
		else {
			assert bitness == 16 : bitness;
			is64bMode = false;
			defaultCodeSize = CodeSize.CODE16;
			defaultOperandSize = OpSize.SIZE16;
			defaultInvertedOperandSize = OpSize.SIZE32;
			defaultAddressSize = OpSize.SIZE16;
			defaultInvertedAddressSize = OpSize.SIZE32;
			maskE0 = 0;
			rexMask = 0;
		}
		is64bMode_and_W = is64bMode ? StateFlags.W : 0;
		reg15Mask = is64bMode ? 0xF : 0x7;
		handlers_MAP0 = OpCodeHandlersTables_Legacy.handlers_MAP0;
		handlers_VEX_MAP0 = OpCodeHandlersTables_VEX.handlers_MAP0;
		handlers_VEX_0F = OpCodeHandlersTables_VEX.handlers_0F;
		handlers_VEX_0F38 = OpCodeHandlersTables_VEX.handlers_0F38;
		handlers_VEX_0F3A = OpCodeHandlersTables_VEX.handlers_0F3A;
		handlers_EVEX_0F = OpCodeHandlersTables_EVEX.handlers_0F;
		handlers_EVEX_0F38 = OpCodeHandlersTables_EVEX.handlers_0F38;
		handlers_EVEX_0F3A = OpCodeHandlersTables_EVEX.handlers_0F3A;
		handlers_EVEX_MAP5 = OpCodeHandlersTables_EVEX.handlers_MAP5;
		handlers_EVEX_MAP6 = OpCodeHandlersTables_EVEX.handlers_MAP6;
		handlers_XOP_MAP8 = OpCodeHandlersTables_XOP.handlers_MAP8;
		handlers_XOP_MAP9 = OpCodeHandlersTables_XOP.handlers_MAP9;
		handlers_XOP_MAP10 = OpCodeHandlersTables_XOP.handlers_MAP10;
		handlers_MVEX_0F = OpCodeHandlersTables_MVEX.handlers_0F;
		handlers_MVEX_0F38 = OpCodeHandlersTables_MVEX.handlers_0F38;
		handlers_MVEX_0F3A = OpCodeHandlersTables_MVEX.handlers_0F3A;
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param data    Data to decode
	 * @param ip      {@code RIP} value
	 */
	public Decoder(int bitness, byte[] data, long ip) {
		this(bitness, data, ip, DecoderOptions.NONE);
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param data    Data to decode
	 * @param ip      {@code RIP} value
	 * @param options Decoder options (a {@link DecoderOptions} flags value)
	 */
	public Decoder(int bitness, byte[] data, long ip, int options) {
		this(bitness, new ByteArrayCodeReader(data), ip, options);
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param reader  Code reader
	 */
	public Decoder(int bitness, CodeReader reader) {
		this(bitness, reader, DecoderOptions.NONE);
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param reader  Code reader
	 * @param options Decoder options (a {@link DecoderOptions} flags value)
	 */
	public Decoder(int bitness, CodeReader reader, int options) {
		this(bitness, reader, 0, options);
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param data    Data to decode
	 */
	public Decoder(int bitness, byte[] data) {
		this(bitness, data, DecoderOptions.NONE);
	}

	/**
	 * Constructor
	 *
	 * @param bitness 16, 32 or 64
	 * @param data    Data to decode
	 * @param options Decoder options (a {@link DecoderOptions} flags value)
	 */
	public Decoder(int bitness, byte[] data, int options) {
		this(bitness, new ByteArrayCodeReader(data), 0, options);
	}

	int readByte() {
		int instrLen = state_zs_instructionLength;
		if (instrLen < IcedConstants.MAX_INSTRUCTION_LENGTH) {
			int b = reader.readByte();
			assert b < 0 || (0 <= b && b <= 0xFF) : b;
			if (Integer.compareUnsigned(b, 0xFF) <= 0) {
				state_zs_instructionLength = instrLen + 1;
				return b;
			}
			state_zs_flags |= StateFlags.NO_MORE_BYTES;
		}
		state_zs_flags |= StateFlags.IS_INVALID;
		return 0;
	}

	int readUInt16() {
		return readByte() | (readByte() << 8);
	}

	int readUInt32() {
		return readByte() | (readByte() << 8) | (readByte() << 16) | (readByte() << 24);
	}

	long readUInt64() {
		return ((long)readUInt32() & 0xFFFF_FFFFL) | ((long)readUInt32() << 32);
	}

	/**
	 * Gets the last decoder error (a {@link DecoderError} enum variant).
	 *
	 * Unless you need to know the reason it failed, it's better to check {@link com.github.icedland.iced.x86.Instruction#isInvalid()}.
	 */
	public int getLastError() {
		// NO_MORE_BYTES error has highest priority
		if ((state_zs_flags & StateFlags.NO_MORE_BYTES) != 0)
			return DecoderError.NO_MORE_BYTES;
		if ((state_zs_flags & StateFlags.IS_INVALID) != 0)
			return DecoderError.INVALID_INSTRUCTION;
		return DecoderError.NONE;
	}

	/**
	 * Decodes the next instruction, see also {@link #decode(Instruction)} which is faster
	 * if you already have an {@link com.github.icedland.iced.x86.Instruction} local, array element or field.
	 * <p>
	 * See also {@link #getLastError()}
	 */
	public Instruction decode() {
		Instruction instr = new Instruction();
		decode(instr);
		return instr;
	}

	/**
	 * Decodes the next instruction, see also {@link #getLastError()}
	 *
	 * @param instruction Updated with the next decoded instruction
	 */
	public void decode(Instruction instruction) {
		instruction.clear();

		state_zs_instructionLength = 0;
		state_zs_extraRegisterBase = 0;
		state_zs_extraIndexRegisterBase = 0;
		state_zs_extraBaseRegisterBase = 0;
		state_zs_extraIndexRegisterBaseVSIB = 0;
		state_zs_flags = 0;
		state_zs_mandatoryPrefix = 0;
		state_zs_segmentPrio = 0;

		state_operandSize = defaultOperandSize;
		state_addressSize = defaultAddressSize;
		int b = readByte();
		if ((b & rexMask) == 0x40) {
			int flags2 = state_zs_flags | StateFlags.HAS_REX;
			if ((b & 8) != 0) {
				flags2 |= StateFlags.W;
				state_operandSize = OpSize.SIZE64;
			}
			state_zs_flags = flags2;
			state_zs_extraRegisterBase = (b << 1) & 8;
			state_zs_extraIndexRegisterBase = (b << 2) & 8;
			state_zs_extraBaseRegisterBase = (b << 3) & 8;

			b = readByte();
		}
		decodeTable(handlers_MAP0[b], instruction);

		instruction.setCodeSize(defaultCodeSize);
		int instrLen = state_zs_instructionLength;
		assert 0 <= instrLen && instrLen <= IcedConstants.MAX_INSTRUCTION_LENGTH : instrLen;// Could be 0 if there were no bytes available
		instruction.setLength(instrLen);
		long ip = instructionPointer;
		ip += instrLen;
		instructionPointer = ip;
		instruction.setNextIP(ip);

		int flags = state_zs_flags;
		if ((flags & (StateFlags.IS_INVALID | StateFlags.LOCK | StateFlags.IP_REL32 | StateFlags.IP_REL64)) != 0) {
			long addr = instruction.getMemoryDisplacement64() + ip;
			instruction.setMemoryDisplacement64(addr);
			// RIP rel ops are common, but invalid/lock bits are usually never set, so exit early if possible
			if ((flags & (StateFlags.IS_INVALID | StateFlags.LOCK | StateFlags.IP_REL64)) == StateFlags.IP_REL64)
				return;
			if ((flags & StateFlags.IP_REL64) == 0) {
				// Undo what we did above
				instruction.setMemoryDisplacement64(addr - ip);
			}
			if ((flags & StateFlags.IP_REL32) != 0)
				instruction.setMemoryDisplacement64((long)((int)instruction.getMemoryDisplacement64() + (int)ip) & 0xFFFF_FFFFL);

			if ((flags & StateFlags.IS_INVALID) != 0 ||
					(((flags & (StateFlags.LOCK | StateFlags.ALLOW_LOCK)) & invalidCheckMask) == StateFlags.LOCK)) {
				instruction.clear();
				state_zs_flags = flags | StateFlags.IS_INVALID;

				instruction.setCodeSize(defaultCodeSize);
				instruction.setLength(instrLen);
				instruction.setNextIP(ip);
			}
		}
	}

	void resetRexPrefixState() {
		state_zs_flags &= ~(StateFlags.HAS_REX | StateFlags.W);
		if ((state_zs_flags & StateFlags.HAS66) == 0)
			state_operandSize = defaultOperandSize;
		else
			state_operandSize = defaultInvertedOperandSize;
		state_zs_extraRegisterBase = 0;
		state_zs_extraIndexRegisterBase = 0;
		state_zs_extraBaseRegisterBase = 0;
	}

	void callOpCodeHandlerXXTable(Instruction instruction) {
		int b = readByte();
		decodeTable(handlers_MAP0[b], instruction);
	}

	int getCurrentInstructionPointer32() {
		return (int)instructionPointer + state_zs_instructionLength;
	}

	long getCurrentInstructionPointer64() {
		return instructionPointer + state_zs_instructionLength;
	}

	void clearMandatoryPrefix(Instruction instruction) {
		instruction.setRepePrefix(false);
		instruction.setRepnePrefix(false);
	}

	void setXacquireXrelease(Instruction instruction) {
		if (instruction.getLockPrefix()) {
			if (state_zs_mandatoryPrefix == MandatoryPrefixByte.PF2) {
				clearMandatoryPrefixF2(instruction);
				instruction.setXacquirePrefix(true);
			}
			else if (state_zs_mandatoryPrefix == MandatoryPrefixByte.PF3) {
				clearMandatoryPrefixF3(instruction);
				instruction.setXreleasePrefix(true);
			}
		}
	}

	void clearMandatoryPrefixF3(Instruction instruction) {
		assert state_zs_mandatoryPrefix == MandatoryPrefixByte.PF3 : state_zs_mandatoryPrefix;
		instruction.setRepePrefix(false);
	}

	void clearMandatoryPrefixF2(Instruction instruction) {
		assert state_zs_mandatoryPrefix == MandatoryPrefixByte.PF2 : state_zs_mandatoryPrefix;
		instruction.setRepnePrefix(false);
	}

	void setInvalidInstruction() {
		state_zs_flags |= StateFlags.IS_INVALID;
	}

	void decodeTable(OpCodeHandler[] table, Instruction instruction) {
		decodeTable(table[readByte()], instruction);
	}

	private void decodeTable(OpCodeHandler handler, Instruction instruction) {
		if (handler.hasModRM) {
			int m = readByte();
			state_modrm = m;
			state_mod = m >>> 6;
			state_reg = (m >>> 3) & 7;
			state_rm = m & 7;
		}
		handler.decode(this, instruction);
	}

	void readModRM() {
		int m = readByte();
		state_modrm = m;
		state_mod = m >>> 6;
		state_reg = (m >>> 3) & 7;
		state_rm = m & 7;
	}

	void vex2(Instruction instruction) {
		if ((((state_zs_flags & StateFlags.HAS_REX) | state_zs_mandatoryPrefix) & invalidCheckMask) != 0)
			setInvalidInstruction();
		// Undo what Decode() did if it got a REX prefix
		state_zs_flags &= ~StateFlags.W;
		state_zs_extraIndexRegisterBase = 0;
		state_zs_extraBaseRegisterBase = 0;

		int b = state_modrm;

		state_vectorLength = (b >>> 2) & 1;

		state_zs_mandatoryPrefix = (byte)(b & 3);

		b = ~b;
		state_zs_extraRegisterBase = (b >>> 4) & 8;

		// Bit 6 can only be 1 if it's 16/32-bit mode, so we don't need to change the mask
		b = (b >>> 3) & 0x0F;
		state_vvvv = b;
		state_vvvv_invalidCheck = b;

		decodeTable(handlers_VEX_0F, instruction);
	}

	void vex3(Instruction instruction) {
		if ((((state_zs_flags & StateFlags.HAS_REX) | state_zs_mandatoryPrefix) & invalidCheckMask) != 0)
			setInvalidInstruction();
		// Undo what Decode() did if it got a REX prefix
		state_zs_flags &= ~StateFlags.W;

		int b2 = readByte();

		state_zs_flags |= b2 & 0x80;

		state_vectorLength = (b2 >>> 2) & 1;

		state_zs_mandatoryPrefix = (byte)(b2 & 3);

		b2 = (~b2 >>> 3) & 0x0F;
		state_vvvv_invalidCheck = b2;
		state_vvvv = b2 & reg15Mask;
		int b1 = state_modrm;
		int b1x = ~b1 & maskE0;
		state_zs_extraRegisterBase = (b1x >>> 4) & 8;
		state_zs_extraIndexRegisterBase = (b1x >>> 3) & 8;
		state_zs_extraBaseRegisterBase = (b1x >>> 2) & 8;

		OpCodeHandler[] handlers;
		int b = readByte();
		int table = b1 & 0x1F;
		if (table == 1)
			handlers = handlers_VEX_0F;
		else if (table == 2)
			handlers = handlers_VEX_0F38;
		else if (table == 3)
			handlers = handlers_VEX_0F3A;
		else if (table == 0)
			handlers = handlers_VEX_MAP0;
		else {
			setInvalidInstruction();
			return;
		}
		decodeTable(handlers[b], instruction);
	}

	void xop(Instruction instruction) {
		if ((((state_zs_flags & StateFlags.HAS_REX) | state_zs_mandatoryPrefix) & invalidCheckMask) != 0)
			setInvalidInstruction();
		// Undo what Decode() did if it got a REX prefix
		state_zs_flags &= ~StateFlags.W;

		int b2 = readByte();

		state_zs_flags |= b2 & 0x80;

		state_vectorLength = (b2 >>> 2) & 1;

		state_zs_mandatoryPrefix = (byte)(b2 & 3);

		b2 = (~b2 >>> 3) & 0x0F;
		state_vvvv_invalidCheck = b2;
		state_vvvv = b2 & reg15Mask;
		int b1 = state_modrm;
		int b1x = ~b1 & maskE0;
		state_zs_extraRegisterBase = (b1x >>> 4) & 8;
		state_zs_extraIndexRegisterBase = (b1x >>> 3) & 8;
		state_zs_extraBaseRegisterBase = (b1x >>> 2) & 8;

		OpCodeHandler[] handlers;
		int b = readByte();
		int table = b1 & 0x1F;
		if (table == 8)
			handlers = handlers_XOP_MAP8;
		else if (table == 9)
			handlers = handlers_XOP_MAP9;
		else if (table == 10)
			handlers = handlers_XOP_MAP10;
		else {
			setInvalidInstruction();
			return;
		}
		decodeTable(handlers[b], instruction);
	}

	void evex_mvex(Instruction instruction) {
		if ((((state_zs_flags & StateFlags.HAS_REX) | state_zs_mandatoryPrefix) & invalidCheckMask) != 0)
			setInvalidInstruction();
		// Undo what Decode() did if it got a REX prefix
		state_zs_flags &= ~StateFlags.W;

		int p0 = state_modrm;
		int p1 = readByte();
		int p2 = readByte();
		int p3 = readByte();
		int p4 = readByte();

		if ((p1 & 4) != 0) {
			if ((p0 & 8) == 0) {
				state_zs_mandatoryPrefix = (byte)(p1 & 3);

				state_zs_flags |= p1 & 0x80;

				int aaa = p2 & 7;
				state_aaa = aaa;
				instruction.setRawOpMask(aaa);
				if ((p2 & 0x80) != 0) {
					// invalid if aaa == 0 and if we check for invalid instructions (it's all 1s)
					if ((aaa ^ invalidCheckMask) == 0xFFFF_FFFF)
						setInvalidInstruction();
					state_zs_flags |= StateFlags.Z;
					instruction.setZeroingMasking(true);
				}

				state_zs_flags |= p2 & 0x10;

				state_vectorLength = (p2 >>> 5) & 3;

				p1 = (~p1 >>> 3) & 0x0F;
				if (is64bMode) {
					int tmp = (~p2 & 8) << 1;
					state_zs_extraIndexRegisterBaseVSIB = tmp;
					tmp += p1;
					state_vvvv = tmp;
					state_vvvv_invalidCheck = tmp;
					int p0x = ~p0;
					state_zs_extraRegisterBase = (p0x >>> 4) & 8;
					state_zs_extraIndexRegisterBase = (p0x >>> 3) & 8;
					state_extraRegisterBaseEVEX = p0x & 0x10;
					p0x >>>= 2;
					state_extraBaseRegisterBaseEVEX = p0x & 0x18;
					state_zs_extraBaseRegisterBase = p0x & 8;
				}
				else {
					state_vvvv_invalidCheck = p1;
					state_vvvv = p1 & 0x07;
					state_zs_flags |= (~p2 & 8) << 3;
				}

				OpCodeHandler[] handlers;
				switch (p0 & 7) {
				case 1:
					handlers = handlers_EVEX_0F;
					break;
				case 2:
					handlers = handlers_EVEX_0F38;
					break;
				case 3:
					handlers = handlers_EVEX_0F3A;
					break;
				case 5:
					handlers = handlers_EVEX_MAP5;
					break;
				case 6:
					handlers = handlers_EVEX_MAP6;
					break;
				default:
					setInvalidInstruction();
					return;
				}
				OpCodeHandler handler = handlers[p3];
				assert handler.hasModRM;
				state_modrm = p4;
				state_mod = p4 >>> 6;
				state_reg = (p4 >>> 3) & 7;
				state_rm = p4 & 7;
				// Invalid if LL=3 and no rc
				if ((((state_zs_flags & StateFlags.B) | state_vectorLength) & invalidCheckMask) == 3)
					setInvalidInstruction();
				handler.decode(this, instruction);
			}
			else
				setInvalidInstruction();
		}
		else {
			if ((options & DecoderOptions.KNC) == 0 || !is64bMode)
				setInvalidInstruction();
			else {
				state_zs_mandatoryPrefix = (byte)(p1 & 3);

				state_zs_flags |= p1 & 0x80;

				int aaa = p2 & 7;
				state_aaa = aaa;
				instruction.setRawOpMask(aaa);

				state_zs_flags |= (p2 & 0xF0) << (StateFlags.MVEX_SSS_SHIFT - 4);

				p1 = (~p1 >>> 3) & 0x0F;
				int tmp = (~p2 & 8) << 1;
				state_zs_extraIndexRegisterBaseVSIB = tmp;
				tmp += p1;
				state_vvvv = tmp;
				state_vvvv_invalidCheck = tmp;
				int p0x = ~p0;
				state_zs_extraRegisterBase = (p0x >>> 4) & 8;
				state_zs_extraIndexRegisterBase = (p0x >>> 3) & 8;
				state_extraRegisterBaseEVEX = p0x & 0x10;
				p0x >>>= 2;
				state_extraBaseRegisterBaseEVEX = p0x & 0x18;
				state_zs_extraBaseRegisterBase = p0x & 8;

				OpCodeHandler[] handlers;
				switch (p0 & 0xF) {
				case 1:
					handlers = handlers_MVEX_0F;
					break;
				case 2:
					handlers = handlers_MVEX_0F38;
					break;
				case 3:
					handlers = handlers_MVEX_0F3A;
					break;
				default:
					setInvalidInstruction();
					return;
				}
				OpCodeHandler handler = handlers[p3];
				assert handler.hasModRM;
				state_modrm = p4;
				state_mod = p4 >>> 6;
				state_reg = (p4 >>> 3) & 7;
				state_rm = p4 & 7;
				handler.decode(this, instruction);
			}
		}
	}

	int readOpSegReg() {
		int reg = state_reg;
		if (reg < 6)
			return Register.ES + reg;
		setInvalidInstruction();
		return Register.NONE;
	}

	boolean readOpMem(Instruction instruction) {
		if (state_addressSize == OpSize.SIZE64)
			return readOpMem32Or64(instruction, Register.RAX, Register.RAX, TupleType.N1, false);
		else if (state_addressSize == OpSize.SIZE32)
			return readOpMem32Or64(instruction, Register.EAX, Register.EAX, TupleType.N1, false);
		else {
			readOpMem16(instruction, TupleType.N1);
			return false;
		}
	}

	void readOpMemSib(Instruction instruction) {
		boolean isValid;
		if (state_addressSize == OpSize.SIZE64)
			isValid = readOpMem32Or64(instruction, Register.RAX, Register.RAX, TupleType.N1, false);
		else if (state_addressSize == OpSize.SIZE32)
			isValid = readOpMem32Or64(instruction, Register.EAX, Register.EAX, TupleType.N1, false);
		else {
			readOpMem16(instruction, TupleType.N1);
			isValid = false;
		}
		if (invalidCheckMask != 0 && !isValid)
			setInvalidInstruction();
	}

	// All MPX instructions in 64-bit mode force 64-bit addressing, and
	// all MPX instructions in 16/32-bit mode require 32-bit addressing
	// (see SDM Vol 1, 17.5.1 Intel MPX and Operating Modes)
	void readOpMem_MPX(Instruction instruction) {
		if (is64bMode) {
			state_addressSize = OpSize.SIZE64;
			readOpMem32Or64(instruction, Register.RAX, Register.RAX, TupleType.N1, false);
		}
		else if (state_addressSize == OpSize.SIZE32)
			readOpMem32Or64(instruction, Register.EAX, Register.EAX, TupleType.N1, false);
		else {
			readOpMem16(instruction, TupleType.N1);
			if (invalidCheckMask != 0)
				setInvalidInstruction();
		}
	}

	void readOpMem(Instruction instruction, int tupleType) {
		if (state_addressSize == OpSize.SIZE64)
			readOpMem32Or64(instruction, Register.RAX, Register.RAX, tupleType, false);
		else if (state_addressSize == OpSize.SIZE32)
			readOpMem32Or64(instruction, Register.EAX, Register.EAX, tupleType, false);
		else
			readOpMem16(instruction, tupleType);
	}

	void readOpMem_VSIB(Instruction instruction, int vsibIndex, int tupleType) {
		boolean isValid;
		if (state_addressSize == OpSize.SIZE64)
			isValid = readOpMem32Or64(instruction, Register.RAX, vsibIndex, tupleType, true);
		else if (state_addressSize == OpSize.SIZE32)
			isValid = readOpMem32Or64(instruction, Register.EAX, vsibIndex, tupleType, true);
		else {
			readOpMem16(instruction, tupleType);
			isValid = false;
		}
		if (invalidCheckMask != 0 && !isValid)
			setInvalidInstruction();
	}

	private static final class RegInfo2 {
		final int baseReg;
		final int indexReg;

		RegInfo2(int baseReg, int indexReg) {
			this.baseReg = baseReg;
			this.indexReg = indexReg;
		}
	}

	private static final RegInfo2[] s_memRegs16 = new RegInfo2[] {
		new RegInfo2(Register.BX, Register.SI),
		new RegInfo2(Register.BX, Register.DI),
		new RegInfo2(Register.BP, Register.SI),
		new RegInfo2(Register.BP, Register.DI),
		new RegInfo2(Register.SI, Register.NONE),
		new RegInfo2(Register.DI, Register.NONE),
		new RegInfo2(Register.BP, Register.NONE),
		new RegInfo2(Register.BX, Register.NONE),
	};

	private void readOpMem16(Instruction instruction, int tupleType) {
		assert state_addressSize == OpSize.SIZE16 : state_addressSize;
		RegInfo2 info = memRegs16[state_rm];
		int baseReg = info.baseReg;
		int indexReg = info.indexReg;
		switch (state_mod) {
		case 0:
			if (state_rm == 6) {
				instruction.setMemoryDisplSize(2);
				displIndex = state_zs_instructionLength;
				instruction.setMemoryDisplacement64(readUInt16());
				baseReg = Register.NONE;
				assert indexReg == Register.NONE : indexReg;
			}
			break;

		case 1:
			instruction.setMemoryDisplSize(1);
			displIndex = state_zs_instructionLength;
			if (tupleType == TupleType.N1)
				instruction.setMemoryDisplacement64((byte)readByte() & 0xFFFF);
			else
				instruction.setMemoryDisplacement64((getDisp8N(tupleType) * (byte)readByte()) & 0xFFFF);
			break;

		default:
			assert state_mod == 2 : state_mod;
			instruction.setMemoryDisplSize(2);
			displIndex = state_zs_instructionLength;
			instruction.setMemoryDisplacement64(readUInt16());
			break;
		}

		instruction.setMemoryBase(baseReg);
		instruction.setMemoryIndex(indexReg);
	}

	// Returns true if the SIB byte was read
	private boolean readOpMem32Or64(Instruction instruction, int baseReg, int indexReg, int tupleType, boolean isVsib) {
		assert state_addressSize == OpSize.SIZE32 || state_addressSize == OpSize.SIZE64 : state_addressSize;
		int sib;
		int displSizeScale, displ;
		switch (state_mod) {
		case 0:
			if (state_rm == 4) {
				sib = readByte();
				displSizeScale = 0;
				displ = 0;
				break;
			}
			else if (state_rm == 5) {
				displIndex = state_zs_instructionLength;
				if (state_addressSize == OpSize.SIZE64) {
					instruction.setMemoryDisplacement64(readUInt32());
					instruction.setMemoryDisplSize(8);
				}
				else {
					instruction.setMemoryDisplacement64((long)readUInt32() & 0xFFFF_FFFFL);
					instruction.setMemoryDisplSize(4);
				}
				if (is64bMode) {
					if (state_addressSize == OpSize.SIZE64) {
						state_zs_flags |= StateFlags.IP_REL64;
						instruction.setMemoryBase(Register.RIP);
					}
					else {
						state_zs_flags |= StateFlags.IP_REL32;
						instruction.setMemoryBase(Register.EIP);
					}
				}
				return false;
			}
			else {
				assert 0 <= state_rm && state_rm <= 7 && state_rm != 4 && state_rm != 5 : state_rm;
				instruction.setMemoryBase(state_zs_extraBaseRegisterBase + state_rm + baseReg);
				return false;
			}

		case 1:
			if (state_rm == 4) {
				sib = readByte();
				displSizeScale = 1;
				displIndex = state_zs_instructionLength;
				if (tupleType == TupleType.N1)
					displ = (byte)readByte();
				else
					displ = getDisp8N(tupleType) * (byte)readByte();
				break;
			}
			else {
				assert 0 <= state_rm && state_rm <= 7 && state_rm != 4 : state_rm;
				instruction.setMemoryDisplSize(1);
				displIndex = state_zs_instructionLength;
				if (state_addressSize == OpSize.SIZE64) {
					if (tupleType == TupleType.N1)
						instruction.setMemoryDisplacement64((byte)readByte());
					else
						instruction.setMemoryDisplacement64((long)getDisp8N(tupleType) * (long)(byte)readByte());
				}
				else {
					if (tupleType == TupleType.N1)
						instruction.setMemoryDisplacement64((long)(byte)readByte() & 0xFFFF_FFFFL);
					else
						instruction.setMemoryDisplacement64((long)(getDisp8N(tupleType) * (byte)readByte()) & 0xFFFF_FFFFL);
				}
				instruction.setMemoryBase(state_zs_extraBaseRegisterBase + state_rm + baseReg);
				return false;
			}

		default:
			assert state_mod == 2 : state_mod;
			if (state_rm == 4) {
				sib = readByte();
				displSizeScale = state_addressSize == OpSize.SIZE64 ? 8 : 4;
				displIndex = state_zs_instructionLength;
				displ = readUInt32();
				break;
			}
			else {
				assert 0 <= state_rm && state_rm <= 7 && state_rm != 4 : state_rm;
				displIndex = state_zs_instructionLength;
				if (state_addressSize == OpSize.SIZE64) {
					instruction.setMemoryDisplacement64(readUInt32());
					instruction.setMemoryDisplSize(8);
				}
				else {
					instruction.setMemoryDisplacement64((long)readUInt32() & 0xFFFF_FFFFL);
					instruction.setMemoryDisplSize(4);
				}
				instruction.setMemoryBase(state_zs_extraBaseRegisterBase + state_rm + baseReg);
				return false;
			}
		}

		int index = ((sib >>> 3) & 7) + state_zs_extraIndexRegisterBase;
		int base = sib & 7;

		instruction.setRawMemoryIndexScale(sib >>> 6);
		if (!isVsib) {
			if (index != 4)
				instruction.setMemoryIndex(index + indexReg);
		}
		else
			instruction.setMemoryIndex(index + state_zs_extraIndexRegisterBaseVSIB + indexReg);

		if (base == 5 && state_mod == 0) {
			displIndex = state_zs_instructionLength;
			if (state_addressSize == OpSize.SIZE64) {
				instruction.setMemoryDisplacement64(readUInt32());
				instruction.setMemoryDisplSize(8);
			}
			else {
				instruction.setMemoryDisplacement64((long)readUInt32() & 0xFFFF_FFFFL);
				instruction.setMemoryDisplSize(4);
			}
		}
		else {
			instruction.setMemoryBase(base + state_zs_extraBaseRegisterBase + baseReg);
			instruction.setMemoryDisplSize(displSizeScale);
			if (state_addressSize == OpSize.SIZE64)
				instruction.setMemoryDisplacement64(displ);
			else
				instruction.setMemoryDisplacement64((long)displ & 0xFFFF_FFFFL);
		}
		return true;
	}

	private int getDisp8N(int tupleType) {
		return TupleTypeTable.getDisp8N(tupleType, (state_zs_flags & StateFlags.B) != 0);
	}

	/**
	 * Gets the offsets of the constants (memory displacement and immediate) in the decoded instruction.
	 * The caller can check if there are any relocations at those addresses.
	 *
	 * @param instruction The latest instruction that was decoded by this decoder
	 */
	public ConstantOffsets getConstantOffsets(Instruction instruction) {
		ConstantOffsets constantOffsets = new ConstantOffsets();

		int displSize = instruction.getMemoryDisplSize();
		if (displSize != 0) {
			constantOffsets.displacementOffset = (byte)displIndex;
			if (displSize == 8 && (state_zs_flags & StateFlags.ADDR64) == 0)
				constantOffsets.displacementSize = 4;
			else
				constantOffsets.displacementSize = (byte)displSize;
		}

		if ((state_zs_flags & StateFlags.NO_IMM) == 0) {
			int extraImmSub = 0;
			for_loop: for (int i = instruction.getOpCount() - 1; i >= 0; i--) {
				switch (instruction.getOpKind(i)) {
				case OpKind.IMMEDIATE8:
				case OpKind.IMMEDIATE8TO16:
				case OpKind.IMMEDIATE8TO32:
				case OpKind.IMMEDIATE8TO64:
					constantOffsets.immediateOffset = (byte)(instruction.getLength() - extraImmSub - 1);
					constantOffsets.immediateSize = 1;
					break for_loop;

				case OpKind.IMMEDIATE16:
					constantOffsets.immediateOffset = (byte)(instruction.getLength() - extraImmSub - 2);
					constantOffsets.immediateSize = 2;
					break for_loop;

				case OpKind.IMMEDIATE32:
				case OpKind.IMMEDIATE32TO64:
					constantOffsets.immediateOffset = (byte)(instruction.getLength() - extraImmSub - 4);
					constantOffsets.immediateSize = 4;
					break for_loop;

				case OpKind.IMMEDIATE64:
					constantOffsets.immediateOffset = (byte)(instruction.getLength() - extraImmSub - 8);
					constantOffsets.immediateSize = 8;
					break for_loop;

				case OpKind.IMMEDIATE8_2ND:
					constantOffsets.immediateOffset2 = (byte)(instruction.getLength() - 1);
					constantOffsets.immediateSize2 = 1;
					extraImmSub = 1;
					break;

				case OpKind.NEAR_BRANCH16:
					if ((state_zs_flags & StateFlags.BRANCH_IMM8) != 0) {
						constantOffsets.immediateOffset = (byte)(instruction.getLength() - 1);
						constantOffsets.immediateSize = 1;
					}
					else if ((state_zs_flags & StateFlags.XBEGIN) == 0) {
						constantOffsets.immediateOffset = (byte)(instruction.getLength() - 2);
						constantOffsets.immediateSize = 2;
					}
					else {
						assert (state_zs_flags & StateFlags.XBEGIN) != 0;
						if (state_operandSize != OpSize.SIZE16) {
							constantOffsets.immediateOffset = (byte)(instruction.getLength() - 4);
							constantOffsets.immediateSize = 4;
						}
						else {
							constantOffsets.immediateOffset = (byte)(instruction.getLength() - 2);
							constantOffsets.immediateSize = 2;
						}
					}
					break;

				case OpKind.NEAR_BRANCH32:
				case OpKind.NEAR_BRANCH64:
					if ((state_zs_flags & StateFlags.BRANCH_IMM8) != 0) {
						constantOffsets.immediateOffset = (byte)(instruction.getLength() - 1);
						constantOffsets.immediateSize = 1;
					}
					else if ((state_zs_flags & StateFlags.XBEGIN) == 0) {
						constantOffsets.immediateOffset = (byte)(instruction.getLength() - 4);
						constantOffsets.immediateSize = 4;
					}
					else {
						assert (state_zs_flags & StateFlags.XBEGIN) != 0;
						if (state_operandSize != OpSize.SIZE16) {
							constantOffsets.immediateOffset = (byte)(instruction.getLength() - 4);
							constantOffsets.immediateSize = 4;
						}
						else {
							constantOffsets.immediateOffset = (byte)(instruction.getLength() - 2);
							constantOffsets.immediateSize = 2;
						}
					}
					break;

				case OpKind.FAR_BRANCH16:
					constantOffsets.immediateOffset = (byte)(instruction.getLength() - (2 + 2));
					constantOffsets.immediateSize = 2;
					constantOffsets.immediateOffset2 = (byte)(instruction.getLength() - 2);
					constantOffsets.immediateSize2 = 2;
					break;

				case OpKind.FAR_BRANCH32:
					constantOffsets.immediateOffset = (byte)(instruction.getLength() - (4 + 2));
					constantOffsets.immediateSize = 4;
					constantOffsets.immediateOffset2 = (byte)(instruction.getLength() - 2);
					constantOffsets.immediateSize2 = 2;
					break;
				}
			}
		}

		return constantOffsets;
	}

	/**
	 * Instruction iterator
	 */
	public final class InstructionIterator implements Iterator<Instruction> {
		private Instruction nextInstruction;

		InstructionIterator() {
			nextInstruction = decode();
		}

		@Override
		public boolean hasNext() {
			return nextInstruction.getLength() != 0;
		}

		@Override
		public Instruction next() {
			Instruction result = nextInstruction;
			nextInstruction = decode();
			return result;
		}
	}

	/**
	 * Gets an iterator that decodes the remaining instructions. This iterator allocates a new
	 * instruction in every iteration so should be avoided if possible.
	 */
	public InstructionIterator iterator() {
		return new InstructionIterator();
	}
}
