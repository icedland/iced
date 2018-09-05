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
	public sealed class DecoderTest_2_0F48_0F4F : DecoderTest {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Cmov_GVEV_1_Data {
			get {
				yield return new object[] { "0F48 18", 3, Code.Cmovs_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F49 18", 3, Code.Cmovns_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F4A 18", 3, Code.Cmovp_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F4B 18", 3, Code.Cmovnp_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F4C 18", 3, Code.Cmovl_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F4D 18", 3, Code.Cmovge_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F4E 18", 3, Code.Cmovle_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "0F4F 18", 3, Code.Cmovg_Gw_Ew, Register.BX, MemorySize.UInt16 };

				yield return new object[] { "66 0F48 18", 4, Code.Cmovs_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F49 18", 4, Code.Cmovns_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F4A 18", 4, Code.Cmovp_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F4B 18", 4, Code.Cmovnp_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F4C 18", 4, Code.Cmovl_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F4D 18", 4, Code.Cmovge_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F4E 18", 4, Code.Cmovle_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "66 0F4F 18", 4, Code.Cmovg_Gd_Ed, Register.EBX, MemorySize.UInt32 };
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
				yield return new object[] { "0F48 CE", 3, Code.Cmovs_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F49 CE", 3, Code.Cmovns_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F4A CE", 3, Code.Cmovp_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F4B CE", 3, Code.Cmovnp_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F4C CE", 3, Code.Cmovl_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F4D CE", 3, Code.Cmovge_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F4E CE", 3, Code.Cmovle_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "0F4F CE", 3, Code.Cmovg_Gw_Ew, Register.CX, Register.SI };

				yield return new object[] { "66 0F48 CE", 4, Code.Cmovs_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F49 CE", 4, Code.Cmovns_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F4A CE", 4, Code.Cmovp_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F4B CE", 4, Code.Cmovnp_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F4C CE", 4, Code.Cmovl_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F4D CE", 4, Code.Cmovge_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F4E CE", 4, Code.Cmovle_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "66 0F4F CE", 4, Code.Cmovg_Gd_Ed, Register.ECX, Register.ESI };
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Cmov_GVEV_1_Data {
			get {
				yield return new object[] { "66 0F48 18", 4, Code.Cmovs_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F49 18", 4, Code.Cmovns_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4A 18", 4, Code.Cmovp_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4B 18", 4, Code.Cmovnp_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4C 18", 4, Code.Cmovl_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4D 18", 4, Code.Cmovge_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4E 18", 4, Code.Cmovle_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4F 18", 4, Code.Cmovg_Gw_Ew, Register.BX, MemorySize.UInt16 };

				yield return new object[] { "0F48 18", 3, Code.Cmovs_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F49 18", 3, Code.Cmovns_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4A 18", 3, Code.Cmovp_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4B 18", 3, Code.Cmovnp_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4C 18", 3, Code.Cmovl_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4D 18", 3, Code.Cmovge_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4E 18", 3, Code.Cmovle_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4F 18", 3, Code.Cmovg_Gd_Ed, Register.EBX, MemorySize.UInt32 };
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
				yield return new object[] { "66 0F48 CE", 4, Code.Cmovs_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F49 CE", 4, Code.Cmovns_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4A CE", 4, Code.Cmovp_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4B CE", 4, Code.Cmovnp_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4C CE", 4, Code.Cmovl_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4D CE", 4, Code.Cmovge_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4E CE", 4, Code.Cmovle_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4F CE", 4, Code.Cmovg_Gw_Ew, Register.CX, Register.SI };

				yield return new object[] { "0F48 CE", 3, Code.Cmovs_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F49 CE", 3, Code.Cmovns_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4A CE", 3, Code.Cmovp_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4B CE", 3, Code.Cmovnp_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4C CE", 3, Code.Cmovl_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4D CE", 3, Code.Cmovge_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4E CE", 3, Code.Cmovle_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4F CE", 3, Code.Cmovg_Gd_Ed, Register.ECX, Register.ESI };
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Cmov_GVEV_1_Data {
			get {
				yield return new object[] { "66 0F48 18", 4, Code.Cmovs_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F49 18", 4, Code.Cmovns_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4A 18", 4, Code.Cmovp_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4B 18", 4, Code.Cmovnp_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4C 18", 4, Code.Cmovl_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4D 18", 4, Code.Cmovge_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4E 18", 4, Code.Cmovle_Gw_Ew, Register.BX, MemorySize.UInt16 };
				yield return new object[] { "66 0F4F 18", 4, Code.Cmovg_Gw_Ew, Register.BX, MemorySize.UInt16 };

				yield return new object[] { "0F48 18", 3, Code.Cmovs_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F49 18", 3, Code.Cmovns_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4A 18", 3, Code.Cmovp_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4B 18", 3, Code.Cmovnp_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4C 18", 3, Code.Cmovl_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4D 18", 3, Code.Cmovge_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4E 18", 3, Code.Cmovle_Gd_Ed, Register.EBX, MemorySize.UInt32 };
				yield return new object[] { "0F4F 18", 3, Code.Cmovg_Gd_Ed, Register.EBX, MemorySize.UInt32 };

				yield return new object[] { "48 0F48 18", 4, Code.Cmovs_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F49 18", 4, Code.Cmovns_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F4A 18", 4, Code.Cmovp_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F4B 18", 4, Code.Cmovnp_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F4C 18", 4, Code.Cmovl_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F4D 18", 4, Code.Cmovge_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F4E 18", 4, Code.Cmovle_Gq_Eq, Register.RBX, MemorySize.UInt64 };
				yield return new object[] { "48 0F4F 18", 4, Code.Cmovg_Gq_Eq, Register.RBX, MemorySize.UInt64 };
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
				yield return new object[] { "66 0F48 CE", 4, Code.Cmovs_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F49 CE", 4, Code.Cmovns_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4A CE", 4, Code.Cmovp_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4B CE", 4, Code.Cmovnp_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4C CE", 4, Code.Cmovl_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4D CE", 4, Code.Cmovge_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4E CE", 4, Code.Cmovle_Gw_Ew, Register.CX, Register.SI };
				yield return new object[] { "66 0F4F CE", 4, Code.Cmovg_Gw_Ew, Register.CX, Register.SI };

				yield return new object[] { "66 44 0F48 CE", 5, Code.Cmovs_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F49 CE", 5, Code.Cmovns_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F4A CE", 5, Code.Cmovp_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F4B CE", 5, Code.Cmovnp_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F4C CE", 5, Code.Cmovl_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F4D CE", 5, Code.Cmovge_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F4E CE", 5, Code.Cmovle_Gw_Ew, Register.R9W, Register.SI };
				yield return new object[] { "66 44 0F4F CE", 5, Code.Cmovg_Gw_Ew, Register.R9W, Register.SI };

				yield return new object[] { "66 41 0F48 CE", 5, Code.Cmovs_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F49 CE", 5, Code.Cmovns_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F4A CE", 5, Code.Cmovp_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F4B CE", 5, Code.Cmovnp_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F4C CE", 5, Code.Cmovl_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F4D CE", 5, Code.Cmovge_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F4E CE", 5, Code.Cmovle_Gw_Ew, Register.CX, Register.R14W };
				yield return new object[] { "66 41 0F4F CE", 5, Code.Cmovg_Gw_Ew, Register.CX, Register.R14W };

				yield return new object[] { "66 45 0F48 CE", 5, Code.Cmovs_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F49 CE", 5, Code.Cmovns_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F4A CE", 5, Code.Cmovp_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F4B CE", 5, Code.Cmovnp_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F4C CE", 5, Code.Cmovl_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F4D CE", 5, Code.Cmovge_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F4E CE", 5, Code.Cmovle_Gw_Ew, Register.R9W, Register.R14W };
				yield return new object[] { "66 45 0F4F CE", 5, Code.Cmovg_Gw_Ew, Register.R9W, Register.R14W };

				yield return new object[] { "0F48 CE", 3, Code.Cmovs_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F49 CE", 3, Code.Cmovns_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4A CE", 3, Code.Cmovp_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4B CE", 3, Code.Cmovnp_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4C CE", 3, Code.Cmovl_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4D CE", 3, Code.Cmovge_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4E CE", 3, Code.Cmovle_Gd_Ed, Register.ECX, Register.ESI };
				yield return new object[] { "0F4F CE", 3, Code.Cmovg_Gd_Ed, Register.ECX, Register.ESI };

				yield return new object[] { "44 0F48 CE", 4, Code.Cmovs_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F49 CE", 4, Code.Cmovns_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F4A CE", 4, Code.Cmovp_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F4B CE", 4, Code.Cmovnp_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F4C CE", 4, Code.Cmovl_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F4D CE", 4, Code.Cmovge_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F4E CE", 4, Code.Cmovle_Gd_Ed, Register.R9D, Register.ESI };
				yield return new object[] { "44 0F4F CE", 4, Code.Cmovg_Gd_Ed, Register.R9D, Register.ESI };

				yield return new object[] { "41 0F48 CE", 4, Code.Cmovs_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F49 CE", 4, Code.Cmovns_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F4A CE", 4, Code.Cmovp_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F4B CE", 4, Code.Cmovnp_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F4C CE", 4, Code.Cmovl_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F4D CE", 4, Code.Cmovge_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F4E CE", 4, Code.Cmovle_Gd_Ed, Register.ECX, Register.R14D };
				yield return new object[] { "41 0F4F CE", 4, Code.Cmovg_Gd_Ed, Register.ECX, Register.R14D };

				yield return new object[] { "45 0F48 CE", 4, Code.Cmovs_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F49 CE", 4, Code.Cmovns_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F4A CE", 4, Code.Cmovp_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F4B CE", 4, Code.Cmovnp_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F4C CE", 4, Code.Cmovl_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F4D CE", 4, Code.Cmovge_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F4E CE", 4, Code.Cmovle_Gd_Ed, Register.R9D, Register.R14D };
				yield return new object[] { "45 0F4F CE", 4, Code.Cmovg_Gd_Ed, Register.R9D, Register.R14D };

				yield return new object[] { "48 0F48 CE", 4, Code.Cmovs_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F49 CE", 4, Code.Cmovns_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F4A CE", 4, Code.Cmovp_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F4B CE", 4, Code.Cmovnp_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F4C CE", 4, Code.Cmovl_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F4D CE", 4, Code.Cmovge_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F4E CE", 4, Code.Cmovle_Gq_Eq, Register.RCX, Register.RSI };
				yield return new object[] { "48 0F4F CE", 4, Code.Cmovg_Gq_Eq, Register.RCX, Register.RSI };

				yield return new object[] { "4C 0F48 CE", 4, Code.Cmovs_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F49 CE", 4, Code.Cmovns_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F4A CE", 4, Code.Cmovp_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F4B CE", 4, Code.Cmovnp_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F4C CE", 4, Code.Cmovl_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F4D CE", 4, Code.Cmovge_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F4E CE", 4, Code.Cmovle_Gq_Eq, Register.R9, Register.RSI };
				yield return new object[] { "4C 0F4F CE", 4, Code.Cmovg_Gq_Eq, Register.R9, Register.RSI };

				yield return new object[] { "49 0F48 CE", 4, Code.Cmovs_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F49 CE", 4, Code.Cmovns_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F4A CE", 4, Code.Cmovp_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F4B CE", 4, Code.Cmovnp_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F4C CE", 4, Code.Cmovl_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F4D CE", 4, Code.Cmovge_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F4E CE", 4, Code.Cmovle_Gq_Eq, Register.RCX, Register.R14 };
				yield return new object[] { "49 0F4F CE", 4, Code.Cmovg_Gq_Eq, Register.RCX, Register.R14 };

				yield return new object[] { "4D 0F48 CE", 4, Code.Cmovs_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F49 CE", 4, Code.Cmovns_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F4A CE", 4, Code.Cmovp_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F4B CE", 4, Code.Cmovnp_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F4C CE", 4, Code.Cmovl_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F4D CE", 4, Code.Cmovge_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F4E CE", 4, Code.Cmovle_Gq_Eq, Register.R9, Register.R14 };
				yield return new object[] { "4D 0F4F CE", 4, Code.Cmovg_Gq_Eq, Register.R9, Register.R14 };
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
				yield return new object[] { "C5CC 4A D3", 4, Code.VEX_Kaddw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 4A D3", 4, Code.VEX_Kaddb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 4A D3", 5, Code.VEX_Kaddq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 4A D3", 5, Code.VEX_Kaddd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 4B D3", 4, Code.VEX_Kunpckwd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 4B D3", 4, Code.VEX_Kunpckbw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 4B D3", 5, Code.VEX_Kunpckdq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
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
				yield return new object[] { "C5CC 4A D3", 4, Code.VEX_Kaddw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 4A D3", 4, Code.VEX_Kaddb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 4A D3", 5, Code.VEX_Kaddq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 4A D3", 5, Code.VEX_Kaddd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 4B D3", 4, Code.VEX_Kunpckwd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 4B D3", 4, Code.VEX_Kunpckbw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 4B D3", 5, Code.VEX_Kunpckdq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
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
				yield return new object[] { "C5CC 4A D3", 4, Code.VEX_Kaddw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 4A D3", 4, Code.VEX_Kaddb_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 4A D3", 5, Code.VEX_Kaddq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CD 4A D3", 5, Code.VEX_Kaddd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };

				yield return new object[] { "C5CC 4B D3", 4, Code.VEX_Kunpckwd_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C5CD 4B D3", 4, Code.VEX_Kunpckbw_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
				yield return new object[] { "C4E1CC 4B D3", 5, Code.VEX_Kunpckdq_VK_HK_RK, Register.K2, Register.K6, Register.K3 };
			}
		}
	}
}
