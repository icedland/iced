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
		[InlineData("66 66 01 CE", 4, Code.Add_rm32_r32)]
		void Test16_double_66_is_same_as_one(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 66 01 CE", 4, Code.Add_rm16_r16)]
		void Test32_double_66_is_same_as_one(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 66 01 CE", 4, Code.Add_rm16_r16)]
		void Test64_double_66_is_same_as_one(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Theory]
		[InlineData("67 67 8D 18", 4, Code.Lea_r16_m)]
		void Test16_double_67_is_same_as_one(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 67 8D 18", 4, Code.Lea_r32_m)]
		void Test32_double_67_is_same_as_one(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("67 67 8D 18", 4, Code.Lea_r32_m)]
		void Test64_double_67_is_same_as_one(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
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
			Assert.Equal(MemorySize.Unknown, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("26 26 01 18", 4, Code.Add_rm16_r16, Register.ES)]
		[InlineData("26 2E 01 18", 4, Code.Add_rm16_r16, Register.CS)]
		[InlineData("26 36 01 18", 4, Code.Add_rm16_r16, Register.SS)]
		[InlineData("26 3E 01 18", 4, Code.Add_rm16_r16, Register.DS)]
		[InlineData("26 64 01 18", 4, Code.Add_rm16_r16, Register.FS)]
		[InlineData("26 65 01 18", 4, Code.Add_rm16_r16, Register.GS)]

		[InlineData("2E 26 01 18", 4, Code.Add_rm16_r16, Register.ES)]
		[InlineData("2E 2E 01 18", 4, Code.Add_rm16_r16, Register.CS)]
		[InlineData("2E 36 01 18", 4, Code.Add_rm16_r16, Register.SS)]
		[InlineData("2E 3E 01 18", 4, Code.Add_rm16_r16, Register.DS)]
		[InlineData("2E 64 01 18", 4, Code.Add_rm16_r16, Register.FS)]
		[InlineData("2E 65 01 18", 4, Code.Add_rm16_r16, Register.GS)]

		[InlineData("36 26 01 18", 4, Code.Add_rm16_r16, Register.ES)]
		[InlineData("36 2E 01 18", 4, Code.Add_rm16_r16, Register.CS)]
		[InlineData("36 36 01 18", 4, Code.Add_rm16_r16, Register.SS)]
		[InlineData("36 3E 01 18", 4, Code.Add_rm16_r16, Register.DS)]
		[InlineData("36 64 01 18", 4, Code.Add_rm16_r16, Register.FS)]
		[InlineData("36 65 01 18", 4, Code.Add_rm16_r16, Register.GS)]

		[InlineData("3E 26 01 18", 4, Code.Add_rm16_r16, Register.ES)]
		[InlineData("3E 2E 01 18", 4, Code.Add_rm16_r16, Register.CS)]
		[InlineData("3E 36 01 18", 4, Code.Add_rm16_r16, Register.SS)]
		[InlineData("3E 3E 01 18", 4, Code.Add_rm16_r16, Register.DS)]
		[InlineData("3E 64 01 18", 4, Code.Add_rm16_r16, Register.FS)]
		[InlineData("3E 65 01 18", 4, Code.Add_rm16_r16, Register.GS)]

		[InlineData("64 26 01 18", 4, Code.Add_rm16_r16, Register.ES)]
		[InlineData("64 2E 01 18", 4, Code.Add_rm16_r16, Register.CS)]
		[InlineData("64 36 01 18", 4, Code.Add_rm16_r16, Register.SS)]
		[InlineData("64 3E 01 18", 4, Code.Add_rm16_r16, Register.DS)]
		[InlineData("64 64 01 18", 4, Code.Add_rm16_r16, Register.FS)]
		[InlineData("64 65 01 18", 4, Code.Add_rm16_r16, Register.GS)]

		[InlineData("65 26 01 18", 4, Code.Add_rm16_r16, Register.ES)]
		[InlineData("65 2E 01 18", 4, Code.Add_rm16_r16, Register.CS)]
		[InlineData("65 36 01 18", 4, Code.Add_rm16_r16, Register.SS)]
		[InlineData("65 3E 01 18", 4, Code.Add_rm16_r16, Register.DS)]
		[InlineData("65 64 01 18", 4, Code.Add_rm16_r16, Register.FS)]
		[InlineData("65 65 01 18", 4, Code.Add_rm16_r16, Register.GS)]
		void Test16_extra_segment_overrides(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(segReg, instr.SegmentPrefix);

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
		[InlineData("26 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("26 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("26 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("26 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("26 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("26 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("2E 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("2E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("2E 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("2E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("2E 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("2E 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("36 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("36 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("36 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("36 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("36 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("36 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("3E 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("3E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("3E 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("3E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("3E 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("3E 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("64 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("64 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("64 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("64 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("64 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("64 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("65 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("65 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("65 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("65 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("65 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("65 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]
		void Test32_extra_segment_overrides(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(segReg, instr.SegmentPrefix);

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
		[InlineData("26 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("26 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("26 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("26 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("26 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("26 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("2E 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("2E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("2E 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("2E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("2E 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("2E 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("36 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("36 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("36 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("36 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("36 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("36 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("3E 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("3E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("3E 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("3E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("3E 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("3E 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("64 26 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("64 2E 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("64 36 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("64 3E 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("64 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("64 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]

		[InlineData("65 26 01 18", 4, Code.Add_rm32_r32, Register.GS)]
		[InlineData("65 2E 01 18", 4, Code.Add_rm32_r32, Register.GS)]
		[InlineData("65 36 01 18", 4, Code.Add_rm32_r32, Register.GS)]
		[InlineData("65 3E 01 18", 4, Code.Add_rm32_r32, Register.GS)]
		[InlineData("65 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("65 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]
		void Test64_extra_segment_overrides(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(segReg, instr.SegmentPrefix);

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
		[InlineData("66 48 01 CE", 4, Code.Add_rm64_r64)]
		void Test64_REX_W_overrides_66(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RSI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RCX, instr.Op1Register);
		}

		[Theory]
		[InlineData("4F 48 01 CE", 4, Code.Add_rm64_r64, Register.RSI, Register.RCX)]
		[InlineData("4F 4C 01 C5", 4, Code.Add_rm64_r64, Register.RBP, Register.R8)]
		[InlineData("4F 49 01 D6", 4, Code.Add_rm64_r64, Register.R14, Register.RDX)]
		[InlineData("4F 4D 01 D0", 4, Code.Add_rm64_r64, Register.R8, Register.R10)]
		[InlineData("4F 49 01 D9", 4, Code.Add_rm64_r64, Register.R9, Register.RBX)]
		[InlineData("4F 4C 01 EC", 4, Code.Add_rm64_r64, Register.RSP, Register.R13)]
		void Test64_double_REX_prefixes(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
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

		[Theory]
		[InlineData("48 66 01 CE", 4, Code.Add_rm16_r16)]
		[InlineData("4F 66 01 CE", 4, Code.Add_rm16_r16)]
		void Test64_REX_prefix_before_66(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 67 01 18", 4, Code.Add_rm32_r32)]
		void Test64_REX_before_67(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
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
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("49 F0 01 18", 4, Code.Add_rm32_r32)]
		void Test64_REX_before_F0(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.True(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("4D F2 01 18", 4, Code.Add_rm32_r32)]
		void Test64_REX_before_F2(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.True(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("4F F3 01 18", 4, Code.Add_rm32_r32)]
		void Test64_REX_before_F3(string hexBytes, int byteLength, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.True(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("48 26 01 18", 4, Code.Add_rm32_r32, Register.ES)]
		[InlineData("49 2E 01 18", 4, Code.Add_rm32_r32, Register.CS)]
		[InlineData("4A 36 01 18", 4, Code.Add_rm32_r32, Register.SS)]
		[InlineData("4B 3E 01 18", 4, Code.Add_rm32_r32, Register.DS)]
		[InlineData("4C 64 01 18", 4, Code.Add_rm32_r32, Register.FS)]
		[InlineData("4F 65 01 18", 4, Code.Add_rm32_r32, Register.GS)]
		void Test64_REX_before_segment_override(string hexBytes, int byteLength, Code code, Register segReg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(segReg, instr.SegmentPrefix);

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.True(instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test16_LockPrefix_Data {
			get {
				yield return new object[] { "F0 10 08", Code.Adc_rm8_r8 };
				yield return new object[] { "F0 80 10 A5", Code.Adc_rm8_imm8 };
				yield return new object[] { "F0 82 10 A5", Code.Adc_rm8_imm8_82 };
				yield return new object[] { "F0 11 08", Code.Adc_rm16_r16 };
				yield return new object[] { "F0 83 10 A5", Code.Adc_rm16_imm8 };
				yield return new object[] { "F0 81 10 5AA5", Code.Adc_rm16_imm16 };
				yield return new object[] { "F0 66 11 08", Code.Adc_rm32_r32 };
				yield return new object[] { "F0 66 83 10 A5", Code.Adc_rm32_imm8 };
				yield return new object[] { "F0 66 81 10 34 12 5AA5", Code.Adc_rm32_imm32 };

				yield return new object[] { "F0 00 08", Code.Add_rm8_r8 };
				yield return new object[] { "F0 80 00 A5", Code.Add_rm8_imm8 };
				yield return new object[] { "F0 82 00 A5", Code.Add_rm8_imm8_82 };
				yield return new object[] { "F0 01 08", Code.Add_rm16_r16 };
				yield return new object[] { "F0 83 00 A5", Code.Add_rm16_imm8 };
				yield return new object[] { "F0 81 00 5AA5", Code.Add_rm16_imm16 };
				yield return new object[] { "F0 66 01 08", Code.Add_rm32_r32 };
				yield return new object[] { "F0 66 83 00 A5", Code.Add_rm32_imm8 };
				yield return new object[] { "F0 66 81 00 34 12 5AA5", Code.Add_rm32_imm32 };

				yield return new object[] { "F0 20 08", Code.And_rm8_r8 };
				yield return new object[] { "F0 80 20 A5", Code.And_rm8_imm8 };
				yield return new object[] { "F0 82 20 A5", Code.And_rm8_imm8_82 };
				yield return new object[] { "F0 21 08", Code.And_rm16_r16 };
				yield return new object[] { "F0 83 20 A5", Code.And_rm16_imm8 };
				yield return new object[] { "F0 81 20 5AA5", Code.And_rm16_imm16 };
				yield return new object[] { "F0 66 21 08", Code.And_rm32_r32 };
				yield return new object[] { "F0 66 83 20 A5", Code.And_rm32_imm8 };
				yield return new object[] { "F0 66 81 20 34 12 5AA5", Code.And_rm32_imm32 };

				yield return new object[] { "F0 08 08", Code.Or_rm8_r8 };
				yield return new object[] { "F0 80 08 A5", Code.Or_rm8_imm8 };
				yield return new object[] { "F0 82 08 A5", Code.Or_rm8_imm8_82 };
				yield return new object[] { "F0 09 08", Code.Or_rm16_r16 };
				yield return new object[] { "F0 83 08 A5", Code.Or_rm16_imm8 };
				yield return new object[] { "F0 81 08 5AA5", Code.Or_rm16_imm16 };
				yield return new object[] { "F0 66 09 08", Code.Or_rm32_r32 };
				yield return new object[] { "F0 66 83 08 A5", Code.Or_rm32_imm8 };
				yield return new object[] { "F0 66 81 08 34 12 5AA5", Code.Or_rm32_imm32 };

				yield return new object[] { "F0 18 08", Code.Sbb_rm8_r8 };
				yield return new object[] { "F0 80 18 A5", Code.Sbb_rm8_imm8 };
				yield return new object[] { "F0 82 18 A5", Code.Sbb_rm8_imm8_82 };
				yield return new object[] { "F0 19 08", Code.Sbb_rm16_r16 };
				yield return new object[] { "F0 83 18 A5", Code.Sbb_rm16_imm8 };
				yield return new object[] { "F0 81 18 5AA5", Code.Sbb_rm16_imm16 };
				yield return new object[] { "F0 66 19 08", Code.Sbb_rm32_r32 };
				yield return new object[] { "F0 66 83 18 A5", Code.Sbb_rm32_imm8 };
				yield return new object[] { "F0 66 81 18 34 12 5AA5", Code.Sbb_rm32_imm32 };

				yield return new object[] { "F0 28 08", Code.Sub_rm8_r8 };
				yield return new object[] { "F0 80 28 A5", Code.Sub_rm8_imm8 };
				yield return new object[] { "F0 82 28 A5", Code.Sub_rm8_imm8_82 };
				yield return new object[] { "F0 29 08", Code.Sub_rm16_r16 };
				yield return new object[] { "F0 83 28 A5", Code.Sub_rm16_imm8 };
				yield return new object[] { "F0 81 28 5AA5", Code.Sub_rm16_imm16 };
				yield return new object[] { "F0 66 29 08", Code.Sub_rm32_r32 };
				yield return new object[] { "F0 66 83 28 A5", Code.Sub_rm32_imm8 };
				yield return new object[] { "F0 66 81 28 34 12 5AA5", Code.Sub_rm32_imm32 };

				yield return new object[] { "F0 30 08", Code.Xor_rm8_r8 };
				yield return new object[] { "F0 80 30 A5", Code.Xor_rm8_imm8 };
				yield return new object[] { "F0 82 30 A5", Code.Xor_rm8_imm8_82 };
				yield return new object[] { "F0 31 08", Code.Xor_rm16_r16 };
				yield return new object[] { "F0 83 30 A5", Code.Xor_rm16_imm8 };
				yield return new object[] { "F0 81 30 5AA5", Code.Xor_rm16_imm16 };
				yield return new object[] { "F0 66 31 08", Code.Xor_rm32_r32 };
				yield return new object[] { "F0 66 83 30 A5", Code.Xor_rm32_imm8 };
				yield return new object[] { "F0 66 81 30 34 12 5AA5", Code.Xor_rm32_imm32 };

				yield return new object[] { "F0 0F BB 08", Code.Btc_rm16_r16 };
				yield return new object[] { "F0 0F BA 38 A5", Code.Btc_rm16_imm8 };
				yield return new object[] { "F0 66 0F BB 08", Code.Btc_rm32_r32 };
				yield return new object[] { "F0 66 0F BA 38 A5", Code.Btc_rm32_imm8 };

				yield return new object[] { "F0 0F B3 08", Code.Btr_rm16_r16 };
				yield return new object[] { "F0 0F BA 30 A5", Code.Btr_rm16_imm8 };
				yield return new object[] { "F0 66 0F B3 08", Code.Btr_rm32_r32 };
				yield return new object[] { "F0 66 0F BA 30 A5", Code.Btr_rm32_imm8 };

				yield return new object[] { "F0 0F AB 08", Code.Bts_rm16_r16 };
				yield return new object[] { "F0 0F BA 28 A5", Code.Bts_rm16_imm8 };
				yield return new object[] { "F0 66 0F AB 08", Code.Bts_rm32_r32 };
				yield return new object[] { "F0 66 0F BA 28 A5", Code.Bts_rm32_imm8 };

				yield return new object[] { "F0 FE 08", Code.Dec_rm8 };
				yield return new object[] { "F0 FF 08", Code.Dec_rm16 };
				yield return new object[] { "F0 66 FF 08", Code.Dec_rm32 };

				yield return new object[] { "F0 FE 00", Code.Inc_rm8 };
				yield return new object[] { "F0 FF 00", Code.Inc_rm16 };
				yield return new object[] { "F0 66 FF 00", Code.Inc_rm32 };

				yield return new object[] { "F0 F6 18", Code.Neg_rm8 };
				yield return new object[] { "F0 F7 18", Code.Neg_rm16 };
				yield return new object[] { "F0 66 F7 18", Code.Neg_rm32 };

				yield return new object[] { "F0 F6 10", Code.Not_rm8 };
				yield return new object[] { "F0 F7 10", Code.Not_rm16 };
				yield return new object[] { "F0 66 F7 10", Code.Not_rm32 };

				yield return new object[] { "F0 0F B0 08", Code.Cmpxchg_rm8_r8 };
				yield return new object[] { "F0 0F B1 08", Code.Cmpxchg_rm16_r16 };
				yield return new object[] { "F0 66 0F B1 08", Code.Cmpxchg_rm32_r32 };

				yield return new object[] { "F0 0F C0 08", Code.Xadd_rm8_r8 };
				yield return new object[] { "F0 0F C1 08", Code.Xadd_rm16_r16 };
				yield return new object[] { "F0 66 0F C1 08", Code.Xadd_rm32_r32 };

				yield return new object[] { "F0 86 08", Code.Xchg_rm8_r8 };
				yield return new object[] { "F0 87 08", Code.Xchg_rm16_r16 };
				yield return new object[] { "F0 66 87 08", Code.Xchg_rm32_r32 };

				yield return new object[] { "F0 0F C7 08", Code.Cmpxchg8b_m64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_LockPrefix_Data))]
		void Test32_LockPrefix(string hexBytes, Code code) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.True(instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test32_LockPrefix_Data {
			get {
				yield return new object[] { "F0 10 08", Code.Adc_rm8_r8 };
				yield return new object[] { "F0 80 10 A5", Code.Adc_rm8_imm8 };
				yield return new object[] { "F0 82 10 A5", Code.Adc_rm8_imm8_82 };
				yield return new object[] { "F0 66 11 08", Code.Adc_rm16_r16 };
				yield return new object[] { "F0 66 83 10 A5", Code.Adc_rm16_imm8 };
				yield return new object[] { "F0 66 81 10 5AA5", Code.Adc_rm16_imm16 };
				yield return new object[] { "F0 11 08", Code.Adc_rm32_r32 };
				yield return new object[] { "F0 83 10 A5", Code.Adc_rm32_imm8 };
				yield return new object[] { "F0 81 10 34 12 5AA5", Code.Adc_rm32_imm32 };

				yield return new object[] { "F0 00 08", Code.Add_rm8_r8 };
				yield return new object[] { "F0 80 00 A5", Code.Add_rm8_imm8 };
				yield return new object[] { "F0 82 00 A5", Code.Add_rm8_imm8_82 };
				yield return new object[] { "F0 66 01 08", Code.Add_rm16_r16 };
				yield return new object[] { "F0 66 83 00 A5", Code.Add_rm16_imm8 };
				yield return new object[] { "F0 66 81 00 5AA5", Code.Add_rm16_imm16 };
				yield return new object[] { "F0 01 08", Code.Add_rm32_r32 };
				yield return new object[] { "F0 83 00 A5", Code.Add_rm32_imm8 };
				yield return new object[] { "F0 81 00 34 12 5AA5", Code.Add_rm32_imm32 };

				yield return new object[] { "F0 20 08", Code.And_rm8_r8 };
				yield return new object[] { "F0 80 20 A5", Code.And_rm8_imm8 };
				yield return new object[] { "F0 82 20 A5", Code.And_rm8_imm8_82 };
				yield return new object[] { "F0 66 21 08", Code.And_rm16_r16 };
				yield return new object[] { "F0 66 83 20 A5", Code.And_rm16_imm8 };
				yield return new object[] { "F0 66 81 20 5AA5", Code.And_rm16_imm16 };
				yield return new object[] { "F0 21 08", Code.And_rm32_r32 };
				yield return new object[] { "F0 83 20 A5", Code.And_rm32_imm8 };
				yield return new object[] { "F0 81 20 34 12 5AA5", Code.And_rm32_imm32 };

				yield return new object[] { "F0 08 08", Code.Or_rm8_r8 };
				yield return new object[] { "F0 80 08 A5", Code.Or_rm8_imm8 };
				yield return new object[] { "F0 82 08 A5", Code.Or_rm8_imm8_82 };
				yield return new object[] { "F0 66 09 08", Code.Or_rm16_r16 };
				yield return new object[] { "F0 66 83 08 A5", Code.Or_rm16_imm8 };
				yield return new object[] { "F0 66 81 08 5AA5", Code.Or_rm16_imm16 };
				yield return new object[] { "F0 09 08", Code.Or_rm32_r32 };
				yield return new object[] { "F0 83 08 A5", Code.Or_rm32_imm8 };
				yield return new object[] { "F0 81 08 34 12 5AA5", Code.Or_rm32_imm32 };

				yield return new object[] { "F0 18 08", Code.Sbb_rm8_r8 };
				yield return new object[] { "F0 80 18 A5", Code.Sbb_rm8_imm8 };
				yield return new object[] { "F0 82 18 A5", Code.Sbb_rm8_imm8_82 };
				yield return new object[] { "F0 66 19 08", Code.Sbb_rm16_r16 };
				yield return new object[] { "F0 66 83 18 A5", Code.Sbb_rm16_imm8 };
				yield return new object[] { "F0 66 81 18 5AA5", Code.Sbb_rm16_imm16 };
				yield return new object[] { "F0 19 08", Code.Sbb_rm32_r32 };
				yield return new object[] { "F0 83 18 A5", Code.Sbb_rm32_imm8 };
				yield return new object[] { "F0 81 18 34 12 5AA5", Code.Sbb_rm32_imm32 };

				yield return new object[] { "F0 28 08", Code.Sub_rm8_r8 };
				yield return new object[] { "F0 80 28 A5", Code.Sub_rm8_imm8 };
				yield return new object[] { "F0 82 28 A5", Code.Sub_rm8_imm8_82 };
				yield return new object[] { "F0 66 29 08", Code.Sub_rm16_r16 };
				yield return new object[] { "F0 66 83 28 A5", Code.Sub_rm16_imm8 };
				yield return new object[] { "F0 66 81 28 5AA5", Code.Sub_rm16_imm16 };
				yield return new object[] { "F0 29 08", Code.Sub_rm32_r32 };
				yield return new object[] { "F0 83 28 A5", Code.Sub_rm32_imm8 };
				yield return new object[] { "F0 81 28 34 12 5AA5", Code.Sub_rm32_imm32 };

				yield return new object[] { "F0 30 08", Code.Xor_rm8_r8 };
				yield return new object[] { "F0 80 30 A5", Code.Xor_rm8_imm8 };
				yield return new object[] { "F0 82 30 A5", Code.Xor_rm8_imm8_82 };
				yield return new object[] { "F0 66 31 08", Code.Xor_rm16_r16 };
				yield return new object[] { "F0 66 83 30 A5", Code.Xor_rm16_imm8 };
				yield return new object[] { "F0 66 81 30 5AA5", Code.Xor_rm16_imm16 };
				yield return new object[] { "F0 31 08", Code.Xor_rm32_r32 };
				yield return new object[] { "F0 83 30 A5", Code.Xor_rm32_imm8 };
				yield return new object[] { "F0 81 30 34 12 5AA5", Code.Xor_rm32_imm32 };

				yield return new object[] { "F0 66 0F BB 08", Code.Btc_rm16_r16 };
				yield return new object[] { "F0 66 0F BA 38 A5", Code.Btc_rm16_imm8 };
				yield return new object[] { "F0 0F BB 08", Code.Btc_rm32_r32 };
				yield return new object[] { "F0 0F BA 38 A5", Code.Btc_rm32_imm8 };

				yield return new object[] { "F0 66 0F B3 08", Code.Btr_rm16_r16 };
				yield return new object[] { "F0 66 0F BA 30 A5", Code.Btr_rm16_imm8 };
				yield return new object[] { "F0 0F B3 08", Code.Btr_rm32_r32 };
				yield return new object[] { "F0 0F BA 30 A5", Code.Btr_rm32_imm8 };

				yield return new object[] { "F0 66 0F AB 08", Code.Bts_rm16_r16 };
				yield return new object[] { "F0 66 0F BA 28 A5", Code.Bts_rm16_imm8 };
				yield return new object[] { "F0 0F AB 08", Code.Bts_rm32_r32 };
				yield return new object[] { "F0 0F BA 28 A5", Code.Bts_rm32_imm8 };

				yield return new object[] { "F0 FE 08", Code.Dec_rm8 };
				yield return new object[] { "F0 66 FF 08", Code.Dec_rm16 };
				yield return new object[] { "F0 FF 08", Code.Dec_rm32 };

				yield return new object[] { "F0 FE 00", Code.Inc_rm8 };
				yield return new object[] { "F0 66 FF 00", Code.Inc_rm16 };
				yield return new object[] { "F0 FF 00", Code.Inc_rm32 };

				yield return new object[] { "F0 F6 18", Code.Neg_rm8 };
				yield return new object[] { "F0 66 F7 18", Code.Neg_rm16 };
				yield return new object[] { "F0 F7 18", Code.Neg_rm32 };

				yield return new object[] { "F0 F6 10", Code.Not_rm8 };
				yield return new object[] { "F0 66 F7 10", Code.Not_rm16 };
				yield return new object[] { "F0 F7 10", Code.Not_rm32 };

				yield return new object[] { "F0 0F B0 08", Code.Cmpxchg_rm8_r8 };
				yield return new object[] { "F0 66 0F B1 08", Code.Cmpxchg_rm16_r16 };
				yield return new object[] { "F0 0F B1 08", Code.Cmpxchg_rm32_r32 };

				yield return new object[] { "F0 0F C0 08", Code.Xadd_rm8_r8 };
				yield return new object[] { "F0 66 0F C1 08", Code.Xadd_rm16_r16 };
				yield return new object[] { "F0 0F C1 08", Code.Xadd_rm32_r32 };

				yield return new object[] { "F0 86 08", Code.Xchg_rm8_r8 };
				yield return new object[] { "F0 66 87 08", Code.Xchg_rm16_r16 };
				yield return new object[] { "F0 87 08", Code.Xchg_rm32_r32 };

				yield return new object[] { "F0 0F C7 08", Code.Cmpxchg8b_m64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_LockPrefix_Data))]
		void Test64_LockPrefix(string hexBytes, Code code) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.True(instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test64_LockPrefix_Data {
			get {
				yield return new object[] { "F0 10 08", Code.Adc_rm8_r8 };
				yield return new object[] { "F0 80 10 A5", Code.Adc_rm8_imm8 };
				yield return new object[] { "F0 66 11 08", Code.Adc_rm16_r16 };
				yield return new object[] { "F0 66 83 10 A5", Code.Adc_rm16_imm8 };
				yield return new object[] { "F0 66 81 10 5AA5", Code.Adc_rm16_imm16 };
				yield return new object[] { "F0 11 08", Code.Adc_rm32_r32 };
				yield return new object[] { "F0 83 10 A5", Code.Adc_rm32_imm8 };
				yield return new object[] { "F0 81 10 34 12 5AA5", Code.Adc_rm32_imm32 };
				yield return new object[] { "F0 48 11 08", Code.Adc_rm64_r64 };
				yield return new object[] { "F0 48 83 10 A5", Code.Adc_rm64_imm8 };
				yield return new object[] { "F0 48 81 10 34 12 5AA5", Code.Adc_rm64_imm32 };

				yield return new object[] { "F0 00 08", Code.Add_rm8_r8 };
				yield return new object[] { "F0 80 00 A5", Code.Add_rm8_imm8 };
				yield return new object[] { "F0 66 01 08", Code.Add_rm16_r16 };
				yield return new object[] { "F0 66 83 00 A5", Code.Add_rm16_imm8 };
				yield return new object[] { "F0 66 81 00 5AA5", Code.Add_rm16_imm16 };
				yield return new object[] { "F0 01 08", Code.Add_rm32_r32 };
				yield return new object[] { "F0 83 00 A5", Code.Add_rm32_imm8 };
				yield return new object[] { "F0 81 00 34 12 5AA5", Code.Add_rm32_imm32 };
				yield return new object[] { "F0 48 01 08", Code.Add_rm64_r64 };
				yield return new object[] { "F0 48 83 00 A5", Code.Add_rm64_imm8 };
				yield return new object[] { "F0 48 81 00 34 12 5AA5", Code.Add_rm64_imm32 };

				yield return new object[] { "F0 20 08", Code.And_rm8_r8 };
				yield return new object[] { "F0 80 20 A5", Code.And_rm8_imm8 };
				yield return new object[] { "F0 66 21 08", Code.And_rm16_r16 };
				yield return new object[] { "F0 66 83 20 A5", Code.And_rm16_imm8 };
				yield return new object[] { "F0 66 81 20 5AA5", Code.And_rm16_imm16 };
				yield return new object[] { "F0 21 08", Code.And_rm32_r32 };
				yield return new object[] { "F0 83 20 A5", Code.And_rm32_imm8 };
				yield return new object[] { "F0 81 20 34 12 5AA5", Code.And_rm32_imm32 };
				yield return new object[] { "F0 48 21 08", Code.And_rm64_r64 };
				yield return new object[] { "F0 48 83 20 A5", Code.And_rm64_imm8 };
				yield return new object[] { "F0 48 81 20 34 12 5AA5", Code.And_rm64_imm32 };

				yield return new object[] { "F0 08 08", Code.Or_rm8_r8 };
				yield return new object[] { "F0 80 08 A5", Code.Or_rm8_imm8 };
				yield return new object[] { "F0 66 09 08", Code.Or_rm16_r16 };
				yield return new object[] { "F0 66 83 08 A5", Code.Or_rm16_imm8 };
				yield return new object[] { "F0 66 81 08 5AA5", Code.Or_rm16_imm16 };
				yield return new object[] { "F0 09 08", Code.Or_rm32_r32 };
				yield return new object[] { "F0 83 08 A5", Code.Or_rm32_imm8 };
				yield return new object[] { "F0 81 08 34 12 5AA5", Code.Or_rm32_imm32 };
				yield return new object[] { "F0 48 09 08", Code.Or_rm64_r64 };
				yield return new object[] { "F0 48 83 08 A5", Code.Or_rm64_imm8 };
				yield return new object[] { "F0 48 81 08 34 12 5AA5", Code.Or_rm64_imm32 };

				yield return new object[] { "F0 18 08", Code.Sbb_rm8_r8 };
				yield return new object[] { "F0 80 18 A5", Code.Sbb_rm8_imm8 };
				yield return new object[] { "F0 66 19 08", Code.Sbb_rm16_r16 };
				yield return new object[] { "F0 66 83 18 A5", Code.Sbb_rm16_imm8 };
				yield return new object[] { "F0 66 81 18 5AA5", Code.Sbb_rm16_imm16 };
				yield return new object[] { "F0 19 08", Code.Sbb_rm32_r32 };
				yield return new object[] { "F0 83 18 A5", Code.Sbb_rm32_imm8 };
				yield return new object[] { "F0 81 18 34 12 5AA5", Code.Sbb_rm32_imm32 };
				yield return new object[] { "F0 48 19 08", Code.Sbb_rm64_r64 };
				yield return new object[] { "F0 48 83 18 A5", Code.Sbb_rm64_imm8 };
				yield return new object[] { "F0 48 81 18 34 12 5AA5", Code.Sbb_rm64_imm32 };

				yield return new object[] { "F0 28 08", Code.Sub_rm8_r8 };
				yield return new object[] { "F0 80 28 A5", Code.Sub_rm8_imm8 };
				yield return new object[] { "F0 66 29 08", Code.Sub_rm16_r16 };
				yield return new object[] { "F0 66 83 28 A5", Code.Sub_rm16_imm8 };
				yield return new object[] { "F0 66 81 28 5AA5", Code.Sub_rm16_imm16 };
				yield return new object[] { "F0 29 08", Code.Sub_rm32_r32 };
				yield return new object[] { "F0 83 28 A5", Code.Sub_rm32_imm8 };
				yield return new object[] { "F0 81 28 34 12 5AA5", Code.Sub_rm32_imm32 };
				yield return new object[] { "F0 48 29 08", Code.Sub_rm64_r64 };
				yield return new object[] { "F0 48 83 28 A5", Code.Sub_rm64_imm8 };
				yield return new object[] { "F0 48 81 28 34 12 5AA5", Code.Sub_rm64_imm32 };

				yield return new object[] { "F0 30 08", Code.Xor_rm8_r8 };
				yield return new object[] { "F0 80 30 A5", Code.Xor_rm8_imm8 };
				yield return new object[] { "F0 66 31 08", Code.Xor_rm16_r16 };
				yield return new object[] { "F0 66 83 30 A5", Code.Xor_rm16_imm8 };
				yield return new object[] { "F0 66 81 30 5AA5", Code.Xor_rm16_imm16 };
				yield return new object[] { "F0 31 08", Code.Xor_rm32_r32 };
				yield return new object[] { "F0 83 30 A5", Code.Xor_rm32_imm8 };
				yield return new object[] { "F0 81 30 34 12 5AA5", Code.Xor_rm32_imm32 };
				yield return new object[] { "F0 48 31 08", Code.Xor_rm64_r64 };
				yield return new object[] { "F0 48 83 30 A5", Code.Xor_rm64_imm8 };
				yield return new object[] { "F0 48 81 30 34 12 5AA5", Code.Xor_rm64_imm32 };

				yield return new object[] { "F0 66 0F BB 08", Code.Btc_rm16_r16 };
				yield return new object[] { "F0 66 0F BA 38 A5", Code.Btc_rm16_imm8 };
				yield return new object[] { "F0 0F BB 08", Code.Btc_rm32_r32 };
				yield return new object[] { "F0 0F BA 38 A5", Code.Btc_rm32_imm8 };
				yield return new object[] { "F0 48 0F BB 08", Code.Btc_rm64_r64 };
				yield return new object[] { "F0 48 0F BA 38 A5", Code.Btc_rm64_imm8 };

				yield return new object[] { "F0 66 0F B3 08", Code.Btr_rm16_r16 };
				yield return new object[] { "F0 66 0F BA 30 A5", Code.Btr_rm16_imm8 };
				yield return new object[] { "F0 0F B3 08", Code.Btr_rm32_r32 };
				yield return new object[] { "F0 0F BA 30 A5", Code.Btr_rm32_imm8 };
				yield return new object[] { "F0 48 0F B3 08", Code.Btr_rm64_r64 };
				yield return new object[] { "F0 48 0F BA 30 A5", Code.Btr_rm64_imm8 };

				yield return new object[] { "F0 66 0F AB 08", Code.Bts_rm16_r16 };
				yield return new object[] { "F0 66 0F BA 28 A5", Code.Bts_rm16_imm8 };
				yield return new object[] { "F0 0F AB 08", Code.Bts_rm32_r32 };
				yield return new object[] { "F0 0F BA 28 A5", Code.Bts_rm32_imm8 };
				yield return new object[] { "F0 48 0F AB 08", Code.Bts_rm64_r64 };
				yield return new object[] { "F0 48 0F BA 28 A5", Code.Bts_rm64_imm8 };

				yield return new object[] { "F0 FE 08", Code.Dec_rm8 };
				yield return new object[] { "F0 66 FF 08", Code.Dec_rm16 };
				yield return new object[] { "F0 FF 08", Code.Dec_rm32 };
				yield return new object[] { "F0 48 FF 08", Code.Dec_rm64 };

				yield return new object[] { "F0 FE 00", Code.Inc_rm8 };
				yield return new object[] { "F0 66 FF 00", Code.Inc_rm16 };
				yield return new object[] { "F0 FF 00", Code.Inc_rm32 };
				yield return new object[] { "F0 48 FF 00", Code.Inc_rm64 };

				yield return new object[] { "F0 F6 18", Code.Neg_rm8 };
				yield return new object[] { "F0 66 F7 18", Code.Neg_rm16 };
				yield return new object[] { "F0 F7 18", Code.Neg_rm32 };
				yield return new object[] { "F0 48 F7 18", Code.Neg_rm64 };

				yield return new object[] { "F0 F6 10", Code.Not_rm8 };
				yield return new object[] { "F0 66 F7 10", Code.Not_rm16 };
				yield return new object[] { "F0 F7 10", Code.Not_rm32 };
				yield return new object[] { "F0 48 F7 10", Code.Not_rm64 };

				yield return new object[] { "F0 0F B0 08", Code.Cmpxchg_rm8_r8 };
				yield return new object[] { "F0 66 0F B1 08", Code.Cmpxchg_rm16_r16 };
				yield return new object[] { "F0 0F B1 08", Code.Cmpxchg_rm32_r32 };
				yield return new object[] { "F0 48 0F B1 08", Code.Cmpxchg_rm64_r64 };

				yield return new object[] { "F0 0F C0 08", Code.Xadd_rm8_r8 };
				yield return new object[] { "F0 66 0F C1 08", Code.Xadd_rm16_r16 };
				yield return new object[] { "F0 0F C1 08", Code.Xadd_rm32_r32 };
				yield return new object[] { "F0 48 0F C1 08", Code.Xadd_rm64_r64 };

				yield return new object[] { "F0 86 08", Code.Xchg_rm8_r8 };
				yield return new object[] { "F0 66 87 08", Code.Xchg_rm16_r16 };
				yield return new object[] { "F0 87 08", Code.Xchg_rm32_r32 };
				yield return new object[] { "F0 48 87 08", Code.Xchg_rm64_r64 };

				yield return new object[] { "F0 0F C7 08", Code.Cmpxchg8b_m64 };
				yield return new object[] { "F0 48 0F C7 08", Code.Cmpxchg16b_m128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XacquirePrefix_Data))]
		void Test16_XacquirePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.Equal(hasLock, instr.HasLockPrefix);
			Assert.True(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test16_XacquirePrefix_Data {
			get {
				yield return new object[] { "F2 F0 10 08", Code.Adc_rm8_r8, true };
				yield return new object[] { "F2 F0 80 10 A5", Code.Adc_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 10 A5", Code.Adc_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 11 08", Code.Adc_rm16_r16, true };
				yield return new object[] { "F2 F0 83 10 A5", Code.Adc_rm16_imm8, true };
				yield return new object[] { "F2 F0 81 10 5AA5", Code.Adc_rm16_imm16, true };
				yield return new object[] { "F2 F0 66 11 08", Code.Adc_rm32_r32, true };
				yield return new object[] { "F2 F0 66 83 10 A5", Code.Adc_rm32_imm8, true };
				yield return new object[] { "F2 F0 66 81 10 34 12 5AA5", Code.Adc_rm32_imm32, true };

				yield return new object[] { "F2 F0 00 08", Code.Add_rm8_r8, true };
				yield return new object[] { "F2 F0 80 00 A5", Code.Add_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 00 A5", Code.Add_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 01 08", Code.Add_rm16_r16, true };
				yield return new object[] { "F2 F0 83 00 A5", Code.Add_rm16_imm8, true };
				yield return new object[] { "F2 F0 81 00 5AA5", Code.Add_rm16_imm16, true };
				yield return new object[] { "F2 F0 66 01 08", Code.Add_rm32_r32, true };
				yield return new object[] { "F2 F0 66 83 00 A5", Code.Add_rm32_imm8, true };
				yield return new object[] { "F2 F0 66 81 00 34 12 5AA5", Code.Add_rm32_imm32, true };

				yield return new object[] { "F2 F0 20 08", Code.And_rm8_r8, true };
				yield return new object[] { "F2 F0 80 20 A5", Code.And_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 20 A5", Code.And_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 21 08", Code.And_rm16_r16, true };
				yield return new object[] { "F2 F0 83 20 A5", Code.And_rm16_imm8, true };
				yield return new object[] { "F2 F0 81 20 5AA5", Code.And_rm16_imm16, true };
				yield return new object[] { "F2 F0 66 21 08", Code.And_rm32_r32, true };
				yield return new object[] { "F2 F0 66 83 20 A5", Code.And_rm32_imm8, true };
				yield return new object[] { "F2 F0 66 81 20 34 12 5AA5", Code.And_rm32_imm32, true };

				yield return new object[] { "F2 F0 08 08", Code.Or_rm8_r8, true };
				yield return new object[] { "F2 F0 80 08 A5", Code.Or_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 08 A5", Code.Or_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 09 08", Code.Or_rm16_r16, true };
				yield return new object[] { "F2 F0 83 08 A5", Code.Or_rm16_imm8, true };
				yield return new object[] { "F2 F0 81 08 5AA5", Code.Or_rm16_imm16, true };
				yield return new object[] { "F2 F0 66 09 08", Code.Or_rm32_r32, true };
				yield return new object[] { "F2 F0 66 83 08 A5", Code.Or_rm32_imm8, true };
				yield return new object[] { "F2 F0 66 81 08 34 12 5AA5", Code.Or_rm32_imm32, true };

				yield return new object[] { "F2 F0 18 08", Code.Sbb_rm8_r8, true };
				yield return new object[] { "F2 F0 80 18 A5", Code.Sbb_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 18 A5", Code.Sbb_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 19 08", Code.Sbb_rm16_r16, true };
				yield return new object[] { "F2 F0 83 18 A5", Code.Sbb_rm16_imm8, true };
				yield return new object[] { "F2 F0 81 18 5AA5", Code.Sbb_rm16_imm16, true };
				yield return new object[] { "F2 F0 66 19 08", Code.Sbb_rm32_r32, true };
				yield return new object[] { "F2 F0 66 83 18 A5", Code.Sbb_rm32_imm8, true };
				yield return new object[] { "F2 F0 66 81 18 34 12 5AA5", Code.Sbb_rm32_imm32, true };

				yield return new object[] { "F2 F0 28 08", Code.Sub_rm8_r8, true };
				yield return new object[] { "F2 F0 80 28 A5", Code.Sub_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 28 A5", Code.Sub_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 29 08", Code.Sub_rm16_r16, true };
				yield return new object[] { "F2 F0 83 28 A5", Code.Sub_rm16_imm8, true };
				yield return new object[] { "F2 F0 81 28 5AA5", Code.Sub_rm16_imm16, true };
				yield return new object[] { "F2 F0 66 29 08", Code.Sub_rm32_r32, true };
				yield return new object[] { "F2 F0 66 83 28 A5", Code.Sub_rm32_imm8, true };
				yield return new object[] { "F2 F0 66 81 28 34 12 5AA5", Code.Sub_rm32_imm32, true };

				yield return new object[] { "F2 F0 30 08", Code.Xor_rm8_r8, true };
				yield return new object[] { "F2 F0 80 30 A5", Code.Xor_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 30 A5", Code.Xor_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 31 08", Code.Xor_rm16_r16, true };
				yield return new object[] { "F2 F0 83 30 A5", Code.Xor_rm16_imm8, true };
				yield return new object[] { "F2 F0 81 30 5AA5", Code.Xor_rm16_imm16, true };
				yield return new object[] { "F2 F0 66 31 08", Code.Xor_rm32_r32, true };
				yield return new object[] { "F2 F0 66 83 30 A5", Code.Xor_rm32_imm8, true };
				yield return new object[] { "F2 F0 66 81 30 34 12 5AA5", Code.Xor_rm32_imm32, true };

				yield return new object[] { "F2 F0 0F BB 08", Code.Btc_rm16_r16, true };
				yield return new object[] { "F2 F0 0F BA 38 A5", Code.Btc_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 0F BB 08", Code.Btc_rm32_r32, true };
				yield return new object[] { "F2 F0 66 0F BA 38 A5", Code.Btc_rm32_imm8, true };

				yield return new object[] { "F2 F0 0F B3 08", Code.Btr_rm16_r16, true };
				yield return new object[] { "F2 F0 0F BA 30 A5", Code.Btr_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 0F B3 08", Code.Btr_rm32_r32, true };
				yield return new object[] { "F2 F0 66 0F BA 30 A5", Code.Btr_rm32_imm8, true };

				yield return new object[] { "F2 F0 0F AB 08", Code.Bts_rm16_r16, true };
				yield return new object[] { "F2 F0 0F BA 28 A5", Code.Bts_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 0F AB 08", Code.Bts_rm32_r32, true };
				yield return new object[] { "F2 F0 66 0F BA 28 A5", Code.Bts_rm32_imm8, true };

				yield return new object[] { "F2 F0 FE 08", Code.Dec_rm8, true };
				yield return new object[] { "F2 F0 FF 08", Code.Dec_rm16, true };
				yield return new object[] { "F2 F0 66 FF 08", Code.Dec_rm32, true };

				yield return new object[] { "F2 F0 FE 00", Code.Inc_rm8, true };
				yield return new object[] { "F2 F0 FF 00", Code.Inc_rm16, true };
				yield return new object[] { "F2 F0 66 FF 00", Code.Inc_rm32, true };

				yield return new object[] { "F2 F0 F6 18", Code.Neg_rm8, true };
				yield return new object[] { "F2 F0 F7 18", Code.Neg_rm16, true };
				yield return new object[] { "F2 F0 66 F7 18", Code.Neg_rm32, true };

				yield return new object[] { "F2 F0 F6 10", Code.Not_rm8, true };
				yield return new object[] { "F2 F0 F7 10", Code.Not_rm16, true };
				yield return new object[] { "F2 F0 66 F7 10", Code.Not_rm32, true };

				yield return new object[] { "F2 F0 0F B0 08", Code.Cmpxchg_rm8_r8, true };
				yield return new object[] { "F2 F0 0F B1 08", Code.Cmpxchg_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F B1 08", Code.Cmpxchg_rm32_r32, true };

				yield return new object[] { "F2 F0 0F C0 08", Code.Xadd_rm8_r8, true };
				yield return new object[] { "F2 F0 0F C1 08", Code.Xadd_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F C1 08", Code.Xadd_rm32_r32, true };

				yield return new object[] { "F2 86 08", Code.Xchg_rm8_r8, false };
				yield return new object[] { "F2 87 08", Code.Xchg_rm16_r16, false };
				yield return new object[] { "F2 66 87 08", Code.Xchg_rm32_r32, false };

				yield return new object[] { "F2 F0 86 08", Code.Xchg_rm8_r8, true };
				yield return new object[] { "F2 F0 87 08", Code.Xchg_rm16_r16, true };
				yield return new object[] { "F2 F0 66 87 08", Code.Xchg_rm32_r32, true };

				yield return new object[] { "F2 F0 0F C7 08", Code.Cmpxchg8b_m64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XacquirePrefix_Data))]
		void Test32_XacquirePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.Equal(hasLock, instr.HasLockPrefix);
			Assert.True(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test32_XacquirePrefix_Data {
			get {
				yield return new object[] { "F2 F0 10 08", Code.Adc_rm8_r8, true };
				yield return new object[] { "F2 F0 80 10 A5", Code.Adc_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 10 A5", Code.Adc_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 66 11 08", Code.Adc_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 10 A5", Code.Adc_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 10 5AA5", Code.Adc_rm16_imm16, true };
				yield return new object[] { "F2 F0 11 08", Code.Adc_rm32_r32, true };
				yield return new object[] { "F2 F0 83 10 A5", Code.Adc_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 10 34 12 5AA5", Code.Adc_rm32_imm32, true };

				yield return new object[] { "F2 F0 00 08", Code.Add_rm8_r8, true };
				yield return new object[] { "F2 F0 80 00 A5", Code.Add_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 00 A5", Code.Add_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 66 01 08", Code.Add_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 00 A5", Code.Add_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 00 5AA5", Code.Add_rm16_imm16, true };
				yield return new object[] { "F2 F0 01 08", Code.Add_rm32_r32, true };
				yield return new object[] { "F2 F0 83 00 A5", Code.Add_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 00 34 12 5AA5", Code.Add_rm32_imm32, true };

				yield return new object[] { "F2 F0 20 08", Code.And_rm8_r8, true };
				yield return new object[] { "F2 F0 80 20 A5", Code.And_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 20 A5", Code.And_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 66 21 08", Code.And_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 20 A5", Code.And_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 20 5AA5", Code.And_rm16_imm16, true };
				yield return new object[] { "F2 F0 21 08", Code.And_rm32_r32, true };
				yield return new object[] { "F2 F0 83 20 A5", Code.And_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 20 34 12 5AA5", Code.And_rm32_imm32, true };

				yield return new object[] { "F2 F0 08 08", Code.Or_rm8_r8, true };
				yield return new object[] { "F2 F0 80 08 A5", Code.Or_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 08 A5", Code.Or_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 66 09 08", Code.Or_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 08 A5", Code.Or_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 08 5AA5", Code.Or_rm16_imm16, true };
				yield return new object[] { "F2 F0 09 08", Code.Or_rm32_r32, true };
				yield return new object[] { "F2 F0 83 08 A5", Code.Or_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 08 34 12 5AA5", Code.Or_rm32_imm32, true };

				yield return new object[] { "F2 F0 18 08", Code.Sbb_rm8_r8, true };
				yield return new object[] { "F2 F0 80 18 A5", Code.Sbb_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 18 A5", Code.Sbb_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 66 19 08", Code.Sbb_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 18 A5", Code.Sbb_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 18 5AA5", Code.Sbb_rm16_imm16, true };
				yield return new object[] { "F2 F0 19 08", Code.Sbb_rm32_r32, true };
				yield return new object[] { "F2 F0 83 18 A5", Code.Sbb_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 18 34 12 5AA5", Code.Sbb_rm32_imm32, true };

				yield return new object[] { "F2 F0 28 08", Code.Sub_rm8_r8, true };
				yield return new object[] { "F2 F0 80 28 A5", Code.Sub_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 28 A5", Code.Sub_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 66 29 08", Code.Sub_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 28 A5", Code.Sub_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 28 5AA5", Code.Sub_rm16_imm16, true };
				yield return new object[] { "F2 F0 29 08", Code.Sub_rm32_r32, true };
				yield return new object[] { "F2 F0 83 28 A5", Code.Sub_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 28 34 12 5AA5", Code.Sub_rm32_imm32, true };

				yield return new object[] { "F2 F0 30 08", Code.Xor_rm8_r8, true };
				yield return new object[] { "F2 F0 80 30 A5", Code.Xor_rm8_imm8, true };
				yield return new object[] { "F2 F0 82 30 A5", Code.Xor_rm8_imm8_82, true };
				yield return new object[] { "F2 F0 66 31 08", Code.Xor_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 30 A5", Code.Xor_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 30 5AA5", Code.Xor_rm16_imm16, true };
				yield return new object[] { "F2 F0 31 08", Code.Xor_rm32_r32, true };
				yield return new object[] { "F2 F0 83 30 A5", Code.Xor_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 30 34 12 5AA5", Code.Xor_rm32_imm32, true };

				yield return new object[] { "F2 F0 66 0F BB 08", Code.Btc_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F BA 38 A5", Code.Btc_rm16_imm8, true };
				yield return new object[] { "F2 F0 0F BB 08", Code.Btc_rm32_r32, true };
				yield return new object[] { "F2 F0 0F BA 38 A5", Code.Btc_rm32_imm8, true };

				yield return new object[] { "F2 F0 66 0F B3 08", Code.Btr_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F BA 30 A5", Code.Btr_rm16_imm8, true };
				yield return new object[] { "F2 F0 0F B3 08", Code.Btr_rm32_r32, true };
				yield return new object[] { "F2 F0 0F BA 30 A5", Code.Btr_rm32_imm8, true };

				yield return new object[] { "F2 F0 66 0F AB 08", Code.Bts_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F BA 28 A5", Code.Bts_rm16_imm8, true };
				yield return new object[] { "F2 F0 0F AB 08", Code.Bts_rm32_r32, true };
				yield return new object[] { "F2 F0 0F BA 28 A5", Code.Bts_rm32_imm8, true };

				yield return new object[] { "F2 F0 FE 08", Code.Dec_rm8, true };
				yield return new object[] { "F2 F0 66 FF 08", Code.Dec_rm16, true };
				yield return new object[] { "F2 F0 FF 08", Code.Dec_rm32, true };

				yield return new object[] { "F2 F0 FE 00", Code.Inc_rm8, true };
				yield return new object[] { "F2 F0 66 FF 00", Code.Inc_rm16, true };
				yield return new object[] { "F2 F0 FF 00", Code.Inc_rm32, true };

				yield return new object[] { "F2 F0 F6 18", Code.Neg_rm8, true };
				yield return new object[] { "F2 F0 66 F7 18", Code.Neg_rm16, true };
				yield return new object[] { "F2 F0 F7 18", Code.Neg_rm32, true };

				yield return new object[] { "F2 F0 F6 10", Code.Not_rm8, true };
				yield return new object[] { "F2 F0 66 F7 10", Code.Not_rm16, true };
				yield return new object[] { "F2 F0 F7 10", Code.Not_rm32, true };

				yield return new object[] { "F2 F0 0F B0 08", Code.Cmpxchg_rm8_r8, true };
				yield return new object[] { "F2 F0 66 0F B1 08", Code.Cmpxchg_rm16_r16, true };
				yield return new object[] { "F2 F0 0F B1 08", Code.Cmpxchg_rm32_r32, true };

				yield return new object[] { "F2 F0 0F C0 08", Code.Xadd_rm8_r8, true };
				yield return new object[] { "F2 F0 66 0F C1 08", Code.Xadd_rm16_r16, true };
				yield return new object[] { "F2 F0 0F C1 08", Code.Xadd_rm32_r32, true };

				yield return new object[] { "F2 86 08", Code.Xchg_rm8_r8, false };
				yield return new object[] { "F2 66 87 08", Code.Xchg_rm16_r16, false };
				yield return new object[] { "F2 87 08", Code.Xchg_rm32_r32, false };

				yield return new object[] { "F2 F0 86 08", Code.Xchg_rm8_r8, true };
				yield return new object[] { "F2 F0 66 87 08", Code.Xchg_rm16_r16, true };
				yield return new object[] { "F2 F0 87 08", Code.Xchg_rm32_r32, true };

				yield return new object[] { "F2 F0 0F C7 08", Code.Cmpxchg8b_m64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XacquirePrefix_Data))]
		void Test64_XacquirePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.Equal(hasLock, instr.HasLockPrefix);
			Assert.True(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test64_XacquirePrefix_Data {
			get {
				yield return new object[] { "F2 F0 10 08", Code.Adc_rm8_r8, true };
				yield return new object[] { "F2 F0 80 10 A5", Code.Adc_rm8_imm8, true };
				yield return new object[] { "F2 F0 66 11 08", Code.Adc_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 10 A5", Code.Adc_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 10 5AA5", Code.Adc_rm16_imm16, true };
				yield return new object[] { "F2 F0 11 08", Code.Adc_rm32_r32, true };
				yield return new object[] { "F2 F0 83 10 A5", Code.Adc_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 10 34 12 5AA5", Code.Adc_rm32_imm32, true };
				yield return new object[] { "F2 F0 48 11 08", Code.Adc_rm64_r64, true };
				yield return new object[] { "F2 F0 48 83 10 A5", Code.Adc_rm64_imm8, true };
				yield return new object[] { "F2 F0 48 81 10 34 12 5AA5", Code.Adc_rm64_imm32, true };

				yield return new object[] { "F2 F0 00 08", Code.Add_rm8_r8, true };
				yield return new object[] { "F2 F0 80 00 A5", Code.Add_rm8_imm8, true };
				yield return new object[] { "F2 F0 66 01 08", Code.Add_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 00 A5", Code.Add_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 00 5AA5", Code.Add_rm16_imm16, true };
				yield return new object[] { "F2 F0 01 08", Code.Add_rm32_r32, true };
				yield return new object[] { "F2 F0 83 00 A5", Code.Add_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 00 34 12 5AA5", Code.Add_rm32_imm32, true };
				yield return new object[] { "F2 F0 48 01 08", Code.Add_rm64_r64, true };
				yield return new object[] { "F2 F0 48 83 00 A5", Code.Add_rm64_imm8, true };
				yield return new object[] { "F2 F0 48 81 00 34 12 5AA5", Code.Add_rm64_imm32, true };

				yield return new object[] { "F2 F0 20 08", Code.And_rm8_r8, true };
				yield return new object[] { "F2 F0 80 20 A5", Code.And_rm8_imm8, true };
				yield return new object[] { "F2 F0 66 21 08", Code.And_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 20 A5", Code.And_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 20 5AA5", Code.And_rm16_imm16, true };
				yield return new object[] { "F2 F0 21 08", Code.And_rm32_r32, true };
				yield return new object[] { "F2 F0 83 20 A5", Code.And_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 20 34 12 5AA5", Code.And_rm32_imm32, true };
				yield return new object[] { "F2 F0 48 21 08", Code.And_rm64_r64, true };
				yield return new object[] { "F2 F0 48 83 20 A5", Code.And_rm64_imm8, true };
				yield return new object[] { "F2 F0 48 81 20 34 12 5AA5", Code.And_rm64_imm32, true };

				yield return new object[] { "F2 F0 08 08", Code.Or_rm8_r8, true };
				yield return new object[] { "F2 F0 80 08 A5", Code.Or_rm8_imm8, true };
				yield return new object[] { "F2 F0 66 09 08", Code.Or_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 08 A5", Code.Or_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 08 5AA5", Code.Or_rm16_imm16, true };
				yield return new object[] { "F2 F0 09 08", Code.Or_rm32_r32, true };
				yield return new object[] { "F2 F0 83 08 A5", Code.Or_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 08 34 12 5AA5", Code.Or_rm32_imm32, true };
				yield return new object[] { "F2 F0 48 09 08", Code.Or_rm64_r64, true };
				yield return new object[] { "F2 F0 48 83 08 A5", Code.Or_rm64_imm8, true };
				yield return new object[] { "F2 F0 48 81 08 34 12 5AA5", Code.Or_rm64_imm32, true };

				yield return new object[] { "F2 F0 18 08", Code.Sbb_rm8_r8, true };
				yield return new object[] { "F2 F0 80 18 A5", Code.Sbb_rm8_imm8, true };
				yield return new object[] { "F2 F0 66 19 08", Code.Sbb_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 18 A5", Code.Sbb_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 18 5AA5", Code.Sbb_rm16_imm16, true };
				yield return new object[] { "F2 F0 19 08", Code.Sbb_rm32_r32, true };
				yield return new object[] { "F2 F0 83 18 A5", Code.Sbb_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 18 34 12 5AA5", Code.Sbb_rm32_imm32, true };
				yield return new object[] { "F2 F0 48 19 08", Code.Sbb_rm64_r64, true };
				yield return new object[] { "F2 F0 48 83 18 A5", Code.Sbb_rm64_imm8, true };
				yield return new object[] { "F2 F0 48 81 18 34 12 5AA5", Code.Sbb_rm64_imm32, true };

				yield return new object[] { "F2 F0 28 08", Code.Sub_rm8_r8, true };
				yield return new object[] { "F2 F0 80 28 A5", Code.Sub_rm8_imm8, true };
				yield return new object[] { "F2 F0 66 29 08", Code.Sub_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 28 A5", Code.Sub_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 28 5AA5", Code.Sub_rm16_imm16, true };
				yield return new object[] { "F2 F0 29 08", Code.Sub_rm32_r32, true };
				yield return new object[] { "F2 F0 83 28 A5", Code.Sub_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 28 34 12 5AA5", Code.Sub_rm32_imm32, true };
				yield return new object[] { "F2 F0 48 29 08", Code.Sub_rm64_r64, true };
				yield return new object[] { "F2 F0 48 83 28 A5", Code.Sub_rm64_imm8, true };
				yield return new object[] { "F2 F0 48 81 28 34 12 5AA5", Code.Sub_rm64_imm32, true };

				yield return new object[] { "F2 F0 30 08", Code.Xor_rm8_r8, true };
				yield return new object[] { "F2 F0 80 30 A5", Code.Xor_rm8_imm8, true };
				yield return new object[] { "F2 F0 66 31 08", Code.Xor_rm16_r16, true };
				yield return new object[] { "F2 F0 66 83 30 A5", Code.Xor_rm16_imm8, true };
				yield return new object[] { "F2 F0 66 81 30 5AA5", Code.Xor_rm16_imm16, true };
				yield return new object[] { "F2 F0 31 08", Code.Xor_rm32_r32, true };
				yield return new object[] { "F2 F0 83 30 A5", Code.Xor_rm32_imm8, true };
				yield return new object[] { "F2 F0 81 30 34 12 5AA5", Code.Xor_rm32_imm32, true };
				yield return new object[] { "F2 F0 48 31 08", Code.Xor_rm64_r64, true };
				yield return new object[] { "F2 F0 48 83 30 A5", Code.Xor_rm64_imm8, true };
				yield return new object[] { "F2 F0 48 81 30 34 12 5AA5", Code.Xor_rm64_imm32, true };

				yield return new object[] { "F2 F0 66 0F BB 08", Code.Btc_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F BA 38 A5", Code.Btc_rm16_imm8, true };
				yield return new object[] { "F2 F0 0F BB 08", Code.Btc_rm32_r32, true };
				yield return new object[] { "F2 F0 0F BA 38 A5", Code.Btc_rm32_imm8, true };
				yield return new object[] { "F2 F0 48 0F BB 08", Code.Btc_rm64_r64, true };
				yield return new object[] { "F2 F0 48 0F BA 38 A5", Code.Btc_rm64_imm8, true };

				yield return new object[] { "F2 F0 66 0F B3 08", Code.Btr_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F BA 30 A5", Code.Btr_rm16_imm8, true };
				yield return new object[] { "F2 F0 0F B3 08", Code.Btr_rm32_r32, true };
				yield return new object[] { "F2 F0 0F BA 30 A5", Code.Btr_rm32_imm8, true };
				yield return new object[] { "F2 F0 48 0F B3 08", Code.Btr_rm64_r64, true };
				yield return new object[] { "F2 F0 48 0F BA 30 A5", Code.Btr_rm64_imm8, true };

				yield return new object[] { "F2 F0 66 0F AB 08", Code.Bts_rm16_r16, true };
				yield return new object[] { "F2 F0 66 0F BA 28 A5", Code.Bts_rm16_imm8, true };
				yield return new object[] { "F2 F0 0F AB 08", Code.Bts_rm32_r32, true };
				yield return new object[] { "F2 F0 0F BA 28 A5", Code.Bts_rm32_imm8, true };
				yield return new object[] { "F2 F0 48 0F AB 08", Code.Bts_rm64_r64, true };
				yield return new object[] { "F2 F0 48 0F BA 28 A5", Code.Bts_rm64_imm8, true };

				yield return new object[] { "F2 F0 FE 08", Code.Dec_rm8, true };
				yield return new object[] { "F2 F0 66 FF 08", Code.Dec_rm16, true };
				yield return new object[] { "F2 F0 FF 08", Code.Dec_rm32, true };
				yield return new object[] { "F2 F0 48 FF 08", Code.Dec_rm64, true };

				yield return new object[] { "F2 F0 FE 00", Code.Inc_rm8, true };
				yield return new object[] { "F2 F0 66 FF 00", Code.Inc_rm16, true };
				yield return new object[] { "F2 F0 FF 00", Code.Inc_rm32, true };
				yield return new object[] { "F2 F0 48 FF 00", Code.Inc_rm64, true };

				yield return new object[] { "F2 F0 F6 18", Code.Neg_rm8, true };
				yield return new object[] { "F2 F0 66 F7 18", Code.Neg_rm16, true };
				yield return new object[] { "F2 F0 F7 18", Code.Neg_rm32, true };
				yield return new object[] { "F2 F0 48 F7 18", Code.Neg_rm64, true };

				yield return new object[] { "F2 F0 F6 10", Code.Not_rm8, true };
				yield return new object[] { "F2 F0 66 F7 10", Code.Not_rm16, true };
				yield return new object[] { "F2 F0 F7 10", Code.Not_rm32, true };
				yield return new object[] { "F2 F0 48 F7 10", Code.Not_rm64, true };

				yield return new object[] { "F2 F0 0F B0 08", Code.Cmpxchg_rm8_r8, true };
				yield return new object[] { "F2 F0 66 0F B1 08", Code.Cmpxchg_rm16_r16, true };
				yield return new object[] { "F2 F0 0F B1 08", Code.Cmpxchg_rm32_r32, true };
				yield return new object[] { "F2 F0 48 0F B1 08", Code.Cmpxchg_rm64_r64, true };

				yield return new object[] { "F2 F0 0F C0 08", Code.Xadd_rm8_r8, true };
				yield return new object[] { "F2 F0 66 0F C1 08", Code.Xadd_rm16_r16, true };
				yield return new object[] { "F2 F0 0F C1 08", Code.Xadd_rm32_r32, true };
				yield return new object[] { "F2 F0 48 0F C1 08", Code.Xadd_rm64_r64, true };

				yield return new object[] { "F2 86 08", Code.Xchg_rm8_r8, false };
				yield return new object[] { "F2 66 87 08", Code.Xchg_rm16_r16, false };
				yield return new object[] { "F2 87 08", Code.Xchg_rm32_r32, false };
				yield return new object[] { "F2 48 87 08", Code.Xchg_rm64_r64, false };

				yield return new object[] { "F2 F0 86 08", Code.Xchg_rm8_r8, true };
				yield return new object[] { "F2 F0 66 87 08", Code.Xchg_rm16_r16, true };
				yield return new object[] { "F2 F0 87 08", Code.Xchg_rm32_r32, true };
				yield return new object[] { "F2 F0 48 87 08", Code.Xchg_rm64_r64, true };

				yield return new object[] { "F2 F0 0F C7 08", Code.Cmpxchg8b_m64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_XreleasePrefix_Data))]
		void Test16_XreleasePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.Equal(hasLock, instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.True(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test16_XreleasePrefix_Data {
			get {
				yield return new object[] { "F3 F0 10 08", Code.Adc_rm8_r8, true };
				yield return new object[] { "F3 F0 80 10 A5", Code.Adc_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 10 A5", Code.Adc_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 11 08", Code.Adc_rm16_r16, true };
				yield return new object[] { "F3 F0 83 10 A5", Code.Adc_rm16_imm8, true };
				yield return new object[] { "F3 F0 81 10 5AA5", Code.Adc_rm16_imm16, true };
				yield return new object[] { "F3 F0 66 11 08", Code.Adc_rm32_r32, true };
				yield return new object[] { "F3 F0 66 83 10 A5", Code.Adc_rm32_imm8, true };
				yield return new object[] { "F3 F0 66 81 10 34 12 5AA5", Code.Adc_rm32_imm32, true };

				yield return new object[] { "F3 F0 00 08", Code.Add_rm8_r8, true };
				yield return new object[] { "F3 F0 80 00 A5", Code.Add_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 00 A5", Code.Add_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 01 08", Code.Add_rm16_r16, true };
				yield return new object[] { "F3 F0 83 00 A5", Code.Add_rm16_imm8, true };
				yield return new object[] { "F3 F0 81 00 5AA5", Code.Add_rm16_imm16, true };
				yield return new object[] { "F3 F0 66 01 08", Code.Add_rm32_r32, true };
				yield return new object[] { "F3 F0 66 83 00 A5", Code.Add_rm32_imm8, true };
				yield return new object[] { "F3 F0 66 81 00 34 12 5AA5", Code.Add_rm32_imm32, true };

				yield return new object[] { "F3 F0 20 08", Code.And_rm8_r8, true };
				yield return new object[] { "F3 F0 80 20 A5", Code.And_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 20 A5", Code.And_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 21 08", Code.And_rm16_r16, true };
				yield return new object[] { "F3 F0 83 20 A5", Code.And_rm16_imm8, true };
				yield return new object[] { "F3 F0 81 20 5AA5", Code.And_rm16_imm16, true };
				yield return new object[] { "F3 F0 66 21 08", Code.And_rm32_r32, true };
				yield return new object[] { "F3 F0 66 83 20 A5", Code.And_rm32_imm8, true };
				yield return new object[] { "F3 F0 66 81 20 34 12 5AA5", Code.And_rm32_imm32, true };

				yield return new object[] { "F3 F0 08 08", Code.Or_rm8_r8, true };
				yield return new object[] { "F3 F0 80 08 A5", Code.Or_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 08 A5", Code.Or_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 09 08", Code.Or_rm16_r16, true };
				yield return new object[] { "F3 F0 83 08 A5", Code.Or_rm16_imm8, true };
				yield return new object[] { "F3 F0 81 08 5AA5", Code.Or_rm16_imm16, true };
				yield return new object[] { "F3 F0 66 09 08", Code.Or_rm32_r32, true };
				yield return new object[] { "F3 F0 66 83 08 A5", Code.Or_rm32_imm8, true };
				yield return new object[] { "F3 F0 66 81 08 34 12 5AA5", Code.Or_rm32_imm32, true };

				yield return new object[] { "F3 F0 18 08", Code.Sbb_rm8_r8, true };
				yield return new object[] { "F3 F0 80 18 A5", Code.Sbb_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 18 A5", Code.Sbb_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 19 08", Code.Sbb_rm16_r16, true };
				yield return new object[] { "F3 F0 83 18 A5", Code.Sbb_rm16_imm8, true };
				yield return new object[] { "F3 F0 81 18 5AA5", Code.Sbb_rm16_imm16, true };
				yield return new object[] { "F3 F0 66 19 08", Code.Sbb_rm32_r32, true };
				yield return new object[] { "F3 F0 66 83 18 A5", Code.Sbb_rm32_imm8, true };
				yield return new object[] { "F3 F0 66 81 18 34 12 5AA5", Code.Sbb_rm32_imm32, true };

				yield return new object[] { "F3 F0 28 08", Code.Sub_rm8_r8, true };
				yield return new object[] { "F3 F0 80 28 A5", Code.Sub_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 28 A5", Code.Sub_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 29 08", Code.Sub_rm16_r16, true };
				yield return new object[] { "F3 F0 83 28 A5", Code.Sub_rm16_imm8, true };
				yield return new object[] { "F3 F0 81 28 5AA5", Code.Sub_rm16_imm16, true };
				yield return new object[] { "F3 F0 66 29 08", Code.Sub_rm32_r32, true };
				yield return new object[] { "F3 F0 66 83 28 A5", Code.Sub_rm32_imm8, true };
				yield return new object[] { "F3 F0 66 81 28 34 12 5AA5", Code.Sub_rm32_imm32, true };

				yield return new object[] { "F3 F0 30 08", Code.Xor_rm8_r8, true };
				yield return new object[] { "F3 F0 80 30 A5", Code.Xor_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 30 A5", Code.Xor_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 31 08", Code.Xor_rm16_r16, true };
				yield return new object[] { "F3 F0 83 30 A5", Code.Xor_rm16_imm8, true };
				yield return new object[] { "F3 F0 81 30 5AA5", Code.Xor_rm16_imm16, true };
				yield return new object[] { "F3 F0 66 31 08", Code.Xor_rm32_r32, true };
				yield return new object[] { "F3 F0 66 83 30 A5", Code.Xor_rm32_imm8, true };
				yield return new object[] { "F3 F0 66 81 30 34 12 5AA5", Code.Xor_rm32_imm32, true };

				yield return new object[] { "F3 F0 0F BB 08", Code.Btc_rm16_r16, true };
				yield return new object[] { "F3 F0 0F BA 38 A5", Code.Btc_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 0F BB 08", Code.Btc_rm32_r32, true };
				yield return new object[] { "F3 F0 66 0F BA 38 A5", Code.Btc_rm32_imm8, true };

				yield return new object[] { "F3 F0 0F B3 08", Code.Btr_rm16_r16, true };
				yield return new object[] { "F3 F0 0F BA 30 A5", Code.Btr_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 0F B3 08", Code.Btr_rm32_r32, true };
				yield return new object[] { "F3 F0 66 0F BA 30 A5", Code.Btr_rm32_imm8, true };

				yield return new object[] { "F3 F0 0F AB 08", Code.Bts_rm16_r16, true };
				yield return new object[] { "F3 F0 0F BA 28 A5", Code.Bts_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 0F AB 08", Code.Bts_rm32_r32, true };
				yield return new object[] { "F3 F0 66 0F BA 28 A5", Code.Bts_rm32_imm8, true };

				yield return new object[] { "F3 F0 FE 08", Code.Dec_rm8, true };
				yield return new object[] { "F3 F0 FF 08", Code.Dec_rm16, true };
				yield return new object[] { "F3 F0 66 FF 08", Code.Dec_rm32, true };

				yield return new object[] { "F3 F0 FE 00", Code.Inc_rm8, true };
				yield return new object[] { "F3 F0 FF 00", Code.Inc_rm16, true };
				yield return new object[] { "F3 F0 66 FF 00", Code.Inc_rm32, true };

				yield return new object[] { "F3 F0 F6 18", Code.Neg_rm8, true };
				yield return new object[] { "F3 F0 F7 18", Code.Neg_rm16, true };
				yield return new object[] { "F3 F0 66 F7 18", Code.Neg_rm32, true };

				yield return new object[] { "F3 F0 F6 10", Code.Not_rm8, true };
				yield return new object[] { "F3 F0 F7 10", Code.Not_rm16, true };
				yield return new object[] { "F3 F0 66 F7 10", Code.Not_rm32, true };

				yield return new object[] { "F3 F0 0F B0 08", Code.Cmpxchg_rm8_r8, true };
				yield return new object[] { "F3 F0 0F B1 08", Code.Cmpxchg_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F B1 08", Code.Cmpxchg_rm32_r32, true };

				yield return new object[] { "F3 F0 0F C0 08", Code.Xadd_rm8_r8, true };
				yield return new object[] { "F3 F0 0F C1 08", Code.Xadd_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F C1 08", Code.Xadd_rm32_r32, true };

				yield return new object[] { "F3 86 08", Code.Xchg_rm8_r8, false };
				yield return new object[] { "F3 87 08", Code.Xchg_rm16_r16, false };
				yield return new object[] { "F3 66 87 08", Code.Xchg_rm32_r32, false };

				yield return new object[] { "F3 F0 86 08", Code.Xchg_rm8_r8, true };
				yield return new object[] { "F3 F0 87 08", Code.Xchg_rm16_r16, true };
				yield return new object[] { "F3 F0 66 87 08", Code.Xchg_rm32_r32, true };

				yield return new object[] { "F3 F0 0F C7 08", Code.Cmpxchg8b_m64, true };

				yield return new object[] { "F3 88 08", Code.Mov_rm8_r8, false };
				yield return new object[] { "F3 89 08", Code.Mov_rm16_r16, false };
				yield return new object[] { "F3 66 89 08", Code.Mov_rm32_r32, false };

				yield return new object[] { "F3 C6 00 A5", Code.Mov_rm8_imm8, false };
				yield return new object[] { "F3 C7 00 A5FF", Code.Mov_rm16_imm16, false };
				yield return new object[] { "F3 66 C7 00 A5FFFFFF", Code.Mov_rm32_imm32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_XreleasePrefix_Data))]
		void Test32_XreleasePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.Equal(hasLock, instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.True(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test32_XreleasePrefix_Data {
			get {
				yield return new object[] { "F3 F0 10 08", Code.Adc_rm8_r8, true };
				yield return new object[] { "F3 F0 80 10 A5", Code.Adc_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 10 A5", Code.Adc_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 66 11 08", Code.Adc_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 10 A5", Code.Adc_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 10 5AA5", Code.Adc_rm16_imm16, true };
				yield return new object[] { "F3 F0 11 08", Code.Adc_rm32_r32, true };
				yield return new object[] { "F3 F0 83 10 A5", Code.Adc_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 10 34 12 5AA5", Code.Adc_rm32_imm32, true };

				yield return new object[] { "F3 F0 00 08", Code.Add_rm8_r8, true };
				yield return new object[] { "F3 F0 80 00 A5", Code.Add_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 00 A5", Code.Add_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 66 01 08", Code.Add_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 00 A5", Code.Add_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 00 5AA5", Code.Add_rm16_imm16, true };
				yield return new object[] { "F3 F0 01 08", Code.Add_rm32_r32, true };
				yield return new object[] { "F3 F0 83 00 A5", Code.Add_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 00 34 12 5AA5", Code.Add_rm32_imm32, true };

				yield return new object[] { "F3 F0 20 08", Code.And_rm8_r8, true };
				yield return new object[] { "F3 F0 80 20 A5", Code.And_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 20 A5", Code.And_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 66 21 08", Code.And_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 20 A5", Code.And_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 20 5AA5", Code.And_rm16_imm16, true };
				yield return new object[] { "F3 F0 21 08", Code.And_rm32_r32, true };
				yield return new object[] { "F3 F0 83 20 A5", Code.And_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 20 34 12 5AA5", Code.And_rm32_imm32, true };

				yield return new object[] { "F3 F0 08 08", Code.Or_rm8_r8, true };
				yield return new object[] { "F3 F0 80 08 A5", Code.Or_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 08 A5", Code.Or_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 66 09 08", Code.Or_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 08 A5", Code.Or_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 08 5AA5", Code.Or_rm16_imm16, true };
				yield return new object[] { "F3 F0 09 08", Code.Or_rm32_r32, true };
				yield return new object[] { "F3 F0 83 08 A5", Code.Or_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 08 34 12 5AA5", Code.Or_rm32_imm32, true };

				yield return new object[] { "F3 F0 18 08", Code.Sbb_rm8_r8, true };
				yield return new object[] { "F3 F0 80 18 A5", Code.Sbb_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 18 A5", Code.Sbb_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 66 19 08", Code.Sbb_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 18 A5", Code.Sbb_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 18 5AA5", Code.Sbb_rm16_imm16, true };
				yield return new object[] { "F3 F0 19 08", Code.Sbb_rm32_r32, true };
				yield return new object[] { "F3 F0 83 18 A5", Code.Sbb_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 18 34 12 5AA5", Code.Sbb_rm32_imm32, true };

				yield return new object[] { "F3 F0 28 08", Code.Sub_rm8_r8, true };
				yield return new object[] { "F3 F0 80 28 A5", Code.Sub_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 28 A5", Code.Sub_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 66 29 08", Code.Sub_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 28 A5", Code.Sub_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 28 5AA5", Code.Sub_rm16_imm16, true };
				yield return new object[] { "F3 F0 29 08", Code.Sub_rm32_r32, true };
				yield return new object[] { "F3 F0 83 28 A5", Code.Sub_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 28 34 12 5AA5", Code.Sub_rm32_imm32, true };

				yield return new object[] { "F3 F0 30 08", Code.Xor_rm8_r8, true };
				yield return new object[] { "F3 F0 80 30 A5", Code.Xor_rm8_imm8, true };
				yield return new object[] { "F3 F0 82 30 A5", Code.Xor_rm8_imm8_82, true };
				yield return new object[] { "F3 F0 66 31 08", Code.Xor_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 30 A5", Code.Xor_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 30 5AA5", Code.Xor_rm16_imm16, true };
				yield return new object[] { "F3 F0 31 08", Code.Xor_rm32_r32, true };
				yield return new object[] { "F3 F0 83 30 A5", Code.Xor_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 30 34 12 5AA5", Code.Xor_rm32_imm32, true };

				yield return new object[] { "F3 F0 66 0F BB 08", Code.Btc_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F BA 38 A5", Code.Btc_rm16_imm8, true };
				yield return new object[] { "F3 F0 0F BB 08", Code.Btc_rm32_r32, true };
				yield return new object[] { "F3 F0 0F BA 38 A5", Code.Btc_rm32_imm8, true };

				yield return new object[] { "F3 F0 66 0F B3 08", Code.Btr_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F BA 30 A5", Code.Btr_rm16_imm8, true };
				yield return new object[] { "F3 F0 0F B3 08", Code.Btr_rm32_r32, true };
				yield return new object[] { "F3 F0 0F BA 30 A5", Code.Btr_rm32_imm8, true };

				yield return new object[] { "F3 F0 66 0F AB 08", Code.Bts_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F BA 28 A5", Code.Bts_rm16_imm8, true };
				yield return new object[] { "F3 F0 0F AB 08", Code.Bts_rm32_r32, true };
				yield return new object[] { "F3 F0 0F BA 28 A5", Code.Bts_rm32_imm8, true };

				yield return new object[] { "F3 F0 FE 08", Code.Dec_rm8, true };
				yield return new object[] { "F3 F0 66 FF 08", Code.Dec_rm16, true };
				yield return new object[] { "F3 F0 FF 08", Code.Dec_rm32, true };

				yield return new object[] { "F3 F0 FE 00", Code.Inc_rm8, true };
				yield return new object[] { "F3 F0 66 FF 00", Code.Inc_rm16, true };
				yield return new object[] { "F3 F0 FF 00", Code.Inc_rm32, true };

				yield return new object[] { "F3 F0 F6 18", Code.Neg_rm8, true };
				yield return new object[] { "F3 F0 66 F7 18", Code.Neg_rm16, true };
				yield return new object[] { "F3 F0 F7 18", Code.Neg_rm32, true };

				yield return new object[] { "F3 F0 F6 10", Code.Not_rm8, true };
				yield return new object[] { "F3 F0 66 F7 10", Code.Not_rm16, true };
				yield return new object[] { "F3 F0 F7 10", Code.Not_rm32, true };

				yield return new object[] { "F3 F0 0F B0 08", Code.Cmpxchg_rm8_r8, true };
				yield return new object[] { "F3 F0 66 0F B1 08", Code.Cmpxchg_rm16_r16, true };
				yield return new object[] { "F3 F0 0F B1 08", Code.Cmpxchg_rm32_r32, true };

				yield return new object[] { "F3 F0 0F C0 08", Code.Xadd_rm8_r8, true };
				yield return new object[] { "F3 F0 66 0F C1 08", Code.Xadd_rm16_r16, true };
				yield return new object[] { "F3 F0 0F C1 08", Code.Xadd_rm32_r32, true };

				yield return new object[] { "F3 86 08", Code.Xchg_rm8_r8, false };
				yield return new object[] { "F3 66 87 08", Code.Xchg_rm16_r16, false };
				yield return new object[] { "F3 87 08", Code.Xchg_rm32_r32, false };

				yield return new object[] { "F3 F0 86 08", Code.Xchg_rm8_r8, true };
				yield return new object[] { "F3 F0 66 87 08", Code.Xchg_rm16_r16, true };
				yield return new object[] { "F3 F0 87 08", Code.Xchg_rm32_r32, true };

				yield return new object[] { "F3 F0 0F C7 08", Code.Cmpxchg8b_m64, true };

				yield return new object[] { "F3 88 08", Code.Mov_rm8_r8, false };
				yield return new object[] { "F3 66 89 08", Code.Mov_rm16_r16, false };
				yield return new object[] { "F3 89 08", Code.Mov_rm32_r32, false };

				yield return new object[] { "F3 C6 00 A5", Code.Mov_rm8_imm8, false };
				yield return new object[] { "F3 66 C7 00 A5FF", Code.Mov_rm16_imm16, false };
				yield return new object[] { "F3 C7 00 A5FFFFFF", Code.Mov_rm32_imm32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_XreleasePrefix_Data))]
		void Test64_XreleasePrefix(string hexBytes, Code code, bool hasLock) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.Equal(hasLock, instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.True(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
		}
		public static IEnumerable<object[]> Test64_XreleasePrefix_Data {
			get {
				yield return new object[] { "F3 F0 10 08", Code.Adc_rm8_r8, true };
				yield return new object[] { "F3 F0 80 10 A5", Code.Adc_rm8_imm8, true };
				yield return new object[] { "F3 F0 66 11 08", Code.Adc_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 10 A5", Code.Adc_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 10 5AA5", Code.Adc_rm16_imm16, true };
				yield return new object[] { "F3 F0 11 08", Code.Adc_rm32_r32, true };
				yield return new object[] { "F3 F0 83 10 A5", Code.Adc_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 10 34 12 5AA5", Code.Adc_rm32_imm32, true };
				yield return new object[] { "F3 F0 48 11 08", Code.Adc_rm64_r64, true };
				yield return new object[] { "F3 F0 48 83 10 A5", Code.Adc_rm64_imm8, true };
				yield return new object[] { "F3 F0 48 81 10 34 12 5AA5", Code.Adc_rm64_imm32, true };

				yield return new object[] { "F3 F0 00 08", Code.Add_rm8_r8, true };
				yield return new object[] { "F3 F0 80 00 A5", Code.Add_rm8_imm8, true };
				yield return new object[] { "F3 F0 66 01 08", Code.Add_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 00 A5", Code.Add_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 00 5AA5", Code.Add_rm16_imm16, true };
				yield return new object[] { "F3 F0 01 08", Code.Add_rm32_r32, true };
				yield return new object[] { "F3 F0 83 00 A5", Code.Add_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 00 34 12 5AA5", Code.Add_rm32_imm32, true };
				yield return new object[] { "F3 F0 48 01 08", Code.Add_rm64_r64, true };
				yield return new object[] { "F3 F0 48 83 00 A5", Code.Add_rm64_imm8, true };
				yield return new object[] { "F3 F0 48 81 00 34 12 5AA5", Code.Add_rm64_imm32, true };

				yield return new object[] { "F3 F0 20 08", Code.And_rm8_r8, true };
				yield return new object[] { "F3 F0 80 20 A5", Code.And_rm8_imm8, true };
				yield return new object[] { "F3 F0 66 21 08", Code.And_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 20 A5", Code.And_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 20 5AA5", Code.And_rm16_imm16, true };
				yield return new object[] { "F3 F0 21 08", Code.And_rm32_r32, true };
				yield return new object[] { "F3 F0 83 20 A5", Code.And_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 20 34 12 5AA5", Code.And_rm32_imm32, true };
				yield return new object[] { "F3 F0 48 21 08", Code.And_rm64_r64, true };
				yield return new object[] { "F3 F0 48 83 20 A5", Code.And_rm64_imm8, true };
				yield return new object[] { "F3 F0 48 81 20 34 12 5AA5", Code.And_rm64_imm32, true };

				yield return new object[] { "F3 F0 08 08", Code.Or_rm8_r8, true };
				yield return new object[] { "F3 F0 80 08 A5", Code.Or_rm8_imm8, true };
				yield return new object[] { "F3 F0 66 09 08", Code.Or_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 08 A5", Code.Or_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 08 5AA5", Code.Or_rm16_imm16, true };
				yield return new object[] { "F3 F0 09 08", Code.Or_rm32_r32, true };
				yield return new object[] { "F3 F0 83 08 A5", Code.Or_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 08 34 12 5AA5", Code.Or_rm32_imm32, true };
				yield return new object[] { "F3 F0 48 09 08", Code.Or_rm64_r64, true };
				yield return new object[] { "F3 F0 48 83 08 A5", Code.Or_rm64_imm8, true };
				yield return new object[] { "F3 F0 48 81 08 34 12 5AA5", Code.Or_rm64_imm32, true };

				yield return new object[] { "F3 F0 18 08", Code.Sbb_rm8_r8, true };
				yield return new object[] { "F3 F0 80 18 A5", Code.Sbb_rm8_imm8, true };
				yield return new object[] { "F3 F0 66 19 08", Code.Sbb_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 18 A5", Code.Sbb_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 18 5AA5", Code.Sbb_rm16_imm16, true };
				yield return new object[] { "F3 F0 19 08", Code.Sbb_rm32_r32, true };
				yield return new object[] { "F3 F0 83 18 A5", Code.Sbb_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 18 34 12 5AA5", Code.Sbb_rm32_imm32, true };
				yield return new object[] { "F3 F0 48 19 08", Code.Sbb_rm64_r64, true };
				yield return new object[] { "F3 F0 48 83 18 A5", Code.Sbb_rm64_imm8, true };
				yield return new object[] { "F3 F0 48 81 18 34 12 5AA5", Code.Sbb_rm64_imm32, true };

				yield return new object[] { "F3 F0 28 08", Code.Sub_rm8_r8, true };
				yield return new object[] { "F3 F0 80 28 A5", Code.Sub_rm8_imm8, true };
				yield return new object[] { "F3 F0 66 29 08", Code.Sub_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 28 A5", Code.Sub_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 28 5AA5", Code.Sub_rm16_imm16, true };
				yield return new object[] { "F3 F0 29 08", Code.Sub_rm32_r32, true };
				yield return new object[] { "F3 F0 83 28 A5", Code.Sub_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 28 34 12 5AA5", Code.Sub_rm32_imm32, true };
				yield return new object[] { "F3 F0 48 29 08", Code.Sub_rm64_r64, true };
				yield return new object[] { "F3 F0 48 83 28 A5", Code.Sub_rm64_imm8, true };
				yield return new object[] { "F3 F0 48 81 28 34 12 5AA5", Code.Sub_rm64_imm32, true };

				yield return new object[] { "F3 F0 30 08", Code.Xor_rm8_r8, true };
				yield return new object[] { "F3 F0 80 30 A5", Code.Xor_rm8_imm8, true };
				yield return new object[] { "F3 F0 66 31 08", Code.Xor_rm16_r16, true };
				yield return new object[] { "F3 F0 66 83 30 A5", Code.Xor_rm16_imm8, true };
				yield return new object[] { "F3 F0 66 81 30 5AA5", Code.Xor_rm16_imm16, true };
				yield return new object[] { "F3 F0 31 08", Code.Xor_rm32_r32, true };
				yield return new object[] { "F3 F0 83 30 A5", Code.Xor_rm32_imm8, true };
				yield return new object[] { "F3 F0 81 30 34 12 5AA5", Code.Xor_rm32_imm32, true };
				yield return new object[] { "F3 F0 48 31 08", Code.Xor_rm64_r64, true };
				yield return new object[] { "F3 F0 48 83 30 A5", Code.Xor_rm64_imm8, true };
				yield return new object[] { "F3 F0 48 81 30 34 12 5AA5", Code.Xor_rm64_imm32, true };

				yield return new object[] { "F3 F0 66 0F BB 08", Code.Btc_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F BA 38 A5", Code.Btc_rm16_imm8, true };
				yield return new object[] { "F3 F0 0F BB 08", Code.Btc_rm32_r32, true };
				yield return new object[] { "F3 F0 0F BA 38 A5", Code.Btc_rm32_imm8, true };
				yield return new object[] { "F3 F0 48 0F BB 08", Code.Btc_rm64_r64, true };
				yield return new object[] { "F3 F0 48 0F BA 38 A5", Code.Btc_rm64_imm8, true };

				yield return new object[] { "F3 F0 66 0F B3 08", Code.Btr_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F BA 30 A5", Code.Btr_rm16_imm8, true };
				yield return new object[] { "F3 F0 0F B3 08", Code.Btr_rm32_r32, true };
				yield return new object[] { "F3 F0 0F BA 30 A5", Code.Btr_rm32_imm8, true };
				yield return new object[] { "F3 F0 48 0F B3 08", Code.Btr_rm64_r64, true };
				yield return new object[] { "F3 F0 48 0F BA 30 A5", Code.Btr_rm64_imm8, true };

				yield return new object[] { "F3 F0 66 0F AB 08", Code.Bts_rm16_r16, true };
				yield return new object[] { "F3 F0 66 0F BA 28 A5", Code.Bts_rm16_imm8, true };
				yield return new object[] { "F3 F0 0F AB 08", Code.Bts_rm32_r32, true };
				yield return new object[] { "F3 F0 0F BA 28 A5", Code.Bts_rm32_imm8, true };
				yield return new object[] { "F3 F0 48 0F AB 08", Code.Bts_rm64_r64, true };
				yield return new object[] { "F3 F0 48 0F BA 28 A5", Code.Bts_rm64_imm8, true };

				yield return new object[] { "F3 F0 FE 08", Code.Dec_rm8, true };
				yield return new object[] { "F3 F0 66 FF 08", Code.Dec_rm16, true };
				yield return new object[] { "F3 F0 FF 08", Code.Dec_rm32, true };
				yield return new object[] { "F3 F0 48 FF 08", Code.Dec_rm64, true };

				yield return new object[] { "F3 F0 FE 00", Code.Inc_rm8, true };
				yield return new object[] { "F3 F0 66 FF 00", Code.Inc_rm16, true };
				yield return new object[] { "F3 F0 FF 00", Code.Inc_rm32, true };
				yield return new object[] { "F3 F0 48 FF 00", Code.Inc_rm64, true };

				yield return new object[] { "F3 F0 F6 18", Code.Neg_rm8, true };
				yield return new object[] { "F3 F0 66 F7 18", Code.Neg_rm16, true };
				yield return new object[] { "F3 F0 F7 18", Code.Neg_rm32, true };
				yield return new object[] { "F3 F0 48 F7 18", Code.Neg_rm64, true };

				yield return new object[] { "F3 F0 F6 10", Code.Not_rm8, true };
				yield return new object[] { "F3 F0 66 F7 10", Code.Not_rm16, true };
				yield return new object[] { "F3 F0 F7 10", Code.Not_rm32, true };
				yield return new object[] { "F3 F0 48 F7 10", Code.Not_rm64, true };

				yield return new object[] { "F3 F0 0F B0 08", Code.Cmpxchg_rm8_r8, true };
				yield return new object[] { "F3 F0 66 0F B1 08", Code.Cmpxchg_rm16_r16, true };
				yield return new object[] { "F3 F0 0F B1 08", Code.Cmpxchg_rm32_r32, true };
				yield return new object[] { "F3 F0 48 0F B1 08", Code.Cmpxchg_rm64_r64, true };

				yield return new object[] { "F3 F0 0F C0 08", Code.Xadd_rm8_r8, true };
				yield return new object[] { "F3 F0 66 0F C1 08", Code.Xadd_rm16_r16, true };
				yield return new object[] { "F3 F0 0F C1 08", Code.Xadd_rm32_r32, true };
				yield return new object[] { "F3 F0 48 0F C1 08", Code.Xadd_rm64_r64, true };

				yield return new object[] { "F3 86 08", Code.Xchg_rm8_r8, false };
				yield return new object[] { "F3 66 87 08", Code.Xchg_rm16_r16, false };
				yield return new object[] { "F3 87 08", Code.Xchg_rm32_r32, false };
				yield return new object[] { "F3 48 87 08", Code.Xchg_rm64_r64, false };

				yield return new object[] { "F3 F0 86 08", Code.Xchg_rm8_r8, true };
				yield return new object[] { "F3 F0 66 87 08", Code.Xchg_rm16_r16, true };
				yield return new object[] { "F3 F0 87 08", Code.Xchg_rm32_r32, true };
				yield return new object[] { "F3 F0 48 87 08", Code.Xchg_rm64_r64, true };

				yield return new object[] { "F3 F0 0F C7 08", Code.Cmpxchg8b_m64, true };

				yield return new object[] { "F3 88 08", Code.Mov_rm8_r8, false };
				yield return new object[] { "F3 66 89 08", Code.Mov_rm16_r16, false };
				yield return new object[] { "F3 89 08", Code.Mov_rm32_r32, false };
				yield return new object[] { "F3 48 89 08", Code.Mov_rm64_r64, false };

				yield return new object[] { "F3 C6 00 A5", Code.Mov_rm8_imm8, false };
				yield return new object[] { "F3 66 C7 00 A5FF", Code.Mov_rm16_imm16, false };
				yield return new object[] { "F3 C7 00 A5FFFFFF", Code.Mov_rm32_imm32, false };
				yield return new object[] { "F3 48 C7 00 A5FFFFFF", Code.Mov_rm64_imm32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_RepPrefix_Data))]
		void Test16_RepPrefix(string hexBytes, Code code, bool repe, bool repne) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(repe, instr.HasRepePrefix);
			Assert.Equal(repne, instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}
		public static IEnumerable<object[]> Test16_RepPrefix_Data {
			get {
				yield return new object[] { "F3 6C", Code.Insb_m8_DX, true, false };
				yield return new object[] { "F3 6D", Code.Insw_m16_DX, true, false };
				yield return new object[] { "F3 66 6D", Code.Insd_m32_DX, true, false };
				yield return new object[] { "F3 6E", Code.Outsb_DX_m8, true, false };
				yield return new object[] { "F3 6F", Code.Outsw_DX_m16, true, false };
				yield return new object[] { "F3 66 6F", Code.Outsd_DX_m32, true, false };
				yield return new object[] { "F3 A4", Code.Movsb_m8_m8, true, false };
				yield return new object[] { "F3 A5", Code.Movsw_m16_m16, true, false };
				yield return new object[] { "F3 66 A5", Code.Movsd_m32_m32, true, false };
				yield return new object[] { "F3 A6", Code.Cmpsb_m8_m8, true, false };
				yield return new object[] { "F2 A6", Code.Cmpsb_m8_m8, false, true };
				yield return new object[] { "F3 A7", Code.Cmpsw_m16_m16, true, false };
				yield return new object[] { "F2 A7", Code.Cmpsw_m16_m16, false, true };
				yield return new object[] { "F3 66 A7", Code.Cmpsd_m32_m32, true, false };
				yield return new object[] { "F2 66 A7", Code.Cmpsd_m32_m32, false, true };
				yield return new object[] { "F3 AA", Code.Stosb_m8_AL, true, false };
				yield return new object[] { "F3 AB", Code.Stosw_m16_AX, true, false };
				yield return new object[] { "F3 66 AB", Code.Stosd_m32_EAX, true, false };
				yield return new object[] { "F3 AC", Code.Lodsb_AL_m8, true, false };
				yield return new object[] { "F3 AD", Code.Lodsw_AX_m16, true, false };
				yield return new object[] { "F3 66 AD", Code.Lodsd_EAX_m32, true, false };
				yield return new object[] { "F3 AE", Code.Scasb_AL_m8, true, false };
				yield return new object[] { "F2 AE", Code.Scasb_AL_m8, false, true };
				yield return new object[] { "F3 AF", Code.Scasw_AX_m16, true, false };
				yield return new object[] { "F2 AF", Code.Scasw_AX_m16, false, true };
				yield return new object[] { "F3 66 AF", Code.Scasd_EAX_m32, true, false };
				yield return new object[] { "F2 66 AF", Code.Scasd_EAX_m32, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_RepPrefix_Data))]
		void Test32_RepPrefix(string hexBytes, Code code, bool repe, bool repne) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(repe, instr.HasRepePrefix);
			Assert.Equal(repne, instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}
		public static IEnumerable<object[]> Test32_RepPrefix_Data {
			get {
				yield return new object[] { "F3 6C", Code.Insb_m8_DX, true, false };
				yield return new object[] { "F3 66 6D", Code.Insw_m16_DX, true, false };
				yield return new object[] { "F3 6D", Code.Insd_m32_DX, true, false };
				yield return new object[] { "F3 6E", Code.Outsb_DX_m8, true, false };
				yield return new object[] { "F3 66 6F", Code.Outsw_DX_m16, true, false };
				yield return new object[] { "F3 6F", Code.Outsd_DX_m32, true, false };
				yield return new object[] { "F3 A4", Code.Movsb_m8_m8, true, false };
				yield return new object[] { "F3 66 A5", Code.Movsw_m16_m16, true, false };
				yield return new object[] { "F3 A5", Code.Movsd_m32_m32, true, false };
				yield return new object[] { "F3 A6", Code.Cmpsb_m8_m8, true, false };
				yield return new object[] { "F2 A6", Code.Cmpsb_m8_m8, false, true };
				yield return new object[] { "F3 66 A7", Code.Cmpsw_m16_m16, true, false };
				yield return new object[] { "F2 66 A7", Code.Cmpsw_m16_m16, false, true };
				yield return new object[] { "F3 A7", Code.Cmpsd_m32_m32, true, false };
				yield return new object[] { "F2 A7", Code.Cmpsd_m32_m32, false, true };
				yield return new object[] { "F3 AA", Code.Stosb_m8_AL, true, false };
				yield return new object[] { "F3 66 AB", Code.Stosw_m16_AX, true, false };
				yield return new object[] { "F3 AB", Code.Stosd_m32_EAX, true, false };
				yield return new object[] { "F3 AC", Code.Lodsb_AL_m8, true, false };
				yield return new object[] { "F3 66 AD", Code.Lodsw_AX_m16, true, false };
				yield return new object[] { "F3 AD", Code.Lodsd_EAX_m32, true, false };
				yield return new object[] { "F3 AE", Code.Scasb_AL_m8, true, false };
				yield return new object[] { "F2 AE", Code.Scasb_AL_m8, false, true };
				yield return new object[] { "F3 66 AF", Code.Scasw_AX_m16, true, false };
				yield return new object[] { "F2 66 AF", Code.Scasw_AX_m16, false, true };
				yield return new object[] { "F3 AF", Code.Scasd_EAX_m32, true, false };
				yield return new object[] { "F2 AF", Code.Scasd_EAX_m32, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_RepPrefix_Data))]
		void Test64_RepPrefix(string hexBytes, Code code, bool repe, bool repne) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(repe, instr.HasRepePrefix);
			Assert.Equal(repne, instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.False(instr.HasXacquirePrefix);
			Assert.False(instr.HasXreleasePrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);
		}
		public static IEnumerable<object[]> Test64_RepPrefix_Data {
			get {
				yield return new object[] { "F3 6C", Code.Insb_m8_DX, true, false };
				yield return new object[] { "F3 66 6D", Code.Insw_m16_DX, true, false };
				yield return new object[] { "F3 6D", Code.Insd_m32_DX, true, false };
				yield return new object[] { "F3 6E", Code.Outsb_DX_m8, true, false };
				yield return new object[] { "F3 66 6F", Code.Outsw_DX_m16, true, false };
				yield return new object[] { "F3 6F", Code.Outsd_DX_m32, true, false };
				yield return new object[] { "F3 A4", Code.Movsb_m8_m8, true, false };
				yield return new object[] { "F3 66 A5", Code.Movsw_m16_m16, true, false };
				yield return new object[] { "F3 A5", Code.Movsd_m32_m32, true, false };
				yield return new object[] { "F3 48 A5", Code.Movsq_m64_m64, true, false };
				yield return new object[] { "F3 A6", Code.Cmpsb_m8_m8, true, false };
				yield return new object[] { "F2 A6", Code.Cmpsb_m8_m8, false, true };
				yield return new object[] { "F3 66 A7", Code.Cmpsw_m16_m16, true, false };
				yield return new object[] { "F2 66 A7", Code.Cmpsw_m16_m16, false, true };
				yield return new object[] { "F3 A7", Code.Cmpsd_m32_m32, true, false };
				yield return new object[] { "F2 A7", Code.Cmpsd_m32_m32, false, true };
				yield return new object[] { "F3 48 A7", Code.Cmpsq_m64_m64, true, false };
				yield return new object[] { "F2 48 A7", Code.Cmpsq_m64_m64, false, true };
				yield return new object[] { "F3 AA", Code.Stosb_m8_AL, true, false };
				yield return new object[] { "F3 66 AB", Code.Stosw_m16_AX, true, false };
				yield return new object[] { "F3 AB", Code.Stosd_m32_EAX, true, false };
				yield return new object[] { "F3 48 AB", Code.Stosq_m64_RAX, true, false };
				yield return new object[] { "F3 AC", Code.Lodsb_AL_m8, true, false };
				yield return new object[] { "F3 66 AD", Code.Lodsw_AX_m16, true, false };
				yield return new object[] { "F3 AD", Code.Lodsd_EAX_m32, true, false };
				yield return new object[] { "F3 48 AD", Code.Lodsq_RAX_m64, true, false };
				yield return new object[] { "F3 AE", Code.Scasb_AL_m8, true, false };
				yield return new object[] { "F2 AE", Code.Scasb_AL_m8, false, true };
				yield return new object[] { "F3 66 AF", Code.Scasw_AX_m16, true, false };
				yield return new object[] { "F2 66 AF", Code.Scasw_AX_m16, false, true };
				yield return new object[] { "F3 AF", Code.Scasd_EAX_m32, true, false };
				yield return new object[] { "F2 AF", Code.Scasd_EAX_m32, false, true };
				yield return new object[] { "F3 48 AF", Code.Scasq_RAX_m64, true, false };
				yield return new object[] { "F2 48 AF", Code.Scasq_RAX_m64, false, true };
			}
		}
	}
}
