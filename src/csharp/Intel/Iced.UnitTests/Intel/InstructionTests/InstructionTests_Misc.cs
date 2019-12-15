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

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class InstructionTests_Misc {
		[Fact]
		void Register_is_not_too_big() {
			const int maxValue = IcedConstants.NumberOfRegisters - 1;
			Static.Assert(maxValue < (1 << IcedConstants.RegisterBits) ? 0 : -1);
			Static.Assert(maxValue >= (1 << (IcedConstants.RegisterBits - 1)) ? 0 : -1);
		}

		[Fact]
		void INVALID_Code_value_is_zero() {
			// A 'default' Instruction should be an invalid instruction
			Static.Assert((int)Code.INVALID == 0 ? 0 : -1);
			Instruction instr1 = default;
			Assert.Equal(Code.INVALID, instr1.Code);
			var instr2 = new Instruction();
			Assert.Equal(Code.INVALID, instr2.Code);
			Assert.True(Instruction.EqualsAllBits(instr1, instr2));
		}

#if !NO_ENCODER
		[Fact]
		void Equals_and_GetHashCode_ignore_some_fields() {
			var instr1 = Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm2, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RCX, Register.R14, 8, 0x12345678, 8, false, Register.FS), Register.XMM10, 0xA5);
			var instr2 = instr1;
			Assert.True(Instruction.EqualsAllBits(instr1, instr2));
			instr1.CodeSize = CodeSize.Code32;
			instr2.CodeSize = CodeSize.Code64;
			Assert.False(Instruction.EqualsAllBits(instr1, instr2));
			instr1.Length = 10;
			instr2.Length = 5;
			Assert.True(instr1.Equals(instr2));
			Assert.True(instr1.Equals(ToObject(instr2)));
			Assert.Equal(instr1, instr2);
			Assert.Equal(instr1.GetHashCode(), instr2.GetHashCode());
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		static object ToObject<T>(T value) => value;
#endif

		[Fact]
		void Write_all_properties() {
			Instruction instr = default;

			instr.IP = 0x8A6BD04A9B683A92;
			instr.IP16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instr.IP16);
			Assert.Equal(ushort.MinValue, instr.IP32);
			Assert.Equal(ushort.MinValue, instr.IP);
			instr.IP = 0x8A6BD04A9B683A92;
			instr.IP16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.IP16);
			Assert.Equal(ushort.MaxValue, instr.IP32);
			Assert.Equal(ushort.MaxValue, instr.IP);

			instr.IP = 0x8A6BD04A9B683A92;
			instr.IP32 = uint.MinValue;
			Assert.Equal(ushort.MinValue, instr.IP16);
			Assert.Equal(uint.MinValue, instr.IP32);
			Assert.Equal(uint.MinValue, instr.IP);
			instr.IP = 0x8A6BD04A9B683A92;
			instr.IP32 = uint.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.IP16);
			Assert.Equal(uint.MaxValue, instr.IP32);
			Assert.Equal(uint.MaxValue, instr.IP);

			instr.IP = ulong.MinValue;
			Assert.Equal(ushort.MinValue, instr.IP16);
			Assert.Equal(uint.MinValue, instr.IP32);
			Assert.Equal(ulong.MinValue, instr.IP);
			instr.IP = ulong.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.IP16);
			Assert.Equal(uint.MaxValue, instr.IP32);
			Assert.Equal(ulong.MaxValue, instr.IP);

			instr.NextIP = 0x8A6BD04A9B683A92;
			instr.NextIP16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instr.NextIP16);
			Assert.Equal(ushort.MinValue, instr.NextIP32);
			Assert.Equal(ushort.MinValue, instr.NextIP);
			instr.NextIP = 0x8A6BD04A9B683A92;
			instr.NextIP16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.NextIP16);
			Assert.Equal(ushort.MaxValue, instr.NextIP32);
			Assert.Equal(ushort.MaxValue, instr.NextIP);

			instr.NextIP = 0x8A6BD04A9B683A92;
			instr.NextIP32 = uint.MinValue;
			Assert.Equal(ushort.MinValue, instr.NextIP16);
			Assert.Equal(uint.MinValue, instr.NextIP32);
			Assert.Equal(uint.MinValue, instr.NextIP);
			instr.NextIP = 0x8A6BD04A9B683A92;
			instr.NextIP32 = uint.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.NextIP16);
			Assert.Equal(uint.MaxValue, instr.NextIP32);
			Assert.Equal(uint.MaxValue, instr.NextIP);

			instr.NextIP = ulong.MinValue;
			Assert.Equal(ushort.MinValue, instr.NextIP16);
			Assert.Equal(uint.MinValue, instr.NextIP32);
			Assert.Equal(ulong.MinValue, instr.NextIP);
			instr.NextIP = ulong.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.NextIP16);
			Assert.Equal(uint.MaxValue, instr.NextIP32);
			Assert.Equal(ulong.MaxValue, instr.NextIP);

			instr.MemoryDisplacement = uint.MinValue;
			Assert.Equal(uint.MinValue, instr.MemoryDisplacement);
			Assert.Equal(ulong.MinValue, instr.MemoryDisplacement64);
			instr.MemoryDisplacement = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instr.MemoryDisplacement);
			Assert.Equal(ulong.MaxValue, instr.MemoryDisplacement64);

			instr.Immediate8 = byte.MinValue;
			Assert.Equal(byte.MinValue, instr.Immediate8);
			instr.Immediate8 = byte.MaxValue;
			Assert.Equal(byte.MaxValue, instr.Immediate8);

			instr.Immediate8_2nd = byte.MinValue;
			Assert.Equal(byte.MinValue, instr.Immediate8_2nd);
			instr.Immediate8_2nd = byte.MaxValue;
			Assert.Equal(byte.MaxValue, instr.Immediate8_2nd);

			instr.Immediate16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instr.Immediate16);
			instr.Immediate16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.Immediate16);

			instr.Immediate32 = uint.MinValue;
			Assert.Equal(uint.MinValue, instr.Immediate32);
			instr.Immediate32 = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instr.Immediate32);

			instr.Immediate64 = ulong.MinValue;
			Assert.Equal(ulong.MinValue, instr.Immediate64);
			instr.Immediate64 = ulong.MaxValue;
			Assert.Equal(ulong.MaxValue, instr.Immediate64);

			instr.Immediate8to16 = sbyte.MinValue;
			Assert.Equal(sbyte.MinValue, instr.Immediate8to16);
			instr.Immediate8to16 = sbyte.MaxValue;
			Assert.Equal(sbyte.MaxValue, instr.Immediate8to16);

			instr.Immediate8to32 = sbyte.MinValue;
			Assert.Equal(sbyte.MinValue, instr.Immediate8to32);
			instr.Immediate8to32 = sbyte.MaxValue;
			Assert.Equal(sbyte.MaxValue, instr.Immediate8to32);

			instr.Immediate8to64 = sbyte.MinValue;
			Assert.Equal(sbyte.MinValue, instr.Immediate8to64);
			instr.Immediate8to64 = sbyte.MaxValue;
			Assert.Equal(sbyte.MaxValue, instr.Immediate8to64);

			instr.Immediate32to64 = int.MinValue;
			Assert.Equal(int.MinValue, instr.Immediate32to64);
			instr.Immediate32to64 = int.MaxValue;
			Assert.Equal(int.MaxValue, instr.Immediate32to64);

			instr.MemoryAddress64 = ulong.MinValue;
			Assert.Equal(ulong.MinValue, instr.MemoryAddress64);
			instr.MemoryAddress64 = ulong.MaxValue;
			Assert.Equal(ulong.MaxValue, instr.MemoryAddress64);

			instr.Op0Kind = OpKind.NearBranch16;
			instr.NearBranch16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instr.NearBranch16);
			Assert.Equal(ushort.MinValue, instr.NearBranchTarget);
			instr.NearBranch16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.NearBranch16);
			Assert.Equal(ushort.MaxValue, instr.NearBranchTarget);

			instr.Op0Kind = OpKind.NearBranch32;
			instr.NearBranch32 = uint.MinValue;
			Assert.Equal(uint.MinValue, instr.NearBranch32);
			Assert.Equal(uint.MinValue, instr.NearBranchTarget);
			instr.NearBranch32 = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instr.NearBranch32);
			Assert.Equal(uint.MaxValue, instr.NearBranchTarget);

			instr.Op0Kind = OpKind.NearBranch64;
			instr.NearBranch64 = ulong.MinValue;
			Assert.Equal(ulong.MinValue, instr.NearBranch64);
			Assert.Equal(ulong.MinValue, instr.NearBranchTarget);
			instr.NearBranch64 = ulong.MaxValue;
			Assert.Equal(ulong.MaxValue, instr.NearBranch64);
			Assert.Equal(ulong.MaxValue, instr.NearBranchTarget);

			instr.FarBranch16 = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instr.FarBranch16);
			instr.FarBranch16 = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.FarBranch16);

			instr.FarBranch32 = uint.MinValue;
			Assert.Equal(uint.MinValue, instr.FarBranch32);
			instr.FarBranch32 = uint.MaxValue;
			Assert.Equal(uint.MaxValue, instr.FarBranch32);

			instr.FarBranchSelector = ushort.MinValue;
			Assert.Equal(ushort.MinValue, instr.FarBranchSelector);
			instr.FarBranchSelector = ushort.MaxValue;
			Assert.Equal(ushort.MaxValue, instr.FarBranchSelector);

			instr.HasXacquirePrefix = false;
			Assert.False(instr.HasXacquirePrefix);
			instr.HasXacquirePrefix = true;
			Assert.True(instr.HasXacquirePrefix);

			instr.HasXreleasePrefix = false;
			Assert.False(instr.HasXreleasePrefix);
			instr.HasXreleasePrefix = true;
			Assert.True(instr.HasXreleasePrefix);

			instr.HasRepPrefix = false;
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			instr.HasRepPrefix = true;
			Assert.True(instr.HasRepPrefix);
			Assert.True(instr.HasRepePrefix);

			instr.HasRepePrefix = false;
			Assert.False(instr.HasRepPrefix);
			Assert.False(instr.HasRepePrefix);
			instr.HasRepePrefix = true;
			Assert.True(instr.HasRepPrefix);
			Assert.True(instr.HasRepePrefix);

			instr.HasRepnePrefix = false;
			Assert.False(instr.HasRepnePrefix);
			instr.HasRepnePrefix = true;
			Assert.True(instr.HasRepnePrefix);

			instr.HasLockPrefix = false;
			Assert.False(instr.HasLockPrefix);
			instr.HasLockPrefix = true;
			Assert.True(instr.HasLockPrefix);

			instr.IsBroadcast = false;
			Assert.False(instr.IsBroadcast);
			instr.IsBroadcast = true;
			Assert.True(instr.IsBroadcast);

			instr.SuppressAllExceptions = false;
			Assert.False(instr.SuppressAllExceptions);
			instr.SuppressAllExceptions = true;
			Assert.True(instr.SuppressAllExceptions);

			for (int i = 0; i <= IcedConstants.MaxInstructionLength; i++) {
				instr.Length = i;
				Assert.Equal(i, instr.Length);
			}

			foreach (var codeSize in GetCodeSizeValues()) {
				instr.CodeSize = codeSize;
				Assert.Equal(codeSize, instr.CodeSize);
			}

			foreach (var code in GetCodeValues()) {
				instr.Code = code;
				Assert.Equal(code, instr.Code);
			}
			foreach (var code in GetCodeValues()) {
				instr.InternalSetCodeNoCheck(code);
				Assert.Equal(code, instr.Code);
			}
			Assert.Throws<ArgumentOutOfRangeException>(() => instr.Code = (Code)(-1));
			Assert.Throws<ArgumentOutOfRangeException>(() => instr.Code = (Code)IcedConstants.NumberOfCodeValues);

			Static.Assert(IcedConstants.MaxOpCount == 5 ? 0 : -1);
			foreach (var opKind in GetOpKindValues()) {
				instr.Op0Kind = opKind;
				Assert.Equal(opKind, instr.Op0Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instr.Op1Kind = opKind;
				Assert.Equal(opKind, instr.Op1Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instr.Op2Kind = opKind;
				Assert.Equal(opKind, instr.Op2Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instr.Op3Kind = opKind;
				Assert.Equal(opKind, instr.Op3Kind);
			}

			foreach (var opKind in GetOpKindValues()) {
				if (opKind == OpKind.Immediate8) {
					instr.Op4Kind = opKind;
					Assert.Equal(opKind, instr.Op4Kind);
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instr.Op4Kind = opKind);
			}

			foreach (var opKind in GetOpKindValues()) {
				instr.SetOpKind(0, opKind);
				Assert.Equal(opKind, instr.Op0Kind);
				Assert.Equal(opKind, instr.GetOpKind(0));
			}

			foreach (var opKind in GetOpKindValues()) {
				instr.SetOpKind(1, opKind);
				Assert.Equal(opKind, instr.Op1Kind);
				Assert.Equal(opKind, instr.GetOpKind(1));
			}

			foreach (var opKind in GetOpKindValues()) {
				instr.SetOpKind(2, opKind);
				Assert.Equal(opKind, instr.Op2Kind);
				Assert.Equal(opKind, instr.GetOpKind(2));
			}

			foreach (var opKind in GetOpKindValues()) {
				instr.SetOpKind(3, opKind);
				Assert.Equal(opKind, instr.Op3Kind);
				Assert.Equal(opKind, instr.GetOpKind(3));
			}

			foreach (var opKind in GetOpKindValues()) {
				if (opKind == OpKind.Immediate8) {
					instr.SetOpKind(4, opKind);
					Assert.Equal(opKind, instr.Op4Kind);
					Assert.Equal(opKind, instr.GetOpKind(4));
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instr.SetOpKind(4, opKind));
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
				instr.SegmentPrefix = seg;
				Assert.Equal(seg, instr.SegmentPrefix);
				if (instr.SegmentPrefix == Register.None)
					Assert.False(instr.HasSegmentPrefix);
				else
					Assert.True(instr.HasSegmentPrefix);
			}

			var displSizes = new int[] { 8, 4, 2, 1, 0 };
			foreach (var displSize in displSizes) {
				instr.MemoryDisplSize = displSize;
				Assert.Equal(displSize, instr.MemoryDisplSize);
			}

			var scaleValues = new int[] { 8, 4, 2, 1 };
			foreach (var scaleValue in scaleValues) {
				instr.MemoryIndexScale = scaleValue;
				Assert.Equal(scaleValue, instr.MemoryIndexScale);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.MemoryBase = reg;
				Assert.Equal(reg, instr.MemoryBase);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.MemoryIndex = reg;
				Assert.Equal(reg, instr.MemoryIndex);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.Op0Register = reg;
				Assert.Equal(reg, instr.Op0Register);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.Op1Register = reg;
				Assert.Equal(reg, instr.Op1Register);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.Op2Register = reg;
				Assert.Equal(reg, instr.Op2Register);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.Op3Register = reg;
				Assert.Equal(reg, instr.Op3Register);
			}

			foreach (var reg in GetRegisterValues()) {
				if (reg == Register.None) {
					instr.Op4Register = reg;
					Assert.Equal(reg, instr.Op4Register);
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instr.Op4Register = reg);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.SetOpRegister(0, reg);
				Assert.Equal(reg, instr.Op0Register);
				Assert.Equal(reg, instr.GetOpRegister(0));
			}

			foreach (var reg in GetRegisterValues()) {
				instr.SetOpRegister(1, reg);
				Assert.Equal(reg, instr.Op1Register);
				Assert.Equal(reg, instr.GetOpRegister(1));
			}

			foreach (var reg in GetRegisterValues()) {
				instr.SetOpRegister(2, reg);
				Assert.Equal(reg, instr.Op2Register);
				Assert.Equal(reg, instr.GetOpRegister(2));
			}

			foreach (var reg in GetRegisterValues()) {
				instr.SetOpRegister(3, reg);
				Assert.Equal(reg, instr.Op3Register);
				Assert.Equal(reg, instr.GetOpRegister(3));
			}

			foreach (var reg in GetRegisterValues()) {
				if (reg == Register.None) {
					instr.SetOpRegister(4, reg);
					Assert.Equal(reg, instr.Op4Register);
					Assert.Equal(reg, instr.GetOpRegister(4));
				}
				else
					Assert.Throws<ArgumentOutOfRangeException>(() => instr.SetOpRegister(4, reg));
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
				instr.OpMask = opMask;
				Assert.Equal(opMask, instr.OpMask);
				Assert.Equal(opMask != Register.None, instr.HasOpMask);
			}

			instr.ZeroingMasking = false;
			Assert.False(instr.ZeroingMasking);
			Assert.True(instr.MergingMasking);
			instr.ZeroingMasking = true;
			Assert.True(instr.ZeroingMasking);
			Assert.False(instr.MergingMasking);
			instr.MergingMasking = false;
			Assert.False(instr.MergingMasking);
			Assert.True(instr.ZeroingMasking);
			instr.MergingMasking = true;
			Assert.True(instr.MergingMasking);
			Assert.False(instr.ZeroingMasking);

			foreach (var rc in GetRoundingControlValues()) {
				instr.RoundingControl = rc;
				Assert.Equal(rc, instr.RoundingControl);
			}

			foreach (var reg in GetRegisterValues()) {
				instr.MemoryBase = reg;
				Assert.Equal(reg == Register.RIP || reg == Register.EIP, instr.IsIPRelativeMemoryOperand);
			}

			instr.MemoryBase = Register.EIP;
			instr.NextIP = 0x123456701EDCBA98;
			instr.MemoryDisplacement = 0x87654321;
			Assert.True(instr.IsIPRelativeMemoryOperand);
			Assert.Equal(0xA641FDB9UL, instr.IPRelativeMemoryAddress);

			instr.MemoryBase = Register.RIP;
			instr.NextIP = 0x123456701EDCBA98;
			instr.MemoryDisplacement = 0x87654321;
			Assert.True(instr.IsIPRelativeMemoryOperand);
			Assert.Equal(0x1234566FA641FDB9UL, instr.IPRelativeMemoryAddress);

			instr.DeclareDataCount = 1;
			Assert.Equal(1, instr.DeclareDataCount);
			instr.DeclareDataCount = 15;
			Assert.Equal(15, instr.DeclareDataCount);
			instr.DeclareDataCount = 16;
			Assert.Equal(16, instr.DeclareDataCount);
		}

		static IEnumerable<CodeSize> GetCodeSizeValues() {
			for (int i = 0; i < IcedConstants.NumberOfCodeSizes; i++) {
				yield return (CodeSize)i;
			}
		}
		static IEnumerable<Code> GetCodeValues() {
			for (int i = 0; i < IcedConstants.NumberOfCodeValues; i++) {
				yield return (Code)i;
			}
		}
		static IEnumerable<OpKind> GetOpKindValues() {
			for (int i = 0; i < IcedConstants.NumberOfOpKinds; i++) {
				yield return (OpKind)i;
			}
		}
		static IEnumerable<Register> GetRegisterValues() {
			for (int i = 0; i < IcedConstants.NumberOfRegisters; i++) {
				yield return (Register)i;
			}
		}
		static IEnumerable<RoundingControl> GetRoundingControlValues() {
			for (int i = 0; i < IcedConstants.NumberOfRoundingControlValues; i++) {
				yield return (RoundingControl)i;
			}
		}

		[Fact]
		void Verify_GetSetImmediate() {
			Instruction instr = default;

			instr.Code = Code.Add_AL_imm8;
			instr.Op1Kind = OpKind.Immediate8;
			instr.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA5);
			Assert.Equal(0xA5UL, instr.GetImmediate(1));

			instr.Code = Code.Add_AX_imm16;
			instr.Op1Kind = OpKind.Immediate16;
			instr.SetImmediate(1, 0x5AA5);
			Assert.Equal(0x5AA5UL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA55A);
			Assert.Equal(0xA55AUL, instr.GetImmediate(1));

			instr.Code = Code.Add_EAX_imm32;
			instr.Op1Kind = OpKind.Immediate32;
			instr.SetImmediate(1, 0x5AA51234);
			Assert.Equal(0x5AA51234UL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA54A1234);
			Assert.Equal(0xA54A1234UL, instr.GetImmediate(1));

			instr.Code = Code.Add_RAX_imm32;
			instr.Op1Kind = OpKind.Immediate32to64;
			instr.SetImmediate(1, 0x5AA51234);
			Assert.Equal(0x5AA51234UL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA54A1234);
			Assert.Equal(0xFFFFFFFFA54A1234UL, instr.GetImmediate(1));

			instr.Code = Code.Enterq_imm16_imm8;
			instr.Op1Kind = OpKind.Immediate8_2nd;
			instr.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA5);
			Assert.Equal(0xA5UL, instr.GetImmediate(1));

			instr.Code = Code.Adc_rm16_imm8;
			instr.Op1Kind = OpKind.Immediate8to16;
			instr.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA5);
			Assert.Equal(0xFFFFFFFFFFFFFFA5UL, instr.GetImmediate(1));

			instr.Code = Code.Adc_rm32_imm8;
			instr.Op1Kind = OpKind.Immediate8to32;
			instr.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA5);
			Assert.Equal(0xFFFFFFFFFFFFFFA5UL, instr.GetImmediate(1));

			instr.Code = Code.Adc_rm64_imm8;
			instr.Op1Kind = OpKind.Immediate8to64;
			instr.SetImmediate(1, 0x5A);
			Assert.Equal(0x5AUL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA5);
			Assert.Equal(0xFFFFFFFFFFFFFFA5UL, instr.GetImmediate(1));

			instr.Code = Code.Mov_r64_imm64;
			instr.Op1Kind = OpKind.Immediate64;
			instr.SetImmediate(1, 0x5AA5123456789ABC);
			Assert.Equal(0x5AA5123456789ABCUL, instr.GetImmediate(1));
			instr.SetImmediate(1, 0xA54A123456789ABC);
			Assert.Equal(0xA54A123456789ABCUL, instr.GetImmediate(1));
			instr.SetImmediate(1, unchecked((long)0xA54A123456789ABC));
			Assert.Equal(0xA54A123456789ABCUL, instr.GetImmediate(1));

			Assert.Throws<ArgumentException>(() => instr.GetImmediate(0));
			Assert.Throws<ArgumentException>(() => instr.SetImmediate(0, 0));
			Assert.Throws<ArgumentException>(() => instr.SetImmediate(0, 0U));
			Assert.Throws<ArgumentException>(() => instr.SetImmediate(0, 0L));
			Assert.Throws<ArgumentException>(() => instr.SetImmediate(0, 0UL));
		}

		[Fact]
		unsafe void Verify_Instruction_size() {
#pragma warning disable xUnit2000 // Constants and literals should be the expected argument
			Assert.Equal(Instruction.TOTAL_SIZE, sizeof(Instruction));
#pragma warning restore xUnit2000 // Constants and literals should be the expected argument
		}
	}
}
