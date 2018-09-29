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
		[InlineData("0F00 C1", 3, Code.Sldt_rm16, Register.CX)]
		[InlineData("0F00 CA", 3, Code.Str_rm16, Register.DX)]
		[InlineData("0F00 D3", 3, Code.Lldt_rm16, Register.BX)]
		[InlineData("0F00 DC", 3, Code.Ltr_rm16, Register.SP)]
		[InlineData("0F00 E5", 3, Code.Verr_rm16, Register.BP)]
		[InlineData("0F00 EE", 3, Code.Verw_rm16, Register.SI)]
		[InlineData("0F00 F1", 3, Code.Jmpe_rm16, Register.CX)]
		[InlineData("66 0F00 F2", 4, Code.Jmpe_rm32, Register.EDX)]
		void Test16_Grp6_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F00 00", 3, Code.Sldt_rm16, MemorySize.UInt16)]
		[InlineData("0F00 08", 3, Code.Str_rm16, MemorySize.UInt16)]
		[InlineData("0F00 10", 3, Code.Lldt_rm16, MemorySize.UInt16)]
		[InlineData("0F00 18", 3, Code.Ltr_rm16, MemorySize.UInt16)]
		[InlineData("0F00 20", 3, Code.Verr_rm16, MemorySize.UInt16)]
		[InlineData("0F00 28", 3, Code.Verw_rm16, MemorySize.UInt16)]
		[InlineData("0F00 30", 3, Code.Jmpe_rm16, MemorySize.UInt16)]
		[InlineData("66 0F00 30", 4, Code.Jmpe_rm32, MemorySize.UInt32)]
		void Test16_Grp6_Ew_2(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F00 C1", 4, Code.Sldt_rm16, Register.CX)]
		[InlineData("66 0F00 CA", 4, Code.Str_rm16, Register.DX)]
		[InlineData("66 0F00 D3", 4, Code.Lldt_rm16, Register.BX)]
		[InlineData("66 0F00 DC", 4, Code.Ltr_rm16, Register.SP)]
		[InlineData("66 0F00 E5", 4, Code.Verr_rm16, Register.BP)]
		[InlineData("66 0F00 EE", 4, Code.Verw_rm16, Register.SI)]
		[InlineData("66 0F00 F1", 4, Code.Jmpe_rm16, Register.CX)]
		[InlineData("0F00 F2", 3, Code.Jmpe_rm32, Register.EDX)]
		void Test32_Grp6_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F00 00", 4, Code.Sldt_rm16, MemorySize.UInt16)]
		[InlineData("66 0F00 08", 4, Code.Str_rm16, MemorySize.UInt16)]
		[InlineData("66 0F00 10", 4, Code.Lldt_rm16, MemorySize.UInt16)]
		[InlineData("66 0F00 18", 4, Code.Ltr_rm16, MemorySize.UInt16)]
		[InlineData("66 0F00 20", 4, Code.Verr_rm16, MemorySize.UInt16)]
		[InlineData("66 0F00 28", 4, Code.Verw_rm16, MemorySize.UInt16)]
		[InlineData("66 0F00 30", 4, Code.Jmpe_rm16, MemorySize.UInt16)]
		[InlineData("0F00 30", 3, Code.Jmpe_rm32, MemorySize.UInt32)]
		void Test32_Grp6_Ew_2(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F00 C1", 4, Code.Sldt_rm16, Register.CX)]
		[InlineData("66 0F00 CA", 4, Code.Str_rm16, Register.DX)]
		[InlineData("66 0F00 D3", 4, Code.Lldt_rm16, Register.BX)]
		[InlineData("66 0F00 DC", 4, Code.Ltr_rm16, Register.SP)]
		[InlineData("66 0F00 E5", 4, Code.Verr_rm16, Register.BP)]
		[InlineData("66 0F00 EE", 4, Code.Verw_rm16, Register.SI)]

		[InlineData("66 41 0F00 C1", 5, Code.Sldt_rm16, Register.R9W)]
		[InlineData("66 41 0F00 CA", 5, Code.Str_rm16, Register.R10W)]
		[InlineData("66 41 0F00 D3", 5, Code.Lldt_rm16, Register.R11W)]
		[InlineData("66 41 0F00 DC", 5, Code.Ltr_rm16, Register.R12W)]
		[InlineData("66 41 0F00 E5", 5, Code.Verr_rm16, Register.R13W)]
		[InlineData("66 41 0F00 EE", 5, Code.Verw_rm16, Register.R14W)]
		void Test64_Grp6_Ew_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F00 00", 4, Code.Sldt_rm16)]
		[InlineData("66 0F00 08", 4, Code.Str_rm16)]
		[InlineData("66 0F00 10", 4, Code.Lldt_rm16)]
		[InlineData("66 0F00 18", 4, Code.Ltr_rm16)]
		[InlineData("66 0F00 20", 4, Code.Verr_rm16)]
		[InlineData("66 0F00 28", 4, Code.Verw_rm16)]
		void Test64_Grp6_Ew_2(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("66 0F00 C1", 4, Code.Sldt_r32m16, Register.ECX)]
		[InlineData("66 0F00 CA", 4, Code.Str_r32m16, Register.EDX)]
		[InlineData("66 0F00 D3", 4, Code.Lldt_r32m16, Register.EBX)]
		[InlineData("66 0F00 DC", 4, Code.Ltr_r32m16, Register.ESP)]
		[InlineData("66 0F00 E5", 4, Code.Verr_r32m16, Register.EBP)]
		[InlineData("66 0F00 EE", 4, Code.Verw_r32m16, Register.ESI)]
		void Test16_Grp6_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F00 00", 4, Code.Sldt_r32m16)]
		[InlineData("66 0F00 08", 4, Code.Str_r32m16)]
		[InlineData("66 0F00 10", 4, Code.Lldt_r32m16)]
		[InlineData("66 0F00 18", 4, Code.Ltr_r32m16)]
		[InlineData("66 0F00 20", 4, Code.Verr_r32m16)]
		[InlineData("66 0F00 28", 4, Code.Verw_r32m16)]
		void Test16_Grp6_Ed_2(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F00 C1", 3, Code.Sldt_r32m16, Register.ECX)]
		[InlineData("0F00 CA", 3, Code.Str_r32m16, Register.EDX)]
		[InlineData("0F00 D3", 3, Code.Lldt_r32m16, Register.EBX)]
		[InlineData("0F00 DC", 3, Code.Ltr_r32m16, Register.ESP)]
		[InlineData("0F00 E5", 3, Code.Verr_r32m16, Register.EBP)]
		[InlineData("0F00 EE", 3, Code.Verw_r32m16, Register.ESI)]
		void Test32_Grp6_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F00 00", 3, Code.Sldt_r32m16)]
		[InlineData("0F00 08", 3, Code.Str_r32m16)]
		[InlineData("0F00 10", 3, Code.Lldt_r32m16)]
		[InlineData("0F00 18", 3, Code.Ltr_r32m16)]
		[InlineData("0F00 20", 3, Code.Verr_r32m16)]
		[InlineData("0F00 28", 3, Code.Verw_r32m16)]
		void Test32_Grp6_Ed_2(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F00 C1", 3, Code.Sldt_r32m16, Register.ECX)]
		[InlineData("0F00 CA", 3, Code.Str_r32m16, Register.EDX)]
		[InlineData("0F00 D3", 3, Code.Lldt_r32m16, Register.EBX)]
		[InlineData("0F00 DC", 3, Code.Ltr_r32m16, Register.ESP)]
		[InlineData("0F00 E5", 3, Code.Verr_r32m16, Register.EBP)]
		[InlineData("0F00 EE", 3, Code.Verw_r32m16, Register.ESI)]

		[InlineData("41 0F00 C1", 4, Code.Sldt_r32m16, Register.R9D)]
		[InlineData("41 0F00 CA", 4, Code.Str_r32m16, Register.R10D)]
		[InlineData("41 0F00 D3", 4, Code.Lldt_r32m16, Register.R11D)]
		[InlineData("41 0F00 DC", 4, Code.Ltr_r32m16, Register.R12D)]
		[InlineData("41 0F00 E5", 4, Code.Verr_r32m16, Register.R13D)]
		[InlineData("41 0F00 EE", 4, Code.Verw_r32m16, Register.R14D)]
		void Test64_Grp6_Ed_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F00 00", 3, Code.Sldt_r32m16)]
		[InlineData("0F00 08", 3, Code.Str_r32m16)]
		[InlineData("0F00 10", 3, Code.Lldt_r32m16)]
		[InlineData("0F00 18", 3, Code.Ltr_r32m16)]
		[InlineData("0F00 20", 3, Code.Verr_r32m16)]
		[InlineData("0F00 28", 3, Code.Verw_r32m16)]
		void Test64_Grp6_Ed_2(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("48 0F00 C1", 4, Code.Sldt_r64m16, Register.RCX)]
		[InlineData("48 0F00 CA", 4, Code.Str_r64m16, Register.RDX)]
		[InlineData("48 0F00 D3", 4, Code.Lldt_r64m16, Register.RBX)]
		[InlineData("48 0F00 DC", 4, Code.Ltr_r64m16, Register.RSP)]
		[InlineData("48 0F00 E5", 4, Code.Verr_r64m16, Register.RBP)]
		[InlineData("48 0F00 EE", 4, Code.Verw_r64m16, Register.RSI)]

		[InlineData("49 0F00 C1", 4, Code.Sldt_r64m16, Register.R9)]
		[InlineData("49 0F00 CA", 4, Code.Str_r64m16, Register.R10)]
		[InlineData("49 0F00 D3", 4, Code.Lldt_r64m16, Register.R11)]
		[InlineData("49 0F00 DC", 4, Code.Ltr_r64m16, Register.R12)]
		[InlineData("49 0F00 E5", 4, Code.Verr_r64m16, Register.R13)]
		[InlineData("49 0F00 EE", 4, Code.Verw_r64m16, Register.R14)]
		void Test64_Grp6_Eq_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("48 0F00 00", 4, Code.Sldt_r64m16)]
		[InlineData("48 0F00 08", 4, Code.Str_r64m16)]
		[InlineData("48 0F00 10", 4, Code.Lldt_r64m16)]
		[InlineData("48 0F00 18", 4, Code.Ltr_r64m16)]
		[InlineData("48 0F00 20", 4, Code.Verr_r64m16)]
		[InlineData("48 0F00 28", 4, Code.Verw_r64m16)]
		void Test64_Grp6_Eq_2(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F01 00", 3, Code.Sgdt_m40)]
		[InlineData("0F01 08", 3, Code.Sidt_m40)]
		[InlineData("0F01 10", 3, Code.Lgdt_m40)]
		[InlineData("0F01 18", 3, Code.Lidt_m40)]
		void Test16_Grp7_Ms_Ew_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("66 0F01 00", 4, Code.Sgdt_m40)]
		[InlineData("66 0F01 08", 4, Code.Sidt_m40)]
		[InlineData("66 0F01 10", 4, Code.Lgdt_m40)]
		[InlineData("66 0F01 18", 4, Code.Lidt_m40)]
		void Test32_Grp7_Ms_Ew_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("66 0F01 00", 4, Code.Sgdt_m48)]
		[InlineData("66 0F01 08", 4, Code.Sidt_m48)]
		[InlineData("66 0F01 10", 4, Code.Lgdt_m48)]
		[InlineData("66 0F01 18", 4, Code.Lidt_m48)]
		void Test16_Grp7_Ms_Ed_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F01 00", 3, Code.Sgdt_m48)]
		[InlineData("0F01 08", 3, Code.Sidt_m48)]
		[InlineData("0F01 10", 3, Code.Lgdt_m48)]
		[InlineData("0F01 18", 3, Code.Lidt_m48)]
		void Test32_Grp7_Ms_Ed_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F01 00", 3, Code.Sgdt_m80)]
		[InlineData("0F01 08", 3, Code.Sidt_m80)]
		[InlineData("0F01 10", 3, Code.Lgdt_m80)]
		[InlineData("0F01 18", 3, Code.Lidt_m80)]

		[InlineData("66 0F01 00", 4, Code.Sgdt_m80)]
		[InlineData("66 0F01 08", 4, Code.Sidt_m80)]
		[InlineData("66 0F01 10", 4, Code.Lgdt_m80)]
		[InlineData("66 0F01 18", 4, Code.Lidt_m80)]

		[InlineData("48 0F01 00", 4, Code.Sgdt_m80)]
		[InlineData("48 0F01 08", 4, Code.Sidt_m80)]
		[InlineData("48 0F01 10", 4, Code.Lgdt_m80)]
		[InlineData("48 0F01 18", 4, Code.Lidt_m80)]
		void Test64_Grp7_Ms_Eq_1(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test16_Invlpg_m_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Invlpg_m, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Invlpg_m_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Invlpg_m, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test64_Invlpg_m_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Invlpg_m, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F01 E5", 3, Code.Smsw_rm16, Register.BP)]
		[InlineData("0F01 F6", 3, Code.Lmsw_rm16, Register.SI)]
		[InlineData("66 0F01 E5", 4, Code.Smsw_r32m16, Register.EBP)]
		[InlineData("66 0F01 F6", 4, Code.Lmsw_r32m16, Register.ESI)]
		void Test16_Grp7_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("0F01 20", 3, Code.Smsw_rm16, MemorySize.UInt16)]
		[InlineData("0F01 30", 3, Code.Lmsw_rm16, MemorySize.UInt16)]
		[InlineData("66 0F01 20", 4, Code.Smsw_r32m16, MemorySize.UInt16)]
		[InlineData("66 0F01 30", 4, Code.Lmsw_r32m16, MemorySize.UInt16)]
		[InlineData("F3 0F01 28", 4, Code.Rstorssp_m64, MemorySize.UInt64)]
		void Test16_Grp7_2(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F01 E5", 4, Code.Smsw_rm16, Register.BP)]
		[InlineData("66 0F01 F6", 4, Code.Lmsw_rm16, Register.SI)]
		[InlineData("0F01 E5", 3, Code.Smsw_r32m16, Register.EBP)]
		[InlineData("0F01 F6", 3, Code.Lmsw_r32m16, Register.ESI)]
		void Test32_Grp7_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F01 20", 4, Code.Smsw_rm16, MemorySize.UInt16)]
		[InlineData("66 0F01 30", 4, Code.Lmsw_rm16, MemorySize.UInt16)]
		[InlineData("0F01 20", 3, Code.Smsw_r32m16, MemorySize.UInt16)]
		[InlineData("0F01 30", 3, Code.Lmsw_r32m16, MemorySize.UInt16)]
		[InlineData("F3 0F01 28", 4, Code.Rstorssp_m64, MemorySize.UInt64)]
		void Test32_Grp7_2(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0F01 E5", 4, Code.Smsw_rm16, Register.BP)]
		[InlineData("66 0F01 F6", 4, Code.Lmsw_rm16, Register.SI)]
		[InlineData("66 41 0F01 E5", 5, Code.Smsw_rm16, Register.R13W)]
		[InlineData("66 41 0F01 F6", 5, Code.Lmsw_rm16, Register.R14W)]

		[InlineData("0F01 E5", 3, Code.Smsw_r32m16, Register.EBP)]
		[InlineData("0F01 F6", 3, Code.Lmsw_r32m16, Register.ESI)]
		[InlineData("41 0F01 E5", 4, Code.Smsw_r32m16, Register.R13D)]
		[InlineData("41 0F01 F6", 4, Code.Lmsw_r32m16, Register.R14D)]

		[InlineData("48 0F01 E5", 4, Code.Smsw_r64m16, Register.RBP)]
		[InlineData("48 0F01 F6", 4, Code.Lmsw_r64m16, Register.RSI)]
		[InlineData("49 0F01 E5", 4, Code.Smsw_r64m16, Register.R13)]
		[InlineData("49 0F01 F6", 4, Code.Lmsw_r64m16, Register.R14)]
		void Test64_Grp7_1(string hexBytes, int byteLength, Code code, Register reg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[InlineData("66 0F01 20", 4, Code.Smsw_rm16, MemorySize.UInt16)]
		[InlineData("66 0F01 30", 4, Code.Lmsw_rm16, MemorySize.UInt16)]

		[InlineData("0F01 20", 3, Code.Smsw_r32m16, MemorySize.UInt16)]
		[InlineData("0F01 30", 3, Code.Lmsw_r32m16, MemorySize.UInt16)]

		[InlineData("48 0F01 20", 4, Code.Smsw_r64m16, MemorySize.UInt16)]
		[InlineData("48 0F01 30", 4, Code.Lmsw_r64m16, MemorySize.UInt16)]

		[InlineData("F3 0F01 28", 4, Code.Rstorssp_m64, MemorySize.UInt64)]
		void Test64_Grp7_2(string hexBytes, int byteLength, Code code, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Lar_r16_rm16_1() {
			var decoder = CreateDecoder16("0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lar_r16_rm16_2() {
			var decoder = CreateDecoder16("0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Lar_r16_rm16_1() {
			var decoder = CreateDecoder32("66 0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lar_r16_rm16_2() {
			var decoder = CreateDecoder32("66 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test64_Lar_r16_rm16_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Lar_r16_rm16_2() {
			var decoder = CreateDecoder64("66 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test16_Lar_r32_rm32_1() {
			var decoder = CreateDecoder16("66 0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lar_r32_rm32_2() {
			var decoder = CreateDecoder16("66 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Lar_r32_rm32_1() {
			var decoder = CreateDecoder32("0F02 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lar_r32_rm32_2() {
			var decoder = CreateDecoder32("0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test64_Lar_r32_rm32_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Lar_r32_rm32_2() {
			var decoder = CreateDecoder64("0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test64_Lar_r64_rm64_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Lar_r64_rm64_2() {
			var decoder = CreateDecoder64("48 0F02 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lar_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test16_Lsl_r16_rm16_1() {
			var decoder = CreateDecoder16("0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lsl_r16_rm16_2() {
			var decoder = CreateDecoder16("0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Lsl_r16_rm16_1() {
			var decoder = CreateDecoder32("66 0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lsl_r16_rm16_2() {
			var decoder = CreateDecoder32("66 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test64_Lsl_r16_rm16_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Lsl_r16_rm16_2() {
			var decoder = CreateDecoder64("66 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r16_rm16, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test16_Lsl_r32_rm32_1() {
			var decoder = CreateDecoder16("66 0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Lsl_r32_rm32_2() {
			var decoder = CreateDecoder16("66 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test32_Lsl_r32_rm32_1() {
			var decoder = CreateDecoder32("0F03 CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Lsl_r32_rm32_2() {
			var decoder = CreateDecoder32("0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test64_Lsl_r32_rm32_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Lsl_r32_rm32_2() {
			var decoder = CreateDecoder64("0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r32_rm32, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		void Test64_Lsl_r64_rm64_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}

		[Fact]
		void Test64_Lsl_r64_rm64_2() {
			var decoder = CreateDecoder64("48 0F03 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lsl_r64_rm64, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F01 C0", 3, Code.Enclv, DecoderOptions.None)]
		[InlineData("0F01 C1", 3, Code.Vmcall, DecoderOptions.None)]
		[InlineData("0F01 C2", 3, Code.Vmlaunch, DecoderOptions.None)]
		[InlineData("0F01 C3", 3, Code.Vmresume, DecoderOptions.None)]
		[InlineData("0F01 C4", 3, Code.Vmxoff, DecoderOptions.None)]
		[InlineData("0F01 C5", 3, Code.Pconfig, DecoderOptions.None)]
		[InlineData("0F01 C8", 3, Code.Monitorw, DecoderOptions.None)]
		[InlineData("67 0F01 C8", 4, Code.Monitord, DecoderOptions.None)]
		[InlineData("0F01 C9", 3, Code.Mwait, DecoderOptions.None)]
		[InlineData("0F01 CA", 3, Code.Clac, DecoderOptions.None)]
		[InlineData("0F01 CB", 3, Code.Stac, DecoderOptions.None)]
		[InlineData("0F01 CF", 3, Code.Encls, DecoderOptions.None)]
		[InlineData("0F01 D0", 3, Code.Xgetbv, DecoderOptions.None)]
		[InlineData("0F01 D1", 3, Code.Xsetbv, DecoderOptions.None)]
		[InlineData("0F01 D4", 3, Code.Vmfunc, DecoderOptions.None)]
		[InlineData("0F01 D5", 3, Code.Xend, DecoderOptions.None)]
		[InlineData("0F01 D6", 3, Code.Xtest, DecoderOptions.None)]
		[InlineData("0F01 D7", 3, Code.Enclu, DecoderOptions.None)]
		[InlineData("0F01 D8", 3, Code.Vmrunw, DecoderOptions.None)]
		[InlineData("67 0F01 D8", 4, Code.Vmrund, DecoderOptions.None)]
		[InlineData("0F01 D9", 3, Code.Vmmcall, DecoderOptions.None)]
		[InlineData("0F01 DA", 3, Code.Vmloadw, DecoderOptions.None)]
		[InlineData("67 0F01 DA", 4, Code.Vmloadd, DecoderOptions.None)]
		[InlineData("0F01 DB", 3, Code.Vmsavew, DecoderOptions.None)]
		[InlineData("67 0F01 DB", 4, Code.Vmsaved, DecoderOptions.None)]
		[InlineData("0F01 DC", 3, Code.Stgi, DecoderOptions.None)]
		[InlineData("0F01 DD", 3, Code.Clgi, DecoderOptions.None)]
		[InlineData("0F01 DE", 3, Code.Skinit, DecoderOptions.None)]
		[InlineData("0F01 DF", 3, Code.Invlpgaw, DecoderOptions.None)]
		[InlineData("67 0F01 DF", 4, Code.Invlpgad, DecoderOptions.None)]
		[InlineData("F3 0F01 E8", 4, Code.Setssbsy, DecoderOptions.None)]
		[InlineData("F3 0F01 EA", 4, Code.Saveprevssp, DecoderOptions.None)]
		[InlineData("0F01 EE", 3, Code.Rdpkru, DecoderOptions.None)]
		[InlineData("0F01 EF", 3, Code.Wrpkru, DecoderOptions.None)]
		[InlineData("0F01 F9", 3, Code.Rdtscp, DecoderOptions.None)]
		[InlineData("0F01 FA", 3, Code.Monitorxw, DecoderOptions.None)]
		[InlineData("67 0F01 FA", 4, Code.Monitorxd, DecoderOptions.None)]
		[InlineData("0F01 FB", 3, Code.Mwaitx, DecoderOptions.None)]
		[InlineData("0F01 FC", 3, Code.Clzerow, DecoderOptions.None)]
		[InlineData("66 0F01 FC", 4, Code.Clzerod, DecoderOptions.None)]
		[InlineData("0F04", 2, Code.Loadallreset286, DecoderOptions.Loadall286)]
		[InlineData("0F05", 2, Code.Loadall286, DecoderOptions.Loadall286)]
		[InlineData("0F06", 2, Code.Clts, DecoderOptions.None)]
		[InlineData("0F07", 2, Code.Loadall386, DecoderOptions.Loadall386)]
		void Test16_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("0F01 C0", 3, Code.Enclv, DecoderOptions.None)]
		[InlineData("0F01 C1", 3, Code.Vmcall, DecoderOptions.None)]
		[InlineData("0F01 C2", 3, Code.Vmlaunch, DecoderOptions.None)]
		[InlineData("0F01 C3", 3, Code.Vmresume, DecoderOptions.None)]
		[InlineData("0F01 C4", 3, Code.Vmxoff, DecoderOptions.None)]
		[InlineData("0F01 C5", 3, Code.Pconfig, DecoderOptions.None)]
		[InlineData("67 0F01 C8", 4, Code.Monitorw, DecoderOptions.None)]
		[InlineData("0F01 C8", 3, Code.Monitord, DecoderOptions.None)]
		[InlineData("0F01 C9", 3, Code.Mwait, DecoderOptions.None)]
		[InlineData("0F01 CA", 3, Code.Clac, DecoderOptions.None)]
		[InlineData("0F01 CB", 3, Code.Stac, DecoderOptions.None)]
		[InlineData("0F01 CF", 3, Code.Encls, DecoderOptions.None)]
		[InlineData("0F01 D0", 3, Code.Xgetbv, DecoderOptions.None)]
		[InlineData("0F01 D1", 3, Code.Xsetbv, DecoderOptions.None)]
		[InlineData("0F01 D4", 3, Code.Vmfunc, DecoderOptions.None)]
		[InlineData("0F01 D5", 3, Code.Xend, DecoderOptions.None)]
		[InlineData("0F01 D6", 3, Code.Xtest, DecoderOptions.None)]
		[InlineData("0F01 D7", 3, Code.Enclu, DecoderOptions.None)]
		[InlineData("67 0F01 D8", 4, Code.Vmrunw, DecoderOptions.None)]
		[InlineData("0F01 D8", 3, Code.Vmrund, DecoderOptions.None)]
		[InlineData("0F01 D9", 3, Code.Vmmcall, DecoderOptions.None)]
		[InlineData("67 0F01 DA", 4, Code.Vmloadw, DecoderOptions.None)]
		[InlineData("0F01 DA", 3, Code.Vmloadd, DecoderOptions.None)]
		[InlineData("67 0F01 DB", 4, Code.Vmsavew, DecoderOptions.None)]
		[InlineData("0F01 DB", 3, Code.Vmsaved, DecoderOptions.None)]
		[InlineData("0F01 DC", 3, Code.Stgi, DecoderOptions.None)]
		[InlineData("0F01 DD", 3, Code.Clgi, DecoderOptions.None)]
		[InlineData("0F01 DE", 3, Code.Skinit, DecoderOptions.None)]
		[InlineData("67 0F01 DF", 4, Code.Invlpgaw, DecoderOptions.None)]
		[InlineData("0F01 DF", 3, Code.Invlpgad, DecoderOptions.None)]
		[InlineData("F3 0F01 E8", 4, Code.Setssbsy, DecoderOptions.None)]
		[InlineData("F3 0F01 EA", 4, Code.Saveprevssp, DecoderOptions.None)]
		[InlineData("0F01 EE", 3, Code.Rdpkru, DecoderOptions.None)]
		[InlineData("0F01 EF", 3, Code.Wrpkru, DecoderOptions.None)]
		[InlineData("0F01 F9", 3, Code.Rdtscp, DecoderOptions.None)]
		[InlineData("67 0F01 FA", 4, Code.Monitorxw, DecoderOptions.None)]
		[InlineData("0F01 FA", 3, Code.Monitorxd, DecoderOptions.None)]
		[InlineData("0F01 FB", 3, Code.Mwaitx, DecoderOptions.None)]
		[InlineData("66 0F01 FC", 4, Code.Clzerow, DecoderOptions.None)]
		[InlineData("0F01 FC", 3, Code.Clzerod, DecoderOptions.None)]
		[InlineData("0F04", 2, Code.Loadallreset286, DecoderOptions.Loadall286)]
		[InlineData("0F05", 2, Code.Loadall286, DecoderOptions.Loadall286)]
		[InlineData("0F06", 2, Code.Clts, DecoderOptions.None)]
		[InlineData("0F07", 2, Code.Loadall386, DecoderOptions.Loadall386)]
		void Test32_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}

		[Theory]
		[InlineData("0F01 C0", 3, Code.Enclv, DecoderOptions.None)]
		[InlineData("0F01 C1", 3, Code.Vmcall, DecoderOptions.None)]
		[InlineData("0F01 C2", 3, Code.Vmlaunch, DecoderOptions.None)]
		[InlineData("0F01 C3", 3, Code.Vmresume, DecoderOptions.None)]
		[InlineData("0F01 C4", 3, Code.Vmxoff, DecoderOptions.None)]
		[InlineData("0F01 C5", 3, Code.Pconfig, DecoderOptions.None)]
		[InlineData("67 0F01 C8", 4, Code.Monitord, DecoderOptions.None)]
		[InlineData("0F01 C8", 3, Code.Monitorq, DecoderOptions.None)]
		[InlineData("0F01 C9", 3, Code.Mwait, DecoderOptions.None)]
		[InlineData("0F01 CA", 3, Code.Clac, DecoderOptions.None)]
		[InlineData("0F01 CB", 3, Code.Stac, DecoderOptions.None)]
		[InlineData("0F01 CF", 3, Code.Encls, DecoderOptions.None)]
		[InlineData("0F01 D0", 3, Code.Xgetbv, DecoderOptions.None)]
		[InlineData("0F01 D1", 3, Code.Xsetbv, DecoderOptions.None)]
		[InlineData("0F01 D4", 3, Code.Vmfunc, DecoderOptions.None)]
		[InlineData("0F01 D5", 3, Code.Xend, DecoderOptions.None)]
		[InlineData("0F01 D6", 3, Code.Xtest, DecoderOptions.None)]
		[InlineData("0F01 D7", 3, Code.Enclu, DecoderOptions.None)]
		[InlineData("67 0F01 D8", 4, Code.Vmrund, DecoderOptions.None)]
		[InlineData("0F01 D8", 3, Code.Vmrunq, DecoderOptions.None)]
		[InlineData("0F01 D9", 3, Code.Vmmcall, DecoderOptions.None)]
		[InlineData("67 0F01 DA", 4, Code.Vmloadd, DecoderOptions.None)]
		[InlineData("0F01 DA", 3, Code.Vmloadq, DecoderOptions.None)]
		[InlineData("67 0F01 DB", 4, Code.Vmsaved, DecoderOptions.None)]
		[InlineData("0F01 DB", 3, Code.Vmsaveq, DecoderOptions.None)]
		[InlineData("0F01 DC", 3, Code.Stgi, DecoderOptions.None)]
		[InlineData("0F01 DD", 3, Code.Clgi, DecoderOptions.None)]
		[InlineData("0F01 DE", 3, Code.Skinit, DecoderOptions.None)]
		[InlineData("67 0F01 DF", 4, Code.Invlpgad, DecoderOptions.None)]
		[InlineData("0F01 DF", 3, Code.Invlpgaq, DecoderOptions.None)]
		[InlineData("F3 0F01 E8", 4, Code.Setssbsy, DecoderOptions.None)]
		[InlineData("F3 0F01 EA", 4, Code.Saveprevssp, DecoderOptions.None)]
		[InlineData("0F01 EE", 3, Code.Rdpkru, DecoderOptions.None)]
		[InlineData("0F01 EF", 3, Code.Wrpkru, DecoderOptions.None)]
		[InlineData("0F01 F8", 3, Code.Swapgs, DecoderOptions.None)]
		[InlineData("0F01 F9", 3, Code.Rdtscp, DecoderOptions.None)]
		[InlineData("67 0F01 FA", 4, Code.Monitorxd, DecoderOptions.None)]
		[InlineData("0F01 FA", 3, Code.Monitorxq, DecoderOptions.None)]
		[InlineData("0F01 FB", 3, Code.Mwaitx, DecoderOptions.None)]
		[InlineData("66 0F01 FC", 4, Code.Clzerow, DecoderOptions.None)]
		[InlineData("0F01 FC", 3, Code.Clzerod, DecoderOptions.None)]
		[InlineData("48 0F01 FC", 4, Code.Clzeroq, DecoderOptions.None)]
		[InlineData("0F05", 2, Code.Syscall, DecoderOptions.None)]
		[InlineData("0F06", 2, Code.Clts, DecoderOptions.None)]
		[InlineData("0F07", 2, Code.Sysretd, DecoderOptions.None)]
		[InlineData("48 0F07", 3, Code.Sysretq, DecoderOptions.None)]
		void Test64_Simple_1(string hexBytes, int byteLength, Code code, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}
	}
}
