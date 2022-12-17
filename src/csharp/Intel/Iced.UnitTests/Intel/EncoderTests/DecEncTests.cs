// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if ENCODER && OPCODE_INFO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.EncoderTests {
	public sealed class DecEncTests {
		[Fact]
		void Verify_invalid_and_valid_lock_prefix() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				bool hasLock;
				bool canUseLock;

				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
					decoder.Decode(out var instruction);
					Assert.Equal(info.Code, instruction.Code);
					hasLock = instruction.HasLockPrefix;
					var opCode = info.Code.ToOpCode();
					canUseLock = opCode.CanUseLockPrefix && HasModRMMemoryOperand(instruction);
					if (opCode.AmdLockRegBit)
						continue;
				}

				if (canUseLock) {
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(AddLock(info.HexBytes, hasLock)), info.Options);
					decoder.Decode(out var instruction);
					Assert.Equal(info.Code, instruction.Code);
					Assert.True(instruction.HasLockPrefix);
				}
				else {
					Debug.Assert(!hasLock);
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(AddLock(info.HexBytes, hasLock)), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(Code.INVALID, instruction.Code);
						Assert.NotEqual(DecoderError.None, decoder.LastError);
						Assert.False(instruction.HasLockPrefix);
					}
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(AddLock(info.HexBytes, hasLock)), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(instruction.HasLockPrefix);
					}
				}
			}

			static string AddLock(string hexBytes, bool hasLock) => hasLock ? hexBytes : "F0" + hexBytes;

			static bool HasModRMMemoryOperand(in Instruction instruction) {
				int opCount = instruction.OpCount;
				for (int i = 0; i < opCount; i++) {
					if (instruction.GetOpKind(i) == OpKind.Memory)
						return true;
				}
				return false;
			}
		}

		[Fact]
		void Verify_invalid_REX_mandatory_prefixes_VEX_EVEX_XOP_MVEX() {
			var prefixes1632 = new string[] { "66", "F3", "F2" };
			var prefixes64   = new string[] { "66", "F3", "F2",
											  "40", "41", "42", "43", "44", "45", "46", "47",
											  "48", "49", "4A", "4B", "4C", "4D", "4E", "4F" };
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				switch (info.Code.ToOpCode().Encoding) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					continue;

				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
				case EncodingKind.MVEX:
					break;

				default:
					throw new InvalidOperationException();
				}

				string[] prefixes;
				switch (info.Bitness) {
				case 16:
				case 32:
					prefixes = prefixes1632;
					break;
				case 64:
					prefixes = prefixes64;
					break;
				default:
					throw new InvalidOperationException();
				}
				foreach (var prefix in prefixes) {
					Instruction origInstr;
					{
						var bytes = HexUtils.ToByteArray(info.HexBytes);
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
						// Mandatory prefix must be right before the opcode. If it has a seg override, there's also
						// a test without a seg override so just skip this.
						if (origInstr.SegmentPrefix != Register.None)
							continue;
						int memRegSize = GetMemoryRegisterSize(origInstr);
						// 67h prefix
						if (memRegSize != 0 && memRegSize != info.Bitness)
							continue;
						int nonPrefixIndex = SkipPrefixes(bytes, info.Bitness, out _);
						bool has67 = false;
						for (int i = 0; i < nonPrefixIndex; i++) {
							if (bytes[i] == 0x67) {
								has67 = true;
								break;
							}
						}
						if (has67)
							continue;
					}
					var hexBytes = prefix + info.HexBytes;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);

						instruction.Length--;
						instruction.NextIP--;
						if (prefix == "F3") {
							Assert.True(instruction.HasRepPrefix);
							Assert.True(instruction.HasRepePrefix);
							instruction.HasRepPrefix = false;
						}
						else if (prefix == "F2") {
							Assert.True(instruction.HasRepnePrefix);
							instruction.HasRepnePrefix = false;
						}
						if (instruction.Op1Kind == OpKind.NearBranch64)
							instruction.NearBranch64--;
						Assert.True(Instruction.EqualsAllBits(instruction, origInstr));
					}
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(Code.INVALID, instruction.Code);
						Assert.NotEqual(DecoderError.None, decoder.LastError);
					}
				}
			}
		}

		static int GetMemoryRegisterSize(in Instruction instruction) {
			int opCount = instruction.OpCount;
			for (int i = 0; i < opCount; i++) {
				switch (instruction.GetOpKind(i)) {
				case OpKind.Register:
				case OpKind.NearBranch16:
				case OpKind.NearBranch32:
				case OpKind.NearBranch64:
				case OpKind.FarBranch16:
				case OpKind.FarBranch32:
				case OpKind.Immediate8:
				case OpKind.Immediate8_2nd:
				case OpKind.Immediate16:
				case OpKind.Immediate32:
				case OpKind.Immediate64:
				case OpKind.Immediate8to16:
				case OpKind.Immediate8to32:
				case OpKind.Immediate8to64:
				case OpKind.Immediate32to64:
					break;
				case OpKind.MemorySegSI:
				case OpKind.MemorySegDI:
				case OpKind.MemoryESDI:
					return 16;
				case OpKind.MemorySegESI:
				case OpKind.MemorySegEDI:
				case OpKind.MemoryESEDI:
					return 32;
				case OpKind.MemorySegRSI:
				case OpKind.MemorySegRDI:
				case OpKind.MemoryESRDI:
					return 64;
				case OpKind.Memory:
					var reg = instruction.MemoryBase;
					if (reg == Register.None)
						reg = instruction.MemoryIndex;
					if (reg != Register.None)
						return GetSize(reg) * 8;
					if (instruction.MemoryDisplSize == 4)
						return 32;
					if (instruction.MemoryDisplSize == 8)
						return 64;
					break;
				default:
					throw new InvalidOperationException();
				}
			}
			return 0;
		}

		static int GetSize(Register reg) {
#if INSTR_INFO
			return reg.GetSize();
#else
			if (Register.AX <= reg && reg <= Register.R15W)
				return 2;
			if (Register.EAX <= reg && reg <= Register.R15D || reg == Register.EIP)
				return 4;
			if (Register.RAX <= reg && reg <= Register.R15 || reg == Register.RIP)
				return 8;
			throw new InvalidOperationException();
#endif
		}

		static int GetNumber(Register reg) {
#if INSTR_INFO
			return reg.GetNumber();
#else
			if (Register.AL <= reg && reg <= Register.R15L)
				return reg - Register.AL;
			if (Register.AX <= reg && reg <= Register.R15W)
				return reg - Register.AX;
			if (Register.EAX <= reg && reg <= Register.R15D)
				return reg - Register.EAX;
			if (Register.RAX <= reg && reg <= Register.R15)
				return reg - Register.RAX;
			if (Register.XMM0 <= reg && reg <= Register.XMM31)
				return reg - Register.XMM0;
			if (Register.YMM0 <= reg && reg <= Register.YMM31)
				return reg - Register.YMM0;
			if (Register.ZMM0 <= reg && reg <= Register.ZMM31)
				return reg - Register.ZMM0;
			if (Register.K0 <= reg && reg <= Register.K7)
				return reg - Register.K0;
			if (Register.BND0 <= reg && reg <= Register.BND3)
				return reg - Register.BND0;
			if (Register.CR0 <= reg && reg <= Register.CR15)
				return reg - Register.CR0;
			if (Register.DR0 <= reg && reg <= Register.DR15)
				return reg - Register.DR0;
			if (Register.MM0 <= reg && reg <= Register.MM7)
				return reg - Register.MM0;
			if (Register.ST0 <= reg && reg <= Register.ST7)
				return reg - Register.ST0;
			if (Register.TR0 <= reg && reg <= Register.TR7)
				return reg - Register.TR0;
			throw new InvalidOperationException();
#endif
		}

		static int SkipPrefixes(byte[] bytes, int bitness, out uint rex) {
			rex = 0;
			for (int i = 0; i < bytes.Length; i++) {
				byte b = bytes[i];
				switch (b) {
				case 0x26:
				case 0x2E:
				case 0x36:
				case 0x3E:
				case 0x64:
				case 0x65:
				case 0x66:
				case 0x67:
				case 0xF0:
				case 0xF2:
				case 0xF3:
					rex = 0;
					break;
				default:
					if (bitness == 64 && (b & 0xF0) == 0x40) {
						rex = b;
					}
					else
						return i;
					break;
				}
			}
			throw new InvalidOperationException();
		}

		[Fact]
		void Test_EVEX_reserved_bits() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if (info.Code.ToOpCode().Encoding != EncodingKind.EVEX)
					continue;
				var bytes = HexUtils.ToByteArray(info.HexBytes);
				int evexIndex = GetEvexIndex(bytes);
				for (int i = 1; i <= 1; i++) {
					bytes[evexIndex + 1] = (byte)((bytes[evexIndex + 1] & ~8) | (i << 3));
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(Code.INVALID, instruction.Code);
						Assert.NotEqual(DecoderError.None, decoder.LastError);
					}
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options ^ DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instruction);
						Assert.Equal(Code.INVALID, instruction.Code);
						Assert.NotEqual(DecoderError.None, decoder.LastError);
					}
				}
			}
		}

		static int GetEvexIndex(byte[] bytes) {
			for (int i = 0; ; i++) {
				if (i >= bytes.Length)
					throw new InvalidOperationException();
				if (bytes[i] == 0x62)
					return i;
			}
		}

		static int GetVexXopIndex(byte[] bytes) {
			for (int i = 0; ; i++) {
				if (i >= bytes.Length)
					throw new InvalidOperationException();
				var b = bytes[i];
				if (b == 0xC4 || b == 0xC5 || b == 0x8F)
					return i;
			}
		}

		[Fact]
		void Test_WIG_instructions_ignore_W() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				var encoding = opCode.Encoding;
				bool isWIG = opCode.IsWIG || (opCode.IsWIG32 && info.Bitness != 64);
				if (encoding == EncodingKind.EVEX || encoding == EncodingKind.MVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);

					if (isWIG) {
						Instruction instruction1, instruction2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction1);
							Assert.Equal(info.Code, instruction1.Code);
						}
						{
							bytes[evexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction2);
							Assert.Equal(info.Code, instruction2.Code);
						}
						Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
					}
					else {
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
						}
						{
							bytes[evexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.NotEqual(info.Code, instruction.Code);
						}
					}
				}
				else if (encoding == EncodingKind.VEX || encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					if (bytes[vexIndex] == 0xC5)
						continue;

					if (isWIG) {
						Instruction instruction1, instruction2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction1);
							Assert.Equal(info.Code, instruction1.Code);
						}
						{
							bytes[vexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction2);
							Assert.Equal(info.Code, instruction2.Code);
						}
						Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
					}
					else {
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
						}
						{
							bytes[vexIndex + 2] ^= 0x80;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.NotEqual(info.Code, instruction.Code);
						}
					}
				}
				else if (encoding == EncodingKind.Legacy || encoding == EncodingKind.D3NOW)
					continue;
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Test_LIG_instructions_ignore_L() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				var encoding = opCode.Encoding;
				if (encoding == EncodingKind.EVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);

					bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
					bool EVEX_b = (bytes[evexIndex + 3] & 0x10) != 0;
					if (opCode.CanUseRoundingControl && isRegOnly && EVEX_b)
						continue;
					bool isSae = opCode.CanSuppressAllExceptions && isRegOnly && EVEX_b;

					if (opCode.IsLIG) {
						Instruction instruction1, instruction2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction1);
							Assert.Equal(info.Code, instruction1.Code);
						}
						var origByte = bytes[evexIndex + 3];
						for (int i = 1; i <= 3; i++) {
							bytes[evexIndex + 3] = (byte)(origByte ^ (i << 5));
							var ll = (bytes[evexIndex + 3] >> 5) & 3;
							bool invalid = (info.Options & DecoderOptions.NoInvalidCheck) == 0 &&
								ll == 3 && (bytes[evexIndex + 5] < 0xC0 || (bytes[evexIndex + 3] & 0x10) == 0);
							if (invalid) {
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out instruction2);
								Assert.Equal(Code.INVALID, instruction2.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);

								decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out instruction2);
								Assert.Equal(info.Code, instruction2.Code);
								Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
							}
							else {
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out instruction2);
								Assert.Equal(info.Code, instruction2.Code);
								Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
							}
						}
					}
					else {
						Instruction instruction1;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction1);
							Assert.Equal(info.Code, instruction1.Code);
						}
						var origByte = bytes[evexIndex + 3];
						for (int i = 1; i <= 3; i++) {
							bytes[evexIndex + 3] = (byte)(origByte ^ (i << 5));
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction2);
							if (isSae) {
								Assert.Equal(info.Code, instruction2.Code);
								Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
							}
							else
								Assert.NotEqual(info.Code, instruction2.Code);
						}
					}
				}
				else if (encoding == EncodingKind.VEX || encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					int lIndex = bytes[vexIndex] == 0xC5 ? vexIndex + 1 : vexIndex + 2;

					if (opCode.IsLIG) {
						Instruction instruction1, instruction2;
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction1);
							Assert.Equal(info.Code, instruction1.Code);
						}
						{
							bytes[lIndex] ^= 4;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out instruction2);
							Assert.Equal(info.Code, instruction2.Code);
						}
						Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
					}
					else {
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
						}
						{
							bytes[lIndex] ^= 4;
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.NotEqual(info.Code, instruction.Code);
						}
					}
				}
				else if (encoding == EncodingKind.Legacy || encoding == EncodingKind.D3NOW)
					continue;
				else if (encoding == EncodingKind.MVEX)
					continue;
				else
					throw new InvalidOperationException();
			}
		}

		static bool HasIs4OrIs5Operands(OpCodeInfo opCode) {
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.xmm_is4:
				case OpCodeOperandKind.xmm_is5:
				case OpCodeOperandKind.ymm_is4:
				case OpCodeOperandKind.ymm_is5:
					return true;
				default:
					break;
				}
			}
			return false;
		}

		[Fact]
		void Test_is4_is5_instructions_ignore_bit7_in_1632mode() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if (info.Bitness != 16 && info.Bitness != 32)
					continue;
				var opCode = info.Code.ToOpCode();
				if (!HasIs4OrIs5Operands(opCode))
					continue;
				var bytes = HexUtils.ToByteArray(info.HexBytes);
				Instruction instruction1, instruction2;
				{
					var decoder = Decoder.Create(info.Bitness, bytes, info.Options);
					decoder.Decode(out instruction1);
				}
				bytes[bytes.Length - 1] ^= 0x80;
				{
					var decoder = Decoder.Create(info.Bitness, bytes, info.Options);
					decoder.Decode(out instruction2);
				}
				Assert.Equal(info.Code, instruction1.Code);
				Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
			}
		}

		[Fact]
		void Test_EVEX_k1_z_bits() {
			var p2Values_k1z = new (bool valid, byte bits)[] { (true, 0x00), (true, 0x01), (false, 0x80), (true, 0x86) };
			var p2Values_k1 = new (bool valid, byte bits)[] { (true, 0x00), (true, 0x01), (false, 0x80), (false, 0x86) };
			var p2Values_k1_fk = new (bool valid, byte bits)[] { (false, 0x00), (true, 0x01), (false, 0x80), (false, 0x86) };
			var p2Values_nothing = new (bool valid, byte bits)[] { (true, 0x00), (false, 0x01), (false, 0x80), (false, 0x86) };
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();
				if (opCode.Encoding != EncodingKind.EVEX)
					continue;
				var bytes = HexUtils.ToByteArray(info.HexBytes);
				int evexIndex = GetEvexIndex(bytes);
				(bool valid, byte bits)[] p2Values;
				if (opCode.CanUseZeroingMasking) {
					Assert.True(opCode.CanUseOpMaskRegister);
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
					decoder.Decode(out var instruction);
					Debug.Assert(instruction.Code != Code.INVALID);
					if (instruction.Op0Kind == OpKind.Memory)
						p2Values = p2Values_k1;
					else
						p2Values = p2Values_k1z;
				}
				else if (opCode.CanUseOpMaskRegister) {
					if (opCode.RequireOpMaskRegister)
						p2Values = p2Values_k1_fk;
					else
						p2Values = p2Values_k1;
				}
				else
					p2Values = p2Values_nothing;

				var b = bytes[evexIndex + 3];
				foreach (var p2v in p2Values) {
					for (int i = 0; i < 2; i++) {
						bytes[evexIndex + 3] = (byte)((b & ~0x87U) | p2v.bits);
						var options = info.Options;
						if (i == 1)
							options |= DecoderOptions.NoInvalidCheck;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), options);
						decoder.Decode(out var instruction);
						if (p2v.valid || (options & DecoderOptions.NoInvalidCheck) != 0) {
							Assert.Equal(info.Code, instruction.Code);
							Assert.Equal((p2v.bits & 0x80) != 0, instruction.ZeroingMasking);
							if ((p2v.bits & 7) != 0)
								Assert.Equal(Register.K0 + (p2v.bits & 7), instruction.OpMask);
							else
								Assert.Equal(Register.None, instruction.OpMask);
						}
						else {
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
					}
				}
			}
		}

		[Fact]
		void Test_EVEX_b_bit() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();
				if (opCode.Encoding != EncodingKind.EVEX)
					continue;
				var bytes = HexUtils.ToByteArray(info.HexBytes);
				int evexIndex = GetEvexIndex(bytes);

				bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
				bool isSaeOrEr = isRegOnly && (opCode.CanUseRoundingControl || opCode.CanSuppressAllExceptions);
				bool newCodeSaeOrEr = TryGetSaeErInstruction(opCode, out var newCode);

				if (opCode.CanBroadcast && !isRegOnly) {
					{
						bytes[evexIndex + 3] &= 0xEF;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.False(instruction.IsBroadcast);
					}
					{
						bytes[evexIndex + 3] |= 0x10;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(instruction.IsBroadcast);
					}
				}
				else {
					if (!isSaeOrEr) {
						bytes[evexIndex + 3] &= 0xEF;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.False(instruction.IsBroadcast);
					}
					{
						bytes[evexIndex + 3] |= 0x10;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						if (isSaeOrEr)
							Assert.Equal(info.Code, instruction.Code);
						else if (newCodeSaeOrEr && isRegOnly)
							Assert.Equal(newCode, instruction.Code);
						else {
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
						Assert.False(instruction.IsBroadcast);
					}
					{
						bytes[evexIndex + 3] |= 0x10;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instruction);
						if (newCodeSaeOrEr && isRegOnly)
							Assert.Equal(newCode, instruction.Code);
						else
							Assert.Equal(info.Code, instruction.Code);
						Assert.False(instruction.IsBroadcast);
					}
				}
			}
		}

		static bool TryGetSaeErInstruction(OpCodeInfo opCode, out Code newCode) {
			if (opCode.Encoding == EncodingKind.EVEX && !(opCode.CanSuppressAllExceptions || opCode.CanUseRoundingControl)) {
				var mnemonic = opCode.Mnemonic;
				for (int i = (int)opCode.Code + 1, j = 1; i < IcedConstants.CodeEnumCount && j <= 2; i++, j++) {
					var nextCode = (Code)i;
					if (nextCode.Mnemonic() != mnemonic)
						break;
					var nextOpCode = nextCode.ToOpCode();
					if (nextOpCode.Encoding != opCode.Encoding)
						break;
					if (nextOpCode.CanSuppressAllExceptions || nextOpCode.CanUseRoundingControl) {
						newCode = nextCode;
						return true;
					}
				}
			}
			newCode = Code.INVALID;
			return false;
		}

		[Fact]
		void Verify_tuple_type_bcst() {
			var codeNames = ToEnumConverter.GetCodeNames();
			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				if (CodeUtils.IsIgnored(codeNames[i]))
					continue;
				var opCode = ((Code)i).ToOpCode();
				bool expectedBcst;
				switch (opCode.TupleType) {
				case TupleType.N8b4:
				case TupleType.N16b4:
				case TupleType.N32b4:
				case TupleType.N64b4:
				case TupleType.N16b8:
				case TupleType.N32b8:
				case TupleType.N64b8:
				case TupleType.N4b2:
				case TupleType.N8b2:
				case TupleType.N16b2:
				case TupleType.N32b2:
				case TupleType.N64b2:
					expectedBcst = true;
					break;
				default:
					expectedBcst = false;
					break;
				}
				Assert.Equal(expectedBcst, opCode.CanBroadcast);
			}
		}

		[Fact]
		void Verify_invalid_vvvv() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();

				switch (opCode.Encoding) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					continue;

				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
				case EncodingKind.MVEX:
					break;

				default:
					throw new InvalidOperationException();
				}

				Get_Vvvvv_info(opCode, out var uses_vvvv, out var isVsib, out var vvvv_mask);

				if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					int b2i = vexIndex + 1;
					if (bytes[vexIndex] != 0xC5)
						b2i++;
					var b2 = bytes[b2i];
					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					bool isVEX2 = bytes[vexIndex] == 0xC5;
					uint b2_mask = info.Bitness == 64 || !isVEX2 ? 0x78U : 0x38;
					if (uses_vvvv) {
						bytes[b2i] = (byte)((b2 & ~b2_mask) | (b2_mask & ~(vvvv_mask << 3)));
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
						}
						if (info.Bitness != 64 && !isVEX2) {
							// vvvv[3] is ignored in 16/32-bit modes, clear it (it's inverted, so 'set' it)
							bytes[b2i] = (byte)(b2 & ~0x40);
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
						if (info.Bitness == 64 && vvvv_mask != 0xF) {
							bytes[b2i] = (byte)(b2 & ~b2_mask);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
							}
						}
					}
					else {
						bytes[b2i] = (byte)(b2 & ~b2_mask);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
					}
				}
				else if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					var b2 = bytes[evexIndex + 2];
					var b3 = bytes[evexIndex + 3];
					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}

					bytes[evexIndex + 2] = (byte)(b2 & 0x87);
					if (!isVsib) {
						bytes[evexIndex + 3] = (byte)(b3 & 0xF7);
					}
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						if (info.Bitness != 64) {
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
						else if (uses_vvvv) {
							if (vvvv_mask != 0x1F)
								Assert.Equal(Code.INVALID, instruction.Code);
							else
								Assert.Equal(info.Code, instruction.Code);
						}
						else {
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
							decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
							decoder.Decode(out instruction);
							Assert.Equal(info.Code, instruction.Code);
						}
					}
					if (!uses_vvvv && info.Bitness == 64) {
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}

					// vvvv[3] isn't ignored in 16/32-bit mode if the operand doesn't use the vvvv bits
					bytes[evexIndex + 2] = (byte)(b2 & 0xBF);
					bytes[evexIndex + 3] = b3;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						if (uses_vvvv) {
							if (vvvv_mask != 0x1F)
								Assert.Equal(Code.INVALID, instruction.Code);
							else
								Assert.Equal(info.Code, instruction.Code);
						}
						else {
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
							decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
							decoder.Decode(out instruction);
							Assert.Equal(info.Code, instruction.Code);
						}
					}
					if (!uses_vvvv) {
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}

					// V' must be 1 in 16/32-bit modes
					bytes[evexIndex + 2] = b2;
					bytes[evexIndex + 3] = (byte)(b3 & 0xF7);
					if (info.Bitness != 64) {
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(Code.INVALID, instruction.Code);
						Assert.NotEqual(DecoderError.None, decoder.LastError);
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		static void Get_Vvvvv_info(OpCodeInfo opCode, out bool uses_vvvv, out bool isVsib, out uint vvvv_mask) {
			uses_vvvv = false;
			isVsib = false;
			switch (opCode.Encoding) {
			case EncodingKind.EVEX:
			case EncodingKind.MVEX:
				vvvv_mask = 0x1F;
				break;
			case EncodingKind.VEX:
			case EncodingKind.XOP:
				vvvv_mask = 0xF;
				break;
			case EncodingKind.Legacy:
			case EncodingKind.D3NOW:
			default:
				throw new InvalidOperationException();
			}
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib32z:
				case OpCodeOperandKind.mem_vsib64z:
					isVsib = true;
					break;
				case OpCodeOperandKind.k_vvvv:
				case OpCodeOperandKind.tmm_vvvv:
					uses_vvvv = true;
					vvvv_mask = 0x7;
					break;
				case OpCodeOperandKind.r32_vvvv:
				case OpCodeOperandKind.r64_vvvv:
				case OpCodeOperandKind.xmm_vvvv:
				case OpCodeOperandKind.xmmp3_vvvv:
				case OpCodeOperandKind.ymm_vvvv:
				case OpCodeOperandKind.zmm_vvvv:
				case OpCodeOperandKind.zmmp3_vvvv:
					uses_vvvv = true;
					break;
				}
			}
		}

		[Fact]
		void Verify_GPR_RRXB_bits() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();

				switch (opCode.Encoding) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					continue;

				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
				case EncodingKind.MVEX:
					break;

				default:
					throw new InvalidOperationException();
				}

				bool uses_rm = false;
				bool uses_reg = false;
				bool other_rm = false;
				bool other_reg = false;
				bool mem_only = false;
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
					case OpCodeOperandKind.sibmem:
						mem_only = true;
						break;
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
						uses_rm = true;
						break;
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
						uses_reg = true;
						break;
					case OpCodeOperandKind.k_or_mem:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.tmm_rm:
						other_rm = true;
						break;
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.kp1_reg:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.tmm_reg:
						other_reg = true;
						break;
					}
				}
				if (mem_only) {
					if (uses_reg)
						uses_rm = true;
					if (other_reg)
						other_rm = true;
				}
				if (!uses_rm && !uses_reg && opCode.OpCount > 0)
					continue;

				if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					bool isVEX2 = bytes[vexIndex] == 0xC5;
					int mrmi = vexIndex + 3 + (isVEX2 ? 0 : 1);
					bool isRegOnly = mrmi >= bytes.Length || (bytes[mrmi] >> 6) == 3;
					var b1 = bytes[vexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
					}
					else if (!other_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}
					if (uses_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly)
								Assert.False(Instruction.EqualsAllBits(origInstr, instruction));
						}
					}
					else if (!other_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x80;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}
				}
				else if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
					var b1 = bytes[evexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
					}
					else if (!other_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[evexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}
					if (uses_reg) {
						if (info.Bitness == 64) {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
							}
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x80);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
							}
						}
						else {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
							}
						}
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Verify_K_reg_RRXB_bits() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;

				var opCode = info.Code.ToOpCode();

				switch (opCode.Encoding) {
				case EncodingKind.Legacy:
				case EncodingKind.D3NOW:
					continue;

				case EncodingKind.VEX:
				case EncodingKind.EVEX:
				case EncodingKind.XOP:
				case EncodingKind.MVEX:
					break;

				default:
					throw new InvalidOperationException();
				}

				bool uses_rm = false;
				bool maybe_uses_rm = false;
				bool uses_reg = false;
				bool other_rm = false;
				bool other_reg = false;
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
						maybe_uses_rm = true;
						break;
					case OpCodeOperandKind.k_or_mem:
					case OpCodeOperandKind.k_rm:
						uses_rm = true;
						break;
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.kp1_reg:
						uses_reg = true;
						break;

					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.tmm_rm:
						other_rm = true;
						break;
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.tmm_reg:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
						other_reg = true;
						break;
					}
				}
				if (uses_reg && maybe_uses_rm)
					uses_rm = true;
				if (!uses_rm && !uses_reg && opCode.OpCount > 0)
					continue;

				if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);
					bool isVEX2 = bytes[vexIndex] == 0xC5;
					int mrmi = vexIndex + 3 + (isVEX2 ? 0 : 1);
					bool isRegOnly = mrmi >= bytes.Length || (bytes[mrmi] >> 6) == 3;
					var b1 = bytes[vexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
					}
					else if (!other_rm && !isVEX2) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}
					if (uses_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness == 64) {
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
								if (isRegOnly)
									Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
							}
						}
					}
					else if (!other_reg) {
						bytes[vexIndex + 1] = (byte)(b1 ^ 0x80);
						if (info.Bitness != 64)
							bytes[vexIndex + 1] |= 0x80;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}
				}
				else if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					bool isRegOnly = (bytes[evexIndex + 5] >> 6) == 3;
					var b1 = bytes[evexIndex + 1];

					Instruction origInstr;
					{
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out origInstr);
						Assert.Equal(info.Code, origInstr.Code);
					}
					if (uses_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x20);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly && info.Bitness != 64)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x40);
						if (info.Bitness == 64) {
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(info.Code, instruction.Code);
							if (isRegOnly)
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
						}
					}
					else if (!other_rm) {
						bytes[evexIndex + 1] = (byte)(b1 ^ 0x60);
						if (info.Bitness != 64)
							bytes[evexIndex + 1] |= 0x40;
						var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
						decoder.Decode(out var instruction);
						Assert.Equal(info.Code, instruction.Code);
						Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
					}
					if (uses_reg) {
						if (info.Bitness == 64) {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
							}
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x80);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
							}
						}
						else {
							bytes[evexIndex + 1] = (byte)(b1 ^ 0x10);
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
								Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
							}
						}
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Verify_vsib_with_invalid_index_register_EVEX() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;
				var opCode = info.Code.ToOpCode();
				if (!CanHaveInvalidIndexRegister_EVEX(opCode))
					continue;

				if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int evexIndex = GetEvexIndex(bytes);
					var p0 = bytes[evexIndex + 1];
					var p2 = bytes[evexIndex + 3];
					var m = bytes[evexIndex + 5];
					var s = bytes[evexIndex + 6];
					for (int i = 0; i < 32; i++) {
						int regNum = info.Bitness == 64 ? i : i & 7;
						bool alwaysInvalid = info.Bitness != 64 && (i & 0x10) != 0;
						int t = i ^ 0x1F;
						// reg  = R' R modrm.reg
						// vidx = V' X sib.index
						bytes[evexIndex + 1] = (byte)((p0 & ~0xD0) | /*R'*/(t & 0x10) | /*R*/((t & 0x08) << 4) | /*X*/((t & 0x08) << 3));
						if (info.Bitness != 64)
							bytes[evexIndex + 1] |= 0xC0;
						bytes[evexIndex + 3] = (byte)((p2 & ~0x08) | /*V'*/((t & 0x10) >> 1));
						bytes[evexIndex + 5] = (byte)((m & 0xC7) | /*modrm.reg*/((i & 7) << 3));
						bytes[evexIndex + 6] = (byte)((s & 0xC7) | /*sib.index*/((i & 7) << 3));

						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instruction);
							if (alwaysInvalid) {
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
							else {
								Assert.Equal(info.Code, instruction.Code);
								Assert.Equal(OpKind.Register, instruction.Op0Kind);
								Assert.Equal(OpKind.Memory, instruction.Op1Kind);
								Assert.NotEqual(Register.None, instruction.MemoryIndex);
								Assert.Equal(regNum, GetNumber(instruction.Op0Register));
								Assert.Equal(regNum, GetNumber(instruction.MemoryIndex));
							}
						}
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		// All Vk_VSIB instructions, eg. EVEX_Vpgatherdd_xmm_k1_vm32x
		static bool CanHaveInvalidIndexRegister_EVEX(OpCodeInfo opCode) {
			if (opCode.Encoding != EncodingKind.EVEX && opCode.Encoding != EncodingKind.MVEX)
				return false;

			switch (opCode.Op0Kind) {
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.zmm_reg:
				break;
			default:
				return false;
			}
			return opCode.RequiresUniqueRegNums;
		}

		[Fact]
		void Verify_vsib_with_invalid_index_mask_dest_register_VEX() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;
				var opCode = info.Code.ToOpCode();
				if (!CanHaveInvalidIndexMaskDestRegister_VEX(opCode))
					continue;

				if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var bytes = HexUtils.ToByteArray(info.HexBytes);
					int vexIndex = GetVexXopIndex(bytes);

					bool isVEX2 = bytes[vexIndex] == 0xC5;
					int rIndex = vexIndex + 1;
					int vIndex = isVEX2 ? rIndex : rIndex + 1;
					int mIndex = vIndex + 2;
					int sIndex = vIndex + 3;

					var r = bytes[rIndex];
					var v = bytes[vIndex];
					var m = bytes[mIndex];
					var s = bytes[sIndex];

					const int reg_eq_vvvv = 0;
					const int reg_eq_vidx = 1;
					const int vvvv_eq_vidx = 2;
					const int all_eq_all = 3;
					foreach (var testKind in new[] { reg_eq_vvvv, reg_eq_vidx, vvvv_eq_vidx, all_eq_all }) {
						for (int i = 0; i < 16; i++) {
							int regNum = info.Bitness == 64 ? i : i & 7;
							// Use a small number (0-7) in case it's VEX2 and 'other' is vidx (uses VEX.X bit)
							int other = regNum == 0 ? 1 : 0;
							int newReg, newVvvv, newVidx;

							switch (testKind) {
							case reg_eq_vvvv:
								newReg = newVvvv = regNum;
								newVidx = other;
								break;
							case reg_eq_vidx:
								newReg = newVidx = regNum;
								newVvvv = other;
								break;
							case vvvv_eq_vidx:
								newVvvv = newVidx = regNum;
								newReg = other;
								break;
							case all_eq_all:
								newReg = newVvvv = newVidx = regNum;
								break;
							default:
								throw new InvalidOperationException();
							}

							// reg  = R modrm.reg
							// vidx = X sib.index
							if (isVEX2) {
								if (newVidx >= 8)
									continue;
								bytes[rIndex] = (byte)((r & 0x07) | /*R*/(((newReg ^ 8) & 0x8) << 4) | /*vvvv*/((newVvvv ^ 0xF) << 3));
							}
							else {
								bytes[rIndex] = (byte)((r & 0x3F) | /*R*/(((newReg ^ 8) & 8) << 4) | /*X*/(((newVidx ^ 8) & 8) << 3));
								bytes[vIndex] = (byte)((v & 0x87) | /*vvvv*/((newVvvv ^ 0xF) << 3));
							}
							bytes[mIndex] = (byte)((m & 0xC7) | /*modrm.reg*/((newReg & 7) << 3));
							bytes[sIndex] = (byte)((s & 0xC7) | /*sib.index*/((newVidx & 7) << 3));

							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
								decoder.Decode(out var instruction);
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
							{
								var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
								decoder.Decode(out var instruction);
								Assert.Equal(info.Code, instruction.Code);
								Assert.Equal(OpKind.Register, instruction.Op0Kind);
								Assert.Equal(OpKind.Memory, instruction.Op1Kind);
								Assert.Equal(OpKind.Register, instruction.Op2Kind);
								Assert.NotEqual(Register.None, instruction.MemoryIndex);
								Assert.Equal(newReg, GetNumber(instruction.Op0Register));
								Assert.Equal(newVidx, GetNumber(instruction.MemoryIndex));
								Assert.Equal(newVvvv, GetNumber(instruction.Op2Register));
							}
						}
					}
				}
				else
					throw new InvalidOperationException();
			}
		}

		// All VX_VSIB_HX instructions, eg. VEX_Vpgatherdd_xmm_vm32x_xmm
		static bool CanHaveInvalidIndexMaskDestRegister_VEX(OpCodeInfo opCode) {
			if (opCode.Encoding != EncodingKind.VEX && opCode.Encoding != EncodingKind.XOP)
				return false;

			switch (opCode.Op0Kind) {
			case OpCodeOperandKind.xmm_reg:
			case OpCodeOperandKind.ymm_reg:
			case OpCodeOperandKind.zmm_reg:
				break;
			default:
				return false;
			}

			return opCode.RequiresUniqueRegNums;
		}

		static bool IsVsib(OpCodeInfo opCode) => TryGetVsib(opCode, out _, out _);

		static bool TryGetVsib(OpCodeInfo opCode, out bool isVsib32, out bool isVsib64) {
			for (int i = 0; i < opCode.OpCount; i++) {
				switch (opCode.GetOpKind(i)) {
				case OpCodeOperandKind.mem_vsib32x:
				case OpCodeOperandKind.mem_vsib32y:
				case OpCodeOperandKind.mem_vsib32z:
					isVsib32 = true;
					isVsib64 = false;
					return true;

				case OpCodeOperandKind.mem_vsib64x:
				case OpCodeOperandKind.mem_vsib64y:
				case OpCodeOperandKind.mem_vsib64z:
					isVsib32 = false;
					isVsib64 = true;
					return true;
				}
			}

			isVsib32 = false;
			isVsib64 = false;
			return false;
		}

		[Fact]
		void Test_vsib_props() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
				decoder.Decode(out var instruction);
				Assert.Equal(info.Code, instruction.Code);

				bool isVsib = TryGetVsib(info.Code.ToOpCode(), out var isVsib32, out var isVsib64);
				Assert.Equal(instruction.IsVsib, isVsib);
				Assert.Equal(instruction.IsVsib32, isVsib32);
				Assert.Equal(instruction.IsVsib64, isVsib64);
			}
		}

		struct TestedInfo {
			// VEX/XOP.L and EVEX.L'L values
			public uint LBits;// bit 0 = L0/L128, bit 1 = L1/L256, etc
			public uint VEX2_LBits;

			// REX/VEX/XOP/EVEX/MVEX: W values
			public uint WBits;// bit 0 = W0, bit 1 = W1

			// REX/VEX/XOP/EVEX/MVEX.R
			public uint RBits;
			public uint VEX2_RBits;
			// REX/VEX/XOP/EVEX/MVEX.X
			public uint XBits;
			// REX/VEX/XOP/EVEX/MVEX.B
			public uint BBits;
			// EVEX/MVEX.R'
			public uint R2Bits;
			// EVEX/MVEX.V'
			public uint V2Bits;

			// mod=11
			public bool RegReg;
			// mod=00,01,10
			public bool RegMem;

			// EVEX/MVEX only
			public bool MemDisp8;

			// Tested VEX2 prefix
			public bool VEX2;
			// Tested VEX3 prefix
			public bool VEX3;

			// EVEX/MVEX: tested opmask
			public bool OpMask;
			// EVEX/MVEX: tested no opmask
			public bool NoOpMask;

			public bool PrefixXacquire;
			public bool PrefixNoXacquire;
			public bool PrefixXrelease;
			public bool PrefixNoXrelease;
			public bool PrefixLock;
			public bool PrefixNoLock;
			public bool PrefixHnt;
			public bool PrefixNoHnt;
			public bool PrefixHt;
			public bool PrefixNoHt;
			public bool PrefixRep;
			public bool PrefixNoRep;
			public bool PrefixRepne;
			public bool PrefixNoRepne;
			public bool PrefixNotrack;
			public bool PrefixNoNotrack;
			public bool PrefixBnd;
			public bool PrefixNoBnd;
		}
		[Fact]
		void Verify_that_test_cases_test_enough_bits() {
			var testedInfos16 = new TestedInfo[IcedConstants.CodeEnumCount];
			var testedInfos32 = new TestedInfo[IcedConstants.CodeEnumCount];
			var testedInfos64 = new TestedInfo[IcedConstants.CodeEnumCount];

			var canUseW = new bool[IcedConstants.CodeEnumCount];
			{
				var usesW = new HashSet<(OpCodeTableKind table, uint opCode)>();
				for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
					var code = (Code)i;
					var opCode = code.ToOpCode();
					if (opCode.Encoding != EncodingKind.Legacy)
						continue;
					if (opCode.OperandSize != 0)
						usesW.Add((opCode.Table, opCode.OpCode));
				}
				for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
					var code = (Code)i;
					var opCode = code.ToOpCode();
					switch (opCode.Encoding) {
					case EncodingKind.Legacy:
					case EncodingKind.D3NOW:
						canUseW[i] = !usesW.Contains((opCode.Table, opCode.OpCode));
						break;

					case EncodingKind.VEX:
					case EncodingKind.EVEX:
					case EncodingKind.XOP:
					case EncodingKind.MVEX:
						break;

					default:
						throw new InvalidOperationException();
					}
				}
			}

			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;
				var testedInfos = info.Bitness switch {
					16 => testedInfos16,
					32 => testedInfos32,
					64 => testedInfos64,
					_ => throw new InvalidOperationException(),
				};

				var opCode = info.Code.ToOpCode();
				ref var tested = ref testedInfos[(int)info.Code];

				var bytes = HexUtils.ToByteArray(info.HexBytes);
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
				decoder.Decode(out var instruction);
				Assert.Equal(info.Code, instruction.Code);

				if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX) {
					int evexIndex = GetEvexIndex(bytes);

					if (opCode.Encoding == EncodingKind.EVEX) {
						if (instruction.RoundingControl == RoundingControl.None)
							tested.LBits |= 1U << ((bytes[evexIndex + 3] >> 5) & 3);

						var ll = (bytes[evexIndex + 3] >> 5) & 3;
						bool invalid = (info.Options & DecoderOptions.NoInvalidCheck) == 0 &&
							ll == 3 && (bytes[evexIndex + 5] < 0xC0 || (bytes[evexIndex + 3] & 0x10) == 0);
						if (!invalid)
							tested.LBits |= 1U << 3;
					}

					tested.WBits |= 1U << (bytes[evexIndex + 2] >> 7);
					tested.RBits |= 1U << ((bytes[evexIndex + 1] >> 7) ^ 1);
					tested.XBits |= 1U << (((bytes[evexIndex + 1] >> 6) & 1) ^ 1);
					tested.BBits |= 1U << (((bytes[evexIndex + 1] >> 5) & 1) ^ 1);
					tested.R2Bits |= 1U << (((bytes[evexIndex + 1] >> 4) & 1) ^ 1);
					tested.V2Bits |= 1U << (((bytes[evexIndex + 3] >> 3) & 1) ^ 1);
					if ((bytes[evexIndex + 5] >> 6) != 3) {
						tested.RegMem = true;
						if (instruction.MemoryDisplSize == 1 && instruction.MemoryDisplacement64 != 0)
							tested.MemDisp8 = true;
					}
					else
						tested.RegReg = true;
					if (instruction.OpMask != Register.None)
						tested.OpMask = true;
					else
						tested.NoOpMask = true;
				}
				else if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					int vexIndex = GetVexXopIndex(bytes);
					int mrmi;
					if (bytes[vexIndex] == 0xC5) {
						mrmi = vexIndex + 3;
						tested.VEX2 = true;
						tested.VEX2_RBits |= 1U << ((bytes[vexIndex + 1] >> 7) ^ 1);
						tested.VEX2_LBits |= 1U << ((bytes[vexIndex + 1] >> 2) & 1);
					}
					else {
						mrmi = vexIndex + 4;
						if (opCode.Encoding == EncodingKind.VEX)
							tested.VEX3 = true;
						tested.RBits |= 1U << ((bytes[vexIndex + 1] >> 7) ^ 1);
						tested.XBits |= 1U << (((bytes[vexIndex + 1] >> 6) & 1) ^ 1);
						tested.BBits |= 1U << (((bytes[vexIndex + 1] >> 5) & 1) ^ 1);
						tested.WBits |= 1U << (bytes[vexIndex + 2] >> 7);
						tested.LBits |= 1U << ((bytes[vexIndex + 2] >> 2) & 1);
					}
					if (HasModRM(opCode)) {
						if ((bytes[mrmi] >> 6) != 3)
							tested.RegMem = true;
						else
							tested.RegReg = true;
					}
				}
				else if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW) {
					int i = SkipPrefixes(bytes, info.Bitness, out var rex);
					if (info.Bitness == 64) {
						tested.WBits |= 1U << (int)((rex >> 3) & 1);
						tested.RBits |= 1U << (int)((rex >> 2) & 1);
						tested.XBits |= 1U << (int)((rex >> 1) & 1);
						tested.BBits |= 1U << (int)(rex & 1);
						// Can't access regs dr8-dr15
						if (info.Code == Code.Mov_r64_dr || info.Code == Code.Mov_dr_r64)
							tested.RBits |= 1U << 1;
					}
					else {
						tested.WBits |= 1;
						tested.RBits |= 1;
						tested.XBits |= 1;
						tested.BBits |= 1;
					}
					if (HasModRM(opCode)) {
						switch (opCode.Table) {
						case OpCodeTableKind.Normal:
							break;
						case OpCodeTableKind.T0F:
							if (bytes[i++] != 0x0F)
								throw new InvalidOperationException();
							break;
						case OpCodeTableKind.T0F38:
							if (bytes[i++] != 0x0F)
								throw new InvalidOperationException();
							if (bytes[i++] != 0x38)
								throw new InvalidOperationException();
							break;
						case OpCodeTableKind.T0F3A:
							if (bytes[i++] != 0x0F)
								throw new InvalidOperationException();
							if (bytes[i++] != 0x3A)
								throw new InvalidOperationException();
							break;
						default:
							throw new InvalidOperationException();
						}
						i++;
						if ((bytes[i] >> 6) != 3)
							tested.RegMem = true;
						else
							tested.RegReg = true;
					}
					if (opCode.CanUseXacquirePrefix) {
						if (instruction.HasXacquirePrefix)
							tested.PrefixXacquire = true;
						else
							tested.PrefixNoXacquire = true;
					}
					if (opCode.CanUseXreleasePrefix) {
						if (instruction.HasXreleasePrefix)
							tested.PrefixXrelease = true;
						else
							tested.PrefixNoXrelease = true;
					}
					if (opCode.CanUseLockPrefix) {
						if (instruction.HasLockPrefix)
							tested.PrefixLock = true;
						else
							tested.PrefixNoLock = true;
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (instruction.SegmentPrefix == Register.CS)
							tested.PrefixHnt = true;
						else
							tested.PrefixNoHnt = true;
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (instruction.SegmentPrefix == Register.DS)
							tested.PrefixHt = true;
						else
							tested.PrefixNoHt = true;
					}
					if (opCode.CanUseRepPrefix) {
						if (instruction.HasRepPrefix)
							tested.PrefixRep = true;
						else
							tested.PrefixNoRep = true;
					}
					if (opCode.CanUseRepnePrefix) {
						if (instruction.HasRepnePrefix)
							tested.PrefixRepne = true;
						else
							tested.PrefixNoRepne = true;
					}
					if (opCode.CanUseNotrackPrefix) {
						if (instruction.SegmentPrefix == Register.DS)
							tested.PrefixNotrack = true;
						else
							tested.PrefixNoNotrack = true;
					}
					if (opCode.CanUseBndPrefix) {
						if (instruction.HasRepnePrefix)
							tested.PrefixBnd = true;
						else
							tested.PrefixNoBnd = true;
					}
				}
				else
					throw new InvalidOperationException();
			}

			var wig32_16 = new List<Code>();
			var wig32_32 = new List<Code>();

			var wig_16 = new List<Code>();
			var wig_32 = new List<Code>();
			var wig_64 = new List<Code>();

			var w_64 = new List<Code>();

			var lig_16 = new List<Code>();
			var lig_32 = new List<Code>();
			var lig_64 = new List<Code>();

			var vex2_lig_16 = new List<Code>();
			var vex2_lig_32 = new List<Code>();
			var vex2_lig_64 = new List<Code>();

			var rr_16 = new List<Code>();
			var rr_32 = new List<Code>();
			var rr_64 = new List<Code>();

			var rm_16 = new List<Code>();
			var rm_32 = new List<Code>();
			var rm_64 = new List<Code>();

			var disp8_16 = new List<Code>();
			var disp8_32 = new List<Code>();
			var disp8_64 = new List<Code>();

			var vex2_16 = new List<Code>();
			var vex2_32 = new List<Code>();
			var vex2_64 = new List<Code>();

			var vex3_16 = new List<Code>();
			var vex3_32 = new List<Code>();
			var vex3_64 = new List<Code>();

			var opmask_16 = new List<Code>();
			var opmask_32 = new List<Code>();
			var opmask_64 = new List<Code>();

			var noopmask_16 = new List<Code>();
			var noopmask_32 = new List<Code>();
			var noopmask_64 = new List<Code>();

			var b_16 = new List<Code>();
			var b_32 = new List<Code>();
			var b_64 = new List<Code>();

			var r2_16 = new List<Code>();
			var r2_32 = new List<Code>();
			var r2_64 = new List<Code>();

			var r_64 = new List<Code>();
			var vex2_r_64 = new List<Code>();
			var x_64 = new List<Code>();
			var v2_64 = new List<Code>();

			var pfx_xacquire_16 = new List<Code>();
			var pfx_xacquire_32 = new List<Code>();
			var pfx_xacquire_64 = new List<Code>();

			var pfx_xrelease_16 = new List<Code>();
			var pfx_xrelease_32 = new List<Code>();
			var pfx_xrelease_64 = new List<Code>();

			var pfx_lock_16 = new List<Code>();
			var pfx_lock_32 = new List<Code>();
			var pfx_lock_64 = new List<Code>();

			var pfx_hnt_16 = new List<Code>();
			var pfx_hnt_32 = new List<Code>();
			var pfx_hnt_64 = new List<Code>();

			var pfx_ht_16 = new List<Code>();
			var pfx_ht_32 = new List<Code>();
			var pfx_ht_64 = new List<Code>();

			var pfx_rep_16 = new List<Code>();
			var pfx_rep_32 = new List<Code>();
			var pfx_rep_64 = new List<Code>();

			var pfx_repne_16 = new List<Code>();
			var pfx_repne_32 = new List<Code>();
			var pfx_repne_64 = new List<Code>();

			var pfx_notrack_16 = new List<Code>();
			var pfx_notrack_32 = new List<Code>();
			var pfx_notrack_64 = new List<Code>();

			var pfx_bnd_16 = new List<Code>();
			var pfx_bnd_32 = new List<Code>();
			var pfx_bnd_64 = new List<Code>();

			var pfx_no_xacquire_16 = new List<Code>();
			var pfx_no_xacquire_32 = new List<Code>();
			var pfx_no_xacquire_64 = new List<Code>();

			var pfx_no_xrelease_16 = new List<Code>();
			var pfx_no_xrelease_32 = new List<Code>();
			var pfx_no_xrelease_64 = new List<Code>();

			var pfx_no_lock_16 = new List<Code>();
			var pfx_no_lock_32 = new List<Code>();
			var pfx_no_lock_64 = new List<Code>();

			var pfx_no_hnt_16 = new List<Code>();
			var pfx_no_hnt_32 = new List<Code>();
			var pfx_no_hnt_64 = new List<Code>();

			var pfx_no_ht_16 = new List<Code>();
			var pfx_no_ht_32 = new List<Code>();
			var pfx_no_ht_64 = new List<Code>();

			var pfx_no_rep_16 = new List<Code>();
			var pfx_no_rep_32 = new List<Code>();
			var pfx_no_rep_64 = new List<Code>();

			var pfx_no_repne_16 = new List<Code>();
			var pfx_no_repne_32 = new List<Code>();
			var pfx_no_repne_64 = new List<Code>();

			var pfx_no_notrack_16 = new List<Code>();
			var pfx_no_notrack_32 = new List<Code>();
			var pfx_no_notrack_64 = new List<Code>();

			var pfx_no_bnd_16 = new List<Code>();
			var pfx_no_bnd_32 = new List<Code>();
			var pfx_no_bnd_64 = new List<Code>();

			var codeNames = ToEnumConverter.GetCodeNames();
			foreach (var bitness in new int[] { 16, 32, 64 }) {
				var testedInfos = bitness switch {
					16 => testedInfos16,
					32 => testedInfos32,
					64 => testedInfos64,
					_ => throw new InvalidOperationException(),
				};

				for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
					if (CodeUtils.IsIgnored(codeNames[i]))
						continue;
					var code = (Code)i;
					switch (code) {
					case Code.Montmul_16:
					case Code.Montmul_64:
						continue;
					}
					var opCode = code.ToOpCode();
					if (!opCode.IsInstruction || opCode.Code == Code.Popw_CS)
						continue;
					if (opCode.Fwait)
						continue;

					switch (bitness) {
					case 16:
						if (!opCode.Mode16)
							continue;
						break;
					case 32:
						if (!opCode.Mode32)
							continue;
						break;
					case 64:
						if (!opCode.Mode64)
							continue;
						break;
					default:
						throw new InvalidOperationException();
					}

					ref var tested = ref testedInfos[i];

					if ((bitness == 16 || bitness == 32) && opCode.IsWIG32) {
						if (tested.WBits != 3)
							GetList2(bitness, wig32_16, wig32_32).Add(code);
					}
					if (opCode.IsWIG) {
						if (tested.WBits != 3)
							GetList(bitness, wig_16, wig_32, wig_64).Add(code);
					}
					if (bitness == 64 && opCode.Mode64 && (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW)) {
						Debug.Assert(!opCode.IsWIG);
						Debug.Assert(!opCode.IsWIG32);
						if (canUseW[(int)code] && tested.WBits != 3)
							w_64.Add(code);
					}
					if (opCode.IsLIG) {
						uint allLBits;
						switch (opCode.Encoding) {
						case EncodingKind.VEX:
						case EncodingKind.XOP:
							allLBits = 3;// 1 bit = 2 values
							break;

						case EncodingKind.EVEX:
							allLBits = 0xF;// 2 bits = 4 values
							break;

						case EncodingKind.Legacy:
						case EncodingKind.D3NOW:
						case EncodingKind.MVEX:
						default:
							throw new InvalidOperationException();
						}
						if (tested.LBits != allLBits)
							GetList(bitness, lig_16, lig_32, lig_64).Add(code);
					}
					if (opCode.IsLIG && opCode.Encoding == EncodingKind.VEX) {
						if (tested.VEX2_LBits != 3 && CanUseVEX2(opCode))
							GetList(bitness, vex2_lig_16, vex2_lig_32, vex2_lig_64).Add(code);
					}
					if (CanUseModRM_rm_mem(opCode)) {
						if (!tested.RegMem)
							GetList(bitness, rm_16, rm_32, rm_64).Add(code);
					}
					if (CanUseModRM_rm_reg(opCode)) {
						if (!tested.RegReg)
							GetList(bitness, rr_16, rr_32, rr_64).Add(code);
					}
					switch (opCode.Encoding) {
					case EncodingKind.Legacy:
					case EncodingKind.VEX:
					case EncodingKind.XOP:
					case EncodingKind.D3NOW:
						break;
					case EncodingKind.EVEX:
					case EncodingKind.MVEX:
						if (!tested.MemDisp8 && CanUseModRM_rm_mem(opCode))
							GetList(bitness, disp8_16, disp8_32, disp8_64).Add(code);
						break;
					default:
						throw new InvalidOperationException();
					}
					if (opCode.Encoding == EncodingKind.VEX) {
						if (!tested.VEX3)
							GetList(bitness, vex3_16, vex3_32, vex3_64).Add(code);
						if (!tested.VEX2 && CanUseVEX2(opCode))
							GetList(bitness, vex2_16, vex2_32, vex2_64).Add(code);
					}
					if (opCode.CanUseOpMaskRegister) {
						if (!tested.OpMask)
							GetList(bitness, opmask_16, opmask_32, opmask_64).Add(code);
						if (!tested.NoOpMask && !opCode.RequireOpMaskRegister)
							GetList(bitness, noopmask_16, noopmask_32, noopmask_64).Add(code);
					}
					if (CanUseB(bitness, opCode)) {
						if (tested.BBits != 3)
							GetList(bitness, b_16, b_32, b_64).Add(code);
					}
					else {
						if ((tested.BBits & 1) == 0)
							GetList(bitness, b_16, b_32, b_64).Add(code);
					}
					switch (opCode.Encoding) {
					case EncodingKind.EVEX:
					case EncodingKind.MVEX:
						if (CanUseR2(opCode)) {
							if (tested.R2Bits != 3)
								GetList(bitness, r2_16, r2_32, r2_64).Add(code);
						}
						else {
							if ((tested.R2Bits & 1) == 0)
								GetList(bitness, r2_16, r2_32, r2_64).Add(code);
						}
						break;
					case EncodingKind.Legacy:
					case EncodingKind.VEX:
					case EncodingKind.XOP:
					case EncodingKind.D3NOW:
						break;
					default:
						throw new InvalidOperationException();
					}
					if (bitness == 64 && opCode.Mode64) {
						if (tested.VEX2_RBits != 3 && opCode.Encoding == EncodingKind.VEX && CanUseVEX2(opCode) && CanUseR(opCode))
							vex2_r_64.Add(code);
						if (CanUseR(opCode)) {
							if (tested.RBits != 3)
								r_64.Add(code);
						}
						else {
							if ((tested.RBits & 1) == 0)
								r_64.Add(code);
						}
						if (IsVsib(opCode)) {
							// The memory tests test vsib memory operands
						}
						else if (CanUseX(opCode)) {
							if (tested.XBits != 3)
								x_64.Add(code);
						}
						else {
							if ((tested.XBits & 1) == 0)
								x_64.Add(code);
						}
						switch (opCode.Encoding) {
						case EncodingKind.EVEX:
						case EncodingKind.MVEX:
							if (IsVsib(opCode)) {
								// The memory tests test vsib memory operands
							}
							else if (CanUseV2(opCode)) {
								if (tested.V2Bits != 3)
									v2_64.Add(code);
							}
							else {
								if ((tested.V2Bits & 1) == 0)
									v2_64.Add(code);
							}
							break;
						case EncodingKind.Legacy:
						case EncodingKind.VEX:
						case EncodingKind.XOP:
						case EncodingKind.D3NOW:
							break;
						default:
							throw new InvalidOperationException();
						}
					}
					if (opCode.CanUseXacquirePrefix) {
						if (!tested.PrefixXacquire)
							GetList(bitness, pfx_xacquire_16, pfx_xacquire_32, pfx_xacquire_64).Add(code);
						if (!tested.PrefixNoXacquire)
							GetList(bitness, pfx_no_xacquire_16, pfx_no_xacquire_32, pfx_no_xacquire_64).Add(code);
					}
					if (opCode.CanUseXreleasePrefix) {
						if (!tested.PrefixXrelease)
							GetList(bitness, pfx_xrelease_16, pfx_xrelease_32, pfx_xrelease_64).Add(code);
						if (!tested.PrefixNoXrelease)
							GetList(bitness, pfx_no_xrelease_16, pfx_no_xrelease_32, pfx_no_xrelease_64).Add(code);
					}
					if (opCode.CanUseLockPrefix) {
						if (!tested.PrefixLock)
							GetList(bitness, pfx_lock_16, pfx_lock_32, pfx_lock_64).Add(code);
						if (!tested.PrefixNoLock)
							GetList(bitness, pfx_no_lock_16, pfx_no_lock_32, pfx_no_lock_64).Add(code);
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (!tested.PrefixHnt)
							GetList(bitness, pfx_hnt_16, pfx_hnt_32, pfx_hnt_64).Add(code);
						if (!tested.PrefixNoHnt)
							GetList(bitness, pfx_no_hnt_16, pfx_no_hnt_32, pfx_no_hnt_64).Add(code);
					}
					if (opCode.CanUseHintTakenPrefix) {
						if (!tested.PrefixHt)
							GetList(bitness, pfx_ht_16, pfx_ht_32, pfx_ht_64).Add(code);
						if (!tested.PrefixNoHt)
							GetList(bitness, pfx_no_ht_16, pfx_no_ht_32, pfx_no_ht_64).Add(code);
					}
					if (opCode.CanUseRepPrefix) {
						if (!tested.PrefixRep)
							GetList(bitness, pfx_rep_16, pfx_rep_32, pfx_rep_64).Add(code);
						if (!tested.PrefixNoRep)
							GetList(bitness, pfx_no_rep_16, pfx_no_rep_32, pfx_no_rep_64).Add(code);
					}
					if (opCode.CanUseRepnePrefix) {
						if (!tested.PrefixRepne)
							GetList(bitness, pfx_repne_16, pfx_repne_32, pfx_repne_64).Add(code);
						if (!tested.PrefixNoRepne)
							GetList(bitness, pfx_no_repne_16, pfx_no_repne_32, pfx_no_repne_64).Add(code);
					}
					if (opCode.CanUseNotrackPrefix) {
						if (!tested.PrefixNotrack)
							GetList(bitness, pfx_notrack_16, pfx_notrack_32, pfx_notrack_64).Add(code);
						if (!tested.PrefixNoNotrack)
							GetList(bitness, pfx_no_notrack_16, pfx_no_notrack_32, pfx_no_notrack_64).Add(code);
					}
					if (opCode.CanUseBndPrefix) {
						if (!tested.PrefixBnd)
							GetList(bitness, pfx_bnd_16, pfx_bnd_32, pfx_bnd_64).Add(code);
						if (!tested.PrefixNoBnd)
							GetList(bitness, pfx_no_bnd_16, pfx_no_bnd_32, pfx_no_bnd_64).Add(code);
					}
				}
			}

			Assert.Equal("wig32_16:", "wig32_16:" + string.Join(",", wig32_16.ToArray()));
			Assert.Equal("wig32_32:", "wig32_32:" + string.Join(",", wig32_32.ToArray()));
			Assert.Equal("wig_16:", "wig_16:" + string.Join(",", wig_16.ToArray()));
			Assert.Equal("wig_32:", "wig_32:" + string.Join(",", wig_32.ToArray()));
			Assert.Equal("wig_64:", "wig_64:" + string.Join(",", wig_64.ToArray()));
			Assert.Equal("w_64:", "w_64:" + string.Join(",", w_64.ToArray()));
			Assert.Equal("lig_16:", "lig_16:" + string.Join(",", lig_16.ToArray()));
			Assert.Equal("lig_32:", "lig_32:" + string.Join(",", lig_32.ToArray()));
			Assert.Equal("lig_64:", "lig_64:" + string.Join(",", lig_64.ToArray()));
			Assert.Equal("vex2_lig_16:", "vex2_lig_16:" + string.Join(",", vex2_lig_16.ToArray()));
			Assert.Equal("vex2_lig_32:", "vex2_lig_32:" + string.Join(",", vex2_lig_32.ToArray()));
			Assert.Equal("vex2_lig_64:", "vex2_lig_64:" + string.Join(",", vex2_lig_64.ToArray()));
			Assert.Equal("rr_16:", "rr_16:" + string.Join(",", rr_16.ToArray()));
			Assert.Equal("rr_32:", "rr_32:" + string.Join(",", rr_32.ToArray()));
			Assert.Equal("rr_64:", "rr_64:" + string.Join(",", rr_64.ToArray()));
			Assert.Equal("rm_16:", "rm_16:" + string.Join(",", rm_16.ToArray()));
			Assert.Equal("rm_32:", "rm_32:" + string.Join(",", rm_32.ToArray()));
			Assert.Equal("rm_64:", "rm_64:" + string.Join(",", rm_64.ToArray()));
			Assert.Equal("disp8_16:", "disp8_16:" + string.Join(",", disp8_16.ToArray()));
			Assert.Equal("disp8_32:", "disp8_32:" + string.Join(",", disp8_32.ToArray()));
			Assert.Equal("disp8_64:", "disp8_64:" + string.Join(",", disp8_64.ToArray()));
			Assert.Equal("vex2_16:", "vex2_16:" + string.Join(",", vex2_16.ToArray()));
			Assert.Equal("vex2_32:", "vex2_32:" + string.Join(",", vex2_32.ToArray()));
			Assert.Equal("vex2_64:", "vex2_64:" + string.Join(",", vex2_64.ToArray()));
			Assert.Equal("vex3_16:", "vex3_16:" + string.Join(",", vex3_16.ToArray()));
			Assert.Equal("vex3_32:", "vex3_32:" + string.Join(",", vex3_32.ToArray()));
			Assert.Equal("vex3_64:", "vex3_64:" + string.Join(",", vex3_64.ToArray()));
			Assert.Equal("opmask_16:", "opmask_16:" + string.Join(",", opmask_16.ToArray()));
			Assert.Equal("opmask_32:", "opmask_32:" + string.Join(",", opmask_32.ToArray()));
			Assert.Equal("opmask_64:", "opmask_64:" + string.Join(",", opmask_64.ToArray()));
			Assert.Equal("noopmask_16:", "noopmask_16:" + string.Join(",", noopmask_16.ToArray()));
			Assert.Equal("noopmask_32:", "noopmask_32:" + string.Join(",", noopmask_32.ToArray()));
			Assert.Equal("noopmask_64:", "noopmask_64:" + string.Join(",", noopmask_64.ToArray()));
			Assert.Equal("b_16:", "b_16:" + string.Join(",", b_16.ToArray()));
			Assert.Equal("b_32:", "b_32:" + string.Join(",", b_32.ToArray()));
			Assert.Equal("b_64:", "b_64:" + string.Join(",", b_64.ToArray()));
			Assert.Equal("r2_16:", "r2_16:" + string.Join(",", r2_16.ToArray()));
			Assert.Equal("r2_32:", "r2_32:" + string.Join(",", r2_32.ToArray()));
			Assert.Equal("r2_64:", "r2_64:" + string.Join(",", r2_64.ToArray()));
			Assert.Equal("r_64:", "r_64:" + string.Join(",", r_64.ToArray()));
			Assert.Equal("vex2_r_64:", "vex2_r_64:" + string.Join(",", vex2_r_64.ToArray()));
			Assert.Equal("x_64:", "x_64:" + string.Join(",", x_64.ToArray()));
			Assert.Equal("v2_64:", "v2_64:" + string.Join(",", v2_64.ToArray()));
			Assert.Equal("pfx_xacquire_16:", "pfx_xacquire_16:" + string.Join(",", pfx_xacquire_16.ToArray()));
			Assert.Equal("pfx_xacquire_32:", "pfx_xacquire_32:" + string.Join(",", pfx_xacquire_32.ToArray()));
			Assert.Equal("pfx_xacquire_64:", "pfx_xacquire_64:" + string.Join(",", pfx_xacquire_64.ToArray()));
			Assert.Equal("pfx_xrelease_16:", "pfx_xrelease_16:" + string.Join(",", pfx_xrelease_16.ToArray()));
			Assert.Equal("pfx_xrelease_32:", "pfx_xrelease_32:" + string.Join(",", pfx_xrelease_32.ToArray()));
			Assert.Equal("pfx_xrelease_64:", "pfx_xrelease_64:" + string.Join(",", pfx_xrelease_64.ToArray()));
			Assert.Equal("pfx_lock_16:", "pfx_lock_16:" + string.Join(",", pfx_lock_16.ToArray()));
			Assert.Equal("pfx_lock_32:", "pfx_lock_32:" + string.Join(",", pfx_lock_32.ToArray()));
			Assert.Equal("pfx_lock_64:", "pfx_lock_64:" + string.Join(",", pfx_lock_64.ToArray()));
			Assert.Equal("pfx_hnt_16:", "pfx_hnt_16:" + string.Join(",", pfx_hnt_16.ToArray()));
			Assert.Equal("pfx_hnt_32:", "pfx_hnt_32:" + string.Join(",", pfx_hnt_32.ToArray()));
			Assert.Equal("pfx_hnt_64:", "pfx_hnt_64:" + string.Join(",", pfx_hnt_64.ToArray()));
			Assert.Equal("pfx_ht_16:", "pfx_ht_16:" + string.Join(",", pfx_ht_16.ToArray()));
			Assert.Equal("pfx_ht_32:", "pfx_ht_32:" + string.Join(",", pfx_ht_32.ToArray()));
			Assert.Equal("pfx_ht_64:", "pfx_ht_64:" + string.Join(",", pfx_ht_64.ToArray()));
			Assert.Equal("pfx_rep_16:", "pfx_rep_16:" + string.Join(",", pfx_rep_16.ToArray()));
			Assert.Equal("pfx_rep_32:", "pfx_rep_32:" + string.Join(",", pfx_rep_32.ToArray()));
			Assert.Equal("pfx_rep_64:", "pfx_rep_64:" + string.Join(",", pfx_rep_64.ToArray()));
			Assert.Equal("pfx_repne_16:", "pfx_repne_16:" + string.Join(",", pfx_repne_16.ToArray()));
			Assert.Equal("pfx_repne_32:", "pfx_repne_32:" + string.Join(",", pfx_repne_32.ToArray()));
			Assert.Equal("pfx_repne_64:", "pfx_repne_64:" + string.Join(",", pfx_repne_64.ToArray()));
			Assert.Equal("pfx_notrack_16:", "pfx_notrack_16:" + string.Join(",", pfx_notrack_16.ToArray()));
			Assert.Equal("pfx_notrack_32:", "pfx_notrack_32:" + string.Join(",", pfx_notrack_32.ToArray()));
			Assert.Equal("pfx_notrack_64:", "pfx_notrack_64:" + string.Join(",", pfx_notrack_64.ToArray()));
			Assert.Equal("pfx_bnd_16:", "pfx_bnd_16:" + string.Join(",", pfx_bnd_16.ToArray()));
			Assert.Equal("pfx_bnd_32:", "pfx_bnd_32:" + string.Join(",", pfx_bnd_32.ToArray()));
			Assert.Equal("pfx_bnd_64:", "pfx_bnd_64:" + string.Join(",", pfx_bnd_64.ToArray()));
			Assert.Equal("pfx_no_xacquire_16:", "pfx_no_xacquire_16:" + string.Join(",", pfx_no_xacquire_16.ToArray()));
			Assert.Equal("pfx_no_xacquire_32:", "pfx_no_xacquire_32:" + string.Join(",", pfx_no_xacquire_32.ToArray()));
			Assert.Equal("pfx_no_xacquire_64:", "pfx_no_xacquire_64:" + string.Join(",", pfx_no_xacquire_64.ToArray()));
			Assert.Equal("pfx_no_xrelease_16:", "pfx_no_xrelease_16:" + string.Join(",", pfx_no_xrelease_16.ToArray()));
			Assert.Equal("pfx_no_xrelease_32:", "pfx_no_xrelease_32:" + string.Join(",", pfx_no_xrelease_32.ToArray()));
			Assert.Equal("pfx_no_xrelease_64:", "pfx_no_xrelease_64:" + string.Join(",", pfx_no_xrelease_64.ToArray()));
			Assert.Equal("pfx_no_lock_16:", "pfx_no_lock_16:" + string.Join(",", pfx_no_lock_16.ToArray()));
			Assert.Equal("pfx_no_lock_32:", "pfx_no_lock_32:" + string.Join(",", pfx_no_lock_32.ToArray()));
			Assert.Equal("pfx_no_lock_64:", "pfx_no_lock_64:" + string.Join(",", pfx_no_lock_64.ToArray()));
			Assert.Equal("pfx_no_hnt_16:", "pfx_no_hnt_16:" + string.Join(",", pfx_no_hnt_16.ToArray()));
			Assert.Equal("pfx_no_hnt_32:", "pfx_no_hnt_32:" + string.Join(",", pfx_no_hnt_32.ToArray()));
			Assert.Equal("pfx_no_hnt_64:", "pfx_no_hnt_64:" + string.Join(",", pfx_no_hnt_64.ToArray()));
			Assert.Equal("pfx_no_ht_16:", "pfx_no_ht_16:" + string.Join(",", pfx_no_ht_16.ToArray()));
			Assert.Equal("pfx_no_ht_32:", "pfx_no_ht_32:" + string.Join(",", pfx_no_ht_32.ToArray()));
			Assert.Equal("pfx_no_ht_64:", "pfx_no_ht_64:" + string.Join(",", pfx_no_ht_64.ToArray()));
			Assert.Equal("pfx_no_rep_16:", "pfx_no_rep_16:" + string.Join(",", pfx_no_rep_16.ToArray()));
			Assert.Equal("pfx_no_rep_32:", "pfx_no_rep_32:" + string.Join(",", pfx_no_rep_32.ToArray()));
			Assert.Equal("pfx_no_rep_64:", "pfx_no_rep_64:" + string.Join(",", pfx_no_rep_64.ToArray()));
			Assert.Equal("pfx_no_repne_16:", "pfx_no_repne_16:" + string.Join(",", pfx_no_repne_16.ToArray()));
			Assert.Equal("pfx_no_repne_32:", "pfx_no_repne_32:" + string.Join(",", pfx_no_repne_32.ToArray()));
			Assert.Equal("pfx_no_repne_64:", "pfx_no_repne_64:" + string.Join(",", pfx_no_repne_64.ToArray()));
			Assert.Equal("pfx_no_notrack_16:", "pfx_no_notrack_16:" + string.Join(",", pfx_no_notrack_16.ToArray()));
			Assert.Equal("pfx_no_notrack_32:", "pfx_no_notrack_32:" + string.Join(",", pfx_no_notrack_32.ToArray()));
			Assert.Equal("pfx_no_notrack_64:", "pfx_no_notrack_64:" + string.Join(",", pfx_no_notrack_64.ToArray()));
			Assert.Equal("pfx_no_bnd_16:", "pfx_no_bnd_16:" + string.Join(",", pfx_no_bnd_16.ToArray()));
			Assert.Equal("pfx_no_bnd_32:", "pfx_no_bnd_32:" + string.Join(",", pfx_no_bnd_32.ToArray()));
			Assert.Equal("pfx_no_bnd_64:", "pfx_no_bnd_64:" + string.Join(",", pfx_no_bnd_64.ToArray()));

			static bool CanUseModRM_rm_reg(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.r8_or_mem:
					case OpCodeOperandKind.r16_or_mem:
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.mm_or_mem:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
					case OpCodeOperandKind.bnd_or_mem_mpx:
					case OpCodeOperandKind.k_or_mem:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.tmm_rm:
						return true;
					}
				}
				return false;
			}

			static bool CanUseModRM_rm_mem(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.sibmem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
					case OpCodeOperandKind.r8_or_mem:
					case OpCodeOperandKind.r16_or_mem:
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.mm_or_mem:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
					case OpCodeOperandKind.bnd_or_mem_mpx:
					case OpCodeOperandKind.k_or_mem:
						return true;
					}
				}
				return false;
			}

			static bool CanUseVEX2(OpCodeInfo opCode) => opCode.Table == OpCodeTableKind.T0F && opCode.W == 0;

			static bool CanUseB(int bitness, OpCodeInfo opCode) {
				switch (opCode.Code) {
				case Code.Nopw:
				case Code.Nopd:
				case Code.Nopq:
				case Code.Bndmov_bnd_bndm128:
				case Code.Bndmov_bndm128_bnd:
					return false;
				}

				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.sibmem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib64z:
						// The memory test tests all combinations
						return false;
					}
				}

				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.tmm_rm:
						return false;

					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:

					case OpCodeOperandKind.bnd_or_mem_mpx:
					case OpCodeOperandKind.k_or_mem:
					case OpCodeOperandKind.mm_or_mem:
					case OpCodeOperandKind.r16_or_mem:
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.r8_or_mem:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
						if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW)
							return bitness == 64;
						return true;
					}
				}
				if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW)
					return bitness == 64;
				return true;
			}

			static bool CanUseX(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.tmm_rm:

					case OpCodeOperandKind.bnd_or_mem_mpx:
					case OpCodeOperandKind.k_or_mem:
					case OpCodeOperandKind.mm_or_mem:
					case OpCodeOperandKind.r16_or_mem:
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.r8_or_mem:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
						return true;

					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.sibmem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib64z:
						// The memory test tests all combinations
						return false;
					}
				}
				return true;
			}

			static bool CanUseR(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.kp1_reg:
					case OpCodeOperandKind.tr_reg:
					case OpCodeOperandKind.bnd_reg:
					case OpCodeOperandKind.tmm_reg:
						return false;

					case OpCodeOperandKind.cr_reg:
					case OpCodeOperandKind.dr_reg:
					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.seg_reg:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
						return true;
					}
				}
				return true;
			}

			static bool CanUseR2(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.kp1_reg:
					case OpCodeOperandKind.tr_reg:
					case OpCodeOperandKind.bnd_reg:
					case OpCodeOperandKind.cr_reg:
					case OpCodeOperandKind.dr_reg:
					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.seg_reg:
					case OpCodeOperandKind.tmm_reg:
						return false;

					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.zmm_reg:
						return true;
					}
				}
				return true;
			}

			static bool CanUseV2(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.k_vvvv:
					case OpCodeOperandKind.r32_vvvv:
					case OpCodeOperandKind.r64_vvvv:
					case OpCodeOperandKind.tmm_vvvv:
						return false;

					case OpCodeOperandKind.xmm_vvvv:
					case OpCodeOperandKind.xmmp3_vvvv:
					case OpCodeOperandKind.ymm_vvvv:
					case OpCodeOperandKind.zmm_vvvv:
					case OpCodeOperandKind.zmmp3_vvvv:
						return true;

					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib64z:
						// The memory test tests all combinations
						return false;
					}
				}
				return false;
			}

			static bool HasModRM(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.sibmem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
					case OpCodeOperandKind.r8_or_mem:
					case OpCodeOperandKind.r16_or_mem:
					case OpCodeOperandKind.r32_or_mem:
					case OpCodeOperandKind.r32_or_mem_mpx:
					case OpCodeOperandKind.r64_or_mem:
					case OpCodeOperandKind.r64_or_mem_mpx:
					case OpCodeOperandKind.mm_or_mem:
					case OpCodeOperandKind.xmm_or_mem:
					case OpCodeOperandKind.ymm_or_mem:
					case OpCodeOperandKind.zmm_or_mem:
					case OpCodeOperandKind.bnd_or_mem_mpx:
					case OpCodeOperandKind.k_or_mem:
					case OpCodeOperandKind.r8_reg:
					case OpCodeOperandKind.r16_reg:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_reg:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_reg:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.seg_reg:
					case OpCodeOperandKind.k_reg:
					case OpCodeOperandKind.kp1_reg:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_reg:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.xmm_reg:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_reg:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_reg:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.tmm_reg:
					case OpCodeOperandKind.tmm_rm:
					case OpCodeOperandKind.cr_reg:
					case OpCodeOperandKind.dr_reg:
					case OpCodeOperandKind.tr_reg:
					case OpCodeOperandKind.bnd_reg:
						return true;
					}
				}
				return false;
			}

			static List<Code> GetList2(int bitness, List<Code> l16, List<Code> l32) =>
				bitness switch {
					16 => l16,
					32 => l32,
					_ => throw new InvalidOperationException(),
				};

			static List<Code> GetList(int bitness, List<Code> l16, List<Code> l32, List<Code> l64) =>
				bitness switch {
					16 => l16,
					32 => l32,
					64 => l64,
					_ => throw new InvalidOperationException(),
				};
		}

		[Fact]
		void Test_invalid_zero_opmask_reg() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if ((info.Options & DecoderOptions.NoInvalidCheck) != 0)
					continue;
				var opCode = info.Code.ToOpCode();
				if (!opCode.RequireOpMaskRegister)
					continue;

				var bytes = HexUtils.ToByteArray(info.HexBytes);
				Instruction origInstr;
				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
					decoder.Decode(out origInstr);
					Assert.Equal(info.Code, origInstr.Code);
				}

				int evexIndex = GetEvexIndex(bytes);
				bytes[evexIndex + 3] &= 0xF8;
				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
					decoder.Decode(out var instruction);
					Assert.Equal(Code.INVALID, instruction.Code);
					Assert.NotEqual(DecoderError.None, decoder.LastError);
				}
				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options | DecoderOptions.NoInvalidCheck);
					decoder.Decode(out var instruction);
					Assert.Equal(info.Code, instruction.Code);
					Assert.Equal(Register.None, instruction.OpMask);
					origInstr.OpMask = Register.None;
					Assert.True(Instruction.EqualsAllBits(origInstr, instruction));
				}
			}
		}

		[Fact]
		void Verify_cpu_mode() {
			var hash1632 = new HashSet<Code>(DecoderTestUtils.Code32Only);
			foreach (var code in DecoderTestUtils.NotDecoded32Only)
				hash1632.Add(code);
			var hash64 = new HashSet<Code>(DecoderTestUtils.Code64Only);
			foreach (var code in DecoderTestUtils.NotDecoded64Only)
				hash64.Add(code);
			var codeNames = ToEnumConverter.GetCodeNames();
			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				if (CodeUtils.IsIgnored(codeNames[i]))
					continue;
				var code = (Code)i;
				var opCode = code.ToOpCode();
				if (hash1632.Contains(code)) {
					Assert.True(opCode.Mode16);
					Assert.True(opCode.Mode32);
					Assert.False(opCode.Mode64);
				}
				else if (hash64.Contains(code)) {
					Assert.False(opCode.Mode16);
					Assert.False(opCode.Mode32);
					Assert.True(opCode.Mode64);
				}
				else {
					Assert.True(opCode.Mode16);
					Assert.True(opCode.Mode32);
					Assert.True(opCode.Mode64);
				}
			}
		}

		[Fact]
		void Verify_can_only_decode_in_correct_mode() {
			var extraBytes = new string('0', (IcedConstants.MaxInstructionLength - 1) * 2);
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				var newHexBytes = info.HexBytes + extraBytes;
				if (!opCode.Mode16) {
					var decoder = Decoder.Create(16, new ByteArrayCodeReader(newHexBytes), info.Options);
					decoder.Decode(out var instruction);
					Assert.NotEqual(info.Code, instruction.Code);
				}
				if (!opCode.Mode32) {
					var decoder = Decoder.Create(32, new ByteArrayCodeReader(newHexBytes), info.Options);
					decoder.Decode(out var instruction);
					Assert.NotEqual(info.Code, instruction.Code);
				}
				if (!opCode.Mode64) {
					var decoder = Decoder.Create(64, new ByteArrayCodeReader(newHexBytes), info.Options);
					decoder.Decode(out var instruction);
					Assert.NotEqual(info.Code, instruction.Code);
				}
			}
		}

		[Fact]
		void Verify_invalid_table_encoding() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX) {
					var hexBytes = HexUtils.ToByteArray(info.HexBytes);
					var evexIndex = GetEvexIndex(hexBytes);
					var maxTable = opCode.Encoding == EncodingKind.EVEX ? 8 : 0x10;
					for (int i = 0; i < 8; i++) {
						switch (opCode.Encoding) {
						case EncodingKind.EVEX:
							switch (i) {
							case 1:// 0F
							case 2:// 0F 38
							case 3:// 0F 3A
							case 5:// MAP5
							case 6:// MAP6
								continue;
							}
							break;
						case EncodingKind.MVEX:
							switch (i) {
							case 1:// 0F
							case 2:// 0F 38
							case 3:// 0F 3A
								continue;
							}
							break;
						default:
							throw new InvalidOperationException();
						}
						hexBytes[evexIndex + 1] = (byte)((hexBytes[evexIndex + 1] & ~(byte)(maxTable - 1)) | i);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options ^ DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instruction);
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
					}
				}
				else if (opCode.Encoding == EncodingKind.VEX) {
					var hexBytes = HexUtils.ToByteArray(info.HexBytes);
					var vexIndex = GetVexXopIndex(hexBytes);
					if (hexBytes[vexIndex] == 0xC5)
						continue;
					for (int i = 0; i < 32; i++) {
						switch (i) {
#if MVEX
						case 0:// MAP0
							continue;
#endif
						case 1:// 0F
						case 2:// 0F 38
						case 3:// 0F 3A
							continue;
						}
						hexBytes[vexIndex + 1] = (byte)((hexBytes[vexIndex + 1] & 0xE0) | i);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options ^ DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instruction);
							Assert.Equal(Code.INVALID, instruction.Code);
							Assert.NotEqual(DecoderError.None, decoder.LastError);
						}
					}
				}
				else if (opCode.Encoding == EncodingKind.XOP) {
					var hexBytes = HexUtils.ToByteArray(info.HexBytes);
					var vexIndex = GetVexXopIndex(hexBytes);
					for (int i = 0; i < 32; i++) {
						switch (i) {
						case 8:// MAP8
						case 9:// MAP9
						case 10:// MAP10
							continue;
						}
						hexBytes[vexIndex + 1] = (byte)((hexBytes[vexIndex + 1] & 0xE0) | i);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options);
							decoder.Decode(out var instruction);
							if (i < 8)
								Assert.NotEqual(info.Code, instruction.Code);
							else {
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options ^ DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instruction);
							if (i < 8)
								Assert.NotEqual(info.Code, instruction.Code);
							else {
								Assert.Equal(Code.INVALID, instruction.Code);
								Assert.NotEqual(DecoderError.None, decoder.LastError);
							}
						}
					}
				}
				else if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW) {
				}
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Verify_invalid_pp_field() {
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX) {
					var hexBytes = HexUtils.ToByteArray(info.HexBytes);
					var evexIndex = GetEvexIndex(hexBytes);
					var b = hexBytes[evexIndex + 2];
					for (int i = 1; i <= 3; i++) {
						hexBytes[evexIndex + 2] = (byte)(b ^ i);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.NotEqual(info.Code, instruction.Code);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options ^ DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instruction);
							Assert.NotEqual(info.Code, instruction.Code);
						}
					}
				}
				else if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					var hexBytes = HexUtils.ToByteArray(info.HexBytes);
					var vexIndex = GetVexXopIndex(hexBytes);
					int ppIndex = hexBytes[vexIndex] == 0xC5 ? vexIndex + 1 : vexIndex + 2;
					var b = hexBytes[ppIndex];
					for (int i = 1; i <= 3; i++) {
						hexBytes[ppIndex] = (byte)(b ^ i);
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options);
							decoder.Decode(out var instruction);
							Assert.NotEqual(info.Code, instruction.Code);
						}
						{
							var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(hexBytes), info.Options ^ DecoderOptions.NoInvalidCheck);
							decoder.Decode(out var instruction);
							Assert.NotEqual(info.Code, instruction.Code);
						}
					}
				}
				else if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW) {
				}
				else
					throw new InvalidOperationException();
			}
		}

		[Fact]
		void Verify_regonly_or_regmemonly_mod_bits() {
			var extraBytes = new string('0', (IcedConstants.MaxInstructionLength - 1) * 2);
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				var opCode = info.Code.ToOpCode();
				if (!IsRegOnlyOrRegMemOnlyModRM(opCode))
					continue;
				// There are a few instructions that ignore the mod bits...
				if (opCode.IgnoresModBits)
					continue;

				var bytes = HexUtils.ToByteArray(info.HexBytes + extraBytes);
				int mIndex;
				if (opCode.Encoding == EncodingKind.EVEX || opCode.Encoding == EncodingKind.MVEX)
					mIndex = GetEvexIndex(bytes) + 5;
				else if (opCode.Encoding == EncodingKind.VEX || opCode.Encoding == EncodingKind.XOP) {
					int vexIndex = GetVexXopIndex(bytes);
					mIndex = bytes[vexIndex] == 0xC5 ? vexIndex + 3 : vexIndex + 4;
				}
				else if (opCode.Encoding == EncodingKind.Legacy || opCode.Encoding == EncodingKind.D3NOW) {
					mIndex = SkipPrefixes(bytes, info.Bitness, out var rex);
					switch (opCode.Table) {
					case OpCodeTableKind.Normal:
						break;
					case OpCodeTableKind.T0F:
						if (bytes[mIndex++] != 0x0F)
							throw new InvalidOperationException();
						break;
					case OpCodeTableKind.T0F38:
						if (bytes[mIndex++] != 0x0F)
							throw new InvalidOperationException();
						if (bytes[mIndex++] != 0x38)
							throw new InvalidOperationException();
						break;
					case OpCodeTableKind.T0F3A:
						if (bytes[mIndex++] != 0x0F)
							throw new InvalidOperationException();
						if (bytes[mIndex++] != 0x3A)
							throw new InvalidOperationException();
						break;
					default:
						throw new InvalidOperationException();
					}
					mIndex++;
				}
				else
					throw new InvalidOperationException();

				if (bytes[mIndex] >= 0xC0)
					bytes[mIndex] &= 0x3F;
				else
					bytes[mIndex] |= 0xC0;
				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options);
					decoder.Decode(out var instruction);
					Assert.NotEqual(info.Code, instruction.Code);
				}
				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(bytes), info.Options ^ DecoderOptions.NoInvalidCheck);
					decoder.Decode(out var instruction);
					Assert.NotEqual(info.Code, instruction.Code);
				}
			}

			static bool IsRegOnlyOrRegMemOnlyModRM(OpCodeInfo opCode) {
				for (int i = 0; i < opCode.OpCount; i++) {
					switch (opCode.GetOpKind(i)) {
					case OpCodeOperandKind.mem:
					case OpCodeOperandKind.sibmem:
					case OpCodeOperandKind.mem_mpx:
					case OpCodeOperandKind.mem_mib:
					case OpCodeOperandKind.mem_vsib32x:
					case OpCodeOperandKind.mem_vsib64x:
					case OpCodeOperandKind.mem_vsib32y:
					case OpCodeOperandKind.mem_vsib64y:
					case OpCodeOperandKind.mem_vsib32z:
					case OpCodeOperandKind.mem_vsib64z:
					case OpCodeOperandKind.r16_rm:
					case OpCodeOperandKind.r32_rm:
					case OpCodeOperandKind.r64_rm:
					case OpCodeOperandKind.k_rm:
					case OpCodeOperandKind.mm_rm:
					case OpCodeOperandKind.xmm_rm:
					case OpCodeOperandKind.ymm_rm:
					case OpCodeOperandKind.zmm_rm:
					case OpCodeOperandKind.tmm_rm:
						return true;
					}
				}
				return false;
			}
		}

		[Fact]
		void Disable_decoder_option_disables_instruction() {
			var extraBytes = new string('0', (IcedConstants.MaxInstructionLength - 1) * 2);
			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				if (info.Options == DecoderOptions.None)
					continue;
				const DecoderOptions NoOptions =
					DecoderOptions.NoInvalidCheck |
					DecoderOptions.NoPause |
					DecoderOptions.NoWbnoinvd |
					DecoderOptions.NoMPFX_0FBC |
					DecoderOptions.NoMPFX_0FBD |
					DecoderOptions.NoLahfSahf64;
				if ((info.Options & NoOptions) != 0)
					continue;
				if (!IsPowerOfTwo((uint)info.Options))
					continue;
				if (info.Options == DecoderOptions.ForceReservedNop)
					continue;
				if ((info.TestOptions & DecoderTestOptions.NoOptDisableTest) != 0)
					continue;

				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
					decoder.Decode(out var instr);
					Assert.Equal(info.Code, instr.Code);
				}
				{
					var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes + extraBytes), DecoderOptions.None);
					decoder.Decode(out var instr);
					Assert.NotEqual(info.Code, instr.Code);
				}
			}

			static bool IsPowerOfTwo(uint v) =>
				v != 0 && (v & (v - 1)) == 0;
		}
	}
}
#endif
