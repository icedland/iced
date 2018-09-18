/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Iced.Intel {
	/// <summary>
	/// A 16/32/64-bit instruction
	/// </summary>
	public struct Instruction {
		internal const int TEST_MemorySizeBits = (int)MemoryFlags.MemorySizeBits;
		internal const int TEST_OpKindBits = (int)OpKindFlags.OpKindBits;
		internal const int TEST_CodeBits = (int)CodeFlags.CodeBits;
		internal const int TEST_RegisterBits = 8;

		/// <summary>
		/// [1:0]	= Scale
		/// [4:2]	= Size of displacement: 0, 1, 2, 4, 8
		/// [7:5]	= Segment register prefix: none, es, cs, ss, ds, fs, gs, reserved
		/// [14:8]	= <see cref="Intel.MemorySize"/>
		/// [15]	= Not used
		/// </summary>
		[Flags]
		enum MemoryFlags : ushort {
			ScaleMask				= 3,
			DisplSizeShift			= 2,
			DisplSizeMask			= 7,
			PrefixSegmentShift		= 5,
			PrefixSegmentMask		= 7,
			MemorySizeBits			= 7,
			MemorySizeShift			= 8,
			MemorySizeMask			= (1 << (int)MemorySizeBits) - 1,
			// Unused bits here
		}

		/// <summary>
		/// [4:0]	= Operand #0's <see cref="OpKind"/>
		/// [9:5]	= Operand #1's <see cref="OpKind"/>
		/// [14:10]	= Operand #2's <see cref="OpKind"/>
		/// [19:15]	= Operand #3's <see cref="OpKind"/>
		/// [29:20]	= Not used
		/// [31:30] = CodeSize
		/// </summary>
		[Flags]
		enum OpKindFlags : uint {
			OpKindBits				= 5,
			OpKindMask				= (1 << (int)OpKindBits) - 1,
			Op1KindShift			= 5,
			Op2KindShift			= 10,
			Op3KindShift			= 15,
			// Unused bits here
			CodeSizeMask			= 3,
			CodeSizeShift			= 30,
		}

		/// <summary>
		/// [12:0]	= <see cref="Intel.Code"/>
		/// [15:13]	= <see cref="Intel.RoundingControl"/>
		/// [18:16]	= Opmask register or 0 if none
		/// [22:19]	= Instruction length
		/// [24:23] = Not used
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
			PrefixXacquire			= 0x08000000,
			PrefixXrelease			= 0x10000000,
			PrefixRepe				= 0x20000000,
			PrefixRepne				= 0x40000000,
			PrefixLock				= 0x80000000,
		}

		// All fields, size: 32 bytes with bits to spare
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

		internal static bool TEST_BitByBitEquals(ref Instruction a, ref Instruction b) =>
			a.nextRip == b.nextRip &&
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
			a.reg3 == b.reg3;

		void ThrowArgumentOutOfRangeException(string paramName) => throw new ArgumentOutOfRangeException(paramName);
		void ThrowArgumentNullException(string paramName) => throw new ArgumentNullException(paramName);

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
		public ulong IP64 {
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
		public ulong NextIP64 {
			get => nextRip;
			set => nextRip = value;
		}

		/// <summary>
		/// Gets the code size when the instruction was decoded. This value is informational and can
		/// be used by a formatter.
		/// </summary>
		public CodeSize CodeSize {
			get => (CodeSize)((opKindFlags >> (int)OpKindFlags.CodeSizeShift) & (uint)OpKindFlags.CodeSizeMask);
			set => opKindFlags = ((opKindFlags & ~((uint)OpKindFlags.CodeSizeMask << (int)OpKindFlags.CodeSizeShift)) |
				(((uint)value & (uint)OpKindFlags.CodeSizeMask) << (int)OpKindFlags.CodeSizeShift));
		}
		internal CodeSize InternalCodeSize {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => opKindFlags |= ((uint)value << (int)OpKindFlags.CodeSizeShift);
		}

		/// <summary>
		/// Instruction code
		/// </summary>
		public Code Code {
			get => (Code)(codeFlags & (uint)CodeFlags.CodeMask);
			set => codeFlags = (codeFlags & ~(uint)CodeFlags.CodeMask) | ((uint)value & (uint)CodeFlags.CodeMask);
		}
		internal Code InternalCode {
			// x86 jitter doesn't always inline some of these props that should be inlined. Force it.
			// RyuJIT seems to be better and doesn't seem to require force-inline attrs.
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => codeFlags |= (uint)value;
		}

		/// <summary>
		/// Gets the operand count. Up to 5 operands is allowed.
		/// </summary>
		public int OpCount => InstructionUtils.InstructionOpCount[(int)(codeFlags & (uint)CodeFlags.CodeMask)];

		/// <summary>
		/// Gets the length of the instruction, 0-15 bytes. This is just informational. If you modify the instruction
		/// or create a new one, this property could return the wrong value.
		/// </summary>
		public int ByteLength {
			get => (int)((codeFlags >> (int)CodeFlags.InstrLengthShift) & (uint)CodeFlags.InstrLengthMask);
			set => codeFlags = (codeFlags & ~((uint)CodeFlags.InstrLengthMask << (int)CodeFlags.InstrLengthShift)) |
				(((uint)value & (uint)CodeFlags.InstrLengthMask) << (int)CodeFlags.InstrLengthShift);
		}
		internal uint InternalByteLength {
			set => codeFlags |= (value << (int)CodeFlags.InstrLengthShift);
		}

		internal bool Internal_HasPrefixRepe_HasPrefixXrelease => (codeFlags & (uint)(CodeFlags.PrefixRepe | CodeFlags.PrefixXrelease)) != 0;
		internal bool Internal_HasPrefixRepne_HasPrefixXacquire => (codeFlags & (uint)(CodeFlags.PrefixRepne | CodeFlags.PrefixXacquire)) != 0;
		internal bool Internal_HasPrefixRepeOrRepne => (codeFlags & (uint)(CodeFlags.PrefixRepe | CodeFlags.PrefixRepne)) != 0;

		/// <summary>
		/// Checks if the instruction has the XACQUIRE prefix (F2)
		/// </summary>
		public bool HasPrefixXacquire {
			get => (codeFlags & (uint)CodeFlags.PrefixXacquire) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.PrefixXacquire;
				else
					codeFlags &= ~(uint)CodeFlags.PrefixXacquire;
			}
		}
		internal void InternalSetHasPrefixXacquire() => codeFlags |= (uint)CodeFlags.PrefixXacquire;

		/// <summary>
		/// Checks if the instruction has the XACQUIRE prefix (F3)
		/// </summary>
		public bool HasPrefixXrelease {
			get => (codeFlags & (uint)CodeFlags.PrefixXrelease) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.PrefixXrelease;
				else
					codeFlags &= ~(uint)CodeFlags.PrefixXrelease;
			}
		}
		internal void InternalSetHasPrefixXrelease() => codeFlags |= (uint)CodeFlags.PrefixXrelease;

		/// <summary>
		/// Checks if the instruction has the REPE prefix (F3)
		/// </summary>
		public bool HasPrefixRepe {
			get => (codeFlags & (uint)CodeFlags.PrefixRepe) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.PrefixRepe;
				else
					codeFlags &= ~(uint)CodeFlags.PrefixRepe;
			}
		}
		internal void InternalSetHasPrefixRepe() => codeFlags |= (uint)CodeFlags.PrefixRepe;
		internal void InternalClearHasPrefixRepe() => codeFlags &= ~(uint)CodeFlags.PrefixRepe;

		/// <summary>
		/// Checks if the instruction has the REPNE prefix (F2)
		/// </summary>
		public bool HasPrefixRepne {
			get => (codeFlags & (uint)CodeFlags.PrefixRepne) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.PrefixRepne;
				else
					codeFlags &= ~(uint)CodeFlags.PrefixRepne;
			}
		}
		internal void InternalSetHasPrefixRepne() => codeFlags |= (uint)CodeFlags.PrefixRepne;
		internal void InternalClearHasPrefixRepne() => codeFlags &= ~(uint)CodeFlags.PrefixRepne;

		/// <summary>
		/// Checks if the instruction has the LOCK prefix (F0)
		/// </summary>
		public bool HasPrefixLock {
			get => (codeFlags & (uint)CodeFlags.PrefixLock) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.PrefixLock;
				else
					codeFlags &= ~(uint)CodeFlags.PrefixLock;
			}
		}
		internal void InternalSetHasPrefixLock() => codeFlags |= (uint)CodeFlags.PrefixLock;

		/// <summary>
		/// Gets operand #0's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op0Kind {
			get => (OpKind)(opKindFlags & (uint)OpKindFlags.OpKindMask);
			set => opKindFlags = (opKindFlags & ~(uint)OpKindFlags.OpKindMask) | ((uint)value & (uint)OpKindFlags.OpKindMask);
		}
		internal OpKind InternalOp0Kind {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => opKindFlags |= (uint)value;
		}

		/// <summary>
		/// Gets operand #1's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op1Kind {
			get => (OpKind)((opKindFlags >> (int)OpKindFlags.Op1KindShift) & (uint)OpKindFlags.OpKindMask);
			set => opKindFlags = (opKindFlags & ~((uint)OpKindFlags.OpKindMask << (int)OpKindFlags.Op1KindShift)) |
				(((uint)value & (uint)OpKindFlags.OpKindMask) << (int)OpKindFlags.Op1KindShift);
		}
		internal OpKind InternalOp1Kind {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => opKindFlags |= (uint)value << (int)OpKindFlags.Op1KindShift;
		}

		/// <summary>
		/// Gets operand #2's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op2Kind {
			get => (OpKind)((opKindFlags >> (int)OpKindFlags.Op2KindShift) & (uint)OpKindFlags.OpKindMask);
			set => opKindFlags = (opKindFlags & ~((uint)OpKindFlags.OpKindMask << (int)OpKindFlags.Op2KindShift)) |
				(((uint)value & (uint)OpKindFlags.OpKindMask) << (int)OpKindFlags.Op2KindShift);
		}
		internal OpKind InternalOp2Kind {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => opKindFlags |= (uint)value << (int)OpKindFlags.Op2KindShift;
		}

		/// <summary>
		/// Gets operand #3's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op3Kind {
			get => (OpKind)((opKindFlags >> (int)OpKindFlags.Op3KindShift) & (uint)OpKindFlags.OpKindMask);
			set => opKindFlags = (opKindFlags & ~((uint)OpKindFlags.OpKindMask << (int)OpKindFlags.Op3KindShift)) |
				(((uint)value & (uint)OpKindFlags.OpKindMask) << (int)OpKindFlags.Op3KindShift);
		}
		internal OpKind InternalOp3Kind {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => opKindFlags |= (uint)value << (int)OpKindFlags.Op3KindShift;
		}

		/// <summary>
		/// Gets operand #4's kind if the operand exists (see <see cref="OpCount"/>)
		/// </summary>
		public OpKind Op4Kind {
			get => OpKind.Immediate8;
			set {
				if (value != OpKind.Immediate8)
					ThrowArgumentOutOfRangeException(nameof(value));
			}
		}
		internal OpKind InternalOp4Kind {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (value != OpKind.Immediate8)
					ThrowArgumentOutOfRangeException(nameof(value));
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
				ThrowArgumentOutOfRangeException(nameof(operand));
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
			default: ThrowArgumentOutOfRangeException(nameof(operand)); break;
			}
		}

		/// <summary>
		/// Gets the segment override prefix or <see cref="Register.None"/> if none. See also <see cref="MemorySegment"/>.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>, <see cref="OpKind.Memory64"/>,
		/// <see cref="OpKind.MemorySegSI"/>, <see cref="OpKind.MemorySegESI"/>, <see cref="OpKind.MemorySegRSI"/>
		/// </summary>
		public Register PrefixSegment {
			get {
				uint index = (((uint)memoryFlags >> (int)MemoryFlags.PrefixSegmentShift) & (uint)MemoryFlags.PrefixSegmentMask) - 1;
				return index < 6 ? Register.ES + (int)index : Register.None;
			}
			set {
				uint encValue;
				if (value == Register.None)
					encValue = 0;
				else
					encValue = (((uint)value - (uint)Register.ES) + 1) & (uint)MemoryFlags.PrefixSegmentMask;
				memoryFlags = (ushort)((memoryFlags & ~((uint)MemoryFlags.PrefixSegmentMask << (int)MemoryFlags.PrefixSegmentShift)) |
					(encValue << (int)MemoryFlags.PrefixSegmentShift));
			}
		}

		/// <summary>
		/// Gets the effective segment register used to reference the memory location.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>, <see cref="OpKind.Memory64"/>,
		/// <see cref="OpKind.MemorySegSI"/>, <see cref="OpKind.MemorySegESI"/>, <see cref="OpKind.MemorySegRSI"/>
		/// </summary>
		public Register MemorySegment {
			get {
				var segReg = PrefixSegment;
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
		internal void InternalSetMemoryDisplSize(uint scale) {
			Debug.Assert(0 <= scale && scale <= 4);
			memoryFlags |= (ushort)(scale << (int)MemoryFlags.DisplSizeShift);
		}

		/// <summary>
		/// Gets the size of the memory location that is referenced by the operand.
		/// Use this property if the operand has kind <see cref="OpKind.Memory"/>, <see cref="OpKind.Memory64"/>,
		/// <see cref="OpKind.MemorySegSI"/>, <see cref="OpKind.MemorySegESI"/>, <see cref="OpKind.MemorySegRSI"/>,
		/// <see cref="OpKind.MemoryESDI"/>, <see cref="OpKind.MemoryESEDI"/>, <see cref="OpKind.MemoryESRDI"/>
		/// </summary>
		public MemorySize MemorySize {
			get => (MemorySize)(((uint)memoryFlags >> (int)MemoryFlags.MemorySizeShift) & (uint)MemoryFlags.MemorySizeMask);
			set => memoryFlags = (ushort)((memoryFlags & ~((uint)MemoryFlags.MemorySizeMask << (int)MemoryFlags.MemorySizeShift)) |
				(((uint)value & (uint)MemoryFlags.MemorySizeMask) << (int)MemoryFlags.MemorySizeShift));
		}
		internal MemorySize InternalMemorySize {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => memoryFlags |= (ushort)((uint)value << (int)MemoryFlags.MemorySizeShift);
		}

		/// <summary>
		/// Gets the index register scale value, valid values are *1, *2, *4, *8. Use this property if the operand has kind <see cref="OpKind.Memory"/>
		/// </summary>
		public int MemoryIndexScale {
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
			get => (int)(memoryFlags & (uint)MemoryFlags.ScaleMask);
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
			get => ((ulong)memDispl << 32) | immediate;
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
			get => ((ulong)memDispl << 32) | immediate;
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
		public ushort NearBranch16Target {
			get => (ushort)immediate;
			set => immediate = value;
		}
		internal uint InternalNearBranch16Target {
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.NearBranch32"/>
		/// </summary>
		public uint NearBranch32Target {
			get => immediate;
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.NearBranch64"/>
		/// </summary>
		public ulong NearBranch64Target {
			get => ((ulong)memDispl << 32) | immediate;
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
				case OpKind.NearBranch16:	return NearBranch16Target;
				case OpKind.NearBranch32:	return NearBranch32Target;
				case OpKind.NearBranch64:	return NearBranch64Target;
				default:					return 0;
				}
			}
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.FarBranch16"/>
		/// </summary>
		public ushort FarBranch16Target {
			get => (ushort)immediate;
			set => immediate = value;
		}
		internal uint InternalFarBranch16Target {
			set => immediate = value;
		}

		/// <summary>
		/// Gets the operand's branch target. Use this property if the operand has kind <see cref="OpKind.FarBranch32"/>
		/// </summary>
		public uint FarBranch32Target {
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
					ThrowArgumentOutOfRangeException(nameof(value));
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
				ThrowArgumentOutOfRangeException(nameof(operand));
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
			default: ThrowArgumentOutOfRangeException(nameof(operand)); break;
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
			get => (codeFlags >> (int)CodeFlags.OpMaskShift) & (uint)CodeFlags.OpMaskMask;
			set => codeFlags |= value << (int)CodeFlags.OpMaskShift;
		}

		/// <summary>
		/// true if there's an opmask register (<see cref="OpMask"/>)
		/// </summary>
		public bool HasOpMask => (codeFlags & ((uint)CodeFlags.OpMaskMask << (int)CodeFlags.OpMaskShift)) != 0;

		/// <summary>
		/// true if zeroing-masking, false if merging-masking.
		/// Only used by most EVEX encoded instructions that use opmask registers.
		/// </summary>
		public bool ZeroingMasking {
			get => (codeFlags & (uint)CodeFlags.ZeroingMasking) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.ZeroingMasking;
				else
					codeFlags &= ~(uint)CodeFlags.ZeroingMasking;
			}
		}
		internal void InternalSetZeroingMasking() => codeFlags |= (uint)CodeFlags.ZeroingMasking;

		/// <summary>
		/// true if merging-masking, false if zeroing-masking.
		/// Only used by most EVEX encoded instructions that use opmask registers.
		/// </summary>
		public bool MergingMasking {
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
			get => (RoundingControl)((codeFlags >> (int)CodeFlags.RoundingControlShift) & (int)CodeFlags.RoundingControlMask);
			set => codeFlags = (codeFlags & ~((uint)CodeFlags.RoundingControlMask << (int)CodeFlags.RoundingControlShift)) |
				(((uint)value & (uint)CodeFlags.RoundingControlMask) << (int)CodeFlags.RoundingControlShift);
		}
		internal uint InternalRoundingControl {
			set => codeFlags |= value << (int)CodeFlags.RoundingControlShift;
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
			get => (codeFlags & (uint)CodeFlags.SuppressAllExceptions) != 0;
			set {
				if (value)
					codeFlags |= (uint)CodeFlags.SuppressAllExceptions;
				else
					codeFlags &= ~(uint)CodeFlags.SuppressAllExceptions;
			}
		}
		internal void InternalSetSuppressAllExceptions() => codeFlags |= (uint)CodeFlags.SuppressAllExceptions;

		/// <summary>
		/// Checks if the memory operand is RIP/EIP relative
		/// </summary>
		public bool IsIPRelativeMemoryOp => MemoryBase == Register.RIP || MemoryBase == Register.EIP;

		/// <summary>
		/// Gets the RIP/EIP releative address ((<see cref="NextIP64"/> or <see cref="NextIP32"/>) + <see cref="MemoryDisplacement"/>). This property is only valid if there's a memory operand with RIP/EIP relative addressing.
		/// </summary>
		public ulong IPRelativeMemoryAddress {
			get {
				if (MemoryBase == Register.EIP)
					return NextIP32 + MemoryDisplacement;// 32-bit result
				return NextIP64 + (ulong)(int)MemoryDisplacement;// 64-bit result
			}
		}

		sealed class VARegisterValueProviderImpl : IVARegisterValueProvider {
			readonly VAGetRegisterValue getRegisterValue;
			public VARegisterValueProviderImpl(VAGetRegisterValue getRegisterValue) =>
				this.getRegisterValue = getRegisterValue ?? throw new ArgumentNullException(nameof(getRegisterValue));
			public ulong GetRegisterValue(Register register, int elementIndex, int elementSize) =>
				getRegisterValue(register, elementIndex, elementSize);
		}

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="operand">Operand number, must be a memory operand</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="getRegisterValue">Delegate that returns the value of a register or the base address of a segment register</param>
		/// <returns></returns>
		public ulong GetVirtualAddress(int operand, int elementIndex, VAGetRegisterValue getRegisterValue) =>
			GetVirtualAddress(operand, elementIndex, new VARegisterValueProviderImpl(getRegisterValue));

		/// <summary>
		/// Gets the virtual address of a memory operand
		/// </summary>
		/// <param name="operand">Operand number, must be a memory operand</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index of the vector index register.</param>
		/// <param name="registerValueProvider">Returns values of registers and segment base addresses</param>
		/// <returns></returns>
		public ulong GetVirtualAddress(int operand, int elementIndex, IVARegisterValueProvider registerValueProvider) {
			switch (GetOpKind(operand)) {
			case OpKind.Register:
			case OpKind.NearBranch16:
			case OpKind.NearBranch32:
			case OpKind.NearBranch64:
			case OpKind.FarBranch16:
			case OpKind.FarBranch32:
			case OpKind.Immediate8:
			case OpKind.Immediate8_2nd:
			case OpKind.Immediate16:
			case OpKind.Immediate32:
			case OpKind.Immediate64:
			case OpKind.Immediate8to16:
			case OpKind.Immediate8to32:
			case OpKind.Immediate8to64:
			case OpKind.Immediate32to64:
				return 0;

			case OpKind.MemorySegSI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (ushort)registerValueProvider.GetRegisterValue(Register.SI, 0, 0);

			case OpKind.MemorySegESI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (uint)registerValueProvider.GetRegisterValue(Register.ESI, 0, 0);

			case OpKind.MemorySegRSI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + registerValueProvider.GetRegisterValue(Register.RSI, 0, 0);

			case OpKind.MemorySegDI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (ushort)registerValueProvider.GetRegisterValue(Register.DI, 0, 0);

			case OpKind.MemorySegEDI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + (uint)registerValueProvider.GetRegisterValue(Register.EDI, 0, 0);

			case OpKind.MemorySegRDI:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + registerValueProvider.GetRegisterValue(Register.RDI, 0, 0);

			case OpKind.MemoryESDI:
				return registerValueProvider.GetRegisterValue(Register.ES, 0, 0) + (ushort)registerValueProvider.GetRegisterValue(Register.DI, 0, 0);

			case OpKind.MemoryESEDI:
				return registerValueProvider.GetRegisterValue(Register.ES, 0, 0) + (uint)registerValueProvider.GetRegisterValue(Register.EDI, 0, 0);

			case OpKind.MemoryESRDI:
				return registerValueProvider.GetRegisterValue(Register.ES, 0, 0) + registerValueProvider.GetRegisterValue(Register.RDI, 0, 0);

			case OpKind.Memory64:
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + MemoryAddress64;

			case OpKind.Memory:
				var baseReg = MemoryBase;
				var indexReg = MemoryIndex;
				int addrSize = InstructionUtils.GetAddressSizeInBytes(baseReg, indexReg, MemoryDisplSize, CodeSize);
				ulong offset = MemoryDisplacement;
				ulong offsetMask;
				if (addrSize == 8) {
					offset = (ulong)(int)offset;
					offsetMask = ulong.MaxValue;
				}
				else if (addrSize == 4)
					offsetMask = uint.MaxValue;
				else {
					Debug.Assert(addrSize == 2);
					offsetMask = ushort.MaxValue;
				}
				if (baseReg != Register.None) {
					if (baseReg == Register.RIP)
						offset += NextIP64;
					else if (baseReg == Register.EIP)
						offset += NextIP32;
					else
						offset += registerValueProvider.GetRegisterValue(baseReg, 0, 0);
				}
				if (indexReg != Register.None) {
					if (TryGetVsib64(out bool vsib64)) {
						if (vsib64)
							offset += registerValueProvider.GetRegisterValue(indexReg, elementIndex, 8) << InternalMemoryIndexScale;
						else
							offset += (ulong)(uint)registerValueProvider.GetRegisterValue(indexReg, elementIndex, 4) << InternalMemoryIndexScale;
					}
					else
						offset += registerValueProvider.GetRegisterValue(indexReg, elementIndex, 0) << InternalMemoryIndexScale;
				}
				offset &= offsetMask;
				return registerValueProvider.GetRegisterValue(MemorySegment, 0, 0) + offset;

			default:
				throw new InvalidOperationException();
			}
		}

#if !NO_INSTR_INFO
		/// <summary>
		/// Gets the number of bytes added to SP/ESP/RSP or 0 if it's not an instruction that pushes or pops data. This method
		/// assumes the instruction doesn't change privilege (eg. iret/d/q). If it's the leave instruction, this method returns 0.
		/// </summary>
		/// <returns></returns>
		public int StackPointerIncrement {
			get {
				switch (Code) {
				case Code.Pushw_ES:
				case Code.Pushw_CS:
				case Code.Pushw_SS:
				case Code.Pushw_DS:
				case Code.Push_AX:
				case Code.Push_R8W:
				case Code.Push_CX:
				case Code.Push_R9W:
				case Code.Push_DX:
				case Code.Push_R10W:
				case Code.Push_BX:
				case Code.Push_R11W:
				case Code.Push_SP:
				case Code.Push_R12W:
				case Code.Push_BP:
				case Code.Push_R13W:
				case Code.Push_SI:
				case Code.Push_R14W:
				case Code.Push_DI:
				case Code.Push_R15W:
				case Code.Push_imm16:
				case Code.Pushw_imm8:
				case Code.Pushfw:
				case Code.Push_rm16:
				case Code.Pushw_FS:
				case Code.Pushw_GS:
					return -2;

				case Code.Pushd_ES:
				case Code.Pushd_CS:
				case Code.Pushd_SS:
				case Code.Pushd_DS:
				case Code.Push_EAX:
				case Code.Push_ECX:
				case Code.Push_EDX:
				case Code.Push_EBX:
				case Code.Push_ESP:
				case Code.Push_EBP:
				case Code.Push_ESI:
				case Code.Push_EDI:
				case Code.Pushd_imm32:
				case Code.Pushd_imm8:
				case Code.Pushfd:
				case Code.Push_rm32:
				case Code.Pushd_FS:
				case Code.Pushd_GS:
					return -4;

				case Code.Push_RAX:
				case Code.Push_R8:
				case Code.Push_RCX:
				case Code.Push_R9:
				case Code.Push_RDX:
				case Code.Push_R10:
				case Code.Push_RBX:
				case Code.Push_R11:
				case Code.Push_RSP:
				case Code.Push_R12:
				case Code.Push_RBP:
				case Code.Push_R13:
				case Code.Push_RSI:
				case Code.Push_R14:
				case Code.Push_RDI:
				case Code.Push_R15:
				case Code.Pushq_imm32:
				case Code.Pushq_imm8:
				case Code.Pushfq:
				case Code.Push_rm64:
				case Code.Pushq_FS:
				case Code.Pushq_GS:
					return -8;

				case Code.Pushaw:
					return -2 * 8;

				case Code.Pushad:
					return -4 * 8;

				case Code.Popw_ES:
				case Code.Popw_SS:
				case Code.Popw_DS:
				case Code.Pop_AX:
				case Code.Pop_R8W:
				case Code.Pop_CX:
				case Code.Pop_R9W:
				case Code.Pop_DX:
				case Code.Pop_R10W:
				case Code.Pop_BX:
				case Code.Pop_R11W:
				case Code.Pop_SP:
				case Code.Pop_R12W:
				case Code.Pop_BP:
				case Code.Pop_R13W:
				case Code.Pop_SI:
				case Code.Pop_R14W:
				case Code.Pop_DI:
				case Code.Pop_R15W:
				case Code.Pop_rm16:
				case Code.Popfw:
				case Code.Popw_FS:
				case Code.Popw_GS:
					return 2;

				case Code.Popd_ES:
				case Code.Popd_SS:
				case Code.Popd_DS:
				case Code.Pop_EAX:
				case Code.Pop_ECX:
				case Code.Pop_EDX:
				case Code.Pop_EBX:
				case Code.Pop_ESP:
				case Code.Pop_EBP:
				case Code.Pop_ESI:
				case Code.Pop_EDI:
				case Code.Pop_rm32:
				case Code.Popfd:
				case Code.Popd_FS:
				case Code.Popd_GS:
					return 4;

				case Code.Pop_RAX:
				case Code.Pop_R8:
				case Code.Pop_RCX:
				case Code.Pop_R9:
				case Code.Pop_RDX:
				case Code.Pop_R10:
				case Code.Pop_RBX:
				case Code.Pop_R11:
				case Code.Pop_RSP:
				case Code.Pop_R12:
				case Code.Pop_RBP:
				case Code.Pop_R13:
				case Code.Pop_RSI:
				case Code.Pop_R14:
				case Code.Pop_RDI:
				case Code.Pop_R15:
				case Code.Pop_rm64:
				case Code.Popfq:
				case Code.Popq_FS:
				case Code.Popq_GS:
					return 8;

				case Code.Popaw:
					return 2 * 8;

				case Code.Popad:
					return 4 * 8;

				case Code.Call_ptr1616:
				case Code.Call_m1616:
					return -(2 + 2);

				case Code.Call_ptr3216:
				case Code.Call_m3216:
					return -(4 + 4);

				case Code.Call_m6416:
					return -(8 + 8);

				case Code.Call_rel16:
				case Code.Call_rm16:
					return -2;

				case Code.Call_rel32_32:
				case Code.Call_rm32:
					return -4;

				case Code.Call_rel32_64:
				case Code.Call_rm64:
					return -8;

				case Code.Retnw_imm16:
					return 2 + Immediate16;

				case Code.Retnd_imm16:
					return 4 + Immediate16;

				case Code.Retnq_imm16:
					return 8 + Immediate16;

				case Code.Retnw:
					return 2;

				case Code.Retnd:
					return 4;

				case Code.Retnq:
					return 8;

				case Code.Retfw_imm16:
					return 2 + 2 + Immediate16;

				case Code.Retfd_imm16:
					return 4 + 4 + Immediate16;

				case Code.Retfq_imm16:
					return 8 + 8 + Immediate16;

				case Code.Retfw:
					return 2 + 2;

				case Code.Retfd:
					return 4 + 4;

				case Code.Retfq:
					return 8 + 8;

				case Code.Iretw:
					if (CodeSize == CodeSize.Code64)
						return 2 * 5;
					return 2 * 3;

				case Code.Iretd:
					if (CodeSize == CodeSize.Code64)
						return 4 * 5;
					return 4 * 3;

				case Code.Iretq:
					return 8 * 5;

				case Code.Enterw_imm16_imm8:
					return -(2 + (Immediate8_2nd & 0x1F) * 2 + Immediate16);

				case Code.Enterd_imm16_imm8:
					return -(4 + (Immediate8_2nd & 0x1F) * 4 + Immediate16);

				case Code.Enterq_imm16_imm8:
					return -(8 + (Immediate8_2nd & 0x1F) * 8 + Immediate16);

				case Code.Leavew:
				case Code.Leaved:
				case Code.Leaveq:
					return 0;

				default:
					return 0;
				}
			}
		}

		/// <summary>
		/// (This method allocates and is slower than using an <see cref="InstructionInfoFactory"/>.)
		/// 
		/// Gets instruction info such as which register is read and written etc.
		/// </summary>
		/// <returns></returns>
		public InstructionInfo GetInfo() {
			var usedRegisters = InstructionInfoInternal.SimpleList<UsedRegister>.Empty;
			var usedMemoryLocations = InstructionInfoInternal.SimpleList<UsedMemory>.Empty;
			return InstructionInfoFactory.Create(ref this, ref usedRegisters, ref usedMemoryLocations, InstructionInfoOptions.None);
		}

		/// <summary>
		/// (This method allocates and is slower than using an <see cref="InstructionInfoFactory"/>.)
		/// 
		/// Gets instruction info such as which register is read and written etc.
		/// </summary>
		/// <param name="options">Options</param>
		/// <returns></returns>
		public InstructionInfo GetInfo(InstructionInfoOptions options) {
			var usedRegisters = InstructionInfoInternal.SimpleList<UsedRegister>.Empty;
			var usedMemoryLocations = InstructionInfoInternal.SimpleList<UsedMemory>.Empty;
			return InstructionInfoFactory.Create(ref this, ref usedRegisters, ref usedMemoryLocations, options);
		}

		/// <summary>
		/// (This method allocates and is slower than using an <see cref="InstructionInfoFactory"/>.)
		/// 
		/// Gets a struct iterator that returns all read and written registers. There are some exceptions, this method doesn't return all used registers:
		/// 
		/// 1) If <see cref="SaveRestoreInstruction"/> is true, or
		/// 
		/// 2) If it's a <see cref="FlowControl.Call"/> or <see cref="FlowControl.Interrupt"/> instruction (call, sysenter, int n etc), it can read and write any register (including RFLAGS).
		/// </summary>
		/// <returns></returns>
		public InstructionInfo.UsedRegisterIterator GetUsedRegisters() {
			var usedRegisters = InstructionInfoInternal.SimpleList<UsedRegister>.Empty;
			var usedMemoryLocations = InstructionInfoInternal.SimpleList<UsedMemory>.Empty;
			return InstructionInfoFactory.Create(ref this, ref usedRegisters, ref usedMemoryLocations, InstructionInfoOptions.NoMemoryUsage).GetUsedRegisters();
		}

		/// <summary>
		/// (This method allocates and is slower than using an <see cref="InstructionInfoFactory"/>.)
		/// 
		/// Gets a struct iterator that returns all read and written memory locations
		/// </summary>
		/// <returns></returns>
		public InstructionInfo.UsedMemoryIterator GetUsedMemory() {
			var usedRegisters = InstructionInfoInternal.SimpleList<UsedRegister>.Empty;
			var usedMemoryLocations = InstructionInfoInternal.SimpleList<UsedMemory>.Empty;
			return InstructionInfoFactory.Create(ref this, ref usedRegisters, ref usedMemoryLocations, InstructionInfoOptions.NoRegisterUsage).GetUsedMemory();
		}

		/// <summary>
		/// Instruction encoding, eg. legacy, VEX, EVEX, ...
		/// </summary>
		public EncodingKind Encoding => Code.Encoding();

		/// <summary>
		/// CPU or CPUID feature flag
		/// </summary>
		public CpuidFeature CpuidFeature {
			get {
				var code = Code;
				var cpuidFeature = code.CpuidFeature();
				if (cpuidFeature == CpuidFeature.AVX && Op1Kind == OpKind.Register && (code == Code.VEX_Vbroadcastss_xmm_xmmm32 || code == Code.VEX_Vbroadcastss_ymm_xmmm32 || code == Code.VEX_Vbroadcastsd_ymm_xmmm64))
					return CpuidFeature.AVX2;
				return cpuidFeature;
			}
		}

		/// <summary>
		/// Flow control info
		/// </summary>
		public FlowControl FlowControl => Code.FlowControl();

		/// <summary>
		/// true if the instruction isn't available in real mode or virtual 8086 mode
		/// </summary>
		public bool ProtectedMode => Code.ProtectedMode();

		/// <summary>
		/// true if this is a privileged instruction
		/// </summary>
		public bool Privileged => Code.Privileged();

		/// <summary>
		/// true if this is an instruction that implicitly uses the stack pointer (SP/ESP/RSP), eg. call, push, pop, ret, etc.
		/// See also <see cref="StackPointerIncrement"/>
		/// </summary>
		public bool StackInstruction => Code.StackInstruction();

		/// <summary>
		/// true if it's an instruction that saves or restores too many registers (eg. fxrstor, xsave, etc).
		/// </summary>
		public bool SaveRestoreInstruction => Code.SaveRestoreInstruction();

		InstructionInfoInternal.RflagsInfo GetRflagsInfo() {
			var flags1 = InstructionInfoInternal.InfoHandlers.Data[(int)Code << 1];
			var codeInfo = (InstructionInfoInternal.CodeInfo)((flags1 >> (int)InstructionInfoInternal.InfoFlags1.CodeInfoShift) & (uint)InstructionInfoInternal.InfoFlags1.CodeInfoMask);
			Debug.Assert(InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 + 1 == InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD11);
			Debug.Assert(InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 + 2 == InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1F);
			Debug.Assert(InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9 + 3 == InstructionInfoInternal.CodeInfo.Shift_Ib_MASK3F);
			if ((uint)(codeInfo - InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9) <= 3) {
				switch (codeInfo) {
				case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD9:
					if ((Immediate8 & 0x1F) % 9 == 0)
						return InstructionInfoInternal.RflagsInfo.None;
					break;

				case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1FMOD11:
					if ((Immediate8 & 0x1F) % 17 == 0)
						return InstructionInfoInternal.RflagsInfo.None;
					break;

				case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK1F:
					if ((Immediate8 & 0x1F) == 0)
						return InstructionInfoInternal.RflagsInfo.None;
					break;

				case InstructionInfoInternal.CodeInfo.Shift_Ib_MASK3F:
					if ((Immediate8 & 0x3F) == 0)
						return InstructionInfoInternal.RflagsInfo.None;
					break;
				}
			}
			return (InstructionInfoInternal.RflagsInfo)((flags1 >> (int)InstructionInfoInternal.InfoFlags1.RflagsInfoShift) & (uint)InstructionInfoInternal.InfoFlags1.RflagsInfoMask);
		}

		/// <summary>
		/// All flags that are read by the CPU when executing the instruction
		/// </summary>
		public RflagsBits RflagsRead => (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsRead[(int)GetRflagsInfo()];

		/// <summary>
		/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared. See also <see cref="RflagsModified"/>
		/// </summary>
		public RflagsBits RflagsWritten => (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsWritten[(int)GetRflagsInfo()];

		/// <summary>
		/// All flags that are always cleared by the CPU
		/// </summary>
		public RflagsBits RflagsCleared => (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsCleared[(int)GetRflagsInfo()];

		/// <summary>
		/// All flags that are always set by the CPU
		/// </summary>
		public RflagsBits RflagsSet => (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsSet[(int)GetRflagsInfo()];

		/// <summary>
		/// All flags that are undefined after executing the instruction
		/// </summary>
		public RflagsBits RflagsUndefined => (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsUndefined[(int)GetRflagsInfo()];

		/// <summary>
		/// All flags that are modified by the CPU. This is <see cref="RflagsWritten"/> + <see cref="RflagsCleared"/> + <see cref="RflagsSet"/> + <see cref="RflagsUndefined"/>
		/// </summary>
		public RflagsBits RflagsModified => (RflagsBits)InstructionInfoInternal.RflagsInfoConstants.flagsModified[(int)GetRflagsInfo()];
#endif

#if !NO_ENCODER
		static OpKind GetImmediateOpKind(Code code, int operand) {
			var handlers = EncoderInternal.OpCodeHandlers.Handlers;
			if ((uint)code >= (uint)handlers.Length)
				throw new ArgumentOutOfRangeException(nameof(code));
			var operands = handlers[(int)code].Operands;
			if ((uint)operand >= (uint)operands.Length)
				throw new ArgumentOutOfRangeException(nameof(operand), $"{code} doesn't have at least {operand + 1} operands");
			var opKind = operands[operand].GetImmediateOpKind();
			if (opKind == (OpKind)(-1))
				throw new ArgumentException($"{code}'s op{operand} isn't an immediate operand");
			return opKind;
		}

		static OpKind GetNearBranchOpKind(Code code, int operand) {
			var handlers = EncoderInternal.OpCodeHandlers.Handlers;
			if ((uint)code >= (uint)handlers.Length)
				throw new ArgumentOutOfRangeException(nameof(code));
			var operands = handlers[(int)code].Operands;
			if ((uint)operand >= (uint)operands.Length)
				throw new ArgumentOutOfRangeException(nameof(operand), $"{code} doesn't have at least {operand + 1} operands");
			var opKind = operands[operand].GetNearBranchOpKind();
			if (opKind == (OpKind)(-1))
				throw new ArgumentException($"{code}'s op{operand} isn't a near branch operand");
			return opKind;
		}

		static OpKind GetFarBranchOpKind(Code code, int operand) {
			var handlers = EncoderInternal.OpCodeHandlers.Handlers;
			if ((uint)code >= (uint)handlers.Length)
				throw new ArgumentOutOfRangeException(nameof(code));
			var operands = handlers[(int)code].Operands;
			if ((uint)operand >= (uint)operands.Length)
				throw new ArgumentOutOfRangeException(nameof(operand), $"{code} doesn't have at least {operand + 1} operands");
			var opKind = operands[operand].GetFarBranchOpKind();
			if (opKind == (OpKind)(-1))
				throw new ArgumentException($"{code}'s op{operand} isn't a far branch operand");
			return opKind;
		}

		static MemorySize GetMemorySize(Code code) {
			switch (code) {
			case Code.Movsb_m8_m8:
			case Code.Cmpsb_m8_m8:
			case Code.Stosb_m8_AL:
			case Code.Lodsb_AL_m8:
			case Code.Scasb_AL_m8:
			case Code.Insb_m8_DX:
			case Code.Outsb_DX_m8:
				return MemorySize.UInt8;

			case Code.Movsw_m16_m16:
			case Code.Cmpsw_m16_m16:
			case Code.Stosw_m16_AX:
			case Code.Lodsw_AX_m16:
			case Code.Scasw_AX_m16:
			case Code.Insw_m16_DX:
			case Code.Outsw_DX_m16:
				return MemorySize.UInt16;

			case Code.Movsd_m32_m32:
			case Code.Cmpsd_m32_m32:
			case Code.Stosd_m32_EAX:
			case Code.Lodsd_EAX_m32:
			case Code.Scasd_EAX_m32:
			case Code.Insd_m32_DX:
			case Code.Outsd_DX_m32:
				return MemorySize.UInt32;

			case Code.Movsq_m64_m64:
			case Code.Cmpsq_m64_m64:
			case Code.Stosq_m64_RAX:
			case Code.Lodsq_RAX_m64:
			case Code.Scasq_RAX_m64:
				return MemorySize.UInt64;

			case Code.Maskmovq_rDI_mm_mm:
				return MemorySize.UInt64;

			case Code.Maskmovdqu_rDI_xmm_xmm:
			case Code.VEX_Vmaskmovdqu_rDI_xmm_xmm:
				return MemorySize.UInt128;

			default:
				throw new ArgumentOutOfRangeException(nameof(code));
			}
		}

		/// <summary>
		/// Creates a new <see cref="Instruction"/> with no operands
		/// </summary>
		/// <param name="code">Code value</param>
		/// <returns></returns>
		public static Instruction Create(Code code) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(instruction.OpCount == 0);
			return instruction;
		}

		/// <summary>
		/// Creates a new near/short branch <see cref="Instruction"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="target">Target address</param>
		/// <returns></returns>
		public static Instruction CreateBranch(Code code, ulong target) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.Op0Kind = GetNearBranchOpKind(code, 0);
			instruction.NearBranch64Target = target;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates a new far branch <see cref="Instruction"/>
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="selector">Selector/segment value</param>
		/// <param name="offset">Offset</param>
		/// <returns></returns>
		public static Instruction CreateBranch(Code code, ushort selector, uint offset) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.Op0Kind = GetFarBranchOpKind(code, 0);
			instruction.FarBranchSelector = selector;
			instruction.FarBranch32Target = offset;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction with a 64-bit memory offset as the second operand, eg. 'mov al,[123456789ABCDEF0]'
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register (al, ax, eax, rax)</param>
		/// <param name="address">64-bit address</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateMemory64(Code code, Register register, ulong address, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Memory64;
			instruction.MemoryAddress64 = address;
			if (register == Register.AL)
				instruction.InternalMemorySize = MemorySize.UInt8;
			else if (register == Register.AX)
				instruction.InternalMemorySize = MemorySize.UInt16;
			else if (register == Register.EAX)
				instruction.InternalMemorySize = MemorySize.UInt32;
			else if (register == Register.RAX)
				instruction.InternalMemorySize = MemorySize.UInt64;
			else
				throw new ArgumentOutOfRangeException(nameof(register));
			instruction.InternalSetMemoryDisplSize(4);
			instruction.PrefixSegment = prefixSegment;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction with a 64-bit memory offset as the first operand, eg. 'mov [123456789ABCDEF0],al'
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="address">64-bit address</param>
		/// <param name="register">Register (al, ax, eax, rax)</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateMemory64(Code code, ulong address, Register register, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory64;
			instruction.MemoryAddress64 = address;
			if (register == Register.AL)
				instruction.InternalMemorySize = MemorySize.UInt8;
			else if (register == Register.AX)
				instruction.InternalMemorySize = MemorySize.UInt16;
			else if (register == Register.EAX)
				instruction.InternalMemorySize = MemorySize.UInt32;
			else if (register == Register.RAX)
				instruction.InternalMemorySize = MemorySize.UInt64;
			else
				throw new ArgumentOutOfRangeException(nameof(register));
			instruction.InternalSetMemoryDisplSize(4);
			instruction.PrefixSegment = prefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = GetImmediateOpKind(code, 0);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(instruction.OpCount == 1);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			var opKind = GetImmediateOpKind(code, 1);
			instruction.InternalOp1Kind = opKind;
			if (opKind == OpKind.Immediate64)
				instruction.Immediate64 = (ulong)immediate;
			else
				instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction with a 64-bit immediate value
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">64-bit immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, long immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Immediate64;
			instruction.Immediate64 = (ulong)immediate;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register (eg. dx, al, ax, eax, rax)</param>
		/// <param name="rSI">si, esi, or rsi</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateString_Reg_SegRSI(Code code, Register register, Register rSI, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			if (rSI == Register.RSI)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else if (rSI == Register.ESI)
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			else if (rSI == Register.SI)
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
			else
				throw new ArgumentOutOfRangeException(nameof(rSI));

			instruction.PrefixSegment = prefixSegment;

			instruction.InternalMemorySize = GetMemorySize(code);

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register (eg. al, ax, eax, rax)</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <returns></returns>
		public static Instruction CreateString_Reg_ESRDI(Code code, Register register, Register rDI) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			if (rDI == Register.RDI)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			instruction.InternalMemorySize = GetMemorySize(code);

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, int immediate, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = GetImmediateOpKind(code, 0);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, int immediate, byte immediate2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = GetImmediateOpKind(code, 0);
			instruction.Immediate32 = (uint)immediate;

			instruction.InternalOp1Kind = OpKind.Immediate8_2nd;
			instruction.Immediate8_2nd = immediate2;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rSI">si, esi, or rsi</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateString_SegRSI_ESRDI(Code code, Register rSI, Register rDI, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rSI == Register.RSI)
				instruction.InternalOp0Kind = OpKind.MemorySegRSI;
			else if (rSI == Register.ESI)
				instruction.InternalOp0Kind = OpKind.MemorySegESI;
			else if (rSI == Register.SI)
				instruction.InternalOp0Kind = OpKind.MemorySegSI;
			else
				throw new ArgumentOutOfRangeException(nameof(rSI));

			if (rDI == Register.RDI)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			instruction.PrefixSegment = prefixSegment;

			instruction.InternalMemorySize = GetMemorySize(code);

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="register">Register (eg. dx, al, ax, eax, rax)</param>
		/// <returns></returns>
		public static Instruction CreateString_ESRDI_Reg(Code code, Register rDI, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rDI == Register.RDI)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			instruction.InternalMemorySize = GetMemorySize(code);

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="rSI">si, esi, or rsi</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateString_ESRDI_SegRSI(Code code, Register rDI, Register rSI, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rDI == Register.RDI)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			if (rSI == Register.RSI)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else if (rSI == Register.ESI)
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			else if (rSI == Register.SI)
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
			else
				throw new ArgumentOutOfRangeException(nameof(rSI));

			instruction.PrefixSegment = prefixSegment;

			instruction.InternalMemorySize = GetMemorySize(code);

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, Register register) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			instruction.InternalOp1Kind = GetImmediateOpKind(code, 1);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 2);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, int immediate, byte immediate2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = GetImmediateOpKind(code, 1);
			instruction.Immediate32 = (uint)immediate;

			instruction.InternalOp2Kind = OpKind.Immediate8_2nd;
			instruction.Immediate8_2nd = immediate2;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register2">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, in MemoryOperand memory, Register register2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			instruction.InternalOp1Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register2;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;

			instruction.InternalOp1Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="rDI">di, edi, or rdi</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="prefixSegment">Segment override or <see cref="Register.None"/></param>
		/// <returns></returns>
		public static Instruction CreateMaskmov_SegRDI_Reg_Reg(Code code, Register rDI, Register register1, Register register2, Register prefixSegment = Register.None) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			if (rDI == Register.RDI)
				instruction.InternalOp0Kind = OpKind.MemorySegRDI;
			else if (rDI == Register.EDI)
				instruction.InternalOp0Kind = OpKind.MemorySegEDI;
			else if (rDI == Register.DI)
				instruction.InternalOp0Kind = OpKind.MemorySegDI;
			else
				throw new ArgumentOutOfRangeException(nameof(rDI));

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register2;

			instruction.PrefixSegment = prefixSegment;

			instruction.InternalMemorySize = GetMemorySize(code);

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, Register register1, Register register2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register2;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, in MemoryOperand memory, Register register, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			instruction.InternalOp0Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 3);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="register4">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, Register register4) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register4;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			instruction.InternalOp3Kind = GetImmediateOpKind(code, 3);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, in MemoryOperand memory) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			instruction.InternalOp3Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <param name="immediate2">Second immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, int immediate, byte immediate2) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = GetImmediateOpKind(code, 2);
			instruction.Immediate32 = (uint)immediate;

			instruction.InternalOp3Kind = OpKind.Immediate8_2nd;
			instruction.Immediate8_2nd = immediate2;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register3">Register</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, Register register3) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register3;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			instruction.InternalOp3Kind = GetImmediateOpKind(code, 3);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 4);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="register4">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, Register register4, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register4;

			instruction.InternalOp4Kind = GetImmediateOpKind(code, 4);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 5);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="register3">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, Register register3, in MemoryOperand memory, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = register3;

			instruction.InternalOp3Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			instruction.InternalOp4Kind = GetImmediateOpKind(code, 4);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 5);
			return instruction;
		}

		/// <summary>
		/// Creates an instruction
		/// </summary>
		/// <param name="code">Code value</param>
		/// <param name="register1">Register</param>
		/// <param name="register2">Register</param>
		/// <param name="memory">Memory operand</param>
		/// <param name="register3">Register</param>
		/// <param name="immediate">Immediate</param>
		/// <returns></returns>
		public static Instruction Create(Code code, Register register1, Register register2, in MemoryOperand memory, Register register3, int immediate) {
			Instruction instruction = default;
			instruction.InternalCode = code;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register1;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = register2;

			instruction.InternalOp2Kind = OpKind.Memory;
			instruction.InternalMemoryBase = memory.Base;
			instruction.InternalMemoryIndex = memory.Index;
			instruction.MemoryIndexScale = memory.Scale;
			instruction.MemoryDisplSize = memory.DisplSize;
			instruction.MemoryDisplacement = (uint)memory.Displacement;
			instruction.InternalMemorySize = memory.Size;
			instruction.PrefixSegment = memory.PrefixSegment;

			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp3Kind = OpKind.Register;
			instruction.InternalOp3Register = register3;

			instruction.InternalOp4Kind = GetImmediateOpKind(code, 4);
			instruction.Immediate32 = (uint)immediate;

			Debug.Assert(instruction.OpCount == 5);
			return instruction;
		}
#endif

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
	/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base value,
	/// not the segment register value.
	/// </summary>
	/// <param name="register">Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg)</param>
	/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index in the vector register.</param>
	/// <param name="elementSize">Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).</param>
	/// <returns></returns>
	public delegate ulong VAGetRegisterValue(Register register, int elementIndex, int elementSize);

	/// <summary>
	/// Called when calculating the virtual address of a memory operand
	/// </summary>
	public interface IVARegisterValueProvider {
		/// <summary>
		/// Gets a register value. If <paramref name="register"/> is a segment register, this method should return the segment's base value,
		/// not the segment register value.
		/// </summary>
		/// <param name="register">Register (GPR8, GPR16, GPR32, GPR64, XMM, YMM, ZMM, seg)</param>
		/// <param name="elementIndex">Only used if it's a vsib memory operand. This is the element index in the vector register.</param>
		/// <param name="elementSize">Only used if it's a vsib memory operand. Size in bytes of elements in vector index register (4 or 8).</param>
		/// <returns></returns>
		ulong GetRegisterValue(Register register, int elementIndex, int elementSize);
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
