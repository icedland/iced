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

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class DecoderTest_3_0F3A10_0F3A17 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpextrbV_RegMem_Reg_Ib_1_Data))]
		void Test16_VpextrbV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpextrbV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A14 48 01 A5", 7, Code.Pextrb_r32m8_xmm_imm8, Register.XMM1, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "C4E379 14 50 01 A5", 7, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };
				yield return new object[] { "C4E3F9 14 50 01 A5", 7, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "62 F37D08 14 50 01 A5", 8, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };
				yield return new object[] { "62 F3FD08 14 50 01 A5", 8, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpextrbV_RegMem_Reg_Ib_2_Data))]
		void Test16_VpextrbV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpextrbV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A14 CD A5", 6, Code.Pextrb_r32m8_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 14 D3 A5", 6, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 14 D3 A5", 7, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpextrbV_RegMem_Reg_Ib_1_Data))]
		void Test32_VpextrbV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpextrbV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A14 48 01 A5", 7, Code.Pextrb_r32m8_xmm_imm8, Register.XMM1, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "C4E379 14 50 01 A5", 7, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };
				yield return new object[] { "C4E3F9 14 50 01 A5", 7, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "62 F37D08 14 50 01 A5", 8, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };
				yield return new object[] { "62 F3FD08 14 50 01 A5", 8, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpextrbV_RegMem_Reg_Ib_2_Data))]
		void Test32_VpextrbV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpextrbV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A14 CD A5", 6, Code.Pextrb_r32m8_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 14 D3 A5", 6, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 14 D3 A5", 7, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpextrbV_RegMem_Reg_Ib_1_Data))]
		void Test64_VpextrbV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpextrbV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A14 48 01 A5", 7, Code.Pextrb_r32m8_xmm_imm8, Register.XMM1, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "66 48 0F3A14 48 01 A5", 8, Code.Pextrb_r64m8_xmm_imm8, Register.XMM1, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "C4E379 14 50 01 A5", 7, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "C4E3F9 14 50 01 A5", 7, Code.VEX_Vpextrb_r64m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "62 F37D08 14 50 01 A5", 8, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };

				yield return new object[] { "62 F3FD08 14 50 01 A5", 8, Code.EVEX_Vpextrb_r64m8_xmm_imm8, Register.XMM2, MemorySize.UInt8, 1, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpextrbV_RegMem_Reg_Ib_2_Data))]
		void Test64_VpextrbV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpextrbV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A14 CD A5", 6, Code.Pextrb_r32m8_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 44 0F3A14 CD 5A", 7, Code.Pextrb_r32m8_xmm_imm8, Register.EBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 41 0F3A14 CD A5", 7, Code.Pextrb_r32m8_xmm_imm8, Register.R13D, Register.XMM1, 0xA5 };
				yield return new object[] { "66 45 0F3A14 CD 5A", 7, Code.Pextrb_r32m8_xmm_imm8, Register.R13D, Register.XMM9, 0x5A };

				yield return new object[] { "66 48 0F3A14 CD A5", 7, Code.Pextrb_r64m8_xmm_imm8, Register.RBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4C 0F3A14 CD 5A", 7, Code.Pextrb_r64m8_xmm_imm8, Register.RBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 49 0F3A14 CD A5", 7, Code.Pextrb_r64m8_xmm_imm8, Register.R13, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4D 0F3A14 CD 5A", 7, Code.Pextrb_r64m8_xmm_imm8, Register.R13, Register.XMM9, 0x5A };

				yield return new object[] { "C4E379 14 D3 A5", 6, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C46379 14 D3 A5", 6, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C379 14 D3 A5", 6, Code.VEX_Vpextrb_r32m8_xmm_imm8, Register.R11D, Register.XMM2, 0xA5 };

				yield return new object[] { "C4E3F9 14 D3 A5", 6, Code.VEX_Vpextrb_r64m8_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C463F9 14 D3 A5", 6, Code.VEX_Vpextrb_r64m8_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C3F9 14 D3 A5", 6, Code.VEX_Vpextrb_r64m8_xmm_imm8, Register.R11, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 14 D3 A5", 7, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 737D08 14 D3 A5", 7, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C37D08 14 D3 A5", 7, Code.EVEX_Vpextrb_r32m8_xmm_imm8, Register.R11D, Register.XMM18, 0xA5 };

				yield return new object[] { "62 F3FD08 14 D3 A5", 7, Code.EVEX_Vpextrb_r64m8_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 73FD08 14 D3 A5", 7, Code.EVEX_Vpextrb_r64m8_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C3FD08 14 D3 A5", 7, Code.EVEX_Vpextrb_r64m8_xmm_imm8, Register.R11, Register.XMM18, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpextrwV_RegMem_Reg_Ib_1_Data))]
		void Test16_VpextrwV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpextrwV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A15 48 01 A5", 7, Code.Pextrw_r32m16_xmm_imm8, Register.XMM1, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "C4E379 15 50 01 A5", 7, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 1, 0xA5 };
				yield return new object[] { "C4E3F9 15 50 01 A5", 7, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "62 F37D08 15 50 01 A5", 8, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 2, 0xA5 };
				yield return new object[] { "62 F3FD08 15 50 01 A5", 8, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpextrwV_RegMem_Reg_Ib_2_Data))]
		void Test16_VpextrwV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpextrwV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A15 CD A5", 6, Code.Pextrw_r32m16_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 15 D3 A5", 6, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 15 D3 A5", 7, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpextrwV_RegMem_Reg_Ib_1_Data))]
		void Test32_VpextrwV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpextrwV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A15 48 01 A5", 7, Code.Pextrw_r32m16_xmm_imm8, Register.XMM1, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "C4E379 15 50 01 A5", 7, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 1, 0xA5 };
				yield return new object[] { "C4E3F9 15 50 01 A5", 7, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "62 F37D08 15 50 01 A5", 8, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 2, 0xA5 };
				yield return new object[] { "62 F3FD08 15 50 01 A5", 8, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpextrwV_RegMem_Reg_Ib_2_Data))]
		void Test32_VpextrwV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpextrwV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A15 CD A5", 6, Code.Pextrw_r32m16_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 15 D3 A5", 6, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 15 D3 A5", 7, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpextrwV_RegMem_Reg_Ib_1_Data))]
		void Test64_VpextrwV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpextrwV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A15 48 01 A5", 7, Code.Pextrw_r32m16_xmm_imm8, Register.XMM1, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "66 48 0F3A15 48 01 A5", 8, Code.Pextrw_r64m16_xmm_imm8, Register.XMM1, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "C4E379 15 50 01 A5", 7, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "C4E3F9 15 50 01 A5", 7, Code.VEX_Vpextrw_r64m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 1, 0xA5 };

				yield return new object[] { "62 F37D08 15 50 01 A5", 8, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 2, 0xA5 };

				yield return new object[] { "62 F3FD08 15 50 01 A5", 8, Code.EVEX_Vpextrw_r64m16_xmm_imm8, Register.XMM2, MemorySize.UInt16, 2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpextrwV_RegMem_Reg_Ib_2_Data))]
		void Test64_VpextrwV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpextrwV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A15 CD A5", 6, Code.Pextrw_r32m16_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 44 0F3A15 CD 5A", 7, Code.Pextrw_r32m16_xmm_imm8, Register.EBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 41 0F3A15 CD A5", 7, Code.Pextrw_r32m16_xmm_imm8, Register.R13D, Register.XMM1, 0xA5 };
				yield return new object[] { "66 45 0F3A15 CD 5A", 7, Code.Pextrw_r32m16_xmm_imm8, Register.R13D, Register.XMM9, 0x5A };

				yield return new object[] { "66 48 0F3A15 CD A5", 7, Code.Pextrw_r64m16_xmm_imm8, Register.RBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4C 0F3A15 CD 5A", 7, Code.Pextrw_r64m16_xmm_imm8, Register.RBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 49 0F3A15 CD A5", 7, Code.Pextrw_r64m16_xmm_imm8, Register.R13, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4D 0F3A15 CD 5A", 7, Code.Pextrw_r64m16_xmm_imm8, Register.R13, Register.XMM9, 0x5A };

				yield return new object[] { "C4E379 15 D3 A5", 6, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C46379 15 D3 A5", 6, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C379 15 D3 A5", 6, Code.VEX_Vpextrw_r32m16_xmm_imm8, Register.R11D, Register.XMM2, 0xA5 };

				yield return new object[] { "C4E3F9 15 D3 A5", 6, Code.VEX_Vpextrw_r64m16_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C463F9 15 D3 A5", 6, Code.VEX_Vpextrw_r64m16_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C3F9 15 D3 A5", 6, Code.VEX_Vpextrw_r64m16_xmm_imm8, Register.R11, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 15 D3 A5", 7, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 737D08 15 D3 A5", 7, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C37D08 15 D3 A5", 7, Code.EVEX_Vpextrw_r32m16_xmm_imm8, Register.R11D, Register.XMM18, 0xA5 };

				yield return new object[] { "62 F3FD08 15 D3 A5", 7, Code.EVEX_Vpextrw_r64m16_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 73FD08 15 D3 A5", 7, Code.EVEX_Vpextrw_r64m16_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C3FD08 15 D3 A5", 7, Code.EVEX_Vpextrw_r64m16_xmm_imm8, Register.R11, Register.XMM18, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpextrdV_RegMem_Reg_Ib_1_Data))]
		void Test16_VpextrdV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpextrdV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A16 48 01 A5", 7, Code.Pextrd_rm32_xmm_imm8, Register.XMM1, MemorySize.UInt32, 1, 0xA5 };

				yield return new object[] { "C4E379 16 50 01 A5", 7, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 1, 0xA5 };
				yield return new object[] { "C4E3F9 16 50 01 A5", 7, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 1, 0xA5 };

				yield return new object[] { "62 F37D08 16 50 01 A5", 8, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 4, 0xA5 };
				yield return new object[] { "62 F3FD08 16 50 01 A5", 8, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 4, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpextrdV_RegMem_Reg_Ib_2_Data))]
		void Test16_VpextrdV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpextrdV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A16 CD A5", 6, Code.Pextrd_rm32_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 16 D3 A5", 6, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 16 D3 A5", 7, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpextrdV_RegMem_Reg_Ib_1_Data))]
		void Test32_VpextrdV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpextrdV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A16 48 01 A5", 7, Code.Pextrd_rm32_xmm_imm8, Register.XMM1, MemorySize.UInt32, 1, 0xA5 };

				yield return new object[] { "C4E379 16 50 01 A5", 7, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 1, 0xA5 };
				yield return new object[] { "C4E3F9 16 50 01 A5", 7, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 1, 0xA5 };

				yield return new object[] { "62 F37D08 16 50 01 A5", 8, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 4, 0xA5 };
				yield return new object[] { "62 F3FD08 16 50 01 A5", 8, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 4, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpextrdV_RegMem_Reg_Ib_2_Data))]
		void Test32_VpextrdV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpextrdV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A16 CD A5", 6, Code.Pextrd_rm32_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 16 D3 A5", 6, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 16 D3 A5", 7, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpextrdV_RegMem_Reg_Ib_1_Data))]
		void Test64_VpextrdV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpextrdV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A16 48 01 A5", 7, Code.Pextrd_rm32_xmm_imm8, Register.XMM1, MemorySize.UInt32, 1, 0xA5 };

				yield return new object[] { "66 48 0F3A16 48 01 A5", 8, Code.Pextrq_rm64_xmm_imm8, Register.XMM1, MemorySize.UInt64, 1, 0xA5 };

				yield return new object[] { "C4E379 16 50 01 A5", 7, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 1, 0xA5 };

				yield return new object[] { "C4E3F9 16 50 01 A5", 7, Code.VEX_Vpextrq_rm64_xmm_imm8, Register.XMM2, MemorySize.UInt64, 1, 0xA5 };

				yield return new object[] { "62 F37D08 16 50 01 A5", 8, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.XMM2, MemorySize.UInt32, 4, 0xA5 };

				yield return new object[] { "62 F3FD08 16 50 01 A5", 8, Code.EVEX_Vpextrq_rm64_xmm_imm8, Register.XMM2, MemorySize.UInt64, 8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpextrdV_RegMem_Reg_Ib_2_Data))]
		void Test64_VpextrdV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpextrdV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A16 CD A5", 6, Code.Pextrd_rm32_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 44 0F3A16 CD 5A", 7, Code.Pextrd_rm32_xmm_imm8, Register.EBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 41 0F3A16 CD A5", 7, Code.Pextrd_rm32_xmm_imm8, Register.R13D, Register.XMM1, 0xA5 };
				yield return new object[] { "66 45 0F3A16 CD 5A", 7, Code.Pextrd_rm32_xmm_imm8, Register.R13D, Register.XMM9, 0x5A };

				yield return new object[] { "66 48 0F3A16 CD A5", 7, Code.Pextrq_rm64_xmm_imm8, Register.RBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4C 0F3A16 CD 5A", 7, Code.Pextrq_rm64_xmm_imm8, Register.RBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 49 0F3A16 CD A5", 7, Code.Pextrq_rm64_xmm_imm8, Register.R13, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4D 0F3A16 CD 5A", 7, Code.Pextrq_rm64_xmm_imm8, Register.R13, Register.XMM9, 0x5A };

				yield return new object[] { "C4E379 16 D3 A5", 6, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C46379 16 D3 A5", 6, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C379 16 D3 A5", 6, Code.VEX_Vpextrd_rm32_xmm_imm8, Register.R11D, Register.XMM2, 0xA5 };

				yield return new object[] { "C4E3F9 16 D3 A5", 6, Code.VEX_Vpextrq_rm64_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C463F9 16 D3 A5", 6, Code.VEX_Vpextrq_rm64_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C3F9 16 D3 A5", 6, Code.VEX_Vpextrq_rm64_xmm_imm8, Register.R11, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 16 D3 A5", 7, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 737D08 16 D3 A5", 7, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C37D08 16 D3 A5", 7, Code.EVEX_Vpextrd_rm32_xmm_imm8, Register.R11D, Register.XMM18, 0xA5 };

				yield return new object[] { "62 F3FD08 16 D3 A5", 7, Code.EVEX_Vpextrq_rm64_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 73FD08 16 D3 A5", 7, Code.EVEX_Vpextrq_rm64_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C3FD08 16 D3 A5", 7, Code.EVEX_Vpextrq_rm64_xmm_imm8, Register.R11, Register.XMM18, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VextractV_RegMem_Reg_Ib_1_Data))]
		void Test16_VextractV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VextractV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A17 48 01 A5", 7, Code.Extractps_rm32_xmm_imm8, Register.XMM1, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "C4E379 17 50 01 A5", 7, Code.VEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 1, 0xA5 };
				yield return new object[] { "C4E3F9 17 50 01 A5", 7, Code.VEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "62 F37D08 17 50 01 A5", 8, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F3FD08 17 50 01 A5", 8, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 4, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VextractV_RegMem_Reg_Ib_2_Data))]
		void Test16_VextractV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VextractV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A17 CD A5", 6, Code.Extractps_rm32_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 17 D3 A5", 6, Code.VEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 17 D3 A5", 7, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VextractV_RegMem_Reg_Ib_1_Data))]
		void Test32_VextractV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VextractV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A17 48 01 A5", 7, Code.Extractps_rm32_xmm_imm8, Register.XMM1, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "C4E379 17 50 01 A5", 7, Code.VEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 1, 0xA5 };
				yield return new object[] { "C4E3F9 17 50 01 A5", 7, Code.VEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "62 F37D08 17 50 01 A5", 8, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 4, 0xA5 };
				yield return new object[] { "62 F3FD08 17 50 01 A5", 8, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 4, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VextractV_RegMem_Reg_Ib_2_Data))]
		void Test32_VextractV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VextractV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A17 CD A5", 6, Code.Extractps_rm32_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E379 17 D3 A5", 6, Code.VEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 17 D3 A5", 7, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VextractV_RegMem_Reg_Ib_1_Data))]
		void Test64_VextractV_RegMem_Reg_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VextractV_RegMem_Reg_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A17 48 01 A5", 7, Code.Extractps_rm32_xmm_imm8, Register.XMM1, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "66 48 0F3A17 48 01 A5", 8, Code.Extractps_rm64_xmm_imm8, Register.XMM1, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "C4E379 17 50 01 A5", 7, Code.VEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "C4E3F9 17 50 01 A5", 7, Code.VEX_Vextractps_rm64_xmm_imm8, Register.XMM2, MemorySize.Float32, 1, 0xA5 };

				yield return new object[] { "62 F37D08 17 50 01 A5", 8, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.XMM2, MemorySize.Float32, 4, 0xA5 };

				yield return new object[] { "62 F3FD08 17 50 01 A5", 8, Code.EVEX_Vextractps_rm64_xmm_imm8, Register.XMM2, MemorySize.Float32, 4, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VextractV_RegMem_Reg_Ib_2_Data))]
		void Test64_VextractV_RegMem_Reg_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VextractV_RegMem_Reg_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A17 CD A5", 6, Code.Extractps_rm32_xmm_imm8, Register.EBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 44 0F3A17 CD 5A", 7, Code.Extractps_rm32_xmm_imm8, Register.EBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 41 0F3A17 CD A5", 7, Code.Extractps_rm32_xmm_imm8, Register.R13D, Register.XMM1, 0xA5 };
				yield return new object[] { "66 45 0F3A17 CD 5A", 7, Code.Extractps_rm32_xmm_imm8, Register.R13D, Register.XMM9, 0x5A };

				yield return new object[] { "66 48 0F3A17 CD A5", 7, Code.Extractps_rm64_xmm_imm8, Register.RBP, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4C 0F3A17 CD 5A", 7, Code.Extractps_rm64_xmm_imm8, Register.RBP, Register.XMM9, 0x5A };
				yield return new object[] { "66 49 0F3A17 CD A5", 7, Code.Extractps_rm64_xmm_imm8, Register.R13, Register.XMM1, 0xA5 };
				yield return new object[] { "66 4D 0F3A17 CD 5A", 7, Code.Extractps_rm64_xmm_imm8, Register.R13, Register.XMM9, 0x5A };

				yield return new object[] { "C4E379 17 D3 A5", 6, Code.VEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C46379 17 D3 A5", 6, Code.VEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C379 17 D3 A5", 6, Code.VEX_Vextractps_rm32_xmm_imm8, Register.R11D, Register.XMM2, 0xA5 };

				yield return new object[] { "C4E3F9 17 D3 A5", 6, Code.VEX_Vextractps_rm64_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "C463F9 17 D3 A5", 6, Code.VEX_Vextractps_rm64_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "C4C3F9 17 D3 A5", 6, Code.VEX_Vextractps_rm64_xmm_imm8, Register.R11, Register.XMM2, 0xA5 };

				yield return new object[] { "62 F37D08 17 D3 A5", 7, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 737D08 17 D3 A5", 7, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.EBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C37D08 17 D3 A5", 7, Code.EVEX_Vextractps_rm32_xmm_imm8, Register.R11D, Register.XMM18, 0xA5 };

				yield return new object[] { "62 F3FD08 17 D3 A5", 7, Code.EVEX_Vextractps_rm64_xmm_imm8, Register.RBX, Register.XMM2, 0xA5 };
				yield return new object[] { "62 73FD08 17 D3 A5", 7, Code.EVEX_Vextractps_rm64_xmm_imm8, Register.RBX, Register.XMM10, 0xA5 };
				yield return new object[] { "62 C3FD08 17 D3 A5", 7, Code.EVEX_Vextractps_rm64_xmm_imm8, Register.R11, Register.XMM18, 0xA5 };
			}
		}
	}
}
