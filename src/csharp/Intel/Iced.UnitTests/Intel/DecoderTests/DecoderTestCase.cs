// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
#if MVEX
	struct MvexDecoderInfo {
		public bool EvictionHint;
		public MvexRegMemConv RegMemConv;
	}
#endif

	sealed class DecoderTestCase {
		public int LineNumber;
		public DecoderTestOptions TestOptions;
		public DecoderError DecoderError;
		public DecoderOptions DecoderOptions;
		public int Bitness;
		public string HexBytes;
		public string EncodedHexBytes;
		public ulong IP;
		public Code Code;
		public Mnemonic Mnemonic;
		public int OpCount;
		public bool ZeroingMasking;
		public bool SuppressAllExceptions;
		public bool IsBroadcast;
		public bool HasXacquirePrefix;
		public bool HasXreleasePrefix;
		public bool HasRepePrefix;
		public bool HasRepnePrefix;
		public bool HasLockPrefix;
		public int VsibBitness;
		public Register OpMask;
		public RoundingControl RoundingControl;
		public OpKind Op0Kind, Op1Kind, Op2Kind, Op3Kind, Op4Kind;
		public Register SegmentPrefix;
		public Register MemorySegment;
		public Register MemoryBase;
		public Register MemoryIndex;
		public int MemoryDisplSize;
		public MemorySize MemorySize;
		public int MemoryIndexScale;
		public ulong MemoryDisplacement;
		public ulong Immediate;
		public byte Immediate_2nd;
		public ulong NearBranch;
		public uint FarBranch;
		public ushort FarBranchSelector;
		public Register Op0Register, Op1Register, Op2Register, Op3Register, Op4Register;
		public ConstantOffsets ConstantOffsets;
#if MVEX
		public MvexDecoderInfo Mvex;
#endif

		public OpKind GetOpKind(int operand) =>
			operand switch {
				0 => Op0Kind,
				1 => Op1Kind,
				2 => Op2Kind,
				3 => Op3Kind,
				4 => Op4Kind,
				_ => throw new ArgumentOutOfRangeException(nameof(operand)),
			};

		public void SetOpKind(int operand, OpKind opKind) {
			switch (operand) {
			case 0: Op0Kind = opKind; break;
			case 1: Op1Kind = opKind; break;
			case 2: Op2Kind = opKind; break;
			case 3: Op3Kind = opKind; break;
			case 4: Op4Kind = opKind; break;
			default: throw new ArgumentOutOfRangeException(nameof(operand));
			}
		}

		public Register GetOpRegister(int operand) =>
			operand switch {
				0 => Op0Register,
				1 => Op1Register,
				2 => Op2Register,
				3 => Op3Register,
				4 => Op4Register,
				_ => throw new ArgumentOutOfRangeException(nameof(operand)),
			};

		public void SetOpRegister(int operand, Register register) {
			switch (operand) {
			case 0: Op0Register = register; break;
			case 1: Op1Register = register; break;
			case 2: Op2Register = register; break;
			case 3: Op3Register = register; break;
			case 4: Op4Register = register; break;
			default: throw new ArgumentOutOfRangeException(nameof(operand));
			}
		}
	}

	sealed class DecoderMemoryTestCase {
		public int LineNumber;
		public DecoderOptions DecoderOptions;
		public int Bitness;
		public string HexBytes;
		public string EncodedHexBytes;
		public ulong IP;
		public Code Code;
		public Register Register;
		public Register SegmentPrefix;
		public Register SegmentRegister;
		public Register BaseRegister;
		public Register IndexRegister;
		public int Scale;
		public ulong Displacement;
		public int DisplacementSize;
		public DecoderTestOptions TestOptions;
		public ConstantOffsets ConstantOffsets;
	}
}
