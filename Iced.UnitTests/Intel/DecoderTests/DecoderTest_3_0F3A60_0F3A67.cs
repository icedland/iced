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
	public sealed class DecoderTest_3_0F3A60_0F3A67 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpcmpestrmV_VX_WX_Ib_1_Data))]
		void Test16_VpcmpestrmV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpcmpestrmV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A60 08 A5", 6, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 60 10 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 60 10 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpestrmV_VX_WX_Ib_2_Data))]
		void Test16_VpcmpestrmV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpcmpestrmV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A60 CD A5", 6, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 60 D3 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpestrmV_VX_WX_Ib_1_Data))]
		void Test32_VpcmpestrmV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpcmpestrmV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A60 08 A5", 6, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 60 10 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 60 10 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpestrmV_VX_WX_Ib_2_Data))]
		void Test32_VpcmpestrmV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpcmpestrmV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A60 CD A5", 6, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 60 D3 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpestrmV_VX_WX_Ib_1_Data))]
		void Test64_VpcmpestrmV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpcmpestrmV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A60 08 A5", 6, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "66 48 0F3A60 08 A5", 7, Code.Pcmpestrm64_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 60 10 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 60 10 A5", 6, Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpestrmV_VX_WX_Ib_2_Data))]
		void Test64_VpcmpestrmV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpcmpestrmV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A60 CD A5", 6, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A60 CD A5", 7, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A60 CD A5", 7, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A60 CD A5", 7, Code.Pcmpestrm_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "66 48 0F3A60 CD A5", 7, Code.Pcmpestrm64_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 4C 0F3A60 CD A5", 7, Code.Pcmpestrm64_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 49 0F3A60 CD A5", 7, Code.Pcmpestrm64_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 4D 0F3A60 CD A5", 7, Code.Pcmpestrm64_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C4E379 60 D3 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 60 D3 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 60 D3 A5", 6, Code.VEX_Vpcmpestrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E3F9 60 D3 A5", 6, Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C463F9 60 D3 A5", 6, Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C3F9 60 D3 A5", 6, Code.VEX_Vpcmpestrm64_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpestriV_VX_WX_Ib_1_Data))]
		void Test16_VpcmpestriV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpcmpestriV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A61 08 A5", 6, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 61 10 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 61 10 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpestriV_VX_WX_Ib_2_Data))]
		void Test16_VpcmpestriV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpcmpestriV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A61 CD A5", 6, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 61 D3 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpestriV_VX_WX_Ib_1_Data))]
		void Test32_VpcmpestriV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpcmpestriV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A61 08 A5", 6, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 61 10 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 61 10 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpestriV_VX_WX_Ib_2_Data))]
		void Test32_VpcmpestriV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpcmpestriV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A61 CD A5", 6, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 61 D3 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpestriV_VX_WX_Ib_1_Data))]
		void Test64_VpcmpestriV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpcmpestriV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A61 08 A5", 6, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "66 48 0F3A61 08 A5", 7, Code.Pcmpestri64_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 61 10 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 61 10 A5", 6, Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpestriV_VX_WX_Ib_2_Data))]
		void Test64_VpcmpestriV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpcmpestriV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A61 CD A5", 6, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A61 CD A5", 7, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A61 CD A5", 7, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A61 CD A5", 7, Code.Pcmpestri_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "66 48 0F3A61 CD A5", 7, Code.Pcmpestri64_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 4C 0F3A61 CD A5", 7, Code.Pcmpestri64_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 49 0F3A61 CD A5", 7, Code.Pcmpestri64_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 4D 0F3A61 CD A5", 7, Code.Pcmpestri64_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C4E379 61 D3 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 61 D3 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 61 D3 A5", 6, Code.VEX_Vpcmpestri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E3F9 61 D3 A5", 6, Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C463F9 61 D3 A5", 6, Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C3F9 61 D3 A5", 6, Code.VEX_Vpcmpestri64_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpistrmV_VX_WX_Ib_1_Data))]
		void Test16_VpcmpistrmV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpcmpistrmV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A62 08 A5", 6, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 62 10 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 62 10 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpistrmV_VX_WX_Ib_2_Data))]
		void Test16_VpcmpistrmV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpcmpistrmV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A62 CD A5", 6, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 62 D3 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpistrmV_VX_WX_Ib_1_Data))]
		void Test32_VpcmpistrmV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpcmpistrmV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A62 08 A5", 6, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 62 10 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 62 10 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpistrmV_VX_WX_Ib_2_Data))]
		void Test32_VpcmpistrmV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpcmpistrmV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A62 CD A5", 6, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 62 D3 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpistrmV_VX_WX_Ib_1_Data))]
		void Test64_VpcmpistrmV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpcmpistrmV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A62 08 A5", 6, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 62 10 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 62 10 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpistrmV_VX_WX_Ib_2_Data))]
		void Test64_VpcmpistrmV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpcmpistrmV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A62 CD A5", 6, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A62 CD A5", 7, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A62 CD A5", 7, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A62 CD A5", 7, Code.Pcmpistrm_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C4E379 62 D3 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 62 D3 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 62 D3 A5", 6, Code.VEX_Vpcmpistrm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpistriV_VX_WX_Ib_1_Data))]
		void Test16_VpcmpistriV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpcmpistriV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A63 08 A5", 6, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 63 10 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 63 10 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpistriV_VX_WX_Ib_2_Data))]
		void Test16_VpcmpistriV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VpcmpistriV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A63 CD A5", 6, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 63 D3 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpistriV_VX_WX_Ib_1_Data))]
		void Test32_VpcmpistriV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpcmpistriV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A63 08 A5", 6, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 63 10 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 63 10 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpistriV_VX_WX_Ib_2_Data))]
		void Test32_VpcmpistriV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VpcmpistriV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A63 CD A5", 6, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C4E379 63 D3 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpistriV_VX_WX_Ib_1_Data))]
		void Test64_VpcmpistriV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpcmpistriV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A63 08 A5", 6, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };

				yield return new object[] { "C4E379 63 10 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3F9 63 10 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpistriV_VX_WX_Ib_2_Data))]
		void Test64_VpcmpistriV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VpcmpistriV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A63 CD A5", 6, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A63 CD A5", 7, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A63 CD A5", 7, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A63 CD A5", 7, Code.Pcmpistri_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C4E379 63 D3 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C46379 63 D3 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C379 63 D3 A5", 6, Code.VEX_Vpcmpistri_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfpclasspsV_K_k1_WX_Ib_1_Data))]
		void Test16_VfpclasspsV_K_k1_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VfpclasspsV_K_k1_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D0B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D1D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast128_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D2B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37D3D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast256_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D28 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F37D4B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37D5D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast512_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D48 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD1D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast128_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FD3D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FD5D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfpclasspsV_K_k1_WX_Ib_2_Data))]
		void Test16_VfpclasspsV_K_k1_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VfpclasspsV_K_k1_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D0B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F37D2B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F37D4B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.ZMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfpclasspsV_K_k1_WX_Ib_1_Data))]
		void Test32_VfpclasspsV_K_k1_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VfpclasspsV_K_k1_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D0B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D1D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast128_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D2B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37D3D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast256_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D28 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F37D4B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37D5D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast512_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D48 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD1D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast128_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FD3D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FD5D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfpclasspsV_K_k1_WX_Ib_2_Data))]
		void Test32_VfpclasspsV_K_k1_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VfpclasspsV_K_k1_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D0B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F37D2B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F37D4B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.ZMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfpclasspsV_K_k1_WX_Ib_1_Data))]
		void Test64_VfpclasspsV_K_k1_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VfpclasspsV_K_k1_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D0B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37D1D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast128_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D2B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37D3D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast256_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D28 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F37D4B 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.K3, MemorySize.Packed512_Float32, 64, false, 0xA5 };
				yield return new object[] { "62 F37D5D 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.K5, MemorySize.Broadcast512_Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D48 66 50 01 A5", 8, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.None, MemorySize.Packed512_Float32, 64, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FD1D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast128_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FD3D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast256_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD28 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.K3, MemorySize.Packed512_Float64, 64, false, 0xA5 };
				yield return new object[] { "62 F3FD5D 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.K5, MemorySize.Broadcast512_Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD48 66 50 01 A5", 8, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.None, MemorySize.Packed512_Float64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfpclasspsV_K_k1_WX_Ib_2_Data))]
		void Test64_VfpclasspsV_K_k1_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VfpclasspsV_K_k1_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D0B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 937D0B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B37D0B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_xmmm128b32_imm8, Register.K2, Register.XMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F37D2B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 937D2B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B37D2B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_ymmm256b32_imm8, Register.K2, Register.YMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F37D4B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 937D4B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B37D4B 66 D3 A5", 7, Code.EVEX_Vfpclassps_k_k1_zmmm512b32_imm8, Register.K2, Register.ZMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93FD0B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3FD0B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_xmmm128b64_imm8, Register.K2, Register.XMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93FD2B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3FD2B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_ymmm256b64_imm8, Register.K2, Register.YMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93FD4B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3FD4B 66 D3 A5", 7, Code.EVEX_Vfpclasspd_k_k1_zmmm512b64_imm8, Register.K2, Register.ZMM19, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfpclassssV_K_k1_WX_Ib_1_Data))]
		void Test16_VfpclassssV_K_k1_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VfpclassssV_K_k1_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D0B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D2B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D4B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D6B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD2B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD4B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD6B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VfpclassssV_K_k1_WX_Ib_2_Data))]
		void Test16_VfpclassssV_K_k1_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VfpclassssV_K_k1_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D0B 67 D3 A5", 7, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 67 D3 A5", 7, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfpclassssV_K_k1_WX_Ib_1_Data))]
		void Test32_VfpclassssV_K_k1_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VfpclassssV_K_k1_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D0B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D2B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D4B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D6B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD2B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD4B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD6B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VfpclassssV_K_k1_WX_Ib_2_Data))]
		void Test32_VfpclassssV_K_k1_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VfpclassssV_K_k1_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D0B 67 D3 A5", 7, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 67 D3 A5", 7, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfpclassssV_K_k1_WX_Ib_1_Data))]
		void Test64_VfpclassssV_K_k1_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VfpclassssV_K_k1_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D0B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D08 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.None, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D2B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D4B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };
				yield return new object[] { "62 F37D6B 67 50 01 A5", 8, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.K3, MemorySize.Float32, 4, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD08 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.None, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD2B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD4B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
				yield return new object[] { "62 F3FD6B 67 50 01 A5", 8, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.K3, MemorySize.Float64, 8, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VfpclassssV_K_k1_WX_Ib_2_Data))]
		void Test64_VfpclassssV_K_k1_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VfpclassssV_K_k1_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D0B 67 D3 A5", 7, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 937D0B 67 D3 A5", 7, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B37D0B 67 D3 A5", 7, Code.EVEX_Vfpclassss_k_k1_xmmm32_imm8, Register.K2, Register.XMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3FD0B 67 D3 A5", 7, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93FD0B 67 D3 A5", 7, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3FD0B 67 D3 A5", 7, Code.EVEX_Vfpclasssd_k_k1_xmmm64_imm8, Register.K2, Register.XMM19, Register.K3, false, 0xA5 };
			}
		}
	}
}
