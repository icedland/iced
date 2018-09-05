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
	public sealed class DecoderTest_3_0F38D8_0F38DF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_AesimcV_VX_WX_1_Data))]
		void Test16_AesimcV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_AesimcV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DB 08", 5, Code.Aesimc_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 DB 10", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 DB 10", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AesimcV_VX_WX_2_Data))]
		void Test16_AesimcV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AesimcV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DB CD", 5, Code.Aesimc_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 DB D3", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesimcV_VX_WX_1_Data))]
		void Test32_AesimcV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_AesimcV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DB 08", 5, Code.Aesimc_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 DB 10", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 DB 10", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesimcV_VX_WX_2_Data))]
		void Test32_AesimcV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AesimcV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DB CD", 5, Code.Aesimc_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 DB D3", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesimcV_VX_WX_1_Data))]
		void Test64_AesimcV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_AesimcV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DB 08", 5, Code.Aesimc_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C4E279 DB 10", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 DB 10", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesimcV_VX_WX_2_Data))]
		void Test64_AesimcV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AesimcV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DB CD", 5, Code.Aesimc_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DB CD", 6, Code.Aesimc_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DB CD", 6, Code.Aesimc_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DB CD", 6, Code.Aesimc_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 DB D3", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, Register.XMM3 };
				yield return new object[] { "C46279 DB D3", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM10, Register.XMM3 };
				yield return new object[] { "C4C279 DB D3", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM2, Register.XMM11 };
				yield return new object[] { "C44279 DB D3", 5, Code.VEX_Vaesimc_VX_WX, Register.XMM10, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AesEncDecV_VX_WX_1_Data))]
		void Test16_AesEncDecV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_AesEncDecV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DC 08", 5, Code.Aesenc_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DD 08", 5, Code.Aesenclast_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DE 08", 5, Code.Aesdec_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DF 08", 5, Code.Aesdeclast_VX_WX, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AesEncDecV_VX_WX_2_Data))]
		void Test16_AesEncDecV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_AesEncDecV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DC CD", 5, Code.Aesenc_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DD CD", 5, Code.Aesenclast_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DE CD", 5, Code.Aesdec_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DF CD", 5, Code.Aesdeclast_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesEncDecV_VX_WX_1_Data))]
		void Test32_AesEncDecV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_AesEncDecV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DC 08", 5, Code.Aesenc_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DD 08", 5, Code.Aesenclast_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DE 08", 5, Code.Aesdec_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DF 08", 5, Code.Aesdeclast_VX_WX, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesEncDecV_VX_WX_2_Data))]
		void Test32_AesEncDecV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_AesEncDecV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DC CD", 5, Code.Aesenc_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DD CD", 5, Code.Aesenclast_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DE CD", 5, Code.Aesdec_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F38DF CD", 5, Code.Aesdeclast_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesEncDecV_VX_WX_1_Data))]
		void Test64_AesEncDecV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_AesEncDecV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F38DC 08", 5, Code.Aesenc_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DD 08", 5, Code.Aesenclast_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DE 08", 5, Code.Aesdec_VX_WX, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "66 0F38DF 08", 5, Code.Aesdeclast_VX_WX, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesEncDecV_VX_WX_2_Data))]
		void Test64_AesEncDecV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_AesEncDecV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F38DC CD", 5, Code.Aesenc_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DC CD", 6, Code.Aesenc_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DC CD", 6, Code.Aesenc_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DC CD", 6, Code.Aesenc_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F38DD CD", 5, Code.Aesenclast_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DD CD", 6, Code.Aesenclast_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DD CD", 6, Code.Aesenclast_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DD CD", 6, Code.Aesenclast_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F38DE CD", 5, Code.Aesdec_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DE CD", 6, Code.Aesdec_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DE CD", 6, Code.Aesdec_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DE CD", 6, Code.Aesdec_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F38DF CD", 5, Code.Aesdeclast_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F38DF CD", 6, Code.Aesdeclast_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F38DF CD", 6, Code.Aesdeclast_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F38DF CD", 6, Code.Aesdeclast_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_VX_HX_WX_1_Data))]
		void Test16_VaesdecV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VaesdecV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 DC 10", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DC 10", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DD 10", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DD 10", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DE 10", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DE 10", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DF 10", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DF 10", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VaesdecV_VX_HX_WX_2_Data))]
		void Test16_VaesdecV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VaesdecV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 DC D3", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DD D3", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DE D3", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DF D3", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_VX_HX_WX_1_Data))]
		void Test32_VaesdecV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VaesdecV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 DC 10", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DC 10", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DD 10", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DD 10", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DE 10", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DE 10", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DF 10", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DF 10", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VaesdecV_VX_HX_WX_2_Data))]
		void Test32_VaesdecV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VaesdecV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 DC D3", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DD D3", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DE D3", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E249 DF D3", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_VX_HX_WX_1_Data))]
		void Test64_VaesdecV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VaesdecV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 DC 10", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DC 10", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DD 10", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DD 10", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DE 10", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DE 10", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };

				yield return new object[] { "C4E249 DF 10", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
				yield return new object[] { "C4E2C9 DF 10", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VaesdecV_VX_HX_WX_2_Data))]
		void Test64_VaesdecV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VaesdecV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 DC D3", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DC D3", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DC D3", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DC D3", 5, Code.VEX_Vaesenc_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E249 DD D3", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DD D3", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DD D3", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DD D3", 5, Code.VEX_Vaesenclast_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E249 DE D3", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DE D3", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DE D3", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DE D3", 5, Code.VEX_Vaesdec_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E249 DF D3", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 DF D3", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4C289 DF D3", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "C44249 DF D3", 5, Code.VEX_Vaesdeclast_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM11 };
			}
		}
	}
}
