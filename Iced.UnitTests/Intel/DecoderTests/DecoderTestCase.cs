/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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
using Iced.Intel;

namespace Iced.UnitTests.Intel.DecoderTests {
	sealed class DecoderTestCase {
		public int LineNumber;
		public DecoderOptions DecoderOptions;
		public int Bitness;
		public string HexBytes;
		public Code Code;
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

		public OpKind GetOpKind(int operand) {
			switch (operand) {
			case 0: return Op0Kind;
			case 1: return Op1Kind;
			case 2: return Op2Kind;
			case 3: return Op3Kind;
			case 4: return Op4Kind;
			default: throw new ArgumentOutOfRangeException(nameof(operand));
			}
		}

		public Register GetOpRegister(int operand) {
			switch (operand) {
			case 0: return Op0Register;
			case 1: return Op1Register;
			case 2: return Op2Register;
			case 3: return Op3Register;
			case 4: return Op4Register;
			default: throw new ArgumentOutOfRangeException(nameof(operand));
			}
		}
	}
}
