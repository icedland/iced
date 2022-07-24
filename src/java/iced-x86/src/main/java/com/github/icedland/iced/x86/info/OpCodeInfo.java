// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.MemorySize;
import com.github.icedland.iced.x86.MvexConvFn;
import com.github.icedland.iced.x86.MvexEHBit;
import com.github.icedland.iced.x86.TupleType;
import com.github.icedland.iced.x86.MvexTupleTypeLutKind;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.internal.InstructionMemorySizes;
import com.github.icedland.iced.x86.internal.InstructionOpCounts;
import com.github.icedland.iced.x86.internal.MandatoryPrefixByte;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.enc.EncFlags1;
import com.github.icedland.iced.x86.internal.enc.EncFlags2;
import com.github.icedland.iced.x86.internal.enc.EncFlags3;
import com.github.icedland.iced.x86.internal.enc.EvexOpCodeTable;
import com.github.icedland.iced.x86.internal.enc.LBit;
import com.github.icedland.iced.x86.internal.enc.LKind;
import com.github.icedland.iced.x86.internal.enc.LegacyOpCodeTable;
import com.github.icedland.iced.x86.internal.enc.MvexOpCodeTable;
import com.github.icedland.iced.x86.internal.enc.OpCodeInfoFlags1;
import com.github.icedland.iced.x86.internal.enc.OpCodeInfoFlags2;
import com.github.icedland.iced.x86.internal.enc.VexOpCodeTable;
import com.github.icedland.iced.x86.internal.enc.WBit;
import com.github.icedland.iced.x86.internal.enc.XopOpCodeTable;

/**
 * Opcode info
 */
public final class OpCodeInfo {
	private final String toOpCodeStringValue;
	private final String toInstructionStringValue;
	private final int encFlags2;
	private final int encFlags3;
	private final int opcFlags1;
	private final int opcFlags2;
	private final int code;
	private final byte encoding;
	private final byte operandSize;
	private final byte addressSize;
	private final byte l;
	private final byte tupleType;
	private final byte table;
	private final byte mandatoryPrefix;
	private final byte groupIndex;
	private final byte rmGroupIndex;
	private final byte op0Kind;
	private final byte op1Kind;
	private final byte op2Kind;
	private final byte op3Kind;
	private final byte op4Kind;
	private final int flags;

	private static final class Flags {
		static final int IGNORES_ROUNDING_CONTROL = 0x00000001;
		static final int AMD_LOCK_REG_BIT = 0x00000002;
		static final int LIG = 0x00000004;
		static final int W = 0x00000008;
		static final int WIG = 0x00000010;
		static final int WIG32 = 0x00000020;
		static final int CPL0 = 0x00000040;
		static final int CPL1 = 0x00000080;
		static final int CPL2 = 0x00000100;
		static final int CPL3 = 0x00000200;
	}

