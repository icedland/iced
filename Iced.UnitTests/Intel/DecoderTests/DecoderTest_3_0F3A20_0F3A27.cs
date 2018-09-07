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
	public sealed class DecoderTest_3_0F3A20_0F3A27 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PinsrbV_VX_Gd_Mb_Ib_1_Data))]
		void Test16_PinsrbV_VX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_PinsrbV_VX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A20 08 A5", 6, Code.Pinsrb_VX_RdMb_Ib, Register.XMM1, MemorySize.UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PinsrbV_VX_Gd_Mb_Ib_2_Data))]
		void Test16_PinsrbV_VX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PinsrbV_VX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A20 CD A5", 6, Code.Pinsrb_VX_RdMb_Ib, Register.XMM1, Register.EBP, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrbV_VX_Gd_Mb_Ib_1_Data))]
		void Test32_PinsrbV_VX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_PinsrbV_VX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A20 08 A5", 6, Code.Pinsrb_VX_RdMb_Ib, Register.XMM1, MemorySize.UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrbV_VX_Gd_Mb_Ib_2_Data))]
		void Test32_PinsrbV_VX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PinsrbV_VX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A20 CD A5", 6, Code.Pinsrb_VX_RdMb_Ib, Register.XMM1, Register.EBP, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrbV_VX_Gd_Mb_Ib_1_Data))]
		void Test64_PinsrbV_VX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_PinsrbV_VX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A20 08 A5", 6, Code.Pinsrb_VX_RdMb_Ib, Register.XMM1, MemorySize.UInt8, 0xA5 };

				yield return new object[] { "66 48 0F3A20 08 A5", 7, Code.Pinsrb_VX_RqMb_Ib, Register.XMM1, MemorySize.UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrbV_VX_Gd_Mb_Ib_2_Data))]
		void Test64_PinsrbV_VX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PinsrbV_VX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A20 CD A5", 6, Code.Pinsrb_VX_RdMb_Ib, Register.XMM1, Register.EBP, 0xA5 };
				yield return new object[] { "66 44 0F3A20 CD A5", 7, Code.Pinsrb_VX_RdMb_Ib, Register.XMM9, Register.EBP, 0xA5 };
				yield return new object[] { "66 41 0F3A20 CD A5", 7, Code.Pinsrb_VX_RdMb_Ib, Register.XMM1, Register.R13D, 0xA5 };
				yield return new object[] { "66 45 0F3A20 CD A5", 7, Code.Pinsrb_VX_RdMb_Ib, Register.XMM9, Register.R13D, 0xA5 };

				yield return new object[] { "66 48 0F3A20 CD A5", 7, Code.Pinsrb_VX_RqMb_Ib, Register.XMM1, Register.RBP, 0xA5 };
				yield return new object[] { "66 4C 0F3A20 CD A5", 7, Code.Pinsrb_VX_RqMb_Ib, Register.XMM9, Register.RBP, 0xA5 };
				yield return new object[] { "66 49 0F3A20 CD A5", 7, Code.Pinsrb_VX_RqMb_Ib, Register.XMM1, Register.R13, 0xA5 };
				yield return new object[] { "66 4D 0F3A20 CD A5", 7, Code.Pinsrb_VX_RqMb_Ib, Register.XMM9, Register.R13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrbV_VX_HX_Gd_Mb_Ib_1_Data))]
		void Test16_VpinsrbV_VX_HX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpinsrbV_VX_HX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 20 10 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt8, 0xA5 };
				yield return new object[] { "C4E3C9 20 10 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrbV_VX_HX_Gd_Mb_Ib_2_Data))]
		void Test16_VpinsrbV_VX_HX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpinsrbV_VX_HX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrbV_VX_HX_Gd_Mb_Ib_1_Data))]
		void Test32_VpinsrbV_VX_HX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpinsrbV_VX_HX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 20 10 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt8, 0xA5 };
				yield return new object[] { "C4E3C9 20 10 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrbV_VX_HX_Gd_Mb_Ib_2_Data))]
		void Test32_VpinsrbV_VX_HX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpinsrbV_VX_HX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrbV_VX_HX_Gd_Mb_Ib_1_Data))]
		void Test64_VpinsrbV_VX_HX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpinsrbV_VX_HX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 20 10 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt8, 0xA5 };

				yield return new object[] { "C4E3C9 20 10 A5", 6, Code.VEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrbV_VX_HX_Gd_Mb_Ib_2_Data))]
		void Test64_VpinsrbV_VX_HX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpinsrbV_VX_HX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
				yield return new object[] { "C46349 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM10, Register.XMM6, Register.EBX, 0xA5 };
				yield return new object[] { "C4E309 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM14, Register.EBX, 0xA5 };
				yield return new object[] { "C4C349 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.R11D, 0xA5 };

				yield return new object[] { "C4E3C9 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM2, Register.XMM6, Register.RBX, 0xA5 };
				yield return new object[] { "C463C9 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM10, Register.XMM6, Register.RBX, 0xA5 };
				yield return new object[] { "C4E389 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM2, Register.XMM14, Register.RBX, 0xA5 };
				yield return new object[] { "C4C3C9 20 D3 A5", 6, Code.VEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM2, Register.XMM6, Register.R11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrbEV_VX_HX_Gd_Mb_Ib_1_Data))]
		void Test16_VpinsrbEV_VX_HX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpinsrbEV_VX_HX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 20 50 01 A5", 8, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt8, 1, false, 0xA5 };
				yield return new object[] { "62 F3CD08 20 50 01 A5", 8, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt8, 1, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrbEV_VX_HX_Gd_Mb_Ib_2_Data))]
		void Test16_VpinsrbEV_VX_HX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpinsrbEV_VX_HX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrbEV_VX_HX_Gd_Mb_Ib_1_Data))]
		void Test32_VpinsrbEV_VX_HX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpinsrbEV_VX_HX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 20 50 01 A5", 8, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt8, 1, false, 0xA5 };
				yield return new object[] { "62 F3CD08 20 50 01 A5", 8, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt8, 1, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrbEV_VX_HX_Gd_Mb_Ib_2_Data))]
		void Test32_VpinsrbEV_VX_HX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpinsrbEV_VX_HX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrbEV_VX_HX_Gd_Mb_Ib_1_Data))]
		void Test64_VpinsrbEV_VX_HX_Gd_Mb_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpinsrbEV_VX_HX_Gd_Mb_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 20 50 01 A5", 8, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt8, 1, false, 0xA5 };

				yield return new object[] { "62 F3CD08 20 50 01 A5", 8, Code.EVEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt8, 1, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrbEV_VX_HX_Gd_Mb_Ib_2_Data))]
		void Test64_VpinsrbEV_VX_HX_Gd_Mb_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpinsrbEV_VX_HX_Gd_Mb_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM18, Register.XMM14, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 534D00 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM10, Register.XMM22, Register.R11D, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D34D08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RdMb_Ib, Register.XMM2, Register.XMM6, Register.R11D, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38D08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM18, Register.XMM14, Register.RBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 53CD00 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM10, Register.XMM22, Register.R11, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D3CD08 20 D3 A5", 7, Code.EVEX_Vpinsrb_VX_HX_RqMb_Ib, Register.XMM2, Register.XMM6, Register.R11, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_InsertV_VX_WX_Ib_1_Data))]
		void Test16_InsertV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_InsertV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A21 08 A5", 6, Code.Insertps_VX_WX_Ib, Register.XMM1, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_InsertV_VX_WX_Ib_2_Data))]
		void Test16_InsertV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_InsertV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A21 CD A5", 6, Code.Insertps_VX_WX_Ib, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_InsertV_VX_WX_Ib_1_Data))]
		void Test32_InsertV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_InsertV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A21 08 A5", 6, Code.Insertps_VX_WX_Ib, Register.XMM1, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_InsertV_VX_WX_Ib_2_Data))]
		void Test32_InsertV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_InsertV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A21 CD A5", 6, Code.Insertps_VX_WX_Ib, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_InsertV_VX_WX_Ib_1_Data))]
		void Test64_InsertV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_InsertV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A21 08 A5", 6, Code.Insertps_VX_WX_Ib, Register.XMM1, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_InsertV_VX_WX_Ib_2_Data))]
		void Test64_InsertV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_InsertV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A21 CD A5", 6, Code.Insertps_VX_WX_Ib, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A21 CD 5A", 7, Code.Insertps_VX_WX_Ib, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3A21 CD A5", 7, Code.Insertps_VX_WX_Ib, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A21 CD 5A", 7, Code.Insertps_VX_WX_Ib, Register.XMM9, Register.XMM13, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VinsertV_VX_HX_WX_Ib_1_Data))]
		void Test16_VinsertV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VinsertV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 21 10 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E3C9 21 10 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VinsertV_VX_HX_WX_Ib_2_Data))]
		void Test16_VinsertV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VinsertV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 21 D3 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VinsertV_VX_HX_WX_Ib_1_Data))]
		void Test32_VinsertV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VinsertV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 21 10 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E3C9 21 10 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VinsertV_VX_HX_WX_Ib_2_Data))]
		void Test32_VinsertV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VinsertV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 21 D3 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VinsertV_VX_HX_WX_Ib_1_Data))]
		void Test64_VinsertV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VinsertV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 21 10 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
				yield return new object[] { "C4E3C9 21 10 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VinsertV_VX_HX_WX_Ib_2_Data))]
		void Test64_VinsertV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VinsertV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 21 D3 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C46349 21 D3 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E309 21 D3 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C349 21 D3 A5", 6, Code.VEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_EVEX_VinsertV_VX_HX_WX_Ib_1_Data))]
		void Test16_EVEX_VinsertV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_EVEX_VinsertV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 21 50 01 A5", 8, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_EVEX_VinsertV_VX_HX_WX_Ib_2_Data))]
		void Test16_EVEX_VinsertV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_EVEX_VinsertV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 21 D3 A5", 7, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_EVEX_VinsertV_VX_HX_WX_Ib_1_Data))]
		void Test32_EVEX_VinsertV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_EVEX_VinsertV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 21 50 01 A5", 8, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_EVEX_VinsertV_VX_HX_WX_Ib_2_Data))]
		void Test32_EVEX_VinsertV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_EVEX_VinsertV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 21 D3 A5", 7, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_EVEX_VinsertV_VX_HX_WX_Ib_1_Data))]
		void Test64_EVEX_VinsertV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_EVEX_VinsertV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 21 50 01 A5", 8, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, MemorySize.Float32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_EVEX_VinsertV_VX_HX_WX_Ib_2_Data))]
		void Test64_EVEX_VinsertV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_EVEX_VinsertV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 21 D3 A5", 7, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM3, false, 0xA5 };
				yield return new object[] { "62 E30D08 21 D3 A5", 7, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM18, Register.XMM14, Register.XMM3, false, 0xA5 };
				yield return new object[] { "62 134D00 21 D3 A5", 7, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM10, Register.XMM22, Register.XMM27, false, 0xA5 };
				yield return new object[] { "62 B34D08 21 D3 A5", 7, Code.EVEX_Vinsertps_VX_HX_WX_Ib, Register.XMM2, Register.XMM6, Register.XMM19, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PinsrdV_VX_Ed_Ib_1_Data))]
		void Test16_PinsrdV_VX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_PinsrdV_VX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A22 08 A5", 6, Code.Pinsrd_VX_Ed_Ib, Register.XMM1, MemorySize.UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PinsrdV_VX_Ed_Ib_2_Data))]
		void Test16_PinsrdV_VX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PinsrdV_VX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A22 CD A5", 6, Code.Pinsrd_VX_Ed_Ib, Register.XMM1, Register.EBP, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrdV_VX_Ed_Ib_1_Data))]
		void Test32_PinsrdV_VX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_PinsrdV_VX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A22 08 A5", 6, Code.Pinsrd_VX_Ed_Ib, Register.XMM1, MemorySize.UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PinsrdV_VX_Ed_Ib_2_Data))]
		void Test32_PinsrdV_VX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PinsrdV_VX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A22 CD A5", 6, Code.Pinsrd_VX_Ed_Ib, Register.XMM1, Register.EBP, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrdV_VX_Ed_Ib_1_Data))]
		void Test64_PinsrdV_VX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_PinsrdV_VX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A22 08 A5", 6, Code.Pinsrd_VX_Ed_Ib, Register.XMM1, MemorySize.UInt32, 0xA5 };

				yield return new object[] { "66 48 0F3A22 08 A5", 7, Code.Pinsrq_VX_Eq_Ib, Register.XMM1, MemorySize.UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PinsrdV_VX_Ed_Ib_2_Data))]
		void Test64_PinsrdV_VX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PinsrdV_VX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A22 CD A5", 6, Code.Pinsrd_VX_Ed_Ib, Register.XMM1, Register.EBP, 0xA5 };
				yield return new object[] { "66 44 0F3A22 CD A5", 7, Code.Pinsrd_VX_Ed_Ib, Register.XMM9, Register.EBP, 0xA5 };
				yield return new object[] { "66 41 0F3A22 CD A5", 7, Code.Pinsrd_VX_Ed_Ib, Register.XMM1, Register.R13D, 0xA5 };
				yield return new object[] { "66 45 0F3A22 CD A5", 7, Code.Pinsrd_VX_Ed_Ib, Register.XMM9, Register.R13D, 0xA5 };

				yield return new object[] { "66 48 0F3A22 CD A5", 7, Code.Pinsrq_VX_Eq_Ib, Register.XMM1, Register.RBP, 0xA5 };
				yield return new object[] { "66 4C 0F3A22 CD A5", 7, Code.Pinsrq_VX_Eq_Ib, Register.XMM9, Register.RBP, 0xA5 };
				yield return new object[] { "66 49 0F3A22 CD A5", 7, Code.Pinsrq_VX_Eq_Ib, Register.XMM1, Register.R13, 0xA5 };
				yield return new object[] { "66 4D 0F3A22 CD A5", 7, Code.Pinsrq_VX_Eq_Ib, Register.XMM9, Register.R13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrdV_VX_HX_Ed_Ib_1_Data))]
		void Test16_VpinsrdV_VX_HX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpinsrdV_VX_HX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 22 10 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt32, 0xA5 };
				yield return new object[] { "C4E3C9 22 10 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrdV_VX_HX_Ed_Ib_2_Data))]
		void Test16_VpinsrdV_VX_HX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpinsrdV_VX_HX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 22 D3 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrdV_VX_HX_Ed_Ib_1_Data))]
		void Test32_VpinsrdV_VX_HX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpinsrdV_VX_HX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 22 10 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt32, 0xA5 };
				yield return new object[] { "C4E3C9 22 10 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrdV_VX_HX_Ed_Ib_2_Data))]
		void Test32_VpinsrdV_VX_HX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpinsrdV_VX_HX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 22 D3 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrdV_VX_HX_Ed_Ib_1_Data))]
		void Test64_VpinsrdV_VX_HX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpinsrdV_VX_HX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 22 10 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt32, 0xA5 };

				yield return new object[] { "C4E3C9 22 10 A5", 6, Code.VEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM2, Register.XMM6, MemorySize.UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrdV_VX_HX_Ed_Ib_2_Data))]
		void Test64_VpinsrdV_VX_HX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpinsrdV_VX_HX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 22 D3 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.EBX, 0xA5 };
				yield return new object[] { "C46349 22 D3 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM10, Register.XMM6, Register.EBX, 0xA5 };
				yield return new object[] { "C4E309 22 D3 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM14, Register.EBX, 0xA5 };
				yield return new object[] { "C4C349 22 D3 A5", 6, Code.VEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.R11D, 0xA5 };

				yield return new object[] { "C4E3C9 22 D3 A5", 6, Code.VEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM2, Register.XMM6, Register.RBX, 0xA5 };
				yield return new object[] { "C463C9 22 D3 A5", 6, Code.VEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM10, Register.XMM6, Register.RBX, 0xA5 };
				yield return new object[] { "C4E389 22 D3 A5", 6, Code.VEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM2, Register.XMM14, Register.RBX, 0xA5 };
				yield return new object[] { "C4C3C9 22 D3 A5", 6, Code.VEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM2, Register.XMM6, Register.R11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrdEV_VX_HX_Ed_Ib_1_Data))]
		void Test16_VpinsrdEV_VX_HX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpinsrdEV_VX_HX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 22 50 01 A5", 8, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F3CD08 22 50 01 A5", 8, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpinsrdEV_VX_HX_Ed_Ib_2_Data))]
		void Test16_VpinsrdEV_VX_HX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpinsrdEV_VX_HX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 22 D3 A5", 7, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrdEV_VX_HX_Ed_Ib_1_Data))]
		void Test32_VpinsrdEV_VX_HX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpinsrdEV_VX_HX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 22 50 01 A5", 8, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F3CD08 22 50 01 A5", 8, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpinsrdEV_VX_HX_Ed_Ib_2_Data))]
		void Test32_VpinsrdEV_VX_HX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpinsrdEV_VX_HX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 22 D3 A5", 7, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrdEV_VX_HX_Ed_Ib_1_Data))]
		void Test64_VpinsrdEV_VX_HX_Ed_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpinsrdEV_VX_HX_Ed_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 22 50 01 A5", 8, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt32, 4, false, 0xA5 };

				yield return new object[] { "62 F3CD08 22 50 01 A5", 8, Code.EVEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM2, Register.XMM6, Register.None, MemorySize.UInt64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpinsrdEV_VX_HX_Ed_Ib_2_Data))]
		void Test64_VpinsrdEV_VX_HX_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpinsrdEV_VX_HX_Ed_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 22 D3 A5", 7, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D08 22 D3 A5", 7, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM18, Register.XMM14, Register.EBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 534D00 22 D3 A5", 7, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM10, Register.XMM22, Register.R11D, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D34D08 22 D3 A5", 7, Code.EVEX_Vpinsrd_VX_HX_Ed_Ib, Register.XMM2, Register.XMM6, Register.R11D, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD08 22 D3 A5", 7, Code.EVEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38D08 22 D3 A5", 7, Code.EVEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM18, Register.XMM14, Register.RBX, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 53CD00 22 D3 A5", 7, Code.EVEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM10, Register.XMM22, Register.R11, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D3CD08 22 D3 A5", 7, Code.EVEX_Vpinsrq_VX_HX_Eq_Ib, Register.XMM2, Register.XMM6, Register.R11, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vshuff32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vshuff32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vshuff32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vshuff32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vshuff32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vshuff32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vshuff32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vshuff32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vshuff32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vshuff32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vshuff32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vshuff32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vshuff32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vshuff32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vshuff32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 23 50 01 A5", 8, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 23 50 01 A5", 8, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vshuff32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vshuff32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vshuff32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D2B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D23 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D4B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D43 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 23 D3 A5", 7, Code.EVEX_Vshuff32x4_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 23 D3 A5", 7, Code.EVEX_Vshuff64x2_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpternlogdV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VpternlogdV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpternlogdV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpternlogdV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VpternlogdV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpternlogdV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpternlogdV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VpternlogdV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpternlogdV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpternlogdV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VpternlogdV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpternlogdV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpternlogdV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VpternlogdV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpternlogdV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 25 50 01 A5", 8, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 25 50 01 A5", 8, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpternlogdV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VpternlogdV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpternlogdV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D0B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D03 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D0B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D2B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D23 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 25 D3 A5", 7, Code.EVEX_Vpternlogd_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D4B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D43 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 25 D3 A5", 7, Code.EVEX_Vpternlogd_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D0B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD03 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VX_k1z_HX_WX_Ib_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VY_k1z_HY_WY_Ib_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 25 D3 A5", 7, Code.EVEX_Vpternlogq_VZ_k1z_HZ_WZ_Ib_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test16_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3FD08 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test16_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F37D1B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D3B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3FD0B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F3FD1B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD3B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test32_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3FD08 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test32_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F37D1B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D3B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3FD0B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 F3FD1B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD3B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1_Data))]
		void Test64_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VgetmantpsV_VX_k1z_WX_Ib_EVEX_1_Data {
			get {
				yield return new object[] { "62 F37D08 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D9B 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D28 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DBB 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F37D48 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37DDB 26 50 01 A5", 8, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true, 0xA5 };

				yield return new object[] { "62 F3FD08 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD28 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDBB 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true, 0xA5 };

				yield return new object[] { "62 F3FD48 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FDDB 26 50 01 A5", 8, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2_Data))]
		void Test64_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VgetmantpsV_VX_k1z_WX_Ib_EVEX_2_Data {
			get {
				yield return new object[] { "62 F37D0B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D8B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 337D0B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C37D8B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VX_k1z_WX_Ib_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D2B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 337D2B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C37DAB 26 D3 A5", 7, Code.EVEX_Vgetmantps_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 337D1B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 C37D3B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D5B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F37D7B 26 D3 A5", 7, Code.EVEX_Vgetmantps_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F3FD0B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD8B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 33FD0B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C3FD8B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VX_k1z_WX_Ib_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 33FD2B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 C3FDAB 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VY_k1z_WY_Ib_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FD9B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true, 0xA5 };
				yield return new object[] { "62 33FD1B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 C3FD3B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD5B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3FD7B 26 D3 A5", 7, Code.EVEX_Vgetmantpd_VZ_k1z_WZ_Ib_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgetmantss_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vgetmantss_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vgetmantss_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vgetmantss_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vgetmantss_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vgetmantss_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F34D1B 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F34D08 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD8B 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3CD08 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgetmantss_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vgetmantss_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vgetmantss_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vgetmantss_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vgetmantss_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vgetmantss_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F34D1B 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F34D08 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD8B 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F3CD1B 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 F3CD08 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgetmantss_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vgetmantss_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vgetmantss_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D8D 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 27 50 01 A5", 8, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Float64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 27 50 01 A5", 8, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vgetmantss_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vgetmantss_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vgetmantss_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D8B 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E30D1B 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 134D03 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D08 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D28 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D48 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D68 27 D3 A5", 7, Code.EVEX_Vgetmantss_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD8B 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E38D1B 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, true, 0xA5 };
				yield return new object[] { "62 13CD03 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD08 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD28 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD48 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD68 27 D3 A5", 7, Code.EVEX_Vgetmantsd_VX_k1z_HX_WX_Ib_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}
	}
}
