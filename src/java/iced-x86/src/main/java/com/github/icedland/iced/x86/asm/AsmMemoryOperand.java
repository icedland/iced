// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import com.github.icedland.iced.x86.ICRegister;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.MemoryOperand;
import com.github.icedland.iced.x86.Register;

/**
 * A memory operand passed to {@link CodeAssembler} methods
 */
public final class AsmMemoryOperand {
	AsmMemoryOperand(int size, ICRegister segment, ICRegister base, ICRegister index, int scale, long displacement, int flags) {
		if (segment == null)
			segment = ICRegister.NONE;
		if (base == null)
			base = ICRegister.NONE;
		if (index == null)
			index = ICRegister.NONE;
		this.size = size;
		this.segment = segment;
		this.base = base;
		this.index = index;
		this.scale = scale;
		this.displacement = displacement;
		this.flags = flags;
	}

	/**
	 * Size of the operand (a {@link MemoryOperandSize} enum variant)
	 */
	final int size;

	/**
	 * Segment register
	 */
	public final ICRegister segment;

	/**
	 * Base register
	 */
	public final ICRegister base;

	/**
	 * Index register
	 */
	public final ICRegister index;

	/**
	 * Scale (1, 2, 4 or 8)
	 */
	public final int scale;

	/**
	 * Displacement
	 */
	public final long displacement;

	/**
	 * Flags (a {@link AsmOperandFlags} flags value)
	 */
	final int flags;

	/**
	 * {@code true} if memory is broadcast
	 */
	public boolean isBroadcast() {
		return (flags & AsmOperandFlags.BROADCAST) != 0;
	}

	boolean isDisplacementOnly() {
		return base.get() == Register.NONE && index.get() == Register.NONE;
	}

