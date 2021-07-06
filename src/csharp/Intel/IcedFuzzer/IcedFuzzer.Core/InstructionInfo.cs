// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Iced.Intel;

namespace IcedFuzzer.Core {
	[Flags]
	enum EncodedInfoFlags : uint {
		None				= 0,
		HasREX				= 0x00000001,
		HasModrm			= 0x00000002,
		HasSib				= 0x00000004,
		HasOpCodeBits		= 0x00000008,
	}

	[Flags]
	enum UsedBits : uint {
		None				= 0,
		r					= 0x00000001,
		x					= 0x00000002,
		b					= 0x00000004,
		r2					= 0x00000008,
		vvvv				= 0x00000010,
		z					= 0x00000020,
		bcst				= 0x00000040,
		v2					= 0x00000080,
		aaa					= 0x00000100,
		w					= 0x00000200,
		modrm_mod			= 0x00000400,
		modrm_reg			= 0x00000800,
		modrm_rm			= 0x00001000,
	}

	enum WritePrefixKind {
		RawBytes,
		AddressSize,
		OperandSize,
		MandatoryPrefix,
	}

	readonly struct WritePrefix {
		public static readonly WritePrefix[] DefaultPrefixes = new WritePrefix[] {
			new WritePrefix(WritePrefixKind.AddressSize),
			new WritePrefix(WritePrefixKind.OperandSize),
			new WritePrefix(WritePrefixKind.MandatoryPrefix),
		};

		public readonly WritePrefixKind Kind;
		public readonly byte[] Prefixes;
		public readonly byte Prefix;

		public WritePrefix(byte[] prefixes) {
			Kind = WritePrefixKind.RawBytes;
			Prefixes = prefixes;
			Prefix = 0;
		}

		public WritePrefix(WritePrefixKind kind) {
			Assert.True(kind != WritePrefixKind.RawBytes);
			Kind = kind;
			Prefixes = Array.Empty<byte>();
			Prefix = 0;
		}
	}

	struct InstructionInfo {
		public uint r {
			readonly get => bf_r;
			set {
				Assert.True(value <= 1);
				SetUsedBits(UsedBits.r);
				bf_r = value;
			}
		}

		public uint x {
			readonly get => bf_x;
			set {
				Assert.True(value <= 1);
				SetUsedBits(UsedBits.x);
				bf_x = value;
			}
		}

		public uint b {
			readonly get => bf_b;
			set {
				Assert.True(value <= 1);
				SetUsedBits(UsedBits.b);
				bf_b = value;
			}
		}

		public uint r2 {
			readonly get => bf_r2;
			set {
				Assert.True(value <= 1);
				SetUsedBits(UsedBits.r2);
				bf_r2 = value;
			}
		}

		public uint vvvv {
			readonly get => bf_vvvv;
			set {
				Assert.True(value <= 0xF);
				SetUsedBits(UsedBits.vvvv);
				bf_vvvv = value;
			}
		}

		public uint z {
			readonly get => bf_z;
			set {
				Assert.True(value <= 1);
				SetUsedBits(UsedBits.z);
				bf_z = value;
			}
		}

		public uint bcst {
			readonly get => bf_bcst;
			set {
				Assert.True(value <= 1);
				SetUsedBits(UsedBits.bcst);
				bf_bcst = value;
			}
		}

		public uint v2 {
			readonly get => bf_v2;
			set {
				Assert.True(value <= 1);
				SetUsedBits(UsedBits.v2);
				bf_v2 = value;
			}
		}

		public uint aaa {
			readonly get => bf_aaa;
			set {
				Assert.True(value <= 7);
				SetUsedBits(UsedBits.aaa);
				bf_aaa = value;
			}
		}

		uint bf_r;
		uint bf_x;
		uint bf_b;
		uint bf_r2;
		uint bf_vvvv;
		uint bf_z;
		uint bf_bcst;
		uint bf_v2;
		uint bf_aaa;

		public readonly int Bitness;
		public readonly FuzzerInstruction Instruction;
		public readonly FuzzerEncodingKind Encoding;
		public readonly OpCode OpCode;
		public uint w;
		public uint l;
		public readonly uint pp;
		public readonly uint mmmmm;

		public EncodedInfoFlags Flags;
		public readonly UsedBits UsedBits => usedBits;
		UsedBits usedBits;
		public WritePrefix[] WritePrefixes;
		public uint EVEX_res3;
		public uint EVEX_res10;
		public uint modrm;
		public uint sib;
		public uint imm0;
		public uint imm0Hi;
		public uint imm1;
		public byte imm0Size;
		public byte imm1Size;

