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
	public sealed class DecoderTest_2_0F70_0F77 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PshufV_VX_WX_1_Data))]
		void Test16_PshufV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PshufV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F70 08 A5", 4, Code.Pshufw_mm_mmm64_imm8, Register.MM1, MemorySize.Packed64_Int16, 0xA5 };

				yield return new object[] { "66 0F70 08 A5", 5, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int32, 0xA5 };

				yield return new object[] { "C5F9 70 10 A5", 5, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "C5FD 70 10 A5", 5, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int32, 0xA5 };
				yield return new object[] { "C4E1F9 70 10 A5", 6, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "C4E1FD 70 10 A5", 6, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int32, 0xA5 };

				yield return new object[] { "F3 0F70 08 A5", 5, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };

				yield return new object[] { "C5FA 70 10 A5", 5, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C5FE 70 10 A5", 5, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
				yield return new object[] { "C4E1FA 70 10 A5", 6, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E1FE 70 10 A5", 6, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };

				yield return new object[] { "F2 0F70 08 A5", 5, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };

				yield return new object[] { "C5FB 70 10 A5", 5, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C5FF 70 10 A5", 5, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
				yield return new object[] { "C4E1FB 70 10 A5", 6, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E1FF 70 10 A5", 6, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PshufV_VX_WX_2_Data))]
		void Test16_PshufV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_PshufV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F70 CD A5", 4, Code.Pshufw_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F70 CD A5", 5, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C5F9 70 D3 A5", 5, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FD 70 D3 A5", 5, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };

				yield return new object[] { "F3 0F70 CD A5", 5, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C5FA 70 D3 A5", 5, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FE 70 D3 A5", 5, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };

				yield return new object[] { "F2 0F70 CD A5", 5, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C5FB 70 D3 A5", 5, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FF 70 D3 A5", 5, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PshufV_VX_WX_1_Data))]
		void Test32_PshufV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PshufV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F70 08 A5", 4, Code.Pshufw_mm_mmm64_imm8, Register.MM1, MemorySize.Packed64_Int16, 0xA5 };

				yield return new object[] { "66 0F70 08 A5", 5, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int32, 0xA5 };

				yield return new object[] { "C5F9 70 10 A5", 5, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "C5FD 70 10 A5", 5, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int32, 0xA5 };
				yield return new object[] { "C4E1F9 70 10 A5", 6, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "C4E1FD 70 10 A5", 6, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int32, 0xA5 };

				yield return new object[] { "F3 0F70 08 A5", 5, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };

				yield return new object[] { "C5FA 70 10 A5", 5, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C5FE 70 10 A5", 5, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
				yield return new object[] { "C4E1FA 70 10 A5", 6, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E1FE 70 10 A5", 6, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };

				yield return new object[] { "F2 0F70 08 A5", 5, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };

				yield return new object[] { "C5FB 70 10 A5", 5, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C5FF 70 10 A5", 5, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
				yield return new object[] { "C4E1FB 70 10 A5", 6, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E1FF 70 10 A5", 6, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PshufV_VX_WX_2_Data))]
		void Test32_PshufV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_PshufV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F70 CD A5", 4, Code.Pshufw_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F70 CD A5", 5, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C5F9 70 D3 A5", 5, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FD 70 D3 A5", 5, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };

				yield return new object[] { "F3 0F70 CD A5", 5, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C5FA 70 D3 A5", 5, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FE 70 D3 A5", 5, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };

				yield return new object[] { "F2 0F70 CD A5", 5, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "C5FB 70 D3 A5", 5, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FF 70 D3 A5", 5, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PshufV_VX_WX_1_Data))]
		void Test64_PshufV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PshufV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F70 08 A5", 4, Code.Pshufw_mm_mmm64_imm8, Register.MM1, MemorySize.Packed64_Int16, 0xA5 };

				yield return new object[] { "66 0F70 08 A5", 5, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int32, 0xA5 };

				yield return new object[] { "C5F9 70 10 A5", 5, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "C5FD 70 10 A5", 5, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int32, 0xA5 };
				yield return new object[] { "C4E1F9 70 10 A5", 6, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "C4E1FD 70 10 A5", 6, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int32, 0xA5 };

				yield return new object[] { "F3 0F70 08 A5", 5, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };

				yield return new object[] { "C5FA 70 10 A5", 5, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C5FE 70 10 A5", 5, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
				yield return new object[] { "C4E1FA 70 10 A5", 6, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E1FE 70 10 A5", 6, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };

				yield return new object[] { "F2 0F70 08 A5", 5, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Int16, 0xA5 };

				yield return new object[] { "C5FB 70 10 A5", 5, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C5FF 70 10 A5", 5, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
				yield return new object[] { "C4E1FB 70 10 A5", 6, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "C4E1FF 70 10 A5", 6, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, MemorySize.Packed256_Int16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PshufV_VX_WX_2_Data))]
		void Test64_PshufV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_PshufV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F70 CD A5", 4, Code.Pshufw_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F70 CD A5", 5, Code.Pshufw_mm_mmm64_imm8, Register.MM1, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F70 CD A5", 5, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F70 CD A5", 6, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F70 CD A5", 6, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F70 CD A5", 6, Code.Pshufd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C5F9 70 D3 A5", 5, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FD 70 D3 A5", 5, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C579 70 D3 A5", 5, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C57D 70 D3 A5", 5, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C179 70 D3 A5", 6, Code.VEX_Vpshufd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C17D 70 D3 A5", 6, Code.VEX_Vpshufd_ymm_ymmm256_imm8, Register.YMM2, Register.YMM11, 0xA5 };

				yield return new object[] { "F3 0F70 CD A5", 5, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "F3 44 0F70 CD A5", 6, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "F3 41 0F70 CD A5", 6, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "F3 45 0F70 CD A5", 6, Code.Pshufhw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C5FA 70 D3 A5", 5, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FE 70 D3 A5", 5, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C57A 70 D3 A5", 5, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C57E 70 D3 A5", 5, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C17A 70 D3 A5", 6, Code.VEX_Vpshufhw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C17E 70 D3 A5", 6, Code.VEX_Vpshufhw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM11, 0xA5 };

				yield return new object[] { "F2 0F70 CD A5", 5, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "F2 44 0F70 CD A5", 6, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "F2 41 0F70 CD A5", 6, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "F2 45 0F70 CD A5", 6, Code.Pshuflw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };

				yield return new object[] { "C5FB 70 D3 A5", 5, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "C5FF 70 D3 A5", 5, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM3, 0xA5 };
				yield return new object[] { "C57B 70 D3 A5", 5, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0xA5 };
				yield return new object[] { "C57F 70 D3 A5", 5, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM10, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C17B 70 D3 A5", 6, Code.VEX_Vpshuflw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C17F 70 D3 A5", 6, Code.VEX_Vpshuflw_ymm_ymmm256_imm8, Register.YMM2, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufV_VX_k1_HX_WX_1_Data))]
		void Test16_VpshufV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshufV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F17D8B 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F17D1D 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D08 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F17DAB 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F17D3D 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D28 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F17DCB 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true, 0xA5 };
				yield return new object[] { "62 F17D5D 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D48 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F17E8B 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F17E08 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1FE8B 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };

				yield return new object[] { "62 F17EAB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F17E28 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1FEAB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };

				yield return new object[] { "62 F17ECB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F17E48 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1FECB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };

				yield return new object[] { "62 F17F8B 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F17F08 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1FF8B 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };

				yield return new object[] { "62 F17FAB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F17F28 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1FFAB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };

				yield return new object[] { "62 F17FCB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F17F48 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1FFCB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufV_VX_k1_HX_WX_2_Data))]
		void Test16_VpshufV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpshufV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F17D8B 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17D08 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17DAB 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17D28 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17DCB 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17D48 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17E8B 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17E08 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17EAB 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17E28 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17ECB 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17E48 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17F8B 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17F08 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17FAB 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17F28 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17FCB 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17F48 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufV_VX_k1_HX_WX_1_Data))]
		void Test32_VpshufV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshufV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F17D8B 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F17D1D 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D08 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F17DAB 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F17D3D 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D28 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F17DCB 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true, 0xA5 };
				yield return new object[] { "62 F17D5D 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D48 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F17E8B 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F17E08 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1FE8B 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };

				yield return new object[] { "62 F17EAB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F17E28 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1FEAB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };

				yield return new object[] { "62 F17ECB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F17E48 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1FECB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };

				yield return new object[] { "62 F17F8B 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F17F08 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1FF8B 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };

				yield return new object[] { "62 F17FAB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F17F28 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1FFAB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };

				yield return new object[] { "62 F17FCB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F17F48 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1FFCB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufV_VX_k1_HX_WX_2_Data))]
		void Test32_VpshufV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpshufV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F17D8B 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17D08 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17DAB 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17D28 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17DCB 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17D48 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17E8B 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17E08 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17EAB 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17E28 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17ECB 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17E48 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17F8B 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17F08 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17FAB 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17F28 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17FCB 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 F17F48 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufV_VX_k1_HX_WX_1_Data))]
		void Test64_VpshufV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshufV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F17D8B 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true, 0xA5 };
				yield return new object[] { "62 F17D1D 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.K5, MemorySize.Broadcast128_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D08 70 50 01 A5", 8, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F17DAB 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true, 0xA5 };
				yield return new object[] { "62 F17D3D 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.K5, MemorySize.Broadcast256_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D28 70 50 01 A5", 8, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F17DCB 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true, 0xA5 };
				yield return new object[] { "62 F17D5D 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F17D48 70 50 01 A5", 8, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F17E8B 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F17E08 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1FE8B 70 50 01 A5", 8, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };

				yield return new object[] { "62 F17EAB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F17E28 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1FEAB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };

				yield return new object[] { "62 F17ECB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F17E48 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1FECB 70 50 01 A5", 8, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };

				yield return new object[] { "62 F17F8B 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F17F08 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1FF8B 70 50 01 A5", 8, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true, 0xA5 };

				yield return new object[] { "62 F17FAB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F17F28 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1FFAB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true, 0xA5 };

				yield return new object[] { "62 F17FCB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F17F48 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1FFCB 70 50 01 A5", 8, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufV_VX_k1_HX_WX_2_Data))]
		void Test64_VpshufV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpshufV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F17D8B 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17D0B 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117D0B 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17D08 70 D3 A5", 7, Code.EVEX_Vpshufd_xmm_k1z_xmmm128b32_imm8, Register.XMM2, Register.XMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17DAB 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17D2B 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117D2B 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17D28 70 D3 A5", 7, Code.EVEX_Vpshufd_ymm_k1z_ymmm256b32_imm8, Register.YMM2, Register.YMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17DCB 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17D4B 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117D4B 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17D48 70 D3 A5", 7, Code.EVEX_Vpshufd_zmm_k1z_zmmm512b32_imm8, Register.ZMM2, Register.ZMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17E8B 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17E0B 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117E0B 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17E08 70 D3 A5", 7, Code.EVEX_Vpshufhw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17EAB 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17E2B 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117E2B 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17E28 70 D3 A5", 7, Code.EVEX_Vpshufhw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17ECB 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17E4B 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117E4B 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17E48 70 D3 A5", 7, Code.EVEX_Vpshufhw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17F8B 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17F0B 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117F0B 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17F08 70 D3 A5", 7, Code.EVEX_Vpshuflw_xmm_k1z_xmmm128_imm8, Register.XMM2, Register.XMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17FAB 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17F2B 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM18, Register.YMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117F2B 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM10, Register.YMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17F28 70 D3 A5", 7, Code.EVEX_Vpshuflw_ymm_k1z_ymmm256_imm8, Register.YMM2, Register.YMM19, Register.None, RoundingControl.None, false, 0xA5 };

				yield return new object[] { "62 F17FCB 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, 0xA5 };
				yield return new object[] { "62 E17F4B 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM18, Register.ZMM3, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 117F4B 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM10, Register.ZMM27, Register.K3, RoundingControl.None, false, 0xA5 };
				yield return new object[] { "62 B17F48 70 D3 A5", 7, Code.EVEX_Vpshuflw_zmm_k1z_zmmm512_imm8, Register.ZMM2, Register.ZMM19, Register.None, RoundingControl.None, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift71V_1_Data))]
		void Test16_Pshift71V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Pshift71V_1_Data {
			get {
				yield return new object[] { "0F71 D5 A5", 4, Code.Psrlw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 D5 A5", 5, Code.Psrlw_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F71 E5 A5", 4, Code.Psraw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 E5 A5", 5, Code.Psraw_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F71 F5 A5", 4, Code.Psllw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 F5 A5", 5, Code.Psllw_xmm_imm8, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift71V_1_Data))]
		void Test32_Pshift71V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Pshift71V_1_Data {
			get {
				yield return new object[] { "0F71 D5 A5", 4, Code.Psrlw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 D5 A5", 5, Code.Psrlw_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F71 E5 A5", 4, Code.Psraw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 E5 A5", 5, Code.Psraw_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F71 F5 A5", 4, Code.Psllw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 F5 A5", 5, Code.Psllw_xmm_imm8, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift71V_1_Data))]
		void Test64_Pshift71V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Pshift71V_1_Data {
			get {
				yield return new object[] { "0F71 D5 A5", 4, Code.Psrlw_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F71 D5 A5", 5, Code.Psrlw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 D5 A5", 5, Code.Psrlw_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F71 D5 A5", 6, Code.Psrlw_xmm_imm8, Register.XMM13, 0xA5 };

				yield return new object[] { "0F71 E5 A5", 4, Code.Psraw_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F71 E5 A5", 5, Code.Psraw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 E5 A5", 5, Code.Psraw_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F71 E5 A5", 6, Code.Psraw_xmm_imm8, Register.XMM13, 0xA5 };

				yield return new object[] { "0F71 F5 A5", 4, Code.Psllw_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F71 F5 A5", 5, Code.Psllw_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F71 F5 A5", 5, Code.Psllw_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F71 F5 A5", 6, Code.Psllw_xmm_imm8, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift71VEX_1_Data))]
		void Test16_Pshift71VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Pshift71VEX_1_Data {
			get {
				yield return new object[] { "C5C9 71 D5 A5", 5, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 71 D5 A5", 6, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 D5 A5", 5, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 71 D5 A5", 6, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 71 E5 A5", 5, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 71 E5 A5", 6, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 E5 A5", 5, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 71 E5 A5", 6, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 71 F5 A5", 5, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 71 F5 A5", 6, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 F5 A5", 5, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 71 F5 A5", 6, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift71VEX_1_Data))]
		void Test32_Pshift71VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Pshift71VEX_1_Data {
			get {
				yield return new object[] { "C5C9 71 D5 A5", 5, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 71 D5 A5", 6, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 D5 A5", 5, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 71 D5 A5", 6, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 71 E5 A5", 5, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 71 E5 A5", 6, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 E5 A5", 5, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 71 E5 A5", 6, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 71 F5 A5", 5, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 71 F5 A5", 6, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 F5 A5", 5, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 71 F5 A5", 6, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift71VEX_1_Data))]
		void Test64_Pshift71VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Pshift71VEX_1_Data {
			get {
				yield return new object[] { "C5C9 71 D5 A5", 5, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 71 D5 A5", 5, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 71 D5 A5", 6, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 71 D5 A5", 6, Code.VEX_Vpsrlw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 D5 A5", 5, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 71 D5 A5", 5, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 71 D5 A5", 6, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 71 D5 A5", 6, Code.VEX_Vpsrlw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 71 E5 A5", 5, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 71 E5 A5", 5, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 71 E5 A5", 6, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 71 E5 A5", 6, Code.VEX_Vpsraw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 E5 A5", 5, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 71 E5 A5", 5, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 71 E5 A5", 6, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 71 E5 A5", 6, Code.VEX_Vpsraw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 71 F5 A5", 5, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 71 F5 A5", 5, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 71 F5 A5", 6, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 71 F5 A5", 6, Code.VEX_Vpsllw_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 71 F5 A5", 5, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 71 F5 A5", 5, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 71 F5 A5", 6, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 71 F5 A5", 6, Code.VEX_Vpsllw_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift71EVEX_1_Data))]
		void Test16_Pshift71EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Pshift71EVEX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift71EVEX_2_Data))]
		void Test16_Pshift71EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Pshift71EVEX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift71EVEX_1_Data))]
		void Test32_Pshift71EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Pshift71EVEX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift71EVEX_2_Data))]
		void Test32_Pshift71EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Pshift71EVEX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift71EVEX_1_Data))]
		void Test64_Pshift71EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Pshift71EVEX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 D5 A5", 7, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 D5 A5", 7, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 D5 A5", 7, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 E5 A5", 7, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 E5 A5", 7, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 E5 A5", 7, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 F5 A5", 7, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 F5 A5", 7, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 F5 A5", 7, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift71EVEX_2_Data))]
		void Test64_Pshift71EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Pshift71EVEX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 50 01 A5", 8, Code.EVEX_Vpsrlw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 60 01 A5", 8, Code.EVEX_Vpsraw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 60 01 A5", 8, Code.EVEX_Vpsraw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 60 01 A5", 8, Code.EVEX_Vpsraw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F14D8D 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F14D08 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD0B 71 70 01 A5", 8, Code.EVEX_Vpsllw_xmm_k1z_xmmm128_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F14DAD 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F14D28 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD2B 71 70 01 A5", 8, Code.EVEX_Vpsllw_ymm_k1z_ymmm256_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F14DCD 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F14D48 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD4B 71 70 01 A5", 8, Code.EVEX_Vpsllw_zmm_k1z_zmmm512_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift72V_1_Data))]
		void Test16_Pshift72V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Pshift72V_1_Data {
			get {
				yield return new object[] { "0F72 D5 A5", 4, Code.Psrld_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 D5 A5", 5, Code.Psrld_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F72 E5 A5", 4, Code.Psrad_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 E5 A5", 5, Code.Psrad_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F72 F5 A5", 4, Code.Pslld_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 F5 A5", 5, Code.Pslld_xmm_imm8, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift72V_1_Data))]
		void Test32_Pshift72V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Pshift72V_1_Data {
			get {
				yield return new object[] { "0F72 D5 A5", 4, Code.Psrld_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 D5 A5", 5, Code.Psrld_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F72 E5 A5", 4, Code.Psrad_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 E5 A5", 5, Code.Psrad_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F72 F5 A5", 4, Code.Pslld_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 F5 A5", 5, Code.Pslld_xmm_imm8, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift72V_1_Data))]
		void Test64_Pshift72V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Pshift72V_1_Data {
			get {
				yield return new object[] { "0F72 D5 A5", 4, Code.Psrld_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F72 D5 A5", 5, Code.Psrld_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 D5 A5", 5, Code.Psrld_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F72 D5 A5", 6, Code.Psrld_xmm_imm8, Register.XMM13, 0xA5 };

				yield return new object[] { "0F72 E5 A5", 4, Code.Psrad_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F72 E5 A5", 5, Code.Psrad_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 E5 A5", 5, Code.Psrad_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F72 E5 A5", 6, Code.Psrad_xmm_imm8, Register.XMM13, 0xA5 };

				yield return new object[] { "0F72 F5 A5", 4, Code.Pslld_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F72 F5 A5", 5, Code.Pslld_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F72 F5 A5", 5, Code.Pslld_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F72 F5 A5", 6, Code.Pslld_xmm_imm8, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift72VEX_1_Data))]
		void Test16_Pshift72VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Pshift72VEX_1_Data {
			get {
				yield return new object[] { "C5C9 72 D5 A5", 5, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 72 D5 A5", 6, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 D5 A5", 5, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 72 D5 A5", 6, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 72 E5 A5", 5, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 72 E5 A5", 6, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 E5 A5", 5, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 72 E5 A5", 6, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 72 F5 A5", 5, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 72 F5 A5", 6, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 F5 A5", 5, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 72 F5 A5", 6, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift72VEX_1_Data))]
		void Test32_Pshift72VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Pshift72VEX_1_Data {
			get {
				yield return new object[] { "C5C9 72 D5 A5", 5, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 72 D5 A5", 6, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 D5 A5", 5, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 72 D5 A5", 6, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 72 E5 A5", 5, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 72 E5 A5", 6, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 E5 A5", 5, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 72 E5 A5", 6, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 72 F5 A5", 5, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 72 F5 A5", 6, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 F5 A5", 5, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 72 F5 A5", 6, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift72VEX_1_Data))]
		void Test64_Pshift72VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Pshift72VEX_1_Data {
			get {
				yield return new object[] { "C5C9 72 D5 A5", 5, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 72 D5 A5", 5, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 72 D5 A5", 6, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 72 D5 A5", 6, Code.VEX_Vpsrld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 D5 A5", 5, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 72 D5 A5", 5, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 72 D5 A5", 6, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 72 D5 A5", 6, Code.VEX_Vpsrld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 72 E5 A5", 5, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 72 E5 A5", 5, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 72 E5 A5", 6, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 72 E5 A5", 6, Code.VEX_Vpsrad_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 E5 A5", 5, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 72 E5 A5", 5, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 72 E5 A5", 6, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 72 E5 A5", 6, Code.VEX_Vpsrad_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 72 F5 A5", 5, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 72 F5 A5", 5, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 72 F5 A5", 6, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 72 F5 A5", 6, Code.VEX_Vpslld_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 72 F5 A5", 5, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 72 F5 A5", 5, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 72 F5 A5", 6, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 72 F5 A5", 6, Code.VEX_Vpslld_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift72EVEX_1_Data))]
		void Test16_Pshift72EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Pshift72EVEX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift72EVEX_2_Data))]
		void Test16_Pshift72EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Pshift72EVEX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift72EVEX_1_Data))]
		void Test32_Pshift72EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Pshift72EVEX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14D8B 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D08 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DAB 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D28 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F14DCB 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F14D48 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift72EVEX_2_Data))]
		void Test32_Pshift72EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Pshift72EVEX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift72EVEX_1_Data))]
		void Test64_Pshift72EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Pshift72EVEX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 72 C5 A5", 7, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 72 C5 A5", 7, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 72 C5 A5", 7, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CD8B 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D08 72 C5 A5", 7, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDAB 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D28 72 C5 A5", 7, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDCB 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D48 72 C5 A5", 7, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 72 CD A5", 7, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 72 CD A5", 7, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 72 CD A5", 7, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CD8B 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D08 72 CD A5", 7, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDAB 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D28 72 CD A5", 7, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDCB 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D48 72 CD A5", 7, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 72 D5 A5", 7, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 72 D5 A5", 7, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 72 D5 A5", 7, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 72 E5 A5", 7, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 72 E5 A5", 7, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 72 E5 A5", 7, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CD8B 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D08 72 E5 A5", 7, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDAB 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D28 72 E5 A5", 7, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDCB 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D48 72 E5 A5", 7, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D8B 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D08 72 F5 A5", 7, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DAB 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D28 72 F5 A5", 7, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14DCB 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B10D48 72 F5 A5", 7, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift72EVEX_2_Data))]
		void Test64_Pshift72EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Pshift72EVEX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 40 01 A5", 8, Code.EVEX_Vprord_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 40 01 A5", 8, Code.EVEX_Vprord_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 40 01 A5", 8, Code.EVEX_Vprord_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 40 01 A5", 8, Code.EVEX_Vprorq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 40 01 A5", 8, Code.EVEX_Vprorq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 40 01 A5", 8, Code.EVEX_Vprorq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 48 01 A5", 8, Code.EVEX_Vprold_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 48 01 A5", 8, Code.EVEX_Vprold_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 48 01 A5", 8, Code.EVEX_Vprold_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 48 01 A5", 8, Code.EVEX_Vprolq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 48 01 A5", 8, Code.EVEX_Vprolq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 48 01 A5", 8, Code.EVEX_Vprolq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 50 01 A5", 8, Code.EVEX_Vpsrld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 50 01 A5", 8, Code.EVEX_Vpsrld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 50 01 A5", 8, Code.EVEX_Vpsrld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 60 01 A5", 8, Code.EVEX_Vpsrad_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 60 01 A5", 8, Code.EVEX_Vpsrad_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 60 01 A5", 8, Code.EVEX_Vpsrad_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 72 60 01 A5", 8, Code.EVEX_Vpsraq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 72 60 01 A5", 8, Code.EVEX_Vpsraq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 72 60 01 A5", 8, Code.EVEX_Vpsraq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D0B 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F14D9D 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D08 72 70 01 A5", 8, Code.EVEX_Vpslld_xmm_k1z_xmmm128b32_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F14D2B 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F14DBD 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D28 72 70 01 A5", 8, Code.EVEX_Vpslld_ymm_k1z_ymmm256b32_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F14D4B 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F14DDD 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F14D48 72 70 01 A5", 8, Code.EVEX_Vpslld_zmm_k1z_zmmm512b32_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift73V_1_Data))]
		void Test16_Pshift73V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Pshift73V_1_Data {
			get {
				yield return new object[] { "0F73 D5 A5", 4, Code.Psrlq_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F73 D5 A5", 5, Code.Psrlq_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F73 DD A5", 5, Code.Psrldq_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F73 F5 A5", 4, Code.Psllq_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F73 F5 A5", 5, Code.Psllq_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F73 FD A5", 5, Code.Pslldq_xmm_imm8, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift73V_1_Data))]
		void Test32_Pshift73V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Pshift73V_1_Data {
			get {
				yield return new object[] { "0F73 D5 A5", 4, Code.Psrlq_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F73 D5 A5", 5, Code.Psrlq_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F73 DD A5", 5, Code.Psrldq_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "0F73 F5 A5", 4, Code.Psllq_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F73 F5 A5", 5, Code.Psllq_xmm_imm8, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F73 FD A5", 5, Code.Pslldq_xmm_imm8, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift73V_1_Data))]
		void Test64_Pshift73V_1(string hexBytes, int byteLength, Code code, Register reg1, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Pshift73V_1_Data {
			get {
				yield return new object[] { "0F73 D5 A5", 4, Code.Psrlq_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F73 D5 A5", 5, Code.Psrlq_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F73 D5 A5", 5, Code.Psrlq_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F73 D5 A5", 6, Code.Psrlq_xmm_imm8, Register.XMM13, 0xA5 };

				yield return new object[] { "66 0F73 DD A5", 5, Code.Psrldq_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F73 DD A5", 6, Code.Psrldq_xmm_imm8, Register.XMM13, 0xA5 };

				yield return new object[] { "0F73 F5 A5", 4, Code.Psllq_mm_imm8, Register.MM5, 0xA5 };
				yield return new object[] { "4F 0F73 F5 A5", 5, Code.Psllq_mm_imm8, Register.MM5, 0xA5 };

				yield return new object[] { "66 0F73 F5 A5", 5, Code.Psllq_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F73 F5 A5", 6, Code.Psllq_xmm_imm8, Register.XMM13, 0xA5 };

				yield return new object[] { "66 0F73 FD A5", 5, Code.Pslldq_xmm_imm8, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F73 FD A5", 6, Code.Pslldq_xmm_imm8, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift73VEX_1_Data))]
		void Test16_Pshift73VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Pshift73VEX_1_Data {
			get {
				yield return new object[] { "C5C9 73 D5 A5", 5, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 D5 A5", 6, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 D5 A5", 5, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 D5 A5", 6, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 DD A5", 5, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 DD A5", 6, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 DD A5", 5, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 DD A5", 6, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 F5 A5", 5, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 F5 A5", 6, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 F5 A5", 5, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 F5 A5", 6, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 FD A5", 5, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 FD A5", 6, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 FD A5", 5, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 FD A5", 6, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift73VEX_1_Data))]
		void Test32_Pshift73VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Pshift73VEX_1_Data {
			get {
				yield return new object[] { "C5C9 73 D5 A5", 5, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 D5 A5", 6, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 D5 A5", 5, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 D5 A5", 6, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 DD A5", 5, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 DD A5", 6, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 DD A5", 5, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 DD A5", 6, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 F5 A5", 5, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 F5 A5", 6, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 F5 A5", 5, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 F5 A5", 6, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 FD A5", 5, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C4E1C9 73 FD A5", 6, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 FD A5", 5, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C4E1CD 73 FD A5", 6, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift73VEX_1_Data))]
		void Test64_Pshift73VEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Pshift73VEX_1_Data {
			get {
				yield return new object[] { "C5C9 73 D5 A5", 5, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 73 D5 A5", 5, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 73 D5 A5", 6, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 73 D5 A5", 6, Code.VEX_Vpsrlq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 D5 A5", 5, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 73 D5 A5", 5, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 73 D5 A5", 6, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 73 D5 A5", 6, Code.VEX_Vpsrlq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 DD A5", 5, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 73 DD A5", 5, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 73 DD A5", 6, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 73 DD A5", 6, Code.VEX_Vpsrldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 DD A5", 5, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 73 DD A5", 5, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 73 DD A5", 6, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 73 DD A5", 6, Code.VEX_Vpsrldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 F5 A5", 5, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 73 F5 A5", 5, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 73 F5 A5", 6, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 73 F5 A5", 6, Code.VEX_Vpsllq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 F5 A5", 5, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 73 F5 A5", 5, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 73 F5 A5", 6, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 73 F5 A5", 6, Code.VEX_Vpsllq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };

				yield return new object[] { "C5C9 73 FD A5", 5, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };
				yield return new object[] { "C589 73 FD A5", 5, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM14, Register.XMM5, 0xA5 };
				yield return new object[] { "C4C1C9 73 FD A5", 6, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM6, Register.XMM13, 0xA5 };
				yield return new object[] { "C4E1C9 73 FD A5", 6, Code.VEX_Vpslldq_xmm_xmm_imm8, Register.XMM6, Register.XMM5, 0xA5 };

				yield return new object[] { "C5CD 73 FD A5", 5, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
				yield return new object[] { "C58D 73 FD A5", 5, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM14, Register.YMM5, 0xA5 };
				yield return new object[] { "C4C1CD 73 FD A5", 6, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM6, Register.YMM13, 0xA5 };
				yield return new object[] { "C4E1CD 73 FD A5", 6, Code.VEX_Vpslldq_ymm_ymm_imm8, Register.YMM6, Register.YMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift73EVEX_1_Data))]
		void Test16_Pshift73EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Pshift73EVEX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 DD A5", 7, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 DD A5", 7, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 DD A5", 7, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 DD A5", 7, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 DD A5", 7, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 DD A5", 7, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 FD A5", 7, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 FD A5", 7, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 FD A5", 7, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 FD A5", 7, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 FD A5", 7, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 FD A5", 7, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Pshift73EVEX_2_Data))]
		void Test16_Pshift73EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Pshift73EVEX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 58 01 A5", 8, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 58 01 A5", 8, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 58 01 A5", 8, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 58 01 A5", 8, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 58 01 A5", 8, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 58 01 A5", 8, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 78 01 A5", 8, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 78 01 A5", 8, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 78 01 A5", 8, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 78 01 A5", 8, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 78 01 A5", 8, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 78 01 A5", 8, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift73EVEX_1_Data))]
		void Test32_Pshift73EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Pshift73EVEX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 DD A5", 7, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 DD A5", 7, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 DD A5", 7, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 DD A5", 7, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 DD A5", 7, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 DD A5", 7, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD8B 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDAB 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CDCB 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 FD A5", 7, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 FD A5", 7, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 FD A5", 7, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 FD A5", 7, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 FD A5", 7, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 FD A5", 7, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Pshift73EVEX_2_Data))]
		void Test32_Pshift73EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Pshift73EVEX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 58 01 A5", 8, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 58 01 A5", 8, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 58 01 A5", 8, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 58 01 A5", 8, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 58 01 A5", 8, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 58 01 A5", 8, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 78 01 A5", 8, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 78 01 A5", 8, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 78 01 A5", 8, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 78 01 A5", 8, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 78 01 A5", 8, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 78 01 A5", 8, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift73EVEX_1_Data))]
		void Test64_Pshift73EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Pshift73EVEX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CD8B 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D08 73 D5 A5", 7, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDAB 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D28 73 D5 A5", 7, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDCB 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D48 73 D5 A5", 7, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 DD A5", 7, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D08 73 DD A5", 7, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM13, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B10D08 73 DD A5", 7, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 DD A5", 7, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D28 73 DD A5", 7, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM13, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B10D28 73 DD A5", 7, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 DD A5", 7, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D48 73 DD A5", 7, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM13, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B10D48 73 DD A5", 7, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CD8B 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.XMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D08 73 F5 A5", 7, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDAB 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.YMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D28 73 F5 A5", 7, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM5, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D1CDCB 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.ZMM13, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 B18D48 73 F5 A5", 7, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 FD A5", 7, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D08 73 FD A5", 7, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.XMM13, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B10D08 73 FD A5", 7, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM14, Register.XMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 FD A5", 7, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D28 73 FD A5", 7, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.YMM13, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B10D28 73 FD A5", 7, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM14, Register.YMM21, Register.None, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 FD A5", 7, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM5, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 D14D48 73 FD A5", 7, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.ZMM13, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B10D48 73 FD A5", 7, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM14, Register.ZMM21, Register.None, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Pshift73EVEX_2_Data))]
		void Test64_Pshift73EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Pshift73EVEX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 73 50 01 A5", 8, Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 73 50 01 A5", 8, Code.EVEX_Vpsrlq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 73 50 01 A5", 8, Code.EVEX_Vpsrlq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 58 01 A5", 8, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 58 01 A5", 8, Code.EVEX_Vpsrldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 58 01 A5", 8, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 58 01 A5", 8, Code.EVEX_Vpsrldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 58 01 A5", 8, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 58 01 A5", 8, Code.EVEX_Vpsrldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };

				yield return new object[] { "62 F1CD0B 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD9D 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD08 73 70 01 A5", 8, Code.EVEX_Vpsllq_xmm_k1z_xmmm128b64_imm8, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F1CD2B 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F1CDBD 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD28 73 70 01 A5", 8, Code.EVEX_Vpsllq_ymm_k1z_ymmm256b64_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F1CD4B 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F1CDDD 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F1CD48 73 70 01 A5", 8, Code.EVEX_Vpsllq_zmm_k1z_zmmm512b64_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };

				yield return new object[] { "62 F14D08 73 78 01 A5", 8, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };
				yield return new object[] { "62 F1CD08 73 78 01 A5", 8, Code.EVEX_Vpslldq_xmm_xmmm128_imm8, Register.XMM6, Register.None, MemorySize.UInt128, 16, false, 0xA5 };

				yield return new object[] { "62 F14D28 73 78 01 A5", 8, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };
				yield return new object[] { "62 F1CD28 73 78 01 A5", 8, Code.EVEX_Vpslldq_ymm_ymmm256_imm8, Register.YMM6, Register.None, MemorySize.Packed256_UInt128, 32, false, 0xA5 };

				yield return new object[] { "62 F14D48 73 78 01 A5", 8, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
				yield return new object[] { "62 F1CD48 73 78 01 A5", 8, Code.EVEX_Vpslldq_zmm_zmmm512_imm8, Register.ZMM6, Register.None, MemorySize.Packed512_UInt128, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqbV_VX_WX_1_Data))]
		void Test16_PcmpeqbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test16_PcmpeqbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F74 08", 3, Code.Pcmpeqb_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F74 08", 4, Code.Pcmpeqb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqbV_VX_WX_2_Data))]
		void Test16_PcmpeqbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PcmpeqbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F74 CD", 3, Code.Pcmpeqb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F74 CD", 4, Code.Pcmpeqb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqbV_VX_WX_1_Data))]
		void Test32_PcmpeqbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test32_PcmpeqbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F74 08", 3, Code.Pcmpeqb_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F74 08", 4, Code.Pcmpeqb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqbV_VX_WX_2_Data))]
		void Test32_PcmpeqbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PcmpeqbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F74 CD", 3, Code.Pcmpeqb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F74 CD", 4, Code.Pcmpeqb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqbV_VX_WX_1_Data))]
		void Test64_PcmpeqbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test64_PcmpeqbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F74 08", 3, Code.Pcmpeqb_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F74 08", 4, Code.Pcmpeqb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqbV_VX_WX_2_Data))]
		void Test64_PcmpeqbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PcmpeqbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F74 CD", 3, Code.Pcmpeqb_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F74 CD", 4, Code.Pcmpeqb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F74 CD", 4, Code.Pcmpeqb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F74 CD", 5, Code.Pcmpeqb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F74 CD", 5, Code.Pcmpeqb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F74 CD", 5, Code.Pcmpeqb_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqbV_VX_HX_WX_1_Data))]
		void Test16_VpcmpeqbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 74 10", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD 74 10", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 74 10", 5, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD 74 10", 5, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqbV_VX_HX_WX_2_Data))]
		void Test16_VpcmpeqbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 74 D3", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 74 D3", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqbV_VX_HX_WX_1_Data))]
		void Test32_VpcmpeqbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 74 10", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD 74 10", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 74 10", 5, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD 74 10", 5, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqbV_VX_HX_WX_2_Data))]
		void Test32_VpcmpeqbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 74 D3", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 74 D3", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqbV_VX_HX_WX_1_Data))]
		void Test64_VpcmpeqbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 74 10", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD 74 10", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 74 10", 5, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD 74 10", 5, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqbV_VX_HX_WX_2_Data))]
		void Test64_VpcmpeqbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 74 D3", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 74 D3", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 74 D3", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 74 D3", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 74 D3", 4, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 74 D3", 4, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 74 D3", 5, Code.VEX_Vpcmpeqb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 74 D3", 5, Code.VEX_Vpcmpeqb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpcmpeqbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D0D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D08 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14D2D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14D28 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14D4D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14D48 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpcmpeqbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpcmpeqbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D0D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D08 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14D2D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14D28 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14D4D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14D48 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpcmpeqbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpcmpeqbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D0D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D08 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14D2D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14D28 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14D4D 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14D48 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B 74 50 01", 7, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpcmpeqbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F10D0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 914D03 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F10D2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 914D23 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F10D4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 914D43 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 74 D3", 6, Code.EVEX_Vpcmpeqb_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqwV_VX_WX_1_Data))]
		void Test16_PcmpeqwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test16_PcmpeqwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F75 08", 3, Code.Pcmpeqw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F75 08", 4, Code.Pcmpeqw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqwV_VX_WX_2_Data))]
		void Test16_PcmpeqwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PcmpeqwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F75 CD", 3, Code.Pcmpeqw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F75 CD", 4, Code.Pcmpeqw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqwV_VX_WX_1_Data))]
		void Test32_PcmpeqwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test32_PcmpeqwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F75 08", 3, Code.Pcmpeqw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F75 08", 4, Code.Pcmpeqw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqwV_VX_WX_2_Data))]
		void Test32_PcmpeqwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PcmpeqwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F75 CD", 3, Code.Pcmpeqw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F75 CD", 4, Code.Pcmpeqw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqwV_VX_WX_1_Data))]
		void Test64_PcmpeqwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test64_PcmpeqwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F75 08", 3, Code.Pcmpeqw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F75 08", 4, Code.Pcmpeqw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqwV_VX_WX_2_Data))]
		void Test64_PcmpeqwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PcmpeqwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F75 CD", 3, Code.Pcmpeqw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F75 CD", 4, Code.Pcmpeqw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F75 CD", 4, Code.Pcmpeqw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F75 CD", 5, Code.Pcmpeqw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F75 CD", 5, Code.Pcmpeqw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F75 CD", 5, Code.Pcmpeqw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqwV_VX_HX_WX_1_Data))]
		void Test16_VpcmpeqwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 75 10", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD 75 10", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 75 10", 5, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD 75 10", 5, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqwV_VX_HX_WX_2_Data))]
		void Test16_VpcmpeqwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 75 D3", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 75 D3", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqwV_VX_HX_WX_1_Data))]
		void Test32_VpcmpeqwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 75 10", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD 75 10", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 75 10", 5, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD 75 10", 5, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqwV_VX_HX_WX_2_Data))]
		void Test32_VpcmpeqwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 75 D3", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 75 D3", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqwV_VX_HX_WX_1_Data))]
		void Test64_VpcmpeqwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 75 10", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD 75 10", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 75 10", 5, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD 75 10", 5, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqwV_VX_HX_WX_2_Data))]
		void Test64_VpcmpeqwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 75 D3", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 75 D3", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 75 D3", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 75 D3", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 75 D3", 4, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 75 D3", 4, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 75 D3", 5, Code.VEX_Vpcmpeqw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 75 D3", 5, Code.VEX_Vpcmpeqw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpcmpeqwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D0D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D08 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14D2D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14D28 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14D4D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14D48 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpcmpeqwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpcmpeqwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D0D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D08 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14D2D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14D28 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14D4D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14D48 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpcmpeqwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpcmpeqwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D0D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D08 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14D2D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14D28 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14D4D 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14D48 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B 75 50 01", 7, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpcmpeqwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F10D0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 914D03 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_xmm_xmmm128, Register.K2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F10D2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 914D23 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_ymm_ymmm256, Register.K2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F10D4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 914D43 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 75 D3", 6, Code.EVEX_Vpcmpeqw_k_k1_zmm_zmmm512, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqdV_VX_WX_1_Data))]
		void Test16_PcmpeqdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test16_PcmpeqdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F76 08", 3, Code.Pcmpeqd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F76 08", 4, Code.Pcmpeqd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PcmpeqdV_VX_WX_2_Data))]
		void Test16_PcmpeqdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PcmpeqdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F76 CD", 3, Code.Pcmpeqd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F76 CD", 4, Code.Pcmpeqd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqdV_VX_WX_1_Data))]
		void Test32_PcmpeqdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test32_PcmpeqdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F76 08", 3, Code.Pcmpeqd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F76 08", 4, Code.Pcmpeqd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PcmpeqdV_VX_WX_2_Data))]
		void Test32_PcmpeqdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PcmpeqdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F76 CD", 3, Code.Pcmpeqd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F76 CD", 4, Code.Pcmpeqd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqdV_VX_WX_1_Data))]
		void Test64_PcmpeqdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test64_PcmpeqdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F76 08", 3, Code.Pcmpeqd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F76 08", 4, Code.Pcmpeqd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PcmpeqdV_VX_WX_2_Data))]
		void Test64_PcmpeqdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PcmpeqdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F76 CD", 3, Code.Pcmpeqd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F76 CD", 4, Code.Pcmpeqd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F76 CD", 4, Code.Pcmpeqd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F76 CD", 5, Code.Pcmpeqd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F76 CD", 5, Code.Pcmpeqd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F76 CD", 5, Code.Pcmpeqd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqdV_VX_HX_WX_1_Data))]
		void Test16_VpcmpeqdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 76 10", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 76 10", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 76 10", 5, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 76 10", 5, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqdV_VX_HX_WX_2_Data))]
		void Test16_VpcmpeqdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 76 D3", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 76 D3", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqdV_VX_HX_WX_1_Data))]
		void Test32_VpcmpeqdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 76 10", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 76 10", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 76 10", 5, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 76 10", 5, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqdV_VX_HX_WX_2_Data))]
		void Test32_VpcmpeqdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 76 D3", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 76 D3", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqdV_VX_HX_WX_1_Data))]
		void Test64_VpcmpeqdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 76 10", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 76 10", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 76 10", 5, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 76 10", 5, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqdV_VX_HX_WX_2_Data))]
		void Test64_VpcmpeqdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 76 D3", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 76 D3", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 76 D3", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 76 D3", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 76 D3", 4, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 76 D3", 4, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 76 D3", 5, Code.VEX_Vpcmpeqd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 76 D3", 5, Code.VEX_Vpcmpeqd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpcmpeqdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D1D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, false };
				yield return new object[] { "62 F14D08 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14D3D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, false };
				yield return new object[] { "62 F14D28 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14D5D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, false };
				yield return new object[] { "62 F14D48 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpeqdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpcmpeqdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpcmpeqdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14D2B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14D4B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpcmpeqdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D1D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, false };
				yield return new object[] { "62 F14D08 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14D3D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, false };
				yield return new object[] { "62 F14D28 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14D5D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, false };
				yield return new object[] { "62 F14D48 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpeqdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpcmpeqdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpcmpeqdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14D2B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14D4B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpcmpeqdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D1D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, false };
				yield return new object[] { "62 F14D08 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14D3D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, false };
				yield return new object[] { "62 F14D28 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14D5D 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, false };
				yield return new object[] { "62 F14D48 76 50 01", 7, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpeqdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpcmpeqdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpcmpeqdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F10D0B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 914D03 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14D0B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_xmm_xmmm128b32, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false };

				yield return new object[] { "62 F14D2B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F10D2B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 914D23 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14D2B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_ymm_ymmm256b32, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false };

				yield return new object[] { "62 F14D4B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F10D4B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 914D43 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14D4B 76 D3", 6, Code.EVEX_Vpcmpeqd_k_k1_zmm_zmmm512b32, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false };
			}
		}

		[Theory]
		[InlineData("0F77", 2, Code.Emms)]
		[InlineData("C5F8 77", 3, Code.VEX_Vzeroupper)]
		[InlineData("C4E1F8 77", 4, Code.VEX_Vzeroupper)]
		[InlineData("C5FC 77", 3, Code.VEX_Vzeroall)]
		[InlineData("C4E1FC 77", 4, Code.VEX_Vzeroall)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("0F77", 2, Code.Emms)]
		[InlineData("C5F8 77", 3, Code.VEX_Vzeroupper)]
		[InlineData("C4E1F8 77", 4, Code.VEX_Vzeroupper)]
		[InlineData("C5FC 77", 3, Code.VEX_Vzeroall)]
		[InlineData("C4E1FC 77", 4, Code.VEX_Vzeroall)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("0F77", 2, Code.Emms)]
		[InlineData("C5F8 77", 3, Code.VEX_Vzeroupper)]
		[InlineData("C4E1F8 77", 4, Code.VEX_Vzeroupper)]
		[InlineData("C5FC 77", 3, Code.VEX_Vzeroall)]
		[InlineData("C4E1FC 77", 4, Code.VEX_Vzeroall)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}
	}
}
