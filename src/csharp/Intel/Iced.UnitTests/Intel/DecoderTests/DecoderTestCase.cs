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
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	sealed class DecoderTestCase {
		public int LineNumber;
		public bool CanEncode;
		public DecoderOptions DecoderOptions;
		public int Bitness;
		public string HexBytes;
		public string EncodedHexBytes;
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
		public uint MemoryDisplacement;
		public ulong Immediate;
		public byte Immediate_2nd;
		public ulong MemoryAddress64;
		public ulong NearBranch;
		public uint FarBranch;
		public ushort FarBranchSelector;
		public Register Op0Register, Op1Register, Op2Register, Op3Register, Op4Register;
		public ConstantOffsets ConstantOffsets;

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
		public Code Code;
		public Register Register;
		public Register SegmentPrefix;
		public Register SegmentRegister;
		public Register BaseRegister;
		public Register IndexRegister;
		public int Scale;
		public uint Displacement;
		public int DisplacementSize;
		public bool CanEncode;
		public ConstantOffsets ConstantOffsets;
	}
}