		public byte addressSize;
		// 0 if no prefix, else the actual prefix byte (eg. 66h)
		public byte addressSizePrefix;
		// legacy/3dnow only
		public byte operandSizePrefix;
		public byte mandatoryPrefix;
		public byte opCodeBits;

		public bool IsValid;

		public void SetUsedBits(UsedBits bits) {
			Assert.True((usedBits & bits) == 0);
			usedBits |= bits;
		}

		public static InstructionInfo Create(FuzzerGenContext context) =>
			new InstructionInfo(context.Bitness, context.Instruction, context.Encoding);

		InstructionInfo(int bitness, FuzzerInstruction instruction, FuzzerEncodingKind encoding) {
			Bitness = bitness;
			Instruction = instruction;
			Encoding = encoding;
			OpCode = instruction.OpCode;
			WritePrefixes = WritePrefix.DefaultPrefixes;
			IsValid = true;
			bf_r = 0;
			bf_x = 0;
			bf_b = 0;
			bf_r2 = 0;
			bf_vvvv = 0;
			bf_z = 0;
			bf_bcst = 0;
			bf_v2 = 0;
			bf_aaa = 0;
			EVEX_res3 = 0;
			EVEX_res10 = 1;
			w = instruction.W;
			l = instruction.L;
			pp = GetPrefixBits(instruction.MandatoryPrefix);
			mmmmm = (uint)instruction.Table.TableIndex;
			Flags = EncodedInfoFlags.None;
			usedBits = UsedBits.None;
			modrm = 0;
			if (instruction.GroupIndex >= 0) {
				Flags |= EncodedInfoFlags.HasModrm;
				usedBits |= UsedBits.modrm_reg;
				modrm |= (uint)instruction.GroupIndex << 3;
			}
			if (instruction.RmGroupIndex >= 0) {
				Flags |= EncodedInfoFlags.HasModrm;
				usedBits |= UsedBits.modrm_rm;
				modrm |= (uint)instruction.RmGroupIndex;
				if (!instruction.IsModrmMemory) {
					usedBits |= UsedBits.modrm_mod;
					modrm |= 0xC0;
				}
			}
			sib = 0;
			imm0 = 0;
			imm0Hi = 0;
			imm1 = 0;
			imm0Size = 0;
			imm1Size = 0;
			addressSize = (byte)instruction.AddressSize;
			operandSizePrefix = 0;
			addressSizePrefix = 0;
			mandatoryPrefix = 0;
			opCodeBits = 0;

			switch (instruction.AddressSize) {
			case 0:
				break;

			case 16:
				if (bitness == 32)
					addressSizePrefix = 0x67;
				else
					Assert.True(bitness != 64);
				break;

			case 32:
				if (bitness != 32)
					addressSizePrefix = 0x67;
				break;

			case 64:
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}

			if (encoding == FuzzerEncodingKind.Legacy || encoding == FuzzerEncodingKind.D3NOW) {
				switch (instruction.OperandSize) {
				case 0:
					break;

				case 16:
					if (bitness != 16)
						operandSizePrefix = 0x66;
					break;

				case 32:
					if (bitness == 16)
						operandSizePrefix = 0x66;
					break;

				case 64:
					if (!instruction.DefaultOperandSize64) {
						w = 1;
						SetUsedBits(UsedBits.w);
					}
					break;

				default:
					throw ThrowHelpers.Unreachable;
				}

				switch (instruction.MandatoryPrefix) {
				case MandatoryPrefix.None:
				case MandatoryPrefix.PNP:
					break;
				case MandatoryPrefix.P66:
					mandatoryPrefix = 0x66;
					break;
				case MandatoryPrefix.PF3:
					mandatoryPrefix = 0xF3;
					break;
				case MandatoryPrefix.PF2:
					mandatoryPrefix = 0xF2;
					break;
				default:
					throw ThrowHelpers.Unreachable;
				}
			}

			if (instruction.IsNop)
				SetUsedBits(UsedBits.b);
		}

		static uint GetPrefixBits(MandatoryPrefix prefix) =>
			prefix switch {
				MandatoryPrefix.None => 0,
				MandatoryPrefix.PNP => 0,
				MandatoryPrefix.P66 => 1,
				MandatoryPrefix.PF3 => 2,
				MandatoryPrefix.PF2 => 3,
				_ => throw ThrowHelpers.Unreachable,
			};

