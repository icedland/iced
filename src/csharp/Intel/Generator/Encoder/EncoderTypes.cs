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
using System.Linq;
using Generator.Enums;
using Generator.Enums.Encoder;

namespace Generator.Encoder {
	[TypeGen(TypeGenOrders.CreatedInstructions)]
	sealed class EncoderTypes {
		public (EnumValue value, uint size)[] ImmSizes { get; }
		public (EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] LegacyOpHandlers { get; }
		public (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] VexOpHandlers { get; }
		public (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] XopOpHandlers { get; }
		public (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] EvexOpHandlers { get; }
		readonly Dictionary<OpCodeOperandKind, LegacyOpKind> toLegacy;
		readonly Dictionary<OpCodeOperandKind, VexOpKind> toVex;
		readonly Dictionary<OpCodeOperandKind, XopOpKind> toXop;
		readonly Dictionary<OpCodeOperandKind, EvexOpKind> toEvex;

		EncoderTypes(GenTypes genTypes) {
			var gen = new EncoderTypesGen(genTypes);
			gen.Generate();
			ImmSizes = gen.ImmSizes ?? throw new InvalidOperationException();
			genTypes.Add(gen.EncFlags1 ?? throw new InvalidOperationException());

			var opCodeOperandKind = genTypes[TypeIds.OpCodeOperandKind];
			var legacyOpKind = genTypes[TypeIds.LegacyOpKind];
			var vexOpKind = genTypes[TypeIds.VexOpKind];
			var xopOpKind = genTypes[TypeIds.XopOpKind];
			var evexOpKind = genTypes[TypeIds.EvexOpKind];
			var register = genTypes[TypeIds.Register];
			var opKind = genTypes[TypeIds.OpKind];

			LegacyOpHandlers = new (EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], legacyOpKind[nameof(LegacyOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.farbr2_2)], legacyOpKind[nameof(LegacyOpKind.farbr2_2)], OpHandlerKind.OpA, new object[] { 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.farbr4_2)], legacyOpKind[nameof(LegacyOpKind.farbr4_2)], OpHandlerKind.OpA, new object[] { 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.mem)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { false }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_mpx)], legacyOpKind[nameof(LegacyOpKind.mem_mpx)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { false }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r8_or_mem)], legacyOpKind[nameof(LegacyOpKind.r8_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.AL)], register[nameof(Register.R15L)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_or_mem)], legacyOpKind[nameof(LegacyOpKind.r16_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], legacyOpKind[nameof(LegacyOpKind.r32_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem_mpx)], legacyOpKind[nameof(LegacyOpKind.r32_or_mem_mpx)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], legacyOpKind[nameof(LegacyOpKind.r64_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem_mpx)], legacyOpKind[nameof(LegacyOpKind.r64_or_mem_mpx)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r8_reg)], legacyOpKind[nameof(LegacyOpKind.r8_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.AL)], register[nameof(Register.R15L)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_reg)], legacyOpKind[nameof(LegacyOpKind.r16_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], legacyOpKind[nameof(LegacyOpKind.r32_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], legacyOpKind[nameof(LegacyOpKind.r64_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_reg_mem)], legacyOpKind[nameof(LegacyOpKind.r16_reg_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg_mem)], legacyOpKind[nameof(LegacyOpKind.r32_reg_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg_mem)], legacyOpKind[nameof(LegacyOpKind.r64_reg_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_rm)], legacyOpKind[nameof(LegacyOpKind.r16_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], legacyOpKind[nameof(LegacyOpKind.r32_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], legacyOpKind[nameof(LegacyOpKind.r64_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_reg)], legacyOpKind[nameof(LegacyOpKind.seg_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.ES)], register[nameof(Register.GS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.cr_reg)], legacyOpKind[nameof(LegacyOpKind.cr_reg)], OpHandlerKind.OpModRM_regF0, new object[] { register[nameof(Register.CR0)], register[nameof(Register.CR15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.dr_reg)], legacyOpKind[nameof(LegacyOpKind.dr_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.DR0)], register[nameof(Register.DR15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.tr_reg)], legacyOpKind[nameof(LegacyOpKind.tr_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.TR0)], register[nameof(Register.TR7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], legacyOpKind[nameof(LegacyOpKind.imm8)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8sex16)], legacyOpKind[nameof(LegacyOpKind.imm8sex16)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8to16)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8sex32)], legacyOpKind[nameof(LegacyOpKind.imm8sex32)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8to32)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8sex64)], legacyOpKind[nameof(LegacyOpKind.imm8sex64)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8to64)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm16)], legacyOpKind[nameof(LegacyOpKind.imm16)], OpHandlerKind.OpIw, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm32)], legacyOpKind[nameof(LegacyOpKind.imm32)], OpHandlerKind.OpId, new object[] { opKind[nameof(OpKind.Immediate32)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm32sex64)], legacyOpKind[nameof(LegacyOpKind.imm32sex64)], OpHandlerKind.OpId, new object[] { opKind[nameof(OpKind.Immediate32to64)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm64)], legacyOpKind[nameof(LegacyOpKind.imm64)], OpHandlerKind.OpIq, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rSI)], legacyOpKind[nameof(LegacyOpKind.seg_rSI)], OpHandlerKind.OpX, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.es_rDI)], legacyOpKind[nameof(LegacyOpKind.es_rDI)], OpHandlerKind.OpY, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br16_1)], legacyOpKind[nameof(LegacyOpKind.br16_1)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch16)], 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br32_1)], legacyOpKind[nameof(LegacyOpKind.br32_1)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch32)], 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br64_1)], legacyOpKind[nameof(LegacyOpKind.br64_1)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch64)], 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br16_2)], legacyOpKind[nameof(LegacyOpKind.br16_2)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch16)], 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br32_4)], legacyOpKind[nameof(LegacyOpKind.br32_4)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch32)], 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br64_4)], legacyOpKind[nameof(LegacyOpKind.br64_4)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch64)], 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xbegin_2)], legacyOpKind[nameof(LegacyOpKind.xbegin_2)], OpHandlerKind.OpJx, new object[] { 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xbegin_4)], legacyOpKind[nameof(LegacyOpKind.xbegin_4)], OpHandlerKind.OpJx, new object[] { 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.brdisp_2)], legacyOpKind[nameof(LegacyOpKind.brdisp_2)], OpHandlerKind.OpJdisp, new object[] { 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.brdisp_4)], legacyOpKind[nameof(LegacyOpKind.brdisp_4)], OpHandlerKind.OpJdisp, new object[] { 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_offs)], legacyOpKind[nameof(LegacyOpKind.mem_offs)], OpHandlerKind.OpO, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8_const_1)], legacyOpKind[nameof(LegacyOpKind.imm8_const_1)], OpHandlerKind.OpImm, new object[] { 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.bnd_reg)], legacyOpKind[nameof(LegacyOpKind.bnd_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.BND0)], register[nameof(Register.BND3)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.bnd_or_mem_mpx)], legacyOpKind[nameof(LegacyOpKind.bnd_or_mem_mpx)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.BND0)], register[nameof(Register.BND3)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_mib)], legacyOpKind[nameof(LegacyOpKind.mem_mib)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { false }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mm_rm)], legacyOpKind[nameof(LegacyOpKind.mm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.MM0)], register[nameof(Register.MM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mm_reg)], legacyOpKind[nameof(LegacyOpKind.mm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.MM0)], register[nameof(Register.MM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mm_or_mem)], legacyOpKind[nameof(LegacyOpKind.mm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.MM0)], register[nameof(Register.MM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)], legacyOpKind[nameof(LegacyOpKind.xmm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], legacyOpKind[nameof(LegacyOpKind.xmm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], legacyOpKind[nameof(LegacyOpKind.xmm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rDI)], legacyOpKind[nameof(LegacyOpKind.seg_rDI)], OpHandlerKind.OprDI, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rBX_al)], legacyOpKind[nameof(LegacyOpKind.seg_rBX_al)], OpHandlerKind.OpMRBX, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.es)], legacyOpKind[nameof(LegacyOpKind.es)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.ES)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.cs)], legacyOpKind[nameof(LegacyOpKind.cs)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.CS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ss)], legacyOpKind[nameof(LegacyOpKind.ss)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.SS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ds)], legacyOpKind[nameof(LegacyOpKind.ds)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.DS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.fs)], legacyOpKind[nameof(LegacyOpKind.fs)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.FS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.gs)], legacyOpKind[nameof(LegacyOpKind.gs)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.GS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.al)], legacyOpKind[nameof(LegacyOpKind.al)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.AL)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.cl)], legacyOpKind[nameof(LegacyOpKind.cl)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.CL)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ax)], legacyOpKind[nameof(LegacyOpKind.ax)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.AX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.dx)], legacyOpKind[nameof(LegacyOpKind.dx)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.DX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.eax)], legacyOpKind[nameof(LegacyOpKind.eax)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.EAX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.rax)], legacyOpKind[nameof(LegacyOpKind.rax)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.RAX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.st0)], legacyOpKind[nameof(LegacyOpKind.st0)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.ST0)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.sti_opcode)], legacyOpKind[nameof(LegacyOpKind.sti_opcode)], OpHandlerKind.OpRegSTi, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r8_opcode)], legacyOpKind[nameof(LegacyOpKind.r8_opcode)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.AL)], register[nameof(Register.R15L)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_opcode)], legacyOpKind[nameof(LegacyOpKind.r16_opcode)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_opcode)], legacyOpKind[nameof(LegacyOpKind.r32_opcode)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_opcode)], legacyOpKind[nameof(LegacyOpKind.r64_opcode)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
			};
			VexOpHandlers = new (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], vexOpKind[nameof(VexOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], vexOpKind[nameof(VexOpKind.r32_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], vexOpKind[nameof(VexOpKind.r64_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], vexOpKind[nameof(VexOpKind.r32_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], vexOpKind[nameof(VexOpKind.r64_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], vexOpKind[nameof(VexOpKind.r32_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], vexOpKind[nameof(VexOpKind.r64_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_vvvv)], vexOpKind[nameof(VexOpKind.r32_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_vvvv)], vexOpKind[nameof(VexOpKind.r64_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_vvvv)], vexOpKind[nameof(VexOpKind.k_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_vvvv)], vexOpKind[nameof(VexOpKind.xmm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_vvvv)], vexOpKind[nameof(VexOpKind.ymm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], vexOpKind[nameof(VexOpKind.imm8)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm2_m2z)], vexOpKind[nameof(VexOpKind.imm2_m2z)], OpHandlerKind.OpI2, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_is4)], vexOpKind[nameof(VexOpKind.xmm_is4)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_is4)], vexOpKind[nameof(VexOpKind.ymm_is4)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_is5)], vexOpKind[nameof(VexOpKind.xmm_is5)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_is5)], vexOpKind[nameof(VexOpKind.ymm_is5)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], vexOpKind[nameof(VexOpKind.mem)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { false }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rDI)], vexOpKind[nameof(VexOpKind.seg_rDI)], OpHandlerKind.OprDI, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_rm)], vexOpKind[nameof(VexOpKind.k_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)], vexOpKind[nameof(VexOpKind.xmm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_rm)], vexOpKind[nameof(VexOpKind.ymm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_reg)], vexOpKind[nameof(VexOpKind.k_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)], vexOpKind[nameof(VexOpKind.mem_vsib32x)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)], vexOpKind[nameof(VexOpKind.mem_vsib32y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)], vexOpKind[nameof(VexOpKind.mem_vsib64x)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)], vexOpKind[nameof(VexOpKind.mem_vsib64y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], vexOpKind[nameof(VexOpKind.xmm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)], vexOpKind[nameof(VexOpKind.ymm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_or_mem)], vexOpKind[nameof(VexOpKind.k_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], vexOpKind[nameof(VexOpKind.xmm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)], vexOpKind[nameof(VexOpKind.ymm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.sibmem)], vexOpKind[nameof(VexOpKind.sibmem)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { true }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.tmm_reg)], vexOpKind[nameof(VexOpKind.tmm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.TMM0)], register[nameof(Register.TMM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.tmm_rm)], vexOpKind[nameof(VexOpKind.tmm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.TMM0)], register[nameof(Register.TMM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.tmm_vvvv)], vexOpKind[nameof(VexOpKind.tmm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.TMM0)], register[nameof(Register.TMM7)] }),
			};
			XopOpHandlers = new (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], xopOpKind[nameof(XopOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], xopOpKind[nameof(XopOpKind.r32_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], xopOpKind[nameof(XopOpKind.r64_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], xopOpKind[nameof(XopOpKind.r32_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], xopOpKind[nameof(XopOpKind.r64_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], xopOpKind[nameof(XopOpKind.r32_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], xopOpKind[nameof(XopOpKind.r64_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_vvvv)], xopOpKind[nameof(XopOpKind.r32_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_vvvv)], xopOpKind[nameof(XopOpKind.r64_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_vvvv)], xopOpKind[nameof(XopOpKind.xmm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_vvvv)], xopOpKind[nameof(XopOpKind.ymm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], xopOpKind[nameof(XopOpKind.imm8)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm32)], xopOpKind[nameof(XopOpKind.imm32)], OpHandlerKind.OpId, new object[] { opKind[nameof(OpKind.Immediate32)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_is4)], xopOpKind[nameof(XopOpKind.xmm_is4)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_is4)], xopOpKind[nameof(XopOpKind.ymm_is4)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], xopOpKind[nameof(XopOpKind.xmm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)], xopOpKind[nameof(XopOpKind.ymm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], xopOpKind[nameof(XopOpKind.xmm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)], xopOpKind[nameof(XopOpKind.ymm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
			};
			EvexOpHandlers = new (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], evexOpKind[nameof(EvexOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], evexOpKind[nameof(EvexOpKind.r32_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], evexOpKind[nameof(EvexOpKind.r64_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], evexOpKind[nameof(EvexOpKind.r32_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], evexOpKind[nameof(EvexOpKind.r64_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_vvvv)], evexOpKind[nameof(EvexOpKind.xmm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_vvvv)], evexOpKind[nameof(EvexOpKind.ymm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_vvvv)], evexOpKind[nameof(EvexOpKind.zmm_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmmp3_vvvv)], evexOpKind[nameof(EvexOpKind.xmmp3_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmmp3_vvvv)], evexOpKind[nameof(EvexOpKind.zmmp3_vvvv)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], evexOpKind[nameof(EvexOpKind.imm8)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], evexOpKind[nameof(EvexOpKind.mem)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { false }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], evexOpKind[nameof(EvexOpKind.r32_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], evexOpKind[nameof(EvexOpKind.r64_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)], evexOpKind[nameof(EvexOpKind.xmm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_rm)], evexOpKind[nameof(EvexOpKind.ymm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_rm)], evexOpKind[nameof(EvexOpKind.zmm_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_rm)], evexOpKind[nameof(EvexOpKind.k_rm)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)], evexOpKind[nameof(EvexOpKind.mem_vsib32x)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)], evexOpKind[nameof(EvexOpKind.mem_vsib32y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32z)], evexOpKind[nameof(EvexOpKind.mem_vsib32z)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)], evexOpKind[nameof(EvexOpKind.mem_vsib64x)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)], evexOpKind[nameof(EvexOpKind.mem_vsib64y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64z)], evexOpKind[nameof(EvexOpKind.mem_vsib64z)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_reg)], evexOpKind[nameof(EvexOpKind.k_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.kp1_reg)], evexOpKind[nameof(EvexOpKind.kp1_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], evexOpKind[nameof(EvexOpKind.xmm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)], evexOpKind[nameof(EvexOpKind.ymm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_reg)], evexOpKind[nameof(EvexOpKind.zmm_reg)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], evexOpKind[nameof(EvexOpKind.xmm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)], evexOpKind[nameof(EvexOpKind.ymm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_or_mem)], evexOpKind[nameof(EvexOpKind.zmm_or_mem)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
			};

			Sort(LegacyOpHandlers);
			Sort(VexOpHandlers);
			Sort(XopOpHandlers);
			Sort(EvexOpHandlers);
			if (new HashSet<EnumValue>(LegacyOpHandlers.Select(a => a.legacyOpKind)).Count != legacyOpKind.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(VexOpHandlers.Select(a => a.vexOpKind)).Count != vexOpKind.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(XopOpHandlers.Select(a => a.xopOpKind)).Count != xopOpKind.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(EvexOpHandlers.Select(a => a.evexOpKind)).Count != evexOpKind.Values.Length)
				throw new InvalidOperationException();

			toLegacy = LegacyOpHandlers.ToDictionary(a => (OpCodeOperandKind)a.opCodeOperandKind.Value, a => (LegacyOpKind)a.legacyOpKind.Value);
			toVex = VexOpHandlers.ToDictionary(a => (OpCodeOperandKind)a.opCodeOperandKind.Value, a => (VexOpKind)a.vexOpKind.Value);
			toXop = XopOpHandlers.ToDictionary(a => (OpCodeOperandKind)a.opCodeOperandKind.Value, a => (XopOpKind)a.xopOpKind.Value);
			toEvex = EvexOpHandlers.ToDictionary(a => (OpCodeOperandKind)a.opCodeOperandKind.Value, a => (EvexOpKind)a.evexOpKind.Value);

			genTypes.AddObject(TypeIds.EncoderTypes, this);

			static void Sort((EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] handlers) =>
				Array.Sort(handlers, (a, b) => a.opKind.Value.CompareTo(b.opKind.Value));
		}

		public LegacyOpKind ToLegacy(OpCodeOperandKind opKind) => toLegacy[opKind];
		public VexOpKind ToVex(OpCodeOperandKind opKind) => toVex[opKind];
		public XopOpKind ToXop(OpCodeOperandKind opKind) => toXop[opKind];
		public EvexOpKind ToEvex(OpCodeOperandKind opKind) => toEvex[opKind];
	}
}
