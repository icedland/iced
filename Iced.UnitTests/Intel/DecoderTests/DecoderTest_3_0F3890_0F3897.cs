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
	public sealed class DecoderTest_3_0F3890_0F3897 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpgatherddV_VX_HX_WX_1_Data))]
		void Test16_VpgatherddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VpgatherddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "67 C4E249 90 54 A1 01", 8, Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.ECX, Register.XMM4, false };
				yield return new object[] { "67 C4E24D 90 54 A1 01", 8, Code.VEX_Vpgatherdd_ymm_vm32y_ymm, Register.YMM2, Register.YMM6, MemorySize.Int32, Register.ECX, Register.YMM4, false };

				yield return new object[] { "67 C4E2C9 90 54 A1 01", 8, Code.VEX_Vpgatherdq_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int64, Register.ECX, Register.XMM4, false };
				yield return new object[] { "67 C4E2CD 90 54 A1 01", 8, Code.VEX_Vpgatherdq_ymm_vm32x_ymm, Register.YMM2, Register.YMM6, MemorySize.Int64, Register.ECX, Register.XMM4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpgatherddV_VX_HX_WX_1_Data))]
		void Test32_VpgatherddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VpgatherddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 90 54 A1 01", 7, Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.ECX, Register.XMM4, false };
				yield return new object[] { "C4E24D 90 54 A1 01", 7, Code.VEX_Vpgatherdd_ymm_vm32y_ymm, Register.YMM2, Register.YMM6, MemorySize.Int32, Register.ECX, Register.YMM4, false };

				yield return new object[] { "C4E2C9 90 54 A1 01", 7, Code.VEX_Vpgatherdq_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int64, Register.ECX, Register.XMM4, false };
				yield return new object[] { "C4E2CD 90 54 A1 01", 7, Code.VEX_Vpgatherdq_ymm_vm32x_ymm, Register.YMM2, Register.YMM6, MemorySize.Int64, Register.ECX, Register.XMM4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpgatherddV_VX_HX_WX_1_Data))]
		void Test64_VpgatherddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VpgatherddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 90 54 A1 01", 7, Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C46249 90 54 A1 01", 7, Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM10, Register.XMM6, MemorySize.Int32, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C4E209 90 54 A1 01", 7, Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM2, Register.XMM14, MemorySize.Int32, Register.RCX, Register.XMM4, false };

				yield return new object[] { "C4E24D 90 54 A1 01", 7, Code.VEX_Vpgatherdd_ymm_vm32y_ymm, Register.YMM2, Register.YMM6, MemorySize.Int32, Register.RCX, Register.YMM4, false };
				yield return new object[] { "C4624D 90 54 A1 01", 7, Code.VEX_Vpgatherdd_ymm_vm32y_ymm, Register.YMM10, Register.YMM6, MemorySize.Int32, Register.RCX, Register.YMM4, false };
				yield return new object[] { "C4E20D 90 54 A1 01", 7, Code.VEX_Vpgatherdd_ymm_vm32y_ymm, Register.YMM2, Register.YMM14, MemorySize.Int32, Register.RCX, Register.YMM4, false };

				yield return new object[] { "C4E2C9 90 54 A1 01", 7, Code.VEX_Vpgatherdq_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C462C9 90 54 A1 01", 7, Code.VEX_Vpgatherdq_xmm_vm32x_xmm, Register.XMM10, Register.XMM6, MemorySize.Int64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C4E289 90 54 A1 01", 7, Code.VEX_Vpgatherdq_xmm_vm32x_xmm, Register.XMM2, Register.XMM14, MemorySize.Int64, Register.RCX, Register.XMM4, false };

				yield return new object[] { "C4E2CD 90 54 A1 01", 7, Code.VEX_Vpgatherdq_ymm_vm32x_ymm, Register.YMM2, Register.YMM6, MemorySize.Int64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C462CD 90 54 A1 01", 7, Code.VEX_Vpgatherdq_ymm_vm32x_ymm, Register.YMM10, Register.YMM6, MemorySize.Int64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C4E28D 90 54 A1 01", 7, Code.VEX_Vpgatherdq_ymm_vm32x_ymm, Register.YMM2, Register.YMM14, MemorySize.Int64, Register.RCX, Register.XMM4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpgatherddV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpgatherddV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VpgatherddV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "67 62 F27D0B 90 54 A1 01", 9, Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.XMM4, 4, false };

				yield return new object[] { "67 62 F27D2B 90 54 A1 01", 9, Code.EVEX_Vpgatherdd_ymm_k1_vm32y, Register.YMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.YMM4, 4, false };

				yield return new object[] { "67 62 F27D4B 90 54 A1 01", 9, Code.EVEX_Vpgatherdd_zmm_k1_vm32z, Register.ZMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "67 62 F2FD0B 90 54 A1 01", 9, Code.EVEX_Vpgatherdq_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "67 62 F2FD2B 90 54 A1 01", 9, Code.EVEX_Vpgatherdq_ymm_k1_vm32x, Register.YMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "67 62 F2FD4B 90 54 A1 01", 9, Code.EVEX_Vpgatherdq_zmm_k1_vm32y, Register.ZMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.YMM4, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpgatherddV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpgatherddV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VpgatherddV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 90 54 A1 01", 8, Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.XMM4, 4, false };

				yield return new object[] { "62 F27D2B 90 54 A1 01", 8, Code.EVEX_Vpgatherdd_ymm_k1_vm32y, Register.YMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.YMM4, 4, false };

				yield return new object[] { "62 F27D4B 90 54 A1 01", 8, Code.EVEX_Vpgatherdd_zmm_k1_vm32z, Register.ZMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "62 F2FD0B 90 54 A1 01", 8, Code.EVEX_Vpgatherdq_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "62 F2FD2B 90 54 A1 01", 8, Code.EVEX_Vpgatherdq_ymm_k1_vm32x, Register.YMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "62 F2FD4B 90 54 A1 01", 8, Code.EVEX_Vpgatherdq_zmm_k1_vm32y, Register.ZMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.YMM4, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpgatherddV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpgatherddV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VpgatherddV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 90 54 A1 01", 8, Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Int32, Register.RCX, Register.XMM4, 4, false };
				yield return new object[] { "62 A27D03 90 5C A9 01", 8, Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM19, Register.K3, MemorySize.Int32, Register.RCX, Register.XMM29, 4, false };
				yield return new object[] { "62 427D03 90 5C 8F 01", 8, Code.EVEX_Vpgatherdd_xmm_k1_vm32x, Register.XMM27, Register.K3, MemorySize.Int32, Register.R15, Register.XMM17, 4, false };

				yield return new object[] { "62 F27D2B 90 54 A1 01", 8, Code.EVEX_Vpgatherdd_ymm_k1_vm32y, Register.YMM2, Register.K3, MemorySize.Int32, Register.RCX, Register.YMM4, 4, false };
				yield return new object[] { "62 A27D23 90 5C A9 01", 8, Code.EVEX_Vpgatherdd_ymm_k1_vm32y, Register.YMM19, Register.K3, MemorySize.Int32, Register.RCX, Register.YMM29, 4, false };
				yield return new object[] { "62 427D23 90 5C 8F 01", 8, Code.EVEX_Vpgatherdd_ymm_k1_vm32y, Register.YMM27, Register.K3, MemorySize.Int32, Register.R15, Register.YMM17, 4, false };

				yield return new object[] { "62 F27D4B 90 54 A1 01", 8, Code.EVEX_Vpgatherdd_zmm_k1_vm32z, Register.ZMM2, Register.K3, MemorySize.Int32, Register.RCX, Register.ZMM4, 4, false };
				yield return new object[] { "62 A27D43 90 5C A9 01", 8, Code.EVEX_Vpgatherdd_zmm_k1_vm32z, Register.ZMM19, Register.K3, MemorySize.Int32, Register.RCX, Register.ZMM29, 4, false };
				yield return new object[] { "62 427D43 90 5C 8F 01", 8, Code.EVEX_Vpgatherdd_zmm_k1_vm32z, Register.ZMM27, Register.K3, MemorySize.Int32, Register.R15, Register.ZMM17, 4, false };

				yield return new object[] { "62 F2FD0B 90 54 A1 01", 8, Code.EVEX_Vpgatherdq_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Int64, Register.RCX, Register.XMM4, 8, false };
				yield return new object[] { "62 A2FD03 90 5C A9 01", 8, Code.EVEX_Vpgatherdq_xmm_k1_vm32x, Register.XMM19, Register.K3, MemorySize.Int64, Register.RCX, Register.XMM29, 8, false };
				yield return new object[] { "62 42FD03 90 5C 8F 01", 8, Code.EVEX_Vpgatherdq_xmm_k1_vm32x, Register.XMM27, Register.K3, MemorySize.Int64, Register.R15, Register.XMM17, 8, false };

				yield return new object[] { "62 F2FD2B 90 54 A1 01", 8, Code.EVEX_Vpgatherdq_ymm_k1_vm32x, Register.YMM2, Register.K3, MemorySize.Int64, Register.RCX, Register.XMM4, 8, false };
				yield return new object[] { "62 A2FD23 90 5C A9 01", 8, Code.EVEX_Vpgatherdq_ymm_k1_vm32x, Register.YMM19, Register.K3, MemorySize.Int64, Register.RCX, Register.XMM29, 8, false };
				yield return new object[] { "62 42FD23 90 5C 8F 01", 8, Code.EVEX_Vpgatherdq_ymm_k1_vm32x, Register.YMM27, Register.K3, MemorySize.Int64, Register.R15, Register.XMM17, 8, false };

				yield return new object[] { "62 F2FD4B 90 54 A1 01", 8, Code.EVEX_Vpgatherdq_zmm_k1_vm32y, Register.ZMM2, Register.K3, MemorySize.Int64, Register.RCX, Register.YMM4, 8, false };
				yield return new object[] { "62 A2FD43 90 5C A9 01", 8, Code.EVEX_Vpgatherdq_zmm_k1_vm32y, Register.ZMM19, Register.K3, MemorySize.Int64, Register.RCX, Register.YMM29, 8, false };
				yield return new object[] { "62 42FD43 90 5C 8F 01", 8, Code.EVEX_Vpgatherdq_zmm_k1_vm32y, Register.ZMM27, Register.K3, MemorySize.Int64, Register.R15, Register.YMM17, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpgatherqdV_VX_HX_WX_1_Data))]
		void Test16_VpgatherqdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VpgatherqdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "67 C4E249 91 54 A1 01", 8, Code.VEX_Vpgatherqd_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.ECX, Register.XMM4, true };
				yield return new object[] { "67 C4E24D 91 54 A1 01", 8, Code.VEX_Vpgatherqd_xmm_vm64y_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.ECX, Register.YMM4, true };

				yield return new object[] { "67 C4E2C9 91 54 A1 01", 8, Code.VEX_Vpgatherqq_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int64, Register.ECX, Register.XMM4, true };
				yield return new object[] { "67 C4E2CD 91 54 A1 01", 8, Code.VEX_Vpgatherqq_ymm_vm64y_ymm, Register.YMM2, Register.YMM6, MemorySize.Int64, Register.ECX, Register.YMM4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpgatherqdV_VX_HX_WX_1_Data))]
		void Test32_VpgatherqdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VpgatherqdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.ECX, Register.XMM4, true };
				yield return new object[] { "C4E24D 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64y_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.ECX, Register.YMM4, true };

				yield return new object[] { "C4E2C9 91 54 A1 01", 7, Code.VEX_Vpgatherqq_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int64, Register.ECX, Register.XMM4, true };
				yield return new object[] { "C4E2CD 91 54 A1 01", 7, Code.VEX_Vpgatherqq_ymm_vm64y_ymm, Register.YMM2, Register.YMM6, MemorySize.Int64, Register.ECX, Register.YMM4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpgatherqdV_VX_HX_WX_1_Data))]
		void Test64_VpgatherqdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VpgatherqdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C46249 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64x_xmm, Register.XMM10, Register.XMM6, MemorySize.Int32, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C4E209 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64x_xmm, Register.XMM2, Register.XMM14, MemorySize.Int32, Register.RCX, Register.XMM4, true };

				yield return new object[] { "C4E24D 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64y_xmm, Register.XMM2, Register.XMM6, MemorySize.Int32, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C4624D 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64y_xmm, Register.XMM10, Register.XMM6, MemorySize.Int32, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C4E20D 91 54 A1 01", 7, Code.VEX_Vpgatherqd_xmm_vm64y_xmm, Register.XMM2, Register.XMM14, MemorySize.Int32, Register.RCX, Register.YMM4, true };

				yield return new object[] { "C4E2C9 91 54 A1 01", 7, Code.VEX_Vpgatherqq_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Int64, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C462C9 91 54 A1 01", 7, Code.VEX_Vpgatherqq_xmm_vm64x_xmm, Register.XMM10, Register.XMM6, MemorySize.Int64, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C4E289 91 54 A1 01", 7, Code.VEX_Vpgatherqq_xmm_vm64x_xmm, Register.XMM2, Register.XMM14, MemorySize.Int64, Register.RCX, Register.XMM4, true };

				yield return new object[] { "C4E2CD 91 54 A1 01", 7, Code.VEX_Vpgatherqq_ymm_vm64y_ymm, Register.YMM2, Register.YMM6, MemorySize.Int64, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C462CD 91 54 A1 01", 7, Code.VEX_Vpgatherqq_ymm_vm64y_ymm, Register.YMM10, Register.YMM6, MemorySize.Int64, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C4E28D 91 54 A1 01", 7, Code.VEX_Vpgatherqq_ymm_vm64y_ymm, Register.YMM2, Register.YMM14, MemorySize.Int64, Register.RCX, Register.YMM4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpgatherqdV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VpgatherqdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VpgatherqdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "67 62 F27D0B 91 54 A1 01", 9, Code.EVEX_Vpgatherqd_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.XMM4, 4, true };

				yield return new object[] { "67 62 F27D2B 91 54 A1 01", 9, Code.EVEX_Vpgatherqd_xmm_k1_vm64y, Register.XMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.YMM4, 4, true };

				yield return new object[] { "67 62 F27D4B 91 54 A1 01", 9, Code.EVEX_Vpgatherqd_ymm_k1_vm64z, Register.YMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "67 62 F2FD0B 91 54 A1 01", 9, Code.EVEX_Vpgatherqq_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.XMM4, 8, true };

				yield return new object[] { "67 62 F2FD2B 91 54 A1 01", 9, Code.EVEX_Vpgatherqq_ymm_k1_vm64y, Register.YMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.YMM4, 8, true };

				yield return new object[] { "67 62 F2FD4B 91 54 A1 01", 9, Code.EVEX_Vpgatherqq_zmm_k1_vm64z, Register.ZMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.ZMM4, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpgatherqdV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VpgatherqdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VpgatherqdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 91 54 A1 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.XMM4, 4, true };

				yield return new object[] { "62 F27D2B 91 54 A1 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64y, Register.XMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.YMM4, 4, true };

				yield return new object[] { "62 F27D4B 91 54 A1 01", 8, Code.EVEX_Vpgatherqd_ymm_k1_vm64z, Register.YMM2, Register.K3, MemorySize.Int32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "62 F2FD0B 91 54 A1 01", 8, Code.EVEX_Vpgatherqq_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.XMM4, 8, true };

				yield return new object[] { "62 F2FD2B 91 54 A1 01", 8, Code.EVEX_Vpgatherqq_ymm_k1_vm64y, Register.YMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.YMM4, 8, true };

				yield return new object[] { "62 F2FD4B 91 54 A1 01", 8, Code.EVEX_Vpgatherqq_zmm_k1_vm64z, Register.ZMM2, Register.K3, MemorySize.Int64, Register.ECX, Register.ZMM4, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpgatherqdV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VpgatherqdV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VpgatherqdV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 91 54 A1 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Int32, Register.RCX, Register.XMM4, 4, true };
				yield return new object[] { "62 A27D03 91 5C A9 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64x, Register.XMM19, Register.K3, MemorySize.Int32, Register.RCX, Register.XMM29, 4, true };
				yield return new object[] { "62 427D03 91 5C 8F 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64x, Register.XMM27, Register.K3, MemorySize.Int32, Register.R15, Register.XMM17, 4, true };

				yield return new object[] { "62 F27D2B 91 54 A1 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64y, Register.XMM2, Register.K3, MemorySize.Int32, Register.RCX, Register.YMM4, 4, true };
				yield return new object[] { "62 A27D23 91 5C A9 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64y, Register.XMM19, Register.K3, MemorySize.Int32, Register.RCX, Register.YMM29, 4, true };
				yield return new object[] { "62 427D23 91 5C 8F 01", 8, Code.EVEX_Vpgatherqd_xmm_k1_vm64y, Register.XMM27, Register.K3, MemorySize.Int32, Register.R15, Register.YMM17, 4, true };

				yield return new object[] { "62 F27D4B 91 54 A1 01", 8, Code.EVEX_Vpgatherqd_ymm_k1_vm64z, Register.YMM2, Register.K3, MemorySize.Int32, Register.RCX, Register.ZMM4, 4, true };
				yield return new object[] { "62 A27D43 91 5C A9 01", 8, Code.EVEX_Vpgatherqd_ymm_k1_vm64z, Register.YMM19, Register.K3, MemorySize.Int32, Register.RCX, Register.ZMM29, 4, true };
				yield return new object[] { "62 427D43 91 5C 8F 01", 8, Code.EVEX_Vpgatherqd_ymm_k1_vm64z, Register.YMM27, Register.K3, MemorySize.Int32, Register.R15, Register.ZMM17, 4, true };

				yield return new object[] { "62 F2FD0B 91 54 A1 01", 8, Code.EVEX_Vpgatherqq_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Int64, Register.RCX, Register.XMM4, 8, true };
				yield return new object[] { "62 A2FD03 91 5C A9 01", 8, Code.EVEX_Vpgatherqq_xmm_k1_vm64x, Register.XMM19, Register.K3, MemorySize.Int64, Register.RCX, Register.XMM29, 8, true };
				yield return new object[] { "62 42FD03 91 5C 8F 01", 8, Code.EVEX_Vpgatherqq_xmm_k1_vm64x, Register.XMM27, Register.K3, MemorySize.Int64, Register.R15, Register.XMM17, 8, true };

				yield return new object[] { "62 F2FD2B 91 54 A1 01", 8, Code.EVEX_Vpgatherqq_ymm_k1_vm64y, Register.YMM2, Register.K3, MemorySize.Int64, Register.RCX, Register.YMM4, 8, true };
				yield return new object[] { "62 A2FD23 91 5C A9 01", 8, Code.EVEX_Vpgatherqq_ymm_k1_vm64y, Register.YMM19, Register.K3, MemorySize.Int64, Register.RCX, Register.YMM29, 8, true };
				yield return new object[] { "62 42FD23 91 5C 8F 01", 8, Code.EVEX_Vpgatherqq_ymm_k1_vm64y, Register.YMM27, Register.K3, MemorySize.Int64, Register.R15, Register.YMM17, 8, true };

				yield return new object[] { "62 F2FD4B 91 54 A1 01", 8, Code.EVEX_Vpgatherqq_zmm_k1_vm64z, Register.ZMM2, Register.K3, MemorySize.Int64, Register.RCX, Register.ZMM4, 8, true };
				yield return new object[] { "62 A2FD43 91 5C A9 01", 8, Code.EVEX_Vpgatherqq_zmm_k1_vm64z, Register.ZMM19, Register.K3, MemorySize.Int64, Register.RCX, Register.ZMM29, 8, true };
				yield return new object[] { "62 42FD43 91 5C 8F 01", 8, Code.EVEX_Vpgatherqq_zmm_k1_vm64z, Register.ZMM27, Register.K3, MemorySize.Int64, Register.R15, Register.ZMM17, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgatherdpsV_VX_HX_WX_1_Data))]
		void Test16_VgatherdpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VgatherdpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "67 C4E249 92 54 A1 01", 8, Code.VEX_Vgatherdps_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.ECX, Register.XMM4, false };
				yield return new object[] { "67 C4E24D 92 54 A1 01", 8, Code.VEX_Vgatherdps_ymm_vm32y_ymm, Register.YMM2, Register.YMM6, MemorySize.Float32, Register.ECX, Register.YMM4, false };

				yield return new object[] { "67 C4E2C9 92 54 A1 01", 8, Code.VEX_Vgatherdpd_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.ECX, Register.XMM4, false };
				yield return new object[] { "67 C4E2CD 92 54 A1 01", 8, Code.VEX_Vgatherdpd_ymm_vm32x_ymm, Register.YMM2, Register.YMM6, MemorySize.Float64, Register.ECX, Register.XMM4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgatherdpsV_VX_HX_WX_1_Data))]
		void Test32_VgatherdpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VgatherdpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 92 54 A1 01", 7, Code.VEX_Vgatherdps_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.ECX, Register.XMM4, false };
				yield return new object[] { "C4E24D 92 54 A1 01", 7, Code.VEX_Vgatherdps_ymm_vm32y_ymm, Register.YMM2, Register.YMM6, MemorySize.Float32, Register.ECX, Register.YMM4, false };

				yield return new object[] { "C4E2C9 92 54 A1 01", 7, Code.VEX_Vgatherdpd_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.ECX, Register.XMM4, false };
				yield return new object[] { "C4E2CD 92 54 A1 01", 7, Code.VEX_Vgatherdpd_ymm_vm32x_ymm, Register.YMM2, Register.YMM6, MemorySize.Float64, Register.ECX, Register.XMM4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgatherdpsV_VX_HX_WX_1_Data))]
		void Test64_VgatherdpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VgatherdpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 92 54 A1 01", 7, Code.VEX_Vgatherdps_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C46249 92 54 A1 01", 7, Code.VEX_Vgatherdps_xmm_vm32x_xmm, Register.XMM10, Register.XMM6, MemorySize.Float32, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C4E209 92 54 A1 01", 7, Code.VEX_Vgatherdps_xmm_vm32x_xmm, Register.XMM2, Register.XMM14, MemorySize.Float32, Register.RCX, Register.XMM4, false };

				yield return new object[] { "C4E24D 92 54 A1 01", 7, Code.VEX_Vgatherdps_ymm_vm32y_ymm, Register.YMM2, Register.YMM6, MemorySize.Float32, Register.RCX, Register.YMM4, false };
				yield return new object[] { "C4624D 92 54 A1 01", 7, Code.VEX_Vgatherdps_ymm_vm32y_ymm, Register.YMM10, Register.YMM6, MemorySize.Float32, Register.RCX, Register.YMM4, false };
				yield return new object[] { "C4E20D 92 54 A1 01", 7, Code.VEX_Vgatherdps_ymm_vm32y_ymm, Register.YMM2, Register.YMM14, MemorySize.Float32, Register.RCX, Register.YMM4, false };

				yield return new object[] { "C4E2C9 92 54 A1 01", 7, Code.VEX_Vgatherdpd_xmm_vm32x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C462C9 92 54 A1 01", 7, Code.VEX_Vgatherdpd_xmm_vm32x_xmm, Register.XMM10, Register.XMM6, MemorySize.Float64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C4E289 92 54 A1 01", 7, Code.VEX_Vgatherdpd_xmm_vm32x_xmm, Register.XMM2, Register.XMM14, MemorySize.Float64, Register.RCX, Register.XMM4, false };

				yield return new object[] { "C4E2CD 92 54 A1 01", 7, Code.VEX_Vgatherdpd_ymm_vm32x_ymm, Register.YMM2, Register.YMM6, MemorySize.Float64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C462CD 92 54 A1 01", 7, Code.VEX_Vgatherdpd_ymm_vm32x_ymm, Register.YMM10, Register.YMM6, MemorySize.Float64, Register.RCX, Register.XMM4, false };
				yield return new object[] { "C4E28D 92 54 A1 01", 7, Code.VEX_Vgatherdpd_ymm_vm32x_ymm, Register.YMM2, Register.YMM14, MemorySize.Float64, Register.RCX, Register.XMM4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgatherdpsV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VgatherdpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VgatherdpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "67 62 F27D0B 92 54 A1 01", 9, Code.EVEX_Vgatherdps_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.XMM4, 4, false };

				yield return new object[] { "67 62 F27D2B 92 54 A1 01", 9, Code.EVEX_Vgatherdps_ymm_k1_vm32y, Register.YMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.YMM4, 4, false };

				yield return new object[] { "67 62 F27D4B 92 54 A1 01", 9, Code.EVEX_Vgatherdps_zmm_k1_vm32z, Register.ZMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "67 62 F2FD0B 92 54 A1 01", 9, Code.EVEX_Vgatherdpd_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "67 62 F2FD2B 92 54 A1 01", 9, Code.EVEX_Vgatherdpd_ymm_k1_vm32x, Register.YMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "67 62 F2FD4B 92 54 A1 01", 9, Code.EVEX_Vgatherdpd_zmm_k1_vm32y, Register.ZMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgatherdpsV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VgatherdpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VgatherdpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 92 54 A1 01", 8, Code.EVEX_Vgatherdps_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.XMM4, 4, false };

				yield return new object[] { "62 F27D2B 92 54 A1 01", 8, Code.EVEX_Vgatherdps_ymm_k1_vm32y, Register.YMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.YMM4, 4, false };

				yield return new object[] { "62 F27D4B 92 54 A1 01", 8, Code.EVEX_Vgatherdps_zmm_k1_vm32z, Register.ZMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, false };

				yield return new object[] { "62 F2FD0B 92 54 A1 01", 8, Code.EVEX_Vgatherdpd_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "62 F2FD2B 92 54 A1 01", 8, Code.EVEX_Vgatherdpd_ymm_k1_vm32x, Register.YMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.XMM4, 8, false };

				yield return new object[] { "62 F2FD4B 92 54 A1 01", 8, Code.EVEX_Vgatherdpd_zmm_k1_vm32y, Register.ZMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgatherdpsV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VgatherdpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VgatherdpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 92 54 A1 01", 8, Code.EVEX_Vgatherdps_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Float32, Register.RCX, Register.XMM4, 4, false };
				yield return new object[] { "62 A27D03 92 5C A9 01", 8, Code.EVEX_Vgatherdps_xmm_k1_vm32x, Register.XMM19, Register.K3, MemorySize.Float32, Register.RCX, Register.XMM29, 4, false };
				yield return new object[] { "62 427D03 92 5C 8F 01", 8, Code.EVEX_Vgatherdps_xmm_k1_vm32x, Register.XMM27, Register.K3, MemorySize.Float32, Register.R15, Register.XMM17, 4, false };

				yield return new object[] { "62 F27D2B 92 54 A1 01", 8, Code.EVEX_Vgatherdps_ymm_k1_vm32y, Register.YMM2, Register.K3, MemorySize.Float32, Register.RCX, Register.YMM4, 4, false };
				yield return new object[] { "62 A27D23 92 5C A9 01", 8, Code.EVEX_Vgatherdps_ymm_k1_vm32y, Register.YMM19, Register.K3, MemorySize.Float32, Register.RCX, Register.YMM29, 4, false };
				yield return new object[] { "62 427D23 92 5C 8F 01", 8, Code.EVEX_Vgatherdps_ymm_k1_vm32y, Register.YMM27, Register.K3, MemorySize.Float32, Register.R15, Register.YMM17, 4, false };

				yield return new object[] { "62 F27D4B 92 54 A1 01", 8, Code.EVEX_Vgatherdps_zmm_k1_vm32z, Register.ZMM2, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, false };
				yield return new object[] { "62 A27D43 92 5C A9 01", 8, Code.EVEX_Vgatherdps_zmm_k1_vm32z, Register.ZMM19, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, false };
				yield return new object[] { "62 427D43 92 5C 8F 01", 8, Code.EVEX_Vgatherdps_zmm_k1_vm32z, Register.ZMM27, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, false };

				yield return new object[] { "62 F2FD0B 92 54 A1 01", 8, Code.EVEX_Vgatherdpd_xmm_k1_vm32x, Register.XMM2, Register.K3, MemorySize.Float64, Register.RCX, Register.XMM4, 8, false };
				yield return new object[] { "62 A2FD03 92 5C A9 01", 8, Code.EVEX_Vgatherdpd_xmm_k1_vm32x, Register.XMM19, Register.K3, MemorySize.Float64, Register.RCX, Register.XMM29, 8, false };
				yield return new object[] { "62 42FD03 92 5C 8F 01", 8, Code.EVEX_Vgatherdpd_xmm_k1_vm32x, Register.XMM27, Register.K3, MemorySize.Float64, Register.R15, Register.XMM17, 8, false };

				yield return new object[] { "62 F2FD2B 92 54 A1 01", 8, Code.EVEX_Vgatherdpd_ymm_k1_vm32x, Register.YMM2, Register.K3, MemorySize.Float64, Register.RCX, Register.XMM4, 8, false };
				yield return new object[] { "62 A2FD23 92 5C A9 01", 8, Code.EVEX_Vgatherdpd_ymm_k1_vm32x, Register.YMM19, Register.K3, MemorySize.Float64, Register.RCX, Register.XMM29, 8, false };
				yield return new object[] { "62 42FD23 92 5C 8F 01", 8, Code.EVEX_Vgatherdpd_ymm_k1_vm32x, Register.YMM27, Register.K3, MemorySize.Float64, Register.R15, Register.XMM17, 8, false };

				yield return new object[] { "62 F2FD4B 92 54 A1 01", 8, Code.EVEX_Vgatherdpd_zmm_k1_vm32y, Register.ZMM2, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM4, 8, false };
				yield return new object[] { "62 A2FD43 92 5C A9 01", 8, Code.EVEX_Vgatherdpd_zmm_k1_vm32y, Register.ZMM19, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM29, 8, false };
				yield return new object[] { "62 42FD43 92 5C 8F 01", 8, Code.EVEX_Vgatherdpd_zmm_k1_vm32y, Register.ZMM27, Register.K3, MemorySize.Float64, Register.R15, Register.YMM17, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgatherqpsV_VX_HX_WX_1_Data))]
		void Test16_VgatherqpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VgatherqpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "67 C4E249 93 54 A1 01", 8, Code.VEX_Vgatherqps_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.ECX, Register.XMM4, true };
				yield return new object[] { "67 C4E24D 93 54 A1 01", 8, Code.VEX_Vgatherqps_xmm_vm64y_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.ECX, Register.YMM4, true };

				yield return new object[] { "67 C4E2C9 93 54 A1 01", 8, Code.VEX_Vgatherqpd_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.ECX, Register.XMM4, true };
				yield return new object[] { "67 C4E2CD 93 54 A1 01", 8, Code.VEX_Vgatherqpd_ymm_vm64y_ymm, Register.YMM2, Register.YMM6, MemorySize.Float64, Register.ECX, Register.YMM4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgatherqpsV_VX_HX_WX_1_Data))]
		void Test32_VgatherqpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VgatherqpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.ECX, Register.XMM4, true };
				yield return new object[] { "C4E24D 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64y_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.ECX, Register.YMM4, true };

				yield return new object[] { "C4E2C9 93 54 A1 01", 7, Code.VEX_Vgatherqpd_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.ECX, Register.XMM4, true };
				yield return new object[] { "C4E2CD 93 54 A1 01", 7, Code.VEX_Vgatherqpd_ymm_vm64y_ymm, Register.YMM2, Register.YMM6, MemorySize.Float64, Register.ECX, Register.YMM4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgatherqpsV_VX_HX_WX_1_Data))]
		void Test64_VgatherqpsV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register memBase, Register memIndex, bool isVsib64) {
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
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(1U, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg2, instr.Op2Register);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VgatherqpsV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C46249 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64x_xmm, Register.XMM10, Register.XMM6, MemorySize.Float32, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C4E209 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64x_xmm, Register.XMM2, Register.XMM14, MemorySize.Float32, Register.RCX, Register.XMM4, true };

				yield return new object[] { "C4E24D 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64y_xmm, Register.XMM2, Register.XMM6, MemorySize.Float32, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C4624D 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64y_xmm, Register.XMM10, Register.XMM6, MemorySize.Float32, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C4E20D 93 54 A1 01", 7, Code.VEX_Vgatherqps_xmm_vm64y_xmm, Register.XMM2, Register.XMM14, MemorySize.Float32, Register.RCX, Register.YMM4, true };

				yield return new object[] { "C4E2C9 93 54 A1 01", 7, Code.VEX_Vgatherqpd_xmm_vm64x_xmm, Register.XMM2, Register.XMM6, MemorySize.Float64, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C462C9 93 54 A1 01", 7, Code.VEX_Vgatherqpd_xmm_vm64x_xmm, Register.XMM10, Register.XMM6, MemorySize.Float64, Register.RCX, Register.XMM4, true };
				yield return new object[] { "C4E289 93 54 A1 01", 7, Code.VEX_Vgatherqpd_xmm_vm64x_xmm, Register.XMM2, Register.XMM14, MemorySize.Float64, Register.RCX, Register.XMM4, true };

				yield return new object[] { "C4E2CD 93 54 A1 01", 7, Code.VEX_Vgatherqpd_ymm_vm64y_ymm, Register.YMM2, Register.YMM6, MemorySize.Float64, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C462CD 93 54 A1 01", 7, Code.VEX_Vgatherqpd_ymm_vm64y_ymm, Register.YMM10, Register.YMM6, MemorySize.Float64, Register.RCX, Register.YMM4, true };
				yield return new object[] { "C4E28D 93 54 A1 01", 7, Code.VEX_Vgatherqpd_ymm_vm64y_ymm, Register.YMM2, Register.YMM14, MemorySize.Float64, Register.RCX, Register.YMM4, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgatherqpsV_Reg_RegMem_EVEX_1_Data))]
		void Test16_VgatherqpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test16_VgatherqpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "67 62 F27D0B 93 54 A1 01", 9, Code.EVEX_Vgatherqps_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.XMM4, 4, true };

				yield return new object[] { "67 62 F27D2B 93 54 A1 01", 9, Code.EVEX_Vgatherqps_xmm_k1_vm64y, Register.XMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.YMM4, 4, true };

				yield return new object[] { "67 62 F27D4B 93 54 A1 01", 9, Code.EVEX_Vgatherqps_ymm_k1_vm64z, Register.YMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "67 62 F2FD0B 93 54 A1 01", 9, Code.EVEX_Vgatherqpd_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.XMM4, 8, true };

				yield return new object[] { "67 62 F2FD2B 93 54 A1 01", 9, Code.EVEX_Vgatherqpd_ymm_k1_vm64y, Register.YMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, true };

				yield return new object[] { "67 62 F2FD4B 93 54 A1 01", 9, Code.EVEX_Vgatherqpd_zmm_k1_vm64z, Register.ZMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgatherqpsV_Reg_RegMem_EVEX_1_Data))]
		void Test32_VgatherqpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test32_VgatherqpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 93 54 A1 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.XMM4, 4, true };

				yield return new object[] { "62 F27D2B 93 54 A1 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64y, Register.XMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.YMM4, 4, true };

				yield return new object[] { "62 F27D4B 93 54 A1 01", 8, Code.EVEX_Vgatherqps_ymm_k1_vm64z, Register.YMM2, Register.K3, MemorySize.Float32, Register.ECX, Register.ZMM4, 4, true };

				yield return new object[] { "62 F2FD0B 93 54 A1 01", 8, Code.EVEX_Vgatherqpd_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.XMM4, 8, true };

				yield return new object[] { "62 F2FD2B 93 54 A1 01", 8, Code.EVEX_Vgatherqpd_ymm_k1_vm64y, Register.YMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.YMM4, 8, true };

				yield return new object[] { "62 F2FD4B 93 54 A1 01", 8, Code.EVEX_Vgatherqpd_zmm_k1_vm64z, Register.ZMM2, Register.K3, MemorySize.Float64, Register.ECX, Register.ZMM4, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgatherqpsV_Reg_RegMem_EVEX_1_Data))]
		void Test64_VgatherqpsV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, Register memBase, Register memIndex, uint displ, bool isVsib64) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(memBase, instr.MemoryBase);
			Assert.Equal(memIndex, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(4, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);

			Assert.True(instr.IsVsib);
			Assert.Equal(!isVsib64, instr.IsVsib32);
			Assert.Equal(isVsib64, instr.IsVsib64);
			Assert.True(instr.TryGetVsib64(out var vsib64) && vsib64 == isVsib64);
		}
		public static IEnumerable<object[]> Test64_VgatherqpsV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 93 54 A1 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Float32, Register.RCX, Register.XMM4, 4, true };
				yield return new object[] { "62 A27D03 93 5C A9 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64x, Register.XMM19, Register.K3, MemorySize.Float32, Register.RCX, Register.XMM29, 4, true };
				yield return new object[] { "62 427D03 93 5C 8F 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64x, Register.XMM27, Register.K3, MemorySize.Float32, Register.R15, Register.XMM17, 4, true };

				yield return new object[] { "62 F27D2B 93 54 A1 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64y, Register.XMM2, Register.K3, MemorySize.Float32, Register.RCX, Register.YMM4, 4, true };
				yield return new object[] { "62 A27D23 93 5C A9 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64y, Register.XMM19, Register.K3, MemorySize.Float32, Register.RCX, Register.YMM29, 4, true };
				yield return new object[] { "62 427D23 93 5C 8F 01", 8, Code.EVEX_Vgatherqps_xmm_k1_vm64y, Register.XMM27, Register.K3, MemorySize.Float32, Register.R15, Register.YMM17, 4, true };

				yield return new object[] { "62 F27D4B 93 54 A1 01", 8, Code.EVEX_Vgatherqps_ymm_k1_vm64z, Register.YMM2, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM4, 4, true };
				yield return new object[] { "62 A27D43 93 5C A9 01", 8, Code.EVEX_Vgatherqps_ymm_k1_vm64z, Register.YMM19, Register.K3, MemorySize.Float32, Register.RCX, Register.ZMM29, 4, true };
				yield return new object[] { "62 427D43 93 5C 8F 01", 8, Code.EVEX_Vgatherqps_ymm_k1_vm64z, Register.YMM27, Register.K3, MemorySize.Float32, Register.R15, Register.ZMM17, 4, true };

				yield return new object[] { "62 F2FD0B 93 54 A1 01", 8, Code.EVEX_Vgatherqpd_xmm_k1_vm64x, Register.XMM2, Register.K3, MemorySize.Float64, Register.RCX, Register.XMM4, 8, true };
				yield return new object[] { "62 A2FD03 93 5C A9 01", 8, Code.EVEX_Vgatherqpd_xmm_k1_vm64x, Register.XMM19, Register.K3, MemorySize.Float64, Register.RCX, Register.XMM29, 8, true };
				yield return new object[] { "62 42FD03 93 5C 8F 01", 8, Code.EVEX_Vgatherqpd_xmm_k1_vm64x, Register.XMM27, Register.K3, MemorySize.Float64, Register.R15, Register.XMM17, 8, true };

				yield return new object[] { "62 F2FD2B 93 54 A1 01", 8, Code.EVEX_Vgatherqpd_ymm_k1_vm64y, Register.YMM2, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM4, 8, true };
				yield return new object[] { "62 A2FD23 93 5C A9 01", 8, Code.EVEX_Vgatherqpd_ymm_k1_vm64y, Register.YMM19, Register.K3, MemorySize.Float64, Register.RCX, Register.YMM29, 8, true };
				yield return new object[] { "62 42FD23 93 5C 8F 01", 8, Code.EVEX_Vgatherqpd_ymm_k1_vm64y, Register.YMM27, Register.K3, MemorySize.Float64, Register.R15, Register.YMM17, 8, true };

				yield return new object[] { "62 F2FD4B 93 54 A1 01", 8, Code.EVEX_Vgatherqpd_zmm_k1_vm64z, Register.ZMM2, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM4, 8, true };
				yield return new object[] { "62 A2FD43 93 5C A9 01", 8, Code.EVEX_Vgatherqpd_zmm_k1_vm64z, Register.ZMM19, Register.K3, MemorySize.Float64, Register.RCX, Register.ZMM29, 8, true };
				yield return new object[] { "62 42FD43 93 5C 8F 01", 8, Code.EVEX_Vgatherqpd_zmm_k1_vm64z, Register.ZMM27, Register.K3, MemorySize.Float64, Register.R15, Register.ZMM17, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub132psV_VX_HX_WX_1_Data))]
		void Test16_Vfmaddsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmaddsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 96 10", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 96 10", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 96 10", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 96 10", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub132psV_VX_HX_WX_2_Data))]
		void Test16_Vfmaddsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Vfmaddsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 96 D3", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 96 D3", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 96 D3", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 96 D3", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub132psV_VX_HX_WX_1_Data))]
		void Test32_Vfmaddsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmaddsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 96 10", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 96 10", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 96 10", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 96 10", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub132psV_VX_HX_WX_2_Data))]
		void Test32_Vfmaddsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Vfmaddsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 96 D3", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 96 D3", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 96 D3", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 96 D3", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub132psV_VX_HX_WX_1_Data))]
		void Test64_Vfmaddsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmaddsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 96 10", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 96 10", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 96 10", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 96 10", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub132psV_VX_HX_WX_2_Data))]
		void Test64_Vfmaddsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Vfmaddsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 96 D3", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 96 D3", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 96 D3", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 96 D3", 5, Code.VEX_Vfmaddsub132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 96 D3", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 96 D3", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 96 D3", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 96 D3", 5, Code.VEX_Vfmaddsub132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 96 D3", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 96 D3", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 96 D3", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 96 D3", 5, Code.VEX_Vfmaddsub132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD 96 D3", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD 96 D3", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D 96 D3", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD 96 D3", 5, Code.VEX_Vfmaddsub132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub132psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmaddsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmaddsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub132psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmaddsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmaddsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub132psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmaddsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmaddsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub132psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmaddsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmaddsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub132psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmaddsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmaddsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 96 50 01", 7, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 96 50 01", 7, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub132psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmaddsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmaddsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 96 D3", 6, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 96 D3", 6, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 96 D3", 6, Code.EVEX_Vfmaddsub132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 96 D3", 6, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 96 D3", 6, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 96 D3", 6, Code.EVEX_Vfmaddsub132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd132psV_VX_HX_WX_1_Data))]
		void Test16_Vfmsubadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmsubadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 97 10", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 97 10", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 97 10", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 97 10", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd132psV_VX_HX_WX_2_Data))]
		void Test16_Vfmsubadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Vfmsubadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 97 D3", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 97 D3", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 97 D3", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 97 D3", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd132psV_VX_HX_WX_1_Data))]
		void Test32_Vfmsubadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmsubadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 97 10", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 97 10", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 97 10", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 97 10", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd132psV_VX_HX_WX_2_Data))]
		void Test32_Vfmsubadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Vfmsubadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 97 D3", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 97 D3", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 97 D3", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 97 D3", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd132psV_VX_HX_WX_1_Data))]
		void Test64_Vfmsubadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmsubadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 97 10", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 97 10", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 97 10", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 97 10", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd132psV_VX_HX_WX_2_Data))]
		void Test64_Vfmsubadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Vfmsubadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 97 D3", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 97 D3", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 97 D3", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 97 D3", 5, Code.VEX_Vfmsubadd132ps_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 97 D3", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 97 D3", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 97 D3", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 97 D3", 5, Code.VEX_Vfmsubadd132ps_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 97 D3", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 97 D3", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 97 D3", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 97 D3", 5, Code.VEX_Vfmsubadd132pd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD 97 D3", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD 97 D3", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D 97 D3", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD 97 D3", 5, Code.VEX_Vfmsubadd132pd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd132psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsubadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsubadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd132psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsubadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmsubadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd132psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsubadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsubadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd132psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsubadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmsubadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd132psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsubadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsubadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 97 50 01", 7, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 97 50 01", 7, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd132psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsubadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmsubadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 97 D3", 6, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_xmm_k1z_xmm_xmmm128b32, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 97 D3", 6, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_ymm_k1z_ymm_ymmm256b32, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 97 D3", 6, Code.EVEX_Vfmsubadd132ps_zmm_k1z_zmm_zmmm512b32_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 97 D3", 6, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 97 D3", 6, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 97 D3", 6, Code.EVEX_Vfmsubadd132pd_zmm_k1z_zmm_zmmm512b64_er, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}
	}
}
