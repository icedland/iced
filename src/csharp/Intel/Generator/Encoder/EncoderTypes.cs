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
		public Dictionary<OpCodeFlags, EnumValue> AllowedPrefixesMap { get; }
		public (EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] LegacyOpHandlers { get; }
		public (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] VexOpHandlers { get; }
		public (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] XopOpHandlers { get; }
		public (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] EvexOpHandlers { get; }

		EncoderTypes(GenTypes genTypes) {
			var gen = new EncoderTypesGen(genTypes);
			gen.Generate();
			ImmSizes = gen.ImmSizes ?? throw new InvalidOperationException();
			AllowedPrefixesMap = gen.AllowedPrefixesMap ?? throw new InvalidOperationException();
			genTypes.Add(gen.EncFlags1 ?? throw new InvalidOperationException());
			genTypes.Add(gen.LegacyFlags3 ?? throw new InvalidOperationException());
			genTypes.Add(gen.VexFlags3 ?? throw new InvalidOperationException());
			genTypes.Add(gen.XopFlags3 ?? throw new InvalidOperationException());
			genTypes.Add(gen.EvexFlags3 ?? throw new InvalidOperationException());
			genTypes.Add(gen.AllowedPrefixes ?? throw new InvalidOperationException());
			genTypes.Add(gen.LegacyFlags ?? throw new InvalidOperationException());
			genTypes.Add(gen.VexFlags ?? throw new InvalidOperationException());
			genTypes.Add(gen.XopFlags ?? throw new InvalidOperationException());
			genTypes.Add(gen.EvexFlags ?? throw new InvalidOperationException());
			genTypes.Add(gen.D3nowFlags ?? throw new InvalidOperationException());

			var opCodeOperandKind = genTypes[TypeIds.OpCodeOperandKind];
			var legacyOpKind = genTypes[TypeIds.LegacyOpKind];
			var vexOpKind = genTypes[TypeIds.VexOpKind];
			var xopOpKind = genTypes[TypeIds.XopOpKind];
			var evexOpKind = genTypes[TypeIds.EvexOpKind];
			var register = genTypes[TypeIds.Register];
			var opKind = genTypes[TypeIds.OpKind];

			LegacyOpHandlers = new (EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], legacyOpKind[nameof(LegacyOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.farbr2_2)], legacyOpKind[nameof(LegacyOpKind.Aww)], OpHandlerKind.OpA, new object[] { 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.farbr4_2)], legacyOpKind[nameof(LegacyOpKind.Adw)], OpHandlerKind.OpA, new object[] { 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.M)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mfbcd)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mf32)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mf64)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mf80)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mfi16)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mfi32)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mfi64)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.M14)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.M28)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.M98)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.M108)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mp)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Ms)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mo)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mb)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mw)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Md)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_mpx)], legacyOpKind[nameof(LegacyOpKind.Md_MPX)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mq)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_mpx)], legacyOpKind[nameof(LegacyOpKind.Mq_MPX)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Mw2)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Md2)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r8_or_mem)], legacyOpKind[nameof(LegacyOpKind.Eb)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.AL)], register[nameof(Register.R15L)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_or_mem)], legacyOpKind[nameof(LegacyOpKind.Ew)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], legacyOpKind[nameof(LegacyOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem_mpx)], legacyOpKind[nameof(LegacyOpKind.Ed_MPX)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], legacyOpKind[nameof(LegacyOpKind.Ew_d)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], legacyOpKind[nameof(LegacyOpKind.Ew_q)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], legacyOpKind[nameof(LegacyOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem_mpx)], legacyOpKind[nameof(LegacyOpKind.Eq_MPX)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Eww)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Edw)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], legacyOpKind[nameof(LegacyOpKind.Eqw)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], legacyOpKind[nameof(LegacyOpKind.RdMb)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], legacyOpKind[nameof(LegacyOpKind.RqMb)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], legacyOpKind[nameof(LegacyOpKind.RdMw)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], legacyOpKind[nameof(LegacyOpKind.RqMw)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r8_reg)], legacyOpKind[nameof(LegacyOpKind.Gb)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.AL)], register[nameof(Register.R15L)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_reg)], legacyOpKind[nameof(LegacyOpKind.Gw)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], legacyOpKind[nameof(LegacyOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], legacyOpKind[nameof(LegacyOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_reg_mem)], legacyOpKind[nameof(LegacyOpKind.Gw_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg_mem)], legacyOpKind[nameof(LegacyOpKind.Gd_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg_mem)], legacyOpKind[nameof(LegacyOpKind.Gq_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_rm)], legacyOpKind[nameof(LegacyOpKind.Rw)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], legacyOpKind[nameof(LegacyOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], legacyOpKind[nameof(LegacyOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_reg)], legacyOpKind[nameof(LegacyOpKind.Sw)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.ES)], register[nameof(Register.GS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.cr_reg)], legacyOpKind[nameof(LegacyOpKind.Cd)], OpHandlerKind.OpModRM_regF0, new object[] { register[nameof(Register.CR0)], register[nameof(Register.CR15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.cr_reg)], legacyOpKind[nameof(LegacyOpKind.Cq)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.CR0)], register[nameof(Register.CR15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.dr_reg)], legacyOpKind[nameof(LegacyOpKind.Dd)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.DR0)], register[nameof(Register.DR15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.dr_reg)], legacyOpKind[nameof(LegacyOpKind.Dq)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.DR0)], register[nameof(Register.DR15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.tr_reg)], legacyOpKind[nameof(LegacyOpKind.Td)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.TR0)], register[nameof(Register.TR7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], legacyOpKind[nameof(LegacyOpKind.Ib)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8sex16)], legacyOpKind[nameof(LegacyOpKind.Ib16)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8to16)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8sex32)], legacyOpKind[nameof(LegacyOpKind.Ib32)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8to32)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8sex64)], legacyOpKind[nameof(LegacyOpKind.Ib64)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8to64)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm16)], legacyOpKind[nameof(LegacyOpKind.Iw)], OpHandlerKind.OpIw, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm32)], legacyOpKind[nameof(LegacyOpKind.Id)], OpHandlerKind.OpId, new object[] { opKind[nameof(OpKind.Immediate32)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm32sex64)], legacyOpKind[nameof(LegacyOpKind.Id64)], OpHandlerKind.OpId, new object[] { opKind[nameof(OpKind.Immediate32to64)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm64)], legacyOpKind[nameof(LegacyOpKind.Iq)], OpHandlerKind.OpIq, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], legacyOpKind[nameof(LegacyOpKind.Ib21)], OpHandlerKind.OpIb21, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], legacyOpKind[nameof(LegacyOpKind.Ib11)], OpHandlerKind.OpIb11, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rSI)], legacyOpKind[nameof(LegacyOpKind.Xb)], OpHandlerKind.OpX, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rSI)], legacyOpKind[nameof(LegacyOpKind.Xw)], OpHandlerKind.OpX, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rSI)], legacyOpKind[nameof(LegacyOpKind.Xd)], OpHandlerKind.OpX, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rSI)], legacyOpKind[nameof(LegacyOpKind.Xq)], OpHandlerKind.OpX, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.es_rDI)], legacyOpKind[nameof(LegacyOpKind.Yb)], OpHandlerKind.OpY, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.es_rDI)], legacyOpKind[nameof(LegacyOpKind.Yw)], OpHandlerKind.OpY, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.es_rDI)], legacyOpKind[nameof(LegacyOpKind.Yd)], OpHandlerKind.OpY, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.es_rDI)], legacyOpKind[nameof(LegacyOpKind.Yq)], OpHandlerKind.OpY, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br16_1)], legacyOpKind[nameof(LegacyOpKind.wJb)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch16)], 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br32_1)], legacyOpKind[nameof(LegacyOpKind.dJb)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch32)], 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br64_1)], legacyOpKind[nameof(LegacyOpKind.qJb)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch64)], 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br16_2)], legacyOpKind[nameof(LegacyOpKind.Jw)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch16)], 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br32_4)], legacyOpKind[nameof(LegacyOpKind.wJd)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch32)], 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br32_4)], legacyOpKind[nameof(LegacyOpKind.dJd)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch32)], 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.br64_4)], legacyOpKind[nameof(LegacyOpKind.qJd)], OpHandlerKind.OpJ, new object[] { opKind[nameof(OpKind.NearBranch64)], 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xbegin_2)], legacyOpKind[nameof(LegacyOpKind.Jxw)], OpHandlerKind.OpJx, new object[] { 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xbegin_4)], legacyOpKind[nameof(LegacyOpKind.Jxd)], OpHandlerKind.OpJx, new object[] { 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.brdisp_2)], legacyOpKind[nameof(LegacyOpKind.Jdisp16)], OpHandlerKind.OpJdisp, new object[] { 2 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.brdisp_4)], legacyOpKind[nameof(LegacyOpKind.Jdisp32)], OpHandlerKind.OpJdisp, new object[] { 4 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_offs)], legacyOpKind[nameof(LegacyOpKind.Ob)], OpHandlerKind.OpO, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_offs)], legacyOpKind[nameof(LegacyOpKind.Ow)], OpHandlerKind.OpO, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_offs)], legacyOpKind[nameof(LegacyOpKind.Od)], OpHandlerKind.OpO, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_offs)], legacyOpKind[nameof(LegacyOpKind.Oq)], OpHandlerKind.OpO, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8_const_1)], legacyOpKind[nameof(LegacyOpKind.Imm1)], OpHandlerKind.OpImm, new object[] { 1 }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.bnd_reg)], legacyOpKind[nameof(LegacyOpKind.B)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.BND0)], register[nameof(Register.BND3)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.bnd_or_mem_mpx)], legacyOpKind[nameof(LegacyOpKind.BMq)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.BND0)], register[nameof(Register.BND3)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.bnd_or_mem_mpx)], legacyOpKind[nameof(LegacyOpKind.BMo)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.BND0)], register[nameof(Register.BND3)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_mib)], legacyOpKind[nameof(LegacyOpKind.MIB)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mm_rm)], legacyOpKind[nameof(LegacyOpKind.N)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.MM0)], register[nameof(Register.MM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mm_reg)], legacyOpKind[nameof(LegacyOpKind.P)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.MM0)], register[nameof(Register.MM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mm_or_mem)], legacyOpKind[nameof(LegacyOpKind.Q)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.MM0)], register[nameof(Register.MM7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)], legacyOpKind[nameof(LegacyOpKind.RX)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], legacyOpKind[nameof(LegacyOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], legacyOpKind[nameof(LegacyOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rDI)], legacyOpKind[nameof(LegacyOpKind.rDI)], OpHandlerKind.OprDI, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rBX_al)], legacyOpKind[nameof(LegacyOpKind.MRBX)], OpHandlerKind.OpMRBX, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.es)], legacyOpKind[nameof(LegacyOpKind.ES)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.ES)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.cs)], legacyOpKind[nameof(LegacyOpKind.CS)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.CS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ss)], legacyOpKind[nameof(LegacyOpKind.SS)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.SS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ds)], legacyOpKind[nameof(LegacyOpKind.DS)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.DS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.fs)], legacyOpKind[nameof(LegacyOpKind.FS)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.FS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.gs)], legacyOpKind[nameof(LegacyOpKind.GS)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.GS)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.al)], legacyOpKind[nameof(LegacyOpKind.AL)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.AL)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.cl)], legacyOpKind[nameof(LegacyOpKind.CL)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.CL)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ax)], legacyOpKind[nameof(LegacyOpKind.AX)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.AX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.dx)], legacyOpKind[nameof(LegacyOpKind.DX)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.DX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.eax)], legacyOpKind[nameof(LegacyOpKind.EAX)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.EAX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.rax)], legacyOpKind[nameof(LegacyOpKind.RAX)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.RAX)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.st0)], legacyOpKind[nameof(LegacyOpKind.ST)], OpHandlerKind.OpReg, new object[] { register[nameof(Register.ST0)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.sti_opcode)], legacyOpKind[nameof(LegacyOpKind.STi)], OpHandlerKind.OpRegSTi, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r8_opcode)], legacyOpKind[nameof(LegacyOpKind.r8_rb)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.AL)], register[nameof(Register.R15L)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r16_opcode)], legacyOpKind[nameof(LegacyOpKind.r16_rw)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.AX)], register[nameof(Register.R15W)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_opcode)], legacyOpKind[nameof(LegacyOpKind.r32_rd)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_opcode)], legacyOpKind[nameof(LegacyOpKind.r64_ro)], OpHandlerKind.OpRegEmbed8, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
			};
			VexOpHandlers = new (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], vexOpKind[nameof(VexOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], vexOpKind[nameof(VexOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], vexOpKind[nameof(VexOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], vexOpKind[nameof(VexOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], vexOpKind[nameof(VexOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], vexOpKind[nameof(VexOpKind.RdMb)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], vexOpKind[nameof(VexOpKind.RqMb)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], vexOpKind[nameof(VexOpKind.RdMw)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], vexOpKind[nameof(VexOpKind.RqMw)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], vexOpKind[nameof(VexOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], vexOpKind[nameof(VexOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_vvvv)], vexOpKind[nameof(VexOpKind.Hd)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_vvvv)], vexOpKind[nameof(VexOpKind.Hq)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_vvvv)], vexOpKind[nameof(VexOpKind.HK)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_vvvv)], vexOpKind[nameof(VexOpKind.HX)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_vvvv)], vexOpKind[nameof(VexOpKind.HY)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], vexOpKind[nameof(VexOpKind.Ib)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm2_m2z)], vexOpKind[nameof(VexOpKind.I2)], OpHandlerKind.OpI2, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_is4)], vexOpKind[nameof(VexOpKind.Is4X)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_is4)], vexOpKind[nameof(VexOpKind.Is4Y)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_is5)], vexOpKind[nameof(VexOpKind.Is5X)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_is5)], vexOpKind[nameof(VexOpKind.Is5Y)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], vexOpKind[nameof(VexOpKind.M)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], vexOpKind[nameof(VexOpKind.Md)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], vexOpKind[nameof(VexOpKind.MK)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.seg_rDI)], vexOpKind[nameof(VexOpKind.rDI)], OpHandlerKind.OprDI, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_rm)], vexOpKind[nameof(VexOpKind.RK)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)], vexOpKind[nameof(VexOpKind.RX)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_rm)], vexOpKind[nameof(VexOpKind.RY)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_reg)], vexOpKind[nameof(VexOpKind.VK)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)], vexOpKind[nameof(VexOpKind.VM32X)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)], vexOpKind[nameof(VexOpKind.VM32Y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)], vexOpKind[nameof(VexOpKind.VM64X)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)], vexOpKind[nameof(VexOpKind.VM64Y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], vexOpKind[nameof(VexOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)], vexOpKind[nameof(VexOpKind.VY)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_or_mem)], vexOpKind[nameof(VexOpKind.WK)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], vexOpKind[nameof(VexOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)], vexOpKind[nameof(VexOpKind.WY)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
			};
			XopOpHandlers = new (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], xopOpKind[nameof(XopOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], xopOpKind[nameof(XopOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], xopOpKind[nameof(XopOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], xopOpKind[nameof(XopOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], xopOpKind[nameof(XopOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], xopOpKind[nameof(XopOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], xopOpKind[nameof(XopOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_vvvv)], xopOpKind[nameof(XopOpKind.Hd)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_vvvv)], xopOpKind[nameof(XopOpKind.Hq)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_vvvv)], xopOpKind[nameof(XopOpKind.HX)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_vvvv)], xopOpKind[nameof(XopOpKind.HY)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], xopOpKind[nameof(XopOpKind.Ib)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm32)], xopOpKind[nameof(XopOpKind.Id)], OpHandlerKind.OpId, new object[] { opKind[nameof(OpKind.Immediate32)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_is4)], xopOpKind[nameof(XopOpKind.Is4X)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_is4)], xopOpKind[nameof(XopOpKind.Is4Y)], OpHandlerKind.OpIs4x, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], xopOpKind[nameof(XopOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)], xopOpKind[nameof(XopOpKind.VY)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], xopOpKind[nameof(XopOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)], xopOpKind[nameof(XopOpKind.WY)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM15)] }),
			};
			EvexOpHandlers = new (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
				(opCodeOperandKind[nameof(OpCodeOperandKind.None)], evexOpKind[nameof(EvexOpKind.None)], OpHandlerKind.None, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], evexOpKind[nameof(EvexOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], evexOpKind[nameof(EvexOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_reg)], evexOpKind[nameof(EvexOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_reg)], evexOpKind[nameof(EvexOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], evexOpKind[nameof(EvexOpKind.RdMb)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], evexOpKind[nameof(EvexOpKind.RqMb)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_or_mem)], evexOpKind[nameof(EvexOpKind.RdMw)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_or_mem)], evexOpKind[nameof(EvexOpKind.RqMw)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_vvvv)], evexOpKind[nameof(EvexOpKind.HX)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_vvvv)], evexOpKind[nameof(EvexOpKind.HY)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_vvvv)], evexOpKind[nameof(EvexOpKind.HZ)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmmp3_vvvv)], evexOpKind[nameof(EvexOpKind.HXP3)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmmp3_vvvv)], evexOpKind[nameof(EvexOpKind.HZP3)], OpHandlerKind.OpHx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.imm8)], evexOpKind[nameof(EvexOpKind.Ib)], OpHandlerKind.OpIb, new object[] { opKind[nameof(OpKind.Immediate8)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem)], evexOpKind[nameof(EvexOpKind.M)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r32_rm)], evexOpKind[nameof(EvexOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.EAX)], register[nameof(Register.R15D)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.r64_rm)], evexOpKind[nameof(EvexOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.RAX)], register[nameof(Register.R15)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_rm)], evexOpKind[nameof(EvexOpKind.RX)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_rm)], evexOpKind[nameof(EvexOpKind.RY)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_rm)], evexOpKind[nameof(EvexOpKind.RZ)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_rm)], evexOpKind[nameof(EvexOpKind.RK)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32x)], evexOpKind[nameof(EvexOpKind.VM32X)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32y)], evexOpKind[nameof(EvexOpKind.VM32Y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib32z)], evexOpKind[nameof(EvexOpKind.VM32Z)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64x)], evexOpKind[nameof(EvexOpKind.VM64X)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64y)], evexOpKind[nameof(EvexOpKind.VM64Y)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.mem_vsib64z)], evexOpKind[nameof(EvexOpKind.VM64Z)], OpHandlerKind.OpVMx, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.k_reg)], evexOpKind[nameof(EvexOpKind.VK)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.kp1_reg)], evexOpKind[nameof(EvexOpKind.VKP1)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.K0)], register[nameof(Register.K7)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_reg)], evexOpKind[nameof(EvexOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_reg)], evexOpKind[nameof(EvexOpKind.VY)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_reg)], evexOpKind[nameof(EvexOpKind.VZ)], OpHandlerKind.OpModRM_reg, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.xmm_or_mem)], evexOpKind[nameof(EvexOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.XMM0)], register[nameof(Register.XMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.ymm_or_mem)], evexOpKind[nameof(EvexOpKind.WY)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.YMM0)], register[nameof(Register.YMM31)] }),
				(opCodeOperandKind[nameof(OpCodeOperandKind.zmm_or_mem)], evexOpKind[nameof(EvexOpKind.WZ)], OpHandlerKind.OpModRM_rm, new object[] { register[nameof(Register.ZMM0)], register[nameof(Register.ZMM31)] }),
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

			genTypes.AddObject(TypeIds.EncoderTypes, this);

			static void Sort((EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] handlers) =>
				Array.Sort(handlers, (a, b) => a.opKind.Value.CompareTo(b.opKind.Value));
		}
	}
}
