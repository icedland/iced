// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER
namespace Iced.Intel {
	/// <summary>
	/// Memory operand
	/// </summary>
	public readonly struct MemoryOperand {
		/// <summary>
		/// Segment override or <see cref="Register.None"/>
		/// </summary>
		public readonly Register SegmentPrefix;

		/// <summary>
		/// Base register or <see cref="Register.None"/>
		/// </summary>
		public readonly Register Base;

		/// <summary>
		/// Index register or <see cref="Register.None"/>
		/// </summary>
		public readonly Register Index;

		/// <summary>
		/// Index register scale (1, 2, 4, or 8)
		/// </summary>
		public readonly int Scale;

		/// <summary>
		/// Memory displacement
		/// </summary>
		public readonly long Displacement;

		/// <summary>
		/// 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
		/// </summary>
		public readonly int DisplSize;

		/// <summary>
		/// <see langword="true"/> if it's broadcast memory (EVEX instructions)
		/// </summary>
		public readonly bool IsBroadcast;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="displSize">0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		/// <param name="isBroadcast"><see langword="true"/> if it's broadcast memory (EVEX instructions)</param>
		/// <param name="segmentPrefix">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, Register index, int scale, long displacement, int displSize, bool isBroadcast, Register segmentPrefix) {
			SegmentPrefix = segmentPrefix;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			IsBroadcast = isBroadcast;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="isBroadcast"><see langword="true"/> if it's broadcast memory (EVEX instructions)</param>
		/// <param name="segmentPrefix">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, Register index, int scale, bool isBroadcast, Register segmentPrefix) {
			SegmentPrefix = segmentPrefix;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = 0;
			DisplSize = 0;
			IsBroadcast = isBroadcast;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="displSize">0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		/// <param name="isBroadcast"><see langword="true"/> if it's broadcast memory (EVEX instructions)</param>
		/// <param name="segmentPrefix">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, long displacement, int displSize, bool isBroadcast, Register segmentPrefix) {
			SegmentPrefix = segmentPrefix;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = displSize;
			IsBroadcast = isBroadcast;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="displSize">0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		/// <param name="isBroadcast"><see langword="true"/> if it's broadcast memory (EVEX instructions)</param>
		/// <param name="segmentPrefix">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register index, int scale, long displacement, int displSize, bool isBroadcast, Register segmentPrefix) {
			SegmentPrefix = segmentPrefix;
			Base = Register.None;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			IsBroadcast = isBroadcast;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="isBroadcast"><see langword="true"/> if it's broadcast memory (EVEX instructions)</param>
		/// <param name="segmentPrefix">Segment override or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, long displacement, bool isBroadcast, Register segmentPrefix) {
			SegmentPrefix = segmentPrefix;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = 1;
			IsBroadcast = isBroadcast;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="displSize">0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		public MemoryOperand(Register @base, Register index, int scale, long displacement, int displSize) {
			SegmentPrefix = Register.None;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			IsBroadcast = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		public MemoryOperand(Register @base, Register index, int scale) {
			SegmentPrefix = Register.None;
			Base = @base;
			Index = index;
			Scale = scale;
			Displacement = 0;
			DisplSize = 0;
			IsBroadcast = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base, Register index) {
			SegmentPrefix = Register.None;
			Base = @base;
			Index = index;
			Scale = 1;
			Displacement = 0;
			DisplSize = 0;
			IsBroadcast = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="displSize">0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		public MemoryOperand(Register @base, long displacement, int displSize) {
			SegmentPrefix = Register.None;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = displSize;
			IsBroadcast = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index">Index register or <see cref="Register.None"/></param>
		/// <param name="scale">Index register scale (1, 2, 4, or 8)</param>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="displSize">0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		public MemoryOperand(Register index, int scale, long displacement, int displSize) {
			SegmentPrefix = Register.None;
			Base = Register.None;
			Index = index;
			Scale = scale;
			Displacement = displacement;
			DisplSize = displSize;
			IsBroadcast = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		/// <param name="displacement">Memory displacement</param>
		public MemoryOperand(Register @base, long displacement) {
			SegmentPrefix = Register.None;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = displacement;
			DisplSize = 1;
			IsBroadcast = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="base">Base register or <see cref="Register.None"/></param>
		public MemoryOperand(Register @base) {
			SegmentPrefix = Register.None;
			Base = @base;
			Index = Register.None;
			Scale = 1;
			Displacement = 0;
			DisplSize = 0;
			IsBroadcast = false;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="displacement">Memory displacement</param>
		/// <param name="displSize">0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a <see cref="sbyte"/>), 2 (16-bit), 4 (32-bit) or 8 (64-bit)</param>
		public MemoryOperand(ulong displacement, int displSize) {
			SegmentPrefix = Register.None;
			Base = Register.None;
			Index = Register.None;
			Scale = 1;
			Displacement = (long)displacement;
			DisplSize = displSize;
			IsBroadcast = false;
		}
	}
}
#endif