	OpCodeInfo(int code, int encFlags1, int encFlags2, int encFlags3, int opcFlags1, int opcFlags2, StringBuilder sb, String[] mnemonics) {
		this.code = code;
		this.encFlags2 = encFlags2;
		this.encFlags3 = encFlags3;
		this.opcFlags1 = opcFlags1;
		this.opcFlags2 = opcFlags2;

		int flagsTmp = 0;

		if ((encFlags1 & EncFlags1.IGNORES_ROUNDING_CONTROL) != 0)
			flagsTmp |= Flags.IGNORES_ROUNDING_CONTROL;
		if ((encFlags1 & EncFlags1.AMD_LOCK_REG_BIT) != 0)
			flagsTmp |= Flags.AMD_LOCK_REG_BIT;
		switch (opcFlags1 & (OpCodeInfoFlags1.CPL0_ONLY | OpCodeInfoFlags1.CPL3_ONLY)) {
		case OpCodeInfoFlags1.CPL0_ONLY:
			flagsTmp |= Flags.CPL0;
			break;
		case OpCodeInfoFlags1.CPL3_ONLY:
			flagsTmp |= Flags.CPL3;
			break;
		default:
			flagsTmp |= Flags.CPL0 | Flags.CPL1 | Flags.CPL2 | Flags.CPL3;
			break;
		}

		encoding = (byte)((encFlags3 >>> EncFlags3.ENCODING_SHIFT) & EncFlags3.ENCODING_MASK);
		switch ((encFlags2 >>> EncFlags2.MANDATORY_PREFIX_SHIFT) & EncFlags2.MANDATORY_PREFIX_MASK) {
		case MandatoryPrefixByte.NONE:
			mandatoryPrefix = (byte)((encFlags2 & EncFlags2.HAS_MANDATORY_PREFIX) != 0 ? MandatoryPrefix.PNP : MandatoryPrefix.NONE);
			break;
		case MandatoryPrefixByte.P66:
			mandatoryPrefix = (byte)MandatoryPrefix.P66;
			break;
		case MandatoryPrefixByte.PF3:
			mandatoryPrefix = (byte)MandatoryPrefix.PF3;
			break;
		case MandatoryPrefixByte.PF2:
			mandatoryPrefix = (byte)MandatoryPrefix.PF2;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		switch ((encFlags3 >>> EncFlags3.OPERAND_SIZE_SHIFT) & EncFlags3.OPERAND_SIZE_MASK) {
		case CodeSize.UNKNOWN:
			operandSize = 0;
			break;
		case CodeSize.CODE16:
			operandSize = 16;
			break;
		case CodeSize.CODE32:
			operandSize = 32;
			break;
		case CodeSize.CODE64:
			operandSize = 64;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		switch ((encFlags3 >>> EncFlags3.ADDRESS_SIZE_SHIFT) & EncFlags3.ADDRESS_SIZE_MASK) {
		case CodeSize.UNKNOWN:
			addressSize = 0;
			break;
		case CodeSize.CODE16:
			addressSize = 16;
			break;
		case CodeSize.CODE32:
			addressSize = 32;
			break;
		case CodeSize.CODE64:
			addressSize = 64;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		groupIndex = (byte)((encFlags2 & EncFlags2.HAS_GROUP_INDEX) == 0 ? -1 : (encFlags2 >>> EncFlags2.GROUP_INDEX_SHIFT) & 7);
		rmGroupIndex = (byte)((encFlags3 & EncFlags3.HAS_RM_GROUP_INDEX) == 0 ? -1 : (encFlags2 >>> EncFlags2.GROUP_INDEX_SHIFT) & 7);
		tupleType = (byte)((encFlags3 >>> EncFlags3.TUPLE_TYPE_SHIFT) & EncFlags3.TUPLE_TYPE_MASK);

		int lkind;
		switch (((encFlags2 >>> EncFlags2.LBIT_SHIFT) & EncFlags2.LBIT_MASK)) {
		case LBit.LZ:
			lkind = LKind.LZ;
			l = 0;
			break;
		case LBit.L0:
			lkind = LKind.L0;
			l = 0;
			break;
		case LBit.L1:
			lkind = LKind.L0;
			l = 1;
			break;
		case LBit.L128:
			lkind = LKind.L128;
			l = 0;
			break;
		case LBit.L256:
			lkind = LKind.L128;
			l = 1;
			break;
		case LBit.L512:
			lkind = LKind.L128;
			l = 2;
			break;
		case LBit.LIG:
			lkind = LKind.NONE;
			l = 0;
			flagsTmp |= Flags.LIG;
			break;
		default:
			throw new UnsupportedOperationException();
		}

		switch (((encFlags2 >>> EncFlags2.WBIT_SHIFT) & EncFlags2.WBIT_MASK)) {
		case WBit.W0:
			break;
		case WBit.W1:
			flagsTmp |= Flags.W;
			break;
		case WBit.WIG:
			flagsTmp |= Flags.WIG;
			break;
		case WBit.WIG32:
			flagsTmp |= Flags.WIG32;
			break;
		default:
			throw new UnsupportedOperationException();
		}

		byte[] opKinds;
		switch (encoding) {
		case EncodingKind.LEGACY:
			opKinds = OpCodeOperandKinds.legacyOpKinds;
			op0Kind = opKinds[((encFlags1 >>> EncFlags1.LEGACY_OP0_SHIFT) & EncFlags1.LEGACY_OP_MASK)];
			op1Kind = opKinds[((encFlags1 >>> EncFlags1.LEGACY_OP1_SHIFT) & EncFlags1.LEGACY_OP_MASK)];
			op2Kind = opKinds[((encFlags1 >>> EncFlags1.LEGACY_OP2_SHIFT) & EncFlags1.LEGACY_OP_MASK)];
			op3Kind = opKinds[((encFlags1 >>> EncFlags1.LEGACY_OP3_SHIFT) & EncFlags1.LEGACY_OP_MASK)];
			op4Kind = 0;

			switch ((encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK) {
			case LegacyOpCodeTable.MAP0:
				table = (byte)OpCodeTableKind.NORMAL;
				break;
			case LegacyOpCodeTable.MAP0F:
				table = (byte)OpCodeTableKind.T0F;
				break;
			case LegacyOpCodeTable.MAP0F38:
				table = (byte)OpCodeTableKind.T0F38;
				break;
			case LegacyOpCodeTable.MAP0F3A:
				table = (byte)OpCodeTableKind.T0F3A;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			break;

		case EncodingKind.VEX:
			opKinds = OpCodeOperandKinds.vexOpKinds;
			op0Kind = opKinds[((encFlags1 >>> EncFlags1.VEX_OP0_SHIFT) & EncFlags1.VEX_OP_MASK)];
			op1Kind = opKinds[((encFlags1 >>> EncFlags1.VEX_OP1_SHIFT) & EncFlags1.VEX_OP_MASK)];
			op2Kind = opKinds[((encFlags1 >>> EncFlags1.VEX_OP2_SHIFT) & EncFlags1.VEX_OP_MASK)];
			op3Kind = opKinds[((encFlags1 >>> EncFlags1.VEX_OP3_SHIFT) & EncFlags1.VEX_OP_MASK)];
			op4Kind = opKinds[((encFlags1 >>> EncFlags1.VEX_OP4_SHIFT) & EncFlags1.VEX_OP_MASK)];

			switch ((encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK) {
			case VexOpCodeTable.MAP0:
				table = (byte)OpCodeTableKind.NORMAL;
				break;
			case VexOpCodeTable.MAP0F:
				table = (byte)OpCodeTableKind.T0F;
				break;
			case VexOpCodeTable.MAP0F38:
				table = (byte)OpCodeTableKind.T0F38;
				break;
			case VexOpCodeTable.MAP0F3A:
				table = (byte)OpCodeTableKind.T0F3A;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			break;

		case EncodingKind.EVEX:
			opKinds = OpCodeOperandKinds.evexOpKinds;
			op0Kind = opKinds[((encFlags1 >>> EncFlags1.EVEX_OP0_SHIFT) & EncFlags1.EVEX_OP_MASK)];
			op1Kind = opKinds[((encFlags1 >>> EncFlags1.EVEX_OP1_SHIFT) & EncFlags1.EVEX_OP_MASK)];
			op2Kind = opKinds[((encFlags1 >>> EncFlags1.EVEX_OP2_SHIFT) & EncFlags1.EVEX_OP_MASK)];
			op3Kind = opKinds[((encFlags1 >>> EncFlags1.EVEX_OP3_SHIFT) & EncFlags1.EVEX_OP_MASK)];
			op4Kind = 0;

			switch ((encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK) {
			case EvexOpCodeTable.MAP0F:
				table = (byte)OpCodeTableKind.T0F;
				break;
			case EvexOpCodeTable.MAP0F38:
				table = (byte)OpCodeTableKind.T0F38;
				break;
			case EvexOpCodeTable.MAP0F3A:
				table = (byte)OpCodeTableKind.T0F3A;
				break;
			case EvexOpCodeTable.MAP5:
				table = (byte)OpCodeTableKind.MAP5;
				break;
			case EvexOpCodeTable.MAP6:
				table = (byte)OpCodeTableKind.MAP6;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			break;

		case EncodingKind.XOP:
			opKinds = OpCodeOperandKinds.xopOpKinds;
			op0Kind = opKinds[((encFlags1 >>> EncFlags1.XOP_OP0_SHIFT) & EncFlags1.XOP_OP_MASK)];
			op1Kind = opKinds[((encFlags1 >>> EncFlags1.XOP_OP1_SHIFT) & EncFlags1.XOP_OP_MASK)];
			op2Kind = opKinds[((encFlags1 >>> EncFlags1.XOP_OP2_SHIFT) & EncFlags1.XOP_OP_MASK)];
			op3Kind = opKinds[((encFlags1 >>> EncFlags1.XOP_OP3_SHIFT) & EncFlags1.XOP_OP_MASK)];
			op4Kind = 0;

			switch ((encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK) {
			case XopOpCodeTable.MAP8:
				table = (byte)OpCodeTableKind.MAP8;
				break;
			case XopOpCodeTable.MAP9:
				table = (byte)OpCodeTableKind.MAP9;
				break;
			case XopOpCodeTable.MAP10:
				table = (byte)OpCodeTableKind.MAP10;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			break;

		case EncodingKind.D3NOW:
			op0Kind = (byte)OpCodeOperandKind.MM_REG;
			op1Kind = (byte)OpCodeOperandKind.MM_OR_MEM;
			op2Kind = 0;
			op3Kind = 0;
			op4Kind = 0;
			table = (byte)OpCodeTableKind.T0F;
			break;

		case EncodingKind.MVEX:
			opKinds = OpCodeOperandKinds.mvexOpKinds;
			op0Kind = opKinds[((encFlags1 >>> EncFlags1.MVEX_OP0_SHIFT) & EncFlags1.MVEX_OP_MASK)];
			op1Kind = opKinds[((encFlags1 >>> EncFlags1.MVEX_OP1_SHIFT) & EncFlags1.MVEX_OP_MASK)];
			op2Kind = opKinds[((encFlags1 >>> EncFlags1.MVEX_OP2_SHIFT) & EncFlags1.MVEX_OP_MASK)];
			op3Kind = opKinds[((encFlags1 >>> EncFlags1.MVEX_OP3_SHIFT) & EncFlags1.MVEX_OP_MASK)];
			op4Kind = 0;

			switch ((encFlags2 >>> EncFlags2.TABLE_SHIFT) & EncFlags2.TABLE_MASK) {
			case MvexOpCodeTable.MAP0F:
				table = (byte)OpCodeTableKind.T0F;
				break;
			case MvexOpCodeTable.MAP0F38:
				table = (byte)OpCodeTableKind.T0F38;
				break;
			case MvexOpCodeTable.MAP0F3A:
				table = (byte)OpCodeTableKind.T0F3A;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			break;

		default:
			throw new UnsupportedOperationException();
		}

		flags = flagsTmp;
		this.toOpCodeStringValue = new OpCodeFormatter(this, sb, lkind, (opcFlags1 & OpCodeInfoFlags1.MOD_REG_RM_STRING) != 0).format();
		int fmtOption = (opcFlags2 >>> OpCodeInfoFlags2.INSTR_STR_FMT_OPTION_SHIFT) & OpCodeInfoFlags2.INSTR_STR_FMT_OPTION_MASK;
		this.toInstructionStringValue = new InstructionFormatter(this, fmtOption, sb, mnemonics).format();
	}

	/**
	 * Gets the code (a {@link Code} enum variant)
	 */
	public int getCode() {
		return code;
	}

	/**
	 * Gets the mnemonic (a {@link com.github.icedland.iced.x86.Mnemonic} enum variant)
	 */
	public int getMnemonic() {
		return Code.mnemonic(getCode());
	}

	/**
	 * Gets the encoding (an {@link EncodingKind} enum variant)
	 */
	public int getEncoding() {
		return encoding;
	}

	/**
	 * {@code true} if it's an instruction, {@code false} if it's eg.<!-- --> {@link Code#INVALID}, {@code db}, {@code dw},
	 * {@code dd}, {@code dq}, {@code zero_bytes}
	 */
	public boolean isInstruction() {
		return !(code <= Code.DECLAREQWORD || code == Code.ZERO_BYTES);
	}

	/**
	 * {@code true} if it's an instruction available in 16-bit mode
	 */
	public boolean getMode16() {
		return (encFlags3 & EncFlags3.BIT16OR32) != 0;
	}

	/**
	 * {@code true} if it's an instruction available in 32-bit mode
	 */
	public boolean getMode32() {
		return (encFlags3 & EncFlags3.BIT16OR32) != 0;
	}

	/**
	 * {@code true} if it's an instruction available in 64-bit mode
	 */
	public boolean getMode64() {
		return (encFlags3 & EncFlags3.BIT64) != 0;
	}

	/**
	 * {@code true} if an {@code FWAIT} ({@code 9B}) instruction is added before the instruction
	 */
	public boolean getFwait() {
		return (encFlags3 & EncFlags3.FWAIT) != 0;
	}

	/**
	 * (Legacy encoding) Gets the required operand size (16,32,64) or 0
	 */
	public int getOperandSize() {
		return operandSize;
	}

	/**
	 * (Legacy encoding) Gets the required address size (16,32,64) or 0
	 */
	public int getAddressSize() {
		return addressSize;
	}

	/**
	 * (VEX/XOP/EVEX) {@code L} / {@code L'L} value or default value if {@link #isLIG()} is {@code true}
	 */
	public int getL() {
		return l;
	}

	/**
	 * (VEX/XOP/EVEX/MVEX) {@code W} value or default value if {@link #isWIG()} or {@link #isWIG32()} is {@code true}
	 */
	public int getW() {
		return (flags & Flags.W) != 0 ? 1 : 0;
	}

	/**
	 * (VEX/XOP/EVEX) {@code true} if the {@code L} / {@code L'L} fields are ignored.
	 * <p>
	 * EVEX: if reg-only ops and {@code {er}} ({@code EVEX.b} is set), {@code L'L} is the rounding control and not ignored.
	 */
	public boolean isLIG() {
		return (flags & Flags.LIG) != 0;
	}

	/**
	 * (VEX/XOP/EVEX/MVEX) {@code true} if the {@code W} field is ignored in 16/32/64-bit modes
	 */
	public boolean isWIG() {
		return (flags & Flags.WIG) != 0;
	}

	/**
	 * (VEX/XOP/EVEX/MVEX) {@code true} if the {@code W} field is ignored in 16/32-bit modes (but not 64-bit mode)
	 */
	public boolean isWIG32() {
		return (flags & Flags.WIG32) != 0;
	}

	/**
	 * (EVEX/MVEX) Gets the tuple type (a {@link TupleType} enum variant)
	 */
	public int getTupleType() {
		return tupleType;
	}

	/**
	 * (MVEX) Gets the {@code EH} bit that's required to encode this instruction (an {@link MvexEHBit} enum variant)
	 */
	public int getMvexEHBit() {
		return getEncoding() == EncodingKind.MVEX ? MvexInfo.getEHBit(getCode()) : MvexEHBit.NONE;
	}

	/**
	 * (MVEX) {@code true} if the instruction supports eviction hint (if it has a memory operand)
	 */
	public boolean getMvexCanUseEvictionHint() {
		return getEncoding() == EncodingKind.MVEX && MvexInfo.canUseEvictionHint(getCode());
	}

	/**
	 * (MVEX) {@code true} if the instruction's rounding control bits are stored in {@code imm8[1:0]}
	 */
	public boolean getMvexCanUseImmRoundingControl() {
		return getEncoding() == EncodingKind.MVEX && MvexInfo.canUseImmRoundingControl(getCode());
	}

	/**
	 * (MVEX) {@code true} if the instruction ignores op mask registers (eg.<!-- --> {@code {k1}})
	 */
	public boolean getMvexIgnoresOpMaskRegister() {
		return getEncoding() == EncodingKind.MVEX && MvexInfo.getIgnoresOpMaskRegister(getCode());
	}

	/**
	 * (MVEX) {@code true} if the instruction must have {@code MVEX.SSS=000} if {@code MVEX.EH=1}
	 */
	public boolean getMvexNoSaeRc() {
		return getEncoding() == EncodingKind.MVEX && MvexInfo.getNoSaeRc(getCode());
	}

	/**
	 * (MVEX) Gets the tuple type / conv lut kind (an {@link MvexTupleTypeLutKind} enum variant)
	 */
	public int getMvexTupleTypeLutKind() {
		return getEncoding() == EncodingKind.MVEX ? MvexInfo.getTupleTypeLutKind(getCode()) : MvexTupleTypeLutKind.INT32;
	}

	/**
	 * (MVEX) Gets the conversion function, eg.<!-- --> {@code Sf32} (an {@link MvexConvFn} enum variant)
	 */
	public int getMvexConversionFunc() {
		return getEncoding() == EncodingKind.MVEX ? MvexInfo.getConvFn(getCode()) : MvexConvFn.NONE;
	}

	/**
	 * (MVEX) Gets flags indicating which conversion functions are valid (bit 0 == func 0)
	 */
	public int getMvexValidConversionFuncsMask() {
		return getEncoding() == EncodingKind.MVEX ? ~MvexInfo.getInvalidConvFns(getCode()) : 0;
	}

	/**
	 * (MVEX) Gets flags indicating which swizzle functions are valid (bit 0 == func 0)
	 */
	public int getMvexValidSwizzleFuncsMask() {
		return getEncoding() == EncodingKind.MVEX ? ~MvexInfo.getInvalidSwizzleFns(getCode()) : 0;
	}

	/**
	 * If it has a memory operand, gets the {@link MemorySize} (non-broadcast memory type) (a {@link MemorySize} enum variant)
	 */
	public int getMemorySize() {
		return InstructionMemorySizes.sizesNormal[code] & 0xFF;
	}

	/**
	 * If it has a memory operand, gets the {@link MemorySize} (broadcast memory type) (a {@link MemorySize} enum variant)
	 */
	public int getBroadcastMemorySize() {
		return InstructionMemorySizes.sizesBcst[code] & 0xFF;
	}

	/**
	 * (EVEX) {@code true} if the instruction supports broadcasting ({@code EVEX.b} bit) (if it has a memory operand)
	 */
	public boolean canBroadcast() {
		return (encFlags3 & EncFlags3.BROADCAST) != 0;
	}

	/**
	 * (EVEX/MVEX) {@code true} if the instruction supports rounding control
	 */
	public boolean canUseRoundingControl() {
		return (encFlags3 & EncFlags3.ROUNDING_CONTROL) != 0;
	}

	/**
	 * (EVEX/MVEX) {@code true} if the instruction supports suppress all exceptions
	 */
	public boolean canSuppressAllExceptions() {
		return (encFlags3 & EncFlags3.SUPPRESS_ALL_EXCEPTIONS) != 0;
	}

	/**
	 * (EVEX/MVEX) {@code true} if an opmask register can be used
	 */
	public boolean canUseOpMaskRegister() {
		return (encFlags3 & EncFlags3.OP_MASK_REGISTER) != 0;
	}

	/**
	 * (EVEX/MVEX) {@code true} if a non-zero opmask register must be used
	 */
	public boolean getRequireOpMaskRegister() {
		return (encFlags3 & EncFlags3.REQUIRE_OP_MASK_REGISTER) != 0;
	}

	/**
	 * (EVEX) {@code true} if the instruction supports zeroing masking (if one of the opmask registers {@code K1}-{@code K7} is used
	 * and destination operand is not a memory operand)
	 */
	public boolean canUseZeroingMasking() {
		return (encFlags3 & EncFlags3.ZEROING_MASKING) != 0;
	}

	/**
	 * {@code true} if the {@code LOCK} ({@code F0}) prefix can be used
	 */
	public boolean canUseLockPrefix() {
		return (encFlags3 & EncFlags3.LOCK) != 0;
	}

	/**
	 * {@code true} if the {@code XACQUIRE} ({@code F2}) prefix can be used
	 */
	public boolean canUseXacquirePrefix() {
		return (encFlags3 & EncFlags3.XACQUIRE) != 0;
	}

	/**
	 * {@code true} if the {@code XRELEASE} ({@code F3}) prefix can be used
	 */
	public boolean canUseXreleasePrefix() {
		return (encFlags3 & EncFlags3.XRELEASE) != 0;
	}

	/**
	 * {@code true} if the {@code REP} / {@code REPE} ({@code F3}) prefixes can be used
	 */
	public boolean canUseRepPrefix() {
		return (encFlags3 & EncFlags3.REP) != 0;
	}

	/**
	 * {@code true} if the {@code REPNE} ({@code F2}) prefix can be used
	 */
	public boolean canUseRepnePrefix() {
		return (encFlags3 & EncFlags3.REPNE) != 0;
	}

	/**
	 * {@code true} if the {@code BND} ({@code F2}) prefix can be used
	 */
	public boolean canUseBndPrefix() {
		return (encFlags3 & EncFlags3.BND) != 0;
	}

	/**
	 * {@code true} if the {@code HINT-TAKEN} ({@code 3E}) and {@code HINT-NOT-TAKEN} ({@code 2E}) prefixes can be used
	 */
	public boolean canUseHintTakenPrefix() {
		return (encFlags3 & EncFlags3.HINT_TAKEN) != 0;
	}

	/**
	 * {@code true} if the {@code NOTRACK} ({@code 3E}) prefix can be used
	 */
	public boolean canUseNotrackPrefix() {
		return (encFlags3 & EncFlags3.NOTRACK) != 0;
	}

	/**
	 * {@code true} if rounding control is ignored (#UD is not generated)
	 */
	public boolean getIgnoresRoundingControl() {
		return (flags & Flags.IGNORES_ROUNDING_CONTROL) != 0;
	}

	/**
	 * (AMD) {@code true} if the {@code LOCK} prefix can be used as an extra register bit (bit 3) to access registers 8-15 without a
	 * {@code REX} prefix (eg.<!-- --> in 32-bit mode)
	 */
	public boolean getAmdLockRegBit() {
		return (flags & Flags.AMD_LOCK_REG_BIT) != 0;
	}

	/**
	 * {@code true} if the default operand size is 64 in 64-bit mode. A {@code 66} prefix can switch to 16-bit operand size.
	 */
	public boolean getDefaultOpSize64() {
		return (encFlags3 & EncFlags3.DEFAULT_OP_SIZE64) != 0;
	}

	/**
	 * {@code true} if the operand size is always 64 in 64-bit mode. A {@code 66} prefix is ignored.
	 */
	public boolean getForceOpSize64() {
		return (opcFlags1 & OpCodeInfoFlags1.FORCE_OP_SIZE64) != 0;
	}

	/**
	 * {@code true} if the Intel decoder forces 64-bit operand size. A {@code 66} prefix is ignored.
	 */
	public boolean getIntelForceOpSize64() {
		return (encFlags3 & EncFlags3.INTEL_FORCE_OP_SIZE64) != 0;
	}

	/**
	 * {@code true} if it can only be executed when CPL=0
	 */
	public boolean getMustBeCpl0() {
		return (flags & (Flags.CPL0 | Flags.CPL1 | Flags.CPL2 | Flags.CPL3)) == Flags.CPL0;
	}

	/**
	 * {@code true} if it can be executed when CPL=0
	 */
	public boolean getCpl0() {
		return (flags & Flags.CPL0) != 0;
	}

	/**
	 * {@code true} if it can be executed when CPL=1
	 */
	public boolean getCpl1() {
		return (flags & Flags.CPL1) != 0;
	}

	/**
	 * {@code true} if it can be executed when CPL=2
	 */
	public boolean getCpl2() {
		return (flags & Flags.CPL2) != 0;
	}

	/**
	 * {@code true} if it can be executed when CPL=3
	 */
	public boolean getCpl3() {
		return (flags & Flags.CPL3) != 0;
	}

	/**
	 * {@code true} if the instruction accesses the I/O address space (eg.<!-- --> {@code IN}, {@code OUT}, {@code INS},
	 * {@code OUTS})
	 */
	public boolean isInputOutput() {
		return (opcFlags1 & OpCodeInfoFlags1.INPUT_OUTPUT) != 0;
	}

	/**
	 * {@code true} if it's one of the many nop instructions (does not include FPU nop instructions, eg.<!-- --> {@code FNOP})
	 */
	public boolean isNop() {
		return (opcFlags1 & OpCodeInfoFlags1.NOP) != 0;
	}

	/**
	 * {@code true} if it's one of the many reserved nop instructions (eg.<!-- --> {@code 0F0D}, {@code 0F18-0F1F})
	 */
	public boolean isReservedNop() {
		return (opcFlags1 & OpCodeInfoFlags1.RESERVED_NOP) != 0;
	}

	/**
	 * {@code true} if it's a serializing instruction (Intel CPUs)
	 */
	public boolean isSerializingIntel() {
		return (opcFlags1 & OpCodeInfoFlags1.SERIALIZING_INTEL) != 0;
	}

	/**
	 * {@code true} if it's a serializing instruction (AMD CPUs)
	 */
	public boolean isSerializingAmd() {
		return (opcFlags1 & OpCodeInfoFlags1.SERIALIZING_AMD) != 0;
	}

	/**
	 * {@code true} if the instruction requires either CPL=0 or CPL&lt;=3 depending on some CPU option (eg.<!-- --> {@code CR4.TSD},
	 * {@code CR4.PCE}, {@code CR4.UMIP})
	 */
	public boolean getMayRequireCpl0() {
		return (opcFlags1 & OpCodeInfoFlags1.MAY_REQUIRE_CPL0) != 0;
	}

	/**
	 * {@code true} if it's a tracked {@code JMP}/{@code CALL} indirect instruction (CET)
	 */
	public boolean isCetTracked() {
		return (opcFlags1 & OpCodeInfoFlags1.CET_TRACKED) != 0;
	}

	/**
	 * {@code true} if it's a non-temporal hint memory access (eg.<!-- --> {@code MOVNTDQ})
	 */
	public boolean isNonTemporal() {
		return (opcFlags1 & OpCodeInfoFlags1.NON_TEMPORAL) != 0;
	}

	/**
	 * {@code true} if it's a no-wait FPU instruction, eg.<!-- --> {@code FNINIT}
	 */
	public boolean isFpuNoWait() {
		return (opcFlags1 & OpCodeInfoFlags1.FPU_NO_WAIT) != 0;
	}

	/**
	 * {@code true} if the mod bits are ignored and it's assumed {@code modrm[7:6] == 11b}
	 */
	public boolean getIgnoresModBits() {
		return (opcFlags1 & OpCodeInfoFlags1.IGNORES_MOD_BITS) != 0;
	}

	/**
	 * {@code true} if the {@code 66} prefix is not allowed (it will #UD)
	 */
	public boolean getNo66() {
		return (opcFlags1 & OpCodeInfoFlags1.NO66) != 0;
	}

	/**
	 * {@code true} if the {@code F2}/{@code F3} prefixes aren't allowed
	 */
	public boolean getNFx() {
		return (opcFlags1 & OpCodeInfoFlags1.NFX) != 0;
	}

	/**
	 * {@code true} if the index reg's reg-num (vsib op) (if any) and register ops' reg-nums must be unique,
	 * eg.<!-- --> {@code MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]} is invalid. Registers =
	 * {@code XMM}/{@code YMM}/{@code ZMM}/{@code TMM}.
	 */
	public boolean getRequiresUniqueRegNums() {
		return (opcFlags1 & OpCodeInfoFlags1.REQUIRES_UNIQUE_REG_NUMS) != 0;
	}

	/**
	 * {@code true} if the destination register's reg-num must not be present in any other operand, eg.<!-- -->
	 * {@code MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]}
	 * is invalid. Registers = {@code XMM}/{@code YMM}/{@code ZMM}/{@code TMM}.
	 */
	public boolean getRequiresUniqueDestRegNum() {
		return (opcFlags1 & OpCodeInfoFlags1.REQUIRES_UNIQUE_DEST_REG_NUM) != 0;
	}

	/**
	 * {@code true} if it's a privileged instruction (all CPL=0 instructions (except {@code VMCALL}) and IOPL instructions {@code IN},
	 * {@code INS}, {@code OUT}, {@code OUTS}, {@code CLI}, {@code STI})
	 */
	public boolean isPrivileged() {
		return (opcFlags1 & OpCodeInfoFlags1.PRIVILEGED) != 0;
	}

	/**
	 * {@code true} if it reads/writes too many registers
	 */
	public boolean isSaveRestore() {
		return (opcFlags1 & OpCodeInfoFlags1.SAVE_RESTORE) != 0;
	}

	/**
	 * {@code true} if it's an instruction that implicitly uses the stack register, eg.<!-- --> {@code CALL}, {@code POP}, etc
	 */
	public boolean isStackInstruction() {
		return (opcFlags1 & OpCodeInfoFlags1.STACK_INSTRUCTION) != 0;
	}

	/**
	 * {@code true} if the instruction doesn't read the segment register if it uses a memory operand
	 */
	public boolean getIgnoresSegment() {
		return (opcFlags1 & OpCodeInfoFlags1.IGNORES_SEGMENT) != 0;
	}

	/**
	 * {@code true} if the opmask register is read and written (instead of just read). This also implies that it can't be {@code K0}.
	 */
	public boolean isOpMaskReadWrite() {
		return (opcFlags1 & OpCodeInfoFlags1.OP_MASK_READ_WRITE) != 0;
	}

	/**
	 * {@code true} if it can be executed in real mode
	 */
	public boolean getRealMode() {
		return (opcFlags2 & OpCodeInfoFlags2.REAL_MODE) != 0;
	}

	/**
	 * {@code true} if it can be executed in protected mode
	 */
	public boolean getProtectedMode() {
		return (opcFlags2 & OpCodeInfoFlags2.PROTECTED_MODE) != 0;
	}

	/**
	 * {@code true} if it can be executed in virtual 8086 mode
	 */
	public boolean getVirtual8086Mode() {
		return (opcFlags2 & OpCodeInfoFlags2.VIRTUAL8086_MODE) != 0;
	}

	/**
	 * {@code true} if it can be executed in compatibility mode
	 */
	public boolean getCompatibilityMode() {
		return (opcFlags2 & OpCodeInfoFlags2.COMPATIBILITY_MODE) != 0;
	}

	/**
	 * {@code true} if it can be executed in 64-bit mode
	 */
	public boolean getLongMode() {
		return (encFlags3 & EncFlags3.BIT64) != 0;
	}

	/**
	 * {@code true} if it can be used outside SMM
	 */
	public boolean getUseOutsideSmm() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_OUTSIDE_SMM) != 0;
	}

	/**
	 * {@code true} if it can be used in SMM
	 */
	public boolean getUseInSmm() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_IN_SMM) != 0;
	}

	/**
	 * {@code true} if it can be used outside an enclave (SGX)
	 */
	public boolean getUseOutsideEnclaveSgx() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_OUTSIDE_ENCLAVE_SGX) != 0;
	}

	/**
	 * {@code true} if it can be used inside an enclave (SGX1)
	 */
	public boolean getUseInEnclaveSgx1() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_IN_ENCLAVE_SGX1) != 0;
	}

	/**
	 * {@code true} if it can be used inside an enclave (SGX2)
	 */
	public boolean getUseInEnclaveSgx2() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_IN_ENCLAVE_SGX2) != 0;
	}

	/**
	 * {@code true} if it can be used outside VMX operation
	 */
	public boolean getUseOutsideVmxOp() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_OUTSIDE_VMX_OP) != 0;
	}

