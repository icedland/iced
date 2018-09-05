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
	public sealed class DecoderTest_1_C0_C7 : DecoderTest {
		[Theory]
		[InlineData("C0 00 5A", 3, Code.Rol_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 08 A5", 3, Code.Ror_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 10 5A", 3, Code.Rcl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 18 A5", 3, Code.Rcr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 20 5A", 3, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 28 A5", 3, Code.Shr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 30 5A", 3, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 38 A5", 3, Code.Sar_Eb_Ib, 0xA5, MemorySize.Int8)]
		void Test16_Grp2_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C0 C1 5A", 3, Code.Rol_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("C0 CA A5", 3, Code.Ror_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("C0 D3 5A", 3, Code.Rcl_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("C0 DC A5", 3, Code.Rcr_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("C0 E5 5A", 3, Code.Shl_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("C0 EE A5", 3, Code.Shr_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("C0 F7 5A", 3, Code.Shl_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("C0 F8 A5", 3, Code.Sar_Eb_Ib, Register.AL, 0xA5)]
		void Test16_Grp2_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C0 00 5A", 3, Code.Rol_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 08 A5", 3, Code.Ror_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 10 5A", 3, Code.Rcl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 18 A5", 3, Code.Rcr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 20 5A", 3, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 28 A5", 3, Code.Shr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 30 5A", 3, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 38 A5", 3, Code.Sar_Eb_Ib, 0xA5, MemorySize.Int8)]
		void Test32_Grp2_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C0 C1 5A", 3, Code.Rol_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("C0 CA A5", 3, Code.Ror_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("C0 D3 5A", 3, Code.Rcl_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("C0 DC A5", 3, Code.Rcr_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("C0 E5 5A", 3, Code.Shl_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("C0 EE A5", 3, Code.Shr_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("C0 F7 5A", 3, Code.Shl_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("C0 F8 A5", 3, Code.Sar_Eb_Ib, Register.AL, 0xA5)]
		void Test32_Grp2_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C0 00 5A", 3, Code.Rol_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 08 A5", 3, Code.Ror_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 10 5A", 3, Code.Rcl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 18 A5", 3, Code.Rcr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 20 5A", 3, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 28 A5", 3, Code.Shr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("C0 30 5A", 3, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("C0 38 A5", 3, Code.Sar_Eb_Ib, 0xA5, MemorySize.Int8)]

		[InlineData("44 C0 00 5A", 4, Code.Rol_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("44 C0 08 A5", 4, Code.Ror_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("44 C0 10 5A", 4, Code.Rcl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("44 C0 18 A5", 4, Code.Rcr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("44 C0 20 5A", 4, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("44 C0 28 A5", 4, Code.Shr_Eb_Ib, 0xA5, MemorySize.UInt8)]
		[InlineData("44 C0 30 5A", 4, Code.Shl_Eb_Ib, 0x5A, MemorySize.UInt8)]
		[InlineData("44 C0 38 A5", 4, Code.Sar_Eb_Ib, 0xA5, MemorySize.Int8)]
		void Test64_Grp2_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C0 C1 5A", 3, Code.Rol_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("C0 CA A5", 3, Code.Ror_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("C0 D3 5A", 3, Code.Rcl_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("C0 DC A5", 3, Code.Rcr_Eb_Ib, Register.AH, 0xA5)]
		[InlineData("C0 E5 5A", 3, Code.Shl_Eb_Ib, Register.CH, 0x5A)]
		[InlineData("C0 EE A5", 3, Code.Shr_Eb_Ib, Register.DH, 0xA5)]
		[InlineData("C0 F7 5A", 3, Code.Shl_Eb_Ib, Register.BH, 0x5A)]
		[InlineData("C0 F8 A5", 3, Code.Sar_Eb_Ib, Register.AL, 0xA5)]

		[InlineData("40 C0 C1 5A", 4, Code.Rol_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("40 C0 CA A5", 4, Code.Ror_Eb_Ib, Register.DL, 0xA5)]
		[InlineData("40 C0 D3 5A", 4, Code.Rcl_Eb_Ib, Register.BL, 0x5A)]
		[InlineData("40 C0 DC A5", 4, Code.Rcr_Eb_Ib, Register.SPL, 0xA5)]
		[InlineData("40 C0 E5 5A", 4, Code.Shl_Eb_Ib, Register.BPL, 0x5A)]
		[InlineData("40 C0 EE A5", 4, Code.Shr_Eb_Ib, Register.SIL, 0xA5)]
		[InlineData("40 C0 F7 5A", 4, Code.Shl_Eb_Ib, Register.DIL, 0x5A)]
		[InlineData("41 C0 F8 A5", 4, Code.Sar_Eb_Ib, Register.R8L, 0xA5)]
		[InlineData("41 C0 C1 5A", 4, Code.Rol_Eb_Ib, Register.R9L, 0x5A)]
		[InlineData("41 C0 CA A5", 4, Code.Ror_Eb_Ib, Register.R10L, 0xA5)]
		[InlineData("41 C0 D3 5A", 4, Code.Rcl_Eb_Ib, Register.R11L, 0x5A)]
		[InlineData("41 C0 DC A5", 4, Code.Rcr_Eb_Ib, Register.R12L, 0xA5)]
		[InlineData("41 C0 E5 5A", 4, Code.Shl_Eb_Ib, Register.R13L, 0x5A)]
		[InlineData("41 C0 EE A5", 4, Code.Shr_Eb_Ib, Register.R14L, 0xA5)]
		[InlineData("41 C0 F7 5A", 4, Code.Shl_Eb_Ib, Register.R15L, 0x5A)]
		[InlineData("40 C0 F8 A5", 4, Code.Sar_Eb_Ib, Register.AL, 0xA5)]
		void Test64_Grp2_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C1 00 5A", 3, Code.Rol_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("C1 08 A5", 3, Code.Ror_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("C1 10 5A", 3, Code.Rcl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("C1 18 A5", 3, Code.Rcr_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("C1 20 5A", 3, Code.Shl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("C1 28 A5", 3, Code.Shr_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("C1 30 5A", 3, Code.Shl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("C1 38 A5", 3, Code.Sar_Ew_Ib, 0xA5, MemorySize.Int16)]
		void Test16_Grp2_Ew_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C1 C1 5A", 3, Code.Rol_Ew_Ib, Register.CX, 0x5A)]
		[InlineData("C1 CA A5", 3, Code.Ror_Ew_Ib, Register.DX, 0xA5)]
		[InlineData("C1 D3 5A", 3, Code.Rcl_Ew_Ib, Register.BX, 0x5A)]
		[InlineData("C1 DC A5", 3, Code.Rcr_Ew_Ib, Register.SP, 0xA5)]
		[InlineData("C1 E5 5A", 3, Code.Shl_Ew_Ib, Register.BP, 0x5A)]
		[InlineData("C1 EE A5", 3, Code.Shr_Ew_Ib, Register.SI, 0xA5)]
		[InlineData("C1 F7 5A", 3, Code.Shl_Ew_Ib, Register.DI, 0x5A)]
		[InlineData("C1 F8 A5", 3, Code.Sar_Ew_Ib, Register.AX, 0xA5)]
		void Test16_Grp2_Ew_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 C1 00 5A", 4, Code.Rol_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 08 A5", 4, Code.Ror_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("66 C1 10 5A", 4, Code.Rcl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 18 A5", 4, Code.Rcr_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("66 C1 20 5A", 4, Code.Shl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 28 A5", 4, Code.Shr_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("66 C1 30 5A", 4, Code.Shl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 38 A5", 4, Code.Sar_Ew_Ib, 0xA5, MemorySize.Int16)]
		void Test32_Grp2_Ew_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 C1 C1 5A", 4, Code.Rol_Ew_Ib, Register.CX, 0x5A)]
		[InlineData("66 C1 CA A5", 4, Code.Ror_Ew_Ib, Register.DX, 0xA5)]
		[InlineData("66 C1 D3 5A", 4, Code.Rcl_Ew_Ib, Register.BX, 0x5A)]
		[InlineData("66 C1 DC A5", 4, Code.Rcr_Ew_Ib, Register.SP, 0xA5)]
		[InlineData("66 C1 E5 5A", 4, Code.Shl_Ew_Ib, Register.BP, 0x5A)]
		[InlineData("66 C1 EE A5", 4, Code.Shr_Ew_Ib, Register.SI, 0xA5)]
		[InlineData("66 C1 F7 5A", 4, Code.Shl_Ew_Ib, Register.DI, 0x5A)]
		[InlineData("66 C1 F8 A5", 4, Code.Sar_Ew_Ib, Register.AX, 0xA5)]
		void Test32_Grp2_Ew_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 C1 00 5A", 4, Code.Rol_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 08 A5", 4, Code.Ror_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("66 C1 10 5A", 4, Code.Rcl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 18 A5", 4, Code.Rcr_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("66 C1 20 5A", 4, Code.Shl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 28 A5", 4, Code.Shr_Ew_Ib, 0xA5, MemorySize.UInt16)]
		[InlineData("66 C1 30 5A", 4, Code.Shl_Ew_Ib, 0x5A, MemorySize.UInt16)]
		[InlineData("66 C1 38 A5", 4, Code.Sar_Ew_Ib, 0xA5, MemorySize.Int16)]
		void Test64_Grp2_Ew_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 C1 C1 5A", 4, Code.Rol_Ew_Ib, Register.CX, 0x5A)]
		[InlineData("66 C1 CA A5", 4, Code.Ror_Ew_Ib, Register.DX, 0xA5)]
		[InlineData("66 C1 D3 5A", 4, Code.Rcl_Ew_Ib, Register.BX, 0x5A)]
		[InlineData("66 C1 DC A5", 4, Code.Rcr_Ew_Ib, Register.SP, 0xA5)]
		[InlineData("66 C1 E5 5A", 4, Code.Shl_Ew_Ib, Register.BP, 0x5A)]
		[InlineData("66 C1 EE A5", 4, Code.Shr_Ew_Ib, Register.SI, 0xA5)]
		[InlineData("66 C1 F7 5A", 4, Code.Shl_Ew_Ib, Register.DI, 0x5A)]
		[InlineData("66 41 C1 F8 A5", 5, Code.Sar_Ew_Ib, Register.R8W, 0xA5)]

		[InlineData("66 41 C1 C1 5A", 5, Code.Rol_Ew_Ib, Register.R9W, 0x5A)]
		[InlineData("66 41 C1 CA A5", 5, Code.Ror_Ew_Ib, Register.R10W, 0xA5)]
		[InlineData("66 41 C1 D3 5A", 5, Code.Rcl_Ew_Ib, Register.R11W, 0x5A)]
		[InlineData("66 41 C1 DC A5", 5, Code.Rcr_Ew_Ib, Register.R12W, 0xA5)]
		[InlineData("66 41 C1 E5 5A", 5, Code.Shl_Ew_Ib, Register.R13W, 0x5A)]
		[InlineData("66 41 C1 EE A5", 5, Code.Shr_Ew_Ib, Register.R14W, 0xA5)]
		[InlineData("66 41 C1 F7 5A", 5, Code.Shl_Ew_Ib, Register.R15W, 0x5A)]
		[InlineData("66 C1 F8 A5", 4, Code.Sar_Ew_Ib, Register.AX, 0xA5)]
		void Test64_Grp2_Ew_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 C1 00 5A", 4, Code.Rol_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("66 C1 08 A5", 4, Code.Ror_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("66 C1 10 5A", 4, Code.Rcl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("66 C1 18 A5", 4, Code.Rcr_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("66 C1 20 5A", 4, Code.Shl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("66 C1 28 A5", 4, Code.Shr_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("66 C1 30 5A", 4, Code.Shl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("66 C1 38 A5", 4, Code.Sar_Ed_Ib, 0xA5, MemorySize.Int32)]
		void Test16_Grp2_Ed_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 C1 C1 5A", 4, Code.Rol_Ed_Ib, Register.ECX, 0x5A)]
		[InlineData("66 C1 CA A5", 4, Code.Ror_Ed_Ib, Register.EDX, 0xA5)]
		[InlineData("66 C1 D3 5A", 4, Code.Rcl_Ed_Ib, Register.EBX, 0x5A)]
		[InlineData("66 C1 DC A5", 4, Code.Rcr_Ed_Ib, Register.ESP, 0xA5)]
		[InlineData("66 C1 E5 5A", 4, Code.Shl_Ed_Ib, Register.EBP, 0x5A)]
		[InlineData("66 C1 EE A5", 4, Code.Shr_Ed_Ib, Register.ESI, 0xA5)]
		[InlineData("66 C1 F7 5A", 4, Code.Shl_Ed_Ib, Register.EDI, 0x5A)]
		[InlineData("66 C1 F8 A5", 4, Code.Sar_Ed_Ib, Register.EAX, 0xA5)]
		void Test16_Grp2_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C1 00 5A", 3, Code.Rol_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 08 A5", 3, Code.Ror_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("C1 10 5A", 3, Code.Rcl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 18 A5", 3, Code.Rcr_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("C1 20 5A", 3, Code.Shl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 28 A5", 3, Code.Shr_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("C1 30 5A", 3, Code.Shl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 38 A5", 3, Code.Sar_Ed_Ib, 0xA5, MemorySize.Int32)]
		void Test32_Grp2_Ed_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C1 C1 5A", 3, Code.Rol_Ed_Ib, Register.ECX, 0x5A)]
		[InlineData("C1 CA A5", 3, Code.Ror_Ed_Ib, Register.EDX, 0xA5)]
		[InlineData("C1 D3 5A", 3, Code.Rcl_Ed_Ib, Register.EBX, 0x5A)]
		[InlineData("C1 DC A5", 3, Code.Rcr_Ed_Ib, Register.ESP, 0xA5)]
		[InlineData("C1 E5 5A", 3, Code.Shl_Ed_Ib, Register.EBP, 0x5A)]
		[InlineData("C1 EE A5", 3, Code.Shr_Ed_Ib, Register.ESI, 0xA5)]
		[InlineData("C1 F7 5A", 3, Code.Shl_Ed_Ib, Register.EDI, 0x5A)]
		[InlineData("C1 F8 A5", 3, Code.Sar_Ed_Ib, Register.EAX, 0xA5)]
		void Test32_Grp2_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C1 00 5A", 3, Code.Rol_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 08 A5", 3, Code.Ror_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("C1 10 5A", 3, Code.Rcl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 18 A5", 3, Code.Rcr_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("C1 20 5A", 3, Code.Shl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 28 A5", 3, Code.Shr_Ed_Ib, 0xA5, MemorySize.UInt32)]
		[InlineData("C1 30 5A", 3, Code.Shl_Ed_Ib, 0x5A, MemorySize.UInt32)]
		[InlineData("C1 38 A5", 3, Code.Sar_Ed_Ib, 0xA5, MemorySize.Int32)]
		void Test64_Grp2_Ed_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C1 C1 5A", 3, Code.Rol_Ed_Ib, Register.ECX, 0x5A)]
		[InlineData("C1 CA A5", 3, Code.Ror_Ed_Ib, Register.EDX, 0xA5)]
		[InlineData("C1 D3 5A", 3, Code.Rcl_Ed_Ib, Register.EBX, 0x5A)]
		[InlineData("C1 DC A5", 3, Code.Rcr_Ed_Ib, Register.ESP, 0xA5)]
		[InlineData("C1 E5 5A", 3, Code.Shl_Ed_Ib, Register.EBP, 0x5A)]
		[InlineData("C1 EE A5", 3, Code.Shr_Ed_Ib, Register.ESI, 0xA5)]
		[InlineData("C1 F7 5A", 3, Code.Shl_Ed_Ib, Register.EDI, 0x5A)]
		[InlineData("41 C1 F8 A5", 4, Code.Sar_Ed_Ib, Register.R8D, 0xA5)]

		[InlineData("41 C1 C1 5A", 4, Code.Rol_Ed_Ib, Register.R9D, 0x5A)]
		[InlineData("41 C1 CA A5", 4, Code.Ror_Ed_Ib, Register.R10D, 0xA5)]
		[InlineData("41 C1 D3 5A", 4, Code.Rcl_Ed_Ib, Register.R11D, 0x5A)]
		[InlineData("41 C1 DC A5", 4, Code.Rcr_Ed_Ib, Register.R12D, 0xA5)]
		[InlineData("41 C1 E5 5A", 4, Code.Shl_Ed_Ib, Register.R13D, 0x5A)]
		[InlineData("41 C1 EE A5", 4, Code.Shr_Ed_Ib, Register.R14D, 0xA5)]
		[InlineData("41 C1 F7 5A", 4, Code.Shl_Ed_Ib, Register.R15D, 0x5A)]
		[InlineData("C1 F8 A5", 3, Code.Sar_Ed_Ib, Register.EAX, 0xA5)]
		void Test64_Grp2_Ed_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("48 C1 00 5A", 4, Code.Rol_Eq_Ib, 0x5A, MemorySize.UInt64)]
		[InlineData("48 C1 08 A5", 4, Code.Ror_Eq_Ib, 0xA5, MemorySize.UInt64)]
		[InlineData("48 C1 10 5A", 4, Code.Rcl_Eq_Ib, 0x5A, MemorySize.UInt64)]
		[InlineData("48 C1 18 A5", 4, Code.Rcr_Eq_Ib, 0xA5, MemorySize.UInt64)]
		[InlineData("48 C1 20 5A", 4, Code.Shl_Eq_Ib, 0x5A, MemorySize.UInt64)]
		[InlineData("48 C1 28 A5", 4, Code.Shr_Eq_Ib, 0xA5, MemorySize.UInt64)]
		[InlineData("48 C1 30 5A", 4, Code.Shl_Eq_Ib, 0x5A, MemorySize.UInt64)]
		[InlineData("48 C1 38 A5", 4, Code.Sar_Eq_Ib, 0xA5, MemorySize.Int64)]
		void Test64_Grp2_Eq_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("48 C1 C1 5A", 4, Code.Rol_Eq_Ib, Register.RCX, 0x5A)]
		[InlineData("48 C1 CA A5", 4, Code.Ror_Eq_Ib, Register.RDX, 0xA5)]
		[InlineData("48 C1 D3 5A", 4, Code.Rcl_Eq_Ib, Register.RBX, 0x5A)]
		[InlineData("48 C1 DC A5", 4, Code.Rcr_Eq_Ib, Register.RSP, 0xA5)]
		[InlineData("48 C1 E5 5A", 4, Code.Shl_Eq_Ib, Register.RBP, 0x5A)]
		[InlineData("48 C1 EE A5", 4, Code.Shr_Eq_Ib, Register.RSI, 0xA5)]
		[InlineData("48 C1 F7 5A", 4, Code.Shl_Eq_Ib, Register.RDI, 0x5A)]
		[InlineData("49 C1 F8 A5", 4, Code.Sar_Eq_Ib, Register.R8, 0xA5)]

		[InlineData("49 C1 C1 5A", 4, Code.Rol_Eq_Ib, Register.R9, 0x5A)]
		[InlineData("49 C1 CA A5", 4, Code.Ror_Eq_Ib, Register.R10, 0xA5)]
		[InlineData("49 C1 D3 5A", 4, Code.Rcl_Eq_Ib, Register.R11, 0x5A)]
		[InlineData("49 C1 DC A5", 4, Code.Rcr_Eq_Ib, Register.R12, 0xA5)]
		[InlineData("49 C1 E5 5A", 4, Code.Shl_Eq_Ib, Register.R13, 0x5A)]
		[InlineData("49 C1 EE A5", 4, Code.Shr_Eq_Ib, Register.R14, 0xA5)]
		[InlineData("49 C1 F7 5A", 4, Code.Shl_Eq_Ib, Register.R15, 0x5A)]
		[InlineData("48 C1 F8 A5", 4, Code.Sar_Eq_Ib, Register.RAX, 0xA5)]
		void Test64_Grp2_Eq_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C2 5AA5", 3, 0xA55A)]
		[InlineData("C2 A55A", 3, 0x5AA5)]
		void Test16_Retnw_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnw_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 C2 5AA5", 4, 0xA55A)]
		[InlineData("66 C2 A55A", 4, 0x5AA5)]
		void Test32_Retnw_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnw_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 C2 5AA5", 4, 0xA55A)]
		[InlineData("66 C2 A55A", 4, 0x5AA5)]
		void Test16_Retnd_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnd_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("C2 5AA5", 3, 0xA55A)]
		[InlineData("C2 A55A", 3, 0x5AA5)]
		void Test32_Retnd_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnd_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Theory]
		[InlineData("C2 5AA5", 3, 0xA55A)]
		[InlineData("C2 A55A", 3, 0x5AA5)]
		[InlineData("66 C2 5AA5", 4, 0xA55A)]
		[InlineData("48 C2 A55A", 4, 0x5AA5)]
		[InlineData("66 48 C2 5AA5", 5, 0xA55A)]
		void Test64_Retnq_Iw_1(string hexBytes, int byteLength, ushort immediate) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnq_Iw, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Immediate16, instr.Op0Kind);
			Assert.Equal(immediate, instr.Immediate16);
		}

		[Fact]
		void Test16_Retnw_1() {
			var decoder = CreateDecoder16("C3");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Retnw_1() {
			var decoder = CreateDecoder32("66 C3");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnw, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Retnd_1() {
			var decoder = CreateDecoder16("66 C3");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test32_Retnd_1() {
			var decoder = CreateDecoder32("C3");
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnd, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(1, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Theory]
		[InlineData("C3", 1)]
		[InlineData("66 C3", 2)]
		[InlineData("48 C3", 2)]
		[InlineData("66 48 C3", 3)]
		void Test64_Retnq_1(string hexBytes, int byteLength) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Retnq, instr.Code);
			Assert.Equal(0, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}

		[Fact]
		void Test16_Les_Gw_Mp_1() {
			var decoder = CreateDecoder16("C4 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Les_Gw_Mp, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Les_Gw_Mp_1() {
			var decoder = CreateDecoder32("66 C4 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Les_Gw_Mp, instr.Code);
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
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Les_Gd_Mp_1() {
			var decoder = CreateDecoder16("66 C4 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Les_Gd_Mp, instr.Code);
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
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Les_Gd_Mp_1() {
			var decoder = CreateDecoder32("C4 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Les_Gd_Mp, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Lds_Gw_Mp_1() {
			var decoder = CreateDecoder16("C5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lds_Gw_Mp, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Lds_Gw_Mp_1() {
			var decoder = CreateDecoder32("66 C5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lds_Gw_Mp, instr.Code);
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
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Lds_Gd_Mp_1() {
			var decoder = CreateDecoder16("66 C5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lds_Gd_Mp, instr.Code);
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
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Lds_Gd_Mp_1() {
			var decoder = CreateDecoder32("C5 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Lds_Gd_Mp, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.SegPtr32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("C6 00 5A", 3, Code.Mov_Eb_Ib, 0x5A)]
		[InlineData("C6 00 A5", 3, Code.Mov_Eb_Ib, 0xA5)]
		void Test16_Grp11_Mov_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C6 C1 5A", 3, Code.Mov_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("C6 C2 A5", 3, Code.Mov_Eb_Ib, Register.DL, 0xA5)]
		void Test16_Grp11_Mov_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C6 00 5A", 3, Code.Mov_Eb_Ib, 0x5A)]
		[InlineData("C6 00 A5", 3, Code.Mov_Eb_Ib, 0xA5)]
		void Test32_Grp11_Mov_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C6 C1 5A", 3, Code.Mov_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("C6 C2 A5", 3, Code.Mov_Eb_Ib, Register.DL, 0xA5)]
		void Test32_Grp11_Mov_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C6 00 5A", 3, Code.Mov_Eb_Ib, 0x5A)]
		[InlineData("C6 00 A5", 3, Code.Mov_Eb_Ib, 0xA5)]

		[InlineData("44 C6 00 5A", 4, Code.Mov_Eb_Ib, 0x5A)]
		[InlineData("44 C6 00 A5", 4, Code.Mov_Eb_Ib, 0xA5)]
		void Test64_Grp11_Mov_Eb_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt8, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op1Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C6 C1 5A", 3, Code.Mov_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("C6 C6 A5", 3, Code.Mov_Eb_Ib, Register.DH, 0xA5)]

		[InlineData("40 C6 C1 5A", 4, Code.Mov_Eb_Ib, Register.CL, 0x5A)]
		[InlineData("40 C6 C6 A5", 4, Code.Mov_Eb_Ib, Register.SIL, 0xA5)]
		[InlineData("41 C6 C1 5A", 4, Code.Mov_Eb_Ib, Register.R9L, 0x5A)]
		[InlineData("41 C6 C6 A5", 4, Code.Mov_Eb_Ib, Register.R14L, 0xA5)]
		void Test64_Grp11_Mov_Eb_Ib_2(string hexBytes, int byteLength, Code code, Register reg, byte immediate8) {
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
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Theory]
		[InlineData("C6 F8 5A", 3, Code.Xabort_Ib, 0x5A)]
		[InlineData("C6 F8 A5", 3, Code.Xabort_Ib, 0xA5)]
		void Test16_Grp11_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		[InlineData("C6 F8 5A", 3, Code.Xabort_Ib, 0x5A)]
		[InlineData("C6 F8 A5", 3, Code.Xabort_Ib, 0xA5)]
		void Test32_Grp11_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		[InlineData("C6 F8 5A", 3, Code.Xabort_Ib, 0x5A)]
		[InlineData("C6 F8 A5", 3, Code.Xabort_Ib, 0xA5)]
		void Test64_Grp11_Ib_1(string hexBytes, int byteLength, Code code, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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
		[InlineData("C7 00 5AA5", 4, Code.Mov_Ew_Iw, 0xA55A)]
		[InlineData("C7 00 A55A", 4, Code.Mov_Ew_Iw, 0x5AA5)]
		void Test16_Grp11_Mov_Ew_Iw_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("C7 C1 5AA5", 4, Code.Mov_Ew_Iw, Register.CX, 0xA55A)]
		[InlineData("C7 C2 A55A", 4, Code.Mov_Ew_Iw, Register.DX, 0x5AA5)]
		void Test16_Grp11_Mov_Ew_Iw_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 C7 00 5AA5", 5, Code.Mov_Ew_Iw, 0xA55A)]
		[InlineData("66 C7 00 A55A", 5, Code.Mov_Ew_Iw, 0x5AA5)]
		void Test32_Grp11_Mov_Ew_Iw_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 C7 C1 5AA5", 5, Code.Mov_Ew_Iw, Register.CX, 0xA55A)]
		[InlineData("66 C7 C2 A55A", 5, Code.Mov_Ew_Iw, Register.DX, 0x5AA5)]
		void Test32_Grp11_Mov_Ew_Iw_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 C7 00 5AA5", 5, Code.Mov_Ew_Iw, 0xA55A)]
		[InlineData("66 C7 00 A55A", 5, Code.Mov_Ew_Iw, 0x5AA5)]
		void Test64_Grp11_Mov_Ew_Iw_1(string hexBytes, int byteLength, Code code, ushort immediate16) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 C7 C1 5AA5", 5, Code.Mov_Ew_Iw, Register.CX, 0xA55A)]
		[InlineData("66 C7 C6 A55A", 5, Code.Mov_Ew_Iw, Register.SI, 0x5AA5)]
		[InlineData("66 41 C7 C1 5AA5", 6, Code.Mov_Ew_Iw, Register.R9W, 0xA55A)]
		[InlineData("66 41 C7 C6 A55A", 6, Code.Mov_Ew_Iw, Register.R14W, 0x5AA5)]
		void Test64_Grp11_Mov_Ew_Iw_2(string hexBytes, int byteLength, Code code, Register reg, ushort immediate16) {
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

			Assert.Equal(OpKind.Immediate16, instr.Op1Kind);
			Assert.Equal(immediate16, instr.Immediate16);
		}

		[Theory]
		[InlineData("66 C7 00 5AA51234", 7, Code.Mov_Ed_Id, 0x3412A55A)]
		[InlineData("66 C7 00 A55A89AB", 7, Code.Mov_Ed_Id, 0xAB895AA5)]
		void Test16_Grp11_Mov_Ed_Id_1(string hexBytes, int byteLength, Code code, uint immediate32) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("66 C7 C1 5AA51234", 7, Code.Mov_Ed_Id, Register.ECX, 0x3412A55A)]
		[InlineData("66 C7 C2 A55A89AB", 7, Code.Mov_Ed_Id, Register.EDX, 0xAB895AA5)]
		void Test16_Grp11_Mov_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("C7 00 5AA51234", 6, Code.Mov_Ed_Id, 0x3412A55A)]
		[InlineData("C7 00 A55A89AB", 6, Code.Mov_Ed_Id, 0xAB895AA5)]
		void Test32_Grp11_Mov_Ed_Id_1(string hexBytes, int byteLength, Code code, uint immediate32) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("C7 C1 5AA51234", 6, Code.Mov_Ed_Id, Register.ECX, 0x3412A55A)]
		[InlineData("C7 C2 A55A89AB", 6, Code.Mov_Ed_Id, Register.EDX, 0xAB895AA5)]
		void Test32_Grp11_Mov_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("C7 00 5AA51234", 6, Code.Mov_Ed_Id, 0x3412A55A)]
		[InlineData("C7 00 A55A89AB", 6, Code.Mov_Ed_Id, 0xAB895AA5)]
		void Test64_Grp11_Mov_Ed_Id_1(string hexBytes, int byteLength, Code code, uint immediate32) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("C7 C1 5AA51234", 6, Code.Mov_Ed_Id, Register.ECX, 0x3412A55A)]
		[InlineData("C7 C6 A55A89AB", 6, Code.Mov_Ed_Id, Register.ESI, 0xAB895AA5)]
		[InlineData("41 C7 C1 5AA51234", 7, Code.Mov_Ed_Id, Register.R9D, 0x3412A55A)]
		[InlineData("41 C7 C6 A55A89AB", 7, Code.Mov_Ed_Id, Register.R14D, 0xAB895AA5)]
		void Test64_Grp11_Mov_Ed_Id_2(string hexBytes, int byteLength, Code code, Register reg, uint immediate32) {
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

			Assert.Equal(OpKind.Immediate32, instr.Op1Kind);
			Assert.Equal(immediate32, instr.Immediate32);
		}

		[Theory]
		[InlineData("48 C7 00 5AA51234", 7, Code.Mov_Eq_Id64, 0x3412A55AUL)]
		[InlineData("48 C7 00 A55A89AB", 7, Code.Mov_Eq_Id64, 0xFFFFFFFFAB895AA5UL)]
		void Test64_Grp11_Mov_EqId_1(string hexBytes, int byteLength, Code code, ulong immediate64) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate32to64, instr.Op1Kind);
			Assert.Equal((long)immediate64, instr.Immediate32to64);
		}

		[Theory]
		[InlineData("48 C7 C1 5AA51234", 7, Code.Mov_Eq_Id64, Register.RCX, 0x3412A55AUL)]
		[InlineData("48 C7 C6 A55A89AB", 7, Code.Mov_Eq_Id64, Register.RSI, 0xFFFFFFFFAB895AA5UL)]
		[InlineData("49 C7 C1 5AA51234", 7, Code.Mov_Eq_Id64, Register.R9, 0x3412A55AUL)]
		[InlineData("49 C7 C6 A55A89AB", 7, Code.Mov_Eq_Id64, Register.R14, 0xFFFFFFFFAB895AA5UL)]
		void Test64_Grp11_Mov_EqId_2(string hexBytes, int byteLength, Code code, Register reg, ulong immediate64) {
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

			Assert.Equal(OpKind.Immediate32to64, instr.Op1Kind);
			Assert.Equal((long)immediate64, instr.Immediate32to64);
		}

		[Theory]
		[InlineData("C7 F8 5AA5", 4, Code.Xbegin_Jw16, DecoderConstants.DEFAULT_IP16 + 4 + 0xA55A)]
		[InlineData("C7 F8 A55A", 4, Code.Xbegin_Jw16, DecoderConstants.DEFAULT_IP16 + 4 + 0x5AA5)]
		void Test16_Xbegin_Jw16_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch16, instr.Op0Kind);
			Assert.Equal((ushort)target, instr.NearBranch16Target);
		}

		[Theory]
		[InlineData("66 C7 F8 5AA5", 5, Code.Xbegin_Jw16, DecoderConstants.DEFAULT_IP32 + 5 + 0xFFFFA55A)]
		[InlineData("66 C7 F8 A55A", 5, Code.Xbegin_Jw16, DecoderConstants.DEFAULT_IP32 + 5 + 0x5AA5)]
		void Test32_Xbegin_Jw16_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch32, instr.Op0Kind);
			Assert.Equal((uint)target, instr.NearBranch32Target);
		}

		[Theory]
		[InlineData("66 C7 F8 5AA5", 5, Code.Xbegin_Jw16, DecoderConstants.DEFAULT_IP64 + 5 - 0x5AA6)]
		[InlineData("66 C7 F8 A55A", 5, Code.Xbegin_Jw16, DecoderConstants.DEFAULT_IP64 + 5 + 0x5AA5)]
		void Test64_Xbegin_Jw16_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch64, instr.Op0Kind);
			Assert.Equal(target, instr.NearBranch64Target);
		}

		[Theory]
		[InlineData("66 C7 F8 5AA51234", 7, Code.Xbegin_Jd32, DecoderConstants.DEFAULT_IP16 + 7 + 0x3412A55A)]
		[InlineData("66 C7 F8 A56789AB", 7, Code.Xbegin_Jd32, DecoderConstants.DEFAULT_IP16 + 7 + 0xAB8967A5)]
		void Test16_Xbegin_Jd32_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch16, instr.Op0Kind);
			Assert.Equal((ushort)target, instr.NearBranch16Target);
		}

		[Theory]
		[InlineData("C7 F8 5AA51234", 6, Code.Xbegin_Jd32, DecoderConstants.DEFAULT_IP32 + 6 + 0x3412A55A)]
		[InlineData("C7 F8 A56789AB", 6, Code.Xbegin_Jd32, DecoderConstants.DEFAULT_IP32 + 6 + 0xAB8967A5)]
		void Test32_Xbegin_Jd32_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch32, instr.Op0Kind);
			Assert.Equal((uint)target, instr.NearBranch32Target);
		}

		[Theory]
		[InlineData("C7 F8 5AA51234", 6, Code.Xbegin_Jd32, DecoderConstants.DEFAULT_IP64 + 6 + 0x3412A55A)]
		[InlineData("C7 F8 A56789AB", 6, Code.Xbegin_Jd32, DecoderConstants.DEFAULT_IP64 + 6 - 0x5476985B)]
		void Test64_Xbegin_Jd32_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch64, instr.Op0Kind);
			Assert.Equal(target, instr.NearBranch64Target);
		}

		[Theory]
		[InlineData("48 C7 F8 5AA51234", 7, Code.Xbegin_Jd64, DecoderConstants.DEFAULT_IP64 + 7 + 0x3412A55A)]
		[InlineData("48 C7 F8 A56789AB", 7, Code.Xbegin_Jd64, DecoderConstants.DEFAULT_IP64 + 7 - 0x5476985B)]

		[InlineData("66 4F C7 F8 5AA51234", 8, Code.Xbegin_Jd64, DecoderConstants.DEFAULT_IP64 + 8 + 0x3412A55A)]
		[InlineData("66 4F C7 F8 A56789AB", 8, Code.Xbegin_Jd64, DecoderConstants.DEFAULT_IP64 + 8 - 0x5476985B)]
		void Test64_Xbegin_Jd64_1(string hexBytes, int byteLength, Code code, ulong target) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.NearBranch64, instr.Op0Kind);
			Assert.Equal(target, instr.NearBranch64Target);
		}
	}
}
