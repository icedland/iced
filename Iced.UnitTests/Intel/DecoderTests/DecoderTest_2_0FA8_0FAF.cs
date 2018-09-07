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
	public sealed class DecoderTest_2_0FA8_0FAF : DecoderTest {
		[Fact]
		void Test16_Pushw_GS_1() {
			var decoder = CreateDecoder16("0FA8");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushw_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test32_Pushw_GS_1() {
			var decoder = CreateDecoder32("66 0FA8");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushw_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test64_Pushw_GS_1() {
			var decoder = CreateDecoder64("66 0FA8");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushw_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test16_Pushd_GS_1() {
			var decoder = CreateDecoder16("66 0FA8");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushd_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test32_Pushd_GS_1() {
			var decoder = CreateDecoder32("0FA8");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushd_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test64_Pushq_GS_1() {
			var decoder = CreateDecoder64("0FA8");
			var instr = decoder.Decode();

			Assert.Equal(Code.Pushq_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test16_Popw_GS_1() {
			var decoder = CreateDecoder16("0FA9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popw_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test32_Popw_GS_1() {
			var decoder = CreateDecoder32("66 0FA9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popw_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test64_Popw_GS_1() {
			var decoder = CreateDecoder64("66 0FA9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popw_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test16_Popd_GS_1() {
			var decoder = CreateDecoder16("66 0FA9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popd_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test32_Popd_GS_1() {
			var decoder = CreateDecoder32("0FA9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popd_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Fact]
		void Test64_Popq_GS_1() {
			var decoder = CreateDecoder64("0FA9");
			var instr = decoder.Decode();

			Assert.Equal(Code.Popq_GS, instr.Code);
			Assert.Equal(1, instr.OpCount);
			Assert.Equal(2, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.GS, instr.Op0Register);
		}

		[Theory]
		[MemberData(nameof(Test16_Simple_1_Data))]
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
		public static IEnumerable<object[]> Test16_Simple_1_Data {
			get {
				yield return new object[] { "0FAA", 2, Code.Rsm };

				yield return new object[] { "0FAE E8", 3, Code.Lfence };
				yield return new object[] { "0FAE E9", 3, Code.Lfence };
				yield return new object[] { "0FAE EA", 3, Code.Lfence };
				yield return new object[] { "0FAE EB", 3, Code.Lfence };
				yield return new object[] { "0FAE EC", 3, Code.Lfence };
				yield return new object[] { "0FAE ED", 3, Code.Lfence };
				yield return new object[] { "0FAE EE", 3, Code.Lfence };
				yield return new object[] { "0FAE EF", 3, Code.Lfence };

				yield return new object[] { "0FAE F0", 3, Code.Mfence };
				yield return new object[] { "0FAE F1", 3, Code.Mfence };
				yield return new object[] { "0FAE F2", 3, Code.Mfence };
				yield return new object[] { "0FAE F3", 3, Code.Mfence };
				yield return new object[] { "0FAE F4", 3, Code.Mfence };
				yield return new object[] { "0FAE F5", 3, Code.Mfence };
				yield return new object[] { "0FAE F6", 3, Code.Mfence };
				yield return new object[] { "0FAE F7", 3, Code.Mfence };

				yield return new object[] { "0FAE F8", 3, Code.Sfence };
				yield return new object[] { "0FAE F9", 3, Code.Sfence };
				yield return new object[] { "0FAE FA", 3, Code.Sfence };
				yield return new object[] { "0FAE FB", 3, Code.Sfence };
				yield return new object[] { "0FAE FC", 3, Code.Sfence };
				yield return new object[] { "0FAE FD", 3, Code.Sfence };
				yield return new object[] { "0FAE FE", 3, Code.Sfence };
				yield return new object[] { "0FAE FF", 3, Code.Sfence };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Simple_1_Data))]
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
		public static IEnumerable<object[]> Test32_Simple_1_Data {
			get {
				yield return new object[] { "0FAA", 2, Code.Rsm };

				yield return new object[] { "0FAE E8", 3, Code.Lfence };
				yield return new object[] { "0FAE E9", 3, Code.Lfence };
				yield return new object[] { "0FAE EA", 3, Code.Lfence };
				yield return new object[] { "0FAE EB", 3, Code.Lfence };
				yield return new object[] { "0FAE EC", 3, Code.Lfence };
				yield return new object[] { "0FAE ED", 3, Code.Lfence };
				yield return new object[] { "0FAE EE", 3, Code.Lfence };
				yield return new object[] { "0FAE EF", 3, Code.Lfence };

				yield return new object[] { "0FAE F0", 3, Code.Mfence };
				yield return new object[] { "0FAE F1", 3, Code.Mfence };
				yield return new object[] { "0FAE F2", 3, Code.Mfence };
				yield return new object[] { "0FAE F3", 3, Code.Mfence };
				yield return new object[] { "0FAE F4", 3, Code.Mfence };
				yield return new object[] { "0FAE F5", 3, Code.Mfence };
				yield return new object[] { "0FAE F6", 3, Code.Mfence };
				yield return new object[] { "0FAE F7", 3, Code.Mfence };

				yield return new object[] { "0FAE F8", 3, Code.Sfence };
				yield return new object[] { "0FAE F9", 3, Code.Sfence };
				yield return new object[] { "0FAE FA", 3, Code.Sfence };
				yield return new object[] { "0FAE FB", 3, Code.Sfence };
				yield return new object[] { "0FAE FC", 3, Code.Sfence };
				yield return new object[] { "0FAE FD", 3, Code.Sfence };
				yield return new object[] { "0FAE FE", 3, Code.Sfence };
				yield return new object[] { "0FAE FF", 3, Code.Sfence };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Simple_1_Data))]
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
		public static IEnumerable<object[]> Test64_Simple_1_Data {
			get {
				yield return new object[] { "0FAA", 2, Code.Rsm };

				yield return new object[] { "0FAE E8", 3, Code.Lfence };
				yield return new object[] { "0FAE E9", 3, Code.Lfence };
				yield return new object[] { "0FAE EA", 3, Code.Lfence };
				yield return new object[] { "0FAE EB", 3, Code.Lfence };
				yield return new object[] { "0FAE EC", 3, Code.Lfence };
				yield return new object[] { "0FAE ED", 3, Code.Lfence };
				yield return new object[] { "0FAE EE", 3, Code.Lfence };
				yield return new object[] { "0FAE EF", 3, Code.Lfence };

				yield return new object[] { "0FAE F0", 3, Code.Mfence };
				yield return new object[] { "0FAE F1", 3, Code.Mfence };
				yield return new object[] { "0FAE F2", 3, Code.Mfence };
				yield return new object[] { "0FAE F3", 3, Code.Mfence };
				yield return new object[] { "0FAE F4", 3, Code.Mfence };
				yield return new object[] { "0FAE F5", 3, Code.Mfence };
				yield return new object[] { "0FAE F6", 3, Code.Mfence };
				yield return new object[] { "0FAE F7", 3, Code.Mfence };

				yield return new object[] { "0FAE F8", 3, Code.Sfence };
				yield return new object[] { "0FAE F9", 3, Code.Sfence };
				yield return new object[] { "0FAE FA", 3, Code.Sfence };
				yield return new object[] { "0FAE FB", 3, Code.Sfence };
				yield return new object[] { "0FAE FC", 3, Code.Sfence };
				yield return new object[] { "0FAE FD", 3, Code.Sfence };
				yield return new object[] { "0FAE FE", 3, Code.Sfence };
				yield return new object[] { "0FAE FF", 3, Code.Sfence };
			}
		}

		[Fact]
		void Test16_Bts_Ew_Gw_1() {
			var decoder = CreateDecoder16("0FAB CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Fact]
		void Test16_Bts_Ew_Gw_2() {
			var decoder = CreateDecoder16("0FAB 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test32_Bts_Ew_Gw_1() {
			var decoder = CreateDecoder32("66 0FAB CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);
		}

		[Fact]
		void Test32_Bts_Ew_Gw_2() {
			var decoder = CreateDecoder32("66 0FAB 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Theory]
		[InlineData("66 0FAB CE", 4, Register.SI, Register.CX)]
		[InlineData("66 44 0FAB C5", 5, Register.BP, Register.R8W)]
		[InlineData("66 41 0FAB D6", 5, Register.R14W, Register.DX)]
		[InlineData("66 45 0FAB D0", 5, Register.R8W, Register.R10W)]
		[InlineData("66 41 0FAB D9", 5, Register.R9W, Register.BX)]
		[InlineData("66 44 0FAB EC", 5, Register.SP, Register.R13W)]
		void Test64_Bts_Ew_Gw_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ew_Gw, instr.Code);
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
		void Test64_Bts_Ew_Gw_2() {
			var decoder = CreateDecoder64("66 0FAB 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ew_Gw, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);
		}

		[Fact]
		void Test16_Bts_Ed_Gd_1() {
			var decoder = CreateDecoder16("66 0FAB CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test16_Bts_Ed_Gd_2() {
			var decoder = CreateDecoder16("66 0FAB 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Fact]
		void Test32_Bts_Ed_Gd_1() {
			var decoder = CreateDecoder32("0FAB CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);
		}

		[Fact]
		void Test32_Bts_Ed_Gd_2() {
			var decoder = CreateDecoder32("0FAB 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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
		[InlineData("0FAB CE", 3, Register.ESI, Register.ECX)]
		[InlineData("44 0FAB C5", 4, Register.EBP, Register.R8D)]
		[InlineData("41 0FAB D6", 4, Register.R14D, Register.EDX)]
		[InlineData("45 0FAB D0", 4, Register.R8D, Register.R10D)]
		[InlineData("41 0FAB D9", 4, Register.R9D, Register.EBX)]
		[InlineData("44 0FAB EC", 4, Register.ESP, Register.R13D)]
		void Test64_Bts_Ed_Gd_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ed_Gd, instr.Code);
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
		void Test64_Bts_Ed_Gd_2() {
			var decoder = CreateDecoder64("0FAB 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Ed_Gd, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);
		}

		[Theory]
		[InlineData("48 0FAB CE", 4, Register.RSI, Register.RCX)]
		[InlineData("4C 0FAB C5", 4, Register.RBP, Register.R8)]
		[InlineData("49 0FAB D6", 4, Register.R14, Register.RDX)]
		[InlineData("4D 0FAB D0", 4, Register.R8, Register.R10)]
		[InlineData("49 0FAB D9", 4, Register.R9, Register.RBX)]
		[InlineData("4C 0FAB EC", 4, Register.RSP, Register.R13)]
		void Test64_Bts_Eq_Gq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Eq_Gq, instr.Code);
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
		void Test64_Bts_Eq_Gq_2() {
			var decoder = CreateDecoder64("48 0FAB 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Bts_Eq_Gq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RBX, instr.Op1Register);
		}

		[Fact]
		void Test16_Shrd_Ew_Gw_Ib_1() {
			var decoder = CreateDecoder16("0FAC CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shrd_Ew_Gw_Ib_2() {
			var decoder = CreateDecoder16("0FAC 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shrd_Ew_Gw_Ib_1() {
			var decoder = CreateDecoder32("66 0FAC CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shrd_Ew_Gw_Ib_2() {
			var decoder = CreateDecoder32("66 0FAC 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Theory]
		[InlineData("66 0FAC CE 5A", 5, Register.SI, Register.CX, 0x5A)]
		[InlineData("66 44 0FAC C5 5A", 6, Register.BP, Register.R8W, 0x5A)]
		[InlineData("66 41 0FAC D6 5A", 6, Register.R14W, Register.DX, 0x5A)]
		[InlineData("66 45 0FAC D0 5A", 6, Register.R8W, Register.R10W, 0x5A)]
		[InlineData("66 41 0FAC D9 5A", 6, Register.R9W, Register.BX, 0x5A)]
		[InlineData("66 44 0FAC EC 5A", 6, Register.SP, Register.R13W, 0x5A)]
		void Test64_Shrd_Ew_Gw_Ib_1(string hexBytes, int byteLength, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_Ib, instr.Code);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Fact]
		void Test64_Shrd_Ew_Gw_Ib_2() {
			var decoder = CreateDecoder64("66 0FAC 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shrd_Ed_Gd_Ib_1() {
			var decoder = CreateDecoder16("66 0FAC CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shrd_Ed_Gd_Ib_2() {
			var decoder = CreateDecoder16("66 0FAC 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shrd_Ed_Gd_Ib_1() {
			var decoder = CreateDecoder32("0FAC CE 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test32_Shrd_Ed_Gd_Ib_2() {
			var decoder = CreateDecoder32("0FAC 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Theory]
		[InlineData("0FAC CE 5A", 4, Register.ESI, Register.ECX, 0x5A)]
		[InlineData("44 0FAC C5 5A", 5, Register.EBP, Register.R8D, 0x5A)]
		[InlineData("41 0FAC D6 5A", 5, Register.R14D, Register.EDX, 0x5A)]
		[InlineData("45 0FAC D0 5A", 5, Register.R8D, Register.R10D, 0x5A)]
		[InlineData("41 0FAC D9 5A", 5, Register.R9D, Register.EBX, 0x5A)]
		[InlineData("44 0FAC EC 5A", 5, Register.ESP, Register.R13D, 0x5A)]
		void Test64_Shrd_Ed_Gd_Ib_1(string hexBytes, int byteLength, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_Ib, instr.Code);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Fact]
		void Test64_Shrd_Ed_Gd_Ib_2() {
			var decoder = CreateDecoder64("0FAC 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Theory]
		[InlineData("48 0FAC CE 5A", 5, Register.RSI, Register.RCX, 0x5A)]
		[InlineData("4C 0FAC C5 5A", 5, Register.RBP, Register.R8, 0x5A)]
		[InlineData("49 0FAC D6 5A", 5, Register.R14, Register.RDX, 0x5A)]
		[InlineData("4D 0FAC D0 5A", 5, Register.R8, Register.R10, 0x5A)]
		[InlineData("49 0FAC D9 5A", 5, Register.R9, Register.RBX, 0x5A)]
		[InlineData("4C 0FAC EC 5A", 5, Register.RSP, Register.R13, 0x5A)]
		void Test64_Shrd_Eq_Gq_Ib_1(string hexBytes, int byteLength, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Eq_Gq_Ib, instr.Code);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}

		[Fact]
		void Test64_Shrd_Eq_Gq_Ib_2() {
			var decoder = CreateDecoder64("48 0FAC 18 5A");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Eq_Gq_Ib, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(5, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RBX, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(0x5A, instr.Immediate8);
		}

		[Fact]
		void Test16_Shrd_Ew_Gw_CL_1() {
			var decoder = CreateDecoder16("0FAD CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test16_Shrd_Ew_Gw_CL_2() {
			var decoder = CreateDecoder16("0FAD 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shrd_Ew_Gw_CL_1() {
			var decoder = CreateDecoder32("66 0FAD CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.SI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.CX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shrd_Ew_Gw_CL_2() {
			var decoder = CreateDecoder32("66 0FAD 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[InlineData("66 0FAD CE", 4, Register.SI, Register.CX)]
		[InlineData("66 44 0FAD C5", 5, Register.BP, Register.R8W)]
		[InlineData("66 41 0FAD D6", 5, Register.R14W, Register.DX)]
		[InlineData("66 45 0FAD D0", 5, Register.R8W, Register.R10W)]
		[InlineData("66 41 0FAD D9", 5, Register.R9W, Register.BX)]
		[InlineData("66 44 0FAD EC", 5, Register.SP, Register.R13W)]
		void Test64_Shrd_Ew_Gw_CL_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_CL, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test64_Shrd_Ew_Gw_CL_2() {
			var decoder = CreateDecoder64("66 0FAD 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ew_Gw_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.BX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test16_Shrd_Ed_Gd_CL_1() {
			var decoder = CreateDecoder16("66 0FAD CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test16_Shrd_Ed_Gd_CL_2() {
			var decoder = CreateDecoder16("66 0FAD 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shrd_Ed_Gd_CL_1() {
			var decoder = CreateDecoder32("0FAD CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ESI, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ECX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test32_Shrd_Ed_Gd_CL_2() {
			var decoder = CreateDecoder32("0FAD 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[InlineData("0FAD CE", 3, Register.ESI, Register.ECX)]
		[InlineData("44 0FAD C5", 4, Register.EBP, Register.R8D)]
		[InlineData("41 0FAD D6", 4, Register.R14D, Register.EDX)]
		[InlineData("45 0FAD D0", 4, Register.R8D, Register.R10D)]
		[InlineData("41 0FAD D9", 4, Register.R9D, Register.EBX)]
		[InlineData("44 0FAD EC", 4, Register.ESP, Register.R13D)]
		void Test64_Shrd_Ed_Gd_CL_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_CL, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test64_Shrd_Ed_Gd_CL_2() {
			var decoder = CreateDecoder64("0FAD 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Ed_Gd_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.EBX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[InlineData("48 0FAD CE", 4, Register.RSI, Register.RCX)]
		[InlineData("4C 0FAD C5", 4, Register.RBP, Register.R8)]
		[InlineData("49 0FAD D6", 4, Register.R14, Register.RDX)]
		[InlineData("4D 0FAD D0", 4, Register.R8, Register.R10)]
		[InlineData("49 0FAD D9", 4, Register.R9, Register.RBX)]
		[InlineData("4C 0FAD EC", 4, Register.RSP, Register.R13)]
		void Test64_Shrd_Eq_Gq_CL_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Eq_Gq_CL, instr.Code);
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Fact]
		void Test64_Shrd_Eq_Gq_CL_2() {
			var decoder = CreateDecoder64("48 0FAD 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Shrd_Eq_Gq_CL, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
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

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.RBX, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(Register.CL, instr.Op2Register);
		}

		[Theory]
		[MemberData(nameof(Test16_Grp_RM_1_Data))]
		void Test16_Grp_RM_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Grp_RM_1_Data {
			get {
				yield return new object[] { "0FAE 00", 3, Code.Fxsave_M, MemorySize.Fxsave_512Byte };

				yield return new object[] { "0FAE 08", 3, Code.Fxrstor_M, MemorySize.Fxsave_512Byte };

				yield return new object[] { "0FAE 10", 3, Code.Ldmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "0FAE 18", 3, Code.Stmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "0FAE 20", 3, Code.Xsave_M, MemorySize.Xsave };

				yield return new object[] { "F3 0FAE 20", 4, Code.Ptwrite_Ed, MemorySize.UInt32 };

				yield return new object[] { "0FAE 28", 3, Code.Xrstor_M, MemorySize.Xsave };

				yield return new object[] { "0FAE 30", 3, Code.Xsaveopt_M, MemorySize.Xsave };

				yield return new object[] { "66 0FAE 30", 4, Code.Clwb_Mb, MemorySize.UInt8 };

				yield return new object[] { "0FAE 38", 3, Code.Clflush_Mb, MemorySize.UInt8 };

				yield return new object[] { "66 0FAE 38", 4, Code.Clflushopt_Mb, MemorySize.UInt8 };

				yield return new object[] { "C5F8 AE 10", 4, Code.VEX_Vldmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "C4E1F8 AE 10", 5, Code.VEX_Vldmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "C5F8 AE 18", 4, Code.VEX_Vstmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "C4E1F8 AE 18", 5, Code.VEX_Vstmxcsr_Md, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Grp_RM_2_Data))]
		void Test16_Grp_RM_2(string hexBytes, int byteLength, Code code, Register reg1) {
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
			Assert.Equal(reg1, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test16_Grp_RM_2_Data {
			get {
				yield return new object[] { "F3 0FAE E5", 4, Code.Ptwrite_Ed, Register.EBP };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Grp_RM_1_Data))]
		void Test32_Grp_RM_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Grp_RM_1_Data {
			get {
				yield return new object[] { "0FAE 00", 3, Code.Fxsave_M, MemorySize.Fxsave_512Byte };

				yield return new object[] { "0FAE 08", 3, Code.Fxrstor_M, MemorySize.Fxsave_512Byte };

				yield return new object[] { "0FAE 10", 3, Code.Ldmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "0FAE 18", 3, Code.Stmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "0FAE 20", 3, Code.Xsave_M, MemorySize.Xsave };

				yield return new object[] { "F3 0FAE 20", 4, Code.Ptwrite_Ed, MemorySize.UInt32 };

				yield return new object[] { "0FAE 28", 3, Code.Xrstor_M, MemorySize.Xsave };

				yield return new object[] { "0FAE 30", 3, Code.Xsaveopt_M, MemorySize.Xsave };

				yield return new object[] { "66 0FAE 30", 4, Code.Clwb_Mb, MemorySize.UInt8 };

				yield return new object[] { "0FAE 38", 3, Code.Clflush_Mb, MemorySize.UInt8 };

				yield return new object[] { "66 0FAE 38", 4, Code.Clflushopt_Mb, MemorySize.UInt8 };

				yield return new object[] { "C5F8 AE 10", 4, Code.VEX_Vldmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "C4E1F8 AE 10", 5, Code.VEX_Vldmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "C5F8 AE 18", 4, Code.VEX_Vstmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "C4E1F8 AE 18", 5, Code.VEX_Vstmxcsr_Md, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Grp_RM_2_Data))]
		void Test32_Grp_RM_2(string hexBytes, int byteLength, Code code, Register reg1) {
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
			Assert.Equal(reg1, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test32_Grp_RM_2_Data {
			get {
				yield return new object[] { "F3 0FAE E5", 4, Code.Ptwrite_Ed, Register.EBP };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Grp_RM_1_Data))]
		void Test64_Grp_RM_1(string hexBytes, int byteLength, Code code, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Grp_RM_1_Data {
			get {
				yield return new object[] { "0FAE 00", 3, Code.Fxsave_M, MemorySize.Fxsave_512Byte };
				yield return new object[] { "48 0FAE 00", 4, Code.Fxsave64_M, MemorySize.Fxsave64_512Byte };

				yield return new object[] { "0FAE 08", 3, Code.Fxrstor_M, MemorySize.Fxsave_512Byte };
				yield return new object[] { "48 0FAE 08", 4, Code.Fxrstor64_M, MemorySize.Fxsave64_512Byte };

				yield return new object[] { "0FAE 10", 3, Code.Ldmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "48 0FAE 10", 4, Code.Ldmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "0FAE 18", 3, Code.Stmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "48 0FAE 18", 4, Code.Stmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "0FAE 20", 3, Code.Xsave_M, MemorySize.Xsave };
				yield return new object[] { "48 0FAE 20", 4, Code.Xsave64_M, MemorySize.Xsave64 };

				yield return new object[] { "F3 0FAE 20", 4, Code.Ptwrite_Ed, MemorySize.UInt32 };
				yield return new object[] { "F3 48 0FAE 20", 5, Code.Ptwrite_Eq, MemorySize.UInt64 };

				yield return new object[] { "0FAE 28", 3, Code.Xrstor_M, MemorySize.Xsave };
				yield return new object[] { "48 0FAE 28", 4, Code.Xrstor64_M, MemorySize.Xsave64 };

				yield return new object[] { "0FAE 30", 3, Code.Xsaveopt_M, MemorySize.Xsave };
				yield return new object[] { "48 0FAE 30", 4, Code.Xsaveopt64_M, MemorySize.Xsave64 };

				yield return new object[] { "66 0FAE 30", 4, Code.Clwb_Mb, MemorySize.UInt8 };

				yield return new object[] { "0FAE 38", 3, Code.Clflush_Mb, MemorySize.UInt8 };

				yield return new object[] { "66 0FAE 38", 4, Code.Clflushopt_Mb, MemorySize.UInt8 };

				yield return new object[] { "C5F8 AE 10", 4, Code.VEX_Vldmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "C4E1F8 AE 10", 5, Code.VEX_Vldmxcsr_Md, MemorySize.UInt32 };

				yield return new object[] { "C5F8 AE 18", 4, Code.VEX_Vstmxcsr_Md, MemorySize.UInt32 };
				yield return new object[] { "C4E1F8 AE 18", 5, Code.VEX_Vstmxcsr_Md, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Grp_RM_2_Data))]
		void Test64_Grp_RM_2(string hexBytes, int byteLength, Code code, Register reg1) {
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
			Assert.Equal(reg1, instr.Op0Register);
		}
		public static IEnumerable<object[]> Test64_Grp_RM_2_Data {
			get {
				yield return new object[] { "F3 0FAE C5", 4, Code.Rdfsbase_Rd, Register.EBP };
				yield return new object[] { "F3 41 0FAE C5", 5, Code.Rdfsbase_Rd, Register.R13D };

				yield return new object[] { "F3 48 0FAE C5", 5, Code.Rdfsbase_Rq, Register.RBP };
				yield return new object[] { "F3 49 0FAE C5", 5, Code.Rdfsbase_Rq, Register.R13 };

				yield return new object[] { "F3 0FAE CD", 4, Code.Rdgsbase_Rd, Register.EBP };
				yield return new object[] { "F3 41 0FAE CD", 5, Code.Rdgsbase_Rd, Register.R13D };

				yield return new object[] { "F3 48 0FAE CD", 5, Code.Rdgsbase_Rq, Register.RBP };
				yield return new object[] { "F3 49 0FAE CD", 5, Code.Rdgsbase_Rq, Register.R13 };

				yield return new object[] { "F3 0FAE D5", 4, Code.Wrfsbase_Rd, Register.EBP };
				yield return new object[] { "F3 41 0FAE D5", 5, Code.Wrfsbase_Rd, Register.R13D };

				yield return new object[] { "F3 48 0FAE D5", 5, Code.Wrfsbase_Rq, Register.RBP };
				yield return new object[] { "F3 49 0FAE D5", 5, Code.Wrfsbase_Rq, Register.R13 };

				yield return new object[] { "F3 0FAE DD", 4, Code.Wrgsbase_Rd, Register.EBP };
				yield return new object[] { "F3 41 0FAE DD", 5, Code.Wrgsbase_Rd, Register.R13D };

				yield return new object[] { "F3 48 0FAE DD", 5, Code.Wrgsbase_Rq, Register.RBP };
				yield return new object[] { "F3 49 0FAE DD", 5, Code.Wrgsbase_Rq, Register.R13 };

				yield return new object[] { "F3 0FAE E5", 4, Code.Ptwrite_Ed, Register.EBP };
				yield return new object[] { "F3 41 0FAE E5", 5, Code.Ptwrite_Ed, Register.R13D };

				yield return new object[] { "F3 48 0FAE E5", 5, Code.Ptwrite_Eq, Register.RBP };
				yield return new object[] { "F3 49 0FAE E5", 5, Code.Ptwrite_Eq, Register.R13 };
			}
		}

		[Fact]
		void Test16_Imul_Gw_Ew_1() {
			var decoder = CreateDecoder16("0FAF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew, instr.Code);
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
		void Test16_Imul_Gw_Ew_2() {
			var decoder = CreateDecoder16("0FAF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew, instr.Code);
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
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Imul_Gw_Ew_1() {
			var decoder = CreateDecoder32("66 0FAF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew, instr.Code);
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
		void Test32_Imul_Gw_Ew_2() {
			var decoder = CreateDecoder32("66 0FAF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew, instr.Code);
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
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0FAF CE", 4, Register.CX, Register.SI)]
		[InlineData("66 44 0FAF C5", 5, Register.R8W, Register.BP)]
		[InlineData("66 41 0FAF D6", 5, Register.DX, Register.R14W)]
		[InlineData("66 45 0FAF D0", 5, Register.R10W, Register.R8W)]
		[InlineData("66 41 0FAF D9", 5, Register.BX, Register.R9W)]
		[InlineData("66 44 0FAF EC", 5, Register.R13W, Register.SP)]
		void Test64_Imul_Gw_Ew_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew, instr.Code);
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
		void Test64_Imul_Gw_Ew_2() {
			var decoder = CreateDecoder64("66 0FAF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gw_Ew, instr.Code);
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
			Assert.Equal(MemorySize.Int16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Imul_Gd_Ed_1() {
			var decoder = CreateDecoder16("66 0FAF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed, instr.Code);
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
		void Test16_Imul_Gd_Ed_2() {
			var decoder = CreateDecoder16("66 0FAF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed, instr.Code);
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
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Imul_Gd_Ed_1() {
			var decoder = CreateDecoder32("0FAF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed, instr.Code);
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
		void Test32_Imul_Gd_Ed_2() {
			var decoder = CreateDecoder32("0FAF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed, instr.Code);
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
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0FAF CE", 3, Register.ECX, Register.ESI)]
		[InlineData("44 0FAF C5", 4, Register.R8D, Register.EBP)]
		[InlineData("41 0FAF D6", 4, Register.EDX, Register.R14D)]
		[InlineData("45 0FAF D0", 4, Register.R10D, Register.R8D)]
		[InlineData("41 0FAF D9", 4, Register.EBX, Register.R9D)]
		[InlineData("44 0FAF EC", 4, Register.R13D, Register.ESP)]
		void Test64_Imul_Gd_Ed_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed, instr.Code);
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
		void Test64_Imul_Gd_Ed_2() {
			var decoder = CreateDecoder64("0FAF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gd_Ed, instr.Code);
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
			Assert.Equal(MemorySize.Int32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 0FAF CE", 4, Register.RCX, Register.RSI)]
		[InlineData("4C 0FAF C5", 4, Register.R8, Register.RBP)]
		[InlineData("49 0FAF D6", 4, Register.RDX, Register.R14)]
		[InlineData("4D 0FAF D0", 4, Register.R10, Register.R8)]
		[InlineData("49 0FAF D9", 4, Register.RBX, Register.R9)]
		[InlineData("4C 0FAF EC", 4, Register.R13, Register.RSP)]
		void Test64_Imul_Gq_Eq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gq_Eq, instr.Code);
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
		void Test64_Imul_Gq_Eq_2() {
			var decoder = CreateDecoder64("48 0FAF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Imul_Gq_Eq, instr.Code);
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
			Assert.Equal(MemorySize.Int64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
	}
}
