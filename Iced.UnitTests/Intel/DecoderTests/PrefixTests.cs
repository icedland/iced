/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class PrefixTests : DecoderTest {
		static void Dummy(string s) { }

		[Theory]
		[InlineData("66 66 01 CE", 4, Code.Add_rm32_r32, "66 01 CE")]
		void Test16_double_66_is_same_as_one(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("66 66 01 CE", 4, Code.Add_rm16_r16, "66 01 CE")]
		void Test32_double_66_is_same_as_one(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("66 66 01 CE", 4, Code.Add_rm16_r16, "66 01 CE")]
		void Test64_double_66_is_same_as_one(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("67 67 8D 18", 4, Code.Lea_r16_m, "67 8D 18")]
		void Test16_double_67_is_same_as_one(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("67 67 8D 18", 4, Code.Lea_r32_m, "67 8D 18")]
		void Test32_double_67_is_same_as_one(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("67 67 8D 18", 4, Code.Lea_r32_m, "67 8D 18")]
		void Test64_double_67_is_same_as_one(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("26 26 01 18", 4, Code.Add_rm16_r16, Register.ES, "26 01 18")]
		[InlineData("26 2E 01 18", 4, Code.Add_rm16_r16, Register.CS, "2E 01 18")]
		[InlineData("26 36 01 18", 4, Code.Add_rm16_r16, Register.SS, "36 01 18")]
		[InlineData("26 3E 01 18", 4, Code.Add_rm16_r16, Register.DS, "3E 01 18")]
		[InlineData("26 64 01 18", 4, Code.Add_rm16_r16, Register.FS, "64 01 18")]
		[InlineData("26 65 01 18", 4, Code.Add_rm16_r16, Register.GS, "65 01 18")]

		[InlineData("2E 26 01 18", 4, Code.Add_rm16_r16, Register.ES, "26 01 18")]
		[InlineData("2E 2E 01 18", 4, Code.Add_rm16_r16, Register.CS, "2E 01 18")]
		[InlineData("2E 36 01 18", 4, Code.Add_rm16_r16, Register.SS, "36 01 18")]
		[InlineData("2E 3E 01 18", 4, Code.Add_rm16_r16, Register.DS, "3E 01 18")]
		[InlineData("2E 64 01 18", 4, Code.Add_rm16_r16, Register.FS, "64 01 18")]
		[InlineData("2E 65 01 18", 4, Code.Add_rm16_r16, Register.GS, "65 01 18")]

		[InlineData("36 26 01 18", 4, Code.Add_rm16_r16, Register.ES, "26 01 18")]
		[InlineData("36 2E 01 18", 4, Code.Add_rm16_r16, Register.CS, "2E 01 18")]
		[InlineData("36 36 01 18", 4, Code.Add_rm16_r16, Register.SS, "36 01 18")]
		[InlineData("36 3E 01 18", 4, Code.Add_rm16_r16, Register.DS, "3E 01 18")]
		[InlineData("36 64 01 18", 4, Code.Add_rm16_r16, Register.FS, "64 01 18")]
		[InlineData("36 65 01 18", 4, Code.Add_rm16_r16, Register.GS, "65 01 18")]

		[InlineData("3E 26 01 18", 4, Code.Add_rm16_r16, Register.ES, "26 01 18")]
		[InlineData("3E 2E 01 18", 4, Code.Add_rm16_r16, Register.CS, "2E 01 18")]
		[InlineData("3E 36 01 18", 4, Code.Add_rm16_r16, Register.SS, "36 01 18")]
		[InlineData("3E 3E 01 18", 4, Code.Add_rm16_r16, Register.DS, "3E 01 18")]
		[InlineData("3E 64 01 18", 4, Code.Add_rm16_r16, Register.FS, "64 01 18")]
		[InlineData("3E 65 01 18", 4, Code.Add_rm16_r16, Register.GS, "65 01 18")]

		[InlineData("64 26 01 18", 4, Code.Add_rm16_r16, Register.ES, "26 01 18")]
		[InlineData("64 2E 01 18", 4, Code.Add_rm16_r16, Register.CS, "2E 01 18")]
		[InlineData("64 36 01 18", 4, Code.Add_rm16_r16, Register.SS, "36 01 18")]
		[InlineData("64 3E 01 18", 4, Code.Add_rm16_r16, Register.DS, "3E 01 18")]
		[InlineData("64 64 01 18", 4, Code.Add_rm16_r16, Register.FS, "64 01 18")]
		[InlineData("64 65 01 18", 4, Code.Add_rm16_r16, Register.GS, "65 01 18")]

		[InlineData("65 26 01 18", 4, Code.Add_rm16_r16, Register.ES, "26 01 18")]
		[InlineData("65 2E 01 18", 4, Code.Add_rm16_r16, Register.CS, "2E 01 18")]
		[InlineData("65 36 01 18", 4, Code.Add_rm16_r16, Register.SS, "36 01 18")]
		[InlineData("65 3E 01 18", 4, Code.Add_rm16_r16, Register.DS, "3E 01 18")]
		[InlineData("65 64 01 18", 4, Code.Add_rm16_r16, Register.FS, "64 01 18")]
		[InlineData("65 65 01 18", 4, Code.Add_rm16_r16, Register.GS, "65 01 18")]
		void Test16_extra_segment_overrides(string hexBytes, int length, Code code, Register segReg, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("26 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("26 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("26 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("26 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("26 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("26 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("2E 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("2E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("2E 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("2E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("2E 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("2E 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("36 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("36 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("36 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("36 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("36 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("36 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("3E 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("3E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("3E 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("3E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("3E 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("3E 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("64 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("64 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("64 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("64 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("64 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("64 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("65 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("65 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("65 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("65 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("65 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("65 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]
		void Test32_extra_segment_overrides(string hexBytes, int length, Code code, Register segReg, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("26 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("26 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("26 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("26 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("26 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("26 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("2E 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("2E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("2E 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("2E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("2E 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("2E 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("36 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("36 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("36 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("36 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("36 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("36 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("3E 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("3E 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("3E 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("3E 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("3E 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("3E 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("64 26 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("64 2E 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("64 36 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("64 3E 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("64 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("64 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]

		[InlineData("65 26 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]
		[InlineData("65 2E 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]
		[InlineData("65 36 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]
		[InlineData("65 3E 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]
		[InlineData("65 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("65 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]
		void Test64_extra_segment_overrides(string hexBytes, int length, Code code, Register segReg, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("66 48 01 CE", 4, Code.Add_rm64_r64, "48 01 CE")]
		void Test64_REX_W_overrides_66(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("4F 48 01 CE", 4, Code.Add_rm64_r64, Register.RSI, Register.RCX, "48 01 CE")]
		[InlineData("4F 4C 01 C5", 4, Code.Add_rm64_r64, Register.RBP, Register.R8, "4C 01 C5")]
		[InlineData("4F 49 01 D6", 4, Code.Add_rm64_r64, Register.R14, Register.RDX, "49 01 D6")]
		[InlineData("4F 4D 01 D0", 4, Code.Add_rm64_r64, Register.R8, Register.R10, "4D 01 D0")]
		[InlineData("4F 49 01 D9", 4, Code.Add_rm64_r64, Register.R9, Register.RBX, "49 01 D9")]
		[InlineData("4F 4C 01 EC", 4, Code.Add_rm64_r64, Register.RSP, Register.R13, "4C 01 EC")]
		void Test64_double_REX_prefixes(string hexBytes, int length, Code code, Register reg1, Register reg2, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("48 66 01 CE", 4, Code.Add_rm16_r16, "66 01 CE")]
		[InlineData("4F 66 01 CE", 4, Code.Add_rm16_r16, "66 01 CE")]
		void Test64_REX_prefix_before_66(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("48 67 01 18", 4, Code.Add_rm32_r32, "67 01 18")]
		void Test64_REX_before_67(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("49 F0 01 18", 4, Code.Add_rm32_r32, "F0 01 18")]
		void Test64_REX_before_F0(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("4D F2 01 18", 4, Code.Add_rm32_r32, "F2 01 18")]
		void Test64_REX_before_F2(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("4F F3 01 18", 4, Code.Add_rm32_r32, "F3 01 18")]
		void Test64_REX_before_F3(string hexBytes, int length, Code code, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
		[InlineData("48 26 01 18", 4, Code.Add_rm32_r32, Register.ES, "26 01 18")]
		[InlineData("49 2E 01 18", 4, Code.Add_rm32_r32, Register.CS, "2E 01 18")]
		[InlineData("4A 36 01 18", 4, Code.Add_rm32_r32, Register.SS, "36 01 18")]
		[InlineData("4B 3E 01 18", 4, Code.Add_rm32_r32, Register.DS, "3E 01 18")]
		[InlineData("4C 64 01 18", 4, Code.Add_rm32_r32, Register.FS, "64 01 18")]
		[InlineData("4F 65 01 18", 4, Code.Add_rm32_r32, Register.GS, "65 01 18")]
		void Test64_REX_before_segment_override(string hexBytes, int length, Code code, Register segReg, string encodedBytes) {
			Dummy(encodedBytes);
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(length, instr.Length);
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
	}
}
