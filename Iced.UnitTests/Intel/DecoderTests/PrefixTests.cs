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
	public sealed class PrefixTests : DecoderTest {
		[Theory]
		[InlineData("66 66 01 CE", 4, Code.Add_Ed_Gd)]
		void Test16_double_66_is_same_as_one(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 66 01 CE", 4, Code.Add_Ew_Gw)]
		void Test32_double_66_is_same_as_one(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 66 01 CE", 4, Code.Add_Ew_Gw)]
		void Test64_double_66_is_same_as_one(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 67 8D 18", 4, Code.Lea_Gw_M)]
		void Test16_double_67_is_same_as_one(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 67 8D 18", 4, Code.Lea_Gd_M)]
		void Test32_double_67_is_same_as_one(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 67 8D 18", 4, Code.Lea_Gd_M)]
		void Test64_double_67_is_same_as_one(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("26 26 01 18", 4, Code.Add_Ew_Gw, Register.ES)]
		[InlineData("26 2E 01 18", 4, Code.Add_Ew_Gw, Register.CS)]
		[InlineData("26 36 01 18", 4, Code.Add_Ew_Gw, Register.SS)]
		[InlineData("26 3E 01 18", 4, Code.Add_Ew_Gw, Register.DS)]
		[InlineData("26 64 01 18", 4, Code.Add_Ew_Gw, Register.FS)]
		[InlineData("26 65 01 18", 4, Code.Add_Ew_Gw, Register.GS)]

		[InlineData("2E 26 01 18", 4, Code.Add_Ew_Gw, Register.ES)]
		[InlineData("2E 2E 01 18", 4, Code.Add_Ew_Gw, Register.CS)]
		[InlineData("2E 36 01 18", 4, Code.Add_Ew_Gw, Register.SS)]
		[InlineData("2E 3E 01 18", 4, Code.Add_Ew_Gw, Register.DS)]
		[InlineData("2E 64 01 18", 4, Code.Add_Ew_Gw, Register.FS)]
		[InlineData("2E 65 01 18", 4, Code.Add_Ew_Gw, Register.GS)]

		[InlineData("36 26 01 18", 4, Code.Add_Ew_Gw, Register.ES)]
		[InlineData("36 2E 01 18", 4, Code.Add_Ew_Gw, Register.CS)]
		[InlineData("36 36 01 18", 4, Code.Add_Ew_Gw, Register.SS)]
		[InlineData("36 3E 01 18", 4, Code.Add_Ew_Gw, Register.DS)]
		[InlineData("36 64 01 18", 4, Code.Add_Ew_Gw, Register.FS)]
		[InlineData("36 65 01 18", 4, Code.Add_Ew_Gw, Register.GS)]

		[InlineData("3E 26 01 18", 4, Code.Add_Ew_Gw, Register.ES)]
		[InlineData("3E 2E 01 18", 4, Code.Add_Ew_Gw, Register.CS)]
		[InlineData("3E 36 01 18", 4, Code.Add_Ew_Gw, Register.SS)]
		[InlineData("3E 3E 01 18", 4, Code.Add_Ew_Gw, Register.DS)]
		[InlineData("3E 64 01 18", 4, Code.Add_Ew_Gw, Register.FS)]
		[InlineData("3E 65 01 18", 4, Code.Add_Ew_Gw, Register.GS)]

		[InlineData("64 26 01 18", 4, Code.Add_Ew_Gw, Register.ES)]
		[InlineData("64 2E 01 18", 4, Code.Add_Ew_Gw, Register.CS)]
		[InlineData("64 36 01 18", 4, Code.Add_Ew_Gw, Register.SS)]
		[InlineData("64 3E 01 18", 4, Code.Add_Ew_Gw, Register.DS)]
		[InlineData("64 64 01 18", 4, Code.Add_Ew_Gw, Register.FS)]
		[InlineData("64 65 01 18", 4, Code.Add_Ew_Gw, Register.GS)]

		[InlineData("65 26 01 18", 4, Code.Add_Ew_Gw, Register.ES)]
		[InlineData("65 2E 01 18", 4, Code.Add_Ew_Gw, Register.CS)]
		[InlineData("65 36 01 18", 4, Code.Add_Ew_Gw, Register.SS)]
		[InlineData("65 3E 01 18", 4, Code.Add_Ew_Gw, Register.DS)]
		[InlineData("65 64 01 18", 4, Code.Add_Ew_Gw, Register.FS)]
		[InlineData("65 65 01 18", 4, Code.Add_Ew_Gw, Register.GS)]
		void Test16_extra_segment_overrides(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segReg, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(segReg, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Theory]
		[InlineData("26 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("26 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("26 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("26 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("26 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("26 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("2E 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("2E 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("2E 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("2E 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("2E 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("2E 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("36 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("36 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("36 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("36 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("36 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("36 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("3E 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("3E 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("3E 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("3E 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("3E 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("3E 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("64 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("64 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("64 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("64 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("64 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("64 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("65 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("65 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("65 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("65 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("65 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("65 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]
		void Test32_extra_segment_overrides(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segReg, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(segReg, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("26 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("26 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("26 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("26 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("26 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("26 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("2E 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("2E 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("2E 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("2E 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("2E 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("2E 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("36 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("36 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("36 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("36 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("36 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("36 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("3E 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("3E 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("3E 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("3E 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("3E 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("3E 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("64 26 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("64 2E 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("64 36 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("64 3E 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("64 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("64 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]

		[InlineData("65 26 01 18", 4, Code.Add_Ed_Gd, Register.GS)]
		[InlineData("65 2E 01 18", 4, Code.Add_Ed_Gd, Register.GS)]
		[InlineData("65 36 01 18", 4, Code.Add_Ed_Gd, Register.GS)]
		[InlineData("65 3E 01 18", 4, Code.Add_Ed_Gd, Register.GS)]
		[InlineData("65 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("65 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]
		void Test64_extra_segment_overrides(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segReg, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(segReg, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 48 01 CE", 4, Code.Add_Eq_Gq)]
		void Test64_REX_W_overrides_66(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.RSI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RCX, instr.Op1Register);
		}

		[Theory]
		[InlineData("4F 48 01 CE", 4, Code.Add_Eq_Gq, Register.RSI, Register.RCX)]
		[InlineData("4F 4C 01 C5", 4, Code.Add_Eq_Gq, Register.RBP, Register.R8)]
		[InlineData("4F 49 01 D6", 4, Code.Add_Eq_Gq, Register.R14, Register.RDX)]
		[InlineData("4F 4D 01 D0", 4, Code.Add_Eq_Gq, Register.R8, Register.R10)]
		[InlineData("4F 49 01 D9", 4, Code.Add_Eq_Gq, Register.R9, Register.RBX)]
		[InlineData("4F 4C 01 EC", 4, Code.Add_Eq_Gq, Register.RSP, Register.R13)]
		void Test64_double_REX_prefixes(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

		[Theory]
		[InlineData("48 66 01 CE", 4, Code.Add_Ew_Gw)]
		[InlineData("4F 66 01 CE", 4, Code.Add_Ew_Gw)]
		void Test64_REX_prefix_before_66(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 67 01 18", 4, Code.Add_Ed_Gd)]
		void Test64_REX_before_67(string hexBytes, int byteLength, Code code) {
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
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("49 F0 01 18", 4, Code.Add_Ed_Gd)]
		void Test64_REX_before_F0(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.True(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("4D F2 01 18", 4, Code.Add_Ed_Gd)]
		void Test64_REX_before_F2(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.True(instr.HasPrefixRepne);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("4F F3 01 18", 4, Code.Add_Ed_Gd)]
		void Test64_REX_before_F3(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.True(instr.HasPrefixRepe);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 26 01 18", 4, Code.Add_Ed_Gd, Register.ES)]
		[InlineData("49 2E 01 18", 4, Code.Add_Ed_Gd, Register.CS)]
		[InlineData("4A 36 01 18", 4, Code.Add_Ed_Gd, Register.SS)]
		[InlineData("4B 3E 01 18", 4, Code.Add_Ed_Gd, Register.DS)]
		[InlineData("4C 64 01 18", 4, Code.Add_Ed_Gd, Register.FS)]
		[InlineData("4F 65 01 18", 4, Code.Add_Ed_Gd, Register.GS)]
		void Test64_REX_before_segment_override(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(segReg, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(segReg, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[MemberData(nameof(Test16_LockPrefix_Data))]
		void Test16_LockPrefix(string hexBytes, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.True(instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test16_LockPrefix_Data {
			get {
				yield return new object[] { "F0 10 08", Code.Adc_Eb_Gb };
				yield return new object[] { "F0 80 10 A5", Code.Adc_Eb_Ib };
				yield return new object[] { "F0 11 08", Code.Adc_Ew_Gw };
				yield return new object[] { "F0 83 10 A5", Code.Adc_Ew_Ib16 };
				yield return new object[] { "F0 81 10 5AA5", Code.Adc_Ew_Iw };
				yield return new object[] { "F0 66 11 08", Code.Adc_Ed_Gd };
				yield return new object[] { "F0 66 83 10 A5", Code.Adc_Ed_Ib32 };
				yield return new object[] { "F0 66 81 10 34 12 5AA5", Code.Adc_Ed_Id };

				yield return new object[] { "F0 00 08", Code.Add_Eb_Gb };
				yield return new object[] { "F0 80 00 A5", Code.Add_Eb_Ib };
				yield return new object[] { "F0 01 08", Code.Add_Ew_Gw };
				yield return new object[] { "F0 83 00 A5", Code.Add_Ew_Ib16 };
				yield return new object[] { "F0 81 00 5AA5", Code.Add_Ew_Iw };
				yield return new object[] { "F0 66 01 08", Code.Add_Ed_Gd };
				yield return new object[] { "F0 66 83 00 A5", Code.Add_Ed_Ib32 };
				yield return new object[] { "F0 66 81 00 34 12 5AA5", Code.Add_Ed_Id };

				yield return new object[] { "F0 20 08", Code.And_Eb_Gb };
				yield return new object[] { "F0 80 20 A5", Code.And_Eb_Ib };
				yield return new object[] { "F0 21 08", Code.And_Ew_Gw };
				yield return new object[] { "F0 83 20 A5", Code.And_Ew_Ib16 };
				yield return new object[] { "F0 81 20 5AA5", Code.And_Ew_Iw };
				yield return new object[] { "F0 66 21 08", Code.And_Ed_Gd };
				yield return new object[] { "F0 66 83 20 A5", Code.And_Ed_Ib32 };
				yield return new object[] { "F0 66 81 20 34 12 5AA5", Code.And_Ed_Id };

				yield return new object[] { "F0 08 08", Code.Or_Eb_Gb };
				yield return new object[] { "F0 80 08 A5", Code.Or_Eb_Ib };
				yield return new object[] { "F0 09 08", Code.Or_Ew_Gw };
				yield return new object[] { "F0 83 08 A5", Code.Or_Ew_Ib16 };
				yield return new object[] { "F0 81 08 5AA5", Code.Or_Ew_Iw };
				yield return new object[] { "F0 66 09 08", Code.Or_Ed_Gd };
				yield return new object[] { "F0 66 83 08 A5", Code.Or_Ed_Ib32 };
				yield return new object[] { "F0 66 81 08 34 12 5AA5", Code.Or_Ed_Id };

				yield return new object[] { "F0 18 08", Code.Sbb_Eb_Gb };
				yield return new object[] { "F0 80 18 A5", Code.Sbb_Eb_Ib };
				yield return new object[] { "F0 19 08", Code.Sbb_Ew_Gw };
				yield return new object[] { "F0 83 18 A5", Code.Sbb_Ew_Ib16 };
				yield return new object[] { "F0 81 18 5AA5", Code.Sbb_Ew_Iw };
				yield return new object[] { "F0 66 19 08", Code.Sbb_Ed_Gd };
				yield return new object[] { "F0 66 83 18 A5", Code.Sbb_Ed_Ib32 };
				yield return new object[] { "F0 66 81 18 34 12 5AA5", Code.Sbb_Ed_Id };

				yield return new object[] { "F0 28 08", Code.Sub_Eb_Gb };
				yield return new object[] { "F0 80 28 A5", Code.Sub_Eb_Ib };
				yield return new object[] { "F0 29 08", Code.Sub_Ew_Gw };
				yield return new object[] { "F0 83 28 A5", Code.Sub_Ew_Ib16 };
				yield return new object[] { "F0 81 28 5AA5", Code.Sub_Ew_Iw };
				yield return new object[] { "F0 66 29 08", Code.Sub_Ed_Gd };
				yield return new object[] { "F0 66 83 28 A5", Code.Sub_Ed_Ib32 };
				yield return new object[] { "F0 66 81 28 34 12 5AA5", Code.Sub_Ed_Id };

				yield return new object[] { "F0 30 08", Code.Xor_Eb_Gb };
				yield return new object[] { "F0 80 30 A5", Code.Xor_Eb_Ib };
				yield return new object[] { "F0 31 08", Code.Xor_Ew_Gw };
				yield return new object[] { "F0 83 30 A5", Code.Xor_Ew_Ib16 };
				yield return new object[] { "F0 81 30 5AA5", Code.Xor_Ew_Iw };
				yield return new object[] { "F0 66 31 08", Code.Xor_Ed_Gd };
				yield return new object[] { "F0 66 83 30 A5", Code.Xor_Ed_Ib32 };
				yield return new object[] { "F0 66 81 30 34 12 5AA5", Code.Xor_Ed_Id };

				yield return new object[] { "F0 0F BB 08", Code.Btc_Ew_Gw };
				yield return new object[] { "F0 0F BA 38 A5", Code.Btc_Ew_Ib };
				yield return new object[] { "F0 66 0F BB 08", Code.Btc_Ed_Gd };
				yield return new object[] { "F0 66 0F BA 38 A5", Code.Btc_Ed_Ib };

				yield return new object[] { "F0 0F B3 08", Code.Btr_Ew_Gw };
				yield return new object[] { "F0 0F BA 30 A5", Code.Btr_Ew_Ib };
				yield return new object[] { "F0 66 0F B3 08", Code.Btr_Ed_Gd };
				yield return new object[] { "F0 66 0F BA 30 A5", Code.Btr_Ed_Ib };

				yield return new object[] { "F0 0F AB 08", Code.Bts_Ew_Gw };
				yield return new object[] { "F0 0F BA 28 A5", Code.Bts_Ew_Ib };
				yield return new object[] { "F0 66 0F AB 08", Code.Bts_Ed_Gd };
				yield return new object[] { "F0 66 0F BA 28 A5", Code.Bts_Ed_Ib };

				yield return new object[] { "F0 FE 08", Code.Dec_Eb };
				yield return new object[] { "F0 FF 08", Code.Dec_Ew };
				yield return new object[] { "F0 66 FF 08", Code.Dec_Ed };

				yield return new object[] { "F0 FE 00", Code.Inc_Eb };
				yield return new object[] { "F0 FF 00", Code.Inc_Ew };
				yield return new object[] { "F0 66 FF 00", Code.Inc_Ed };

				yield return new object[] { "F0 F6 18", Code.Neg_Eb };
				yield return new object[] { "F0 F7 18", Code.Neg_Ew };
				yield return new object[] { "F0 66 F7 18", Code.Neg_Ed };

				yield return new object[] { "F0 F6 10", Code.Not_Eb };
				yield return new object[] { "F0 F7 10", Code.Not_Ew };
				yield return new object[] { "F0 66 F7 10", Code.Not_Ed };

				yield return new object[] { "F0 0F B0 08", Code.Cmpxchg_Eb_Gb };
				yield return new object[] { "F0 0F B1 08", Code.Cmpxchg_Ew_Gw };
				yield return new object[] { "F0 66 0F B1 08", Code.Cmpxchg_Ed_Gd };

				yield return new object[] { "F0 0F C0 08", Code.Xadd_Eb_Gb };
				yield return new object[] { "F0 0F C1 08", Code.Xadd_Ew_Gw };
				yield return new object[] { "F0 66 0F C1 08", Code.Xadd_Ed_Gd };

				yield return new object[] { "F0 86 08", Code.Xchg_Eb_Gb };
				yield return new object[] { "F0 87 08", Code.Xchg_Ew_Gw };
				yield return new object[] { "F0 66 87 08", Code.Xchg_Ed_Gd };

				yield return new object[] { "F0 0F C7 08", Code.Cmpxchg8b_Mq };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_LockPrefix_Data))]
		void Test32_LockPrefix(string hexBytes, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.True(instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test32_LockPrefix_Data {
			get {
				yield return new object[] { "F0 10 08", Code.Adc_Eb_Gb };
				yield return new object[] { "F0 80 10 A5", Code.Adc_Eb_Ib };
				yield return new object[] { "F0 66 11 08", Code.Adc_Ew_Gw };
				yield return new object[] { "F0 66 83 10 A5", Code.Adc_Ew_Ib16 };
				yield return new object[] { "F0 66 81 10 5AA5", Code.Adc_Ew_Iw };
				yield return new object[] { "F0 11 08", Code.Adc_Ed_Gd };
				yield return new object[] { "F0 83 10 A5", Code.Adc_Ed_Ib32 };
				yield return new object[] { "F0 81 10 34 12 5AA5", Code.Adc_Ed_Id };

				yield return new object[] { "F0 00 08", Code.Add_Eb_Gb };
				yield return new object[] { "F0 80 00 A5", Code.Add_Eb_Ib };
				yield return new object[] { "F0 66 01 08", Code.Add_Ew_Gw };
				yield return new object[] { "F0 66 83 00 A5", Code.Add_Ew_Ib16 };
				yield return new object[] { "F0 66 81 00 5AA5", Code.Add_Ew_Iw };
				yield return new object[] { "F0 01 08", Code.Add_Ed_Gd };
				yield return new object[] { "F0 83 00 A5", Code.Add_Ed_Ib32 };
				yield return new object[] { "F0 81 00 34 12 5AA5", Code.Add_Ed_Id };

				yield return new object[] { "F0 20 08", Code.And_Eb_Gb };
				yield return new object[] { "F0 80 20 A5", Code.And_Eb_Ib };
				yield return new object[] { "F0 66 21 08", Code.And_Ew_Gw };
				yield return new object[] { "F0 66 83 20 A5", Code.And_Ew_Ib16 };
				yield return new object[] { "F0 66 81 20 5AA5", Code.And_Ew_Iw };
				yield return new object[] { "F0 21 08", Code.And_Ed_Gd };
				yield return new object[] { "F0 83 20 A5", Code.And_Ed_Ib32 };
				yield return new object[] { "F0 81 20 34 12 5AA5", Code.And_Ed_Id };

				yield return new object[] { "F0 08 08", Code.Or_Eb_Gb };
				yield return new object[] { "F0 80 08 A5", Code.Or_Eb_Ib };
				yield return new object[] { "F0 66 09 08", Code.Or_Ew_Gw };
				yield return new object[] { "F0 66 83 08 A5", Code.Or_Ew_Ib16 };
				yield return new object[] { "F0 66 81 08 5AA5", Code.Or_Ew_Iw };
				yield return new object[] { "F0 09 08", Code.Or_Ed_Gd };
				yield return new object[] { "F0 83 08 A5", Code.Or_Ed_Ib32 };
				yield return new object[] { "F0 81 08 34 12 5AA5", Code.Or_Ed_Id };

				yield return new object[] { "F0 18 08", Code.Sbb_Eb_Gb };
				yield return new object[] { "F0 80 18 A5", Code.Sbb_Eb_Ib };
				yield return new object[] { "F0 66 19 08", Code.Sbb_Ew_Gw };
				yield return new object[] { "F0 66 83 18 A5", Code.Sbb_Ew_Ib16 };
				yield return new object[] { "F0 66 81 18 5AA5", Code.Sbb_Ew_Iw };
				yield return new object[] { "F0 19 08", Code.Sbb_Ed_Gd };
				yield return new object[] { "F0 83 18 A5", Code.Sbb_Ed_Ib32 };
				yield return new object[] { "F0 81 18 34 12 5AA5", Code.Sbb_Ed_Id };

				yield return new object[] { "F0 28 08", Code.Sub_Eb_Gb };
				yield return new object[] { "F0 80 28 A5", Code.Sub_Eb_Ib };
				yield return new object[] { "F0 66 29 08", Code.Sub_Ew_Gw };
				yield return new object[] { "F0 66 83 28 A5", Code.Sub_Ew_Ib16 };
				yield return new object[] { "F0 66 81 28 5AA5", Code.Sub_Ew_Iw };
				yield return new object[] { "F0 29 08", Code.Sub_Ed_Gd };
				yield return new object[] { "F0 83 28 A5", Code.Sub_Ed_Ib32 };
				yield return new object[] { "F0 81 28 34 12 5AA5", Code.Sub_Ed_Id };

				yield return new object[] { "F0 30 08", Code.Xor_Eb_Gb };
				yield return new object[] { "F0 80 30 A5", Code.Xor_Eb_Ib };
				yield return new object[] { "F0 66 31 08", Code.Xor_Ew_Gw };
				yield return new object[] { "F0 66 83 30 A5", Code.Xor_Ew_Ib16 };
				yield return new object[] { "F0 66 81 30 5AA5", Code.Xor_Ew_Iw };
				yield return new object[] { "F0 31 08", Code.Xor_Ed_Gd };
				yield return new object[] { "F0 83 30 A5", Code.Xor_Ed_Ib32 };
				yield return new object[] { "F0 81 30 34 12 5AA5", Code.Xor_Ed_Id };

				yield return new object[] { "F0 66 0F BB 08", Code.Btc_Ew_Gw };
				yield return new object[] { "F0 66 0F BA 38 A5", Code.Btc_Ew_Ib };
				yield return new object[] { "F0 0F BB 08", Code.Btc_Ed_Gd };
				yield return new object[] { "F0 0F BA 38 A5", Code.Btc_Ed_Ib };

				yield return new object[] { "F0 66 0F B3 08", Code.Btr_Ew_Gw };
				yield return new object[] { "F0 66 0F BA 30 A5", Code.Btr_Ew_Ib };
				yield return new object[] { "F0 0F B3 08", Code.Btr_Ed_Gd };
				yield return new object[] { "F0 0F BA 30 A5", Code.Btr_Ed_Ib };

				yield return new object[] { "F0 66 0F AB 08", Code.Bts_Ew_Gw };
				yield return new object[] { "F0 66 0F BA 28 A5", Code.Bts_Ew_Ib };
				yield return new object[] { "F0 0F AB 08", Code.Bts_Ed_Gd };
				yield return new object[] { "F0 0F BA 28 A5", Code.Bts_Ed_Ib };

				yield return new object[] { "F0 FE 08", Code.Dec_Eb };
				yield return new object[] { "F0 66 FF 08", Code.Dec_Ew };
				yield return new object[] { "F0 FF 08", Code.Dec_Ed };

				yield return new object[] { "F0 FE 00", Code.Inc_Eb };
				yield return new object[] { "F0 66 FF 00", Code.Inc_Ew };
				yield return new object[] { "F0 FF 00", Code.Inc_Ed };

				yield return new object[] { "F0 F6 18", Code.Neg_Eb };
				yield return new object[] { "F0 66 F7 18", Code.Neg_Ew };
				yield return new object[] { "F0 F7 18", Code.Neg_Ed };

				yield return new object[] { "F0 F6 10", Code.Not_Eb };
				yield return new object[] { "F0 66 F7 10", Code.Not_Ew };
				yield return new object[] { "F0 F7 10", Code.Not_Ed };

				yield return new object[] { "F0 0F B0 08", Code.Cmpxchg_Eb_Gb };
				yield return new object[] { "F0 66 0F B1 08", Code.Cmpxchg_Ew_Gw };
				yield return new object[] { "F0 0F B1 08", Code.Cmpxchg_Ed_Gd };

				yield return new object[] { "F0 0F C0 08", Code.Xadd_Eb_Gb };
				yield return new object[] { "F0 66 0F C1 08", Code.Xadd_Ew_Gw };
				yield return new object[] { "F0 0F C1 08", Code.Xadd_Ed_Gd };

				yield return new object[] { "F0 86 08", Code.Xchg_Eb_Gb };
				yield return new object[] { "F0 66 87 08", Code.Xchg_Ew_Gw };
				yield return new object[] { "F0 87 08", Code.Xchg_Ed_Gd };

				yield return new object[] { "F0 0F C7 08", Code.Cmpxchg8b_Mq };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_LockPrefix_Data))]
		void Test64_LockPrefix(string hexBytes, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.True(instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test64_LockPrefix_Data {
			get {
				yield return new object[] { "F0 10 08", Code.Adc_Eb_Gb };
				yield return new object[] { "F0 80 10 A5", Code.Adc_Eb_Ib };
				yield return new object[] { "F0 66 11 08", Code.Adc_Ew_Gw };
				yield return new object[] { "F0 66 83 10 A5", Code.Adc_Ew_Ib16 };
				yield return new object[] { "F0 66 81 10 5AA5", Code.Adc_Ew_Iw };
				yield return new object[] { "F0 11 08", Code.Adc_Ed_Gd };
				yield return new object[] { "F0 83 10 A5", Code.Adc_Ed_Ib32 };
				yield return new object[] { "F0 81 10 34 12 5AA5", Code.Adc_Ed_Id };
				yield return new object[] { "F0 48 11 08", Code.Adc_Eq_Gq };
				yield return new object[] { "F0 48 83 10 A5", Code.Adc_Eq_Ib64 };
				yield return new object[] { "F0 48 81 10 34 12 5AA5", Code.Adc_Eq_Id64 };

				yield return new object[] { "F0 00 08", Code.Add_Eb_Gb };
				yield return new object[] { "F0 80 00 A5", Code.Add_Eb_Ib };
				yield return new object[] { "F0 66 01 08", Code.Add_Ew_Gw };
				yield return new object[] { "F0 66 83 00 A5", Code.Add_Ew_Ib16 };
				yield return new object[] { "F0 66 81 00 5AA5", Code.Add_Ew_Iw };
				yield return new object[] { "F0 01 08", Code.Add_Ed_Gd };
				yield return new object[] { "F0 83 00 A5", Code.Add_Ed_Ib32 };
				yield return new object[] { "F0 81 00 34 12 5AA5", Code.Add_Ed_Id };
				yield return new object[] { "F0 48 01 08", Code.Add_Eq_Gq };
				yield return new object[] { "F0 48 83 00 A5", Code.Add_Eq_Ib64 };
				yield return new object[] { "F0 48 81 00 34 12 5AA5", Code.Add_Eq_Id64 };

				yield return new object[] { "F0 20 08", Code.And_Eb_Gb };
				yield return new object[] { "F0 80 20 A5", Code.And_Eb_Ib };
				yield return new object[] { "F0 66 21 08", Code.And_Ew_Gw };
				yield return new object[] { "F0 66 83 20 A5", Code.And_Ew_Ib16 };
				yield return new object[] { "F0 66 81 20 5AA5", Code.And_Ew_Iw };
				yield return new object[] { "F0 21 08", Code.And_Ed_Gd };
				yield return new object[] { "F0 83 20 A5", Code.And_Ed_Ib32 };
				yield return new object[] { "F0 81 20 34 12 5AA5", Code.And_Ed_Id };
				yield return new object[] { "F0 48 21 08", Code.And_Eq_Gq };
				yield return new object[] { "F0 48 83 20 A5", Code.And_Eq_Ib64 };
				yield return new object[] { "F0 48 81 20 34 12 5AA5", Code.And_Eq_Id64 };

				yield return new object[] { "F0 08 08", Code.Or_Eb_Gb };
				yield return new object[] { "F0 80 08 A5", Code.Or_Eb_Ib };
				yield return new object[] { "F0 66 09 08", Code.Or_Ew_Gw };
				yield return new object[] { "F0 66 83 08 A5", Code.Or_Ew_Ib16 };
				yield return new object[] { "F0 66 81 08 5AA5", Code.Or_Ew_Iw };
				yield return new object[] { "F0 09 08", Code.Or_Ed_Gd };
				yield return new object[] { "F0 83 08 A5", Code.Or_Ed_Ib32 };
				yield return new object[] { "F0 81 08 34 12 5AA5", Code.Or_Ed_Id };
				yield return new object[] { "F0 48 09 08", Code.Or_Eq_Gq };
				yield return new object[] { "F0 48 83 08 A5", Code.Or_Eq_Ib64 };
				yield return new object[] { "F0 48 81 08 34 12 5AA5", Code.Or_Eq_Id64 };

				yield return new object[] { "F0 18 08", Code.Sbb_Eb_Gb };
				yield return new object[] { "F0 80 18 A5", Code.Sbb_Eb_Ib };
				yield return new object[] { "F0 66 19 08", Code.Sbb_Ew_Gw };
				yield return new object[] { "F0 66 83 18 A5", Code.Sbb_Ew_Ib16 };
				yield return new object[] { "F0 66 81 18 5AA5", Code.Sbb_Ew_Iw };
				yield return new object[] { "F0 19 08", Code.Sbb_Ed_Gd };
				yield return new object[] { "F0 83 18 A5", Code.Sbb_Ed_Ib32 };
				yield return new object[] { "F0 81 18 34 12 5AA5", Code.Sbb_Ed_Id };
				yield return new object[] { "F0 48 19 08", Code.Sbb_Eq_Gq };
				yield return new object[] { "F0 48 83 18 A5", Code.Sbb_Eq_Ib64 };
				yield return new object[] { "F0 48 81 18 34 12 5AA5", Code.Sbb_Eq_Id64 };

				yield return new object[] { "F0 28 08", Code.Sub_Eb_Gb };
				yield return new object[] { "F0 80 28 A5", Code.Sub_Eb_Ib };
				yield return new object[] { "F0 66 29 08", Code.Sub_Ew_Gw };
				yield return new object[] { "F0 66 83 28 A5", Code.Sub_Ew_Ib16 };
				yield return new object[] { "F0 66 81 28 5AA5", Code.Sub_Ew_Iw };
				yield return new object[] { "F0 29 08", Code.Sub_Ed_Gd };
				yield return new object[] { "F0 83 28 A5", Code.Sub_Ed_Ib32 };
				yield return new object[] { "F0 81 28 34 12 5AA5", Code.Sub_Ed_Id };
				yield return new object[] { "F0 48 29 08", Code.Sub_Eq_Gq };
				yield return new object[] { "F0 48 83 28 A5", Code.Sub_Eq_Ib64 };
				yield return new object[] { "F0 48 81 28 34 12 5AA5", Code.Sub_Eq_Id64 };

				yield return new object[] { "F0 30 08", Code.Xor_Eb_Gb };
				yield return new object[] { "F0 80 30 A5", Code.Xor_Eb_Ib };
				yield return new object[] { "F0 66 31 08", Code.Xor_Ew_Gw };
				yield return new object[] { "F0 66 83 30 A5", Code.Xor_Ew_Ib16 };
				yield return new object[] { "F0 66 81 30 5AA5", Code.Xor_Ew_Iw };
				yield return new object[] { "F0 31 08", Code.Xor_Ed_Gd };
				yield return new object[] { "F0 83 30 A5", Code.Xor_Ed_Ib32 };
				yield return new object[] { "F0 81 30 34 12 5AA5", Code.Xor_Ed_Id };
				yield return new object[] { "F0 48 31 08", Code.Xor_Eq_Gq };
				yield return new object[] { "F0 48 83 30 A5", Code.Xor_Eq_Ib64 };
				yield return new object[] { "F0 48 81 30 34 12 5AA5", Code.Xor_Eq_Id64 };

				yield return new object[] { "F0 66 0F BB 08", Code.Btc_Ew_Gw };
				yield return new object[] { "F0 66 0F BA 38 A5", Code.Btc_Ew_Ib };
				yield return new object[] { "F0 0F BB 08", Code.Btc_Ed_Gd };
				yield return new object[] { "F0 0F BA 38 A5", Code.Btc_Ed_Ib };
				yield return new object[] { "F0 48 0F BB 08", Code.Btc_Eq_Gq };
				yield return new object[] { "F0 48 0F BA 38 A5", Code.Btc_Eq_Ib };

				yield return new object[] { "F0 66 0F B3 08", Code.Btr_Ew_Gw };
				yield return new object[] { "F0 66 0F BA 30 A5", Code.Btr_Ew_Ib };
				yield return new object[] { "F0 0F B3 08", Code.Btr_Ed_Gd };
				yield return new object[] { "F0 0F BA 30 A5", Code.Btr_Ed_Ib };
				yield return new object[] { "F0 48 0F B3 08", Code.Btr_Eq_Gq };
				yield return new object[] { "F0 48 0F BA 30 A5", Code.Btr_Eq_Ib };

				yield return new object[] { "F0 66 0F AB 08", Code.Bts_Ew_Gw };
				yield return new object[] { "F0 66 0F BA 28 A5", Code.Bts_Ew_Ib };
				yield return new object[] { "F0 0F AB 08", Code.Bts_Ed_Gd };
				yield return new object[] { "F0 0F BA 28 A5", Code.Bts_Ed_Ib };
				yield return new object[] { "F0 48 0F AB 08", Code.Bts_Eq_Gq };
				yield return new object[] { "F0 48 0F BA 28 A5", Code.Bts_Eq_Ib };

				yield return new object[] { "F0 FE 08", Code.Dec_Eb };
				yield return new object[] { "F0 66 FF 08", Code.Dec_Ew };
				yield return new object[] { "F0 FF 08", Code.Dec_Ed };
				yield return new object[] { "F0 48 FF 08", Code.Dec_Eq };

				yield return new object[] { "F0 FE 00", Code.Inc_Eb };
				yield return new object[] { "F0 66 FF 00", Code.Inc_Ew };
				yield return new object[] { "F0 FF 00", Code.Inc_Ed };
				yield return new object[] { "F0 48 FF 00", Code.Inc_Eq };

				yield return new object[] { "F0 F6 18", Code.Neg_Eb };
				yield return new object[] { "F0 66 F7 18", Code.Neg_Ew };
				yield return new object[] { "F0 F7 18", Code.Neg_Ed };
				yield return new object[] { "F0 48 F7 18", Code.Neg_Eq };

				yield return new object[] { "F0 F6 10", Code.Not_Eb };
				yield return new object[] { "F0 66 F7 10", Code.Not_Ew };
				yield return new object[] { "F0 F7 10", Code.Not_Ed };
				yield return new object[] { "F0 48 F7 10", Code.Not_Eq };

				yield return new object[] { "F0 0F B0 08", Code.Cmpxchg_Eb_Gb };
				yield return new object[] { "F0 66 0F B1 08", Code.Cmpxchg_Ew_Gw };
				yield return new object[] { "F0 0F B1 08", Code.Cmpxchg_Ed_Gd };
				yield return new object[] { "F0 48 0F B1 08", Code.Cmpxchg_Eq_Gq };

				yield return new object[] { "F0 0F C0 08", Code.Xadd_Eb_Gb };
				yield return new object[] { "F0 66 0F C1 08", Code.Xadd_Ew_Gw };
				yield return new object[] { "F0 0F C1 08", Code.Xadd_Ed_Gd };
				yield return new object[] { "F0 48 0F C1 08", Code.Xadd_Eq_Gq };

				yield return new object[] { "F0 86 08", Code.Xchg_Eb_Gb };
				yield return new object[] { "F0 66 87 08", Code.Xchg_Ew_Gw };
				yield return new object[] { "F0 87 08", Code.Xchg_Ed_Gd };
				yield return new object[] { "F0 48 87 08", Code.Xchg_Eq_Gq };

				yield return new object[] { "F0 0F C7 08", Code.Cmpxchg8b_Mq };
				yield return new object[] { "F0 48 0F C7 08", Code.Cmpxchg16b_Mo };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XacquirePrefix_Data))]
		void Test16_XacquirePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.Equal(hasLock, instr.HasPrefixLock);
			Assert.True(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test16_XacquirePrefix_Data {
			get {
				yield return new object[] { "F2 F0 10 08", Code.Adc_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 10 A5", Code.Adc_Eb_Ib, true };
				yield return new object[] { "F2 F0 11 08", Code.Adc_Ew_Gw, true };
				yield return new object[] { "F2 F0 83 10 A5", Code.Adc_Ew_Ib16, true };
				yield return new object[] { "F2 F0 81 10 5AA5", Code.Adc_Ew_Iw, true };
				yield return new object[] { "F2 F0 66 11 08", Code.Adc_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 83 10 A5", Code.Adc_Ed_Ib32, true };
				yield return new object[] { "F2 F0 66 81 10 34 12 5AA5", Code.Adc_Ed_Id, true };

				yield return new object[] { "F2 F0 00 08", Code.Add_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 00 A5", Code.Add_Eb_Ib, true };
				yield return new object[] { "F2 F0 01 08", Code.Add_Ew_Gw, true };
				yield return new object[] { "F2 F0 83 00 A5", Code.Add_Ew_Ib16, true };
				yield return new object[] { "F2 F0 81 00 5AA5", Code.Add_Ew_Iw, true };
				yield return new object[] { "F2 F0 66 01 08", Code.Add_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 83 00 A5", Code.Add_Ed_Ib32, true };
				yield return new object[] { "F2 F0 66 81 00 34 12 5AA5", Code.Add_Ed_Id, true };

				yield return new object[] { "F2 F0 20 08", Code.And_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 20 A5", Code.And_Eb_Ib, true };
				yield return new object[] { "F2 F0 21 08", Code.And_Ew_Gw, true };
				yield return new object[] { "F2 F0 83 20 A5", Code.And_Ew_Ib16, true };
				yield return new object[] { "F2 F0 81 20 5AA5", Code.And_Ew_Iw, true };
				yield return new object[] { "F2 F0 66 21 08", Code.And_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 83 20 A5", Code.And_Ed_Ib32, true };
				yield return new object[] { "F2 F0 66 81 20 34 12 5AA5", Code.And_Ed_Id, true };

				yield return new object[] { "F2 F0 08 08", Code.Or_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 08 A5", Code.Or_Eb_Ib, true };
				yield return new object[] { "F2 F0 09 08", Code.Or_Ew_Gw, true };
				yield return new object[] { "F2 F0 83 08 A5", Code.Or_Ew_Ib16, true };
				yield return new object[] { "F2 F0 81 08 5AA5", Code.Or_Ew_Iw, true };
				yield return new object[] { "F2 F0 66 09 08", Code.Or_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 83 08 A5", Code.Or_Ed_Ib32, true };
				yield return new object[] { "F2 F0 66 81 08 34 12 5AA5", Code.Or_Ed_Id, true };

				yield return new object[] { "F2 F0 18 08", Code.Sbb_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 18 A5", Code.Sbb_Eb_Ib, true };
				yield return new object[] { "F2 F0 19 08", Code.Sbb_Ew_Gw, true };
				yield return new object[] { "F2 F0 83 18 A5", Code.Sbb_Ew_Ib16, true };
				yield return new object[] { "F2 F0 81 18 5AA5", Code.Sbb_Ew_Iw, true };
				yield return new object[] { "F2 F0 66 19 08", Code.Sbb_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 83 18 A5", Code.Sbb_Ed_Ib32, true };
				yield return new object[] { "F2 F0 66 81 18 34 12 5AA5", Code.Sbb_Ed_Id, true };

				yield return new object[] { "F2 F0 28 08", Code.Sub_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 28 A5", Code.Sub_Eb_Ib, true };
				yield return new object[] { "F2 F0 29 08", Code.Sub_Ew_Gw, true };
				yield return new object[] { "F2 F0 83 28 A5", Code.Sub_Ew_Ib16, true };
				yield return new object[] { "F2 F0 81 28 5AA5", Code.Sub_Ew_Iw, true };
				yield return new object[] { "F2 F0 66 29 08", Code.Sub_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 83 28 A5", Code.Sub_Ed_Ib32, true };
				yield return new object[] { "F2 F0 66 81 28 34 12 5AA5", Code.Sub_Ed_Id, true };

				yield return new object[] { "F2 F0 30 08", Code.Xor_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 30 A5", Code.Xor_Eb_Ib, true };
				yield return new object[] { "F2 F0 31 08", Code.Xor_Ew_Gw, true };
				yield return new object[] { "F2 F0 83 30 A5", Code.Xor_Ew_Ib16, true };
				yield return new object[] { "F2 F0 81 30 5AA5", Code.Xor_Ew_Iw, true };
				yield return new object[] { "F2 F0 66 31 08", Code.Xor_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 83 30 A5", Code.Xor_Ed_Ib32, true };
				yield return new object[] { "F2 F0 66 81 30 34 12 5AA5", Code.Xor_Ed_Id, true };

				yield return new object[] { "F2 F0 0F BB 08", Code.Btc_Ew_Gw, true };
				yield return new object[] { "F2 F0 0F BA 38 A5", Code.Btc_Ew_Ib, true };
				yield return new object[] { "F2 F0 66 0F BB 08", Code.Btc_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 0F BA 38 A5", Code.Btc_Ed_Ib, true };

				yield return new object[] { "F2 F0 0F B3 08", Code.Btr_Ew_Gw, true };
				yield return new object[] { "F2 F0 0F BA 30 A5", Code.Btr_Ew_Ib, true };
				yield return new object[] { "F2 F0 66 0F B3 08", Code.Btr_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 0F BA 30 A5", Code.Btr_Ed_Ib, true };

				yield return new object[] { "F2 F0 0F AB 08", Code.Bts_Ew_Gw, true };
				yield return new object[] { "F2 F0 0F BA 28 A5", Code.Bts_Ew_Ib, true };
				yield return new object[] { "F2 F0 66 0F AB 08", Code.Bts_Ed_Gd, true };
				yield return new object[] { "F2 F0 66 0F BA 28 A5", Code.Bts_Ed_Ib, true };

				yield return new object[] { "F2 F0 FE 08", Code.Dec_Eb, true };
				yield return new object[] { "F2 F0 FF 08", Code.Dec_Ew, true };
				yield return new object[] { "F2 F0 66 FF 08", Code.Dec_Ed, true };

				yield return new object[] { "F2 F0 FE 00", Code.Inc_Eb, true };
				yield return new object[] { "F2 F0 FF 00", Code.Inc_Ew, true };
				yield return new object[] { "F2 F0 66 FF 00", Code.Inc_Ed, true };

				yield return new object[] { "F2 F0 F6 18", Code.Neg_Eb, true };
				yield return new object[] { "F2 F0 F7 18", Code.Neg_Ew, true };
				yield return new object[] { "F2 F0 66 F7 18", Code.Neg_Ed, true };

				yield return new object[] { "F2 F0 F6 10", Code.Not_Eb, true };
				yield return new object[] { "F2 F0 F7 10", Code.Not_Ew, true };
				yield return new object[] { "F2 F0 66 F7 10", Code.Not_Ed, true };

				yield return new object[] { "F2 F0 0F B0 08", Code.Cmpxchg_Eb_Gb, true };
				yield return new object[] { "F2 F0 0F B1 08", Code.Cmpxchg_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F B1 08", Code.Cmpxchg_Ed_Gd, true };

				yield return new object[] { "F2 F0 0F C0 08", Code.Xadd_Eb_Gb, true };
				yield return new object[] { "F2 F0 0F C1 08", Code.Xadd_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F C1 08", Code.Xadd_Ed_Gd, true };

				yield return new object[] { "F2 86 08", Code.Xchg_Eb_Gb, false };
				yield return new object[] { "F2 87 08", Code.Xchg_Ew_Gw, false };
				yield return new object[] { "F2 66 87 08", Code.Xchg_Ed_Gd, false };

				yield return new object[] { "F2 F0 86 08", Code.Xchg_Eb_Gb, true };
				yield return new object[] { "F2 F0 87 08", Code.Xchg_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 87 08", Code.Xchg_Ed_Gd, true };

				yield return new object[] { "F2 F0 0F C7 08", Code.Cmpxchg8b_Mq, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XacquirePrefix_Data))]
		void Test32_XacquirePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.Equal(hasLock, instr.HasPrefixLock);
			Assert.True(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test32_XacquirePrefix_Data {
			get {
				yield return new object[] { "F2 F0 10 08", Code.Adc_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 10 A5", Code.Adc_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 11 08", Code.Adc_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 10 A5", Code.Adc_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 10 5AA5", Code.Adc_Ew_Iw, true };
				yield return new object[] { "F2 F0 11 08", Code.Adc_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 10 A5", Code.Adc_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 10 34 12 5AA5", Code.Adc_Ed_Id, true };

				yield return new object[] { "F2 F0 00 08", Code.Add_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 00 A5", Code.Add_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 01 08", Code.Add_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 00 A5", Code.Add_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 00 5AA5", Code.Add_Ew_Iw, true };
				yield return new object[] { "F2 F0 01 08", Code.Add_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 00 A5", Code.Add_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 00 34 12 5AA5", Code.Add_Ed_Id, true };

				yield return new object[] { "F2 F0 20 08", Code.And_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 20 A5", Code.And_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 21 08", Code.And_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 20 A5", Code.And_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 20 5AA5", Code.And_Ew_Iw, true };
				yield return new object[] { "F2 F0 21 08", Code.And_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 20 A5", Code.And_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 20 34 12 5AA5", Code.And_Ed_Id, true };

				yield return new object[] { "F2 F0 08 08", Code.Or_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 08 A5", Code.Or_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 09 08", Code.Or_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 08 A5", Code.Or_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 08 5AA5", Code.Or_Ew_Iw, true };
				yield return new object[] { "F2 F0 09 08", Code.Or_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 08 A5", Code.Or_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 08 34 12 5AA5", Code.Or_Ed_Id, true };

				yield return new object[] { "F2 F0 18 08", Code.Sbb_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 18 A5", Code.Sbb_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 19 08", Code.Sbb_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 18 A5", Code.Sbb_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 18 5AA5", Code.Sbb_Ew_Iw, true };
				yield return new object[] { "F2 F0 19 08", Code.Sbb_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 18 A5", Code.Sbb_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 18 34 12 5AA5", Code.Sbb_Ed_Id, true };

				yield return new object[] { "F2 F0 28 08", Code.Sub_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 28 A5", Code.Sub_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 29 08", Code.Sub_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 28 A5", Code.Sub_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 28 5AA5", Code.Sub_Ew_Iw, true };
				yield return new object[] { "F2 F0 29 08", Code.Sub_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 28 A5", Code.Sub_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 28 34 12 5AA5", Code.Sub_Ed_Id, true };

				yield return new object[] { "F2 F0 30 08", Code.Xor_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 30 A5", Code.Xor_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 31 08", Code.Xor_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 30 A5", Code.Xor_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 30 5AA5", Code.Xor_Ew_Iw, true };
				yield return new object[] { "F2 F0 31 08", Code.Xor_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 30 A5", Code.Xor_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 30 34 12 5AA5", Code.Xor_Ed_Id, true };

				yield return new object[] { "F2 F0 66 0F BB 08", Code.Btc_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F BA 38 A5", Code.Btc_Ew_Ib, true };
				yield return new object[] { "F2 F0 0F BB 08", Code.Btc_Ed_Gd, true };
				yield return new object[] { "F2 F0 0F BA 38 A5", Code.Btc_Ed_Ib, true };

				yield return new object[] { "F2 F0 66 0F B3 08", Code.Btr_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F BA 30 A5", Code.Btr_Ew_Ib, true };
				yield return new object[] { "F2 F0 0F B3 08", Code.Btr_Ed_Gd, true };
				yield return new object[] { "F2 F0 0F BA 30 A5", Code.Btr_Ed_Ib, true };

				yield return new object[] { "F2 F0 66 0F AB 08", Code.Bts_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F BA 28 A5", Code.Bts_Ew_Ib, true };
				yield return new object[] { "F2 F0 0F AB 08", Code.Bts_Ed_Gd, true };
				yield return new object[] { "F2 F0 0F BA 28 A5", Code.Bts_Ed_Ib, true };

				yield return new object[] { "F2 F0 FE 08", Code.Dec_Eb, true };
				yield return new object[] { "F2 F0 66 FF 08", Code.Dec_Ew, true };
				yield return new object[] { "F2 F0 FF 08", Code.Dec_Ed, true };

				yield return new object[] { "F2 F0 FE 00", Code.Inc_Eb, true };
				yield return new object[] { "F2 F0 66 FF 00", Code.Inc_Ew, true };
				yield return new object[] { "F2 F0 FF 00", Code.Inc_Ed, true };

				yield return new object[] { "F2 F0 F6 18", Code.Neg_Eb, true };
				yield return new object[] { "F2 F0 66 F7 18", Code.Neg_Ew, true };
				yield return new object[] { "F2 F0 F7 18", Code.Neg_Ed, true };

				yield return new object[] { "F2 F0 F6 10", Code.Not_Eb, true };
				yield return new object[] { "F2 F0 66 F7 10", Code.Not_Ew, true };
				yield return new object[] { "F2 F0 F7 10", Code.Not_Ed, true };

				yield return new object[] { "F2 F0 0F B0 08", Code.Cmpxchg_Eb_Gb, true };
				yield return new object[] { "F2 F0 66 0F B1 08", Code.Cmpxchg_Ew_Gw, true };
				yield return new object[] { "F2 F0 0F B1 08", Code.Cmpxchg_Ed_Gd, true };

				yield return new object[] { "F2 F0 0F C0 08", Code.Xadd_Eb_Gb, true };
				yield return new object[] { "F2 F0 66 0F C1 08", Code.Xadd_Ew_Gw, true };
				yield return new object[] { "F2 F0 0F C1 08", Code.Xadd_Ed_Gd, true };

				yield return new object[] { "F2 86 08", Code.Xchg_Eb_Gb, false };
				yield return new object[] { "F2 66 87 08", Code.Xchg_Ew_Gw, false };
				yield return new object[] { "F2 87 08", Code.Xchg_Ed_Gd, false };

				yield return new object[] { "F2 F0 86 08", Code.Xchg_Eb_Gb, true };
				yield return new object[] { "F2 F0 66 87 08", Code.Xchg_Ew_Gw, true };
				yield return new object[] { "F2 F0 87 08", Code.Xchg_Ed_Gd, true };

				yield return new object[] { "F2 F0 0F C7 08", Code.Cmpxchg8b_Mq, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XacquirePrefix_Data))]
		void Test64_XacquirePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.Equal(hasLock, instr.HasPrefixLock);
			Assert.True(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test64_XacquirePrefix_Data {
			get {
				yield return new object[] { "F2 F0 10 08", Code.Adc_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 10 A5", Code.Adc_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 11 08", Code.Adc_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 10 A5", Code.Adc_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 10 5AA5", Code.Adc_Ew_Iw, true };
				yield return new object[] { "F2 F0 11 08", Code.Adc_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 10 A5", Code.Adc_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 10 34 12 5AA5", Code.Adc_Ed_Id, true };
				yield return new object[] { "F2 F0 48 11 08", Code.Adc_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 83 10 A5", Code.Adc_Eq_Ib64, true };
				yield return new object[] { "F2 F0 48 81 10 34 12 5AA5", Code.Adc_Eq_Id64, true };

				yield return new object[] { "F2 F0 00 08", Code.Add_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 00 A5", Code.Add_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 01 08", Code.Add_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 00 A5", Code.Add_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 00 5AA5", Code.Add_Ew_Iw, true };
				yield return new object[] { "F2 F0 01 08", Code.Add_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 00 A5", Code.Add_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 00 34 12 5AA5", Code.Add_Ed_Id, true };
				yield return new object[] { "F2 F0 48 01 08", Code.Add_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 83 00 A5", Code.Add_Eq_Ib64, true };
				yield return new object[] { "F2 F0 48 81 00 34 12 5AA5", Code.Add_Eq_Id64, true };

				yield return new object[] { "F2 F0 20 08", Code.And_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 20 A5", Code.And_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 21 08", Code.And_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 20 A5", Code.And_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 20 5AA5", Code.And_Ew_Iw, true };
				yield return new object[] { "F2 F0 21 08", Code.And_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 20 A5", Code.And_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 20 34 12 5AA5", Code.And_Ed_Id, true };
				yield return new object[] { "F2 F0 48 21 08", Code.And_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 83 20 A5", Code.And_Eq_Ib64, true };
				yield return new object[] { "F2 F0 48 81 20 34 12 5AA5", Code.And_Eq_Id64, true };

				yield return new object[] { "F2 F0 08 08", Code.Or_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 08 A5", Code.Or_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 09 08", Code.Or_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 08 A5", Code.Or_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 08 5AA5", Code.Or_Ew_Iw, true };
				yield return new object[] { "F2 F0 09 08", Code.Or_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 08 A5", Code.Or_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 08 34 12 5AA5", Code.Or_Ed_Id, true };
				yield return new object[] { "F2 F0 48 09 08", Code.Or_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 83 08 A5", Code.Or_Eq_Ib64, true };
				yield return new object[] { "F2 F0 48 81 08 34 12 5AA5", Code.Or_Eq_Id64, true };

				yield return new object[] { "F2 F0 18 08", Code.Sbb_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 18 A5", Code.Sbb_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 19 08", Code.Sbb_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 18 A5", Code.Sbb_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 18 5AA5", Code.Sbb_Ew_Iw, true };
				yield return new object[] { "F2 F0 19 08", Code.Sbb_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 18 A5", Code.Sbb_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 18 34 12 5AA5", Code.Sbb_Ed_Id, true };
				yield return new object[] { "F2 F0 48 19 08", Code.Sbb_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 83 18 A5", Code.Sbb_Eq_Ib64, true };
				yield return new object[] { "F2 F0 48 81 18 34 12 5AA5", Code.Sbb_Eq_Id64, true };

				yield return new object[] { "F2 F0 28 08", Code.Sub_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 28 A5", Code.Sub_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 29 08", Code.Sub_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 28 A5", Code.Sub_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 28 5AA5", Code.Sub_Ew_Iw, true };
				yield return new object[] { "F2 F0 29 08", Code.Sub_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 28 A5", Code.Sub_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 28 34 12 5AA5", Code.Sub_Ed_Id, true };
				yield return new object[] { "F2 F0 48 29 08", Code.Sub_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 83 28 A5", Code.Sub_Eq_Ib64, true };
				yield return new object[] { "F2 F0 48 81 28 34 12 5AA5", Code.Sub_Eq_Id64, true };

				yield return new object[] { "F2 F0 30 08", Code.Xor_Eb_Gb, true };
				yield return new object[] { "F2 F0 80 30 A5", Code.Xor_Eb_Ib, true };
				yield return new object[] { "F2 F0 66 31 08", Code.Xor_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 83 30 A5", Code.Xor_Ew_Ib16, true };
				yield return new object[] { "F2 F0 66 81 30 5AA5", Code.Xor_Ew_Iw, true };
				yield return new object[] { "F2 F0 31 08", Code.Xor_Ed_Gd, true };
				yield return new object[] { "F2 F0 83 30 A5", Code.Xor_Ed_Ib32, true };
				yield return new object[] { "F2 F0 81 30 34 12 5AA5", Code.Xor_Ed_Id, true };
				yield return new object[] { "F2 F0 48 31 08", Code.Xor_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 83 30 A5", Code.Xor_Eq_Ib64, true };
				yield return new object[] { "F2 F0 48 81 30 34 12 5AA5", Code.Xor_Eq_Id64, true };

				yield return new object[] { "F2 F0 66 0F BB 08", Code.Btc_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F BA 38 A5", Code.Btc_Ew_Ib, true };
				yield return new object[] { "F2 F0 0F BB 08", Code.Btc_Ed_Gd, true };
				yield return new object[] { "F2 F0 0F BA 38 A5", Code.Btc_Ed_Ib, true };
				yield return new object[] { "F2 F0 48 0F BB 08", Code.Btc_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 0F BA 38 A5", Code.Btc_Eq_Ib, true };

				yield return new object[] { "F2 F0 66 0F B3 08", Code.Btr_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F BA 30 A5", Code.Btr_Ew_Ib, true };
				yield return new object[] { "F2 F0 0F B3 08", Code.Btr_Ed_Gd, true };
				yield return new object[] { "F2 F0 0F BA 30 A5", Code.Btr_Ed_Ib, true };
				yield return new object[] { "F2 F0 48 0F B3 08", Code.Btr_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 0F BA 30 A5", Code.Btr_Eq_Ib, true };

				yield return new object[] { "F2 F0 66 0F AB 08", Code.Bts_Ew_Gw, true };
				yield return new object[] { "F2 F0 66 0F BA 28 A5", Code.Bts_Ew_Ib, true };
				yield return new object[] { "F2 F0 0F AB 08", Code.Bts_Ed_Gd, true };
				yield return new object[] { "F2 F0 0F BA 28 A5", Code.Bts_Ed_Ib, true };
				yield return new object[] { "F2 F0 48 0F AB 08", Code.Bts_Eq_Gq, true };
				yield return new object[] { "F2 F0 48 0F BA 28 A5", Code.Bts_Eq_Ib, true };

				yield return new object[] { "F2 F0 FE 08", Code.Dec_Eb, true };
				yield return new object[] { "F2 F0 66 FF 08", Code.Dec_Ew, true };
				yield return new object[] { "F2 F0 FF 08", Code.Dec_Ed, true };
				yield return new object[] { "F2 F0 48 FF 08", Code.Dec_Eq, true };

				yield return new object[] { "F2 F0 FE 00", Code.Inc_Eb, true };
				yield return new object[] { "F2 F0 66 FF 00", Code.Inc_Ew, true };
				yield return new object[] { "F2 F0 FF 00", Code.Inc_Ed, true };
				yield return new object[] { "F2 F0 48 FF 00", Code.Inc_Eq, true };

				yield return new object[] { "F2 F0 F6 18", Code.Neg_Eb, true };
				yield return new object[] { "F2 F0 66 F7 18", Code.Neg_Ew, true };
				yield return new object[] { "F2 F0 F7 18", Code.Neg_Ed, true };
				yield return new object[] { "F2 F0 48 F7 18", Code.Neg_Eq, true };

				yield return new object[] { "F2 F0 F6 10", Code.Not_Eb, true };
				yield return new object[] { "F2 F0 66 F7 10", Code.Not_Ew, true };
				yield return new object[] { "F2 F0 F7 10", Code.Not_Ed, true };
				yield return new object[] { "F2 F0 48 F7 10", Code.Not_Eq, true };

				yield return new object[] { "F2 F0 0F B0 08", Code.Cmpxchg_Eb_Gb, true };
				yield return new object[] { "F2 F0 66 0F B1 08", Code.Cmpxchg_Ew_Gw, true };
				yield return new object[] { "F2 F0 0F B1 08", Code.Cmpxchg_Ed_Gd, true };
				yield return new object[] { "F2 F0 48 0F B1 08", Code.Cmpxchg_Eq_Gq, true };

				yield return new object[] { "F2 F0 0F C0 08", Code.Xadd_Eb_Gb, true };
				yield return new object[] { "F2 F0 66 0F C1 08", Code.Xadd_Ew_Gw, true };
				yield return new object[] { "F2 F0 0F C1 08", Code.Xadd_Ed_Gd, true };
				yield return new object[] { "F2 F0 48 0F C1 08", Code.Xadd_Eq_Gq, true };

				yield return new object[] { "F2 86 08", Code.Xchg_Eb_Gb, false };
				yield return new object[] { "F2 66 87 08", Code.Xchg_Ew_Gw, false };
				yield return new object[] { "F2 87 08", Code.Xchg_Ed_Gd, false };
				yield return new object[] { "F2 48 87 08", Code.Xchg_Eq_Gq, false };

				yield return new object[] { "F2 F0 86 08", Code.Xchg_Eb_Gb, true };
				yield return new object[] { "F2 F0 66 87 08", Code.Xchg_Ew_Gw, true };
				yield return new object[] { "F2 F0 87 08", Code.Xchg_Ed_Gd, true };
				yield return new object[] { "F2 F0 48 87 08", Code.Xchg_Eq_Gq, true };

				yield return new object[] { "F2 F0 0F C7 08", Code.Cmpxchg8b_Mq, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XreleasePrefix_Data))]
		void Test16_XreleasePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.Equal(hasLock, instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.True(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test16_XreleasePrefix_Data {
			get {
				yield return new object[] { "F3 F0 10 08", Code.Adc_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 10 A5", Code.Adc_Eb_Ib, true };
				yield return new object[] { "F3 F0 11 08", Code.Adc_Ew_Gw, true };
				yield return new object[] { "F3 F0 83 10 A5", Code.Adc_Ew_Ib16, true };
				yield return new object[] { "F3 F0 81 10 5AA5", Code.Adc_Ew_Iw, true };
				yield return new object[] { "F3 F0 66 11 08", Code.Adc_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 83 10 A5", Code.Adc_Ed_Ib32, true };
				yield return new object[] { "F3 F0 66 81 10 34 12 5AA5", Code.Adc_Ed_Id, true };

				yield return new object[] { "F3 F0 00 08", Code.Add_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 00 A5", Code.Add_Eb_Ib, true };
				yield return new object[] { "F3 F0 01 08", Code.Add_Ew_Gw, true };
				yield return new object[] { "F3 F0 83 00 A5", Code.Add_Ew_Ib16, true };
				yield return new object[] { "F3 F0 81 00 5AA5", Code.Add_Ew_Iw, true };
				yield return new object[] { "F3 F0 66 01 08", Code.Add_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 83 00 A5", Code.Add_Ed_Ib32, true };
				yield return new object[] { "F3 F0 66 81 00 34 12 5AA5", Code.Add_Ed_Id, true };

				yield return new object[] { "F3 F0 20 08", Code.And_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 20 A5", Code.And_Eb_Ib, true };
				yield return new object[] { "F3 F0 21 08", Code.And_Ew_Gw, true };
				yield return new object[] { "F3 F0 83 20 A5", Code.And_Ew_Ib16, true };
				yield return new object[] { "F3 F0 81 20 5AA5", Code.And_Ew_Iw, true };
				yield return new object[] { "F3 F0 66 21 08", Code.And_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 83 20 A5", Code.And_Ed_Ib32, true };
				yield return new object[] { "F3 F0 66 81 20 34 12 5AA5", Code.And_Ed_Id, true };

				yield return new object[] { "F3 F0 08 08", Code.Or_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 08 A5", Code.Or_Eb_Ib, true };
				yield return new object[] { "F3 F0 09 08", Code.Or_Ew_Gw, true };
				yield return new object[] { "F3 F0 83 08 A5", Code.Or_Ew_Ib16, true };
				yield return new object[] { "F3 F0 81 08 5AA5", Code.Or_Ew_Iw, true };
				yield return new object[] { "F3 F0 66 09 08", Code.Or_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 83 08 A5", Code.Or_Ed_Ib32, true };
				yield return new object[] { "F3 F0 66 81 08 34 12 5AA5", Code.Or_Ed_Id, true };

				yield return new object[] { "F3 F0 18 08", Code.Sbb_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 18 A5", Code.Sbb_Eb_Ib, true };
				yield return new object[] { "F3 F0 19 08", Code.Sbb_Ew_Gw, true };
				yield return new object[] { "F3 F0 83 18 A5", Code.Sbb_Ew_Ib16, true };
				yield return new object[] { "F3 F0 81 18 5AA5", Code.Sbb_Ew_Iw, true };
				yield return new object[] { "F3 F0 66 19 08", Code.Sbb_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 83 18 A5", Code.Sbb_Ed_Ib32, true };
				yield return new object[] { "F3 F0 66 81 18 34 12 5AA5", Code.Sbb_Ed_Id, true };

				yield return new object[] { "F3 F0 28 08", Code.Sub_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 28 A5", Code.Sub_Eb_Ib, true };
				yield return new object[] { "F3 F0 29 08", Code.Sub_Ew_Gw, true };
				yield return new object[] { "F3 F0 83 28 A5", Code.Sub_Ew_Ib16, true };
				yield return new object[] { "F3 F0 81 28 5AA5", Code.Sub_Ew_Iw, true };
				yield return new object[] { "F3 F0 66 29 08", Code.Sub_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 83 28 A5", Code.Sub_Ed_Ib32, true };
				yield return new object[] { "F3 F0 66 81 28 34 12 5AA5", Code.Sub_Ed_Id, true };

				yield return new object[] { "F3 F0 30 08", Code.Xor_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 30 A5", Code.Xor_Eb_Ib, true };
				yield return new object[] { "F3 F0 31 08", Code.Xor_Ew_Gw, true };
				yield return new object[] { "F3 F0 83 30 A5", Code.Xor_Ew_Ib16, true };
				yield return new object[] { "F3 F0 81 30 5AA5", Code.Xor_Ew_Iw, true };
				yield return new object[] { "F3 F0 66 31 08", Code.Xor_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 83 30 A5", Code.Xor_Ed_Ib32, true };
				yield return new object[] { "F3 F0 66 81 30 34 12 5AA5", Code.Xor_Ed_Id, true };

				yield return new object[] { "F3 F0 0F BB 08", Code.Btc_Ew_Gw, true };
				yield return new object[] { "F3 F0 0F BA 38 A5", Code.Btc_Ew_Ib, true };
				yield return new object[] { "F3 F0 66 0F BB 08", Code.Btc_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 0F BA 38 A5", Code.Btc_Ed_Ib, true };

				yield return new object[] { "F3 F0 0F B3 08", Code.Btr_Ew_Gw, true };
				yield return new object[] { "F3 F0 0F BA 30 A5", Code.Btr_Ew_Ib, true };
				yield return new object[] { "F3 F0 66 0F B3 08", Code.Btr_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 0F BA 30 A5", Code.Btr_Ed_Ib, true };

				yield return new object[] { "F3 F0 0F AB 08", Code.Bts_Ew_Gw, true };
				yield return new object[] { "F3 F0 0F BA 28 A5", Code.Bts_Ew_Ib, true };
				yield return new object[] { "F3 F0 66 0F AB 08", Code.Bts_Ed_Gd, true };
				yield return new object[] { "F3 F0 66 0F BA 28 A5", Code.Bts_Ed_Ib, true };

				yield return new object[] { "F3 F0 FE 08", Code.Dec_Eb, true };
				yield return new object[] { "F3 F0 FF 08", Code.Dec_Ew, true };
				yield return new object[] { "F3 F0 66 FF 08", Code.Dec_Ed, true };

				yield return new object[] { "F3 F0 FE 00", Code.Inc_Eb, true };
				yield return new object[] { "F3 F0 FF 00", Code.Inc_Ew, true };
				yield return new object[] { "F3 F0 66 FF 00", Code.Inc_Ed, true };

				yield return new object[] { "F3 F0 F6 18", Code.Neg_Eb, true };
				yield return new object[] { "F3 F0 F7 18", Code.Neg_Ew, true };
				yield return new object[] { "F3 F0 66 F7 18", Code.Neg_Ed, true };

				yield return new object[] { "F3 F0 F6 10", Code.Not_Eb, true };
				yield return new object[] { "F3 F0 F7 10", Code.Not_Ew, true };
				yield return new object[] { "F3 F0 66 F7 10", Code.Not_Ed, true };

				yield return new object[] { "F3 F0 0F B0 08", Code.Cmpxchg_Eb_Gb, true };
				yield return new object[] { "F3 F0 0F B1 08", Code.Cmpxchg_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F B1 08", Code.Cmpxchg_Ed_Gd, true };

				yield return new object[] { "F3 F0 0F C0 08", Code.Xadd_Eb_Gb, true };
				yield return new object[] { "F3 F0 0F C1 08", Code.Xadd_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F C1 08", Code.Xadd_Ed_Gd, true };

				yield return new object[] { "F3 86 08", Code.Xchg_Eb_Gb, false };
				yield return new object[] { "F3 87 08", Code.Xchg_Ew_Gw, false };
				yield return new object[] { "F3 66 87 08", Code.Xchg_Ed_Gd, false };

				yield return new object[] { "F3 F0 86 08", Code.Xchg_Eb_Gb, true };
				yield return new object[] { "F3 F0 87 08", Code.Xchg_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 87 08", Code.Xchg_Ed_Gd, true };

				yield return new object[] { "F3 F0 0F C7 08", Code.Cmpxchg8b_Mq, true };

				yield return new object[] { "F3 88 08", Code.Mov_Eb_Gb, false };
				yield return new object[] { "F3 89 08", Code.Mov_Ew_Gw, false };
				yield return new object[] { "F3 66 89 08", Code.Mov_Ed_Gd, false };

				yield return new object[] { "F3 C6 00 A5", Code.Mov_Eb_Ib, false };
				yield return new object[] { "F3 C7 00 A5FF", Code.Mov_Ew_Iw, false };
				yield return new object[] { "F3 66 C7 00 A5FFFFFF", Code.Mov_Ed_Id, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XreleasePrefix_Data))]
		void Test32_XreleasePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.Equal(hasLock, instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.True(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test32_XreleasePrefix_Data {
			get {
				yield return new object[] { "F3 F0 10 08", Code.Adc_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 10 A5", Code.Adc_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 11 08", Code.Adc_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 10 A5", Code.Adc_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 10 5AA5", Code.Adc_Ew_Iw, true };
				yield return new object[] { "F3 F0 11 08", Code.Adc_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 10 A5", Code.Adc_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 10 34 12 5AA5", Code.Adc_Ed_Id, true };

				yield return new object[] { "F3 F0 00 08", Code.Add_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 00 A5", Code.Add_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 01 08", Code.Add_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 00 A5", Code.Add_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 00 5AA5", Code.Add_Ew_Iw, true };
				yield return new object[] { "F3 F0 01 08", Code.Add_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 00 A5", Code.Add_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 00 34 12 5AA5", Code.Add_Ed_Id, true };

				yield return new object[] { "F3 F0 20 08", Code.And_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 20 A5", Code.And_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 21 08", Code.And_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 20 A5", Code.And_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 20 5AA5", Code.And_Ew_Iw, true };
				yield return new object[] { "F3 F0 21 08", Code.And_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 20 A5", Code.And_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 20 34 12 5AA5", Code.And_Ed_Id, true };

				yield return new object[] { "F3 F0 08 08", Code.Or_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 08 A5", Code.Or_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 09 08", Code.Or_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 08 A5", Code.Or_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 08 5AA5", Code.Or_Ew_Iw, true };
				yield return new object[] { "F3 F0 09 08", Code.Or_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 08 A5", Code.Or_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 08 34 12 5AA5", Code.Or_Ed_Id, true };

				yield return new object[] { "F3 F0 18 08", Code.Sbb_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 18 A5", Code.Sbb_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 19 08", Code.Sbb_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 18 A5", Code.Sbb_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 18 5AA5", Code.Sbb_Ew_Iw, true };
				yield return new object[] { "F3 F0 19 08", Code.Sbb_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 18 A5", Code.Sbb_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 18 34 12 5AA5", Code.Sbb_Ed_Id, true };

				yield return new object[] { "F3 F0 28 08", Code.Sub_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 28 A5", Code.Sub_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 29 08", Code.Sub_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 28 A5", Code.Sub_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 28 5AA5", Code.Sub_Ew_Iw, true };
				yield return new object[] { "F3 F0 29 08", Code.Sub_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 28 A5", Code.Sub_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 28 34 12 5AA5", Code.Sub_Ed_Id, true };

				yield return new object[] { "F3 F0 30 08", Code.Xor_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 30 A5", Code.Xor_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 31 08", Code.Xor_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 30 A5", Code.Xor_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 30 5AA5", Code.Xor_Ew_Iw, true };
				yield return new object[] { "F3 F0 31 08", Code.Xor_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 30 A5", Code.Xor_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 30 34 12 5AA5", Code.Xor_Ed_Id, true };

				yield return new object[] { "F3 F0 66 0F BB 08", Code.Btc_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F BA 38 A5", Code.Btc_Ew_Ib, true };
				yield return new object[] { "F3 F0 0F BB 08", Code.Btc_Ed_Gd, true };
				yield return new object[] { "F3 F0 0F BA 38 A5", Code.Btc_Ed_Ib, true };

				yield return new object[] { "F3 F0 66 0F B3 08", Code.Btr_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F BA 30 A5", Code.Btr_Ew_Ib, true };
				yield return new object[] { "F3 F0 0F B3 08", Code.Btr_Ed_Gd, true };
				yield return new object[] { "F3 F0 0F BA 30 A5", Code.Btr_Ed_Ib, true };

				yield return new object[] { "F3 F0 66 0F AB 08", Code.Bts_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F BA 28 A5", Code.Bts_Ew_Ib, true };
				yield return new object[] { "F3 F0 0F AB 08", Code.Bts_Ed_Gd, true };
				yield return new object[] { "F3 F0 0F BA 28 A5", Code.Bts_Ed_Ib, true };

				yield return new object[] { "F3 F0 FE 08", Code.Dec_Eb, true };
				yield return new object[] { "F3 F0 66 FF 08", Code.Dec_Ew, true };
				yield return new object[] { "F3 F0 FF 08", Code.Dec_Ed, true };

				yield return new object[] { "F3 F0 FE 00", Code.Inc_Eb, true };
				yield return new object[] { "F3 F0 66 FF 00", Code.Inc_Ew, true };
				yield return new object[] { "F3 F0 FF 00", Code.Inc_Ed, true };

				yield return new object[] { "F3 F0 F6 18", Code.Neg_Eb, true };
				yield return new object[] { "F3 F0 66 F7 18", Code.Neg_Ew, true };
				yield return new object[] { "F3 F0 F7 18", Code.Neg_Ed, true };

				yield return new object[] { "F3 F0 F6 10", Code.Not_Eb, true };
				yield return new object[] { "F3 F0 66 F7 10", Code.Not_Ew, true };
				yield return new object[] { "F3 F0 F7 10", Code.Not_Ed, true };

				yield return new object[] { "F3 F0 0F B0 08", Code.Cmpxchg_Eb_Gb, true };
				yield return new object[] { "F3 F0 66 0F B1 08", Code.Cmpxchg_Ew_Gw, true };
				yield return new object[] { "F3 F0 0F B1 08", Code.Cmpxchg_Ed_Gd, true };

				yield return new object[] { "F3 F0 0F C0 08", Code.Xadd_Eb_Gb, true };
				yield return new object[] { "F3 F0 66 0F C1 08", Code.Xadd_Ew_Gw, true };
				yield return new object[] { "F3 F0 0F C1 08", Code.Xadd_Ed_Gd, true };

				yield return new object[] { "F3 86 08", Code.Xchg_Eb_Gb, false };
				yield return new object[] { "F3 66 87 08", Code.Xchg_Ew_Gw, false };
				yield return new object[] { "F3 87 08", Code.Xchg_Ed_Gd, false };

				yield return new object[] { "F3 F0 86 08", Code.Xchg_Eb_Gb, true };
				yield return new object[] { "F3 F0 66 87 08", Code.Xchg_Ew_Gw, true };
				yield return new object[] { "F3 F0 87 08", Code.Xchg_Ed_Gd, true };

				yield return new object[] { "F3 F0 0F C7 08", Code.Cmpxchg8b_Mq, true };

				yield return new object[] { "F3 88 08", Code.Mov_Eb_Gb, false };
				yield return new object[] { "F3 66 89 08", Code.Mov_Ew_Gw, false };
				yield return new object[] { "F3 89 08", Code.Mov_Ed_Gd, false };

				yield return new object[] { "F3 C6 00 A5", Code.Mov_Eb_Ib, false };
				yield return new object[] { "F3 66 C7 00 A5FF", Code.Mov_Ew_Iw, false };
				yield return new object[] { "F3 C7 00 A5FFFFFF", Code.Mov_Ed_Id, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XreleasePrefix_Data))]
		void Test64_XreleasePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.Equal(hasLock, instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.True(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test64_XreleasePrefix_Data {
			get {
				yield return new object[] { "F3 F0 10 08", Code.Adc_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 10 A5", Code.Adc_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 11 08", Code.Adc_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 10 A5", Code.Adc_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 10 5AA5", Code.Adc_Ew_Iw, true };
				yield return new object[] { "F3 F0 11 08", Code.Adc_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 10 A5", Code.Adc_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 10 34 12 5AA5", Code.Adc_Ed_Id, true };
				yield return new object[] { "F3 F0 48 11 08", Code.Adc_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 83 10 A5", Code.Adc_Eq_Ib64, true };
				yield return new object[] { "F3 F0 48 81 10 34 12 5AA5", Code.Adc_Eq_Id64, true };

				yield return new object[] { "F3 F0 00 08", Code.Add_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 00 A5", Code.Add_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 01 08", Code.Add_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 00 A5", Code.Add_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 00 5AA5", Code.Add_Ew_Iw, true };
				yield return new object[] { "F3 F0 01 08", Code.Add_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 00 A5", Code.Add_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 00 34 12 5AA5", Code.Add_Ed_Id, true };
				yield return new object[] { "F3 F0 48 01 08", Code.Add_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 83 00 A5", Code.Add_Eq_Ib64, true };
				yield return new object[] { "F3 F0 48 81 00 34 12 5AA5", Code.Add_Eq_Id64, true };

				yield return new object[] { "F3 F0 20 08", Code.And_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 20 A5", Code.And_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 21 08", Code.And_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 20 A5", Code.And_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 20 5AA5", Code.And_Ew_Iw, true };
				yield return new object[] { "F3 F0 21 08", Code.And_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 20 A5", Code.And_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 20 34 12 5AA5", Code.And_Ed_Id, true };
				yield return new object[] { "F3 F0 48 21 08", Code.And_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 83 20 A5", Code.And_Eq_Ib64, true };
				yield return new object[] { "F3 F0 48 81 20 34 12 5AA5", Code.And_Eq_Id64, true };

				yield return new object[] { "F3 F0 08 08", Code.Or_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 08 A5", Code.Or_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 09 08", Code.Or_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 08 A5", Code.Or_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 08 5AA5", Code.Or_Ew_Iw, true };
				yield return new object[] { "F3 F0 09 08", Code.Or_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 08 A5", Code.Or_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 08 34 12 5AA5", Code.Or_Ed_Id, true };
				yield return new object[] { "F3 F0 48 09 08", Code.Or_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 83 08 A5", Code.Or_Eq_Ib64, true };
				yield return new object[] { "F3 F0 48 81 08 34 12 5AA5", Code.Or_Eq_Id64, true };

				yield return new object[] { "F3 F0 18 08", Code.Sbb_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 18 A5", Code.Sbb_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 19 08", Code.Sbb_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 18 A5", Code.Sbb_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 18 5AA5", Code.Sbb_Ew_Iw, true };
				yield return new object[] { "F3 F0 19 08", Code.Sbb_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 18 A5", Code.Sbb_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 18 34 12 5AA5", Code.Sbb_Ed_Id, true };
				yield return new object[] { "F3 F0 48 19 08", Code.Sbb_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 83 18 A5", Code.Sbb_Eq_Ib64, true };
				yield return new object[] { "F3 F0 48 81 18 34 12 5AA5", Code.Sbb_Eq_Id64, true };

				yield return new object[] { "F3 F0 28 08", Code.Sub_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 28 A5", Code.Sub_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 29 08", Code.Sub_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 28 A5", Code.Sub_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 28 5AA5", Code.Sub_Ew_Iw, true };
				yield return new object[] { "F3 F0 29 08", Code.Sub_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 28 A5", Code.Sub_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 28 34 12 5AA5", Code.Sub_Ed_Id, true };
				yield return new object[] { "F3 F0 48 29 08", Code.Sub_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 83 28 A5", Code.Sub_Eq_Ib64, true };
				yield return new object[] { "F3 F0 48 81 28 34 12 5AA5", Code.Sub_Eq_Id64, true };

				yield return new object[] { "F3 F0 30 08", Code.Xor_Eb_Gb, true };
				yield return new object[] { "F3 F0 80 30 A5", Code.Xor_Eb_Ib, true };
				yield return new object[] { "F3 F0 66 31 08", Code.Xor_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 83 30 A5", Code.Xor_Ew_Ib16, true };
				yield return new object[] { "F3 F0 66 81 30 5AA5", Code.Xor_Ew_Iw, true };
				yield return new object[] { "F3 F0 31 08", Code.Xor_Ed_Gd, true };
				yield return new object[] { "F3 F0 83 30 A5", Code.Xor_Ed_Ib32, true };
				yield return new object[] { "F3 F0 81 30 34 12 5AA5", Code.Xor_Ed_Id, true };
				yield return new object[] { "F3 F0 48 31 08", Code.Xor_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 83 30 A5", Code.Xor_Eq_Ib64, true };
				yield return new object[] { "F3 F0 48 81 30 34 12 5AA5", Code.Xor_Eq_Id64, true };

				yield return new object[] { "F3 F0 66 0F BB 08", Code.Btc_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F BA 38 A5", Code.Btc_Ew_Ib, true };
				yield return new object[] { "F3 F0 0F BB 08", Code.Btc_Ed_Gd, true };
				yield return new object[] { "F3 F0 0F BA 38 A5", Code.Btc_Ed_Ib, true };
				yield return new object[] { "F3 F0 48 0F BB 08", Code.Btc_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 0F BA 38 A5", Code.Btc_Eq_Ib, true };

				yield return new object[] { "F3 F0 66 0F B3 08", Code.Btr_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F BA 30 A5", Code.Btr_Ew_Ib, true };
				yield return new object[] { "F3 F0 0F B3 08", Code.Btr_Ed_Gd, true };
				yield return new object[] { "F3 F0 0F BA 30 A5", Code.Btr_Ed_Ib, true };
				yield return new object[] { "F3 F0 48 0F B3 08", Code.Btr_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 0F BA 30 A5", Code.Btr_Eq_Ib, true };

				yield return new object[] { "F3 F0 66 0F AB 08", Code.Bts_Ew_Gw, true };
				yield return new object[] { "F3 F0 66 0F BA 28 A5", Code.Bts_Ew_Ib, true };
				yield return new object[] { "F3 F0 0F AB 08", Code.Bts_Ed_Gd, true };
				yield return new object[] { "F3 F0 0F BA 28 A5", Code.Bts_Ed_Ib, true };
				yield return new object[] { "F3 F0 48 0F AB 08", Code.Bts_Eq_Gq, true };
				yield return new object[] { "F3 F0 48 0F BA 28 A5", Code.Bts_Eq_Ib, true };

				yield return new object[] { "F3 F0 FE 08", Code.Dec_Eb, true };
				yield return new object[] { "F3 F0 66 FF 08", Code.Dec_Ew, true };
				yield return new object[] { "F3 F0 FF 08", Code.Dec_Ed, true };
				yield return new object[] { "F3 F0 48 FF 08", Code.Dec_Eq, true };

				yield return new object[] { "F3 F0 FE 00", Code.Inc_Eb, true };
				yield return new object[] { "F3 F0 66 FF 00", Code.Inc_Ew, true };
				yield return new object[] { "F3 F0 FF 00", Code.Inc_Ed, true };
				yield return new object[] { "F3 F0 48 FF 00", Code.Inc_Eq, true };

				yield return new object[] { "F3 F0 F6 18", Code.Neg_Eb, true };
				yield return new object[] { "F3 F0 66 F7 18", Code.Neg_Ew, true };
				yield return new object[] { "F3 F0 F7 18", Code.Neg_Ed, true };
				yield return new object[] { "F3 F0 48 F7 18", Code.Neg_Eq, true };

				yield return new object[] { "F3 F0 F6 10", Code.Not_Eb, true };
				yield return new object[] { "F3 F0 66 F7 10", Code.Not_Ew, true };
				yield return new object[] { "F3 F0 F7 10", Code.Not_Ed, true };
				yield return new object[] { "F3 F0 48 F7 10", Code.Not_Eq, true };

				yield return new object[] { "F3 F0 0F B0 08", Code.Cmpxchg_Eb_Gb, true };
				yield return new object[] { "F3 F0 66 0F B1 08", Code.Cmpxchg_Ew_Gw, true };
				yield return new object[] { "F3 F0 0F B1 08", Code.Cmpxchg_Ed_Gd, true };
				yield return new object[] { "F3 F0 48 0F B1 08", Code.Cmpxchg_Eq_Gq, true };

				yield return new object[] { "F3 F0 0F C0 08", Code.Xadd_Eb_Gb, true };
				yield return new object[] { "F3 F0 66 0F C1 08", Code.Xadd_Ew_Gw, true };
				yield return new object[] { "F3 F0 0F C1 08", Code.Xadd_Ed_Gd, true };
				yield return new object[] { "F3 F0 48 0F C1 08", Code.Xadd_Eq_Gq, true };

				yield return new object[] { "F3 86 08", Code.Xchg_Eb_Gb, false };
				yield return new object[] { "F3 66 87 08", Code.Xchg_Ew_Gw, false };
				yield return new object[] { "F3 87 08", Code.Xchg_Ed_Gd, false };
				yield return new object[] { "F3 48 87 08", Code.Xchg_Eq_Gq, false };

				yield return new object[] { "F3 F0 86 08", Code.Xchg_Eb_Gb, true };
				yield return new object[] { "F3 F0 66 87 08", Code.Xchg_Ew_Gw, true };
				yield return new object[] { "F3 F0 87 08", Code.Xchg_Ed_Gd, true };
				yield return new object[] { "F3 F0 48 87 08", Code.Xchg_Eq_Gq, true };

				yield return new object[] { "F3 F0 0F C7 08", Code.Cmpxchg8b_Mq, true };

				yield return new object[] { "F3 88 08", Code.Mov_Eb_Gb, false };
				yield return new object[] { "F3 66 89 08", Code.Mov_Ew_Gw, false };
				yield return new object[] { "F3 89 08", Code.Mov_Ed_Gd, false };
				yield return new object[] { "F3 48 89 08", Code.Mov_Eq_Gq, false };

				yield return new object[] { "F3 C6 00 A5", Code.Mov_Eb_Ib, false };
				yield return new object[] { "F3 66 C7 00 A5FF", Code.Mov_Ew_Iw, false };
				yield return new object[] { "F3 C7 00 A5FFFFFF", Code.Mov_Ed_Id, false };
				yield return new object[] { "F3 48 C7 00 A5FFFFFF", Code.Mov_Eq_Id64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_RepPrefix_Data))]
		void Test16_RepPrefix(string hexBytes, Code code, bool repe, bool repne) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(repe, instr.HasPrefixRepe);
			Assert.Equal(repne, instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
		public static IEnumerable<object[]> Test16_RepPrefix_Data {
			get {
				yield return new object[] { "F3 6C", Code.Insb_Yb_DX, true, false };
				yield return new object[] { "F3 6D", Code.Insw_Yw_DX, true, false };
				yield return new object[] { "F3 66 6D", Code.Insd_Yd_DX, true, false };
				yield return new object[] { "F3 6E", Code.Outsb_DX_Xb, true, false };
				yield return new object[] { "F3 6F", Code.Outsw_DX_Xw, true, false };
				yield return new object[] { "F3 66 6F", Code.Outsd_DX_Xd, true, false };
				yield return new object[] { "F3 A4", Code.Movsb_Yb_Xb, true, false };
				yield return new object[] { "F3 A5", Code.Movsw_Yw_Xw, true, false };
				yield return new object[] { "F3 66 A5", Code.Movsd_Yd_Xd, true, false };
				yield return new object[] { "F3 A6", Code.Cmpsb_Xb_Yb, true, false };
				yield return new object[] { "F2 A6", Code.Cmpsb_Xb_Yb, false, true };
				yield return new object[] { "F3 A7", Code.Cmpsw_Xw_Yw, true, false };
				yield return new object[] { "F2 A7", Code.Cmpsw_Xw_Yw, false, true };
				yield return new object[] { "F3 66 A7", Code.Cmpsd_Xd_Yd, true, false };
				yield return new object[] { "F2 66 A7", Code.Cmpsd_Xd_Yd, false, true };
				yield return new object[] { "F3 AA", Code.Stosb_Yb_AL, true, false };
				yield return new object[] { "F3 AB", Code.Stosw_Yw_AX, true, false };
				yield return new object[] { "F3 66 AB", Code.Stosd_Yd_EAX, true, false };
				yield return new object[] { "F3 AC", Code.Lodsb_AL_Xb, true, false };
				yield return new object[] { "F3 AD", Code.Lodsw_AX_Xw, true, false };
				yield return new object[] { "F3 66 AD", Code.Lodsd_EAX_Xd, true, false };
				yield return new object[] { "F3 AE", Code.Scasb_AL_Yb, true, false };
				yield return new object[] { "F2 AE", Code.Scasb_AL_Yb, false, true };
				yield return new object[] { "F3 AF", Code.Scasw_AX_Yw, true, false };
				yield return new object[] { "F2 AF", Code.Scasw_AX_Yw, false, true };
				yield return new object[] { "F3 66 AF", Code.Scasd_EAX_Yd, true, false };
				yield return new object[] { "F2 66 AF", Code.Scasd_EAX_Yd, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_RepPrefix_Data))]
		void Test32_RepPrefix(string hexBytes, Code code, bool repe, bool repne) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(repe, instr.HasPrefixRepe);
			Assert.Equal(repne, instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
		public static IEnumerable<object[]> Test32_RepPrefix_Data {
			get {
				yield return new object[] { "F3 6C", Code.Insb_Yb_DX, true, false };
				yield return new object[] { "F3 66 6D", Code.Insw_Yw_DX, true, false };
				yield return new object[] { "F3 6D", Code.Insd_Yd_DX, true, false };
				yield return new object[] { "F3 6E", Code.Outsb_DX_Xb, true, false };
				yield return new object[] { "F3 66 6F", Code.Outsw_DX_Xw, true, false };
				yield return new object[] { "F3 6F", Code.Outsd_DX_Xd, true, false };
				yield return new object[] { "F3 A4", Code.Movsb_Yb_Xb, true, false };
				yield return new object[] { "F3 66 A5", Code.Movsw_Yw_Xw, true, false };
				yield return new object[] { "F3 A5", Code.Movsd_Yd_Xd, true, false };
				yield return new object[] { "F3 A6", Code.Cmpsb_Xb_Yb, true, false };
				yield return new object[] { "F2 A6", Code.Cmpsb_Xb_Yb, false, true };
				yield return new object[] { "F3 66 A7", Code.Cmpsw_Xw_Yw, true, false };
				yield return new object[] { "F2 66 A7", Code.Cmpsw_Xw_Yw, false, true };
				yield return new object[] { "F3 A7", Code.Cmpsd_Xd_Yd, true, false };
				yield return new object[] { "F2 A7", Code.Cmpsd_Xd_Yd, false, true };
				yield return new object[] { "F3 AA", Code.Stosb_Yb_AL, true, false };
				yield return new object[] { "F3 66 AB", Code.Stosw_Yw_AX, true, false };
				yield return new object[] { "F3 AB", Code.Stosd_Yd_EAX, true, false };
				yield return new object[] { "F3 AC", Code.Lodsb_AL_Xb, true, false };
				yield return new object[] { "F3 66 AD", Code.Lodsw_AX_Xw, true, false };
				yield return new object[] { "F3 AD", Code.Lodsd_EAX_Xd, true, false };
				yield return new object[] { "F3 AE", Code.Scasb_AL_Yb, true, false };
				yield return new object[] { "F2 AE", Code.Scasb_AL_Yb, false, true };
				yield return new object[] { "F3 66 AF", Code.Scasw_AX_Yw, true, false };
				yield return new object[] { "F2 66 AF", Code.Scasw_AX_Yw, false, true };
				yield return new object[] { "F3 AF", Code.Scasd_EAX_Yd, true, false };
				yield return new object[] { "F2 AF", Code.Scasd_EAX_Yd, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_RepPrefix_Data))]
		void Test64_RepPrefix(string hexBytes, Code code, bool repe, bool repne) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(repe, instr.HasPrefixRepe);
			Assert.Equal(repne, instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.False(instr.HasPrefixXacquire);
			Assert.False(instr.HasPrefixXrelease);
			Assert.Equal(Register.None, instr.PrefixSegment);
		}
		public static IEnumerable<object[]> Test64_RepPrefix_Data {
			get {
				yield return new object[] { "F3 6C", Code.Insb_Yb_DX, true, false };
				yield return new object[] { "F3 66 6D", Code.Insw_Yw_DX, true, false };
				yield return new object[] { "F3 6D", Code.Insd_Yd_DX, true, false };
				yield return new object[] { "F3 6E", Code.Outsb_DX_Xb, true, false };
				yield return new object[] { "F3 66 6F", Code.Outsw_DX_Xw, true, false };
				yield return new object[] { "F3 6F", Code.Outsd_DX_Xd, true, false };
				yield return new object[] { "F3 A4", Code.Movsb_Yb_Xb, true, false };
				yield return new object[] { "F3 66 A5", Code.Movsw_Yw_Xw, true, false };
				yield return new object[] { "F3 A5", Code.Movsd_Yd_Xd, true, false };
				yield return new object[] { "F3 48 A5", Code.Movsq_Yq_Xq, true, false };
				yield return new object[] { "F3 A6", Code.Cmpsb_Xb_Yb, true, false };
				yield return new object[] { "F2 A6", Code.Cmpsb_Xb_Yb, false, true };
				yield return new object[] { "F3 66 A7", Code.Cmpsw_Xw_Yw, true, false };
				yield return new object[] { "F2 66 A7", Code.Cmpsw_Xw_Yw, false, true };
				yield return new object[] { "F3 A7", Code.Cmpsd_Xd_Yd, true, false };
				yield return new object[] { "F2 A7", Code.Cmpsd_Xd_Yd, false, true };
				yield return new object[] { "F3 48 A7", Code.Cmpsq_Xq_Yq, true, false };
				yield return new object[] { "F2 48 A7", Code.Cmpsq_Xq_Yq, false, true };
				yield return new object[] { "F3 AA", Code.Stosb_Yb_AL, true, false };
				yield return new object[] { "F3 66 AB", Code.Stosw_Yw_AX, true, false };
				yield return new object[] { "F3 AB", Code.Stosd_Yd_EAX, true, false };
				yield return new object[] { "F3 48 AB", Code.Stosq_Yq_RAX, true, false };
				yield return new object[] { "F3 AC", Code.Lodsb_AL_Xb, true, false };
				yield return new object[] { "F3 66 AD", Code.Lodsw_AX_Xw, true, false };
				yield return new object[] { "F3 AD", Code.Lodsd_EAX_Xd, true, false };
				yield return new object[] { "F3 48 AD", Code.Lodsq_RAX_Xq, true, false };
				yield return new object[] { "F3 AE", Code.Scasb_AL_Yb, true, false };
				yield return new object[] { "F2 AE", Code.Scasb_AL_Yb, false, true };
				yield return new object[] { "F3 66 AF", Code.Scasw_AX_Yw, true, false };
				yield return new object[] { "F2 66 AF", Code.Scasw_AX_Yw, false, true };
				yield return new object[] { "F3 AF", Code.Scasd_EAX_Yd, true, false };
				yield return new object[] { "F2 AF", Code.Scasd_EAX_Yd, false, true };
				yield return new object[] { "F3 48 AF", Code.Scasq_RAX_Yq, true, false };
				yield return new object[] { "F2 48 AF", Code.Scasq_RAX_Yq, false, true };
			}
		}
	}
}
