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
	public sealed class DecoderTest_2_0F40_0F47 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Cmov_GVEV_1_Data))]
		void Test16_Cmov_GVEV_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Cmov_GVEV_1_Data {
			get {
				yield return new object[] { "0F40 18", 3, Code.Cmovo_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F41 18", 3, Code.Cmovno_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F42 18", 3, Code.Cmovb_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F43 18", 3, Code.Cmovae_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F44 18", 3, Code.Cmove_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F45 18", 3, Code.Cmovne_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F46 18", 3, Code.Cmovbe_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F47 18", 3, Code.Cmova_Gw_Ew, Register.BX, MemorySize.UInt16 };

				yield return new object[] { "66 0F40 18", 4, Code.Cmovo_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F41 18", 4, Code.Cmovno_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F42 18", 4, Code.Cmovb_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F43 18", 4, Code.Cmovae_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F44 18", 4, Code.Cmove_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F45 18", 4, Code.Cmovne_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F46 18", 4, Code.Cmovbe_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F47 18", 4, Code.Cmova_Gd_Ed, Register.EBX, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Cmov_GVEV_2_Data))]
		void Test16_Cmov_GVEV_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_Cmov_GVEV_2_Data {
			get {
				yield return new object[] { "0F40 CE", 3, Code.Cmovo_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F41 CE", 3, Code.Cmovno_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F42 CE", 3, Code.Cmovb_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F43 CE", 3, Code.Cmovae_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F44 CE", 3, Code.Cmove_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F45 CE", 3, Code.Cmovne_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F46 CE", 3, Code.Cmovbe_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F47 CE", 3, Code.Cmova_Gw_Ew, Register.CX, Register.SI };

				yield return new object[] { "66 0F40 CE", 4, Code.Cmovo_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F41 CE", 4, Code.Cmovno_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F42 CE", 4, Code.Cmovb_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F43 CE", 4, Code.Cmovae_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F44 CE", 4, Code.Cmove_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F45 CE", 4, Code.Cmovne_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F46 CE", 4, Code.Cmovbe_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F47 CE", 4, Code.Cmova_Gd_Ed, Register.ECX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cmov_GVEV_1_Data))]
		void Test32_Cmov_GVEV_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Cmov_GVEV_1_Data {
			get {
				yield return new object[] { "66 0F40 18", 4, Code.Cmovo_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F41 18", 4, Code.Cmovno_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F42 18", 4, Code.Cmovb_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F43 18", 4, Code.Cmovae_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F44 18", 4, Code.Cmove_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F45 18", 4, Code.Cmovne_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F46 18", 4, Code.Cmovbe_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F47 18", 4, Code.Cmova_Gw_Ew, Register.BX, MemorySize.UInt16 };

				yield return new object[] { "0F40 18", 3, Code.Cmovo_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F41 18", 3, Code.Cmovno_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F42 18", 3, Code.Cmovb_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F43 18", 3, Code.Cmovae_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F44 18", 3, Code.Cmove_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F45 18", 3, Code.Cmovne_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F46 18", 3, Code.Cmovbe_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F47 18", 3, Code.Cmova_Gd_Ed, Register.EBX, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Cmov_GVEV_2_Data))]
		void Test32_Cmov_GVEV_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_Cmov_GVEV_2_Data {
			get {
				yield return new object[] { "66 0F40 CE", 4, Code.Cmovo_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F41 CE", 4, Code.Cmovno_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F42 CE", 4, Code.Cmovb_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F43 CE", 4, Code.Cmovae_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F44 CE", 4, Code.Cmove_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F45 CE", 4, Code.Cmovne_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F46 CE", 4, Code.Cmovbe_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F47 CE", 4, Code.Cmova_Gw_Ew, Register.CX, Register.SI };

				yield return new object[] { "0F40 CE", 3, Code.Cmovo_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F41 CE", 3, Code.Cmovno_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F42 CE", 3, Code.Cmovb_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F43 CE", 3, Code.Cmovae_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F44 CE", 3, Code.Cmove_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F45 CE", 3, Code.Cmovne_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F46 CE", 3, Code.Cmovbe_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F47 CE", 3, Code.Cmova_Gd_Ed, Register.ECX, Register.ESI };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cmov_GVEV_1_Data))]
		void Test64_Cmov_GVEV_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Cmov_GVEV_1_Data {
			get {
				yield return new object[] { "66 0F40 18", 4, Code.Cmovo_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F41 18", 4, Code.Cmovno_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F42 18", 4, Code.Cmovb_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F43 18", 4, Code.Cmovae_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F44 18", 4, Code.Cmove_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F45 18", 4, Code.Cmovne_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F46 18", 4, Code.Cmovbe_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F47 18", 4, Code.Cmova_Gw_Ew, Register.BX, MemorySize.UInt16 };

				yield return new object[] { "0F40 18", 3, Code.Cmovo_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F41 18", 3, Code.Cmovno_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F42 18", 3, Code.Cmovb_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F43 18", 3, Code.Cmovae_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F44 18", 3, Code.Cmove_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F45 18", 3, Code.Cmovne_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F46 18", 3, Code.Cmovbe_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F47 18", 3, Code.Cmova_Gd_Ed, Register.EBX, MemorySize.UInt32 };

				yield return new object[] { "48 0F40 18", 4, Code.Cmovo_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F41 18", 4, Code.Cmovno_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F42 18", 4, Code.Cmovb_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F43 18", 4, Code.Cmovae_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F44 18", 4, Code.Cmove_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F45 18", 4, Code.Cmovne_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F46 18", 4, Code.Cmovbe_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F47 18", 4, Code.Cmova_Gq_Eq, Register.RBX, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Cmov_GVEV_2_Data))]
		void Test64_Cmov_GVEV_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_Cmov_GVEV_2_Data {
			get {
				yield return new object[] { "66 0F40 CE", 4, Code.Cmovo_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F41 CE", 4, Code.Cmovno_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F42 CE", 4, Code.Cmovb_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F43 CE", 4, Code.Cmovae_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F44 CE", 4, Code.Cmove_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F45 CE", 4, Code.Cmovne_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F46 CE", 4, Code.Cmovbe_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F47 CE", 4, Code.Cmova_Gw_Ew, Register.CX, Register.SI };

				yield return new object[] { "66 44 0F40 CE", 5, Code.Cmovo_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F41 CE", 5, Code.Cmovno_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F42 CE", 5, Code.Cmovb_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F43 CE", 5, Code.Cmovae_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F44 CE", 5, Code.Cmove_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F45 CE", 5, Code.Cmovne_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F46 CE", 5, Code.Cmovbe_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F47 CE", 5, Code.Cmova_Gw_Ew, Register.R9W, Register.SI };

				yield return new object[] { "66 41 0F40 CE", 5, Code.Cmovo_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F41 CE", 5, Code.Cmovno_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F42 CE", 5, Code.Cmovb_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F43 CE", 5, Code.Cmovae_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F44 CE", 5, Code.Cmove_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F45 CE", 5, Code.Cmovne_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F46 CE", 5, Code.Cmovbe_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F47 CE", 5, Code.Cmova_Gw_Ew, Register.CX, Register.R14W };

				yield return new object[] { "66 45 0F40 CE", 5, Code.Cmovo_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F41 CE", 5, Code.Cmovno_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F42 CE", 5, Code.Cmovb_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F43 CE", 5, Code.Cmovae_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F44 CE", 5, Code.Cmove_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F45 CE", 5, Code.Cmovne_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F46 CE", 5, Code.Cmovbe_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F47 CE", 5, Code.Cmova_Gw_Ew, Register.R9W, Register.R14W };

				yield return new object[] { "0F40 CE", 3, Code.Cmovo_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F41 CE", 3, Code.Cmovno_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F42 CE", 3, Code.Cmovb_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F43 CE", 3, Code.Cmovae_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F44 CE", 3, Code.Cmove_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F45 CE", 3, Code.Cmovne_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F46 CE", 3, Code.Cmovbe_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F47 CE", 3, Code.Cmova_Gd_Ed, Register.ECX, Register.ESI };

				yield return new object[] { "44 0F40 CE", 4, Code.Cmovo_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F41 CE", 4, Code.Cmovno_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F42 CE", 4, Code.Cmovb_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F43 CE", 4, Code.Cmovae_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F44 CE", 4, Code.Cmove_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F45 CE", 4, Code.Cmovne_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F46 CE", 4, Code.Cmovbe_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F47 CE", 4, Code.Cmova_Gd_Ed, Register.R9D, Register.ESI };

				yield return new object[] { "41 0F40 CE", 4, Code.Cmovo_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F41 CE", 4, Code.Cmovno_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F42 CE", 4, Code.Cmovb_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F43 CE", 4, Code.Cmovae_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F44 CE", 4, Code.Cmove_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F45 CE", 4, Code.Cmovne_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F46 CE", 4, Code.Cmovbe_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F47 CE", 4, Code.Cmova_Gd_Ed, Register.ECX, Register.R14D };

				yield return new object[] { "45 0F40 CE", 4, Code.Cmovo_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F41 CE", 4, Code.Cmovno_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F42 CE", 4, Code.Cmovb_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F43 CE", 4, Code.Cmovae_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F44 CE", 4, Code.Cmove_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F45 CE", 4, Code.Cmovne_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F46 CE", 4, Code.Cmovbe_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F47 CE", 4, Code.Cmova_Gd_Ed, Register.R9D, Register.R14D };

				yield return new object[] { "48 0F40 CE", 4, Code.Cmovo_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F41 CE", 4, Code.Cmovno_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F42 CE", 4, Code.Cmovb_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F43 CE", 4, Code.Cmovae_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F44 CE", 4, Code.Cmove_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F45 CE", 4, Code.Cmovne_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F46 CE", 4, Code.Cmovbe_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F47 CE", 4, Code.Cmova_Gq_Eq, Register.RCX, Register.RSI };

				yield return new object[] { "4C 0F40 CE", 4, Code.Cmovo_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F41 CE", 4, Code.Cmovno_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F42 CE", 4, Code.Cmovb_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F43 CE", 4, Code.Cmovae_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F44 CE", 4, Code.Cmove_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F45 CE", 4, Code.Cmovne_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F46 CE", 4, Code.Cmovbe_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F47 CE", 4, Code.Cmova_Gq_Eq, Register.R9, Register.RSI };

				yield return new object[] { "49 0F40 CE", 4, Code.Cmovo_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F41 CE", 4, Code.Cmovno_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F42 CE", 4, Code.Cmovb_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F43 CE", 4, Code.Cmovae_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F44 CE", 4, Code.Cmove_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F45 CE", 4, Code.Cmovne_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F46 CE", 4, Code.Cmovbe_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F47 CE", 4, Code.Cmova_Gq_Eq, Register.RCX, Register.R14 };

				yield return new object[] { "4D 0F40 CE", 4, Code.Cmovo_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F41 CE", 4, Code.Cmovno_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F42 CE", 4, Code.Cmovb_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F43 CE", 4, Code.Cmovae_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F44 CE", 4, Code.Cmove_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F45 CE", 4, Code.Cmovne_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F46 CE", 4, Code.Cmovbe_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F47 CE", 4, Code.Cmova_Gq_Eq, Register.R9, Register.R14 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Mask_VK_HK_RK_1_Data))]
		void Test16_Mask_VK_HK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Mask_VK_HK_RK_1_Data {
			get {
				yield return new object[] { "C5CC 41 D3", 4, Code.VEX_Kandw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 41 D3", 4, Code.VEX_Kandb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 41 D3", 5, Code.VEX_Kandq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 41 D3", 5, Code.VEX_Kandd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 42 D3", 4, Code.VEX_Kandnw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 42 D3", 4, Code.VEX_Kandnb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 42 D3", 5, Code.VEX_Kandnq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 42 D3", 5, Code.VEX_Kandnd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 45 D3", 4, Code.VEX_Korw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 45 D3", 4, Code.VEX_Korb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 45 D3", 5, Code.VEX_Korq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 45 D3", 5, Code.VEX_Kord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 46 D3", 4, Code.VEX_Kxnorw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 46 D3", 4, Code.VEX_Kxnorb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 46 D3", 5, Code.VEX_Kxnorq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 46 D3", 5, Code.VEX_Kxnord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 47 D3", 4, Code.VEX_Kxorw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 47 D3", 4, Code.VEX_Kxorb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 47 D3", 5, Code.VEX_Kxorq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 47 D3", 5, Code.VEX_Kxord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mask_VK_HK_RK_1_Data))]
		void Test32_Mask_VK_HK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Mask_VK_HK_RK_1_Data {
			get {
				yield return new object[] { "C5CC 41 D3", 4, Code.VEX_Kandw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 41 D3", 4, Code.VEX_Kandb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 41 D3", 5, Code.VEX_Kandq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 41 D3", 5, Code.VEX_Kandd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 42 D3", 4, Code.VEX_Kandnw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 42 D3", 4, Code.VEX_Kandnb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 42 D3", 5, Code.VEX_Kandnq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 42 D3", 5, Code.VEX_Kandnd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 45 D3", 4, Code.VEX_Korw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 45 D3", 4, Code.VEX_Korb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 45 D3", 5, Code.VEX_Korq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 45 D3", 5, Code.VEX_Kord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 46 D3", 4, Code.VEX_Kxnorw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 46 D3", 4, Code.VEX_Kxnorb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 46 D3", 5, Code.VEX_Kxnorq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 46 D3", 5, Code.VEX_Kxnord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 47 D3", 4, Code.VEX_Kxorw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 47 D3", 4, Code.VEX_Kxorb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 47 D3", 5, Code.VEX_Kxorq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 47 D3", 5, Code.VEX_Kxord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mask_VK_HK_RK_1_Data))]
		void Test64_Mask_VK_HK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Mask_VK_HK_RK_1_Data {
			get {
				yield return new object[] { "C5CC 41 D3", 4, Code.VEX_Kandw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 41 D3", 4, Code.VEX_Kandb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 41 D3", 5, Code.VEX_Kandq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 41 D3", 5, Code.VEX_Kandd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 42 D3", 4, Code.VEX_Kandnw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 42 D3", 4, Code.VEX_Kandnb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 42 D3", 5, Code.VEX_Kandnq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 42 D3", 5, Code.VEX_Kandnd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 45 D3", 4, Code.VEX_Korw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 45 D3", 4, Code.VEX_Korb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 45 D3", 5, Code.VEX_Korq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 45 D3", 5, Code.VEX_Kord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 46 D3", 4, Code.VEX_Kxnorw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 46 D3", 4, Code.VEX_Kxnorb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 46 D3", 5, Code.VEX_Kxnorq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 46 D3", 5, Code.VEX_Kxnord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 47 D3", 4, Code.VEX_Kxorw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 47 D3", 4, Code.VEX_Kxorb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 47 D3", 5, Code.VEX_Kxorq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 47 D3", 5, Code.VEX_Kxord_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Mask_VK_RK_1_Data))]
		void Test16_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 44 D3", 4, Code.VEX_Knotw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 44 D3", 4, Code.VEX_Knotb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 44 D3", 5, Code.VEX_Knotq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 44 D3", 5, Code.VEX_Knotd_VK_RK, Register.K2, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Mask_VK_RK_1_Data))]
		void Test32_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 44 D3", 4, Code.VEX_Knotw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 44 D3", 4, Code.VEX_Knotb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 44 D3", 5, Code.VEX_Knotq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 44 D3", 5, Code.VEX_Knotd_VK_RK, Register.K2, Register.K3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Mask_VK_RK_1_Data))]
		void Test64_Mask_VK_RK_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_Mask_VK_RK_1_Data {
			get {
				yield return new object[] { "C5F8 44 D3", 4, Code.VEX_Knotw_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C5F9 44 D3", 4, Code.VEX_Knotb_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F8 44 D3", 5, Code.VEX_Knotq_VK_RK, Register.K2, Register.K3 };
				yield return new object[] { "C4E1F9 44 D3", 5, Code.VEX_Knotd_VK_RK, Register.K2, Register.K3 };
			}
		}
	}
}
