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
	public sealed class DecoderTest_2_0F18_0F1F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Prefetch_M_Reg_RegMem_1_Data))]
		void Test16_Prefetch_M_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test16_Prefetch_M_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F18 00", 3, Code.Prefetchnta_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 08", 3, Code.Prefetcht0_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 10", 3, Code.Prefetcht1_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 18", 3, Code.Prefetcht2_m8, MemorySize.UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Prefetch_M_Reg_RegMem_1_Data))]
		void Test32_Prefetch_M_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test32_Prefetch_M_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F18 00", 3, Code.Prefetchnta_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 08", 3, Code.Prefetcht0_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 10", 3, Code.Prefetcht1_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 18", 3, Code.Prefetcht2_m8, MemorySize.UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Prefetch_M_Reg_RegMem_1_Data))]
		void Test64_Prefetch_M_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
		}
		public static IEnumerable<object[]> Test64_Prefetch_M_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F18 00", 3, Code.Prefetchnta_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 08", 3, Code.Prefetcht0_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 10", 3, Code.Prefetcht1_m8, MemorySize.UInt8 };
				yield return new object[] { "0F18 18", 3, Code.Prefetcht2_m8, MemorySize.UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BND_Reg_RegMem_Reg_RegMem_1_Data))]
		void Test16_BND_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_BND_Reg_RegMem_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F1A 08", 3, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "66 0F1A 08", 4, Code.Bndmov_bnd_bndm64, Register.BND1, MemorySize.Bnd32 };

				yield return new object[] { "F3 0F1A 08", 4, Code.Bndcl_bnd_rm32, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "F2 0F1A 08", 4, Code.Bndcu_bnd_rm32, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "F3 0F1B 08", 4, Code.Bndmk_bnd_m32, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "F2 0F1B 08", 4, Code.Bndcn_bnd_rm32, Register.BND1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BND_Reg_RegMem_Reg_RegMem_2_Data))]
		void Test16_BND_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_BND_Reg_RegMem_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F1A CA", 4, Code.Bndmov_bnd_bndm64, Register.BND1, Register.BND2 };

				yield return new object[] { "F3 0F1A CA", 4, Code.Bndcl_bnd_rm32, Register.BND1, Register.EDX };

				yield return new object[] { "F2 0F1A CA", 4, Code.Bndcu_bnd_rm32, Register.BND1, Register.EDX };

				yield return new object[] { "F2 0F1B CA", 4, Code.Bndcn_bnd_rm32, Register.BND1, Register.EDX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BND_Reg_RegMem_Reg_RegMem_1_Data))]
		void Test32_BND_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_BND_Reg_RegMem_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F1A 08", 3, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "66 0F1A 08", 4, Code.Bndmov_bnd_bndm64, Register.BND1, MemorySize.Bnd32 };

				yield return new object[] { "F3 0F1A 08", 4, Code.Bndcl_bnd_rm32, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "F2 0F1A 08", 4, Code.Bndcu_bnd_rm32, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "F3 0F1B 08", 4, Code.Bndmk_bnd_m32, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "F2 0F1B 08", 4, Code.Bndcn_bnd_rm32, Register.BND1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BND_Reg_RegMem_Reg_RegMem_2_Data))]
		void Test32_BND_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_BND_Reg_RegMem_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F1A CA", 4, Code.Bndmov_bnd_bndm64, Register.BND1, Register.BND2 };

				yield return new object[] { "F3 0F1A CA", 4, Code.Bndcl_bnd_rm32, Register.BND1, Register.EDX };

				yield return new object[] { "F2 0F1A CA", 4, Code.Bndcu_bnd_rm32, Register.BND1, Register.EDX };

				yield return new object[] { "F2 0F1B CA", 4, Code.Bndcn_bnd_rm32, Register.BND1, Register.EDX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BND_Reg_RegMem_Reg_RegMem_1_Data))]
		void Test64_BND_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_BND_Reg_RegMem_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F1A 08", 3, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.UInt64 };
				yield return new object[] { "48 0F1A 08", 4, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.UInt64 };

				yield return new object[] { "66 0F1A 08", 4, Code.Bndmov_bnd_bndm128, Register.BND1, MemorySize.Bnd64 };
				yield return new object[] { "66 48 0F1A 08", 5, Code.Bndmov_bnd_bndm128, Register.BND1, MemorySize.Bnd64 };

				yield return new object[] { "F3 0F1A 08", 4, Code.Bndcl_bnd_rm64, Register.BND1, MemorySize.UInt64 };
				yield return new object[] { "F3 48 0F1A 08", 5, Code.Bndcl_bnd_rm64, Register.BND1, MemorySize.UInt64 };

				yield return new object[] { "F2 0F1A 08", 4, Code.Bndcu_bnd_rm64, Register.BND1, MemorySize.UInt64 };
				yield return new object[] { "F2 48 0F1A 08", 5, Code.Bndcu_bnd_rm64, Register.BND1, MemorySize.UInt64 };

				yield return new object[] { "F3 0F1B 08", 4, Code.Bndmk_bnd_m64, Register.BND1, MemorySize.UInt64 };
				yield return new object[] { "F3 48 0F1B 08", 5, Code.Bndmk_bnd_m64, Register.BND1, MemorySize.UInt64 };

				yield return new object[] { "F2 0F1B 08", 4, Code.Bndcn_bnd_rm64, Register.BND1, MemorySize.UInt64 };
				yield return new object[] { "F2 48 0F1B 08", 5, Code.Bndcn_bnd_rm64, Register.BND1, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BND_Reg_RegMem_Reg_RegMem_2_Data))]
		void Test64_BND_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_BND_Reg_RegMem_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F1A CA", 4, Code.Bndmov_bnd_bndm128, Register.BND1, Register.BND2 };
				yield return new object[] { "66 48 0F1A CA", 5, Code.Bndmov_bnd_bndm128, Register.BND1, Register.BND2 };

				yield return new object[] { "F3 0F1A CA", 4, Code.Bndcl_bnd_rm64, Register.BND1, Register.RDX };
				yield return new object[] { "F3 41 0F1A CA", 5, Code.Bndcl_bnd_rm64, Register.BND1, Register.R10 };
				yield return new object[] { "F3 48 0F1A CA", 5, Code.Bndcl_bnd_rm64, Register.BND1, Register.RDX };

				yield return new object[] { "F2 0F1A CA", 4, Code.Bndcu_bnd_rm64, Register.BND1, Register.RDX };
				yield return new object[] { "F2 41 0F1A CA", 5, Code.Bndcu_bnd_rm64, Register.BND1, Register.R10 };
				yield return new object[] { "F2 48 0F1A CA", 5, Code.Bndcu_bnd_rm64, Register.BND1, Register.RDX };

				yield return new object[] { "F2 0F1B CA", 4, Code.Bndcn_bnd_rm64, Register.BND1, Register.RDX };
				yield return new object[] { "F2 41 0F1B CA", 5, Code.Bndcn_bnd_rm64, Register.BND1, Register.R10 };
				yield return new object[] { "F2 48 0F1B CA", 5, Code.Bndcn_bnd_rm64, Register.BND1, Register.RDX };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BND_RegMem_Reg_RegMem_Reg_1_Data))]
		void Test16_BND_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(reg1, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_BND_RegMem_Reg_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F1B 08", 3, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "66 0F1B 08", 4, Code.Bndmov_bndm64_bnd, Register.BND1, MemorySize.Bnd32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BND_RegMem_Reg_RegMem_Reg_2_Data))]
		void Test16_BND_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_BND_RegMem_Reg_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F1B CA", 4, Code.Bndmov_bndm64_bnd, Register.BND2, Register.BND1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BND_RegMem_Reg_RegMem_Reg_1_Data))]
		void Test32_BND_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(reg1, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_BND_RegMem_Reg_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F1B 08", 3, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.UInt32 };

				yield return new object[] { "66 0F1B 08", 4, Code.Bndmov_bndm64_bnd, Register.BND1, MemorySize.Bnd32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BND_RegMem_Reg_RegMem_Reg_2_Data))]
		void Test32_BND_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_BND_RegMem_Reg_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F1B CA", 4, Code.Bndmov_bndm64_bnd, Register.BND2, Register.BND1 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BND_RegMem_Reg_RegMem_Reg_1_Data))]
		void Test64_BND_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize) {
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
			Assert.Equal(reg1, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_BND_RegMem_Reg_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F1B 08", 3, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.UInt64 };
				yield return new object[] { "48 0F1B 08", 4, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.UInt64 };

				yield return new object[] { "66 0F1B 08", 4, Code.Bndmov_bndm128_bnd, Register.BND1, MemorySize.Bnd64 };
				yield return new object[] { "66 48 0F1B 08", 5, Code.Bndmov_bndm128_bnd, Register.BND1, MemorySize.Bnd64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BND_RegMem_Reg_RegMem_Reg_2_Data))]
		void Test64_BND_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_BND_RegMem_Reg_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "66 0F1B CA", 4, Code.Bndmov_bndm128_bnd, Register.BND2, Register.BND1 };
				yield return new object[] { "66 48 0F1B CA", 5, Code.Bndmov_bndm128_bnd, Register.BND2, Register.BND1 };
			}
		}

		[Theory]
		[InlineData("0F1F 00", 3, Code.Nop_rm16)]
		void Test16_Grp_Ew_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F1F C1", 3, Code.Nop_rm16, Register.CX)]
		void Test16_Grp_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
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
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F1F 00", 4, Code.Nop_rm16)]
		void Test32_Grp_Ew_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F1F C1", 4, Code.Nop_rm16, Register.CX)]
		void Test32_Grp_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
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
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F1F 00", 4, Code.Nop_rm16)]
		void Test64_Grp_Ew_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F1F C1", 4, Code.Nop_rm16, Register.CX)]
		[InlineData("66 41 0F1F C1", 5, Code.Nop_rm16, Register.R9W)]
		void Test64_Grp_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
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
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F1F 00", 4, Code.Nop_rm32)]
		void Test16_Grp_Ed_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F1F C1", 4, Code.Nop_rm32, Register.ECX)]
		void Test16_Grp_Ed_2(string hexBytes, int byteLength, Code code, Register reg) {
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
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F1F 00", 3, Code.Nop_rm32)]
		void Test32_Grp_Ed_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F1F C1", 3, Code.Nop_rm32, Register.ECX)]
		void Test32_Grp_Ed_2(string hexBytes, int byteLength, Code code, Register reg) {
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
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F1F 00", 3, Code.Nop_rm32)]
		void Test64_Grp_Ed_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F1F C1", 3, Code.Nop_rm32, Register.ECX)]
		[InlineData("41 0F1F C1", 4, Code.Nop_rm32, Register.R9D)]
		void Test64_Grp_Ed_2(string hexBytes, int byteLength, Code code, Register reg) {
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
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("48 0F1F 00", 4, Code.Nop_rm64)]
		void Test64_Grp_Eq_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 0F1F C1", 4, Code.Nop_rm64, Register.RCX)]
		[InlineData("49 0F1F C1", 4, Code.Nop_rm64, Register.R9)]
		void Test64_Grp_Eq_2(string hexBytes, int byteLength, Code code, Register reg) {
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
			Assert.Equal(reg, instr.Op0Register);
		}
	}
}
