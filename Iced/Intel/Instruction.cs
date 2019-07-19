/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Iced.Intel {
	/// <summary>
	/// A 16/32/64-bit instruction
	/// </summary>
	public partial struct Instruction : IEquatable<Instruction> {
		internal const int TEST_OpKindBits = (int)OpKindFlags.OpKindBits;
		internal const int TEST_CodeBits = (int)CodeFlags.CodeBits;
		internal const int TEST_RegisterBits = 8;

		/// <summary>
		/// [1:0]	= Scale
		/// [4:2]	= Size of displacement: 0, 1, 2, 4, 8
		/// [7:5]	= Segment register prefix: none, es, cs, ss, ds, fs, gs, reserved
		/// [14:8]	= Not used
		/// [15]	= Broadcasted memory
		/// </summary>
		[Flags]
		enum MemoryFlags : ushort {
			ScaleMask				= 3,
			DisplSizeShift			= 2,
			DisplSizeMask			= 7,
			SegmentPrefixShift		= 5,
			SegmentPrefixMask		= 7,
			// Unused bits here
			BroadcastedMemory		= 0x8000,
		}

		/// <summary>
		/// [4:0]	= Operand #0's <see cref="OpKind"/>
		/// [9:5]	= Operand #1's <see cref="OpKind"/>
		/// [14:10]	= Operand #2's <see cref="OpKind"/>
		/// [19:15]	= Operand #3's <see cref="OpKind"/>
		/// [23:20]	= db/dw/dd/dq element count (1-16, 1-8, 1-4, or 1-2)
		/// [29:24]	= Not used
		/// [31:30]	= CodeSize
		/// </summary>
		[Flags]
		enum OpKindFlags : uint {
			OpKindBits				= 5,
			OpKindMask				= (1 << (int)OpKindBits) - 1,
			Op1KindShift			= 5,
			Op2KindShift			= 10,
			Op3KindShift			= 15,
			DataLengthMask			= 0xF,
			DataLengthShift			= 20,
			// Unused bits here
			CodeSizeMask			= 3,
			CodeSizeShift			= 30,

			// Bits ignored by Equals()
			EqualsIgnoreMask		= CodeSizeMask << (int)CodeSizeShift,
		}

		/// <summary>
		/// [12:0]	= <see cref="Intel.Code"/>
		/// [15:13]	= <see cref="Intel.RoundingControl"/>
		/// [18:16]	= Opmask register or 0 if none
		/// [22:19]	= Instruction length
		/// [24:23]	= Not used
		/// [25]	= Suppress all exceptions
		/// [26]	= Zeroing masking
		/// [27]	= xacquire prefix
		/// [28]	= xrelease prefix
		/// [29]	= repe prefix
		/// [30]	= repne prefix
		/// [31]	= lock prefix
		/// </summary>
		[Flags]
		enum CodeFlags : uint {
			CodeBits				= 13,
			CodeMask				= (1 << (int)CodeBits) - 1,
			RoundingControlMask		= 7,
			RoundingControlShift	= 13,
			OpMaskMask				= 7,
			OpMaskShift				= 16,
			InstrLengthMask			= 0xF,
			InstrLengthShift		= 19,
			// Unused bits here
			SuppressAllExceptions	= 0x02000000,
			ZeroingMasking			= 0x04000000,
			XacquirePrefix			= 0x08000000,
			XreleasePrefix			= 0x10000000,
			RepePrefix				= 0x20000000,
			RepnePrefix				= 0x40000000,
			LockPrefix				= 0x80000000,

			// Bits ignored by Equals()
			EqualsIgnoreMask		= InstrLengthMask << (int)InstrLengthShift,
		}

		// All fields, size: 32 bytes with bits to spare
		internal const int TOTAL_SIZE = 32;
		// Next RIP is only needed by RIP relative memory operands. Without this field the user would have
		// to pass this value to the formatter and encoder methods.
		ulong nextRip;
		uint codeFlags;// CodeFlags
		uint opKindFlags;// OpKindFlags
		// If it's a 64-bit immediate/offset/target, the high 32 bits is in memDispl
		uint immediate;
		// This is the high 32 bits if it's a 64-bit immediate/offset/target
		uint memDispl;
		ushort memoryFlags;// MemoryFlags
		byte memBaseReg;// Register
		byte memIndexReg;// Register
		// If a Register will need 9 bits in the future, it's probably best to turn this into a
		// uint (and move it below the other uint fields above). The remaining 4 bits of 'reg3'
		// can be stored in some other field (it's rarely used)
		byte reg0, reg1, reg2, reg3;// Register

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool operator ==(in Instruction left, in Instruction right) => EqualsInternal(left, right);
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public static bool operator !=(in Instruction left, in Instruction right) => !EqualsInternal(left, right);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		/// <summary>
		/// Checks if this instance equals <paramref name="other"/>
		/// </summary>
		/// <param name="other">Other instruction</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		public bool Equals(in Instruction other) => EqualsInternal(this, other);
		bool IEquatable<Instruction>.Equals(Instruction other) => EqualsInternal(this, other);

		static bool EqualsInternal(in Instruction a, in Instruction b) =>
			((a.codeFlags ^ b.codeFlags) & ~(uint)CodeFlags.EqualsIgnoreMask) == 0 &&
			((a.opKindFlags ^ b.opKindFlags) & ~(uint)OpKindFlags.EqualsIgnoreMask) == 0 &&
			a.immediate == b.immediate &&
			a.memDispl == b.memDispl &&
			a.memoryFlags == b.memoryFlags &&
			a.memBaseReg == b.memBaseReg &&
			a.memIndexReg == b.memIndexReg &&
			a.reg0 == b.reg0 &&
			a.reg1 == b.reg1 &&
			a.reg2 == b.reg2 &&
			a.reg3 == b.reg3;

		/// <summary>
		/// Gets the hash code
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			uint c = codeFlags & ~(uint)CodeFlags.EqualsIgnoreMask;
			c ^= opKindFlags & ~(uint)OpKindFlags.EqualsIgnoreMask;
			c ^= immediate;
			c ^= memDispl;
			c ^= memoryFlags;
			c ^= (uint)memBaseReg << 16;
			c ^= (uint)memIndexReg << 24;
			c ^= reg3;
			c ^= (uint)reg2 << 8;
			c ^= (uint)reg1 << 16;
			c ^= (uint)reg0 << 24;
			return (int)c;
		}

		/// <summary>
		/// Checks if this instance equals <paramref name="obj"/>
		/// </summary>
		/// <param name="obj">Other instruction</param>
		/// <returns></returns>
		public override bool Equals(object obj) => obj is Instruction other && EqualsInternal(this, other);

		/// <summary>
		/// Checks if two instructions are equal, comparing all bits, not ignoring anything
		/// </summary>
		/// <param name="a">Instruction #1</param>
		/// <param name="b">Instruction #2</param>
		/// <returns></returns>
		public static bool EqualsAllBits(in Instruction a, in Instruction b) =>
			a.codeFlags == b.codeFlags &&
			a.opKindFlags == b.opKindFlags &&
			a.immediate == b.immediate &&
			a.memDispl == b.memDispl &&
			a.memoryFlags == b.memoryFlags &&
			a.memBaseReg == b.memBaseReg &&
			a.memIndexReg == b.memIndexReg &&
			a.reg0 == b.reg0 &&
			a.reg1 == b.reg1 &&
			a.reg2 == b.reg2 &&
			a.reg3 == b.reg3 &&
			a.nextRip == b.nextRip;

		internal static bool TEST_BitByBitEquals(in Instruction a, in Instruction b) => EqualsAllBits(a, b);

		internal static string TEST_DumpDiff(in Instruction a, in Instruction b) {
			var builder = new StringBuilder();
			if (a.nextRip != b.nextRip)
				builder.AppendLine($"a.nextRip={a.nextRip:X16} b.nextRip={b.nextRip:X16}");
			if (a.codeFlags != b.codeFlags)
				builder.AppendLine($"a.codeFlags={a.codeFlags:X} b.codeFlags={b.codeFlags:X}");
			if (a.opKindFlags != b.opKindFlags)
				builder.AppendLine($"a.opKindFlags={a.opKindFlags:X} b.opKindFlags={b.opKindFlags:X}");
			if (a.immediate != b.immediate)
				builder.AppendLine($"a.immediate={a.immediate:X} b.immediate={b.immediate:X}");
			if (a.memDispl != b.memDispl)
				builder.AppendLine($"a.memDispl={a.memDispl:X} b.memDispl={b.memDispl:X}");
			if (a.memoryFlags != b.memoryFlags)
				builder.AppendLine($"a.memoryFlags={a.memoryFlags:X} b.memoryFlags={b.memoryFlags:X}");
			if (a.memBaseReg != b.memBaseReg)
				builder.AppendLine($"a.MemoryBase={(Register)a.memBaseReg} b.MemoryBase={(Register)b.memBaseReg}");
			if (a.memIndexReg != b.memIndexReg)
				builder.AppendLine($"a.MemoryIndex={(Register)a.memIndexReg} b.MemoryIndex={(Register)b.memIndexReg}");
			if (a.reg0 != b.reg0)
				builder.AppendLine($"a.Op0Register={(Register)a.reg0} b.Op0Register={(Register)b.reg0}");
			if (a.reg1 != b.reg1)
				builder.AppendLine($"a.Op1Register={(Register)a.reg1} b.Op1Register={(Register)b.reg1}");
			if (a.reg2 != b.reg2)
				builder.AppendLine($"a.Op2Register={(Register)a.reg2} b.Op2Register={(Register)b.reg2}");
			if (a.reg3 != b.reg3)
				builder.AppendLine($"a.Op3Register={(Register)a.reg3} b.Op3Register={(Register)b.reg3}");
			return builder.ToString();
		}

		/// <summary>
		/// 16-bit IP of the instruction
		/// </summary>
		public ushort IP16 {
			get => (ushort)((uint)nextRip - (uint)ByteLength);
			set => nextRip = value + (uint)ByteLength;
		}

		/// <summary>
		/// 32-bit IP of the instruction
		/// </summary>
		public uint IP32 {
			get => (uint)nextRip - (uint)ByteLength;
			set => nextRip = value + (uint)ByteLength;
		}

		/// <summary>
		/// 64-bit IP of the instruction
		/// </summary>
		[Obsolete("Use " + nameof(IP) + " instead of this property", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ulong IP64 {
			get => nextRip - (uint)ByteLength;
			set => nextRip = value + (uint)ByteLength;
		}

		/// <summary>
		/// 64-bit IP of the instruction
		/// </summary>
		public ulong IP {
			get => nextRip - (uint)ByteLength;
			set => nextRip = value + (uint)ByteLength;
		}

		/// <summary>
		/// 16-bit IP of the next instruction
		/// </summary>
		public ushort NextIP16 {
			get => (ushort)nextRip;
			set => nextRip = value;
		}

		/// <summary>
		/// 32-bit IP of the next instruction
		/// </summary>
		public uint NextIP32 {
			get => (uint)nextRip;
			set => nextRip = value;
		}

		/// <summary>
		/// 64-bit IP of the next instruction
		/// </summary>
		[Obsolete("Use " + nameof(NextIP) + " instead of this property", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ulong NextIP64 {
			get => nextRip;
			set => nextRip = value;
		}

		/// <summary>
		/// 64-bit IP of the next instruction
		/// </summary>
		public ulong NextIP {
			get => nextRip;
			set => nextRip = value;
		}

		/// <summary>
		/// Gets the code size when the instruction was decoded. This value is informational and can
		/// be used by a formatter.
		/// </summary>
		public CodeSize CodeSize {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (CodeSize)((opKindFlags >> (int)OpKindFlags.CodeSizeShift) & (uint)OpKindFlags.CodeSizeMask);
			set => opKindFlags = ((opKindFlags & ~((uint)OpKindFlags.CodeSizeMask << (int)OpKindFlags.CodeSizeShift)) |
				(((uint)value & (uint)OpKindFlags.CodeSizeMask) << (int)OpKindFlags.CodeSizeShift));
		}
		internal CodeSize InternalCodeSize {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags |= ((uint)value << (int)OpKindFlags.CodeSizeShift);
		}

		/// <summary>
		/// Instruction code
		/// </summary>
		public Code Code {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (Code)(codeFlags & (uint)CodeFlags.CodeMask);
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set {
				if ((uint)value >= (uint)DecoderConstants.NumberOfCodeValues)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
				codeFlags = (codeFlags & ~(uint)CodeFlags.CodeMask) | (uint)value;
			}
		}
		internal Code InternalCode {
			// x86 jitter doesn't always inline some of these props that should be inlined. Force it.
			// RyuJIT seems to be better and doesn't seem to require force-inline attrs.
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => codeFlags |= (uint)value;
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void SetCodeNoCheck(Code code) =>
			codeFlags = (codeFlags & ~(uint)CodeFlags.CodeMask) | (uint)code;

		/// <summary>
		/// Gets the mnemonic
		/// </summary>
		public Mnemonic Mnemonic {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => Code.ToMnemonic();
		}

		/// <summary>
		/// Gets the operand count. Up to 5 operands is allowed.
		/// </summary>
		public int OpCount {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => InstructionOpCounts.OpCount[(int)(codeFlags & (uint)CodeFlags.CodeMask)];
		}

		/// <summary>
		/// Gets the length of the instruction, 0-15 bytes. This is just informational. If you modify the instruction
		/// or create a new one, this property could return the wrong value.
		/// </summary>
		public int ByteLength {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (int)((codeFlags >> (int)CodeFlags.InstrLengthShift) & (uint)CodeFlags.InstrLengthMask);
			set => codeFlags = (codeFlags & ~((uint)CodeFlags.InstrLengthMask << (int)CodeFlags.InstrLengthShift)) |
				(((uint)value & (uint)CodeFlags.InstrLengthMask) << (int)CodeFlags.InstrLengthShift);
		}
		internal uint InternalByteLength {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => codeFlags |= (value << (int)CodeFlags.InstrLengthShift);
		}

		internal bool Internal_HasRepePrefix_HasXreleasePrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)(CodeFlags.RepePrefix | CodeFlags.XreleasePrefix)) != 0;
		}
		internal bool Internal_HasRepnePrefix_HasXacquirePrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)(CodeFlags.RepnePrefix | CodeFlags.XacquirePrefix)) != 0;
		}
		internal bool Internal_HasRepeOrRepnePrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)(CodeFlags.RepePrefix | CodeFlags.RepnePrefix)) != 0;
		}

		/// <summary>
		/// Checks if the instruction has the XACQUIRE prefix (F2)
		/// </summary>
		public bool HasXacquirePrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.XacquirePrefix) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.XacquirePrefix;
				else
					codeFlags &= ~(uint)CodeFlags.XacquirePrefix;
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetHasXacquirePrefix() => codeFlags |= (uint)CodeFlags.XacquirePrefix;

		/// <summary>
		/// Checks if the instruction has the XACQUIRE prefix (F3)
		/// </summary>
		public bool HasXreleasePrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.XreleasePrefix) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.XreleasePrefix;
				else
					codeFlags &= ~(uint)CodeFlags.XreleasePrefix;
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetHasXreleasePrefix() => codeFlags |= (uint)CodeFlags.XreleasePrefix;

		/// <summary>
		/// Checks if the instruction has the REPE or REP prefix (F3)
		/// </summary>
		public bool HasRepePrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.RepePrefix) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.RepePrefix;
				else
					codeFlags &= ~(uint)CodeFlags.RepePrefix;
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetHasRepePrefix() => codeFlags |= (uint)CodeFlags.RepePrefix;
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalClearHasRepePrefix() => codeFlags &= ~(uint)CodeFlags.RepePrefix;

		/// <summary>
		/// Checks if the instruction has the REPNE prefix (F2)
		/// </summary>
		public bool HasRepnePrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.RepnePrefix) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.RepnePrefix;
				else
					codeFlags &= ~(uint)CodeFlags.RepnePrefix;
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetHasRepnePrefix() => codeFlags |= (uint)CodeFlags.RepnePrefix;
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalClearHasRepnePrefix() => codeFlags &= ~(uint)CodeFlags.RepnePrefix;

		/// <summary>
		/// Checks if the instruction has the LOCK prefix (F0)
		/// </summary>
		public bool HasLockPrefix {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.LockPrefix) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.LockPrefix;
				else
					codeFlags &= ~(uint)CodeFlags.LockPrefix;
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetHasLockPrefix() => codeFlags |= (uint)CodeFlags.LockPrefix;
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalClearHasLockPrefix() => codeFlags &= ~(uint)CodeFlags.LockPrefix;

		/// <summary>
		/// Gets operand #0's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op0Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (OpKind)(opKindFlags & (uint)OpKindFlags.OpKindMask);
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags = (opKindFlags & ~(uint)OpKindFlags.OpKindMask) | ((uint)value & (uint)OpKindFlags.OpKindMask);
		}
		internal OpKind InternalOp0Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags |= (uint)value;
		}
		internal bool Internal_Op0IsNotReg_or_Op0IsNotReg {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (opKindFlags & ((uint)OpKindFlags.OpKindMask | ((uint)OpKindFlags.OpKindMask << (int)OpKindFlags.Op1KindShift))) != 0;
		}

		/// <summary>
		/// Gets operand #1's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op1Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (OpKind)((opKindFlags >> (int)OpKindFlags.Op1KindShift) & (uint)OpKindFlags.OpKindMask);
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags = (opKindFlags & ~((uint)OpKindFlags.OpKindMask << (int)OpKindFlags.Op1KindShift)) |
				(((uint)value & (uint)OpKindFlags.OpKindMask) << (int)OpKindFlags.Op1KindShift);
		}
		internal OpKind InternalOp1Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags |= (uint)value << (int)OpKindFlags.Op1KindShift;
		}

		/// <summary>
		/// Gets operand #2's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op2Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (OpKind)((opKindFlags >> (int)OpKindFlags.Op2KindShift) & (uint)OpKindFlags.OpKindMask);
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags = (opKindFlags & ~((uint)OpKindFlags.OpKindMask << (int)OpKindFlags.Op2KindShift)) |
				(((uint)value & (uint)OpKindFlags.OpKindMask) << (int)OpKindFlags.Op2KindShift);
		}
		internal OpKind InternalOp2Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags |= (uint)value << (int)OpKindFlags.Op2KindShift;
		}

		/// <summary>
		/// Gets operand #3's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op3Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (OpKind)((opKindFlags >> (int)OpKindFlags.Op3KindShift) & (uint)OpKindFlags.OpKindMask);
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags = (opKindFlags & ~((uint)OpKindFlags.OpKindMask << (int)OpKindFlags.Op3KindShift)) |
				(((uint)value & (uint)OpKindFlags.OpKindMask) << (int)OpKindFlags.Op3KindShift);
		}
		internal OpKind InternalOp3Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags |= (uint)value << (int)OpKindFlags.Op3KindShift;
		}

		/// <summary>
		/// Gets operand #4's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op4Kind {
			get => OpKind.Immediate8;
			set {
				if (value != OpKind.Immediate8)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
			}
		}
		internal OpKind InternalOp4Kind {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set {
				if (value != OpKind.Immediate8)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
			}
		}

		/// <summary>
		/// Gets an operand's kind if it exists (see <see cref="OpCount"/>)
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <returns></returns>
		public OpKind GetOpKind(int operand) {
			switch (operand) {
			case 0: return Op0Kind;
			case 1: return Op1Kind;
			case 2: return Op2Kind;
			case 3: return Op3Kind;
			case 4: return Op4Kind;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_operand();
				return 0;
			}
		}

		/// <summary>
		/// Sets an operand's kind
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <param name="opKind">Operand kind</param>
		public void SetOpKind(int operand, OpKind opKind) {
			switch (operand) {
			case 0: Op0Kind = opKind; break;
			case 1: Op1Kind = opKind; break;
			case 2: Op2Kind = opKind; break;
			case 3: Op3Kind = opKind; break;
			case 4: Op4Kind = opKind; break;
			default: ThrowHelper.ThrowArgumentOutOfRangeException_operand(); break;
			}
		}

		/// <summary>
		/// Gets the segment override prefix or <see cref="Register.None"/> if none. See also <see cref="MemorySegment"/>.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>, <see cref="OpKind.Memory64"/>,
		/// <see cref="OpKind.MemorySegSI"/>, <see cref="OpKind.MemorySegESI"/>, <see cref="OpKind.MemorySegRSI"/>
		/// </summary>
		public Register SegmentPrefix {
			get {
				uint index = (((uint)memoryFlags >> (int)MemoryFlags.SegmentPrefixShift) & (uint)MemoryFlags.SegmentPrefixMask) - 1;
				return index < 6 ? Register.ES + (int)index : Register.None;
			}
			set {
				uint encValue;
				if (value == Register.None)
					encValue = 0;
				else
					encValue = (((uint)value - (uint)Register.ES) + 1) & (uint)MemoryFlags.SegmentPrefixMask;
				memoryFlags = (ushort)((memoryFlags & ~((uint)MemoryFlags.SegmentPrefixMask << (int)MemoryFlags.SegmentPrefixShift)) |
					(encValue << (int)MemoryFlags.SegmentPrefixShift));
			}
		}

		/// <summary>
		/// Gets the effective segment register used to reference the memory location.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>, <see cref="OpKind.Memory64"/>,
		/// <see cref="OpKind.MemorySegSI"/>, <see cref="OpKind.MemorySegESI"/>, <see cref="OpKind.MemorySegRSI"/>
		/// </summary>
		public Register MemorySegment {
			get {
				var segReg = SegmentPrefix;
				if (segReg != Register.None)
					return segReg;
				var baseReg = MemoryBase;
				if (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP || baseReg == Register.RBP || baseReg == Register.RSP)
					return Register.SS;
				return Register.DS;
			}
		}

		/// <summary>
		/// Gets the size of the memory displacement in bytes. Valid values are 0, 1 (16/32/64-bit), 2 (16-bit), 4 (32-bit), 8 (64-bit).
		/// Note that the return value can be 1 and <see cref="MemoryDisplacement"/> may still not fit in
		/// a signed byte if it's an EVEX encoded instruction.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>
		/// </summary>
		public int MemoryDisplSize {
			get {
				switch (((uint)memoryFlags >> (int)MemoryFlags.DisplSizeShift) & (uint)MemoryFlags.DisplSizeMask) {
				case 0: return 0;
				case 1: return 1;
				case 2: return 2;
				case 3: return 4;
				default:
				case 4: return 8;
				}
			}
			set {
				uint encValue;
				switch (value) {
				case 0: encValue = 0; break;
				case 1: encValue = 1; break;
				case 2: encValue = 2; break;
				case 4: encValue = 3; break;
				default:
				case 8: encValue = 4; break;
				}
				memoryFlags = (ushort)((memoryFlags & ~((uint)MemoryFlags.DisplSizeMask << (int)MemoryFlags.DisplSizeShift)) |
					(encValue << (int)MemoryFlags.DisplSizeShift));
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetMemoryDisplSize(uint scale) {
			Debug.Assert(0 <= scale && scale <= 4);
			memoryFlags |= (ushort)(scale << (int)MemoryFlags.DisplSizeShift);
		}

		/// <summary>
		/// true if the data is broadcasted (EVEX instructions only)
		/// </summary>
		public bool IsBroadcast {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (memoryFlags & (uint)MemoryFlags.BroadcastedMemory) != 0;
			set {
				if (value)
					memoryFlags |= (ushort)MemoryFlags.BroadcastedMemory;
				else
					memoryFlags &= unchecked((ushort)~(ushort)MemoryFlags.BroadcastedMemory);
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void SetIsBroadcast() => memoryFlags |= (ushort)MemoryFlags.BroadcastedMemory;

		/// <summary>
		/// Gets the size of the memory location that is referenced by the operand. See also <see cref="IsBroadcast"/>.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>, <see cref="OpKind.Memory64"/>,
		/// <see cref="OpKind.MemorySegSI"/>, <see cref="OpKind.MemorySegESI"/>, <see cref="OpKind.MemorySegRSI"/>,
		/// <see cref="OpKind.MemoryESDI"/>, <see cref="OpKind.MemoryESEDI"/>, <see cref="OpKind.MemoryESRDI"/>
		/// </summary>
		public MemorySize MemorySize {
			get {
				int index = (int)Code * 2;
				if (IsBroadcast)
					index++;
				return (MemorySize)InstructionMemorySizes.Sizes[index];
			}
		}

		/// <summary>
		/// Gets the index register scale value, valid values are *1, *2, *4, *8. Use this property if the operand has kind <see cref="OpKind.Memory"/>
		/// </summary>
		public int MemoryIndexScale {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => 1 << (int)(memoryFlags & (uint)MemoryFlags.ScaleMask);
			set {
				if (value == 1)
					memoryFlags &= 0xFFFC;
				else if (value == 2)
					memoryFlags = (ushort)((memoryFlags & ~(uint)MemoryFlags.ScaleMask) | 1);
				else if (value == 4)
					memoryFlags = (ushort)((memoryFlags & ~(uint)MemoryFlags.ScaleMask) | 2);
				else {
					Debug.Assert(value == 8);
					memoryFlags |= 3;
				}
			}
		}
		internal int InternalMemoryIndexScale {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (int)(memoryFlags & (uint)MemoryFlags.ScaleMask);
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => memoryFlags |= (ushort)value;
		}

		/// <summary>
		/// Gets the memory operand's displacement. This should be sign extended to 64 bits if it's 64-bit addressing.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>
		/// </summary>
		public uint MemoryDisplacement {
			get => memDispl;
			set => memDispl = value;
		}

		/// <summary>
		/// Gets an operand's immediate value
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <returns></returns>
		public ulong GetImmediate(int operand) {
			switch (GetOpKind(operand)) {
			case OpKind.Immediate8:			return Immediate8;
			case OpKind.Immediate8_2nd:		return Immediate8_2nd;
			case OpKind.Immediate16:		return Immediate16;
			case OpKind.Immediate32:		return Immediate32;
			case OpKind.Immediate64:		return Immediate64;
			case OpKind.Immediate8to16:		return (ulong)Immediate8to16;
			case OpKind.Immediate8to32:		return (ulong)Immediate8to32;
			case OpKind.Immediate8to64:		return (ulong)Immediate8to64;
			case OpKind.Immediate32to64:	return (ulong)Immediate32to64;
			default:
				throw new ArgumentException($"Op{operand} isn't an immediate operand", nameof(operand));
			}
		}

		/// <summary>
		/// Sets an operand's immediate value
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <param name="immediate">New immediate</param>
		/// <returns></returns>
		public void SetImmediate(int operand, int immediate) => SetImmediate(operand, (ulong)immediate);

		/// <summary>
		/// Sets an operand's immediate value
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <param name="immediate">New immediate</param>
		/// <returns></returns>
		public void SetImmediate(int operand, uint immediate) => SetImmediate(operand, (ulong)immediate);

		/// <summary>
		/// Sets an operand's immediate value
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <param name="immediate">New immediate</param>
		/// <returns></returns>
		public void SetImmediate(int operand, long immediate) => SetImmediate(operand, (ulong)immediate);

		/// <summary>
		/// Sets an operand's immediate value
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <param name="immediate">New immediate</param>
		/// <returns></returns>
		public void SetImmediate(int operand, ulong immediate) {
			switch (GetOpKind(operand)) {
			case OpKind.Immediate8:
			case OpKind.Immediate8to16:
			case OpKind.Immediate8to32:
			case OpKind.Immediate8to64:
				this.immediate = (byte)immediate;
				break;
			case OpKind.Immediate8_2nd:
				memDispl = (byte)immediate;
				break;
			case OpKind.Immediate16:
				this.immediate = (ushort)immediate;
				break;
			case OpKind.Immediate32to64:
			case OpKind.Immediate32:
				this.immediate = (uint)immediate;
				break;
			case OpKind.Immediate64:
				Immediate64 = immediate;
				break;
			default:
				throw new ArgumentException($"Op{operand} isn't an immediate operand", nameof(operand));
			}
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate8"/>
		/// </summary>
		public byte Immediate8 {
			get => (byte)immediate;
			set => immediate = value;
		}
		internal uint InternalImmediate8 {
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate8_2nd"/>
		/// </summary>
		public byte Immediate8_2nd {
			get => (byte)memDispl;
			set => memDispl = value;
		}
		internal uint InternalImmediate8_2nd {
			set => memDispl = value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate16"/>
		/// </summary>
		public ushort Immediate16 {
			get => (ushort)immediate;
			set => immediate = value;
		}
		internal uint InternalImmediate16 {
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate32"/>
		/// </summary>
		public uint Immediate32 {
			get => immediate;
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate64"/>
		/// </summary>
		public ulong Immediate64 {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => ((ulong)memDispl << 32) | immediate;
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set {
				immediate = (uint)value;
				memDispl = (uint)(value >> 32);
			}
		}
		internal uint InternalImmediate64_lo {
			set => immediate = value;
		}
		internal uint InternalImmediate64_hi {
			set => memDispl = value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate8to16"/>
		/// </summary>
		public short Immediate8to16 {
			get => (sbyte)immediate;
			set => immediate = (uint)(sbyte)value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate8to32"/>
		/// </summary>
		public int Immediate8to32 {
			get => (sbyte)immediate;
			set => immediate = (uint)(sbyte)value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate8to64"/>
		/// </summary>
		public long Immediate8to64 {
			get => (sbyte)immediate;
			set => immediate = (uint)(sbyte)value;
		}

		/// <summary>
		/// Gets the operand's immediate value. Use this property if the operand has kind <see cref="OpKind.Immediate32to64"/>
		/// </summary>
		public long Immediate32to64 {
			get => (int)immediate;
			set => immediate = (uint)value;
		}

		/// <summary>
		/// Gets the operand's 64-bit address value. Use this property if the operand has kind <see cref="OpKind.Memory64"/>
		/// </summary>
		public ulong MemoryAddress64 {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => ((ulong)memDispl << 32) | immediate;
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set {
				immediate = (uint)value;
				memDispl = (uint)(value >> 32);
			}
		}
		internal uint InternalMemoryAddress64_lo {
			set => immediate = value;
		}
		internal uint InternalMemoryAddress64_hi {
			set => memDispl = value;
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.NearBranch16"/>
		/// </summary>
		public ushort NearBranch16 {
			get => (ushort)immediate;
			set => immediate = value;
		}
		internal uint InternalNearBranch16 {
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.NearBranch32"/>
		/// </summary>
		public uint NearBranch32 {
			get => immediate;
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.NearBranch64"/>
		/// </summary>
		public ulong NearBranch64 {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => ((ulong)memDispl << 32) | immediate;
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set {
				immediate = (uint)value;
				memDispl = (uint)(value >> 32);
			}
		}

		/// <summary>
		/// Gets the near branch target if it's a call/jmp near branch instruction
		/// </summary>
		public ulong NearBranchTarget {
			get {
				switch (Op0Kind) {
				case OpKind.NearBranch16:	return NearBranch16;
				case OpKind.NearBranch32:	return NearBranch32;
				case OpKind.NearBranch64:	return NearBranch64;
				default:					return 0;
				}
			}
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.FarBranch16"/>
		/// </summary>
		public ushort FarBranch16 {
			get => (ushort)immediate;
			set => immediate = value;
		}
		internal uint InternalFarBranch16 {
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.FarBranch32"/>
		/// </summary>
		public uint FarBranch32 {
			get => immediate;
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's branch target selector. Use this property if the operand has kind <see cref="OpKind.FarBranch16"/> or <see cref="OpKind.FarBranch32"/>
		/// </summary>
		public ushort FarBranchSelector {
			get => (ushort)memDispl;
			set => memDispl = value;
		}
		internal uint InternalFarBranchSelector {
			set => memDispl = value;
		}

		/// <summary>
		/// Gets the memory operand's base register or <see cref="Register.None"/> if none. Use this property if the operand has kind <see cref="OpKind.Memory"/>
		/// </summary>
		public Register MemoryBase {
			get => (Register)memBaseReg;
			set => memBaseReg = (byte)value;
		}
		internal Register InternalMemoryBase {
			set => memBaseReg = (byte)value;
		}

		/// <summary>
		/// Gets the memory operand's index register or <see cref="Register.None"/> if none. Use this property if the operand has kind <see cref="OpKind.Memory"/>
		/// </summary>
		public Register MemoryIndex {
			get => (Register)memIndexReg;
			set => memIndexReg = (byte)value;
		}
		internal Register InternalMemoryIndex {
			set => memIndexReg = (byte)value;
		}

		/// <summary>
		/// Gets operand #0's register value. Use this property if operand #0 (<see cref="Op0Kind"/>) has kind <see cref="OpKind.Register"/>
		/// </summary>
		public Register Op0Register {
			get => (Register)reg0;
			set => reg0 = (byte)value;
		}
		internal Register InternalOp0Register {
			set => reg0 = (byte)value;
		}

		/// <summary>
		/// Gets operand #1's register value. Use this property if operand #1 (<see cref="Op1Kind"/>) has kind <see cref="OpKind.Register"/>
		/// </summary>
		public Register Op1Register {
			get => (Register)reg1;
			set => reg1 = (byte)value;
		}
		internal Register InternalOp1Register {
			set => reg1 = (byte)value;
		}

		/// <summary>
		/// Gets operand #2's register value. Use this property if operand #2 (<see cref="Op2Kind"/>) has kind <see cref="OpKind.Register"/>
		/// </summary>
		public Register Op2Register {
			get => (Register)reg2;
			set => reg2 = (byte)value;
		}
		internal Register InternalOp2Register {
			set => reg2 = (byte)value;
		}

		/// <summary>
		/// Gets operand #3's register value. Use this property if operand #3 (<see cref="Op3Kind"/>) has kind <see cref="OpKind.Register"/>
		/// </summary>
		public Register Op3Register {
			get => (Register)reg3;
			set => reg3 = (byte)value;
		}
		internal Register InternalOp3Register {
			set => reg3 = (byte)value;
		}

		/// <summary>
		/// Gets operand #4's register value. Use this property if operand #4 (<see cref="Op4Kind"/>) has kind <see cref="OpKind.Register"/>
		/// </summary>
		public Register Op4Register {
			get => Register.None;
			set {
				if (value != Register.None)
					ThrowHelper.ThrowArgumentOutOfRangeException_value();
			}
		}

		/// <summary>
		/// Gets the operand's register value. Use this property if the operand has kind <see cref="OpKind.Register"/>
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <returns></returns>
		public Register GetOpRegister(int operand) {
			switch (operand) {
			case 0: return Op0Register;
			case 1: return Op1Register;
			case 2: return Op2Register;
			case 3: return Op3Register;
			case 4: return Op4Register;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_operand();
				return 0;
			}
		}

		/// <summary>
		/// Sets the operand's register value. Use this property if the operand has kind <see cref="OpKind.Register"/>
		/// </summary>
		/// <param name="operand">Operand number, 0-4</param>
		/// <param name="register">Register</param>
		public void SetOpRegister(int operand, Register register) {
			switch (operand) {
			case 0: Op0Register = register; break;
			case 1: Op1Register = register; break;
			case 2: Op2Register = register; break;
			case 3: Op3Register = register; break;
			case 4: Op4Register = register; break;
			default: ThrowHelper.ThrowArgumentOutOfRangeException_operand(); break;
			}
		}

		/// <summary>
		/// Gets the opmask register (<see cref="Register.K1"/> - <see cref="Register.K7"/>) or <see cref="Register.None"/> if none
		/// </summary>
		public Register OpMask {
			get {
				int r = (int)(codeFlags >> (int)CodeFlags.OpMaskShift) & (int)CodeFlags.OpMaskMask;
				return r == 0 ? Register.None : r + Register.K0;
			}
			set {
				uint r;
				if (value == Register.None)
					r = 0;
				else
					r = (uint)((uint)(value - Register.K0) & (uint)CodeFlags.OpMaskMask);
				codeFlags = (codeFlags & ~((uint)CodeFlags.OpMaskMask << (int)CodeFlags.OpMaskShift)) |
						(r << (int)CodeFlags.OpMaskShift);
			}
		}
		internal uint InternalOpMask {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags >> (int)CodeFlags.OpMaskShift) & (uint)CodeFlags.OpMaskMask;
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => codeFlags |= value << (int)CodeFlags.OpMaskShift;
		}

		/// <summary>
		/// true if there's an opmask register (<see cref="OpMask"/>)
		/// </summary>
		public bool HasOpMask {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & ((uint)CodeFlags.OpMaskMask << (int)CodeFlags.OpMaskShift)) != 0;
		}

		/// <summary>
		/// true if zeroing-masking, false if merging-masking.
		/// Only used by most EVEX encoded instructions that use opmask registers.
		/// </summary>
		public bool ZeroingMasking {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.ZeroingMasking) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.ZeroingMasking;
				else
					codeFlags &= ~(uint)CodeFlags.ZeroingMasking;
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetZeroingMasking() => codeFlags |= (uint)CodeFlags.ZeroingMasking;

		/// <summary>
		/// true if merging-masking, false if zeroing-masking.
		/// Only used by most EVEX encoded instructions that use opmask registers.
		/// </summary>
		public bool MergingMasking {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.ZeroingMasking) == 0;
			set {
				if (value)
					codeFlags &= ~(uint)CodeFlags.ZeroingMasking;
				else
					codeFlags |= (uint)CodeFlags.ZeroingMasking;
			}
		}

		/// <summary>
		/// Rounding control (<see cref="SuppressAllExceptions"/> is implied but still returns false)
		/// or <see cref="RoundingControl.None"/> if the instruction doesn't use it.
		/// </summary>
		public RoundingControl RoundingControl {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (RoundingControl)((codeFlags >> (int)CodeFlags.RoundingControlShift) & (int)CodeFlags.RoundingControlMask);
			set => codeFlags = (codeFlags & ~((uint)CodeFlags.RoundingControlMask << (int)CodeFlags.RoundingControlShift)) |
				(((uint)value & (uint)CodeFlags.RoundingControlMask) << (int)CodeFlags.RoundingControlShift);
		}
		internal uint InternalRoundingControl {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => codeFlags |= value << (int)CodeFlags.RoundingControlShift;
		}

		/// <summary>
		/// Number of elements in a db/dw/dd/dq directive.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareByte"/>, <see cref="Code.DeclareWord"/>, <see cref="Code.DeclareDword"/>, <see cref="Code.DeclareQword"/>
		/// </summary>
		public int DeclareDataCount {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (int)((opKindFlags >> (int)OpKindFlags.DataLengthShift) & (uint)OpKindFlags.DataLengthMask) + 1;
			set => opKindFlags = (opKindFlags & ~((uint)OpKindFlags.DataLengthMask << (int)OpKindFlags.DataLengthShift)) |
					(((uint)(value - 1) & (uint)OpKindFlags.DataLengthMask) << (int)OpKindFlags.DataLengthShift);
		}
		internal uint InternalDeclareDataCount {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			set => opKindFlags |= (value - 1) << (int)OpKindFlags.DataLengthShift;
		}

		/// <summary>
		/// Sets a new 'db' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareByte"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareByteValue(int index, sbyte value) => SetDeclareByteValue(index, (byte)value);

		/// <summary>
		/// Sets a new 'db' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareByte"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareByteValue(int index, byte value) {
			switch (index) {
			case 0:
				reg0 = value;
				break;
			case 1:
				reg1 = value;
				break;
			case 2:
				reg2 = value;
				break;
			case 3:
				reg3 = value;
				break;
			case 4:
				immediate = (immediate & 0xFFFFFF00) | value;
				break;
			case 5:
				immediate = (immediate & 0xFFFF00FF) | ((uint)value << 8);
				break;
			case 6:
				immediate = (immediate & 0xFF00FFFF) | ((uint)value << 16);
				break;
			case 7:
				immediate = (immediate & 0x00FFFFFF) | ((uint)value << 24);
				break;
			case 8:
				memDispl = (memDispl & 0xFFFFFF00) | value;
				break;
			case 9:
				memDispl = (memDispl & 0xFFFF00FF) | ((uint)value << 8);
				break;
			case 10:
				memDispl = (memDispl & 0xFF00FFFF) | ((uint)value << 16);
				break;
			case 11:
				memDispl = (memDispl & 0x00FFFFFF) | ((uint)value << 24);
				break;
			case 12:
				memBaseReg = value;
				break;
			case 13:
				memIndexReg = value;
				break;
			case 14:
				opKindFlags = (opKindFlags & 0xFFFFFF00) | value;
				break;
			case 15:
				opKindFlags = (opKindFlags & 0xFFFF00FF) | ((uint)value << 8);
				break;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				break;
			}
		}

		/// <summary>
		/// Gets a 'db' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareByte"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns></returns>
		public byte GetDeclareByteValue(int index) {
			switch (index) {
			case 0:		return reg0;
			case 1:		return reg1;
			case 2:		return reg2;
			case 3:		return reg3;
			case 4:		return (byte)immediate;
			case 5:		return (byte)(immediate >> 8);
			case 6:		return (byte)(immediate >> 16);
			case 7:		return (byte)(immediate >> 24);
			case 8:		return (byte)memDispl;
			case 9:		return (byte)(memDispl >> 8);
			case 10:	return (byte)(memDispl >> 16);
			case 11:	return (byte)(memDispl >> 24);
			case 12:	return memBaseReg;
			case 13:	return memIndexReg;
			case 14:	return (byte)opKindFlags;
			case 15:	return (byte)(opKindFlags >> 8);
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				return 0;
			}
		}

		/// <summary>
		/// Sets a new 'dw' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareWord"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareWordValue(int index, short value) => SetDeclareWordValue(index, (ushort)value);

		/// <summary>
		/// Sets a new 'dw' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareWord"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareWordValue(int index, ushort value) {
			switch (index) {
			case 0:
				reg0 = (byte)value;
				reg1 = (byte)(value >> 8);
				break;
			case 1:
				reg2 = (byte)value;
				reg3 = (byte)(value >> 8);
				break;
			case 2:
				immediate = (immediate & 0xFFFF0000) | value;
				break;
			case 3:
				immediate = (uint)(ushort)immediate | ((uint)value << 16);
				break;
			case 4:
				memDispl = (memDispl & 0xFFFF0000) | value;
				break;
			case 5:
				memDispl = (uint)(ushort)memDispl | ((uint)value << 16);
				break;
			case 6:
				memBaseReg = (byte)value;
				memIndexReg = (byte)(value >> 8);
				break;
			case 7:
				opKindFlags = (opKindFlags & 0xFFFF0000) | value;
				break;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				break;
			}
		}

		/// <summary>
		/// Gets a 'dw' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareWord"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns></returns>
		public ushort GetDeclareWordValue(int index) {
			switch (index) {
			case 0:	return (ushort)((uint)reg0 | (uint)(reg1 << 8));
			case 1:	return (ushort)((uint)reg2 | (uint)(reg3 << 8));
			case 2:	return (ushort)immediate;
			case 3:	return (ushort)(immediate >> 16);
			case 4:	return (ushort)memDispl;
			case 5:	return (ushort)(memDispl >> 16);
			case 6:	return (ushort)((uint)memBaseReg | (uint)(memIndexReg << 8));
			case 7:	return (ushort)opKindFlags;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				return 0;
			}
		}

		/// <summary>
		/// Sets a new 'dd' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareDword"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareDwordValue(int index, int value) => SetDeclareDwordValue(index, (uint)value);

		/// <summary>
		/// Sets a new 'dd' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareDword"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareDwordValue(int index, uint value) {
			switch (index) {
			case 0:
				reg0 = (byte)value;
				reg1 = (byte)(value >> 8);
				reg2 = (byte)(value >> 16);
				reg3 = (byte)(value >> 24);
				break;
			case 1:
				immediate = value;
				break;
			case 2:
				memDispl = value;
				break;
			case 3:
				memBaseReg = (byte)value;
				memIndexReg = (byte)(value >> 8);
				opKindFlags = (opKindFlags & 0xFFFF0000) | (value >> 16);
				break;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				break;
			}
		}

		/// <summary>
		/// Gets a 'dd' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareDword"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns></returns>
		public uint GetDeclareDwordValue(int index) {
			switch (index) {
			case 0:	return (uint)reg0 | (uint)(reg1 << 8) | (uint)(reg2 << 16) | (uint)(reg3 << 24);
			case 1:	return immediate;
			case 2:	return memDispl;
			case 3:	return (uint)memBaseReg | (uint)(memIndexReg << 8) | (opKindFlags << 16);
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				return 0;
			}
		}

		/// <summary>
		/// Sets a new 'dq' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareQword"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareQwordValue(int index, long value) => SetDeclareQwordValue(index, (ulong)value);

		/// <summary>
		/// Sets a new 'dq' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareQword"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="value">New value</param>
		public void SetDeclareQwordValue(int index, ulong value) {
			uint v;
			switch (index) {
			case 0:
				v = (uint)value;
				reg0 = (byte)v;
				reg1 = (byte)(v >> 8);
				reg2 = (byte)(v >> 16);
				reg3 = (byte)(v >> 24);
				immediate = (uint)(value >> 32);
				break;
			case 1:
				memDispl = (uint)value;
				v = (uint)(value >> 32);
				memBaseReg = (byte)v;
				memIndexReg = (byte)(v >> 8);
				opKindFlags = (opKindFlags & 0xFFFF0000) | (v >> 16);
				break;
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				break;
			}
		}

		/// <summary>
		/// Gets a 'dq' value, see also <see cref="DeclareDataCount"/>.
		/// Can only be called if <see cref="Code"/> is <see cref="Code.DeclareQword"/>
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns></returns>
		public ulong GetDeclareQwordValue(int index) {
			switch (index) {
			case 0:	return (ulong)reg0 | (ulong)((uint)reg1 << 8) | (ulong)((uint)reg2 << 16) | (ulong)((uint)reg3 << 24) | ((ulong)immediate << 32);
			case 1:	return (ulong)memDispl | ((ulong)memBaseReg << 32) | ((ulong)memIndexReg << 40) | ((ulong)opKindFlags << 48);
			default:
				ThrowHelper.ThrowArgumentOutOfRangeException_index();
				return 0;
			}
		}

		/// <summary>
		/// Checks if this is a VSIB instruction, see also <see cref="IsVsib32"/>, <see cref="IsVsib64"/>
		/// </summary>
		public bool IsVsib => TryGetVsib64(out _);

		/// <summary>
		/// VSIB instructions only (<see cref="IsVsib"/>): true if it's using 32-bit indexes, false if it's using 64-bit indexes
		/// </summary>
		public bool IsVsib32 => TryGetVsib64(out bool vsib64) && !vsib64;

		/// <summary>
		/// VSIB instructions only (<see cref="IsVsib"/>): true if it's using 64-bit indexes, false if it's using 32-bit indexes
		/// </summary>
		public bool IsVsib64 => TryGetVsib64(out bool vsib64) && vsib64;

		/// <summary>
		/// Checks if it's a VSIB instruction. If it's a VSIB instruction, it sets <paramref name="vsib64"/> to true if it's
		/// a VSIB instruction with 64-bit indexes, and clears it if it's using 32-bit indexes.
		/// </summary>
		/// <param name="vsib64">If it's a VSIB instruction, set to true if it's using 64-bit indexes, set to false if it's using 32-bit indexes</param>
		/// <returns></returns>
		public bool TryGetVsib64(out bool vsib64) {
			switch (Code) {
			case Code.VEX_Vpgatherdd_xmm_vm32x_xmm:
			case Code.VEX_Vpgatherdd_ymm_vm32y_ymm:
			case Code.VEX_Vpgatherdq_xmm_vm32x_xmm:
			case Code.VEX_Vpgatherdq_ymm_vm32x_ymm:
			case Code.EVEX_Vpgatherdd_xmm_k1_vm32x:
			case Code.EVEX_Vpgatherdd_ymm_k1_vm32y:
			case Code.EVEX_Vpgatherdd_zmm_k1_vm32z:
			case Code.EVEX_Vpgatherdq_xmm_k1_vm32x:
			case Code.EVEX_Vpgatherdq_ymm_k1_vm32x:
			case Code.EVEX_Vpgatherdq_zmm_k1_vm32y:

			case Code.VEX_Vgatherdps_xmm_vm32x_xmm:
			case Code.VEX_Vgatherdps_ymm_vm32y_ymm:
			case Code.VEX_Vgatherdpd_xmm_vm32x_xmm:
			case Code.VEX_Vgatherdpd_ymm_vm32x_ymm:
			case Code.EVEX_Vgatherdps_xmm_k1_vm32x:
			case Code.EVEX_Vgatherdps_ymm_k1_vm32y:
			case Code.EVEX_Vgatherdps_zmm_k1_vm32z:
			case Code.EVEX_Vgatherdpd_xmm_k1_vm32x:
			case Code.EVEX_Vgatherdpd_ymm_k1_vm32x:
			case Code.EVEX_Vgatherdpd_zmm_k1_vm32y:

			case Code.EVEX_Vpscatterdd_vm32x_k1_xmm:
			case Code.EVEX_Vpscatterdd_vm32y_k1_ymm:
			case Code.EVEX_Vpscatterdd_vm32z_k1_zmm:
			case Code.EVEX_Vpscatterdq_vm32x_k1_xmm:
			case Code.EVEX_Vpscatterdq_vm32x_k1_ymm:
			case Code.EVEX_Vpscatterdq_vm32y_k1_zmm:

			case Code.EVEX_Vscatterdps_vm32x_k1_xmm:
			case Code.EVEX_Vscatterdps_vm32y_k1_ymm:
			case Code.EVEX_Vscatterdps_vm32z_k1_zmm:
			case Code.EVEX_Vscatterdpd_vm32x_k1_xmm:
			case Code.EVEX_Vscatterdpd_vm32x_k1_ymm:
			case Code.EVEX_Vscatterdpd_vm32y_k1_zmm:

			case Code.EVEX_Vgatherpf0dps_vm32z_k1:
			case Code.EVEX_Vgatherpf0dpd_vm32y_k1:
			case Code.EVEX_Vgatherpf1dps_vm32z_k1:
			case Code.EVEX_Vgatherpf1dpd_vm32y_k1:
			case Code.EVEX_Vscatterpf0dps_vm32z_k1:
			case Code.EVEX_Vscatterpf0dpd_vm32y_k1:
			case Code.EVEX_Vscatterpf1dps_vm32z_k1:
			case Code.EVEX_Vscatterpf1dpd_vm32y_k1:
				vsib64 = false;
				return true;

			case Code.VEX_Vpgatherqd_xmm_vm64x_xmm:
			case Code.VEX_Vpgatherqd_xmm_vm64y_xmm:
			case Code.VEX_Vpgatherqq_xmm_vm64x_xmm:
			case Code.VEX_Vpgatherqq_ymm_vm64y_ymm:
			case Code.EVEX_Vpgatherqd_xmm_k1_vm64x:
			case Code.EVEX_Vpgatherqd_xmm_k1_vm64y:
			case Code.EVEX_Vpgatherqd_ymm_k1_vm64z:
			case Code.EVEX_Vpgatherqq_xmm_k1_vm64x:
			case Code.EVEX_Vpgatherqq_ymm_k1_vm64y:
			case Code.EVEX_Vpgatherqq_zmm_k1_vm64z:

			case Code.VEX_Vgatherqps_xmm_vm64x_xmm:
			case Code.VEX_Vgatherqps_xmm_vm64y_xmm:
			case Code.VEX_Vgatherqpd_xmm_vm64x_xmm:
			case Code.VEX_Vgatherqpd_ymm_vm64y_ymm:
			case Code.EVEX_Vgatherqps_xmm_k1_vm64x:
			case Code.EVEX_Vgatherqps_xmm_k1_vm64y:
			case Code.EVEX_Vgatherqps_ymm_k1_vm64z:
			case Code.EVEX_Vgatherqpd_xmm_k1_vm64x:
			case Code.EVEX_Vgatherqpd_ymm_k1_vm64y:
			case Code.EVEX_Vgatherqpd_zmm_k1_vm64z:

			case Code.EVEX_Vpscatterqd_vm64x_k1_xmm:
			case Code.EVEX_Vpscatterqd_vm64y_k1_xmm:
			case Code.EVEX_Vpscatterqd_vm64z_k1_ymm:
			case Code.EVEX_Vpscatterqq_vm64x_k1_xmm:
			case Code.EVEX_Vpscatterqq_vm64y_k1_ymm:
			case Code.EVEX_Vpscatterqq_vm64z_k1_zmm:

			case Code.EVEX_Vscatterqps_vm64x_k1_xmm:
			case Code.EVEX_Vscatterqps_vm64y_k1_xmm:
			case Code.EVEX_Vscatterqps_vm64z_k1_ymm:
			case Code.EVEX_Vscatterqpd_vm64x_k1_xmm:
			case Code.EVEX_Vscatterqpd_vm64y_k1_ymm:
			case Code.EVEX_Vscatterqpd_vm64z_k1_zmm:

			case Code.EVEX_Vgatherpf0qps_vm64z_k1:
			case Code.EVEX_Vgatherpf0qpd_vm64z_k1:
			case Code.EVEX_Vgatherpf1qps_vm64z_k1:
			case Code.EVEX_Vgatherpf1qpd_vm64z_k1:
			case Code.EVEX_Vscatterpf0qps_vm64z_k1:
			case Code.EVEX_Vscatterpf0qpd_vm64z_k1:
			case Code.EVEX_Vscatterpf1qps_vm64z_k1:
			case Code.EVEX_Vscatterpf1qpd_vm64z_k1:
				vsib64 = true;
				return true;

			default:
				vsib64 = false;
				return false;
			}
		}

		/// <summary>
		/// Suppress all exceptions (EVEX encoded instructions). Note that if <see cref="RoundingControl"/> is
		/// not <see cref="RoundingControl.None"/>, SAE is implied but this property will still return false.
		/// </summary>
		public bool SuppressAllExceptions {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => (codeFlags & (uint)CodeFlags.SuppressAllExceptions) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.SuppressAllExceptions;
				else
					codeFlags &= ~(uint)CodeFlags.SuppressAllExceptions;
			}
		}
		[MethodImpl(MethodImplOptions2.AggressiveInlining)]
		internal void InternalSetSuppressAllExceptions() => codeFlags |= (uint)CodeFlags.SuppressAllExceptions;

		/// <summary>
		/// Checks if the memory operand is RIP/EIP relative
		/// </summary>
		[Obsolete("Use " + nameof(IsIPRelativeMemoryOperand) + " instead of this property", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool IsIPRelativeMemoryOp {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => MemoryBase == Register.RIP || MemoryBase == Register.EIP;
		}

		/// <summary>
		/// Checks if the memory operand is RIP/EIP relative
		/// </summary>
		public bool IsIPRelativeMemoryOperand {
			[MethodImpl(MethodImplOptions2.AggressiveInlining)]
			get => MemoryBase == Register.RIP || MemoryBase == Register.EIP;
		}

		/// <summary>
		/// Gets the RIP/EIP releative address ((<see cref="NextIP"/> or <see cref="NextIP32"/>) + <see cref="MemoryDisplacement"/>). This property is only valid if there's a memory operand with RIP/EIP relative addressing.
		/// </summary>
		public ulong IPRelativeMemoryAddress {
			get {
				ulong result = NextIP + (ulong)(int)MemoryDisplacement;
				if (MemoryBase == Register.EIP)
					result = (uint)result;
				return result;
			}
		}

		/// <summary>
		/// Formats the instruction using the default formatter with default formatter options
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
#if !NO_MASM_FORMATTER && !NO_FORMATTER
			var output = new StringBuilderFormatterOutput();
			new MasmFormatter().Format(ref this, output);
			return output.ToString();
#elif !NO_NASM_FORMATTER && !NO_FORMATTER
			var output = new StringBuilderFormatterOutput();
			new NasmFormatter().Format(ref this, output);
			return output.ToString();
#elif !NO_INTEL_FORMATTER && !NO_FORMATTER
			var output = new StringBuilderFormatterOutput();
			new IntelFormatter().Format(ref this, output);
			return output.ToString();
#elif !NO_GAS_FORMATTER && !NO_FORMATTER
			var output = new StringBuilderFormatterOutput();
			new GasFormatter().Format(ref this, output);
			return output.ToString();
#else
			return base.ToString();
#endif
		}
	}

	/// <summary>
	/// Default code size when an instruction was decoded
	/// </summary>
	public enum CodeSize {
		/// <summary>
		/// Unknown size
		/// </summary>
		Unknown				= 0,

		/// <summary>
		/// 16-bit code
		/// </summary>
		Code16				= 1,

		/// <summary>
		/// 32-bit code
		/// </summary>
		Code32				= 2,

		/// <summary>
		/// 64-bit code
		/// </summary>
		Code64				= 3,
	}
}