	/**
	 * {@code true} if it can be used in VMX root operation
	 */
	public boolean getUseInVmxRootOp() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_IN_VMX_ROOT_OP) != 0;
	}

	/**
	 * {@code true} if it can be used in VMX non-root operation
	 */
	public boolean getUseInVmxNonRootOp() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_IN_VMX_NON_ROOT_OP) != 0;
	}

	/**
	 * {@code true} if it can be used outside SEAM
	 */
	public boolean getUseOutsideSeam() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_OUTSIDE_SEAM) != 0;
	}

	/**
	 * {@code true} if it can be used in SEAM
	 */
	public boolean getUseInSeam() {
		return (opcFlags2 & OpCodeInfoFlags2.USE_IN_SEAM) != 0;
	}

	/**
	 * {@code true} if #UD is generated in TDX non-root operation
	 */
	public boolean getTdxNonRootGenUd() {
		return (opcFlags2 & OpCodeInfoFlags2.TDX_NON_ROOT_GEN_UD) != 0;
	}

	/**
	 * {@code true} if #VE is generated in TDX non-root operation
	 */
	public boolean getTdxNonRootGenVe() {
		return (opcFlags2 & OpCodeInfoFlags2.TDX_NON_ROOT_GEN_VE) != 0;
	}

	/**
	 * {@code true} if an exception (eg.<!-- --> #GP(0), #VE) may be generated in TDX non-root operation
	 */
	public boolean getTdxNonRootMayGenEx() {
		return (opcFlags2 & OpCodeInfoFlags2.TDX_NON_ROOT_MAY_GEN_EX) != 0;
	}

	/**
	 * (Intel VMX) {@code true} if it causes a VM exit in VMX non-root operation
	 */
	public boolean getIntelVmExit() {
		return (opcFlags2 & OpCodeInfoFlags2.INTEL_VM_EXIT) != 0;
	}

	/**
	 * (Intel VMX) {@code true} if it may cause a VM exit in VMX non-root operation
	 */
	public boolean getIntelMayVmExit() {
		return (opcFlags2 & OpCodeInfoFlags2.INTEL_MAY_VM_EXIT) != 0;
	}

	/**
	 * (Intel VMX) {@code true} if it causes an SMM VM exit in VMX root operation (if dual-monitor treatment is activated)
	 */
	public boolean getIntelSmmVmExit() {
		return (opcFlags2 & OpCodeInfoFlags2.INTEL_SMM_VM_EXIT) != 0;
	}

	/**
	 * (AMD SVM) {@code true} if it causes a #VMEXIT in guest mode
	 */
	public boolean getAmdVmExit() {
		return (opcFlags2 & OpCodeInfoFlags2.AMD_VM_EXIT) != 0;
	}

	/**
	 * (AMD SVM) {@code true} if it may cause a #VMEXIT in guest mode
	 */
	public boolean getAmdMayVmExit() {
		return (opcFlags2 & OpCodeInfoFlags2.AMD_MAY_VM_EXIT) != 0;
	}

	/**
	 * {@code true} if it causes a TSX abort inside a TSX transaction
	 */
	public boolean getTsxAbort() {
		return (opcFlags2 & OpCodeInfoFlags2.TSX_ABORT) != 0;
	}

	/**
	 * {@code true} if it causes a TSX abort inside a TSX transaction depending on the implementation
	 */
	public boolean getTsxImplAbort() {
		return (opcFlags2 & OpCodeInfoFlags2.TSX_IMPL_ABORT) != 0;
	}

	/**
	 * {@code true} if it may cause a TSX abort inside a TSX transaction depending on some condition
	 */
	public boolean getTsxMayAbort() {
		return (opcFlags2 & OpCodeInfoFlags2.TSX_MAY_ABORT) != 0;
	}

	/**
	 * {@code true} if it's decoded by iced's 16-bit Intel decoder
	 */
	public boolean getIntelDecoder16() {
		return (opcFlags2 & OpCodeInfoFlags2.INTEL_DECODER16OR32) != 0;
	}

	/**
	 * {@code true} if it's decoded by iced's 32-bit Intel decoder
	 */
	public boolean getIntelDecoder32() {
		return (opcFlags2 & OpCodeInfoFlags2.INTEL_DECODER16OR32) != 0;
	}

	/**
	 * {@code true} if it's decoded by iced's 64-bit Intel decoder
	 */
	public boolean getIntelDecoder64() {
		return (opcFlags2 & OpCodeInfoFlags2.INTEL_DECODER64) != 0;
	}

	/**
	 * {@code true} if it's decoded by iced's 16-bit AMD decoder
	 */
	public boolean getAmdDecoder16() {
		return (opcFlags2 & OpCodeInfoFlags2.AMD_DECODER16OR32) != 0;
	}

	/**
	 * {@code true} if it's decoded by iced's 32-bit AMD decoder
	 */
	public boolean getAmdDecoder32() {
		return (opcFlags2 & OpCodeInfoFlags2.AMD_DECODER16OR32) != 0;
	}

	/**
	 * {@code true} if it's decoded by iced's 64-bit AMD decoder
	 */
	public boolean getAmdDecoder64() {
		return (opcFlags2 & OpCodeInfoFlags2.AMD_DECODER64) != 0;
	}

	private static final int[] toDecoderOptions = new int[] {
		// GENERATOR-BEGIN: ToDecoderOptionsTable
		// ⚠️This was generated by GENERATOR!🦹‍♂️
		DecoderOptions.NONE,
		DecoderOptions.ALTINST,
		DecoderOptions.CL1INVMB,
		DecoderOptions.CMPXCHG486A,
		DecoderOptions.CYRIX,
		DecoderOptions.CYRIX_DMI,
		DecoderOptions.CYRIX_SMINT_0F7E,
		DecoderOptions.JMPE,
		DecoderOptions.LOADALL286,
		DecoderOptions.LOADALL386,
		DecoderOptions.MOV_TR,
		DecoderOptions.MPX,
		DecoderOptions.OLD_FPU,
		DecoderOptions.PCOMMIT,
		DecoderOptions.UMOV,
		DecoderOptions.XBTS,
		DecoderOptions.UDBG,
		DecoderOptions.KNC,
		// GENERATOR-END: ToDecoderOptionsTable
	};

	/**
	 * Gets the decoder option (a {@link DecoderOptions} flags value) that's needed to decode the instruction or {@link DecoderOptions#NONE}
	 */
	public int getDecoderOption() {
		return toDecoderOptions[((opcFlags1 >>> OpCodeInfoFlags1.DEC_OPTION_VALUE_SHIFT) & OpCodeInfoFlags1.DEC_OPTION_VALUE_MASK)];
	}

	/**
	 * Gets the opcode table (an {@link OpCodeTableKind} enum variant)
	 */
	public int getTable() {
		return table;
	}

	/**
	 * Gets the mandatory prefix (a {@link MandatoryPrefix} enum variant)
	 */
	public int getMandatoryPrefix() {
		return mandatoryPrefix;
	}

	/**
	 * Gets the opcode byte(s). The low byte(s) of this value is the opcode. The length is in {@link #getOpCodeLength()}.
	 * It doesn't include the table value, see {@link #getTable}.
	 * <p>
	 * Example values: {@code 0xDFC0} ({@link Code#FFREEP_STI}), {@code 0x01D8} ({@link Code#VMRUNW}), {@code 0x2A}
	 * ({@link Code#SUB_R8_RM8}, {@link Code#CVTPI2PS_XMM_MMM64}, etc).
	 */
	public int getOpCode() {
		return (encFlags2 >>> EncFlags2.OP_CODE_SHIFT) & 0xFFFF;
	}

	/**
	 * Gets the length of the opcode bytes ({@link #getOpCode()}). The low bytes is the opcode value.
	 */
	public int getOpCodeLength() {
		return (encFlags2 & EncFlags2.OP_CODE_IS2_BYTES) != 0 ? 2 : 1;
	}

	/**
	 * {@code true} if it's part of a group
	 */
	public boolean isGroup() {
		return groupIndex >= 0;
	}

	/**
	 * Group index (0-7) or -1. If it's 0-7, it's stored in the {@code reg} field of the {@code modrm} byte.
	 */
	public int getGroupIndex() {
		return groupIndex;
	}

	/**
	 * {@code true} if it's part of a {@code modrm.rm} group
	 */
	public boolean isRmGroup() {
		return rmGroupIndex >= 0;
	}

	/**
	 * {@code modrm.rm} group index (0-7) or -1. If it's 0-7, it's stored in the {@code rm} field of the {@code modrm} byte.
	 */
	public int getRmGroupIndex() {
		return rmGroupIndex;
	}

	/**
	 * Gets the number of operands
	 */
	public int getOpCount() {
		return InstructionOpCounts.opCount[code];
	}

	/**
	 * Gets operand #0's opkind (an {@link com.github.icedland.iced.x86.info.OpCodeOperandKind} enum variant)
	 */
	public int getOp0Kind() {
		return op0Kind;
	}

	/**
	 * Gets operand #1's opkind (an {@link com.github.icedland.iced.x86.info.OpCodeOperandKind} enum variant)
	 */
	public int getOp1Kind() {
		return op1Kind;
	}

	/**
	 * Gets operand #2's opkind (an {@link com.github.icedland.iced.x86.info.OpCodeOperandKind} enum variant)
	 */
	public int getOp2Kind() {
		return op2Kind;
	}

	/**
	 * Gets operand #3's opkind (an {@link com.github.icedland.iced.x86.info.OpCodeOperandKind} enum variant)
	 */
	public int getOp3Kind() {
		return op3Kind;
	}

	/**
	 * Gets operand #4's opkind (an {@link com.github.icedland.iced.x86.info.OpCodeOperandKind} enum variant)
	 */
	public int getOp4Kind() {
		return op4Kind;
	}

	/**
	 * Gets an operand's opkind (an {@link com.github.icedland.iced.x86.info.OpCodeOperandKind} enum variant)
	 *
	 * @param operand Operand number, 0-4
	 */
	public int getOpKind(int operand) {
		switch (operand) {
		case 0:
			return getOp0Kind();
		case 1:
			return getOp1Kind();
		case 2:
			return getOp2Kind();
		case 3:
			return getOp3Kind();
		case 4:
			return getOp4Kind();
		default:
			throw new IllegalArgumentException("operand");
		}
	}

	/**
	 * Checks if the instruction is available in 16-bit mode, 32-bit mode or 64-bit mode
	 *
	 * @param bitness 16, 32 or 64
	 */
	public boolean isAvailableInMode(int bitness) {
		switch (bitness) {
		case 16:
			return getMode16();
		case 32:
			return getMode32();
		case 64:
			return getMode64();
		default:
			throw new IllegalArgumentException("bitness");
		}
	}

	/**
	 * Gets the opcode string, eg.<!-- --> {@code VEX.128.66.0F38.W0 78 /r}
	 *
	 * @see #toInstructionString()
	 */
	public String toOpCodeString() {
		return toOpCodeStringValue;
	}

	/**
	 * Gets the instruction string, eg.<!-- --> {@code VPBROADCASTB xmm1, xmm2/m8}
	 *
	 * @see #toOpCodeString()
	 */
	public String toInstructionString() {
		return toInstructionStringValue;
	}

	/**
	 * Gets the instruction string, eg.<!-- --> {@code VPBROADCASTB xmm1, xmm2/m8}
	 *
	 * @see #toOpCodeString()
	 */
	@Override
	public String toString() {
		return toInstructionString();
	}
}
