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

namespace IcedFuzzer.Core {
	enum FuzzerOperandKind {
		None,
		Immediate,
		MemOffs,
		ImpliedMem,
		Mem,
		Register,
	}

	class FuzzerOperand {
		public readonly FuzzerOperandKind Kind;
		public FuzzerOperand(FuzzerOperandKind kind) => Kind = kind;
	}

	enum FuzzerImmediateKind {
		Imm1,
		Imm2,
		Imm4,
		Imm8,
		Imm2_2,
		Imm4_2,
	}

	sealed class ImmediateFuzzerOperand : FuzzerOperand {
		public readonly FuzzerImmediateKind ImmKind;
		public ImmediateFuzzerOperand(FuzzerImmediateKind kind)
			: base(FuzzerOperandKind.Immediate) => ImmKind = kind;
	}

	[Flags]
	enum ModrmMemoryFuzzerOperandFlags : uint {
		None					= 0,
		// 16/32-bit mode: must be 32-bit addressing, else #UD. 64-bit mode: 64-bit addressing is forced (67h has no effect)
		MPX						= 0x00000001,
		// RIP-rel operands #UD
		NoRipRel				= 0x00000002,
		// VSIB operand
		Vsib					= 0x00000004,
		// SIB required
		Sib						= 0x00000008,
	}

	sealed class ModrmMemoryFuzzerOperand : FuzzerOperand {
		readonly ModrmMemoryFuzzerOperandFlags flags;

		public bool IsVSIB => (flags & ModrmMemoryFuzzerOperandFlags.Vsib) != 0;
		public bool MustNotUseAddrSize16 => (flags & (ModrmMemoryFuzzerOperandFlags.MPX | ModrmMemoryFuzzerOperandFlags.Vsib | ModrmMemoryFuzzerOperandFlags.Sib)) != 0;
		public bool NoRipRel => (flags & ModrmMemoryFuzzerOperandFlags.NoRipRel) != 0;
		public bool MustUseSib => (flags & (ModrmMemoryFuzzerOperandFlags.Sib | ModrmMemoryFuzzerOperandFlags.Vsib)) != 0;

