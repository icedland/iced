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
	public sealed class DecoderTest_1_D0_D7 : DecoderTest {
		[Theory]
		[InlineData("D0 00", 2, Code.Rol_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 08", 2, Code.Ror_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 10", 2, Code.Rcl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 18", 2, Code.Rcr_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 20", 2, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 28", 2, Code.Shr_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 30", 2, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 38", 2, Code.Sar_rm8_1, MemorySize.Int8)]
		void Test16_Grp2_Eb_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D0 C1", 2, Code.Rol_rm8_1, Register.CL)]
		[InlineData("D0 CA", 2, Code.Ror_rm8_1, Register.DL)]
		[InlineData("D0 D3", 2, Code.Rcl_rm8_1, Register.BL)]
		[InlineData("D0 DC", 2, Code.Rcr_rm8_1, Register.AH)]
		[InlineData("D0 E5", 2, Code.Shl_rm8_1, Register.CH)]
		[InlineData("D0 EE", 2, Code.Shr_rm8_1, Register.DH)]
		[InlineData("D0 F7", 2, Code.Shl_rm8_1, Register.BH)]
		[InlineData("D0 F8", 2, Code.Sar_rm8_1, Register.AL)]
		void Test16_Grp2_Eb_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D0 00", 2, Code.Rol_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 08", 2, Code.Ror_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 10", 2, Code.Rcl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 18", 2, Code.Rcr_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 20", 2, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 28", 2, Code.Shr_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 30", 2, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 38", 2, Code.Sar_rm8_1, MemorySize.Int8)]
		void Test32_Grp2_Eb_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D0 C1", 2, Code.Rol_rm8_1, Register.CL)]
		[InlineData("D0 CA", 2, Code.Ror_rm8_1, Register.DL)]
		[InlineData("D0 D3", 2, Code.Rcl_rm8_1, Register.BL)]
		[InlineData("D0 DC", 2, Code.Rcr_rm8_1, Register.AH)]
		[InlineData("D0 E5", 2, Code.Shl_rm8_1, Register.CH)]
		[InlineData("D0 EE", 2, Code.Shr_rm8_1, Register.DH)]
		[InlineData("D0 F7", 2, Code.Shl_rm8_1, Register.BH)]
		[InlineData("D0 F8", 2, Code.Sar_rm8_1, Register.AL)]
		void Test32_Grp2_Eb_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D0 00", 2, Code.Rol_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 08", 2, Code.Ror_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 10", 2, Code.Rcl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 18", 2, Code.Rcr_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 20", 2, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 28", 2, Code.Shr_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 30", 2, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("D0 38", 2, Code.Sar_rm8_1, MemorySize.Int8)]

		[InlineData("44 D0 00", 3, Code.Rol_rm8_1, MemorySize.UInt8)]
		[InlineData("44 D0 08", 3, Code.Ror_rm8_1, MemorySize.UInt8)]
		[InlineData("44 D0 10", 3, Code.Rcl_rm8_1, MemorySize.UInt8)]
		[InlineData("44 D0 18", 3, Code.Rcr_rm8_1, MemorySize.UInt8)]
		[InlineData("44 D0 20", 3, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("44 D0 28", 3, Code.Shr_rm8_1, MemorySize.UInt8)]
		[InlineData("44 D0 30", 3, Code.Shl_rm8_1, MemorySize.UInt8)]
		[InlineData("44 D0 38", 3, Code.Sar_rm8_1, MemorySize.Int8)]
		void Test64_Grp2_Eb_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D0 C1", 2, Code.Rol_rm8_1, Register.CL)]
		[InlineData("D0 CA", 2, Code.Ror_rm8_1, Register.DL)]
		[InlineData("D0 D3", 2, Code.Rcl_rm8_1, Register.BL)]
		[InlineData("D0 DC", 2, Code.Rcr_rm8_1, Register.AH)]
		[InlineData("D0 E5", 2, Code.Shl_rm8_1, Register.CH)]
		[InlineData("D0 EE", 2, Code.Shr_rm8_1, Register.DH)]
		[InlineData("D0 F7", 2, Code.Shl_rm8_1, Register.BH)]
		[InlineData("D0 F8", 2, Code.Sar_rm8_1, Register.AL)]

		[InlineData("40 D0 C1", 3, Code.Rol_rm8_1, Register.CL)]
		[InlineData("40 D0 CA", 3, Code.Ror_rm8_1, Register.DL)]
		[InlineData("40 D0 D3", 3, Code.Rcl_rm8_1, Register.BL)]
		[InlineData("40 D0 DC", 3, Code.Rcr_rm8_1, Register.SPL)]
		[InlineData("40 D0 E5", 3, Code.Shl_rm8_1, Register.BPL)]
		[InlineData("40 D0 EE", 3, Code.Shr_rm8_1, Register.SIL)]
		[InlineData("40 D0 F7", 3, Code.Shl_rm8_1, Register.DIL)]
		[InlineData("41 D0 F8", 3, Code.Sar_rm8_1, Register.R8L)]
		[InlineData("41 D0 C1", 3, Code.Rol_rm8_1, Register.R9L)]
		[InlineData("41 D0 CA", 3, Code.Ror_rm8_1, Register.R10L)]
		[InlineData("41 D0 D3", 3, Code.Rcl_rm8_1, Register.R11L)]
		[InlineData("41 D0 DC", 3, Code.Rcr_rm8_1, Register.R12L)]
		[InlineData("41 D0 E5", 3, Code.Shl_rm8_1, Register.R13L)]
		[InlineData("41 D0 EE", 3, Code.Shr_rm8_1, Register.R14L)]
		[InlineData("41 D0 F7", 3, Code.Shl_rm8_1, Register.R15L)]
		[InlineData("40 D0 F8", 3, Code.Sar_rm8_1, Register.AL)]
		void Test64_Grp2_Eb_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D1 00", 2, Code.Rol_rm16_1, MemorySize.UInt16)]
		[InlineData("D1 08", 2, Code.Ror_rm16_1, MemorySize.UInt16)]
		[InlineData("D1 10", 2, Code.Rcl_rm16_1, MemorySize.UInt16)]
		[InlineData("D1 18", 2, Code.Rcr_rm16_1, MemorySize.UInt16)]
		[InlineData("D1 20", 2, Code.Shl_rm16_1, MemorySize.UInt16)]
		[InlineData("D1 28", 2, Code.Shr_rm16_1, MemorySize.UInt16)]
		[InlineData("D1 30", 2, Code.Shl_rm16_1, MemorySize.UInt16)]
		[InlineData("D1 38", 2, Code.Sar_rm16_1, MemorySize.Int16)]
		void Test16_Grp2_Ew_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D1 C1", 2, Code.Rol_rm16_1, Register.CX)]
		[InlineData("D1 CA", 2, Code.Ror_rm16_1, Register.DX)]
		[InlineData("D1 D3", 2, Code.Rcl_rm16_1, Register.BX)]
		[InlineData("D1 DC", 2, Code.Rcr_rm16_1, Register.SP)]
		[InlineData("D1 E5", 2, Code.Shl_rm16_1, Register.BP)]
		[InlineData("D1 EE", 2, Code.Shr_rm16_1, Register.SI)]
		[InlineData("D1 F7", 2, Code.Shl_rm16_1, Register.DI)]
		[InlineData("D1 F8", 2, Code.Sar_rm16_1, Register.AX)]
		void Test16_Grp2_Ew_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 D1 00", 3, Code.Rol_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 08", 3, Code.Ror_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 10", 3, Code.Rcl_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 18", 3, Code.Rcr_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 20", 3, Code.Shl_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 28", 3, Code.Shr_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 30", 3, Code.Shl_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 38", 3, Code.Sar_rm16_1, MemorySize.Int16)]
		void Test32_Grp2_Ew_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 D1 C1", 3, Code.Rol_rm16_1, Register.CX)]
		[InlineData("66 D1 CA", 3, Code.Ror_rm16_1, Register.DX)]
		[InlineData("66 D1 D3", 3, Code.Rcl_rm16_1, Register.BX)]
		[InlineData("66 D1 DC", 3, Code.Rcr_rm16_1, Register.SP)]
		[InlineData("66 D1 E5", 3, Code.Shl_rm16_1, Register.BP)]
		[InlineData("66 D1 EE", 3, Code.Shr_rm16_1, Register.SI)]
		[InlineData("66 D1 F7", 3, Code.Shl_rm16_1, Register.DI)]
		[InlineData("66 D1 F8", 3, Code.Sar_rm16_1, Register.AX)]
		void Test32_Grp2_Ew_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 D1 00", 3, Code.Rol_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 08", 3, Code.Ror_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 10", 3, Code.Rcl_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 18", 3, Code.Rcr_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 20", 3, Code.Shl_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 28", 3, Code.Shr_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 30", 3, Code.Shl_rm16_1, MemorySize.UInt16)]
		[InlineData("66 D1 38", 3, Code.Sar_rm16_1, MemorySize.Int16)]
		void Test64_Grp2_Ew_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 D1 C1", 3, Code.Rol_rm16_1, Register.CX)]
		[InlineData("66 D1 CA", 3, Code.Ror_rm16_1, Register.DX)]
		[InlineData("66 D1 D3", 3, Code.Rcl_rm16_1, Register.BX)]
		[InlineData("66 D1 DC", 3, Code.Rcr_rm16_1, Register.SP)]
		[InlineData("66 D1 E5", 3, Code.Shl_rm16_1, Register.BP)]
		[InlineData("66 D1 EE", 3, Code.Shr_rm16_1, Register.SI)]
		[InlineData("66 D1 F7", 3, Code.Shl_rm16_1, Register.DI)]
		[InlineData("66 41 D1 F8", 4, Code.Sar_rm16_1, Register.R8W)]

		[InlineData("66 41 D1 C1", 4, Code.Rol_rm16_1, Register.R9W)]
		[InlineData("66 41 D1 CA", 4, Code.Ror_rm16_1, Register.R10W)]
		[InlineData("66 41 D1 D3", 4, Code.Rcl_rm16_1, Register.R11W)]
		[InlineData("66 41 D1 DC", 4, Code.Rcr_rm16_1, Register.R12W)]
		[InlineData("66 41 D1 E5", 4, Code.Shl_rm16_1, Register.R13W)]
		[InlineData("66 41 D1 EE", 4, Code.Shr_rm16_1, Register.R14W)]
		[InlineData("66 41 D1 F7", 4, Code.Shl_rm16_1, Register.R15W)]
		[InlineData("66 D1 F8", 3, Code.Sar_rm16_1, Register.AX)]
		void Test64_Grp2_Ew_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 D1 00", 3, Code.Rol_rm32_1, MemorySize.UInt32)]
		[InlineData("66 D1 08", 3, Code.Ror_rm32_1, MemorySize.UInt32)]
		[InlineData("66 D1 10", 3, Code.Rcl_rm32_1, MemorySize.UInt32)]
		[InlineData("66 D1 18", 3, Code.Rcr_rm32_1, MemorySize.UInt32)]
		[InlineData("66 D1 20", 3, Code.Shl_rm32_1, MemorySize.UInt32)]
		[InlineData("66 D1 28", 3, Code.Shr_rm32_1, MemorySize.UInt32)]
		[InlineData("66 D1 30", 3, Code.Shl_rm32_1, MemorySize.UInt32)]
		[InlineData("66 D1 38", 3, Code.Sar_rm32_1, MemorySize.Int32)]
		void Test16_Grp2_Ed_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 D1 C1", 3, Code.Rol_rm32_1, Register.ECX)]
		[InlineData("66 D1 CA", 3, Code.Ror_rm32_1, Register.EDX)]
		[InlineData("66 D1 D3", 3, Code.Rcl_rm32_1, Register.EBX)]
		[InlineData("66 D1 DC", 3, Code.Rcr_rm32_1, Register.ESP)]
		[InlineData("66 D1 E5", 3, Code.Shl_rm32_1, Register.EBP)]
		[InlineData("66 D1 EE", 3, Code.Shr_rm32_1, Register.ESI)]
		[InlineData("66 D1 F7", 3, Code.Shl_rm32_1, Register.EDI)]
		[InlineData("66 D1 F8", 3, Code.Sar_rm32_1, Register.EAX)]
		void Test16_Grp2_Ed_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D1 00", 2, Code.Rol_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 08", 2, Code.Ror_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 10", 2, Code.Rcl_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 18", 2, Code.Rcr_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 20", 2, Code.Shl_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 28", 2, Code.Shr_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 30", 2, Code.Shl_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 38", 2, Code.Sar_rm32_1, MemorySize.Int32)]
		void Test32_Grp2_Ed_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D1 C1", 2, Code.Rol_rm32_1, Register.ECX)]
		[InlineData("D1 CA", 2, Code.Ror_rm32_1, Register.EDX)]
		[InlineData("D1 D3", 2, Code.Rcl_rm32_1, Register.EBX)]
		[InlineData("D1 DC", 2, Code.Rcr_rm32_1, Register.ESP)]
		[InlineData("D1 E5", 2, Code.Shl_rm32_1, Register.EBP)]
		[InlineData("D1 EE", 2, Code.Shr_rm32_1, Register.ESI)]
		[InlineData("D1 F7", 2, Code.Shl_rm32_1, Register.EDI)]
		[InlineData("D1 F8", 2, Code.Sar_rm32_1, Register.EAX)]
		void Test32_Grp2_Ed_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D1 00", 2, Code.Rol_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 08", 2, Code.Ror_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 10", 2, Code.Rcl_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 18", 2, Code.Rcr_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 20", 2, Code.Shl_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 28", 2, Code.Shr_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 30", 2, Code.Shl_rm32_1, MemorySize.UInt32)]
		[InlineData("D1 38", 2, Code.Sar_rm32_1, MemorySize.Int32)]
		void Test64_Grp2_Ed_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D1 C1", 2, Code.Rol_rm32_1, Register.ECX)]
		[InlineData("D1 CA", 2, Code.Ror_rm32_1, Register.EDX)]
		[InlineData("D1 D3", 2, Code.Rcl_rm32_1, Register.EBX)]
		[InlineData("D1 DC", 2, Code.Rcr_rm32_1, Register.ESP)]
		[InlineData("D1 E5", 2, Code.Shl_rm32_1, Register.EBP)]
		[InlineData("D1 EE", 2, Code.Shr_rm32_1, Register.ESI)]
		[InlineData("D1 F7", 2, Code.Shl_rm32_1, Register.EDI)]
		[InlineData("41 D1 F8", 3, Code.Sar_rm32_1, Register.R8D)]

		[InlineData("41 D1 C1", 3, Code.Rol_rm32_1, Register.R9D)]
		[InlineData("41 D1 CA", 3, Code.Ror_rm32_1, Register.R10D)]
		[InlineData("41 D1 D3", 3, Code.Rcl_rm32_1, Register.R11D)]
		[InlineData("41 D1 DC", 3, Code.Rcr_rm32_1, Register.R12D)]
		[InlineData("41 D1 E5", 3, Code.Shl_rm32_1, Register.R13D)]
		[InlineData("41 D1 EE", 3, Code.Shr_rm32_1, Register.R14D)]
		[InlineData("41 D1 F7", 3, Code.Shl_rm32_1, Register.R15D)]
		[InlineData("D1 F8", 2, Code.Sar_rm32_1, Register.EAX)]
		void Test64_Grp2_Ed_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("48 D1 00", 3, Code.Rol_rm64_1, MemorySize.UInt64)]
		[InlineData("48 D1 08", 3, Code.Ror_rm64_1, MemorySize.UInt64)]
		[InlineData("48 D1 10", 3, Code.Rcl_rm64_1, MemorySize.UInt64)]
		[InlineData("48 D1 18", 3, Code.Rcr_rm64_1, MemorySize.UInt64)]
		[InlineData("48 D1 20", 3, Code.Shl_rm64_1, MemorySize.UInt64)]
		[InlineData("48 D1 28", 3, Code.Shr_rm64_1, MemorySize.UInt64)]
		[InlineData("48 D1 30", 3, Code.Shl_rm64_1, MemorySize.UInt64)]
		[InlineData("48 D1 38", 3, Code.Sar_rm64_1, MemorySize.Int64)]
		void Test64_Grp2_Eq_1_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("48 D1 C1", 3, Code.Rol_rm64_1, Register.RCX)]
		[InlineData("48 D1 CA", 3, Code.Ror_rm64_1, Register.RDX)]
		[InlineData("48 D1 D3", 3, Code.Rcl_rm64_1, Register.RBX)]
		[InlineData("48 D1 DC", 3, Code.Rcr_rm64_1, Register.RSP)]
		[InlineData("48 D1 E5", 3, Code.Shl_rm64_1, Register.RBP)]
		[InlineData("48 D1 EE", 3, Code.Shr_rm64_1, Register.RSI)]
		[InlineData("48 D1 F7", 3, Code.Shl_rm64_1, Register.RDI)]
		[InlineData("49 D1 F8", 3, Code.Sar_rm64_1, Register.R8)]

		[InlineData("49 D1 C1", 3, Code.Rol_rm64_1, Register.R9)]
		[InlineData("49 D1 CA", 3, Code.Ror_rm64_1, Register.R10)]
		[InlineData("49 D1 D3", 3, Code.Rcl_rm64_1, Register.R11)]
		[InlineData("49 D1 DC", 3, Code.Rcr_rm64_1, Register.R12)]
		[InlineData("49 D1 E5", 3, Code.Shl_rm64_1, Register.R13)]
		[InlineData("49 D1 EE", 3, Code.Shr_rm64_1, Register.R14)]
		[InlineData("49 D1 F7", 3, Code.Shl_rm64_1, Register.R15)]
		[InlineData("48 D1 F8", 3, Code.Sar_rm64_1, Register.RAX)]
		void Test64_Grp2_Eq_1_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(1, instr.Immediate8);
		}

		[Theory]
		[InlineData("D2 00", 2, Code.Rol_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 08", 2, Code.Ror_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 10", 2, Code.Rcl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 18", 2, Code.Rcr_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 20", 2, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 28", 2, Code.Shr_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 30", 2, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 38", 2, Code.Sar_rm8_CL, MemorySize.Int8)]
		void Test16_Grp2_Eb_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D2 C1", 2, Code.Rol_rm8_CL, Register.CL)]
		[InlineData("D2 CA", 2, Code.Ror_rm8_CL, Register.DL)]
		[InlineData("D2 D3", 2, Code.Rcl_rm8_CL, Register.BL)]
		[InlineData("D2 DC", 2, Code.Rcr_rm8_CL, Register.AH)]
		[InlineData("D2 E5", 2, Code.Shl_rm8_CL, Register.CH)]
		[InlineData("D2 EE", 2, Code.Shr_rm8_CL, Register.DH)]
		[InlineData("D2 F7", 2, Code.Shl_rm8_CL, Register.BH)]
		[InlineData("D2 F8", 2, Code.Sar_rm8_CL, Register.AL)]
		void Test16_Grp2_Eb_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D2 00", 2, Code.Rol_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 08", 2, Code.Ror_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 10", 2, Code.Rcl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 18", 2, Code.Rcr_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 20", 2, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 28", 2, Code.Shr_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 30", 2, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 38", 2, Code.Sar_rm8_CL, MemorySize.Int8)]
		void Test32_Grp2_Eb_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D2 C1", 2, Code.Rol_rm8_CL, Register.CL)]
		[InlineData("D2 CA", 2, Code.Ror_rm8_CL, Register.DL)]
		[InlineData("D2 D3", 2, Code.Rcl_rm8_CL, Register.BL)]
		[InlineData("D2 DC", 2, Code.Rcr_rm8_CL, Register.AH)]
		[InlineData("D2 E5", 2, Code.Shl_rm8_CL, Register.CH)]
		[InlineData("D2 EE", 2, Code.Shr_rm8_CL, Register.DH)]
		[InlineData("D2 F7", 2, Code.Shl_rm8_CL, Register.BH)]
		[InlineData("D2 F8", 2, Code.Sar_rm8_CL, Register.AL)]
		void Test32_Grp2_Eb_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D2 00", 2, Code.Rol_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 08", 2, Code.Ror_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 10", 2, Code.Rcl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 18", 2, Code.Rcr_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 20", 2, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 28", 2, Code.Shr_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 30", 2, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("D2 38", 2, Code.Sar_rm8_CL, MemorySize.Int8)]

		[InlineData("44 D2 00", 3, Code.Rol_rm8_CL, MemorySize.UInt8)]
		[InlineData("44 D2 08", 3, Code.Ror_rm8_CL, MemorySize.UInt8)]
		[InlineData("44 D2 10", 3, Code.Rcl_rm8_CL, MemorySize.UInt8)]
		[InlineData("44 D2 18", 3, Code.Rcr_rm8_CL, MemorySize.UInt8)]
		[InlineData("44 D2 20", 3, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("44 D2 28", 3, Code.Shr_rm8_CL, MemorySize.UInt8)]
		[InlineData("44 D2 30", 3, Code.Shl_rm8_CL, MemorySize.UInt8)]
		[InlineData("44 D2 38", 3, Code.Sar_rm8_CL, MemorySize.Int8)]
		void Test64_Grp2_Eb_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D2 C1", 2, Code.Rol_rm8_CL, Register.CL)]
		[InlineData("D2 CA", 2, Code.Ror_rm8_CL, Register.DL)]
		[InlineData("D2 D3", 2, Code.Rcl_rm8_CL, Register.BL)]
		[InlineData("D2 DC", 2, Code.Rcr_rm8_CL, Register.AH)]
		[InlineData("D2 E5", 2, Code.Shl_rm8_CL, Register.CH)]
		[InlineData("D2 EE", 2, Code.Shr_rm8_CL, Register.DH)]
		[InlineData("D2 F7", 2, Code.Shl_rm8_CL, Register.BH)]
		[InlineData("D2 F8", 2, Code.Sar_rm8_CL, Register.AL)]

		[InlineData("40 D2 C1", 3, Code.Rol_rm8_CL, Register.CL)]
		[InlineData("40 D2 CA", 3, Code.Ror_rm8_CL, Register.DL)]
		[InlineData("40 D2 D3", 3, Code.Rcl_rm8_CL, Register.BL)]
		[InlineData("40 D2 DC", 3, Code.Rcr_rm8_CL, Register.SPL)]
		[InlineData("40 D2 E5", 3, Code.Shl_rm8_CL, Register.BPL)]
		[InlineData("40 D2 EE", 3, Code.Shr_rm8_CL, Register.SIL)]
		[InlineData("40 D2 F7", 3, Code.Shl_rm8_CL, Register.DIL)]
		[InlineData("41 D2 F8", 3, Code.Sar_rm8_CL, Register.R8L)]
		[InlineData("41 D2 C1", 3, Code.Rol_rm8_CL, Register.R9L)]
		[InlineData("41 D2 CA", 3, Code.Ror_rm8_CL, Register.R10L)]
		[InlineData("41 D2 D3", 3, Code.Rcl_rm8_CL, Register.R11L)]
		[InlineData("41 D2 DC", 3, Code.Rcr_rm8_CL, Register.R12L)]
		[InlineData("41 D2 E5", 3, Code.Shl_rm8_CL, Register.R13L)]
		[InlineData("41 D2 EE", 3, Code.Shr_rm8_CL, Register.R14L)]
		[InlineData("41 D2 F7", 3, Code.Shl_rm8_CL, Register.R15L)]
		[InlineData("40 D2 F8", 3, Code.Sar_rm8_CL, Register.AL)]
		void Test64_Grp2_Eb_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D3 00", 2, Code.Rol_rm16_CL, MemorySize.UInt16)]
		[InlineData("D3 08", 2, Code.Ror_rm16_CL, MemorySize.UInt16)]
		[InlineData("D3 10", 2, Code.Rcl_rm16_CL, MemorySize.UInt16)]
		[InlineData("D3 18", 2, Code.Rcr_rm16_CL, MemorySize.UInt16)]
		[InlineData("D3 20", 2, Code.Shl_rm16_CL, MemorySize.UInt16)]
		[InlineData("D3 28", 2, Code.Shr_rm16_CL, MemorySize.UInt16)]
		[InlineData("D3 30", 2, Code.Shl_rm16_CL, MemorySize.UInt16)]
		[InlineData("D3 38", 2, Code.Sar_rm16_CL, MemorySize.Int16)]
		void Test16_Grp2_Ew_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D3 C1", 2, Code.Rol_rm16_CL, Register.CX)]
		[InlineData("D3 CA", 2, Code.Ror_rm16_CL, Register.DX)]
		[InlineData("D3 D3", 2, Code.Rcl_rm16_CL, Register.BX)]
		[InlineData("D3 DC", 2, Code.Rcr_rm16_CL, Register.SP)]
		[InlineData("D3 E5", 2, Code.Shl_rm16_CL, Register.BP)]
		[InlineData("D3 EE", 2, Code.Shr_rm16_CL, Register.SI)]
		[InlineData("D3 F7", 2, Code.Shl_rm16_CL, Register.DI)]
		[InlineData("D3 F8", 2, Code.Sar_rm16_CL, Register.AX)]
		void Test16_Grp2_Ew_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 D3 00", 3, Code.Rol_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 08", 3, Code.Ror_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 10", 3, Code.Rcl_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 18", 3, Code.Rcr_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 20", 3, Code.Shl_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 28", 3, Code.Shr_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 30", 3, Code.Shl_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 38", 3, Code.Sar_rm16_CL, MemorySize.Int16)]
		void Test32_Grp2_Ew_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 D3 C1", 3, Code.Rol_rm16_CL, Register.CX)]
		[InlineData("66 D3 CA", 3, Code.Ror_rm16_CL, Register.DX)]
		[InlineData("66 D3 D3", 3, Code.Rcl_rm16_CL, Register.BX)]
		[InlineData("66 D3 DC", 3, Code.Rcr_rm16_CL, Register.SP)]
		[InlineData("66 D3 E5", 3, Code.Shl_rm16_CL, Register.BP)]
		[InlineData("66 D3 EE", 3, Code.Shr_rm16_CL, Register.SI)]
		[InlineData("66 D3 F7", 3, Code.Shl_rm16_CL, Register.DI)]
		[InlineData("66 D3 F8", 3, Code.Sar_rm16_CL, Register.AX)]
		void Test32_Grp2_Ew_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 D3 00", 3, Code.Rol_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 08", 3, Code.Ror_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 10", 3, Code.Rcl_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 18", 3, Code.Rcr_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 20", 3, Code.Shl_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 28", 3, Code.Shr_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 30", 3, Code.Shl_rm16_CL, MemorySize.UInt16)]
		[InlineData("66 D3 38", 3, Code.Sar_rm16_CL, MemorySize.Int16)]
		void Test64_Grp2_Ew_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 D3 C1", 3, Code.Rol_rm16_CL, Register.CX)]
		[InlineData("66 D3 CA", 3, Code.Ror_rm16_CL, Register.DX)]
		[InlineData("66 D3 D3", 3, Code.Rcl_rm16_CL, Register.BX)]
		[InlineData("66 D3 DC", 3, Code.Rcr_rm16_CL, Register.SP)]
		[InlineData("66 D3 E5", 3, Code.Shl_rm16_CL, Register.BP)]
		[InlineData("66 D3 EE", 3, Code.Shr_rm16_CL, Register.SI)]
		[InlineData("66 D3 F7", 3, Code.Shl_rm16_CL, Register.DI)]
		[InlineData("66 41 D3 F8", 4, Code.Sar_rm16_CL, Register.R8W)]

		[InlineData("66 41 D3 C1", 4, Code.Rol_rm16_CL, Register.R9W)]
		[InlineData("66 41 D3 CA", 4, Code.Ror_rm16_CL, Register.R10W)]
		[InlineData("66 41 D3 D3", 4, Code.Rcl_rm16_CL, Register.R11W)]
		[InlineData("66 41 D3 DC", 4, Code.Rcr_rm16_CL, Register.R12W)]
		[InlineData("66 41 D3 E5", 4, Code.Shl_rm16_CL, Register.R13W)]
		[InlineData("66 41 D3 EE", 4, Code.Shr_rm16_CL, Register.R14W)]
		[InlineData("66 41 D3 F7", 4, Code.Shl_rm16_CL, Register.R15W)]
		[InlineData("66 D3 F8", 3, Code.Sar_rm16_CL, Register.AX)]
		void Test64_Grp2_Ew_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 D3 00", 3, Code.Rol_rm32_CL, MemorySize.UInt32)]
		[InlineData("66 D3 08", 3, Code.Ror_rm32_CL, MemorySize.UInt32)]
		[InlineData("66 D3 10", 3, Code.Rcl_rm32_CL, MemorySize.UInt32)]
		[InlineData("66 D3 18", 3, Code.Rcr_rm32_CL, MemorySize.UInt32)]
		[InlineData("66 D3 20", 3, Code.Shl_rm32_CL, MemorySize.UInt32)]
		[InlineData("66 D3 28", 3, Code.Shr_rm32_CL, MemorySize.UInt32)]
		[InlineData("66 D3 30", 3, Code.Shl_rm32_CL, MemorySize.UInt32)]
		[InlineData("66 D3 38", 3, Code.Sar_rm32_CL, MemorySize.Int32)]
		void Test16_Grp2_Ed_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 D3 C1", 3, Code.Rol_rm32_CL, Register.ECX)]
		[InlineData("66 D3 CA", 3, Code.Ror_rm32_CL, Register.EDX)]
		[InlineData("66 D3 D3", 3, Code.Rcl_rm32_CL, Register.EBX)]
		[InlineData("66 D3 DC", 3, Code.Rcr_rm32_CL, Register.ESP)]
		[InlineData("66 D3 E5", 3, Code.Shl_rm32_CL, Register.EBP)]
		[InlineData("66 D3 EE", 3, Code.Shr_rm32_CL, Register.ESI)]
		[InlineData("66 D3 F7", 3, Code.Shl_rm32_CL, Register.EDI)]
		[InlineData("66 D3 F8", 3, Code.Sar_rm32_CL, Register.EAX)]
		void Test16_Grp2_Ed_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D3 00", 2, Code.Rol_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 08", 2, Code.Ror_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 10", 2, Code.Rcl_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 18", 2, Code.Rcr_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 20", 2, Code.Shl_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 28", 2, Code.Shr_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 30", 2, Code.Shl_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 38", 2, Code.Sar_rm32_CL, MemorySize.Int32)]
		void Test32_Grp2_Ed_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D3 C1", 2, Code.Rol_rm32_CL, Register.ECX)]
		[InlineData("D3 CA", 2, Code.Ror_rm32_CL, Register.EDX)]
		[InlineData("D3 D3", 2, Code.Rcl_rm32_CL, Register.EBX)]
		[InlineData("D3 DC", 2, Code.Rcr_rm32_CL, Register.ESP)]
		[InlineData("D3 E5", 2, Code.Shl_rm32_CL, Register.EBP)]
		[InlineData("D3 EE", 2, Code.Shr_rm32_CL, Register.ESI)]
		[InlineData("D3 F7", 2, Code.Shl_rm32_CL, Register.EDI)]
		[InlineData("D3 F8", 2, Code.Sar_rm32_CL, Register.EAX)]
		void Test32_Grp2_Ed_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D3 00", 2, Code.Rol_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 08", 2, Code.Ror_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 10", 2, Code.Rcl_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 18", 2, Code.Rcr_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 20", 2, Code.Shl_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 28", 2, Code.Shr_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 30", 2, Code.Shl_rm32_CL, MemorySize.UInt32)]
		[InlineData("D3 38", 2, Code.Sar_rm32_CL, MemorySize.Int32)]
		void Test64_Grp2_Ed_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D3 C1", 2, Code.Rol_rm32_CL, Register.ECX)]
		[InlineData("D3 CA", 2, Code.Ror_rm32_CL, Register.EDX)]
		[InlineData("D3 D3", 2, Code.Rcl_rm32_CL, Register.EBX)]
		[InlineData("D3 DC", 2, Code.Rcr_rm32_CL, Register.ESP)]
		[InlineData("D3 E5", 2, Code.Shl_rm32_CL, Register.EBP)]
		[InlineData("D3 EE", 2, Code.Shr_rm32_CL, Register.ESI)]
		[InlineData("D3 F7", 2, Code.Shl_rm32_CL, Register.EDI)]
		[InlineData("41 D3 F8", 3, Code.Sar_rm32_CL, Register.R8D)]

		[InlineData("41 D3 C1", 3, Code.Rol_rm32_CL, Register.R9D)]
		[InlineData("41 D3 CA", 3, Code.Ror_rm32_CL, Register.R10D)]
		[InlineData("41 D3 D3", 3, Code.Rcl_rm32_CL, Register.R11D)]
		[InlineData("41 D3 DC", 3, Code.Rcr_rm32_CL, Register.R12D)]
		[InlineData("41 D3 E5", 3, Code.Shl_rm32_CL, Register.R13D)]
		[InlineData("41 D3 EE", 3, Code.Shr_rm32_CL, Register.R14D)]
		[InlineData("41 D3 F7", 3, Code.Shl_rm32_CL, Register.R15D)]
		[InlineData("D3 F8", 2, Code.Sar_rm32_CL, Register.EAX)]
		void Test64_Grp2_Ed_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 D3 00", 3, Code.Rol_rm64_CL, MemorySize.UInt64)]
		[InlineData("48 D3 08", 3, Code.Ror_rm64_CL, MemorySize.UInt64)]
		[InlineData("48 D3 10", 3, Code.Rcl_rm64_CL, MemorySize.UInt64)]
		[InlineData("48 D3 18", 3, Code.Rcr_rm64_CL, MemorySize.UInt64)]
		[InlineData("48 D3 20", 3, Code.Shl_rm64_CL, MemorySize.UInt64)]
		[InlineData("48 D3 28", 3, Code.Shr_rm64_CL, MemorySize.UInt64)]
		[InlineData("48 D3 30", 3, Code.Shl_rm64_CL, MemorySize.UInt64)]
		[InlineData("48 D3 38", 3, Code.Sar_rm64_CL, MemorySize.Int64)]
		void Test64_Grp2_Eq_CL_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 D3 C1", 3, Code.Rol_rm64_CL, Register.RCX)]
		[InlineData("48 D3 CA", 3, Code.Ror_rm64_CL, Register.RDX)]
		[InlineData("48 D3 D3", 3, Code.Rcl_rm64_CL, Register.RBX)]
		[InlineData("48 D3 DC", 3, Code.Rcr_rm64_CL, Register.RSP)]
		[InlineData("48 D3 E5", 3, Code.Shl_rm64_CL, Register.RBP)]
		[InlineData("48 D3 EE", 3, Code.Shr_rm64_CL, Register.RSI)]
		[InlineData("48 D3 F7", 3, Code.Shl_rm64_CL, Register.RDI)]
		[InlineData("49 D3 F8", 3, Code.Sar_rm64_CL, Register.R8)]

		[InlineData("49 D3 C1", 3, Code.Rol_rm64_CL, Register.R9)]
		[InlineData("49 D3 CA", 3, Code.Ror_rm64_CL, Register.R10)]
		[InlineData("49 D3 D3", 3, Code.Rcl_rm64_CL, Register.R11)]
		[InlineData("49 D3 DC", 3, Code.Rcr_rm64_CL, Register.R12)]
		[InlineData("49 D3 E5", 3, Code.Shl_rm64_CL, Register.R13)]
		[InlineData("49 D3 EE", 3, Code.Shr_rm64_CL, Register.R14)]
		[InlineData("49 D3 F7", 3, Code.Shl_rm64_CL, Register.R15)]
		[InlineData("48 D3 F8", 3, Code.Sar_rm64_CL, Register.RAX)]
		void Test64_Grp2_Eq_CL_2(string hexBytes, int byteLength, Code code, Register reg) {
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CL, instr.Op1Register);
		}

		[Theory]
		[InlineData("D4 0A", 2, 0x0A)]
		[InlineData("D4 A5", 2, 0xA5)]
		[InlineData("D4 5A", 2, 0x5A)]
		void Test16_Aam_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Aam_imm8, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("D4 0A", 2, 0x0A)]
		[InlineData("D4 A5", 2, 0xA5)]
		[InlineData("D4 5A", 2, 0x5A)]
		void Test32_Aam_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Aam_imm8, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("D5 0A", 2, 0x0A)]
		[InlineData("D5 A5", 2, 0xA5)]
		[InlineData("D5 5A", 2, 0x5A)]
		void Test16_Aad_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Aad_imm8, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("D5 0A", 2, 0x0A)]
		[InlineData("D5 A5", 2, 0xA5)]
		[InlineData("D5 5A", 2, 0x5A)]
		void Test32_Aad_imm8_1(string hexBytes, int byteLength, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Aad_imm8, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate8, instr.Op0Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("D6", 1)]
		[InlineData("66 D6", 2)]
		void Test16_Salc_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Salc, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("D6", 1)]
		[InlineData("66 D6", 2)]
		void Test32_Salc_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Salc, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("D7", 1, Register.BX, Register.DS, Register.None)]
		[InlineData("26 D7", 2, Register.BX, Register.ES, Register.ES)]
		[InlineData("2E D7", 2, Register.BX, Register.CS, Register.CS)]
		[InlineData("36 D7", 2, Register.BX, Register.SS, Register.SS)]
		[InlineData("3E D7", 2, Register.BX, Register.DS, Register.DS)]
		[InlineData("64 D7", 2, Register.BX, Register.FS, Register.FS)]
		[InlineData("65 D7", 2, Register.BX, Register.GS, Register.GS)]
		[InlineData("66 D7", 2, Register.BX, Register.DS, Register.None)]

		[InlineData("67 D7", 2, Register.EBX, Register.DS, Register.None)]
		[InlineData("26 67 D7", 3, Register.EBX, Register.ES, Register.ES)]
		[InlineData("2E 67 D7", 3, Register.EBX, Register.CS, Register.CS)]
		[InlineData("36 67 D7", 3, Register.EBX, Register.SS, Register.SS)]
		[InlineData("3E 67 D7", 3, Register.EBX, Register.DS, Register.DS)]
		[InlineData("64 67 D7", 3, Register.EBX, Register.FS, Register.FS)]
		[InlineData("65 67 D7", 3, Register.EBX, Register.GS, Register.GS)]
		[InlineData("66 67 D7", 3, Register.EBX, Register.DS, Register.None)]
		void Test16_Xlatb_1(string hexBytes, int byteLength, Register baseReg, Register seg, Register segPrefix) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xlatb, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(baseReg, instr.MemoryBase);
			Assert.Equal(Register.AL, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("D7", 1, Register.EBX, Register.DS, Register.None)]
		[InlineData("26 D7", 2, Register.EBX, Register.ES, Register.ES)]
		[InlineData("2E D7", 2, Register.EBX, Register.CS, Register.CS)]
		[InlineData("36 D7", 2, Register.EBX, Register.SS, Register.SS)]
		[InlineData("3E D7", 2, Register.EBX, Register.DS, Register.DS)]
		[InlineData("64 D7", 2, Register.EBX, Register.FS, Register.FS)]
		[InlineData("65 D7", 2, Register.EBX, Register.GS, Register.GS)]
		[InlineData("66 D7", 2, Register.EBX, Register.DS, Register.None)]

		[InlineData("67 D7", 2, Register.BX, Register.DS, Register.None)]
		[InlineData("26 67 D7", 3, Register.BX, Register.ES, Register.ES)]
		[InlineData("2E 67 D7", 3, Register.BX, Register.CS, Register.CS)]
		[InlineData("36 67 D7", 3, Register.BX, Register.SS, Register.SS)]
		[InlineData("3E 67 D7", 3, Register.BX, Register.DS, Register.DS)]
		[InlineData("64 67 D7", 3, Register.BX, Register.FS, Register.FS)]
		[InlineData("65 67 D7", 3, Register.BX, Register.GS, Register.GS)]
		[InlineData("66 67 D7", 3, Register.BX, Register.DS, Register.None)]
		void Test32_Xlatb_1(string hexBytes, int byteLength, Register baseReg, Register seg, Register segPrefix) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xlatb, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(baseReg, instr.MemoryBase);
			Assert.Equal(Register.AL, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("D7", 1, Register.RBX, Register.DS, Register.None)]
		[InlineData("26 D7", 2, Register.RBX, Register.ES, Register.ES)]
		[InlineData("2E D7", 2, Register.RBX, Register.CS, Register.CS)]
		[InlineData("36 D7", 2, Register.RBX, Register.SS, Register.SS)]
		[InlineData("3E D7", 2, Register.RBX, Register.DS, Register.DS)]
		[InlineData("64 D7", 2, Register.RBX, Register.FS, Register.FS)]
		[InlineData("65 D7", 2, Register.RBX, Register.GS, Register.GS)]
		[InlineData("66 D7", 2, Register.RBX, Register.DS, Register.None)]

		[InlineData("67 D7", 2, Register.EBX, Register.DS, Register.None)]
		[InlineData("26 67 D7", 3, Register.EBX, Register.ES, Register.ES)]
		[InlineData("2E 67 D7", 3, Register.EBX, Register.CS, Register.CS)]
		[InlineData("36 67 D7", 3, Register.EBX, Register.SS, Register.SS)]
		[InlineData("3E 67 D7", 3, Register.EBX, Register.DS, Register.DS)]
		[InlineData("64 67 D7", 3, Register.EBX, Register.FS, Register.FS)]
		[InlineData("65 67 D7", 3, Register.EBX, Register.GS, Register.GS)]
		[InlineData("66 67 D7", 3, Register.EBX, Register.DS, Register.None)]
		void Test64_Xlatb_1(string hexBytes, int byteLength, Register baseReg, Register seg, Register segPrefix) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Xlatb, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segPrefix, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(seg, instr.MemorySegment);
			Assert.Equal(baseReg, instr.MemoryBase);
			Assert.Equal(Register.AL, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
	}
}
