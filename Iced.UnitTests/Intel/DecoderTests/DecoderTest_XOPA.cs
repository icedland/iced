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
	public sealed class DecoderTest_XOPA : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Bextr_Gd_Ed_Id_1_Data))]
		void Test16_Bextr_Gd_Ed_Id_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test16_Bextr_Gd_Ed_Id_1_Data {
			get {
				yield return new object[] { "8FEA78 10 10 A55A1234", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, MemorySize.UInt32, 0x34125AA5 };
				yield return new object[] { "8FEAF8 10 10 A55A1234", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, MemorySize.UInt32, 0x34125AA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Bextr_Gd_Ed_Id_2_Data))]
		void Test16_Bextr_Gd_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test16_Bextr_Gd_Ed_Id_2_Data {
			get {
				yield return new object[] { "8FEA78 10 D3 34125AA5", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FEAF8 10 D3 34125AA5", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, Register.EBX, 0xA55A1234 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Bextr_Gd_Ed_Id_1_Data))]
		void Test32_Bextr_Gd_Ed_Id_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test32_Bextr_Gd_Ed_Id_1_Data {
			get {
				yield return new object[] { "8FEA78 10 10 A55A1234", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, MemorySize.UInt32, 0x34125AA5 };
				yield return new object[] { "8FEAF8 10 10 A55A1234", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, MemorySize.UInt32, 0x34125AA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Bextr_Gd_Ed_Id_2_Data))]
		void Test32_Bextr_Gd_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test32_Bextr_Gd_Ed_Id_2_Data {
			get {
				yield return new object[] { "8FEA78 10 D3 34125AA5", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FEAF8 10 D3 34125AA5", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, Register.EBX, 0xA55A1234 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Bextr_Gd_Ed_Id_1_Data))]
		void Test64_Bextr_Gd_Ed_Id_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test64_Bextr_Gd_Ed_Id_1_Data {
			get {
				yield return new object[] { "8FEA78 10 10 A55A1234", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, MemorySize.UInt32, 0x34125AA5 };

				yield return new object[] { "8FEAF8 10 10 34125AA5", 9, Code.XOP_Bextr_r64_rm64_imm32, Register.RDX, MemorySize.UInt64, 0xA55A1234 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Bextr_Gd_Ed_Id_2_Data))]
		void Test64_Bextr_Gd_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test64_Bextr_Gd_Ed_Id_2_Data {
			get {
				yield return new object[] { "8FEA78 10 D3 34125AA5", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8F6A78 10 D3 A55A1234", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.R10D, Register.EBX, 0x34125AA5 };
				yield return new object[] { "8FCA78 10 D3 34125AA5", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.EDX, Register.R11D, 0xA55A1234 };
				yield return new object[] { "8F4A78 10 D3 A55A1234", 9, Code.XOP_Bextr_r32_rm32_imm32, Register.R10D, Register.R11D, 0x34125AA5 };

				yield return new object[] { "8FEAF8 10 D3 34125AA5", 9, Code.XOP_Bextr_r64_rm64_imm32, Register.RDX, Register.RBX, 0xA55A1234 };
				yield return new object[] { "8F6AF8 10 D3 A55A1234", 9, Code.XOP_Bextr_r64_rm64_imm32, Register.R10, Register.RBX, 0x34125AA5 };
				yield return new object[] { "8FCAF8 10 D3 34125AA5", 9, Code.XOP_Bextr_r64_rm64_imm32, Register.RDX, Register.R11, 0xA55A1234 };
				yield return new object[] { "8F4AF8 10 D3 A55A1234", 9, Code.XOP_Bextr_r64_rm64_imm32, Register.R10, Register.R11, 0x34125AA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_Hd_Ed_Id_1_Data))]
		void Test16_XOP_Hd_Ed_Id_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test16_XOP_Hd_Ed_Id_1_Data {
			get {
				yield return new object[] { "8FEA48 12 00 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 00 78563412", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0x12345678 };

				yield return new object[] { "8FEA48 12 08 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 08 78563412", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0x12345678 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XOP_Hd_Ed_Id_2_Data))]
		void Test16_XOP_Hd_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test16_XOP_Hd_Ed_Id_2_Data {
			get {
				yield return new object[] { "8FEA48 12 C3 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 C3 78563412", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, Register.EBX, 0x12345678 };

				yield return new object[] { "8FEA48 12 CB 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 CB 78563412", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, Register.EBX, 0x12345678 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_Hd_Ed_Id_1_Data))]
		void Test32_XOP_Hd_Ed_Id_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test32_XOP_Hd_Ed_Id_1_Data {
			get {
				yield return new object[] { "8FEA48 12 00 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 00 78563412", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0x12345678 };

				yield return new object[] { "8FEA48 12 08 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 08 78563412", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0x12345678 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XOP_Hd_Ed_Id_2_Data))]
		void Test32_XOP_Hd_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test32_XOP_Hd_Ed_Id_2_Data {
			get {
				yield return new object[] { "8FEA48 12 C3 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 C3 78563412", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, Register.EBX, 0x12345678 };

				yield return new object[] { "8FEA48 12 CB 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 CB 78563412", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, Register.EBX, 0x12345678 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_Hd_Ed_Id_1_Data))]
		void Test64_XOP_Hd_Ed_Id_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test64_XOP_Hd_Ed_Id_1_Data {
			get {
				yield return new object[] { "8FEA48 12 00 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 00 34125AA5", 9, Code.XOP_Lwpins_r64_rm32_imm32, Register.RSI, MemorySize.UInt32, 0xA55A1234 };

				yield return new object[] { "8FEA48 12 08 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, MemorySize.UInt32, 0xA55A1234 };
				yield return new object[] { "8FEAC8 12 08 34125AA5", 9, Code.XOP_Lwpval_r64_rm32_imm32, Register.RSI, MemorySize.UInt32, 0xA55A1234 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XOP_Hd_Ed_Id_2_Data))]
		void Test64_XOP_Hd_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op2Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}
		public static IEnumerable<object[]> Test64_XOP_Hd_Ed_Id_2_Data {
			get {
				yield return new object[] { "8FEA48 12 C3 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FCA08 12 C3 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.R14D, Register.R11D, 0xA55A1234 };
				yield return new object[] { "8F4A48 12 C3 34125AA5", 9, Code.XOP_Lwpins_r32_rm32_imm32, Register.ESI, Register.R11D, 0xA55A1234 };

				yield return new object[] { "8FEAC8 12 C3 34125AA5", 9, Code.XOP_Lwpins_r64_rm32_imm32, Register.RSI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FCA88 12 C3 34125AA5", 9, Code.XOP_Lwpins_r64_rm32_imm32, Register.R14, Register.R11D, 0xA55A1234 };
				yield return new object[] { "8F4AC8 12 C3 34125AA5", 9, Code.XOP_Lwpins_r64_rm32_imm32, Register.RSI, Register.R11D, 0xA55A1234 };

				yield return new object[] { "8FEA48 12 CB 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FCA08 12 CB 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.R14D, Register.R11D, 0xA55A1234 };
				yield return new object[] { "8F4A48 12 CB 34125AA5", 9, Code.XOP_Lwpval_r32_rm32_imm32, Register.ESI, Register.R11D, 0xA55A1234 };

				yield return new object[] { "8FEAC8 12 CB 34125AA5", 9, Code.XOP_Lwpval_r64_rm32_imm32, Register.RSI, Register.EBX, 0xA55A1234 };
				yield return new object[] { "8FCA88 12 CB 34125AA5", 9, Code.XOP_Lwpval_r64_rm32_imm32, Register.R14, Register.R11D, 0xA55A1234 };
				yield return new object[] { "8F4AC8 12 CB 34125AA5", 9, Code.XOP_Lwpval_r64_rm32_imm32, Register.RSI, Register.R11D, 0xA55A1234 };
			}
		}
	}
}
