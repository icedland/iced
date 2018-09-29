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
		[InlineData("F3 0F1E FA", 4, Code.Endbr64, DecoderOptions.None)]
		[InlineData("F3 0F1E FB", 4, Code.Endbr32, DecoderOptions.None)]
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
		[InlineData("F3 0F1E FA", 4, Code.Endbr64, DecoderOptions.None)]
		[InlineData("F3 0F1E FB", 4, Code.Endbr32, DecoderOptions.None)]
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
		[InlineData("F3 0F1E FA", 4, Code.Endbr64, DecoderOptions.None)]
		[InlineData("F3 0F1E FB", 4, Code.Endbr32, DecoderOptions.None)]
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

		[Theory]
		[MemberData(nameof(Test16_ReservedNop_E_G_1_Data))]
		void Test16_ReservedNop_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
		public static IEnumerable<object[]> Test16_ReservedNop_E_G_1_Data {
			get {
				yield return new object[] { "0F18 CE", 3, Code.ReservedNop_rm16_r16_0F18, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F18 CE", 4, Code.ReservedNop_rm32_r32_0F18, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F19 CE", 3, Code.ReservedNop_rm16_r16_0F19, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F19 CE", 4, Code.ReservedNop_rm32_r32_0F19, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1A CE", 3, Code.ReservedNop_rm16_r16_0F1A, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1A CE", 4, Code.ReservedNop_rm32_r32_0F1A, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1B CE", 3, Code.ReservedNop_rm16_r16_0F1B, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1B CE", 4, Code.ReservedNop_rm32_r32_0F1B, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1C CE", 3, Code.ReservedNop_rm16_r16_0F1C, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1C CE", 4, Code.ReservedNop_rm32_r32_0F1C, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1D CE", 3, Code.ReservedNop_rm16_r16_0F1D, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1D CE", 4, Code.ReservedNop_rm32_r32_0F1D, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1E CE", 3, Code.ReservedNop_rm16_r16_0F1E, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1E CE", 4, Code.ReservedNop_rm32_r32_0F1E, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1F CE", 3, Code.ReservedNop_rm16_r16_0F1F, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1F CE", 4, Code.ReservedNop_rm32_r32_0F1F, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_ReservedNop_E_G_2_Data))]
		void Test16_ReservedNop_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_ReservedNop_E_G_2_Data {
			get {
				yield return new object[] { "0F18 18", 3, Code.ReservedNop_rm16_r16_0F18, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F18 18", 4, Code.ReservedNop_rm32_r32_0F18, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F19 18", 3, Code.ReservedNop_rm16_r16_0F19, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F19 18", 4, Code.ReservedNop_rm32_r32_0F19, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1A 18", 3, Code.ReservedNop_rm16_r16_0F1A, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1A 18", 4, Code.ReservedNop_rm32_r32_0F1A, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1B 18", 3, Code.ReservedNop_rm16_r16_0F1B, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1B 18", 4, Code.ReservedNop_rm32_r32_0F1B, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1C 18", 3, Code.ReservedNop_rm16_r16_0F1C, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1C 18", 4, Code.ReservedNop_rm32_r32_0F1C, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1D 18", 3, Code.ReservedNop_rm16_r16_0F1D, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1D 18", 4, Code.ReservedNop_rm32_r32_0F1D, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1E 18", 3, Code.ReservedNop_rm16_r16_0F1E, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1E 18", 4, Code.ReservedNop_rm32_r32_0F1E, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1F 18", 3, Code.ReservedNop_rm16_r16_0F1F, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 0F1F 18", 4, Code.ReservedNop_rm32_r32_0F1F, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ReservedNop_E_G_1_Data))]
		void Test32_ReservedNop_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
		public static IEnumerable<object[]> Test32_ReservedNop_E_G_1_Data {
			get {
				yield return new object[] { "66 0F18 CE", 4, Code.ReservedNop_rm16_r16_0F18, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F18 CE", 3, Code.ReservedNop_rm32_r32_0F18, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F19 CE", 4, Code.ReservedNop_rm16_r16_0F19, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F19 CE", 3, Code.ReservedNop_rm32_r32_0F19, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1A CE", 4, Code.ReservedNop_rm16_r16_0F1A, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1A CE", 3, Code.ReservedNop_rm32_r32_0F1A, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1B CE", 4, Code.ReservedNop_rm16_r16_0F1B, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1B CE", 3, Code.ReservedNop_rm32_r32_0F1B, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1C CE", 4, Code.ReservedNop_rm16_r16_0F1C, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1C CE", 3, Code.ReservedNop_rm32_r32_0F1C, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1D CE", 4, Code.ReservedNop_rm16_r16_0F1D, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1D CE", 3, Code.ReservedNop_rm32_r32_0F1D, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1E CE", 4, Code.ReservedNop_rm16_r16_0F1E, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1E CE", 3, Code.ReservedNop_rm32_r32_0F1E, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1F CE", 4, Code.ReservedNop_rm16_r16_0F1F, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1F CE", 3, Code.ReservedNop_rm32_r32_0F1F, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_ReservedNop_E_G_2_Data))]
		void Test32_ReservedNop_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_ReservedNop_E_G_2_Data {
			get {
				yield return new object[] { "66 0F18 18", 4, Code.ReservedNop_rm16_r16_0F18, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F18 18", 3, Code.ReservedNop_rm32_r32_0F18, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F19 18", 4, Code.ReservedNop_rm16_r16_0F19, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F19 18", 3, Code.ReservedNop_rm32_r32_0F19, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1A 18", 4, Code.ReservedNop_rm16_r16_0F1A, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1A 18", 3, Code.ReservedNop_rm32_r32_0F1A, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1B 18", 4, Code.ReservedNop_rm16_r16_0F1B, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1B 18", 3, Code.ReservedNop_rm32_r32_0F1B, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1C 18", 4, Code.ReservedNop_rm16_r16_0F1C, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1C 18", 3, Code.ReservedNop_rm32_r32_0F1C, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1D 18", 4, Code.ReservedNop_rm16_r16_0F1D, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1D 18", 3, Code.ReservedNop_rm32_r32_0F1D, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1E 18", 4, Code.ReservedNop_rm16_r16_0F1E, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1E 18", 3, Code.ReservedNop_rm32_r32_0F1E, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1F 18", 4, Code.ReservedNop_rm16_r16_0F1F, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1F 18", 3, Code.ReservedNop_rm32_r32_0F1F, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ReservedNop_E_G_1_Data))]
		void Test64_ReservedNop_E_G_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
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
		public static IEnumerable<object[]> Test64_ReservedNop_E_G_1_Data {
			get {
				yield return new object[] { "66 0F18 CE", 4, Code.ReservedNop_rm16_r16_0F18, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F18 C5", 5, Code.ReservedNop_rm16_r16_0F18, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F18 D6", 5, Code.ReservedNop_rm16_r16_0F18, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F18 D0", 5, Code.ReservedNop_rm16_r16_0F18, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F18 D9", 5, Code.ReservedNop_rm16_r16_0F18, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F18 EC", 5, Code.ReservedNop_rm16_r16_0F18, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F18 CE", 3, Code.ReservedNop_rm32_r32_0F18, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F18 C5", 4, Code.ReservedNop_rm32_r32_0F18, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F18 D6", 4, Code.ReservedNop_rm32_r32_0F18, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F18 D0", 4, Code.ReservedNop_rm32_r32_0F18, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F18 D9", 4, Code.ReservedNop_rm32_r32_0F18, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F18 EC", 4, Code.ReservedNop_rm32_r32_0F18, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F18 CE", 4, Code.ReservedNop_rm64_r64_0F18, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F18 C5", 4, Code.ReservedNop_rm64_r64_0F18, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F18 D6", 4, Code.ReservedNop_rm64_r64_0F18, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F18 D0", 4, Code.ReservedNop_rm64_r64_0F18, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F18 D9", 4, Code.ReservedNop_rm64_r64_0F18, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F18 EC", 4, Code.ReservedNop_rm64_r64_0F18, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F19 CE", 4, Code.ReservedNop_rm16_r16_0F19, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F19 C5", 5, Code.ReservedNop_rm16_r16_0F19, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F19 D6", 5, Code.ReservedNop_rm16_r16_0F19, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F19 D0", 5, Code.ReservedNop_rm16_r16_0F19, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F19 D9", 5, Code.ReservedNop_rm16_r16_0F19, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F19 EC", 5, Code.ReservedNop_rm16_r16_0F19, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F19 CE", 3, Code.ReservedNop_rm32_r32_0F19, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F19 C5", 4, Code.ReservedNop_rm32_r32_0F19, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F19 D6", 4, Code.ReservedNop_rm32_r32_0F19, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F19 D0", 4, Code.ReservedNop_rm32_r32_0F19, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F19 D9", 4, Code.ReservedNop_rm32_r32_0F19, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F19 EC", 4, Code.ReservedNop_rm32_r32_0F19, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F19 CE", 4, Code.ReservedNop_rm64_r64_0F19, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F19 C5", 4, Code.ReservedNop_rm64_r64_0F19, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F19 D6", 4, Code.ReservedNop_rm64_r64_0F19, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F19 D0", 4, Code.ReservedNop_rm64_r64_0F19, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F19 D9", 4, Code.ReservedNop_rm64_r64_0F19, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F19 EC", 4, Code.ReservedNop_rm64_r64_0F19, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1A CE", 4, Code.ReservedNop_rm16_r16_0F1A, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1A C5", 5, Code.ReservedNop_rm16_r16_0F1A, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1A D6", 5, Code.ReservedNop_rm16_r16_0F1A, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F1A D0", 5, Code.ReservedNop_rm16_r16_0F1A, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1A D9", 5, Code.ReservedNop_rm16_r16_0F1A, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1A EC", 5, Code.ReservedNop_rm16_r16_0F1A, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1A CE", 3, Code.ReservedNop_rm32_r32_0F1A, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1A C5", 4, Code.ReservedNop_rm32_r32_0F1A, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1A D6", 4, Code.ReservedNop_rm32_r32_0F1A, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F1A D0", 4, Code.ReservedNop_rm32_r32_0F1A, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1A D9", 4, Code.ReservedNop_rm32_r32_0F1A, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1A EC", 4, Code.ReservedNop_rm32_r32_0F1A, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F1A CE", 4, Code.ReservedNop_rm64_r64_0F1A, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1A C5", 4, Code.ReservedNop_rm64_r64_0F1A, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1A D6", 4, Code.ReservedNop_rm64_r64_0F1A, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F1A D0", 4, Code.ReservedNop_rm64_r64_0F1A, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1A D9", 4, Code.ReservedNop_rm64_r64_0F1A, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1A EC", 4, Code.ReservedNop_rm64_r64_0F1A, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1B CE", 4, Code.ReservedNop_rm16_r16_0F1B, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1B C5", 5, Code.ReservedNop_rm16_r16_0F1B, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1B D6", 5, Code.ReservedNop_rm16_r16_0F1B, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F1B D0", 5, Code.ReservedNop_rm16_r16_0F1B, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1B D9", 5, Code.ReservedNop_rm16_r16_0F1B, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1B EC", 5, Code.ReservedNop_rm16_r16_0F1B, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1B CE", 3, Code.ReservedNop_rm32_r32_0F1B, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1B C5", 4, Code.ReservedNop_rm32_r32_0F1B, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1B D6", 4, Code.ReservedNop_rm32_r32_0F1B, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F1B D0", 4, Code.ReservedNop_rm32_r32_0F1B, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1B D9", 4, Code.ReservedNop_rm32_r32_0F1B, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1B EC", 4, Code.ReservedNop_rm32_r32_0F1B, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F1B CE", 4, Code.ReservedNop_rm64_r64_0F1B, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1B C5", 4, Code.ReservedNop_rm64_r64_0F1B, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1B D6", 4, Code.ReservedNop_rm64_r64_0F1B, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F1B D0", 4, Code.ReservedNop_rm64_r64_0F1B, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1B D9", 4, Code.ReservedNop_rm64_r64_0F1B, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1B EC", 4, Code.ReservedNop_rm64_r64_0F1B, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1C CE", 4, Code.ReservedNop_rm16_r16_0F1C, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1C C5", 5, Code.ReservedNop_rm16_r16_0F1C, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1C D6", 5, Code.ReservedNop_rm16_r16_0F1C, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F1C D0", 5, Code.ReservedNop_rm16_r16_0F1C, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1C D9", 5, Code.ReservedNop_rm16_r16_0F1C, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1C EC", 5, Code.ReservedNop_rm16_r16_0F1C, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1C CE", 3, Code.ReservedNop_rm32_r32_0F1C, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1C C5", 4, Code.ReservedNop_rm32_r32_0F1C, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1C D6", 4, Code.ReservedNop_rm32_r32_0F1C, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F1C D0", 4, Code.ReservedNop_rm32_r32_0F1C, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1C D9", 4, Code.ReservedNop_rm32_r32_0F1C, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1C EC", 4, Code.ReservedNop_rm32_r32_0F1C, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F1C CE", 4, Code.ReservedNop_rm64_r64_0F1C, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1C C5", 4, Code.ReservedNop_rm64_r64_0F1C, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1C D6", 4, Code.ReservedNop_rm64_r64_0F1C, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F1C D0", 4, Code.ReservedNop_rm64_r64_0F1C, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1C D9", 4, Code.ReservedNop_rm64_r64_0F1C, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1C EC", 4, Code.ReservedNop_rm64_r64_0F1C, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1D CE", 4, Code.ReservedNop_rm16_r16_0F1D, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1D C5", 5, Code.ReservedNop_rm16_r16_0F1D, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1D D6", 5, Code.ReservedNop_rm16_r16_0F1D, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F1D D0", 5, Code.ReservedNop_rm16_r16_0F1D, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1D D9", 5, Code.ReservedNop_rm16_r16_0F1D, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1D EC", 5, Code.ReservedNop_rm16_r16_0F1D, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1D CE", 3, Code.ReservedNop_rm32_r32_0F1D, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1D C5", 4, Code.ReservedNop_rm32_r32_0F1D, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1D D6", 4, Code.ReservedNop_rm32_r32_0F1D, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F1D D0", 4, Code.ReservedNop_rm32_r32_0F1D, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1D D9", 4, Code.ReservedNop_rm32_r32_0F1D, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1D EC", 4, Code.ReservedNop_rm32_r32_0F1D, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F1D CE", 4, Code.ReservedNop_rm64_r64_0F1D, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1D C5", 4, Code.ReservedNop_rm64_r64_0F1D, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1D D6", 4, Code.ReservedNop_rm64_r64_0F1D, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F1D D0", 4, Code.ReservedNop_rm64_r64_0F1D, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1D D9", 4, Code.ReservedNop_rm64_r64_0F1D, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1D EC", 4, Code.ReservedNop_rm64_r64_0F1D, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1E CE", 4, Code.ReservedNop_rm16_r16_0F1E, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1E C5", 5, Code.ReservedNop_rm16_r16_0F1E, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1E D6", 5, Code.ReservedNop_rm16_r16_0F1E, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F1E D0", 5, Code.ReservedNop_rm16_r16_0F1E, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1E D9", 5, Code.ReservedNop_rm16_r16_0F1E, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1E EC", 5, Code.ReservedNop_rm16_r16_0F1E, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1E CE", 3, Code.ReservedNop_rm32_r32_0F1E, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1E C5", 4, Code.ReservedNop_rm32_r32_0F1E, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1E D6", 4, Code.ReservedNop_rm32_r32_0F1E, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F1E D0", 4, Code.ReservedNop_rm32_r32_0F1E, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1E D9", 4, Code.ReservedNop_rm32_r32_0F1E, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1E EC", 4, Code.ReservedNop_rm32_r32_0F1E, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F1E CE", 4, Code.ReservedNop_rm64_r64_0F1E, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1E C5", 4, Code.ReservedNop_rm64_r64_0F1E, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1E D6", 4, Code.ReservedNop_rm64_r64_0F1E, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F1E D0", 4, Code.ReservedNop_rm64_r64_0F1E, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1E D9", 4, Code.ReservedNop_rm64_r64_0F1E, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1E EC", 4, Code.ReservedNop_rm64_r64_0F1E, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1F CE", 4, Code.ReservedNop_rm16_r16_0F1F, Register.SI, Register.CX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1F C5", 5, Code.ReservedNop_rm16_r16_0F1F, Register.BP, Register.R8W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1F D6", 5, Code.ReservedNop_rm16_r16_0F1F, Register.R14W, Register.DX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 45 0F1F D0", 5, Code.ReservedNop_rm16_r16_0F1F, Register.R8W, Register.R10W, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 41 0F1F D9", 5, Code.ReservedNop_rm16_r16_0F1F, Register.R9W, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "66 44 0F1F EC", 5, Code.ReservedNop_rm16_r16_0F1F, Register.SP, Register.R13W, DecoderOptions.ForceReservedNop };

				yield return new object[] { "0F1F CE", 3, Code.ReservedNop_rm32_r32_0F1F, Register.ESI, Register.ECX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1F C5", 4, Code.ReservedNop_rm32_r32_0F1F, Register.EBP, Register.R8D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1F D6", 4, Code.ReservedNop_rm32_r32_0F1F, Register.R14D, Register.EDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "45 0F1F D0", 4, Code.ReservedNop_rm32_r32_0F1F, Register.R8D, Register.R10D, DecoderOptions.ForceReservedNop };
				yield return new object[] { "41 0F1F D9", 4, Code.ReservedNop_rm32_r32_0F1F, Register.R9D, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "44 0F1F EC", 4, Code.ReservedNop_rm32_r32_0F1F, Register.ESP, Register.R13D, DecoderOptions.ForceReservedNop };

				yield return new object[] { "48 0F1F CE", 4, Code.ReservedNop_rm64_r64_0F1F, Register.RSI, Register.RCX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1F C5", 4, Code.ReservedNop_rm64_r64_0F1F, Register.RBP, Register.R8, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1F D6", 4, Code.ReservedNop_rm64_r64_0F1F, Register.R14, Register.RDX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4D 0F1F D0", 4, Code.ReservedNop_rm64_r64_0F1F, Register.R8, Register.R10, DecoderOptions.ForceReservedNop };
				yield return new object[] { "49 0F1F D9", 4, Code.ReservedNop_rm64_r64_0F1F, Register.R9, Register.RBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "4C 0F1F EC", 4, Code.ReservedNop_rm64_r64_0F1F, Register.RSP, Register.R13, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_ReservedNop_E_G_2_Data))]
		void Test64_ReservedNop_E_G_2(string hexBytes, int byteLength, Code code, MemorySize memSize, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
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
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_ReservedNop_E_G_2_Data {
			get {
				yield return new object[] { "66 0F18 18", 4, Code.ReservedNop_rm16_r16_0F18, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F18 18", 3, Code.ReservedNop_rm32_r32_0F18, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F18 18", 4, Code.ReservedNop_rm64_r64_0F18, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F19 18", 4, Code.ReservedNop_rm16_r16_0F19, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F19 18", 3, Code.ReservedNop_rm32_r32_0F19, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F19 18", 4, Code.ReservedNop_rm64_r64_0F19, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1A 18", 4, Code.ReservedNop_rm16_r16_0F1A, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1A 18", 3, Code.ReservedNop_rm32_r32_0F1A, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F1A 18", 4, Code.ReservedNop_rm64_r64_0F1A, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1B 18", 4, Code.ReservedNop_rm16_r16_0F1B, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1B 18", 3, Code.ReservedNop_rm32_r32_0F1B, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F1B 18", 4, Code.ReservedNop_rm64_r64_0F1B, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1C 18", 4, Code.ReservedNop_rm16_r16_0F1C, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1C 18", 3, Code.ReservedNop_rm32_r32_0F1C, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F1C 18", 4, Code.ReservedNop_rm64_r64_0F1C, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1D 18", 4, Code.ReservedNop_rm16_r16_0F1D, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1D 18", 3, Code.ReservedNop_rm32_r32_0F1D, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F1D 18", 4, Code.ReservedNop_rm64_r64_0F1D, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1E 18", 4, Code.ReservedNop_rm16_r16_0F1E, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1E 18", 3, Code.ReservedNop_rm32_r32_0F1E, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F1E 18", 4, Code.ReservedNop_rm64_r64_0F1E, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };

				yield return new object[] { "66 0F1F 18", 4, Code.ReservedNop_rm16_r16_0F1F, MemorySize.UInt16, Register.BX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "0F1F 18", 3, Code.ReservedNop_rm32_r32_0F1F, MemorySize.UInt32, Register.EBX, DecoderOptions.ForceReservedNop };
				yield return new object[] { "48 0F1F 18", 4, Code.ReservedNop_rm64_r64_0F1F, MemorySize.UInt64, Register.RBX, DecoderOptions.ForceReservedNop };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Prefetch_M_Reg_RegMem_1_Data))]
		void Test16_Prefetch_M_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
				yield return new object[] { "0F1A 08", 3, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.Unknown };

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
				yield return new object[] { "0F1A 08", 3, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.Unknown };

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
				yield return new object[] { "0F1A 08", 3, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.Unknown };
				yield return new object[] { "48 0F1A 08", 4, Code.Bndldx_bnd_mib, Register.BND1, MemorySize.Unknown };

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg1, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_BND_RegMem_Reg_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F1B 08", 3, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.Unknown };

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg1, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_BND_RegMem_Reg_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F1B 08", 3, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.Unknown };

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg1, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_BND_RegMem_Reg_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0F1B 08", 3, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.Unknown };
				yield return new object[] { "48 0F1B 08", 4, Code.Bndstx_mib_bnd, Register.BND1, MemorySize.Unknown };

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		[InlineData("0F1F C1", 3, Code.Nop_rm16, Register.CX)]
		void Test16_Grp_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F1F 00", 4, Code.Nop_rm16)]
		void Test32_Grp_Ew_1(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F1F C1", 4, Code.Nop_rm16, Register.CX)]
		void Test32_Grp_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F1F 00", 4, Code.Nop_rm16)]
		void Test64_Grp_Ew_1(string hexBytes, int byteLength, Code code) {
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
		[InlineData("66 0F1F C1", 4, Code.Nop_rm16, Register.CX)]
		[InlineData("66 41 0F1F C1", 5, Code.Nop_rm16, Register.R9W)]
		void Test64_Grp_Ew_2(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("66 0F1F 00", 4, Code.Nop_rm32)]
		void Test16_Grp_Ed_1(string hexBytes, int byteLength, Code code) {
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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		}

		[Theory]
		[InlineData("0F1F C1", 3, Code.Nop_rm32, Register.ECX)]
		void Test32_Grp_Ed_2(string hexBytes, int byteLength, Code code, Register reg) {
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
		[InlineData("0F1F 00", 3, Code.Nop_rm32)]
		void Test64_Grp_Ed_1(string hexBytes, int byteLength, Code code) {
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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);
		}

		[Theory]
		[MemberData(nameof(Test16_Grp_Mem_1_Data))]
		void Test16_Grp_Mem_1(string hexBytes, int byteLength, Code code, MemorySize memSize, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
		public static IEnumerable<object[]> Test16_Grp_Mem_1_Data {
			get {
				yield return new object[] { "0F1C 00", 3, Code.Cldemote_m8, MemorySize.UInt8, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Grp_Mem_1_Data))]
		void Test32_Grp_Mem_1(string hexBytes, int byteLength, Code code, MemorySize memSize, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
		public static IEnumerable<object[]> Test32_Grp_Mem_1_Data {
			get {
				yield return new object[] { "0F1C 00", 3, Code.Cldemote_m8, MemorySize.UInt8, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Grp_Mem_1_Data))]
		void Test64_Grp_Mem_1(string hexBytes, int byteLength, Code code, MemorySize memSize, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
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
		public static IEnumerable<object[]> Test64_Grp_Mem_1_Data {
			get {
				yield return new object[] { "0F1C 00", 3, Code.Cldemote_m8, MemorySize.UInt8, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Grp_Reg_1_Data))]
		void Test16_Grp_Reg_1(string hexBytes, int byteLength, Code code, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder16(hexBytes, options);
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
		public static IEnumerable<object[]> Test16_Grp_Reg_1_Data {
			get {
				yield return new object[] { "F3 0F1E C9", 4, Code.Rdsspd_r32, Register.ECX, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Grp_Reg_1_Data))]
		void Test32_Grp_Reg_1(string hexBytes, int byteLength, Code code, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder32(hexBytes, options);
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
		public static IEnumerable<object[]> Test32_Grp_Reg_1_Data {
			get {
				yield return new object[] { "F3 0F1E C9", 4, Code.Rdsspd_r32, Register.ECX, DecoderOptions.None };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Grp_Reg_1_Data))]
		void Test64_Grp_Reg_1(string hexBytes, int byteLength, Code code, Register reg, DecoderOptions options) {
			var decoder = CreateDecoder64(hexBytes, options);
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
		public static IEnumerable<object[]> Test64_Grp_Reg_1_Data {
			get {
				yield return new object[] { "F3 0F1E C9", 4, Code.Rdsspd_r32, Register.ECX, DecoderOptions.None };
				yield return new object[] { "F3 41 0F1E C9", 5, Code.Rdsspd_r32, Register.R9D, DecoderOptions.None };

				yield return new object[] { "F3 48 0F1E C9", 5, Code.Rdsspq_r64, Register.RCX, DecoderOptions.None };
				yield return new object[] { "F3 49 0F1E C9", 5, Code.Rdsspq_r64, Register.R9, DecoderOptions.None };
			}
		}
	}
}