		public void SetImmediate(uint size, ulong value) {
			Assert.True(size > 0);
			if (imm0Size == 0) {
				Assert.True(size <= 8);
				imm0Size = (byte)size;
				imm0 = (uint)value;
				imm0Hi = (uint)(value >> 32);
			}
			else if (imm1Size == 0) {
				Assert.True(size <= 4);
				imm1Size = (byte)size;
				imm1 = (uint)value;
			}
			else
				throw ThrowHelpers.Unreachable;
		}

		public void SetRegister(FuzzerOperandRegLocation regLocation, uint regNum) {
			switch (regLocation) {
			case FuzzerOperandRegLocation.ModrmRegBits:
				SetModrmRegBitsRegister(regNum);
				break;

			case FuzzerOperandRegLocation.ModrmRmBits:
				SetModrmRmBitsRegister(regNum);
				break;

			case FuzzerOperandRegLocation.VvvvBits:
				SetVvvvBitsRegister(regNum);
				break;

			case FuzzerOperandRegLocation.OpCodeBits:
				SetOpCodeBitsRegister(regNum);
				break;

			case FuzzerOperandRegLocation.Is4Bits:
				SetIs4Is5BitsRegister(regNum);
				break;

			case FuzzerOperandRegLocation.Is5Bits:
				SetIs4Is5BitsRegister(regNum);
				break;

			case FuzzerOperandRegLocation.AaaBits:
				SetAaaBitsRegister(regNum);
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		public void SetModrmMemory(uint memModrm, UsedBits ignoreBits = UsedBits.None) {
			Assert.True(memModrm < 0xC0 && (memModrm & 0x38) == 0);
			Flags |= EncodedInfoFlags.HasModrm;
			modrm |= memModrm;
			SetUsedBits((UsedBits.modrm_mod | UsedBits.modrm_rm | UsedBits.b) & ~ignoreBits);
		}

		public void SetModrmSibMemory(uint memModrm, uint sib, UsedBits ignoreBits = UsedBits.None) {
			Assert.True(memModrm < 0xC0 && (memModrm & 0x38) == 0 && (memModrm & 7) == 4 && sib <= 0xFF);
			Assert.True((memModrm & 7) == 4);
			Assert.True((Flags & EncodedInfoFlags.HasSib) == 0);
			Flags |= EncodedInfoFlags.HasModrm | EncodedInfoFlags.HasSib;
			modrm |= memModrm;
			this.sib = sib;

			var usedBits = UsedBits.modrm_mod | UsedBits.modrm_rm | UsedBits.x | UsedBits.b;
			switch (Encoding) {
			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
			case FuzzerEncodingKind.VEX2:
			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
				break;
			case FuzzerEncodingKind.EVEX:
				if (Instruction.IsVsib)
					usedBits |= UsedBits.v2;
				break;
			default:
				throw ThrowHelpers.Unreachable;
			}
			SetUsedBits(usedBits & ~ignoreBits);
		}

		public void SetModrmModBits(uint mod) {
			Assert.True(mod <= 3);
			Flags |= EncodedInfoFlags.HasModrm;
			SetUsedBits(UsedBits.modrm_mod);
			modrm |= mod << 6;
		}

		public void SetModrmRmBits(uint rm) {
			Assert.True(rm <= 7);
			Flags |= EncodedInfoFlags.HasModrm;
			SetUsedBits(UsedBits.modrm_rm);
			modrm |= rm;
		}

		public void SetModrmRegBits(uint reg) {
			Assert.True(reg <= 7);
			Flags |= EncodedInfoFlags.HasModrm;
			SetUsedBits(UsedBits.modrm_reg);
			modrm |= reg << 3;
		}

		public void SetModrmRegRmBits(uint regRm) {
			Assert.True(regRm <= 0x3F);
			Flags |= EncodedInfoFlags.HasModrm;
			SetUsedBits(UsedBits.modrm_reg | UsedBits.modrm_rm);
			modrm |= regRm;
		}

		void SetModrmRegBitsRegister(uint regNum) {
			Flags |= EncodedInfoFlags.HasModrm;
			SetUsedBits(UsedBits.modrm_reg);
			modrm |= (regNum & 7) << 3;
			switch (Encoding) {
			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
			case FuzzerEncodingKind.VEX2:
			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
				Assert.True(regNum <= 15);
				r = regNum >> 3;
				break;

			case FuzzerEncodingKind.EVEX:
				Assert.True(regNum <= 31);
				r = (regNum >> 3) & 1;
				// Don't write to r2 in 16/32-bit mode so we can test ignored R' bit
				if (Bitness == 64 || regNum >= 16)
					r2 = regNum >> 4;
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		void SetModrmRmBitsRegister(uint regNum) {
			Flags |= EncodedInfoFlags.HasModrm;
			SetUsedBits(UsedBits.modrm_mod | UsedBits.modrm_rm);
			modrm |= 0xC0 | (regNum & 7);
			switch (Encoding) {
			case FuzzerEncodingKind.VEX2:
				Assert.True(regNum <= 7);
				break;

			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
				Assert.True(regNum <= 15);
				b = regNum >> 3;
				break;

			case FuzzerEncodingKind.EVEX:
				Assert.True(regNum <= 31);
				b = (regNum >> 3) & 1;
				x = regNum >> 4;
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		void SetVvvvBitsRegister(uint regNum) {
			switch (Encoding) {
			case FuzzerEncodingKind.VEX2:
			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.XOP:
				Assert.True(regNum <= 15);
				vvvv = regNum;
				break;

			case FuzzerEncodingKind.EVEX:
				Assert.True(regNum <= 31);
				vvvv = regNum & 0xF;
				v2 = regNum >> 4;
				break;

			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		void SetOpCodeBitsRegister(uint regNum) {
			switch (Encoding) {
			case FuzzerEncodingKind.Legacy:
				Assert.True(regNum <= 15);
				Assert.True((Flags & EncodedInfoFlags.HasOpCodeBits) == 0);
				Flags |= EncodedInfoFlags.HasOpCodeBits;
				opCodeBits = (byte)(regNum & 7);
				b = regNum >> 3;
				break;

			case FuzzerEncodingKind.VEX2:
			case FuzzerEncodingKind.VEX3:
			case FuzzerEncodingKind.EVEX:
			case FuzzerEncodingKind.XOP:
			case FuzzerEncodingKind.D3NOW:
			default:
				throw ThrowHelpers.Unreachable;
			}
		}

		void SetIs4Is5BitsRegister(uint regNum) {
			Assert.True(regNum <= 0xFF);
			SetImmediate(1, (regNum << 4) | (regNum >> 4));
		}

		void SetAaaBitsRegister(uint regNum) {
			Assert.True(regNum <= 7);
			aaa = regNum;
		}

		public void InitializeXOP0to7() {
			if (Encoding != FuzzerEncodingKind.XOP)
				return;
			if (mmmmm >= 8)
				return;
			// XOP.B must be 0 (inverted value: 1)
			if ((usedBits & UsedBits.b) == 0)
				b = 0;
			else
				bf_b = 0;
		}

		public void SetUnusedBits() {
			switch (Encoding) {
			case FuzzerEncodingKind.Legacy:
			case FuzzerEncodingKind.D3NOW:
				if (Bitness == 64) {
					if ((usedBits & UsedBits.w) == 0 && !Instruction.DontUsePrefixREXW) {
						Assert.True(w == 0);
						SetUsedBits(UsedBits.w);
						w = 1;
					}
					if ((usedBits & UsedBits.r) == 0)
						r = 1;
					if ((usedBits & UsedBits.x) == 0)
						x = 1;
					if ((usedBits & UsedBits.b) == 0)
						b = 1;
				}
				break;

			case FuzzerEncodingKind.VEX2:
				if (Bitness == 64) {
					if ((usedBits & UsedBits.r) == 0)
						r = 1;
				}
				break;

			case FuzzerEncodingKind.VEX3:
				if (Bitness == 64) {
					if ((usedBits & UsedBits.r) == 0)
						r = 1;
					if ((usedBits & UsedBits.x) == 0)
						x = 1;
				}
				if ((usedBits & UsedBits.b) == 0)
					b = 1;
				break;

			case FuzzerEncodingKind.XOP:
				if ((usedBits & UsedBits.r) == 0)
					r = 1;
				if ((usedBits & UsedBits.x) == 0)
					x = 1;
				if ((usedBits & UsedBits.b) == 0)
					b = 1;
				if (Bitness != 64) {
					Assert.True((usedBits & (UsedBits.r | UsedBits.x)) == (UsedBits.r | UsedBits.x));
					bf_r = 1;
					bf_x = 1;
				}
				break;

			case FuzzerEncodingKind.EVEX:
				if (Bitness == 64) {
					if ((usedBits & UsedBits.r) == 0)
						r = 1;
					if ((usedBits & UsedBits.x) == 0)
						x = 1;
				}
				if ((usedBits & UsedBits.b) == 0)
					b = 1;
				if ((usedBits & UsedBits.r2) == 0)
					r2 = 1;
				break;

			default:
				throw ThrowHelpers.Unreachable;
			}
		}
	}
}