		public ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags flags)
			: base(FuzzerOperandKind.Mem) => this.flags = flags;
	}

	enum FuzzerRegisterKind {
		GPR8,
		GPR16,
		GPR32,
		GPR64,
		Segment,
		ST,
		CR,
		DR,
		TR,
		BND,
		K,
		MM,
		XMM,
		YMM,
		ZMM,
		TMM,
	}
	enum FuzzerRegisterClass {
		GPR,
		Segment,
		ST,
		CR,
		DR,
		TR,
		BND,
		K,
		MM,
		Vector,
		TMM,
	}

	enum FuzzerOperandRegLocation {
		// [4] = EVEX.R'
		// [3] = REX/VEX/EVEX/XOP.R
		// [2:0] = modrm.reg
		ModrmRegBits,
		// [4] = EVEX.X
		// [3] = REX/VEX/EVEX/XOP.B
		// [2:0] = modrm.rm
		ModrmRmBits,
		// [4] = EVEX.V'
		// [3:0] = VEX/EVEX/XOP.vvvv
		VvvvBits,
		// [3] = REX.B
		// [2:0] = opcode[2:0]
		OpCodeBits,
		// [3:0] = imm8[7:4]
		Is4Bits,
		// [3:0] = imm8[7:4]
		Is5Bits,
		// [2:0] = aaa
		AaaBits,
	}

	sealed partial class RegisterFuzzerOperand : FuzzerOperand {
		public readonly FuzzerRegisterClass RegisterClass;
		public readonly FuzzerRegisterKind Register;
		public readonly FuzzerOperandRegLocation RegLocation;

		public RegisterFuzzerOperand(FuzzerRegisterClass registerClass, FuzzerRegisterKind register, FuzzerOperandRegLocation regLocation)
			: base(FuzzerOperandKind.Register) {
			RegisterClass = registerClass;
			Register = register;
			RegLocation = regLocation;
		}

		public RegisterInfo GetRegisterInfo(int bitness, FuzzerEncodingKind encoding) {
			switch (encoding) {
			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
				switch (RegLocation) {
				case FuzzerOperandRegLocation.ModrmRegBits:
				case FuzzerOperandRegLocation.ModrmRmBits:
				case FuzzerOperandRegLocation.OpCodeBits:
					if (bitness < 64)
						return new RegisterInfo(bitness, RegLocation, 8);
					else
						return new RegisterInfo(bitness, RegLocation, 16);

				case FuzzerOperandRegLocation.VvvvBits:
				case FuzzerOperandRegLocation.Is4Bits:
				case FuzzerOperandRegLocation.Is5Bits:
				case FuzzerOperandRegLocation.AaaBits:
					throw ThrowHelpers.Unreachable;

				default:
					throw ThrowHelpers.Unreachable;
				}

			case FuzzerEncodingKind.VEX2:
				switch (RegLocation) {
				case FuzzerOperandRegLocation.ModrmRmBits:
					return new RegisterInfo(bitness, RegLocation, 8);

				case FuzzerOperandRegLocation.VvvvBits:
				case FuzzerOperandRegLocation.ModrmRegBits:
					if (bitness < 64)
						return new RegisterInfo(bitness, RegLocation, 8);
					else
						return new RegisterInfo(bitness, RegLocation, 16);

				case FuzzerOperandRegLocation.Is4Bits:
				case FuzzerOperandRegLocation.Is5Bits:
					return new RegisterInfo(bitness, RegLocation, 256);

				case FuzzerOperandRegLocation.OpCodeBits:
				case FuzzerOperandRegLocation.AaaBits:
					throw ThrowHelpers.Unreachable;

				default:
					throw ThrowHelpers.Unreachable;
				}

			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
				switch (RegLocation) {
				case FuzzerOperandRegLocation.ModrmRegBits:
					if (bitness < 64 && encoding != FuzzerEncodingKind.XOP)
						return new RegisterInfo(bitness, RegLocation, 8);
					else
						return new RegisterInfo(bitness, RegLocation, 16);

				case FuzzerOperandRegLocation.ModrmRmBits:
				case FuzzerOperandRegLocation.VvvvBits:
					return new RegisterInfo(bitness, RegLocation, 16);

				case FuzzerOperandRegLocation.Is4Bits:
				case FuzzerOperandRegLocation.Is5Bits:
					return new RegisterInfo(bitness, RegLocation, 256);

				case FuzzerOperandRegLocation.OpCodeBits:
				case FuzzerOperandRegLocation.AaaBits:
					throw ThrowHelpers.Unreachable;

				default:
					throw ThrowHelpers.Unreachable;
				}

			case FuzzerEncodingKind.EVEX:
				switch (RegLocation) {
				case FuzzerOperandRegLocation.ModrmRegBits:
					// 16/32-bit: it's R'Rrrr but R must be 0 (or it's BOUND). We return 8, and ignored-bits test will test R'
					if (bitness < 64)
						return new RegisterInfo(bitness, RegLocation, 8);
					else
						return new RegisterInfo(bitness, RegLocation, 32);

				case FuzzerOperandRegLocation.ModrmRmBits:
					// 16/32-bit: it's XBbbb but X must be 0 (inverted: 1) or it's BOUND
					if (bitness < 64)
						return new RegisterInfo(bitness, RegLocation, 16);
					else
						return new RegisterInfo(bitness, RegLocation, 32);

				case FuzzerOperandRegLocation.VvvvBits:
					return new RegisterInfo(bitness, RegLocation, 32);

				case FuzzerOperandRegLocation.AaaBits:
					return new RegisterInfo(bitness, RegLocation, 8);

				case FuzzerOperandRegLocation.OpCodeBits:
				case FuzzerOperandRegLocation.Is4Bits:
				case FuzzerOperandRegLocation.Is5Bits:
					throw ThrowHelpers.Unreachable;

				default:
					throw ThrowHelpers.Unreachable;
				}

			default:
				throw ThrowHelpers.Unreachable;
			}
		}
	}
}
