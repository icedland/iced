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
	public sealed class DecoderTest_2_0F10_0F17 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Umov_E_G_1_Data))]
		void Test16_Umov_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
		public static IEnumerable<object[]> Test16_Umov_E_G_1_Data {
			get {
				yield return new object[] { "0F10 CE", 3, Code.Umov_rm8_r8, Register.DH, Register.CL, DecoderOptions.Umov };
				yield return new object[] { "0F11 CE", 3, Code.Umov_rm16_r16, Register.SI, Register.CX, DecoderOptions.Umov };
				yield return new object[] { "66 0F11 CE", 4, Code.Umov_rm32_r32, Register.ESI, Register.ECX, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Umov_E_G_2_Data))]
		void Test16_Umov_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_Umov_E_G_2_Data {
			get {
				yield return new object[] { "0F10 18", 3, Code.Umov_rm8_r8, MemorySize.UInt8, Register.BL, DecoderOptions.Umov };
				yield return new object[] { "0F11 18", 3, Code.Umov_rm16_r16, MemorySize.UInt16, Register.BX, DecoderOptions.Umov };
				yield return new object[] { "66 0F11 18", 4, Code.Umov_rm32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Umov_E_G_1_Data))]
		void Test32_Umov_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
		public static IEnumerable<object[]> Test32_Umov_E_G_1_Data {
			get {
				yield return new object[] { "0F10 CE", 3, Code.Umov_rm8_r8, Register.DH, Register.CL, DecoderOptions.Umov };
				yield return new object[] { "66 0F11 CE", 4, Code.Umov_rm16_r16, Register.SI, Register.CX, DecoderOptions.Umov };
				yield return new object[] { "0F11 CE", 3, Code.Umov_rm32_r32, Register.ESI, Register.ECX, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Umov_E_G_2_Data))]
		void Test32_Umov_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_Umov_E_G_2_Data {
			get {
				yield return new object[] { "0F10 18", 3, Code.Umov_rm8_r8, MemorySize.UInt8, Register.BL, DecoderOptions.Umov };
				yield return new object[] { "66 0F11 18", 4, Code.Umov_rm16_r16, MemorySize.UInt16, Register.BX, DecoderOptions.Umov };
				yield return new object[] { "0F11 18", 3, Code.Umov_rm32_r32, MemorySize.UInt32, Register.EBX, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Umov_G_E_1_Data))]
		void Test16_Umov_G_E_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
		public static IEnumerable<object[]> Test16_Umov_G_E_1_Data {
			get {
				yield return new object[] { "0F12 CE", 3, Code.Umov_r8_rm8, Register.CL, Register.DH, DecoderOptions.Umov };
				yield return new object[] { "0F13 CE", 3, Code.Umov_r16_rm16, Register.CX, Register.SI, DecoderOptions.Umov };
				yield return new object[] { "66 0F13 CE", 4, Code.Umov_r32_rm32, Register.ECX, Register.ESI, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Umov_G_E_2_Data))]
		void Test16_Umov_G_E_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
		public static IEnumerable<object[]> Test16_Umov_G_E_2_Data {
			get {
				yield return new object[] { "0F12 18", 3, Code.Umov_r8_rm8, Register.BL, MemorySize.UInt8, DecoderOptions.Umov };
				yield return new object[] { "0F13 18", 3, Code.Umov_r16_rm16, Register.BX, MemorySize.UInt16, DecoderOptions.Umov };
				yield return new object[] { "66 0F13 18", 4, Code.Umov_r32_rm32, Register.EBX, MemorySize.UInt32, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Umov_G_E_1_Data))]
		void Test32_Umov_G_E_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
		public static IEnumerable<object[]> Test32_Umov_G_E_1_Data {
			get {
				yield return new object[] { "0F12 CE", 3, Code.Umov_r8_rm8, Register.CL, Register.DH, DecoderOptions.Umov };
				yield return new object[] { "66 0F13 CE", 4, Code.Umov_r16_rm16, Register.CX, Register.SI, DecoderOptions.Umov };
				yield return new object[] { "0F13 CE", 3, Code.Umov_r32_rm32, Register.ECX, Register.ESI, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Umov_G_E_2_Data))]
		void Test32_Umov_G_E_2(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
		public static IEnumerable<object[]> Test32_Umov_G_E_2_Data {
			get {
				yield return new object[] { "0F12 18", 3, Code.Umov_r8_rm8, Register.BL, MemorySize.UInt8, DecoderOptions.Umov };
				yield return new object[] { "66 0F13 18", 4, Code.Umov_r16_rm16, Register.BX, MemorySize.UInt16, DecoderOptions.Umov };
				yield return new object[] { "0F13 18", 3, Code.Umov_r32_rm32, Register.EBX, MemorySize.UInt32, DecoderOptions.Umov };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_Reg_RegMem_1_Data))]
		void Test16_MovuV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovuV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F10 08", 3, Code.Movups_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F10 08", 4, Code.Movupd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 10 10", 4, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 10 10", 5, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 10 10", 4, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 10 10", 5, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 10 10", 4, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 10 10", 5, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 10 10", 4, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 10 10", 5, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_Reg_RegMem_2_Data))]
		void Test16_MovuV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovuV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F10 CD", 3, Code.Movups_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F10 CD", 4, Code.Movupd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 10 CD", 4, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 10 CD", 4, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5F9 10 CD", 4, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 10 CD", 4, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_Reg_RegMem_1_Data))]
		void Test32_MovuV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovuV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F10 08", 3, Code.Movups_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F10 08", 4, Code.Movupd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 10 10", 4, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 10 10", 5, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 10 10", 4, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 10 10", 5, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 10 10", 4, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 10 10", 5, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 10 10", 4, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 10 10", 5, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_Reg_RegMem_2_Data))]
		void Test32_MovuV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovuV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F10 CD", 3, Code.Movups_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F10 CD", 4, Code.Movupd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 10 CD", 4, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 10 CD", 4, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5F9 10 CD", 4, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 10 CD", 4, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_Reg_RegMem_1_Data))]
		void Test64_MovuV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovuV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F10 08", 3, Code.Movups_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F10 08", 4, Code.Movupd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 10 10", 4, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 10 10", 5, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 10 10", 4, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 10 10", 5, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 10 10", 4, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 10 10", 5, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 10 10", 4, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 10 10", 5, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_Reg_RegMem_2_Data))]
		void Test64_MovuV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovuV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F10 CD", 3, Code.Movups_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F10 CD", 4, Code.Movups_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F10 CD", 4, Code.Movups_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F10 CD", 4, Code.Movups_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F10 CD", 4, Code.Movupd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F10 CD", 5, Code.Movupd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F10 CD", 5, Code.Movupd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F10 CD", 5, Code.Movupd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F8 10 CD", 4, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C578 10 CD", 4, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C178 10 CD", 5, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44178 10 CD", 5, Code.VEX_Vmovups_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FC 10 CD", 4, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57C 10 CD", 4, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17C 10 CD", 5, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417C 10 CD", 5, Code.VEX_Vmovups_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "C5F9 10 CD", 4, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C579 10 CD", 4, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C179 10 CD", 5, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44179 10 CD", 5, Code.VEX_Vmovupd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FD 10 CD", 4, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57D 10 CD", 4, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17D 10 CD", 5, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417D 10 CD", 5, Code.VEX_Vmovupd_ymm_ymmm256, Register.YMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_Reg_RegMem_EVEX_1_Data))]
		void Test16_MovuV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test16_MovuV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 10 50 01", 7, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C8B 10 50 01", 7, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17C28 10 50 01", 7, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CAB 10 50 01", 7, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17C48 10 50 01", 7, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CCB 10 50 01", 7, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FD08 10 50 01", 7, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD8B 10 50 01", 7, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F1FD28 10 50 01", 7, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDAB 10 50 01", 7, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FD48 10 50 01", 7, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDCB 10 50 01", 7, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_Reg_RegMem_EVEX_2_Data))]
		void Test16_MovuV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovuV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_Reg_RegMem_EVEX_1_Data))]
		void Test32_MovuV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test32_MovuV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 10 50 01", 7, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C8B 10 50 01", 7, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17C28 10 50 01", 7, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CAB 10 50 01", 7, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17C48 10 50 01", 7, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CCB 10 50 01", 7, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FD08 10 50 01", 7, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD8B 10 50 01", 7, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F1FD28 10 50 01", 7, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDAB 10 50 01", 7, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FD48 10 50 01", 7, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDCB 10 50 01", 7, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_Reg_RegMem_EVEX_2_Data))]
		void Test32_MovuV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovuV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_Reg_RegMem_EVEX_1_Data))]
		void Test64_MovuV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test64_MovuV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 10 50 01", 7, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C8B 10 50 01", 7, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17C28 10 50 01", 7, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CAB 10 50 01", 7, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17C48 10 50 01", 7, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CCB 10 50 01", 7, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FD08 10 50 01", 7, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD8B 10 50 01", 7, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F1FD28 10 50 01", 7, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDAB 10 50 01", 7, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FD48 10 50 01", 7, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDCB 10 50 01", 7, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_Reg_RegMem_EVEX_2_Data))]
		void Test64_MovuV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovuV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317C8B 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17C8B 10 D3", 6, Code.EVEX_Vmovups_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CAB 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CAB 10 D3", 6, Code.EVEX_Vmovups_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CCB 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CCB 10 D3", 6, Code.EVEX_Vmovups_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FD8B 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FD8B 10 D3", 6, Code.EVEX_Vmovupd_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDAB 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDAB 10 D3", 6, Code.EVEX_Vmovupd_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDCB 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDCB 10 D3", 6, Code.EVEX_Vmovupd_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_RegMem_Reg_1_Data))]
		void Test16_MovuV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_MovuV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F11 08", 3, Code.Movups_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F11 08", 4, Code.Movupd_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 11 10", 4, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 11 10", 5, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 11 10", 4, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 11 10", 5, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 11 10", 4, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 11 10", 5, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 11 10", 4, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 11 10", 5, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_RegMem_Reg_2_Data))]
		void Test16_MovuV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovuV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F11 CD", 3, Code.Movups_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "66 0F11 CD", 4, Code.Movupd_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F8 11 CD", 4, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FC 11 CD", 4, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM5, Register.YMM1 };

				yield return new object[] { "C5F9 11 CD", 4, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FD 11 CD", 4, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM5, Register.YMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_RegMem_Reg_1_Data))]
		void Test32_MovuV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_MovuV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F11 08", 3, Code.Movups_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F11 08", 4, Code.Movupd_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 11 10", 4, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 11 10", 5, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 11 10", 4, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 11 10", 5, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 11 10", 4, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 11 10", 5, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 11 10", 4, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 11 10", 5, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_RegMem_Reg_2_Data))]
		void Test32_MovuV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovuV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F11 CD", 3, Code.Movups_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "66 0F11 CD", 4, Code.Movupd_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F8 11 CD", 4, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FC 11 CD", 4, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM5, Register.YMM1 };

				yield return new object[] { "C5F9 11 CD", 4, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FD 11 CD", 4, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM5, Register.YMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_RegMem_Reg_1_Data))]
		void Test64_MovuV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_MovuV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F11 08", 3, Code.Movups_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F11 08", 4, Code.Movupd_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 11 10", 4, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 11 10", 5, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 11 10", 4, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 11 10", 5, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 11 10", 4, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 11 10", 5, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 11 10", 4, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 11 10", 5, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_RegMem_Reg_2_Data))]
		void Test64_MovuV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovuV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F11 CD", 3, Code.Movups_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "44 0F11 CD", 4, Code.Movups_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "41 0F11 CD", 4, Code.Movups_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "45 0F11 CD", 4, Code.Movups_xmmm128_xmm, Register.XMM13, Register.XMM9 };

				yield return new object[] { "66 0F11 CD", 4, Code.Movupd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "66 44 0F11 CD", 5, Code.Movupd_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "66 41 0F11 CD", 5, Code.Movupd_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "66 45 0F11 CD", 5, Code.Movupd_xmmm128_xmm, Register.XMM13, Register.XMM9 };

				yield return new object[] { "C5F8 11 CD", 4, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C578 11 CD", 4, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "C4C178 11 CD", 5, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "C44178 11 CD", 5, Code.VEX_Vmovups_xmmm128_xmm, Register.XMM13, Register.XMM9 };
				yield return new object[] { "C5FC 11 CD", 4, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM5, Register.YMM1 };
				yield return new object[] { "C57C 11 CD", 4, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM5, Register.YMM9 };
				yield return new object[] { "C4C17C 11 CD", 5, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM13, Register.YMM1 };
				yield return new object[] { "C4417C 11 CD", 5, Code.VEX_Vmovups_ymmm256_ymm, Register.YMM13, Register.YMM9 };

				yield return new object[] { "C5F9 11 CD", 4, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C579 11 CD", 4, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "C4C179 11 CD", 5, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "C44179 11 CD", 5, Code.VEX_Vmovupd_xmmm128_xmm, Register.XMM13, Register.XMM9 };
				yield return new object[] { "C5FD 11 CD", 4, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM5, Register.YMM1 };
				yield return new object[] { "C57D 11 CD", 4, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM5, Register.YMM9 };
				yield return new object[] { "C4C17D 11 CD", 5, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM13, Register.YMM1 };
				yield return new object[] { "C4417D 11 CD", 5, Code.VEX_Vmovupd_ymmm256_ymm, Register.YMM13, Register.YMM9 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_RegMem_Reg_EVEX_1_Data))]
		void Test16_MovuV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovuV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 11 50 01", 7, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C0B 11 50 01", 7, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 11 50 01", 7, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17C2B 11 50 01", 7, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 11 50 01", 7, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17C4B 11 50 01", 7, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 11 50 01", 7, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD0B 11 50 01", 7, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 11 50 01", 7, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FD2B 11 50 01", 7, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 11 50 01", 7, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FD4B 11 50 01", 7, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovuV_RegMem_Reg_EVEX_2_Data))]
		void Test16_MovuV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovuV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_RegMem_Reg_EVEX_1_Data))]
		void Test32_MovuV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovuV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 11 50 01", 7, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C0B 11 50 01", 7, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 11 50 01", 7, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17C2B 11 50 01", 7, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 11 50 01", 7, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17C4B 11 50 01", 7, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 11 50 01", 7, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD0B 11 50 01", 7, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 11 50 01", 7, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FD2B 11 50 01", 7, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 11 50 01", 7, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FD4B 11 50 01", 7, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovuV_RegMem_Reg_EVEX_2_Data))]
		void Test32_MovuV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovuV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_RegMem_Reg_EVEX_1_Data))]
		void Test64_MovuV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovuV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 11 50 01", 7, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C0B 11 50 01", 7, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 11 50 01", 7, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17C2B 11 50 01", 7, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 11 50 01", 7, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17C4B 11 50 01", 7, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 11 50 01", 7, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD0B 11 50 01", 7, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 11 50 01", 7, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FD2B 11 50 01", 7, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 11 50 01", 7, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FD4B 11 50 01", 7, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovuV_RegMem_Reg_EVEX_2_Data))]
		void Test64_MovuV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovuV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317C8B 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17C8B 11 D3", 6, Code.EVEX_Vmovups_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CAB 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CAB 11 D3", 6, Code.EVEX_Vmovups_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CCB 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CCB 11 D3", 6, Code.EVEX_Vmovups_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FD8B 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FD8B 11 D3", 6, Code.EVEX_Vmovupd_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDAB 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDAB 11 D3", 6, Code.EVEX_Vmovupd_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDCB 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDCB 11 D3", 6, Code.EVEX_Vmovupd_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_Reg_RegMem_1_Data))]
		void Test16_MovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "F3 0F10 08", 4, Code.Movss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F10 08", 4, Code.Movsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FA 10 10", 4, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FE 10 10", 4, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 10 10", 5, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5FB 10 10", 4, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FF 10 10", 4, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 10 10", 5, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_Reg_RegMem_2_Data))]
		void Test16_MovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "F3 0F10 CD", 4, Code.Movss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F10 CD", 4, Code.Movsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_Reg_RegMem_3_Data))]
		void Test16_MovV_Reg_RegMem_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_MovV_Reg_RegMem_3_Data {
			get {
				yield return new object[] { "C5CA 10 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C5CE 10 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4E1CE 10 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };

				yield return new object[] { "C5CB 10 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C5CF 10 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4E1CF 10 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_Reg_RegMem_1_Data))]
		void Test32_MovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "F3 0F10 08", 4, Code.Movss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F10 08", 4, Code.Movsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FA 10 10", 4, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FE 10 10", 4, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 10 10", 5, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5FB 10 10", 4, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FF 10 10", 4, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 10 10", 5, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_Reg_RegMem_2_Data))]
		void Test32_MovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "F3 0F10 CD", 4, Code.Movss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0F10 CD", 4, Code.Movsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_Reg_RegMem_3_Data))]
		void Test32_MovV_Reg_RegMem_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_MovV_Reg_RegMem_3_Data {
			get {
				yield return new object[] { "C5CA 10 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C5CE 10 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4E1CE 10 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };

				yield return new object[] { "C5CB 10 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C5CF 10 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4E1CF 10 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_Reg_RegMem_1_Data))]
		void Test64_MovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "F3 0F10 08", 4, Code.Movss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F10 08", 4, Code.Movsd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FA 10 10", 4, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C57A 10 10", 4, Code.VEX_Vmovss_xmm_m32, Register.XMM10, MemorySize.Float32 };
				yield return new object[] { "C5FE 10 10", 4, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 10 10", 5, Code.VEX_Vmovss_xmm_m32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5FB 10 10", 4, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C57B 10 10", 4, Code.VEX_Vmovsd_xmm_m64, Register.XMM10, MemorySize.Float64 };
				yield return new object[] { "C5FF 10 10", 4, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 10 10", 5, Code.VEX_Vmovsd_xmm_m64, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_Reg_RegMem_2_Data))]
		void Test64_MovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "F3 0F10 CD", 4, Code.Movss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F10 CD", 5, Code.Movss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F10 CD", 5, Code.Movss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F10 CD", 5, Code.Movss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0F10 CD", 4, Code.Movsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F10 CD", 5, Code.Movsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F10 CD", 5, Code.Movsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F10 CD", 5, Code.Movsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_Reg_RegMem_3_Data))]
		void Test64_MovV_Reg_RegMem_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_MovV_Reg_RegMem_3_Data {
			get {
				yield return new object[] { "C5CA 10 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4414A 10 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM9, Register.XMM6, Register.XMM13 };
				yield return new object[] { "C4c10A 10 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM14, Register.XMM13 };
				yield return new object[] { "C50A 10 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM9, Register.XMM14, Register.XMM5 };
				yield return new object[] { "C5CE 10 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4E14E 10 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };

				yield return new object[] { "C5CB 10 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4414B 10 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM9, Register.XMM6, Register.XMM13 };
				yield return new object[] { "C4c10B 10 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM14, Register.XMM13 };
				yield return new object[] { "C50B 10 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM9, Register.XMM14, Register.XMM5 };
				yield return new object[] { "C5CF 10 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
				yield return new object[] { "C4E1CF 10 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm, Register.XMM1, Register.XMM6, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_Reg_RegMem_EVEX_1_Data))]
		void Test16_MovV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test16_MovV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E8B 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F17E28 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E48 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E68 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F1FF08 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF8B 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F1FF28 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF48 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF68 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_Reg_RegMem_EVEX_2_Data))]
		void Test16_MovV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_MovV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E8B 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F14E28 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E48 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E68 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CF08 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF8B 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF28 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF48 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF68 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_Reg_RegMem_EVEX_1_Data))]
		void Test32_MovV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test32_MovV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E8B 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1FF08 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF8B 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_Reg_RegMem_EVEX_2_Data))]
		void Test32_MovV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_MovV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E8B 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F14E28 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E48 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E68 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CF08 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF8B 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF28 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF48 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF68 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_Reg_RegMem_EVEX_1_Data))]
		void Test64_MovV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test64_MovV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 717E08 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM10, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 617E08 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM26, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E8B 10 50 01", 7, Code.EVEX_Vmovss_xmm_k1z_m32, Register.XMM2, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F1FF08 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 71FF08 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM10, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 61FF08 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM26, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF8B 10 50 01", 7, Code.EVEX_Vmovsd_xmm_k1z_m64, Register.XMM2, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_Reg_RegMem_EVEX_2_Data))]
		void Test64_MovV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_MovV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 310E03 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM10, Register.XMM30, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 810E8B 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM18, Register.XMM14, Register.XMM27, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F14E28 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E48 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E68 10 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CF08 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 318F03 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM10, Register.XMM30, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 818F8B 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM18, Register.XMM14, Register.XMM27, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF28 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF48 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF68 10 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_RegMem_Reg_1_Data))]
		void Test16_MovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_MovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "F3 0F11 08", 4, Code.Movss_xmmm32_xmm, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F11 08", 4, Code.Movsd_xmmm64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FA 11 10", 4, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FE 11 10", 4, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 11 10", 5, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5FB 11 10", 4, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FF 11 10", 4, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 11 10", 5, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_RegMem_Reg_2_Data))]
		void Test16_MovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "F3 0F11 CD", 4, Code.Movss_xmmm32_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "F2 0F11 CD", 4, Code.Movsd_xmmm64_xmm, Register.XMM5, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_RegMem_Reg_3_Data))]
		void Test16_MovV_RegMem_Reg_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_MovV_RegMem_Reg_3_Data {
			get {
				yield return new object[] { "C5CA 11 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C5CE 11 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4E1CE 11 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };

				yield return new object[] { "C5CB 11 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C5CF 11 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4E1CF 11 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_RegMem_Reg_1_Data))]
		void Test32_MovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_MovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "F3 0F11 08", 4, Code.Movss_xmmm32_xmm, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F11 08", 4, Code.Movsd_xmmm64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FA 11 10", 4, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FE 11 10", 4, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 11 10", 5, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5FB 11 10", 4, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FF 11 10", 4, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 11 10", 5, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_RegMem_Reg_2_Data))]
		void Test32_MovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "F3 0F11 CD", 4, Code.Movss_xmmm32_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "F2 0F11 CD", 4, Code.Movsd_xmmm64_xmm, Register.XMM5, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_RegMem_Reg_3_Data))]
		void Test32_MovV_RegMem_Reg_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_MovV_RegMem_Reg_3_Data {
			get {
				yield return new object[] { "C5CA 11 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C5CE 11 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4E1CE 11 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };

				yield return new object[] { "C5CB 11 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C5CF 11 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4E1CF 11 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_RegMem_Reg_1_Data))]
		void Test64_MovV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_MovV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "F3 0F11 08", 4, Code.Movss_xmmm32_xmm, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F11 08", 4, Code.Movsd_xmmm64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FA 11 10", 4, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C57A 11 10", 4, Code.VEX_Vmovss_m32_xmm, Register.XMM10, MemorySize.Float32 };
				yield return new object[] { "C5FE 11 10", 4, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 11 10", 5, Code.VEX_Vmovss_m32_xmm, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5FB 11 10", 4, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C57B 11 10", 4, Code.VEX_Vmovsd_m64_xmm, Register.XMM10, MemorySize.Float64 };
				yield return new object[] { "C5FF 11 10", 4, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 11 10", 5, Code.VEX_Vmovsd_m64_xmm, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_RegMem_Reg_2_Data))]
		void Test64_MovV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "F3 0F11 CD", 4, Code.Movss_xmmm32_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "F3 44 0F11 CD", 5, Code.Movss_xmmm32_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "F3 41 0F11 CD", 5, Code.Movss_xmmm32_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "F3 45 0F11 CD", 5, Code.Movss_xmmm32_xmm, Register.XMM13, Register.XMM9 };

				yield return new object[] { "F2 0F11 CD", 4, Code.Movsd_xmmm64_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "F2 44 0F11 CD", 5, Code.Movsd_xmmm64_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "F2 41 0F11 CD", 5, Code.Movsd_xmmm64_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "F2 45 0F11 CD", 5, Code.Movsd_xmmm64_xmm, Register.XMM13, Register.XMM9 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_RegMem_Reg_3_Data))]
		void Test64_MovV_RegMem_Reg_3(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_MovV_RegMem_Reg_3_Data {
			get {
				yield return new object[] { "C5CA 11 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4414A 11 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM13, Register.XMM6, Register.XMM9 };
				yield return new object[] { "C4c10A 11 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM13, Register.XMM14, Register.XMM1 };
				yield return new object[] { "C50A 11 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM14, Register.XMM9 };
				yield return new object[] { "C5CE 11 CD", 4, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4E14E 11 CD", 5, Code.VEX_Vmovss_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };

				yield return new object[] { "C5CB 11 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4414B 11 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM13, Register.XMM6, Register.XMM9 };
				yield return new object[] { "C4c10B 11 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM13, Register.XMM14, Register.XMM1 };
				yield return new object[] { "C50B 11 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM14, Register.XMM9 };
				yield return new object[] { "C5CF 11 CD", 4, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
				yield return new object[] { "C4E1CF 11 CD", 5, Code.VEX_Vmovsd_xmm_xmm_xmm_0F11, Register.XMM5, Register.XMM6, Register.XMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_RegMem_Reg_EVEX_1_Data))]
		void Test16_MovV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E0B 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E28 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E48 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E68 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F1FF08 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF0B 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF28 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF48 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF68 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_RegMem_Reg_EVEX_2_Data))]
		void Test16_MovV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_MovV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E8B 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F14E28 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E48 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E68 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CF08 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF8B 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF28 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF48 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF68 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_RegMem_Reg_EVEX_1_Data))]
		void Test32_MovV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E0B 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F1FF08 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF0B 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_RegMem_Reg_EVEX_2_Data))]
		void Test32_MovV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_MovV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E8B 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F14E28 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E48 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E68 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CF08 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF8B 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF28 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF48 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF68 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_RegMem_Reg_EVEX_1_Data))]
		void Test64_MovV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 717E08 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM10, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 617E08 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM26, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F17E0B 11 50 01", 7, Code.EVEX_Vmovss_m32_k1_xmm, Register.XMM2, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F1FF08 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 71FF08 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM10, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 61FF08 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM26, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF0B 11 50 01", 7, Code.EVEX_Vmovsd_m64_k1_xmm, Register.XMM2, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_RegMem_Reg_EVEX_2_Data))]
		void Test64_MovV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_MovV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 310E03 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM19, Register.XMM30, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 810E8B 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM27, Register.XMM14, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F14E28 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E48 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F14E68 11 D3", 6, Code.EVEX_Vmovss_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CF08 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 318F03 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM19, Register.XMM30, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 818F8B 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM27, Register.XMM14, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CF28 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF48 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CF68 11 D3", 6, Code.EVEX_Vmovsd_xmm_k1z_xmm_xmm_0F11, Register.XMM3, Register.XMM6, Register.XMM2, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovddupV_Reg_RegMem_1_Data))]
		void Test16_MovddupV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovddupV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "F3 0F12 08", 4, Code.Movsldup_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FA 12 10", 4, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 12 10", 5, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 12 10", 4, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 12 10", 5, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "F3 0F16 08", 4, Code.Movshdup_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FA 16 10", 4, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 16 10", 5, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 16 10", 4, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 16 10", 5, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "F2 0F12 08", 4, Code.Movddup_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FB 12 10", 4, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FB 12 10", 5, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5FF 12 10", 4, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FF 12 10", 5, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovddupV_Reg_RegMem_2_Data))]
		void Test16_MovddupV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovddupV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "F3 0F12 CD", 4, Code.Movsldup_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 12 CD", 4, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 12 CD", 4, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "F3 0F16 CD", 4, Code.Movshdup_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 16 CD", 4, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 16 CD", 4, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "F2 0F12 CD", 4, Code.Movddup_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FB 12 CD", 4, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FF 12 CD", 4, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovddupV_Reg_RegMem_1_Data))]
		void Test32_MovddupV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovddupV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "F3 0F12 08", 4, Code.Movsldup_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FA 12 10", 4, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 12 10", 5, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 12 10", 4, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 12 10", 5, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "F3 0F16 08", 4, Code.Movshdup_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FA 16 10", 4, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 16 10", 5, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 16 10", 4, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 16 10", 5, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "F2 0F12 08", 4, Code.Movddup_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FB 12 10", 4, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FB 12 10", 5, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5FF 12 10", 4, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FF 12 10", 5, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovddupV_Reg_RegMem_2_Data))]
		void Test32_MovddupV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovddupV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "F3 0F12 CD", 4, Code.Movsldup_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 12 CD", 4, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 12 CD", 4, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "F3 0F16 CD", 4, Code.Movshdup_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 16 CD", 4, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 16 CD", 4, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "F2 0F12 CD", 4, Code.Movddup_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FB 12 CD", 4, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FF 12 CD", 4, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovddupV_Reg_RegMem_1_Data))]
		void Test64_MovddupV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovddupV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "F3 0F12 08", 4, Code.Movsldup_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FA 12 10", 4, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 12 10", 5, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 12 10", 4, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 12 10", 5, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "F3 0F16 08", 4, Code.Movshdup_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FA 16 10", 4, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1FA 16 10", 5, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FE 16 10", 4, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FE 16 10", 5, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "F2 0F12 08", 4, Code.Movddup_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5FB 12 10", 4, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FB 12 10", 5, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5FF 12 10", 4, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FF 12 10", 5, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovddupV_Reg_RegMem_2_Data))]
		void Test64_MovddupV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovddupV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "F3 0F12 CD", 4, Code.Movsldup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F12 CD", 5, Code.Movsldup_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F12 CD", 5, Code.Movsldup_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F12 CD", 5, Code.Movsldup_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FA 12 CD", 4, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57A 12 CD", 4, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17A 12 CD", 5, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417A 12 CD", 5, Code.VEX_Vmovsldup_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FE 12 CD", 4, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57E 12 CD", 4, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17E 12 CD", 5, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417E 12 CD", 5, Code.VEX_Vmovsldup_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "F3 0F16 CD", 4, Code.Movshdup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F16 CD", 5, Code.Movshdup_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F16 CD", 5, Code.Movshdup_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F16 CD", 5, Code.Movshdup_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FA 16 CD", 4, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57A 16 CD", 4, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17A 16 CD", 5, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417A 16 CD", 5, Code.VEX_Vmovshdup_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FE 16 CD", 4, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57E 16 CD", 4, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17E 16 CD", 5, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417E 16 CD", 5, Code.VEX_Vmovshdup_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "F2 0F12 CD", 4, Code.Movddup_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0F12 CD", 5, Code.Movddup_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0F12 CD", 5, Code.Movddup_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0F12 CD", 5, Code.Movddup_xmm_xmmm64, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FB 12 CD", 4, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57B 12 CD", 4, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17B 12 CD", 5, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417B 12 CD", 5, Code.VEX_Vmovddup_xmm_xmmm64, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FF 12 CD", 4, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57F 12 CD", 4, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17F 12 CD", 5, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417F 12 CD", 5, Code.VEX_Vmovddup_ymm_ymmm256, Register.YMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovddupV_Reg_RegMem_EVEX_1_Data))]
		void Test16_MovddupV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test16_MovddupV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 12 50 01", 7, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E8B 12 50 01", 7, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17E28 12 50 01", 7, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EAB 12 50 01", 7, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17E48 12 50 01", 7, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17ECB 12 50 01", 7, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F17E08 16 50 01", 7, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E8B 16 50 01", 7, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17E28 16 50 01", 7, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EAB 16 50 01", 7, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17E48 16 50 01", 7, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17ECB 16 50 01", 7, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FF08 12 50 01", 7, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF8B 12 50 01", 7, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.K3, MemorySize.Float64, 8, true };

				yield return new object[] { "62 F1FF28 12 50 01", 7, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FFAB 12 50 01", 7, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FF48 12 50 01", 7, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FFCB 12 50 01", 7, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovddupV_Reg_RegMem_EVEX_2_Data))]
		void Test16_MovddupV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovddupV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17E08 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFCB 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovddupV_Reg_RegMem_EVEX_1_Data))]
		void Test32_MovddupV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test32_MovddupV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 12 50 01", 7, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E8B 12 50 01", 7, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17E28 12 50 01", 7, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EAB 12 50 01", 7, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17E48 12 50 01", 7, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17ECB 12 50 01", 7, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F17E08 16 50 01", 7, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E8B 16 50 01", 7, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17E28 16 50 01", 7, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EAB 16 50 01", 7, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17E48 16 50 01", 7, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17ECB 16 50 01", 7, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FF08 12 50 01", 7, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF8B 12 50 01", 7, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.K3, MemorySize.Float64, 8, true };

				yield return new object[] { "62 F1FF28 12 50 01", 7, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FFAB 12 50 01", 7, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FF48 12 50 01", 7, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FFCB 12 50 01", 7, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovddupV_Reg_RegMem_EVEX_2_Data))]
		void Test32_MovddupV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovddupV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17E08 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFCB 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovddupV_Reg_RegMem_EVEX_1_Data))]
		void Test64_MovddupV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
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
		public static IEnumerable<object[]> Test64_MovddupV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17E08 12 50 01", 7, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E8B 12 50 01", 7, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17E28 12 50 01", 7, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EAB 12 50 01", 7, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17E48 12 50 01", 7, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17ECB 12 50 01", 7, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F17E08 16 50 01", 7, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17E8B 16 50 01", 7, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17E28 16 50 01", 7, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17EAB 16 50 01", 7, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17E48 16 50 01", 7, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17ECB 16 50 01", 7, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FF08 12 50 01", 7, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F1FF8B 12 50 01", 7, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.K3, MemorySize.Float64, 8, true };

				yield return new object[] { "62 F1FF28 12 50 01", 7, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FFAB 12 50 01", 7, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FF48 12 50 01", 7, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FFCB 12 50 01", 7, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovddupV_Reg_RegMem_EVEX_2_Data))]
		void Test64_MovddupV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovddupV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17E08 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317E8B 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17E8B 12 D3", 6, Code.EVEX_Vmovsldup_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317EAB 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17EAB 12 D3", 6, Code.EVEX_Vmovsldup_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317ECB 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17ECB 12 D3", 6, Code.EVEX_Vmovsldup_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317E8B 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17E8B 16 D3", 6, Code.EVEX_Vmovshdup_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317EAB 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17EAB 16 D3", 6, Code.EVEX_Vmovshdup_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317ECB 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17ECB 16 D3", 6, Code.EVEX_Vmovshdup_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FF8B 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FF8B 12 D3", 6, Code.EVEX_Vmovddup_xmm_k1z_xmmm64, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FFAB 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FFAB 12 D3", 6, Code.EVEX_Vmovddup_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FFCB 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FFCB 12 D3", 6, Code.EVEX_Vmovddup_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_VX_WX_1_Data))]
		void Test16_MovLHV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovLHV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F12 08", 3, Code.Movlps_xmm_m64, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F12 08", 4, Code.Movlpd_xmm_m64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F16 08", 3, Code.Movhps_xmm_m64, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F16 08", 4, Code.Movhpd_xmm_m64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_VX_WX_2_Data))]
		void Test16_MovLHV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovLHV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F12 CD", 3, Code.Movhlps_xmm_xmm, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F16 CD", 3, Code.Movlhps_xmm_xmm, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_VX_WX_1_Data))]
		void Test32_MovLHV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovLHV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F12 08", 3, Code.Movlps_xmm_m64, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F12 08", 4, Code.Movlpd_xmm_m64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F16 08", 3, Code.Movhps_xmm_m64, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F16 08", 4, Code.Movhpd_xmm_m64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_VX_WX_2_Data))]
		void Test32_MovLHV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovLHV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F12 CD", 3, Code.Movhlps_xmm_xmm, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F16 CD", 3, Code.Movlhps_xmm_xmm, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_VX_WX_1_Data))]
		void Test64_MovLHV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovLHV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F12 08", 3, Code.Movlps_xmm_m64, Register.XMM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "44 0F12 08", 4, Code.Movlps_xmm_m64, Register.XMM9, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F12 08", 4, Code.Movlpd_xmm_m64, Register.XMM1, MemorySize.Float64 };
				yield return new object[] { "66 44 0F12 08", 5, Code.Movlpd_xmm_m64, Register.XMM9, MemorySize.Float64 };

				yield return new object[] { "0F16 08", 3, Code.Movhps_xmm_m64, Register.XMM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "44 0F16 08", 4, Code.Movhps_xmm_m64, Register.XMM9, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F16 08", 4, Code.Movhpd_xmm_m64, Register.XMM1, MemorySize.Float64 };
				yield return new object[] { "66 44 0F16 08", 5, Code.Movhpd_xmm_m64, Register.XMM9, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_VX_WX_2_Data))]
		void Test64_MovLHV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovLHV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F12 CD", 3, Code.Movhlps_xmm_xmm, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F12 CD", 4, Code.Movhlps_xmm_xmm, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F12 CD", 4, Code.Movhlps_xmm_xmm, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F12 CD", 4, Code.Movhlps_xmm_xmm, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F16 CD", 3, Code.Movlhps_xmm_xmm, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F16 CD", 4, Code.Movlhps_xmm_xmm, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F16 CD", 4, Code.Movlhps_xmm_xmm, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F16 CD", 4, Code.Movlhps_xmm_xmm, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_VX_HX_WX_1_Data))]
		void Test16_MovLHV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovLHV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 12 10", 4, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1C8 12 10", 5, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5C9 12 10", 4, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1C9 12 10", 5, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };

				yield return new object[] { "C5C8 16 10", 4, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1C8 16 10", 5, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5C9 16 10", 4, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1C9 16 10", 5, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_VX_HX_WX_2_Data))]
		void Test16_MovLHV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_MovLHV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 12 D3", 4, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E1C8 12 D3", 5, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5C8 16 D3", 4, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E1C8 16 D3", 5, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_VX_HX_WX_1_Data))]
		void Test32_MovLHV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovLHV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 12 10", 4, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1C8 12 10", 5, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5C9 12 10", 4, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1C9 12 10", 5, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };

				yield return new object[] { "C5C8 16 10", 4, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1C8 16 10", 5, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5C9 16 10", 4, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E1C9 16 10", 5, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_VX_HX_WX_2_Data))]
		void Test32_MovLHV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_MovLHV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 12 D3", 4, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E1C8 12 D3", 5, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5C8 16 D3", 4, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E1C8 16 D3", 5, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_VX_HX_WX_1_Data))]
		void Test64_MovLHV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovLHV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 12 10", 4, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C548 12 10", 4, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM10, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C588 12 10", 4, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM14, MemorySize.Packed64_Float32 };
				yield return new object[] { "C508 12 10", 4, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM10, Register.XMM14, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1C8 12 10", 5, Code.VEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5C9 12 10", 4, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C549 12 10", 4, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM10, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C589 12 10", 4, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM14, MemorySize.Float64 };
				yield return new object[] { "C509 12 10", 4, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM10, Register.XMM14, MemorySize.Float64 };
				yield return new object[] { "C4E1C9 12 10", 5, Code.VEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };

				yield return new object[] { "C5C8 16 10", 4, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C548 16 10", 4, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM10, Register.XMM6, MemorySize.Packed64_Float32 };
				yield return new object[] { "C588 16 10", 4, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM14, MemorySize.Packed64_Float32 };
				yield return new object[] { "C508 16 10", 4, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM10, Register.XMM14, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1C8 16 10", 5, Code.VEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5C9 16 10", 4, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C549 16 10", 4, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM10, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C589 16 10", 4, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM14, MemorySize.Float64 };
				yield return new object[] { "C509 16 10", 4, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM10, Register.XMM14, MemorySize.Float64 };
				yield return new object[] { "C4E1C9 16 10", 5, Code.VEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_VX_HX_WX_2_Data))]
		void Test64_MovLHV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_MovLHV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 12 D3", 4, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C548 12 D3", 4, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C588 12 D3", 4, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C148 12 D3", 5, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4E1C8 12 D3", 5, Code.VEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C5C8 16 D3", 4, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C548 16 D3", 4, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C588 16 D3", 4, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C148 16 D3", 5, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4E1C8 16 D3", 5, Code.VEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_VX_HX_WX_EVEX_1_Data))]
		void Test16_MovLHV_VX_HX_WX_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovLHV_VX_HX_WX_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14C08 12 50 01", 7, Code.EVEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1CD08 12 50 01", 7, Code.EVEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64, 8 };

				yield return new object[] { "62 F14C08 16 50 01", 7, Code.EVEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1CD08 16 50 01", 7, Code.EVEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_VX_HX_WX_EVEX_2_Data))]
		void Test16_MovLHV_VX_HX_WX_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovLHV_VX_HX_WX_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14C08 12 D3", 6, Code.EVEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "62 F14C08 16 D3", 6, Code.EVEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_VX_HX_WX_EVEX_1_Data))]
		void Test32_MovLHV_VX_HX_WX_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovLHV_VX_HX_WX_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14C08 12 50 01", 7, Code.EVEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1CD08 12 50 01", 7, Code.EVEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64, 8 };

				yield return new object[] { "62 F14C08 16 50 01", 7, Code.EVEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1CD08 16 50 01", 7, Code.EVEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_VX_HX_WX_EVEX_2_Data))]
		void Test32_MovLHV_VX_HX_WX_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovLHV_VX_HX_WX_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14C08 12 D3", 6, Code.EVEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "62 F14C08 16 D3", 6, Code.EVEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_VX_HX_WX_EVEX_1_Data))]
		void Test64_MovLHV_VX_HX_WX_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovLHV_VX_HX_WX_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14C08 12 50 01", 7, Code.EVEX_Vmovlps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 E10C08 12 50 01", 7, Code.EVEX_Vmovlps_xmm_xmm_m64, Register.XMM18, Register.XMM14, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 714C00 12 50 01", 7, Code.EVEX_Vmovlps_xmm_xmm_m64, Register.XMM10, Register.XMM22, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1CD08 12 50 01", 7, Code.EVEX_Vmovlpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64, 8 };
				yield return new object[] { "62 E18D08 12 50 01", 7, Code.EVEX_Vmovlpd_xmm_xmm_m64, Register.XMM18, Register.XMM14, MemorySize.Float64, 8 };
				yield return new object[] { "62 71CD00 12 50 01", 7, Code.EVEX_Vmovlpd_xmm_xmm_m64, Register.XMM10, Register.XMM22, MemorySize.Float64, 8 };

				yield return new object[] { "62 F14C08 16 50 01", 7, Code.EVEX_Vmovhps_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 E10C08 16 50 01", 7, Code.EVEX_Vmovhps_xmm_xmm_m64, Register.XMM18, Register.XMM14, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 714C00 16 50 01", 7, Code.EVEX_Vmovhps_xmm_xmm_m64, Register.XMM10, Register.XMM22, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1CD08 16 50 01", 7, Code.EVEX_Vmovhpd_xmm_xmm_m64, Register.XMM2, Register.XMM6, MemorySize.Float64, 8 };
				yield return new object[] { "62 E18D08 16 50 01", 7, Code.EVEX_Vmovhpd_xmm_xmm_m64, Register.XMM18, Register.XMM14, MemorySize.Float64, 8 };
				yield return new object[] { "62 71CD00 16 50 01", 7, Code.EVEX_Vmovhpd_xmm_xmm_m64, Register.XMM10, Register.XMM22, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_VX_HX_WX_EVEX_2_Data))]
		void Test64_MovLHV_VX_HX_WX_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovLHV_VX_HX_WX_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14C08 12 D3", 6, Code.EVEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "62 E10C08 12 D3", 6, Code.EVEX_Vmovhlps_xmm_xmm_xmm, Register.XMM18, Register.XMM14, Register.XMM3 };
				yield return new object[] { "62 114C00 12 D3", 6, Code.EVEX_Vmovhlps_xmm_xmm_xmm, Register.XMM10, Register.XMM22, Register.XMM27 };
				yield return new object[] { "62 B14C08 12 D3", 6, Code.EVEX_Vmovhlps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM19 };

				yield return new object[] { "62 F14C08 16 D3", 6, Code.EVEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "62 E10C08 16 D3", 6, Code.EVEX_Vmovlhps_xmm_xmm_xmm, Register.XMM18, Register.XMM14, Register.XMM3 };
				yield return new object[] { "62 114C00 16 D3", 6, Code.EVEX_Vmovlhps_xmm_xmm_xmm, Register.XMM10, Register.XMM22, Register.XMM27 };
				yield return new object[] { "62 B14C08 16 D3", 6, Code.EVEX_Vmovlhps_xmm_xmm_xmm, Register.XMM2, Register.XMM6, Register.XMM19 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_M_VX_1_Data))]
		void Test16_MovLHV_M_VX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_MovLHV_M_VX_1_Data {
			get {
				yield return new object[] { "0F13 08", 3, Code.Movlps_m64_xmm, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F13 08", 4, Code.Movlpd_m64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F17 08", 3, Code.Movhps_m64_xmm, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F17 08", 4, Code.Movhpd_m64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5F8 13 10", 4, Code.VEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 13 10", 5, Code.VEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5F9 13 10", 4, Code.VEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 13 10", 5, Code.VEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5F8 17 10", 4, Code.VEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 17 10", 5, Code.VEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5F9 17 10", 4, Code.VEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 17 10", 5, Code.VEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_M_VX_1_Data))]
		void Test32_MovLHV_M_VX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_MovLHV_M_VX_1_Data {
			get {
				yield return new object[] { "0F13 08", 3, Code.Movlps_m64_xmm, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F13 08", 4, Code.Movlpd_m64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F17 08", 3, Code.Movhps_m64_xmm, Register.XMM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F17 08", 4, Code.Movhpd_m64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5F8 13 10", 4, Code.VEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 13 10", 5, Code.VEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5F9 13 10", 4, Code.VEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 13 10", 5, Code.VEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5F8 17 10", 4, Code.VEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 17 10", 5, Code.VEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5F9 17 10", 4, Code.VEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 17 10", 5, Code.VEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_M_VX_1_Data))]
		void Test64_MovLHV_M_VX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_MovLHV_M_VX_1_Data {
			get {
				yield return new object[] { "0F13 08", 3, Code.Movlps_m64_xmm, Register.XMM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "44 0F13 08", 4, Code.Movlps_m64_xmm, Register.XMM9, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F13 08", 4, Code.Movlpd_m64_xmm, Register.XMM1, MemorySize.Float64 };
				yield return new object[] { "66 44 0F13 08", 5, Code.Movlpd_m64_xmm, Register.XMM9, MemorySize.Float64 };

				yield return new object[] { "0F17 08", 3, Code.Movhps_m64_xmm, Register.XMM1, MemorySize.Packed64_Float32 };
				yield return new object[] { "44 0F17 08", 4, Code.Movhps_m64_xmm, Register.XMM9, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F17 08", 4, Code.Movhpd_m64_xmm, Register.XMM1, MemorySize.Float64 };
				yield return new object[] { "66 44 0F17 08", 5, Code.Movhpd_m64_xmm, Register.XMM9, MemorySize.Float64 };

				yield return new object[] { "C5F8 13 10", 4, Code.VEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C578 13 10", 4, Code.VEX_Vmovlps_m64_xmm, Register.XMM10, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 13 10", 5, Code.VEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5F9 13 10", 4, Code.VEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C579 13 10", 4, Code.VEX_Vmovlpd_m64_xmm, Register.XMM10, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 13 10", 5, Code.VEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5F8 17 10", 4, Code.VEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };
				yield return new object[] { "C578 17 10", 4, Code.VEX_Vmovhps_m64_xmm, Register.XMM10, MemorySize.Packed64_Float32 };
				yield return new object[] { "C4E1F8 17 10", 5, Code.VEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32 };

				yield return new object[] { "C5F9 17 10", 4, Code.VEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C579 17 10", 4, Code.VEX_Vmovhpd_m64_xmm, Register.XMM10, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 17 10", 5, Code.VEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovLHV_M_VX_EVEX_1_Data))]
		void Test16_MovLHV_M_VX_EVEX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovLHV_M_VX_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 13 50 01", 7, Code.EVEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1FD08 13 50 01", 7, Code.EVEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17C08 17 50 01", 7, Code.EVEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1FD08 17 50 01", 7, Code.EVEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovLHV_M_VX_EVEX_1_Data))]
		void Test32_MovLHV_M_VX_EVEX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovLHV_M_VX_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 13 50 01", 7, Code.EVEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1FD08 13 50 01", 7, Code.EVEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17C08 17 50 01", 7, Code.EVEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1FD08 17 50 01", 7, Code.EVEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovLHV_M_VX_EVEX_1_Data))]
		void Test64_MovLHV_M_VX_EVEX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint displ) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovLHV_M_VX_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 13 50 01", 7, Code.EVEX_Vmovlps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 717C08 13 50 01", 7, Code.EVEX_Vmovlps_m64_xmm, Register.XMM10, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 E17C08 13 50 01", 7, Code.EVEX_Vmovlps_m64_xmm, Register.XMM18, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1FD08 13 50 01", 7, Code.EVEX_Vmovlpd_m64_xmm, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 71FD08 13 50 01", 7, Code.EVEX_Vmovlpd_m64_xmm, Register.XMM10, MemorySize.Float64, 8 };
				yield return new object[] { "62 E1FD08 13 50 01", 7, Code.EVEX_Vmovlpd_m64_xmm, Register.XMM18, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17C08 17 50 01", 7, Code.EVEX_Vmovhps_m64_xmm, Register.XMM2, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 717C08 17 50 01", 7, Code.EVEX_Vmovhps_m64_xmm, Register.XMM10, MemorySize.Packed64_Float32, 8 };
				yield return new object[] { "62 E17C08 17 50 01", 7, Code.EVEX_Vmovhps_m64_xmm, Register.XMM18, MemorySize.Packed64_Float32, 8 };

				yield return new object[] { "62 F1FD08 17 50 01", 7, Code.EVEX_Vmovhpd_m64_xmm, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 71FD08 17 50 01", 7, Code.EVEX_Vmovhpd_m64_xmm, Register.XMM10, MemorySize.Float64, 8 };
				yield return new object[] { "62 E1FD08 17 50 01", 7, Code.EVEX_Vmovhpd_m64_xmm, Register.XMM18, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_UnpcklV_VX_WX_1_Data))]
		void Test16_UnpcklV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_UnpcklV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F14 08", 3, Code.Unpcklps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F14 08", 4, Code.Unpcklpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_UnpcklV_VX_WX_2_Data))]
		void Test16_UnpcklV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_UnpcklV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F14 CD", 3, Code.Unpcklps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F14 CD", 4, Code.Unpcklpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_UnpcklV_VX_WX_1_Data))]
		void Test32_UnpcklV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_UnpcklV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F14 08", 3, Code.Unpcklps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F14 08", 4, Code.Unpcklpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_UnpcklV_VX_WX_2_Data))]
		void Test32_UnpcklV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_UnpcklV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F14 CD", 3, Code.Unpcklps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F14 CD", 4, Code.Unpcklpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_UnpcklV_VX_WX_1_Data))]
		void Test64_UnpcklV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_UnpcklV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F14 08", 3, Code.Unpcklps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F14 08", 4, Code.Unpcklpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_UnpcklV_VX_WX_2_Data))]
		void Test64_UnpcklV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_UnpcklV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F14 CD", 3, Code.Unpcklps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F14 CD", 4, Code.Unpcklps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F14 CD", 4, Code.Unpcklps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F14 CD", 4, Code.Unpcklps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F14 CD", 4, Code.Unpcklpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F14 CD", 5, Code.Unpcklpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F14 CD", 5, Code.Unpcklpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F14 CD", 5, Code.Unpcklpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpcklV_VX_HX_WX_1_Data))]
		void Test16_VunpcklV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VunpcklV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 14 10", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 14 10", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 14 10", 5, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 14 10", 5, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 14 10", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 14 10", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 14 10", 5, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 14 10", 5, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpcklV_VX_HX_WX_2_Data))]
		void Test16_VunpcklV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VunpcklV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 14 D3", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 14 D3", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 14 D3", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 14 D3", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpcklV_VX_HX_WX_1_Data))]
		void Test32_VunpcklV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VunpcklV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 14 10", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 14 10", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 14 10", 5, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 14 10", 5, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 14 10", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 14 10", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 14 10", 5, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 14 10", 5, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpcklV_VX_HX_WX_2_Data))]
		void Test32_VunpcklV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VunpcklV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 14 D3", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 14 D3", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 14 D3", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 14 D3", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpcklV_VX_HX_WX_1_Data))]
		void Test64_VunpcklV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VunpcklV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 14 10", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 14 10", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 14 10", 5, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 14 10", 5, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 14 10", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 14 10", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 14 10", 5, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 14 10", 5, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpcklV_VX_HX_WX_2_Data))]
		void Test64_VunpcklV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VunpcklV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 14 D3", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 14 D3", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 14 D3", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 14 D3", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 14 D3", 4, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 14 D3", 4, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 14 D3", 5, Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 14 D3", 5, Code.VEX_Vunpcklps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 14 D3", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 14 D3", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 14 D3", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 14 D3", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 14 D3", 4, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 14 D3", 4, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 14 D3", 5, Code.VEX_Vunpcklpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 14 D3", 5, Code.VEX_Vunpcklpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpcklV_VX_k1_HX_WX_1_Data))]
		void Test16_VunpcklV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VunpcklV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpcklV_VX_k1_HX_WX_2_Data))]
		void Test16_VunpcklV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VunpcklV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpcklV_VX_k1_HX_WX_1_Data))]
		void Test32_VunpcklV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VunpcklV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpcklV_VX_k1_HX_WX_2_Data))]
		void Test32_VunpcklV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VunpcklV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpcklV_VX_k1_HX_WX_1_Data))]
		void Test64_VunpcklV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VunpcklV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 14 50 01", 7, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 14 50 01", 7, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 14 50 01", 7, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 14 50 01", 7, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 14 50 01", 7, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 14 50 01", 7, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpcklV_VX_k1_HX_WX_2_Data))]
		void Test64_VunpcklV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VunpcklV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 E10C0B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114C03 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14C0B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14C8B 14 D3", 6, Code.EVEX_Vunpcklps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 E10C2B 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114C23 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14C2B 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14CAB 14 D3", 6, Code.EVEX_Vunpcklps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 E10C4B 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114C43 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14C4B 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14CCB 14 D3", 6, Code.EVEX_Vunpcklps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 14 D3", 6, Code.EVEX_Vunpcklpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 14 D3", 6, Code.EVEX_Vunpcklpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 14 D3", 6, Code.EVEX_Vunpcklpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_UnpckhV_VX_WX_1_Data))]
		void Test16_UnpckhV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_UnpckhV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F15 08", 3, Code.Unpckhps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F15 08", 4, Code.Unpckhpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_UnpckhV_VX_WX_2_Data))]
		void Test16_UnpckhV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_UnpckhV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F15 CD", 3, Code.Unpckhps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F15 CD", 4, Code.Unpckhpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_UnpckhV_VX_WX_1_Data))]
		void Test32_UnpckhV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_UnpckhV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F15 08", 3, Code.Unpckhps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F15 08", 4, Code.Unpckhpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_UnpckhV_VX_WX_2_Data))]
		void Test32_UnpckhV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_UnpckhV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F15 CD", 3, Code.Unpckhps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F15 CD", 4, Code.Unpckhpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_UnpckhV_VX_WX_1_Data))]
		void Test64_UnpckhV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_UnpckhV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F15 08", 3, Code.Unpckhps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F15 08", 4, Code.Unpckhpd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_UnpckhV_VX_WX_2_Data))]
		void Test64_UnpckhV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_UnpckhV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F15 CD", 3, Code.Unpckhps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F15 CD", 4, Code.Unpckhps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F15 CD", 4, Code.Unpckhps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F15 CD", 4, Code.Unpckhps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F15 CD", 4, Code.Unpckhpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F15 CD", 5, Code.Unpckhpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F15 CD", 5, Code.Unpckhpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F15 CD", 5, Code.Unpckhpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpckhV_VX_HX_WX_1_Data))]
		void Test16_VunpckhV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VunpckhV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 15 10", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 15 10", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 15 10", 5, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 15 10", 5, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 15 10", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 15 10", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 15 10", 5, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 15 10", 5, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpckhV_VX_HX_WX_2_Data))]
		void Test16_VunpckhV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VunpckhV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 15 D3", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 15 D3", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 15 D3", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 15 D3", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpckhV_VX_HX_WX_1_Data))]
		void Test32_VunpckhV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VunpckhV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 15 10", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 15 10", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 15 10", 5, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 15 10", 5, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 15 10", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 15 10", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 15 10", 5, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 15 10", 5, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpckhV_VX_HX_WX_2_Data))]
		void Test32_VunpckhV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VunpckhV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 15 D3", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 15 D3", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C5C9 15 D3", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 15 D3", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpckhV_VX_HX_WX_1_Data))]
		void Test64_VunpckhV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VunpckhV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C8 15 10", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C5CC 15 10", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1C8 15 10", 5, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1CC 15 10", 5, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5C9 15 10", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C5CD 15 10", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1C9 15 10", 5, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1CD 15 10", 5, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpckhV_VX_HX_WX_2_Data))]
		void Test64_VunpckhV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VunpckhV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C8 15 D3", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CC 15 D3", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C548 15 D3", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54C 15 D3", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C588 15 D3", 4, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58C 15 D3", 4, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C148 15 D3", 5, Code.VEX_Vunpckhps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14C 15 D3", 5, Code.VEX_Vunpckhps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C5C9 15 D3", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 15 D3", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 15 D3", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 15 D3", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 15 D3", 4, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 15 D3", 4, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 15 D3", 5, Code.VEX_Vunpckhpd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 15 D3", 5, Code.VEX_Vunpckhpd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpckhV_VX_k1_HX_WX_1_Data))]
		void Test16_VunpckhV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VunpckhV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VunpckhV_VX_k1_HX_WX_2_Data))]
		void Test16_VunpckhV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VunpckhV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpckhV_VX_k1_HX_WX_1_Data))]
		void Test32_VunpckhV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VunpckhV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VunpckhV_VX_k1_HX_WX_2_Data))]
		void Test32_VunpckhV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VunpckhV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 F14C8B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 F14CAB 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 F14CCB 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpckhV_VX_k1_HX_WX_1_Data))]
		void Test64_VunpckhV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VunpckhV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14C0B 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F14C9D 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F14C08 15 50 01", 7, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F14C2B 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F14CBD 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F14C28 15 50 01", 7, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F14C4B 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F14CDD 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F14C48 15 50 01", 7, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1CD0B 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1CD9D 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F1CD08 15 50 01", 7, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1CD2B 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1CDBD 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F1CD28 15 50 01", 7, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1CD4B 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1CDDD 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F1CD48 15 50 01", 7, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VunpckhV_VX_k1_HX_WX_2_Data))]
		void Test64_VunpckhV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VunpckhV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14C0B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 E10C0B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114C03 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14C0B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14C8B 15 D3", 6, Code.EVEX_Vunpckhps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };

				yield return new object[] { "62 F14C2B 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 E10C2B 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114C23 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14C2B 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14CAB 15 D3", 6, Code.EVEX_Vunpckhps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };

				yield return new object[] { "62 F14C4B 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 E10C4B 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114C43 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14C4B 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14CCB 15 D3", 6, Code.EVEX_Vunpckhps_zmm_k1z_zmm_zmmm512b32, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };

				yield return new object[] { "62 F1CD8B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 15 D3", 6, Code.EVEX_Vunpckhpd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 15 D3", 6, Code.EVEX_Vunpckhpd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 15 D3", 6, Code.EVEX_Vunpckhpd_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
