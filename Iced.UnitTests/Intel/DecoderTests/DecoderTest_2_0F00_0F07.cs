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

using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class DecoderTest_2_0F00_0F07 : DecoderTest {
		[Theory]
		[InlineData("0F00 C1", 3, Code.Sldt_Ew, Register.CX)]
		[InlineData("0F00 CA", 3, Code.Str_Ew, Register.DX)]
		[InlineData("0F00 D3", 3, Code.Lldt_Ew, Register.BX)]
		[InlineData("0F00 DC", 3, Code.Ltr_Ew, Register.SP)]
		[InlineData("0F00 E5", 3, Code.Verr_Ew, Register.BP)]
		[InlineData("0F00 EE", 3, Code.Verw_Ew, Register.SI)]
		void Test16_Grp6_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("0F00 00", 3, Code.Sldt_Ew)]
		[InlineData("0F00 08", 3, Code.Str_Ew)]
		[InlineData("0F00 10", 3, Code.Lldt_Ew)]
		[InlineData("0F00 18", 3, Code.Ltr_Ew)]
		[InlineData("0F00 20", 3, Code.Verr_Ew)]
		[InlineData("0F00 28", 3, Code.Verw_Ew)]
		void Test16_Grp6_Ew_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F00 C1", 4, Code.Sldt_Ew, Register.CX)]
		[InlineData("66 0F00 CA", 4, Code.Str_Ew, Register.DX)]
		[InlineData("66 0F00 D3", 4, Code.Lldt_Ew, Register.BX)]
		[InlineData("66 0F00 DC", 4, Code.Ltr_Ew, Register.SP)]
		[InlineData("66 0F00 E5", 4, Code.Verr_Ew, Register.BP)]
		[InlineData("66 0F00 EE", 4, Code.Verw_Ew, Register.SI)]
		void Test32_Grp6_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F00 00", 4, Code.Sldt_Ew)]
		[InlineData("66 0F00 08", 4, Code.Str_Ew)]
		[InlineData("66 0F00 10", 4, Code.Lldt_Ew)]
		[InlineData("66 0F00 18", 4, Code.Ltr_Ew)]
		[InlineData("66 0F00 20", 4, Code.Verr_Ew)]
		[InlineData("66 0F00 28", 4, Code.Verw_Ew)]
		void Test32_Grp6_Ew_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F00 C1", 4, Code.Sldt_Ew, Register.CX)]
		[InlineData("66 0F00 CA", 4, Code.Str_Ew, Register.DX)]
		[InlineData("66 0F00 D3", 4, Code.Lldt_Ew, Register.BX)]
		[InlineData("66 0F00 DC", 4, Code.Ltr_Ew, Register.SP)]
		[InlineData("66 0F00 E5", 4, Code.Verr_Ew, Register.BP)]
		[InlineData("66 0F00 EE", 4, Code.Verw_Ew, Register.SI)]

		[InlineData("66 41 0F00 C1", 5, Code.Sldt_Ew, Register.R9W)]
		[InlineData("66 41 0F00 CA", 5, Code.Str_Ew, Register.R10W)]
		[InlineData("66 41 0F00 D3", 5, Code.Lldt_Ew, Register.R11W)]
		[InlineData("66 41 0F00 DC", 5, Code.Ltr_Ew, Register.R12W)]
		[InlineData("66 41 0F00 E5", 5, Code.Verr_Ew, Register.R13W)]
		[InlineData("66 41 0F00 EE", 5, Code.Verw_Ew, Register.R14W)]
		void Test64_Grp6_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F00 00", 4, Code.Sldt_Ew)]
		[InlineData("66 0F00 08", 4, Code.Str_Ew)]
		[InlineData("66 0F00 10", 4, Code.Lldt_Ew)]
		[InlineData("66 0F00 18", 4, Code.Ltr_Ew)]
		[InlineData("66 0F00 20", 4, Code.Verr_Ew)]
		[InlineData("66 0F00 28", 4, Code.Verw_Ew)]
		void Test64_Grp6_Ew_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F00 C1", 4, Code.Sldt_RdMw, Register.ECX)]
		[InlineData("66 0F00 CA", 4, Code.Str_RdMw, Register.EDX)]
		[InlineData("66 0F00 D3", 4, Code.Lldt_RdMw, Register.EBX)]
		[InlineData("66 0F00 DC", 4, Code.Ltr_RdMw, Register.ESP)]
		[InlineData("66 0F00 E5", 4, Code.Verr_RdMw, Register.EBP)]
		[InlineData("66 0F00 EE", 4, Code.Verw_RdMw, Register.ESI)]
		void Test16_Grp6_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F00 00", 4, Code.Sldt_RdMw)]
		[InlineData("66 0F00 08", 4, Code.Str_RdMw)]
		[InlineData("66 0F00 10", 4, Code.Lldt_RdMw)]
		[InlineData("66 0F00 18", 4, Code.Ltr_RdMw)]
		[InlineData("66 0F00 20", 4, Code.Verr_RdMw)]
		[InlineData("66 0F00 28", 4, Code.Verw_RdMw)]
		void Test16_Grp6_Ed_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("0F00 C1", 3, Code.Sldt_RdMw, Register.ECX)]
		[InlineData("0F00 CA", 3, Code.Str_RdMw, Register.EDX)]
		[InlineData("0F00 D3", 3, Code.Lldt_RdMw, Register.EBX)]
		[InlineData("0F00 DC", 3, Code.Ltr_RdMw, Register.ESP)]
		[InlineData("0F00 E5", 3, Code.Verr_RdMw, Register.EBP)]
		[InlineData("0F00 EE", 3, Code.Verw_RdMw, Register.ESI)]
		void Test32_Grp6_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("0F00 00", 3, Code.Sldt_RdMw)]
		[InlineData("0F00 08", 3, Code.Str_RdMw)]
		[InlineData("0F00 10", 3, Code.Lldt_RdMw)]
		[InlineData("0F00 18", 3, Code.Ltr_RdMw)]
		[InlineData("0F00 20", 3, Code.Verr_RdMw)]
		[InlineData("0F00 28", 3, Code.Verw_RdMw)]
		void Test32_Grp6_Ed_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("0F00 C1", 3, Code.Sldt_RdMw, Register.ECX)]
		[InlineData("0F00 CA", 3, Code.Str_RdMw, Register.EDX)]
		[InlineData("0F00 D3", 3, Code.Lldt_RdMw, Register.EBX)]
		[InlineData("0F00 DC", 3, Code.Ltr_RdMw, Register.ESP)]
		[InlineData("0F00 E5", 3, Code.Verr_RdMw, Register.EBP)]
		[InlineData("0F00 EE", 3, Code.Verw_RdMw, Register.ESI)]

		[InlineData("41 0F00 C1", 4, Code.Sldt_RdMw, Register.R9D)]
		[InlineData("41 0F00 CA", 4, Code.Str_RdMw, Register.R10D)]
		[InlineData("41 0F00 D3", 4, Code.Lldt_RdMw, Register.R11D)]
		[InlineData("41 0F00 DC", 4, Code.Ltr_RdMw, Register.R12D)]
		[InlineData("41 0F00 E5", 4, Code.Verr_RdMw, Register.R13D)]
		[InlineData("41 0F00 EE", 4, Code.Verw_RdMw, Register.R14D)]
		void Test64_Grp6_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("0F00 00", 3, Code.Sldt_RdMw)]
		[InlineData("0F00 08", 3, Code.Str_RdMw)]
		[InlineData("0F00 10", 3, Code.Lldt_RdMw)]
		[InlineData("0F00 18", 3, Code.Ltr_RdMw)]
		[InlineData("0F00 20", 3, Code.Verr_RdMw)]
		[InlineData("0F00 28", 3, Code.Verw_RdMw)]
		void Test64_Grp6_Ed_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("48 0F00 C1", 4, Code.Sldt_RqMw, Register.RCX)]
		[InlineData("48 0F00 CA", 4, Code.Str_RqMw, Register.RDX)]
		[InlineData("48 0F00 D3", 4, Code.Lldt_RqMw, Register.RBX)]
		[InlineData("48 0F00 DC", 4, Code.Ltr_RqMw, Register.RSP)]
		[InlineData("48 0F00 E5", 4, Code.Verr_RqMw, Register.RBP)]
		[InlineData("48 0F00 EE", 4, Code.Verw_RqMw, Register.RSI)]

		[InlineData("49 0F00 C1", 4, Code.Sldt_RqMw, Register.R9)]
		[InlineData("49 0F00 CA", 4, Code.Str_RqMw, Register.R10)]
		[InlineData("49 0F00 D3", 4, Code.Lldt_RqMw, Register.R11)]
		[InlineData("49 0F00 DC", 4, Code.Ltr_RqMw, Register.R12)]
		[InlineData("49 0F00 E5", 4, Code.Verr_RqMw, Register.R13)]
		[InlineData("49 0F00 EE", 4, Code.Verw_RqMw, Register.R14)]
		void Test64_Grp6_Eq_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("48 0F00 00", 4, Code.Sldt_RqMw)]
		[InlineData("48 0F00 08", 4, Code.Str_RqMw)]
		[InlineData("48 0F00 10", 4, Code.Lldt_RqMw)]
		[InlineData("48 0F00 18", 4, Code.Ltr_RqMw)]
		[InlineData("48 0F00 20", 4, Code.Verr_RqMw)]
		[InlineData("48 0F00 28", 4, Code.Verw_RqMw)]
		void Test64_Grp6_Eq_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("0F01 00", 3, Code.Sgdtw_Ms)]
		[InlineData("0F01 08", 3, Code.Sidtw_Ms)]
		[InlineData("0F01 10", 3, Code.Lgdtw_Ms)]
		[InlineData("0F01 18", 3, Code.Lidtw_Ms)]
		void Test16_Grp7_Ms_Ew_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(MemorySize.Fword5, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F01 00", 4, Code.Sgdtw_Ms)]
		[InlineData("66 0F01 08", 4, Code.Sidtw_Ms)]
		[InlineData("66 0F01 10", 4, Code.Lgdtw_Ms)]
		[InlineData("66 0F01 18", 4, Code.Lidtw_Ms)]
		void Test32_Grp7_Ms_Ew_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(MemorySize.Fword5, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F01 00", 4, Code.Sgdtd_Ms)]
		[InlineData("66 0F01 08", 4, Code.Sidtd_Ms)]
		[InlineData("66 0F01 10", 4, Code.Lgdtd_Ms)]
		[InlineData("66 0F01 18", 4, Code.Lidtd_Ms)]
		void Test16_Grp7_Ms_Ed_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(MemorySize.Fword6, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F01 00", 3, Code.Sgdtd_Ms)]
		[InlineData("0F01 08", 3, Code.Sidtd_Ms)]
		[InlineData("0F01 10", 3, Code.Lgdtd_Ms)]
		[InlineData("0F01 18", 3, Code.Lidtd_Ms)]
		void Test32_Grp7_Ms_Ed_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(MemorySize.Fword6, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F01 00", 3, Code.Sgdtq_Ms)]
		[InlineData("0F01 08", 3, Code.Sidtq_Ms)]
		[InlineData("0F01 10", 3, Code.Lgdtq_Ms)]
		[InlineData("0F01 18", 3, Code.Lidtq_Ms)]

		[InlineData("66 0F01 00", 4, Code.Sgdtq_Ms)]
		[InlineData("66 0F01 08", 4, Code.Sidtq_Ms)]
		[InlineData("66 0F01 10", 4, Code.Lgdtq_Ms)]
		[InlineData("66 0F01 18", 4, Code.Lidtq_Ms)]

		[InlineData("48 0F01 00", 4, Code.Sgdtq_Ms)]
		[InlineData("48 0F01 08", 4, Code.Sidtq_Ms)]
		[InlineData("48 0F01 10", 4, Code.Lgdtq_Ms)]
		[InlineData("48 0F01 18", 4, Code.Lidtq_Ms)]
		void Test64_Grp7_Ms_Eq_1(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(MemorySize.Fword10, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F01 38", 3)]
		[InlineData("66 0F01 38", 4)]
		void Test16_Invlpg_M_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Invlpg_M, instr.Code);
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
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F01 38", 3)]
		[InlineData("66 0F01 38", 4)]
		void Test32_Invlpg_M_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Invlpg_M, instr.Code);
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
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F01 38", 3)]
		[InlineData("48 0F01 38", 4)]
		[InlineData("66 0F01 38", 4)]
		[InlineData("66 48 0F01 38", 5)]
		void Test64_Invlpg_M_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Invlpg_M, instr.Code);
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
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F01 E5", 3, Code.Smsw_Ew, Register.BP)]
		[InlineData("0F01 F6", 3, Code.Lmsw_Ew, Register.SI)]
		void Test16_Grp7_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("0F01 20", 3, Code.Smsw_Ew)]
		[InlineData("0F01 30", 3, Code.Lmsw_Ew)]
		void Test16_Grp7_Ew_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F01 E5", 4, Code.Smsw_Ew, Register.BP)]
		[InlineData("66 0F01 F6", 4, Code.Lmsw_Ew, Register.SI)]
		void Test32_Grp7_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F01 20", 4, Code.Smsw_Ew)]
		[InlineData("66 0F01 30", 4, Code.Lmsw_Ew)]
		void Test32_Grp7_Ew_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F01 E5", 4, Code.Smsw_Ew, Register.BP)]
		[InlineData("66 0F01 F6", 4, Code.Lmsw_Ew, Register.SI)]

		[InlineData("66 41 0F01 E5", 5, Code.Smsw_Ew, Register.R13W)]
		[InlineData("66 41 0F01 F6", 5, Code.Lmsw_Ew, Register.R14W)]
		void Test64_Grp7_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F01 20", 4, Code.Smsw_Ew)]
		[InlineData("66 0F01 30", 4, Code.Lmsw_Ew)]
		void Test64_Grp7_Ew_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F01 E5", 4, Code.Smsw_RdMw, Register.EBP)]
		[InlineData("66 0F01 F6", 4, Code.Lmsw_RdMw, Register.ESI)]
		void Test16_Grp7_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F01 20", 4, Code.Smsw_RdMw)]
		[InlineData("66 0F01 30", 4, Code.Lmsw_RdMw)]
		void Test16_Grp7_Ed_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("0F01 E5", 3, Code.Smsw_RdMw, Register.EBP)]
		[InlineData("0F01 F6", 3, Code.Lmsw_RdMw, Register.ESI)]
		void Test32_Grp7_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("0F01 20", 3, Code.Smsw_RdMw)]
		[InlineData("0F01 30", 3, Code.Lmsw_RdMw)]
		void Test32_Grp7_Ed_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("0F01 E5", 3, Code.Smsw_RdMw, Register.EBP)]
		[InlineData("0F01 F6", 3, Code.Lmsw_RdMw, Register.ESI)]

		[InlineData("41 0F01 E5", 4, Code.Smsw_RdMw, Register.R13D)]
		[InlineData("41 0F01 F6", 4, Code.Lmsw_RdMw, Register.R14D)]
		void Test64_Grp7_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("0F01 20", 3, Code.Smsw_RdMw)]
		[InlineData("0F01 30", 3, Code.Lmsw_RdMw)]
		void Test64_Grp7_Ed_2(string hexBytes, int byteLength, Code code) {
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
		[InlineData("48 0F01 E5", 4, Code.Smsw_RqMw, Register.RBP)]
		[InlineData("48 0F01 F6", 4, Code.Lmsw_RqMw, Register.RSI)]

		[InlineData("49 0F01 E5", 4, Code.Smsw_RqMw, Register.R13)]
		[InlineData("49 0F01 F6", 4, Code.Lmsw_RqMw, Register.R14)]
		void Test64_Grp7_Eq_1(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("48 0F01 20", 4, Code.Smsw_RqMw)]
		[InlineData("48 0F01 30", 4, Code.Lmsw_RqMw)]
		void Test64_Grp7_Eq_2(string hexBytes, int byteLength, Code code) {
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

		[Fact]
		void Test16_Lar_Gw_Ew_1() {
			var decoder = CreateDecoder16("0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lar_Gw_Ew_2() {
			var decoder = CreateDecoder16("0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Lar_Gw_Ew_1() {
			var decoder = CreateDecoder32("66 0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lar_Gw_Ew_2() {
			var decoder = CreateDecoder32("66 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F02 CE", 4, Register.CX, Register.SI)]
		[InlineData("66 44 0F02 C5", 5, Register.R8W, Register.BP)]
		[InlineData("66 41 0F02 D6", 5, Register.DX, Register.R14W)]
		[InlineData("66 45 0F02 D0", 5, Register.R10W, Register.R8W)]
		[InlineData("66 41 0F02 D9", 5, Register.BX, Register.R9W)]
		[InlineData("66 44 0F02 EC", 5, Register.R13W, Register.SP)]
		void Test64_Lar_Gw_Ew_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gw_Ew, instr.Code);
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

		[Fact]
		void Test64_Lar_Gw_Ew_2() {
			var decoder = CreateDecoder64("66 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Lar_Gd_Ed_1() {
			var decoder = CreateDecoder16("66 0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lar_Gd_Ed_2() {
			var decoder = CreateDecoder16("66 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Lar_Gd_Ed_1() {
			var decoder = CreateDecoder32("0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lar_Gd_Ed_2() {
			var decoder = CreateDecoder32("0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F02 CE", 3, Register.ECX, Register.ESI)]
		[InlineData("44 0F02 C5", 4, Register.R8D, Register.EBP)]
		[InlineData("41 0F02 D6", 4, Register.EDX, Register.R14D)]
		[InlineData("45 0F02 D0", 4, Register.R10D, Register.R8D)]
		[InlineData("41 0F02 D9", 4, Register.EBX, Register.R9D)]
		[InlineData("44 0F02 EC", 4, Register.R13D, Register.ESP)]
		void Test64_Lar_Gd_Ed_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gd_Ed, instr.Code);
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

		[Fact]
		void Test64_Lar_Gd_Ed_2() {
			var decoder = CreateDecoder64("0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 0F02 CE", 4, Register.RCX, Register.RSI)]
		[InlineData("4C 0F02 C5", 4, Register.R8, Register.RBP)]
		[InlineData("49 0F02 D6", 4, Register.RDX, Register.R14)]
		[InlineData("4D 0F02 D0", 4, Register.R10, Register.R8)]
		[InlineData("49 0F02 D9", 4, Register.RBX, Register.R9)]
		[InlineData("4C 0F02 EC", 4, Register.R13, Register.RSP)]
		void Test64_Lar_Gq_Eq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gq_Eq, instr.Code);
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

		[Fact]
		void Test64_Lar_Gq_Eq_2() {
			var decoder = CreateDecoder64("48 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_Gq_Eq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Lsl_Gw_Ew_1() {
			var decoder = CreateDecoder16("0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lsl_Gw_Ew_2() {
			var decoder = CreateDecoder16("0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Lsl_Gw_Ew_1() {
			var decoder = CreateDecoder32("66 0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lsl_Gw_Ew_2() {
			var decoder = CreateDecoder32("66 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F03 CE", 4, Register.CX, Register.SI)]
		[InlineData("66 44 0F03 C5", 5, Register.R8W, Register.BP)]
		[InlineData("66 41 0F03 D6", 5, Register.DX, Register.R14W)]
		[InlineData("66 45 0F03 D0", 5, Register.R10W, Register.R8W)]
		[InlineData("66 41 0F03 D9", 5, Register.BX, Register.R9W)]
		[InlineData("66 44 0F03 EC", 5, Register.R13W, Register.SP)]
		void Test64_Lsl_Gw_Ew_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gw_Ew, instr.Code);
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

		[Fact]
		void Test64_Lsl_Gw_Ew_2() {
			var decoder = CreateDecoder64("66 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Lsl_Gd_Ed_1() {
			var decoder = CreateDecoder16("66 0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lsl_Gd_Ed_2() {
			var decoder = CreateDecoder16("66 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Lsl_Gd_Ed_1() {
			var decoder = CreateDecoder32("0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lsl_Gd_Ed_2() {
			var decoder = CreateDecoder32("0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F03 CE", 3, Register.ECX, Register.ESI)]
		[InlineData("44 0F03 C5", 4, Register.R8D, Register.EBP)]
		[InlineData("41 0F03 D6", 4, Register.EDX, Register.R14D)]
		[InlineData("45 0F03 D0", 4, Register.R10D, Register.R8D)]
		[InlineData("41 0F03 D9", 4, Register.EBX, Register.R9D)]
		[InlineData("44 0F03 EC", 4, Register.R13D, Register.ESP)]
		void Test64_Lsl_Gd_Ed_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gd_Ed, instr.Code);
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

		[Fact]
		void Test64_Lsl_Gd_Ed_2() {
			var decoder = CreateDecoder64("0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 0F03 CE", 4, Register.RCX, Register.RSI)]
		[InlineData("4C 0F03 C5", 4, Register.R8, Register.RBP)]
		[InlineData("49 0F03 D6", 4, Register.RDX, Register.R14)]
		[InlineData("4D 0F03 D0", 4, Register.R10, Register.R8)]
		[InlineData("49 0F03 D9", 4, Register.RBX, Register.R9)]
		[InlineData("4C 0F03 EC", 4, Register.R13, Register.RSP)]
		void Test64_Lsl_Gq_Eq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gq_Eq, instr.Code);
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

		[Fact]
		void Test64_Lsl_Gq_Eq_2() {
			var decoder = CreateDecoder64("48 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_Gq_Eq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0F01 C0", 3, Code.Enclv)]
		[InlineData("0F01 C1", 3, Code.Vmcall)]
		[InlineData("0F01 C2", 3, Code.Vmlaunch)]
		[InlineData("0F01 C3", 3, Code.Vmresume)]
		[InlineData("0F01 C4", 3, Code.Vmxoff)]
		[InlineData("0F01 C8", 3, Code.Monitorw)]
		[InlineData("67 0F01 C8", 4, Code.Monitord)]
		[InlineData("0F01 C9", 3, Code.Mwait)]
		[InlineData("0F01 CA", 3, Code.Clac)]
		[InlineData("0F01 CB", 3, Code.Stac)]
		[InlineData("0F01 CF", 3, Code.Encls)]
		[InlineData("0F01 D0", 3, Code.Xgetbv)]
		[InlineData("0F01 D1", 3, Code.Xsetbv)]
		[InlineData("0F01 D4", 3, Code.Vmfunc)]
		[InlineData("0F01 D5", 3, Code.Xend)]
		[InlineData("0F01 D6", 3, Code.Xtest)]
		[InlineData("0F01 D7", 3, Code.Enclu)]
		[InlineData("0F01 EE", 3, Code.Rdpkru)]
		[InlineData("0F01 EF", 3, Code.Wrpkru)]
		[InlineData("0F01 F9", 3, Code.Rdtscp)]
		[InlineData("0F06", 2, Code.Clts)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("0F01 C0", 3, Code.Enclv)]
		[InlineData("0F01 C1", 3, Code.Vmcall)]
		[InlineData("0F01 C2", 3, Code.Vmlaunch)]
		[InlineData("0F01 C3", 3, Code.Vmresume)]
		[InlineData("0F01 C4", 3, Code.Vmxoff)]
		[InlineData("67 0F01 C8", 4, Code.Monitorw)]
		[InlineData("0F01 C8", 3, Code.Monitord)]
		[InlineData("0F01 C9", 3, Code.Mwait)]
		[InlineData("0F01 CA", 3, Code.Clac)]
		[InlineData("0F01 CB", 3, Code.Stac)]
		[InlineData("0F01 CF", 3, Code.Encls)]
		[InlineData("0F01 D0", 3, Code.Xgetbv)]
		[InlineData("0F01 D1", 3, Code.Xsetbv)]
		[InlineData("0F01 D4", 3, Code.Vmfunc)]
		[InlineData("0F01 D5", 3, Code.Xend)]
		[InlineData("0F01 D6", 3, Code.Xtest)]
		[InlineData("0F01 D7", 3, Code.Enclu)]
		[InlineData("0F01 EE", 3, Code.Rdpkru)]
		[InlineData("0F01 EF", 3, Code.Wrpkru)]
		[InlineData("0F01 F9", 3, Code.Rdtscp)]
		[InlineData("0F06", 2, Code.Clts)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("0F01 C0", 3, Code.Enclv)]
		[InlineData("0F01 C1", 3, Code.Vmcall)]
		[InlineData("0F01 C2", 3, Code.Vmlaunch)]
		[InlineData("0F01 C3", 3, Code.Vmresume)]
		[InlineData("0F01 C4", 3, Code.Vmxoff)]
		[InlineData("67 0F01 C8", 4, Code.Monitord)]
		[InlineData("0F01 C8", 3, Code.Monitorq)]
		[InlineData("0F01 C9", 3, Code.Mwait)]
		[InlineData("0F01 CA", 3, Code.Clac)]
		[InlineData("0F01 CB", 3, Code.Stac)]
		[InlineData("0F01 CF", 3, Code.Encls)]
		[InlineData("0F01 D0", 3, Code.Xgetbv)]
		[InlineData("0F01 D1", 3, Code.Xsetbv)]
		[InlineData("0F01 D4", 3, Code.Vmfunc)]
		[InlineData("0F01 D5", 3, Code.Xend)]
		[InlineData("0F01 D6", 3, Code.Xtest)]
		[InlineData("0F01 D7", 3, Code.Enclu)]
		[InlineData("0F01 EE", 3, Code.Rdpkru)]
		[InlineData("0F01 EF", 3, Code.Wrpkru)]
		[InlineData("0F01 F8", 3, Code.Swapgs)]
		[InlineData("0F01 F9", 3, Code.Rdtscp)]
		[InlineData("0F05", 2, Code.Syscall)]
		[InlineData("0F06", 2, Code.Clts)]
		[InlineData("0F07", 2, Code.Sysretd)]
		[InlineData("48 0F07", 3, Code.Sysretq)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
	}
}
