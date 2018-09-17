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
	public sealed class DecoderTest_2_0F28_0F2F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_MovaV_Reg_RegMem_1_Data))]
		void Test16_MovaV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovaV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F28 08", 3, Code.Movaps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F28 08", 4, Code.Movapd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 28 10", 4, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 28 10", 5, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 28 10", 4, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 28 10", 5, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 28 10", 4, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 28 10", 5, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 28 10", 4, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 28 10", 5, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovaV_Reg_RegMem_2_Data))]
		void Test16_MovaV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_MovaV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F28 CD", 3, Code.Movaps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F28 CD", 4, Code.Movapd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 28 CD", 4, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 28 CD", 4, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5F9 28 CD", 4, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 28 CD", 4, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_Reg_RegMem_1_Data))]
		void Test32_MovaV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovaV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F28 08", 3, Code.Movaps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F28 08", 4, Code.Movapd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 28 10", 4, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 28 10", 5, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 28 10", 4, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 28 10", 5, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 28 10", 4, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 28 10", 5, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 28 10", 4, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 28 10", 5, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_Reg_RegMem_2_Data))]
		void Test32_MovaV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_MovaV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F28 CD", 3, Code.Movaps_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F28 CD", 4, Code.Movapd_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F8 28 CD", 4, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FC 28 CD", 4, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM1, Register.YMM5 };

				yield return new object[] { "C5F9 28 CD", 4, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 28 CD", 4, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_Reg_RegMem_1_Data))]
		void Test64_MovaV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovaV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F28 08", 3, Code.Movaps_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F28 08", 4, Code.Movapd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 28 10", 4, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 28 10", 5, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 28 10", 4, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 28 10", 5, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 28 10", 4, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 28 10", 5, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 28 10", 4, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 28 10", 5, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_Reg_RegMem_2_Data))]
		void Test64_MovaV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_MovaV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F28 CD", 3, Code.Movaps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F28 CD", 4, Code.Movaps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F28 CD", 4, Code.Movaps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F28 CD", 4, Code.Movaps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F28 CD", 4, Code.Movapd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F28 CD", 5, Code.Movapd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F28 CD", 5, Code.Movapd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F28 CD", 5, Code.Movapd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F8 28 CD", 4, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C578 28 CD", 4, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C178 28 CD", 5, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44178 28 CD", 5, Code.VEX_Vmovaps_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FC 28 CD", 4, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57C 28 CD", 4, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17C 28 CD", 5, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417C 28 CD", 5, Code.VEX_Vmovaps_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "C5F9 28 CD", 4, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C579 28 CD", 4, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C179 28 CD", 5, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44179 28 CD", 5, Code.VEX_Vmovapd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FD 28 CD", 4, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57D 28 CD", 4, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17D 28 CD", 5, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417D 28 CD", 5, Code.VEX_Vmovapd_ymm_ymmm256, Register.YMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovaV_Reg_RegMem_EVEX_1_Data))]
		void Test16_MovaV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_MovaV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 28 50 01", 7, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C8B 28 50 01", 7, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17C28 28 50 01", 7, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CAB 28 50 01", 7, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17C48 28 50 01", 7, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CCB 28 50 01", 7, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FD08 28 50 01", 7, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD8B 28 50 01", 7, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F1FD28 28 50 01", 7, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDAB 28 50 01", 7, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FD48 28 50 01", 7, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDCB 28 50 01", 7, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovaV_Reg_RegMem_EVEX_2_Data))]
		void Test16_MovaV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovaV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_Reg_RegMem_EVEX_1_Data))]
		void Test32_MovaV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_MovaV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 28 50 01", 7, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C8B 28 50 01", 7, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17C28 28 50 01", 7, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CAB 28 50 01", 7, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17C48 28 50 01", 7, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CCB 28 50 01", 7, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FD08 28 50 01", 7, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD8B 28 50 01", 7, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F1FD28 28 50 01", 7, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDAB 28 50 01", 7, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FD48 28 50 01", 7, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDCB 28 50 01", 7, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_Reg_RegMem_EVEX_2_Data))]
		void Test32_MovaV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovaV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_Reg_RegMem_EVEX_1_Data))]
		void Test64_MovaV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_MovaV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 28 50 01", 7, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C8B 28 50 01", 7, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, true };

				yield return new object[] { "62 F17C28 28 50 01", 7, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17CAB 28 50 01", 7, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, true };

				yield return new object[] { "62 F17C48 28 50 01", 7, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17CCB 28 50 01", 7, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, true };

				yield return new object[] { "62 F1FD08 28 50 01", 7, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD8B 28 50 01", 7, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, true };

				yield return new object[] { "62 F1FD28 28 50 01", 7, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDAB 28 50 01", 7, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, true };

				yield return new object[] { "62 F1FD48 28 50 01", 7, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDCB 28 50 01", 7, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_Reg_RegMem_EVEX_2_Data))]
		void Test64_MovaV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovaV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317C8B 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17C8B 28 D3", 6, Code.EVEX_Vmovaps_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CAB 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CAB 28 D3", 6, Code.EVEX_Vmovaps_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CCB 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CCB 28 D3", 6, Code.EVEX_Vmovaps_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FD8B 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FD8B 28 D3", 6, Code.EVEX_Vmovapd_xmm_k1z_xmmm128, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDAB 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDAB 28 D3", 6, Code.EVEX_Vmovapd_ymm_k1z_ymmm256, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDCB 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDCB 28 D3", 6, Code.EVEX_Vmovapd_zmm_k1z_zmmm512, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovaV_RegMem_Reg_1_Data))]
		void Test16_MovaV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test16_MovaV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F29 08", 3, Code.Movaps_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F29 08", 4, Code.Movapd_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 29 10", 4, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 29 10", 5, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 29 10", 4, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 29 10", 5, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 29 10", 4, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 29 10", 5, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 29 10", 4, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 29 10", 5, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovaV_RegMem_Reg_2_Data))]
		void Test16_MovaV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_MovaV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F29 CD", 3, Code.Movaps_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "66 0F29 CD", 4, Code.Movapd_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F8 29 CD", 4, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FC 29 CD", 4, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM5, Register.YMM1 };

				yield return new object[] { "C5F9 29 CD", 4, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FD 29 CD", 4, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM5, Register.YMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_RegMem_Reg_1_Data))]
		void Test32_MovaV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test32_MovaV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F29 08", 3, Code.Movaps_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F29 08", 4, Code.Movapd_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 29 10", 4, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 29 10", 5, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 29 10", 4, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 29 10", 5, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 29 10", 4, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 29 10", 5, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 29 10", 4, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 29 10", 5, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_RegMem_Reg_2_Data))]
		void Test32_MovaV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_MovaV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F29 CD", 3, Code.Movaps_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "66 0F29 CD", 4, Code.Movapd_xmmm128_xmm, Register.XMM5, Register.XMM1 };

				yield return new object[] { "C5F8 29 CD", 4, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FC 29 CD", 4, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM5, Register.YMM1 };

				yield return new object[] { "C5F9 29 CD", 4, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C5FD 29 CD", 4, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM5, Register.YMM1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_RegMem_Reg_1_Data))]
		void Test64_MovaV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test64_MovaV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F29 08", 3, Code.Movaps_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F29 08", 4, Code.Movapd_xmmm128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F8 29 10", 4, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 29 10", 5, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 29 10", 4, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 29 10", 5, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 29 10", 4, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 29 10", 5, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 29 10", 4, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 29 10", 5, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_RegMem_Reg_2_Data))]
		void Test64_MovaV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_MovaV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "0F29 CD", 3, Code.Movaps_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "44 0F29 CD", 4, Code.Movaps_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "41 0F29 CD", 4, Code.Movaps_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "45 0F29 CD", 4, Code.Movaps_xmmm128_xmm, Register.XMM13, Register.XMM9 };

				yield return new object[] { "66 0F29 CD", 4, Code.Movapd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "66 44 0F29 CD", 5, Code.Movapd_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "66 41 0F29 CD", 5, Code.Movapd_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "66 45 0F29 CD", 5, Code.Movapd_xmmm128_xmm, Register.XMM13, Register.XMM9 };

				yield return new object[] { "C5F8 29 CD", 4, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C578 29 CD", 4, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "C4C178 29 CD", 5, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "C44178 29 CD", 5, Code.VEX_Vmovaps_xmmm128_xmm, Register.XMM13, Register.XMM9 };
				yield return new object[] { "C5FC 29 CD", 4, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM5, Register.YMM1 };
				yield return new object[] { "C57C 29 CD", 4, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM5, Register.YMM9 };
				yield return new object[] { "C4C17C 29 CD", 5, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM13, Register.YMM1 };
				yield return new object[] { "C4417C 29 CD", 5, Code.VEX_Vmovaps_ymmm256_ymm, Register.YMM13, Register.YMM9 };

				yield return new object[] { "C5F9 29 CD", 4, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM5, Register.XMM1 };
				yield return new object[] { "C579 29 CD", 4, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM5, Register.XMM9 };
				yield return new object[] { "C4C179 29 CD", 5, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM13, Register.XMM1 };
				yield return new object[] { "C44179 29 CD", 5, Code.VEX_Vmovapd_xmmm128_xmm, Register.XMM13, Register.XMM9 };
				yield return new object[] { "C5FD 29 CD", 4, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM5, Register.YMM1 };
				yield return new object[] { "C57D 29 CD", 4, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM5, Register.YMM9 };
				yield return new object[] { "C4C17D 29 CD", 5, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM13, Register.YMM1 };
				yield return new object[] { "C4417D 29 CD", 5, Code.VEX_Vmovapd_ymmm256_ymm, Register.YMM13, Register.YMM9 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovaV_RegMem_Reg_EVEX_1_Data))]
		void Test16_MovaV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovaV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 29 50 01", 7, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C0B 29 50 01", 7, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 29 50 01", 7, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17C2B 29 50 01", 7, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 29 50 01", 7, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17C4B 29 50 01", 7, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 29 50 01", 7, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD0B 29 50 01", 7, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 29 50 01", 7, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FD2B 29 50 01", 7, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 29 50 01", 7, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FD4B 29 50 01", 7, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovaV_RegMem_Reg_EVEX_2_Data))]
		void Test16_MovaV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovaV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_RegMem_Reg_EVEX_1_Data))]
		void Test32_MovaV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovaV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 29 50 01", 7, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C0B 29 50 01", 7, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 29 50 01", 7, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17C2B 29 50 01", 7, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 29 50 01", 7, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17C4B 29 50 01", 7, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 29 50 01", 7, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD0B 29 50 01", 7, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 29 50 01", 7, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FD2B 29 50 01", 7, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 29 50 01", 7, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FD4B 29 50 01", 7, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovaV_RegMem_Reg_EVEX_2_Data))]
		void Test32_MovaV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovaV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C8B 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CAB 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17CCB 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_RegMem_Reg_EVEX_1_Data))]
		void Test64_MovaV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovaV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 29 50 01", 7, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F17C0B 29 50 01", 7, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 29 50 01", 7, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F17C2B 29 50 01", 7, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 29 50 01", 7, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F17C4B 29 50 01", 7, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 29 50 01", 7, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD0B 29 50 01", 7, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 29 50 01", 7, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FD2B 29 50 01", 7, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 29 50 01", 7, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FD4B 29 50 01", 7, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovaV_RegMem_Reg_EVEX_2_Data))]
		void Test64_MovaV_RegMem_Reg_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovaV_RegMem_Reg_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17C08 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C0B 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317C8B 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17C8B 29 D3", 6, Code.EVEX_Vmovaps_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C28 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C2B 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CAB 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CAB 29 D3", 6, Code.EVEX_Vmovaps_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17C48 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17C4B 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317CCB 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17CCB 29 D3", 6, Code.EVEX_Vmovaps_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FD8B 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM19, Register.XMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FD8B 29 D3", 6, Code.EVEX_Vmovapd_xmmm128_k1z_xmm, Register.XMM11, Register.XMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDAB 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM19, Register.YMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDAB 29 D3", 6, Code.EVEX_Vmovapd_ymmm256_k1z_ymm, Register.YMM11, Register.YMM18, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDCB 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM19, Register.ZMM10, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDCB 29 D3", 6, Code.EVEX_Vmovapd_zmmm512_k1z_zmm, Register.ZMM11, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_1_Data))]
		void Test16_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F2A 08", 3, Code.Cvtpi2ps_xmm_mmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F2A 08", 4, Code.Cvtpi2pd_xmm_mmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "F3 0F2A 08", 4, Code.Cvtsi2ss_xmm_rm32, Register.XMM1, MemorySize.Int32 };

				yield return new object[] { "F2 0F2A 08", 4, Code.Cvtsi2sd_xmm_rm32, Register.XMM1, MemorySize.Int32 };

				yield return new object[] { "0F2C 08", 3, Code.Cvttps2pi_mm_xmmm64, Register.MM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F2C 08", 4, Code.Cvttpd2pi_mm_xmmm128, Register.MM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2C 08", 4, Code.Cvttss2si_r32_xmmm32, Register.ECX, MemorySize.Float32 };

				yield return new object[] { "F2 0F2C 08", 4, Code.Cvttsd2si_r32_xmmm64, Register.ECX, MemorySize.Float64 };

				yield return new object[] { "0F2D 08", 3, Code.Cvtps2pi_mm_xmmm64, Register.MM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F2D 08", 4, Code.Cvtpd2pi_mm_xmmm128, Register.MM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2D 08", 4, Code.Cvtss2si_r32_xmmm32, Register.ECX, MemorySize.Float32 };

				yield return new object[] { "F2 0F2D 08", 4, Code.Cvtsd2si_r32_xmmm64, Register.ECX, MemorySize.Float64 };

				yield return new object[] { "C5FA 2C 10", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };
				yield return new object[] { "C5FE 2C 10", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };

				yield return new object[] { "C5FB 2C 10", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
				yield return new object[] { "C5FF 2C 10", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };

				yield return new object[] { "C5FA 2D 10", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };
				yield return new object[] { "C5FE 2D 10", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };

				yield return new object[] { "C5FB 2D 10", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
				yield return new object[] { "C5FF 2D 10", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_2_Data))]
		void Test16_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F2A CD", 3, Code.Cvtpi2ps_xmm_mmm64, Register.XMM1, Register.MM5 };

				yield return new object[] { "66 0F2A CD", 4, Code.Cvtpi2pd_xmm_mmm64, Register.XMM1, Register.MM5 };

				yield return new object[] { "F3 0F2A CD", 4, Code.Cvtsi2ss_xmm_rm32, Register.XMM1, Register.EBP };

				yield return new object[] { "F2 0F2A CD", 4, Code.Cvtsi2sd_xmm_rm32, Register.XMM1, Register.EBP };

				yield return new object[] { "0F2C CD", 3, Code.Cvttps2pi_mm_xmmm64, Register.MM1, Register.XMM5 };

				yield return new object[] { "66 0F2C CD", 4, Code.Cvttpd2pi_mm_xmmm128, Register.MM1, Register.XMM5 };

				yield return new object[] { "F3 0F2C CD", 4, Code.Cvttss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "F2 0F2C CD", 4, Code.Cvttsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };

				yield return new object[] { "0F2D CD", 3, Code.Cvtps2pi_mm_xmmm64, Register.MM1, Register.XMM5 };

				yield return new object[] { "66 0F2D CD", 4, Code.Cvtpd2pi_mm_xmmm128, Register.MM1, Register.XMM5 };

				yield return new object[] { "F3 0F2D CD", 4, Code.Cvtss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "F2 0F2D CD", 4, Code.Cvtsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FA 2C CD", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FB 2C CD", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FA 2D CD", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FB 2D CD", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_1_Data))]
		void Test32_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F2A 08", 3, Code.Cvtpi2ps_xmm_mmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F2A 08", 4, Code.Cvtpi2pd_xmm_mmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "F3 0F2A 08", 4, Code.Cvtsi2ss_xmm_rm32, Register.XMM1, MemorySize.Int32 };

				yield return new object[] { "F2 0F2A 08", 4, Code.Cvtsi2sd_xmm_rm32, Register.XMM1, MemorySize.Int32 };

				yield return new object[] { "0F2C 08", 3, Code.Cvttps2pi_mm_xmmm64, Register.MM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F2C 08", 4, Code.Cvttpd2pi_mm_xmmm128, Register.MM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2C 08", 4, Code.Cvttss2si_r32_xmmm32, Register.ECX, MemorySize.Float32 };

				yield return new object[] { "F2 0F2C 08", 4, Code.Cvttsd2si_r32_xmmm64, Register.ECX, MemorySize.Float64 };

				yield return new object[] { "0F2D 08", 3, Code.Cvtps2pi_mm_xmmm64, Register.MM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F2D 08", 4, Code.Cvtpd2pi_mm_xmmm128, Register.MM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2D 08", 4, Code.Cvtss2si_r32_xmmm32, Register.ECX, MemorySize.Float32 };

				yield return new object[] { "F2 0F2D 08", 4, Code.Cvtsd2si_r32_xmmm64, Register.ECX, MemorySize.Float64 };

				yield return new object[] { "C5FA 2C 10", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };
				yield return new object[] { "C5FE 2C 10", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };

				yield return new object[] { "C5FB 2C 10", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
				yield return new object[] { "C5FF 2C 10", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };

				yield return new object[] { "C5FA 2D 10", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };
				yield return new object[] { "C5FE 2D 10", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };

				yield return new object[] { "C5FB 2D 10", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
				yield return new object[] { "C5FF 2D 10", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_2_Data))]
		void Test32_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F2A CD", 3, Code.Cvtpi2ps_xmm_mmm64, Register.XMM1, Register.MM5 };

				yield return new object[] { "66 0F2A CD", 4, Code.Cvtpi2pd_xmm_mmm64, Register.XMM1, Register.MM5 };

				yield return new object[] { "F3 0F2A CD", 4, Code.Cvtsi2ss_xmm_rm32, Register.XMM1, Register.EBP };

				yield return new object[] { "F2 0F2A CD", 4, Code.Cvtsi2sd_xmm_rm32, Register.XMM1, Register.EBP };

				yield return new object[] { "0F2C CD", 3, Code.Cvttps2pi_mm_xmmm64, Register.MM1, Register.XMM5 };

				yield return new object[] { "66 0F2C CD", 4, Code.Cvttpd2pi_mm_xmmm128, Register.MM1, Register.XMM5 };

				yield return new object[] { "F3 0F2C CD", 4, Code.Cvttss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "F2 0F2C CD", 4, Code.Cvttsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };

				yield return new object[] { "0F2D CD", 3, Code.Cvtps2pi_mm_xmmm64, Register.MM1, Register.XMM5 };

				yield return new object[] { "66 0F2D CD", 4, Code.Cvtpd2pi_mm_xmmm128, Register.MM1, Register.XMM5 };

				yield return new object[] { "F3 0F2D CD", 4, Code.Cvtss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "F2 0F2D CD", 4, Code.Cvtsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FA 2C CD", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FB 2C CD", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FA 2D CD", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.ECX, Register.XMM5 };

				yield return new object[] { "C5FB 2D CD", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_1_Data))]
		void Test64_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F2A 08", 3, Code.Cvtpi2ps_xmm_mmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F2A 08", 4, Code.Cvtpi2pd_xmm_mmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "F3 0F2A 08", 4, Code.Cvtsi2ss_xmm_rm32, Register.XMM1, MemorySize.Int32 };

				yield return new object[] { "F3 48 0F2A 08", 5, Code.Cvtsi2ss_xmm_rm64, Register.XMM1, MemorySize.Int64 };

				yield return new object[] { "F2 0F2A 08", 4, Code.Cvtsi2sd_xmm_rm32, Register.XMM1, MemorySize.Int32 };

				yield return new object[] { "F2 48 0F2A 08", 5, Code.Cvtsi2sd_xmm_rm64, Register.XMM1, MemorySize.Int64 };

				yield return new object[] { "0F2C 08", 3, Code.Cvttps2pi_mm_xmmm64, Register.MM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F2C 08", 4, Code.Cvttpd2pi_mm_xmmm128, Register.MM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2C 08", 4, Code.Cvttss2si_r32_xmmm32, Register.ECX, MemorySize.Float32 };

				yield return new object[] { "F3 48 0F2C 08", 5, Code.Cvttss2si_r64_xmmm32, Register.RCX, MemorySize.Float32 };

				yield return new object[] { "F2 0F2C 08", 4, Code.Cvttsd2si_r32_xmmm64, Register.ECX, MemorySize.Float64 };

				yield return new object[] { "F2 48 0F2C 08", 5, Code.Cvttsd2si_r64_xmmm64, Register.RCX, MemorySize.Float64 };

				yield return new object[] { "0F2D 08", 3, Code.Cvtps2pi_mm_xmmm64, Register.MM1, MemorySize.Packed64_Float32 };

				yield return new object[] { "66 0F2D 08", 4, Code.Cvtpd2pi_mm_xmmm128, Register.MM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2D 08", 4, Code.Cvtss2si_r32_xmmm32, Register.ECX, MemorySize.Float32 };

				yield return new object[] { "F3 48 0F2D 08", 5, Code.Cvtss2si_r64_xmmm32, Register.RCX, MemorySize.Float32 };

				yield return new object[] { "F2 0F2D 08", 4, Code.Cvtsd2si_r32_xmmm64, Register.ECX, MemorySize.Float64 };

				yield return new object[] { "F2 48 0F2D 08", 5, Code.Cvtsd2si_r64_xmmm64, Register.RCX, MemorySize.Float64 };

				yield return new object[] { "C5FA 2C 10", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };
				yield return new object[] { "C5FE 2C 10", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };

				yield return new object[] { "C4E1FA 2C 10", 5, Code.VEX_Vcvttss2si_r64_xmmm32, Register.RDX, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 2C 10", 5, Code.VEX_Vcvttss2si_r64_xmmm32, Register.RDX, MemorySize.Float32 };

				yield return new object[] { "C5FB 2C 10", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
				yield return new object[] { "C5FF 2C 10", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };

				yield return new object[] { "C4E1FB 2C 10", 5, Code.VEX_Vcvttsd2si_r64_xmmm64, Register.RDX, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 2C 10", 5, Code.VEX_Vcvttsd2si_r64_xmmm64, Register.RDX, MemorySize.Float64 };

				yield return new object[] { "C5FA 2D 10", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };
				yield return new object[] { "C5FE 2D 10", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.EDX, MemorySize.Float32 };

				yield return new object[] { "C4E1FA 2D 10", 5, Code.VEX_Vcvtss2si_r64_xmmm32, Register.RDX, MemorySize.Float32 };
				yield return new object[] { "C4E1FE 2D 10", 5, Code.VEX_Vcvtss2si_r64_xmmm32, Register.RDX, MemorySize.Float32 };

				yield return new object[] { "C5FB 2D 10", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };
				yield return new object[] { "C5FF 2D 10", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.EDX, MemorySize.Float64 };

				yield return new object[] { "C4E1FB 2D 10", 5, Code.VEX_Vcvtsd2si_r64_xmmm64, Register.RDX, MemorySize.Float64 };
				yield return new object[] { "C4E1FF 2D 10", 5, Code.VEX_Vcvtsd2si_r64_xmmm64, Register.RDX, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_2_Data))]
		void Test64_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F2A CD", 3, Code.Cvtpi2ps_xmm_mmm64, Register.XMM1, Register.MM5 };
				yield return new object[] { "44 0F2A CD", 4, Code.Cvtpi2ps_xmm_mmm64, Register.XMM9, Register.MM5 };

				yield return new object[] { "66 0F2A CD", 4, Code.Cvtpi2pd_xmm_mmm64, Register.XMM1, Register.MM5 };
				yield return new object[] { "66 44 0F2A CD", 5, Code.Cvtpi2pd_xmm_mmm64, Register.XMM9, Register.MM5 };

				yield return new object[] { "F3 0F2A CD", 4, Code.Cvtsi2ss_xmm_rm32, Register.XMM1, Register.EBP };
				yield return new object[] { "F3 44 0F2A CD", 5, Code.Cvtsi2ss_xmm_rm32, Register.XMM9, Register.EBP };
				yield return new object[] { "F3 41 0F2A CD", 5, Code.Cvtsi2ss_xmm_rm32, Register.XMM1, Register.R13D };
				yield return new object[] { "F3 45 0F2A CD", 5, Code.Cvtsi2ss_xmm_rm32, Register.XMM9, Register.R13D };

				yield return new object[] { "F3 48 0F2A CD", 5, Code.Cvtsi2ss_xmm_rm64, Register.XMM1, Register.RBP };
				yield return new object[] { "F3 4C 0F2A CD", 5, Code.Cvtsi2ss_xmm_rm64, Register.XMM9, Register.RBP };
				yield return new object[] { "F3 49 0F2A CD", 5, Code.Cvtsi2ss_xmm_rm64, Register.XMM1, Register.R13 };
				yield return new object[] { "F3 4D 0F2A CD", 5, Code.Cvtsi2ss_xmm_rm64, Register.XMM9, Register.R13 };

				yield return new object[] { "F2 0F2A CD", 4, Code.Cvtsi2sd_xmm_rm32, Register.XMM1, Register.EBP };
				yield return new object[] { "F2 44 0F2A CD", 5, Code.Cvtsi2sd_xmm_rm32, Register.XMM9, Register.EBP };
				yield return new object[] { "F2 41 0F2A CD", 5, Code.Cvtsi2sd_xmm_rm32, Register.XMM1, Register.R13D };
				yield return new object[] { "F2 45 0F2A CD", 5, Code.Cvtsi2sd_xmm_rm32, Register.XMM9, Register.R13D };

				yield return new object[] { "F2 48 0F2A CD", 5, Code.Cvtsi2sd_xmm_rm64, Register.XMM1, Register.RBP };
				yield return new object[] { "F2 4C 0F2A CD", 5, Code.Cvtsi2sd_xmm_rm64, Register.XMM9, Register.RBP };
				yield return new object[] { "F2 49 0F2A CD", 5, Code.Cvtsi2sd_xmm_rm64, Register.XMM1, Register.R13 };
				yield return new object[] { "F2 4D 0F2A CD", 5, Code.Cvtsi2sd_xmm_rm64, Register.XMM9, Register.R13 };

				yield return new object[] { "0F2C CD", 3, Code.Cvttps2pi_mm_xmmm64, Register.MM1, Register.XMM5 };
				yield return new object[] { "41 0F2C CD", 4, Code.Cvttps2pi_mm_xmmm64, Register.MM1, Register.XMM13 };

				yield return new object[] { "66 0F2C CD", 4, Code.Cvttpd2pi_mm_xmmm128, Register.MM1, Register.XMM5 };
				yield return new object[] { "66 41 0F2C CD", 5, Code.Cvttpd2pi_mm_xmmm128, Register.MM1, Register.XMM13 };

				yield return new object[] { "F3 0F2C CD", 4, Code.Cvttss2si_r32_xmmm32, Register.ECX, Register.XMM5 };
				yield return new object[] { "F3 44 0F2C CD", 5, Code.Cvttss2si_r32_xmmm32, Register.R9D, Register.XMM5 };
				yield return new object[] { "F3 41 0F2C CD", 5, Code.Cvttss2si_r32_xmmm32, Register.ECX, Register.XMM13 };
				yield return new object[] { "F3 45 0F2C CD", 5, Code.Cvttss2si_r32_xmmm32, Register.R9D, Register.XMM13 };

				yield return new object[] { "F3 48 0F2C CD", 5, Code.Cvttss2si_r64_xmmm32, Register.RCX, Register.XMM5 };
				yield return new object[] { "F3 4C 0F2C CD", 5, Code.Cvttss2si_r64_xmmm32, Register.R9, Register.XMM5 };
				yield return new object[] { "F3 49 0F2C CD", 5, Code.Cvttss2si_r64_xmmm32, Register.RCX, Register.XMM13 };
				yield return new object[] { "F3 4D 0F2C CD", 5, Code.Cvttss2si_r64_xmmm32, Register.R9, Register.XMM13 };

				yield return new object[] { "F2 0F2C CD", 4, Code.Cvttsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };
				yield return new object[] { "F2 44 0F2C CD", 5, Code.Cvttsd2si_r32_xmmm64, Register.R9D, Register.XMM5 };
				yield return new object[] { "F2 41 0F2C CD", 5, Code.Cvttsd2si_r32_xmmm64, Register.ECX, Register.XMM13 };
				yield return new object[] { "F2 45 0F2C CD", 5, Code.Cvttsd2si_r32_xmmm64, Register.R9D, Register.XMM13 };

				yield return new object[] { "F2 48 0F2C CD", 5, Code.Cvttsd2si_r64_xmmm64, Register.RCX, Register.XMM5 };
				yield return new object[] { "F2 4C 0F2C CD", 5, Code.Cvttsd2si_r64_xmmm64, Register.R9, Register.XMM5 };
				yield return new object[] { "F2 49 0F2C CD", 5, Code.Cvttsd2si_r64_xmmm64, Register.RCX, Register.XMM13 };
				yield return new object[] { "F2 4D 0F2C CD", 5, Code.Cvttsd2si_r64_xmmm64, Register.R9, Register.XMM13 };

				yield return new object[] { "0F2D CD", 3, Code.Cvtps2pi_mm_xmmm64, Register.MM1, Register.XMM5 };
				yield return new object[] { "41 0F2D CD", 4, Code.Cvtps2pi_mm_xmmm64, Register.MM1, Register.XMM13 };

				yield return new object[] { "66 0F2D CD", 4, Code.Cvtpd2pi_mm_xmmm128, Register.MM1, Register.XMM5 };
				yield return new object[] { "66 41 0F2D CD", 5, Code.Cvtpd2pi_mm_xmmm128, Register.MM1, Register.XMM13 };

				yield return new object[] { "F3 0F2D CD", 4, Code.Cvtss2si_r32_xmmm32, Register.ECX, Register.XMM5 };
				yield return new object[] { "F3 44 0F2D CD", 5, Code.Cvtss2si_r32_xmmm32, Register.R9D, Register.XMM5 };
				yield return new object[] { "F3 41 0F2D CD", 5, Code.Cvtss2si_r32_xmmm32, Register.ECX, Register.XMM13 };
				yield return new object[] { "F3 45 0F2D CD", 5, Code.Cvtss2si_r32_xmmm32, Register.R9D, Register.XMM13 };

				yield return new object[] { "F3 48 0F2D CD", 5, Code.Cvtss2si_r64_xmmm32, Register.RCX, Register.XMM5 };
				yield return new object[] { "F3 4C 0F2D CD", 5, Code.Cvtss2si_r64_xmmm32, Register.R9, Register.XMM5 };
				yield return new object[] { "F3 49 0F2D CD", 5, Code.Cvtss2si_r64_xmmm32, Register.RCX, Register.XMM13 };
				yield return new object[] { "F3 4D 0F2D CD", 5, Code.Cvtss2si_r64_xmmm32, Register.R9, Register.XMM13 };

				yield return new object[] { "F2 0F2D CD", 4, Code.Cvtsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };
				yield return new object[] { "F2 44 0F2D CD", 5, Code.Cvtsd2si_r32_xmmm64, Register.R9D, Register.XMM5 };
				yield return new object[] { "F2 41 0F2D CD", 5, Code.Cvtsd2si_r32_xmmm64, Register.ECX, Register.XMM13 };
				yield return new object[] { "F2 45 0F2D CD", 5, Code.Cvtsd2si_r32_xmmm64, Register.R9D, Register.XMM13 };

				yield return new object[] { "F2 48 0F2D CD", 5, Code.Cvtsd2si_r64_xmmm64, Register.RCX, Register.XMM5 };
				yield return new object[] { "F2 4C 0F2D CD", 5, Code.Cvtsd2si_r64_xmmm64, Register.R9, Register.XMM5 };
				yield return new object[] { "F2 49 0F2D CD", 5, Code.Cvtsd2si_r64_xmmm64, Register.RCX, Register.XMM13 };
				yield return new object[] { "F2 4D 0F2D CD", 5, Code.Cvtsd2si_r64_xmmm64, Register.R9, Register.XMM13 };

				yield return new object[] { "C5FA 2C CD", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.ECX, Register.XMM5 };
				yield return new object[] { "C57A 2C CD", 4, Code.VEX_Vcvttss2si_r32_xmmm32, Register.R9D, Register.XMM5 };
				yield return new object[] { "C4C17A 2C CD", 5, Code.VEX_Vcvttss2si_r32_xmmm32, Register.ECX, Register.XMM13 };
				yield return new object[] { "C4417A 2C CD", 5, Code.VEX_Vcvttss2si_r32_xmmm32, Register.R9D, Register.XMM13 };

				yield return new object[] { "C4E1FA 2C CD", 5, Code.VEX_Vcvttss2si_r64_xmmm32, Register.RCX, Register.XMM5 };
				yield return new object[] { "C461FA 2C CD", 5, Code.VEX_Vcvttss2si_r64_xmmm32, Register.R9, Register.XMM5 };
				yield return new object[] { "C4C1FA 2C CD", 5, Code.VEX_Vcvttss2si_r64_xmmm32, Register.RCX, Register.XMM13 };
				yield return new object[] { "C441FA 2C CD", 5, Code.VEX_Vcvttss2si_r64_xmmm32, Register.R9, Register.XMM13 };

				yield return new object[] { "C5FB 2C CD", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };
				yield return new object[] { "C57B 2C CD", 4, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.R9D, Register.XMM5 };
				yield return new object[] { "C4C17B 2C CD", 5, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.ECX, Register.XMM13 };
				yield return new object[] { "C4417B 2C CD", 5, Code.VEX_Vcvttsd2si_r32_xmmm64, Register.R9D, Register.XMM13 };

				yield return new object[] { "C4E1FB 2C CD", 5, Code.VEX_Vcvttsd2si_r64_xmmm64, Register.RCX, Register.XMM5 };
				yield return new object[] { "C461FB 2C CD", 5, Code.VEX_Vcvttsd2si_r64_xmmm64, Register.R9, Register.XMM5 };
				yield return new object[] { "C4C1FB 2C CD", 5, Code.VEX_Vcvttsd2si_r64_xmmm64, Register.RCX, Register.XMM13 };
				yield return new object[] { "C441FB 2C CD", 5, Code.VEX_Vcvttsd2si_r64_xmmm64, Register.R9, Register.XMM13 };

				yield return new object[] { "C5FA 2D CD", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.ECX, Register.XMM5 };
				yield return new object[] { "C57A 2D CD", 4, Code.VEX_Vcvtss2si_r32_xmmm32, Register.R9D, Register.XMM5 };
				yield return new object[] { "C4C17A 2D CD", 5, Code.VEX_Vcvtss2si_r32_xmmm32, Register.ECX, Register.XMM13 };
				yield return new object[] { "C4417A 2D CD", 5, Code.VEX_Vcvtss2si_r32_xmmm32, Register.R9D, Register.XMM13 };

				yield return new object[] { "C4E1FA 2D CD", 5, Code.VEX_Vcvtss2si_r64_xmmm32, Register.RCX, Register.XMM5 };
				yield return new object[] { "C461FA 2D CD", 5, Code.VEX_Vcvtss2si_r64_xmmm32, Register.R9, Register.XMM5 };
				yield return new object[] { "C4C1FA 2D CD", 5, Code.VEX_Vcvtss2si_r64_xmmm32, Register.RCX, Register.XMM13 };
				yield return new object[] { "C441FA 2D CD", 5, Code.VEX_Vcvtss2si_r64_xmmm32, Register.R9, Register.XMM13 };

				yield return new object[] { "C5FB 2D CD", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.ECX, Register.XMM5 };
				yield return new object[] { "C57B 2D CD", 4, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.R9D, Register.XMM5 };
				yield return new object[] { "C4C17B 2D CD", 5, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.ECX, Register.XMM13 };
				yield return new object[] { "C4417B 2D CD", 5, Code.VEX_Vcvtsd2si_r32_xmmm64, Register.R9D, Register.XMM13 };

				yield return new object[] { "C4E1FB 2D CD", 5, Code.VEX_Vcvtsd2si_r64_xmmm64, Register.RCX, Register.XMM5 };
				yield return new object[] { "C461FB 2D CD", 5, Code.VEX_Vcvtsd2si_r64_xmmm64, Register.R9, Register.XMM5 };
				yield return new object[] { "C4C1FB 2D CD", 5, Code.VEX_Vcvtsd2si_r64_xmmm64, Register.RCX, Register.XMM13 };
				yield return new object[] { "C441FB 2D CD", 5, Code.VEX_Vcvtsd2si_r64_xmmm64, Register.R9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_1_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C5CA 2A 10", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C5CE 2A 10", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CA 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CE 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };

				yield return new object[] { "C5CB 2A 10", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C5CF 2A 10", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CB 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CF 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_2_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C5CA 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C5CE 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };

				yield return new object[] { "C5CB 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C5CF 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_1_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C5CA 2A 10", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C5CE 2A 10", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CA 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CE 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };

				yield return new object[] { "C5CB 2A 10", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C5CF 2A 10", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CB 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E1CF 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_2_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C5CA 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C5CE 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };

				yield return new object[] { "C5CB 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C5CF 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_1_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C5CA 2A 10", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C5CE 2A 10", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E14A 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E14E 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };

				yield return new object[] { "C4E1CA 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM6, MemorySize.Int64 };
				yield return new object[] { "C4E1CE 2A 10", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM6, MemorySize.Int64 };

				yield return new object[] { "C5CB 2A 10", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C5CF 2A 10", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E14B 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };
				yield return new object[] { "C4E14F 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, MemorySize.Int32 };

				yield return new object[] { "C4E1CB 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM6, MemorySize.Int64 };
				yield return new object[] { "C4E1CF 2A 10", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM6, MemorySize.Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_2_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C5CA 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C5CE 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C54A 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM10, Register.XMM6, Register.EBX };
				yield return new object[] { "C54E 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM10, Register.XMM6, Register.EBX };
				yield return new object[] { "C58A 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM14, Register.EBX };
				yield return new object[] { "C58E 2A D3", 4, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM14, Register.EBX };
				yield return new object[] { "C4C14A 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.R11D };
				yield return new object[] { "C4C14E 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.R11D };

				yield return new object[] { "C4E1CA 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.RBX };
				yield return new object[] { "C4E1CE 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.RBX };
				yield return new object[] { "C461CA 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM10, Register.XMM6, Register.RBX };
				yield return new object[] { "C461CE 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM10, Register.XMM6, Register.RBX };
				yield return new object[] { "C4E18A 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM14, Register.RBX };
				yield return new object[] { "C4E18E 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM14, Register.RBX };
				yield return new object[] { "C4C1CA 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.R11 };
				yield return new object[] { "C4C1CE 2A D3", 5, Code.VEX_Vcvtsi2ss_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.R11 };

				yield return new object[] { "C5CB 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C5CF 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX };
				yield return new object[] { "C54B 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM10, Register.XMM6, Register.EBX };
				yield return new object[] { "C54F 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM10, Register.XMM6, Register.EBX };
				yield return new object[] { "C58B 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM14, Register.EBX };
				yield return new object[] { "C58F 2A D3", 4, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM14, Register.EBX };
				yield return new object[] { "C4C14B 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.R11D };
				yield return new object[] { "C4C14F 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.R11D };

				yield return new object[] { "C4E1CB 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.RBX };
				yield return new object[] { "C4E1CF 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.RBX };
				yield return new object[] { "C461CB 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM10, Register.XMM6, Register.RBX };
				yield return new object[] { "C461CF 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM10, Register.XMM6, Register.RBX };
				yield return new object[] { "C4E18B 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM14, Register.RBX };
				yield return new object[] { "C4E18F 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM14, Register.RBX };
				yield return new object[] { "C4C1CB 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.R11 };
				yield return new object[] { "C4C1CF 2A D3", 5, Code.VEX_Vcvtsi2sd_xmm_xmm_rm64, Register.XMM2, Register.XMM6, Register.R11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E28 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E48 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E68 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };

				yield return new object[] { "62 F14F08 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F28 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F48 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F68 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test16_CvtV_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14E18 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F14E38 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F14E58 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F14E78 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14F08 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F18 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F38 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F58 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F78 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E28 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E48 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E68 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };

				yield return new object[] { "62 F14F08 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F28 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F48 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F68 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test32_CvtV_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14E18 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F14E38 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F14E58 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F14E78 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14F08 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F18 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F38 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F58 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F78 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_EVEX_1_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F14E08 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E28 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E48 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14E68 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };

				yield return new object[] { "62 F1CE08 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F1CE28 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F1CE48 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F1CE68 2A 50 01", 7, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };

				yield return new object[] { "62 F14F08 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F28 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F48 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };
				yield return new object[] { "62 F14F68 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int32, 4, false };

				yield return new object[] { "62 F1CF08 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F1CF28 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F1CF48 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };
				yield return new object[] { "62 F1CF68 2A 50 01", 7, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Int64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_Reg_RegMem_EVEX_2_Data))]
		void Test64_CvtV_Reg_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F14E08 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E10E18 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM18, Register.XMM14, Register.EBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 714E30 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM10, Register.XMM22, Register.EBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 D14E58 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.R11D, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F14E78 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm32_er, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F1CE08 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E18E18 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM18, Register.XMM14, Register.RBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 71CE30 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM10, Register.XMM22, Register.RBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 D1CE58 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.R11, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1CE78 2A D3", 6, Code.EVEX_Vcvtsi2ss_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F14F08 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E10F18 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM18, Register.XMM14, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 714F30 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM10, Register.XMM22, Register.EBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 D14F58 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.R11D, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 F14F78 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm32, Register.XMM2, Register.XMM6, Register.EBX, Register.None, RoundingControl.None, false };

				yield return new object[] { "62 F1CF08 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.None, false };
				yield return new object[] { "62 E18F18 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM18, Register.XMM14, Register.RBX, Register.None, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 71CF30 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM10, Register.XMM22, Register.RBX, Register.None, RoundingControl.RoundDown, false };
				yield return new object[] { "62 D1CF58 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.R11, Register.None, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1CF78 2A D3", 6, Code.EVEX_Vcvtsi2sd_xmm_xmm_rm64_er, Register.XMM2, Register.XMM6, Register.RBX, Register.None, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovntV_RegMem_Reg_1_Data))]
		void Test16_MovntV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test16_MovntV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F2B 08", 3, Code.Movntps_m128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F2B 08", 4, Code.Movntpd_m128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2B 08", 4, Code.Movntss_m32_xmm, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F2B 08", 4, Code.Movntsd_m64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5F8 2B 10", 4, Code.VEX_Vmovntps_m128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 2B 10", 5, Code.VEX_Vmovntps_m128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 2B 10", 4, Code.VEX_Vmovntps_m256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 2B 10", 5, Code.VEX_Vmovntps_m256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 2B 10", 4, Code.VEX_Vmovntpd_m128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 2B 10", 5, Code.VEX_Vmovntpd_m128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 2B 10", 4, Code.VEX_Vmovntpd_m256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 2B 10", 5, Code.VEX_Vmovntpd_m256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovntV_RegMem_Reg_1_Data))]
		void Test32_MovntV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test32_MovntV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F2B 08", 3, Code.Movntps_m128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F2B 08", 4, Code.Movntpd_m128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2B 08", 4, Code.Movntss_m32_xmm, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "F2 0F2B 08", 4, Code.Movntsd_m64_xmm, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "C5F8 2B 10", 4, Code.VEX_Vmovntps_m128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 2B 10", 5, Code.VEX_Vmovntps_m128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 2B 10", 4, Code.VEX_Vmovntps_m256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 2B 10", 5, Code.VEX_Vmovntps_m256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 2B 10", 4, Code.VEX_Vmovntpd_m128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 2B 10", 5, Code.VEX_Vmovntpd_m128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 2B 10", 4, Code.VEX_Vmovntpd_m256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 2B 10", 5, Code.VEX_Vmovntpd_m256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovntV_RegMem_Reg_1_Data))]
		void Test64_MovntV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

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
		public static IEnumerable<object[]> Test64_MovntV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F2B 08", 3, Code.Movntps_m128_xmm, Register.XMM1, MemorySize.Packed128_Float32 };
				yield return new object[] { "44 0F2B 08", 4, Code.Movntps_m128_xmm, Register.XMM9, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F2B 08", 4, Code.Movntpd_m128_xmm, Register.XMM1, MemorySize.Packed128_Float64 };
				yield return new object[] { "66 44 0F2B 08", 5, Code.Movntpd_m128_xmm, Register.XMM9, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0F2B 08", 4, Code.Movntss_m32_xmm, Register.XMM1, MemorySize.Float32 };
				yield return new object[] { "F3 44 0F2B 08", 5, Code.Movntss_m32_xmm, Register.XMM9, MemorySize.Float32 };

				yield return new object[] { "F2 0F2B 08", 4, Code.Movntsd_m64_xmm, Register.XMM1, MemorySize.Float64 };
				yield return new object[] { "F2 44 0F2B 08", 5, Code.Movntsd_m64_xmm, Register.XMM9, MemorySize.Float64 };

				yield return new object[] { "C5F8 2B 10", 4, Code.VEX_Vmovntps_m128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C4E1F8 2B 10", 5, Code.VEX_Vmovntps_m128_xmm, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "C578 2B 10", 4, Code.VEX_Vmovntps_m128_xmm, Register.XMM10, MemorySize.Packed128_Float32 };

				yield return new object[] { "C5FC 2B 10", 4, Code.VEX_Vmovntps_m256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C4E1FC 2B 10", 5, Code.VEX_Vmovntps_m256_ymm, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "C57C 2B 10", 4, Code.VEX_Vmovntps_m256_ymm, Register.YMM10, MemorySize.Packed256_Float32 };

				yield return new object[] { "C5F9 2B 10", 4, Code.VEX_Vmovntpd_m128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 2B 10", 5, Code.VEX_Vmovntpd_m128_xmm, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C579 2B 10", 4, Code.VEX_Vmovntpd_m128_xmm, Register.XMM10, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD 2B 10", 4, Code.VEX_Vmovntpd_m256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD 2B 10", 5, Code.VEX_Vmovntpd_m256_ymm, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C57D 2B 10", 4, Code.VEX_Vmovntpd_m256_ymm, Register.YMM10, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovntV_RegMem_Reg_EVEX_1_Data))]
		void Test16_MovntV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovntV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 2B 50 01", 7, Code.EVEX_Vmovntps_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 2B 50 01", 7, Code.EVEX_Vmovntps_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 2B 50 01", 7, Code.EVEX_Vmovntps_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 2B 50 01", 7, Code.EVEX_Vmovntpd_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 2B 50 01", 7, Code.EVEX_Vmovntpd_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 2B 50 01", 7, Code.EVEX_Vmovntpd_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovntV_RegMem_Reg_EVEX_1_Data))]
		void Test32_MovntV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovntV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 2B 50 01", 7, Code.EVEX_Vmovntps_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 2B 50 01", 7, Code.EVEX_Vmovntps_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 2B 50 01", 7, Code.EVEX_Vmovntps_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 2B 50 01", 7, Code.EVEX_Vmovntpd_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 2B 50 01", 7, Code.EVEX_Vmovntpd_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 2B 50 01", 7, Code.EVEX_Vmovntpd_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovntV_RegMem_Reg_EVEX_1_Data))]
		void Test64_MovntV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovntV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17C08 2B 50 01", 7, Code.EVEX_Vmovntps_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 717C08 2B 50 01", 7, Code.EVEX_Vmovntps_m128_xmm, Register.XMM10, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 617C08 2B 50 01", 7, Code.EVEX_Vmovntps_m128_xmm, Register.XMM26, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F17C28 2B 50 01", 7, Code.EVEX_Vmovntps_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 717C28 2B 50 01", 7, Code.EVEX_Vmovntps_m256_ymm, Register.YMM10, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 617C28 2B 50 01", 7, Code.EVEX_Vmovntps_m256_ymm, Register.YMM26, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F17C48 2B 50 01", 7, Code.EVEX_Vmovntps_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 717C48 2B 50 01", 7, Code.EVEX_Vmovntps_m512_zmm, Register.ZMM10, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 617C48 2B 50 01", 7, Code.EVEX_Vmovntps_m512_zmm, Register.ZMM26, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F1FD08 2B 50 01", 7, Code.EVEX_Vmovntpd_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 71FD08 2B 50 01", 7, Code.EVEX_Vmovntpd_m128_xmm, Register.XMM10, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 61FD08 2B 50 01", 7, Code.EVEX_Vmovntpd_m128_xmm, Register.XMM26, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F1FD28 2B 50 01", 7, Code.EVEX_Vmovntpd_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 71FD28 2B 50 01", 7, Code.EVEX_Vmovntpd_m256_ymm, Register.YMM10, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 61FD28 2B 50 01", 7, Code.EVEX_Vmovntpd_m256_ymm, Register.YMM26, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F1FD48 2B 50 01", 7, Code.EVEX_Vmovntpd_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 71FD48 2B 50 01", 7, Code.EVEX_Vmovntpd_m512_zmm, Register.ZMM10, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 61FD48 2B 50 01", 7, Code.EVEX_Vmovntpd_m512_zmm, Register.ZMM26, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_er_1_Data))]
		void Test16_CvtV_Reg_RegMem_er_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_er_1_Data {
			get {
				yield return new object[] { "62 F17E08 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17E08 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_er_2_Data))]
		void Test16_CvtV_Reg_RegMem_er_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, RoundingControl rc, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_er_2_Data {
			get {
				yield return new object[] { "62 F17E08 2C D3", 6, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 2C D3", 6, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17F08 2C D3", 6, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 2C D3", 6, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17E08 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17E38 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17E58 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17E78 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F17F08 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17F38 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17F58 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17F78 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_er_1_Data))]
		void Test32_CvtV_Reg_RegMem_er_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_er_1_Data {
			get {
				yield return new object[] { "62 F17E08 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17E08 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_er_2_Data))]
		void Test32_CvtV_Reg_RegMem_er_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, RoundingControl rc, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_er_2_Data {
			get {
				yield return new object[] { "62 F17E08 2C D3", 6, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 2C D3", 6, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17F08 2C D3", 6, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 2C D3", 6, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, Register.XMM3, RoundingControl.None, true };

				yield return new object[] { "62 F17E08 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17E18 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17E38 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17E58 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17E78 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F17F08 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 F17F18 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 F17F38 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17F58 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17F78 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_er_1_Data))]
		void Test64_CvtV_Reg_RegMem_er_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_er_1_Data {
			get {
				yield return new object[] { "62 F17E08 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 2C 50 01", 7, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FE08 2C 50 01", 7, Code.EVEX_Vcvttss2si_r64_xmmm32_sae, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE28 2C 50 01", 7, Code.EVEX_Vcvttss2si_r64_xmmm32_sae, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE48 2C 50 01", 7, Code.EVEX_Vcvttss2si_r64_xmmm32_sae, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE68 2C 50 01", 7, Code.EVEX_Vcvttss2si_r64_xmmm32_sae, Register.RDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F1FF08 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r64_xmmm64_sae, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF28 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r64_xmmm64_sae, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF48 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r64_xmmm64_sae, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF68 2C 50 01", 7, Code.EVEX_Vcvttsd2si_r64_xmmm64_sae, Register.RDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17E08 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E28 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E48 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17E68 2D 50 01", 7, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FE08 2D 50 01", 7, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE28 2D 50 01", 7, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE48 2D 50 01", 7, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, MemorySize.Float32, 4 };
				yield return new object[] { "62 F1FE68 2D 50 01", 7, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, MemorySize.Float32, 4 };

				yield return new object[] { "62 F17F08 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F28 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F48 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F17F68 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, MemorySize.Float64, 8 };

				yield return new object[] { "62 F1FF08 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF28 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF48 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FF68 2D 50 01", 7, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_er_2_Data))]
		void Test64_CvtV_Reg_RegMem_er_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, RoundingControl rc, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_er_2_Data {
			get {
				yield return new object[] { "62 F17E08 2C D3", 6, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17E18 2C D3", 6, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.EDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 317E08 2C D3", 6, Code.EVEX_Vcvttss2si_r32_xmmm32_sae, Register.R10D, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F1FE08 2C D3", 6, Code.EVEX_Vcvttss2si_r64_xmmm32_sae, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FE18 2C D3", 6, Code.EVEX_Vcvttss2si_r64_xmmm32_sae, Register.RDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 31FE08 2C D3", 6, Code.EVEX_Vcvttss2si_r64_xmmm32_sae, Register.R10, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F17F08 2C D3", 6, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17F18 2C D3", 6, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.EDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 317F08 2C D3", 6, Code.EVEX_Vcvttsd2si_r32_xmmm64_sae, Register.R10D, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F1FF08 2C D3", 6, Code.EVEX_Vcvttsd2si_r64_xmmm64_sae, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FF18 2C D3", 6, Code.EVEX_Vcvttsd2si_r64_xmmm64_sae, Register.RDX, Register.XMM11, RoundingControl.None, true };
				yield return new object[] { "62 31FF08 2C D3", 6, Code.EVEX_Vcvttsd2si_r64_xmmm64_sae, Register.R10, Register.XMM19, RoundingControl.None, false };

				yield return new object[] { "62 F17E08 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17E18 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 317E38 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.R10D, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17E58 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17E78 2D D3", 6, Code.EVEX_Vcvtss2si_r32_xmmm32_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F1FE08 2D D3", 6, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FE18 2D D3", 6, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 31FE38 2D D3", 6, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.R10, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F1FE58 2D D3", 6, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1FE78 2D D3", 6, Code.EVEX_Vcvtss2si_r64_xmmm32_er, Register.RDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F17F08 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D17F18 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 317F38 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.R10D, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F17F58 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F17F78 2D D3", 6, Code.EVEX_Vcvtsd2si_r32_xmmm64_er, Register.EDX, Register.XMM3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F1FF08 2D D3", 6, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, Register.XMM3, RoundingControl.None, false };
				yield return new object[] { "62 D1FF18 2D D3", 6, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, Register.XMM11, RoundingControl.RoundToNearest, false };
				yield return new object[] { "62 31FF38 2D D3", 6, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.R10, Register.XMM19, RoundingControl.RoundDown, false };
				yield return new object[] { "62 F1FF58 2D D3", 6, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, Register.XMM3, RoundingControl.RoundUp, false };
				yield return new object[] { "62 F1FF78 2D D3", 6, Code.EVEX_Vcvtsd2si_r64_xmmm64_er, Register.RDX, Register.XMM3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ComiV_VX_WX_1_Data))]
		void Test16_ComiV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_ComiV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F2E 08", 3, Code.Ucomiss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "66 0F2E 08", 4, Code.Ucomisd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F2F 08", 3, Code.Comiss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "66 0F2F 08", 4, Code.Comisd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ComiV_VX_WX_2_Data))]
		void Test16_ComiV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_ComiV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F2E CD", 3, Code.Ucomiss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F2E CD", 4, Code.Ucomisd_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F2F CD", 3, Code.Comiss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F2F CD", 4, Code.Comisd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ComiV_VX_WX_1_Data))]
		void Test32_ComiV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_ComiV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F2E 08", 3, Code.Ucomiss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "66 0F2E 08", 4, Code.Ucomisd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F2F 08", 3, Code.Comiss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "66 0F2F 08", 4, Code.Comisd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ComiV_VX_WX_2_Data))]
		void Test32_ComiV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_ComiV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F2E CD", 3, Code.Ucomiss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F2E CD", 4, Code.Ucomisd_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "0F2F CD", 3, Code.Comiss_xmm_xmmm32, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F2F CD", 4, Code.Comisd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ComiV_VX_WX_1_Data))]
		void Test64_ComiV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_ComiV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F2E 08", 3, Code.Ucomiss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "66 0F2E 08", 4, Code.Ucomisd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };

				yield return new object[] { "0F2F 08", 3, Code.Comiss_xmm_xmmm32, Register.XMM1, MemorySize.Float32 };

				yield return new object[] { "66 0F2F 08", 4, Code.Comisd_xmm_xmmm64, Register.XMM1, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ComiV_VX_WX_2_Data))]
		void Test64_ComiV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_ComiV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F2E CD", 3, Code.Ucomiss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F2E CD", 4, Code.Ucomiss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F2E CD", 4, Code.Ucomiss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F2E CD", 4, Code.Ucomiss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F2E CD", 4, Code.Ucomisd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F2E CD", 5, Code.Ucomisd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F2E CD", 5, Code.Ucomisd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F2E CD", 5, Code.Ucomisd_xmm_xmmm64, Register.XMM9, Register.XMM13 };

				yield return new object[] { "0F2F CD", 3, Code.Comiss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "44 0F2F CD", 4, Code.Comiss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "41 0F2F CD", 4, Code.Comiss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "45 0F2F CD", 4, Code.Comiss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F2F CD", 4, Code.Comisd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F2F CD", 5, Code.Comisd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F2F CD", 5, Code.Comisd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F2F CD", 5, Code.Comisd_xmm_xmmm64, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcomiV_VX_WX_1_Data))]
		void Test16_VcomiV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VcomiV_VX_WX_1_Data {
			get {
				yield return new object[] { "C5F8 2E 10", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FC 2E 10", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1F8 2E 10", 5, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FC 2E 10", 5, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5F9 2E 10", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FD 2E 10", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 2E 10", 5, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FD 2E 10", 5, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5F8 2F 10", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FC 2F 10", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1F8 2F 10", 5, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FC 2F 10", 5, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5F9 2F 10", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FD 2F 10", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 2F 10", 5, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FD 2F 10", 5, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcomiV_VX_WX_2_Data))]
		void Test16_VcomiV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_VcomiV_VX_WX_2_Data {
			get {
				yield return new object[] { "C5F8 2E D3", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C5F9 2E D3", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C5F8 2F D3", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C5F9 2F D3", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcomiV_VX_WX_1_Data))]
		void Test32_VcomiV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VcomiV_VX_WX_1_Data {
			get {
				yield return new object[] { "C5F8 2E 10", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FC 2E 10", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1F8 2E 10", 5, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FC 2E 10", 5, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5F9 2E 10", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FD 2E 10", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 2E 10", 5, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FD 2E 10", 5, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5F8 2F 10", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FC 2F 10", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1F8 2F 10", 5, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FC 2F 10", 5, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5F9 2F 10", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FD 2F 10", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 2F 10", 5, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FD 2F 10", 5, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcomiV_VX_WX_2_Data))]
		void Test32_VcomiV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_VcomiV_VX_WX_2_Data {
			get {
				yield return new object[] { "C5F8 2E D3", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C5F9 2E D3", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C5F8 2F D3", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, Register.XMM3 };

				yield return new object[] { "C5F9 2F D3", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcomiV_VX_WX_1_Data))]
		void Test64_VcomiV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VcomiV_VX_WX_1_Data {
			get {
				yield return new object[] { "C5F8 2E 10", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FC 2E 10", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1F8 2E 10", 5, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FC 2E 10", 5, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5F9 2E 10", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FD 2E 10", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 2E 10", 5, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FD 2E 10", 5, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };

				yield return new object[] { "C5F8 2F 10", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C5FC 2F 10", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1F8 2F 10", 5, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "C4E1FC 2F 10", 5, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };

				yield return new object[] { "C5F9 2F 10", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C5FD 2F 10", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1F9 2F 10", 5, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "C4E1FD 2F 10", 5, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcomiV_VX_WX_2_Data))]
		void Test64_VcomiV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_VcomiV_VX_WX_2_Data {
			get {
				yield return new object[] { "C5F8 2E D3", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C578 2E D3", 4, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C4C178 2E D3", 5, Code.VEX_Vucomiss_xmm_xmmm32, Register.XMM2, Register.XMM11 };

				yield return new object[] { "C5F9 2E D3", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C579 2E D3", 4, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C4C179 2E D3", 5, Code.VEX_Vucomisd_xmm_xmmm64, Register.XMM2, Register.XMM11 };

				yield return new object[] { "C5F8 2F D3", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C578 2F D3", 4, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C4C178 2F D3", 5, Code.VEX_Vcomiss_xmm_xmmm32, Register.XMM2, Register.XMM11 };

				yield return new object[] { "C5F9 2F D3", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C579 2F D3", 4, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C4C179 2F D3", 5, Code.VEX_Vcomisd_xmm_xmmm64, Register.XMM2, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcomiV_VX_WX_sae_1_Data))]
		void Test16_VcomiV_VX_WX_sae_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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
		public static IEnumerable<object[]> Test16_VcomiV_VX_WX_sae_1_Data {
			get {
				yield return new object[] { "62 F17C08 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C28 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C48 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C68 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FD08 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD28 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD48 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD68 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17C08 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C28 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C48 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C68 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FD08 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD28 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD48 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD68 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VcomiV_VX_WX_sae_2_Data))]
		void Test16_VcomiV_VX_WX_sae_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VcomiV_VX_WX_sae_2_Data {
			get {
				yield return new object[] { "62 F17C08 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F17C18 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, true };

				yield return new object[] { "62 F1FD08 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F1FD18 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, true };

				yield return new object[] { "62 F17C08 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F17C18 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, true };

				yield return new object[] { "62 F1FD08 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F1FD18 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcomiV_VX_WX_sae_1_Data))]
		void Test32_VcomiV_VX_WX_sae_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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
		public static IEnumerable<object[]> Test32_VcomiV_VX_WX_sae_1_Data {
			get {
				yield return new object[] { "62 F17C08 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C28 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C48 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C68 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FD08 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD28 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD48 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD68 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17C08 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C28 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C48 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C68 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FD08 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD28 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD48 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD68 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VcomiV_VX_WX_sae_2_Data))]
		void Test32_VcomiV_VX_WX_sae_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VcomiV_VX_WX_sae_2_Data {
			get {
				yield return new object[] { "62 F17C08 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F17C18 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, true };

				yield return new object[] { "62 F1FD08 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F1FD18 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, true };

				yield return new object[] { "62 F17C08 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F17C18 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, true };

				yield return new object[] { "62 F1FD08 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 F1FD18 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcomiV_VX_WX_sae_1_Data))]
		void Test64_VcomiV_VX_WX_sae_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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
		public static IEnumerable<object[]> Test64_VcomiV_VX_WX_sae_1_Data {
			get {
				yield return new object[] { "62 F17C08 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 E17C08 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM18, MemorySize.Float32, 4 };
				yield return new object[] { "62 717C08 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM10, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C28 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C48 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C68 2E 50 01", 7, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FD08 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 E1FD08 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM18, MemorySize.Float64, 8 };
				yield return new object[] { "62 71FD08 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM10, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD28 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD48 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD68 2E 50 01", 7, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };

				yield return new object[] { "62 F17C08 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 E17C08 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM18, MemorySize.Float32, 4 };
				yield return new object[] { "62 717C08 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM10, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C28 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C48 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };
				yield return new object[] { "62 F17C68 2F 50 01", 7, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, MemorySize.Float32, 4 };

				yield return new object[] { "62 F1FD08 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 E1FD08 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM18, MemorySize.Float64, 8 };
				yield return new object[] { "62 71FD08 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM10, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD28 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD48 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
				yield return new object[] { "62 F1FD68 2F 50 01", 7, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, MemorySize.Float64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VcomiV_VX_WX_sae_2_Data))]
		void Test64_VcomiV_VX_WX_sae_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VcomiV_VX_WX_sae_2_Data {
			get {
				yield return new object[] { "62 F17C08 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 E17C18 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM18, Register.XMM3, true };
				yield return new object[] { "62 117C08 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM10, Register.XMM27, false };
				yield return new object[] { "62 B17C18 2E D3", 6, Code.EVEX_Vucomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM19, true };

				yield return new object[] { "62 F1FD08 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 E1FD18 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM18, Register.XMM3, true };
				yield return new object[] { "62 11FD08 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM10, Register.XMM27, false };
				yield return new object[] { "62 B1FD18 2E D3", 6, Code.EVEX_Vucomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM19, true };

				yield return new object[] { "62 F17C08 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 E17C18 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM18, Register.XMM3, true };
				yield return new object[] { "62 117C08 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM10, Register.XMM27, false };
				yield return new object[] { "62 B17C18 2F D3", 6, Code.EVEX_Vcomiss_xmm_xmmm32_sae, Register.XMM2, Register.XMM19, true };

				yield return new object[] { "62 F1FD08 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM3, false };
				yield return new object[] { "62 E1FD18 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM18, Register.XMM3, true };
				yield return new object[] { "62 11FD08 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM10, Register.XMM27, false };
				yield return new object[] { "62 B1FD18 2F D3", 6, Code.EVEX_Vcomisd_xmm_xmmm64_sae, Register.XMM2, Register.XMM19, true };
			}
		}
	}
}