	/**
	 * Apply op mask register {@code K1}
	 */
	public AsmMemoryOperand k1() {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K1);
	}

	/**
	 * Apply op mask register {@code K2}
	 */
	public AsmMemoryOperand k2() {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K2);
	}

	/**
	 * Apply op mask register {@code K3}
	 */
	public AsmMemoryOperand k3() {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K3);
	}

	/**
	 * Apply op mask register {@code K4}
	 */
	public AsmMemoryOperand k4() {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K4);
	}

	/**
	 * Apply op mask register {@code K5}
	 */
	public AsmMemoryOperand k5() {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K5);
	}

	/**
	 * Apply op mask register {@code K6}
	 */
	public AsmMemoryOperand k6() {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K6);
	}

	/**
	 * Apply op mask register {@code K7}
	 */
	public AsmMemoryOperand k7() {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, (flags & ~AsmOperandFlags.REGISTER_MASK) | AsmOperandFlags.K7);
	}

	/**
	 * Changes segment to {@code ES:}
	 */
	public AsmMemoryOperand es() {
		return new AsmMemoryOperand(size, ICRegisters.es, base, index, scale, displacement, flags);
	}

	/**
	 * Changes segment to {@code CS:}
	 */
	public AsmMemoryOperand cs() {
		return new AsmMemoryOperand(size, ICRegisters.cs, base, index, scale, displacement, flags);
	}

	/**
	 * Changes segment to {@code SS:}
	 */
	public AsmMemoryOperand ss() {
		return new AsmMemoryOperand(size, ICRegisters.ss, base, index, scale, displacement, flags);
	}

	/**
	 * Changes segment to {@code DS:}
	 */
	public AsmMemoryOperand ds() {
		return new AsmMemoryOperand(size, ICRegisters.ds, base, index, scale, displacement, flags);
	}

	/**
	 * Changes segment to {@code FS:}
	 */
	public AsmMemoryOperand fs() {
		return new AsmMemoryOperand(size, ICRegisters.fs, base, index, scale, displacement, flags);
	}

	/**
	 * Changes segment to {@code GS:}
	 */
	public AsmMemoryOperand gs() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement, flags);
	}

	/**
	 * Changes memory operand to a broadcast memory operand
	 */
	public AsmMemoryOperand broadcast() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement, flags | AsmOperandFlags.BROADCAST);
	}

	/**
	 * Enables zeroing masking (`{z}`)
	 */
	public AsmMemoryOperand z() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement, flags | AsmOperandFlags.ZEROING);
	}

	/**
	 * Enables suppress all exceptions (`{sae}`)
	 */
	public AsmMemoryOperand sae() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement, flags | AsmOperandFlags.SUPPRESS_ALL_EXCEPTIONS);
	}

	/**
	 * Enables round-to-nearest (`{rn-sae}`)
	 */
	public AsmMemoryOperand rn_sae() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement,
				(flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_TO_NEAREST);
	}

	/**
	 * Enables round-down (`{rd-sae}`)
	 */
	public AsmMemoryOperand rd_sae() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement,
				(flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_DOWN);
	}

	/**
	 * Enables round-up (`{ru-sae}`)
	 */
	public AsmMemoryOperand ru_sae() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement,
				(flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_UP);
	}

	/**
	 * Enables round-toward-zero (`{rz-sae}`)
	 */
	public AsmMemoryOperand rz_sae() {
		return new AsmMemoryOperand(size, ICRegisters.gs, base, index, scale, displacement,
				(flags & ~AsmOperandFlags.ROUNDING_CONTROL_MASK) | AsmOperandFlags.ROUND_TOWARD_ZERO);
	}

	/**
	 * Adds a displacement to a memory operand.
	 *
	 * @param displacement displacement.
	 */
	public AsmMemoryOperand add(long displacement) {
		return new AsmMemoryOperand(size, segment, base, index, scale, this.displacement + displacement, flags);
	}

	/**
	 * Subtracts a displacement from a memory operand.
	 *
	 * @param displacement displacement.
	 */
	public AsmMemoryOperand sub(long displacement) {
		return new AsmMemoryOperand(size, segment, base, index, scale, this.displacement - displacement, flags);
	}

	/**
	 * Adds a base register (if no base reg) or an index register (if base register but no index register)
	 *
	 * @param reg Register (base or index)
	 */
	public AsmMemoryOperand add(AsmRegister16 reg) {
		return add(reg.get());
	}

	/**
	 * Adds a base register (if no base reg) or an index register (if base register but no index register)
	 *
	 * @param reg Register (base or index)
	 */
	public AsmMemoryOperand add(AsmRegister32 reg) {
		return add(reg.get());
	}

	/**
	 * Adds a base register (if no base reg) or an index register (if base register but no index register)
	 *
	 * @param reg Register (base or index)
	 */
	public AsmMemoryOperand add(AsmRegister64 reg) {
		return add(reg.get());
	}

	/**
	 * Adds a base register (if no base reg) or an index register (if base register but no index register)
	 *
	 * @param reg Register (base or index)
	 */
	public AsmMemoryOperand add(AsmRegisterXMM reg) {
		return add(reg.get());
	}

	/**
	 * Adds a base register (if no base reg) or an index register (if base register but no index register)
	 *
	 * @param reg Register (base or index)
	 */
	public AsmMemoryOperand add(AsmRegisterYMM reg) {
		return add(reg.get());
	}

	/**
	 * Adds a base register (if no base reg) or an index register (if base register but no index register)
	 *
	 * @param reg Register (base or index)
	 */
	public AsmMemoryOperand add(AsmRegisterZMM reg) {
		return add(reg.get());
	}

	private AsmMemoryOperand add(ICRegister reg) {
		ICRegister base = this.base;
		ICRegister index = this.index;
		if (reg == null) {
			// Nothing
		} else if (base.get() == Register.NONE && !Register.isVectorRegister(reg.get()))
			base = reg;
		else if (index.get() == Register.NONE)
			index = reg;
		else
			throw new IllegalArgumentException("Trying to add an index register when base + index already exist");
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, flags);
	}

	/**
	 * Changes the segment register
	 *
	 * @param segment Segment register or {@code null}
	 */
	public AsmMemoryOperand segment(AsmRegisterSegment segment) {
		return new AsmMemoryOperand(size, segment.get(), base, index, scale, displacement, flags);
	}

	/**
	 * Changes the base register
	 *
	 * @param base Base register or {@code null}
	 */
	public AsmMemoryOperand base(AsmRegister16 base) {
		return new AsmMemoryOperand(size, segment, base.get(), index, scale, displacement, flags);
	}

	/**
	 * Changes the base register
	 *
	 * @param base Base register or {@code null}
	 */
	public AsmMemoryOperand base(AsmRegister32 base) {
		return new AsmMemoryOperand(size, segment, base.get(), index, scale, displacement, flags);
	}

	/**
	 * Changes the base register
	 *
	 * @param base Base register or {@code null}
	 */
	public AsmMemoryOperand base(AsmRegister64 base) {
		return new AsmMemoryOperand(size, segment, base.get(), index, scale, displacement, flags);
	}

	/**
	 * Changes the index register
	 *
	 * @param index Index register or {@code null}
	 */
	public AsmMemoryOperand index(AsmRegister16 index) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register
	 *
	 * @param index Index register or {@code null}
	 */
	public AsmMemoryOperand index(AsmRegister32 index) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register
	 *
	 * @param index Index register or {@code null}
	 */
	public AsmMemoryOperand index(AsmRegister64 index) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register
	 *
	 * @param index Index register or {@code null}
	 */
	public AsmMemoryOperand index(AsmRegisterXMM index) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register
	 *
	 * @param index Index register or {@code null}
	 */
	public AsmMemoryOperand index(AsmRegisterYMM index) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register
	 *
	 * @param index Index register or {@code null}
	 */
	public AsmMemoryOperand index(AsmRegisterZMM index) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register and scale
	 *
	 * @param index Index register or {@code null}
	 * @param scale Scale (1, 2, 4 or 8)
	 */
	public AsmMemoryOperand index(AsmRegister32 index, int scale) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register and scale
	 *
	 * @param index Index register or {@code null}
	 * @param scale Scale (1, 2, 4 or 8)
	 */
	public AsmMemoryOperand index(AsmRegister64 index, int scale) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register and scale
	 *
	 * @param index Index register or {@code null}
	 * @param scale Scale (1, 2, 4 or 8)
	 */
	public AsmMemoryOperand index(AsmRegisterXMM index, int scale) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register and scale
	 *
	 * @param index Index register or {@code null}
	 * @param scale Scale (1, 2, 4 or 8)
	 */
	public AsmMemoryOperand index(AsmRegisterYMM index, int scale) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the index register and scale
	 *
	 * @param index Index register or {@code null}
	 * @param scale Scale (1, 2, 4 or 8)
	 */
	public AsmMemoryOperand index(AsmRegisterZMM index, int scale) {
		return new AsmMemoryOperand(size, segment, base, index.get(), scale, displacement, flags);
	}

	/**
	 * Changes the scale value
	 *
	 * @param scale Scale (1, 2, 4 or 8)
	 */
	public AsmMemoryOperand scale(int scale) {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, flags);
	}

	/**
	 * Changes the displacement value
	 *
	 * @param displacement displacement
	 */
	public AsmMemoryOperand displacement(long displacement) {
		return new AsmMemoryOperand(size, segment, base, index, scale, displacement, flags);
	}

	/**
	 * Gets a memory operand for the specified bitness.
	 *
	 * @param bitness The bitness
	 */
	public MemoryOperand toMemoryOperand(int bitness) {
		int dispSize = 1;
		if (isDisplacementOnly())
			dispSize = bitness / 8;
		else if (displacement == 0)
			dispSize = 0;
		return new MemoryOperand(base, index, scale, displacement, dispSize, (flags & AsmOperandFlags.BROADCAST) != 0, segment);
	}

	/** Checks if this equals {@code obj} */
	@Override
	public boolean equals(Object obj) {
		if (obj == null || getClass() != obj.getClass())
			return false;
		AsmMemoryOperand other = (AsmMemoryOperand)obj;
		return size == other.size && segment == other.segment && base == other.base && index == other.index && scale == other.scale
				&& displacement == other.displacement && flags == other.flags;
	}

	/** Gets the hash code */
	@Override
	public int hashCode() {
		int hashCode = size;
		hashCode = (hashCode * 397) ^ segment.hashCode();
		hashCode = (hashCode * 397) ^ base.hashCode();
		hashCode = (hashCode * 397) ^ index.hashCode();
		hashCode = (hashCode * 397) ^ scale;
		hashCode = (hashCode * 397) ^ Long.hashCode(displacement);
		hashCode = (hashCode * 397) ^ flags;
		return hashCode;
	}
}
