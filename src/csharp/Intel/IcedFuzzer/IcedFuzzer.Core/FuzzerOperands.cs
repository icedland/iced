// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Iced.Intel;

namespace IcedFuzzer.Core {
	static class FuzzerOperands {
		static readonly FuzzerOperand invalid = new FuzzerOperand(FuzzerOperandKind.None);
		static readonly (FuzzerOperand op, FuzzerOperand modrmMem)[] operands = CreateOperands();
		public static readonly RegisterFuzzerOperand OpMaskRegister = new RegisterFuzzerOperand(FuzzerRegisterClass.K, FuzzerRegisterKind.K, FuzzerOperandRegLocation.AaaBits);

		public static FuzzerOperand GetOperand(OpCodeOperandKind opKind, bool isModrmMemory) {
			var info = operands[(int)opKind];
			var op = isModrmMemory ? info.modrmMem : info.op;
			Assert.True(op != invalid);
			return op;
		}

		static (FuzzerOperand op, FuzzerOperand modrmMem)[] CreateOperands() {
			Assert.False(invalid is null);
			var opKinds = (OpCodeOperandKind[])Enum.GetValues(typeof(OpCodeOperandKind));
			var operands = new (FuzzerOperand op, FuzzerOperand modrmMem)[opKinds.Length];

			var none = new FuzzerOperand(FuzzerOperandKind.None);
			var imm1 = new ImmediateFuzzerOperand(FuzzerImmediateKind.Imm1);
			var imm2 = new ImmediateFuzzerOperand(FuzzerImmediateKind.Imm2);
			var imm4 = new ImmediateFuzzerOperand(FuzzerImmediateKind.Imm4);
			foreach (var opKind in opKinds) {
				var (op, modrmMem) = opKind switch {
					OpCodeOperandKind.None => (none, null),
					OpCodeOperandKind.farbr2_2 => (new ImmediateFuzzerOperand(FuzzerImmediateKind.Imm2_2), null),
					OpCodeOperandKind.farbr4_2 => (new ImmediateFuzzerOperand(FuzzerImmediateKind.Imm4_2), null),
					OpCodeOperandKind.mem_offs => (new FuzzerOperand(FuzzerOperandKind.MemOffs), null),
					OpCodeOperandKind.mem => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.mem_mpx => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.MPX | ModrmMemoryFuzzerOperandFlags.NoRipRel)),
					OpCodeOperandKind.mem_mib => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.MPX | ModrmMemoryFuzzerOperandFlags.NoRipRel)),
					OpCodeOperandKind.mem_vsib32x => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.Vsib)),
					OpCodeOperandKind.mem_vsib64x => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.Vsib)),
					OpCodeOperandKind.mem_vsib32y => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.Vsib)),
					OpCodeOperandKind.mem_vsib64y => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.Vsib)),
					OpCodeOperandKind.mem_vsib32z => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.Vsib)),
					OpCodeOperandKind.mem_vsib64z => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.Vsib)),
					OpCodeOperandKind.r8_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR8, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.r16_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR16, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.r32_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR32, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.r32_or_mem_mpx => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR32, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.MPX)),
					OpCodeOperandKind.r64_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR64, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.r64_or_mem_mpx => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR64, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.MPX)),
					OpCodeOperandKind.mm_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.MM, FuzzerRegisterKind.MM, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.xmm_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.XMM, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.ymm_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.YMM, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.zmm_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.ZMM, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.bnd_or_mem_mpx => (new RegisterFuzzerOperand(FuzzerRegisterClass.BND, FuzzerRegisterKind.BND, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.MPX)),
					OpCodeOperandKind.k_or_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.K, FuzzerRegisterKind.K, FuzzerOperandRegLocation.ModrmRmBits), new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.None)),
					OpCodeOperandKind.r8_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR8, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.r8_opcode => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR8, FuzzerOperandRegLocation.OpCodeBits), null),
					OpCodeOperandKind.r16_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR16, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.r16_reg_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR16, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.r16_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR16, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.r16_opcode => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR16, FuzzerOperandRegLocation.OpCodeBits), null),
					OpCodeOperandKind.r32_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR32, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.r32_reg_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR32, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.r32_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR32, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.r32_opcode => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR32, FuzzerOperandRegLocation.OpCodeBits), null),
					OpCodeOperandKind.r32_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR32, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.r64_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR64, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.r64_reg_mem => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR64, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.r64_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR64, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.r64_opcode => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR64, FuzzerOperandRegLocation.OpCodeBits), null),
					OpCodeOperandKind.r64_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.GPR, FuzzerRegisterKind.GPR64, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.seg_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.Segment, FuzzerRegisterKind.Segment, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.k_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.K, FuzzerRegisterKind.K, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.kp1_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.K, FuzzerRegisterKind.K, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.k_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.K, FuzzerRegisterKind.K, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.k_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.K, FuzzerRegisterKind.K, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.mm_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.MM, FuzzerRegisterKind.MM, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.mm_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.MM, FuzzerRegisterKind.MM, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.xmm_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.XMM, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.xmm_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.XMM, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.xmm_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.XMM, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.xmmp3_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.XMM, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.xmm_is4 => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.XMM, FuzzerOperandRegLocation.Is4Bits), null),
					OpCodeOperandKind.xmm_is5 => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.XMM, FuzzerOperandRegLocation.Is5Bits), null),
					OpCodeOperandKind.ymm_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.YMM, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.ymm_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.YMM, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.ymm_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.YMM, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.ymm_is4 => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.YMM, FuzzerOperandRegLocation.Is4Bits), null),
					OpCodeOperandKind.ymm_is5 => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.YMM, FuzzerOperandRegLocation.Is5Bits), null),
					OpCodeOperandKind.zmm_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.ZMM, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.zmm_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.ZMM, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.zmm_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.ZMM, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.zmmp3_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.Vector, FuzzerRegisterKind.ZMM, FuzzerOperandRegLocation.VvvvBits), null),
					OpCodeOperandKind.cr_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.CR, FuzzerRegisterKind.CR, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.dr_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.DR, FuzzerRegisterKind.DR, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.tr_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.TR, FuzzerRegisterKind.TR, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.bnd_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.BND, FuzzerRegisterKind.BND, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.es => (none, null),
					OpCodeOperandKind.cs => (none, null),
					OpCodeOperandKind.ss => (none, null),
					OpCodeOperandKind.ds => (none, null),
					OpCodeOperandKind.fs => (none, null),
					OpCodeOperandKind.gs => (none, null),
					OpCodeOperandKind.al => (none, null),
					OpCodeOperandKind.cl => (none, null),
					OpCodeOperandKind.ax => (none, null),
					OpCodeOperandKind.dx => (none, null),
					OpCodeOperandKind.eax => (none, null),
					OpCodeOperandKind.rax => (none, null),
					OpCodeOperandKind.st0 => (none, null),
					OpCodeOperandKind.sti_opcode => (new RegisterFuzzerOperand(FuzzerRegisterClass.ST, FuzzerRegisterKind.ST, FuzzerOperandRegLocation.OpCodeBits), null),
					OpCodeOperandKind.imm4_m2z => (none, null),
					OpCodeOperandKind.imm8 => (imm1, null),
					OpCodeOperandKind.imm8_const_1 => (none, null),
					OpCodeOperandKind.imm8sex16 => (imm1, null),
					OpCodeOperandKind.imm8sex32 => (imm1, null),
					OpCodeOperandKind.imm8sex64 => (imm1, null),
					OpCodeOperandKind.imm16 => (imm2, null),
					OpCodeOperandKind.imm32 => (imm4, null),
					OpCodeOperandKind.imm32sex64 => (imm4, null),
					OpCodeOperandKind.imm64 => (new ImmediateFuzzerOperand(FuzzerImmediateKind.Imm8), null),
					OpCodeOperandKind.seg_rSI => (new FuzzerOperand(FuzzerOperandKind.ImpliedMem), null),
					OpCodeOperandKind.es_rDI => (new FuzzerOperand(FuzzerOperandKind.ImpliedMem), null),
					OpCodeOperandKind.seg_rDI => (new FuzzerOperand(FuzzerOperandKind.ImpliedMem), null),
					OpCodeOperandKind.seg_rBX_al => (new FuzzerOperand(FuzzerOperandKind.ImpliedMem), null),
					OpCodeOperandKind.br16_1 => (imm1, null),
					OpCodeOperandKind.br32_1 => (imm1, null),
					OpCodeOperandKind.br64_1 => (imm1, null),
					OpCodeOperandKind.br16_2 => (imm2, null),
					OpCodeOperandKind.br32_4 => (imm4, null),
					OpCodeOperandKind.br64_4 => (imm4, null),
					OpCodeOperandKind.xbegin_2 => (imm2, null),
					OpCodeOperandKind.xbegin_4 => (imm4, null),
					OpCodeOperandKind.brdisp_2 => (imm2, null),
					OpCodeOperandKind.brdisp_4 => (imm4, null),
					OpCodeOperandKind.sibmem => (invalid, new ModrmMemoryFuzzerOperand(ModrmMemoryFuzzerOperandFlags.Sib)),
					OpCodeOperandKind.tmm_reg => (new RegisterFuzzerOperand(FuzzerRegisterClass.TMM, FuzzerRegisterKind.TMM, FuzzerOperandRegLocation.ModrmRegBits), null),
					OpCodeOperandKind.tmm_rm => (new RegisterFuzzerOperand(FuzzerRegisterClass.TMM, FuzzerRegisterKind.TMM, FuzzerOperandRegLocation.ModrmRmBits), null),
					OpCodeOperandKind.tmm_vvvv => (new RegisterFuzzerOperand(FuzzerRegisterClass.TMM, FuzzerRegisterKind.TMM, FuzzerOperandRegLocation.VvvvBits), null),
					_ => throw ThrowHelpers.Unreachable,
				};
				operands[(int)opKind] = (op, modrmMem ?? op);
			}

			foreach (var info in operands)
				Assert.False(info.op is null || info.modrmMem is null);
			return operands;
		}
	}
}
