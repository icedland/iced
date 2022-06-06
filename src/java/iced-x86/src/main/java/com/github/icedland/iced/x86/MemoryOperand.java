// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Memory operand
 */
public final class MemoryOperand {
	/**
	 * Segment override (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public final int segmentPrefix;

	/**
	 * Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public final int base;

	/**
	 * Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public final int index;

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
	 * @param base          Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param index         Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param scale         Index register scale (1, 2, 4, or 8)
	 * @param displacement  Memory displacement
	 * @param displSize     0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                      (64-bit)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public MemoryOperand(int base, int index, int scale, long displacement, int displSize, boolean isBroadcast, int segmentPrefix) {
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
	 * @param base          Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param index         Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param scale         Index register scale (1, 2, 4, or 8)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public MemoryOperand(int base, int index, int scale, boolean isBroadcast, int segmentPrefix) {
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
	 * @param base          Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param displacement  Memory displacement
	 * @param displSize     0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                      (64-bit)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public MemoryOperand(int base, long displacement, int displSize, boolean isBroadcast, int segmentPrefix) {
		this.segmentPrefix = segmentPrefix;
		this.base = base;
		this.index = Register.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param index         Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param scale         Index register scale (1, 2, 4, or 8)
	 * @param displacement  Memory displacement
	 * @param displSize     0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                      (64-bit)
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public MemoryOperand(int index, int scale, long displacement, int displSize, boolean isBroadcast, int segmentPrefix) {
		this.segmentPrefix = segmentPrefix;
		this.base = Register.NONE;
		this.index = index;
		this.scale = scale;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param base          Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param displacement  Memory displacement
	 * @param isBroadcast   <code>true</code> if it's broadcast memory (EVEX instructions)
	 * @param segmentPrefix Segment override (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public MemoryOperand(int base, long displacement, boolean isBroadcast, int segmentPrefix) {
		this.segmentPrefix = segmentPrefix;
		this.base = base;
		this.index = Register.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = 1;
		this.isBroadcast = isBroadcast;
	}

	/**
	 * Constructor
	 *
	 * @param base         Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param index        Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param scale        Index register scale (1, 2, 4, or 8)
	 * @param displacement Memory displacement
	 * @param displSize    0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                     (64-bit)
	 */
	public MemoryOperand(int base, int index, int scale, long displacement, int displSize) {
		this.segmentPrefix = Register.NONE;
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
	 * @param base  Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param index Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param scale Index register scale (1, 2, 4, or 8)
	 */
	public MemoryOperand(int base, int index, int scale) {
		this.segmentPrefix = Register.NONE;
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
	 * @param base  Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param index Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public MemoryOperand(int base, int index) {
		this.segmentPrefix = Register.NONE;
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
	 * @param base         Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param displacement Memory displacement
	 * @param displSize    0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                     (64-bit)
	 */
	public MemoryOperand(int base, long displacement, int displSize) {
		this.segmentPrefix = Register.NONE;
		this.base = base;
		this.index = Register.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param index        Index register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param scale        Index register scale (1, 2, 4, or 8)
	 * @param displacement Memory displacement
	 * @param displSize    0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <code>byte</code>), 2 (16-bit), 4 (32-bit) or 8
	 *                     (64-bit)
	 */
	public MemoryOperand(int index, int scale, long displacement, int displSize) {
		this.segmentPrefix = Register.NONE;
		this.base = Register.NONE;
		this.index = index;
		this.scale = scale;
		this.displacement = displacement;
		this.displSize = displSize;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param base         Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 * @param displacement Memory displacement
	 */
	public MemoryOperand(int base, long displacement) {
		this.segmentPrefix = Register.NONE;
		this.base = base;
		this.index = Register.NONE;
		this.scale = 1;
		this.displacement = displacement;
		this.displSize = 1;
		this.isBroadcast = false;
	}

	/**
	 * Constructor
	 *
	 * @param base Base register (a {@link Register} enum variant) or {@link Register#NONE}
	 */
	public MemoryOperand(int base) {
		this.segmentPrefix = Register.NONE;
		this.base = base;
		this.index = Register.NONE;
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
		this.segmentPrefix = Register.NONE;
		this.base = Register.NONE;
		this.index = Register.NONE;
		this.scale = 1;
		this.displacement = (long)displacement;
		this.displSize = displSize;
		this.isBroadcast = false;
	}
}
