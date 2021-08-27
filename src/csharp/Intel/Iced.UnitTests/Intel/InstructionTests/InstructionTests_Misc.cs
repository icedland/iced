// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class InstructionTests_Misc {
		[Fact]
		void INVALID_Code_value_is_zero() {
			// A 'default' Instruction should be an invalid instruction
			Static.Assert((int)Code.INVALID == 0 ? 0 : -1);
			Instruction instruction1 = default;
			Assert.Equal(Code.INVALID, instruction1.Code);
			var instruction2 = new Instruction();
			Assert.Equal(Code.INVALID, instruction2.Code);
			Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
		}

#if ENCODER
#if !NO_VEX
		[Fact]
		void Equals_and_GetHashCode_ignore_some_fields() {
			var instruction1 = Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RCX, Register.R14, 8, 0x12345678, 8, false, Register.FS), Register.XMM10, 0xA5);
			var instruction2 = instruction1;
			Assert.True(Instruction.EqualsAllBits(instruction1, instruction2));
			instruction1.CodeSize = CodeSize.Code32;
			instruction2.CodeSize = CodeSize.Code64;
			Assert.False(Instruction.EqualsAllBits(instruction1, instruction2));
			instruction1.Length = 10;
			instruction2.Length = 5;
			instruction1.IP = 0x97333795FA7CEAAB;
			instruction2.IP = 0x9BE5A3A07A66FC05;
			Assert.True(instruction1 == instruction2);
			Assert.True(instruction1.Equals(instruction2));
			Assert.True(instruction1.Equals(ToObject(instruction2)));
			Assert.Equal(instruction1, instruction2);
			Assert.Equal(instruction1.GetHashCode(), instruction2.GetHashCode());
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		static object ToObject<T>(T value) => value;
#endif
#endif

		[Fact]
		void Write_all_properties() {
			Instruction instruction = default;

			instruction.IP = 0x8A6BD04A9B683A92;
			instruction.IP16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instruction.IP16);
			Assert.Equal(ushort.MinValue, instruction.IP32);
			Assert.Equal(ushort.MinValue, instruction.IP);
			instruction.IP = 0x8A6BD04A9B683A92;
			instruction.IP16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.IP16);
			Assert.Equal(ushort.MaxValue, instruction.IP32);
			Assert.Equal(ushort.MaxValue, instruction.IP);

			instruction.IP = 0x8A6BD04A9B683A92;
			instruction.IP32 = uint.MinValue;
			Assert.Equal(ushort.MinValue, instruction.IP16);
			Assert.Equal(uint.MinValue, instruction.IP32);
			Assert.Equal(uint.MinValue, instruction.IP);
			instruction.IP = 0x8A6BD04A9B683A92;
			instruction.IP32 = uint.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.IP16);
			Assert.Equal(uint.MaxValue, instruction.IP32);
			Assert.Equal(uint.MaxValue, instruction.IP);

			instruction.IP = ulong.MinValue;
			Assert.Equal(ushort.MinValue, instruction.IP16);
			Assert.Equal(uint.MinValue, instruction.IP32);
			Assert.Equal(ulong.MinValue, instruction.IP);
			instruction.IP = ulong.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.IP16);
			Assert.Equal(uint.MaxValue, instruction.IP32);
			Assert.Equal(ulong.MaxValue, instruction.IP);

			instruction.NextIP = 0x8A6BD04A9B683A92;
			instruction.NextIP16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instruction.NextIP16);
			Assert.Equal(ushort.MinValue, instruction.NextIP32);
			Assert.Equal(ushort.MinValue, instruction.NextIP);
			instruction.NextIP = 0x8A6BD04A9B683A92;
			instruction.NextIP16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.NextIP16);
			Assert.Equal(ushort.MaxValue, instruction.NextIP32);
			Assert.Equal(ushort.MaxValue, instruction.NextIP);

			instruction.NextIP = 0x8A6BD04A9B683A92;
			instruction.NextIP32 = uint.MinValue;
			Assert.Equal(ushort.MinValue, instruction.NextIP16);
			Assert.Equal(uint.MinValue, instruction.NextIP32);
			Assert.Equal(uint.MinValue, instruction.NextIP);
			instruction.NextIP = 0x8A6BD04A9B683A92;
			instruction.NextIP32 = uint.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.NextIP16);
			Assert.Equal(uint.MaxValue, instruction.NextIP32);
			Assert.Equal(uint.MaxValue, instruction.NextIP);

			instruction.NextIP = ulong.MinValue;
			Assert.Equal(ushort.MinValue, instruction.NextIP16);
			Assert.Equal(uint.MinValue, instruction.NextIP32);
			Assert.Equal(ulong.MinValue, instruction.NextIP);
			instruction.NextIP = ulong.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.NextIP16);
			Assert.Equal(uint.MaxValue, instruction.NextIP32);
			Assert.Equal(ulong.MaxValue, instruction.NextIP);

			instruction.MemoryDisplacement32 = uint.MinValue;
			Assert.Equal(uint.MinValue, instruction.MemoryDisplacement32);
			Assert.Equal(uint.MinValue, instruction.MemoryDisplacement64);
			instruction.MemoryDisplacement32 = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instruction.MemoryDisplacement32);
			Assert.Equal(uint.MaxValue, instruction.MemoryDisplacement64);

			instruction.MemoryDisplacement64 = ulong.MinValue;
			Assert.Equal(uint.MinValue, instruction.MemoryDisplacement32);
			Assert.Equal(ulong.MinValue, instruction.MemoryDisplacement64);
			instruction.MemoryDisplacement64 = ulong.MaxValue;
			Assert.Equal(uint.MaxValue, instruction.MemoryDisplacement32);
			Assert.Equal(ulong.MaxValue, instruction.MemoryDisplacement64);

			instruction.MemoryDisplacement64 = 0x1234_5678_9ABC_DEF1;
			instruction.MemoryDisplacement32 = 0x5AA5_4321;
			Assert.Equal(0x5AA5_4321U, instruction.MemoryDisplacement32);
			Assert.Equal(0x5AA5_4321U, instruction.MemoryDisplacement64);

			instruction.Immediate8 = byte.MinValue;
			Assert.Equal(byte.MinValue, instruction.Immediate8);
			instruction.Immediate8 = byte.MaxValue;
			Assert.Equal(byte.MaxValue, instruction.Immediate8);

			instruction.Immediate8_2nd = byte.MinValue;
			Assert.Equal(byte.MinValue, instruction.Immediate8_2nd);
			instruction.Immediate8_2nd = byte.MaxValue;
			Assert.Equal(byte.MaxValue, instruction.Immediate8_2nd);

			instruction.Immediate16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instruction.Immediate16);
			instruction.Immediate16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.Immediate16);

			instruction.Immediate32 = uint.MinValue;
			Assert.Equal(uint.MinValue, instruction.Immediate32);
			instruction.Immediate32 = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instruction.Immediate32);

			instruction.Immediate64 = ulong.MinValue;
			Assert.Equal(ulong.MinValue, instruction.Immediate64);
			instruction.Immediate64 = ulong.MaxValue;
			Assert.Equal(ulong.MaxValue, instruction.Immediate64);

			instruction.Immediate8to16 = sbyte.MinValue;
			Assert.Equal(sbyte.MinValue, instruction.Immediate8to16);
			instruction.Immediate8to16 = sbyte.MaxValue;
			Assert.Equal(sbyte.MaxValue, instruction.Immediate8to16);

			instruction.Immediate8to32 = sbyte.MinValue;
			Assert.Equal(sbyte.MinValue, instruction.Immediate8to32);
			instruction.Immediate8to32 = sbyte.MaxValue;
			Assert.Equal(sbyte.MaxValue, instruction.Immediate8to32);

			instruction.Immediate8to64 = sbyte.MinValue;
			Assert.Equal(sbyte.MinValue, instruction.Immediate8to64);
			instruction.Immediate8to64 = sbyte.MaxValue;
			Assert.Equal(sbyte.MaxValue, instruction.Immediate8to64);

			instruction.Immediate32to64 = int.MinValue;
			Assert.Equal(int.MinValue, instruction.Immediate32to64);
			instruction.Immediate32to64 = int.MaxValue;
			Assert.Equal(int.MaxValue, instruction.Immediate32to64);

			instruction.Op0Kind = OpKind.NearBranch16;
			instruction.NearBranch16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instruction.NearBranch16);
			Assert.Equal(ushort.MinValue, instruction.NearBranchTarget);
			instruction.NearBranch16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.NearBranch16);
			Assert.Equal(ushort.MaxValue, instruction.NearBranchTarget);

			instruction.Op0Kind = OpKind.NearBranch32;
			instruction.NearBranch32 = uint.MinValue;
			Assert.Equal(uint.MinValue, instruction.NearBranch32);
			Assert.Equal(uint.MinValue, instruction.NearBranchTarget);
			instruction.NearBranch32 = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instruction.NearBranch32);
			Assert.Equal(uint.MaxValue, instruction.NearBranchTarget);

			instruction.Op0Kind = OpKind.NearBranch64;
			instruction.NearBranch64 = ulong.MinValue;
			Assert.Equal(ulong.MinValue, instruction.NearBranch64);
			Assert.Equal(ulong.MinValue, instruction.NearBranchTarget);
			instruction.NearBranch64 = ulong.MaxValue;
			Assert.Equal(ulong.MaxValue, instruction.NearBranch64);
			Assert.Equal(ulong.MaxValue, instruction.NearBranchTarget);

			instruction.FarBranch16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instruction.FarBranch16);
			instruction.FarBranch16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.FarBranch16);

			instruction.FarBranch32 = uint.MinValue;
			Assert.Equal(uint.MinValue, instruction.FarBranch32);
			instruction.FarBranch32 = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instruction.FarBranch32);

			instruction.FarBranchSelector = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instruction.FarBranchSelector);
			instruction.FarBranchSelector = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instruction.FarBranchSelector);

			{
				var instr = instruction;
				instr.Code = Code.Cmpxchg8b_m64;
				instr.Op0Kind = OpKind.Memory;
				instr.HasLockPrefix = true;

				instr.HasXacquirePrefix = false;
				Assert.False(instr.HasXacquirePrefix);
				instr.HasXacquirePrefix = true;
				Assert.True(instr.HasXacquirePrefix);

				instr.HasXreleasePrefix = false;
				Assert.False(instr.HasXreleasePrefix);
				instr.HasXreleasePrefix = true;
				Assert.True(instr.HasXreleasePrefix);
			}

			instruction.HasRepPrefix = false;
			Assert.False(instruction.HasRepPrefix);
			Assert.False(instruction.HasRepePrefix);
			instruction.HasRepPrefix = true;
			Assert.True(instruction.HasRepPrefix);
			Assert.True(instruction.HasRepePrefix);

			instruction.HasRepePrefix = false;
			Assert.False(instruction.HasRepPrefix);
			Assert.False(instruction.HasRepePrefix);
			instruction.HasRepePrefix = true;
			Assert.True(instruction.HasRepPrefix);
			Assert.True(instruction.HasRepePrefix);

			instruction.HasRepnePrefix = false;
			Assert.False(instruction.HasRepnePrefix);
			instruction.HasRepnePrefix = true;
			Assert.True(instruction.HasRepnePrefix);

			instruction.HasLockPrefix = false;
			Assert.False(instruction.HasLockPrefix);
			instruction.HasLockPrefix = true;
			Assert.True(instruction.HasLockPrefix);

			instruction.IsBroadcast = false;
			Assert.False(instruction.IsBroadcast);
			instruction.IsBroadcast = true;
			Assert.True(instruction.IsBroadcast);

			instruction.SuppressAllExceptions = false;
			Assert.False(instruction.SuppressAllExceptions);
			instruction.SuppressAllExceptions = true;
			Assert.True(instruction.SuppressAllExceptions);

			for (int i = 0; i <= IcedConstants.MaxInstructionLength; i++) {
				instruction.Length = i;
				Assert.Equal(i, instruction.Length);
			}

			foreach (var codeSize in GetCodeSizeValues()) {
				instruction.CodeSize = codeSize;
				Assert.Equal(codeSize, instruction.CodeSize);
			}

			foreach (var code in GetCodeValues()) {
				instruction.Code = code;
				Assert.Equal(code, instruction.Code);
			}
			foreach (var code in GetCodeValues()) {
				instruction.InternalSetCodeNoCheck(code);
				Assert.Equal(code, instruction.Code);
			}
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.Code = (Code)(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => instruction.Code = (Code)IcedConstants.CodeEnumCount);

			Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
			foreach (var opKind in GetOpKindValues()) {
				instruction.Op0Kind = opKind;
				Assert.Equal(opKind, instruction.Op0Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instruction.Op1Kind = opKind;
				Assert.Equal(opKind, instruction.Op1Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instruction.Op2Kind = opKind;
				Assert.Equal(opKind, instruction.Op2Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instruction.Op3Kind = opKind;
				Assert.Equal(opKind, instruction.Op3Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				if (opKind == OpKind.Immediate8) {
					instruction.Op4Kind = opKind;
					Assert.Equal(opKind, instruction.Op4Kind);
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instruction.Op4Kind = opKind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instruction.SetOpKind(0, opKind);
				Assert.Equal(opKind, instruction.Op0Kind);
				Assert.Equal(opKind, instruction.GetOpKind(0));
			}

			foreach (var opKind in GetOpKindValues()) {
				instruction.SetOpKind(1, opKind);
				Assert.Equal(opKind, instruction.Op1Kind);
				Assert.Equal(opKind, instruction.GetOpKind(1));
			}

			foreach (var opKind in GetOpKindValues()) {
				instruction.SetOpKind(2, opKind);
				Assert.Equal(opKind, instruction.Op2Kind);
				Assert.Equal(opKind, instruction.GetOpKind(2));
			}

			foreach (var opKind in GetOpKindValues()) {
				instruction.SetOpKind(3, opKind);
				Assert.Equal(opKind, instruction.Op3Kind);
				Assert.Equal(opKind, instruction.GetOpKind(3));
			}

			foreach (var opKind in GetOpKindValues()) {
				if (opKind == OpKind.Immediate8) {
					instruction.SetOpKind(4, opKind);
					Assert.Equal(opKind, instruction.Op4Kind);
					Assert.Equal(opKind, instruction.GetOpKind(4));
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetOpKind(4, opKind));
			}

			var segValues = new Register[] {
				Register.ES,
				Register.CS,
				Register.SS,
				Register.DS,
				Register.FS,
				Register.GS,
				Register.None,
			};
			foreach (var seg in segValues) {
				instruction.SegmentPrefix = seg;
				Assert.Equal(seg, instruction.SegmentPrefix);
				if (instruction.SegmentPrefix == Register.None)
					Assert.False(instruction.HasSegmentPrefix);
				else
					Assert.True(instruction.HasSegmentPrefix);
			}

			var displSizes = new int[] { 8, 4, 2, 1, 0 };
			foreach (var displSize in displSizes) {
				instruction.MemoryDisplSize = displSize;
				Assert.Equal(displSize, instruction.MemoryDisplSize);
			}

			var scaleValues = new int[] { 8, 4, 2, 1 };
			foreach (var scaleValue in scaleValues) {
				instruction.MemoryIndexScale = scaleValue;
				Assert.Equal(scaleValue, instruction.MemoryIndexScale);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.MemoryBase = reg;
				Assert.Equal(reg, instruction.MemoryBase);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.MemoryIndex = reg;
				Assert.Equal(reg, instruction.MemoryIndex);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.Op0Register = reg;
				Assert.Equal(reg, instruction.Op0Register);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.Op1Register = reg;
				Assert.Equal(reg, instruction.Op1Register);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.Op2Register = reg;
				Assert.Equal(reg, instruction.Op2Register);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.Op3Register = reg;
				Assert.Equal(reg, instruction.Op3Register);
			}

			foreach (var reg in GetRegisterValues()) {
				if (reg == Register.None) {
					instruction.Op4Register = reg;
					Assert.Equal(reg, instruction.Op4Register);
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instruction.Op4Register = reg);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.SetOpRegister(0, reg);
				Assert.Equal(reg, instruction.Op0Register);
				Assert.Equal(reg, instruction.GetOpRegister(0));
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.SetOpRegister(1, reg);
				Assert.Equal(reg, instruction.Op1Register);
				Assert.Equal(reg, instruction.GetOpRegister(1));
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.SetOpRegister(2, reg);
				Assert.Equal(reg, instruction.Op2Register);
				Assert.Equal(reg, instruction.GetOpRegister(2));
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.SetOpRegister(3, reg);
				Assert.Equal(reg, instruction.Op3Register);
				Assert.Equal(reg, instruction.GetOpRegister(3));
			}

			foreach (var reg in GetRegisterValues()) {
				if (reg == Register.None) {
					instruction.SetOpRegister(4, reg);
					Assert.Equal(reg, instruction.Op4Register);
					Assert.Equal(reg, instruction.GetOpRegister(4));
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instruction.SetOpRegister(4, reg));
			}

			var opMasks = new Register[] {
				Register.K1,
				Register.K2,
				Register.K3,
				Register.K4,
				Register.K5,
				Register.K6,
				Register.K7,
				Register.None,
			};
			foreach (var opMask in opMasks) {
				instruction.OpMask = opMask;
				Assert.Equal(opMask, instruction.OpMask);
				Assert.Equal(opMask != Register.None, instruction.HasOpMask);
			}

			instruction.ZeroingMasking = false;
			Assert.False(instruction.ZeroingMasking);
			Assert.True(instruction.MergingMasking);
			instruction.ZeroingMasking = true;
			Assert.True(instruction.ZeroingMasking);
			Assert.False(instruction.MergingMasking);
			instruction.MergingMasking = false;
			Assert.False(instruction.MergingMasking);
			Assert.True(instruction.ZeroingMasking);
			instruction.MergingMasking = true;
			Assert.True(instruction.MergingMasking);
			Assert.False(instruction.ZeroingMasking);

			foreach (var rc in GetRoundingControlValues()) {
				instruction.RoundingControl = rc;
				Assert.Equal(rc, instruction.RoundingControl);
			}

			foreach (var reg in GetRegisterValues()) {
				instruction.MemoryBase = reg;
				Assert.Equal(reg == Register.RIP || reg == Register.EIP, instruction.IsIPRelativeMemoryOperand);
			}

			instruction.MemoryBase = Register.EIP;
			instruction.NextIP = 0x123456709EDCBA98;
			instruction.MemoryDisplacement64 = 0x876543219ABCDEF5;
			Assert.True(instruction.IsIPRelativeMemoryOperand);
			Assert.Equal(0x9ABCDEF5UL, instruction.IPRelativeMemoryAddress);

			instruction.MemoryBase = Register.RIP;
			instruction.NextIP = 0x123456709EDCBA98;
			instruction.MemoryDisplacement64 = 0x876543219ABCDEF5;
			Assert.True(instruction.IsIPRelativeMemoryOperand);
			Assert.Equal(0x876543219ABCDEF5UL, instruction.IPRelativeMemoryAddress);

			instruction.DeclareDataCount = 1;
			Assert.Equal(1, instruction.DeclareDataCount);
			instruction.DeclareDataCount = 15;
			Assert.Equal(15, instruction.DeclareDataCount);
			instruction.DeclareDataCount = 16;
			Assert.Equal(16, instruction.DeclareDataCount);
		}

		static IEnumerable<CodeSize> GetCodeSizeValues() {
			for (int i = 0; i < IcedConstants.CodeSizeEnumCount; i++) {
				yield return (CodeSize)i;
			}
		}
		static IEnumerable<Code> GetCodeValues() {
			for (int i = 0; i < IcedConstants.CodeEnumCount; i++) {
				yield return (Code)i;
			}
		}
		static IEnumerable<OpKind> GetOpKindValues() {
			for (int i = 0; i < IcedConstants.OpKindEnumCount; i++) {
				yield return (OpKind)i;
			}
		}
		static IEnumerable<Register> GetRegisterValues() {
			for (int i = 0; i < IcedConstants.RegisterEnumCount; i++) {
				yield return (Register)i;
			}
		}
		static IEnumerable<RoundingControl> GetRoundingControlValues() {
			for (int i = 0; i < IcedConstants.RoundingControlEnumCount; i++) {
				yield return (RoundingControl)i;
			}
		}

		[Fact]
		void Verify_GetSetImmediate() {
			Instruction instruction = default;

			instruction.Code = Code.Add_AL_imm8;
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA5);
			Assert.Equal(0xA5UL, instruction.GetImmediate(1));

			instruction.Code = Code.Add_AX_imm16;
			instruction.Op1Kind = OpKind.Immediate16;
			instruction.SetImmediate(1, 0x5AA5);
			Assert.Equal(0x5AA5UL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA55A);
			Assert.Equal(0xA55AUL, instruction.GetImmediate(1));

			instruction.Code = Code.Add_EAX_imm32;
			instruction.Op1Kind = OpKind.Immediate32;
			instruction.SetImmediate(1, 0x5AA51234);
			Assert.Equal(0x5AA51234UL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA54A1234);
			Assert.Equal(0xA54A1234UL, instruction.GetImmediate(1));

			instruction.Code = Code.Add_RAX_imm32;
			instruction.Op1Kind = OpKind.Immediate32to64;
			instruction.SetImmediate(1, 0x5AA51234);
			Assert.Equal(0x5AA51234UL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA54A1234);
			Assert.Equal(0xFFFFFFFFA54A1234UL, instruction.GetImmediate(1));

			instruction.Code = Code.Enterq_imm16_imm8;
			instruction.Op1Kind = OpKind.Immediate8_2nd;
			instruction.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA5);
			Assert.Equal(0xA5UL, instruction.GetImmediate(1));

			instruction.Code = Code.Adc_rm16_imm8;
			instruction.Op1Kind = OpKind.Immediate8to16;
			instruction.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA5);
			Assert.Equal(0xFFFFFFFFFFFFFFA5UL, instruction.GetImmediate(1));

			instruction.Code = Code.Adc_rm32_imm8;
			instruction.Op1Kind = OpKind.Immediate8to32;
			instruction.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA5);
			Assert.Equal(0xFFFFFFFFFFFFFFA5UL, instruction.GetImmediate(1));

			instruction.Code = Code.Adc_rm64_imm8;
			instruction.Op1Kind = OpKind.Immediate8to64;
			instruction.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA5);
			Assert.Equal(0xFFFFFFFFFFFFFFA5UL, instruction.GetImmediate(1));

			instruction.Code = Code.Mov_r64_imm64;
			instruction.Op1Kind = OpKind.Immediate64;
			instruction.SetImmediate(1, 0x5AA5123456789ABC);
			Assert.Equal(0x5AA5123456789ABCUL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, 0xA54A123456789ABC);
			Assert.Equal(0xA54A123456789ABCUL, instruction.GetImmediate(1));
			instruction.SetImmediate(1, unchecked((long)0xA54A123456789ABC));
			Assert.Equal(0xA54A123456789ABCUL, instruction.GetImmediate(1));

			Assert.Throws<ArgumentException>(() => instruction.GetImmediate(0));
			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0));
			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0U));
			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0L));
			Assert.Throws<ArgumentException>(() => instruction.SetImmediate(0, 0UL));
		}

		[Fact]
		unsafe void Verify_Instruction_size() {
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
			Assert.Equal(Instruction.TOTAL_SIZE, sizeof(Instruction));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
		}
	}
}
