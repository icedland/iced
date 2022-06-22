// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MvexEHBit;
import com.github.icedland.iced.x86.MvexRegMemConv;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.internal.MandatoryPrefixByte;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.MvexTupleTypeLut;
import com.github.icedland.iced.x86.internal.TupleTypeTable;
import com.github.icedland.iced.x86.internal.enc.EncFlags1;
import com.github.icedland.iced.x86.internal.enc.EncFlags2;
import com.github.icedland.iced.x86.internal.enc.EncFlags3;
import com.github.icedland.iced.x86.internal.enc.EncoderFlags;
import com.github.icedland.iced.x86.internal.enc.ImmSize;
import com.github.icedland.iced.x86.internal.enc.LBit;
import com.github.icedland.iced.x86.internal.enc.LegacyOpCodeTable;
import com.github.icedland.iced.x86.internal.enc.VexOpCodeTable;
import com.github.icedland.iced.x86.internal.enc.WBit;

/** DO NOT USE: INTERNAL API */
public final class InternalOpCodeHandlers {
	private InternalOpCodeHandlers() {
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class InvalidHandler extends OpCodeHandler {
		/** DO NOT USE: INTERNAL API */
		public static final String ERROR_MESSAGE = "Can't encode an invalid instruction";

		/** DO NOT USE: INTERNAL API */
		public InvalidHandler() {
			super(EncFlags2.NONE, EncFlags3.BIT16OR32 | EncFlags3.BIT64, false, null, new Op[0]);
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			encoder.setErrorMessage(ERROR_MESSAGE);
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class DeclareDataHandler extends OpCodeHandler {
		final int elemLength;
		final int maxLength;

		/** DO NOT USE: INTERNAL API */
		public DeclareDataHandler(int code) {
			super(EncFlags2.NONE, EncFlags3.BIT16OR32 | EncFlags3.BIT64, true, null, new Op[0]);
			switch (code) {
			case Code.DECLAREBYTE:
				elemLength = 1;
				break;
			case Code.DECLAREWORD:
				elemLength = 2;
				break;
			case Code.DECLAREDWORD:
				elemLength = 4;
				break;
			case Code.DECLAREQWORD:
				elemLength = 8;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			maxLength = 16 / elemLength;
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			int declDataCount = instruction.getDeclareDataCount();
			if (declDataCount < 1 || declDataCount > maxLength) {
				encoder.setErrorMessage(String.format("Invalid db/dw/dd/dq data count. Count = %d, max count = %d", declDataCount, maxLength));
				return;
			}
			int length = declDataCount * elemLength;
			for (int i = 0; i < length; i++)
				encoder.writeByteInternal(instruction.getDeclareByteValue(i));
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class ZeroBytesHandler extends OpCodeHandler {
		/** DO NOT USE: INTERNAL API */
		public ZeroBytesHandler(int code) {
			super(EncFlags2.NONE, EncFlags3.BIT16OR32 | EncFlags3.BIT64, true, null, new Op[0]);
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class LegacyHandler extends OpCodeHandler {
		private final int tableByte1, tableByte2;
		private final int mandatoryPrefix;

		private static Op[] createOps(int encFlags1) {
			int op0 = (encFlags1 >>> EncFlags1.LEGACY_OP0_SHIFT) & EncFlags1.LEGACY_OP_MASK;
			int op1 = (encFlags1 >>> EncFlags1.LEGACY_OP1_SHIFT) & EncFlags1.LEGACY_OP_MASK;
			int op2 = (encFlags1 >>> EncFlags1.LEGACY_OP2_SHIFT) & EncFlags1.LEGACY_OP_MASK;
			int op3 = (encFlags1 >>> EncFlags1.LEGACY_OP3_SHIFT) & EncFlags1.LEGACY_OP_MASK;
			if (op3 != 0) {
				assert op0 != 0 && op1 != 0 && op2 != 0;
				return new Op[] { OpTables.legacyOps[op0 - 1], OpTables.legacyOps[op1 - 1], OpTables.legacyOps[op2 - 1],
						OpTables.legacyOps[op3 - 1] };
			}
			if (op2 != 0) {
				assert op0 != 0 && op1 != 0;
				return new Op[] { OpTables.legacyOps[op0 - 1], OpTables.legacyOps[op1 - 1], OpTables.legacyOps[op2 - 1] };
			}
			if (op1 != 0) {
				assert op0 != 0;
				return new Op[] { OpTables.legacyOps[op0 - 1], OpTables.legacyOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpTables.legacyOps[op0 - 1] };
			return new Op[0];
		}

		/** DO NOT USE: INTERNAL API */
		public LegacyHandler(int encFlags1, int encFlags2, int encFlags3) {
			super(encFlags2, encFlags3, false, null, createOps(encFlags1));
			switch ((encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK) {
			case LegacyOpCodeTable.MAP0:
				tableByte1 = 0;
				tableByte2 = 0;
				break;

			case LegacyOpCodeTable.MAP0F:
				tableByte1 = 0x0F;
				tableByte2 = 0;
				break;

			case LegacyOpCodeTable.MAP0F38:
				tableByte1 = 0x0F;
				tableByte2 = 0x38;
				break;

			case LegacyOpCodeTable.MAP0F3A:
				tableByte1 = 0x0F;
				tableByte2 = 0x3A;
				break;

			default:
				throw new UnsupportedOperationException();
			}

			switch ((encFlags2 >>> EncFlags2.MANDATORY_PREFIX_SHIFT) & EncFlags2.MANDATORY_PREFIX_MASK) {
			case MandatoryPrefixByte.NONE:
				mandatoryPrefix = 0x00;
				break;
			case MandatoryPrefixByte.P66:
				mandatoryPrefix = 0x66;
				break;
			case MandatoryPrefixByte.PF3:
				mandatoryPrefix = 0xF3;
				break;
			case MandatoryPrefixByte.PF2:
				mandatoryPrefix = 0xF2;
				break;
			default:
				throw new UnsupportedOperationException();
			}
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			int b = mandatoryPrefix;
			encoder.writePrefixes(instruction, b != 0xF3);
			if (b != 0)
				encoder.writeByteInternal(b);

			b = encoder.encoderFlags;
			b &= 0x4F;
			if (b != 0) {
				if ((encoder.encoderFlags & EncoderFlags.HIGH_LEGACY_8_BIT_REGS) != 0)
					encoder.setErrorMessage(
							"Registers AH, CH, DH, BH can't be used if there's a REX prefix. Use AL, CL, DL, BL, SPL, BPL, SIL, DIL, R8L-R15L instead.");
				b |= 0x40;
				encoder.writeByteInternal(b);
			}

			if ((b = tableByte1) != 0) {
				encoder.writeByteInternal(b);
				if ((b = tableByte2) != 0)
					encoder.writeByteInternal(b);
			}
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class VexHandler extends OpCodeHandler {
		private final int table;
		private final int lastByte;
		private final int mask_W_L;
		private final int mask_L;
		private final int W1;

		private static Op[] createOps(int encFlags1) {
			int op0 = (encFlags1 >>> EncFlags1.VEX_OP0_SHIFT) & EncFlags1.VEX_OP_MASK;
			int op1 = (encFlags1 >>> EncFlags1.VEX_OP1_SHIFT) & EncFlags1.VEX_OP_MASK;
			int op2 = (encFlags1 >>> EncFlags1.VEX_OP2_SHIFT) & EncFlags1.VEX_OP_MASK;
			int op3 = (encFlags1 >>> EncFlags1.VEX_OP3_SHIFT) & EncFlags1.VEX_OP_MASK;
			int op4 = (encFlags1 >>> EncFlags1.VEX_OP4_SHIFT) & EncFlags1.VEX_OP_MASK;
			if (op4 != 0) {
				assert op0 != 0 && op1 != 0 && op2 != 0 && op3 != 0;
				return new Op[] { OpTables.vexOps[op0 - 1], OpTables.vexOps[op1 - 1], OpTables.vexOps[op2 - 1], OpTables.vexOps[op3 - 1],
						OpTables.vexOps[op4 - 1] };
			}
			if (op3 != 0) {
				assert op0 != 0 && op1 != 0 && op2 != 0;
				return new Op[] { OpTables.vexOps[op0 - 1], OpTables.vexOps[op1 - 1], OpTables.vexOps[op2 - 1], OpTables.vexOps[op3 - 1] };
			}
			if (op2 != 0) {
				assert op0 != 0 && op1 != 0;
				return new Op[] { OpTables.vexOps[op0 - 1], OpTables.vexOps[op1 - 1], OpTables.vexOps[op2 - 1] };
			}
			if (op1 != 0) {
				assert op0 != 0;
				return new Op[] { OpTables.vexOps[op0 - 1], OpTables.vexOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpTables.vexOps[op0 - 1] };
			return new Op[0];
		}

		/** DO NOT USE: INTERNAL API */
		public VexHandler(int encFlags1, int encFlags2, int encFlags3) {
			super(encFlags2, encFlags3, false, null, createOps(encFlags1));
			int lastByteTmp = 0;
			int mask_W_L_tmp = 0;
			int mask_L_tmp = 0;
			table = (encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK;
			int wbit = (encFlags2 >>> EncFlags2.WBIT_SHIFT) & EncFlags2.WBIT_MASK;
			W1 = wbit == WBit.W1 ? 0xFFFF_FFFF : 0;
			int lbit = (encFlags2 >>> EncFlags2.LBIT_SHIFT) & EncFlags2.LBIT_MASK;
			switch (lbit) {
			case LBit.L1:
			case LBit.L256:
				lastByteTmp = 4;
				break;
			}
			if (W1 != 0)
				lastByteTmp |= 0x80;
			lastByteTmp |= (encFlags2 >>> EncFlags2.MANDATORY_PREFIX_SHIFT) & EncFlags2.MANDATORY_PREFIX_MASK;
			if (wbit == WBit.WIG)
				mask_W_L_tmp |= 0x80;
			if (lbit == LBit.LIG) {
				mask_W_L_tmp |= 4;
				mask_L_tmp |= 4;
			}
			lastByte = lastByteTmp;
			mask_W_L = mask_W_L_tmp;
			mask_L = mask_L_tmp;
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			encoder.writePrefixes(instruction);

			int encoderFlags = encoder.encoderFlags;

			int b = lastByte;
			b |= (~encoderFlags >>> (EncoderFlags.VVVVV_SHIFT - 3)) & 0x78;

			if ((encoder.internal_PreventVEX2 | W1 | (table - VexOpCodeTable.MAP0F)
					| (encoderFlags & (EncoderFlags.X | EncoderFlags.B | EncoderFlags.W))) != 0) {
				encoder.writeByteInternal(0xC4);
				int b2 = table;
				b2 |= (~encoderFlags & 7) << 5;
				encoder.writeByteInternal(b2);
				b |= mask_W_L & encoder.internal_VEX_WIG_LIG;
				encoder.writeByteInternal(b);
			}
			else {
				encoder.writeByteInternal(0xC5);
				b |= (~encoderFlags & 4) << 5;
				b |= mask_L & encoder.internal_VEX_LIG;
				encoder.writeByteInternal(b);
			}
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class XopHandler extends OpCodeHandler {
		private final int table;
		private final int lastByte;

		private static Op[] createOps(int encFlags1) {
			int op0 = (encFlags1 >>> EncFlags1.XOP_OP0_SHIFT) & EncFlags1.XOP_OP_MASK;
			int op1 = (encFlags1 >>> EncFlags1.XOP_OP1_SHIFT) & EncFlags1.XOP_OP_MASK;
			int op2 = (encFlags1 >>> EncFlags1.XOP_OP2_SHIFT) & EncFlags1.XOP_OP_MASK;
			int op3 = (encFlags1 >>> EncFlags1.XOP_OP3_SHIFT) & EncFlags1.XOP_OP_MASK;
			if (op3 != 0) {
				assert op0 != 0 && op1 != 0 && op2 != 0;
				return new Op[] { OpTables.xopOps[op0 - 1], OpTables.xopOps[op1 - 1], OpTables.xopOps[op2 - 1], OpTables.xopOps[op3 - 1] };
			}
			if (op2 != 0) {
				assert op0 != 0 && op1 != 0;
				return new Op[] { OpTables.xopOps[op0 - 1], OpTables.xopOps[op1 - 1], OpTables.xopOps[op2 - 1] };
			}
			if (op1 != 0) {
				assert op0 != 0;
				return new Op[] { OpTables.xopOps[op0 - 1], OpTables.xopOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpTables.xopOps[op0 - 1] };
			return new Op[0];
		}

		/** DO NOT USE: INTERNAL API */
		public XopHandler(int encFlags1, int encFlags2, int encFlags3) {
			super(encFlags2, encFlags3, false, null, createOps(encFlags1));
			int lastByteTmp = 0;
			table = 8 + ((encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK);
			assert table == 8 || table == 9 || table == 10 : table;
			switch ((encFlags2 >>> EncFlags2.LBIT_SHIFT) & EncFlags2.LBIT_MASK) {
			case LBit.L1:
			case LBit.L256:
				lastByteTmp = 4;
				break;
			}
			int wbit = (encFlags2 >>> EncFlags2.WBIT_SHIFT) & EncFlags2.WBIT_MASK;
			if (wbit == WBit.W1)
				lastByteTmp |= 0x80;
			lastByteTmp |= (encFlags2 >>> EncFlags2.MANDATORY_PREFIX_SHIFT) & EncFlags2.MANDATORY_PREFIX_MASK;
			lastByte = lastByteTmp;
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			encoder.writePrefixes(instruction);

			encoder.writeByteInternal(0x8F);

			int encoderFlags = encoder.encoderFlags;

			int b = table;
			b |= (~encoderFlags & 7) << 5;
			encoder.writeByteInternal(b);
			b = lastByte;
			b |= (~encoderFlags >>> (EncoderFlags.VVVVV_SHIFT - 3)) & 0x78;
			encoder.writeByteInternal(b);
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class EvexHandler extends OpCodeHandler {
		private final int wbit;
		private final int tupleType;
		private final int table;
		private final int p1Bits;
		private final int llBits;
		private final int mask_W;
		private final int mask_LL;

		private static Op[] createOps(int encFlags1) {
			int op0 = (encFlags1 >>> EncFlags1.EVEX_OP0_SHIFT) & EncFlags1.EVEX_OP_MASK;
			int op1 = (encFlags1 >>> EncFlags1.EVEX_OP1_SHIFT) & EncFlags1.EVEX_OP_MASK;
			int op2 = (encFlags1 >>> EncFlags1.EVEX_OP2_SHIFT) & EncFlags1.EVEX_OP_MASK;
			int op3 = (encFlags1 >>> EncFlags1.EVEX_OP3_SHIFT) & EncFlags1.EVEX_OP_MASK;
			if (op3 != 0) {
				assert op0 != 0 && op1 != 0 && op2 != 0;
				return new Op[] { OpTables.evexOps[op0 - 1], OpTables.evexOps[op1 - 1], OpTables.evexOps[op2 - 1], OpTables.evexOps[op3 - 1] };
			}
			if (op2 != 0) {
				assert op0 != 0 && op1 != 0;
				return new Op[] { OpTables.evexOps[op0 - 1], OpTables.evexOps[op1 - 1], OpTables.evexOps[op2 - 1] };
			}
			if (op1 != 0) {
				assert op0 != 0;
				return new Op[] { OpTables.evexOps[op0 - 1], OpTables.evexOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpTables.evexOps[op0 - 1] };
			return new Op[0];
		}

		static final TryConvertToDisp8N tryConvertToDisp8N = new TryConvertToDisp8NImpl();

		/** DO NOT USE: INTERNAL API */
		public EvexHandler(int encFlags1, int encFlags2, int encFlags3) {
			super(encFlags2, encFlags3, false, tryConvertToDisp8N, createOps(encFlags1));
			int mask_LL_tmp = 0;
			int p1BitsTmp = 0;
			int mask_W_tmp = 0;
			tupleType = (encFlags3 >>> EncFlags3.TUPLE_TYPE_SHIFT) & EncFlags3.TUPLE_TYPE_MASK;
			table = (encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK;
			p1BitsTmp = 4 | ((encFlags2 >>> EncFlags2.MANDATORY_PREFIX_SHIFT) & EncFlags2.MANDATORY_PREFIX_MASK);
			wbit = (encFlags2 >>> EncFlags2.WBIT_SHIFT) & EncFlags2.WBIT_MASK;
			if (wbit == WBit.W1)
				p1BitsTmp |= 0x80;
			switch ((encFlags2 >>> EncFlags2.LBIT_SHIFT) & EncFlags2.LBIT_MASK) {
			case LBit.LIG:
				llBits = 0 << 5;
				mask_LL_tmp = 3 << 5;
				break;
			case LBit.L0:
			case LBit.LZ:
			case LBit.L128:
				llBits = 0 << 5;
				break;
			case LBit.L1:
			case LBit.L256:
				llBits = 1 << 5;
				break;
			case LBit.L512:
				llBits = 2 << 5;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			if (wbit == WBit.WIG)
				mask_W_tmp |= 0x80;
			mask_LL = mask_LL_tmp;
			p1Bits = p1BitsTmp;
			mask_W = mask_W_tmp;
		}

		static final class TryConvertToDisp8NImpl extends TryConvertToDisp8N {
			@Override
			Integer convert(Encoder encoder, OpCodeHandler handler, Instruction instruction, int displ) {
				EvexHandler evexHandler = (EvexHandler)handler;
				int n = TupleTypeTable.getDisp8N(evexHandler.tupleType, (encoder.encoderFlags & EncoderFlags.BROADCAST) != 0);
				int res = displ / n;
				if (res * n == displ && -0x80 <= res && res <= 0x7F)
					return res;

				return null;
			}
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			encoder.writePrefixes(instruction);

			int encoderFlags = encoder.encoderFlags;

			encoder.writeByteInternal(0x62);

			int b = table;
			b |= (encoderFlags & 7) << 5;
			b |= (encoderFlags >>> (9 - 4)) & 0x10;
			b ^= ~0xF;
			encoder.writeByteInternal(b);

			b = p1Bits;
			b |= (~encoderFlags >>> (EncoderFlags.VVVVV_SHIFT - 3)) & 0x78;
			b |= mask_W & encoder.internal_EVEX_WIG;
			encoder.writeByteInternal(b);

			b = instruction.getRawOpMask();
			if (b != 0) {
				if ((encFlags3 & EncFlags3.OP_MASK_REGISTER) == 0)
					encoder.setErrorMessage("The instruction doesn't support opmask registers");
			}
			else {
				if ((encFlags3 & EncFlags3.REQUIRE_OP_MASK_REGISTER) != 0)
					encoder.setErrorMessage("The instruction must use an opmask register");
			}
			b |= (encoderFlags >>> (EncoderFlags.VVVVV_SHIFT + 4 - 3)) & 8;
			if (instruction.getSuppressAllExceptions()) {
				if ((encFlags3 & EncFlags3.SUPPRESS_ALL_EXCEPTIONS) == 0)
					encoder.setErrorMessage("The instruction doesn't support suppress-all-exceptions");
				b |= 0x10;
			}
			int rc = instruction.getRoundingControl();
			if (rc != RoundingControl.NONE) {
				if ((encFlags3 & EncFlags3.ROUNDING_CONTROL) == 0)
					encoder.setErrorMessage("The instruction doesn't support rounding control");
				b |= 0x10;
				b |= (rc - RoundingControl.ROUND_TO_NEAREST) << 5;
			}
			else if ((encFlags3 & EncFlags3.SUPPRESS_ALL_EXCEPTIONS) == 0 || !instruction.getSuppressAllExceptions())
				b |= llBits;
			if ((encoderFlags & EncoderFlags.BROADCAST) != 0)
				b |= 0x10;
			else if (instruction.getBroadcast())
				encoder.setErrorMessage("The instruction doesn't support broadcasting");
			if (instruction.getZeroingMasking()) {
				if ((encFlags3 & EncFlags3.ZEROING_MASKING) == 0)
					encoder.setErrorMessage("The instruction doesn't support zeroing masking");
				b |= 0x80;
			}
			b ^= 8;
			b |= mask_LL & encoder.internal_EVEX_LIG;
			encoder.writeByteInternal(b);
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class MvexHandler extends OpCodeHandler {
		private final int wbit;
		private final int table;
		private final int p1Bits;
		private final int mask_W;

		private static Op[] createOps(int encFlags1) {
			int op0 = (encFlags1 >>> EncFlags1.MVEX_OP0_SHIFT) & EncFlags1.MVEX_OP_MASK;
			int op1 = (encFlags1 >>> EncFlags1.MVEX_OP1_SHIFT) & EncFlags1.MVEX_OP_MASK;
			int op2 = (encFlags1 >>> EncFlags1.MVEX_OP2_SHIFT) & EncFlags1.MVEX_OP_MASK;
			int op3 = (encFlags1 >>> EncFlags1.MVEX_OP3_SHIFT) & EncFlags1.MVEX_OP_MASK;
			if (op3 != 0) {
				assert op0 != 0 && op1 != 0 && op2 != 0;
				return new Op[] { OpTables.mvexOps[op0 - 1], OpTables.mvexOps[op1 - 1], OpTables.mvexOps[op2 - 1], OpTables.mvexOps[op3 - 1] };
			}
			if (op2 != 0) {
				assert op0 != 0 && op1 != 0;
				return new Op[] { OpTables.mvexOps[op0 - 1], OpTables.mvexOps[op1 - 1], OpTables.mvexOps[op2 - 1] };
			}
			if (op1 != 0) {
				assert op0 != 0;
				return new Op[] { OpTables.mvexOps[op0 - 1], OpTables.mvexOps[op1 - 1] };
			}
			if (op0 != 0)
				return new Op[] { OpTables.mvexOps[op0 - 1] };
			return new Op[0];
		}

		static final TryConvertToDisp8N tryConvertToDisp8N = new TryConvertToDisp8NImpl();

		/** DO NOT USE: INTERNAL API */
		public MvexHandler(int encFlags1, int encFlags2, int encFlags3) {
			super(encFlags2, encFlags3, false, tryConvertToDisp8N, createOps(encFlags1));
			int p1BitsTmp = 0;
			int mask_W_tmp = 0;
			table = (encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK;
			p1BitsTmp = (encFlags2 >>> EncFlags2.MANDATORY_PREFIX_SHIFT) & EncFlags2.MANDATORY_PREFIX_MASK;
			wbit = ((encFlags2 >>> EncFlags2.WBIT_SHIFT) & EncFlags2.WBIT_MASK);
			if (wbit == WBit.W1)
				p1BitsTmp |= 0x80;
			if (wbit == WBit.WIG)
				mask_W_tmp |= 0x80;
			p1Bits = p1BitsTmp;
			mask_W = mask_W_tmp;
		}

		static final class TryConvertToDisp8NImpl extends TryConvertToDisp8N {
			@Override
			Integer convert(Encoder encoder, OpCodeHandler handler, Instruction instruction, int displ) {
				int sss = (instruction.getMvexRegMemConv() - MvexRegMemConv.MEM_CONV_NONE) & 7;
				int tupleType = MvexTupleTypeLut.data[MvexInfo.getTupleTypeLutKind(instruction.getCode()) * 8 + sss];
				int n = TupleTypeTable.getDisp8N(tupleType, false);
				int res = displ / n;
				if (res * n == displ && -0x80 <= res && res <= 0x7F)
					return res;

				return null;
			}
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			encoder.writePrefixes(instruction);

			int encoderFlags = encoder.encoderFlags;

			encoder.writeByteInternal(0x62);

			int b = table;
			b |= (encoderFlags & 7) << 5;
			b |= (encoderFlags >>> (9 - 4)) & 0x10;
			b ^= ~0xF;
			encoder.writeByteInternal(b);

			b = p1Bits;
			b |= (~encoderFlags >>> (EncoderFlags.VVVVV_SHIFT - 3)) & 0x78;
			b |= mask_W & encoder.internal_MVEX_WIG;
			encoder.writeByteInternal(b);

			b = instruction.getRawOpMask();
			if (b != 0) {
				if ((encFlags3 & EncFlags3.OP_MASK_REGISTER) == 0)
					encoder.setErrorMessage("The instruction doesn't support opmask registers");
			}
			else {
				if ((encFlags3 & EncFlags3.REQUIRE_OP_MASK_REGISTER) != 0)
					encoder.setErrorMessage("The instruction must use an opmask register");
			}
			b |= (encoderFlags >>> (EncoderFlags.VVVVV_SHIFT + 4 - 3)) & 8;
			int conv = instruction.getMvexRegMemConv();
			// Memory ops can only be op0-op2, never op3 (imm8)
			if (instruction.getOp0Kind() == OpKind.MEMORY || instruction.getOp1Kind() == OpKind.MEMORY || instruction.getOp2Kind() == OpKind.MEMORY) {
				if (conv >= MvexRegMemConv.MEM_CONV_NONE && conv <= MvexRegMemConv.MEM_CONV_SINT16)
					b |= (conv - MvexRegMemConv.MEM_CONV_NONE) << 4;
				else if (conv == MvexRegMemConv.NONE) {
					// Nothing, treat it as MvexRegMemConv.MEM_CONV_NONE
				}
				else
					encoder.setErrorMessage("Memory operands must use a valid MvexRegMemConv variant, eg. MvexRegMemConv.MEM_CONV_NONE");
				if (instruction.getMvexEvictionHint()) {
					if (MvexInfo.canUseEvictionHint(instruction.getCode()))
						b |= 0x80;
					else
						encoder.setErrorMessage("This instruction doesn't support eviction hint (`{eh}`)");
				}
			}
			else {
				if (instruction.getMvexEvictionHint())
					encoder.setErrorMessage("Only memory operands can enable eviction hint (`{eh}`)");
				if (conv == MvexRegMemConv.NONE) {
					b |= 0x80;
					if (instruction.getSuppressAllExceptions()) {
						b |= 0x40;
						if ((encFlags3 & EncFlags3.SUPPRESS_ALL_EXCEPTIONS) == 0)
							encoder.setErrorMessage("The instruction doesn't support suppress-all-exceptions");
					}
					int rc = instruction.getRoundingControl();
					if (rc == RoundingControl.NONE) {
						// Nothing
					}
					else {
						if ((encFlags3 & EncFlags3.ROUNDING_CONTROL) == 0)
							encoder.setErrorMessage("The instruction doesn't support rounding control");
						else {
							b |= (rc - RoundingControl.ROUND_TO_NEAREST) << 4;
						}
					}
				}
				else if (conv >= MvexRegMemConv.REG_SWIZZLE_NONE && conv <= MvexRegMemConv.REG_SWIZZLE_DDDD) {
					if (instruction.getSuppressAllExceptions())
						encoder.setErrorMessage("Can't use {sae} with register swizzles");
					else if (instruction.getRoundingControl() != RoundingControl.NONE)
						encoder.setErrorMessage("Can't use rounding control with register swizzles");
					b |= ((conv - MvexRegMemConv.REG_SWIZZLE_NONE) & 7) << 4;
				}
				else
					encoder.setErrorMessage("Register operands can't use memory up/down conversions");
			}
			if (MvexInfo.getEHBit(instruction.getCode()) == MvexEHBit.EH1)
				b |= 0x80;
			b ^= 8;
			encoder.writeByteInternal(b);
		}
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final class D3nowHandler extends OpCodeHandler {
		final int immediate;

		/** DO NOT USE: INTERNAL API */
		public D3nowHandler(int encFlags2, int encFlags3) {
			super((encFlags2 & ~(0xFFFF << EncFlags2.OP_CODE_SHIFT)) | (0x000F << EncFlags2.OP_CODE_SHIFT), encFlags3, false, null,
					Op.operands_3dnow);
			immediate = getOpCode(encFlags2);
			assert Integer.compareUnsigned(immediate, 0xFF) <= 0 : immediate;
		}

		@Override
		void encode(Encoder encoder, Instruction instruction) {
			encoder.writePrefixes(instruction);
			encoder.writeByteInternal(0x0F);
			encoder.immSize = ImmSize.SIZE1_OP_CODE;
			encoder.immediate = immediate;
		}
	}
}
