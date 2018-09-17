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
	public sealed class DecoderTest_XOP8 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_XOP_VX_HX_WX_Is4X_1_Data))]
		void Test16_XOP_VX_HX_WX_Is4X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_XOP_VX_HX_WX_Is4X_1_Data {
			get {
				yield return new object[] { "8FE848 85 10 40", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 86 10 40", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 87 10 40", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 8E 10 40", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 8F 10 40", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 95 10 40", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 96 10 40", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 97 10 40", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 9E 10 40", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 9F 10 40", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 A6 10 40", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 B6 10 40", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_VX_HX_WX_Is4X_2_Data))]
		void Test16_XOP_VX_HX_WX_Is4X_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_XOP_VX_HX_WX_Is4X_2_Data {
			get {
				yield return new object[] { "8FE848 85 D3 40", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 86 D3 40", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 87 D3 40", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 8E D3 40", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 8F D3 40", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 95 D3 40", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 96 D3 40", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 97 D3 40", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 9E D3 40", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 9F D3 40", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 A6 D3 40", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 B6 D3 40", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_HX_WX_Is4X_1_Data))]
		void Test32_XOP_VX_HX_WX_Is4X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_XOP_VX_HX_WX_Is4X_1_Data {
			get {
				yield return new object[] { "8FE848 85 10 40", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 86 10 40", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 87 10 40", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 8E 10 40", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 8F 10 40", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 95 10 40", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 96 10 40", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 97 10 40", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 9E 10 40", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 9F 10 40", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 A6 10 40", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 B6 10 40", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_HX_WX_Is4X_2_Data))]
		void Test32_XOP_VX_HX_WX_Is4X_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_XOP_VX_HX_WX_Is4X_2_Data {
			get {
				yield return new object[] { "8FE848 85 D3 40", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 86 D3 40", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 87 D3 40", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 8E D3 40", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 8F D3 40", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 95 D3 40", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 96 D3 40", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 97 D3 40", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 9E D3 40", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 9F D3 40", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 A6 D3 40", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE848 B6 D3 40", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_HX_WX_Is4X_1_Data))]
		void Test64_XOP_VX_HX_WX_Is4X_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_XOP_VX_HX_WX_Is4X_1_Data {
			get {
				yield return new object[] { "8FE848 85 10 40", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 86 10 40", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 87 10 40", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 8E 10 40", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 8F 10 40", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 95 10 40", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 96 10 40", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 97 10 40", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 9E 10 40", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 9F 10 40", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, Register.XMM4 };
				yield return new object[] { "8FE848 A6 10 40", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
				yield return new object[] { "8FE848 B6 10 40", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_HX_WX_Is4X_2_Data))]
		void Test64_XOP_VX_HX_WX_Is4X_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_XOP_VX_HX_WX_Is4X_2_Data {
			get {
				yield return new object[] { "8FE848 85 D3 40", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 85 D3 C0", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 85 D3 E0", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 85 D3 40", 6, Code.XOP_Vpmacssww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 86 D3 40", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 86 D3 C0", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 86 D3 E0", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 86 D3 40", 6, Code.XOP_Vpmacsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 87 D3 40", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 87 D3 C0", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 87 D3 E0", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 87 D3 40", 6, Code.XOP_Vpmacssdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 8E D3 40", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 8E D3 C0", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 8E D3 E0", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 8E D3 40", 6, Code.XOP_Vpmacssdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 8F D3 40", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 8F D3 C0", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 8F D3 E0", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 8F D3 40", 6, Code.XOP_Vpmacssdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 95 D3 40", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 95 D3 C0", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 95 D3 E0", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 95 D3 40", 6, Code.XOP_Vpmacsww_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 96 D3 40", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 96 D3 C0", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 96 D3 E0", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 96 D3 40", 6, Code.XOP_Vpmacswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 97 D3 40", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 97 D3 C0", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 97 D3 E0", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 97 D3 40", 6, Code.XOP_Vpmacsdql_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 9E D3 40", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 9E D3 C0", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 9E D3 E0", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 9E D3 40", 6, Code.XOP_Vpmacsdd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 9F D3 40", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 9F D3 C0", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 9F D3 E0", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 9F D3 40", 6, Code.XOP_Vpmacsdqh_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 A6 D3 40", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 A6 D3 C0", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 A6 D3 E0", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 A6 D3 40", 6, Code.XOP_Vpmadcsswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };

				yield return new object[] { "8FE848 B6 D3 40", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 B6 D3 C0", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 B6 D3 E0", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 B6 D3 40", 6, Code.XOP_Vpmadcswd_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpcmov_W0_1_Data))]
		void Test16_Vpcmov_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_Vpcmov_W0_1_Data {
			get {
				yield return new object[] { "8FE848 A2 10 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.UInt128, Register.XMM4 };
				yield return new object[] { "8FE84C A2 10 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.UInt256, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpcmov_W0_2_Data))]
		void Test16_Vpcmov_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_Vpcmov_W0_2_Data {
			get {
				yield return new object[] { "8FE848 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE84C A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpcmov_W0_1_Data))]
		void Test32_Vpcmov_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_Vpcmov_W0_1_Data {
			get {
				yield return new object[] { "8FE848 A2 10 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.UInt128, Register.XMM4 };
				yield return new object[] { "8FE84C A2 10 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.UInt256, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpcmov_W0_2_Data))]
		void Test32_Vpcmov_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_Vpcmov_W0_2_Data {
			get {
				yield return new object[] { "8FE848 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE84C A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpcmov_W0_1_Data))]
		void Test64_Vpcmov_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_Vpcmov_W0_1_Data {
			get {
				yield return new object[] { "8FE848 A2 10 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.UInt128, Register.XMM4 };
				yield return new object[] { "8FE84C A2 10 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, MemorySize.UInt256, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpcmov_W0_2_Data))]
		void Test64_Vpcmov_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_Vpcmov_W0_2_Data {
			get {
				yield return new object[] { "8FE848 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8FE84C A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM3, Register.YMM5 };
				yield return new object[] { "8F6848 A2 D3 C0", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8F684C A2 D3 D0", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM10, Register.YMM6, Register.YMM3, Register.YMM13 };
				yield return new object[] { "8FE808 A2 D3 E0", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FE80C A2 D3 80", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM14, Register.YMM3, Register.YMM8 };
				yield return new object[] { "8FC848 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
				yield return new object[] { "8FC84C A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymmm256_ymm, Register.YMM2, Register.YMM6, Register.YMM11, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpcmov_W1_1_Data))]
		void Test16_Vpcmov_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vpcmov_W1_1_Data {
			get {
				yield return new object[] { "8FE8C8 A2 10 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.UInt128 };
				yield return new object[] { "8FE8CC A2 10 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpcmov_W1_2_Data))]
		void Test16_Vpcmov_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_Vpcmov_W1_2_Data {
			get {
				yield return new object[] { "8FE8C8 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "8FE8CC A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpcmov_W1_1_Data))]
		void Test32_Vpcmov_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vpcmov_W1_1_Data {
			get {
				yield return new object[] { "8FE8C8 A2 10 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.UInt128 };
				yield return new object[] { "8FE8CC A2 10 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpcmov_W1_2_Data))]
		void Test32_Vpcmov_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_Vpcmov_W1_2_Data {
			get {
				yield return new object[] { "8FE8C8 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "8FE8CC A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpcmov_W1_1_Data))]
		void Test64_Vpcmov_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vpcmov_W1_1_Data {
			get {
				yield return new object[] { "8FE8C8 A2 10 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.UInt128 };
				yield return new object[] { "8FE8CC A2 10 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpcmov_W1_2_Data))]
		void Test64_Vpcmov_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_Vpcmov_W1_2_Data {
			get {
				yield return new object[] { "8FE8C8 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "8FE8CC A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM3 };
				yield return new object[] { "8F68C8 A2 D3 C0", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "8F68CC A2 D3 D0", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM13, Register.YMM3 };
				yield return new object[] { "8FE888 A2 D3 E0", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "8FE88C A2 D3 80", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM8, Register.YMM3 };
				yield return new object[] { "8FC8C8 A2 D3 40", 6, Code.XOP_Vpcmov_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
				yield return new object[] { "8FC8CC A2 D3 50", 6, Code.XOP_Vpcmov_ymm_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM5, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpperm_W0_1_Data))]
		void Test16_Vpperm_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_Vpperm_W0_1_Data {
			get {
				yield return new object[] { "8FE848 A3 10 40", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpperm_W0_2_Data))]
		void Test16_Vpperm_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_Vpperm_W0_2_Data {
			get {
				yield return new object[] { "8FE848 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpperm_W0_1_Data))]
		void Test32_Vpperm_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_Vpperm_W0_1_Data {
			get {
				yield return new object[] { "8FE848 A3 10 40", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpperm_W0_2_Data))]
		void Test32_Vpperm_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_Vpperm_W0_2_Data {
			get {
				yield return new object[] { "8FE848 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpperm_W0_1_Data))]
		void Test64_Vpperm_W0_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_Vpperm_W0_1_Data {
			get {
				yield return new object[] { "8FE848 A3 10 40", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpperm_W0_2_Data))]
		void Test64_Vpperm_W0_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_Vpperm_W0_2_Data {
			get {
				yield return new object[] { "8FE848 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM3, Register.XMM4 };
				yield return new object[] { "8F6848 A3 D3 C0", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM10, Register.XMM6, Register.XMM3, Register.XMM12 };
				yield return new object[] { "8FE808 A3 D3 E0", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM14, Register.XMM3, Register.XMM14 };
				yield return new object[] { "8FC848 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmmm128_xmm, Register.XMM2, Register.XMM6, Register.XMM11, Register.XMM4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpperm_W1_1_Data))]
		void Test16_Vpperm_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vpperm_W1_1_Data {
			get {
				yield return new object[] { "8FE8C8 A3 10 40", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpperm_W1_2_Data))]
		void Test16_Vpperm_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test16_Vpperm_W1_2_Data {
			get {
				yield return new object[] { "8FE8C8 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpperm_W1_1_Data))]
		void Test32_Vpperm_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vpperm_W1_1_Data {
			get {
				yield return new object[] { "8FE8C8 A3 10 40", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpperm_W1_2_Data))]
		void Test32_Vpperm_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test32_Vpperm_W1_2_Data {
			get {
				yield return new object[] { "8FE8C8 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpperm_W1_1_Data))]
		void Test64_Vpperm_W1_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, MemorySize memSize) {
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

			Assert.Equal(OpKind.Memory, instr.Op3Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vpperm_W1_1_Data {
			get {
				yield return new object[] { "8FE8C8 A3 10 40", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpperm_W1_2_Data))]
		void Test64_Vpperm_W1_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register reg4) {
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

			Assert.Equal(OpKind.Register, instr.Op3Kind);
			Assert.Equal(reg4, instr.Op3Register);
		}
		public static IEnumerable<object[]> Test64_Vpperm_W1_2_Data {
			get {
				yield return new object[] { "8FE8C8 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM3 };
				yield return new object[] { "8F68C8 A3 D3 C0", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM12, Register.XMM3 };
				yield return new object[] { "8FE888 A3 D3 E0", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM14, Register.XMM3 };
				yield return new object[] { "8FC8C8 A3 D3 40", 6, Code.XOP_Vpperm_xmm_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM4, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VprotV_VX_WX_Ib_1_Data))]
		void Test16_VprotV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VprotV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "8FE878 C0 10 A5", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "8FE878 C1 10 A5", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt16, 0xA5 };
				yield return new object[] { "8FE878 C2 10 A5", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt32, 0xA5 };
				yield return new object[] { "8FE878 C3 10 A5", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VprotV_VX_WX_Ib_2_Data))]
		void Test16_VprotV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VprotV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "8FE878 C0 D3 5A", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
				yield return new object[] { "8FE878 C1 D3 5A", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
				yield return new object[] { "8FE878 C2 D3 5A", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
				yield return new object[] { "8FE878 C3 D3 5A", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VprotV_VX_WX_Ib_1_Data))]
		void Test32_VprotV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VprotV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "8FE878 C0 10 A5", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "8FE878 C1 10 A5", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt16, 0xA5 };
				yield return new object[] { "8FE878 C2 10 A5", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt32, 0xA5 };
				yield return new object[] { "8FE878 C3 10 A5", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VprotV_VX_WX_Ib_2_Data))]
		void Test32_VprotV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VprotV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "8FE878 C0 D3 5A", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
				yield return new object[] { "8FE878 C1 D3 5A", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
				yield return new object[] { "8FE878 C2 D3 5A", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
				yield return new object[] { "8FE878 C3 D3 5A", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VprotV_VX_WX_Ib_1_Data))]
		void Test64_VprotV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VprotV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "8FE878 C0 10 A5", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "8FE878 C1 10 A5", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt16, 0xA5 };
				yield return new object[] { "8FE878 C2 10 A5", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt32, 0xA5 };
				yield return new object[] { "8FE878 C3 10 A5", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM2, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VprotV_VX_WX_Ib_2_Data))]
		void Test64_VprotV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VprotV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "8FE878 C0 D3 A5", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6878 C0 D3 5A", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0x5A };
				yield return new object[] { "8FC878 C0 D3 A5", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
				yield return new object[] { "8F4878 C0 D3 5A", 6, Code.XOP_Vprotb_xmm_xmmm128_imm8, Register.XMM10, Register.XMM11, 0x5A };

				yield return new object[] { "8FE878 C1 D3 A5", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6878 C1 D3 5A", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0x5A };
				yield return new object[] { "8FC878 C1 D3 A5", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
				yield return new object[] { "8F4878 C1 D3 5A", 6, Code.XOP_Vprotw_xmm_xmmm128_imm8, Register.XMM10, Register.XMM11, 0x5A };

				yield return new object[] { "8FE878 C2 D3 A5", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6878 C2 D3 5A", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0x5A };
				yield return new object[] { "8FC878 C2 D3 A5", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
				yield return new object[] { "8F4878 C2 D3 5A", 6, Code.XOP_Vprotd_xmm_xmmm128_imm8, Register.XMM10, Register.XMM11, 0x5A };

				yield return new object[] { "8FE878 C3 D3 A5", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM2, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6878 C3 D3 5A", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM10, Register.XMM3, 0x5A };
				yield return new object[] { "8FC878 C3 D3 A5", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM2, Register.XMM11, 0xA5 };
				yield return new object[] { "8F4878 C3 D3 5A", 6, Code.XOP_Vprotq_xmm_xmmm128_imm8, Register.XMM10, Register.XMM11, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcomV_VX_HX_WX_Ib_1_Data))]
		void Test16_VpcomV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpcomV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "8FE848 CC 10 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "8FE848 CD 10 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "8FE848 CE 10 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "8FE848 CF 10 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64, 0xA5 };
				yield return new object[] { "8FE848 EC 10 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "8FE848 ED 10 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16, 0xA5 };
				yield return new object[] { "8FE848 EE 10 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32, 0xA5 };
				yield return new object[] { "8FE848 EF 10 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcomV_VX_HX_WX_Ib_2_Data))]
		void Test16_VpcomV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpcomV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "8FE848 CC D3 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 CD D3 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 CE D3 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 CF D3 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 EC D3 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 ED D3 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 EE D3 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 EF D3 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcomV_VX_HX_WX_Ib_1_Data))]
		void Test32_VpcomV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpcomV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "8FE848 CC 10 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "8FE848 CD 10 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "8FE848 CE 10 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "8FE848 CF 10 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64, 0xA5 };
				yield return new object[] { "8FE848 EC 10 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "8FE848 ED 10 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16, 0xA5 };
				yield return new object[] { "8FE848 EE 10 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32, 0xA5 };
				yield return new object[] { "8FE848 EF 10 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcomV_VX_HX_WX_Ib_2_Data))]
		void Test32_VpcomV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpcomV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "8FE848 CC D3 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 CD D3 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 CE D3 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 CF D3 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 EC D3 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 ED D3 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 EE D3 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE848 EF D3 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcomV_VX_HX_WX_Ib_1_Data))]
		void Test64_VpcomV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpcomV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "8FE848 CC 10 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8, 0xA5 };
				yield return new object[] { "8FE848 CD 10 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16, 0xA5 };
				yield return new object[] { "8FE848 CE 10 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32, 0xA5 };
				yield return new object[] { "8FE848 CF 10 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64, 0xA5 };
				yield return new object[] { "8FE848 EC 10 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "8FE848 ED 10 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16, 0xA5 };
				yield return new object[] { "8FE848 EE 10 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32, 0xA5 };
				yield return new object[] { "8FE848 EF 10 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcomV_VX_HX_WX_Ib_2_Data))]
		void Test64_VpcomV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpcomV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "8FE848 CC D3 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 CC D3 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 CC D3 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 CC D3 A5", 6, Code.XOP_Vpcomb_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "8FE848 CD D3 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 CD D3 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 CD D3 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 CD D3 A5", 6, Code.XOP_Vpcomw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "8FE848 CE D3 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 CE D3 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 CE D3 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 CE D3 A5", 6, Code.XOP_Vpcomd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "8FE848 CF D3 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 CF D3 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 CF D3 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 CF D3 A5", 6, Code.XOP_Vpcomq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "8FE848 EC D3 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 EC D3 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 EC D3 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 EC D3 A5", 6, Code.XOP_Vpcomub_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "8FE848 ED D3 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 ED D3 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 ED D3 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 ED D3 A5", 6, Code.XOP_Vpcomuw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "8FE848 EE D3 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 EE D3 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 EE D3 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 EE D3 A5", 6, Code.XOP_Vpcomud_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "8FE848 EF D3 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8F6848 EF D3 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "8FE808 EF D3 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "8FC848 EF D3 A5", 6, Code.XOP_Vpcomuq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
			}
		}
	}
}
