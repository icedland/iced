// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Memory operand
 */
public final class MemoryOperand {
	/**
	 * Segment override or <code>null</code>
	 */
	public final ICRegister segmentPrefix;

	/**
	 * Base register or <code>null</code>
	 */
	public final ICRegister base;

	/**
	 * Index register or <code>null</code>
	 */
	public final ICRegister index;

	/**
	 * Index register scale (1, 2, 4, or 8)
	 */
	public final int scale;

	/**
	 * Memory displacement
	 */
	public final long displacement;

	/**
	 * 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	 */
	public final int displSize;

	/**
	 * <code>true</code> if it's broadcast memory (EVEX instructions)
	 */
	public final boolean isBroadcast;

	/**
	 * Constructor
	 *
	 * @param base          Base register or <code>null</code>
	 * @param index         Index register or <code>null</code>
	 * @param scale         Index register scale (1, 2, 4, or 8)
	 * @param displacement  Memory displacement
	 * @param displSize     0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                      (64-bit)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override or <code>null</code>
	 */
	public MemoryOperand(ICRegister base, ICRegister index, int scale, long displacement, int displSize, boolean isBroadcast,
			ICRegister segmentPrefix) {
		if (base == null)
			base = ICRegister.NONE;
		if (index == null)
			index = ICRegister.NONE;
		if (segmentPrefix == null)
			segmentPrefix = ICRegister.NONE;
		this.segmentPrefix = segmentPrefix;
		this.base = base;
		this.index = index;
		this.scale = scale;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param base          Base register or <code>null</code>
	 * @param index         Index register or <code>null</code>
	 * @param scale         Index register scale (1, 2, 4, or 8)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override or <code>null</code>
	 */
	public MemoryOperand(ICRegister base, ICRegister index, int scale, boolean isBroadcast, ICRegister segmentPrefix) {
		if (base == null)
			base = ICRegister.NONE;
		if (index == null)
			index = ICRegister.NONE;
		if (segmentPrefix == null)
			segmentPrefix = ICRegister.NONE;
		this.segmentPrefix = segmentPrefix;
		this.base = base;
		this.index = index;
		this.scale = scale;
		this.displacement = 0;
		this.displSize = 0;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param base          Base register or <code>null</code>
	 * @param displacement  Memory displacement
	 * @param displSize     0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                      (64-bit)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override or <code>null</code>
	 */
	public MemoryOperand(ICRegister base, long displacement, int displSize, boolean isBroadcast, ICRegister segmentPrefix) {
		if (base == null)
			base = ICRegister.NONE;
		if (segmentPrefix == null)
			segmentPrefix = ICRegister.NONE;
		this.segmentPrefix = segmentPrefix;
		this.base = base;
		this.index = ICRegister.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param index         Index register or <code>null</code>
	 * @param scale         Index register scale (1, 2, 4, or 8)
	 * @param displacement  Memory displacement
	 * @param displSize     0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                      (64-bit)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override or <code>null</code>
	 */
	public MemoryOperand(ICRegister index, int scale, long displacement, int displSize, boolean isBroadcast, ICRegister segmentPrefix) {
		if (index == null)
			index = ICRegister.NONE;
		if (segmentPrefix == null)
			segmentPrefix = ICRegister.NONE;
		this.segmentPrefix = segmentPrefix;
		this.base = ICRegister.NONE;
		this.index = index;
		this.scale = scale;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param base          Base register or <code>null</code>
	 * @param displacement  Memory displacement
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override or <code>null</code>
	 */
	public MemoryOperand(ICRegister base, long displacement, boolean isBroadcast, ICRegister segmentPrefix) {
		if (base == null)
			base = ICRegister.NONE;
		if (segmentPrefix == null)
			segmentPrefix = ICRegister.NONE;
		this.segmentPrefix = segmentPrefix;
		this.base = base;
		this.index = ICRegister.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = 1;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param base         Base register or <code>null</code>
	 * @param index        Index register or <code>null</code>
	 * @param scale        Index register scale (1, 2, 4, or 8)
	 * @param displacement Memory displacement
	 * @param displSize    0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                     (64-bit)
	 */
	public MemoryOperand(ICRegister base, ICRegister index, int scale, long displacement, int displSize) {
		if (base == null)
			base = ICRegister.NONE;
		if (index == null)
			index = ICRegister.NONE;
		this.segmentPrefix = ICRegister.NONE;
		this.base = base;
		this.index = index;
		this.scale = scale;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param base  Base register or <code>null</code>
	 * @param index Index register or <code>null</code>
	 * @param scale Index register scale (1, 2, 4, or 8)
	 */
	public MemoryOperand(ICRegister base, ICRegister index, int scale) {
		if (base == null)
			base = ICRegister.NONE;
		if (index == null)
			index = ICRegister.NONE;
		this.segmentPrefix = ICRegister.NONE;
		this.base = base;
		this.index = index;
		this.scale = scale;
		this.displacement = 0;
		this.displSize = 0;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param base  Base register or <code>null</code>
	 * @param index Index register or <code>null</code>
	 */
	public MemoryOperand(ICRegister base, ICRegister index) {
		if (base == null)
			base = ICRegister.NONE;
		if (index == null)
			index = ICRegister.NONE;
		this.segmentPrefix = ICRegister.NONE;
		this.base = base;
		this.index = index;
		this.scale = 1;
		this.displacement = 0;
		this.displSize = 0;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param base         Base register or <code>null</code>
	 * @param displacement Memory displacement
	 * @param displSize    0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                     (64-bit)
	 */
	public MemoryOperand(ICRegister base, long displacement, int displSize) {
		if (base == null)
			base = ICRegister.NONE;
		this.segmentPrefix = ICRegister.NONE;
		this.base = base;
		this.index = ICRegister.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param index        Index register or <code>null</code>
	 * @param scale        Index register scale (1, 2, 4, or 8)
	 * @param displacement Memory displacement
	 * @param displSize    0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                     (64-bit)
	 */
	public MemoryOperand(ICRegister index, int scale, long displacement, int displSize) {
		if (index == null)
			index = ICRegister.NONE;
		this.segmentPrefix = ICRegister.NONE;
		this.base = ICRegister.NONE;
		this.index = index;
		this.scale = scale;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param base         Base register or <code>null</code>
	 * @param displacement Memory displacement
	 */
	public MemoryOperand(ICRegister base, long displacement) {
		if (base == null)
			base = ICRegister.NONE;
		this.segmentPrefix = ICRegister.NONE;
		this.base = base;
		this.index = ICRegister.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = 1;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param base Base register or <code>null</code>
	 */
	public MemoryOperand(ICRegister base) {
		if (base == null)
			base = ICRegister.NONE;
		this.segmentPrefix = ICRegister.NONE;
		this.base = base;
		this.index = ICRegister.NONE;
		this.scale = 1;
		this.displacement = 0;
		this.displSize = 0;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param displacement Memory displacement
	 * @param displSize    0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                     (64-bit)
	 */
	public MemoryOperand(long displacement, int displSize) {
		this.segmentPrefix = ICRegister.NONE;
		this.base = ICRegister.NONE;
		this.index = ICRegister.NONE;
		this.scale = 1;
		this.displacement = (long)displacement;
		this.displSize = displSize;
		this.isBroadcast = false;
	}
}
