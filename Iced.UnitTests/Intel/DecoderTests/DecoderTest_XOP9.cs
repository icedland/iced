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
	public sealed class DecoderTest_XOP9 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_XOP_Hd_Ed_1_Data))]
		void Test16_XOP_Hd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_XOP_Hd_Ed_1_Data {
			get {
				yield return new object[] { "8FE948 01 08", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 08", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 10", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 10", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 18", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 18", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 20", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 20", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 28", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 28", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 30", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 30", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 38", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 38", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 02 08", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 02 08", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 02 30", 5, Code.XOP_Blci_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 02 30", 5, Code.XOP_Blci_r32_rm32, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_Hd_Ed_2_Data))]
		void Test16_XOP_Hd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_XOP_Hd_Ed_2_Data {
			get {
				yield return new object[] { "8FE948 01 CB", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 CB", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 D3", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 D3", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 DB", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 DB", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 E3", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 E3", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 EB", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 EB", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 F3", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 F3", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 FB", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 FB", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 02 CB", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 02 CB", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 02 F3", 5, Code.XOP_Blci_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 02 F3", 5, Code.XOP_Blci_r32_rm32, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_Hd_Ed_1_Data))]
		void Test32_XOP_Hd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_XOP_Hd_Ed_1_Data {
			get {
				yield return new object[] { "8FE948 01 08", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 08", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 10", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 10", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 18", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 18", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 20", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 20", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 28", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 28", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 30", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 30", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 01 38", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 38", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 02 08", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 02 08", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };

				yield return new object[] { "8FE948 02 30", 5, Code.XOP_Blci_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 02 30", 5, Code.XOP_Blci_r32_rm32, Register.ESI, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_Hd_Ed_2_Data))]
		void Test32_XOP_Hd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_XOP_Hd_Ed_2_Data {
			get {
				yield return new object[] { "8FE948 01 CB", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 CB", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 D3", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 D3", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 DB", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 DB", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 E3", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 E3", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 EB", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 EB", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 F3", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 F3", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 01 FB", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 01 FB", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 02 CB", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 02 CB", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, Register.EBX };

				yield return new object[] { "8FE948 02 F3", 5, Code.XOP_Blci_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FE9C8 02 F3", 5, Code.XOP_Blci_r32_rm32, Register.ESI, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_Hd_Ed_1_Data))]
		void Test64_XOP_Hd_Ed_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_XOP_Hd_Ed_1_Data {
			get {
				yield return new object[] { "8FE948 01 08", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 08", 5, Code.XOP_Blcfill_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 01 10", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 10", 5, Code.XOP_Blsfill_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 01 18", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 18", 5, Code.XOP_Blcs_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 01 20", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 20", 5, Code.XOP_Tzmsk_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 01 28", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 28", 5, Code.XOP_Blcic_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 01 30", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 30", 5, Code.XOP_Blsic_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 01 38", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 01 38", 5, Code.XOP_T1mskc_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 02 08", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 02 08", 5, Code.XOP_Blcmsk_r64_rm64, Register.RSI, MemorySize.UInt64 };

				yield return new object[] { "8FE948 02 30", 5, Code.XOP_Blci_r32_rm32, Register.ESI, MemorySize.UInt32 };
				yield return new object[] { "8FE9C8 02 30", 5, Code.XOP_Blci_r64_rm64, Register.RSI, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_Hd_Ed_2_Data))]
		void Test64_XOP_Hd_Ed_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_XOP_Hd_Ed_2_Data {
			get {
				yield return new object[] { "8FE948 01 CB", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 01 CB", 5, Code.XOP_Blcfill_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 01 CB", 5, Code.XOP_Blcfill_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 01 CB", 5, Code.XOP_Blcfill_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 01 CB", 5, Code.XOP_Blcfill_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 01 CB", 5, Code.XOP_Blcfill_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 01 D3", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 01 D3", 5, Code.XOP_Blsfill_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 01 D3", 5, Code.XOP_Blsfill_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 01 D3", 5, Code.XOP_Blsfill_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 01 D3", 5, Code.XOP_Blsfill_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 01 D3", 5, Code.XOP_Blsfill_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 01 DB", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 01 DB", 5, Code.XOP_Blcs_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 01 DB", 5, Code.XOP_Blcs_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 01 DB", 5, Code.XOP_Blcs_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 01 DB", 5, Code.XOP_Blcs_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 01 DB", 5, Code.XOP_Blcs_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 01 E3", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 01 E3", 5, Code.XOP_Tzmsk_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 01 E3", 5, Code.XOP_Tzmsk_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 01 E3", 5, Code.XOP_Tzmsk_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 01 E3", 5, Code.XOP_Tzmsk_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 01 E3", 5, Code.XOP_Tzmsk_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 01 EB", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 01 EB", 5, Code.XOP_Blcic_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 01 EB", 5, Code.XOP_Blcic_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 01 EB", 5, Code.XOP_Blcic_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 01 EB", 5, Code.XOP_Blcic_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 01 EB", 5, Code.XOP_Blcic_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 01 F3", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 01 F3", 5, Code.XOP_Blsic_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 01 F3", 5, Code.XOP_Blsic_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 01 F3", 5, Code.XOP_Blsic_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 01 F3", 5, Code.XOP_Blsic_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 01 F3", 5, Code.XOP_Blsic_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 01 FB", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 01 FB", 5, Code.XOP_T1mskc_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 01 FB", 5, Code.XOP_T1mskc_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 01 FB", 5, Code.XOP_T1mskc_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 01 FB", 5, Code.XOP_T1mskc_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 01 FB", 5, Code.XOP_T1mskc_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 02 CB", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 02 CB", 5, Code.XOP_Blcmsk_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 02 CB", 5, Code.XOP_Blcmsk_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 02 CB", 5, Code.XOP_Blcmsk_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 02 CB", 5, Code.XOP_Blcmsk_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 02 CB", 5, Code.XOP_Blcmsk_r64_rm64, Register.RSI, Register.R11 };

				yield return new object[] { "8FE948 02 F3", 5, Code.XOP_Blci_r32_rm32, Register.ESI, Register.EBX };
				yield return new object[] { "8FC908 02 F3", 5, Code.XOP_Blci_r32_rm32, Register.R14D, Register.R11D };
				yield return new object[] { "8F4948 02 F3", 5, Code.XOP_Blci_r32_rm32, Register.ESI, Register.R11D };

				yield return new object[] { "8FE9C8 02 F3", 5, Code.XOP_Blci_r64_rm64, Register.RSI, Register.RBX };
				yield return new object[] { "8FC988 02 F3", 5, Code.XOP_Blci_r64_rm64, Register.R14, Register.R11 };
				yield return new object[] { "8F49C8 02 F3", 5, Code.XOP_Blci_r64_rm64, Register.RSI, Register.R11 };

			}
		}

		[Theory]
		[MemberData(nameof(Test16_Xlwpcb_1_Data))]
		void Test16_Xlwpcb_1(string hexBytes, int byteLength, Code code, Register reg1) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test16_Xlwpcb_1_Data {
			get {
				yield return new object[] { "8FE978 12 C3", 5, Code.XOP_Llwpcb_r32, Register.EBX };
				yield return new object[] { "8FE9F8 12 C3", 5, Code.XOP_Llwpcb_r32, Register.EBX };

				yield return new object[] { "8FE978 12 CB", 5, Code.XOP_Slwpcb_r32, Register.EBX };
				yield return new object[] { "8FE9F8 12 CB", 5, Code.XOP_Slwpcb_r32, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Xlwpcb_1_Data))]
		void Test32_Xlwpcb_1(string hexBytes, int byteLength, Code code, Register reg1) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test32_Xlwpcb_1_Data {
			get {
				yield return new object[] { "8FE978 12 C3", 5, Code.XOP_Llwpcb_r32, Register.EBX };
				yield return new object[] { "8FE9F8 12 C3", 5, Code.XOP_Llwpcb_r32, Register.EBX };

				yield return new object[] { "8FE978 12 CB", 5, Code.XOP_Slwpcb_r32, Register.EBX };
				yield return new object[] { "8FE9F8 12 CB", 5, Code.XOP_Slwpcb_r32, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Xlwpcb_1_Data))]
		void Test64_Xlwpcb_1(string hexBytes, int byteLength, Code code, Register reg1) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test64_Xlwpcb_1_Data {
			get {
				yield return new object[] { "8FE978 12 C3", 5, Code.XOP_Llwpcb_r32, Register.EBX };
				yield return new object[] { "8FC978 12 C3", 5, Code.XOP_Llwpcb_r32, Register.R11D };

				yield return new object[] { "8FE9F8 12 C3", 5, Code.XOP_Llwpcb_r64, Register.RBX };
				yield return new object[] { "8FC9F8 12 C3", 5, Code.XOP_Llwpcb_r64, Register.R11 };

				yield return new object[] { "8FE978 12 CB", 5, Code.XOP_Slwpcb_r32, Register.EBX };
				yield return new object[] { "8FC978 12 CB", 5, Code.XOP_Slwpcb_r32, Register.R11D };

				yield return new object[] { "8FE9F8 12 CB", 5, Code.XOP_Slwpcb_r64, Register.RBX };
				yield return new object[] { "8FC9F8 12 CB", 5, Code.XOP_Slwpcb_r64, Register.R11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_VX_WX_1_Data))]
		void Test16_XOP_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_XOP_VX_WX_1_Data {
			get {
				yield return new object[] { "8FE978 80 10", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "8FE97C 80 10", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "8FE978 81 10", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "8FE97C 81 10", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "8FE978 82 10", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "8FE978 83 10", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "8FE978 C1 10", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C2 10", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C3 10", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C6 10", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 C7 10", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 CB 10", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "8FE978 D1 10", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D2 10", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D3 10", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D6 10", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE978 D7 10", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE978 DB 10", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE978 E1 10", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 E2 10", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 E3 10", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_VX_WX_2_Data))]
		void Test16_XOP_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_XOP_VX_WX_2_Data {
			get {
				yield return new object[] { "8FE978 80 CD", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE97C 80 CD", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "8FE978 81 CD", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE97C 81 CD", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "8FE978 82 CD", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 83 CD", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C1 CD", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C2 CD", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C3 CD", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C6 CD", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C7 CD", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 CB CD", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D1 CD", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D2 CD", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D3 CD", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D6 CD", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D7 CD", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 DB CD", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 E1 CD", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 E2 CD", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 E3 CD", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_WX_1_Data))]
		void Test32_XOP_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_XOP_VX_WX_1_Data {
			get {
				yield return new object[] { "8FE978 80 10", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "8FE97C 80 10", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "8FE978 81 10", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "8FE97C 81 10", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "8FE978 82 10", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "8FE978 83 10", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "8FE978 C1 10", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C2 10", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C3 10", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C6 10", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 C7 10", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 CB 10", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "8FE978 D1 10", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D2 10", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D3 10", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D6 10", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE978 D7 10", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE978 DB 10", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE978 E1 10", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 E2 10", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 E3 10", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_WX_2_Data))]
		void Test32_XOP_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_XOP_VX_WX_2_Data {
			get {
				yield return new object[] { "8FE978 80 CD", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE97C 80 CD", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "8FE978 81 CD", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE97C 81 CD", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "8FE978 82 CD", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 83 CD", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C1 CD", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C2 CD", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C3 CD", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C6 CD", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 C7 CD", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 CB CD", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D1 CD", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D2 CD", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D3 CD", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D6 CD", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 D7 CD", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 DB CD", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 E1 CD", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 E2 CD", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8FE978 E3 CD", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_WX_1_Data))]
		void Test64_XOP_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_XOP_VX_WX_1_Data {
			get {
				yield return new object[] { "8FE978 80 10", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float32 };
				yield return new object[] { "8FE97C 80 10", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float32 };
				yield return new object[] { "8FE978 81 10", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "8FE97C 81 10", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "8FE978 82 10", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM2, MemorySize.Float32 };
				yield return new object[] { "8FE978 83 10", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM2, MemorySize.Float64 };
				yield return new object[] { "8FE978 C1 10", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C2 10", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C3 10", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 C6 10", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 C7 10", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 CB 10", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "8FE978 D1 10", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D2 10", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D3 10", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE978 D6 10", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE978 D7 10", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE978 DB 10", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE978 E1 10", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE978 E2 10", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE978 E3 10", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_WX_2_Data))]
		void Test64_XOP_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_XOP_VX_WX_2_Data {
			get {
				yield return new object[] { "8FE978 80 CD", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 80 CD", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 80 CD", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 80 CD", 5, Code.XOP_Vfrczps_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE97C 80 CD", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "8F697C 80 CD", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "8FC97C 80 CD", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "8F497C 80 CD", 5, Code.XOP_Vfrczps_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "8FE978 81 CD", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 81 CD", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 81 CD", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 81 CD", 5, Code.XOP_Vfrczpd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE97C 81 CD", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM1, Register.YMM5 };
				yield return new object[] { "8F697C 81 CD", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM9, Register.YMM5 };
				yield return new object[] { "8FC97C 81 CD", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM1, Register.YMM13 };
				yield return new object[] { "8F497C 81 CD", 5, Code.XOP_Vfrczpd_ymm_ymmm256, Register.YMM9, Register.YMM13 };

				yield return new object[] { "8FE978 82 CD", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 82 CD", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 82 CD", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 82 CD", 5, Code.XOP_Vfrczss_xmm_xmmm32, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 83 CD", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 83 CD", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 83 CD", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 83 CD", 5, Code.XOP_Vfrczsd_xmm_xmmm64, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 C1 CD", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 C1 CD", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 C1 CD", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 C1 CD", 5, Code.XOP_Vphaddbw_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 C2 CD", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 C2 CD", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 C2 CD", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 C2 CD", 5, Code.XOP_Vphaddbd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 C3 CD", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 C3 CD", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 C3 CD", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 C3 CD", 5, Code.XOP_Vphaddbq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 C6 CD", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 C6 CD", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 C6 CD", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 C6 CD", 5, Code.XOP_Vphaddwd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 C7 CD", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 C7 CD", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 C7 CD", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 C7 CD", 5, Code.XOP_Vphaddwq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 CB CD", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 CB CD", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 CB CD", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 CB CD", 5, Code.XOP_Vphadddq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 D1 CD", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 D1 CD", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 D1 CD", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 D1 CD", 5, Code.XOP_Vphaddubw_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 D2 CD", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 D2 CD", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 D2 CD", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 D2 CD", 5, Code.XOP_Vphaddubd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 D3 CD", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 D3 CD", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 D3 CD", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 D3 CD", 5, Code.XOP_Vphaddubq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 D6 CD", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 D6 CD", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 D6 CD", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 D6 CD", 5, Code.XOP_Vphadduwd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 D7 CD", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 D7 CD", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 D7 CD", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 D7 CD", 5, Code.XOP_Vphadduwq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 DB CD", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 DB CD", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 DB CD", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 DB CD", 5, Code.XOP_Vphaddudq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 E1 CD", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 E1 CD", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 E1 CD", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 E1 CD", 5, Code.XOP_Vphsubbw_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 E2 CD", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 E2 CD", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 E2 CD", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 E2 CD", 5, Code.XOP_Vphsubwd_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "8FE978 E3 CD", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "8F6978 E3 CD", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "8FC978 E3 CD", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "8F4978 E3 CD", 5, Code.XOP_Vphsubdq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_VX_HX_WX_1_Data))]
		void Test16_XOP_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_XOP_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "8FE9C8 90 10", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE9C8 91 10", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE9C8 92 10", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE9C8 93 10", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "8FE9C8 94 10", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE9C8 95 10", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE9C8 96 10", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE9C8 97 10", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "8FE9C8 98 10", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE9C8 99 10", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE9C8 9A 10", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "8FE9C8 9B 10", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_VX_HX_WX_2_Data))]
		void Test16_XOP_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_XOP_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "8FE9C8 90 D3", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 91 D3", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 92 D3", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 93 D3", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 94 D3", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 95 D3", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 96 D3", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 97 D3", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 98 D3", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 99 D3", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 9A D3", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 9B D3", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_HX_WX_1_Data))]
		void Test32_XOP_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_XOP_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "8FE9C8 90 10", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE9C8 91 10", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE9C8 92 10", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE9C8 93 10", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "8FE9C8 94 10", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE9C8 95 10", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE9C8 96 10", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE9C8 97 10", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "8FE9C8 98 10", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE9C8 99 10", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE9C8 9A 10", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "8FE9C8 9B 10", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_HX_WX_2_Data))]
		void Test32_XOP_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_XOP_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "8FE9C8 90 D3", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 91 D3", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 92 D3", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 93 D3", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 94 D3", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 95 D3", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 96 D3", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 97 D3", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 98 D3", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 99 D3", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 9A D3", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FE9C8 9B D3", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_HX_WX_1_Data))]
		void Test64_XOP_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_XOP_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "8FE9C8 90 10", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE9C8 91 10", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE9C8 92 10", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE9C8 93 10", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "8FE9C8 94 10", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "8FE9C8 95 10", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "8FE9C8 96 10", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "8FE9C8 97 10", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "8FE9C8 98 10", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "8FE9C8 99 10", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "8FE9C8 9A 10", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "8FE9C8 9B 10", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_HX_WX_2_Data))]
		void Test64_XOP_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_XOP_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "8FE9C8 90 D3", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 90 D3", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 90 D3", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 90 D3", 5, Code.XOP_Vprotb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 91 D3", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 91 D3", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 91 D3", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 91 D3", 5, Code.XOP_Vprotw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 92 D3", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 92 D3", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 92 D3", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 92 D3", 5, Code.XOP_Vprotd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 93 D3", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 93 D3", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 93 D3", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 93 D3", 5, Code.XOP_Vprotq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 94 D3", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 94 D3", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 94 D3", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 94 D3", 5, Code.XOP_Vpshlb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 95 D3", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 95 D3", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 95 D3", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 95 D3", 5, Code.XOP_Vpshlw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 96 D3", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 96 D3", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 96 D3", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 96 D3", 5, Code.XOP_Vpshld_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 97 D3", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 97 D3", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 97 D3", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 97 D3", 5, Code.XOP_Vpshlq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 98 D3", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 98 D3", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 98 D3", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 98 D3", 5, Code.XOP_Vpshab_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 99 D3", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 99 D3", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 99 D3", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 99 D3", 5, Code.XOP_Vpshaw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 9A D3", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 9A D3", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 9A D3", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 9A D3", 5, Code.XOP_Vpshad_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };

				yield return new object[] { "8FE9C8 9B D3", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8F69C8 9B D3", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "8FC988 9B D3", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM11 };
				yield return new object[] { "8F49C8 9B D3", 5, Code.XOP_Vpshaq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_VX_WX_HX_1_Data))]
		void Test16_XOP_VX_WX_HX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, Register reg3) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_XOP_VX_WX_HX_1_Data {
			get {
				yield return new object[] { "8FE948 90 10", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt8, Register.XMM6 };
				yield return new object[] { "8FE948 91 10", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt16, Register.XMM6 };
				yield return new object[] { "8FE948 92 10", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt32, Register.XMM6 };
				yield return new object[] { "8FE948 93 10", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt64, Register.XMM6 };
				yield return new object[] { "8FE948 94 10", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt8, Register.XMM6 };
				yield return new object[] { "8FE948 95 10", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt16, Register.XMM6 };
				yield return new object[] { "8FE948 96 10", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt32, Register.XMM6 };
				yield return new object[] { "8FE948 97 10", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt64, Register.XMM6 };
				yield return new object[] { "8FE948 98 10", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int8, Register.XMM6 };
				yield return new object[] { "8FE948 99 10", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int16, Register.XMM6 };
				yield return new object[] { "8FE948 9A 10", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int32, Register.XMM6 };
				yield return new object[] { "8FE948 9B 10", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int64, Register.XMM6 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_VX_WX_HX_2_Data))]
		void Test16_XOP_VX_WX_HX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_XOP_VX_WX_HX_2_Data {
			get {
				yield return new object[] { "8FE948 90 D3", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 91 D3", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 92 D3", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 93 D3", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 94 D3", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 95 D3", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 96 D3", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 97 D3", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 98 D3", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 99 D3", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 9A D3", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 9B D3", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_WX_HX_1_Data))]
		void Test32_XOP_VX_WX_HX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, Register reg3) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_XOP_VX_WX_HX_1_Data {
			get {
				yield return new object[] { "8FE948 90 10", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt8, Register.XMM6 };
				yield return new object[] { "8FE948 91 10", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt16, Register.XMM6 };
				yield return new object[] { "8FE948 92 10", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt32, Register.XMM6 };
				yield return new object[] { "8FE948 93 10", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt64, Register.XMM6 };
				yield return new object[] { "8FE948 94 10", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt8, Register.XMM6 };
				yield return new object[] { "8FE948 95 10", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt16, Register.XMM6 };
				yield return new object[] { "8FE948 96 10", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt32, Register.XMM6 };
				yield return new object[] { "8FE948 97 10", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt64, Register.XMM6 };
				yield return new object[] { "8FE948 98 10", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int8, Register.XMM6 };
				yield return new object[] { "8FE948 99 10", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int16, Register.XMM6 };
				yield return new object[] { "8FE948 9A 10", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int32, Register.XMM6 };
				yield return new object[] { "8FE948 9B 10", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int64, Register.XMM6 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_VX_WX_HX_2_Data))]
		void Test32_XOP_VX_WX_HX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_XOP_VX_WX_HX_2_Data {
			get {
				yield return new object[] { "8FE948 90 D3", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 91 D3", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 92 D3", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 93 D3", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 94 D3", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 95 D3", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 96 D3", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 97 D3", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 98 D3", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 99 D3", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 9A D3", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FE948 9B D3", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_WX_HX_1_Data))]
		void Test64_XOP_VX_WX_HX_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, Register reg3) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_XOP_VX_WX_HX_1_Data {
			get {
				yield return new object[] { "8FE948 90 10", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt8, Register.XMM6 };
				yield return new object[] { "8FE948 91 10", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt16, Register.XMM6 };
				yield return new object[] { "8FE948 92 10", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt32, Register.XMM6 };
				yield return new object[] { "8FE948 93 10", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt64, Register.XMM6 };
				yield return new object[] { "8FE948 94 10", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt8, Register.XMM6 };
				yield return new object[] { "8FE948 95 10", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt16, Register.XMM6 };
				yield return new object[] { "8FE948 96 10", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt32, Register.XMM6 };
				yield return new object[] { "8FE948 97 10", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_UInt64, Register.XMM6 };
				yield return new object[] { "8FE948 98 10", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int8, Register.XMM6 };
				yield return new object[] { "8FE948 99 10", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int16, Register.XMM6 };
				yield return new object[] { "8FE948 9A 10", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int32, Register.XMM6 };
				yield return new object[] { "8FE948 9B 10", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM2, MemorySize.Packed128_Int64, Register.XMM6 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_VX_WX_HX_2_Data))]
		void Test64_XOP_VX_WX_HX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_XOP_VX_WX_HX_2_Data {
			get {
				yield return new object[] { "8FE948 90 D3", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 90 D3", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 90 D3", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 90 D3", 5, Code.XOP_Vprotb_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 91 D3", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 91 D3", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 91 D3", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 91 D3", 5, Code.XOP_Vprotw_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 92 D3", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 92 D3", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 92 D3", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 92 D3", 5, Code.XOP_Vprotd_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 93 D3", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 93 D3", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 93 D3", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 93 D3", 5, Code.XOP_Vprotq_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 94 D3", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 94 D3", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 94 D3", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 94 D3", 5, Code.XOP_Vpshlb_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 95 D3", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 95 D3", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 95 D3", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 95 D3", 5, Code.XOP_Vpshlw_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 96 D3", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 96 D3", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 96 D3", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 96 D3", 5, Code.XOP_Vpshld_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 97 D3", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 97 D3", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 97 D3", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 97 D3", 5, Code.XOP_Vpshlq_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 98 D3", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 98 D3", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 98 D3", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 98 D3", 5, Code.XOP_Vpshab_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 99 D3", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 99 D3", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 99 D3", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 99 D3", 5, Code.XOP_Vpshaw_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 9A D3", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 9A D3", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 9A D3", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 9A D3", 5, Code.XOP_Vpshad_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };

				yield return new object[] { "8FE948 9B D3", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8F6948 9B D3", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM10, Register.XMM3, Register.XMM6 };
				yield return new object[] { "8FC908 9B D3", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM2, Register.XMM11, Register.XMM14 };
				yield return new object[] { "8F4948 9B D3", 5, Code.XOP_Vpshaq_xmm_xmmm128_xmm, Register.XMM10, Register.XMM11, Register.XMM6 };
			}
		}
	}
}
